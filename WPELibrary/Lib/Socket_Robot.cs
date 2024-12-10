using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

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

        #region//初始化

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

        #endregion

        #region//启动机器人

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

                        int iReturn = Socket_Cache.Robot.CheckRobotInstruction(this.RobotInstruction, true);

                        if (iReturn > -1)
                        {
                            string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_123), iReturn + 1, this.RobotName);
                            Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sLog);
                        }
                        else
                        {
                            this.Worker.RunWorkerAsync();

                            string sLog = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_109), this.RobotName);
                            Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, sLog);
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

        #region//停止机器人

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
                                }

                                break;

                            case Socket_Cache.Robot.InstructionType.Delay:

                                if (int.TryParse(sContent, out int iDelay))
                                {
                                    Thread.Sleep(iDelay);                                
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

                                        case Socket_Cache.Robot.KeyBoardType.Down:

                                            if (Enum.TryParse(KeyCode, true, out kCode))
                                            {
                                                SendKeyMouse.SendMessageDown(((byte)kCode));
                                            }

                                            break;

                                        case Socket_Cache.Robot.KeyBoardType.Up:

                                            if (Enum.TryParse(KeyCode, true, out kCode))
                                            {
                                                SendKeyMouse.SendMessageUp(((byte)kCode));
                                            }

                                            break;

                                        case Socket_Cache.Robot.KeyBoardType.Combine:

                                            if (KeyCode.IndexOf("+") > 0)
                                            {
                                                string[] slKeyCode = KeyCode.Split('+');
                                                byte[] bKeyCode = new byte[slKeyCode.Length];

                                                for (int j = 0; j < slKeyCode.Length; j++)
                                                {
                                                    string sKey = slKeyCode[j].ToString().Trim();                                                    

                                                    if (Enum.TryParse(sKey, true, out kCode))
                                                    {                                                        
                                                        bKeyCode[j] = (byte)kCode;                                                        
                                                    }
                                                }

                                                SendKeyMouse.SendCombineKey(bKeyCode);
                                            }

                                            break;                                   
                                    }                                    
                                }                           

                                break;

                            case Socket_Cache.Robot.InstructionType.Mouse:

                                if (!string.IsNullOrEmpty(sContent) && sContent.IndexOf("|") > 0)
                                {
                                    Socket_Cache.Robot.MouseType mType = Socket_Cache.Robot.GetMouseType_ByString(sContent.Split('|')[0].ToString());
                                    string MouseCode = sContent.Split('|')[1];

                                    int iMouseCode = 0;
                                    switch (mType)
                                    {
                                        case Socket_Cache.Robot.MouseType.LeftClick:
                                            SendKeyMouse.MouseLeftClick();
                                            break;

                                        case Socket_Cache.Robot.MouseType.RightClick:
                                            SendKeyMouse.MouseRightClick();
                                            break;

                                        case Socket_Cache.Robot.MouseType.LeftDBClick:
                                            SendKeyMouse.MouseLeftDBClick();
                                            break;

                                        case Socket_Cache.Robot.MouseType.RightDBClick:
                                            SendKeyMouse.MouseRightDBClick();
                                            break;

                                        case Socket_Cache.Robot.MouseType.LeftDown:
                                            SendKeyMouse.MouseLeftDown();
                                            break;

                                        case Socket_Cache.Robot.MouseType.LeftUp:
                                            SendKeyMouse.MouseLeftUp();
                                            break;

                                        case Socket_Cache.Robot.MouseType.RightDown:
                                            SendKeyMouse.MouseRightDown();
                                            break;

                                        case Socket_Cache.Robot.MouseType.RightUp:
                                            SendKeyMouse.MouseRightUp();
                                            break;

                                        case Socket_Cache.Robot.MouseType.WheelUp:

                                            if (int.TryParse(MouseCode, out iMouseCode))
                                            {
                                                SendKeyMouse.MouseWheel(iMouseCode);
                                            }
                                            
                                            break;

                                        case Socket_Cache.Robot.MouseType.WheelDown:

                                            if (int.TryParse(MouseCode, out iMouseCode))
                                            {
                                                SendKeyMouse.MouseWheel(-iMouseCode);
                                            }

                                            break;

                                        case Socket_Cache.Robot.MouseType.Move:

                                            if (MouseCode.IndexOf(",") > 0)
                                            {
                                                string sMoveX = MouseCode.Split(',')[0].Trim();
                                                string sMoveY = MouseCode.Split(',')[1].Trim();

                                                if (int.TryParse(sMoveX, out int iX) && int.TryParse(sMoveY, out int iY))
                                                {
                                                    SendKeyMouse.MouseMove(new Point(iX, iY));
                                                }                                              
                                            }                                         

                                            break;
                                    }
                                }

                                break;
                        }

                        if (instructionType != Socket_Cache.Robot.InstructionType.LoopStart && instructionType != Socket_Cache.Robot.InstructionType.LoopEnd)
                        {
                            this.Total_Instruction++;
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
