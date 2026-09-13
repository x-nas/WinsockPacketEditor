<script setup lang="ts">
/*
  十六进制面板。

  字节流<b>不进推送流</b>：100 条/秒 × 4KB 走 base64 是每秒 1MB 的纯浪费。
  只在用户点行时按 Id 回来取一次，见 CLAUDE.md「字节流按需拉取」。
  往返耗时显示在标题栏上 —— 验收线是 <50ms。
*/
import { computed, nextTick, ref, watch } from 'vue'
import { call } from '../bridge'
import { FeedList, type PacketDetail, type SendRow } from '../bridge/types'
import { byteLen } from '../hex'
import HexView from './HexView.vue'
import { ICON, type MenuItem } from './menu'
import { t } from '../i18n'
import { useList } from '../stores/lists'
import { pushToast } from '../stores/toast'
import { textA, textB } from '../stores/tools'
import { gotoPage } from '../stores/runtime'

const props = withDefaults(defineProps<{
  id: number | null
  /**
   * 选中那一行的 PacketType。17–20 是 HTTP / HTTPS 的请求与响应，
   * 这几类默认按文本看 —— 与 WinForms 的 PacketData 控件判断的是同一组值。
   */
  packetType?: number | null
  /** 「查找封包」命中的那一段字节，取到字节后圈出来；null = 不圈 */
  highlight?: { offset: number; length: number } | null
  /**
   * 这一条封包属于哪份列表。
   *
   * <b>两份列表各有一套独立的 Id 序列</b>（ProxyInfo 与 PacketInfo 各自 Interlocked 自增），
   * 拿注入模式的 Id 去代理表里找，轻则找不到、重则找出另一条包的字节 —— 而且不报错。
   * 右键那几个动作也吃它（C# 侧的 PacketEditConfig 按同一个串分流）。
   */
  list?: 'proxy' | 'packet'
}>(), { packetType: null, highlight: null, list: 'proxy' })

const detail = ref<PacketDetail | null>(null)
const ms = ref(0)
const err = ref('')
const loading = ref(false)
const showRaw = ref(false)

/** 只认最后一次请求的结果：快速连点时早先的响应可能后到，落后的要丢掉。 */
let token = 0

watch(
  () => props.id,
  async (id) => {
    detail.value = null
    err.value = ''

    if (id == null) return

    const mine = ++token
    loading.value = true
    const t0 = performance.now()

    try {
      // 显式带上通道：封包与代理各有一套独立的 Id 序列，C# 那边要据此选表
      const d = await call<PacketDetail | null>('getPacketDetail', {
        id,
        list: props.list === 'packet' ? FeedList.Packet : FeedList.Proxy,
      })
      if (mine !== token) return

      ms.value = Math.round((performance.now() - t0) * 10) / 10
      if (!d) {
        err.value = `Id ${id} · ${t('hex.gone')}`
        return
      }
      detail.value = d
      if (!d.modified) showRaw.value = false
    } catch (e) {
      if (mine !== token) return
      err.value = String(e)
    } finally {
      if (mine === token) loading.value = false
    }
  },
  { immediate: true },
)


/*
  正文交给 HexView（全项目唯一的十六进制视图，封包编辑用的是同一个）。
  它只认字节数组，这里把 base64 解开；「改写前 / 改写后」的差异高亮由它的 compare 做：
  看「改写后」就拿改写前当参照，反过来也一样，来回切时同一批字节始终亮着。
  detail.modified 为假时不给参照物 —— 那是 C# 侧逐字节比过的结论（ShellForm.SameBytes），没改过就没有差异可标。
*/
function toBytes(b64: string | null | undefined): Uint8Array {
  if (!b64) return new Uint8Array(0)
  const bin = atob(b64)
  const out = new Uint8Array(bin.length)
  for (let i = 0; i < bin.length; i++) out[i] = bin.charCodeAt(i)
  return out
}

const shown = computed(() => toBytes(showRaw.value ? detail.value?.raw : detail.value?.packet))

const other = computed<Uint8Array | null>(() => {
  const d = detail.value
  if (!d || !d.modified) return null
  return toBytes(showRaw.value ? d.packet : d.raw)
})

/* ── 文本 / 十六进制两种看法 ──────────────────────────────── */

/*
  WinForms 的 PacketData 控件有两个页签：HTTP / HTTPS 那四类封包（PacketType 17–20）
  显示解码后的文本，其余显示十六进制。这里保留同一条判据，只是把「自动切页签」
  换成「自动选一个默认值 + 允许手动切」—— 抓 HTTP 时偶尔也要看原始字节，
  而 WinForms 那边一旦判成 HTTP 就<b>看不到</b>十六进制了。

  asText 为 null 表示「还没手动切过，跟着封包类型走」；用户点过之后就一直用他选的，
  直到换一条封包（下面那个 watch 会重置）。
*/
const asText = ref<boolean | null>(null)

