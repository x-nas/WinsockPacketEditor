# TCP 连接级远程映射

WPE 代理模式的「远程映射」支持 `TCP` 协议。它在建立连接时改写**目标地址**，不是改写应用层报文；连接建立后的数据仍走既有转发、抓包、日志和 TCP 滤镜链路。

## 配置

在「远程映射」编辑框中选择 `TCP`：

- 来源填写主机名或 IP 与端口；目标填写新的主机名或 IP 与端口。
- TCP 不使用 URL 路径，界面会隐藏路径输入框，并把目标协议固定为 `TCP`。
- 主机匹配支持精确值、`*`、`prefix*` 和 `*suffix`；WPE 会依次尝试连接请求中的主机名和已解析 IP。多个已启用规则同时匹配时按列表顺序取第一条。

例如，将 `127.0.0.1:39101` 映射到 `127.0.0.1:39102`，所有原本连接 39101 的 TCP 会话都会实际连向 39102。

## 与其他映射、外部代理和滤镜的关系

- 裸 TCP / Socket 以及 HTTPS CONNECT 会在连接建立时直接应用 TCP 映射。
- HTTP 会先等到完整的首个请求头，以保持已有 HTTP 本地文件和按路径远程映射的优先级；这些规则都未命中时，才以 TCP 映射作为连接级回退。
- 若会话使用外部 SOCKS5 代理，WPE 会重新生成并发送给上游的 SOCKS5 CONNECT 请求，因此上游看到的也是映射后的目标。
- TCP 映射本身不修改字节。连接成功后的上下行数据仍经过 `ForwardData` 和 TCP 滤镜，因而仍可抓包和改包。对 TLS 等加密协议，滤镜能看到的是密文；HTTPS 由 TCP 规则命中时按普通 TCP 转发，不进入 HTTPS MITM 请求级映射。

## 回归测试

`tests/TcpMapRemoteTest` 是一个真实 SOCKS5 联调测试：它在本地启动 `39102` 回显服务，经 WPE SOCKS5 请求 `39101`，并验证随机负载是否完整回显。

先在运行中的 WPE 中启用 TCP 规则 `127.0.0.1:39101 -> 127.0.0.1:39102`，再执行：

```powershell
MSBuild tests\TcpMapRemoteTest\TcpMapRemoteTest.csproj -restore -p:Configuration=Release
tests\TcpMapRemoteTest\bin\Release\net48\TcpMapRemoteTest.exe --user <SOCKS5账号> --password <SOCKS5密码>
```

可用 `--socks-host`、`--socks-port`、`--source-port` 和 `--target-port` 覆盖默认的本机地址和端口。
