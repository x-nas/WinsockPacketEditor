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

        private CancellationTokenSource cts;
        private SendInfo siSelect;
        private BindingList<PacketInfo> SendCollection;
        public BackgroundWorker Worker = new BackgroundWorker();

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
                        this.Total_Send = 0;
                        this.Send_Success = 0;
                        this.Send_Failure = 0;

                        this.siSelect = si;
                        this.SendName = si.SName;
                        this.SystemSocket = si.SSystemSocket;
                        this.LoopCNT = si.SLoopCNT;
                        this.LoopINT = si.SLoopINT;
                        this.SendCollection = si.SCollection;                        

                        this.cts = new CancellationTokenSource();
                        this.Worker.RunWorkerAsync();

                        string sLog = string.Format(AntdUI.Localization.Get("SendExecute.DoSend", "执行发送 [{0}]"), this.SendName);
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
                    if (this.cts != null)
                    {
                        this.cts.Cancel();
                    }
                    
                    this.Worker.CancelAsync();
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(StopSend), ex);
            }
        }

        #endregion

        #region//执行发送集

        private void Send_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (this.SystemSocket)
                {
                    if (Operate.SystemConfig.SystemSocket <= 0)
                    {
                        Operate.DoLog(nameof(Send_DoWork), AntdUI.Localization.Get("System.SystemSocket.Error", "系统套接字未设置"));
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
                            if (Worker.CancellationPending)
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

                                if (Socket > 0)
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
                                        Operate.SystemConfig.DoSleep(this.LoopINT, this.Worker);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(Send_DoWork), ex);
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
                    sMsg = string.Format(AntdUI.Localization.Get("SendExecute.Stop", "发送 [{0}] 已停止"), this.SendName);                    
                }
                else if (e.Error != null) 
                {
                    sMsg = string.Format(AntdUI.Localization.Get("SendExecute.Error", "发送[{0}] 发生错误: {1}"), this.SendName, e.Error.Message);                    
                }
                else
                {
                    sMsg = string.Format(AntdUI.Localization.Get("SendExecute.Success", "发送[{0}] 执行完毕"), this.SendName);                    
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
