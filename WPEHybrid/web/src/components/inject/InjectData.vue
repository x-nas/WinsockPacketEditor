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
import type { PacketListRow, PacketRow, Prefs } from '../../bridge/types'
import { injectFeed } from '../../stores/packets'
import { listSetting } from '../../stores/runtime'
import { useRowPick } from '../../usePick'
import { status } from '../../stores/inject'
import { t } from '../../i18n'
import PacketList from '../PacketList.vue'
import HexPanel from '../HexPanel.vue'
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
    try { listSetting.value = await call('getListSetting') } catch { /* 桥没接上 */ }
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
}

/** 列表被清空（自动清理 / 手动清空）之后收拾右边的面板。 */
function onCleared(): void {
  selected.value = null
  selectedId.value = null
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
      <PacketList
        ref="listRef"
        class="list"
        mode="inject"
        :prefs="prefs"
        :selected-id="selectedId"
        :picked="picked"
        :follow="follow"
        @select="onSelect"
      />
    </div>

    <div class="lower">
      <!-- packetType 只用来决定默认按文本还是十六进制看（HTTP 那四类默认文本）-->
      <HexPanel :id="selectedId" list="packet" :packet-type="selected?.Type ?? null" />
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

.lower { flex: 1 1 0; min-height: 180px; display: flex; min-width: 0; }

/* 十六进制面板要占满这一条 —— 代理那边是两列栅格自然拉伸，这里只有一件东西 */
.lower > * { flex: 1; min-width: 0; }
</style>