const isHttp = computed(() => {
  const p = props.packetType
  return p === 17 || p === 18 || p === 19 || p === 20
})

const textMode = computed(() => (asText.value === null ? isHttp.value : asText.value))

/*
  UTF8 解码，不可打印的字节显示成「·」。

  fatal: false —— 抓到的常常是半截包或二进制夹带，遇上非法序列不能抛异常。
  制表与换行留着（HTTP 头靠它们分行），其余控制字符与 U+FFFD 都换成中点：
  原样输出会让 <pre> 里出现空洞和方框，反而更难读。
*/
const text = computed(() => {
  const b = shown.value
  if (b.length === 0) return ''

  let s: string
  try {
    s = new TextDecoder('utf-8', { fatal: false }).decode(b)
  } catch {
    s = ''
    for (let i = 0; i < b.length; i++) s += String.fromCharCode(b[i])
  }

  // eslint-disable-next-line no-control-regex
  return s.replace(/[\u0000-\u0008\u000B\u000C\u000E-\u001F\u007F\uFFFD]/g, "\u00B7")
})

/* ── 右键菜单：这一屏自己的动作 ───────────────────────────── */

/*
  对应 WinForms 的 GetCMS_PacketData：添加到发送 ▸ / 添加到滤镜 / 添加到文本 A·B。
  <b>作用于选中的那一段字节，没选就是整包</b> —— 与那边一致（它取的是 SelectionStart/Length，
  没选才退回整个缓冲）。剪切 / 复制 / 粘贴 / 全选是 HexView 内建的，不在这里重复。

  「编辑」不放进来：封包编辑是列表右键的入口，这里再来一份只会让两条路都要维护。
*/
const hv = ref<InstanceType<typeof HexView> | null>(null)
const sends = useList<SendRow>(FeedList.Send)

const extraItems = computed<MenuItem[]>(() => {
  const n = hv.value?.selCount ?? 0
  const tag = n ? ' (' + n + ')' : ''

  return [
    { id: 'toFilter', label: t('pm.toFilter') + tag, icon: ICON.filter },
    sends.value.length
      ? { id: 'toSend', label: t('pm.toSend') + tag, icon: ICON.send, sub: sends.value.map((x) => ({ id: 'send:' + x.Id, label: x.Name })) }
      : { id: 'toSend', label: t('pm.toSend'), icon: ICON.send, disabled: true },
    { divider: true },
    { id: 'toTextA', label: t('pm.toTextA') + tag, icon: ICON.text },
    { id: 'toTextB', label: t('pm.toTextB') + tag, icon: ICON.text },
  ]
})

/** 选中那段（没选就是整包）。 */
function pickedBytes(): Uint8Array {
  return hv.value?.selectedBytes() ?? shown.value.slice()
}

function bytesToB64(a: Uint8Array): string {
  let s = ''
  for (let i = 0; i < a.length; i++) s += String.fromCharCode(a[i])
  return btoa(s)
}

function bytesToHex(a: Uint8Array): string {
  const out: string[] = []
  for (let i = 0; i < a.length; i++) out.push(a[i].toString(16).toUpperCase().padStart(2, '0'))
  return out.join(' ')
}

async function onPick(id: string): Promise<void> {
  const pid = props.id
  if (pid == null) return

  try {
    if (id.startsWith('send:')) {
      const sid = id.slice(5)
      //list —— C# 侧按这个决定去代理列表、注入模式的封包列表、还是发送编辑的工作副本里找那一条
      const r = await call<{ ok: boolean }>('packetEditToSend', { sid, list: props.list, id: pid, buffer: bytesToB64(pickedBytes()) })
      const name = sends.value.find((x) => x.Id === sid)?.Name ?? ''
      pushToast(r?.ok ? 'success' : 'error', r?.ok ? t('pm.added') + ' ' + name : t('pm.addFail'))
      return
    }

    if (id === 'toFilter') {
      const r = await call<{ ok: boolean }>('packetEditToFilter', { list: props.list, id: pid, buffer: bytesToB64(pickedBytes()) })
      pushToast(r?.ok ? 'success' : 'error', t(r?.ok ? 'pm.added' : 'pm.addFail'))
      return
    }

    if (id === 'toTextA' || id === 'toTextB') {
      //与列表右键那两项一致：整体替换（WinForms 的 SetTextA 也是替换不是追加），然后切到文本对比页
      const hex = bytesToHex(pickedBytes())
      if (id === 'toTextA') textA.value = hex
      else textB.value = hex
      pushToast('success', t(id === 'toTextA' ? 'pm.toTextAOk' : 'pm.toTextBOk'))
      gotoPage.value = 'diff'
    }
  } catch (e) {
    console.error('[hex] ' + id + ' 失败', e)
    pushToast('error', String(e))
  }
}

