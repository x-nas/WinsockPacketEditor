<script setup lang="ts">
/*
  设置弹窗的外壳 —— 12 个设置面板共用这一个。

  对应 WinForms 的 AntdUI.Modal（ProxyList 的 ddMenu 那 12 项各弹一个 UserControl）。
  这里不用 ant-design-vue 的 a-modal：它自带一整套圆角 + 亮色边框的观感，
  在这套赛博皮肤里像是从别的程序飞过来的。自己画一个反而更短。

  【为什么单独抽一层】
  剩下 11 个设置面板（进程 / 过滤 / 拦截 / 列表 / 映射 / 外部代理 / 快捷键 /
  备份 / 远程管理 / 防火墙 / 系统）结构完全一样：标题 + 内容 + 保存/取消。
  抽出来之后每个只要写自己的表单。

  【焦点】与 BetaNotice 同一套：打开时给页面其余部分加 :inert，
  否则 Tab 能走到标题栏的退出按钮上，回车就把程序关了。
*/
import { nextTick, ref, watch } from 'vue'
import { t } from '../../i18n'
import { useModal } from '../../useModal'

const props = defineProps<{
  open: boolean
  title: string
  /** 副标题，通常写这一屏对应的 WinForms 控件名，便于对照 */
  subtitle?: string
  busy?: boolean
  /** 校验失败时由父组件填，显示在底部 */
  error?: string
  /**
   * 只读弹窗：藏掉「保存」，把「取消」改成「关闭」。
   * 账号的登录记录就是这种 —— 它只是把一份明细摊开看，没有可保存的东西。
   */
  readonly?: boolean
  /** 弹窗宽度（px）。默认 620；装着表格的那几个（进程 / 映射 / 规则）要宽一些 */
  width?: number
  /** 「保存」那颗按钮的文案。默认「保存」；确认类的弹窗写它真正要做的事（如「启动并注入」）*/
  saveText?: string
  /**
   * 「保存」那颗按钮点不点得动。挑选类的弹窗用得着 ——
   * 进程表里一行都没选中时，「注入」没有对象可注。
   * ⚠️ 与 busy 分开：busy 是「正在办」，这个是「还没得办」，两者都会禁用但含义不同。
   */
  saveDisabled?: boolean
  /** 「取消」那颗按钮的文案。默认「取消」；挑选类的弹窗写「关闭」更准（没什么可取消的）*/
  cancelText?: string
}>()

const emit = defineEmits<{
  (e: 'update:open', v: boolean): void
  (e: 'save'): void
}>()

const box = ref<HTMLElement | null>(null)

/*
  打开时把焦点移进弹窗。

  不这样做的话焦点还留在触发它的那个按钮上，Tab 的第一下会跳到弹窗外面 ——
  而外面此刻是 inert 的，于是焦点直接掉到浏览器地址栏（外壳里就是无处可去）。
*/
watch(() => props.open, async (on) => {
  if (!on) return
  await nextTick()
  const el = box.value?.querySelector<HTMLElement>('input, select, button, [tabindex]')
  el?.focus()
})

function close(): void {
  if (props.busy) return
  emit('update:open', false)
}

/* 登记进模态栈：父窗体因此变 inert；自己被后开的弹窗盖住时也会 inert。见 useModal.ts */
const { covered } = useModal(() => props.open)
</script>

<template>
  <!--
    ⚠️ <b>Teleport 到 body</b> —— 不是为了好看，是必须的，两个理由都在 useModal.ts 里：
    ① 代理模式的 .proxy 是 z-index: 10 的层叠上下文，弹窗留在里面时遮罩盖不住标题栏；
    ② 出去了才不会被 .shell 的 inert 一起禁掉。

    ⚠️ <b>刻意不换行、不重排缩进</b>：模板里有 white-space: pre 的块，
    整体缩进一动，Vue 模板编译器的 condense 会连带改掉渲染结果。

    ⚠️ <b>点遮罩不再关闭弹窗</b>：编辑器里都是填了一半的东西，点空白处就丢掉太容易误操作。
    出口只留「取消 / 关闭」按钮与 Esc。
  -->
  <Teleport to="body"><div v-if="props.open" class="mask" :inert="covered">
    <div ref="box" class="dlg" role="dialog" aria-modal="true" :style="props.width ? { width: props.width + 'px' } : undefined" @keydown.esc="close">
      <header class="hd">
        <span class="mk tl" /><span class="mk tr" />
        <div class="tt">
          <span class="zh">{{ props.title }}</span>
          <span v-if="props.subtitle" class="sub">{{ props.subtitle }}</span>
        </div>
        <button class="x" :title="t('dlg.cancel')" @click="close">
          <svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg>
        </button>
      </header>

      <div class="bd">
        <slot />
      </div>

      <footer class="ft">
        <span v-if="props.error" class="err">
          <svg class="ico" viewBox="0 0 24 24"><circle cx="12" cy="12" r="9" /><path d="M12 8v5M12 16h.01" /></svg>
          {{ props.error }}
        </span>
        <span class="grow" />
        <button class="btn" :class="{ primary: props.readonly }" :disabled="props.busy" @click="close">
          {{ props.cancelText || (props.readonly ? t('dlg.close') : t('dlg.cancel')) }}
        </button>
        <button v-if="!props.readonly" class="btn primary" :disabled="props.busy || props.saveDisabled" @click="emit('save')">
          {{ props.busy ? t('proxy.working') : (props.saveText || t('set.save')) }}
        </button>
        <span class="mk bl" /><span class="mk br" />
      </footer>
    </div>
  </div></Teleport>
