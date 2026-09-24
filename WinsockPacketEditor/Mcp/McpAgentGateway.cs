using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Pipes;
using System.Globalization;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WinsockPacketEditor.Mcp
{
    /// <summary>
    /// Local-only boundary for the external MCP process. This is intentionally separate from
    /// the injected-process IPC protocol: reads are bounded snapshots and writes remain
    /// behind the shared WPE confirmation and idempotency guard.
    /// </summary>
    internal sealed class McpAgentGateway : IDisposable
    {
        // The local, current-user pipe can carry complete packet data. This remains a
        // protocol allocation guard, not a data-redaction policy.
        private const int MaxFrameBytes = 128 * 1024 * 1024;
        // Keep enough acceptors available for parallel MCP tool calls.  A request may
        // wait up to one minute for local write approval, so one blocked caller must
        // never make reads or other callers lose the pipe listener.
        private const int PipeInstanceCount = 8;
        private static readonly TimeSpan RequestDeadline = TimeSpan.FromSeconds(75);
        // Connection ids are only stable for this WPE process. The random salt keeps
        // account/IP/device tuples from becoming externally meaningful identifiers.
        private static readonly string connectionIdSalt = Guid.NewGuid().ToString("N");
        private readonly CancellationTokenSource cancellation = new CancellationTokenSource();
        private readonly int processId = Process.GetCurrentProcess().Id;
        // The name is an unguessable capability; it is disclosed only through the
        // current user's LocalAppData discovery record.
        private readonly string pipeName = "WPE64-Mcp-" + Guid.NewGuid().ToString("N");
        private Task acceptLoop;
        internal static Action<string> StartModeRequested;
        // Operations that must preserve ShellForm's existing target-process routing
        // or native-dialog behaviour are delegated back to the owning shell.
        internal static Func<string, JObject, Task<JObject>> ShellActionRequested;

        public bool Enabled { get; private set; }

        public void Start()
        {
            Enabled = true;
            PublishInstance(true);
            acceptLoop = Task.WhenAll(Enumerable.Range(0, PipeInstanceCount)
                .Select(_ => Task.Run(() => AcceptLoopAsync(cancellation.Token))).ToArray());
        }

        public void Stop()
        {
            Enabled = false;
            PublishInstance(false);
        }

        public void Dispose()
        {
            StartModeRequested = null;
            ShellActionRequested = null;
            cancellation.Cancel();
            Stop();
            try { if (acceptLoop != null) acceptLoop.Wait(1000); } catch { }
            cancellation.Dispose();
        }

        private async Task AcceptLoopAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (!Enabled) { await Task.Delay(250, token).ConfigureAwait(false); continue; }
                    using (var pipe = CreateCurrentUserPipe())
                    {
                        await pipe.WaitForConnectionAsync(token).ConfigureAwait(false);
                        if (!Enabled) continue;
                        await ServeAsync(pipe, token).ConfigureAwait(false);
                    }
                }
                catch (OperationCanceledException) { return; }
                // A stdio MCP client may terminate while a pipe connection is being
                // established or while its process is being torn down.  That is a
                // normal peer disconnect, not a WPE system error.
                catch (EndOfStreamException) { }
                catch (Exception) { /* MCP 工具错误会在 ServeAsync 中写入专用 MCP 日志。 */ }
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
            return new NamedPipeServerStream(pipeName, PipeDirection.InOut, PipeInstanceCount, PipeTransmissionMode.Byte, PipeOptions.Asynchronous, 0, 0, security);
        }

        private static async Task ServeAsync(Stream stream, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var bytes = await ReadFrameAsync(stream, token).ConfigureAwait(false);
                var request = JObject.Parse(Encoding.UTF8.GetString(bytes));
                var requestId = (string)request["requestId"];
                var operation = (string)request["operation"];
                JObject response;
                try
                {
                    using (var requestCancellation = CancellationTokenSource.CreateLinkedTokenSource(token))
                    {
                        requestCancellation.CancelAfter(ReadRequestDeadline(request));
                        var dispatch = Task.Run(async () =>
                        {
                            var arguments = request["arguments"] as JObject ?? new JObject();
                            return await DispatchAsync(operation, arguments).ConfigureAwait(false);
                        });
                        var completed = await Task.WhenAny(dispatch, Task.Delay(Timeout.Infinite, requestCancellation.Token)).ConfigureAwait(false);
                        if (completed != dispatch) throw new TimeoutException("WPE gateway request deadline expired.");
                        var result = await dispatch.ConfigureAwait(false);
                        response = new JObject { ["requestId"] = requestId, ["ok"] = true, ["result"] = result };
                        var outcome = (string)(result as JObject)?["outcome"];
                        var changed = (bool?)(result as JObject)?["changed"];
                        var message = string.IsNullOrEmpty(outcome) ? "调用结果：成功。"
                            : "调用结果：" + outcome + (changed.HasValue ? (changed.Value ? "，已变更。" : "，无需变更。") : "。");
                // 写入守卫已经把审计原始记录保存在内存中；界面日志只展示人能读懂的
                // 审计结论，并与该次调用结果合并成一行，避免一项操作占两行。
                        if (outcome == "approved") message += Operate.SystemConfig.McpRequiresConfirmation ? " 审计结果：已本地确认并完成。" : " 审计结果：已自动确认并完成。";
                        else if (outcome == "rejected") message += " 审计结果：本地用户已拒绝。";
                        else if (outcome == "expired") message += " 审计结果：等待本地确认超时。";
                        McpLog.Write(McpLog.ToolName(operation), message);
                    }
                }
                catch (Exception ex)
                {
                    McpLog.Error(McpLog.ToolName(operation), ex);
                    response = new JObject { ["requestId"] = requestId, ["ok"] = false, ["error"] = ex is TimeoutException ? "WPE gateway request deadline expired." : "WPE gateway request failed." };
                }
                await WriteFrameAsync(stream, Encoding.UTF8.GetBytes(response.ToString(Formatting.None)), token).ConfigureAwait(false);
            }
        }

        private static TimeSpan ReadRequestDeadline(JObject request)
        {
            DateTime deadline;
            var value = (string)request?["deadlineUtc"];
            if (!DateTime.TryParse(value, CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal | DateTimeStyles.AssumeUniversal, out deadline)) return RequestDeadline;
            var remaining = deadline.ToUniversalTime() - DateTime.UtcNow;
            if (remaining <= TimeSpan.Zero) throw new TimeoutException("WPE gateway request deadline expired.");
            return remaining < RequestDeadline ? remaining : RequestDeadline;
        }

        private static JToken Dispatch(string operation, JObject arguments)
        {
            if (operation == "runtime.status")
            {
                return ReadOnUi(() => new JObject
                {
                    ["version"] = Operate.SystemConfig.AssemblyVersion,
                    // 模式和监听器生命周期是两件事：进入代理页面后可以尚未启动 SOCKS5 监听。
                    ["mode"] = Operate.SystemConfig.SelectMode == Operate.SystemConfig.SystemMode.Proxy ? "proxy"
                        : Operate.SystemConfig.SelectMode == Operate.SystemConfig.SystemMode.Inject ? "inject" : "start",
                    ["proxyRunning"] = Operate.ProxyConfig.Proxy.IsRunning,
                    ["captured"] = Operate.PacketConfig.Packet.TotalPackets,
                    ["proxyConnections"] = Operate.ProxyConfig.Proxy.SessionCount
                });
            }
            if (operation == "capture.findNext") return ReadOnUi(() => FindNextCapture(arguments));
            if (operation == "logs.list") return ReadOnUi(() => ListLogs(arguments));
            if (operation == "logs.all.list") return ReadOnUi(() => ListAllLogs(arguments));
            if (operation == "filters.list") return ReadOnUi(() => ListFilters(arguments));
            if (operation == "filters.get") return ReadOnUi(() => GetFilter(arguments));
            if (operation == "filters.stats.get") return ReadOnUi(() => GetFilterStats(arguments));
            if (operation == "accounts.list") return ReadOnUi(() => ListAccounts(arguments));
            if (operation == "accounts.get") return ReadOnUi(() => GetAccount(arguments));
            if (operation == "accounts.logins.list") return ReadOnUi(() => ListAccountLogins(arguments));
            if (operation == "connections.list") return ReadOnUi(() => ListConnections(arguments));
            if (operation == "executors.list") return ReadOnUi(ListExecutors);
            if (operation == "firewall.get") return ReadOnUi(GetFirewall);
            if (operation == "firewall.rules.list") return ReadOnUi(() => ListFirewallRules(arguments));
            if (operation == "proxy.settings.get") return ReadOnUi(GetProxySettings);
            if (operation == "proxy.config.get") return ReadOnUi(GetProxyConfig);
            if (operation == "proxy.runtime.get") return ReadOnUi(GetProxyRuntime);
            if (operation == "remoteManagement.get") return ReadOnUi(GetRemoteManagement);
            if (operation == "settings.get") return ReadOnUi(() => GetSetting(arguments));
            if (operation == "wpc.servers.list") return ReadOnUi(ListWpcServers);
            if (operation == "wpc.server.rules.list") return ReadOnUi(() => ListWpcServerRules(arguments));
            if (operation == "connections.summary.get") return ReadOnUi(GetConnectionsSummary);
            if (operation == "bytes.transcode") return BytesTranscode(arguments);
            if (operation == "bytes.compare") return BytesCompare(arguments);
            if (operation == "bytes.extract") return BytesExtract(arguments);
            if (operation == "sends.list") return ReadOnUi(() => ListSends(arguments));
            if (operation == "sends.get") return ReadOnUi(() => GetSend(arguments));
            if (operation == "sends.collection.list") return ReadOnUi(() => ListSendCollection(arguments));
            if (operation == "robots.list") return ReadOnUi(() => ListRobots(arguments));
            if (operation == "robots.get") return ReadOnUi(() => GetRobot(arguments));
            if (operation == "warehouses.list") return ReadOnUi(() => ListWarehouses(arguments));
            if (operation == "warehouses.get") return ReadOnUi(() => GetWarehouse(arguments));
            if (operation == "autoStores.list") return ReadOnUi(ListAutoStores);
            if (operation == "packet.edit.get") return ReadOnUi(() => GetPacketEdit(arguments));
            throw new InvalidOperationException("The requested MCP operation is not available.");
        }

        private static Task<JToken> DispatchAsync(string operation, JObject arguments)
        {
            if (operation == "capture.search") return SearchPacketsAsync(arguments);
            if (operation == "capture.get") return GetPacketAsync(arguments);
            if (operation == "filters.setEnabled") return SetFilterEnabledAsync(arguments);
            if (operation == "filters.setAllEnabled") return SetAllFiltersEnabledAsync(arguments);
            if (operation == "filters.counts.reset") return ResetFilterCountsAsync(arguments);
            if (operation == "filters.move") return MoveFiltersAsync(arguments);
            if (operation == "filters.copy") return CopyFiltersAsync(arguments);
            if (operation == "filters.clearAll") return ClearAllFiltersAsync(arguments);
            if (operation == "filters.createFromCapture") return CreateFilterFromCaptureAsync(arguments);
            if (operation == "filters.create") return CreateFilterAsync(arguments);
            if (operation == "filters.rule.save") return SaveFilterRuleAsync(arguments);
            if (operation == "filters.delete") return DeleteFilterAsync(arguments);
            if (operation == "accounts.setEnabled") return SetAccountEnabledAsync(arguments);
            if (operation == "accounts.create") return CreateAccountAsync(arguments);
            if (operation == "accounts.update") return UpdateAccountAsync(arguments);
            if (operation == "accounts.delete") return DeleteAccountAsync(arguments);
            if (operation == "proxy.auth.setEnabled") return SetProxyAuthEnabledAsync(arguments);
            if (operation == "proxy.maxConnections.set") return SetProxyMaxConnectionsAsync(arguments);
            if (operation == "proxy.socks5Port.set") return SetProxySocks5PortAsync(arguments);
            if (operation == "firewall.setEnabled") return SetFirewallEnabledAsync(arguments);
            if (operation == "proxy.onlyWpc.setEnabled") return SetOnlyWpcEnabledAsync(arguments);
            if (operation == "proxy.bindIp.set") return SetProxyBindIpAsync(arguments);
            if (operation == "proxy.external.setEnabled") return SetExternalProxyEnabledAsync(arguments);
            if (operation == "proxy.start") return StartProxyAsync(arguments);
            if (operation == "proxy.stop") return StopProxyAsync(arguments);
            if (operation == "remoteManagement.save") return SaveRemoteManagementAsync(arguments);
            if (operation == "settings.save") return SaveSettingAsync(arguments);
            if (operation == "map.local.save") return SaveMapLocalAsync(arguments);
            if (operation == "map.remote.save") return SaveMapRemoteAsync(arguments);
            if (operation == "backup.export") return ShellActionAsync("backup.export", arguments, "导出选定的 WPE 备份内容；WPE 会显示本地保存对话框。", "backup.export");
            if (operation == "backup.import") return ShellActionAsync("backup.import", arguments, "导入 WPE 备份；WPE 会显示本地选择文件对话框并替换所含配置。", "backup.import");
            if (operation == "wpc.server.save") return SaveWpcServerAsync(arguments);
            if (operation == "wpc.server.rule.save") return SaveWpcServerRuleAsync(arguments);
            if (operation == "executors.stopAll") return StopAllExecutorsAsync(arguments);
            if (operation == "start.mode.select") return SelectStartModeAsync(arguments);
            if (operation == "firewall.rule.add") return AddFirewallRuleAsync(arguments);
            if (operation == "firewall.rule.remove") return RemoveFirewallRuleAsync(arguments);
            if (operation == "sends.setEnabled") return SetTaskEnabledAsync("send", arguments);
            if (operation == "robots.setEnabled") return SetTaskEnabledAsync("robot", arguments);
            if (operation == "tasks.create") return CreateTaskAsync(arguments);
            if (operation == "tasks.update") return UpdateTaskAsync(arguments);
            if (operation == "tasks.move" || operation == "tasks.copy" || operation == "tasks.delete") return TaskListActionAsync(operation, arguments);
            if (operation == "tasks.clear") return ClearTasksAsync(arguments);
            if (operation == "capture.addToSend") return AddCaptureAsync("capture.addToSend", true, false, arguments);
            if (operation == "capture.addToWarehouse") return AddCaptureAsync("capture.addToWarehouse", false, false, arguments);
            if (operation == "proxyCapture.addToSend") return AddCaptureAsync("proxyCapture.addToSend", true, true, arguments);
            if (operation == "proxyCapture.addToWarehouse") return AddCaptureAsync("proxyCapture.addToWarehouse", false, true, arguments);
            if (operation == "sends.collection.action") return SendCollectionActionAsync(arguments);
            if (operation == "sends.collection.clear") return ClearSendCollectionAsync(arguments);
            if (operation == "robots.instructions.add") return AddRobotInstructionAsync(arguments);
            if (operation == "robots.instructions.save") return SaveRobotInstructionsAsync(arguments);
            if (operation == "robots.instructions.action") return RobotInstructionActionAsync(arguments);
            if (operation == "autoStores.save") return SaveAutoStoresAsync(arguments);
            if (operation == "autoStores.setEnabled") return SetAutoStoresEnabledAsync(arguments);
            if (operation == "autoStores.delete") return DeleteAutoStoresAsync(arguments);
            if (operation == "warehouses.stores.action") return WarehouseStoresActionAsync(arguments);
            if (operation == "warehouses.stores.command") return WarehouseStoresCommandAsync(arguments);
            if (operation == "packet.edit.save") return SavePacketEditAsync(arguments);
            if (operation == "packet.edit.addToSend") return AddPacketEditToSendAsync(arguments);
            if (operation == "capture.clear") return ShellActionAsync("capture.clear", arguments, "清空当前捕获封包列表。", "capture.clear");
            if (operation == "capture.export") return ShellActionAsync("capture.export", arguments, "导出捕获封包；WPE 会显示本地保存文件对话框。", "capture.export");
            if (operation == "filters.import") return ShellActionAsync("filters.import", arguments, "导入滤镜列表；WPE 会显示本地选择文件和必要的密码对话框。", "filters.import");
            if (operation == "import.run") return ShellActionAsync("import.run", arguments, "通过 WPE 原生选择文件对话框导入数据。", "import.run");
            if (operation == "filters.export") return ShellActionAsync("filters.export", arguments, "导出滤镜列表；WPE 会显示本地保存文件和必要的密码对话框。", "filters.export");
            if (operation == "export.run") return ShellActionAsync("export.run", arguments, "通过 WPE 原生保存对话框导出数据。", "export.run");
            if (operation == "sends.start") return ShellActionAsync("sends.start", arguments, "启动已启用的发送任务列表，可能主动发送封包。", "sends.start");
            if (operation == "sends.stop") return ShellActionAsync("sends.stop", arguments, "停止发送任务列表。", "sends.stop");
            if (operation == "send.start") return ShellActionAsync("send.start", arguments, "启动一个已启用的发送任务，可能主动发送封包。", "send.start");
            if (operation == "robots.start") return ShellActionAsync("robots.start", arguments, "启动已启用的机器人任务列表。", "robots.start");
            if (operation == "robots.stop") return ShellActionAsync("robots.stop", arguments, "停止机器人任务列表。", "robots.stop");
            if (operation == "robot.start") return ShellActionAsync("robot.start", arguments, "启动一个已启用的机器人任务。", "robot.start");
            if (operation == "inject.attach") return ShellActionAsync("inject.attach", arguments, "按 WPE 当前注入方式附加目标进程或启动并注入目标程序。", "inject.attach");
            if (operation == "inject.quickAttach") return ShellActionAsync("inject.quickAttach", arguments, "按 WPE 已保存的上次注入记录重新附加目标。", "inject.quickAttach");
            if (operation == "inject.detach") return ShellActionAsync("inject.detach", arguments, "卸载当前注入钩子并断开目标进程连接。", "inject.detach");
            if (operation == "inject.startHook") return ShellActionAsync("inject.startHook", arguments, "在当前已附加目标中启动 WPE 钩子。", "inject.startHook");
            if (operation == "inject.stopHook") return ShellActionAsync("inject.stopHook", arguments, "停止当前已附加目标中的 WPE 钩子。", "inject.stopHook");
            if (operation == "driver.uninstall") return ShellActionAsync("driver.uninstall", arguments, "卸载 WPE 进程代理驱动；该原生操作会要求重启电脑。", "driver.uninstall");
            if (operation == "processProxy.save") return ShellActionAsync("processProxy.save", arguments, "保存 WPE 进程代理设置；原生逻辑会按需安装或配置驱动。", "processProxy.save");
            if (operation == "packetEdit.sendStart") return ShellActionAsync("packetEdit.sendStart", arguments, "按当前编辑快照发送封包。", "packetEdit.sendStart");
            if (operation == "packetEdit.sendStop") return ShellActionAsync("packetEdit.sendStop", arguments, "停止当前封包编辑器发送会话。", "packetEdit.sendStop");
            return Task.FromResult(Dispatch(operation, arguments));
        }

        private static async Task<JToken> ShellActionAsync(string operation, JObject arguments, string summary, string action)
        {
            var key = (string)arguments?["idempotencyKey"];
            JObject prior;
            if (McpWriteGuard.TryGetCompleted(operation, key, arguments, out prior)) return prior;
            if (ShellActionRequested == null) throw new InvalidOperationException("WPE shell action is not available.");
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync(
                operation, key, arguments, summary,
                () => ShellActionRequested(action, arguments ?? new JObject()))).ConfigureAwait(false);
        }

        private static int PageLimit(JObject arguments) { return Math.Max(1, Math.Min(200, (int?)arguments?["limit"] ?? 50)); }
        private static string RequireGuidId(JObject arguments)
        {
            var id = (string)arguments?["id"];
            if (!Guid.TryParse(id, out _)) throw new InvalidOperationException("A task GUID id is required.");
            return id;
        }
        private static JObject Page(JArray rows, int total, int offset)
        {
            return new JObject { ["rows"] = rows, ["nextCursor"] = NextCursor(offset, rows.Count, offset + rows.Count < total) };
        }
        private static JObject ListSends(JObject a)
        {
            var offset = ReadOffset(a); var limit = PageLimit(a); var rows = new JArray(); var list = Operate.SendConfig.List.lstSendInfo;
            for (var i = offset; i < list.Count && rows.Count < limit; i++) rows.Add(JObject.FromObject(SendRow.From_(list[i])));
            return Page(rows, list.Count, offset);
        }
        private static JObject GetSend(JObject a)
        {
            var row = Operate.SendConfig.Send.OpenSendEdit_ById(RequireGuidId(a));
            try { if (string.IsNullOrEmpty(row.Id)) throw new InvalidOperationException("The send task does not exist."); return JObject.FromObject(row); }
            finally { Operate.SendConfig.Send.CloseSendEdit(); }
        }
        private static JObject ListSendCollection(JObject a)
        {
            var id = RequireGuidId(a); var offset = ReadOffset(a); var limit = PageLimit(a); var edit = Operate.SendConfig.Send.OpenSendEdit_ById(id);
            try
            {
                if (string.IsNullOrEmpty(edit.Id)) throw new InvalidOperationException("The send task does not exist.");
                var source = Operate.SendConfig.Send.GetSendCollectionRows(); var rows = new JArray();
                for (var i = offset; i < source.Length && rows.Count < limit; i++) rows.Add(JObject.FromObject(source[i]));
                return Page(rows, source.Length, offset);
            }
            finally { Operate.SendConfig.Send.CloseSendEdit(); }
        }
        private static JObject ListRobots(JObject a)
        {
            var offset = ReadOffset(a); var limit = PageLimit(a); var rows = new JArray(); var list = Operate.RobotConfig.List.lstRobotInfo;
            for (var i = offset; i < list.Count && rows.Count < limit; i++) rows.Add(JObject.FromObject(RobotRow.From_(list[i])));
            return Page(rows, list.Count, offset);
        }
        private static JObject GetRobot(JObject a)
        {
            var edit = Operate.RobotConfig.Robot.OpenRobotEdit_ById(RequireGuidId(a));
            try
            {
                if (string.IsNullOrEmpty(edit.Id)) throw new InvalidOperationException("The robot task does not exist.");
                return new JObject { ["id"] = edit.Id, ["name"] = edit.Name, ["instructions"] = JArray.FromObject(Operate.RobotConfig.Robot.GetRobotInstructionRows()) };
            }
            finally { Operate.RobotConfig.Robot.CloseRobotEdit(); }
        }
        private static JObject ListWarehouses(JObject a)
        {
            var offset = ReadOffset(a); var limit = PageLimit(a); var rows = new JArray(); var list = Operate.WareHouseConfig.List.lstWareHouseInfo;
            for (var i = offset; i < list.Count && rows.Count < limit; i++) rows.Add(JObject.FromObject(WareHouseRow.From_(list[i])));
            return Page(rows, list.Count, offset);
        }
        private static JObject GetWarehouse(JObject a)
        {
            var id = RequireGuidId(a); var warehouse = Operate.WareHouseConfig.List.OpenWareHouseEdit_ById(id);
            if (string.IsNullOrEmpty(warehouse.Id)) throw new InvalidOperationException("The warehouse does not exist.");
            var all = Operate.WareHouseConfig.List.GetStoreRows_ById(id); var offset = ReadOffset(a); var limit = PageLimit(a); var rows = new JArray();
            for (var i = offset; i < all.Length && rows.Count < limit; i++) rows.Add(JObject.FromObject(all[i]));
            return new JObject { ["id"] = warehouse.Id, ["name"] = warehouse.Name, ["dataCount"] = warehouse.DataCount, ["rows"] = rows, ["nextCursor"] = NextCursor(offset, rows.Count, offset + rows.Count < all.Length) };
        }
        private static JObject ListAutoStores()
        {
            var rows = new JArray();
            foreach (var rule in Operate.WareHouseConfig.List.lstAutoStoresInfo) rows.Add(JObject.FromObject(AutoStoresRow.From_(rule)));
            return new JObject { ["rows"] = rows };
        }
        private static JObject GetPacketEdit(JObject a)
        {
            var list = ReadEditablePacketList((string)a?["list"]); var id = (long?)a?["id"];
            if (!id.HasValue || id.Value < 1) throw new InvalidOperationException("A positive packet id is required.");
            var row = Operate.PacketEditConfig.Open(list, id.Value);
            if (string.IsNullOrEmpty(row.Id)) throw new InvalidOperationException("The packet no longer exists.");
            var result = JObject.FromObject(row);
            // Never emit the same bytes twice under two casing conventions. MCP uses
            // an explicit payload name, while the in-process DTO keeps its UI field.
            result.Remove("Buffer");
            result["payloadBase64"] = Convert.ToBase64String(row.Buffer ?? new byte[0]);
            return result;
        }

        private static string ReadTaskKind(JObject a)
        {
            var kind = ((string)a?["kind"] ?? string.Empty).Trim().ToLowerInvariant();
            if (kind != "send" && kind != "robot" && kind != "warehouse") throw new InvalidOperationException("kind must be send, robot, or warehouse.");
            return kind;
        }
        private static IList<string> ReadTaskIds(JObject a)
        {
            var result = new List<string>(); var values = a?["ids"] as JArray;
            if (values == null) throw new InvalidOperationException("At least one task id is required.");
            foreach (var value in values) { var id = (string)value; if (Guid.TryParse(id, out _)) result.Add(id); else throw new InvalidOperationException("Each task id must be a GUID."); }
            if (result.Count == 0) throw new InvalidOperationException("At least one task id is required."); return result;
        }
        private static async Task<JToken> SetTaskEnabledAsync(string kind, JObject a)
        {
            var id = RequireGuidId(a); var enabled = (bool?)a?["enabled"]; if (!enabled.HasValue) throw new InvalidOperationException("A boolean enabled value is required.");
            var key = (string)a?["idempotencyKey"]; var op = kind == "send" ? "sends.setEnabled" : "robots.setEnabled"; JObject prior;
            if (McpWriteGuard.TryGetCompleted(op, key, a, out prior)) return prior;
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync(op, key, a, (enabled.Value ? "启用" : "停用") + (kind == "send" ? "发送任务。" : "机器人任务。"), () =>
            {
                object current = kind == "send" ? (object)Operate.SendConfig.List.FindSend_ById(id) : Operate.RobotConfig.List.FindRobot_ById(id);
                if (current == null) throw new InvalidOperationException("The task no longer exists.");
                var already = kind == "send" ? ((SendInfo)current).IsEnable == enabled.Value : ((RobotInfo)current).IsEnable == enabled.Value;
                if (already) return new JObject { ["id"] = id, ["enabled"] = enabled.Value, ["changed"] = false };
                var changed = kind == "send" ? Operate.SendConfig.List.SetSendEnable_ById(id, enabled.Value) : Operate.RobotConfig.List.SetRobotEnable_ById(id, enabled.Value);
                if (!changed) throw new InvalidOperationException("The task no longer exists."); return new JObject { ["id"] = id, ["enabled"] = enabled.Value, ["changed"] = true };
            })).ConfigureAwait(false);
        }
        private static async Task<JToken> CreateTaskAsync(JObject a)
        {
            var kind = ReadTaskKind(a); var key = (string)a?["idempotencyKey"]; JObject prior; if (McpWriteGuard.TryGetCompleted("tasks.create", key, a, out prior)) return prior;
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("tasks.create", key, a, "新建一个空白" + kind + "任务；不会启动执行器或发送封包。", () =>
            {
                var id = kind == "send" ? Operate.SendConfig.List.AddSend_New_ById() : kind == "robot" ? Operate.RobotConfig.List.AddRobot_New_ById() : Operate.WareHouseConfig.List.AddWareHouse_New_ById();
                if (string.IsNullOrEmpty(id)) throw new InvalidOperationException("Unable to create the task."); return new JObject { ["kind"] = kind, ["id"] = id, ["changed"] = true };
            })).ConfigureAwait(false);
        }
        private static async Task<JToken> UpdateTaskAsync(JObject a)
        {
            var kind = ReadTaskKind(a); var id = RequireGuidId(a); var name = (string)a?["name"]; if (string.IsNullOrWhiteSpace(name)) throw new InvalidOperationException("A non-empty name is required.");
            var key = (string)a?["idempotencyKey"]; JObject prior; if (McpWriteGuard.TryGetCompleted("tasks.update", key, a, out prior)) return prior;
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("tasks.update", key, a, "更新" + kind + "任务“" + name.Trim() + "”；不会启动执行器或发送封包。", () =>
            {
                string error;
                if (kind == "send") { var current = Operate.SendConfig.Send.OpenSendEdit_ById(id); try { if (string.IsNullOrEmpty(current.Id)) throw new InvalidOperationException("The task no longer exists."); error = Operate.SendConfig.Send.SaveSendEdit(name, (bool?)a?["useSystemSocket"] ?? current.UseSystemSocket, (int?)a?["loopCount"] ?? current.LoopCount, (int?)a?["loopInterval"] ?? current.LoopInterval, (string)a?["notes"] ?? current.Notes); } finally { Operate.SendConfig.Send.CloseSendEdit(); } }
                else if (kind == "robot") { var current = Operate.RobotConfig.Robot.OpenRobotEdit_ById(id); try { if (string.IsNullOrEmpty(current.Id)) throw new InvalidOperationException("The task no longer exists."); error = Operate.RobotConfig.Robot.SaveRobotEdit(name); } finally { Operate.RobotConfig.Robot.CloseRobotEdit(); } }
                else error = Operate.WareHouseConfig.List.SaveWareHouseName_ById(id, name);
                if (!string.IsNullOrEmpty(error)) throw new InvalidOperationException(error); return new JObject { ["kind"] = kind, ["id"] = id, ["changed"] = true };
            })).ConfigureAwait(false);
        }
        private static int ReadTaskAction(string direction)
        {
            switch ((direction ?? string.Empty).Trim().ToLowerInvariant())
            {
                case "top": return 0; case "up": return 1; case "down": return 2; case "bottom": return 3; case "copy": return 4; case "delete": return 6;
                default: throw new InvalidOperationException("direction must be top, up, down, bottom, copy, or delete.");
            }
        }
        private static async Task<JToken> TaskListActionAsync(string operation, JObject a)
        {
            var kind = ReadTaskKind(a); var ids = ReadTaskIds(a); var action = ReadTaskAction((string)a?["direction"]); var key = (string)a?["idempotencyKey"]; JObject prior;
            if (McpWriteGuard.TryGetCompleted(operation, key, a, out prior)) return prior;
            var verb = action == 4 ? "复制" : action == 6 ? "删除" : "调整顺序";
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync(operation, key, a, verb + "选中的" + kind + "任务；不会启动执行器或发送封包。", async () =>
            {
                var before = kind == "send" ? Operate.SendConfig.List.lstSendInfo.Count : kind == "robot" ? Operate.RobotConfig.List.lstRobotInfo.Count : Operate.WareHouseConfig.List.lstWareHouseInfo.Count;
                var delta = kind == "send" ? await Operate.SendConfig.List.SendListAction_ByIds(action, ids) : kind == "robot" ? await Operate.RobotConfig.List.RobotListAction_ByIds(action, ids) : await Operate.WareHouseConfig.List.WareHouseListAction_ByIds(action, ids);
                var after = kind == "send" ? Operate.SendConfig.List.lstSendInfo.Count : kind == "robot" ? Operate.RobotConfig.List.lstRobotInfo.Count : Operate.WareHouseConfig.List.lstWareHouseInfo.Count;
                return new JObject { ["kind"] = kind, ["changed"] = action < 4 ? true : before != after, ["delta"] = delta, ["count"] = after };
            })).ConfigureAwait(false);
        }
        private static async Task<JToken> ClearTasksAsync(JObject a)
        {
            var kind = ReadTaskKind(a); var key = (string)a?["idempotencyKey"]; JObject prior; if (McpWriteGuard.TryGetCompleted("tasks.clear", key, a, out prior)) return prior;
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("tasks.clear", key, a, "清空全部" + kind + "任务；不会启动执行器或发送封包。", async () =>
            {
                var before = kind == "send" ? Operate.SendConfig.List.lstSendInfo.Count : kind == "robot" ? Operate.RobotConfig.List.lstRobotInfo.Count : Operate.WareHouseConfig.List.lstWareHouseInfo.Count;
                if (kind == "send") await Operate.SendConfig.List.CleanUpSendList_Dialog_Shell();
                else if (kind == "robot") await Operate.RobotConfig.List.CleanUpRobotList_Dialog_Shell();
                else await Operate.WareHouseConfig.List.CleanUpWareHouseList_Dialog_Shell();
                var after = kind == "send" ? Operate.SendConfig.List.lstSendInfo.Count : kind == "robot" ? Operate.RobotConfig.List.lstRobotInfo.Count : Operate.WareHouseConfig.List.lstWareHouseInfo.Count;
                return new JObject { ["kind"] = kind, ["changed"] = before != after, ["removed"] = before - after };
            })).ConfigureAwait(false);
        }
        private static async Task<JToken> AddCaptureAsync(string operation, bool send, bool proxy, JObject a)
        {
            var target = (string)a?["targetId"]; if (!Guid.TryParse(target, out _)) throw new InvalidOperationException("A target task GUID is required.");
            var values = a?["packetIds"] as JArray; if (values == null || values.Count == 0) throw new InvalidOperationException("At least one packet id is required.");
            var ids = new List<long>(); foreach (var value in values) { var id = (long?)value; if (!id.HasValue || id.Value < 1) throw new InvalidOperationException("Each packet id must be positive."); ids.Add(id.Value); }
            var key = (string)a?["idempotencyKey"]; JObject prior; if (McpWriteGuard.TryGetCompleted(operation, key, a, out prior)) return prior;
            var what = proxy ? "代理捕获封包" : "捕获封包"; var targetKind = send ? "发送任务" : "仓库";
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync(operation, key, a, "将 " + ids.Count + " 条" + what + "加入" + targetKind + "；不会启动执行器或发送封包。", () =>
            {
                var count = proxy ? (send ? Operate.ProxyConfig.List.AddToSend_ByProxyIds(target, ids) : Operate.ProxyConfig.List.AddToWareHouse_ByProxyIds(target, ids)) : (send ? Operate.PacketConfig.List.AddToSend_ByPacketIds(target, ids) : Operate.PacketConfig.List.AddToWareHouse_ByPacketIds(target, ids));
                return new JObject { ["targetId"] = target, ["requested"] = ids.Count, ["added"] = count, ["changed"] = count > 0 };
            })).ConfigureAwait(false);
        }

        private static string ReadEditablePacketList(string value)
        {
            value = (value ?? string.Empty).Trim().ToLowerInvariant();
            if (value != Operate.PacketEditConfig.ListProxy && value != Operate.PacketEditConfig.ListPacket)
                throw new InvalidOperationException("list must be proxy or packet; send collections are edited through their dedicated tools.");
            return value;
        }
        private static byte[] ReadPacketBytes(JObject a)
        {
            var text = (string)a?["payloadBase64"];
            if (string.IsNullOrWhiteSpace(text)) throw new InvalidOperationException("payloadBase64 is required.");
            try { var bytes = Convert.FromBase64String(text); if (bytes.Length == 0 || bytes.Length > MaxFrameBytes * 3 / 4) throw new InvalidOperationException("Payload size is out of range."); return bytes; }
            catch (FormatException) { throw new InvalidOperationException("payloadBase64 must be valid Base64."); }
        }
        private static int ReadCollectionAction(string value, bool export)
        {
            switch ((value ?? string.Empty).Trim().ToLowerInvariant())
            {
                case "top": return 0; case "up": return 1; case "down": return 2; case "bottom": return 3;
                case "copy": return 4; case "delete": return 6; case "clear": return 7;
                case "export": if (export) return 5; break;
            }
            throw new InvalidOperationException(export ? "action must be top, up, down, bottom, copy, export, or delete." : "action must be top, up, down, bottom, copy, or delete.");
        }
        private static IList<string> ReadCollectionIds(JObject a, string name)
        {
            var values = a?[name] as JArray; if (values == null || values.Count == 0 || values.Count > 200) throw new InvalidOperationException(name + " must contain between 1 and 200 ids.");
            var result = new List<string>(); var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var value in values) { var id = ((string)value ?? string.Empty).Trim(); if (id.Length == 0) throw new InvalidOperationException("Every id must be non-empty."); if (seen.Add(id)) result.Add(id); }
            return result;
        }
        private static async Task<JToken> SendCollectionActionAsync(JObject a)
        {
            var id = RequireGuidId(new JObject { ["id"] = a?["sendId"] }); var ids = ReadCollectionIds(a, "packetIds"); var action = ReadCollectionAction((string)a?["action"], false); var key = (string)a?["idempotencyKey"]; JObject prior;
            if (McpWriteGuard.TryGetCompleted("sends.collection.action", key, a, out prior)) return prior;
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("sends.collection.action", key, a, "编辑发送任务的 " + ids.Count + " 条封包；不会启动发送器。", async () =>
            {
                var edit = Operate.SendConfig.Send.OpenSendEdit_ById(id); try { if (string.IsNullOrEmpty(edit.Id)) throw new InvalidOperationException("The send task no longer exists."); var before = Operate.SendConfig.Send.GetSendCollectionRows().Length; var delta = await Operate.SendConfig.Send.SendCollectionAction_ByIds(action, ids); var error = Operate.SendConfig.Send.SaveSendEdit(edit.Name, edit.UseSystemSocket, edit.LoopCount, edit.LoopInterval, edit.Notes); if (!string.IsNullOrEmpty(error)) throw new InvalidOperationException(error); return new JObject { ["sendId"] = id, ["delta"] = delta, ["count"] = Operate.SendConfig.Send.GetSendCollectionRows().Length, ["changed"] = before != Operate.SendConfig.Send.GetSendCollectionRows().Length || action < 4 }; } finally { Operate.SendConfig.Send.CloseSendEdit(); }
            })).ConfigureAwait(false);
        }
        private static async Task<JToken> ClearSendCollectionAsync(JObject a)
        {
            var id = RequireGuidId(a); var key = (string)a?["idempotencyKey"]; JObject prior; if (McpWriteGuard.TryGetCompleted("sends.collection.clear", key, a, out prior)) return prior;
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("sends.collection.clear", key, a, "清空该发送任务的全部封包；不会启动发送器。", async () =>
            { var edit = Operate.SendConfig.Send.OpenSendEdit_ById(id); try { if (string.IsNullOrEmpty(edit.Id)) throw new InvalidOperationException("The send task no longer exists."); var before = Operate.SendConfig.Send.GetSendCollectionRows().Length; await Operate.SendConfig.Send.ClearSendCollection_Dialog_Shell(); var error = Operate.SendConfig.Send.SaveSendEdit(edit.Name, edit.UseSystemSocket, edit.LoopCount, edit.LoopInterval, edit.Notes); if (!string.IsNullOrEmpty(error)) throw new InvalidOperationException(error); return new JObject { ["sendId"] = id, ["removed"] = before - Operate.SendConfig.Send.GetSendCollectionRows().Length, ["changed"] = before != Operate.SendConfig.Send.GetSendCollectionRows().Length }; } finally { Operate.SendConfig.Send.CloseSendEdit(); } })).ConfigureAwait(false);
        }
        private static async Task<JToken> AddRobotInstructionAsync(JObject a)
        {
            var robot = ((string)a?["robot"] ?? (string)a?["robotId"] ?? string.Empty).Trim();
            if (robot.Length == 0) throw new InvalidOperationException("robot is required.");
            string id;
            if (Guid.TryParse(robot, out _)) id = robot;
            else
            {
                var matches = Operate.RobotConfig.List.lstRobotInfo.Where(x => x != null && string.Equals(x.RName, robot, StringComparison.OrdinalIgnoreCase)).ToList();
                if (matches.Count == 0) throw new InvalidOperationException("Robot '" + robot + "' does not exist.");
                if (matches.Count > 1) throw new InvalidOperationException("Robot name '" + robot + "' is not unique; use its GUID from wpe_robots_list.");
                id = matches[0].RID.ToString().ToUpperInvariant();
            }
            var type = (int?)a?["type"]; if (!type.HasValue) throw new InvalidOperationException("type is required."); var key = (string)a?["idempotencyKey"]; JObject prior; if (McpWriteGuard.TryGetCompleted("robots.instructions.add", key, a, out prior)) return prior;
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("robots.instructions.add", key, a, "向机器人任务添加一条指令；不会启动机器人。", () => { var edit = Operate.RobotConfig.Robot.OpenRobotEdit_ById(id); try { if (string.IsNullOrEmpty(edit.Id)) throw new InvalidOperationException("The robot task no longer exists."); var error = Operate.RobotConfig.Robot.AddRobotInstruction_Edit(type.Value, (string)a?["content"], (int?)a?["insertAt"] ?? -1); if (!string.IsNullOrEmpty(error)) throw new InvalidOperationException(error); error = Operate.RobotConfig.Robot.SaveRobotEdit(edit.Name); if (!string.IsNullOrEmpty(error)) throw new InvalidOperationException(error); return new JObject { ["robotId"] = id, ["changed"] = true }; } finally { Operate.RobotConfig.Robot.CloseRobotEdit(); } })).ConfigureAwait(false);
        }

        // A loop is only valid as a complete pair.  Saving each line separately (the
        // legacy add tool's behavior) can never persist a loop start, so accept the
        // ordered native rows and validate/save them as one edit transaction.
        private static async Task<JToken> SaveRobotInstructionsAsync(JObject a)
        {
            var id = (string)a?["robot"];
            if (!Guid.TryParse(id, out _)) throw new InvalidOperationException("robot must be an existing robot name or GUID.");
            var rows = a?["instructions"] as JArray;
            if (rows == null) throw new InvalidOperationException("instructions must be an array.");
            var parsed = new List<KeyValuePair<int, string>>();
            foreach (var row in rows.OfType<JObject>())
            {
                var type = (int?)row["type"];
                if (!type.HasValue || type.Value < 0 || type.Value > 8) throw new InvalidOperationException("Each instruction type must be an integer from 0 through 8.");
                parsed.Add(new KeyValuePair<int, string>(type.Value, ((string)row["content"] ?? string.Empty).Trim()));
            }
            if (parsed.Count != rows.Count) throw new InvalidOperationException("Each instruction must be an object with type and content.");
            var key = (string)a?["idempotencyKey"]; JObject prior;
            if (McpWriteGuard.TryGetCompleted("robots.instructions.save", key, a, out prior)) return prior;
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("robots.instructions.save", key, a, "替换机器人任务的完整指令列表；不会启动机器人。", async () =>
            {
                var edit = Operate.RobotConfig.Robot.OpenRobotEdit_ById(id);
                try
                {
                    if (string.IsNullOrEmpty(edit.Id)) throw new InvalidOperationException("The robot task no longer exists.");
                    var existing = Operate.RobotConfig.Robot.GetRobotInstructionRows();
                    if (existing.Length > 0) await Operate.RobotConfig.Robot.RobotInstructionAction_ByIndexes(6, existing.Select(x => x.Index).ToArray());
                    foreach (var item in parsed)
                    {
                        var error = Operate.RobotConfig.Robot.AddRobotInstruction_Edit(item.Key, item.Value, -1);
                        if (!string.IsNullOrEmpty(error)) throw new InvalidOperationException(error);
                    }
                    var saveError = Operate.RobotConfig.Robot.SaveRobotEdit(edit.Name);
                    if (!string.IsNullOrEmpty(saveError)) throw new InvalidOperationException(saveError);
                    return new JObject { ["robotId"] = id, ["changed"] = true, ["instructions"] = JArray.FromObject(Operate.RobotConfig.Robot.GetRobotInstructionRows()) };
                }
                finally { Operate.RobotConfig.Robot.CloseRobotEdit(); }
            })).ConfigureAwait(false);
        }
        private static async Task<JToken> RobotInstructionActionAsync(JObject a)
        {
            var id = (string)a?["robotId"]; if (!Guid.TryParse(id, out _)) throw new InvalidOperationException("A robot GUID id is required."); var action = ReadCollectionAction((string)a?["action"], false); if (action == 4) throw new InvalidOperationException("Robot instructions cannot be copied."); var values = a?["indexes"] as JArray; if (values == null) throw new InvalidOperationException("indexes is required."); var indexes = new List<int>(); foreach (var value in values) { var n = (int?)value; if (!n.HasValue || n.Value < 0) throw new InvalidOperationException("Every index must be non-negative."); indexes.Add(n.Value); } if (action != 7 && indexes.Count == 0) throw new InvalidOperationException("At least one index is required."); var key = (string)a?["idempotencyKey"]; JObject prior; if (McpWriteGuard.TryGetCompleted("robots.instructions.action", key, a, out prior)) return prior;
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("robots.instructions.action", key, a, "编辑机器人任务的指令；不会启动机器人。", async () => { var edit = Operate.RobotConfig.Robot.OpenRobotEdit_ById(id); try { if (string.IsNullOrEmpty(edit.Id)) throw new InvalidOperationException("The robot task no longer exists."); var delta = await Operate.RobotConfig.Robot.RobotInstructionAction_ByIndexes(action, indexes); var error = Operate.RobotConfig.Robot.SaveRobotEdit(edit.Name); if (!string.IsNullOrEmpty(error)) throw new InvalidOperationException(error); return new JObject { ["robotId"] = id, ["delta"] = delta, ["changed"] = action < 4 || delta != 0 }; } finally { Operate.RobotConfig.Robot.CloseRobotEdit(); } })).ConfigureAwait(false);
        }
        private static async Task<JToken> SaveAutoStoresAsync(JObject a)
        {
            var id = (string)a?["id"] ?? string.Empty; var warehouse = (string)a?["warehouseId"]; if (!Guid.TryParse(warehouse, out _)) throw new InvalidOperationException("warehouseId must be a GUID."); var head = (string)a?["packetHead"]; if (string.IsNullOrWhiteSpace(head)) throw new InvalidOperationException("packetHead is required."); var key = (string)a?["idempotencyKey"]; JObject prior; if (McpWriteGuard.TryGetCompleted("autoStores.save", key, a, out prior)) return prior;
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("autoStores.save", key, a, string.IsNullOrEmpty(id) ? "新增一条自动入库规则（默认停用）。" : "更新一条自动入库规则。", () => { var error = Operate.WareHouseConfig.List.SaveAutoStores_Shell(id, head, warehouse); if (!string.IsNullOrEmpty(error)) throw new InvalidOperationException(error); if (string.IsNullOrEmpty(id)) { foreach (var rule in Operate.WareHouseConfig.List.lstAutoStoresInfo) { if (rule != null && string.Equals(rule.PacketHead, head.Trim(), StringComparison.Ordinal)) { id = rule.AID.ToString().ToUpperInvariant(); break; } } } return new JObject { ["id"] = id, ["changed"] = true }; })).ConfigureAwait(false);
        }
        private static async Task<JToken> SetAutoStoresEnabledAsync(JObject a)
        {
            var id = RequireGuidId(a); var enabled = (bool?)a?["enabled"]; if (!enabled.HasValue) throw new InvalidOperationException("A boolean enabled value is required."); var key = (string)a?["idempotencyKey"]; JObject prior; if (McpWriteGuard.TryGetCompleted("autoStores.setEnabled", key, a, out prior)) return prior;
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("autoStores.setEnabled", key, a, (enabled.Value ? "启用" : "停用") + "一条自动入库规则。", () => { if (!Operate.WareHouseConfig.List.SetAutoStoresEnable_ById(id, enabled.Value)) throw new InvalidOperationException("The automatic-storage rule no longer exists."); return new JObject { ["id"] = id, ["enabled"] = enabled.Value, ["changed"] = true }; })).ConfigureAwait(false);
        }
        private static async Task<JToken> DeleteAutoStoresAsync(JObject a)
        {
            var id = RequireGuidId(a); var key = (string)a?["idempotencyKey"]; JObject prior; if (McpWriteGuard.TryGetCompleted("autoStores.delete", key, a, out prior)) return prior;
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("autoStores.delete", key, a, "删除一条自动入库规则。", async () => { var deleted = await Operate.WareHouseConfig.List.DeleteAutoStores_Dialog_ById(id); return new JObject { ["id"] = id, ["deleted"] = deleted, ["changed"] = deleted }; })).ConfigureAwait(false);
        }
        private static async Task<JToken> WarehouseStoresActionAsync(JObject a)
        {
            var id = RequireGuidId(new JObject { ["id"] = a?["warehouseId"] }); var ids = ReadCollectionIds(a, "storeIds"); var action = ReadCollectionAction((string)a?["action"], true); var key = (string)a?["idempotencyKey"]; JObject prior; if (McpWriteGuard.TryGetCompleted("warehouses.stores.action", key, a, out prior)) return prior;
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("warehouses.stores.action", key, a, "编辑仓库中的 " + ids.Count + " 条封包。", async () => { var before = Operate.WareHouseConfig.List.GetStoreRows_ById(id).Length; var delta = await Operate.WareHouseConfig.List.StoresAction_ByIds(id, action, ids); var after = Operate.WareHouseConfig.List.GetStoreRows_ById(id).Length; return new JObject { ["warehouseId"] = id, ["delta"] = delta, ["count"] = after, ["changed"] = before != after || action < 4 }; })).ConfigureAwait(false);
        }
        private static async Task<JToken> WarehouseStoresCommandAsync(JObject a)
        {
            var id = RequireGuidId(new JObject { ["id"] = a?["warehouseId"] }); var name = ((string)a?["action"] ?? string.Empty).Trim().ToLowerInvariant(); int action; if (name == "import") action = 8; else if (name == "export") action = 5; else if (name == "clear") action = 7; else throw new InvalidOperationException("action must be import, export, or clear."); var key = (string)a?["idempotencyKey"]; JObject prior; if (McpWriteGuard.TryGetCompleted("warehouses.stores.command", key, a, out prior)) return prior;
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("warehouses.stores.command", key, a, "运行仓库 " + name + " 命令。导入和导出会显示 WPE 的本地文件对话框。", async () => { var before = Operate.WareHouseConfig.List.GetStoreRows_ById(id).Length; await Operate.WareHouseConfig.List.StoresCommand_Shell(id, action); var after = Operate.WareHouseConfig.List.GetStoreRows_ById(id).Length; return new JObject { ["warehouseId"] = id, ["action"] = name, ["count"] = after, ["changed"] = before != after }; })).ConfigureAwait(false);
        }
        private static async Task<JToken> SavePacketEditAsync(JObject a)
        {
            var list = ReadEditablePacketList((string)a?["list"]); var id = (long?)a?["id"]; if (!id.HasValue || id.Value < 1) throw new InvalidOperationException("A positive packet id is required."); var bytes = ReadPacketBytes(a); var socket = (int?)a?["socket"]; if (!socket.HasValue) throw new InvalidOperationException("socket is required."); var key = (string)a?["idempotencyKey"]; JObject prior; if (McpWriteGuard.TryGetCompleted("packet.edit.save", key, a, out prior)) return prior;
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("packet.edit.save", key, a, "修改一条已捕获封包的套接字和内容。", () => { var error = Operate.PacketEditConfig.Save(list, id.Value, socket.Value, bytes); if (!string.IsNullOrEmpty(error)) throw new InvalidOperationException(error); return new JObject { ["id"] = id.Value, ["list"] = list, ["length"] = bytes.Length, ["changed"] = true }; })).ConfigureAwait(false);
        }
        private static async Task<JToken> AddPacketEditToSendAsync(JObject a)
        {
            var send = (string)a?["sendId"]; if (!Guid.TryParse(send, out _)) throw new InvalidOperationException("sendId must be a GUID."); var list = ReadEditablePacketList((string)a?["list"]); var id = (long?)a?["id"]; if (!id.HasValue || id.Value < 1) throw new InvalidOperationException("A positive packet id is required."); var bytes = ReadPacketBytes(a); var key = (string)a?["idempotencyKey"]; JObject prior; if (McpWriteGuard.TryGetCompleted("packet.edit.addToSend", key, a, out prior)) return prior;
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("packet.edit.addToSend", key, a, "复制一条封包到发送任务；不会启动发送器。", () => { if (!Operate.PacketEditConfig.AddToSend(send, list, id.Value, bytes)) throw new InvalidOperationException("The packet or send task no longer exists."); return new JObject { ["sendId"] = send, ["packetId"] = id.Value, ["added"] = true }; })).ConfigureAwait(false);
        }

        private static async Task<JToken> CreateFilterAsync(JObject arguments)
        {
            var key = (string)arguments?["idempotencyKey"];
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("filters.create", key, arguments, out prior)) return prior;
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("filters.create", key, arguments, "新建一个空白筛选器。", () =>
            {
                var id = Operate.FilterConfig.List.AddFilter_New_ById();
                if (string.IsNullOrEmpty(id)) throw new InvalidOperationException("Unable to create the filter.");
                return new JObject { ["changed"] = true, ["id"] = id };
            })).ConfigureAwait(false);
        }

        private static async Task<JToken> CreateFilterFromCaptureAsync(JObject arguments)
        {
            var packetId = (long?)arguments?["packetId"];
            var key = (string)arguments?["idempotencyKey"];
            if (!packetId.HasValue || packetId.Value < 1) throw new InvalidOperationException("A positive packetId is required.");
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("filters.createFromCapture", key, arguments, out prior)) return prior;
            if (ReadOnUi(() => Operate.PacketConfig.List.GetPacketById(packetId.Value)) == null) throw new InvalidOperationException("The captured packet does not exist.");
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("filters.createFromCapture", key, arguments, "从已捕获封包 #" + packetId.Value + " 新建滤镜。", () =>
            {
                var packet = Operate.PacketConfig.List.GetPacketById(packetId.Value);
                if (packet == null) throw new InvalidOperationException("The captured packet no longer exists.");
                var before = Operate.FilterConfig.List.lstFilterInfo.Count;
                if (!Operate.FilterConfig.Filter.AddFilter_ByPacketInfo(packet, null)) throw new InvalidOperationException("Unable to create a filter from the captured packet.");
                Operate.FilterConfig.List.SaveFilterList_ToDB();
                var filter = Operate.FilterConfig.List.lstFilterInfo.Count > before ? Operate.FilterConfig.List.lstFilterInfo[Operate.FilterConfig.List.lstFilterInfo.Count - 1] : null;
                return new JObject { ["changed"] = filter != null, ["filter"] = filter == null ? new JObject() : JObject.FromObject(FilterRow.From_(filter)) };
            })).ConfigureAwait(false);
        }

        private static async Task<JToken> SetAllFiltersEnabledAsync(JObject arguments)
        {
            var enabled = RequireBoolean(arguments, "enabled");
            var key = (string)arguments?["idempotencyKey"];
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("filters.setAllEnabled", key, arguments, out prior)) return prior;
            var total = ReadOnUi(() => Operate.FilterConfig.List.lstFilterInfo.Count);
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("filters.setAllEnabled", key, arguments, (enabled ? "启用" : "停用") + "全部 " + total + " 条滤镜。", () =>
            {
                var changed = Operate.FilterConfig.List.SetAllFilterEnable(enabled);
                return new JObject { ["changed"] = changed > 0, ["count"] = changed, ["enabled"] = enabled };
            })).ConfigureAwait(false);
        }

        private static async Task<JToken> ResetFilterCountsAsync(JObject arguments)
        {
            var key = (string)arguments?["idempotencyKey"];
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("filters.counts.reset", key, arguments, out prior)) return prior;
            var total = ReadOnUi(() => Operate.FilterConfig.List.lstFilterInfo.Count);
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("filters.counts.reset", key, arguments, "重置 " + total + " 条滤镜的运行期执行计数。", () =>
            {
                Operate.FilterConfig.List.ResetFilterCount();
                return new JObject { ["changed"] = total > 0, ["count"] = total };
            })).ConfigureAwait(false);
        }

        private static async Task<JToken> MoveFiltersAsync(JObject arguments)
        {
            var ids = ReadFilterIds(arguments);
            var direction = ((string)arguments?["direction"] ?? string.Empty).Trim().ToLowerInvariant();
            var action = direction == "top" ? 0 : direction == "up" ? 1 : direction == "down" ? 2 : direction == "bottom" ? 3 : -1;
            if (action < 0) throw new InvalidOperationException("direction must be top, up, down, or bottom.");
            var key = (string)arguments?["idempotencyKey"];
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("filters.move", key, arguments, out prior)) return prior;
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("filters.move", key, arguments, "调整 " + ids.Count + " 条滤镜的执行顺序（" + direction + "）。", async () =>
            {
                var before = FilterOrder();
                await Operate.FilterConfig.List.FilterListAction_ByIds(action, ids);
                var after = FilterOrder();
                return new JObject { ["changed"] = !string.Equals(before, after, StringComparison.Ordinal), ["direction"] = direction, ["count"] = ids.Count };
            })).ConfigureAwait(false);
        }

        private static async Task<JToken> CopyFiltersAsync(JObject arguments)
        {
            var ids = ReadFilterIds(arguments);
            var key = (string)arguments?["idempotencyKey"];
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("filters.copy", key, arguments, out prior)) return prior;
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("filters.copy", key, arguments, "复制 " + ids.Count + " 条滤镜。", async () =>
            {
                var before = Operate.FilterConfig.List.lstFilterInfo.Count;
                await Operate.FilterConfig.List.FilterListAction_ByIds(4, ids);
                var copied = Operate.FilterConfig.List.lstFilterInfo.Count - before;
                return new JObject { ["changed"] = copied > 0, ["count"] = copied };
            })).ConfigureAwait(false);
        }

        private static async Task<JToken> ClearAllFiltersAsync(JObject arguments)
        {
            var key = (string)arguments?["idempotencyKey"];
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("filters.clearAll", key, arguments, out prior)) return prior;
            var total = ReadOnUi(() => Operate.FilterConfig.List.lstFilterInfo.Count);
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("filters.clearAll", key, arguments, "清空全部 " + total + " 条滤镜；WPE 还会显示其原生删除确认。", async () =>
            {
                var before = Operate.FilterConfig.List.lstFilterInfo.Count;
                await Operate.FilterConfig.List.CleanUpFilterList_Dialog_Shell();
                return new JObject { ["changed"] = Operate.FilterConfig.List.lstFilterInfo.Count != before, ["count"] = before };
            })).ConfigureAwait(false);
        }

        // Semantic MCP contract for the complete WPE filter editor.  It deliberately
        // maps names and cell rules here instead of exposing FunctionMask, enum values
        // or the persisted FSearch/FModify strings to clients.
        private static async Task<JToken> SaveFilterRuleAsync(JObject arguments)
        {
            var id = ((string)arguments?["filter"] ?? string.Empty).Trim();
            var rule = arguments?["rule"] as JObject;
            if (rule == null) throw new InvalidOperationException("A complete rule object is required.");
            var key = (string)arguments?["idempotencyKey"];
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("filters.rule.save", key, arguments, out prior)) return prior;
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("filters.rule.save", key, arguments, "保存滤镜规则。", () =>
            {
                var row = Operate.FilterConfig.List.GetFilterEdit_ById(id);
                if (row == null) throw new InvalidOperationException("The filter does not exist.");
                ApplyFilterRule(row, rule, id);
                var error = Operate.FilterConfig.List.SaveFilterEdit(row);
                if (!string.IsNullOrEmpty(error)) throw new InvalidOperationException(error);
                var detail = Operate.FilterConfig.List.GetFilterEdit_ById(id);
                return new JObject { ["changed"] = true, ["filter"] = detail == null ? new JObject() : FilterRuleResult(detail, GetFilterEnabled(id) ?? false) };
            })).ConfigureAwait(false);
        }

        private static void ApplyFilterRule(FilterEditRow row, JObject rule, string filterId)
        {
            row.Name = OptionalRuleString(rule, "name") ?? row.Name;
            row.Mode = FilterChoice(rule, "mode", new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) { ["normal"] = 0, ["advanced"] = 1 });
            row.Action = FilterChoice(rule, "action", new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) { ["replace"] = 0, ["intercept"] = 1, ["display"] = 2, ["hide"] = 3, ["change"] = 5 });
            if (row.Mode == 1 && string.IsNullOrEmpty(OptionalRuleString(rule, "startFrom"))) throw new InvalidOperationException("Advanced filters must explicitly set startFrom: head processes only the first match; position processes every match.");
            row.StartFrom = FilterChoice(rule, "startFrom", new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) { ["head"] = 0, ["position"] = 1 }, 0);
            if (row.Mode == 0 && row.StartFrom != 0) throw new InvalidOperationException("startFrom position is available only in advanced mode.");
            row.FunctionMask = FilterPacketTypes(rule["packetTypes"] as JArray);
            row.Search = FilterSearchCells(rule["search"] as JArray, row.Mode);
            row.Modify = FilterModifyCells(rule["modify"] as JArray, row.Mode, row.StartFrom);
            if (row.Action == 5 && !IsCompleteReplacement(row.Modify))
                throw new InvalidOperationException("action change replaces the entire packet. Its modify cells must be contiguous from offset 0; use action replace to change selected bytes.");
            ApplyFilterConditions(row, rule["conditions"] as JObject);
            ApplyFilterProgression(row, rule["progression"] as JObject);
            ApplyFilterOnMatch(row, rule["onMatch"] as JObject, filterId);
        }

        private static int FilterChoice(JObject source, string field, IDictionary<string, int> choices, int? fallback = null)
        {
            var value = OptionalRuleString(source, field);
            if (string.IsNullOrEmpty(value) && fallback.HasValue) return fallback.Value;
            int result;
            if (string.IsNullOrEmpty(value) || !choices.TryGetValue(value, out result)) throw new InvalidOperationException(field + " must be one of: " + string.Join(", ", choices.Keys) + ".");
            return result;
        }

        private static string OptionalRuleString(JObject source, string field)
        {
            var token = source?[field];
            return token == null || token.Type == JTokenType.Null ? null : ((string)token ?? string.Empty).Trim();
        }

        private static int FilterPacketTypes(JArray values)
        {
            if (values == null) return AllPacketTypesForCurrentMode();
            if (values.Count == 0) throw new InvalidOperationException("packetTypes must contain at least one packet type; use all to select every packet type for the current mode.");
            var bits = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
            {
                ["send"] = 0, ["sendTo"] = 1, ["recv"] = 2, ["recvFrom"] = 3,
                ["wsaSend"] = 4, ["wsaSendTo"] = 5, ["wsaRecv"] = 6, ["wsaRecvFrom"] = 7,
                ["tcpRequest"] = 8, ["udpRequest"] = 9, ["tcpResponse"] = 10, ["udpResponse"] = 11,
            };
            var mask = 0;
            foreach (var token in values)
            {
                int bit;
                var value = ((string)token ?? string.Empty).Trim();
                if (string.Equals(value, "all", StringComparison.OrdinalIgnoreCase) || value == "全部" || value == "全选") { mask |= AllPacketTypesForCurrentMode(); continue; }
                if (string.Equals(value, "tcp", StringComparison.OrdinalIgnoreCase) || value == "TCP") { mask |= (1 << 8) | (1 << 10); continue; }
                if (string.Equals(value, "udp", StringComparison.OrdinalIgnoreCase) || value == "UDP") { mask |= (1 << 9) | (1 << 11); continue; }
                if (!bits.TryGetValue(value, out bit)) throw new InvalidOperationException("Unknown packet type: " + value + ".");
                mask |= 1 << bit;
            }
            var active = AllPacketTypesForCurrentMode();
            if ((mask & active) == 0) throw new InvalidOperationException("packetTypes does not select any packet type for the current WPE mode. Use all or choose a packet type for that mode.");
            return mask;
        }

        private static int AllPacketTypesForCurrentMode()
        {
            if (Operate.SystemConfig.SelectMode == Operate.SystemConfig.SystemMode.Proxy) return 0xF00;
            if (Operate.SystemConfig.SelectMode == Operate.SystemConfig.SystemMode.Inject) return 0x0FF;
            return 0xFFF;
        }

        private static FilterSearchCell[] FilterSearchCells(JArray values, int mode)
        {
            if (values == null || values.Count == 0) throw new InvalidOperationException("search must contain at least one matching cell.");
            var seen = new HashSet<int>();
            var cells = new List<FilterSearchCell>();
            foreach (var item in values.OfType<JObject>())
            {
                var index = (int?)item["offset"];
                var value = OptionalRuleString(item, "value");
                if (!index.HasValue || index.Value < 0 || index.Value >= Operate.FilterConfig.Filter.FilterSize_MaxLen || !seen.Add(index.Value)) throw new InvalidOperationException("search offsets must be unique integers from 0 to 999.");
                if (!IsFilterByte(value, mode == 1)) throw new InvalidOperationException("Each search value must be XX, X*, or *X; normal mode allows XX only.");
                cells.Add(new FilterSearchCell { Index = index.Value, Value = value.ToUpperInvariant(), Exclude = (bool?)item["exclude"] ?? false });
            }
            if (cells.Count != values.Count) throw new InvalidOperationException("Each search item must be an object.");
            if (mode == 1 && !cells.Any(x => x.Index == 0)) throw new InvalidOperationException("Advanced-mode search must define an exact anchor at offset 0.");
            if (mode == 1 && cells.Any(x => x.Index == 0 && x.Value.IndexOf('*') >= 0)) throw new InvalidOperationException("The advanced-mode search anchor at offset 0 must be an exact byte.");
            return cells.OrderBy(x => x.Index).ToArray();
        }

        private static FilterModifyCell[] FilterModifyCells(JArray values, int mode, int startFrom)
        {
            var seen = new HashSet<int>();
            var cells = new List<FilterModifyCell>();
            if (values == null) return cells.ToArray();
            var min = mode == 1 && startFrom == 1 ? -Operate.FilterConfig.Filter.FilterSize_MaxLen : 0;
            foreach (var item in values.OfType<JObject>())
            {
                var index = (int?)item["offset"];
                var progression = (bool?)item["progression"] ?? false;
                var random = (bool?)item["random"] ?? false;
                var value = OptionalRuleString(item, "value");
                if (!index.HasValue || index.Value < min || index.Value >= Operate.FilterConfig.Filter.FilterSize_MaxLen || !seen.Add(index.Value)) throw new InvalidOperationException("modify offsets are out of range or duplicated.");
                if (progression && random) throw new InvalidOperationException("A modify cell cannot be both progression and random.");
                if (!random && !IsFilterByte(value, false)) throw new InvalidOperationException("A non-random modify value must be an exact hexadecimal byte.");
                cells.Add(new FilterModifyCell { Index = index.Value, Value = random ? string.Empty : value.ToUpperInvariant(), Progression = progression, Random = random });
            }
            if (cells.Count != values.Count) throw new InvalidOperationException("Each modify item must be an object.");
            return cells.OrderBy(x => x.Index).ToArray();
        }

        private static bool IsCompleteReplacement(FilterModifyCell[] cells)
        {
            if (cells == null || cells.Length == 0) return false;
            var ordered = cells.OrderBy(x => x.Index).ToArray();
            if (ordered[0].Index != 0) return false;
            for (var i = 0; i < ordered.Length; i++) if (ordered[i].Index != i || ordered[i].Random) return false;
            return true;
        }

        private static bool IsFilterByte(string value, bool wildcard)
        {
            if (string.IsNullOrEmpty(value) || value.Length != 2) return false;
            var first = Uri.IsHexDigit(value[0]); var second = Uri.IsHexDigit(value[1]);
            return first && second || wildcard && ((first && value[1] == '*') || (value[0] == '*' && second));
        }

        private static void ApplyFilterConditions(FilterEditRow row, JObject conditions)
        {
            row.AppointHeader = SetCondition(conditions, "header", out row.HeaderContent);
            row.AppointSocket = SetCondition(conditions, "socket", out row.SocketContent);
            row.AppointLength = SetCondition(conditions, "length", out row.LengthContent);
            row.AppointPort = SetCondition(conditions, "port", out row.PortContent);
        }

        private static bool SetCondition(JObject conditions, string field, out string content)
        {
            content = OptionalRuleString(conditions, field) ?? string.Empty;
            return content.Length > 0;
        }

        private static void ApplyFilterProgression(FilterEditRow row, JObject progression)
        {
            row.IsProgressionContinuous = (bool?)progression?["continuous"] ?? false;
            row.ProgressionStep = (int?)progression?["step"] ?? 1;
            row.IsProgressionCarry = (bool?)progression?["carry"] ?? false;
            row.ProgressionCarryNumber = (int?)progression?["carryCount"] ?? 1;
            if (row.ProgressionStep < 1 || row.ProgressionCarryNumber < 1) throw new InvalidOperationException("progression step and carryCount must be positive.");
        }

        private static void ApplyFilterOnMatch(FilterEditRow row, JObject onMatch, string filterId)
        {
            var kind = OptionalRuleString(onMatch, "type") ?? "none";
            if (string.Equals(kind, "none", StringComparison.OrdinalIgnoreCase)) { row.IsExecute = false; row.ExecuteType = 2; row.ExecuteId = Guid.Empty.ToString(); return; }
            var kinds = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase) { ["send"] = 0, ["robot"] = 1, ["filter"] = 3, ["warehouse"] = 4 };
            int type;
            if (!kinds.TryGetValue(kind, out type)) throw new InvalidOperationException("onMatch type must be none, send, robot, filter, or warehouse.");
            var target = OptionalRuleString(onMatch, "target");
            if (string.IsNullOrEmpty(target)) throw new InvalidOperationException("onMatch target is required when type is not none.");
            var targetId = RequireGuid(kind, target);
            if (type == 3 && string.Equals(targetId, filterId, StringComparison.OrdinalIgnoreCase)) throw new InvalidOperationException("A filter cannot execute itself.");
            row.IsExecute = true; row.ExecuteType = type; row.ExecuteId = targetId;
        }

        private static string RequireGuid(string entityKind, string value)
        {
            Guid id;
            if (!Guid.TryParse(value, out id)) throw new InvalidOperationException(entityKind + " target must be a GUID returned by its list tool.");
            return id.ToString().ToUpperInvariant();
        }

        // Do not leak the editor DTO back to MCP callers: its numeric enums and
        // persistence-shaped field names invite callers to replay invalid payloads.
        private static JObject FilterRuleResult(FilterEditRow row, bool enabled)
        {
            var packetTypes = new JArray();
            var names = new[] { "send", "sendTo", "recv", "recvFrom", "wsaSend", "wsaSendTo", "wsaRecv", "wsaRecvFrom", "tcpRequest", "udpRequest", "tcpResponse", "udpResponse" };
            for (var i = 0; i < names.Length; i++) if ((row.FunctionMask & (1 << i)) != 0) packetTypes.Add(names[i]);
            var search = new JArray((row.Search ?? new FilterSearchCell[0]).OrderBy(x => x.Index).Select(x => new JObject { ["offset"] = x.Index, ["value"] = x.Value, ["exclude"] = x.Exclude }));
            var modify = new JArray((row.Modify ?? new FilterModifyCell[0]).OrderBy(x => x.Index).Select(x => new JObject { ["offset"] = x.Index, ["value"] = x.Random ? null : x.Value, ["progression"] = x.Progression, ["random"] = x.Random }));
            var conditions = new JObject();
            if (row.AppointHeader) conditions["header"] = row.HeaderContent;
            if (row.AppointSocket) conditions["socket"] = row.SocketContent;
            if (row.AppointLength) conditions["length"] = row.LengthContent;
            if (row.AppointPort) conditions["port"] = row.PortContent;
            var onMatch = new JObject { ["type"] = FilterOnMatchName(row.ExecuteType, row.IsExecute) };
            if (row.IsExecute) onMatch["target"] = row.ExecuteId;
            return new JObject
            {
                ["id"] = row.Id,
                ["name"] = row.Name,
                ["enabled"] = enabled,
                ["rule"] = new JObject
                {
                    ["mode"] = row.Mode == 1 ? "advanced" : "normal",
                    ["action"] = FilterActionName(row.Action),
                    ["startFrom"] = row.StartFrom == 1 ? "position" : "head",
                    ["packetTypes"] = packetTypes,
                    ["search"] = search,
                    ["modify"] = modify,
                    ["conditions"] = conditions,
                    ["progression"] = new JObject { ["continuous"] = row.IsProgressionContinuous, ["step"] = row.ProgressionStep, ["carry"] = row.IsProgressionCarry, ["carryCount"] = row.ProgressionCarryNumber },
                    ["onMatch"] = onMatch,
                },
            };
        }

        private static string FilterActionName(int action)
        {
            switch (action) { case 0: return "replace"; case 1: return "intercept"; case 2: return "display"; case 3: return "hide"; case 5: return "change"; default: return "replace"; }
        }

        private static string FilterOnMatchName(int type, bool enabled)
        {
            if (!enabled) return "none";
            switch (type) { case 0: return "send"; case 1: return "robot"; case 3: return "filter"; case 4: return "warehouse"; default: return "none"; }
        }


        private static async Task<JToken> DeleteFilterAsync(JObject arguments)
        {
            var id = ((string)arguments?["id"] ?? string.Empty).Trim();
            var key = (string)arguments?["idempotencyKey"];
            if (!Guid.TryParse(id, out _)) throw new InvalidOperationException("Filter id must be a GUID.");
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("filters.delete", key, arguments, out prior)) return prior;
            var name = ReadOnUi(() => { foreach (var item in Operate.FilterConfig.List.lstFilterInfo) if (item != null && string.Equals(item.FID.ToString(), id, StringComparison.OrdinalIgnoreCase)) return item.FName; return null; });
            if (name == null) throw new InvalidOperationException("The filter does not exist.");
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("filters.delete", key, arguments, "删除筛选器：" + name, async () =>
            {
                var before = Operate.FilterConfig.List.lstFilterInfo.Count;
                await Operate.FilterConfig.List.FilterListAction_ByIds((int)Operate.SystemConfig.ListAction.Delete, new[] { id });
                return new JObject { ["changed"] = Operate.FilterConfig.List.lstFilterInfo.Count < before, ["id"] = id };
            })).ConfigureAwait(false);
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

        private static async Task<JToken> CreateAccountAsync(JObject arguments)
        {
            var change = ReadAccountChange(arguments, true);
            var key = (string)arguments?["idempotencyKey"];
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("accounts.create", key, arguments, out prior)) return prior;
            if (ReadOnUi(() => FindAccountByName(change.UserName)) != null) throw new InvalidOperationException("An account with this user name already exists.");
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("accounts.create", key, arguments, "新建代理账号“" + change.UserName + "”。", () =>
            {
                if (FindAccountByName(change.UserName) != null) throw new InvalidOperationException("An account with this user name already exists.");
                var encrypted = Operate.SystemConfig.PassWord_Encrypt(change.Password);
                if (!Operate.ProxyConfig.Account.AddProxyAccount(change.Enabled, change.UserName, encrypted, change.LimitLinksEnabled, change.LimitLinks, change.LimitDevicesEnabled, change.LimitDevices, change.ExpiryEnabled, change.ExpiryTime)) throw new InvalidOperationException("Unable to create the account.");
                var account = FindAccountByName(change.UserName);
                if (account == null) throw new InvalidOperationException("The account was created but could not be read back.");
                return new JObject { ["changed"] = true, ["account"] = JObject.FromObject(AccountRow.From_(account)) };
            })).ConfigureAwait(false);
        }

        private static async Task<JToken> UpdateAccountAsync(JObject arguments)
        {
            var id = RequireAccountId(arguments);
            var change = ReadAccountChange(arguments, false);
            var key = (string)arguments?["idempotencyKey"];
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("accounts.update", key, arguments, out prior)) return prior;
            var account = ReadOnUi(() => FindAccount(id));
            if (account == null) throw new InvalidOperationException("The account does not exist.");
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("accounts.update", key, arguments, "修改代理账号“" + account.UserName + "”。", () =>
            {
                var current = FindAccount(id);
                if (current == null) throw new InvalidOperationException("The account no longer exists.");
                var password = string.IsNullOrEmpty(change.Password) ? string.Empty : Operate.SystemConfig.PassWord_Encrypt(change.Password);
                if (!Operate.ProxyConfig.Account.UpdateProxyAccount_ByAccountID(id, change.Enabled, password, change.LimitLinksEnabled, change.LimitLinks, change.LimitDevicesEnabled, change.LimitDevices, change.ExpiryEnabled, change.ExpiryTime)) throw new InvalidOperationException("Unable to update the account.");
                return new JObject { ["changed"] = true, ["account"] = JObject.FromObject(AccountRow.From_(current)) };
            })).ConfigureAwait(false);
        }

        private static async Task<JToken> DeleteAccountAsync(JObject arguments)
        {
            var id = RequireAccountId(arguments);
            var key = (string)arguments?["idempotencyKey"];
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("accounts.delete", key, arguments, out prior)) return prior;
            var account = ReadOnUi(() => FindAccount(id));
            if (account == null) throw new InvalidOperationException("The account does not exist.");
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("accounts.delete", key, arguments, "删除代理账号“" + account.UserName + "”。", () =>
            {
                if (!Operate.ProxyConfig.Account.DeleteAccount_ById(id)) throw new InvalidOperationException("The account no longer exists.");
                return new JObject { ["changed"] = true, ["id"] = id };
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

            var config = ReadOnUi(() => new { onlyWpc = Operate.ProxyConfig.Proxy.Only_WPC_Client });

            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync(
                "proxy.auth.setEnabled",
                idempotencyKey,
                arguments,
                (enabled ? "启用" : "停用") + "代理身份认证；只允许 WPC 客户端当前为" + (config.onlyWpc ? "开启" : "关闭") + "。",
                () =>
                {
                    bool changed;
                    var error = Operate.ProxyConfig.Proxy.SetProxyAuthEnabled(enabled, out changed);
                    if (!string.IsNullOrEmpty(error)) throw new InvalidOperationException(error);
                    return new JObject { ["changed"] = changed, ["enabled"] = enabled };
                })).ConfigureAwait(false);
        }

        private static async Task<JToken> SetProxyMaxConnectionsAsync(JObject arguments)
        {
            var valueToken = arguments?["maxConnection"];
            var idempotencyKey = (string)arguments?["idempotencyKey"];
            if (valueToken == null || valueToken.Type != JTokenType.Integer) throw new InvalidOperationException("maxConnection must be an integer.");
            var requested = valueToken.Value<int>();
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("proxy.maxConnections.set", idempotencyKey, arguments, out prior)) return prior;

            var current = ReadOnUi(() => Operate.ProxyConfig.Proxy.MaxConnectionNumber);
            var cap = ReadOnUi(() => Operate.ProxyConfig.Proxy.MaxConnectionCap());
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync(
                "proxy.maxConnections.set",
                idempotencyKey,
                arguments,
                "将代理最大连接数从 " + current + " 调整为 " + requested + "（当前上限 " + cap + "）。",
                () =>
                {
                    bool changed; int liveCap;
                    var error = Operate.ProxyConfig.Proxy.SetProxyMaxConnections(requested, out changed, out liveCap);
                    if (!string.IsNullOrEmpty(error)) throw new InvalidOperationException(error);
                    return new JObject { ["changed"] = changed, ["maxConnection"] = requested, ["cap"] = liveCap };
                })).ConfigureAwait(false);
        }

        private static async Task<JToken> SetProxySocks5PortAsync(JObject arguments)
        {
            var valueToken = arguments?["port"];
            var idempotencyKey = (string)arguments?["idempotencyKey"];
            if (valueToken == null || valueToken.Type != JTokenType.Integer) throw new InvalidOperationException("port must be an integer.");
            var requested = valueToken.Value<int>();
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("proxy.socks5Port.set", idempotencyKey, arguments, out prior)) return prior;

            var config = ReadOnUi(() => new { current = (int)Operate.ProxyConfig.Proxy.SOCKS5_Port });
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync(
                "proxy.socks5Port.set",
                idempotencyKey,
                arguments,
                "将 SOCKS5 监听端口从 " + config.current + " 调整为 " + requested + "。",
                () =>
                {
                    bool changed;
                    var error = Operate.ProxyConfig.Proxy.SetProxySocks5Port(requested, out changed);
                    if (!string.IsNullOrEmpty(error)) throw new InvalidOperationException(error);
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
                    bool changed;
                    var error = Operate.ProxyConfig.Proxy.SetFirewallEnabled(enabled, out changed);
                    if (!string.IsNullOrEmpty(error)) throw new InvalidOperationException(error);
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
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync(
                "proxy.onlyWpc.setEnabled",
                idempotencyKey,
                arguments,
                (enabled ? "启用" : "停用") + "只允许 WPC 客户端连接。",
                () =>
                {
                    bool changed;
                    var error = Operate.ProxyConfig.Proxy.SetOnlyWpcEnabled(enabled, out changed);
                    if (!string.IsNullOrEmpty(error)) throw new InvalidOperationException(error);
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

        private static Task<JToken> SearchPacketsAsync(JObject arguments)
        {
            var limit = Math.Max(1, Math.Min(200, (int?)arguments?["limit"] ?? 50));
            var offset = ReadOffset(arguments);
            var pattern = ((string)arguments?["pattern"] ?? string.Empty).Trim();
            var mode = ReadCaptureMode(arguments, "proxy");
            var snapshot = ReadOnUi(() => SnapshotPackets(mode));
            return Task.Run<JToken>(() => SearchPackets(snapshot, limit, offset, pattern));
        }

        private static CaptureSearchSnapshot[] SnapshotPackets(string mode)
        {
            var rows = new List<CaptureSearchSnapshot>();
            if (mode == "proxy")
            {
                foreach (var packet in Operate.ProxyConfig.List.lstProxyInfo)
                    if (packet != null) rows.Add(new CaptureSearchSnapshot(packet.Id, packet.ProxyTime, (int)packet.PacketType, packet.PacketLen, packet.PacketData, packet.ClientAddr, packet.ServerAddr, packet.ServerDomain, (int)packet.FilterAction));
            }
            else
            {
                foreach (var packet in Operate.PacketConfig.List.lstPacketInfo)
                    if (packet != null) rows.Add(new CaptureSearchSnapshot(packet.Id, packet.PacketTime, (int)packet.PacketType, packet.PacketLen, packet.PacketData, packet.PacketFrom, packet.PacketTo, null, (int)packet.FilterAction));
            }
            return rows.ToArray();
        }

        private static JObject SearchPackets(CaptureSearchSnapshot[] snapshot, int limit, int offset, string pattern)
        {
            var rows = new JArray(); var matched = 0;
            for (var i = snapshot.Length - 1; i >= 0 && rows.Count < limit; i--)
            {
                var packet = snapshot[i];
                var searchable = (packet.From ?? string.Empty) + " " + (packet.To ?? string.Empty) + " " + (packet.Domain ?? string.Empty) + " " + (packet.Preview ?? string.Empty);
                if (pattern.Length > 0 && searchable.IndexOf(pattern, StringComparison.OrdinalIgnoreCase) < 0) continue;
                if (matched++ < offset) continue;
                var preview = packet.Preview ?? string.Empty;
                if (preview.Length > 192) preview = preview.Substring(0, 192);
                rows.Add(new JObject { ["id"] = packet.Id, ["time"] = packet.Time.ToUniversalTime().ToString("o"), ["type"] = packet.Type, ["length"] = packet.Length, ["preview"] = preview, ["from"] = packet.From, ["to"] = packet.To, ["action"] = packet.Action });
            }
            return new JObject { ["rows"] = rows, ["nextCursor"] = NextCursor(offset, rows.Count, matched < snapshot.Length) };
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

        private static List<string> ReadFilterIds(JObject arguments)
        {
            var values = arguments?["ids"] as JArray;
            if (values == null || values.Count == 0 || values.Count > 200) throw new InvalidOperationException("ids must contain between 1 and 200 filter UUIDs.");
            var ids = new List<string>();
            var unique = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var value in values)
            {
                var id = ((string)value ?? string.Empty).Trim();
                if (!Guid.TryParse(id, out _)) throw new InvalidOperationException("Every filter id must be a GUID.");
                if (unique.Add(id)) ids.Add(id);
            }
            if (ids.Count == 0) throw new InvalidOperationException("At least one filter id is required.");
            return ids;
        }

        private static string FilterOrder()
        {
            var ids = new StringBuilder();
            foreach (var filter in Operate.FilterConfig.List.lstFilterInfo)
            {
                if (filter != null) ids.Append(filter.FID.ToString("N")).Append('|');
            }
            return ids.ToString();
        }

        private static AccountInfo FindAccountByName(string userName)
        {
            foreach (var account in Operate.ProxyConfig.Account.lstAccountInfo)
            {
                if (account != null && string.Equals(account.UserName, userName, StringComparison.OrdinalIgnoreCase)) return account;
            }
            return null;
        }

        private static string RequireAccountId(JObject arguments)
        {
            var id = ((string)arguments?["id"] ?? string.Empty).Trim();
            if (!Guid.TryParse(id, out _)) throw new InvalidOperationException("Account id must be a GUID.");
            return id;
        }

        private static AccountChange ReadAccountChange(JObject arguments, bool creating)
        {
            var userName = ((string)arguments?["userName"] ?? string.Empty).Trim();
            var password = (string)arguments?["password"] ?? string.Empty;
            if (creating && string.IsNullOrWhiteSpace(userName)) throw new InvalidOperationException("A userName is required.");
            if (userName.Length > 64) throw new InvalidOperationException("The userName must be at most 64 characters.");
            if (creating && string.IsNullOrEmpty(password)) throw new InvalidOperationException("A password is required when creating an account.");
            if (password.Length > 256) throw new InvalidOperationException("The password must be at most 256 characters.");
            var enabled = RequireBoolean(arguments, "enabled");
            var limitLinksEnabled = RequireBoolean(arguments, "limitLinksEnabled");
            var limitDevicesEnabled = RequireBoolean(arguments, "limitDevicesEnabled");
            var expiryEnabled = RequireBoolean(arguments, "expiryEnabled");
            var limitLinks = RequireNonNegativeInt(arguments, "limitLinks");
            var limitDevices = RequireNonNegativeInt(arguments, "limitDevices");
            var expiryTime = DateTime.Now.AddYears(1);
            var expiry = (string)arguments?["expiryTime"];
            if (expiryEnabled && (!DateTime.TryParse(expiry, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out expiryTime) || expiryTime <= DateTime.Now)) throw new InvalidOperationException("A future ISO 8601 expiryTime is required when expiry is enabled.");
            return new AccountChange { UserName = userName, Password = password, Enabled = enabled, LimitLinksEnabled = limitLinksEnabled, LimitLinks = limitLinks, LimitDevicesEnabled = limitDevicesEnabled, LimitDevices = limitDevices, ExpiryEnabled = expiryEnabled, ExpiryTime = expiryTime };
        }

        private static bool RequireBoolean(JObject arguments, string name)
        {
            var value = arguments?[name];
            if (value == null || value.Type != JTokenType.Boolean) throw new InvalidOperationException("A boolean " + name + " value is required.");
            return value.Value<bool>();
        }

        private static int RequireNonNegativeInt(JObject arguments, string name)
        {
            var value = arguments?[name];
            if (value == null || value.Type != JTokenType.Integer || value.Value<int>() < 0) throw new InvalidOperationException("A non-negative integer " + name + " value is required.");
            return value.Value<int>();
        }

        private sealed class AccountChange
        {
            public string UserName;
            public string Password;
            public bool Enabled;
            public bool LimitLinksEnabled;
            public int LimitLinks;
            public bool LimitDevicesEnabled;
            public int LimitDevices;
            public bool ExpiryEnabled;
            public DateTime ExpiryTime;
        }

        private static string DisplayFilterName(string name)
        {
            var value = (name ?? string.Empty).Replace("\r", " ").Replace("\n", " ").Trim();
            if (value.Length == 0) return "未命名滤镜";
            return value.Length <= 120 ? value : value.Substring(0, 120) + "…";
        }

        private static Task<JToken> GetPacketAsync(JObject arguments)
        {
            var id = (long?)arguments?["id"];
            if (!id.HasValue || id.Value < 1) throw new InvalidOperationException("A positive packet id is required.");
            var mode = ReadCaptureMode(arguments, "proxy");
            var snapshot = ReadOnUi(() => SnapshotPacket(id.Value, mode));
            if (snapshot == null) return Task.FromResult<JToken>(CaptureNotFound(id.Value, mode));
            return Task.Run<JToken>(() => BuildPacketResult(id.Value, mode, snapshot));
        }

        private static PacketSnapshot SnapshotPacket(long id, string mode)
        {
            byte[] bytes = null; byte[] raw = null;
            if (mode == "proxy") { var packet = Operate.ProxyConfig.List.GetProxyById(id); if (packet != null) { bytes = packet.PacketBuffer; raw = packet.RawBuffer; } }
            else { var packet = Operate.PacketConfig.List.GetPacketById(id); if (packet != null) { bytes = packet.PacketBuffer; raw = packet.RawBuffer; } }
            if (bytes == null && raw == null) return null;
            return new PacketSnapshot(bytes == null ? new byte[0] : (byte[])bytes.Clone(), raw == null ? new byte[0] : (byte[])raw.Clone());
        }

        private static JObject BuildPacketResult(long id, string mode, PacketSnapshot snapshot)
        {
            var bytes = snapshot.Bytes; var raw = snapshot.Raw;
            var result = new JObject { ["id"] = id, ["mode"] = mode, ["found"] = true, ["truncated"] = false };
            using (var sha = System.Security.Cryptography.SHA256.Create()) result["sha256"] = BitConverter.ToString(sha.ComputeHash(bytes)).Replace("-", string.Empty).ToLowerInvariant();
            result["payloadBase64"] = Convert.ToBase64String(bytes);
            result["rawPayloadBase64"] = Convert.ToBase64String(raw);
            result["modified"] = !bytes.AsSpan().SequenceEqual(raw);
            return result;
        }

        private static JObject FindNextCapture(JObject arguments)
        {
            var pattern = ((string)arguments?["pattern"] ?? string.Empty);
            if (pattern.Length == 0 || pattern.Length > 4096) throw new InvalidOperationException("pattern must contain between 1 and 4096 characters.");
            var mode = ReadCaptureMode(arguments, "proxy");
            var hex = (bool?)arguments?["hex"] ?? false;
            var fromIndex = Math.Max(0, (int?)arguments?["fromIndex"] ?? 0);
            var fromPosition = Math.Max(0, (int?)arguments?["fromPosition"] ?? 0);
            var hit = mode == "proxy" ? Operate.ProxyConfig.List.SearchProxy_Shell(pattern, hex, fromIndex, fromPosition) : Operate.PacketConfig.List.SearchPacket_Shell(pattern, hex, fromIndex, fromPosition);
            return JObject.FromObject(hit);
        }

        private static JObject CaptureNotFound(long id, string mode)
        {
            return new JObject { ["id"] = id, ["mode"] = mode, ["found"] = false, ["truncated"] = false, ["sha256"] = JValue.CreateNull(), ["payloadBase64"] = JValue.CreateNull(), ["rawPayloadBase64"] = JValue.CreateNull(), ["modified"] = false };
        }

        private static string ReadCaptureMode(JObject arguments, string fallback)
        {
            var mode = ((string)arguments?["mode"] ?? fallback).Trim().ToLowerInvariant();
            if (mode == "proxy" || mode == "inject") return mode;
            throw new InvalidOperationException("mode must be proxy or inject.");
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
            else if (kind == "mcp")
            {
                var list = Operate.LogConfig.List.lstMcpLogInfo;
                total = list.Count;
                for (var i = list.Count - 1 - offset; i >= 0 && rows.Count < limit; i--) { var x = list[i]; rows.Add(new JObject { ["time"] = x.LogTime.ToUniversalTime().ToString("o"), ["source"] = x.FuncName, ["message"] = x.LogContent }); }
            }
            else throw new InvalidOperationException("Log kind must be system, filter, proxy, or mcp.");
            return new JObject { ["rows"] = rows, ["nextCursor"] = NextCursor(offset, rows.Count, offset + rows.Count < total) };
        }

        private static JObject ListAllLogs(JObject arguments)
        {
            var limit = Math.Max(1, Math.Min(200, (int?)arguments?["limit"] ?? 50));
            var offset = ReadOffset(arguments);
            var all = new List<JObject>();
            foreach (var x in Operate.LogConfig.List.lstLogInfo)
                all.Add(new JObject { ["kind"] = "system", ["time"] = x.LogTime.ToUniversalTime().ToString("o"), ["source"] = x.FuncName, ["message"] = x.LogContent });
            foreach (var x in Operate.LogConfig.List.lstFilterLogInfo)
                all.Add(new JObject { ["kind"] = "filter", ["time"] = x.LogTime.ToUniversalTime().ToString("o"), ["filter"] = x.FName, ["action"] = (int)x.FAction, ["matches"] = x.MatchNum, ["type"] = (int)x.PacketType, ["length"] = x.PacketLen });
            foreach (var x in Operate.LogConfig.List.lstProxyLogInfo)
                all.Add(new JObject { ["kind"] = "proxy", ["time"] = x.LogTime.ToUniversalTime().ToString("o"), ["account"] = x.UserName, ["ip"] = x.LoginIP, ["message"] = x.LogContent });
            foreach (var x in Operate.LogConfig.List.lstMcpLogInfo)
                all.Add(new JObject { ["kind"] = "mcp", ["time"] = x.LogTime.ToUniversalTime().ToString("o"), ["source"] = x.FuncName, ["message"] = x.LogContent });
            all.Sort((left, right) => string.CompareOrdinal((string)right["time"], (string)left["time"]));
            var rows = new JArray();
            for (var i = offset; i < all.Count && rows.Count < limit; i++) rows.Add(all[i]);
            return new JObject { ["rows"] = rows, ["nextCursor"] = NextCursor(offset, rows.Count, offset + rows.Count < all.Count) };
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

        private static JObject GetFilter(JObject arguments)
        {
            var id = ((string)arguments?["id"] ?? string.Empty).Trim();
            if (!Guid.TryParse(id, out _)) throw new InvalidOperationException("Filter id must be a GUID.");
            var detail = Operate.FilterConfig.List.GetFilterEdit_ById(id);
            if (detail == null) throw new InvalidOperationException("The filter does not exist.");
            return FilterRuleResult(detail, GetFilterEnabled(id) ?? false);
        }

        private static JObject GetFilterStats(JObject arguments)
        {
            var id = ((string)arguments?["id"] ?? string.Empty).Trim();
            if (!Guid.TryParse(id, out _)) throw new InvalidOperationException("Filter id must be a GUID.");
            FilterInfo filter = null;
            foreach (var item in Operate.FilterConfig.List.lstFilterInfo)
            {
                if (item != null && string.Equals(item.FID.ToString(), id, StringComparison.OrdinalIgnoreCase)) { filter = item; break; }
            }
            if (filter == null) throw new InvalidOperationException("The filter does not exist.");
            return new JObject { ["id"] = filter.FID.ToString().ToUpperInvariant(), ["name"] = filter.FName, ["enabled"] = filter.IsEnable, ["executionCount"] = filter.ExecutionCount, ["sampledAtUtc"] = DateTime.UtcNow.ToString("o") };
        }

        private static JObject ListAccounts(JObject arguments)
        {
            var limit = Math.Max(1, Math.Min(200, (int?)arguments?["limit"] ?? 50));
            var offset = ReadOffset(arguments);
            var userName = ((string)arguments?["userName"] ?? string.Empty).Trim();
            if (userName.Length > 128) throw new InvalidOperationException("userName must not exceed 128 characters.");
            var rows = new JArray();
            var list = Operate.ProxyConfig.Account.lstAccountInfo;
            var matched = new List<AccountInfo>();
            for (var i = 0; i < list.Count; i++)
            {
                var account = list[i];
                if (account == null || (userName.Length > 0 && (account.UserName ?? string.Empty).IndexOf(userName, StringComparison.OrdinalIgnoreCase) < 0)) continue;
                matched.Add(account);
            }
            for (var i = offset; i < matched.Count && rows.Count < limit; i++)
            {
                var account = matched[i];
                var row = AccountToMcp(account);
                if (row != null) rows.Add(row);
            }
            return new JObject { ["rows"] = rows, ["nextCursor"] = NextCursor(offset, rows.Count, offset + rows.Count < matched.Count) };
        }

        private static JObject GetAccount(JObject arguments)
        {
            var id = ((string)arguments?["id"] ?? string.Empty).Trim();
            Guid accountId;
            if (!Guid.TryParse(id, out accountId)) throw new InvalidOperationException("Account id must be a GUID.");
            var account = Operate.ProxyConfig.Account.GetProxyAccount_ByAccountID(accountId);
            if (account == null) throw new InvalidOperationException("The account does not exist.");
            var result = AccountToMcp(account);
            result["logins"] = JArray.FromObject(Operate.ProxyConfig.Account.GetAccountLogins_ById(accountId.ToString()));
            return result;
        }

        private static JObject AccountToMcp(AccountInfo account)
        {
            var row = AccountRow.From_(account);
            if (row == null) return null;
            var result = JObject.FromObject(row);
            result["password"] = Operate.SystemConfig.PassWord_Decrypt(account.Password ?? string.Empty);
            return result;
        }

        private static JObject ListAccountLogins(JObject arguments)
        {
            var id = RequireAccountId(arguments);
            if (FindAccount(id) == null) throw new InvalidOperationException("The account does not exist.");
            var limit = Math.Max(1, Math.Min(200, (int?)arguments?["limit"] ?? 50));
            var offset = ReadOffset(arguments);
            var list = Operate.ProxyConfig.Account.GetAccountLogins_ById(id);
            var rows = new JArray();
            for (var i = offset; i < list.Count && rows.Count < limit; i++) rows.Add(JObject.FromObject(list[i]));
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
                var connection = list[i];
                var row = AuthRow.From_(connection);
                if (row != null)
                {
                    var item = JObject.FromObject(row);
                    item["id"] = GetConnectionId(connection);
                    rows.Add(item);
                }
            }
            return new JObject { ["rows"] = rows, ["nextCursor"] = NextCursor(offset, rows.Count, offset + rows.Count < list.Count) };
        }

        private static string GetConnectionId(AuthInfo connection)
        {
            var material = connectionIdSalt + "|" + connection.AID.ToString("N") + "|" + (connection.AuthIP ?? string.Empty) + "|" + (connection.DeviceId ?? string.Empty);
            using (var hash = SHA256.Create())
            {
                return Convert.ToBase64String(hash.ComputeHash(Encoding.UTF8.GetBytes(material)))
                    .TrimEnd('=').Replace('+', '-').Replace('/', '_');
            }
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
                ["authEnabled"] = Operate.ProxyConfig.Proxy.Enable_Auth,
                ["onlyWpc"] = Operate.ProxyConfig.Proxy.Only_WPC_Client,
                ["maxConnection"] = Operate.ProxyConfig.Proxy.MaxConnectionNumber,
                ["maxConnectionCap"] = Operate.ProxyConfig.Proxy.MaxConnectionCap(),
                ["externalProxyEnabled"] = Operate.ProxyConfig.Proxy.Enable_ExternalProxy,
                ["externalProxyIp"] = Operate.ProxyConfig.Proxy.ExternalProxy_IP ?? string.Empty,
                ["externalProxyPort"] = (int)Operate.ProxyConfig.Proxy.ExternalProxy_Port,
                ["externalProxyAppointPort"] = Operate.ProxyConfig.Proxy.Enable_ExternalProxy_AppointPort,
                ["externalProxyAuthEnabled"] = Operate.ProxyConfig.Proxy.Enable_ExternalProxy_Auth,
                ["externalProxyUserName"] = Operate.ProxyConfig.Proxy.ExternalProxy_UserName ?? string.Empty,
                ["externalProxyPassword"] = Operate.ProxyConfig.Proxy.ExternalProxy_PassWord ?? string.Empty,
                ["externalProxyAppointPortContent"] = Operate.ProxyConfig.Proxy.ExternalProxy_AppointPort ?? string.Empty,
                ["running"] = Operate.ProxyConfig.Proxy.IsRunning
            };
        }

        private static JObject GetProxyConfig()
        {
            var result = GetProxySettings();
            result["schemaVersion"] = 1;
            result["requiresRestart"] = false;
            return result;
        }

        private static JObject GetProxyRuntime()
        {
            var result = GetProxySettings();
            result["sessionCount"] = Operate.ProxyConfig.Proxy.SessionCount;
            result["capturedPackets"] = Operate.PacketConfig.Packet.TotalPackets;
            result["sampledAtUtc"] = DateTime.UtcNow.ToString("o");
            return result;
        }

        private static JObject GetRemoteManagement()
        {
            var setting = Operate.SystemConfig.GetRemoteSetting();
            return new JObject
            {
                ["enabled"] = setting.IsRemote,
                ["running"] = setting.Running,
                ["ip"] = setting.IP,
                ["port"] = setting.Port,
                ["url"] = Operate.SystemConfig.GetRemoteMGT_URL(setting.IP, setting.Port.ToString()),
                ["userName"] = setting.UserName,
                ["password"] = setting.PassWord,
                ["availableIps"] = new JArray(setting.IPs ?? new string[0]),
                ["ipMissing"] = setting.IPMissing
            };
        }

        private static JObject GetSetting(JObject arguments)
        {
            var page = ((string)arguments?["page"] ?? string.Empty).Trim().ToLowerInvariant();
            var mode = ((string)arguments?["mode"] ?? "proxy").Trim().ToLowerInvariant();
            if (page == "proxy") return GetProxySettings();
            if (page == "mihomo") return JObject.FromObject(Operate.ProxyConfig.Proxy.GetMihomoSetting());
            if (page == "extproxy") return JObject.FromObject(Operate.ProxyConfig.Proxy.GetExtProxySetting());
            if (page == "remote") return GetRemoteManagement();
            if (page == "firewall") return GetFirewall();
            if (page == "map") return new JObject { ["enableLocal"] = Operate.ProxyConfig.Mapping.Enable_MapLocal, ["enableRemote"] = Operate.ProxyConfig.Mapping.Enable_MapRemote };
            if (page == "hotkey") return JObject.FromObject(Operate.SystemConfig.GetHotkeySetting());
            if (page == "system") return new JObject { ["speedMode"] = Operate.SystemConfig.SpeedMode, ["listExecute"] = (int)Operate.SystemConfig.ListExecute, ["filterExecute"] = (int)Operate.FilterConfig.Filter.FilterExecute };
            if (page == "log") return new JObject { ["autoClear"] = Operate.LogConfig.List.AutoClear, ["autoClearValue"] = (int)Operate.LogConfig.List.AutoClear_Value };
            if (page == "hook") return new JObject { ["ws1Send"] = Operate.PacketConfig.Packet.HookWS1_Send, ["ws1SendTo"] = Operate.PacketConfig.Packet.HookWS1_SendTo, ["ws1Recv"] = Operate.PacketConfig.Packet.HookWS1_Recv, ["ws1RecvFrom"] = Operate.PacketConfig.Packet.HookWS1_RecvFrom, ["ws2Send"] = Operate.PacketConfig.Packet.HookWS2_Send, ["ws2SendTo"] = Operate.PacketConfig.Packet.HookWS2_SendTo, ["ws2Recv"] = Operate.PacketConfig.Packet.HookWS2_Recv, ["ws2RecvFrom"] = Operate.PacketConfig.Packet.HookWS2_RecvFrom, ["wsaSend"] = Operate.PacketConfig.Packet.HookWSA_Send, ["wsaSendTo"] = Operate.PacketConfig.Packet.HookWSA_SendTo, ["wsaRecv"] = Operate.PacketConfig.Packet.HookWSA_Recv, ["wsaRecvFrom"] = Operate.PacketConfig.Packet.HookWSA_RecvFrom, ["tcpReq"] = Operate.ProxyConfig.Proxy.HookTCP_Req, ["tcpResp"] = Operate.ProxyConfig.Proxy.HookTCP_Resp, ["udpReq"] = Operate.ProxyConfig.Proxy.HookUDP_Req, ["udpResp"] = Operate.ProxyConfig.Proxy.HookUDP_Resp, ["unpack"] = Operate.ProxyConfig.Proxy.Enable_UnPack, ["unpackHead"] = Operate.ProxyConfig.Proxy.UnPack_Head ?? string.Empty, ["unpackLength"] = Operate.ProxyConfig.Proxy.UnPack_Length ?? string.Empty };
            if (page == "leach") { var f = Operate.SystemConfig.CheckType_Value; return new JObject { ["notShow"] = Operate.SystemConfig.CheckNotShow, ["checkSocket"] = Operate.SystemConfig.CheckSocket, ["socketValue"] = Operate.SystemConfig.CheckSocket_Value ?? string.Empty, ["checkIP"] = Operate.SystemConfig.CheckIP, ["ipValue"] = Operate.SystemConfig.CheckIP_Value ?? string.Empty, ["checkPort"] = Operate.SystemConfig.CheckPort, ["portValue"] = Operate.SystemConfig.CheckPort_Value ?? string.Empty, ["checkHead"] = Operate.SystemConfig.CheckHead, ["headValue"] = Operate.SystemConfig.CheckHead_Value ?? string.Empty, ["checkData"] = Operate.SystemConfig.CheckData, ["dataValue"] = Operate.SystemConfig.CheckData_Value ?? string.Empty, ["checkLen"] = Operate.SystemConfig.CheckLen, ["lenValue"] = Operate.SystemConfig.CheckLength_Value ?? string.Empty, ["checkType"] = Operate.SystemConfig.CheckType, ["send"] = f.Send, ["sendTo"] = f.SendTo, ["recv"] = f.Recv, ["recvFrom"] = f.RecvFrom, ["wsaSend"] = f.WSASend, ["wsaSendTo"] = f.WSASendTo, ["wsaRecv"] = f.WSARecv, ["wsaRecvFrom"] = f.WSARecvFrom, ["tcpReq"] = f.TCP_Req, ["tcpResp"] = f.TCP_Resp, ["udpReq"] = f.UDP_Req, ["udpResp"] = f.UDP_Resp }; }
            if (page == "list")
            {
                var inject = mode == "inject" || mode == "packet";
                return new JObject { ["mode"] = inject ? "inject" : "proxy", ["showSocket"] = inject ? Operate.PacketConfig.List.IsShow_PacketSocket : Operate.ProxyConfig.List.IsShow_PacketSocket, ["showType"] = inject ? Operate.PacketConfig.List.IsShow_PacketType : Operate.ProxyConfig.List.IsShow_PacketType, ["showClientAddr"] = inject ? Operate.PacketConfig.List.IsShow_ClientAddr : Operate.ProxyConfig.List.IsShow_ClientAddr, ["showClientLoc"] = inject ? Operate.PacketConfig.List.IsShow_ClientLocation : Operate.ProxyConfig.List.IsShow_ClientLocation, ["showServerAddr"] = inject ? Operate.PacketConfig.List.IsShow_ServerAddr : Operate.ProxyConfig.List.IsShow_ServerAddr, ["showServerLoc"] = inject ? Operate.PacketConfig.List.IsShow_ServerLocation : Operate.ProxyConfig.List.IsShow_ServerLocation, ["showLen"] = inject ? Operate.PacketConfig.List.IsShow_PacketLen : Operate.ProxyConfig.List.IsShow_PacketLen, ["autoClear"] = Operate.PacketConfig.List.AutoClear, ["autoClearValue"] = (int)Operate.PacketConfig.List.AutoClear_Value };
            }
            throw new InvalidOperationException("Unsupported settings page: " + page);
        }

        private static JObject ListWpcServers()
        {
            var rows = new JArray();
            foreach (var server in Operate.WPCConfig.ServerList.lstServerInfo)
            {
                if (server == null) continue;
                rows.Add(new JObject { ["id"] = server.SID.ToString().ToUpperInvariant(), ["enabled"] = server.IsEnable, ["name"] = server.ServerName ?? string.Empty, ["ip"] = server.ServerIP ?? string.Empty, ["port"] = server.ServerPort, ["forgotUrl"] = server.ForgotURL ?? string.Empty, ["registerUrl"] = server.RegisterURL ?? string.Empty, ["verifyUrl"] = server.VerifyURL ?? string.Empty, ["ruleCount"] = server.ServerRInfo == null ? 0 : server.ServerRInfo.Count });
            }
            return new JObject { ["rows"] = rows };
        }

        private static JObject ListWpcServerRules(JObject arguments)
        {
            var serverId = (string)arguments?["serverId"];
            if (Operate.WPCConfig.ServerList.FindServer_ById(serverId) == null) throw new InvalidOperationException("The WPC server does not exist.");
            var rows = new JArray();
            foreach (var rule in Operate.WPCConfig.ServerList.GetRuleRows_ById(serverId))
            {
                rows.Add(new JObject { ["id"] = rule.Id, ["enabled"] = rule.IsEnable, ["type"] = rule.Type, ["typeName"] = rule.TypeName, ["argument"] = rule.Argument, ["action"] = rule.Action });
            }
            return new JObject { ["serverId"] = serverId, ["rows"] = rows };
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

        private static async Task<JToken> SetProxyBindIpAsync(JObject arguments)
        {
            var autoToken = arguments?["auto"];
            var ip = ((string)arguments?["ip"] ?? string.Empty).Trim();
            var idempotencyKey = (string)arguments?["idempotencyKey"];
            if (autoToken == null || autoToken.Type != JTokenType.Boolean) throw new InvalidOperationException("auto must be a boolean.");
            var auto = autoToken.Value<bool>();
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
                    bool changed;
                    var error = Operate.ProxyConfig.Proxy.SetProxyBindIp(auto, ip, out changed);
                    if (!string.IsNullOrEmpty(error)) throw new InvalidOperationException(error);
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
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync(
                "proxy.external.setEnabled", idempotencyKey, arguments,
                (enabled ? "启用" : "停用") + "外部代理（" + config.ip + ":" + config.port + "；认证" + (config.auth ? "已启用" : "未启用") + "）。",
                () =>
                {
                    var host = (Operate.ProxyConfig.Proxy.ExternalProxy_IP ?? string.Empty).Trim();
                    var port = (int)Operate.ProxyConfig.Proxy.ExternalProxy_Port;
                    bool changed;
                    var error = Operate.ProxyConfig.Proxy.SetExternalProxyEnabled(enabled, out changed);
                    if (!string.IsNullOrEmpty(error)) throw new InvalidOperationException(error);
                    return new JObject { ["changed"] = changed, ["enabled"] = enabled, ["host"] = host, ["port"] = port };
                })).ConfigureAwait(false);
        }

        private static Task<JToken> SaveMapLocalAsync(JObject a) { return SaveMapAsync("map.local.save", a, false); }
        private static Task<JToken> SaveMapRemoteAsync(JObject a) { return SaveMapAsync("map.remote.save", a, true); }
        private static async Task<JToken> SaveMapAsync(string operation, JObject a, bool remote)
        {
            var key = (string)a?["idempotencyKey"]; JObject prior; if (McpWriteGuard.TryGetCompleted(operation, key, a, out prior)) return prior;
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync(operation, key, a, (string.IsNullOrEmpty((string)a?["id"]) ? "新增" : "更新") + " HTTP 映射规则。", () =>
            {
                string error;
                if (remote) error = Operate.ProxyConfig.Mapping.SaveMapRemote_Shell((string)a?["id"], (int?)a?["protocolFrom"] ?? 0, (string)a?["hostFrom"], (int?)a?["portFrom"] ?? 80, (string)a?["pathFrom"], (int?)a?["protocolTo"] ?? 0, (string)a?["hostTo"], (int?)a?["portTo"] ?? 80, (string)a?["pathTo"]);
                else error = Operate.ProxyConfig.Mapping.SaveMapLocal_Shell((string)a?["id"], (int?)a?["protocol"] ?? 0, (string)a?["host"], (int?)a?["port"] ?? 80, (string)a?["remotePath"], (string)a?["localPath"]);
                if (!string.IsNullOrEmpty(error)) throw new InvalidOperationException(error);
                return new JObject { ["changed"] = true, ["created"] = string.IsNullOrEmpty((string)a?["id"]) };
            })).ConfigureAwait(false);
        }

        private static async Task<JToken> SaveSettingAsync(JObject arguments)
        {
            var page = ((string)arguments?["page"] ?? string.Empty).Trim().ToLowerInvariant();
            var values = arguments?["values"] as JObject;
            var key = (string)arguments?["idempotencyKey"];
            if (values == null) throw new InvalidOperationException("values must be an object.");
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("settings.save", key, arguments, out prior)) return prior;
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("settings.save", key, arguments, "保存 WPE “" + page + "”设置页的完整配置。", () =>
            {
                bool Bool(string n) { return (bool?)values[n] ?? false; }
                int Int(string n, int d) { return (int?)values[n] ?? d; }
                string Text(string n) { return ((string)values[n] ?? string.Empty).Trim(); }
                if (page == "proxy")
                {
                    var error = Operate.ProxyConfig.Proxy.SaveProxySettings(Bool("proxyIpAuto"), Text("proxyIp"), Bool("enableSocks5"), Int("socks5Port", 1080), Bool("enableAuth"), Bool("onlyWpc"), Int("maxConnection", Operate.ProxyConfig.Proxy.DefaultMaxConnectionNumber));
                    if (!string.IsNullOrEmpty(error)) throw new InvalidOperationException(error);
                }
                else if (page == "extproxy")
                {
                    var error = Operate.ProxyConfig.Proxy.SaveExtProxySetting(Bool("enable"), Text("ip"), Int("port", 8889), Bool("appointPort"), Text("appointPortContent"), Bool("auth"), Text("userName"), Text("passWord"));
                    if (!string.IsNullOrEmpty(error)) throw new InvalidOperationException(error);
                }
                else if (page == "map") Operate.ProxyConfig.Mapping.SaveMapSetting_Shell(Bool("enableLocal"), Bool("enableRemote"));
                else if (page == "system")
                {
                    Operate.SystemConfig.SpeedMode = Bool("speedMode");
                    Operate.SystemConfig.ListExecute = Int("listExecute", 0) == 1 ? Operate.SystemConfig.Execute.Sequence : Operate.SystemConfig.Execute.Together;
                    Operate.FilterConfig.Filter.FilterExecute = Int("filterExecute", 0) == 1 ? Operate.FilterConfig.Filter.Execute.Sequence : Operate.FilterConfig.Filter.Execute.Priority;
                    Operate.SystemConfig.SaveSystemConfig_ToDB();
                }
                else if (page == "log")
                {
                    var keep = Int("autoClearValue", (int)Operate.LogConfig.List.AutoClear_Value); if (keep < 100 || keep > 500000) throw new InvalidOperationException("autoClearValue must be from 100 to 500000.");
                    Operate.LogConfig.List.AutoClear = Bool("autoClear"); Operate.LogConfig.List.AutoClear_Value = keep; Operate.SystemConfig.SaveSystemConfig_ToDB();
                }
                else if (page == "list")
                {
                    var inject = Text("mode") == "inject" || Text("mode") == "packet";
                    if (inject) { Operate.PacketConfig.List.IsShow_PacketSocket = Bool("showSocket"); Operate.PacketConfig.List.IsShow_PacketType = Bool("showType"); Operate.PacketConfig.List.IsShow_ClientAddr = Bool("showClientAddr"); Operate.PacketConfig.List.IsShow_ClientLocation = Bool("showClientLoc"); Operate.PacketConfig.List.IsShow_ServerAddr = Bool("showServerAddr"); Operate.PacketConfig.List.IsShow_ServerLocation = Bool("showServerLoc"); Operate.PacketConfig.List.IsShow_PacketLen = Bool("showLen"); }
                    else { Operate.ProxyConfig.List.IsShow_PacketSocket = Bool("showSocket"); Operate.ProxyConfig.List.IsShow_PacketType = Bool("showType"); Operate.ProxyConfig.List.IsShow_ClientAddr = Bool("showClientAddr"); Operate.ProxyConfig.List.IsShow_ClientLocation = Bool("showClientLoc"); Operate.ProxyConfig.List.IsShow_ServerAddr = Bool("showServerAddr"); Operate.ProxyConfig.List.IsShow_ServerLocation = Bool("showServerLoc"); Operate.ProxyConfig.List.IsShow_PacketLen = Bool("showLen"); }
                    Operate.SystemConfig.SaveProxyMode_ToDB(); Operate.SystemConfig.SaveInjectMode_ToDB();
                }
                else if (page == "hook")
                {
                    Operate.PacketConfig.Packet.HookWS1_Send = Bool("ws1Send"); Operate.PacketConfig.Packet.HookWS1_SendTo = Bool("ws1SendTo"); Operate.PacketConfig.Packet.HookWS1_Recv = Bool("ws1Recv"); Operate.PacketConfig.Packet.HookWS1_RecvFrom = Bool("ws1RecvFrom"); Operate.PacketConfig.Packet.HookWS2_Send = Bool("ws2Send"); Operate.PacketConfig.Packet.HookWS2_SendTo = Bool("ws2SendTo"); Operate.PacketConfig.Packet.HookWS2_Recv = Bool("ws2Recv"); Operate.PacketConfig.Packet.HookWS2_RecvFrom = Bool("ws2RecvFrom"); Operate.PacketConfig.Packet.HookWSA_Send = Bool("wsaSend"); Operate.PacketConfig.Packet.HookWSA_SendTo = Bool("wsaSendTo"); Operate.PacketConfig.Packet.HookWSA_Recv = Bool("wsaRecv"); Operate.PacketConfig.Packet.HookWSA_RecvFrom = Bool("wsaRecvFrom");
                    Operate.ProxyConfig.Proxy.HookTCP_Req = Bool("tcpReq"); Operate.ProxyConfig.Proxy.HookTCP_Resp = Bool("tcpResp"); Operate.ProxyConfig.Proxy.HookUDP_Req = Bool("udpReq"); Operate.ProxyConfig.Proxy.HookUDP_Resp = Bool("udpResp"); Operate.ProxyConfig.Proxy.Enable_UnPack = Bool("unpack"); Operate.ProxyConfig.Proxy.UnPack_Head = Text("unpackHead"); Operate.ProxyConfig.Proxy.UnPack_Length = Text("unpackLength"); Operate.SystemConfig.SaveInjectMode_ToDB(); Operate.SystemConfig.SaveProxyMode_ToDB();
                }
                else if (page == "leach")
                {
                    Operate.SystemConfig.CheckNotShow = Bool("notShow"); Operate.SystemConfig.CheckSocket = Bool("checkSocket"); Operate.SystemConfig.CheckSocket_Value = Text("socketValue"); Operate.SystemConfig.CheckIP = Bool("checkIP"); Operate.SystemConfig.CheckIP_Value = Text("ipValue"); Operate.SystemConfig.CheckPort = Bool("checkPort"); Operate.SystemConfig.CheckPort_Value = Text("portValue"); Operate.SystemConfig.CheckHead = Bool("checkHead"); Operate.SystemConfig.CheckHead_Value = Text("headValue"); Operate.SystemConfig.CheckData = Bool("checkData"); Operate.SystemConfig.CheckData_Value = Text("dataValue"); Operate.SystemConfig.CheckLen = Bool("checkLen"); Operate.SystemConfig.CheckLength_Value = Text("lenValue"); Operate.SystemConfig.CheckType = Bool("checkType"); var f = Operate.SystemConfig.CheckType_Value; f.Send = Bool("send"); f.SendTo = Bool("sendTo"); f.Recv = Bool("recv"); f.RecvFrom = Bool("recvFrom"); f.WSASend = Bool("wsaSend"); f.WSASendTo = Bool("wsaSendTo"); f.WSARecv = Bool("wsaRecv"); f.WSARecvFrom = Bool("wsaRecvFrom"); f.TCP_Req = Bool("tcpReq"); f.TCP_Resp = Bool("tcpResp"); f.UDP_Req = Bool("udpReq"); f.UDP_Resp = Bool("udpResp"); Operate.SystemConfig.CheckType_Value = f; Operate.SystemConfig.SaveSystemConfig_ToDB();
                }
                else if (page == "remote")
                {
                    var error = Operate.SystemConfig.SaveRemoteSetting(Bool("enabled"), Text("ip"), Int("port", 88), Text("userName"), Text("password")); if (!string.IsNullOrEmpty(error)) throw new InvalidOperationException(error);
                }
                else if (page == "firewall")
                {
                    Operate.ProxyConfig.Proxy.EnableFireWall = Bool("enable"); Operate.ProxyConfig.Proxy.FireWall_AutoWhiteList_AuthSuccess = Bool("autoWhiteAuthOk"); Operate.ProxyConfig.Proxy.FireWall_AutoBlackList_UnSupport = Bool("autoBlackUnsupport"); Operate.ProxyConfig.Proxy.FireWall_AutoBlackList_AuthFail = Bool("autoBlackAuthFail"); Operate.ProxyConfig.Proxy.FireWall_AutoBlackList_Minutes = Int("autoBlackMinutes", 30); Operate.ProxyConfig.Proxy.FireWall_AutoClear_Expiry = Bool("autoClearExpiry"); Operate.SystemConfig.SaveProxyMode_ToDB();
                }
                else if (page == "hotkey")
                {
                    var keys = values["keys"] as JArray; if (keys != null) for (var i = 0; i < keys.Count && i < 12; i++) { if (!Operate.SystemConfig.RegisterHotkey_Shell(i + 1, (string)keys[i])) throw new InvalidOperationException("Invalid hotkey at index " + (i + 1)); } Operate.SystemConfig.SaveHotkeyType_Shell(Int("type", 0));
                }
                else throw new InvalidOperationException("Unsupported writable settings page: " + page);
                return new JObject { ["page"] = page, ["changed"] = true };
            })).ConfigureAwait(false);
        }

        private static async Task<JToken> SaveWpcServerAsync(JObject arguments)
        {
            var id = (string)arguments?["id"] ?? string.Empty;
            var enabled = (bool?)arguments?["enabled"];
            var name = (string)arguments?["name"] ?? string.Empty;
            var ip = (string)arguments?["ip"] ?? string.Empty;
            var port = (int?)arguments?["port"];
            var key = (string)arguments?["idempotencyKey"];
            if (!enabled.HasValue || !port.HasValue) throw new InvalidOperationException("enabled and port are required.");
            if (!string.IsNullOrEmpty(id) && !Guid.TryParse(id, out _)) throw new InvalidOperationException("WPC server id must be a GUID or an empty string for creation.");
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("wpc.server.save", key, arguments, out prior)) return prior;
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("wpc.server.save", key, arguments, (string.IsNullOrEmpty(id) ? "新增" : "更新") + " WPC 服务器“" + name.Trim() + "”。", () =>
            {
                var error = Operate.WPCConfig.ServerList.SaveServer_Shell(id, enabled.Value, name, ip, port.Value, (string)arguments?["forgotUrl"], (string)arguments?["registerUrl"], (string)arguments?["verifyUrl"]);
                if (!string.IsNullOrEmpty(error)) throw new InvalidOperationException(error);
                var server = string.IsNullOrEmpty(id) ? Operate.WPCConfig.ServerList.lstServerInfo[Operate.WPCConfig.ServerList.lstServerInfo.Count - 1] : Operate.WPCConfig.ServerList.FindServer_ById(id);
                return new JObject { ["id"] = server.SID.ToString().ToUpperInvariant(), ["created"] = string.IsNullOrEmpty(id), ["changed"] = true };
            })).ConfigureAwait(false);
        }

        private static async Task<JToken> SaveWpcServerRuleAsync(JObject arguments)
        {
            var serverId = (string)arguments?["serverId"];
            var id = (string)arguments?["id"] ?? string.Empty;
            var enabled = (bool?)arguments?["enabled"];
            var type = (int?)arguments?["type"];
            var action = (int?)arguments?["action"];
            var key = (string)arguments?["idempotencyKey"];
            if (!Guid.TryParse(serverId, out _) || !enabled.HasValue || !type.HasValue || !action.HasValue) throw new InvalidOperationException("serverId, enabled, type and action are required.");
            if (!string.IsNullOrEmpty(id) && !Guid.TryParse(id, out _)) throw new InvalidOperationException("WPC rule id must be a GUID or an empty string for creation.");
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("wpc.server.rule.save", key, arguments, out prior)) return prior;
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync("wpc.server.rule.save", key, arguments, (string.IsNullOrEmpty(id) ? "新增" : "更新") + " WPC 服务器规则。", () =>
            {
                var error = Operate.WPCConfig.ServerList.SaveRule_Shell(serverId, id, enabled.Value, type.Value, (string)arguments?["argument"], action.Value);
                if (!string.IsNullOrEmpty(error)) throw new InvalidOperationException(error);
                var server = Operate.WPCConfig.ServerList.FindServer_ById(serverId);
                var ruleId = string.IsNullOrEmpty(id) ? server.ServerRInfo[server.ServerRInfo.Count - 1].RID.ToString().ToUpperInvariant() : id;
                return new JObject { ["serverId"] = serverId, ["id"] = ruleId, ["created"] = string.IsNullOrEmpty(id), ["changed"] = true };
            })).ConfigureAwait(false);
        }

        private static async Task<JToken> SaveRemoteManagementAsync(JObject arguments)
        {
            var enabledToken = arguments?["enabled"];
            var ip = ((string)arguments?["ip"] ?? string.Empty).Trim();
            var portToken = arguments?["port"];
            var userName = (string)arguments?["userName"] ?? string.Empty;
            var password = (string)arguments?["password"] ?? string.Empty;
            var idempotencyKey = (string)arguments?["idempotencyKey"];
            if (enabledToken == null || enabledToken.Type != JTokenType.Boolean) throw new InvalidOperationException("A boolean enabled value is required.");
            if (string.IsNullOrEmpty(ip)) throw new InvalidOperationException("A remote-management IP address is required.");
            if (portToken == null || portToken.Type != JTokenType.Integer) throw new InvalidOperationException("A remote-management port is required.");
            var port = portToken.Value<int>();
            if (port < 1 || port > 65535) throw new InvalidOperationException("Remote-management port must be from 1 to 65535.");
            if (enabledToken.Value<bool>() && string.IsNullOrWhiteSpace(userName)) throw new InvalidOperationException("An administrator user name is required when enabling remote management.");
            if (enabledToken.Value<bool>() && string.IsNullOrWhiteSpace(password)) throw new InvalidOperationException("An administrator password is required when enabling remote management.");

            JObject prior;
            if (McpWriteGuard.TryGetCompleted("remoteManagement.save", idempotencyKey, arguments, out prior)) return prior;
            var summary = (enabledToken.Value<bool>() ? "启用" : "停用") + "远程管理，监听 " + ip + ":" + port + "，管理员账号为“" + userName.Trim() + "”。";
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync(
                "remoteManagement.save", idempotencyKey, arguments, summary,
                () =>
                {
                    var error = Operate.SystemConfig.SaveRemoteSetting(enabledToken.Value<bool>(), ip, port, userName, password);
                    var setting = GetRemoteManagement();
                    setting["changed"] = true;
                    setting["error"] = string.IsNullOrEmpty(error) ? null : error;
                    return setting;
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

        private static async Task<JToken> StopAllExecutorsAsync(JObject arguments)
        {
            var idempotencyKey = (string)arguments?["idempotencyKey"];
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("executors.stopAll", idempotencyKey, arguments, out prior)) return prior;
            var before = ReadOnUi(() => new { sendRunning = Operate.SendConfig.List.SendExecute_Count(), robotRunning = Operate.RobotConfig.List.RobotExecute_Count() });
            return await InvokeOnUiAsync(() => McpWriteGuard.ApproveAndApplyAsync(
                "executors.stopAll", idempotencyKey, arguments,
                "停止全部发送器和机器人执行器；不会启动任务或主动发包。",
                () =>
                {
                    Operate.SendConfig.List.StopSendList();
                    Operate.RobotConfig.List.StopRobotList();
                    var after = new JObject
                    {
                        ["changed"] = before.sendRunning > 0 || before.robotRunning > 0,
                        ["sendRunning"] = Operate.SendConfig.List.SendExecute_Count(),
                        ["robotRunning"] = Operate.RobotConfig.List.RobotExecute_Count(),
                        ["stoppedSend"] = before.sendRunning,
                        ["stoppedRobot"] = before.robotRunning
                    };
                    return after;
                })).ConfigureAwait(false);
        }

        private static async Task<JToken> SelectStartModeAsync(JObject arguments)
        {
            var mode = ((string)arguments?["mode"] ?? string.Empty).Trim().ToLowerInvariant();
            if (mode != "proxy" && mode != "inject") throw new InvalidOperationException("Mode must be proxy or inject.");
            var idempotencyKey = (string)arguments?["idempotencyKey"];
            JObject prior;
            if (McpWriteGuard.TryGetCompleted("start.mode.select", idempotencyKey, arguments, out prior)) return prior;
            return await InvokeOnUiAsync<JToken>(async () =>
            {
                var selected = Operate.SystemConfig.SelectMode;
                if (selected != Operate.SystemConfig.SystemMode.None)
                {
                    var current = selected == Operate.SystemConfig.SystemMode.Proxy ? "proxy" : "inject";
                    // Repeating the desired startup action is normal when an AI retries after
                    // a delayed response. It is not an error and must not prompt again.
                    if (current == mode)
                    {
                        return (JToken)new JObject
                        {
                            ["changed"] = false,
                            ["mode"] = current,
                            ["screen"] = current,
                            ["proxyRunning"] = Operate.ProxyConfig.Proxy.IsRunning,
                            ["outcome"] = "alreadySelected"
                        };
                    }

                    // Mode selection is intentionally one-way. A caller on any non-start page
                    // receives a machine-readable unavailable result instead of a vague error.
                    return (JToken)new JObject
                    {
                        ["changed"] = false,
                        ["mode"] = current,
                        ["requestedMode"] = mode,
                        ["outcome"] = "unavailable",
                        ["reason"] = "mode_already_selected"
                    };
                }

                return await McpWriteGuard.ApproveAndApplyAsync(
                    "start.mode.select", idempotencyKey, arguments,
                    mode == "proxy" ? "进入 WPE 代理模式。不会自动启动代理监听。" : "进入 WPE 注入模式选择页。不会自动选择目标或执行注入。",
                    () =>
                    {
                        var request = StartModeRequested;
                        if (request == null) throw new InvalidOperationException("WPE start page is not ready.");
                        // The bridge event only asks the WebView to navigate. Commit the
                        // one-way mode selection before publishing it so a second MCP
                        // request cannot race the component's later enter*Mode callback.
                        Operate.SystemConfig.SelectMode = mode == "proxy"
                            ? Operate.SystemConfig.SystemMode.Proxy
                            : Operate.SystemConfig.SystemMode.Inject;
                        request(mode);
                        return new JObject
                        {
                            ["changed"] = true,
                            ["mode"] = mode,
                            ["screen"] = mode,
                            ["proxyRunning"] = Operate.ProxyConfig.Proxy.IsRunning,
                            ["outcome"] = "approved"
                        };
                    });
            }).ConfigureAwait(false);
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
            // Keep the MCP result aligned with WPE's Encoding Conversion page: one
            // input produces its complete native conversion table, not a separate
            // MCP-only UTF-8/Base64 conversion.
            return new JObject { ["rows"] = JArray.FromObject(Operate.SystemConfig.Transcode(text, decode)) };
        }

        private static JObject BytesCompare(JObject arguments)
        {
            var left = (string)arguments?["left"] ?? string.Empty;
            var right = (string)arguments?["right"] ?? string.Empty;
            if (left.Length > 1024 * 1024 || right.Length > 1024 * 1024) throw new InvalidOperationException("Input exceeds 1 MiB.");
            var minimumRun = Math.Max(1, Math.Min(4096, (int?)arguments?["minimumRun"] ?? 2));
            return new JObject { ["rows"] = JArray.FromObject(Operate.SystemConfig.FindDuplicates(left, right, minimumRun)) };
        }

        private static JObject BytesExtract(JObject arguments)
        {
            var kind = (int?)arguments?["kind"] ?? -1;
            var content = (string)arguments?["contentBase64"] ?? string.Empty;
            var bytes = Convert.FromBase64String(content);
            if (bytes.Length > 4 * 1024 * 1024) throw new InvalidOperationException("Content exceeds 4 MiB.");
            if (kind < 0 || kind > 32) throw new InvalidOperationException("Unsupported extraction kind.");
            return JObject.FromObject(Operate.SystemConfig.ExtractData(kind, bytes));
        }

        private sealed class CaptureSearchSnapshot
        {
            public readonly long Id; public readonly DateTime Time; public readonly int Type; public readonly int Length; public readonly string Preview; public readonly string From; public readonly string To; public readonly string Domain; public readonly int Action;
            public CaptureSearchSnapshot(long id, DateTime time, int type, int length, string preview, string from, string to, string domain, int action)
            { Id = id; Time = time; Type = type; Length = length; Preview = preview; From = from; To = to; Domain = domain; Action = action; }
        }

        private sealed class PacketSnapshot
        {
            public readonly byte[] Bytes; public readonly byte[] Raw;
            public PacketSnapshot(byte[] bytes, byte[] raw) { Bytes = bytes; Raw = raw; }
        }

        private static T ReadOnUi<T>(Func<T> action)
        {
            // WebUi.OnUi is WPE's established synchronous UI-boundary used by
            // remote management. Reusing it avoids a second, subtly different
            // invoke/wait protocol for MCP reads.
            return WebUi.OnUi(action);
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
                        var entryPid = (int?)entries[i]["processId"];
                        if (entryPid == processId || !IsProcessAlive(entryPid)) entries.RemoveAt(i);
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
                    if (!add && entries.Count == 0)
                    {
                        if (File.Exists(path)) File.Delete(path);
                        return;
                    }
                    root["protocol"] = 1;
                    root["instances"] = entries;
                    File.WriteAllText(path, root.ToString(Formatting.None), Encoding.UTF8);
                }
                finally { mutex.ReleaseMutex(); }
            }
        }

        private static bool IsProcessAlive(int? pid)
        {
            if (!pid.HasValue || pid.Value <= 0) return false;
            try { using (Process.GetProcessById(pid.Value)) return true; }
            catch { return false; }
        }
    }
}
