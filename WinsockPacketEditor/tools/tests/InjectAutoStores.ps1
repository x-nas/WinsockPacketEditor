# 注入模式的「自动入库」跑测
#
# 起因：Enable_AutoStores 全项目原来只在 ProxyConfig.List.FlushToFeed 判一次，
#       注入那份 PacketConfig.List.FlushToFeed 里没有 —— 于是注入模式下
#       「自动入库」的总开关与规则表全都能开能填，一条都不会命中，而界面上毫无提示。
#
# ⚠️ 这份脚本在<b>改动前的构建</b>上第 ① 项必然失败（仓库里 0 条），那才是它的价值。
#
# 用法（必须在 bin 目录下跑 —— ProxyConfig.Proxy 的静态构造按工作目录找 IPLocation/qqwry.dat）：
#   powershell -ExecutionPolicy Bypass -File tools\tests\InjectAutoStores.ps1
#
# ⚠️ 本文件要存成 UTF-8 带 BOM，否则 PowerShell 5.1 按 ANSI 读，路径里的中文会烂掉。

# -Config <名字> 指向 bin 下的另一次构建。
#
# 2026-09-10 取反证时就是这么用的：那会儿 binRelease 还是<b>改动前</b>那一份，
# 第 ① 项在它上面 FAIL（仓储 0 -> 0）、在 Debug 上 PASS（0 -> 1）——
# 「在旧代码上不会失败的断言等于没写」。
# ⚠️ 之后 Release 也重新编过了，这条反证路<b>现在已经不成立</b>；
#    要再取一次证，得先把改动前的那份 Operate.cs 编到另一个目录去。
param([string]$Config = "Debug")

$ErrorActionPreference = 'Stop'

$bin = Join-Path $PSScriptRoot ('..\..\bin\' + $Config)
$bin = [System.IO.Path]::GetFullPath($bin)
if (-not (Test-Path (Join-Path $bin 'WinsockPacketEditor.exe'))) {
    throw "找不到 $bin\WinsockPacketEditor.exe —— 先用 VS 的 MSBuild 编一次 Debug"
}

# ⚠️ Set-Location 不改 .NET 侧的工作目录，两句都要写
Set-Location $bin
[Environment]::CurrentDirectory = $bin

$asm = [Reflection.Assembly]::LoadFrom((Join-Path $bin 'WinsockPacketEditor.exe'))
$op = $asm.GetType('WinsockPacketEditor.Operate')

$B = [Reflection.BindingFlags]'Public,NonPublic,Static'
function T($name) { $asm.GetType("WinsockPacketEditor.Operate+$name") }

$packetList = T 'PacketConfig+List'
$whCfg      = T 'WareHouseConfig+WareHouse'
$whList     = T 'WareHouseConfig+List'
$pktQueue   = T 'PacketConfig+Queue'

$pass = 0; $fail = 0
function Check($name, $ok, $detail) {
    if ($ok) { $script:pass++; Write-Host ("  [PASS] {0}  {1}" -f $name, $detail) }
    else     { $script:fail++; Write-Host ("  [FAIL] {0}  {1}" -f $name, $detail) -ForegroundColor Red }
}

Write-Host "`n=== 注入模式 · 自动入库 ===`n"

# ── 备一个仓库 + 一条规则（包头 AA BB）──────────────────────────────
$whInfoType = $asm.GetType('WinsockPacketEditor.WareHouseInfo')
$asInfoType = $asm.GetType('WinsockPacketEditor.AutoStoresInfo')

$wid = [Guid]::NewGuid()

# ⚠️ 第三个参数是 Stores 那份 BindingList<DataInfo> —— 传 null 的话 AddStores 无处可放，
#    第 ① 项会报 FAIL 而看着像「产品代码没生效」。第一版就是这么把自己骗了一回。
$diType = $asm.GetType('WinsockPacketEditor.DataInfo')
$blType = [System.ComponentModel.BindingList`1].MakeGenericType($diType)
$stores = [Activator]::CreateInstance($blType)
$whObj = $whInfoType.GetConstructor(@([Guid], [string], $blType)).Invoke(@($wid, '跑测仓库', $stores))

$lstWh = $whList.GetField('lstWareHouseInfo', $B).GetValue($null)
$lstWh.Clear()
$lstWh.Add($whObj) | Out-Null

$lstAs = $whList.GetField('lstAutoStoresInfo', $B).GetValue($null)
$lstAs.Clear()
$lstAs.Add($asInfoType.GetConstructor(@([bool], [string], [Guid])).Invoke(@($true, 'AA BB', $wid))) | Out-Null

$whCfg.GetField('Enable_AutoStores', $B).SetValue($null, $true)

# ── 造两条封包：一条命中包头、一条不命中 ────────────────────────────
$piType = $asm.GetType('WinsockPacketEditor.PacketInfo')
$ctor = $piType.GetConstructors() | Where-Object { $_.GetParameters().Count -eq 12 } | Select-Object -First 1
$ptType = $asm.GetType('WinsockPacketEditor.Operate+PacketConfig+Packet+PacketType')
$faType = $asm.GetType('WinsockPacketEditor.Operate+FilterConfig+Filter+FilterAction')

function NewPacket([byte[]]$bytes) {
    $ctor.Invoke(@(
        [DateTime]::Now, 1,
        [Enum]::ToObject($ptType, 1),
        '127.0.0.1:1', '', '127.0.0.1:2', '',
        $bytes, $bytes, '', $bytes.Length,
        [Enum]::ToObject($faType, 0)
    ))
}

$q = $pktQueue.GetField('cqPacketInfo', $B).GetValue($null)
while ($q.Count -gt 0) { $null = $q.TryDequeue([ref]$null) }

$q.Enqueue((NewPacket ([byte[]](0xAA, 0xBB, 0x01, 0x02))))   # 命中
$q.Enqueue((NewPacket ([byte[]](0xCC, 0xDD, 0x03, 0x04))))   # 不命中

$storesBefore = $whObj.Stores.Count

# ── 搬一拍 ──────────────────────────────────────────────────────────
$packetList.GetMethod('FlushToFeed', $B).Invoke($null, $null) | Out-Null

$storesAfter = $whObj.Stores.Count

Check '① 命中包头的那条进了仓库' ($storesAfter -eq ($storesBefore + 1)) `
    ("仓储 {0} -> {1}（期望 +1，改动前是 +0）" -f $storesBefore, $storesAfter)

# ── 总开关关掉就不该再进 ────────────────────────────────────────────
$whCfg.GetField('Enable_AutoStores', $B).SetValue($null, $false)
$q.Enqueue((NewPacket ([byte[]](0xAA, 0xBB, 0x05, 0x06))))
$packetList.GetMethod('FlushToFeed', $B).Invoke($null, $null) | Out-Null

Check '② 总开关关掉就不入库' ($whObj.Stores.Count -eq $storesAfter) `
    ("仓储仍是 {0}" -f $whObj.Stores.Count)

# ── 规则停用也不该进 ────────────────────────────────────────────────
$whCfg.GetField('Enable_AutoStores', $B).SetValue($null, $true)
$lstAs[0].IsEnable = $false
$q.Enqueue((NewPacket ([byte[]](0xAA, 0xBB, 0x07, 0x08))))
$packetList.GetMethod('FlushToFeed', $B).Invoke($null, $null) | Out-Null

Check '③ 规则停用就不入库' ($whObj.Stores.Count -eq $storesAfter) `
    ("仓储仍是 {0}" -f $whObj.Stores.Count)

Write-Host ("`n通过 {0} · 失败 {1}`n" -f $pass, $fail)
if ($fail -gt 0) { exit 1 }
