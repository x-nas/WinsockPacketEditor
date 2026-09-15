using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WPEHybrid
{
    #region//桥的消息结构

    /*
        桥上跑四种消息，用 type 字段区分。两个方向都能发起请求，这一点和
        WPEProxyCap 不同 —— 那边只有「JS 发起、C# 应答」加上「C# 单向推事件」。

        WPE 这边必须支持反方向：IUiHost.ConfirmAsync / PickSaveAsync / PromptAsync 都是
        C# 问、前端答，Operate 在 await 它们的结果。

            JS  → C#   { type:"call",   id, method, args }      调用 C# 方法
            C# → JS    { type:"result", id, ok, result, error } 上一条的应答
            C# → JS    { type:"ask",    id, method, args }      C# 向前端提问（弹窗等）
            JS  → C#   { type:"answer", id, ok, result, error } 上一条的应答
            C# → JS    { type:"event",  name, data }            单向推送（封包批次等）
    */

    /// <summary>JS → C# 的调用请求。</summary>
    public sealed class RpcCall
    {
        [JsonProperty("type")] public string Type { get; set; }
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("method")] public string Method { get; set; }
        [JsonProperty("args")] public JObject Args { get; set; }
    }

    /// <summary>C# → JS 的调用应答。</summary>
    public sealed class RpcResult
    {
        [JsonProperty("type")] public string Type { get { return "result"; } }
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("ok")] public bool Ok { get; set; }
        [JsonProperty("result")] public object Result { get; set; }
        [JsonProperty("error")] public string Error { get; set; }
    }

    /// <summary>C# → JS 的提问（弹窗 / 文件框 / 表单）。</summary>
    public sealed class RpcAsk
    {
        [JsonProperty("type")] public string Type { get { return "ask"; } }
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("method")] public string Method { get; set; }
        [JsonProperty("args")] public object Args { get; set; }
    }

    /// <summary>JS → C# 的提问应答。</summary>
    public sealed class RpcAnswer
    {
        [JsonProperty("type")] public string Type { get; set; }
        [JsonProperty("id")] public string Id { get; set; }
        [JsonProperty("ok")] public bool Ok { get; set; }
        [JsonProperty("result")] public JToken Result { get; set; }
        [JsonProperty("error")] public string Error { get; set; }
    }

    /// <summary>C# → JS 的单向推送。</summary>
    public sealed class RpcEvent
    {
        [JsonProperty("type")] public string Type { get { return "event"; } }
        [JsonProperty("name")] public string Name { get; set; }
        [JsonProperty("data")] public object Data { get; set; }
    }

    #endregion
}
