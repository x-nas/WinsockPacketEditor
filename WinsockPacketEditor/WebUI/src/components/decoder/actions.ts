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

/** 右键菜单里的「解码 ▸ 解码器」一项，sub 是已保存的解码器。 */
export function decoderMenuItem(label: string): MenuItem {
  const enabled = decRows.value.filter((d) => d.IsEnable)
  if (!enabled.length) {
    return { id: 'decodeWith', label, icon: DEC_ICON, disabled: true }
  }
  return {
    id: 'decodeWith',
    label,
    icon: DEC_ICON,
    sub: enabled.map((d) => ({ id: 'dec:' + d.Id, label: d.Name })),
  }
}

/** 智能解码：遍历启用的解码器，挑可读明文。 */
export function smartDecodeMenuItem(label: string): MenuItem {
  return { id: 'smartDecode', label, icon: DEC_ICON }
}

/** 批量解码 ▸ 解码器：对选中的封包逐条解码。 */
export function batchDecodeMenuItem(label: string): MenuItem {
  const enabled = decRows.value.filter((d) => d.IsEnable)
  if (!enabled.length) {
    return { id: 'batchDecode', label, icon: DEC_ICON, disabled: true }
  }
  return {
    id: 'batchDecode',
    label,
    icon: DEC_ICON,
    sub: enabled.map((d) => ({ id: 'batch:' + d.Id, label: d.Name })),
  }
}

const DEC_ICON = '<ellipse cx="8" cy="12" rx="3.5" ry="5.5"/><path d="M14 8l6 8M20 8l-6 8"/>'

/** 智能解码的一条命中。 */
export interface SmartHit {
  Id: string
  Name: string
  Text: string
  OutputBase64: string
  Error: string
  Ok: boolean
  Offset: number
}
export interface SmartPayload {
  hits: SmartHit[]
}

/** 批量解码的一行。 */
export interface BatchRow {
  Id: number
  Ok: boolean
  Text: string
  Hex: string
  Error: string
}
export interface BatchPayload {
  decoder: string
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
