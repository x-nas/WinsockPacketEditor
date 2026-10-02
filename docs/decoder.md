# 解码器（CodecEngine / DecoderConfig）

「解码器」是**跨注入 / 代理两种模式共用**的一套按需解码配置：把算法、密钥、帧格式与适用范围存下来，在封包列表 / 详情右键按需解码，也可以在解码器页里即时测试。它与 `ClassObject/ProtocolDecoder.cs`（封包详情里那个「按需解码」工具）是两回事：后者是固定的几种通用算法、不落库；前者可增删改、落库、带范围校验。

## 数据模型

`DecoderInfo`（`ClassObject/Decoder/DecoderInfo.cs`）是纯数据类，字段与 `Decoder` 表的列一一对应；过桥时另拼 `DecoderRow`（`ClassObject/Decoder/DecoderRow.cs`），**不直接把 `DecoderInfo` 序列化出去**。

字段分四组：

- **算法**：`Kind`、`Charset`（文本编码器用）、`KeyFormat` / `Key`、`IvFormat` / `Iv`、`CipherMode`、`Padding`。
- **帧**：`LengthBytes`、`BigEndian`、`LengthIncludesSelf`、`HasFixedHeader` / `FixedHeader`、`LengthIncludesFixedHeader`、`DataOffset`。
- **范围**：`ProtocolType`、`Direction`。
- **扩展位**：`ParamsJson`（保留，当前没有算法读它）。

`BlockSize` 目前是**死字段**：引擎按 AES=16 / DES=8 自己定块长，界面也没有这一项。`ParamsJson` 同理是保留位。两者都不要当成可用的调参。

枚举（存 int，新增往后加、不改已有值）：`DecoderKind`（Xor=1 … FlatBuffers=11）、`DecoderProtocol`（Any/Tcp/Udp/Http/WebSocket）、`DecoderDirection`（Any/Request/Response）、`DecoderKeyFormat`、`DecoderCipherMode`（CBC/ECB/OFB/CFB，CTS 只读旧值、保存会拒）、`DecoderPadding`、`DecoderCharset`。

## 算法（`CodecEngine`）

`CodecEngine.Process(input, cfg, encode, applyFrame)` 是统一入口：

- `applyFrame = true` 时先 `FrameExtractor.Extract`（编码方向先算算法再 `FrameExtractor.Build` 装回帧）。
- 按 `Kind` 分派：XOR / AES / DES 走内置 `Core`；RC4 / XXTEA / BSON / AMF / FlatBuffers 走 `AdditionalDecoders`；TextCharset 走文本编码；Protobuf / MessagePack 用 `ProtocolDecoder.ProtobufGuess` 与 `MsgpackGuess` 推测结构（结构类**只解码、不编码**，编码方向原样回送字节）。
- 输入上限 4 MB；AES/DES 的 `CryptographicException` 统一报「加解密失败」（`Proto.CipherFail`，不再区分 AES/DES）。

## 帧解析（`FrameExtractor`）

`LengthBytes > 0` 时按长度字段切一段；`LengthIncludesSelf` / `LengthIncludesFixedHeader` 决定长度值是否包含自身 / 固定头；`HasFixedHeader` + `FixedHeader` 校验并跳过固定头；`DataOffset` 再跳过起始偏移。`LengthBytes` 只允许 1 / 2 / 4。

## 适用范围（`CodecEngine.AppliesTo`）

配置里的协议 / 方向**只对来自列表 / 详情的真实封包校验**：`AppliesTo` 把封包的 `PacketType`（0–22）归成 TCP / UDP / HTTP / WebSocket × 请求 / 响应，与 `cfg.ProtocolType` / `Direction` 比对，不匹配返回「当前封包不在此解码器的协议或方向适用范围内」。三条真实路径共用同一口径：

- 右键「解码器 ▸」→ 桥 `decodeWith`（带 `packetType`）→ `CodecEngine.Run(input, cfg, packetType, applyFrame)`；
- 右键「智能解码」→ 桥 `smartDecode`（带 `packetType`）→ `SmartDecode` 跳过不匹配的解码器；
- 批量解码 → 后端按每条封包自己的 `PacketType` 调 `DecoderResultRow`。

解码器工作台的测试台**没有封包上下文**，走 `testDecoder` → `CodecEngine.Process`，**不校验范围**（`dec.scopeHint` 已写明）。

## 存储与备份

`Decoder` 表：`GUID` 主键 + `IsEnable` + `Name` + `Description` + 算法 / 帧 / 范围列（**没有 Author 列**）。整表保存走 `DataBase.SaveTable_Decoder` 单事务（`InsertTable_Decoder(Conn, Tx)` 重载）。老库的列增删分别走 `EnsureColumn` / `DropColumnIfExists`（在 `CreateTable_Decoder` 末尾调用，幂等、失败只记日志）。备份 XML 的 `<Decoders>` 节由 `GetDecoderList_XML` / `LoadDecoderList_XML` 读写。

## 桥方法

`getDecoders` · `addDecoder` · `saveDecoder` · `deleteDecoders` · `setDecoderEnable` · `testDecoder`（测试台，不校验范围）· `decodeWith` · `smartDecode` · `batchDecode` · `smartDecodeBatch`（多选智能解码）· `cancelDecodeJob`。
解码器列表页另有一组：`setAllDecoderEnable` · `decoderListAction`（置顶 / 上移 / 下移 / 置底 / 复制 / 导出 / 删除，编号与其它列表一致）· `importDecoders` · `exportDecoders` · `clearDecoders`。

## 批量 / 多选解码的执行（2026-09-28）

