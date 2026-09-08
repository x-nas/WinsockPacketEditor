using System;

namespace WinsockPacketEditor
{
    #region//列表行 DTO

    /*
        B9b 引入。这些是 IUiFeed 推送的行对象，也是将来过桥时的 JSON 契约。

        【统一的映射规则】
          DateTime            -> string   在 C# 侧格式化好，前端不做时间处理
                                          列表用 "HH:mm:ss:fffffff"，与迁移前显示一致
          Guid                -> string   大写无括号，与现有 .ToString().ToUpper() 一致
          枚举                -> int      前端按 int 分支，不依赖 C# 枚举名
          byte[]              -> 排除     字节流不进推送流，按 Id 单独取
                                          （见 Operate.PacketConfig.List.GetPacketBufferById）
          Image               -> 排除     图标由 UI 层按路径自己生成（UiImages.GetProcessIcon）
          嵌套 BindingList<T> -> 排除     改为计数字段，明细按需单独取

        【为什么带上全部标量字段】
        DTO 是两个实现（WinForms 与将来的桥）之间的契约，加字段要改两处、删字段只改一处。
        所以这里取模型的标量字段全集；B9e 接线时若某列用不上，删掉即可。
    */

    #region//高频列表

    /// <summary>封包列表的一行。<b>不含字节流</b>，需要完整字节时按 Id 调 GetPacketBufferById / GetRawBufferById。</summary>
    public sealed class PacketRow
    {
        public long Id;
        public string Time;          // HH:mm:ss:fffffff
        public int Socket;
        public int Type;             // Operate.PacketConfig.Packet.PacketType
        public string From;
        public string FromLocation;
        public string To;
        public string ToLocation;
        public int Len;
        public string Preview;       // 截断的十六进制预览，复用 PacketInfo.PacketData
        public int Action;           // Operate.FilterConfig.Filter.FilterAction，界面据此上色

        public static PacketRow From_(PacketInfo Src)
        {
            if (Src == null) { return null; }

            return new PacketRow
            {
                Id = Src.Id,
                Time = Src.PacketTime.ToString("HH:mm:ss:fffffff"),
                Socket = Src.PacketSocket,
                Type = (int)Src.PacketType,
                From = Src.PacketFrom,
                FromLocation = Src.FromLocation,
                To = Src.PacketTo,
                ToLocation = Src.ToLocation,
                Len = Src.PacketLen,
                Preview = Src.PacketData,
                Action = (int)Src.FilterAction,
            };
        }
    }

    /// <summary>代理数据列表的一行。同样不含字节流。</summary>
    public sealed class ProxyRow
    {
        public long Id;
        public string Time;
        public int Socket;
        public long TheologyID;
        public int Type;
        public long WebSocketType;
        public string ClientAddr;
        public string ClientLocation;
        public string ServerAddr;
        public string ServerLocation;
        public string ServerDomain;
        public int DomainType;
        public int Len;
        public string Preview;
        public int Action;

        public static ProxyRow From_(ProxyInfo Src)
        {
            if (Src == null) { return null; }

            return new ProxyRow
            {
                Id = Src.Id,
                Time = Src.ProxyTime.ToString("HH:mm:ss:fffffff"),
                Socket = Src.PacketSocket,
                TheologyID = Src.TheologyID,
                Type = (int)Src.PacketType,
                WebSocketType = Src.WebSocketType,
                ClientAddr = Src.ClientAddr,
                ClientLocation = Src.ClientLocation,
                ServerAddr = Src.ServerAddr,
                ServerLocation = Src.ServerLocation,
                ServerDomain = Src.ServerDomain,
                DomainType = (int)Src.DomainType,
                Len = Src.PacketLen,
                Preview = Src.PacketData,
                Action = (int)Src.FilterAction,
            };
        }
    }

    /// <summary>系统日志的一行。</summary>
    public sealed class LogRow
    {
        public string Time;
        public string FuncName;
        public string Content;

        public static LogRow From_(LogInfo Src)
        {
            if (Src == null) { return null; }

            return new LogRow
            {
                //日志到毫秒够了。封包那两份仍用 7 位小数 —— 那是用来分辨同一毫秒内的收发顺序的
                Time = Src.LogTime.ToString("HH:mm:ss.fff"),
                FuncName = Src.FuncName,
                Content = Src.LogContent,
            };
        }
    }

    /// <summary>滤镜日志的一行。</summary>
    public sealed class FilterLogRow
    {
        public string Time;
        public string FilterName;
        public int Action;
        public int MatchNum;
        public int Type;
        public int Len;

        public static FilterLogRow From_(FilterLogInfo Src)
        {
            if (Src == null) { return null; }

            return new FilterLogRow
            {
                Time = Src.LogTime.ToString("HH:mm:ss.fff"),
                FilterName = Src.FName,
                Action = (int)Src.FAction,
                MatchNum = Src.MatchNum,
                Type = (int)Src.PacketType,
                Len = Src.PacketLen,
            };
        }
    }

