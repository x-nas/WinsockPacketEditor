<script setup lang="ts">
/*
  发送列表 —— 对应 WinForms 的 Controls/SendList。

  一条「发送」= 一组封包（发送集）+ 循环次数 / 间隔 + 用哪个套接字。
  点「开始发送」后，按<b>列表顺序</b>把已启用的那些依次（或同时）执行一遍。

  【⚠️ 顺序就是数据】和滤镜同一个道理：SendList_DoWork 是 for 循环按列表下标走的，
  所以右键那四个移动动作是有意义的。因此这一屏同样：
  ① FeedList.Send 走整表 Replace，不做增量推；
  ② 前端不排序、不重排、不筛选，永远按 C# 推来的顺序原样显示。

  【与 WinForms 的三处不同，都是有理由的】
  1. <b>去掉了「状态」列。</b>那一列画的是徽标，取值只有三种：
     禁用 / 启用 / 处理中，而「处理中」的判据是 `si.ExecutionCount > 0` ——
     它其实是「执行过」不是「正在执行」。这两个信息在这里已经各有一列
     （启用勾选框 + 执行次数），再摆一个名不副实的徽标只会误导。
  2. <b>加了「封包」列。</b>发送集为空的发送执行起来什么都不会发，
     而 WinForms 那张表上完全看不出来 —— 得点进编辑器才知道。
  3. <b>备注不换行。</b>WinForms 那列是 LineBreak = true（自动折行），
     而这里是定高虚拟滚动，行高必须与 ROW_H 逐像素一致，折行会让滚动位置漂移。
     改成单行省略号 + 悬停看全文。
*/
import { computed, onBeforeUnmount, onMounted, ref } from 'vue'
import { call, on } from '../../bridge'
import { FeedList, ListAction, type SendRow } from '../../bridge/types'
import { t } from '../../i18n'
import { useList } from '../../stores/lists'
import { useRowPick } from '../../usePick'
import { pushToast } from '../../stores/toast'
import ContextMenu from '../ContextMenu.vue'
import { ICON, type MenuItem } from '../menu'
import SendEdit from './SendEdit.vue'

const rows = useList<SendRow>(FeedList.Send)

/*
  ── 虚拟滚动 ───────────────────────────────────────────────────
  与封包 / 账号 / 滤镜三张表同一套：定高窗口 + translateY。
  ROW_H <b>必须与 .row 的 height 逐像素一致</b>，对不上会随行数线性漂移且不报错。
*/
const ROW_H = 34
const OVERSCAN = 8

const scroller = ref<HTMLElement | null>(null)
const scrollTop = ref(0)
const viewH = ref(600)

const total = computed(() => rows.value.length)
const start = computed(() => Math.max(0, Math.floor(scrollTop.value / ROW_H) - OVERSCAN))

const end = computed(() =>
  Math.min(total.value, Math.ceil((scrollTop.value + viewH.value) / ROW_H) + OVERSCAN))

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

onMounted(async () => {
  const el = scroller.value
  if (el) {
    viewH.value = el.clientHeight
    ro = new ResizeObserver(() => { viewH.value = el.clientHeight })
    ro.observe(el)
  }

  try {
    const r = await call<{ systemSocket: number; running: boolean; listExecute: number }>('getSendMeta')
    systemSocket.value = r?.systemSocket ?? 0
    running.value = !!r?.running
    execMode.value = r?.listExecute ?? 1
  } catch (e) {
    console.error('[snd] 取元信息失败', e)
  }

  //C# 每秒推一次运行态；worker 会自己跑完，前端不能自己记
  stopRunning = on('send:running', (d: { running?: boolean }) => {
    running.value = !!d?.running
  })
})

onBeforeUnmount(() => {
  ro?.disconnect()
  stopRunning?.()
})

/* ── 运行态 ─────────────────────────────────────────────────── */

/*
  「在跑没在跑」由 C# 说了算：SendList_DoWork 把所有发送执行完就自己结束了，
  没有任何人通知前端。前端若自己记一个 running，会一直显示在发送中。
*/
const running = ref(false)
const systemSocket = ref(0)

let stopRunning: (() => void) | null = null

