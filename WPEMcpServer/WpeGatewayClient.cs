using System.IO.Pipes;
using System.Security.Principal;
using System.Text;
using System.Text.Json;

internal sealed class WpeGatewayClient
{
    private const int MaxFrameBytes = 1024 * 1024;
    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

    public async Task<string> InvokeAsync(string operation, object? arguments, CancellationToken cancellationToken)
    {
        var instance = await WpeInstanceDiscovery.FindOneAsync(cancellationToken);
        using var pipe = new NamedPipeClientStream(".", instance.PipeName, PipeDirection.InOut,
            PipeOptions.Asynchronous, TokenImpersonationLevel.Identification);
        await pipe.ConnectAsync(3000, cancellationToken);

        var request = JsonSerializer.SerializeToUtf8Bytes(new GatewayRequest(Guid.NewGuid().ToString("N"), operation, arguments), JsonOptions);
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

    private static async Task WriteFrameAsync(Stream stream, byte[] payload, CancellationToken cancellationToken)
    {
        if (payload.Length > MaxFrameBytes) throw new InvalidOperationException("Gateway request exceeds 1 MiB.");
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
