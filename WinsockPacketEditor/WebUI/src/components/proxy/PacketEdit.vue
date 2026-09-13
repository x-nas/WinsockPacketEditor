<script setup lang="ts">
/*
  封包编辑 —— 对应 WinForms 的 Controls/PacketEdit（HexBox + 套接字 / 发送 / 递进三块面板）。

  【谁在用】封包列表右键「编辑」（list = proxy，改的是列表里那一条，保存即生效并按行推回列表）；
  发送编辑里双击发送集的一条（list = send，改的是发送编辑的工作副本，随那边的「保存」写回）。
  仓库里的仓储数据没有编辑入口 —— WinForms 也没有。

  【编辑器是纯前端的】打开时整段字节交过来（这一屏就是来改它的，不像列表那样按需取），
  改完整段交回去。C# 侧不维护编辑状态，只有「发送」是个跑在后台的会话。

  【编辑器是 HexView】十六进制那一块是全项目共用的 HexView（代理数据页的面板用的是同一个，只读），
  这里以可编辑模式挂它：字节由本组件持有（shallowRef），HexView 每次改动 emit 一份新数组回来，
  保存 / 发送时拿到的永远是最新的。右键菜单里「添加到滤镜 / 添加到发送」是这一屏自己的动作，
  通过 extraItems 加进去、按 pick 事件回来处理，剪切 / 复制 / 粘贴 / 全选是 HexView 内建的。

  【发送】发的是编辑器里<b>此刻</b>的字节（含未保存的改动），与 WinForms 一致。
  递进每发一次改一步、就地改在那份字节上，也与 WinForms 一致。点编辑器里的字节就是选递进位置。
*/
import { computed, onBeforeUnmount, ref, shallowRef, watch } from 'vue'
import { call } from '../../bridge'
import { FeedList, PACKET_TYPE, type SendRow } from '../../bridge/types'
import { t, type Key } from '../../i18n'
import { useList } from '../../stores/lists'
import { pushToast } from '../../stores/toast'
import HexView from '../HexView.vue'
import { ICON, type MenuItem } from '../menu'
import { useModal } from '../../useModal'

const props = defineProps<{
  /**
   * null = 不开。list：proxy = 代理数据列表；packet = 注入模式的封包列表；
   * send = 发送编辑的工作副本。id 是运行期自增的 long。
   *
   * ⚠️ proxy 与 packet 的 Id <b>各自独立自增</b>，同一个数字在两份表里是两条不同的包 ——
   * 所以 list 必须一路带到 C#，靠 id 猜表会静默改错东西。
   */
  target: { list: 'proxy' | 'packet' | 'send'; id: number } | null
}>()

const emit = defineEmits<{
  (e: 'close'): void
  (e: 'saved'): void
}>()

interface Head {
  Id: string
  List: string
  Socket: number
  Type: number
  From: string
  To: string
  Buffer: string   //base64
  CanSendBySession: boolean
  SystemSocket: number
}

const head = ref<Head | null>(null)
const loaded = ref(false)
const error = ref('')
const busy = ref(false)

const socket = ref(0)

/* ── 字节 ───────────────────────────────────────────────────── */

//shallowRef：HexView 每次改动给一份新数组，不需要深响应
const bytes = shallowRef<Uint8Array>(new Uint8Array(0))
const len = computed(() => bytes.value.length)

function b64ToBytes(s: string): Uint8Array {
  if (!s) return new Uint8Array(0)
  const bin = atob(s)
  const out = new Uint8Array(bin.length)
  for (let i = 0; i < bin.length; i++) out[i] = bin.charCodeAt(i)
  return out
}

function bytesToB64(a: Uint8Array): string {
  //分块：一次 apply 几万个参数会爆栈
  let s = ''
  for (let i = 0; i < a.length; i += 0x8000) {
    s += String.fromCharCode.apply(null, Array.from(a.subarray(i, i + 0x8000)))
  }
  return btoa(s)
}



