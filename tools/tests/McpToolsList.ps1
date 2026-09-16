[CmdletBinding()]
param(
    [string]$Server,
    [int]$ExpectedCount = 37
)

$ErrorActionPreference = 'Stop'
if ([string]::IsNullOrWhiteSpace($Server)) { $Server = Join-Path $PSScriptRoot '..\..\WPEMcpServer\bin\Release\net10.0\WPEMcpServer.exe' }
if (-not (Test-Path $Server)) { throw "MCP Sidecar not found: $Server" }

$psi = New-Object System.Diagnostics.ProcessStartInfo
$psi.FileName = (Resolve-Path $Server).Path
$psi.UseShellExecute = $false
$psi.CreateNoWindow = $true
$psi.RedirectStandardInput = $true
$psi.RedirectStandardOutput = $true
$psi.RedirectStandardError = $true
$process = New-Object System.Diagnostics.Process
$process.StartInfo = $psi
[void]$process.Start()

function Send-Message([System.IO.StreamWriter]$writer, [object]$message) {
    $writer.WriteLine(($message | ConvertTo-Json -Compress -Depth 20))
    $writer.Flush()
}

function Read-Message([System.IO.StreamReader]$reader) {
    $readTask = $reader.ReadLineAsync()
    if (-not $readTask.Wait(5000)) { throw 'Timed out waiting for MCP Sidecar response.' }
    if ($null -eq $readTask.Result) { throw 'MCP Sidecar closed stdout unexpectedly.' }
    return $readTask.Result | ConvertFrom-Json
}

try {
    $input = $process.StandardInput
    $output = $process.StandardOutput
    Send-Message $input @{ jsonrpc = '2.0'; id = 1; method = 'initialize'; params = @{ protocolVersion = '2025-06-18'; capabilities = @{}; clientInfo = @{ name = 'WPEMcpContractCheck'; version = '1.0' } } }
    $initialize = Read-Message $output
    if ($initialize.error) { throw "MCP initialize failed: $($initialize.error.message)" }
    Send-Message $input @{ jsonrpc = '2.0'; method = 'notifications/initialized'; params = @{} }
    Send-Message $input @{ jsonrpc = '2.0'; id = 2; method = 'tools/list'; params = @{} }
    $list = Read-Message $output
    if ($list.error) { throw "MCP tools/list failed: $($list.error.message)" }
    $tools = @($list.result.tools)
    $names = @($tools | ForEach-Object { $_.name })
    if ($names.Count -ne $ExpectedCount) { throw "Expected $ExpectedCount MCP tools, got $($names.Count)." }
    if (($names | Sort-Object -Unique).Count -ne $names.Count) { throw 'MCP tools/list contains duplicate names.' }
    if (@($names | Where-Object { $_ -notmatch '^wpe_[a-z0-9_]+$' }).Count -gt 0) { throw 'MCP tools/list contains an invalid tool name.' }
    Write-Host "MCP tools/list: PASS ($($names.Count) tools)."
}
finally {
    if (-not $process.HasExited) { $process.Kill() }
    $process.Dispose()
}
