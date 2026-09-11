# BW → Task 第二步的验收：SendExecute / RobotExecute 换成 Task 之后
#   正常跑完 → Completed("done")；中途 StopSend/StopRobot → Completed("stopped") 且取消及时。
# 反射进已构建的 dll 跑，不启动 GUI。
param([string]$Bin)
if (-not $Bin) {
    $Bin = Join-Path $PSScriptRoot '..\..\WinsockPacketEditor\bin\Debug'
    if (-not (Test-Path (Join-Path $Bin 'WinsockPacketEditor.dll'))) {
        $Bin = Join-Path $PSScriptRoot '..\..\WPEHybrid\bin\Debug\net48'
    }
}
$Bin = (Resolve-Path $Bin).Path
Set-Location $Bin
[Environment]::CurrentDirectory = $Bin

$asm = [Reflection.Assembly]::LoadFrom((Join-Path $Bin 'WinsockPacketEditor.dll'))

$reType   = $asm.GetType('WinsockPacketEditor.RobotExecute')
$riType   = $asm.GetType('WinsockPacketEditor.RobotInfo')
$iiType   = $asm.GetType('WinsockPacketEditor.InstructionInfo')
$blType   = [type]("System.ComponentModel.BindingList``1[$($iiType.FullName)]")
$instEnum = $asm.GetType('WinsockPacketEditor.Operate+RobotConfig+Robot+InstructionType')
$listType = $asm.GetType('WinsockPacketEditor.Operate+RobotConfig+List')
$sysCfg   = $asm.GetType('WinsockPacketEditor.Operate+SystemConfig')
$execEnum = $asm.GetType('WinsockPacketEditor.Operate+SystemConfig+Execute')
$running  = $reType.GetProperty('Running')

function New-Robot([string[]]$delayContents) {
    # 每个元素一条 Delay 指令。⚠️ 取消只在每轮循环<b>顶上</b>检测（与旧 BW 逐字相同）——
    # 所以要验「停止 → stopped」，延迟指令后面必须还有一条指令，好让 DoSleep 返回后的下一轮抛出取消。
    $list = [Activator]::CreateInstance($blType)
    $delay = [enum]::Parse($instEnum, 'Delay')
    foreach ($c in $delayContents) {
        $list.Add([Activator]::CreateInstance($iiType, @($delay, $c)))
    }
    return [Activator]::CreateInstance($riType, @($true, [Guid]::NewGuid(), 'T', $list))
}

$sid = 0
function Run-Robot($ri, [int]$stopAfterMs) {
    $script:sid++
    $src = "done$($script:sid)"
    $re = [Activator]::CreateInstance($reType)
    $sub = Register-ObjectEvent -InputObject $re -EventName Completed -SourceIdentifier $src
    try {
        $re.GetType().GetMethod('StartRobot').Invoke($re, @($ri, $null)) | Out-Null
        $wasRunning = [bool]$running.GetValue($re)     # 启动后立刻读

        $sw = [Diagnostics.Stopwatch]::StartNew()
        if ($stopAfterMs -ge 0) {
            Start-Sleep -Milliseconds $stopAfterMs
            $re.GetType().GetMethod('StopRobot').Invoke($re, @()) | Out-Null
        }
        while ([bool]$running.GetValue($re) -and $sw.ElapsedMilliseconds -lt 3000) { Start-Sleep -Milliseconds 10 }
        $stopMs = $sw.ElapsedMilliseconds

        $ev = Wait-Event -SourceIdentifier $src -Timeout 2
        $result = if ($ev) { $ev.SourceArgs[0] } else { $null }
        if ($ev) { Remove-Event -SourceIdentifier $src }
        return [pscustomobject]@{ WasRunning = $wasRunning; Result = $result; StopMs = $stopMs }
    }
    finally { Unregister-Event -SourceIdentifier $src -ErrorAction SilentlyContinue }
}

$fail = 0
function T($name, $cond, $detail) {
    $ok = [bool]$cond
    if (-not $ok) { $script:fail++ }
    '{0}  {1,-46} {2}' -f ($(if ($ok) { 'PASS' } else { 'FAIL' })), $name, $detail
}

# ① 正常跑完（延迟 400ms）→ Running 起过、结局 done
$r = Run-Robot (New-Robot @('400')) -1
T '正常跑完：Running 起过' $r.WasRunning $r.WasRunning
T '正常跑完：Completed = done' ($r.Result -eq 'done') $r.Result

# ② 跑到一半停（延迟 5000ms，200ms 后停）→ 结局 stopped、取消及时
$r = Run-Robot (New-Robot @('5000','1')) 200
T '中途停止：Running 起过' $r.WasRunning $r.WasRunning
T '中途停止：Completed = stopped' ($r.Result -eq 'stopped') $r.Result
T '中途停止：取消及时 (<400ms)' ($r.StopMs -lt 400) ("停到不 Running: " + $r.StopMs + "ms")

# ③ 列表级：两条机器人（各延迟 3000），Together 同时执行
$lst = $listType.GetField('lstRobotInfo').GetValue($null)
$lst.Clear()
$lst.Add((New-Robot '3000')); $lst.Add((New-Robot '3000'))
$sysCfg.GetField('ListExecute').SetValue($null, [enum]::Parse($execEnum, 'Together'))
$isRunning = $listType.GetProperty('IsRobotListRunning')
$listType.GetMethod('StartRobotList').Invoke($null, @()) | Out-Null
Start-Sleep -Milliseconds 250
$listWasRunning = [bool]$isRunning.GetValue($null)
$sw = [Diagnostics.Stopwatch]::StartNew()
$listType.GetMethod('StopRobotList').Invoke($null, @()) | Out-Null
while ([bool]$isRunning.GetValue($null) -and $sw.ElapsedMilliseconds -lt 3000) { Start-Sleep -Milliseconds 10 }
$listStopMs = $sw.ElapsedMilliseconds
$lst.Clear()
T '列表级：IsRobotListRunning 起过' $listWasRunning $listWasRunning
T '列表级：停止后很快 false (<800ms)' ($listStopMs -lt 800) ($listStopMs.ToString() + 'ms')

''
"失败 $fail 项  ($(Split-Path $Bin -Leaf))"
exit $fail
