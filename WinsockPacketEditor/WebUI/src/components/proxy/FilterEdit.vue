<script setup lang="ts">
/*
  滤镜编辑 —— 对应 WinForms 的 Controls/FilterEdit（2016 行，全项目最复杂的弹窗）。

  【滤镜的本质是一张十六进制格子表】
    列号 = 字节位置，1000 列（Operate.FilterConfig.Filter.FilterSize_MaxLen）
      查找位非空 → 这一位要等于这个值
      修改位非空 → 命中后把这一位改成这个值
    格子的<b>标记</b>在 WinForms 里是底色（UiTheme 那三个常量）：
      排除 —— 这一位<b>不能</b>等于该值（查找位）
      递进 —— 每次命中后累加（修改位）
      随机 —— 每次命中随机取值（修改位）

  【三种表形，不是一张表换个样子】
    普通：一张表两行，查找与修改<b>对齐同一列</b>
    高级：查找与修改<b>各自独立</b>的位置 —— 这才是「高级」的意思，
          再配合「修改起始于：包头 / 指定位置」决定修改从哪儿算起
    详见下面「三种表形」与 mHead / mPos 两段。

  【存储格式不在前端】源模型存成 "索引|值," 加三串位置列表，
  解析与拼装都在 Operate 侧（GetFilterEdit_ById / SaveFilterEdit）。
  两边各写一份必然在分隔符、尾逗号、越界索引上走岔。

  【校验也在 C# 侧】与 CheckFilterIsValid 同一份规则 —— 那是「能不能匹配上」的业务约束。
*/
import { computed, nextTick, onBeforeUnmount, reactive, ref, watch } from 'vue'
import { call } from '../../bridge'
import { FilterAction } from '../../bridge/types'
import { t, type Key } from '../../i18n'
import { pushToast } from '../../stores/toast'
import ContextMenu from '../ContextMenu.vue'
import CyberSelect from '../CyberSelect.vue'
import SettingsModal from './SettingsModal.vue'
import { formatOptions } from '../extractor/formats'
import { ICON, type MenuItem } from '../menu'
import { useModal } from '../../useModal'

const props = withDefaults(defineProps<{ id: string | null; mode?: 'proxy' | 'inject' }>(), { mode: 'proxy' })
const emit = defineEmits<{ (e: 'close'): void }>()

/*
  ── 三种表形（照 FilterEdit 的四张 DataTable）────────────────────

    普通            一张表两行：查找 + 修改，列 0…999，<b>对齐同一列</b>
    高级 · 包头     两张表各一行：查找 0…999、修改 0…999，<b>位置互相独立</b>
    高级 · 指定位置 查找 0…999、修改 <b>-1000…999（2000 列）</b>

  「指定位置」那张表的列头带符号，含义是<b>相对匹配点的偏移</b> ——
  WinForms 里是 `for (i = -iSize; i < iSize; i++)`，保存时取的是<b>列名</b>
  而不是循环下标，所以存进库的就是那个带符号的数。
  切到这一档时它会 ScrollColumn 到偏移 0，也就是匹配点本身。

  【查找与修改是两套索引】源模型就是 FSearch / FModify 两个独立的串。
  普通模式下恰好对齐，看不出区别；高级模式下必须分开存，否则改一个会串到另一个。
*/
interface SearchCell { v: string; exclude: boolean }
interface ModifyCell { v: string; progression: boolean; random: boolean }

/**
 * 修改行上的一个取值器替换格：从这一列开始写入指定变量渲染出的字节。
 * 与固定值 / 递进 / 随机互斥（设了替换格就清掉那三样）。
 */
interface ReplaceBinding { ExtractorId: string; VariableId: string; Format: string }

interface EditRow {
  Id: string
  Name: string
  Mode: number
  Action: number
  StartFrom: number
  FunctionMask: number
  AppointHeader: boolean; HeaderContent: string
  AppointSocket: boolean; SocketContent: string
  AppointLength: boolean; LengthContent: string
  AppointPort: boolean; PortContent: string
  IsExecute: boolean; ExecuteType: number; ExecuteId: string
  IsProgressionContinuous: boolean; ProgressionStep: number
  IsProgressionCarry: boolean; ProgressionCarryNumber: number
  IsVariableAction: boolean; CaptureVariable: boolean
  VariableExtractorId: string; VariableId: string
  Search: Array<{ Index: number; Value: string; Exclude: boolean }>
  Modify: Array<{ Index: number; Value: string; Progression: boolean; Random: boolean }>
  ModifyReplacements: Array<{ Index: number; ExtractorId: string; VariableId: string; Format: string }>
}

const busy = ref(false)
const error = ref('')
const f = ref<EditRow | null>(null)

/** 普通模式：一张表，查找与修改共用一个滚动条（两行必须对齐同一列）。 */
const isNormal = computed(() => f.value?.Mode === 0)

/** 高级 + 指定位置：修改表变成 -1000…999 的偏移表。 */
const isOffset = computed(() => f.value?.Mode === 1 && f.value?.StartFrom === 1)

/*
  格子用<b>稀疏</b>存储：只留碰过的列。
  两千列全建成对象是几千个白建的字段，而真实滤镜通常只用前几个字节。
  查找与修改分开存 —— 它们是两套索引，见上面那段说明。
*/
const sCells = ref<Record<number, SearchCell>>({})

/*
  修改位<b>存两份</b>，按「起始于」分家。

  WinForms 那边是四个各自独立的 Table 摆在 Tab 里，切换只换显示、互不搬运，
  保存时只读当前模式对应的那张（FilterEdit 1860–1935）。
  这里没照搬到四份，只在<b>真正会出错的那一处</b>分家：

    mHead —— 普通 与 高级·包头 共用。两者的下标都是「从包头数第几个字节」，
             含义相同，切换时留着不会被曲解，还省得白打一遍。
    mPos  —— 高级·指定位置。下标是<b>相对匹配点的偏移</b>，
             偏移 +2 和绝对位置 2 是完全不同的两个字节。合成一份的话，
             切一下档就会把偏移悄悄读成绝对位置 —— 不报错，滤镜从此改错地方。

  查找位不用分：三种表形下它一律是绝对位置。
*/
const mHead = ref<Record<number, ModifyCell>>({})
const mPos = ref<Record<number, ModifyCell>>({})

const S0: SearchCell = { v: '', exclude: false }
const M0: ModifyCell = { v: '', progression: false, random: false }

/** 当前该用哪一份修改位。 */
const mCells = computed(() => (isOffset.value ? mPos : mHead))

/*
  取值器替换格<b>也按模式分两份</b>，与 mHead / mPos 同一个道理：
  「指定位置」下的索引是相对匹配点的偏移，和绝对位置不是一回事。
*/
const rHead = ref<Record<number, ReplaceBinding>>({})
const rPos = ref<Record<number, ReplaceBinding>>({})
const rCells = computed(() => (isOffset.value ? rPos : rHead))

const searchAt = (i: number): SearchCell => sCells.value[i] || S0
const modifyAt = (i: number): ModifyCell => mCells.value.value[i] || M0
const replaceAt = (i: number): ReplaceBinding | undefined => rCells.value.value[i]

/** 每格代表一个字节：必须完整填写两位；高级查找保留既有的半字节通配符。 */
function isCompleteCellValue(value: string, row: 'search' | 'modify'): boolean {
  const allowed = row === 'search' && f.value?.Mode === 1 ? '[0-9A-F*]' : '[0-9A-F]'
  return new RegExp(`^${allowed}{2}$`).test(value)
}

/** 表很宽，横向滚动后仍能一眼确认哪些格已经填过。 */
const searchFilledCount = computed(() => Object.values(sCells.value).filter(c => isCompleteCellValue(c.v, 'search')).length)
const modifyFilledCount = computed(() => {
  //变量值也是实际写入数据；空的递进 / 随机标记则不算「已填写」。
  const values = Object.values(mCells.value.value).filter(c => isCompleteCellValue(c.v, 'modify')).length
  return values + Object.keys(rCells.value.value).length
})

function filledText(n: number): string {
  return t('flt.e.filledCount').replace('{n}', String(n))
}

/** 写之前才建对象 —— 保持稀疏。 */
function editSearch(i: number, patch: Partial<SearchCell>): void {
  sCells.value[i] = { ...(sCells.value[i] || S0), ...patch }
}

function editModify(i: number, patch: Partial<ModifyCell>): void {
  const m = mCells.value
  m.value[i] = { ...(m.value[i] || M0), ...patch }
}

/** 设为取值器替换格：与固定值 / 递进 / 随机互斥，设了就把那一格清空。 */
function setReplace(i: number, b: ReplaceBinding): void {
  //整份替换引用，确保虚拟格的颜色和 title 都在同一拍重新计算。
  rCells.value.value = { ...rCells.value.value, [i]: b }
  delete mCells.value.value[i]
}

function clearReplace(i: number): void {
  //不要只 delete 深层属性：横向虚拟表复用 DOM 节点时，显式换引用才能可靠撤掉旧 title。
  const next = { ...rCells.value.value }
  delete next[i]
  rCells.value.value = next
}

/* ── 载入 ──────────────────────────────────────────────────── */

