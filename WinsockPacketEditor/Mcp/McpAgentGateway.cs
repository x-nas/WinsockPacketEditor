using System;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Globalization;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WinsockPacketEditor.Mcp
{
    /// <summary>
    /// Local-only boundary for the external MCP process. This is intentionally separate from
    /// the injected-process IPC protocol: it only serves bounded, read-only snapshots.
    /// </summary>
    internal sealed class McpAgentGateway : IDisposable
    {
        private const int MaxFrameBytes = 1024 * 1024;
        private readonly CancellationTokenSource cancellation = new CancellationTokenSource();
        private readonly int processId = Process.GetCurrentProcess().Id;
        // The name is an unguessable capability; it is disclosed only through the
        // current user's LocalAppData discovery record.
        private readonly string pipeName = "WPE64-Mcp-" + Guid.NewGuid().ToString("N");
        private Task acceptLoop;

        public void Start()
        {
            PublishInstance(true);
            acceptLoop = Task.Run(() => AcceptLoopAsync(cancellation.Token));
        }

        public void Dispose()
        {
            cancellation.Cancel();
            PublishInstance(false);
            try { if (acceptLoop != null) acceptLoop.Wait(1000); } catch { }
            cancellation.Dispose();
        }

        private async Task AcceptLoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    using (var pipe = CreateCurrentUserPipe())
                    {
                        await pipe.WaitForConnectionAsync(token).ConfigureAwait(false);
                        await ServeAsync(pipe, token).ConfigureAwait(false);
                    }
                }
                catch (OperationCanceledException) { return; }
                catch (Exception ex) { Operate.DoLog("McpAgentGateway", ex); }
            }
        }

        private NamedPipeServerStream CreateCurrentUserPipe()
        {
            // Granting the pipe itself broadly is safe because its random, per-run
            // capability name lives only in the current user's protected discovery file.
            // It also works across Windows UAC integrity levels on .NET Framework 4.8.
            var security = new PipeSecurity();
            security.SetAccessRuleProtection(true, false);
            security.AddAccessRule(new PipeAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), PipeAccessRights.FullControl, AccessControlType.Allow));
            return new NamedPipeServerStream(pipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous, 0, 0, security);
        }

        private static async Task ServeAsync(Stream stream, CancellationToken token)
        {
            var bytes = await ReadFrameAsync(stream, token).ConfigureAwait(false);
            var request = JObject.Parse(Encoding.UTF8.GetString(bytes));
            var requestId = (string)request["requestId"];
            var operation = (string)request["operation"];
            JObject response;
            try
            {
                response = new JObject
                {
                    ["requestId"] = requestId,
                    ["ok"] = true,
                    ["result"] = await DispatchAsync(operation, request["arguments"] as JObject).ConfigureAwait(false)
                };
            }
            catch (Exception ex)
            {
                Operate.DoLog("McpAgentGateway." + operation, ex);
                response = new JObject { ["requestId"] = requestId, ["ok"] = false, ["error"] = "WPE gateway request failed." };
            }
            await WriteFrameAsync(stream, Encoding.UTF8.GetBytes(response.ToString(Formatting.None)), token).ConfigureAwait(false);
        }

        private static JToken Dispatch(string operation, JObject arguments)
        {
            if (operation == "runtime.status")
            {
                return ReadOnUi(() => new JObject
                {
                    ["version"] = Operate.SystemConfig.AssemblyVersion,
                    ["mode"] = Operate.ProxyConfig.Proxy.IsRunning ? "proxy" : "idle",
                    ["proxyRunning"] = Operate.ProxyConfig.Proxy.IsRunning,
                    ["captured"] = Operate.PacketConfig.Packet.TotalPackets,
                    ["proxyConnections"] = Operate.ProxyConfig.Proxy.SessionCount
                });
            }
            if (operation == "capture.search") return ReadOnUi(() => SearchPackets(arguments));
            if (operation == "capture.get") return ReadOnUi(() => GetPacket(arguments));
            if (operation == "logs.list") return ReadOnUi(() => ListLogs(arguments));
            if (operation == "filters.list") return ReadOnUi(() => ListFilters(arguments));
            if (operation == "accounts.list") return ReadOnUi(() => ListAccounts(arguments));
            if (operation == "connections.list") return ReadOnUi(() => ListConnections(arguments));
            if (operation == "executors.list") return ReadOnUi(ListExecutors);
            if (operation == "firewall.get") return ReadOnUi(GetFirewall);
            if (operation == "firewall.rules.list") return ReadOnUi(() => ListFirewallRules(arguments));
            if (operation == "proxy.settings.get") return ReadOnUi(GetProxySettings);
            if (operation == "proxy.runtime.get") return ReadOnUi(GetProxyRuntime);
            if (operation == "connections.summary.get") return ReadOnUi(GetConnectionsSummary);
            if (operation == "proxy.failures.list") return ReadOnUi(() => ListProxyFailures(arguments));
            if (operation == "proxy.health.get") return ReadOnUi(GetProxyHealth);
            if (operation == "executors.detail.get") return ReadOnUi(GetExecutorsDetail);
            if (operation == "storage.health.get") return ReadOnUi(GetStorageHealth);
            if (operation == "bytes.transcode") return BytesTranscode(arguments);
            if (operation == "bytes.compare") return BytesCompare(arguments);
            if (operation == "bytes.extract") return BytesExtract(arguments);
            throw new InvalidOperationException("The requested MCP operation is not available in phase 1.");
        }

        private static Task<JToken> DispatchAsync(string operation, JObject arguments)
        {
            if (operation == "filters.setEnabled") return SetFilterEnabledAsync(arguments);
            if (operation == "accounts.setEnabled") return SetAccountEnabledAsync(arguments);
            if (operation == "proxy.auth.setEnabled") return SetProxyAuthEnabledAsync(arguments);
            if (operation == "proxy.http.setEnabled") return SetProxyHttpEnabledAsync(arguments);
            if (operation == "proxy.maxConnections.set") return SetProxyMaxConnectionsAsync(arguments);
            if (operation == "proxy.socks5Port.set") return SetProxySocks5PortAsync(arguments);
            if (operation == "proxy.httpPort.set") return SetProxyHttpPortAsync(arguments);
            if (operation == "firewall.setEnabled") return SetFirewallEnabledAsync(arguments);
            if (operation == "proxy.onlyWpc.setEnabled") return SetOnlyWpcEnabledAsync(arguments);
            if (operation == "proxy.bindIp.set") return SetProxyBindIpAsync(arguments);
            if (operation == "proxy.external.setEnabled") return SetExternalProxyEnabledAsync(arguments);
            if (operation == "proxy.start") return StartProxyAsync(arguments);
            if (operation == "proxy.stop") return StopProxyAsync(arguments);
            if (operation == "firewall.rule.add") return AddFirewallRuleAsync(arguments);
            if (operation == "firewall.rule.remove") return RemoveFirewallRuleAsync(arguments);
            return Task.FromResult(Dispatch(operation, arguments));
        }

        private static async Task<JToken> SetAccountEnabledAsync(JObject arguments)
        {
            var accountId = (string)arguments?["id"];
            var enabledToken = arguments?["enabled"];
            var idempotencyKey = (string)arguments?["idempotencyKey"];
            if (string.IsNullOrWhiteSpace(accountId)) throw new InvalidOperationException("An account id is required.");
            if (enabledToken == null || enabledToken.Type != JTokenType.Boolean) throw new InvalidOperationException("A boolean enabled value is required.");
            if (!Guid.TryParse(accountId, out _)) throw new InvalidOperationException("Account id must be a GUID.");

            var enabled = enabledToken.Value<bool>();
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("accounts.setEnabled", idempotencyKey, arguments, out prior)) return prior;
            var account = ReadOnUi(() => FindAccount(accountId));
            if (account == null) throw new InvalidOperationException("The account does not exist.");

            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync(
                "accounts.setEnabled",
                idempotencyKey,
                arguments,
                "将代理账号“" + account.UserName + "”" + (enabled ? "启用。" : "停用。"),
                () =>
                {
                    var current = FindAccount(accountId);
                    if (current == null) throw new InvalidOperationException("The account no longer exists.");
                    if (current.IsEnable == enabled) return new JObject { ["found"] = true, ["changed"] = false, ["enabled"] = enabled };
                    if (!Operate.ProxyConfig.Account.SetAccountEnable_ById(accountId, enabled)) throw new InvalidOperationException("The account no longer exists.");
                    return new JObject { ["found"] = true, ["changed"] = true, ["enabled"] = enabled };
                })).ConfigureAwait(false);
        }

        private static async Task<JToken> SetProxyAuthEnabledAsync(JObject arguments)
        {
            var enabledToken = arguments?["enabled"];
            var idempotencyKey = (string)arguments?["idempotencyKey"];
            if (enabledToken == null || enabledToken.Type != JTokenType.Boolean) throw new InvalidOperationException("A boolean enabled value is required.");
            var enabled = enabledToken.Value<bool>();
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("proxy.auth.setEnabled", idempotencyKey, arguments, out prior)) return prior;

            var config = ReadOnUi(() => new { enabled = Operate.ProxyConfig.Proxy.Enable_Auth, onlyWpc = Operate.ProxyConfig.Proxy.Only_WPC_Client });
            if (!enabled && config.onlyWpc) throw new InvalidOperationException("Proxy authentication cannot be disabled while Only_WPC_Client is enabled.");

            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync(
                "proxy.auth.setEnabled",
                idempotencyKey,
                arguments,
                (enabled ? "启用" : "停用") + "代理身份认证；只允许 WPC 客户端当前为" + (config.onlyWpc ? "开启" : "关闭") + "。",
                () =>
                {
                    if (Operate.ProxyConfig.Proxy.Enable_Auth == enabled) return new JObject { ["changed"] = false, ["enabled"] = enabled };
                    if (!enabled && Operate.ProxyConfig.Proxy.Only_WPC_Client) throw new InvalidOperationException("Proxy authentication cannot be disabled while Only_WPC_Client is enabled.");
                    Operate.ProxyConfig.Proxy.Enable_Auth = enabled;
                    Operate.SystemConfig.SaveProxyMode_ToDB();
                    return new JObject { ["changed"] = true, ["enabled"] = enabled };
                })).ConfigureAwait(false);
        }

        private static async Task<JToken> SetProxyHttpEnabledAsync(JObject arguments)
        {
            var enabledToken = arguments?["enabled"];
            var idempotencyKey = (string)arguments?["idempotencyKey"];
            if (enabledToken == null || enabledToken.Type != JTokenType.Boolean) throw new InvalidOperationException("A boolean enabled value is required.");
            var enabled = enabledToken.Value<bool>();
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("proxy.http.setEnabled", idempotencyKey, arguments, out prior)) return prior;

            var config = ReadOnUi(() => new { enabled = Operate.ProxyConfig.Proxy.Enable_HTTP, httpPort = (int)Operate.ProxyConfig.Proxy.HTTP_Port, socks5Enabled = Operate.ProxyConfig.Proxy.Enable_SOCKS5, socks5Port = (int)Operate.ProxyConfig.Proxy.SOCKS5_Port });
            if (enabled)
            {
                if (!config.socks5Enabled) throw new InvalidOperationException("HTTP proxy requires SOCKS5 to be enabled.");
                if (config.httpPort < 1 || config.httpPort > 65535) throw new InvalidOperationException("The HTTP proxy port must be between 1 and 65535.");
                if (config.httpPort == config.socks5Port) throw new InvalidOperationException("HTTP and SOCKS5 proxy ports must be different.");
            }

            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync(
                "proxy.http.setEnabled",
                idempotencyKey,
                arguments,
                (enabled ? "启用" : "停用") + " HTTP 代理（端口 " + config.httpPort + "）。",
                () =>
                {
                    if (Operate.ProxyConfig.Proxy.Enable_HTTP == enabled) return new JObject { ["changed"] = false, ["enabled"] = enabled, ["port"] = config.httpPort };
                    if (enabled)
                    {
                        if (!Operate.ProxyConfig.Proxy.Enable_SOCKS5) throw new InvalidOperationException("HTTP proxy requires SOCKS5 to be enabled.");
                        if (Operate.ProxyConfig.Proxy.HTTP_Port == Operate.ProxyConfig.Proxy.SOCKS5_Port) throw new InvalidOperationException("HTTP and SOCKS5 proxy ports must be different.");
                    }
                    Operate.ProxyConfig.Proxy.Enable_HTTP = enabled;
                    Operate.SystemConfig.SaveProxyMode_ToDB();
                    return new JObject { ["changed"] = true, ["enabled"] = enabled, ["port"] = (int)Operate.ProxyConfig.Proxy.HTTP_Port };
                })).ConfigureAwait(false);
        }

        private static async Task<JToken> SetProxyMaxConnectionsAsync(JObject arguments)
        {
            var valueToken = arguments?["maxConnection"];
            var idempotencyKey = (string)arguments?["idempotencyKey"];
            if (valueToken == null || valueToken.Type != JTokenType.Integer) throw new InvalidOperationException("maxConnection must be an integer.");
            var requested = valueToken.Value<int>();
            var cap = ReadOnUi(() => Operate.ProxyConfig.Proxy.MaxConnectionCap());
            if (requested < 1 || requested > cap) throw new InvalidOperationException("maxConnection is outside the current machine limit.");
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("proxy.maxConnections.set", idempotencyKey, arguments, out prior)) return prior;

            var current = ReadOnUi(() => Operate.ProxyConfig.Proxy.MaxConnectionNumber);
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync(
                "proxy.maxConnections.set",
                idempotencyKey,
                arguments,
                "将代理最大连接数从 " + current + " 调整为 " + requested + "（当前上限 " + cap + "）。",
                () =>
                {
                    var liveCap = Operate.ProxyConfig.Proxy.MaxConnectionCap();
                    if (requested < 1 || requested > liveCap) throw new InvalidOperationException("maxConnection is outside the current machine limit.");
                    var changed = Operate.ProxyConfig.Proxy.MaxConnectionNumber != requested;
                    if (changed)
                    {
                        Operate.ProxyConfig.Proxy.MaxConnectionNumber = requested;
                        Operate.SystemConfig.SaveProxyMode_ToDB();
                    }
                    return new JObject { ["changed"] = changed, ["maxConnection"] = requested, ["cap"] = liveCap };
                })).ConfigureAwait(false);
        }

        private static async Task<JToken> SetProxySocks5PortAsync(JObject arguments)
        {
            var valueToken = arguments?["port"];
            var idempotencyKey = (string)arguments?["idempotencyKey"];
            if (valueToken == null || valueToken.Type != JTokenType.Integer) throw new InvalidOperationException("port must be an integer.");
            var requested = valueToken.Value<int>();
            if (requested < 1 || requested > 65535) throw new InvalidOperationException("The SOCKS5 port must be between 1 and 65535.");
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("proxy.socks5Port.set", idempotencyKey, arguments, out prior)) return prior;

            var config = ReadOnUi(() => new { current = (int)Operate.ProxyConfig.Proxy.SOCKS5_Port, httpEnabled = Operate.ProxyConfig.Proxy.Enable_HTTP, httpPort = (int)Operate.ProxyConfig.Proxy.HTTP_Port });
            if (config.httpEnabled && requested == config.httpPort) throw new InvalidOperationException("SOCKS5 and HTTP proxy ports must be different.");
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync(
                "proxy.socks5Port.set",
                idempotencyKey,
                arguments,
                "将 SOCKS5 监听端口从 " + config.current + " 调整为 " + requested + "。",
                () =>
                {
                    if (requested < 1 || requested > 65535) throw new InvalidOperationException("The SOCKS5 port must be between 1 and 65535.");
                    if (Operate.ProxyConfig.Proxy.Enable_HTTP && requested == Operate.ProxyConfig.Proxy.HTTP_Port) throw new InvalidOperationException("SOCKS5 and HTTP proxy ports must be different.");
                    var changed = Operate.ProxyConfig.Proxy.SOCKS5_Port != requested;
                    if (changed)
                    {
                        Operate.ProxyConfig.Proxy.SOCKS5_Port = (ushort)requested;
                        Operate.SystemConfig.SaveProxyMode_ToDB();
                    }
                    return new JObject { ["changed"] = changed, ["port"] = requested };
                })).ConfigureAwait(false);
        }

        private static async Task<JToken> SetProxyHttpPortAsync(JObject arguments)
        {
            var valueToken = arguments?["port"];
            var idempotencyKey = (string)arguments?["idempotencyKey"];
            if (valueToken == null || valueToken.Type != JTokenType.Integer) throw new InvalidOperationException("port must be an integer.");
            var requested = valueToken.Value<int>();
            if (requested < 1 || requested > 65535) throw new InvalidOperationException("The HTTP port must be between 1 and 65535.");
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("proxy.httpPort.set", idempotencyKey, arguments, out prior)) return prior;

            var config = ReadOnUi(() => new { current = (int)Operate.ProxyConfig.Proxy.HTTP_Port, httpEnabled = Operate.ProxyConfig.Proxy.Enable_HTTP, socks5Enabled = Operate.ProxyConfig.Proxy.Enable_SOCKS5, socks5Port = (int)Operate.ProxyConfig.Proxy.SOCKS5_Port });
            if (!config.httpEnabled) throw new InvalidOperationException("The HTTP proxy is disabled; enable it before changing its port.");
            if (config.socks5Enabled && requested == config.socks5Port) throw new InvalidOperationException("HTTP and SOCKS5 proxy ports must be different.");
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync(
                "proxy.httpPort.set",
                idempotencyKey,
                arguments,
                "将 HTTP 监听端口从 " + config.current + " 调整为 " + requested + "。",
                () =>
                {
                    if (requested < 1 || requested > 65535) throw new InvalidOperationException("The HTTP port must be between 1 and 65535.");
                    if (!Operate.ProxyConfig.Proxy.Enable_HTTP) throw new InvalidOperationException("The HTTP proxy is disabled; enable it before changing its port.");
                    if (Operate.ProxyConfig.Proxy.Enable_SOCKS5 && requested == Operate.ProxyConfig.Proxy.SOCKS5_Port) throw new InvalidOperationException("HTTP and SOCKS5 proxy ports must be different.");
                    var changed = Operate.ProxyConfig.Proxy.HTTP_Port != requested;
                    if (changed)
                    {
                        Operate.ProxyConfig.Proxy.HTTP_Port = (ushort)requested;
                        Operate.SystemConfig.SaveProxyMode_ToDB();
                    }
                    return new JObject { ["changed"] = changed, ["port"] = requested };
                })).ConfigureAwait(false);
        }

        private static async Task<JToken> SetFirewallEnabledAsync(JObject arguments)
        {
            var enabledToken = arguments?["enabled"];
            var idempotencyKey = (string)arguments?["idempotencyKey"];
            if (enabledToken == null || enabledToken.Type != JTokenType.Boolean) throw new InvalidOperationException("A boolean enabled value is required.");
            var enabled = enabledToken.Value<bool>();
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("firewall.setEnabled", idempotencyKey, arguments, out prior)) return prior;
            var current = ReadOnUi(() => Operate.ProxyConfig.Proxy.EnableFireWall);
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync(
                "firewall.setEnabled",
                idempotencyKey,
                arguments,
                (enabled ? "启用" : "停用") + "代理防火墙。",
                () =>
                {
                    var changed = Operate.ProxyConfig.Proxy.EnableFireWall != enabled;
                    if (changed)
                    {
                        Operate.ProxyConfig.Proxy.EnableFireWall = enabled;
                        Operate.SystemConfig.SaveProxyMode_ToDB();
                    }
                    return new JObject { ["changed"] = changed, ["enabled"] = enabled };
                })).ConfigureAwait(false);
        }

        private static async Task<JToken> SetOnlyWpcEnabledAsync(JObject arguments)
        {
            var enabledToken = arguments?["enabled"];
            var idempotencyKey = (string)arguments?["idempotencyKey"];
            if (enabledToken == null || enabledToken.Type != JTokenType.Boolean) throw new InvalidOperationException("A boolean enabled value is required.");
            var enabled = enabledToken.Value<bool>();
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("proxy.onlyWpc.setEnabled", idempotencyKey, arguments, out prior)) return prior;
            var config = ReadOnUi(() => new { current = Operate.ProxyConfig.Proxy.Only_WPC_Client, auth = Operate.ProxyConfig.Proxy.Enable_Auth });
            if (enabled && !config.auth) throw new InvalidOperationException("Only-WPC mode requires proxy authentication to be enabled.");
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync(
                "proxy.onlyWpc.setEnabled",
                idempotencyKey,
                arguments,
                (enabled ? "启用" : "停用") + "只允许 WPC 客户端连接。",
                () =>
                {
                    if (enabled && !Operate.ProxyConfig.Proxy.Enable_Auth) throw new InvalidOperationException("Only-WPC mode requires proxy authentication to be enabled.");
                    var changed = Operate.ProxyConfig.Proxy.Only_WPC_Client != enabled;
                    if (changed)
                    {
                        Operate.ProxyConfig.Proxy.Only_WPC_Client = enabled;
                        Operate.SystemConfig.SaveProxyMode_ToDB();
                    }
                    return new JObject { ["changed"] = changed, ["enabled"] = enabled };
                })).ConfigureAwait(false);
        }

        private static async Task<JToken> SetFilterEnabledAsync(JObject arguments)
        {
            var filterId = (string)arguments?["id"];
            var enabledToken = arguments?["enabled"];
            var idempotencyKey = (string)arguments?["idempotencyKey"];
            if (string.IsNullOrWhiteSpace(filterId)) throw new InvalidOperationException("A filter id is required.");
            if (enabledToken == null || enabledToken.Type != JTokenType.Boolean) throw new InvalidOperationException("A boolean enabled value is required.");
            if (!Guid.TryParse(filterId, out _)) throw new InvalidOperationException("Filter id must be a GUID.");

            var enabled = enabledToken.Value<bool>();
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("filters.setEnabled", idempotencyKey, arguments, out prior)) return prior;
            var filterName = ReadOnUi(() => GetFilterName(filterId));
            if (filterName == null) throw new InvalidOperationException("The filter does not exist.");

            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync(
                "filters.setEnabled",
                idempotencyKey,
                arguments,
                "将筛选器“" + DisplayFilterName(filterName) + "”" + (enabled ? "启用。" : "停用。"),
                () =>
                {
                    var current = GetFilterEnabled(filterId);
                    if (!current.HasValue) throw new InvalidOperationException("The filter no longer exists.");
                    if (current.Value == enabled) return new JObject { ["found"] = true, ["changed"] = false, ["enabled"] = enabled };
                    if (!Operate.FilterConfig.List.SetFilterEnable_ById(filterId, enabled)) throw new InvalidOperationException("The filter no longer exists.");
                    return new JObject { ["found"] = true, ["changed"] = true, ["enabled"] = enabled };
                })).ConfigureAwait(false);
        }

        private static async Task<JToken> RemoveFirewallRuleAsync(JObject arguments)
        {
            var list = ReadFirewallListKind((string)arguments?["list"]);
            var address = ((string)arguments?["address"] ?? string.Empty).Trim();
            var idempotencyKey = (string)arguments?["idempotencyKey"];
            if (address.Length == 0) throw new InvalidOperationException("A firewall rule address is required.");
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("firewall.rule.remove", idempotencyKey, arguments, out prior)) return prior;
            if (!ReadOnUi(() => FirewallRuleExists(list, address))) throw new InvalidOperationException("The firewall rule does not exist.");

            var listName = list == "black" ? "黑名单" : "白名单";
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync(
                "firewall.rule.remove",
                idempotencyKey,
                arguments,
                "从" + listName + "移除规则“" + address + "”。此变更可能影响代理连接访问控制。",
                () =>
                {
                    if (!Operate.ProxyConfig.Proxy.DeleteIPRule(list == "black", address)) throw new InvalidOperationException("The firewall rule no longer exists.");
                    return new JObject { ["list"] = list, ["address"] = address, ["removed"] = true };
                })).ConfigureAwait(false);
        }

        private static async Task<JToken> AddFirewallRuleAsync(JObject arguments)
        {
            var list = ReadFirewallListKind((string)arguments?["list"]);
            var address = ((string)arguments?["address"] ?? string.Empty).Trim();
            var expiryEnabledToken = arguments?["expiryEnabled"];
            var expiryEnabled = expiryEnabledToken != null && expiryEnabledToken.Type == JTokenType.Boolean && expiryEnabledToken.Value<bool>();
            var expiryText = ((string)arguments?["expiryTime"] ?? string.Empty).Trim();
            var idempotencyKey = (string)arguments?["idempotencyKey"];

            if (address.Length == 0) throw new InvalidOperationException("A firewall rule address is required.");
            if (expiryEnabledToken != null && expiryEnabledToken.Type != JTokenType.Boolean) throw new InvalidOperationException("expiryEnabled must be a boolean.");
            if (!expiryEnabled && expiryText.Length > 0) throw new InvalidOperationException("expiryTime requires expiryEnabled=true.");

            DateTime expiry = Operate.SystemConfig.MaxDateTime;
            if (expiryEnabled)
            {
                DateTimeOffset parsed;
                if (expiryText.Length == 0 || !DateTimeOffset.TryParse(expiryText, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out parsed))
                {
                    throw new InvalidOperationException("expiryTime must be an ISO-8601 date-time when expiryEnabled=true.");
                }
                expiry = parsed.LocalDateTime;
            }

            JObject prior;
            if (McpWriteGuard.TryGetCompleted("firewall.rule.add", idempotencyKey, arguments, out prior)) return prior;

            var validationError = ReadOnUi(() => Operate.ProxyConfig.Proxy.ValidateIPRule(list == "black", string.Empty, address));
            if (!string.IsNullOrEmpty(validationError)) throw new InvalidOperationException(validationError);

            var listName = list == "black" ? "黑名单" : "白名单";
            var summary = "向" + listName + "新增规则“" + address + "”";
            if (expiryEnabled) summary += "，到期时间 " + expiry.ToString("yyyy-MM-dd HH:mm:ss") + "。";
            else summary += "，永久有效。";

            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync(
                "firewall.rule.add",
                idempotencyKey,
                arguments,
                summary + " 此变更可能影响代理连接访问控制。",
                async () =>
                {
                    var error = await Operate.ProxyConfig.Proxy.SaveIPRuleAsync(
                        list == "black", string.Empty, address, expiryEnabled,
                        expiryEnabled ? expiry.ToString("o", CultureInfo.InvariantCulture) : string.Empty);
                    if (!string.IsNullOrEmpty(error)) throw new InvalidOperationException(error);
                    return new JObject
                    {
                        ["list"] = list,
                        ["address"] = address,
                        ["added"] = true,
                        ["expiryEnabled"] = expiryEnabled,
                        ["expiryTime"] = expiryEnabled ? (JToken)expiry.ToUniversalTime().ToString("o") : JValue.CreateNull()
                    };
                })).ConfigureAwait(false);
        }

        private static JObject SearchPackets(JObject arguments)
        {
            var limit = Math.Max(1, Math.Min(200, (int?)arguments?["limit"] ?? 50));
            var offset = ReadOffset(arguments);
            var pattern = ((string)arguments?["pattern"] ?? string.Empty).Trim();
            var rows = new JArray();
            var list = Operate.PacketConfig.List.lstPacketInfo;
            var matched = 0;
            for (var i = list.Count - 1; i >= 0 && rows.Count < limit; i--)
            {
                var packet = list[i];
                if (packet == null) continue;
                var searchable = (packet.PacketFrom ?? string.Empty) + " " + (packet.PacketTo ?? string.Empty) + " " + (packet.PacketData ?? string.Empty);
                if (pattern.Length > 0 && searchable.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) < 0) continue;
                if (matched++ < offset) continue;
                var preview = packet.PacketData ?? string.Empty;
                if (preview.Length > 192) preview = preview.Substring(0, 192);
                rows.Add(new JObject { ["id"] = packet.Id, ["time"] = packet.PacketTime.ToUniversalTime().ToString("o"), ["type"] = (int)packet.PacketType, ["length"] = packet.PacketLen, ["preview"] = preview, ["from"] = packet.PacketFrom, ["to"] = packet.PacketTo, ["action"] = (int)packet.FilterAction });
            }
            return new JObject { ["rows"] = rows, ["nextCursor"] = NextCursor(offset, rows.Count, matched < list.Count) };
        }

        private static bool? GetFilterEnabled(string filterId)
        {
            foreach (var filter in Operate.FilterConfig.List.lstFilterInfo)
            {
                if (filter != null && string.Equals(filter.FID.ToString(), filterId, StringComparison.OrdinalIgnoreCase)) return filter.IsEnable;
            }
            return null;
        }

        private static string GetFilterName(string filterId)
        {
            foreach (var filter in Operate.FilterConfig.List.lstFilterInfo)
            {
                if (filter != null && string.Equals(filter.FID.ToString(), filterId, StringComparison.OrdinalIgnoreCase)) return filter.FName;
            }
            return null;
        }

        private static AccountInfo FindAccount(string accountId)
        {
            Guid id;
            if (!Guid.TryParse(accountId, out id)) return null;
            foreach (var account in Operate.ProxyConfig.Account.lstAccountInfo)
            {
                if (account != null && account.AID == id) return account;
            }
            return null;
        }

        private static string DisplayFilterName(string name)
        {
            var value = (name ?? string.Empty).Replace("\r", " ").Replace("\n", " ").Trim();
            if (value.Length == 0) return "未命名滤镜";
            return value.Length <= 120 ? value : value.Substring(0, 120) + "…";
        }

        private static JObject GetPacket(JObject arguments)
        {
            var id = (long?)arguments?["id"];
            if (!id.HasValue || id.Value < 1) throw new InvalidOperationException("A positive packet id is required.");
            var packet = Operate.PacketConfig.List.GetPacketById(id.Value);
            if (packet == null) return new JObject { ["id"] = id.Value, ["found"] = false, ["truncated"] = false, ["sha256"] = JValue.CreateNull(), ["payloadBase64"] = JValue.CreateNull() };

            var includePayload = (bool?)arguments?["includePayload"] ?? false;
            var bytes = packet.PacketBuffer ?? new byte[0];
            const int maxPayloadBytes = 4 * 1024 * 1024;
            var truncated = bytes.Length > maxPayloadBytes;
            var result = new JObject { ["id"] = id.Value, ["found"] = true, ["truncated"] = truncated };
            using (var sha = System.Security.Cryptography.SHA256.Create()) result["sha256"] = BitConverter.ToString(sha.ComputeHash(bytes)).Replace("-", string.Empty).ToLowerInvariant();
            result["payloadBase64"] = includePayload ? Convert.ToBase64String(bytes, 0, Math.Min(bytes.Length, maxPayloadBytes)) : (JToken)JValue.CreateNull();
            return result;
        }

        private static JObject ListLogs(JObject arguments)
        {
            var kind = (string)arguments?["kind"];
            var limit = Math.Max(1, Math.Min(200, (int?)arguments?["limit"] ?? 50));
            var offset = ReadOffset(arguments);
            var rows = new JArray();
            var total = 0;
            if (kind == "system")
            {
                var list = Operate.LogConfig.List.lstLogInfo;
                total = list.Count;
                for (var i = list.Count - 1 - offset; i >= 0 && rows.Count < limit; i--) { var x = list[i]; rows.Add(new JObject { ["time"] = x.LogTime.ToUniversalTime().ToString("o"), ["source"] = x.FuncName, ["message"] = x.LogContent }); }
            }
            else if (kind == "filter")
            {
                var list = Operate.LogConfig.List.lstFilterLogInfo;
                total = list.Count;
                for (var i = list.Count - 1 - offset; i >= 0 && rows.Count < limit; i--) { var x = list[i]; rows.Add(new JObject { ["time"] = x.LogTime.ToUniversalTime().ToString("o"), ["filter"] = x.FName, ["action"] = (int)x.FAction, ["matches"] = x.MatchNum, ["type"] = (int)x.PacketType, ["length"] = x.PacketLen }); }
            }
            else if (kind == "proxy")
            {
                var list = Operate.LogConfig.List.lstProxyLogInfo;
                total = list.Count;
                for (var i = list.Count - 1 - offset; i >= 0 && rows.Count < limit; i--) { var x = list[i]; rows.Add(new JObject { ["time"] = x.LogTime.ToUniversalTime().ToString("o"), ["account"] = x.UserName, ["ip"] = x.LoginIP, ["message"] = x.LogContent }); }
            }
            else throw new InvalidOperationException("Log kind must be system, filter, or proxy.");
            return new JObject { ["rows"] = rows, ["nextCursor"] = NextCursor(offset, rows.Count, offset + rows.Count < total) };
        }

        private static JObject ListFilters(JObject arguments)
        {
            var limit = Math.Max(1, Math.Min(200, (int?)arguments?["limit"] ?? 50));
            var offset = ReadOffset(arguments);
            var rows = new JArray();
            var list = Operate.FilterConfig.List.lstFilterInfo;
            for (var i = offset; i < list.Count && rows.Count < limit; i++)
            {
                var row = FilterRow.From_(list[i]);
                if (row != null) rows.Add(JObject.FromObject(row));
            }
            return new JObject { ["rows"] = rows, ["nextCursor"] = NextCursor(offset, rows.Count, offset + rows.Count < list.Count) };
        }

        private static JObject ListAccounts(JObject arguments)
        {
            var limit = Math.Max(1, Math.Min(200, (int?)arguments?["limit"] ?? 50));
            var offset = ReadOffset(arguments);
            var rows = new JArray();
            var list = Operate.ProxyConfig.Account.lstAccountInfo;
            for (var i = offset; i < list.Count && rows.Count < limit; i++)
            {
                var row = AccountRow.From_(list[i]);
                if (row != null) rows.Add(JObject.FromObject(row));
            }
            return new JObject { ["rows"] = rows, ["nextCursor"] = NextCursor(offset, rows.Count, offset + rows.Count < list.Count) };
        }

        private static JObject ListConnections(JObject arguments)
        {
            var limit = Math.Max(1, Math.Min(200, (int?)arguments?["limit"] ?? 50));
            var offset = ReadOffset(arguments);
            var rows = new JArray();
            var list = Operate.ProxyConfig.Account.lstAuthInfo;
            for (var i = offset; i < list.Count && rows.Count < limit; i++)
            {
                var row = AuthRow.From_(list[i]);
                if (row != null) rows.Add(JObject.FromObject(row));
            }
            return new JObject { ["rows"] = rows, ["nextCursor"] = NextCursor(offset, rows.Count, offset + rows.Count < list.Count) };
        }

        private static int ReadOffset(JObject arguments)
        {
            var cursor = (string)arguments?["cursor"];
            if (string.IsNullOrEmpty(cursor)) return 0;
            try { var value = Encoding.UTF8.GetString(Convert.FromBase64String(cursor)); var offset = int.Parse(value); return offset >= 0 ? offset : 0; }
            catch { throw new InvalidOperationException("Invalid page cursor."); }
        }

        private static JToken NextCursor(int offset, int count, bool hasMore)
        {
            return hasMore ? (JToken)Convert.ToBase64String(Encoding.UTF8.GetBytes((offset + count).ToString())) : JValue.CreateNull();
        }

        private static JObject ListExecutors()
        {
            return new JObject
            {
                ["sendRunning"] = Operate.SendConfig.List.SendExecute_Count(),
                ["robotRunning"] = Operate.RobotConfig.List.RobotExecute_Count()
            };
        }

        private static JObject GetFirewall()
        {
            return new JObject
            {
                ["enabled"] = Operate.ProxyConfig.Proxy.EnableFireWall,
                ["autoWhiteListAuthSuccess"] = Operate.ProxyConfig.Proxy.FireWall_AutoWhiteList_AuthSuccess,
                ["autoBlackListUnsupported"] = Operate.ProxyConfig.Proxy.FireWall_AutoBlackList_UnSupport,
                ["autoBlackListAuthFail"] = Operate.ProxyConfig.Proxy.FireWall_AutoBlackList_AuthFail,
                ["autoBlackListMinutes"] = Operate.ProxyConfig.Proxy.FireWall_AutoBlackList_Minutes,
                ["autoClearExpired"] = Operate.ProxyConfig.Proxy.FireWall_AutoClear_Expiry
            };
        }

        private static JObject GetProxySettings()
        {
            return new JObject
            {
                ["socks5Enabled"] = Operate.ProxyConfig.Proxy.Enable_SOCKS5,
                ["socks5Port"] = (int)Operate.ProxyConfig.Proxy.SOCKS5_Port,
                ["proxyIpAuto"] = Operate.ProxyConfig.Proxy.ProxyIP_Auto,
                ["proxyIp"] = Operate.ProxyConfig.Proxy.ProxyIP ?? string.Empty,
                ["httpEnabled"] = Operate.ProxyConfig.Proxy.Enable_HTTP,
                ["httpPort"] = (int)Operate.ProxyConfig.Proxy.HTTP_Port,
                ["authEnabled"] = Operate.ProxyConfig.Proxy.Enable_Auth,
                ["onlyWpc"] = Operate.ProxyConfig.Proxy.Only_WPC_Client,
                ["maxConnection"] = Operate.ProxyConfig.Proxy.MaxConnectionNumber,
                ["maxConnectionCap"] = Operate.ProxyConfig.Proxy.MaxConnectionCap(),
                ["externalProxyEnabled"] = Operate.ProxyConfig.Proxy.Enable_ExternalProxy,
                ["externalProxyIp"] = Operate.ProxyConfig.Proxy.ExternalProxy_IP ?? string.Empty,
                ["externalProxyPort"] = (int)Operate.ProxyConfig.Proxy.ExternalProxy_Port,
                ["externalProxyAppointPort"] = Operate.ProxyConfig.Proxy.Enable_ExternalProxy_AppointPort,
                ["externalProxyAuthEnabled"] = Operate.ProxyConfig.Proxy.Enable_ExternalProxy_Auth,
                ["running"] = Operate.ProxyConfig.Proxy.IsRunning
            };
        }

        private static JObject GetProxyRuntime()
        {
            var result = GetProxySettings();
            result["sessionCount"] = Operate.ProxyConfig.Proxy.SessionCount;
            result["capturedPackets"] = Operate.PacketConfig.Packet.TotalPackets;
            result["sampledAtUtc"] = DateTime.UtcNow.ToString("o");
            return result;
        }

        private static JObject GetConnectionsSummary()
        {
            var rows = Operate.ProxyConfig.Account.GetClientConnections(string.Empty) ?? new ClientConnRow[0];
            var udp = 0;
            var wpc = 0;
            foreach (var row in rows)
            {
                if (row.Udp) udp++;
                if (row.Wpc) wpc++;
            }
            return new JObject
            {
                ["total"] = rows.Length,
                ["tcp"] = rows.Length - udp,
                ["udp"] = udp,
                ["wpcControl"] = wpc,
                ["ordinary"] = rows.Length - wpc,
                ["sampledAtUtc"] = DateTime.UtcNow.ToString("o")
            };
        }

        private static JObject ListProxyFailures(JObject arguments)
        {
            var limit = Math.Max(1, Math.Min(50, (int?)arguments?["limit"] ?? 20));
            var rows = new JArray();
            var list = Operate.LogConfig.List.lstProxyLogInfo;
            for (var i = list.Count - 1; i >= 0 && rows.Count < limit; i--)
            {
                var entry = list[i];
                var message = (entry.LogContent ?? string.Empty).Replace("\r", " ").Replace("\n", " ").Trim();
                var lower = message.ToLowerInvariant();
                if (lower.IndexOf("fail", StringComparison.Ordinal) < 0 && lower.IndexOf("error", StringComparison.Ordinal) < 0 && lower.IndexOf("exception", StringComparison.Ordinal) < 0 && message.IndexOf("失败", StringComparison.Ordinal) < 0 && message.IndexOf("错误", StringComparison.Ordinal) < 0 && message.IndexOf("异常", StringComparison.Ordinal) < 0) continue;
                if (message.Length > 160) message = message.Substring(0, 160) + "…";
                rows.Add(new JObject { ["time"] = entry.LogTime.ToUniversalTime().ToString("o"), ["summary"] = message });
            }
            return new JObject { ["rows"] = rows, ["sampledAtUtc"] = DateTime.UtcNow.ToString("o") };
        }

        private static JObject GetProxyHealth()
        {
            var settings = GetProxySettings();
            var socks = (bool)settings["socks5Enabled"];
            var http = (bool)settings["httpEnabled"];
            var socksPort = (int)settings["socks5Port"];
            var httpPort = (int)settings["httpPort"];
            var portConflict = socks && http && socksPort == httpPort;
            var running = (bool)settings["running"];
            return new JObject
            {
                ["healthy"] = !portConflict,
                ["running"] = running,
                ["socks5Enabled"] = socks,
                ["httpEnabled"] = http,
                ["portConflict"] = portConflict,
                ["sessionCount"] = Operate.ProxyConfig.Proxy.SessionCount,
                ["sampledAtUtc"] = DateTime.UtcNow.ToString("o")
            };
        }

        private static JObject GetExecutorsDetail()
        {
            var send = Operate.SendConfig.List.SendExecute_Count();
            var robot = Operate.RobotConfig.List.RobotExecute_Count();
            return new JObject
            {
                ["sendRunning"] = send,
                ["sendStopped"] = 0,
                ["robotRunning"] = robot,
                ["robotStopped"] = 0,
                ["sampledAtUtc"] = DateTime.UtcNow.ToString("o")
            };
        }

        private static JObject GetStorageHealth()
        {
            var path = Operate.DataBase.dbPath ?? string.Empty;
            var name = Operate.DataBase.dbName ?? string.Empty;
            return new JObject
            {
                ["directoryAvailable"] = Directory.Exists(path),
                ["databaseFileAvailable"] = File.Exists(Path.Combine(path, name)),
                ["databaseName"] = name,
                ["version"] = Operate.SystemConfig.AssemblyVersion,
                ["sampledAtUtc"] = DateTime.UtcNow.ToString("o")
            };
        }

        private static async Task<JToken> SetProxyBindIpAsync(JObject arguments)
        {
            var autoToken = arguments?["auto"];
            var ip = ((string)arguments?["ip"] ?? string.Empty).Trim();
            var idempotencyKey = (string)arguments?["idempotencyKey"];
            if (autoToken == null || autoToken.Type != JTokenType.Boolean) throw new InvalidOperationException("auto must be a boolean.");
            var auto = autoToken.Value<bool>();
            if (!auto && !System.Net.IPAddress.TryParse(ip, out System.Net.IPAddress _)) throw new InvalidOperationException("ip must be a valid IPv4 or IPv6 address when auto is false.");
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("proxy.bindIp.set", idempotencyKey, arguments, out prior)) return prior;
            var current = ReadOnUi(() => new { auto = Operate.ProxyConfig.Proxy.ProxyIP_Auto, ip = Operate.ProxyConfig.Proxy.ProxyIP ?? string.Empty });
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync(
                "proxy.bindIp.set",
                idempotencyKey,
                arguments,
                auto ? "将代理监听地址切换为自动检测。" : "将代理监听地址切换为“" + ip + "”。",
                () =>
                {
                    if (!auto && !System.Net.IPAddress.TryParse(ip, out System.Net.IPAddress _)) throw new InvalidOperationException("ip must be a valid IPv4 or IPv6 address when auto is false.");
                    var changed = Operate.ProxyConfig.Proxy.ProxyIP_Auto != auto || !string.Equals(Operate.ProxyConfig.Proxy.ProxyIP ?? string.Empty, ip, StringComparison.Ordinal);
                    if (changed)
                    {
                        Operate.ProxyConfig.Proxy.ProxyIP_Auto = auto;
                        Operate.ProxyConfig.Proxy.ProxyIP = ip;
                        Operate.SystemConfig.SaveProxyMode_ToDB();
                    }
                    return new JObject { ["changed"] = changed, ["auto"] = auto, ["ip"] = auto ? (JToken)JValue.CreateNull() : ip };
                })).ConfigureAwait(false);
        }

        private static async Task<JToken> SetExternalProxyEnabledAsync(JObject arguments)
        {
            var enabledToken = arguments?["enabled"];
            var idempotencyKey = (string)arguments?["idempotencyKey"];
            if (enabledToken == null || enabledToken.Type != JTokenType.Boolean) throw new InvalidOperationException("A boolean enabled value is required.");
            var enabled = enabledToken.Value<bool>();
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("proxy.external.setEnabled", idempotencyKey, arguments, out prior)) return prior;
            var config = ReadOnUi(() => new { enabled = Operate.ProxyConfig.Proxy.Enable_ExternalProxy, ip = (Operate.ProxyConfig.Proxy.ExternalProxy_IP ?? string.Empty).Trim(), port = (int)Operate.ProxyConfig.Proxy.ExternalProxy_Port, auth = Operate.ProxyConfig.Proxy.Enable_ExternalProxy_Auth });
            if (enabled)
            {
                if (string.IsNullOrWhiteSpace(config.ip)) throw new InvalidOperationException("An external proxy host is required before enabling it.");
                if (config.port < 1 || config.port > 65535) throw new InvalidOperationException("The external proxy port must be between 1 and 65535.");
            }
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync(
                "proxy.external.setEnabled", idempotencyKey, arguments,
                (enabled ? "启用" : "停用") + "外部代理（" + config.ip + ":" + config.port + "；认证" + (config.auth ? "已启用" : "未启用") + "）。",
                () =>
                {
                    if (Operate.ProxyConfig.Proxy.Enable_ExternalProxy == enabled) return new JObject { ["changed"] = false, ["enabled"] = enabled, ["host"] = config.ip, ["port"] = config.port };
                    var host = (Operate.ProxyConfig.Proxy.ExternalProxy_IP ?? string.Empty).Trim();
                    var port = (int)Operate.ProxyConfig.Proxy.ExternalProxy_Port;
                    if (enabled && (string.IsNullOrWhiteSpace(host) || port < 1 || port > 65535)) throw new InvalidOperationException("The configured external proxy endpoint is invalid.");
                    Operate.ProxyConfig.Proxy.Enable_ExternalProxy = enabled;
                    Operate.SystemConfig.SaveProxyMode_ToDB();
                    return new JObject { ["changed"] = true, ["enabled"] = enabled, ["host"] = host, ["port"] = port };
                })).ConfigureAwait(false);
        }

        private static async Task<JToken> StartProxyAsync(JObject arguments)
        {
            var idempotencyKey = (string)arguments?["idempotencyKey"];
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("proxy.start", idempotencyKey, arguments, out prior)) return prior;
            var running = ReadOnUi(() => Operate.ProxyConfig.Proxy.IsRunning);
            if (running) return await McpWriteGuard.ApproveAndApplyAsync("proxy.start", idempotencyKey, arguments, "代理服务已经在运行。", () => new JObject { ["changed"] = false, ["running"] = true, ["outcome"] = "approved" }).ConfigureAwait(false);
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("proxy.start", idempotencyKey, arguments, "启动 WPE 代理监听服务并占用配置的端口。", () =>
            {
                var ok = Operate.ProxyConfig.Proxy.StartProxy();
                return new JObject { ["changed"] = ok, ["running"] = Operate.ProxyConfig.Proxy.IsRunning, ["ok"] = ok };
            })).ConfigureAwait(false);
        }

        private static async Task<JToken> StopProxyAsync(JObject arguments)
        {
            var idempotencyKey = (string)arguments?["idempotencyKey"];
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("proxy.stop", idempotencyKey, arguments, out prior)) return prior;
            var running = ReadOnUi(() => Operate.ProxyConfig.Proxy.IsRunning);
            if (!running) return await McpWriteGuard.ApproveAndApplyAsync("proxy.stop", idempotencyKey, arguments, "代理服务已经停止。", () => new JObject { ["changed"] = false, ["running"] = false, ["outcome"] = "approved" }).ConfigureAwait(false);
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("proxy.stop", idempotencyKey, arguments, "停止 WPE 代理监听服务；现有代理连接可能会断开。", () =>
            {
                Operate.ProxyConfig.Proxy.StopProxy();
                return new JObject { ["changed"] = true, ["running"] = Operate.ProxyConfig.Proxy.IsRunning };
            })).ConfigureAwait(false);
        }

        private static JObject ListFirewallRules(JObject arguments)
        {
            var list = ReadFirewallListKind((string)arguments?["list"]);
            var limit = Math.Max(1, Math.Min(200, (int?)arguments?["limit"] ?? 50));
            var offset = ReadOffset(arguments);
            var rows = new JArray();
            if (list == "black")
            {
                var source = Operate.ProxyConfig.Proxy.lstBlackList;
                for (var i = offset; i < source.Count && rows.Count < limit; i++)
                {
                    var rule = source[i];
                    rows.Add(new JObject { ["address"] = rule.IPAddress, ["location"] = rule.IPLocation, ["expiryEnabled"] = rule.IsExpiry, ["expiryTime"] = rule.IsExpiry ? rule.ExpiryTime.ToUniversalTime().ToString("o") : (JToken)JValue.CreateNull(), ["createdTime"] = rule.CreateTime.ToUniversalTime().ToString("o"), ["effectCount"] = rule.EffectCount });
                }
                return new JObject { ["list"] = list, ["rows"] = rows, ["nextCursor"] = NextCursor(offset, rows.Count, offset + rows.Count < source.Count) };
            }
            else
            {
                var source = Operate.ProxyConfig.Proxy.lstWhiteList;
                for (var i = offset; i < source.Count && rows.Count < limit; i++)
                {
                    var rule = source[i];
                    rows.Add(new JObject { ["address"] = rule.IPAddress, ["location"] = rule.IPLocation, ["expiryEnabled"] = rule.IsExpiry, ["expiryTime"] = rule.IsExpiry ? rule.ExpiryTime.ToUniversalTime().ToString("o") : (JToken)JValue.CreateNull(), ["createdTime"] = rule.CreateTime.ToUniversalTime().ToString("o"), ["effectCount"] = rule.EffectCount });
                }
                return new JObject { ["list"] = list, ["rows"] = rows, ["nextCursor"] = NextCursor(offset, rows.Count, offset + rows.Count < source.Count) };
            }
        }

        private static string ReadFirewallListKind(string value)
        {
            if (string.Equals(value, "white", StringComparison.OrdinalIgnoreCase)) return "white";
            if (string.Equals(value, "black", StringComparison.OrdinalIgnoreCase)) return "black";
            throw new InvalidOperationException("Firewall list must be white or black.");
        }

        private static bool FirewallRuleExists(string list, string address)
        {
            if (list == "black") return Operate.ProxyConfig.Proxy.IsExistsInBlackList(address);
            return Operate.ProxyConfig.Proxy.IsExistsInWhiteList(address);
        }

        private static JObject BytesTranscode(JObject arguments)
        {
            var text = (string)arguments?["text"] ?? string.Empty;
            if (text.Length > 1024 * 1024) throw new InvalidOperationException("Text exceeds 1 MiB.");
            var decode = (bool?)arguments?["decode"] ?? false;
            return new JObject { ["text"] = decode ? Encoding.UTF8.GetString(Convert.FromBase64String(text)) : Convert.ToBase64String(Encoding.UTF8.GetBytes(text)), ["format"] = decode ? "utf8" : "base64" };
        }

        private static JObject BytesCompare(JObject arguments)
        {
            var left = (string)arguments?["left"] ?? string.Empty;
            var right = (string)arguments?["right"] ?? string.Empty;
            if (left.Length > 1024 * 1024 || right.Length > 1024 * 1024) throw new InvalidOperationException("Input exceeds 1 MiB.");
            var a = Encoding.UTF8.GetBytes(left); var b = Encoding.UTF8.GetBytes(right);
            var same = 0; while (same < a.Length && same < b.Length && a[same] == b[same]) same++;
            return new JObject { ["equal"] = a.Length == b.Length && same == a.Length, ["commonPrefixBytes"] = same, ["leftBytes"] = a.Length, ["rightBytes"] = b.Length };
        }

        private static JObject BytesExtract(JObject arguments)
        {
            var kind = (int?)arguments?["kind"] ?? -1;
            var content = (string)arguments?["contentBase64"] ?? string.Empty;
            var bytes = Convert.FromBase64String(content);
            if (bytes.Length > 4 * 1024 * 1024) throw new InvalidOperationException("Content exceeds 4 MiB.");
            if (kind < 0 || kind > 32) throw new InvalidOperationException("Unsupported extraction kind.");
            return new JObject { ["kind"] = kind, ["value"] = Operate.SystemConfig.BytesToString((Operate.PacketConfig.Packet.EncodingFormat)kind, bytes) };
        }

        private static T ReadOnUi<T>(Func<T> action)
        {
            if (Operate.SystemConfig.InvokeAction == null) throw new InvalidOperationException("WPE UI is not ready.");
            T result = default(T);
            Exception failure = null;
            using (var done = new ManualResetEventSlim(false))
            {
                Operate.SystemConfig.InvokeAction(() => { try { result = action(); } catch (Exception ex) { failure = ex; } finally { done.Set(); } });
                if (!done.Wait(TimeSpan.FromSeconds(3))) throw new TimeoutException("WPE UI did not answer in time.");
            }
            if (failure != null) throw failure;
            return result;
        }

        private static Task<T> InvokeOnUiAsync<T>(Func<Task<T>> action)
        {
            if (Operate.SystemConfig.InvokeAction == null) throw new InvalidOperationException("WPE UI is not ready.");
            var completion = new TaskCompletionSource<T>();
            Operate.SystemConfig.InvokeAction(async () =>
            {
                try { completion.TrySetResult(await action()); }
                catch (Exception ex) { completion.TrySetException(ex); }
            });
            return completion.Task;
        }

        private static async Task WriteFrameAsync(Stream stream, byte[] payload, CancellationToken token)
        {
            if (payload.Length > MaxFrameBytes) throw new InvalidOperationException("Response too large.");
            var header = BitConverter.GetBytes(payload.Length);
            await stream.WriteAsync(header, 0, header.Length, token).ConfigureAwait(false);
            await stream.WriteAsync(payload, 0, payload.Length, token).ConfigureAwait(false);
            await stream.FlushAsync(token).ConfigureAwait(false);
        }

        private static async Task<byte[]> ReadFrameAsync(Stream stream, CancellationToken token)
        {
            var header = await ReadExactlyAsync(stream, sizeof(int), token).ConfigureAwait(false);
            var length = BitConverter.ToInt32(header, 0);
            if (length <= 0 || length > MaxFrameBytes) throw new InvalidOperationException("Invalid MCP gateway frame.");
            return await ReadExactlyAsync(stream, length, token).ConfigureAwait(false);
        }

        private static async Task<byte[]> ReadExactlyAsync(Stream stream, int length, CancellationToken token)
        {
            var buffer = new byte[length];
            var offset = 0;
            while (offset < length)
            {
                var read = await stream.ReadAsync(buffer, offset, length - offset, token).ConfigureAwait(false);
                if (read == 0) throw new EndOfStreamException();
                offset += read;
            }
            return buffer;
        }

        private void PublishInstance(bool add)
        {
            var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WPE64", "mcp");
            Directory.CreateDirectory(directory);
            var path = Path.Combine(directory, "instances.json");
            using (var mutex = new Mutex(false, "Local\\WPE64.Mcp.Discovery.v1"))
            {
                mutex.WaitOne();
                try
                {
                    var root = File.Exists(path) ? JObject.Parse(File.ReadAllText(path)) : new JObject();
                    var entries = root["instances"] as JArray ?? new JArray();
                    for (var i = entries.Count - 1; i >= 0; i--)
                    {
                        if ((int?)entries[i]["processId"] == processId) entries.RemoveAt(i);
                    }
                    if (add)
                    {
                        entries.Add(new JObject
                        {
                            ["pipeName"] = pipeName,
                            ["processId"] = processId,
                            ["startedUtc"] = DateTime.UtcNow.ToString("o")
                        });
                    }
                    root["protocol"] = 1;
                    root["instances"] = entries;
                    File.WriteAllText(path, root.ToString(Formatting.None), Encoding.UTF8);
                }
                finally { mutex.ReleaseMutex(); }
            }
        }
    }
}
