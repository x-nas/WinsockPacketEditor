<script setup lang="ts">
/*
  「封包列表」页 —— 对应 WinForms 的 Controls/PacketList，注入模式的主屏。

  与代理模式的 ProxyData 是同一套结构（状态条 + 统计 + 表 + 下半部），
  数据源换成 FeedList.Packet 那一路（PacketInfo，另一套 Id 序列）。

  【数据来自另一个进程】钩子与滤镜引擎留在目标里，封包经命名管道过来，
  由 ShellLink.Ingest 还原成 PacketInfo 进 cqPacketInfo；进队之后的下游
  （FlushToFeed → PacketRow → 这里的表）与代理模式完全共用。
*/
import { computed, onBeforeUnmount, onMounted, ref, watch } from 'vue'
import { call } from '../../bridge'
import { FeedList, type PacketListRow, type PacketRow, type Prefs, type SendRow, type WareHouseRow } from '../../bridge/types'
import { injectFeed } from '../../stores/packets'
import { gotoPage, listSetting } from '../../stores/runtime'
import { useList } from '../../stores/lists'
import { useRowPick } from '../../usePick'
import { pushToast } from '../../stores/toast'
import { textA, textB } from '../../stores/tools'
import { status } from '../../stores/inject'
import { t } from '../../i18n'
import PacketList from '../PacketList.vue'
import HexPanel from '../HexPanel.vue'
import ContextMenu from '../ContextMenu.vue'
import { ICON, type MenuItem } from '../menu'
import PacketEdit from '../proxy/PacketEdit.vue'
import PacketModification from '../proxy/PacketModification.vue'
import QuickPanel from '../proxy/QuickPanel.vue'
//图例上点色块开的配色弹窗 —— 与代理数据页同一个组件（滤镜配色是两种模式共用的）
import ActionColor from '../proxy/ActionColor.vue'
import InjectBar from './InjectBar.vue'
import type { SettingKey } from '../proxy/settings'

const emit = defineEmits<{
  (e: 'toggleHook'): void
  (e: 'clear'): void
  (e: 'openSetting', key: SettingKey): void
}>()

const props = defineProps<{ busy: boolean }>()

const prefs = ref<Prefs | null>(null)

/*
  正在改配色的那一组动作（工具条上那排色块点开的），null = 没开。
  取当前值直接从 prefs 拿 —— 图例画的就是它，不必再往返一次。
*/
const colorEdit = ref<string | null>(null)

async function reloadPrefs(): Promise<void> {
  try {
    //改完立刻重取：图例与列表的行配色都吃这份 prefs，重取就地生效
    prefs.value = await call<Prefs>('getPrefs')
  } catch (e) {
    console.error('[inject] 重取界面偏好失败', e)
  }
}

/*
  行底色图例。四组与 PacketList 的 colorOf() 一一对应 ——
  那里按 FilterAction 取 prefs.filter 的某一组打行内样式，这里就把同一组摊出来。

  ⚠️ 与代理数据页<b>逐条相同</b>：滤镜是两种模式共用的（注入模式下它跑在目标进程里），
  同一条规则命中之后行的底色也一样，图例没有理由只在一边有。

  <b>没有「不显示」那一档</b>：NoModify_NoDisplay 的行根本不进列表，
  给一个永远看不到的颜色配图例只会让人去找它。
  colorOf 的 default 分支（未命中滤镜）也不列 —— 那是「没有底色」，不是一种底色。
*/
const ROW_LEGEND = computed(() => {
  const f = prefs.value?.filter
  if (!f) return []

  return [
    { key: 'replace', label: 'proxy.act.replace' as const, pair: f.replace },
    { key: 'change', label: 'proxy.act.change' as const, pair: f.change },
    { key: 'intercept', label: 'proxy.act.intercept' as const, pair: f.intercept },
    { key: 'display', label: 'proxy.act.display' as const, pair: f.display },
  ]
})
const rows = injectFeed.rows

const selected = ref<PacketRow | null>(null)
const selectedId = ref<number | null>(null)

const listRef = ref<InstanceType<typeof PacketList> | null>(null)
const follow = ref(true)

/*
  ── 统计条 ────────────────────────────────────────────

  对应 WinForms 的 PacketList 信息栏：10 个 WinSock 计数 + Total + Queue
  + 滤镜执行 / 已过滤 + 收发字节。数据走 getInjectStats（<b>不是 getStats</b> ——
  那一份全是代理口径，两种模式的计数器不是同一批字段）。

  与代理数据页同样 500ms 轮询，不占推送通道。
*/
interface InjectStats {
  queue: number; list: number; total: number
  send: number; sendTo: number; recv: number; recvFrom: number
  wsaSend: number; wsaSendTo: number; wsaRecv: number; wsaRecvFrom: number
  filterExecute: number; filterPacket: number
  totalSend: number; totalRecv: number
}

