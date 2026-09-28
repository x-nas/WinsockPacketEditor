<script setup lang="ts">
/*
  智能解码（工作台）—— 跨注入 / 代理两种模式共用。

  解码器的<b>管理</b>已经单独成页（DecoderList.vue，侧栏与滤镜 / 发送 / 机器人 / 仓库并列）；
  这一屏只留工作台：中间栏顶部选解码器，左边输入 / 中间参数 / 右边结果，右上「智能解码」。

  选择器里第一项是「快速编解码」（一次性临时算法，不落库），其余是已保存的解码器。
  选到快速编解码时，整块工作台换成 Transcode 组件（它自带算法与参数）——
  选择器经 Transcode 的 #extra 插槽放进它的中间栏（否则选到快速编解码后就切不回来了）；
  选到某个解码器时显示测试台，参数来自解码器、可用「按帧配置」开关。

  ⚠️ 中间栏宽度固定 250px，且与 Transcode.vue 的 .workbench 一致 —— 两种来源之间切换时
  中间栏不能一宽一窄。

  算法与帧解析都在 C# 侧（CodecEngine / FrameExtractor）；这里只负责界面与桥调用。
*/
import { computed, onMounted, ref } from 'vue'
import { call } from '../../bridge'
import type { CodecResult, DecoderRow } from '../../bridge/types'
import { t } from '../../i18n'
import { pushToast } from '../../stores/toast'
import CyberSelect from '../CyberSelect.vue'
import QuickCodec from '../proxy/Transcode.vue'
import DecoderEdit from './DecoderEdit.vue'
import SmartResult from './SmartResult.vue'
import { DecKind, kindLabel } from './enums'
import { ensureDecoders, type SmartPayload } from './actions'
import {
  decApplyFrame, decError, decInput, decMode, decOutput,
  decRows, decSelected, decSelectedId, decTransformDirection,
} from '../../stores/decoder'

const busy = ref(false)
const editTarget = ref<DecoderRow | null>(null)
const smartPayload = ref<SmartPayload | null>(null)
const inputFormat = ref<'hex' | 'base64' | 'text'>('hex')
/** 快速编解码工作台的实例：顶栏的「清空」调用它暴露的 clearAll。 */
const quickRef = ref<{ clearAll: () => void } | null>(null)
function clearQuick(): void { quickRef.value?.clearAll() }
function clearWorkbench(): void {
  if (decMode.value === 'quick') { clearQuick(); return }
  decInput.value = ''
  decOutput.value = ''
  decError.value = ''
}

onMounted(load)

async function load(): Promise<void> {
  await ensureDecoders(true)

  if (!decRows.value.length) {
    //没有已保存的解码器，只能落在临时工作台
    decSelectedId.value = ''
    decMode.value = 'quick'
  } else if (decMode.value === 'decoder' && (!decSelectedId.value || !decRows.value.some((x) => x.Id === decSelectedId.value))) {
    decSelectedId.value = decRows.value[0]?.Id ?? ''
  }
}

/* ── 解码器选择器 ─────────────────────────────────────────── */

/** 选项：快速编解码 + 所有已保存的解码器（停用的也能选中试用）。 */
const pickerOptions = computed(() => [
  { value: 'quick', label: t('dec.tabQuick') },
  ...decRows.value.map((d) => ({ value: d.Id, label: d.Name })),
])

const pickId = computed<string>({
  get: () => (decMode.value === 'quick' ? 'quick' : (decSelectedId.value || 'quick')),
  set: (v) => {
    if (v === 'quick') { decMode.value = 'quick'; decSelectedId.value = '' }
    else { decMode.value = 'decoder'; decSelectedId.value = v }
  },
})

/* ── 源数据 / 结果 ─────────────────────────────────────────── */

function hexToBytes(hex: string): Uint8Array {
  const s = hex.replace(/[\s-]/g, '')
  if (!s || (s.length & 1) || !/^[0-9a-f]+$/i.test(s)) throw new Error(t('tr.errOddHex'))
  const out = new Uint8Array(s.length / 2)
  for (let i = 0; i < out.length; i++) out[i] = parseInt(s.slice(i * 2, i * 2 + 2), 16)
  return out
}
function bytesToHex(bytes: Uint8Array): string {
  return Array.from(bytes, (b) => b.toString(16).padStart(2, '0').toUpperCase()).join(' ')
}
function b64ToBytes(b64: string): Uint8Array {
  const bin = atob(b64)
  const out = new Uint8Array(bin.length)
  for (let i = 0; i < bin.length; i++) out[i] = bin.charCodeAt(i)
  return out
}
function bytesToB64(bytes: Uint8Array): string {
  let s = ''
  for (let i = 0; i < bytes.length; i++) s += String.fromCharCode(bytes[i])
  return btoa(s)
}

