<script setup lang="ts">
/*
  运行状态条 —— 代理模式的核心开关。

  WinForms 里「开始 / 停止」只是工具条上两个普通按钮，和「清空」并排，
  而它其实是这一屏最重要的状态。这里提成独立一条：状态灯 + 监听地址 +
  运行时长 + 实时速率，右侧才是动作按钮。

  启停逻辑在 Operate.ProxyConfig.Proxy（与 WinForms 共用同一份，B10 之后搬过去的）。
*/
import { computed, onBeforeUnmount, onMounted, ref, watch } from 'vue'
import { call } from '../../bridge'
import { t } from '../../i18n'
import { proxyRunning, socks5Addr } from '../../stores/runtime'
// 设置清单与 SettingKey 在独立模块里 —— <script setup> 不能写 export
import { SETTINGS, type SettingKey } from './settings'
import ContextMenu from '../ContextMenu.vue'
import type { MenuItem } from '../menu'

const emit = defineEmits<{ (e: 'clear'): void; (e: 'openSetting', key: SettingKey): void }>()

/*
  「设置 ▾」弹的是共用的 ContextMenu（锚定在按钮下方），不再自己画一份：
  外观与右键菜单、下拉面板完全一致，以后调样式只改一处。
  没做的那几项压暗（disabled），与侧栏「没做的页压暗」同一口径。
*/
const menuAt = ref<{ x: number; y: number; anchor: { left: number; right: number; top: number; bottom: number } } | null>(null)
const menuOpen = computed(() => menuAt.value !== null)

const settingItems = computed<MenuItem[]>(() =>
  SETTINGS.map((x) => ({ id: x.key, label: t(x.label), disabled: !x.ready })))

function openMenu(e: MouseEvent): void {
  if (menuAt.value) { menuAt.value = null; return }
  const b = (e.currentTarget as HTMLElement).getBoundingClientRect()
  menuAt.value = { x: b.left, y: b.bottom + 4, anchor: { left: b.left, right: b.right, top: b.top, bottom: b.bottom } }
}

function onPick(id: string): void {
  emit('openSetting', id as SettingKey)
}

const busy = ref(false)

/*
  运行时长。

  只在前端计时，不问 C# 要 —— 服务的启动时刻在 Operate 里没有记录，
  为一个计时器往 Operate 里加字段不划算。代价是：外壳启动前代理就已经在跑的话
  （目前不可能，服务只能从这里启动），时长会从 0 开始算。
*/
const startedAt = ref(0)
const now = ref(0)
let timer = 0

onMounted(() => {
  timer = window.setInterval(() => { now.value = Date.now() }, 1000)
})

onBeforeUnmount(() => window.clearInterval(timer))

watch(proxyRunning, (on) => {
  startedAt.value = on ? Date.now() : 0
}, { immediate: true })

const uptime = computed(() => {
  if (!proxyRunning.value || !startedAt.value) return '--:--:--'

  const s = Math.max(0, Math.floor((now.value - startedAt.value) / 1000))
  const p = (n: number) => String(n).padStart(2, '0')
  return p(Math.floor(s / 3600)) + ':' + p(Math.floor(s / 60) % 60) + ':' + p(s % 60)
})

async function toggle(): Promise<void> {
  busy.value = true
  try {
    const r = await call<any>(proxyRunning.value ? 'stopProxy' : 'startProxy')
    proxyRunning.value = !!r?.running
    if (r?.socks5Addr) socks5Addr.value = r.socks5Addr
  } catch (e) {
    console.error('[proxy] 启停失败', e)
  } finally {
    busy.value = false
  }
}
</script>

<template>
  <div class="runbar" :class="{ on: proxyRunning }">
    <span class="led" />
    <span class="st">{{ proxyRunning ? t('proxy.running') : t('proxy.stopped') }}</span>
    <span class="addr">SOCKS5 · {{ socks5Addr || '—' }}</span>
    <span class="meta">{{ t('proxy.uptime') }} {{ uptime }}</span>

    <span class="grow" />

    <button class="tb" :class="proxyRunning ? 'stop' : 'go'" :disabled="busy" @click="toggle">
      <svg v-if="proxyRunning" class="ico" viewBox="0 0 24 24"><rect x="6" y="6" width="12" height="12" /></svg>
      <svg v-else class="ico" viewBox="0 0 24 24"><path d="M7 4l13 8-13 8z" /></svg>
      {{ busy ? t('proxy.working') : (proxyRunning ? t('proxy.stop') : t('proxy.start')) }}
    </button>

    <button class="tb" @click="emit('clear')">
      <svg class="ico" viewBox="0 0 24 24"><path d="M4 7h16M9 7V4h6v3M6 7l1 13h10l1-13" /></svg>
      {{ t('proxy.clear') }}
    </button>

    <button class="tb" :class="{ on: menuOpen }" @click="openMenu">
      <svg class="ico" viewBox="0 0 24 24"><circle cx="12" cy="12" r="3" /><path d="M12 2v3M12 19v3M2 12h3M19 12h3M5 5l2 2M17 17l2 2M19 5l-2 2M7 17l-2 2" /></svg>
      {{ t('proxy.settings') }} ▾
    </button>

    <ContextMenu :at="menuAt" :items="settingItems" @pick="onPick" @close="menuAt = null" />
  </div>
