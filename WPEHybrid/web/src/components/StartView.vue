<script setup lang="ts">
/*
  启动页（模式选择）—— 对应 WinForms 的 Forms/StartForm.cs。

  风格取自官网 WPEWeb.Cyber：glitch 标题、eyebrow、发丝分隔的卡片网格、终端自检块。
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
  <main class="start">
    <div class="eyebrow">
      <span class="dash" />
      <span class="lbl">{{ t('start.eyebrow') }}</span>
    </div>

    <h1 class="glitch" data-text="Winsock Packet Editor">Winsock Packet Editor</h1>

    <p class="subtitle">{{ typed }}<span class="cur" /></p>

    <div class="cards">
      <!--
        注入模式：IPC 改造之后可用了。
        与另外两张卡同构 —— 不带 aria-disabled / title，它们是「点了没反应」时才需要的。
      -->
      <div
        class="cd"
        role="button"
        tabindex="0"
        @click="enterInject"
        @keydown.enter.prevent="enterInject"
        @keydown.space.prevent="enterInject"
      >
        <div class="num">Mode 01</div>
        <div class="t">
          <!-- 芯片：外壳 + 内核 + 四面引脚。原来的图只有左右两侧有脚，更像一枚电池 -->
          <svg class="ico" viewBox="0 0 24 24">
            <rect x="5" y="5" width="14" height="14" rx="1.5" />
            <rect x="9.5" y="9.5" width="5" height="5" />
            <path d="M9 2v3M15 2v3M9 19v3M15 19v3M2 9h3M2 15h3M19 9h3M19 15h3" />
          </svg>
          Inject
        </div>
        <div class="zh">{{ t('start.inject.zh') }}</div>
        <p>{{ t('start.inject.desc') }}</p>
        <!-- 与另外两张同构：各显示一条本模式的实测数据。上次注入的目标进程 -->
        <div class="last">Target // <b>{{ sys?.lastInjection || '—' }}</b></div>
      </div>

      <!-- 代理模式：可用 -->
      <!--
        用 role="button" + tabindex 而不是真的 <button>：
        <button> 的内容模型只允许短语内容，而卡片里是 div/h3/p 这些块级元素，
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
        <div class="num">Mode 02</div>
        <div class="t">
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
          Proxy
        </div>
        <div class="zh">{{ t('start.proxy.zh') }}</div>
        <p>{{ t('start.proxy.desc') }}</p>
        <div class="last">Socks5 // <b>0.0.0.0:{{ sys?.socks5Port ?? '—' }}</b></div>
      </div>
    </div>

    <!--
      多开设置：贴在卡片网格<b>下沿</b>的一条窄入口（共用那圈发丝边，去掉上边框接上去）。

      它与上面两张是「同一件事的两级」——都在这一屏决定「这次怎么跑」，
      但它不是模式，所以不给它一张同款卡片：高度只有卡片的四分之一，
      一眼就读得出主次。

      这里可以用<b>真的 &lt;button&gt;</b>（上面两张卡不行）——
      一行里全是 svg 与 span，都是短语内容，合规范；卡片里是 div/h3/p，
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
.start {
  position: relative;
  z-index: 10;
  flex: 1;
  min-height: 0;
  display: flex;
  flex-direction: column;
  justify-content: center;
  padding: 0 56px;
  overflow: auto;
}

/* eyebrow：绿色短横 + 代号 */
.eyebrow { display: flex; align-items: center; gap: 10px; margin-bottom: 16px; }
.eyebrow .dash { width: 32px; height: 1px; background: var(--green); box-shadow: 0 0 6px var(--green); }
.eyebrow .lbl {
  font-family: var(--share);
  font-size: 10px;
  letter-spacing: .3em;
  text-transform: uppercase;
  color: var(--green);
}

