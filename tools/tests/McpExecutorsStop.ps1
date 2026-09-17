[CmdletBinding()]
param(
    [string]$Server,
    [switch]$RequireRunningRobot,
    [switch]$RequireRunningSend
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
    $writer.WriteLine(($message | ConvertTo-Json -Compress -Depth 20))
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

function Call-Tool($writer, $reader, [int]$id, [string]$name, $toolArguments) {
    Send-Message $writer @{ jsonrpc = '2.0'; id = $id; method = 'tools/call'; params = @{ name = $name; arguments = $toolArguments } }
    $message = Read-Response $reader
    if ($message.error) { throw "$name failed: $($message.error.message)" }
    return $message.result
}

try {
    $stdin = $process.StandardInput
    $stdout = $process.StandardOutput
    Send-Message $stdin @{ jsonrpc = '2.0'; id = 1; method = 'initialize'; params = @{ protocolVersion = '2025-06-18'; capabilities = @{}; clientInfo = @{ name = 'McpExecutorsStop'; version = '1.0' } } }
    $initialize = Read-Response $stdout
    if ($initialize.error) { throw "MCP initialize failed: $($initialize.error.message)" }
    Send-Message $stdin @{ jsonrpc = '2.0'; method = 'notifications/initialized'; params = @{} }

    $before = ((Call-Tool $stdin $stdout 2 'wpe_executors_detail_get' @{}).content[0].text | ConvertFrom-Json)
    if ($RequireRunningRobot -or $RequireRunningSend) {
        if ($RequireRunningRobot -and $before.robotRunning -lt 1) {
            throw "Expected a native robot executor to be running before the stop request (send=$($before.sendRunning), robot=$($before.robotRunning))."
        }
        if ($RequireRunningSend -and $before.sendRunning -lt 1) {
            throw "Expected a native sender executor to be running before the stop request (send=$($before.sendRunning), robot=$($before.robotRunning))."
        }
    }
    elseif ($before.sendRunning -ne 0 -or $before.robotRunning -ne 0) {
        throw "This safe no-task regression requires no running executors (send=$($before.sendRunning), robot=$($before.robotRunning))."
    }

    $key = [guid]::NewGuid().ToString()
    $firstResult = Call-Tool $stdin $stdout 3 'wpe_executors_stop_all' @{ idempotencyKey = $key }
    if ($firstResult.isError) { throw 'wpe_executors_stop_all returned an MCP error for a valid UUID.' }
    $first = $firstResult.content[0].text | ConvertFrom-Json
    if ($first.outcome -ne 'approved' -or $first.sendRunning -ne 0 -or $first.robotRunning -ne 0) {
        throw 'Stop request did not report an approved, fully stopped result.'
    }
    if ($RequireRunningRobot -or $RequireRunningSend) {
        if (-not $first.changed) {
            throw 'An active executor was not reported as changed by the stop request.'
        }
        if ($RequireRunningRobot -and $first.stoppedRobot -lt 1) {
            throw "The active robot was not reported as stopped (stoppedRobot=$($first.stoppedRobot))."
        }
        if ($RequireRunningSend -and $first.stoppedSend -lt 1) {
            throw "The active sender was not reported as stopped (stoppedSend=$($first.stoppedSend))."
        }
    }
    elseif ($first.changed -or $first.stoppedSend -ne 0 -or $first.stoppedRobot -ne 0) {
        throw 'No-task stop returned an unexpected result.'
    }

    $secondResult = Call-Tool $stdin $stdout 4 'wpe_executors_stop_all' @{ idempotencyKey = $key }
    if ($secondResult.isError) { throw 'Repeated idempotency key returned an MCP error.' }
    $second = $secondResult.content[0].text | ConvertFrom-Json
    if ($second.requestHash -ne $first.requestHash -or $second.changed -ne $first.changed -or $second.stoppedSend -ne $first.stoppedSend -or $second.stoppedRobot -ne $first.stoppedRobot) {
        throw 'Repeated idempotency key did not return the original result.'
    }

    $invalid = Call-Tool $stdin $stdout 5 'wpe_executors_stop_all' @{ idempotencyKey = 'not-a-uuid' }
    if (-not $invalid.isError) { throw 'Invalid idempotency key was accepted.' }

    $after = ((Call-Tool $stdin $stdout 6 'wpe_executors_detail_get' @{}).content[0].text | ConvertFrom-Json)
    if ($after.sendRunning -ne 0 -or $after.robotRunning -ne 0) { throw 'No-task stop unexpectedly started an executor.' }

    if ($RequireRunningRobot -or $RequireRunningSend) {
        $kind = if ($RequireRunningRobot -and $RequireRunningSend) { 'robot and sender' } elseif ($RequireRunningRobot) { 'robot' } else { 'sender' }
        Write-Host "MCP executors stop: PASS (active native $kind stopped, idempotency and UUID rejection)." -ForegroundColor Green
    }
    else {
        Write-Host 'MCP executors stop: PASS (no running tasks, idempotency and UUID rejection).' -ForegroundColor Green
    }
}
finally {
    if ($null -ne $process) {
        if (-not $process.HasExited) { try { $process.Kill() } catch {} }
        $process.Dispose()
    }
}
