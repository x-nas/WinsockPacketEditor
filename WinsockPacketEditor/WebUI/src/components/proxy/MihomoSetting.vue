<script setup lang="ts">
/*
  进程设置 —— 2026-09-23 由旧屏重做而来（内置 mihomo 内核接管进程抓取）。

  【与旧屏的区别】
  旧屏是「进程 → 驱动 → SunnyNet → 强制转代理 → WPE 的 SOCKS5」一条链路 + 四张步骤卡。
  现在抓取改由内置 mihomo 内核完成，链路固定为：

      勾选的进程 ── TUN ──▶ mihomo ── SOCKS5 ──▶ WPE 的 SOCKS5（抓包 / 滤镜）──▶ 互联网

  【两段编号卡】与其它设置屏同一个形态（`.setf .sec` + `.grp`，编号由 CSS counter 生成）：
      01 · Mihomo 内核（启用开关 + TUN 栈 + DNS 模式）
      02 · 拦截进程（一块表）
  没启用进程拦截时，02 整卡压暗、控件与勾选全部 disabled —— 一眼看出要先启用。

  【内核不随「启动代理」加载】它只在需要「把进程强制转代理」时才用得上，所以这里要手动开。
  开关本身不落库，每次开 WPE 默认关闭。

  ⚠️ <b>只有一块进程表。</b>旧屏分「按编号拦截」和「按名称拦截」，是因为 SunnyNet 同时支持
  PID 与进程名两种拦截；而 mihomo 的规则只有 PROCESS-NAME / PROCESS-PATH（及正则版），
  <b>没有按 PID 的规则</b>（PID 每次启动都变）。所以一行 = 一个进程名，勾上就是一条 PROCESS-NAME 规则；
  已保存但当前没启动的进程也列出来（编号显示 —），取消勾选即移除。
*/
import { computed, ref, shallowRef, watch } from 'vue'
import { call } from '../../bridge'
import { FeedList, type ProcessRow } from '../../bridge/types'
import { t } from '../../i18n'
import { useList } from '../../stores/lists'
import { useSort } from '../../useSort'
import { pushToast } from '../../stores/toast'
import SettingsModal from './SettingsModal.vue'

const props = defineProps<{ open: boolean }>()
const emit = defineEmits<{ (e: 'update:open', v: boolean): void }>()

interface Setting {
  ProxyRunning: boolean; KernelRunning: boolean; KernelReady: boolean; KernelVersion: string
  EnableSocks5: boolean; Socks5Port: number; EnableAuth: boolean; IsAdmin: boolean; LastError: string
  EnableMihomo: boolean; TunStack: string; DnsMode: string
}

const EMPTY: Setting = {
  ProxyRunning: false, KernelRunning: false, KernelReady: false, KernelVersion: '',
  EnableSocks5: true, Socks5Port: 1080, EnableAuth: true, IsAdmin: true, LastError: '',
  EnableMihomo: false, TunStack: 'system', DnsMode: 'fake-ip',
}

const busy = ref(false)
const loading = ref(false)
const error = ref('')
const f = ref<Setting>({ ...EMPTY })

/** 没启用进程拦截时，栈 / DNS / 进程表全部置灰不可调。 */
const on = computed(() => f.value.EnableMihomo)

/* 合并后的进程行：运行中的 + 已保存但没启动的 */
interface Row {
  ModuleName: string
  ProcessName: string
  ProcessID: number
  ProcessPath: string
  running: boolean
  /** 同名进程的个数（0 = 已保存但当前没在跑）。>1 时在名字后面显示 ×N */
  count: number
}

const procs = shallowRef<ProcessRow[]>([])
const names = useList<ProcessRow>(FeedList.SelectProcess)
const filter = ref('')

/*
  拦截名单（小写进程名集合）。
  ⚠️ <b>唯一真源是后端推来的 SelectProcess 列表</b>（FeedPump 每拍整表 Replace），
  前端<b>不做</b>乐观更新 —— 之前那版点一下先本地勾上、再用 watch 从「procs 快照 + names」重建，
  重建时若 procs 快照还没算上刚加的条目，就会把勾选<b>抹掉</b>（点了没反应，而同名那条因为
  后端 IsCheck=true 反倒显示已选中）。现在点击只调桥，等列表推回来自然点亮，不存在这个竞态。
*/
const intercepted = computed<Set<string>>(() => {
  const s = new Set<string>()
  for (const p of names.value) { if (p.ModuleName) s.add(p.ModuleName.toLowerCase()) }
  return s
})

