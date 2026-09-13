<script setup lang="ts">
/*
  账号列表 —— 对应 WinForms 的 Controls/AccountList（+ AccountEdit / AccountLocation 两个弹窗）。

  代理服务器的登录凭据表。开了「身份认证」之后，客户端要拿这里的用户名密码
  过 SOCKS5 的握手才连得上；没开则这张表不参与连接（顶部会说明这件事）。

  【数据从哪来】整表由 C# 推（FeedList.Account，走 B9d 的 FeedPump），
  这里只读那份副本。用户名搜索与到期区间筛选都在前端做 —— 这张表是几十到几百行，
  再走一次桥只是把同样的 LINQ 搬到另一侧（WinForms 那边走
  GetAccount_ByUserName / GetProxyAccount_ByExpireTime）。

  【和 WinForms 的三处不同】
  ① 不分页，改用虚拟滚动。那边是 20/30/50/100/200 的分页器，因为 AntdUI 的表格
     没有虚拟滚动；这里照搬 PacketList.vue 那套定高窗口，DOM 里永远只有约 40 行，
     几万个账号也能一路滚下去。见下面「为什么必须虚拟滚动」。
  ② 「链接数 / 设备数」在那边是右键菜单里的批量调整弹窗，这里并进了编辑弹窗 ——
     它们本来就是这个账号的属性，拆成两处只是历史原因。
  ③ 没有「在线 / 离线」筛选。状态列本来就写着在线与否，而这张表的常态是几十行、
     在线的往往是零星几个 —— 为它单开三个按钮，占掉的工具条宽度比省下的翻找多。

  【启用可以在列表里直接改】这一列一度去掉过：那时它和「选中」勾选框并排，
  两个长得一模一样，而点错的代价不对称 —— 一个只是选中，另一个会立刻让某个客户端连不上。
  <b>现在选中改成了按键（单击 / Ctrl / Shift，见 usePick.ts），那个理由不成立了</b>，
  所以加回来。全项目只剩一种勾选框，就是「启用」，一律是绿的。
  禁用的行仍然整行压暗（.row.off），但勾选框那格除外 —— 它正是用来点亮这一行的控件。
*/
import { computed, onBeforeUnmount, onMounted, ref, watch } from 'vue'
import { call } from '../../bridge'
import { FeedList, type AccountRow } from '../../bridge/types'
import { t } from '../../i18n'
import { useList } from '../../stores/lists'
import { useRowPick } from '../../usePick'
import { timeKey, useSort } from '../../useSort'
import { pushToast } from '../../stores/toast'
import ContextMenu from '../ContextMenu.vue'
import { ICON, type MenuItem } from '../menu'
import AccountAdjust from './AccountAdjust.vue'
import AccountBatch from './AccountBatch.vue'
import AccountEdit from './AccountEdit.vue'
import AccountLogins from './AccountLogins.vue'

const rows = useList<AccountRow>(FeedList.Account)

const q = ref('')

/*
  按到期日筛选 —— 对应 WinForms 的 dtpExpiryTime 区间 + 「查询」按钮
  （那边走 Operate 的 GetProxyAccount_ByExpireTime）。
  这里在前端做：整表副本本来就在手上，几十到几百行，走一趟桥只是把同样的比较搬到另一侧。
  两端各自可留空 —— 只填右边就是「这天之前到期的」，续期前最常问的就是这一句。
*/
const dFrom = ref('')
const dTo = ref('')

/** 编辑弹窗：null = 关；'' = 新增；其余 = 要改的那条的 Id。 */
const editing = ref<string | null>(null)
/** 登录记录弹窗看的是哪个账号。 */
const viewing = ref<AccountRow | null>(null)
const batch = ref(false)

/*
  身份认证关着的时候这张表不参与连接 —— 这是个很容易踩的坑：
  建好了账号却发现随便什么密码都能连上，回头查半天。顶部直说一句。
  只在进页面时取一次：改它要去代理设置弹窗，改完那边会自己提示。
*/
const authOn = ref(true)

