<script setup lang="ts">
/*
  机器人编辑 —— 对应 WinForms 的 Controls/RobotEdit（1000 + 2217 行）。

  左边是指令面板：四组（封包 / 控制 / 键盘 / 鼠标）共 12 种插入方式，每一种就是「填参数 → 插入」。
  右边是名称 + 执行条 + 指令集表；指令集就是这个机器人要按顺序做的事。

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
import { t } from '../../i18n'
import { comboOf, isModifierEvent, keyNameOf } from '../../keys'
import { useList } from '../../stores/lists'
import { pushToast } from '../../stores/toast'
import { useRowPick } from '../../usePick'
import ContextMenu from '../ContextMenu.vue'
import CyberSelect from '../CyberSelect.vue'
import { ICON, type MenuItem } from '../menu'

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

/* ── 指令面板的状态 ─────────────────────────────────────────── */

const sends = useList<SendRow>(FeedList.Send)
const robots = useList<RobotRow>(FeedList.Robot)
const filters = useList<FilterRow>(FeedList.Filter)

//四组折叠：照 WinForms，封包指令默认收起、其余展开
const open = ref<Record<'packet' | 'control' | 'key' | 'mouse', boolean>>({ packet: false, control: true, key: true, mouse: true })

//发送 - 发送列表
const sendSel = ref('')
const sendOptions = computed(() => sends.value.map((s) => ({ value: s.Id, label: s.Name })))
watch(sendOptions, (o) => { if (!o.some((x) => x.value === sendSel.value)) sendSel.value = o[0]?.value ?? '' }, { immediate: true })

//设置 - 系统套接字
const sockMode = ref<'packet' | 'filter' | 'custom'>('packet')
const sockNum = ref(0)

function sockContent(): string {
  if (sockMode.value === 'packet') return 'PacketConfig.List'
  if (sockMode.value === 'filter') return 'FilterSocket'
  return 'Customize|' + Math.trunc(sockNum.value || 0)
}

//延迟
const delayMode = ref<'fix' | 'random'>('fix')
const delayFix = ref(100)
const delayFrom = ref(1)
const delayTo = ref(100)

function delayContent(): string {
  if (delayMode.value === 'fix') return String(Math.trunc(delayFix.value || 0))
  return Math.trunc(delayFrom.value || 0) + '-' + Math.trunc(delayTo.value || 0)
}

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

function switchContent(): string {
  return (switchOn.value ? 'Enable' : 'Disable') + '|' + switchType.value + '|' + switchTarget.value.toUpperCase()
}

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

/* ── 执行 ───────────────────────────────────────────────────── */

function stopPoll(): void {
  if (poll) { clearInterval(poll); poll = 0 }
}