</template>

<style scoped>
.mask {
  position: fixed;
  inset: 0;
  z-index: 999;
  background: rgb(var(--scrim-rgb) / 78%);
  backdrop-filter: blur(3px);
  display: flex;
  align-items: center;
  justify-content: center;
}

.dlg {
  position: relative;
  width: 620px;
  max-width: calc(100vw - 64px);
  max-height: calc(100vh - 120px);
  display: flex;
  flex-direction: column;
  background: var(--card);
  border: 1px solid var(--border);
  box-shadow: 0 18px 60px rgb(var(--shadow-rgb) / 55%);
}

/* 四角标记：与启动页、测试版提示同一种做法，让弹窗也属于这套语言 */
.mk { position: absolute; width: 11px; height: 11px; border: 1px solid rgb(var(--cyan-rgb) / 45%); }
.mk.tl { top: 6px; left: 6px; border-right: 0; border-bottom: 0; }
.mk.tr { top: 6px; right: 6px; border-left: 0; border-bottom: 0; }
.mk.bl { bottom: 6px; left: 6px; border-right: 0; border-top: 0; }
.mk.br { bottom: 6px; right: 6px; border-left: 0; border-top: 0; }

.hd {
  position: relative;
  flex: none;
  display: flex;
  align-items: center;
  gap: 12px;
  padding: 14px 16px 13px 20px;
  border-bottom: 1px solid var(--border);
  background: var(--panel);
}

.tt { flex: 1; min-width: 0; display: flex; align-items: baseline; gap: 12px; }

.tt .zh {
  font-family: var(--orbit);
  font-weight: 700;
  font-size: var(--fs-title);
  letter-spacing: .04em;
  color: var(--cyan);
}

.tt .sub {
  font-family: var(--share);
  font-size: var(--fs-caption);
  letter-spacing: .14em;
  text-transform: uppercase;
  color: var(--muted);
}

.x {
  flex: none;
  width: 26px;
  height: 26px;
  border: 0;
  background: transparent;
  color: var(--muted);
  cursor: pointer;
  display: flex;
  align-items: center;
  justify-content: center;
}

.x:hover { color: var(--danger); }
.x .ico { width: 15px; height: 15px; stroke: currentColor; stroke-width: 2; fill: none; }
.x:focus-visible { outline-offset: -2px; outline-color: var(--danger); }

.bd { flex: 1; min-height: 0; overflow-y: auto; padding: 4px 0; }

.ft {
  position: relative;
  flex: none;
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 13px 20px;
  border-top: 1px solid var(--border);
  background: var(--panel);
}

.ft .grow { flex: 1; }

/*
  ⚠️ 错误文字可以很长（比如「远程管理启动失败：… 不是本机的地址（可能换了网络），请重新选择监听地址」），
  它要<b>自己折行</b>，不能去挤右边的按钮 —— 原来按钮没写 flex: none，一句长错误就把「取消 / 保存」
  压成竖排的两个字（2026-09-11 远程管理那一轮实测撞到）。所以这里 min-width: 0 允许收缩、按钮那边 flex: none。
*/
.err {
  display: inline-flex;
  align-items: center;
  gap: 7px;
  flex: 0 1 auto;
  min-width: 0;
  font-size: var(--fs-small);
  line-height: 1.5;
  color: var(--danger);
}

.err .ico { width: 14px; height: 14px; stroke: currentColor; stroke-width: 2; fill: none; flex: none; }

.btn {
  flex: none;                /* 页脚的错误文字再长也不许挤压按钮（见 .err） */
  white-space: nowrap;
  padding: 11.25px 20px 10.75px;   /* 上多四分之一像素、高度不变：原来 100% 缩放下「保存 / 取消」偏高 1px，11.5 / 10.5 又让 125% 偏低 1px，取中间（2026-09-13 字体度量覆写后按 100% 缩放实测重调） */
  background: transparent;
  border: 1px solid var(--border);
  color: var(--gray);
  font-family: var(--share);
  font-size: var(--btn-size);
  /* 显式 1：Share Tech Mono 在 line-height: normal 下会把行距全压在字的下面，字号一大就明显偏上（实测） */
  line-height: 1;
  letter-spacing: .14em;
  text-transform: uppercase;
  cursor: pointer;
  transition: .15s;
}

.btn:hover:not(:disabled) { border-color: var(--cyan); color: var(--cyan); }
.btn:disabled { opacity: .4; cursor: default; }
.btn.primary { border-color: rgb(var(--green-rgb) / 45%); color: var(--green); }
.btn.primary:hover:not(:disabled) { background: rgb(var(--green-rgb) / 10%); }
.btn.primary:focus-visible { outline-color: var(--green); }
</style>
