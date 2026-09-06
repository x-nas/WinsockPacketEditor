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
  这一屏<b>没有「保存」</b> —— 两项都是点了立刻生效并落库（与「系统代理」那个
  滑动开关同一条口径），留个「保存」按钮反而让人以为不按就不算数。
*/
import { computed } from 'vue'
import { LANGS, defOf, lang, setLang, t, type Lang } from '../i18n'
import { setTheme, theme, type Theme } from '../stores/theme'
import SettingsModal from './proxy/SettingsModal.vue'

const props = defineProps<{ open: boolean }>()
const emit = defineEmits<{ (e: 'update:open', v: boolean): void }>()

/*
  两个主题各画一张小预览：三条色带（底 / 卡片 / 强调）加一条假标题栏。
  只写「深色 / 浅色」四个字的话，没切过的人不知道会变成什么样；
  色块一眼就说清了。用的是写死的十六进制而不是令牌 ——
  这两张图要<b>同时</b>显示两套配色，令牌只能给出当前那一套。
*/
const THEMES: Array<{ key: Theme; label: 'set.app.dark' | 'set.app.light'; sw: string[] }> = [
  { key: 'dark', label: 'set.app.dark', sw: ['#0a0a0f', '#12121a', '#1c1c2e', '#00ff88'] },
  { key: 'light', label: 'set.app.light', sw: ['#eef1f6', '#ffffff', '#e6eaf1', '#00874a'] },
]

const cur = computed(() => defOf(lang.value))

function pickLang(code: Lang): void {
  void setLang(code)
}

function pickTheme(k: Theme): void {
  void setTheme(k)
}
</script>

<template>
  <SettingsModal
    :open="props.open"
    :title="t('set.app')"
    subtitle="Appearance"
    readonly
    @update:open="emit('update:open', $event)"
  >
    <div class="setf">
      <div class="grp">{{ t('set.app.lang') }}</div>

      <!--
        七种语言铺成网格而不是下拉：一共就七个，铺开一眼看全，
        比「展开 → 找 → 点」少一步。名字一律用该语言自己的写法 ——
        切到看不懂的语言之后，自称是屏幕上唯一还认得出来的东西。
      -->
      <div class="opts lang">
        <button
          v-for="l in LANGS"
          :key="l.code"
          class="opt"
          :class="{ on: l.code === lang }"
          :aria-pressed="l.code === lang"
          @click="pickLang(l.code)"
        >
          <i class="tag">{{ l.short }}</i>
          <span class="nm">{{ l.label }}</span>
          <svg v-if="l.code === lang" class="tick" viewBox="0 0 24 24"><path d="M5 13l4 4L19 7" /></svg>
        </button>
      </div>
      <p class="tip">{{ t('set.app.langHint') }}</p>

      <div class="grp">{{ t('set.app.theme') }}</div>

      <div class="opts theme">
        <button
          v-for="x in THEMES"
          :key="x.key"
          class="opt th"
          :class="{ on: x.key === theme }"
          :aria-pressed="x.key === theme"
          @click="pickTheme(x.key)"
        >
          <span class="prev">
            <i v-for="(c, i) in x.sw" :key="i" class="sw" :style="{ background: c }" />
          </span>
          <span class="nm">{{ t(x.label) }}</span>
          <svg v-if="x.key === theme" class="tick" viewBox="0 0 24 24"><path d="M5 13l4 4L19 7" /></svg>
        </button>
      </div>
      <p class="tip">{{ t('set.app.themeHint') }}</p>

      <div class="grp">{{ t('set.app.now') }}</div>
      <div class="row">
        <div class="k">{{ t('set.app.lang') }}</div>
        <div class="v">{{ cur.label }} <i class="cult">{{ cur.culture }}</i></div>
      </div>
      <div class="row">
        <div class="k">{{ t('set.app.theme') }}</div>
        <div class="v">{{ t(theme === 'dark' ? 'set.app.dark' : 'set.app.light') }}</div>
      </div>
    </div>
  </SettingsModal>
</template>

<style scoped>
/* 选项网格。语言七个排三列，主题两个排两列 —— 主题那两张要放得下色带预览 */
.opts { display: grid; gap: 8px; padding: 2px 20px 4px; }
.opts.lang { grid-template-columns: repeat(3, 1fr); }
.opts.theme { grid-template-columns: repeat(2, 1fr); }

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
  font-size: 12.5px;
  line-height: 1;
  text-align: left;
  cursor: pointer;
}

.opt:hover { border-color: var(--cyan); color: var(--cyan); }
/* 选中：青边 + 一层极淡的底，与全项目「选中行」同一条视觉语言 */
.opt.on { border-color: var(--cyan); background: rgb(var(--cyan-rgb) / 10%); color: var(--cyan); }
.opt:focus-visible { outline: 1px solid var(--cyan); outline-offset: -2px; }

/* 两字母角标定宽，各行的语言名起点才对得齐 */
.tag {
  flex: none;
  width: 22px;
  font-family: var(--share);
  font-size: 10px;
  letter-spacing: .1em;
  color: var(--dim2);
  font-style: normal;
}

.opt.on .tag { color: var(--cyan); }
.nm { flex: 1; min-width: 0; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }

.tick { flex: none; width: 13px; height: 13px; fill: none; stroke: var(--cyan); stroke-width: 2.4; }

/* 主题预览：四条色带并排，宽度按令牌的层次递减 */
.opt.th { flex-direction: column; align-items: stretch; gap: 9px; padding: 11px; }
.prev { display: flex; height: 26px; border: 1px solid var(--border); overflow: hidden; }
.sw { flex: 1; }
.sw:first-child { flex: 2; }
.sw:last-child { flex: .5; }
.opt.th .nm { flex: none; }
.opt.th .tick { position: absolute; right: 9px; bottom: 11px; }

.cult { font-family: var(--mono); font-size: 11px; color: var(--dim2); font-style: normal; margin-left: 6px; }

/* 说明是整句，截断了就没意义 —— 与列表设置那几处同一条口径 */
.tip { margin: 0; padding: 0 20px 10px; font-size: 11.5px; line-height: 1.6; color: var(--dim2); }
</style>