const selected = computed(() => decSelected())
const isEncode = computed(() => decTransformDirection.value === 'encode')
const inputFormatLabel = computed(() => inputFormat.value === 'hex' ? t('dec.fmtHex') : inputFormat.value === 'base64' ? 'Base64' : t('dec.fmtText'))
const inputTitle = computed(() => (isEncode.value ? t('dec.plainText') : t('dec.cipherHex')) + '（' + inputFormatLabel.value + '）')
const outputTitle = computed(() => isEncode.value ? t('dec.encodedResult') : t('dec.decodedResult'))
const inputPlaceholder = computed(() => inputFormat.value === 'hex' ? t('dec.cipherHint') : inputFormat.value === 'base64' ? t('dec.base64Hint') : t('tr.inPhEnc'))
const outputPlaceholder = computed(() => isEncode.value ? t('dec.encodedHint') : t('dec.decodedHint'))
const smartDisabled = computed(() => decMode.value !== 'decoder' || busy.value || isEncode.value || !decInput.value.trim())

/** 切换方向时交换内容：上一方向的结果正好是下一方向的输入，可直接往返验证。 */
function setTransformDirection(next: 'decode' | 'encode'): void {
  if (decTransformDirection.value === next) return
  const current = decInput.value
  decInput.value = decOutput.value
  decOutput.value = current
  decTransformDirection.value = next
  decError.value = ''
}

function inputToBytes(): Uint8Array {
  if (inputFormat.value === 'hex') return hexToBytes(decInput.value)
  if (inputFormat.value === 'base64') return b64ToBytes(decInput.value.trim())
  return new TextEncoder().encode(decInput.value)
}

const modelSummary = computed(() => {
  const d = selected.value
  if (!d) return ''
  const parts: string[] = [kindLabel(d.Kind)]
  if (d.LengthBytes) parts.push(d.LengthBytes + 'B ' + (d.BigEndian ? 'BE' : 'LE'))
  if (d.HasFixedHeader && d.FixedHeader) parts.push('HDR ' + d.FixedHeader)
  if (d.DataOffset) parts.push('+' + d.DataOffset)
  return parts.join(' · ')
})

async function run(): Promise<void> {
  const d = selected.value
  if (!d) { decError.value = t('dec.pickFirst'); return }
  const encode = isEncode.value
  const source = decInput.value
  if (!source.trim()) { decError.value = t('tr.errDecode'); return }

  decError.value = ''
  decOutput.value = ''
  busy.value = true
  try {
    const bytes = inputToBytes()
    const r = await call<CodecResult>('testDecoder', { data: bytesToB64(bytes), decoder: d, encode, applyFrame: decApplyFrame.value })
    if (!r?.Ok) { decError.value = r?.Error ?? t('tr.errDecode'); return }
    decOutput.value = encode
      ? (r.OutputBase64 ? bytesToHex(b64ToBytes(r.OutputBase64)) : '')
      : (r.Text ?? '')
  } catch (e) {
    decError.value = String(e)
  } finally {
    busy.value = false
  }
}

/** 对结果区的密文做智能解码（不依赖是否已选解码器）。 */
async function runSmart(): Promise<void> {
  const src = decInput.value.trim()
  if (!src) { decError.value = t('tr.errDecode'); return }
  decError.value = ''
  busy.value = true
  try {
    const r = await call<{ hits: SmartPayload['hits'] }>('smartDecode', { data: bytesToB64(inputToBytes()) })
    smartPayload.value = { hits: r?.hits ?? [] }
    if (!smartPayload.value.hits.length) pushToast('info', t('dec.smartNone'))
  } catch (e) {
    decError.value = String(e)
  } finally {
    busy.value = false
  }
}

type QuickDecoderDraft = { kind: 'xor' | 'aes'; key: string; keyFormat: 'hex' | 'text'; iv: string }
function newDecoderDraft(): DecoderRow {
  return {
    Id: '', IsEnable: true, Name: '', Description: '',
    Kind: DecKind.Xor, Charset: 3, KeyFormat: 0, Key: '', IvFormat: 0, Iv: '',
    CipherMode: 0, Padding: 1, BlockSize: 0,
    LengthBytes: 0, BigEndian: false, LengthIncludesSelf: false,
    HasFixedHeader: false, FixedHeader: '', LengthIncludesFixedHeader: false, DataOffset: 0,
    ProtocolType: 0, Direction: 0, ParamsJson: '',
  }
}
/** 快速编解码里「保存为解码器」：把参数带进编辑弹窗。 */
function saveQuickAsDecoder(draft: QuickDecoderDraft): void {
  editTarget.value = {
    ...newDecoderDraft(),
    Name: draft.kind === 'aes' ? 'AES' : 'XOR',
    Kind: draft.kind === 'aes' ? DecKind.Aes : DecKind.Xor,
    KeyFormat: draft.keyFormat === 'text' ? 2 : 0,
    Key: draft.key,
    IvFormat: 0,
    Iv: draft.iv,
  }
}

