// 注入模式的运行态 —— 目标是谁、钩子装上没有、丢了多少包。
//
// 【为什么要一份 store】这一屏被拆成了三层：InjectView（外壳 + 选目标 + 轮询）、
// InjectBar（状态条）、InjectData（封包页），另外底部状态栏在 App.vue 里、
// 根本不是 InjectView 的子孙。逐层 props 传下去要穿三层、还要把 emit 一层层抬上来，
// 而这就是一份「谁都要读、只有一处写」的运行态。

import { ref } from 'vue'
import { call } from '../bridge'
import { injectHooked, injectTarget } from './runtime'

export interface InjectStatus {
  /** idle = 还没附加（此时显示选目标屏）；disconnected = 目标没了，数据仍在 */
  state: 'idle' | 'attaching' | 'attached' | 'disconnected'
  pid: number
  name: string
  is64: boolean
  hooked: boolean
  dropped: number
  /** 目标自己探出来的 WinSock 版本 —— 只有它看得见自己的模块表 */
  ws1: boolean
  ws2: boolean
  msws: boolean
}

export const status = ref<InjectStatus>({
  state: 'idle', pid: 0, name: '', is64: false,
  hooked: false, dropped: 0, ws1: false, ws2: false, msws: false,
})

/**
 * 写状态的<b>唯一入口</b>。
 *
 * 底部状态栏那两项跟着一起更新 —— 它在 App.vue 里，跨视图的运行态走 stores/runtime。
 * 分成两处写的话，总会有一条路径忘了同步（状态栏就会停在上一个目标上）。
 */
export function setStatus(s: InjectStatus): void {
  status.value = s
  injectHooked.value = !!s.hooked
  injectTarget.value = s.pid > 0 ? s.name + ' #' + s.pid : ''
}

/** 目标用的是哪几套 WinSock。显示成 "1.1 / 2.0 / MS"，一个都没有时是 "—"。 */
export function wsText(s: InjectStatus): string {
  const on: string[] = []
  if (s.ws1) on.push('1.1')
  if (s.ws2) on.push('2.0')
  if (s.msws) on.push('MS')
  return on.length ? on.join(' / ') : '—'
}

/** 从 C# 重取一次。桥没接上（探针页）时静默保持原值。 */
export async function refresh(): Promise<void> {
  try { setStatus(await call<InjectStatus>('getInjectStatus')) } catch { /* 断了 */ }
}
