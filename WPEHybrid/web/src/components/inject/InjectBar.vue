<script setup lang="ts">
/*
  注入模式的运行状态条 —— 对应 WinForms 的 PacketList 工具条左半边
  （bHookStart / bHookStop / lProcessName / lModuleName / lWinsockInfo / lSpeedInfo）。

  与代理模式的 RunBar 是同一个位置、同一套外观，但内容不能共用：
  那边的核心状态是「SOCKS5 在不在监听」，这边是「附在哪个进程上、钩子装没装」。
  硬凑成一个组件会变成一堆 v-if，两种模式各自的读法都被拖累。

  「设置 ▾」与代理那边同样弹共用的 ContextMenu，清单取 INJECT_SETTINGS（代理 12 项的子集）。
*/
import { computed, ref } from 'vue'
import { t } from '../../i18n'
import { status, wsText } from '../../stores/inject'
import { INJECT_SETTINGS, type SettingKey } from '../proxy/settings'
import ContextMenu from '../ContextMenu.vue'
import type { MenuItem } from '../menu'

const props = defineProps<{ busy: boolean; rate: number; rows: number }>()

const emit = defineEmits<{
  (e: 'toggleHook'): void
  (e: 'clear'): void
  (e: 'detach'): void
  (e: 'openSetting', key: SettingKey): void
}>()

const disconnected = computed(() => status.value.state === 'disconnected')

const menuAt = ref<{ x: number; y: number; anchor: { left: number; right: number; top: number; bottom: number } } | null>(null)
const menuOpen = computed(() => menuAt.value !== null)

const settingItems = computed<MenuItem[]>(() =>
  INJECT_SETTINGS.map((x) => ({ id: x.key, label: t(x.label), disabled: !x.ready })))

function openMenu(e: MouseEvent): void {
  if (menuAt.value) { menuAt.value = null; return }
  const b = (e.currentTarget as HTMLElement).getBoundingClientRect()
  menuAt.value = { x: b.left, y: b.bottom + 4, anchor: { left: b.left, right: b.right, top: b.top, bottom: b.bottom } }
}
</script>

<template>
  <div class="runbar" :class="{ on: status.hooked, off: disconnected }">
    <span class="led" />

    <button
      class="tb"
      :class="status.hooked ? 'stop' : 'go'"
      :disabled="props.busy || disconnected"
      @click="emit('toggleHook')"
    >
      <svg v-if="status.hooked" class="ico" viewBox="0 0 24 24"><rect x="6" y="6" width="12" height="12" /></svg>
      <svg v-else class="ico" viewBox="0 0 24 24"><path d="M7 4l13 8-13 8z" /></svg>
      {{ status.hooked ? t('inject.stopHook') : t('inject.startHook') }}
    </button>

    <div class="meta">
      <span class="k">{{ t('inject.target') }}</span>
      <b>{{ status.name || '—' }}</b>
      <span class="dim">#{{ status.pid }} · {{ status.is64 ? 'x64' : 'x86' }}</span>
    </div>

    <!-- 主窗口标题：同名进程开好几个时，这是唯一能分清「注的是哪一个」的东西 -->
    <div v-if="status.module" class="meta win">
      <span class="k">{{ t('inject.window') }}</span>
      <b :title="status.module">{{ status.module }}</b>
    </div>

    <div class="meta">
      <span class="k">WinSock</span>
      <b>{{ wsText(status) }}</b>
    </div>

    <div class="meta">
      <span class="k">{{ t('inject.rate') }}</span>
      <b>{{ props.rate }}/s</b>
      <span class="dim">{{ props.rows }} {{ t('inject.rows') }}</span>
    </div>

    <!--
      丢弃计数：环满时目标丢的是最旧的包。
      无声丢包比阻塞更糟，所以只要不是 0 就必须显示出来。
    -->
    <div v-if="status.dropped > 0" class="meta warn">
      <span class="k">{{ t('inject.dropped') }}</span>
      <b>{{ status.dropped }}</b>
    </div>

    <span class="grow" />

    <div class="state" :class="status.state">
      {{ disconnected ? t('inject.state.lost') : t('inject.state.ok') }}
    </div>

    <button class="tb" @click="emit('clear')">
      <svg class="ico" viewBox="0 0 24 24"><path d="M4 7h16M9 7V4h6v3M6 7l1 13h10l1-13" /></svg>
      {{ t('inject.clear') }}
    </button>

    <button class="tb" :class="{ on: menuOpen }" @click="openMenu">
      <svg class="ico" viewBox="0 0 24 24"><circle cx="12" cy="12" r="3" /><path d="M12 2v3M12 19v3M2 12h3M19 12h3M5 5l2 2M17 17l2 2M19 5l-2 2M7 17l-2 2" /></svg>
      {{ t('proxy.settings') }} ▾
    </button>

    <button class="tb" :disabled="props.busy" @click="emit('detach')">
      <svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg>
      {{ t('inject.detach') }}
    </button>

    <ContextMenu :at="menuAt" :items="settingItems" @pick="emit('openSetting', $event as SettingKey)" @close="menuAt = null" />
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