async function toggleRun(): Promise<void> {
  try {
    const r = await call<{ running: boolean }>(running.value ? 'stopSendList' : 'startSendList')
    running.value = !!r?.running
  } catch (e) {
    console.error('[snd] 启停失败', e)
  }
}

/* ── 多选 ───────────────────────────────────────────────────── */

//单击 / Ctrl / Shift 多选，全项目一份实现，见 usePick.ts
const { picked, pickedIds, onRowClick, selectAll, clear } =
  useRowPick(rows, (r) => r.Id)

/* ── 单元格 ─────────────────────────────────────────────────── */

/** 套接字：用系统套接字时显示它的编号，否则「自定义」——与 WinForms 的两个 CellTag 一致。 */
function socketText(r: SendRow): string {
  return r.UseSystemSocket ? String(systemSocket.value) : t('snd.socketCustom')
}

/* ── 行为 ───────────────────────────────────────────────────── */

async function toggle(r: SendRow): Promise<void> {
  try {
    await call('setSendEnable', { id: r.Id, enable: !r.IsEnable })
  } catch (e) {
    console.error('[snd] 切换启用失败', e)
  }
}

async function add(): Promise<void> {
  try {
    await call('addSend')
    //新发送追加在表尾，滚过去让人看见它
    requestAnimationFrame(() => {
      const el = scroller.value
      if (el) el.scrollTop = el.scrollHeight
    })
  } catch (e) {
    console.error('[snd] 新增失败', e)
  }
}

async function simple(method: string, arg?: Record<string, unknown>): Promise<void> {
  try {
    await call(method, arg)
  } catch (e) {
    console.error('[snd] ' + method + ' 失败', e)
  }
}

/* 双击行、点行内那支笔，两条路都走这一个函数 */
const editing = ref<string | null>(null)

function openEdit(r: SendRow): void {
  editing.value = r.Id
}

function onRowDblClick(e: MouseEvent, r: SendRow): void {
  //落在行内按钮上的双击不算：双击勾选框等于没切却还开了弹窗
  if ((e.target as HTMLElement | null)?.closest('button')) return
  openEdit(r)
}

/* ── 右键菜单 ───────────────────────────────────────────────── */

const menuAt = ref<{ x: number; y: number } | null>(null)