watch(() => props.id, async (id) => {
  if (!id) return

  error.value = ''
  f.value = null
  sCells.value = {}
  mHead.value = {}
  mPos.value = {}
  rHead.value = {}
  rPos.value = {}
  sel.value = null

  try {
    const r = await call<{ row: EditRow | null }>('getFilterEdit', { id })

    if (!r?.row) {
      error.value = t('flt.e.notFound')
      return
    }

    f.value = r.row
    //命中后执行里的取值器只剩「取值器赋值」；类型选到取值器就预置好。
    if (f.value.ExecuteType === 5) f.value.CaptureVariable = true
    void loadExtractors()

    const sm: Record<number, SearchCell> = {}
    for (const c of r.row.Search || []) sm[c.Index] = { v: c.Value || '', exclude: !!c.Exclude }
    sCells.value = sm

    /*
      落到哪一份由这条滤镜<b>自己保存时的</b>模式决定 ——
      f.value 已经赋好，isOffset 此刻就是对的。
      放错一份就等于把偏移当绝对位置读，见上面 mHead / mPos 那段。
    */
    const mm: Record<number, ModifyCell> = {}
    for (const c of r.row.Modify || []) {
      mm[c.Index] = { v: c.Value || '', progression: !!c.Progression, random: !!c.Random }
    }
    mCells.value.value = mm

    //取值器替换格与修改位同一套索引，落到同一份。
    const rb: Record<number, ReplaceBinding> = {}
    for (const c of r.row.ModifyReplacements || []) {
      rb[c.Index] = { ExtractorId: guidKey(c.ExtractorId), VariableId: guidKey(c.VariableId), Format: c.Format || '' }
    }
    rCells.value.value = rb

    await nextTick()
    resetGrids()
  } catch (e) {
    console.error('[flt] 读取滤镜失败', e)
    error.value = String(e)
  }
}, { immediate: true })

/* ── 格子：横向虚拟滚动 ────────────────────────────────────── */

/*
  1000 列 × 46px = 46000px。全铺出来是一千个可编辑输入框，
  首次渲染就要好几百毫秒，而且每次改一个格子都要重算。
  所以照 PacketList / AccountList 那套定高窗口的做法，换成<b>横向</b>：
  只渲染视口里的约 40 列，代价与总列数无关。

  COL_W 必须与 .cell 的 width 逐像素一致 —— 对不上会随列号线性漂移，
  滚到第 900 列时偏出十几列，而且不报任何错。
*/
const COL_W = 46
const MAX = 1000
const OVERSCAN = 4

/** 修改表的列下界。0 = 绝对位置；-1000 = 相对匹配点的偏移。 */
const mLo = computed(() => (isOffset.value ? -MAX : 0))

/*
  两个独立的滚动状态。

  普通模式只用 sg（查找与修改在同一张表里，必须共用一个滚动条 ——
  两行是按列对齐的，各滚各的就对不上了）；
  高级模式两张表各自独立，正是「查找与修改位置无关」这件事的界面体现。
*/
function makeGrid(lo: () => number, hi: () => number) {
  const el = ref<HTMLElement | null>(null)
  const left = ref(0)
  const w = ref(900)

  const start = computed(() => Math.max(lo(), Math.floor(left.value / COL_W) + lo() - OVERSCAN))
  const end = computed(() => Math.min(hi(), start.value + Math.ceil(w.value / COL_W) + OVERSCAN * 2))

  const cols = computed(() => {
    const out: number[] = []
    for (let i = start.value; i < end.value; i++) out.push(i)
    return out
  })

  function onScroll(): void {
    if (el.value) left.value = el.value.scrollLeft
  }

  /** 把某个列号滚到最左。偏移表里 0 不是第一列，所以要减去下界。 */
  function scrollToCol(n: number): void {
    if (el.value) el.value.scrollLeft = Math.max(0, (n - lo()) * COL_W)
  }

  return { el, left, w, start, cols, onScroll, scrollToCol }
}

/*
  用 reactive 包一层，不是多此一举：模板里只会自动解包<b>顶层</b>的 ref，
  普通对象里的嵌套 ref 拿到的是 ref 对象本身 —— `sg.start * COL_W` 会算成 NaN，
  而且不报错，表现成整张表停在第一屏。reactive 会把嵌套 ref 一并解包，
  读写都照常（`sg.w = n` 等价于 `w.value = n`）。
*/
const sg = reactive(makeGrid(() => 0, () => MAX))
const mg = reactive(makeGrid(() => mLo.value, () => MAX))

let ros: ResizeObserver[] = []

function resetGrids(): void {
  for (const r of ros) r.disconnect()
  ros = []

  for (const g of [sg, mg]) {
    const el = g.el
    if (!el) continue

    g.w = el.clientWidth
    const r = new ResizeObserver(() => { g.w = el.clientWidth })
    r.observe(el)
    ros.push(r)
  }

  sg.scrollToCol(0)
  //偏移表开在「偏移 0」上 —— 与 WinForms 切过去时 ScrollColumn 到匹配点是同一个用意
  mg.scrollToCol(0)
}

onBeforeUnmount(() => { for (const r of ros) r.disconnect(); ros = [] })

/*
  切模式 / 切起始于都要重来一遍：
  表的列范围变了，旧的 scrollLeft 换算过去会落在莫名其妙的列上。
*/
watch([isNormal, isOffset], async () => {
  await nextTick()
  resetGrids()
})

/** 跳到某一列。1000（乃至 2000）列全铺时找位置全靠它。 */
const jumpTo = ref('')

function jump(): void {
  const n = Number(jumpTo.value)
  if (!Number.isFinite(n)) return

  /*
    查找表的列头永远是 001 起算的<b>字节序号</b>，所以输入 1 要落到第 0 列。
    修改表只有「指定位置」那一档换成了带符号的偏移，那时输入几就是几
    （0 = 匹配点，负数在它左边）。
  */
  sg.scrollToCol(Math.min(MAX - 1, Math.max(0, n - 1)))
  mg.scrollToCol(
    isOffset.value
      ? Math.min(MAX - 1, Math.max(-MAX, n))
      : Math.min(MAX - 1, Math.max(0, n - 1)),
  )
}

/* ── 格子：编辑与选中 ──────────────────────────────────────── */

/*
  只允许 0-9 A-F，两位。<b>修改位额外允许 `*` 通配符</b> ——
  普通模式的查找位不允许、高级模式的查找位允许，这是照抄
  FilterEdit 里 VerifyHexCharWithWildcard 的 allowWildcard 传参，
  看着不对称，但那是源程序的规则，不自己改。
*/
function clean(v: string, wildcard: boolean): string {
  const ok = wildcard ? /[0-9A-F*]/ : /[0-9A-F]/
  return (v || '').toUpperCase().split('').filter((c) => ok.test(c)).join('').slice(0, 2)
}

function allowWildcard(row: 'search' | 'modify'): boolean {
  //通配符只属于高级查找；修改行最终会按字节解析，不能接受 *。
  return row === 'search' && f.value?.Mode === 1
}

function onCellInput(i: number, row: 'search' | 'modify', e: Event): void {
  const el = e.target as HTMLInputElement
  const v = clean(el.value, allowWildcard(row))
  el.value = v

  /*
    把值删空时<b>顺手撤掉排除标记</b>。

    WinForms 只在设标记那一刻校验，之后把值删掉，紫底还留着 ——
    保存时那条排除会被丢掉（值为空就整格跳过），于是界面显示的和存进去的对不上。
    既然「空值不能排除」，那让标记跟着值走才是完整的规则。
  */
  if (row === 'search') editSearch(i, v ? { v } : { v, exclude: false })
  else {
    //替换格与固定值互斥：往替换格键入字节，就等于把它改回普通修改位
    if (v && replaceAt(i)) clearReplace(i)
    editModify(i, { v })
  }
}

/**
 * 先允许用户键入第一个半字节，离开格子才判定「填写完成」。
 * 不完整值不能留在表里误导计数或在稍后保存时才暴露。
 */
function onCellBlur(i: number, row: 'search' | 'modify'): void {
  const value = valueAt(row, i)
  if (!value || isCompleteCellValue(value, row)) return

  if (row === 'search') editSearch(i, { v: '', exclude: false })
  else editModify(i, { v: '' })

  pushToast('error', t('flt.e.byteRequired'))
}

/**
 * 列头。普通 / 包头表从 1 起算显示成 001。
 *
 * 偏移表<b>一律补到三位</b>（000 / +001 / -999），与另一张表的位数看齐 ——
 * 不补位的话 `+9` 和 `+999` 宽度差一倍，扫过去列号像在左右晃。
 */
function colLabel(i: number, offset: boolean): string {
  if (!offset) return (i + 1).toString().padStart(3, '0')

  const n = Math.abs(i).toString().padStart(3, '0')
  return i === 0 ? n : (i > 0 ? '+' : '-') + n
}

/*
  选中<b>一格</b>，键是 "行:列"。右键菜单的标记动作只作用于它。

  曾经支持 Shift 连选 / Ctrl 加选，已去掉：标记是逐格的语义
  （这一位排除、那一位递进），一次改一格才对得上心里想的事；
  成段选中还会让「右键落在选中区之外要不要改选中」这类规则冒出来，
  而这一屏本来就够复杂了。
*/
const sel = ref<string | null>(null)
const keyOf = (row: string, i: number) => row + ':' + i

function pick(row: 'search' | 'modify', i: number): void {
  sel.value = keyOf(row, i)
}

/* ── 右键菜单 ──────────────────────────────────────────────── */

const menuAt = ref<{ x: number; y: number } | null>(null)

