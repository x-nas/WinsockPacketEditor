<script setup lang="ts">
/*
  注入模式的外壳 —— 对应 WinForms 的 Forms/InjectModeForm + Controls/ProcessList。

  【与代理模式最大的不同】数据来自<b>另一个进程</b>。
  钩子与滤镜引擎留在目标里（滤镜必须在目标的收发线程上同步给出答案，
  每包一次跨进程往返会把目标的每一次 send/recv 都拖慢），
  封包经命名管道过来，由 C# 侧的 ShellLink 还原成 PacketInfo 进 cqPacketInfo。
  进队之后的下游与代理模式<b>完全共用</b>：FlushToFeed → PacketRow → 封包页的表。

  所以这一屏有两个状态：
    ① 还没附加 —— 选目标（进程列表 / 启动并注入）
    ② 已附加   —— 侧栏 11 页（对应 InjectModeForm 那 11 个页签）+ 7 个设置弹窗

  ⚠️ 那 11 页里只有第一页「封包列表」是注入模式独有的，其余 10 页与代理模式
  <b>是同一个组件、同一份 stores/lists 数据源</b> —— 滤镜 / 发送 / 机器人 / 仓库
  四个子系统在 Operate 里本来就是两种模式共用的（WinForms 那边也是同一个 UserControl）。
*/
import { computed, nextTick, onMounted, onBeforeUnmount, ref, watch } from 'vue'
import { call, on } from '../bridge'
import { FeedList } from '../bridge/types'
import { injectFeed } from '../stores/packets'
import { attachListFeed } from '../stores/lists'
import { loadCountryTable } from '../flags'
import { gotoPage } from '../stores/runtime'
import { lang, t } from '../i18n'
import { pushToast } from '../stores/toast'
import { refresh as refreshStatus, setStatus, status, type InjectStatus } from '../stores/inject'
import { useSort } from '../useSort'
import ProxySide from './proxy/ProxySide.vue'
import SettingsModal from './proxy/SettingsModal.vue'
import { INJECT_GROUPS, INJECT_PAGES, type PageKey } from './proxy/pages'
import type { SettingKey } from './proxy/settings'
import InjectData from './inject/InjectData.vue'
//注入模式的另外 10 页：与代理模式同一个组件，只是从这边的侧栏进来
import FilterList from './proxy/FilterList.vue'
import SendList from './proxy/SendList.vue'
import RobotList from './proxy/RobotList.vue'
import WareHouseList from './proxy/WareHouseList.vue'
import StatData from './proxy/StatData.vue'
import TextCompare from './proxy/TextCompare.vue'
import XorCalc from './proxy/XorCalc.vue'
import Transcode from './proxy/Transcode.vue'
import ExtractData from './proxy/ExtractData.vue'
import SystemLog from './proxy/SystemLog.vue'
//7 个设置弹窗：代理那 12 项的真子集
import LeachSetting from './proxy/LeachSetting.vue'
import HookSetting from './proxy/HookSetting.vue'
import ListSetting from './proxy/ListSetting.vue'
import HotkeySetting from './proxy/HotkeySetting.vue'
import BackupSetting from './proxy/BackupSetting.vue'
import RemoteSetting from './proxy/RemoteSetting.vue'
import SystemSetting from './proxy/SystemSetting.vue'

interface ProcRow {
  ProcessName: string
  ProcessID: number
  ModuleName: string
  ProcessPath: string
}

/** 光标下那个窗口属于谁 —— 「选择窗体」进行中时由 C# 每换一个窗口推一次。 */
interface HoverInfo { pid: number; name: string; path: string; title: string }

const procs = ref<ProcRow[]>([])
/*
  上次注入的那一条记录（时间 / 方式 / 目标 / 路径 / 参数），由 C# 落库。

  ⚠️ 原来这里只有一个进程名 —— 够在启动页那张卡上显示一行，但不够重放：
  方式 03 是「挂起启动某个 exe」，没有完整路径与启动参数就重放不出同一件事。
  时间由 <b>C# 侧格式化好</b>（与 19 份列表的 DTO 同一条口径），前端不解析。
*/
interface LastInject { target: string; method: number; path: string; args: string; time: string }

const lastInject = ref<LastInject>({ target: '', method: 0, path: '', args: '', time: '' })
/** 「选择窗体」进行中；null = 没在选 */
const hover = ref<HoverInfo | null>(null)
const pickingWindow = ref(false)
const loadingProcs = ref(false)
const search = ref('')
const selectedPid = ref<number | null>(null)
const busy = ref(false)

/** 当前页。附加成功后落在封包列表上（对应 InjectModeForm 的第一个页签）。 */
const page = ref<PageKey>('packet')

/** 当前打开的设置弹窗。null = 没开。 */
const setting = ref<SettingKey | null>(null)

const dataRef = ref<InstanceType<typeof InjectData> | null>(null)

//别处（十六进制面板右键「添加到文本 A / B」）要求切页：切完清掉，下次还能再切同一页
watch(gotoPage, (k) => {
  if (!k) return
  if (INJECT_PAGES.some((p) => p.key === k)) page.value = k as PageKey
  gotoPage.value = null
})

/*
  进程列表按名字或 PID 搜；排序走全项目那一份 useSort（升 → 降 → 回到原始顺序三档）。
  原始顺序是 GetProcessList 里按进程名排好的，回得去才有意义。
*/
const filteredProcs = computed(() => {
  const q = search.value.trim().toLowerCase()
  if (!q) return procs.value

  //⚠️ 路径也要匹配 —— 它就摆在表里，搜不了会让人以为搜索坏了。
  //同名进程（好几个 svchost / chrome）恰恰只能靠路径分辨。
  return procs.value.filter(
    (p) =>
      p.ProcessName.toLowerCase().includes(q) ||
      String(p.ProcessID).includes(q) ||
      (p.ProcessPath || '').toLowerCase().includes(q),
  )
})

const sorter = useSort<ProcRow>(filteredProcs, {
  ProcessName: (p) => p.ProcessName,
  ProcessID: (p) => p.ProcessID,
})

const shownProcs = sorter.sorted

const picking = computed(() => status.value.state === 'idle')

/*
  ⚠️ 「选择窗体」进行中时，别的注入入口都要锁住。

  那件事在本程序<b>之外</b>进行（C# 装着 WH_MOUSE_LL 全局钩子等用户点别的窗口），
  这时候再从表里注入一个、或去开文件框选 exe，等于同时开两条注入路径 ——
  而全局鼠标钩子还挂着，文件框自己也要吃鼠标。

  ⚠️ 这个闸只加在<b>界面</b>上，不能加进 attachTo：pickWindow 选中之后正是靠它去附加的，
  那一刻 pickingWindow 还是 true（要等 finally 才落下）。
*/
/* ─────────────── 三种注入方式 ─────────────── */

/*
  这一屏只有三张卡（形制与启动页那两张模式卡同源），点哪张就弹哪一种「选目标」：

    01 进程   → 弹一张进程表（本文件里的 SettingsModal）
    02 窗体   → <b>不弹东西</b>：C# 把主窗口最小化，让用户在屏幕上点目标，注入完再还原
    03 文件   → 弹系统文件框，挑完再弹一个小确认（顺便填启动参数）

  ⚠️ 方式 02 的最小化 / 还原<b>做在 C# 里</b>（PickWindowAsync / FinishPickWindow），
  不是前端发两条命令 —— 取消、Esc、选中、装钩失败四条出口都汇到 FinishPickWindow，
  还原写在那儿才不会漏掉其中一条。
*/

/** 方式 01 的进程弹窗、方式 03 的文件确认弹窗。 */
const procOpen = ref(false)
const fileOpen = ref(false)

