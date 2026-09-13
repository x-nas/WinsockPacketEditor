<script setup lang="ts">
/*
  代理数据列表 · 虚拟滚动。这是代理模式的主界面（对应 WinForms 的 Controls/ProxyList.cs）。

  【为什么不用 a-table】
  ant-design-vue 4.x 的 Table 有 virtual 属性（底层 rc-virtual-list），能用，
  但它为每行建了一层组件实例与列渲染管线；在「每秒新增数千行 + 列表五万行」这个量级下，
  开销和行为都不好预测。这里的表结构是固定的 13 列纯文本，手写一个定高虚拟列表
  只有几十行代码，DOM 里永远只有约 40 个 div，性能完全可控。
  （写这段时外围的按钮 / 弹窗 / 通知还都是 ant-design-vue 的。后来它们逐个换成了
  自绘件 —— ToastStack、BusyMask、SettingsModal、ContextMenu、ConfirmDialog ——
  现在项目里只剩 main.ts 里那句 reset.css 还引着这个包。）

  【定高的前提】
  行高写死 ROW_H，不做动态测量。数据行都是单行不换行的等宽文本，本来就等高；
  动态测高要引入 ResizeObserver + 位置累加表，为一个不存在的问题付代价。
*/
import { computed, onBeforeUnmount, onMounted, ref, watch } from 'vue'
import { flagSrc } from '../flags'
import { listSetting } from '../stores/runtime'
import { t, type Key } from '../i18n'
import { rows as proxyRows, injectFeed } from '../stores/packets'
import { DOMAIN_TYPE, FilterAction, PACKET_TYPE, type PacketListRow, type Prefs, type ProxyRow } from '../bridge/types'

/*
  ── 两种模式共用这一份表 ──────────────────────────────

  代理模式的行是 ProxyRow，注入模式的是 PacketRow（各有独立的 Id 序列）。
  两者的<b>骨架完全一样</b> —— 定高虚拟滚动、列宽可拖、滤镜配色、多选、跟随底部 ——
  差别只在「有哪几列」。所以这里只把列定义分岔，其余全部共用。

  抄第二份的代价这个项目吃过：三屏列表样式手抄之后抄歪了四处（见 CLAUDE.md 的 .list-page）。
*/
type AnyRow = PacketListRow

/*
  picked 是<b>多选集</b>，selectedId 是「详情面板正在看哪一行」。
  两者分开：右键菜单作用于多选集，而十六进制面板永远只看一行。
  选中状态由 ProxyData 持有（它才是发起动作的那一层），这里只负责显示与派事件。
*/
const props = withDefaults(
  defineProps<{
    prefs: Prefs | null
    selectedId: number | null
    picked?: Set<number>
    follow?: boolean
    /** 'proxy' = 代理数据（ProxyRow）；'inject' = 注入模式的封包列表（PacketRow）。 */
    mode?: 'proxy' | 'inject'
  }>(),
  { follow: true, picked: () => new Set<number>(), mode: 'proxy' },
)

const emit = defineEmits<{
  (e: 'select', row: AnyRow, ev: MouseEvent, index: number): void
  (e: 'menu', ev: MouseEvent, row: AnyRow): void
  /*
    双击一行 —— 两个数据页都拿它开「封包编辑」，与 WinForms 的
    `dgvPacketList_CellMouseDoubleClick` / `dgvProxyList_CellMouseDoubleClick` 对应。
    全项目别的列表（滤镜 / 发送 / 机器人 / 仓库 / 账号 / 进程表）双击都是打开编辑，
    只有这两屏一直没有，是移植时漏的。
  */
  (e: 'open', row: AnyRow): void
}>()

/*
  当前这一路的行。

  ⚠️ <b>刻意不做成 prop</b>。把数组从父组件传进来的话，父组件自己也得读一次
  那个 shallowRef —— 于是每帧 triggerRef 都会连带把父组件整棵模板重渲染一遍，
  而这一屏正是全项目唯一的性能热点。让依赖只挂在这个组件里，父组件一帧都不用重渲染。

  ⚠️⚠️ <b>这里存的是「哪一个 ref」，不是「哪一个数组」。</b>
  写成 computed(() => 那个数组) 会<b>静默地把整张表变空</b>：
  Vue 3.4 起 computed 会比较新旧值，相同（===）就不往下传播；
  而这个 store 的热路径正是「就地 push 同一个数组 + triggerRef」——
  数组引用永远不变，于是 total / windowRows 一次都不会重算。
  实测现象是状态条上的「30 条」在涨、表里一行都没有（探针页抓到的）。

  存 ref 本身则没有这个问题：它只在 mode 变化时才换，
  而 total / windowRows 各自去读 .value，依赖直接挂在 shallowRef 上，triggerRef 照常生效。
*/
const feed = computed(() => (props.mode === 'inject' ? injectFeed.rows : proxyRows))
const rows = { get value(): AnyRow[] { return feed.value.value } }

