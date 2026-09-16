[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
$repo = (Resolve-Path (Join-Path $PSScriptRoot '..\..')).Path
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

Write-Host ('MCP contract check: PASS ({0} tools, docs and schemas synchronized).' -f $toolNames.Count)
