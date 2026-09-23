<script setup lang="ts">
/*
  进程设置 —— 2026-09-23 由旧屏重做而来（内置 mihomo 内核接管进程抓取）。

  【与旧屏的区别】
  旧屏是「进程 → 驱动 → SunnyNet → 强制转代理 → WPE 的 SOCKS5」一条链路 + 四张步骤卡。
  现在抓取改由内置 mihomo 内核完成，链路固定为：

      勾选的进程 ── TUN ──▶ mihomo ── SOCKS5 ──▶ WPE 的 SOCKS5（抓包 / 滤镜）──▶ 互联网

  【内核不再随「启动代理」加载】
  内核只在需要「把进程强制转代理」时才用得上。所以这里第一步是「启用进程拦截」开关：
  打开并保存 → 内核加载（代理没起时先落库，启动代理再加载）；关掉保存 → 内核卸载、网络立刻恢复。
  「启动代理」只在开关已打开时才顺带把内核拉起来。

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

/* 合并后的进程行：运行中的 + 已保存但没启动的 */
interface Row {
  ModuleName: string
  ProcessName: string
  ProcessID: number
  ProcessPath: string
  running: boolean
}

const procs = shallowRef<ProcessRow[]>([])
const names = useList<ProcessRow>(FeedList.SelectProcess)
const intercepted = ref<Set<string>>(new Set())
const filter = ref('')

/** 拦截名单（小写进程名集合）：已保存的 ∪ 运行中且 IsCheck 的。 */
function rebuildIntercepted(): void {
  const s = new Set<string>()
  for (const p of names.value) { if (p.ModuleName) s.add(p.ModuleName.toLowerCase()) }
  for (const p of procs.value) { if (p.IsCheck && p.ModuleName) s.add(p.ModuleName.toLowerCase()) }
  intercepted.value = s
}

watch([procs, names], rebuildIntercepted, { immediate: true })

