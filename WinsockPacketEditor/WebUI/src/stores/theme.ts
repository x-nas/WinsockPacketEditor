import { computed, ref } from 'vue'
import { call } from '../bridge'

/*
  主题：深色 / 浅色 / 跟随系统。

  【真源在 C#】用的是已有的 UI.Prefs.IsDark ＋ 新加的 UI.Prefs.FollowSystemTheme。
  前者是 WinForms 侧顶栏那个暗色开关存的东西（SystemConfig 表，备份 XML 里也带着），
  外壳另起一份的话，两套 UI 会各记各的 —— 用户在 WinForms 里切了、进外壳又变回去，
  而且备份导入之后两边对不上。

  【为什么是两个字段而不是一个枚举】IsDark 是 bool，装不下三态；
  改成枚举则 WinForms 侧每个读 IsDark 的地方都要跟着改。
  所以 IsDark 保持「<b>解析后的实际主题</b>」这个语义（跟随系统时存的是那一刻
  系统给出的值），另出一个布尔说明「这个值是不是跟着系统走出来的」。
  好处是 AntdUI 那半边完全不用知道有第三档存在。

  【浅色怎么实现】不是反相，是同一套设计换一组令牌值 ——
  版式 / 间距 / 边框 / 四角标记 / 网格底纹 / 游走亮带全部保留，只有颜色换掉。
  规则都在 style.css 的 :root[data-theme="light"] 里。

  ⚠️ <b>属性写在 <html> 上，不是 .win 上。</b>右键菜单、提示浮层、确认框
  都是 Teleport 到 body 的，挂在 .win 上它们拿不到令牌，
  会出现「页面浅色、右键菜单还是深色」。
*/
export type Theme = 'dark' | 'light' | 'system'

/** 用户<b>选的</b>那一档（三态）。界面上打勾的是它。 */
export const theme = ref<Theme>('dark')

/*
  氛围层那条<b>缓慢上下游走的亮带</b>（`.scan`，10 秒一趟）开不开。

  【为什么放在 theme.ts】它和主题是同一类东西 —— 「这个程序长什么样」，
  同一个真源（C# 的 UI.Prefs）、同一条桥（setAppearance 的稀疏报文）、
  同一个入口（标题栏齿轮）。单开一个 store 只会多一处要对齐的地方。

  【为什么不用 localStorage】偏好的唯一真源是 UI.Prefs：它要落库、要进备份、
  多开切库时要跟着换。localStorage 那份在这三件事上都对不上。
*/
export const scanLine = ref(true)

/**
 * 系统此刻是不是深色。
 *
 * WebView2 的 PreferredColorScheme 默认是 Auto，会跟着操作系统走，
 * 所以 prefers-color-scheme 在外壳里与在浏览器里一样可信。
 */
const _systemDark = ref(true)

/**
 * 系统<b>此刻</b>是不是深色。只读。
 *
 * 与 <see cref="effective"/> 不是一回事：那个是「现在实际生效的主题」，
 * 而这个不管用户选了哪一档，说的都是系统那边的事。
 * 设置弹窗里「跟随系统」那一档的提示要的是后者 —— 改成按「保存」才生效之后，
 * 草稿选了跟随系统但还没保存时，effective 还停在旧主题上，拿它去显示就是错的。
 */
export const systemIsDark = computed(() => _systemDark.value)

/**
 * <b>实际</b>生效的主题（两态）—— 界面按它渲染，推给 C# 的也是它。
 *
 * 「跟随系统」只是一种选择方式，落到像素上仍然只有深浅两种。
 */
export const effective = computed<'dark' | 'light'>(() => {
  if (theme.value === 'system') return _systemDark.value ? 'dark' : 'light'
  return theme.value
})

/*
  matchMedia 只建一次。
  每次进设置页都新建一个的话，监听会越挂越多，切一次主题跑 N 遍。
*/
let mq: MediaQueryList | null = null
let listening = false

function readSystem(): boolean {
  try {
    if (!mq) mq = window.matchMedia('(prefers-color-scheme: dark)')
    return mq.matches
  } catch {
    //拿不到就当深色 —— 这套皮肤是照深色设计的，深色是默认
    return true
  }
}

/*
  ⚠️ <b>开着不写属性、关掉才写</b>，与主题那条同一个道理：
  `style.css` 里 `.scan` 的默认就是开着的，两边都写等于维护两个入口。
*/
function applyScan(): void {
  try {
    if (scanLine.value) document.documentElement.removeAttribute('data-scan')
    else document.documentElement.setAttribute('data-scan', 'off')
  } catch {
    /* 非浏览器环境，忽略 */
  }
}

/**
 * 开 / 关游走亮带。
 *
 * 与 setTheme 同一条路数：先改本地（同步、界面立刻变），再推 C#；
 * 落库失败也只是「这次没记住」，不该让界面卡在旧状态上。
 */
export async function setScan(on: boolean): Promise<void> {
  //无条件同步一次：ref 与 <html> 上的属性万一对不上（备份导入、别处改了 DOM），
  //点当前这一项永远修不回来 —— 与 setTheme 里那条告诫是同一个坑
  applyScan()

  if (on === scanLine.value) return

  scanLine.value = on
  applyScan()

  try {
    //稀疏报文：只送变了的这一项，别把 mode / isDark 一起送
    await call('setAppearance', { scan: on })
  } catch (e) {
    console.error('[theme] 扫描线开关未能写回 C#，本次切换不会被记住', e)
  }
}

