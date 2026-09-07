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

  【每次 pointermove 都要找一次，但只找，不画】
  <b>不能等停稳再找</b> —— 摘 title 必须赶在原生提示（Chromium 约 300ms）之前，
  而那要先知道指着谁。所以 elementFromPoint 每次移动都跑一遍，
  <b>等 DELAY 的只是画盒子那一步</b>。

  代价实测（浏览器里连派 1000 次合成 pointermove）：<b>0.034ms 一次</b>，
  鼠标移动时最多百来次每秒，与封包列表每秒几千行那条路不在一个量级。
  真要再省，可以按时间戳节流到 ~60ms 一次 —— 但那要另配一次「停下来补一次」的收尾，
  为这点开销不值得。
*/

/**
 * 悬停多久才弹出盒子。
 *
 * ⚠️ 这个值<b>不再需要比原生快</b>（原生实测约 300ms，比它快是做不到的）——
 * title 在指到的那一刻就摘走了，原生根本没得画。这里只管「扫过去别闪一下」。
 */
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

/*
  ⚠️ <b>摘 title 与画盒子必须是两步，而且摘要立刻做。</b>

  第一版把两件事写在一起、都等 DELAY 之后 —— 结果是<b>原生提示先闪一下</b>：
  Chromium 大约 300ms 就弹它自己那个，比这里的 380ms 早。
  于是顺序成了「原生弹出 → 我摘掉 title（Chromium 随即收掉它）→ 我的盒子出来」，
  看着就是闪一下再换一个。

  现在指到就摘（claim），盒子仍然等停稳 380ms 再画（render）——
  原生那条路从此没有 title 可画，早晚都轮不到它。
*/
function claim(el: HTMLElement): void {
  if (el.hasAttribute('title')) {
    const text = el.getAttribute('title') || ''

    //空 title 不接管：那是别人用来「关掉继承提示」的写法，摘了反而多事
    if (!text.trim()) return

    el.setAttribute('data-tip', text)
    el.removeAttribute('title')
  }

  host = el
}

function render(): void {
  if (!host || !host.isConnected) return

  const text = textOf(host)
  if (!text) return

  const b = ensureBox()
  b.textContent = text
  place(host, b)
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
  timer = window.setTimeout(render, DELAY)
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

    //离开了原来那个：先把它的 title 放回去
    if (host) hide()

    if (!el) return

    //⚠️ 立刻摘，别等计时器 —— 否则原生提示会赶在前面弹出来（见 claim 上面那段）
    claim(el)
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
