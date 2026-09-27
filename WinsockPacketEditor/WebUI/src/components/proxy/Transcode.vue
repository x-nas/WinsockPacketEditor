<script setup lang="ts">
/* 单一编解码工作台：左输入、中间算法与参数、右输出。 */
import { computed, ref, watch } from 'vue'
import { call } from '../../bridge'
import { t } from '../../i18n'
import { pushToast } from '../../stores/toast'
import { trInput } from '../../stores/tools'
import CyberSelect from '../CyberSelect.vue'

type Result = { Ok?: boolean; Error?: string; Text?: string; text?: string }
const props = withDefaults(defineProps<{ embedded?: boolean }>(), { embedded: false })
const emit = defineEmits<{ (e: 'saveAsDecoder', draft: { kind: 'xor' | 'aes'; key: string; keyFormat: 'hex' | 'text'; iv: string }): void }>()
const codec = ref('utf8')
const key = ref('')
const iv = ref('')
const xorMode = ref<'key' | 'b'>('key')
const xorKeyFormat = ref<'hex' | 'text'>('hex')
const xorB = ref('')
const output = ref('')
const error = ref('')
const busy = ref(false)
let seq = 0
let timer = 0

const charCodecs = computed<[string, string][]>(() => [
  ['default', t('tr.codecDefault')], ['gbk', 'GBK'], ['utf7', 'UTF-7'], ['utf8', 'UTF-8'],
  ['utf16be', 'UTF-16 BE'], ['utf32', 'UTF-32 LE'], ['utf16le', 'UTF-16 LE'], ['base64', 'Base64'],
])
const codecs = computed<[string, string][]>(() => [
  ...charCodecs.value, ['xor', 'XOR'], ['aes-cbc', 'AES-CBC / PKCS7'], ['protobuf', t('tr.codecProtobuf')],
])
const codecOptions = computed(() => codecs.value.map(([value, label]) => ({ value, label })))
const xorKeyOptions = computed(() => [
  { value: 'hex', label: t('tr.xorKeyHex') },
  { value: 'text', label: t('tr.xorKeyText') },
])
const isProtocol = computed(() => ['aes-cbc', 'protobuf'].includes(codec.value))
const needsKey = computed(() => codec.value === 'aes-cbc')
const needsIv = computed(() => codec.value === 'aes-cbc')
const selectedLabel = computed(() => codecs.value.find(x => x[0] === codec.value)?.[1] ?? codec.value)
const inputBytes = computed(() => new TextEncoder().encode(trInput.value).length)
const outputBytes = computed(() => new TextEncoder().encode(output.value).length)

watch([trInput, output, codec, key, iv, xorMode, xorKeyFormat, xorB], () => { error.value = ''; window.clearTimeout(timer) })

