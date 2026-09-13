<script setup lang="ts">
/*
  机器人编辑 —— 对应 WinForms 的 Controls/RobotEdit（1000 + 2217 行）。

  ══ 2026-09-09 整屏重做：从「一屏 12 张表单」改成「指令块 + 程序流」══

  改造前左边是<b>四组折叠面板、12 个各长各样的插入表单竖着摞</b>，一屏放不下要滚，
  每种指令的行式还都不一样（有的两个单选、有的三个输入框、有的一个下拉）；
  右边是一张平表，循环嵌套完全看不出来。用户的原话是「太繁琐太复杂」。

  现在：
  ① <b>调色板</b>——四组分段 + 13 块瓷砖（图标 + 名字）。挑中哪一块，
     下面那<b>唯一一处</b>参数区才画它的表单。屏幕上永远只有一份表单。
  ② <b>程序流</b>——指令不再是表格行，是一块块带类型色轨的积木；
     <b>循环用缩进 + 竖轨画出嵌套</b>，闭合不上的当场标红。

  【为什么嵌套要按栈画】执行器 RobotExecute 就是拿 Stack<int> 走的：
  LoopStart 入栈、LoopEnd 看栈顶。所以缩进按栈算才与真实执行一致。
  顺带把两种真错误在保存之前就显出来：**循环没闭合**、**多出来的循环结束**
  （C# 的 CheckRobotInstruction 只会在保存 / 执行时报一句「循环指令不正确」）。

  【改的是工作副本】WinForms 的 RobotEdit_Load 里 new BindingList<InstructionInfo>(ri.RInstruction.ToList())，
  只按「保存」才写回，取消就是真的取消。副本放在 C#（RobotConfig.Robot 的 editInstruction），
  这边每次改动后整表重取 —— InstructionInfo 没有主键，按<b>下标</b>收发，重取才不会错位。

  【内容串由前端拼、C# 校验】各类指令的 InstContent 格式（"Press|A"、"Enable|SendList|<GUID>"、
  "100-200"、"MoveTo|10, 20"…）照 WinForms 的 bInsert_* 逐个抄，两套 UI 存的串必须一样。
  格式对不对在 C# 侧查（ValidateInstruction），前端不再写一份。

  【执行】先保存再跑（与 bExecute_Click 一致），200ms 轮询进度；执行轨迹（"1, 2, 3, 2, 3"）
  在 C# 攒，对应 WinForms 右上那个 txtINSTLog。
*/
import { computed, nextTick, onBeforeUnmount, ref, watch } from 'vue'
import { call } from '../../bridge'
import { FeedList, ListAction, type FilterRow, type RobotRow, type SendRow } from '../../bridge/types'
import { t, type Key } from '../../i18n'
import { comboOf, isModifierEvent, keyNameOf } from '../../keys'
import { useList } from '../../stores/lists'
import { pushToast } from '../../stores/toast'
import { useRowPick } from '../../usePick'
import ContextMenu from '../ContextMenu.vue'
import CyberSelect from '../CyberSelect.vue'
import { ICON, type MenuItem } from '../menu'
import { useModal } from '../../useModal'

const props = defineProps<{ id: string | null }>()

const emit = defineEmits<{
  (e: 'close'): void
  (e: 'saved'): void
}>()

interface Head { Id: string; Name: string }
interface Inst { Index: number; Type: number; TypeName: string; Text: string; Content: string }

/** 与 C# 的 InstructionType 序号一致 */
const IT = {
  SendSendList: 0, Delay: 1, LoopStart: 2, LoopEnd: 3, KeyBoard: 4,
  Mouse: 5, SendPacketList: 6, SetSystemSocket: 7, Switch: 8,
} as const

const f = ref<Head | null>(null)
const rows = ref<Inst[]>([])
const busy = ref(false)
const error = ref('')
const badIndex = ref(-1)
const tbody = ref<HTMLElement | null>(null)

/* ── 打开 / 关闭 ────────────────────────────────────────────── */

//⚠️ 这三样必须在下面那个 immediate 的 watch 之前声明：它第一次跑就会用到（暂时性死区，SendEdit 栽过一次、这里又栽一次）
let poll = 0
const prog = ref({ Running: false, Index: -1, Total: 0, Trail: '', Result: '' })

//单击 / Ctrl / Shift 多选，全项目一份实现。键是下标的字符串形式
const { picked, onRowClick, selectAll, clear } = useRowPick(rows, (r) => String(r.Index))

watch(() => props.id, async (id) => {
  if (id === null) {
    stopPoll()
    return
  }

  f.value = null
  rows.value = []
  picked.value = new Set()
  error.value = ''
  badIndex.value = -1
  prog.value = { Running: false, Index: -1, Total: 0, Trail: '', Result: '' }

  try {
    const r = await call<Head>('openRobotEdit', { id })

    if (!r?.Id) {
      error.value = t('rb.e.gone')
      return
    }

    f.value = r
    await reload()
  } catch (e) {
    console.error('[rb.e] 打开失败', e)
    error.value = String(e)
  }
}, { immediate: true })

/** 关弹窗。C# 侧要收尾（停执行器、丢掉工作副本），保存与取消都得走这里。 */
async function close(): Promise<void> {
  stopPoll()

  try {
    await call('closeRobotEdit')
  } catch (e) {
    console.error('[rb.e] 收尾失败', e)
  }

  emit('close')
}

onBeforeUnmount(stopPoll)

/* ── 指令集 ─────────────────────────────────────────────────── */

async function reload(): Promise<void> {
  try {
    const r = await call<{ rows: Inst[] }>('getRobotInstructions')
    rows.value = r?.rows ?? []

    //下标是键，动过顺序就全对不上了 —— 整表换掉后一律清掉选中集（WinForms 也是 SelectedIndex = -1）
    picked.value = new Set()
  } catch (e) {
    console.error('[rb.e] 取指令集失败', e)
  }
}

const pickedIndexes = computed(() => [...picked.value].map(Number).sort((a, b) => a - b))

/*
  ══ 程序流：按<b>栈</b>算缩进 ══

  这不是「看着像代码所以缩一下」——执行器 RobotExecute 就是拿 Stack<int> 走的
  （LoopStart 入栈、LoopEnd 看栈顶、次数没跑完就跳回栈顶那条）。按同一套规则画，
  屏幕上的嵌套才等于真实的执行嵌套。

  顺带把两种<b>真错误</b>在保存之前就显出来：
  · openLoop —— 到末尾还没闭合的 LoopStart；
  · strayEnd —— 栈是空的时候撞上的 LoopEnd。
  C# 的 CheckRobotInstruction 也拦这两种，但它只在保存 / 执行时报一句
  「循环指令不正确」，不告诉你是哪一条。

  ⚠️ LoopEnd 要<b>先退一层再取 depth</b>、LoopStart 要<b>先取 depth 再进一层</b>——
  这样一对循环的头尾落在同一层，中间的指令比它们深一层，括号才读得出来。
*/
interface FlowRow extends Inst { depth: number; openLoop: boolean; strayEnd: boolean }

