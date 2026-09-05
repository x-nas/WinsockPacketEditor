<script setup lang="ts">
/*
  数据提取 —— 对应 WinForms 的 Controls/ExtractionData（一个下拉 + 拖放区 + 结果框 + 生成按钮）。

  三种提取都在 C# 做（SystemConfig.ExtractData），这里只管把文件送过去、把文本显示出来。
  文件有两条路进来：点「选择文件」走 C# 的原生文件框（能拿到路径）；直接拖进来的文件浏览器只给内容，
  读成 base64 送过去（extractBytes）。结果框可以改，改完再「生成文件」。
*/
import { computed, ref } from 'vue'
import { call } from '../../bridge'
import { t } from '../../i18n'
import { pushToast } from '../../stores/toast'
import { exKind, exText, exPath } from '../../stores/tools'
import CyberSelect from '../CyberSelect.vue'

interface ExtractResult { Path: string; Text: string; Error: string }

const busy = ref(false)
const over = ref(false)

const kindOptions = computed(() => [
  { value: 0, label: t('ex.k0') },
  { value: 1, label: t('ex.k1') },
  { value: 2, label: t('ex.k2') },
])

const EXT = ['.chlsx', '.filt', '.pa']
const OUT = ['.txt', '.fp', '.ini']

const lines = computed(() => (exText.value ? exText.value.split('\n').length : 0))

function apply(r: ExtractResult | null, fallbackName = ''): void {
  if (!r) return
  if (r.Error) { pushToast('error', r.Error); return }
  if (!r.Text && !r.Path) return   //文件框被取消
  exText.value = r.Text
  exPath.value = r.Path || fallbackName
  pushToast('success', t('ex.extracted'))
}

async function pick(): Promise<void> {
  busy.value = true
  try {
    apply(await call<ExtractResult>('extractPick', { kind: exKind.value }))
  } catch (e) {
    console.error('[ex] 选择文件失败', e)
  } finally {
    busy.value = false
  }
}

function toBase64(buf: ArrayBuffer): string {
  const u8 = new Uint8Array(buf)
  let s = ''
  for (let i = 0; i < u8.length; i += 0x8000) s += String.fromCharCode.apply(null, Array.from(u8.subarray(i, i + 0x8000)))
  return btoa(s)
}

async function onDrop(e: DragEvent): Promise<void> {
  over.value = false
  const f = e.dataTransfer?.files?.[0]
  if (!f) return

  //拖进来的文件类型不对就提醒一句，不猜 —— 三种格式的解析方式完全不同
  if (!f.name.toLowerCase().endsWith(EXT[exKind.value])) {
    pushToast('warning', t('ex.wrongExt') + ' ' + EXT[exKind.value])
    return
  }

  busy.value = true
  try {
    const content = toBase64(await f.arrayBuffer())
    apply(await call<ExtractResult>('extractBytes', { kind: exKind.value, content, name: f.name }), f.name)
  } catch (e) {
    console.error('[ex] 读取拖入文件失败', e)
  } finally {
    busy.value = false
  }
}

async function save(): Promise<void> {
  if (!exText.value.trim()) { pushToast('warning', t('ex.empty')); return }
  try {
    await call<{ path: string }>('saveExtraction', { kind: exKind.value, text: exText.value })
  } catch (e) {
    console.error('[ex] 生成文件失败', e)
  }
}

function clearAll(): void {
  exText.value = ''
  exPath.value = ''
}
</script>