- **可以并行**：`CodecEngine` 是无共享状态的纯函数（每次只用传入的 buffer 与一份配置；`SymmetricAlgorithm` 也是每次 new），`AdditionalDecoders` / `ProtocolDecoder` / `FrameExtractor` 同样只有静态方法、没有静态可变字段；`DecoderInfo.Clone()` 是 `MemberwiseClone`，字段全是值类型 / 字符串，所以克隆出一份就能安全地跨线程只读。每条「封包 × 解码器」相互独立，**并行结果与串行逐条完全一致**。
- **但共享列表不能拿到后台线程遍历**：`batchDecode` / `smartDecodeBatch` 都在 **UI 线程先快照**（封包的 `PacketBuffer` + `packetType`、启用的解码器 `Clone()`），再 `Task.Run` + `Parallel.For`（`MaxDegreeOfParallelism = Environment.ProcessorCount`）。抓包热路径上那张 `BindingList` 只被 UI 线程碰。
- **进度与取消**：进度经 `decode:progress` 事件回推，事件里带前端发起时给的 `job` 号（用来丢弃上一个任务迟到的进度）。遮罩是 `DecodeProgress.vue`（进度条 + 取消），`cancelDecodeJob` 取消当前任务 —— 任务是**单例**的，再发起一个会先把上一个取消。取消后不回半截结果（前端拿到 `cancelled` 就不弹窗）。
- UI 线程只在快照与收发消息时占用；解码本身在后台线程池，所以选中几十条也不会把界面卡住。

## 前端

解码与「智能解码」分成两屏（侧栏 Rules 组的「解码器列表」与 Tools 组的「编码解码」）：

- `components/decoder/DecoderList.vue`：解码器<b>列表页</b>，与滤镜 / 发送 / 机器人 / 仓库并列。表格 + 工具条（添加 / 全部启用 / 全部禁用 / 导入 / 导出 / 清空）+ 右键菜单七个动作（置顶 / 上移 / 下移 / 置底 / 复制 / 导出 / 删除 + 全选 / 取消选择）。解码器不在 FeedList 推送流里，每个改动动作后自己 `ensureDecoders(true)` 重拉。
- **快捷面板**（数据页下半屏左侧，`components/proxy/QuickPanel.vue`，注入 / 代理共用）：也有一栏「解码器」（与「取值器」并列）。列表走 `getDecoders` 拉取，勾选启停走 `setDecoderEnable`、双击开 `DecoderEdit`、右键走 `decoderListAction`。因为解码器不在 Feed 推送流里，面板里每个改动动作后都要 `ensureDecoders(true)` 重拉 —— 不重拉就会看着像「复制 / 移动没生效」；面板里**不提供导出**（导出在解码器列表页）。
- `components/decoder/Decoder.vue`：<b>智能解码</b>工作台。选择器（`快速编解码` 或某个已保存解码器）在<b>中间栏顶部</b>，右边是「智能解码 / 清空」。选到快速编解码时整块工作台换成 `Transcode.vue`，选择器经它的 `#extra` 插槽放进它的中间栏（否则选到快速编解码后就切不回来了）；选到解码器时显示测试台。中间栏宽度固定 **250px**（与 `Transcode.vue` 的 `.workbench` 一致），两种来源之间切换时不会一宽一窄。
- `DecoderEdit.vue`（编辑弹窗，列表页与「保存为解码器」共用）、`SmartResult.vue` / `BatchResult.vue` / `DecodeResult.vue`（结果弹窗）、`actions.ts`（右键菜单项 + DTO）、`enums.ts`（枚举与 `kindLabel`）。
- 快速编解码复用 `components/proxy/Transcode.vue`（嵌入态），与已保存解码器的测试台同一种「左原文 / 中参数 / 右结果」布局；编码 `原文 → 结果`、解码 `结果 → 原文`（右栏可编辑，解码读右栏、写回左栏并保留右栏）。
- 状态在 `stores/decoder.ts`（跨页面切换保留；`decMode` 默认 `'quick'`，`decRows` 由列表页 / 工作台 / 侧栏计数共用）。
- 右键菜单集成在 `ProxyData.vue` / `InjectData.vue` / `HexPanel.vue`。封包列表里的「解码器」菜单项<b>按选中条数自动分流</b>：选一条走单条 `decodeWith`（`DecodeResult`），选多条走 `batchDecode`（`BatchResult`）—— 不再有单独的「批量解码」菜单项；「解码器」与「智能解码」都照其它菜单项带选中条数。「智能解码」同样按条数走：单条解一条；多选逐条跑 `smartDecode`，结果按封包分组（`SmartPayload.items`，`SmartResult` 画封包头）。原「编码 / 解码」菜单项已删除。

## 内置预置

**不再自动创建任何预置解码器**（原先 `InstallBuiltinPresets` 会在空库时装 10 条样板，现已移除）。空库打开解码器列表是空的，点「添加」新建一条，或从 `.dec` 文件导入。

## 回归测试

`tools/tests/` 下四套，都在 `bin` 目录用反射跑、不需要 SQLite：

- `Decoder.ps1`：引擎（XOR/AES/DES/RC4/XXTEA/BSON/AMF/FlatBuffers、帧、Protobuf/MessagePack、文本编码、4 MB 上限），39 项。
- `DecoderConfig.ps1`：`ToRow` / `FromRow` / `Normalize` / 列表增删，18 项。
- `DecoderIntegration.ps1`：从列表取字节解码，5 项。
- `DecoderSmart.ps1`：智能解码 + 范围校验，9 项。
