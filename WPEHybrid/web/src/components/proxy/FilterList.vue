<script setup lang="ts">
/*
  滤镜列表 —— 对应 WinForms 的 Controls/FilterList。

  滤镜是 WPE 的核心：命中之后改写封包内容 / 长度 / 拦截。
  与「过滤设置」不是一回事 —— 那个只决定收不收这个包，这里决定怎么改它。

  【⚠️ 顺序就是数据】DoFilterList 按<b>列表顺序</b>逐个执行滤镜，
  这正是右键菜单里「置顶 / 上移 / 下移 / 置底」存在的理由。所以：
  ① `FeedList.Filter` 一直走整表 Replace（FeedPump 的默认），
     绝不能照账号列表那样改增量推 —— 账号漏一条只是少一行，
     滤镜漏一条是执行顺序错了而且看不出来；
  ② 前端<b>不排序、不重排、不筛选</b>，永远按 C# 推来的顺序原样显示。

  【这一屏是模板】发送 / 机器人 / 仓库三屏结构几乎相同，右键菜单更是
  WinForms 侧就共用的（SystemConfig.GetCMS_List() + ListAction）。
  照抄这一份改个列表名即可。
*/
import { computed, onBeforeUnmount, onMounted, ref } from 'vue'
import { call } from '../../bridge'
import { FeedList, FilterAction, ListAction, type FilterRow } from '../../bridge/types'
import { t, type Key } from '../../i18n'
import { useList } from '../../stores/lists'
import { useRowPick } from '../../usePick'
import { pushToast } from '../../stores/toast'
import ContextMenu from '../ContextMenu.vue'
import FilterEdit from './FilterEdit.vue'
import { ICON, type MenuItem } from '../menu'

/*
  这一屏<b>没有搜索框</b>，是有意的（也与 WinForms 一致，那边也没有）。

  滤镜表是几十条、而且顺序本身就是内容（执行顺序），
  一旦筛掉一部分，右键那四个移动动作就没法用了 ——
  「向上移动」是相对整张表的，会把行移到一个屏幕上看不见的位置去。
  与其做一个「一搜索就半残」的筛选，不如不做。
*/
const rows = useList<FilterRow>(FeedList.Filter)

//注入模式下滤镜编辑的「作用于哪些封包」要出 8 个 WinSock 函数类别而非代理的 4 个，见 FilterEdit
const props = withDefaults(defineProps<{ mode?: 'proxy' | 'inject' }>(), { mode: 'proxy' })

/*
  ── 虚拟滚动 ───────────────────────────────────────────────────
  与账号列表同一套。滤镜通常几十条，远到不了两万，但这一份是后面三屏的模板，
  一开始就照着能长的写，省得将来某一份撑爆了再回来改。
  行高写死 34px，必须与 .row 的 height 一致。
*/
const ROW_H = 34
const OVERSCAN = 8

const scroller = ref<HTMLElement | null>(null)
const scrollTop = ref(0)
const viewH = ref(600)

const total = computed(() => rows.value.length)
const start = computed(() => Math.max(0, Math.floor(scrollTop.value / ROW_H) - OVERSCAN))

const end = computed(() =>
  Math.min(total.value, start.value + Math.ceil(viewH.value / ROW_H) + OVERSCAN * 2),
)

const windowRows = computed(() => rows.value.slice(start.value, end.value))

function onScroll(): void {
  const el = scroller.value
  if (el) scrollTop.value = el.scrollTop
}

/*
  ── 执行模式 ───────────────────────────────────────────────────

  两种，在<b>系统设置</b>里切（Controls/SystemSetting 的 rbFilterSet_*）。
  语义以 Operate.FilterConfig.List.DoFilterList 为准，不是照标签猜的：

    Priority（优先原则）  从上到下，第一个命中的执行完就 return，后面的滤镜不再跑
    Sequence（按顺序执行）从上到下逐个匹配，命中的都跑，改写逐个叠加

  【两种模式共有的一条】命中的那条动作是 拦截 / 换包 / 只显示 / 不显示 时立刻返回 ——
  也就是说 Sequence 下<b>只有「替换」会继续往下走</b>。那四个动作本质上是终结性的。

  枚举值来自 Operate.FilterConfig.Filter.Execute：0 = Priority，1 = Sequence。
  别和 SystemConfig.ListExecute 搞混 —— 那是另一个同名枚举（Sequence / Together），
  管的是发送列表与机器人列表。
*/
const execMode = ref(1)
const isPriority = computed(() => execMode.value === 0)

