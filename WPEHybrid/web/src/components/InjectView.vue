<script setup lang="ts">
/*
  注入模式（B-IPC 阶段 1）。对应 WinForms 的 Forms/InjectModeForm + Controls/ProcessList + PacketList。

  【与代理模式最大的不同】数据来自<b>另一个进程</b>。
  钩子与滤镜引擎留在目标里（滤镜必须在目标的收发线程上同步给出答案，
  每包一次跨进程往返会把目标的每一次 send/recv 都拖慢），
  封包经命名管道过来，由 C# 侧的 ShellLink 还原成 PacketInfo 进 cqPacketInfo。
  进队之后的下游与代理模式<b>完全共用</b>：FlushToFeed → PacketRow → 这里的表。

  所以这一屏只有两个状态：
    ① 还没附加 —— 选目标（进程列表 / 启动并注入）
    ② 已附加   —— 状态条 + 封包列表 + 十六进制面板
*/
import { computed, onMounted, onBeforeUnmount, ref } from 'vue'
import { call, on } from '../bridge'
import { FeedList, type PacketListRow, type PacketRow, type Prefs } from '../bridge/types'
import { injectFeed } from '../stores/packets'
import { t } from '../i18n'
import { pushToast } from '../stores/toast'
import PacketList from './PacketList.vue'
import HexPanel from './HexPanel.vue'
import HookSetting from './proxy/HookSetting.vue'
import { useRowPick } from '../usePick'
import { useSort } from '../useSort'
import { injectHooked, injectTarget } from '../stores/runtime'

interface ProcRow {
  ProcessName: string
  ProcessID: number
  ModuleName: string
  ProcessPath: string
}

interface Status {
  state: 'idle' | 'attaching' | 'attached' | 'disconnected'
  pid: number
  name: string
  is64: boolean
  hooked: boolean
  dropped: number
  ws1: boolean
  ws2: boolean
  msws: boolean
}

const status = ref<Status>({
  state: 'idle', pid: 0, name: '', is64: false,
  hooked: false, dropped: 0, ws1: false, ws2: false, msws: false,
})

/**
 * 每次拿到新状态就同步一份给底部状态栏 ——
 * 那一栏在 App.vue 里，不是这个组件的子孙，跨视图的运行态走 stores/runtime。
 */
function setStatus(s: Status): void {
  status.value = s
  injectHooked.value = !!s.hooked
  injectTarget.value = s.pid > 0 ? s.name + ' #' + s.pid : ''
}

const prefs = ref<Prefs | null>(null)
const procs = ref<ProcRow[]>([])
const loadingProcs = ref(false)
const search = ref('')
const selectedPid = ref<number | null>(null)
const busy = ref(false)
const showHook = ref(false)

/** 选中的那一行封包（十六进制面板看的是它）。 */
const selectedId = ref<number | null>(null)
const hexBytes = ref<{ packet: Uint8Array | null; raw: Uint8Array | null } | null>(null)

const rows = injectFeed.rows
const stat = injectFeed.stat

const listRef = ref<InstanceType<typeof PacketList> | null>(null)
const follow = ref(true)

/*
  ⚠️ 必须传 autoPrune: false。共用实现默认挂一个 watch(rows) 剔除已消失的 Id，
  而这一屏的 rows 每帧都 triggerRef —— 挂上去就是每秒跑 60 次。
  这里改成只在列表被清空时 clear() 一次（见 clearList）。
*/
const pick = useRowPick<PacketRow, number>(rows, (r) => r.Id, { autoPrune: false })
const picked = pick.picked

/*
  进程列表按名字搜；排序走全项目那一份 useSort（升 → 降 → 回到原始顺序三档）。
  原始顺序是 GetProcessList 里按进程名排好的，回得去才有意义。
*/
const filteredProcs = computed(() => {
  const q = search.value.trim().toLowerCase()
  if (!q) return procs.value
  return procs.value.filter(
    (p) => p.ProcessName.toLowerCase().includes(q) || String(p.ProcessID).includes(q),
  )
})

/*
  排序三档：升 → 降 → 回到原始顺序。原始顺序是 GetProcessList 里按进程名排好的，
  回得去才有意义（用户按 PID 排过一轮之后还想按名字找）。
*/
const sorter = useSort<ProcRow>(filteredProcs, {
  ProcessName: (p) => p.ProcessName,
  ProcessID: (p) => p.ProcessID,
})