/* 钩子装上时左侧透出一层绿光 —— 与代理的 RunBar 同一条视觉约定 */
.runbar.on { background: linear-gradient(90deg, rgb(var(--green-rgb) / 7%), transparent 45%), var(--card); }
.runbar.off { opacity: .72; }

.led { width: 9px; height: 9px; background: var(--muted); flex: none; }
.runbar.on .led { background: var(--green); box-shadow: 0 0 8px var(--green); animation: beat 1.6s steps(1) infinite; }

@keyframes beat { 50% { opacity: .35; } }

/* flex: none —— 让它们各占自然宽度；空间不够时由上面的 flex-wrap 折行，而不是逐个压扁 */
.meta { display: flex; align-items: baseline; gap: 6px; font-size: 12px; min-width: 0; flex: none; }

/* 窗口标题可以很长，让它先被压缩，别把右边的动作按钮挤出去 */
.meta.win { flex: 0 1 auto; overflow: hidden; }
.meta.win b { max-width: 22ch; }

.meta .k {
  font-family: var(--share);
  font-size: var(--label-size);
  line-height: 1;
  letter-spacing: .1em;
  color: var(--muted);
  text-transform: uppercase;
}

.meta b { color: var(--gray); font-family: var(--mono); overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
.meta .dim { color: var(--muted); font-size: 11px; white-space: nowrap; }
.meta.warn b { color: var(--danger); }

.grow { flex: 1; }

.state {
  font-family: var(--share);
  font-size: var(--label-size);
  letter-spacing: .12em;
  padding: 3px 9px 1px;
  border: 1px solid var(--border);
  text-transform: uppercase;
  white-space: nowrap;
}

.state.attached { color: var(--green); border-color: rgb(var(--green-rgb) / 40%); }
.state.disconnected { color: var(--danger); border-color: rgb(var(--danger-rgb) / 40%); }

.tb {
  padding: 11px 15px 9px;   /* 上 +1 下 -1：Share Tech Mono 的字形在 em 框里偏上 1px */
  background: transparent;
  border: 1px solid var(--border);
  color: var(--gray);
  font-family: var(--share);
  font-size: var(--btn-size);
  line-height: 1;
  letter-spacing: .12em;
  text-transform: uppercase;
  cursor: pointer;
  display: inline-flex;
  align-items: center;
  gap: 7px;
  white-space: nowrap;
  transition: .15s;
}

.tb .ico { width: 13px; height: 13px; position: relative; top: -1px; }
.tb:hover:not(:disabled) { border-color: var(--cyan); color: var(--cyan); }
.tb:disabled { opacity: .45; cursor: default; }
.tb:focus-visible { outline-offset: -2px; }

/* 悬停要把边框与字色一起写上：通用的 .tb:hover 是 (0,3,0)，压得过 .tb.go 的 (0,2,0) */
.tb.go { border-color: rgb(var(--green-rgb) / 45%); color: var(--green); }
.tb.go:hover:not(:disabled) { background: rgb(var(--green-rgb) / 10%); border-color: var(--green); color: var(--green); }
.tb.go:focus-visible { outline-color: var(--green); }

.tb.stop { border-color: rgb(var(--danger-rgb) / 45%); color: var(--danger); }
.tb.stop:hover:not(:disabled) { background: rgb(var(--danger-rgb) / 12%); border-color: var(--danger); color: var(--danger); }
.tb.stop:focus-visible { outline-color: var(--danger); }

.tb.on { border-color: var(--cyan); color: var(--cyan); }
</style>
