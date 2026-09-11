<script setup lang="ts">
/*
  查看数据修改 —— 对应 WinForms 的 Controls/PacketModification。

  一条封包经过滤镜之后，原始字节（RawBuffer）与改写后的字节（PacketBuffer）各一份，
  这一屏把两者<b>并排</b>摆出来：上面是共用的 DiffView（与「文本对比」那一页同一个组件），
  左「改写前」右「改写后」；下面是一张差异表，点一行滚到上面那一处。

  ⚠️ <b>2026-09-09 从「一个 HexView + 改写前/改写后分段按钮」改成并排。</b>
  用户提的：要看两份数据得先点一下切标签，没法对着比。而这一屏的全部意义就是「对比」——
  把两份摆在同一屏里才谈得上比，切换等于每次只给看一半。

  【差异怎么算】走 <b>src/diff.ts 的 diffBytes（Myers 最短编辑脚本）</b>，与文本对比页同一份实现。

  ⚠️ 这里<b>换掉了原来那套「按位置逐字节比」</b>，不是顺手统一，是并排视图的前提：
  滤镜的 Replace 完全可以改变长度（FilterInfo 允许替换成不等长的内容），
  而按位置比的话，插进一个字节之后<b>后面全部</b>会被报成「修改」——
  两栏也就从那个字节起<b>整体错位</b>，并排反而比切换还难看。
  Myers 会把「插了 N 个字节」认成一块 ins，两侧用占位格顶住，后面继续对齐。
  （文本对比页 2026-09-08 整页重做，病根一模一样，那边记着同一笔账。）

  WinForms 那边是把两份字节转成十六进制文本再跑 DiffPlex，给出的位置是文本里的<b>字符位</b>；
  这里位置一律是<b>字节偏移</b>，更直接。

  【字节从哪来】与代理数据页的面板同一个桥方法 getPacketDetail（改写前后两份 + 是否改过），
  所以只有代理数据列表里的封包能看 —— 发送集里的封包没有「改写前」这回事。
*/
import { computed, ref, watch } from 'vue'
import { call } from '../../bridge'
import { FeedList, type PacketDetail } from '../../bridge/types'
import { diffBytes } from '../../diff'
import { t } from '../../i18n'
import DiffView from './DiffView.vue'
import { useModal } from '../../useModal'

const props = withDefaults(
  defineProps<{ id: number | null; list?: 'proxy' | 'packet' }>(),
  { list: 'proxy' },
)
const emit = defineEmits<{ (e: 'close'): void }>()

const detail = ref<PacketDetail | null>(null)
const loaded = ref(false)
const error = ref('')

function toBytes(b64: string | null | undefined): Uint8Array {
  if (!b64) return new Uint8Array(0)
  const bin = atob(b64)
  const out = new Uint8Array(bin.length)
  for (let i = 0; i < bin.length; i++) out[i] = bin.charCodeAt(i)
  return out
}

const raw = computed(() => toBytes(detail.value?.raw))
const packet = computed(() => toBytes(detail.value?.packet))

/* ── 差异表 ─────────────────────────────────────────────────── */

const dr = computed(() => diffBytes(raw.value, packet.value))
const blocks = computed(() => dr.value.blocks)

interface DiffRow {
  kind: 'modified' | 'inserted' | 'deleted'
  pos: string
  dec: number
  posTitle: string
  count: string
  from: string
  to: string
}

const HEX = '0123456789ABCDEF'

/*
  一段字节转成十六进制。⚠️ <b>封顶 24 个字节</b>，多的写成「… +N」——
  滤镜整包替换时一块就是几千字节，铺开既读不了，挂进 title 也没人看得完
  （与查重表「位置只列前 6 个」同一条口径）。
*/
const HEX_MAX = 24

function hexOf(a: Uint8Array, lo: number, len: number): string {
  if (len <= 0) return ''

  const n = Math.min(len, HEX_MAX)
  let s = ''
  for (let i = 0; i < n; i++) s += (i ? ' ' : '') + HEX[a[lo + i] >> 4] + HEX[a[lo + i] & 15]

  return len > n ? s + ' … +' + (len - n) : s
}

function offset(i: number): string { return i.toString(16).toUpperCase().padStart(8, '0') }