const ROW_H = 24
const OVERSCAN = 8

/*
  列与 WinForms 的 ProxyList 保持一致的信息量。

  表头文案走 i18n，所以列定义得是 computed —— 写成常量数组的话切换语言后
  表头不会重画（数组本身没变，Vue 无从知道里面的字该更新）。
  列宽<b>不随语言变</b>：英文表头普遍更短，宽度按中文定就都装得下。

  <b>表头与数据行各有各的对齐</b>：align 管数据行，halign 管表头，都默认左对齐。
  两者不能共用一个值 —— 长度列就是反例：数字右对齐便于比大小，
  而它的表头居中才和左右两列的窄表头看齐。
*/
/*
  ⚠️ <b>序号列 84px 是量出来的，别随手改回 74。</b>

  序号是 ProxyInfo.Id / PacketInfo.Id —— 运行期自增、<b>停止再开始代理也不复位</b>，
  只有重启程序才归零，所以抓一整天很容易上到九位十位。
  74px 时实测（Consolas 12px）：8 位刚好放下，<b>9 位（一亿）开始截断</b>成省略号。
  84px 装得下 10 位（10 位实测占 82px），约 43 亿 ——
  按 3000 包/秒连续抓要 16 天才用得完。代价是最后那个弹性的「数据」列少 10px。

  列宽本来就能拖（表头右边界那条 7px 手柄），这里改的只是默认值。
*/
const columns = computed(() => (props.mode === 'inject' ? injectColumns() : proxyColumns()))

/*
  注入模式的列。与代理模式的差别来自模型本身：
  PacketInfo 只有「本机 / 远端」两侧（From / To），没有域名、没有协议类型 ——
  那两样是 SunnyNet 的中间人那条路才有的东西，钩子这边根本不产生。
*/
function injectColumns() {
  return [
    { key: 'Id', title: t('col.id'), w: 84, align: 'center', halign: 'center', cls: 'c-dim' },
    { key: 'Time', title: t('col.time'), w: 136, cls: 'c-meta' },
    { key: 'Socket', title: t('col.socket'), w: 64, align: 'center', halign: 'center', cls: 'c-dim' },
    //类型不给固定色 —— 按「请求 / 响应」分色，见 cellClass
    { key: 'Type', title: t('col.type'), w: 96, align: 'center', halign: 'center' },
    { key: 'From', title: t('col.from'), w: 158, cls: 'c-local' },
    { key: 'FromLocation', title: t('col.fromLoc'), w: 88, cls: 'c-local-dim', flag: true },
    { key: 'To', title: t('col.to'), w: 158, cls: 'c-remote' },
    { key: 'ToLocation', title: t('col.toLoc'), w: 88, cls: 'c-remote-dim', flag: true },
    { key: 'Len', title: t('col.len'), w: 62, align: 'right', halign: 'center', cls: 'c-local' },
    { key: 'Preview', title: t('col.data'), w: 460, cls: 'c-data' },
  ]
}

function proxyColumns() {
  return [
  { key: 'Id', title: t('col.id'), w: 84, align: 'center', halign: 'center', cls: 'c-dim' },
  /*
    时间是 HH:mm:ss:fffffff（16 字符，见 FeedRows 的 PacketTime 格式）。
    Consolas 12px 每字约 6.6px、Cascadia Mono 约 7.2px，取宽的那个算：
    16 × 7.2 + 左右各 8px 内边距 ≈ 131 —— 所以给 136，两种字体下都不会被截。
  */
  { key: 'Time', title: t('col.time'), w: 136, cls: 'c-meta' },
  { key: 'Socket', title: t('col.socket'), w: 64, align: 'center', halign: 'center', cls: 'c-dim' },
  /*
    这里原先有一列「会话」（ProxyInfo.TheologyID）。去掉了 —— 它<b>只在
    SunnyNet 的 HTTP/HTTPS 中间人那条路上才有值</b>（SunnyNetCallback.cs 里 6 处
    传 Conn.TheologyID()）；SOCKS5 的三个调用点（Operate.cs 的 10902 / 15701 / 15766）
    传的都是写死的 0，而 SOCKS5 才是主路径。
    整列恒为 0 不只是白占宽度，0 看着还像一个真实的会话号，比空着更误导。
    将来给 ProxySession 补上自增会话 ID 时再加回来。
  */
  /*
    类型不给固定色 —— 它按「请求 / 响应」分色，见 cellClass。

    宽度比注入模式那份多 16px：代理模式会出现 WebSocket 请求 / 响应
    （SunnyNet 的 WebSocket 中间人那条路），是全部类型名里最长的一个，96 装不下。
  */
  { key: 'Type', title: t('col.type'), w: 112, align: 'center', halign: 'center' },
  { key: 'DomainType', title: t('col.proto'), w: 82, align: 'center', halign: 'center', cls: 'c-meta' },
  { key: 'ClientAddr', title: t('col.client'), w: 158, cls: 'c-local' },
  { key: 'ClientLocation', title: t('col.clientLoc'), w: 88, cls: 'c-local-dim', flag: true },
  { key: 'ServerAddr', title: t('col.server'), w: 158, cls: 'c-remote' },
  { key: 'ServerLocation', title: t('col.serverLoc'), w: 88, cls: 'c-remote-dim', flag: true },
  { key: 'ServerDomain', title: t('col.domain'), w: 190, cls: 'c-domain' },
  //数据行右对齐（数字要比大小），表头居中
  { key: 'Len', title: t('col.len'), w: 62, align: 'right', halign: 'center', cls: 'c-local' },
  //数据列是弹性的，这里的 460 是它的<b>最小</b>宽度，实际宽度见 lastWidth
  { key: 'Preview', title: t('col.data'), w: 460, cls: 'c-data' },
  ]
}

