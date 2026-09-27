<script setup lang="ts">
/*
  解码器页（跨注入 / 代理两种模式共用）。

  一个页面同时承担「管理」与「测试」：左侧列表（增删改 / 启停），中间测试台，右侧结果。
  「快速编解码」页签沿用原来的 Transcode 组件（一次性临时算法，不落库）。

  算法与帧解析都在 C# 侧（CodecEngine / FrameExtractor）；这里只负责界面与桥调用。
*/
import { computed, onMounted, ref } from 'vue'
import { call } from '../../bridge'
import type { CodecResult, DecoderRow } from '../../bridge/types'
import { t } from '../../i18n'
import { pushToast } from '../../stores/toast'
import QuickCodec from '../proxy/Transcode.vue'
import DecoderEdit from './DecoderEdit.vue'
import SmartResult from './SmartResult.vue'
import { DecKind } from './enums'
import type { SmartPayload } from './actions'
import {
  decApplyFrame, decError, decInput, decMode, decOutput,
  decRows, decSelected, decSelectedId,
} from '../../stores/decoder'

const busy = ref(false)
const editTarget = ref<DecoderRow | null>(null)
const smartPayload = ref<SmartPayload | null>(null)
/** 快速编解码工作台的实例：顶栏的「清空」调用它暴露的 clearAll。 */
const quickRef = ref<{ clearAll: () => void } | null>(null)
function clearQuick(): void { quickRef.value?.clearAll() }

onMounted(load)

async function load(): Promise<void> {
  try {
    const r = await call<{ rows: DecoderRow[] }>('getDecoders')
    decRows.value = r?.rows ?? []
    if (!decRows.value.length) {
      //空列表先落在仍可立即使用的临时工作台，避免中间只剩“先选一个解码器”。
      decSelectedId.value = ''
      decMode.value = 'quick'
    } else if (decMode.value === 'decoder' && (!decSelectedId.value || !decRows.value.some((x) => x.Id === decSelectedId.value))) {
      decSelectedId.value = decRows.value[0]?.Id ?? ''
    }
  } catch (e) {
    console.error('[dec] 读取解码器失败', e)
  }
}

function select(id: string): void {
  decMode.value = 'decoder'
  decSelectedId.value = id
}

function selectQuick(): void {
  decMode.value = 'quick'
  decSelectedId.value = ''
}

function kindLabel(kind: number): string {
  switch (kind) {
    case DecKind.Aes: return 'AES'
    case DecKind.Des: return 'DES'
    case DecKind.Protobuf: return 'Protobuf'
    case DecKind.MessagePack: return 'MessagePack'
    case DecKind.Rc4: return 'RC4'
    case DecKind.Xxtea: return 'XXTEA'
    case DecKind.Amf: return 'AMF'
    case DecKind.Bson: return 'BSON'
    case DecKind.FlatBuffers: return 'FlatBuffers'
    case DecKind.TextCharset: return t('dec.kindText')
    default: return 'XOR'
  }
}

