<script setup lang="ts">
/*
  带高亮的多行文本框。

  textarea 自己画不了局部底色，所以底下垫一层 <pre>：字是透明的、只画每段的背景色，
  textarea 透明底叠在上面负责真正的文字与编辑，两层字体 / 内边距 / 行高完全一致、滚动同步，
  看起来就是「文本框里有些字被涂了颜色」。WinForms 那边是 AntdUI.Input.SetStyle 做的同一件事。

  marks 是字符区间；连着的同类区间调用方先合并，这里不再合 —— 差异比较在两段完全不同的文本上
  会给出几万个单字符区间，合并放在算差异的那一步更便宜。文本超过 MAX 字符时不画高亮（只剩纯文本），
  免得几十万个 span 把页面卡死。
*/
import { computed, ref } from 'vue'
import type { Mark } from '../stores/tools'

const props = withDefaults(defineProps<{
  modelValue: string
  marks?: Mark[]
  placeholder?: string
  readonly?: boolean
}>(), { marks: () => [], placeholder: '', readonly: false })

const emit = defineEmits<{ (e: 'update:modelValue', v: string): void }>()

const MAX = 400000

const ta = ref<HTMLTextAreaElement | null>(null)
const pre = ref<HTMLElement | null>(null)

const segs = computed<Array<{ t: string; c: string }>>(() => {
  const s = props.modelValue
  if (!props.marks.length || s.length > MAX) return [{ t: s, c: '' }]

  const ms = props.marks.filter((m) => m.end > m.start).sort((a, b) => a.start - b.start)
  const out: Array<{ t: string; c: string }> = []
  let i = 0

  for (const m of ms) {
    const st = Math.max(m.start, i)
    if (st >= s.length) break
    const en = Math.min(m.end, s.length)
    if (st > i) out.push({ t: s.slice(i, st), c: '' })
    if (en > st) out.push({ t: s.slice(st, en), c: m.cls })
    i = Math.max(i, en)
  }

  if (i < s.length) out.push({ t: s.slice(i), c: '' })
  return out
})

function onScroll(): void {
  const a = ta.value, p = pre.value
  if (a && p) { p.scrollTop = a.scrollTop; p.scrollLeft = a.scrollLeft }
}

function onInput(e: Event): void {
  emit('update:modelValue', (e.target as HTMLTextAreaElement).value)
}

/** 把第 pos 个字符滚进视野并选中 len 个字符。setSelectionRange 自己不会滚，按行号估一个 scrollTop。 */
function focusAt(pos: number, len = 1): void {
  const a = ta.value
  if (!a) return

  const s = props.modelValue
  const p = Math.max(0, Math.min(pos, s.length))
  const line = (s.slice(0, p).match(/\n/g) || []).length
  const lh = parseFloat(getComputedStyle(a).lineHeight) || 18

  a.scrollTop = Math.max(0, line * lh - a.clientHeight / 2)
  a.focus({ preventScroll: true })
  a.setSelectionRange(p, Math.min(s.length, p + len))
  onScroll()
}

defineExpose({ focusAt })
</script>

<template>
  <div class="ha">
    <pre ref="pre" class="ha-hl" aria-hidden="true"><span v-for="(g, i) in segs" :key="i" :class="g.c || undefined">{{ g.t }}</span>{{ '\n' }}</pre>
    <textarea
      ref="ta"
      class="ha-ta"
      :value="modelValue"
      :placeholder="placeholder"
      :readonly="readonly"
      spellcheck="false"
      wrap="off"
      @input="onInput"
      @scroll="onScroll"
    />
  </div>
</template>

<style scoped>
.ha { position: relative; flex: 1; min-height: 0; min-width: 0; }

/* 两层必须逐项一致：字体、字号、行高、内边距、换行策略，差一项就错位 */
.ha-hl,
.ha-ta {
  position: absolute;
  inset: 0;
  margin: 0;
  padding: 8px 10px;
  font-family: var(--mono);
  font-size: var(--fs-body);
  line-height: 18px;
  white-space: pre;
  tab-size: 4;
  box-sizing: border-box;
}

.ha-hl { overflow: hidden; color: transparent; pointer-events: none; }

.ha-ta {
  background: transparent;
  color: var(--gray);
  caret-color: var(--cyan);
  border: 0;
  outline: none;
  resize: none;
  overflow: auto;
}

/* 焦点由外层容器的边框表示（TextCompare 的 .iop），这里不画环 —— 见 style.css 的 .inp:focus */
.ha-ta:focus-visible { outline: none; }

.ha-ta::placeholder { color: var(--dim); }
.ha-ta::selection { background: rgb(var(--cyan-rgb) / 28%); }

/* 差异：A 里被删 / 改的红，B 里新增 / 改的绿 —— 与 WinForms 的 SetStyle 同一套配色语义 */
.ha-hl :deep(.del) { background: rgb(var(--danger-rgb) / 32%); }
.ha-hl :deep(.ins) { background: rgb(var(--green-rgb) / 26%); }
/* 正则命中：青 */
.ha-hl :deep(.rx) { background: rgb(var(--cyan-rgb) / 26%); }
/* 查重命中的字节：琥珀 */
.ha-hl :deep(.dup) { background: rgb(var(--amber-rgb) / 28%); }
</style>
