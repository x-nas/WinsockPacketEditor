<#
    把一个已经构建好的程序目录打成「单文件启动器」—— WPE x64 与 WPE Proxy Cap 共用。

    由各产品的 tools\pack\Pack.ps1 调用，自己不负责构建产品，只做：
        ① 从 -SourceDir 挑文件（按 -ExcludeRx 排除、顶层 exe 只留 -AllowedExe）
        ② 校验 -Required 里的文件都在
        ③ 打 payload.zip + payload.txt（Version / Hash / Exe / Name / Title / Files / Bytes）
        ④ 构建本目录的 WPELauncher.csproj，把载荷嵌进去
        ⑤ 输出 <DistDir>\<OutBaseName> <-VersionPrefix><版本>.exe 及其 .sha256.txt

    用户双击那个 exe：第一次解压到 %LOCALAPPDATA%\<Name>\app\<版本>-<哈希>\，之后直接启动 <Exe>。
#>
[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)][string]$SourceDir,
    [Parameter(Mandatory = $true)][string]$DistDir,
    [Parameter(Mandatory = $true)][string]$Name,
    [Parameter(Mandatory = $true)][string]$Title,
    [Parameter(Mandatory = $true)][string]$Exe,
    [Parameter(Mandatory = $true)][string]$OutBaseName,
    [Parameter(Mandatory = $true)][string]$Icon,
    [string]$LauncherAssembly = 'Launcher',
    # 只加在「输出的 exe 文件名」上（WPE 传 'v' → WPE64 v2.4.exe）。
    # payload.txt 的 Version 与解压目录 <版本>-<哈希> 不带它：那是版本号，不是给人看的名字。
    [string]$VersionPrefix = '',
    [string[]]$AllowedExe = @(),
    [string[]]$ExcludeRx = @(),
    [string[]]$Required = @()
)

$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.IO.Compression
Add-Type -AssemblyName System.IO.Compression.FileSystem

function Step([string]$text) { Write-Host ''; Write-Host "== $text" -ForegroundColor Cyan }

# 不依赖 Get-FileHash：某些由 cmd 启动的旧 PowerShell 环境不能解析该 cmdlet。
# 用 BCL 保持 Windows PowerShell 5.1 与 PowerShell 7 的一致性。
function Get-Sha256([string]$path) {
    $stream = [IO.File]::OpenRead($path)
    try {
        $sha = [Security.Cryptography.SHA256]::Create()
        try { return ([BitConverter]::ToString($sha.ComputeHash($stream))).Replace('-', '').ToLowerInvariant() }
        finally { $sha.Dispose() }
    } finally { $stream.Dispose() }
}

function Find-MSBuild {
    $vswhere = Join-Path ([Environment]::GetFolderPath('ProgramFilesX86')) 'Microsoft Visual Studio\Installer\vswhere.exe'
    if (-not (Test-Path $vswhere)) { throw "找不到 vswhere：$vswhere" }
    $vs = & $vswhere -latest -products * -requires Microsoft.Component.MSBuild -property installationPath
    $msb = Join-Path $vs 'MSBuild\Current\Bin\MSBuild.exe'
    if (-not (Test-Path $msb)) { throw "找不到 MSBuild：$msb" }
    return $msb
}

