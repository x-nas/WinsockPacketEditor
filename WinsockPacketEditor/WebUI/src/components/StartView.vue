<script setup lang="ts">
/*
  启动页（模式选择）—— 对应 WinForms 的 Forms/StartForm.cs。

  风格取自官网 WPEWeb：glitch 标题、eyebrow、发丝分隔的卡片网格、终端自检块。
  配色令牌在 style.css 里，与 cyber.css 的 :root 逐个对应。

  【卡片网格里只有「模式」】
  注入 / 代理是两种<b>运行方式</b>，选完就回不去了（照搬主程序：StartForm 设完模式直接 Close）。
  多开设置不是模式，是一屏设置，所以它<b>不占卡片</b>，降到网格下面那条窄入口里。

  ⚠️ <b>但它也不能搬进标题栏的齿轮。</b>切库只在这一屏是安全的：
  saveInstance 只重载 LoadSystemConfig_FromDB，那 14 份列表一份都不重载
  （它们由 enterProxyMode 加载一次，之后 proxyLoaded 就挡住了）。
  进过模式再切库，内存里还是<b>旧库</b>的列表，而 SaveProxyState 会在关窗与每 10 分钟
  把它们写进<b>新库</b> —— 多开本来是为了两个实例互不干扰，那样等于开了一条静默污染的路。
  「还没进模式 ⇒ 列表还没加载 ⇒ 切库无害」这个前提只在启动页成立，
  WinForms 那边 Controls/DataBaseSetting 也只能从 StartForm 打开，同一个约束。
*/
import { computed, onMounted, ref, watch } from 'vue'
import { call } from '../bridge'
import { lang, t } from '../i18n'

const emit = defineEmits<{ (e: 'enter', mode: 'proxy' | 'instance' | 'inject'): void }>()

interface SystemCheck {
  isAdmin: boolean
  dbDir: string
  dbFile: string
  dbFull: string
  dbInstance: string
  version: string
  isBeta: boolean
  lastInjection: string
  socks5Port: number
  geoVersion: string
  geoCount: number
}

const sys = ref<SystemCheck | null>(null)

/*
  打字机：副标题逐字打出来，与官网 .subtitle[data-type] 同一效果。

  切换语言时<b>重新打一遍</b>，而不是把 typed 直接换成新文案 ——
  后者会让一句已经打完的英文凭空出现，与首屏观感不一致。
  runId 让上一轮循环认出自己已经过期：不加它，两轮会交替往 typed 里写。
*/
const typed = ref('')
let runId = 0

async function typeSubtitle(): Promise<void> {
  const mine = ++runId
  const text = t('start.subtitle')
  typed.value = ''

  // 官网是每 32ms 一个字
  for (let i = 1; i <= text.length; i++) {
    if (mine !== runId) return
    typed.value = text.slice(0, i)
    await new Promise((r) => setTimeout(r, 32))
  }
}

watch(lang, () => { void typeSubtitle() })

/** 点击 / 回车 / 空格 都走这里。 */
function enterInject(): void {
  emit('enter', 'inject')
}

function enterProxy(): void {
  emit('enter', 'proxy')
}

function enterInstance(): void {
  emit('enter', 'instance')
}

/** 条目数加千分位，1512917 这种数字不分组基本读不出量级。 */
const geoCountText = computed(() =>
  sys.value?.geoCount ? sys.value.geoCount.toLocaleString() : '—')

onMounted(async () => {
  try {
    sys.value = await call<SystemCheck>('getSystemCheck')
  } catch (e) {
    console.error('[start] 取自检信息失败', e)
  }

  void typeSubtitle()
})

</script>

