<script setup lang="ts">
/*
  机器人列表 —— 对应 WinForms 的 Controls/RobotList。

  一个「机器人」= 一组按顺序执行的指令（发送某条发送 / 延迟 / 循环开始·结束 / 键盘 / 鼠标 /
  设置系统套接字 / 开关）。点「开始执行」后，按<b>列表顺序</b>把已启用的那些依次（或同时）跑一遍，
  执行方式取 Operate.SystemConfig.ListExecute，与发送列表同一个开关。

  【与发送列表几乎同一屏】表结构、右键七项、启停由 C# 说了算、跑的时候只锁按钮不锁表 ——
  全都照 SendList.vue。不同的只有列：没有套接字 / 循环 / 备注，多一列「指令条数」。

  【去掉了「状态」列】WinForms 那一列画徽标，「处理中」的判据是 ExecutionCount > 0 ——
  它其实是「执行过」不是「正在执行」；启用与执行次数这里已经各有一列。与发送列表同一口径。

  【顺序就是数据】RobotList_DoWork 是 foreach 按列表顺序走的，FeedList.Robot 走整表 Replace，
  前端不排序不筛选，右键那四个移动动作才有意义。

  【编辑】双击行 / 点那支笔开 RobotEdit（与 QuickPanel 机器人页签的双击同一个弹窗）。
*/
import { computed, onBeforeUnmount, onMounted, ref } from 'vue'
import { call, on } from '../../bridge'
import { FeedList, ListAction, type RobotRow } from '../../bridge/types'
import { t } from '../../i18n'
import { useList } from '../../stores/lists'
import { useRowPick } from '../../usePick'
import { pushToast } from '../../stores/toast'
import ContextMenu from '../ContextMenu.vue'
import RobotEdit from './RobotEdit.vue'
import { ICON, type MenuItem } from '../menu'

const rows = useList<RobotRow>(FeedList.Robot)

/* ── 虚拟滚动（与各列表同一套；ROW_H 必须与 .row 的 height 一致）── */
const ROW_H = 34
const OVERSCAN = 8

const scroller = ref<HTMLElement | null>(null)
const scrollTop = ref(0)
const viewH = ref(600)

const total = computed(() => rows.value.length)
const start = computed(() => Math.max(0, Math.floor(scrollTop.value / ROW_H) - OVERSCAN))
const end = computed(() => Math.min(total.value, Math.ceil((scrollTop.value + viewH.value) / ROW_H) + OVERSCAN))
const windowRows = computed(() => rows.value.slice(start.value, end.value))

function onScroll(): void {
  const el = scroller.value
  if (el) scrollTop.value = el.scrollTop
}

/*
  执行方式（Operate.SystemConfig.ListExecute）：0 = 同时执行，1 = 按顺序执行。
  <b>发送列表与机器人列表共用这一个开关</b>，改它的入口在系统设置。

  ⚠️ 它与滤镜的 FilterConfig.Filter.Execute 是<b>两个反着的</b>同名枚举
  （那边 0 = 优先原则、1 = 按顺序），别照抄那边的 `=== 0` 判断。

  ⚠️ 只在挂载时读一次 —— 与滤镜列表同一条口径。这一屏是 v-if 挂的，
  从系统设置改完再切回来会重新挂载、跟着重取；开着这一屏改的话要切一次页才刷新。
*/
const execMode = ref(1)
const isTogether = computed(() => execMode.value === 0)

let ro: ResizeObserver | null = null
let stopRunning: (() => void) | null = null

onMounted(async () => {
  const el = scroller.value
  if (el) {
    viewH.value = el.clientHeight
    ro = new ResizeObserver(() => { viewH.value = el.clientHeight })
    ro.observe(el)
  }

  try {
    const r = await call<{ running: boolean; listExecute: number }>('getRobotMeta')
    running.value = !!r?.running
    execMode.value = r?.listExecute ?? 1
  } catch (e) {
    console.error('[rb] 取元信息失败', e)
  }

  //C# 每秒推一次运行态；worker 会自己跑完，前端不能自己记
  stopRunning = on('robot:running', (d: { running?: boolean }) => {
    running.value = !!d?.running
  })
})

onBeforeUnmount(() => {
  ro?.disconnect()
  stopRunning?.()
})

/* ── 运行态 ─────────────────────────────────────────────────── */

const running = ref(false)

async function toggleRun(): Promise<void> {
  try {
    const r = await call<{ running: boolean }>(running.value ? 'stopRobotList' : 'startRobotList')
    running.value = !!r?.running
  } catch (e) {
    console.error('[rb] 启停失败', e)
  }
}

