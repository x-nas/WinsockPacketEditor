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
import { computed, onMounted, onBeforeUnmount, ref, watch } from 'vue'
import { call, on } from '../bridge'
import { FeedList } from '../bridge/types'
import { injectFeed } from '../stores/packets'
import { attachListFeed } from '../stores/lists'
import { loadCountryTable } from '../flags'
import { gotoPage } from '../stores/runtime'
import { t } from '../i18n'
import { pushToast } from '../stores/toast'
import { refresh as refreshStatus, setStatus, status, type InjectStatus } from '../stores/inject'
import { useSort } from '../useSort'
import ProxySide from './proxy/ProxySide.vue'
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

const procs = ref<ProcRow[]>([])
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
  return procs.value.filter(
    (p) => p.ProcessName.toLowerCase().includes(q) || String(p.ProcessID).includes(q),
  )
})

const sorter = useSort<ProcRow>(filteredProcs, {
  ProcessName: (p) => p.ProcessName,
  ProcessID: (p) => p.ProcessID,
})

const shownProcs = sorter.sorted

const picking = computed(() => status.value.state === 'idle')

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

  offState = on('inject:state', (d: InjectStatus) => {
    setStatus(d)

    if (d.state === 'disconnected') {
      //目标没了。<b>数据一条都不清</b> —— 用户还要看、还要导出（方案 3.5）
      pushToast('warning', t('inject.lost'))
    }
  })

  try {
    await call('enterInjectMode')
    await refreshStatus()
  } catch {
    /* 桥没接上（浏览器里跑探针页），下面的按钮点了会各自报错 */
  }

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
  offState?.()
  offFeed?.()
  offList?.()
  if (poll) window.clearInterval(poll)
})

async function refreshProcs(): Promise<void> {
  loadingProcs.value = true
  try {
    procs.value = await call<ProcRow[]>('getInjectProcessList')
  } catch {
    procs.value = []
  } finally {
    loadingProcs.value = false
  }
}

