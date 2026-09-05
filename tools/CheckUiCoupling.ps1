<#
    Operate.cs 的 UI 耦合守门脚本
    ------------------------------------------------------------------
    用途：统计 Operate.cs 里对 UI 的依赖，并与基线比较。
          任何一类只允许「减少或持平」，一旦增加即失败。

    背景：Operate.cs 是 27000 行的业务中枢，目前混写了 UI 调用。
          解耦按批次进行（B0..B8，见 x-nas/CLAUDE.md 的改造方案），
          本脚本保证「解耦过程中不会有人往回加」。

    用法：
        pwsh tools\CheckUiCoupling.ps1              # 检查（超基线则 exit 1）
        pwsh tools\CheckUiCoupling.ps1 -UpdateBaseline   # 某批次完成后刷新基线
        pwsh tools\CheckUiCoupling.ps1 -List A      # 列出 A 类的全部命中行

    最终目标：所有计数归零，且 Operate.cs 顶部这四行 using 可以删除：
        using AntdUI;   using Be.Windows.Forms;
        using System.Drawing;   using System.Windows.Forms;
#>
[CmdletBinding()]
param(
    [switch] $UpdateBaseline,
    [string] $List
)

$ErrorActionPreference = 'Stop'

$Root         = Split-Path -Parent $PSScriptRoot
$Target       = Join-Path $Root 'WinsockPacketEditor\Operate.cs'
$BaselineFile = Join-Path $PSScriptRoot 'ui-coupling.baseline.txt'

if (-not (Test-Path $Target)) { throw "找不到 $Target" }

# 类别 → 正则。与改造方案的 A..J 分类一一对应。
$Categories = [ordered]@{
    # 收紧于 B2：Get 已全部换成 UI.T，但 Provider / SetLanguage / DefaultLanguage /
    # CurrentLanguage 还有 10 处（在配置持久化方法里，随 B3 一起处理），必须继续被计入。
    'A 本地化'   = 'AntdUI\.Localization\.'
    'B 模态框'   = 'AntdUI\.Modal\.open'
    'C 右键菜单' = 'AntdUI\.I?ContextMenuStripItem'
    'D 通知'     = 'AntdUI\.Notification\.'
    'E 轻提示'   = 'AntdUI\.Message\.'
    'F 文件框'   = '\b(OpenFileDialog|SaveFileDialog|FolderBrowserDialog)\b'
    'G 主题'     = 'AntdUI\.Config\.'
    'H 控件参数' = '\b(HexBox|AntdUI\.(Table|Select|Input|Tree|Checkbox|FloatButton|Spin))\b'
    # 收紧于 B3b：旧写法 \b(Color|Bitmap|Icon|Font)\s*[.\[\w] 会把 IconSvg="XxxOutlined"（菜单
    # 图标名，属 C 类）和 Icon = TType.Warn（Modal 属性，属 B 类）也算进来，虚高到 105。
    # 现在只匹配真正的 System.Drawing 用法，剩下的就是 B3c 要清的图标/图片提取。
    'I 绘图'     = '\bColor\b|\bBitmap\b|\bImage\b|\bSystemIcons\b|\bIcon\s*\[|\bIcon\.|new\s+Icon\(|new\s+Font\('
    'J Form参数' = '\bForm\s+form\b|\bApplication\.'
}

$Lines = Get-Content -LiteralPath $Target -Encoding UTF8

if ($List) {
    $key = $Categories.Keys | Where-Object { $_.StartsWith($List) } | Select-Object -First 1
    if (-not $key) { throw "未知类别 '$List'，可选：$($Categories.Keys -join ', ')" }
    $re = $Categories[$key]
    for ($i = 0; $i -lt $Lines.Count; $i++) {
        if ($Lines[$i] -match $re) { '{0,6}: {1}' -f ($i + 1), $Lines[$i].Trim() }
    }
    exit 0
}

# 统计出现次数（同一行多次也计多次）
$Current = [ordered]@{}
foreach ($key in $Categories.Keys) {
    $re = [regex]$Categories[$key]
    $n  = 0
    foreach ($line in $Lines) { $n += $re.Matches($line).Count }
    $Current[$key] = $n
}
$Total = ($Current.Values | Measure-Object -Sum).Sum

if ($UpdateBaseline) {
    $out = foreach ($key in $Current.Keys) { '{0}={1}' -f $key, $Current[$key] }
    $out | Out-File -LiteralPath $BaselineFile -Encoding utf8
    Write-Output "基线已更新：$BaselineFile（合计 $Total）"
    exit 0
}

if (-not (Test-Path $BaselineFile)) {
    throw "基线文件不存在，请先运行：tools\CheckUiCoupling.ps1 -UpdateBaseline"
}

$Baseline = @{}
foreach ($line in (Get-Content -LiteralPath $BaselineFile -Encoding UTF8)) {
    if ($line -match '^(.+?)=(\d+)$') { $Baseline[$Matches[1]] = [int]$Matches[2] }
}

$Failed    = $false
$BaseTotal = 0

Write-Output ''
Write-Output ('{0,-14} {1,8} {2,8} {3,8}' -f '类别', '基线', '当前', '增减')
Write-Output ('-' * 42)

foreach ($key in $Current.Keys) {
    $b = if ($Baseline.ContainsKey($key)) { $Baseline[$key] } else { 0 }
    $c = $Current[$key]
    $BaseTotal += $b
    $delta = $c - $b
    $mark  = if ($delta -gt 0) { '  <== 新增，不允许' } elseif ($delta -lt 0) { '  已清理' } else { '' }
    if ($delta -gt 0) { $Failed = $true }
    Write-Output ('{0,-14} {1,8} {2,8} {3,8}{4}' -f $key, $b, $c, $delta, $mark)
}

Write-Output ('-' * 42)
Write-Output ('{0,-14} {1,8} {2,8} {3,8}' -f '合计', $BaseTotal, $Total, ($Total - $BaseTotal))
Write-Output ''

if ($Failed) {
    Write-Output 'FAIL：Operate.cs 新增了 UI 耦合。请改用 UI 门面（ClassObject\Ui\UI.cs）。'
    exit 1
}

if ($Total -eq 0) {
    Write-Output 'DONE：Operate.cs 已无 UI 耦合，可以删除顶部那四行 using 了。'
}
else {
    Write-Output 'PASS：未新增 UI 耦合。'
}
exit 0
