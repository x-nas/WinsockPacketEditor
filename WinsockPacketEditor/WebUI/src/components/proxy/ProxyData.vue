<script setup lang="ts">
/*
  「代理数据」页 —— 对应 WinForms 的 Controls/ProxyList。

  四层结构与 WinForms 一致：
    ① 运行状态条（RunBar）—— WinForms 里是工具条上两个普通按钮，这里提成一条
    ② 统计网格 13 项    —— 取值与「计时器 - 更新代理统计信息」逐条对齐
    ③ 数据表           —— 定高虚拟滚动，B10 那个 PacketList 原样复用
    ④ 快捷面板 + 十六进制

  这一屏的骨架（.datapage / .stats / .grid / .lower …）与工具条那一套（.gtool …）
  都在 style.css 里 —— 注入模式的封包页用的是<b>同一份</b>，所以本文件没有 scoped 样式。
*/
import { computed, onBeforeUnmount, onMounted, ref, watch } from 'vue'
import { call } from '../../bridge'
import { FeedList, type PacketListRow, type Prefs, type ProxyRow, type SendRow, type Stats, type WareHouseRow } from '../../bridge/types'
import { t } from '../../i18n'
import { attachPacketFeed, onProxyTrimmed, resetStat, rows } from '../../stores/packets'
import { useList } from '../../stores/lists'
import { pushToast } from '../../stores/toast'
import { useRowPick } from '../../usePick'
import { gotoPage, listSetting, proxyRunning } from '../../stores/runtime'
import { textA, textB } from '../../stores/tools'
import PacketList from '../PacketList.vue'
import ContextMenu from '../ContextMenu.vue'
import { ICON, type MenuItem } from '../menu'
import HexPanel from '../HexPanel.vue'
import PacketEdit from './PacketEdit.vue'
import PacketModification from './PacketModification.vue'
import RunBar from './RunBar.vue'
import QuickPanel from './QuickPanel.vue'
import ProxySetting from './ProxySetting.vue'
import ListSetting from './ListSetting.vue'
import LeachSetting from './LeachSetting.vue'
import FireWallSetting from './FireWallSetting.vue'
import HookSetting from './HookSetting.vue'
import SystemSetting from './SystemSetting.vue'
import ProcessSetting from './ProcessSetting.vue'
import MapSetting from './MapSetting.vue'
import ExtProxySetting from './ExtProxySetting.vue'
import HotkeySetting from './HotkeySetting.vue'
import BackupSetting from './BackupSetting.vue'
import RemoteSetting from './RemoteSetting.vue'
import ActionColor from './ActionColor.vue'
import type { SettingKey } from './settings'

const prefs = ref<Prefs | null>(null)
const stats = ref<Stats | null>(null)
const selected = ref<ProxyRow | null>(null)
const selectedId = computed(() => selected.value?.Id ?? null)

/** 当前打开的设置弹窗。null = 没开。12 项里目前只有 proxy 有实现。 */
const setting = ref<SettingKey | null>(null)

/*
  正在改配色的那一组动作（图例上点开的），null = 没开。
  取当前值直接从 prefs 拿 —— 图例画的就是它，不必再往返一次。
*/
const colorEdit = ref<string | null>(null)

async function reloadPrefs(): Promise<void> {
  try {
    //改完立刻重取：图例与列表的行配色都吃这份 prefs，重取就地生效
    prefs.value = await call<Prefs>('getPrefs')
  } catch (e) {
    console.error('[proxy] 重取界面偏好失败', e)
  }
}

const autoRoll = ref(true)

