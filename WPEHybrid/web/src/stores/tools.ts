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
export const tcRegex = ref('')
export const tcMinBytes = ref(2)

/* ── 异或计算 ── */
export const xorSrc = shallowRef<Uint8Array>(new Uint8Array(0))
export const xorOut = shallowRef<Uint8Array>(new Uint8Array(0))
export const xorKey = ref('')

/* ── 编码转换 ── */
export const trInput = ref('')
export const trRows = ref<TranscodeRow[]>([])
export const trMode = ref<'' | 'enc' | 'dec'>('')

/* ── 数据提取 ── */
export const exKind = ref(0)
export const exText = ref('')
export const exPath = ref('')
