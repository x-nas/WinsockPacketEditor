# HTTPS 映射改造方案（待实施）

状态：已确认架构方向，尚未开始实现。

## 已完成的前置修复（2026-09-25）

- HTTP 列表嗅探改为先判定、后入队：不再把同一段数据先记成 TCP 再重复记成 HTTP；会话关闭时会刷出未完成残片。
- 嗅探缓存对所有路径（包括 `Content-Length`）执行 4 MB 上限，超限退回 TCP 展示，不影响转发。
- HTTP 条目现在保留过滤前与实际过滤后的字节及动作，展示不再假定它永远是未修改数据。
- HTTP 映射会等待完整请求头，支持 PATCH / DELETE / OPTIONS / TRACE，并正确解析 `Host` 的 IPv6 字面量和端口；映射改写只重写头部，二进制正文原样保留。

这些修复不包含 TLS 解密或 HTTPS 映射；它们是后续引入 `HttpsMitmSession` 的基础。

## 目标与首期范围

为代理模式现有 SOCKS5 端口上的 HTTPS（TCP）流量增加可选映射：按 `https://主机:端口/完整路径?查询串` 精确匹配，命中后返回本地文件或固定 HTTP 响应。

首期仅支持 HTTP/1.1；不支持 HTTP/2、HTTP/3/QUIC、WebSocket、gRPC、转发到另一 HTTPS URL，亦不尝试绕过证书固定或安全软件校验。TLS 握手失败或客户端拒绝 WPE 证书时必须直通并写系统日志。

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

只有存在已启用 HTTPS 规则的主机/端口时才尝试 MITM。SOCKS CONNECT 含域名时按其域名预判；IP CONNECT 时可在 TLS ClientHello 中读取 SNI 后决定。不能判定或无规则时直接沿用当前路径。

## 实现边界与依赖

- 不把 Titanium.Web.Proxy 作为完整监听代理接入主链路：它拥有自己的 socket/会话生命周期，独立串联会污染 WPE 的外网 `ClientIP` 语义。
- 可在前期 POC 评估其兼容 `net48` 的 3.2.0 版本，用于证书/API 研究；不得让它成为主 SOCKS5 监听入口。
- 正式实现新增 WPE 自己管理的 `HttpsMitmSession`：`SslStream` 负责 TLS，证书组件负责 CA/站点证书，数据面只实现受限的 HTTP/1.1。
- 需要新增专用 HTTPS Feed 条目（请求、响应、本地命中、固定响应），携带原始 `ProxySession` 的 ClientIP、账号、目标与 URL；不能把解密结果伪装成普通 TLS TCP 分片。
- 对 MITM 会话避免运行现有普通 TCP 字节滤镜，防止把 TLS 或解密 HTTP 的语义混淆、重复入列或误改数据。

## 配置、证书与规则

新增独立 HTTPS 映射模型与表，不改变旧 `MapLocal`/`MapRemote` 的 HTTP-only 语义：

- 总开关与 MITM 状态；
- 规则：启用、主机、端口、完整 Path+Query、动作（本地文件/固定响应）、文件路径或状态码/Content-Type/Body/可选响应头；
- 规则仅精确匹配；
- 数据库、备份 XML、导入/导出、空库初始化、Feed DTO、桥方法与 Vue 界面同步增加。

根证书为每台机器生成，仅在用户明确确认后安装到“当前用户”受信任根证书库。私钥放 `%LOCALAPPDATA%\WPE64\https-mitm\`，不随安装包、备份或日志分发；提供导出公钥、取消信任和删除本地材料的独立操作。绝不静默写入“本地计算机”根证书库。

## 状态机与回退

- 启用 HTTPS 映射前先检查证书可用性；失败则配置不生效。
- TLS 协商、证书校验或 HTTP/1.1 解析失败时记录“MITM 不兼容/客户端拒绝证书”，会话改走普通上游直通；不得阻断其它会话。
- 可按主机短时缓存失败直通决定，避免同一不兼容客户端反复触发失败。
- HTTP/2、QUIC 与 UDP 永远不进入 MITM，维持当前路径。

## 实施顺序与验证

1. 独立 net48 POC：动态 CA、TLS 服务端/客户端双握手、HTTP/1.1 透传与本地响应；确认包依赖/绑定重定向。
2. 新增证书生命周期和 HTTPS 规则数据层，完整补齐数据库/备份/导入导出。
3. 在 `ProxySession` CONNECT 后接入 `HttpsMitmSession`，保证真实 ClientIP、账号、设备/连接限制不变。
4. 接入 HTTPS Feed、规则界面与系统日志；限制缓存体积。
5. 回归：普通 SOCKS5、WPC、TCP/UDP、HTTP 映射、外网真实 ClientIP、本机 mihomo、TLS 不兼容、证书固定、应用退出清理。

改代码前必须先完成第 1 步 POC；POC 不通过时不要半集成到主代理链路。
