using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Web.WebView2.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WinsockPacketEditor;

namespace WPEHybrid
{
    #region//JSON-RPC 桥

    /// <summary>
    /// JSON-RPC over postMessage（B10b）。本类只做「消息路由 + 序列化 + 线程切换」，不含业务逻辑。
    ///
    /// 【双向请求】
    /// 与 WPEProxyCap.Hybrid 的桥不同，这里两个方向都能发起请求：
    ///   JS  → C#  前端调 Operate 的方法（取封包字节、启停代理…）
    ///   C# → JS   IUiHost 的弹窗 / 文件框 / 表单，Operate 在 await 前端的回答
    /// 后者是 WPE 特有的：B0–B8 把 96 处弹窗收口到 IUiHost，它们全是要等结果的。
    ///
    /// 【超时】
    /// C# 发出的提问一律带超时（默认 5 分钟）。前端崩溃或刷新时若不兜底，
    /// 一条 await UI.Confirm 会把整条 _Dialog 调用链永久挂起。
    /// 页面导航时也会把所有在途提问统一失败掉。
    /// </summary>
    public sealed class WebBridge
    {
        #region//字段

        private readonly CoreWebView2 core;
        private readonly SynchronizationContext ui;
        private readonly HashSet<string> allowedHosts;

        /// <summary>C# 发出、等前端回答的在途提问。</summary>
        private readonly ConcurrentDictionary<string, TaskCompletionSource<JToken>> pending =
            new ConcurrentDictionary<string, TaskCompletionSource<JToken>>();

        private long seq;

        /// <summary>JS 可调用的方法表。新增一个方法只需在 ShellForm 里 Register 一行。</summary>
        private readonly Dictionary<string, Func<JObject, Task<object>>> methods =
            new Dictionary<string, Func<JObject, Task<object>>>(StringComparer.OrdinalIgnoreCase);

        private static readonly JsonSerializerSettings JsonOpts = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            //中文直出，不转成 \uXXXX
            StringEscapeHandling = StringEscapeHandling.Default,
        };

        #endregion

        #region//构造

        public WebBridge(CoreWebView2 core, HashSet<string> allowedHosts)
        {
            this.core = core;
            this.allowedHosts = allowedHosts;
            this.ui = SynchronizationContext.Current ?? new SynchronizationContext();

            this.core.WebMessageReceived += this.OnWebMessage;

            //页面刷新/跳转：把在途提问全部失败掉，避免 Operate 那边永久挂起
            this.core.NavigationStarting += (s, e) => this.FailAllPending("页面已导航");

            //渲染进程崩溃：页面已经没了，等答案的调用链同样必须立刻失败，
            //否则每一条 await UI.Confirm 都要挂到 AskAsync 的 5 分钟超时
            this.core.ProcessFailed += (s, e) => this.FailAllPending("渲染进程已崩溃");
        }

        #endregion

        #region//方法注册

        /// <summary>登记一个 JS 可调用的方法。</summary>
        public void Register(string Method, Func<JObject, Task<object>> Handler)
        {
            this.methods[Method] = Handler;
        }

        /// <summary>登记一个同步的 JS 可调用方法。</summary>
        public void Register(string Method, Func<JObject, object> Handler)
        {
            this.methods[Method] = args => Task.FromResult(Handler(args));
        }

        #endregion

        #region//JS → C#

