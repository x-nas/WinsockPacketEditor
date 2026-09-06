<script setup lang="ts">
/*
  查看数据修改 —— 对应 WinForms 的 Controls/PacketModification。

  一条封包经过滤镜之后，原始字节（RawBuffer）与改写后的字节（PacketBuffer）各一份，
  这一屏把两者摆在一起：上面是共用的 HexView（只读，compare 给另一份，不同的字节标琥珀），
  可以切「改写前 / 改写后」看任一份；下面是一张差异表，点一行在上面高亮那一段。

  【差异怎么算】按位置逐字节比对，把相邻的不同字节并成一段 —— 与 HexView 的高亮同一条规则，
  表里的段和上面亮着的段一一对应。长度不同时多出来的那一截记为「新增」、少掉的记为「删除」。
  WinForms 那边是把两份字节转成十六进制文本再跑 DiffPlex，给出的位置是文本里的字符位，
  跳转也是跳到文本框的字符位；这里位置一律是字节偏移，更直接。

  【字节从哪来】与代理数据页的面板同一个桥方法 getPacketDetail（改写前后两份 + 是否改过），
  所以只有代理数据列表里的封包能看 —— 发送集里的封包没有「改写前」这回事。
*/
import { computed, nextTick, ref, watch } from 'vue'
import { call } from '../../bridge'
import { FeedList, type PacketDetail } from '../../bridge/types'
import { t } from '../../i18n'
import HexView from '../HexView.vue'

const props = withDefaults(
  defineProps<{ id: number | null; list?: 'proxy' | 'packet' }>(),
  { list: 'proxy' },
)
const emit = defineEmits<{ (e: 'close'): void }>()

const detail = ref<PacketDetail | null>(null)
const loaded = ref(false)
const error = ref('')
const showRaw = ref(false)

function toBytes(b64: string | null | undefined): Uint8Array {
  if (!b64) return new Uint8Array(0)
  const bin = atob(b64)
  const out = new Uint8Array(bin.length)
  for (let i = 0; i < bin.length; i++) out[i] = bin.charCodeAt(i)
  return out
}

const raw = computed(() => toBytes(detail.value?.raw))
const packet = computed(() => toBytes(detail.value?.packet))
const shown = computed(() => (showRaw.value ? raw.value : packet.value))
const other = computed(() => (detail.value?.modified ? (showRaw.value ? packet.value : raw.value) : null))

/* ── 差异表 ─────────────────────────────────────────────────── */

interface Diff { pos: number; count: number; from: string; to: string; kind: 'modified' | 'inserted' | 'deleted' }

const HEX = '0123456789ABCDEF'
function hexOf(a: Uint8Array, lo: number, hi: number): string {
  let s = ''
  for (let i = lo; i < hi; i++) s += (i > lo ? ' ' : '') + HEX[a[i] >> 4] + HEX[a[i] & 15]
  return s
}

const diffs = computed<Diff[]>(() => {
  const a = raw.value, b = packet.value
  const out: Diff[] = []
  const n = Math.min(a.length, b.length)

  //相邻的不同字节并成一段
  let i = 0
  while (i < n) {
    if (a[i] === b[i]) { i++; continue }
    let j = i
    while (j < n && a[j] !== b[j]) j++
    out.push({ pos: i, count: j - i, from: hexOf(a, i, j), to: hexOf(b, i, j), kind: 'modified' })
    i = j
  }

  if (b.length > n) out.push({ pos: n, count: b.length - n, from: '', to: hexOf(b, n, b.length), kind: 'inserted' })
  if (a.length > n) out.push({ pos: n, count: a.length - n, from: hexOf(a, n, a.length), to: '', kind: 'deleted' })

  return out
})

const hv = ref<InstanceType<typeof HexView> | null>(null)
const picked = ref(-1)

/** 点一行：在上面高亮那一段。「删除」那段只存在于原始那份，先切过去再选 */
async function jump(d: Diff, i: number): Promise<void> {
  picked.value = i
  if (d.kind === 'deleted' && !showRaw.value) showRaw.value = true
  if (d.kind === 'inserted' && showRaw.value) showRaw.value = false
  //切换后 HexView 要先重绘再选。用 nextTick 而不是 rAF：窗口被遮住时 Chromium 不发 rAF，逻辑不能挂在它上面
  await nextTick()
  hv.value?.selectRange(d.pos, d.pos + d.count)
}

function offset(i: number): string { return i.toString(16).toUpperCase().padStart(8, '0') }

/* ── 打开 / 关闭 ────────────────────────────────────────────── */

watch(() => props.id, async (id) => {
  if (id === null) return

  loaded.value = false
  error.value = ''
  detail.value = null
  showRaw.value = false
  picked.value = -1

  try {
    const d = await call<PacketDetail | null>('getPacketDetail', {
      id,
      list: props.list === 'packet' ? FeedList.Packet : FeedList.Proxy,
    })
    if (!d) { error.value = t('hex.gone'); return }
    detail.value = d
    loaded.value = true
  } catch (e) {
    console.error('[pmd] 打开失败', e)
    error.value = String(e)
  }
}, { immediate: true })

