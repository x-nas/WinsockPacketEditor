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
import { computed, reactive, ref, watch } from 'vue'
import { call } from '../../bridge'
import { FeedList, type IPRuleRow } from '../../bridge/types'
import { flagSrc } from '../../flags'
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

/** 当前看的是哪张表。两张表结构一样，用一个开关切，不并排摆 —— 并排的话每张都只剩一半宽。 */
const tab = ref<'white' | 'black'>('white')
const source = computed(() => (tab.value === 'black' ? black.value : white.value))
const isBlack = computed(() => tab.value === 'black')

/*
  表头排序。WinForms 那张表只有「IP地址」与「过期时间」两列带 SortMode，这里照着给这两列。

  <b>IP 用 StartIP 排</b>：DTO 里现成的数字（C# 侧解析好的段起点），
  比在前端拆点四段准 —— 名单里可以写成 IP 段（"10.0.0.1-10.0.0.99"），那种拆不出四段。
  StartIP 为 0 的（解析失败）退回自己拆一次，至少同类的挨在一起。

  「永久有效」的没有到期时间，按最大值排到最后 —— 与账号列表「永不过期」同一个处理。
*/
const sort = useSort<IPRuleRow>(source, {
  ip: (r) => (r.StartIP || ipKey(r.IPAddress)),
  expiry: (r) => (r.IsExpiry ? timeKey(r.ExpiryTime) : Number.MAX_SAFE_INTEGER),
})

const rows = sort.sorted

/*
  列宽可拖 —— 「客户端地」装的是「中国-上海-上海徐汇区电信」这种长串，
  固定宽度下只看得到省略号。做法照搬 PacketList：手柄挂在表头右边界、
  监听挂 window（快拖时鼠标会跑出那条 7px 窄条）、双击恢复默认。
  <b>不持久化</b>，与封包列表一致 —— 关掉弹窗回默认值。
*/
const DEF_W: Record<string, number> = { ip: 150, loc: 160 }
const colW = reactive<Record<string, number>>({ ...DEF_W })
const MIN_COL = 60

//「过期时间」用 1fr 吃掉剩余宽度，所以它和操作列都不给手柄
const gridCols = computed(
  () => `${colW.ip}px ${colW.loc}px 64px minmax(110px, 1fr) 56px`)

