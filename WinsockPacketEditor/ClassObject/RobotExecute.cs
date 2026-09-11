using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using WindowsInput.Native;
using static WinsockPacketEditor.Operate;

namespace WinsockPacketEditor
{
    public class RobotExecute
    {
        public int Instruction_Index = 0;      
        public int Total_Instruction = 0;
        public string RobotName = string.Empty;
        private Dictionary<string, object> RParameters = new Dictionary<string, object>();

        private RobotInfo riSelect;
        private BindingList<InstructionInfo> RInstruction;
        public BackgroundWorker Worker = new BackgroundWorker();

        /*
            ⚠️⚠️ <b>取消的唯一真源</b>（2026-09-10）—— 与 SendExecute 那处逐字同构，理由见那边。
            简版：CancellationPending 是裸 bool、只能轮询；token 有 WaitHandle，
            「等某件事做完」那几处才改得成阻塞等待。StopRobot 里刻意不再调 CancelAsync()。
        */
        private CancellationTokenSource cts;
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
                        //⚠️ 每轮一个新的：CTS 取消过就不能复位
                        if (this.cts != null) { this.cts.Dispose(); }
                        this.cts = new CancellationTokenSource();

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

                        int iReturn = Operate.RobotConfig.Robot.CheckRobotInstruction(false, this.RInstruction);
                        if (iReturn > -1)
                        {
                            string sLog = string.Format(AntdUI.Localization.Get("System.Robot.Error", "机器人指令 {0} 错误! [{1}]"), iReturn + 1, this.RobotName);
                            Operate.DoLog(nameof(StartRobot), sLog);
                        }
                        else
                        {
                            this.Worker.RunWorkerAsync();

                            string sLog = string.Format(AntdUI.Localization.Get("System.Robot.Start", "启动机器人 [{0}]"), this.RobotName);
                            Operate.DoLog(nameof(StartRobot), sLog);
                        }
                    }
                }      
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(StartRobot), ex);
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
                    /*
                        ⚠️ 2026-09-09 这里删掉过一个「建了 CTS 却没人读 Token」的死写法，
                        2026-09-10 把它<b>正着</b>加了回来 —— 这次 DoWork 与 DoSleep 查的都是它。

                        <b>只 Cancel token，不调 Worker.CancelAsync()</b>：取消只能有一个真源。
                    */
                    CancellationTokenSource c = this.cts;
                    if (c != null) { c.Cancel(); }
                }                
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(StopRobot), ex);
            }
        }

        #endregion

        #region//执行指令集

        /*
            ⚠️ <b>这里不加 try/catch。</b> 加了的话异常被吞掉、e.Error 恒为 null，
            RunWorkerCompleted 里「error:」那一支就成了死代码 ——
            机器人炸在半路，界面弹的却是绿色的「执行完毕」（2026-09-09 修）。

            BackgroundWorker 会把 DoWork 抛出的异常收进 e.Error，<b>不会崩进程</b>；
            日志由 Robot_RunCompleted 统一记一处，外壳那边的 editResult 也才拿得到 "error:"。
        */
        private void Robot_DoWork(object sender, DoWorkEventArgs e)
        {
            {
                CancellationToken token = this.cts.Token;

                if (this.RInstruction.Count > 0)
                {
                    Stack<int> sLoopStart = new Stack<int>();
                    Dictionary<int, int> dLoopCNT = new Dictionary<int, int>();

                    for (int i = 0; i < this.RInstruction.Count; i++)
                    {
                        if (token.IsCancellationRequested)
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
                                            /*
                                                ⚠️ 这是「等某件事做完」，不是「等准一段时长」——
                                                所以用 WaitOne：取消当场返回，不必先睡满这一觉
                                                （原来最坏 100ms 才看一眼取消标记）。
                                                精度在这儿毫无意义，与 DoSleep 的取舍正好相反。
                                            */
                                            while (ss.Worker.IsBusy)
                                            {
                                                if (token.WaitHandle.WaitOne(100))
                                                {
                                                    ss.StopSend();

                                                    e.Cancel = true;
                                                    return;
                                                }
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
                                        //【B-IPC 阶段 0】走 IHookHost，不再直读 piSelect。
                                        SelectedPacket sp = HookHost.Current.GetSelectedPacket();
                                        if (sp != null)
                                        {
                                            iSocket = sp.Socket;
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
                                            iDelay = NextRandom(iFrom, iTo);

                                            Operate.SystemConfig.DoSleep(iDelay, token);
                                        }
                                    }
                                    else
                                    {
                                        if (int.TryParse(sContent, out iDelay))
                                        {
                                            Operate.SystemConfig.DoSleep(iDelay, token);
                                        }
                                    }                                    

                                    break;

                                case Operate.RobotConfig.Robot.InstructionType.LoopStart:

                                    /*
                                        ⚠️⚠️ <b>无论内容解析成不成功都要入栈。</b>
                                        原来是「解析失败就不 Push」—— 那条指令对应的 LoopEnd 随后会
                                        Peek 到<b>外层</b>循环并减它的计数，整个嵌套当场错位，
                                        而 CheckRobotInstruction 只数个数、不查内容，拦不住。

                                        ⚠️ 次数下限 <b>1</b>：存 0 时原来的走法是
                                        「LoopEnd 里 0-- = -1，不大于 0 → 出栈」，
                                        <b>循环体已经跑过一遍了</b> —— 那既不是 0 次也不是 1 次，是个说不清的状态。
                                        新界面的 ValidateInstruction 要求 >= 1，这里对齐它：老库 / 备份
                                        进来的 0 一律当 1，并记一条节流日志，别静默改语义。
                                    */
                                    int LoopCount;
                                    if (!int.TryParse(sContent, out LoopCount) || LoopCount < 1)
                                    {
                                        Operate.SystemConfig.LogThrottled(nameof(Robot_DoWork),
                                            string.Format(AntdUI.Localization.Get("System.Robot.LoopCount",
                                                "指令 {0} 的循环次数不正确（{1}），已按 1 次处理"), i + 1, sContent));

                                        LoopCount = 1;
                                    }

                                    sLoopStart.Push(i);
                                    dLoopCNT[i] = LoopCount;

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

                                case Operate.RobotConfig.Robot.InstructionType.Switch:

                                    if (!string.IsNullOrEmpty(sContent))
                                    {
                                        if (sContent.Contains("|"))
                                        {
                                            string[] slSwitch = sContent.Split('|');
                                            if (slSwitch.Length == 3)
                                            {
                                                if (Guid.TryParse(slSwitch[2], out Guid GID))
                                                {
                                                    bool Switch = false;
                                                    switch (slSwitch[0])
                                                    {
                                                        case "Enable":
                                                            Switch = true;
                                                            break;

                                                        case "Disable":
                                                            Switch = false;
                                                            break;
                                                    }

                                                    switch (slSwitch[1])
                                                    {
                                                        case "SendList":
                                                            Operate.SendConfig.Send.SetIsEnable_ByGUID(GID, Switch);
                                                            break;

                                                        case "RobotList":
                                                            Operate.RobotConfig.Robot.SetIsEnable_ByGUID(GID, Switch);
                                                            break;

                                                        case "FilterList":
                                                            Operate.FilterConfig.Filter.SetIsEnable_ByGUID(GID, Switch);
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    break;

                                case Operate.RobotConfig.Robot.InstructionType.KeyBoard:

                                    if (!string.IsNullOrEmpty(sContent) && sContent.IndexOf("|") > 0)
                                    {
                                        Operate.RobotConfig.Robot.KeyBoardType kbType = Operate.RobotConfig.Robot.GetKeyBoardType_ByString(sContent.Split('|')[0].ToString());
                                        string KeyCode = sContent.Split('|')[1];

                                        VirtualKeyCode vkCode;

                                        switch (kbType)
                                        {
                                            /*
                                                ⚠️ 这三支原来是
                                                    Enum.TryParse(KeyCode, out kCode) -> Enum.TryParse(((int)kCode).ToString(), out vkCode)
                                                后半句拿<b>数字串</b>解枚举，而那么做<b>不检查该值是否定义</b> ——
                                                映射不到 VirtualKeyCode 的键会被静默当成一个未定义值发出去。
                                                Combine 那一支 2026-09-06 已经换成 TryGetVirtualKey 了，这三支<b>漏了</b>；
                                                2026-09-09 补齐（TryGetVirtualKey 里有 Enum.IsDefined 那道闸）。
                                            */
                                            case Operate.RobotConfig.Robot.KeyBoardType.Press:

                                                if (TryGetVirtualKey(KeyCode, out vkCode))
                                                {
                                                    sim.Keyboard.KeyPress(vkCode);
                                                }

                                                break;

                                            case Operate.RobotConfig.Robot.KeyBoardType.Down:

                                                if (TryGetVirtualKey(KeyCode, out vkCode))
                                                {
                                                    sim.Keyboard.KeyDown(vkCode);
                                                }

                                                break;

                                            case Operate.RobotConfig.Robot.KeyBoardType.Up:

                                                if (TryGetVirtualKey(KeyCode, out vkCode))
                                                {
                                                    sim.Keyboard.KeyUp(vkCode);
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
                                                        if (TryGetVirtualKey(sKey, out vkCode))
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

                    this.riSelect.ExecutionCount++;
                }
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
                Operate.DoLog(nameof(Robot_RunCompleted), ex);
            }
        }

        #endregion

        #region//随机延迟的随机数

        /*
            ⚠️⚠️ <b>不要在这儿 new Random()</b>（原来就是那么写的，2026-09-09 修）。

            .NET Framework 的无参 Random 按 <b>Environment.TickCount</b> 播种，而它的分辨率是
            ~15.6ms —— 紧挨着建出来的多个实例拿到<b>同一个种子</b>，于是吐<b>同一个数</b>。
            实测连着建 12 个 Random 各取一次 Next(1,101)：

                31, 67, 67, 67, 67, 67, 67, 67, 67, 67, 67, 67      不同值 2 / 12

            也就是说「随机延迟」在循环里其实是<b>每个时钟节拍内的一个常数</b> ——
            这个功能的全部意义正好被它抵消掉了。

            ⚠️ 静态一份 + lock：「同时执行」模式下多个 RobotExecute 各跑一条线程，
            <b>Random 不是线程安全的</b>，无锁并发调用会把它的内部状态搅坏（表现是一直返回 0）。
            延迟指令一秒最多几十次，锁的代价可以忽略。
        */
        private static readonly Random rndDelay = new Random();

        /*
            ⚠️ <b>区间要就地摆正，不能直接喂给 Next。</b>
            Random.Next(from, to + 1) 在 from > to 时抛 ArgumentOutOfRangeException，
            而 Robot_DoWork 现在<b>不吞异常</b>了 —— 整个机器人会从这条指令起中断。

            ValidateInstruction 拦得住新界面插进来的（要求 b >= a），但拦不住
            <b>WinForms 那条插入路径、老库、备份导入</b>进来的（实测能一路走到执行端）。
            这里摆正 + 记一条节流日志：既不中断，也不装作没发生过。
        */
        private static int NextRandom(int from, int to)
        {
            if (from < 0) { from = 0; }
            if (to < 0) { to = 0; }

            if (from > to)
            {
                Operate.SystemConfig.LogThrottled(nameof(NextRandom),
                    string.Format(AntdUI.Localization.Get("System.Robot.RandomRange",
                        "随机延迟的区间是反的（{0}-{1}），已按 {1}-{0} 处理"), from, to));

                int t = from; from = to; to = t;
            }

            lock (rndDelay)
            {
                return rndDelay.Next(from, to + 1);
            }
        }

        #endregion

        #region//按键名 → 虚拟键

        /*
            组合按键的内容串是 SystemConfig.ConvertHotkeyToString 拼出来的，形如
            "Ctrl + Shift + A"、"Alt + 1"、"Ctrl + NumPad5"：修饰键写的是 Ctrl / Alt / Shift
            （不是 Keys 枚举里的 ControlKey / Menu / ShiftKey），数字写的是 0–9（不是 D0–D9）。

            原来这里直接 Enum.TryParse<Keys> 再按数字塞进 VirtualKeyCode：
            "Ctrl" 解析失败被丢掉；"Alt" / "Shift" 解析成修饰<b>位</b>（262144 / 65536），
            而数字串解析枚举<b>不检查该值是否定义</b>，于是被当成普通键按下去；"1" 解析成 Keys.LButton。
            三条合起来就是：组合按键从来没按对过。这里按内容串的实际写法解。
        */
        private static bool TryGetVirtualKey(string sKey, out VirtualKeyCode vkCode)
        {
            vkCode = default(VirtualKeyCode);

            string key = (sKey ?? string.Empty).Trim();
            if (key.Length == 0) { return false; }

            switch (key.ToLowerInvariant())
            {
                case "ctrl":
                case "control":
                case "controlkey":
                    vkCode = VirtualKeyCode.CONTROL;
                    return true;

                case "alt":
                case "menu":
                    vkCode = VirtualKeyCode.MENU;
                    return true;

                case "shift":
                case "shiftkey":
                    vkCode = VirtualKeyCode.SHIFT;
                    return true;
            }

            if (key.Length == 1)
            {
                char c = char.ToUpperInvariant(key[0]);
                if (c >= '0' && c <= '9') { vkCode = (VirtualKeyCode)(0x30 + (c - '0')); return true; }
                if (c >= 'A' && c <= 'Z') { vkCode = (VirtualKeyCode)(0x41 + (c - 'A')); return true; }
            }

            Keys kCode;
            if (Enum.TryParse(key, true, out kCode))
            {
                int code = (int)(kCode & Keys.KeyCode);
                if (code > 0 && Enum.IsDefined(typeof(VirtualKeyCode), code))
                {
                    vkCode = (VirtualKeyCode)code;
                    return true;
                }
            }

            return false;
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
                Operate.DoLog(nameof(GetParameter), ex);
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
                Operate.DoLog(nameof(GetParameter), ex);
            }
            
            return defaultValue;
        }

        #endregion
    }
}