const stats = ref<InjectStats | null>(null)
let statsTimer = 0

/*
  「自动清理」在工具条上<b>只读显示</b>，点它跳到列表设置去改 ——
  与代理数据页同一条口径：两处都能改反而要同步。
*/
/*
  自动清理 —— 2026-09-07 从「列表设置」弹窗搬到这条工具条上。

  「设置摆在哪儿，就代表它管哪张表」：日志那份一直在日志页的工具条上，
  而这一份原先藏在弹窗里，两个长得一模一样的「自动清理」谁都会以为是同一个。
  在这儿它紧挨着它真正会裁的那张表。

  ⚠️ <b>封包列表与代理列表共用这一份配置</b>（PacketConfig.List.AutoClear，
  InjectMode 表），所以桥方法 saveListAutoClear <b>不带 mode</b> ——
  与列显隐那一对相反，别顺手给它加。

  ⚠️ 初值走 getListSetting（挂载时本来就调了），<b>不再从 500ms 的统计轮询里拿</b>：
  轮询会在用户正在输入时把条数框冲掉，而且那两个字段本来就是白搭在热路径上的。

  开关与条数各自单发（桥那边是「字段出现才改」）：勾选框点一下就存，
  条数框失焦或按 Enter 才存 —— 一起发的话，点开关会把正在编辑的半截数字也写进去。
*/
const autoClear = ref(true)
const clearAt = ref(5000)
const keepInput = ref('5000')
const keepBad = ref(false)

async function toggleAutoClear(): Promise<void> {
  autoClear.value = !autoClear.value
  try { await call('saveListAutoClear', { autoClear: autoClear.value }) } catch { /* 存不上不影响本次会话 */ }
}

/** 条数：范围校验在 C# 侧（100~500000），这里只负责别把空串发过去。 */
async function commitKeep(): Promise<void> {
  const n = Number(keepInput.value)

  if (!Number.isFinite(n) || n < 100 || n > 500000) {
    keepBad.value = true
    return
  }

  keepBad.value = false

  if (n === clearAt.value) return

  try {
    const r = await call<{ ok: boolean }>('saveListAutoClear', { autoClearValue: n })
    if (r?.ok) clearAt.value = n
    else keepBad.value = true
  } catch {
    keepBad.value = true
  }
}

function n(v: number | undefined): string {
  return (v ?? 0).toLocaleString()
}

/*
  字节数按量级给单位，只给一位小数 —— 与代理数据页那两格同一个理由：
  C# 那份 GetDisplayBytes 的详版（「62.4 KB (63,915 Bytes)」）塞不进七分之一屏宽的格子。
*/
function bytes(v: number | undefined): string {
  let x = v ?? 0
  if (x < 1024) return x + ' B'

  const u = ['KB', 'MB', 'GB', 'TB']
  let i = -1
  while (x >= 1024 && i < u.length - 1) { x /= 1024; i++ }
  return x.toFixed(1) + ' ' + u[i]
}