const menuItems = computed<MenuItem[]>(() => {
  //七项全都作用于选中的行，所以七项都带上条数 —— 尤其删除，动手前该看见要删几条
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

  //一处拦住就够了（加菜单项时最容易漏这句）
  if (picked.value.size === 0) {
    pushToast('warning', t('lst.needPick'))
    return
  }

  const action = ACTION_OF[id]
  if (action === undefined) return

  try {
    await call('sendListAction', { action, ids: pickedIds.value })
  } catch (e) {
    console.error('[snd] 列表操作失败', e)
  }
}
</script>

<template>
  <div class="page list-page">
    <div class="bar">
      <!--
        启停单独放在最左边并与其余按钮用竖线隔开：它是这一屏唯一会
        「真的往网络上发东西」的动作，和增删改查不是一类。
      -->
      <button class="btn run" :class="{ on: running }" :disabled="!rows.length" @click="toggleRun">
        <!-- 与代理数据页「开始代理 / 停止」同一对图标：三角 = 开始，方块 = 停止 -->
        <svg v-if="running" class="ico" viewBox="0 0 24 24"><rect x="6" y="6" width="12" height="12" /></svg>
        <svg v-else class="ico" viewBox="0 0 24 24"><path d="M7 4l13 8-13 8z" /></svg>
        {{ running ? t('snd.stop') : t('snd.start') }}
      </button>

      <span class="sep" />

      <!--
        跑的时候锁住会改列表的动作。WinForms 是把整张表 Enabled = false，
        这里只锁按钮 —— 表还要能滚、能看计数在涨，禁掉整张表就看不成了。
      -->
      <button class="btn primary" :disabled="running" @click="add">{{ t('snd.add') }}</button>

      <button class="btn" :disabled="!rows.length || running"
              @click="simple('setAllSendEnable', { enable: true })">
        {{ t('snd.enableAll') }}
      </button>
      <button class="btn" :disabled="!rows.length || running"
              @click="simple('setAllSendEnable', { enable: false })">
        {{ t('snd.disableAll') }}
      </button>
      <button class="btn" :disabled="!rows.length || running" @click="simple('resetSendCount')">
        {{ t('snd.resetCount') }}
      </button>

      <span class="grow" />

      <button class="btn" :disabled="running" @click="simple('importSends')">{{ t('snd.import') }}</button>
      <button class="btn" :disabled="!rows.length" @click="simple('exportSends')">{{ t('snd.export') }}</button>
      <button class="btn danger" :disabled="!rows.length || running" @click="simple('clearSends')">
        {{ t('snd.clearAll') }}
      </button>
    </div>

    <!--
      顺序即执行顺序 —— 但「顺序意味着什么」取决于执行方式，所以模式是<b>读出来的实际值</b>，
      不是写死的一句通用话（与滤镜列表同一条口径）。样式在 style.css 的 .list-page .ordbar。
    -->
    <div v-if="rows.length > 1" class="ordbar" :class="{ alt: isTogether }">
      <span class="mk">{{ isTogether ? t('set.exec.together') : t('set.exec.sequence') }}</span>
      <span class="tx">{{ isTogether ? t('snd.mode.togetherHint') : t('snd.mode.sequenceHint') }}</span>
      <span class="note">{{ t('lst.mode.note') }}</span>
    </div>

    <div ref="scroller" class="body" @scroll.passive="onScroll">
      <div class="head">
        <!-- 表头每格带着与数据行同名的 class，对齐规则按 class 写、增删列不必重编序号 -->
        <span class="no">{{ t('col.id') }}</span>
        <span class="ck">{{ t('col.enable') }}</span>
        <span class="name">{{ t('col.sendName') }}</span>
        <span class="cnt">{{ t('col.execCount') }}</span>
        <span class="cnt">{{ t('col.success') }}</span>
        <span class="cnt">{{ t('col.fail') }}</span>
        <span class="sock">{{ t('col.socket') }}</span>
        <span class="loop">{{ t('col.loop') }}</span>
        <span class="pk">{{ t('col.packets') }}</span>
        <span class="notes">{{ t('col.notes') }}</span>
        <span class="ops">{{ t('col.ops') }}</span>
      </div>

      <div v-if="!rows.length" class="empty">{{ t('snd.empty') }}</div>

      <div v-else class="spacer" :style="{ height: total * ROW_H + 'px' }">
        <div class="win" :style="{ transform: `translateY(${start * ROW_H}px)` }">
          <!-- key 用窗口内下标：滚动时就地改文本、复用这些 DOM 节点 -->
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

            <span class="cnt run" :class="{ zero: !r.ExecutionCount }">{{ r.ExecutionCount }}</span>
            <span class="cnt ok" :class="{ zero: !r.ExecutionSuccess }">{{ r.ExecutionSuccess }}</span>
            <span class="cnt bad" :class="{ zero: !r.ExecutionFail }">{{ r.ExecutionFail }}</span>

            <span class="sock">
              <span class="tg" :class="r.UseSystemSocket ? 'sys' : 'cus'">{{ socketText(r) }}</span>
            </span>

            <span class="loop">
              <span class="tg ok">{{ r.LoopCount }} {{ t('snd.loopTimes') }}</span>
              <span class="tg wa">{{ r.LoopInterval }} {{ t('snd.loopMs') }}</span>
            </span>

            <!-- 空发送集执行起来什么都不会发，标出来省得点进编辑器才发现 -->
            <span class="pk" :class="{ none: !r.PacketCount }"
                  :title="r.PacketCount ? '' : t('snd.noPackets')">
              {{ r.PacketCount }}
            </span>

            <span class="notes" :title="r.Notes">{{ r.Notes }}</span>

            <span class="ops">
              <button class="op" :title="t('acct.op.edit')" @click="openEdit(r)">
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

    <SendEdit :id="editing" @close="editing = null" />
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

.sep {
  width: 1px;
  height: 16px;
  background: var(--border);
}

/* 启停：停止态是红的，与「这会真的发东西出去」相称 */
.btn.run {
  display: inline-flex;
  align-items: center;
  gap: 7px;
  border-color: rgb(var(--green-rgb) / 45%);
  color: var(--green);
}

.btn.run .ico {
  /*
    11px 而不是代理状态条那枚的 13px：这个按钮的内容高度是 10.5px 的文字行，
    图标一超过它就把整个按钮撑高。再用 -1px 的上下外边距把图标的占位收回到 9px，
    按钮高度与旁边那些纯文字按钮完全一致。
  */
  width: 11px;
  height: 11px;
  margin: -1px 0;
  flex: none;
}

.btn.run.on { border-color: rgb(var(--danger-rgb) / 30%); color: var(--danger); }

/*
  悬停要压过公共的 .btn:hover（那条是青色，(0,3,0)）：开始态照 .btn.primary 的绿、
  停止态照 .btn.danger 的红，否则这个绿按钮一悬停就变蓝，与旁边的「新增发送」不一样。
*/
.btn.run:hover:not(:disabled) { background: rgb(var(--green-rgb) / 10%); border-color: var(--green); color: var(--green); }
.btn.run.on:hover:not(:disabled) { background: rgb(var(--danger-rgb) / 12%); border-color: var(--danger); color: var(--danger); }
.btn.run.on .ico { animation: pulse 1.1s ease-in-out infinite; }

@keyframes pulse { 50% { opacity: .25; } }

@media (prefers-reduced-motion: reduce) {
  .btn.run.on .ico { animation: none; }
}

/*
  只留这一屏独有的：列宽、列间距、最小宽度。
  display / align-items / padding / 高度 / 配色都在 style.css 的 .list-page 里。
  两个 1fr（名称、备注）吃剩余宽度；min-width 让窄窗口下整体横向滚，而不是把列压成一团。
*/
.head,
.row {
  grid-template-columns:
    46px 50px minmax(130px, 1.2fr)
    64px 64px 64px 80px 116px 58px
    minmax(100px, 1fr) 60px;
  gap: 8px;
  min-width: 940px;
}

.head > span { overflow: hidden; text-overflow: ellipsis; }

/*
  除「发送名称」和「备注」外全部居中 —— 表头与内容一起。
  那两列是长度不可预知的文本，居中会让每行的起点忽左忽右，扫不下来；
  其余都是定宽的短值（序号 / 勾选框 / 五个数字 / 两组标签 / 三个按钮），居中更整齐。

  写成「先全居中、再把两列拉回左」而不是逐列标 class：
  加一列时默认就是对的，忘了标不会露馅。第 4 / 11 列即名称与备注。
*/
.head > span,
.row > span { text-align: center; }

.head > span.name,
.head > span.notes,
.row > span.name,
.row > span.notes { text-align: left; }

.row > span { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }

.no { color: var(--dim); font-variant-numeric: tabular-nums; }
.name { color: var(--gray); }
.notes { color: var(--dim4); font-size: var(--fs-small); }

.cnt { font-family: var(--mono); font-variant-numeric: tabular-nums; }

/* 三个计数各有各的颜色，照搬 WinForms（蓝 / 绿 / 红） */
.cnt.run { color: var(--cyan); }
.cnt.ok { color: var(--green); }
.cnt.bad { color: var(--danger); }

/* 0 一律压暗：一屏几十行里，真正有数的那几行才该跳出来 */
.cnt.zero { color: var(--dim); }

.pk { font-family: var(--mono); font-variant-numeric: tabular-nums; color: var(--dim3); }

.pk.none { color: var(--danger); }

/* flex 容器里的标签不吃 text-align，得自己居中 */
.sock,
.loop { display: flex; align-items: center; justify-content: center; gap: 4px; overflow: hidden; }

.tg {
  flex: none;
  padding: 4px 6px 4px;
  border: 1px solid;
  font-size: var(--fs-label);
  /* 显式 1：默认行高会把行距全压在字的下面，字在框里偏上（与按钮同一个问题）*/
  line-height: 1;
  font-family: var(--share);
  letter-spacing: .04em;
  white-space: nowrap;
}

.tg.ok { border-color: rgb(var(--green-rgb) / 35%); color: var(--green); }
.tg.wa { border-color: rgb(var(--amber-rgb) / 35%); color: var(--amber); }
.tg.sys { border-color: rgb(var(--danger-rgb) / 30%); color: var(--danger); }
.tg.cus { border-color: rgb(var(--green-rgb) / 35%); color: var(--green); }

</style>
