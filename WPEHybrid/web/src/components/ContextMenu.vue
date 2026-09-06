<script setup lang="ts">
/*
  自绘右键菜单 —— 全项目第一个，做成通用件给后面 13 份列表用。

  【为什么能做】外壳把 WebView2 的原生右键菜单关掉了
  （core.Settings.AreDefaultContextMenusEnabled = false），
  但 DOM 的 contextmenu 事件<b>照常触发</b>，连 preventDefault 都不用写。

  【为什么不用 ant-design-vue】它没有独立的右键菜单组件，Dropdown 的
  trigger="contextmenu" 要挂在触发元素上，而我们要跟着鼠标坐标走；
  它自带的圆角浅色外观在这套皮肤里也是另一个程序的样子。自己画反而更短。

  【定位】固定在视口坐标（position: fixed）。要防止贴边溢出：
  菜单渲染出来才知道尺寸，所以先放上去、nextTick 量一次、越界就翻到另一侧。
*/
import { computed, nextTick, onBeforeUnmount, ref, watch } from 'vue'
import type { MenuItem } from './menu'

const props = defineProps<{
  /**
   * null = 关。非空 = 在这个视口坐标打开（右键）；
   * 带 anchor = 锚定在那个矩形下方、左对齐（状态条的「设置」按钮这类下拉式菜单），
   * 右边放不下就改成右对齐、下面放不下就翻到矩形上方。
   */
  at: { x: number; y: number; anchor?: { left: number; right: number; top: number; bottom: number } } | null
  items: MenuItem[]
}>()

const emit = defineEmits<{
  (e: 'pick', id: string): void
  (e: 'close'): void
}>()

const box = ref<HTMLElement | null>(null)
const pos = ref({ x: 0, y: 0 })
/** 展开中的二级菜单是哪一项。null = 都收着 */
const openSub = ref<string | null>(null)
/** 二级菜单往左开。主菜单靠右时它会顶出视口，见下面的说明 */
const subLeft = ref(false)

/*
  二级菜单的估算宽度。

  刻意<b>不</b>等它渲染出来再量：那要等到 hover 之后，量完再翻会当着用户的面
  跳一下。这里在主菜单定位时一并算好，宽一点算（真实是 min-width 148 加左右内边距），
  代价只是「本来贴边也放得下时也往左开」，看不出毛病。
*/
const SUB_W = 170

watch(() => props.at, async (at) => {
  openSub.value = null
  if (!at) return

  pos.value = { x: at.x, y: at.y }
  await nextTick()

  //量出来再纠偏：贴着右下角点开时，菜单会整块掉到视口外
  const el = box.value
  if (!el) return

  const r = el.getBoundingClientRect()
  let { x, y } = pos.value
  const a = at.anchor

  if (a) {
    //锚定：贴着矩形底边、左对齐；右边越界改右对齐，下面越界翻到矩形上方
    if (x + r.width > window.innerWidth - 4) x = Math.max(4, a.right - r.width)
    if (y + r.height > window.innerHeight - 4) y = Math.max(4, a.top - r.height - 4)
  } else {
    if (x + r.width > window.innerWidth - 4) x = Math.max(4, x - r.width)
    if (y + r.height > window.innerHeight - 4) y = Math.max(4, y - r.height)
  }

  pos.value = { x, y }

  //主菜单翻到左边之后，二级菜单往右开一样会顶出去 —— 它得跟着翻
  subLeft.value = x + r.width + SUB_W > window.innerWidth - 4
})

function pick(it: MenuItem): void {
  if (it.disabled || it.divider || it.sub) return
  emit('pick', it.id || '')
  emit('close')
}

/*
  关掉的三条路：点别处、按 Esc、滚动。

  滚动那条容易漏 —— 菜单是 fixed 的，页面一滚它就停在原地、指向的行已经变了。
  用捕获阶段监听，滚动容器不冒泡也能收到。
*/
function onKey(e: KeyboardEvent): void {
  if (e.key === 'Escape') emit('close')
}

watch(() => props.at, (at) => {
  if (at) {
    window.addEventListener('keydown', onKey)
    window.addEventListener('scroll', () => emit('close'), { capture: true, once: true })
  } else {
    window.removeEventListener('keydown', onKey)
  }
})

onBeforeUnmount(() => window.removeEventListener('keydown', onKey))

const style = computed(() => ({ left: pos.value.x + 'px', top: pos.value.y + 'px' }))
</script>

