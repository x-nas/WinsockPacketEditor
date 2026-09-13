<script setup lang="ts">
/*
  快捷面板 —— 对应 WinForms 的 Controls/QuickList（滤镜 / 发送 / 机器人 / 仓库四个标签）。

  它的用处是「抓包时不离开这一屏就能启停规则」：勾选框直接改 IsEnable。
  完整的增删改在各自的列表页里做（滤镜 / 发送 / 机器人 / 仓库四个列表页都已完成）。

  数据全部来自 B9d 的推送通道（FeedPump 订阅 ListChanged 整表 Replace），
  这里只读前端副本，不产生往返。
*/
import { computed, onBeforeUnmount, onMounted, ref } from 'vue'
import { call, on } from '../../bridge'
import { FeedList, ListAction, type FilterRow, type RobotRow, type SendRow, type WareHouseRow } from '../../bridge/types'
import { t, type Key } from '../../i18n'
import { useList } from '../../stores/lists'
import { hotkeyCount, hotkeyType, refreshHotkey } from '../../stores/runtime'
import { pushToast } from '../../stores/toast'
import ContextMenu from '../ContextMenu.vue'
import { ICON, type MenuItem } from '../menu'
import FilterEdit from './FilterEdit.vue'
import SendEdit from './SendEdit.vue'
import WareHouseEdit from './WareHouseEdit.vue'
import RobotEdit from './RobotEdit.vue'

type TabKey = 'filter' | 'send' | 'robot' | 'warehouse'

//注入模式下滤镜编辑的「作用于哪些封包」要出 8 个 WinSock 函数类别而非代理的 4 个，见 FilterEdit
const props = withDefaults(defineProps<{ mode?: 'proxy' | 'inject' }>(), { mode: 'proxy' })

const tab = ref<TabKey>('filter')

const filters = useList<FilterRow>(FeedList.Filter)
const sends = useList<SendRow>(FeedList.Send)
const robots = useList<RobotRow>(FeedList.Robot)
const houses = useList<WareHouseRow>(FeedList.WareHouse)

const TABS: Array<{ key: TabKey; label: 'quick.tab.filter' | 'quick.tab.send' | 'quick.tab.robot' | 'quick.tab.warehouse' }> = [
  { key: 'filter', label: 'quick.tab.filter' },
  { key: 'send', label: 'quick.tab.send' },
  { key: 'robot', label: 'quick.tab.robot' },
  { key: 'warehouse', label: 'quick.tab.warehouse' },
]

/** 四份表统一成「勾选 + 名称 + 右侧标记」三段，面板才不用为每种类型各写一套。 */
interface Item { id: string; on: boolean; name: string; badge: string; tone: string }

/*
  四栏的右侧标记<b>统一是"跑了多少次"</b>：滤镜 / 发送 / 机器人是执行次数，
  仓库是存了多少条。滤镜那栏原来显示的是动作（替换 / 拦截 …），
  但动作是配置、打开编辑弹窗就看得到，而这块面板是运行起来之后盯的，
  要看的是哪条在命中、命中了多少次。
*/
const items = computed<Item[]>(() => {
  if (tab.value === 'filter') {
    return filters.value.map((r) => ({
      id: r.Id,
      on: r.IsEnable,
      name: r.Name,
      badge: String(r.ExecutionCount ?? 0),
      tone: 'n',
    }))
  }

  if (tab.value === 'send') {
    return sends.value.map((r) => ({
      id: r.Id, on: r.IsEnable, name: r.Name,
      badge: String(r.ExecutionCount), tone: 'n',
    }))
  }

  if (tab.value === 'robot') {
    return robots.value.map((r) => ({
      id: r.Id, on: r.IsEnable, name: r.Name,
      badge: String(r.ExecutionCount), tone: 'n',
    }))
  }

  return houses.value.map((r) => ({
    id: r.Id, on: true, name: r.Name,
    badge: String(r.DataCount ?? 0), tone: 'n',
  }))
})

const listOf: Record<TabKey, FeedList> = {
  filter: FeedList.Filter,
  send: FeedList.Send,
  robot: FeedList.Robot,
  warehouse: FeedList.WareHouse,
}

