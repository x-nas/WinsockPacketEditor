# HTTPS 映射改造方案与当前实现

状态：首期实现已完成（2026-09-25）。HTTPS 本地映射复用既有规则与编辑器：本地映射可选 HTTP / HTTPS；远程映射保留协议下拉框，但当前只提供 HTTP。

## POC 结论（2026-09-25）

独立的 `tests/HttpsMitmPoc` 已在 Windows 上以 net48 Release 构建并运行通过：

- BCL `CertificateRequest` 创建根 CA，并动态签发带 DNS SAN 的叶证书；
- `SslStream` 成功承接客户端 TLS，按路径返回本地 HTTP/1.1 响应；
- 未命中请求经另一个 `SslStream` 建立上游 TLS 后原样转发，测试上游收到原始请求；
- 没有使用 Titanium.Web.Proxy、BouncyCastle 或 SunnyNet，也没有安装系统根证书。

net48 的 Schannel 不能直接使用 `CertificateRequest` 产生的临时私钥句柄；POC 需把内存 PFX
以 `UserKeySet` 重载为 Schannel 可用的证书。正式实现的 CA 私钥持久化在服务器级
`%PROGRAMDATA%\WPE64\https-mitm\`，并由机器 DPAPI 保护。
POC 只证明 BCL 路线可行，不是生产代理实现。

## 已完成的前置修复（2026-09-25）

- HTTP 列表嗅探改为先判定、后入队：不再把同一段数据先记成 TCP 再重复记成 HTTP；会话关闭时会刷出未完成残片。
- 嗅探缓存对所有路径（包括 `Content-Length`）执行 4 MB 上限，超限退回 TCP 展示，不影响转发。
- HTTP 条目现在保留过滤前与实际过滤后的字节及动作，展示不再假定它永远是未修改数据。
- HTTP 映射会等待完整请求头，支持 PATCH / DELETE / OPTIONS / TRACE，并正确解析 `Host` 的 IPv6 字面量和端口；映射改写只重写头部，二进制正文原样保留。

这些修复是 TLS 解密接入的基础。

## 目标与首期范围

为代理模式现有 SOCKS5 端口上的 HTTPS（TCP）流量增加可选本地文件映射。规则匹配沿用既有本地映射语义：主机和端口相等、路径以前缀匹配（查询串属于路径的一部分）。

首期仅支持 HTTP/1.1；不支持 HTTP/2、HTTP/3/QUIC、WebSocket、gRPC、转发到另一 HTTPS URL，亦不尝试绕过证书固定或安全软件校验。TLS 握手或上游校验失败时关闭该会话并写系统日志：握手字节已由 `SslStream` 消费，不能安全地把同一连接降级成明文直通。

## 不可破坏的现有语义

- 外网客户端继续连接现有 WPE SOCKS5 端口，不能要求其改用新端口。
- `ProxySession.ClientIP`、账号认证、设备/连接限制、客户端列表与代理日志必须继续保留 TCP 对端的真实外网地址。
- WPC 控制通道、普通 TCP/UDP、未命中的 HTTPS 与本机 mihomo 的其它流量必须保持现有纯转发行为。
- 进程设置模式的 mihomo 当前已将流量转发到 `127.0.0.1`，该路径本来就没有游戏 PID/真实源 IP；不得将其语义误用于外网客户端。

## 目标架构

HTTPS MITM 不能作为独立的前置/后置代理插在 WPE 外面；那会令 WPE 只看见本机中转连接并丢失外网客户端地址。必须在现有 SOCKS5 `ProxySession` 完成认证和 CONNECT 后，按会话条件选择处理路径：

```text
客户端 → WPE 既有 SOCKS5 端口 → ProxySession（认证/ClientIP/限额）
                                  ├─ 普通会话：当前 TCP/UDP 直通
                                  └─ 可映射 HTTPS：HttpsMitmSession
                                         ├─ 向客户端作 TLS 服务端握手
                                         ├─ 动态签发目标域名证书
                                         ├─ 解析 HTTP/1.1
                                         ├─ 命中：本地文件 / 固定响应
                                         └─ 未命中：向目标作 TLS 客户端握手并双向转发
```

只有存在已启用 HTTPS 规则的主机/端口、且根证书已由用户创建并信任时才尝试 MITM。当前按 SOCKS CONNECT 的主机名预判；IP CONNECT 与无规则继续当前直通路径。

## 实现边界与依赖

- 不把 Titanium.Web.Proxy 作为完整监听代理接入主链路：它拥有自己的 socket/会话生命周期，独立串联会污染 WPE 的外网 `ClientIP` 语义。
- 可在前期 POC 评估其兼容 `net48` 的 3.2.0 版本，用于证书/API 研究；不得让它成为主 SOCKS5 监听入口。
- 正式实现新增 WPE 自己管理的 `HttpsMitmSession`：`SslStream` 负责 TLS，证书组件负责 CA/站点证书，数据面只实现受限的 HTTP/1.1。
- MITM 会话不进入既有普通 TCP 字节滤镜，避免把 TLS 或解密 HTTP 的语义混淆、重复入列或误改数据。

## 配置、证书与规则

HTTPS 直接复用 `MapLocal` 的持久化模型、数据库、备份 XML、导入/导出和编辑器；不增加独立总开关。只要存在启用的 HTTPS 主机/端口规则，就在该 CONNECT 会话上启用 MITM；没有规则即不启用。远程映射明确保持 HTTP-only。

根证书为每台服务器首次生成后持续复用，仅在用户明确确认后安装到“当前用户”受信任根证书库。私钥以机器 DPAPI 放 `%PROGRAMDATA%\WPE64\https-mitm\`，不随安装包、备份或日志分发；提供导出公钥、取消信任和删除本地材料的独立操作。导出支持 `.cer` / `.crt` / `.der`、PEM（`.pem`），以及 Android 系统 CA 目录所需的 PEM `<subject_hash_old>.0`；后者自动按 Android 规则命名。绝不静默写入“本地计算机”根证书库。删除材料等同于主动轮换根证书，之后客户端必须重新安装新根证书。

## 状态机与回退

- 启用 HTTPS 映射前先检查证书可用性；失败则配置不生效。
- TLS 协商、证书校验或 HTTP/1.1 解析失败时关闭该会话；不得影响其它会话。客户端主动拒绝 WPE 证书的 SSPI `0x80090327` 是正常分支，静默处理，不刷系统错误日志。
- HTTP/2、QUIC 与 UDP 永远不进入 MITM，维持当前路径。

## 已完成项与验证

1. 独立 net48 POC：动态 CA、TLS 服务端/客户端双握手、HTTP/1.1 透传与本地响应，已通过。
2. `HttpsMitmCertificateManager`：机器 DPAPI 保护服务器级根 CA 私钥；首版 CurrentUser 材料自动原样迁移；显式创建、信任、取消信任、多格式导出（`.cer/.crt/.der/.pem`、Android `<hash>.0`）和删除材料（删除时同时取消信任）。根证书只显式信任到 CurrentUser 证书库。
3. `ProxySession` CONNECT 后接入 `HttpsMitmSession`：认证、ClientIP、账号、设备和连接限制仍先由既有 SOCKS5 会话处理；未命中时建立上游 TLS 并双向转发，包括请求头后的正文。
4. 证书管理位于代理设置，页面文案已经接入六语言 i18n；导出格式在程序内选择，系统对话框只选择保存位置并带入默认文件名。
5. Vue Release 构建、.NET Release 全解决方案构建，以及 `CheckUiCoupling.ps1` 均已通过。NuGet 漏洞索引因网络不可用产生的 `NU1900` 不影响编译结果。
