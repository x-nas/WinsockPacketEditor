<script setup lang="ts">
/*
  外壳的最外层：氛围层 + 自绘标题栏 + 视图切换 + 状态栏。

  窗口是无边框的（ShellForm 里 FormBorderStyle.None），所以标题栏、拖动、
  最小化/最大化/关闭都由这里负责：
    拖动 → 标题栏上的 CSS app-region: drag，由 WebView2 原生处理
    缩放 → 窗体留 3px 内边距，C# 侧 WndProc 处理 WM_NCHITTEST，前端不用管

  三个视图：启动页（模式选择）· 多开设置 · 代理模式。
  注入模式要等 IPC 改造完成才能进来。

  【选完模式就<b>回不去</b>了，与主程序一致】
  WinForms 的 StartForm 选完模式直接 this.Close()，启动页窗体就销毁了，
  换模式只能重启程序。外壳照搬这个模式，所以标题栏没有「返回启动页」。

  这不只是为了像：进代理模式会加载 14 份列表、设 SelectMode、并可能已经把
  SOCKS5 服务起起来了 —— 退回一个写着「Ready」的启动页，而后台正在监听端口，
  是在骗人。真要换模式，重启是最干净的。

  <b>多开设置不受这条限制</b>：它不是模式，是启动页上的一屏设置
  （WinForms 那边是浮在 StartForm 上的弹窗），所以它照常能返回。
*/
import { onMounted, ref, watchEffect } from 'vue'
import { call, inHost, on } from './bridge'
import { attachUiHost, busy, modalOpen } from './bridge/host'
import { anyModalOpen } from './useModal'
import { defOf, initLang, isEn, lang, t } from './i18n'
import { httpAddr, injectHooked, injectTarget, proxyRunning, socks5Addr } from './stores/runtime'
import { initTheme } from './stores/theme'
import StartView from './components/StartView.vue'
import ProxyView from './components/ProxyView.vue'
import InstanceView from './components/InstanceView.vue'
import InjectView from './components/InjectView.vue'
import BetaNotice from './components/BetaNotice.vue'
import EncryptPassword from './components/EncryptPassword.vue'
import ToastStack from './components/ToastStack.vue'
import BusyMask from './components/BusyMask.vue'
import ConfirmDialog from './components/ConfirmDialog.vue'
import AppSetting from './components/AppSetting.vue'

type View = 'start' | 'proxy' | 'instance' | 'inject'

const view = ref<View>('start')
const version = ref('')
const isBeta = ref(false)
const maximized = ref(false)

/** 数据库<b>目录</b>名 —— 多开时区分实例靠的是它，不是文件名（文件名由版本号推导）。 */
const dbInstance = ref('')

onMounted(async () => {
  if (!inHost) return

  attachUiHost()

  // 最大化状态由 C# 推 —— 用户也可能通过双击拖动区、贴边吸附改变它
  on('window:state', (d: { maximized: boolean }) => { maximized.value = d.maximized })

  try {
    const s = await call<any>('getSystemCheck')

    /*
      语言要在<b>任何界面文字画出来之前</b>定好，否则英文用户会先看见一帧中文再跳成英文。
      getSystemCheck 已经是首屏的第一次往返，顺便把 language 捎回来，
      不为它单开一次 getPrefs。
    */
    initLang(s.language)

    /*
      主题与语言同一条理由：要在任何像素画出来之前定好。

      两个值一起给：themeMode 是用户选的那一档（深 / 浅 / 跟随系统），
      isDark 是上次解析出来的实际值 —— 只在「跟随系统」而 matchMedia
      又不可用时才兜底。认不出来一律深色（这套皮肤照深色设计），
      所以桥没接上（探针页）时保持深色、不会闪。
    */
    initTheme(s.themeMode, s.isDark, s.scanLine)

    version.value = s.version
    isBeta.value = s.isBeta
    dbInstance.value = s.dbInstance || ''
    socks5Addr.value = s.socks5Addr || ''
    //空串 = HTTP 代理没启用，不是取不到
    httpAddr.value = s.httpAddr || ''
    nativeDrag.value = s.nativeDrag !== false
  } catch (e) {
    console.error('[app] 取自检信息失败', e)
  }

  /*
    告诉 C# 前端已经挂好了。

    必须由前端主动报，而不是让 C# 用 NavigationCompleted 判断 —— 那个事件只说明
    文档到了，Vue 还没 mount，此时 C# 发 ask 没人接，只能干等到 5 分钟超时。
    BetaNotice 的 registerForm 在它自己的 onMounted 里跑，而子组件的 onMounted
    先于父组件，所以走到这一行时处理器一定已经登记好了。
  */
  call('uiReady').catch(() => {})
})

