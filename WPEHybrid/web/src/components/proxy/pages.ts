// 代理模式的 14 个页面：侧栏与内容区共用这一份定义。
//
// 顺序与分组照 WinForms 的 ProxyModeForm（那边是平铺 14 项的 Menu + 同步的 Tabs），
// 分组是这里加的 —— 平铺 14 项扫起来太累，见 ProxySide.vue 的说明。
//
// ready 标出这一页的 Vue 版做没做。没做的在侧栏里压暗、点了不响应，
// 与启动页那张注入卡同一套口径。

import type { Key } from '../../i18n'

export type PageKey =
  | 'data' | 'client' | 'account'
  | 'filter' | 'send' | 'robot' | 'warehouse'
  | 'stat' | 'diff' | 'xor' | 'transcode' | 'extract'
  | 'wpc' | 'log'

export interface PageDef {
  key: PageKey
  /** i18n 键。用 Key 类型约束，漏译在 vue-tsc 就报错 */
  label: Key
  /** SVG path 串，内联进 <svg class="ico" viewBox="0 0 24 24">。与官网 cyber.js 的 NAV 同一种写法 */
  icon: string
  ready: boolean
}

export interface PageGroup {
  cap: string
  items: PageDef[]
}

export const GROUPS: PageGroup[] = [
  {
    cap: 'Data',
    items: [
      { key: 'data', label: 'proxy.nav.data', ready: true, icon: '<path d="M4 6h16M4 12h16M4 18h10"/>' },
      { key: 'client', label: 'proxy.nav.client', ready: true, icon: '<rect x="3" y="4" width="18" height="12" rx="1"/><path d="M8 20h8"/>' },
      { key: 'account', label: 'proxy.nav.account', ready: true, icon: '<circle cx="12" cy="8" r="3.4"/><path d="M5 20a7 7 0 0 1 14 0"/>' },
    ],
  },
  {
    cap: 'Rules',
    items: [
      { key: 'filter', label: 'proxy.nav.filter', ready: true, icon: '<path d="M3 5h18l-7 8v6l-4 2v-8z"/>' },
      { key: 'send', label: 'proxy.nav.send', ready: true, icon: '<path d="M4 12l16-8-6 16-2-6z"/>' },
      { key: 'robot', label: 'proxy.nav.robot', ready: true, icon: '<rect x="4" y="8" width="16" height="11" rx="2"/><path d="M12 8V4M8 13h.01M16 13h.01"/>' },
      // 仓库：尖顶 + 两面墙 + 门里叠着的货架。早先是一个六边形箱子，缩到 13px 就成了个圆点
      { key: 'warehouse', label: 'proxy.nav.warehouse', ready: true, icon: '<path d="M3 20V9l9-5 9 5v11"/><path d="M2 20h20"/><path d="M8 20v-7h8v7"/><path d="M8 16.5h8"/>' },
    ],
  },
  {
    cap: 'Tools',
    items: [
      { key: 'stat', label: 'proxy.nav.stat', ready: true, icon: '<path d="M4 20V10M10 20V4M16 20v-8M22 20v-5"/>' },
      { key: 'diff', label: 'proxy.nav.diff', ready: true, icon: '<rect x="3" y="4" width="7" height="16"/><rect x="14" y="4" width="7" height="16"/>' },
      { key: 'xor', label: 'proxy.nav.xor', ready: true, icon: '<circle cx="12" cy="12" r="8"/><path d="M8 8l8 8M16 8l-8 8"/>' },
      { key: 'transcode', label: 'proxy.nav.transcode', ready: true, icon: '<path d="M9 6L3 12l6 6M15 6l6 6-6 6"/>' },
      { key: 'extract', label: 'proxy.nav.extract', ready: true, icon: '<path d="M12 3v12M8 11l4 4 4-4M4 19h16"/>' },
    ],
  },
  {
    cap: 'System',
    items: [
      // 火箭 = 加速器，与官网侧栏「代理客户端」同一枚图标
      { key: 'wpc', label: 'proxy.nav.wpc', ready: true, icon: '<path d="M4.5 16.5c-1.5 1.26-2 5-2 5s3.74-.5 5-2c.71-.84.7-2.13-.09-2.91a2.18 2.18 0 0 0-2.91-.09z"/><path d="M12 15l-3-3a22 22 0 0 1 2-3.95A12.88 12.88 0 0 1 22 2c0 2.72-.78 7.5-6 11a22.35 22.35 0 0 1-4 2z"/>' },
      { key: 'log', label: 'proxy.nav.log', ready: true, icon: '<path d="M5 4h11l3 3v13H5z"/><path d="M8 11h8M8 15h5"/>' },
    ],
  },
]

/** 扁平查找用。 */
export const PAGES: PageDef[] = GROUPS.flatMap((g) => g.items)