async function toggle(it: Item): Promise<void> {
  // 仓库没有启停开关
  if (tab.value === 'warehouse') return

  try {
    await call('setListEnable', { list: listOf[tab.value], id: it.id, enable: !it.on })
  } catch (e) {
    console.error('[quick] 切换启用状态失败', e)
  }
}

/*
  ── 打开编辑 ──────────────────────────────────────────────

  双击整行开编辑弹窗，与滤镜列表 / 账号列表同一条口径。
  <b>启停只认勾选框</b>：整行可点的话，想看一眼内容而点了一下，
  就会把一条正在生效的滤镜关掉 —— 代价与意图完全不对等。

  四栏双击各开各的编辑器（与列表页里双击同一个弹窗）。
*/
const editing = ref<string | null>(null)
const editingSend = ref<string | null>(null)
const editingHouse = ref<string | null>(null)
const editingRobot = ref<string | null>(null)

function onRowDblClick(e: MouseEvent, it: Item): void {
  //落在勾选框上的双击不算：那是切两次（等于没切）却还开了弹窗
  if ((e.target as HTMLElement | null)?.closest('button.ck')) return

  switch (tab.value) {
    case 'filter': editing.value = it.id; return
    case 'send': editingSend.value = it.id; return
    case 'warehouse': editingHouse.value = it.id; return
    case 'robot': editingRobot.value = it.id; return
  }
}

/* ── 右键菜单 ───────────────────────────────────────────────── */

/*
  对应 WinForms 的 QuickList：那边每个页签上方有一排图标按钮（新增 / 全部启用 / 全部禁用 /
  重置计数 / 清空，发送与机器人再加执行 / 停止），表格上还有一份 GetCMS_List 的右键菜单。

  【这里只做右键，不加按钮排】面板固定 320px 宽、又只占下半屏的一小块，
  再压一行工具条要么把标签挤到换行（整条标签栏高一倍），要么吃掉两行本来能看见的规则。
  所以两组动作合进同一个右键菜单：上半是<b>这一行</b>的（移动 / 复制 / 导出 / 删除），
  下半是<b>整份列表</b>的（新增 / 全部启停 / 重置计数 / 执行停止）。

  【作用于右键点中的那一行，不是选中集】这块面板没有多选 —— 与自动入库、映射设置那两张
  小表同一条口径。所以移动类动作按行的位置压暗（第一行不给置顶 / 上移）。
*/
const menuAt = ref<{ x: number; y: number } | null>(null)
/** 右键点中的那一行；null = 点在空白处，只出下半组 */
const menuRow = ref<Item | null>(null)

/** 发送 / 机器人两份列表在不在跑 —— 与列表页一样由 C# 的 1 秒统计拍推过来。 */
const sendRunning = ref(false)
const robotRunning = ref(false)
let offSend: (() => void) | null = null
let offRobot: (() => void) | null = null

onMounted(() => {
  offSend = on('send:running', (v: boolean) => { sendRunning.value = !!v })
  offRobot = on('robot:running', (v: boolean) => { robotRunning.value = !!v })
  void refreshHotkey()
})

/*
  ── 全局快捷键作用在哪 ──────────────────────────────────

  快捷键 1–10 按<b>列表下标</b>执行第 1–10 条、「执行 / 停止」启停整份列表 ——
  而「整份」是发送列表还是机器人列表，由快捷键设置里那个二选一决定（HotKeyType）。
  那个开关藏在设置弹窗里，抓包时按下快捷键之前根本看不出它会动哪一边；
  这块面板正好就摆着这两份列表，所以在<b>它作用的那个页签标题旁边</b>挂一枚键盘图标，
  悬停出完整说明。一个快捷键都没设时不挂 —— 那时说「作用在哪」没有意义。

  （2026-09-11 先做过一版面板底部的一整条「全局快捷键 → 发送列表」，按要求改成了这枚图标：
   不占列表的可见行，而且直接指着那个页签，比一句话再去对应更快。）
*/
const hkList = computed<TabKey | null>(() => (hotkeyCount.value ? (hotkeyType.value === 1 ? 'robot' : 'send') : null))
const hkName = computed(() => t(hotkeyType.value === 1 ? 'rb.e.swRobot' : 'rb.e.swSend'))
const hkTip = computed(() => t('quick.hkTip').split('{0}').join(hkName.value))

