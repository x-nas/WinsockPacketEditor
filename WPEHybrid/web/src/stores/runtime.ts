// 跨视图共享的运行态。
//
// 状态栏在 App.vue，而这些值来自 ProxyView 已有的 getStats 轮询 ——
// 放在这里让两边共用一份，而不是让状态栏自己再开一个 500ms 的轮询
// （启动页并不需要那份数据，白轮询是浪费）。

import { ref } from 'vue'

/**
 * 别的视图想切到代理模式的某一页时写这里（例如封包列表右键「添加到文本 A」后想看文本对比页）。
 * ProxyView 监听它，切完就清空。null = 没人要切。
 */
export const gotoPage = ref<string | null>(null)

/** SOCKS5 的监听地址，形如 192.168.1.10:1080。启动时取一次，配置不变就不会变。 */
export const socks5Addr = ref('')

/**
 * SOCKS5 服务在不在跑。
 *
 * 由 ProxyView 的 getStats 轮询写入。
 * <b>目前恒为 false</b> —— 外壳还起不了代理服务：那段启停逻辑（约 240 行）
 * 还在 Controls/ProxyList.cs 里，没搬进 Operate，外壳复用不了。
 * 搬完之后这里会自动变活，界面不用改。
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