/*
  一行 = 一个进程名（一条 PROCESS-NAME 规则）。
  ⚠️ <b>运行中那段必须按 ModuleName 去重</b>：同名进程（7 个 msedge）只留一行、只计个数。
  漏掉这个判断会让同名进程各占一行，而 v-for 的 :key 用的就是 ModuleName —— key 一重复，
  Vue 就复用 DOM，点击绑到别的行上（「点一行、下面几行被选中」2026-09-23 撞到）。
*/
const rows = computed<Row[]>(() => {
  const out: Row[] = []
  const at = new Map<string, Row>()

  for (const p of procs.value) {
    const k = (p.ModuleName || '').toLowerCase()
    if (!k) { continue }

    const hit = at.get(k)
    if (hit) { hit.count++; continue }

    const row: Row = { ModuleName: p.ModuleName, ProcessName: p.ProcessName, ProcessID: p.ProcessID, ProcessPath: p.ProcessPath, running: true, count: 1 }
    at.set(k, row)
    out.push(row)
  }

  for (const p of names.value) {
    const k = (p.ModuleName || '').toLowerCase()
    if (!k || at.has(k)) { continue }

    const row: Row = { ModuleName: p.ModuleName, ProcessName: p.ProcessName || p.ModuleName, ProcessID: 0, ProcessPath: p.ProcessPath, running: false, count: 0 }
    at.set(k, row)
    out.push(row)
  }

  return out
})

const filtered = computed(() => {
  const q = filter.value.trim().toLowerCase()
  if (!q) { return rows.value }
  return rows.value.filter((r) => r.ProcessName.toLowerCase().includes(q) || r.ModuleName.toLowerCase().includes(q) || String(r.ProcessID).includes(q))
})

/* 表头排序（编号 / 进程名称）—— 接在过滤之后 */
const sort = useSort<Row>(filtered, {
  pid: (r) => r.ProcessID,
  name: (r) => r.ProcessName || '',
})
const shown = sort.sorted

/* TUN 栈 / DNS 模式的可选项：技术名词，照原样显示 */
const STACKS = ['system', 'gvisor', 'mixed'] as const
const DNSMODES = ['fake-ip', 'redir-host'] as const

/* ─────────────── 进程图标 ─────────────── */

const icons = ref<Record<string, string>>({})

function iconOf(path: string): string {
  return path ? (icons.value[path] ?? '') : ''
}

async function fetchIcons(list: { ProcessPath: string }[]): Promise<void> {
  const want = new Set<string>()
  for (const p of list) if (p.ProcessPath && icons.value[p.ProcessPath] === undefined) want.add(p.ProcessPath)
  if (!want.size) return
  try {
    const r = await call<{ icons: Record<string, string> }>('getProcessIcons', { paths: [...want] })
    const next = { ...icons.value }
    for (const path of want) {
      const png = r?.icons?.[path]
      next[path] = png ? 'data:image/png;base64,' + png : ''
    }
    icons.value = next
  } catch (e) {
    console.error('[ps] 取进程图标失败', e)
  }
}

watch(names, (list) => { void fetchIcons(list) }, { immediate: true })

watch(() => props.open, async (opened) => {
  if (!opened) return
  error.value = ''
  try {
    f.value = { ...EMPTY, ...(await call<Setting>('getMihomoSetting')) }
    await refresh()
  } catch (e) {
    console.error('[ps] 读取设置失败', e)
  }
}, { immediate: true })

async function refresh(): Promise<void> {
  loading.value = true
  try {
    const r = await call<{ rows: ProcessRow[] }>('getProcessRows')
    procs.value = r?.rows ?? []
    void fetchIcons(procs.value)
  } catch (e) {
    console.error('[ps] 取进程列表失败', e)
  } finally {
    loading.value = false
  }
}

/*
  勾选 / 取消：勾上 = 加一条 PROCESS-NAME 规则（走桥 addSelectProcessName，按 Pid 取到名字与路径）；
  取消 = 从名单里删掉。已保存但没启动的行 pid 为 0，只能取消、不能再勾上。
  前端先乐观改 intercepted，桥失败再退回。
*/
async function toggleRow(r: Row): Promise<void> {
  if (!on.value) { return }   // 没启用进程拦截时整块只读

  const isOn = intercepted.value.has(r.ModuleName.toLowerCase())

  //取消：从名单里删掉（运行中 / 已保存没跑的都走这条）
  if (isOn) {
    try { await call('removeSelectProcessName', { name: r.ModuleName }) }
    catch (e) { console.error('[ps] 移除拦截失败', e) }
    return
  }

  //勾上：只有运行中的、拿得到 Pid 的才能加（加的是「进程名」，同名进程一起生效）
  if (!r.running || r.ProcessID <= 0) { return }

  try {
    const res = await call<{ ok: boolean }>('addSelectProcessName', { pid: r.ProcessID })
    if (!res?.ok) { pushToast('warning', t('ps.noModule')) }
  } catch (e) {
    console.error('[ps] 添加拦截失败', e)
  }
}