onBeforeUnmount(() => { offSend?.(); offRobot?.() })

/** 当前页签在跑没在跑（只有发送与机器人有这回事）。 */
const running = computed(() => (tab.value === 'send' ? sendRunning.value : tab.value === 'robot' ? robotRunning.value : false))

/** 四个页签各自的桥方法名与文案键，省得四处 switch。 */
const TAB_API: Record<TabKey, {
  action: string; add: string; addLabel: Key
  enableAll?: string; enableLabel?: Key; disableLabel?: Key
  resetCount?: string; resetLabel?: Key
  start?: string; stop?: string; startLabel?: Key; stopLabel?: Key
}> = {
  filter: {
    action: 'filterListAction', add: 'addFilter', addLabel: 'flt.add',
    enableAll: 'setAllFilterEnable', enableLabel: 'flt.enableAll', disableLabel: 'flt.disableAll',
    resetCount: 'resetFilterCount', resetLabel: 'flt.resetCount',
  },
  send: {
    action: 'sendListAction', add: 'addSend', addLabel: 'snd.add',
    enableAll: 'setAllSendEnable', enableLabel: 'snd.enableAll', disableLabel: 'snd.disableAll',
    resetCount: 'resetSendCount', resetLabel: 'snd.resetCount',
    start: 'startSendList', stop: 'stopSendList', startLabel: 'snd.start', stopLabel: 'snd.stop',
  },
  robot: {
    action: 'robotListAction', add: 'addRobot', addLabel: 'rb.add',
    enableAll: 'setAllRobotEnable', enableLabel: 'rb.enableAll', disableLabel: 'rb.disableAll',
    resetCount: 'resetRobotCount', resetLabel: 'rb.resetCount',
    start: 'startRobotList', stop: 'stopRobotList', startLabel: 'rb.start', stopLabel: 'rb.stop',
  },
  //仓库没有启停、没有执行次数，只有新增与行动作
  warehouse: { action: 'wareHouseListAction', add: 'addWareHouse', addLabel: 'wh.add' },
}

function onMenu(e: MouseEvent, it: Item | null): void {
  menuRow.value = it
  menuAt.value = { x: e.clientX, y: e.clientY }
}

