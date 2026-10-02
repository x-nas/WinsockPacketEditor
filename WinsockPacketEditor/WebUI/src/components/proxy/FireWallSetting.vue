<script setup lang="ts">
/*
  防火墙设置 —— 对应 WinForms 的 Controls/FireWallSetting + FireWallRules。

  【为什么优先做这一屏】客户端列表已经能「加入白 / 黑名单」了，
  但在此之前外壳里<b>没有任何地方能看到或删除</b>这两张表 ——
  可以把一个 IP 永久拉黑，然后没法解除。先补上这个缺口。

  【规则那五项并进来了】WinForms 是在设置窗上再开一个 Modal（FireWallRules）。
  弹窗套弹窗在这套皮肤下要处理两层 inert 与焦点，而那五项本来就属于防火墙，
  没必要分开。

  【名单不走桥读】lstWhiteList / lstBlackList 已在 FeedPump 的推送流里，
  这里只读前端副本；桥只提供改的入口（saveIPRule / deleteIPRule / ipRuleAction）。
*/
import { computed, onBeforeUnmount, reactive, ref, watch } from 'vue'
import { call } from '../../bridge'
import { FeedList, type IPRuleRow } from '../../bridge/types'
import { t } from '../../i18n'
import { useList } from '../../stores/lists'
import { ipKey, timeKey, useSort } from '../../useSort'
import { pushToast } from '../../stores/toast'
import SettingsModal from './SettingsModal.vue'
import IPRuleEdit from './IPRuleEdit.vue'

const props = defineProps<{ open: boolean }>()
const emit = defineEmits<{ (e: 'update:open', v: boolean): void }>()

const busy = ref(false)
const error = ref('')

const white = useList<IPRuleRow>(FeedList.WhiteList)
const black = useList<IPRuleRow>(FeedList.BlackList)

interface Form {
  enable: boolean
  whiteMode: boolean
  autoWhiteAuthOk: boolean
  autoBlackUnsupport: boolean
  autoBlackAuthFail: boolean
  autoBlackMinutes: number
  autoClearExpiry: boolean
}

const form = ref<Form>({
  enable: false,
  whiteMode: false,
  autoWhiteAuthOk: false,
  autoBlackUnsupport: false,
  autoBlackAuthFail: false,
  autoBlackMinutes: 30,
  autoClearExpiry: false,
})

/*
  两张表各自排序（2026-10-02 改成「白名单 + 黑名单两张表并排」，不再用页签二选一）。
  排序状态原本只有一份，现在每张表一份 —— 否则点白名单表头会把黑名单也一起排了。

  IP 用 StartIP 排：DTO 里现成的数字（C# 侧解析好的段起点），比在前端拆点四段准 ——
  名单里可以写成 IP 段（"10.0.0.1-10.0.0.99"），那种拆不出四段。StartIP 为 0 的退回自己拆一次。
  「永久有效」的没有到期时间，按最大值排到最后 —— 与账号列表「永不过期」同一个处理。
*/
const sortWhite = useSort<IPRuleRow>(white, {
  ip: (r) => (r.StartIP || ipKey(r.IPAddress)),
  expiry: (r) => (r.IsExpiry ? timeKey(r.ExpiryTime) : Number.MAX_SAFE_INTEGER),
})

const sortBlack = useSort<IPRuleRow>(black, {
  ip: (r) => (r.StartIP || ipKey(r.IPAddress)),
  expiry: (r) => (r.IsExpiry ? timeKey(r.ExpiryTime) : Number.MAX_SAFE_INTEGER),
})

const whiteRows = sortWhite.sorted
const blackRows = sortBlack.sorted

/*
  列宽可拖 —— 「IP 地址」装的是 IP 段这种长串，固定宽度下只看得到省略号。
  做法照搬 PacketList：手柄挂在表头右边界、监听挂 window（快拖时鼠标会跑出那条 7px 窄条）、
  双击恢复默认。<b>不持久化</b>，与封包列表一致 —— 关掉弹窗回默认值。
*/
const DEF_W: Record<string, number> = { ip: 150 }
const colW = reactive<Record<string, number>>({ ...DEF_W })
const MIN_COL = 60

