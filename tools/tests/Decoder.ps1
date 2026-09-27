# Decoder.ps1 -- P0 解码引擎回归：CodecEngine / FrameExtractor
#
# 跑法（在 bin 目录下）：
#   powershell -ExecutionPolicy Bypass -File tools\tests\Decoder.ps1
#   可选 -Bin <目录>，默认 ..\..\WinsockPacketEditor\bin\Release
#
# 覆盖：XOR 三种密钥格式、AES 五种模式 / 五种填充、DES、帧（包长 / 字节序 / 含自身 /
#       固定头 / 偏移）、Protobuf 推测、MessagePack 推测、文本编码 GBK、4MB 上限与错误分支。
# 每个用例在旧代码上都应当失败（DES / 模式选择 / 帧 / 偏移旧版根本没有）。
param([string]$Bin)
$ErrorActionPreference = 'Stop'

if (-not $Bin) { $Bin = Join-Path $PSScriptRoot '..\..\WinsockPacketEditor\bin\Release' }
$Bin = (Resolve-Path $Bin).Path
Set-Location $Bin
[Environment]::CurrentDirectory = $Bin

$asmFile = Join-Path $Bin 'WinsockPacketEditor.exe'
$asm = [Reflection.Assembly]::LoadFrom($asmFile)

$engineType  = $asm.GetType('WinsockPacketEditor.CodecEngine')
$diType      = $asm.GetType('WinsockPacketEditor.DecoderInfo')
$kindType    = $asm.GetType('WinsockPacketEditor.DecoderKind')
$keyFmtType  = $asm.GetType('WinsockPacketEditor.DecoderKeyFormat')
$modeType    = $asm.GetType('WinsockPacketEditor.DecoderCipherMode')
$padType     = $asm.GetType('WinsockPacketEditor.DecoderPadding')
$charsetType = $asm.GetType('WinsockPacketEditor.DecoderCharset')

$runMethod   = @($engineType.GetMethods() | Where-Object { $_.Name -eq 'Run' -and $_.GetParameters().Count -eq 2 })[0]
$encMethod   = $engineType.GetMethod('RunEncode')

$pass = 0
$fail = 0
function Check([string]$name, [bool]$cond, [string]$detail = '') {
    if ($cond) { $script:pass++; Write-Host ("  [PASS] " + $name) }
    else { $script:fail++; Write-Host ("  [FAIL] " + $name + "  " + $detail) -ForegroundColor Red }
}

function New-Decoder([string]$kind = 'Xor') {
    $di = [Activator]::CreateInstance($diType)
    $di.Kind = [enum]::Parse($kindType, $kind)
    return $di
}

function Invoke-Run($data, $di) {
    $a = New-Object object[] 2
    $a[0] = [byte[]]$data; $a[1] = $di
    return $runMethod.Invoke($null, $a)
}

function Invoke-Encode($data, $di) {
    $a = New-Object object[] 2
    $a[0] = [byte[]]$data; $a[1] = $di
    return $encMethod.Invoke($null, $a)
}

function B64ToBytes([string]$b64) { return ,([Convert]::FromBase64String($b64)) }
function BytesEqual([byte[]]$a, [byte[]]$b) {
    if ($null -eq $a -or $null -eq $b) { return $false }
    if ($a.Length -ne $b.Length) { return $false }
    for ($i = 0; $i -lt $a.Length; $i++) { if ($a[$i] -ne $b[$i]) { return $false } }
    return $true
}
function HexToBytes([string]$hex) {
    $hex = $hex -replace '[\s-]', ''
    $out = New-Object byte[] ($hex.Length / 2)
    for ($i = 0; $i -lt $out.Length; $i++) { $out[$i] = [Convert]::ToByte($hex.Substring($i * 2, 2), 16) }
    return ,$out
}
function TextBytes([string]$s) { return ,([Text.Encoding]::UTF8.GetBytes($s)) }

Write-Host "`n== P0 Decoder engine ==" -ForegroundColor Cyan

# ── XOR ───────────────────────────────────────────────────────────────
Write-Host "`n-- XOR --" -ForegroundColor Cyan
$di = New-Decoder 'Xor'
$di.Key = '0102'; $di.KeyFormat = [enum]::Parse($keyFmtType, 'Hex')
$r = Invoke-Run ([byte[]](1,2,3,4)) $di
$exp = [byte[]](0,0,2,6)
Check 'XOR hex key' ($r.Ok -and (BytesEqual (B64ToBytes $r.OutputBase64) $exp)) $r.Error

