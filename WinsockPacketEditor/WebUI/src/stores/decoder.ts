/*
  解码器的界面状态（跨注入 / 代理共用）。

  解码器的<b>管理</b>在独立的 DecoderList.vue 页（侧栏与滤镜 / 发送 / 机器人 / 仓库并列），
  这里只放「智能解码」工作台要用的状态。与 stores/tools.ts 同一个理由：
  工具页在 ProxyView / InjectView 里是 v-if 挂载的，切页组件就销毁了，
  用户贴进去的一段数据、选中的解码器不能跟着没。
*/
import { ref } from 'vue'
import type { DecoderRow } from '../bridge/types'

/** 解码器列表（来自 getDecoders，不进 Feed 推送流）。列表页与工作台读同一份。 */
export const decRows = ref<DecoderRow[]>([])

/** 工作台当前选中的解码器 Id。 */
export const decSelectedId = ref('')

/** 工作台的来源：已保存解码器，或「快速编解码」临时入口。默认落在快速编解码。 */
export const decMode = ref<'decoder' | 'quick'>('quick')

/** 测试台：是否套用帧配置。 */
export const decApplyFrame = ref(true)

/** 工作台的方向；两种操作共用同一页，切换时由页面交换两侧内容。 */
export const decTransformDirection = ref<'decode' | 'encode'>('decode')

/** 左侧工作区：编码时是明文，解码时是十六进制密文。 */
export const decInput = ref('')

/** 右侧工作区：编码时是十六进制结果，解码时是明文结果。 */
export const decOutput = ref('')

export const decError = ref('')

/** 当前选中解码器对象。 */
export function decSelected(): DecoderRow | null {
  return decRows.value.find((x) => x.Id === decSelectedId.value) ?? null
}