onMounted(async () => {
  try {
    const s = await call<{ enableAuth: boolean }>('getProxySetting')
    authOn.value = !!s?.enableAuth
  } catch (e) {
    console.error('[acct] 读取代理设置失败', e)
  }
})

/*
  过期时间是 C# 侧格式化好的 "yyyy-MM-dd HH:mm:ss"（见 AccountRow.From_）。
  <b>不限制时存的是一个远期日期</b>，所以显示与判定都只看 IsExpiry，不去解析那个值 ——
  下面的区间筛选也因此先看 IsExpiry：「永不过期」不该落进任何一段区间里。
*/
function expiryText(r: AccountRow): string {
  return r.IsExpiry ? r.ExpiryTime : t('acct.never')
}

/**
 * 把 "yyyy-MM-dd HH:mm:ss" 解析成时间戳。
 * 换掉横杠是必须的：Safari 之外的引擎虽然认，但带横杠的日期串按 ISO 解析会当成 UTC，
 * 而这个值是本地时间，差几个时区就会把边界上的账号算进/算出区间。
 */
function stamp(s: string): number {
  return new Date((s || '').replace(/-/g, '/')).getTime()
}

/** 已经过期的要一眼看得见 —— 它和「禁用」的效果一样，但原因完全不同。 */
function isExpired(r: AccountRow): boolean {
  return r.IsExpiry && stamp(r.ExpiryTime) < Date.now()
}

const filtered = computed<AccountRow[]>(() => {
  const kw = q.value.trim().toLowerCase()

  //两个端点都含在内：到期日填 2026-10-02 的账号，查「10-02 之前到期」要能查到
  const from = dFrom.value ? stamp(dFrom.value + ' 00:00:00') : NaN
  const to = dTo.value ? stamp(dTo.value + ' 23:59:59') : NaN
  const ranged = !isNaN(from) || !isNaN(to)

  return rows.value.filter((r) => {
    if (kw && !(r.UserName || '').toLowerCase().includes(kw)) return false

    if (ranged) {
      //永不过期的不属于任何区间
      if (!r.IsExpiry) return false

      const e = stamp(r.ExpiryTime)
      if (!isNaN(from) && e < from) return false
      if (!isNaN(to) && e > to) return false
    }

    return true
  })
})

/*
  表头排序，接在筛选<b>之后</b>：先按搜索 / 到期区间筛出 filtered，再排成 shown。

  顺序不能反 —— 下面的虚拟滚动、多选、「全选只管筛出来的行」全都吃 shown，
  排在筛选之前的话窗口切片会落到没筛掉的那批上。

  「状态」列排的是<b>在线与否</b>（IsOnLine），与那一格画的圆点一致；
  序号那一列不排，它显示的本来就是「在你眼前这一份里的第几行」。
*/
const sort = useSort<AccountRow>(filtered, {
  user: (r) => r.UserName || '',
  state: (r) => (r.IsOnLine ? 1 : 0),
  links: (r) => (r.IsLimitLinks ? r.LimitLinks : Number.MAX_SAFE_INTEGER),
  devices: (r) => (r.IsLimitDevices ? r.LimitDevices : Number.MAX_SAFE_INTEGER),
  //永不过期的排最后：它比任何一个具体日期都"晚"
  expiry: (r) => (r.IsExpiry ? timeKey(r.ExpiryTime) : Number.MAX_SAFE_INTEGER),
})

const shown = sort.sorted

const ranged = computed(() => !!(dFrom.value || dTo.value))

function clearRange(): void {
  dFrom.value = ''
  dTo.value = ''
}

/*
  ── 虚拟滚动 ───────────────────────────────────────────────────

  【为什么必须做】这一行有 22 个 DOM 节点（8 个格子 + 勾选框 + 状态圆点 +
  三个带内联 SVG 的操作按钮）。实测把这一行原样铺开：

      1000 行  131ms /  2.2 万节点
      5000 行  665ms / 11.0 万节点
     20000 行 2993ms / 44.0 万节点，之后每滚一次还要 205ms

  代理账号是拿来卖的，几万个是真实规模，不是假想。所以照搬 PacketList.vue
  那套定高窗口：只渲染视口里的约 40 行，代价与总行数无关。

  行高写死 34px，与 .row 的 height 必须一致 —— 这两个数一旦对不上，
  滚动位置就会随行数线性漂移，越往下越离谱。
*/
const ROW_H = 34
const OVERSCAN = 8

