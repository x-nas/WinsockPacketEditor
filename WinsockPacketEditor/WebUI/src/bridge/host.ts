// 把 C# 侧 IUiHost 的出口接到前端。
//
// 对应 WPEHybrid/Bridge/BridgeUiHost.cs。Operate 里 96 处弹窗全走这几条：
//   ask   confirm { title, content, icon }  -> bool
//   ask   prompt  { formId, arg }           -> object | null
//   event notify  { level, title, content }
//   event toast   { level, text }
//   event busy    { on, text }
//
// 文件框不在这里 —— 那个在 C# 侧弹原生对话框（浏览器拿不到完整路径）。

import { computed, ref, shallowRef } from 'vue'
import { pushToast } from '../stores/toast'
import { on, onAsk } from './index'

/** UiIcon（ClassObject/Ui/IUiHost.cs）。 */
const enum UiIcon {
  None = 0,
  Success = 1,
  Info = 2,
  Warn = 3,
  Error = 4,
}

/** 遮罩状态，由 App.vue 渲染。 */
export const busy = ref({ on: false, text: '' })

/* ────────────────────────────────────────────────────────────────
   确认框

   状态放在这里、渲染在 ConfirmDialog.vue，与 busy / activeForm 同一套路数。
   原来用的是 ant-design-vue 的 Modal.confirm —— 那是全项目最后一处 antd 组件，
   浅色圆角配蓝色主按钮，在这套深色皮肤上像是从别的程序飞过来的。

   【一次提问只答一次】answerConfirm 会先把 pending 取空再 resolve。
   C# 侧是 `await UI.Confirm(...)` 挂在业务调用链上，重复 resolve 不会出错，
   但漏掉一次就会让整条 _Dialog 挂到 AskAsync 的 5 分钟超时 ——
   所以确定 / 取消 / Esc / 点遮罩每条路都要走到这里。
   ──────────────────────────────────────────────────────────────── */

export interface ConfirmState {
  title: string
  content: string
  level: 'success' | 'info' | 'warning' | 'error'
}

export const confirmState = ref<ConfirmState | null>(null)

let pendingConfirm: ((ok: boolean) => void) | null = null

export function answerConfirm(ok: boolean): void {
  const resolve = pendingConfirm
  pendingConfirm = null
  confirmState.value = null

  if (resolve) resolve(ok)
}

/* ────────────────────────────────────────────────────────────────
   表单弹窗注册表

   C# 的 UI.Prompt(formId, arg) 走到这里。与 WinForms 侧的
   WinFormsUiHost.RegisterPrompt 是平级的两套实现 —— 那边按 formId 找工厂建
   UserControl，这边按 formId 找一个返回 Promise 的渲染函数。

   【为什么必须每条路都给出答案】
   C# 那边是 `await UI.Prompt(...)`，挂在业务调用链上。这里若忘了 resolve，
   Operate 的整条 _Dialog 调用链会一直挂到 AskAsync 的 5 分钟超时。
   所以：未登记的 formId 立刻回 null（= 用户取消），而不是静默丢弃。
   ──────────────────────────────────────────────────────────────── */

type FormRenderer = (arg: any) => Promise<any>

const forms = new Map<string, FormRenderer>()

/** 登记一个表单弹窗的渲染方式。组件在 setup 里调一次即可。 */
export function registerForm(formId: string, render: FormRenderer): void {
  forms.set(formId, render)
}

/*
  当前正在渲染的弹窗。

  <b>不导出</b>：组件不是靠读它来决定挂不挂载的 —— 各表单组件在自己的 setup 里
  registerForm 一次，是否显示由它自己拿到的 arg 决定（见 EncryptPassword / BetaNotice）。
  外面唯一需要知道的是「有没有弹窗开着」，那是下面的 modalOpen。
*/
const activeForm = shallowRef<{ id: string; arg: any } | null>(null)

/**
 * 是否有模态弹窗在显示。
 *
 * App.vue 用它给弹窗之外的整块加 <b>inert</b> —— 不加的话 Tab 能从弹窗里
 * 走到标题栏的最小化/最大化/退出上，回车就把程序关了，而 C# 那边还在
 * await 这个弹窗的答案。inert 会同时挡住键盘与鼠标，这才是模态该有的行为。
 *
 * 确认框也算在内 —— 它现在也是自己画的（ConfirmDialog.vue），
 * 没有 antd Modal 自带的那套焦点陷阱，得靠 inert 挡住外面。
 */
export const modalOpen = computed(() => activeForm.value !== null || confirmState.value !== null)

export function attachUiHost(): void {
  onAsk('confirm', (args: { title: string; content: string; icon: number }) => {
    return new Promise<boolean>((resolve) => {
      /*
        上一个还没答就又来一个 —— 理论上不该发生（C# 侧是串行 await），
        但真发生了要先把旧的答掉，否则那条调用链会永远挂着。
        按「取消」结掉是安全的一侧。
      */
      if (pendingConfirm) answerConfirm(false)

      pendingConfirm = resolve

      confirmState.value = {
        title: args.title || '',
        //C# 侧的文案常带首尾的 \r\n（AntdUI 那边靠它撑高度），这里由样式管间距
        content: (args.content || '').trim(),
        level: levelOf(args.icon),
      }
    })
  })

  onAsk('prompt', async (args: { formId: string; arg: unknown }) => {
    const render = forms.get(args.formId)

    if (!render) {
      console.warn('[host] 尚未登记的表单弹窗:', args.formId, args.arg)
      return null
    }

    activeForm.value = { id: args.formId, arg: args.arg }

    try {
      return await render(args.arg)
    } finally {
      activeForm.value = null
    }
  })

  /*
    通知与轻提示都进同一个队列，由 ToastStack 渲染。
    区别只在有没有标题 —— 有标题的停留久一些，见 stores/toast.ts。

    不再用 ant-design-vue 的 notification / message：它们自带浅色圆角胶囊，
    而且 message 固定顶部居中，正好压住自绘标题栏的拖动区。
  */
  on('notify', (d: { level: number; title: string; content: string }) => {
    pushToast(levelOf(d.level), d.content || '', d.title)
  })

  on('toast', (d: { level: number; text: string }) => {
    pushToast(levelOf(d.level), d.text)
  })

  on('busy', (d: { on: boolean; text: string }) => {
    busy.value = { on: d.on, text: d.text || '' }
  })
}

function levelOf(icon: number): 'success' | 'info' | 'warning' | 'error' {
  switch (icon) {
    case UiIcon.Success:
      return 'success'
    case UiIcon.Error:
      return 'error'
    case UiIcon.Warn:
      return 'warning'
    default:
      return 'info'
  }
}
