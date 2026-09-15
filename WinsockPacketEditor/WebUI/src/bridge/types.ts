// 与 C# 侧的契约。改这里必须同步改对应的 C# 文件，否则只会在运行时静默错位。
//
//   FeedList / 行 DTO  ← WinsockPacketEditor/ClassObject/Ui/IUiFeed.cs + FeedRows.cs
//   PacketType         ← Operate.PacketConfig.Packet.PacketType
//   FilterAction       ← Operate.FilterConfig.Filter.FilterAction
//
// 枚举一律按 int 传，前端不依赖 C# 的枚举名。

/** 19 份列表。与 ClassObject/Ui/IUiFeed.cs 的 FeedList 顺序严格一致。 */
export const enum FeedList {
  Packet = 0,
  Proxy = 1,
  SystemLog = 2,
  FilterLog = 3,
  ProxyLog = 4,
  Account = 5,
  Auth = 6,
  SelectProcess = 7,
  Filter = 8,
  Send = 9,
  Robot = 10,
  WareHouse = 11,
  AutoStores = 12,
  MapLocal = 13,
  MapRemote = 14,
  WhiteList = 15,
  BlackList = 16,
  Server = 17,
  Notice = 18,
}

/**
 * 封包列表的一行。字段名是 C# PacketRow 的原名 —— 桥的 JSON 序列化没配驼峰策略，
 * 两侧必须逐字对上。<b>不含字节流</b>，完整字节点行时调 getPacketDetail 取。
 */
export interface PacketRow {
  Id: number
  Time: string // HH:mm:ss:fffffff
  Socket: number
  Type: number // PacketType
  From: string
  FromLocation: string
  To: string
  ToLocation: string
  Len: number
  Preview: string // 截断的十六进制预览
  Action: number // FilterAction，界面据此上色
}

/**
 * 代理数据列表的一行（<b>代理模式的主列表</b>，对应 WinForms 的 Controls/ProxyList.cs）。
 *
 * 外壳服务的是代理模式，所以主界面用的是这个而不是 PacketRow ——
 * 两者结构相近但不是一回事，各自有独立的 Id 序列，取字节时要说清是哪一份。
 * 同样不含字节流，完整字节点行时调 getPacketDetail 取。
 */
export interface ProxyRow {
  Id: number
  Time: string // HH:mm:ss:fffffff
  Socket: number
  //SunnyNet 的连接标识（HTTP/HTTPS 中间人那条路才有值；SOCKS5 三个调用点传的都是 0）
  //界面上没有这一列 —— 恒为 0 的列比空着更误导，见 PacketList 里的说明
  TheologyID: number
  Type: number // PacketType
  WebSocketType: number
  ClientAddr: string
  ClientLocation: string
  ServerAddr: string
  ServerLocation: string
  ServerDomain: string
  DomainType: number
  Len: number
  Preview: string
  Action: number // FilterAction，界面据此上色
}

/**
 * 封包表能显示的行。两种模式各一种，骨架相同、列不同 ——
 * PacketList 用它做参数类型，两个消费方（代理数据页 / 注入模式）各自把它收窄。
 */
export type PacketListRow = ProxyRow | PacketRow

/** Operate.ProxyConfig.Proxy.DomainType */
/** 存的是 i18n 键，与 PACKET_TYPE 同理。下标是 C# 枚举的整数值。 */
export const DOMAIN_TYPE: Record<number, string> = {
  0: 'dt.socket',
  1: 'dt.http',
  2: 'dt.https',
  3: 'dt.external',
  4: 'dt.websocket',
}

