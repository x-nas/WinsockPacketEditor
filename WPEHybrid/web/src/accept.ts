// B10 验收跑测。
//
// 验收线（CLAUDE.md「下一步：B10（垂直切片）」）：
//   ① ≥3000 条/秒不掉帧
//   ② 5000 行滚动 60fps
//   ③ 内存稳定
//   ④ 点击到十六进制出内容 <50ms
//
// 【为什么写成脚本而不是手点】
// 「不掉帧」「60fps」是分布问题，肉眼看不出来 —— 每秒掉两帧的界面看着完全正常。
// 而且验收要能复现：改了实现之后得能跑同一套测试比数字，否则「优化了」只是感觉。
//
// 【为什么用稳态灌包而不是一次性倒】
// 一次性把 5 万条倒进队列，测到的是队列排空速度（搬运定时器每拍 200 条 ≈ 13000 条/秒），
// 那是突发，不是稳态 3000 条/秒。两者的帧时间分布完全不同，用突发交差是自欺。
// 对应 C# 的 devStartLoad，它开一个后台线程按累计时间补差地灌。

import { call } from './bridge'
import { FeedList } from './bridge/types'
import { resetStat, rows, stat } from './stores/packets'
import { dump } from './hex'

/**
 * 一次帧测量至少要采到多少帧才算数（按每秒计）。
 *
 * 这条判据不是可有可无的：窗口被最小化或完全遮挡时 Chromium 会停发 rAF，
 * 一帧都采不到 —— 而「0 个样本」算出来的掉帧率<b>正好是 0</b>，
 * 于是整项测试会静默地判成「通过」。能假阳性的验收脚本比没有更糟。
 */
const MIN_FPS_VALID = 20

/** 样本够不够多到可以下判断。不够就说明测量本身没成立，而不是被测对象好或坏。 */
function valid(f: FrameSummary): boolean {
  return f.frames > 0 && f.fps >= MIN_FPS_VALID
}

const INVALID_HINT =
  '测量无效：这段时间内几乎没有渲染帧。请确认窗口没有被最小化、没有被其它窗口完全遮挡，' +
  '也没有切到别的虚拟桌面 —— 那些情况下浏览器内核会停发 requestAnimationFrame。'

export interface FrameSummary {
  frames: number
  seconds: number
  fps: number
  /** 帧间隔（vsync 对齐）的分位数 */
  p50: number
  p95: number
  max: number
  /** 真实缺帧数：间隔超过 1.5 个刷新周期 */
  dropped: number
  dropPct: number
  /** 推断出的屏幕刷新周期，ms */
  period: number
  /** 主线程派发延迟：回调比帧开始晚跑了多久。不掉帧，但反映主线程压力 */
  lagP50: number
  lagP95: number
  lagMax: number
}

const r1 = (x: number) => Math.round(x * 10) / 10
const pct = (sorted: number[], q: number) => sorted[Math.min(sorted.length - 1, Math.floor(sorted.length * q))]

function summarize(dts: number[], lags: number[]): FrameSummary {
  if (!dts.length) {
    return {
      frames: 0, seconds: 0, fps: 0, p50: 0, p95: 0, max: 0,
      dropped: 0, dropPct: 0, period: 0, lagP50: 0, lagP95: 0, lagMax: 0,
    }
  }

  const sorted = [...dts].sort((a, b) => a - b)
  const total = dts.reduce((s, x) => s + x, 0)

  /*
    掉帧的判据用「中位间隔的 1.5 倍」，不写死 20ms。

    中位数就是这块屏的刷新周期（60Hz → 16.7ms，144Hz → 6.9ms）。
    真正缺一帧时间隔会跳到 2 个周期，1.5 倍能干净地把两者分开，
    而写死的阈值换一块高刷屏就会把每一帧都算成掉帧。
  */
  const period = pct(sorted, 0.5)
  const dropped = dts.filter((x) => x > period * 1.5).length

  const ls = [...lags].sort((a, b) => a - b)

  return {
    frames: dts.length,
    seconds: r1(total / 1000),
    fps: r1((dts.length * 1000) / total),
    p50: r1(period),
    p95: r1(pct(sorted, 0.95)),
    max: r1(sorted[sorted.length - 1]),
    dropped,
    dropPct: r1((dropped * 100) / dts.length),
    period: r1(period),
    lagP50: r1(pct(ls, 0.5)),
    lagP95: r1(pct(ls, 0.95)),
    lagMax: r1(ls[ls.length - 1]),
  }
}

