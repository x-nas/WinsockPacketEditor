using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
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

        /*
            ⚠️ 2026-09-11：从 BackgroundWorker 换成 Task（BW → Task 第二步），与 SendExecute 逐条同构。
              IsBusy         → Running
              ReportProgress → Progressed 事件（执行轨迹的订阅者用它）+ 直接写 Instruction_Index
              Completed      → Robot_OnDone continuation（记日志）+ Completed 事件（editResult 用它）
            取消仍是唯一真源的 cts（第一步已在）；body 里 token.ThrowIfCancellationRequested()。
        */
        private CancellationTokenSource cts;
        private Task task;
        private readonly WindowsInput.InputSimulator sim = new WindowsInput.InputSimulator();

        /// <summary>正在跑没有（与 BW 的 IsBusy 同窗口：到 Robot_OnDone 跑完为止）。</summary>
        public bool Running
        {
            get { Task t = this.task; return t != null && !t.IsCompleted; }
        }

        /// <summary>走到第几条指令（下标）。机器人编辑的执行轨迹订阅它。</summary>
        public event Action<int> Progressed;

        /// <summary>跑完了：参数是 "done" / "stopped" / "error:消息"。机器人编辑用它收 editResult。</summary>
        public event Action<string> Completed;

        #region//启动机器人

        public void StartRobot(RobotInfo ri, Dictionary<string, object> parameters)
        {
            try
            {
                if (ri != null && ri.RInstruction.Count > 0)
                {
                    if (!this.Running)
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
                            string sLog = string.Format(UI.T("System.Robot.Error", "机器人指令 {0} 错误! [{1}]"), iReturn + 1, this.RobotName);
                            Operate.DoLog(nameof(StartRobot), sLog);
                        }
                        else
                        {
                            CancellationToken token = this.cts.Token;
                            this.task = Task.Run(() => Robot_Body(token), token)
                                .ContinueWith(Robot_OnDone, TaskScheduler.Default);

                            string sLog = string.Format(UI.T("System.Robot.Start", "启动机器人 [{0}]"), this.RobotName);
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
                //只 Cancel token（取消只能有一个真源）
                CancellationTokenSource c = this.cts;
                if (c != null) { c.Cancel(); }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(StopRobot), ex);
            }
        }

        #endregion

        #region//执行指令集

        /*
            ⚠️ <b>这里不加 try/catch。</b> 加了的话异常被吞掉、task.Exception 恒为 null，
            Robot_OnDone 里「error:」那一支就成了死代码 ——
            机器人炸在半路，界面弹的却是绿色的「执行完毕」。

            取消走 token.ThrowIfCancellationRequested()：task.IsCanceled 变 true，
            Robot_OnDone 据此报「已停止」、Completed 事件带回 "stopped"。
        */
        private void Robot_Body(CancellationToken token)
        {
            {
                if (this.RInstruction.Count > 0)
                {
                    Stack<int> sLoopStart = new Stack<int>();
                    Dictionary<int, int> dLoopCNT = new Dictionary<int, int>();

                    for (int i = 0; i < this.RInstruction.Count; i++)
                    {
                        token.ThrowIfCancellationRequested();

                        {
                            //进度：轮询读 Instruction_Index，执行轨迹订阅 Progressed（200ms 一拍会漏掉飞快的那几步）
                            this.Instruction_Index = i;
                            Progressed?.Invoke(i);

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
                                            while (ss.Running)
                                            {
                                                if (token.WaitHandle.WaitOne(100))
                                                {
                                                    ss.StopSend();
                                                    token.ThrowIfCancellationRequested();
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
                                        Operate.SystemConfig.LogThrottled(nameof(Robot_Body),
                                            string.Format(UI.T("System.Robot.LoopCount",
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

                            /*
                                ⚠️ 处理完这条指令再查一次取消（顶上那次只挡「还没开始下一条」）。

                                延迟指令的 DoSleep 被取消时是<b>提前返回</b>、不抛 —— 如果它正好是
                                最后一条，循环随即自然结束，body 正常返回、报「执行完毕」，而用户明明按了停止。
                                在这里补一句，让「取消发生在最后一条延迟里」也如实报「已停止」。
                                （SendSendList 那一支自己已经 ThrowIfCancellationRequested 了，不重复。）
                            */
                            token.ThrowIfCancellationRequested();

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

        #region//执行完毕

        private void Robot_OnDone(Task t)
        {
            try
            {
                string result;
                string sLog;

                if (t.IsCanceled)
                {
                    result = "stopped";
                    sLog = string.Format(UI.T("Robot.Stop", "机器人 [{0}] 已停止"), this.RobotName);
                }
                else if (t.Exception != null)
                {
                    Exception ex = t.Exception.InnerException ?? t.Exception;
                    result = "error:" + ex.Message;
                    sLog = string.Format(UI.T("Robot.Error", "机器人 [{0}] 发生错误: {1}"), this.RobotName, ex.Message);
                }
                else
                {
                    result = "done";
                    sLog = string.Format(UI.T("Robot.Success", "机器人 [{0}] 执行完毕"), this.RobotName);
                }

                Operate.DoLog(nameof(Robot_OnDone), sLog);

                //机器人编辑要拿这个结果收 editResult；列表执行没有订阅者，Invoke 空转
                Completed?.Invoke(result);
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(Robot_OnDone), ex);
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
                    string.Format(UI.T("System.Robot.RandomRange",
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