const scroller = ref<HTMLElement | null>(null)
const scrollTop = ref(0)
const viewH = ref(600)

const total = computed(() => shown.value.length)
const start = computed(() => Math.max(0, Math.floor(scrollTop.value / ROW_H) - OVERSCAN))

const end = computed(() =>
  Math.min(total.value, start.value + Math.ceil(viewH.value / ROW_H) + OVERSCAN * 2),
)

/** 当前渲染窗口。slice 约 40 个元素，每帧一次，可以忽略。 */
const windowRows = computed(() => shown.value.slice(start.value, end.value))

function onScroll(): void {
  const el = scroller.value
  if (el) scrollTop.value = el.scrollTop
}

/*
  改了筛选条件就回到顶部。

  两个理由：新的结果本来就该从头看；以及列表变短之后浏览器会自己把 scrollTop
  夹回去，但那次夹取<b>不一定</b>派发 scroll 事件 —— 我们的 scrollTop 副本
  就会停在旧值上，窗口算到了表尾之外，slice 出空数组，界面一片空白。
*/
/* 排序列 / 方向变了也一样要回顶 —— 排完之后停在原来那个位置看到的是另一批行，毫无意义。 */
watch([q, dFrom, dTo, sort.key, sort.dir], () => {
  const el = scroller.value
  if (el) el.scrollTop = 0
  scrollTop.value = 0
})

let ro: ResizeObserver | null = null

onMounted(() => {
  const el = scroller.value
  if (!el) return

  viewH.value = el.clientHeight
  ro = new ResizeObserver(() => { viewH.value = el.clientHeight })
  ro.observe(el)
})

onBeforeUnmount(() => {
  ro?.disconnect()
  ro = null
})

function limitText(on: boolean, n: number): string {
  return on ? String(n) : t('acct.unlimited')
}

/*
  ── 多选与右键菜单（对应 WinForms 的 ColumnCheck + tAccountList_CellClick）──

  【选中态留在前端】WinForms 把它存在模型上（AccountInfo.IsCheck，DTO 里也带着
  这个字段）。这边不跟：选中是纯界面状态，存进 Operate 只会多一份要同步的东西，
  两个前端还会互相干扰。C# 那几个批量入口收的是 Id 数组。

  用 Set 而不是数组：全选两万行时 has/add/delete 都要是 O(1)。
*/
/*
  单击 / Ctrl / Shift 多选，全项目一份实现，见 usePick.ts。

  ⚠️ 传进去的是 <b>shown（筛选后的行）而不是 rows</b>：
  「全选」只管当前筛选出来的那些 —— 用户搜出「10 月到期的」再全选，意思就是这一批，
  不该把被筛掉的也一起选上，那是删错东西最容易发生的地方。
  Shift 连选同理，按屏幕上看到的顺序连。
*/
const { picked, pickedIds, onRowClick, selectAll, clear } =
  useRowPick(shown, (r) => r.Id)

/*
  改启用。走 setAccountEnable 才会落库并把这一行推回来 ——
  就地改属性不触发 ListChanged，C# 侧 SetAccountEnable_ById 里那句 PushAccountRow 是必需的。
*/
async function toggleEnable(r: AccountRow): Promise<void> {
  try {
    await call('setAccountEnable', { id: r.Id, enable: !r.IsEnable })
  } catch (e) {
    console.error('[acct] 切换启用失败', e)
  }
}

const menuAt = ref<{ x: number; y: number } | null>(null)
const adjusting = ref<'expiry' | 'links' | 'devices' | null>(null)