/*
  14 格 = 7 列 × 2 行。tone 决定数字的颜色：
    g 绿（常规计数）· c 青（合计 / 流量）· a 琥珀（值得注意的：滤镜、积压、速率）

  ⚠️ 这八个 WinSock 计数<b>各自覆盖 WinSock 1.1 与 2.0 两套入口</b>
  （CountPacketInfo 把 WS1_Send 与 WS2_Send 都记进 Send_CNT），
  与过滤设置里那八个勾选框是同一套口径 —— 不要以为少了一半。
*/
const cells = computed(() => {
  const s = stats.value
  const st = injectFeed.stat.value

  /*
    ⚠️ **顺序照代理数据页的口径排**，两屏切过去眼睛不用重新找位置：

    | 列 | 行 1 | 行 2 | 与代理的对应 |
    |---|---|---|---|
    | 1 | Total（封包总数）| Filter（滤镜执行）| 代理第 1 列也是 Total 那个「总计」|
    | 2~5 | **发**的四类 | **收**的四类 | 代理第 2~5 列是 TCP/UDP 与它们的请求·响应 |
    | 6 | Queue（待入列）| Filtered（已过滤）| 代理也是 Filtered / Queue 挨在 Bytes 前面 |
    | 7 | **Bytes** | **Rate** | 代理正是 Bytes / Speed 各落在两行末尾（那边注释写着「视觉上成对」）|

    第 2~5 列还多一层：上下<b>一一对应</b>（Send↔Recv · SendTo↔RecvFrom ·
    WSASend↔WSARecv · WSASendTo↔WSARecvFrom），竖着看就是同一个 API 的收发两头。
    ⚠️ 改这张表的顺序之前先想清楚破坏的是哪一条 —— 这四对是有意竖排的。
  */
  return [
    { k: 'Total', z: t('inject.st.total'), v: n(s?.total), tone: 'g' },
    { k: 'Send', z: t('pt.ws2Send'), v: n(s?.send), tone: 'g' },
    { k: 'SendTo', z: t('pt.ws2SendTo'), v: n(s?.sendTo), tone: 'g' },
    { k: 'WSASend', z: t('pt.wsaSend'), v: n(s?.wsaSend), tone: 'g' },
    { k: 'WSASendTo', z: t('pt.wsaSendTo'), v: n(s?.wsaSendTo), tone: 'g' },
    { k: 'Queue', z: t('inject.st.queue'), v: n(s?.queue), tone: (s?.queue || 0) > 5000 ? 'a' : 'g' },
    /*
      总流量这一格是双值的：大字给合计，小字给 ↑发 / ↓收 的拆分 —— 与代理那格同一种排法。
    */
    {
      k: 'Bytes',
      z: '↑ ' + bytes(s?.totalSend) + ' · ↓ ' + bytes(s?.totalRecv),
      v: bytes((s?.totalSend || 0) + (s?.totalRecv || 0)),
      tone: 'c',
    },

    //目标报上来的滤镜执行次数（引擎在那边跑）
    { k: 'Filter', z: t('inject.st.filterExec'), v: n(s?.filterExecute), tone: 'a' },
    { k: 'Recv', z: t('pt.ws2Recv'), v: n(s?.recv), tone: 'g' },
    { k: 'RecvFrom', z: t('pt.ws2RecvFrom'), v: n(s?.recvFrom), tone: 'g' },
    { k: 'WSARecv', z: t('pt.wsaRecv'), v: n(s?.wsaRecv), tone: 'g' },
    { k: 'WSARecvFrom', z: t('pt.wsaRecvFrom'), v: n(s?.wsaRecvFrom), tone: 'g' },
    //被「过滤设置」挡掉、没进列表的条数（外壳这边 FlushToFeed 数的）
    { k: 'Filtered', z: t('inject.st.filtered'), v: n(s?.filterPacket), tone: 'a' },
    /*
      速率是前端自己按推送量算的（每 500ms 结算一次），与目标无关。

      副标题<b>被丢包数抢占</b>：环满时目标丢的是最旧的包，无声丢包比阻塞更糟，
      所以只要不是 0 就顶到最显眼的地方；为 0 时才说自己是什么。
    */
    {
      k: 'Rate',
      z: status.value.dropped > 0
        ? t('inject.st.dropped') + ' ' + n(status.value.dropped)
        : t('inject.st.rate'),
      v: st.rate + '/s',
      tone: 'a',
    },
  ]

})

/*
  ⚠️ 必须传 autoPrune: false。共用实现默认挂一个 watch(rows) 剔除已消失的 Id，
  而这一屏的 rows 每帧都 triggerRef —— 挂上去就是每秒跑 60 次。
  这里改成只在列表被清空时 clear() 一次（下面那个 watch）。
*/
const pick = useRowPick<PacketRow, number>(rows, (r) => r.Id, { autoPrune: false })
const picked = pick.picked

//整表清空（用户点清空 / 切库）走这条；自动清理是环形裁剪，走下面的 onTrimmed
watch(() => rows.value.length, (n) => { if (n === 0) { pick.clear(); pickTrimmed.value = false } })

/*
  选中的封包被自动清理（环形，只留最近 N 条）裁掉了 —— 从选中集里剔掉，并记下是被裁掉的，
  右键动作撞上空选中集时说清楚原因。理由与代理数据页的同名那段一样（真机冒烟报过）；
  详情面板刻意不清，用户可能正读着。
*/
const pickTrimmed = ref(false)

const offTrimmed = injectFeed.onTrimmed((removed) => {
  if (pick.trimHead(removed) > 0 && picked.value.size === 0) pickTrimmed.value = true
})

function needPickToast(): void {
  pushToast('warning', pickTrimmed.value ? t('pm.pickTrimmed') : t('lst.needPick'))
}

