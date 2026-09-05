<script setup lang="ts">
/*
  编码转换 —— 对应 WinForms 的 Controls/Transcoding（一个输入框 + 14 个只读结果框）。

  14 行结果全在 C# 算（SystemConfig.Transcode）：GBK 与系统默认编码浏览器里没有。
  这里只负责显示：每行一个标签 + 结果 + 复制按钮，结果可以选中。
  编码 = 文本 → 各编码的字节（ANSI-x 那一半是十六进制）；解码 = 十六进制 / 文本 → 各编码解回的字符串。
*/
import { ref } from 'vue'
import { call } from '../../bridge'
import { t } from '../../i18n'
import { pushToast } from '../../stores/toast'
import { trInput, trRows, trMode, type TranscodeRow } from '../../stores/tools'

const busy = ref(false)

async function run(decode: boolean): Promise<void> {
  if (!trInput.value.trim()) { pushToast('warning', t('tr.empty')); return }

  busy.value = true
  try {
    const r = await call<{ rows: TranscodeRow[] }>('transcode', { text: trInput.value, decode })
    trRows.value = r?.rows ?? []
    trMode.value = decode ? 'dec' : 'enc'
  } catch (e) {
    console.error('[tr] 转换失败', e)
  } finally {
    busy.value = false
  }
}

function clearAll(): void {
  trInput.value = ''
  trRows.value = []
  trMode.value = ''
}

async function copy(v: string): Promise<void> {
  if (!v) return
  try {
    await call('clipboardWrite', { text: v })
    pushToast('success', t('pm.copied'))
  } catch (e) {
    console.error('[tr] 写剪贴板失败', e)
  }
}

/** 结果放回输入框：编码完再解码、或者拿某一行继续转 */
function useAsInput(v: string): void {
  if (!v) return
  trInput.value = v
}
</script>

<template>
  <div class="page list-page tr">
    <div class="bar">
      <button class="btn primary" :disabled="busy || !trInput.trim()" @click="run(false)">{{ t('tr.encode') }}</button>
      <button class="btn primary" :disabled="busy || !trInput.trim()" @click="run(true)">{{ t('tr.decode') }}</button>
      <span class="sep" />
      <span class="lb">{{ t('tr.hint') }}</span>
      <span class="grow" />
      <span v-if="trMode" class="tg" :class="trMode">{{ trMode === 'enc' ? t('tr.encResult') : t('tr.decResult') }}</span>
      <button class="btn danger" :disabled="!trInput && !trRows.length" @click="clearAll">{{ t('rb.clearAll') }}</button>
    </div>

    <div class="src">
      <div class="ph">
        <span class="tt">{{ t('tr.input') }}</span>
        <span class="meta">{{ t('tc.length') }} <b>{{ trInput.length }}</b></span>
      </div>
      <textarea v-model="trInput" class="ta" spellcheck="false" :placeholder="t('tr.inputPh')" @keydown.ctrl.enter="run(false)" />
    </div>

    <div class="out">
      <div class="ph">
        <span class="tt">{{ t('tr.results') }}</span>
        <span class="meta">{{ trRows.length }}</span>
      </div>

      <div class="rows">
        <div v-if="!trRows.length" class="empty">{{ t('tr.emptyOut') }}</div>

        <div v-for="r in trRows" :key="r.Key" class="r" :class="{ ansi: r.Key.startsWith('ANSI') }">
          <span class="k">{{ r.Key }}</span>
          <span class="v" :class="{ none: !r.Value }">{{ r.Value || '—' }}</span>
          <span class="ops">
            <button class="op" :title="t('tr.useAsInput')" :disabled="!r.Value" @click="useAsInput(r.Value)">
              <svg class="ico" viewBox="0 0 24 24"><path d="M20 12H6M11 6l-6 6 6 6" /></svg>
            </button>
            <button class="op" :title="t('lst.copy')" :disabled="!r.Value" @click="copy(r.Value)">
              <svg class="ico" viewBox="0 0 24 24"><rect x="9" y="9" width="11" height="11" /><path d="M5 15V5h10" /></svg>
            </button>
          </span>
        </div>
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

.sep { width: 1px; height: 16px; background: var(--border); flex: none; }
.lb { font-size: 11.5px; color: #8a94a6; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; min-width: 0; }

.tg {
  flex: none;
  padding: 5px 7px 3px;
  border: 1px solid;
  font-family: var(--share);
  font-size: 10.5px;
  line-height: 1;
  letter-spacing: .08em;
  text-transform: uppercase;
}

.tg.enc { border-color: rgb(0 255 136 / 35%); color: var(--green); }
.tg.dec { border-color: rgb(0 212 255 / 35%); color: var(--cyan); }

.src,
.out {
  min-height: 0;
  display: flex;
  flex-direction: column;
  border: 1px solid var(--border);
  background: #000;
}

.src { flex: 0 0 150px; }
.out { flex: 1; }

.ph {
  flex: none;
  display: flex;
  align-items: center;
  gap: 14px;
  height: var(--th-h);
  padding: 0 12px;
  background: var(--panel);
  border-bottom: 1px solid var(--border);
  font-family: var(--share);
  font-size: var(--th-size);
  letter-spacing: .14em;
  text-transform: uppercase;
  color: #a8b2c0;
}

.ph > span { padding-top: 4px; }
.ph .tt { color: var(--cyan); }
.ph .meta { color: var(--muted); letter-spacing: .06em; text-transform: none; }
.ph .meta b { font-family: var(--mono); color: var(--gray); font-weight: 400; }

.ta {
  flex: 1;
  min-height: 0;
  margin: 0;
  padding: 8px 10px;
  background: transparent;
  border: 0;
  outline: none;
  resize: none;
  color: var(--gray);
  caret-color: var(--cyan);
  font-family: var(--mono);
  font-size: 12.5px;
  line-height: 1.6;
}

.ta::placeholder { color: #4b5563; }

.rows { flex: 1; min-height: 0; overflow-y: auto; }

.r {
  display: grid;
  grid-template-columns: 128px minmax(0, 1fr) 64px;
  align-items: center;
  gap: 12px;
  min-height: 34px;
  padding: 6px 14px;
  border-bottom: 1px solid rgb(42 42 58 / 45%);
}

.r:hover { background: rgb(255 255 255 / 3%); }

/* 成对的两行：原文行与 ANSI（十六进制）行，后者字色压一档，扫下来能看出成对 */
.k { font-family: var(--share); font-size: 11px; letter-spacing: .1em; color: var(--cyan); }
.r.ansi .k { color: #7cc4d8; }

.v {
  min-width: 0;
  font-family: var(--mono);
  font-size: 12px;
  line-height: 1.5;
  color: #6ee7a8;
  word-break: break-all;
  white-space: pre-wrap;
  user-select: text;
}

.r.ansi .v { color: #94a3b8; }
.v.none { color: #4b5563; }

.ops { display: flex; align-items: center; justify-content: center; gap: 4px; }

.empty { padding: 40px 20px; text-align: center; color: var(--muted); font-size: 12.5px; line-height: 1.8; }
</style>
