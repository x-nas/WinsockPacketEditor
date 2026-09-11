<script setup lang="ts">
/*
  自绘下拉框 —— 全项目的 <select> 都换成它。

  原生 <select> 的展开面板是操作系统画的，吃不到这套皮肤（白底、圆角、系统字），
  给 option 指定颜色只能救一半。自绘之后与右键菜单同一套语言：卡片底、发丝边、四角标记、青色高亮。

  原生下拉有三样东西自绘容易丢，这里都补上了：
  · 列表可滚（max-height + overflow），选项几十上百条也行；
  · 键盘：上下移动、Enter / 空格选中、Esc 关闭、Home / End、按首字母跳到下一个匹配项；
  · 超出视口时向上翻（先渲染再量，与 ContextMenu 同一套）。

  列表 Teleport 到 body：z-index 只在所在的层叠上下文里算数，留在弹窗里会被别的层盖住（右键菜单栽过）。
*/
import { computed, nextTick, onBeforeUnmount, ref, watch } from 'vue'

type Val = string | number

const props = withDefaults(defineProps<{
  modelValue: Val | null | undefined
  options: Array<{ value: Val; label: string; disabled?: boolean }>
  placeholder?: string
  disabled?: boolean
}>(), { placeholder: '', disabled: false })

const emit = defineEmits<{ (e: 'update:modelValue', v: Val): void }>()

const trig = ref<HTMLElement | null>(null)
const list = ref<HTMLElement | null>(null)
const scroll = ref<HTMLElement | null>(null)
const open = ref(false)
/** 键盘高亮的那一项 */
const hi = ref(-1)
const pos = ref({ x: 0, y: 0, w: 0 })

const current = computed(() => props.options.find((o) => o.value === props.modelValue) ?? null)

function firstEnabled(from: number, step: 1 | -1): number {
  const n = props.options.length
  for (let k = 0; k < n; k++) {
    const i = (from + k * step + n * k) % n   //绕圈
    if (!props.options[i]?.disabled) return i
  }
  return -1
}

async function openList(): Promise<void> {
  if (props.disabled || open.value) return
  open.value = true
  const idx = props.options.findIndex((o) => o.value === props.modelValue)
  hi.value = idx >= 0 ? idx : firstEnabled(0, 1)

  await nextTick()
  place()
  scrollHiIntoView()

  document.addEventListener('mousedown', onDocDown, true)
  document.addEventListener('scroll', onDocScroll, true)
  window.addEventListener('resize', close)
}

function close(): void {
  if (!open.value) return
  open.value = false
  document.removeEventListener('mousedown', onDocDown, true)
  document.removeEventListener('scroll', onDocScroll, true)
  window.removeEventListener('resize', close)
}

function toggle(): void { open.value ? close() : void openList() }

/** 放在触发框正下方、等宽；下面放不下就翻到上面 */
function place(): void {
  const t = trig.value, el = list.value
  if (!t || !el) return
  const r = t.getBoundingClientRect()
  const h = el.offsetHeight
  const below = window.innerHeight - r.bottom - 8
  const up = h > below && r.top - 8 > below
  pos.value = { x: r.left, y: up ? r.top - h - 2 : r.bottom + 2, w: r.width }
}

function onDocDown(e: MouseEvent): void {
  const tgt = e.target as Node
  if (list.value?.contains(tgt) || trig.value?.contains(tgt)) return
  close()
}

/** 列表自己滚不算；页面别处滚了它就停在原地、指向的框已经不在那了，与右键菜单同一条 */
function onDocScroll(e: Event): void {
  if (list.value && list.value.contains(e.target as Node)) return
  close()
}

function select(i: number): void {
  const o = props.options[i]
  if (!o || o.disabled) return
  emit('update:modelValue', o.value)
  close()
  trig.value?.focus()
}

function scrollHiIntoView(): void {
  const el = scroll.value?.children[hi.value] as HTMLElement | undefined
  el?.scrollIntoView?.({ block: 'nearest' })
}

function move(step: 1 | -1): void {
  if (!props.options.length) return
  if (!open.value) { void openList(); return }
  const n = props.options.length
  let i = hi.value
  for (let k = 0; k < n; k++) {
    i = (i + step + n) % n
    if (!props.options[i].disabled) break
  }
  hi.value = i
  scrollHiIntoView()
}

