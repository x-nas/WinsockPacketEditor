<script setup lang="ts">
/*
  mihomo 模式设置 —— 2026-09-23 由「进程设置」重做而来。

  【这一版换掉了什么】
  旧屏讲的是「进程 → 驱动 → SunnyNet → 强制转代理 → WPE 的 SOCKS5」一条链路，四张步骤卡。
  现在抓取改由内置 mihomo 内核完成，链路固定为：

      勾选的进程 ── TUN ──▶ mihomo ── SOCKS5 ──▶ WPE 的 SOCKS5（抓包 / 滤镜）──▶ 互联网

  所以这一屏只剩两件事：① 选哪些进程；② 内核怎么接管（TUN 栈 / DNS 模式）。
  驱动类型、转代理地址 / 认证、指定端口、卸载驱动全部随 SunnyNet 一起去掉。
  断环（WPE 自身 / 启动器 / 内核固定直连）由 C# 写进内核配置，界面上只作说明。

  数据：getMihomoSetting（MihomoSettingRow）。进程名单走原有的 getProcessRows +
  addSelectProcessName / removeSelectProcessName（落库在 ProxyMode.SelectProcessNames）。
  图标按去重路径一次批量取（getProcessIcons）、按路径记忆化 —— DTO 里不带 Image。
*/
import { computed, onBeforeUnmount, ref, shallowRef, watch } from 'vue'
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
  TunStack: string; DnsMode: string
}

const EMPTY: Setting = {
  ProxyRunning: false, KernelRunning: false, KernelReady: false, KernelVersion: '',
  EnableSocks5: true, Socks5Port: 1080, EnableAuth: true, IsAdmin: true, LastError: '',
  TunStack: 'system', DnsMode: 'fake-ip',
}

const busy = ref(false)
const loading = ref(false)
const error = ref('')
const f = ref<Setting>({ ...EMPTY })

const procs = shallowRef<ProcessRow[]>([])
const checked = ref<Set<number>>(new Set())
const filter = ref('')
const names = useList<ProcessRow>(FeedList.SelectProcess)

const filtered = computed(() => {
  const q = filter.value.trim().toLowerCase()
  return q ? procs.value.filter((p) => p.ProcessName.toLowerCase().includes(q) || String(p.ProcessID).includes(q)) : procs.value
})

/* 表头排序（编号 / 进程名称两列）—— 接在过滤之后 */
const sort = useSort<ProcessRow>(filtered, {
  pid: (p) => p.ProcessID,
  name: (p) => p.ProcessName || '',
})

const shown = sort.sorted

/* TUN 栈 / DNS 模式的可选项：技术名词，直接照原样显示（system / gvisor / mixed / fake-ip / redir-host） */
const STACKS = ['system', 'gvisor', 'mixed'] as const
const DNSMODES = ['fake-ip', 'redir-host'] as const

/* ─────────────── 进程图标 ─────────────── */

const icons = ref<Record<string, string>>({})

function iconOf(path: string): string {
  return path ? (icons.value[path] ?? '') : ''
}

async function fetchIcons(rows: ProcessRow[]): Promise<void> {
  const want = new Set<string>()
  for (const p of rows) if (p.ProcessPath && icons.value[p.ProcessPath] === undefined) want.add(p.ProcessPath)
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
    console.error('[mh] 取进程图标失败', e)
  }
}

/* 右表（按名称拦截）的路径来自库里存的那份，也要一起取 */
watch(names, (rows) => { void fetchIcons(rows) }, { immediate: true })

watch(() => props.open, async (on) => {
  if (!on) return
  error.value = ''
  try {
    f.value = { ...EMPTY, ...(await call<Setting>('getMihomoSetting')) }
    await refresh()
  } catch (e) {
    console.error('[mh] 读取设置失败', e)
  }
}, { immediate: true })

async function refresh(): Promise<void> {
  loading.value = true
  try {
    const r = await call<{ rows: ProcessRow[] }>('getProcessRows')
    procs.value = r?.rows ?? []
    //保留本地已勾但还在表里的；进程退出了它的 Pid 就该从勾选集里走
    const alive = new Set(procs.value.map((p) => p.ProcessID))
    const next = new Set<number>()
    for (const pid of checked.value) if (alive.has(pid)) next.add(pid)
    for (const p of procs.value) if (p.IsCheck) next.add(p.ProcessID)
    checked.value = next
    void fetchIcons(procs.value)
  } catch (e) {
    console.error('[mh] 取进程列表失败', e)
  } finally {
    loading.value = false
  }
}