/*
  拖动。

  首选让 WebView2 原生处理：标题栏加 CSS `app-region: drag`，
  由浏览器进程发起系统拖动（需要 Runtime 121+，C# 侧开 IsNonClientRegionSupportEnabled）。

  只有原生支持不可用时才回落到 startDragWindow —— 那条路是 C# 侧
  SendMessage(WM_NCLBUTTONDOWN)，系统会抢走鼠标捕获，Chromium 收不到 mouseup，
  拖完会在光标处补派发一次 click。所以下面的按钮还有一道独立防线。
*/
const nativeDrag = ref(true)

function onTitlebarMouseDown(e: MouseEvent): void {
  if (nativeDrag.value) return
  if (e.button !== 0) return
  if ((e.target as HTMLElement).closest('button')) return
  call('startDragWindow').catch(() => {})
}

/*
  窗口按钮的点击防线。

  只认「在这个按钮上按下、又在这个按钮上松开」的点击。
  凭空出现的 click（比如回落拖动结束后 Chromium 补派发的那次）没有配对的
  mousedown，会被丢掉 —— 否则它落在「退出」上就直接把程序关了。
*/
const armed = ref<HTMLElement | null>(null)

/*
  ── 语言下拉 ───────────────────────────────────────────────

  这里原先是<b>语言</b>下拉（更早还是「点一下切一种」的两态开关）。
  加了深浅色之后一个下拉装不下两组选项，而把两个 chip 并排摆在标题栏又太挤
  —— 右边紧挨着四个窗口按钮。所以收成一个齿轮按钮 + 一屏「软件设置」，
  语言与主题都在里面，标题栏反而空了一格。

  为什么不并进代理模式那 12 个设置：那些管的是抓包行为、只在模式里有意义，
  而语言与主题在启动页上就要能改，且两种模式共用。见 AppSetting.vue 的说明。
*/
const appSetOpen = ref(false)

function armBtn(e: MouseEvent): void {
  armed.value = e.button === 0 ? (e.currentTarget as HTMLElement) : null
}

function fireBtn(e: MouseEvent, run: () => void): void {
  const el = e.currentTarget as HTMLElement
  const ok = armed.value === el
  armed.value = null

  if (!ok) {
    console.warn('[app] 丢弃一次没有配对 mousedown 的窗口按钮点击')
    return
  }

  run()
}

async function toggleMax(): Promise<void> {
  try {
    const r = await call<{ maximized: boolean }>('toggleMaximize')
    maximized.value = r.maximized
  } catch {
    /* 忽略 */
  }
}

/*
  窗口保持最前。对应 WinForms 里 ProxyList 工具条上的「窗口保持最前」勾选框。

  <b>状态以 C# 回的为准</b>，不是本地先翻再发：置顶是 Windows 说了算的，
  极少数情况下（全屏独占的程序）设了也不生效，那时按钮该照实显示。
  同样<b>不持久化</b> —— WinForms 那边也只是个运行期的窗体属性，重启回到不置顶。
*/
const topMost = ref(false)

async function toggleTopMost(): Promise<void> {
  try {
    const r = await call<{ topMost: boolean }>('setTopMost', { on: !topMost.value })
    topMost.value = !!r.topMost
  } catch {
    /* 忽略 */
  }
}

function open(url: string): void {
  call('openExternal', { url }).catch(() => {})
}

/**
 * 官网页面地址，按当前语言分流。
 *
 * 官网 15 个中文页在根目录、同名英文页在 en/ 下（见 WPEWeb 第 4.8 节），
 * 所以只是加个前缀。首页不走这里 —— 那条链接指的是站点本身。
 */
