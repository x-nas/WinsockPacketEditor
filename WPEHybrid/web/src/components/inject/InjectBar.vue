<script setup lang="ts">
/*
  注入模式的运行状态条 —— 对应 WinForms 的 PacketList 工具条左半边
  （bHookStart / bHookStop / lProcessName / lModuleName / lWinsockInfo / lSpeedInfo）。

  【2026-09-10 整条改成与代理模式的 RunBar 同形】

  外观与构件走 style.css 的共用件 `.modebar`（容器 / 状态灯 / 状态字 / 读数窗 / 右端按钮），
  <b>次序也照它</b>：状态灯 → 状态字 → 一排读数窗 → 撑开 → 右端一排动作按钮。

  改之前这一条是自己长出来的另一套：「开始 / 停止」蹲在<b>最左边</b>、没有状态字、
  几段信息是裸文字 —— 两屏并排切过去读出来就是两个程序，而它们本来是同一件事的两种数据源。

  ⚠️ 内容仍然不共用：代理那边的核心状态是「SOCKS5 在不在监听」，这边是
  「附在哪个进程上、钩子装没装」。硬凑成一个组件会变成一堆 v-if，两种模式的读法都被拖累。
  共用的是<b>构件与次序</b>，不是数据。

  【2026-09-10 第二轮，按要求收窄】

  ① **去掉了速率读数窗** —— 统计格里本来就有一格 `RATE 实时速率`，就在这条状态条正下方，
     同一个数字在相隔十几像素的地方说两遍，只是在挤这一行的横向空间。
  ② **去掉了「断开」按钮** —— 退出程序时由 `ShellForm.OnFormClosing` 的
     `DetachInjectOnExit()` 自动断开（那一句本来就在，与「关系统代理」同级）。
     ⚠️ 代价要知道：**换一个目标得重启程序**，与代理模式「选完模式就回不去了」同一条口径。
  ③ **窗口标题那一块也去掉了**（第三轮）—— 注入成功时系统日志里已经记了一条完整的
     「已注入目标 […] 方式 […] 路径」，那才是事后要回去查的地方；
     而它装的是完整路径、长起来没有上限，摆在状态条上要独占一整行。
     ⚠️ 顺带留下的教训别丢：`.ports` <b>不能写 `flex: none`</b> ——
     那样基准宽度是 max-content，整组超宽之后既不折行也不截断，直接从右边<b>裁掉</b>
     （用户截图里 WinSock 那块就被切了一半）。现在是 `flex: 0 1 auto` + 自己内部折行。

  「设置 ▾」与代理那边同样弹共用的 ContextMenu，清单取 INJECT_SETTINGS（代理 12 项的子集）。
*/
import { computed, ref } from 'vue'
import { t } from '../../i18n'
import { status, wsText } from '../../stores/inject'
import { INJECT_SETTINGS, type SettingKey } from '../proxy/settings'
import ContextMenu from '../ContextMenu.vue'
import type { MenuItem } from '../menu'

const props = defineProps<{ busy: boolean }>()

const emit = defineEmits<{
  (e: 'toggleHook'): void
  (e: 'clear'): void
  (e: 'openSetting', key: SettingKey): void
}>()

const disconnected = computed(() => status.value.state === 'disconnected')

/*
  已附加、但还没开始拦截 —— 状态灯与状态字走<b>青</b>（`.modebar.ready`）。

  ⚠️ 这一档不能留在 `.st` 的默认灰上：那是代理那边「未启动」用的颜色，
  而两者正相反 —— 那边是<b>还没开始</b>，这边是<b>已经连上目标了</b>，只差点一下拦截。
  全项目的色语是「绿＝在跑 · 青＝就绪 · 红＝出事 · 灰＝没开始」，这里正是青那一档。
*/
const attached = computed(() => !disconnected.value && !status.value.hooked)

