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
import { httpAddr, proxyRunning, socks5Addr } from '../../stores/runtime'
// 设置清单与 SettingKey 在独立模块里 —— <script setup> 不能写 export
import { SETTINGS, type SettingKey } from './settings'
import ContextMenu from '../ContextMenu.vue'
import type { MenuItem } from '../menu'

const emit = defineEmits<{ (e: 'clear'): void; (e: 'openSetting', key: SettingKey): void }>()

/*
  「设置 ▾」弹的是共用的 ContextMenu（锚定在按钮下方），不再自己画一份：
  外观与右键菜单、下拉面板完全一致，以后调样式只改一处。
*/
const menuAt = ref<{ x: number; y: number; anchor: { left: number; right: number; top: number; bottom: number } } | null>(null)
const menuOpen = computed(() => menuAt.value !== null)

const settingItems = computed<MenuItem[]>(() => SETTINGS.map((x) => ({ id: x.key, label: t(x.label) })))

function openMenu(e: MouseEvent): void {
  if (menuAt.value) { menuAt.value = null; return }
  const b = (e.currentTarget as HTMLElement).getBoundingClientRect()
  menuAt.value = { x: b.left, y: b.bottom + 4, anchor: { left: b.left, right: b.right, top: b.top, bottom: b.bottom } }
}

function onPick(id: string): void {
  emit('openSetting', id as SettingKey)
}

/*
  HTTP 那块端口牌：与 SOCKS5 同一个 IP 时只显示「:端口」，完整地址进悬停提示。

  两个地址本来就是 ShellForm.ProxyAddresses 用<b>同一个</b> GetLocalIPAddress() 拼出来的，
  IP 恒相同 —— 并排写两遍，占掉的一百多像素正是 125% 缩放下把右端按钮挤到第二行的那一截
  （2026-09-11 按要求收成一行）。万一哪天两边不同了（改成各自绑定），这里自动退回完整地址。
*/
function hostOf(addr: string): string {
  const i = addr.lastIndexOf(':')
  return i > 0 ? addr.slice(0, i) : addr
}

const httpShort = computed(() => {
  const h = httpAddr.value
  if (!h) return ''
  const s = socks5Addr.value
  return s && hostOf(s) === hostOf(h) ? h.slice(hostOf(h).length) : h
})

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
  <div class="modebar" :class="{ on: proxyRunning }">
    <span class="led" />
    <span class="st">{{ proxyRunning ? t('proxy.running') : t('proxy.stopped') }}</span>
    <!--
      两块端口牌包在 .ports 里当一个整体：这条状态条是 flex-wrap 的，
      不包的话换行点正好可能落在两块中间，HTTP 那块会被甩到第二行去挨着不相干的东西
      （与「勾选框和它的条数框要包一层 .pair」是同一条教训）。
    -->
    <span class="ports">
      <span class="port">
        <b class="pk">SOCKS5</b><span class="pv">{{ socks5Addr || '—' }}</span>
      </span>
      <!-- httpAddr 空串 = 代理设置里没开 HTTP，不是取不到 —— 显示「未启用」并压暗 -->
      <span class="port" :class="{ off: !httpAddr }" :title="httpAddr && httpShort !== httpAddr ? 'HTTP ' + httpAddr : undefined">
        <b class="pk">HTTP</b><span class="pv">{{ httpShort || t('proxy.notEnabled') }}</span>
      </span>
    </span>

    <!-- 「已运行」三个字换成一枚表盘图标，文案挪进悬停提示 —— 同样是为了让这条状态条在 125% 缩放下一行放得下 -->
    <span class="meta" :title="t('proxy.uptime')">
      <svg class="clk" viewBox="0 0 24 24"><circle cx="12" cy="12" r="8" /><path d="M12 8v4l3 2" /></svg>{{ uptime }}
    </span>

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
/*
  状态条的外观（容器 / 状态灯 / 状态字 / 读数窗 / 右端按钮）已经收进 style.css 的 `.modebar`
  —— 注入模式那条状态条用的是同一套构件，抄第二份就会开始走样（2026-09-10 前正是如此）。
  这里只留代理这一屏独有的：「已运行 hh:mm:ss」那一段。
*/
.meta {
  flex: none;
  display: inline-flex;
  align-items: center;
  gap: 5px;
  font-family: var(--share);
  font-size: var(--label-size);
  /*
    ⚠️ 这里原来还有一句 padding-top: 3px，2026-09-10 去掉了 —— 那是<b>补第二遍</b>：
    line-height: 1 本身已经把 Share Tech Mono 的字形偏上治好了，再补 3px 反而把它压到
    条中心下面 +1.24px，成了这条状态条上唯一一个离群的（其余各件都在 −0.06 ~ −0.84）。
    与 2026-09-09 撤掉 34 处按钮那个「上 +1 下 −1」是同一个错。
  */
  line-height: 1;
  letter-spacing: .14em;
  text-transform: uppercase;
  color: var(--muted);
}

.clk {
  width: 12px;
  height: 12px;
  flex: none;
  fill: none;
  stroke: currentColor;
  stroke-width: 1.8;
}
</style>
