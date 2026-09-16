<#
    WPE x64 单文件打包

    用法（在仓库根目录 WinsockPacketEditor\ 下）：
        powershell -ExecutionPolicy Bypass -File tools\pack\Pack.ps1
        powershell -ExecutionPolicy Bypass -File tools\pack\Pack.ps1 -SkipBuild   # 直接用现有的 bin\Release

    流程：npm run build → MSBuild 解决方案 Release → WPELauncher\New-LauncherPackage.ps1
    输出：dist\WPE64 <版本>.exe（首次运行解压到 %LOCALAPPDATA%\WPE64\app\<版本>-<哈希>\）
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
& dotnet publish $McpProject --no-restore -c Release -o $McpOutput
if ($LASTEXITCODE -ne 0) { throw "MCP Server 发布失败（$LASTEXITCODE）" }

& (Join-Path $Repo 'WPELauncher\New-LauncherPackage.ps1') `
    -SourceDir (Join-Path $Main 'bin\Release') `
    -DistDir (Join-Path $Repo 'dist') `
    -Name 'WPE64' -Title 'WPE x64' -Exe 'WinsockPacketEditor.exe' `
    -OutBaseName 'WPE64' -LauncherAssembly 'WPE64' `
    -Icon (Join-Path $Main 'wpe.ico') `
    -AllowedExe @('EasyHook32Svc.exe', 'EasyHook64Svc.exe', 'SuperSocket.SocketService.exe') `
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
        'x64\SunnyNet64.dll', 'x64\SQLite.Interop.dll', 'x86\SQLite.Interop.dll',
        'SuperSocket.SocketEngine.dll', 'Microsoft.Owin.Host.HttpListener.dll',
        'runtimes\win-x64\native\WebView2Loader.dll',
        'IPLocation\qqwry.dat', 'Web\index.html', 'wwwroot\index.html', 'wpe-data.ico',
        'McpServer\WPEMcpServer.exe', 'McpServer\WPEMcpServer.dll',
        'McpServer\WPEMcpServer.deps.json', 'McpServer\WPEMcpServer.runtimeconfig.json'
    )
if (-not $?) { exit 1 }
