<script setup lang="ts">
/*
  遮罩 —— 对应 C# 的 UI.Busy(文案, 工作体)。

  【为什么不用 ant-design-vue 的 a-spin】
  两条都栽过：
    ① 它渲染出来的外层 div 留在 .win 的 flex 流里，遮罩一出现就把整页顶一下；
       给外层补了 position: fixed 才治住。
    ② 内层的 .ant-spin 结构在 v4 下不吃我们设的 flex 居中，转圈跑到了屏幕顶部。
       要治得去 :deep 它的内部类名，还得盯着版本升级。
  而且那套蓝色圆点与这里的配色完全不搭。自己画反而更短、行为可预期。

  目前只有「开始代理」在用（SuperSocket 的 Setup/Start 是同步阻塞的），
  停止是同步返回的，不走这里。
*/
defineProps<{ text?: string }>()
</script>

<template>
  <div class="busy" role="alert" aria-busy="true">
    <div class="box">
      <!--
        两层反向旋转的方环，不是圆点 —— 与这套界面的方角语言一致。
        纯 CSS，没有额外资源。
      -->
      <span class="ring">
        <i class="a" />
        <i class="b" />
      </span>

      <span v-if="text" class="tx">{{ text }}</span>
    </div>
  </div>
</template>

<style scoped>
.busy {
  position: fixed;
  inset: 0;
  z-index: 1200;
  display: flex;
  align-items: center;
  justify-content: center;
  background: rgb(var(--scrim-rgb) / 72%);
  backdrop-filter: blur(2px);
}

.box {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 16px;
}

.ring {
  position: relative;
  width: 46px;
  height: 46px;
  display: block;
}

.ring i {
  position: absolute;
  inset: 0;
  border: 2px solid transparent;
  display: block;
}

/* 外环顺时针，只画两条对角边，转起来是一段追着一段的效果 */
.ring .a {
  border-top-color: var(--green);
  border-bottom-color: var(--green);
  animation: spin 1.1s linear infinite;
}

/* 内环反向且慢一点，两层错开才不像单调的匀速圆圈 */
.ring .b {
  inset: 8px;
  border-left-color: var(--cyan);
  border-right-color: var(--cyan);
  animation: spin 1.6s linear infinite reverse;
}

@keyframes spin {
  to { transform: rotate(360deg); }
}

.tx {
  font-family: var(--share);
  font-size: 11px;
  letter-spacing: .22em;
  text-transform: uppercase;
  color: var(--green);
  text-shadow: 0 0 10px rgb(var(--green-rgb) / 35%);
}

/* 关掉动效时不转，但要留下「正在忙」的静态形态 */
@media (prefers-reduced-motion: reduce) {
  .ring .a,
  .ring .b { animation: none; }
}
</style>
