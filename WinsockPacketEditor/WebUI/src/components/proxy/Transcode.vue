<script setup lang="ts">
/*
  编码转换 —— 对应 WinForms 的 Controls/Transcoding（一个输入框 + 14 个只读结果框）。

  14 行结果全在 C# 算（SystemConfig.Transcode）：GBK 与「本机默认编码」浏览器里没有。

  ══ 2026-09-08 整屏重做，先说清为什么 ══

  老版本是「输入框 + 两个按钮 + 14 行平铺的流水账」，三处说不清：

    ① <b>14 行其实是 8 组 × 2</b>（每种编码一行文本、一行十六进制），平铺出来只靠字色深浅
       暗示成对关系 —— 而「UTF-8 的字节长什么样」正是要把这两行<b>并排</b>看的。现在收成 8 组。
    ② <b>两个标签在骗人。</b> 「UTF16」是 Encoding.BigEndianUnicode（<b>大端</b>），
       「Unicode」是 Encoding.Unicode（<b>小端</b>）—— 同一族编码、两个看不出关系的名字，
       而且都没写字节序。实测「你」在 ANSI-UTF16 里是 4F 60、在 ANSI-Unicode 里是 60 4F，
       正好相反。界面上照实写成 UTF-16 BE / UTF-16 LE（UTF32 同理，是 LE）。
       ⚠️ <b>Key 本身没改</b> —— 那是与 C# 的契约，改了两边就对不上。改的只是显示名。
    ③ <b>每一行到底做了什么运算，看不出来。</b> 这一屏最难懂的就是这个：
       解码模式下「UTF8」那行返回的是输入原样，因为它做的是「把输入当默认编码的文本取字节、
       再按 UTF-8 读回来」。现在每一行都挂一句算式（文本 →(UTF-8)→ 字节 → 十六进制）。

  另外「编码 / 解码」从两个按钮改成<b>分段按钮</b>并实时跑：它本来就是二选一
  （项目里那条「标签会跟着状态变的勾选框、以及事实上的二选一，一律换分段按钮」的口径）。
*/
import { computed, onBeforeUnmount, ref, watch } from 'vue'
import { call } from '../../bridge'
import { t } from '../../i18n'
import { pushToast } from '../../stores/toast'
import { trInput, trRows, trMode, type TranscodeRow } from '../../stores/tools'

const busy = ref(false)

/* ── 分组表 ────────────────────────────────────────────────
   ⚠️ 这张表必须覆盖 C# 返回的<b>每一个</b> Key，与 PACKET_TYPE 那条是同一条规矩。
   漏掉的那个会静默地不显示 —— 所以下面 groups 里另有一支兜底：认不出来的 Key
   单独成一组挂在最后并标「未归组」，宁可难看也不要凭空少一行。 */
interface GroupDef {
  /** 界面上的名字。刻意与 Key 不同 —— Key 是与 C# 的契约，名字是给人读的 */
  name: string
  /** 文本行的 Key（空 = 这一组没有文本行） */
  text: string
  /** 十六进制行的 Key */
  hex: string
  /** 这一组在算式里用的编码名 */
  enc: string
  /** 特殊说明 */
  note?: 'bytes' | 'gbk' | 'b64'
}

const GROUPS: GroupDef[] = [
  { name: 'BYTES', text: 'Bytes', hex: '', enc: '', note: 'bytes' },
  { name: 'GBK', text: '', hex: 'ANSI-GBK', enc: 'GBK', note: 'gbk' },
  { name: 'UTF-7', text: 'UTF7', hex: 'ANSI-UTF7', enc: 'UTF-7' },
  { name: 'UTF-8', text: 'UTF8', hex: 'ANSI-UTF8', enc: 'UTF-8' },
  { name: 'UTF-16 BE', text: 'UTF16', hex: 'ANSI-UTF16', enc: 'UTF-16 BE' },
  { name: 'UTF-32 LE', text: 'UTF32', hex: 'ANSI-UTF32', enc: 'UTF-32 LE' },
  { name: 'UTF-16 LE', text: 'Unicode', hex: 'ANSI-Unicode', enc: 'UTF-16 LE' },
  { name: 'BASE64', text: 'base64', hex: 'ANSI-base64', enc: 'Base64', note: 'b64' },
]