/*
  自动清理 —— 2026-09-07 从「列表设置」弹窗搬到这条工具条上。

  「设置摆在哪儿，就代表它管哪张表」：日志那份一直在日志页的工具条上，
  而这一份原先藏在弹窗里，两个长得一模一样的「自动清理」谁都会以为是同一个。
  在这儿它紧挨着它真正会裁的那张表。

  ⚠️ <b>封包列表与代理列表共用这一份配置</b>（PacketConfig.List.AutoClear，
  InjectMode 表），所以桥方法 saveListAutoClear <b>不带 mode</b> ——
  与列显隐那一对相反，别顺手给它加。

  ⚠️ 初值走 getListSetting（挂载时本来就调了），<b>不再从 500ms 的 getStats 里拿</b>：
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

let detach: (() => void) | null = null
let statsTimer = 0

onMounted(async () => {
  detach = attachPacketFeed()

  try {
    prefs.value = await call<Prefs>('getPrefs')
  } catch (e) {
    console.error('[proxy] 取界面偏好失败', e)
  }

  //列显隐要在第一帧之前拿到，否则列表先按「全显示」画一遍再跳
  try {
    const s = await call<any>('getListSetting')
    listSetting.value = s

    //自动清理的初值搭同一趟车，不另开一条桥
    autoClear.value = !!s?.autoClear
    clearAt.value = Number(s?.autoClearValue) || 5000
    keepInput.value = String(clearAt.value)
  } catch (e) {
    console.error('[proxy] 取列表设置失败', e)
  }

  // 统计单独轮询，不占推送通道
  statsTimer = window.setInterval(async () => {
    try {
      const s = await call<Stats>('getStats')
      stats.value = s
      proxyRunning.value = !!(s as any).proxyRunning
    } catch {
      /* 窗口关闭中，忽略 */
    }
  }, 500)
})

onBeforeUnmount(() => {
  detach?.()
  offTrimmed()
  window.clearInterval(statsTimer)
})

async function clearAll(): Promise<void> {
  selected.value = null
  picked.value = new Set()
  /*
    ⚠️ <b>要指明清哪一张表。</b> 不带 list 的话桥会把封包列表也一起清了 ——
    外壳一次只在一种模式里，另一张本来就是空的，所以看不出差别；
    但自从「清空」<b>连计数一起复位</b>之后，含糊的写法会顺带把另一种模式的计数也归零。
  */
  await call('clearPackets', { list: FeedList.Proxy })
  resetStat()
}

/* ── 封包列表的选中与右键菜单 ──────────────────────────────── */

/*
  两套选中态，别混：
    selected  详情面板正在看的那一行（只有一行）
    picked    多选集，右键菜单的动作作用于它

  一行可以既在 picked 里、又是 selected —— 所以行上是两种不同的记号
  （描边 vs 左边线 + 淡底），见 PacketList 的 .pl-row.sel / .pl-row.pick。
*/
/*
  多选走全项目共用的那份（usePick.ts）。

  ⚠️ <b>autoPrune 必须关掉</b>：那份实现默认挂一个 watch(rows) 来剔除已消失的 Id，
  而这一屏的 rows 每帧都 triggerRef —— 挂上去等于每秒跑 60 次。
  这里改成只在列表被清空时清一次（下面那个 watch），代价与收益都对得上。
*/
const { picked, onRowClick, selectAll, clear, trimHead } =
  useRowPick(rows, (r) => r.Id, { autoPrune: false })

/*
  选中的封包被自动清理裁掉了。

  自动清理 2026-09-07 起是<b>环形</b>（只留最近 N 条），不是整表清空 ——
  选中集里的 Id 会随着表头被裁一起失效，而页面上看不出来（那一行已经滚走了）。
  以前没人收拾：右键「编辑」拿着一条 C# 那边已经没有的 Id 去开编辑器，
  弹出来的是「这条封包已经不在列表里了」—— 真机冒烟里被报成了毛病。

  现在裁剪一来就把它们从选中集里剔掉，并记下「是被裁掉的」：
  右键动作撞上空选中集时，说清楚是自动清理把它清掉了，而不是一句笼统的「请先选中」。
  详情面板（selected）<b>刻意不清</b>：字节已经取回来了，用户可能正读着 ——
  与「环形而不是整表清空」是同一个理由，别把正在看的那一屏抹掉。
*/
const pickTrimmed = ref(false)

