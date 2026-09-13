<script setup lang="ts">
/*
  发送编辑 —— 对应 WinForms 的 Controls/SendEdit。

  上半部是这条发送的字段（名称 / 套接字 / 循环 / 备注），
  下半部是它的<b>发送集</b>：一串要按顺序重放的封包。

  【改的是工作副本，保存才算数】C# 侧 OpenSendEdit_ById 会把发送集拷一份出来
  （与 WinForms 的 SendEdit_Load 一致），排序、删除、导入、清空都只动那一份，
  按「取消」就真的什么都没发生。所以关弹窗时<b>无论保存与否都要调 closeSendEdit</b>，
  否则执行器还挂在上一条发送上。

  【为什么本机地址这一列有用】它不是摆设：SendPacket 按封包类型挑地址 ——
  请求类（*Send / TCP_Req / UDP_Req）用<b>远端</b>地址，
  响应类（*Recv / TCP_Resp / UDP_Resp）用<b>本机</b>地址。
  TCP 两种都走 send()、不用地址；真正会读它的是 UDP 响应类 —— 重放一条 UDP 响应，
  就是冒充服务端往当初那个客户端地址回包。

  【这一屏只管「有哪些封包、按什么顺序」】要看某条封包的字节、要改它，
  双击那一行进封包编辑（PacketEdit.vue，封包列表那边用的是同一个弹窗）。<b>不在这里塞一块只读的十六进制</b> ——
  它占掉小半屏高度，而真正要看字节的时候还是得进编辑器，两头都不讨好。
*/
import { computed, onBeforeUnmount, ref, watch } from 'vue'
import { call } from '../../bridge'
import { ListAction, PACKET_TYPE } from '../../bridge/types'
import { t, type Key } from '../../i18n'
import { pushToast } from '../../stores/toast'
import { useRowPick } from '../../usePick'
import ContextMenu from '../ContextMenu.vue'
import { ICON, type MenuItem } from '../menu'
import PacketEdit from './PacketEdit.vue'
import { useModal } from '../../useModal'

const props = defineProps<{ id: string | null }>()

const emit = defineEmits<{
  (e: 'close'): void
  (e: 'saved'): void
}>()

interface Head {
  Id: string
  Name: string
  UseSystemSocket: boolean
  LoopCount: number
  LoopInterval: number
  Notes: string
  SystemSocket: number
}

interface PacketRow {
  Id: string
  Socket: number
  Type: number
  From: string
  To: string
  Len: number
  Preview: string
}

const f = ref<Head | null>(null)
const rows = ref<PacketRow[]>([])
const busy = ref(false)
const error = ref('')

/* ── 打开 / 关闭 ────────────────────────────────────────────── */

/*
  ⚠️ <b>下面这四个必须声明在那个 immediate 的 watch 之前。</b>

  watch 的回调虽然是 async，但它在第一个 await <b>之前</b>就同步动了 picked / prog / error，
  而 const / let 有暂时性死区 —— 声明在后面就是
  "Cannot access 'picked' before initialization"，与 PacketEdit 那个 TDZ 同一类。

  ⚠️ <b>这条在真实用法里不发作，所以特别容易漏</b>：外壳里这个弹窗是一直挂着、
  靠 id 从 null 变成非 null 打开的，首次 immediate 那一趟走的是 id === null 的早退分支。
  只有<b>直接带着非空 id 挂载</b>（探针页 #send 就是）才会踩到。别因为「跑起来没事」就搬回去。
*/
let poll = 0

//单击 / Ctrl / Shift 多选，全项目一份实现，见 usePick.ts
const { picked, onRowClick, selectAll, clear } = useRowPick(rows, (r) => r.Id)

const prog = ref({ Running: false, Index: -1, Total: 0, Success: 0, Fail: 0 })

watch(() => props.id, async (id) => {
  if (id === null) {
    stopPoll()
    return
  }

  f.value = null
  rows.value = []
  picked.value = new Set()
  error.value = ''
  prog.value = { Running: false, Index: -1, Total: 0, Success: 0, Fail: 0 }

  try {
    const r = await call<Head>('openSendEdit', { id })

    if (!r?.Id) {
      error.value = t('snd.e.gone')
      return
    }

    f.value = r
    await reload()
  } catch (e) {
    console.error('[snd.e] 打开失败', e)
    error.value = String(e)
  }
}, { immediate: true })

