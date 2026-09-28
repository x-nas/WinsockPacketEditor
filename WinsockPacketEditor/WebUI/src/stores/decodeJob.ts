/*
  批量解码 / 多选智能解码的进度状态。

  C# 侧同一时刻只有一个解码任务在跑（发起新的会先取消旧的），进度经 decode:progress
  事件推过来。job 号是前端发起时生成、随请求回传的，C# 原样带回 ——
  用它丢弃「上一个任务迟到的进度」，免得把新任务的进度条写花。
*/
import { ref } from 'vue'
import { call, on } from '../bridge'

export interface DecodeJob {
  /** 前端生成的任务号（回传给 C# 的 job 参数）。 */
  id: number
  done: number
  total: number
}

export const decodeJob = ref<DecodeJob | null>(null)

let seq = 0

/** 开一个进度遮罩，返回它自己的任务号（要作为 job 回传给 C#）。 */
export function beginDecodeJob(total: number): number {
  const id = ++seq
  decodeJob.value = { id, done: 0, total }
  return id
}

/** 只清掉自己那一份：期间可能已经开了新任务，不能把它的进度条一起清掉。 */
export function endDecodeJob(id: number): void {
  if (decodeJob.value?.id === id) decodeJob.value = null
}

/** 取消当前解码任务。 */
export function cancelDecodeJob(): void {
  void call('cancelDecodeJob').catch(() => {})
}

/** 在 App.vue 挂一次。 */
export function attachDecodeJob(): void {
  on('decode:progress', (d: { job: number; done: number; total: number }) => {
    const j = decodeJob.value
    if (j && j.id === d.job) decodeJob.value = { id: j.id, done: d.done, total: d.total }
  })
}
