// JS 侧 JSON-RPC 客户端：封装与 C# WebBridge 的通信。
//
// 与 WPEProxyCap 的 bridge.ts 相比多了一个方向：
//   call(m, a)   JS → C#   调 Operate 的方法，等结果
//   on(n, cb)    C# → JS   单向推送（封包批次、通知、遮罩…）
//   onAsk(m, h)  C# → JS   C# 发起提问、等前端回答 ← Hybrid 没有这一路
//
// 第三条是 WPE 特有的：B0–B8 把 96 处弹窗收口到 IUiHost，它们全都要等用户的答案。
// 对应 C# 侧 WPEHybrid/Bridge/WebBridge.cs。

type Pending = { resolve: (v: any) => void; reject: (e: any) => void }
type AskHandler = (args: any) => any | Promise<any>

const webview = (window as any).chrome?.webview

const pending = new Map<string, Pending>()
const listeners = new Map<string, Set<(data: any) => void>>()
const askHandlers = new Map<string, AskHandler>()

let seq = 0

/** 是否跑在 WebView2 宿主里。普通浏览器里打开时为 false，界面应降级而不是崩。 */
export const inHost = !!webview

if (webview) {
  webview.addEventListener('message', (e: MessageEvent) => {
    const msg: any = e.data
    if (!msg?.type) return

    // C# 对 call 的应答
    if (msg.type === 'result') {
      const p = pending.get(msg.id)
      if (!p) return
      pending.delete(msg.id)
      msg.ok ? p.resolve(msg.result) : p.reject(new Error(msg.error || '未知错误'))
      return
    }

    // C# 发起的提问，答完要回
    if (msg.type === 'ask') {
      const h = askHandlers.get(msg.method)
      if (!h) {
        answer(msg.id, false, null, `前端未实现: ${msg.method}`)
        return
      }
      Promise.resolve()
        .then(() => h(msg.args))
        .then((r) => answer(msg.id, true, r, null))
        .catch((err) => answer(msg.id, false, null, String(err)))
      return
    }

    // C# 单向推送
    if (msg.type === 'event') {
      const set = listeners.get(msg.name)
      if (!set) return
      // 拷一份再遍历：回调里可能反过来 off 掉自己
      for (const cb of [...set]) {
        try {
          cb(msg.data)
        } catch (err) {
          console.error(`[bridge] 事件处理器抛错: ${msg.name}`, err)
        }
      }
    }
  })
}

/** 调 C# 的方法。仅在 WebView2 宿主内可用。 */
export function call<T = any>(method: string, args: Record<string, unknown> = {}): Promise<T> {
  if (!webview) {
    return Promise.reject(new Error(`[bridge] 缺少 WebView2 宿主，无法调用 ${method}`))
  }

  const id = `c${++seq}`
  return new Promise<T>((resolve, reject) => {
    pending.set(id, { resolve, reject })

    /*
      postMessage 抛了就把这条从在途表里摘掉再 reject。

      不这么写的话，这个 Promise <b>永远不会 settle</b>：登记在先、发送在后，
      发送失败就再也不会有 result 回来。调用方那句 await 就此挂住 ——
      表现是「按了保存没反应、也不报错」，而 C# 侧压根没收到过这条调用。
      （C# 那个方向有超时 + FailAllPending 兜着，这边没有，见 WebBridge.AskAsync。）

      <b>刻意不加统一超时</b>：这条通道上有一批调用天生就要等人 ——
      导入导出会弹原生文件框、账号导入还要先问密码，几分钟都算正常。
      一刀切的超时会把这些正常路径判成失败。
    */
    try {
      webview.postMessage({ type: 'call', id, method, args })
    } catch (err) {
      pending.delete(id)
      reject(err instanceof Error ? err : new Error(String(err)))
    }
  })
}

/** 订阅 C# 的单向推送。返回取消订阅的函数。 */
export function on(event: string, cb: (data: any) => void): () => void {
  let set = listeners.get(event)
  if (!set) {
    set = new Set()
    listeners.set(event, set)
  }
  set.add(cb)
  return () => set!.delete(cb)
}

/** 登记一个 C# 提问的处理器（confirm / prompt）。 */
export function onAsk(method: string, handler: AskHandler): void {
  askHandlers.set(method, handler)
}

function answer(id: string, ok: boolean, result: unknown, error: string | null): void {
  webview.postMessage({ type: 'answer', id, ok, result, error })
}
