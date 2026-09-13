<script setup lang="ts">
/*
  异或计算 —— 对应 WinForms 的 Controls/XORCalculation（两块 HexBox + 一个异或值输入）。

  ══ 2026-09-08 整屏重做，先说清为什么 ══

  老版本只有「数据 ⊕ 一个十六进制密钥」，而且有三处真问题：

    ① <b>结果会过期而且没人说。</b> 结果存在 store 的一个 ref 里、只有按「计算」才写一次 ——
       算完之后改源数据或改密钥，右边仍举着<b>上一次</b>的结果，界面上一点提示都没有。
       现在结果是 computed，改什么都当场跟着变，这一类 bug 从根上没有了（按钮也就不需要了）。
    ② <b>密钥只认「全带空格」或「全不带空格」。</b> 「4142 4344」这种再正常不过的写法
       会被判成格式错误 —— 分词之后每段是 4 个字符，过不了「一到两位」那条正则。
       现在与 C# 的 HexToBytes 同一条规则：把空白与 - : 剔掉、要求偶数位、两位一字节。
       <b>口径一致本身就是价值</b>：同一串字符贴进滤镜包头、自动入库、这里，含义必须一样。
    ③ <b>只能与密钥异或。</b> 抓包里最常做的其实是<b>两段数据互相异或</b> ——
       两条只差一点的封包异或一下，非零的字节就是变了的那几个；
       拿「明文 + 密文」异或则直接把密钥掏出来。这一版把它做成第二种模式。

  另外密钥可以按<b>文本</b>写（Latin-1，逐字符一个字节）—— 真实的异或密钥十有八九是
  一串可见字符，先手工查一遍 ASCII 表再填十六进制是白费事。
  ⚠️ 用 Latin-1 而不是 UTF-8，是为了与十六进制视图的字符栏、与 HexView 的字符输入<b>同一条口径</b>：
  这个程序里「一个字符 = 一个字节」是贯穿的约定，为这一处破例只会多一处要记的例外。
*/
import { computed } from 'vue'
import { call } from '../../bridge'
import { t } from '../../i18n'
import { pushToast } from '../../stores/toast'
import { xorSrc, xorB, xorKey, xorMode, xorKeyFmt } from '../../stores/tools'
import HexView from '../HexView.vue'

const EMPTY = new Uint8Array(0)

/* ── 密钥 ──────────────────────────────────────────────────── */

/**
 * 解析密钥。返回 null = 写法不合法（与「还没填」要分开，界面上说的话不一样）。
 *
 * ⚠️ 十六进制这一路<b>逐字照抄 C# 的 SystemConfig.HexToBytes</b>：剔掉空白与 - :、
 * 必须偶数位、两位一字节。别自己另发明一套 —— 同一串字符在滤镜包头与这里必须是同一个意思。
 */
function parseKey(): Uint8Array | null {
  const raw = xorKey.value
  if (!raw.trim()) return null

  if (xorKeyFmt.value === 'text') {
    const out = new Uint8Array(raw.length)
    for (let i = 0; i < raw.length; i++) {
      const c = raw.charCodeAt(i)
      if (c > 0xff) return null      //超出 Latin-1，一个字符装不进一个字节
      out[i] = c
    }
    return out.length ? out : null
  }

  const h = raw.replace(/[\s\-:]/g, '')
  if (!h.length || h.length % 2 || !/^[0-9a-fA-F]+$/.test(h)) return null

  const out = new Uint8Array(h.length / 2)
  for (let i = 0; i < out.length; i++) out[i] = parseInt(h.substr(i * 2, 2), 16)
  return out
}

const keyBytes = computed(() => parseKey())
const keyTyped = computed(() => xorKey.value.trim().length > 0)
const keyBad = computed(() => keyTyped.value && !keyBytes.value)

/* ── 结果 ──────────────────────────────────────────────────── */

/**
 * ⚠️ 两段长度不同时按 <b>00 补齐</b>，不是截到短的那一段。
 *
 * 异或 00 就是原样带过，所以补齐既不丢数据也不造数据；截断则会把长出来的那一截
 * 悄悄扔掉 —— 而「B 比 A 长出来的部分」恰恰常常正是要看的东西。界面上另有一句提示说明。
 */
const outBytes = computed<Uint8Array>(() => {
  const a = xorSrc.value

  if (xorMode.value === 'b') {
    const b = xorB.value
    const n = Math.max(a.length, b.length)
    if (!n) return EMPTY

    const o = new Uint8Array(n)
    for (let i = 0; i < n; i++) o[i] = (i < a.length ? a[i] : 0) ^ (i < b.length ? b[i] : 0)
    return o
  }

  const k = keyBytes.value
  if (!a.length || !k) return EMPTY

  const o = new Uint8Array(a.length)
  for (let i = 0; i < a.length; i++) o[i] = a[i] ^ k[i % k.length]
  return o
})

