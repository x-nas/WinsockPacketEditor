<script setup lang="ts">
/*
  确认框 —— C# 那 96 处 `await UI.Confirm(...)` 全走这一个。

  【为什么自己画】原来用的是 ant-design-vue 的 Modal.confirm，它是这个项目里
  最后一处 antd 组件。浅色圆角、蓝色主按钮，压在这套深色皮肤上就是从别的程序
  飞过来的一块。message / notification / Spin 早已各自换掉（ToastStack、BusyMask），
  这是同一笔账的最后一项。

  【必须每条出口都给出答案】C# 那边是挂在业务调用链上的 await，
  漏一条路就让整条 _Dialog 挂到 AskAsync 的 5 分钟超时。
  所以确定 / 取消 / Esc / 点遮罩 / 组件被卸载，五条路都会 resolve。
  真正的收口在 bridge/host.ts —— 那边保证同一次提问只答一次。

  【键盘】Esc = 取消；Enter 触发当前有焦点的那个按钮，<b>不额外绑</b>。
  打开时焦点落在「取消」上而不是「确定」—— 这些确认框里有删账号、清空列表
  这类不可撤销的操作，默认停在安全的那一侧。
  （曾经在整个弹窗上绑过 Enter = 确定：它会和「焦点在取消上、按 Enter 触发取消」
  同时发生，两条路一起走到 answerConfirm，谁先谁后决定答案。已去掉。）
*/
import { nextTick, ref, watch } from 'vue'
import { confirmState, answerConfirm } from '../bridge/host'
import { t } from '../i18n'
import { useModal } from '../useModal'

const cancelBtn = ref<HTMLElement | null>(null)

watch(() => confirmState.value, async (s) => {
  if (!s) return
  await nextTick()
  cancelBtn.value?.focus()
})

/* 登记进模态栈：底下那层编辑器因此也会 inert。见 useModal.ts */
const { covered } = useModal(() => confirmState.value !== null)
</script>

<template>
  <div
    v-if="confirmState"
    class="mask"
    :inert="covered"
    @contextmenu.prevent
  >
    <div
      class="dlg"
      :class="confirmState.level"
      role="alertdialog"
      aria-modal="true"
      @keydown.esc="answerConfirm(false)"
    >
      <span class="mk tl" /><span class="mk tr" />
      <span class="mk bl" /><span class="mk br" />

      <div class="bd">
        <!-- 图标按级别换色，与 toast 那套一致：成功绿 / 警告琥珀 / 错误红 / 其余青 -->
        <svg class="ico" viewBox="0 0 24 24">
          <circle cx="12" cy="12" r="9" />
          <template v-if="confirmState.level === 'success'">
            <path d="M8 12.5l2.5 2.5L16 9" />
          </template>
          <template v-else>
            <path d="M12 7.5v5.5M12 16.2h.01" />
          </template>
        </svg>

        <div class="tx">
          <div v-if="confirmState.title" class="tt">{{ confirmState.title }}</div>
          <!-- C# 侧的文案带 \r\n，用 pre-wrap 原样保留换行 -->
          <div class="ct">{{ confirmState.content }}</div>
        </div>
      </div>

      <footer class="ft">
        <button ref="cancelBtn" class="btn" @click="answerConfirm(false)">{{ t('dlg.cancel') }}</button>
        <button class="btn primary" @click="answerConfirm(true)">{{ t('dlg.ok') }}</button>
      </footer>
    </div>
  </div>
</template>

<style scoped>
.mask {
  position: fixed;
  inset: 0;
  z-index: 1000;
  background: rgb(var(--scrim-rgb) / 78%);
  backdrop-filter: blur(3px);
  display: flex;
  align-items: center;
  justify-content: center;
}

.dlg {
  position: relative;
  width: min(420px, calc(100vw - 48px));
  background: var(--card);
  border: 1px solid var(--border);
  box-shadow: 0 18px 50px rgb(var(--shadow-rgb) / 60%);
}

/* 四角标记，与设置弹窗、右键菜单同一套记号 */
.mk { position: absolute; width: 7px; height: 7px; border: 1px solid var(--cyan); }
.mk.tl { top: -1px; left: -1px; border-right: 0; border-bottom: 0; }
.mk.tr { top: -1px; right: -1px; border-left: 0; border-bottom: 0; }
.mk.bl { bottom: -1px; left: -1px; border-right: 0; border-top: 0; }
.mk.br { bottom: -1px; right: -1px; border-left: 0; border-top: 0; }

/* 危险级别的四角改成红的 —— 删除类确认一眼能看出不一样 */
.dlg.error .mk { border-color: var(--danger); }
.dlg.warning .mk { border-color: var(--amber); }

.bd { display: flex; gap: 14px; padding: 22px 22px 16px; }

.ico {
  width: 24px;
  height: 24px;
  flex: none;
  fill: none;
  stroke: var(--cyan);
  stroke-width: 1.6;
  stroke-linecap: round;
}

.dlg.success .ico { stroke: var(--green); }
.dlg.warning .ico { stroke: var(--amber); }
.dlg.error .ico { stroke: var(--danger); }

.tx { min-width: 0; }

.tt {
  font-family: var(--share);
  font-size: var(--fs-label);
  letter-spacing: .2em;
  text-transform: uppercase;
  color: var(--muted);
  margin-bottom: 8px;
}

.ct {
  font-size: var(--fs-lead);
  line-height: 1.7;
  color: var(--gray);
  white-space: pre-wrap;
  word-break: break-word;
  /* 确认框里常有路径、账号名，要能选中复制 */
  user-select: text;
}

.ft {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
  padding: 0 22px 20px;
}

.btn {
  min-width: 84px;
  padding: 10px 16px 10px;   /* 上 +1 下 -1：字形在 em 框里偏上 1px（上伸 9 / 下伸 3，实测），补回来 */
  background: transparent;
  border: 1px solid var(--border);
  color: var(--gray);
  font-family: var(--share);
  font-size: var(--btn-size);
  /* 显式 1：Share Tech Mono 在 line-height: normal 下会把行距全压在字的下面，字号一大就明显偏上（实测） */
  line-height: 1;
  letter-spacing: .12em;
  text-transform: uppercase;
  cursor: pointer;
}

.btn:hover { border-color: var(--cyan); color: var(--cyan); }
.btn:focus-visible { outline-offset: -2px; }

.btn.primary { border-color: rgb(var(--green-rgb) / 45%); color: var(--green); }
.btn.primary:hover { background: rgb(var(--green-rgb) / 10%); border-color: var(--green); }

/* 危险确认的主按钮画成红的：它是这个弹窗里唯一会造成后果的按钮 */
.dlg.error .btn.primary,
.dlg.warning .btn.primary { border-color: rgb(var(--danger-rgb) / 45%); color: var(--danger); }

.dlg.error .btn.primary:hover,
.dlg.warning .btn.primary:hover { background: rgb(var(--danger-rgb) / 12%); border-color: var(--danger); }
</style>
