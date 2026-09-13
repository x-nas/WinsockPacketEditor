<script setup lang="ts">
/*
  进程设置 —— 对应 WinForms 的 Controls/ProcessSetting。

  ⚠️ 2026-09-10 整屏重做：从「三块并列的表单」改成「一条链路 + 四个步骤」。

  【为什么重做】这一屏要办的事其实是一条<b>链路</b>：
      目标进程 →（驱动截下来）→ SunnyNet →（强制转代理）→ WPE 的 SOCKS5 → 滤镜改包
  而它有<b>四个必要条件</b>，缺一条就什么都抓不到，且缺哪一条界面上都看不出来：
      ① 驱动装了  ② 至少选了一个进程  ③ 转代理指向本机 SOCKS5  ④ 代理服务真的在跑
  更要命的是 ③④ 里有两项<b>不归这一屏管</b>（HTTP 代理与 SOCKS5 的开关在「代理设置」里）——
  旧版就只能在页脚写一句「需要启用 HTTP 代理后才可以拦截进程的数据」，用户读完还是不知道自己缺哪一步。

  【现在的形态】
    · 顶部一条链路图：五个节点各带状态灯，连线通了才亮 —— 一眼看出断在哪一段；
    · 下面四张步骤卡，每张自己判断「就绪 / 待办 / 注意 / 不通」，并把那一步的控件装在卡里；
    · 第 4 步是<b>只读体检</b>：三项状态 + 一个「打开代理设置」的入口（emit goto），
      这一屏<b>不代管别人的配置</b> —— HTTP / SOCKS5 的开关改了也要重启代理服务才生效，
      就地写会造出「界面说开了、实际没生效」的第二个真源。

  数据：getProcessSetting 除了自己那几项，还带回一组只读的环境状态
  （EnableHttp / EnableSocks5 / Socks5Port / EnableAuth / Running / IsAdmin），见 ProcessSettingRow。

  图标按去重路径一次批量取（getProcessIcons）、按路径记忆化 —— DTO 里不带 Image（B9 的规则）。
  保存时先校验、真连一次转代理服务器，再装驱动、把勾选的 Pid 与名称交给 SunnyNet，全在 C#（SaveProcessSetting）。
  按名称拦截的名单与驱动类型会随「代理设置」落库，WPE 重启后还在；Pid 那份刻意不存。
*/
import { computed, onBeforeUnmount, ref, shallowRef, watch } from 'vue'
import { call } from '../../bridge'
import { FeedList, type ProcessRow } from '../../bridge/types'
import { t, type Key } from '../../i18n'
import { useList } from '../../stores/lists'
import { useSort } from '../../useSort'
import { pushToast } from '../../stores/toast'
import SettingsModal from './SettingsModal.vue'
import type { SettingKey } from './settings'

const props = defineProps<{ open: boolean }>()
const emit = defineEmits<{
  (e: 'update:open', v: boolean): void
  /** 第 4 步的「打开代理设置」——只导航，不改对方的配置 */
  (e: 'goto', key: SettingKey): void
}>()

interface Setting {
  DriverType: number; IsLoadDriver: boolean; MustTCP: boolean; IP: string; Port: number
  AppointPort: boolean; AppointPortContent: string; Auth: boolean; UserName: string; PassWord: string
  CheckedPids: number[]
  /* 只读环境状态 */
  EnableHttp: boolean; HttpPort: number; EnableSocks5: boolean; Socks5Port: number
  EnableAuth: boolean; Running: boolean; IsAdmin: boolean
}

const EMPTY: Setting = {
  DriverType: 1, IsLoadDriver: false, MustTCP: true, IP: '127.0.0.1', Port: 1080,
  AppointPort: false, AppointPortContent: '', Auth: false, UserName: '', PassWord: '', CheckedPids: [],
  EnableHttp: true, HttpPort: 1081, EnableSocks5: true, Socks5Port: 1080,
  EnableAuth: true, Running: false, IsAdmin: true,
}

const busy = ref(false)
const loading = ref(false)
const testing = ref(false)
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

/*
  表头排序（编号 / 进程名称两列，与 WinForms 那张表带 SortMode 的两列一致）。
  接在过滤之后 —— 上面那个搜索框先筛，这里再排。
*/
const sort = useSort<ProcessRow>(filtered, {
  pid: (p) => p.ProcessID,
  name: (p) => p.ProcessName || '',
})

const shown = sort.sorted

/* ─────────────── 链路体检 ─────────────── */

type St = 'ok' | 'todo' | 'warn' | 'bad'

/** 转代理指的是不是本机 —— 与 C# 的 IsLocalProxyAddress 同一条判据（回环 / localhost）。 */
const toLocal = computed(() => {
  const ip = (f.value.IP || '').trim().toLowerCase()
  return ip === 'localhost' || ip === '0.0.0.0' || ip.startsWith('127.')
})

