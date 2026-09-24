$ErrorActionPreference = 'Stop'
$bin = 'C:\Users\Gary\Desktop\程序源代码\x-nas\WinsockPacketEditor\WinsockPacketEditor\bin\Debug'
Set-Location $bin
[Environment]::CurrentDirectory = $bin

# ---- SQLite provider（2026-09-24 起是 Microsoft.Data.Sqlite + SQLitePCLRaw）----
# 跑测在 PowerShell 进程里加载主程序集并调用 DataBase，所以 provider 的装载要在 PS 里成立：
#  ① SQLitePCLRaw.core 是强命名：Microsoft.Data.Sqlite 引用 2.1.12，输出里是 2.1.13
#     （主程序靠自动生成的 binding redirect 接上），PowerShell 没有重定向 ——
#     用 AssemblyResolve 把请求指回输出目录。
#  ② e_sqlite3.dll 是原生库，在 runtimes\win-<arch>\native\；PS 进程的 BaseDirectory 不是
#     输出目录，先在输出目录里预加载一次，之后按名加载就会命中已加载的模块。
Add-Type -Namespace WpeTest -Name Native -MemberDefinition '[DllImport("kernel32", SetLastError=true, CharSet=CharSet.Unicode)] public static extern IntPtr LoadLibraryW(string path);' -ErrorAction SilentlyContinue
$nativeDir = Join-Path $bin ("runtimes\win-{0}\native" -f $(if ([Environment]::Is64BitProcess) { 'x64' } else { 'x86' }))
[WpeTest.Native]::LoadLibraryW((Join-Path $nativeDir 'e_sqlite3.dll')) | Out-Null

$global:WpeResolveBin = $bin
[AppDomain]::CurrentDomain.add_AssemblyResolve({
    param($sender, $e)
    try {
        $name = (New-Object Reflection.AssemblyName $e.Name).Name
        $path = Join-Path $global:WpeResolveBin ($name + '.dll')
        if (Test-Path $path) { return [Reflection.Assembly]::LoadFrom($path) }
    } catch { }
    return $null
}) | Out-Null
[Reflection.Assembly]::LoadFrom((Join-Path $bin 'Microsoft.Data.Sqlite.dll')) | Out-Null

# 合并 WPEHybrid 之后主程序集是 WinsockPacketEditor.exe（不再有独立的 .dll）
$asmFile = Join-Path $bin 'WinsockPacketEditor.exe'
if (-not (Test-Path $asmFile)) { $asmFile = Join-Path $bin 'WinsockPacketEditor.dll' }
$asm = [Reflection.Assembly]::LoadFrom($asmFile)
$op  = $asm.GetType('WinsockPacketEditor.Operate')
$sc  = $op.GetNestedType('SystemConfig')
$db  = $op.GetNestedType('DataBase')

function F($t, $n) { $t.GetField($n, 'Public,Static,NonPublic') }
function Get-V($t, $n) { (F $t $n).GetValue($null) }
function Set-V($t, $n, $v) { (F $t $n).SetValue($null, $v) }

# 全新空库
$dir = Join-Path $env:TEMP ('wpe-lastinj-' + [Guid]::NewGuid().ToString('N').Substring(0, 8))
New-Item -ItemType Directory -Path $dir | Out-Null
Set-V $db 'dbPath' $dir
$db.GetMethod('InitDB').Invoke($null, @()) | Out-Null

$load = $sc.GetMethod('LoadSystemConfig_FromDB')
$saveAll = $sc.GetMethod('SaveSystemConfig_ToDB')
$saveLast = $sc.GetMethod('SaveSystemConfig_LastInjection_ToDB')

$load.Invoke($null, @()) | Out-Null

$r = @()
function T($name, $ok, $got) { $script:r += [pscustomobject]@{ 项 = $name; 结果 = $(if ($ok) { 'PASS' } else { 'FAIL' }); 实测 = $got } }

# ① 空库默认：还没注入过
T '空库 · 目标为空'  ((Get-V $sc 'LastInjection') -eq '') ("'" + (Get-V $sc 'LastInjection') + "'")
T '空库 · 方式为 0'  ((Get-V $sc 'LastInjectMethod') -eq 0) (Get-V $sc 'LastInjectMethod')
T '空库 · 时间为空'  ((Get-V $sc 'LastInjectTime') -eq '') ("'" + (Get-V $sc 'LastInjectTime') + "'")

