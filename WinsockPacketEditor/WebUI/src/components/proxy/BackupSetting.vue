<script setup lang="ts">
/*
  备份设置 —— 对应 WinForms 的 Controls/BackUpSetting。

  导出：勾选要带的十四样东西，C# 弹保存框（可加密）。导入：C# 弹打开框，整份配置与各份列表换掉，
  外壳那边随即应用偏好、整表重推、落库（见 ShellForm 的 importBackup）；备份里可能带着语言，页面字典要跟着切。
  这个弹窗没有「保存」——两个动作各自就是终点。
*/
import { computed, ref } from 'vue'
import { call } from '../../bridge'
import { lang, normalize, t } from '../../i18n'
import { httpAddr, refreshHotkey, socks5Addr } from '../../stores/runtime'
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

/*
  ⚠️ <b>组内排两列，只给放得下的语言开。</b>

  格子宽是算出来又量过的：弹窗 620 − .groups 两侧内边距 40 = 580，外层 columns:2 扣掉 10 的列缝
  → 每列 285；组的边框 2 + 内边距 24 → 255；组内两列再扣 10 的缝 → <b>每格 122.5</b>。

  逐语言量过 14 条标签的最宽那条（13px 勾选框 + 7px 间隙 + 文字，量的是真实渲染不是 canvas 估值）：

  | 放得下 | 放不下 |
  |---|---|
  | 简 95 · 繁 95 · 韩 114.5 | 越 154.9 · 英 161.8 · 日 170 · 俄 217.6 |

  放不下的那四种<b>不是截断，是折行</b> —— 而折行之后「3 行 × 两行高」比原来「5 行 × 一行高」
  还要高，等于白折腾。所以它们保持一列，这一屏在那几种语言下本来也没有空白可省。

  ⚠️ 判据是<b>量出来的宽度</b>，不是现成的 `defOf(lang).wide`（那面标的是 en/vi/ru）——
  日语不 wide 却放不下，韩语的三条「…설정」是 114.5、离 122.5 只剩 8px。
  改了这几条文案或改了弹窗宽度，回来重新量一遍再定这张名单。
*/
const TWO_COL = new Set(['zh', 'tw', 'ko'])
const twoCol = computed(() => TWO_COL.has(lang.value))

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
    //备份里带着快捷键与它作用的列表，快捷面板底部那一条要跟上
    void refreshHotkey()
    //监听地址可能跟着代理配置一起换了
    try {
      const s = await call<{ socks5Addr?: string; httpAddr?: string }>('getSystemCheck')
      if (s?.socks5Addr) socks5Addr.value = s.socks5Addr
      //httpAddr 要无条件写：备份里可能把 HTTP 代理关掉了，那时它就该变回空串
      if (s) httpAddr.value = s.httpAddr || ''
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

      <div class="groups" :class="{ two: twoCol }">
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

/*
  ⚠️ 卡片本身走共用的 .sec（边框 / 底色 / 左沿色轨 / 带编号的抬头都在 style.css），
  这里只覆盖<b>多列流里特有</b>的那三条：
    · 左右外边距归 0 —— .groups 已经有 20px 内边距了，再让 20 会把卡挤成窄条
    · break-inside: avoid —— 不加的话一组会被拦腰断到下一列去
    · padding-bottom 收一点 —— 卡身是勾选框不是表单行，不需要 .sec 默认那 8px
*/
.groups .sec { margin: 0 0 10px; padding-bottom: 10px; break-inside: avoid; }

/* 卡身：勾选框竖排 */
.gb { display: flex; flex-direction: column; gap: 8px; padding: 8px 14px 0; }

/*
  组内两列（判据与量法见上面 twoCol 那段注释）。5 项的组从 5 行变 3 行，两个长组各省两行。
  内边距从 14 收到 12、列缝取 10，都是为了把格子从 121.5 挤到 122.5 —— 韩语最长那条 114.5 差得不多。
*/
.groups.two .gb { display: grid; grid-template-columns: 1fr 1fr; gap: 8px 10px; padding: 8px 12px 0; }

/*
  ⚠️ <b>标签在这里必须允许折行。</b>`.setf .chk` 是 nowrap 的（那是给定高表行准备的），
  半宽格子里一旦有哪条超了，nowrap 会让它<b>压到右边那一列的字上</b>，而不是安静地换行。
  今天量过的三种语言都是一行放得下，所以这条现在不改变任何像素；
  它是给「以后哪条文案变长了」留的软着陆。
  align-items 跟着从 center 改成 flex-start，否则真折了行勾选框会跑到两行的正中间。
*/
.groups.two .gb .chk { align-items: flex-start; white-space: normal; text-align: left; }
/* 勾选框 12px 高，跟首行文字的中心对齐：12.5px × 1.4 行高 ≈ 17.5，(17.5 − 12) ÷ 2 ≈ 2.75 */
.groups.two .gb .chk i { margin-top: 2.75px; }
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
  /*
    ⚠️ 分区卡的抬头在这一档也要收 —— 它比原来那条纯文本小标题高出约 12px/张，
    四张就是 48px，而这一屏在 533px（150% 缩放）下本来就只剩「滚一点点」的余量。
    不收的话实测从 44px 溢出涨到 86px，韩语更是从 0 变成 45。
  */
  .groups .sec { margin-bottom: 6px; padding-bottom: 5px; }
  .groups .sec > .grp { padding: 3px 12px 2px; margin-bottom: 0; }
  .groups .sec > .grp::before { padding: 1px 3px 0; min-width: 19px; }
  .gb { gap: 5px; padding: 4px 12px 0; }
  .groups.two .gb { gap: 5px 10px; padding: 4px 12px 0; }
  .groups { margin-bottom: -7px; }
  .acts { padding: 10px 20px 2px; }
}
</style>