const menuItems = computed<MenuItem[]>(() => {
  const api = TAB_API[tab.value]
  const out: MenuItem[] = []
  const row = menuRow.value

  if (row) {
    const i = items.value.findIndex((x) => x.id === row.id)
    const first = i <= 0
    const last = i < 0 || i >= items.value.length - 1

    out.push(
      { id: 'top', label: t('lst.top'), icon: ICON.top, disabled: first },
      { divider: true },
      { id: 'up', label: t('lst.up'), icon: ICON.up, disabled: first },
      { id: 'down', label: t('lst.down'), icon: ICON.down, disabled: last },
      { divider: true },
      { id: 'bottom', label: t('lst.bottom'), icon: ICON.bottom, disabled: last },
      { divider: true },
      { id: 'export', label: t('lst.export'), icon: ICON.save },
      { id: 'copy', label: t('lst.copy'), icon: ICON.copy },
      { divider: true },
      { id: 'delete', label: t('lst.delete'), icon: ICON.del, danger: true },
      { divider: true },
    )
  }

  //整份列表的动作，右键空白处时就只剩这一组
  out.push({ id: 'add', label: t(api.addLabel), icon: ICON.add })

  if (api.enableAll) {
    out.push(
      { id: 'enableAll', label: t(api.enableLabel as Key), icon: ICON.check },
      { id: 'disableAll', label: t(api.disableLabel as Key), icon: ICON.uncheck },
      { id: 'resetCount', label: t(api.resetLabel as Key), icon: ICON.undo },
    )
  }

  if (api.start) {
    out.push(
      { divider: true },
      running.value
        ? { id: 'stop', label: t(api.stopLabel as Key), icon: ICON.stop, danger: true }
        : { id: 'start', label: t(api.startLabel as Key), icon: ICON.play },
    )
  }

  return out
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
  const api = TAB_API[tab.value]
  const row = menuRow.value

  try {
    //① 行动作
    if (id in ACTION_OF) {
      if (!row) { pushToast('warning', t('lst.needPick')); return }
      await call(api.action, { action: ACTION_OF[id], ids: [row.id] })
      return
    }

    //② 整份列表
    if (id === 'add') { await call(api.add); return }
    if (id === 'enableAll' && api.enableAll) { await call(api.enableAll, { enable: true }); return }
    if (id === 'disableAll' && api.enableAll) { await call(api.enableAll, { enable: false }); return }
    if (id === 'resetCount' && api.resetCount) { await call(api.resetCount); return }
    if (id === 'start' && api.start) { await call(api.start); return }
    if (id === 'stop' && api.stop) { await call(api.stop) }
  } catch (e) {
    console.error('[quick] ' + id + ' 失败', e)
    pushToast('error', String(e))
  }
}
</script>

<template>
  <div class="pane">
    <div class="ptabs">
      <button
        v-for="x in TABS"
        :key="x.key"
        class="ptab"
        :class="{ on: tab === x.key }"
        @click="tab = x.key"
      >
        <span class="lb">{{ t(x.label) }}</span>
        <!--
          全局快捷键作用在这一页上。提示挂在图标上而不是整个页签上 ——
          整个页签悬停就弹一大段说明，每次想切页都被它挡一下。
        -->
        <span v-if="hkList === x.key" class="hk" :title="hkTip">
          <svg viewBox="0 0 24 24">
            <rect x="2" y="6" width="20" height="12" rx="1.5" />
            <path d="M6 10h1M10 10h1M14 10h1M18 10h1M7 14h10" />
          </svg>
        </span>
      </button>
    </div>

    <!-- 空白处也能右键：那时只出「整份列表」那一组（新增 / 全部启停 / 执行） -->
    <div class="pbody" @contextmenu.prevent="onMenu($event, null)">
      <div v-if="!items.length" class="empty">{{ t('proxy.emptyRules') }}</div>

      <div
        v-for="it in items"
        v-else
        :key="it.id"
        class="qrow"
        :class="{ on: it.on, edit: true }"
        @dblclick="onRowDblClick($event, it)"
        @contextmenu.prevent.stop="onMenu($event, it)"
      >
        <!--
          启停只在这个勾选框上 —— 它是真 <button>，所以键盘也能到达。
          原来是整行 role="button"，点一下就切启停：想看内容而点了一下，
          就把一条正在生效的规则关掉了。
        -->
        <button
          class="ck"
          :class="{ on: it.on }"
          :disabled="tab === 'warehouse'"
          :title="t('col.enable')"
          @click="toggle(it)"
        ><i /></button>

        <span class="t">{{ it.name }}</span>
        <span class="b" :class="it.tone">{{ it.badge }}</span>
      </div>
    </div>

    <ContextMenu :at="menuAt" :items="menuItems" @pick="onMenuPick" @close="menuAt = null" />

    <FilterEdit :id="editing" :mode="props.mode" @close="editing = null" />
    <SendEdit :id="editingSend" @close="editingSend = null" />
    <WareHouseEdit :id="editingHouse" @close="editingHouse = null" />
    <RobotEdit :id="editingRobot" @close="editingRobot = null" />
  </div>
</template>

<style scoped>
.pane { border: 1px solid var(--border); background: var(--card); display: flex; flex-direction: column; overflow: hidden; }

.ptabs { flex: none; display: flex; border-bottom: 1px solid var(--border); background: var(--panel); }

/*
  四个标签必须排成一行（面板固定 320px）。三道保险：
  左右内边距收到 11px 给长标签留余量；nowrap 让它不折行 ——
  折行会把整条标签栏撑高一倍，把下面的列表挤掉两行；
  再加 flex-shrink + 省略号，超宽时是<b>一起收窄</b>而不是把最后一个切掉半截
  （俄语的「Фильтр / Отправка / Робот / Хранилище」比 320px 多出 8px，
   不给收缩余量的话「Хранилище」正好被面板边缘裁掉）。
*/
.ptab {
  height: var(--th-h);
  flex: 0 1 auto;
  min-width: 0;
  overflow: hidden;
  display: inline-flex;
  align-items: center;
  gap: 5px;
  padding: 2px 11px 0;   /* 上 2 下 0：与底边那条 2px 的选中下划线对称，文字落在整个 30px 页签的中线上 */
  background: transparent;
  border: 0;
  border-bottom: 2px solid transparent;
  font-family: var(--share);
  font-size: var(--btn-size);
  /* 显式 1：Share Tech Mono 在 line-height: normal 下会把行距全压在字的下面，字号一大就明显偏上（实测） */
  line-height: 1;
  letter-spacing: .14em;
  text-transform: uppercase;
  white-space: nowrap;
  color: var(--muted);
  cursor: pointer;
}

.ptab:hover { color: var(--gray); }
.ptab.on { color: var(--green); border-bottom-color: var(--green); }
.ptab:focus-visible { outline-offset: -2px; }

/* 截断挪到文字那一格：页签收窄时省略号吃文字，键盘图标永远完整 */
/*
  ⚠️ line-height 要比 1 高：这一格为了省略号是 overflow: hidden，而页签整体是 line-height: 1 ——
  盒子只有 10.5px 高，中文字形比它高，100% 缩放下<b>字的顶和底都被切掉一行像素</b>（真机报的）。
  放高到 1.5 只是让裁切框装得下字形，页签是 flex 居中的，文字位置不变。
*/
.ptab .lb { min-width: 0; overflow: hidden; text-overflow: ellipsis; line-height: 1.5; }

/*
  全局快捷键图标。青色 —— 页签的选中态是绿、未选是灰，青与两者都分得开，
  而且与「就绪 / 配置类」的色语一致（它说的是一条配置，不是在跑）。
*/
/* 页签是「上 2 下 0」的内边距（给字形偏上的补偿），几何图标不需要那 1px，退回去（实测改前低 1.2px）*/
.ptab .hk { flex: none; display: flex; color: var(--cyan); position: relative; top: -1px; }
.ptab .hk svg { width: 14px; height: 14px; fill: none; stroke: currentColor; stroke-width: 1.6; stroke-linecap: round; }

.pbody { flex: 1; min-height: 0; overflow-y: auto; padding: 6px 0; }

.empty { padding: 22px 0; text-align: center; color: var(--muted); font-size: var(--fs-body); }

/*
  整行<b>不再是按钮</b>，所以 cursor 保持默认 —— 手型会把「点一下就会怎样」
  的暗示重新给回来，而现在点一下什么也不做。
  滤镜那栏可以双击开编辑，光标给 text 太弱、给 pointer 又是骗人，
  就交给行悬停的底色提示"这一行是可交互的"。
*/
.qrow { display: flex; align-items: center; gap: 9px; padding: 3px 12px; font-size: var(--fs-body); }
.qrow:hover { background: rgb(var(--tint-rgb) / 3%); }

/* 只有滤镜那栏双击有反应，让它自己带上文字光标以外的提示 */
.qrow.edit { user-select: none; }

.ck {
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 3px;
  margin: -3px;
  background: transparent;
  border: 0;
  cursor: pointer;
  flex: none;
}

.ck:disabled { cursor: default; }
.ck:focus-visible { outline-offset: -1px; }

.qrow i { width: 11px; height: 11px; border: 1px solid var(--border); flex: none; display: block; }
.ck.on i { border-color: var(--green); background: var(--green); box-shadow: 0 0 6px var(--green); }

.qrow .t { flex: 1; min-width: 0; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; color: var(--muted); }
.qrow.on .t { color: var(--gray); }

.qrow .b {
  font-family: var(--share);
  font-size: var(--label-size);   /* 9px 太小、--muted 太暗，看不清（用户反馈）；提到标签令牌的字号与表头那档灰 */
  /* 显式 1：默认行高会把行距全压在字的下面，字在框里偏上（与按钮同一个问题）*/
  line-height: 1;
  letter-spacing: .12em;
  text-transform: uppercase;
  padding: 3px 6px 3px;   /* 上 +1 下 -1：字形在 em 框里偏上 1px（上伸 9 / 下伸 3，实测），补回来 */
  border: 1px solid var(--border2);
  color: var(--th-fg);
}

</style>