interface Row { label: string; value: string; how: string; hex: boolean }
interface Group { name: string; rows: Row[]; note?: string }

/** 每一行的算式。这一屏最难懂的就是它，所以逐行写出来而不是笼统说一句。 */
function how(g: GroupDef, isHex: boolean, dec: boolean): string {
  const T = t('hex.asText')
  const H = t('hex.asHex')
  const B = t('tr.wBytes')
  const D = t('tr.wDefault')

  if (g.note === 'bytes') return t('tr.bytesNote')

  if (g.note === 'b64') {
    if (!dec) return isHex ? t('tr.b64EncHex') : t('tr.b64EncText')
    return isHex ? t('tr.b64DecHex') : t('tr.b64DecText')
  }

  if (!dec) {
    //编码：文本 →(X)→ 字节 → 十六进制 / 再按本机默认读回文本
    return isHex
      ? `${T} →(${g.enc})→ ${B} → ${H}`
      : `${T} →(${g.enc})→ ${B} →(${D})→ ${T}`
  }

  //解码：十六进制那半把输入当字节，另一半把输入当本机默认编码的文本
  return isHex
    ? `${H} → ${B} →(${g.enc})→ ${T}`
    : `${T} →(${D})→ ${B} →(${g.enc})→ ${T}`
}

const groups = computed<Group[]>(() => {
  const map = new Map<string, string>()
  for (const r of trRows.value) map.set(r.Key, r.Value)

  const dec = trMode.value === 'dec'
  const used = new Set<string>()
  const out: Group[] = []

  for (const g of GROUPS) {
    const rows: Row[] = []

    /*
      ⚠️ <b>标签在两种模式下说的不是同一件事。</b>

      编码时两行的<b>值</b>一个是文本、一个是十六进制，标「文本 / 十六进制」正好。
      解码时<b>两行的值都是文本</b> —— 差别在于输入被当成什么读的（默认编码的文本，还是十六进制），
      再标「十六进制」就是指着一段文本说它是十六进制。所以解码时改标输入的读法。
    */
    if (g.text && map.has(g.text)) {
      used.add(g.text)
      rows.push({
        label: g.note === 'bytes' ? t('tr.wDec') : dec ? t('tr.fromText') : t('hex.asText'),
        value: map.get(g.text) ?? '',
        how: how(g, false, dec),
        hex: false,
      })
    }

    if (g.hex && map.has(g.hex)) {
      used.add(g.hex)
      rows.push({
        label: dec ? t('tr.fromHex') : t('hex.asHex'),
        value: map.get(g.hex) ?? '',
        how: how(g, true, dec),
        hex: true,
      })
    }

    if (!rows.length) continue

    //GBK 少的那一行<b>两种模式下都少</b>，所以这句注不分模式
    out.push({ name: g.name, rows, note: g.note === 'gbk' ? t('tr.gbkNote') : undefined })
  }

  //兜底：C# 哪天加了新 Key，宁可挂在最后也不要静默丢掉
  for (const r of trRows.value) {
    if (used.has(r.Key)) continue
    out.push({ name: r.Key, rows: [{ label: t('tr.unknown'), value: r.Value, how: '', hex: false }] })
  }

  return out
})

/* ── 实时跑 ────────────────────────────────────────────────
   ⚠️ 要带一个序号：防抖之后仍可能有两次调用在途，先发的后回就会把新结果盖成旧的。
   桥调用是异步的，这一条不是理论风险。 */
let seq = 0
let timer = 0

