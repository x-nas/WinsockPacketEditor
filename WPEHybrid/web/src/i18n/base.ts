// 中英对照表 —— 全项目文案的<b>基准</b>。
//
// 【为什么前端自己存一份，而不是每句都去问 C# 的 UI.T】
// UI.T 走桥，是一次异步往返。界面上几百处静态文案逐句 await 既慢又难写
// （模板里没法 await），而这些字本来就只属于 Vue 这一层，Operate 并不认识它们。
// 所以：<b>页面上的字前端自己管，弹窗与通知的字仍归 C#</b>——后者由 Operate 发起，
// 文案来自 Localizer.cs，前端只是渲染。
//
// 【中英贴在同一个键下，其余语言各自一份 Record<Key, string>】
// 官网 cyber.js 的 INDEX_ZH / INDEX_EN 是两份平行<b>数组</b>，加条目要改两处、
// 顺序错位还不报错。这里两件事都不会：
//   · 中英同键，漏译在类型上就是缺字段；
//   · 其余语言是<b>按键索引的对象</b>而不是数组，顺序无关，
//     而且声明成 Record<Key, string> —— 往这里加一个键，四份译文当场编译不过。
// 换句话说「加条目要改五处」是<b>故意的</b>：它保证不会出现半翻译的界面。
//
// 术语一律照 ClassObject/Localizer.cs（那是产品的官方英文），
// 不要另起译法：Inject / Proxy / WareHouse / Filter / Robot。

export interface Entry {
  zh: string
  en: string
}