//「过期时间」用 1fr 吃掉剩余宽度，所以它和操作列都不给手柄
const gridCols = computed(() => `${colW.ip}px minmax(96px, 1fr) 52px`)

/*
  表头与每一行共用这一份样式，宽度才必然一致。

  ⚠️ <b>不要用 width: max-content</b>：那样带 1fr 的那一列会按<b>各自</b>的内容
  去撑宽，地址长的行就比别的行宽，与表头对不齐。改成给一个共同的 min-width
  （固定列之和 + 4 条 10px 间隙 + 自己的左右内边距），
  容器够宽时 1fr 吃掉余量、不够宽时一起横向滚。

  ⚠️ <b>最后那 20 是 .thead / .trow 自己的 padding: 0 10px。</b>
  全局 box-sizing 是 border-box，min-width <b>含内边距</b> —— 漏掉它，
  声明的 580 里只有 560 留给 grid，而 grid 要 580，于是元素被内容撑开，
  声明值与实际宽度对不上。（这不是滚动条的成因，成因是弹窗宽度，见下面 width。）
*/
const GRID_PAD = 20
const rowStyle = computed(() => ({
  gridTemplateColumns: gridCols.value,
  minWidth: `${colW.ip + 96 + 52 + 20 + GRID_PAD}px`,
}))

let drag: { key: string; x: number; w: number } | null = null

function onDragMove(e: MouseEvent): void {
  if (!drag) return
  colW[drag.key] = Math.max(MIN_COL, Math.round(drag.w + e.clientX - drag.x))
}

function onDragUp(): void {
  drag = null
  window.removeEventListener('mousemove', onDragMove)
  window.removeEventListener('mouseup', onDragUp)
  document.body.classList.remove('col-resizing')
}

//拖着列宽时按 Esc 关掉弹窗：监听与整页的 col-resizing 不收掉就一直挂着
onBeforeUnmount(() => { if (drag) onDragUp() })

function startResize(e: MouseEvent, key: string): void {
  drag = { key, x: e.clientX, w: colW[key] }

  //整页禁选 + 统一光标，否则拖动会把表头文字刷成选中态
  document.body.classList.add('col-resizing')
  window.addEventListener('mousemove', onDragMove)
  window.addEventListener('mouseup', onDragUp)
}

/** 双击手柄恢复默认宽度 —— 拖乱了不必去猜原来是多少。 */
function resetWidth(key: string): void { colW[key] = DEF_W[key] }

watch(() => props.open, async (on) => {
  if (!on) return

  error.value = ''

  try {
    form.value = await call<Form>('getFireWall')
  } catch (e) {
    console.error('[fw] 读取防火墙设置失败', e)
  }
})

/* ── 名单的增删改 ────────────────────────────────────────── */

const editingWhite = ref<string | null>(null)
const editingBlack = ref<string | null>(null)
const editWhiteRow = ref<IPRuleRow | null>(null)
const editBlackRow = ref<IPRuleRow | null>(null)

function add(black: boolean): void {
  if (black) { editBlackRow.value = null; editingBlack.value = 'add' }
  else { editWhiteRow.value = null; editingWhite.value = 'add' }
}

function edit(r: IPRuleRow, black: boolean): void {
  if (black) { editBlackRow.value = r; editingBlack.value = r.IPAddress }
  else { editWhiteRow.value = r; editingWhite.value = r.IPAddress }
}

async function del(r: IPRuleRow, black: boolean): Promise<void> {
  /*
    确认框在 C# 侧弹（DeleteIPRule_Dialog 里 await UI.Confirm），与账号删除、
    各列表的删除同一条路数 —— 全项目的确认框只有 ConfirmDialog 这一个渲染处。

    原来这里用的是浏览器原生 window.confirm：能用，但那是系统画的灰色方框，
    在这套深色皮肤里明显是外来物，而且它会同步阻塞整个渲染进程。
    底层的 DeleteIPRule 仍然不弹框（批量路径要能直接调它），弹框的是新加的那层。
  */
  try {
    await call('deleteIPRule', { black, ip: r.IPAddress })
  } catch (e) {
    console.error('[fw] 删除失败', e)
    pushToast('error', String(e))
  }
}

