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

  ⚠️ <b>摘掉之后 Vue 是会把它写回来的</b>，只要那是个<b>绑定的、值在变的</b> title。
  Vue 的 patch 是「新旧值不等就 setAttribute」—— 静态 `title="…"` 的值永远不变，
  所以确实不会回来；但统计格写的是 `:title="c.z + ' · ' + c.v"`，
  而 `c.v` 每 500ms 跟着 getStats 变一次，于是每半秒就往回写一次。
  结果是元素上<b>同时</b>挂着我们的 data-tip 与 Vue 刚写回的 title：
  自绘的盒子还举着旧文案，原生的过一会儿又冒出来盖在旁边。
  所以要盯着这个属性、回来一次就再摘一次，见下面的 watch / reclaim。

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

/*
  盯着当前目标的 title 属性 —— 它<b>会</b>被写回来（见文件头那段）。
  全局只建一个，换目标时 disconnect 再 observe。
*/
let watcher: MutationObserver | null = null

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

  //盯住它：绑定值一变 Vue 就会把 title 写回来
  watch(el)
}

/*
  title 又回来了：把新值接过来、再摘一次。

  ⚠️ 顺手把盒子里的字也换成新的 —— 统计格的数字每 500ms 变一次，
  举着一个半秒前的旧值比不显示更误导。

  ⚠️ <b>不会递归。</b>这里的 removeAttribute 自己也会产生一条变更记录，
  但下一次回调进来时 title 已经没了、getAttribute 返回 null，第一句就 return 了。
*/
function reclaim(): void {
  if (!host) return

  const text = host.getAttribute('title')
  if (text === null) return

  //空 title 是「关掉继承提示」的写法，原生也画不出东西来，不去动它
  if (!text.trim()) return

  host.setAttribute('data-tip', text)
  host.removeAttribute('title')

  if (box && box.style.display === 'block') {
    box.textContent = text
    place(host, box)
  }
}

function watch(el: HTMLElement): void {
  if (!watcher) watcher = new MutationObserver(reclaim)
  else watcher.disconnect()

  watcher.observe(el, { attributes: true, attributeFilter: ['title'] })
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
    //⚠️ 先停掉观察再放回去，否则下面这句 setAttribute 会被自己的观察者再摘一遍
    if (watcher) watcher.disconnect()

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
