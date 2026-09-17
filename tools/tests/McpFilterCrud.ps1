[CmdletBinding()]
param([switch]$IncludeGlobalOperations)

$ErrorActionPreference = 'Stop'
$exe = (Resolve-Path (Join-Path $PSScriptRoot '..\..\WPEMcpServer\bin\Release\net10.0\WPEMcpServer.exe')).Path
$psi = [Diagnostics.ProcessStartInfo]::new($exe)
$psi.UseShellExecute = $false; $psi.RedirectStandardInput = $true; $psi.RedirectStandardOutput = $true; $psi.RedirectStandardError = $true
$p = [Diagnostics.Process]::new(); $p.StartInfo = $psi; [void]$p.Start()
$i = $p.StandardInput; $o = $p.StandardOutput
function Send($x) { $i.WriteLine(($x | ConvertTo-Json -Compress -Depth 40)); $i.Flush() }
function ReadResponse() {
    while ($true) {
        $line = $o.ReadLine(); if ($null -eq $line) { throw 'Sidecar closed stdout.' }
        if ([string]::IsNullOrWhiteSpace($line)) { continue }
        try { $msg = $line | ConvertFrom-Json } catch { continue }
        if ($null -ne $msg.id) { return $msg }
    }
}
function Call([int]$id, [string]$name, $toolArguments) {
    Send @{ jsonrpc='2.0'; id=$id; method='tools/call'; params=@{ name=$name; arguments=$toolArguments } }
    $msg = ReadResponse
    if ($msg.error) { throw "$name failed: $($msg.error.message)" }
    if ($msg.result.isError) { throw "$name returned an MCP error: $($msg.result.content[0].text)" }
    return ($msg.result.content[0].text | ConvertFrom-Json)
}
try {
    Send @{ jsonrpc='2.0'; id=1; method='initialize'; params=@{ protocolVersion='2025-06-18'; capabilities=@{}; clientInfo=@{ name='McpFilterCrud'; version='1.0' } } }; $null = ReadResponse
    Send @{ jsonrpc='2.0'; method='notifications/initialized'; params=@{} }
    $created = Call 2 'wpe_filter_create' @{ idempotencyKey = ([guid]::NewGuid().ToString()) }
    $id = $created.id; if ([string]::IsNullOrWhiteSpace($id)) { throw 'Create returned no filter id.' }
    $filter = Call 3 'wpe_filter_get' @{ id = $id }
    $filter.Name = 'MCP CRUD E2E'
    $updated = Call 4 'wpe_filter_update' @{ filter = $filter; idempotencyKey = ([guid]::NewGuid().ToString()) }
    $verified = Call 5 'wpe_filter_get' @{ id = $id }
    if ($verified.Name -ne 'MCP CRUD E2E') { throw "Update verification failed: $($verified.Name)" }
    $beforeCopy = Call 6 'wpe_filters_list' @{ limit = 200 }
    $copied = Call 7 'wpe_filters_copy' @{ ids = @($id); idempotencyKey = ([guid]::NewGuid().ToString()) }
    if (-not $copied.changed -or $copied.count -ne 1) { throw 'Copy verification failed.' }
    $afterCopy = Call 8 'wpe_filters_list' @{ limit = 200 }
    $copyId = @($afterCopy.rows | Where-Object { $_.Id -notin @($beforeCopy.rows.Id) } | Select-Object -First 1).Id
    if ([string]::IsNullOrWhiteSpace($copyId)) { throw 'Copied filter is absent from list.' }
    $moved = Call 9 'wpe_filters_move' @{ ids = @($copyId); direction = 'top'; idempotencyKey = ([guid]::NewGuid().ToString()) }
    if (-not $moved.changed) { throw 'Move verification failed.' }
    if ($IncludeGlobalOperations) {
        $reset = Call 10 'wpe_filter_counts_reset' @{ idempotencyKey = ([guid]::NewGuid().ToString()) }
        if ($reset.count -lt 2) { throw 'Count reset did not include test filters.' }
    }
    $deletedCopy = Call 11 'wpe_filter_delete' @{ id = $copyId; idempotencyKey = ([guid]::NewGuid().ToString()) }
    if (-not $deletedCopy.changed) { throw 'Copied filter deletion failed.' }
    $deleted = Call 12 'wpe_filter_delete' @{ id = $id; idempotencyKey = ([guid]::NewGuid().ToString()) }
    if (-not $deleted.changed) { throw 'Delete returned changed=false.' }
    Write-Host "MCP filter CRUD: PASS ($id)" -ForegroundColor Green
}
finally { $i.Close(); if (-not $p.HasExited) { try { $p.Kill() } catch {} }; $p.Dispose() }