/*
  菜单项照抄 Operate.GetCMS_AccountList 的结构与顺序。

  【一个都没选时不压暗，照样能点】与 WinForms 一致：点下去弹一句「请选择账号」。
  压暗的坏处是它只说「不能点」，不说为什么 —— 而这里的原因（还没勾行）
  恰恰是用户自己能解决的，说出来比拦住更有用。
*/
const menuItems = computed<MenuItem[]>(() => {
  /*
    每项都带上条数，尤其删除 —— 动手前该看见要删几条。
    「批量调整」的三个子项不再重复标一遍：条数挂在父项上，
    二级菜单是贴着父项开的，那个数就在旁边，写两遍只是噪音。
  */
  const n = picked.value.size
  const tag = n ? ' (' + n + ')' : ''

  return [
    {
      id: 'adjust',
      label: t('acct.cm.adjust') + tag,
      icon: ICON.list,
      sub: [
        { id: 'expiry', label: t('acct.adj.expiry'), icon: ICON.clock },
        { id: 'links', label: t('acct.adj.links'), icon: ICON.link },
        { id: 'devices', label: t('acct.adj.devices'), icon: ICON.device },
      ],
    },
    { divider: true },
    {
      id: 'export',
      //与滤镜列表同一枚软盘：弹的是「另存为」文件框，而且旁边的删除是实物形状
      label: t('acct.cm.export') + tag,
      icon: ICON.save,
    },
    { divider: true },
    {
      id: 'delete',
      label: t('acct.cm.delete') + tag,
      icon: ICON.del,
      danger: true,
    },
    { divider: true },
    /* 勾选框列去掉之后，全选得在这儿有个入口 —— 与其余各屏同一套 */
    { id: 'selectAll', label: t('pm.selectAll'), icon: ICON.list },
    { id: 'deselect', label: t('pm.deselect'), icon: ICON.del, disabled: !n },
  ]
})

/*
  右键<b>只开菜单，不动选中集</b>（与 WinForms 一致）。

  曾经做成「在没选中的行上右键就先选中它」，撤掉了：批量操作动的是一整批，
  而右键的落点常常只是「鼠标正好停在那儿」—— 用它去改选中，等于让一个
  看不出后果的动作决定接下来删掉哪些账号。

  一个都没选时菜单项是压暗的（见 menuItems 的 disabled），
  比 WinForms 那句「请选择账号」的提示更早说清楚。
*/
function openMenu(e: MouseEvent): void {
  menuAt.value = { x: e.clientX, y: e.clientY }
}

/**
 * 双击一行打开编辑（与 WinForms 的 tAccountList_CellDoubleClick 一致）。
 *
 * 【落在行内按钮上的双击不算】行里有勾选框和三个操作按钮：
 * 双击勾选框会把它切两次（等于没切）却还开了弹窗，双击删除按钮更不该顺带开编辑。
 * 所以先看事件源在不在某个 button 里。
 *
 * DOM 的 dblclick 只对主键（左键）派发，不必像 WinForms 那边那样再判一次
 * e.Button —— AntdUI 的 Table 是任意键双击都抛事件，那是它自己的毛病。
 */
function onRowDblClick(e: MouseEvent, r: AccountRow): void {
  if ((e.target as HTMLElement | null)?.closest('button')) return
  editing.value = r.Id
}

async function onMenuPick(id: string): Promise<void> {
  if (id === 'selectAll') { selectAll(); return }
  if (id === 'deselect') { clear(); return }

  /*
    五项全都作用于选中的行，所以一处拦住就够了 ——
    放在这里而不是各分支里，是因为将来加菜单项时最容易漏的就是这一句。
    与 WinForms 的 tAccountList_CellClick 同一个位置、同一句话。
  */
  if (picked.value.size === 0) {
    pushToast('warning', t('acct.cm.needPick'))
    return
  }

  if (id === 'expiry' || id === 'links' || id === 'devices') {
    adjusting.value = id
    return
  }

  try {
    if (id === 'export') await call('exportSelectedAccounts', { ids: pickedIds.value })
    else if (id === 'delete') await call('deleteSelectedAccounts', { ids: pickedIds.value })
  } catch (e) {
    console.error('[acct] 批量操作失败', e)
  }
}