onMounted(async () => {
  try {
    prefs.value = await call<Prefs>('getPrefs')
  } catch (e) {
    console.error('[inject] 取界面偏好失败', e)
  }

  //列显隐要在第一帧之前拿到，否则列表先按「全显示」画一遍再跳
  //注入模式读注入那一套列显隐（C# 侧是 PacketConfig.List.IsShow_*）
  try {
    const s = await call<any>('getListSetting', { mode: 'inject' })
    if (!listSetting.value) listSetting.value = s

    //自动清理的初值搭同一趟车。它<b>不分模式</b>，两种模式是同一份配置
    autoClear.value = !!s?.autoClear
    clearAt.value = Number(s?.autoClearValue) || 5000
    keepInput.value = String(clearAt.value)
  } catch { /* 桥没接上 */ }

  statsTimer = window.setInterval(async () => {
    try { stats.value = await call<InjectStats>('getInjectStats') } catch { /* 窗口关闭中 */ }
  }, 500)
})

onBeforeUnmount(() => {
  //推送的订阅在 InjectView 上，这一屏切走时不该断掉；这个轮询是自己的，要收
  window.clearInterval(statsTimer)
  offTrimmed()
})

function onSelect(anyRow: PacketListRow, ev: MouseEvent, index: number): void {
  //mode="inject" 时表里的行一定是 PacketRow
  const row = anyRow as PacketRow
  pick.onRowClick(row, ev, index)
  pickTrimmed.value = false
  selected.value = row
  selectedId.value = row.Id

  /*
    手动点行就把查找的高亮丢掉。不丢的话那个偏移会原样套到新点开的这一条上 ——
    面板只认「圈第 N 到第 M 字节」，圈出来的会是一段毫不相干的字节，而且看着像是查到的。
  */
  searchHit.value = null

  /*
    ⚠️ 还要告诉 C#「现在选中的是哪一条」。

    WinForms 侧是表格的 SelectedIndexChanged 顺手做的（PacketList.cs:969），
    外壳没有那个控件 —— 不设的话两条机器人指令会静默失效：
    「发送 → 封包列表」（发的就是这一条）与「设置系统套接字 → 封包列表」。
    C# 那边设完会把 Runtime 快照推给目标（执行器在那边跑）。
  */
  void call('setSelectedPacket', { id: row.Id }).catch(() => {})
}

/* ── 查找封包（对应 WinForms 的 Controls/SearchPacket）───────── */

/*
  与代理数据页那一份逐句对应，只差调 searchPacketList 而不是 searchProxyList。
  （包括「只有一个按钮」那条：「查找下一个」走到末尾自己回头再来一圈，
  所以不摆「从头查找」—— 曾经摆过，就是这个理由删掉的。）

  【游标留在前端】C# 侧不存 Search_Index：搜索是「从第 N 行往后找第一条」这一件事，
  游标是谁在翻页谁的状态。
  【命中之后做三件事】选中那一行、把它滚进视野、再把命中的那段字节圈出来
  —— 第三件靠 C# 一并返回的 Offset / Length，不像 WinForms 那样让十六进制控件自己再找一次。
*/
const q = ref('')
const qHex = ref(false)
const searching = ref(false)
//「正在查找」的可见状态，超过 SLOW_SEARCH_MS 才亮 —— 理由见 ProxyData 的同名声明（每点一次闪一圈琥珀框）
const slowSearch = ref(false)
const SLOW_SEARCH_MS = 250
/** 上一次命中的行在 C# 列表里的下标。−1 = 还没找过 */
const searchAt = ref(-1)
/*
  上一次命中<b>之后</b>该从这一行的哪个位置接着找（C# 给的 NextPos，原样存、原样传回去，
  前端不需要知道它是什么单位）。

  ⚠️ 这一条是「查找下一个」能走到<b>同一个封包里下一处</b>的全部依据。
  早先游标只有 searchAt 一个、每次传 searchAt + 1，于是一个包里命中三次也只看得到第一处，
  点一下直接跳去下一条包 —— 而 WinForms 那边是逐处走的。
*/
const searchPos = ref(0)
/** 命中的那段字节，交给十六进制面板圈出来 */
const searchHit = ref<{ offset: number; length: number } | null>(null)

interface SearchResult { Found: boolean; Id: number; Index: number; Offset: number; Length: number; NextPos: number; Error: string | null }

