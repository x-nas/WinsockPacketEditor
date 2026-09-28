<script setup lang="ts">
/*
  解码结果弹窗。列表 / 详情右键「解码 ▸」与解码器页共用的展示件。

  动作：复制 / 放回输入（跳到解码器页并选中同一个解码器）/ 加入文本 A·B /
  添加到发送器（把解码后的明文当一条发送包加进某个发送列表）。
*/
import { ref, computed } from 'vue'
import { call } from '../../bridge'
import { FeedList, type SendRow } from '../../bridge/types'
import { t } from '../../i18n'
import { pushToast } from '../../stores/toast'
import { textA, textB } from '../../stores/tools'
import { gotoPage } from '../../stores/runtime'
import { useList } from '../../stores/lists'
import SettingsModal from '../proxy/SettingsModal.vue'
import CyberSelect from '../CyberSelect.vue'
import { decInput, decMode, decOutput, decSelectedId, decTransformDirection } from '../../stores/decoder'
import type { DecodePayload } from './actions'

const props = defineProps<{ payload: DecodePayload | null }>()
const emit = defineEmits<{ (e: 'close'): void }>()

const sends = useList<SendRow>(FeedList.Send)
const sendSid = ref('')
const sendOptions = computed(() => [
  { value: '', label: t('dec.selectSend') },
  ...sends.value.map((s) => ({ value: s.Id, label: s.Name })),
])

async function copyOut(): Promise<void> {
  const s = props.payload?.text || props.payload?.hex || ''
  if (!s) return
  try { await call('clipboardWrite', { text: s }); pushToast('success', t('pm.copied')) }
  catch (e) { console.error('[dec] copy failed', e) }
}

function useAsInput(): void {
  const p = props.payload
  if (!p) return
  //解码结果带回工作台时，最常见的下一步是修改明文后重新编码。
  //因此明确落在编码模式，避免“明文却出现在解码的密文输入框”这一反直觉状态。
  decTransformDirection.value = 'encode'
  decInput.value = p.text || ''
  decOutput.value = ''
  decMode.value = 'decoder'
  if (p.decoderId) decSelectedId.value = p.decoderId
  gotoPage.value = 'decoder'
  emit('close')
}

function toText(which: 'a' | 'b'): void {
  const p = props.payload
  if (!p?.hex) return
  if (which === 'a') textA.value = p.hex
  else textB.value = p.hex
  pushToast('success', t(which === 'a' ? 'pm.toTextAOk' : 'pm.toTextBOk'))
  gotoPage.value = 'diff'
  emit('close')
}

async function addToSend(): Promise<void> {
  const p = props.payload
  if (!p || !sendSid.value || !p.list || !p.id) return
  try {
    const r = await call<{ ok: boolean }>('packetEditToSend', { sid: sendSid.value, list: p.list, id: p.id, buffer: p.bufferB64 })
    pushToast(r?.ok ? 'success' : 'error', t(r?.ok ? 'pm.added' : 'pm.addFail'))
  } catch (e) { console.error('[dec] 添加到发送失败', e) }
}
</script>

<template>
  <SettingsModal :open="props.payload !== null" :title="t('dec.resultTitle')" subtitle="Decode" readonly :width="720"
                 @update:open="emit('close')">
    <div class="setf dr" v-if="props.payload">
      <div class="who">{{ props.payload.decoderName }}</div>

      <div v-if="props.payload.error" class="err">{{ props.payload.error }}</div>
      <template v-else>
        <div class="lbl">{{ t('tr.outPane') }}</div>
        <pre class="box">{{ props.payload.text }}</pre>
        <div class="lbl">{{ t('dec.hexLabel') }}</div>
        <pre class="box hx">{{ props.payload.hex }}</pre>

        <div class="acts">
          <button class="op" @click="copyOut">{{ t('tr.copyOut') }}</button>
          <button class="op" @click="useAsInput">{{ t('tr.useAsInput') }}</button>
          <button class="op" @click="toText('a')">{{ t('pm.toTextA') }}</button>
          <button class="op" @click="toText('b')">{{ t('pm.toTextB') }}</button>
        </div>

        <div v-if="props.payload.list && sends.length" class="send">
          <span class="sl">{{ t('pm.toSend') }}</span>
          <CyberSelect class="grow" v-model="sendSid" :options="sendOptions" />
          <button class="op" :disabled="!sendSid" @click="addToSend">{{ t('dec.add') }}</button>
        </div>
      </template>
    </div>
  </SettingsModal>
</template>

<style scoped>
.dr { padding: 4px 0 8px; }
.who { padding: 8px 20px; font-family: var(--mono); font-size: var(--fs-small); color: var(--cyan); }
.lbl { padding: 8px 20px 4px; color: var(--muted); font: var(--fs-small) var(--mono); }
.box { margin: 0 20px; padding: 8px 10px; max-height: 180px; overflow: auto; white-space: pre-wrap; overflow-wrap: anywhere; background: rgb(var(--inset-rgb) / 30%); border: 1px solid var(--border); font: var(--fs-small)/1.6 var(--mono); color: var(--acc-green2); }
.box.hx { color: var(--gray); }
.err { margin: 0 20px; padding: 10px; color: var(--danger); font-size: var(--fs-small); }
.acts { display: flex; gap: 8px; padding: 12px 20px 4px; }
.op { padding: 6px 12px; border: 1px solid var(--border); background: transparent; color: var(--gray); cursor: pointer; font-size: var(--fs-small); }
.op:hover:not(:disabled) { color: var(--cyan); border-color: var(--cyan); }
.op:disabled { opacity: .4; cursor: default; }
.send { display: flex; align-items: center; gap: 8px; padding: 10px 20px 0; }
.send .sl { flex: none; color: var(--muted); font: var(--fs-small) var(--mono); }
.send .grow { flex: 1; min-width: 0; }
</style>