/*
  最后一列（数据）铺满剩余宽度。

  前面 12 列是定宽的 —— 它们的内容长度可预期（Id / 时间 / 地址 / 长度…），
  定宽才能让上下行对齐。数据列装的是十六进制串，有多少空间就该用多少。

  算法：视口宽 - 前 12 列之和；不够 460 就退回 460，此时整个表横向滚动
  （与原来的行为一致）。减 1 是给容器的右边框留位，否则会多出 1px 的横向滚动条。
*/
/*
  用户手动拖出来的列宽，按列 key 存。

  只覆盖前 12 列 —— 最后一列（数据）始终吃剩余空间，给它一个固定宽度
  就等于把「铺满」这个行为关掉了，那是刚做过的需求。
*/
const userWidth = ref<Record<string, number>>({})

/** 某列的<b>有效</b>宽度：用户拖过就用拖出来的，否则用默认值。 */
function baseWidth(c: { key: string; w: number }): number {
  return userWidth.value[c.key] ?? c.w
}

const fixedWidth = computed(() =>
  shown.value.slice(0, -1).reduce((sum, c) => sum + baseWidth(c), 0))

const lastWidth = computed(() => {
  const min = shown.value[shown.value.length - 1].w
  const rest = viewW.value - fixedWidth.value - 1
  return Math.max(min, rest)
})

/** 表头与行的总宽。它决定横向滚动范围，所以必须跟着最后一列一起变。 */
const totalWidth = computed(() => fixedWidth.value + lastWidth.value)

/** 某一列该用多宽：最后一列用算出来的，其余用定值。 */
function colWidth(i: number): number {
  return i === shown.value.length - 1 ? lastWidth.value : baseWidth(shown.value[i])
}

/*
  按「列表设置」过滤掉关掉的列。

  <b>序号 / 时间 / 域名 / 数据不参与</b> —— 序号是取字节的钥匙（getPacketDetail 靠它），
  另外三个是这张表的意义所在。设置面板里也没给这几个开关。
*/
const shown = computed(() => {
  const s = listSetting.value
  if (!s) return columns.value

  /*
    注入模式的 From / To 与代理模式的 ClientAddr / ServerAddr 是同一件事的两个名字，
    所以两边映射到同一个开关名。

    但<b>值是分开的</b>：listSetting 由各自的页面按 mode 取（ProxyData 取代理那套、
    InjectData 取注入那套），C# 侧对应 ProxyConfig.List.IsShow_* 与
    PacketConfig.List.IsShow_* 两组字段。这里只负责把开关名对上，不管值从哪来。
  */
  const off: Record<string, boolean> = {
    Socket: !s.showSocket,
    Type: !s.showType,
    ClientAddr: !s.showClientAddr,
    ClientLocation: !s.showClientLoc,
    ServerAddr: !s.showServerAddr,
    ServerLocation: !s.showServerLoc,
    From: !s.showClientAddr,
    FromLocation: !s.showClientLoc,
    To: !s.showServerAddr,
    ToLocation: !s.showServerLoc,
    Len: !s.showLen,
  }

  return columns.value.filter((c) => !off[c.key])
})

/*
  ── 渲染就绪的列 ──────────────────────────────────────

  <b>模板里一次函数都不该调。</b>可见窗口约 40 行 × 12 列，每帧就是几百次调用，
  而 colWidth 里还各带一次 columns / userWidth 的响应式读取 —— 依赖收集的开销
  比函数本身还大。B10 验收的 ① 项就是被这个吃掉余量的（掉帧 0 → 2.6%）。

  这里把宽度、对齐、class 一次算好，列宽或语言变了才重算。
  只有「类型」列的 class 随行取值变，那一处仍在行里判（见 typeClass）。
*/
const cols = computed(() =>
  shown.value.map((c, i) => ({
    key: c.key,
    title: c.title,
    width: colWidth(i) + 'px',
    halign: (c as any).halign || 'left',
    align: (c as any).align || 'left',
    cls: (c as any).cls || '',
    flag: !!(c as any).flag,
    resizable: i < shown.value.length - 1,
  })),
)