/**
 * 逐帧记录。
 *
 * 【必须用回调参数 timestamp，不能用 performance.now()】
 * timestamp 是这一帧的<b>呈现时刻</b>，vsync 对齐，同一帧里所有回调拿到的值相同。
 * 在回调体内读 performance.now() 量到的则是「回调被派发的时刻」——
 * 主线程当帧忙一点回调就晚跑，间隔会显示成 25ms 上下，而下一次回调又正好卡回
 * 它自己的 vsync，间隔缩回去，均值仍是一个周期。于是量到一堆并不存在的「掉帧」。
 *
 * B10e 第一次跑测就栽在这里：报告说掉帧 3.3%，可 15 秒采到 899 帧
 * （60Hz 满帧是 900）—— 真掉 30 帧只会剩约 870 帧。帧数自己就证伪了掉帧数。
 *
 * 派发延迟本身是有用的信号（反映主线程压力），所以单独记成 lag，
 * 但它不叫掉帧。
 *
 * 第一帧的间隔要丢掉：它量的是「从开始记录到下一帧」，不是一个完整周期。
 */
function frameProbe(): { stop: () => FrameSummary } {
  const dts: number[] = []
  const lags: number[] = []
  let prevTs = 0
  let raf = 0
  let running = true

  const tick = (ts: number) => {
    if (prevTs) dts.push(ts - prevTs)
    prevTs = ts

    // rAF 的 timestamp 与 performance.now() 同一时间原点，可以直接相减
    lags.push(performance.now() - ts)

    if (running) raf = requestAnimationFrame(tick)
  }

  raf = requestAnimationFrame(tick)

  return {
    stop() {
      running = false
      cancelAnimationFrame(raf)
      return summarize(dts, lags)
    },
  }
}

const sleep = (ms: number) => new Promise<void>((r) => setTimeout(r, ms))

/**
 * 等下一帧，但带超时。
 *
 * 窗口最小化或被完全遮挡时 Chromium 会<b>彻底停掉</b> rAF（不是降频，是不发），
 * 裸 await rAF 会永远不返回，整套跑测就挂在那里、连报告都出不来。
 * 超时值给 100ms：正常 60Hz 下永远走不到，只在 rAF 真停了的时候兜底。
 */
function nextFrame(): Promise<void> {
  return new Promise<void>((r) => {
    let done = false
    const fin = () => {
      if (done) return
      done = true
      r()
    }
    requestAnimationFrame(fin)
    setTimeout(fin, 100)
  })
}

/** 列表的滚动容器。测试模块直接查 DOM，不为跑测往组件上开 API。 */
function scroller(): HTMLElement | null {
  return document.querySelector<HTMLElement>('.pl-scroll')
}

export interface MemSample {
  jsHeap: number
  managed: number
  workingSet: number
  privateBytes: number
  gc2: number
}

async function sampleMem(): Promise<MemSample> {
  const m = await call<any>('devMemory')
  const perf = (performance as any).memory
  return {
    jsHeap: perf?.usedJSHeapSize ?? 0,
    managed: m.managed,
    workingSet: m.workingSet,
    privateBytes: m.privateBytes,
    gc2: m.gc2,
  }
}

const MB = (n: number) => Math.round((n / 1048576) * 10) / 10

export interface Report {
  text: string
  pass: boolean
}

/**
 * 跑完整套验收。log 会在每一步被调用，供界面显示进度。
 * 全程约 45 秒。
 */
