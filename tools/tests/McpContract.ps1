[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
$repo = (Resolve-Path (Join-Path $PSScriptRoot '..\..')).Path
$csharpIcons = Get-Content (Join-Path $repo 'WinsockPacketEditor\ClassObject\Ui\IUiHost.cs') -Raw
$tsIcons = Get-Content (Join-Path $repo 'WinsockPacketEditor\WebUI\src\bridge\host.ts') -Raw
$expectedIconNames = @('None', 'Info', 'Success', 'Warn', 'Error')
$csharpBlock = [regex]::Match($csharpIcons, 'public enum UiIcon\s*\{(?<body>.*?)\}', [Text.RegularExpressions.RegexOptions]::Singleline).Groups['body'].Value
$csharpNames = [regex]::Matches($csharpBlock, '(?m)^\s*(None|Info|Success|Warn|Error)\s*,') | ForEach-Object { $_.Groups[1].Value }
$tsNames = [regex]::Matches($tsIcons, '(?m)^\s*(None|Info|Success|Warn|Error)\s*=\s*\d+') | ForEach-Object { $_.Groups[1].Value }
if (($csharpNames -join ',') -ne ($expectedIconNames -join ',') -or ($tsNames -join ',') -ne ($expectedIconNames -join ',')) {
    throw 'UiIcon mapping mismatch: expected None, Info, Success, Warn, Error in both C# and TypeScript.'
}
$toolsSource = Get-Content (Join-Path $repo 'WPEMcpServer\WpeTools.cs') -Raw
$toolNames = [regex]::Matches($toolsSource, 'Name\s*=\s*"(wpe_[a-z0-9_]+)"') |
    ForEach-Object { $_.Groups[1].Value } | Sort-Object -Unique

if ($toolNames.Count -eq 0) { throw 'MCP tool registry is empty.' }

$docs = Get-Content (Join-Path $repo 'docs\mcp\tools.md') -Raw
$schemaNames = @()
foreach ($schemaPath in @('docs\mcp\schemas\readonly-tools.schema.json', 'docs\mcp\schemas\write-tools.schema.json')) {
    $schema = Get-Content (Join-Path $repo $schemaPath) -Raw | ConvertFrom-Json
    $schemaNames += $schema.tools.psobject.Properties.Name
}
$schemaNames = $schemaNames | Sort-Object -Unique

$missingDocs = @($toolNames | Where-Object { $docs -notmatch [regex]::Escape($_) })
$missingSchemas = @($toolNames | Where-Object { $_ -notin $schemaNames })

if ($missingDocs.Count -gt 0) { throw ('Tools missing from docs/mcp/tools.md: ' + ($missingDocs -join ', ')) }
if ($missingSchemas.Count -gt 0) { throw ('Tools missing from MCP schemas: ' + ($missingSchemas -join ', ')) }

Write-Host ('MCP contract check: PASS ({0} tools, docs, schemas and UiIcon mapping synchronized).' -f $toolNames.Count)
