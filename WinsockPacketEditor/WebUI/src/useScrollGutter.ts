import { onBeforeUnmount, onMounted, ref, type Ref } from 'vue'

/*
  表头在滚动容器外的表，表头要补上滚动条占掉的那几像素，两边的列才对得齐。

  以前写死 padding-right: 14 + 10（自绘滚动条 10px）。这在浏览器里对，在 WebView2 里不一定：
  新版 Edge / WebView2 默认开着 Fluent 覆盖式滚动条，scrollbar-gutter: stable 一个像素都不预留，
  于是表头比行窄 10px，弹性列一分摊，后面所有定宽列整体左移 10px（2026-09-14 在客户端列表上撞到）。

  这里实测：offsetWidth − clientWidth 就是容器右侧被滚动条 / 槽位占掉的宽度（覆盖式为 0），
  容器尺寸变了（窗口缩放、内容从不满到满）再量一次。
*/
export function useScrollGutter(body: Ref<HTMLElement | null>): Ref<number> {
  const gutter = ref(0)
  let ro: ResizeObserver | null = null

  function measure(): void {
    const el = body.value
    if (!el) return
    gutter.value = Math.max(0, el.offsetWidth - el.clientWidth)
  }

  onMounted(() => {
    measure()
    if (typeof ResizeObserver !== 'undefined' && body.value) {
      ro = new ResizeObserver(measure)
      ro.observe(body.value)
    }
  })

  onBeforeUnmount(() => { ro?.disconnect(); ro = null })

  return gutter
}