const rows = computed<Row[]>(() => {
  const out: Row[] = []
  const seen = new Set<string>()

  for (const p of procs.value) {
    const k = (p.ModuleName || '').toLowerCase()
    if (!k) { continue }
    seen.add(k)
    out.push({ ModuleName: p.ModuleName, ProcessName: p.ProcessName, ProcessID: p.ProcessID, ProcessPath: p.ProcessPath, running: true })
  }
  for (const p of names.value) {
    const k = (p.ModuleName || '').toLowerCase()
    if (!k || seen.has(k)) { continue }
    seen.add(k)
    out.push({ ModuleName: p.ModuleName, ProcessName: p.ProcessName || p.ModuleName, ProcessID: 0, ProcessPath: p.ProcessPath, running: false })
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

watch(() => props.open, async (on) => {
  if (!on) return
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
  const k = r.ModuleName.toLowerCase()
  const on = intercepted.value.has(k)

  if (!on && (!r.running || r.ProcessID <= 0)) { return }

  const next = new Set(intercepted.value)
  if (on) next.delete(k)
  else next.add(k)
  intercepted.value = next

  try {
    if (on) {
      await call('removeSelectProcessName', { name: r.ModuleName })
    } else {
      const res = await call<{ ok: boolean }>('addSelectProcessName', { pid: r.ProcessID })
      if (!res?.ok) {
        const back = new Set(intercepted.value)
        back.delete(k)
        intercepted.value = back
        pushToast('warning', t('ps.noModule'))
      }
    }
  } catch (e) {
    console.error('[ps] 切换拦截失败', e)
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
  <SettingsModal :open="props.open" :title="t('set.process')" subtitle="mihomo · TUN" :busy="busy" :error="error" :width="980"
                 @update:open="emit('update:open', $event)" @save="save">
    <div class="setf list-page ps">

      <!-- 内核状态条 -->
      <div class="stat">
        <span class="led" :class="f.KernelReady ? 'on' : (f.KernelRunning ? 'wait' : 'off')" />
        <span class="sl">{{ t('mh.kernel') }}</span>
        <span class="sv" :class="f.KernelReady ? 'ok' : (f.KernelRunning ? 'warn' : 'dim')">
          {{ f.KernelReady ? t('mh.ready') : t('mh.notReady') }}
        </span>
        <span v-if="f.KernelVersion" class="sv dim">{{ t('mh.version') }} {{ f.KernelVersion }}</span>
        <span class="grow" />
        <span class="sv dim">SOCKS5 :{{ f.Socks5Port }}</span>
      </div>

      <p v-if="f.LastError" class="hint bad">{{ f.LastError }}</p>

      <!-- 第一步：启用进程拦截（= 加载内核） -->
      <section class="sec">
        <div class="grp">{{ t('mh.kernel') }}</div>

        <div class="row">
          <div class="k">{{ t('mh.enable') }}</div>
          <div class="v">
            <button class="chk" :class="{ on: f.EnableMihomo }" @click="f.EnableMihomo = !f.EnableMihomo">
              <i />{{ f.EnableMihomo ? t('ps.s4.on') : t('ps.s4.off') }}
            </button>
          </div>
        </div>
        <p class="hint">{{ t('mh.enableHint') }}</p>
        <p v-if="f.EnableMihomo && !f.ProxyRunning" class="hint warn">{{ t('mh.needProxy') }}</p>

        <div class="row" :class="{ off: !f.EnableMihomo }">
          <div class="k">{{ t('mh.stack') }}</div>
          <div class="v">
            <button v-for="s in STACKS" :key="s" class="rd" :class="{ on: f.TunStack === s }" @click="f.TunStack = s"><i />{{ s }}</button>
          </div>
        </div>
        <p class="hint">{{ t('mh.stackHint') }}</p>

        <div class="row" :class="{ off: !f.EnableMihomo }">
          <div class="k">{{ t('mh.dns') }}</div>
          <div class="v">
            <button v-for="d in DNSMODES" :key="d" class="rd" :class="{ on: f.DnsMode === d }" @click="f.DnsMode = d"><i />{{ d }}</button>
          </div>
        </div>
        <p class="hint">{{ t('mh.dnsHint') }}</p>
      </section>

      <!-- 第二步：拦截进程（一块表） -->
      <section class="sec" :class="{ off: !f.EnableMihomo }">
        <div class="grp">{{ t('mh.procs') }}</div>

        <div class="tbl">
          <div class="tbar">
            <input v-model="filter" class="inp sm" spellcheck="false" :placeholder="t('ps.filterPh')">
            <span class="grow" />
            <span class="cnt">{{ intercepted.size }} / {{ rows.length }}</span>
            <button class="sbtn" :disabled="loading" @click="refresh">{{ loading ? t('proxy.working') : t('ps.refresh') }}</button>
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
              <span class="ck"><button class="chk" :class="{ on: intercepted.has(r.ModuleName.toLowerCase()) }" @click.stop="toggleRow(r)"><i /></button></span>
              <span class="ico"><img v-if="iconOf(r.ProcessPath)" :src="iconOf(r.ProcessPath)" alt=""><i v-else class="ph" /></span>
              <span class="name">{{ r.ProcessName }}</span>
              <span class="pid">{{ r.running ? r.ProcessID : '—' }}</span>
            </div>
          </div>
          <div class="tf">{{ t('ps.byNameHint') }}</div>
        </div>
      </section>

      <p class="hint tail">{{ t('mh.loop') }}</p>
    </div>
  </SettingsModal>
</template>

<style scoped>
/*
  竖直呼吸量：窗口矮就收进程表高度。默认 1280×800 在 125% 缩放下只有 640 CSS 高。
*/
.ps { --tall: 280px; }

@media (max-height: 760px) { .ps { --tall: 200px; } }
@media (max-height: 620px) { .ps { --tall: 150px; } }

/* 内核状态条 */
.stat {
  display: flex;
  align-items: center;
  gap: 10px;
  margin: 0 20px 8px;
  padding: 8px 12px;
  border: 1px solid var(--border);
  background: var(--sink);
}

.stat .led { flex: none; width: 8px; height: 8px; border-radius: 50%; background: var(--dim3); }
.stat .led.on { background: var(--green); box-shadow: 0 0 7px var(--green); }
.stat .led.wait { background: var(--amber); }
.stat .led.off { background: var(--dim3); }
.stat .grow { flex: 1; }
.stat .sl { font-family: var(--share); font-size: var(--fs-label); letter-spacing: .12em; text-transform: uppercase; color: var(--cyan); }
.stat .sv { font-size: var(--fs-small); color: var(--soft); }
.stat .sv.ok { color: var(--green); }
.stat .sv.warn { color: var(--amber); }
.stat .sv.dim { color: var(--dim); font-family: var(--mono); }

/* 进程表（一块） */
.cap { position: relative; top: 1px; font-family: var(--share); font-size: var(--fs-label); letter-spacing: .12em; text-transform: uppercase; color: var(--cyan); white-space: nowrap; }
.cnt { font-family: var(--mono); font-size: var(--fs-small); color: var(--muted); }
.tbody.tall { height: var(--tall); max-height: var(--tall); }
.tf { padding: 6px 12px; border-top: 1px solid var(--border); font-size: var(--fs-small); color: var(--dim2); }

.ps .head.hp, .ps .tr.hp { grid-template-columns: 34px 26px minmax(120px, 1fr) 64px; }
.ps .tr { height: 30px; cursor: pointer; }
.ps .head > span, .ps .tr > span { text-align: left; }
.ps .head > span.pid, .ps .tr > span.pid { text-align: center; }

.ico { display: flex; align-items: center; justify-content: center; }
.ico img { width: 16px; height: 16px; image-rendering: auto; }
.ico .ph { width: 16px; height: 16px; }
.pid { font-family: var(--mono); font-size: var(--fs-body); color: var(--muted); font-variant-numeric: tabular-nums; }
.name { color: var(--gray); }

.tail { padding: 0 20px; margin: 2px 0 6px; }
</style>