function site(page: string): string {
  return 'https://www.wpe64.com/' + (isEn.value ? 'en/' : '') + page
}
/*
  两个跟语言走的尺寸令牌，写在 <b>:root</b> 上而不是 .win 上。

    --side-w   侧栏宽度。不加宽的话「Извлечение данных」这类词全靠省略号截断，
               一屏四五处 …，很难扫。
    --setf-kx  设置弹窗标签列的乘数。那几个宽度是按中文字数定的，
               俄语标签会在列里折成两行、行高参差不齐。

  ⚠️ <b>必须是 :root，不能挂在 .win 上</b>：弹窗现在一律 Teleport 到 body
  （见 useModal.ts 里那段），挂在 .win 上的话它们就在继承链之外，
  --setf-kx 会退回默认的 1 —— 表现是俄语 / 越南语下设置弹窗的标签列突然变窄。
*/
watchEffect(() => {
  const r = document.documentElement.style
  const wide = defOf(lang.value).wide
  r.setProperty('--side-w', wide ? '228px' : '196px')
  r.setProperty('--setf-kx', wide ? '1.3' : '1')
})
</script>

<template>
  <!--
    两个跟语言走的尺寸令牌写在 :root 上（见 script 里那段 watchEffect）—— 弹窗一律 Teleport 到 body，挂在 .win 上的话它们就在继承链之外。

      --side-w   侧栏宽度。不加宽的话「Извлечение данных」这类词全靠省略号截断，
                 一屏四五处 …，很难扫。
      --setf-kx  设置弹窗标签列的乘数。那几个宽度是按中文字数定的，
                 俄语标签会在列里折成两行、行高参差不齐。
  -->
  <div class="win">
    <!--
      氛围层：网格底纹 + 游走亮带（四角标记在最上层，见文件末尾）。

      曾经还有一层 .crt（固定的 4px 横纹 + RGB 分色），已去掉 —— 对比之后
      横纹压在正文上会让小字发糊，而它对氛围的贡献不如这条游走亮带。
    -->
    <div class="pcb" />
    <div class="scan" />

    <!--
      模态弹窗打开时，除弹窗以外的整块置 inert：
      Tab 走不进去、鼠标也点不动。不这么做的话焦点能从弹窗跳到标题栏的退出按钮上，
      回车就把程序关了 —— 而 C# 那边还在 await 这个弹窗的答案。
      窗口真要强制关闭还有 Alt+F4（那走的是窗体，不经过页面）。
    -->
    <div class="shell" :inert="modalOpen || anyModalOpen">
      <header class="titlebar" @mousedown="onTitlebarMouseDown">
        <div class="brand">
          <!-- 与官网侧栏 logo 逐项对齐：文本 WPE x64、紧字距、x64 更小更细且灰（cyber.css 的 .sb-logo）-->
          <span class="bname">WPE <small>x64</small></span>
          <span class="bver">V {{ version || '—' }}</span>
          <span v-if="isBeta" class="tag">Beta</span>
        </div>

        <div class="tbright">
          <!--
            软件设置（语言 + 主题）。

            <b>只有齿轮，没有文字标签。</b>这颗按钮曾经是语言 chip，改成齿轮之后
            还留着那个两字母标签（CN / EN / JA…），但那时它已经名不副实了：
            按钮打开的是「软件设置」，里面装着语言<b>和</b>主题，只把语言摆在外面
            既说不全、也让人以为这仍是一颗语言按钮。
            而且它本来就多余 —— 整个界面就是那种语言，标签没多说任何事。

            aria-haspopup 用 dialog 不用 true：true 的语义是「弹菜单」，
            这里弹的是模态弹窗，读屏软件报的词不一样。
          -->
          <button class="wb gear" :class="{ on: appSetOpen }" :title="t('set.app')"
                  aria-haspopup="dialog" :aria-expanded="appSetOpen"
                  @mousedown="armBtn" @click="fireBtn($event, () => { appSetOpen = true })">
            <!--
              画布是 32 不是 24 —— 见下面 .wb.gear .ico 的说明。
              图形仍然以 (12,12) 为心，正好是这个画布的中心。
            -->
            <svg class="ico" viewBox="-4 -4 32 32">
              <circle cx="12" cy="12" r="3.2" />
              <path d="M19.4 15a1.6 1.6 0 0 0 .3 1.8l.1.1a2 2 0 1 1-2.8 2.8l-.1-.1a1.6 1.6 0 0 0-1.8-.3 1.6 1.6 0 0 0-1 1.5V21a2 2 0 1 1-4 0v-.1A1.6 1.6 0 0 0 9 19.4a1.6 1.6 0 0 0-1.8.3l-.1.1a2 2 0 1 1-2.8-2.8l.1-.1a1.6 1.6 0 0 0 .3-1.8 1.6 1.6 0 0 0-1.5-1H3a2 2 0 1 1 0-4h.1A1.6 1.6 0 0 0 4.6 9a1.6 1.6 0 0 0-.3-1.8l-.1-.1a2 2 0 1 1 2.8-2.8l.1.1a1.6 1.6 0 0 0 1.8.3H9a1.6 1.6 0 0 0 1-1.5V3a2 2 0 1 1 4 0v.1a1.6 1.6 0 0 0 1 1.5 1.6 1.6 0 0 0 1.8-.3l.1-.1a2 2 0 1 1 2.8 2.8l-.1.1a1.6 1.6 0 0 0-.3 1.8V9a1.6 1.6 0 0 0 1.5 1H21a2 2 0 1 1 0 4h-.1a1.6 1.6 0 0 0-1.5 1z" />
            </svg>
          </button>

          <!--
            窗口保持最前。WinForms 把它放在「代理数据」页的工具条上，这里提到标题栏 ——
            它改的是<b>窗口</b>的属性，不属于某一页；而且那条工具条已经很挤了。
            按下去时图钉转成竖直并点亮，与勾选框那套「选中即变绿」是同一条视觉语言。
          -->
          <button class="wb pin" :class="{ on: topMost }" :title="t(topMost ? 'win.unpin' : 'win.pin')"
                  :aria-pressed="topMost"
                  @mousedown="armBtn" @click="fireBtn($event, toggleTopMost)">
            <svg class="ico" viewBox="0 0 24 24">
              <path d="M9 4h6M10 4v6l-3 3v2h10v-2l-3-3V4M12 15v5" />
            </svg>
          </button>

          <span class="tbsep" />

          <!--
            ⚠️ 最小化 / 最大化 / 关闭这三个<b>不给悬停提示</b>：右上角这三个图标是所有人
            都不用学的东西，弹一句「最小化」纯属打扰。旁边的齿轮与图钉<b>保留 title</b> ——
            「软件设置」「窗口保持最前」并不是一眼就懂的。

            ⚠️ <b>但不能直接把 title 删掉，要换成 aria-label。</b>
            它们是纯图标按钮（里面只有一个 svg），title 原本是它们<b>唯一的可访问名</b> ——
            删了读屏就只会念「按钮」。tooltip.ts 摘 title 时正是这么补的（见那一节），
            这里等于把它要做的事提前写好。
            aria-label 不进 tooltip.ts 的取值（它只认 title 与 data-tip），所以不会又弹出来。
          -->
          <button class="wb" :aria-label="t('win.min')"
                  @mousedown="armBtn" @click="fireBtn($event, () => call('minimizeWindow'))">
            <svg class="ico" viewBox="0 0 24 24"><path d="M5 12h14" /></svg>
          </button>

          <button class="wb" :aria-label="maximized ? t('win.restore') : t('win.max')"
                  @mousedown="armBtn" @click="fireBtn($event, toggleMax)">
            <svg v-if="!maximized" class="ico" viewBox="0 0 24 24"><rect x="4" y="4" width="16" height="16" rx="1" /></svg>
            <svg v-else class="ico" viewBox="0 0 24 24"><rect x="4" y="7" width="13" height="13" rx="1" /><path d="M8 7V4h12v12h-3" /></svg>
          </button>

          <button class="wb close" :aria-label="t('win.close')"
                  @mousedown="armBtn" @click="fireBtn($event, () => call('closeWindow'))">
            <svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg>
          </button>
        </div>
      </header>

      <div v-if="!inHost" class="nohost">
        {{ t('win.nohost') }}
      </div>

      <StartView v-else-if="view === 'start'" @enter="view = $event" />

      <!--
        多开设置。用 v-if 而不是 keep-alive：每次进来都该重新读一次当前库位置
        （用户可能刚在别处切过），组件重挂载正好把这件事做了。
      -->
      <InstanceView v-else-if="view === 'instance'" @back="view = 'start'" />

      <!--
        注入模式。与代理模式一样<b>进去就回不来</b> ——
        进来会附加到目标、装钩子，退回一个写着「Ready」的启动页而钩子还在目标里，是在骗人。
      -->
      <InjectView v-else-if="view === 'inject'" />

      <ProxyView v-else />

      <footer class="statusbar">
        <div class="sb-left">
          <a @click="open('https://www.wpe64.com')">© 2026 Winsock Packet Editor</a>
          <!--
            官网的英文站与中文站是同名页分居 en/ 下，链接跟着语言走，
            否则英文界面点进去看到的是中文页。
          -->
          <a @click="open(site('tutorial.html'))">{{ t('foot.tutorial') }}</a>
          <a @click="open(site('faq.html'))">{{ t('foot.faq') }}</a>
          <a @click="open('https://github.com/x-nas/WinsockPacketEditor')">GitHub</a>
        </div>
        <!--
          右侧放代理服务的监听地址与运行状态 —— 那是代理模式下唯一需要随时能看到、
          又不在指标板里的东西（指标板全是抓包管线的数字）。
          原程序启动代理后打的第一条日志也正是这个地址。
        -->
        <div class="sb-right">
          <template v-if="view === 'start'">
            <span class="dot" />
            {{ t('foot.ready') }}
          </template>
          <template v-else-if="view === 'instance'">
            <span class="dot mg" />
            Instance <span class="addr">{{ dbInstance || '—' }}</span>
          </template>
          <template v-else-if="view === 'inject'">
            <span class="dot" :class="{ off: !injectHooked }" />
            Inject <span class="addr">{{ injectTarget || '—' }}</span>
            <span class="sep">//</span>
            <span :class="injectHooked ? 'on' : 'off-t'">
              {{ injectHooked ? t('foot.hooking') : t('foot.stopped') }}
            </span>
          </template>
          <template v-else>
            <span class="dot" :class="{ off: !proxyRunning }" />
            Socks5 <span class="addr">{{ socks5Addr || '—' }}</span>
            <span class="sep">//</span>
            <span :class="proxyRunning ? 'on' : 'off-t'">
              {{ proxyRunning ? t('foot.running') : t('foot.stopped') }}
            </span>
          </template>
        </div>
      </footer>
    </div>

    <!-- 软件设置（语言 + 主题）。挂在 .shell 之外：它自己就是那个把外面置 inert 的弹窗 -->
    <AppSetting v-model:open="appSetOpen" />

    <BetaNotice />

    <!-- 加密导入 / 导出的密码框。同样在 .shell 之外 —— 它自己就是弹窗 -->
    <EncryptPassword />

    <!-- 同样在 .shell 之外：它自己就是那个把外面置 inert 的弹窗 -->
    <ConfirmDialog />

    <!-- 放在 .shell 之外：弹窗把 .shell 整块置 inert 时，提示仍要看得见、点得掉 -->
    <ToastStack />

    <BusyMask v-if="busy.on" :text="busy.text" />

    <!-- 四角标记压在最上层（含弹窗之上，见 style.css 的 --z-ambience） -->
    <span class="cn tl" /><span class="cn tr" /><span class="cn bl" /><span class="cn br" />
  </div>
