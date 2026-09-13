<script setup lang="ts">
/*
  数据提取 —— 对应 WinForms 的 Controls/ExtractionData（一个下拉 + 拖放区 + 结果框 + 生成按钮）。

  三种提取都在 C# 做（SystemConfig.ExtractData），这里只管把文件送过去、把文本显示出来。
  文件有两条路进来：点「选择文件」走 C# 的原生文件框（能拿到路径）；直接拖进来的浏览器只给内容，
  读成 base64 送过去（extractBytes）。结果框可以改，改完再「生成」。

  ══ 2026-09-08 整屏重做，先说清为什么 ══

    ① <b>类型从 420px 的下拉改成三张卡。</b> 它是个只有三项的固定枚举，而三项的差别
       （吃什么文件、吐什么文件、干什么用）恰恰是选之前最需要知道的 ——
       下拉把这些藏在一行文字里，还要点开才看得见另外两项。
    ② <b>结果头上加「提取到 N 条」。</b> 老版本只有行数与字符数，那两个数回答不了
       「到底提出来几条」，而那是按下「生成」之前唯一想确认的事。
       （C# 的 ExtractResult 为此多了一个 Count。）
    ③ 加「复制」—— 提出来的十六进制多半是要贴到滤镜 / 异或计算里去的，
       为此先存一个文件再打开它，纯属绕路。

  ══ 顺带修掉的一个 bug ══

    <b>Charles 那一路原来只提第一条会话。</b> 写法是 Descendants("response").FirstOrDefault()，
    而一个 .chlsx 里通常有几十上百条 —— 拖一整段抓包进来只出来一条封包，而且<b>没有任何提示</b>，
    看着像「这文件里就这么点东西」。现在每一条会话的响应体都提出来，块之间空一行隔开。
*/
import { computed, ref } from 'vue'
import { call } from '../../bridge'
import { t, type Key } from '../../i18n'
import { pushToast } from '../../stores/toast'
import { exKind, exText, exPath, exCount } from '../../stores/tools'

interface ExtractResult { Path: string; Text: string; Error: string; Count: number }

const busy = ref(false)
const over = ref(false)

/*
  三种类型。

  ⚠️ <b>扩展名与 C# 的 ExtractOpenFilter / SaveExtraction_Dialog 是<b>两份</b>写法，要一起改。</b>
  这边只是给人看的提示与拖放时的类型检查，真正决定文件框过滤器的是 C# 那两处 ——
  它们对不上的表现是「提示写着 .pa，文件框却只让选 .chlsx」。
*/
const KINDS: Array<{ name: Key; desc: Key; ext: string; out: string }> = [
  { name: 'ex.k0', desc: 'ex.k0d', ext: '.chlsx', out: '.txt' },
  { name: 'ex.k1', desc: 'ex.k1d', ext: '.filt', out: '.fp' },
  { name: 'ex.k2', desc: 'ex.k2d', ext: '.pa', out: '.ini' },
]

const cur = computed(() => KINDS[exKind.value] ?? KINDS[0])
const lines = computed(() => (exText.value ? exText.value.split('\n').length : 0))

function apply(r: ExtractResult | null, fallbackName = ''): void {
  if (!r) return
  if (r.Error) { pushToast('error', r.Error); return }

  //文件框被取消：既没有文本也没有路径
  if (!r.Text && !r.Path) return

  exText.value = r.Text
  exPath.value = r.Path || fallbackName
  exCount.value = r.Count || 0
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
  //分块：一次 apply 整个数组会在几十万字节上炸掉调用栈
  for (let i = 0; i < u8.length; i += 0x8000) s += String.fromCharCode.apply(null, Array.from(u8.subarray(i, i + 0x8000)))
  return btoa(s)
}

/*
  ⚠️ `dragleave` 在<b>子元素之间移动</b>时也会冒上来 —— 直接 `over = false`
  会让覆盖层在整页上疯狂闪。判一下鼠标是不是真的离开了这棵子树。
*/
function onDragLeave(e: DragEvent): void {
  const to = e.relatedTarget as Node | null
  if (to && (e.currentTarget as HTMLElement).contains(to)) { return }
  over.value = false
}

