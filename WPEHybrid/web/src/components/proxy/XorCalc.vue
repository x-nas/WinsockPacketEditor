<script setup lang="ts">
/*
  异或计算 —— 对应 WinForms 的 Controls/XORCalculation（两块 HexBox + 一个异或值输入）。

  纯前端：字节在浏览器里异或，一个桥方法都不用。两块十六进制区域是共用的 HexView
  （左边可编辑，右边只读），异或值在工具条上，支持循环 —— 值比数据短就从头再来。

  ⚠️ WinForms 那边的循环异或写错了：结果写到 blXOR_To[j]（j 是异或值的下标，不是字节的下标），
  数据一超过异或值的长度，结果就只剩最后一轮的那几个字节。这里按正确的算法写：out[i] = src[i] ^ key[i % keyLen]。
*/
import { computed, ref } from 'vue'
import { call } from '../../bridge'
import { t } from '../../i18n'
import { pushToast } from '../../stores/toast'
import { xorSrc, xorOut, xorKey } from '../../stores/tools'
import HexView from '../HexView.vue'

const keyBad = ref(false)

/** 异或值：空格分隔的十六进制；没有空格但整串都是十六进制也认（按两位一拆）。 */
function parseKey(): Uint8Array | null {
  const s = xorKey.value.trim()
  if (!s) return null

  let tokens: string[]
  if (/\s/.test(s)) tokens = s.split(/\s+/)
  else {
    if (!/^[0-9a-fA-F]+$/.test(s) || s.length % 2) return null
    tokens = s.match(/../g) ?? []
  }

  const out = new Uint8Array(tokens.length)
  for (let i = 0; i < tokens.length; i++) {
    if (!/^[0-9a-fA-F]{1,2}$/.test(tokens[i])) return null
    out[i] = parseInt(tokens[i], 16)
  }
  return out
}

const keyBytes = computed(() => parseKey())

function run(): void {
  const src = xorSrc.value
  if (!src.length) { pushToast('error', t('xo.srcEmpty')); return }

  const key = parseKey()
  if (!key) {
    keyBad.value = true
    pushToast('error', t(xorKey.value.trim() ? 'xo.keyBad' : 'xo.keyEmpty'))
    return
  }
  keyBad.value = false

  const out = new Uint8Array(src.length)
  for (let i = 0; i < src.length; i++) out[i] = src[i] ^ key[i % key.length]
  xorOut.value = out
}

function clearAll(): void {
  xorSrc.value = new Uint8Array(0)
  xorOut.value = new Uint8Array(0)
  xorKey.value = ''
  keyBad.value = false
}

function hexOf(b: Uint8Array): string {
  let s = ''
  for (let i = 0; i < b.length; i++) s += (i ? ' ' : '') + b[i].toString(16).toUpperCase().padStart(2, '0')
  return s
}

/** 从剪贴板读一串十六进制填进左边（右键菜单里也有粘贴，这里是给不想右键的人一个按钮） */
async function pasteHex(): Promise<void> {
  try {
    const r = await call<{ text: string }>('clipboardRead')
    const txt = (r?.text ?? '').replace(/[^0-9a-fA-F]/g, '')
    if (!txt) { pushToast('warning', t('xo.clipNoHex')); return }
    const pairs = txt.length % 2 ? txt.slice(0, -1) : txt
    xorSrc.value = new Uint8Array((pairs.match(/../g) ?? []).map((h) => parseInt(h, 16)))
  } catch (e) {
    console.error('[xo] 读剪贴板失败', e)
  }
}

async function copyOut(): Promise<void> {
  if (!xorOut.value.length) return
  try {
    await call('clipboardWrite', { text: hexOf(xorOut.value) })
    pushToast('success', t('pm.copied'))
  } catch (e) {
    console.error('[xo] 写剪贴板失败', e)
  }
}

/** 结果搬到左边再算一次 —— 连续异或时省得复制粘贴 */
function useOut(): void {
  if (!xorOut.value.length) return
  xorSrc.value = xorOut.value
  xorOut.value = new Uint8Array(0)
}
</script>