</template>

<style scoped>
.win {
  position: relative;
  height: 100vh;
  display: flex;
  flex-direction: column;
  background: var(--black);
  overflow: hidden;
}

/* 纵向布局从 .win 移到这里 —— 它要能被整体 inert，所以必须是一个能包住三段的容器 */
.shell {
  position: relative;
  z-index: 10;
  flex: 1;
  min-height: 0;
  display: flex;
  flex-direction: column;
}

/* ── 标题栏 ─────────────────────────────────────────────── */
.titlebar {
  position: relative;
  z-index: 20;
  height: 46px;
  flex: none;
  display: flex;
  align-items: center;
  justify-content: space-between;
  /* 左 28px 给左上角标的横臂让位；右 8px 让右上角标不贴着关闭按钮 */
  padding: 0 8px 0 28px;
  background: rgb(var(--chrome-rgb) / 80%);
  backdrop-filter: blur(8px);
  border-bottom: 1px solid var(--border);
  user-select: none;

  /*
    交给 WebView2 原生处理拖动（IsNonClientRegionSupportEnabled）。
    -webkit- 前缀那条是给旧内核的别名，两条都写不会冲突。
  */
  -webkit-app-region: drag;
  app-region: drag;
}

/* 按钮必须排除，否则点不动 */
.titlebar button {
  -webkit-app-region: no-drag;
  app-region: no-drag;
}

