<#
    单文件启动器（WPELauncher / WPE64.exe）的跑测。

    启动器清单是 requireAdministrator，直接运行会弹 UAC —— 所以这里<b>反射进 exe 当程序集用</b>，
    只跑 Payload 那一层（读说明 / 解压 / 核对 / 修复 / 清理），根目录指到临时目录，不碰真的 %LOCALAPPDATA%。

    用法（先跑过 tools\pack\Pack.ps1）：
        powershell -ExecutionPolicy Bypass -File tools\tests\Launcher.ps1
#>
[CmdletBinding()]
param(
    # 默认验 WPE x64 的包；验 WPE Proxy Cap 的包时传：
    #   -Exe ..\WPEProxyCap.Hybrid\dist\launcher\WPCLauncher.exe -Bin ..\WPEProxyCap.Hybrid\App\publish
    [string]$Exe,
    [string]$Bin
)

$ErrorActionPreference = 'Stop'

$Repo = (Resolve-Path (Join-Path $PSScriptRoot '..\..')).Path
if (-not $Exe) { $Exe = Join-Path $Repo 'dist\launcher\WPE64.exe' }
if (-not $Bin) { $Bin = Join-Path $Repo 'WinsockPacketEditor\bin\Release' }
if (-not (Test-Path $Exe)) { throw "没有 $Exe，先跑 tools\pack\Pack.ps1" }
$Exe = (Resolve-Path $Exe).Path
$Bin = (Resolve-Path $Bin).Path

Add-Type -TypeDefinition @'
using System;
public sealed class ProgressSink : IProgress<int>
{
    public int Max = -1; public int Count;
    public void Report(int v) { Count++; if (v > Max) Max = v; }
}
'@

# 与 New-LauncherPackage.ps1 同一条口径：去掉末尾为 0 的段，至少留两段
function ShortVer([string]$v) {
    $p = @($v.Split('.'))
    while ($p.Count -gt 2 -and $p[-1] -eq '0') { $p = @($p[0..($p.Count - 2)]) }
    return ($p -join '.')
}

$pass = 0; $fail = 0
function Check([string]$name, [bool]$ok, [string]$detail = '') {
    if ($ok) { $script:pass++; Write-Host "  PASS  $name  $detail" -ForegroundColor Green }
    else     { $script:fail++; Write-Host "  FAIL  $name  $detail" -ForegroundColor Red }
}

$asm = [Reflection.Assembly]::LoadFrom($Exe)
$P = $asm.GetType('WPELauncher.Payload', $true)
$BF = [Reflection.BindingFlags]'Static,Public,NonPublic'
# ⚠️ Join-Path 这类 cmdlet 给的字符串是 PSObject 包着的，反射 Invoke 转不成 string —— 先解包
function Call([string]$m, [object[]]$a) {
    $args2 = New-Object 'object[]' $a.Length
    for ($i = 0; $i -lt $a.Length; $i++) {
        $v = $a[$i]
        if ($v -is [Management.Automation.PSObject]) { $v = $v.PSObject.BaseObject }
        $args2[$i] = $v
    }
    return $P.GetMethod($m, $BF).Invoke($null, $args2)
}

$Root = Join-Path ([IO.Path]::GetTempPath()) ('wpe64-launcher-test-' + [Guid]::NewGuid().ToString('N').Substring(0, 8))
New-Item -ItemType Directory -Path $Root | Out-Null