/*
  枚举 → 显示文案，按语言预先展平。

  原先每个单元格调一次 t()，那不只是查字典 —— t() 读 lang.value，
  于是每格都往依赖表里挂一笔。展平之后只在语言切换时重算一次。
*/
const typeText = computed<Record<number, string>>(() => {
  const m: Record<number, string> = {}
  for (const k of Object.keys(PACKET_TYPE)) {
    m[+k] = t(PACKET_TYPE[+k] as Key)
  }
  return m
})

const protoText = computed<Record<number, string>>(() => {
  const m: Record<number, string> = {}
  for (const k of Object.keys(DOMAIN_TYPE)) {
    m[+k] = t(DOMAIN_TYPE[+k] as Key)
  }
  return m
})

/*
  ── 拖动列宽 ──────────────────────────────────────────

  监听挂在 window 上而不是手柄上：拖动时鼠标经常会跑出那条 5px 的窄条
  （尤其快速拖动），只听手柄的话会中途断掉。
*/
const MIN_COL = 44

let drag: { key: string; startX: number; startW: number } | null = null

function onDragMove(e: MouseEvent): void {
  if (!drag) return
  userWidth.value = {
    ...userWidth.value,
    [drag.key]: Math.max(MIN_COL, Math.round(drag.startW + e.clientX - drag.startX)),
  }
}

function onDragUp(): void {
  drag = null
  window.removeEventListener('mousemove', onDragMove)
  window.removeEventListener('mouseup', onDragUp)
  document.body.classList.remove('col-resizing')
}

//拖到一半组件被卸载（切页 / 关弹窗）时，监听与整页的 col-resizing 要一起收掉
onBeforeUnmount(() => { if (drag) onDragUp() })

function startResize(e: MouseEvent, c: { key: string; w: number }): void {
  drag = { key: c.key, startX: e.clientX, startW: baseWidth(c) }

  //整页禁选 + 统一光标，否则拖动时会把表头文字刷成选中态
  document.body.classList.add('col-resizing')
  window.addEventListener('mousemove', onDragMove)
  window.addEventListener('mouseup', onDragUp)
}

/** 双击手柄恢复默认宽度 —— 拖乱了不必去猜原来是多少。 */
function resetWidth(key: string): void {
  const next = { ...userWidth.value }
  delete next[key]
  userWidth.value = next
}

const scroller = ref<HTMLElement | null>(null)
const scrollTop = ref(0)
const viewH = ref(400)

/** 视口宽度。最后一列要按它算，所以和高度一样得跟着容器变。 */
const viewW = ref(1200)

/** 是否跟随底部。用户往上翻就停住，方便盯住某一行看。 */
const following = ref(true)

/*
  外部的「自动滚动」开关。

  关掉时立刻停住；重新打开时贴回底部 —— 否则用户勾上之后要等下一批数据到才生效，
  看着像开关没反应。内部的 following 仍然保留：用户往上翻会自己停，
  那是「临时脱离」，不该反过来把外面的开关也关掉。
*/
watch(() => props.follow, (on) => {
  following.value = on
  if (on) scrollToBottom()
})

const total = computed(() => rows.value.length)

const start = computed(() => Math.max(0, Math.floor(scrollTop.value / ROW_H) - OVERSCAN))

const end = computed(() =>
  Math.min(total.value, start.value + Math.ceil(viewH.value / ROW_H) + OVERSCAN * 2),
)

/** 当前渲染窗口。slice 约 40 个元素，每帧一次，可以忽略。 */
const windowRows = computed(() => rows.value.slice(start.value, end.value))

function onScroll(): void {
  const el = scroller.value
  if (!el) return

  scrollTop.value = el.scrollTop

  // 距底 2 行以内算「在底部」，给一点余量，否则滚动条像素误差会让跟随反复开关
  following.value = el.scrollHeight - el.scrollTop - el.clientHeight < ROW_H * 2
}

function scrollToBottom(): void {
  const el = scroller.value
  if (!el) return
  el.scrollTop = el.scrollHeight
  scrollTop.value = el.scrollTop
  following.value = true
}

/*
  把某一行滚进视野。「查找封包」命中之后调它。

  <b>顺带把跟随关掉</b>：不关的话下一批封包一到就贴回底部，刚定位到的那一行立刻被冲走。
  这与用户往上翻时的行为一致（滚动处理器也会把 following 置 false），
  要接着看最新的，点右下角那个「回到最新」即可。

  居中而不是贴到顶：命中的那一条前后往往都要看一眼。
*/
function scrollToIndex(i: number): void {
  const el = scroller.value
  if (!el || i < 0 || i >= total.value) return

  following.value = false
  el.scrollTop = Math.max(0, i * ROW_H - el.clientHeight / 2 + ROW_H / 2)
  scrollTop.value = el.scrollTop
}