.brand { display: flex; align-items: center; gap: 10px; }

.bname {
  font-family: var(--orbit);
  font-weight: 800;
  font-size: 16px;   /* 14 → 16；x64 那个 <small> 不写字号，按比例跟着长到 12.8px */
  /* 顶部 1px：WPE 与 x64 按基线对齐，大的那个字形中心比小的高 1px，整体下移 0.5 让两者各偏半像素 */
  padding-top: 1px;
  /* 与官网 .sb-logo .name 一致的紧字距（原来是 .1em，标题栏自己放松过） */
  letter-spacing: -.02em;
  color: var(--green);
}

/*
  官网把型号那截压成次要信息：更小、更细、灰色，与前面的绿色主名分层。

  <b>不要写 font-size</b> —— 官网的 .sb-logo .name small 也没写，
  靠的是 <small> 自带的 UA 默认 font-size: smaller（约 0.83×）。
  之前这里写了 font-size: inherit，把那个默认覆盖掉了，两截才变得一样大。
*/
.bname small {
  font-weight: 500;
  color: var(--muted);
}

.bver {
  font-family: var(--share);
  font-size: var(--label-size);
  /* 行高收到 1 + 顶部 2px：默认行高把行距压在字下面、字形本身又偏上，实测两者合计高 2.4px；顶部 3px 让盒子长 3、居中后内容下移 1.5，正好补回 */
  line-height: 1;
  padding-top: 0;   /* 原 3px 是给字形偏上的补偿，字体度量覆写之后偏低 1.5px（2026-09-13 字体度量覆写后按 100% 缩放实测重调） */
  letter-spacing: .16em;
  color: var(--muted);
  border-left: 1px solid var(--border);
  padding-left: 10px;
}