function toggle(p: ProcessRow): void {
  const s = new Set(checked.value)
  if (s.has(p.ProcessID)) s.delete(p.ProcessID)
  else s.add(p.ProcessID)
  checked.value = s
}

/*
  单击勾选、双击加到右表。行上的单击推迟一拍（220ms）再执行，dblclick 到了就取消；
  第 2 次 click 的 detail 是 2，直接忽略。勾选框本身（.chk，@click.stop）仍是即时的。
*/
let rowClickTimer = 0
onBeforeUnmount(() => window.clearTimeout(rowClickTimer))

function onRowClick(p: ProcessRow, e: MouseEvent): void {
  if (e.detail > 1) return
  window.clearTimeout(rowClickTimer)
  rowClickTimer = window.setTimeout(() => { rowClickTimer = 0; toggle(p) }, 220)
}

function onRowDblClick(p: ProcessRow): void {
  window.clearTimeout(rowClickTimer)
  rowClickTimer = 0
  void addName(p)
}

async function addName(p: ProcessRow): Promise<void> {
  try {
    const r = await call<{ ok: boolean }>('addSelectProcessName', { pid: p.ProcessID })
    if (!r?.ok) pushToast('warning', t('ps.noModule'))
  } catch (e) {
    console.error('[mh] 添加名称失败', e)
  }
}

async function removeName(p: ProcessRow): Promise<void> {
  try { await call('removeSelectProcessName', { name: p.ModuleName }) }
  catch (e) { console.error('[mh] 删除名称失败', e) }
}