/** 有几个字节真的不一样。密钥模式比「结果 vs 源」，数据模式比「A vs B」（那才是这个模式的用途）。 */
const diffCount = computed(() => {
  const l = xorSrc.value
  const r = xorMode.value === 'b' ? xorB.value : outBytes.value
  if (!outBytes.value.length) return 0

  let n = 0
  const max = Math.max(l.length, r.length)
  for (let i = 0; i < max; i++) {
    if ((i < l.length ? l[i] : 0) !== (i < r.length ? r[i] : 0)) n++
  }
  return n
})

const lenMismatch = computed(() =>
  xorMode.value === 'b' && xorSrc.value.length > 0 && xorB.value.length > 0 &&
  xorSrc.value.length !== xorB.value.length)

/** 密钥要铺几轮才盖满源数据 —— 「够不够长」是填密钥时唯一想知道的事 */
const tiles = computed(() => {
  const k = keyBytes.value
  if (!k || !k.length || !xorSrc.value.length) return 0
  return Math.ceil(xorSrc.value.length / k.length)
})

/* ── 动作 ──────────────────────────────────────────────────── */

function hexOf(b: Uint8Array): string {
  let s = ''
  for (let i = 0; i < b.length; i++) s += (i ? ' ' : '') + b[i].toString(16).toUpperCase().padStart(2, '0')
  return s
}

/** 从剪贴板读一串十六进制。右键菜单里也有粘贴，这里是给不想右键的人一个按钮。 */
async function pasteInto(which: 'a' | 'b'): Promise<void> {
  try {
    const r = await call<{ text: string }>('clipboardRead')
    const txt = (r?.text ?? '').replace(/[^0-9a-fA-F]/g, '')
    if (!txt) { pushToast('warning', t('xo.clipNoHex')); return }

    //奇数位就丢掉最后半个字节 —— 半个字节没法成字节，留着只会算出个错的
    const pairs = txt.length % 2 ? txt.slice(0, -1) : txt
    const bytes = new Uint8Array((pairs.match(/../g) ?? []).map((h) => parseInt(h, 16)))

    if (which === 'a') xorSrc.value = bytes
    else xorB.value = bytes
  } catch (e) {
    console.error('[xo] 读剪贴板失败', e)
  }
}

async function copyOut(): Promise<void> {
  if (!outBytes.value.length) return
  try {
    await call('clipboardWrite', { text: hexOf(outBytes.value) })
    pushToast('success', t('pm.copied'))
  } catch (e) {
    console.error('[xo] 写剪贴板失败', e)
  }
}

/*
  结果转入源，接着换个密钥再异或一次。

  ⚠️ <b>顺手要把密钥清掉。</b> 结果是实时算的，只搬不清的话下一拍立刻拿同一个密钥再异或回去 ——
  屏幕上看到的是「点了一下什么都没变」（异或两次等于没异或），比不做还费解。
*/
function useOut(): void {
  if (!outBytes.value.length) return
  xorSrc.value = outBytes.value
  xorKey.value = ''
}

function clearAll(): void {
  xorSrc.value = EMPTY
  xorB.value = EMPTY
  xorKey.value = ''
}

const anything = computed(() => xorSrc.value.length > 0 || xorB.value.length > 0 || xorKey.value.length > 0)
</script>

