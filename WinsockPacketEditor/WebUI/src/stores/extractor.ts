/*
  取值器的共享配置副本。

  与滤镜 / 发送等配置列表一样，页面和侧栏读同一份响应式数据：
  管理页的写操作完成后才重新拉取，不为侧栏另开轮询或重复桥接。
*/
import { ref } from 'vue'
import { call } from '../bridge'

export interface Extraction { Offset: number; Length: number; RelativeToMatch: boolean; BigEndian: boolean; Signed: boolean; Encoding: number }
export interface Variable { Id: string; Name: string; Kind: number; DataType: number; Value: string; CurrentValue?: string; Extraction: Extraction; TtlSeconds: number }
export interface Extractor { Id: string; Name: string; IsEnable: boolean; Scope: number; Description: string; Variables: Variable[] }

export const extractorRows = ref<Extractor[]>([])

/** 仅在首次进入侧栏或取值器配置发生写入后调用。 */
export async function ensurePacketExtractors(): Promise<void> {
  extractorRows.value = (await call<{ rows: Extractor[] }>('getPacketExtractors')).rows || []
}
