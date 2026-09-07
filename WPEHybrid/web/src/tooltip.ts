/*
  自绘悬停提示 —— 接管浏览器的原生 `title`。

  【为什么不做成组件 / 指令】
  全项目有 169 处 `title=`，散在 57 个文件里。做成 `v-tip` 指令要把这 169 处
  逐个改掉，而且新写一屏时忘了用指令就会静默退回原生样式（这个项目已经栽过
  好几次「抄着抄着就不一样了」）。这里改成<b>事件委托 + 接管 title 属性</b>：
  调用点一个字都不用动，`title="…"` 照写，谁写都自动是这套皮肤。

  【怎么接管】
  原生提示是浏览器自己画的，没有任何 CSS 能改它，唯一的办法是<b>让它没得画</b> ——
  悬停时把 `title` 摘到 `data-tip` 上，自己渲染一个盒子；鼠标离开再放回去。

  ⚠️ 摘掉之后如果 Vue 因为别的原因重渲染，它<b>不会</b>把 title 补回来 ——
  Vue 只在绑定值变化时才 patch 这个属性，而值没变。所以不会出现「自绘的和原生的
  一起冒出来」。真放不回去也只是这一个元素以后不再有提示，不影响别的。

  【为什么用 elementFromPoint，不是给每个元素挂 mouseenter】
  两条理由，第二条是硬的：
    ① 169 个元素挂 338 个监听器，还要跟着 v-if / 虚拟滚动增删；
    ② <b>禁用的控件不派发鼠标事件</b>（Chromium 如此），而原生 title 在禁用按钮上
       是<b>能弹的</b> —— 改成挂监听会把「为什么这个按钮点不了」这类提示全弄丢，
       而那正是最需要提示的地方。elementFromPoint 对禁用元素照样返回它。

  【为什么等鼠标停下来再算】
  pointermove 只记坐标、重置计时器，真正的 elementFromPoint + 走 DOM 只在
  停稳 DELAY 毫秒之后做一次。封包列表每秒重渲染 60 次，这条路径不能跟着动。
*/

/** 悬停多久才弹。原生大约 500ms，这里略快一点，但别快到扫过去就闪一下。 */
const DELAY = 380

/** 贴边时离视口留的余量。 */
const EDGE = 6

/** 提示框与目标之间的缝。 */
const GAP = 8

let box: HTMLElement | null = null
let timer = 0
let host: HTMLElement | null = null
let lastX = 0
let lastY = 0

/** 找到光标下最近的一个带 title 的元素。inert / 禁用的也找得到。 */
function pick(x: number, y: number): HTMLElement | null {
  let el = document.elementFromPoint(x, y) as HTMLElement | null

  while (el) {
    //已经被我们摘走的（data-tip）也算，否则移出再移回来会认不出同一个目标
    if (el.hasAttribute?.('title') || el.hasAttribute?.('data-tip')) return el
    el = el.parentElement
  }

  return null
}

function textOf(el: HTMLElement): string {
  return (el.getAttribute('title') || el.getAttribute('data-tip') || '').trim()
}

function ensureBox(): HTMLElement {
  if (box) return box

  box = document.createElement('div')
  box.className = 'tipbox'
  //自己不吃鼠标事件，否则它一挡住目标就立刻触发 mouseleave，提示会闪
  box.setAttribute('aria-hidden', 'true')
  document.body.appendChild(box)

  return box
}

function place(el: HTMLElement, b: HTMLElement): void {
  const r = el.getBoundingClientRect()

  //先放上去才量得到尺寸（与 ContextMenu 的贴边翻转同一个套路）
  b.style.left = '0px'
  b.style.top = '0px'
  b.style.visibility = 'hidden'
  b.style.display = 'block'

  const w = b.offsetWidth
  const h = b.offsetHeight

  //默认放在目标下方居中；下面放不下就翻到上方
  let top = r.bottom + GAP
  if (top + h + EDGE > window.innerHeight) top = r.top - GAP - h

  //还是放不下（目标本身比视口高）就贴着顶
  if (top < EDGE) top = EDGE

  let left = r.left + (r.width - w) / 2
  if (left + w + EDGE > window.innerWidth) left = window.innerWidth - w - EDGE
  if (left < EDGE) left = EDGE

  b.style.left = Math.round(left) + 'px'
  b.style.top = Math.round(top) + 'px'
  b.style.visibility = 'visible'
}

function show(el: HTMLElement): void {
  const text = textOf(el)
  if (!text) return

  //把 title 摘走，原生提示就没得画了
  if (el.hasAttribute('title')) {
    el.setAttribute('data-tip', text)
    el.removeAttribute('title')
  }

  host = el

  const b = ensureBox()
  b.textContent = text
  place(el, b)
}

function hide(): void {
  window.clearTimeout(timer)
  timer = 0

  if (host) {
    //放回去：万一这套脚本以后出问题，至少还能退回原生提示
    const text = host.getAttribute('data-tip')
    if (text && host.isConnected) {
      host.setAttribute('title', text)
      host.removeAttribute('data-tip')
    }
    host = null
  }

  if (box) box.style.display = 'none'
}

function schedule(): void {
  window.clearTimeout(timer)

  timer = window.setTimeout(() => {
    const el = pick(lastX, lastY)
    if (el && el !== host) show(el)
  }, DELAY)
}

export function installTooltip(): void {
  //幂等：main.ts 与探针页都会调，别装两遍
  if ((window as any).__wpeTip) return
  ;(window as any).__wpeTip = true

  document.addEventListener('pointermove', (e) => {
    lastX = e.clientX
    lastY = e.clientY

    const el = pick(lastX, lastY)

    //还停在同一个目标上：已经弹出来的就别动，正在等的也别重新计时
    if (el === host && host) return

    if (!el) { hide(); return }

    if (host) hide()
    schedule()
  }, { passive: true, capture: true })

  /*
    这几条都要收，少一条就会留下一个「指向的东西已经不在那儿了」的浮框：
      · 滚动 —— 提示是 fixed 的，页面一滚它就停在原地（与 ContextMenu 同一个坑，
        同样要用捕获阶段，滚动容器不冒泡也收得到）
      · 点击 —— 点完多半是要看结果，浮框挡着碍事
      · 按键 —— Esc 之外，开始打字时也该让开
      · 窗口失焦 / 鼠标移出文档
  */
  document.addEventListener('scroll', hide, { passive: true, capture: true })
  document.addEventListener('pointerdown', hide, { passive: true, capture: true })
  document.addEventListener('keydown', hide, { passive: true, capture: true })
  window.addEventListener('blur', hide)
  document.addEventListener('mouseleave', hide)
}