function openMenu(row: 'search' | 'modify', i: number, e: MouseEvent): void {
  //右键就是选中这一格 —— 只能选一格，不存在「落在选中区之外」这回事
  sel.value = keyOf(row, i)
  menuAt.value = { x: e.clientX, y: e.clientY }
}

/**
 * 菜单项照抄 FilterEdit 的 cmsFilterEdit_*。
 * 按选中的是查找位还是修改位给不同的项 —— 排除只属于查找位，递进 / 随机只属于修改位。
 */
const menuItems = computed<MenuItem[]>(() => {
  const parts = sel.value ? sel.value.split(':') : []
  const row = parts[0] || ''
  const out: MenuItem[] = []

  if (row === 'search') {
    out.push({ id: 'exclude-on', label: t('flt.e.excludeOn'), icon: ICON.plus })
    out.push({ id: 'exclude-off', label: t('flt.e.excludeOff'), icon: ICON.minus })
  }

  if (row === 'modify') {
    /*
      取值器替换格：右键修改行的某一格，设为「从这一格起写入某个取值器变量」。
      与递进 / 随机 / 固定值互斥 —— 替换格本身就是这一格的写值方式。
    */
    out.push({ id: 'progression-on', label: t('flt.e.progressionOn'), icon: ICON.step })
    out.push({ id: 'random-on', label: t('flt.e.randomOn'), icon: ICON.dice })
    out.push({ id: 'replace-on', label: t('flt.e.replaceOn'), icon: ICON.link })
    out.push({ id: 'mark-off', label: t('flt.e.markOff'), icon: ICON.minus })
  }

  /*
    剪贴板四项，照 FilterEdit 的 cmsFilterEdit_Copy / Cut / Paste / Delete。
    快捷键提示写在 label 里 —— MenuItem 没有单独的副标题字段，
    而 WinForms 那边这四项也是带着 Ctrl+C / Ctrl+X / Ctrl+V / Del 显示的。
  */
  out.push({ divider: true })
  out.push({ id: 'copy', label: t('flt.e.copy'), icon: ICON.copy })
  out.push({ id: 'cut', label: t('flt.e.cut'), icon: ICON.cut })
  out.push({ id: 'paste', label: t('flt.e.paste'), icon: ICON.paste })
  out.push({ id: 'del', label: t('flt.e.del'), icon: ICON.del, danger: true })

  out.push({ divider: true })
  out.push({ id: 'clear', label: t('flt.e.clearCells'), icon: ICON.clear, danger: true })

  return out
})

/*
  格子里是真的 <input>，所以 Ctrl+V 默认会把整段文本塞进<b>这一个</b>格子，
  再被 clean() 削成两位 —— 「AA BB CC」只剩「AA」。
  拦下来交给 pasteAt，键盘与右键菜单才是同一个行为（WinForms 的 Ctrl+V 也是铺开的）。

  Ctrl+C / Ctrl+X / Del <b>不拦</b>：那三样在输入框里就是选中文本的复制剪切与删字符，
  是人预期的编辑行为，抢过来反而别扭。
*/
function onCellPaste(row: 'search' | 'modify', i: number, e: ClipboardEvent): void {
  void pasteAt(row, i, e.clipboardData?.getData('text') ?? undefined)
}

/** 当前选中格的值 —— 复制 / 剪切 / 删除都要先看它空不空。 */
function valueAt(row: string, i: number): string {
  return row === 'search' ? searchAt(i).v : modifyAt(i).v
}

/**
 * 粘贴：从选中格开始<b>连续铺开若干格</b>，这是 PastePacketData 的核心行为。
 *
 * WinForms 先用 IsHexString（`^([A-Fa-f0-9]{2}\s?)+$`）验一遍，再按<b>空格</b>切开
 * 逐格写下去，写到表尾就停。这里有一处<b>刻意的不同</b>：
 * 那个正则也放行不带空格的 "AABBCC"，而按空格切出来只有一段，
 * 于是 WinForms 会把 6 个字符整个塞进一个格子 —— 存库时就是 `索引|AABBCC`，
 * 一个格子装了三个字节。这里改成先去掉所有空白、再按两位切，
 * "AA BB CC" 与 "AABBCC" 都得到三格，也不会有超长的格子。
 */
async function pasteAt(row: 'search' | 'modify', i: number, text?: string): Promise<void> {
  //paste 事件自带数据，不必再问一次剪贴板；右键菜单没有事件，才走桥去读
  const raw = (text ?? (await call<{ text: string }>('clipboardRead'))?.text ?? '').trim()

  //与 IsHexString 同一份规则：两位一组，组间可以有空白
  if (!raw || !/^([A-Fa-f0-9]{2}\s*)+$/.test(raw)) {
    pushToast('error', t('flt.e.invalidHex'))
    return
  }

  const bytes = (raw.replace(/\s+/g, '').toUpperCase().match(/.{2}/g) || [])
  const hi = MAX
  const lo = row === 'modify' ? mLo.value : 0
  let n = 0

  for (let k = 0; k < bytes.length; k++) {
    const at = i + k
    if (at < lo || at >= hi) break   //写到表尾就停，与 WinForms 一致

    if (row === 'search') editSearch(at, { v: bytes[k] })
    else {
      //与键盘键入同一条规矩：往替换格写固定字节就等于改回普通修改位
      if (replaceAt(at)) clearReplace(at)
      editModify(at, { v: bytes[k] })
    }

    n++
  }

  if (n > 0) pushToast('success', t('flt.e.pasteOk'))
}

function onMenuPick(id: string): void {
  if (!sel.value) return

  const [row, s] = sel.value.split(':')
  const i = Number(s)

  /* ── 剪贴板四项 ────────────────────────────────────────── */

  if (id === 'copy' || id === 'cut' || id === 'del') {
    /*
      三项都要求格子非空（照 WinForms 的 IsNullOrEmpty 判断）。
      不同的是 WinForms 空值时<b>什么都不做、也不吭声</b>，
      点了没反应最容易被当成程序坏了 —— 这里补一句提示。
    */
    const v = valueAt(row, i)

    if (!v) {
      pushToast('warning', t('flt.e.emptyCell'))
      return
    }

    if (id === 'del') {
      //只清值，不动标记 —— 与「取消标记」是两件事
      if (row === 'search') editSearch(i, { v: '', exclude: false })
      else editModify(i, { v: '' })

      pushToast('success', t('flt.e.delOk'))
      return
    }

    void call('clipboardWrite', { text: v }).then(() => {
      if (id === 'cut') {
        if (row === 'search') editSearch(i, { v: '', exclude: false })
        else editModify(i, { v: '' })
      }

      pushToast('success', t(id === 'cut' ? 'flt.e.cutOk' : 'flt.e.copyOk'))
    })

    return
  }

  if (id === 'paste') {
    void pasteAt(row as 'search' | 'modify', i)
    return
  }

  if (id === 'exclude-on') {
    /*
      <b>空格子不能设排除</b>（照 FilterEdit 的 cmsFilterEdit_Exclude_Enable）。
      「这一位不能等于（空）」本身不成立，存的时候也会被丢掉 ——
      与其让人标上、看见变紫、存完又没了，不如当场说清楚。

      刻意不把菜单项压暗：压暗只说「不能点」，不说为什么，
      而这里的原因恰恰是用户自己能解决的（先填两位十六进制）。
      与账号列表「没选中就提示」是同一条口径。
    */
    if (!searchAt(i).v) {
      pushToast('error', t('flt.e.excludeEmpty'))
      return
    }

    editSearch(i, { exclude: true })
  } else if (id === 'exclude-off') editSearch(i, { exclude: false })
  //替换格始终从同一个菜单入口打开；重新选择即覆盖当前绑定。
  else if (id === 'replace-on') openBind(i)
  //递进与随机互斥：源模型里也是 else-if，一位不可能既递进又随机
  else if (id === 'progression-on') { clearReplace(i); editModify(i, { progression: true, random: false }) }
  //随机<b>连值一起清掉</b>（ctSelect.Text = string.Empty）—— 随机位不需要预设值，
  //留着一个再也用不上的值只会让人以为它还起作用
  else if (id === 'random-on') { clearReplace(i); editModify(i, { v: '', random: true, progression: false }) }
  //取消标记也负责取消变量值：三种修改方式在同一格上互斥。
  else if (id === 'mark-off') { clearReplace(i); editModify(i, { progression: false, random: false }) }
  else if (id === 'clear') {
    if (row === 'search') editSearch(i, { v: '', exclude: false })
    else { clearReplace(i); editModify(i, { v: '', progression: false, random: false }) }
  }
}

/* ── 各段选项 ──────────────────────────────────────────────── */

const ACTIONS: Array<{ v: number; label: Key }> = [
  { v: FilterAction.Replace, label: 'proxy.act.replace' },
  { v: FilterAction.Change, label: 'proxy.act.change' },
  { v: FilterAction.Intercept, label: 'proxy.act.intercept' },
  { v: FilterAction.NoModify_Display, label: 'proxy.act.display' },
  { v: FilterAction.NoModify_NoDisplay, label: 'proxy.act.hide' },
]

