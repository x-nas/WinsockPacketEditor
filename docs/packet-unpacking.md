# 代理 TCP 拆包

代理模式的「拦截设置 → 拆包」把 SOCKS5 TCP 字节流还原为应用层帧，使每个完整帧独立经过滤镜、抓包列表和转发。它不用于 UDP，也不用于注入模式的 WinSock 钩子。

## 当前规则模型

`Enable_UnPack` 开启后可维护有序的拆包规则列表；每条规则有名称、启用状态、方向（双向 / 请求 / 响应）、包头与长度字段。规则存入 `ProxyMode.UnpackRules` JSON，并随设置备份 XML 导入导出；旧库的单条 `UnPack_Head` / `UnPack_Length` 会自动迁成「默认规则」。

- 包头由空格、逗号或分号分隔的十六进制字节组成，长度为 1–64 字节。
- 长度位置是包内从 1 开始的闭区间，如 `4-5`。
- 长度字段为 1–4 字节、大端，值表示整个包的字节数。
- 最大完整帧和残片为 4 MB；零长度、小于必要头部的长度或超限长度均视为无效，不能阻塞流量。

设置保存、MCP 设置和运行时使用同一套 `ValidateUnpackSettings` 校验。运行时将文本预编译、缓存为不可变配置；配置未变时热路径不会再次拆字符串或解析十六进制。

## 数据面语义

`ProxySession.ForwardUnpack`（客户端到目标）和 `ResponseUnpack`（目标到客户端）彼此独立。增量拆帧器在每个方向执行以下规则：

1. 完整帧立刻交给过滤器和正常转发。
2. 还不完整的首帧或尾帧保留到下一次 TCP 接收，绝不提前丢弃。
3. 找不到包头时，非帧前缀保持原字节输出；可能构成跨接收包头的末尾前缀才暂存。
4. 遇到坏长度时输出一个字节并重新同步，防止零长度死循环或恶意大长度无限占用内存。

首次成功拆帧后，该 TCP 会话方向固定命中的规则直至会话关闭；后续设置变更只影响新会话。这避免同一流因 payload 恰好出现另一规则包头而错误切换。快照按包头首字节索引候选，已固定规则时只进行单规则匹配，因此规则列表不会给常规热路径引入线性规则扫描。

## 回归测试

`tests/PacketUnpackTest` 是独立的 net48 控制台程序，已加入 `WinSockPacketEditor.sln`。它覆盖首帧/包头分片、粘包、完整帧加半帧、重同步、零长度、超大长度以及配置边界。

```powershell
& 'C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe' WinSockPacketEditor.sln -restore -p:Configuration=Release
& .\tests\PacketUnpackTest\bin\Release\net48\PacketUnpackTest.exe
```

## 多规则扩展约束

多规则可在现有增量拆帧器上实现，但不能把规则列表在每一帧中任意切换。正确模型是：按 TCP 会话、按方向维护残片与已选规则；首次成功拆帧后固定该规则，直至会话结束或明确重同步。规则应有稳定 ID、名称、启用状态、优先级、包头和长度字段，并可选限制目标地址/端口与方向。这样可避免不同协议包头重叠，或 payload 恰好包含另一规则包头时错误切换。
