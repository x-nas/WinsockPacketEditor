<script setup lang="ts">
/*
  批量 / 多选解码的进度遮罩。

  与 BusyMask 分开：那个是 C# 的 UI.Busy（忙但没进度），这个是带进度条与「取消」的
  解码任务。两者外观同源（同样的 veil 底 + 绿色），只是这里多一条进度与一个按钮。
*/
import { computed } from 'vue'
import { t } from '../i18n'
import { cancelDecodeJob, decodeJob } from '../stores/decodeJob'

const pct = computed(() => {
  const j = decodeJob.value
  return j && j.total > 0 ? Math.min(100, Math.round((j.done * 100) / j.total)) : 0
})
</script>

<template>
  <div v-if="decodeJob" class="dj" role="alert" aria-busy="true">
    <div class="box">
      <div class="ttl">{{ t('proxy.working') }}</div>
      <div class="bar"><i :style="{ width: pct + '%' }" /></div>
      <div class="num">{{ decodeJob.done }} / {{ decodeJob.total }}</div>
      <button class="cbtn" @click="cancelDecodeJob">{{ t('dlg.cancel') }}</button>
    </div>
  </div>
</template>

<style scoped>
.dj {
  position: fixed;
  inset: 0;
  z-index: 1200;
  display: flex;
  align-items: center;
  justify-content: center;
  background: rgb(var(--veil-rgb) / 72%);
  backdrop-filter: blur(2px);
}

.box {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 12px;
  min-width: 280px;
}

.ttl {
  font-family: var(--share);
  font-size: var(--fs-label);
  letter-spacing: .22em;
  text-transform: uppercase;
  color: var(--green);
  text-shadow: 0 0 10px rgb(var(--green-rgb) / 35%);
}

.bar {
  width: 240px;
  height: 6px;
  border: 1px solid var(--border);
  background: rgb(var(--inset-rgb) / 40%);
}

.bar i {
  display: block;
  height: 100%;
  background: var(--green);
  box-shadow: 0 0 8px rgb(var(--green-rgb) / 50%);
  transition: width .12s linear;
}

.num { font-family: var(--mono); font-size: var(--fs-small); color: var(--gray); }

.cbtn {
  padding: 6px 20px;
  border: 1px solid var(--border);
  background: transparent;
  color: var(--gray);
  cursor: pointer;
  font-size: var(--fs-small);
}
.cbtn:hover { color: var(--danger); border-color: var(--danger); }
</style>