/*
  轨迹只显示尾巴：循环一多它会长到几千个数，而看的人关心的是「现在走到哪了」。
  <b>不要用 direction: rtl 来「靠右截断」</b>——bidi 会把标点顺序颠过来，"1, 2" 画成 "2 ,1"（真出过）。
  完整的一串在 title 里，悬停能看。
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
  tbody.value?.querySelector('.row2.bad')?.scrollIntoView({ block: 'nearest' })
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
</script>

<template>
  <div v-if="props.id !== null" class="editor-mask" @mousedown.self="close">
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

      <div v-else class="bd" :class="{ running: prog.Running }">
        <!-- ══ 左：指令面板 ══ -->
        <aside class="pal" :inert="prog.Running">
          <!-- 封包指令 -->
          <section class="grp" :class="{ open: open.packet }">
            <button class="gh" @click="open.packet = !open.packet">
              <svg class="ico" viewBox="0 0 24 24"><path d="M9 6l6 6-6 6" /></svg>{{ t('rb.e.gPacket') }}
            </button>
            <div v-show="open.packet" class="gb">
              <div class="cap">{{ t('rb.e.sendList') }}</div>
              <div class="fr">
                <CyberSelect v-model="sendSel" :options="sendOptions" :placeholder="t('rb.e.pick')" class="sel grow" />
                <button class="btn mini" :disabled="!sendSel" @click="add(IT.SendSendList, sendSel)">{{ t('rb.e.insert') }}</button>
              </div>

              <div class="cap">{{ t('rb.e.packetList') }}</div>
              <div class="fr">
                <span class="lb grow">{{ t('rb.e.packetListHint') }}</span>
                <button class="btn mini" @click="add(IT.SendPacketList, '')">{{ t('rb.e.insert') }}</button>
              </div>
              <div class="note">{{ t('rb.e.packetListNote') }}</div>

              <div class="cap">{{ t('rb.e.sysSocket') }}</div>
              <div class="fc">
                <button class="rd" :class="{ on: sockMode === 'packet' }" @click="sockMode = 'packet'"><i />{{ t('rb.e.sockPacket') }}</button>
                <button class="rd" :class="{ on: sockMode === 'filter' }" @click="sockMode = 'filter'"><i />{{ t('rb.e.sockFilter') }}</button>
                <div class="fr">
                  <button class="rd" :class="{ on: sockMode === 'custom' }" @click="sockMode = 'custom'"><i />{{ t('rb.e.sockCustom') }}</button>
                  <input v-model.number="sockNum" class="inp num" type="number" min="0" max="2147483647" :disabled="sockMode !== 'custom'">
                  <span class="grow" />
                  <button class="btn mini" @click="add(IT.SetSystemSocket, sockContent())">{{ t('rb.e.insert') }}</button>
                </div>
              </div>
            </div>
          </section>

          <!-- 控制指令 -->
          <section class="grp" :class="{ open: open.control }">
            <button class="gh" @click="open.control = !open.control">
              <svg class="ico" viewBox="0 0 24 24"><path d="M9 6l6 6-6 6" /></svg>{{ t('rb.e.gControl') }}
            </button>
            <div v-show="open.control" class="gb">
              <div class="cap">{{ t('rb.e.delay') }}</div>
              <div class="fc">
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
                  <span class="grow" />
                  <button class="btn mini" @click="add(IT.Delay, delayContent())">{{ t('rb.e.insert') }}</button>
                </div>
              </div>

              <div class="cap">{{ t('rb.e.loop') }}</div>
              <div class="fr">
                <input v-model.number="loopCount" class="inp num" type="number" min="1" max="999999999">
                <span class="lb">{{ t('rb.e.loopTimes') }}</span>
                <span class="grow" />
                <button class="btn mini" @click="add(IT.LoopStart, String(Math.trunc(loopCount || 0)))">{{ t('rb.e.begin') }}</button>
                <button class="btn mini" @click="add(IT.LoopEnd, '')">{{ t('rb.e.end') }}</button>
              </div>

              <div class="cap">{{ t('rb.e.sw') }}</div>
              <div class="fc">
                <div class="fr">
                  <button class="rd" :class="{ on: switchOn }" @click="switchOn = true"><i />{{ t('rb.e.on') }}</button>
                  <button class="rd" :class="{ on: !switchOn }" @click="switchOn = false"><i />{{ t('rb.e.off') }}</button>
                </div>
                <div class="fr">
                  <CyberSelect v-model="switchType" :options="switchTypeOptions" class="sel half" />
                  <CyberSelect v-model="switchTarget" :options="switchTargetOptions" :placeholder="t('rb.e.pick')" class="sel grow" />
                  <button class="btn mini" :disabled="!switchTarget" @click="add(IT.Switch, switchContent())">{{ t('rb.e.insert') }}</button>
                </div>
              </div>
            </div>
          </section>

          <!-- 键盘指令 -->
          <section class="grp" :class="{ open: open.key }">
            <button class="gh" @click="open.key = !open.key">
              <svg class="ico" viewBox="0 0 24 24"><path d="M9 6l6 6-6 6" /></svg>{{ t('rb.e.gKey') }}
            </button>
            <div v-show="open.key" class="gb">
              <div class="cap">{{ t('rb.e.key') }}</div>
              <div class="fr">
                <span class="lb">{{ t('rb.e.keyType') }}</span>
                <CyberSelect v-model="keyType" :options="keyTypeOptions" class="sel half" />
                <input :value="keyName" class="inp grow cap-key" readonly :placeholder="t('rb.e.keyPh')" @keydown="onKeyCapture">
                <button class="btn mini" :disabled="!keyName" @click="add(IT.KeyBoard, KEY_TYPES[keyType] + '|' + keyName)">{{ t('rb.e.insert') }}</button>
              </div>

              <div class="cap">{{ t('rb.e.combo') }}</div>
              <div class="fr">
                <input :value="combo" class="inp grow cap-key" readonly :placeholder="t('rb.e.comboPh')" @keydown="onComboCapture">
                <button class="btn mini" :disabled="!combo" @click="add(IT.KeyBoard, 'Combine|' + combo)">{{ t('rb.e.insert') }}</button>
              </div>

              <div class="cap">{{ t('rb.e.text') }}</div>
              <div class="fr">
                <input v-model="text" class="inp grow" spellcheck="false" :placeholder="t('rb.e.textPh')">
                <button class="btn mini" :disabled="!text.trim()" @click="add(IT.KeyBoard, 'Text|' + text.trim())">{{ t('rb.e.insert') }}</button>
              </div>
            </div>
          </section>

          <!-- 鼠标指令 -->
          <section class="grp" :class="{ open: open.mouse }">
            <button class="gh" @click="open.mouse = !open.mouse">
              <svg class="ico" viewBox="0 0 24 24"><path d="M9 6l6 6-6 6" /></svg>{{ t('rb.e.gMouse') }}
            </button>
            <div v-show="open.mouse" class="gb">
              <div class="cap">{{ t('rb.e.mouseKey') }}</div>
              <div class="fr">
                <CyberSelect v-model="mouseKey" :options="mouseKeyOptions" class="sel grow" />
                <button class="btn mini" @click="add(IT.Mouse, MOUSE_KEYS[mouseKey] + '|')">{{ t('rb.e.insert') }}</button>
              </div>

              <div class="cap">{{ t('rb.e.wheel') }}</div>
              <div class="fr">
                <span class="lb">{{ t('rb.e.scroll') }}</span>
                <CyberSelect v-model="wheelDir" :options="wheelOptions" class="sel half" />
                <span class="lb">{{ t('rb.e.distance') }}</span>
                <input v-model.number="wheelDist" class="inp num" type="number" min="1" max="99999">
                <span class="grow" />
                <button class="btn mini" @click="add(IT.Mouse, wheelDir + '|' + Math.trunc(wheelDist || 0))">{{ t('rb.e.insert') }}</button>
              </div>

              <div class="cap">{{ t('rb.e.move') }}</div>
              <div class="fc">
                <div class="fr">
                  <button class="rd" :class="{ on: moveMode === 'MoveTo' }" @click="moveMode = 'MoveTo'"><i />{{ t('rb.e.moveTo') }}</button>
                  <button class="rd" :class="{ on: moveMode === 'MoveBy' }" @click="moveMode = 'MoveBy'"><i />{{ t('rb.e.moveBy') }}</button>
                </div>
                <div class="fr">
                  <span class="lb">X</span>
                  <input v-model.number="mx" class="inp num" type="number" min="-99999" max="99999">
                  <span class="lb">Y</span>
                  <input v-model.number="my" class="inp num" type="number" min="-99999" max="99999">
                  <span class="grow" />
                  <button class="btn mini" @click="add(IT.Mouse, moveMode + '|' + Math.trunc(mx || 0) + ', ' + Math.trunc(my || 0))">{{ t('rb.e.insert') }}</button>
                </div>
              </div>
            </div>
          </section>

          <div class="note tail">{{ t('rb.e.insertHint') }}</div>
        </aside>

        <!-- ══ 右：名称 + 执行条 + 指令集 ══ -->
        <div class="main">
          <div class="row" :class="{ off: prog.Running }">
            <div class="k">{{ t('col.robotName') }}</div>
            <div class="v">
              <input v-model="f.Name" class="inp" spellcheck="false" :disabled="prog.Running" :placeholder="t('rb.e.namePh')">
            </div>
          </div>

          <div class="runbar">
            <button class="btn run" :class="{ on: prog.Running }" :disabled="!rows.length" @click="toggleRun">
              <svg v-if="prog.Running" class="ico" viewBox="0 0 24 24"><rect x="6" y="6" width="12" height="12" /></svg>
              <svg v-else class="ico" viewBox="0 0 24 24"><path d="M7 4l13 8-13 8z" /></svg>
              {{ prog.Running ? t('rb.e.stop') : t('rb.e.execute') }}
            </button>

            <span class="cnt run">{{ t('col.execCount') }} <b>{{ prog.Total }}</b></span>

            <!--
              执行轨迹：WinForms 右上那个 txtINSTLog 的 "1, 2, 3, "；长了就在框里横向滚。

              ⚠️ <b>:data-tip 而不是 :title</b> —— 跑起来之后这串每 200ms 跟着轮询变长一次，
              写 title 就是每 200ms 往元素上安一次原生提示（详见 tooltip.ts 头上「两道防线」）。
            -->
            <span class="trail" :data-tip="prog.Trail">
              <span class="tl">{{ t('rb.e.trail') }}</span>
              <span class="tv">{{ trailTail || '—' }}</span>
            </span>
          </div>

          <div class="tbl list-page">
            <div ref="tbody" class="tbody">
              <div class="head">
                <span class="no">{{ t('rb.e.colInst') }}</span>
                <span class="ty">{{ t('rb.e.colType') }}</span>
                <span class="ct">{{ t('rb.e.colContent') }}</span>
                <span class="ops">{{ t('col.ops') }}</span>
              </div>

              <div v-if="!rows.length" class="empty">{{ t('rb.e.empty') }}</div>

              <div
                v-for="(r, i) in rows"
                v-else
                :key="i"
                class="row2"
                :class="['t' + r.Type, { sel: picked.has(String(r.Index)), run: prog.Running && prog.Index === i, bad: badIndex === i }]"
                @click="onRowClick(r, $event, i)"
                @contextmenu.prevent="menuAt = { x: $event.clientX, y: $event.clientY }"
              >
                <span class="no">{{ t('rb.e.inst') }} {{ i + 1 }}</span>
                <span class="ty">{{ r.TypeName }}</span>
                <span class="ct" :title="r.Content">{{ r.Text || r.Content }}</span>
                <span class="ops">
                  <button class="op del" :title="t('acct.op.del')" :disabled="prog.Running"
                          @click.stop="picked = new Set([String(r.Index)]); onMenuPick('delete')">
                    <svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg>
                  </button>
                </span>
              </div>
            </div>
          </div>
        </div>
      </div>

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
  </div>
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
.tt .zh { font-family: var(--orbit); font-size: 14px; color: var(--gray); letter-spacing: .04em; }
.tt .sub { font-family: var(--share); font-size: 10px; letter-spacing: .14em; text-transform: uppercase; color: var(--dim); }

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

.loading { padding: 60px 0; text-align: center; color: var(--muted); font-size: 12.5px; }

/* 左右两栏：面板定宽、右边吃掉剩下的；两栏各自滚，弹窗本身不滚 */
.bd {
  flex: 1;
  min-height: 0;
  display: grid;
  grid-template-columns: 372px 1fr;
  overflow: hidden;
}

