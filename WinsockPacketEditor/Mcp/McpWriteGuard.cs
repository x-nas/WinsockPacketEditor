using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace WinsockPacketEditor.Mcp
{
    /// <summary>Shared Phase-2 gate. Callers must already be executing on WPE's UI thread.</summary>
    internal static class McpWriteGuard
    {
        private static readonly ConcurrentDictionary<string, JObject> Completed = new ConcurrentDictionary<string, JObject>();
        private static readonly ConcurrentDictionary<string, Lazy<Task<JObject>>> InFlight = new ConcurrentDictionary<string, Lazy<Task<JObject>>>();
        private static readonly ConcurrentDictionary<string, string> RequestHashes = new ConcurrentDictionary<string, string>();
        private static readonly ConcurrentQueue<string> CompletedOrder = new ConcurrentQueue<string>();
        private static readonly ConcurrentQueue<JObject> Audit = new ConcurrentQueue<JObject>();
        private const int AuditLimit = 1000;
        private const int IdempotencyCacheLimit = 4096;

        public static bool TryGetCompleted(string operation, string idempotencyKey, JObject arguments, out JObject result)
        {
            if (!Guid.TryParse(idempotencyKey, out _)) throw new InvalidOperationException("A UUID idempotencyKey is required.");
            var requestKey = operation + "\n" + idempotencyKey;
            var hash = Hash(operation + "\n" + Canonicalize(arguments ?? new JObject()));
            string registeredHash;
            if (RequestHashes.TryGetValue(requestKey, out registeredHash) && !string.Equals(registeredHash, hash, StringComparison.Ordinal))
            {
                throw new InvalidOperationException("The idempotencyKey was already used with different arguments.");
            }

            JObject prior;
            if (Completed.TryGetValue(requestKey, out prior))
            {
                result = (JObject)prior.DeepClone();
                return true;
            }

            result = null;
            return false;
        }

        public static async Task<JObject> ApproveAndApplyAsync(string operation, string idempotencyKey, JObject arguments, string summary, Func<JObject> apply)
        {
            return await ApproveAndApplyAsync(operation, idempotencyKey, arguments, summary, () => Task.FromResult(apply()));
        }

        public static async Task<JObject> ApproveAndApplyAsync(string operation, string idempotencyKey, JObject arguments, string summary, Func<Task<JObject>> apply)
        {
            if (!Guid.TryParse(idempotencyKey, out _)) throw new InvalidOperationException("A UUID idempotencyKey is required.");
            var requestKey = operation + "\n" + idempotencyKey;
            var hash = Hash(operation + "\n" + Canonicalize(arguments ?? new JObject()));
            var registeredHash = RequestHashes.GetOrAdd(requestKey, hash);
            if (!string.Equals(registeredHash, hash, StringComparison.Ordinal))
            {
                throw new InvalidOperationException("The idempotencyKey was already used with different arguments.");
            }
            JObject prior;
            if (Completed.TryGetValue(requestKey, out prior)) return (JObject)prior.DeepClone();

            var pending = InFlight.GetOrAdd(requestKey, _ => new Lazy<Task<JObject>>(
                () => ApproveCoreAsync(operation, idempotencyKey, requestKey, hash, arguments, summary, apply),
                LazyThreadSafetyMode.ExecutionAndPublication));
            try { return (JObject)(await pending.Value).DeepClone(); }
            finally
            {
                if (pending.IsValueCreated && pending.Value.IsCompleted) InFlight.TryRemove(requestKey, out _);
            }
        }

        private static async Task<JObject> ApproveCoreAsync(string operation, string idempotencyKey, string requestKey, string hash, JObject arguments, string summary, Func<Task<JObject>> apply)
        {
            if (!Operate.SystemConfig.McpRequiresConfirmation)
            {
                JObject automatic;
                try { automatic = await apply(); }
                catch (Exception ex) { Record(operation, idempotencyKey, hash, "failed", arguments, new JObject { ["error"] = ex.Message }); throw; }
                // Keep the public result contract stable; the audit record distinguishes autoApproved.
                automatic["outcome"] = "approved";
                automatic["requestHash"] = hash;
                StoreCompleted(requestKey, automatic);
                Record(operation, idempotencyKey, hash, "autoApproved", arguments, automatic);
                return automatic;
            }
            var confirmation = UI.Confirm(UI.T("Mcp.Write.Title", "MCP 写入请求"), operation + "\r\n\r\n" + summary + "\r\n\r\n请求摘要: " + hash.Substring(0, 12));
            if (await Task.WhenAny(confirmation, Task.Delay(TimeSpan.FromSeconds(60))) != confirmation)
            {
                var expired = new JObject { ["outcome"] = "expired", ["requestHash"] = hash };
                StoreCompleted(requestKey, expired);
                Record(operation, idempotencyKey, hash, "expired", arguments, null);
                return expired;
            }
            var approved = await confirmation;
            if (!approved)
            {
                var rejected = new JObject { ["outcome"] = "rejected", ["requestHash"] = hash };
                StoreCompleted(requestKey, rejected);
                Record(operation, idempotencyKey, hash, "rejected", arguments, null);
                return rejected;
            }

            JObject result;
            try { result = await apply(); }
            catch (Exception ex) { Record(operation, idempotencyKey, hash, "failed", arguments, new JObject { ["error"] = ex.Message }); throw; }
            result["outcome"] = "approved";
            result["requestHash"] = hash;
            StoreCompleted(requestKey, result);
            Record(operation, idempotencyKey, hash, "approved", arguments, result);
            return result;
        }

        private static void Record(string operation, string key, string hash, string outcome, JObject arguments, JObject result)
        {
            // MCP is a local, current-user boundary: the caller is the WPE operator.
            // Keep the original request and result in the audit record so the caller can
            // inspect credentials, tokens and packet bytes exactly as supplied/returned.
            Audit.Enqueue(new JObject { ["time"] = DateTime.UtcNow.ToString("o"), ["operation"] = operation, ["idempotencyKey"] = key, ["requestHash"] = hash, ["outcome"] = outcome, ["arguments"] = (arguments ?? new JObject()).DeepClone(), ["result"] = (result ?? new JObject()).DeepClone() });
            while (Audit.Count > AuditLimit) Audit.TryDequeue(out _);
        }

        private static void StoreCompleted(string requestKey, JObject result)
        {
            if (!Completed.TryAdd(requestKey, (JObject)result.DeepClone())) return;
            CompletedOrder.Enqueue(requestKey);
            while (Completed.Count > IdempotencyCacheLimit && CompletedOrder.TryDequeue(out var oldest))
            {
                Completed.TryRemove(oldest, out _);
                RequestHashes.TryRemove(oldest, out _);
            }
        }

        private static string Canonicalize(JToken value)
        {
            if (value.Type == JTokenType.Object)
            {
                var result = new JObject();
                var properties = new List<JProperty>(((JObject)value).Properties());
                properties.Sort((left, right) => string.CompareOrdinal(left.Name, right.Name));
                foreach (var property in properties) result[property.Name] = JToken.Parse(Canonicalize(property.Value));
                return result.ToString(Newtonsoft.Json.Formatting.None);
            }
            if (value.Type == JTokenType.Array)
            {
                var result = new JArray();
                foreach (var item in value.Children()) result.Add(JToken.Parse(Canonicalize(item)));
                return result.ToString(Newtonsoft.Json.Formatting.None);
            }
            return value.ToString(Newtonsoft.Json.Formatting.None);
        }

        private static string Hash(string text)
        {
            using (var sha = SHA256.Create()) return BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-", string.Empty).ToLowerInvariant();
        }
    }
}
