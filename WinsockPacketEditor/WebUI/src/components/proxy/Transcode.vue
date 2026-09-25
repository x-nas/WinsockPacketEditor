<script setup lang="ts">
/* 单一编解码工作台：左输入、中间算法与参数、右输出。 */
import { computed, ref, watch } from 'vue'
import { call } from '../../bridge'
import { t } from '../../i18n'
import { pushToast } from '../../stores/toast'
import { trInput, trMode } from '../../stores/tools'
import CyberSelect from '../CyberSelect.vue'

type Result = { Ok?: boolean; Error?: string; Text?: string; text?: string }
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

const charCodecs = [
  ['default', '本机默认编码'], ['gbk', 'GBK'], ['utf7', 'UTF-7'], ['utf8', 'UTF-8'],
  ['utf16be', 'UTF-16 BE'], ['utf32', 'UTF-32 LE'], ['utf16le', 'UTF-16 LE'], ['base64', 'Base64'],
]
const decodeCodecs = computed(() => [
  ...charCodecs, ['xor', 'XOR'], ['aes-cbc', 'AES-CBC / PKCS7'], ['protobuf', 'Protobuf（推测结构）'],
])
const options = computed(() => trMode.value === 'enc' ? charCodecs : decodeCodecs.value)
const codecOptions = computed(() => options.value.map(([value, label]) => ({ value, label })))
const xorKeyOptions = [
  { value: 'hex', label: '十六进制' },
  { value: 'text', label: '文本（Latin-1）' },
]
const isProtocol = computed(() => ['aes-cbc', 'protobuf'].includes(codec.value))
const needsKey = computed(() => codec.value === 'aes-cbc')
const needsIv = computed(() => codec.value === 'aes-cbc')
const selectedLabel = computed(() => options.value.find(x => x[0] === codec.value)?.[1] ?? codec.value)
const inputHint = computed(() => trMode.value === 'dec' ? '输入十六进制（空格可有可无）' : '输入原文文本')
const inputBytes = computed(() => new TextEncoder().encode(trInput.value).length)
const outputBytes = computed(() => new TextEncoder().encode(output.value).length)

watch(trMode, () => { if (trMode.value === 'enc' && isProtocol.value) codec.value = 'utf8' })
watch([trInput, trMode, codec, key, iv, xorMode, xorKeyFormat, xorB], () => { output.value = ''; error.value = ''; window.clearTimeout(timer) })

function inputAsBase64(): string {
  const hex = trInput.value.replace(/[\s-]/g, '')
  if (!hex || (hex.length & 1) || !/^[0-9a-f]+$/i.test(hex)) throw new Error('解码输入必须是偶数位十六进制')
  let bin = ''
  for (let i = 0; i < hex.length; i += 2) bin += String.fromCharCode(parseInt(hex.slice(i, i + 2), 16))
  return btoa(bin)
}
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
async function transform(): Promise<void> {
  if (!trInput.value.trim()) return
  const mine = ++seq
  busy.value = true; error.value = ''; output.value = ''
  try {
    if (codec.value === 'xor') {
      const left = hexBytes(trInput.value)
      const right = xorMode.value === 'b' ? hexBytes(xorB.value) : xorKeyBytes()
      if (!left || !right) throw new Error(xorMode.value === 'b' ? '数据 A 与数据 B 必须是有效十六进制' : 'XOR 密钥无效')
      const length = xorMode.value === 'b' ? Math.max(left.length, right.length) : left.length
      const bytes = new Uint8Array(length)
      for (let i = 0; i < length; i++) bytes[i] = (i < left.length ? left[i] : 0) ^ (xorMode.value === 'b' ? (i < right.length ? right[i] : 0) : right[i % right.length])
      if (mine === seq) output.value = toHex(bytes)
      return
    }
    if (trMode.value === 'dec' && isProtocol.value) {
      const r = await call<Result>('decodeBytes', { data: inputAsBase64(), kind: codec.value, key: key.value, iv: iv.value })
      if (mine !== seq) return
      if (!r.Ok) error.value = r.Error ?? '解码失败'
      else output.value = r.Text ?? ''
    } else {
      const r = await call<Result>('transcodeOne', { text: trInput.value, decode: trMode.value === 'dec', format: codec.value })
      if (mine === seq) output.value = r.text ?? ''
    }
  } catch (e) { if (mine === seq) error.value = String(e) }
  finally { if (mine === seq) busy.value = false }
}
async function copyOut(): Promise<void> { if (!output.value) return; try { await call('clipboardWrite', { text: output.value }); pushToast('success', t('pm.copied')) } catch (e) { console.error('[tr] copy failed', e) } }
function useOut(): void { if (output.value) trInput.value = output.value }
function clearAll(): void { trInput.value = ''; output.value = ''; error.value = '' }
</script>

