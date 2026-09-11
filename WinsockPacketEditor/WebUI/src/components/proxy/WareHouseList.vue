<script setup lang="ts">
/*
  仓库列表 —— 对应 WinForms 的 Controls/WareHouseList。

  一个「仓库」= 一个名字 + 一堆封包字节（仓储数据）。封包进仓库有两条路：
  ① 封包列表右键「添加到仓库 ▸」手动放；② 「自动入库」按包头自动收（见 AutoStoresSetting）。
  仓库里的封包供滤镜的「执行仓库」动作与机器人指令取用。

  【这一屏只管仓库这一层】仓储明细在仓库编辑器（WareHouseEdit）里看，双击行或点那支笔打开。
  DTO 里刻意只给条数不给明细（B9 的规则）。

  【顺序就是数据】FeedList.WareHouse 走整表 Replace，前端不排序不筛选，
  右键的四个移动动作才有意义 —— 滤镜「执行仓库」按仓库挑，顺序影响的是用户找起来顺不顺手，
  但与滤镜 / 发送保持同一套右键菜单，少一种要记的规则。

  【自动入库是这一屏的一个按钮，不进设置菜单】WinForms 里它是 WareHouseList 工具条上的
  「设置」按钮弹出的 Modal（AutoStoresList），不在 ProxyList 的 12 项设置里。这里照搬。
*/
import { computed, onBeforeUnmount, onMounted, ref } from 'vue'
import { call } from '../../bridge'
import { FeedList, ListAction, type WareHouseRow } from '../../bridge/types'
import { t } from '../../i18n'
import { useList } from '../../stores/lists'
import { useRowPick } from '../../usePick'
import { pushToast } from '../../stores/toast'
import ContextMenu from '../ContextMenu.vue'
import { ICON, type MenuItem } from '../menu'
import AutoStoresSetting from './AutoStoresSetting.vue'
import WareHouseEdit from './WareHouseEdit.vue'

const rows = useList<WareHouseRow>(FeedList.WareHouse)

/*
  ── 虚拟滚动 ───────────────────────────────────────────────────
  与封包 / 账号 / 滤镜 / 发送四张表同一套：定高窗口 + translateY。
  仓库通常只有几个，虚拟滚动在这里几乎不省什么 —— 用它是为了五张表一个写法，
  而不是为了性能。ROW_H <b>必须与 .row 的 height 逐像素一致</b>。
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

let ro: ResizeObserver | null = null

onMounted(() => {
  const el = scroller.value
  if (el) {
    viewH.value = el.clientHeight
    ro = new ResizeObserver(() => { viewH.value = el.clientHeight })
    ro.observe(el)
  }
})

onBeforeUnmount(() => { ro?.disconnect() })

/* ── 多选 ───────────────────────────────────────────────────── */

//单击 / Ctrl / Shift 多选，全项目一份实现，见 usePick.ts
const { picked, pickedIds, onRowClick, selectAll, clear } =
  useRowPick(rows, (r) => r.Id)

/* ── 行为 ───────────────────────────────────────────────────── */

async function add(): Promise<void> {
  try {
    await call('addWareHouse')
    //新仓库追加在表尾，滚过去让人看见它
    requestAnimationFrame(() => {
      const el = scroller.value
      if (el) el.scrollTop = el.scrollHeight
    })
  } catch (e) {
    console.error('[wh] 新增失败', e)
  }
}

async function simple(method: string): Promise<void> {
  try {
    await call(method)
  } catch (e) {
    console.error('[wh] ' + method + ' 失败', e)
  }
}

/* 双击行、点行内那支笔，两条路都走这一个函数 */
const editing = ref<string | null>(null)

function openEdit(r: WareHouseRow): void {
  editing.value = r.Id
}

function onRowDblClick(e: MouseEvent, r: WareHouseRow): void {
  //落在行内按钮上的双击不算
  if ((e.target as HTMLElement | null)?.closest('button')) return
  openEdit(r)
}

/* ── 自动入库 ───────────────────────────────────────────────── */

const autoOpen = ref(false)

/* ── 右键菜单 ───────────────────────────────────────────────── */

const menuAt = ref<{ x: number; y: number } | null>(null)

const menuItems = computed<MenuItem[]>(() => {
  //七项全都作用于选中的行，所以七项都带上条数 —— 尤其删除，动手前该看见要删几个
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
    await call('wareHouseListAction', { action, ids: pickedIds.value })
  } catch (e) {
    console.error('[wh] 列表操作失败', e)
  }
}
</script>