async function copyOut(): Promise<void> {
  if (!decOutput.value) return
  try { await call('clipboardWrite', { text: decOutput.value }); pushToast('success', t('pm.copied')) }
  catch (e) { console.error('[dec] copy failed', e) }
}
async function copyIn(): Promise<void> {
  if (!decInput.value) return
  try { await call('clipboardWrite', { text: decInput.value }); pushToast('success', t('pm.copied')) }
  catch (e) { console.error('[dec] copy failed', e) }
}
</script>

<template>
  <div class="page list-page dec">
    <div class="bar">
      <button class="btn warn" :disabled="smartDisabled" @click="runSmart">{{ busy ? t('proxy.working') : t('dec.smart') }}</button>
      <span class="lb">{{ t('dec.explain') }}</span>
      <button class="btn danger" @click="clearWorkbench">{{ t('rb.clearAll') }}</button>
    </div>

    <div class="mainwork" :class="{ quickmode: decMode === 'quick' }">
      <!-- 快速编解码：自带算法与参数，占满整块工作台；选择器经 #extra 插进它的中间栏 -->
      <QuickCodec v-if="decMode === 'quick'" ref="quickRef" embedded @save-as-decoder="saveQuickAsDecoder">
        <template #extra>
          <div class="pickf"><span>{{ t('dec.selectDecoder') }}</span><CyberSelect v-model="pickId" :options="pickerOptions" /></div>
        </template>
      </QuickCodec>

      <!-- 左：当前方向的输入 -->
      <section v-if="decMode === 'decoder'" class="pane">
        <div class="ph"><span class="tt">{{ inputTitle }}</span><span class="meta">{{ decInput.length }} {{ t('tr.chars') }}</span><span class="fill" /><button class="op" :disabled="!decInput" :title="t('tr.copyOut')" @click="copyIn">⧉</button></div>
        <textarea v-model="decInput" spellcheck="false" :placeholder="inputPlaceholder" />
      </section>

      <!-- 中：参数与按钮 -->
      <section v-if="decMode === 'decoder'" class="control">
        <div class="ph"><span class="tt">{{ selected ? selected.Name : t('dec.selectDecoder') }}</span></div>
        <div class="controls">
          <div class="pickf"><span>{{ t('dec.selectDecoder') }}</span><CyberSelect v-model="pickId" :options="pickerOptions" /></div>
          <div class="mode-switch" role="group" :aria-label="t('dec.direction')">
            <button class="mode-btn decode" :class="{ on: !isEncode }" @click="setTransformDirection('decode')">{{ t('tr.decode') }}</button>
            <button class="mode-btn encode" :class="{ on: isEncode }" @click="setTransformDirection('encode')">{{ t('tr.encode') }}</button>
          </div>
          <div class="meta">{{ modelSummary || t('dec.pickFirst') }}</div>
          <div class="source-format"><span>{{ t('dec.sourceFormat') }}</span><div class="input-format-switch" role="group" :aria-label="t('dec.sourceFormat')"><button :class="{ on: inputFormat === 'hex' }" @click="inputFormat = 'hex'">{{ t('dec.fmtHex') }}</button><button :class="{ on: inputFormat === 'base64' }" @click="inputFormat = 'base64'">Base64</button><button :class="{ on: inputFormat === 'text' }" @click="inputFormat = 'text'">{{ t('dec.fmtText') }}</button></div></div>
          <button class="chk wide" :class="{ on: decApplyFrame }" @click="decApplyFrame = !decApplyFrame"><i />{{ t('dec.applyFrame') }}</button>
          <div class="btns">
            <button class="go" :class="{ alt: isEncode }" :disabled="busy || !selected || !decInput.trim()" @click="run">{{ isEncode ? t('tr.encode') : t('tr.decode') }} →</button>
          </div>
        </div>
      </section>

      <!-- 右：结果 -->
      <section v-if="decMode === 'decoder'" class="pane out">
        <div class="ph"><span class="tt">{{ outputTitle }}</span><span class="meta">{{ decOutput.length }} {{ t('tr.chars') }}</span><span class="fill" /><button class="op" :disabled="!decOutput" :title="t('tr.copyOut')" @click="copyOut">⧉</button></div>
        <div v-if="decError" class="error">{{ decError }}</div>
        <textarea v-model="decOutput" spellcheck="false" :placeholder="outputPlaceholder" />
      </section>
    </div>

    <!-- 快速编解码的「保存为解码器」走这里（管理已在解码器列表页） -->
    <DecoderEdit :target="editTarget" @close="editTarget = null" @saved="load" />
    <SmartResult :payload="smartPayload" @close="smartPayload = null" />
  </div>
