using System;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace WPELibrary.Lib
{
    public class Socket_Robot
    {
        public int Instruction_Index = 0;
        public int Send_Success = 0;
        public int Send_Failure = 0;
        public int Total_Instruction = 0;
        public string RobotName = string.Empty;

        private User32.SendKeyboardMouse SendKeyMouse = new User32.SendKeyboardMouse();
        private DataTable RobotInstruction = new DataTable();
        public BackgroundWorker Worker = new BackgroundWorker();

        public Socket_Robot()
        {  
            this.Worker.WorkerSupportsCancellation = true;
            this.Worker.WorkerReportsProgress = true;

            this.Worker.DoWork -= Robot_DoWork;
            this.Worker.DoWork += Robot_DoWork;

            this.Worker.ProgressChanged -= Robot_ProgressChanged;
            this.Worker.ProgressChanged += Robot_ProgressChanged;

            this.Worker.RunWorkerCompleted -= Robot_RunCompleted;
            this.Worker.RunWorkerCompleted += Robot_RunCompleted;
        }        

        #region//开始

        public void StartRobot(string RobotName, DataTable dtRobotInstruction)
        {
            try
            {
                if (!this.Worker.IsBusy)
                {
                    if (dtRobotInstruction.Rows.Count > 0)
                    {
                        this.RobotName = RobotName;
                        this.RobotInstruction = dtRobotInstruction;
                        this.Worker.RunWorkerAsync();

                        string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_109), this.RobotName);
                        Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sLog);
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//停止

        public void StopRobot()
        {
            try
            {
                if (this.Worker.IsBusy)
                {
                    this.Worker.CancelAsync();
                }                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//执行指令集

        private void Robot_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (this.RobotInstruction.Rows.Count > 0)
                {
                    this.Total_Instruction = 0;
                    this.Send_Success = 0;
                    this.Send_Failure = 0;

                    int iLoopStart = 0;
                    int iLoopCNT = 1;          

                    for (int i = 0; i < this.RobotInstruction.Rows.Count; i++)
                    {
                        if (Worker.CancellationPending)
                        {
                            e.Cancel = true;
                            return;
                        }

                        Worker.ReportProgress(i);             

                        Socket_Cache.Robot.InstructionType instructionType = (Socket_Cache.Robot.InstructionType)RobotInstruction.Rows[i]["Type"];
                        string sContent = RobotInstruction.Rows[i]["Content"].ToString();

                        switch (instructionType)
                        {
                            case Socket_Cache.Robot.InstructionType.Send:

                                if (!string.IsNullOrEmpty(sContent) && sContent.IndexOf("|") > 0)
                                {
                                    int SendPacket_Index = 0;
                                    if (int.TryParse(sContent.Split('|')[0], out int iIndex))
                                    {
                                        SendPacket_Index = iIndex;
                                    }

                                    int SendPacket_Socket = 0;
                                    if (int.TryParse(sContent.Split('|')[1], out int iSocket))
                                    {
                                        SendPacket_Socket = iSocket;
                                    }

                                    bool bOK = Socket_Cache.SendList.DoSendList_ByIndex(SendPacket_Socket, SendPacket_Index);

                                    if (bOK)
                                    {
                                        this.Send_Success++;
                                    }
                                    else
                                    {
                                        this.Send_Failure++;
                                    }

                                    this.Total_Instruction++;
                                }

                                break;

                            case Socket_Cache.Robot.InstructionType.Delay:

                                if (int.TryParse(sContent, out int iDelay))
                                {
                                    Thread.Sleep(iDelay);

                                    this.Total_Instruction++;
                                }

                                break;

                            case Socket_Cache.Robot.InstructionType.LoopStart:

                                iLoopStart = i;
                                int.TryParse(sContent, out iLoopCNT);

                                break;

                            case Socket_Cache.Robot.InstructionType.LoopEnd:

                                iLoopCNT--;

                                if (iLoopCNT > 0)
                                {
                                    i = iLoopStart;
                                }

                                break;

                            case Socket_Cache.Robot.InstructionType.KeyBoard:

                                if (!string.IsNullOrEmpty(sContent) && sContent.IndexOf("|") > 0)
                                {
                                    Socket_Cache.Robot.KeyBoardType kbType = Socket_Cache.Robot.GetKeyBoardType_ByString(sContent.Split('|')[0].ToString());
                                    string KeyCode = sContent.Split('|')[1];

                                    Keys kCode = Keys.None;

                                    switch (kbType)
                                    {
                                        case Socket_Cache.Robot.KeyBoardType.Press:

                                            if (Enum.TryParse(KeyCode, true, out kCode))
                                            {
                                                SendKeyMouse.SendMessagePress(((byte)kCode));
                                            }

                                            break;

                                        case Socket_Cache.Robot.KeyBoardType.Combine:

                                            if (KeyCode.IndexOf("+") > 0)
                                            {
                                                KeyCode = KeyCode.Replace(" ", "");

                                                string[] slKeyCode = KeyCode.Split('+');                                                

                                                byte[] bKeyCode = new byte[slKeyCode.Length];

                                                for (int j = 0; j < slKeyCode.Length; j++)
                                                {
                                                    string sKey = slKeyCode[j].ToString();

                                                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sKey);

                                                    if (Enum.TryParse(sKey, true, out kCode))
                                                    {                                                        
                                                        bKeyCode[j] = (byte)kCode;

                                                        Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ((int)kCode).ToString("X2"));
                                                    }
                                                }

                                                SendKeyMouse.SendCombineKey(bKeyCode);
                                            }

                                            break;
                                    }                                    
                                }                           

                                break;
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//汇报进度

        private void Robot_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.Instruction_Index = e.ProgressPercentage;
        }

        #endregion

        #region//执行完毕

        private void Robot_RunCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_110), this.RobotName);
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sLog);                    
                }
                else if (e.Error != null)
                {
                    string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_111), this.RobotName, e.Error.Message);
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sLog);
                }
                else
                {
                    string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_112), this.RobotName);
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sLog);
                }              
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion
    }
}
