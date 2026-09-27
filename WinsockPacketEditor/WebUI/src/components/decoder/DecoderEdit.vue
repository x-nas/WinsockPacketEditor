<script setup lang="ts">
/*
  解码器编辑弹窗。分区卡：基础信息 / 算法 / 帧配置 / 适用范围。
  按类型显示不同字段；保存走 saveDecoder（校验在 C# 侧）。
*/
import { computed, ref, watch } from 'vue'
import { call } from '../../bridge'
import type { DecoderRow } from '../../bridge/types'
import { t } from '../../i18n'
import { pushToast } from '../../stores/toast'
import SettingsModal from '../proxy/SettingsModal.vue'
import CyberSelect from '../CyberSelect.vue'
import { CHARSET_LABELS, CIPHER_MODES, DecCharset, DecKeyFormat, DecKind, PADDINGS, isCipher, needsKey } from './enums'

const props = defineProps<{ target: DecoderRow | null }>()
const emit = defineEmits<{ (e: 'close'): void; (e: 'saved'): void }>()

const f = ref<DecoderRow>(blank())
const busy = ref(false)
const error = ref('')

function blank(): DecoderRow {
  return {
    Id: '', IsEnable: true, Name: '', Description: '',
    Kind: DecKind.Xor, Charset: DecCharset.UTF8, KeyFormat: DecKeyFormat.Hex, Key: '',
    IvFormat: DecKeyFormat.Hex, Iv: '', CipherMode: 0, Padding: 1, BlockSize: 0,
    LengthBytes: 0, BigEndian: false, LengthIncludesSelf: false,
    HasFixedHeader: false, FixedHeader: '', LengthIncludesFixedHeader: false, DataOffset: 0,
    ProtocolType: 0, Direction: 0, ParamsJson: '',
  }
}

watch(() => props.target, (v) => {
  if (v) { f.value = { ...v } }
  error.value = ''
}, { immediate: true })

const kindOptions = computed(() => [
  { value: DecKind.Xor, label: 'XOR' },
  { value: DecKind.Aes, label: 'AES' },
  { value: DecKind.Des, label: 'DES' },
  { value: DecKind.Protobuf, label: 'Protobuf' },
  { value: DecKind.MessagePack, label: 'MessagePack' },
  { value: DecKind.Rc4, label: 'RC4' },
  { value: DecKind.Xxtea, label: 'XXTEA' },
  { value: DecKind.Amf, label: 'AMF0 / AMF3' },
  { value: DecKind.Bson, label: 'BSON' },
  { value: DecKind.FlatBuffers, label: 'FlatBuffers' },
  { value: DecKind.TextCharset, label: t('dec.kindText') },
])
const protocolOptions = computed(() => [
  { value: 0, label: t('dec.any') },
  { value: 1, label: 'TCP' }, { value: 2, label: 'UDP' }, { value: 3, label: 'HTTP' }, { value: 4, label: 'WebSocket' },
])
const directionOptions = computed(() => [
  { value: 0, label: t('dec.any') }, { value: 1, label: t('dec.dirReq') }, { value: 2, label: t('dec.dirResp') },
])
const keyFormatOptions = computed(() => [
  { value: DecKeyFormat.Hex, label: t('dec.fmtHex') },
  { value: DecKeyFormat.Base64, label: 'Base64' },
  { value: DecKeyFormat.Text, label: t('dec.fmtText') },
])
const charsetOptions = computed(() => CHARSET_LABELS.map((label, value) => ({
  value, label: value === 0 ? t('dec.charsetDefault') : label,
})))
const cipherOptions = CIPHER_MODES.map((label, value) => ({ value, label }))
const paddingOptions = PADDINGS.map((label, value) => ({ value, label }))
const lengthOptions = [
  { value: 0, label: '0' }, { value: 1, label: '1' }, { value: 2, label: '2' }, { value: 4, label: '4' },
]

const showCipher = computed(() => isCipher(f.value.Kind))
const showKey = computed(() => needsKey(f.value.Kind))
const showCharset = computed(() => f.value.Kind === DecKind.TextCharset)

async function save(): Promise<void> {
  busy.value = true
  error.value = ''
  try {
    const r = await call<{ ok: boolean; error?: string }>('saveDecoder', { decoder: f.value })
    if (!r?.ok) { error.value = r?.error ?? t('dec.saveFail'); return }
    pushToast('success', t('set.save'))
    emit('saved')
    emit('close')
  } catch (e) {
    error.value = String(e)
  } finally {
    busy.value = false
  }
}
</script>