/** 导入 / 导出 / 清空 —— action 取 SystemConfig.ListAction 的值。 */
async function listAction(action: number, black: boolean): Promise<void> {
  try {
    await call('ipRuleAction', { black, action })
  } catch (e) {
    console.error('[fw] 列表操作失败', e)
    pushToast('error', String(e))
  }
}

async function save(): Promise<void> {
  busy.value = true
  error.value = ''

  try {
    const r = await call<{ ok: boolean; error: string }>('saveFireWall', {
      ...form.value,
      autoBlackMinutes: Number(form.value.autoBlackMinutes),
    })

    if (!r?.ok) {
      error.value = r?.error || ''
      return
    }

    emit('update:open', false)
  } catch (e) {
    console.error('[fw] 保存防火墙设置失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}
</script>

<template>
  <!--
    ⚠️ <b>:width 是必须的，不能用默认的 620。</b>
    这张名单有 5 列（IP / 客户端地 / 生效 / 过期时间 / 操作），默认列宽加上间距与内边距
    要 600px，而 620 的弹窗只留得出 576 —— 于是<b>名单为空时也挂着一根横向滚动条</b>
    （表头一直在，它自己就撑破了容器）。用户直接问了这根线是哪来的。

    700 之后可用宽度 656：既装得下，「过期时间」那列（minmax(110, 1fr)）还能吃到 166px，
    刚好显示完整的 <b>2026-09-09 15:49:00</b> 而不截断。

    ⚠️ 横向滚动本身要留着 —— IP / 客户端地两列是<b>可以拖宽</b>的，
    拖宽之后出滚动条是对的；这里治的只是「默认状态就溢出」。
  -->
  <!--
    ⚠️ 宽度 700 → 770。两笔加起来的：分区卡的左右外边距 + 边框吃掉 <b>42px</b>，
    「生效」那列为俄语从 64 加宽到 88 又吃掉 <b>24px</b> —— 不补回来，
    「过期时间」那个 1fr 列会缩到 132px，而完整的 2026-09-06 08:00:00 要 143px。
    1024 CSS 宽（125% 缩放）下仍在 max-width: calc(100vw - 64px) = 960 之内。

    ⚠️⚠️ <b>注释不能待在开标签的属性区里</b> —— Vue 会把它当成一串属性名，
    报的却是「缺少 title 属性」，看不出是注释的事。真栽过一次。
  -->
  <SettingsModal
    :open="props.open"
    :width="770"
    :title="t('set.firewall')"
    subtitle="Access Control"
    :busy="busy"
    :error="error"
    :hint="form.enable ? undefined : t('fw.listIdle')"
    @update:open="emit('update:open', $event)"
    @save="save"
  >    <div class="setf" style="--setf-k: 132px">

    <section class="sec">
    <div class="grp">{{ t('fw.grp.main') }}</div>

    <div class="row">
      <div class="k">{{ t('fw.enable') }}</div>
      <div class="v">
        <button class="chk" :class="{ on: form.enable }" @click="form.enable = !form.enable">
          <i />{{ t('set.speedModeOn') }}
        </button>
      </div>
    </div>

    <!-- 总开关关掉时，下面所有东西都压暗 —— 与 WinForms 的 EnableFireWall_Changed 一致 -->
    <div class="row" :class="{ off: !form.enable }">
      <div class="k">{{ t('fw.mode') }}</div>
      <div class="v">
        <button class="rd" :class="{ on: form.whiteMode }" :disabled="!form.enable"
                @click="form.whiteMode = true"><i />{{ t('fw.whiteMode') }}</button>
        <button class="rd" :class="{ on: !form.whiteMode }" :disabled="!form.enable"
                @click="form.whiteMode = false"><i />{{ t('fw.blackMode') }}</button>
      </div>
    </div>

    <p class="hint">{{ form.whiteMode ? t('fw.whiteModeHint') : t('fw.blackModeHint') }}</p>
    </section>

    <section class="sec">
    <div class="grp">{{ t('fw.grp.rules') }}</div>

    <div class="row" :class="{ off: !form.enable }">
      <div class="k">{{ t('fw.autoWhite') }}</div>
      <div class="v">
        <button class="chk" :class="{ on: form.autoWhiteAuthOk }" :disabled="!form.enable"
                @click="form.autoWhiteAuthOk = !form.autoWhiteAuthOk">
          <i />{{ t('fw.authOk') }}
        </button>
      </div>
    </div>

    <!--
      两条规则各占一行。挤在一行时英文的「不支持的 Socks 协议」加后面的时长
      会把「分钟」挤到第二行，而那一行还带着左边的标签，看着像三件事缠在一起。
    -->
    <div class="row" :class="{ off: !form.enable }">
      <div class="k">{{ t('fw.autoBlack') }}</div>
      <div class="v">
        <button class="chk" :class="{ on: form.autoBlackAuthFail }" :disabled="!form.enable"
                @click="form.autoBlackAuthFail = !form.autoBlackAuthFail">
          <i />{{ t('fw.authFail') }}
        </button>
      </div>
    </div>

    <div class="row" :class="{ off: !form.enable }">
      <div class="k" />
      <div class="v">
        <button class="chk" :class="{ on: form.autoBlackUnsupport }" :disabled="!form.enable"
                @click="form.autoBlackUnsupport = !form.autoBlackUnsupport">
          <i />{{ t('fw.unsupport') }}
        </button>
      </div>
    </div>

    <!--
      时长<b>两条规则共用</b>：认证失败那条在 Operate.cs:5172 用它，
      不支持协议那条在 SocksSwitchReceiveFilter.cs:38 用的是同一个字段。
      WinForms 那个控件叫 nudAutoBlackList_UnSupport，名字容易让人以为只管后者。
    -->
    <div class="row" :class="{ off: !form.enable }">
      <div class="k">{{ t('fw.blockTime') }}</div>
      <div class="v">
        <input v-model.number="form.autoBlackMinutes" class="inp num" type="number"
               min="1" max="525600" :disabled="!form.enable">
        <span class="k2">{{ t('fw.minutes') }}</span>
        <span class="k2 dim">{{ t('fw.blockTimeHint') }}</span>
      </div>
    </div>

    <div class="row" :class="{ off: !form.enable }">
      <div class="k">{{ t('fw.autoClear') }}</div>
      <div class="v">
        <button class="chk" :class="{ on: form.autoClearExpiry }" :disabled="!form.enable"
                @click="form.autoClearExpiry = !form.autoClearExpiry">
          <i />{{ t('fw.expired') }}
        </button>
      </div>
    </div>
    </section>

    <!--
      2026-10-02：原来「03 名单」那张卡的外壳去掉了 —— 白名单直接是 03 区、黑名单是 04 区，
      黄字提示（防火墙没启用时的提醒）挪到页脚按钮区左侧。
      两张表各自排序、各自编辑弹窗；卡片底内边距归零、表底边框让掉 —— 表铺满卡片不留空隙。
    -->
    <section class="sec">
    <div class="grp">{{ t('fw.whiteList') }} <span class="cnt">{{ white.length }}</span></div>

    <div class="tbl">
      <div class="tbar">
        <button class="sbtn primary" @click="add(false)">{{ t('fw.add') }}</button>
        <span class="grow" />
        <button class="sbtn" @click="listAction(8, false)">{{ t('flt.import') }}</button>
        <button class="sbtn" :disabled="!whiteRows.length" @click="listAction(5, false)">{{ t('lst.export') }}</button>
        <button class="sbtn danger" :disabled="!whiteRows.length" @click="listAction(7, false)">{{ t('flt.clearAll') }}</button>
      </div>
      <div class="thead" :style="rowStyle">
        <span class="so" :class="{ on: sortWhite.active('ip') }" @click="sortWhite.toggle('ip')">
          {{ t('cli.ip') }}<i class="ar">{{ sortWhite.mark('ip') }}</i>
          <i class="grip" :title="t('col.resizeHint')"
             @click.stop
             @mousedown.prevent.stop="startResize($event, 'ip')"
             @dblclick.prevent.stop="resetWidth('ip')" />
        </span>
        <span class="so" :class="{ on: sortWhite.active('expiry') }" @click="sortWhite.toggle('expiry')">{{ t('fw.expiryTime') }}<i class="ar">{{ sortWhite.mark('expiry') }}</i></span>
        <span class="th-act">{{ t('col.ops') }}</span>
      </div>
      <div class="tbody">
        <div v-if="!whiteRows.length" class="empty">{{ t('fw.emptyList') }}</div>
        <div v-for="(r, i) in whiteRows" v-else :key="r.IPAddress + '|' + i" class="trow"
             :style="rowStyle" @dblclick="edit(r, false)">
          <span class="ip">{{ r.IPAddress }}</span>
          <span class="exp" :class="{ never: !r.IsExpiry }">
            {{ r.IsExpiry ? r.ExpiryTime : t('fw.never') }}
          </span>
          <span class="ops">
            <button class="op" :title="t('fw.edit')" @click.stop="edit(r, false)">
              <svg viewBox="0 0 24 24"><path d="M4 20h4L19 9l-4-4L4 16v4z" /></svg>
            </button>
            <button class="op del" :title="t('lst.delete')" @click.stop="del(r, false)">
              <svg viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg>
            </button>
          </span>
        </div>
      </div>
    </div>
    </section>

    <section class="sec">
    <div class="grp">{{ t('fw.blackList') }} <span class="cnt">{{ black.length }}</span></div>

    <div class="tbl">
      <div class="tbar">
        <button class="sbtn primary" @click="add(true)">{{ t('fw.add') }}</button>
        <span class="grow" />
        <button class="sbtn" @click="listAction(8, true)">{{ t('flt.import') }}</button>
        <button class="sbtn" :disabled="!blackRows.length" @click="listAction(5, true)">{{ t('lst.export') }}</button>
        <button class="sbtn danger" :disabled="!blackRows.length" @click="listAction(7, true)">{{ t('flt.clearAll') }}</button>
      </div>
      <div class="thead" :style="rowStyle">
        <span class="so" :class="{ on: sortBlack.active('ip') }" @click="sortBlack.toggle('ip')">
          {{ t('cli.ip') }}<i class="ar">{{ sortBlack.mark('ip') }}</i>
          <i class="grip" :title="t('col.resizeHint')"
             @click.stop
             @mousedown.prevent.stop="startResize($event, 'ip')"
             @dblclick.prevent.stop="resetWidth('ip')" />
        </span>
        <span class="so" :class="{ on: sortBlack.active('expiry') }" @click="sortBlack.toggle('expiry')">{{ t('fw.expiryTime') }}<i class="ar">{{ sortBlack.mark('expiry') }}</i></span>
        <span class="th-act">{{ t('col.ops') }}</span>
      </div>
      <div class="tbody">
        <div v-if="!blackRows.length" class="empty">{{ t('fw.emptyList') }}</div>
        <div v-for="(r, i) in blackRows" v-else :key="r.IPAddress + '|' + i" class="trow"
             :style="rowStyle" @dblclick="edit(r, true)">
          <span class="ip">{{ r.IPAddress }}</span>
          <span class="exp" :class="{ never: !r.IsExpiry }">
            {{ r.IsExpiry ? r.ExpiryTime : t('fw.never') }}
          </span>
          <span class="ops">
            <button class="op" :title="t('fw.edit')" @click.stop="edit(r, true)">
              <svg viewBox="0 0 24 24"><path d="M4 20h4L19 9l-4-4L4 16v4z" /></svg>
            </button>
            <button class="op del" :title="t('lst.delete')" @click.stop="del(r, true)">
              <svg viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg>
            </button>
          </span>
        </div>
      </div>
    </div>
    </section>
    <IPRuleEdit
      :target="editingWhite"
      :black="false"
      :is-expiry="editWhiteRow?.IsExpiry ?? false"
      :expiry="editWhiteRow?.ExpiryTime ?? ''"
      @close="editingWhite = null"
      @saved="() => {}"
    />
    <IPRuleEdit
      :target="editingBlack"
      :black="true"
      :is-expiry="editBlackRow?.IsExpiry ?? false"
      :expiry="editBlackRow?.ExpiryTime ?? ''"
      @close="editingBlack = null"
      @saved="() => {}"
    />
    </div>
  </SettingsModal>
</template>

<style scoped>

.k2 { font-size: var(--fs-body); color: var(--muted); }
/* 时长后面那句注解，比「分钟」再暗一档，读成旁注而不是第二个标签 */
.k2.dim { color: var(--muted); }

/* ── 名单 ── */

/* 条数跟在名单名后面，用等宽字，与工具条里那个计数同一套 */
.cnt { margin-left: 6px; font-family: var(--mono); font-size: var(--fs-caption); color: var(--muted); }

/*
  表体：<b>表直接铺满卡片</b> —— 表的左右边框去掉、卡片底内边距归零、
  表的底边框让掉（否则与卡片自己的下边框叠成双线），高度随行数自适应，
  行多时由 tbody 自己滚（max-height 兜底），不留缝隙。
*/
.tbl {
  margin: 6px 0 0;
  border-left: 0;
  border-right: 0;
  border-bottom: 0;
  background: rgb(var(--inset-rgb) / 20%);
  overflow: hidden;
}

/* 两张表的工具条：新增靠左（绿色，同映射设置的主按钮），导入 / 导出 / 清空靠右 */
.tbar { display: flex; align-items: center; gap: 8px; padding: 6px 14px; border-bottom: 1px solid var(--border); background: var(--card); }
.tbar .grow { flex: 1; }

.thead,
.trow {
  display: grid;
  /* 实际模板由 :style 的 gridCols 给，这里只留一份兜底 */
  grid-template-columns: 150px minmax(96px, 1fr) 52px;
  /* 实际最小宽度由 rowStyle 给，见脚本里那段说明 */
  align-items: center;
  gap: 8px;
  padding: 0 14px;
  font-size: var(--fs-body);
}

.thead {
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

.tbody { max-height: 178px; overflow-y: auto; }

.empty { padding: 26px 0; text-align: center; color: var(--muted); font-size: var(--fs-body); }

.trow { height: 30px; color: var(--soft); cursor: default; }
.trow:hover { background: rgb(var(--tint-rgb) / 4%); }
.trow > span,
/* 表头也要：列宽是按中文的字数定的，「过期时间」四个字换成「Истекает」就装不下 */
.thead > span { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }

.ip { color: var(--cyan); font-family: var(--mono); }

.num { color: var(--dim3); text-align: center; font-variant-numeric: tabular-nums; }

/* 操作列的表头：跟着「操作」两个字缩小，别跟着数据格的字号 */
.th-act { font-size: var(--fs-caption); text-align: right; }

/* 手柄要贴在表头格子的右边界上，格子得先能定位 */
.thead > span { position: relative; }

/*
  宽 7px 而不是 1px —— 1px 的靶子几乎点不中。它跨在相邻两列的分界处，
  各占一半，看上去仍是那条分隔线；hover 时显一条青线给反馈。
*/
.grip {
  position: absolute;
  top: 0;
  right: -8px;
  width: 7px;
  height: 100%;
  cursor: col-resize;
  z-index: 3;
}

.grip:hover::after {
  content: "";
  position: absolute;
  top: 4px;
  bottom: 4px;
  left: 3px;
  width: 1px;
  background: var(--cyan);
  box-shadow: 0 0 4px var(--cyan);
}
.exp { color: var(--muted); font-family: var(--mono); font-size: var(--fs-body); }
/* 「永久有效」是句话不是时间戳，等宽字体反而别扭 */
.exp.never { font-family: inherit; font-size: var(--fs-body); color: var(--dim4); }

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
