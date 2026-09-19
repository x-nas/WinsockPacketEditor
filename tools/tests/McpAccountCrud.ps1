$ErrorActionPreference = 'Stop'
$exe = (Resolve-Path (Join-Path $PSScriptRoot '..\..\WPEMcpServer\bin\Release\net10.0\WPEMcpServer.exe')).Path
$psi = [Diagnostics.ProcessStartInfo]::new($exe)
$psi.UseShellExecute = $false; $psi.RedirectStandardInput = $true; $psi.RedirectStandardOutput = $true; $psi.RedirectStandardError = $true
$p = [Diagnostics.Process]::new(); $p.StartInfo = $psi; [void]$p.Start()
$i = $p.StandardInput; $o = $p.StandardOutput
function Send($x) { $i.WriteLine(($x | ConvertTo-Json -Compress -Depth 40)); $i.Flush() }
function ReadResponse() { while ($true) { $line = $o.ReadLine(); if ($null -eq $line) { throw 'Sidecar closed stdout.' }; if ([string]::IsNullOrWhiteSpace($line)) { continue }; try { $msg = $line | ConvertFrom-Json } catch { continue }; if ($null -ne $msg.id) { return $msg } } }
function Call([int]$id, [string]$name, $toolArguments) { Send @{ jsonrpc='2.0'; id=$id; method='tools/call'; params=@{ name=$name; arguments=$toolArguments } }; $msg = ReadResponse; if ($msg.error) { throw "$name failed: $($msg.error.message)" }; if ($msg.result.isError) { throw "$name returned an MCP error." }; return ($msg.result.content[0].text | ConvertFrom-Json) }
$accountId = $null
try {
    Send @{ jsonrpc='2.0'; id=1; method='initialize'; params=@{ protocolVersion='2025-06-18'; capabilities=@{}; clientInfo=@{ name='McpAccountCrud'; version='1.0' } } }; $null = ReadResponse
    Send @{ jsonrpc='2.0'; method='notifications/initialized'; params=@{} }
    $suffix = [guid]::NewGuid().ToString('N').Substring(0, 12)
    $createArgs = @{ userName = "mcp-crud-$suffix"; password = "McpCrud!$suffix"; enabled = $true; limitLinksEnabled = $true; limitLinks = 3; limitDevicesEnabled = $true; limitDevices = 2; expiryEnabled = $false; expiryTime = $null; idempotencyKey = ([guid]::NewGuid().ToString()) }
    $created = Call 2 'wpe_account_create' $createArgs
    $accountId = $created.account.Id; if ([string]::IsNullOrWhiteSpace($accountId)) { throw 'Create returned no account id.' }
    $got = Call 3 'wpe_account_get' @{ id = $accountId }; if ($got.UserName -ne $createArgs.userName -or -not $got.IsEnable) { throw 'Create read-back verification failed.' }
    $listed = Call 4 'wpe_accounts_list' @{ limit = 200; userName = $createArgs.userName }; if (@($listed.rows).Count -ne 1 -or $listed.rows[0].Id -ne $accountId) { throw 'Account userName search did not return exactly the created account GUID.' }
    $updated = Call 5 'wpe_account_update' @{ id = $accountId; enabled = $false; limitLinksEnabled = $true; limitLinks = 5; limitDevicesEnabled = $true; limitDevices = 4; expiryEnabled = $false; expiryTime = $null; password = $null; idempotencyKey = ([guid]::NewGuid().ToString()) }
    if ($updated.account.IsEnable -or $updated.account.LimitLinks -ne 5 -or $updated.account.LimitDevices -ne 4) { throw 'Update verification failed.' }
    $enabled = Call 6 'wpe_account_set_enabled' @{ id = $accountId; enabled = $true; idempotencyKey = ([guid]::NewGuid().ToString()) }; if (-not $enabled.changed) { throw 'Set enabled returned changed=false.' }
    $logins = Call 7 'wpe_account_logins_list' @{ id = $accountId; limit = 50 }; if ($null -eq $logins.rows) { throw 'Login records response has no rows array.' }
    $deleted = Call 8 'wpe_account_delete' @{ id = $accountId; idempotencyKey = ([guid]::NewGuid().ToString()) }; if (-not $deleted.changed) { throw 'Delete returned changed=false.' }; $accountId = $null
    Write-Host 'MCP account CRUD: PASS' -ForegroundColor Green
}
finally {
    if ($accountId) { try { Call 99 'wpe_account_delete' @{ id = $accountId; idempotencyKey = ([guid]::NewGuid().ToString()) } | Out-Null } catch {} }
    $i.Close(); if (-not $p.HasExited) { try { $p.Kill() } catch {} }; $p.Dispose()
}