.tbright { display: flex; align-items: center; height: 46px; }

.tag {
  font-family: var(--share);
  font-size: var(--label-size);
  letter-spacing: .18em;
  text-transform: uppercase;
  color: var(--amber);
  border: 1px solid rgb(var(--amber-rgb) / 40%);
  padding: 3px 8px 3px;   /* 上 +1 下 -1：字形在 em 框里偏上 1px（与按钮同一处理）*/
  /* 原来挂在标题栏右侧、靠 margin-right 与窗口按钮拉开；
     现在跟在版本号后面，间距归 .brand 的 gap 管 */
  line-height: 1;
}

.wb {
  width: 44px;
  height: 46px;
  border: 0;
  background: transparent;
  color: var(--muted);
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: .15s;
}

.wb:hover { color: var(--green); background: rgb(var(--green-rgb) / 8%); }
.wb.close:hover { color: var(--danger); background: rgb(var(--danger-rgb) / 12%); }

/*
  窗口保持最前。这是这一排里唯一有「开 / 关」两态的按钮，所以按下去要看得出来 ——
  琥珀 + 一层底色，与勾选框「选中即变绿」是同一条视觉语言，只是换个色相：
  绿已经是整排按钮的 hover 色，用绿的话开着和只是划过分不出来。

  图钉<b>斜着</b>是未固定、<b>立起来</b>是已固定，与各家的图钉按钮一致。
  这里用 rotate 做，图标只画一个。
*/
.wb.pin .ico { transform: rotate(-35deg); transition: transform .15s; }
.wb.pin.on .ico { transform: none; }
.wb.pin.on { color: var(--amber); background: rgb(var(--amber-rgb) / 12%); }
.wb.pin.on:hover { color: var(--amber); background: rgb(var(--amber-rgb) / 20%); }
.wb.pin:focus-visible { outline-color: var(--amber); }