/*
  状态字 —— 三态，与代理那边「运行中 / 未启动」占同一个位置。

  ⚠️ 这一句同时替掉了原来右端那枚「已附加 / 已断开」徽标：
  「断开」与「在不在拦截」不是两件要并排显示的事 —— 断开之后钩子早就没有意义了，
  两处各说一半反而要读两遍。
*/
const stateText = computed(() => {
  if (disconnected.value) return t('inject.state.lost')
  return status.value.hooked ? t('inject.hooking') : t('inject.state.ok')
})

const menuAt = ref<{ x: number; y: number; anchor: { left: number; right: number; top: number; bottom: number } } | null>(null)
const menuOpen = computed(() => menuAt.value !== null)

const settingItems = computed<MenuItem[]>(() =>
  INJECT_SETTINGS.map((x) => ({ id: x.key, label: t(x.label) })))

function openMenu(e: MouseEvent): void {
  if (menuAt.value) { menuAt.value = null; return }
  const b = (e.currentTarget as HTMLElement).getBoundingClientRect()
  menuAt.value = { x: b.left, y: b.bottom + 4, anchor: { left: b.left, right: b.right, top: b.top, bottom: b.bottom } }
}
</script>

<template>
  <div class="modebar" :class="{ on: status.hooked, ready: attached, bad: disconnected, off: disconnected }">
    <span class="led" />
    <span class="st">{{ stateText }}</span>

    <!--
      定长的那几块读数窗。包在 .ports 里当<b>一个</b> flex 项 —— 这条是 flex-wrap 的，
      不包的话换行点可能落在两块中间，后一块会被甩到第二行去挨着不相干的东西。
    -->
    <span class="ports">
      <span class="port">
        <b class="pk">{{ t('inject.target') }}</b>
        <span class="pv">{{ status.name || '—' }}</span>
        <span class="px">#{{ status.pid }} · {{ status.is64 ? 'x64' : 'x86' }}</span>
      </span>

      <span class="port">
        <b class="pk">WinSock</b>
        <span class="pv">{{ wsText(status) }}</span>
      </span>

      <!--
        丢弃计数：环满时目标丢的是最旧的包。
        无声丢包比阻塞更糟，所以只要不是 0 就必须显示出来 —— 用 .bad 那一档（红）。
      -->
      <span v-if="status.dropped > 0" class="port bad">
        <b class="pk">{{ t('inject.dropped') }}</b>
        <span class="pv">{{ status.dropped }}</span>
      </span>
    </span>

    <span class="grow" />

    <!-- 右端一排动作按钮，次序照代理那边：<b>主动作在最左</b>，然后是清空、设置 -->
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

    <button class="tb" @click="emit('clear')">
      <svg class="ico" viewBox="0 0 24 24"><path d="M4 7h16M9 7V4h6v3M6 7l1 13h10l1-13" /></svg>
      {{ t('inject.clear') }}
    </button>

    <button class="tb" :class="{ on: menuOpen }" @click="openMenu">
      <svg class="ico" viewBox="0 0 24 24"><circle cx="12" cy="12" r="3" /><path d="M12 2v3M12 19v3M2 12h3M19 12h3M5 5l2 2M17 17l2 2M19 5l-2 2M7 17l-2 2" /></svg>
      {{ t('proxy.settings') }} ▾
    </button>


    <ContextMenu :at="menuAt" :items="settingItems" @pick="emit('openSetting', $event as SettingKey)" @close="menuAt = null" />
  </div>
</template>

<style scoped>
/*
  状态条的外观（容器 / 状态灯 / 状态字 / 读数窗 / 右端按钮）在 style.css 的 `.modebar` 里，
  与代理数据页那条<b>共用同一份</b>。这里只留注入这一屏独有的一条。
*/

/*
  目标没了：整条压暗。

  ⚠️ 与 `.bad`（状态灯与状态字转红）是两件事，一起加才完整 ——
  红是「出事了」，压暗是「这上面的数字已经不再更新了」。
  数据仍留在列表里，所以只压暗、不隐藏。
*/
.modebar.off { opacity: .72; }
</style>
