<script setup lang="ts">
/*
  备份设置 —— 对应 WinForms 的 Controls/BackUpSetting。

  导出：勾选要带的十四样东西，C# 弹保存框（可加密）。导入：C# 弹打开框，整份配置与各份列表换掉，
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
/*
  ⚠️ <b>仓库默认不勾。</b>仓储封包是原始字节，自动入库开着抓一阵就是几万条 ——
  实测 50000 条 × 512 字节已经是十几 MB 的 XML，真实封包 4KB 时还要再乘几倍。
  其余十三项都是「配置与规则」量级，默认勾上无妨。
*/
const f = ref({
  systemConfig: true, proxySet: true, proxyAccount: true, whiteList: true, blackList: true, proxyMapping: true,
  injectSet: true,
  filterList: true, sendList: true, robotList: true, autoStores: true,
  wareHouse: false,
  wpcServer: true, wpcNotice: true,
})

const GROUPS = [
  { key: 'bk.grp.system', items: [['systemConfig', 'bk.systemConfig'], ['injectSet', 'bk.injectSet']] },
  { key: 'bk.grp.proxy', items: [['proxySet', 'bk.proxySet'], ['proxyAccount', 'bk.proxyAccount'], ['whiteList', 'bk.whiteList'], ['blackList', 'bk.blackList'], ['proxyMapping', 'bk.proxyMapping']] },
  /*
    ⚠️ 第三个元素是<b>悬停说明</b>（可选）。仓库那一项要交代「可能很大」，
    但把这句写进标签会在俄语下把格子撑破 —— 实测「Хранилище (с пакетами…」被截掉了尾巴。
    标签只留名字，说明交给提示（自绘的那套，见 tooltip.ts）。
  */
  { key: 'bk.grp.lists', items: [['filterList', 'bk.filterList'], ['sendList', 'bk.sendList'], ['robotList', 'bk.robotList'], ['autoStores', 'bk.autoStores'], ['wareHouse', 'bk.wareHouse', 'bk.wareHouseHint']] },
  { key: 'bk.grp.wpc', items: [['wpcServer', 'bk.wpcServer'], ['wpcNotice', 'bk.wpcNotice']] },
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
          <button
            v-for="[k, lb, tip] in g.items"
            :key="k"
            class="chk"
            :class="{ on: f[k] }"
            :title="tip ? t(tip) : undefined"
            @click="f[k] = !f[k]"
          ><i />{{ t(lb) }}</button>
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
/*
  ⚠️ <b>两列走多列流式（columns），不是 2×2 网格。</b>

  网格的行高由该行最高的那组撑：四组里「系统运行」与「WPC 配置」各只有 2 项，
  却被拉成和 5 项的那两组一样高（实测各 181px，四组一共 380px）——
  <b>一半是空的</b>。1024×640（也就是 1280×800 @125%）下内容 483px、可用 403px，
  溢出 80px，弹窗就出滚动条了。

  多列会按高度自己平衡：短的两组各自贴着长的那组下面填进去，实测降到约 280px。
  代价是<b>组的上下沿不再左右对齐</b> —— 这一屏是一堆勾选框，对齐没有信息量，
  比空半格划算。

  break-inside: avoid 是必须的：不加的话一组会被拦腰断到下一列去。
*/
.groups {
  columns: 2;
  column-gap: 10px;
  padding: 4px 20px 0;
  /* 每列最后一组的 margin-bottom 抵掉，否则底下白多 10px（这一屏正好差这么多） */
  margin-bottom: -10px;
}

.g {
  display: flex;
  flex-direction: column;
  gap: 8px;
  padding: 12px 14px;
  margin-bottom: 10px;
  border: 1px solid var(--border);
  background: rgb(var(--inset-rgb) / 20%);
  break-inside: avoid;
}
.gt { font-family: var(--share); font-size: 10.5px; letter-spacing: .14em; text-transform: uppercase; color: var(--cyan); margin-bottom: 2px; }
.acts { display: flex; align-items: center; gap: 8px; padding: 11px 20px 4px; }   /* 上边距是量着定的：日语最长的那几条差 1px 就会出滚动条 */
.acts .grow { flex: 1; }

/*
  矮视口再收一档 —— 与启动页、数据页那两处同一个思路。

  弹窗高度是 SettingsModal 的 `calc(100vh - 120px)`，而窗口的 CSS 高 = 设备像素 ÷ 缩放比：
  1280×800 在 100% 下是 800、125% 下 640、<b>150% 下只有 533</b>。
  前两档收完之后是 0 溢出；150% 那一档 14 个勾选项确实塞不下，
  这一档只把它压到「滚一点点」，滚动条在这儿当地板是认的
  —— 把窗口拉大或最大化就又不用滚了（1080 ÷ 1.5 = 720，够）。
*/
@media (max-height: 620px) {
  .bk .hint { margin: 2px 0; }
  .groups { padding: 0 20px; }
  .g { gap: 5px; padding: 8px 12px; margin-bottom: 7px; }
  .groups { margin-bottom: -7px; }
  .acts { padding: 10px 20px 2px; }
}
</style>
