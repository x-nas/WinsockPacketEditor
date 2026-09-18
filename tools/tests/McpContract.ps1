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
$unregisteredSchemas = @($schemaNames | Where-Object { $_ -notin $toolNames })

if ($missingDocs.Count -gt 0) { throw ('Tools missing from docs/mcp/tools.md: ' + ($missingDocs -join ', ')) }
if ($missingSchemas.Count -gt 0) { throw ('Tools missing from MCP schemas: ' + ($missingSchemas -join ', ')) }
if ($unregisteredSchemas.Count -gt 0) { throw ('MCP schemas contain tools not registered by the Sidecar: ' + ($unregisteredSchemas -join ', ')) }

# The Sidecar is allowed to discover a tool only when the in-process gateway
# can dispatch its corresponding operation. This prevents a newly published
# tool from failing only after a client calls it.
$sidecarOperations = [regex]::Matches($toolsSource, 'gateway\.InvokeAsync\("([a-z]+(?:\.[A-Za-z]+)+)"') |
    ForEach-Object { $_.Groups[1].Value } | Sort-Object -Unique
$gatewaySource = Get-Content (Join-Path $repo 'WinsockPacketEditor\Mcp\McpAgentGateway.cs') -Raw
$gatewayOperations = [regex]::Matches($gatewaySource, 'operation\s*==\s*"([a-z]+(?:\.[A-Za-z]+)+)"') |
    ForEach-Object { $_.Groups[1].Value } | Sort-Object -Unique
$missingGatewayOperations = @($sidecarOperations | Where-Object { $_ -notin $gatewayOperations })
if ($missingGatewayOperations.Count -gt 0) { throw ('Sidecar operations missing from McpAgentGateway dispatch: ' + ($missingGatewayOperations -join ', ')) }

Write-Host ('MCP contract check: PASS ({0} tools, docs, schemas, gateway dispatch and UiIcon mapping synchronized).' -f $toolNames.Count)