const shownProcs = sorter.sorted

const disconnected = computed(() => status.value.state === 'disconnected')
const picking = computed(() => status.value.state === 'idle')

let offState: (() => void) | null = null
let offFeed: (() => void) | null = null
let poll = 0

onMounted(async () => {
  offFeed = injectFeed.attach()

  offState = on('inject:state', (d: Status) => {
    setStatus(d)

    if (d.state === 'disconnected') {
      //目标没了。<b>数据一条都不清</b> —— 用户还要看、还要导出（方案 3.5）
      pushToast('warning', t('inject.lost'))
    }
  })

  try {
    await call('enterInjectMode')
    prefs.value = await call<Prefs>('getPrefs')
    setStatus(await call<Status>('getInjectStatus'))
  } catch {
    /* 桥没接上（浏览器里跑探针页），下面的按钮点了会各自报错 */
  }

  await refreshProcs()

  //丢包数与钩子状态只在事件里推，1 秒兜一次底就够（这几个数变得很慢）
  poll = window.setInterval(async () => {
    if (status.value.state === 'idle') return
    try { setStatus(await call<Status>('getInjectStatus')) } catch { /* 断了 */ }
  }, 1000)
})

onBeforeUnmount(() => {
  offState?.()
  offFeed?.()
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
    setStatus(await call<Status>('injectDetach'))
    selectedId.value = null
    hexBytes.value = null
  } finally {
    busy.value = false
  }
}

async function onSelect(anyRow: PacketListRow, ev: MouseEvent, index: number): Promise<void> {
  //mode="inject" 时表里的行一定是 PacketRow —— 收窄一下，好用它的字段
  const row = anyRow as PacketRow
  pick.onRowClick(row, ev, index)
  selectedId.value = row.Id

  try {
    //⚠️ 注入模式的列表是 FeedList.Packet，不是 Proxy —— 两份表各有独立的 Id 序列
    const d = await call<any>('getPacketDetail', { id: row.Id, list: FeedList.Packet })
    hexBytes.value = d ? { packet: b64(d.packet), raw: b64(d.raw) } : null
  } catch {
    hexBytes.value = null
  }
}

function b64(s: string | null): Uint8Array | null {
  if (!s) return null
  const bin = atob(s)
  const out = new Uint8Array(bin.length)
  for (let i = 0; i < bin.length; i++) out[i] = bin.charCodeAt(i)
  return out
}

async function clearList(): Promise<void> {
  try { await call('clearPackets', { list: FeedList.Packet }) } catch { /* 桥没接上 */ }
  injectFeed.clearLocal()
  selectedId.value = null
  hexBytes.value = null
  pick.clear()
}

/** 目标用的是哪几套 WinSock —— 目标自己探的，这里只显示。 */
const wsText = computed(() => {
  const s = status.value
  const on: string[] = []
  if (s.ws1) on.push('1.1')
  if (s.ws2) on.push('2.0')
  if (s.msws) on.push('MS')
  return on.length ? on.join(' / ') : '—'
})
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

    <!-- ══════════ ② 已附加：状态条 + 封包列表 ══════════ -->
    <div v-else class="workscr">
      <div class="runbar" :class="{ off: disconnected }">
        <button
          class="tb"
          :class="status.hooked ? 'stop' : 'go'"
          :disabled="busy || disconnected"
          @click="toggleHook"
        >
          {{ status.hooked ? t('inject.stopHook') : t('inject.startHook') }}
        </button>

        <div class="meta">
          <span class="k">{{ t('inject.target') }}</span>
          <b>{{ status.name }}</b>
          <span class="dim">#{{ status.pid }} · {{ status.is64 ? 'x64' : 'x86' }}</span>
        </div>

        <div class="meta">
          <span class="k">WinSock</span>
          <b>{{ wsText }}</b>
        </div>

        <div class="meta">
          <span class="k">{{ t('inject.rate') }}</span>
          <b>{{ stat.rate }}/s</b>
          <span class="dim">{{ rows.length }} {{ t('inject.rows') }}</span>
        </div>

        <!--
          丢弃计数：环满时目标丢的是最旧的包。
          无声丢包比阻塞更糟，所以只要不是 0 就必须显示出来。
        -->
        <div v-if="status.dropped > 0" class="meta warn">
          <span class="k">{{ t('inject.dropped') }}</span>
          <b>{{ status.dropped }}</b>
        </div>

        <div class="spacer" />

        <div class="state" :class="status.state">
          {{ disconnected ? t('inject.state.lost') : t('inject.state.ok') }}
        </div>

        <button class="tb" @click="showHook = true">{{ t('inject.hookSetting') }}</button>
        <button class="tb" @click="clearList">{{ t('inject.clear') }}</button>
        <button class="tb" :disabled="busy" @click="detach">{{ t('inject.detach') }}</button>
      </div>

      <!--
        目标没了的横幅。刻意<b>不清任何数据</b>：抓到的封包、配置、界面全部保留，
        用户还能继续看、继续导出 —— 这正是 IPC 改造要解决的第一条风险（崩溃不隔离）。
      -->
      <div v-if="disconnected" class="lostbar">{{ t('inject.lostHint') }}</div>

      <div class="body">
        <PacketList
          ref="listRef"
          mode="inject"
          :prefs="prefs"
          :selected-id="selectedId"
          :picked="picked"
          :follow="follow"
          @select="onSelect"
        />

        <HexPanel
          v-if="selectedId !== null"
          :id="selectedId"
          :bytes="hexBytes?.packet || null"
          :compare="hexBytes?.raw || null"
        />
      </div>
    </div>

    <HookSetting v-model:open="showHook" />
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