    /// <summary>代理日志的一行。</summary>
    public sealed class ProxyLogRow
    {
        public string Time;
        public string UserName;
        public string LoginIP;
        public string Content;

        public static ProxyLogRow From_(ProxyLogInfo Src)
        {
            if (Src == null) { return null; }

            return new ProxyLogRow
            {
                Time = Src.LogTime.ToString("HH:mm:ss.fff"),
                UserName = Src.UserName,
                LoginIP = Src.LoginIP,
                Content = Src.LogContent,
            };
        }
    }

    #endregion

    #region//中频列表

    /// <summary>代理账号的一行。密码不下发，需要时走单独的解密接口。</summary>
    public sealed class AccountRow
    {
        public string Id;            // AID
        public bool IsCheck;
        public bool IsEnable;
        public string UserName;
        public bool IsLimitLinks;
        public int LimitLinks;
        public bool IsLimitDevices;
        public int LimitDevices;
        public bool IsExpiry;
        public string ExpiryTime;    // yyyy-MM-dd HH:mm:ss
        public string CreateTime;
        public bool IsOnLine;
        public int LoginCount;       // AIPInfo 的条数，明细单独取

        public static AccountRow From_(AccountInfo Src)
        {
            if (Src == null) { return null; }

            return new AccountRow
            {
                Id = Src.AID.ToString().ToUpper(),
                IsCheck = Src.IsCheck,
                IsEnable = Src.IsEnable,
                UserName = Src.UserName,
                IsLimitLinks = Src.IsLimitLinks,
                LimitLinks = Src.LimitLinks,
                IsLimitDevices = Src.IsLimitDevices,
                LimitDevices = Src.LimitDevices,
                IsExpiry = Src.IsExpiry,
                ExpiryTime = Src.ExpiryTime.ToString("yyyy-MM-dd HH:mm:ss"),
                CreateTime = Src.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
                IsOnLine = Src.IsOnLine,
                LoginCount = Src.AIPInfo == null ? 0 : Src.AIPInfo.Count,
            };
        }
    }

    /// <summary>
    /// 账号的一条登录记录（AccountInfo.AIPInfo 里的 AccountIPInfo）。
    /// 嵌套列表不进推送流（AccountRow 只留 LoginCount），要明细时按账号 Id 单独取。
    /// </summary>
    public sealed class AccountLoginRow
    {
        public string LoginTime;
        public string LoginIP;
        public string IPLocation;

        public static AccountLoginRow From_(AccountIPInfo Src)
        {
            if (Src == null) { return null; }

            return new AccountLoginRow
            {
                LoginTime = Src.LoginTime.ToString("yyyy-MM-dd HH:mm:ss"),
                LoginIP = Src.LoginIP,
                IPLocation = Src.IPLocation,
            };
        }
    }

    /// <summary>
    /// 批量创建账号时的一条草稿（对应 WinForms 的 BatchAccounts 预览表）。
    /// <b>不是</b>推送行 —— 它还没落库，密码也是明文：预览要显示、导出的表格要给使用者，
    /// 加密串对谁都没用。落库那一步才 PassWord_Encrypt。
    /// </summary>
    public sealed class BatchAccountRow
    {
        public string UserName;
        public string Password;
    }

    /// <summary>
    /// 滤镜格子里的一列（对应 Controls/FilterEdit 那张表的一列）。
    ///
    /// 【滤镜的本质就是这张表】一行行十六进制字节，列号 = 字节位置：
    ///   Search 非空 → 这一位要匹配这个值；空 → 这一位不参与匹配
    ///   Modify 非空 → 命中后把这一位改成这个值
    ///
    /// 【三个标记在 WinForms 里是<b>格子底色</b>】紫 = 排除、暗红 = 递进、蓝 = 随机
    /// （UiTheme.FilterExclude_Color / FilterProgression_Color / FilterRandom_Color）。
    /// 底色是绘制细节，不该过桥，所以这里拆成三个 bool ——
    /// 前端爱画成什么样是前端的事，但语义得是同一套。
    ///
    /// Exclude 属于查找位，Progression / Random 属于修改位，三者在源模型里
    /// 存成三串独立的位置列表（ExcludePosition / ProgressionPosition / RandomPosition）。
    /// </summary>
    public sealed class FilterSearchCell
    {
        public int Index;
        public string Value;
        public bool Exclude;
    }

