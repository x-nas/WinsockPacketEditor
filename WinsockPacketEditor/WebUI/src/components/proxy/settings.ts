// 12 个设置弹窗的清单 —— 与 WinForms 的 ProxyList.ddMenu 逐条对应（顺序也一样）。
//
// 它们是弹窗不是页面，所以挂在运行状态条的「设置 ▾」上，没有进侧栏。
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
}

export const SETTINGS: SettingDef[] = [
  { key: 'proxy', label: 'set.proxy' },
  { key: 'process', label: 'set.process' },
  { key: 'leach', label: 'set.leach' },
  { key: 'hook', label: 'set.hook' },
  { key: 'list', label: 'set.list' },
  { key: 'map', label: 'set.map' },
  { key: 'extproxy', label: 'set.extproxy' },
  { key: 'hotkey', label: 'set.hotkey' },
  { key: 'backup', label: 'set.backup' },
  { key: 'remote', label: 'set.remote' },
  { key: 'firewall', label: 'set.firewall' },
  { key: 'system', label: 'set.system' },
]

/*
  注入模式的 7 个设置 —— 与 WinForms 的 PacketList.ddMenu 逐条对应（顺序也一样）。

  它是代理那 12 项的<b>真子集</b>：少的五项（代理设置 / 进程设置 / 映射设置 /
  外部代理设置 / 防火墙设置）全是 SOCKS5 服务器那条路上的东西，注入模式里没有对应物。
  所以这里直接从 SETTINGS 里挑，不另写一份定义 —— 抄一份的下场见 CLAUDE.md 的 .list-page。
*/
const INJECT_KEYS: SettingKey[] = ['leach', 'hook', 'list', 'hotkey', 'backup', 'remote', 'system']

export const INJECT_SETTINGS: SettingDef[] = INJECT_KEYS.map((k) => {
  const d = SETTINGS.find((x) => x.key === k)
  if (!d) throw new Error('未知设置 ' + k)
  return d
})
