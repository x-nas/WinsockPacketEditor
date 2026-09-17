[CmdletBinding()]
param(
    [string]$Server
)

$ErrorActionPreference = 'Stop'
if ([string]::IsNullOrWhiteSpace($Server)) { $Server = Join-Path $PSScriptRoot '..\..\WPEMcpServer\bin\Release\net10.0\WPEMcpServer.exe' }
$Server = (Resolve-Path $Server).Path
$psi = [Diagnostics.ProcessStartInfo]::new($Server)
$psi.UseShellExecute = $false; $psi.RedirectStandardInput = $true; $psi.RedirectStandardOutput = $true
$process = [Diagnostics.Process]::new(); $process.StartInfo = $psi; [void]$process.Start()

function Send-Message($writer, $message) { $writer.WriteLine(($message | ConvertTo-Json -Compress -Depth 20)); $writer.Flush() }
function Read-Response($reader) {
    while ($true) {
        $line = $reader.ReadLine(); if ($null -eq $line) { throw 'MCP Sidecar closed stdout unexpectedly.' }
        if ([string]::IsNullOrWhiteSpace($line)) { continue }
        try { $message = $line | ConvertFrom-Json } catch { continue }
        if ($null -ne $message.id) { return $message }
    }
}
function Call-Tool($writer, $reader, [int]$id, [string]$name, $toolArguments) {
    Send-Message $writer @{ jsonrpc = '2.0'; id = $id; method = 'tools/call'; params = @{ name = $name; arguments = $toolArguments } }
    $message = Read-Response $reader
    if ($message.error) { throw "$name failed: $($message.error.message)" }
    return $message.result
}

try {
    $stdin = $process.StandardInput; $stdout = $process.StandardOutput
    Send-Message $stdin @{ jsonrpc = '2.0'; id = 1; method = 'initialize'; params = @{ protocolVersion = '2025-06-18'; capabilities = @{}; clientInfo = @{ name = 'McpStartMode'; version = '1.0' } } }
    $initialize = Read-Response $stdout; if ($initialize.error) { throw "MCP initialize failed: $($initialize.error.message)" }
    Send-Message $stdin @{ jsonrpc = '2.0'; method = 'notifications/initialized'; params = @{} }

    $key = [guid]::NewGuid().ToString()
    $firstResult = Call-Tool $stdin $stdout 2 'wpe_start_mode_select' @{ mode = 'proxy'; idempotencyKey = $key }
    if ($firstResult.isError) { throw 'Initial start-mode selection returned an MCP error.' }
    $first = $firstResult.content[0].text | ConvertFrom-Json
    if ($first.outcome -ne 'approved' -or -not $first.changed -or $first.mode -ne 'proxy') { throw 'Initial start-mode selection returned an unexpected result.' }

    $repeatResult = Call-Tool $stdin $stdout 3 'wpe_start_mode_select' @{ mode = 'proxy'; idempotencyKey = $key }
    if ($repeatResult.isError) { throw 'Repeated start-mode idempotency key returned an MCP error.' }
    $repeat = $repeatResult.content[0].text | ConvertFrom-Json
    if ($repeat.requestHash -ne $first.requestHash -or $repeat.mode -ne 'proxy' -or $repeat.changed -ne $first.changed) { throw 'Repeated start-mode idempotency key did not return the original result.' }

    $afterLeave = Call-Tool $stdin $stdout 4 'wpe_start_mode_select' @{ mode = 'inject'; idempotencyKey = ([guid]::NewGuid().ToString()) }
    if (-not $afterLeave.isError) { throw 'A new start-mode selection was accepted after leaving the start page.' }

    Write-Host 'MCP start-mode: PASS (selection, idempotency and post-selection rejection).' -ForegroundColor Green
}
finally {
    if ($null -ne $process) { if (-not $process.HasExited) { try { $process.Kill() } catch {} }; $process.Dispose() }
}
