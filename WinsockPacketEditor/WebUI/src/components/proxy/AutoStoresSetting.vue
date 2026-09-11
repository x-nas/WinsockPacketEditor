<script setup lang="ts">
/*
  自动入库 —— 对应 WinForms 的 Controls/AutoStoresList。

  一条规则 = 指定包头 + 入库到哪个仓库。总开关打开后，经过代理的每个封包
  都会与启用的规则逐条比对（Operate 的 FlushToFeed 里那段），包头相同就把字节存一份进仓库。

  【总开关不落库，规则落库】总开关是 Operate.WareHouseConfig.WareHouse.Enable_AutoStores，
  一个纯运行期的 bool，WinForms 那句「每次重启软件后需手动开启」说的就是它。
  它决定的是每个封包都要多跑一遍规则比对，默认关着是刻意的，这里照搬。
  规则本身（增删改 / 启用 / 顺序）则每一步都落库 —— 外壳没有「关窗统一保存」那个时机。

  【与 WinForms 的一处不同】那边只有右键菜单能调顺序，且规则表用 AntdUI 的 ColumnCheck，
  勾一下就直接改模型。这里勾选框走 setAutoStoresEnable（改完落库 + 标脏），
  右键菜单同样四个移动项 —— 作用于<b>菜单开在的那一行</b>，这张表没有多选。

  【规则列表不走桥读】lstAutoStoresInfo 在 FeedPump 的推送流里（FeedList.AutoStores），
  这里只读前端副本；桥只提供改的入口。仓库名也是从 FeedList.WareHouse 的副本里查的。
*/
import { computed, ref, watch } from 'vue'
import { call } from '../../bridge'
import { FeedList, type AutoStoresRow, type WareHouseRow } from '../../bridge/types'
import { t } from '../../i18n'
import { useList } from '../../stores/lists'
import { pushToast } from '../../stores/toast'
import ContextMenu from '../ContextMenu.vue'
import { ICON, type MenuItem } from '../menu'
import SettingsModal from './SettingsModal.vue'
import AutoStoresEdit from './AutoStoresEdit.vue'

const props = defineProps<{ open: boolean }>()
const emit = defineEmits<{ (e: 'update:open', v: boolean): void }>()

const busy = ref(false)
const error = ref('')

const rules = useList<AutoStoresRow>(FeedList.AutoStores)
const houses = useList<WareHouseRow>(FeedList.WareHouse)

/** 仓库 Id → 名字。规则只存 WID，名字要回仓库列表查；仓库被删了就查不到。 */
const houseName = computed(() => {
  const m = new Map<string, string>()
  for (const h of houses.value) m.set(h.Id.toUpperCase(), h.Name)
  return m
})

function nameOf(wid: string): string {
  return houseName.value.get((wid || '').toUpperCase()) ?? ''
}

/* ── 总开关 ─────────────────────────────────────────────────── */

const enable = ref(false)

/*
  仓库上限。总开关是纯运行期的，上限则<b>要落库</b> ——
  前者决定「每个经过的封包多跑一遍规则比对」，默认关着是刻意的；
  后者是一条配置，用户改完当然要记住。

  满了<b>丢最旧的</b>（保留最近 N 条），不是像封包列表那样整表清空 ——
  仓库是拿来「留东西」的，隔一阵全没了不像话。
*/
const limit = ref(true)
const limitValue = ref(5000)

watch(() => props.open, async (on) => {
  if (!on) return

  error.value = ''

  try {
    const r = await call<{ enable: boolean; limit: boolean; limitValue: number }>('getAutoStoresMeta')
    enable.value = !!r?.enable
    limit.value = !!r?.limit
    limitValue.value = Number(r?.limitValue) || 5000
  } catch (e) {
    console.error('[as] 读取总开关失败', e)
  }
})

