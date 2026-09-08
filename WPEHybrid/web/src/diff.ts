/*
  对齐差异（Myers）—— 文本对比页的核心。

  ⚠️ <b>为什么不能按位置逐个比。</b> 老版本就是那么写的（位置 i 上 A/B 谁缺就是增/删，
  都有但不同就是改），后果在<b>插入一个单位</b>时最难看：实测

      A = AA BB CC DD EE FF        （6 字节）
      B = AA 11 BB CC DD EE FF     （B 在偏移 1 多了一个 11）

  给出 <b>13 条</b>「修改」，还都是错的（「位置 7：C vs B 修改」）。
  正确答案只有一句：B 在偏移 1 多了一个字节。而「插了一个字节」恰恰是抓包对比里最常见的事。

  所以这里用 Myers 的 O(ND) 贪心版求<b>最短编辑脚本</b>，出的是<b>块</b>不是位置：
  上面那个例子只有一块 —— 增 · B@1 · 1 个单位。

  单位由调用方定：十六进制模式按<b>字节</b>，文本模式按<b>行</b>（见 diffBytes / diffLines）。
*/

/** same=两边相同 · mod=两边都有但不同 · ins=只有 B 有 · del=只有 A 有 */
export type DiffOp = 'same' | 'mod' | 'ins' | 'del'

export interface DiffBlock {
  op: DiffOp
  /** 在 A 的单位序列里的起点与长度（ins 时 aLen = 0） */
  aStart: number
  aLen: number
  /** 在 B 的单位序列里的起点与长度（del 时 bLen = 0） */
  bStart: number
  bLen: number
}

export interface DiffResult {
  blocks: DiffBlock[]
  /** 非 same 的块数 —— 界面上那个「n / 总数」用它 */
  changes: number
  /**
   * true = 差异太多，超出了算力上限，中间那一整段被当成一块「改」。
   * ⚠️ 一定要让界面把这件事说出来：否则用户看到的是一块巨大的「改」，
   * 却以为算法认真比过了。
   */
  truncated: boolean
}

/*
  编辑距离上限。

  ⚠️ Myers 的这个写法要留 d 条轨迹，内存是 O(D²)；D 不封顶的话两段毫不相干的
  长文本能吃掉几百 MB。封在 600：抓包对比的典型情形是「大同小异」，
  掐头去尾之后 D 通常只有个位数；真超了就退化成「中间整段算一块改」，
  并把 truncated 标出来 —— <b>宁可说不知道，也不要装作算过了</b>。
*/
const MAX_D = 600

/** 掐头去尾之后还剩这么多单位就不算了，直接整段算一块改（保护 UI 不被卡死） */
const MAX_UNITS = 60000

function myers<T>(a: T[], b: T[], eq: (x: T, y: T) => boolean): { ops: Array<0 | 1 | 2>; ok: boolean } {
  //0 = 两边一起走（相同）、1 = 只走 A（删）、2 = 只走 B（增）
  const N = a.length
  const M = b.length

  const off = MAX_D + 1
  const v = new Int32Array(2 * MAX_D + 3)
  const trace: Int32Array[] = []

  for (let d = 0; d <= MAX_D; d++) {
    trace.push(v.slice())

    for (let k = -d; k <= d; k += 2) {
      let x: number

      if (k === -d || (k !== d && v[off + k - 1] < v[off + k + 1])) x = v[off + k + 1]
      else x = v[off + k - 1] + 1

      let y = x - k
      while (x < N && y < M && eq(a[x], b[y])) { x++; y++ }

      v[off + k] = x

      if (x >= N && y >= M) return { ops: backtrack(trace, d, off, N, M), ok: true }
    }
  }

  return { ops: [], ok: false }
}

/** 从轨迹倒着走回起点，还原成一串「相同 / 删 / 增」 */
function backtrack(trace: Int32Array[], d: number, off: number, N: number, M: number): Array<0 | 1 | 2> {
  const out: Array<0 | 1 | 2> = []
  let x = N
  let y = M

  for (let dd = d; dd > 0; dd--) {
    const v = trace[dd]
    const k = x - y

    const down = k === -dd || (k !== dd && v[off + k - 1] < v[off + k + 1])
    const kPrev = down ? k + 1 : k - 1
    const xStart = v[off + kPrev]
    const yStart = xStart - kPrev
    const xMid = down ? xStart : xStart + 1

    while (x > xMid) { out.push(0); x--; y-- }        //斜着走过来的那一段是相同
    if (down) { out.push(2); y = yStart } else { out.push(1); x = xStart }
  }

  while (x > 0) { out.push(0); x--; y-- }

  out.reverse()
  return out
}