    /// <summary>
    /// 修改位的一列。
    ///
    /// 【⚠️ Index 的含义随 FStartFrom 变】
    ///   Head（起始于包头）    绝对位置，0 ~ 999
    ///   Position（指定位置）  相对匹配点的<b>偏移，可以是负数</b>，-1000 ~ 999
    /// 后者在 WinForms 里是一张 2000 列、列头带符号的表（FilterEdit 建
    /// dtFilterAdvanced_Modify_Position 时是 <c>for (i = -iSize; i &lt; iSize; i++)</c>），
    /// 保存时取的是<b>列名</b>而不是循环下标，所以存进库里的就是那个带符号的偏移。
    /// 任何地方写 <c>Index &gt;= 0</c> 的判断都会把左半边一千列悄悄丢掉。
    /// </summary>
    public sealed class FilterModifyCell
    {
        public int Index;
        public string Value;
        public bool Progression;
        public bool Random;
    }

    /// <summary>
    /// 滤镜编辑弹窗要的全部数据（对应 Controls/FilterEdit）。
    ///
    /// 比列表用的 <see cref="FilterRow"/> 多出格子内容与递进参数 ——
    /// 那些只有编辑时才用得上，几百字节乘整表推送毫无必要，所以按 Id 单独取。
    ///
    /// 【格子是解析好的，不是原始串】源模型把它存成
    /// <c>FSearch = "0|AB,3|CD,"</c> 这样的串，位置标记另外三串。
    /// 那个格式只该有一处知道 —— 解析与拼装都在 Operate 侧，
    /// 前端拿到的是一个干净的数组，省得两边各写一份解析、迟早走岔。
    /// </summary>
    /*
        滤镜「执行」下拉的候选项。不进推送流，按类型现取。

        只出名称与 Id：四份列表的模型（SendInfo / RobotInfo / FilterInfo /
        WareHouseInfo）都（当年）继承 AntdUI.NotifyProperty，出现在外壳能看到的签名上就是 CS0012。
    */
    public sealed class ExecuteTargetRow
    {
        public string Id;
        public string Name;
    }

    public sealed class FilterEditRow
    {
        public string Id;

        /*
            这里<b>没有 IsEnable</b>：启用与否由滤镜列表上的开关管，
            编辑弹窗不碰它。SaveFilterEdit 本来也从不回写这个字段，
            留一个只出不进的字段会让人以为弹窗能改。
        */
        public string Name;

        public int Mode;             // FilterMode：0 普通 1 高级
        public int Action;           // FilterAction
        public int StartFrom;        // FilterStartFrom：0 包头 1 指定位置
        public int FunctionMask;     // 12 个封包类型，位序见 FilterRow.FunctionMask

        public bool AppointHeader;
        public string HeaderContent;
        public bool AppointSocket;
        public string SocketContent;
        public bool AppointLength;
        public string LengthContent;
        public bool AppointPort;
        public string PortContent;

        public bool IsExecute;
        public int ExecuteType;      // FilterExecuteType
        public string ExecuteId;     // Execute_GUID

        public bool IsProgressionContinuous;
        public int ProgressionStep;
        public bool IsProgressionCarry;
        public int ProgressionCarryNumber;

        /*
            查找与修改是<b>两套独立的索引</b>（源模型里就是 FSearch / FModify 两个串）。
            普通模式下两者恰好对齐同一列，看不出区别；
            高级模式下它们各自独立，修改位在「指定位置」时索引还可能是负的。
            合成一个数组会在高级模式下错，这里分开。

            只放有内容或有标记的列 —— 一千（乃至两千）列全下发没有意义。
        */
        public FilterSearchCell[] Search;
        public FilterModifyCell[] Modify;
    }

    /// <summary>客户端认证记录的一行。</summary>
    /*
        一条客户端连接 —— 对应 WinForms 客户端列表那棵树的<b>叶子</b>。

        不进推送流：它是从 ProxyServer 的活动会话现算的，没有 BindingList 可挂，
        由界面按需拉（getClientConnections）。

        <b>必须出 DTO 而不是 ProxySession。</b>那个类继承 SuperSocket 的 AppSession，
        一出现在外壳能看到的签名上就是 CS0012 —— 与 QQWry / AntdUI 那几次同一个坑。
    */
    public sealed class ClientConnRow
    {
        /// <summary>客户端自己的 IP（会话的 RemoteEndPoint）。</summary>
        public string ClientIP;

        /// <summary>该 IP 上这条会话用的源端口。</summary>
        public int ClientPort;

        /// <summary>连去哪儿：GetClientAddress 拼的 "目标:端口"。</summary>
        public string Target;

        /// <summary>DomainType：0 Socket · 1 HTTP · 2 HTTPS · 3 External · 4 WebSocket。</summary>
        public int DomainType;

        public string ServerAddress;
    }

    public sealed class AuthRow
    {
        public string AccountId;