/** 「保存」只管总开关 —— 规则在各自的动作里已经落过库了。与 WinForms 的 bSave_Click 一致。 */
async function save(): Promise<void> {
  busy.value = true
  error.value = ''

  try {
    await call('setAutoStoresSwitch', {
      enable: enable.value,
      limit: limit.value,
      limitValue: Number(limitValue.value),
    })
    pushToast('success', t('as.saved'))
    emit('update:open', false)
  } catch (e) {
    console.error('[as] 保存总开关失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}

/* ── 规则的增删改 ───────────────────────────────────────────── */

/** null = 不开；'add' = 新增；其它 = 要改的那条的 Id */
const editing = ref<string | null>(null)
const editRow = ref<AutoStoresRow | null>(null)

function add(): void {
  //没有仓库时规则存不下来（C# 侧会拒），先说清楚，别让人填完再挨一句错
  if (!houses.value.length) {
    pushToast('warning', t('as.noWareHouse'))
    return
  }

  editRow.value = null
  editing.value = 'add'
}

function edit(r: AutoStoresRow): void {
  editRow.value = r
  editing.value = r.Id
}

async function toggle(r: AutoStoresRow): Promise<void> {
  try {
    await call('setAutoStoresEnable', { id: r.Id, enable: !r.IsEnable })
  } catch (e) {
    console.error('[as] 切换启用失败', e)
  }
}

async function del(r: AutoStoresRow): Promise<void> {
  //确认框在 C# 侧（DeleteAutoStores_Dialog），与 WinForms 同一条链路
  try {
    await call('deleteAutoStores', { id: r.Id })
  } catch (e) {
    console.error('[as] 删除失败', e)
    pushToast('error', String(e))
  }
}

/** 导入(8) / 导出(5) / 清空(7)，以及右键的四个移动 —— action 取 SystemConfig.ListAction 的值。 */
async function listAction(action: number, id?: string): Promise<void> {
  try {
    await call('autoStoresAction', { action, id: id ?? '' })
  } catch (e) {
    console.error('[as] 列表操作失败', e)
    pushToast('error', String(e))
  }
}

/* ── 右键菜单：只调顺序，作用于开菜单的那一行 ───────────────── */

const menuAt = ref<{ x: number; y: number } | null>(null)
const menuRow = ref<AutoStoresRow | null>(null)

function openMenu(e: MouseEvent, r: AutoStoresRow): void {
  menuRow.value = r
  menuAt.value = { x: e.clientX, y: e.clientY }
}

const menuItems = computed<MenuItem[]>(() => {
  const i = menuRow.value ? rules.value.findIndex((x) => x.Id === menuRow.value!.Id) : -1
  const first = i <= 0
  const last = i < 0 || i >= rules.value.length - 1

  //已经在顶上就压暗置顶 / 上移，在底下就压暗下移 / 置底 —— 点了没反应比看不见更糟
  return [
    { id: 'top', label: t('lst.top'), icon: ICON.top, disabled: first },
    { divider: true },
    { id: 'up', label: t('lst.up'), icon: ICON.up, disabled: first },
    { id: 'down', label: t('lst.down'), icon: ICON.down, disabled: last },
    { divider: true },
    { id: 'bottom', label: t('lst.bottom'), icon: ICON.bottom, disabled: last },
  ]
})

const MOVE_OF: Record<string, number> = { top: 0, up: 1, down: 2, bottom: 3 }

function onMenuPick(id: string): void {
  const r = menuRow.value
  const action = MOVE_OF[id]
  if (!r || action === undefined) return
  void listAction(action, r.Id)
}
</script>

<template>
  <SettingsModal
    :open="props.open"
    :title="t('as.title')"
    subtitle="Auto Store"
    :busy="busy"
    :error="error"
    @update:open="emit('update:open', $event)"
    @save="save"
  >
    <div class="row">
      <div class="k">{{ t('as.enable') }}</div>
      <div class="v">
        <button class="chk" :class="{ on: enable }" @click="enable = !enable">
          <i />{{ t('set.speedModeOn') }}
        </button>
        <!-- 琥珀色：这是个会让人以为「设了怎么没用」的坑，不能读成普通旁注 -->
        <span class="notice">{{ t('as.enableNotice') }}</span>
      </div>
    </div>

    <!--
      仓库上限。放在自动入库这一屏，是因为无界增长只有自动入库这条路走得出来
      （右键「添加到仓库」是人一条条点的）—— 但上限本身<b>管所有入库路径</b>。
    -->
    <div class="row">
      <div class="k">{{ t('as.limit') }}</div>
      <div class="v">
        <button class="chk" :class="{ on: limit }" @click="limit = !limit">
          <i />{{ t('as.limitOn') }}
        </button>
      </div>
    </div>

    <div class="row">
      <div class="k">{{ t('set.keepRows') }}</div>
      <div class="v">
        <input v-model.number="limitValue" class="inp num" type="number"
               min="1" max="1000000" :disabled="!limit">
        <span class="tip">{{ t('as.limitHint') }}</span>
      </div>
    </div>

    <p class="hint">{{ t('as.lead') }}</p>

    <div class="lbar">
      <span class="cap">{{ t('as.rules') }} <span class="n">{{ rules.length }}</span></span>

      <span class="grow" />

      <button class="mini" @click="add">{{ t('as.add') }}</button>
      <button class="mini" @click="listAction(8)">{{ t('flt.import') }}</button>
      <button class="mini" :disabled="!rules.length" @click="listAction(5)">{{ t('lst.export') }}</button>
      <button class="mini danger" :disabled="!rules.length" @click="listAction(7)">{{ t('flt.clearAll') }}</button>
    </div>

    <!-- 总开关关着时把表压暗但<b>不锁</b>：规则可以先备好、再开开关 -->
    <div class="tbl" :class="{ off: !enable }">
      <div class="thead">
        <span class="ck">{{ t('col.enable') }}</span>
        <span>{{ t('as.head') }}</span>
        <span>{{ t('as.wareHouse') }}</span>
        <span />
      </div>

      <div class="tbody">
        <div v-if="!rules.length" class="empty">{{ t('as.empty') }}</div>

        <div v-for="r in rules" v-else :key="r.Id" class="trow" :class="{ off: !r.IsEnable, aim: menuRow?.Id === r.Id && !!menuAt }"
             @dblclick="edit(r)" @contextmenu.prevent="openMenu($event, r)">
          <span class="ck">
            <button class="chk" :class="{ on: r.IsEnable }" :title="t('col.enable')" @click.stop="toggle(r)"><i /></button>
          </span>
          <span class="head" :title="r.PacketHead">{{ r.PacketHead }}</span>
          <!-- 仓库被删了规则还在：标红说明，而不是留空 —— 留空看着像没填 -->
          <span class="house" :class="{ gone: !nameOf(r.WareHouseId) }">
            {{ nameOf(r.WareHouseId) || t('as.gone') }}
          </span>
          <span class="ops">
            <button class="op" :title="t('as.edit')" @click.stop="edit(r)">
              <svg viewBox="0 0 24 24"><path d="M4 20h4L19 9l-4-4L4 16v4z" /></svg>
            </button>
            <button class="op del" :title="t('lst.delete')" @click.stop="del(r)">
              <svg viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg>
            </button>
          </span>
        </div>
      </div>
    </div>

    <ContextMenu :at="menuAt" :items="menuItems" @pick="onMenuPick" @close="menuAt = null" />

    <AutoStoresEdit
      :target="editing"
      :row="editRow"
      @close="editing = null"
    />
  </SettingsModal>
</template>

<style scoped>
.row {
  display: grid;
  grid-template-columns: 132px 1fr;
  align-items: center;
  gap: 12px;
  padding: 3px 20px;
  min-height: 30px;
  margin-top: 10px;
}

.row > .k { font-size: var(--fs-body); color: var(--muted); }
.row > .v { display: flex; align-items: center; gap: 14px; min-width: 0; }

.notice { font-size: var(--fs-small); color: var(--amber); }

.hint { padding: 0 20px; margin: 4px 0 12px; font-size: var(--fs-small); color: var(--dim2); line-height: 1.6; }

/* 与 FireWallSetting 同一份勾选框写法 */
/* 基样式在 style.css 的「勾选框 / 单选框」；这一屏勾选框成片出现，用压暗的绿 */
.chk { --chk-fill: var(--chk-on); --chk-fill-rgb: var(--chk-on-rgb); --chk-tint: 14%; }

/* ── 规则表 ── */

.lbar { display: flex; align-items: center; gap: 8px; padding: 0 20px 6px; }
.grow { flex: 1; }

.cap {
  font-family: var(--share);
  font-size: var(--fs-label);
  letter-spacing: .14em;
  text-transform: uppercase;
  color: var(--dim2);
}

.cap .n { color: var(--cyan); }

/* 小按钮的样式在 style.css 的 .mini */

.tbl {
  margin: 0 20px 6px;
  border: 1px solid var(--border);
  background: rgb(var(--inset-rgb) / 20%);
  max-height: 260px;
  overflow: auto;
}

/* 关着总开关只压暗，不 pointer-events: none —— 规则得能先编好 */
.tbl.off .trow { opacity: .55; }

.thead,
.trow {
  display: grid;
  grid-template-columns: 44px minmax(140px, 1fr) minmax(120px, 1fr) 56px;
  align-items: center;
  gap: 10px;
  padding: 0 10px;
  font-size: var(--fs-body);
}

.thead {
  position: sticky;
  top: 0;
  z-index: 2;
  height: var(--th-h);
  background: var(--panel);
  border-bottom: 1px solid var(--border);
  font-family: var(--share);
  font-size: var(--th-size);
  letter-spacing: .14em;
  text-transform: uppercase;
  color: var(--th-fg);
  white-space: nowrap;
}

.empty { padding: 26px 0; text-align: center; color: var(--muted); font-size: var(--fs-body); }

.trow { height: 30px; color: var(--soft); cursor: default; }
.trow:hover { background: rgb(var(--tint-rgb) / 4%); }
.trow.aim { background: rgb(var(--cyan-rgb) / 8%); }
.trow > span { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }

/* 禁用的行压暗，但勾选框那格除外 —— 它正是把这一行点亮的控件 */
.trow.off > span:not(.ck) { opacity: .45; }

.ck { display: flex; align-items: center; justify-content: center; }

.head { font-family: var(--mono); color: var(--cyan); letter-spacing: .04em; }
.house { color: var(--dim3); }
.house.gone { color: var(--danger); font-size: var(--fs-small); }

.ops { display: flex; align-items: center; justify-content: flex-end; gap: 2px; }

.op {
  display: inline-flex;
  padding: 3px;
  background: transparent;
  border: 0;
  color: var(--muted);
  cursor: pointer;
}

.op svg { width: 14px; height: 14px; fill: none; stroke: currentColor; stroke-width: 1.6; }
.op:hover { color: var(--cyan); }
.op.del:hover { color: var(--danger); }

/*
  表头每格与数据格同名（对齐规则靠这个），于是数据列的字体 / 字号 / 颜色
  （.notes 12px、.ad / .dt 等宽字、.cnt 青色……）会一并漏进表头，看着就是「备注」「数据」比别的表头大。
  这里按格子把它们收回来：表头只认表头自己那一份。(0,2,1) 压得过任何单类名的列规则。
*/
.thead > span {
  /*
    表头字形偏上 2px（像素级实测：Share Tech Mono 10.5px 在 30px 表头里，中文 / 英文的墨迹中心都在盒中心上方约 2px）。
    格子是 grid / flex 项，内边距只在上面补 4px 就把内容框中心压下 2px，对文本与 flex 居中的格子都成立。
    改字号 / 字体 / 表头高度后要重新量（SVG foreignObject 逐行扫像素那套；量尺页 dev-headers.html 已于 2.1.9 随开发工具删除，要用从 git 历史取回）。
  */
  padding-top: 4px;
  font-family: inherit;
  font-size: inherit;
  font-weight: inherit;
  letter-spacing: inherit;
  text-transform: inherit;
  color: inherit;
}
</style>