/*
  键名规则：<区域>.<语义>。区域取自这一屏在 WinForms 里的对应物，
  便于日后与 Localizer.cs 的键对照。
*/
export const DICT = {
  // ── 标题栏 / 状态栏 ──────────────────────────────────
  //窗口保持最前：对应 WinForms ProxyList 工具条上那个勾选框，这里提到标题栏（它是窗口属性，不属于某一页）
  'win.pin': { zh: '窗口保持最前', en: 'Keep window on top' },
  'win.unpin': { zh: '取消保持最前', en: 'Stop keeping on top' },
  'win.min': { zh: '最小化', en: 'Minimize' },
  'win.max': { zh: '最大化', en: 'Maximize' },
  'win.restore': { zh: '还原', en: 'Restore' },
  'win.close': { zh: '退出', en: 'Exit' },
  'win.nohost': {
    zh: '不在 WebView2 宿主内 —— 请通过 WPEHybrid.exe 打开。',
    en: 'Not inside the WebView2 host — please launch WPEHybrid.exe.',
  },

  'foot.tutorial': { zh: '使用教程', en: 'Tutorial' },
  'foot.faq': { zh: '常见问题', en: 'FAQ' },
  'foot.ready': { zh: 'Ready', en: 'Ready' },
  'foot.running': { zh: '运行中', en: 'Running' },
  'foot.stopped': { zh: '未启动', en: 'Stopped' },

  // ── 启动页 ──────────────────────────────────────────
  'start.eyebrow': { zh: 'WPE_X64 // Select Mode', en: 'WPE_X64 // Select Mode' },
  'start.subtitle': { zh: '选择一种方式开始拦截封包', en: 'Choose a way to start capturing packets' },

  'start.inject.zh': { zh: '注入模式', en: 'Inject Mode' },
  'start.inject.desc': { zh: '以注入进程的方式来拦截封包', en: 'Capture packets by injecting into the process' },
  'start.proxy.zh': { zh: '代理模式', en: 'Proxy Mode' },
  'start.proxy.desc': { zh: '以代理服务端的方式来拦截封包', en: 'Capture packets by acting as a proxy server' },

  'start.inst.zh': { zh: '多开设置', en: 'Instance Settings' },
  'start.inst.desc': { zh: '配置数据库的路径以实现软件多开', en: 'Set the database path to run multiple instances' },
  'start.check': { zh: 'System Check', en: 'System Check' },
  'start.admin': { zh: '管理员权限', en: 'Administrator' },
  'start.geo': { zh: '国家地理库', en: 'Geo database' },
  'start.geoUnit': { zh: '条', en: 'entries' },
  'start.db': { zh: '数据库', en: 'Database' },
  'start.done': { zh: '就绪，等待选择模式', en: 'Ready, waiting for mode selection' },

  // ── 多开设置 ────────────────────────────────────────
  'inst.eyebrow': { zh: 'WPE_X64 // Instance Settings', en: 'WPE_X64 // Instance Settings' },
  'inst.subtitle': { zh: '为本次运行选择一个独立的数据库', en: 'Pick a separate database for this run' },
  'inst.onceTag': { zh: '// 仅本次有效', en: '// this run only' },
  'inst.onceText': {
    zh: '数据库路径不会被保存，程序每次启动都回到默认的 C:\\WPE64DB。要多开，就在每次选择模式之前先到这里改一次。',
    en: 'The database path is not persisted — every launch returns to the default C:\\WPE64DB. To run multiple instances, change it here before each mode selection.',
  },
  'inst.pathPlaceholder': { zh: '例如 D:\\WPE64DB\\instance-02', en: 'e.g. D:\\WPE64DB\\instance-02' },
  'inst.browse': { zh: '浏览', en: 'Browse' },
  'inst.badPath': { zh: '路径无效 —— 需要一个绝对路径，且不能含非法字符', en: 'Invalid path — must be absolute and free of illegal characters' },
  'inst.dir': { zh: '目录', en: 'Directory' },
  'inst.dirOk': { zh: '已存在', en: 'exists' },
  'inst.dirNew': { zh: '将自动创建', en: 'will be created' },
  'inst.db': { zh: '数据库', en: 'Database' },
  'inst.dbOld': { zh: '沿用已有的', en: 'reuse existing' },
  'inst.dbNew': { zh: '将新建一个空库', en: 'a new empty one' },
  'inst.current': { zh: '当前生效', en: 'in effect' },
  'inst.save': { zh: '保存并返回', en: 'Save and go back' },
  'inst.saving': { zh: '切换中…', en: 'Switching…' },
  'inst.cancel': { zh: '取消', en: 'Cancel' },
  'inst.hint': { zh: '保存后请回到启动页选择模式', en: 'After saving, pick a mode on the start page' },

  // ── 代理模式：侧栏 14 页 ────────────────────────────
  'proxy.nav.data': { zh: '代理数据', en: 'Proxy Data' },
  'proxy.nav.client': { zh: '客户端列表', en: 'Client List' },
  'proxy.nav.account': { zh: '账号列表', en: 'Account List' },
  'proxy.nav.filter': { zh: '滤镜列表', en: 'Filter List' },
  'proxy.nav.send': { zh: '发送列表', en: 'Send List' },
  'proxy.nav.robot': { zh: '机器人列表', en: 'Robot List' },
  'proxy.nav.warehouse': { zh: '仓库列表', en: 'WareHouse List' },
  'proxy.nav.stat': { zh: '统计数据', en: 'Statistics' },
  'proxy.nav.diff': { zh: '文本对比', en: 'Text Comparison' },
  'proxy.nav.xor': { zh: '异或计算', en: 'XOR Calculator' },
  'proxy.nav.transcode': { zh: '编码转换', en: 'Transcoding' },
  'proxy.nav.extract': { zh: '数据提取', en: 'Data Extraction' },
  'proxy.nav.wpc': { zh: 'WPC 配置', en: 'WPC Config' },
  'proxy.nav.log': { zh: '系统日志', en: 'System Log' },
  /*
    快捷面板的四个标签<b>另起一组短键</b>，不复用 proxy.nav.* ——
    那组还给侧栏和滤镜编辑的「执行」下拉用，在那两处「滤镜列表」才是全名。

    中英都去掉「列表 / List」：面板固定 320px 宽，四个标签要排成一行，
    英文的 FILTER LIST / WAREHOUSE LIST 会折行（还是大写，更宽）。
    面板本身已经在说这是列表，标签重复一遍纯属挤宽度。
  */
  'quick.tab.filter': { zh: '滤镜', en: 'Filter' },
  'quick.tab.send': { zh: '发送', en: 'Send' },
  'quick.tab.robot': { zh: '机器人', en: 'Robot' },
  'quick.tab.warehouse': { zh: '仓库', en: 'WareHouse' },

  'proxy.notReady': { zh: '这一页的 Vue 版还没做', en: 'This page has no Vue version yet' },

  // ── 代理模式：运行状态条 ────────────────────────────
  'proxy.running': { zh: '运行中', en: 'Running' },
  'proxy.stopped': { zh: '未启动', en: 'Stopped' },
  'proxy.working': { zh: '处理中…', en: 'Working…' },
  'proxy.uptime': { zh: '已运行', en: 'Uptime' },
  'proxy.start': { zh: '开始代理', en: 'Start' },
  'proxy.stop': { zh: '停止代理', en: 'Stop' },
  'proxy.clear': { zh: '清空', en: 'Clear' },
  'proxy.settings': { zh: '设置', en: 'Settings' },

  // ── 代理模式：统计 ──────────────────────────────────
  'proxy.st.total': { zh: '代理总数', en: 'Proxy total' },
  'proxy.st.tcpConn': { zh: 'TCP 连接数', en: 'TCP sessions' },
  'proxy.st.udpConn': { zh: 'UDP 连接数', en: 'UDP sessions' },
  'proxy.st.account': { zh: '在线/账号', en: 'Online/total' },
  'proxy.st.filter': { zh: '已过滤', en: 'Filtered out' },
  'proxy.st.queue': { zh: '缓存区', en: 'Queue' },
  'proxy.st.tcpReq': { zh: 'TCP 请求', en: 'TCP req' },
  'proxy.st.tcpResp': { zh: 'TCP 响应', en: 'TCP resp' },
  'proxy.st.udpReq': { zh: 'UDP 请求', en: 'UDP req' },
  'proxy.st.udpResp': { zh: 'UDP 响应', en: 'UDP resp' },
  'proxy.st.httpReq': { zh: 'HTTP 请求', en: 'HTTP req' },
  'proxy.st.httpResp': { zh: 'HTTP 响应', en: 'HTTP resp' },

  // ── 代理模式：工具条与快捷面板 ──────────────────────
  'proxy.search': { zh: '在封包内容中查找…', en: 'Search in packet data…' },
  /* ── 查找封包（对应 WinForms 的 Controls/SearchPacket）───────── */
  'sp.hex': { zh: '十六进制', en: 'Hex' },
  'sp.text': { zh: '文本', en: 'Text' },
  'sp.modeHint': { zh: '切换匹配对象：文本（UTF8 解码后）/ 十六进制（形如 0A 1B 2C）', en: 'Match against decoded text (UTF-8) or hex (e.g. 0A 1B 2C)' },
  'sp.next': { zh: '查找下一个', en: 'Find next' },
  'sp.fromHead': { zh: '从头查找', en: 'From start' },
  'sp.searching': { zh: '查找中…', en: 'Searching…' },
  'sp.noMatch': { zh: '没有匹配的封包', en: 'No matching packet' },
  'sp.wrapped': { zh: '已到末尾，从头继续', en: 'Reached the end, wrapped to start' },
  'sp.badRegex': { zh: '正则表达式有误', en: 'Invalid regular expression' },
  'sp.clear': { zh: '清除查找（Esc）', en: 'Clear search (Esc)' },
  'proxy.autoRoll': { zh: '自动滚动', en: 'Auto scroll' },
  'proxy.autoClear': { zh: '自动清理', en: 'Auto clear' },
  'proxy.emptyRules': { zh: '暂无数据', en: 'No data' },
  'proxy.act.replace': { zh: '替换', en: 'Replace' },
  'proxy.act.intercept': { zh: '拦截', en: 'Intercept' },
  'proxy.act.display': { zh: '仅显示', en: 'Display' },
  'proxy.act.hide': { zh: '不显示', en: 'Hidden' },
  'proxy.act.none': { zh: '无', en: 'None' },
  //照 FilterEdit.Designer 的 rbFilterAction_Change.Text，不是自拟的「改写」
  'proxy.act.change': { zh: '换包', en: 'Change' },

  // ── 设置弹窗（12 项，目前只做了代理设置）────────────
  'set.save': { zh: '保存', en: 'Save' },
  'set.proxy': { zh: '代理设置', en: 'Proxy Settings' },
  'set.process': { zh: '进程设置', en: 'Process Settings' },
  'set.leach': { zh: '过滤设置', en: 'Leach Settings' },
  'set.hook': { zh: '拦截设置', en: 'Hook Settings' },
  'set.list': { zh: '列表设置', en: 'List Settings' },
  'set.map': { zh: '映射设置', en: 'Map Settings' },
  'set.extproxy': { zh: '外部代理设置', en: 'External Proxy Settings' },
  'set.hotkey': { zh: '快捷键设置', en: 'HotKey Settings' },
  'set.backup': { zh: '备份设置', en: 'BackUp Settings' },
  'set.remote': { zh: '远程管理设置', en: 'Remote MGT Settings' },
  'set.firewall': { zh: '防火墙设置', en: 'FireWall Settings' },
  //软件设置（标题栏齿轮）—— 语言与深浅色。与那 12 个抓包设置不是一类，见 AppSetting.vue
  'set.app': { zh: '软件设置', en: 'Preferences' },
  'set.app.lang': { zh: '界面语言', en: 'Language' },
  'set.app.langHint': { zh: '页面文字与弹窗文案一起切换。按「保存」后生效并记住。', en: 'Switches both page text and dialog messages. Applied and remembered once you press Save.' },
  'set.app.theme': { zh: '外观', en: 'Appearance' },
  'set.app.dark': { zh: '深色', en: 'Dark' },
  'set.app.light': { zh: '浅色', en: 'Light' },
  'set.app.system': { zh: '跟随系统', en: 'Follow system' },
  'set.app.themeHint': { zh: '同一套版式换一组配色，与主程序共用这个设置。跟随系统时随操作系统的深浅设置一起变。', en: 'Same layout, different palette. Shared with the main program. “Follow system” tracks the OS light/dark setting.' },
  'set.app.now': { zh: '当前', en: 'Current' },
  'set.system': { zh: '系统设置', en: 'System Settings' },
  'set.lockedHint': {
    zh: '代理服务正在运行 —— 监听相关的设置改了不会生效（SuperSocket 只在启动时读一次）。要修改请先停止服务。',
    en: 'The proxy is running — listener settings will not take effect (SuperSocket reads them only at startup). Stop the service first.',
  },

  // 列表设置
  // ── 系统设置 ──────────────────────────────────────
  // ── 防火墙设置 ────────────────────────────────────
  //文案照 FireWallSetting / FireWallRules 的 Designer，不另起说法
  'fw.grp.main': { zh: '防火墙', en: 'Firewall' },
  'fw.enable': { zh: '启用防火墙', en: 'Enable firewall' },
  'fw.mode': { zh: '工作模式', en: 'Mode' },
  'fw.whiteMode': { zh: '白名单模式', en: 'Whitelist' },
  'fw.blackMode': { zh: '黑名单模式', en: 'Blacklist' },
  'fw.whiteModeHint': {
    zh: '只放行白名单里的 IP，其余一律拒绝 —— 名单为空等于谁都连不上。',
    en: 'Only IPs on the whitelist are allowed; everything else is refused. An empty list blocks everyone.',
  },
  'fw.blackModeHint': {
    zh: '默认放行，只拒绝黑名单里的 IP。',
    en: 'Allow by default; refuse only IPs on the blacklist.',
  },

  'fw.grp.rules': { zh: '自动规则', en: 'Automatic Rules' },
  'fw.autoWhite': { zh: '自动加入白名单', en: 'Auto-allow' },
  'fw.authOk': { zh: '认证成功的 IP', en: 'IPs that authenticated' },
  'fw.autoBlack': { zh: '自动加入黑名单', en: 'Auto-block' },
  'fw.authFail': { zh: '认证失败的 IP', en: 'IPs that failed auth' },
  'fw.unsupport': { zh: '不支持的 Socks 协议', en: 'Unsupported SOCKS' },
  'fw.never': { zh: '永久有效', en: 'Never expires' },
  'fw.blockTime': { zh: '屏蔽时长', en: 'Block for' },
  'fw.blockTimeHint': { zh: '两条规则共用这个时长', en: 'shared by both rules above' },
  'fw.minutes': { zh: '分钟', en: 'minutes' },
  'fw.autoClear': { zh: '自动清理', en: 'Auto-clean' },
  'fw.expired': { zh: '已过期的 IP', en: 'Expired entries' },

  'fw.grp.lists': { zh: '名单', en: 'Lists' },
  'fw.whiteList': { zh: '白名单', en: 'Whitelist' },
  'fw.blackList': { zh: '黑名单', en: 'Blacklist' },
  'fw.add': { zh: '新增', en: 'Add' },
  'fw.edit': { zh: '编辑', en: 'Edit' },
  'fw.effect': { zh: '生效', en: 'Hits' },
  'fw.expiryTime': { zh: '过期时间', en: 'Expires' },
  'fw.emptyList': { zh: '这张名单是空的', en: 'This list is empty' },

  'fw.ipKind': { zh: '类型', en: 'Kind' },
  'fw.single': { zh: '单个 IP', en: 'Single IP' },
  'fw.range': { zh: 'IP 段', en: 'IP range' },
  'fw.expiry': { zh: '有效期', en: 'Validity' },
  'fw.hasExpiry': { zh: '设置过期时间', en: 'Set an expiry' },
  'fw.expiryHint': {
    zh: '不勾选就是永久有效。过期的条目在「自动清理」开着时会被清掉。',
    en: 'Unchecked means permanent. Expired entries are removed when Auto-clean is on.',
  },

  // ── 客户端列表 ────────────────────────────────────
  //列名照 ClientList.InitTable_AuthList 里那九列，不另起译法
  'cli.authTime': { zh: '认证时间', en: 'Auth Time' },
  'cli.ip': { zh: 'IP 地址', en: 'IP' },
  'cli.links': { zh: '链接数', en: 'Links' },
  'cli.devices': { zh: '设备数', en: 'Devices' },
  'cli.traffic': { zh: '流量统计', en: 'Traffic' },
  'cli.online': { zh: '在线 (分钟)', en: 'Online (min)' },
  'cli.empty': { zh: '还没有客户端连上来', en: 'No client has connected yet' },
  //文案照 Localizer.cs 的 FireWallSetting.WhiteList.Add / BlackList.*
  'cli.toWhite': { zh: '加入白名单', en: 'Add to Whitelist' },
  'cli.toBlack': { zh: '加入黑名单', en: 'Add to Blacklist' },
  'cli.b1h': { zh: '屏蔽 1 小时', en: 'Block for 1 hour' },
  'cli.b1d': { zh: '屏蔽 1 天', en: 'Block for 1 day' },
  'cli.b30d': { zh: '屏蔽 30 天', en: 'Block for 30 days' },
  'cli.bever': { zh: '永久屏蔽', en: 'Block permanently' },
  'cli.whiteOk': { zh: '已加入到白名单', en: 'added to the whitelist' },
  'cli.blackOk': { zh: '已加入到黑名单', en: 'added to the blacklist' },
  'cli.noIp': { zh: '这一行没有 IP 地址', en: 'This row has no IP address' },
  'cli.conns': { zh: '连接明细', en: 'Connections' },
  'cli.pickHint': { zh: '选中上面一行，看这个客户端当前开着的连接', en: 'Pick a row above to see its open connections' },
  'cli.noConn': { zh: '这个客户端当前没有连接', en: 'This client has no open connection' },
  //英文用 Port 不用 Src Port：列宽 48px，SRC PORT 大写后约 55px 会折成两行。
  //旁边就是 Target，单说 Port 不会有歧义。
  'cli.srcPort': { zh: '源端口', en: 'Port' },
  'cli.target': { zh: '目标', en: 'Target' },
  'cli.via': { zh: '实际出口', en: 'Via' },
  //相同时写「直连」而不是短横 —— 短横看着像没取到数据，而事实是没有被转走
  'cli.direct': { zh: '直连', en: 'Direct' },
  'cli.viaHint': {
    zh: '实际连过去的地址。没有被转走时显示「直连」；走外部代理或命中远程映射规则时，这里显示真正连过去的地址。',
    en: 'Where it actually connected. Shows Direct when untouched; shows the real address when an external proxy or a remote mapping rule redirected it.',
  },

  // ── 拦截设置 ──────────────────────────────────────
  //「请求 / 响应」两个词照 HookSetting.Designer 的 cbTCP_Req / cbTCP_Resp
  'pt.req': { zh: '请求', en: 'Request' },
  'pt.resp': { zh: '响应', en: 'Response' },

  'set.grp.hookDir': { zh: '抓取方向', en: 'Capture Directions' },
  'set.hook.tcp': { zh: 'TCP 协议', en: 'TCP' },
  'set.hook.udp': { zh: 'UDP 协议', en: 'UDP' },
  'set.hook.dirHint': {
    zh: '只有勾上的方向会被抓：其余方向直接转发，不进列表、也不跑滤镜。',
    en: 'Only checked directions are captured; the rest are forwarded untouched — no list, no filters.',
  },
  'set.hook.offWarn': {
    zh: '有方向被关掉了 —— 那个方向看不到封包、滤镜也不会命中，是正常的。',
    en: 'Some directions are off — those packets will not appear and filters will not hit them.',
  },
  'set.hook.runOnly': {
    zh: '抓取方向仅本次运行有效，重启后回到全部开启（拆包设置则会保存）。',
    en: 'Directions apply to this run only and reset on restart. Unpacking settings are saved.',
  },

  //注入模式那一页：12 个 WinSock 钩子，按 1.1 / 2.0 / WSA 三组
  'set.grp.hookWs1': { zh: 'Winsock 1.1', en: 'Winsock 1.1' },
  'set.grp.hookWs2': { zh: 'Winsock 2.0', en: 'Winsock 2.0' },
  'set.grp.hookWsa': { zh: 'Winsock 2.0 · WSA', en: 'Winsock 2.0 · WSA' },
  'set.hook.injectHint': {
    zh: '关掉哪个入口，那个入口就既不进列表也不过滤镜。这 12 个开关会落库，也会立刻推给目标进程。',
    en: 'A disabled entry point is neither listed nor filtered. These 12 switches are persisted and pushed to the target at once.',
  },
  'set.hook.injectWarn': {
    zh: '有入口被关掉了 —— 那个方向的封包一条都不会出现。',
    en: 'Some entry points are off — no packet from those directions will show up.',
  },
  'set.grp.unpack': { zh: '拆包设置', en: 'Packet Unpacking' },
  'set.hook.unpack': { zh: '启用拆包', en: 'Unpacking' },
  'set.hook.unpackHint': {
    zh: 'TCP 是字节流，一次收包可能含多个应用层封包。按包头特征切开，列表里才是一条一条的。',
    en: 'TCP is a stream, so one read may hold several application packets. Split them by header signature to list them one by one.',
  },
  'set.hook.head': { zh: '包头特征', en: 'Header bytes' },
  'set.hook.headPh': { zh: '如 01 00 00', en: 'e.g. 01 00 00' },
  'set.hook.length': { zh: '长度字段位置', en: 'Length field' },
  'set.hook.lengthPh': { zh: '如 4-5', en: 'e.g. 4-5' },
  'set.hook.lengthHint': { zh: '包内第几到第几个字节存长度，从 1 数起', en: 'Byte range holding the length, counting from 1' },

  'set.grp.workMode': { zh: '工作模式', en: 'Work Mode' },
  'set.speedMode': { zh: '极速模式', en: 'Speed Mode' },
  'set.speedModeOn': { zh: '开启', en: 'Enabled' },
  'set.speedModeHint': {
    zh: '只做记数以及滤镜的匹配和修改，不再显示封包数据',
    en: 'Only counts and runs filter matching and rewriting; packet data is no longer shown',
  },

  'set.grp.listExecute': { zh: '列表执行模式', en: 'List Execution' },
  'set.listExecute': { zh: '多条同时启用时', en: 'When several are on' },
  'set.exec.together': { zh: '同时执行', en: 'Together' },
  'set.exec.sequence': { zh: '按顺序执行', en: 'In sequence' },
  'set.exec.togetherHint': { zh: '发送 / 机器人列表里启用的条目一起跑', en: 'Enabled send / robot entries all run at once' },
  'set.exec.sequenceHint': { zh: '一条跑完再跑下一条', en: 'Each waits for the previous one to finish' },

  'set.grp.filterExecute': { zh: '滤镜执行模式', en: 'Filter Execution' },
  'set.filterExecute': { zh: '命中多条滤镜时', en: 'On multiple matches' },
  'set.exec.priority': { zh: '优先原则', en: 'First match wins' },
  'set.exec.fseq': { zh: '按顺序执行', en: 'Run all' },
  'set.exec.priorityHint': { zh: '只执行列表里命中的第一条', en: 'Only the first matching filter runs' },
  'set.exec.fseqHint': { zh: '逐条执行，改写会叠加', en: 'All matching filters run; rewrites stack' },

  'set.colorTitle': { zh: '动作配色', en: 'Action Colors' },
  'set.colorPrevType': { zh: '请求', en: 'Req' },
  'set.colorTip': { zh: '点色块可改配色', en: 'Click a swatch to edit its colors' },
  'set.foreColor': { zh: '文字', en: 'Text' },
  'set.backColor': { zh: '背景', en: 'Background' },
  'set.resetColor': { zh: '还原', en: 'Reset' },

  'set.grp.cols': { zh: '显示哪些列', en: 'Visible Columns' },
  'set.colsHint': {
    zh: '序号 / 时间 / 域名 / 数据始终显示 —— 序号是取字节的钥匙，另外三个是这张表的意义所在。',
    en: 'No., Time, Domain and Data are always shown — No. is the key used to fetch bytes, the other three are what the table is for.',
  },
  'set.grp.autoClear': { zh: '自动清理', en: 'Auto Clear' },
  'set.autoClearOn': { zh: '超出后整表清空', en: 'Clear the whole list when exceeded' },
  'set.keepRows': { zh: '保留条数', en: 'Keep rows' },
  'set.keepRowsHint': { zh: '100 ~ 500000。到达后整表清空，不是只删旧的', en: '100–500000. The whole list is cleared, not just the oldest rows' },

  // 过滤设置
  'set.leach.lead': {
    zh: '决定哪些封包进入列表 —— 与「滤镜」不同，这里只管收不收，不改内容。',
    en: 'Decides which packets enter the list. Unlike filters, this only accepts or drops — it never rewrites.',
  },
  'set.leach.mode': { zh: '过滤方式', en: 'Mode' },
  'set.leach.only': { zh: '只显示命中的', en: 'Show only matches' },
  'set.leach.hide': { zh: '不显示命中的', en: 'Hide matches' },
  'set.leach.conds': { zh: '条件', en: 'Conditions' },
  'set.leach.none': { zh: '一个条件都没开 —— 过滤不会生效，所有封包都会进列表', en: 'No condition enabled — filtering is off, every packet enters the list' },
  'set.leach.socket': { zh: '套接字', en: 'Socket' },
  'set.leach.socketPh': { zh: '如 1284，多个用逗号隔开', en: 'e.g. 1284, comma-separated' },
  'set.leach.ip': { zh: 'IP 地址', en: 'IP address' },
  'set.leach.ipPh': { zh: '如 192.168.1.1', en: 'e.g. 192.168.1.1' },
  'set.leach.port': { zh: '端口号', en: 'Port' },
  'set.leach.portPh': { zh: '如 443', en: 'e.g. 443' },
  'set.leach.head': { zh: '指定包头', en: 'Header' },
  'set.leach.headPh': { zh: '十六进制，如 16 03 01', en: 'Hex, e.g. 16 03 01' },
  'set.leach.data': { zh: '指定内容', en: 'Content' },
  'set.leach.dataPh': { zh: '十六进制或文本', en: 'Hex or text' },
  'set.leach.len': { zh: '长度', en: 'Length' },
  'set.leach.lenPh': { zh: '如 512', en: 'e.g. 512' },
  'set.leach.types': { zh: '类别', en: 'Packet Types' },
  'set.leach.byType': { zh: '按类别过滤', en: 'Filter by type' },

  'set.grp.addr': { zh: '监听地址', en: 'Listen Address' },
  'set.grp.socks': { zh: 'SOCKS 代理', en: 'SOCKS Proxy' },
  'set.grp.http': { zh: 'HTTP 代理', en: 'HTTP Proxy' },
  'set.grp.system': { zh: '系统 · 证书', en: 'System · Certificate' },

  'set.autoIp': { zh: '代理服务IP', en: 'Proxy IP' },
  'set.autoDetect': { zh: '自动检测', en: 'Auto detect' },
  'set.appointIp': { zh: '指定地址', en: 'Specify address' },
  'set.enableSocks': { zh: '启用 SOCKS5', en: 'Enable SOCKS5' },
  'set.socksRequired': { zh: '必选 —— 关掉服务就起不来', en: 'Required — the service cannot start without it' },
  'set.socksPort': { zh: 'SOCKS5 端口', en: 'SOCKS5 port' },
  'set.enableAuth': { zh: '启用身份认证', en: 'Require authentication' },
  'set.authUserPass': { zh: '用户名 / 密码', en: 'Username / password' },
  'set.authOnHint': { zh: '需要代理账号才能连接', en: 'A proxy account is required to connect' },
  'set.authOffHint': { zh: '任何客户端都能连接', en: 'Any client can connect' },
  'set.maxConn': { zh: '最大连接数', en: 'Max connections' },
  'set.enableHttp': { zh: '启用 HTTP 代理', en: 'Enable HTTP proxy' },
  'set.httpPort': { zh: 'HTTP 端口', en: 'HTTP port' },
  'set.portDiffer': { zh: '不能与 SOCKS5 端口相同', en: 'Must differ from the SOCKS5 port' },
  'set.systemProxy': { zh: '系统代理', en: 'System proxy' },
  'set.systemProxyHint': { zh: '立即生效，不等保存', en: 'Applies immediately, no need to save' },
  'set.cert': { zh: '导出证书', en: 'Export certificate' },
  'set.exportCert': { zh: '导出', en: 'Export' },
  'set.cert.cerBin': { zh: 'Cer 证书（二进制格式）', en: 'Cer certificate (binary)' },
  'set.cert.cerB64': { zh: 'Cer 证书（文本格式）', en: 'Cer certificate (text)' },
  'set.cert.crtBin': { zh: 'Crt 证书（二进制格式）', en: 'Crt certificate (binary)' },
  'set.cert.crtB64': { zh: 'Crt 证书（文本格式）', en: 'Crt certificate (text)' },
  'set.cert.pemB64': { zh: 'Pem 证书（文本格式）', en: 'Pem certificate (text)' },
  'set.cert.android': { zh: '安卓系统证书（9a7ae4b0.0）', en: 'Android system cert (9a7ae4b0.0)' },

  // ── 代理模式：滤镜列表 ──────────────────────────────
  //列与文案照 Controls/FilterList.cs 的那组 AntdUI.Column
  'flt.add': { zh: '新增滤镜', en: 'New filter' },
  'flt.import': { zh: '导入', en: 'Import' },
  'flt.export': { zh: '导出', en: 'Export' },
  'flt.clearAll': { zh: '清空', en: 'Clear all' },
  'flt.resetCount': { zh: '重置计数', en: 'Reset counts' },
  'flt.enableAll': { zh: '全部启用', en: 'Enable all' },
  'flt.disableAll': { zh: '全部禁用', en: 'Disable all' },
  'flt.empty': { zh: '还没有滤镜。新增一个，就能在这里改写经过的封包。', en: 'No filters yet. Add one to start rewriting packets.' },
  /*
    执行模式的说明。语义以 Operate.FilterConfig.List.DoFilterList 为准：
      Priority 从上到下，第一个命中的执行完就返回，后面的不再跑
      Sequence 从上到下逐个匹配，命中的都跑，改写逐个叠加
    两种模式共有：命中的动作是 拦截 / 换包 / 只显示 / 不显示 时立刻停 ——
    所以 Sequence 下只有「替换」会继续往下走。
  */
  'flt.mode.priority': { zh: '优先原则', en: 'First match wins' },
  'flt.mode.sequence': { zh: '按顺序执行', en: 'Run all matches' },
  'flt.mode.priorityHint': {
    zh: '从上到下匹配，第一个命中的执行完就停，后面的滤镜不再执行 —— 顺序决定了谁说了算。',
    en: 'Matched top-down; the first hit runs and the rest are skipped — order decides which one wins.',
  },
  'flt.mode.sequenceHint': {
    zh: '从上到下匹配，命中的滤镜都会执行，改写逐个叠加 —— 顺序决定了叠加的先后。',
    en: 'Matched top-down; every hit runs and the rewrites stack — order decides the sequence.',
  },
  'flt.mode.stopNote': {
    zh: '两种模式下，命中的动作是拦截 / 换包 / 只显示 / 不显示时都会立即停止。',
    en: 'In both modes, a hit whose action is Intercept / Change / Display / Hidden stops the chain immediately.',
  },

  //账号列表去掉启用列时删过一次，滤镜列表又要用 —— 这里是它唯一的使用者
  'col.enable': { zh: '启用', en: 'On' },
  'col.filterName': { zh: '滤镜名称', en: 'Filter' },
  'col.action': { zh: '动作', en: 'Action' },
  'col.execCount': { zh: '执行次数', en: 'Runs' },
  'col.appoint': { zh: '指定类型', en: 'Conditions' },
  'col.progression': { zh: '递进', en: 'Step' },


  //「指定类型」四个标签，照抄 FilterList 的 Render
  'flt.ap.head': { zh: '包头', en: 'Header' },
  'flt.ap.socket': { zh: '套接字', en: 'Socket' },
  'flt.ap.port': { zh: '端口', en: 'Port' },
  'flt.ap.length': { zh: '长度', en: 'Length' },

  //「递进」三个标签
  'flt.pg.on': { zh: '启用', en: 'On' },
  'flt.pg.continuous': { zh: '连续', en: 'Continuous' },
  'flt.pg.carry': { zh: '进位', en: 'Carry' },

  //滤镜编辑弹窗（Controls/FilterEdit）
  'flt.e.title': { zh: '滤镜编辑', en: 'Edit filter' },
  'flt.e.notFound': { zh: '这条滤镜已经不在列表里了', en: 'This filter is no longer in the list' },
  'flt.e.mode': { zh: '模式', en: 'Mode' },
  'flt.e.normal': { zh: '普通', en: 'Normal' },
  'flt.e.advanced': { zh: '高级', en: 'Advanced' },
  'flt.e.normalTip': { zh: '查找与修改对齐同一列', en: 'Search and modify share the same columns' },
  'flt.e.advancedTip': { zh: '查找与修改的位置各自独立', en: 'Search and modify positions are independent' },
  'flt.e.startFrom': { zh: '修改起始于', en: 'Modify from' },
  'flt.e.fromHead': { zh: '包头', en: 'Packet head' },
  'flt.e.fromPos': { zh: '指定位置', en: 'Given position' },
  'flt.e.fromHeadTip': { zh: '从包头第 1 字节算起', en: 'Counted from the first byte' },
  'flt.e.fromPosTip': { zh: '相对匹配点的偏移，可以是负数', en: 'Offset from the match point; may be negative' },

  'flt.e.gridNormal': { zh: '查找 / 修改', en: 'SEARCH / MODIFY' },
  'flt.e.gridSearch': { zh: '查找', en: 'SEARCH' },
  'flt.e.gridModify': { zh: '修改', en: 'MODIFY' },
  'flt.e.jump': { zh: '跳到第', en: 'Go to' },
  'flt.e.go': { zh: '跳转', en: 'Go' },

  'flt.e.excludeOn': { zh: '设为排除', en: 'Mark exclude' },
  'flt.e.excludeOff': { zh: '取消排除', en: 'Unmark exclude' },
  'flt.e.progressionOn': { zh: '设为递进', en: 'Mark step' },
  'flt.e.randomOn': { zh: '设为随机', en: 'Mark random' },
  'flt.e.markOff': { zh: '取消标记', en: 'Clear mark' },
  //文案照 Localizer.cs 的 FilterEditForm.Exclude.Error，不另起译法
  'flt.e.excludeEmpty': { zh: '空值无法设置排除', en: 'Cannot set Exclude for Empty' },
  //剪贴板四项：文案与提示语照 Localizer.cs（Copy / Cut / Paste / Delete 及各自的 .Success）
  'flt.e.execute': { zh: '执行', en: 'Execute' },
  'flt.e.executeOn': { zh: '命中后执行', en: 'Run on match' },
  'flt.e.executeEmpty': { zh: '这张表还没有可选项', en: 'That list is empty' },
  'flt.e.copy': { zh: '复制', en: 'Copy' },
  'flt.e.cut': { zh: '剪切', en: 'Cut' },
  'flt.e.paste': { zh: '粘贴', en: 'Paste' },
  'flt.e.del': { zh: '删除', en: 'Delete' },
  'flt.e.copyOk': { zh: '数据已复制', en: 'Copy Completed' },
  'flt.e.cutOk': { zh: '数据已剪切', en: 'Cut Completed' },
  'flt.e.pasteOk': { zh: '数据已粘贴', en: 'Pasting Completed' },
  'flt.e.delOk': { zh: '数据已删除', en: 'Delete Completed' },
  'flt.e.invalidHex': { zh: '请输入有效的十六进制数值', en: 'Please enter a valid HEX or (*)' },
  'flt.e.emptyCell': { zh: '这个格子是空的', en: 'This cell is empty' },
  'flt.e.clearCells': { zh: '清空内容', en: 'Clear cells' },

  'flt.e.funcs': { zh: '作用于哪些封包', en: 'Applies to' },

  'flt.e.headPh': { zh: '如 AABB 或 AA BB', en: 'e.g. AABB or AA BB' },
  //占位符直接示范各自正则允许的写法 —— 现在保存会按格式卡，光写「套接字编号」等于没说
  'flt.e.socketPh': { zh: '如 1234，多个用 ; 隔开', en: 'e.g. 1234, separate with ;' },
  'flt.e.lengthPh': { zh: '如 20 或 20-64，多个用 ; 隔开', en: 'e.g. 20 or 20-64, separate with ;' },
  'flt.e.portPh': { zh: '如 80 或 80-90，多个用 ; 隔开', en: 'e.g. 80 or 80-90, separate with ;' },
  'flt.e.step': { zh: '步长', en: 'Step' },
  'flt.e.carryNum': { zh: '进位值', en: 'Carry at' },

  //右键菜单，与 SystemConfig.GetCMS_List() 逐条对应
  'lst.top': { zh: '置顶', en: 'Move to top' },
  'lst.up': { zh: '向上移动', en: 'Move up' },
  'lst.down': { zh: '向下移动', en: 'Move down' },
  'lst.bottom': { zh: '置底', en: 'Move to bottom' },
  'lst.copy': { zh: '复制', en: 'Duplicate' },
  'lst.export': { zh: '导出', en: 'Export' },
  'lst.delete': { zh: '删除', en: 'Delete' },
  /* ── 封包列表的右键菜单 ──────────────────────────────────── */
  'pm.toSend': { zh: '添加到发送', en: 'Add to send' },
  'pm.toFilter': { zh: '添加到滤镜列表', en: 'Add to filters' },
  'pm.toTextA': { zh: '添加到文本 A', en: 'Add to text A' },
  'pm.toTextB': { zh: '添加到文本 B', en: 'Add to text B' },
  'pm.toTextAOk': { zh: '已添加到文本 A', en: 'Added to text A' },
  'pm.toTextBOk': { zh: '已添加到文本 B', en: 'Added to text B' },
  'pm.toWareHouse': { zh: '添加到仓库', en: 'Add to warehouse' },
  'pm.setSysSocket': { zh: '设置系统套接字', en: 'Set as system socket' },
  'pm.toExcel': { zh: '导出到 Excel', en: 'Export to Excel' },
  'pm.selectAll': { zh: '全选', en: 'Select all' },
  'pm.deselect': { zh: '取消选择', en: 'Deselect' },
  'pm.added': { zh: '已添加', en: 'Added' },
  'pm.addFail': { zh: '添加失败', en: 'Failed to add' },
  'pm.copied': { zh: '已复制到剪贴板', en: 'Copied to clipboard' },
  'pm.copyFail': { zh: '没有可复制的数据', en: 'Nothing to copy' },
  'pm.toFilterOk': { zh: '添加到滤镜列表成功', en: 'Added to filters' },
  'pm.toFilterFail': { zh: '添加到滤镜列表失败', en: 'Failed to add to filters' },
  'pm.sysSocketOk': { zh: '系统套接字已设为', en: 'System socket set to' },
  'pm.sysSocketFail': { zh: '设置系统套接字失败', en: 'Failed to set system socket' },

  'lst.needPick': { zh: '请先勾选要操作的行', en: 'Select the rows to act on first' },

  // ── 代理模式：账号列表 ──────────────────────────────
  //列名照 Controls/AccountList.cs 里那组 AntdUI.Column
  'acct.add': { zh: '新增账号', en: 'New account' },
  'acct.batch': { zh: '批量创建', en: 'Batch create' },
  'acct.edit': { zh: '编辑账号', en: 'Edit account' },
  'acct.import': { zh: '导入', en: 'Import' },
  'acct.export': { zh: '导出', en: 'Export' },
  'acct.clearAll': { zh: '清空', en: 'Clear all' },
  'acct.search': { zh: '搜索用户名…', en: 'Search username…' },
  'acct.empty': { zh: '还没有代理账号', en: 'No proxy accounts yet' },
  'acct.noMatch': { zh: '没有匹配的账号', en: 'No matching account' },
  'acct.authOffHint': { zh: '身份认证已关闭，账号当前不生效', en: 'Authentication is off — accounts are not in use' },

  //英文用 Expiry 而不是 Expires in：工具条按 1120 的最小宽度算，那两个词的差额正好是溢出的那一截
  'acct.expiryIn': { zh: '到期于', en: 'Expiry' },
  'acct.anyDate': { zh: '不限', en: 'Any' },
  'acct.clearRange': { zh: '清除时间范围', en: 'Clear range' },
  'acct.clearSearch': { zh: '清除搜索（Esc）', en: 'Clear search (Esc)' },


  'col.user': { zh: '用户名', en: 'Username' },
  'col.state': { zh: '状态', en: 'State' },
  'col.links': { zh: '链接数', en: 'Links' },
  'col.devices': { zh: '设备数', en: 'Devices' },
  'col.expiry': { zh: '过期时间', en: 'Expires' },
  'col.ops': { zh: '操作', en: 'Actions' },

  'acct.online': { zh: '在线', en: 'Online' },
  'acct.offline': { zh: '离线', en: 'Offline' },
  'acct.unlimited': { zh: '无限制', en: 'Unlimited' },
  'acct.never': { zh: '永不过期', en: 'Never' },

  //右键菜单，条目与顺序照 Operate.GetCMS_AccountList
  //与 WinForms 的 AccountList.Empty 同一句
  'acct.cm.needPick': { zh: '请先勾选要操作的账号', en: 'Select the accounts to act on first' },
  'acct.cm.adjust': { zh: '批量调整', en: 'Bulk adjust' },
  'acct.cm.export': { zh: '批量导出', en: 'Bulk export' },
  'acct.cm.delete': { zh: '批量删除', en: 'Bulk delete' },

  //批量调整弹窗（Controls/ExpiryTime · LimitLinks · LimitDevices）
  'acct.adj.expiry': { zh: '过期时间', en: 'Expiry' },
  'acct.adj.links': { zh: '链接数', en: 'Links' },
  'acct.adj.devices': { zh: '设备数', en: 'Devices' },
  'acct.adj.scope': { zh: '将对选中的 {0} 个账号生效', en: 'Applies to {0} selected account(s)' },
  'acct.adj.add': { zh: '增加时长', en: 'Add time' },
  'acct.adj.amount': { zh: '增加', en: 'Add' },
  'acct.adj.hour': { zh: '小时', en: 'Hours' },
  'acct.adj.day': { zh: '天', en: 'Days' },
  'acct.adj.base': { zh: '从哪算起', en: 'Starting from' },
  'acct.adj.fromExpiry': { zh: '原有到期时间', en: 'Current expiry' },
  'acct.adj.fromNow': { zh: '当前时间', en: 'Now' },
  'acct.adj.baseHint': {
    zh: '两者只在已过期的账号上不同：选「当前时间」会让它们从此刻重新起算。',
    en: 'These differ only for already-expired accounts: “Now” restarts their clock from this moment.',
  },
  'acct.adj.limitOn': { zh: '启用限制', en: 'Enable limit' },

  'acct.op.edit': { zh: '编辑', en: 'Edit' },
  'acct.op.logins': { zh: '登录记录', en: 'Login history' },
  'acct.op.del': { zh: '删除', en: 'Delete' },

  //编辑弹窗，字段照 Controls/AccountEdit
  'acct.e.user': { zh: '用户名', en: 'Username' },
  'acct.e.pass': { zh: '密码', en: 'Password' },
  'acct.e.passKeep': { zh: '留空不改', en: 'Blank = unchanged' },
  'acct.e.enable': { zh: '启用此账号', en: 'Account enabled' },
  'acct.e.expiry': { zh: '设置过期时间', en: 'Set expiry' },
  'acct.e.userLocked': { zh: '不可修改', en: 'Read-only' },
  'acct.e.lead': { zh: 'SOCKS5 登录凭据', en: 'SOCKS5 credentials' },
  'acct.e.limits': { zh: '限制', en: 'Limits' },

  //批量创建（Controls/BatchAccounts）
  'acct.b.rule': { zh: '生成规则', en: 'Rules' },
  'acct.b.naming': { zh: '用户名', en: 'Username' },
  'acct.b.byTime': { zh: '时间 + 序号', en: 'Time + No.' },
  'acct.b.byPrefix': { zh: '前缀 + 序号', en: 'Prefix + No.' },
  'acct.b.prefixPh': { zh: '前缀', en: 'Prefix' },
  'acct.b.count': { zh: '数量', en: 'Count' },
  'acct.b.passLen': { zh: '密码长度', en: 'Password length' },
  'acct.b.preview': { zh: '预览', en: 'Preview' },
  'acct.b.gen': { zh: '生成', en: 'Generate' },
  'acct.b.needPreview': { zh: '先点「生成」', en: 'Click Generate first' },
  'acct.b.dup': { zh: '{0} 个用户名已存在，保存时会跳过', en: '{0} usernames already exist and will be skipped' },
  'acct.b.saved': { zh: '已创建 {0} 个账号', en: '{0} accounts created' },
  'acct.b.savedSome': { zh: '已创建 {0} 个，跳过 {1} 个重名', en: '{0} created, {1} skipped (duplicate)' },

  //登录记录弹窗
  'acct.lg.title': { zh: '账号登录情况', en: 'Login history' },
  'acct.lg.time': { zh: '登录时间', en: 'Time' },
  'acct.lg.ip': { zh: '登录 IP', en: 'IP' },
  'acct.lg.loc': { zh: '归属地', en: 'Location' },
  'acct.lg.empty': { zh: '这个账号还没有登录记录', en: 'No login records for this account' },

  // ── 系统日志 ────────────────────────────────────────
  //三路日志的名字照 Controls/LogList 的三个标签页
  'log.sys': { zh: '系统日志', en: 'System Log' },
  'log.filter': { zh: '滤镜日志', en: 'Filter Log' },
  'log.proxy': { zh: '代理日志', en: 'Proxy Log' },
  'log.empty': { zh: '暂无日志', en: 'No log entries yet' },
  //列名照 LogList.cs 里那三组 AntdUI.Column
  'log.module': { zh: '模块', en: 'Module' },
  'log.content': { zh: '日志内容', en: 'Content' },
  'log.filterName': { zh: '滤镜名称', en: 'Filter' },
  'log.action': { zh: '动作', en: 'Action' },
  'log.matchNum': { zh: '匹配数', en: 'Matches' },
  'log.account': { zh: '账号', en: 'Account' },
  'log.ip': { zh: 'IP地址', en: 'IP Address' },
  //日志自己的自动清理条数（与封包列表那份是两套配置，见 SystemLog.vue）

  // ── 代理模式：指标板 ────────────────────────────────

  /*
    协议（ProxyConfig.Proxy.DomainType）。

    <b>WinForms 的 ProxyList 没有这一列</b>（那边 13 列里没有它），所以没有现成译法可抄。
    这里的口径：协议名本身不翻（HTTP / HTTPS / WebSocket 是通用写法，翻了反而难认），
    只翻那两个是普通英文词的：
      Socket   -> 套接字（与「套接字」那一列同一个词，Localizer.cs 的官方英文就是 Socket）
      External -> 外部代理（对应「外部代理设置」，官方英文 EXTProxy）
  */
  'dt.socket': { zh: '套接字', en: 'Socket' },
  'dt.http': { zh: 'HTTP', en: 'HTTP' },
  'dt.https': { zh: 'HTTPS', en: 'HTTPS' },
  'dt.external': { zh: '外部代理', en: 'EXTProxy' },
  'dt.websocket': { zh: 'WebSocket', en: 'WebSocket' },

  /*
    封包类型。<b>逐条照抄 Operate.PacketConfig.Packet.PacketTypeNames</b>
    （中文取那里的兜底串，英文取 Localizer.cs 的 HookSettingsForm.* 词条），
    别自己另译 —— 这些是产品术语，两套 UI 必须一模一样。

    注意 WSARecvEx 与 WSARecv 在 C# 侧共用 HookSettingsForm.WSARecv 一个键，
    显示成同一个词，这里照做。
  */
  'pt.ws1Send': { zh: '发送 1.1', en: 'Send 1.1' },
  'pt.ws2Send': { zh: '发送', en: 'Send' },
  'pt.ws1SendTo': { zh: '发送到 1.1', en: 'SendTo 1.1' },
  'pt.ws2SendTo': { zh: '发送到', en: 'SendTo' },
  'pt.ws1Recv': { zh: '接收 1.1', en: 'Recv 1.1' },
  'pt.ws2Recv': { zh: '接收', en: 'Recv' },
  'pt.ws1RecvFrom': { zh: '接收自 1.1', en: 'RecvFrom 1.1' },
  'pt.ws2RecvFrom': { zh: '接收自', en: 'RecvFrom' },
  'pt.wsaSend': { zh: 'WSA发送', en: 'WSASend' },
  'pt.wsaSendTo': { zh: 'WSA发送到', en: 'WSASendTo' },
  'pt.wsaRecv': { zh: 'WSA接收', en: 'WSARecv' },
  'pt.wsaRecvFrom': { zh: 'WSA接收自', en: 'WSARecvFrom' },
  'pt.tcpReq': { zh: 'TCP 请求', en: 'TCP Req' },
  'pt.udpReq': { zh: 'UDP 请求', en: 'UDP Req' },
  'pt.tcpResp': { zh: 'TCP 响应', en: 'TCP Res' },
  'pt.udpResp': { zh: 'UDP 响应', en: 'UDP Res' },
  'pt.httpReq': { zh: 'HTTP 请求', en: 'HTTP Req' },
  'pt.httpResp': { zh: 'HTTP 响应', en: 'HTTP Res' },
  'pt.httpsReq': { zh: 'HTTPS 请求', en: 'HTTPS Req' },
  'pt.httpsResp': { zh: 'HTTPS 响应', en: 'HTTPS Res' },
  'pt.wsReq': { zh: 'WebSocket 请求', en: 'WebSocket Req' },
  'pt.wsResp': { zh: 'WebSocket 响应', en: 'WebSocket Res' },

  // ── 代理模式：数据列表 ──────────────────────────────
  //与 WinForms 的 cID.HeaderText / cPacketSocket.HeaderText 一致
  'col.id': { zh: '序号', en: 'No.' },

  /* ── 发送列表（对应 Controls/SendList）────────────────────── */
  'snd.add': { zh: '新增发送', en: 'Add Send' },
  'snd.import': { zh: '导入', en: 'Import' },
  'snd.export': { zh: '导出', en: 'Export' },
  'snd.clearAll': { zh: '清空', en: 'Clear' },
  'snd.resetCount': { zh: '重置计数', en: 'Reset counts' },
  'snd.enableAll': { zh: '全部启用', en: 'Enable all' },
  'snd.disableAll': { zh: '全部禁用', en: 'Disable all' },
  'snd.start': { zh: '开始发送', en: 'Start' },
  'snd.stop': { zh: '停止', en: 'Stop' },
  'snd.runHint': {
    zh: '按列表顺序执行已启用的发送；执行方式（依次 / 同时）在系统设置里。',
    en: 'Runs the enabled sends in list order. Sequential or simultaneous is set in System Settings.',
  },
  'snd.empty': {
    zh: '还没有发送。新增一个，把要重放的封包放进它的发送集。',
    en: 'No sends yet. Add one and put the packets to replay into its collection.',
  },
  'snd.noPackets': { zh: '发送集是空的，执行时不会发出任何封包', en: 'Empty collection — this send will emit nothing' },
  'snd.socketCustom': { zh: '自定义', en: 'Customize' },
  'snd.loopTimes': { zh: '次', en: 'x' },
  'snd.loopMs': { zh: '毫秒', en: 'ms' },
  /* ── 发送编辑（对应 Controls/SendEdit）──────────────────── */
  'snd.e.title': { zh: '发送编辑', en: 'Send Editor' },
  'snd.e.gone': { zh: '这条发送已经不在列表里了', en: 'This send is no longer in the list' },
  'snd.e.namePh': { zh: '给这条发送起个名字', en: 'Name this send' },
  'snd.e.useSysSocket': { zh: '使用系统套接字', en: 'Use system socket' },
  'snd.e.sysSocketUnset': { zh: '未设置', en: 'not set' },
  'snd.e.socketHint': {
    zh: '不勾就用每条封包自己记录的套接字',
    en: 'Otherwise each packet uses the socket it was captured on',
  },
  'snd.e.interval': { zh: '间隔', en: 'interval' },
  'snd.e.intervalHint': {
    zh: '间隔为 0 时不汇报进度，当前行不会高亮',
    en: 'With interval 0 no progress is reported — the current row will not highlight',
  },
  'snd.e.execute': { zh: '执行', en: 'Execute' },
  'snd.e.done': { zh: '发送执行完毕', en: 'Send finished' },
  'snd.e.import': { zh: '导入发送集', en: 'Import collection' },
  'snd.e.export': { zh: '导出发送集', en: 'Export collection' },
  'snd.e.clear': { zh: '清空发送集', en: 'Clear collection' },
  'snd.e.from': { zh: '本机地址', en: 'Local' },
  'snd.e.to': { zh: '远端地址', en: 'Remote' },
  'snd.e.empty': {
    zh: '发送集是空的。在封包列表里右键「添加到发送」，或从文件导入。',
    en: 'Empty collection. Add packets from the packet list, or import from a file.',
  },


  'col.sendName': { zh: '发送名称', en: 'Send Name' },
  'col.success': { zh: '成功', en: 'Success' },
  'col.fail': { zh: '失败', en: 'Fail' },
  'col.loop': { zh: '循环', en: 'Loop' },
  'col.packets': { zh: '封包', en: 'Packets' },
  'col.notes': { zh: '备注', en: 'Notes' },
  'col.socket': { zh: '套接字', en: 'Socket' },
  'col.time': { zh: '时间', en: 'Time' },
  'col.type': { zh: '类型', en: 'Type' },
  'col.proto': { zh: '协议', en: 'Protocol' },
  //注入模式的封包列表：PacketInfo 只有「本机 / 远端」两侧，没有客户端 / 服务端之分。
  //文案照 Localizer.cs 的 PacketList 那几列，不另起译法。
  //── 注入模式 ─────────────────────────────────────
  'foot.hooking': { zh: '拦截中', en: 'Hooking' },

  //侧栏：注入模式的 11 页。除主屏外的 10 页与代理模式同名，直接复用 proxy.nav.* 那几个键
  'inject.nav.packet': { zh: '封包列表', en: 'Packet List' },

  'inject.pick.title': { zh: '选择注入目标', en: 'Select Target' },
  'inject.pick.lede': {
    zh: '双击一行注入到已运行的进程；或用「启动并注入」从头拉起一个 —— 后者会在钩子就位之后才唤醒它，连第一个封包都抓得到。',
    en: 'Double-click a row to inject into a running process, or use Launch & Inject to start one suspended — it is woken only after the hooks are in place, so even the first packet is captured.',
  },
  'inject.pick.search': { zh: '搜索进程名或 PID', en: 'Search name or PID' },
  'inject.pick.refresh': { zh: '刷新', en: 'Refresh' },
  'inject.pick.loading': { zh: '枚举中…', en: 'Enumerating…' },
  'inject.pick.launch': { zh: '启动并注入', en: 'Launch & Inject' },
  'inject.pick.attach': { zh: '注入', en: 'Inject' },
  'inject.pick.none': { zh: '没有匹配的进程', en: 'No matching process' },

  //「选择窗体」：装低级鼠标钩子，在屏幕上点哪个窗口就选中哪个进程
  'inject.pick.window': { zh: '选择窗体', en: 'Pick Window' },
  'inject.pick.picking': { zh: '正在选择窗体', en: 'Picking a window' },
  'inject.pick.pickHint': {
    zh: '把鼠标移到目标程序的窗口上，点一下就选中它；按 Esc 或点回本窗口取消。',
    en: 'Move the cursor over the target program and click to select it; press Esc or click back here to cancel.',
  },
  'inject.pick.cancel': { zh: '取消选择', en: 'Cancel' },
  'inject.pick.hoverNone': { zh: '把鼠标移到别的窗口上…', en: 'Hover another window…' },
  'inject.pick.noTitle': { zh: '无标题', en: 'Untitled' },
  'inject.pick.last': { zh: '上次注入', en: 'Last target' },

  'inject.col.name': { zh: '进程名', en: 'Process' },
  'inject.col.pid': { zh: 'PID', en: 'PID' },
  'inject.col.path': { zh: '路径', en: 'Path' },

  'inject.target': { zh: '目标', en: 'Target' },
  'inject.window': { zh: '窗口', en: 'Window' },
  'inject.rate': { zh: '速率', en: 'Rate' },
  'inject.rows': { zh: '条', en: 'rows' },
  'inject.dropped': { zh: '丢弃', en: 'Dropped' },
  'inject.startHook': { zh: '开始拦截', en: 'Start Hook' },
  'inject.stopHook': { zh: '停止拦截', en: 'Stop Hook' },
  'inject.clear': { zh: '清空', en: 'Clear' },
  'inject.detach': { zh: '断开', en: 'Detach' },

  //统计条 14 格。八个 WinSock 计数直接用 pt.* 那一组（与过滤设置的类别同一套口径）
  'inject.st.total': { zh: '封包总数', en: 'Total packets' },
  'inject.st.filterExec': { zh: '滤镜执行', en: 'Filter runs' },
  'inject.st.filtered': { zh: '已过滤', en: 'Filtered out' },
  'inject.st.queue': { zh: '待入列', en: 'Queued' },
  'inject.st.dropped': { zh: '丢弃', en: 'Dropped' },
  'inject.st.rate': { zh: '实时速率', en: 'Live rate' },

  'inject.state.ok': { zh: '已附加', en: 'Attached' },
  'inject.state.lost': { zh: '已断开', en: 'Disconnected' },

  'inject.attached': { zh: '已附加到目标', en: 'Attached to target' },
  'inject.launched': { zh: '已启动并注入', en: 'Launched and injected' },
  'inject.failed': { zh: '注入失败', en: 'Injection failed' },
  'inject.lost': { zh: '目标已退出', en: 'Target exited' },
  'inject.lostHint': {
    zh: '目标进程已经不在了。已抓到的封包与配置全部保留，可以继续查看和导出。',
    en: 'The target process is gone. Everything captured so far is kept — you can still browse and export it.',
  },

  'col.from': { zh: '本机地址', en: 'Local' },
  'col.fromLoc': { zh: '本机所属地', en: 'Local Loc.' },
  'col.to': { zh: '远端地址', en: 'Remote' },
  'col.toLoc': { zh: '远端所属地', en: 'Remote Loc.' },

  'col.client': { zh: '客户端', en: 'Client' },
  'col.clientLoc': { zh: '客户端地', en: 'Client Loc.' },
  'col.server': { zh: '服务端', en: 'Server' },
  'col.serverLoc': { zh: '服务端地', en: 'Server Loc.' },
  'col.domain': { zh: '域名', en: 'Domain' },
  'col.len': { zh: '长度', en: 'Length' },
  'col.data': { zh: '数据', en: 'Data' },
  'col.resizeHint': { zh: '拖动调整列宽，双击恢复默认', en: 'Drag to resize, double-click to reset' },

  // ── 封包编辑（PacketEdit）────────────────────────────
  'pe.title': { zh: '封包编辑', en: 'Packet Editor' },
  'pe.gone': { zh: '这条封包已经不在列表里了', en: 'This packet is no longer in the list' },
  'pe.socket': { zh: '使用套接字', en: 'Socket' },
  'pe.socketHint': {
    zh: '0 = 按抓到它的会话回发（仅 HTTP / HTTPS / WebSocket 中间人抓到的包）；否则填要发出去的套接字号',
    en: '0 = reply through the session it was captured on (MITM packets only); otherwise the socket to send on',
  },
  'pe.noSession': { zh: '这条包没有会话号，套接字填 0 发不出去', en: 'No session on this packet — socket 0 will not send' },
  'pe.useSys': { zh: '用系统套接字', en: 'Use system socket' },
  'pe.to': { zh: '远端地址', en: 'Remote' },
  'pe.from': { zh: '本机地址', en: 'Local' },
  'pe.len': { zh: '长度', en: 'Length' },
  'pe.send': { zh: '发送', en: 'Send' },
  'pe.stop': { zh: '停止', en: 'Stop' },
  'pe.byTimes': { zh: '按次发送', en: 'Send N times' },
  'pe.continuous': { zh: '连续发送', en: 'Send continuously' },
  'pe.times': { zh: '次', en: 'x' },
  'pe.interval': { zh: '间隔', en: 'interval' },
  'pe.ms': { zh: '毫秒', en: 'ms' },
  'pe.sendTotal': { zh: '发送总数', en: 'Sent' },
  'pe.sendOk': { zh: '成功', en: 'OK' },
  'pe.sendFail': { zh: '失败', en: 'Failed' },
  'pe.sendDone': { zh: '发送完毕', en: 'Send finished' },
  'pe.prog': { zh: '递进', en: 'Step' },
  'pe.progOn': { zh: '启用递进', en: 'Enable stepping' },
  'pe.progPos': { zh: '位置', en: 'at byte' },
  'pe.progStep': { zh: '步长', en: 'step' },
  'pe.progCarry': { zh: '进位递进', en: 'Carry' },
  'pe.progCarryN': { zh: '进位位数', en: 'carry bytes' },
  'pe.progHint': {
    zh: '每发一次，指定位置的字节加上步长；点编辑器里的字节就是选位置。',
    en: 'Each send adds the step to the byte at the position; click a byte in the editor to pick it.',
  },
  'pe.progCarryHint': {
    zh: '进位递进 = 溢出时往前面的字节进位，进位位数是最多往前进几个字节。',
    en: 'Carry = overflow rolls into the preceding bytes; carry bytes is how many bytes it may roll into.',
  },
  'pe.hex': { zh: '十六进制', en: 'Hex' },
  'pe.ascii': { zh: '文本', en: 'Text' },
  'pe.insert': { zh: '插入', en: 'INS' },
  'pe.overwrite': { zh: '覆盖', en: 'OVR' },
  'pe.cursor': { zh: '光标', en: 'Cursor' },
  'pe.selected': { zh: '已选', en: 'Selected' },
  'pe.bytes': { zh: '字节', en: 'bytes' },
  'pe.keysHint': {
    zh: '直接键入十六进制或文本改写；Insert 切换插入 / 覆盖；Backspace / Delete 删字节；Ctrl+A / C / X / V',
    en: 'Type hex or text to overwrite; Insert toggles insert / overwrite; Backspace / Delete remove bytes; Ctrl+A / C / X / V',
  },
  'pe.empty': { zh: '封包是空的，直接键入字节', en: 'Packet is empty — type bytes to add' },
  'pe.m.toFilter': { zh: '添加到滤镜列表', en: 'Add to filters' },
  'pe.m.toSend': { zh: '添加到发送', en: 'Add to send' },
  'pe.m.cut': { zh: '剪切', en: 'Cut' },
  'pe.m.copyText': { zh: '复制文本', en: 'Copy text' },
  'pe.m.copyHex': { zh: '复制十六进制', en: 'Copy hex' },
  'pe.m.pasteText': { zh: '粘贴文本', en: 'Paste text' },
  'pe.m.pasteHex': { zh: '粘贴十六进制', en: 'Paste hex' },
  'pe.m.selectAll': { zh: '全选', en: 'Select all' },
  'pe.pasteEmpty': { zh: '剪贴板里没有可粘贴的内容', en: 'Nothing to paste' },
  'pe.pasteBadHex': { zh: '剪贴板里的不是十六进制', en: 'Clipboard is not hex' },
  'pe.saved': { zh: '封包保存成功', en: 'Packet saved' },
  'pe.toFilterOk': { zh: '已添加到滤镜列表', en: 'Added to filters' },
  'pe.toFilterFail': { zh: '添加到滤镜列表出错', en: 'Failed to add to filters' },
  'pe.toSendOk': { zh: '已添加到', en: 'Added to' },
  'pe.toSendFail': { zh: '添加到发送列表出错', en: 'Failed to add to send' },
  'pm.edit': { zh: '编辑', en: 'Edit' },
  'pm.modify': { zh: '查看数据修改', en: 'View changes' },

  // ── 查看数据修改（PacketModification）──────────────
  'pmd.title': { zh: '查看数据修改', en: 'Packet Changes' },
  'pmd.raw': { zh: '原始封包数据', en: 'Original' },
  'pmd.new': { zh: '修改后封包数据', en: 'Modified' },
  'pmd.len': { zh: '长度', en: 'length' },
  'pmd.same': { zh: '改写前后逐字节相同，没有滤镜动过这个封包', en: 'Byte-identical before and after — no filter touched this packet' },
  'pmd.diffs': { zh: '差异', en: 'Differences' },
  'pmd.pos': { zh: '位置', en: 'Offset' },
  'pmd.count': { zh: '字节数', en: 'Bytes' },
  'pmd.from': { zh: '原值', en: 'Original' },
  'pmd.to': { zh: '新值', en: 'Modified' },
  'pmd.kind': { zh: '变更类型', en: 'Change' },
  'pmd.modified': { zh: '修改', en: 'Modified' },
  'pmd.inserted': { zh: '新增', en: 'Inserted' },
  'pmd.deleted': { zh: '删除', en: 'Deleted' },
  'pmd.jumpHint': { zh: '点一行在上面高亮那一段', en: 'Click a row to highlight that range above' },
  'pmd.algoHint': {
    zh: '按位置逐字节比对（与上方高亮同一算法）：长度不同时多出或少掉的那一截记为新增 / 删除',
    en: 'Byte-by-byte by offset (same rule as the highlight above); when lengths differ the tail is reported as inserted / deleted',
  },

  // ── 工具页：文本对比（术语照 Localizer.cs 的 ComparisonText.*）──
  'tc.modeDiff': { zh: '文本比较', en: 'Compare' },
  'tc.modeDup': { zh: '文本查重', en: 'Duplicates' },
  'tc.regexPh': { zh: '正则表达式，输入即高亮命中', en: 'Regex — matches highlight as you type' },
  'tc.leach': { zh: '过滤', en: 'Leach' },
  'tc.minBytes': { zh: '最少字节', en: 'Min bytes' },
  'tc.runDiff': { zh: '比较', en: 'Compare' },
  'tc.runDup': { zh: '查重', en: 'Find duplicates' },
  'tc.store': { zh: '暂存文本', en: 'Store text' },
  'tc.reset': { zh: '还原', en: 'Reset' },
  'tc.clear': { zh: '清空', en: 'Clear' },
  'tc.stored': { zh: '已暂存，「还原」可以退回这一份', en: 'Stored — Reset brings this copy back' },
  'tc.nothingStored': { zh: '还没有暂存过', en: 'Nothing stored yet' },
  'tc.needBoth': { zh: '两边都要有十六进制数据才能查重', en: 'Both sides need hex data to find duplicates' },
  'tc.needRegex': { zh: '先输入正则表达式', en: 'Enter a regex first' },
  'tc.textA': { zh: '文本 A', en: 'Text A' },
  'tc.textB': { zh: '文本 B', en: 'Text B' },
  'tc.length': { zh: '长度', en: 'Length' },
  'tc.marks': { zh: '标记', en: 'Marks' },
  'tc.phA': { zh: '在这里粘贴文本 A，或在封包列表右键「添加到文本 A」', en: 'Paste text A here, or right-click a packet → Add to text A' },
  'tc.phB': { zh: '在这里粘贴文本 B，或在封包列表右键「添加到文本 B」', en: 'Paste text B here, or right-click a packet → Add to text B' },
  'tc.position': { zh: '位置', en: 'Position' },
  'tc.valueA': { zh: 'A 值', en: 'Value A' },
  'tc.valueB': { zh: 'B 值', en: 'Value B' },
  'tc.changeType': { zh: '变更类型', en: 'Type' },
  'tc.inserted': { zh: '新增', en: 'Inserted' },
  'tc.deleted': { zh: '删除', en: 'Deleted' },
  'tc.modified': { zh: '修改', en: 'Modified' },
  'tc.sequence': { zh: '重复值', en: 'Duplicate value' },
  'tc.countA': { zh: 'A 次数', en: 'Count in A' },
  'tc.countB': { zh: 'B 次数', en: 'Count in B' },
  'tc.posA': { zh: 'A 位置', en: 'Positions in A' },
  'tc.posB': { zh: 'B 位置', en: 'Positions in B' },
  'tc.emptyDiff': { zh: '按「比较」逐字符对齐两段文本，差异会列在这里，点一行两边一起定位。', en: 'Press Compare to align both texts character by character; differences list here, click a row to locate it on both sides.' },
  'tc.emptyDup': { zh: '按「查重」找出两段十六进制里共同的字节序列，两边会整理成 AA BB CC 的形态并高亮重复部分。', en: 'Press Find duplicates to find byte sequences common to both hex texts; both sides are normalised to AA BB CC and the repeats highlighted.' },
  'tc.more': { zh: '只列出前 3000 条，差异总数', en: 'Showing the first 3000 rows; total differences' },

  // ── 工具页：异或计算 ──
  'xo.key': { zh: '异或值', en: 'XOR value' },
  'xo.keyPh': { zh: '十六进制，空格分隔，支持循环异或', en: 'Hex bytes separated by spaces; cycles when shorter than the data' },
  'xo.run': { zh: '计算', en: 'Calculate' },
  'xo.pasteHex': { zh: '粘贴十六进制', en: 'Paste hex' },
  'xo.copyOut': { zh: '复制结果', en: 'Copy result' },
  'xo.useOut': { zh: '结果作为输入', en: 'Use result as input' },
  'xo.src': { zh: '原始数据', en: 'Source' },
  'xo.srcHint': { zh: '可直接编辑；右键可粘贴十六进制或文本', en: 'Editable; right-click to paste hex or text' },
  'xo.out': { zh: '计算结果', en: 'Result' },
  'xo.emptySrc': { zh: '还没有数据。按「粘贴十六进制」从剪贴板载入，或在封包编辑里复制一段过来。', en: 'No data yet. Press Paste hex to load from the clipboard, or copy bytes from the packet editor.' },
  'xo.emptyOut': { zh: '填好异或值，按「计算」。', en: 'Enter the XOR value and press Calculate.' },
  'xo.srcEmpty': { zh: '原始数据为空', en: 'Source data is empty' },
  'xo.keyEmpty': { zh: '异或值为空', en: 'XOR value is empty' },
  'xo.keyBad': { zh: '异或值不是十六进制', en: 'XOR value is not hex' },
  'xo.clipNoHex': { zh: '剪贴板里没有十六进制数据', en: 'No hex data on the clipboard' },

  // ── 工具页：编码转换 ──
  'tr.encode': { zh: '编码', en: 'Encode' },
  'tr.decode': { zh: '解码', en: 'Decode' },
  'tr.hint': { zh: '编码：文本 → 各编码的字节；解码：十六进制 / 文本 → 各编码解回的字符串。Ctrl+Enter 编码。', en: 'Encode: text → bytes per encoding; Decode: hex / text → strings per encoding. Ctrl+Enter encodes.' },
  'tr.encResult': { zh: '编码结果', en: 'Encoded' },
  'tr.decResult': { zh: '解码结果', en: 'Decoded' },
  'tr.input': { zh: '输入', en: 'Input' },
  'tr.inputPh': { zh: '请输入文本；解码时可以是带空格的十六进制', en: 'Enter text; for decoding this can be space-separated hex' },
  'tr.results': { zh: '结果', en: 'Results' },
  'tr.empty': { zh: '请输入文本', en: 'Please enter text' },
  'tr.emptyOut': { zh: '按「编码」或「解码」，14 种结果会列在这里。', en: 'Press Encode or Decode; the 14 results list here.' },
  'tr.useAsInput': { zh: '放回输入框', en: 'Use as input' },

  // ── 工具页：数据提取 ──
  'ex.k0': { zh: 'Charles XML 会话（.chlsx）→ 十六进制数据', en: 'Charles XML session (.chlsx) → hex data' },
  'ex.k1': { zh: 'FILT 过滤器（.filt）→ WPE64 滤镜列表（.fp）', en: 'FILT filter (.filt) → WPE64 filter list (.fp)' },
  'ex.k2': { zh: 'WPE 账号文件（.pa）→ CCProxy 账号文件（.ini）', en: 'WPE account file (.pa) → CCProxy account file (.ini)' },
  'ex.pick': { zh: '选择文件', en: 'Choose file' },
  'ex.save': { zh: '生成文件', en: 'Generate' },
  'ex.drop': { zh: '单击或拖动文件到此区域进行数据提取', en: 'Click or drop a file here to extract' },
  'ex.dropAgain': { zh: '再拖一个文件进来会替换当前结果', en: 'Drop another file here to replace the current result' },
  'ex.dropHint': { zh: '接受', en: 'Accepts' },
  'ex.to': { zh: '生成', en: 'generates' },
  'ex.wrongExt': { zh: '这种提取类型需要的文件是', en: 'This extraction type expects a file of type' },
  'ex.extracted': { zh: '数据提取成功', en: 'Data extracted' },
  'ex.empty': { zh: '提取数据为空', en: 'Extracted data is empty' },
  'ex.result': { zh: '提取结果', en: 'Result' },
  'ex.lines': { zh: '行数', en: 'Lines' },
  'ex.editHint': { zh: '这里可以直接修改，改完按「生成文件」导出对应格式的数据文件。', en: 'Edit freely here, then press Generate to export the file in the target format.' },

  // ── 统计数据 ──
  'st.refresh': { zh: '刷新数据', en: 'Refresh' },
  'st.autoHint': { zh: '页面开着时每秒自动刷新', en: 'Refreshes every second while open' },
  //注入模式下分母是封包总数，不是代理总数（GetFilterStats 按 SelectMode 取）
  'st.packetTotal': { zh: '封包总数', en: 'Packet total' },
  'st.proxyTotal': { zh: '代理总数', en: 'Proxy total' },
  'st.filterExec': { zh: '滤镜执行', en: 'Filter runs' },
  'st.execute': { zh: '滤镜执行占比', en: 'Filter hit rate' },
  'st.executeHint': { zh: '滤镜执行次数 ÷ 代理封包总数', en: 'filter runs ÷ proxied packets' },
  'st.executeHintInject': { zh: '滤镜执行次数 ÷ 拦截封包总数', en: 'filter runs ÷ hooked packets' },
  'st.ofExec': { zh: '占滤镜执行次数', en: 'of filter runs' },
  'st.status': { zh: '状态', en: 'Status' },
  'st.share': { zh: '占比', en: 'Share' },
  'st.working': { zh: '处理中', en: 'Working' },
  'st.stopped': { zh: '停止', en: 'Off' },
  'st.emptyFilters': { zh: '还没有滤镜。', en: 'No filters yet.' },

  // ── WPC 配置（术语照 Localizer.cs 的 WPCConfig.*）──
  'wpc.servers': { zh: '服务器列表', en: 'Servers' },
  'wpc.notices': { zh: '公告列表', en: 'Notices' },
  'wpc.addServer': { zh: '新增服务器', en: 'Add server' },
  'wpc.addNotice': { zh: '新增公告', en: 'Add notice' },
  'wpc.clearServers': { zh: '清空所有服务器', en: 'Clear servers' },
  'wpc.clearNotices': { zh: '清空所有公告', en: 'Clear notices' },
  'wpc.serverHint': { zh: '下发给 WPE Proxy Cap 的可用节点，每台服务器带一组 Clash 规则', en: 'Nodes served to WPE Proxy Cap; each server carries a set of Clash rules' },
  'wpc.noticeHint': { zh: '下发给 WPE Proxy Cap 的公告', en: 'Notices served to WPE Proxy Cap' },
  'wpc.serverName': { zh: '服务器名称', en: 'Server name' },
  'wpc.serverNamePh': { zh: '请输入服务器名称', en: 'Server name' },
  'wpc.serverAddr': { zh: '服务器地址', en: 'Address' },
  'wpc.forgotUrl': { zh: '找回密码地址', en: 'Forgot URL' },
  'wpc.registerUrl': { zh: '立即注册地址', en: 'Register URL' },
  'wpc.verifyUrl': { zh: '验证地址', en: 'Verify URL' },
  'wpc.grp.urls': { zh: '客户端链接', en: 'Client links' },
  'wpc.urlHint': { zh: '三个地址显示在 WPE Proxy Cap 的登录界面上，可以留空。', en: 'The three links show on the WPE Proxy Cap sign-in screen; they can be left empty.' },
  'wpc.rules': { zh: '规则', en: 'Rules' },
  'wpc.editRules': { zh: '规则集', en: 'Rules' },
  'wpc.rulesOf': { zh: '规则集', en: 'Rules' },
  'wpc.ruleAdd': { zh: '新增规则', en: 'Add rule' },
  'wpc.ruleEdit': { zh: '编辑规则', en: 'Edit rule' },
  'wpc.ruleType': { zh: '类型', en: 'Type' },
  'wpc.ruleAction': { zh: '动作', en: 'Action' },
  'wpc.ruleArg': { zh: '参数', en: 'Argument' },
  'wpc.ruleArgPh': { zh: '多个参数用分号分隔，一次插入多条', en: 'Separate several arguments with ; to add them at once' },
  'wpc.ruleInsert': { zh: '插入', en: 'Insert' },
  'wpc.ruleNew': { zh: '改为新增', en: 'New instead' },
  'wpc.ruleHint': { zh: '点表里一行就把它装进上面改；MATCH 这类不带参数的留空即可。', en: 'Click a row to edit it above; rules like MATCH take no argument.' },
  'wpc.ruleCount': { zh: '条规则', en: 'rules' },
  'wpc.clearRules': { zh: '清空规则', en: 'Clear rules' },
  'wpc.emptyRules': { zh: '这台服务器还没有规则。', en: 'This server has no rules yet.' },
  'wpc.emptyServers': { zh: '还没有服务器。新增一台，填上 WPE x64 代理服务的地址，客户端就能拿到它。', en: 'No servers yet. Add one with the address of this WPE x64 proxy and clients will receive it.' },
  'wpc.emptyNotices': { zh: '还没有公告。', en: 'No notices yet.' },
  'wpc.noticeType': { zh: '公告类型', en: 'Type' },
  'wpc.noticeTitle': { zh: '标题', en: 'Title' },
  'wpc.noticeTitlePh': { zh: '请输入公告标题', en: 'Notice title' },
  'wpc.noticeContent': { zh: '内容', en: 'Content' },
  'wpc.noticeContentPh': { zh: '请输入公告内容', en: 'Notice content' },
  'wpc.noticeMore': { zh: '更多详情链接', en: 'More link' },
  'wpc.noticeTime': { zh: '发布时间', en: 'Published' },
  'wpc.noticeTimeHint': { zh: '发布时间按保存的那一刻记，每改一次就刷新一次。', en: 'Published time is taken when saved and refreshes on every edit.' },
  'wpc.nt1': { zh: '活动情报', en: 'Event' },
  'wpc.nt2': { zh: '维护说明', en: 'Maintenance' },
  'wpc.nt3': { zh: '电竞赛事', en: 'Esports' },
  'wpc.nt4': { zh: '限时商城', en: 'Shop' },
  'wpc.nt5': { zh: '玩家社区', en: 'Community' },

  // ── 进程设置 ──
  'ps.driver': { zh: '驱动', en: 'Driver' },
  'ps.driverType': { zh: '驱动类型', en: 'Driver type' },
  'ps.driverLoaded': { zh: '已加载', en: 'Loaded' },
  'ps.driverNotLoaded': { zh: '未加载', en: 'Not loaded' },
  'ps.uninstall': { zh: '卸载驱动', en: 'Uninstall driver' },
  'ps.tipNfapi': { zh: 'NFAPI：限制 1000000 个 TCP 连接和 UDP 套接字，超过后需要重启才能继续拦截。', en: 'NFAPI: limited to 1,000,000 TCP connections and UDP sockets; restart to continue past that.' },
  'ps.tipProxifier': { zh: 'Proxifier：不支持 UDP，不支持 32 位操作系统。', en: 'Proxifier: no UDP, no 32-bit Windows.' },
  'ps.tipWinDivert': { zh: 'WinDivert：不支持拦截 127.0.0.1 的数据。', en: 'WinDivert: cannot intercept 127.0.0.1 traffic.' },
  'ps.processes': { zh: '拦截的进程', en: 'Processes' },
  'ps.byPid': { zh: '按进程编号拦截', en: 'By process ID' },
  'ps.byName': { zh: '按进程名称拦截', en: 'By process name' },
  'ps.byPidHint': { zh: '勾选 = 按编号拦截（进程重启编号会变）；双击一行加到右边按名称拦截。', en: 'Check = intercept by ID (IDs change on restart); double-click a row to add it by name on the right.' },
  'ps.byNameHint': { zh: '按名称拦截的进程重启后仍然生效；双击或点 × 删除。', en: 'Name-based entries survive restarts; double-click or × to remove.' },
  'ps.filterPh': { zh: '筛选进程', en: 'Filter' },
  'ps.refresh': { zh: '刷新进程', en: 'Refresh' },
  'ps.pid': { zh: '编号', en: 'PID' },
  'ps.processName': { zh: '进程名称', en: 'Process' },
  'ps.moduleName': { zh: '模块名称', en: 'Module' },
  'ps.emptyProcs': { zh: '没有匹配的进程。', en: 'No matching processes.' },
  'ps.emptyNames': { zh: '还没有按名称拦截的进程。', en: 'No name-based entries yet.' },
  'ps.noModule': { zh: '这个进程取不到模块名，只能按编号拦截', en: 'No module name for this process; intercept it by ID' },
  'ps.mustTcp': { zh: '强制转代理', en: 'Force proxy' },
  'ps.mustTcpHint': { zh: '把目标进程的 TCP 连接强制转到下面的 SOCKS 代理', en: 'Redirect the target processes\' TCP connections to the SOCKS proxy below' },
  'ps.proxyAddr': { zh: '代理地址', en: 'Proxy' },
  'ps.appointPort': { zh: '指定端口', en: 'Only ports' },
  'ps.auth': { zh: '需要认证', en: 'Auth' },
  'ps.userPh': { zh: '请输入账号', en: 'User name' },
  'ps.passPh': { zh: '请输入密码', en: 'Password' },
  'ps.detect': { zh: '检测代理', en: 'Test' },
  'ps.connected': { zh: '代理服务器连接成功', en: 'Proxy server reachable' },
  'ps.saveReminder': { zh: '需要启用 HTTP 代理后才可以拦截进程的数据；需要启用强制转代理后才可以使用滤镜功能。保存时会断开目标进程已建立的 TCP 连接。', en: 'HTTP proxy must be on to intercept process traffic; force proxy must be on for filters to apply. Saving drops the target processes\' existing TCP connections.' },

  // ── 映射设置 ──
  'map.local': { zh: '本地映射', en: 'Local mapping' },
  'map.remote': { zh: '远程映射', en: 'Remote mapping' },
  'map.enableLocal': { zh: '启用本地映射', en: 'Enable local mapping' },
  'map.enableRemote': { zh: '启用远程映射', en: 'Enable remote mapping' },
  'map.localHint': { zh: '把某个远端地址的响应换成本地文件', en: 'Answer a remote address with a local file' },
  'map.remoteHint': { zh: '把某个请求地址改写到另一个地址', en: 'Rewrite one request address to another' },
  'map.import': { zh: '导入', en: 'Import' },
  'map.export': { zh: '导出', en: 'Export' },
  'map.remoteAddr': { zh: '远端地址', en: 'Remote address' },
  'map.localFile': { zh: '本地文件', en: 'Local file' },
  'map.localFilePh': { zh: '请选择本地文件', en: 'Choose a local file' },
  'map.reqAddr': { zh: '请求地址', en: 'Request' },
  'map.mapAddr': { zh: '映射地址', en: 'Mapped to' },
  'map.host': { zh: '主机', en: 'Host' },
  'map.path': { zh: '路径', en: 'Path' },
  'map.emptyLocal': { zh: '还没有本地映射。', en: 'No local mappings yet.' },
  'map.emptyRemote': { zh: '还没有远程映射。', en: 'No remote mappings yet.' },
  'map.localEditHint': { zh: '请上传远端映射的本地文件，切勿使用不支持的文件类型。', en: 'Point it at the local file that should answer the remote address.' },
  'map.remoteEditHint': { zh: '命中请求地址的 HTTP 请求会被改写成映射地址再发出。', en: 'HTTP requests matching the request address are rewritten to the mapped address.' },
  'map.saveHint': { zh: '「保存」只管两个总开关；表里的增删改已经各自落库。', en: 'Save only covers the two switches; table edits are stored as you make them.' },

  // ── 外部代理 ──
  'xp.enable': { zh: '外部代理', en: 'External proxy' },
  'xp.hint': { zh: '启用后，WPE 的 SOCKS5 出口再经这个外部 SOCKS 代理出去。', en: 'When on, WPE\'s SOCKS5 egress goes through this external SOCKS proxy.' },
  'xp.grp': { zh: '外部 SOCKS 代理', en: 'External SOCKS proxy' },
  'xp.addr': { zh: '代理地址', en: 'Proxy' },
  'xp.addrPh': { zh: 'IP 或域名', en: 'IP or domain' },
  'xp.portHint': { zh: '只对这些目标端口走外部代理，逗号分隔；不勾就全部走。', en: 'Only these destination ports use the external proxy (comma separated); unchecked = all.' },

  // ── 快捷键 ──
  'hk.applyTo': { zh: '作用于', en: 'Applies to' },
  'hk.hint': { zh: '快捷键 1–10 触发列表里对应序号的那一条，「执行」「停止」管整份列表。', en: 'Keys 1–10 trigger the matching row of the list; Execute / Stop act on the whole list.' },
  'hk.custom': { zh: '自定义快捷键', en: 'Custom hotkeys' },
  'hk.key': { zh: '快捷键', en: 'Key' },
  'hk.execute': { zh: '执行', en: 'Execute' },
  'hk.stop': { zh: '停止', en: 'Stop' },
  'hk.ph': { zh: '请按组合键', en: 'Press a combination' },
  'hk.register': { zh: '注册', en: 'Register' },
  'hk.registered': { zh: '快捷键设置成功', en: 'Hotkey registered' },
  'hk.registerFail': { zh: '快捷键设置失败，可能已被别的程序占用', en: 'Hotkey registration failed; it may be taken by another program' },
  'hk.stOk': { zh: '已注册', en: 'OK' },
  'hk.stBad': { zh: '失败', en: 'Failed' },
  'hk.stChanged': { zh: '未注册', en: 'Pending' },
  'hk.stSaved': { zh: '生效中', en: 'Active' },
  'hk.registerHint': { zh: '按下「注册」那一刻就向系统登记，不等保存；「保存」只管「作用于」。', en: 'Register applies immediately; Save only stores the "applies to" choice.' },

  // ── 备份 ──
  'bk.hint': { zh: '勾选要带进备份的内容，导出成一个 .sb 文件（可加密）。', en: 'Tick what to include and export it as one .sb file (optionally encrypted).' },
  'bk.grp.system': { zh: '系统运行', en: 'System' },
  'bk.grp.proxy': { zh: '代理模式', en: 'Proxy mode' },
  'bk.grp.inject': { zh: '注入模式', en: 'Inject mode' },
  'bk.grp.lists': { zh: '列表数据', en: 'Lists' },
  'bk.systemConfig': { zh: '系统运行配置', en: 'System configuration' },
  'bk.proxySet': { zh: '代理模式配置', en: 'Proxy mode configuration' },
  'bk.proxyAccount': { zh: '代理账号', en: 'Proxy accounts' },
  'bk.whiteList': { zh: '白名单', en: 'White list' },
  'bk.blackList': { zh: '黑名单', en: 'Black list' },
  'bk.proxyMapping': { zh: '代理映射', en: 'Proxy mappings' },
  'bk.injectSet': { zh: '注入模式配置', en: 'Inject mode configuration' },
  'bk.filterList': { zh: '滤镜列表', en: 'Filter list' },
  'bk.sendList': { zh: '发送列表', en: 'Send list' },
  'bk.robotList': { zh: '机器人列表', en: 'Robot list' },
  'bk.import': { zh: '导入备份', en: 'Import backup' },
  'bk.export': { zh: '导出备份', en: 'Export backup' },
  'bk.importHint': { zh: '导入会整份替换当前配置与各份列表，并立即落库。', en: 'Importing replaces the current configuration and lists in full and stores them at once.' },

  // ── 远程管理 ──
  'rm.enable': { zh: '远程管理', en: 'Remote management' },
  'rm.running': { zh: '运行中', en: 'Running' },
  'rm.stopped': { zh: '未运行', en: 'Stopped' },
  'rm.hint': { zh: '内嵌的 Web 管理台：账号、日志与连接状态都能在浏览器里看。保存即启动或停止。', en: 'The built-in web console: accounts, logs and connections in a browser. Save starts or stops it.' },
  'rm.grp': { zh: '监听与账号', en: 'Listen & credentials' },
  'rm.listen': { zh: '监听地址', en: 'Listen on' },
  'rm.admin': { zh: '管理员', en: 'Admin' },
  'rm.userPh': { zh: '请输入管理员账号', en: 'Admin user name' },
  'rm.url': { zh: '访问地址', en: 'URL' },
  'rm.saveHint': { zh: '管理台用 HTTP 基本认证；/ProxyCap/* 对客户端免认证开放。', en: 'The console uses HTTP basic auth; /ProxyCap/* is open to clients without auth.' },

  // ── 代理模式：机器人列表 ────────────────────────────
  'rb.add': { zh: '新增机器人', en: 'Add Robot' },
  'rb.import': { zh: '导入', en: 'Import' },
  'rb.export': { zh: '导出', en: 'Export' },
  'rb.clearAll': { zh: '清空', en: 'Clear' },
  'rb.resetCount': { zh: '重置计数', en: 'Reset counts' },
  'rb.enableAll': { zh: '全部启用', en: 'Enable all' },
  'rb.disableAll': { zh: '全部禁用', en: 'Disable all' },
  'rb.start': { zh: '开始执行', en: 'Start' },
  'rb.stop': { zh: '停止', en: 'Stop' },
  'rb.runHint': {
    zh: '按列表顺序执行已启用的机器人；执行方式（依次 / 同时）在系统设置里。',
    en: 'Enabled robots run in list order; the mode (in sequence / together) is in System Settings.',
  },
  'rb.empty': {
    zh: '还没有机器人。新增一个，在编辑器里编排指令集（发送 / 延迟 / 循环 / 键盘 / 鼠标 / 开关），它就能替你按顺序执行。',
    en: 'No robots yet. Add one and arrange its instructions (send / delay / loop / keyboard / mouse / switch) in the editor.',
  },
  'rb.noInst': { zh: '指令集是空的，执行时什么都不会做', en: 'Empty instruction set — this robot will do nothing' },
  'col.robotName': { zh: '机器人名称', en: 'Robot Name' },

  // ── 代理模式：机器人编辑（术语照 Localizer.cs 的 RobotEditForm.*）──
  'rb.e.title': { zh: '机器人编辑', en: 'Robot Edit' },
  'rb.e.namePh': { zh: '请输入字符', en: 'Enter a name' },
  'rb.e.gone': { zh: '这条机器人已经不在列表里了', en: 'This robot is no longer in the list' },
  'rb.e.empty': { zh: '还没有指令。从左边挑一条填好参数，按「插入」放进来。', en: 'No instructions yet. Pick one on the left, fill in its parameters and press Insert.' },
  'rb.e.execute': { zh: '执行', en: 'Execute' },
  'rb.e.stop': { zh: '停止', en: 'Stop' },
  'rb.e.done': { zh: '机器人执行完毕', en: 'Robot finished' },
  'rb.e.stopped': { zh: '机器人已停止', en: 'Robot stopped' },
  'rb.e.failed': { zh: '发生错误:', en: 'Error:' },
  'rb.e.trail': { zh: '记录', en: 'Record' },
  'rb.e.clearAll': { zh: '清空所有指令', en: 'Clear all instructions' },
  'rb.e.colInst': { zh: '指令', en: 'Inst' },
  'rb.e.colType': { zh: '类型', en: 'Type' },
  'rb.e.colContent': { zh: '内容', en: 'Content' },
  'rb.e.inst': { zh: '指令', en: 'Inst' },
  'rb.e.insert': { zh: '插入', en: 'Insert' },
  'rb.e.insertHint': { zh: '有选中行时插在它前面，否则追加到末尾。', en: 'Inserts before the selected row, or appends at the end.' },
  'rb.e.pick': { zh: '请选择', en: 'Select' },
  'rb.e.gPacket': { zh: '封包指令', en: 'Packet Instruction' },
  'rb.e.gControl': { zh: '控制指令', en: 'Control Instruction' },
  'rb.e.gKey': { zh: '键盘指令', en: 'KeyBoard Instruction' },
  'rb.e.gMouse': { zh: '鼠标指令', en: 'Mouse Instruction' },
  'rb.e.sendList': { zh: '发送 - 发送列表', en: 'Send - Send List' },
  'rb.e.packetList': { zh: '发送 - 封包列表', en: 'Send - Packet List' },
  'rb.e.packetListHint': { zh: '封包列表中选中的封包', en: 'Selected packet in the Packet List' },
  'rb.e.packetListNote': { zh: '这一条读的是注入模式封包列表里的选中行，代理模式下不会发任何东西。', en: 'Reads the selected row of the Inject-mode packet list; it sends nothing in Proxy mode.' },
  'rb.e.sysSocket': { zh: '设置 - 系统套接字', en: 'Set - System Socket' },
  'rb.e.sockPacket': { zh: '选中封包的套接字', en: 'Socket of the selected packet' },
  'rb.e.sockFilter': { zh: '调用滤镜的套接字', en: 'Socket for calling filter' },
  'rb.e.sockCustom': { zh: '自定义', en: 'Custom' },
  'rb.e.delay': { zh: '延迟', en: 'Delay' },
  'rb.e.fixed': { zh: '定时', en: 'Fixed' },
  'rb.e.random': { zh: '随机', en: 'Random' },
  'rb.e.ms': { zh: '毫秒', en: 'ms' },
  'rb.e.loop': { zh: '循环', en: 'Loop' },
  'rb.e.loopTimes': { zh: '次', en: 'times' },
  'rb.e.begin': { zh: '开始', en: 'Begin' },
  'rb.e.end': { zh: '结束', en: 'End' },
  'rb.e.sw': { zh: '开关', en: 'Switch' },
  'rb.e.on': { zh: '启用', en: 'Enable' },
  'rb.e.off': { zh: '禁用', en: 'Disable' },
  'rb.e.swSend': { zh: '发送列表', en: 'Send List' },
  'rb.e.swRobot': { zh: '机器人列表', en: 'Robot List' },
  'rb.e.swFilter': { zh: '滤镜列表', en: 'Filter List' },
  'rb.e.key': { zh: '按键', en: 'Key' },
  'rb.e.keyType': { zh: '类型', en: 'Type' },
  'rb.e.keyPress': { zh: '按键', en: 'Key Press' },
  'rb.e.keyDown': { zh: '按下', en: 'Key Down' },
  'rb.e.keyUp': { zh: '弹起', en: 'Key Up' },
  'rb.e.keyPh': { zh: '请按键', en: 'Press a key' },
  'rb.e.combo': { zh: '组合按键', en: 'Combination Key' },
  'rb.e.comboPh': { zh: '请组合按键', en: 'Press a key combination' },
  'rb.e.text': { zh: '文本', en: 'Text' },
  'rb.e.textPh': { zh: '请输入文本', en: 'Enter text' },
  'rb.e.mouseKey': { zh: '按键', en: 'Key' },
  'rb.e.m0': { zh: '左键单击', en: 'Left Click' },
  'rb.e.m1': { zh: '右键单击', en: 'Right Click' },
  'rb.e.m2': { zh: '左键双击', en: 'Left Double Click' },
  'rb.e.m3': { zh: '右键双击', en: 'Right Double Click' },
  'rb.e.m4': { zh: '左键按下', en: 'Left Down' },
  'rb.e.m5': { zh: '左键弹起', en: 'Left Up' },
  'rb.e.m6': { zh: '右键按下', en: 'Right Down' },
  'rb.e.m7': { zh: '右键弹起', en: 'Right Up' },
  'rb.e.wheel': { zh: '滚轮', en: 'Wheel' },
  'rb.e.scroll': { zh: '滚动', en: 'Scroll' },
  'rb.e.distance': { zh: '距离', en: 'Distance' },
  'rb.e.move': { zh: '移动', en: 'Move' },
  'rb.e.moveTo': { zh: '移动到', en: 'Move To' },
  'rb.e.moveBy': { zh: '相对移动', en: 'Move By' },
  'col.instructions': { zh: '指令条数', en: 'Instructions' },

  // ── 代理模式：仓库列表 ──────────────────────────────
  'wh.add': { zh: '新增仓库', en: 'Add WareHouse' },
  'wh.import': { zh: '导入', en: 'Import' },
  'wh.export': { zh: '导出', en: 'Export' },
  'wh.clearAll': { zh: '清空', en: 'Clear' },
  'wh.autoStores': { zh: '自动入库', en: 'Auto Stores' },
  'wh.empty': {
    zh: '还没有仓库。新增一个，然后在封包列表里右键「添加到仓库」，或用「自动入库」按包头自动收集。',
    en: 'No warehouses yet. Add one, then use "Add to warehouse" on the packet list, or let Auto Stores collect by header.',
  },
  'wh.noStores': { zh: '这个仓库是空的', en: 'This warehouse is empty' },
  'col.wareHouseName': { zh: '仓库名称', en: 'WareHouse Name' },
  'col.stores': { zh: '仓储数量', en: 'Stores' },

  // 仓库编辑（WareHouseEdit）
  'wh.e.title': { zh: '仓库编辑', en: 'WareHouse Editor' },
  'wh.e.gone': { zh: '这个仓库已经不在列表里了', en: 'This warehouse is no longer in the list' },
  'wh.e.namePh': { zh: '给这个仓库起个名字', en: 'Name this warehouse' },
  'wh.e.stores': { zh: '仓储数据', en: 'Stores' },
  'wh.e.import': { zh: '导入数据', en: 'Import data' },
  'wh.e.export': { zh: '导出所有数据', en: 'Export all' },
  'wh.e.clear': { zh: '清空所有数据', en: 'Clear all' },
  'wh.e.copyHex': { zh: '复制十六进制', en: 'Copy hex' },
  'wh.e.exportPicked': { zh: '导出选中', en: 'Export selected' },
  'wh.e.empty': {
    zh: '这个仓库还是空的。封包列表右键「添加到仓库」、自动入库、或上面的「导入数据」都能往里放。',
    en: 'This warehouse is empty. Use "Add to warehouse" on the packet list, Auto Stores, or Import data above.',
  },
  'wh.e.liveHint': {
    zh: '对仓储数据的改动立即生效，取消不会撤销；只有名称按保存才写回。',
    en: 'Changes to the stored data take effect at once and are not undone by Cancel; only the name waits for Save.',
  },

  // 自动入库（AutoStoresList + AutoStoresEdit）
  'as.title': { zh: '自动入库', en: 'Auto Stores' },
  'as.enable': { zh: '启用自动入库', en: 'Enable auto stores' },
  'as.enableNotice': { zh: '每次重启软件后需手动开启', en: 'Must be switched on again after each restart' },
  'as.lead': {
    zh: '开启后，经过代理的每个封包都会与下面的规则逐条比对：包头相同就存一份进对应的仓库。',
    en: 'When on, every packet through the proxy is checked against the rules below; a matching header stores a copy into that warehouse.',
  },
  'as.rules': { zh: '规则', en: 'Rules' },
  'as.add': { zh: '新增规则', en: 'Add rule' },
  'as.edit': { zh: '编辑规则', en: 'Edit rule' },
  'as.empty': { zh: '还没有规则', en: 'No rules yet' },
  'as.head': { zh: '指定包头', en: 'Packet header' },
  'as.headPh': { zh: '十六进制，如 16 03 01', en: 'Hex, e.g. 16 03 01' },
  'as.headHint': { zh: '封包开头的字节与这里逐字节相同才算命中', en: 'Matches when the packet starts with exactly these bytes' },
  'as.wareHouse': { zh: '入库名称', en: 'WareHouse' },
  'as.pickWareHouse': { zh: '选择仓库…', en: 'Pick a warehouse…' },
  'as.noWareHouse': { zh: '还没有仓库，先在仓库列表里新增一个', en: 'No warehouses yet — add one in the WareHouse List first' },
  'as.gone': { zh: '仓库已不存在', en: 'warehouse no longer exists' },
  'as.saved': { zh: '自动入库保存成功', en: 'Auto stores saved' },

  'list.paused': { zh: '已暂停跟随 · 点此回到底部', en: 'Auto-scroll paused · click to jump to the end' },

  // ── 代理模式：十六进制面板 ──────────────────────────
  'hex.title': { zh: '十六进制', en: 'Hex' },
  'hex.bytes': { zh: '字节', en: 'bytes' },
  'hex.rtt': { zh: '往返', en: 'RTT' },
  'hex.rttHint': {
    zh: '从请求字节到拿到结果的耗时（含桥往返与按 Id 取字节），不含本地渲染',
    en: 'Time from requesting the bytes to receiving them (bridge round-trip + lookup); excludes local rendering',
  },
  'hex.unmodifiedHint': {
    zh: '改写前后两份缓冲逐字节相同 —— 没有滤镜动过这个封包',
    en: 'The before/after buffers are byte-identical — no filter touched this packet',
  },
  'hex.after': { zh: '改写后', en: 'Modified' },
  'hex.before': { zh: '改写前', en: 'Original' },
  'hex.unmodified': { zh: '未被滤镜改写', en: 'Not changed by filters' },
  'hex.loading': { zh: '取字节中…', en: 'Loading bytes…' },
  'hex.pick': { zh: '点一行查看完整字节', en: 'Click a row to view its bytes' },
  'hex.gone': { zh: '已不在列表中（被自动清理了）', en: 'no longer in the list (auto cleared)' },
  //文本 / 十六进制两种看法。HTTP 那几类默认落在文本，照 WinForms 的 PacketData 控件
  'hex.asText': { zh: '文本', en: 'Text' },
  'hex.asHex': { zh: '十六进制', en: 'Hex' },
  'hex.textHint': {
    zh: 'HTTP / HTTPS 的封包默认按文本看（UTF8 解码，不可读的字节显示为 ·）',
    en: 'HTTP/HTTPS packets default to the text view (UTF-8; unprintable bytes show as ·)',
  },

  'toast.dismiss': { zh: '点击关闭', en: 'Click to dismiss' },

  // ── 弹窗（前端渲染那部分的按钮）──────────────────────
  // ── 加密导入 / 导出的密码框 ─────────────────────────
  'pw.exportTitle': { zh: '为导出的文件设置密码', en: 'Set a password for the export' },
  'pw.importTitle': { zh: '这个文件已加密', en: 'This file is encrypted' },
  'pw.exportLead': {
    zh: '设了密码，别人拿到这个文件也打不开；导入时要输入同一个密码。',
    en: 'With a password, the file is useless to anyone else — the same password is needed to import it.',
  },
  'pw.importLead': { zh: '输入导出时设置的密码。', en: 'Enter the password that was set when it was exported.' },
  'pw.password': { zh: '密码', en: 'Password' },
  'pw.confirm': { zh: '再输一次', en: 'Repeat' },
  'pw.show': { zh: '显示密码', en: 'Show password' },
  'pw.hide': { zh: '隐藏密码', en: 'Hide password' },
  'pw.mismatch': { zh: '两次输入不一致', en: 'The two entries do not match' },
  'pw.empty': { zh: '请输入密码', en: 'Enter a password' },
  'pw.wrong': { zh: '密码不对，再试一次', en: 'Wrong password — try again' },
  'pw.checking': { zh: '校验中…', en: 'Checking…' },
  'pw.skip': { zh: '不加密', en: 'Skip encryption' },
  'pw.skipHint': { zh: '直接导出，任何人都能打开', en: 'Export as plain text — anyone can open it' },
  'pw.encrypt': { zh: '加密导出', en: 'Encrypt' },
  'pw.unlock': { zh: '解锁导入', en: 'Unlock' },
  'pw.caps': { zh: '大写锁定已打开', en: 'Caps Lock is on' },

  'dlg.ok': { zh: '确定', en: 'OK' },
  'dlg.cancel': { zh: '取消', en: 'Cancel' },
  'dlg.close': { zh: '关闭', en: 'Close' },
  'beta.warn': { zh: '未经完整回归验证的构建', en: 'build not fully regression-tested' },
  'beta.ok': { zh: '知道了', en: 'Got it' },
} satisfies Record<string, Entry>

export type Key = keyof typeof DICT