const offTrimmed = onProxyTrimmed((removed) => {
  if (trimHead(removed) > 0 && picked.value.size === 0) pickTrimmed.value = true
})

function needPickToast(): void {
  pushToast('warning', pickTrimmed.value ? t('pm.pickTrimmed') : t('lst.needPick'))
}

function onSelect(anyRow: PacketListRow, ev: MouseEvent, index: number): void {
  //PacketList 两种模式共用，事件签名是并集；这一屏是 mode="proxy"，行一定是 ProxyRow
  const r = anyRow as ProxyRow
  selected.value = r
  onRowClick(r, ev, index)
  pickTrimmed.value = false

  /*
    手动点行就把查找的高亮丢掉。

    不丢的话，那个偏移会原样套到新点开的这一条上 —— 十六进制面板只认「圈第 N 到第 M 字节」，
    并不知道那是上一条封包里的位置，圈出来的会是一段毫不相干的字节，而且看着像是查到的。
  */
  searchHit.value = null
}

/*
  用户点「清空」/ 切库会把整表清空（feed:clear），选中集跟着清 ——
  留着已经不存在的 Id 不会报错，但「选了 8 条」却一条都动不了，比空着更费解。
  自动清理不走这条（它是环形裁剪），见上面 onProxyTrimmed。
*/
watch(() => rows.value.length, (n) => {
  if (n === 0) { clear(); pickTrimmed.value = false }
})

/* ── 查找封包（对应 WinForms 的 Controls/SearchPacket）───────── */

/*
  WinForms 那边是工具条上一个放大镜按钮，点开一个从顶部滑下来的抽屉，里面四样东西：
  正则输入 + 文本/十六进制 + 从头/向下 + 「查找下一个」。这里摊平成工具条上的一条 ——
  抽屉盖住的正是要看的表头那一片，而这几个控件横过来一行就放得下。

  【只有一个按钮】那边的「从头 / 向下」是一对单选，这里两个都不要：
  「查找下一个」<b>本身就是向下找，走到末尾自己回头再来一圈</b>，
  所以「向下」是默认、「从头」也没有它能做而这个做不了的事。
  （曾经真摆过一个「从头查找」按钮，就是这个理由删掉的。
  要从头再来：改一下查找内容，或按 Esc / 点 × 清空，游标都会退回开头。）

  【游标留在前端】C# 侧不存 Search_Index：搜索是「从第 N 行往后找第一条」这一件事，
  游标是谁在翻页谁的状态。WinForms 把它放在 Operate 里，是因为控件与 Operate 之间没有别的通道。

  【命中之后做三件事】选中那一行（详情面板跟着换）、把它滚进视野、
  再把命中的那段字节圈出来 —— 第三件靠 C# 一并返回的 Offset / Length，
  不像 WinForms 那样让十六进制控件自己再找一次。
*/
const listRef = ref<InstanceType<typeof PacketList> | null>(null)

