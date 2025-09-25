using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using WindowsInput.Native;

namespace WinsockPacketEditor
{
    public class RobotExecute
    {
        public int Instruction_Index = 0;      
        public int Total_Instruction = 0;
        public string RobotName = string.Empty;
        private Dictionary<string, object> RParameters = new Dictionary<string, object>();

        private CancellationTokenSource cts;
        private RobotInfo riSelect;
        private BindingList<InstructionInfo> RInstruction;
        public BackgroundWorker Worker = new BackgroundWorker();
        private readonly WindowsInput.InputSimulator sim = new WindowsInput.InputSimulator();        

        #region//初始化

        public RobotExecute()
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

        public void StartRobot(RobotInfo ri, Dictionary<string, object> parameters)
        {
            try
            {
                if (ri != null && ri.RInstruction.Count > 0)
                {
                    if (!this.Worker.IsBusy)
                    {
                        this.Total_Instruction = 0;

                        this.riSelect = ri;
                        this.RobotName = ri.RName;
                        this.RInstruction = ri.RInstruction;

                        if (parameters != null)
                        {
                            this.RParameters = parameters;
                        }
                        else
                        {
                            this.RParameters.Clear();
                        }

                        int iReturn = Operate.RobotConfig.Robot.CheckRobotInstruction(null, this.RInstruction);
                        if (iReturn > -1)
                        {
                            string sLog = string.Format(AntdUI.Localization.Get("System.Robot.Error", "机器人指令 {0} 错误! [{1}]"), iReturn + 1, this.RobotName);
                            Operate.DoLog(nameof(StartRobot), sLog);
                        }
                        else
                        {
                            this.cts = new CancellationTokenSource();
                            this.Worker.RunWorkerAsync();

                            string sLog = string.Format(AntdUI.Localization.Get("System.Robot.Start", "启动机器人 [{0}]"), this.RobotName);
                            Operate.DoLog(nameof(StartRobot), sLog);
                        }
                    }
                }      
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(StartRobot), ex.Message);
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
                    if (this.cts != null)
                    {
                        this.cts.Cancel();
                    }
                    
                    this.Worker.CancelAsync();
                }                
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(StopRobot), ex.Message);
            }
        }

        #endregion

        #region//执行指令集

        private void Robot_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (this.RInstruction.Count > 0)
                {
                    Stack<int> sLoopStart = new Stack<int>();
                    Dictionary<int, int> dLoopCNT = new Dictionary<int, int>();

                    for (int i = 0; i < this.RInstruction.Count; i++)
                    {
                        if (Worker.CancellationPending)
                        {
                            e.Cancel = true;
                            return;
                        }
                        else
                        {
                            Worker.ReportProgress(i);

                            string sContent = RInstruction[i].InstContent;
                            switch (RInstruction[i].InstType)
                            {
                                case Operate.RobotConfig.Robot.InstructionType.SendSendList:

                                    if (!string.IsNullOrEmpty(sContent))
                                    {
                                        Guid SID = Guid.Parse(sContent);
                                        SendExecute ss = Operate.SendConfig.Send.DoSend(SID);

                                        if (ss != null)
                                        {
                                            while (ss.Worker.IsBusy)
                                            {
                                                if (this.Worker.CancellationPending)
                                                {
                                                    ss.StopSend();

                                                    e.Cancel = true;
                                                    return;
                                                }

                                                Thread.Sleep(100);
                                            }
                                        }                                        
                                    }

                                    break;

                                case Operate.RobotConfig.Robot.InstructionType.SendPacketList:

                                    Operate.PacketConfig.List.SendSocketList_BySelect();

                                    break;

                                case Operate.RobotConfig.Robot.InstructionType.SetSystemSocket:

                                    int iSocket = 0;
                                    if (sContent.Equals("PacketConfig.List"))
                                    {
                                        if (Operate.PacketConfig.List.piSelect != null)
                                        {
                                            iSocket = Operate.PacketConfig.List.piSelect.PacketSocket;                                            
                                        }
                                    }
                                    else if (sContent.Equals("FilterSocket"))
                                    {
                                        iSocket = GetParameter<int>("FilterSocket", -1);
                                    }
                                    else if (sContent.Contains("Customize") && sContent.Contains("|"))
                                    {
                                        if (int.TryParse(sContent.Split('|')[1], out int CustomSocket))
                                        {
                                            iSocket = CustomSocket;
                                        }
                                    }

                                    if (iSocket > 0)
                                    {
                                        Operate.SystemConfig.SystemSocket = iSocket;
                                    }

                                    break;

                                case Operate.RobotConfig.Robot.InstructionType.Delay:

                                    int iDelay = 0;
                                    if (sContent.Contains("-"))
                                    {
                                        string sFrom = sContent.Split('-')[0];
                                        string sTo = sContent.Split('-')[1];

                                        if (int.TryParse(sFrom, out int iFrom) && int.TryParse(sTo, out int iTo))
                                        {
                                            Random random = new Random();
                                            iDelay = random.Next(iFrom, iTo + 1);

                                            Operate.SystemConfig.DoSleepAsync(iDelay, this.cts.Token).Wait();
                                        }
                                    }
                                    else
                                    {
                                        if (int.TryParse(sContent, out iDelay))
                                        {
                                            Operate.SystemConfig.DoSleepAsync(iDelay, this.cts.Token).Wait();
                                        }
                                    }                                    

                                    break;

                                case Operate.RobotConfig.Robot.InstructionType.LoopStart:

                                    if (int.TryParse(sContent, out int Count))
                                    {
                                        sLoopStart.Push(i);

                                        if (dLoopCNT.ContainsKey(i))
                                        {
                                            dLoopCNT[i] = Count;
                                        }
                                        else
                                        {
                                            dLoopCNT.Add(i, Count);
                                        }
                                    }

                                    break;

                                case Operate.RobotConfig.Robot.InstructionType.LoopEnd:

                                    if (sLoopStart.Count > 0)
                                    {
                                        int iLoopStart = sLoopStart.Peek();

                                        if (dLoopCNT.ContainsKey(iLoopStart))
                                        {
                                            int iLoopCNT = dLoopCNT[iLoopStart];

                                            iLoopCNT--;

                                            if (iLoopCNT > 0)
                                            {
                                                dLoopCNT[iLoopStart] = iLoopCNT;
                                                i = iLoopStart;
                                            }
                                            else
                                            {
                                                sLoopStart.Pop();
                                            }
                                        }
                                    }

                                    break;

                                case Operate.RobotConfig.Robot.InstructionType.KeyBoard:

                                    if (!string.IsNullOrEmpty(sContent) && sContent.IndexOf("|") > 0)
                                    {
                                        Operate.RobotConfig.Robot.KeyBoardType kbType = Operate.RobotConfig.Robot.GetKeyBoardType_ByString(sContent.Split('|')[0].ToString());
                                        string KeyCode = sContent.Split('|')[1];

                                        Keys kCode;
                                        VirtualKeyCode vkCode;

                                        switch (kbType)
                                        {
                                            case Operate.RobotConfig.Robot.KeyBoardType.Press:

                                                if (Enum.TryParse(KeyCode, true, out kCode))
                                                {
                                                    if (Enum.TryParse(((int)kCode).ToString(), true, out vkCode))
                                                    {
                                                        sim.Keyboard.KeyPress(vkCode);
                                                    }
                                                }

                                                break;

                                            case Operate.RobotConfig.Robot.KeyBoardType.Down:

                                                if (Enum.TryParse(KeyCode, true, out kCode))
                                                {
                                                    if (Enum.TryParse(((int)kCode).ToString(), true, out vkCode))
                                                    {
                                                        sim.Keyboard.KeyDown(vkCode);
                                                    }
                                                }

                                                break;

                                            case Operate.RobotConfig.Robot.KeyBoardType.Up:

                                                if (Enum.TryParse(KeyCode, true, out kCode))
                                                {
                                                    if (Enum.TryParse(((int)kCode).ToString(), true, out vkCode))
                                                    {
                                                        sim.Keyboard.KeyUp(vkCode);
                                                    }
                                                }

                                                break;

                                            case Operate.RobotConfig.Robot.KeyBoardType.Combine:

                                                if (KeyCode.IndexOf("+") > 0)
                                                {
                                                    string[] slKeyCode = KeyCode.Split('+');

                                                    List<VirtualKeyCode> ControlKey = new List<VirtualKeyCode>();
                                                    List<VirtualKeyCode> NormalKey = new List<VirtualKeyCode>();

                                                    foreach (string sKey in slKeyCode)
                                                    {
                                                        if (Enum.TryParse(sKey, true, out kCode))
                                                        {
                                                            if (Enum.TryParse(((int)kCode).ToString(), true, out vkCode))
                                                            {
                                                                if (vkCode == VirtualKeyCode.CONTROL || vkCode == VirtualKeyCode.MENU || vkCode == VirtualKeyCode.SHIFT)
                                                                {
                                                                    ControlKey.Add(vkCode);
                                                                }
                                                                else
                                                                {
                                                                    NormalKey.Add(vkCode);
                                                                }
                                                            }
                                                        }
                                                    }

                                                    sim.Keyboard.ModifiedKeyStroke(ControlKey, NormalKey);
                                                }

                                                break;

                                            case Operate.RobotConfig.Robot.KeyBoardType.Text:

                                                if (!string.IsNullOrEmpty(KeyCode))
                                                {
                                                    sim.Keyboard.TextEntry(KeyCode);
                                                }

                                                break;
                                        }
                                    }

                                    break;

                                case Operate.RobotConfig.Robot.InstructionType.Mouse:

                                    if (!string.IsNullOrEmpty(sContent) && sContent.IndexOf("|") > 0)
                                    {
                                        Operate.RobotConfig.Robot.MouseType mType = Operate.RobotConfig.Robot.GetMouseType_ByString(sContent.Split('|')[0].ToString());
                                        string MouseCode = sContent.Split('|')[1];

                                        int iMouseCode = 0;
                                        switch (mType)
                                        {
                                            case Operate.RobotConfig.Robot.MouseType.LeftClick:
                                                sim.Mouse.LeftButtonClick();
                                                break;

                                            case Operate.RobotConfig.Robot.MouseType.RightClick:
                                                sim.Mouse.RightButtonClick();
                                                break;

                                            case Operate.RobotConfig.Robot.MouseType.LeftDBClick:
                                                sim.Mouse.LeftButtonDoubleClick();
                                                break;

                                            case Operate.RobotConfig.Robot.MouseType.RightDBClick:
                                                sim.Mouse.RightButtonDoubleClick();
                                                break;

                                            case Operate.RobotConfig.Robot.MouseType.LeftDown:
                                                sim.Mouse.LeftButtonDown();
                                                break;

                                            case Operate.RobotConfig.Robot.MouseType.LeftUp:
                                                sim.Mouse.LeftButtonUp();
                                                break;

                                            case Operate.RobotConfig.Robot.MouseType.RightDown:
                                                sim.Mouse.RightButtonDown();
                                                break;

                                            case Operate.RobotConfig.Robot.MouseType.RightUp:
                                                sim.Mouse.RightButtonUp();
                                                break;

                                            case Operate.RobotConfig.Robot.MouseType.WheelUp:

                                                if (int.TryParse(MouseCode, out iMouseCode))
                                                {
                                                    sim.Mouse.VerticalScroll(iMouseCode);
                                                }

                                                break;

                                            case Operate.RobotConfig.Robot.MouseType.WheelDown:

                                                if (int.TryParse(MouseCode, out iMouseCode))
                                                {
                                                    sim.Mouse.VerticalScroll(-iMouseCode);
                                                }

                                                break;

                                            case Operate.RobotConfig.Robot.MouseType.MoveTo:

                                                if (MouseCode.IndexOf(",") > 0)
                                                {
                                                    string sMoveX = MouseCode.Split(',')[0].Trim();
                                                    string sMoveY = MouseCode.Split(',')[1].Trim();

                                                    if (int.TryParse(sMoveX, out int iX) && int.TryParse(sMoveY, out int iY))
                                                    {
                                                        sim.Mouse.MoveMouseTo(iX, iY);
                                                    }
                                                }

                                                break;

                                            case Operate.RobotConfig.Robot.MouseType.MoveBy:

                                                if (MouseCode.IndexOf(",") > 0)
                                                {
                                                    string sMoveX = MouseCode.Split(',')[0].Trim();
                                                    string sMoveY = MouseCode.Split(',')[1].Trim();

                                                    if (int.TryParse(sMoveX, out int iX) && int.TryParse(sMoveY, out int iY))
                                                    {
                                                        sim.Mouse.MoveMouseBy(iX, iY);
                                                    }
                                                }

                                                break;
                                        }
                                    }

                                    break;
                            }

                            if (RInstruction[i].InstType != Operate.RobotConfig.Robot.InstructionType.LoopStart && 
                                RInstruction[i].InstType != Operate.RobotConfig.Robot.InstructionType.LoopEnd)
                            {
                                this.Total_Instruction++;
                            }
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(Robot_DoWork), ex.Message);
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
                    string sLog = string.Format(AntdUI.Localization.Get("Robot.Stop", "机器人 [{0}] 已停止"), this.RobotName);
                    Operate.DoLog(nameof(Robot_RunCompleted), sLog);                    
                }
                else if (e.Error != null)
                {
                    string sLog = string.Format(AntdUI.Localization.Get("Robot.Error", "机器人 [{0}] 发生错误: {1}"), this.RobotName, e.Error.Message);
                    Operate.DoLog(nameof(Robot_RunCompleted), sLog);
                }
                else
                {
                    string sLog = string.Format(AntdUI.Localization.Get("Robot.Success", "机器人 [{0}] 执行完毕"), this.RobotName);
                    Operate.DoLog(nameof(Robot_RunCompleted), sLog);
                }              
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(Robot_RunCompleted), ex.Message);
            }
        }

        #endregion

        #region//解析参数

        private object GetParameter(string key)
        {
            try
            {
                if (this.RParameters.ContainsKey(key))
                {
                    return this.RParameters[key];
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(GetParameter), ex.Message);
            }            

            return null;
        }

        private T GetParameter<T>(string key, T defaultValue = default(T))
        {
            try
            {
                if (this.RParameters.ContainsKey(key))
                {
                    try
                    {
                        return (T)Convert.ChangeType(this.RParameters[key], typeof(T));
                    }
                    catch
                    {
                        return defaultValue;
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(GetParameter), ex.Message);
            }
            
            return defaultValue;
        }

        #endregion
    }
}
