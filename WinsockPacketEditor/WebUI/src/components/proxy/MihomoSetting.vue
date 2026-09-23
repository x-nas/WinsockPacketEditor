<script setup lang="ts">
/*
  进程设置 —— 2026-09-23 由旧屏重做而来（内置 mihomo 内核接管进程抓取）。

  【与旧屏的区别】
  旧屏是「进程 → 驱动 → SunnyNet → 强制转代理 → WPE 的 SOCKS5」一条链路 + 四张步骤卡。
  现在抓取改由内置 mihomo 内核完成，链路固定为：

      勾选的进程 ── TUN ──▶ mihomo ── SOCKS5 ──▶ WPE 的 SOCKS5（抓包 / 滤镜）──▶ 互联网

  【布局】左列 = 内核设置（启用开关 + TUN 栈 + DNS 模式），右列 = 拦截进程表。
  并排是为了让进程表一次能看到更多行；没勾「启用进程拦截」时，右边的列表与栈/DNS 全部置灰不可调。

  【内核不再随「启动代理」加载】内核只在需要「把进程强制转代理」时才用得上，所以这里第一步是
  「启用进程拦截」开关：打开并保存 → 内核加载（代理没起时先落库，启动代理再加载）；关掉保存 → 内核卸载。
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

watch(() => props.open, async (on2) => {
  if (!on2) return
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

  const k = r.ModuleName.toLowerCase()
  const isOn = intercepted.value.has(k)

  if (!isOn && (!r.running || r.ProcessID <= 0)) { return }

  const next = new Set(intercepted.value)
  if (isOn) next.delete(k)
  else next.add(k)
  intercepted.value = next

  try {
    if (isOn) {
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
  <SettingsModal :open="props.open" :title="t('set.process')" subtitle="mihomo · TUN" :busy="busy" :error="error"
                 :hint="on && !f.ProxyRunning ? t('mh.needProxy') : ''" :width="980"
                 @update:open="emit('update:open', $event)" @save="save">
    <div class="setf list-page ps">

      <div class="cols">
        <!-- 左：内核设置 -->
        <div class="col">
          <div class="grp">{{ t('mh.kernel') }}</div>

          <div class="swrow">
            <button class="chk" :class="{ on }" @click="f.EnableMihomo = !f.EnableMihomo"><i />{{ t('mh.enable') }}</button>
            <!-- 内核跑到哪一步：小灯 + 一句，不做成单独一块区域 -->
            <span class="tag" :class="f.KernelReady ? 'ok' : (f.KernelRunning ? 'warn' : 'dim')">
              {{ f.KernelReady ? t('mh.ready') : t('mh.notReady') }}
            </span>
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
        </div>

        <!-- 右：拦截进程 -->
        <div class="col">
          <div class="grp">{{ t('mh.procs') }}</div>

          <div class="tbl" :class="{ off: !on }">
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
                <span class="name">{{ r.ProcessName }}</span>
                <span class="pid">{{ r.running ? r.ProcessID : '—' }}</span>
              </div>
            </div>
            <div class="tf">{{ t('ps.byNameHint') }}</div>
          </div>
        </div>
      </div>

      <p class="hint tail">{{ t('mh.loop') }}</p>
    </div>
  </SettingsModal>
</template>

<style scoped>
/*
  竖直呼吸量：窗口矮就把进程表收矮。默认 1280×800 在 125% 缩放下只有 640 CSS 高。
  两列并排，左列窄（内核设置），右列吃掉剩余宽度（进程表）。
*/
.ps { --tall: 360px; --col-l: 340px; }

@media (max-height: 760px) { .ps { --tall: 300px; } }
@media (max-height: 620px) { .ps { --tall: 230px; --col-l: 300px; } }

.cols {
  display: grid;
  grid-template-columns: minmax(280px, var(--col-l)) minmax(0, 1fr);
  gap: 12px;
  padding: 4px 20px 0;
  align-items: start;
}

/* 列内把 .setf 的左右内边距归零（那是给整页留的），标签列也收窄 */
.ps .cols .col { min-width: 0; --setf-k: 92px; }
.ps .cols .grp { padding: 6px 0 6px; }
.ps .cols .row { padding-left: 0; padding-right: 0; }
.ps .cols .hint { padding-left: 0; padding-right: 0; margin: 2px 0 8px; }
.ps .cols .tbl { margin: 0; }
/*
  进程表高度<b>自适应</b>：最多 --tall 高，行少时贴着内容收起来，
  不在列表底部留一大片空白（原来是写死 height: --tall）。
*/
.ps .cols .tbody.tall { max-height: var(--tall); }

/* 启用开关一行：勾选框 + 一枚内核状态小标 */
.swrow { display: flex; align-items: center; gap: 10px; padding: 4px 0; min-height: 30px; flex-wrap: wrap; }

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

/* 没启用进程拦截：右列整块压暗并禁止交互（按钮自己 disabled，行点击在 JS 里也挡了） */
.ps .cols .tbl.off { opacity: .45; }
.ps .cols .tbl.off .tr { cursor: default; }

/* 进程表列 */
.cap { position: relative; top: 1px; font-family: var(--share); font-size: var(--fs-label); letter-spacing: .12em; text-transform: uppercase; color: var(--cyan); white-space: nowrap; }
.cnt { font-family: var(--mono); font-size: var(--fs-small); color: var(--muted); }
/* 说明文字离表格底边的距离，与左列最后一句 hint 到列的底边一致（都是 8px） */
.tf { padding: 6px 12px 8px; border-top: 1px solid var(--border); font-size: var(--fs-small); color: var(--dim2); }

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