const flow = computed<FlowRow[]>(() => {
  const out: FlowRow[] = []
  const stack: number[] = []
  let depth = 0

  rows.value.forEach((r) => {
    let strayEnd = false

    if (r.Type === IT.LoopEnd) {
      if (stack.length) { stack.pop(); depth-- } else { strayEnd = true }
    }

    out.push({ ...r, depth, openLoop: false, strayEnd })

    if (r.Type === IT.LoopStart) { stack.push(out.length - 1); depth++ }
  })

  stack.forEach((i) => { out[i].openLoop = true })
  return out
})

const loopBad = computed(() => flow.value.some((r) => r.openLoop || r.strayEnd))

/** 有选中行就插在最靠前的那行前面，否则追加到末尾（WinForms 的 AddInstruction）。 */
async function add(type: number, content: string): Promise<void> {
  if (prog.value.Running) return
  error.value = ''
  badIndex.value = -1

  const insertAt = pickedIndexes.value.length ? pickedIndexes.value[0] : -1

  try {
    const r = await call<{ error: string }>('addRobotInstruction', { type, content, insertAt })

    if (r?.error) {
      pushToast('error', r.error)
      return
    }

    await reload()

    if (insertAt < 0) {
      await nextTick()
      const el = tbody.value
      if (el) el.scrollTop = el.scrollHeight
    }
  } catch (e) {
    console.error('[rb.e] 插入失败', e)
  }
}

/* ── 右键菜单（置顶 / 上移 / 下移 / 置底 / 删除 / 清空 + 全选 / 取消选择）── */

const menuAt = ref<{ x: number; y: number } | null>(null)

const menuItems = computed<MenuItem[]>(() => {
  const n = picked.value.size
  const tag = n ? ' (' + n + ')' : ''

  return [
    { id: 'top', label: t('lst.top') + tag, icon: ICON.top },
    { divider: true },
    { id: 'up', label: t('lst.up') + tag, icon: ICON.up },
    { id: 'down', label: t('lst.down') + tag, icon: ICON.down },
    { divider: true },
    { id: 'bottom', label: t('lst.bottom') + tag, icon: ICON.bottom },
    { divider: true },
    { id: 'delete', label: t('lst.delete') + tag, icon: ICON.del, danger: true },
    { id: 'clear', label: t('rb.e.clearAll'), icon: ICON.del, danger: true, disabled: !rows.value.length },
    { divider: true },
    { id: 'selectAll', label: t('pm.selectAll'), icon: ICON.list },
    { id: 'deselect', label: t('pm.deselect'), icon: ICON.del, disabled: !n },
  ]
})

const ACTION_OF: Record<string, ListAction> = {
  top: ListAction.Top,
  up: ListAction.Up,
  down: ListAction.Down,
  bottom: ListAction.Bottom,
  delete: ListAction.Delete,
  clear: ListAction.CleanUp,
}

async function onMenuPick(id: string): Promise<void> {
  if (id === 'selectAll') { selectAll(); return }
  if (id === 'deselect') { clear(); return }
  if (prog.value.Running) return

  const action = ACTION_OF[id]
  if (action === undefined) return

  if (id !== 'clear' && picked.value.size === 0) {
    pushToast('warning', t('lst.needPick'))
    return
  }

  try {
    await call('robotInstructionAction', { action, indexes: pickedIndexes.value })
    await reload()
  } catch (e) {
    console.error('[rb.e] 指令集操作失败', e)
  }
}

/* ── 指令块（调色板）─────────────────────────────────────────

  13 块瓷砖，与改造前那 12 个插入表单一一对应（循环那一格原来是「开始 / 结束」
  两颗按钮共用一份表单，这里拆成两块 —— 它们在程序流里本来就是两条独立指令）。

  ⚠️ <b>颜色按「这条指令干什么」分，不是按分组分</b>：
  发送出去的＝绿（与全项目「在跑 / 发出」同一个色语）、配置类＝青、
  循环＝琥珀（与嵌套竖轨同色，一眼看出这是结构不是动作）、
  开关＝洋红（它动的是<b>别的列表</b>，独一份）、延迟＝压暗的灰（什么都不做，
  不该跟动作抢注意力）。七个色全部走令牌，深浅两套皮肤各自算。
*/
type Grp = 'packet' | 'control' | 'key' | 'mouse'
type BKey = 'send' | 'pkt' | 'sock' | 'delay' | 'loopStart' | 'loopEnd' | 'switch'
  | 'key' | 'combo' | 'text' | 'mkey' | 'wheel' | 'move'

interface Block { k: BKey; g: Grp; type: number; label: Key; icon: string }

const I = {
  send: '<path d="M4 12l16-8-6 16-2-6z"/>',
  pkt: '<path d="M4 6h16M4 12h16M4 18h10"/><circle cx="19" cy="18" r="2"/>',
  sock: '<path d="M9 3v5M15 3v5M6 8h12v3a6 6 0 0 1-12 0zM12 17v4"/>',
  delay: '<circle cx="12" cy="12" r="8"/><path d="M12 7.5V12l3 2"/>',
  loopStart: '<path d="M4 9h11a4 4 0 0 1 0 8H9"/><path d="M12 14l-3 3 3 3"/>',
  loopEnd: '<path d="M20 15H9a4 4 0 0 1 0-8h6"/><path d="M12 10l3-3-3-3"/>',
  switch: '<rect x="2.5" y="7" width="19" height="10" rx="5"/><circle cx="8" cy="12" r="2.6"/>',
  key: '<rect x="3" y="6" width="18" height="12" rx="2"/><path d="M7 10h.01M11 10h.01M15 10h.01M8.5 14h7"/>',
  combo: '<rect x="2.5" y="7" width="9" height="10" rx="1.6"/><rect x="12.5" y="7" width="9" height="10" rx="1.6"/><path d="M11.5 12h1"/>',
  text: '<path d="M5 7V5h14v2M12 5v14M9.5 19h5"/>',
  mkey: '<rect x="7.5" y="2.5" width="9" height="19" rx="4.5"/><path d="M12 6.5v3.5"/>',
  wheel: '<rect x="7.5" y="2.5" width="9" height="19" rx="4.5"/><path d="M12 6.5v3.5M12 1.5l-2 2M12 1.5l2 2"/>',
  move: '<path d="M12 3v18M3 12h18M12 3L9.5 5.5M12 3l2.5 2.5M12 21l-2.5-2.5M12 21l2.5-2.5M3 12l2.5-2.5M3 12l2.5 2.5M21 12l-2.5-2.5M21 12l2.5 2.5"/>',
} as const