/** 方式 03 选中的可执行文件与它的启动参数。 */
const filePath = ref('')
const fileArgs = ref('')

const fileName = computed(() => filePath.value.replace(/^.*[\\/]/, ''))

/**
 * 打开进程弹窗。
 * @param useLastName 从「上次注入」那枚 chip 进来时把名字填进搜索框，一步定位到同一个目标
 */
function openProcs(useLastName = false): void {
  search.value = useLastName ? lastInject.value.target.replace(/\.exe$/i, '') : ''
  selectedPid.value = null
  procOpen.value = true

  //列表是进这一屏时取的，隔一会儿再打开往往已经过期 —— 顺手重新枚举一遍
  void refreshProcs()
}

/*
  ⚠️ 焦点要<b>抢在 SettingsModal 后面</b>。
  它自己也会在打开时把焦点移进弹窗，而它 querySelector 命中的是 DOM 里第一个可聚焦元素 ——
  标题栏那颗关闭按钮排在搜索框前面（实测焦点落在 BUTTON.x 上）。
  这一屏的键盘那几下（↑↓ / Enter）全挂在搜索框上，所以等两拍再抢一次。
*/
watch(procOpen, async (on) => {
  if (!on) return
  await nextTick()
  await nextTick()
  searchRef.value?.focus()
})

/** 方式 3：只挑文件，不立刻注入 —— 挑完还要给个填参数与确认的机会。 */
async function chooseFile(): Promise<void> {
  if (busy.value) return

  const p = await call<{ path: string }>('pickTargetExe').catch(() => null)
  if (!p?.path) return

  filePath.value = p.path
  fileOpen.value = true
  //可执行文件的图标走的是同一条批量接口（ExtractAssociatedIcon 对任意 exe 都认）
  void fetchIcons([{ ProcessName: '', ProcessID: 0, ModuleName: '', ProcessPath: p.path }])
}

/* ─────────────── 进程图标 ─────────────── */

/*
  照「进程设置」那一屏的做法：进程表回来之后按<b>去重路径一次批量取</b>，按路径记忆化。
  ⚠️ 别在 render 里逐个发桥调用 —— 二百多个进程就是二百多次往返加二百多次整表重渲染
  （那正是 ProcessSetting 当初改掉的写法）。iconOf 只是一次查表，没有副作用。
*/
const icons = ref<Record<string, string>>({})

function iconOf(path: string): string {
  return path ? (icons.value[path] ?? '') : ''
}

async function fetchIcons(rows: ProcRow[]): Promise<void> {
  const want = new Set<string>()
  for (const p of rows) if (p.ProcessPath && icons.value[p.ProcessPath] === undefined) want.add(p.ProcessPath)
  if (!want.size) return

  try {
    const r = await call<{ icons: Record<string, string> }>('getProcessIcons', { paths: [...want] })
    const next = { ...icons.value }
    //⚠️ 取不到也要写一个空串占位，否则下一次刷新又会把这一批重新问一遍
    for (const path of want) {
      const png = r?.icons?.[path]
      next[path] = png ? 'data:image/png;base64,' + png : ''
    }
    icons.value = next
  } catch {
    /* 桥没接上（探针页）；没有图标不影响选目标 */
  }
}

/* ─────────────── 键盘 ─────────────── */

const searchRef = ref<HTMLInputElement | null>(null)
const bodyRef = ref<HTMLElement | null>(null)

/** 让选中行留在视野里 —— 键盘走到表外面时不滚一下就等于走丢了。 */
function revealSelected(): void {
  void nextTick(() => {
    const el = bodyRef.value?.querySelector<HTMLElement>('.row.sel')
    el?.scrollIntoView({ block: 'nearest' })
  })
}

/*
  ⚠️ 键盘只挂在搜索框上，不挂 window。
  进程弹窗一开焦点就在搜索框里（watch(procOpen) 里抢了一次，见那儿的注释），手本来就在那儿；
  挂 window 则要另外判「焦点是不是在别的输入框里 / 另一个弹窗开着没有」，白多两层判断。

  ⚠️ 「选窗体」那一档的 Esc <b>不在这儿</b>，也不可能在页面里 ——
  那时窗口已经被 C# 最小化了，页面一个按键都收不到。接它的是 ShellForm.PickKeyHook
  （WH_KEYBOARD_LL 全局钩子里判 vkCode == Keys.Escape）。卡片上那句 CANCEL Esc 说的是它。

  ⚠️ 这里<b>不接 Esc</b>（2026-09-11 去掉）。原来是「Esc 清空搜索框」，可 SettingsModal 自己也接 Esc 关弹窗 ——
  两件事同时发生，用户看到的只有「按 Esc 弹窗没了」，清空那一半从来没被看见过。
  Esc 就只剩关弹窗这一个意思，与全项目别的弹窗一致；要清搜索框就删字。
*/
function onSearchKey(e: KeyboardEvent): void {
  const rows = shownProcs.value
  if (!rows.length) return

  /*
    ⚠️ Enter <b>只注入已经选中的那一行</b>；一行都没选时先把第一行选上。

    「没选中就注入第一条」看着更快，但注入是往<b>别的进程</b>里塞一个 DLL ——
    搜完顺手一个回车就打进某个系统进程，代价与「少按一下方向键」完全不成比例。
  */
  if (e.key === 'Enter') {
    e.preventDefault()
    if (selectedPid.value === null) { selectedPid.value = rows[0].ProcessID; revealSelected(); return }
    void attachTo(selectedPid.value)
    return
  }

  if (e.key !== 'ArrowDown' && e.key !== 'ArrowUp') return
  e.preventDefault()

  const cur = rows.findIndex((p) => p.ProcessID === selectedPid.value)
  const next = e.key === 'ArrowDown'
    ? (cur < 0 ? 0 : Math.min(cur + 1, rows.length - 1))
    : (cur < 0 ? rows.length - 1 : Math.max(cur - 1, 0))

  selectedPid.value = rows[next].ProcessID
  revealSelected()
}

/*
  ⚠️ 筛掉之后选中项要跟着收 —— 留着一个看不见的选中行，
  按 Enter 会注入到一个屏幕上根本没显示的进程。顺带把滚动位置退回顶部。
*/
watch(shownProcs, (rows) => {
  if (selectedPid.value !== null && !rows.some((p) => p.ProcessID === selectedPid.value)) {
    selectedPid.value = null
  }
})

watch(search, () => { if (bodyRef.value) bodyRef.value.scrollTop = 0 })

/*
  ⚠️ 从「已附加」退回选目标屏时要重新枚举一遍。
  列表是挂载那一次取的，卸载目标之后回来往往已经过了几分钟 ——
  屏幕上还是那份旧快照，里面躺着一批早就退出的进程（点了只会报注入失败）。
*/
watch(picking, (on, was) => {
  if (!on) { procOpen.value = false; fileOpen.value = false; return }
  //列表是挂载那一次取的，卸载目标之后回来往往过了几分钟，里面躺着一批早就退出的进程
  if (was === false) { void refreshProcs(); selectedPid.value = null }
})

let offHover: (() => void) | null = null
let offState: (() => void) | null = null
let offFeed: (() => void) | null = null
let offList: (() => void) | null = null
let poll = 0

