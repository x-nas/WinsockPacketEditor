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
function Key { [guid]::NewGuid().ToString() }
try {
    Send @{ jsonrpc='2.0'; id=1; method='initialize'; params=@{ protocolVersion='2025-06-18'; capabilities=@{}; clientInfo=@{ name='McpTaskWarehouseWrite'; version='1.0' } } }; $null = ReadResponse
    Send @{ jsonrpc='2.0'; method='notifications/initialized'; params=@{} }
    $send = Call 2 'wpe_send_create' @{ idempotencyKey=Key }; $robot = Call 3 'wpe_robot_create' @{ idempotencyKey=Key }; $warehouse = Call 4 'wpe_warehouse_create' @{ idempotencyKey=Key }
    foreach ($item in @($send, $robot, $warehouse)) { if (-not $item.changed -or [string]::IsNullOrEmpty($item.id)) { throw 'Native task creation failed.' } }
    $inject = @(Call 30 'wpe_capture_search' @{ mode='inject'; limit=1 }).rows | Select-Object -First 1
    if ($inject) { $null = Call 31 'wpe_capture_add_to_send' @{ sendId=$send.id; packetIds=@([long]$inject.id); idempotencyKey=Key }; $null = Call 32 'wpe_capture_add_to_warehouse' @{ warehouseId=$warehouse.id; packetIds=@([long]$inject.id); idempotencyKey=Key } }
    $proxy = @(Call 33 'wpe_capture_search' @{ mode='proxy'; limit=1 }).rows | Select-Object -First 1
    if ($proxy) { $null = Call 34 'wpe_proxy_capture_add_to_send' @{ sendId=$send.id; packetIds=@([long]$proxy.id); idempotencyKey=Key }; $null = Call 35 'wpe_proxy_capture_add_to_warehouse' @{ warehouseId=$warehouse.id; packetIds=@([long]$proxy.id); idempotencyKey=Key } }
    $null = Call 5 'wpe_send_update' @{ id=$send.id; name='MCP write-test send'; useSystemSocket=$false; loopCount=1; loopInterval=0; notes='temporary MCP regression task'; idempotencyKey=Key }
    $null = Call 6 'wpe_robot_update' @{ id=$robot.id; name='MCP write-test robot'; idempotencyKey=Key }
    $null = Call 7 'wpe_warehouse_update' @{ id=$warehouse.id; name='MCP write-test warehouse'; idempotencyKey=Key }
    $enabled = Call 8 'wpe_send_set_enabled' @{ id=$send.id; enabled=$true; idempotencyKey=Key }; if (-not $enabled.enabled) { throw 'Send enable did not apply.' }
    $enabled = Call 9 'wpe_robot_set_enabled' @{ id=$robot.id; enabled=$true; idempotencyKey=Key }; if (-not $enabled.enabled) { throw 'Robot enable did not apply.' }
    $copies = @(); foreach ($item in @(@{ kind='send'; id=$send.id; tool='wpe_sends_list'; copy='wpe_send_copy' }, @{ kind='robot'; id=$robot.id; tool='wpe_robots_list'; copy='wpe_robot_copy' }, @{ kind='warehouse'; id=$warehouse.id; tool='wpe_warehouse_list'; copy='wpe_warehouse_copy' })) { $before = @(Call (10 + $copies.Count * 2) $item.tool @{ limit=200 }).rows.Id; $copy = Call (11 + $copies.Count * 2) $item.copy @{ ids=@($item.id); idempotencyKey=Key }; $after = @(Call (12 + $copies.Count * 2) $item.tool @{ limit=200 }).rows.Id; $copyId = @($after | Where-Object { $_ -notin $before }) | Select-Object -First 1; if (-not $copy.changed -or [string]::IsNullOrEmpty($copyId)) { throw "Copy failed for $($item.kind)." }; $copies += @{ kind=$item.kind; id=$copyId } }
    $null = Call 20 'wpe_send_move' @{ ids=@($send.id); direction='top'; idempotencyKey=Key }
    $null = Call 21 'wpe_robot_move' @{ ids=@($robot.id); direction='top'; idempotencyKey=Key }
    $null = Call 22 'wpe_warehouse_move' @{ ids=@($warehouse.id); direction='top'; idempotencyKey=Key }
    $null = Call 23 'wpe_send_delete' @{ ids=@($send.id, $copies[0].id); idempotencyKey=Key }
    $null = Call 24 'wpe_robot_delete' @{ ids=@($robot.id, $copies[1].id); idempotencyKey=Key }
    $null = Call 25 'wpe_warehouse_delete' @{ ids=@($warehouse.id, $copies[2].id); idempotencyKey=Key }
    Write-Host 'MCP task and warehouse write tools: PASS' -ForegroundColor Green
}
finally { $i.Close(); if (-not $p.HasExited) { try { $p.Kill(); $p.WaitForExit() } catch {} }; $p.Dispose() }