<template>
  <main class="start scrn">
    <div class="eyebrow">
      <span class="dash" />
      <span class="lbl">{{ t('start.eyebrow') }}</span>
    </div>

    <h1 class="ttl glitch" data-text="Winsock Packet Editor">Winsock Packet Editor</h1>

    <p class="subtitle">{{ typed }}<span class="cur" /></p>

    <!--
      机架：一圈 1px 外框把「这次怎么跑」的三个入口装在一起，槽位之间留 10px 缝。

      【为什么不是三块各自漂着】多开那条窄行的从属关系是<b>画出来的</b> ——
      它靠共用同一个框才读得出「比上面两张矮一级」。拆成三个各带完整边框的独立块之后，
      主次就只剩高度在撑，而这一屏下面还有自检终端，整屏会变成四块没有骨架的浮板。

      【为什么不是连体】卡片顶上各有一条色轨。贴着 1px 发丝缝相邻的话，
      两条轨会读成一条「左绿右青」的双色长条 —— 像进度条，而不是两台设备各自的电源轨。

      ⚠️ <b>槽缝不给背景，透出页面底色。</b>不能填 --sink：深色下它是纯黑、比页面更深，
      看着对；但浅色下 --sink 与 --card 都是 #ffffff，缝隙会整个消失，
      外框那条线就成了一条没来由的双线。透出页面底色则两套皮肤下「腔比卡深」都成立
      （深 #0a0a0f vs #12121a、浅 #eef1f6 vs #ffffff），一个令牌都不用加。
    -->
    <div class="rack">
      <div class="cards">
        <!--
          注入模式：IPC 改造之后可用了。
          与代理那张同构 —— 不带 aria-disabled / title，它们是「点了没反应」时才需要的。
        -->
        <div
          class="cd"
          role="button"
          tabindex="0"
          @click="enterInject"
          @keydown.enter.prevent="enterInject"
          @keydown.space.prevent="enterInject"
        >
          <span class="rail" />
          <!-- 机位号丝印。它是印在面板上的，不是内容 —— 所以不吃点击、也选不中 -->
          <span class="wm">01</span>

          <div class="hd">
            <span class="num">Mode 01</span>
            <!--
              状态灯。启动页上两种模式都还没起来，所以恒为 Ready；
              它要说的不是运行状态，是「这台设备通着电」——
              与下面自检终端那三颗窗口灯同一套语汇。
            -->
            <span class="st"><i class="dot" />Ready</span>
          </div>

          <div class="t">
            <span class="well">
              <!-- 芯片：外壳 + 内核 + 四面引脚。原来的图只有左右两侧有脚，更像一枚电池 -->
              <svg class="ico" viewBox="0 0 24 24">
                <rect x="5" y="5" width="14" height="14" rx="1.5" />
                <rect x="9.5" y="9.5" width="5" height="5" />
                <path d="M9 2v3M15 2v3M9 19v3M15 19v3M2 9h3M2 15h3M19 9h3M19 15h3" />
              </svg>
            </span>
            <span class="tx">
              <b class="en">Inject</b>
              <i class="zh">{{ t('start.inject.zh') }}</i>
            </span>
          </div>

          <p>{{ t('start.inject.desc') }}</p>

          <!-- 读数条：与另外两个入口同构，各显示一条本模式的实测数据 -->
          <div class="foot">
            <span class="last">
              <span class="k">Target</span>
              <b>{{ sys?.lastInjection || '—' }}</b>
            </span>
            <span class="ar">→</span>
          </div>
        </div>

        <!--
          代理模式。
          用 role="button" + tabindex 而不是真的 <button>：
          <button> 的内容模型只允许短语内容，而卡片里是 div/p 这些块级元素，
          塞进去不合规范。卡片式可点区域的标准做法就是这一套。
        -->
        <div
          class="cd cy"
          role="button"
          tabindex="0"
          @click="enterProxy"
          @keydown.enter.prevent="enterProxy"
          @keydown.space.prevent="enterProxy"
        >
          <span class="rail" />
          <span class="wm">02</span>

          <div class="hd">
            <span class="num">Mode 02</span>
            <span class="st"><i class="dot" />Ready</span>
          </div>

          <div class="t">
            <span class="well">
              <!--
                火箭 = 加速器。与官网侧栏「代理客户端」那一项用的是同一枚图标
                （cyber.js 的 wpc.html），产品家族里「加速」一直是这个符号。
                原来那枚是房子轮廓，读起来是「主页」而不是代理。
              -->
              <svg class="ico" viewBox="0 0 24 24">
                <path d="M4.5 16.5c-1.5 1.26-2 5-2 5s3.74-.5 5-2c.71-.84.7-2.13-.09-2.91a2.18 2.18 0 0 0-2.91-.09z" />
                <path d="M12 15l-3-3a22 22 0 0 1 2-3.95A12.88 12.88 0 0 1 22 2c0 2.72-.78 7.5-6 11a22.35 22.35 0 0 1-4 2z" />
                <path d="M9 12H4s.55-3.03 2-4c1.62-1.08 5 0 5 0" />
                <path d="M12 15v5s3.03-.55 4-2c1.08-1.62 0-5 0-5" />
              </svg>
            </span>
            <span class="tx">
              <b class="en">Proxy</b>
              <i class="zh">{{ t('start.proxy.zh') }}</i>
            </span>
          </div>

          <p>{{ t('start.proxy.desc') }}</p>

          <div class="foot">
            <span class="last">
              <span class="k">Socks5</span>
              <b>0.0.0.0:{{ sys?.socks5Port ?? '—' }}</b>
            </span>
            <span class="ar">→</span>
          </div>
        </div>
      </div>

      <!--
        多开设置：机架里的第三个槽位，高度只有卡片的六分之一。

        它与上面两张是「同一件事的两级」—— 都在这一屏决定「这次怎么跑」，
        但它不是模式，所以不给它一张同款卡片。

        这里可以用<b>真的 &lt;button&gt;</b>（上面两张卡不行）——
        一行里全是 svg 与 span，都是短语内容，合规范；卡片里是 div/p，
        塞进 button 不合内容模型，那两张才要 role="button" + tabindex 那套。
      -->
      <button class="inst" @click="enterInstance">
        <svg class="ico" viewBox="0 0 24 24">
          <ellipse cx="12" cy="6" rx="8" ry="3" />
          <path d="M4 6v6c0 1.7 3.6 3 8 3s8-1.3 8-3V6M4 12v6c0 1.7 3.6 3 8 3s8-1.3 8-3v-6" />
        </svg>
        <span class="nm">{{ t('start.inst.zh') }}</span>
        <span class="ds">{{ t('start.inst.desc') }}</span>
        <span class="cur">{{ sys?.dbInstance || '—' }}</span>
        <span class="ar">→</span>
      </button>
    </div>

    <!-- 系统自检 -->
    <div class="term">
      <div class="term-bar">
        <span class="d" style="background:#ff5f57" />
        <span class="d" style="background:#febc2e" />
        <span class="d" style="background:#28c840" />
        <span class="lbl">{{ t('start.check') }}</span>
      </div>
      <div class="term-body">
        <span class="l"><span class="c">$</span> <span class="g">wpe64</span> --self-check</span>
        <span class="l">
          <span class="c">  ├─</span> {{ t('start.admin') }}
          <span :class="sys?.isAdmin ? 'g' : 'r'">{{ sys ? (sys.isAdmin ? 'OK' : 'NO') : '…' }}</span>
          <span class="c">   ├─</span> {{ t('start.geo') }}
          <span class="y">{{ sys?.geoVersion || '…' }}</span>
          <!--
            分隔点两侧的空格写在 span 的文本节点<b>里面</b>，不要靠元素之间的换行。
            这一行是 white-space: pre，而 Vue 模板编译器默认的 condense 会把
            「纯空白且含换行」的文本节点整个删掉 —— 上一个 </span> 与这里之间正是这种节点，
            于是点号左边没空格、右边（同一行的那个空格）有，看着就不居中。
            上面几个 ├─ 的空格也是写在 span 内部的，同一套写法。
          -->
          <span class="c"> · </span><span class="g">{{ geoCountText }}</span> {{ t('start.geoUnit') }}
          <span class="c">   ├─</span> {{ t('start.db') }} <span class="y">{{ sys?.dbFull || '…' }}</span>
        </span>
        <span class="l"><span class="c">  └─</span> {{ t('start.done') }} <span class="a">_</span></span>
      </div>
    </div>
  </main>