onMounted(async () => {
  offFeed = injectFeed.attach()

  /*
    14 份中低频列表的接收端在这里挂，而不是在某一页里 ——
    侧栏的计数用得着它，切到别的页时也不该断掉。
    ⚠️ 顺序要紧：先订阅再通知 C#。反过来的话 C# 加载完立刻标脏，
    10ms 后的搬运拍就把整表推出来了，而此刻还没人订阅，那一批直接丢掉。
  */
  offList = attachListFeed()

  offHover = on('inject:hover', (d: HoverInfo) => { if (pickingWindow.value) hover.value = d })

  offState = on('inject:state', (d: InjectStatus) => {
    setStatus(d)

    if (d.state === 'disconnected') {
      //目标没了。<b>数据一条都不清</b> —— 用户还要看、还要导出（方案 3.5）
      pushToast('warning', t('inject.lost'))
    }
  })

  try {
    const r = await call<{ lastInject?: LastInject }>('enterInjectMode')
    if (r?.lastInject) lastInject.value = r.lastInject
    await refreshStatus()
  } catch {
    /* 桥没接上（浏览器里跑探针页），下面的按钮点了会各自报错 */
  }

  void typeSubtitle()

  //国旗用的中文国名对照表，整个会话取一次（约 4KB）
  void loadCountryTable()

  await refreshProcs()


  //丢包数与钩子状态只在事件里推，1 秒兜一次底就够（这几个数变得很慢）
  poll = window.setInterval(() => {
    if (status.value.state === 'idle') return
    void refreshStatus()
  }, 1000)
})

onBeforeUnmount(() => {
  //选窗口还开着就把全局钩子收掉 —— 这一屏没了，钩子留着没人卸
  if (pickingWindow.value) void call('cancelPickWindow').catch(() => {})

  offHover?.()
  offState?.()
  offFeed?.()
  offList?.()
  if (poll) window.clearInterval(poll)
})

async function refreshProcs(): Promise<void> {
  //⚠️ 防重入：枚举几百个进程 + 读 MainModule 是几十毫秒，连点两下就是两趟白跑
  if (loadingProcs.value) return
  loadingProcs.value = true

  try {
    procs.value = await call<ProcRow[]>('getInjectProcessList')
  } catch {
    procs.value = []
  } finally {
    loadingProcs.value = false
  }

  //图标不阻塞列表：表先画出来，图标回来了再补上（一次批量）
  void fetchIcons(procs.value)
}

/** 打字机副标题，与启动页 / 多开设置同一套。切语言要重打一遍，见 StartView 里的说明。 */
const typed = ref('')
let typeRun = 0

async function typeSubtitle(): Promise<void> {
  const mine = ++typeRun
  const text = t('inject.pick.subtitle')
  typed.value = ''

  for (let i = 1; i <= text.length; i++) {
    if (mine !== typeRun) return
    typed.value = text.slice(0, i)
    await new Promise((r) => setTimeout(r, 32))
  }
}

watch(lang, () => { void typeSubtitle() })

/** 方式号 → 卡片上那个名字。⚠️ 与三张卡的顺序一一对应，加卡片要回来补一项。 */
const METHOD_KEYS = ['inject.pick.mProc', 'inject.pick.window', 'inject.pick.mFile'] as const

const lastMethodName = computed(() => {
  const k = METHOD_KEYS[lastInject.value.method]
  return k ? t(k) : '—'
})

/** 有没有上次那条记录 —— 没有的话检测块显示「还没有注入过」，按钮压暗。 */
const hasLast = computed(() => !!(lastInject.value.target || lastInject.value.path))

/*
  快捷注入：照上次那条记录再来一遍。

  ⚠️ <b>目标解析与兜底全在 C# 那边</b>（ResolveQuickTarget）—— 前端只发一条命令、
  把回来的那句话显示出来。理由是「进程还在不在 / 文件还在不在」这两件事只有那一侧问得准，
  而且三种不成立（没注入过 / 文件没了 / 进程关了）各有各的下一步动作，一句笼统的
  「注入失败」在这儿最没用。

  ⚠️ 也<b>不能</b>把上次那个 pid 存下来重用：pid 会被系统回收，下次开机同一个号
  多半是别的进程 —— 那就成了「注进一个毫不相干的程序」。所以每次都按名字重新找。
*/
async function quickInject(): Promise<void> {
  if (busy.value || !hasLast.value) return
  busy.value = true

  try {
    const r = await call<any>('injectQuick')
    if (!r?.ok) { pushToast('error', r?.error || t('inject.failed')); return }

    setStatus(r)
    page.value = 'packet'
    pushToast('success', t('inject.attached'))
  } catch (e: any) {
    pushToast('error', String(e?.message || e))
  } finally {
    busy.value = false
  }
}

/*
  附加到一个已经在跑的进程。

  ⚠️ `method` 要传：方式 01（进程表）与 02（选窗体）到这一步是同一件事，
  C# 那边从参数上分辨不出来 —— 而「快捷注入」要照原样重放，得知道当初点的是哪张卡。
*/
async function attachTo(pid: number, method = 0): Promise<void> {
  if (busy.value) return
  busy.value = true

  try {
    const r = await call<any>('injectAttach', { pid, method })
    if (!r?.ok) { pushToast('error', r?.error || t('inject.failed')); return }
    setStatus(r)
    procOpen.value = false
    page.value = 'packet'
    pushToast('success', t('inject.attached'))
  } catch (e: any) {
    pushToast('error', String(e?.message || e))
  } finally {
    busy.value = false
  }
}

/*
  方式 3 的「启动并注入」。⚠️ 与早先那一版不同：<b>文件在上一步就挑好了</b>，
  这里只负责发命令 —— 挑完立刻注入的话，用户既看不清挑中的是哪个文件，
  也没有地方填启动参数（C# 侧的 injectAttach 一直收 args，只是界面从来没给过入口）。
*/
async function launchAndAttach(): Promise<void> {
  if (busy.value || !filePath.value) return

  busy.value = true
  try {
    const args = fileArgs.value.trim()
    const r = await call<any>('injectAttach', { pid: -1, path: filePath.value, args: args || null, method: 2 })
    if (!r?.ok) { pushToast('error', r?.error || t('inject.failed')); return }
    setStatus(r)
    fileOpen.value = false
    page.value = 'packet'
    pushToast('success', t('inject.launched'))
  } catch (e: any) {
    pushToast('error', String(e?.message || e))
  } finally {
    busy.value = false
  }
}

/*
  「选择窗体」—— 对应 WinForms 的 ProcessList.bSelectForm_Click。

  C# 那边装 WH_MOUSE_LL + WH_KEYBOARD_LL 两个低级钩子，用户在屏幕上点哪个窗口
  就选中哪个进程；悬停信息经 inject:hover 推过来。选中之后<b>直接注入</b>，
  与 WinForms 一致（那边是 ShowSelectProcess() 紧跟 DoInject()）。
*/
async function pickWindow(): Promise<void> {
  if (busy.value || pickingWindow.value) return

  pickingWindow.value = true
  hover.value = null

  try {
    const r = await call<{ ok: boolean; cancelled?: boolean; error?: string; pid: number; name: string }>('pickWindow')

    if (!r?.ok) {
      if (r?.error) pushToast('error', r.error)
      return
    }

    await attachTo(r.pid, 1)
  } catch (e: any) {
    pushToast('error', String(e?.message || e))
  } finally {
    pickingWindow.value = false
    hover.value = null
  }
}

/*
  卡 02 在「正在选窗体」这一档点下去是<b>取消</b>。

  ⚠️ 正常情况下这一下点不着 —— 选窗体时主窗口是最小化的，取消走 Esc（键盘钩子接）。
  留着它是为了两种漏网的情形：装钩成功但最小化没生效，
  以及用户自己从任务栏把窗口点回来了。那时屏幕上得有个出口。
*/
function togglePick(): void {
  if (pickingWindow.value) { cancelPickWindow(); return }
  void pickWindow()
}

function cancelPickWindow(): void {
  if (!pickingWindow.value) return
  void call('cancelPickWindow').catch(() => {})
}