function hexBytes(value: string): Uint8Array | null {
  const hex = value.replace(/[\s-]/g, '')
  if (!hex || (hex.length & 1) || !/^[0-9a-f]+$/i.test(hex)) return null
  const bytes = new Uint8Array(hex.length / 2)
  for (let i = 0; i < bytes.length; i++) bytes[i] = parseInt(hex.slice(i * 2, i * 2 + 2), 16)
  return bytes
}
function xorKeyBytes(): Uint8Array | null {
  if (xorKeyFormat.value === 'hex') return hexBytes(key.value)
  if (!key.value.length) return null
  const bytes = new Uint8Array(key.value.length)
  for (let i = 0; i < key.value.length; i++) bytes[i] = key.value.charCodeAt(i) & 0xff
  return bytes
}
function toHex(bytes: Uint8Array): string {
  return Array.from(bytes, b => b.toString(16).padStart(2, '0').toUpperCase()).join(' ')
}
function fromBase64(value: string | undefined): Uint8Array {
  const bin = atob(value ?? '')
  return Uint8Array.from(bin, c => c.charCodeAt(0))
}
async function transform(direction: 'enc' | 'dec'): Promise<void> {
  const source = direction === 'enc' ? trInput.value : output.value
  if (!source.trim()) return
  const mine = ++seq
  busy.value = true; error.value = ''
  // 只有编码才把右侧当输出区清空；解码时右侧是输入（密文），要留着让用户能改参数重解或复用。
  if (direction === 'enc') output.value = ''
  try {
    if (codec.value === 'xor') {
      const left = hexBytes(source)
      const right = xorMode.value === 'b' ? hexBytes(xorB.value) : xorKeyBytes()
      if (!left || !right) throw new Error(xorMode.value === 'b' ? t('tr.errXorB') : t('tr.errXorKey'))
      const length = xorMode.value === 'b' ? Math.max(left.length, right.length) : left.length
      const bytes = new Uint8Array(length)
      for (let i = 0; i < length; i++) bytes[i] = (i < left.length ? left[i] : 0) ^ (xorMode.value === 'b' ? (i < right.length ? right[i] : 0) : right[i % right.length])
      if (mine === seq) {
        if (direction === 'enc') output.value = toHex(bytes)
        else trInput.value = toHex(bytes)
      }
      return
    }
    if (isProtocol.value && direction === 'dec') {
      const hex = source.replace(/[\s-]/g, '')
      if (!hex || (hex.length & 1) || !/^[0-9a-f]+$/i.test(hex)) throw new Error(t('tr.errOddHex'))
      let bin = ''; for (let i = 0; i < hex.length; i += 2) bin += String.fromCharCode(parseInt(hex.slice(i, i + 2), 16))
      const r = await call<Result>('decodeBytes', { data: btoa(bin), kind: codec.value, key: key.value, iv: iv.value })
      if (mine !== seq) return
      if (!r.Ok) error.value = r.Error ?? t('tr.errDecode')
      else trInput.value = r.Text ?? ''
    } else if (isProtocol.value && direction === 'enc') {
      if (codec.value === 'protobuf') throw new Error(t('tr.decHexOnly'))
      const r = await call<Result>('encodeBytes', { text: source, kind: codec.value, key: key.value, iv: iv.value })
      if (mine !== seq) return
      if (!r.Ok) error.value = r.Error ?? t('tr.errDecode')
      else output.value = toHex(fromBase64((r as Result & { OutputBase64?: string }).OutputBase64))
    } else {
      const r = await call<Result>('transcodeOne', { text: source, decode: direction === 'dec', format: codec.value })
      if (mine === seq) {
        if (direction === 'enc') output.value = r.text ?? ''
        else trInput.value = r.text ?? ''
      }
    }
  } catch (e) { if (mine === seq) error.value = String(e) }
  finally { if (mine === seq) busy.value = false }
}
async function copyOut(): Promise<void> { if (!output.value) return; try { await call('clipboardWrite', { text: output.value }); pushToast('success', t('pm.copied')) } catch (e) { console.error('[tr] copy failed', e) } }
async function copyIn(): Promise<void> { if (!trInput.value) return; try { await call('clipboardWrite', { text: trInput.value }); pushToast('success', t('pm.copied')) } catch (e) { console.error('[tr] copy failed', e) } }
function clearAll(): void { trInput.value = ''; output.value = ''; error.value = '' }
defineExpose({ clearAll })
const canSaveAsDecoder = computed(() => (codec.value === 'xor' && xorMode.value === 'key' && !!key.value.trim()) || (codec.value === 'aes-cbc' && !!key.value.trim()))
function saveAsDecoder(): void {
  if (!canSaveAsDecoder.value) return
  emit('saveAsDecoder', codec.value === 'aes-cbc'
    ? { kind: 'aes', key: key.value, keyFormat: 'hex', iv: iv.value }
    : { kind: 'xor', key: key.value, keyFormat: xorKeyFormat.value, iv: '' })
}
</script>

<template>
  <div class="tr" :class="{ embedded: props.embedded }">
    <div class="workbench">
      <section class="pane"><div class="ph"><span class="tt">{{ t('tr.inputPane') }}</span><span class="meta">{{ trInput.length }} {{ t('tr.chars') }} · {{ inputBytes }} {{ t('tr.bytes') }}</span><span class="fill" /><button class="op" :disabled="!trInput" :title="t('tr.copyOut')" @click="copyIn">⧉</button></div><textarea v-model="trInput" spellcheck="false" :placeholder="t('tr.inPhEnc')" /></section>
      <section class="control"><div class="ph"><span class="tt">{{ t('tr.algorithm') }}</span></div><div class="controls"><label>{{ t('tr.algorithm') }}<CyberSelect v-model="codec" :options="codecOptions" /></label><template v-if="codec === 'xor'"><div class="hx-seg xor-modes"><button class="hx-segb after" :class="{ on: xorMode === 'key' }" @click="xorMode = 'key'">{{ t('tr.xorKeyMode') }}</button><button class="hx-segb before" :class="{ on: xorMode === 'b' }" @click="xorMode = 'b'">{{ t('tr.xorBMode') }}</button></div><label v-if="xorMode === 'key'" class="field key-field">{{ t('tr.xorKey') }}<CyberSelect v-model="xorKeyFormat" :options="xorKeyOptions" /><textarea v-model="key" spellcheck="false" :placeholder="xorKeyFormat === 'hex' ? '01 02 A0 FF' : t('tr.xorKeyPh')" /></label><label v-else class="field xor-b">{{ t('tr.xorDataB') }}<textarea v-model="xorB" spellcheck="false" :placeholder="t('tr.xorDataBPh')" /></label><p>{{ xorMode === 'b' ? t('tr.xorPadB') : t('tr.xorPadKey') }}</p></template><template v-else><label v-if="needsKey" class="field">{{ t('tr.keyHex') }}<textarea v-model="key" spellcheck="false" placeholder="01 02 A0 FF" /></label><label v-if="needsIv" class="field">{{ t('tr.ivHex') }}<textarea v-model="iv" spellcheck="false" placeholder="00 11 22 …" /></label><p v-if="codec === 'protobuf'">{{ t('tr.protobufHint') }}</p><p v-else-if="isProtocol">{{ t('tr.decHexOnly') }}</p><p v-else>{{ t('tr.encToHex') }}</p></template><button class="go" :disabled="busy || !trInput.trim() || codec === 'protobuf'" @click="transform('enc')">{{ busy ? t('proxy.working') : t('tr.doEnc') }} →</button><button class="go reverse" :disabled="busy || !output.trim()" @click="transform('dec')">← {{ t('tr.doDec') }}</button><button v-if="canSaveAsDecoder" class="save-dec" @click="saveAsDecoder">{{ t('dec.saveAsDecoder') }}</button></div></section>
      <section class="pane out"><div class="ph"><span class="tt">{{ t('tr.outPane') }} · {{ selectedLabel }}</span><span class="meta">{{ outputBytes }} {{ t('tr.bytes') }}</span><span class="fill" /><button class="op" :disabled="!output" :title="t('tr.copyOut')" @click="copyOut">⧉</button></div><div v-if="error" class="error">{{ error }}</div><textarea v-model="output" spellcheck="false" :placeholder="t('tr.outPh')" /></section>
    </div>
  </div>