/* ── 多选 ───────────────────────────────────────────────────── */

const { picked, pickedIds, onRowClick, selectAll, clear } = useRowPick(rows, (r) => r.Id)

/* ── 行为 ───────────────────────────────────────────────────── */

async function toggle(r: RobotRow): Promise<void> {
  try {
    await call('setRobotEnable', { id: r.Id, enable: !r.IsEnable })
  } catch (e) {
    console.error('[rb] 切换启用失败', e)
  }
}

async function add(): Promise<void> {
  try {
    await call('addRobot')
    requestAnimationFrame(() => {
      const el = scroller.value
      if (el) el.scrollTop = el.scrollHeight
    })
  } catch (e) {
    console.error('[rb] 新增失败', e)
  }
}

async function simple(method: string, arg?: Record<string, unknown>): Promise<void> {
  try {
    await call(method, arg)
  } catch (e) {
    console.error('[rb] ' + method + ' 失败', e)
  }
}

/** 编辑弹窗的目标；null = 关着 */
const editing = ref<string | null>(null)

function onRowDblClick(e: MouseEvent, r: RobotRow): void {
  if ((e.target as HTMLElement | null)?.closest('button')) return
  if (running.value) return
  editing.value = r.Id
}

/* ── 右键菜单 ───────────────────────────────────────────────── */

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
    { id: 'export', label: t('lst.export') + tag, icon: ICON.save },
    { id: 'copy', label: t('lst.copy') + tag, icon: ICON.copy },
    { divider: true },
    { id: 'delete', label: t('lst.delete') + tag, icon: ICON.del, danger: true },
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
  copy: ListAction.Copy,
  export: ListAction.Export,
  delete: ListAction.Delete,
}

async function onMenuPick(id: string): Promise<void> {
  if (id === 'selectAll') { selectAll(); return }
  if (id === 'deselect') { clear(); return }

  if (picked.value.size === 0) {
    pushToast('warning', t('lst.needPick'))
    return
  }

  const action = ACTION_OF[id]
  if (action === undefined) return

  try {
    await call('robotListAction', { action, ids: pickedIds.value })
  } catch (e) {
    console.error('[rb] 列表操作失败', e)
  }
}
</script>