async function del(r: AccountRow): Promise<void> {
  //确认框在 C# 侧（DeleteAccount_Dialog），这里不再问一遍
  if (!r.Id) {
    //防御：没有 Id 就别发出去。C# 侧也会拒，但错误越早暴露越好查
    console.error('[acct] 这一行没有 Id，删除已取消', r)
    return
  }

  try {
    await call('deleteAccount', { id: r.Id })
  } catch (e) {
    console.error('[acct] 删除失败', e)
  }
}

/*
  清空全部走<b>独立的</b>桥方法，不是「deleteAccount 不传 id」。

  原来是后者，那意味着 del() 那边一旦 r.Id 变成空串或 undefined，
  「删这一条」就静默升级成「删全部」—— 参数丢失的后果不该是不可逆的数据丢失。
*/
async function clearAll(): Promise<void> {
  try {
    await call('clearAllAccounts')
  } catch (e) {
    console.error('[acct] 清空失败', e)
  }
}

async function io(method: 'importAccounts' | 'exportAccounts'): Promise<void> {
  try {
    await call(method)
  } catch (e) {
    console.error('[acct] ' + method + ' 失败', e)
  }
}
</script>

<template>
  <div class="page list-page acct">
    <div class="bar">
      <button class="btn primary" @click="editing = ''">{{ t('acct.add') }}</button>
      <button class="btn" @click="batch = true">{{ t('acct.batch') }}</button>

      <!--
        清除按钮浮在输入框里（靠右侧留出的内边距），不占工具条的横向预算 ——
        这条工具条按 1120 的最小宽度排得很满，多一个并排的元素就会溢出。
        Esc 也能清，键盘上不用去够那个小叉。
      -->
      <span class="sw">
        <input
          v-model="q"
          class="inp"
          spellcheck="false"
          :placeholder="t('acct.search')"
          @keydown.esc="q = ''"
        >
        <button v-if="q" class="x" :title="t('acct.clearSearch')" @click="q = ''">
          <svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg>
        </button>
      </span>

      <!-- 按到期日筛选。两端都可留空，只填一边就是「之后 / 之前」 -->
      <div class="rng" :class="{ on: ranged }">
        <span class="lb">{{ t('acct.expiryIn') }}</span>

        <!--
          空态提示自己画。

          <input type="date"> 空着时 Chromium 会画一个格式提示，它跟着<b>浏览器的
          界面语言</b>走而不是页面的 lang，我们既翻不了也改不了样式 —— 实测同一份代码
          在不同环境下会出现「年/月/日」「mm/dd/yyyy」甚至两者混排。
          所以把它整段透明掉，自己写一个「不限」：顺带比格式提示更说得清，
          这一端留空的语义本来就是不限。
        -->
        <span class="dtw">
          <input v-model="dFrom" class="inp dt" :class="{ ph: !dFrom }" type="date">
          <span v-if="!dFrom" class="phtx">{{ t('acct.anyDate') }}</span>
        </span>

        <span class="sep">~</span>

        <span class="dtw">
          <input v-model="dTo" class="inp dt" :class="{ ph: !dTo }" type="date">
          <span v-if="!dTo" class="phtx">{{ t('acct.anyDate') }}</span>
        </span>
        <span v-if="ranged" class="n">{{ shown.length }}</span>
        <button v-if="ranged" class="x" :title="t('acct.clearRange')" @click="clearRange">
          <svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg>
        </button>
      </div>

      <!-- 原来这里有一个 .grow 撑开；现在搜索框自己吃掉剩余宽度，右边三颗按钮照样贴右 -->

      <button class="btn" @click="io('importAccounts')">{{ t('acct.import') }}</button>
      <button class="btn" :disabled="!rows.length" @click="io('exportAccounts')">{{ t('acct.export') }}</button>
      <button class="btn danger" :disabled="!rows.length" @click="clearAll">{{ t('acct.clearAll') }}</button>
    </div>

    <p v-if="!authOn && rows.length" class="hint">{{ t('acct.authOffHint') }}</p>

    <div ref="scroller" class="body" @scroll.passive="onScroll">
      <!-- 表头每格带着与数据行同名的 class，对齐规则按 class 写、增删列不必重编序号 -->
      <div class="head">
        <!-- 序号不排：它显示的本来就是「在你眼前这一份里的第几行」，排它没有意义 -->
        <span class="no">{{ t('col.id') }}</span>
        <span class="ck">{{ t('col.enable') }}</span>
        <span class="user so" :class="{ on: sort.active('user') }" @click="sort.toggle('user')">{{ t('col.user') }}<i class="ar">{{ sort.mark('user') }}</i></span>
        <span class="st so" :class="{ on: sort.active('state') }" @click="sort.toggle('state')">{{ t('col.state') }}<i class="ar">{{ sort.mark('state') }}</i></span>
        <span class="tag so" :class="{ on: sort.active('links') }" @click="sort.toggle('links')">{{ t('col.links') }}<i class="ar">{{ sort.mark('links') }}</i></span>
        <span class="tag so" :class="{ on: sort.active('devices') }" @click="sort.toggle('devices')">{{ t('col.devices') }}<i class="ar">{{ sort.mark('devices') }}</i></span>
        <span class="tm so" :class="{ on: sort.active('expiry') }" @click="sort.toggle('expiry')">{{ t('col.expiry') }}<i class="ar">{{ sort.mark('expiry') }}</i></span>
        <span class="ops">{{ t('col.ops') }}</span>
      </div>

      <div v-if="!rows.length" class="empty">{{ t('acct.empty') }}</div>
      <div v-else-if="!shown.length" class="empty">{{ t('acct.noMatch') }}</div>

      <!-- 撑出总高度的占位；真实的那 40 行绝对定位在它上面 -->
      <div v-else class="spacer" :style="{ height: total * ROW_H + 'px' }">
        <div class="win" :style="{ transform: `translateY(${start * ROW_H}px)` }">
          <!--
            key 用窗口内下标而不是 r.Id：滚动时窗口整体平移，按下标 key 能让 Vue
            就地改文本、复用这 40 个 DOM 节点；按 Id key 则每滚一行都要销毁重建
            —— 而这一行有 22 个节点、3 个内联 SVG，重建的代价正是要躲开的那个。
            行内没有自身状态（勾选框读的是 r.IsEnable），就地复用不会串。
          -->
          <div
            v-for="(r, i) in windowRows"
            :key="i"
            class="row"
            :class="{ off: !r.IsEnable, sel: picked.has(r.Id) }"
            @click="onRowClick(r, $event, start + i)"
            @contextmenu.prevent="openMenu($event)"
            @dblclick="onRowDblClick($event, r)"
          >
            <!-- 序号是全表的位次，不是窗口内的 -->
            <span class="no">{{ start + i + 1 }}</span>

            <!-- @click.stop：这一格是开关，不该顺带把这一行选中 -->
            <span class="ck">
              <button class="chk" :class="{ on: r.IsEnable }" :title="t('col.enable')"
                      @click.stop="toggleEnable(r)"><i /></button>
            </span>
            <span class="user">{{ r.UserName }}</span>

            <span class="st" :class="r.IsOnLine ? 'on' : 'no-'">
              <i />{{ r.IsOnLine ? t('acct.online') : t('acct.offline') }}
            </span>

            <span class="tag" :class="{ un: !r.IsLimitLinks }">{{ limitText(r.IsLimitLinks, r.LimitLinks) }}</span>
            <span class="tag" :class="{ un: !r.IsLimitDevices }">{{ limitText(r.IsLimitDevices, r.LimitDevices) }}</span>

            <span class="tm" :class="{ bad: isExpired(r) }">{{ expiryText(r) }}</span>

            <span class="ops">
              <button class="op" :title="t('acct.op.edit')" @click="editing = r.Id">
                <svg class="ico" viewBox="0 0 24 24"><path d="M4 20h4L20 8l-4-4L4 16z" /></svg>
              </button>
              <button class="op loc" :title="t('acct.op.logins')" @click="viewing = r">
                <svg class="ico" viewBox="0 0 24 24"><path d="M12 21s7-6.2 7-11a7 7 0 1 0-14 0c0 4.8 7 11 7 11z" /><circle cx="12" cy="10" r="2.5" /></svg>
                <span v-if="r.LoginCount" class="n">{{ r.LoginCount }}</span>
              </button>
              <button class="op del" :title="t('acct.op.del')" @click="del(r)">
                <svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg>
              </button>
            </span>
          </div>
        </div>
      </div>
    </div>

    <ContextMenu :at="menuAt" :items="menuItems" @pick="onMenuPick" @close="menuAt = null" />

    <AccountEdit :id="editing" @close="editing = null" />
    <AccountLogins :row="viewing" @close="viewing = null" />
    <AccountBatch v-model:open="batch" />
    <AccountAdjust :kind="adjusting" :ids="pickedIds" @close="adjusting = null" />
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
  ⚠️ 这条工具条<b>尽量不折行</b>（2026-09-11 按要求）。
  做法不是 nowrap，而是让搜索框的 flex-basis 为 0、只保一个 90px 的下限 ——
  flex-wrap 按各项的「假想宽度」断行，搜索框只算 90，于是<b>只有它缩到底还装不下时才会折</b>。
  原来它的基准是 170、右边还有一个 .grow，窗口稍窄「清空」就被甩到第二行。

  ⚠️ 为什么不硬写 nowrap：实测（真机可用宽 = 视口 − 侧栏 − 24）
    1280 宽：中 / 繁 / 英 / 日 / 韩 / 越 753~929 ≤ 1028~1060，一行；俄语要 1310，装不下
    1024 宽（125% 缩放）：中 / 繁 753 ≤ 804 一行；英 / 日 / 韩 / 越 845~929 > 772~804
  硬 nowrap 的话那几种会把右边的按钮<b>静默裁掉</b>（见 .modebar .ports 那次）；
  折一行至少按钮都点得到。
