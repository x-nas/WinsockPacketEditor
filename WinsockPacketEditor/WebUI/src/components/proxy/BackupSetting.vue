<script setup lang="ts">
/*
  备份设置 —— 对应 WinForms 的 Controls/BackUpSetting。

  导出：勾选要带的十六样东西，C# 弹保存框（可加密）。导入：C# 弹打开框，整份配置与各份列表换掉，
  外壳那边随即应用偏好、整表重推、落库（见 ShellForm 的 importBackup）；备份里可能带着语言，页面字典要跟着切。
  这个弹窗没有「保存」——两个动作各自就是终点。
*/
import { ref } from 'vue'
import { call } from '../../bridge'
import { lang, normalize, t } from '../../i18n'
import { kernelRunning, refreshHotkey, socks5Addr, tunReady } from '../../stores/runtime'
import { initTheme } from '../../stores/theme'
import { ensureDecoders } from '../decoder/actions'
import { ensurePacketExtractors } from '../../stores/extractor'
import SettingsModal from './SettingsModal.vue'

const props = defineProps<{ open: boolean }>()
const emit = defineEmits<{ (e: 'update:open', v: boolean): void }>()

const busy = ref(false)
/*
  ⚠️ <b>仓库默认不勾。</b>仓储封包是原始字节，自动入库开着抓一阵就是几万条 ——
  实测 50000 条 × 512 字节已经是十几 MB 的 XML，真实封包 4KB 时还要再乘几倍。
  其余十五项都是「配置与规则」量级，默认勾上无妨。
*/
const f = ref({
  systemConfig: true, proxySet: true, proxyAccount: true, whiteList: true, blackList: true, proxyMapping: true,
  injectSet: true,
  filterList: true, sendList: true, robotList: true, autoStores: true,
  decoderList: true, packetExtractorList: true,
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
  { key: 'bk.grp.lists', items: [['filterList', 'bk.filterList'], ['sendList', 'bk.sendList'], ['robotList', 'bk.robotList'], ['autoStores', 'bk.autoStores'], ['decoderList', 'proxy.nav.decoders'], ['packetExtractorList', 'proxy.nav.extractors'], ['wareHouse', 'bk.wareHouse', 'bk.wareHouseHint']] },
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
    const r = await call<{ language: string; isDark: boolean; themeMode: string; scanLine: boolean; fontScale?: number; mainTextColor?: string | null }>('importBackup')
    //直接写 lang，不走 setLang —— 后者会反过来再写一次 C#（多开设置那一屏同一个理由）
    if (r?.language) lang.value = normalize(r.language)
    //主题同理：用 initTheme（只应用、不回写），备份里带的那份已经在 C# 侧落库了
    if (r?.themeMode) initTheme(r.themeMode, r.isDark, r.scanLine, r.fontScale, r.mainTextColor)
    //备份里带着快捷键与它作用的列表，快捷面板底部那一条要跟上
    void refreshHotkey()
    //解码器不在推送流里，备份导入后要自己重拉一次（列表页与工作台读同一份 decRows）
    void ensureDecoders(true)
    void ensurePacketExtractors()
    //监听地址可能跟着代理配置一起换了
    try {
      const s = await call<{ socks5Addr?: string; tunReady?: boolean; kernelRunning?: boolean }>('getSystemCheck')
      if (s?.socks5Addr) socks5Addr.value = s.socks5Addr
      //内核状态要无条件写：备份里可能换了代理配置，旧值不能留
      if (s) { tunReady.value = !!s.tunReady; kernelRunning.value = !!s.kernelRunning }
    } catch { /* 取不到就留旧值 */ }
  } catch (e) {
    console.error('[bk] 导入失败', e)
  } finally {
    busy.value = false
  }
}
</script>