/* ── 指令面板 ── */
.pal {
  min-height: 0;
  overflow-y: auto;
  overflow-x: hidden;   /* 某一行再超宽也只裁，不出横向滚动条 */
  border-right: 1px solid var(--border);
  background: rgb(var(--inset-rgb) / 18%);
}

.pal[inert] { opacity: .45; }

.grp { border-bottom: 1px solid var(--border); }

.gh {
  width: 100%;
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 11px 14px 9px;
  background: var(--panel);
  border: 0;
  color: var(--th-fg);
  font-family: var(--share);
  font-size: var(--th-size);
  line-height: 1;
  letter-spacing: .14em;
  text-transform: uppercase;
  text-align: left;
  cursor: pointer;
}

.gh:hover { color: var(--cyan); }
.gh .ico { width: 11px; height: 11px; stroke: currentColor; stroke-width: 2; fill: none; transition: transform .15s; position: relative; top: -1px; }
.grp.open .gh .ico { transform: rotate(90deg); }

.gb { padding: 6px 14px 12px; display: flex; flex-direction: column; gap: 8px; }

/* 小节标题：与 WinForms 的 Divider 一个意思 */
.cap {
  margin-top: 6px;
  padding-bottom: 4px;
  border-bottom: 1px dashed var(--border);
  font-size: 11.5px;
  color: var(--cyan);
  letter-spacing: .04em;
}