<template>
  <div class="page list-page tr">
    <div class="bar">
      <div class="hx-seg"><button class="hx-segb after" :class="{ on: trMode === 'enc' }" @click="trMode = 'enc'">{{ t('tr.encode') }}</button><button class="hx-segb before" :class="{ on: trMode === 'dec' }" @click="trMode = 'dec'">{{ t('tr.decode') }}</button></div>
      <span class="lb">{{ trMode === 'enc' ? '左侧原文 → 选择编码器 → 右侧字节（Hex）' : '左侧十六进制 → 选择解码器 → 右侧结果' }}</span>
      <button class="btn danger" :disabled="!trInput && !output" @click="clearAll">{{ t('rb.clearAll') }}</button>
    </div>
    <div class="workbench">
      <section class="pane"><div class="ph"><span class="tt">原文 / 输入</span><span class="meta">{{ trInput.length }} 字符 · {{ inputBytes }} 字节</span></div><textarea v-model="trInput" spellcheck="false" :placeholder="inputHint" /></section>
      <section class="control"><div class="ph"><span class="tt">{{ trMode === 'enc' ? '编码器' : '解码器' }}</span></div><div class="controls"><label>算法<CyberSelect v-model="codec" :options="codecOptions" /></label><template v-if="codec === 'xor'"><div class="hx-seg xor-modes"><button class="hx-segb after" :class="{ on: xorMode === 'key' }" @click="xorMode = 'key'">数据 ⊕ 密钥</button><button class="hx-segb before" :class="{ on: xorMode === 'b' }" @click="xorMode = 'b'">数据 A ⊕ B</button></div><label v-if="xorMode === 'key'" class="field key-field">密钥<CyberSelect v-model="xorKeyFormat" :options="xorKeyOptions" /><textarea v-model="key" spellcheck="false" :placeholder="xorKeyFormat === 'hex' ? '01 02 A0 FF' : '输入文本密钥'" /></label><label v-else class="field xor-b">数据 B（HEX）<textarea v-model="xorB" spellcheck="false" placeholder="输入第二段十六进制数据" /></label><p>{{ xorMode === 'b' ? '长度不足的一端按 00 补齐。' : '密钥将循环铺满数据 A。' }}</p></template><template v-else><label v-if="needsKey" class="field">密钥（HEX）<textarea v-model="key" spellcheck="false" placeholder="01 02 A0 FF" /></label><label v-if="needsIv" class="field">IV（HEX，16 字节）<textarea v-model="iv" spellcheck="false" placeholder="00 11 22 …" /></label><p v-if="codec === 'protobuf'">不含 .proto 时显示字段号与推测结构。</p><p v-else-if="isProtocol">输入按十六进制读取，只读解码。</p><p v-else>{{ trMode === 'enc' ? '结果将以 Hex 显示。' : '输入按 Hex 读取。' }}</p></template><button class="go" :disabled="busy || !trInput.trim()" @click="transform">{{ busy ? '处理中…' : (codec === 'xor' ? '计算 →' : (trMode === 'enc' ? '编码 →' : '解码 →')) }}</button></div></section>
      <section class="pane out"><div class="ph"><span class="tt">结果 · {{ selectedLabel }}</span><span class="meta">{{ outputBytes }} 字节</span><span class="fill" /><button class="op" :disabled="!output" title="放回输入" @click="useOut">←</button><button class="op" :disabled="!output" :title="t('lst.copy')" @click="copyOut">⧉</button></div><pre v-if="output">{{ output }}</pre><div v-else-if="error" class="error">{{ error }}</div><div v-else class="empty">选择算法后执行编解码。</div></section>
    </div>
  </div>
</template>

<style scoped>
.page{flex:1;min-width:0;min-height:0;display:flex;flex-direction:column;gap:8px;padding:10px 12px 12px}.lb{flex:1;min-width:0;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;color:var(--dim2);font-size:var(--fs-small)}.workbench{flex:1;min-height:0;display:grid;grid-template-columns:minmax(0,1fr) 250px minmax(0,1fr);gap:8px}.pane,.control{min-width:0;min-height:0;display:flex;flex-direction:column;border:1px solid var(--border);background:var(--sink)}.ph{flex:none;display:flex;align-items:center;gap:10px;height:var(--th-h);padding:0 12px;background:var(--panel);border-bottom:1px solid var(--border);font-family:var(--share);font-size:var(--th-size);letter-spacing:.12em;text-transform:uppercase;color:var(--th-fg)}.tt{color:var(--cyan)}.meta{color:var(--muted);letter-spacing:.02em;text-transform:none}.fill{flex:1}.pane textarea,.pane pre{flex:1;min-height:0;margin:0;padding:10px 12px;resize:none;border:0;outline:0;background:transparent;color:var(--gray);caret-color:var(--cyan);font:var(--fs-body)/1.6 var(--mono);white-space:pre-wrap;overflow:auto;overflow-wrap:anywhere}.out pre{color:var(--acc-green2);user-select:text}.controls{flex:1;min-height:0;display:flex;flex-direction:column;gap:12px;padding:14px}.controls label{display:grid;gap:5px;color:var(--muted);font:var(--fs-small) var(--mono)}.controls textarea{box-sizing:border-box;border:1px solid var(--border);outline:0;background:rgb(var(--inset-rgb) / 30%);color:var(--gray);caret-color:var(--cyan);font:var(--fs-small) var(--mono);text-align:left;vertical-align:top;padding:7px 8px;min-height:0;resize:none;overflow:auto}.controls textarea:hover{border-color:var(--dim)}.controls textarea:focus{border-color:var(--cyan)}.xor-modes{display:flex;width:100%}.xor-modes .hx-segb{flex:1;min-width:0}.controls .field{flex:1;min-height:0;grid-template-rows:auto minmax(0,1fr);align-content:start}.controls .key-field{grid-template-rows:auto auto minmax(0,1fr)}.field textarea{width:100%;height:100%;min-height:0}.controls p{margin:0;color:var(--dim2);font-size:var(--fs-small);line-height:1.6}.go{height:32px;margin-top:auto;border:1px solid var(--cyan);background:rgb(var(--cyan-rgb)/10%);color:var(--cyan);cursor:pointer;font:var(--fs-small) var(--share);letter-spacing:.12em}.go:disabled,.op:disabled{opacity:.45;cursor:default}.op{min-width:26px;height:24px;border:1px solid var(--border);background:transparent;color:var(--gray);cursor:pointer}.empty,.error{flex:1;display:flex;align-items:center;justify-content:center;padding:20px;text-align:center;color:var(--muted);font-size:var(--fs-body)}.error{color:var(--danger)}
</style>
