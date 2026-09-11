/*
  自绘悬停提示 —— 接管浏览器的原生 `title`。

  【为什么不做成组件 / 指令】
  全项目有 169 处 `title=`，散在 57 个文件里。做成 `v-tip` 指令要把这 169 处
  逐个改掉，而且新写一屏时忘了用指令就会静默退回原生样式（这个项目已经栽过
  好几次「抄着抄着就不一样了」）。这里改成<b>事件委托 + 接管 title 属性</b>：
  调用点一个字都不用动，`title="…"` 照写，谁写都自动是这套皮肤。

  【怎么接管：整份文档里<b>一个 title 都不留</b>】
  原生提示是浏览器自己画的，没有任何 CSS 能改它，唯一的办法是<b>让它没得画</b>。

  ⚠️ 早先的做法是「悬停时摘、离开时还回去」，那<b>只挡得住鼠标那一条路</b>。
  用户报的就是漏掉的另一条：<b>用 Tab 切焦点时原生提示又冒出来了</b> ——
  Chromium 在元素<b>获得键盘焦点</b>时也会弹 title，而那个元素根本没被鼠标指到过，
  它的 title 还好端端待在 DOM 里。

  所以现在改成<b>全局摘除</b>：装上时全扫一遍，之后一个 MutationObserver 盯着整份文档，
  凡是出现 title 就立刻挪到 data-tip 上，<b>再也不还回去</b>。
  没有 title，浏览器就没有任何时机能画那个灰框 —— 悬停、聚焦、长按，一条都没有。

  这么改顺带删掉了一整类 bug：不再有「摘得比原生慢」的竞速、不再有「离开时还回去
  结果又被弹出来」、也不再需要区分「这个 data-tip 是我摘的还是调用点自己写的」。

  ⚠️ <b>无障碍要补一手</b>：title 有可能是这个元素唯一的可访问名（图标按钮就是这样）。
  摘走时若它没有别的名字（没有 aria-label / aria-labelledby、也没有文字内容），
  就把这段文案写成 aria-label —— 读屏照旧念得出来，而 aria-label 不会画出原生框。

  ⚠️ 新建的元素<b>不会</b>产生 attributes 记录：Vue 是在元素还没插进文档时就 setAttribute 的。
  所以观察器必须<b>同时</b>盯 childList，对每一批新加进来的子树再扫一遍。

  【键盘聚焦也要弹，只是弹的是我们自己那个盒子】
  摘光 title 之后 Tab 过去就<b>什么都不弹</b>了 —— 而原生在聚焦时弹提示本来是一件对的事
  （键盘用户与读屏用户就靠它知道这颗图标按钮是干什么的），不该顺手一起丢掉。
  所以 focusin 那条路自己补一个盒子出来，位置 / 外观 / 延时与鼠标那条完全一样。

  ⚠️ <b>只认键盘来的焦点，点出来的不算。</b>点一颗按钮也会让它获得焦点，
  跟着弹一个提示是纯粹的打扰 —— 用户刚点完，已经知道那是什么了。
  判据是「最近 300ms 内有没有按下过指针」，不是 <b>:focus-visible</b>：
  后者在浏览器面板没有系统焦点时<b>根本不匹配</b>（本项目已经栽过一次），
  拿它当闸门等于让这条路在自动化验证里永远走不到，出了问题也看不出来。

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
  盯着当前目标的 title / data-tip —— 前者<b>会</b>被写回来（见文件头那段），
  后者是调用点自己写的、值也会变。全局只建一个，换目标时 disconnect 再 observe。
*/
let watcher: MutationObserver | null = null

/**
 * 当前这个提示是<b>键盘聚焦</b>弹出来的吗（false = 鼠标指出来的）。
 *
 * 两条路的差别只在「谁决定它该收起来」：鼠标那条看光标还在不在目标上，
 * 键盘这条看焦点还在不在目标上。滚动时的处理也因此不同（见 onScroll）。
 */
let viaKey = false

/** 最近一次按下指针的时刻 —— 用来把「点出来的焦点」挡在门外，见 onFocusIn。 */
let lastDown = 0

/*
  把一个元素的 title 挪到 data-tip 上。摘完就不再还回去了。

  ⚠️ <b>先 removeAttribute 再 setAttribute</b>：反过来的话中间那一瞬间两个属性都在，
  而观察器是下一个微任务才跑 —— 那一瞬间原生框是有机会画出来的。
*/
function harvest(el: Element): void {
  const t = el.getAttribute('title')
  if (t === null) return

  el.removeAttribute('title')

  //空 title 是「关掉继承提示」的写法，删掉就够了，不用往 data-tip 上搬
  if (!t.trim()) return

  el.setAttribute('data-tip', t)

  /*
    无障碍兜底：title 摘走之后，图标按钮这类<b>没有文字的</b>元素就没名字了。
    ⚠️ 只在它确实没有别的名字时才补 —— 给一个本来有文字的元素加 aria-label
    等于把那段文字对读屏<b>盖掉</b>。
  */
  if (
    !el.hasAttribute('aria-label') &&
    !el.hasAttribute('aria-labelledby') &&
    !(el.textContent || '').trim()
  ) {
    el.setAttribute('aria-label', t)
  }
}