export async function runAcceptance(log: (s: string) => void): Promise<Report> {
  const lines: string[] = []
  const fails: string[] = []

  const out = (s: string) => {
    lines.push(s)
    log(s)
  }

  const stamp = new Date().toLocaleString('zh-CN')
  const info = await call<any>('getAppInfo')

  out('WPE x64 · B10 验收跑测')
  out('='.repeat(64))
  out(`时间        ${stamp}`)
  out(`进程        ${info.is64Bit ? '64 位' : '32 位'} · CLR ${info.clr}`)
  out(`系统        ${info.os}`)
  out(`UA          ${navigator.userAgent.match(/Chrome\/[\d.]+/)?.[0] ?? '未知'}`)
  out(`推送批上限  ${info.batchMax} 条/拍`)
  out('')

  // 记下原始设置，跑完要还回去
  const saved = await call<any>('devSetAutoClear', {})

  try {
    // ── 基线：空闲时的帧率 ────────────────────────────────
    // 没有这条基线，后面的数字没法归因：60fps 达不到，可能是被测代码的问题，
    // 也可能是这台机器/这个窗口本来就到不了 60（比如 WebView2 被限帧、显示器是 50Hz）。
    out('【基线】空闲 3 秒')
    await call('devStopLoad')
    await call('clearPackets')
    resetStat()
    await sleep(500)

    let p = frameProbe()
    await sleep(3000)
    const base = p.stop()
    out(`  ${fmtFrame(base)}`)

    if (!valid(base)) {
      out('')
      out(INVALID_HINT)
      out('')
      out('基线都不成立，后面的数字全是废的，就此中止。')
      fails.push('基线帧率过低，测量环境不成立')
      throw new Error('BASELINE_INVALID')
    }
    out('')

    const mem0 = await sampleMem()

    // ── ① 稳态 3000 条/秒 × 15 秒 ────────────────────────
    out('【① 吞吐】稳态 3000 条/秒 · 15 秒 · 自动清理保持默认')
    await call('devSetAutoClear', { on: true, value: 5000 })
    await call('clearPackets')
    resetStat()

    await call('devStartLoad', { rate: 3000, size: 512 })
    await sleep(1500) // 让速率稳下来再开始记
    p = frameProbe()
    await sleep(15000)
    const load = p.stop()
    const rateSeen = stat.value.rate
    const received = stat.value.received
    const st1 = await call<any>('getStats')
    await call('devStopLoad')

    out(`  ${fmtFrame(load)}`)
    out(`  实收 ${received.toLocaleString()} 条 · 末次速率 ${rateSeen.toLocaleString()} 条/秒`)
    out(`  最大批 ${stat.value.maxBatch} · 残留队列 ${st1.queue.toLocaleString()}`)

    // 判定分两条，缺一不可：
    //   帧 —— 掉帧率 <1%（偶发一两帧不算问题，持续掉才是）
    //   量 —— 队列没有持续堆积，说明前端确实吃下了 3000 条/秒而不是被队列兜住了
const okFrame1 = valid(load) && load.dropPct < 1
    const okQueue1 = st1.queue <= 3000
    if (!valid(load)) fails.push(`① ${INVALID_HINT}`)
    else if (load.dropPct >= 1) fails.push(`① 掉帧率 ${load.dropPct}% ≥ 1%`)
    if (!okQueue1) fails.push(`① 队列积压 ${st1.queue} 条，前端没吃住 3000 条/秒`)
    out(`  判定：${okFrame1 && okQueue1 ? '通过' : '未通过'}`)
    out('')

    // ── ② 5000 行滚动 ────────────────────────────────────
    out('【② 滚动】列表满 5000 行 · 连续滚动 8 秒')
    await call('devSetAutoClear', { on: false })
    await call('clearPackets')
    resetStat()
    await call('devGeneratePackets', { count: 5000, size: 512 })

    // 等搬运定时器把 5000 条全搬完（每拍 200 条 → 约 250ms，给足余量）
    for (let i = 0; i < 40; i++) {
      const s = await call<any>('getStats')
      if (s.list >= 5000 && s.queue === 0) break
      await sleep(100)
    }

    const st2 = await call<any>('getStats')
    out(`  列表行数 ${st2.list.toLocaleString()}`)

    const el = scroller()
    if (!el) {
      fails.push('② 找不到滚动容器')
      out('  判定：未通过（找不到滚动容器）')
    } else {
      const max = el.scrollHeight - el.clientHeight
      p = frameProbe()

      // 每帧挪一步、来回扫全程。比人手滚更狠：真人滚不到每帧都产生新的可见窗口。
      const t0 = performance.now()
      let dir = 1
      let pos = 0
      const step = Math.max(1, Math.round(max / 90)) // 约 1.5 秒扫完一趟

      while (performance.now() - t0 < 8000) {
        pos += dir * step
        if (pos >= max) { pos = max; dir = -1 }
        if (pos <= 0) { pos = 0; dir = 1 }
        el.scrollTop = pos
        await nextFrame()
      }

      const scr = p.stop()
      out(`  ${fmtFrame(scr)}`)

      // 滚动的判定比吞吐宽一点：滚动是持续重排，允许 2% 的掉帧
      const ok2 = valid(scr) && scr.dropPct < 2 && scr.fps >= 55
      if (!valid(scr)) fails.push(`② ${INVALID_HINT}`)
      else {
        if (scr.dropPct >= 2) fails.push(`② 滚动掉帧率 ${scr.dropPct}% ≥ 2%`)
        if (scr.fps < 55) fails.push(`② 滚动帧率 ${scr.fps} < 55`)
      }
      out(`  判定：${ok2 ? '通过' : '未通过'}`)
    }
    out('')

    // ── ④ 点击到十六进制 ─────────────────────────────────
    // 顺序上放在滚动之后，因为它要用列表里现成的行
    out('【④ 取字节】点击到十六进制出内容 · 50 次')

    // Id 必须从列表里现取。PacketInfo.Id 是<b>进程内单调自增</b>的，
    // 跑完测试 ① 早就涨到几万，写死 1..5000 取到的全是不存在的 Id ——
    // 那量的是「扫完整表没找到」，既不是真实路径，也拿不到字节做格式化。
    const live = rows.value
    const ids: number[] = []
    const stride = Math.max(1, Math.floor(live.length / 50))
    for (let i = 0; i < live.length && ids.length < 50; i += stride) ids.push(live[i].Id)

    if (!ids.length) {
      fails.push('④ 列表为空，取不到可测的 Id')
      out('  判定：未通过（列表为空）')
    }

    const lat: number[] = []   // 端到端：点下去到这一帧画得出来
    const net: number[] = []   // 净成本：桥往返 + 格式化，不含等帧
    let bytes = 0
    for (const id of ids) {
      const t0 = performance.now()
      const d = await call<any>('getPacketDetail', { id, list: FeedList.Proxy })
      // 把面板真正要做的格式化也算进去：只测桥的往返会偏乐观
      const text = dump(d?.packet ?? null)
      bytes += text.length
      net.push(performance.now() - t0)
      // 等一帧：把「数据到手」延伸到「这一帧画得出来」，与用户实际感知一致
      await nextFrame()
      lat.push(performance.now() - t0)
    }

    if (!lat.length) { lat.push(0); net.push(0) }
    net.sort((a, b) => a - b)

    lat.sort((a, b) => a - b)
    const p50 = r1(lat[Math.floor(lat.length * 0.5)])
    const p95 = r1(lat[Math.floor(lat.length * 0.95)])
    const pMax = r1(lat[lat.length - 1])

    out(`  取样 ${ids.length} 个 Id（${ids[0]} … ${ids[ids.length - 1]}）· 格式化 ${bytes} 字符`)
    out(`  净成本（桥往返 + 格式化）p50 ${r1(net[Math.floor(net.length * 0.5)])}ms · ` +
        `p95 ${r1(net[Math.floor(net.length * 0.95)])}ms`)
    out(`  端到端（含等一帧，约 ${Math.round(1000 / 60)}ms 地板）p50 ${p50}ms · p95 ${p95}ms · max ${pMax}ms`)
    if (p95 >= 50) fails.push(`④ 取字节 p95 ${p95}ms ≥ 50ms`)
    out(`  判定：${p95 < 50 ? '通过' : '未通过'}`)
    out('')

    // ── ③ 内存 ───────────────────────────────────────────
    out('【③ 内存】压测前 / 压测后 / 清空静置后')
    const mem1 = await sampleMem()

    await call('devStopLoad')
    await call('clearPackets')
    resetStat()
    await sleep(3000) // 给 GC 一点时间

    const mem2 = await sampleMem()

    out('              JS 堆     托管堆    工作集    私有字节')
    out(`  压测前    ${memRow(mem0)}`)
    out(`  压测后    ${memRow(mem1)}`)
    out(`  清空静置  ${memRow(mem2)}`)
    out(`  Gen2 回收 ${mem0.gc2} → ${mem2.gc2} 次`)

    // 判定：清空静置后应该回落到接近压测前。放宽到 1.6 倍 —— WebView2 与 .NET 都不会
    // 把峰值内存立刻还给系统，纠结绝对值没意义；要抓的是「越跑越涨、不回落」。
    const grow = mem2.workingSet / Math.max(1, mem0.workingSet)
    out(`  工作集回落比 ${Math.round(grow * 100) / 100}×`)
    if (grow > 1.6) fails.push(`③ 清空后工作集仍为压测前的 ${Math.round(grow * 100) / 100} 倍`)
    out(`  判定：${grow <= 1.6 ? '通过' : '未通过'}`)
    out('')
  } catch (e) {
    // BASELINE_INVALID 是主动中止，理由已经写进报告了，不必再重复一遍
    if (!(e instanceof Error) || e.message !== 'BASELINE_INVALID') {
      out('')
      out(`跑测中断：${e}`)
      fails.push(`跑测中断：${e}`)
    }
  } finally {
    // 无论中途出什么事，都把灌包停掉、把自动清理还原
    await call('devStopLoad').catch(() => {})
    await call('devSetAutoClear', { on: saved.on, value: saved.value }).catch(() => {})
  }

  out('='.repeat(64))
  if (fails.length) {
    out(`结论：未通过（${fails.length} 项）`)
    fails.forEach((f) => out(`  · ${f}`))
    out('')
    out('按 CLAUDE.md 的约定，任一项不达标就停下重新评估，不要继续做其余 13 个页面。')
  } else {
    out('结论：全部通过')
  }

  const text = lines.join('\r\n')

  try {
    const r = await call<any>('devWriteReport', { name: 'B10-acceptance', text })
    out('')
    out(`报告已写入 ${r.path}`)
    lines.push('', `报告已写入 ${r.path}`)
  } catch (e) {
    out(`报告落盘失败：${e}`)
  }

  return { text, pass: fails.length === 0 }
}

function fmtFrame(f: FrameSummary): string {
  // 满帧基准 = 时长 / 刷新周期。把它和实采帧数并排放出来，
  // 是为了让「掉帧数」永远有一个独立的交叉验证：两者对不上就说明测量本身有问题，
  // 而不是被测对象有问题（B10e 第一版就是这么发现测错了的）。
  const ideal = f.period > 0 ? Math.round((f.seconds * 1000) / f.period) : 0

  return (
    `${f.fps} fps · 实采 ${f.frames} 帧 / 满帧 ${ideal} 帧 / ${f.seconds}s\n` +
    `    帧间隔  p50 ${f.p50}ms · p95 ${f.p95}ms · max ${f.max}ms · 掉帧 ${f.dropped} (${f.dropPct}%)\n` +
    `    派发延迟 p50 ${f.lagP50}ms · p95 ${f.lagP95}ms · max ${f.lagMax}ms（主线程压力，不是掉帧）`
  )
}

function memRow(m: MemSample): string {
  const c = (n: number) => String(MB(n)).padStart(8, ' ')
  return `${c(m.jsHeap)}  ${c(m.managed)}  ${c(m.workingSet)}  ${c(m.privateBytes)}`
}