const BLOCKS: Block[] = [
  { k: 'send', g: 'packet', type: IT.SendSendList, label: 'rb.e.sendList', icon: I.send },
  { k: 'pkt', g: 'packet', type: IT.SendPacketList, label: 'rb.e.packetList', icon: I.pkt },
  { k: 'sock', g: 'packet', type: IT.SetSystemSocket, label: 'rb.e.sysSocket', icon: I.sock },
  { k: 'delay', g: 'control', type: IT.Delay, label: 'rb.e.delay', icon: I.delay },
  { k: 'loopStart', g: 'control', type: IT.LoopStart, label: 'rb.e.loopStart', icon: I.loopStart },
  { k: 'loopEnd', g: 'control', type: IT.LoopEnd, label: 'rb.e.loopEnd', icon: I.loopEnd },
  { k: 'switch', g: 'control', type: IT.Switch, label: 'rb.e.sw', icon: I.switch },
  { k: 'key', g: 'key', type: IT.KeyBoard, label: 'rb.e.key', icon: I.key },
  { k: 'combo', g: 'key', type: IT.KeyBoard, label: 'rb.e.combo', icon: I.combo },
  { k: 'text', g: 'key', type: IT.KeyBoard, label: 'rb.e.text', icon: I.text },
  { k: 'mkey', g: 'mouse', type: IT.Mouse, label: 'rb.e.mouseKey', icon: I.mkey },
  { k: 'wheel', g: 'mouse', type: IT.Mouse, label: 'rb.e.wheel', icon: I.wheel },
  { k: 'move', g: 'mouse', type: IT.Mouse, label: 'rb.e.move', icon: I.move },
]

/*
  ⚠️ 页签用的是<b>短名</b>（封包 / 控制 / 键盘 / 鼠标），不是那四个带「指令」的长名。
  四个页签平分 296px 的调色板，一格只有 <b>64.8px</b>（实测）——
  中文的「封包指令」正好卡在 63px，而英文 "Packet Instruction" 要 130px、
  俄语「Команда управления」要 211px，一律截断。
  这一栏里「指令」二字本来也是废话：整块调色板装的就是指令。
*/
const GROUPS: Array<{ g: Grp; label: Key }> = [
  { g: 'packet', label: 'rb.e.tPacket' },
  { g: 'control', label: 'rb.e.tControl' },
  { g: 'key', label: 'rb.e.tKey' },
  { g: 'mouse', label: 'rb.e.tMouse' },
]

const group = ref<Grp>('control')
const pick = ref<BKey>('delay')

const tiles = computed(() => BLOCKS.filter((b) => b.g === group.value))
const block = computed(() => BLOCKS.find((b) => b.k === pick.value) ?? BLOCKS[0])

//换组时把选中块落到该组第一块 —— 参数区永远有东西可画，不会出现空白的一格
watch(group, (g) => { if (block.value.g !== g) pick.value = BLOCKS.find((b) => b.g === g)!.k })

/* ── 各块的参数 ─────────────────────────────────────────────── */

const sends = useList<SendRow>(FeedList.Send)
const robots = useList<RobotRow>(FeedList.Robot)
const filters = useList<FilterRow>(FeedList.Filter)

//发送 - 发送列表
const sendSel = ref('')
const sendOptions = computed(() => sends.value.map((s) => ({ value: s.Id, label: s.Name })))
watch(sendOptions, (o) => { if (!o.some((x) => x.value === sendSel.value)) sendSel.value = o[0]?.value ?? '' }, { immediate: true })

//设置 - 系统套接字
const sockMode = ref<'packet' | 'filter' | 'custom'>('packet')
const sockNum = ref(0)

//延迟
const delayMode = ref<'fix' | 'random'>('fix')
const delayFix = ref(100)
const delayFrom = ref(1)
const delayTo = ref(100)

//循环
const loopCount = ref(1)

//开关
const switchOn = ref(true)
const switchType = ref<'SendList' | 'RobotList' | 'FilterList'>('SendList')
const switchTarget = ref('')

const switchTypeOptions = computed(() => [
  { value: 'SendList', label: t('rb.e.swSend'), disabled: !sends.value.length },
  { value: 'RobotList', label: t('rb.e.swRobot'), disabled: !robots.value.length },
  { value: 'FilterList', label: t('rb.e.swFilter'), disabled: !filters.value.length },
])

const switchTargetOptions = computed(() => {
  const src = switchType.value === 'SendList' ? sends.value : switchType.value === 'RobotList' ? robots.value : filters.value
  return (src as Array<{ Id: string; Name: string }>).map((x) => ({ value: x.Id, label: x.Name }))
})

watch(switchTargetOptions, (o) => { if (!o.some((x) => x.value === switchTarget.value)) switchTarget.value = o[0]?.value ?? '' }, { immediate: true })

//键盘
const KEY_TYPES = ['Press', 'Down', 'Up'] as const
const keyType = ref(0)
const keyTypeOptions = computed(() => [
  { value: 0, label: t('rb.e.keyPress') },
  { value: 1, label: t('rb.e.keyDown') },
  { value: 2, label: t('rb.e.keyUp') },
])
const keyName = ref('')
const combo = ref('')
const text = ref('')

/*
  按键框是「按一下就记住名字」的只读框，与 WinForms 的 txtKey_KeyDown 一样；
  组合框要等到一个「非修饰键 + 至少一个修饰键」才记（HotkeyTextBox 的逻辑）。
  两个框都要 preventDefault：Tab / 空格 / 方向键这些默认行为会把焦点带走或滚动页面。
*/
function onKeyCapture(e: KeyboardEvent): void {
  e.preventDefault()
  const n = keyNameOf(e)
  if (n) keyName.value = n
}

function onComboCapture(e: KeyboardEvent): void {
  e.preventDefault()
  if (isModifierEvent(e)) return
  combo.value = comboOf(e)
}

//鼠标
const MOUSE_KEYS = ['LeftClick', 'RightClick', 'LeftDBClick', 'RightDBClick', 'LeftDown', 'LeftUp', 'RightDown', 'RightUp'] as const
const mouseKey = ref(0)
const mouseKeyOptions = computed(() => MOUSE_KEYS.map((_, i) => ({ value: i, label: t(('rb.e.m' + i) as 'rb.e.m0') })))
const wheelDir = ref<'WheelUp' | 'WheelDown'>('WheelUp')
const wheelOptions = computed(() => [
  { value: 'WheelUp', label: t('lst.up') },
  { value: 'WheelDown', label: t('lst.down') },
])
const wheelDist = ref(10)
const moveMode = ref<'MoveTo' | 'MoveBy'>('MoveTo')
const mx = ref(0)
const my = ref(0)

