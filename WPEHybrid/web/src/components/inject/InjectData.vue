<script setup lang="ts">
/*
  「封包列表」页 —— 对应 WinForms 的 Controls/PacketList，注入模式的主屏。

  与代理模式的 ProxyData 是同一套结构（状态条 + 统计 + 表 + 下半部），
  数据源换成 FeedList.Packet 那一路（PacketInfo，另一套 Id 序列）。

  【数据来自另一个进程】钩子与滤镜引擎留在目标里，封包经命名管道过来，
  由 ShellLink.Ingest 还原成 PacketInfo 进 cqPacketInfo；进队之后的下游
  （FlushToFeed → PacketRow → 这里的表）与代理模式完全共用。
*/
import { onBeforeUnmount, onMounted, ref, watch } from 'vue'
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
})

onBeforeUnmount(() => { /* 推送的订阅在 InjectView 上，这一屏切走时不该断掉 */ })

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