$di.KeyFormat = [enum]::Parse($keyFmtType, 'Text')
$di.Key = [char]1 + [char]2
$r = Invoke-Run ([byte[]](1,2,3,4)) $di
Check 'XOR text key' ($r.Ok -and (BytesEqual (B64ToBytes $r.OutputBase64) $exp)) $r.Error

$di.KeyFormat = [enum]::Parse($keyFmtType, 'Base64')
$di.Key = [Convert]::ToBase64String([byte[]](1,2))
$r = Invoke-Run ([byte[]](1,2,3,4)) $di
Check 'XOR base64 key' ($r.Ok -and (BytesEqual (B64ToBytes $r.OutputBase64) $exp)) $r.Error

$di.KeyFormat = [enum]::Parse($keyFmtType, 'Hex'); $di.Key = ''
$r = Invoke-Run ([byte[]](1,2)) $di
Check 'XOR empty key fails' (-not $r.Ok) ''

# ── AES ───────────────────────────────────────────────────────────────
Write-Host "`n-- AES --" -ForegroundColor Cyan
$key16 = HexToBytes '000102030405060708090A0B0C0D0E0F'
$iv16  = HexToBytes '0F0E0D0C0B0A09080706050403020100'
$plain = TextBytes 'hello wpe decoder 你好'

foreach ($mode in @('CBC','ECB','OFB','CFB','CTS')) {
    foreach ($pad in @('None','PKCS7','Zeros','ANSIX923','ISO10126')) {
        $di = New-Decoder 'Aes'
        $di.KeyFormat = [enum]::Parse($keyFmtType, 'Hex'); $di.Key = ($key16 | ForEach-Object { $_.ToString('X2') }) -join ''
        $di.IvFormat  = [enum]::Parse($keyFmtType, 'Hex'); $di.Iv  = ($iv16  | ForEach-Object { $_.ToString('X2') }) -join ''
        $di.CipherMode = [enum]::Parse($modeType, $mode)
        $di.Padding = [enum]::Parse($padType, $pad)

        # .NET 限制：OFB/CFB/CTS 只支持 None 填充，跳过多余组合
        if (($mode -in @('OFB','CFB','CTS')) -and $pad -ne 'None') { continue }

        $src = $plain
        if ($pad -eq 'None' -and ($plain.Length % 16) -ne 0) {
            # 无填充要求长度是块大小整数倍，补齐到 16
            $src = New-Object byte[] 16
            [Array]::Copy($plain, $src, [Math]::Min($plain.Length, 16))
        }

        if ($mode -eq 'CTS') {
            # 本机 .NET 的对称 provider 不支持 CTS；契约是「返回可读错误、不抛、不出半对结果」
            $enc = Invoke-Encode $src $di
            Check 'AES CTS clean unsupported error' ((-not $enc.Ok) -and $enc.Error) ''
            continue
        }

        $enc = Invoke-Encode $src $di
        if (-not $enc.Ok) { Check ("AES $mode/$pad encrypt") $false $enc.Error; continue }
        $dec = Invoke-Run (B64ToBytes $enc.OutputBase64) $di
        Check ("AES $mode/$pad roundtrip") ($dec.Ok -and (BytesEqual (B64ToBytes $dec.OutputBase64) $src)) $dec.Error
    }
}

$di = New-Decoder 'Aes'
$di.Key = '00'; $di.KeyFormat = [enum]::Parse($keyFmtType, 'Hex')
$r = Invoke-Run ([byte[]](1,2,3)) $di
Check 'AES bad key size fails' (-not $r.Ok) ''