/** 指向本机自己的 SOCKS5：只有这一种情形滤镜才吃得到这些封包。 */
const toOwnSocks = computed(() => toLocal.value && f.value.Port === f.value.Socks5Port)

/** 本机 SOCKS5 开着认证，而这边没勾「需要认证」—— 保存会被 C# 拦下，目标进程也会断网。 */
const authMissing = computed(() => f.value.MustTCP && toOwnSocks.value && f.value.EnableAuth && !f.value.Auth)

/** ① 驱动 */
const s1 = computed<St>(() => (f.value.IsLoadDriver ? 'ok' : f.value.IsAdmin ? 'todo' : 'bad'))

/** ② 目标进程：按编号与按名称任意一份非空即可 */
const picked = computed(() => checked.value.size + names.value.length)
const s2 = computed<St>(() => (picked.value > 0 ? 'ok' : 'todo'))

/** ③ 转发到 WPE 代理 —— 关着不是错，是「只能看不能改」 */
const s3 = computed<St>(() => {
  if (!f.value.MustTCP) return 'warn'
  if (authMissing.value) return 'bad'
  return toOwnSocks.value ? 'ok' : 'warn'
})

/** ④ 代理服务：HTTP 没开就没人接驱动送来的流量；转本机却没开 SOCKS5 同理 */
const s4 = computed<St>(() => {
  if (!f.value.EnableHttp) return 'bad'
  if (f.value.MustTCP && toOwnSocks.value && !f.value.EnableSocks5) return 'bad'
  return f.value.Running ? 'ok' : 'todo'
})

/**
 * 还差几步才算「数据到得了封包列表」。
 * ③ 的 warn 不计 —— 那时链路是通的，只是改不了包；bad 要计，那会让目标进程断网。
 */
const missing = computed(() => {
  let n = 0
  if (s1.value !== 'ok') n++
  if (s2.value !== 'ok') n++
  if (s3.value === 'bad') n++
  if (s4.value !== 'ok') n++
  return n
})

/** 整条链路的结论：通且可改 / 通但只读 / 不通 */
const flowSt = computed<'ok' | 'view' | 'wait'>(() =>
  missing.value > 0 ? 'wait' : s3.value === 'ok' ? 'ok' : 'view')

const flowText = computed(() =>
  flowSt.value === 'wait' ? t('ps.flow.wait').replace('{0}', String(missing.value))
    : flowSt.value === 'ok' ? t('ps.flow.ok') : t('ps.flow.view'))

/*
  链路图的五个节点。

  'on' 亮 · 'off' 暗（还没通，但不是错）· 'bad' 红 · 'skip' 虚（这一段被绕开了）。
  ⚠️ 后两个节点在「强制转代理」关着时是 skip 而不是 off —— 那时数据根本不走 SOCKS5，
  滤镜也就不在链路上。画成暗的会读成「快好了」，画成虚的才读得出「这条支路没接」。
*/
type NodeSt = 'on' | 'off' | 'bad' | 'skip'

const ICON = {
  app: ['M3 4h18v13H3z', 'M9 21h6'],
  drv: ['M7 7h10v10H7z', 'M10 3v4M14 3v4M10 17v4M14 17v4M3 10h4M3 14h4M17 10h4M17 14h4'],
  mitm: ['M3 12h4M17 12h4', 'M12 7l5 5-5 5-5-5z'],
  socks: ['M3 4h18v7H3zM3 13h18v7H3z', 'M6.5 7.5h.01M6.5 16.5h.01'],
  edit: ['M4 5h16l-6 7v7l-4-2v-5z'],
} as const

const nodes = computed(() => {
  const svcOn = f.value.EnableHttp && f.value.Running
  const mitm: NodeSt = !f.value.EnableHttp ? 'bad' : f.value.Running ? 'on' : 'off'

  let socks: NodeSt = 'skip'
  let edit: NodeSt = 'skip'
  if (f.value.MustTCP) {
    socks = authMissing.value || (toOwnSocks.value && !f.value.EnableSocks5) ? 'bad'
      : toOwnSocks.value && svcOn ? 'on' : 'off'
    edit = s3.value === 'bad' ? 'bad' : s3.value === 'ok' && svcOn && s1.value === 'ok' && s2.value === 'ok' ? 'on' : 'off'
  }

  return [
    { k: 'app', icon: ICON.app, label: t('ps.flow.app'), val: picked.value ? String(picked.value) : '—', st: (s2.value === 'ok' ? 'on' : 'off') as NodeSt },
    { k: 'drv', icon: ICON.drv, label: t('ps.flow.drv'), val: f.value.IsLoadDriver ? DRIVERS.find((d) => d.v === f.value.DriverType)?.name ?? '—' : '—', st: (s1.value === 'ok' ? 'on' : s1.value === 'bad' ? 'bad' : 'off') as NodeSt },
    { k: 'mitm', icon: ICON.mitm, label: t('ps.flow.mitm'), val: ':' + f.value.HttpPort, st: mitm },
    { k: 'socks', icon: ICON.socks, label: t('ps.flow.socks'), val: f.value.MustTCP ? ':' + f.value.Port : '—', st: socks },
    { k: 'edit', icon: ICON.edit, label: t('ps.flow.edit'), val: '', st: edit },
  ]
})