/** 关弹窗。C# 侧要收尾（停执行器、丢掉工作副本），保存与取消都得走这里。 */
async function close(): Promise<void> {
  stopPoll()

  try {
    await call('closeSendEdit')
  } catch (e) {
    console.error('[snd.e] 收尾失败', e)
  }

  emit('close')
}

onBeforeUnmount(stopPoll)

/* ── 发送集 ─────────────────────────────────────────────────── */

async function reload(): Promise<void> {
  try {
    const r = await call<{ rows: PacketRow[] }>('getSendCollection')
    rows.value = r?.rows ?? []

    //整表换掉后对一次选中集，理由与各列表一屏相同
    const alive = new Set(rows.value.map((x) => x.Id))
    const next = new Set<string>()
    for (const id of picked.value) { if (alive.has(id)) next.add(id) }
    picked.value = next
  } catch (e) {
    console.error('[snd.e] 取发送集失败', e)
  }
}


/** 类别文案走 i18n 键，与封包列表同一份，不另起译法。 */
const typeText = computed<Record<number, string>>(() => {
  const m: Record<number, string> = {}
  for (const k of Object.keys(PACKET_TYPE)) m[+k] = t(PACKET_TYPE[+k] as Key)
  return m
})

/* ── 行为 ───────────────────────────────────────────────────── */

/** 封包编辑弹窗的目标；null = 关着。改的是工作副本里那一条，随本弹窗的「保存」写回 */
const editTarget = ref<{ list: 'proxy' | 'send'; id: number } | null>(null)

function onRowDblClick(e: MouseEvent, r: PacketRow): void {
  if ((e.target as HTMLElement | null)?.closest('button')) return
  if (prog.value.Running) return
  editTarget.value = { list: 'send', id: Number(r.Id) }
}

/* ── 右键菜单（六项，没有导出）──────────────────────────────── */

const menuAt = ref<{ x: number; y: number } | null>(null)

const menuItems = computed<MenuItem[]>(() => {
  const n = picked.value.size
  const tag = n ? ' (' + n + ')' : ''

  return [
    { id: 'top', label: t('lst.top') + tag, icon: ICON.top },
    { divider: true },
    { id: 'up', label: t('lst.up') + tag, icon: ICON.up },
    { id: 'down', label: t('lst.down') + tag, icon: ICON.down },
    { divider: true },
    { id: 'bottom', label: t('lst.bottom') + tag, icon: ICON.bottom },
    { divider: true },
    { id: 'copy', label: t('lst.copy') + tag, icon: ICON.copy },
    { id: 'delete', label: t('lst.delete') + tag, icon: ICON.del, danger: true },
    { divider: true },
    /* 勾选框列去掉之后，全选得在这儿有个入口 —— 与封包列表的右键菜单同一套 */
    { id: 'selectAll', label: t('pm.selectAll'), icon: ICON.list },
    { id: 'deselect', label: t('pm.deselect'), icon: ICON.del, disabled: !n },
  ]
})

const ACTION_OF: Record<string, ListAction> = {
  top: ListAction.Top,
  up: ListAction.Up,
  down: ListAction.Down,
  bottom: ListAction.Bottom,
  copy: ListAction.Copy,
  delete: ListAction.Delete,
}

async function onMenuPick(id: string): Promise<void> {
  if (id === 'selectAll') { selectAll(); return }
  if (id === 'deselect') { clear(); return }

  if (picked.value.size === 0) {
    pushToast('warning', t('lst.needPick'))
    return
  }

  const action = ACTION_OF[id]
  if (action === undefined) return

  try {
    await call('sendCollectionAction', { action, ids: [...picked.value] })
    await reload()
  } catch (e) {
    console.error('[snd.e] 发送集操作失败', e)
  }
}

async function collectionCmd(method: string): Promise<void> {
  try {
    await call(method)
    await reload()
  } catch (e) {
    console.error('[snd.e] ' + method + ' 失败', e)
  }
}