</template>

<style scoped>
.runbar {
  flex: none;
  display: flex;
  align-items: center;
  gap: 14px;
  /*
    ⚠️ 换行 + 各项 flex: none。这一条是加了六种语言之后补的：
    越南语的「Bắt đầu bắt」「Cài đặt ▾」比中文长一半，1280 宽的窗口里
    刚好把中间那几段元信息挤到要用省略号 —— 而被吃掉的正是目标进程名。
    宁可让整条状态条折成两行，也不要把它认不出来。
  */
  flex-wrap: wrap;
  row-gap: 8px;
  padding: 9px 14px;
  border: 1px solid var(--border);
  background: var(--card);
}

/* 服务在跑时左侧透出一层绿光，停着时不透 —— 余光里也能看出状态 */
.runbar.on { background: linear-gradient(90deg, rgb(var(--green-rgb) / 7%), transparent 45%), var(--card); }

.led { width: 9px; height: 9px; background: var(--muted); flex: none; }
.runbar.on .led { background: var(--green); box-shadow: 0 0 8px var(--green); animation: beat 1.6s steps(1) infinite; }

@keyframes beat { 50% { opacity: .35; } }

.st {
  font-family: var(--orbit);
  font-weight: 800;
  font-size: 13px;
  letter-spacing: .06em;
  text-transform: uppercase;
  color: var(--muted);
}

.runbar.on .st { color: var(--green); }

.addr { font-family: var(--share); font-size: 12px; letter-spacing: .1em; color: var(--cyan); }

.meta {
  font-family: var(--share);
  font-size: var(--label-size);
  /* 行高 1 + 顶部 3px：默认行高把行距压在字下面，实测字形高 1.9px；4px 会过头 0.75，3px 剩 0.25（实测）*/
  line-height: 1;
  padding-top: 3px;
  letter-spacing: .14em;
  text-transform: uppercase;
  color: var(--muted);
}

.grow { flex: 1; }

.tb {
  padding: 11px 15px 9px;   /* 上 +1 下 -1：字形在 em 框里偏上 1px（上伸 9 / 下伸 3，实测），补回来 */
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
  display: inline-flex;
  align-items: center;
  gap: 7px;
  transition: .15s;
}

.tb .ico { width: 13px; position: relative; top: -1px;   /* 按钮上内边距比下多 2px 是给字形的补偿，图标不需要，退回 1px */ height: 13px; }
.tb:hover:not(:disabled) { border-color: var(--cyan); color: var(--cyan); }
.tb:disabled { opacity: .45; cursor: default; }
.tb:focus-visible { outline-offset: -2px; }

.tb.go { border-color: rgb(var(--green-rgb) / 45%); color: var(--green); }
/* 悬停要把边框与字色一起写上：上面那条通用的 .tb:hover 是 (0,3,0)，压得过 .tb.go 的 (0,2,0)，
   不写的话点完一瞬间（鼠标还停在按钮上）绿 / 红按钮会变成青色 —— 与 .list-page .btn.primary:hover 同一个坑 */
.tb.go:hover:not(:disabled) { background: rgb(var(--green-rgb) / 10%); border-color: var(--green); color: var(--green); }
.tb.go:focus-visible { outline-color: var(--green); }

.tb.stop { border-color: rgb(var(--danger-rgb) / 45%); color: var(--danger); }
.tb.stop:hover:not(:disabled) { background: rgb(var(--danger-rgb) / 12%); border-color: var(--danger); color: var(--danger); }
.tb.stop:focus-visible { outline-color: var(--danger); }

/* 「设置」按钮开着菜单时的高亮；菜单本身是共用的 ContextMenu */
.tb.on { border-color: var(--cyan); color: var(--cyan); }
</style>