.gb > .cap:first-child { margin-top: 0; }

.fr { display: flex; align-items: center; gap: 8px; min-width: 0; }
.fc { display: flex; flex-direction: column; gap: 8px; }
.grow { flex: 1; min-width: 0; }
.lb { flex: none; font-size: 12px; color: var(--muted); white-space: nowrap; }

.note { font-size: 11px; color: var(--dim2); line-height: 1.5; }
.note.tail { padding: 10px 14px 14px; }

.sel { flex: none; }
.sel.half { width: 112px; }
.sel.grow { flex: 1; width: auto; }

.inp {
  flex: 1;
  min-width: 0;
  height: 28px;
  padding: 0 10px;
  background: rgb(var(--inset-rgb) / 30%);
  border: 1px solid var(--border);
  color: var(--gray);
  font-family: var(--mono);
  font-size: 12.5px;
  outline: none;
}

.inp:focus { border-color: var(--cyan); }
.inp:disabled { opacity: .5; }
/*
  76px 而不是发送编辑那边的 92px：面板只有 372px 宽，「随机 [从] - [到] 毫秒 插入」和
  「滚动 [向上] 距离 [10] 插入」两行是按这个宽度算出来刚好放下的（实测 88px 时溢出 15–19px，面板底下多一条横向滚动条）。
*/
.inp.num { flex: none; width: 76px; text-align: center; }