function applyDocumentTheme(): void {
  try {
    /*
      深色不写属性、浅色才写 —— :root 上那份定义本来就是深色，
      两边都写等于要维护两个入口。
    */
    if (effective.value === 'light') document.documentElement.setAttribute('data-theme', 'light')
    else document.documentElement.removeAttribute('data-theme')
  } catch {
    /* 非浏览器环境，忽略 */
  }
}

/**
 * 系统主题变了。
 *
 * <b>只有「跟随系统」那一档才理它</b> —— 用户明确选了深色 / 浅色时，
 * 系统怎么变都不该动他的选择。
 *
 * 除了改自己的界面，还要把新解析出来的值推回 C#：同一个进程里
 * UiDialogs 的几个原生弹窗是 AntdUI 画的，读的是 UI.Prefs.IsDark，
 * 不推的话它们会停在旧主题上。
 */
function onSystemChange(): void {
  const next = readSystem()
  if (next === _systemDark.value) return

  _systemDark.value = next
  if (theme.value !== 'system') return

  applyDocumentTheme()
  //只送 isDark，不送 mode —— 用户选的那一档没变（桥那边是「字段出现才改」）
  call('setAppearance', { isDark: next }).catch(() => { /* 存不上不影响本次会话 */ })
}

function listen(): void {
  /*
    只挂一次。initTheme 不止启动那一次会调 —— 备份导入之后也要调一次（那边
    也是「只应用、不回写」）。addEventListener 传同一个函数引用本来就会去重，
    但下面那条老式 addListener 不会，靠这个标记一并管住。
  */
  if (listening) return

  try {
    if (!mq) mq = window.matchMedia('(prefers-color-scheme: dark)')
    //addEventListener 是新写法，addListener 是 Safari 14 之前的；WebView2 走前者
    if (typeof mq.addEventListener === 'function') mq.addEventListener('change', onSystemChange)
    else if (typeof (mq as any).addListener === 'function') (mq as any).addListener(onSystemChange)

    listening = true
  } catch {
    /* 拿不到 matchMedia 就退化成「跟随系统 = 启动时那一次的值」 */
  }
}

/**
 * 用 C# 给的初值设定主题。启动时调一次，不回写 —— 值就是从那边来的。
 *
 * @param mode   用户选的那一档。认不出来一律深色（这套皮肤照深色设计，深色是默认）
 * @param isDark 上次解析出来的实际值。<b>只在「跟随系统」而 matchMedia 又不可用时</b>
 *               才派上用场；能读到系统就以系统为准。
 */
export function initTheme(
  mode: string | undefined | null,
  isDark?: boolean | null,
  scan?: boolean | null,
): void {
  //认不出来就当开着 —— 它是这套皮肤的一部分，默认状态是开
  scanLine.value = scan !== false
  applyScan()

  theme.value = mode === 'light' || mode === 'system' || mode === 'dark' ? mode : 'dark'

  _systemDark.value = mq || typeof window.matchMedia === 'function'
    ? readSystem()
    : isDark !== false

  applyDocumentTheme()
  listen()

  /*
    「跟随系统」时，C# 手里的 isDark 是<b>上次</b>解析出来的值 —— 程序关着的时候系统换了深浅，
    这里解析出来的就和它对不上。那一份不只是存档：窗体四周那圈缩放内边距露的是窗体底色，
    它按 isDark 取色，不对齐的话页面是浅的、四周却是一道黑框。
    只在确实对不上时送一次，且只送 isDark（稀疏报文，别把 mode 一起送）。
  */
  if (theme.value === 'system' && typeof isDark === 'boolean' && isDark !== _systemDark.value) {
    call('setAppearance', { isDark: _systemDark.value }).catch(() => { /* 存不上不影响本次会话 */ })
  }
}

/**
 * 切主题。
 *
 * 先改本地再推 C#：本地这一步是同步的，界面立刻就变；
 * 落库那一步万一失败也只是「这次没记住」，不该让界面卡在旧主题上。
 * 与 setLang 同一条路数。
 */
export async function setTheme(next: Theme): Promise<void> {
  /*
    ⚠️ 属性<b>无条件</b>同步一次，再判要不要往下走。
    先判等再 apply 的话，一旦 ref 与 <html> 上的属性对不上（备份导入、
    或别处直接改了 DOM），点当前这一项永远修不回来 —— 而那正是用户
    会去点的那一下。同步一次几乎不要钱。
  */
  applyDocumentTheme()

  if (next === theme.value) return

  //切进「跟随系统」时先把系统值读新，否则会拿上一次监听到的旧值算 effective
  if (next === 'system') _systemDark.value = readSystem()

  theme.value = next
  applyDocumentTheme()

  try {
    //mode 与解析后的 isDark 一起送：前者是用户的选择，后者是 AntdUI 那半边要的值
    await call('setAppearance', { mode: next, isDark: effective.value === 'dark' })
  } catch (e) {
    console.error('[theme] 主题未能写回 C#，本次切换不会被记住', e)
  }
}