function close(): void { emit('close') }
</script>

<template>
  <div v-if="props.id !== null" class="editor-mask" @mousedown.self="close">
    <div class="dlg" role="dialog" aria-modal="true" @keydown.esc="close">
      <span class="mk tl" /><span class="mk tr" /><span class="mk bl" /><span class="mk br" />

      <header class="hd">
        <div class="tt">
          <span class="zh">{{ t('pmd.title') }}</span>
          <span class="sub">Controls/PacketModification</span>
        </div>
        <button class="x" :title="t('dlg.close')" @click="close">
          <svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg>
        </button>
      </header>

      <div v-if="!loaded" class="loading">{{ error || t('proxy.working') }}</div>

      <div v-else-if="detail" class="bd">
        <!-- 两份的长度，与 WinForms 的两行标题同一内容 -->
        <div class="info">
          <span class="k">{{ t('col.id') }} <b>{{ detail.id }}</b></span>
          <span class="k">{{ t('pmd.raw') }} <b>{{ raw.length }}</b> {{ t('pmd.len') }}</span>
          <span class="k">{{ t('pmd.new') }} <b class="new">{{ packet.length }}</b> {{ t('pmd.len') }}</span>
          <span class="grow" />
          <span v-if="!detail.modified" class="same">{{ t('pmd.same') }}</span>
        </div>

        <div class="hexed">
          <div class="hx-bar">
            <span class="hx-title">{{ t('hex.title') }}</span>
            <span class="hx-meta">{{ shown.length }} {{ t('hex.bytes') }}</span>
            <div class="hx-seg">
              <button class="hx-segb after" :class="{ on: !showRaw }" @click="showRaw = false">{{ t('hex.after') }}</button>
              <button class="hx-segb before" :class="{ on: showRaw }" @click="showRaw = true">{{ t('hex.before') }}</button>
            </div>
          </div>
          <HexView ref="hv" :bytes="shown" :compare="other" readonly />
        </div>

        <!-- 差异表 -->
        <div class="tbl list-page">
          <div class="cap">
            <span>{{ t('pmd.diffs') }} <b>{{ diffs.length }}</b></span>
            <span class="dim">{{ t('pmd.jumpHint') }} · {{ t('pmd.algoHint') }}</span>
          </div>
          <div class="tbody">
            <div class="head">
              <span class="no">{{ t('col.id') }}</span>
              <span class="pos">{{ t('pmd.pos') }}</span>
              <span class="cnt">{{ t('pmd.count') }}</span>
              <span class="kind">{{ t('pmd.kind') }}</span>
              <span class="hx">{{ t('pmd.from') }}</span>
              <span class="hx">{{ t('pmd.to') }}</span>
            </div>
            <div v-if="!diffs.length" class="empty">{{ t('pmd.same') }}</div>
            <div v-for="(d, i) in diffs" v-else :key="i" class="row2" :class="{ sel: picked === i }" @click="jump(d, i)">
              <span class="no">{{ i + 1 }}</span>
              <span class="pos">{{ offset(d.pos) }} <i>({{ d.pos }})</i></span>
              <span class="cnt">{{ d.count }}</span>
              <span class="kind"><span class="tg" :class="d.kind">{{ t(d.kind === 'modified' ? 'pmd.modified' : d.kind === 'inserted' ? 'pmd.inserted' : 'pmd.deleted') }}</span></span>
              <span class="hx" :title="d.from">{{ d.from || '—' }}</span>
              <span class="hx" :title="d.to">{{ d.to || '—' }}</span>
            </div>
          </div>
        </div>
      </div>

      <footer class="ft">
        <span v-if="loaded && error" class="err">{{ error }}</span>
        <span class="grow" />
        <button class="btn primary" @click="close">{{ t('dlg.close') }}</button>
      </footer>
    </div>
  </div>
</template>

<style scoped>
.hd {
  flex: none;
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 14px 18px;
  border-bottom: 1px solid var(--border);
  background: var(--panel);
}

.tt { flex: 1; min-width: 0; display: flex; align-items: baseline; gap: 12px; }
.tt .zh { font-family: var(--orbit); font-size: 14px; color: var(--gray); letter-spacing: .04em; }
.tt .sub { font-family: var(--share); font-size: 10px; letter-spacing: .14em; text-transform: uppercase; color: var(--dim); }

.x {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 26px;
  height: 26px;
  background: transparent;
  border: 1px solid transparent;
  color: var(--muted);
  cursor: pointer;
}

.x:hover { border-color: var(--danger); color: var(--danger); }
.x .ico { width: 15px; height: 15px; stroke: currentColor; stroke-width: 2; fill: none; }

.loading { padding: 60px 0; text-align: center; color: var(--muted); font-size: 12.5px; }

.bd {
  flex: 1;
  min-height: 0;
  display: flex;
  flex-direction: column;
  gap: 8px;
  overflow: hidden;
  padding: 10px 18px 10px;
}

.info { flex: none; display: flex; align-items: center; gap: 22px; font-size: 12.5px; color: var(--muted); }
.info b { font-family: var(--mono); color: var(--cyan); font-weight: 400; }
.info b.new { color: var(--green); }
.info .grow { flex: 1; }
.info .same { color: var(--dim2); font-size: 11.5px; }