/*
  差异表的行 = blocks 里非 same 的那些，<b>顺序不变</b>。
  ⚠️ 这一点是硬约束：DiffView.scrollToChange(i) 收的正是「第几处差异」，
  它自己也是按块序数出来的（见 DiffView 里的 ci）。这张表一排序或一筛选，点行就跳错地方。
*/
const diffs = computed<DiffRow[]>(() => blocks.value
  .filter((b) => b.op !== 'same')
  .map((b) => {
    //ins 在 A 侧没有长度，位置该报它在 B 里的落点；其余按 A 报
    const main = b.op === 'ins' ? b.bStart : b.aStart

    return {
      kind: b.op === 'mod' ? 'modified' : b.op === 'ins' ? 'inserted' : 'deleted',
      pos: offset(main),
      dec: main,
      posTitle: '改写前 ' + offset(b.aStart) + ' · 改写后 ' + offset(b.bStart),
      //长度两侧可能不同（Myers 的 mod 块不保证等长），不同就两个都写出来
      count: b.aLen === b.bLen ? String(b.aLen) : b.aLen + ' → ' + b.bLen,
      from: hexOf(raw.value, b.aStart, b.aLen),
      to: hexOf(packet.value, b.bStart, b.bLen),
    }
  }))

const dv = ref<InstanceType<typeof DiffView> | null>(null)
const picked = ref(-1)

/** 点一行：把上面的并排视图滚到那一处（DiffView 自己按块序数找行） */
function jump(i: number): void {
  picked.value = i
  dv.value?.scrollToChange(i)
}

/* ── 打开 / 关闭 ────────────────────────────────────────────── */