interface ExtractorChoice {
  Id: string; Name: string; IsEnable: boolean
  Variables: Array<{ Id: string; Name: string; Kind: number; DataType: number; Value: string; CurrentValue?: string }>
}
const extractorChoices = ref<ExtractorChoice[]>([])
/** C# 读取已保存绑定时会输出大写 GUID；统一列表的 GUID，避免同一引用因大小写不同显示为「?」。 */
function guidKey(id: string): string { return String(id || '').toUpperCase() }
async function loadExtractors(): Promise<void> {
  const rows = (await call<{ rows: ExtractorChoice[] }>('getPacketExtractors')).rows || []
  extractorChoices.value = rows.map(extractor => ({
    ...extractor,
    Id: guidKey(extractor.Id),
    Variables: (extractor.Variables || []).map(variable => ({ ...variable, Id: guidKey(variable.Id) })),
  }))
}
const captureVariables = computed(() => f.value ? (extractorChoices.value.find(x => x.Id === f.value!.VariableExtractorId)?.Variables || []).filter(x => x.Kind === 1) : [])
//取值器动作的两个选择框也走全局统一的 CyberSelect；原生 select 的展开菜单由系统绘制，
//会脱离当前暗色主题。切换取值器时必须清空变量，避免把旧取值器的变量 Id 留到保存。
const extractorOptions = computed(() => extractorChoices.value.map((x) => ({ value: x.Id, label: x.Name })))
const captureVariableOptions = computed(() => captureVariables.value.map((x) => ({ value: x.Id, label: x.Name })))
function selectCaptureExtractor(id: string | number): void {
  if (!f.value) return
  f.value.VariableExtractorId = String(id)
  f.value.VariableId = ''
}

/* ── 取值器替换格：绑定编辑弹窗 ─────────────────────────────── */

/*
  右键修改行的某一格 → 选取值器 + 变量 + 写入格式。
  GUID 定位，改名字不会失效（旧的模板按名称引用，改名就静默坏掉）。

  变量可以是任意一种（简单值 / 从封包取值 / 表达式）：表达式会经引擎算好，
  从封包取值则依赖此前某条滤镜的「取值器赋值」抓进来的值。
*/
const bindAt = ref<number | null>(null)
const bindDraft = ref<{ ExtractorId: string; VariableId: string; Format: string } | null>(null)
const bindKeyword = ref('')

const bindVariables = computed(() => {
  if (!bindDraft.value) return []
  return extractorChoices.value.find(x => x.Id === bindDraft.value!.ExtractorId)?.Variables || []
})
const bindVariable = computed(() => bindVariables.value.find(x => x.Id === bindDraft.value?.VariableId) || null)
const bindFormatOptions = computed(() => (bindVariable.value ? formatOptions(bindVariable.value.DataType) : []))
const bindCandidates = computed(() => extractorChoices.value
  .flatMap(extractor => extractor.Variables.map(variable => ({ extractor, variable })))
  .filter(x => !bindKeyword.value.trim() || (x.extractor.Name + ' ' + x.variable.Name).toLowerCase().includes(bindKeyword.value.trim().toLowerCase())))

function bindTypeName(type: number): string { return ['整数', '浮点数', '字节数组', '字符串'][type] || '未知类型' }
function bindKindName(kind: number): string { return ['简单值', '从封包取值', '表达式'][kind] || '未知类型' }
function bindCurrentValue(variable: ExtractorChoice['Variables'][number]): string {
  if (variable.CurrentValue !== undefined && variable.CurrentValue !== '') return variable.CurrentValue
  return variable.Kind === 0 ? (variable.Value || '—') : '—'
}

/** 取该变量类型的默认格式（选项里的第一项）。 */
function defaultFormat(dataType: number): string {
  return formatOptions(dataType)[0]?.value ?? ''
}

function openBind(i: number): void {
  const existing = replaceAt(i)

  //一个取值器都没有时给一句提示，不要点了什么都没发生
  if (!existing && extractorChoices.value.length === 0) { pushToast('warning', t('flt.e.replaceNoExtractor')); return }

  const extractorId = existing?.ExtractorId || extractorChoices.value[0]?.Id || ''
  const vars = extractorChoices.value.find(x => x.Id === extractorId)?.Variables || []
  const variableId = existing?.VariableId && vars.some(x => x.Id === existing.VariableId) ? existing.VariableId : (vars[0]?.Id || '')
  const variable = vars.find(x => x.Id === variableId)

  //格式要么沿用旧的，要么按当前变量类型取回默认值（换过变量类型时旧格式就失效了）
  let format = existing?.Format ?? ''
  if (!variable || !formatOptions(variable.DataType).some(o => o.value === format)) format = variable ? defaultFormat(variable.DataType) : ''

  bindAt.value = i
  bindDraft.value = { ExtractorId: extractorId, VariableId: variableId, Format: format }
  bindKeyword.value = ''
}

/** 从完整变量表选择一项；同一入口统一重置成该类型的合法默认格式。 */
function selectBindVariable(extractorId: string, variableId: string): void {
  if (!bindDraft.value) return
  const variable = extractorChoices.value.find(x => x.Id === extractorId)?.Variables.find(x => x.Id === variableId)
  if (!variable) return
  bindDraft.value.ExtractorId = extractorId
  bindDraft.value.VariableId = variableId
  bindDraft.value.Format = defaultFormat(variable.DataType)
}

function closeBind(): void {
  bindAt.value = null
  bindDraft.value = null
}

function saveBind(): void {
  if (bindAt.value === null || !bindDraft.value) return

  const d = bindDraft.value

  if (!d.ExtractorId || !d.VariableId) { pushToast('warning', t('flt.e.replaceNoVariable')); return }

  //数值/浮点必须有一个明确的写入格式，空格式会退化成 8 字节
  const v = bindVariable.value
  if (v && (v.DataType === 0 || v.DataType === 1) && !d.Format) { pushToast('warning', t('flt.e.replaceNeedFormat')); return }

  setReplace(bindAt.value, { ExtractorId: d.ExtractorId, VariableId: d.VariableId, Format: d.Format })
  closeBind()
}

/** 格子悬停提示：这条替换格写的是哪个取值器的哪个变量、什么格式。 */
function replaceTitle(i: number): string {
  const b = replaceAt(i)
  if (!b) return ''
  const ex = extractorChoices.value.find(x => x.Id === b.ExtractorId)
  const v = ex?.Variables.find(x => x.Id === b.VariableId)
  return !ex || !v ? '' : ex.Name + '.' + v.Name + (b.Format ? ' · ' + b.Format : '')
}

/** 引用被删除时不再拿 ? 占位，沿用单元格原生悬停提示样式。 */
function replaceMissing(i: number): boolean {
  const b = replaceAt(i)
  if (!b) return false
  const ex = extractorChoices.value.find(x => x.Id === b.ExtractorId)
  return !ex || !ex.Variables.some(x => x.Id === b.VariableId)
}

/**
 * 横向虚拟表默认按列号复用格子 DOM。把绑定状态编进 key，取消变量值时就会重建该格，
 * 连同浏览器可能缓存的原生 title 提示一起丢掉。
 */
function replaceDomKey(prefix: string, i: number): string {
  const b = replaceAt(i)
  return prefix + i + ':' + (b ? b.ExtractorId + ':' + b.VariableId + ':' + b.Format : 'none')
}

/*
  作用域<b>随 mode 变</b>——与 LeachSetting 的类别开关同一条口径。

  FunctionMask 有 12 位，WinForms 里是两张互斥的页，按宿主窗体挑一张（FilterEdit 96–103）：

      this.form is IInjectMode → tpInjectMode  Send / SendTo / Recv / RecvFrom
                                               + WSA 那四个（第 0~7 位）
      this.form is IProxyMode  → tpProxyMode   TCP / UDP 的请求与响应（第 8~11 位）

  道理很直接：注入模式钩的是 WinSock 函数，代理模式根本不经过那些钩子；
  在代理模式下勾 WSASend 只会得到一条永不命中的滤镜，反过来在注入模式勾 TCP 请求同理。
  <b>位序照 Operate.cs 的 MaskToFunction</b>：0 Send · 1 SendTo · 2 Recv · 3 RecvFrom
  · 4 WSASend · 5 WSASendTo · 6 WSARecv · 7 WSARecvFrom（注入）；8 TCP_Req · 9 UDP_Req
  · 10 TCP_Resp · 11 UDP_Resp（代理）。

  <b>看不见的那半边不会丢。</b>save() 把 f.FunctionMask 整个交回去，没有界面碰过的位
  保持原值 —— 一条在注入模式下建的滤镜，在代理模式改完名字存回去，它的 Send 位仍然在。
*/
const PROXY_FUNCS: Array<{ bit: number; label: Key }> = [
  { bit: 8, label: 'pt.tcpReq' }, { bit: 10, label: 'pt.tcpResp' },
  { bit: 9, label: 'pt.udpReq' }, { bit: 11, label: 'pt.udpResp' },
]
const INJECT_FUNCS: Array<{ bit: number; label: Key }> = [
  { bit: 0, label: 'pt.ws2Send' }, { bit: 1, label: 'pt.ws2SendTo' },
  { bit: 2, label: 'pt.ws2Recv' }, { bit: 3, label: 'pt.ws2RecvFrom' },
  { bit: 4, label: 'pt.wsaSend' }, { bit: 5, label: 'pt.wsaSendTo' },
  { bit: 6, label: 'pt.wsaRecv' }, { bit: 7, label: 'pt.wsaRecvFrom' },
]
const FUNCS = computed(() => (props.mode === 'inject' ? INJECT_FUNCS : PROXY_FUNCS))

function hasFunc(bit: number): boolean {
  return !!f.value && (f.value.FunctionMask & (1 << bit)) !== 0
}