async function toggleHook(): Promise<void> {
  if (busy.value) return
  busy.value = true

  try {
    const r = await call<any>(status.value.hooked ? 'injectStopHook' : 'injectStartHook')
    if (!r?.ok) { pushToast('error', r?.error || t('inject.failed')); return }
    setStatus(r)
  } catch (e: any) {
    pushToast('error', String(e?.message || e))
  } finally {
    busy.value = false
  }
}

async function clearList(): Promise<void> {
  try { await call('clearPackets', { list: FeedList.Packet }) } catch { /* 桥没接上 */ }
  injectFeed.clearLocal()
  dataRef.value?.onCleared()
}
</script>

<template>
  <div class="inject">
    <!-- ══════════ ① 还没附加：选目标 ══════════ -->
    <div v-if="picking" class="pickscr scrn">
      <!--
        标题块走 style.css 的共用件 `.scrn`（启动页 / 多开设置同一套）：
        短横 + 代号的 eyebrow，下面一行模式色的大标题。

        ⚠️ eyebrow 那句<b>不进字典</b> —— 它是 `WPE_X64 // …` 这种代号，七种语言都一样
        （与启动页的 `Mode 01` / `Ready` 同一条口径）。
      -->
      <div class="ptitle">
        <div class="eyebrow">
          <span class="dash" />
          <span class="lbl">WPE_X64 // Select Method</span>
        </div>

        <h2 class="ttl">{{ t('inject.pick.title') }}</h2>

        <!--
          第三行与启动页 / 多开设置<b>同一件东西</b>：一句短话 + 打字机 + 闪烁光标。
          ⚠️ 它原来是一整段说明（还卡着 max-width: 92ch）—— 那与另外两屏对不上，
          而且窗口拉多大都在同一个地方折行。三种方式各自的说明写在各自的卡片上，
          这一行只说「这一屏要你干什么」。
        -->
        <p class="subtitle">{{ typed }}<span class="cur" /></p>
      </div>

      <!--
        机架 + 三个槽位 —— 与启动页那两张模式卡<b>同一套构件</b>
        （色轨 / 机位号水印 / 状态灯 / 仪表窗 / 双行标题 / 读数条 / 常驻箭头）。
        ⚠️ 槽缝与机架内边距取同一个令牌，卡片到框的距离才处处一样。
      -->
      <div class="rack">
        <div class="cards">
          <!-- 01 · 进程：弹一张进程表出来挑 -->
          <div
            class="cd"
            role="button"
            tabindex="0"
            @click="openProcs()"
            @keydown.enter.prevent="openProcs()"
            @keydown.space.prevent="openProcs()"
          >
            <span class="rail" />
            <span class="wm">01</span>

            <div class="hd">
              <span class="num">Method 01</span>
              <span class="rdy"><i class="dot" />Ready</span>
            </div>

            <div class="t">
              <span class="well">
                <!-- 芯片：与启动页「注入模式」那张卡同一枚符号 -->
                <svg class="ico" viewBox="0 0 24 24">
                  <rect x="5" y="5" width="14" height="14" rx="1.5" />
                  <rect x="9.5" y="9.5" width="5" height="5" />
                  <path d="M9 2v3M15 2v3M9 19v3M15 19v3M2 9h3M2 15h3M19 9h3M19 15h3" />
                </svg>
              </span>
              <span class="tx">
                <b class="en">Process</b>
                <i class="zh">{{ t('inject.pick.mProc') }}</i>
              </span>
            </div>

            <p>{{ t('inject.pick.mProcD') }}</p>

            <div class="foot">
              <span class="last"><span class="k">Procs</span><b>{{ procs.length || '—' }}</b></span>
              <span class="ar">→</span>
            </div>
          </div>

          <!-- 02 · 窗体：最小化本窗口，让用户在屏幕上点目标 -->
          <div
            class="cd cy"
            :class="{ live: pickingWindow }"
            role="button"
            tabindex="0"
            @click="togglePick"
            @keydown.enter.prevent="togglePick"
            @keydown.space.prevent="togglePick"
          >
            <span class="rail" />
            <span class="wm">02</span>

            <div class="hd">
              <span class="num">Method 02</span>
              <span class="rdy"><i class="dot" />{{ pickingWindow ? 'Picking' : 'Ready' }}</span>
            </div>

            <div class="t">
              <span class="well">
                <!-- 准星 -->
                <svg class="ico" viewBox="0 0 24 24">
                  <circle cx="12" cy="12" r="7" />
                  <path d="M12 1v4M12 19v4M1 12h4M19 12h4" />
                </svg>
              </span>
              <span class="tx">
                <b class="en">Window</b>
                <i class="zh">{{ t('inject.pick.window') }}</i>
              </span>
            </div>

            <p>{{ pickingWindow ? t('inject.pick.pickHint') : t('inject.pick.wayWindow') }}</p>

            <div class="foot">
              <span class="last"><span class="k">Cancel</span><b>Esc</b></span>
              <span class="ar">→</span>
            </div>
          </div>

          <!-- 03 · 选择文件：弹系统文件框，挑完再确认参数 -->
          <div
            class="cd am"
            role="button"
            tabindex="0"
            @click="chooseFile"
            @keydown.enter.prevent="chooseFile"
            @keydown.space.prevent="chooseFile"
          >
            <span class="rail" />
            <span class="wm">03</span>

            <div class="hd">
              <span class="num">Method 03</span>
              <span class="rdy"><i class="dot" />Ready</span>
            </div>

            <div class="t">
              <span class="well">
                <!-- 文件 + 一枚播放三角：挑一个 exe 拉起来 -->
                <svg class="ico" viewBox="0 0 24 24">
                  <path d="M6 2h8l4 4v16H6z" />
                  <path d="M14 2v4h4" />
                  <path d="M10 12.5l4 2.5-4 2.5z" />
                </svg>
              </span>
              <span class="tx">
                <b class="en">File</b>
                <i class="zh">{{ t('inject.pick.mFile') }}</i>
              </span>
            </div>

            <p>{{ t('inject.pick.mFileD') }}</p>

            <div class="foot">
              <span class="last"><span class="k">Type</span><b>.exe</b></span>
              <span class="ar">→</span>
            </div>
          </div>
        </div>

      </div>

      <!--
        注入检测 —— 形制照启动页机架下面那个「系统自检」终端块（窗口栏三颗灯 + 提示符 + 树枝）。
        装的是<b>上次注入那一条记录</b>：时间 / 方式 / 目标 / 路径。

        ⚠️ 右上角那颗「快捷注入」是这一块唯一的动作 —— 摆在窗口栏里而不是正文下面，
        正文那几行是<b>读</b>的（等宽、对齐成一棵树），塞一颗按钮进去会把那份秩序打断。
        没有上次记录时整块显示一句「还没有注入过」，按钮压暗 —— 那时没有对象可注。

        ⚠️ 分隔点两侧的空格要写在 span 的文本节点<b>里面</b>：这几行是 white-space: pre，
        而 Vue 模板编译器默认的 condense 会把「纯空白且含换行」的文本节点整个删掉
        （启动页那块记过同一条，照抄时最容易丢的就是它）。
      -->
      <div class="term">
        <div class="term-bar">
          <span class="d" style="background:#ff5f57" />
          <span class="d" style="background:#febc2e" />
          <span class="d" style="background:#28c840" />
          <span class="lbl">{{ t('inject.pick.check') }}</span>
        </div>

        <div class="term-body">
          <span class="l"><span class="c">$</span> <span class="g">wpe64</span> --last-injection</span>

          <template v-if="hasLast">
            <span class="l">
              <span class="c">  ├─</span> {{ t('inject.pick.time') }}
              <span class="y">{{ lastInject.time || '—' }}</span>
              <span class="c">   ├─</span> {{ t('inject.pick.method') }}
              <span class="g">{{ lastMethodName }}</span>
              <span class="c">   ├─</span> {{ t('inject.target') }}
              <span class="a">{{ lastInject.target || '—' }}</span>
            </span>
            <span class="l pathline">
              <span class="c">  └─</span> {{ t('inject.col.path') }}
              <span class="y" :title="lastInject.path">{{ lastInject.path || '—' }}</span>
            </span>
          </template>

          <span v-else class="l"><span class="c">  └─</span> {{ t('inject.pick.lastNone') }} <span class="a">_</span></span>
        </div>

        <!--
          动作放在正文<b>下面</b>，不放窗口栏 —— 正文那几行是拿来读的（等宽、对齐成一棵树），
          窗口栏里塞一颗按钮会把「这是一块终端输出」这件事打断。
          右对齐是全项目页脚按钮的既定位置。
        -->
        <div class="term-act">
          <button class="qbtn" :disabled="!hasLast || busy" :title="t('inject.pick.quickHint')" @click="quickInject">
            <svg class="ico" viewBox="0 0 24 24"><path d="M13 2L4.5 13H11l-1 9 8.5-11H12z" /></svg>
            {{ t('inject.pick.quick') }}
          </button>
        </div>
      </div>
    </div>

    <!-- ══════════ ② 已附加：侧栏 + 11 页 ══════════ -->
    <div v-else class="workscr">
      <ProxySide :current="page" :groups="INJECT_GROUPS" mode="inject" @go="page = $event" />

      <!--
        封包页用 v-show 保活：切走再切回来若重新挂载，PacketList 的滚动位置与选中行都会重来一遍。
        日志页同理 —— 它自己维护一份 2000 条的环形缓冲，重新挂载就全没了。
      -->
      <InjectData
        v-show="page === 'packet'"
        ref="dataRef"
        :busy="busy"
        @toggle-hook="toggleHook"
        @clear="clearList"
        @open-setting="setting = $event"
      />

      <SystemLog v-show="page === 'log'" mode="inject" />

      <!-- 其余各页没有要保住的运行态（列表都在 stores/lists，工具页的状态在 stores/tools）-->
      <FilterList v-if="page === 'filter'" mode="inject" />
      <SendList v-if="page === 'send'" />
      <RobotList v-if="page === 'robot'" />
      <WareHouseList v-if="page === 'warehouse'" />
      <StatData v-if="page === 'stat'" mode="inject" />
      <TextCompare v-if="page === 'diff'" />
      <XorCalc v-if="page === 'xor'" />
      <Transcode v-if="page === 'transcode'" />
      <ExtractData v-if="page === 'extract'" />
    </div>

    <!-- 7 个设置弹窗。挂在外壳这一层，切到哪一页都还开着 -->
    <!--
      ⚠️ 这两个弹窗必须放在 v-if / v-else 那一对<b>外面</b>。
      夹在中间的话 v-else 就没有相邻的 v-if 了，Vue 模板编译器直接报
      「v-else has no adjacent v-if」—— 而 <b>vue-tsc 查不出这个</b>，
      只有真跑一次 Vite 才会炸（这一轮就是这么栽的）。
    -->
    <!--
      方式 01 的目标：进程表。

      ⚠️ <b>行内没有「操作」列</b> —— 一张两百多行的表，每行摆一颗「注入」按钮等于把
      整整一列宽度花在「重复两百遍的同一个词」上，而真正要读的路径列被挤窄。
      出口收成两个：<b>双击那一行</b>，或<b>选中之后按页脚的「注入」</b>（与 Enter 同一条路）。

      ⚠️ 因此它不再是 readonly 弹窗：页脚要有主按钮。没选中时按钮压暗（saveDisabled）——
      压暗在这儿是对的，因为「先选一行」这件事屏幕上已经写着了（工具条那句键盘提示）。
      左边那颗仍写「关闭」而不是「取消」：这一屏没有改动可撤销，只是挑一个目标。
    -->
    <SettingsModal
      :open="procOpen"
      :title="t('inject.pick.mProc')"
      subtitle="Controls/ProcessList"
      :width="1000"
      :busy="busy"
      :save-disabled="selectedPid === null"
      :save-text="t('inject.pick.attach')"
      :cancel-text="t('dlg.close')"
      @update:open="procOpen = $event"
      @save="selectedPid !== null && attachTo(selectedPid)"
    >
      <div class="procpick list-page">
        <div class="bar">
          <span class="sbox">
            <svg class="ico" viewBox="0 0 24 24"><circle cx="11" cy="11" r="6" /><path d="M15.5 15.5L21 21" /></svg>
            <input
              ref="searchRef"
              v-model="search"
              class="inp psearch"
              spellcheck="false"
              :placeholder="t('inject.pick.search')"
              @keydown="onSearchKey"
            />
          </span>

          <span class="cnt">
            <b>{{ shownProcs.length }}</b>
            <i v-if="shownProcs.length !== procs.length">/ {{ procs.length }}</i>
            {{ t('inject.pick.shown') }}
          </span>

          <span class="grow" />
          <span class="keys">{{ t('inject.pick.keys') }}</span>

          <button class="btn" :disabled="loadingProcs" @click="refreshProcs">
            {{ loadingProcs ? t('inject.pick.loading') : t('inject.pick.refresh') }}
          </button>
        </div>

        <!--
          ⚠️ 表头在滚动容器<b>里面</b>（sticky），不是外面 —— 外面的话滚动条一出现，
          行的可用宽度就比表头少 10px，后面几列全错开。走 .list-page 那套共用件即可。
        -->
        <div ref="bodyRef" class="body">
          <div class="head hp">
            <span class="so nm" :class="{ on: sorter.active('ProcessName') }" @click="sorter.toggle('ProcessName')">
              {{ t('inject.col.name') }}<span class="ar">{{ sorter.mark('ProcessName') }}</span>
            </span>
            <span class="so pid" :class="{ on: sorter.active('ProcessID') }" @click="sorter.toggle('ProcessID')">
              {{ t('inject.col.pid') }}<span class="ar">{{ sorter.mark('ProcessID') }}</span>
            </span>
            <span class="path">{{ t('inject.col.path') }}</span>
          </div>

          <div
            v-for="p in shownProcs"
            :key="p.ProcessID"
            class="row hp"
            :class="{ sel: selectedPid === p.ProcessID }"
            @click="selectedPid = p.ProcessID"
            @dblclick="busy || attachTo(p.ProcessID)"
          >
            <span class="nm">
              <img v-if="iconOf(p.ProcessPath)" class="pico" :src="iconOf(p.ProcessPath)" alt="" />
              <i v-else class="pico ph" />
              <b>{{ p.ProcessName }}</b>
            </span>
            <span class="pid">{{ p.ProcessID }}</span>
            <span class="path" :title="p.ProcessPath">{{ p.ProcessPath || '—' }}</span>
          </div>

          <div v-if="!shownProcs.length" class="empty">
            {{ procs.length ? t('inject.pick.none') : t('inject.pick.noneAll') }}
          </div>
        </div>
      </div>
    </SettingsModal>

    <!-- 方式 03 的目标：文件框挑完之后确认一下，顺便填启动参数 -->
    <SettingsModal
      :open="fileOpen"
      :title="t('inject.pick.mFile')"
      :busy="busy"
      :width="560"
      :save-text="t('inject.pick.launch')"
      @update:open="fileOpen = $event"
      @save="launchAndAttach"
    >
      <div class="filepick setf">
        <div class="fhead">
          <span class="well sm">
            <img v-if="iconOf(filePath)" class="fico" :src="iconOf(filePath)" alt="" />
            <svg v-else class="ico" viewBox="0 0 24 24"><path d="M6 2h8l4 4v16H6z" /><path d="M14 2v4h4" /></svg>
          </span>
          <span class="fname">
            <b>{{ fileName }}</b>
            <i :title="filePath">{{ filePath }}</i>
          </span>
          <button class="mini" :disabled="busy" @click="chooseFile">{{ t('inject.pick.choose') }}</button>
        </div>

        <div class="row">
          <span class="k">{{ t('inject.pick.args') }}</span>
          <input v-model="fileArgs" class="inp" spellcheck="false" :placeholder="t('inject.pick.argsPh')" @keydown.enter="launchAndAttach" />
        </div>

        <p class="hint">{{ t('inject.pick.wayLaunch') }}</p>
      </div>
    </SettingsModal>

    <LeachSetting :open="setting === 'leach'" mode="inject" @update:open="setting = $event ? 'leach' : null" />
    <HookSetting :open="setting === 'hook'" mode="inject" @update:open="setting = $event ? 'hook' : null" />
    <ListSetting :open="setting === 'list'" mode="inject" @update:open="setting = $event ? 'list' : null" />
    <HotkeySetting :open="setting === 'hotkey'" @update:open="setting = $event ? 'hotkey' : null" />
    <BackupSetting :open="setting === 'backup'" @update:open="setting = $event ? 'backup' : null" />
    <RemoteSetting :open="setting === 'remote'" @update:open="setting = $event ? 'remote' : null" />
    <SystemSetting :open="setting === 'system'" @update:open="setting = $event ? 'system' : null" />
  </div>