/** 一串 0/1/2 收成块，并把「删紧挨着增」并成一块「改」 */
function toBlocks(ops: Array<0 | 1 | 2>, aOff: number, bOff: number): DiffBlock[] {
  const raw: DiffBlock[] = []
  let ai = aOff
  let bi = bOff

  for (let i = 0; i < ops.length;) {
    const op = ops[i]
    let n = 0
    while (i + n < ops.length && ops[i + n] === op) n++

    if (op === 0) raw.push({ op: 'same', aStart: ai, aLen: n, bStart: bi, bLen: n })
    else if (op === 1) raw.push({ op: 'del', aStart: ai, aLen: n, bStart: bi, bLen: 0 })
    else raw.push({ op: 'ins', aStart: ai, aLen: 0, bStart: bi, bLen: n })

    if (op !== 2) ai += n
    if (op !== 1) bi += n
    i += n
  }

  //删 + 增贴在一起，其实是「这一段改了」—— 分成两块读起来像凭空少一段又凭空多一段
  const out: DiffBlock[] = []
  for (const blk of raw) {
    const prev = out[out.length - 1]

    if (prev && ((prev.op === 'del' && blk.op === 'ins') || (prev.op === 'ins' && blk.op === 'del'))) {
      out[out.length - 1] = {
        op: 'mod',
        aStart: Math.min(prev.aStart, blk.aStart),
        aLen: prev.aLen + blk.aLen,
        bStart: Math.min(prev.bStart, blk.bStart),
        bLen: prev.bLen + blk.bLen,
      }
      continue
    }

    out.push(blk)
  }

  return out
}

/** 掐掉两头相同的部分 —— 抓包对比十有八九只在中间差几个字节，这一步能把 D 压到个位数 */
function trim<T>(a: T[], b: T[], eq: (x: T, y: T) => boolean): { head: number; tail: number } {
  const n = Math.min(a.length, b.length)

  let head = 0
  while (head < n && eq(a[head], b[head])) head++

  let tail = 0
  while (tail < n - head && eq(a[a.length - 1 - tail], b[b.length - 1 - tail])) tail++

  return { head, tail }
}

function core<T>(a: T[], b: T[], eq: (x: T, y: T) => boolean): DiffResult {
  if (!a.length && !b.length) return { blocks: [], changes: 0, truncated: false }

  const { head, tail } = trim(a, b, eq)
  const midA = a.slice(head, a.length - tail)
  const midB = b.slice(head, b.length - tail)

  const blocks: DiffBlock[] = []
  if (head) blocks.push({ op: 'same', aStart: 0, aLen: head, bStart: 0, bLen: head })

  let truncated = false

  if (midA.length || midB.length) {
    const tooBig = midA.length + midB.length > MAX_UNITS
    const r = tooBig ? { ops: [] as Array<0 | 1 | 2>, ok: false } : myers(midA, midB, eq)

    if (r.ok) {
      blocks.push(...toBlocks(r.ops, head, head))
    } else {
      truncated = true
      blocks.push({
        op: midA.length && midB.length ? 'mod' : midA.length ? 'del' : 'ins',
        aStart: head, aLen: midA.length,
        bStart: head, bLen: midB.length,
      })
    }
  }

  if (tail) {
    blocks.push({ op: 'same', aStart: a.length - tail, aLen: tail, bStart: b.length - tail, bLen: tail })
  }

  //相邻同类合一下（掐头去尾会在接缝处留下两段 same）
  const merged: DiffBlock[] = []
  for (const blk of blocks) {
    const prev = merged[merged.length - 1]
    if (prev && prev.op === blk.op && prev.aStart + prev.aLen === blk.aStart && prev.bStart + prev.bLen === blk.bStart) {
      prev.aLen += blk.aLen
      prev.bLen += blk.bLen
      continue
    }
    merged.push({ ...blk })
  }

  return { blocks: merged, changes: merged.filter((x) => x.op !== 'same').length, truncated }
}

/** 按<b>字节</b>对齐（十六进制模式） */
export function diffBytes(a: Uint8Array, b: Uint8Array): DiffResult {
  return core(Array.from(a), Array.from(b), (x, y) => x === y)
}

/** 按<b>行</b>对齐（文本模式） */
export function diffLines(a: string[], b: string[]): DiffResult {
  return core(a, b, (x, y) => x === y)
}
