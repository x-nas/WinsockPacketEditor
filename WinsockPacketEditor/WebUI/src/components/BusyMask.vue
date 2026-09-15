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
        三格音量条 —— 与官网 WPEWeb 的加载动画是<b>同一个</b>件，
        取值逐字照抄 assets/css/cyber.css 的 .loader（见下面样式块的说明）。
      -->
      <span class="loader"><i /><i /><i /></span>

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
  /* 动画与文案直接画在遮罩上（没有卡片），所以用 --veil 而不是 --scrim：浅色下是磨砂，见 tokens.css */
  background: rgb(var(--veil-rgb) / 72%);
  backdrop-filter: blur(2px);
}

.box {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 16px;
}

/*
  ⚠️ 这几条是<b>官网 WPEWeb/assets/css/cyber.css 的 .loader 逐字搬过来的</b>
  （宽 6 / 高 20 / 缝 5 / 1s ease-in-out / 三格错开 0 · .18s · .36s / 0 0 8px 辉光），
  官网首页那四格统计的加载态就是它。改动这里等于让两边不一样，要改就两边一起改。

  早先这里是「两层反向旋转的方环」，形态上没问题，但网站与程序各有一套加载动画
  —— 同一个产品该只有一种「正在忙」的样子。

  颜色走 currentColor（官网也是），所以由下面 .loader 那句 color 决定；
  与文案同为 --green，两者是一组。
*/
.loader {
  display: inline-flex;
  gap: 5px;
  align-items: flex-end;
  height: 26px;
  min-width: 44px;
  color: var(--green);
}

.loader i {
  width: 6px;
  height: 20px;
  background: currentColor;
  box-shadow: 0 0 8px currentColor;
  transform-origin: bottom;
  animation: eq 1s ease-in-out infinite;
}

.loader i:nth-child(2) { animation-delay: .18s; }
.loader i:nth-child(3) { animation-delay: .36s; }

@keyframes eq {
  0%, 100% { transform: scaleY(.35); opacity: .45; }
  50% { transform: scaleY(1); opacity: 1; }
}

.tx {
  font-family: var(--share);
  font-size: var(--fs-label);
  letter-spacing: .22em;
  text-transform: uppercase;
  color: var(--green);
  text-shadow: 0 0 10px rgb(var(--green-rgb) / 35%);
}

/* 关掉动效时不动，但要留下「正在忙」的静态形态（这一条官网也有，取值相同）*/
@media (prefers-reduced-motion: reduce) {
  .loader i {
    animation: none;
    transform: scaleY(.7);
    opacity: .7;
  }
}
</style>