</template>

<style scoped>
/*
  ⚠️ 要 flex: 1 + min-width: 0。
  这一屏在 App.vue 里是个 flex 项，不写的话它按内容定宽 —— 表只有几百像素，
  右边一大片空着（探针页第一版就是这样）。min-width: 0 是让里面的
  「路径」那一列能被压缩，否则长路径会把整张表撑出容器。
*/
.inject {
  display: flex;
  flex-direction: column;
  flex: 1;
  min-width: 0;
  height: 100%;
  min-height: 0;
}

/* ── 选目标 ─────────────────────────────── */

/*
  ⚠️ 这一屏是「三选一」，不是一张表 —— 所以它<b>不挂 .list-page</b>（那套是给列表页的），
  进程表挪进了弹窗，弹窗里那一层才挂。
*/
/*
  ⚠️ 这一屏与启动页同一条口径：<b>一眼看完、选一种</b>，所以竖直居中而不是顶着上沿排。
  顶着排的话大屏上就是「上半截挤满、下半屏空一大片」（用户提的正是这个）。

  ⚠️ justify-content 必须是 `safe center`：普通的 center 在装不下时会把上半截推出滚动区，
  那一截既滚不到也点不着（启动页那节记过同一条）。

  所有竖直呼吸量都提成令牌，下面两档 @media (max-height) 整组重定义 —— 默认窗口
  ClientSize 1280×800 是<b>设备像素</b>，125% 缩放时页面只有 640 CSS 高、150% 只有 533，
  照 800 那一档排版，缩放一开就出滚动条。
*/
.pickscr {
  /* 共用件 .scrn 的三个令牌 —— 注入模式跟启动页一样走绿 */
  --hd: var(--green);
  --hd-rgb: var(--green-rgb);
  --ttl-size: clamp(26px, 2.6vw, 34px);
  --hd-gap: 16px;   /* eyebrow → 标题（与启动页同档）*/
  --hd-sub: 14px;   /* 标题 → 副标题（同上）*/

  --pk-py: 26px;    /* 整屏上下内边距 */
  --pk-gap: 24px;   /* 标题块 → 机架 */
  --pk-slot: 12px;  /* 机架内边距 ＝ 槽缝（同一个令牌，两者必须同值）*/
  --cd-h: 208px;    /* 卡片最矮多高 —— 撑开之后说明与读数条才分得开 */
  --cd-py: 16px;    /* 卡片上下内边距 */
  --cd-ry: 14px;    /* 卡内三处竖直间距 */
  --term-py: 12px;  /* 注入检测那块终端的正文内边距 */
  --term-lh: 1.7;   /* 终端正文的行高 —— 三行字，矮窗口下这一项很值钱 */

  display: flex;
  flex-direction: column;
  justify-content: safe center;
  min-height: 0;
  height: 100%;
  overflow: auto;
  padding: var(--pk-py) 28px;
}