/* ── 右键菜单里这一屏自己的两项 ─────────────────────────────── */

const sends = useList<SendRow>(FeedList.Send)
const hv = ref<InstanceType<typeof HexView> | null>(null)

const extraItems = computed<MenuItem[]>(() => {
  const n = hv.value?.selCount ?? 0
  const tag = n ? ' (' + n + ')' : ''
  return [
    { id: 'toFilter', label: t('pe.m.toFilter') + tag, icon: ICON.filter },
    sends.value.length
      ? { id: 'toSend', label: t('pe.m.toSend') + tag, icon: ICON.send, sub: sends.value.map((x) => ({ id: 'send:' + x.Id, label: x.Name })) }
      : { id: 'toSend', label: t('pe.m.toSend'), icon: ICON.send, disabled: true },
  ]
})

/** HexView 里选中那段（没选就是整包）。 */
function pickedBytes(): Uint8Array {
  return hv.value?.selectedBytes() ?? bytes.value.slice()
}

async function onPick(id: string): Promise<void> {
  const tg = props.target
  if (!tg) return

  try {
    if (id.startsWith('send:')) {
      const sid = id.slice(5)
      const r = await call<{ ok: boolean }>('packetEditToSend', { sid, list: tg.list, id: tg.id, buffer: bytesToB64(pickedBytes()) })
      const name = sends.value.find((x) => x.Id === sid)?.Name ?? ''
      pushToast(r?.ok ? 'success' : 'error', r?.ok ? t('pe.toSendOk') + ' ' + name : t('pe.toSendFail'))
      return
    }

    if (id === 'toFilter') {
      const r = await call<{ ok: boolean }>('packetEditToFilter', { list: tg.list, id: tg.id, buffer: bytesToB64(pickedBytes()) })
      pushToast(r?.ok ? 'success' : 'error', t(r?.ok ? 'pe.toFilterOk' : 'pe.toFilterFail'))
    }
  } catch (e) {
    console.error('[pe] ' + id + ' 失败', e)
    pushToast('error', String(e))
  }
}

/* ── 发送 ───────────────────────────────────────────────────── */

const sendMode = ref<'times' | 'cont'>('times')
const times = ref(1)
const interval = ref(100)    //WinForms 的默认值

const progOn = ref(false)
const progPos = ref(0)
const progStep = ref(1)
const progCarry = ref(false)
const progCarryN = ref(1)

const running = ref(false)
const prog = ref({ Running: false, Total: 0, Success: 0, Fail: 0 })
let poll = 0

function stopPoll(): void { if (poll) { clearInterval(poll); poll = 0 } }

function startPoll(): void {
  stopPoll()
  poll = window.setInterval(async () => {
    try {
      const p = await call<typeof prog.value>('getPacketSendProgress')
      if (p) prog.value = p
      if (!p?.Running) { running.value = false; stopPoll(); pushToast('success', t('pe.sendDone')) }
    } catch { running.value = false; stopPoll() }
  }, 200)
}

async function toggleSend(): Promise<void> {
  const tg = props.target
  if (!tg) return

  if (running.value) {
    try { await call('stopPacketSend') } catch (e) { console.error('[pe] 停止失败', e) }
    return
  }

  error.value = ''

  try {
    const r = await call<{ error: string }>('startPacketSend', {
      list: tg.list, id: tg.id, socket: Number(socket.value) || 0, buffer: bytesToB64(bytes.value),
      continuous: sendMode.value === 'cont', times: Number(times.value) || 1, interval: Number(interval.value) || 0,
      progression: progOn.value, position: Number(progPos.value) || 0, step: Number(progStep.value) || 1,
      carry: progCarry.value, carryCount: Number(progCarryN.value) || 1,
    })

    if (r?.error) { error.value = r.error; return }

    running.value = true
    prog.value = { Running: true, Total: 0, Success: 0, Fail: 0 }
    startPoll()
  } catch (e) {
    console.error('[pe] 发送失败', e)
    error.value = String(e)
  }
}

