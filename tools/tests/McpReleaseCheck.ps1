[CmdletBinding()]
param(
    [switch]$RequireWpe
)

$ErrorActionPreference = 'Stop'
$root = (Resolve-Path (Join-Path $PSScriptRoot '..\..')).Path
$checks = @(
    'McpContract.ps1',
    'McpToolsList.ps1'
)

foreach ($check in $checks) {
    Write-Host "== MCP $check" -ForegroundColor Cyan
    & powershell -ExecutionPolicy Bypass -File (Join-Path $PSScriptRoot $check)
    if ($LASTEXITCODE -ne 0) { throw "$check failed." }
}

$discovery = Join-Path ([Environment]::GetFolderPath('LocalApplicationData')) 'WPE64\mcp\instances.json'
if (Test-Path $discovery) {
    Write-Host '== MCP McpLifecycle.ps1' -ForegroundColor Cyan
    & powershell -ExecutionPolicy Bypass -File (Join-Path $PSScriptRoot 'McpLifecycle.ps1')
    if ($LASTEXITCODE -ne 0) { throw 'McpLifecycle.ps1 failed.' }
}
elseif ($RequireWpe) {
    throw 'WPE is required for lifecycle validation but no MCP discovery file exists.'
}
else {
    Write-Host '== MCP lifecycle skipped (WPE is not running).' -ForegroundColor Yellow
}

Write-Host 'MCP release check: PASS.' -ForegroundColor Green