let ro: ResizeObserver | null = null

onMounted(async () => {
  const el = scroller.value

  if (el) {
    viewH.value = el.clientHeight
    ro = new ResizeObserver(() => { viewH.value = el.clientHeight })
    ro.observe(el)
  }

  try {
    const r = await call<{ mode: number }>('getFilterExecute')
    execMode.value = r?.mode ?? 1
  } catch (e) {
    console.error('[flt] 读取执行模式失败', e)
  }
})

onBeforeUnmount(() => {
  ro?.disconnect()
  ro = null
})

/* ── 多选 ───────────────────────────────────────────────────── */

//单击 / Ctrl / Shift 多选，全项目一份实现，见 usePick.ts
const { picked, pickedIds, onRowClick, selectAll, clear } =
  useRowPick(rows, (r) => r.Id)

/* ── 显示用的换算 ───────────────────────────────────────────── */

/** 动作文案。逐条照抄 FilterList.cs 的 Render，不另起译法。 */
const ACTION_LABEL: Record<number, Key> = {
  [FilterAction.Replace]: 'proxy.act.replace',
  [FilterAction.Change]: 'proxy.act.change',
  [FilterAction.Intercept]: 'proxy.act.intercept',
  [FilterAction.NoModify_Display]: 'proxy.act.display',
  [FilterAction.NoModify_NoDisplay]: 'proxy.act.hide',
  [FilterAction.None]: 'proxy.act.none',
}

//按语言展平：模板里逐格调 t() 会把 lang 挂进每一格的依赖表
const actionText = computed<Record<number, string>>(() => {
  const m: Record<number, string> = {}
  for (const k of Object.keys(ACTION_LABEL)) m[+k] = t(ACTION_LABEL[+k])
  return m
})

/** 「指定类型」的四个标签，顺序与颜色照抄 Render。 */
function appointTags(r: FilterRow): Array<{ key: Key; cls: string }> {
  const out: Array<{ key: Key; cls: string }> = []
  if (r.AppointHeader) out.push({ key: 'flt.ap.head', cls: 'g' })
  if (r.AppointSocket) out.push({ key: 'flt.ap.socket', cls: 'a' })
  if (r.AppointPort) out.push({ key: 'flt.ap.port', cls: 'd' })
  if (r.AppointLength) out.push({ key: 'flt.ap.length', cls: 'c' })
  return out
}

/** 「递进」的三个标签。第一个看的是<b>位置串非空</b>，不是某个 bool。 */
function progressionTags(r: FilterRow): Array<{ key: Key; cls: string }> {
  const out: Array<{ key: Key; cls: string }> = []
  if (r.ProgressionPosition) out.push({ key: 'flt.pg.on', cls: 'r' })
  if (r.IsProgressionContinuous) out.push({ key: 'flt.pg.continuous', cls: 'g' })
  if (r.IsProgressionCarry) out.push({ key: 'flt.pg.carry', cls: 'a' })
  return out
}

/* ── 动作 ───────────────────────────────────────────────────── */

async function toggle(r: FilterRow): Promise<void> {
  try {
    await call('setFilterEnable', { id: r.Id, enable: !r.IsEnable })
  } catch (e) {
    console.error('[flt] 切换启用失败', e)
  }
}

async function add(): Promise<void> {
  try {
    await call('addFilter')
    //新滤镜追加在表尾，滚过去让人看见它
    requestAnimationFrame(() => {
      const el = scroller.value
      if (el) el.scrollTop = el.scrollHeight
    })
  } catch (e) {
    console.error('[flt] 新增失败', e)
  }
}

/*
  打开滤镜编辑（与 WinForms 的 tFilterList_CellDoubleClick / bEdit 一致）。
  双击行、点行内那支笔，两条路都走这一个函数。
*/
const editing = ref<string | null>(null)

function openEdit(r: FilterRow): void {
  editing.value = r.Id
}