        /*
            账号名。<b>在这里查好，不让界面自己去关联账号表。</b>

            WinForms 那边是列的 Render 里现调 GetUserName_ByAccountID —— 它跟账号表在
            同一个进程里，随手就取到了。桥这边不行：前端要拿 AccountId 去
            FeedList.Account 的副本里查，就多了一个时序前提（那份表得先推过来），
            而认证是客户端一连上就发生的，很可能早于账号表那一拍。
        */
        public string UserName;

        public string AuthIP;
        public string IPLocation;
        public int LinksNumber;
        public int DevicesNumber;
        public long TrafficStatistics;
        public bool AuthResult;
        public string AuthTime;

        public static AuthRow From_(AuthInfo Src)
        {
            if (Src == null) { return null; }

            return new AuthRow
            {
                AccountId = Src.AID.ToString().ToUpper(),
                UserName = Operate.ProxyConfig.Account.GetUserName_ByAccountID(Src.AID),
                AuthIP = Src.AuthIP,
                IPLocation = Src.IPLocation,
                LinksNumber = Src.LinksNumber,
                DevicesNumber = Src.DevicesNumber,
                TrafficStatistics = Src.TrafficStatistics,
                AuthResult = Src.AuthResult,
                AuthTime = Src.AuthTime.ToString("yyyy-MM-dd HH:mm:ss"),
            };
        }
    }

    /// <summary>进程的一行。<b>不含图标</b>，界面按 ProcessPath 自己生成（UiImages.GetProcessIcon）。</summary>
    public sealed class ProcessRow
    {
        public bool IsCheck;
        public string ProcessName;
        public int ProcessID;
        public string ModuleName;
        public string ProcessPath;

        public static ProcessRow From_(ProcessInfo Src)
        {
            if (Src == null) { return null; }

            return new ProcessRow
            {
                IsCheck = Src.IsCheck,
                ProcessName = Src.ProcessName,
                ProcessID = Src.ProcessID,
                ModuleName = Src.ModuleName,
                ProcessPath = Src.ProcessPath,
            };
        }
    }

    #endregion

    #region//低频列表

    /// <summary>滤镜的一行。搜索/修改内容是长文本，列表页只带摘要标志，明细在编辑器里单独取。</summary>
    public sealed class FilterRow
    {
        public string Id;            // FID
        public bool IsEnable;
        public string Name;
        public long ExecutionCount;
        public int Mode;             // FilterMode
        public int Action;           // FilterAction
        public int StartFrom;        // FilterStartFrom

        /// <summary>
        /// 滤镜作用于哪些封包类型。
        ///
        /// 源模型的 FFunction 是个 12 个 bool 的 struct（不是枚举），这里压成位掩码，
        /// 位序照搬 Operate.FilterConfig.Filter.FilterFunction 里标注的编号：
        ///   0 Send  1 SendTo  2 Recv  3 RecvFrom
        ///   4 WSASend  5 WSASendTo  6 WSARecv  7 WSARecvFrom
        ///   8 TCP_Req  9 UDP_Req  10 TCP_Resp  11 UDP_Resp
        /// </summary>
        public int FunctionMask;
        public bool AppointHeader;
        public string HeaderContent;
        public bool AppointSocket;
        public string SocketContent;
        public bool AppointLength;
        public string LengthContent;
        public bool AppointPort;
        public string PortContent;
        public bool IsExecute;
        public int ExecuteType;      // FilterExecuteType
        public string ExecuteId;     // Execute_GUID

        /*
            递进那一列要的三个字段。

            B9 建这个 DTO 时漏了它们 —— 当时没有界面在用，直到滤镜列表才发现：
            那一列的三个标签分别看「ProgressionPosition 非空」「连续」「进位」。
            <b>「启用」标签看的是位置串非空，不是某个 bool</b>，这一点照抄
            Controls/FilterList.cs 的 Render，别自己发明一个 IsProgression。
        */
        public string ProgressionPosition;
        public bool IsProgressionContinuous;
        public bool IsProgressionCarry;

        public static FilterRow From_(FilterInfo Src)
        {
            if (Src == null) { return null; }

            return new FilterRow
            {
                Id = Src.FID.ToString().ToUpper(),
                IsEnable = Src.IsEnable,
                Name = Src.FName,
                ExecutionCount = Src.ExecutionCount,
                Mode = (int)Src.FMode,
                Action = (int)Src.FAction,
                StartFrom = (int)Src.FStartFrom,
                FunctionMask = ToMask(Src.FFunction),
                AppointHeader = Src.AppointHeader,
                HeaderContent = Src.HeaderContent,
                AppointSocket = Src.AppointSocket,
                SocketContent = Src.SocketContent,
                AppointLength = Src.AppointLength,
                LengthContent = Src.LengthContent,
                AppointPort = Src.AppointPort,
                PortContent = Src.PortContent,
                IsExecute = Src.IsExecute,
                ExecuteType = (int)Src.FEType,
                ExecuteId = Src.Execute_GUID.ToString().ToUpper(),
                ProgressionPosition = Src.ProgressionPosition,
                IsProgressionContinuous = Src.IsProgressionContinuous,
                IsProgressionCarry = Src.IsProgressionCarry,
            };
        }