async function save(): Promise<void> {
  busy.value = true
  error.value = ''
  try {
    const r = await call<{ error: string }>('saveMihomoSetting', {
      enable: f.value.EnableMihomo,
      tunStack: f.value.TunStack,
      dnsMode: f.value.DnsMode,
    })
    if (r?.error) { error.value = r.error; return }
    emit('update:open', false)
  } catch (e) {
    console.error('[ps] 保存失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}
</script>

<template>
  <SettingsModal :open="props.open" :title="t('set.process')" subtitle="mihomo · TUN" :busy="busy" :error="error"
                 :hint="on && !f.ProxyRunning ? t('mh.needProxy') : ''" :width="980"
                 @update:open="emit('update:open', $event)" @save="save">
    <div class="setf list-page ps">
      <!-- 01 / 02 并排：左列内核设置窄，右列进程表吃剩余宽度（一次能看更多进程） -->
      <div class="cols">

      <!-- 01 · Mihomo 内核 -->
      <section class="sec">
        <div class="grp">{{ t('mh.kernel') }}</div>

        <div class="row">
          <div class="k">{{ t('mh.enable') }}</div>
          <div class="v">
            <button class="chk" :class="{ on }" @click="f.EnableMihomo = !f.EnableMihomo"><i />{{ on ? t('ps.s4.on') : t('ps.s4.off') }}</button>
            <!-- 内核跑到哪一步：一枚小标，不做成单独一块 -->
            <span class="tag" :class="f.KernelReady ? 'ok' : (f.KernelRunning ? 'warn' : 'dim')">
              {{ f.KernelReady ? t('mh.ready') : t('mh.notReady') }}
            </span>
          </div>
        </div>
        <p class="hint">{{ t('mh.enableHint') }}</p>

        <div class="row" :class="{ off: !on }">
          <div class="k">{{ t('mh.stack') }}</div>
          <div class="v">
            <button v-for="s in STACKS" :key="s" class="rd" :class="{ on: f.TunStack === s }" :disabled="!on" @click="f.TunStack = s"><i />{{ s }}</button>
          </div>
        </div>
        <p class="hint">{{ t('mh.stackHint') }}</p>

        <div class="row" :class="{ off: !on }">
          <div class="k">{{ t('mh.dns') }}</div>
          <div class="v">
            <button v-for="d in DNSMODES" :key="d" class="rd" :class="{ on: f.DnsMode === d }" :disabled="!on" @click="f.DnsMode = d"><i />{{ d }}</button>
          </div>
        </div>
        <p class="hint">{{ t('mh.dnsHint') }}</p>

        <!-- 断环说明：钉在 01 卡的最下部（卡高度铺满整行，见 .cols 的 stretch） -->
        <p class="hint loop">{{ t('mh.loop') }}</p>
      </section>

      <!-- 02 · 拦截进程 -->
      <section class="sec" :class="{ off: !on }">
        <div class="grp">{{ t('mh.procs') }}</div>

        <div class="tbl">
          <div class="tbar">
            <input v-model="filter" class="inp sm" spellcheck="false" :disabled="!on" :placeholder="t('ps.filterPh')">
            <span class="grow" />
            <span class="cnt">{{ intercepted.size }} / {{ rows.length }}</span>
            <button class="sbtn" :disabled="!on || loading" @click="refresh">{{ loading ? t('proxy.working') : t('ps.refresh') }}</button>
          </div>
          <div class="tbody tall">
            <div class="head hp">
              <span class="ck" />
              <span class="ico" />
              <span class="name so" :class="{ on: sort.active('name') }" @click="sort.toggle('name')">{{ t('ps.processName') }}<i class="ar">{{ sort.mark('name') }}</i></span>
              <span class="pid so" :class="{ on: sort.active('pid') }" @click="sort.toggle('pid')">{{ t('ps.pid') }}<i class="ar">{{ sort.mark('pid') }}</i></span>
            </div>
            <div v-if="!shown.length" class="empty">{{ loading ? t('proxy.working') : t('ps.emptyProcs') }}</div>
            <div
              v-for="r in shown"
              v-else
              :key="r.ModuleName.toLowerCase()"
              class="tr hp"
              :class="{ sel: intercepted.has(r.ModuleName.toLowerCase()), off: !r.running }"
              :title="r.ProcessPath"
              @click="toggleRow(r)"
            >
              <span class="ck"><button class="chk" :disabled="!on" :class="{ on: intercepted.has(r.ModuleName.toLowerCase()) }" @click.stop="toggleRow(r)"><i /></button></span>
              <span class="ico"><img v-if="iconOf(r.ProcessPath)" :src="iconOf(r.ProcessPath)" alt=""><i v-else class="ph" /></span>
              <span class="name">{{ r.ProcessName }}<i v-if="r.count > 1" class="dup">×{{ r.count }}</i></span>
              <span class="pid">{{ r.running ? r.ProcessID : '—' }}</span>
            </div>
          </div>
          <div class="tf">{{ t('ps.byNameHint') }}</div>
        </div>
      </section>
      </div>
    </div>
  </SettingsModal>
</template>

<style scoped>
/*
  进程表高度：最多 --tall 高，行少时贴着内容收起来（不写死 height），行多就滚动。
  窗口矮就收矮一点。默认 1280×800 在 125% 缩放下只有 640 CSS 高。
*/
.ps { --tall: 320px; }

@media (max-height: 760px) { .ps { --tall: 250px; } }
@media (max-height: 620px) { .ps { --tall: 190px; } }

/*
  01 / 02 两张编号卡并排。编号仍是 CSS counter 按 DOM 顺序生成（左 01、右 02）。
  ⚠️ .setf .sec 自带 margin: 10px 20px 12px，卡片进网格后左右外边距要归零（否则卡间凭空多 40px），
  由容器统一的 20px 内边距与 12px 列间距负责。
*/
.cols {
  display: grid;
  grid-template-columns: minmax(300px, 360px) minmax(0, 1fr);
  gap: 0 12px;
  padding: 0 20px;
  /*
    ⚠️ 不写 align-items: start —— 默认 stretch，两张卡才会铺满整行高度，
    于是 01 与 02 的底边齐平；01 卡里最后那句断环说明靠 margin-top: auto 顶到底部。
  */
}

/* 卡片自己撑成 flex 列，好把内容顶到上、把最后一句钉到下 */
.ps .cols > .sec { margin: 10px 0 12px; display: flex; flex-direction: column; }
/* 左卡窄：标签列收窄，给 system/gvisor/mixed 三个选项留出宽度 */
.ps .cols > .sec:first-child { --setf-k: 96px; }
/* 断环说明钉在 01 卡最下部 */
.ps .cols .loop { margin-top: auto; padding-top: 6px; padding-bottom: 2px; }

/* 内核状态小标（跟在启用开关后面，不做成单独区域） */
.tag {
  font-family: var(--share);
  font-size: var(--fs-label);
  letter-spacing: .08em;
  text-transform: uppercase;
  color: var(--dim);
  white-space: nowrap;
}
.tag.ok { color: var(--green); }
.tag.warn { color: var(--amber); }
.tag.dim { color: var(--dim); }

/* 02 卡里的进程表 */
.cnt { font-family: var(--mono); font-size: var(--fs-small); color: var(--muted); }
.ps .tbody.tall { max-height: var(--tall); }
/* 说明文字离表格底边的距离，与卡内最后一句 hint 的下内边距一致 */
.tf { padding: 6px 14px 8px; border-top: 1px solid var(--border); font-size: var(--fs-small); color: var(--dim2); }

.ps .head.hp, .ps .tr.hp { grid-template-columns: 34px 26px minmax(140px, 1fr) 64px; }
.ps .tr { height: 30px; cursor: pointer; }
.ps .head > span, .ps .tr > span { text-align: left; }
.ps .head > span.pid, .ps .tr > span.pid { text-align: center; }

.ico { display: flex; align-items: center; justify-content: center; }
.ico img { width: 16px; height: 16px; image-rendering: auto; }
.ico .ph { width: 16px; height: 16px; }
.pid { font-family: var(--mono); font-size: var(--fs-body); color: var(--muted); font-variant-numeric: tabular-nums; }
.name { color: var(--gray); }
/* 同名进程的个数：一行就是一条按名规则，×N 提示它同时管着几个进程 */
.name .dup { margin-left: 6px; font-style: normal; font-family: var(--mono); font-size: var(--fs-caption); color: var(--dim3); }

/* 没启用进程拦截：整卡压暗，行点击在 JS 里也挡了 */
.sec.off .tr { cursor: default; }
</style>