// 数据变了：跟随时贴到底；被整表清空时回到顶部。
// watch 一个 getter 时 Vue 也按值比较，所以盯的是<b>条数</b>而不是数组本身
//（数组引用永远不变，盯它一次都不会触发）。
watch(total, () => {
  if (total.value === 0) {
    const el = scroller.value
    if (el) el.scrollTop = 0
    scrollTop.value = 0
    following.value = true
    return
  }

  if (following.value) {
    // 等 DOM 把新的 spacer 高度算出来再贴底
    requestAnimationFrame(scrollToBottom)
  }
})

let ro: ResizeObserver | null = null

onMounted(() => {
  const el = scroller.value
  if (!el) return

  viewH.value = el.clientHeight
  viewW.value = el.clientWidth
  ro = new ResizeObserver(() => {
    viewH.value = el.clientHeight
    viewW.value = el.clientWidth
  })
  ro.observe(el)
})

onBeforeUnmount(() => {
  ro?.disconnect()
  ro = null
})

/** 按 FilterAction 取行配色。颜色来自 C# 的 UiPrefs，前端不写死。 */
function colorOf(action: number): { color?: string; background?: string } {
  const f = props.prefs?.filter
  if (!f) return {}

  switch (action) {
    case FilterAction.Replace:
      return { color: f.replace.fore, background: f.replace.back }
    case FilterAction.Intercept:
      return { color: f.intercept.fore, background: f.intercept.back }
    case FilterAction.Change:
      return { color: f.change.fore, background: f.change.back }
    case FilterAction.NoModify_Display:
      return { color: f.display.fore, background: f.display.back }
    default:
      return {}
  }
}

/*
  发送方向。类型枚举里请求与响应是交错编号的，不能按大小分段。
  注入模式的那些钩子也算进来：Send / SendTo / WSASend 系都是「出」。
*/
const REQUEST_TYPES = new Set([0, 1, 2, 3, 8, 9, 13, 14, 17, 19])

/**
 * 类型列的颜色 —— 全表唯一按<b>取值</b>变色的一格
 * （「请求 / 响应」是这张表里最值得一眼分辨的语义）。
 * 其余列的固定色已经烘进 cols 里了，不再逐格判。
 */
function typeClass(r: AnyRow): string {
  return REQUEST_TYPES.has(r.Type) ? 'c-req' : 'c-resp'
}

function cellText(r: AnyRow, key: string): string | number {
  //查的是预先展平好的表；映射不到时退回原始数字，好排查是不是 C# 加了新枚举值
  if (key === 'Type') return typeText.value[r.Type] ?? r.Type
  if (key === 'DomainType') return protoText.value[(r as ProxyRow).DomainType] ?? (r as ProxyRow).DomainType
  return (r as any)[key]
}

defineExpose({ scrollToBottom, scrollToIndex })
</script>

<template>
  <div class="pl">
    <div ref="scroller" class="pl-scroll" @scroll.passive="onScroll">
      <!-- 表头与行同在一个滚动容器里：sticky 只锁纵向，横向自然跟着一起滚，列不会错位 -->
      <div class="pl-head" :style="{ width: totalWidth + 'px' }">
        <div
          v-for="(c, i) in cols"
          :key="c.key"
          class="pl-cell"
          :style="{ width: c.width, textAlign: c.halign }"
        >
          {{ c.title }}

          <!-- 最后一列铺满剩余宽度，没有「拖宽」这回事 -->
          <span
            v-if="c.resizable"
            class="pl-resize"
            :title="t('col.resizeHint')"
            @mousedown.prevent.stop="startResize($event, shown[i])"
            @dblclick.prevent.stop="resetWidth(c.key)"
          />
        </div>
      </div>

      <!-- 撑出总高度的占位；真实行绝对定位在上面 -->
      <div class="pl-spacer" :style="{ height: total * ROW_H + 'px', width: totalWidth + 'px' }">
        <div class="pl-win" :style="{ transform: `translateY(${start * ROW_H}px)` }">
          <!--
            key 用窗口内下标而不是 r.Id：滚动时窗口整体平移，按下标 key 能让 Vue
            就地改文本、复用这 40 个 DOM 节点；按 Id key 则每滚一行就要销毁重建节点。
            行内没有任何自身状态，就地复用不会串。
          -->
          <div
            v-for="(r, i) in windowRows"
            :key="i"
            class="pl-row"
            :class="{
              sel: r.Id === props.selectedId,
              pick: props.picked.has(r.Id),
              hit: !!colorOf(r.Action).background,
            }"
            :style="colorOf(r.Action)"
            @click="emit('select', r, $event, start + i)"
            @contextmenu.prevent="emit('menu', $event, r)"
            @dblclick="emit('open', r)"
          >
            <div
              v-for="c in cols"
              :key="c.key"
              class="pl-cell"
              :class="c.key === 'Type' ? typeClass(r) : c.cls"
              :style="{ width: c.width, textAlign: c.align }"
            >
              <!--
                国旗与文字同格，不像 WinForms 那样单开一列 —— 省一列宽度，
                而且滚动时图和文永远对得上（两列各自渲染就有对不齐的风险）。
                loading/decoding 都设成同步：图只有 400 字节且已缓存，
                异步解码反而会让滚动时出现一格空白。
              -->
              <img
                v-if="c.flag"
                class="flag"
                :src="flagSrc((r as any)[c.key])"
                alt=""
                loading="eager"
                decoding="sync"
              >{{ cellText(r, c.key) }}
            </div>
          </div>
        </div>
      </div>
    </div>

    <!--
      圆形悬浮按钮，只有一个向下箭头 —— 与聊天窗口「回到最新」是同一个约定，
      不用文字也认得。原来那句「已暂停跟随 · 点此回到底部」搬进 title，
      鼠标停一下仍然能看到解释，读屏则走 aria-label。
    -->
    <button
      v-if="!following"
      class="pl-paused"
      :title="t('list.paused')"
      :aria-label="t('list.paused')"
      @click="scrollToBottom"
    >
      <svg class="ico" viewBox="0 0 24 24"><path d="M12 5v13M6 12l6 6 6-6" /></svg>
    </button>
  </div>