        /// <summary>把 12 个 bool 压成位掩码，位序见 <see cref="FunctionMask"/> 的说明。</summary>
        private static int ToMask(Operate.FilterConfig.Filter.FilterFunction F)
        {
            int m = 0;

            if (F.Send) { m |= 1 << 0; }
            if (F.SendTo) { m |= 1 << 1; }
            if (F.Recv) { m |= 1 << 2; }
            if (F.RecvFrom) { m |= 1 << 3; }
            if (F.WSASend) { m |= 1 << 4; }
            if (F.WSASendTo) { m |= 1 << 5; }
            if (F.WSARecv) { m |= 1 << 6; }
            if (F.WSARecvFrom) { m |= 1 << 7; }
            if (F.TCP_Req) { m |= 1 << 8; }
            if (F.UDP_Req) { m |= 1 << 9; }
            if (F.TCP_Resp) { m |= 1 << 10; }
            if (F.UDP_Resp) { m |= 1 << 11; }

            return m;
        }
    }

    /// <summary>发送列表的一行。封包集合不下发，只给条数。</summary>
    /*
        发送编辑（Controls/SendEdit）用的三个 DTO。<b>都不进推送流</b> ——
        编辑弹窗是「打开时取一次、动一下重取一次」，没有整表定时推的必要。
    */

    /// <summary>
    /// 封包编辑打开时的一次性快照。Id 为空表示没找到。
    /// 字节直接带上 —— 这一屏就是来改它的，不像列表那样按需取。
    /// </summary>
    public sealed class PacketEditRow
    {
        public string Id = string.Empty;
        /// <summary>来源："proxy" = 代理数据列表；"send" = 发送编辑当前打开的那份发送集。</summary>
        public string List = string.Empty;
        public int Socket;
        public int Type;                        // Operate.PacketConfig.Packet.PacketType 的序号
        public string From = string.Empty;
        public string To = string.Empty;
        public byte[] Buffer = new byte[0];
        /// <summary>套接字填 0 时能不能走 SunnyNet 的会话发送 —— 只有 HTTP / HTTPS / WebSocket 那条路抓到的包有会话号。</summary>
        public bool CanSendBySession;
        public int SystemSocket;
    }

    /// <summary>发送编辑打开时的一次性快照。Id 为空表示没找到那条发送。</summary>
    public sealed class SendEditRow
    {
        public string Id = string.Empty;
        public string Name = string.Empty;
        public bool UseSystemSocket;
        public int LoopCount;
        public int LoopInterval;
        public string Notes = string.Empty;

        /// <summary>全局系统套接字号。界面上要显示「用系统套接字」到底是哪个号。</summary>
        public int SystemSocket;
    }

    /// <summary>发送集里的一条封包。字节不进来，按 Id 单独取。</summary>
    public sealed class SendPacketRow
    {
        /// <summary>PacketInfo.Id，运行期自增、<b>不持久化</b>，只在本次会话内唯一。</summary>
        public string Id = string.Empty;

        public int Socket;
        public int Type;            // Operate.PacketConfig.Packet.PacketType 的序号
        public string From = string.Empty;
        public string To = string.Empty;
        public int Len;
        public string Preview = string.Empty;   // PacketData

        public static SendPacketRow From_(PacketInfo Src)
        {
            if (Src == null) { return null; }

            return new SendPacketRow
            {
                Id = Src.Id.ToString(),
                Socket = Src.PacketSocket,
                Type = (int)Src.PacketType,
                From = Src.PacketFrom,
                To = Src.PacketTo,
                Len = Src.PacketLen,
                Preview = Src.PacketData,
            };
        }
    }

    /// <summary>发送编辑里「执行」的进度。前端在跑的时候轮询。</summary>
    public sealed class SendProgressRow
    {
        public bool Running;

        /// <summary>当前发到第几条（下标）。间隔为 0 时不会动，见 GetSendEditProgress 的说明。</summary>
        public int Index;

        public int Total;
        public int Success;
        public int Fail;
    }

    public sealed class SendRow
    {
        public string Id;            // SID
        public bool IsEnable;
        public string Name;
        public long ExecutionCount;
        public long ExecutionSuccess;
        public long ExecutionFail;
        public bool UseSystemSocket;
        public int LoopCount;
        public int LoopInterval;
        public string Notes;
        public int PacketCount;      // SCollection 的条数

