# HTTPS MITM POC（net48）

这是与产品工程完全隔离的验证程序：不进入 `WinSockPacketEditor.sln`，不引用
`WinsockPacketEditor`，不监听产品 SOCKS5 端口，也不会安装或修改系统证书库。
为使 net48 Schannel 能使用临时叶证书，它会把内存 PFX 重载到当前用户的临时密钥容器；
程序退出和证书对象释放后不保留根证书或产品配置。

它验证首期方案依赖的四件事：

1. .NET Framework 4.8 `CertificateRequest` 能创建根 CA，并签发带 DNS SAN 的叶证书；
2. 客户端能经该 CA 验证 MITM 的叶证书；
3. MITM 能以 `SslStream` 同时承接客户端 TLS 和上游 TLS；
4. HTTP/1.1 可在 TLS 内按路径返回本地响应，或将未命中请求转发给上游。

运行（从 WPE 仓库根目录）：

```powershell
& 'C:\Program Files\Microsoft Visual Studio\18\Community\MSBuild\Current\Bin\MSBuild.exe' tests\HttpsMitmPoc\HttpsMitmPoc.csproj -restore -p:Configuration=Release -verbosity:minimal -nologo
& tests\HttpsMitmPoc\bin\Release\net48\HttpsMitmPoc.exe
```

这不是生产级代理：不包含 SOCKS5 接入、证书持久化、系统根证书安装、SNI 解析、HTTP
流水线、chunked、HTTP/2、超时、取消或流量 Feed。它的唯一目的是决定正式实现能否保持
在 BCL TLS/证书 API 内，而无需先引入 Titanium.Web.Proxy 或 BouncyCastle。