/* 按键捕获框：只读但要能拿焦点，焦点时青边提示「现在按」 */
.inp.cap-key { cursor: pointer; caret-color: transparent; }
.inp.cap-key:focus { border-color: var(--green); box-shadow: inset 0 0 0 1px rgb(var(--green-rgb) / 25%); }

/* 单选：与全项目的勾选框同一种画法，圆的 */
.rd {
  display: inline-flex;
  align-items: center;
  gap: 7px;
  padding: 0;
  background: transparent;
  border: 0;
  font-size: 12.5px;
  color: var(--muted);
  cursor: pointer;
  white-space: nowrap;
  flex: none;
}

.rd i { width: 13px; height: 13px; border: 1px solid var(--dim); border-radius: 50%; position: relative; flex: none; }
.rd.on { color: var(--gray); }
.rd.on i { border-color: var(--green); }
.rd.on i::after { content: ""; position: absolute; inset: 3px; border-radius: 50%; background: var(--green); }

/* ── 右栏 ── */
.main { min-width: 0; min-height: 0; display: flex; flex-direction: column; padding: 10px 0 12px; }
.main > .row, .main > .runbar { flex: none; }

.row {
  display: grid;
  grid-template-columns: 92px 1fr;
  align-items: center;
  gap: 12px;
  padding: 3px 18px;
  min-height: 30px;
}

.row > .k { font-size: 12.5px; color: var(--muted); }
.row > .v { display: flex; align-items: center; gap: 12px; min-width: 0; }
.row.off > .k { opacity: .45; }

.runbar {
  display: flex;
  align-items: center;
  gap: 12px;
  margin: 8px 18px 6px;
  padding: 7px 12px;
  border: 1px solid var(--border);
  background: rgb(var(--inset-rgb) / 20%);
  min-width: 0;
}

.runbar .cnt { flex: none; font-size: 12px; color: var(--muted); font-family: var(--share); letter-spacing: .06em; }
.runbar .cnt b { font-family: var(--mono); font-variant-numeric: tabular-nums; }
.runbar .cnt.run b { color: var(--cyan); }

.trail { flex: 1; min-width: 0; display: flex; align-items: center; gap: 8px; }
.trail .tl { flex: none; font-size: 12px; color: var(--muted); font-family: var(--share); letter-spacing: .06em; }
.trail .tv { flex: 1; min-width: 0; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; font-family: var(--mono); font-size: 12px; color: var(--acc-green2); }

.btn {
  flex: none;
  padding: 9px 13px 7px;   /* 上 +1 下 -1：字形在 em 框里偏上 1px（上伸 9 / 下伸 3，实测），补回来 */
  background: transparent;
  border: 1px solid var(--border);
  color: var(--gray);
  font-family: var(--share);
  font-size: var(--btn-size);
  /* 显式 1：Share Tech Mono 在 line-height: normal 下会把行距全压在字的下面，字号一大就明显偏上（实测） */
  line-height: 1;
  letter-spacing: .12em;
  text-transform: uppercase;
  cursor: pointer;
  white-space: nowrap;
}