async function add(): Promise<void> {
  try {
    const r = await call<{ id: string }>('addDecoder')
    await load()
    if (r?.id) {
      decSelectedId.value = r.id
      editTarget.value = decRows.value.find((x) => x.Id === r.id) ?? null
    }
  } catch (e) { console.error('[dec] 新增失败', e) }
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

async function toggle(row: DecoderRow): Promise<void> {
  try {
    await call('setDecoderEnable', { id: row.Id, enable: !row.IsEnable })
    await load()
  } catch (e) { console.error('[dec] 启停失败', e) }
}

async function remove(row: DecoderRow): Promise<void> {
  try {
    await call('deleteDecoders', { ids: [row.Id] })
    pushToast('success', t('dec.deleted'))
    if (decSelectedId.value === row.Id) { decSelectedId.value = '' }
    await load()
  } catch (e) { console.error('[dec] 删除失败', e) }
}

/* ── 源数据 / 结果 ─────────────────────────────────────── */

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

const modelSummary = computed(() => {
  const d = selected.value
  if (!d) return ''
  const parts: string[] = [kindLabel(d.Kind)]
  if (d.LengthBytes) parts.push(d.LengthBytes + 'B ' + (d.BigEndian ? 'BE' : 'LE'))
  if (d.HasFixedHeader && d.FixedHeader) parts.push('HDR ' + d.FixedHeader)
  if (d.DataOffset) parts.push('+' + d.DataOffset)
  return parts.join(' · ')
})

async function run(encode: boolean): Promise<void> {
  const d = selected.value
  if (!d) { decError.value = t('dec.pickFirst'); return }
  const source = encode ? decInput.value : decOutput.value
  if (!source.trim()) { decError.value = t('tr.errDecode'); return }

  decError.value = ''
  // 只有编码才把结果区当输出区清空；解码时结果区是输入（密文），要留着。
  if (encode) decOutput.value = ''
  busy.value = true
  try {
    const bytes = encode ? new TextEncoder().encode(decInput.value) : hexToBytes(decOutput.value)
    const r = await call<CodecResult>('testDecoder', { data: bytesToB64(bytes), decoder: d, encode, applyFrame: decApplyFrame.value })
    if (!r?.Ok) { decError.value = r?.Error ?? t('tr.errDecode'); return }
    if (encode) decOutput.value = r.OutputBase64 ? bytesToHex(b64ToBytes(r.OutputBase64)) : ''
    else decInput.value = r.Text ?? ''
  } catch (e) {
    decError.value = String(e)
  } finally {
    busy.value = false
  }
}

/** 对结果区的密文做智能解码（不依赖是否已选解码器）。 */
async function runSmart(): Promise<void> {
  const src = decOutput.value.trim()
  if (!src) { decError.value = t('tr.errDecode'); return }
  decError.value = ''
  try {
    const r = await call<{ hits: SmartPayload['hits'] }>('smartDecode', { data: bytesToB64(hexToBytes(src)) })
    smartPayload.value = { hits: r?.hits ?? [] }
  } catch (e) {
    decError.value = String(e)
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
      <span class="lb">{{ t('dec.explain') }}</span>
      <button v-if="decMode === 'quick'" class="btn danger" @click="clearQuick">{{ t('rb.clearAll') }}</button>
    </div>

    <div class="mainwork" :class="{ quickmode: decMode === 'quick' }">
      <!-- 左：解码器列表（管理） -->
      <section class="pane list">
        <div class="ph"><span class="tt">{{ t('dec.tabDecoders') }}</span><span class="fill" /><button class="op add" :title="t('dec.add')" @click="add">＋</button></div>
        <div class="rows">
          <div class="r quick" :class="{ on: decMode === 'quick' }" @click="selectQuick">
            <span class="nm">{{ t('dec.tabQuick') }}</span><span class="kd">TEMP</span>
          </div>
          <div v-if="!decRows.length" class="empty">{{ t('dec.empty') }}</div>
          <div v-for="d in decRows" v-else :key="d.Id" class="r" :class="{ on: decMode === 'decoder' && d.Id === decSelectedId, off: !d.IsEnable }" @click="select(d.Id)" @dblclick="editTarget = d">
            <button class="chk" :class="{ on: d.IsEnable }" :title="t('dec.enable')" @click.stop="toggle(d)"><i /></button>
            <span class="nm" :title="d.Name">{{ d.Name }}</span>
            <span class="kd">{{ kindLabel(d.Kind) }}</span>
            <button class="op" :title="t('dec.edit')" @click.stop="editTarget = d"><svg viewBox="0 0 24 24"><path d="M4 20h4L20 8l-4-4L4 16z" /></svg></button>
            <button class="op del" :title="t('dec.del')" @click.stop="remove(d)"><svg viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg></button>
          </div>
        </div>
      </section>

      <QuickCodec v-if="decMode === 'quick'" ref="quickRef" embedded @save-as-decoder="saveQuickAsDecoder" />

      <!-- 左中：原文 / 输入 -->
      <section v-if="decMode === 'decoder'" class="pane">
        <div class="ph"><span class="tt">{{ t('tr.inputPane') }}</span><span class="meta">{{ decInput.length }} {{ t('tr.chars') }}</span><span class="fill" /><button class="op" :disabled="!decInput" :title="t('tr.copyOut')" @click="copyIn">⧉</button></div>
        <textarea v-model="decInput" spellcheck="false" :placeholder="t('tr.inPhEnc')" />
      </section>

      <!-- 中：参数与按钮 -->
      <section v-if="decMode === 'decoder'" class="control">
        <div class="ph"><span class="tt">{{ selected ? selected.Name : t('dec.tabDecoders') }}</span></div>
        <div class="controls">
          <div class="meta">{{ modelSummary || t('dec.pickFirst') }}</div>
          <button class="chk wide" :class="{ on: decApplyFrame }" @click="decApplyFrame = !decApplyFrame"><i />{{ t('dec.applyFrame') }}</button>
          <div class="btns">
            <button class="go" :disabled="busy || !selected" @click="run(false)">{{ t('tr.decode') }} →</button>
            <button class="go alt" :disabled="busy || !selected" @click="run(true)">← {{ t('tr.encode') }}</button>
            <button class="go smart" :disabled="busy || !decOutput.trim()" @click="runSmart">{{ t('dec.smart') }}</button>
          </div>
        </div>
      </section>

      <!-- 右：结果 -->
      <section v-if="decMode === 'decoder'" class="pane out">
        <div class="ph"><span class="tt">{{ t('tr.outPane') }}</span><span class="meta">{{ decOutput.length }} {{ t('tr.chars') }}</span><span class="fill" /><button class="op" :disabled="!decOutput" :title="t('tr.copyOut')" @click="copyOut">⧉</button></div>
        <div v-if="decError" class="error">{{ decError }}</div>
        <textarea v-model="decOutput" spellcheck="false" :placeholder="t('tr.outPh')" />
      </section>
    </div>

    <DecoderEdit :target="editTarget" @close="editTarget = null" @saved="load" />
    <SmartResult :payload="smartPayload" @close="smartPayload = null" />
  </div>
</template>

<style scoped>
.dec{flex:1;min-width:0;min-height:0;display:flex;flex-direction:column;gap:8px;padding:10px 12px 12px}
/* 顶栏高度固定：清空按钮只在快速编解码页签显示，不固定高度会随按钮显隐抖一下。46px = 按钮行的自然高度，也与其它页面的工具条一致。 */
.dec .bar{height:46px}
.lb{flex:1;min-width:0;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;color:var(--dim2);font-size:var(--fs-small)}
.mainwork{flex:1;min-height:0;display:grid;grid-template-columns:260px minmax(0,1fr) 250px minmax(0,1fr);gap:8px}
.mainwork.quickmode{grid-template-columns:260px minmax(0,1fr)}
.pane,.control{min-width:0;min-height:0;display:flex;flex-direction:column;border:1px solid var(--border);background:var(--sink)}
.ph{flex:none;display:flex;align-items:center;gap:10px;height:var(--th-h);padding:0 12px;background:var(--panel);border-bottom:1px solid var(--border);font-family:var(--share);font-size:var(--th-size);letter-spacing:.12em;text-transform:uppercase;color:var(--th-fg)}
.tt{color:var(--cyan);overflow:hidden;text-overflow:ellipsis;white-space:nowrap}
.fill{flex:1}
.pane textarea{flex:1;min-height:0;margin:0;padding:10px 12px;resize:none;border:0;outline:0;background:transparent;color:var(--gray);caret-color:var(--cyan);font:var(--fs-body)/1.6 var(--mono);white-space:pre-wrap;overflow:auto;overflow-wrap:anywhere}
.pane .meta{flex:none;color:var(--muted);font:var(--fs-small) var(--mono);min-width:0;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;flex-shrink:100}
.out textarea{color:var(--acc-green2)}
.rows{flex:1;min-height:0;overflow:auto}
.empty{padding:16px 12px;color:var(--muted);font-size:var(--fs-small)}
.r{display:flex;align-items:center;gap:8px;padding:6px 10px;cursor:pointer;border-bottom:1px solid var(--border)}
.r:hover{background:rgb(var(--cyan-rgb) / 5%)}
.r.on{background:rgb(var(--cyan-rgb) / 9%);box-shadow:inset 2px 0 0 var(--cyan)}
.r.off{opacity:.5}
.r.quick{background:rgb(var(--amber-rgb) / 5%);border-bottom:1px solid rgb(var(--amber-rgb) / 24%)}
.r.quick .kd{color:var(--amber)}
.nm{flex:1;min-width:0;overflow:hidden;text-overflow:ellipsis;white-space:nowrap;font-size:var(--fs-body);color:var(--gray)}
.kd{flex:none;font-family:var(--mono);font-size:var(--fs-small);color:var(--dim2)}
.op{flex:none;width:24px;height:24px;display:flex;align-items:center;justify-content:center;border:0;background:transparent;color:var(--muted);cursor:pointer}
.op:hover:not(:disabled){color:var(--cyan)}
.op:disabled{opacity:.35;cursor:default}
.op.del:hover{color:var(--danger)}
.op svg{width:14px;height:14px;stroke:currentColor;stroke-width:1.8;fill:none}
.op.add{font-size:var(--fs-title);color:var(--acc-green2)}
.controls{flex:1;min-height:0;display:flex;flex-direction:column;gap:12px;padding:12px;overflow:auto}
.controls .meta{color:var(--dim2);font:var(--fs-small) var(--mono);min-height:16px}
.chk.wide{justify-content:flex-start}
.btns{display:flex;flex-direction:column;gap:8px;margin-top:auto}
.go{min-height:32px;padding:3px 8px;box-sizing:border-box;line-height:1.25;white-space:normal;overflow-wrap:break-word;border:1px solid rgb(var(--cyan-rgb) / 45%);background:transparent;color:var(--cyan);cursor:pointer;font-family:var(--share);letter-spacing:.1em}
.go:hover:not(:disabled){background:rgb(var(--cyan-rgb) / 8%)}
.go:disabled{opacity:.4;cursor:default}
.go.alt{border-color:rgb(var(--green-rgb) / 45%);color:var(--green)}
.go.smart{border-color:var(--border);color:var(--amber)}
.out .error{padding:8px 12px;border-bottom:1px solid var(--border);color:var(--danger);font:var(--fs-small) var(--mono);white-space:pre-wrap;overflow-wrap:anywhere}
</style>