        public static SendRow From_(SendInfo Src)
        {
            if (Src == null) { return null; }

            return new SendRow
            {
                Id = Src.SID.ToString().ToUpper(),
                IsEnable = Src.IsEnable,
                Name = Src.SName,
                ExecutionCount = Src.ExecutionCount,
                ExecutionSuccess = Src.ExecutionSuccess,
                ExecutionFail = Src.ExecutionFail,
                UseSystemSocket = Src.SSystemSocket,
                LoopCount = Src.SLoopCNT,
                LoopInterval = Src.SLoopINT,
                Notes = Src.SNotes,
                PacketCount = Src.SCollection == null ? 0 : Src.SCollection.Count,
            };
        }
    }

    /// <summary>机器人的一行。指令集不下发，只给条数。</summary>
    public sealed class RobotRow
    {
        public string Id;            // RID
        public bool IsEnable;
        public string Name;
        public long ExecutionCount;
        public int InstructionCount;

        public static RobotRow From_(RobotInfo Src)
        {
            if (Src == null) { return null; }

            return new RobotRow
            {
                Id = Src.RID.ToString().ToUpper(),
                IsEnable = Src.IsEnable,
                Name = Src.RName,
                ExecutionCount = Src.ExecutionCount,
                InstructionCount = Src.RInstruction == null ? 0 : Src.RInstruction.Count,
            };
        }
    }

    /// <summary>机器人编辑打开时的一次性快照。Id 为空表示没找到那条机器人。</summary>
    public sealed class RobotEditRow
    {
        public string Id = string.Empty;
        public string Name = string.Empty;
    }

    /// <summary>
    /// 指令集里的一条。InstructionInfo 没有主键，按<b>下标</b>收发 ——
    /// 工作副本只在编辑弹窗里活着，每次改动后整表重取，下标不会错位。
    /// </summary>
    public sealed class InstructionRow
    {
        public int Index;
        public int Type;                          // Operate.RobotConfig.Robot.InstructionType 的序号
        public string TypeName = string.Empty;    // 已本地化：发送 / 设置 / 延迟 …（GetName_ByInstructionType）
        public string Text = string.Empty;        // 已本地化的内容描述（GetContentString_ByInstructionType）
        public string Content = string.Empty;     // 原始内容串「类型|参数」
    }

    /// <summary>机器人编辑里「执行」的进度，前端按 200ms 轮询。</summary>
    public sealed class RobotEditProgress
    {
        public bool Running;
        public int Index = -1;                    // 正在执行的指令下标（Worker.ReportProgress）
        public int Total;                         // 已执行的指令条数（不含循环开始 / 结束）
        public string Trail = string.Empty;       // 执行轨迹 "1, 2, 3, 2, 3"，对应 WinForms 的 txtINSTLog
        public string Result = string.Empty;      // 跑完后：done / stopped / error:<msg>；在跑或没跑过为空
    }

    /// <summary>ProxyCap 服务器下面的一条规则（ServerInfo.ServerRInfo 里的嵌套列表，按服务器 Id 单独取）。</summary>
    public sealed class RuleRow
    {
        public string Id;            // RID
        public bool IsEnable;
        public int Type;             // RuleType
        public string TypeName = string.Empty;   // GetRuleTypeDescription：DOMAIN-SUFFIX 这种带横线的写法
        public string Argument = string.Empty;
        public int Action;           // RuleAction：0 PROXY · 1 REJECT · 2 DIRECT
    }

    /// <summary>规则类型的选项（值 + 显示名）。</summary>
    public sealed class RuleTypeRow
    {
        public int Value;
        public string Name = string.Empty;
    }

    #region//设置页 DTO（WPEHybrid 的六个设置弹窗）

    public sealed class ProcessSettingRow
    {
        public int DriverType;          // 0 Proxifier · 1 NFAPI · 2 WinDivert
        public bool IsLoadDriver;       // 驱动已加载后就不能再换
        public bool MustTCP;
        public string IP = string.Empty;
        public int Port;
        public bool AppointPort;
        public string AppointPortContent = string.Empty;
        public bool Auth;
        public string UserName = string.Empty;
        public string PassWord = string.Empty;
        public int[] CheckedPids = new int[0];
    }

    public sealed class ExtProxySettingRow
    {
        public bool Enable;
        public string IP = string.Empty;
        public int Port;
        public bool AppointPort;
        public string AppointPortContent = string.Empty;
        public bool Auth;
        public string UserName = string.Empty;
        public string PassWord = string.Empty;
    }

    public sealed class RemoteSettingRow
    {
        public bool IsRemote;
        public string IP = string.Empty;
        public string[] IPs = new string[0];
        public int Port;
        public string UserName = string.Empty;
        public string PassWord = string.Empty;
        public bool Running;
    }

    public sealed class HotkeySettingRow
    {
        public int Type;                // 0 发送列表 · 1 机器人列表
        public string[] Keys = new string[12];
    }