/* ── 保存 ───────────────────────────────────────────────────── */

async function save(): Promise<void> {
  const tg = props.target
  if (!tg) return

  busy.value = true
  error.value = ''

  try {
    const r = await call<{ error: string }>('savePacketEdit', {
      list: tg.list, id: tg.id, socket: Number(socket.value) || 0, buffer: bytesToB64(bytes.value),
    })

    if (r?.error) { error.value = r.error; return }

    pushToast('success', t('pe.saved'))
    emit('saved')
    await close()
  } catch (e) {
    console.error('[pe] 保存失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}

/*
  打开 / 关闭放在<b>所有状态声明之后</b>：这个 watch 是 immediate 的，挂载时立刻跑，
  里面要重置光标 / 选区 / 发送进度 —— 写在它们前面就是 TDZ（探针页抓到过 Cannot access cur before initialization）。
*/
/* ── 打开 / 关闭 ────────────────────────────────────────────── */

watch(() => props.target, async (tg) => {
  if (!tg) { stopPoll(); return }

  loaded.value = false
  error.value = ''
  head.value = null
  bytes.value = new Uint8Array(0)
  running.value = false
  prog.value = { Running: false, Total: 0, Success: 0, Fail: 0 }

  try {
    const r = await call<Head>('openPacketEdit', { list: tg.list, id: tg.id })

    if (!r?.Id) { error.value = t('pe.gone'); return }

    head.value = r
    socket.value = r.Socket
    bytes.value = b64ToBytes(r.Buffer)
    loaded.value = true
  } catch (e) {
    console.error('[pe] 打开失败', e)
    error.value = String(e)
  }
}, { immediate: true })

async function close(): Promise<void> {
  stopPoll()
  if (running.value) { try { await call('stopPacketSend') } catch { /* 关都关了 */ } }
  emit('close')
}

onBeforeUnmount(stopPoll)

const typeText = computed(() => {
  const k = head.value ? PACKET_TYPE[head.value.Type] : undefined
  return k ? t(k as Key) : String(head.value?.Type ?? '')
})

function offset(i: number): string { return i.toString(16).toUpperCase().padStart(8, '0') }

/* 登记进模态栈：父窗体因此变 inert；自己被后开的弹窗盖住时也会 inert。见 useModal.ts */
const { covered } = useModal(() => !!props.target)
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
  <Teleport to="body"><div v-if="props.target" class="editor-mask" :inert="covered">
    <div class="dlg" role="dialog" aria-modal="true" @keydown.esc="close">
      <span class="mk tl" /><span class="mk tr" /><span class="mk bl" /><span class="mk br" />

      <header class="hd">
        <div class="tt">
          <span class="zh">{{ t('pe.title') }}</span>
          <span class="sub">Controls/PacketEdit</span>
        </div>
        <button class="x" :title="t('dlg.cancel')" @click="close">
          <svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg>
        </button>
      </header>

      <div v-if="!loaded" class="loading">{{ error || t('proxy.working') }}</div>

      <div v-else-if="head" class="bd">
        <!-- 字段：套接字可改；地址与长度只看 -->
        <div class="fields">
          <div class="f">
            <span class="k">{{ t('pe.socket') }}</span>
            <input v-model.number="socket" class="inp num" type="number" min="0" :disabled="running" :title="t('pe.socketHint')">
            <button class="mini" :disabled="running || !head.SystemSocket" @click="socket = head.SystemSocket"
                    :title="head.SystemSocket ? String(head.SystemSocket) : ''">{{ t('pe.useSys') }}</button>
            <span v-if="socket === 0 && !head.CanSendBySession" class="warn">{{ t('pe.noSession') }}</span>
          </div>
          <div class="f"><span class="k">{{ t('col.type') }}</span><span class="v ty">{{ typeText }}</span></div>
          <div class="f"><span class="k">{{ t('pe.from') }}</span><span class="v mono">{{ head.From || '—' }}</span></div>
          <div class="f"><span class="k">{{ t('pe.to') }}</span><span class="v mono">{{ head.To || '—' }}</span></div>
          <div class="f"><span class="k">{{ t('pe.len') }}</span><span class="v mono cyan">{{ len }}</span></div>
        </div>

        <!-- 发送 + 递进，并排两块 -->
        <div class="panels">
          <div class="panel">
            <div class="cap">{{ t('pe.send') }}</div>
            <div class="line">
              <button class="rd" :class="{ on: sendMode === 'times' }" :disabled="running" @click="sendMode = 'times'"><i />{{ t('pe.byTimes') }}</button>
              <input v-model.number="times" class="inp num sm" type="number" min="1" max="999999" :disabled="running || sendMode !== 'times'">
              <span class="k2">{{ t('pe.times') }}</span>
              <button class="rd" :class="{ on: sendMode === 'cont' }" :disabled="running" @click="sendMode = 'cont'"><i />{{ t('pe.continuous') }}</button>
            </div>
            <div class="line">
              <span class="k2">{{ t('pe.interval') }}</span>
              <input v-model.number="interval" class="inp num sm" type="number" min="0" max="999999" :disabled="running">
              <span class="k2">{{ t('pe.ms') }}</span>
              <span class="grow" />
              <button class="btn run" :class="{ on: running }" :disabled="!len" @click="toggleSend">
                <svg v-if="running" class="ico" viewBox="0 0 24 24"><rect x="6" y="6" width="12" height="12" /></svg>
                <svg v-else class="ico" viewBox="0 0 24 24"><path d="M7 4l13 8-13 8z" /></svg>
                {{ running ? t('pe.stop') : t('pe.send') }}
              </button>
            </div>
            <div class="line cnts">
              <span class="cnt run">{{ t('pe.sendTotal') }} <b>{{ prog.Total }}</b></span>
              <span class="cnt ok">{{ t('pe.sendOk') }} <b>{{ prog.Success }}</b></span>
              <span class="cnt bad">{{ t('pe.sendFail') }} <b>{{ prog.Fail }}</b></span>
            </div>
          </div>

          <div class="panel">
            <div class="cap">{{ t('pe.prog') }}</div>
            <div class="line">
              <button class="chk" :class="{ on: progOn }" :disabled="running" @click="progOn = !progOn"><i />{{ t('pe.progOn') }}</button>
              <span class="k2">{{ t('pe.progPos') }}</span>
              <input v-model.number="progPos" class="inp num sm" type="number" min="0" :max="Math.max(0, len - 1)" :disabled="running || !progOn">
              <span class="k2">{{ t('pe.progStep') }}</span>
              <input v-model.number="progStep" class="inp num sm" type="number" min="-255" max="255" :disabled="running || !progOn">
            </div>
            <p class="tip">{{ t('pe.progHint') }}</p>
            <div class="line">
              <button class="chk" :class="{ on: progCarry }" :disabled="running || !progOn" @click="progCarry = !progCarry"><i />{{ t('pe.progCarry') }}</button>
              <span class="k2">{{ t('pe.progCarryN') }}</span>
              <input v-model.number="progCarryN" class="inp num sm" type="number" min="1" max="16" :disabled="running || !progOn || !progCarry">
            </div>
            <p class="tip">{{ t('pe.progCarryHint') }}</p>
          </div>
        </div>

        <!-- 十六进制编辑器：外框 + 标题栏归这里，正文是共用的 HexView -->
        <div class="hexed">
          <!-- 标题栏与代理数据页的面板同一条：标题 + 元信息；光标 / 选区 / 模式从 HexView 读 -->
          <div class="hx-bar">
            <span class="hx-title">{{ t('pe.hex') }}</span>
            <span class="hx-meta">{{ len }} {{ t('hex.bytes') }}</span>
            <span class="hx-meta">{{ t('pe.cursor') }} {{ offset(hv?.cur ?? 0) }} ({{ hv?.cur ?? 0 }})</span>
            <span v-if="hv?.hasSel" class="hx-meta sel">{{ t('pe.selected') }} {{ hv?.selCount }} {{ t('pe.bytes') }}</span>
            <span class="hx-meta dim">{{ hv?.col === 'asc' ? t('pe.ascii') : t('pe.hex') }}</span>
            <!-- 覆盖 / 插入：唯一决定「键入会不会改变封包长度」的开关，必须一眼看得出来，见 <style> 里那段 -->
            <span class="hx-meta mode" :class="hv?.insertMode ? 'ins' : 'ovr'">{{ hv?.insertMode ? t('pe.insert') : t('pe.overwrite') }}</span>
            <span class="grow" />
            <span class="hx-meta dim keys" :title="t('pe.keysHint')">{{ t('pe.keysHint') }}</span>
          </div>

          <HexView
            ref="hv"
            v-model:bytes="bytes"
            :extra-items="extraItems"
            @cursor="progPos = $event"
            @pick="onPick"
          />
        </div>
      </div>

      <footer class="ft">
        <span v-if="loaded && error" class="err">{{ error }}</span>
        <span class="grow" />
        <button class="btn" :disabled="busy" @click="close">{{ t('dlg.cancel') }}</button>
        <button class="btn primary" :disabled="busy || !loaded || running" @click="save">
          {{ busy ? t('proxy.working') : t('set.save') }}
        </button>
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

/* 字段行：一行铺开，五个字段 */
.fields { flex: none; display: flex; flex-wrap: wrap; align-items: center; gap: 6px 22px; }
.f { display: flex; align-items: center; gap: 8px; min-width: 0; }
.f .k { font-size: var(--fs-body); color: var(--muted); white-space: nowrap; }
.f .v { font-size: var(--fs-body); color: var(--gray); }
.f .v.mono { font-family: var(--mono); font-size: var(--fs-body); color: var(--dim3); }
.f .v.cyan { color: var(--cyan); }
.f .v.ty { color: var(--acc-violet); }
.f .warn { font-size: var(--fs-small); color: var(--amber); }

/* 基样式在 style.css 的 .inp，这一屏没有需要覆盖的 */

.inp.num { width: 92px; text-align: center; }
.inp.num.sm { width: 76px; height: 26px; }

/* 小按钮的样式在 style.css 的 .mini */

/* 发送 / 递进两块 */
.panels { flex: none; display: grid; grid-template-columns: 1fr 1fr; gap: 10px; }

.panel {
  display: flex;
  flex-direction: column;
  gap: 7px;
  padding: 8px 12px 9px;
  border: 1px solid var(--border);
  background: rgb(var(--inset-rgb) / 20%);
}

.cap {
  font-family: var(--share);
  font-size: var(--th-size);
  letter-spacing: .14em;
  text-transform: uppercase;
  color: var(--th-fg);
}

.line { display: flex; align-items: center; gap: 10px; min-height: 26px; }
.line .grow { flex: 1; }
.k2 { font-size: var(--fs-body); color: var(--muted); white-space: nowrap; }
.tip { margin: 0; font-size: var(--fs-small); color: var(--dim2); line-height: 1.5; }

.cnts .cnt { font-size: var(--fs-label); color: var(--muted); font-family: var(--share); letter-spacing: .06em; position: relative; top: 1px; }   /* 与大一号的数字排在一行，实测偏高 1.3px（2026-09-13 字体度量覆写后按 100% 缩放实测重调） */
.cnts .cnt b { font-family: var(--mono); font-variant-numeric: tabular-nums; }
.cnts .cnt.run b { color: var(--cyan); }
.cnts .cnt.ok b { color: var(--green); }
.cnts .cnt.bad b { color: var(--danger); }

/* 基样式在 style.css 的「勾选框 / 单选框」，这一屏没有需要覆盖的 */

.btn {
  flex: none;
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
  white-space: nowrap;
}

.btn:hover:not(:disabled) { border-color: var(--cyan); color: var(--cyan); }
.btn:disabled { opacity: .35; cursor: default; }
.btn.primary { border-color: rgb(var(--green-rgb) / 45%); color: var(--green); }
.btn.primary:hover:not(:disabled) { background: rgb(var(--green-rgb) / 10%); border-color: var(--green); }

.btn.run { display: inline-flex; align-items: center; gap: 7px; border-color: rgb(var(--green-rgb) / 45%); color: var(--green); }
.btn.run .ico { width: 11px; height: 11px; margin: -1px 0; flex: none; }
.btn.run.on { border-color: rgb(var(--danger-rgb) / 30%); color: var(--danger); }
.btn.run.on .ico { animation: pulse 1.1s ease-in-out infinite; }
.btn.run:hover:not(:disabled) { background: rgb(var(--green-rgb) / 10%); border-color: var(--green); color: var(--green); }
.btn.run.on:hover:not(:disabled) { background: rgb(var(--danger-rgb) / 12%); border-color: var(--danger); color: var(--danger); }

@keyframes pulse { 50% { opacity: .25; } }

@media (prefers-reduced-motion: reduce) {
  .btn.run.on .ico { animation: none; }
}

/* ── 十六进制编辑器的外框与标题栏（正文是 HexView）── */

.hexed {
  flex: 1;
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
  padding-top: 2px;   /* 同 HexPanel：原来的 4px 在字体度量覆写之后补过头 1px（2026-09-13 字体度量覆写后按 100% 缩放实测重调） */
  font-family: var(--share);
  font-size: var(--th-size);
  letter-spacing: .14em;
  text-transform: uppercase;
  color: var(--th-fg);
}

.hx-meta { padding-top: 2px; font-family: Consolas, monospace; font-size: var(--th-size); color: var(--gray); }   /* Consolas 实测偏上 1px */
.hx-meta.sel { color: var(--cyan); }
.hx-meta.dim { color: var(--muted); }
.hx-meta.keys { overflow: hidden; text-overflow: ellipsis; min-width: 0; }

/*
  覆盖 / 插入 —— 编辑器里唯一一个「键入会不会改变封包长度」的开关，
  而它<b>只能用 Insert 键切，界面上没有第二个入口</b>。所以它必须一眼看得出来。

  原先它和栏位挤在同一个 .dim 里（`十六进制 · 覆盖`），两种状态还<b>同色</b> ——
  等于得把那行灰色小字读完才知道现在是哪种。现在拆成独立的描边胶囊，两态两色：

    覆盖 → 绿：常态，长度不变
    插入 → 琥珀：会把包撑长，更该当心

  ⚠️ 不用青色 —— 旁边的「选中 N 字节」（.hx-meta.sel）就是青的，两个挨着分不出来。
*/
.hx-meta.mode {
  flex: none;
  /* 3.6/1.4 是量出来的：胶囊自带边框、结构与旁边的裸文字不同，要让它的墨迹与同排那几项落在同一条线上 */
  padding: 3.6px 7px 1.4px;
  border: 1px solid;
  letter-spacing: .08em;
}

.hx-meta.mode.ovr { border-color: rgb(var(--green-rgb) / 45%); background: rgb(var(--green-rgb) / 12%); color: var(--green); }
.hx-meta.mode.ins { border-color: rgb(var(--amber-rgb) / 55%); background: rgb(var(--amber-rgb) / 16%); color: var(--amber); }
.hx-bar .grow { flex: 1; min-width: 12px; }

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
</style>
