<script setup lang="ts">
/*
  软件设置 —— 界面语言 + 深浅色。

  【为什么单独一屏，而不是并进「系统设置」】那 12 个设置弹窗管的都是<b>抓包行为</b>
  （端口 / 拦截 / 过滤 / 防火墙…），只在进了模式之后才有意义，而且各自只属于一种模式。
  语言与主题管的是<b>这个程序长什么样</b>：启动页上就要能改，两种模式共用，
  也不该跟着「代理设置」一起被锁在代理模式里。所以它挂在标题栏，不进那个菜单。

  【为什么不是下拉而是弹窗】早先语言就是标题栏上一个 chip 点开的 ContextMenu。
  加了主题之后一个下拉装不下两组选项，而把两个 chip 并排摆在标题栏又太挤
  （右边紧挨着四个窗口按钮）。收进一个齿轮按钮 + 弹窗，标题栏反而空了一格。

  用 SettingsModal 这个共用外壳：四角标记 / 焦点管理 / 页脚按钮那几样它都做好了。

  【这一屏是「改草稿 → 按保存才生效」】
  早先是点一下立刻生效并落库（与「系统代理」那个滑动开关同一条口径），
  <b>2026-09-07 按要求改成了保存制</b>，与其余 12 个设置弹窗一致。

  所以下面两个 draft* 是<b>草稿</b>，不是真值：点卡片 / 选下拉只改草稿，
  onSave 才把改动推给 setLang / setTheme（它们自己会落库并同步到 C#）。
  取消、按 Esc、点遮罩关掉 —— 都不应用，草稿在下次打开时按当前真值重置。

  ⚠️ 由此带来的一个后果：<b>主题不再有「点了就看见」的即时预览</b>。
  三张卡上的色带预览就是补这个的 —— 它本来是为了「没切过的人不知道会变成什么样」，
  改成保存制之后它更要紧了。
*/
import { computed, ref, watch } from 'vue'
import { LANGS, defOf, lang, setLang, t, type Lang } from '../i18n'
import { scanLine, setScan, setTheme, systemIsDark, theme, type Theme } from '../stores/theme'
import CyberSelect from './CyberSelect.vue'
import SettingsModal from './proxy/SettingsModal.vue'

const props = defineProps<{ open: boolean }>()
const emit = defineEmits<{ (e: 'update:open', v: boolean): void }>()

/*
  三个主题各画一张小预览：四条色带（底 / 卡片 / 面板 / 强调）。
  只写「深色 / 浅色」几个字的话，没切过的人不知道会变成什么样；
  色块一眼就说清了。用的是写死的十六进制而不是令牌 ——
  这几张图要<b>同时</b>显示两套配色，而令牌只能给出当前那一套。

  「跟随系统」那张把两套各取一半拼起来（左深右浅），一眼就能读出
  「这一档不固定，看系统」。它<b>没有自己的配色</b>，落到像素上仍然只有深浅两种。
*/
type ThemeCard = { key: Theme; label: 'set.app.dark' | 'set.app.light' | 'set.app.system'; sw: string[] }

const DARK_SW = ['#0a0a0f', '#12121a', '#1c1c2e', '#00ff88']
const LIGHT_SW = ['#eef1f6', '#ffffff', '#e6eaf1', '#00874a']

const THEMES: ThemeCard[] = [
  { key: 'dark', label: 'set.app.dark', sw: DARK_SW },
  { key: 'light', label: 'set.app.light', sw: LIGHT_SW },
  { key: 'system', label: 'set.app.system', sw: [DARK_SW[0], DARK_SW[3], LIGHT_SW[3], LIGHT_SW[1]] },
]

/*
  草稿。打开弹窗时从当前真值取一份，之后只改它 —— 按「保存」才落到真值上。

  ⚠️ 必须在<b>每次打开时</b>重置，不能只在组件创建时取一次：
  弹窗是 v-model:open 控制显隐、组件一直挂着的，上一次改完没保存就关掉的话，
  草稿会留在那儿，下次打开看到的是上次没保存的选择。
*/
const draftLang = ref<Lang>(lang.value)
const draftTheme = ref<Theme>(theme.value)
const draftScan = ref(scanLine.value)