# ② 只调「保存上次注入」那一条 —— 这正是原来会抛「no such column」的那句
Set-V $sc 'LastInjection' 'game.exe'
Set-V $sc 'LastInjectMethod' 2
Set-V $sc 'LastInjectPath' 'D:\Games\game.exe'
Set-V $sc 'LastInjectArgs' '-windowed -novid'
Set-V $sc 'LastInjectTime' ([DateTime]'2026-09-10T15:42:07').ToString('o')
$saveLast.Invoke($null, @()) | Out-Null

# 把内存抹掉，只能从库里读回来
Set-V $sc 'LastInjection' ''
Set-V $sc 'LastInjectMethod' 0
Set-V $sc 'LastInjectPath' ''
Set-V $sc 'LastInjectArgs' ''
Set-V $sc 'LastInjectTime' ''
$load.Invoke($null, @()) | Out-Null

T 'UpdateTable 落库 · 目标' ((Get-V $sc 'LastInjection') -eq 'game.exe') (Get-V $sc 'LastInjection')
T 'UpdateTable 落库 · 方式' ((Get-V $sc 'LastInjectMethod') -eq 2) (Get-V $sc 'LastInjectMethod')
T 'UpdateTable 落库 · 路径' ((Get-V $sc 'LastInjectPath') -eq 'D:\Games\game.exe') (Get-V $sc 'LastInjectPath')
T 'UpdateTable 落库 · 参数' ((Get-V $sc 'LastInjectArgs') -eq '-windowed -novid') (Get-V $sc 'LastInjectArgs')
T 'UpdateTable 落库 · 时间' ((Get-V $sc 'LastInjectTime') -like '2026-09-10T15:42:07*') (Get-V $sc 'LastInjectTime')

# ③ 整表保存那条路也要带上四个新列
Set-V $sc 'LastInjection' 'notepad'
Set-V $sc 'LastInjectMethod' 1
Set-V $sc 'LastInjectPath' 'C:\Windows\notepad.exe'
Set-V $sc 'LastInjectArgs' ''
Set-V $sc 'LastInjectTime' ([DateTime]'2026-09-09T08:00:00').ToString('o')
$saveAll.Invoke($null, @()) | Out-Null
Set-V $sc 'LastInjection' ''
Set-V $sc 'LastInjectMethod' 0
$load.Invoke($null, @()) | Out-Null

T '整表保存 · 目标' ((Get-V $sc 'LastInjection') -eq 'notepad') (Get-V $sc 'LastInjection')
T '整表保存 · 方式' ((Get-V $sc 'LastInjectMethod') -eq 1) (Get-V $sc 'LastInjectMethod')

# ④ 老库（没有这四列）经 EnsureColumn 补上，且不抛
$old = Join-Path $env:TEMP ('wpe-oldlib-' + [Guid]::NewGuid().ToString('N').Substring(0, 8))
New-Item -ItemType Directory -Path $old | Out-Null
$dbFile = Join-Path $old ((Get-V $db 'dbName'))
$cn = New-Object Microsoft.Data.Sqlite.SqliteConnection ("Data Source=" + $dbFile)
$cn.Open()
$cmd = $cn.CreateCommand()
$cmd.CommandText = "CREATE TABLE SystemConfig (IsAnimation BOOLEAN, IsShadowEnabled BOOLEAN, IsShowInWindow BOOLEAN, IsScrollBarHide BOOLEAN, IsTextRenderingHighQuality BOOLEAN, IsDark BOOLEAN, DefaultLanguage TEXT, LastInjection TEXT);"
$cmd.ExecuteNonQuery() | Out-Null
$cn.Close()

Set-V $db 'dbPath' $old
$db.GetMethod('InitDB').Invoke($null, @()) | Out-Null

$cn2 = New-Object Microsoft.Data.Sqlite.SqliteConnection ("Data Source=" + $dbFile)
$cn2.Open()
$c2 = $cn2.CreateCommand()
$c2.CommandText = "PRAGMA table_info(SystemConfig);"
$rd = $c2.ExecuteReader()
$cols = @()
while ($rd.Read()) { $cols += [string]$rd['name'] }
$rd.Close(); $cn2.Close()

foreach ($c in @('LastInjectMethod', 'LastInjectPath', 'LastInjectArgs', 'LastInjectTime')) {
    T ('老库补列 · ' + $c) ($cols -contains $c) ($cols -contains $c)
}

$r | Format-Table -AutoSize
$bad = @($r | Where-Object { $_.结果 -eq 'FAIL' })
Write-Host ("`n" + $r.Count + " 项，失败 " + $bad.Count)
Remove-Item -Recurse -Force $dir, $old -ErrorAction SilentlyContinue