@media (max-height: 690px) {
  .pickscr {
    --hd-gap: 12px; --hd-sub: 11px; --ttl-size: clamp(24px, 2.4vw, 30px);
    --pk-py: 20px; --pk-gap: 18px; --pk-slot: 10px; --cd-h: 190px; --cd-py: 13px; --cd-ry: 11px;
    --term-py: 9px; --term-lh: 1.6;
  }
}

@media (max-height: 590px) {
  .pickscr {
    /*
      ⚠️ 这一档（150% 缩放的默认窗口，页面只有 533 CSS 高、扣掉标题栏与状态栏剩 457）
      要装下「标题块 + 三张卡 + 注入检测」三块，俄语 / 英语 / 越南语的 lede 还会折到三行 ——
      所以是<b>整组</b>往下收，不是收一两项。加进来的注入检测块本身就要 100px 上下。
    */
    --hd-gap: 6px; --hd-sub: 7px; --ttl-size: clamp(19px, 2vw, 24px);
    --pk-py: 10px; --pk-gap: 10px; --pk-slot: 8px; --cd-h: 0px; --cd-py: 10px; --cd-ry: 8px;
    --term-py: 4px; --term-lh: 1.45;
  }
}

.ptitle { flex: none; margin-bottom: var(--pk-gap); }

/*
  eyebrow 与标题的基样式在 style.css 的共用件 `.scrn` 里（启动页 / 多开设置同一套），
  这一屏只补一条间距。

  ⚠️ 别在这儿写 font-family / 字号 / 颜色：`.ttl[data-v-x]` 与 `.scrn .ttl` 同为 (0,2,0)，
  平局时后加载的 style.css 赢 —— 写了也不生效。
  ⚠️ 也别顺手加 `text-transform: uppercase` / 负字距：这一句是<b>中文</b>，
  uppercase 是空操作，负字距会把方块字挤在一起（那两条只对拉丁标题有意义）。

  ⚠️ 三行之间的间距由共用件的 `--hd-gap` / `--hd-sub` 出（副标题自己的 margin-top），
  别在这儿给标题补 margin-bottom —— 那样两处各管一半，改一处就对不上了。
*/

/* ── 注入检测（照启动页机架下面那个自检终端）───── */

/*
  自检终端块的外观在 `style.css` 的 `.scrn .term`（与启动页共用一份）。
  这里只留本屏独有的：不参与主轴伸缩、与上面三张卡的间距。
*/
.term {
  flex: none;
  margin-top: var(--pk-gap);
}

/* 动作条：贴着正文下沿，与正文同一套左右内边距 */
.term-act {
  display: flex;
  justify-content: flex-end;
  padding: 0 16px var(--term-py);
}

