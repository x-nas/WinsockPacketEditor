[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
$exe = (Resolve-Path (Join-Path $PSScriptRoot '..\..\WPEMcpServer\bin\Release\net10.0\WPEMcpServer.exe')).Path
$psi = [Diagnostics.ProcessStartInfo]::new($exe)
$psi.UseShellExecute = $false; $psi.RedirectStandardInput = $true; $psi.RedirectStandardOutput = $true; $psi.RedirectStandardError = $true
$p = [Diagnostics.Process]::new(); $p.StartInfo = $psi; [void]$p.Start(); $i = $p.StandardInput; $o = $p.StandardOutput
function Send($x) { $i.WriteLine(($x | ConvertTo-Json -Compress -Depth 40)); $i.Flush() }
function ReadResponse() { while ($true) { $line = $o.ReadLine(); if ($null -eq $line) { throw 'Sidecar closed stdout.' }; try { $msg = $line | ConvertFrom-Json } catch { continue }; if ($null -ne $msg.id) { return $msg } } }
function Call([int]$id, [string]$name, $arguments) { Send @{ jsonrpc='2.0'; id=$id; method='tools/call'; params=@{ name=$name; arguments=$arguments } }; $msg = ReadResponse; if ($msg.error -or $msg.result.isError) { throw "$name failed: $($msg.error.message)$($msg.result.content[0].text)" }; return ($msg.result.content[0].text | ConvertFrom-Json) }
try {
    Send @{ jsonrpc='2.0'; id=1; method='initialize'; params=@{ protocolVersion='2025-06-18'; capabilities=@{}; clientInfo=@{ name='McpTaskWarehouse'; version='1.0' } } }; $null = ReadResponse
    Send @{ jsonrpc='2.0'; method='notifications/initialized'; params=@{} }
    $sends = Call 2 'wpe_sends_list' @{ limit=50 }
    $robots = Call 3 'wpe_robots_list' @{ limit=50 }
    $warehouses = Call 4 'wpe_warehouse_list' @{ limit=50 }
    foreach ($result in @($sends, $robots, $warehouses)) { if ($null -eq $result.rows) { throw 'A task list omitted rows.' } }
    if (@($sends.rows).Count -gt 0) { $send = Call 5 'wpe_send_get' @{ id=$sends.rows[0].Id }; if ([string]::IsNullOrEmpty($send.Id)) { throw 'send_get omitted Id.' }; $collection = Call 6 'wpe_send_collection_list' @{ id=$send.Id; limit=50 }; if ($null -eq $collection.rows) { throw 'send_collection_list omitted rows.' } }
    if (@($robots.rows).Count -gt 0) { $robot = Call 7 'wpe_robot_get' @{ id=$robots.rows[0].Id }; if ([string]::IsNullOrEmpty($robot.id) -or $null -eq $robot.instructions) { throw 'robot_get response is incomplete.' } }
    if (@($warehouses.rows).Count -gt 0) { $warehouse = Call 8 'wpe_warehouse_get' @{ id=$warehouses.rows[0].Id; limit=50 }; if ([string]::IsNullOrEmpty($warehouse.id) -or $null -eq $warehouse.rows) { throw 'warehouse_get response is incomplete.' } }
    Write-Host 'MCP task and warehouse read tools: PASS' -ForegroundColor Green
}
finally { $i.Close(); if (-not $p.HasExited) { try { $p.Kill(); $p.WaitForExit() } catch {} }; $p.Dispose() }