/* glitch 标题：三层错位霓虹 */
.glitch {
  font-family: var(--orbit);
  font-weight: 900;
  text-transform: uppercase;
  letter-spacing: -.02em;
  font-size: clamp(32px, 3.6vw, 46px);
  line-height: 1;
  color: var(--green);
  position: relative;
  display: inline-block;
  align-self: flex-start;
  text-shadow: 0 0 26px rgb(var(--green-rgb) / 40%);
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

@keyframes g1 {
  0% { clip-path: inset(20% 0 60% 0); transform: translate(-2px); }
  100% { clip-path: inset(65% 0 8% 0); transform: translate(2px); }
}

@keyframes g2 {
  0% { clip-path: inset(70% 0 10% 0); transform: translate(2px); }
  100% { clip-path: inset(15% 0 70% 0); transform: translate(-2px); }
}

.subtitle {
  margin: 14px 0 0;
  font-family: var(--share);
  font-size: 13px;
  letter-spacing: .16em;
  text-transform: uppercase;
  color: var(--muted);
  min-height: 1.3em;
}

.subtitle .cur {
  display: inline-block;
  width: 8px;
  height: 1em;
  background: var(--green);
  box-shadow: 0 0 6px var(--green);
  vertical-align: -2px;
  margin-left: 3px;
  animation: cur 1s steps(1) infinite;
}

@keyframes cur { 50% { opacity: 0; } }

/*
  三张卡：gap 1px + 底色是 --border，做出发丝分隔线。
  这是官网 .cards 的招 —— 比给每张卡加 border 干净，相邻处不会变成 2px。
*/
.cards {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 1px;
  background: var(--border);
  border: 1px solid var(--border);
  margin: 34px 0 0;
}

.cd {
  position: relative;
  background: var(--card);
  padding: 24px 22px 22px;
  cursor: pointer;
  transition: background .15s;
}

.cd:hover { background: var(--panel); }

.cd::after {
  content: "→";
  position: absolute;
  right: 22px;
  bottom: 18px;
  color: var(--green);
  opacity: 0;
  transition: .15s;
}

.cd:hover::after { opacity: 1; right: 18px; }
.cd.cy::after { color: var(--cyan); }

/*
  焦点环画在<b>内侧</b>：卡片之间只有 1px 的发丝线，正偏移会压到邻居身上。
  顺带让 hover 的那个箭头在键盘聚焦时也出来 —— 两种操作方式给同样的提示。
*/
.cd:focus-visible { outline-offset: -2px; }
.cd:focus-visible::after { opacity: 1; right: 18px; }
.cd.cy:focus-visible { outline-color: var(--cyan); }

.cd .num {
  font-family: var(--share);
  font-size: 10px;
  letter-spacing: .18em;
  color: var(--muted);
  margin-bottom: 10px;
}

.cd .t {
  font-family: var(--orbit);
  font-size: 15px;
  text-transform: uppercase;
  letter-spacing: .06em;
  color: var(--green);
  margin-bottom: 10px;
  display: flex;
  align-items: center;
  gap: 9px;
}

.cd.cy .t { color: var(--cyan); }

/*
  多开设置那条窄入口。

  <b>去掉上边框</b>与卡片网格接在一起 —— 网格自己有一圈 1px 的边，
  这里再来一条就成了 2px 的粗线（.cards 用 gap:1px + 底色画分隔线，正是为了避开这个）。
  洋红沿用它当卡片时的色相：这一屏三个入口各有一个颜色，换了位置不该换身份。
*/
.inst {
  display: flex;
  align-items: center;
  gap: 10px;
  width: 100%;
  padding: 0 18px 0 20px;
  height: 40px;
  border: 1px solid var(--border);
  border-top: 0;
  background: var(--card);
  color: var(--muted);
  font-family: inherit;
  font-size: 13px;
  text-align: left;
  cursor: pointer;
  transition: .15s;
}

.inst:hover { background: var(--panel); }
.inst:hover .nm,
.inst:hover .ar { color: var(--magenta); }
/* 焦点环走内侧：它左右都贴着网格的边，正偏移会压到边框上 */
.inst:focus-visible { outline: 1px solid var(--magenta); outline-offset: -2px; }
.inst:focus-visible .ar { opacity: 1; }

.inst .ico { width: 15px; height: 15px; stroke: var(--magenta); flex: none; }
.inst .nm { color: var(--gray); flex: none; transition: .15s; }

/* 说明占满中间；窄屏或俄语这类长文案下截断，别把右边的实例名挤掉 */
.inst .ds {
  flex: 1;
  min-width: 0;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
  color: var(--dim4);
  font-size: 12.5px;
}

/* 当前实例名 —— 与卡片底部那行 last 同一个作用：这一屏的每个入口都显示一条实测值 */
.inst .cur {
  flex: none;
  font-family: var(--mono);
  font-size: 12px;
  color: var(--magenta);
}

.inst .ar { flex: none; color: var(--dim); transition: .15s; }

.cd .ico { width: 17px; height: 17px; }

.cd .zh { font-size: 13px; color: var(--gray); margin-bottom: 6px; }
.cd p { font-size: 13px; color: var(--muted); margin: 0; }

.cd .last {
  margin-top: 14px;
  padding-top: 12px;
  border-top: 1px solid var(--border);
  font-family: var(--share);
  font-size: 10px;
  letter-spacing: .14em;
  color: var(--muted);
}

.cd .last b { color: var(--cyan); font-weight: 400; }

/* 系统自检终端 */
.term { margin: 26px 0 0; background: var(--sink); border: 1px solid var(--border); }

.term-bar {
  display: flex;
  align-items: center;
  gap: 8px;
  padding: 8px 14px;
  background: var(--panel);
  border-bottom: 1px solid var(--border);
}

.term-bar .d { width: 10px; height: 10px; border-radius: 50%; }

.term-bar .lbl {
  margin-left: 8px;
  font-family: var(--share);
  font-size: 10px;
  letter-spacing: .16em;
  text-transform: uppercase;
  color: var(--muted);
}

.term-body {
  padding: 12px 16px;
  font-size: 12.5px;
  color: var(--soft);
  overflow-x: auto;
}

.term-body .l { display: block; white-space: pre; }
.term-body .g { color: var(--green); }
.term-body .c { color: var(--muted); }
.term-body .y { color: var(--cyan); }
.term-body .a { color: var(--amber); }
.term-body .r { color: var(--danger); }
</style>