<template>
  <div class="page list-page ex">
    <div class="bar">
      <CyberSelect v-model="exKind" :options="kindOptions" class="sel" />
      <button class="btn" :disabled="busy" @click="pick">{{ t('ex.pick') }}</button>
      <span class="grow" />
      <button class="btn primary" :disabled="!exText.trim()" @click="save">{{ t('ex.save') }} {{ OUT[exKind] }}</button>
      <button class="btn danger" :disabled="!exText" @click="clearAll">{{ t('rb.clearAll') }}</button>
    </div>

    <!-- 拖放区：没有结果时占满，有结果时缩成一条，让位给文本 -->
    <div
      class="drop"
      :class="{ over, slim: !!exText, busy }"
      @dragover.prevent="over = true"
      @dragleave="over = false"
      @drop.prevent="onDrop"
      @click="!exText && pick()"
    >
      <svg class="ico" viewBox="0 0 24 24"><path d="M12 16V4M7 9l5-5 5 5" /><path d="M4 20h16" /></svg>
      <div class="tx">
        <div class="t1">{{ busy ? t('proxy.working') : (exText ? t('ex.dropAgain') : t('ex.drop')) }}</div>
        <div class="t2">{{ t('ex.dropHint') }} <b>{{ EXT[exKind] }}</b> · {{ t('ex.to') }} <b>{{ OUT[exKind] }}</b></div>
      </div>
    </div>

    <div v-if="exText" class="res">
      <div class="ph">
        <span class="tt">{{ t('ex.result') }}</span>
        <span class="meta path" :title="exPath">{{ exPath || '—' }}</span>
        <span class="grow" />
        <span class="meta">{{ t('ex.lines') }} <b>{{ lines }}</b></span>
        <span class="meta">{{ t('tc.length') }} <b>{{ exText.length }}</b></span>
      </div>
      <textarea v-model="exText" class="ta" spellcheck="false" wrap="off" />
      <div class="ft">{{ t('ex.editHint') }}</div>
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

.sel { width: 420px; flex: none; }

/* 拖放区 */
.drop {
  flex: 1;
  min-height: 0;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 14px;
  padding: 24px;
  border: 1px dashed #3a3a4e;
  background:
    linear-gradient(rgb(0 212 255 / 3%), transparent),
    repeating-linear-gradient(0deg, transparent 0 39px, rgb(255 255 255 / 2.5%) 39px 40px),
    repeating-linear-gradient(90deg, transparent 0 39px, rgb(255 255 255 / 2.5%) 39px 40px);
  color: var(--muted);
  cursor: pointer;
  transition: border-color .15s, background-color .15s;
}

.drop:hover { border-color: #4b5563; }
.drop.over { border-color: var(--cyan); background-color: rgb(0 212 255 / 5%); color: var(--cyan); }
.drop.busy { pointer-events: none; opacity: .6; }

.drop .ico { width: 34px; height: 34px; stroke: currentColor; stroke-width: 1.6; fill: none; opacity: .8; }
.drop .tx { text-align: center; }
.drop .t1 { font-size: 13px; color: var(--gray); }
.drop .t2 { margin-top: 6px; font-size: 11.5px; color: #8a94a6; }
.drop .t2 b { font-family: var(--mono); font-weight: 400; color: var(--cyan); }

/* 有结果之后收成一条横幅，仍然能接着拖 */
.drop.slim { flex: none; flex-direction: row; justify-content: flex-start; gap: 12px; padding: 8px 14px; cursor: default; }
.drop.slim .ico { width: 18px; height: 18px; }
.drop.slim .tx { text-align: left; display: flex; align-items: baseline; gap: 12px; }
.drop.slim .t1 { font-size: 12px; }
.drop.slim .t2 { margin: 0; }

/* 结果 */
.res {
  flex: 1;
  min-height: 0;
  display: flex;
  flex-direction: column;
  border: 1px solid var(--border);
  background: #000;
}

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
  min-width: 0;
}

.ph > span { padding-top: 4px; }
.ph .tt { color: var(--cyan); flex: none; }
.ph .meta { color: var(--muted); letter-spacing: .06em; text-transform: none; flex: none; }
.ph .meta b { font-family: var(--mono); color: var(--gray); font-weight: 400; }
.ph .meta.path { flex: 0 1 auto; min-width: 0; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; font-family: var(--mono); letter-spacing: 0; }
.ph .grow { flex: 1; }

.ta {
  flex: 1;
  min-height: 0;
  margin: 0;
  padding: 8px 10px;
  background: transparent;
  border: 0;
  outline: none;
  resize: none;
  color: #6ee7a8;
  caret-color: var(--cyan);
  font-family: var(--mono);
  font-size: 12px;
  line-height: 18px;
  white-space: pre;
}

.ft { flex: none; padding: 6px 12px; border-top: 1px solid var(--border); font-size: 11px; color: #8a94a6; }
</style>
