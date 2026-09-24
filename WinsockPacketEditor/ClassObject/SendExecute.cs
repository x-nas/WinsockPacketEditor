using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace WinsockPacketEditor
{
    public class SendExecute
    {
        public bool SystemSocket = false;
        public int LoopCNT = 0;
        public int LoopINT = 0;
        public int SendCollection_Index = 0;
        public int Send_Success = 0;
        public int Send_Failure = 0;
        public int Total_Send = 0;
        public string SendName = string.Empty;

        private SendInfo siSelect;
        private BindingList<PacketInfo> SendCollection;

        /*
            ⚠️ 2026-09-11：从 BackgroundWorker 换成 Task（BW → Task 第二步）。

            BW 提供的五件事各自的去处：
              IsBusy            → Running（task 建好到 continuation 跑完这一段为 true，与 IsBusy 的窗口一致）
              CancelAsync       → cts.Cancel()（第一步已经是它）
              DoWork 的异常     → 不 try/catch，异常进 task.Exception，由 continuation 记一处（BW 时代是 e.Error）
              ReportProgress    → 直接写 SendCollection_Index（这个值只有轮询在读，不必 marshal）
              RunWorkerCompleted→ Send_OnDone 这个 continuation（TaskScheduler.Default，一律在线程池上）

            为什么不再需要 InitListExecute 那样的外部挂钩：Task 自带 body，不用谁来挂 DoWork。
            注入模式的目标进程里这一点尤其要紧 —— 那边从来没调过 InitListExecute，
            所以 BW 时代「注入模式下开始发送整份列表什么都不做」，换 Task 之后自然就好了。
        */
        private CancellationTokenSource cts;
        private Task task;

        /// <summary>正在跑没有。task 建好到 Send_OnDone 跑完这一整段都算在跑（与 BW 的 IsBusy 同窗口）。</summary>
        public bool Running
        {
            get { Task t = this.task; return t != null && !t.IsCompleted; }
        }

        #region//启动发送

        public void StartSend(SendInfo si)
        {
            try
            {
                if (si != null && si.SCollection.Count > 0)
                {
                    if (!this.Running)
                    {
                        //⚠️ 每轮一个新的：CTS 取消过就不能复位
                        if (this.cts != null) { this.cts.Dispose(); }
                        this.cts = new CancellationTokenSource();

                        this.Total_Send = 0;
                        this.Send_Success = 0;
                        this.Send_Failure = 0;

                        //上一轮跑到第几条的痕迹要一起清 —— 不清的话新一轮开头那一瞬间，
                        //界面上高亮的是上一次停在的那一行（LoopINT 为 0 时它更是从头到尾都不会被覆盖）。
                        this.SendCollection_Index = 0;

                        this.siSelect = si;
                        this.SendName = si.SName;
                        this.SystemSocket = si.SSystemSocket;
                        this.LoopCNT = si.SLoopCNT;
                        this.LoopINT = si.SLoopINT;
                        this.SendCollection = si.SCollection;

                        CancellationToken token = this.cts.Token;
                        this.task = Task.Run(() => Send_Body(token), token)
                            .ContinueWith(Send_OnDone, TaskScheduler.Default);

                        string sLog = string.Format(UI.T("SendExecute.DoSend", "执行发送 [{0}]"), this.SendName);
                        Operate.DoLog(nameof(StartSend), sLog);
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(StartSend), ex);
            }
        }

        #endregion

        #region//停止发送

        public void StopSend()
        {
            try
            {
                //⚠️ 只 Cancel token（第一步的口径没变）：取消只能有一个真源
                CancellationTokenSource c = this.cts;
                if (c != null) { c.Cancel(); }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(StopSend), ex);
            }
        }

        #endregion

        #region//执行发送集

        /*
            ⚠️ <b>这里不加 try/catch。</b> 加了的话异常被吞掉、task.Exception 恒为 null，
            Send_OnDone 里「发生错误」那一支就成了死代码 ——
            发送炸在半路，日志里写的却是「执行完毕」（与机器人那处同一个病根）。

            取消走 <c>token.ThrowIfCancellationRequested()</c>：抛 OperationCanceledException，
            task.IsCanceled 变 true，Send_OnDone 据此报「已停止」（取代 BW 的 e.Cancel）。
        */
        private void Send_Body(CancellationToken token)
        {
            if (this.SystemSocket)
            {
                if (Operate.SystemConfig.SystemSocket <= 0)
                {
                    Operate.DoLog(nameof(Send_Body), UI.T("System.SystemSocket.Error", "系统套接字未设置"));
                    return;
                }
            }

            for (int i = 0; i < this.LoopCNT; i++)
            {
                this.siSelect.ExecutionCount++;

                for (int j = 0; j < this.SendCollection.Count; j++)
                {
                    PacketInfo pi = this.SendCollection[j];
                    if (pi != null)
                    {
                        token.ThrowIfCancellationRequested();

                        int Socket = pi.PacketSocket;
                        if (this.SystemSocket)
                        {
                            Socket = Operate.SystemConfig.SystemSocket;
                        }

                        /*
                            ⚠️⚠️ 套接字 <= 0 原来是<b>整条静默跳过</b>：不发、不计数、不记日志。

                            以前 SunnyNet 的中间人那条路产出的封包套接字一律是 0（靠会话号回发）；
                            中间人移除后不会再产出这种包，但从升级前存下来的发送集里读出来的仍可能是 0。
                            而「代理数据页右键 → 添加到发送」正是把这种封包放进发送集最自然的一条路。

                            于是不勾「使用系统套接字」时：<b>一个包都发不出去，三个计数全是 0，
                            日志还写着「执行完毕」</b> —— 又一次「点了没反应」。

                            现在<b>计入失败</b>并记一条节流日志。判据很简单：用户要求发这一条、
                            结果什么都没发出去，那就是失败，不是「没这回事」。
                        */
                        if (Socket <= 0)
                        {
                            this.Send_Failure++;
                            this.siSelect.ExecutionFail++;
                            this.Total_Send++;

                            //⚠️ 必须节流：一条几百个包的发送集全是 0 的话，逐条记会把日志刷爆
                            Operate.SystemConfig.LogThrottled("SendNoSocket." + this.SendName, string.Format(
                                UI.T("SendExecute.Socket.Missing",
                                     "发送「{0}」里有封包没有套接字（套接字号为 0）—— 这些封包发不出去，请勾上「使用系统套接字」并先在封包列表里右键设置它。"),
                                this.SendName));
                        }
                        else
                        {
                            /*
                                第三个参数<b>必须传 PacketFrom，不能是 string.Empty</b>。

                                SendPacket 是按封包类型挑地址的：
                                  请求类（*Send / *SendTo / TCP_Req / UDP_Req）取 sIPTo；
                                  响应类（*Recv / *RecvFrom / TCP_Resp / UDP_Resp）取 sIPFrom。
                                而 UDP 那几条走 sendto()，进去之前还有一道
                                `if (!string.IsNullOrEmpty(sIPString))`。

                                所以原来传空串的后果是：<b>UDP 响应类封包永远发不出去</b> ——
                                条件不成立、res 保持 -1、静默计入「失败」，一点线索都没有。
                                TCP 不受影响（send() 压根不用地址），UDP 请求也不受影响（用的是 sIPTo）。

                                PacketFrom 就是抓包时的本机地址（ProxyInfo 那边叫 ClientAddr，
                                AddSendCollection_ByProxyInfo 里映射过来的）。重放一条 UDP 响应，
                                本来就是「冒充服务端往当初那个客户端地址回包」。
                            */
                            bool bOK = Operate.PacketConfig.Packet.SendPacket(Socket, pi.PacketType, pi.PacketFrom, pi.PacketTo, pi.PacketBuffer);

                            if (bOK)
                            {
                                this.Send_Success++;
                                this.siSelect.ExecutionSuccess++;
                            }
                            else
                            {
                                this.Send_Failure++;
                                this.siSelect.ExecutionFail++;
                            }

                            this.Total_Send++;

                            if (this.LoopINT > 0)
                            {
                                //进度只有轮询在读，直接写就行，不必 marshal（BW 时代走 ReportProgress）
                                this.SendCollection_Index = j;
                                Operate.SystemConfig.DoSleep(this.LoopINT, token);

                                //⚠️ DoSleep 被取消时提前返回、不抛 —— 若这是最后一发，两层循环随即
                                //自然结束、报「执行完毕」，而用户按了停止。补一句让它如实报「已停止」。
                                token.ThrowIfCancellationRequested();
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region//执行完毕

        private void Send_OnDone(Task t)
        {
            try
            {
                string sMsg;

                if (t.IsCanceled)
                {
                    sMsg = string.Format(UI.T("SendExecute.Stop", "发送 [{0}] 已停止"), this.SendName);
                }
                else if (t.Exception != null)
                {
                    Exception ex = t.Exception.InnerException ?? t.Exception;
                    sMsg = string.Format(UI.T("SendExecute.Error", "发送[{0}] 发生错误: {1}"), this.SendName, ex.Message);
                }
                else
                {
                    sMsg = string.Format(UI.T("SendExecute.Success", "发送[{0}] 执行完毕"), this.SendName);
                }

                Operate.DoLog(nameof(Send_OnDone), sMsg);
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(Send_OnDone), ex);
            }
        }

        #endregion
    }
}