*/

/* 到期区间与按钮都不缩 —— 该让位的只有搜索框 */
.rng { flex: none; }

/*
  搜索框<b>按宽度自适应</b>：吃掉工具条上所有剩余的宽度（原来那个 .grow 撑开的空当），
  窗口窄下来它先让，一路让到 90px（还装得下「搜索…」加清除叉）。
  伸缩性挂在外层的 .sw 上 —— 清除按钮要相对它绝对定位。
*/
.sw {
  position: relative;
  display: flex;
  flex: 1 1 0;
  min-width: 90px;
}

.sw .inp { flex: 1; min-width: 0; padding-right: 24px; }

.sw .x {
  position: absolute;
  right: 6px;
  top: 50%;
  transform: translateY(-50%);
}

/* 基样式在 style.css 的 .inp。这一屏的框在工具条上，比表单里的矮 2px */
.inp { height: 26px; }

/* 到期日区间。没填时整块压暗，填了就点亮，一眼能看出筛选是否生效 */
.rng {
  display: flex;
  align-items: center;
  gap: 6px;
  padding: 0 8px;
  border: 1px solid transparent;
}

.rng.on { border-color: rgb(var(--cyan-rgb) / 32%); background: rgb(var(--cyan-rgb) / 6%); }

.rng .lb {
  /* top .9px：--fs-caption 提到 10.5px 且 --share 显式回退到微软雅黑 UI 之后（2026-09-13）放大实测，
     .4px 时墨迹比所在行中线高约 0.5px，.9px 落在中线上 */
  position: relative;
  top: .9px;
  font-family: var(--share);
  font-size: var(--fs-caption);
  letter-spacing: .12em;
  text-transform: uppercase;
  color: var(--muted);
  white-space: nowrap;
}