</template>

<style scoped>
/*
  这一屏的竖直呼吸量全部提成令牌，窗口一矮就整屏往里收（下面两个 @media）。

  【为什么必须这么做】外壳的默认窗口是 <b>ClientSize 1280×800，那是设备像素</b>；
  页面拿到的是 CSS 像素 = 设备像素 ÷ 缩放比。所以同一个「默认大小」的窗口，
  在 100% 缩放下有 800 CSS 高，125% 只剩 640，150% 只剩 533 ——
  而标题栏 46 + 状态栏 30 还要先扣掉 76。
  照 800 那档排版，缩放一开就出滚动条：这一屏是「一眼看完、选一种模式」的，
  出滚动条比挤一点糟得多。

  ⚠️ 改这一屏的任何间距都改令牌，别直接写死值 —— 写死的那处在矮窗口下不会跟着收，
  而它省下的那几像素往往正是压垮的最后一根。
*/
.start {
  --hd: var(--green);
  --hd-rgb: var(--green-rgb);
  --hd-glow: 40%;      /* 首屏这一档比二级页浓一点，那是原来就量好的 */
  --hd-gap: 16px;      /* eyebrow 与标题之间（共用件 .scrn 的令牌）*/
  --hd-sub: 14px;      /* 标题与副标题之间（共用件 .scrn 的令牌）*/
  --g-rack: 34px;      /* 副标题与机架之间 */
  --g-term: 26px;      /* 机架与自检终端之间 */
  --slot: 10px;        /* 机架的内边距 ＝ 槽缝，两者必须同值 */
  --cd-py: 16px;       /* 卡片的上下内边距 */
  --cd-ry: 12px;       /* 卡片里三处竖直间距（状态行 / 标题行 / 描述）*/
  --term-py: 12px;     /* 终端正文的上下内边距 */
  --inst-h: 40px;      /* 多开那条窄行的高度 */
  --ttl-size: clamp(32px, 3.6vw, 46px);

  position: relative;
  z-index: 10;
  flex: 1;
  min-height: 0;
  display: flex;
  flex-direction: column;
  /*
    safe 是必需的：普通的 center 在内容装不下时会把<b>上半截推到滚动区外</b>，
    那一截既滚不到也点不着。safe 在溢出时自动退回 start 对齐。
  */
  justify-content: safe center;
  padding: 0 56px;
  overflow: auto;
}