function toggleFunc(bit: number): void {
  if (f.value) f.value.FunctionMask ^= (1 << bit)
}

/* ── 执行：命中之后再干一件事 ──────────────────────────────── */

/*
  勾上「执行」后，这条滤镜命中时会接着跑另一张表里的一项。

  <b>枚举值有个坑</b>：FilterExecuteType 是 Send=0 Robot=1 <b>None=2</b> Filter=3 WareHouse=4，
  而 WinForms 的下拉只摆四项、下标是 0 1 2 3 —— 中间跳过了 None，
  两套编号对不上。存进库的是<b>枚举值</b>，所以这里一律按枚举值走，
  不要图省事用数组下标（那样「滤镜」会存成 None、「仓库」会存成 Filter）。
*/
const EXEC_TYPES: Array<{ v: number; label: Key }> = [
  { v: 0, label: 'proxy.nav.send' },
  { v: 1, label: 'proxy.nav.robot' },
  { v: 3, label: 'proxy.nav.filter' },
  { v: 4, label: 'proxy.nav.warehouse' },
  { v: 5, label: 'proxy.nav.extractors' },
]

const execTargets = ref<Array<{ Id: string; Name: string }>>([])

//两个下拉的选项（CyberSelect 收 { value, label }）；类型那份跟着语言变，所以是 computed
const execTypeOptions = computed(() => EXEC_TYPES.map((x) => ({ value: x.v, label: t(x.label) })))
const execTargetOptions = computed(() => execTargets.value.map((x) => ({ value: x.Id, label: x.Name })))

/** 取当前类型下的候选项。滤镜那一档要排掉自己 —— 执行自己就是死循环。 */
async function loadExecTargets(): Promise<void> {
  if (!f.value) return

  const r = await call<{ items: Array<{ Id: string; Name: string }> }>('getExecuteTargets', {
    type: f.value.ExecuteType,
    excludeId: f.value.Id,
  })

  execTargets.value = r?.items || []

  /*
    换类型之后原来的 Id 多半不在新列表里，留着它下拉会显示空白，
    保存下去就是「勾了执行、却指向一个不存在的目标」。
    命中就保留（首次载入的情况），否则退回第一项；一项都没有就清空。
  */
  const hit = execTargets.value.some((x) => x.Id === f.value!.ExecuteId)

  if (!hit) {
    f.value.ExecuteId = execTargets.value.length > 0 ? execTargets.value[0].Id : ''
  }
}

//类型一变就换一批候选项；勾选框刚打开时也要取一次（此前可能从没取过）
watch(() => [f.value?.ExecuteType, f.value?.IsExecute] as const, () => {
  if (f.value?.IsExecute) void loadExecTargets()
})

const APPOINTS: Array<{ on: keyof EditRow; val: keyof EditRow; label: Key; ph: Key }> = [
  { on: 'AppointHeader', val: 'HeaderContent', label: 'flt.ap.head', ph: 'flt.e.headPh' },
  { on: 'AppointSocket', val: 'SocketContent', label: 'flt.ap.socket', ph: 'flt.e.socketPh' },
  { on: 'AppointLength', val: 'LengthContent', label: 'flt.ap.length', ph: 'flt.e.lengthPh' },
  { on: 'AppointPort', val: 'PortContent', label: 'flt.ap.port', ph: 'flt.e.portPh' },
]

/* ── 保存 ──────────────────────────────────────────────────── */