/*
  内容串：一处拼、一处判。格式逐条照 WinForms 的 bInsert_*，两套 UI 存的串必须一样。

  ⚠️ canInsert 拦的是「C# 的 ValidateInstruction 一定会拒」的那几种
  （自定义套接字号 ≤ 0、循环次数 < 1、没挑发送 / 开关目标、没按键、空文本）——
  <b>前置条件不满足的选项不给开</b>，别让人先做一个必然失败的选择。
  它<b>不是</b>另写一份校验：真正的判定仍在 C# 侧。
*/
const CONTENT: Record<BKey, () => string> = {
  send: () => sendSel.value,
  pkt: () => '',
  sock: () => sockMode.value === 'packet' ? 'PacketConfig.List'
    : sockMode.value === 'filter' ? 'FilterSocket'
      : 'Customize|' + Math.trunc(sockNum.value || 0),
  delay: () => delayMode.value === 'fix'
    ? String(Math.trunc(delayFix.value || 0))
    : Math.trunc(delayFrom.value || 0) + '-' + Math.trunc(delayTo.value || 0),
  loopStart: () => String(Math.trunc(loopCount.value || 0)),
  loopEnd: () => '',
  switch: () => (switchOn.value ? 'Enable' : 'Disable') + '|' + switchType.value + '|' + switchTarget.value.toUpperCase(),
  key: () => KEY_TYPES[keyType.value] + '|' + keyName.value,
  combo: () => 'Combine|' + combo.value,
  text: () => 'Text|' + text.value.trim(),
  mkey: () => MOUSE_KEYS[mouseKey.value] + '|',
  wheel: () => wheelDir.value + '|' + Math.trunc(wheelDist.value || 0),
  move: () => moveMode.value + '|' + Math.trunc(mx.value || 0) + ', ' + Math.trunc(my.value || 0),
}

const canInsert = computed(() => {
  switch (pick.value) {
    case 'send': return !!sendSel.value
    case 'sock': return sockMode.value !== 'custom' || sockNum.value > 0
    case 'loopStart': return loopCount.value >= 1
    case 'switch': return !!switchTarget.value
    case 'key': return !!keyName.value
    case 'combo': return !!combo.value
    case 'text': return !!text.value.trim()
    default: return true
  }
})

function insert(): void {
  if (!canInsert.value) return
  add(block.value.type, CONTENT[pick.value]())
}

/* ── 执行 ───────────────────────────────────────────────────── */

function stopPoll(): void {
  if (poll) { clearInterval(poll); poll = 0 }
}

/*
  轨迹只显示尾巴：循环一多它会长到几千个数，而看的人关心的是「现在走到哪了」。
  <b>不要用 direction: rtl 来「靠右截断」</b>——bidi 会把标点顺序颠过来，"1, 2" 画成 "2 ,1"（真出过）。
  完整的一串在提示里，悬停能看。
*/
const trailTail = computed(() => {
  const s = prog.value.Trail
  return s.length > 72 ? '… ' + s.slice(s.lastIndexOf(', ', s.length - 72) + 2) : s
})

function startPoll(): void {
  stopPoll()

  poll = window.setInterval(async () => {
    try {
      const p = await call<typeof prog.value>('getRobotEditProgress')
      if (p) prog.value = p

      if (!p?.Running) {
        stopPoll()
        //三种结局各说各的，与 WinForms 的 Worker_RunWorkerCompleted 一致
        if (p?.Result === 'stopped') pushToast('warning', t('rb.e.stopped'))
        else if (p?.Result?.startsWith('error:')) pushToast('error', t('rb.e.failed') + ' ' + p.Result.slice(6))
        else pushToast('success', t('rb.e.done'))
      }
    } catch {
      stopPoll()
    }
  }, 200)
}

async function toggleRun(): Promise<void> {
  if (prog.value.Running) {
    try { await call('stopRobotEdit') } catch (e) { console.error('[rb.e] 停止失败', e) }
    return
  }

  error.value = ''
  badIndex.value = -1

  try {
    const r = await call<{ error: string; badIndex: number }>('startRobotEdit', { name: f.value?.Name ?? '' })

    if (r?.error) {
      error.value = r.error
      badIndex.value = r.badIndex ?? -1
      await revealBad()
      return
    }

    //执行前先保存了一遍，列表页那边的名称 / 指令条数要跟上
    emit('saved')
    prog.value = { Running: true, Index: -1, Total: 0, Trail: '', Result: '' }
    startPoll()
  } catch (e) {
    console.error('[rb.e] 执行失败', e)
    error.value = String(e)
  }
}

/** 校验没过的那一行滚进视野。用 nextTick 不用 rAF：窗口被遮住时 Chromium 不发 rAF。 */
async function revealBad(): Promise<void> {
  if (badIndex.value < 0) return
  await nextTick()
  tbody.value?.querySelector('.blk.bad')?.scrollIntoView({ block: 'nearest' })
}

/* ── 保存 ───────────────────────────────────────────────────── */