/** Operate.PacketConfig.Packet.PacketType */
/*
  存的是<b>i18n 键</b>，不是 C# 的枚举名。

  界面上要显示「TCP 响应 / TCP Res」这样的产品术语（与 WinForms 的
  Operate.PacketConfig.Packet.GetName_ByPacketType 一致），而不是 TCP_Resp 这种标识符。
  下标就是 C# 枚举的整数值，改 C# 那个枚举的顺序时这里必须跟着改。

  WSARecvEx（11）与 WSARecv（10）指向同一个键 —— C# 侧也是共用一个文案键。

  ⚠️ <b>这张表必须覆盖 C# 枚举的每一个取值。</b>漏掉的那个值查出来是 undefined，
  「类型」列就是<b>一片空白</b>，而且不报任何错。21 / 22（WebSocket 请求 / 响应）
  就这么漏过一次 —— 它们由 SunnyNetCallback 在代理模式的 WebSocket 中间人那条路上产出，
  平时抓 TCP/UDP 撞不上，一抓 WebSocket 就整列空。
*/
export const PACKET_TYPE: Record<number, string> = {
  0: 'pt.ws1Send',
  1: 'pt.ws2Send',
  2: 'pt.ws1SendTo',
  3: 'pt.ws2SendTo',
  4: 'pt.ws1Recv',
  5: 'pt.ws2Recv',
  6: 'pt.ws1RecvFrom',
  7: 'pt.ws2RecvFrom',
  8: 'pt.wsaSend',
  9: 'pt.wsaSendTo',
  10: 'pt.wsaRecv',
  11: 'pt.wsaRecv',
  12: 'pt.wsaRecvFrom',
  13: 'pt.tcpReq',
  14: 'pt.udpReq',
  15: 'pt.tcpResp',
  16: 'pt.udpResp',
  17: 'pt.httpReq',
  18: 'pt.httpResp',
  19: 'pt.httpsReq',
  20: 'pt.httpsResp',
  21: 'pt.wsReq',
  22: 'pt.wsResp',
}

/** Operate.FilterConfig.Filter.FilterAction */
export const enum FilterAction {
  Replace = 0,
  Intercept = 1,
  NoModify_Display = 2,
  NoModify_NoDisplay = 3,
  None = 4,
  Change = 5,
}

/** 一组前景/背景色。 */
export interface ColorPair {
  fore: string
  back: string
}

/** getPrefs 的返回。颜色的唯一真源是 C# 的 UiPrefs，前端不许写死。 */
export interface Prefs {
  isDark: boolean
  language: string
  systemColor: string
  filter: {
    replace: ColorPair
    intercept: ColorPair
    change: ColorPair
    display: ColorPair
  }
}

/** getPacketDetail 的返回。null 表示该封包已被自动清理。 */
export interface PacketDetail {
  id: number
  packet: string | null // base64，滤镜改写后
  raw: string | null // base64，改写前
  modified: boolean
}

/** getStats 的返回。 */
export interface Stats {
  queue: number
  list: number
  total: number
  autoClear: boolean
  autoClearValue: number
}

/* ────────────────────────────────────────────────────────────────
   中低频列表的行 DTO（B9d）

   字段来自 C# 的 ClassObject/Ui/FeedRows.cs，逐字对应 —— 桥的 JSON 序列化
   没配驼峰策略，改一边不改另一边会静默错位（读到 undefined，不报错）。

   映射规则见 FeedRows.cs 顶部：
     DateTime → string     C# 侧格式化好
     Guid     → string     大写无括号
     枚举     → number     前端按 int 分支，不依赖 C# 枚举名
     byte[] / Image / 嵌套 BindingList → 排除（后者改为计数字段）
   ──────────────────────────────────────────────────────────────── */

/** 已选中的目标进程。 */
export interface ProcessRow {
  IsCheck: boolean; ProcessName: string; ProcessID: number
  ModuleName: string; ProcessPath: string
}

/** 白名单 / 黑名单共用 —— 两者字段相同。 */
export interface IPRuleRow {
  IPAddress: string; StartIP: number; EndIP: number; IPLocation: string
  EffectCount: number; IsExpiry: boolean; ExpiryTime: string; CreateTime: string
}

/** 代理账号。 */
export interface AccountRow {
  Id: string; IsCheck: boolean; IsEnable: boolean; UserName: string
  IsLimitLinks: boolean; LimitLinks: number
  IsLimitDevices: boolean; LimitDevices: number
  IsExpiry: boolean; ExpiryTime: string; CreateTime: string
  IsOnLine: boolean; LoginCount: number
}

/** 客户端认证记录。 */
export interface AuthRow {
  //UserName 由 C# 侧查好（AuthRow.From_ 调 GetUserName_ByAccountID），前端不必关联账号表
  AccountId: string; UserName: string; AuthIP: string; IPLocation: string
  LinksNumber: number; DevicesNumber: number; TrafficStatistics: number
  AuthResult: boolean; AuthTime: string
  /** WPC 报上来的设备指纹；普通 SOCKS5 客户端为空串 */
  DeviceId: string
  /** "WPC 1.0" / "SOCKS5" */
  Client: string
}