async function save(): Promise<void> {
  if (!f.value) return

  //输入时允许先键入第一个半字节；保存时必须补齐，不能把 "A" 当作 0A 悄悄写入。
  const badSearch = Object.values(sCells.value).some(c => c.v && !isCompleteCellValue(c.v, 'search'))
  const badModify = Object.values(mCells.value.value).some(c => c.v && !isCompleteCellValue(c.v, 'modify'))
  if (badSearch || badModify) {
    error.value = t('flt.e.byteRequired')
    return
  }

  busy.value = true
  error.value = ''

  try {
    /*
      口径逐条照 FilterEdit 的保存循环（1820–1935），包括那处不对称：

        查找位 —— 值为空就整格跳过，<b>排除标记也一起丢掉</b>。
                  「这一位不能等于（空）」本来也不成立。
        修改位 —— 值为空<b>仍然</b>记下递进 / 随机标记。源码里这两个 append
                  在 IsNullOrEmpty 判断之外，不是笔误：随机位本来就不需要预设值。

      另外<b>不过滤负索引</b>：「指定位置」下 -1000…-1 是合法偏移，
      交给 C# 侧按 StartFrom 定下界（SaveFilterEdit 里的 lo）。
    */
    const S = []

    for (const k of Object.keys(sCells.value)) {
      const i = Number(k)
      const c = sCells.value[i]
      if (c.v) S.push({ Index: i, Value: c.v, Exclude: c.exclude })
    }

    const M = []
    const mm = mCells.value.value   //当前模式那一份，见 mHead / mPos

    for (const k of Object.keys(mm)) {
      const i = Number(k)
      const c = mm[i]

      if (c.v || c.progression || c.random) {
        M.push({ Index: i, Value: c.v, Progression: c.progression, Random: c.random })
      }
    }

    //取值器替换格：当前模式那一份，与修改位共用同一套索引。
    const R = []
    const rr = rCells.value.value

    for (const k of Object.keys(rr)) {
      const b = rr[Number(k)]

      if (b && b.ExtractorId && b.VariableId) {
        R.push({ Index: Number(k), ExtractorId: b.ExtractorId, VariableId: b.VariableId, Format: b.Format || '' })
      }
    }

    const r = await call<{ ok: boolean; error: string }>('saveFilterEdit', {
      row: { ...f.value, Search: S, Modify: M, ModifyReplacements: R },
    })

    if (!r?.ok) {
      error.value = r?.error || ''
      return
    }

    emit('close')
  } catch (e) {
    console.error('[flt] 保存滤镜失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}

/* 登记进模态栈：父窗体因此变 inert；自己被后开的弹窗盖住时也会 inert。见 useModal.ts */
const { covered } = useModal(() => props.id !== null)
</script>

<template>
  <!--
    ⚠️ <b>Teleport 到 body</b> —— 不是为了好看，是必须的，两个理由都在 useModal.ts 里：
    ① 代理模式的 .proxy 是 z-index: 10 的层叠上下文，弹窗留在里面时遮罩盖不住标题栏；
    ② 出去了才不会被 .shell 的 inert 一起禁掉。

    ⚠️ <b>刻意不换行、不重排缩进</b>：模板里有 white-space: pre 的块，
    整体缩进一动，Vue 模板编译器的 condense 会连带改掉渲染结果。

    ⚠️ <b>点遮罩不再关闭弹窗</b>：编辑器里都是填了一半的东西，点空白处就丢掉太容易误操作。
    出口只留「取消 / 关闭」按钮与 Esc。
  -->
  <Teleport to="body"><div v-if="props.id !== null" class="editor-mask" :inert="covered">
    <div class="dlg" role="dialog" aria-modal="true" @keydown.esc="emit('close')">
      <span class="mk tl" /><span class="mk tr" /><span class="mk bl" /><span class="mk br" />

      <header class="hd">
        <div class="tt">
          <span class="zh">{{ t('flt.e.title') }}</span>
          <span class="sub">Controls/FilterEdit</span>
        </div>
        <button class="x" :title="t('dlg.cancel')" @click="emit('close')">
          <svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg>
        </button>
      </header>

      <div v-if="!f" class="loading">{{ error || t('proxy.working') }}</div>

      <div v-else class="bd">
        <!-- 名称 / 模式 / 动作 -->
        <div class="row">
          <div class="k">{{ t('col.filterName') }}</div>
          <div class="v">
            <input v-model="f.Name" class="inp" spellcheck="false">
          </div>
        </div>

        <div class="row">
          <div class="k">{{ t('flt.e.mode') }}</div>
          <div class="v">
            <button class="rd" :class="{ on: f.Mode === 0 }" @click="f.Mode = 0"><i />{{ t('flt.e.normal') }}</button>
            <button class="rd" :class="{ on: f.Mode === 1 }" @click="f.Mode = 1"><i />{{ t('flt.e.advanced') }}</button>
            <span class="tip">{{ f.Mode === 1 ? t('flt.e.advancedTip') : t('flt.e.normalTip') }}</span>
          </div>
        </div>

        <div class="row">
          <div class="k">{{ t('col.action') }}</div>
          <div class="v wrap">
            <button v-for="a in ACTIONS" :key="a.v" class="rd" :class="{ on: f.Action === a.v }"
                    @click="f.Action = a.v"><i />{{ t(a.label) }}</button>
          </div>
        </div>

        <!-- 普通模式下仍保留但置灰，让高级模式的修改基准紧跟模式本身。 -->
        <div class="row" :class="{ off: f.Mode !== 1 }">
          <div class="k">{{ t('flt.e.startFrom') }}</div>
          <div class="v">
            <button class="rd" :class="{ on: f.StartFrom === 0 }" :disabled="f.Mode !== 1"
                    @click="f.StartFrom = 0"><i />{{ t('flt.e.fromHead') }}</button>
            <button class="rd" :class="{ on: f.StartFrom === 1 }" :disabled="f.Mode !== 1"
                    @click="f.StartFrom = 1"><i />{{ t('flt.e.fromPos') }}</button>
            <span v-if="f.Mode === 1" class="tip">
              {{ f.StartFrom === 1 ? t('flt.e.fromPosTip') : t('flt.e.fromHeadTip') }}
            </span>
          </div>
        </div>

        <!--
          执行：命中之后再跑另一张表里的一项。
          两个下拉在没勾选时压暗禁用，与 WinForms 的 FilterAction_ExecuteChange 一致。
        -->
        <div class="row" :class="{ off: !f.IsExecute }">
          <div class="k">{{ t('flt.e.execute') }}</div>
          <div class="v">
            <button class="chk" :class="{ on: f.IsExecute }" @click="f.IsExecute = !f.IsExecute">
              <i />{{ t('flt.e.executeOn') }}
            </button>

            <CyberSelect v-model="f.ExecuteType" class="dd" :options="execTypeOptions" :disabled="!f.IsExecute" />

            <CyberSelect v-model="f.ExecuteId" class="dd grow" :options="execTargetOptions"
                         v-if="f.ExecuteType !== 5" :disabled="!f.IsExecute || execTargets.length === 0" />
            <template v-if="f.ExecuteType === 5">
              <CyberSelect class="dd capture" :model-value="f.VariableExtractorId" :options="extractorOptions" placeholder="选择取值器" :disabled="!f.IsExecute" @update:model-value="selectCaptureExtractor" />
              <CyberSelect v-model="f.VariableId" class="dd capture capture-variable" :options="captureVariableOptions" :placeholder="captureVariableOptions.length ? '选择从封包取值变量' : '—'" :disabled="!f.IsExecute || !f.VariableExtractorId" />
            </template>

          </div>
        </div>

        <!-- ── 格子 ── -->
        <!--
          ── 表 1：查找 ──
          普通模式下这张表<b>还带着修改行</b>（两行必须按列对齐，所以同一个滚动条）；
          高级模式下它只有查找一行，修改是下面另一张独立的表。
        -->
        <div class="gtitle">
          <span>{{ t('flt.e.gridSearch') }}</span><span class="gcount">{{ filledText(searchFilledCount) }}</span>
          <template v-if="isNormal"><span class="gsep">/</span><span>{{ t('flt.e.gridModify') }}</span><span class="gcount">{{ filledText(modifyFilledCount) }}</span></template>
        </div>

        <div :ref="(e) => (sg.el = e as HTMLElement)" class="grid" @scroll.passive="sg.onScroll">
          <div class="gspace" :style="{ width: MAX * COL_W + 'px', height: (isNormal ? 96 : 64) + 'px' }">
            <div class="gwin" :style="{ transform: `translateX(${sg.start * COL_W}px)` }">
              <div class="grow2">
                <div v-for="i in sg.cols" :key="'h' + i" class="cell head">{{ colLabel(i, false) }}</div>
              </div>

              <div class="grow2 r-search">
                <div
                  v-for="i in sg.cols"
                  :key="'s' + i"
                  class="cell"
                  :class="{ 'm-ex': searchAt(i).exclude, sel: sel === 'search:' + i }"
                  @mousedown="pick('search', i)"
                  @contextmenu.prevent="openMenu('search', i, $event)"
                >
                  <input class="hex" :value="searchAt(i).v" maxlength="2"
                         spellcheck="false" @input="onCellInput(i, 'search', $event)"
                         @paste.prevent="onCellPaste('search', i, $event)" @blur="onCellBlur(i, 'search')">
                </div>
              </div>

              <!-- 普通模式：修改行就在这张表里，与查找行共用滚动条、逐列对齐 -->
              <div v-if="isNormal" class="grow2 r-modify">
                <div
                  v-for="i in sg.cols"
                  :key="replaceDomKey('m', i)"
                  class="cell"
                  :class="{ 'm-pg': modifyAt(i).progression, 'm-rd': modifyAt(i).random, 'm-vr': !!replaceAt(i), sel: sel === 'modify:' + i }"
                  :title="replaceMissing(i) ? '变量不存在' : (replaceTitle(i) || '')"
                  @mousedown="pick('modify', i)"
                  @contextmenu.prevent="openMenu('modify', i, $event)"
                >
                  <input class="hex" :value="modifyAt(i).v" maxlength="2"
                         spellcheck="false" @input="onCellInput(i, 'modify', $event)"
                         @paste.prevent="onCellPaste('modify', i, $event)" @blur="onCellBlur(i, 'modify')">
                </div>
              </div>
            </div>
          </div>
        </div>

        <!--
          ── 表 2：修改（仅高级模式）──
          单独一张表、单独一个滚动条，正是「查找与修改位置互不相干」这件事的界面体现。
          「指定位置」时列范围变成 -1000…999，列头带符号，0 就是匹配点本身。
        -->
        <template v-if="!isNormal">
          <div class="gtitle"><span>{{ t('flt.e.gridModify') }}</span><span class="gcount">{{ filledText(modifyFilledCount) }}</span></div>

          <div :ref="(e) => (mg.el = e as HTMLElement)" class="grid" @scroll.passive="mg.onScroll">
            <div class="gspace" :style="{ width: (MAX - mLo) * COL_W + 'px', height: '64px' }">
              <div class="gwin" :style="{ transform: `translateX(${(mg.start - mLo) * COL_W}px)` }">
                <div class="grow2">
                  <div v-for="i in mg.cols" :key="'mh' + i" class="cell head"
                       :class="{ zero: isOffset && i === 0 }">{{ colLabel(i, isOffset) }}</div>
                </div>

                <div class="grow2 r-modify">
                  <div
                    v-for="i in mg.cols"
                    :key="replaceDomKey('mm', i)"
                    class="cell"
                    :class="{ 'm-pg': modifyAt(i).progression, 'm-rd': modifyAt(i).random, 'm-vr': !!replaceAt(i), sel: sel === 'modify:' + i }"
                    :title="replaceMissing(i) ? '变量不存在' : (replaceTitle(i) || '')"
                    @mousedown="pick('modify', i)"
                    @contextmenu.prevent="openMenu('modify', i, $event)"
                  >
                    <input class="hex" :value="modifyAt(i).v" maxlength="2"
                           spellcheck="false" @input="onCellInput(i, 'modify', $event)"
                           @paste.prevent="onCellPaste('modify', i, $event)" @blur="onCellBlur(i, 'modify')">
                  </div>
                </div>
              </div>
            </div>
          </div>
        </template>

        <!--
          图例与「跳到第 N 列」同一行：一个说色块什么意思、一个管滚到哪儿，
          都是<b>看完表之后</b>才用得着的东西，各占一头正好把这一行填满。
          跳转靠右，离右侧的滚动条终点近。
        -->
        <div class="legend">
          <span class="lg m-ex">{{ t('flt.e.excludeOn') }}</span>
          <span class="lg m-pg">{{ t('flt.e.progressionOn') }}</span>
          <span class="lg m-rd">{{ t('flt.e.randomOn') }}</span>
          <span class="lg m-vr">{{ t('flt.e.replaceOn') }}</span>

          <span class="jump">
            {{ t('flt.e.jump') }}
            <input v-model="jumpTo" class="inp num" type="number" :min="isOffset ? -MAX : 1" :max="MAX"
                   @keydown.enter.prevent="jump">
            <button class="mini" @click="jump">{{ t('flt.e.go') }}</button>
          </span>
        </div>

        <!-- ── 封包类型 ── -->
        <div class="grp">{{ t('flt.e.funcs') }}</div>
        <div class="funcs">
          <button v-for="fn in FUNCS" :key="fn.bit" class="chk" :class="{ on: hasFunc(fn.bit) }"
                  @click="toggleFunc(fn.bit)"><i />{{ t(fn.label) }}</button>
        </div>

        <!--
          指定条件：四项排成两行两列。原来一项一整行、输入框铺满，
          四行下来把弹窗顶出了屏幕 —— 而这些值都很短（包头几个字节、端口五位数字），
          铺满的宽度纯属浪费。
        -->
        <!--
          指定类型与递进<b>并排</b>，各占两行，一共只吃三行高度。
          递进原先自成一段挂在下面，光两行小标题就把弹窗顶出了屏幕，
          而它那两样（连续 / 进位）跟套接字、端口一样只需要一个短输入框。
        -->
        <div class="apbar">
          <section class="apcol">
            <div class="grp">{{ t('col.appoint') }}</div>
            <div class="pairs">
              <div v-for="a in APPOINTS" :key="a.on" class="pair">
                <button class="chk" :class="{ on: f[a.on] }" @click="(f[a.on] as boolean) = !f[a.on]">
                  <i />{{ t(a.label) }}
                </button>
                <input v-model="(f[a.val] as string)" class="inp" spellcheck="false"
                       :disabled="!f[a.on]" :placeholder="t(a.ph)">
              </div>
            </div>
          </section>

          <section class="pgcol">
            <div class="grp">{{ t('col.progression') }}</div>
            <!--
              这里<b>不用 .pair 包一层</b>：两行的三样东西各自成列，
              「步长 / 进位值」长短不一，包起来的话两个数字框会错开十几像素。
              摊平进同一个 grid，max-content 那一列取两行的最大宽，就对齐了。
            -->
            <div class="pairs pg3">
              <button class="chk" :class="{ on: f.IsProgressionContinuous }"
                      @click="f.IsProgressionContinuous = !f.IsProgressionContinuous">
                <i />{{ t('flt.pg.continuous') }}
              </button>
              <span class="k2">{{ t('flt.e.step') }}</span>
              <input v-model.number="f.ProgressionStep" class="inp num" type="number" min="1">

              <button class="chk" :class="{ on: f.IsProgressionCarry }"
                      @click="f.IsProgressionCarry = !f.IsProgressionCarry">
                <i />{{ t('flt.pg.carry') }}
              </button>
              <span class="k2">{{ t('flt.e.carryNum') }}</span>
              <input v-model.number="f.ProgressionCarryNumber" class="inp num" type="number" min="1"
                     :disabled="!f.IsProgressionCarry">
            </div>
          </section>
        </div>
      </div>

      <footer class="ft">
        <span v-if="error" class="err">{{ error }}</span>
        <span class="grow" />
        <button class="btn" :disabled="busy" @click="emit('close')">{{ t('dlg.cancel') }}</button>
        <button class="btn primary" :disabled="busy || !f" @click="save">
          {{ busy ? t('proxy.working') : t('set.save') }}
        </button>
      </footer>
    </div>

    <ContextMenu :at="menuAt" :items="menuItems" @pick="onMenuPick" @close="menuAt = null" />
  </div>

  <!--
    取值器替换格的绑定编辑：选取值器 → 选变量 → 选写入格式。
    用统一的设置弹窗外壳（Teleport 到 body，自带模态栈登记），
    打开时本编辑器会被置为 inert。
  -->
  <SettingsModal :open="bindAt !== null" :title="t('flt.e.replaceTitle')" subtitle="VARIABLE REPLACE CELL"
                 :width="820" :hint="t('flt.e.bindHint')" :save-text="t('dlg.ok')"
                 @update:open="!$event && closeBind()" @save="saveBind">
    <div v-if="bindDraft" class="vr-picker">
      <input v-model="bindKeyword" class="inp vr-search" placeholder="搜索取值器或变量名称" autofocus />
      <div class="vr-table">
        <div class="vr-head"><span>取值器</span><span>变量名称</span><span>变量类型</span><span>数据类型</span><span>当前值</span><span>写入格式</span></div>
        <div v-if="!bindCandidates.length" class="vr-empty">没有匹配的取值器变量。</div>
        <div v-for="item in bindCandidates" :key="item.extractor.Id + item.variable.Id" class="vr-row"
                :class="{ selected: bindDraft.ExtractorId === item.extractor.Id && bindDraft.VariableId === item.variable.Id, off: !item.extractor.IsEnable }"
                :aria-disabled="!item.extractor.IsEnable" @click="item.extractor.IsEnable && selectBindVariable(item.extractor.Id, item.variable.Id)">
          <span :title="item.extractor.Name">{{ item.extractor.Name }}</span><span class="vr-var" :title="item.variable.Name">{{ item.variable.Name }}</span><span class="vr-kind">{{ bindKindName(item.variable.Kind) }}</span>
          <span class="vr-type">{{ bindTypeName(item.variable.DataType) }}</span><span class="vr-value" :title="bindCurrentValue(item.variable)">{{ bindCurrentValue(item.variable) }}</span>
          <span class="vr-format" @click.stop><CyberSelect v-if="bindDraft.ExtractorId === item.extractor.Id && bindDraft.VariableId === item.variable.Id" class="full" v-model="bindDraft.Format" :options="bindFormatOptions" :disabled="bindFormatOptions.length <= 1" /></span>
        </div>
      </div>
    </div>
  </SettingsModal></Teleport>
</template>

<style scoped>
.hd {
  flex: none;
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 14px 18px;
  border-bottom: 1px solid var(--border);
  background: var(--panel);
}

.hd .tt { flex: 1; display: flex; align-items: baseline; gap: 10px; }
.hd .zh { font-family: var(--orbit); font-weight: 700; font-size: var(--fs-title); letter-spacing: .04em; color: var(--gray); }

.hd .sub {
  font-family: var(--share);
  font-size: var(--fs-caption);
  letter-spacing: .18em;
  text-transform: uppercase;
  color: var(--dim);
}

.hd .x { display: inline-flex; padding: 0; background: transparent; border: 0; color: var(--muted); cursor: pointer; }
.hd .x:hover { color: var(--danger); }
.hd .x .ico { width: 16px; height: 16px; fill: none; stroke: currentColor; stroke-width: 1.8; }

.loading { padding: 60px; text-align: center; color: var(--muted); font-size: var(--fs-body); }

.bd { flex: 1; min-height: 0; overflow-y: auto; padding: 4px 0 12px; }

/*
  分组标题（作用域 / 指定类型 / 递进）。

  原来是 9px + var(--dim)，对 --card 只有 <b>2.47:1</b> —— 全弹窗最差的一处，
  比正文还小一号却比正文暗得多，等于把「这一组是什么」这句话藏起来。
  提到 10.5px / 6.09:1，并把字距从 .26em 收到 .14em：
  小字上过宽的字距会把字拆散，反而更难认。

  与下面 .gtitle（查找 / 修改）统一成同一套 —— 两者都是「一块内容的标签」，
  只有一个亮一个暗才是真的乱。
*/
.grp {
  font-family: var(--share);
  font-size: var(--fs-label);
  letter-spacing: .14em;
  text-transform: uppercase;
  color: var(--dim2);
  padding: 0 18px;
  margin: 12px 0 5px;
}

.jump { display: flex; align-items: center; gap: 6px; }

.row {
  display: grid;
  grid-template-columns: 100px 1fr;
  align-items: center;
  gap: 12px;
  padding: 3px 18px;
  min-height: 30px;
}

.row > .k { font-size: var(--fs-body); color: var(--muted); }
.row > .v { display: flex; align-items: center; gap: 14px; min-width: 0; }
.row > .v.wrap { flex-wrap: wrap; gap: 8px 16px; }

.k2 { font-size: var(--fs-body); color: var(--muted); }
.tip { font-size: var(--fs-small); color: var(--dim2); }

/*
  下拉是自绘的 CyberSelect（全项目统一；它自带滚动、键盘首字母跳转与超出视口时的翻转，
  早先「自绘不划算」的顾虑已经在组件里补齐）。这里只给宽度。
*/
/* 下拉宽度类叫 .dd 不叫 .sel —— 字节格的选中态也是 .sel，裸 .sel 会把选中的格子一起改宽 */
.dd { width: 150px; max-width: 260px; }
.dd.grow { flex: 1; min-width: 0; max-width: none; }
.dd.capture-variable { flex: 1 1 240px; min-width: 220px; max-width: 420px; }

/*
  作用域只剩四项，一行放得下。
  列宽用 max-content 而不是 1fr：等分会把四个短标签摊到整个弹窗宽上，
  中间空出大片，看着不像一组。
*/
.funcs {
  display: grid;
  grid-template-columns: repeat(4, max-content);
  gap: 7px 28px;
  padding: 0 18px 4px;
}

/*
  指定类型（左，两列）与递进（右，一列）并排，各两行。
  两块的 .grp 小标题与 .pairs 行高一致，所以横着自然对齐 ——
  不必用一个大 grid 把两组语义不同的东西硬拼在一起。
*/
.apbar { display: flex; align-items: flex-start; gap: 24px; padding: 0 18px; }
.apcol { flex: 1; min-width: 0; }
.pgcol { flex: none; }

.apbar .grp { padding: 0; }

/* 指定条件：输入框不再铺满 —— 这些值都很短 */
.pairs {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 7px 24px;
  padding-bottom: 4px;
}

/*
  递进：勾选 / 标签 / 数字框三列，两行共用同一套列宽。

  前两列都用 max-content 而<b>不写死宽度</b>：列宽取两行的最大值，
  「步长 / 进位值」长短不一也不会把数字框推得一上一下。
  写死的话英文的 Continuous 会顶出去 —— 它比中文的「连续」宽一倍多，
  而这一列是 .pgcol（flex: none），让它自己撑开不占别人的地方。
*/
.pairs.pg3 {
  grid-template-columns: max-content max-content 92px;
  gap: 7px 10px;
  align-items: center;
}

.pair {
  display: grid;
  grid-template-columns: 88px 1fr;
  align-items: center;
  gap: 10px;
}

/* 基样式在 style.css 的「勾选框 / 单选框」，这里只覆盖颜色与布局 */
.chk.k { justify-self: start; }

/*
  勾选态用<b>压暗的绿</b>（var(--chk-on)），不是主色 --green。

  这一屏的主角是下面那张字节格 —— 它顶着 WinForms 传下来的浅黄 / 黄底色，
  本来就很亮。上面再排一行满饱和的 --green（对底色 15:1），眼睛会先被
  这些开关抓走，而它们只是设定、不是正在编辑的东西。

  压到 6.5:1：仍然比未勾选的 --muted（3.9:1）亮一档、且有色相差，
  一眼看得出勾没勾，但不再跟格子抢。底色那层浅浅的绿也一并调薄。

  作用域 / 指定类型 / 递进三组共用这条规则 —— 它们是同一类东西，
  只压一组会变成同一个弹窗里两种深浅的绿。
*/
.chk { --chk-fill: var(--chk-on); --chk-fill-rgb: var(--chk-on-rgb); --chk-tint: 14%; }

/*
  普通模式下「修改起始于」压暗但仍在（WinForms 是 Enabled = false）。
  整行一起压暗，标签才不会亮着而选项是灰的 —— 那看着像坏了。
*/
.row.off > .k { opacity: .45; }

/* 基样式在 style.css 的 .inp，这里只补布局 */
.inp { flex: 1; min-width: 0; }

.inp.num { flex: none; width: 92px; font-variant-numeric: tabular-nums; }

/* 小按钮的样式在 style.css 的 .mini */

/* ── 格子 ── */

.grid {
  margin: 0 18px;
  border: 1px solid var(--border);
  background: var(--sink);
  overflow-x: auto;
  overflow-y: hidden;
  /* 滚动时不做像素级平滑，帧内工作量更可控 */
  overflow-anchor: none;
}

/* 高度由行数决定：普通表三行（列号 + 查找 + 修改），高级的两张表各两行 */
.gspace { position: relative; }
.gwin { position: absolute; top: 0; left: 0; will-change: transform; }
.grow2 { display: flex; height: 32px; }

.gtitle {
  display: flex;
  align-items: center;
  gap: 10px;
  /* 上外边距兼作「表格块与上面几行设置」的间隔，两张表之间也靠它 */
  margin: 10px 18px 4px;
  font-family: var(--share);
  font-size: var(--fs-label);
  letter-spacing: .14em;
  /* 与 .grp 同一套标签样式，理由见那里 */
  color: var(--dim2);
}

.gtitle + .grid { margin-bottom: 6px; }
.gsep { color: var(--muted); }
.gcount { color: var(--cyan); }

/* 偏移表的匹配点：这一列就是「命中的那一位」，标出来才找得回原点 */
.cell.head.zero { color: var(--cyan); background: rgb(var(--cyan-rgb) / 12%); }

.cell {
  width: 46px;
  flex: none;
  border-right: 1px solid rgb(var(--border-rgb) / 60%);
  border-bottom: 1px solid rgb(var(--border-rgb) / 60%);
  display: flex;
  align-items: center;
  justify-content: center;
}

.cell.head {
  height: 32px;
  background: var(--panel);
  font-family: var(--share);
  font-size: var(--fs-label);
  /*
    列号必须读得清 —— 它是这张表唯一的坐标，找第几个字节全靠它。
    原来的 --muted 压在 --panel 上只有 3.46:1，低于正文 4.5 的底线，
    而这里还是 10px 的小字，实际更吃亏。换成 7.8:1。
    没有直接用 --gray（12.7:1）：那样列号会跟格子里的数据一样抢眼，
    表头是刻度、不是内容。
  */
  color: var(--th-fg);
}

/*
  ── 格子配色照搬 WinForms ──────────────────────────────────

  这套颜色从老版 WPE 传下来，用的人早就认了，属于约定而不是装饰，
  所以<b>逐个取原值</b>，不换成本皮肤的同义色：

    查找行  LightYellow  #FFFFE0      修改行  Yellow      #FFFF00
    文字    RoyalBlue    #4169E1
    排除    Violet       #EE82EE      （FilterExclude_Color）
    递进    DarkRed      #8B0000      （FilterProgression_Color）
    随机    DodgerBlue   #1E90FF      （FilterRandom_Color）

  WinForms 侧标记时只改 Back、Fore 一直是 RoyalBlue。这里<b>只有一处没照搬</b>：
  标记格的文字色。量过对比度（RoyalBlue 对三种标记底色分别是 2.09 / 2.07 / 1.50），
  压在 DarkRed 和 DodgerBlue 上基本读不出来。
  底色是约定、必须一致；文字色不是约定，所以按底色深浅各挑一个能读的
  （DarkRed 配白 10.01，另两个配近黑 8.52 / 6.10）。
*/
/*
  按<b>行的用途</b>着色，不按 nth-child —— 高级模式下修改行是另一张表里的第 2 行，
  按位置写死的话它会拿到查找行的浅黄。
*/
/*
  底色刷在<b>行</b>上，不是行里的每个格子。

  写成 `.grow2.r-search .cell` 的话它是 (0,3,0)，而标记色 `.cell.m-ex` 只有
  (0,2,0) —— 行底色永远压着标记色，排除 / 递进 / 随机三种底色<b>一次都显示不出来</b>，
  而且不报任何错。刷在行上之后格子本身没有 background，
  标记色直接画在行底色之上，两者不再竞争同一个属性。
*/
.grow2.r-search { background: #ffffe0; }
.grow2.r-modify { background: #ffff00; }

.hex {
  width: 100%;
  height: 100%;
  padding: 0;
  background: transparent;
  border: 0;
  color: #4169e1;
  font-family: var(--mono);
  font-size: var(--fs-body);
  font-weight: 600;
  text-align: center;
  text-transform: uppercase;
  outline: none;
  user-select: text;
}

.hex::placeholder { color: transparent; }

.cell.m-ex { background: #ee82ee; }
.cell.m-pg { background: #8b0000; }
.cell.m-rd { background: #1e90ff; }
/* 取值器替换格：用青绿，避开 WinForms 传下来的紫 / 暗红 / 蓝三种标记色 */
.cell.m-vr { background: #0f766e; }

/* 文字色跟着底色走，理由见上面那段 */
.cell.m-ex .hex,
.cell.m-rd .hex { color: #0a0a0f; }
.cell.m-pg .hex { color: #fff; }
.cell.m-vr .hex { color: #fff; }


/* 选中框用绿色：三种标记底色里没有绿，压在哪一种上都分得出来 */
.cell.sel { outline: 2px solid var(--green); outline-offset: -2px; }

/* 三个色块在左、跳转在右：margin-left:auto 把中间的空隙全给它 */
.legend { display: flex; align-items: center; gap: 16px; padding: 4px 18px 0; font-size: var(--fs-small); }
/* 跳转的文字跟三个色块的说明同色 —— 它们同属这一行的注解层 */
.legend .jump { margin-left: auto; color: var(--muted); }
.legend .lg { display: inline-flex; align-items: center; gap: 6px; color: var(--muted); }

.legend .lg::before {
  content: "";
  width: 12px;
  height: 12px;
  border: 1px solid currentColor;
}

/* 图例的色块直接填标记色本身，与格子一眼对得上 */
/*
  三个色块的修饰类带 m- 前缀（mark），不能叫 ex / pg / rd：
  rd 同时是本文件里单选按钮的类名（.chk, .rd），
  <span class="lg rd"> 会一并吃到它的 font-size:12.5px 与 cursor:pointer ——
  于是「设为随机」比另外两个大一号、鼠标移上去还变成手型，
  而 .legend .lg 没有设这两个属性，压不住。
*/
.legend .lg.m-ex::before { background: #ee82ee; border-color: #ee82ee; }
.legend .lg.m-pg::before { background: #8b0000; border-color: #8b0000; }
.legend .lg.m-rd::before { background: #1e90ff; border-color: #1e90ff; }
.legend .lg.m-vr::before { background: #0f766e; border-color: #0f766e; }

/* 取值器替换格：复用「插入变量」的完整信息表，而非只显示两个名字的下拉框。 */
.vr-picker { padding: 2px 2px 0; }
.vr-search { box-sizing: border-box; width: 100%; margin-bottom: 10px; }
.vr-table { max-height: 380px; overflow: auto; border: 1px solid var(--border); }
.vr-head, .vr-row { display: grid; grid-template-columns: minmax(76px, .55fr) minmax(94px, .75fr) 82px 68px minmax(98px, .85fr) minmax(112px, .8fr); align-items: center; gap: 10px; padding: 0 12px; }
.vr-head { height: 35px; background: var(--bar); color: var(--dim); font: var(--fs-small) var(--mono); }
.vr-row { box-sizing: border-box; width: 100%; min-height: 42px; border: 0; border-top: 1px solid var(--border); background: transparent; color: var(--gray); text-align: left; font: inherit; cursor: pointer; }
.vr-row:hover:not(.off) { background: rgb(var(--cyan-rgb) / 7%); }
.vr-row.selected { background: rgb(var(--cyan-rgb) / 12%); box-shadow: inset 2px 0 var(--cyan); }
.vr-row.off { opacity: .42; cursor: not-allowed; }
.vr-row > span { min-width: 0; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
.vr-var { color: var(--cyan); }.vr-kind, .vr-type { color: var(--dim); font-size: var(--fs-small); }.vr-value { color: var(--text); font: var(--fs-small) var(--mono); }
.vr-format { overflow: visible !important; }.vr-format .full { display: flex; width: 100%; }
.vr-empty { padding: 18px 12px; color: var(--dim); font-size: var(--fs-small); }

.ft {
  flex: none;
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 12px 18px;
  border-top: 1px solid var(--border);
  background: var(--panel);
}

.ft .grow { flex: 1; }
.ft .err { font-size: var(--fs-small); color: var(--danger); }

.btn {
  min-width: 84px;
  padding: 10px 16px 10px;   /* 上 +1 下 -1：字形在 em 框里偏上 1px（上伸 9 / 下伸 3，实测），补回来 */
  background: transparent;
  border: 1px solid var(--border);
  color: var(--gray);
  font-family: var(--share);
  font-size: var(--btn-size);
  /* 显式 1：Share Tech Mono 在 line-height: normal 下会把行距全压在字的下面，字号一大就明显偏上（实测） */
  line-height: 1;
  letter-spacing: .12em;
  text-transform: uppercase;
  cursor: pointer;
}

.btn:hover:not(:disabled) { border-color: var(--cyan); color: var(--cyan); }
.btn:disabled { opacity: .35; cursor: default; }
.btn.primary { border-color: rgb(var(--green-rgb) / 45%); color: var(--green); }
.btn.primary:hover:not(:disabled) { background: rgb(var(--green-rgb) / 10%); border-color: var(--green); }
</style>
