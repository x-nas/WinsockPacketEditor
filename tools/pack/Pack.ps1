<#
    WPE x64 单文件打包

    用法（在仓库根目录 WinsockPacketEditor\ 下）：
        powershell -ExecutionPolicy Bypass -File tools\pack\Pack.ps1
        powershell -ExecutionPolicy Bypass -File tools\pack\Pack.ps1 -SkipBuild   # 直接用现有的 bin\Release

    流程：npm run build → MSBuild 解决方案 Release → WPELauncher\New-LauncherPackage.ps1
    输出：dist\WPE64 v<版本>.exe（首次运行解压到 %LOCALAPPDATA%\WPE64\app\<版本>-<哈希>\）
          文件名里的 v 由 -VersionPrefix 传下去，只影响输出文件名（payload.txt 的 Version 仍是 2.3）
#>
[CmdletBinding()]
param(
    [switch]$SkipBuild
)

$ErrorActionPreference = 'Stop'

$Repo = (Resolve-Path (Join-Path $PSScriptRoot '..\..')).Path
$Main = Join-Path $Repo 'WinsockPacketEditor'

function Find-MSBuild {
    $vswhere = Join-Path ([Environment]::GetFolderPath('ProgramFilesX86')) 'Microsoft Visual Studio\Installer\vswhere.exe'
    $vs = & $vswhere -latest -products * -requires Microsoft.Component.MSBuild -property installationPath
    return (Join-Path $vs 'MSBuild\Current\Bin\MSBuild.exe')
}

if (-not $SkipBuild) {
    Write-Host '== WebUI · npm run build' -ForegroundColor Cyan
    Push-Location (Join-Path $Main 'WebUI')
    try {
        & npm run build
        if ($LASTEXITCODE -ne 0) { throw "npm run build 失败（$LASTEXITCODE）" }
    } finally { Pop-Location }

    Write-Host '== MSBuild · WinSockPacketEditor.sln · Release' -ForegroundColor Cyan
    & (Find-MSBuild) (Join-Path $Repo 'WinSockPacketEditor.sln') -restore -p:Configuration=Release -m -v:minimal -nologo
    if ($LASTEXITCODE -ne 0) { throw "MSBuild 失败（$LASTEXITCODE）" }
}

# MCP sidecar is intentionally a separate .NET 10 process. Publish it into the
# application payload rather than linking it into the net48 WPE executable.
Write-Host '== MCP · dotnet publish · Release' -ForegroundColor Cyan
$McpProject = Join-Path $Repo 'WPEMcpServer\WPEMcpServer.csproj'
$McpOutput = Join-Path $Main 'bin\Release\McpServer'
# 单文件发布不会自动删除旧的 framework-dependent 旁车文件，先仅清理这个生成目录。
if (Test-Path -LiteralPath $McpOutput) { Remove-Item -LiteralPath $McpOutput -Recurse -Force }
New-Item -ItemType Directory -Path $McpOutput -Force | Out-Null
& dotnet publish $McpProject --no-restore -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -o $McpOutput
if ($LASTEXITCODE -ne 0) { throw "MCP Server 发布失败（$LASTEXITCODE）" }
Remove-Item -LiteralPath (Join-Path $McpOutput 'WPEMcpServer.pdb') -Force -ErrorAction SilentlyContinue

& (Join-Path $Repo 'WPELauncher\New-LauncherPackage.ps1') `
    -SourceDir (Join-Path $Main 'bin\Release') `
    -DistDir (Join-Path $Repo 'dist') `
    -Name 'WPE64' -Title 'WPE x64' -Exe 'WinsockPacketEditor.exe' `
    -OutBaseName 'WPE64' -LauncherAssembly 'WPE64' -VersionPrefix 'v' `
    -Icon (Join-Path $Main 'wpe.ico') `
    -AllowedExe @('EasyHook32Svc.exe', 'EasyHook64Svc.exe', 'SuperSocket.SocketService.exe', 'wpe-mihomo.exe') `
    -ExcludeRx @(
        '^WebView2\\',          # 本机运行过留下的 WebView2 用户数据
        '\.WebView2\\',         # 跑测探针的 WebView2 数据目录
        '\.pdb$', '\.xml$', '\.vshost\.', '^Logs\\', '^B10-acceptance\.txt$',
        '^[^\\]+\.exe\.config$(?<!WinsockPacketEditor\.exe\.config)(?<!SuperSocket\.SocketService\.exe\.config)'
    ) `
    -Required @(
        'WinsockPacketEditor.exe.config', 'WPEHook.dll',
        'EasyHook.dll', 'EasyHook32.dll', 'EasyHook64.dll', 'EasyLoad32.dll', 'EasyLoad64.dll',
        'EasyHook32Svc.exe', 'EasyHook64Svc.exe',
        'x64\SQLite.Interop.dll', 'x86\SQLite.Interop.dll',
        'wpe-mihomo.exe',
        'SuperSocket.SocketEngine.dll', 'Microsoft.Owin.Host.HttpListener.dll',
        'runtimes\win-x64\native\WebView2Loader.dll',
        'IPLocation\qqwry.dat', 'Web\index.html', 'wwwroot\index.html', 'wpe-data.ico',
        'McpServer\WPEMcpServer.exe'
    )
if (-not $?) { exit 1 }