# ── DES ───────────────────────────────────────────────────────────────
Write-Host "`n-- DES --" -ForegroundColor Cyan
$desKey = HexToBytes '0123456789ABCDEF'
$desIv  = HexToBytes 'FEDCBA9876543210'
$desPlain = TextBytes 'des-test!'
$di = New-Decoder 'Des'
$di.KeyFormat = [enum]::Parse($keyFmtType, 'Hex'); $di.Key = '0123456789ABCDEF'
$di.IvFormat  = [enum]::Parse($keyFmtType, 'Hex'); $di.Iv  = 'FEDCBA9876543210'
$di.CipherMode = [enum]::Parse($modeType, 'CBC')
$di.Padding = [enum]::Parse($padType, 'PKCS7')
$enc = Invoke-Encode $desPlain $di
Check 'DES encrypt' $enc.Ok $enc.Error
if ($enc.Ok) {
    $dec = Invoke-Run (B64ToBytes $enc.OutputBase64) $di
    Check 'DES CBC roundtrip' ($dec.Ok -and (BytesEqual (B64ToBytes $dec.OutputBase64) $desPlain)) $dec.Error
}

# ── RC4 / XXTEA / structure decoders ──────────────────────────────────
Write-Host "`n-- Additional decoders --" -ForegroundColor Cyan
$di = New-Decoder 'Rc4'; $di.Key = '0102030405060708'
$rc4Plain = TextBytes 'rc4 roundtrip'
$enc = Invoke-Encode $rc4Plain $di; $dec = Invoke-Run (B64ToBytes $enc.OutputBase64) $di
Check 'RC4 roundtrip' ($enc.Ok -and $dec.Ok -and (BytesEqual (B64ToBytes $dec.OutputBase64) $rc4Plain)) $dec.Error

$di = New-Decoder 'Xxtea'; $di.Key = '000102030405060708090A0B0C0D0E0F'
$xxPlain = [byte[]](1,2,3,4,5,6,7,8)
$enc = Invoke-Encode $xxPlain $di; $dec = Invoke-Run (B64ToBytes $enc.OutputBase64) $di
Check 'XXTEA roundtrip' ($enc.Ok -and $dec.Ok -and (BytesEqual (B64ToBytes $dec.OutputBase64) $xxPlain)) $dec.Error

$di = New-Decoder 'Bson'; $r = Invoke-Run ([byte[]](12,0,0,0,16,0x78,0,1,0,0,0,0)) $di
Check 'BSON document' ($r.Ok -and $r.Text.Contains('"x"')) $r.Error
$di = New-Decoder 'Amf'; $r = Invoke-Run ([byte[]](2,0,2,0x68,0x69)) $di
Check 'AMF0 string' ($r.Ok -and $r.Text.Contains('hi')) $r.Error
$di = New-Decoder 'FlatBuffers'; $r = Invoke-Run ([byte[]](8,0,0,0,6,0,8,0,4,0,0,0,4,0,0,0,42,0,0,0)) $di
Check 'FlatBuffers generic table' ($r.Ok -and $r.Text.Contains('field 0')) $r.Error

# ── 帧 ────────────────────────────────────────────────────────────────
Write-Host "`n-- Frame --" -ForegroundColor Cyan
$di = New-Decoder 'Xor'
$di.KeyFormat = [enum]::Parse($keyFmtType, 'Hex'); $di.Key = '00'  # 恒等
$di.LengthBytes = 2; $di.BigEndian = $false
$payload = [byte[]](0xAA,0xBB,0xCC)
$framed = [byte[]](0x03,0x00,0xAA,0xBB,0xCC)
$r = Invoke-Run $framed $di
Check 'frame 2B LE length' ($r.Ok -and (BytesEqual (B64ToBytes $r.OutputBase64) $payload)) $r.Error

$di2 = New-Decoder 'Xor'; $di2.Key = '00'
$di2.LengthBytes = 2; $di2.BigEndian = $true
$r = Invoke-Run ([byte[]](0x00,0x03,0xAA,0xBB,0xCC)) $di2
Check 'frame 2B BE length' ($r.Ok -and (BytesEqual (B64ToBytes $r.OutputBase64) $payload)) $r.Error

$di3 = New-Decoder 'Xor'; $di3.Key = '00'
$di3.HasFixedHeader = $true; $di3.FixedHeader = 'AB CD'
$di3.LengthBytes = 2; $di3.BigEndian = $false; $di3.LengthIncludesFixedHeader = $true
$p2 = [byte[]](0x11,0x22)
$r = Invoke-Run ([byte[]](0xAB,0xCD,0x04,0x00,0x11,0x22)) $di3
Check 'frame fixed header + includes header' ($r.Ok -and (BytesEqual (B64ToBytes $r.OutputBase64) $p2)) $r.Error