/** 把一棵子树里所有带 title 的都摘了（含根自己）。 */
function sweep(root: Element | Document): void {
  if (root.nodeType === 1) harvest(root as Element)
  root.querySelectorAll('[title]').forEach(harvest)
}

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
  认下这个目标。

  ⚠️ 这里<b>不再搬运 title</b> —— 全局观察器早就把它摘走了。留一句 harvest 只是保险：
  万一某个元素是「刚插进来、观察器的微任务还没跑」就被指到，这一下就地补上。
*/
function claim(el: HTMLElement): void {
  harvest(el)
  host = el

  //盯住它的 data-tip：统计格那几处的值每 500ms 变一次，盒子要跟着换字
  watch(el)
}

/*
  当前目标的文案变了：盒子已经画出来的话就换成新值。

  统计格的数字每 500ms 变一次，举着一个半秒前的旧数字比不显示更误导。
  （title 变回来这件事已经不归这里管了 —— 全局观察器会先把它摘掉，
  摘的时候写的是 data-tip，于是这里跟着醒来。）
*/
function reclaim(): void {
  if (!host) return
  if (!box || box.style.display !== 'block') return

  const text = textOf(host)
  if (!text) return

  box.textContent = text
  place(host, box)
}

function watch(el: HTMLElement): void {
  if (!watcher) watcher = new MutationObserver(reclaim)
  else watcher.disconnect()

  watcher.observe(el, { attributes: true, attributeFilter: ['data-tip'] })
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
    //⚠️ <b>不还回去。</b>还回去就等于亲手把原生提示装回来 —— Tab 切焦点时它就会冒出来，
    //   而那正是这一版要根治的事。data-tip 留在元素上，下次指到它照样能用。
    if (watcher) watcher.disconnect()
    host = null
  }

  viaKey = false

  if (box) box.style.display = 'none'
}

function schedule(): void {
  window.clearTimeout(timer)
  timer = window.setTimeout(render, DELAY)
}

/**
 * 光标此刻指着谁 —— 鼠标移动与滚动共用这一段。
 *
 * @param reposition 目标可能被挪了位置（滚动那条路），盒子要跟着走
 */
function track(reposition: boolean): void {
  const el = pick(lastX, lastY)

  //还停在同一个目标上：已经弹出来的就别动，正在等的也别重新计时
  if (el === host && host) {
    if (reposition && box && box.style.display === 'block') place(host, box)
    return
  }

  //离开了原来那个：先把它的 title 放回去
  if (host) hide()

  if (!el) return

  //⚠️ 立刻摘，别等计时器 —— 否则原生提示会赶在前面弹出来（见 claim 上面那段）
  claim(el)
  viaKey = false
  schedule()
}

/*
  键盘把焦点挪到了一个带提示的东西上 —— 补一个自绘的盒子。

  ⚠️ <b>点出来的焦点不算</b>（见文件头）：点一颗按钮也会让它聚焦，跟着弹提示是打扰。
  300ms 这个窗口足够盖住「pointerdown → 焦点落定」这一段，又短到「点完之后再按 Tab」
  仍然算键盘。

  ⚠️ 焦点落在哪个元素上，与提示写在哪个元素上<b>不一定是同一个</b>
  （常见的是按钮里套一个 svg，或者提示写在外面那层包装上），所以要 closest 往上找一层。
*/
function onFocusIn(e: FocusEvent): void {
  if (Date.now() - lastDown < 300) return

  const at = e.target as HTMLElement | null
  if (!at || typeof at.closest !== 'function') return

  const el = at.closest('[data-tip], [title]') as HTMLElement | null
  if (!el) return

  //⚠️ 保险：这个元素可能是「刚插进来、全局观察器的微任务还没跑」就被 Tab 到了
  harvest(el)
  if (!textOf(el)) return

  if (el === host) return

  if (host) hide()

  host = el
  viaKey = true
  watch(el)

  //与鼠标那条同样等 DELAY 再画：一路按住 Tab 扫过去时不会一路闪
  schedule()
}