/*
  设置齿轮。尺寸与其余几个窗口按钮完全一致（.wb 的 44 x 46），只是换个色相：
  <b>青色</b>把它与右边那三个窗口控制分开 —— 那三个的 hover 是绿的，
  再配上中间那条分隔线，一眼能读出「这颗不是窗口控制」。

  以前它是宽按钮（图标 + 两字母语言标签），标签去掉之后自然回到方形，
  连带那条「定宽免得切语言时整排按钮平移」的讲究也不需要了。
*/
/*
  ⚠️ 齿轮<b>只有这一颗</b>要改画布与描边宽，理由是它的墨迹顶满了 24 的画布。

  五颗按钮的 .ico 容器本来就一致（18x18 / viewBox 24 / stroke-width 2），
  但<b>墨迹</b>差很多 —— 实测（含描边，user 单位）：
    齿轮 24x24   最大化 18x18   退出 14x14   图钉 12x18
  齿轮比最大化宽 33%、比退出宽 71%，摆在一排里就是它一个显得胖。
  （这是 Lucide 那套图标的常态：同一格 24 的画布，各个字形占的比例本来就不同。）

  做法是<b>把画布放大到 32</b>，同一条路径就画小了：24 x 18/32 = 13.5px，
  与最大化那个方框逐像素相等。代价是描边也跟着缩到 1.125px，
  所以这里把 stroke-width 补成 2 x 32/24 = 2.667，渲染出来仍是 1.5px ——
  与另外四颗一致。<b>改画布就要同时改这个数，两者是一对。</b>

  不用 transform: scale() 是因为那样描边也会被缩，得再加 vector-effect 去抵消，
  绕的圈更多；也不用直接把 .ico 改小，那同样会连描边一起缩。
*/
.wb.gear .ico { stroke: var(--cyan); stroke-width: 2.667; }
.wb.gear:hover { color: var(--cyan); background: rgb(var(--cyan-rgb) / 8%); }
.wb.gear:hover .ico { stroke: var(--cyan); }
.wb.gear:focus-visible { outline-color: var(--cyan); }

/* 弹窗开着时按钮保持高亮，否则鼠标一移开就看不出是谁弹的 */
.wb.gear.on { color: var(--cyan); background: rgb(var(--cyan-rgb) / 8%); }

/*
  分隔线。左边两个（设置齿轮、窗口置顶）点了不会让窗口消失，
  右边三个（最小化 / 最大化 / 退出）会 —— 别读成一组，尤其别让「退出」显得和它们同类。
*/
.tbsep {
  width: 1px;
  height: 16px;
  margin: 0 6px;
  background: var(--border);
}

/*
  焦点环改成<b>内侧</b>偏移。

  这排按钮和标题栏等高（46px），上边就是窗口的上边；最右那个还贴着窗口右沿。
  全局焦点环用的是 outline-offset: 2px（画在盒子外面），那条上边线落在 y = -2…0，
  跑到窗口外被 .win 的 overflow:hidden 裁掉 —— 表现成「焦点环少一条边」。
  负偏移画在按钮内侧，四条边都在可见区域里。
*/
.wb:focus-visible { outline-offset: -2px; }

/* 关闭按钮的焦点环跟着它的语义走 */
.wb.close:focus-visible { outline-color: var(--danger); }

/* ── 状态栏 ─────────────────────────────────────────────── */
.statusbar {
  position: relative;
  z-index: 20;
  height: 30px;
  flex: none;
  display: flex;
  align-items: center;
  justify-content: space-between;
  /* 与标题栏同理，给左下/右下角标的横臂让位 */
  padding: 1px 28px 0;   /* 顶部 1px：整行字形实测高 0.75px，这样把它压回中线 */
  background: rgb(var(--chrome-rgb) / 90%);
  border-top: 1px solid var(--border);
  font-family: var(--share);
  font-size: var(--label-size);
  /*
    行高收到 1：状态栏只有 30px 高，继承 body 的 1.7 行高会让行盒撑到 17px、
    上下各只剩 6px，底部两个角标怎么躲都会探进这条带。收紧后行盒贴住字形，
    上下各让出约 10px。
  */
  line-height: 1;
  letter-spacing: .14em;
  text-transform: uppercase;
  color: var(--muted);
}

.sb-left { display: flex; align-items: center; gap: 20px; }
.sb-left a { color: var(--muted); text-decoration: none; cursor: pointer; }
.sb-left a:hover { color: var(--green); }

.sb-right { display: flex; align-items: center; gap: 8px; }
.sb-right .addr { color: var(--cyan); }
.sb-right .sep { color: var(--border); }
.sb-right .on { color: var(--green); }
.sb-right .off-t { color: var(--muted); }

.dot { width: 7px; margin-top: -1px;   /* 状态栏顶部补了 1px 内边距，圆点不受字形偏移影响、要退回去 */ height: 7px; background: var(--green); box-shadow: 0 0 6px var(--green); }

/* 多开设置那一屏跟着它自己的主色走 */
.dot.mg { background: var(--magenta); box-shadow: 0 0 6px var(--magenta); }

/* 服务没跑时熄灯：一眼能看出来，不用去读文字 */
.dot.off { background: var(--muted); box-shadow: none; }

.nohost {
  flex: 1;
  display: flex;
  align-items: center;
  justify-content: center;
  color: var(--muted);
}

</style>