/* 十六进制：外框 + 标题栏归这里，正文是共用的 HexView。上下两块 3 : 2 分高度 */
.hexed {
  flex: 3;
  min-height: 160px;
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
}

.hx-title { padding-top: 4px; font-family: var(--share); font-size: var(--th-size); letter-spacing: .14em; text-transform: uppercase; color: var(--th-fg); }
.hx-meta { padding-top: 2px; font-family: Consolas, monospace; font-size: var(--th-size); color: var(--gray); }

/* 改写前 / 改写后：与代理数据页的面板同一对按钮 */
.hx-seg { display: flex; border: 1px solid var(--border); }

.hx-segb {
  padding: 7px 10px 5px;
  background: transparent;
  border: 0;
  font-family: var(--share);
  font-size: var(--btn-size);
  line-height: 1;
  letter-spacing: .12em;
  text-transform: uppercase;
  cursor: pointer;
  white-space: nowrap;
}

.hx-segb + .hx-segb { border-left: 1px solid var(--border); }
.hx-segb.after { color: #5aa080; }
.hx-segb.before { color: #5b93a6; }
.hx-segb.after.on { color: var(--green); background: rgb(var(--green-rgb) / 12%); }
.hx-segb.before.on { color: var(--cyan); background: rgb(var(--cyan-rgb) / 12%); }

/* 差异表 */
.tbl {
  flex: 2;
  min-height: 120px;
  display: flex;
  flex-direction: column;
  border: 1px solid var(--border);
  background: var(--sink);
}

.cap {
  flex: none;
  display: flex;
  align-items: center;
  gap: 14px;
  height: var(--th-h);
  padding: 4px 12px 0;   /* 文字直接放在 flex 容器里，补偿只能打在容器上：字形偏上 2px（像素实测），上内边距 4px 把内容框中心压下 2px */
  background: var(--panel);
  border-bottom: 1px solid var(--border);
  font-family: var(--share);
  font-size: var(--th-size);
  letter-spacing: .14em;
  text-transform: uppercase;
  color: var(--th-fg);
  white-space: nowrap;
  overflow: hidden;
}

.cap b { font-family: var(--mono); color: var(--cyan); font-weight: 400; }
.cap .dim { color: var(--dim); letter-spacing: .02em; text-transform: none; overflow: hidden; text-overflow: ellipsis; }

.tbody { flex: 1; min-height: 0; overflow: auto; }

.head,
.row2 {
  display: grid;
  grid-template-columns: 44px 150px 60px 80px minmax(120px, 1fr) minmax(120px, 1fr);
  align-items: center;
  gap: 8px;
  padding: 0 14px;   /* 必须与 style.css 里 .list-page .head 的 14px 一致 */
  font-size: 12.5px;
}

.row2 { height: 30px; border-bottom: 1px solid rgb(var(--border-rgb) / 45%); color: var(--soft); cursor: pointer; }
.row2:hover { background: rgb(var(--tint-rgb) / 4%); }
.row2.sel { background: rgb(var(--cyan-rgb) / 6%); box-shadow: inset 2px 0 0 var(--cyan); }
.row2 > span, .head > span { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }

.head > span, .row2 > span { text-align: center; }
.head > span.hx, .row2 > span.hx { text-align: left; }
.head > span.kind, .row2 > span.kind { display: flex; align-items: center; justify-content: center; }

.no { color: var(--dim); font-variant-numeric: tabular-nums; }
.pos { font-family: var(--mono); color: var(--cyan); }
.pos i { font-style: normal; color: var(--dim); }
.cnt { font-family: var(--mono); color: var(--dim3); }
.hx { font-family: var(--mono); font-size: 12px; color: var(--acc-green2); }

.tg {
  padding: 5px 6px 3px;
  border: 1px solid;
  font-family: var(--share);
  font-size: 10.5px;
  line-height: 1;
  letter-spacing: .04em;
}

.tg.modified { border-color: rgb(var(--amber-rgb) / 35%); color: var(--amber); }
.tg.inserted { border-color: rgb(var(--green-rgb) / 35%); color: var(--green); }
.tg.deleted { border-color: rgb(var(--danger-rgb) / 30%); color: var(--danger); }

.empty { padding: 26px 0; text-align: center; color: var(--muted); font-size: 12.5px; }

.ft {
  flex: none;
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 12px 18px;
  border-top: 1px solid var(--border);
  background: var(--panel);
}

.ft .grow { flex: 1; }
.err { font-size: 12px; color: var(--danger); }

.btn {
  padding: 9px 13px 7px;
  background: transparent;
  border: 1px solid var(--border);
  color: var(--gray);
  font-family: var(--share);
  font-size: var(--btn-size);
  line-height: 1;
  letter-spacing: .12em;
  text-transform: uppercase;
  cursor: pointer;
}

.btn.primary { border-color: rgb(var(--green-rgb) / 45%); color: var(--green); }
.btn.primary:hover { background: rgb(var(--green-rgb) / 10%); border-color: var(--green); }
</style>