        private async void OnWebMessage(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string raw = null;

            try
            {
                //安全：只接受白名单来源的消息。加载进来的页面能驱动本机的强力方法，这道校验不能省。
                if (!this.IsAllowedSource(e.Source))
                {
                    Operate.DoLog(nameof(OnWebMessage), "已忽略非白名单来源的消息: " + e.Source);
                    return;
                }

                raw = e.WebMessageAsJson;
                JObject msg = JObject.Parse(raw);
                string type = (string)msg["type"];

                //前端对 C# 提问的回答
                if (type == "answer")
                {
                    this.OnAnswer(msg.ToObject<RpcAnswer>());
                    return;
                }

                if (type != "call")
                {
                    Operate.DoLog(nameof(OnWebMessage), "未知消息类型: " + type);
                    return;
                }

                RpcCall call = msg.ToObject<RpcCall>();
                Func<JObject, Task<object>> handler;

                if (!this.methods.TryGetValue(call.Method ?? string.Empty, out handler))
                {
                    this.Reply(call.Id, false, null, "未知方法: " + call.Method);
                    return;
                }

                object result = await handler(call.Args ?? new JObject());
                this.Reply(call.Id, true, result, null);
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(OnWebMessage), ex);

                try
                {
                    string id = raw == null ? null : (string)JObject.Parse(raw)["id"];
                    if (id != null) { this.Reply(id, false, null, ex.Message); }
                }
                catch
                {
                    //连 id 都取不到就没法应答了，日志已记
                }
            }
        }

        private bool IsAllowedSource(string Source)
        {
            Uri u;
            return Uri.TryCreate(Source, UriKind.Absolute, out u) && this.allowedHosts.Contains(u.Host);
        }

        private void Reply(string Id, bool Ok, object Result, string Error)
        {
            this.PostRaw(JsonConvert.SerializeObject(
                new RpcResult { Id = Id, Ok = Ok, Result = Result, Error = Error }, JsonOpts));
        }

        #endregion

        #region//C# → JS（提问，等回答）

        /// <summary>
        /// 向前端提问并等回答。超时或前端报错时返回 default(T)，不抛异常
        /// —— 调用方是 IUiHost，它的约定就是「失败一律返回安全默认值」。
        /// </summary>
        public async Task<T> AskAsync<T>(string Method, object Args, int TimeoutMs = 300000)
        {
            string id = "a" + Interlocked.Increment(ref this.seq).ToString();
            var tcs = new TaskCompletionSource<JToken>(TaskCreationOptions.RunContinuationsAsynchronously);

            if (!this.pending.TryAdd(id, tcs))
            {
                return default(T);
            }

            try
            {
                this.PostRaw(JsonConvert.SerializeObject(
                    new RpcAsk { Id = id, Method = Method, Args = Args }, JsonOpts));

                Task done = await Task.WhenAny(tcs.Task, Task.Delay(TimeoutMs));

                if (done != tcs.Task)
                {
                    Operate.DoLog(nameof(AskAsync), "前端未在超时内回答: " + Method);
                    return default(T);
                }

                JToken token = await tcs.Task;
                return token == null ? default(T) : token.ToObject<T>();
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(AskAsync), ex);
                return default(T);
            }
            finally
            {
                TaskCompletionSource<JToken> ignored;
                this.pending.TryRemove(id, out ignored);
            }
        }

        private void OnAnswer(RpcAnswer Answer)
        {
            if (Answer == null || string.IsNullOrEmpty(Answer.Id))
            {
                return;
            }

            TaskCompletionSource<JToken> tcs;

            if (!this.pending.TryGetValue(Answer.Id, out tcs))
            {
                //超时之后才回来的答案，丢弃即可
                return;
            }

            if (Answer.Ok)
            {
                tcs.TrySetResult(Answer.Result);
            }
            else
            {
                Operate.DoLog(nameof(OnAnswer), "前端回报失败: " + Answer.Error);
                tcs.TrySetResult(null);
            }
        }

        private void FailAllPending(string Reason)
        {
            //逐个 TryRemove 再应答，不整表 Clear：遍历与 Clear 之间新登记的提问会被一起清掉却没人应答，
            //它的调用方要白等满 5 分钟超时
            foreach (string key in this.pending.Keys)
            {
                TaskCompletionSource<JToken> tcs;
                if (this.pending.TryRemove(key, out tcs))
                {
                    tcs.TrySetResult(null);
                }
            }
            Operate.DoLog(nameof(FailAllPending), Reason);
        }

        #endregion

        #region//C# → JS（单向推送）

        /// <summary>推一条事件给前端。可能来自后台线程。</summary>
        public void PushEvent(string Name, object Data)
        {
            this.PostRaw(JsonConvert.SerializeObject(
                new RpcEvent { Name = Name, Data = Data }, JsonOpts));
        }

        private void PostRaw(string Json)
        {
            //WebView2 的 API 必须在 UI 线程调用
            this.ui.Post(_ =>
            {
                try
                {
                    this.core.PostWebMessageAsJson(Json);
                }
                catch (Exception ex)
                {
                    //窗口已关闭 / 页面正在导航
                    Operate.DoLog(nameof(PostRaw), ex);
                }
            }, null);
        }

        #endregion
    }

    #endregion
}
