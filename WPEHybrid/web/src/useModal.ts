/*
  模态弹窗的登记处 —— 全项目一份。

  【为什么需要它】
  弹窗打开时父窗体必须彻底不可交互：既点不动（标题栏的最小化 / 最大化 / 退出、
  侧栏、各页的按钮），Tab 也走不进去。原先只有 C# 那边发起的确认框 / 表单弹窗
  （bridge/host.ts 的 modalOpen）能让 .shell 变 inert，页面自己的编辑器与设置弹窗
  一个都不算 —— 于是弹着「发送编辑」还能点右上角的最大化。

  【为什么弹窗要 Teleport 到 body，不能只靠 z-index】
  ⚠️ 代理模式的 .proxy 是 `position: relative; z-index: 10` 的<b>层叠上下文</b>，
  弹窗渲染在它里面，那层 z-index: 999 的遮罩就被<b>关在这个上下文里</b>；
  而标题栏是 .shell 里 z-index: 20 的兄弟 —— 结果标题栏画在弹窗<b>之上</b>，
  遮罩既盖不住它、也挡不住点击。右键菜单当年踩的是同一个坑（见 CLAUDE.md）。

  ⚠️ 另一半同样重要：弹窗 Teleport 出去之后就<b>不在 .shell 里</b>了，
  这样给 .shell 加 inert 才不会把弹窗自己也禁掉。两件事是配套的，别只做一件。

  【弹窗套弹窗】
  确认框弹在编辑器上面时，下面那层编辑器也该 inert（它已经不在 .shell 里了，
  inert 盖不到它）。所以每个弹窗自己也要 `:inert="covered"`。
*/
import { computed, onScopeDispose, ref, watch } from 'vue'

let seq = 0
const stack = ref<number[]>([])

/** 有没有模态弹窗开着。App.vue 用它给 .shell 整块加 inert。 */
export const anyModalOpen = computed(() => stack.value.length > 0)

/**
 * 把一个弹窗登记进来。
 *
 * @param isOpen 取当前是否显示 —— 传取值函数，别传布尔值（那样只会读一次）
 * @returns covered 自己被后开的弹窗盖住了，这时自己也要 inert
 */
export function useModal(isOpen: () => boolean) {
  const id = ++seq

  const set = (v: boolean) => {
    const has = stack.value.includes(id)
    if (v === has) return
    stack.value = v ? [...stack.value, id] : stack.value.filter((x) => x !== id)
  }

  watch(isOpen, set, { immediate: true })
  onScopeDispose(() => set(false))

  return {
    covered: computed(() => {
      const s = stack.value
      return s.includes(id) && s[s.length - 1] !== id
    }),
  }
}