/** 连线的通断跟着两端走：两头都亮才算通，任意一头是 bad 就报错色。 */
function linkSt(a: NodeSt, b: NodeSt): NodeSt {
  if (a === 'bad' || b === 'bad') return 'bad'
  if (a === 'skip' || b === 'skip') return 'skip'
  return a === 'on' && b === 'on' ? 'on' : 'off'
}

/* ─────────────── 进程图标 ─────────────── */

/*
  进程表回来之后按去重路径<b>一次</b>批量取（getProcessIcons），按路径记忆化。
  原先在 render 里逐个发桥调用、每回来一个就 { ...icons } 整表重渲染一次 ——
  二百多个进程就是二百多次往返加二百多次重渲染。现在 iconOf 只是一次查表，没有副作用。
*/
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
    console.error('[ps] 取进程图标失败', e)
  }
}

/* 右表（按名称拦截）的路径来自库里存的那份，也要一起取 */
watch(names, (rows) => { void fetchIcons(rows) }, { immediate: true })

watch(() => props.open, async (on) => {
  if (!on) return
  error.value = ''
  try {
    f.value = { ...EMPTY, ...(await call<Setting>('getProcessSetting')) }
    checked.value = new Set(f.value.CheckedPids ?? [])
    await refresh()
  } catch (e) {
    console.error('[ps] 读取进程设置失败', e)
  }
}, { immediate: true })

async function refresh(): Promise<void> {
  loading.value = true
  try {
    const r = await call<{ rows: ProcessRow[] }>('getProcessRows')
    procs.value = r?.rows ?? []
    /*
      C# 按 lstSelectProcessID 勾好了；本地已勾但还没保存的也保留 ——
      但只保留<b>还在表里</b>的：进程退出了它的 Pid 就该从勾选集里走，
      否则保存时会把一串死 Pid 送去 AddProcessPid，计数也会出现 12 / 10。
    */
    const alive = new Set(procs.value.map((p) => p.ProcessID))
    const next = new Set<number>()
    for (const pid of checked.value) if (alive.has(pid)) next.add(pid)
    for (const p of procs.value) if (p.IsCheck) next.add(p.ProcessID)
    checked.value = next
    void fetchIcons(procs.value)
  } catch (e) {
    console.error('[ps] 取进程列表失败', e)
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
  单击勾选、双击加到右表 —— 但双击前面先来两次 click，行上的 toggle 会翻两下（净零，勾选框闪一下），
  而且第一下已经真的改了勾选集。所以行上的单击推迟一拍（220ms）再执行，dblclick 到了就取消；
  第 2 次 click 的 detail 是 2，直接忽略。勾选框本身（.chk，@click.stop）仍是即时的。
*/
let rowClickTimer = 0
//单击后 220ms 内关掉弹窗的话，那一下延迟的勾选不该再落到已卸载的表上
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
    console.error('[ps] 添加名称失败', e)
  }
}

async function removeName(p: ProcessRow): Promise<void> {
  try { await call('removeSelectProcessName', { name: p.ModuleName }) }
  catch (e) { console.error('[ps] 删除名称失败', e) }
}

/** 一键把转代理指回本机自己的 SOCKS5 —— 这一屏九成的用法就是这一种。 */
function useLocal(): void {
  f.value.IP = '127.0.0.1'
  f.value.Port = f.value.Socks5Port
  if (f.value.EnableAuth) f.value.Auth = true
}

async function test(): Promise<void> {
  testing.value = true
  error.value = ''
  try {
    const r = await call<{ error: string }>('testSocksProxy', { auth: f.value.Auth, ip: f.value.IP, port: f.value.Port, userName: f.value.UserName, passWord: f.value.PassWord })
    if (r?.error) pushToast('error', r.error)
    else pushToast('success', t('ps.connected'))
  } catch (e) {
    console.error('[ps] 检测失败', e)
  } finally {
    testing.value = false
  }
}

async function uninstall(): Promise<void> {
  try {
    const r = await call<{ ok: boolean }>('uninstallDriver')
    if (r?.ok) f.value.IsLoadDriver = false
  } catch (e) {
    console.error('[ps] 卸载驱动失败', e)
  }
}