</template>

<style scoped>
.tr{flex:1;min-width:0;min-height:0;display:flex;flex-direction:column;gap:8px;padding:10px 12px 12px}.tr.embedded{padding:0}.workbench{flex:1;min-height:0;display:grid;grid-template-columns:minmax(0,1fr) 250px minmax(0,1fr);gap:8px}.pane,.control{min-width:0;min-height:0;display:flex;flex-direction:column;border:1px solid var(--border);background:var(--sink)}.ph{flex:none;display:flex;align-items:center;gap:10px;height:var(--th-h);padding:0 12px;background:var(--panel);border-bottom:1px solid var(--border);font-family:var(--share);font-size:var(--th-size);letter-spacing:.12em;text-transform:uppercase;color:var(--th-fg)}.tt{color:var(--cyan);min-width:0;overflow:hidden;text-overflow:ellipsis;white-space:nowrap}.meta{color:var(--muted);letter-spacing:.02em;text-transform:none;min-width:0;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;flex-shrink:100}.fill{flex:1}.pane textarea,.pane pre{flex:1;min-height:0;margin:0;padding:10px 12px;resize:none;border:0;outline:0;background:transparent;color:var(--gray);caret-color:var(--cyan);font:var(--fs-body)/1.6 var(--mono);white-space:pre-wrap;overflow:auto;overflow-wrap:anywhere}.out textarea{color:var(--acc-green2)}.controls{flex:1;min-height:0;display:flex;flex-direction:column;gap:12px;padding:14px}.controls label{display:grid;gap:5px;color:var(--muted);font:var(--fs-small) var(--mono)}.controls textarea{box-sizing:border-box;border:1px solid var(--border);outline:0;background:rgb(var(--inset-rgb) / 30%);color:var(--gray);caret-color:var(--cyan);font:var(--fs-small) var(--mono);text-align:left;vertical-align:top;padding:7px 8px;min-height:0;resize:none;overflow:auto}.controls textarea:hover{border-color:var(--dim)}.controls textarea:focus{border-color:var(--cyan)}.xor-modes{display:flex;width:100%}.xor-modes .hx-segb{flex:1;min-width:0}.controls .field{flex:1;min-height:0;grid-template-rows:auto minmax(0,1fr);align-content:start}.controls .key-field{grid-template-rows:auto auto minmax(0,1fr)}.field textarea{width:100%;height:100%;min-height:0}.controls p{margin:0;color:var(--dim2);font-size:var(--fs-small);line-height:1.6}.go,.save-dec{min-height:32px;height:auto;padding:3px 8px;box-sizing:border-box;line-height:1.25;white-space:normal;overflow-wrap:break-word;border:1px solid var(--cyan);background:rgb(var(--cyan-rgb)/10%);color:var(--cyan);cursor:pointer;font:var(--fs-small) var(--share);letter-spacing:.12em}.go{margin-top:auto}.go.reverse{margin-top:0;border-color:var(--amber);color:var(--amber);background:rgb(var(--amber-rgb)/10%)}.save-dec{border-color:var(--green);color:var(--green);background:rgb(var(--green-rgb)/8%)}.go:disabled,.op:disabled{opacity:.45;cursor:default}.op{min-width:26px;height:24px;border:1px solid var(--border);background:transparent;color:var(--gray);cursor:pointer}.error{display:block;padding:8px 12px;border-bottom:1px solid var(--border);color:var(--danger);font:var(--fs-small) var(--mono);text-align:left;white-space:pre-wrap;overflow-wrap:anywhere}
</style>