/* ── 工作屏 ─────────────────────────────── */
.workscr {
  display: flex;
  flex-direction: column;
  min-height: 0;
  height: 100%;
}

.runbar {
  display: flex;
  align-items: center;
  gap: 16px;
  padding: 8px 14px;
  border-bottom: 1px solid var(--border);
  flex: none;
}

.runbar.off { opacity: 0.72; }

.meta { display: flex; align-items: baseline; gap: 6px; font-size: 12px; }
.meta .k { font-family: var(--share); font-size: var(--label-size); letter-spacing: 0.1em; color: var(--muted); text-transform: uppercase; }
.meta b { color: var(--gray); font-family: var(--mono); }
.meta .dim { color: var(--muted); font-size: 11px; }
.meta.warn b { color: var(--danger); }

.spacer { flex: 1; }

.state {
  font-family: var(--share);
  font-size: var(--label-size);
  letter-spacing: 0.12em;
  padding: 3px 9px 1px;
  border: 1px solid var(--border);
  text-transform: uppercase;
}

.state.attached { color: var(--green); border-color: rgba(0, 255, 136, 0.4); }
.state.disconnected { color: var(--danger); border-color: rgba(255, 51, 102, 0.4); }

.lostbar {
  flex: none;
  padding: 7px 14px 5px;
  background: rgba(255, 51, 102, 0.08);
  border-bottom: 1px solid rgba(255, 51, 102, 0.3);
  color: var(--danger);
  font-size: 12px;
}

.body { display: flex; flex-direction: column; min-height: 0; flex: 1; }

/*
  「开始 / 停止拦截」那个按钮的悬停要把边框与字色一起写上 ——
  通用的 .tb:hover:not(:disabled) 是 (0,3,0)，压得过 .tb.go 的 (0,2,0)，
  只写背景的话点完那一瞬间（鼠标还停在按钮上）绿 / 红按钮会变成青色。
*/
.tb {
  font-family: var(--share);
  font-size: var(--btn-size);
  letter-spacing: 0.1em;
  line-height: 1;
  padding: 9px 13px 7px;
  border: 1px solid var(--border);
  background: transparent;
  color: var(--gray);
  cursor: pointer;
  text-transform: uppercase;
}

.tb:disabled { opacity: 0.4; cursor: default; }
.tb:hover:not(:disabled) { border-color: var(--cyan); color: var(--cyan); }

.tb.go { border-color: rgba(0, 255, 136, 0.5); color: var(--green); }
.tb.go:hover:not(:disabled) { border-color: var(--green); color: var(--green); background: rgba(0, 255, 136, 0.1); }

.tb.stop { border-color: rgba(255, 51, 102, 0.5); color: var(--danger); }
.tb.stop:hover:not(:disabled) { border-color: var(--danger); color: var(--danger); background: rgba(255, 51, 102, 0.1); }
</style>