.btn:hover:not(:disabled) { border-color: var(--cyan); color: var(--cyan); }
.btn:disabled { opacity: .35; cursor: default; }
.btn.primary { border-color: rgb(var(--green-rgb) / 45%); color: var(--green); }
.btn.primary:hover:not(:disabled) { background: rgb(var(--green-rgb) / 10%); border-color: var(--green); color: var(--green); }
.btn.mini { padding: 8px 11px 6px; }

.btn.run { display: inline-flex; align-items: center; gap: 7px; border-color: rgb(var(--green-rgb) / 45%); color: var(--green); }
.btn.run .ico { width: 11px; height: 11px; margin: -1px 0; flex: none; position: relative; top: -1px; }
.btn.run.on { border-color: rgb(var(--danger-rgb) / 30%); color: var(--danger); }
.btn.run:hover:not(:disabled) { background: rgb(var(--green-rgb) / 10%); border-color: var(--green); color: var(--green); }
.btn.run.on:hover:not(:disabled) { background: rgb(var(--danger-rgb) / 12%); border-color: var(--danger); color: var(--danger); }
.btn.run.on .ico { animation: pulse 1.1s ease-in-out infinite; }

@keyframes pulse { 50% { opacity: .25; } }

@media (prefers-reduced-motion: reduce) {
  .btn.run.on .ico { animation: none; }
}

/* 指令集表 */
.tbl {
  flex: 1;
  min-height: 0;
  display: flex;
  flex-direction: column;
  margin: 0 18px 2px;
  border: 1px solid var(--border);
  background: var(--sink);
}

.tbody { flex: 1; min-height: 0; overflow-y: auto; }

.head,
.row2 {
  display: grid;
  grid-template-columns: 78px 84px minmax(160px, 1fr) 40px;
  align-items: center;
  gap: 8px;
  padding: 0 14px;   /* 必须与 style.css 里 .list-page .head 的 14px 一致，否则表头比内容错 4px */
  font-size: 12.5px;
}

.row2 {
  height: 30px;
  border-bottom: 1px solid rgb(var(--border-rgb) / 45%);
  color: var(--soft);
  cursor: default;
}

.row2:hover { background: rgb(var(--tint-rgb) / 4%); }
.row2.sel { background: rgb(var(--cyan-rgb) / 6%); box-shadow: inset 2px 0 0 var(--cyan); }
.row2.run { background: rgb(var(--green-rgb) / 12%); box-shadow: inset 2px 0 0 var(--green); }
.row2.bad { background: rgb(var(--danger-rgb) / 12%); box-shadow: inset 2px 0 0 var(--danger); }

.row2 > span,
.head > span { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }

.head > span,
.row2 > span { text-align: left; }

.head > span.ops,
.row2 > span.ops { display: flex; align-items: center; justify-content: center; }

.no { color: var(--cyan); font-variant-numeric: tabular-nums; }
.ct { color: var(--soft); }

/* 类型列按指令类型着色，照 UiTheme.GetColor_ByInstructionType（YellowGreen / Khaki / Orchid / LightSeaGreen / LightSkyBlue / Violet / DarkOrange）*/
.row2.t0 .ty, .row2.t6 .ty { color: #9acd32; }
.row2.t1 .ty { color: #f0e68c; }
.row2.t2 .ty, .row2.t3 .ty { color: #da70d6; }
.row2.t4 .ty { color: #20b2aa; }
.row2.t5 .ty { color: #87cefa; }
.row2.t7 .ty { color: #ee82ee; }
.row2.t8 .ty { color: #ff8c00; }

.empty { padding: 40px 20px; text-align: center; color: var(--muted); font-size: 12.5px; line-height: 1.8; }

.ft {
  flex: none;
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 12px 18px;
  border-top: 1px solid var(--border);
  background: var(--panel);
}

.ft .err { font-size: 12px; color: var(--danger); }
.ft .grow { flex: 1; }
</style>