watch(() => props.open, (on) => {
  if (!on) return
  draftLang.value = lang.value
  draftTheme.value = theme.value
  draftScan.value = scanLine.value
})

const busy = ref(false)

const cur = computed(() => defOf(draftLang.value))

/*
  语言用下拉，不铺成网格 —— 七个已经占掉三行，再加语言只会更长，
  而这一屏总共才两组设置。下拉的高度与语言个数无关。

  【label 为什么带两字母前缀】
  · 名字一律用<b>该语言自己的写法</b>（日本語 而不是 Japanese）——
    切到一种看不懂的语言之后，自称是屏幕上唯一还认得出来的东西；
  · 但 CJK 的名字没法按首字母跳，而 CyberSelect 支持「敲首字母跳到下一个匹配项」。
    前缀补上之后 c/t/e/j/k/v/r 七个各不相同，键盘用户敲一下就到。
  等宽字体下这两列自然对齐（.cs-btn / .cs-opt 都是 var(--mono)）。
*/
const langOptions = computed(() =>
  LANGS.map((l) => ({ value: l.code, label: l.short + '  ' + l.label })))

function pickLang(code: Lang): void {
  draftLang.value = code
}

function pickTheme(k: Theme): void {
  draftTheme.value = k
}

/**
 * 保存。
 *
 * 只推<b>真的变了</b>的那一项：setLang / setTheme 内部各带一次桥往返 + 落库，
 * 没变还推一遍是白费一次写库，而 setTheme 还会顺带 ApplyPrefs 让 AntdUI 重画。
 * （两个函数自己也有「值没变就 return」的短路，这里再判一次是为了连 busy 都不必进。）
 */
async function onSave(): Promise<void> {
  busy.value = true

  try {
    if (draftLang.value !== lang.value) { await setLang(draftLang.value) }
    if (draftTheme.value !== theme.value) { await setTheme(draftTheme.value) }
    if (draftScan.value !== scanLine.value) { await setScan(draftScan.value) }

    emit('update:open', false)
  } finally {
    busy.value = false
  }
}
</script>

<template>
  <SettingsModal
    :open="props.open"
    :title="t('set.app')"
    subtitle="Appearance"
    :busy="busy"
    @update:open="emit('update:open', $event)"
    @save="onSave"
  >
    <div class="setf">
      <div class="grp">{{ t('set.app.lang') }}</div>

      <!--
        不套 .setf .row：那是「标签列 + 控件」的两栏格，而组标题已经写着「界面语言」，
        行标签再写一遍就是重复。这一组只有一个控件，直接与下面「外观」那组同构 ——
        组标题 → 控件 → 提示，三行到底。
      -->
      <div class="one">
        <CyberSelect
          class="sel"
          :model-value="draftLang"
          :options="langOptions"
          @update:model-value="pickLang($event as Lang)"
        />
        <!-- 文化名：下拉里只有语言的自称，出问题时要看的是它到底切成了哪个 culture -->
        <i class="cult">{{ cur.culture }}</i>
      </div>
      <p class="tip">{{ t('set.app.langHint') }}</p>

      <div class="grp">{{ t('set.app.theme') }}</div>

      <div class="opts theme">
        <button
          v-for="x in THEMES"
          :key="x.key"
          class="opt th"
          :class="{ on: x.key === draftTheme, sys: x.key === 'system' }"
          :aria-pressed="x.key === draftTheme"
          @click="pickTheme(x.key)"
        >
          <span class="prev">
            <i v-for="(c, i) in x.sw" :key="i" class="sw" :style="{ background: c }" />
          </span>
          <span class="nm">{{ t(x.label) }}</span>
          <svg v-if="x.key === draftTheme" class="tick" viewBox="0 0 24 24"><path d="M5 13l4 4L19 7" /></svg>
        </button>
      </div>
      <!--
        选中「跟随系统」时，把<b>系统此刻是深是浅</b>接在提示语后面 ——
        只显示「跟随系统」的话，用户没法确认它到底认出来没有
        （这正是这一档最容易被怀疑的地方）。

        ⚠️ 读的是 systemIsDark 而不是 effective：后者是「现在实际生效的主题」，
        而这里草稿刚选上跟随系统、还没按保存，effective 仍停在旧主题上，
        拿它显示就是错的。这一句说的是系统那边的事，与应用了没有无关。

        原先这句在下面单独一组「当前」里，连同「当前语言」一行；
        语言换成下拉之后那一行就是重复，整组去掉了。
      -->
      <p class="tip">
        {{ t('set.app.themeHint') }}
        <b v-if="draftTheme === 'system'" class="now">
          {{ t('set.app.now') }} · {{ t(systemIsDark ? 'set.app.dark' : 'set.app.light') }}
        </b>
      </p>

      <div class="grp">{{ t('set.app.ambience') }}</div>

      <!--
        游走亮带的开关。放在主题下面单成一组 —— 它不是「深还是浅」的一部分，
        是「这套皮肤的动效要不要」，与主题正交（浅色下同样有这条带子）。
      -->
      <div class="one">
        <button class="chk" :class="{ on: draftScan }" @click="draftScan = !draftScan">
          <i />{{ t('set.app.scan') }}
        </button>
      </div>
      <p class="tip">{{ t('set.app.scanHint') }}</p>
    </div>
  </SettingsModal>
