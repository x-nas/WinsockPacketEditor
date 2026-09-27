# DecoderConfig.ps1 -- P1 解码器配置回归：行转换 / 校验 / 增删 / 列表
#
# 跑法（在 bin 目录下）：
#   powershell -ExecutionPolicy Bypass -File tools\tests\DecoderConfig.ps1 [-Bin <目录>]
#
# ⚠️ 这里只测<b>纯逻辑</b>（ToRow / FromRow / Normalize / 列表增删）。
# SQLite 落库那一段没法用反射独立跑：Microsoft.Data.Sqlite 需要应用正常启动时的程序集探测
# 与 SQLitePCLRaw 初始化，LoadFrom 单独加载会缺 System.Memory 而 TypeInitializationException。
# 落库由真实程序与 Decoder.ps1（引擎）之外的手工验收覆盖。
param([string]$Bin)
$ErrorActionPreference = 'Stop'

if (-not $Bin) { $Bin = Join-Path $PSScriptRoot '..\..\WinsockPacketEditor\bin\Release' }
$Bin = (Resolve-Path $Bin).Path
Set-Location $Bin
[Environment]::CurrentDirectory = $Bin

$asmFile = Join-Path $Bin 'WinsockPacketEditor.exe'
$asm = [Reflection.Assembly]::LoadFrom($asmFile)

$cfgType  = $asm.GetType('WinsockPacketEditor.Operate+DecoderConfig')
$listType = $asm.GetType('WinsockPacketEditor.Operate+DecoderConfig+List')
$diType   = $asm.GetType('WinsockPacketEditor.DecoderInfo')
$rowType  = $asm.GetType('WinsockPacketEditor.DecoderRow')
$kindType = $asm.GetType('WinsockPacketEditor.DecoderKind')
$modeType = $asm.GetType('WinsockPacketEditor.DecoderCipherMode')

$pass = 0; $fail = 0
function Check([string]$name, [bool]$cond, [string]$detail = '') {
    if ($cond) { $script:pass++; Write-Host ("  [PASS] " + $name) }
    else { $script:fail++; Write-Host ("  [FAIL] " + $name + "  " + $detail) -ForegroundColor Red }
}

$lst = $listType.GetField('lstDecoderInfo').GetValue($null)
$lst.Clear()

# ── ToRow / FromRow 往返 ──
$di = [Activator]::CreateInstance($diType)
$di.GUID = [Guid]::NewGuid()
$di.Name = 'unit-xor'
$di.Kind = [enum]::Parse($kindType, 'Aes')
$di.Key = '000102030405060708090A0B0C0D0E0F'
$di.LengthBytes = 4
$di.BigEndian = $true
$di.ProtocolType = 3
$di.Direction = 1
$di.IsEnable = $false

$row = $cfgType.GetMethod('ToRow').Invoke($null, @($di))
Check 'ToRow Id' (([Convert]::ToString($row.Id)) -eq ([Convert]::ToString($di.GUID).ToUpper())) ''
Check 'ToRow Kind' (([Convert]::ToInt32($row.Kind)) -eq 2) ''
Check 'ToRow BigEndian' ($row.BigEndian -eq $true) ''

$back = $cfgType.GetMethod('FromRow').Invoke($null, @($row))
Check 'FromRow kind' (([Convert]::ToInt32($back.Kind)) -eq 2) ''
Check 'FromRow key' (([Convert]::ToString($back.Key)) -eq '000102030405060708090A0B0C0D0E0F') ''
Check 'FromRow guid' (([Guid]$back.GUID) -eq ([Guid]$di.GUID)) ''
Check 'FromRow enable' ($back.IsEnable -eq $false) ''

# FromRow 无 Id 时补一个新 GUID
$blankRow = [Activator]::CreateInstance($rowType)
$blankRow.Id = ''
$newDi = $cfgType.GetMethod('FromRow').Invoke($null, @($blankRow))
Check 'FromRow empty Id generates guid' (([Guid]$newDi.GUID) -ne [Guid]::Empty) ''

