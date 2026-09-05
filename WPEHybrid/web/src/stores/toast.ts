// 轻提示 / 通知的队列。
//
// 对应 C# 侧 IUiHost 的两条单向推送：
//   toast   { level, text }            —— 一句话，几秒后自己消失
//   notify  { level, title, content }  —— 带标题，停留久一些
//
// 【为什么不用 ant-design-vue 的 message / notification】
// 它们自带一整套浅色圆角胶囊，在这套深色赛博皮肤里像从别的程序飞过来的；
// 而且 message 固定顶部居中，正好压在自绘标题栏的拖动区上。
// 覆盖它的样式要跟 :deep + 内部类名较劲，还得盯着版本升级，
// 自己画一个反而更短、更好控。

import { ref } from 'vue'

/** 与 ClassObject/Ui/IUiHost.cs 的 UiIcon 对应。 */
export type ToastLevel = 'success' | 'info' | 'warning' | 'error'

export interface ToastItem {
  id: number
  level: ToastLevel
  /** 有标题的是 notify，没有的是 toast —— 决定排版与停留时长 */
  title?: string
  text: string
}

/**
 * 同屏最多几条。
 *
 * 超出就把最老的挤掉：停止代理会连着来两条（SOCKS5 + HTTP），
 * 而失败重试之类的场景可能一次来好几条，堆满整屏比丢掉几条更糟。
 */
const MAX = 5

export const toasts = ref<ToastItem[]>([])

let seq = 0

export function pushToast(level: ToastLevel, text: string, title?: string): void {
  if (!text && !title) return

  const id = ++seq
  const next = [...toasts.value, { id, level, title, text }]

  toasts.value = next.length > MAX ? next.slice(next.length - MAX) : next

  //带标题的信息量大，多留一会儿；纯文本的一眼扫完就够
  window.setTimeout(() => dismissToast(id), title ? 6000 : 4000)
}

export function dismissToast(id: number): void {
  toasts.value = toasts.value.filter((x) => x.id !== id)
}