/* 1280×800 在 125% 缩放下的那一档（CSS 640 高）*/
@media (max-height: 690px) {
  .start {
    --hd-gap: 12px;
    --hd-sub: 11px;
    --g-rack: 26px;
    --g-term: 21px;
    --slot: 9px;
    --cd-py: 14px;
    --cd-ry: 10px;
    --term-py: 10px;
    --inst-h: 38px;
    --ttl-size: clamp(30px, 3.4vw, 42px);
  }
}

/* 150% 缩放（CSS 533 高）以及被拖到很矮的窗口 */
@media (max-height: 590px) {
  .start {
    --hd-gap: 6px;
    --hd-sub: 7px;
    --g-rack: 14px;
    --g-term: 12px;
    --slot: 7px;
    --cd-py: 10px;
    --cd-ry: 7px;
    --term-py: 7px;
    --inst-h: 34px;
    --ttl-size: clamp(24px, 2.8vw, 32px);
  }
}

/*
  eyebrow 与标题的<b>基样式在 style.css 的 `.scrn`</b>（三屏共用），这里只留本屏独有的：
  ① 那两条 latin-only 的排版（标题是写死的英文）；② 三层错位霓虹。

  ⚠️ 别把 font-family / 字号 / 颜色再抄回来 —— `.glitch[data-v-x]` 与 `.scrn .ttl`
  同为 (0,2,0)，平局时<b>后加载的 style.css 赢</b>（main.ts 先 import App.vue），抄了也不生效。
  真要覆盖得提高特异度。
*/
.glitch {
  text-transform: uppercase;
  letter-spacing: -.02em;
  line-height: 1;
  position: relative;
  display: inline-block;
}

.glitch::before,
.glitch::after {
  content: attr(data-text);
  position: absolute;
  inset: 0;
  background: var(--black);
  overflow: hidden;
  opacity: .75;
  mix-blend-mode: screen;
}

.glitch::before { color: var(--magenta); animation: g1 2.4s infinite steps(2) alternate-reverse; }
.glitch::after { color: var(--cyan); animation: g2 3.2s infinite steps(2) alternate-reverse; }