async function save(): Promise<void> {
  busy.value = true
  error.value = ''
  try {
    const r = await call<{ error: string }>('saveMihomoSetting', {
      tunStack: f.value.TunStack,
      dnsMode: f.value.DnsMode,
    })
    if (r?.error) { error.value = r.error; return }
    emit('update:open', false)
  } catch (e) {
    console.error('[mh] 保存失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}
</script>

<template>
  <SettingsModal :open="props.open" :title="t('set.process')" subtitle="mihomo · TUN" :busy="busy" :error="error" :width="980"
                 @update:open="emit('update:open', $event)" @save="save">
    <div class="setf list-page mh">

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

      <p v-if="!f.ProxyRunning" class="hint warn">{{ t('mh.needProxy') }}</p>
      <p v-if="f.LastError" class="hint bad">{{ f.LastError }}</p>

      <!-- TUN 模式 -->
      <section class="sec">
        <div class="grp">{{ t('mh.stack') }}</div>

        <div class="row">
          <div class="k">{{ t('mh.stack') }}</div>
          <div class="v">
            <button v-for="s in STACKS" :key="s" class="rd" :class="{ on: f.TunStack === s }" @click="f.TunStack = s"><i />{{ s }}</button>
          </div>
        </div>
        <p class="hint">{{ t('mh.stackHint') }}</p>

        <div class="row">
          <div class="k">{{ t('mh.dns') }}</div>
          <div class="v">
            <button v-for="d in DNSMODES" :key="d" class="rd" :class="{ on: f.DnsMode === d }" @click="f.DnsMode = d"><i />{{ d }}</button>
          </div>
        </div>
        <p class="hint">{{ t('mh.dnsHint') }}</p>
      </section>

      <!-- 拦截进程 -->
      <section class="sec">
        <div class="grp">{{ t('mh.procs') }}</div>

        <div class="two">
          <div class="tbl">
            <div class="tbar">
              <span class="cap">{{ t('ps.byPid') }}</span>
              <input v-model="filter" class="inp sm" spellcheck="false" :placeholder="t('ps.filterPh')">
              <span class="grow" />
              <span class="cnt">{{ checked.size }} / {{ procs.length }}</span>
              <button class="sbtn" :disabled="loading" @click="refresh">{{ loading ? t('proxy.working') : t('ps.refresh') }}</button>
            </div>
            <div class="tbody tall">
              <div class="head hp">
                <span class="ck" />
                <span class="ico" />
                <span class="pid so" :class="{ on: sort.active('pid') }" @click="sort.toggle('pid')">{{ t('ps.pid') }}<i class="ar">{{ sort.mark('pid') }}</i></span>
                <span class="name so" :class="{ on: sort.active('name') }" @click="sort.toggle('name')">{{ t('ps.processName') }}<i class="ar">{{ sort.mark('name') }}</i></span>
              </div>
              <div v-if="!shown.length" class="empty">{{ loading ? t('proxy.working') : t('ps.emptyProcs') }}</div>
              <div
                v-for="p in shown"
                v-else
                :key="p.ProcessID"
                class="tr hp"
                :class="{ sel: checked.has(p.ProcessID) }"
                :title="p.ProcessPath"
                @click="onRowClick(p, $event)"
                @dblclick="onRowDblClick(p)"
              >
                <span class="ck"><button class="chk" :class="{ on: checked.has(p.ProcessID) }" @click.stop="toggle(p)"><i /></button></span>
                <span class="ico"><img v-if="iconOf(p.ProcessPath)" :src="iconOf(p.ProcessPath)" alt=""><i v-else class="ph" /></span>
                <span class="pid">{{ p.ProcessID }}</span>
                <span class="name">{{ p.ProcessName }}</span>
              </div>
            </div>
            <div class="tf">{{ t('ps.byPidHint') }}</div>
          </div>

          <div class="tbl">
            <div class="tbar">
              <span class="cap">{{ t('ps.byName') }}</span>
              <span class="grow" />
              <span class="cnt">{{ names.length }}</span>
            </div>
            <div class="tbody tall">
              <div class="head hn">
                <span class="no">{{ t('col.id') }}</span>
                <span class="ico" />
                <span class="name">{{ t('ps.moduleName') }}</span>
                <span class="ops">{{ t('col.ops') }}</span>
              </div>
              <div v-if="!names.length" class="empty">{{ t('ps.emptyNames') }}</div>
              <div v-for="(p, i) in names" v-else :key="p.ModuleName + i" class="tr hn" :title="p.ProcessPath" @dblclick="removeName(p)">
                <span class="no">{{ i + 1 }}</span>
                <span class="ico"><img v-if="iconOf(p.ProcessPath)" :src="iconOf(p.ProcessPath)" alt=""><i v-else class="ph" /></span>
                <span class="name">{{ p.ModuleName }}</span>
                <span class="ops">
                  <button class="op del" :title="t('acct.op.del')" @click.stop="removeName(p)">
                    <svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg>
                  </button>
                </span>
              </div>
            </div>
            <div class="tf">{{ t('ps.byNameHint') }}</div>
          </div>
        </div>
      </section>

      <p class="hint tail">{{ t('mh.loop') }}</p>
    </div>
  </SettingsModal>
</template>

<style scoped>
/*
  竖直呼吸量：窗口矮就收进程表高度。默认 1280×800 在 125% 缩放下只有 640 CSS 高，
  扣掉弹窗头尾，两张表是唯一能收的地方。
*/
.mh { --tall: 300px; }

@media (max-height: 760px) { .mh { --tall: 220px; } }
@media (max-height: 620px) { .mh { --tall: 170px; } }

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

/* 两张进程表 */
.cap { position: relative; top: 1px; font-family: var(--share); font-size: var(--fs-label); letter-spacing: .12em; text-transform: uppercase; color: var(--cyan); white-space: nowrap; }
.cnt { font-family: var(--mono); font-size: var(--fs-small); color: var(--muted); }
.two { display: grid; grid-template-columns: 1fr 1fr; gap: 8px; }
.two > .tbl { margin: 0; }
.tbody.tall { height: var(--tall); max-height: var(--tall); }
.tf { padding: 6px 12px; border-top: 1px solid var(--border); font-size: var(--fs-small); color: var(--dim2); }

.mh .head.hp, .mh .tr.hp { grid-template-columns: 34px 26px 64px minmax(100px, 1fr); }
.mh .head.hn, .mh .tr.hn { grid-template-columns: 40px 26px minmax(100px, 1fr) 104px; }
.mh .tr { height: 30px; cursor: pointer; }
.mh .head > span, .mh .tr > span { text-align: left; }
.mh .head > span.pid, .mh .tr > span.pid, .mh .head > span.no, .mh .tr > span.no { text-align: center; }

.ico { display: flex; align-items: center; justify-content: center; }
.ico img { width: 16px; height: 16px; image-rendering: auto; }
.ico .ph { width: 16px; height: 16px; }
.pid { font-family: var(--mono); font-size: var(--fs-body); color: var(--muted); font-variant-numeric: tabular-nums; }
.name { color: var(--gray); }
.no { color: var(--dim); font-variant-numeric: tabular-nums; }

.tail { padding: 0 20px; margin: 2px 0 6px; }
</style>
