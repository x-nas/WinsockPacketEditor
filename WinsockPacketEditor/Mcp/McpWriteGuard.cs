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
        private static readonly ConcurrentQueue<JObject> Audit = new ConcurrentQueue<JObject>();
        private const int AuditLimit = 1000;

        public static bool TryGetCompleted(string operation, string idempotencyKey, JObject arguments, out JObject result)
        {
            if (!Guid.TryParse(idempotencyKey, out _)) throw new InvalidOperationException("A UUID idempotencyKey is required.");
            var requestKey = operation + "\n" + idempotencyKey;
            var hash = Hash(operation + "\n" + (arguments ?? new JObject()).ToString(Newtonsoft.Json.Formatting.None));
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
            var hash = Hash(operation + "\n" + (arguments ?? new JObject()).ToString(Newtonsoft.Json.Formatting.None));
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
            if (Operate.SystemConfig.McpAutoApproveWrites)
            {
                var automatic = await apply();
                // Keep the public result contract stable; the audit record distinguishes autoApproved.
                automatic["outcome"] = "approved";
                automatic["requestHash"] = hash;
                Completed.TryAdd(requestKey, (JObject)automatic.DeepClone());
                Record(operation, idempotencyKey, hash, "autoApproved", arguments, automatic);
                return automatic;
            }
            var confirmation = UI.Confirm(UI.T("Mcp.Write.Title", "MCP 写入请求"), operation + "\r\n\r\n" + summary + "\r\n\r\n请求摘要: " + hash.Substring(0, 12));
            if (await Task.WhenAny(confirmation, Task.Delay(TimeSpan.FromSeconds(60))) != confirmation)
            {
                var expired = new JObject { ["outcome"] = "expired", ["requestHash"] = hash };
                Completed.TryAdd(requestKey, (JObject)expired.DeepClone());
                Record(operation, idempotencyKey, hash, "expired", arguments, null);
                return expired;
            }
            var approved = await confirmation;
            if (!approved)
            {
                var rejected = new JObject { ["outcome"] = "rejected", ["requestHash"] = hash };
                Completed.TryAdd(requestKey, (JObject)rejected.DeepClone());
                Record(operation, idempotencyKey, hash, "rejected", arguments, null);
                return rejected;
            }

            var result = await apply();
            result["outcome"] = "approved";
            result["requestHash"] = hash;
            Completed.TryAdd(requestKey, (JObject)result.DeepClone());
            Record(operation, idempotencyKey, hash, "approved", arguments, result);
            return result;
        }

        private static void Record(string operation, string key, string hash, string outcome, JObject arguments, JObject result)
        {
            Audit.Enqueue(new JObject { ["time"] = DateTime.UtcNow.ToString("o"), ["operation"] = operation, ["idempotencyKey"] = key, ["requestHash"] = hash, ["outcome"] = outcome, ["arguments"] = Redact(arguments), ["result"] = Redact(result) });
            while (Audit.Count > AuditLimit) Audit.TryDequeue(out _);
        }

        private static JObject Redact(JObject value)
        {
            var copy = value == null ? new JObject() : (JObject)value.DeepClone();
            foreach (var property in new List<JProperty>(copy.Properties()))
            {
                if (property.Name.IndexOf("password", StringComparison.OrdinalIgnoreCase) >= 0 || property.Name.IndexOf("token", StringComparison.OrdinalIgnoreCase) >= 0 || property.Name.IndexOf("payload", StringComparison.OrdinalIgnoreCase) >= 0) property.Value = "[redacted]";
            }
            return copy;
        }

        private static string Hash(string text)
        {
            using (var sha = SHA256.Create()) return BitConverter.ToString(sha.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-", string.Empty).ToLowerInvariant();
        }
    }
}