async function run(): Promise<void> {
  const text = trInput.value
  const dec = trMode.value === 'dec'

  if (!text.trim()) { trRows.value = []; return }

  const mine = ++seq
  busy.value = true

  try {
    const r = await call<{ rows: TranscodeRow[] }>('transcode', { text, decode: dec })
    if (mine !== seq) return       //已经有更新的一次在路上了，这次的结果是旧的
    trRows.value = r?.rows ?? []
  } catch (e) {
    if (mine === seq) trRows.value = []
    console.error('[tr] 转换失败', e)
  } finally {
    if (mine === seq) busy.value = false
  }
}

function schedule(): void {
  window.clearTimeout(timer)
  timer = window.setTimeout(run, 220)
}

watch([trInput, trMode], schedule, { immediate: true })
onBeforeUnmount(() => window.clearTimeout(timer))

/* ── 动作 ──────────────────────────────────────────────────── */

async function copy(v: string): Promise<void> {
  if (!v) return
  try {
    await call('clipboardWrite', { text: v })
    pushToast('success', t('pm.copied'))
  } catch (e) {
    console.error('[tr] 写剪贴板失败', e)
  }
}

/** 结果放回输入框：编码完再解码、或者拿某一行接着转。会自动重跑，不用再按什么。 */
function useAsInput(v: string): void {
  if (v) trInput.value = v
}

function clearAll(): void {
  trInput.value = ''
  trRows.value = []
}

/** 输入按 UTF-8 有多少字节 —— 与「字符数」并排才看得出中文一个字占几个字节 */
const inputBytes = computed(() => new TextEncoder().encode(trInput.value).length)
</script>

<template>
  <div class="page list-page tr">
    <div class="bar">
      <div class="hx-seg">
        <button class="hx-segb after" :class="{ on: trMode === 'enc' }" @click="trMode = 'enc'">{{ t('tr.encode') }}</button>
        <button class="hx-segb before" :class="{ on: trMode === 'dec' }" @click="trMode = 'dec'">{{ t('tr.decode') }}</button>
      </div>

      <!-- 短的那句常驻，完整的解释（解码时输入怎么被读）挂在悬停提示上 -->
      <span class="lb" :title="t('tr.hintTip')">{{ t('tr.hint') }}</span>

      <button class="btn danger" :disabled="!trInput && !trRows.length" @click="clearAll">{{ t('rb.clearAll') }}</button>
    </div>

    <div class="src">
      <div class="ph">
        <span class="tt">{{ t('tr.input') }}</span>
        <span class="meta"><b>{{ trInput.length }}</b> {{ t('tr.chars') }}</span>
        <span class="meta"><b>{{ inputBytes }}</b> {{ t('tr.wBytes') }} · UTF-8</span>
        <span v-if="busy" class="meta busy">···</span>
      </div>
      <textarea v-model="trInput" class="ta" spellcheck="false" :placeholder="t('tr.inputPh')" />
    </div>

    <div class="out">
      <div class="ph">
        <span class="tt">{{ t('tr.results') }}</span>
        <span class="meta">{{ groups.length }}</span>
      </div>

      <div class="rows">
        <div v-if="!groups.length" class="empty">{{ t('tr.emptyOut') }}</div>

        <div v-for="g in groups" :key="g.name" class="grp">
          <div class="gh">
            <span class="gn">{{ g.name }}</span>
            <span v-if="g.note" class="gnote">{{ g.note }}</span>
          </div>

          <div v-for="(r, i) in g.rows" :key="i" class="r" :class="{ hex: r.hex }">
            <span class="k" :title="r.how">{{ r.label }}</span>
            <span class="v" :class="{ none: !r.value }">{{ r.value || '—' }}</span>
            <span class="ops">
              <button class="op" :title="t('tr.useAsInput')" :disabled="!r.value" @click="useAsInput(r.value)">
                <svg class="ico" viewBox="0 0 24 24"><path d="M20 12H6M11 6l-6 6 6 6" /></svg>
              </button>
              <button class="op" :title="t('lst.copy')" :disabled="!r.value" @click="copy(r.value)">
                <svg class="ico" viewBox="0 0 24 24"><rect x="9" y="9" width="11" height="11" /><path d="M5 15V5h10" /></svg>
              </button>
            </span>
          </div>
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