<template>
  <div class="page list-page xo">
    <div class="bar">
      <span class="lb">{{ t('xo.key') }}</span>
      <input v-model="xorKey" class="inp key" :class="{ bad: keyBad && !keyBytes }" spellcheck="false"
             :placeholder="t('xo.keyPh')" @keydown.enter="run">
      <span class="tg" :class="keyBytes ? 'ok' : 'dim'">{{ keyBytes ? keyBytes.length + ' B' : '—' }}</span>
      <button class="btn primary" :disabled="!xorSrc.length" @click="run">{{ t('xo.run') }}</button>

      <span class="grow" />

      <button class="btn" @click="pasteHex">{{ t('xo.pasteHex') }}</button>
      <button class="btn" :disabled="!xorOut.length" @click="copyOut">{{ t('xo.copyOut') }}</button>
      <button class="btn warn" :disabled="!xorOut.length" @click="useOut">{{ t('xo.useOut') }}</button>
      <button class="btn danger" :disabled="!xorSrc.length && !xorOut.length && !xorKey" @click="clearAll">{{ t('rb.clearAll') }}</button>
    </div>

    <div class="panes">
      <div class="hexed">
        <div class="hx-bar">
          <span class="hx-title">{{ t('xo.src') }}</span>
          <span class="hx-meta">{{ xorSrc.length }} {{ t('hex.bytes') }}</span>
          <span class="hx-meta dim">{{ t('xo.srcHint') }}</span>
        </div>
        <div v-if="!xorSrc.length" class="empty">{{ t('xo.emptySrc') }}</div>
        <HexView v-else v-model:bytes="xorSrc" />
      </div>

      <div class="arrow" aria-hidden="true">
        <svg viewBox="0 0 24 24"><path d="M4 12h14M13 6l6 6-6 6" /></svg>
        <span>XOR</span>
      </div>

      <div class="hexed">
        <div class="hx-bar">
          <span class="hx-title">{{ t('xo.out') }}</span>
          <span class="hx-meta">{{ xorOut.length }} {{ t('hex.bytes') }}</span>
        </div>
        <div v-if="!xorOut.length" class="empty">{{ t('xo.emptyOut') }}</div>
        <HexView v-else :bytes="xorOut" readonly />
      </div>
    </div>
  </div>
</template>

<style scoped>
.page {
  flex: 1;
  min-width: 0;
  min-height: 0;
  display: flex;
  flex-direction: column;
  gap: 8px;
  padding: 10px 12px 12px;
}

.lb { flex: none; font-size: 12px; color: var(--muted); white-space: nowrap; }

.inp {
  height: 28px;
  padding: 0 10px;
  background: rgb(var(--inset-rgb) / 30%);
  border: 1px solid var(--border);
  color: var(--gray);
  font-family: var(--mono);
  font-size: 12.5px;
  outline: none;
}

.inp:focus { border-color: var(--cyan); }
.inp.key { flex: 0 1 380px; min-width: 160px; }
.inp.key.bad { border-color: var(--danger); color: var(--danger); }

.tg {
  flex: none;
  padding: 5px 7px 3px;
  border: 1px solid;
  font-family: var(--share);
  font-size: 10.5px;
  line-height: 1;
  letter-spacing: .04em;
}

.tg.ok { border-color: rgb(var(--green-rgb) / 35%); color: var(--green); }
.tg.dim { border-color: var(--border); color: var(--dim); }

.panes {
  flex: 1;
  min-height: 0;
  display: grid;
  grid-template-columns: 1fr 56px 1fr;
  gap: 8px;
}

/* 与封包编辑的十六进制外框同一套 */
.hexed {
  min-width: 0;
  min-height: 0;
  display: flex;
  flex-direction: column;
  border: 1px solid var(--border);
  background: var(--card);
  --hexview-bg: var(--card);
  overflow: hidden;
}

.hx-bar {
  flex: none;
  display: flex;
  align-items: center;
  gap: 14px;
  height: var(--th-h);
  padding: 0 12px;
  background: var(--panel);
  border-bottom: 1px solid var(--border);
  white-space: nowrap;
  overflow: hidden;
}

.hx-title {
  padding-top: 4px;
  font-family: var(--share);
  font-size: var(--th-size);
  letter-spacing: .14em;
  text-transform: uppercase;
  color: var(--th-fg);
}

.hx-meta { padding-top: 2px; font-family: Consolas, monospace; font-size: var(--th-size); color: var(--gray); }
.hx-meta.dim { color: var(--muted); overflow: hidden; text-overflow: ellipsis; }

.arrow {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 6px;
  color: var(--cyan);
  font-family: var(--share);
  font-size: 10.5px;
  letter-spacing: .14em;
  opacity: .8;
}

.arrow svg { width: 22px; height: 22px; stroke: currentColor; stroke-width: 1.8; fill: none; }

.empty { flex: 1; display: flex; align-items: center; justify-content: center; padding: 20px; text-align: center; color: var(--muted); font-size: 12.5px; line-height: 1.8; }
</style>