/* ── 执行 ───────────────────────────────────────────────────── */

function stopPoll(): void {
  if (poll) { clearInterval(poll); poll = 0 }
}

/*
  在跑的时候轮询，<b>不挂事件</b>：C# 的 Send_DoWork 只在「循环间隔 > 0」时才
  ReportProgress，间隔填 0 就一个事件都没有，三个计数会一直停在 0 直到跑完。
  轮询没有这个问题。同样因为这个，间隔为 0 时 Index 不动、当前行不会高亮 ——
  WinForms 那边也是如此，不是这里漏了。
*/
function startPoll(): void {
  stopPoll()

  poll = window.setInterval(async () => {
    try {
      const p = await call<typeof prog.value>('getSendEditProgress')
      if (p) prog.value = p
      if (!p?.Running) { stopPoll(); pushToast('success', t('snd.e.done')) }
    } catch {
      stopPoll()
    }
  }, 200)
}

function payload(): Record<string, unknown> {
  const v = f.value
  return {
    name: v?.Name ?? '',
    useSystemSocket: !!v?.UseSystemSocket,
    loopCount: v?.LoopCount ?? 1,
    loopInterval: v?.LoopInterval ?? 0,
    notes: v?.Notes ?? '',
  }
}

async function toggleRun(): Promise<void> {
  if (prog.value.Running) {
    try { await call('stopSendEdit') } catch (e) { console.error('[snd.e] 停止失败', e) }
    return
  }

  error.value = ''

  try {
    const r = await call<{ error: string }>('startSendEdit', payload())

    if (r?.error) { error.value = r.error; return }

    prog.value = { Running: true, Index: -1, Total: 0, Success: 0, Fail: 0 }
    startPoll()
  } catch (e) {
    console.error('[snd.e] 执行失败', e)
    error.value = String(e)
  }
}

/* ── 保存 ───────────────────────────────────────────────────── */

