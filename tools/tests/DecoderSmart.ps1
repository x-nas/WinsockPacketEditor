# DecoderSmart.ps1 -- P3 智能解码回归：CodecEngine.SmartDecode（命中 / 排除乱码 / 偏移探测）
#
# 跑法（在 bin 目录下）：
#   powershell -ExecutionPolicy Bypass -File tools\tests\DecoderSmart.ps1 [-Bin <目录>]
param([string]$Bin)
$ErrorActionPreference = 'Stop'

if (-not $Bin) { $Bin = Join-Path $PSScriptRoot '..\..\WinsockPacketEditor\bin\Release' }
$Bin = (Resolve-Path $Bin).Path
Set-Location $Bin
[Environment]::CurrentDirectory = $Bin

$asm = [Reflection.Assembly]::LoadFrom((Join-Path $Bin 'WinsockPacketEditor.exe'))

$engineType = $asm.GetType('WinsockPacketEditor.CodecEngine')
$diType     = $asm.GetType('WinsockPacketEditor.DecoderInfo')
$kindType   = $asm.GetType('WinsockPacketEditor.DecoderKind')
$keyFmtType = $asm.GetType('WinsockPacketEditor.DecoderKeyFormat')
$protocolType = $asm.GetType('WinsockPacketEditor.DecoderProtocol')
$packetType = $asm.GetType('WinsockPacketEditor.Operate+PacketConfig+Packet+PacketType')

$pass = 0; $fail = 0
function Check([string]$name, [bool]$cond, [string]$detail = '') {
    if ($cond) { $script:pass++; Write-Host ("  [PASS] " + $name) }
    else { $script:fail++; Write-Host ("  [FAIL] " + $name + "  " + $detail) -ForegroundColor Red }
}

function New-Decoder([string]$key, [bool]$enable = $true, [int]$offset = 0) {
    $di = [Activator]::CreateInstance($diType)
    $di.GUID = [Guid]::NewGuid()
    $di.Name = 'xor-' + $key
    $di.Kind = [enum]::Parse($kindType, 'Xor')
    $di.KeyFormat = [enum]::Parse($keyFmtType, 'Hex')
    $di.Key = $key
    $di.IsEnable = $enable
    $di.DataOffset = $offset
    return $di
}

function New-DecoderList($items) {
    $t = [type]("System.Collections.Generic.List``1[$($diType.FullName)]")
    $l = [Activator]::CreateInstance($t)
    foreach ($i in $items) { $l.Add($i) }
    return ,$l
}

function Invoke-Smart($data, $list) {
    $a = New-Object object[] 2
    $a[0] = [byte[]]$data; $a[1] = $list
    $method = @($engineType.GetMethods() | Where-Object { $_.Name -eq 'SmartDecode' -and $_.GetParameters().Count -eq 2 })[0]
    return $method.Invoke($null, $a)
}

function XorBytes([byte[]]$data, [byte]$key) {
    $out = New-Object byte[] $data.Length
    for ($i = 0; $i -lt $data.Length; $i++) { $out[$i] = $data[$i] -bxor $key }
    return ,$out
}

$plain = [Text.Encoding]::ASCII.GetBytes('hello smart decode, this is readable text.')

Write-Host "`n== P3 SmartDecode ==" -ForegroundColor Cyan

# 正确密钥 → 命中
$enc = XorBytes $plain 0x20
$l = New-DecoderList @((New-Decoder '20'))
$hits = Invoke-Smart $enc $l
Check 'correct key hits' ($hits.Count -eq 1) ("hits=" + $hits.Count)if ($hits.Count -ge 1) {
    Check 'hit text readable' ((Convert.ToString($hits[0].Text)).Contains('hello smart')) ''
    Check 'hit offset 0' ($hits[0].Offset -eq 0) ''
}

# 错误密钥 → 解出乱码，不命中
$l2 = New-DecoderList @((New-Decoder '7F'))
$hits2 = Invoke-Smart $enc $l2
Check 'wrong key no hit' ($hits2.Count -eq 0) ("hits=" + $hits2.Count)

# 一条对一条错 → 只命中对的
$l3 = New-DecoderList @((New-Decoder '7F'), (New-Decoder '20'))
$hits3 = Invoke-Smart $enc $l3
Check 'mixed decoders only correct hits' ($hits3.Count -eq 1) ("hits=" + $hits3.Count)

# 偏移探测：前面加 1 字节前缀，配置偏移为 0 时应当试出 offset=1
$prefixed = New-Object byte[] ($enc.Length + 1)
$prefixed[0] = 0xAB
[Array]::Copy($enc, 0, $prefixed, 1, $enc.Length)
$l4 = New-DecoderList @((New-Decoder '20'))
$hits4 = Invoke-Smart $prefixed $l4
Check 'offset probe finds hit' ($hits4.Count -eq 1) ("hits=" + $hits4.Count)
if ($hits4.Count -ge 1) { Check 'offset probe reports 1' ($hits4[0].Offset -eq 1) ("off=" + $hits4[0].Offset) }

# 禁用的解码器不参与
$l5 = New-DecoderList @((New-Decoder '20' $false))
$hits5 = Invoke-Smart $enc $l5
Check 'disabled decoder skipped' ($hits5.Count -eq 0) ("hits=" + $hits5.Count)

# 明文的偏移写死时按写的来
$l6 = New-DecoderList @((New-Decoder '20' $true 0))
$hits6 = Invoke-Smart $enc $l6
Check 'pinned offset respected' ($hits6.Count -eq 1 -and $hits6[0].Offset -eq 0) ''

# 实际封包范围：同一配置只能用于匹配的协议/方向，不能只是存库字段。
$scoped = New-Decoder '20'
$scoped.ProtocolType = [enum]::Parse($protocolType, 'Tcp')
$runScoped = @($engineType.GetMethods() | Where-Object { $_.Name -eq 'Run' -and $_.GetParameters().Count -eq 3 })[0]
$scopeArgs = New-Object object[] 3
$scopeArgs[0] = [byte[]]$enc; $scopeArgs[1] = $scoped; $scopeArgs[2] = [enum]::Parse($packetType, 'TCP_Req')
$scopeOk = $runScoped.Invoke($null, $scopeArgs)
Check 'TCP scope accepts TCP request' $scopeOk.Ok $scopeOk.Error
$scopeArgs[2] = [enum]::Parse($packetType, 'UDP_Req')
$scopeBad = $runScoped.Invoke($null, $scopeArgs)
Check 'TCP scope rejects UDP request' (-not $scopeBad.Ok) $scopeBad.Error

Write-Host ""
Write-Host ("PASS=" + $pass + "  FAIL=" + $fail) -ForegroundColor $(if ($fail -eq 0) { 'Green' } else { 'Red' })
if ($fail -gt 0) { exit 1 } else { exit 0 }