async function findNext(): Promise<void> {
  const pattern = q.value.trim()
  if (!pattern || searching.value) return

  searching.value = true
  const slowTimer = window.setTimeout(() => { slowSearch.value = true }, SLOW_SEARCH_MS)

  try {
    //先在当前这一行的剩下部分找（searchPos），找不到 SearchForList 自己会往下一行走
    const from = searchAt.value < 0 ? 0 : searchAt.value
    const fromPos = searchPos.value
    let r = await call<SearchResult>('searchPacketList',{ pattern, isHex: qHex.value, from, fromPos })

    if (r?.Error) {
      /*
        ⚠️ 十六进制那一路的 Error 是 C# 自己写的<b>整句</b>（而且已经本地化过），
        套上「正则表达式有误」的帽子就成了病句 —— 位数是奇数跟正则没关系。
        文本那一路的 Error 是 .NET 抛的 ArgumentException.Message（英文、只有半句），
        那个才需要帽子。
      */
      pushToast('error', qHex.value ? r.Error : t('sp.badRegex') + ' · ' + r.Error)
      return
    }

    //没找到而且不是从头找的 —— 回到开头再来一圈（转完还找不到才算真没有）
    if (!r?.Found && (from > 0 || fromPos > 0)) {
      r = await call<SearchResult>('searchPacketList', { pattern, isHex: qHex.value, from: 0, fromPos: 0 })
      if (r?.Found) pushToast('info', t('sp.wrapped'))
    }

    if (!r?.Found) {
      searchAt.value = -1
      searchPos.value = 0
      searchHit.value = null
      pushToast('warning', t('sp.noMatch'))
      return
    }

    searchAt.value = r.Index
    searchPos.value = r.NextPos

    /*
      滚动用<b>前端副本里的下标</b>，不直接用 C# 给的 Index。
      两边正常是对齐的（推送是顺序 Append、清空是整表），但真错开一格时，
      按 Id 找到的那一行才是刚刚选中的那一行。
    */
    const row = rows.value.find((x) => x.Id === r.Id)

    //高亮只在<b>确实切到那一行</b>之后才给 —— 那个偏移是按命中的那个包算的
    searchHit.value = row && r.Offset >= 0 ? { offset: r.Offset, length: r.Length } : null

    if (row) {
      selected.value = row
      selectedId.value = row.Id
      const i = rows.value.indexOf(row)
      listRef.value?.scrollToIndex(i >= 0 ? i : r.Index)
    }
  } catch (e) {
    console.error('[sp] 查找封包失败', e)
    pushToast('error', String(e))
  } finally {
    window.clearTimeout(slowTimer)
    slowSearch.value = false
    searching.value = false
  }
}

//改了条件就把游标退回开头；上一次的高亮也一并丢掉，它标的是上一个条件命中的那一段
watch([q, qHex], () => {
  searchAt.value = -1
  searchPos.value = 0
  searchHit.value = null
})

function clearSearch(): void {
  q.value = ''
  //watch 也会清，但那是下一拍的事 —— 这里同步清干净，与 searchAt 一致
  searchAt.value = -1
  searchPos.value = 0
  searchHit.value = null
}

/* ── 右键菜单（对应 WinForms 的 GetCMS_PacketList）──────────── */

/*
  与代理数据页那份<b>逐项对应</b>，动作换成 *_ByPacketIds 那一族。
  两份列表的 Id 各自独立自增，同一个数字在两份表里是两条不同的包 ——
  所以桥入口必须分开，不能共用。

  菜单结构照旧<b>由前端自己拼</b>：两个子菜单的内容本来就在推送流里
  （FeedList.Send / FeedList.WareHouse），让 C# 再出一遍 MenuNode 只是多一条要同步的路。
*/
const menuAt = ref<{ x: number; y: number } | null>(null)

/** 封包编辑弹窗的目标；null = 关着 */
const editTarget = ref<{ list: 'proxy' | 'packet' | 'send'; id: number } | null>(null)
/** 「查看数据修改」弹窗看的是哪一条；null = 关着 */
const modifyId = ref<number | null>(null)

//右键只开菜单、不动选中集（与其余各屏同一条口径）
function onMenu(ev: MouseEvent): void {
  menuAt.value = { x: ev.clientX, y: ev.clientY }
}

const sends = useList<SendRow>(FeedList.Send)
const houses = useList<WareHouseRow>(FeedList.WareHouse)