function HashesOf([string]$dir) {
    $h = @{}
    $full = (Resolve-Path $dir).Path.TrimEnd('\') + '\'
    foreach ($f in Get-ChildItem -LiteralPath $dir -Recurse -File) {
        $rel = $f.FullName.Substring($full.Length)
        if ($rel -eq '.wpe64-ready') { continue }
        $h[$rel] = (Get-FileHash -LiteralPath $f.FullName -Algorithm SHA256).Hash
    }
    return $h
}

try {
    Write-Host '== ① 载荷说明与 exe 自身'
    $info = Call 'ReadInfo' @()
    Check '读到 payload.txt' ($null -ne $info)
    Check '版本与主程序一致' ($info.Version -eq (ShortVer ([Diagnostics.FileVersionInfo]::GetVersionInfo((Join-Path $Bin $info.Exe)).FileVersion))) "$($info.Title) $($info.Version) · $($info.Exe)"
    Check '解压根目录名' ($info.Name.Length -gt 0 -and $info.Name.IndexOfAny([IO.Path]::GetInvalidFileNameChars()) -lt 0) $info.Name
    Check '启动器文件版本 = 载荷版本' ((ShortVer ([Diagnostics.FileVersionInfo]::GetVersionInfo($Exe).FileVersion)) -eq $info.Version)
    # 清单在 .rsrc 节里，排在几十 MB 的托管资源（载荷）之后 —— 要读整个文件
    $raw = [Text.Encoding]::ASCII.GetString([IO.File]::ReadAllBytes($Exe))
    Check '清单是 requireAdministrator' ($raw.Contains('level="requireAdministrator"'))
    $raw = $null
    Check '目录名 = 版本-哈希12' ($info.DirName -eq ($info.Version + '-' + $info.Hash.Substring(0, 12))) $info.DirName

    Write-Host '== ② 全新解压'
    $dir = Join-Path $Root $info.DirName
    Check '解压前不完整' (-not (Call 'IsIntact' @($dir, $info)))
    $sink = New-Object ProgressSink
    $sw = [Diagnostics.Stopwatch]::StartNew()
    $got = Call 'Extract' @($Root, $info, $sink)
    $sw.Stop()
    Check '返回的目录' ($got -eq $dir)
    Check '进度报到 100' ($sink.Max -eq 100) "上报 $($sink.Count) 次 · $([int]$sw.Elapsed.TotalMilliseconds) ms"
    Check '解压后完整' (Call 'IsIntact' @($dir, $info))
    $fileCount = @(Get-ChildItem -LiteralPath $dir -Recurse -File | Where-Object Name -ne '.wpe64-ready').Count
    Check '文件数 = 说明里的数' ($fileCount -eq $info.Files) "$fileCount"
    Check '没有留下临时目录' (@(Get-ChildItem -LiteralPath $Root -Directory -Filter '.tmp-*').Count -eq 0)

    # 逐文件与 bin\Release 比哈希（载荷本来就是从那儿打的）
    $hx = HashesOf $dir
    $mismatch = 0
    foreach ($k in $hx.Keys) {
        $src = Join-Path $Bin $k
        if (-not (Test-Path -LiteralPath $src) -or (Get-FileHash -LiteralPath $src -Algorithm SHA256).Hash -ne $hx[$k]) { $mismatch++ }
    }
    Check '每个文件与 bin\Release 逐字节相同' ($mismatch -eq 0) "不同 $mismatch 个"
    Check '没有带进 WebView2 缓存 / pdb' (-not (Test-Path (Join-Path $dir 'WebView2')) -and @(Get-ChildItem $dir -Filter *.pdb).Count -eq 0)

    $sw = [Diagnostics.Stopwatch]::StartNew()
    $null = Call 'IsIntact' @($dir, $info)
    $sw.Stop()
    Check '每次启动的核对耗时' ($sw.ElapsedMilliseconds -lt 1000) "$($sw.ElapsedMilliseconds) ms"

    Write-Host '== ③ 修复：截断一个、删掉一个'
    # 截断包里最大的那个文件（WPE 是 SunnyNet64.dll、WPC 是 wpe-mihomo.exe 之类），两个产品通用
    $bigRel = (Get-ChildItem -LiteralPath $dir -Recurse -File | Where-Object Name -ne '.wpe64-ready' |
        Sort-Object Length -Descending | Select-Object -First 1).FullName.Substring($dir.Length + 1)
    $qq = Join-Path $dir $bigRel
    $fs = [IO.File]::Open($qq, 'Open'); $fs.SetLength(1024); $fs.Dispose()
    Remove-Item -LiteralPath (Join-Path $dir 'wwwroot\index.html')
    Check '损坏之后不完整' (-not (Call 'IsIntact' @($dir, $info)))
    $bad = Call 'FindDamaged' @($dir)
    Check '找出 2 个坏文件' ($bad.Count -eq 2) (($bad | Sort-Object) -join ', ')
    $null = Call 'Extract' @($Root, $info, (New-Object ProgressSink))
    Check '修复后完整' (Call 'IsIntact' @($dir, $info))
    Check "$bigRel 内容恢复" ((Get-FileHash $qq -Algorithm SHA256).Hash -eq (Get-FileHash (Join-Path $Bin $bigRel) -Algorithm SHA256).Hash)

    Write-Host '== ④ 标记丢了：当成残留整体重来'
    Remove-Item -LiteralPath (Join-Path $dir '.wpe64-ready')
    Set-Content -LiteralPath (Join-Path $dir 'leftover.txt') -Value 'x'
    Check '无标记 → 不完整' (-not (Call 'IsIntact' @($dir, $info)))
    $null = Call 'Extract' @($Root, $info, (New-Object ProgressSink))
    Check '重新解压后完整' (Call 'IsIntact' @($dir, $info))
    Check '残留文件被清掉' (-not (Test-Path (Join-Path $dir 'leftover.txt')))

    Write-Host '== ⑤ 清理旧版本'
    $old = Join-Path $Root '2.1.8.0-aaaaaaaaaaaa'
    New-Item -ItemType Directory -Path (Join-Path $old 'wwwroot') -Force | Out-Null
    Copy-Item (Join-Path $env:WINDIR 'System32\ping.exe') (Join-Path $old 'WinsockPacketEditor.exe')
    Set-Content (Join-Path $old '.wpe64-ready') 'aaaaaaaaaaaa'

    $running = Join-Path $Root '2.1.7.0-bbbbbbbbbbbb'
    New-Item -ItemType Directory -Path $running | Out-Null
    $pingCopy = Join-Path $running 'wpe64testping.exe'
    Copy-Item (Join-Path $env:WINDIR 'System32\ping.exe') $pingCopy
    $proc = Start-Process -FilePath $pingCopy -ArgumentList '-n 30 127.0.0.1' -WindowStyle Hidden -PassThru
    Start-Sleep -Milliseconds 500

    $staleTmp = Join-Path $Root '.tmp-stale'
    $freshTmp = Join-Path $Root '.tmp-fresh'
    New-Item -ItemType Directory -Path $staleTmp, $freshTmp | Out-Null
    (Get-Item $staleTmp).LastWriteTimeUtc = [DateTime]::UtcNow.AddHours(-2)

    try {
        Check '在运行的目录判为占用' (Call 'IsInUse' @($running))
        Check '没在运行的目录不占用' (-not (Call 'IsInUse' @($old)))
        $removed = Call 'CleanupOld' @($Root, $info.DirName)
        Check '删掉 2 个（旧版本 + 过期临时目录）' ($removed -eq 2) "removed=$removed"
        Check '旧版本没了' (-not (Test-Path $old))
        Check '在运行的版本还在' (Test-Path $pingCopy)
        Check '过期临时目录没了' (-not (Test-Path $staleTmp))
        Check '新的临时目录留着（可能别的启动器正在解压）' (Test-Path $freshTmp)
        Check '当前版本还在且完整' (Call 'IsIntact' @($dir, $info))
    } finally {
        if (-not $proc.HasExited) { $proc.Kill(); $proc.WaitForExit() }
    }
}
finally {
    Remove-Item -LiteralPath $Root -Recurse -Force -ErrorAction SilentlyContinue
}

Write-Host ''
Write-Host ("{0} PASS · {1} FAIL" -f $pass, $fail) -ForegroundColor $(if ($fail) { 'Red' } else { 'Green' })
if ($fail) { exit 1 }
