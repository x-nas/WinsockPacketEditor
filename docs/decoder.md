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

解码器页的测试台**没有封包上下文**，走 `testDecoder` → `CodecEngine.Process`，**不校验范围**（`dec.scopeHint` 已写明）。

## 存储与备份

`Decoder` 表：`GUID` 主键 + `IsEnable` + `Name` + `Description` + 算法 / 帧 / 范围列（**没有 Author 列**）。整表保存走 `DataBase.SaveTable_Decoder` 单事务（`InsertTable_Decoder(Conn, Tx)` 重载）。老库的列增删分别走 `EnsureColumn` / `DropColumnIfExists`（在 `CreateTable_Decoder` 末尾调用，幂等、失败只记日志）。备份 XML 的 `<Decoders>` 节由 `GetDecoderList_XML` / `LoadDecoderList_XML` 读写。

## 桥方法

`getDecoders` · `addDecoder` · `saveDecoder` · `deleteDecoders` · `setDecoderEnable` · `testDecoder`（测试台，不校验范围）· `decodeWith` · `smartDecode` · `batchDecode`。

## 前端

- `components/decoder/`：`Decoder.vue`（左列表 + 三区工作台：原文 / 参数与按钮 / 结果；顶栏「清空」只在快速编解码显示）、`DecoderEdit.vue`（编辑弹窗）、`SmartResult.vue` / `BatchResult.vue` / `DecodeResult.vue`（结果弹窗）、`actions.ts`（右键菜单项 + DTO）、`enums.ts`。
- 快速编解码复用 `components/proxy/Transcode.vue`（嵌入态），与已保存解码器的测试台同一种「左原文 / 中参数 / 右结果」布局；编码 `原文 → 结果`、解码 `结果 → 原文`（右栏可编辑，解码读右栏、写回左栏并保留右栏）。
- 状态在 `stores/decoder.ts`（跨页面切换保留；`decMode` 默认 `'quick'`）。
- 右键菜单集成在 `ProxyData.vue` / `InjectData.vue` / `HexPanel.vue`。

## 内置预置

`InstallBuiltinPresets` 装入 10 个可编辑样板（AES / DES / RC4 / XOR / XXTEA / Protobuf / MessagePack / BSON / AMF / FlatBuffers），**适用范围统一 `Any / Any`**，帧配置为空。它们是**样板 / 截图数据**：密钥全是演示值（`aeskey1234567890` / `deskey12` / `0x64` / `0102030405060708` / 固定 XXTEA 密钥），对真实流量要按目标协议改密钥、IV、模式与帧配置后才能用。

## 回归测试

`tools/tests/` 下四套，都在 `bin` 目录用反射跑、不需要 SQLite：

- `Decoder.ps1`：引擎（XOR/AES/DES/RC4/XXTEA/BSON/AMF/FlatBuffers、帧、Protobuf/MessagePack、文本编码、4 MB 上限），39 项。
- `DecoderConfig.ps1`：`ToRow` / `FromRow` / `Normalize` / 列表增删，18 项。
- `DecoderIntegration.ps1`：从列表取字节解码，5 项。
- `DecoderSmart.ps1`：智能解码 + 范围校验，9 项。