/*
  ⚠️ 浅色下混合模式要反过来：screen 只在<b>深底</b>上是「加一层彩边」。
  错位层自带一块页面底色（--black）用来盖住那一截原字，深色下它是近黑，screen 上去等于没有，
  只留下彩色的错位字 → 读成霓虹色散；浅色下它是 #eef1f6，screen 一叠那一截原字被洗成近白，
  只剩一条发灰、偏 2px 的错位字 —— 看着就是「字体被切歪了」（2026-09-11 用户截图报的）。
  multiply 是 screen 在浅底上的镜像（浅底 ≈ 不变、彩字把原字压暗一截），字形完整；
  浓度降到 .45，否则压暗的那一截太重，读成一道黑带。
*/
:root[data-theme="light"] .glitch::before,
:root[data-theme="light"] .glitch::after {
  mix-blend-mode: multiply;
  opacity: .45;
}

@keyframes g1 {
  0% { clip-path: inset(20% 0 60% 0); transform: translate(-2px); }
  100% { clip-path: inset(65% 0 8% 0); transform: translate(2px); }
}

@keyframes g2 {
  0% { clip-path: inset(70% 0 10% 0); transform: translate(2px); }
  100% { clip-path: inset(15% 0 70% 0); transform: translate(-2px); }
}

/* 副标题与光标的基样式在 style.css 的 `.scrn` 里（三屏共用），这里只给间距 */

/*
  机架：一圈 1px 外框把两张模式卡与多开窄行装在一起。

  ⚠️ <b>不给 background</b> —— 槽缝要透出页面底色。填 --sink 的话浅色下会烂：
  那边 --sink 与 --card 都是 #ffffff，缝隙整个消失，外框就成了一条没来由的双线。
  透出页面底色则两套皮肤下「腔比卡深」都成立（深 #0a0a0f vs #12121a、浅 #eef1f6 vs #ffffff）。
*/
.rack {
  margin: var(--g-rack) 0 0;
  padding: var(--slot);
  border: 1px solid var(--border);
}

/* 槽缝与机架的内边距取同一个值（同一个令牌），卡片到框的距离才处处一样 */
.cards {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: var(--slot);
}

.cd {
  position: relative;
  overflow: hidden;
  background: var(--card);
  border: 1px solid var(--border);
  padding: var(--cd-py) 16px calc(var(--cd-py) - 2px);
  cursor: pointer;
  transition: background .15s, border-color .15s;
}

.cd:hover { background: var(--panel); border-color: var(--green); }
.cd.cy:hover { border-color: var(--cyan); }

/*
  焦点环画在<b>内侧</b>：卡片外面只隔 10px 就是机架的框，正偏移会撞上去。
  顺带让 hover 那几处在键盘聚焦时也点亮 —— 两种操作方式给同样的提示。
*/
.cd:focus-visible { outline-offset: -2px; }
.cd.cy:focus-visible { outline-color: var(--cyan); }

/*
  顶沿的机架色轨：左三分之一实心、右侧衰减到 14%。
  设备面板上的丝印色条，也是「这张卡是绿的还是青的」在第一眼就说清的地方。
  悬停 / 聚焦时整条点亮。
*/
.rail {
  position: absolute;
  top: 0;
  left: 0;
  right: 0;
  height: 3px;
  background: linear-gradient(90deg, var(--green) 0 34%, rgb(var(--green-rgb) / 14%) 34%);
  transition: background .15s;
}

.cd.cy .rail { background: linear-gradient(90deg, var(--cyan) 0 34%, rgb(var(--cyan-rgb) / 14%) 34%); }
.cd:hover .rail, .cd:focus-visible .rail { background: var(--green); }
.cd.cy:hover .rail, .cd.cy:focus-visible .rail { background: var(--cyan); }

/*
  机位号水印。工业面板上的大号丝印数字 —— 几乎不占视觉预算，却把版面撑开了。
  竖直居中而不是贴顶：贴顶会与右上角那枚状态灯叠在一起，两样东西挤成一团。
  透明度写成通道值，两套皮肤各自算：深色下是亮绿的 8%，浅色下是暗绿的 8%，
  都落在「看得出、读不清」这个刚好的位置上。
*/
.wm {
  position: absolute;
  right: 12px;
  top: 50%;
  transform: translateY(-50%);
  font-family: var(--orbit);
  font-weight: 900;
  font-size: 62px;
  line-height: 1;
  letter-spacing: -.04em;
  color: rgb(var(--green-rgb) / 8%);
  pointer-events: none;
  user-select: none;
}

