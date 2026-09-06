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
import InjectBar from './InjectBar.vue'
import type { SettingKey } from '../proxy/settings'

const emit = defineEmits<{
  (e: 'toggleHook'): void
  (e: 'clear'): void
  (e: 'detach'): void
  (e: 'openSetting', key: SettingKey): void
}>()

const props = defineProps<{ busy: boolean }>()

const prefs = ref<Prefs | null>(null)
const rows = injectFeed.rows
const stat = injectFeed.stat

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
  autoClear: boolean; autoClearValue: number
}

const stats = ref<InjectStats | null>(null)
let statsTimer = 0

/*
  「自动清理」在工具条上<b>只读显示</b>，点它跳到列表设置去改 ——
  与代理数据页同一条口径：两处都能改反而要同步。
*/
const autoClear = computed(() => stats.value?.autoClear ?? true)
const clearAt = computed(() => stats.value?.autoClearValue ?? 5000)

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

  return [
    { k: 'Total', z: t('inject.st.total'), v: n(s?.total), tone: 'c' },
    { k: 'Send', z: t('pt.ws2Send'), v: n(s?.send), tone: 'g' },
    { k: 'SendTo', z: t('pt.ws2SendTo'), v: n(s?.sendTo), tone: 'g' },
    { k: 'Recv', z: t('pt.ws2Recv'), v: n(s?.recv), tone: 'g' },
    { k: 'RecvFrom', z: t('pt.ws2RecvFrom'), v: n(s?.recvFrom), tone: 'g' },
    { k: 'WSASend', z: t('pt.wsaSend'), v: n(s?.wsaSend), tone: 'g' },
    { k: 'WSASendTo', z: t('pt.wsaSendTo'), v: n(s?.wsaSendTo), tone: 'g' },

    { k: 'WSARecv', z: t('pt.wsaRecv'), v: n(s?.wsaRecv), tone: 'g' },
    { k: 'WSARecvFrom', z: t('pt.wsaRecvFrom'), v: n(s?.wsaRecvFrom), tone: 'g' },
    //目标报上来的滤镜执行次数（引擎在那边跑）
    { k: 'Filter', z: t('inject.st.filterExec'), v: n(s?.filterExecute), tone: 'a' },
    //被「过滤设置」挡掉、没进列表的条数（外壳这边 FlushToFeed 数的）
    { k: 'Filtered', z: t('inject.st.filtered'), v: n(s?.filterPacket), tone: 'a' },
    { k: 'Queue', z: t('inject.st.queue'), v: n(s?.queue), tone: (s?.queue || 0) > 5000 ? 'a' : 'g' },
    {
      k: 'Bytes',
      z: '↑ ' + bytes(s?.totalSend) + ' · ↓ ' + bytes(s?.totalRecv),
      v: bytes((s?.totalSend || 0) + (s?.totalRecv || 0)),
      tone: 'c',
    },
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

watch(() => rows.value.length, (n) => { if (n === 0) pick.clear() })

onMounted(async () => {
  try {
    prefs.value = await call<Prefs>('getPrefs')
  } catch (e) {
    console.error('[inject] 取界面偏好失败', e)
  }

  //列显隐要在第一帧之前拿到，否则列表先按「全显示」画一遍再跳
  if (!listSetting.value) {
    //注入模式读注入那一套列显隐（C# 侧是 PacketConfig.List.IsShow_*）
    try { listSetting.value = await call('getListSetting', { mode: 'inject' }) } catch { /* 桥没接上 */ }
  }

  statsTimer = window.setInterval(async () => {
    try { stats.value = await call<InjectStats>('getInjectStats') } catch { /* 窗口关闭中 */ }
  }, 500)
})

onBeforeUnmount(() => {
  //推送的订阅在 InjectView 上，这一屏切走时不该断掉；这个轮询是自己的，要收
  window.clearInterval(statsTimer)
})

function onSelect(anyRow: PacketListRow, ev: MouseEvent, index: number): void {
  //mode="inject" 时表里的行一定是 PacketRow
  const row = anyRow as PacketRow
  pick.onRowClick(row, ev, index)
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

  【游标留在前端】C# 侧不存 Search_Index：搜索是「从第 N 行往后找第一条」这一件事，
  游标是谁在翻页谁的状态。
  【命中之后做三件事】选中那一行、把它滚进视野、再把命中的那段字节圈出来
  —— 第三件靠 C# 一并返回的 Offset / Length，不像 WinForms 那样让十六进制控件自己再找一次。
*/
const q = ref('')
const qHex = ref(false)
const searching = ref(false)
/** 上一次命中的行在 C# 列表里的下标；「查找下一个」从它 + 1 接着找。−1 = 还没找过 */
const searchAt = ref(-1)
/** 命中的那段字节，交给十六进制面板圈出来 */
const searchHit = ref<{ offset: number; length: number } | null>(null)

interface SearchResult { Found: boolean; Id: number; Index: number; Offset: number; Length: number; Error: string | null }

async function findNext(fromHead = false): Promise<void> {
  const pattern = q.value.trim()
  if (!pattern || searching.value) return

  searching.value = true

  try {
    const from = fromHead ? 0 : searchAt.value + 1
    let r = await call<SearchResult>('searchPacketList', { pattern, isHex: qHex.value, from })

    if (r?.Error) {
      pushToast('error', t('sp.badRegex') + ' · ' + r.Error)
      return
    }

    //没找到而且不是从头找的 —— 回到开头再来一圈（转完还找不到才算真没有）
    if (!r?.Found && from > 0) {
      r = await call<SearchResult>('searchPacketList', { pattern, isHex: qHex.value, from: 0 })
      if (r?.Found) pushToast('info', t('sp.wrapped'))
    }

    if (!r?.Found) {
      searchAt.value = -1
      searchHit.value = null
      pushToast('warning', t('sp.noMatch'))
      return
    }

    searchAt.value = r.Index

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
    searching.value = false
  }
}

//改了条件就把游标退回开头；上一次的高亮也一并丢掉，它标的是上一个条件命中的那一段
watch([q, qHex], () => {
  searchAt.value = -1
  searchHit.value = null
})

function clearSearch(): void {
  q.value = ''
  searchAt.value = -1
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
    if (!ids.length) { pushToast('warning', t('lst.needPick')); return }

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
      return

    case 'deselect':
      pick.clear()
      return

    //导出不要求先选：ids 为空时 C# 侧会导整张表
    case 'excel':
      try { await call('exportPacketExcel', { ids }) } catch (e) { console.error('[pm] 导出失败', e) }
      return
  }

  if (!ids.length) {
    pushToast('warning', t('lst.needPick'))
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
  <div class="page">
    <InjectBar
      :busy="props.busy"
      :rate="stat.rate"
      :rows="rows.length"
      @toggle-hook="emit('toggleHook')"
      @clear="emit('clear')"
      @detach="emit('detach')"
      @open-setting="emit('openSetting', $event)"
    />

    <div class="stats">
      <div v-for="c in cells" :key="c.k" class="st-c">
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
          查找封包。Enter = 查找下一个，Esc = 清空；右边两个按钮是「文本 / 十六进制」与「从头查找」。
          没有独立的「向下搜索」单选 —— 主按钮本身就是向下找，找到末尾自己回头再来一圈。
        -->
        <span class="search" :class="{ busy: searching }">
          <svg class="ico" viewBox="0 0 24 24"><circle cx="11" cy="11" r="7" /><path d="M20 20l-4-4" /></svg>
          <input
            v-model="q"
            class="sinp"
            spellcheck="false"
            :placeholder="t('proxy.search')"
            @keydown.enter.prevent="findNext(false)"
            @keydown.esc.prevent="clearSearch"
          >
          <button v-if="q" class="sx" :title="t('sp.clear')" @click="clearSearch">×</button>
        </span>

        <button class="chk" :class="{ on: qHex }" :title="t('sp.modeHint')" @click="qHex = !qHex">
          <i />{{ qHex ? t('sp.hex') : t('sp.text') }}
        </button>
        <button class="tb" :disabled="!q.trim() || searching" @click="findNext(false)">
          {{ searching ? t('sp.searching') : t('sp.next') }}
        </button>
        <button class="tb" :disabled="!q.trim() || searching" :title="t('sp.fromHead')" @click="findNext(true)">
          {{ t('sp.fromHead') }}
        </button>

        <button class="chk" :class="{ on: follow }" @click="follow = !follow"><i />{{ t('proxy.autoRoll') }}</button>
        <!-- 只读显示；改在「设置 ▾ → 列表设置」里，点一下直接跳过去 -->
        <button class="chk" :class="{ on: autoClear }" :title="t('set.list')" @click="emit('openSetting', 'list')">
          <i />{{ t('proxy.autoClear') }}
        </button>
        <button class="num" :title="t('set.list')" @click="emit('openSetting', 'list')">{{ clearAt.toLocaleString() }}</button>
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
        @menu="onMenu"
      />

      <ContextMenu :at="menuAt" :items="menuItems" @pick="onMenuPick" @close="menuAt = null" />

      <!-- 封包编辑：保存后 C# 按行 UI.Feed.Update 推回来，这里不用做别的 -->
      <PacketEdit :target="editTarget" @close="editTarget = null" />
      <PacketModification :id="modifyId" list="packet" @close="modifyId = null" />
    </div>

    <div class="lower">
      <QuickPanel />
      <!-- packetType 只用来决定默认按文本还是十六进制看（HTTP 那四类默认文本）-->
      <HexPanel :id="selectedId" list="packet" :packet-type="selected?.Type ?? null" :highlight="searchHit" />
    </div>
  </div>
</template>

<style scoped>
.page {
  flex: 1;
  min-width: 0;
  min-height: 0;
  display: flex;
  flex-direction: column;
  gap: 8px;
  padding: 10px 12px 12px;
}

/* 统计：7 列 × 2 行，1px 发丝线分隔 —— 与代理数据页那块逐条对齐 */
.stats {
  flex: none;
  display: grid;
  grid-template-columns: repeat(7, 1fr);
  gap: 1px;
  background: var(--border);
  border: 1px solid var(--border);
}

.st-c { background: var(--card); padding: 6px 11px; min-width: 0; }

.st-c .k {
  font-family: var(--share);
  font-size: 9px;
  letter-spacing: .14em;
  text-transform: uppercase;
  color: var(--muted);
}

.st-c .v {
  font-family: var(--orbit);
  font-weight: 800;
  font-size: 17px;
  line-height: 1.25;
  color: var(--green);
  /* 数字每 500ms 变一次，等宽才不会让整格宽度抖动 */
  font-variant-numeric: tabular-nums;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.st-c .v.c { color: var(--cyan); }
.st-c .v.a { color: var(--amber); }

.st-c .z {
  font-size: 10.5px;
  color: var(--muted);
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.lostbar {
  flex: none;
  padding: 7px 14px 5px;
  background: rgb(255 51 102 / 8%);
  border: 1px solid rgb(255 51 102 / 30%);
  color: var(--danger);
  font-size: 12px;
}

/* 与代理数据页同一套比例：表占上面 3 份、下半部 1 份，各自有地板 */
.grid {
  flex: 3 1 0;
  min-height: 220px;
  border: 1px solid var(--border);
  background: var(--card);
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

.list { flex: 1; min-height: 0; border: 0; }

/* 下半部：快捷面板 + 十六进制，与代理数据页同一套栅格与比例 */
.lower {
  flex: 1 1 0;
  min-height: 180px;
  display: grid;
  grid-template-columns: minmax(300px, 22%) 1fr;
  gap: 8px;
}
</style>