</template>

<style scoped>
.dec{flex:1;min-width:0;min-height:0;display:flex;flex-direction:column;gap:8px;padding:10px 12px 12px}
/* 顶栏高度固定：清空按钮只在快速编解码页签显示，不固定高度会随按钮显隐抖一下。46px = 按钮行的自然高度，也与其它页面的工具条一致。 */
.dec .bar{height:46px}
.lb{flex:1;min-width:0;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;color:var(--dim2);font-size:var(--fs-small)}
/* 解码器选择器：中间栏顶部，与源格式那类控件同一种「标签在上、控件在下」的排法 */
.pickf{display:grid;gap:5px;color:var(--muted);font:var(--fs-small) var(--mono)}
/*
  中间栏宽度固定 250px：快速编解码（Transcode.vue 的 .workbench）用的就是 250px，
  这里必须跟着一样，否则在两种来源之间切换时中间栏会一宽一窄跳一下。
*/
.mainwork{flex:1;min-height:0;display:grid;grid-template-columns:minmax(0,1fr) 250px minmax(0,1fr);gap:8px}
.mainwork.quickmode{grid-template-columns:minmax(0,1fr)}
.pane,.control{min-width:0;min-height:0;display:flex;flex-direction:column;border:1px solid var(--border);background:var(--sink)}
.ph{flex:none;display:flex;align-items:center;gap:10px;height:var(--th-h);padding:0 12px;background:var(--panel);border-bottom:1px solid var(--border);font-family:var(--share);font-size:var(--th-size);letter-spacing:.12em;text-transform:uppercase;color:var(--th-fg)}
.tt{color:var(--cyan);overflow:hidden;text-overflow:ellipsis;white-space:nowrap}
.fill{flex:1}
.pane textarea{flex:1;min-height:0;margin:0;padding:10px 12px;resize:none;border:0;outline:0;background:transparent;color:var(--gray);caret-color:var(--cyan);font:var(--fs-body)/1.6 var(--mono);white-space:pre-wrap;overflow:auto;overflow-wrap:anywhere}
.pane .meta{flex:none;color:var(--muted);font:var(--fs-small) var(--mono);min-width:0;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;flex-shrink:100}
.out textarea{color:var(--acc-green2)}
.controls{flex:1;min-height:0;display:flex;flex-direction:column;gap:12px;padding:12px;overflow:auto}
.controls .meta{color:var(--dim2);font:var(--fs-small) var(--mono);min-height:16px}
.mode-switch{display:flex;border:1px solid var(--border)}
.mode-btn{flex:1;min-height:30px;border:0;background:transparent;color:var(--dim);cursor:pointer;font-family:var(--share);font-size:var(--fs-small);letter-spacing:.1em}
.mode-btn + .mode-btn{border-left:1px solid var(--border)}
.mode-btn.decode.on{background:rgb(var(--cyan-rgb) / 12%);color:var(--cyan)}
.mode-btn.encode.on{background:rgb(var(--green-rgb) / 12%);color:var(--green)}
.mode-btn:hover:not(.on){color:var(--gray);background:rgb(var(--chrome-rgb) / 18%)}
.source-format{display:grid;gap:5px;color:var(--muted);font:var(--fs-small) var(--mono)}
.input-format-switch{display:flex;border:1px solid var(--border)}
.input-format-switch button{flex:1;min-width:0;height:28px;padding:0 4px;border:0;background:transparent;color:var(--dim);cursor:pointer;font:var(--fs-small) var(--mono);white-space:nowrap}
.input-format-switch button + button{border-left:1px solid var(--border)}
.input-format-switch button.on{background:rgb(var(--cyan-rgb) / 12%);color:var(--cyan)}
.input-format-switch button:hover:not(.on){color:var(--gray);background:rgb(var(--chrome-rgb) / 18%)}
.chk.wide{justify-content:flex-start}
.btns{display:flex;flex-direction:column;gap:8px;margin-top:auto}
.go{min-height:32px;padding:3px 8px;box-sizing:border-box;line-height:1.25;white-space:normal;overflow-wrap:break-word;border:1px solid rgb(var(--cyan-rgb) / 45%);background:transparent;color:var(--cyan);cursor:pointer;font-family:var(--share);letter-spacing:.1em}
.go:hover:not(:disabled){background:rgb(var(--cyan-rgb) / 8%)}
.go:disabled{opacity:.4;cursor:default}
.go.alt{border-color:rgb(var(--green-rgb) / 45%);color:var(--green)}
.go.smart{border-color:var(--border);color:var(--amber)}
.out .error{padding:8px 12px;border-bottom:1px solid var(--border);color:var(--danger);font:var(--fs-small) var(--mono);white-space:pre-wrap;overflow-wrap:anywhere}
</style>