async function onDrop(e: DragEvent): Promise<void> {
  over.value = false
  const f = e.dataTransfer?.files?.[0]
  if (!f) return

  //拖进来的文件类型不对就提醒一句，不猜 —— 三种格式的解析方式完全不同
  if (!f.name.toLowerCase().endsWith(cur.value.ext)) {
    pushToast('warning', t('ex.wrongExt') + ' ' + cur.value.ext)
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

async function copy(): Promise<void> {
  if (!exText.value) return
  try {
    await call('clipboardWrite', { text: exText.value })
    pushToast('success', t('pm.copied'))
  } catch (e) {
    console.error('[ex] 写剪贴板失败', e)
  }
}

/*
  换类型要把结果清掉。

  ⚠️ 不清的话，屏幕上会是「类型：账号备份」配着一段上一次提出来的十六进制，
  而「生成」按的是<b>新</b>类型 —— 等于拿 Charles 的数据去存一个 .ini。
*/
function setKind(i: number): void {
  if (exKind.value === i) return
  exKind.value = i
  clearAll()
}

function clearAll(): void {
  exText.value = ''
  exPath.value = ''
  exCount.value = 0
}
</script>

<template>
  <!--
    整页都能拖文件进来（原来是页面中间一块常驻的拖放区，按要求去掉了 ——
    工具条第一颗就是「选择文件」，那一块占的是结果区的高度）。
    ⚠️ 能力没丢：拖进浏览器的文件只有内容没有路径，仍走 base64 那条路。
  -->
  <div
    class="page list-page ex"
    :class="{ over }"
    @dragover.prevent="over = true"
    @dragleave="onDragLeave"
    @drop.prevent="onDrop"
  >
    <div v-if="over" class="dropveil">
      <span>{{ t('ex.drop') }} · {{ t('ex.in') }} <b>{{ cur.ext }}</b></span>
    </div>
    <div class="bar">
      <button class="btn" :disabled="busy" @click="pick">{{ t('ex.pick') }}</button>
      <span class="grow" />
      <button class="btn" :disabled="!exText" @click="copy">{{ t('ex.copy') }}</button>
      <button class="btn primary" :disabled="!exText.trim()" @click="save">{{ t('ex.save') }} {{ cur.out }}</button>
      <button class="btn danger" :disabled="!exText" @click="clearAll">{{ t('rb.clearAll') }}</button>
    </div>

    <!-- 三张类型卡：吃什么 → 吐什么 + 一句用途。比一个下拉多说了两件事，还不用点开 -->
    <div class="kinds">
      <button
        v-for="(k, i) in KINDS"
        :key="k.ext"
        class="kd"
        :class="{ on: exKind === i }"
        type="button"
        @click="setKind(i)"
      >
        <span class="kn">{{ t(k.name) }}</span>
        <span class="kf">
          <b>{{ k.ext }}</b>
          <i aria-hidden="true">→</i>
          <b class="o">{{ k.out }}</b>
        </span>
        <span class="kdz">{{ t(k.desc) }}</span>
      </button>
    </div>


    <div v-if="exText" class="res">
      <div class="ph">
        <span class="tt">{{ t('ex.result') }}</span>
        <span v-if="exCount" class="meta big">{{ t('ex.count') }} <b>{{ exCount }}</b> {{ t('ex.items') }}</span>
        <span class="meta path" :title="exPath">{{ exPath || '—' }}</span>
        <span class="grow" />
        <span class="meta">{{ t('ex.lines') }} <b>{{ lines }}</b></span>
        <span class="meta"><b>{{ exText.length }}</b> {{ t('ex.chars') }}</span>
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

/* ── 三张类型卡 ── */
.kinds { flex: none; display: grid; grid-template-columns: repeat(3, 1fr); gap: 8px; }

.kd {
  display: flex;
  flex-direction: column;
  align-items: flex-start;
  gap: 5px;
  min-width: 0;
  padding: 9px 12px 10px;
  border: 1px solid var(--border);
  background: var(--card);
  color: inherit;
  text-align: left;
  cursor: pointer;
  transition: border-color .15s, background-color .15s;
}

.kd:hover { border-color: var(--dim); }
.kd:focus-visible { outline-offset: -2px; }

/* 选中的那张：左边一道青色轨 + 提亮，与机架卡那套色轨同一个语汇 */
.kd.on {
  border-color: rgb(var(--cyan-rgb) / 45%);
  background: rgb(var(--cyan-rgb) / 6%);
  box-shadow: inset 3px 0 0 var(--cyan);
}

.kn { font-size: var(--fs-body); color: var(--gray); white-space: nowrap; overflow: hidden; text-overflow: ellipsis; max-width: 100%; }
.kd.on .kn { color: var(--cyan); }

.kf { display: flex; align-items: baseline; gap: 6px; font-family: var(--mono); font-size: var(--fs-small); color: var(--muted); }
.kf b { font-weight: 400; color: var(--soft); }
.kf b.o { color: var(--green); }
.kf i { font-style: normal; color: var(--dim); }

.kdz { font-size: var(--fs-small); color: var(--dim2); white-space: nowrap; overflow: hidden; text-overflow: ellipsis; max-width: 100%; }

/* ── 拖放区 ── */
/*
  拖文件进来时的覆盖层。原来那一大块常驻的拖放区已经去掉（工具条第一颗就是「选择文件」），
  现在整页都能接文件，只在真的拖着东西悬停时才浮这一层。

  ⚠️ **必须 `pointer-events: none`** —— 它盖在页面上，一旦吃事件，
  盖住的那一刻就等于「拖出去了」，`dragleave` / `dragover` 会开始互相打架、整层闪个不停。
*/
.dropveil {
  position: absolute;
  inset: 0;
  z-index: 5;
  display: flex;
  align-items: center;
  justify-content: center;
  pointer-events: none;
  background: rgb(var(--veil-rgb) / 55%);   /* 字直接在蒙层上，用 --veil（浅色下是磨砂），见 tokens.css */
  border: 1px dashed var(--cyan);
  color: var(--cyan);
  font-size: var(--fs-lead);
  letter-spacing: .04em;
}

.dropveil b { font-family: var(--mono); font-weight: 400; }

/* 覆盖层是 absolute 的，容器要给它一个定位上下文 */
.ex { position: relative; }
.drop.slim .t2 { margin: 0; }

/* ── 结果 ── */
/* 里面的 .ta 没有自己的边框，焦点由这层容器表示 —— 与输入框同一种语言 */
.res:focus-within { border-color: var(--cyan); }
.ta:focus-visible { outline: none; }

.res {
  flex: 1;
  min-height: 0;
  display: flex;
  flex-direction: column;
  border: 1px solid var(--border);
  background: var(--sink);
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
  color: var(--th-fg);
  min-width: 0;
}

.ph > span { padding-top: 2px; }   /* 原 4px 在字体度量覆写之后偏低 1px（2026-09-13 字体度量覆写后按 100% 缩放实测重调） */
.ph .tt { color: var(--cyan); flex: none; }
.ph .meta { color: var(--muted); letter-spacing: .06em; text-transform: none; flex: none; }
.ph .meta b { font-family: var(--mono); color: var(--gray); font-weight: 400; }
.ph .meta.big b { color: var(--green); font-size: var(--fs-lead); }
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
  color: var(--acc-green2);
  caret-color: var(--cyan);
  font-family: var(--mono);
  font-size: var(--fs-body);
  line-height: 18px;
  white-space: pre;
}

.ft { flex: none; padding: 6px 12px; border-top: 1px solid var(--border); font-size: var(--fs-small); color: var(--dim2); }
</style>