/* ── 「查找封包」命中后圈出那一段 ─────────────────────────── */

/*
  字节是异步取回来的，而命中信息可能比它先到，所以两个来源都要触发一次：
  highlight 变了、或者 detail 换成了新的一条。等 nextTick 是因为 HexView
  要先按新字节渲染出来，selectRange 里的 scrollIntoView 才算得对。

  <b>只在十六进制视图下圈</b>：文本视图是个 <pre>，没有字节级选区。
*/
async function applyHighlight(): Promise<void> {
  const h = props.highlight
  if (!h || h.offset < 0 || h.length <= 0 || !detail.value || textMode.value) return

  await nextTick()
  hv.value?.selectRange(h.offset, h.offset + h.length)
}

watch(() => props.highlight, applyHighlight)
watch(detail, applyHighlight)

//换一条封包就把手动切过的视图选择丢掉，重新跟着封包类型走
watch(() => props.id, () => { asText.value = null })
</script>

<template>
  <div class="hx">
    <div class="hx-bar">
      <span class="hx-title">{{ t('hex.title') }}</span>

      <template v-if="detail">
        <!-- 与列表那一列同名，改了那边这里也要跟着改 -->
        <span class="hx-meta">{{ t('col.id') }} {{ detail.id }}</span>
        <span class="hx-meta">
          {{ byteLen(showRaw ? detail.raw : detail.packet) }} {{ t('hex.bytes') }}
        </span>
        <!--
          往返：从发出 getPacketDetail 到拿到结果的墙钟时间（前端自己掐表）。
          包含桥的一次往返 + C# 侧按 Id 线性扫列表取字节 + base64 编解码，
          <b>不含</b>本组件把字节排成十六进制的那一步。
          验收线是 p95 < 50ms，超了标红。
        -->
        <span class="hx-meta" :class="{ slow: ms >= 50 }" :title="t('hex.rttHint')">
          {{ t('hex.rtt') }} {{ ms }}ms
        </span>

        <!--
          改写前 / 改写后。原来是 a-radio-group + button-style="solid" ——
          它自带浅色圆角与蓝色主色，压在这套深色皮肤上就是从别的程序飞过来的一块，
          也是全项目<b>最后一个</b>还在用 ant-design-vue 的正式组件。
          自绘之后与状态条、快捷面板的分段按钮同一套语言。
        -->
        <div v-if="detail.modified" class="hx-seg">
          <button class="hx-segb after" :class="{ on: !showRaw }" @click="showRaw = false">{{ t('hex.after') }}</button>
          <button class="hx-segb before" :class="{ on: showRaw }" @click="showRaw = true">{{ t('hex.before') }}</button>
        </div>
        <!-- modified 由 C# 侧逐字节比较改写前后两份缓冲得出（ShellForm.SameBytes）-->
        <span v-else class="hx-meta dim" :title="t('hex.unmodifiedHint')">{{ t('hex.unmodified') }}</span>

        <!--
          文本 / 十六进制。与「改写前后」那组同一套外观，靠在右端 ——
          那一组切的是「看哪份字节」，这一组切的是「怎么看这份字节」，是两件事。
        -->
        <div class="hx-seg right" :title="t('hex.textHint')">
          <button class="hx-segb after" :class="{ on: !textMode }" @click="asText = false">{{ t('hex.asHex') }}</button>
          <button class="hx-segb before" :class="{ on: textMode }" @click="asText = true">{{ t('hex.asText') }}</button>
        </div>
      </template>

      <span v-else-if="loading" class="hx-meta">{{ t('hex.loading') }}</span>
      <span v-else-if="err" class="hx-meta bad">{{ err }}</span>
      <span v-else class="hx-meta dim">{{ t('hex.pick') }}</span>
    </div>

    <!--
      表头与正文<b>同在一个滚动容器里</b>。分成两个容器的话，
      每行字节数偶尔算不满（面板极窄时退到 8 字节会横向溢出），
      两边就会错位 —— 同容器 + sticky 让它横向跟着滚、纵向钉住。
    -->
    <!--
      文本视图：对应 WinForms PacketData 控件的 tpText 页签。
      用 <pre> 是为了能<b>原生选中复制</b> —— style.css 里 body 是 user-select: none，
      只给 <pre> 放开了。HTTP 头这种东西，选中一段拷出去是最常做的事。
    -->
    <pre v-if="detail && textMode" class="hx-text"><span class="hx-tx">{{ text }}</span></pre>

    <!-- 外框色传给 HexView，它的列号表头 sticky 时要拿这个色盖住滚过去的正文 -->
    <HexView
      v-else-if="detail"
      ref="hv"
      :bytes="shown"
      :compare="other"
      :extra-items="extraItems"
      readonly
      style="--hexview-bg: var(--wpe-panel)"
      @pick="onPick"
    />
    <div v-else class="hx-empty"></div>
  </div>