async function save(): Promise<void> {
  busy.value = true
  error.value = ''

  try {
    const r = await call<{ error: string }>('saveSendEdit', payload())

    if (r?.error) { error.value = r.error; return }

    emit('saved')
    await close()
  } catch (e) {
    console.error('[snd.e] 保存失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}

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
          <span class="zh">{{ t('snd.e.title') }}</span>
          <span class="sub">Controls/SendEdit</span>
        </div>
        <button class="x" :title="t('dlg.cancel')" @click="close">
          <svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg>
        </button>
      </header>

      <div v-if="!f" class="loading">{{ error || t('proxy.working') }}</div>

      <div v-else class="bd">
        <!-- 字段。跑的时候锁住 —— 执行的是保存后的那份，中途改会对不上 -->
        <div class="row" :class="{ off: prog.Running }">
          <div class="k">{{ t('col.sendName') }}</div>
          <div class="v">
            <input v-model="f.Name" class="inp" spellcheck="false" :disabled="prog.Running"
                   :placeholder="t('snd.e.namePh')">
          </div>
        </div>

        <div class="row" :class="{ off: prog.Running }">
          <div class="k">{{ t('col.socket') }}</div>
          <div class="v">
            <!--
              ⚠️ 系统套接字没设过时<b>不给勾</b>：勾上也只会在执行时被 C# 拦下
              （`Send.BlockedBySystemSocket`），等于让人先做一个必然失败的选择。

              这里<b>不需要「已勾着的留一条取消的退路」</b> —— 「勾着 + 未设置」这个状态
              已经不可达，靠的是三条一起成立（少一条就得把退路加回来）：
                ① 这个勾选框只在 SystemSocket > 0 时才点得开；
                ② SSystemSocket <b>不过库</b>，每次启动一律 false（见 LoadSendList_FromDB）；
                ③ SystemSocket <b>只增不减</b> —— 两个 SetSystemSocket_By*Id 与机器人那条指令
                   都只在 > 0 时才赋值，没有任何路径能把它打回 0。
            -->
            <button class="chk" :class="{ on: f.UseSystemSocket }"
                    :disabled="prog.Running || f.SystemSocket <= 0"
                    :title="f.SystemSocket <= 0 ? t('snd.e.sysSocketNeed') : ''"
                    @click="f.UseSystemSocket = !f.UseSystemSocket">
              <i />{{ t('snd.e.useSysSocket') }}
            </button>
            <!--
              把号码显示出来：勾了却没设过（0）时执行会被 C# 拦下，
              事先看得见比事后弹错误好。
            -->
            <span class="tg" :class="f.SystemSocket > 0 ? 'ok' : 'bad'">
              {{ f.SystemSocket > 0 ? f.SystemSocket : t('snd.e.sysSocketUnset') }}
            </span>
            <span class="tip">{{ t('snd.e.socketHint') }}</span>
          </div>
        </div>

        <div class="row" :class="{ off: prog.Running }">
          <div class="k">{{ t('col.loop') }}</div>
          <div class="v">
            <input v-model.number="f.LoopCount" class="inp num" type="number" min="1" max="999999"
                   :disabled="prog.Running">
            <span class="k2">{{ t('snd.loopTimes') }}</span>
            <span class="k2">·</span>
            <span class="k2">{{ t('snd.e.interval') }}</span>
            <input v-model.number="f.LoopInterval" class="inp num" type="number" min="0" max="999999"
                   :disabled="prog.Running">
            <span class="k2">{{ t('snd.loopMs') }}</span>
            <span class="tip">{{ t('snd.e.intervalHint') }}</span>
          </div>
        </div>

        <div class="row" :class="{ off: prog.Running }">
          <div class="k">{{ t('col.notes') }}</div>
          <div class="v">
            <input v-model="f.Notes" class="inp" spellcheck="false">
          </div>
        </div>

        <!-- 执行条。与发送列表的状态条同一套观感 -->
        <div class="runbar">
          <button class="btn run" :class="{ on: prog.Running }" :disabled="!rows.length" @click="toggleRun">
            <!-- 与代理数据页「开始代理 / 停止」同一对图标：三角 = 开始，方块 = 停止 -->
            <svg v-if="prog.Running" class="ico" viewBox="0 0 24 24"><rect x="6" y="6" width="12" height="12" /></svg>
            <svg v-else class="ico" viewBox="0 0 24 24"><path d="M7 4l13 8-13 8z" /></svg>
            {{ prog.Running ? t('snd.stop') : t('snd.e.execute') }}
          </button>

          <span class="cnt run">{{ t('col.execCount') }} <b>{{ prog.Total }}</b></span>
          <span class="cnt ok">{{ t('col.success') }} <b>{{ prog.Success }}</b></span>
          <span class="cnt bad">{{ t('col.fail') }} <b>{{ prog.Fail }}</b></span>

          <span class="grow" />

          <button class="btn" :disabled="prog.Running" @click="collectionCmd('importSendCollection')">
            {{ t('snd.e.import') }}
          </button>
          <button class="btn" :disabled="!rows.length" @click="collectionCmd('exportSendCollection')">
            {{ t('snd.e.export') }}
          </button>
          <button class="btn danger" :disabled="!rows.length || prog.Running"
                  @click="collectionCmd('clearSendCollection')">
            {{ t('snd.e.clear') }}
          </button>
        </div>

        <!-- 发送集 -->
        <div class="tbl list-page">
          <!--
            表头每一格都带着与数据行同名的 class（no / ty / so / ad / len / dt / ops）——
            对齐规则就能按 class 写、表头与数据行共用一条。
            <b>别改回按列号写</b>：这张表的列已经增删过三轮，每次都要把一串 nth-child 重编，
            漏一个就是某列悄悄跑偏。
          -->
          <div class="tbody">
            <!-- 表头在滚动容器<b>里面</b>（sticky）：放在外面时它比行宽出一条滚动条，操作列就和表头错开 -->
            <div class="head">
              <span class="no">{{ t('col.id') }}</span>
              <span class="ty">{{ t('col.type') }}</span>
              <span class="so">{{ t('col.socket') }}</span>
              <span class="ad">{{ t('snd.e.from') }}</span>
              <span class="ad">{{ t('snd.e.to') }}</span>
              <span class="len">{{ t('col.len') }}</span>
              <span class="dt">{{ t('col.data') }}</span>
              <span class="ops">{{ t('col.ops') }}</span>
            </div>


            <div v-if="!rows.length" class="empty">{{ t('snd.e.empty') }}</div>

            <div
              v-for="(r, i) in rows"
              v-else
              :key="r.Id"
              class="row2"
              :class="{ sel: picked.has(r.Id), run: prog.Index === i && prog.Running }"
              @click="onRowClick(r, $event, i)"
              @contextmenu.prevent="menuAt = { x: $event.clientX, y: $event.clientY }"
              @dblclick="onRowDblClick($event, r)"
            >
              <span class="no">{{ i + 1 }}</span>
              <span class="ty">{{ typeText[r.Type] ?? r.Type }}</span>
              <span class="so">{{ r.Socket }}</span>
              <span class="ad">{{ r.From }}</span>
              <span class="ad">{{ r.To }}</span>
              <span class="len">{{ r.Len }}</span>
              <span class="dt">{{ r.Preview }}</span>
              <span class="ops">
                <button class="op" :title="t('acct.op.edit')" :disabled="prog.Running"
                        @click.stop="editTarget = { list: 'send', id: Number(r.Id) }">
                  <svg class="ico" viewBox="0 0 24 24"><path d="M4 20h4L20 8l-4-4L4 16z" /></svg>
                </button>
                <button class="op del" :title="t('acct.op.del')" :disabled="prog.Running"
                        @click.stop="picked = new Set([r.Id]); onMenuPick('delete')">
                  <svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg>
                </button>
              </span>
            </div>
          </div>
        </div>

      </div>

      <footer class="ft">
        <span v-if="error" class="err">{{ error }}</span>
        <span class="grow" />
        <button class="btn" :disabled="busy" @click="close">{{ t('dlg.cancel') }}</button>
        <button class="btn primary" :disabled="busy || !f || prog.Running" @click="save">
          {{ busy ? t('proxy.working') : t('set.save') }}
        </button>
      </footer>

      <ContextMenu :at="menuAt" :items="menuItems" @pick="onMenuPick" @close="menuAt = null" />

      <!-- 封包编辑：改的是工作副本，保存后重取一遍发送集让预览 / 长度跟上 -->
      <PacketEdit :target="editTarget" @close="editTarget = null" @saved="reload" />
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

/*
  原来是整块滚动（.bd 自己 overflow: auto）。改成 flex 列 ——
  字段与执行条定高，发送集表吃掉剩下的全部高度，滚动收进表内部。
  这样窗口拉高时长的是表，而不是在字段下面留一片空白。
*/
.bd {
  flex: 1;
  min-height: 0;
  display: flex;
  flex-direction: column;
  overflow: hidden;
  padding: 10px 0 12px;
}

.bd > .row,
.bd > .runbar { flex: none; }

/* 字段行 */
.row {
  display: grid;
  grid-template-columns: 92px 1fr;
  align-items: center;
  gap: 12px;
  padding: 3px 18px;
  min-height: 30px;
}

.row > .k { font-size: var(--fs-body); color: var(--muted); }
.row > .v { display: flex; align-items: center; gap: 12px; min-width: 0; }
.row.off > .k { opacity: .45; }

.k2 { font-size: var(--fs-body); color: var(--muted); }
.tip { font-size: var(--fs-small); color: var(--dim2); }

/* 基样式在 style.css 的 .inp，这里只补布局 */
.inp { flex: 1; min-width: 0; }

.inp.num { flex: none; width: 92px; text-align: center; }

/* 基样式在 style.css 的「勾选框 / 单选框」，这一屏没有需要覆盖的 */

.tg {
  flex: none;
  padding: 4px 7px 4px;   /* 上 +1 下 -1：字形在 em 框里偏上 1px（上伸 9 / 下伸 3，实测），补回来 */
  border: 1px solid;
  font-family: var(--share);
  font-size: var(--fs-label);
  /* 显式 1：默认行高会把行距全压在字的下面，字在框里偏上（与按钮同一个问题）*/
  line-height: 1;
  letter-spacing: .04em;
}

.tg.ok { border-color: rgb(var(--green-rgb) / 35%); color: var(--green); }
.tg.bad { border-color: rgb(var(--danger-rgb) / 30%); color: var(--danger); }

/* 执行条 */
.runbar {
  display: flex;
  align-items: center;
  gap: 10px;
  margin: 8px 18px 6px;
  padding: 7px 12px;
  border: 1px solid var(--border);
  background: rgb(var(--inset-rgb) / 20%);
}

.grow { flex: 1; }

.runbar .cnt { font-size: var(--fs-label); color: var(--muted); font-family: var(--share); letter-spacing: .06em; position: relative; top: 1px; }   /* 与大一号的数字排在一行，实测偏高 1.25px（2026-09-13 字体度量覆写后按 100% 缩放实测重调） */
.runbar .cnt b { font-family: var(--mono); font-variant-numeric: tabular-nums; }
.runbar .cnt.run b { color: var(--cyan); }
.runbar .cnt.ok b { color: var(--green); }
.runbar .cnt.bad b { color: var(--danger); }

.btn {
  flex: none;
  padding: 8px 13px 8px;   /* 上 +1 下 -1：字形在 em 框里偏上 1px（上伸 9 / 下伸 3，实测），补回来 */
  background: transparent;
  border: 1px solid var(--border);
  color: var(--gray);
  font-family: var(--share);
  font-size: var(--btn-size);
  /* 显式 1：Share Tech Mono 在 line-height: normal 下会把行距全压在字的下面，字号一大就明显偏上（实测） */
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
.btn.danger { border-color: rgb(var(--danger-rgb) / 30%); color: var(--danger); }
.btn.danger:hover:not(:disabled) { background: rgb(var(--danger-rgb) / 12%); border-color: var(--danger); }

.btn.run { display: inline-flex; align-items: center; gap: 7px; border-color: rgb(var(--green-rgb) / 45%); color: var(--green); }
.btn.run .ico {
  /*
    11px 而不是代理状态条那枚的 13px：这个按钮的内容高度是 10.5px 的文字行，
    图标一超过它就把整个按钮撑高。再用 -1px 的上下外边距把图标的占位收回到 9px，
    按钮高度与旁边那些纯文字按钮完全一致。
  */
  width: 11px;
  height: 11px;
  margin: -1px 0;
  flex: none;
}
.btn.run.on { border-color: rgb(var(--danger-rgb) / 30%); color: var(--danger); }

/*
  悬停要压过公共的 .btn:hover（那条是青色，(0,3,0)）：开始态照 .btn.primary 的绿、
  停止态照 .btn.danger 的红，否则这个绿按钮一悬停就变蓝，与旁边的「新增发送」不一样。
*/
.btn.run:hover:not(:disabled) { background: rgb(var(--green-rgb) / 10%); border-color: var(--green); color: var(--green); }
.btn.run.on:hover:not(:disabled) { background: rgb(var(--danger-rgb) / 12%); border-color: var(--danger); color: var(--danger); }
.btn.run.on .ico { animation: pulse 1.1s ease-in-out infinite; }

@keyframes pulse { 50% { opacity: .25; } }

@media (prefers-reduced-motion: reduce) {
  .btn.run.on .ico { animation: none; }
}

/* 发送集表 */
.tbl {
  flex: 1;
  min-height: 0;
  display: flex;
  flex-direction: column;
  margin: 0 18px 2px;
  border: 1px solid var(--border);
  background: var(--sink);
}

/* 表头在 .tbody 里 sticky（style.css 的 .list-page .head 已经是 sticky + top: 0）*/

/*
  这张表<b>没有虚拟滚动</b>：发送集是人手攒出来的重放序列，几十条就顶天了，
  与账号（几万）不是一个量级。铺开直接滚。
*/
.tbody { flex: 1; min-height: 0; overflow-y: auto; }

.head,
.row2 {
  display: grid;
  /*
    ⚠️ <b>不要改成一排 auto</b>。auto 让每列缩到自己内容那么宽，
    再配上一个 1fr，结果是前八列全挤在左边、余量整块堆给数据列 —— 试过，很难看。
    定宽 + 两个地址各给 1fr，列与列之间才有呼吸，地址长了也还能自己伸。
  */
  /*
    两个地址列<b>定宽</b>而不是 1fr：它们装的是 "101.227.22.133:443" 这种
    长度可预期的串（18 字符，12px 等宽字约 130px），给 1fr 的话会跟着窗口一起长，
    结果是地址后面拖一大截空白、真正需要宽度的数据列反而不够。
    142px 留了一点余量；再长（IPv6）就省略号，那是极少数。

    只留数据列一个 1fr —— 剩余宽度全给它。
  */
  grid-template-columns: 36px 78px 74px 142px 142px 78px minmax(160px, 1fr) 96px;   /* 序号 36、类型 78、套接字 74（ru Сокет 70）、地址 142、长度 78（ru Длина 73）、操作 96（ru Действия 90）—— 都从数据列的 480 余量里出 */
  align-items: center;
  gap: 8px;
  /*
    ⚠️ <b>必须与 style.css 里 .list-page .head 的 14px 一致</b>。
    这张表的表头被那条全局规则匹配（外层 .tbl 带了 list-page），数据行却叫 .row2、
    匹配不到 —— 两边特异度相同时全局赢（见那个文件顶部的说明），
    于是表头 14px、数据行 10px，整条表头比内容右移 4px。
  */
  padding: 0 14px;
  font-size: var(--fs-body);
}

.row2 {
  height: 30px;
  border-bottom: 1px solid rgb(var(--border-rgb) / 45%);
  color: var(--soft);
  cursor: default;
}

.row2:hover { background: rgb(var(--tint-rgb) / 4%); }
.row2.sel { background: rgb(var(--cyan-rgb) / 6%); box-shadow: inset 2px 0 0 var(--cyan); }

/* 正在发的那一条。间隔为 0 时不会高亮，见脚本里 startPoll 的说明 */
.row2.run { background: rgb(var(--green-rgb) / 12%); box-shadow: inset 2px 0 0 var(--green); }

.row2 > span,
.head > span { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }

/*
  对齐一律<b>按 class 写，且表头与数据行共用同一条规则</b>。
  表头每格都带了与数据行同名的 class，所以列增删时不必重编任何序号 ——
  这张表的列已经改过三轮，之前用 nth-child 每次都得跟着数一遍。
*/
.head > span,
.row2 > span { text-align: center; }

/* 两个地址与数据靠左：长度不可预知的文本，居中会让每行起点忽左忽右 */
.head > span.ad,
.row2 > span.ad,
.head > span.dt,
.row2 > span.dt { text-align: left; }

/* 长度靠右：数字右对齐才好比大小 */
.head > span.len,
.row2 > span.len { text-align: right; }

/*
  数据列单独往右让一段：它左边是右对齐的长度列，两串数字贴着 8px 的栅格间距，
  看着像连成了一条。加 10px（合计 18px）把两者分开，不去动 gap —— 那会撑开每一列。
*/
.head > span.dt,
.row2 > span.dt { padding-left: 10px; }

/*
  类型 · 套接字 · 操作三列用 flex 居中：操作格里装的是按钮，text-align 管不到；
  另外两列跟着用同一种机制，免得同一张表里两套居中方式并存、总差一两个像素。
*/
.head > span.ty,
.row2 > span.ty,
.head > span.so,
.row2 > span.so,
.head > span.ops,
.row2 > span.ops {
  display: flex;
  align-items: center;
  justify-content: center;
}

.no { color: var(--dim); font-variant-numeric: tabular-nums; }
.ty { color: var(--acc-violet); }
.so { color: var(--muted); font-variant-numeric: tabular-nums; }
.ad { color: var(--dim3); font-family: var(--mono); font-size: var(--fs-body); }
.len { color: var(--cyan); font-variant-numeric: tabular-nums; }
.dt { color: var(--acc-green2); font-family: var(--mono); font-size: var(--fs-body); }

.ft {
  flex: none;
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 12px 18px;
  border-top: 1px solid var(--border);
  background: var(--panel);
}

.err { font-size: var(--fs-small); color: var(--danger); }
</style>