/** 本地端口映射。 */
export interface MapLocalRow {
  /** 运行期 Id（MapLocal.MID），表里没有主键 */
  Id: string
  IsEnable: boolean; Protocol: number; Host: string; Port: number
  RemotePath: string; LocalPath: string
}

/** 远程端口映射。 */
export interface MapRemoteRow {
  /** 运行期 Id（MapRemote.MID） */
  Id: string
  IsCheck: boolean; IsEnable: boolean
  ProtocolFrom: number; HostFrom: string; PortFrom: number; PathFrom: string
  ProtocolTo: number; HostTo: string; PortTo: number; PathTo: string
}

/** 滤镜。FunctionMask 是位掩码 —— C# 的 FilterFunction 是 12 个 bool 的结构体，不是枚举。 */
/**
 * 运行日志。三种日志（系统 / 滤镜 / 代理）共用这一个形状，
 * 由 FeedList 区分是哪一路 —— 与 C# 的 LogRow 逐字对应。
 */
export interface LogRow {
  Time: string
  FuncName: string
  Content: string
}

/**
 * 滤镜日志。与系统日志<b>形状不同</b>，不能混用同一个类型
 * —— Action / Type 是枚举（int），MatchNum / Len 是数字。
 */
export interface FilterLogRow {
  Time: string
  FilterName: string
  Action: number   // FilterAction
  MatchNum: number
  Type: number     // PacketType
  Len: number
}

/** 代理日志。谁、从哪个 IP、做了什么。 */
export interface ProxyLogRow {
  Time: string
  UserName: string
  LoginIP: string
  Content: string
}

export interface FilterRow {
  Id: string; IsEnable: boolean; Name: string; ExecutionCount: number
  Mode: number; Action: number; StartFrom: number; FunctionMask: number
  AppointHeader: boolean; HeaderContent: string
  AppointSocket: boolean; SocketContent: string
  AppointLength: boolean; LengthContent: string
  AppointPort: boolean; PortContent: string
  IsExecute: boolean; ExecuteType: number; ExecuteId: string
  /** 递进那一列要的三个：位置串非空 = 启用，另两个是「连续」「进位」 */
  ProgressionPosition: string; IsProgressionContinuous: boolean; IsProgressionCarry: boolean
}

/**
 * 右键菜单的七个动作。序号照搬 `Operate.SystemConfig.ListAction`。
 *
 * 滤镜 / 发送 / 机器人 / 仓库四份列表共用这一套 —— WinForms 侧的右键菜单
 * 也是同一个 `SystemConfig.GetCMS_List()`，别给某一份单开一套编号。
 */
export const enum ListAction {
  Top = 0,
  Up = 1,
  Down = 2,
  Bottom = 3,
  Copy = 4,
  Export = 5,
  Delete = 6,
  /** 清空整表（带确认框）。机器人编辑的指令集右键菜单用 */
  CleanUp = 7,
  Import = 8,
}

/** 发送列表。PacketCount 是嵌套封包数 —— 明细按需单独取。 */
export interface SendRow {
  Id: string; IsEnable: boolean; Name: string
  ExecutionCount: number; ExecutionSuccess: number; ExecutionFail: number
  UseSystemSocket: boolean; LoopCount: number; LoopInterval: number
  Notes: string; PacketCount: number
}

/** 机器人。 */
export interface RobotRow {
  Id: string; IsEnable: boolean; Name: string
  ExecutionCount: number; InstructionCount: number
}

/** 封包仓库。 */
export interface WareHouseRow {
  Id: string; Name: string; DataCount: number
}

/** 自动入库规则。 */
export interface AutoStoresRow {
  /** 运行期分配的 AID，不落库 —— 这个模型没有自然主键，见 C# 的 AutoStoresInfo.AID */
  Id: string
  IsEnable: boolean; PacketHead: string; WareHouseId: string
}

/** ProxyCap 节点。RuleCount 是嵌套规则数。 */
export interface ServerRow {
  Id: string; IsEnable: boolean; Name: string; IP: string; Port: number
  ForgotURL: string; RegisterURL: string; VerifyURL: string; RuleCount: number
}

/** ProxyCap 公告。 */
export interface NoticeRow {
  Id: string; Type: number; Title: string; Content: string; More: string; Time: string
}