/*
  ⚠️ <b>滚动不能一律 hide。</b>

  用户报的是「封包列表在不停进新封包时，提示<b>根本弹不出来</b>，等它不动了才行」——
  病根就在这儿：封包列表跟随底部时<b>每一拍都在改 scrollTop</b>，而改 scrollTop
  是会真的派发 scroll 事件的。原来这条捕获阶段的监听二话不说就 hide()，
  于是每帧把提示杀一次；更糟的是鼠标没动就<b>再也没有 pointermove 来重新接管</b>，
  提示一去不回，而 title 已经被还回去了 —— 原生那个灰框反倒有机会冒出来。

  真正要防的只有一件事：<b>提示指向的那个东西被滚走了</b>（盒子是 fixed 的，
  会停在原地指着一个已经换了内容的位置）。所以：

    · 滚的容器<b>不包含</b>当前目标 —— 它根本没动，什么都不用做（封包列表就是这一支，
      每帧都滚，这里直接 return，连 elementFromPoint 都不跑）；
    · 包含 —— 按光标最后的位置重新认一次：还是它就让盒子跟着挪，换人了才收。
*/
function onScroll(e: Event): void {
  //没有目标就没有要维护的东西 —— 封包列表刷屏时绝大多数时候走的是这一句
  if (!host) return

  /*
    ⚠️ 键盘那条路不能拿光标位置去重认目标 —— 光标可能停在屏幕上任意一处，
    与聚焦的那个元素毫无关系，一滚就会把提示判给别人（或者直接收掉）。
    焦点还在谁身上是<b>确定</b>的，所以只让盒子跟着它挪。
  */
  if (viaKey) {
    if (!host.isConnected) { hide(); return }
    if (box && box.style.display === 'block') place(host, box)
    return
  }

  const t = e.target as Node | null
  const whole = t === document || t === document.documentElement || t === document.body
  if (!whole && !(t && t.contains(host))) return

  track(true)
}

export function installTooltip(): void {
  //幂等：main.ts 与探针页都会调，别装两遍
  if ((window as any).__wpeTip) return
  ;(window as any).__wpeTip = true

  /*
    ⚠️ <b>全局摘除。</b>整份文档里从此一个 title 都不留 —— 悬停、Tab 聚焦、长按，
    浏览器一条路都画不出那个灰框。

    两种记录都要收：
      · attributes —— Vue patch 一个<b>绑定且值在变</b>的 title（统计格那种）；
      · childList  —— 新插进来的子树。<b>这一支不能省</b>：Vue 是在元素还没进文档时
        就 setAttribute 的，那时观察器根本看不到，不扫新子树就会漏。

    代价：全项目 144 处 title，封包列表那张表里只有 2 处（列宽手柄与回到底部按钮），
    都不在行单元格上 —— 刷屏时这个观察器基本没有活干。
  */
  sweep(document)

  new MutationObserver((recs) => {
    for (const r of recs) {
      if (r.type === 'attributes') { harvest(r.target as Element); continue }
      for (const n of r.addedNodes) { if (n.nodeType === 1) sweep(n as Element) }
    }
  }).observe(document.documentElement, {
    subtree: true,
    childList: true,
    attributes: true,
    attributeFilter: ['title'],
  })

  document.addEventListener('pointermove', (e) => {
    lastX = e.clientX
    lastY = e.clientY

    track(false)
  }, { passive: true, capture: true })

  /*
    这几条都要收，少一条就会留下一个「指向的东西已经不在那儿了」的浮框：
      · 滚动 —— 提示是 fixed 的，页面一滚它就停在原地（与 ContextMenu 同一个坑，
        同样要用捕获阶段，滚动容器不冒泡也收得到）。
        ⚠️ 但<b>不能一律 hide</b>，见 onScroll 上面那段。
      · 点击 —— 点完多半是要看结果，浮框挡着碍事
      · 按键 —— Esc 之外，开始打字时也该让开
      · 窗口失焦 / 鼠标移出文档
  */
  document.addEventListener('scroll', onScroll, { passive: true, capture: true })

  //⚠️ 记时刻要在 hide 之前 —— onFocusIn 靠它把「点出来的焦点」挡掉
  document.addEventListener('pointerdown', () => { lastDown = Date.now(); hide() },
    { passive: true, capture: true })

  /*
    ⚠️ 按键仍然一律收，<b>连 Tab 也不例外</b>，而且这正是键盘那条路要的顺序：
    Tab 的 keydown 先到（收掉上一个），焦点随后落定，focusin 再把新的那个弹出来。
    在这里给 Tab 开后门反而会留下一个指着旧元素的浮框。
  */
  document.addEventListener('keydown', hide, { passive: true, capture: true })

  document.addEventListener('focusin', onFocusIn, { passive: true, capture: true })

  //焦点走了就收 —— 鼠标那条路不受影响（它的 host 不是聚焦出来的）
  document.addEventListener('focusout', () => { if (viaKey) hide() },
    { passive: true, capture: true })
  window.addEventListener('blur', hide)
  document.addEventListener('mouseleave', hide)
}
