/*
  四个工具页（文本对比 / 异或计算 / 编码转换 / 数据提取）的状态。

  这几页在 ProxyView 里是 v-if 挂载的 —— 切到别的页组件就销毁了。工具页的内容是用户一点点填进去的
  （贴了一段十六进制、算到一半的结果），切个页回来就没了会很烦，所以状态全放在这里，组件只是它的视图。
  「添加到文本 A / B」也写这里：封包列表和文本对比页不在同一棵子树上。
*/
import { ref, shallowRef } from 'vue'

/** 高亮叠层的一段：[start, end) 的字符区间 + 样式类（del / ins / rx / dup） */
export interface Mark { start: number; end: number; cls: string }

export interface TranscodeRow { Key: string; Value: string }

/** C# SystemConfig.DuplicateInfo 的原样 JSON */
export interface DupRow {
  Sequence: string
  Length: number
  CountInA: number
  CountInB: number
  PositionsInA: number[]
  PositionsInB: number[]
}

/* ── 文本对比 ── */
export const textA = ref('')
export const textB = ref('')
export const tcMode = ref<'diff' | 'dup'>('diff')
/*
  对齐粒度：hex = 按字节、text = 按行。

  默认十六进制 —— 这一页的主力输入是封包列表右键「添加到文本 A/B」灌进来的十六进制
  （copyProxyHex 的结果），按字符比毫无意义。⚠️ 内容不像十六进制时<b>不自动切</b>，
  只在工具条上挂一句提示：自动切了用户不知道发生过什么。
*/
export const tcView = ref<'hex' | 'text'>('hex')
export const tcRegex = ref('')
/*
  查重的最小片段长度。⚠️ 默认 <b>4 不是 2</b>。

  实测两段各 2048 字节的<b>随机</b>数据，min=2 时报出 65 条「共同片段」—— 全是巧合
  （两个随机字节撞上的概率是 1/65536，2048² 个位置对里期望就有 64 次）。
  min=4 时同一组数据是 0 条。2 这个默认值让这张表在真实数据上基本全是噪声。

  ⚠️ WinForms 那边的默认仍是 2（两条线并行），两套 UI 这一项会不一样。
*/
export const tcMinBytes = ref(4)

/* ── 异或计算 ── */

/**
 * 算法：`⊕ 密钥` 把密钥循环铺开、`⊕ 数据 B` 把两段逐字节配对。
 *
 * 后者是抓包里真正常用的那个 —— 两条只差一点的封包异或一下，非零的字节就是变了的那几个；
 * 拿「明文 + 密文」异或则直接得到密钥。WinForms 那边只有前者。
 */
export const xorMode = ref<'key' | 'b'>('key')

export const xorSrc = shallowRef<Uint8Array>(new Uint8Array(0))
export const xorB = shallowRef<Uint8Array>(new Uint8Array(0))
export const xorKey = ref('')

/** 密钥怎么读：十六进制，还是逐字符取 Latin-1 字节（与十六进制视图的字符栏同一条口径）。 */
export const xorKeyFmt = ref<'hex' | 'text'>('hex')

/*
  ⚠️ <b>结果刻意不存这里，它是算出来的。</b>

  老版本把 xorOut 也放在 store 里、由「计算」按钮写一次 —— 于是改完源数据或密钥而没再点按钮时，
  右边显示的是<b>上一次</b>的结果，界面上一点提示都没有。派生量就该是派生量，
  组件里一个 computed 就把这一类 bug 从根上去掉了。
*/

/* ── 编码转换 ── */
export const trInput = ref('')
export const trRows = ref<TranscodeRow[]>([])

/** 编码还是解码。⚠️ 没有「空」这一档了 —— 现在是实时跑的，进页面就有结果。 */
export const trMode = ref<'enc' | 'dec'>('enc')

/* ── 数据提取 ── */
export const exKind = ref(0)

/** 这一次提出来几条（Charles 是会话数、FILT 是滤镜条数、账号是账号数）。C# 侧的 ExtractResult.Count */
export const exCount = ref(0)
export const exText = ref('')
export const exPath = ref('')