</template>

<style scoped>
.pl {
  position: relative;
  height: 100%;
  min-height: 0;
  border: 1px solid var(--wpe-line);
  overflow: hidden;
  background: var(--wpe-panel);
}

.pl-scroll {
  height: 100%;
  overflow: auto;
  /* 滚动时不做像素级平滑，直接跳，帧内工作量更可控 */
  overflow-anchor: none;
}

.pl-head {
  position: sticky;
  top: 0;
  z-index: 2;
  display: flex;
  align-items: center;   /* 格子在 28px 里垂直居中，原先贴着上沿 */
  height: var(--th-h);
  background: var(--wpe-head);
  border-bottom: 1px solid var(--wpe-line);
}

/*
  表头字与全项目其它表统一：Share Tech Mono 10px、.14em 字距、大写、var(--th-fg)
  （即 style.css 里 .list-page .head 那一份）。原先这张表是 Consolas 12px 加粗 ——
  与其它十来张表并排看就是「有的大有的小」。
  写在 .pl-cell 这一层是因为字体规则在 .pl-cell 上，只改 .pl-head 会被它盖掉。
*/
.pl-head .pl-cell {
  padding-top: 4px;   /* 字形偏上 2px（像素实测），与 style.css 里 .list-page .head > span 同一份补偿 */
  font-family: var(--share);
  font-size: var(--th-size);
  letter-spacing: .14em;
  text-transform: uppercase;
  color: var(--th-fg);
}

.pl-spacer {
  position: relative;
}

.pl-win {
  position: absolute;
  top: 0;
  left: 0;
  will-change: transform;
}

.pl-row {
  display: flex;
  height: 24px;
  line-height: 24px;
  cursor: default;
  border-bottom: 1px solid var(--wpe-rowline);
}

/*
  ── 列配色 ──────────────────────────────────────────────

  整屏一个绿太晃眼，全白又读不出结构。这里按<b>信息层次</b>分色 ——
  原则是「哪些列该被看见、哪些该让位」，不是给每列各挑一个好看的颜色。

    记账列（序号 / 套接字）           最暗    只在对号时才看，平时应该消失
    元数据（时间 / 协议）             次暗    有用，但不是扫描目标
    本地侧（客户端 / 客户端地 / 长度）  冷灰    自己这边，可预期
    远端侧（服务端 / 服务端地）        青      对面是谁，值得留意
    域名                             最亮    定位某条连接时眼睛就落在这一列
    类型                             琥珀/紫  按「请求 / 响应」分色，见 cellClass
    数据                             代码雨绿 保留

  「本地 / 远端」用<b>成对的两个色相，各自一深一浅</b>：地址亮、归属地暗。
  这样四列扫下来能立刻分出左右两侧，而不是八列同色的一大片。

  ⚠️ 这些只管<b>没被滤镜命中</b>的行。命中的行由 colorOf() 打行内样式，
  那 4 组颜色来自 C# 的 UiPrefs、是用户可配的，前端不许写死（CLAUDE.md「契约对齐」）。

  【曾经写错过一条，现已修正】原注释说「行内样式优先级高于 class，两者不会打架」——
  <b>不对</b>。行内 color 打在 .pl-row 上，而列色打在子元素 .pl-cell 上；
  父元素的 color 对子元素只是<b>继承值</b>，任何直接命中子元素的规则都赢过继承，
  跟优先级、跟是不是行内样式都无关。实测 .pl-cell.c-dim 照样赢。
  结果就是命中行<b>只换了底色、前景色从来没生效过</b>，
  UiPrefs 里那四个 ForeColor 等于白设。

  下面 .pl-row.hit .pl-cell 那条就是补这个：让命中行的格子交出自己的颜色、
  回到继承，行内的前景色才落得下来。
*/
.pl-cell.c-dim { color: var(--dim); }
.pl-cell.c-meta { color: var(--muted); }