/**
 * 双击一行打开编辑。
 *
 * 【落在行内按钮上的双击不算】行里有两个勾选框和两个操作按钮：
 * 双击勾选框会把它切两次（等于没切）却还开了弹窗，双击删除更不该顺带开编辑。
 *
 * DOM 的 dblclick 只对主键派发，不必像 WinForms 那边再判一次 e.Button ——
 * 那是 AntdUI 的 Table 任意键双击都抛事件才需要的。
 */
function onRowDblClick(e: MouseEvent, r: FilterRow): void {
  if ((e.target as HTMLElement | null)?.closest('button')) return
  openEdit(r)
}

async function simple(method: string, arg?: Record<string, unknown>): Promise<void> {
  try {
    await call(method, arg)
  } catch (e) {
    console.error('[flt] ' + method + ' 失败', e)
  }
}

/* ── 右键菜单 ───────────────────────────────────────────────── */

const menuAt = ref<{ x: number; y: number } | null>(null)

const menuItems = computed<MenuItem[]>(() => {
  //七项全都作用于选中的行，所以七项都带上条数 —— 尤其删除，动手前该看见要删几条
  const n = picked.value.size
  const tag = n ? ' (' + n + ')' : ''

  /*
    置顶 / 置底比上移 / 下移多一条<b>横杠</b>（⤒ ⤓ 那种画法）——
    四个都是箭头，光靠长短分不出来，实测 14px 下横杠是唯一看得清的差别。
  */
  return [
    { id: 'top', label: t('lst.top') + tag, icon: ICON.top },
    { divider: true },
    { id: 'up', label: t('lst.up') + tag, icon: ICON.up },
    { id: 'down', label: t('lst.down') + tag, icon: ICON.down },
    { divider: true },
    { id: 'bottom', label: t('lst.bottom') + tag, icon: ICON.bottom },
    { divider: true },
    /*
      导出用软盘（存盘）而不是下箭头：它弹的是「另存为」文件框，
      而且旁边的复制、删除都是实物形状，一个箭头夹在中间不成一套。

      画法是「外框 + 底部标签」两笔，<b>刻意省掉顶部那道插槽</b> ——
      三笔的完整软盘在 14px 下会糊成一团（对比过 14 / 20 / 32 三档）。
      左上那个斜切角是软盘的招牌，只要它在就认得出来。
    */
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

  //其余七项全都作用于选中的行，一处拦住就够了（加菜单项时最容易漏这句）
  if (picked.value.size === 0) {
    pushToast('warning', t('lst.needPick'))
    return
  }

  const action = ACTION_OF[id]
  if (action === undefined) return

  try {
    await call('filterListAction', { action, ids: pickedIds.value })
  } catch (e) {
    console.error('[flt] 列表操作失败', e)
  }
}
</script>

<template>
  <div class="page list-page">
    <div class="bar">
      <button class="btn primary" @click="add">{{ t('flt.add') }}</button>

      <button class="btn" :disabled="!rows.length" @click="simple('setAllFilterEnable', { enable: true })">
        {{ t('flt.enableAll') }}
      </button>
      <button class="btn" :disabled="!rows.length" @click="simple('setAllFilterEnable', { enable: false })">
        {{ t('flt.disableAll') }}
      </button>
      <button class="btn" :disabled="!rows.length" @click="simple('resetFilterCount')">
        {{ t('flt.resetCount') }}
      </button>

      <span class="grow" />

      <button class="btn" @click="simple('importFilters')">{{ t('flt.import') }}</button>
      <button class="btn" :disabled="!rows.length" @click="simple('exportFilters')">{{ t('flt.export') }}</button>
      <button class="btn danger" :disabled="!rows.length" @click="simple('clearFilters')">{{ t('flt.clearAll') }}</button>
    </div>

    <!--
      顺序即执行顺序，而「顺序意味着什么」还取决于执行模式 —— 这两层界面上不说就没人知道。
      模式是读出来的实际值，不是写死的一句通用话。
    -->
    <div v-if="rows.length > 1" class="ordbar" :class="{ alt: isPriority }">
      <span class="mk">{{ isPriority ? t('flt.mode.priority') : t('flt.mode.sequence') }}</span>
      <span class="tx">{{ isPriority ? t('flt.mode.priorityHint') : t('flt.mode.sequenceHint') }}</span>
      <span class="note">{{ t('flt.mode.stopNote') }}</span>
    </div>

    <div ref="scroller" class="body" @scroll.passive="onScroll">
      <div class="head">
        <!-- 表头每格带着与数据行同名的 class，对齐规则按 class 写、增删列不必重编序号 -->
        <span class="no">{{ t('col.id') }}</span>
        <span class="ck">{{ t('col.enable') }}</span>
        <span class="name">{{ t('col.filterName') }}</span>
        <span class="act">{{ t('col.action') }}</span>
        <span class="cnt">{{ t('col.execCount') }}</span>
        <span class="tags">{{ t('col.appoint') }}</span>
        <span class="tags">{{ t('col.progression') }}</span>
        <span class="ops">{{ t('col.ops') }}</span>
      </div>

      <div v-if="!rows.length" class="empty">{{ t('flt.empty') }}</div>

      <div v-else class="spacer" :style="{ height: total * ROW_H + 'px' }">
        <div class="win" :style="{ transform: `translateY(${start * ROW_H}px)` }">
          <!-- key 用窗口内下标：滚动时就地改文本、复用这 40 个 DOM 节点 -->
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

            <span class="name">{{ r.Name }}</span>

            <span class="act">{{ actionText[r.Action] ?? r.Action }}</span>
            <span class="cnt" :class="{ zero: !r.ExecutionCount }">{{ r.ExecutionCount }}</span>

            <span class="tags">
              <span v-for="g in appointTags(r)" :key="g.key" class="tg" :class="g.cls">{{ t(g.key) }}</span>
            </span>

            <span class="tags">
              <span v-for="g in progressionTags(r)" :key="g.key" class="tg" :class="g.cls">{{ t(g.key) }}</span>
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

    <FilterEdit :id="editing" :mode="props.mode" @close="editing = null" />
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

/* 除滤镜名外全部居中；带 .row >/.head > 才压得过下面那条通则 */
/* 九列。滤镜名给 1fr —— 它是唯一长度不可预知的字段 */
/* display / align-items / padding / 高度 / 配色都在 style.css 的 .list-page 里 */
.head,
.row {
  grid-template-columns: 46px 46px minmax(120px, 1fr) 92px 76px 172px 148px 68px;
  gap: 10px;
}

.head > span,
.row > span { text-align: center; }

.head > span.name,
.row > span.name { text-align: left; }

.no { color: var(--dim); font-variant-numeric: tabular-nums; }
.name { color: var(--gray); overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
.act { color: var(--acc-violet); }

/*
  执行次数。

  【它顺带接管了原「状态」列】那一列曾显示 禁用 / 启用 / 处理中，
  但三种状态都是从别处推出来的：禁用看启用列，「处理中」= 启用 且 次数 > 0。
  两个输入本来就都在表上，一整列去讲一遍是重复的。
  这里只留一处替代：<b>跑过的次数点亮，没跑过的压暗</b>，
  「这条滤镜到底在不在干活」还是一眼能扫出来，而且比一个徽标多给了具体数字。
*/
.cnt { color: var(--cyan); font-variant-numeric: tabular-nums; }
.cnt.zero { color: var(--dim); }

/* 标签组。行高固定 34px，装不下就横向裁掉，不换行 */
.tags {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 4px;
  overflow: hidden;
}

.tg {
  flex: none;
  padding: 4px 6px 4px;   /* 上 +1 下 -1：字形在 em 框里偏上 1px（上伸 9 / 下伸 3，实测），补回来 */
  border: 1px solid currentColor;
  /* 与发送列表的标签同一款字：上 +1 下 -1 那个补偿是按 Share Tech Mono 量的（上伸 9 / 下伸 3），
     JetBrains Mono 上伸 11，同样的补偿会让字低 1px（实测），所以字体必须跟着一起统一 */
  font-family: var(--share);
  font-size: var(--fs-label);
  letter-spacing: .04em;
  /* 显式 1：默认行高会把行距全压在字的下面，字在框里偏上（与按钮同一个问题）*/
  line-height: 1;
  white-space: nowrap;
}

.tg.g { color: var(--green); }
.tg.a { color: var(--amber); }
.tg.c { color: var(--cyan); }
.tg.r { color: var(--danger); }
.tg.d { color: var(--muted); }

</style>
