[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [bool]$Enabled,
    [string]$Server
)

$ErrorActionPreference = 'Stop'
if ([string]::IsNullOrWhiteSpace($Server)) {
    $Server = Join-Path $PSScriptRoot '..\..\WPEMcpServer\bin\Release\net10.0\WPEMcpServer.exe'
}
$Server = (Resolve-Path $Server).Path

$psi = [Diagnostics.ProcessStartInfo]::new($Server)
$psi.UseShellExecute = $false
$psi.RedirectStandardInput = $true
$psi.RedirectStandardOutput = $true
$psi.RedirectStandardError = $true
$process = [Diagnostics.Process]::new()
$process.StartInfo = $psi
[void]$process.Start()

function Send-Message($writer, $message) {
    $writer.WriteLine(($message | ConvertTo-Json -Compress -Depth 10))
    $writer.Flush()
}

function Read-Response($reader) {
    while ($true) {
        $line = $reader.ReadLine()
        if ($null -eq $line) { throw 'MCP Sidecar closed stdout unexpectedly.' }
        if ([string]::IsNullOrWhiteSpace($line)) { continue }
        try { $message = $line | ConvertFrom-Json } catch { continue }
        if ($null -ne $message.id) { return $message }
    }
}

try {
    $stdin = $process.StandardInput
    $stdout = $process.StandardOutput
    Send-Message $stdin @{ jsonrpc = '2.0'; id = 1; method = 'initialize'; params = @{ protocolVersion = '2025-06-18'; capabilities = @{}; clientInfo = @{ name = 'McpProxyAuthToggle'; version = '1.0' } } }
    $initialize = Read-Response $stdout
    if ($initialize.error) { throw "MCP initialize failed: $($initialize.error.message)" }
    Send-Message $stdin @{ jsonrpc = '2.0'; method = 'notifications/initialized'; params = @{} }

    Send-Message $stdin @{ jsonrpc = '2.0'; id = 2; method = 'tools/call'; params = @{ name = 'wpe_proxy_auth_set_enabled'; arguments = @{ enabled = $Enabled; idempotencyKey = [guid]::NewGuid().ToString() } } }
    $result = (Read-Response $stdout).result
    if ($result.isError) { throw "wpe_proxy_auth_set_enabled returned an MCP error: $($result.content[0].text)" }
    $body = $result.content[0].text | ConvertFrom-Json
    if ($body.outcome -ne 'approved' -or $body.enabled -ne $Enabled) { throw 'Authentication setting did not return the requested approved state.' }
    Write-Host "MCP proxy authentication: PASS (enabled=$Enabled)." -ForegroundColor Green
}
finally {
    if ($null -ne $process) {
        if (-not $process.HasExited) { try { $process.Kill() } catch {} }
        $process.Dispose()
    }
}
