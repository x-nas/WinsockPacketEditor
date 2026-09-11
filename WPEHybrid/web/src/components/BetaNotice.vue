<script setup lang="ts">
/*
  测试版提示 —— 对应 WinForms 的 UiDialogs.ShowBetaMessage。

  它是<b>第一个走通弹窗通道的界面</b>：C# 侧 UI.Prompt("beta-notice", …) 发起，
  等这里点「知道了」才返回。后面 17 个编辑弹窗照同一套注册机制加。

  样式取自官网的 .wip 建设中面板（工业警示斜纹 + 四角定位标记 + 日志块），
  比一个感叹号图标更贴这套语言。
*/
import { onMounted, ref } from 'vue'
import { registerForm } from '../bridge/host'
import { t } from '../i18n'
import { useModal } from '../useModal'

interface BetaArg {
  title: string
  content: string
  ok: string
}

const open = ref(false)
const arg = ref<BetaArg | null>(null)

/** 点了「知道了」之后用来结束 C# 那边的 await。 */
let done: (() => void) | null = null

onMounted(() => {
  registerForm('beta-notice', (a: BetaArg) => {
    arg.value = a
    open.value = true

    return new Promise<null>((resolve) => {
      done = () => {
        open.value = false
        done = null
        // 回 null：这个弹窗没有结果值，C# 侧 UI.Prompt<object> 拿到 null 即可
        resolve(null)
      }
    })
  })
})

function acknowledge(): void {
  // 必须走 done()：直接改 open 会让 C# 那边一直 await 到 5 分钟超时
  done?.()
}

/** 内容里的换行按行拆开，C# 传的是 \r\n。 */
function lines(s: string): string[] {
  return (s || '').split(/\r?\n/)
}

/* 登记进模态栈，见 useModal.ts */
const { covered } = useModal(() => open.value)
</script>

<template>
  <div v-if="open" class="mask" :inert="covered">
    <div class="wip">
      <div class="wip-in">
        <span class="mk tl" /><span class="mk tr" /><span class="mk bl" /><span class="mk br" />

        <span class="wip-tag"><i />Beta Build</span>

        <h3>{{ arg?.title }}</h3>

        <p class="lead">
          <span v-for="(l, i) in lines(arg?.content ?? '')" :key="i">{{ l }}<br></span>
        </p>

        <div class="wip-log">
          <span class="l"><b>warning</b> {{ t('beta.warn') }}<span class="cur" /></span>
        </div>

        <button class="wip-btn" autofocus @click="acknowledge">
          {{ arg?.ok || t('beta.ok') }} // Acknowledge
        </button>
      </div>
    </div>
  </div>
</template>

<style scoped>
.mask {
  position: fixed;
  inset: 0;
  z-index: 999;
  background: rgb(var(--scrim-rgb) / 82%);
  backdrop-filter: blur(3px);
  display: flex;
  align-items: center;
  justify-content: center;
}

.wip {
  position: relative;
  width: 560px;
  padding: 7px 0;
  overflow: hidden;
  border: 1px solid rgb(var(--amber-rgb) / 32%);
  background: linear-gradient(180deg, rgb(var(--amber-rgb) / 7%), rgb(var(--amber-rgb) / 0%) 58%), var(--card);
}

/* 上下两条工业警示斜纹 */
.wip::before,
.wip::after {
  content: "";
  position: absolute;
  left: 0;
  right: 0;
  height: 7px;
  background: repeating-linear-gradient(45deg, rgb(var(--amber-rgb) / 80%) 0 9px, rgb(var(--chrome-rgb) / 92%) 9px 18px);
}

.wip::before { top: 0; }
.wip::after { bottom: 0; }

.wip-in { position: relative; padding: 28px 30px 26px; }

/* 四角定位标记 */
.wip-in > .mk { position: absolute; width: 13px; height: 13px; border: 1px solid rgb(var(--amber-rgb) / 55%); }
.wip-in > .mk.tl { top: 8px; left: 8px; border-right: 0; border-bottom: 0; }
.wip-in > .mk.tr { top: 8px; right: 8px; border-left: 0; border-bottom: 0; }
.wip-in > .mk.bl { bottom: 8px; left: 8px; border-right: 0; border-top: 0; }
.wip-in > .mk.br { bottom: 8px; right: 8px; border-left: 0; border-top: 0; }

.wip-tag {
  display: inline-flex;
  align-items: center;
  gap: 8px;
  padding: 4px 11px;
  margin-bottom: 15px;
  border: 1px solid rgb(var(--amber-rgb) / 45%);
  font-family: var(--share);
  font-size: var(--fs-caption);
  letter-spacing: .2em;
  text-transform: uppercase;
  color: var(--amber);
}

.wip-tag i { width: 7px; height: 7px; background: var(--amber); animation: beat 1.1s steps(1) infinite; }

@keyframes beat { 50% { opacity: .25; } }

.wip h3 {
  font-family: var(--orbit);
  font-size: var(--fs-num-lg);
  font-weight: 700;
  letter-spacing: .05em;
  color: var(--gray);
  margin-bottom: 12px;
}

.wip .lead { margin: 0 0 18px; font-size: var(--fs-lead); color: var(--muted); }

.wip-log {
  margin: 0 0 20px;
  padding: 13px 16px;
  border: 1px solid var(--border);
  background: rgb(var(--inset-rgb) / 34%);
  font-size: var(--fs-body);
  color: var(--muted);
}

.wip-log b { font-weight: 400; color: var(--amber); }
.wip-log .cur::after { content: "_"; margin-left: 2px; color: var(--amber); animation: beat 1s steps(1) infinite; }

.wip-btn {
  display: block;
  width: 100%;
  padding: 16px 0;   /* 12px 时字形已居中（实测），不需要 10.5px 那套上 +1 下 -1 的补偿，补了反而低 1px */
  background: transparent;
  border: 2px solid var(--amber);
  color: var(--amber);
  font-family: var(--share);
  font-size: var(--fs-label);   /* 刻意不走 --btn-size：这是整屏唯一的一个按钮，且要压得住上面 18px 的标题 */
  /* 显式 1：Share Tech Mono 在 line-height: normal 下会把行距全压在字的下面，字号一大就明显偏上（实测） */
  line-height: 1;
  letter-spacing: .2em;
  text-transform: uppercase;
  cursor: pointer;
  transition: .15s;
}

.wip-btn:hover { background: rgb(var(--amber-rgb) / 12%); box-shadow: 0 0 16px rgb(var(--amber-rgb) / 25%); }

/*
  这个弹窗整体是琥珀色调，全局那圈绿色焦点环在这儿不搭。
  换成与 hover 相同的表现 —— 键盘用户照样看得出焦点在哪，观感又统一。
*/
.wip-btn:focus-visible {
  outline: none;
  background: rgb(var(--amber-rgb) / 12%);
  box-shadow: 0 0 16px rgb(var(--amber-rgb) / 25%);
}
</style>
