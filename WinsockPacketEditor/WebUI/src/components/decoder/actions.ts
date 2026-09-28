import { call } from '../../bridge'
import type { DecoderRow } from '../../bridge/types'
import type { MenuItem } from '../menu'
import { decRows } from '../../stores/decoder'

/** 一次解码的结果，交给 DecodeResult.vue 展示。 */
export interface DecodePayload {
  /** 用的解码器 Id / 名称。 */
  decoderId: string
  decoderName: string
  /** 明文文本与十六进制两种看法。 */
  text: string
  hex: string
  /** 解码后字节的 base64（「添加到发送器」当明文包用）。 */
  bufferB64: string
  error: string
  /** 来源封包（有的话才能「添加到发送器」）。 */
  list: '' | 'proxy' | 'packet'
  id: number
}

/** 拉一次解码器列表（进页面 / 开右键菜单时）。列表规模小，重复拉不贵。 */
export async function ensureDecoders(force = false): Promise<DecoderRow[]> {
  if (force || !decRows.value.length) {
    try {
      const r = await call<{ rows: DecoderRow[] }>('getDecoders')
      decRows.value = r?.rows ?? []
    } catch (e) {
      console.error('[dec] 读取解码器失败', e)
    }
  }
  return decRows.value
}

/**
 * 右键菜单里的「解码 ▸ 解码器」一项，sub 是已保存的解码器。
 *
 * ⚠️ 单条 / 批量<b>不再分成两个菜单项</b>：选中几条就按几条走 ——
 * 点选中的解码器时，前端按选中封包数决定走单条解码还是批量解码（见各列表的 onMenuPick）。
 * label 后面照其它菜单项带上选中的条数。
 */
export function decoderMenuItem(label: string, count = 0): MenuItem {
  const enabled = decRows.value.filter((d) => d.IsEnable)
  const tag = count ? ' (' + count + ')' : ''
  if (!enabled.length) {
    return { id: 'decodeWith', label: label + tag, icon: DEC_ICON, disabled: true }
  }
  return {
    id: 'decodeWith',
    label: label + tag,
    icon: DEC_ICON,
    sub: enabled.map((d) => ({ id: 'dec:' + d.Id, label: d.Name })),
  }
}

/** 智能解码：遍历启用的解码器，挑可读明文。label 后面带上选中的条数。 */
export function smartDecodeMenuItem(label: string, count = 0): MenuItem {
  const tag = count ? ' (' + count + ')' : ''
  return { id: 'smartDecode', label: label + tag, icon: DEC_ICON }
}

// 与侧栏「解码器列表」共用钥匙，避免同一项功能在不同入口看起来像两套图标。
const DEC_ICON = '<circle cx="7.5" cy="15.5" r="3.5"/><path d="M10 13L20 3"/><path d="M16.5 6.5l2 2"/><path d="M14 9l2 2"/>'

/** 智能解码的一条命中。 */
export interface SmartHit {
  Id: string
  Name: string
  Text: string
  OutputBase64: string
  Error: string
  Ok: boolean
  Offset: number
  Truncated?: boolean
}
export interface SmartPayload {
  hits: SmartHit[]
  list?: 'proxy' | 'packet'
  /**
   * 多选封包智能解码时按封包分组；有它时 `hits` 为空、以它为显示源。
   * 单选 / 十六进制面板那条路仍用 `hits`。
   */
  items?: SmartBatchItem[]
}

/** 多条封包智能解码里的一条：这条封包自己的命中。 */
export interface SmartBatchItem {
  /** 封包 Id（取字节用；也是分组的 key）。 */
  Id: number
  /** 封包时间与预览（从行数据带过来，仅在结果里做标题用）。 */
  Time: string
  Preview: string
  hits: SmartHit[]
}

/** 批量解码的一行。 */
export interface BatchRow {
  Id: number
  Ok: boolean
  Text: string
  Hex: string
  Error: string
  /** 批量通道只返回预览；完整内容由用户按需加载。 */
  Truncated?: boolean
}
export interface BatchPayload {
  decoder: string
  decoderId: string
  list: 'proxy' | 'packet'
  rows: BatchRow[]
}

export function bytesToHex(a: Uint8Array): string {
  return Array.from(a, (b) => b.toString(16).padStart(2, '0').toUpperCase()).join(' ')
}
export function hexToBytes(hex: string): Uint8Array {
  const s = hex.replace(/[\s-]/g, '')
  const out = new Uint8Array(s.length / 2)
  for (let i = 0; i < out.length; i++) out[i] = parseInt(s.slice(i * 2, i * 2 + 2), 16)
  return out
}
export function bytesToB64(a: Uint8Array): string {
  let s = ''
  for (let i = 0; i < a.length; i++) s += String.fromCharCode(a[i])
  return btoa(s)
}
export function b64ToBytes(b64: string): Uint8Array {
  const bin = atob(b64)
  const out = new Uint8Array(bin.length)
  for (let i = 0; i < bin.length; i++) out[i] = bin.charCodeAt(i)
  return out
}
