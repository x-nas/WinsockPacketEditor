<script setup lang="ts">
/*
  进程设置 —— 对应 WinForms 的 Controls/ProcessSetting。

  三块：驱动类型（装过就锁住，只剩「卸载驱动」）· 两张进程表（按编号拦截 / 按名称拦截）· 强制转代理。
  进程表：左边是当前系统的进程（勾选 = 按 Pid 拦截；双击加到右边）；右边是按名称拦截的（FeedList.SelectProcess，双击删）。
  图标按路径单独取、按路径记忆化 —— DTO 里不带 Image（B9 的规则）。
  保存时装驱动、把勾选的 Pid 与名称交给 SunnyNet，全在 C#（SaveProcessSetting）。
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
  DriverType: number; IsLoadDriver: boolean; MustTCP: boolean; IP: string; Port: number
  AppointPort: boolean; AppointPortContent: string; Auth: boolean; UserName: string; PassWord: string; CheckedPids: number[]
}

const busy = ref(false)
const loading = ref(false)
const testing = ref(false)
const error = ref('')
const f = ref<Setting>({ DriverType: 1, IsLoadDriver: false, MustTCP: true, IP: '127.0.0.1', Port: 1080, AppointPort: false, AppointPortContent: '', Auth: false, UserName: '', PassWord: '', CheckedPids: [] })

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

  进程枚举回来的顺序是系统给的（大体按 PID，但不保证），
  找一个具体的进程时按名称排最快，所以两列都给点。
*/
const sort = useSort<ProcessRow>(filtered, {
  pid: (p) => p.ProcessID,
  name: (p) => p.ProcessName || '',
})

const shown = sort.sorted

/* 图标：按路径记忆化，一次会话里同一个 exe 只取一次 */
const icons = ref<Record<string, string>>({})
const pending = new Set<string>()

function iconOf(path: string): string {
  if (!path) return ''
  const got = icons.value[path]
  if (got !== undefined) return got
  if (!pending.has(path)) {
    pending.add(path)
    call<{ png: string }>('getProcessIcon', { path })
      .then((r) => { icons.value = { ...icons.value, [path]: r?.png ? 'data:image/png;base64,' + r.png : '' } })
      .catch(() => { icons.value = { ...icons.value, [path]: '' } })
  }
  return ''
}

watch(() => props.open, async (on) => {
  if (!on) return
  error.value = ''
  try {
    f.value = await call<Setting>('getProcessSetting')
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
    //C# 按 lstSelectProcessID 勾好了；本地已勾的（还没保存）也保留
    for (const p of procs.value) if (p.IsCheck) checked.value.add(p.ProcessID)
    checked.value = new Set(checked.value)
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
</script>

<template>
  <SettingsModal :open="props.open" :title="t('set.process')" subtitle="Controls/ProcessSetting" :busy="busy" :error="error" :width="960"
                 @update:open="emit('update:open', $event)" @save="save">
    <div class="setf list-page ps">
      <!-- 驱动 -->
      <div class="grp">{{ t('ps.driver') }}</div>
      <div class="row">
        <div class="k">{{ t('ps.driverType') }}</div>
        <div class="v">
          <button v-for="d in DRIVERS" :key="d.v" class="rd" :class="{ on: f.DriverType === d.v }" :disabled="f.IsLoadDriver" :title="t(d.tip)" @click="f.DriverType = d.v">
            <i />{{ d.name }}
          </button>
          <span class="tg" :class="f.IsLoadDriver ? 'ok' : 'dim'">{{ f.IsLoadDriver ? t('ps.driverLoaded') : t('ps.driverNotLoaded') }}</span>
          <button class="sbtn danger" :disabled="!f.IsLoadDriver" @click="uninstall">{{ t('ps.uninstall') }}</button>
        </div>
      </div>
      <p class="hint">{{ t(DRIVERS.find((d) => d.v === f.DriverType)?.tip ?? 'ps.tipNfapi') }}</p>

      <!-- 两张进程表 -->
      <div class="grp">{{ t('ps.processes') }}</div>
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
              @click="toggle(p)"
              @dblclick="addName(p)"
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

      <!-- 强制转代理 -->
      <div class="grp">{{ t('ps.mustTcp') }}</div>
      <div class="row">
        <div class="k">{{ t('ps.mustTcp') }}</div>
        <div class="v">
          <button class="chk" :class="{ on: f.MustTCP }" @click="f.MustTCP = !f.MustTCP"><i />{{ t('set.speedModeOn') }}</button>
          <span class="lb">{{ t('ps.mustTcpHint') }}</span>
        </div>
      </div>
      <div class="row" :class="{ off: !f.MustTCP }">
        <div class="k">{{ t('ps.proxyAddr') }}</div>
        <div class="v">
          <input v-model="f.IP" class="inp sm" spellcheck="false" :disabled="!f.MustTCP" placeholder="127.0.0.1">
          <span class="colon">:</span>
          <input v-model.number="f.Port" class="inp num" type="number" min="1" max="65535" :disabled="!f.MustTCP">
          <button class="sbtn" :disabled="!f.MustTCP || testing" @click="test">{{ testing ? t('proxy.working') : t('ps.detect') }}</button>
        </div>
      </div>
      <div class="row" :class="{ off: !f.MustTCP }">
        <div class="k">{{ t('ps.appointPort') }}</div>
        <div class="v">
          <button class="chk" :class="{ on: f.AppointPort }" :disabled="!f.MustTCP" @click="f.AppointPort = !f.AppointPort"><i />{{ t('set.speedModeOn') }}</button>
          <input v-model="f.AppointPortContent" class="inp" spellcheck="false" :disabled="!f.MustTCP || !f.AppointPort" placeholder="80,443">
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
      <p class="hint warn">{{ t('ps.saveReminder') }}</p>
    </div>
  </SettingsModal>
</template>

<style scoped>
.lb { font-size: 11.5px; color: var(--dim2); }
.colon { color: var(--dim); font-family: var(--mono); }
.cap { font-family: var(--share); font-size: 10.5px; letter-spacing: .12em; text-transform: uppercase; color: var(--cyan); white-space: nowrap; }
.cnt { font-family: var(--mono); font-size: 11px; color: var(--muted); }

.two { display: grid; grid-template-columns: 1fr 1fr; gap: 0; }
.two > .tbl:first-child { margin-right: 4px; }
.two > .tbl:last-child { margin-left: 4px; }
.tbody.tall { height: 250px; max-height: 250px; }
.tf { padding: 6px 12px; border-top: 1px solid var(--border); font-size: 11px; color: var(--dim2); }

.ps .head.hp, .ps .tr.hp { grid-template-columns: 34px 26px 64px minmax(100px, 1fr); }
.ps .head.hn, .ps .tr.hn { grid-template-columns: 40px 26px minmax(100px, 1fr) 44px; }
.ps .tr { height: 30px; cursor: pointer; }
.ps .head > span, .ps .tr > span { text-align: left; }
.ps .head > span.pid, .ps .tr > span.pid, .ps .head > span.no, .ps .tr > span.no { text-align: center; }

.ico { display: flex; align-items: center; justify-content: center; }
.ico img { width: 16px; height: 16px; image-rendering: auto; }
.ico .ph { width: 12px; height: 12px; border: 1px solid var(--border2); }
.pid { font-family: var(--mono); font-size: 12px; color: var(--muted); font-variant-numeric: tabular-nums; }
.name { color: var(--gray); }
.no { color: var(--dim); font-variant-numeric: tabular-nums; }
</style>
