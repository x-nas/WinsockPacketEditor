using System;
using System.ComponentModel;
using System.Threading;

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
        public BackgroundWorker Worker = new BackgroundWorker();

        /*
            ⚠️⚠️ <b>取消的唯一真源</b>（2026-09-10）。

            此前查的是 Worker.CancellationPending —— 它是个<b>裸 bool</b>，
            没有 WaitHandle，于是所有等待只能轮询（DoSleep 切片、排空循环 Thread.Sleep(100)）。
            换成 token 之后「等某件事做完」那几处才能改成阻塞等待。

            ⚠️ <b>StopSend 里刻意不再调 Worker.CancelAsync()</b> —— 两个都翻就是两个真源。
            CancellationPending 现在全项目没有任何人读（grep 过）。
            e.Cancelled 不受影响：它来自 DoWork 里的 e.Cancel，与 CancelAsync 无关。
        */
        private CancellationTokenSource cts;

        #region//初始化

        public SendExecute()
        {
            this.Worker.WorkerSupportsCancellation = true;
            this.Worker.WorkerReportsProgress = true;

            this.Worker.DoWork -= Send_DoWork;
            this.Worker.DoWork += Send_DoWork;

            this.Worker.ProgressChanged -= Send_ProgressChanged;
            this.Worker.ProgressChanged += Send_ProgressChanged;

            this.Worker.RunWorkerCompleted -= Send_RunCompleted;
            this.Worker.RunWorkerCompleted += Send_RunCompleted;
        }

        #endregion

        #region//启动发送

        public void StartSend(SendInfo si)
        {
            try
            {
                if (si != null && si.SCollection.Count > 0)
                {
                    if (!this.Worker.IsBusy)
                    {
                        //⚠️ 每轮一个新的：CTS 取消过就不能复位（与 RunWorkerAsync 会自己清 CancellationPending 不同）
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

                        this.Worker.RunWorkerAsync();

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
                if (this.Worker.IsBusy)
                {
                    /*
                        ⚠️ 2026-09-10 上午这里刚删掉一个「建了 CTS 却没人读 Token」的死写法，
                        下午把它<b>正着</b>加了回来 —— 区别是这次 DoWork 与 DoSleep 查的都是它。

                        <b>只 Cancel token，不调 Worker.CancelAsync()</b>：取消只能有一个真源。
                    */
                    CancellationTokenSource c = this.cts;
                    if (c != null) { c.Cancel(); }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(StopSend), ex);
            }
        }

        #endregion

        #region//执行发送集

        /*
            ⚠️ <b>这里不加 try/catch。</b> 加了的话异常被吞掉、e.Error 恒为 null，
            Send_RunCompleted 里「发生错误」那一支就成了死代码 ——
            发送炸在半路，日志里写的却是「执行完毕」（2026-09-10 修，与机器人那处同一个病根）。

            BackgroundWorker 会把 DoWork 抛出的异常收进 e.Error，<b>不会崩进程</b>；
            日志由 Send_RunCompleted 统一记一处。
        */
        private void Send_DoWork(object sender, DoWorkEventArgs e)
        {
            {
                CancellationToken token = this.cts.Token;

                if (this.SystemSocket)
                {
                    if (Operate.SystemConfig.SystemSocket <= 0)
                    {
                        Operate.DoLog(nameof(Send_DoWork), UI.T("System.SystemSocket.Error", "系统套接字未设置"));
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
                            if (token.IsCancellationRequested)
                            {
                                e.Cancel = true;
                                return;
                            }
                            else
                            {
                                int Socket = pi.PacketSocket;
                                if (this.SystemSocket)
                                {
                                    Socket = Operate.SystemConfig.SystemSocket;
                                }

                                /*
                                    ⚠️⚠️ 套接字 <= 0 原来是<b>整条静默跳过</b>：不发、不计数、不记日志。

                                    而 <b>SunnyNet 那条中间人路上产出的封包套接字一律是 0</b>
                                    （HTTP / HTTPS / WebSocket —— 它们靠会话号回发，压根没有套接字；
                                    SunnyNetCallback 里 8 个入队调用点第 4 个参数都是字面量 0）。
                                    而「代理数据页右键 → 添加到发送」正是把这种封包放进发送集最自然的一条路。

                                    于是不勾「使用系统套接字」时：<b>一个包都发不出去，三个计数全是 0，
                                    日志还写着「执行完毕」</b> —— 又一次「点了没反应」。
                                    实测（2026-09-10，改动前）：Total=0 OK=0 Fail=0，而「执行次数」照加了 1。

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
                                             "发送「{0}」里有封包没有套接字（HTTP / HTTPS / WebSocket 走的是会话号，套接字恒为 0）—— 这些封包发不出去，请勾上「使用系统套接字」并先在封包列表里右键设置它。"),
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
                                        Worker.ReportProgress(j);
                                        Operate.SystemConfig.DoSleep(this.LoopINT, token);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region//汇报进度

        private void Send_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.SendCollection_Index = e.ProgressPercentage;
        }

        #endregion

        #region//执行完毕

        private void Send_RunCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                string sMsg = string.Empty;

                if (e.Cancelled)
                {
                    sMsg = string.Format(UI.T("SendExecute.Stop", "发送 [{0}] 已停止"), this.SendName);                    
                }
                else if (e.Error != null) 
                {
                    sMsg = string.Format(UI.T("SendExecute.Error", "发送[{0}] 发生错误: {1}"), this.SendName, e.Error.Message);                    
                }
                else
                {
                    sMsg = string.Format(UI.T("SendExecute.Success", "发送[{0}] 执行完毕"), this.SendName);                    
                }

                Operate.DoLog(nameof(Send_RunCompleted), sMsg);                
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(Send_RunCompleted), ex);
            }
        }

        #endregion
    }
}