const menuItems = computed<MenuItem[]>(() => {
  const n = picked.value.size
  const tag = n ? ' (' + n + ')' : ''

  const toSend: MenuItem = sends.value.length
    ? {
        id: 'toSend',
        label: t('pm.toSend') + tag,
        icon: ICON.send,
        sub: sends.value.map((x) => ({ id: 'send:' + x.Id, label: x.Name })),
      }
    : { id: 'toSend', label: t('pm.toSend'), icon: ICON.send, disabled: true }

  const toHouse: MenuItem = houses.value.length
    ? {
        id: 'toHouse',
        label: t('pm.toWareHouse') + tag,
        icon: ICON.house,
        sub: houses.value.map((x) => ({ id: 'house:' + x.Id, label: x.Name })),
      }
    : { id: 'toHouse', label: t('pm.toWareHouse'), icon: ICON.house, disabled: true }

  return [
    //只编辑第一条 —— 编辑器一次只装一个包，多选也只能拿一条去改
    { id: 'edit', label: t('pm.edit'), icon: ICON.edit },
    { id: 'modify', label: t('pm.modify'), icon: ICON.hex },
    { divider: true },

    { id: 'copy', label: t('lst.copy') + tag, icon: ICON.copy },
    { divider: true },
    { id: 'toTextA', label: t('pm.toTextA') + tag, icon: ICON.text },
    { id: 'toTextB', label: t('pm.toTextB') + tag, icon: ICON.text },
    { divider: true },
    toSend,
    //添加到滤镜只用第一条（与 WinForms 一致），所以<b>不带条数</b>
    { id: 'toFilter', label: t('pm.toFilter'), icon: ICON.filter },
    toHouse,
    { divider: true },
    { id: 'sysSocket', label: t('pm.setSysSocket'), icon: ICON.check },
    { divider: true },
    //ids 为空就是导整张表，所以不选也能点
    { id: 'excel', label: t('pm.toExcel') + tag, icon: ICON.save },
    { divider: true },
    { id: 'selectAll', label: t('pm.selectAll'), icon: ICON.list },
    { id: 'deselect', label: t('pm.deselect'), icon: ICON.del, disabled: !n },
  ]
})

async function onMenuPick(id: string): Promise<void> {
  const ids = [...picked.value]

  //子菜单：id 形如 "send:<GUID>" / "house:<GUID>"
  if (id.startsWith('send:') || id.startsWith('house:')) {
    if (!ids.length) { needPickToast(); return }

    const toSend = id.startsWith('send:')
    const key = id.slice(id.indexOf(':') + 1)

    try {
      const r = await call<{ count: number }>(
        toSend ? 'addPacketToSend' : 'addPacketToWareHouse',
        toSend ? { sid: key, ids } : { wid: key, ids })

      if (r?.count) pushToast('success', t('pm.added') + ' ' + r.count)
      else pushToast('error', t('pm.addFail'))
    } catch (e) {
      console.error('[pm] 添加失败', e)
    }

    return
  }

  switch (id) {
    case 'selectAll':
      pick.selectAll()
      pickTrimmed.value = false
      return

    case 'deselect':
      pick.clear()
      pickTrimmed.value = false
      return

    //导出不要求先选：ids 为空时 C# 侧会导整张表
    case 'excel':
      try { await call('exportPacketExcel', { ids }) } catch (e) { console.error('[pm] 导出失败', e) }
      return
  }

  if (!ids.length) {
    needPickToast()
    return
  }

  try {
    switch (id) {
      case 'copy': {
        const r = await call<{ text: string }>('copyPacketHex', { ids })
        if (!r?.text) { pushToast('error', t('pm.copyFail')); return }
        await call('clipboardWrite', { text: r.text })
        pushToast('success', t('pm.copied'))
        return
      }

      case 'edit':
        editTarget.value = { list: 'packet', id: ids[0] }
        return

      //添加到文本 A / B：整体替换（WinForms 的 SetTextA 也是替换不是追加），然后切到文本对比页
      case 'toTextA':
      case 'toTextB': {
        const r = await call<{ text: string }>('copyPacketHex', { ids })
        if (!r?.text) { pushToast('error', t('pm.copyFail')); return }
        if (id === 'toTextA') textA.value = r.text
        else textB.value = r.text
        pushToast('success', t(id === 'toTextA' ? 'pm.toTextAOk' : 'pm.toTextBOk'))
        gotoPage.value = 'diff'
        return
      }

      //查看数据修改：也只看第一条
      case 'modify':
        modifyId.value = ids[0]
        return

      case 'toFilter': {
        const r = await call<{ ok: boolean }>('addPacketToFilter', { id: ids[0] })
        pushToast(r?.ok ? 'success' : 'error', t(r?.ok ? 'pm.toFilterOk' : 'pm.toFilterFail'))
        return
      }

      case 'sysSocket': {
        const r = await call<{ socket: number }>('setSystemSocketByPacket', { id: ids[0] })
        if (r?.socket) pushToast('success', t('pm.sysSocketOk') + ' ' + r.socket)
        else pushToast('error', t('pm.sysSocketFail'))
        return
      }
    }
  } catch (e) {
    console.error('[pm] ' + id + ' 失败', e)
  }
}