const q = ref('')
const qHex = ref(false)
const searching = ref(false)
/*
  「正在查找」的<b>可见</b>状态（搜索框转琥珀、按钮写「查找中」）。

  ⚠️ 与 searching 分开：一次查找通常几毫秒就完，把忙碌态直接挂在 searching 上，
  每点一次「查找下一个」搜索框就闪一圈琥珀框 —— 真机冒烟里被当成毛病报上来过。
  超过 SLOW_SEARCH_MS 还没回来才亮，那时它才是有信息量的。防重入仍看 searching。
*/
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
    let r = await call<SearchResult>('searchProxyList', { pattern, isHex: qHex.value, from, fromPos })

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

    /*
      没找到而且不是从头找的 —— 回到开头再来一次（WinForms 是把「从头开始」做成一个单选项，
      要用户自己去点；转一圈更符合「查找下一个」的习惯，转完还找不到才算真没有）。
    */
    if (!r?.Found && (from > 0 || fromPos > 0)) {
      r = await call<SearchResult>('searchProxyList', { pattern, isHex: qHex.value, from: 0, fromPos: 0 })
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
      按 Id 找到的那一行才是刚刚选中的那一行 —— 宁可多扫一遍也不要滚到隔壁。
    */
    const row = rows.value.find((x) => x.Id === r.Id)

    /*
      高亮只在<b>确实切到那一行</b>之后才给。找不到那一行（C# 报了命中、前端副本里却没有，
      通常是这一拍刚好被自动清理了）时必须清空 —— 那个偏移是按命中的那个包算的，
      套在面板上还显示着的另一条包上会圈出一段无关字节。
    */
    searchHit.value = row && r.Offset >= 0 ? { offset: r.Offset, length: r.Length } : null

    if (row) {
      selected.value = row
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

/*
  改了条件就把游标退回开头 —— 沿用旧游标会跳过前面本该命中的行；
  上一次的高亮也一并丢掉，它标的是上一个条件命中的那一段。
*/
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

const menuAt = ref<{ x: number; y: number } | null>(null)

/** 封包编辑弹窗的目标；null = 关着 */
const editTarget = ref<{ list: 'proxy' | 'send'; id: number } | null>(null)
/** 「查看数据修改」弹窗看的是哪一条；null = 关着 */
const modifyId = ref<number | null>(null)

//右键只开菜单、不动选中集（与账号 / 滤镜两屏同一条口径）
function onMenu(ev: MouseEvent, _r: PacketListRow): void {
  menuAt.value = { x: ev.clientX, y: ev.clientY }
}

const sends = useList<SendRow>(FeedList.Send)
const houses = useList<WareHouseRow>(FeedList.WareHouse)

const menuItems = computed<MenuItem[]>(() => {
  const n = picked.value.size
  const tag = n ? ' (' + n + ')' : ''

  /*
    「添加到发送 / 添加到仓库」的子菜单<b>由前端自己拼</b>：
    这两份列表本来就一直在推（FeedList.Send / FeedList.WareHouse），
    再让 C# 出一遍 MenuNode 只是多一条要同步的路。
    列表为空时压暗整项 —— 与 WinForms 的 GetCMS_PacketList 一致。
  */
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
    //与 WinForms 一样一条一行（十六进制），写进文本对比页那两个框；带条数
    { id: 'toTextA', label: t('pm.toTextA') + tag, icon: ICON.text },
    { id: 'toTextB', label: t('pm.toTextB') + tag, icon: ICON.text },
    { divider: true },
    toSend,
    /*
      添加到滤镜只用第一条 —— 与 WinForms 一致（那边也是 piList[0]）：
      一条滤镜描述的是「怎么匹配、怎么改」，多选几条也只能拿一条去造。
      所以这一项<b>不带条数</b>，免得看着像会加好几条。
    */
    { id: 'toFilter', label: t('pm.toFilter'), icon: ICON.filter },
    toHouse,
    { divider: true },
    { id: 'sysSocket', label: t('pm.setSysSocket'), icon: ICON.check },
    { divider: true },
    //ids 为空就是导整张表，所以不选也能点
    { id: 'excel', label: t('pm.toExcel') + tag, icon: ICON.save },
    { divider: true },
    /*
      这一项<b>永远是全选</b>，不是开关 —— 所以标签不能用「全选 / 取消全选」，
      那会让人以为再点一次能取消。取消交给下面那一项。
    */
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
        toSend ? 'addProxyToSend' : 'addProxyToWareHouse',
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
      selectAll()
      pickTrimmed.value = false
      return

    case 'deselect':
      clear()
      pickTrimmed.value = false
      return

    //导出不要求先选：ids 为空时 C# 侧会导整张表
    case 'excel':
      try { await call('exportProxyExcel', { ids }) } catch (e) { console.error('[pm] 导出失败', e) }
      return
  }

  if (!ids.length) {
    needPickToast()
    return
  }

  try {
    switch (id) {
      case 'copy': {
        const r = await call<{ text: string }>('copyProxyHex', { ids })
        if (!r?.text) { pushToast('error', t('pm.copyFail')); return }
        await call('clipboardWrite', { text: r.text })
        pushToast('success', t('pm.copied'))
        return
      }

      case 'edit':
        editTarget.value = { list: 'proxy', id: ids[0] }
        return

      //添加到文本 A / B：整体替换（WinForms 的 SetTextA 也是替换不是追加），然后切到文本对比页
      case 'toTextA':
      case 'toTextB': {
        const r = await call<{ text: string }>('copyProxyHex', { ids })
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
        const r = await call<{ ok: boolean }>('addProxyToFilter', { id: ids[0] })
        pushToast(r?.ok ? 'success' : 'error', t(r?.ok ? 'pm.toFilterOk' : 'pm.toFilterFail'))
        return
      }

      case 'sysSocket': {
        const r = await call<{ socket: number }>('setSystemSocketByProxy', { id: ids[0] })
        if (r?.socket) pushToast('success', t('pm.sysSocketOk') + ' ' + r.socket)
        else pushToast('error', t('pm.sysSocketFail'))
        return
      }
    }
  } catch (e) {
    console.error('[pm] ' + id + ' 失败', e)
  }
}

/** 代理总数是六个计数相加，与 WinForms 的算法一致。 */
const total = computed(() => {
  const s: any = stats.value
  if (!s) return 0
  return (s.tcpReq || 0) + (s.tcpResp || 0) + (s.udpReq || 0)
    + (s.udpResp || 0) + (s.httpReq || 0) + (s.httpResp || 0)
})

function n(v: number | undefined): string {
  return (v ?? 0).toLocaleString()
}

/*
  字节数按量级给单位，<b>只给一位小数</b>。

  C# 那份 GetDisplayBytes 的详版是「62.4 KB (63,915 Bytes)」——
  给 WinForms 的宽标签用正好，塞进这里七分之一屏宽的格子必然溢出（就是这么发现的）。
  统计格子要的是一眼能读的量级，精确字节数没人在这里读。
*/
function bytes(v: number | undefined): string {
  let x = v ?? 0
  if (x < 1024) return x + ' B'

  const u = ['KB', 'MB', 'GB', 'TB']
  let i = -1
  while (x >= 1024 && i < u.length - 1) { x /= 1024; i++ }
  return x.toFixed(1) + ' ' + u[i]
}

/** 速率同理，来源是 C# 算好的 KB/s。 */
function kbps(v: number | undefined): string {
  const x = v ?? 0
  return x >= 1024 ? (x / 1024).toFixed(1) + ' MB/s' : x.toFixed(1) + ' KB/s'
}

/*
  上下行拆分，<b>两个值共用一个单位</b>，单位只写一次。

  各自独立换算会出「↑1.2 ↓8.4 MB/s」这种把 KB 当 MB 读的错行 —— 单位取较大那个值的，
  小的那个跟着换算，才不会骗人。单位写一次是为了塞进七分之一屏宽的格子（实测最紧时约 106px）。
*/
function pairKbps(up: number | undefined, down: number | undefined): string {
  const a = up ?? 0
  const b = down ?? 0
  const mb = Math.max(a, b) >= 1024

  const f = (x: number) => (mb ? x / 1024 : x).toFixed(1)
  return '↑' + f(a) + '  ↓' + f(b) + (mb ? ' MB/s' : ' KB/s')
}

/*
  行底色图例。四组与 PacketList 的 colorOf() 一一对应 ——
  那里按 FilterAction 取 prefs.filter 的某一组打行内样式，这里就把同一组摊出来。

  <b>没有「不显示」那一档</b>：NoModify_NoDisplay 的行根本不进列表，
  给一个永远看不到的颜色配图例只会让人去找它。
  colorOf 的 default 分支（未命中滤镜）也不列 —— 那是"没有底色"，不是一种底色。
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

/*
  统计 13 项。tone 决定数字的颜色：
    g 绿（常规计数）· c 青（连接/流量）· a 琥珀（值得注意的：滤镜命中、积压、速率）
*/
const cells = computed(() => {
  const s: any = stats.value || {}
  return [
    { k: 'Total', z: t('proxy.st.total'), v: n(total.value), tone: 'g' },
    { k: 'TCP', z: t('proxy.st.tcpConn'), v: n(s.tcpConn), tone: 'c' },
    { k: 'UDP', z: t('proxy.st.udpConn'), v: n(s.udpConn), tone: 'c' },
    { k: 'Account', z: t('proxy.st.account'), v: s.onlineInfo || '0/0', tone: 'g' },
    /*
      FILTERED = <b>被过滤设置挡掉、没进列表</b>的封包数（Operate 侧的 FilterProxy_CNT，
      在 FlushToFeed 里 IsShowProxy_ByFilter 判否时自增）。
      不是 FilterExecute_CNT —— 那是滤镜规则总共执行了多少次，是另一回事，
      而且滤镜的执行次数现在按条显示在滤镜列表里，不需要再放一个总数在这儿。
    */
    { k: 'Filtered', z: t('proxy.st.filter'), v: n(s.filterProxy), tone: 'a' },
    { k: 'Queue', z: t('proxy.st.queue'), v: n(s.queue), tone: (s.queue || 0) > 5000 ? 'a' : 'g' },
    /*
      总流量这一格是双值的：大字给合计，小字给 ↑请求 / ↓响应 的拆分。
      其余格子的第三行是中文说明，这里换成拆分 —— 上面的 BYTES 已经说清是什么了，
      而拆分比重复一遍「总流量」有用。
    */
    {
      k: 'Bytes',
      z: '↑ ' + bytes(s.totalRequest) + ' · ↓ ' + bytes(s.totalResponse),
      v: bytes((s.totalRequest || 0) + (s.totalResponse || 0)),
      tone: 'c',
    },
    { k: 'TCP Req', z: t('proxy.st.tcpReq'), v: n(s.tcpReq), tone: 'g' },
    { k: 'TCP Resp', z: t('proxy.st.tcpResp'), v: n(s.tcpResp), tone: 'g' },
    { k: 'UDP Req', z: t('proxy.st.udpReq'), v: n(s.udpReq), tone: 'g' },
    { k: 'UDP Resp', z: t('proxy.st.udpResp'), v: n(s.udpResp), tone: 'g' },
    //这两格含 WebSocket（类型 21 / 22 并进了 HTTP 计数，见 Operate 里那段说明）
    { k: 'HTTP / WS Req', z: t('proxy.st.httpReq'), v: n(s.httpReq), tone: 'g' },
    { k: 'HTTP / WS Resp', z: t('proxy.st.httpResp'), v: n(s.httpResp), tone: 'g' },
    /*
      实时网速。与总流量那格同一种排法（大字给合计、小字给拆分），
      两格分别落在两行的末尾，视觉上成对。
      这里原先是「列表行数」—— 侧栏「代理数据」右侧的计数已经是同一个数，重复了。
    */
    {
      k: 'Speed',
      z: pairKbps(s.speedUp, s.speedDown),
      v: kbps((s.speedUp || 0) + (s.speedDown || 0)),
      tone: 'a',
    },
  ]
})

</script>

<template>
  <div class="datapage">
    <RunBar @clear="clearAll" @open-setting="setting = $event" />

    <ProxySetting
      :open="setting === 'proxy'"
      @update:open="setting = $event ? 'proxy' : null"
    />

    <ListSetting
      :open="setting === 'list'"
      @update:open="setting = $event ? 'list' : null"
    />

    <LeachSetting
      :open="setting === 'leach'"
      @update:open="setting = $event ? 'leach' : null"
    />

    <FireWallSetting
      :open="setting === 'firewall'"
      @update:open="setting = $event ? 'firewall' : null"
    />

    <HookSetting
      :open="setting === 'hook'"
      @update:open="setting = $event ? 'hook' : null"
    />

    <SystemSetting
      :open="setting === 'system'"
      @update:open="setting = $event ? 'system' : null"
    />

    <!-- goto：进程设置第 4 步的「打开代理设置」—— 只是导航，那一屏不代管别人的配置 -->
    <ProcessSetting :open="setting === 'process'" @update:open="setting = $event ? 'process' : null" @goto="setting = $event" />
    <MapSetting :open="setting === 'map'" @update:open="setting = $event ? 'map' : null" />
    <ExtProxySetting :open="setting === 'extproxy'" @update:open="setting = $event ? 'extproxy' : null" />
    <HotkeySetting :open="setting === 'hotkey'" @update:open="setting = $event ? 'hotkey' : null" />
    <BackupSetting :open="setting === 'backup'" @update:open="setting = $event ? 'backup' : null" />
    <RemoteSetting :open="setting === 'remote'" @update:open="setting = $event ? 'remote' : null" />

    <ActionColor
      :action="colorEdit"
      :fore="ROW_LEGEND.find((x) => x.key === colorEdit)?.pair.fore || '#ffffff'"
      :back="ROW_LEGEND.find((x) => x.key === colorEdit)?.pair.back || '#000000'"
      @close="colorEdit = null"
      @saved="reloadPrefs"
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

    <div class="grid">
      <div class="gtool">
        <!--
          行底色图例。放在搜索框<b>之前</b>而不是表格下沿：首页高度紧张，
          单独一条占掉的是封包能看见的行数，而工具条这一行本来就有富余的横向空间。

          <b>色块必须取自 prefs</b>，不能像滤镜编辑那三个那样写死十六进制：
          那三个是老版传下来的固定约定，而这四组是用户在颜色设置里可改的
          （C# 的 UiPrefs），写死了改完颜色图例就开始骗人。
        -->
        <span v-if="prefs" class="plegend">
          <!--
            色块可点：就地改这一组的配色。挑颜色要看着它在表里的样子，
            隔着「设置 ▾ → 系统设置」挑完再回来看效果是反的 ——
            所以入口放在图例本身，改完当场就能在下面的表里看到。
          -->
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
        <!-- 忙碌态看 slowSearch 而不是 searching —— 理由见它的声明；连点由 findNext 自己的防重入挡住 -->
        <button class="tb" :disabled="!q.trim() || slowSearch" @click="findNext()">
          {{ slowSearch ? t('sp.searching') : t('sp.next') }}
        </button>

        <button class="chk" :class="{ on: autoRoll }" @click="autoRoll = !autoRoll"><i />{{ t('proxy.autoRoll') }}</button>
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
        :prefs="prefs"
        :selected-id="selectedId"
        :picked="picked"
        :follow="autoRoll"
        @select="onSelect"
      @open="(r: any) => (editTarget = { list: 'proxy', id: r.Id })"
        @menu="onMenu"
      />

      <ContextMenu :at="menuAt" :items="menuItems" @pick="onMenuPick" @close="menuAt = null" />

      <!-- 封包编辑：保存后 C# 按行 UI.Feed.Update 推回来，这里不用做别的 -->
      <PacketEdit :target="editTarget" @close="editTarget = null" />
      <PacketModification :id="modifyId" @close="modifyId = null" />

    </div>

    <div class="lower">
      <QuickPanel />
      <!-- packetType 只用来决定默认按文本还是十六进制看（HTTP 那四类默认文本）-->
      <HexPanel :id="selectedId" :packet-type="selected?.Type ?? null" :highlight="searchHit" />
    </div>

  </div>
</template>