async function save(): Promise<void> {
  busy.value = true
  error.value = ''
  try {
    const r = await call<{ error: string }>('saveProcessSetting', {
      driverType: f.value.DriverType,
      mustTcp: f.value.MustTCP,
      ip: f.value.IP,
      port: Math.trunc(f.value.Port || 0),
      appointPort: f.value.AppointPort,
      appointPortContent: f.value.AppointPortContent,
      auth: f.value.Auth,
      userName: f.value.UserName,
      passWord: f.value.PassWord,
      pids: [...checked.value],
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

const DRIVERS = [
  { v: 1, name: 'NFAPI', tip: 'ps.tipNfapi' },
  { v: 0, name: 'Proxifier', tip: 'ps.tipProxifier' },
  { v: 2, name: 'WinDivert', tip: 'ps.tipWinDivert' },
] as const

/** 步骤徽标的文字：四个状态各一句。标成 Key 而不是 string —— t() 的键有类型约束，写错要在编译期报出来 */
const TAG: Record<St, Key> = { ok: 'ps.ok', todo: 'ps.todo', warn: 'ps.warn', bad: 'ps.bad' }
</script>

<template>
  <SettingsModal :open="props.open" :title="t('set.process')" subtitle="Process Capture" :busy="busy" :error="error" :width="980"
                 @update:open="emit('update:open', $event)" @save="save">
    <div class="setf list-page ps">
      <!--
        数据链路：五个节点 + 四条连线。这一块是整屏的灵魂 ——
        用户第一眼要得到的不是「有哪些选项」，而是「现在断在哪儿」。
      -->
      <section class="flow" :class="flowSt">
        <header class="fh">
          <span class="fd" />
          <span class="fc">{{ t('ps.flow') }}</span>
          <span class="grow" />
          <span class="fs">{{ flowText }}</span>
        </header>
        <div class="fb">
          <template v-for="(n, i) in nodes" :key="n.k">
            <span v-if="i" class="fa" :class="linkSt(nodes[i - 1].st, n.st)" />
            <div class="fn" :class="n.st">
              <svg class="fi" viewBox="0 0 24 24"><path v-for="d in n.icon" :key="d" :d="d" /></svg>
              <span class="fl">{{ n.label }}</span>
              <span class="fv">{{ n.val }}</span>
            </div>
          </template>
        </div>
      </section>

      <!-- ① 驱动 -->
      <section class="stp" :class="s1">
        <header class="sh">
          <span class="sn">01</span>
          <div class="sx">
            <span class="stt">{{ t('ps.s1.t') }}</span>
            <span class="sdd">{{ t('ps.s1.d') }}</span>
          </div>
          <span class="sg"><i class="dt" />{{ t(TAG[s1]) }}</span>
        </header>
        <div class="sc">
          <div class="row">
            <div class="k">{{ t('ps.driverType') }}</div>
            <div class="v">
              <button v-for="d in DRIVERS" :key="d.v" class="rd" :class="{ on: f.DriverType === d.v }" :disabled="f.IsLoadDriver" :title="t(d.tip)" @click="f.DriverType = d.v">
                <i />{{ d.name }}
              </button>
              <span class="grow" />
              <span class="tg" :class="f.IsLoadDriver ? 'ok' : 'dim'">{{ f.IsLoadDriver ? t('ps.driverLoaded') : t('ps.driverNotLoaded') }}</span>
              <button class="sbtn danger" :disabled="!f.IsLoadDriver" @click="uninstall">{{ t('ps.uninstall') }}</button>
            </div>
          </div>
          <p class="hint">{{ t(DRIVERS.find((d) => d.v === f.DriverType)?.tip ?? 'ps.tipNfapi') }}</p>
          <p v-if="s1 === 'bad'" class="hint bad">{{ t('ps.s1.noadmin') }}</p>
          <p v-else-if="f.IsLoadDriver" class="hint">{{ t('ps.s1.done') }}</p>
        </div>
      </section>

      <!-- ② 进程 -->
      <section class="stp" :class="s2">
        <header class="sh">
          <span class="sn">02</span>
          <div class="sx">
            <span class="stt">{{ t('ps.s2.t') }}</span>
            <span class="sdd">{{ t('ps.s2.d') }}</span>
          </div>
          <span class="sg"><i class="dt" />{{ s2 === 'ok' ? t('ps.s2.some').replace('{0}', String(picked)) : t(TAG[s2]) }}</span>
        </header>
        <div class="sc">
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
          <p v-if="s2 === 'todo'" class="hint warn">{{ t('ps.s2.none') }}</p>
        </div>
      </section>

      <!-- ③ 转发到 WPE 代理 -->
      <section class="stp" :class="s3">
        <header class="sh">
          <span class="sn">03</span>
          <div class="sx">
            <span class="stt">{{ t('ps.s3.t') }}</span>
            <span class="sdd">{{ t('ps.s3.d') }}</span>
          </div>
          <span class="sg"><i class="dt" />{{ t(TAG[s3]) }}</span>
        </header>
        <div class="sc">
          <!--
            ⚠️ 这一行<b>刻意没有标签列</b>：它的标签会是「强制转代理」，而卡片标题
            已经写着「转发到 WPE 代理」—— 同一句话说两遍，还占掉俄语下最紧张的那一列。
            勾选框自带「开启」二字，后面直接跟这一步当前的结论。
          -->
          <div class="lead">
            <button class="chk" :class="{ on: f.MustTCP }" @click="f.MustTCP = !f.MustTCP"><i />{{ t('ps.mustTcp') }}</button>
            <span v-if="!f.MustTCP" class="lb warn">{{ t('ps.s3.off') }}</span>
            <span v-else-if="toOwnSocks && !authMissing" class="lb ok">{{ t('ps.s3.local') }}</span>
            <span v-else-if="!toOwnSocks" class="lb warn">{{ t('ps.s3.ext') }}</span>
          </div>
          <div class="row" :class="{ off: !f.MustTCP }">
            <div class="k">{{ t('ps.proxyAddr') }}</div>
            <div class="v">
              <input v-model="f.IP" class="inp sm" spellcheck="false" :disabled="!f.MustTCP" placeholder="127.0.0.1">
              <span class="colon">:</span>
              <input v-model.number="f.Port" class="inp num" type="number" min="1" max="65535" :disabled="!f.MustTCP">
              <button class="sbtn" :disabled="!f.MustTCP || toOwnSocks" @click="useLocal">{{ t('ps.s3.fill') }}</button>
              <button class="sbtn" :disabled="!f.MustTCP || testing" @click="test">{{ testing ? t('proxy.working') : t('ps.detect') }}</button>
            </div>
          </div>
          <div class="row" :class="{ off: !f.MustTCP }">
            <div class="k">{{ t('ps.auth') }}</div>
            <div class="v">
              <button class="chk" :class="{ on: f.Auth }" :disabled="!f.MustTCP" @click="f.Auth = !f.Auth"><i />{{ t('set.speedModeOn') }}</button>
              <input v-model="f.UserName" class="inp sm" spellcheck="false" :disabled="!f.MustTCP || !f.Auth" :placeholder="t('ps.userPh')">
              <input v-model="f.PassWord" class="inp sm" type="password" :disabled="!f.MustTCP || !f.Auth" :placeholder="t('ps.passPh')">
            </div>
          </div>
          <div class="row" :class="{ off: !f.MustTCP }">
            <div class="k">{{ t('ps.appointPort') }}</div>
            <div class="v">
              <button class="chk" :class="{ on: f.AppointPort }" :disabled="!f.MustTCP" @click="f.AppointPort = !f.AppointPort"><i />{{ t('set.speedModeOn') }}</button>
              <input v-model="f.AppointPortContent" class="inp" spellcheck="false" :disabled="!f.MustTCP || !f.AppointPort" placeholder="80,443">
            </div>
          </div>
          <p v-if="authMissing" class="hint bad">{{ t('ps.s3.auth') }}</p>
        </div>
      </section>

      <!-- ④ 代理服务（只读体检） -->
      <section class="stp" :class="s4">
        <header class="sh">
          <span class="sn">04</span>
          <div class="sx">
            <span class="stt">{{ t('ps.s4.t') }}</span>
            <span class="sdd">{{ t('ps.s4.d') }}</span>
          </div>
          <span class="sg"><i class="dt" />{{ t(TAG[s4]) }}</span>
        </header>
        <div class="sc">
          <div class="chk3">
            <div class="ci" :class="f.EnableHttp ? 'on' : 'bad'">
              <i class="dt" />
              <span class="cl">{{ t('ps.s4.http') }}</span>
              <span class="cv">:{{ f.HttpPort }}</span>
              <span class="cs">{{ f.EnableHttp ? t('ps.s4.on') : t('ps.s4.off') }}</span>
            </div>
            <div class="ci" :class="f.EnableSocks5 ? 'on' : (f.MustTCP && toOwnSocks ? 'bad' : 'off')">
              <i class="dt" />
              <span class="cl">{{ t('ps.s4.socks') }}</span>
              <span class="cv">:{{ f.Socks5Port }}</span>
              <span class="cs">{{ f.EnableSocks5 ? t('ps.s4.on') : t('ps.s4.off') }}</span>
            </div>
            <div class="ci" :class="f.Running ? 'on' : 'off'">
              <i class="dt" />
              <span class="cl">{{ t('ps.s4.svc') }}</span>
              <span class="cv" />
              <span class="cs">{{ f.Running ? t('ps.s4.run') : t('ps.s4.stop') }}</span>
            </div>
          </div>
          <!-- 提示与入口同一行：.setf .row 的 v 是 flex-wrap 的，长句会把按钮挤到第二行 -->
          <div class="s4f">
            <span class="lb">{{ t('ps.s4.hint') }}</span>
            <button class="sbtn" @click="emit('goto', 'proxy')">{{ t('ps.s4.goto') }}</button>
          </div>
        </div>
      </section>

      <p class="hint warn tail">{{ t('ps.saveReminder') }}</p>
    </div>
  </SettingsModal>
</template>

<style scoped>
/*
  这一屏的竖直呼吸量提成令牌，窗口一矮就整屏往里收 —— 与启动页同一套做法。
  默认窗口 ClientSize 1280×800 是<b>设备像素</b>：125% 缩放下页面只有 640 CSS 高，
  扣掉弹窗自己的头尾还剩四百出头，而这四张卡按设计值要六百多。
*/
.ps {
  --sp: 10px;      /* 卡片之间 */
  --tall: 208px;   /* 进程表高度 */
  --fb-py: 12px;   /* 链路图节点区的上下内边距 */
  --fn-py: 8px;    /* 节点自己的上下内边距 */
  --fi: 20px;      /* 节点图标 */
}

/* 1280×800 在 125% 缩放下的那一档（CSS 640 高）—— 链路图是钉住的，它占多少就直接从内容里扣 */
@media (max-height: 760px) {
  .ps { --sp: 8px; --tall: 168px; --fb-py: 8px; --fn-py: 5px; --fi: 18px; }
}

@media (max-height: 620px) {
  .ps { --sp: 6px; --tall: 132px; --fb-py: 6px; --fn-py: 4px; --fi: 17px; }
}

/* ─────────────── 数据链路 ─────────────── */

/*
  ⚠️ <b>钉在顶上</b>（sticky）。四张步骤卡加起来一屏放不下，必然要滚 ——
  而链路图回答的是「现在断在哪儿」，滚走了这一屏就退回成一堆选项。
  钉住之后：改一个开关，眼睛不用离开就看见哪一节点跟着亮 / 灭。

  top: -4px 是 SettingsModal 的 .bd 那 4px 上内边距 —— 不顶掉的话，
  滚动的内容会从链路图上方那道缝里穿过去。
  左右不留边距、去掉左右边框：它是弹窗头的延伸（一条仪表条），不是又一张卡。
*/
.flow {
  position: sticky;
  top: -4px;
  z-index: 2;
  margin: 0 0 var(--sp);
  border: 1px solid var(--border);
  border-left: 0;
  border-right: 0;
  border-top: 0;
  background: var(--sink);
}

.fh {
  display: flex;
  align-items: center;
  gap: 9px;
  padding: 7px 12px 6px;
  border-bottom: 1px solid var(--border);
  background: var(--card);
}

.fh .grow { flex: 1; }
.fd { flex: none; width: 20px; height: 1px; background: var(--cyan); box-shadow: 0 0 6px var(--cyan); }

.fc {
  flex: none;
  font-family: var(--share);
  font-size: var(--fs-label);
  letter-spacing: .14em;
  text-transform: uppercase;
  color: var(--cyan);
}

/* 结论那一句：跟着整条链路的状态换色，它是这一块唯一会变色的文字 */
.fs { font-size: var(--fs-small); color: var(--dim2); text-align: right; }
.flow.ok .fs { color: var(--green); }
.flow.view .fs { color: var(--cyan); }
.flow.wait .fs { color: var(--amber); }

/*
  ⚠️ align-items: stretch —— 五个节点必须等高。
  按内容高的话，末尾那个（「滤镜 · 改包」没有端口可显示）会比旁边矮一截，
  一排设备里有一台缩了水，比它是灰的还显眼。连线要单独 align-self: center 拉回中线。
*/
.fb {
  display: flex;
  align-items: stretch;
  padding: var(--fb-py) 14px calc(var(--fb-py) - 1px);
}

/* 节点 */
.fn {
  flex: 1 1 0;
  min-width: 0;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 5px;
  padding: var(--fn-py) 4px calc(var(--fn-py) - 1px);
  border: 1px solid var(--border);
  background: var(--card);
  transition: border-color .15s, color .15s;
}

.fi { width: var(--fi); height: var(--fi); fill: none; stroke: currentColor; stroke-width: 1.5; stroke-linecap: square; }

.fl {
  font-size: var(--fs-small);
  color: inherit;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  max-width: 100%;
}

.fv {
  font-family: var(--mono);
  font-size: var(--fs-caption);
  color: var(--dim);
  font-variant-numeric: tabular-nums;
  min-height: 1em;
}

/*
  四种节点状态。
  ⚠️ skip 用<b>虚线边框</b>而不是压暗：那一段是被绕开的（转代理关着时数据不走 SOCKS5），
  画成暗的会读成「快好了」，虚线才读得出「这条支路没接上」。
*/
.fn.on {
  color: var(--green);
  border-color: rgb(var(--green-rgb) / 50%);
  background: rgb(var(--green-rgb) / 7%);
  box-shadow: 0 0 12px rgb(var(--green-rgb) / 12%);
}

.fn.off { color: var(--muted); }
.fn.bad { color: var(--danger); border-color: rgb(var(--danger-rgb) / 45%); background: rgb(var(--danger-rgb) / 7%); }
.fn.skip { color: var(--dim); border-style: dashed; background: transparent; }
.fn.skip .fv { color: var(--dim); }

/* 连线：一条 14px 的横线 + 箭头。通了才亮，并让高光流过去 */
.fa {
  flex: none;
  align-self: center;      /* .fb 是 stretch 的，连线要自己回到中线 */
  position: relative;
  width: 22px;
  height: 1px;
  margin: 0 2px;
  background: var(--border);
}

/* 箭头贴在线的右端<b>内侧</b>：right: -1px 会让 .fa 的 scrollWidth 比 clientWidth 大，
   多语言那道「横向溢出」的扫描会把它报成假阳性 */
.fa::after {
  content: '';
  position: absolute;
  right: 0;
  top: -3px;
  width: 5px;
  height: 5px;
  border-top: 1px solid var(--border);
  border-right: 1px solid var(--border);
  transform: rotate(45deg);
}

.fa.on { background: rgb(var(--green-rgb) / 65%); }
.fa.on::after { border-color: rgb(var(--green-rgb) / 65%); }
.fa.bad { background: rgb(var(--danger-rgb) / 55%); }
.fa.bad::after { border-color: rgb(var(--danger-rgb) / 55%); }

.fa.skip {
  background: repeating-linear-gradient(90deg, var(--border) 0 3px, transparent 3px 6px);
}

.fa.skip::after { opacity: .45; }

/* 通了的那几段有一束高光顺着流过去 —— 只在链路真的接通时跑 */
.flow.ok .fa.on::before,
.flow.view .fa.on::before {
  content: '';
  position: absolute;
  inset: 0;
  background: linear-gradient(90deg, transparent, var(--green), transparent);
  animation: run 1.8s linear infinite;
}

@keyframes run { from { transform: translateX(-100%); } to { transform: translateX(100%); } }

@media (prefers-reduced-motion: reduce) {
  .flow .fa.on::before { animation: none; opacity: .5; }
}

/* ─────────────── 步骤卡 ─────────────── */

.stp {
  position: relative;
  margin: 0 20px var(--sp);
  border: 1px solid var(--border);
  background: var(--card);
}

/* 左沿那条色轨：这张卡是什么状态，扫一眼左边就知道 */
.stp::before {
  content: '';
  position: absolute;
  left: 0;
  top: 0;
  bottom: 0;
  width: 2px;
  background: var(--border);
}

.stp.ok::before { background: var(--green); box-shadow: 0 0 8px rgb(var(--green-rgb) / 45%); }
.stp.warn::before { background: var(--amber); }
.stp.bad::before { background: var(--danger); box-shadow: 0 0 8px rgb(var(--danger-rgb) / 45%); }

.sh {
  display: flex;
  align-items: center;
  gap: 11px;
  padding: 9px 12px 8px 14px;
  border-bottom: 1px solid var(--border);
  background: var(--panel);
}

/*
  步号：Orbitron 大字，压得很暗 —— 它是编号不是内容。

  ⚠️ <b>刻意不跟着状态变色。</b>试过 rgb(var(--green-rgb) / 55%)，深色下 4.57、
  <b>浅色下只有 2.26</b>（半透明的深绿压在浅色 panel 上）。而状态本来就由左沿色轨、
  右边徽标、卡片边框表达了三遍 —— 编号再表达第四遍，换来的是一处要分皮肤调的透明度。
*/
.sn {
  flex: none;
  font-family: var(--orbit);
  font-weight: 900;
  font-size: var(--fs-num);
  line-height: 1;
  letter-spacing: -.02em;
  color: var(--dim3);
  font-variant-numeric: tabular-nums;
}

.sx { flex: 1; min-width: 0; display: flex; flex-direction: column; gap: 2px; }
.stt { font-size: var(--fs-lead); color: var(--bright); }
.sdd { font-size: var(--fs-small); color: var(--dim2); line-height: 1.45; }

/* 状态徽标 */
.sg {
  flex: none;
  display: inline-flex;
  align-items: center;
  gap: 6px;
  padding: 4.4px 8px 3.6px;   /* 下压 0.4px：line-height:1 之后字形仍偏上，量出来的 */
  border: 1px solid var(--border);
  font-family: var(--share);
  font-size: var(--fs-label);
  line-height: 1;
  letter-spacing: .08em;
  white-space: nowrap;
  color: var(--muted);
}

.dt { flex: none; width: 6px; height: 6px; border-radius: 50%; background: currentColor; }

.stp.ok .sg { border-color: rgb(var(--green-rgb) / 45%); color: var(--green); }
.stp.ok .dt { box-shadow: 0 0 6px var(--green); }
.stp.warn .sg { border-color: rgb(var(--amber-rgb) / 45%); color: var(--amber); }
.stp.bad .sg { border-color: rgb(var(--danger-rgb) / 45%); color: var(--danger); }
.stp.bad .dt { box-shadow: 0 0 6px var(--danger); }

.sc { padding: 8px 14px 10px; }

/*
  卡里复用 .setf 那套表单行，但左右内边距归零 —— 那 20px 是给弹窗整体留的，
  卡片自己已经有了。标签列也收窄：这几行的标签都很短。
*/
.sc .row { padding: 4px 0; --setf-k: 84px; }
.lead { display: flex; align-items: center; gap: 10px; padding: 4px 0; min-height: 32px; flex-wrap: wrap; }
.sc .hint { padding: 0; margin: 5px 0 0; }
.sc .hint.bad { color: var(--danger); }
.sc .v .grow { flex: 1; }

.lb { font-size: var(--fs-small); color: var(--dim2); }
.lb.ok { color: var(--green); }
.lb.warn { color: var(--amber); }
.colon { color: var(--dim); font-family: var(--mono); }

/* ─────────────── 第 4 步的三项体检 ─────────────── */

.chk3 { display: grid; grid-template-columns: repeat(3, 1fr); gap: 8px; }

.ci {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 8px 10px 7px;
  border: 1px solid var(--border);
  background: rgb(var(--inset-rgb) / 25%);
  color: var(--muted);
  font-size: var(--fs-small);
  min-width: 0;
}

.ci .cl { flex: 1; min-width: 0; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; color: var(--soft); }
.ci .cv { flex: none; font-family: var(--mono); font-size: var(--fs-small); color: var(--dim); font-variant-numeric: tabular-nums; }

.ci .cs {
  flex: none;
  /* top -1px：换成内嵌更纱等宽 SC 后（2026-09-13）放大实测，.4px 时墨迹比所在行中线低约 1.4px（雅黑时 .4px 正好居中） */
  position: relative;
  top: -1px;
  font-family: var(--share);
  font-size: var(--fs-caption);
  letter-spacing: .08em;
  text-transform: uppercase;
  color: inherit;
}

.ci.on { color: var(--green); border-color: rgb(var(--green-rgb) / 40%); }
.ci.on .dt { box-shadow: 0 0 6px var(--green); }
.ci.bad { color: var(--danger); border-color: rgb(var(--danger-rgb) / 40%); }
.ci.off { color: var(--dim); }

.s4f { display: flex; align-items: center; gap: 12px; margin-top: 9px; }
.s4f .lb { flex: 1; min-width: 0; line-height: 1.45; }
.s4f .sbtn { flex: none; }

/* ─────────────── 两张进程表 ─────────────── */

.cap { position: relative; top: 1px; font-family: var(--share); font-size: var(--fs-label); letter-spacing: .12em; text-transform: uppercase; color: var(--cyan); white-space: nowrap; }   /* 实测偏高 1.7px（2026-09-13 字体度量覆写后按 100% 缩放实测重调） */
.cnt { font-family: var(--mono); font-size: var(--fs-small); color: var(--muted); }

.two { display: grid; grid-template-columns: 1fr 1fr; gap: 8px; }
.two > .tbl { margin: 0; }
.tbody.tall { height: var(--tall); max-height: var(--tall); }
.tf { padding: 6px 12px; border-top: 1px solid var(--border); font-size: var(--fs-small); color: var(--dim2); }

.ps .head.hp, .ps .tr.hp { grid-template-columns: 34px 26px 64px minmax(100px, 1fr); }
.ps .head.hn, .ps .tr.hn { grid-template-columns: 40px 26px minmax(100px, 1fr) 104px; }   /* ops 44→104：Действия 含字距要 97 */
.ps .tr { height: 30px; cursor: pointer; }
.ps .head > span, .ps .tr > span { text-align: left; }
.ps .head > span.pid, .ps .tr > span.pid, .ps .head > span.no, .ps .tr > span.no { text-align: center; }

.ico { display: flex; align-items: center; justify-content: center; }
.ico img { width: 16px; height: 16px; image-rendering: auto; }
/* 取不到图标时这一格留空，不画框 —— 这张表本来就有一列勾选框，一个空方框会被读成「没勾上」（2026-09-11 去掉） */
.ico .ph { width: 16px; height: 16px; }
.pid { font-family: var(--mono); font-size: var(--fs-body); color: var(--muted); font-variant-numeric: tabular-nums; }
.name { color: var(--gray); }
.no { color: var(--dim); font-variant-numeric: tabular-nums; }

.tail { padding: 0 20px; margin: 2px 0 6px; }
</style>