<template>
  <div class="page list-page">
    <div class="bar">
      <button class="btn primary" @click="add">{{ t('wh.add') }}</button>

      <!--
        自动入库放在新增旁边而不是右边那组：它是「往仓库里放东西」的另一条路，
        与新增仓库是一类；右边那组是整张表的导入 / 导出 / 清空。
      -->
      <!-- 琥珀色：它是「打开后封包会自动进仓库」的开关入口，与旁边的增删改不是一类；不带图标，与各屏工具条一致 -->
      <button class="btn warn" @click="autoOpen = true">{{ t('wh.autoStores') }}</button>

      <span class="grow" />

      <button class="btn" @click="simple('importWareHouses')">{{ t('wh.import') }}</button>
      <button class="btn" :disabled="!rows.length" @click="simple('exportWareHouses')">{{ t('wh.export') }}</button>
      <button class="btn danger" :disabled="!rows.length" @click="simple('clearWareHouses')">
        {{ t('wh.clearAll') }}
      </button>
    </div>

    <div ref="scroller" class="body" @scroll.passive="onScroll">
      <div class="head">
        <!-- 表头每格带着与数据行同名的 class，对齐规则按 class 写、增删列不必重编序号 -->
        <span class="no">{{ t('col.id') }}</span>
        <span class="name">{{ t('col.wareHouseName') }}</span>
        <span class="cnt">{{ t('col.stores') }}</span>
        <span class="ops">{{ t('col.ops') }}</span>
      </div>

      <div v-if="!rows.length" class="empty">{{ t('wh.empty') }}</div>

      <div v-else class="spacer" :style="{ height: total * ROW_H + 'px' }">
        <div class="win" :style="{ transform: `translateY(${start * ROW_H}px)` }">
          <!-- key 用窗口内下标：滚动时就地改文本、复用这些 DOM 节点 -->
          <div
            v-for="(r, i) in windowRows"
            :key="i"
            class="row"
            :class="{ sel: picked.has(r.Id) }"
            @click="onRowClick(r, $event, start + i)"
            @contextmenu.prevent="menuAt = { x: $event.clientX, y: $event.clientY }"
            @dblclick="onRowDblClick($event, r)"
          >
            <span class="no">{{ start + i + 1 }}</span>

            <span class="name" :title="r.Name">{{ r.Name }}</span>

            <!-- 空仓库压暗：一屏几个仓库里，真正装了东西的才该跳出来 -->
            <span class="cnt" :class="{ zero: !r.DataCount }"
                  :title="r.DataCount ? '' : t('wh.noStores')">
              {{ r.DataCount }}
            </span>

            <span class="ops">
              <button class="op" :title="t('acct.op.edit')" @click="openEdit(r)">
                <svg class="ico" viewBox="0 0 24 24"><path d="M4 20h4L20 8l-4-4L4 16z" /></svg>
              </button>
              <button class="op del" :title="t('acct.op.del')"
                      @click="picked = new Set([r.Id]); onMenuPick('delete')">
                <svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg>
              </button>
            </span>
          </div>
        </div>
      </div>
    </div>

    <ContextMenu :at="menuAt" :items="menuItems" @pick="onMenuPick" @close="menuAt = null" />

    <AutoStoresSetting v-model:open="autoOpen" />

    <WareHouseEdit :id="editing" @close="editing = null" />
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

/*
  只留这一屏独有的：列宽、列间距、最小宽度。
  display / align-items / padding / 高度 / 配色都在 style.css 的 .list-page 里。
  名称那一列吃剩余宽度。
*/
.head,
.row {
  grid-template-columns: 46px minmax(200px, 1fr) 120px 60px;
  gap: 8px;
  min-width: 460px;
}

.head > span { overflow: hidden; text-overflow: ellipsis; }

/* 除「仓库名称」外全部居中 —— 表头与内容一起 */
.head > span,
.row > span { text-align: center; }

.head > span.name,
.row > span.name { text-align: left; }

.row > span { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }

.no { color: var(--dim); font-variant-numeric: tabular-nums; }

/*
  名称前原先有一枚 13px 的仓库图标（侧栏那枚六边形），这个尺寸下六个角看不出来，
  成了一个青色小圆点 —— 像「在线 / 离线」那种状态标记，会被误读。已去掉。
*/
.name { color: var(--gray); }

.cnt { font-family: var(--mono); font-variant-numeric: tabular-nums; color: var(--cyan); }
.cnt.zero { color: var(--dim); }
</style>
