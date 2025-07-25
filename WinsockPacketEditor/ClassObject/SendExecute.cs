using System;
using System.ComponentModel;
using System.Reflection;
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, sLog);
                    }
                }         
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, AntdUI.Localization.Get("System.SystemSocket.Error", "系统套接字未设置"));
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
                                    bool bOK = Operate.PacketConfig.Packet.SendPacket(Socket, pi.PacketType, string.Empty, pi.PacketTo, pi.PacketBuffer);

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
                                        Operate.SystemConfig.DoSleepAsync(this.LoopINT, this.cts.Token).Wait();
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

                Operate.DoLog(MethodBase.GetCurrentMethod().Name, sMsg);                
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion
    }
}