<template>
  <!--
    Teleport 到 body：菜单是 fixed 的，但 z-index 只在它所在的层叠上下文里算数 ——
    代理模式的 .proxy 是 z-index: 10 的层叠上下文，底部状态栏是 z-index: 20 的兄弟，
    于是开在页面下半部的菜单会被状态栏盖住一截（真发生过：十六进制面板的右键菜单少了「全选」）。
    挂到 body 上之后它与状态栏、氛围层在同一层里比大小，1500 才真的是 1500。
  -->
  <Teleport to="body">
  <!-- 遮罩只负责收口点击，透明且铺满；菜单自己浮在它上面 -->
  <div v-if="props.at" class="cm-mask" @mousedown.self="emit('close')" @contextmenu.prevent.self="emit('close')">
    <div ref="box" class="cm" :style="style" role="menu">
      <span class="mk tl" /><span class="mk br" />

      <template v-for="(it, i) in props.items">
        <div v-if="it.divider" :key="'d' + i" class="cm-div" />

        <button
          v-else
          :key="it.id || i"
          class="cm-it"
          :class="{ off: it.disabled, danger: it.danger, has: !!it.sub, on: openSub === it.id }"
          :disabled="it.disabled"
          role="menuitem"
          @click="pick(it)"
          @mouseenter="openSub = it.sub ? (it.id || null) : null"
        >
          <svg v-if="it.icon" class="ico" viewBox="0 0 24 24" v-html="it.icon" />
          <span v-else class="ico" />
          <span class="tx">{{ it.label }}</span>
          <svg v-if="it.sub" class="arr" viewBox="0 0 24 24"><path d="M9 6l6 6-6 6" /></svg>

          <!-- 二级菜单。整块贴在父项右侧，鼠标从父项斜着划过去也不会断 -->
          <span v-if="it.sub && openSub === it.id" class="cm sub" :class="{ left: subLeft }">
            <button
              v-for="s in it.sub"
              :key="s.id"
              class="cm-it"
              :class="{ off: s.disabled, danger: s.danger }"
              :disabled="s.disabled"
              role="menuitem"
              @click.stop="pick(s)"
            >
              <svg v-if="s.icon" class="ico" viewBox="0 0 24 24" v-html="s.icon" />
              <span v-else class="ico" />
              <span class="tx">{{ s.label }}</span>
            </button>
          </span>
        </button>
      </template>
    </div>
  </div>
  </Teleport>
</template>

<style scoped>
/*
  z-index 说明：氛围层（.scan / .cn）在 2000 且是 pointer-events: none，
  菜单压在它下面一点点没关系 —— 那两层本来就是「整块屏幕的质感」。
  但要高于弹窗遮罩（999），否则弹窗里将来要用右键菜单就点不到了。
*/
.cm-mask { position: fixed; inset: 0; z-index: 1500; }

.cm {
  position: fixed;
  min-width: 148px;
  padding: 4px 0;
  background: var(--panel);
  border: 1px solid var(--border);
  box-shadow: 0 6px 22px rgb(var(--shadow-rgb) / 55%);
}

/* 四角标记，与设置弹窗同一套记号 */
.mk { position: absolute; width: 6px; height: 6px; border: 1px solid var(--cyan); opacity: .7; }
.mk.tl { top: -1px; left: -1px; border-right: 0; border-bottom: 0; }
.mk.br { right: -1px; bottom: -1px; border-left: 0; border-top: 0; }

.cm-it {
  position: relative;
  display: flex;
  align-items: center;
  gap: 8px;
  width: 100%;
  padding: 5px 12px;
  background: transparent;
  border: 0;
  color: var(--gray);
  font-size: var(--menu-size);
  text-align: left;
  cursor: pointer;
  white-space: nowrap;
}

.cm-it:hover:not(:disabled),
.cm-it.on { background: rgb(var(--cyan-rgb) / 10%); color: var(--cyan); }
.cm-it.danger:hover:not(:disabled) { background: rgb(var(--danger-rgb) / 12%); color: var(--danger); }
.cm-it.off { opacity: .35; cursor: default; }
.cm-it:focus-visible { outline-offset: -2px; }

.cm-it .ico { width: 14px; height: 14px; flex: none; fill: none; stroke: currentColor; stroke-width: 1.6; }
.cm-it .tx { flex: 1; }
.cm-it .arr { width: 12px; height: 12px; flex: none; fill: none; stroke: currentColor; stroke-width: 1.8; }

.cm-div { height: 1px; margin: 4px 0; background: var(--border); }

/* 二级菜单相对父项定位，父项是 position: relative */
.cm.sub {
  position: absolute;
  left: 100%;
  top: -5px;
  display: block;
}

/* 主菜单靠右时往左开，否则会顶出视口 */
.cm.sub.left { left: auto; right: 100%; }
</style>