async function attachTo(pid: number): Promise<void> {
  if (busy.value) return
  busy.value = true

  try {
    const r = await call<any>('injectAttach', { pid })
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

async function launchAndAttach(): Promise<void> {
  if (busy.value) return

  const p = await call<{ path: string }>('pickTargetExe')
  if (!p?.path) return

  busy.value = true
  try {
    const r = await call<any>('injectAttach', { pid: -1, path: p.path })
    if (!r?.ok) { pushToast('error', r?.error || t('inject.failed')); return }
    setStatus(r)
    page.value = 'packet'
    pushToast('success', t('inject.launched'))
  } catch (e: any) {
    pushToast('error', String(e?.message || e))
  } finally {
    busy.value = false
  }
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

async function detach(): Promise<void> {
  if (busy.value) return
  busy.value = true

  try {
    setStatus(await call<InjectStatus>('injectDetach'))
    dataRef.value?.onCleared()
    //回到选目标屏时把当前页退回主屏，下次附加不会停在上次翻到的那一页
    page.value = 'packet'
  } finally {
    busy.value = false
  }
}

async function clearList(): Promise<void> {
  try { await call('clearPackets', { list: FeedList.Packet }) } catch { /* 桥没接上 */ }
  injectFeed.clearLocal()
  dataRef.value?.onCleared()
}

/** 这一页做完没有。占位块靠它决定要不要出现，与代理模式同一套口径。 */
function isReady(k: PageKey): boolean {
  return INJECT_PAGES.find((x) => x.key === k)?.ready === true
}

function titleOf(k: PageKey): string {
  const p = INJECT_PAGES.find((x) => x.key === k)
  return p ? t(p.label) : ''
}
</script>

<template>
  <div class="inject">
    <!-- ══════════ ① 还没附加：选目标 ══════════ -->
    <div v-if="picking" class="pickscr list-page">
      <div class="ptitle">
        <div class="eyebrow">WPE_INJECT // SELECT_TARGET</div>
        <h2>{{ t('inject.pick.title') }}</h2>
        <p class="lede">{{ t('inject.pick.lede') }}</p>
      </div>

      <div class="tools">
        <input v-model="search" class="search" :placeholder="t('inject.pick.search')" />
        <button class="btn" :disabled="loadingProcs" @click="refreshProcs">
          {{ loadingProcs ? t('inject.pick.loading') : t('inject.pick.refresh') }}
        </button>
        <button class="btn primary" :disabled="busy" @click="launchAndAttach">
          {{ t('inject.pick.launch') }}
        </button>
      </div>

      <div class="ptable">
        <div class="head">
          <span class="so" :class="{ on: sorter.active('ProcessName') }" @click="sorter.toggle('ProcessName')">
            {{ t('inject.col.name') }}<span class="ar">{{ sorter.mark('ProcessName') }}</span>
          </span>
          <span class="so" :class="{ on: sorter.active('ProcessID') }" @click="sorter.toggle('ProcessID')">
            {{ t('inject.col.pid') }}<span class="ar">{{ sorter.mark('ProcessID') }}</span>
          </span>
          <span>{{ t('inject.col.path') }}</span>
          <span></span>
        </div>

        <div class="pbody">
          <div
            v-for="p in shownProcs"
            :key="p.ProcessID"
            class="row"
            :class="{ sel: selectedPid === p.ProcessID }"
            @click="selectedPid = p.ProcessID"
            @dblclick="attachTo(p.ProcessID)"
          >
            <span>{{ p.ProcessName }}</span>
            <span class="num">{{ p.ProcessID }}</span>
            <span class="path" :title="p.ProcessPath">{{ p.ProcessPath || '—' }}</span>
            <span>
              <button class="mini" :disabled="busy" @click.stop="attachTo(p.ProcessID)">
                {{ t('inject.pick.attach') }}
              </button>
            </span>
          </div>

          <div v-if="!shownProcs.length" class="empty">{{ t('inject.pick.none') }}</div>
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
        @detach="detach"
        @open-setting="setting = $event"
      />

      <SystemLog v-show="page === 'log'" />

      <!-- 其余各页没有要保住的运行态（列表都在 stores/lists，工具页的状态在 stores/tools）-->
      <FilterList v-if="page === 'filter'" />
      <SendList v-if="page === 'send'" />
      <RobotList v-if="page === 'robot'" />
      <WareHouseList v-if="page === 'warehouse'" />
      <StatData v-if="page === 'stat'" />
      <TextCompare v-if="page === 'diff'" />
      <XorCalc v-if="page === 'xor'" />
      <Transcode v-if="page === 'transcode'" />
      <ExtractData v-if="page === 'extract'" />

      <!-- 「还没做」那一屏：条件读 pages.ts 的 ready，不写成一串 page !== 'x' -->
      <div v-if="!isReady(page)" class="soon">
        <div class="eyebrow"><span class="dash" /><span class="lbl">{{ titleOf(page) }}</span></div>
        <p>{{ t('proxy.notReady') }}</p>
      </div>
    </div>

    <!-- 7 个设置弹窗。挂在外壳这一层，切到哪一页都还开着 -->
    <LeachSetting :open="setting === 'leach'" mode="inject" @update:open="setting = $event ? 'leach' : null" />
    <HookSetting :open="setting === 'hook'" @update:open="setting = $event ? 'hook' : null" />
    <ListSetting :open="setting === 'list'" @update:open="setting = $event ? 'list' : null" />
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
.pickscr {
  display: flex;
  flex-direction: column;
  min-height: 0;
  height: 100%;
  padding: 22px 26px 0;
}

.ptitle { margin-bottom: 16px; }
.ptitle h2 { margin: 6px 0 4px; font-family: var(--orbit, inherit); font-size: 20px; color: var(--gray); }
.ptitle .lede { margin: 0; color: var(--muted); font-size: 12.5px; }

.eyebrow {
  font-family: var(--share);
  font-size: var(--th-size);
  letter-spacing: 0.14em;
  color: var(--green);
  text-transform: uppercase;
}

.ptable {
  display: flex;
  flex-direction: column;
  min-height: 0;
  flex: 1;
  border: 1px solid var(--border);
  background: var(--panel-bg, transparent);
}

.head,
.row {
  display: grid;
  grid-template-columns: 200px 92px 1fr 96px;
  align-items: center;
  column-gap: 10px;
  padding: 0 14px;
}

.head {
  height: var(--th-h);
  border-bottom: 1px solid var(--border);
  font-family: var(--share);
  font-size: var(--th-size);
  letter-spacing: 0.14em;
  text-transform: uppercase;
  color: #a8b2c0;
}

/* 表头文字比盒中心偏上约 2px，往下补 4px 的上内边距压回正中 */
.head > span { font-family: inherit; font-size: inherit; color: inherit; padding-top: 4px; }

.pbody { overflow: auto; min-height: 0; flex: 1; }

.row {
  height: 30px;
  border-bottom: 1px solid rgba(42, 42, 58, 0.5);
  font-size: 12px;
  cursor: default;
}

.row:hover { background: rgba(0, 255, 136, 0.05); }
.row.sel { background: rgba(0, 212, 255, 0.1); }

.row .num { text-align: right; font-family: var(--mono); color: var(--muted); }

.row .path {
  color: var(--muted);
  font-family: var(--mono);
  font-size: 11px;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.empty { padding: 24px; text-align: center; color: var(--muted); font-size: 12.5px; }

/* ── 工作屏：侧栏 + 内容，与代理模式同一套栅格 ───────────── */
.workscr {
  position: relative;
  z-index: 10;
  flex: 1;
  min-height: 0;
  display: grid;
  grid-template-columns: 196px 1fr;
}

.soon {
  min-width: 0;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 12px;
  color: var(--muted);
}

.soon .eyebrow { display: flex; align-items: center; gap: 10px; }
.soon .eyebrow .dash { width: 32px; height: 1px; background: var(--cyan); box-shadow: 0 0 6px var(--cyan); }

.soon .eyebrow .lbl {
  font-family: var(--share);
  font-size: 11px;
  letter-spacing: .3em;
  text-transform: uppercase;
  color: var(--cyan);
}

.soon p { font-size: 12.5px; margin: 0; }
</style>