/*
  ⚠️ `flex: 1 1 0`：假想宽度为 0，这句提示<b>永远不会</b>让工具条折行 —— 放不下就省略号（完整的在悬停提示里）。
  原来是默认的 `0 1 auto`，假想宽度 = 整句的长度，125% 缩放下它自己占一行、「清空」再占一行（2026-09-11 改）。
  它顺带接替了原来那个 .grow：吃掉剩余宽度，把「清空」顶到最右。
*/
.lb {
  flex: 1 1 0;
  font-size: var(--fs-small);
  color: var(--dim2);
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  min-width: 0;
}

/* 里面的 .ta 没有自己的边框，焦点由这层容器表示 —— 与输入框同一种语言 */
.src:focus-within,
.out:focus-within { border-color: var(--cyan); }
.ta:focus-visible { outline: none; }

.src,
.out {
  min-height: 0;
  display: flex;
  flex-direction: column;
  border: 1px solid var(--border);
  background: var(--sink);
}

.src { flex: 0 0 130px; }
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
  color: var(--th-fg);
}

.ph > span { padding-top: 2px; }   /* 原 4px 在字体度量覆写之后偏低 1px（2026-09-13 字体度量覆写后按 100% 缩放实测重调） */
.ph .tt { color: var(--cyan); }
.ph .meta { color: var(--muted); letter-spacing: .06em; text-transform: none; }
.ph .meta b { font-family: var(--mono); color: var(--gray); font-weight: 400; }
.ph .busy { color: var(--cyan); letter-spacing: .2em; }

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
  font-size: var(--fs-body);
  line-height: 1.6;
}

.ta::placeholder { color: var(--dim); }

.rows { flex: 1; min-height: 0; overflow-y: auto; }

/* ── 一组 = 一种编码，组里是「文本 / 十六进制」两行 ── */

.grp { border-bottom: 1px solid var(--border); }
.grp:last-child { border-bottom: 0; }

.gh {
  display: flex;
  align-items: baseline;
  gap: 12px;
  padding: 7px 14px 3px;
}

.gn {
  font-family: var(--share);
  font-size: var(--fs-label);
  letter-spacing: .14em;
  color: var(--cyan);
}

.gnote { font-size: var(--fs-small); color: var(--dim2); }

.r {
  display: grid;
  grid-template-columns: 96px minmax(0, 1fr) 64px;
  align-items: center;
  gap: 12px;
  min-height: 30px;
  padding: 3px 14px 3px 26px;
}

.r:last-child { padding-bottom: 8px; }
.r:hover { background: rgb(var(--tint-rgb) / 3%); }

/*
  文本行亮、十六进制行压一档 —— 一组两行，字色分层比再画一条线省地方。
  标签带 title（那句算式），所以给个 help 光标提示它可以停一下。
*/
.k {
  font-family: var(--share);
  font-size: var(--fs-label);
  letter-spacing: .1em;
  text-transform: uppercase;
  color: var(--muted);
  cursor: help;
}

.v {
  min-width: 0;
  font-family: var(--mono);
  font-size: var(--fs-body);
  line-height: 1.5;
  color: var(--acc-green2);
  word-break: break-all;
  white-space: pre-wrap;
  user-select: text;
}

.r.hex .v { color: var(--dim3); }
.v.none { color: var(--dim); }

.ops { display: flex; align-items: center; justify-content: center; gap: 4px; }

.empty { padding: 40px 20px; text-align: center; color: var(--muted); font-size: var(--fs-body); line-height: 1.8; }
</style>
