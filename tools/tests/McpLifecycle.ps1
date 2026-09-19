[CmdletBinding()]
param(
    [switch]$ExpectDisabled
)

$ErrorActionPreference = 'Stop'
$discovery = Join-Path ([Environment]::GetFolderPath('LocalApplicationData')) 'WPE64\mcp\instances.json'

function Read-Frame([System.IO.Stream]$stream) {
    $header = New-Object byte[] 4
    if ($stream.Read($header, 0, 4) -ne 4) { throw 'MCP gateway closed before response length.' }
    $length = [BitConverter]::ToInt32($header, 0)
    if ($length -le 0 -or $length -gt 1MB) { throw "Invalid MCP frame length: $length" }
    $body = New-Object byte[] $length
    $offset = 0
    while ($offset -lt $length) {
        $read = $stream.Read($body, $offset, $length - $offset)
        if ($read -le 0) { throw 'MCP gateway closed before response body.' }
        $offset += $read
    }
    return [Text.Encoding]::UTF8.GetString($body)
}

if ($ExpectDisabled) {
    if (Test-Path $discovery) {
        $disabledRoot = Get-Content $discovery -Raw | ConvertFrom-Json
        if ($null -ne $disabledRoot.instances -and $disabledRoot.instances.Count -gt 0) { throw "MCP discovery still contains an instance while disabled: $discovery" }
    }
    Write-Host 'MCP lifecycle: PASS (disabled, no discovery file).'
    exit 0
}

if (-not (Test-Path $discovery)) { throw "MCP discovery file not found: $discovery" }
$root = Get-Content $discovery -Raw | ConvertFrom-Json
if ($null -eq $root.instances -or $root.instances.Count -ne 1) { throw 'MCP discovery must contain exactly one instance.' }
$instance = $root.instances[0]
$process = Get-Process -Id ([int]$instance.processId) -ErrorAction SilentlyContinue
if ($null -eq $process) { throw "MCP discovery points to a dead process: $($instance.processId)" }

$pipe = New-Object System.IO.Pipes.NamedPipeClientStream('.', $instance.pipeName, [IO.Pipes.PipeDirection]::InOut, [IO.Pipes.PipeOptions]::None)
try {
    $pipe.Connect(3000)
    $payload = [Text.Encoding]::UTF8.GetBytes((@{ requestId = [Guid]::NewGuid().ToString('N'); operation = 'runtime.status'; arguments = @{} } | ConvertTo-Json -Compress))
    $pipe.Write([BitConverter]::GetBytes($payload.Length), 0, 4)
    $pipe.Write($payload, 0, $payload.Length)
    $pipe.Flush()
    $response = Read-Frame $pipe | ConvertFrom-Json
    if (-not $response.ok) { throw 'MCP gateway returned an error for runtime.status.' }
    Write-Host "MCP lifecycle: PASS (PID $($instance.processId), pipe reachable, runtime.status returned)."
}
finally { $pipe.Dispose() }