</template>

<style scoped>
.hx {
  display: flex;
  flex-direction: column;
  min-height: 0;
  border: 1px solid var(--wpe-line);
  background: var(--wpe-panel);
  overflow: hidden;
}

.hx-bar {
  display: flex;
  align-items: center;
  gap: 14px;
  height: var(--th-h);
  padding: 0 12px;
  background: var(--wpe-head);
  border-bottom: 1px solid var(--wpe-line);
  flex: none;
}

.hx-title {
  /* 与各表表头同一份字样：--th-size · Share Tech Mono · 大写 · var(--th-fg) */
  padding-top: 2px;   /* 原来的 4px 是给字形偏上的补偿，字体度量覆写之后补过头 1px（2026-09-13 字体度量覆写后按 100% 缩放实测重调） */
  font-family: var(--share);
  font-size: var(--th-size);
  letter-spacing: .14em;
  text-transform: uppercase;
  color: var(--th-fg);
}

.hx-meta {
  padding-top: 2px;   /* Consolas 10.5px 实测偏上 1px */
  font-family: Consolas, monospace;
  font-size: var(--th-size);   /* 与标题、各表表头同一份字号 */
  color: var(--wpe-fg);
}

/* 改写前 / 改写后：两格共用一个边框盒，选中的一格提亮 */
/* .hx-seg / .hx-segb 在 style.css 里，两个数据页的查找框也在用 —— 别在这儿再抄一份 */

/*
  文本视图。等宽字 + 保留原有的空白与换行，<b>过长的行自动折到下一行</b>。
  user-select 显式打开：全局 body 是 none，这一块是特意放行的少数几处之一。

  ⚠️ <b>不横向滚</b>（2026-09-09 改）。这块面板只有小半屏高、又是拿来<b>读</b>的：
  一条 HTTP 头或一段解码出来的正文动辄几百字符，横着拖等于把刚看的那半句推走，
  而且拖到右边之后其余各行的开头全都看不见了。WinForms 那边（txtText 是 AntdUI 的
  多行 Input）本来就是换行的，这一处是移植时跟丢的。

  ⚠️ 两个属性缺一不可：
  · <b>pre-wrap</b> —— 保住原文里真实的换行与缩进（HTTP 头靠它分行），同时允许折行；
    普通的 pre 只保不折。
  · <b>overflow-wrap: anywhere</b> —— pre-wrap 只在<b>空白处</b>断行，而这一栏经常是
    一整段没有空格的解码文本（二进制被当文本解出来就是那样），光有 pre-wrap 照样溢出。
    ⚠️ 刻意<b>不用 word-break: break-all</b>：那个连能在空格处断的地方也一律拆词，
    HTTP 头会被拦腰截断；anywhere 是"实在放不下才拆"。

  ⚠️ <b>cursor 写在滚动容器上，滚动条也会吃这个值</b> —— 原来这里是 cursor: text，
  于是鼠标移到滚动条上仍是「工」字形的文本光标，看着像还能选字。
  正确的分法是：<b>容器 default、内容 text</b>，所以文字包了一层 .hx-tx。
  （原生 textarea 不用管这件事，UA 的 cursor: auto 只在真有文字的地方才给 I 形。）
*/
.hx-text {
  flex: 1;
  margin: 0;
  padding: 8px 12px;
  /* 只留纵向。横向已经不会溢出了，写死 hidden 是防止某一版式下冒出半条来 */
  overflow-y: auto;
  overflow-x: hidden;
  font-family: Consolas, monospace;
  font-size: var(--fs-dense);
  line-height: 18px;
  color: var(--wpe-fg);
  white-space: pre-wrap;
  overflow-wrap: anywhere;
  user-select: text;
  cursor: default;
}

/* display: block 才铺满整宽 —— 短行右边那片空白也该是能选中的文字区 */
.hx-tx { display: block; cursor: text; }

.hx-meta.dim { color: var(--wpe-muted); }
.hx-meta.bad { color: #f48771; }
.hx-meta.slow { color: #dcdcaa; }

.hx-empty { flex: 1; }
</style>
