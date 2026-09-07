<script setup lang="ts">
/*
  备份设置 —— 对应 WinForms 的 Controls/BackUpSetting。

  导出：勾选要带的十样东西，C# 弹保存框（可加密）。导入：C# 弹打开框，整份配置与各份列表换掉，
  外壳那边随即应用偏好、整表重推、落库（见 ShellForm 的 importBackup）；备份里可能带着语言，页面字典要跟着切。
  这个弹窗没有「保存」——两个动作各自就是终点。
*/
import { ref } from 'vue'
import { call } from '../../bridge'
import { lang, normalize, t } from '../../i18n'
import { socks5Addr } from '../../stores/runtime'
import { initTheme } from '../../stores/theme'
import SettingsModal from './SettingsModal.vue'

const props = defineProps<{ open: boolean }>()
const emit = defineEmits<{ (e: 'update:open', v: boolean): void }>()

const busy = ref(false)
const f = ref({ systemConfig: true, proxySet: true, proxyAccount: true, whiteList: true, blackList: true, proxyMapping: true, injectSet: true, filterList: true, sendList: true, robotList: true })

const GROUPS = [
  { key: 'bk.grp.system', items: [['systemConfig', 'bk.systemConfig']] },
  { key: 'bk.grp.proxy', items: [['proxySet', 'bk.proxySet'], ['proxyAccount', 'bk.proxyAccount'], ['whiteList', 'bk.whiteList'], ['blackList', 'bk.blackList'], ['proxyMapping', 'bk.proxyMapping']] },
  { key: 'bk.grp.inject', items: [['injectSet', 'bk.injectSet']] },
  { key: 'bk.grp.lists', items: [['filterList', 'bk.filterList'], ['sendList', 'bk.sendList'], ['robotList', 'bk.robotList']] },
] as const

type FKey = keyof typeof f.value

function allOn(): boolean { return Object.values(f.value).every(Boolean) }
function setAll(v: boolean): void { for (const k of Object.keys(f.value) as FKey[]) f.value[k] = v }

async function exportBackup(): Promise<void> {
  if (!Object.values(f.value).some(Boolean)) return
  busy.value = true
  try { await call('exportBackup', { ...f.value }) }
  catch (e) { console.error('[bk] 导出失败', e) }
  finally { busy.value = false }
}

async function importBackup(): Promise<void> {
  busy.value = true
  try {
    const r = await call<{ language: string; isDark: boolean; themeMode: string; scanLine: boolean }>('importBackup')
    //直接写 lang，不走 setLang —— 后者会反过来再写一次 C#（多开设置那一屏同一个理由）
    if (r?.language) lang.value = normalize(r.language)
    //主题同理：用 initTheme（只应用、不回写），备份里带的那份已经在 C# 侧落库了
    if (r?.themeMode) initTheme(r.themeMode, r.isDark, r.scanLine)
    //监听地址可能跟着代理配置一起换了
    try { const s = await call<{ socks5Addr?: string }>('getSystemCheck'); if (s?.socks5Addr) socks5Addr.value = s.socks5Addr } catch { /* 取不到就留旧值 */ }
  } catch (e) {
    console.error('[bk] 导入失败', e)
  } finally {
    busy.value = false
  }
}
</script>

<template>
  <SettingsModal :open="props.open" :title="t('set.backup')" subtitle="Controls/BackUpSetting" :busy="busy" readonly
                 @update:open="emit('update:open', $event)">
    <div class="setf bk">
      <p class="hint">{{ t('bk.hint') }}</p>

      <div class="groups">
        <div v-for="g in GROUPS" :key="g.key" class="g">
          <div class="gt">{{ t(g.key) }}</div>
          <button v-for="[k, lb] in g.items" :key="k" class="chk" :class="{ on: f[k] }" @click="f[k] = !f[k]"><i />{{ t(lb) }}</button>
        </div>
      </div>

      <div class="acts">
        <button class="sbtn" @click="setAll(!allOn())">{{ allOn() ? t('pm.deselect') : t('pm.selectAll') }}</button>
        <span class="grow" />
        <button class="sbtn warn" :disabled="busy" @click="importBackup">{{ t('bk.import') }}</button>
        <button class="sbtn primary" :disabled="busy || !Object.values(f).some(Boolean)" @click="exportBackup">{{ t('bk.export') }}</button>
      </div>
      <p class="hint">{{ t('bk.importHint') }}</p>
    </div>
  </SettingsModal>
</template>

<style scoped>
.groups { display: grid; grid-template-columns: 1fr 1fr; gap: 10px; padding: 4px 20px; }
.g { display: flex; flex-direction: column; gap: 8px; padding: 12px 14px; border: 1px solid var(--border); background: rgb(var(--inset-rgb) / 20%); }
.gt { font-family: var(--share); font-size: 10.5px; letter-spacing: .14em; text-transform: uppercase; color: var(--cyan); margin-bottom: 2px; }
.acts { display: flex; align-items: center; gap: 8px; padding: 10px 20px 4px; }
.acts .grow { flex: 1; }
</style>