/*
  「快捷注入」—— 形制与全项目那颗 `.btn` <b>逐条相同</b>（灰边 → 悬停转青边青字）。

  ⚠️ 只能自己抄一份：`style.css` 里的 `.btn` 只在 `.list-page` 下有定义，这一屏匹配不到
  （机器人编辑重做时就是这么把按钮弄回浏览器原生外观的，见 CLAUDE.md 那条）。
  全项目现在有四处这样的手抄（三个大编辑器 + 这里），要并得起个新类名，别直接提 `.btn`。

  ⚠️ 上下内边距<b>对称</b>：`line-height: 1` 已经把 Share Tech Mono 的字形偏上治好了，
  再补「上 +1 下 −1」是补第二遍（2026-09-09 全项目撤掉过 34 处）。
*/
.qbtn {
  flex: none;
  display: inline-flex;
  align-items: center;
  gap: 7px;
  padding: 8px 13px;
  background: transparent;
  border: 1px solid var(--border);
  color: var(--gray);
  font-family: var(--share);
  font-size: var(--btn-size);
  line-height: 1;
  letter-spacing: .12em;
  text-transform: uppercase;
  white-space: nowrap;
  cursor: pointer;
  transition: .15s;
}

.qbtn:hover:not(:disabled) { border-color: var(--cyan); color: var(--cyan); }
.qbtn:focus-visible { outline: 1px solid var(--cyan); outline-offset: -2px; }
.qbtn:disabled { opacity: .35; cursor: default; }
.qbtn .ico { width: 13px; height: 13px; flex: none; fill: currentColor; stroke: none; }

/* 路径那一行：长路径要能截断，不然整块被撑宽 */
.term-body .pathline { display: flex; gap: 6px; align-items: baseline; }
.term-body .pathline .y { min-width: 0; overflow: hidden; text-overflow: ellipsis; }

/* ── 机架 + 三个槽位（与启动页同一套构件）───── */

/*
  ⚠️ 槽缝与机架内边距取<b>同一个值</b>，卡片到框的距离才处处一样。
  ⚠️ 槽缝不给背景，透出页面底色 —— 别填 --sink：浅色下它与 --card 都是 #ffffff，
  缝隙会整个消失，外框那条线就成了一条没来由的双线（启动页那节记过）。
*/
.rack {
  flex: none;
  padding: var(--pk-slot);
  border: 1px solid var(--border);
}

.cards { display: grid; grid-template-columns: repeat(3, 1fr); gap: var(--pk-slot); }

/*
  ⚠️ flex 列 + 说明区弹性 —— 三张卡的读数条因此永远落在<b>同一条线</b>上，
  与说明折了几行无关（七种语言下行数是不一样的）。
  靠 `.cd p` 的 min-height 顶着的老写法只在中文那一档对得齐。
*/
.cd {
  position: relative;
  overflow: hidden;
  min-width: 0;
  min-height: var(--cd-h);
  display: flex;
  flex-direction: column;
  background: var(--card);
  border: 1px solid var(--border);
  padding: var(--cd-py) 16px;
  cursor: pointer;
  transition: background .15s, border-color .15s;
}

.cd:hover { background: var(--panel); border-color: var(--green); }
.cd.cy:hover { border-color: var(--cyan); }
.cd.am:hover { border-color: var(--amber); }

/* 焦点环画在内侧：卡片外面只隔 10px 就是机架的框，正偏移会撞上去 */
.cd:focus-visible { outline-offset: -2px; }
.cd.cy:focus-visible { outline-color: var(--cyan); }
.cd.am:focus-visible { outline-color: var(--amber); }

/* 顶沿的机架色轨：左三分之一实心、右侧衰减；悬停 / 聚焦时整条点亮 */
.rail {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  height: 3px;
  background: linear-gradient(90deg, var(--green) 0 34%, rgb(var(--green-rgb) / 14%) 34%);
  transition: background .15s;
}

.cd.cy .rail { background: linear-gradient(90deg, var(--cyan) 0 34%, rgb(var(--cyan-rgb) / 14%) 34%); }
.cd.am .rail { background: linear-gradient(90deg, var(--amber) 0 34%, rgb(var(--amber-rgb) / 14%) 34%); }
.cd:hover .rail, .cd:focus-visible .rail { background: var(--green); }
.cd.cy:hover .rail, .cd.cy:focus-visible .rail { background: var(--cyan); }
.cd.am:hover .rail, .cd.am:focus-visible .rail { background: var(--amber); }

/* 「正在选窗体」那一档：整张卡走琥珀，状态灯跟着闪 */
.cd.cy.live { border-color: var(--amber); }
.cd.cy.live .rail { background: var(--amber); }
.cd.cy.live .rdy { color: var(--amber); }
.cd.cy.live .dot { animation: blink 1s steps(1) infinite; }

@keyframes blink { 50% { opacity: .25; } }

/*
  机位号水印。工业面板上的大号丝印数字 —— 几乎不占视觉预算，却把版面撑开了。
  竖直居中而不是贴顶：贴顶会与右上角那枚状态灯叠在一起。
  透明度写成通道值，两套皮肤各自算。
*/
.wm {
  position: absolute;
  right: 12px;
  top: 50%;
  transform: translateY(-50%);
  font-family: var(--orbit);
  font-weight: 900;
  font-size: 62px;
  line-height: 1;
  letter-spacing: -.04em;
  color: rgb(var(--green-rgb) / 8%);
  pointer-events: none;
  user-select: none;
}

.cd.cy .wm { color: rgb(var(--cyan-rgb) / 8%); }
.cd.am .wm { color: rgb(var(--amber-rgb) / 8%); }

/* 下面几层都要压在水印上面，所以各自 position: relative */
.hd {
  position: relative;
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 10px;
  margin-bottom: var(--cd-ry);
}

/* top .4px：--fs-caption 提到 10.5px（2026-09-13）后重量，−0.61 → −0.21（恢复 9.5px 时的位置） */
.cd .num { position: relative; top: .4px; font-family: var(--share); font-size: var(--fs-caption); letter-spacing: .18em; color: var(--muted); }

/*
  ⚠️⚠️ **这个类不能叫 `.st`。**

  Vue 的 scoped 会把<b>父组件的 scope id 加到子组件的根元素上</b>（好让父组件能给
  子组件根节点设样式）。而 `StatData` 的根元素是 `<div class="page list-page st">` ——
  于是这条裸 `.st` 当场命中它，`align-items: center` 漏进去，
  统计页的工具条 / 两张卡 / 表格各自按内容宽度<b>居中飘着</b>（用户截图）。
  `display: flex` / `font-size: 10px` / `letter-spacing` / `color` 也一起漏，还会继承给子元素。

  ⚠️ **代理模式没这个毛病**是因为 `ProxyView` 的 scoped 里只有一个裸类、没撞上 ——
  纯属运气。这是 `.sel` / `.mini` / `.hint` / `.runbar` 之后同名不同物的第五次。

  查法：**父组件 scoped 里的裸类 ∩ 子组件根元素的 class**，要为空。
*/
.rdy {
  display: flex;
  align-items: center;
  gap: 6px;
  font-family: var(--share);
  font-size: var(--fs-caption);
  letter-spacing: .18em;
  color: var(--green);
  white-space: nowrap;
}

.cd.cy .rdy { color: var(--cyan); }
.cd.am .rdy { color: var(--amber); }

.dot { width: 6px; height: 6px; border-radius: 50%; background: currentColor; box-shadow: 0 0 6px currentColor; }

.cd .t {
  position: relative;
  display: flex;
  align-items: center;
  gap: 11px;
  margin-bottom: var(--cd-ry);
}

/* 仪表窗：图标从「贴在标题左边的装饰」变成一个装在面板上的器件 */
.well {
  position: relative;
  flex: none;
  width: 38px;
  height: 38px;
  display: flex;
  align-items: center;
  justify-content: center;
  border: 1px solid var(--border);
  background: rgb(var(--inset-rgb) / 30%);
  color: var(--green);
}

.cd.cy .well { color: var(--cyan); }
.cd.am .well { color: var(--amber); }
.well.sm { width: 34px; height: 34px; color: var(--amber); }

