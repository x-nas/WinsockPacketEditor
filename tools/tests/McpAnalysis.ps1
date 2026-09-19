$ErrorActionPreference = 'Stop'
$exe = (Resolve-Path (Join-Path $PSScriptRoot '..\..\WPEMcpServer\bin\Release\net10.0\WPEMcpServer.exe')).Path
$psi = [Diagnostics.ProcessStartInfo]::new($exe)
$psi.UseShellExecute = $false; $psi.RedirectStandardInput = $true; $psi.RedirectStandardOutput = $true; $psi.RedirectStandardError = $true
$p = [Diagnostics.Process]::new(); $p.StartInfo = $psi; [void]$p.Start(); $i = $p.StandardInput; $o = $p.StandardOutput
function Send($x) { $i.WriteLine(($x | ConvertTo-Json -Compress -Depth 40)); $i.Flush() }
function ReadResponse() { while ($true) { $line = $o.ReadLine(); if ($null -eq $line) { throw 'Sidecar closed stdout.' }; if ([string]::IsNullOrWhiteSpace($line)) { continue }; try { $msg = $line | ConvertFrom-Json } catch { continue }; if ($null -ne $msg.id) { return $msg } } }
function Call([int]$id, [string]$name, $toolArguments) { Send @{ jsonrpc='2.0'; id=$id; method='tools/call'; params=@{ name=$name; arguments=$toolArguments } }; $msg = ReadResponse; if ($msg.error) { throw "$name RPC failed: $($msg.error.message)" }; if ($msg.result.isError) { throw "$name failed: $($msg.result.content[0].text)" }; return ($msg.result.content[0].text | ConvertFrom-Json) }
try {
    Send @{ jsonrpc='2.0'; id=1; method='initialize'; params=@{ protocolVersion='2025-06-18'; capabilities=@{}; clientInfo=@{ name='McpAnalysis'; version='1.0' } } }; $null = ReadResponse
    Send @{ jsonrpc='2.0'; method='notifications/initialized'; params=@{} }
    $status = Call 2 'wpe_status_get' @{}
    $systemLogs = Call 3 'wpe_logs_list' @{ kind='system'; limit=20 }
    $proxy = Call 3 'wpe_capture_search' @{ mode='proxy'; limit=10 }
    $inject = Call 4 'wpe_capture_search' @{ mode='inject'; limit=10 }
    if ($null -eq $proxy.rows -or $null -eq $inject.rows) { throw 'Capture list response omitted rows.' }
    $find = Call 5 'wpe_capture_find_next' @{ mode='proxy'; pattern='__MCP_NO_MATCH__'; hex=$false; fromIndex=0; fromPosition=0 }
    if ($null -eq $find.Found) { throw 'Native find response omitted Found.' }
    $transcode = Call 6 'wpe_bytes_transcode' @{ text='WPE MCP 分析'; decode=$false }
    if ($null -eq $transcode.rows -or @($transcode.rows).Count -eq 0) { throw 'Native transcode returned no rows.' }
    $compare = Call 7 'wpe_bytes_compare' @{ left='0A 0B 0C 0D'; right='FF 0B 0C EE'; minimumRun=2 }
    if ($null -eq $compare.rows) { throw 'Native duplicate comparison returned no rows.' }
    $extract = Call 8 'wpe_bytes_extract' @{ kind=0; contentBase64=[Convert]::ToBase64String([byte[]](1,2,3,4)) }
    if ($null -eq $extract) { throw 'Native extraction returned no result.' }
    Write-Host 'MCP capture and analysis: PASS' -ForegroundColor Green
}
finally {
    $i.Close()
    if (-not $p.HasExited) { try { $p.Kill(); $p.WaitForExit() } catch {} }
    $p.Dispose()
}