.cd.cy .wm { color: rgb(var(--cyan-rgb) / 8%); }

/* 下面几层都要压在水印上面，所以各自 position: relative */
.hd {
  position: relative;
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 10px;
  margin-bottom: var(--cd-ry);
}

.cd .num {
  /* top .4px：--fs-caption 提到 10.5px（2026-09-13）后重量，−0.61 → −0.21 */
  position: relative;
  top: .4px;
  font-family: var(--share);
  font-size: var(--fs-caption);
  letter-spacing: .18em;
  color: var(--muted);
}

/*
  状态灯。启动页上两种模式都还没起来，所以恒为 Ready ——
  它说的不是运行状态，是「这台设备通着电」。
*/
.st {
  display: flex;
  align-items: center;
  gap: 6px;
  font-family: var(--share);
  font-size: var(--fs-caption);
  letter-spacing: .18em;
  color: var(--green);
  white-space: nowrap;
}

.cd.cy .st { color: var(--cyan); }

.dot {
  width: 6px;
  height: 6px;
  border-radius: 50%;
  background: currentColor;
  box-shadow: 0 0 6px currentColor;
}

.cd .t {
  position: relative;
  display: flex;
  align-items: center;
  gap: 11px;
  margin-bottom: var(--cd-ry);
}

/*
  仪表窗：图标从「贴在标题左边的装饰」变成一个装在面板上的器件。
  颜色给在窗上，.ico 是 stroke: currentColor，跟着走。
*/
.well {
  position: relative;
  flex: none;
  width: 38px;
  height: 38px;
  display: flex;
  align-items: center;
  justify-content: center;
  border: 1px solid var(--border);
  background: rgb(var(--inset-rgb) / 30%);
  color: var(--green);
}

.cd.cy .well { color: var(--cyan); }

/* 对角两枚角标 —— 与全项目那套四角标记同一种语言；四枚太吵，两枚就够点题 */
.well::before,
.well::after {
  content: "";
  position: absolute;
  width: 5px;
  height: 5px;
  border: 0 solid currentColor;
}

.well::before { left: -1px; top: -1px; border-left-width: 1px; border-top-width: 1px; }
.well::after { right: -1px; bottom: -1px; border-right-width: 1px; border-bottom-width: 1px; }

/*
  ⚠️ 双行标题要与仪表窗<b>上下齐平</b>：EN 的墨迹顶 = 窗顶，中文那行的墨迹底 = 窗底。
  两个数都是量出来的，别凭感觉调 ——

    .zh 的 margin-top   决定<b>墨迹跨度</b>（两行墨迹一共占多高）
    .tx 的 padding-bottom 决定<b>整块往上抬多少</b>（窗是在 .t 里居中的，块高一变窗就跟着挪）

  ⚠️ 跨度补不到每种语言都是 0：行高是定值（25.5 / 22.1），所以 EN 那头对所有语言都一样，
  而中文那一行的墨迹底在行盒里的位置<b>随语种变</b>（日韩最高、俄语最低，实测差 2.13px）。
  ⚠️ 中文那行是 13px 时量的是 1.85 / 2；2026-09-11 字号规范把它改成 13.5px（--fs-lead）后，
  margin-top 重量为 <b>1.1px</b>（简体：EN 墨顶 +0.08、中文墨底 −0.42，与改前的 +0.13 / −0.42 相同）。
  当年 1.85 / 2 取的是让<b>最大偏差最小</b>的那一组，与「.ordbar 徽标别按某一种语言调到 0」同一条口径。

  实测（正号＝比窗底低）：EN 墨顶 <b>七种语言恒为 0.00</b>；中文那行的墨底
  简 / 繁 −0.43 · 日 / 韩 −1.06 · 英 +0.20 · 越 +0.83 · 俄 +1.08（改前是 +1.02 ~ +3.15）。

  两行墨迹之间还留着 14.4px，<b>离重叠远得很</b>；块高 51.6 → 51.45，卡片高度不变。
  换字号 / 换字体栈 / 改行高，这两个数都要回来重量。
*/
.tx { min-width: 0; padding-bottom: 2px; }

.en {
  display: block;
  font-family: var(--orbit);
  font-weight: 400;
  font-size: var(--fs-title);
  text-transform: uppercase;
  letter-spacing: .07em;
  color: var(--green);
}