<template>
  <SettingsModal :open="props.open" :title="t('set.backup')" subtitle="Backup · Restore" :busy="busy" readonly
                 @update:open="emit('update:open', $event)">
    <div class="setf bk">
      <p class="hint">{{ t('bk.hint') }}</p>

      <div class="groups">
        <!--
          ⚠️ 用<b>共用的 .sec / .grp</b>，不再自己画一套卡片（原来是 .g / .gt）——
          这一屏原先的抬头还是老的「青色 Share Tech Mono 小标题」，
          与另外 10 屏改成分区卡之后就对不上了，一眼看得出是漏改的那一个。
          勾选框收进 .gb：.sec 只负责卡的外观，卡身怎么排由各屏自己定。
        -->
        <section v-for="g in GROUPS" :key="g.key" class="sec">
          <div class="grp">{{ t(g.key) }}</div>
          <div class="gb">
            <button
              v-for="[k, lb, tip] in g.items"
              :key="k"
              class="chk"
              :class="{ on: f[k] }"
              :title="tip ? t(tip) : undefined"
              @click="f[k] = !f[k]"
            ><i />{{ t(lb) }}</button>
          </div>
        </section>
      </div>

      <p class="hint">{{ t('bk.importHint') }}</p>

      <!--
        全选 / 全不选：<b>放在四个区域下面、靠左</b>（2026-10-02 用户要求）——
        它管的是上面那堆勾选框，跟着它们更顺。
      -->
      <div class="selall">
        <button class="sbtn" @click="setAll(!allOn())">{{ allOn() ? t('pm.deselect') : t('pm.selectAll') }}</button>
      </div>

      <!--
        导入 / 导出用<b>与本屏勾选框同一族的小按钮</b>（.setf .sbtn，绿 = 导出为主、
        琥珀 = 导入带注意色），和映射 / 防火墙名单那几条工具条一个外观。
        ⚠️ 不要再塞进 SettingsModal 的页脚插槽 —— 页脚在组件模板之外，
        scoped 的 .sbtn 样式传不过去，会掉成浏览器原生按钮（2026-10-02 撞过）。
      -->
      <div class="bk-acts">
        <button class="sbtn primary" :disabled="busy || !Object.values(f).some(Boolean)" @click="exportBackup">{{ t('bk.export') }}</button>
        <button class="sbtn warn" :disabled="busy" @click="importBackup">{{ t('bk.import') }}</button>
      </div>
    </div>
  </SettingsModal>
</template>

<style scoped>
/*
  四个区域<b>从上到下竖排</b>（2026-10-02 用户要求）。

  曾经用过 2 列多列流式（columns）来省高度 —— 那时「系统运行」与「WPC 配置」
  各只有 2 项，纵向排在 1024×640（1280×800 @125%）下会让弹窗出滚动条。
  改成竖排之后高度确实更高，但这一屏本来就可以整屏滚（.bd 那条），
  顺序读下来的信息量比挤成两列更清楚。
*/
.groups { display: flex; flex-direction: column; padding: 4px 20px 0; }

/*
  ⚠️ 卡片本身走共用的 .sec（边框 / 底色 / 左沿色轨 / 带编号的抬头都在 style.css），
  这里只覆盖两处：左右外边距归 0（.groups 已经有 20px 内边距），
  以及 padding-bottom 收一点 —— 卡身是勾选框不是表单行，不需要 .sec 默认那 8px。
*/
.groups .sec { margin: 0 0 10px; padding-bottom: 10px; }

/* 卡身：勾选框竖排 */
.gb { display: flex; flex-direction: column; gap: 8px; padding: 8px 14px 0; }

/*
  全选 / 全不选：放在四个区域<b>下面、靠左</b>。
*/
.selall { display: flex; align-items: center; padding: 0 20px 0; }

/*
  导入 / 导出：与本屏勾选框同一族的小按钮（.setf .sbtn），绿色是主动作（导出）、
  琥珀留给带注意色的导入，和映射 / 防火墙名单那几条工具条同一套外观。
*/
.bk-acts { display: flex; align-items: center; gap: 8px; padding: 10px 20px 4px; }

/*
  矮视口再收一档 —— 与启动页、数据页那两处同一个思路。

  弹窗高度是 SettingsModal 的 `calc(100vh - 120px)`，而窗口的 CSS 高 = 设备像素 ÷ 缩放比：
  1280×800 在 100% 下是 800、125% 下 640、<b>150% 下只有 533</b>。
  竖排之后这一屏在矮窗口里会滚 —— 滚动条在这儿当地板是认的。
*/
@media (max-height: 620px) {
  .bk .hint { margin: 2px 0; }
  .groups { padding: 0 20px; }
  /*
    ⚠️ 分区卡的抬头在这一档也要收 —— 它比原来那条纯文本小标题高出约 12px/张，
    四张就是 48px。
  */
  .groups .sec { margin-bottom: 6px; padding-bottom: 5px; }
  .groups .sec > .grp { padding: 3px 12px 2px; margin-bottom: 0; }
  .groups .sec > .grp::before { padding: 1px 3px 0; min-width: 19px; }
  .gb { gap: 5px; padding: 4px 12px 0; }
  .selall { padding: 2px 20px 0; }
  .bk-acts { padding: 6px 20px 2px; }
}
</style>