<template>
  <SettingsModal :open="props.target !== null" :title="t('dec.edit')" subtitle="Decoder" :busy="busy" :error="error"
                 :width="680" @update:open="emit('close')" @save="save">
    <div class="setf de">
      <section class="sec">
        <div class="grp">{{ t('dec.grpBase') }}</div>
        <div class="row"><div class="k">{{ t('dec.name') }}</div><div class="v"><input class="inp" v-model="f.Name" /></div></div>
        <div class="row"><div class="k">{{ t('dec.desc') }}</div><div class="v"><input class="inp" v-model="f.Description" /></div></div>
      </section>

      <section class="sec">
        <div class="grp">{{ t('dec.grpAlgo') }}</div>
        <div class="row"><div class="k">{{ t('dec.kind') }}</div><div class="v"><CyberSelect class="full" v-model="f.Kind" :options="kindOptions" /></div></div>
        <div v-if="showCharset" class="row"><div class="k">{{ t('dec.charset') }}</div><div class="v"><CyberSelect class="full" v-model="f.Charset" :options="charsetOptions" /></div></div>
        <template v-if="showKey">
          <div class="row"><div class="k">{{ t('dec.key') }}</div><div class="v"><div class="inline"><CyberSelect class="fmt" v-model="f.KeyFormat" :options="keyFormatOptions" /><input class="inp" v-model="f.Key" :placeholder="f.KeyFormat === 0 ? '01 02 A0 FF' : ''" /></div></div></div>
          <div v-if="showCipher" class="row"><div class="k">{{ t('dec.iv') }}</div><div class="v"><div class="inline"><CyberSelect class="fmt" v-model="f.IvFormat" :options="keyFormatOptions" /><input class="inp" v-model="f.Iv" :placeholder="f.IvFormat === 0 ? '00 11 22 …' : ''" /></div></div></div>
          <div v-if="showCipher" class="row"><div class="k">{{ t('dec.mode') }}</div><div class="v"><CyberSelect class="full" v-model="f.CipherMode" :options="cipherOptions" /></div></div>
          <div v-if="showCipher" class="row"><div class="k">{{ t('dec.padding') }}</div><div class="v"><CyberSelect class="full" v-model="f.Padding" :options="paddingOptions" /></div></div>
        </template>
      </section>

      <section class="sec">
        <div class="grp">{{ t('dec.grpFrame') }}</div>
        <div class="row"><div class="k">{{ t('dec.lengthBytes') }}</div><div class="v"><CyberSelect class="num" v-model="f.LengthBytes" :options="lengthOptions" /></div></div>
        <div class="row"><div class="k">{{ t('dec.bigEndian') }}</div><div class="v"><div class="flags"><button class="chk" :class="{ on: f.BigEndian }" @click="f.BigEndian = !f.BigEndian"><i />{{ t('dec.bigEndian') }}</button>
          <button class="chk" :class="{ on: f.LengthIncludesSelf }" @click="f.LengthIncludesSelf = !f.LengthIncludesSelf"><i />{{ t('dec.lenSelf') }}</button></div></div></div>
        <div class="row"><div class="k">{{ t('dec.hasHeader') }}</div><div class="v"><div class="inline"><button class="chk" :class="{ on: f.HasFixedHeader }" @click="f.HasFixedHeader = !f.HasFixedHeader"><i />{{ t('dec.hasHeader') }}</button>
          <input class="inp" v-model="f.FixedHeader" :disabled="!f.HasFixedHeader" placeholder="AB CD" /></div></div></div>
        <div class="row"><div class="k">{{ t('dec.lenHeader') }}</div><div class="v"><button class="chk" :class="{ on: f.LengthIncludesFixedHeader }" @click="f.LengthIncludesFixedHeader = !f.LengthIncludesFixedHeader"><i />{{ t('dec.lenHeader') }}</button></div></div>
        <div class="row"><div class="k">{{ t('dec.offset') }}</div><div class="v"><input class="inp num" type="number" min="0" v-model.number="f.DataOffset" /></div></div>
      </section>

      <section class="sec">
        <div class="grp">{{ t('dec.grpScope') }}</div>
        <p class="hint">{{ t('dec.scopeHint') }}</p>
        <div class="row"><div class="k">{{ t('dec.protocol') }}</div><div class="v"><CyberSelect class="full" v-model="f.ProtocolType" :options="protocolOptions" /></div></div>
        <div class="row"><div class="k">{{ t('dec.direction') }}</div><div class="v"><CyberSelect class="full" v-model="f.Direction" :options="directionOptions" /></div></div>
      </section>
    </div>
  </SettingsModal>
</template>

<style scoped>
.de { padding-bottom: 8px; }
.de .row { height: auto; }
.de .inline { display: flex; gap: 8px; align-items: center; width: 100%; min-width: 0; }
.de .full { display: flex; width: 100%; }
.de .fmt { flex: none; width: 120px; }
.de .num { flex: none; width: 120px; }
.de .flags { display: flex; flex-wrap: wrap; gap: 8px; }
.de .chk { white-space: nowrap; }
.de .hint { margin: 0 0 8px; color: var(--dim2); font-size: var(--fs-small); line-height: 1.5; }
</style>
