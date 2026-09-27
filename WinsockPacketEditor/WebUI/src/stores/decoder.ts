/*
  解码器页的界面状态。

  与 stores/tools.ts 同一个理由：工具页在 ProxyView / InjectView 里是 v-if 挂载的，
  切页组件就销毁了，用户贴进去的一段数据、选中的解码器不能跟着没。
  跨模式（注入 / 代理）共用同一份。
*/
import { ref } from 'vue'
import type { DecoderRow } from '../bridge/types'

/** 解码器列表（来自 getDecoders，不进 Feed 推送流）。 */
export const decRows = ref<DecoderRow[]>([])

/** 当前选中的解码器 Id。 */
export const decSelectedId = ref('')

/** 当前工作台来源：已保存解码器，或左侧“临时编解码”入口。默认落在快速编解码。 */
export const decMode = ref<'decoder' | 'quick'>('quick')

/** 测试台：是否套用帧配置。 */
export const decApplyFrame = ref(true)

/** 原文 / 输入：编码的输入，也是解码结果的落点。 */
export const decInput = ref('')

/** 结果：编码的输出，也是解码的输入（十六进制文本），可编辑。 */
export const decOutput = ref('')

export const decError = ref('')

/** 当前选中解码器对象。 */
export function decSelected(): DecoderRow | null {
  return decRows.value.find((x) => x.Id === decSelectedId.value) ?? null
}