<template>
  <div class="page list-page xo">
    <div class="bar">
      <div class="hx-seg" :title="t('xo.modeHint')">
        <button class="hx-segb after" :class="{ on: xorMode === 'key' }" @click="xorMode = 'key'">{{ t('xo.modeKey') }}</button>
        <button class="hx-segb before" :class="{ on: xorMode === 'b' }" @click="xorMode = 'b'">{{ t('xo.modeB') }}</button>
      </div>

      <template v-if="xorMode === 'key'">
        <span class="lb">{{ t('xo.key') }}</span>
        <!--
          写错了：框变红 + 悬停提示给原因。工具条上原来另有一行红字（.kerr），2026-09-11 去掉了 ——
          中间的密钥面板本来就用同一句话说着这件事，而那行红字一出现就把整条工具条挤成两行，
          敲着字工具条跟着上下跳。
        -->
        <input v-model="xorKey" class="inp key" :class="{ bad: keyBad }" spellcheck="false"
               :title="keyBad ? (xorKeyFmt === 'hex' ? t('xo.keyBad') : t('xo.keyBadText')) : (xorKeyFmt === 'hex' ? t('xo.keyTipHex') : t('xo.keyTipText'))"
               :placeholder="xorKeyFmt === 'hex' ? t('xo.keyPhHex') : t('xo.keyPhText')">
        <div class="hx-seg">
          <button class="hx-segb after" :class="{ on: xorKeyFmt === 'hex' }" @click="xorKeyFmt = 'hex'">{{ t('hex.asHex') }}</button>
          <button class="hx-segb before" :class="{ on: xorKeyFmt === 'text' }" @click="xorKeyFmt = 'text'">{{ t('hex.asText') }}</button>
        </div>
      </template>

      <span class="grow" />

      <button class="btn" @click="pasteInto('a')">{{ t('xo.pasteHex') }}</button>
      <button v-if="xorMode === 'b'" class="btn" @click="pasteInto('b')">{{ t('xo.pasteB') }}</button>
      <button class="btn" :disabled="!outBytes.length" @click="copyOut">{{ t('xo.copyOut') }}</button>
      <button v-if="xorMode === 'key'" class="btn warn" :disabled="!outBytes.length" :title="t('xo.useOutHint')"
              @click="useOut">{{ t('xo.useOut') }}</button>
      <button class="btn danger" :disabled="!anything" @click="clearAll">{{ t('rb.clearAll') }}</button>
    </div>

    <div class="panes" :class="xorMode === 'b' ? 'three' : 'keyed'">
      <!-- 源 A -->
      <div class="hexed">
        <div class="hx-bar">
          <span class="hx-title">{{ t('xo.src') }}</span>
          <span class="hx-meta">{{ xorSrc.length }} {{ t('hex.bytes') }}</span>
          <span class="hx-meta dim">{{ t('xo.srcHint') }}</span>
        </div>
        <div v-if="!xorSrc.length" class="empty">{{ t('xo.emptySrc') }}</div>
        <HexView v-else v-model:bytes="xorSrc" />
      </div>

      <!-- 中间：密钥面板 / 数据 B -->
      <div v-if="xorMode === 'key'" class="keypane">
        <div class="hx-bar">
          <span class="hx-title">{{ t('xo.key') }}</span>
          <span class="hx-meta">{{ keyBytes ? keyBytes.length : 0 }} {{ t('hex.bytes') }}</span>
        </div>

        <div class="kbody">
          <div v-if="!keyTyped" class="empty small">{{ t('xo.keyEmpty') }}</div>
          <div v-else-if="!keyBytes" class="empty small bad">{{ xorKeyFmt === 'hex' ? t('xo.keyBad') : t('xo.keyBadText') }}</div>

          <template v-else>
            <!-- 密钥字节本身。文本模式下顺带把它对应的字节摊开，省得再去查一遍 ASCII -->
            <div class="chips">
              <span v-for="(v, i) in Array.from(keyBytes)" :key="i" class="chip">
                {{ v.toString(16).toUpperCase().padStart(2, '0') }}
              </span>
            </div>

            <div v-if="tiles" class="tile">
              <span class="k">{{ t('xo.keyTile') }}</span>
              <span class="v">{{ tiles }} {{ t('xo.rounds') }}</span>
            </div>
          </template>
        </div>

        <div class="op" aria-hidden="true">
          <svg viewBox="0 0 24 24"><path d="M4 12h14M13 6l6 6-6 6" /></svg>
          <span>XOR</span>
        </div>
      </div>

      <div v-else class="hexed">
        <div class="hx-bar">
          <span class="hx-title b">{{ t('xo.dataB') }}</span>
          <span class="hx-meta">{{ xorB.length }} {{ t('hex.bytes') }}</span>
        </div>
        <div v-if="!xorB.length" class="empty">{{ t('xo.emptyB') }}</div>
        <HexView v-else v-model:bytes="xorB" />
      </div>

      <!-- 结果 -->
      <div class="hexed">
        <div class="hx-bar">
          <span class="hx-title out">{{ t('xo.out') }}</span>
          <span class="hx-meta">{{ outBytes.length }} {{ t('hex.bytes') }}</span>
          <span v-if="outBytes.length" class="hx-meta hit">
            {{ xorMode === 'b' ? t('xo.diff') : t('xo.changed') }} {{ diffCount }}
          </span>
          <span v-if="lenMismatch" class="hx-meta pad" :title="t('xo.padHint')">00 →</span>
        </div>
        <div v-if="!outBytes.length" class="empty">{{ xorMode === 'b' ? t('xo.emptyOutB') : t('xo.emptyOutKey') }}</div>
        <HexView v-else :bytes="outBytes" readonly />
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