    public sealed class FilterStatsRow
    {
        public long ProxyTotal;
        public long Execute;
        public long Replace;
        public long Change;
        public long Intercept;
        public long Display;
        public long NoDisplay;
    }

    #endregion

    /// <summary>仓库的一行。仓储明细不下发，只给条数。</summary>
    public sealed class WareHouseRow
    {
        public string Id;            // WID
        public string Name;
        public int DataCount;

        public static WareHouseRow From_(WareHouseInfo Src)
        {
            if (Src == null) { return null; }

            return new WareHouseRow
            {
                Id = Src.WID.ToString().ToUpper(),
                Name = Src.WName,
                DataCount = Src.Stores == null ? 0 : Src.Stores.Count,
            };
        }
    }

    /// <summary>
    /// 仓库编辑器里仓储数据的一行（按需取，<b>不进推送流</b>）。
    /// 字节不带 —— 与封包列表同一个理由，选中某行时再按 Id 单独取（GetStoreBuffer_ById）。
    /// Id 是 <see cref="DataInfo.DID"/>，运行期分配。
    /// </summary>
    public sealed class StoreRow
    {
        public string Id = string.Empty;
        public int Len;
        public string Preview = string.Empty;

        /// <param name="WithPreview">
        /// 要不要带预览串。<b>整表出行时传 false</b> —— 60 字节十六进制约 180 个字符，
        /// 占整条报文的四分之三（50000 条实测：带 12.6 MB、不带 3.4 MB）。
        /// 预览按可见窗口另取，见 <c>GetStorePreviews_ById</c>。
        /// </param>
        public static StoreRow From_(DataInfo Src, bool WithPreview = true)
        {
            if (Src == null) { return null; }

            byte[] buf = Src.PacketBuffer ?? new byte[0];

            return new StoreRow
            {
                Id = Src.DID.ToString().ToUpper(),
                Len = buf.Length,
                Preview = WithPreview
                    ? Operate.PacketConfig.Packet.GetPacketData_Hex(buf, Operate.PacketConfig.Packet.PacketData_MaxLen)
                    : string.Empty,
            };
        }
    }

    /// <summary>
    /// 自动入库规则的一行。
    /// Id 是 <see cref="AutoStoresInfo.AID"/>（运行期分配、不落库）—— 这个模型没有自然主键，
    /// 桥按 Id 收发才对得上行，见那个属性上的说明。
    /// </summary>
    public sealed class AutoStoresRow
    {
        public string Id;            // AID，运行期
        public bool IsEnable;
        public string PacketHead;
        public string WareHouseId;

        public static AutoStoresRow From_(AutoStoresInfo Src)
        {
            if (Src == null) { return null; }

            return new AutoStoresRow
            {
                Id = Src.AID.ToString().ToUpper(),
                IsEnable = Src.IsEnable,
                PacketHead = Src.PacketHead,
                WareHouseId = Src.WID.ToString().ToUpper(),
            };
        }
    }

    /// <summary>本地映射的一行。</summary>
    public sealed class MapLocalRow
    {
        public string Id;            // MapLocal.MID，运行期
        public bool IsEnable;
        public int Protocol;
        public string Host;
        public int Port;
        public string RemotePath;
        public string LocalPath;

        public static MapLocalRow From_(MapLocal Src)
        {
            if (Src == null) { return null; }

            return new MapLocalRow
            {
                Id = Src.MID.ToString().ToUpper(),
                IsEnable = Src.IsEnable,
                Protocol = (int)Src.ProtocolType,
                Host = Src.Host,
                Port = Src.Port,
                RemotePath = Src.RemotePath,
                LocalPath = Src.LocalPath,
            };
        }
    }

    /// <summary>远程映射的一行。</summary>
    public sealed class MapRemoteRow
    {
        public string Id;            // MapRemote.MID，运行期
        public bool IsCheck;
        public bool IsEnable;
        public int ProtocolFrom;
        public string HostFrom;
        public int PortFrom;
        public string PathFrom;
        public int ProtocolTo;
        public string HostTo;
        public int PortTo;
        public string PathTo;

        public static MapRemoteRow From_(MapRemote Src)
        {
            if (Src == null) { return null; }

            return new MapRemoteRow
            {
                Id = Src.MID.ToString().ToUpper(),
                IsCheck = Src.IsCheck,
                IsEnable = Src.IsEnable,
                ProtocolFrom = (int)Src.ProtocolTypeFrom,
                HostFrom = Src.HostFrom,
                PortFrom = Src.PortFrom,
                PathFrom = Src.PathFrom,
                ProtocolTo = (int)Src.ProtocolTypeTo,
                HostTo = Src.HostTo,
                PortTo = Src.PortTo,
                PathTo = Src.PathTo,
            };
        }
    }