# ── Normalize ──
function Invoke-Normalize($obj) {
    $a = New-Object object[] 2
    $a[0] = $obj
    $a[1] = $null
    $ok = $cfgType.GetMethod('Normalize').Invoke($null, $a)
    return [pscustomobject]@{ Ok = $ok; Error = $a[1] }
}

$bad = [Activator]::CreateInstance($diType)
$bad.Name = 'x'
$bad.Kind = [enum]::Parse($kindType, 'Aes')
$bad.Key = ''
$r = Invoke-Normalize $bad
Check 'normalize rejects empty AES key' ($r.Ok -eq $false) ''

$good = [Activator]::CreateInstance($diType)
$good.Name = 'ok'
$good.Kind = [enum]::Parse($kindType, 'Aes')
$good.Key = '00112233445566778899AABBCCDDEEFF'
$good.Iv = '00000000000000000000000000000000'
$r = Invoke-Normalize $good
Check 'normalize accepts valid AES' ($r.Ok -eq $true) ("err=" + $r.Error)

$noname = [Activator]::CreateInstance($diType)
$noname.Name = ''
$noname.Kind = [enum]::Parse($kindType, 'Xor')
$noname.Key = '01'
$r = Invoke-Normalize $noname
Check 'normalize rejects empty name' ($r.Ok -eq $false) ''

$badlen = [Activator]::CreateInstance($diType)
$badlen.Name = 'n'; $badlen.Kind = [enum]::Parse($kindType, 'Xor'); $badlen.Key = '01'; $badlen.LengthBytes = 3
$r = Invoke-Normalize $badlen
Check 'normalize rejects bad length bytes' ($r.Ok -eq $false) ''

$cts = [Activator]::CreateInstance($diType)
$cts.Name = 'cts'; $cts.Kind = [enum]::Parse($kindType, 'Aes')
$cts.Key = '00112233445566778899AABBCCDDEEFF'; $cts.Iv = '00000000000000000000000000000000'
$cts.CipherMode = [enum]::Parse($modeType, 'CTS')
$r = Invoke-Normalize $cts
Check 'normalize rejects unsupported CTS' ($r.Ok -eq $false) $r.Error

# ── 列表增删 ──
$config = $asm.GetType('WinsockPacketEditor.Operate+DecoderConfig')
$d1 = [Activator]::CreateInstance($diType); $d1.GUID = [Guid]::NewGuid(); $d1.Name = 'a'
$d2 = [Activator]::CreateInstance($diType); $d2.GUID = [Guid]::NewGuid(); $d2.Name = 'b'
$config.GetMethod('AddDecoder').Invoke($null, @($d1)) | Out-Null
$config.GetMethod('AddDecoder').Invoke($null, @($d2)) | Out-Null
Check 'add two' ($lst.Count -eq 2) ("count=" + $lst.Count)

$guidListType = [type]('System.Collections.Generic.List``1[System.Guid]')
$gl = [Activator]::CreateInstance($guidListType)
$gl.Add([Guid]$d1.GUID)
$g1 = New-Object object[] 1; $g1[0] = $gl
$del = $config.GetMethod('DeleteDecoder_ByIds').Invoke($null, $g1)
Check 'delete by id' ($del -eq 1) ("del=" + $del)
Check 'list count after delete' ($lst.Count -eq 1) ("count=" + $lst.Count)

$rows = $config.GetMethod('GetRows').Invoke($null, @())
Check 'GetRows count' ($rows.Count -eq 1) ("count=" + $rows.Count)

$label = $config.GetMethod('KindLabel').Invoke($null, @([enum]::Parse($kindType, 'Aes')))
Check 'KindLabel non-empty' (-not [string]::IsNullOrEmpty([string]$label)) ''

$lst.Clear()

Write-Host ""
Write-Host ("PASS=" + $pass + "  FAIL=" + $fail) -ForegroundColor $(if ($fail -eq 0) { 'Green' } else { 'Red' })
if ($fail -gt 0) { exit 1 } else { exit 0 }