function onKey(e: KeyboardEvent): void {
  if (props.disabled) return

  switch (e.key) {
    case 'ArrowDown': e.preventDefault(); move(1); return
    case 'ArrowUp': e.preventDefault(); move(-1); return
    case 'Home': if (open.value) { e.preventDefault(); hi.value = firstEnabled(0, 1); scrollHiIntoView() } return
    case 'End': if (open.value) { e.preventDefault(); hi.value = firstEnabled(props.options.length - 1, -1); scrollHiIntoView() } return
    case 'Enter':
    case ' ':
      e.preventDefault()
      if (open.value) select(hi.value)
      else void openList()
      return
    case 'Escape':
      if (open.value) { e.preventDefault(); e.stopPropagation(); close() }
      return
    case 'Tab':
      close()
      return
  }

  //首字母跳转：从当前高亮的下一项开始找，找不到再从头
  if (e.key.length === 1 && !e.ctrlKey && !e.metaKey && !e.altKey) {
    const ch = e.key.toLowerCase()
    const n = props.options.length
    const from = open.value ? hi.value : props.options.findIndex((o) => o.value === props.modelValue)
    for (let k = 1; k <= n; k++) {
      const i = (from + k + n) % n
      const o = props.options[i]
      if (!o.disabled && o.label.toLowerCase().startsWith(ch)) {
        if (open.value) { hi.value = i; scrollHiIntoView() }
        else emit('update:modelValue', o.value)
        return
      }
    }
  }
}

//外面把它禁用了就收起来
watch(() => props.disabled, (d) => { if (d) close() })

onBeforeUnmount(close)
</script>

<template>
  <div class="cs" :class="{ open, disabled }">
    <button
      ref="trig"
      type="button"
      class="cs-btn"
      :disabled="disabled"
      :aria-expanded="open"
      aria-haspopup="listbox"
      @click="toggle"
      @keydown="onKey"
    >
      <span class="cs-label" :class="{ ph: !current }">{{ current ? current.label : placeholder }}</span>
      <svg class="cs-chev" viewBox="0 0 24 24"><path d="M6 9l6 6 6-6" /></svg>
    </button>

    <Teleport to="body">
      <!-- 外层不滚（角标挂它上面），里面一层才滚：绝对定位到盒子外的角标会被算进可滚动溢出，凭空多出一条滚动条 -->
      <div v-if="open" ref="list" class="cs-list" :style="{ left: pos.x + 'px', top: pos.y + 'px', width: pos.w + 'px' }">
        <span class="mk tl" /><span class="mk br" />
        <div ref="scroll" class="cs-scroll" role="listbox">
        <button
          v-for="(o, i) in options"
          :key="String(o.value)"
          type="button"
          class="cs-opt"
          :class="{ on: o.value === modelValue, hi: i === hi, dis: o.disabled }"
          role="option"
          :aria-selected="o.value === modelValue"
          @mousedown.prevent
          @mouseenter="hi = i"
          @click="select(i)"
        >{{ o.label }}</button>
        <div v-if="!options.length" class="cs-empty">—</div>
        </div>
      </div>
    </Teleport>
  </div>
</template>

<style scoped>
.cs { display: inline-flex; min-width: 0; }

/* 触发框：与各弹窗里的输入框同一副样子 */
.cs-btn {
  display: flex;
  align-items: center;
  gap: 8px;
  width: 100%;
  height: 28px;
  padding: 0 8px;
  background: rgb(var(--inset-rgb) / 30%);
  border: 1px solid var(--border);
  color: var(--gray);
  font-family: var(--mono);
  font-size: var(--fs-body);
  text-align: left;
  cursor: pointer;
  outline: none;
}

.cs-btn:hover:not(:disabled) { border-color: var(--dim); }
.cs-btn:focus-visible, .cs.open .cs-btn { border-color: var(--cyan); }
.cs-btn:disabled { opacity: .45; cursor: default; }

.cs-label { flex: 1; min-width: 0; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
.cs-label.ph { color: var(--muted); }

.cs-chev { flex: none; width: 12px; height: 12px; fill: none; stroke: var(--muted); stroke-width: 2; transition: transform .15s; }
.cs.open .cs-chev { transform: rotate(180deg); stroke: var(--cyan); }
</style>

<style>
/* 列表 Teleport 到了 body，scoped 管不到，用带前缀的全局类 */
.cs-list {
  position: fixed;
  z-index: 1500;
  background: var(--card);
  border: 1px solid var(--border);
  box-shadow: 0 10px 30px rgb(var(--shadow-rgb) / 55%);
}

.cs-scroll { max-height: 240px; overflow-y: auto; padding: 4px 0; }

.cs-list .mk { position: absolute; width: 7px; height: 7px; border: 1px solid var(--cyan); pointer-events: none; }
.cs-list .mk.tl { top: -1px; left: -1px; border-right: 0; border-bottom: 0; }
.cs-list .mk.br { bottom: -1px; right: -1px; border-left: 0; border-top: 0; }

.cs-opt {
  display: block;
  width: 100%;
  padding: 5px 10px;
  background: transparent;
  border: 0;
  color: var(--gray);
  font-family: var(--mono);
  font-size: var(--fs-body);
  text-align: left;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  cursor: pointer;
}

.cs-opt.hi { background: rgb(var(--cyan-rgb) / 10%); }
.cs-opt.on { color: var(--cyan); }
.cs-opt.dis { opacity: .4; cursor: default; }

.cs-empty { padding: 8px 10px; color: var(--muted); font-size: var(--fs-body); text-align: center; }
</style>
