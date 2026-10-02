# 代理 TCP 拆包

代理模式的「拦截设置 → 拆包」把 SOCKS5 TCP 字节流还原为应用层帧，使每个完整帧独立经过滤镜、抓包列表和转发。它不用于 UDP，也不用于注入模式的 WinSock 钩子。

## 当前规则模型

当前版本只有一条全局规则：`Enable_UnPack`、`UnPack_Head`、`UnPack_Length`。它应用到所有开启 TCP 拦截的代理会话的请求、响应两个方向。

- 包头由空格、逗号或分号分隔的十六进制字节组成，长度为 1–64 字节。
- 长度位置是包内从 1 开始的闭区间，如 `4-5`。
- 长度字段为 1–4 字节、大端，值表示整个包的字节数。
- 最大完整帧和残片为 4 MB；零长度、小于必要头部的长度或超限长度均视为无效，不能阻塞流量。

设置保存、MCP 设置和运行时使用同一套 `ValidateUnpackSettings` 校验。运行时将文本预编译、缓存为不可变配置；配置未变时热路径不会再次拆字符串或解析十六进制。

## 数据面语义

`ProxySession.ForwardBuffer`（客户端到目标）和 `ResponseBuffer`（目标到客户端）彼此独立。增量拆帧器在每个方向执行以下规则：

1. 完整帧立刻交给过滤器和正常转发。
2. 还不完整的首帧或尾帧保留到下一次 TCP 接收，绝不提前丢弃。
3. 找不到包头时，非帧前缀保持原字节输出；可能构成跨接收包头的末尾前缀才暂存。
4. 遇到坏长度时输出一个字节并重新同步，防止零长度死循环或恶意大长度无限占用内存。

因此，拆包只改变过滤与展示的帧粒度，不得改变 TCP 字节序列。

## 回归测试

`tests/PacketUnpackTest` 是独立的 net48 控制台程序，已加入 `WinSockPacketEditor.sln`。它覆盖首帧/包头分片、粘包、完整帧加半帧、重同步、零长度、超大长度以及配置边界。

```powershell
& 'C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe' WinSockPacketEditor.sln -restore -p:Configuration=Release
& .\tests\PacketUnpackTest\bin\Release\net48\PacketUnpackTest.exe
```

## 多规则扩展约束

多规则可在现有增量拆帧器上实现，但不能把规则列表在每一帧中任意切换。正确模型是：按 TCP 会话、按方向维护残片与已选规则；首次成功拆帧后固定该规则，直至会话结束或明确重同步。规则应有稳定 ID、名称、启用状态、优先级、包头和长度字段，并可选限制目标地址/端口与方向。这样可避免不同协议包头重叠，或 payload 恰好包含另一规则包头时错误切换。
