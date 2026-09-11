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

/*
  ⚠️ 这里原来还有一个 `module`（目标的主窗口标题）。2026-09-10 删掉了：
  状态条上那块「窗口」读数窗早就撤了（注入成功时系统日志里记了完整的一条），
  从此没有任何地方读它 —— 而 C# 那边为了填它，每秒要 EnumWindows 扫一遍整个桌面
  的顶层窗口，还是在 UI 线程上。「去掉调用方就顺手清被调方」。
*/
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
  /*
    ⚠️ **必须拷一份，不能直接把入参挂上去。**

    Vue 的 ref 赋值是按<b>引用</b>判等的：调用方要是把同一个对象改几个字段再传回来，
    `status.value = s` 什么都不会触发 —— 而模板里 `status.hooked` 这种<b>直接读属性</b>的
    绑定会在下一次因为别的原因重渲染时读到新值，`computed` 却因为没被通知而<b>一直返回旧值</b>。

    表现是「同一条状态条上，按钮已经变成『停止拦截』、灯也绿了，状态字还写着『已附加』」——
    2026-09-10 在探针页当场撞到（假宿主的 injectStartHook 返回的正是同一个 window.__inj）。

    真程序走桥、每次都是新解析出来的 JSON 对象，所以碰不到；但这份 store 不该<b>依赖</b>
    调用方的这个习惯 —— 拷一份是常数开销（十来个标量），换掉的是一整类只在某些调用方身上
    发作的静默 bug。
  */
  status.value = { ...s }
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