$di4 = New-Decoder 'Xor'; $di4.Key = '00'
$di4.HasFixedHeader = $true; $di4.FixedHeader = 'ABCD'
$r = Invoke-Run ([byte[]](0x00,0x00,0x11,0x22)) $di4
Check 'frame header mismatch fails' (-not $r.Ok) ''

$di5 = New-Decoder 'Xor'; $di5.Key = '00'
$di5.DataOffset = 2
$r = Invoke-Run ([byte[]](0x00,0x00,0xAA,0xBB,0xCC)) $di5
Check 'frame data offset' ($r.Ok -and (BytesEqual (B64ToBytes $r.OutputBase64) $payload)) $r.Error

$di6 = New-Decoder 'Xor'; $di6.Key = '00'
$di6.LengthBytes = 2; $di6.BigEndian = $false
$r = Invoke-Run ([byte[]](0x09,0x00,0xAA)) $di6   # 声明 9 字节但只有 1
Check 'frame overrun fails' (-not $r.Ok) ''

# 帧往返
$di7 = New-Decoder 'Aes'
$di7.KeyFormat = [enum]::Parse($keyFmtType, 'Hex'); $di7.Key = ($key16 | ForEach-Object { $_.ToString('X2') }) -join ''
$di7.IvFormat  = [enum]::Parse($keyFmtType, 'Hex'); $di7.Iv  = ($iv16  | ForEach-Object { $_.ToString('X2') }) -join ''
$di7.LengthBytes = 2; $di7.BigEndian = $false; $di7.LengthIncludesSelf = $true
$enc = Invoke-Encode $plain $di7
$dec = Invoke-Run (B64ToBytes $enc.OutputBase64) $di7
Check 'frame roundtrip (length includes self)' ($enc.Ok -and $dec.Ok -and (BytesEqual (B64ToBytes $dec.OutputBase64) $plain)) ($enc.Error + $dec.Error)

# ── Protobuf / MessagePack ────────────────────────────────────────────
Write-Host "`n-- Protobuf / MessagePack --" -ForegroundColor Cyan
$di = New-Decoder 'Protobuf'
$r = Invoke-Run ([byte[]](0x08,0x96,0x01)) $di
Check 'protobuf guess' ($r.Ok -and $r.Text -match '150') $r.Error
Check 'protobuf format tag' ($r.Format -eq 'protobuf') $r.Format

$di = New-Decoder 'MessagePack'
$r = Invoke-Run ([byte[]](0x81,0xA1,0x61,0x01)) $di
Check 'msgpack fixmap' ($r.Ok -and $r.Text -match 'map\(1\)' -and $r.Text -match '"a"') $r.Error
$r = Invoke-Run ([byte[]](0x93,0x01,0x02,0x03)) $di
Check 'msgpack fixarray' ($r.Ok -and $r.Text -match 'array\(3\)') $r.Error

# ── 文本编码 ──────────────────────────────────────────────────────────
Write-Host "`n-- Text charset --" -ForegroundColor Cyan
$di = New-Decoder 'TextCharset'
$di.Charset = [enum]::Parse($charsetType, 'GBK')
$r = Invoke-Run ([byte[]](0xD6,0xD0)) $di
Check 'GBK decode 中' ($r.Ok -and $r.Text -eq ([char]0x4E2D)) ($r.Error + ' -> ' + $r.Text)

$di = New-Decoder 'TextCharset'
$di.Charset = [enum]::Parse($charsetType, 'UTF8')
$r = Invoke-Run (TextBytes 'abc') $di
Check 'UTF8 decode abc' ($r.Ok -and $r.Text -eq 'abc') $r.Error

# ── 上限 ──────────────────────────────────────────────────────────────
Write-Host "`n-- Limits --" -ForegroundColor Cyan
$big = New-Object byte[] (4 * 1024 * 1024 + 1)
$di = New-Decoder 'Xor'; $di.Key = '00'
$r = Invoke-Run $big $di
Check '4MB limit' (-not $r.Ok) ''

Write-Host ""
Write-Host ("PASS=" + $pass + "  FAIL=" + $fail) -ForegroundColor $(if ($fail -eq 0) { 'Green' } else { 'Red' })
if ($fail -gt 0) { exit 1 } else { exit 0 }