watch(() => props.id, async (id) => {
  if (id === null) return

  loaded.value = false
  error.value = ''
  detail.value = null
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

/* 登记进模态栈：父窗体因此变 inert；自己被后开的弹窗盖住时也会 inert。见 useModal.ts */
const { covered } = useModal(() => props.id !== null)
</script>

<template>
  <!--
    ⚠️ <b>Teleport 到 body</b> —— 不是为了好看，是必须的，两个理由都在 useModal.ts 里：
    ① 代理模式的 .proxy 是 z-index: 10 的层叠上下文，弹窗留在里面时遮罩盖不住标题栏；
    ② 出去了才不会被 .shell 的 inert 一起禁掉。

    ⚠️ <b>刻意不换行、不重排缩进</b>：模板里有 white-space: pre 的块，
    整体缩进一动，Vue 模板编译器的 condense 会连带改掉渲染结果。

    ⚠️ <b>点遮罩不再关闭弹窗</b>：编辑器里都是填了一半的东西，点空白处就丢掉太容易误操作。
    出口只留「取消 / 关闭」按钮与 Esc。
  -->
  <Teleport to="body"><div v-if="props.id !== null" class="editor-mask" :inert="covered">
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

        <!--
          并排：左「改写前」右「改写后」，与文本对比页共用 DiffView。
          ⚠️ 两栏在<b>同一个滚动容器</b>里（DiffView 的做法），所以横着扫永远对得齐，
          也就不需要「同步滚动」这种开关 —— 两个独立滚动条同步永远差半行。
        -->
        <div class="cmp">
          <div class="rh">
            <div class="rhh">
              <span class="rt">{{ t('hex.before') }}</span>
              <span class="rm">{{ raw.length }} {{ t('hex.bytes') }}</span>
            </div>
            <span class="rhg" />
            <div class="rhh">
              <span class="rt">{{ t('hex.after') }}</span>
              <span class="rm">{{ packet.length }} {{ t('hex.bytes') }}</span>
            </div>
          </div>

          <DiffView ref="dv" mode="hex" :a-bytes="raw" :b-bytes="packet" :blocks="blocks" />
        </div>

        <!-- 差异表 -->
        <div class="tbl list-page">
          <div class="cap">
            <span>{{ t('pmd.diffs') }} <b>{{ diffs.length }}</b></span>
            <!--
              ⚠️ 差异过多被截断时<b>必须说出来</b>：否则用户看到中间一块巨大的「改」，
              却以为算法认真比过了（与统计页那条恒等式校验同一条规矩）。
            -->
            <span v-if="dr.truncated" class="warn">{{ t('tc.tooMany') }}</span>
            <span class="dim" :title="t('pmd.jumpHint') + ' · ' + t('pmd.algoHint')">{{ t('pmd.jumpHint') }} · {{ t('pmd.algoHint') }}</span>
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
            <div v-for="(d, i) in diffs" v-else :key="i" class="row2" :class="{ sel: picked === i }" @click="jump(i)">
              <span class="no">{{ i + 1 }}</span>
              <span class="pos" :title="d.posTitle">{{ d.pos }} <i>({{ d.dec }})</i></span>
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
  </div></Teleport>
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
.tt .zh { font-family: var(--orbit); font-weight: 700; font-size: var(--fs-title); color: var(--gray); letter-spacing: .04em; }
.tt .sub { font-family: var(--share); font-size: var(--fs-caption); letter-spacing: .14em; text-transform: uppercase; color: var(--dim); }

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

.loading { padding: 60px 0; text-align: center; color: var(--muted); font-size: var(--fs-body); }

.bd {
  flex: 1;
  min-height: 0;
  display: flex;
  flex-direction: column;
  gap: 8px;
  overflow: hidden;
  padding: 10px 18px 10px;
}

.info { flex: none; display: flex; align-items: center; gap: 22px; font-size: var(--fs-body); color: var(--muted); }
.info b { font-family: var(--mono); color: var(--cyan); font-weight: 400; }
.info b.new { color: var(--green); }
.info .grow { flex: 1; }
.info .same { color: var(--dim2); font-size: var(--fs-small); }

/* 并排视图：两栏表头 + DiffView。与下面的差异表 3 : 2 分高度 */
.cmp {
  flex: 3;
  min-height: 160px;
  display: flex;
  flex-direction: column;
  overflow: hidden;
}

/*
  ⚠️ <b>DiffView 自己是 flex: 0 1 auto</b>（文本对比页要它「内容只有两行时就只占两行」，
  好让下面的缩略图贴上来）。这一屏不一样：它上面是并排、下面是差异表，
  中间空一截会看着像少画了东西，所以在这儿把它撑满。
*/
:deep(.dv) { flex: 1; }

/* 两栏表头。逐条照文本对比页的 .rh —— 那是同一个视图的表头，长得不一样才怪 */
.rh {
  flex: none;
  display: flex;
  align-items: center;
  height: var(--th-h);
  padding: 4px 6px 0;   /* 上 4px：字形偏上 2px 的补偿，与全项目其余表头同一条 */
  background: var(--panel);
  border: 1px solid var(--border);
  border-bottom: 0;
}

.rhh { flex: 1; min-width: 0; display: flex; align-items: center; overflow: hidden; }
.rh .rt { font-family: var(--share); font-size: var(--th-size); letter-spacing: .14em; text-transform: uppercase; color: var(--th-fg); }
.rh .rm { margin-left: 8px; font-family: var(--mono); font-size: var(--fs-small); color: var(--dim); }

/* 中缝那一格的宽度必须与 DiffView 的 .dv-gut 一致（26 + 左右各 6），否则两栏表头对不上正文 */
.rhg { flex: none; width: 38px; }

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
.cap .warn { color: var(--amber); letter-spacing: .02em; text-transform: none; }
.cap .dim { color: var(--dim); letter-spacing: .02em; text-transform: none; overflow: hidden; text-overflow: ellipsis; }

.tbody { flex: 1; min-height: 0; overflow: auto; }

.head,
.row2 {
  display: grid;
  grid-template-columns: 44px 150px 60px 116px minmax(120px, 1fr) minmax(120px, 1fr);   /* 变更类型 80→116：ru Изменение 含字距 109、Добавлено 徽标 111 */
  align-items: center;
  gap: 8px;
  padding: 0 14px;   /* 必须与 style.css 里 .list-page .head 的 14px 一致 */
  font-size: var(--fs-body);
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
.hx { font-family: var(--mono); font-size: var(--fs-body); color: var(--acc-green2); }

.tg {
  padding: 4px 6px 4px;
  border: 1px solid;
  font-family: var(--share);
  font-size: var(--fs-label);
  line-height: 1;
  letter-spacing: .04em;
}

.tg.modified { border-color: rgb(var(--amber-rgb) / 35%); color: var(--amber); }
.tg.inserted { border-color: rgb(var(--green-rgb) / 35%); color: var(--green); }
.tg.deleted { border-color: rgb(var(--danger-rgb) / 30%); color: var(--danger); }

.empty { padding: 26px 0; text-align: center; color: var(--muted); font-size: var(--fs-body); }

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
.err { font-size: var(--fs-small); color: var(--danger); }

.btn {
  padding: 8px 13px 8px;
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
