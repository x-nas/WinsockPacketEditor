using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);
// MCP stdio reserves stdout for JSON-RPC frames. Do not let host diagnostics corrupt it.
builder.Logging.ClearProviders();
builder.Services.AddSingleton<WpeGatewayClient>();
builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithToolsFromAssembly();

await builder.Build().RunAsync();