    /// <summary>白名单 / 黑名单的一行（两者字段完全相同，共用一个 DTO）。</summary>
    public sealed class IPRuleRow
    {
        public string IPAddress;
        public long StartIP;
        public long EndIP;
        public string IPLocation;
        public long EffectCount;
        public bool IsExpiry;
        public string ExpiryTime;
        public string CreateTime;

        public static IPRuleRow From_(WhiteListInfo Src)
        {
            if (Src == null) { return null; }

            return new IPRuleRow
            {
                IPAddress = Src.IPAddress,
                StartIP = Src.StartIP,
                EndIP = Src.EndIP,
                IPLocation = Src.IPLocation,
                EffectCount = Src.EffectCount,
                IsExpiry = Src.IsExpiry,
                ExpiryTime = Src.ExpiryTime.ToString("yyyy-MM-dd HH:mm:ss"),
                CreateTime = Src.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
            };
        }

        public static IPRuleRow From_(BlackListInfo Src)
        {
            if (Src == null) { return null; }

            return new IPRuleRow
            {
                IPAddress = Src.IPAddress,
                StartIP = Src.StartIP,
                EndIP = Src.EndIP,
                IPLocation = Src.IPLocation,
                EffectCount = Src.EffectCount,
                IsExpiry = Src.IsExpiry,
                ExpiryTime = Src.ExpiryTime.ToString("yyyy-MM-dd HH:mm:ss"),
                CreateTime = Src.CreateTime.ToString("yyyy-MM-dd HH:mm:ss"),
            };
        }
    }

    /// <summary>ProxyCap 服务器的一行。规则集不下发，只给条数。</summary>
    public sealed class ServerRow
    {
        public string Id;            // SID
        public bool IsEnable;
        public string Name;
        public string IP;
        public int Port;
        public string ForgotURL;
        public string RegisterURL;
        public string VerifyURL;
        public int RuleCount;

        public static ServerRow From_(ServerInfo Src)
        {
            if (Src == null) { return null; }

            return new ServerRow
            {
                Id = Src.SID.ToString().ToUpper(),
                IsEnable = Src.IsEnable,
                Name = Src.ServerName,
                IP = Src.ServerIP,
                Port = Src.ServerPort,
                ForgotURL = Src.ForgotURL,
                RegisterURL = Src.RegisterURL,
                VerifyURL = Src.VerifyURL,
                RuleCount = Src.ServerRInfo == null ? 0 : Src.ServerRInfo.Count,
            };
        }
    }

    /// <summary>ProxyCap 公告的一行。</summary>
    public sealed class NoticeRow
    {
        public string Id;            // NID
        public int Type;
        public string Title;
        public string Content;
        public string More;
        public string Time;

        public static NoticeRow From_(NoticeInfo Src)
        {
            if (Src == null) { return null; }

            return new NoticeRow
            {
                Id = Src.NID.ToString().ToUpper(),
                Type = Src.NoticeType,
                Title = Src.NoticeTitle,
                Content = Src.NoticeContent,
                More = Src.NoticeMore,
                Time = Src.NoticeTime.ToString("yyyy-MM-dd HH:mm:ss"),
            };
        }
    }

    #endregion

    #region//查找封包的命中结果（不进推送流，按次取）

    /// <summary>
    /// 「查找封包」一次搜索的结果。对应 WinForms 的 <c>Controls/SearchPacket</c> +
    /// <c>ProxyList.bgwSearchProxyList</c> 那一套。
    ///
    /// <b>同时给出 Id 与 Index</b>：Id 用来选中那一行（前端副本按 Id 找），
    /// Index 是这一条在 C# 列表里的下标，「查找下一个」要从 Index + 1 接着扫。
    /// <b>Offset / Length</b> 是命中的那段字节在包里的位置，前端拿它直接在十六进制面板上圈出来
    /// —— WinForms 那边是让十六进制控件自己再找一次、找不到就跳下一条，多绕了一圈。
    /// </summary>
    public sealed class PacketSearchHit
    {
        public bool Found;
        public long Id;
        public int Index;
        public int Offset;      // 命中字节在包里的偏移；-1 表示只定位到行、圈不出具体位置
        public int Length;

        /// <summary>
        /// 「查找下一个」该从<b>这一行的哪个位置</b>接着找。前端原样存下、下次原样传回来即可，
        /// <b>不需要知道它是什么单位</b>（十六进制模式是十六进制文本的字符下标，文本模式是解码后的字符下标）。
        /// ⚠️ 有它才走得到<b>同一个封包里的第二处</b>；只按行号加一的话，一个包里命中三次也只看得到第一处。
        /// </summary>
        public int NextPos;

        public string Error;    // 正则写错时的说明，前端直接显示
    }

    #endregion

    #endregion
}
