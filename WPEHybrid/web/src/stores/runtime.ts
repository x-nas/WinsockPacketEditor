// 跨视图共享的运行态。
//
// 状态栏在 App.vue，而这些值来自 ProxyView 已有的 getStats 轮询 ——
// 放在这里让两边共用一份，而不是让状态栏自己再开一个 500ms 的轮询
// （启动页并不需要那份数据，白轮询是浪费）。

import { ref } from 'vue'
import { call } from '../bridge'

/**
 * 别的视图想切到代理模式的某一页时写这里（例如封包列表右键「添加到文本 A」后想看文本对比页）。
 * ProxyView 监听它，切完就清空。null = 没人要切。
 */
export const gotoPage = ref<string | null>(null)

/** SOCKS5 的监听地址，形如 192.168.1.10:1080。启动时取一次，配置不变就不会变。 */
export const socks5Addr = ref('')

/**
 * HTTP 代理（SunnyNet）的监听地址，同上。
 *
 * ⚠️ **空串 = 没启用 HTTP 代理**，不是「取不到」—— C# 侧 `ProxyAddresses` 在
 * `Enable_HTTP` 为 false 时就返回空串，界面据此显示「未启用」而不是一个连不上的地址。
 * 它与 socks5Addr 共用同一次 GetLocalIPAddress()（70ms），所以两者总是一起取、一起写。
 */
export const httpAddr = ref('')

/**
 * SOCKS5 服务在不在跑。由 ProxyData 的 getStats 轮询写入，RunBar 的启停按钮也写它。
 * （启停逻辑早已搬进 Operate.ProxyConfig.Proxy，两套 UI 共用同一份。）
 */
export const proxyRunning = ref(false)

/*
  ── 注入模式的运行态 ────────────────────────────────

  底部状态栏要显示「附在哪个目标上、钩子在不在跑」，而那一栏在 App.vue 里、
  不是 InjectView 的子孙 —— 跨视图共享的运行态一律走这里。
*/

/** 当前附加的目标（进程名 #PID）。没附加时是空串。 */
export const injectTarget = ref('')

/** 目标侧的 13 个钩子装上了没有。 */
export const injectHooked = ref(false)

/**
 * 列表设置（列显隐 + 自动清理）。
 *
 * 放共享状态而不是各组件自取：改它的是设置弹窗，用它的是封包列表与工具条，
 * 两者不在同一棵子树上。null = 还没读过，此时按「全显示」渲染。
 */
export const listSetting = ref<{
  showSocket: boolean
  showType: boolean
  showClientAddr: boolean
  showClientLoc: boolean
  showServerAddr: boolean
  showServerLoc: boolean
  showLen: boolean
  autoClear: boolean
  autoClearValue: number
} | null>(null)

/*
  ── 全局快捷键作用在哪一份列表上 ────────────────────────

  快捷面板底部那一条要显示它，而改它的地方在「快捷键设置」弹窗、备份导入 ——
  三处不是父子关系，所以放在这里共用一份。

  ⚠️ **真源是 C# 的 SystemConfig.HotKeyType**（0 = 发送列表、1 = 机器人列表），
  这里只是个镜像：改过它的地方各自调一次 refreshHotkey()，不要在前端另记一份。
*/

/** 0 = 发送列表、1 = 机器人列表。 */
export const hotkeyType = ref(0)

/** 12 个快捷键里设了几个。0 = 一个都没设 —— 那时说「作用在哪」没有意义。 */
export const hotkeyCount = ref(0)

/** 从 C# 重取一次。桥没接上（探针页）时静默保持原值。 */
export async function refreshHotkey(): Promise<void> {
  try {
    const r = await call<{ Type: number; Keys: string[] }>('getHotkeySetting')
    hotkeyType.value = r?.Type === 1 ? 1 : 0
    hotkeyCount.value = (r?.Keys ?? []).filter((k) => !!(k && k.trim())).length
  } catch { /* 断了 */ }
}
