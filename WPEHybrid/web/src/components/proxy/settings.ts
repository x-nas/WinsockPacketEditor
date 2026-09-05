// 12 个设置弹窗的清单 —— 与 WinForms 的 ProxyList.ddMenu 逐条对应（顺序也一样）。
//
// 它们是弹窗不是页面，所以挂在运行状态条的「设置 ▾」上，没有进侧栏。
// ready 标出做没做；没做的在菜单里压暗、点了不响应，与侧栏同一套口径。
//
// 单独成文件是因为 <script setup> 里不能写 export（Vue 编译器会直接报错），
// 而这个类型 RunBar 与 ProxyData 都要用。

import type { Key } from '../../i18n'

export type SettingKey =
  | 'proxy' | 'process' | 'leach' | 'hook' | 'list' | 'map'
  | 'extproxy' | 'hotkey' | 'backup' | 'remote' | 'firewall' | 'system'

export interface SettingDef {
  key: SettingKey
  label: Key
  ready: boolean
}

export const SETTINGS: SettingDef[] = [
  { key: 'proxy', label: 'set.proxy', ready: true },
  { key: 'process', label: 'set.process', ready: true },
  { key: 'leach', label: 'set.leach', ready: true },
  { key: 'hook', label: 'set.hook', ready: true },
  { key: 'list', label: 'set.list', ready: true },
  { key: 'map', label: 'set.map', ready: true },
  { key: 'extproxy', label: 'set.extproxy', ready: true },
  { key: 'hotkey', label: 'set.hotkey', ready: true },
  { key: 'backup', label: 'set.backup', ready: true },
  { key: 'remote', label: 'set.remote', ready: true },
  { key: 'firewall', label: 'set.firewall', ready: true },
  { key: 'system', label: 'set.system', ready: true },
]