/** 列表被清空（自动清理 / 手动清空）之后收拾右边的面板。 */
function onCleared(): void {
  selected.value = null
  selectedId.value = null
  searchHit.value = null
  pick.clear()
}

defineExpose({ onCleared })
</script>

<template>
  <div class="datapage">
    <InjectBar
      :busy="props.busy"
      @toggle-hook="emit('toggleHook')"
      @clear="emit('clear')"
      @open-setting="emit('openSetting', $event)"
    />

    <div class="stats">
      <!--
        整格挂提示，不是只挂在数字上：格子是<b>固定宽度</b>的（七分之一屏），
        标题与数字都靠省略号兜底，三行都可能被截。

        实测：1280 CSS 宽时格子 156px，十亿都放得下；
        但 <b>125% 缩放下只有 1024 CSS 宽，格子 120px</b>，「100,000,000」要 128px —— 截断。
        去掉千分位也救不回来（Orbitron 的数字不等宽，999999999 仍要 127px）。

        <b>按要求不改字号、也不换成 100.0M 那种单位</b>：这一格的意义就是给一个
        能对得上的准确数（「最大序号 == 代理总数」那条自检要拿它去比），
        换成约数就没法比了。改成挂提示，截断时把完整的一行给出来。
      -->
      <!--
        ⚠️ 这里是 <b>:data-tip 而不是 :title</b> —— 全项目仅有的两处这么写。
        这一格的值每 500ms 跟着 getStats 变一次，而 Vue 的 patch 就是 setAttribute：
        写 title 的话，每半秒就往元素上安一次原生提示，自绘的那套只能在下一个
        微任务里再摘走 —— 那个窗口够不够 Blink 把灰框弹出来，是它的实现细节，靠不住。
        直接写 data-tip 就没有窗口：原生从头到尾无题可画，tooltip.ts 照样认得它。
        详见 tooltip.ts 头上「两道防线」那段。
      -->
      <div v-for="c in cells" :key="c.k" class="st-c" :data-tip="c.z + ' · ' + c.v">
        <div class="k">{{ c.k }}</div>
        <div class="v" :class="c.tone">{{ c.v }}</div>
        <div class="z">{{ c.z }}</div>
      </div>
    </div>

    <!--
      目标没了的横幅。刻意<b>不清任何数据</b>：抓到的封包、配置、界面全部保留，
      用户还能继续看、继续导出 —— 这正是 IPC 改造要解决的第一条风险（崩溃不隔离）。
    -->
    <div v-if="status.state === 'disconnected'" class="lostbar">{{ t('inject.lostHint') }}</div>

    <div class="grid">
      <div class="gtool">
        <!--
          行底色图例。位置与代理数据页一致：放在搜索框<b>之前</b>而不是表格下沿 ——
          这一屏高度紧张，单独一条占掉的是封包能看见的行数，
          而工具条这一行本来就有富余的横向空间。

          <b>色块必须取自 prefs</b>（C# 的 UiPrefs，用户可改），不能写死十六进制。
          点色块就地改这一组的配色 —— 挑颜色要看着它在表里的样子。
        -->
        <span v-if="prefs" class="plegend">
          <button
            v-for="x in ROW_LEGEND"
            :key="x.key"
            class="lg"
            :style="{ background: x.pair.back, color: x.pair.fore }"
            :title="t('set.colorTip')"
            @click="colorEdit = x.key"
          >{{ t(x.label) }}</button>
        </span>

        <!--
          查找封包。Enter = 查找下一个，Esc = 清空；右边两个按钮是「文本 / 十六进制」与「从头查找」。
          没有独立的「向下搜索」单选 —— 主按钮本身就是向下找，找到末尾自己回头再来一圈。
        -->
        <span class="search" :class="{ busy: slowSearch }">
          <svg class="ico" viewBox="0 0 24 24"><circle cx="11" cy="11" r="7" /><path d="M20 20l-4-4" /></svg>
          <input
            v-model="q"
            class="sinp"
            spellcheck="false"
            :placeholder="t('proxy.search')"
            @keydown.enter.prevent="findNext()"
            @keydown.esc.prevent="clearSearch"
          >
          <button v-if="q" class="sx" :title="t('sp.clear')" @click="clearSearch">×</button>
        </span>

        <!--
          ⚠️ 这里<b>不能用勾选框</b>：它原先是一个 .chk，标签跟着自己的状态变
          （勾上＝绿＝「十六进制」，点一下变成没勾＝「文本」），读起来成了
          「『文本』这一项没勾选」—— 而这是二选一，没有「都不选」这个态。
          换成与十六进制面板同一套的分段按钮：两格都摆出来，亮着的那格就是当前值，
          颜色也跟那边对齐（绿＝十六进制、青＝文本）。
        -->
        <div class="hx-seg" :title="t('sp.modeHint')">
          <button class="hx-segb after" :class="{ on: qHex }" @click="qHex = true">{{ t('sp.hex') }}</button>
          <button class="hx-segb before" :class="{ on: !qHex }" @click="qHex = false">{{ t('sp.text') }}</button>
        </div>
        <!--
          只剩一个按钮。原先旁边还有个「从头查找」，在「查找下一个」会自己转一圈之后
          就没有它能做而这个做不了的事了 —— 留着只是多一个要读的控件。
          真要从头再来：改一下查找内容、或者按 Esc / 点 × 清空，游标都会退回开头。
        -->
        <button class="tb" :disabled="!q.trim() || slowSearch" @click="findNext()">
          {{ slowSearch ? t('sp.searching') : t('sp.next') }}
        </button>

        <button class="chk" :class="{ on: follow }" @click="follow = !follow"><i />{{ t('proxy.autoRoll') }}</button>
        <!--
          自动清理 —— 就摆在它管的那张表上面（见 script 里的说明）。
          与日志页工具条上那个是<b>两份配置</b>，所以文案也分开：
          这儿是「自动清理」，那儿是「日志自动清理」。
        -->
        <span class="pair">
          <button class="chk" :class="{ on: autoClear }" @click="toggleAutoClear">
            <i />{{ t('proxy.autoClear') }}
          </button>
          <input
            v-model="keepInput"
            class="num"
            :class="{ bad: keepBad }"
            type="number"
            min="100"
            max="500000"
            :disabled="!autoClear"
            :title="t('set.keepRowsHint')"
            @blur="commitKeep"
            @keydown.enter="commitKeep"
          >
        </span>
      </div>

      <PacketList
        ref="listRef"
        class="list"
        mode="inject"
        :prefs="prefs"
        :selected-id="selectedId"
        :picked="picked"
        :follow="follow"
        @select="onSelect"
      @open="(r: any) => (editTarget = { list: 'packet', id: r.Id })"
        @menu="onMenu"
      />

      <ContextMenu :at="menuAt" :items="menuItems" @pick="onMenuPick" @close="menuAt = null" />

      <!-- 封包编辑：保存后 C# 按行 UI.Feed.Update 推回来，这里不用做别的 -->
      <PacketEdit :target="editTarget" @close="editTarget = null" />
      <PacketModification :id="modifyId" list="packet" @close="modifyId = null" />

      <!-- 图例上点色块开的配色弹窗，与代理数据页同一个组件 -->
      <ActionColor
        :action="colorEdit"
        :fore="ROW_LEGEND.find((x) => x.key === colorEdit)?.pair.fore || '#ffffff'"
        :back="ROW_LEGEND.find((x) => x.key === colorEdit)?.pair.back || '#000000'"
        @close="colorEdit = null"
        @saved="reloadPrefs"
      />
    </div>

    <div class="lower">
      <QuickPanel mode="inject" />
      <!-- packetType 只用来决定默认按文本还是十六进制看（HTTP 那四类默认文本）-->
      <HexPanel :id="selectedId" list="packet" :packet-type="selected?.Type ?? null" :highlight="searchHit" />
    </div>
  </div>
</template>

<style scoped>
/*
  这一屏的骨架与工具条都在 style.css 里（`.datapage` / `.gtool` 两族），
  与代理数据页<b>共用同一份</b> —— 2026-09-10 收拢的，此前各抄一份、已经抄歪了
  （`.st-c .z` 一个 10px 一个 10.5px、`.list` 少一句 border-radius、图例整块没有）。

  这里只剩注入这一屏独有的一条。
*/

/*
  目标没了之后的提示条。
  ⚠️ 数据仍留在列表里（那正是这时候要看的东西），所以是「提示」不是「清空」。
*/
.lostbar {
  flex: none;
  padding: 7px 14px 5px;
  background: rgb(var(--danger-rgb) / 8%);
  border: 1px solid rgb(var(--danger-rgb) / 30%);
  color: var(--danger);
  font-size: var(--fs-small);
}
</style>