.cd.cy .en { color: var(--cyan); }

/* <i> 只是拿来当行内容器，斜体要关掉 */
.zh { display: block; margin-top: 1.1px; font-size: var(--fs-lead); font-style: normal; color: var(--gray); }

.cd p { position: relative; margin: 0 0 var(--cd-ry); font-size: var(--fs-lead); line-height: 1.55; color: var(--muted); }

/* 读数条 + 右端箭头同在一行：箭头另起一行会平白多 20px 高 */
.foot { position: relative; display: flex; align-items: center; gap: 10px; }

/*
  读数条。这一屏每个入口都显示一条自己的实测值，做成读数窗才配得上这个定位。
  底色用 --inset 而不是 --sink：后者在浅色下与 --card 同为 #ffffff，条就没了。
*/
.cd .last {
  flex: 1;
  min-width: 0;
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 10px;
  padding: 6px 10px;
  background: rgb(var(--inset-rgb) / 30%);
  border: 1px solid var(--border);
}

.cd .last .k {
  flex: none;
  /* top .4px：--fs-caption 提到 10.5px（2026-09-13）后重量，−0.62 → −0.22 */
  position: relative;
  top: .4px;
  font-family: var(--share);
  font-size: var(--fs-caption);
  letter-spacing: .14em;
  text-transform: uppercase;
  color: var(--dim);
}

/* 进程路径可以很长 —— 值这一格必须能截断，否则会把整张卡撑宽 */
.cd .last b {
  min-width: 0;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  font-family: var(--mono);
  font-size: var(--fs-small);
  font-weight: 400;
  color: var(--cyan);
}

/*
  箭头常驻，不做成 hover 才出现：多开那条窄行的箭头本来就一直在，两处对齐；
  而且「留一块空位等悬停」在视觉上是个说不清的缺口。
*/
.cd .ar { flex: none; color: var(--dim); transition: color .15s; }
.cd:hover .ar, .cd:focus-visible .ar { color: var(--green); }
.cd.cy:hover .ar, .cd.cy:focus-visible .ar { color: var(--cyan); }

/*
  多开设置那条窄入口 —— 机架里的第三个槽位，与上面两张卡隔同样的 10px。

  洋红沿用它当卡片时的色相：这一屏三个入口各有一个颜色，换了位置不该换身份。
*/
.inst {
  display: flex;
  align-items: center;
  gap: 10px;
  width: 100%;
  margin-top: var(--slot);
  padding: 0 16px 0 18px;
  height: var(--inst-h);
  border: 1px solid var(--border);
  background: var(--card);
  color: var(--muted);
  font-family: inherit;
  font-size: var(--fs-lead);
  text-align: left;
  cursor: pointer;
  transition: .15s;
}

.inst:hover { background: var(--panel); border-color: var(--magenta); }
.inst:hover .nm,
.inst:hover .ar { color: var(--magenta); }
/* 焦点环走内侧：外面只隔 10px 就是机架的框，正偏移会撞上去 */
.inst:focus-visible { outline: 1px solid var(--magenta); outline-offset: -2px; }
.inst:focus-visible .nm,
.inst:focus-visible .ar { color: var(--magenta); }

.inst .ico { width: 15px; height: 15px; color: var(--magenta); flex: none; }
.inst .nm { color: var(--gray); flex: none; transition: .15s; }

/* 说明占满中间；窄屏或俄语这类长文案下截断，别把右边的实例名挤掉 */
.inst .ds {
  flex: 1;
  min-width: 0;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  color: var(--dim4);
  font-size: var(--fs-body);
}

/* 当前实例名 —— 与卡片的读数条同一个作用：这一屏的每个入口都显示一条实测值 */
.inst .cur {
  flex: none;
  font-family: var(--mono);
  font-size: var(--fs-body);
  color: var(--magenta);
}

.inst .ar { flex: none; color: var(--dim); transition: .15s; }

/* 系统自检终端 */
/*
  自检终端块的外观在 `style.css` 的 `.scrn .term`（与注入模式选方式屏共用一份）。
  这里只留「它与上面那台机架隔多远」—— 那是本屏独有的。
*/
.term { margin: var(--g-term) 0 0; }
</style>