/*
  表头与每一行共用这一份样式，宽度才必然一致。

  ⚠️ <b>不要用 width: max-content</b>：那样带 1fr 的那一列会按<b>各自</b>的内容
  去撑宽，地址长的行就比别的行宽，与表头对不齐。改成给一个共同的 min-width
  （固定列之和 + 4 条 10px 间隙），容器够宽时 1fr 吃掉余量、不够宽时一起横向滚。
*/
const rowStyle = computed(() => ({
  gridTemplateColumns: gridCols.value,
  minWidth: `${colW.ip + colW.loc + 64 + 110 + 56 + 40}px`,
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

const editing = ref<string | null>(null)
const editRow = ref<IPRuleRow | null>(null)

function add(): void {
  editRow.value = null
  editing.value = 'add'
}

function edit(r: IPRuleRow): void {
  editRow.value = r
  editing.value = r.IPAddress
}

async function del(r: IPRuleRow): Promise<void> {
  /*
    确认框在 C# 侧弹（DeleteIPRule_Dialog 里 await UI.Confirm），与账号删除、
    各列表的删除同一条路数 —— 全项目的确认框只有 ConfirmDialog 这一个渲染处。

    原来这里用的是浏览器原生 window.confirm：能用，但那是系统画的灰色方框，
    在这套深色皮肤里明显是外来物，而且它会同步阻塞整个渲染进程。
    底层的 DeleteIPRule 仍然不弹框（批量路径要能直接调它），弹框的是新加的那层。
  */
  try {
    await call('deleteIPRule', { black: isBlack.value, ip: r.IPAddress })
  } catch (e) {
    console.error('[fw] 删除失败', e)
    pushToast('error', String(e))
  }
}

/** 导入 / 导出 / 清空 —— action 取 SystemConfig.ListAction 的值。 */
async function listAction(action: number): Promise<void> {
  try {
    await call('ipRuleAction', { black: isBlack.value, action })
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
  <SettingsModal
    :open="props.open"
    :title="t('set.firewall')"
    subtitle="Controls/FireWallSetting"
    :busy="busy"
    :error="error"
    @update:open="emit('update:open', $event)"
    @save="save"
  >    <div class="setf" style="--setf-k: 132px">

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

    <div class="grp">{{ t('fw.grp.lists') }}</div>

    <div class="lbar" :class="{ off: !form.enable }">
      <button class="tab" :class="{ on: !isBlack }" :disabled="!form.enable" @click="tab = 'white'">
        {{ t('fw.whiteList') }} <span class="n">{{ white.length }}</span>
      </button>
      <button class="tab" :class="{ on: isBlack }" :disabled="!form.enable" @click="tab = 'black'">
        {{ t('fw.blackList') }} <span class="n">{{ black.length }}</span>
      </button>

      <span class="grow" />

      <button class="mini" :disabled="!form.enable" @click="add">{{ t('fw.add') }}</button>
      <button class="mini" :disabled="!form.enable" @click="listAction(8)">{{ t('flt.import') }}</button>
      <button class="mini" :disabled="!form.enable || !rows.length" @click="listAction(5)">{{ t('lst.export') }}</button>
      <button class="mini danger" :disabled="!form.enable || !rows.length" @click="listAction(7)">{{ t('flt.clearAll') }}</button>
    </div>

    <div class="tbl" :class="{ off: !form.enable }">
      <div class="thead" :style="rowStyle">
        <span class="so" :class="{ on: sort.active('ip') }" @click="sort.toggle('ip')">
          {{ t('cli.ip') }}<i class="ar">{{ sort.mark('ip') }}</i>
          <!-- @click.stop：拖完列宽松手会补派发一次 click，不拦住就顺带排了一次序 -->
          <i class="grip" :title="t('col.resizeHint')"
             @click.stop
             @mousedown.prevent.stop="startResize($event, 'ip')"
             @dblclick.prevent.stop="resetWidth('ip')" />
        </span>
        <span>
          {{ t('col.clientLoc') }}
          <i class="grip" :title="t('col.resizeHint')"
             @click.stop
             @mousedown.prevent.stop="startResize($event, 'loc')"
             @dblclick.prevent.stop="resetWidth('loc')" />
        </span>
        <span>{{ t('fw.effect') }}</span>
        <span class="so" :class="{ on: sort.active('expiry') }" @click="sort.toggle('expiry')">{{ t('fw.expiryTime') }}<i class="ar">{{ sort.mark('expiry') }}</i></span>
        <span />
      </div>

      <div class="tbody">
        <div v-if="!rows.length" class="empty">{{ t('fw.emptyList') }}</div>

        <div v-for="(r, i) in rows" v-else :key="r.IPAddress + '|' + i" class="trow"
             :style="rowStyle" @dblclick="edit(r)">
          <span class="ip">{{ r.IPAddress }}</span>
          <!-- 国旗 / 局域网图标跟着所属地走，与客户端列表、封包列表一致 -->
          <span class="loc">
            <img class="flag" :src="flagSrc(r.IPLocation)" alt="" width="16" height="16"
                 loading="eager" decoding="sync">
            <span class="t" :title="r.IPLocation">{{ r.IPLocation }}</span>
          </span>
          <span class="num">{{ r.EffectCount }}</span>
          <!--
            不到期的写「永久有效」，而不是 8888/12/31 那个哨兵日期，也不是一条短横 ——
            短横在别的表里读作「没数据」，这里恰恰是一种明确的状态。
          -->
          <span class="exp" :class="{ never: !r.IsExpiry }">
            {{ r.IsExpiry ? r.ExpiryTime : t('fw.never') }}
          </span>
          <span class="ops">
            <button class="op" :title="t('fw.edit')" @click.stop="edit(r)">
              <svg viewBox="0 0 24 24"><path d="M4 20h4L19 9l-4-4L4 16v4z" /></svg>
            </button>
            <button class="op del" :title="t('lst.delete')" @click.stop="del(r)">
              <svg viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg>
            </button>
          </span>
        </div>
      </div>
    </div>

    <IPRuleEdit
      :target="editing"
      :black="isBlack"
      :is-expiry="editRow?.IsExpiry ?? false"
      :expiry="editRow?.ExpiryTime ?? ''"
      @close="editing = null"
      @saved="() => {}"
    />
    </div>
  </SettingsModal>
</template>

<style scoped>

.k2 { font-size: 12.5px; color: var(--muted); }
/* 时长后面那句注解，比「分钟」再暗一档，读成旁注而不是第二个标签 */
.k2.dim { color: #6b7280; }

/* ── 名单 ── */

.lbar { display: flex; align-items: center; gap: 8px; padding: 0 20px 6px; }
.lbar.off, .tbl.off { opacity: .45; pointer-events: none; }
.grow { flex: 1; }

/* 白 / 黑名单二选一，不并排 —— 并排每张只剩一半宽，而 IP + 所属地 + 时间本来就不窄 */
.tab {
  display: inline-flex;
  align-items: center;
  gap: 6px;
  padding: 4px 12px;
  background: transparent;
  border: 1px solid var(--border);
  color: var(--muted);
  font-size: 12px;
  cursor: pointer;
}

.tab.on { border-color: var(--cyan); color: var(--cyan); background: rgb(0 212 255 / 10%); }
.tab .n { font-family: var(--share); font-size: 9.5px; opacity: .8; }

.mini {
  padding: 8px 10px 6px;   /* 上 +1 下 -1：字形在 em 框里偏上 1px（上伸 9 / 下伸 3，实测），补回来 */
  background: transparent;
  border: 1px solid var(--border);
  color: var(--muted);
  font-family: var(--share);
  font-size: var(--btn-size);
  /* 显式 1：Share Tech Mono 在 line-height: normal 下会把行距全压在字的下面，字号一大就明显偏上（实测） */
  line-height: 1;
  letter-spacing: .12em;
  text-transform: uppercase;
  cursor: pointer;
}

.mini:hover:not(:disabled) { border-color: var(--cyan); color: var(--cyan); }
.mini.danger:hover:not(:disabled) { border-color: var(--danger); color: var(--danger); }
.mini:disabled { opacity: .4; cursor: default; }

/*
  表头与数据行同在<b>一个</b>滚动容器里：sticky 只锁纵向，横向自然跟着一起滚。
  分开滚（表头一个容器、表体另一个）在列被拖宽后必然错位 —— PacketList 也是这么做的。
*/
.tbl {
  margin: 0 20px 6px;
  border: 1px solid var(--border);
  background: rgb(0 0 0 / 20%);
  max-height: 218px;
  overflow: auto;
}

.thead,
.trow {
  display: grid;
  /* 实际模板由 :style 的 gridCols 给，这里只留一份兜底 */
  grid-template-columns: 150px 160px 64px minmax(110px, 1fr) 56px;
  /* 实际最小宽度由 rowStyle 给，见脚本里那段说明 */
  align-items: center;
  gap: 10px;
  padding: 0 10px;
  font-size: 12.5px;
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
  color: #a8b2c0;
  white-space: nowrap;
}

/* 定高 + 滚动：名单可能几百条，不能让它把弹窗撑到屏幕外 */
.tbody { min-width: 100%; }

.empty { padding: 26px 0; text-align: center; color: var(--muted); font-size: 12px; }

.trow { height: 30px; color: #cbd5cc; cursor: default; }
.trow:hover { background: rgb(255 255 255 / 4%); }
.trow > span,
/* 表头也要：列宽是按中文的字数定的，「过期时间」四个字换成「Истекает」就装不下 */
.thead > span { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }

.ip { color: var(--cyan); font-family: var(--mono); }
.loc { display: flex; align-items: center; gap: 6px; color: #94a3b8; }

/*
  省略号要挂在<b>内层</b>：.loc 成了 flex 容器之后，text-overflow 对它本身不再起作用，
  而这一列装的是「中国-上海-上海徐汇区电信」这种长串，截断是必需的。
*/
.loc .t { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; min-width: 0; }

/* 三种原始尺寸（16×12 国旗 / 20×20 组织旗 / 16×16 局域网），定框 + contain 才不会压扁 */
.flag { width: 16px; height: 16px; object-fit: contain; flex: none; }
.num { color: #94a3b8; text-align: center; font-variant-numeric: tabular-nums; }
/* 表头要跟着数据格一起居中，只居中数据会看着像错位 */
.thead > span:nth-child(3) { text-align: center; }

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
.exp { color: var(--muted); font-family: var(--mono); font-size: 12px; }
/* 「永久有效」是句话不是时间戳，等宽字体反而别扭 */
.exp.never { font-family: inherit; font-size: 12.5px; color: #7f8b9c; }

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
    改字号 / 字体 / 表头高度后要重新量（dev-headers.html + SVG foreignObject 逐行扫像素那套）。
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
