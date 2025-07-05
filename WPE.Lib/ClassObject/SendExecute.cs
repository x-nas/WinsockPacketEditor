﻿using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading;

namespace WPE.Lib
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

        public void StartSend(string SendName, bool SystemSocket, int LoopCNT, int LoopINT, BindingList<PacketInfo> SendCollection)
        {
            try
            {
                if (SendCollection.Count > 0)
                {
                    if (!this.Worker.IsBusy)
                    {
                        this.Total_Send = 0;
                        this.Send_Success = 0;
                        this.Send_Failure = 0;

                        this.SendName = SendName;
                        this.SystemSocket = SystemSocket;
                        this.LoopCNT = LoopCNT;
                        this.LoopINT = LoopINT;
                        this.SendCollection = SendCollection;

                        this.cts = new CancellationTokenSource();
                        this.Worker.RunWorkerAsync();

                        string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_84), this.SendName);
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
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_49));
                        return;
                    }
                }

                for (int i = 0; i < this.LoopCNT; i++)
                {
                    foreach (PacketInfo spi in this.SendCollection)
                    {
                        if (Worker.CancellationPending)
                        {
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            int Socket = spi.PacketSocket;
                            if (this.SystemSocket)
                            {
                                Socket = Operate.SystemConfig.SystemSocket;
                            }

                            if (Socket > 0)
                            {
                                bool bOK = Socket_Operation.SendPacket(Socket, spi.PacketType, string.Empty, spi.PacketTo, spi.PacketBuffer);

                                if (bOK)
                                {
                                    this.Send_Success++;
                                }
                                else
                                {
                                    this.Send_Failure++;
                                }

                                this.Total_Send++;

                                if (this.LoopINT > 0)
                                {
                                    Worker.ReportProgress(i);
                                    Socket_Operation.DoSleepAsync(this.LoopINT, this.cts.Token).Wait();
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
                if (e.Cancelled)
                {
                    string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_163), this.SendName);
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, sLog);
                }
                else if (e.Error != null)
                {
                    string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_164), this.SendName, e.Error.Message);
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, sLog);
                }
                else
                {
                    string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_165), this.SendName);
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, sLog);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion
    }
}
