<script setup lang="ts">
/*
  轻提示 / 通知的浮层。

  【位置】右下角。这一处换过两轮，理由都是「别挡住接下来要点的东西」：

    顶部居中  ant-design-vue 的 message 默认位置 —— 压住自绘标题栏的拖动区，
              提示一出来就拖不动窗口。
    右上      压住「停止代理 / 清空 / 设置」，而那正是提示出现后最可能去点的三个按钮。
    右下      落在十六进制面板上，那一片是纯文本显示，没有任何可点的东西。

  为什么不放左边：侧栏是 14 项真会用的导航，挡住它和挡按钮是一回事。

  【形态】与状态条上那排按钮同一套：方角、发丝边框、深色半透明 + 模糊。
  左侧一条 3px 的等级色竖条 —— 它是唯一的彩色元素，扫一眼就知道是成功还是出错，
  不用读字。
*/
import { t } from '../i18n'
import { dismissToast, toasts, type ToastLevel } from '../stores/toast'

const ICON: Record<ToastLevel, string> = {
  success: '<path d="M20 6L9 17l-5-5"/>',
  info: '<circle cx="12" cy="12" r="9"/><path d="M12 11v5M12 8h.01"/>',
  warning: '<path d="M12 3l9 16H3z"/><path d="M12 9v4M12 16h.01"/>',
  error: '<circle cx="12" cy="12" r="9"/><path d="M15 9l-6 6M9 9l6 6"/>',
}
</script>

<template>
  <!--
    TransitionGroup 让新条从右侧滑入、旧条淡出，并且在中间某条被移除时
    让下面的条平滑上移（靠 .tst-move）—— 直接删节点会“跳”一下。
  -->
  <TransitionGroup name="tst" tag="div" class="tst-wrap">
    <div
      v-for="x in toasts"
      :key="x.id"
      class="tst"
      :class="x.level"
      role="status"
      :title="t('toast.dismiss')"
      @click="dismissToast(x.id)"
    >
      <span class="bar" />

      <svg class="ico" viewBox="0 0 24 24" v-html="ICON[x.level]" />

      <div class="body">
        <div v-if="x.title" class="tt">{{ x.title }}</div>
        <div class="tx" :class="{ solo: !x.title }">{{ x.text }}</div>
      </div>
    </div>
  </TransitionGroup>
</template>

<style scoped>
/*
  位置换过三轮，每次都是同一条标准：<b>别挡住接下来要点的东西</b>。

    顶部居中 → 压住自绘标题栏的拖动区，提示一出来就拖不动窗口
    右上     → 压住「停止代理 / 清空 / 设置」，正是提示出现后最可能点的三个
    右下     → 主界面上落在十六进制面板，那一片没有可点的东西 —— 一直用到做出大编辑器
    底部居中 → 现在这个

  右下之所以不能用了：<b>每个弹窗的操作按钮都在右下</b>（取消 / 保存），
  发送编辑导入导出时，提示正好盖住它们。这不是某一屏的巧合 ——
  FilterEdit / SendEdit / SettingsModal / ConfirmDialog 的页脚一律右对齐，
  是全项目的约定。所以该动的是提示，不是按钮。

  底部居中同时避开了两处：
    · 弹窗页脚的<b>正中间是空的</b> —— 错误文案在最左、按钮在最右，中间只有一根撑杆；
    · 主界面上仍然落在十六进制面板，与右下同样没有可点的东西。
  左下不行：那儿是快捷面板，四个页签和每一行都能点。
*/
.tst-wrap {
  position: fixed;
  /* 状态栏 30px + 一点余量；右下角（用户定的位置，2026-09-04 从底部居中改回来）*/
  bottom: 46px;
  right: 16px;
  z-index: 1010;
  display: flex;
  /* 从下往上堆：最新的一条离底最近，眼睛不用回头找 */
  flex-direction: column-reverse;
  gap: 8px;
  align-items: flex-end;
  /* 容器本身不吃事件，否则会挡住底下整块区域的点击 */
  pointer-events: none;
}

.tst {
  position: relative;
  display: flex;
  align-items: flex-start;
  gap: 10px;
  min-width: 240px;
  max-width: 420px;
  padding: 10px 14px 10px 16px;
  background: rgb(var(--chrome-rgb) / 94%);
  backdrop-filter: blur(8px);
  border: 1px solid var(--border);
  box-shadow: 0 8px 28px rgb(var(--shadow-rgb) / 55%);
  cursor: pointer;
  /* 条目自己要能点（点一下关掉），所以在这里把事件收回来 */
  pointer-events: auto;
}

/* 左侧等级色竖条：唯一的彩色元素，不读字也知道是什么级别 */
.bar {
  position: absolute;
  top: -1px;
  bottom: -1px;
  left: -1px;
  width: 3px;
}

.ico { width: 15px; height: 15px; stroke-width: 2; fill: none; flex: none; margin-top: 1px; stroke-linecap: round; stroke-linejoin: round; }

.body { min-width: 0; }

.tt {
  font-family: var(--share);
  font-size: var(--fs-label);
  letter-spacing: .12em;
  text-transform: uppercase;
  margin-bottom: 3px;
}

.tx { font-size: var(--fs-body); line-height: 1.5; color: var(--gray); word-break: break-word; }

/* 没有标题时正文就是主角，给它等级色 */
.tx.solo { color: inherit; }

.tst.success { color: var(--green); border-color: rgb(var(--green-rgb) / 40%); }
.tst.success .bar { background: var(--green); box-shadow: 0 0 8px var(--green); }

.tst.info { color: var(--cyan); border-color: rgb(var(--cyan-rgb) / 40%); }
.tst.info .bar { background: var(--cyan); box-shadow: 0 0 8px var(--cyan); }

.tst.warning { color: var(--amber); border-color: rgb(var(--amber-rgb) / 40%); }
.tst.warning .bar { background: var(--amber); box-shadow: 0 0 8px var(--amber); }

.tst.error { color: var(--danger); border-color: rgb(var(--danger-rgb) / 40%); }
.tst.error .bar { background: var(--danger); box-shadow: 0 0 8px var(--danger); }

/* ── 进出场 ─────────────────────────────────────────── */
.tst-enter-active,
.tst-leave-active { transition: opacity .18s, transform .18s; }

.tst-enter-from { opacity: 0; transform: translateX(16px); }
.tst-leave-to { opacity: 0; transform: translateX(16px); }

/* 中间某条消失时，下面的条平滑上移而不是瞬间跳 */
.tst-leave-active { position: absolute; right: 0; }
.tst-move { transition: transform .18s; }

@media (prefers-reduced-motion: reduce) {
  .tst-enter-active,
  .tst-leave-active,
  .tst-move { transition: none; }
}
</style>