</template>

<style scoped>
/* 主题三张卡排三列 —— 每张要放得下色带预览 */
.opts { display: grid; gap: 8px; padding: 2px 20px 4px; }
.opts.theme { grid-template-columns: repeat(3, 1fr); }

/* 单控件那一行：与 .opts 用同一份内边距，控件左沿才和下面的主题卡对齐 */
.one { display: flex; align-items: center; gap: 10px; padding: 2px 20px 4px; }

/*
  语言下拉。定宽 190 —— 最长的是「Tiếng Việt」加两字母前缀，
  给内容宽度会让下拉框随语言变宽，右边那个文化名跟着左右跳。
*/
.sel { width: 190px; }

/* 跟随系统时接在提示语后面的「当前 · 深色」 */
.now { color: var(--cyan); font-weight: 400; white-space: nowrap; }

.opt {
  position: relative;
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 9px 11px 8px;
  background: var(--card);
  border: 1px solid var(--border);
  color: var(--gray);
  font-family: inherit;
  font-size: var(--fs-body);
  line-height: 1;
  text-align: left;
  cursor: pointer;
}

.opt:hover { border-color: var(--cyan); color: var(--cyan); }
/* 选中：青边 + 一层极淡的底，与全项目「选中行」同一条视觉语言 */
.opt.on { border-color: var(--cyan); background: rgb(var(--cyan-rgb) / 10%); color: var(--cyan); }
.opt:focus-visible { outline: 1px solid var(--cyan); outline-offset: -2px; }

.nm { flex: 1; min-width: 0; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }

.tick { flex: none; width: 13px; height: 13px; fill: none; stroke: var(--cyan); stroke-width: 2.4; }

/* 主题预览：四条色带并排，宽度按令牌的层次递减（底色占得最多、强调色只是一道） */
.opt.th { flex-direction: column; align-items: stretch; gap: 9px; padding: 11px; }
.prev { display: flex; height: 26px; border: 1px solid var(--border); overflow: hidden; }
.sw { flex: 1; }
.sw:first-child { flex: 2; }
.sw:last-child { flex: .5; }

/*
  「跟随系统」那张是<b>两套配色各占一半</b>（深底 + 深色强调 | 浅色强调 + 浅底），
  所以四条要等宽 —— 沿用上面那套递减权重会把右边的浅色压成一道细缝，
  看着就不像「一半一半」了。
*/
.opt.sys .sw:first-child, .opt.sys .sw:last-child { flex: 1; }
.opt.th .nm { flex: none; }
.opt.th .tick { position: absolute; right: 9px; bottom: 11px; }

.cult { font-family: var(--mono); font-size: var(--fs-small); color: var(--dim2); font-style: normal; margin-left: 6px; }

/* 说明是整句，截断了就没意义 —— 与列表设置那几处同一条口径 */
.tip { margin: 0; padding: 0 20px 10px; font-size: var(--fs-small); line-height: 1.6; color: var(--dim2); }
</style>