async function save(): Promise<void> {
  busy.value = true
  error.value = ''
  badIndex.value = -1

  try {
    const r = await call<{ error: string; badIndex: number }>('saveRobotEdit', { name: f.value?.Name ?? '' })

    if (r?.error) {
      error.value = r.error
      badIndex.value = r.badIndex ?? -1
      await revealBad()
      return
    }

    emit('saved')
    await close()
  } catch (e) {
    console.error('[rb.e] 保存失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}

/* 登记进模态栈：父窗体因此变 inert；自己被后开的弹窗盖住时也会 inert。见 useModal.ts */
const { covered } = useModal(() => props.id !== null)
</script>

<template>
  <!--
    ⚠️ <b>Teleport 到 body</b> —— 不是为了好看，是必须的，两个理由都在 useModal.ts 里：
    ① 代理模式的 .proxy 是 z-index: 10 的层叠上下文，弹窗留在里面时遮罩盖不住标题栏；
    ② 出去了才不会被 .shell 的 inert 一起禁掉。

    ⚠️ <b>刻意不换行、不重排缩进</b>：模板里有 white-space: pre 的块，
    整体缩进一动，Vue 模板编译器的 condense 会连带改掉渲染结果。

    ⚠️ <b>点遮罩不再关闭弹窗</b>：编辑器里都是填了一半的东西，点空白处就丢掉太容易误操作。
    出口只留「取消 / 关闭」按钮与 Esc。
  -->
  <Teleport to="body"><div v-if="props.id !== null" class="editor-mask" :inert="covered">
    <div class="dlg" role="dialog" aria-modal="true" @keydown.esc="close">
      <span class="mk tl" /><span class="mk tr" /><span class="mk bl" /><span class="mk br" />

      <header class="hd">
        <div class="tt">
          <span class="zh">{{ t('rb.e.title') }}</span>
          <span class="sub">Controls/RobotEdit</span>
        </div>
        <button class="x" :title="t('dlg.cancel')" @click="close">
          <svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg>
        </button>
      </header>

      <div v-if="!f" class="loading">{{ error || t('proxy.working') }}</div>

      <template v-else>
        <!--
          工作条：名称 + 执行 + 计数 + 轨迹合成一条，横跨两栏。
          改造前名称与执行条各占右栏一行，而它们管的都是「这个机器人整体」，不属于程序流。
        -->
        <div class="wbar">
          <span class="wk">{{ t('col.robotName') }}</span>
          <input v-model="f.Name" class="inp nm" spellcheck="false" :disabled="prog.Running" :placeholder="t('rb.e.namePh')">

          <button class="btn run" :class="{ on: prog.Running }" :disabled="!rows.length" @click="toggleRun">
            <svg v-if="prog.Running" class="ico" viewBox="0 0 24 24"><rect x="6" y="6" width="12" height="12" /></svg>
            <svg v-else class="ico" viewBox="0 0 24 24"><path d="M7 4l13 8-13 8z" /></svg>
            {{ prog.Running ? t('rb.e.stop') : t('rb.e.execute') }}
          </button>

          <span class="cnt">{{ t('col.execCount') }} <b>{{ prog.Total }}</b></span>

          <!--
            执行轨迹：WinForms 右上那个 txtINSTLog 的 "1, 2, 3, "。

            ⚠️ <b>:data-tip 而不是 :title</b> —— 跑起来之后这串每 200ms 跟着轮询变长一次，
            写 title 就是每 200ms 往元素上安一次原生提示（详见 tooltip.ts 头上「两道防线」）。
          -->
          <span class="trail" :data-tip="prog.Trail">
            <span class="tl">{{ t('rb.e.trail') }}</span>
            <span class="tv">{{ trailTail || '—' }}</span>
          </span>
        </div>

        <div class="bd">
          <!-- ══ 左：指令块 ══ -->
          <aside class="pal" :inert="prog.Running">
            <div class="ptabs">
              <button v-for="g in GROUPS" :key="g.g" class="ptab" :class="{ on: group === g.g }" @click="group = g.g">{{ t(g.label) }}</button>
            </div>

            <div class="tiles">
              <button
                v-for="b in tiles"
                :key="b.k"
                class="tile"
                :class="['c' + b.type, { on: pick === b.k }]"
                @click="pick = b.k"
              >
                <svg class="ico" viewBox="0 0 24 24" v-html="b.icon" />
                <span class="nm">{{ t(b.label) }}</span>
              </button>
            </div>

            <div class="pcfg" :class="'c' + block.type">
              <div class="ph">
                <span class="dot" />
                <span class="nm">{{ t(block.label) }}</span>
              </div>

              <!-- 发送 - 发送列表 -->
              <template v-if="pick === 'send'">
                <CyberSelect v-model="sendSel" :options="sendOptions" :placeholder="t('rb.e.pick')" class="dd" />
              </template>

              <!-- 发送 - 封包列表：没有参数，只有一句「它读的是哪儿」 -->
              <template v-else-if="pick === 'pkt'">
                <div class="lb">{{ t('rb.e.packetListHint') }}</div>
                <div class="note">{{ t('rb.e.packetListNote') }}</div>
              </template>

              <!-- 设置 - 系统套接字 -->
              <template v-else-if="pick === 'sock'">
                <button class="rd" :class="{ on: sockMode === 'packet' }" @click="sockMode = 'packet'"><i />{{ t('rb.e.sockPacket') }}</button>
                <button class="rd" :class="{ on: sockMode === 'filter' }" @click="sockMode = 'filter'"><i />{{ t('rb.e.sockFilter') }}</button>
                <div class="fr">
                  <button class="rd" :class="{ on: sockMode === 'custom' }" @click="sockMode = 'custom'"><i />{{ t('rb.e.sockCustom') }}</button>
                  <input v-model.number="sockNum" class="inp num" type="number" min="0" max="2147483647" :disabled="sockMode !== 'custom'">
                </div>
              </template>

              <!-- 延迟 -->
              <template v-else-if="pick === 'delay'">
                <div class="fr">
                  <button class="rd" :class="{ on: delayMode === 'fix' }" @click="delayMode = 'fix'"><i />{{ t('rb.e.fixed') }}</button>
                  <input v-model.number="delayFix" class="inp num" type="number" min="0" max="999999999" :disabled="delayMode !== 'fix'">
                  <span class="lb">{{ t('rb.e.ms') }}</span>
                </div>
                <div class="fr">
                  <button class="rd" :class="{ on: delayMode === 'random' }" @click="delayMode = 'random'"><i />{{ t('rb.e.random') }}</button>
                  <input v-model.number="delayFrom" class="inp num" type="number" min="0" max="999999999" :disabled="delayMode !== 'random'">
                  <span class="lb">-</span>
                  <input v-model.number="delayTo" class="inp num" type="number" min="0" max="999999999" :disabled="delayMode !== 'random'">
                  <span class="lb">{{ t('rb.e.ms') }}</span>
                </div>
              </template>

              <!-- 循环开始 -->
              <template v-else-if="pick === 'loopStart'">
                <div class="fr">
                  <input v-model.number="loopCount" class="inp num" type="number" min="1" max="999999999">
                  <span class="lb">{{ t('rb.e.loopTimes') }}</span>
                </div>
                <div class="note">{{ t('rb.e.loopNote') }}</div>
              </template>

              <!-- 循环结束 -->
              <template v-else-if="pick === 'loopEnd'">
                <div class="note">{{ t('rb.e.loopEndNote') }}</div>
              </template>

              <!-- 开关 -->
              <template v-else-if="pick === 'switch'">
                <div class="fr">
                  <button class="rd" :class="{ on: switchOn }" @click="switchOn = true"><i />{{ t('rb.e.on') }}</button>
                  <button class="rd" :class="{ on: !switchOn }" @click="switchOn = false"><i />{{ t('rb.e.off') }}</button>
                </div>
                <CyberSelect v-model="switchType" :options="switchTypeOptions" class="dd" />
                <CyberSelect v-model="switchTarget" :options="switchTargetOptions" :placeholder="t('rb.e.pick')" class="dd" />
              </template>

              <!-- 键盘 - 按键 -->
              <template v-else-if="pick === 'key'">
                <CyberSelect v-model="keyType" :options="keyTypeOptions" class="dd" />
                <input :value="keyName" class="inp cap-key" readonly :placeholder="t('rb.e.keyPh')" @keydown="onKeyCapture">
              </template>

              <!-- 键盘 - 组合键 -->
              <template v-else-if="pick === 'combo'">
                <input :value="combo" class="inp cap-key" readonly :placeholder="t('rb.e.comboPh')" @keydown="onComboCapture">
              </template>

              <!-- 键盘 - 文本 -->
              <template v-else-if="pick === 'text'">
                <input v-model="text" class="inp" spellcheck="false" :placeholder="t('rb.e.textPh')">
              </template>

              <!-- 鼠标 - 按键 -->
              <template v-else-if="pick === 'mkey'">
                <CyberSelect v-model="mouseKey" :options="mouseKeyOptions" class="dd" />
              </template>

              <!-- 鼠标 - 滚轮 -->
              <template v-else-if="pick === 'wheel'">
                <div class="fr">
                  <span class="lb">{{ t('rb.e.scroll') }}</span>
                  <CyberSelect v-model="wheelDir" :options="wheelOptions" class="dd half" />
                </div>
                <div class="fr">
                  <span class="lb">{{ t('rb.e.distance') }}</span>
                  <input v-model.number="wheelDist" class="inp num" type="number" min="1" max="99999">
                </div>
              </template>

              <!-- 鼠标 - 移动 -->
              <template v-else>
                <div class="fr">
                  <button class="rd" :class="{ on: moveMode === 'MoveTo' }" @click="moveMode = 'MoveTo'"><i />{{ t('rb.e.moveTo') }}</button>
                  <button class="rd" :class="{ on: moveMode === 'MoveBy' }" @click="moveMode = 'MoveBy'"><i />{{ t('rb.e.moveBy') }}</button>
                </div>
                <div class="fr">
                  <span class="lb">X</span>
                  <input v-model.number="mx" class="inp num" type="number" min="-99999" max="99999">
                  <span class="lb">Y</span>
                  <input v-model.number="my" class="inp num" type="number" min="-99999" max="99999">
                </div>
              </template>
            </div>

            <div class="pfoot">
              <button class="btn ins" :class="'c' + block.type" :disabled="!canInsert" @click="insert">
                <svg class="ico" viewBox="0 0 24 24"><path d="M12 5v14M5 12h14" /></svg>{{ t('rb.e.insert') }}
              </button>
              <div class="note">{{ t('rb.e.insertHint') }}</div>
            </div>
          </aside>

          <!-- ══ 右：程序流 ══ -->
          <section class="flow">
            <div class="fh">
              <span class="ft-t">{{ t('rb.e.flow') }}</span>
              <span class="ft-n">{{ rows.length }} {{ t('rb.e.steps') }}</span>
              <span class="grow" />
              <span v-if="loopBad" class="warn">{{ t('rb.e.loopBad') }}</span>
            </div>

            <div ref="tbody" class="fbody" @contextmenu.prevent="menuAt = { x: $event.clientX, y: $event.clientY }">
              <div v-if="!rows.length" class="empty">{{ t('rb.e.empty') }}</div>

              <div
                v-for="(r, i) in flow"
                :key="i"
                class="blk"
                :class="['c' + r.Type, {
                  sel: picked.has(String(r.Index)),
                  run: prog.Running && prog.Index === i,
                  bad: badIndex === i || r.openLoop || r.strayEnd,
                  lp: r.Type === IT.LoopStart || r.Type === IT.LoopEnd,
                }]"
                @click="onRowClick(r, $event, i)"
              >
                <span class="stp">{{ String(i + 1).padStart(2, '0') }}</span>
                <!-- 嵌套竖轨：深一层多画一条，与执行器那把 Stack 是同一套算法 -->
                <span v-for="d in r.depth" :key="d" class="rail" />

                <span class="face">
                  <svg class="ico" viewBox="0 0 24 24" v-html="I[r.Type === IT.LoopStart ? 'loopStart' : r.Type === IT.LoopEnd ? 'loopEnd' : r.Type === IT.Delay ? 'delay' : r.Type === IT.Switch ? 'switch' : r.Type === IT.KeyBoard ? 'key' : r.Type === IT.Mouse ? 'mkey' : r.Type === IT.SetSystemSocket ? 'sock' : 'send']" />
                  <span class="ty">{{ r.TypeName }}</span>
                  <!-- 只画本地化后的描述。<b>刻意不挂 :title 显示原始内容串</b> —— 那是排查用的，
                       日常读这一屏的人不关心 GUID 与「Enable|SendList|…」长什么样（按要求去掉）。 -->
                  <span v-if="r.Text || r.Content" class="ct">{{ r.Text || r.Content }}</span>
                  <span class="grow" />
                  <span v-if="r.openLoop" class="tg bad">{{ t('rb.e.loopOpen') }}</span>
                  <span v-else-if="r.strayEnd" class="tg bad">{{ t('rb.e.loopStray') }}</span>
                  <button class="op del" :title="t('acct.op.del')" :disabled="prog.Running"
                          @click.stop="picked = new Set([String(r.Index)]); onMenuPick('delete')">
                    <svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg>
                  </button>
                </span>
              </div>
            </div>
          </section>
        </div>
      </template>

      <footer class="ft">
        <span v-if="error" class="err">{{ error }}</span>
        <span class="grow" />
        <button class="btn" :disabled="busy" @click="close">{{ t('dlg.cancel') }}</button>
        <button class="btn primary" :disabled="busy || !f || prog.Running" @click="save">
          {{ busy ? t('proxy.working') : t('set.save') }}
        </button>
      </footer>

      <ContextMenu :at="menuAt" :items="menuItems" @pick="onMenuPick" @close="menuAt = null" />
    </div>
  </div></Teleport>
</template>

<style scoped>
.hd {
  flex: none;
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 14px 18px;
  border-bottom: 1px solid var(--border);
  background: var(--panel);
}

.tt { flex: 1; min-width: 0; display: flex; align-items: baseline; gap: 12px; }
.tt .zh { font-family: var(--orbit); font-weight: 700; font-size: var(--fs-title); color: var(--gray); letter-spacing: .04em; }
.tt .sub { font-family: var(--share); font-size: var(--fs-caption); letter-spacing: .14em; text-transform: uppercase; color: var(--dim); }

.x {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 26px;
  height: 26px;
  background: transparent;
  border: 1px solid transparent;
  color: var(--muted);
  cursor: pointer;
}

.x:hover { border-color: var(--danger); color: var(--danger); }
.x .ico { width: 15px; height: 15px; stroke: currentColor; stroke-width: 2; fill: none; }

.loading { padding: 60px 0; text-align: center; color: var(--muted); font-size: var(--fs-body); }

/*
  ══ 每种指令一个色 ══

  按「这条指令干什么」分，不按分组分 —— 发送出去的＝绿、配置＝青、循环＝琥珀
  （与嵌套竖轨同色）、开关＝洋红（它动的是<b>别的列表</b>，独一份）、
  键鼠＝同一族的两档紫（都是模拟输入）、延迟＝天蓝（等待）。

  ⚠️ <b>只能用有 -rgb 通道值的令牌</b>：淡底 / 辉光一律是 rgb(var(--bc-rgb) / N%)，
  拿一个没有通道值的令牌顶数会让「线是这个色、底是另一个色」。
  --acc-violet 原来就没有，为这一屏补了 --violet-rgb（见 tokens.css）。

  ⚠️ 也<b>别给某一类用压暗的灰</b>：「插入」按钮跟着当前块走色，
  灰色按钮看着就是禁用的 —— 延迟那一格试过 --dim3，当场翻车。
*/
.c0, .c6 { --bc: var(--green); --bc-rgb: var(--green-rgb); }        /* 发送：发出去的东西 */
.c7 { --bc: var(--cyan); --bc-rgb: var(--cyan-rgb); }               /* 设置套接字：配置类 */
.c1 { --bc: var(--nt1); --bc-rgb: var(--nt1-rgb); }                 /* 延迟：等待 */
.c2, .c3 { --bc: var(--amber); --bc-rgb: var(--amber-rgb); }        /* 循环：结构，与嵌套竖轨同色 */
.c8 { --bc: var(--magenta); --bc-rgb: var(--magenta-rgb); }         /* 开关：它动的是<b>别的列表</b>，独一份 */
.c4 { --bc: var(--acc-violet); --bc-rgb: var(--violet-rgb); }       /* 键盘 ┐ 两种模拟输入，同一族紫 */
.c5 { --bc: var(--nt4); --bc-rgb: var(--nt4-rgb); }                 /* 鼠标 ┘ */

/*
  ⚠️ <b>.btn 的基样式必须自己带。</b> style.css 里它只在 <b>.list-page</b> 下有定义，
  而这一屏是 .editor-mask / .dlg，匹配不到 —— 不写就退回浏览器原生按钮
  （实测底色 rgb(107,107,107)、高 21px），一眼就和旁边的自绘件不是一套。
  与 TextCompare 那次「重写时把 .inp 基样式整块弄丢」是同一个错。
  三个大编辑器（滤镜 / 发送 / 机器人）现在各抄一份，欠着没并。

  上下内边距<b>对称</b>：2026-09-09 全项目那一轮定的口径，line-height: 1 已经把
  字形偏上治好了，「上 +1 下 −1」是补第二遍。
*/
.btn {
  flex: none;
  padding: 8.5px 13px 7.5px;   /* 上多半像素、高度不变：100% 缩放下实测偏高 1px（2026-09-13 字体度量覆写后按 100% 缩放实测重调） */
  background: transparent;
  border: 1px solid var(--border);
  color: var(--gray);
  font-family: var(--share);
  font-size: var(--btn-size);
  line-height: 1;
  letter-spacing: .12em;
  text-transform: uppercase;
  cursor: pointer;
  white-space: nowrap;
}

.btn:hover:not(:disabled) { border-color: var(--cyan); color: var(--cyan); }
.btn:disabled { opacity: .35; cursor: default; }
.btn.primary { border-color: rgb(var(--green-rgb) / 45%); color: var(--green); }
.btn.primary:hover:not(:disabled) { background: rgb(var(--green-rgb) / 10%); border-color: var(--green); }

/* ── 工作条 ── */
.wbar {
  flex: none;
  display: flex;
  align-items: center;
  flex-wrap: wrap;
  gap: 8px 12px;
  padding: 10px 18px;
  border-bottom: 1px solid var(--border);
  background: rgb(var(--chrome-rgb) / 22%);
}

.wbar .wk { flex: none; font-size: var(--fs-body); color: var(--muted); }
.wbar .nm { flex: 1; min-width: 180px; }
.wbar .cnt { flex: none; font-size: var(--fs-label); color: var(--muted); font-family: var(--share); letter-spacing: .06em; }
.wbar .cnt b { font-family: var(--mono); font-variant-numeric: tabular-nums; color: var(--cyan); }

.trail { flex: 1; min-width: 140px; display: flex; align-items: center; gap: 8px; }
.trail .tl { flex: none; font-size: var(--fs-label); color: var(--muted); font-family: var(--share); letter-spacing: .06em; }
.trail .tv { flex: 1; min-width: 0; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; font-family: var(--mono); font-size: var(--fs-body); color: var(--acc-green2); }

.btn.run { flex: none; display: inline-flex; align-items: center; gap: 7px; border-color: rgb(var(--green-rgb) / 45%); color: var(--green); }
.btn.run .ico { width: 11px; height: 11px; fill: currentColor; stroke: none; }
.btn.run.on { border-color: rgb(var(--danger-rgb) / 30%); color: var(--danger); }
.btn.run:hover:not(:disabled) { background: rgb(var(--green-rgb) / 10%); border-color: var(--green); color: var(--green); }
.btn.run.on:hover:not(:disabled) { background: rgb(var(--danger-rgb) / 12%); border-color: var(--danger); color: var(--danger); }

/* 左右两栏：调色板定宽、程序流吃掉剩下的；两栏各自滚，弹窗本身不滚 */
.bd {
  flex: 1;
  min-height: 0;
  display: grid;
  grid-template-columns: 296px 1fr;
  overflow: hidden;
}

/* ══ 调色板 ══ */
.pal {
  min-height: 0;
  display: grid;
  grid-template-rows: auto auto 1fr auto;
  border-right: 1px solid var(--border);
  background: rgb(var(--chrome-rgb) / 14%);
}

.pal[inert] { opacity: .45; }

.ptabs { display: flex; padding: 10px 12px 0; gap: 4px; }

.ptab {
  flex: 1;
  min-width: 0;
  padding: 7px 2px;
  background: transparent;
  border: 1px solid var(--border);
  border-bottom-color: transparent;
  color: var(--muted);
  font-family: var(--share);
  font-size: var(--fs-label);
  line-height: 1;
  letter-spacing: .06em;
  cursor: pointer;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.ptab:hover { color: var(--cyan); }
.ptab.on { border-color: rgb(var(--cyan-rgb) / 45%); background: rgb(var(--cyan-rgb) / 8%); color: var(--cyan); }

/* 瓷砖：两列。一格一种指令，图标 + 名字，选中的整块点亮 */
.tiles {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 6px;
  padding: 10px 12px 12px;
}

.tile {
  position: relative;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 6px;
  /* 定高：一行名与两行名的瓷砖要一样高，否则同一行两块会一高一矮 */
  min-height: 72px;
  padding: 11px 4px 10px;
  background: rgb(var(--inset-rgb) / 26%);
  border: 1px solid var(--border);
  color: var(--muted);
  cursor: pointer;
  overflow: hidden;
}

/* 顶沿一道本色轨：不点开也看得出这块属于哪一类 */
.tile::before {
  content: '';
  position: absolute;
  inset: 0 0 auto;
  height: 2px;
  background: linear-gradient(90deg, rgb(var(--bc-rgb) / 55%), rgb(var(--bc-rgb) / 12%));
}

.tile:hover { border-color: rgb(var(--bc-rgb) / 55%); color: var(--gray); }

.tile.on {
  border-color: var(--bc);
  background: rgb(var(--bc-rgb) / 10%);
  color: var(--bc);
  box-shadow: inset 0 0 0 1px rgb(var(--bc-rgb) / 22%);
}

.tile.on::before { background: var(--bc); }

.tile .ico { width: 19px; height: 19px; stroke: currentColor; stroke-width: 1.6; fill: none; stroke-linecap: round; stroke-linejoin: round; }
/*
  ⚠️ 名字<b>允许折成两行</b>，不是省略号。瓷砖内宽实测只有 <b>124px</b>，而
  「设置 - 系统套接字」中文就要 97、英文 "Set - System Socket" 125、日文 130、
  越南语 152、俄语最长那条 186 —— 一行一律放不下。
  折两行之后最长的俄语（186 / 124 ≈ 1.5 行）也装得进，七种语言全部不截断。
  clamp 到 2 行是兜底：真有更长的文案时截在第二行，而不是把瓷砖顶变形。
*/
.tile .nm {
  max-width: 100%;
  display: -webkit-box;
  -webkit-line-clamp: 2;
  -webkit-box-orient: vertical;
  overflow: hidden;
  font-size: var(--fs-small);
  line-height: 1.25;
  text-align: center;
  overflow-wrap: anywhere;
}

/* 参数区：全屏<b>唯一</b>一处表单 —— 改造前这儿是 12 份同时摊开的 */
.pcfg {
  min-height: 0;
  overflow: auto;
  display: flex;
  flex-direction: column;
  gap: 9px;
  padding: 12px;
  border-top: 1px solid var(--border);
  background: rgb(var(--inset-rgb) / 16%);
}

.ph { display: flex; align-items: center; gap: 8px; margin-bottom: 1px; }
.ph .dot { flex: none; width: 6px; height: 6px; background: var(--bc); box-shadow: 0 0 6px rgb(var(--bc-rgb) / 60%); }
.ph .nm { font-family: var(--share); font-size: var(--fs-label); letter-spacing: .12em; text-transform: uppercase; color: var(--bc); }

.fr { display: flex; flex-wrap: wrap; align-items: center; gap: 8px; row-gap: 6px; min-width: 0; }   /* 长语言（日 / 越 / 俄）的延迟区间一行放不下，允许折行 */
.grow { flex: 1; min-width: 0; }
.lb { flex: none; font-size: var(--fs-body); color: var(--muted); white-space: nowrap; }
.note { font-size: var(--fs-small); color: var(--dim2); line-height: 1.55; }

/* 下拉宽度类叫 .dd 不叫 .sel —— 指令块的选中态也是 .sel，裸 .sel 会作用到选中的那一块上 */
.dd { width: 100%; }
.dd.half { flex: 1; width: auto; }

.inp { width: 100%; }
.inp.num { flex: none; width: 76px; text-align: center; }

/* 只读的捕获框：光标不闪、聚焦时整框转绿表示「正在录」 */
.inp.cap-key { cursor: pointer; caret-color: transparent; }
.inp.cap-key:focus { border-color: var(--green); box-shadow: inset 0 0 0 1px rgb(var(--green-rgb) / 25%); }

.rd { flex: none; }
.rd.on { color: var(--gray); }
.rd.on i { border-color: var(--green); }
.rd.on i::after { background: var(--green); box-shadow: none; }

.pfoot { display: flex; flex-direction: column; gap: 8px; padding: 12px; border-top: 1px solid var(--border); }

/* 插入按钮跟着当前块的色走 —— 按下去会得到什么颜色的积木，按之前就知道 */
.btn.ins {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  gap: 7px;
  border-color: rgb(var(--bc-rgb) / 50%);
  color: var(--bc);
}

.btn.ins:hover:not(:disabled) { background: rgb(var(--bc-rgb) / 10%); border-color: var(--bc); color: var(--bc); }
.btn.ins .ico { width: 12px; height: 12px; stroke: currentColor; stroke-width: 2.2; fill: none; }

/* ══ 程序流 ══ */
.flow { min-width: 0; min-height: 0; display: flex; flex-direction: column; }

.fh {
  flex: none;
  display: flex;
  align-items: center;
  gap: 10px;
  height: var(--th-h);
  padding: 0 14px;
  border-bottom: 1px solid var(--border);
  background: var(--panel);
  font-family: var(--share);
  font-size: var(--th-size);
  letter-spacing: .14em;
  text-transform: uppercase;
  color: var(--th-fg);
}

.fh .ft-n { font-family: var(--mono); letter-spacing: 0; text-transform: none; color: var(--muted); }
.fh .warn { letter-spacing: .04em; text-transform: none; color: var(--danger); }

.fbody { flex: 1; min-height: 0; overflow: auto; padding: 10px 14px 14px; display: flex; flex-direction: column; gap: 5px; }
.empty { padding: 60px 20px; text-align: center; color: var(--dim2); font-size: var(--fs-body); line-height: 1.7; }

/*
  一条指令 = 一块积木。左边是两位步号，接着是 depth 条竖轨（嵌套），最后是本体。
  竖轨与循环块同为琥珀 —— 「这一段被那个循环圈住」是同一件事，用同一个颜色说。
*/
.blk { display: flex; align-items: stretch; min-height: 34px; cursor: default; }

.blk .stp {
  flex: none;
  width: 26px;
  display: flex;
  align-items: center;
  font-family: var(--mono);
  font-size: var(--fs-small);
  font-variant-numeric: tabular-nums;
  color: var(--dim);
}

.rail { flex: none; width: 16px; margin-left: 3px; border-left: 1px dashed rgb(var(--amber-rgb) / 38%); }

.face {
  flex: 1;
  min-width: 0;
  display: flex;
  align-items: center;
  gap: 9px;
  padding: 0 8px 0 10px;
  border: 1px solid var(--border);
  border-left: 3px solid var(--bc);
  background: rgb(var(--inset-rgb) / 24%);
}

.blk:hover .face { border-color: rgb(var(--bc-rgb) / 45%); border-left-color: var(--bc); background: rgb(var(--bc-rgb) / 6%); }
.blk.sel .face { border-color: rgb(var(--cyan-rgb) / 55%); background: rgb(var(--cyan-rgb) / 10%); }

/* 正在执行的那一块：整块点亮 + 左轨加粗，扫过去时看得见「现在走到哪」 */
.blk.run .face { border-color: var(--cyan); background: rgb(var(--cyan-rgb) / 14%); box-shadow: 0 0 10px rgb(var(--cyan-rgb) / 22%); }
.blk.run .stp { color: var(--cyan); }
.blk.bad .face { border-color: var(--danger); background: rgb(var(--danger-rgb) / 10%); }

.face .ico { flex: none; width: 15px; height: 15px; stroke: var(--bc); stroke-width: 1.7; fill: none; stroke-linecap: round; stroke-linejoin: round; }
.face .ty { flex: none; font-size: var(--fs-body); color: var(--bc); }
.face .ct { min-width: 0; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; font-family: var(--mono); font-size: var(--fs-body); color: var(--gray); }

/* 循环那两块自己不装内容，让名字占满，读起来像一对括号 */
.blk.lp .face { background: rgb(var(--amber-rgb) / 7%); }
.blk.lp .face .ty { font-family: var(--share); letter-spacing: .08em; }

.tg {
  flex: none;
  padding: 3px 7px;
  border: 1px solid;
  font-family: var(--share);
  font-size: var(--fs-caption);
  line-height: 1;
  letter-spacing: .06em;
}

.tg.bad { border-color: rgb(var(--danger-rgb) / 55%); color: var(--danger); }

.op {
  flex: none;
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 22px;
  height: 22px;
  background: transparent;
  border: 1px solid transparent;
  color: var(--dim);
  cursor: pointer;
}

.op:hover:not(:disabled) { border-color: var(--danger); color: var(--danger); }
.op:disabled { opacity: .3; cursor: default; }
.op .ico { width: 12px; height: 12px; stroke: currentColor; stroke-width: 2; fill: none; }

/* ── 页脚 ── */
.ft {
  flex: none;
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 12px 18px;
  border-top: 1px solid var(--border);
  background: var(--panel);
}

.err { flex: 1; min-width: 0; font-size: var(--fs-small); color: var(--danger); }

/*
  ⚠️ 矮窗口下让调色板自己滚，别让整个弹窗滚 —— 程序流是要跟着执行看的，
  一滚就把正在跑的那一块推走了（与代理数据页那两块地板同一条口径）。
*/
@media (max-height: 700px) {
  .tiles { gap: 5px; padding: 8px 12px 10px; }
  .tile { min-height: 64px; padding: 8px 4px 7px; gap: 4px; }
  .blk { min-height: 30px; }
}
</style>