<template>
  <div class="page list-page">
    <div class="bar">
      <!-- 启停单独放最左边并用竖线隔开：它会真的去操作键盘鼠标 / 发封包，与增删改查不是一类 -->
      <button class="btn run" :class="{ on: running }" :disabled="!rows.length" @click="toggleRun">
        <svg v-if="running" class="ico" viewBox="0 0 24 24"><rect x="6" y="6" width="12" height="12" /></svg>
        <svg v-else class="ico" viewBox="0 0 24 24"><path d="M7 4l13 8-13 8z" /></svg>
        {{ running ? t('rb.stop') : t('rb.start') }}
      </button>

      <span class="sep" />

      <!-- 跑的时候只锁会改列表的动作，表还要能滚、能看计数在涨 -->
      <button class="btn primary" :disabled="running" @click="add">{{ t('rb.add') }}</button>
      <button class="btn" :disabled="!rows.length || running" @click="simple('setAllRobotEnable', { enable: true })">{{ t('rb.enableAll') }}</button>
      <button class="btn" :disabled="!rows.length || running" @click="simple('setAllRobotEnable', { enable: false })">{{ t('rb.disableAll') }}</button>
      <button class="btn" :disabled="!rows.length || running" @click="simple('resetRobotCount')">{{ t('rb.resetCount') }}</button>

      <span class="grow" />

      <button class="btn" :disabled="running" @click="simple('importRobots')">{{ t('rb.import') }}</button>
      <button class="btn" :disabled="!rows.length" @click="simple('exportRobots')">{{ t('rb.export') }}</button>
      <button class="btn danger" :disabled="!rows.length || running" @click="simple('clearRobots')">{{ t('rb.clearAll') }}</button>
    </div>

    <!--
      顺序即执行顺序 —— 但「顺序意味着什么」取决于执行方式，所以模式是<b>读出来的实际值</b>，
      不是写死的一句通用话（与滤镜列表同一条口径）。样式在 style.css 的 .list-page .ordbar。
    -->
    <div v-if="rows.length > 1" class="ordbar" :class="{ alt: isTogether }">
      <span class="mk">{{ isTogether ? t('set.exec.together') : t('set.exec.sequence') }}</span>
      <span class="tx">{{ isTogether ? t('rb.mode.togetherHint') : t('rb.mode.sequenceHint') }}</span>
      <span class="note">{{ t('lst.mode.note') }}</span>
    </div>

    <div ref="scroller" class="body" @scroll.passive="onScroll">
      <div class="head">
        <span class="no">{{ t('col.id') }}</span>
        <span class="ck">{{ t('col.enable') }}</span>
        <span class="name">{{ t('col.robotName') }}</span>
        <span class="cnt">{{ t('col.execCount') }}</span>
        <span class="inst">{{ t('col.instructions') }}</span>
        <span class="ops">{{ t('col.ops') }}</span>
      </div>

      <div v-if="!rows.length" class="empty">{{ t('rb.empty') }}</div>

      <div v-else class="spacer" :style="{ height: total * ROW_H + 'px' }">
        <div class="win" :style="{ transform: `translateY(${start * ROW_H}px)` }">
          <div
            v-for="(r, i) in windowRows"
            :key="i"
            class="row"
            :class="{ off: !r.IsEnable, sel: picked.has(r.Id) }"
            @click="onRowClick(r, $event, start + i)"
            @contextmenu.prevent="menuAt = { x: $event.clientX, y: $event.clientY }"
            @dblclick="onRowDblClick($event, r)"
          >
            <!-- 序号就是执行序号，所以用全表位次 -->
            <span class="no">{{ start + i + 1 }}</span>

            <span class="ck">
              <button class="chk" :class="{ on: r.IsEnable }" :title="t('col.enable')" @click="toggle(r)"><i /></button>
            </span>

            <span class="name" :title="r.Name">{{ r.Name }}</span>

            <span class="cnt" :class="{ zero: !r.ExecutionCount }">{{ r.ExecutionCount }}</span>

            <!-- 空指令集执行起来什么都不会做，标出来省得点进编辑器才发现 -->
            <span class="inst" :class="{ none: !r.InstructionCount }" :title="r.InstructionCount ? '' : t('rb.noInst')">
              {{ r.InstructionCount }}
            </span>

            <span class="ops">
              <button class="op" :title="t('acct.op.edit')" :disabled="running" @click.stop="editing = r.Id">
                <svg class="ico" viewBox="0 0 24 24"><path d="M4 20h4L20 8l-4-4L4 16z" /></svg>
              </button>
              <button class="op del" :title="t('acct.op.del')" :disabled="running"
                      @click="picked = new Set([r.Id]); onMenuPick('delete')">
                <svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg>
              </button>
            </span>
          </div>
        </div>
      </div>
    </div>

    <ContextMenu :at="menuAt" :items="menuItems" @pick="onMenuPick" @close="menuAt = null" />

    <!-- 机器人编辑：保存即落库并标脏，列表随 FeedList.Robot 的整表推送刷新 -->
    <RobotEdit :id="editing" @close="editing = null" />
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

.sep { width: 1px; height: 16px; background: var(--border); }

/* 启停：与发送列表同一对图标与配色 */
.btn.run { display: inline-flex; align-items: center; gap: 7px; border-color: rgb(var(--green-rgb) / 45%); color: var(--green); }
.btn.run .ico { width: 11px; height: 11px; margin: -1px 0; flex: none; }
.btn.run.on { border-color: rgb(var(--danger-rgb) / 30%); color: var(--danger); }
.btn.run.on .ico { animation: pulse 1.1s ease-in-out infinite; }
.btn.run:hover:not(:disabled) { background: rgb(var(--green-rgb) / 10%); border-color: var(--green); color: var(--green); }
.btn.run.on:hover:not(:disabled) { background: rgb(var(--danger-rgb) / 12%); border-color: var(--danger); color: var(--danger); }

@keyframes pulse { 50% { opacity: .25; } }

@media (prefers-reduced-motion: reduce) {
  .btn.run.on .ico { animation: none; }
}

/* 只留这一屏独有的：列宽、列间距、最小宽度 */
.head,
.row {
  grid-template-columns: 46px 50px minmax(160px, 1fr) 90px 90px 60px;
  gap: 8px;
  min-width: 560px;
}

.head > span { overflow: hidden; text-overflow: ellipsis; }

.head > span,
.row > span { text-align: center; }

.head > span.name,
.row > span.name { text-align: left; }

.row > span { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }

.no { color: var(--dim); font-variant-numeric: tabular-nums; }
.name { color: var(--gray); }

.cnt { font-family: var(--mono); font-variant-numeric: tabular-nums; color: var(--cyan); }
.cnt.zero { color: var(--dim); }

.inst { font-family: var(--mono); font-variant-numeric: tabular-nums; color: var(--green); }
.inst.none { color: var(--danger); }
</style>