.pl-cell.c-local { color: var(--dim3); }
.pl-cell.c-local-dim { color: #64748b; }

/*
  远端侧这一对<b>必须走令牌</b>：#00d4ff 就是深色的 --cyan，白底上只有 1.77，
  整列服务端地址等于看不见（浅色模式上线时漏了这一处，后来量出来的）。
*/
.pl-cell.c-remote { color: var(--cyan); }
.pl-cell.c-remote-dim { color: var(--remote2); }

/* 域名是这张表里最常被读的一列，给最高亮度 */
.pl-cell.c-domain { color: var(--bright); }

/* 方向：出去的琥珀、回来的淡紫。两者与青、绿都拉得开，不会跟别的列混 */
.pl-cell.c-req { color: var(--amber); }
.pl-cell.c-resp { color: var(--acc-violet); }

/*
  数据列：代码雨绿 + 极淡辉光。

  --acc-data 深色下是 #35e07a：比主色 --green(#00ff88) 更偏正绿、压暗一档 ——
  这是整屏最密的文字，满饱和的荧光绿读十六进制串很累。
  辉光只加在这一列：其余列要精确辨认字形，发光会把边缘糊掉。
*/
/*
  国旗图。

  【为什么是 16×16 的方框 + object-fit: contain，而不是写死宽高】
  这批图有三种原始尺寸：16×12（普通国旗，254 个）、20×20（欧盟/阿盟这类组织旗，17 个）、
  16×16（Flag_Local，局域网那个，1 个）。宽高同时写死等于强行改宽高比 ——
  之前设成 16×11，方形的那两类就被压扁了。

  contain 让图按原比例装进方框：
    16×12 → 原样 1:1，像素对齐不发虚
    16×16 → 原样 1:1
    20×20 → 缩到 16×16，略软，但只有 17 个且不常见
  方框尺寸固定，所以后面的文字起点不会随图变宽而左右跳。
*/
/*
  列宽拖动手柄：压在每个表头单元格的右边界上。

  宽 7px 而不是 1px —— 1px 的靶子几乎点不中。它落在相邻两列的分界处，
  各占一半，看起来仍是那条分隔线。
*/
.pl-resize {
  position: absolute;
  top: 0;
  right: -3px;
  width: 7px;
  height: 100%;
  cursor: col-resize;
  /* 表头本身有背景，手柄透明即可；hover 时显出一条青线给个反馈 */
  z-index: 3;
}

.pl-resize:hover::after {
  content: "";
  position: absolute;
  top: 4px;
  bottom: 4px;
  left: 3px;
  width: 1px;
  background: var(--cyan);
  box-shadow: 0 0 4px var(--cyan);
}

.flag {
  width: 16px;
  height: 16px;
  object-fit: contain;
  margin-right: 6px;
  vertical-align: -4px;
  flex: none;
}

/*
  命中滤镜的行：格子放弃自己的列色，回到继承，行内的前景色才生效。
  (0,3,0) 压过上面每一条 .pl-cell.c-xxx (0,2,0)。

  整行变成一个颜色是<b>要的效果</b>：这一行已经被打上标记，
  标记色就该盖过「哪一列是什么」的分层 —— WinForms 那边也是整行一个 ForeColor。
*/
.pl-row.hit .pl-cell { color: inherit; }

.pl-cell.c-data {
  color: var(--acc-data);
  text-shadow: 0 0 6px rgb(var(--green-rgb) / 22%);
}

/*
  ══ 悬停与选中：与全项目<b>同一套青色记号</b>（2026-09-09 统一）══

  原来这两条是<b>绿色描边</b>（`outline: 2px solid var(--wpe-accent)`，而那个令牌就是 --green）——
  与全局焦点环 `outline: 2px solid var(--green)` <b>粗细、颜色一模一样</b>，只差一个
  outline-offset（−2 对 +2），隔着屏幕分不出来。用户直接问「是不是没把焦点框去掉」。

  ⚠️ 两条各自都对的规矩撞在了一起，而错的是这里选的颜色：
  · 绿在这套界面里<b>一律表示「正在跑」</b>（状态灯、开始按钮、数据列 —— 「回到最新」
    那颗按钮的注释里就写着这句），拿它标「选中」本来就串了；
  · 「选中」在全项目是<b>青色左边线 + 淡青底</b>（`.list-page .row.sel`），
    这一屏的多选（.pick）本来就是那一套 —— 只有 .sel 是全项目唯一的例外。

  ⚠️ <b>一律用 inset 阴影，不用 background / outline</b>：命中滤镜的行带<b>行内</b> background
  （colorOf() 打的），写 background 会被行内样式压过去；而阴影画在背景之上，压得住。
  这也是 .pick 当初就这么写的理由。

  ⚠️ box-shadow <b>不会跨规则叠加</b>（后面的整条替换前面的），所以每种组合都要把
  「左边线 + 底色」<b>一起写全</b>，不能指望 .pick 那条自己叠上来。
*/

/* 悬停：4% 提亮，与 .list-page .row:hover 同一个观感（那边用的是 background）*/
.pl-row:hover {
  box-shadow: inset 0 0 0 999px rgb(var(--tint-rgb) / 4%);
}

/*
  在多选集里 —— 逐条对齐 `.list-page .row.sel`（左边线 var(--cyan) + 6% 青底），
  悬停时加深到 10%，与那边的 `.row.sel:hover` 同值。
  ⚠️ 底色从 7% 收到 6% 就是为了与那条逐字相同，别再改回去。
*/
.pl-row.pick {
  box-shadow: inset 2px 0 0 var(--cyan), inset 0 0 0 999px rgb(var(--cyan-rgb) / 6%);
}

.pl-row.pick:hover {
  box-shadow: inset 2px 0 0 var(--cyan), inset 0 0 0 999px rgb(var(--cyan-rgb) / 10%);
}

/*
  详情面板正在看的那一行。**同一套青、只是底色实一档**（13%）——
  不再另起一种记号。

  ⚠️ 它与 .pick <b>几乎总是同一行</b>：onSelect 里 `selected = r` 与 `onRowClick(...)`
  是挨着的两句，普通点击会把多选集清成这一条。所以绝大多数时候屏幕上只有一种记号、
  与别的表一模一样；只有多选了好几行时才看得出「其中哪一条的字节在下面」。

  ⚠️ 这两条写在最后：`.pl-row.sel:hover` 与 `.pl-row.pick:hover` 特异度都是 (0,3,0)，
  平局时后写的赢 —— 挪到 .pick 前面会让「正在看的那一行」在悬停时降回 10%。
*/
.pl-row.sel,
.pl-row.sel:hover {
  box-shadow: inset 2px 0 0 var(--cyan), inset 0 0 0 999px rgb(var(--cyan-rgb) / 13%);
}

.pl-cell {
  position: relative;
  flex: none;
  padding: 0 8px;
  overflow: hidden;
  white-space: nowrap;
  text-overflow: ellipsis;
  font-family: Consolas, 'Cascadia Mono', monospace;
  font-size: var(--fs-dense);
}

/*
  「回到最新」悬浮按钮 —— 圆形 + 向下箭头，与聊天窗口那个约定一致。

  【为什么是琥珀，不是绿】绿在这套界面里一律表示「正在跑」（状态灯、开始按钮、
  数据列）。用绿表示「停住了」正好反过来。琥珀是这里的「需要注意但不是错误」色，
  与测试版提示、滤镜命中那格同一套语义。

  <b>唯一破例用圆角的地方。</b>整套界面是方角 + 发丝边框，但这是个浮在内容上的
  动作按钮、不属于版面结构，圆形反而能立刻和底下那张方格子表分开。
*/
/*
  「回到底部」的悬浮按钮，摆在<b>底部正中</b>（与聊天窗口「回到最新」同一个位置约定）。
  原来在右下角，与那一侧的竖直滚动条挤在一起，也不像个「回到最新」的入口。
*/
.pl-paused {
  position: absolute;
  left: 50%;
  bottom: 16px;
  /* ⚠️ 居中靠 transform，下面 :hover 那条<b>必须把这一段带上</b>，否则一悬停就弹回左边 */
  transform: translateX(-50%);
  width: 34px;
  height: 34px;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 0;
  border-radius: 50%;
  background: rgb(var(--chrome-rgb) / 92%);
  backdrop-filter: blur(6px);
  border: 1px solid rgb(var(--amber-rgb) / 45%);
  color: var(--amber);
  cursor: pointer;
  box-shadow: 0 4px 16px rgb(var(--shadow-rgb) / 45%);
  transition: .15s;
}

.pl-paused:hover {
  background: rgb(var(--amber-rgb) / 16%);
  box-shadow: 0 0 16px rgb(var(--amber-rgb) / 28%);
  /* 微微上抬，暗示「点它会动」。⚠️ translateX(-50%) 是居中用的，不能丢 */
  transform: translateX(-50%) translateY(-1px);
}

.pl-paused:focus-visible { outline-offset: 3px; outline-color: var(--amber); }

.pl-paused .ico {
  width: 17px;
  height: 17px;
  stroke: currentColor;
  stroke-width: 2.2;
  fill: none;
  stroke-linecap: round;
  stroke-linejoin: round;
}
</style>
