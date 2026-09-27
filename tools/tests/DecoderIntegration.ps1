# DecoderIntegration.ps1 -- P2/P3 集成回归：按 Id 从列表取真实字节 → decodeWith / batchDecode
#
# 跑法（在 bin 目录下）：
#   powershell -ExecutionPolicy Bypass -File tools\tests\DecoderIntegration.ps1 [-Bin <目录>]
#
# 验证「列表右键解码」这条链路的 C# 侧：把真包塞进 lstProxyInfo / lstPacketInfo，
# 走 PickProxies / PickPackets 取字节，按规格解码。
param([string]$Bin)
$ErrorActionPreference = 'Stop'

if (-not $Bin) { $Bin = Join-Path $PSScriptRoot '..\..\WinsockPacketEditor\bin\Release' }
$Bin = (Resolve-Path $Bin).Path
Set-Location $Bin
[Environment]::CurrentDirectory = $Bin

$asm = [Reflection.Assembly]::LoadFrom((Join-Path $Bin 'WinsockPacketEditor.exe'))

$cfgType    = $asm.GetType('WinsockPacketEditor.Operate+DecoderConfig')
$listType   = $asm.GetType('WinsockPacketEditor.Operate+DecoderConfig+List')
$diType     = $asm.GetType('WinsockPacketEditor.DecoderInfo')
$kindType   = $asm.GetType('WinsockPacketEditor.DecoderKind')
$kfType     = $asm.GetType('WinsockPacketEditor.DecoderKeyFormat')
$engineType = $asm.GetType('WinsockPacketEditor.CodecEngine')
$proxyList  = $asm.GetType('WinsockPacketEditor.Operate+ProxyConfig+List')
$packetList = $asm.GetType('WinsockPacketEditor.Operate+PacketConfig+List')
$proxyType  = $asm.GetType('WinsockPacketEditor.ProxyInfo')
$packetType = $asm.GetType('WinsockPacketEditor.PacketInfo')

$pass = 0; $fail = 0
function Check([string]$name, [bool]$cond, [string]$detail = '') {
    if ($cond) { $script:pass++; Write-Host ("  [PASS] " + $name) }
    else { $script:fail++; Write-Host ("  [FAIL] " + $name + "  " + $detail) -ForegroundColor Red }
}

function XorBytes([byte[]]$data, [byte]$key) {
    $out = New-Object byte[] $data.Length
    for ($i = 0; $i -lt $data.Length; $i++) { $out[$i] = $data[$i] -bxor $key }
    return ,$out
}

# 造一条 XOR 解码器
$di = [Activator]::CreateInstance($diType)
$di.GUID = [Guid]::NewGuid()
$di.Name = 'it-xor'
$di.Kind = [enum]::Parse($kindType, 'Xor')
$di.KeyFormat = [enum]::Parse($kfType, 'Hex')
$di.Key = '20'
$di.IsEnable = $true

$lst = $listType.GetField('lstDecoderInfo').GetValue($null)
$lst.Clear()
$cfgType.GetMethod('AddDecoder').Invoke($null, @($di)) | Out-Null

# 造两条真实封包（明文 XOR 0x20）
$plain1 = [Text.Encoding]::ASCII.GetBytes('packet one: login request payload.')
$plain2 = [Text.Encoding]::ASCII.GetBytes('packet two: item list response data.')
$enc1 = XorBytes $plain1 0x20
$enc2 = XorBytes $plain2 0x20

$proxies = $proxyList.GetField('lstProxyInfo').GetValue($null)
$proxies.Clear()

foreach ($buf in @($enc1, $enc2)) {
    $pi = [Activator]::CreateInstance($proxyType)
    $pi.PacketBuffer = [byte[]]$buf
    $proxies.Add($pi)
}

# 用真实 Id 建 id 列表
$idList = [Activator]::CreateInstance([type]('System.Collections.Generic.List``1[System.Int64]'))
foreach ($pi in $proxies) { $idList.Add([long]$pi.Id) }
$flags = [Reflection.BindingFlags]::Public -bor [Reflection.BindingFlags]::Static -bor [Reflection.BindingFlags]::NonPublic
$a = New-Object object[] 1; $a[0] = $idList
$hex = $proxyList.GetMethod('GetProxyHexMerged_ByIds').Invoke($null, $a)
Check 'proxy list has hex' (-not [string]::IsNullOrEmpty([string]$hex)) ''

# decodeWith：C# 侧按 Id 取第一条
$one = [Activator]::CreateInstance([type]('System.Collections.Generic.List``1[System.Int64]'))
$one.Add([long]$proxies[0].Id)
$a1 = New-Object object[] 1; $a1[0] = $one
$hexOne = [string]$proxyList.GetMethod('GetProxyHex_ByIds').Invoke($null, $a1)
$bytes = New-Object byte[] ($hexOne.Length / 3 + 1)
$parts = $hexOne -split ' '
$bytes = New-Object byte[] $parts.Length
for ($i = 0; $i -lt $parts.Length; $i++) { $bytes[$i] = [Convert]::ToByte($parts[$i], 16) }

$ra = New-Object object[] 2
$ra[0] = [byte[]]$bytes; $ra[1] = $di
$runMethod = @($engineType.GetMethods() | Where-Object { $_.Name -eq 'Run' -and $_.GetParameters().Count -eq 2 })[0]
$res = $runMethod.Invoke($null, $ra)
Check 'decodeWith proxy[0]' ($res.Ok -and ([string]$res.Text).Contains('login request')) ("text=" + $res.Text)

# batchDecode 的核心：PickProxies 按 Id 顺序取
$pick = $proxyList.GetMethod('PickProxies', $flags).Invoke($null, $a)
Check 'PickProxies count' ($pick.Count -eq 2) ("count=" + $pick.Count)

# 注入模式的封包列表同一条链路
$packets = $packetList.GetField('lstPacketInfo').GetValue($null)
$packets.Clear()
$pkt = [Activator]::CreateInstance($packetType)
$pkt.PacketBuffer = [byte[]]$enc2
$packets.Add($pkt)
$pidList = [Activator]::CreateInstance([type]('System.Collections.Generic.List``1[System.Int64]'))
$pidList.Add([long]$pkt.Id)
$b = New-Object object[] 1; $b[0] = $pidList
$pickPkt = $packetList.GetMethod('PickPackets', $flags).Invoke($null, $b)
Check 'PickPackets count' ($pickPkt.Count -eq 1) ("count=" + $pickPkt.Count)
if ($pickPkt.Count -eq 1) {
    $rb = New-Object object[] 2
    $rb[0] = [byte[]]$pickPkt[0].PacketBuffer; $rb[1] = $di
    $res2 = $runMethod.Invoke($null, $rb)
    Check 'decodeWith packet[0]' ($res2.Ok -and ([string]$res2.Text).Contains('item list')) ("text=" + $res2.Text)
}

# 清理，别把测试包留在列表里
$proxies.Clear(); $packets.Clear(); $lst.Clear()

Write-Host ""
Write-Host ("PASS=" + $pass + "  FAIL=" + $fail) -ForegroundColor $(if ($fail -eq 0) { 'Green' } else { 'Red' })
if ($fail -gt 0) { exit 1 } else { exit 0 }
