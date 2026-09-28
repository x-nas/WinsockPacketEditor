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

# ── 复制 / 按 Id 选取 / XML 导入导出（列表页的列表动作底座）──
$copy = $config.GetMethod('CopyDecoder').Invoke($null, @($d2))
Check 'copy new guid' (([Guid]$copy.GUID) -ne ([Guid]$d2.GUID)) ''
Check 'copy keeps kind' (([Convert]::ToInt32($copy.Kind)) -eq ([Convert]::ToInt32($d2.Kind))) ''
Check 'copy name prefixed' (([string]$copy.Name).StartsWith('b')) ("name=" + $copy.Name)
Check 'copy appended' ($lst.Count -eq 2) ("count=" + $lst.Count)

$strListType = [type]('System.Collections.Generic.List``1[System.String]')
$ids = [Activator]::CreateInstance($strListType)
$ids.Add([string]$copy.GUID)
$ids.Add([string]$d2.GUID)
$idsArg = New-Object object[] 1; $idsArg[0] = $ids
$picked = $config.GetMethod('PickDecoders').Invoke($null, $idsArg)
Check 'PickDecoders count' ($picked.Count -eq 2) ("count=" + $picked.Count)
Check 'PickDecoders list order' (([Guid]$picked[0].GUID) -eq ([Guid]$d2.GUID)) ''

$diListType = [type]('System.Collections.Generic.List``1[WinsockPacketEditor.DecoderInfo]')
$all = [Activator]::CreateInstance($diListType)
$all.Add($d2)
$xmlM = $config.GetMethods() | Where-Object { $_.Name -eq 'GetDecoderList_XML' -and $_.GetParameters().Count -eq 1 }
$allArg = New-Object object[] 1; $allArg[0] = $all
$xe = $xmlM.Invoke($null, $allArg)
Check 'XML export root' (([string]$xe.Name) -eq 'Decoders') ("root=" + $xe.Name)
Check 'XML export one node' ($xe.Elements('Decoder').Count -eq 1) ''

$lst.Clear()
$xd = [System.Xml.Linq.XDocument]::new($xe)
$xdArg = New-Object object[] 1; $xdArg[0] = $xd
$config.GetMethod('LoadDecoderList_FromXDocument').Invoke($null, $xdArg) | Out-Null
Check 'XML import appends' ($lst.Count -eq 1) ("count=" + $lst.Count)
Check 'XML import name' (([string]$lst[0].Name) -eq 'b') ("name=" + $lst[0].Name)

# ── 顺序动作（置顶 / 上移 / 下移 / 置底；与滤镜 / 发送 / 机器人 / 仓库同一套语义）──
$actType = $asm.GetType('WinsockPacketEditor.Operate+SystemConfig+ListAction')

function Invoke-Move([string]$action, $list) {
    $a = New-Object object[] 2
    $a[0] = [enum]::Parse($actType, $action)
    $a[1] = $list
    $t = $config.GetMethod('UpdateDecoderList_ByListAction').Invoke($null, $a)
    $t.Wait()
}

$lst.Clear()
$m1 = [Activator]::CreateInstance($diType); $m1.GUID = [Guid]::NewGuid(); $m1.Name = 'a'
$m2 = [Activator]::CreateInstance($diType); $m2.GUID = [Guid]::NewGuid(); $m2.Name = 'b'
$m3 = [Activator]::CreateInstance($diType); $m3.GUID = [Guid]::NewGuid(); $m3.Name = 'c'
$config.GetMethod('AddDecoder').Invoke($null, @($m1)) | Out-Null
$config.GetMethod('AddDecoder').Invoke($null, @($m2)) | Out-Null
$config.GetMethod('AddDecoder').Invoke($null, @($m3)) | Out-Null

$pick2 = [Activator]::CreateInstance($diListType); $pick2.Add($m2)
Invoke-Move 'Up' $pick2
Check 'move up reorders' (([string]$lst[0].Name) -eq 'b' -and ([string]$lst[1].Name) -eq 'a') ("order=" + (($lst | ForEach-Object { $_.Name }) -join ''))

Invoke-Move 'Down' $pick2
Check 'move down restores' (([string]$lst[0].Name) -eq 'a' -and ([string]$lst[1].Name) -eq 'b') ("order=" + (($lst | ForEach-Object { $_.Name }) -join ''))

$pick3 = [Activator]::CreateInstance($diListType); $pick3.Add($m3)
Invoke-Move 'Top' $pick3
Check 'move top' (([string]$lst[0].Name) -eq 'c') ("order=" + (($lst | ForEach-Object { $_.Name }) -join ''))

Invoke-Move 'Bottom' $pick3
Check 'move bottom' (([string]$lst[2].Name) -eq 'c') ("order=" + (($lst | ForEach-Object { $_.Name }) -join ''))

$lst.Clear()

Write-Host ""
Write-Host ("PASS=" + $pass + "  FAIL=" + $fail) -ForegroundColor $(if ($fail -eq 0) { 'Green' } else { 'Red' })
if ($fail -gt 0) { exit 1 } else { exit 0 }