$SourceDir = (Resolve-Path $SourceDir).Path.TrimEnd('\')
$Icon = (Resolve-Path $Icon).Path
New-Item -ItemType Directory -Force -Path $DistDir | Out-Null
$DistDir = (Resolve-Path $DistDir).Path.TrimEnd('\')

$MainExe = Join-Path $SourceDir $Exe
if (-not (Test-Path -LiteralPath $MainExe)) { throw "没有 $MainExe，先构建产品" }

$Version = [Diagnostics.FileVersionInfo]::GetVersionInfo($MainExe).FileVersion
if ([string]::IsNullOrEmpty($Version)) { throw "$Exe 没有文件版本" }

# 两段式版本号（WPE 2.2 / WPC 1.0 起）：去掉末尾为 0 的段，但至少留「主.次」—— 2.2.0.0 → 2.2、1.0.0.0 → 1.0、2.2.1.0 → 2.2.1
# 输出文件名、进度窗标题、解压目录（<版本>-<哈希12>）都用它；启动器自己的文件版本仍写四段的 $Version
$parts = @($Version.Split('.'))
while ($parts.Count -gt 2 -and $parts[-1] -eq '0') { $parts = @($parts[0..($parts.Count - 2)]) }
$short = $parts -join '.'

#region 挑文件 · 打 payload.zip
Step "打 payload.zip · $Title $short"

$AllowedExe = @($AllowedExe + $Exe | Select-Object -Unique)
$prefix = $SourceDir + '\'
$files = New-Object System.Collections.Generic.List[object]
$skipped = New-Object System.Collections.Generic.List[string]

foreach ($f in Get-ChildItem -LiteralPath $SourceDir -Recurse -File) {
    $rel = $f.FullName.Substring($prefix.Length)
    $drop = $false

    foreach ($rx in $ExcludeRx) { if ($rel -match $rx) { $drop = $true; break } }

    # 顶层 exe 只留白名单：跑测探针、createdump 之类不打包
    if (-not $drop -and $rel -notmatch '\\' -and $f.Extension -eq '.exe' -and $AllowedExe -notcontains $f.Name) { $drop = $true }

    if ($drop) { $skipped.Add($rel) } else { $files.Add([pscustomobject]@{ Rel = $rel; File = $f }) }
}

$have = @{}
foreach ($x in $files) { $have[$x.Rel.ToLowerInvariant()] = $true }
$missing = @(@($Required + $Exe) | Where-Object { -not $have.ContainsKey($_.ToLowerInvariant()) })
if ($missing.Count -gt 0) { throw ("程序目录缺少必需文件：`n  " + ($missing -join "`n  ")) }

if ($skipped.Count -gt 0) {
    Write-Host "排除 $($skipped.Count) 个文件：" -ForegroundColor DarkGray
    $skipped | Group-Object { ($_ -split '\\')[0] } | ForEach-Object { Write-Host "  $($_.Name)  ×$($_.Count)" -ForegroundColor DarkGray }
}

$PayDir = Join-Path $DistDir 'payload'
Remove-Item -LiteralPath $PayDir -Recurse -Force -ErrorAction SilentlyContinue
New-Item -ItemType Directory -Force -Path $PayDir | Out-Null

$ZipPath = Join-Path $PayDir 'payload.zip'
$sorted = $files | Sort-Object { $_.Rel } -CaseSensitive   # 固定顺序：同样的输入打出同样的条目顺序
$total = [long]0

$fs = [IO.File]::Open($ZipPath, [IO.FileMode]::Create)
try {
    $zip = New-Object IO.Compression.ZipArchive($fs, [IO.Compression.ZipArchiveMode]::Create)
    try {
        foreach ($x in $sorted) {
            $entry = $zip.CreateEntry($x.Rel.Replace('\', '/'), [IO.Compression.CompressionLevel]::Optimal)
            $entry.LastWriteTime = $x.File.LastWriteTime
            $dst = $entry.Open()
            try {
                $src = [IO.File]::OpenRead($x.File.FullName)
                try { $src.CopyTo($dst) } finally { $src.Dispose() }
            } finally { $dst.Dispose() }
            $total += $x.File.Length
        }
    } finally { $zip.Dispose() }
} finally { $fs.Dispose() }

$Hash = Get-Sha256 $ZipPath
$zipLen = (Get-Item -LiteralPath $ZipPath).Length

$info = @(
    "Version=$short",
    "Hash=$Hash",
    "Exe=$Exe",
    "Name=$Name",
    "Title=$Title",
    "Files=$($files.Count)",
    "Bytes=$total"
) -join "`r`n"
[IO.File]::WriteAllText((Join-Path $PayDir 'payload.txt'), $info, (New-Object Text.UTF8Encoding($false)))

Write-Host ("{0} 个文件 · 解压后 {1:N1} MB · zip {2:N1} MB · SHA256 {3}" -f $files.Count, ($total / 1MB), ($zipLen / 1MB), $Hash.Substring(0, 12))
#endregion

#region 构建启动器
Step '构建 WPELauncher'
$msb = Find-MSBuild
$LauncherOut = Join-Path $DistDir 'launcher'
& $msb (Join-Path $PSScriptRoot 'WPELauncher.csproj') -restore -t:Rebuild -p:Configuration=Release `
    "-p:PayloadDir=$PayDir\" "-p:OutDir=$LauncherOut\" "-p:AssemblyName=$LauncherAssembly" `
    "-p:LauncherIcon=$Icon" "-p:Product=$Title" "-p:AssemblyTitle=$Title" `
    "-p:Version=$Version" "-p:FileVersion=$Version" "-p:AssemblyVersion=$Version" "-p:InformationalVersion=$Version" `
    -v:minimal -nologo
if ($LASTEXITCODE -ne 0) { throw "构建 WPELauncher 失败（$LASTEXITCODE）" }
#endregion

#region 输出
Step '输出'
$built = Join-Path $LauncherOut "$LauncherAssembly.exe"
if (-not (Test-Path -LiteralPath $built)) { throw "没有 $built" }

$OutExe = Join-Path $DistDir "$OutBaseName $VersionPrefix$short.exe"
Copy-Item -LiteralPath $built -Destination $OutExe -Force

$outLen = (Get-Item -LiteralPath $OutExe).Length
$outHash = Get-Sha256 $OutExe
[IO.File]::WriteAllText("$OutExe.sha256.txt", "$outHash  $OutBaseName $VersionPrefix$short.exe`r`n", (New-Object Text.UTF8Encoding($false)))

Write-Host ("{0}  ({1:N2} MB)" -f $OutExe, ($outLen / 1MB)) -ForegroundColor Green
Write-Host "SHA256 $outHash"
#endregion
