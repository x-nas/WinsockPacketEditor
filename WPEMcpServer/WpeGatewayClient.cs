using System.IO.Pipes;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

internal sealed class WpeGatewayClient
{
    // Matches WPE's local pipe allocation guard. Full packet data is intentionally
    // available to the current-user MCP caller.
    private const int MaxFrameBytes = 128 * 1024 * 1024;
    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public async Task<string> InvokeAsync(string operation, object? arguments, CancellationToken cancellationToken)
    {
        var instance = await WpeInstanceDiscovery.FindOneAsync(cancellationToken);
        using var pipe = new NamedPipeClientStream(".", instance.PipeName, PipeDirection.InOut,
            PipeOptions.Asynchronous, TokenImpersonationLevel.Identification);
        await pipe.ConnectAsync(3000, cancellationToken);

        var request = JsonSerializer.SerializeToUtf8Bytes(new GatewayRequest(Guid.NewGuid().ToString("N"), operation, EnsureIdempotencyKey(arguments)), JsonOptions);
        await WriteFrameAsync(pipe, request, cancellationToken);
        using var response = JsonDocument.Parse(await ReadFrameAsync(pipe, cancellationToken));
        var root = response.RootElement;
        if (!root.TryGetProperty("ok", out var ok) || !ok.GetBoolean())
        {
            var error = root.TryGetProperty("error", out var value) ? value.GetString() : "WPE gateway rejected the request.";
            throw new InvalidOperationException(error);
        }

        return root.GetProperty("result").GetRawText();
    }

    /*
       MCP 客户端常会把模型生成的写入参数原样转发；有些客户端不会替必填 UUID
       生成值，导致本来可安全执行的写入先失败一次。仅在工具声明了
       idempotencyKey、但该值为空时补一个 UUID。部分客户端会将人类可读的
       请求标签（例如 create-wpc-test1）误填入该字段；这类标签会稳定地派生为
       UUID，而不是让一次本可执行的写入失败。

       WPE 侧仍以 UUID 做请求去重与审计。相同标签会得到相同 UUID，因此跨进程
       重试仍然稳定；调用者也可以显式传入自己的 UUID。
    */
    private static object? EnsureIdempotencyKey(object? arguments)
    {
        if (arguments == null) return null;
        var node = JsonSerializer.SerializeToNode(arguments, JsonOptions) as JsonObject;
        if (node == null) return arguments;
        JsonNode? key;
        if (!node.TryGetPropertyValue("idempotencyKey", out key)) return node;
        var text = key == null ? string.Empty : key.ToString().Trim();
        if (string.IsNullOrEmpty(text)) node["idempotencyKey"] = Guid.NewGuid().ToString("D");
        else if (!Guid.TryParse(text, out _)) node["idempotencyKey"] = StableGuid(text).ToString("D");
        return node;
    }

    private static Guid StableGuid(string text)
    {
        using var sha256 = SHA256.Create();
        var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(text));
        var guidBytes = new byte[16];
        Buffer.BlockCopy(bytes, 0, guidBytes, 0, guidBytes.Length);
        // RFC 4122 variant and a deterministic version marker.
        guidBytes[6] = (byte)((guidBytes[6] & 0x0F) | 0x50);
        guidBytes[8] = (byte)((guidBytes[8] & 0x3F) | 0x80);
        return new Guid(guidBytes);
    }

    private static async Task WriteFrameAsync(Stream stream, byte[] payload, CancellationToken cancellationToken)
    {
        if (payload.Length > MaxFrameBytes) throw new InvalidOperationException("Gateway request exceeds the protocol size limit.");
        await stream.WriteAsync(BitConverter.GetBytes(payload.Length), cancellationToken);
        await stream.WriteAsync(payload, cancellationToken);
        await stream.FlushAsync(cancellationToken);
    }

    private static async Task<byte[]> ReadFrameAsync(Stream stream, CancellationToken cancellationToken)
    {
        var header = await ReadExactlyAsync(stream, sizeof(int), cancellationToken);
        var length = BitConverter.ToInt32(header, 0);
        if (length <= 0 || length > MaxFrameBytes) throw new InvalidOperationException("Invalid gateway frame length.");
        return await ReadExactlyAsync(stream, length, cancellationToken);
    }

    private static async Task<byte[]> ReadExactlyAsync(Stream stream, int length, CancellationToken cancellationToken)
    {
        var buffer = new byte[length];
        var offset = 0;
        while (offset < length)
        {
            var read = await stream.ReadAsync(buffer.AsMemory(offset, length - offset), cancellationToken);
            if (read == 0) throw new EndOfStreamException("WPE gateway closed its pipe.");
            offset += read;
        }
        return buffer;
    }

    private sealed record GatewayRequest(string RequestId, string Operation, object? Arguments);
}

internal sealed record WpeInstance(string PipeName, int ProcessId, string StartedUtc);

internal static class WpeInstanceDiscovery
{
    public static async Task<WpeInstance> FindOneAsync(CancellationToken cancellationToken)
    {
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "WPE64", "mcp", "instances.json");
        if (!File.Exists(path)) throw new InvalidOperationException("No running WPE MCP gateway was found. Start WPE x64 first.");
        await using var stream = File.OpenRead(path);
        var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);
        using (document)
        {
            if (!document.RootElement.TryGetProperty("instances", out var instances) || instances.GetArrayLength() != 1)
                throw new InvalidOperationException("Exactly one running WPE instance is required for this MCP connection.");
            var item = instances[0];
            return new WpeInstance(item.GetProperty("pipeName").GetString()!, item.GetProperty("processId").GetInt32(), item.GetProperty("startedUtc").GetString()!);
        }
    }
}