/* 对角两枚角标 —— 与全项目那套四角标记同一种语言；四枚太吵，两枚就够点题 */
.well::before,
.well::after {
  content: "";
  position: absolute;
  width: 5px;
  height: 5px;
  border: 0 solid currentColor;
}

.well::before { left: -1px; top: -1px; border-left-width: 1px; border-top-width: 1px; }
.well::after { right: -1px; bottom: -1px; border-right-width: 1px; border-bottom-width: 1px; }

.well .ico { width: 20px; height: 20px; fill: none; stroke: currentColor; stroke-width: 1.6; }

/*
  ⚠️ 双行标题要与仪表窗上下齐平，两个数是量出来的 —— 与启动页那两张卡同一份，
  改之前先读 StartView.vue 里那段（.zh 的 margin-top 管墨迹跨度、.tx 的 padding-bottom 管整块位置）。
*/
.tx { min-width: 0; padding-bottom: 2px; }

.en {
  display: block;
  font-family: var(--orbit);
  font-weight: 400;
  font-size: var(--fs-title);
  text-transform: uppercase;
  letter-spacing: .07em;
  color: var(--green);
}

.cd.cy .en { color: var(--cyan); }
.cd.am .en { color: var(--amber); }

/* <i> 只是拿来当行内容器，斜体要关掉 */
.zh { display: block; margin-top: 1.1px; font-size: var(--fs-lead); font-style: normal; color: var(--gray); }

.cd p {
  position: relative;
  flex: 1 1 auto;
  margin: 0 0 var(--cd-ry);
  min-height: 36px;
  font-size: var(--fs-body);
  line-height: 1.55;
  color: var(--muted);
}

/* 读数条：与启动页那两张卡同构，各显示一条本方式的实测数据 */
.foot {
  position: relative;
  display: flex;
  align-items: center;
  gap: 10px;
}

.foot .last {
  flex: 1;
  min-width: 0;
  display: flex;
  align-items: baseline;
  gap: 8px;
  padding: 4px 9px 5px;   /* 原 5/4 在字体度量覆写之后偏低 1.3px（2026-09-13 字体度量覆写后按 100% 缩放实测重调） */
  border: 1px solid rgb(var(--border-rgb) / 70%);
  background: rgb(var(--inset-rgb) / 30%);
}

.foot .k {
  flex: none;
  font-family: var(--share);
  font-size: var(--label-size);
  line-height: 1;
  letter-spacing: .12em;
  text-transform: uppercase;
  color: var(--dim3);
}

.foot .last b {
  min-width: 0;
  font-family: var(--mono);
  font-size: var(--fs-body);
  font-weight: 400;
  color: var(--gray);
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.foot .ar { flex: none; color: var(--green); font-size: 13px; }
.cd.cy .foot .ar { color: var(--cyan); }
.cd.am .foot .ar { color: var(--amber); }

/*
  ⚠️ 三张卡一直并排，别在窄窗口叠成一列 —— 叠起来这一屏就要滚，
  而「一眼看完、选一种」正是它存在的理由（与启动页那条口径一样）。
  853 宽（150% 缩放的默认窗口）下每张 264px，说明折两行也放得下。
*/

/* ── 弹窗 ① 进程表 ───────────────────────── */

/*
  ⚠️ 高度是<b>定值与视口二选一</b>，不能只写 420px。

  SettingsModal 的 .dlg 是 max-height: calc(100vh - 120px)，头尾另占 117px、.bd 上下再 8px ——
  也就是说留给这块的只有 100vh - 245。150% 缩放（853×533，扣掉标题栏与状态栏后 458 CSS 高）下
  写死 420 就会让 .bd 自己长出一条滚动条，<b>搜索框跟着被滚走</b>，而表里还嵌着自己那条滚动条 ——
  两层滚动，且最该常驻的那个控件先消失。

  248 比实测的 245 多留 3px 余量。改 SettingsModal 的头尾高度要回来重算这个数。
*/
.procpick { display: flex; flex-direction: column; gap: 8px; height: min(420px, calc(100vh - 248px)); }

.sbox { position: relative; flex: 0 1 300px; min-width: 160px; display: flex; }

.sbox .ico {
  position: absolute;
  left: 9px;
  top: 50%;
  width: 14px;
  height: 14px;
  margin-top: -7px;
  fill: none;
  stroke: var(--dim3);
  stroke-width: 1.7;
  pointer-events: none;
}

/* .inp 刻意不带 flex / min-width，见 CLAUDE.md「输入框已经收进 style.css」 */
.procpick .inp.psearch { flex: 1; min-width: 0; padding-left: 29px; }

.cnt { flex: none; font-family: var(--mono); font-size: var(--fs-body); color: var(--muted); }
.cnt b { color: var(--gray); }
.cnt i { font-style: normal; color: var(--dim3); }

.keys { flex: none; font-size: var(--fs-small); color: var(--dim3); }
@media (max-width: 1120px) { .keys { display: none; } }

/*
  ⚠️ 三列：进程名与路径是变长的，给 1fr / 2fr；只有 PID 定宽。
  「操作」那一列<b>去掉了</b>（原来 128px，为的是装下俄语的「Внедрить」）——
  出口改成双击行或页脚那颗「注入」，省下的宽度全给了路径列。
*/
.head.hp,
.row.hp { grid-template-columns: minmax(150px, 1fr) 92px minmax(180px, 2fr); column-gap: 10px; }

/* 除变长标识（进程名 / 路径）外全部居中，与 WPC 配置、远程管理台账号表同一条口径 */
.head.hp > span.pid, .row.hp > span.pid { text-align: center; }

/* 整行可点：单击选中、双击注入 —— 所以是 pointer，不是原来那个 default */
.row.hp { cursor: pointer; }

.row .nm { display: flex; align-items: center; gap: 8px; min-width: 0; }
.row .nm b { font-weight: 400; color: var(--gray); overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }

/*
  图标一律 16×16 的方框 + object-fit: contain —— 与国旗那处同一个理由：
  写死宽高会把非方形的图压扁，而定宽方框能让文字起点不随图变宽左右跳。
*/
.pico { flex: none; width: 16px; height: 16px; object-fit: contain; }
/*
  取不到图标（系统进程、没权限读的、WindowsApps 下的）时只留一个<b>空的</b> 16px 位子，什么都不画 ——
  名字照样对齐成一列。原来这里画了一圈细边框 + 淡底，用户读成了「一个没勾上的选择框」（2026-09-11 去掉）。
*/
.pico.ph { display: block; }

.row .pid { font-family: var(--mono); color: var(--muted); }
.row .path { color: var(--dim3); font-family: var(--mono); font-size: var(--fs-small); }

/* ── 弹窗 ② 可执行文件 ───────────────────── */

.filepick { padding: 4px 0 2px; }

.fhead {
  display: flex;
  align-items: center;
  gap: 12px;
  margin: 0 20px 12px;
  padding-bottom: 12px;
  border-bottom: 1px solid rgb(var(--border-rgb) / 60%);
}

.fico { width: 20px; height: 20px; object-fit: contain; }

.fname { flex: 1; min-width: 0; }
.fname b { display: block; font-weight: 400; font-size: var(--fs-lead); color: var(--gray); overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }

.fname i {
  display: block;
  margin-top: 4px;
  font-style: normal;
  font-family: var(--mono);
  font-size: var(--fs-small);
  color: var(--dim3);
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.filepick .inp { flex: 1; min-width: 0; font-family: var(--mono); }

/* ── 工作屏：侧栏 + 内容，与代理模式同一套栅格 ───────────── */
.workscr {
  position: relative;
  z-index: 10;
  flex: 1;
  min-height: 0;
  display: grid;
  grid-template-columns: var(--side-w, 196px) 1fr;
}
</style>