.lb { flex: none; font-size: var(--fs-body); color: var(--muted); white-space: nowrap; }

/* 基样式在 style.css 的 .inp，这里只补布局 */
/*
  工具条上的输入框跟着同一行的按钮走（`--btn-size`），与数据页工具条的搜索框同一条口径 ——
  12.5px 夹在一排 10.5px 的按钮与分段按钮中间，提示文字是全场最大的那个（2026-09-11 按要求改）。
  框高仍是 .inp 那 28px，与按钮齐平。
*/
/*
  ⚠️ 原来是 `flex: 0 1 340px; min-width: 150px` —— flex-wrap 按「假想宽度」断行，而那个 340 就是假想宽度，
  125% 缩放下（1024 CSS 宽）整条差 7px 就放不下，右边四颗按钮全被甩到第二行（2026-09-11 按要求收成一行）。
  现在假想宽度是 0 → 夹到 min-width 110：宽度够时长到 340 封顶（剩下的给 .grow），不够时一路让到 110。
*/
.inp.key { flex: 1 1 0; min-width: 110px; max-width: 340px; font-size: var(--btn-size); }
.inp.key.bad { border-color: var(--danger); color: var(--danger); }

/*
  ⚠️ 密钥那一栏刻意<b>比两块数据窄</b>（240px 定宽）—— 它装的是几个字节，
  给它三分之一屏宽只会让两块真正要读的十六进制各少一列。
  数据 B 模式下三块是同等地位的数据，就三等分。
*/
.panes {
  flex: 1;
  min-height: 0;
  display: grid;
  gap: 8px;
}

.panes.keyed { grid-template-columns: 1fr 240px 1fr; }
.panes.three { grid-template-columns: 1fr 1fr 1fr; }

/* 与封包编辑的十六进制外框同一套 */
.hexed,
.keypane {
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
  padding-top: 2px;   /* 原 4px 在字体度量覆写之后补过头 1px（2026-09-13 字体度量覆写后按 100% 缩放实测重调） */
  font-family: var(--share);
  font-size: var(--th-size);
  letter-spacing: .14em;
  text-transform: uppercase;
  color: var(--th-fg);
}

/* 三块各给一个色相：源灰、B 琥珀、结果绿 —— 扫一眼就知道在看哪一块 */
.hx-title.b { color: var(--amber); }
.hx-title.out { color: var(--green); }

.hx-meta { padding-top: 2px; font-family: Consolas, monospace; font-size: var(--th-size); color: var(--gray); }
.hx-meta.dim { color: var(--muted); overflow: hidden; text-overflow: ellipsis; }
.hx-meta.hit { color: var(--amber); }
.hx-meta.pad { color: var(--dim3); cursor: help; }

/* ── 密钥面板 ── */

.kbody {
  flex: 1;
  min-height: 0;
  overflow-y: auto;
  padding: 10px;
}

.chips { display: flex; flex-wrap: wrap; gap: 4px; }

.chip {
  padding: 3px 5px 2px;
  border: 1px solid rgb(var(--cyan-rgb) / 30%);
  background: rgb(var(--inset-rgb) / 30%);
  font-family: Consolas, monospace;
  font-size: var(--fs-dense);
  line-height: 1.2;
  color: var(--cyan);
}

.tile {
  display: flex;
  align-items: baseline;
  gap: 8px;
  margin-top: 10px;
  padding-top: 8px;
  border-top: 1px solid rgb(var(--border-rgb) / 60%);
}

.tile .k { font-family: var(--share); font-size: var(--fs-label); letter-spacing: .1em; text-transform: uppercase; color: var(--muted); }
.tile .v { font-family: Consolas, monospace; font-size: var(--fs-dense); color: var(--gray); }

/* 运算符钉在密钥栏底部：它说的是「这一栏与左边那块做异或」 */
.op {
  flex: none;
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 8px;
  padding: 8px;
  border-top: 1px solid var(--border);
  color: var(--cyan);
  font-family: var(--share);
  font-size: var(--fs-label);
  letter-spacing: .14em;
  opacity: .8;
}

.op svg { width: 18px; height: 18px; stroke: currentColor; stroke-width: 1.8; fill: none; }

.empty {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 20px;
  text-align: center;
  color: var(--muted);
  font-size: var(--fs-body);
  line-height: 1.8;
}

.empty.small { padding: 10px 2px; font-size: var(--fs-small); }
.empty.bad { color: var(--danger); }
</style>
