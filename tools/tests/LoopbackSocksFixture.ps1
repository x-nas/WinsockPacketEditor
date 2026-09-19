[CmdletBinding()]
param(
    [string]$ProxyHost = '192.168.88.15',
    [int]$ProxyPort = 1080,
    [int]$EchoPort = 23123,
    [int]$DurationSeconds = 300
)

$ErrorActionPreference = 'Stop'

function Read-Exactly([System.IO.Stream]$Stream, [int]$Count) {
    $buffer = New-Object byte[] $Count
    $offset = 0
    while ($offset -lt $Count) {
        $read = $Stream.Read($buffer, $offset, $Count - $offset)
        if ($read -le 0) { throw 'Connection closed while reading SOCKS5 response.' }
        $offset += $read
    }
    return $buffer
}

$listener = [System.Net.Sockets.TcpListener]::new([System.Net.IPAddress]::Loopback, $EchoPort)
$client = $null
$echoClient = $null
try {
    $listener.Start()
    $accept = $listener.AcceptTcpClientAsync()

    $client = [System.Net.Sockets.TcpClient]::new()
    $client.Connect($ProxyHost, $ProxyPort)
    $stream = $client.GetStream()

    $stream.Write([byte[]](0x05, 0x01, 0x00), 0, 3)
    $method = Read-Exactly $stream 2
    if ($method[0] -ne 0x05 -or $method[1] -ne 0x00) { throw 'WPE SOCKS5 server did not accept no-authentication.' }

    $request = [byte[]](0x05, 0x01, 0x00, 0x01, 127, 0, 0, 1, (($EchoPort -shr 8) -band 0xff), ($EchoPort -band 0xff))
    $stream.Write($request, 0, $request.Length)
    $reply = Read-Exactly $stream 10
    if ($reply[0] -ne 0x05 -or $reply[1] -ne 0x00) { throw "WPE SOCKS5 CONNECT failed with status $($reply[1])." }

    if (-not $accept.Wait(10000)) { throw 'The local echo listener was not reached through WPE SOCKS5.' }
    $echoClient = $accept.Result

    $payload = [Text.Encoding]::ASCII.GetBytes('WPE-MCP-loopback')
    $deadline = [DateTime]::UtcNow.AddSeconds($DurationSeconds)
    while ([DateTime]::UtcNow -lt $deadline) {
        $stream.Write($payload, 0, $payload.Length)
        Start-Sleep -Milliseconds 500
    }
}
finally {
    if ($client) { $client.Dispose() }
    if ($echoClient) { $echoClient.Dispose() }
    $listener.Stop()
}