.rng.on .lb { color: var(--cyan); }
.rng .sep { color: var(--dim); font-size: var(--fs-small); }
.rng .n { font-family: var(--share); font-size: var(--fs-caption); color: var(--cyan); }

/* 清除按钮：搜索框与到期区间共用一套外观，只有定位不同 */
.x {
  display: inline-flex;
  padding: 0;
  background: transparent;
  border: 0;
  color: var(--muted);
  cursor: pointer;
}

.x:hover { color: var(--danger); }
.x .ico { width: 12px; height: 12px; fill: none; stroke: currentColor; stroke-width: 2; }

/* 104px 是量出来的：装得下 2026/10/02 加日历图标，再窄就开始挤 */
.inp.dt {
  flex: none;
  width: 104px;
  min-width: 0;
  height: 24px;
  padding: 0 6px;
  font-size: var(--fs-small);
  color: var(--muted);
}

.inp.dt::-webkit-calendar-picker-indicator { filter: invert(0.55); cursor: pointer; }
.rng.on .inp.dt { color: var(--gray); }

/* 空态：原生的格式提示整段透明，自己那行浮在上面。日历图标不受影响，照常能点 */
.dtw { position: relative; display: inline-flex; flex: none; }
.inp.dt.ph::-webkit-datetime-edit { opacity: 0; }

.phtx {
  position: absolute;
  left: 7px;
  top: 50%;
  transform: translateY(-50%);
  pointer-events: none;
  font-size: var(--fs-small);
  color: var(--dim);
}

.hint {
  flex: none;
  margin: 0;
  padding: 7px 12px;
  border: 1px solid rgb(var(--amber-rgb) / 32%);
  background: rgb(var(--amber-rgb) / 7%);
  font-size: var(--fs-small);
  color: var(--amber);
}

/*
  除用户名外全部居中 —— 表头与内容一起。
  这几列都是定宽的短值（勾选框 / 序号 / 状态 / 两个数字 / 一个日期 / 三个按钮），
  居中之后每列自成一竖条，扫起来比一堆左对齐的碎片整齐。
  用户名是唯一长度不可预知的字段，它铺满剩余宽度，只能左对齐。
*/
/* 八列。用户名给 1fr —— 它是唯一长度不可预知的字段 */
/* display / align-items / padding / 高度 / 配色都在 style.css 的 .list-page 里 */
.head,
.row {
  grid-template-columns: 52px 46px minmax(120px, 1fr) 78px 84px 84px 148px 96px;
  gap: 12px;
}

.head > span,
.row > span { text-align: center; }

/*
  用户名两处都要左对齐。
  <b>选择器必须带上 .row > / .head > </b>：光写 .user 是 (0,1,0)，
  压不过上面那条 (0,1,1) 的 .row > span，内容会一直是居中的。
*/
.head > span.user,
.row > span.user { text-align: left; }

.no { color: var(--dim); font-variant-numeric: tabular-nums; }
.user { color: var(--gray); overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
.tm { color: var(--muted); font-variant-numeric: tabular-nums; }
.tm.bad { color: var(--danger); }

/* 状态与操作是 flex 容器，text-align 管不到它们里面，得各自再居中一次 */
.st { display: flex; align-items: center; justify-content: center; gap: 6px; font-size: var(--fs-body); }
.st i { width: 6px; height: 6px; border-radius: 50%; flex: none; }
.st.on { color: var(--green); }
.st.on i { background: var(--green); box-shadow: 0 0 6px var(--green); }
.st.no- { color: var(--dim); }
.st.no- i { background: var(--border2); }

/* 数值与「无限制」用两种颜色区分：一个是限额，一个是没有限额 */
.tag { color: var(--cyan); font-variant-numeric: tabular-nums; }
.tag.un { color: var(--dim); }

.op.loc:hover { border-color: var(--amber); color: var(--amber); }

/* 有过登录记录的在图标右上角挂一个数字，不用点进去才知道有没有 */
.op .n {
  position: absolute;
  top: -1px;
  right: -1px;
  font-family: var(--share);
  font-size: 8px;
  line-height: 1;
  color: var(--amber);
}
</style>
