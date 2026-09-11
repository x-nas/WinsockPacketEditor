<script setup lang="ts">
/*
  加密导入 / 导出的密码框 —— 对应 WinForms 的 Controls/EncryptionPassword。

  C# 侧走 UI.Prompt，两个 formId：
    encrypt-export  导出时设密码（PasswordAsk.FilePath 为空）
    encrypt-import  导入时输密码（PasswordAsk.FilePath 是那个加密文件）

  【契约】返回非 null 就<b>必须</b>带非空密码；用户放弃则回 null。
  「密码不合格就别关窗」这个循环归宿主实现管，业务侧只看到「拿到密码」或「放弃」两种结果
  （见 ClassObject/Ui/PasswordResult.cs 的说明）。

  ── 与 WinForms 那个框的四处不同 ──────────────────────────────

  ① <b>导出多一个「再输一次」。</b>设错了密码的代价是这份文件永远打不开，
     而设密码时没有任何东西能校验你输的是不是你以为的那个。原来只有一个输入框。

  ② <b>导入当场验密码，不对就留在框里重输。</b>原来是关窗之后才由 Operate 去解密，
     密码错了整条流程打回原点，得「重新点导入 → 重新选文件 → 再输一遍」。
     现在按下解锁先走一次 verifyEncryptPassword，不对就只是红一行字。
     这也是 PasswordAsk 要多带 FilePath 的唯一原因。

  ③ <b>导出的「不加密」是一个明写的按钮</b>，不再靠「点取消 = 不加密」这种暗规则。
     那条暗规则很危险：用户以为自己取消了整个导出，实际导出了一份谁都能打开的明文。

  ④ 有显示/隐藏切换和大写锁定提示 —— 密码框该有的两样。
*/
import { computed, nextTick, onMounted, ref } from 'vue'
import { call } from '../bridge'
import { registerForm } from '../bridge/host'
import { t } from '../i18n'
import { useModal } from '../useModal'

interface Ask {
  Title: string
  /** 只有导入才有值 —— 用它当场验密码 */
  FilePath: string | null
}

/** null = 关。'export' / 'import' = 开着哪一种。 */
const mode = ref<'export' | 'import' | null>(null)
const ask = ref<Ask | null>(null)

const pw = ref('')
const pw2 = ref('')
const reveal = ref(false)
const caps = ref(false)
const busy = ref(false)
const error = ref('')

const first = ref<HTMLInputElement | null>(null)

/** 拿到密码或放弃时用来结束 C# 那边的 await。 */
let done: ((r: { Password: string } | null) => void) | null = null

onMounted(() => {
  registerForm('encrypt-export', (a: Ask) => open('export', a))
  registerForm('encrypt-import', (a: Ask) => open('import', a))
})

function open(m: 'export' | 'import', a: Ask): Promise<{ Password: string } | null> {
  mode.value = m
  ask.value = a
  pw.value = ''
  pw2.value = ''
  reveal.value = false
  caps.value = false
  busy.value = false
  error.value = ''

  nextTick(() => first.value?.focus())

  return new Promise((resolve) => { done = resolve })
}

/** 每条出口都要经过它 —— 漏一条就让 C# 那边等到 5 分钟超时。 */
function finish(r: { Password: string } | null): void {
  const f = done
  done = null
  mode.value = null
  ask.value = null
  if (f) f(r)
}

const title = computed(() =>
  ask.value?.Title || t(mode.value === 'import' ? 'pw.importTitle' : 'pw.exportTitle'))

const canSubmit = computed(() => {
  if (busy.value || !pw.value) return false
  if (mode.value === 'export') return pw.value === pw2.value
  return true
})

async function submit(): Promise<void> {
  if (!pw.value) {
    error.value = t('pw.empty')
    return
  }

  if (mode.value === 'export') {
    if (pw.value !== pw2.value) {
      error.value = t('pw.mismatch')
      return
    }

    finish({ Password: pw.value })
    return
  }

  //导入：先验一次再关窗
  const path = ask.value?.FilePath

  if (!path) {
    //没给路径就验不了，退回老行为（关窗后由 Operate 报错）
    finish({ Password: pw.value })
    return
  }

  busy.value = true
  error.value = ''

  try {
    const r = await call<{ ok: boolean }>('verifyEncryptPassword', { path, password: pw.value })

    if (!r?.ok) {
      error.value = t('pw.wrong')
      pw.value = ''
      nextTick(() => first.value?.focus())
      return
    }

    finish({ Password: pw.value })
  } catch (e) {
    console.error('[pw] 校验密码失败', e)
    //验不了就别拦着，交给 Operate 那边的老路径去判
    finish({ Password: pw.value })
  } finally {
    busy.value = false
  }
}

function onKey(e: KeyboardEvent): void {
  caps.value = e.getModifierState?.('CapsLock') ?? false
}

/* 登记进模态栈：父窗体因此变 inert；自己被后开的弹窗盖住时也会 inert。见 useModal.ts */
const { covered } = useModal(() => mode.value !== null)
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
  <Teleport to="body"><div v-if="mode" class="mask" :inert="covered">
    <div class="dlg" role="dialog" aria-modal="true" @keydown.esc="finish(null)">
      <span class="mk tl" /><span class="mk tr" />
      <span class="mk bl" /><span class="mk br" />

      <header class="hd">
        <svg class="ico" viewBox="0 0 24 24">
          <rect x="4" y="10" width="16" height="11" rx="1.5" />
          <!-- 导出是「锁上」（锁梁立着），导入是「解锁」（锁梁偏到一侧） -->
          <path v-if="mode === 'export'" d="M8 10V7a4 4 0 0 1 8 0v3" />
          <path v-else d="M8 10V7a4 4 0 0 1 7.5-2" />
          <path d="M12 14v3" />
        </svg>
        <div class="tt">{{ title }}</div>
      </header>

      <div class="bd">
        <p class="lead">{{ t(mode === 'import' ? 'pw.importLead' : 'pw.exportLead') }}</p>

        <label class="fld">
          <span class="lb">{{ t('pw.password') }}</span>
          <span class="wrap">
            <input
              ref="first"
              v-model="pw"
              class="inp"
              :type="reveal ? 'text' : 'password'"
              autocomplete="off"
              spellcheck="false"
              :disabled="busy"
              @keydown="onKey"
              @keydown.enter.prevent="submit"
              @input="error = ''"
            >
            <button
              class="eye"
              type="button"
              :title="t(reveal ? 'pw.hide' : 'pw.show')"
              @click="reveal = !reveal"
            >
              <svg class="ico" viewBox="0 0 24 24">
                <path d="M2 12s3.6-6 10-6 10 6 10 6-3.6 6-10 6-10-6-10-6z" /><circle cx="12" cy="12" r="2.6" />
                <path v-if="reveal" d="M4 20L20 4" />
              </svg>
            </button>
          </span>
        </label>

        <!-- 只有导出要确认：设错了的代价是这份文件永远打不开 -->
        <label v-if="mode === 'export'" class="fld">
          <span class="lb">{{ t('pw.confirm') }}</span>
          <span class="wrap">
            <input
              v-model="pw2"
              class="inp"
              :type="reveal ? 'text' : 'password'"
              autocomplete="off"
              spellcheck="false"
              @keydown="onKey"
              @keydown.enter.prevent="submit"
              @input="error = ''"
            >
          </span>
        </label>

        <p v-if="caps" class="caps">{{ t('pw.caps') }}</p>
        <p v-if="error" class="err">{{ error }}</p>
      </div>

      <footer class="ft">
        <!-- 「不加密」明写出来，不靠「取消 = 不加密」那种暗规则 -->
        <button v-if="mode === 'export'" class="btn plain" :title="t('pw.skipHint')" @click="finish(null)">
          {{ t('pw.skip') }}
        </button>
        <button v-else class="btn" @click="finish(null)">{{ t('dlg.cancel') }}</button>

        <span class="grow" />

        <button class="btn primary" :disabled="!canSubmit" @click="submit">
          {{ busy ? t('pw.checking') : t(mode === 'import' ? 'pw.unlock' : 'pw.encrypt') }}
        </button>
      </footer>
    </div>
  </div></Teleport>
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
  width: min(400px, calc(100vw - 48px));
  background: var(--card);
  border: 1px solid var(--border);
  box-shadow: 0 18px 50px rgb(var(--shadow-rgb) / 60%);
}

.mk { position: absolute; width: 7px; height: 7px; border: 1px solid var(--cyan); }
.mk.tl { top: -1px; left: -1px; border-right: 0; border-bottom: 0; }
.mk.tr { top: -1px; right: -1px; border-left: 0; border-bottom: 0; }
.mk.bl { bottom: -1px; left: -1px; border-right: 0; border-top: 0; }
.mk.br { bottom: -1px; right: -1px; border-left: 0; border-top: 0; }

.hd { display: flex; align-items: center; gap: 11px; padding: 18px 20px 0; }

.hd .ico {
  width: 22px;
  height: 22px;
  flex: none;
  fill: none;
  stroke: var(--cyan);
  stroke-width: 1.6;
  stroke-linecap: round;
}

.hd .tt { font-family: var(--orbit); font-weight: 700; font-size: var(--fs-title); letter-spacing: .04em; color: var(--gray); }

.bd { padding: 14px 20px 4px; }

.lead { margin: 0 0 14px; font-size: var(--fs-body); line-height: 1.7; color: var(--muted); }

.fld { display: grid; grid-template-columns: 68px 1fr; align-items: center; gap: 12px; margin-bottom: 10px; }
.fld .lb { font-size: var(--fs-body); color: var(--muted); }
.wrap { position: relative; display: flex; }

.inp {
  flex: 1;
  min-width: 0;
  height: 30px;
  padding: 0 34px 0 10px;
  background: rgb(var(--inset-rgb) / 34%);
  border: 1px solid var(--border);
  color: var(--gray);
  font-family: var(--mono);
  font-size: var(--fs-lead);
  letter-spacing: .06em;
  outline: none;
  user-select: text;
}


.eye {
  position: absolute;
  right: 6px;
  top: 50%;
  transform: translateY(-50%);
  display: inline-flex;
  padding: 0;
  background: transparent;
  border: 0;
  color: var(--dim);
  cursor: pointer;
}

.eye:hover { color: var(--cyan); }
.eye .ico { width: 15px; height: 15px; fill: none; stroke: currentColor; stroke-width: 1.5; }

.caps { margin: 2px 0 0 80px; font-size: var(--fs-small); color: var(--amber); }
.err { margin: 2px 0 0 80px; font-size: var(--fs-small); color: var(--danger); }

.ft { display: flex; align-items: center; gap: 10px; padding: 14px 20px 18px; }
.ft .grow { flex: 1; }

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

.btn:hover:not(:disabled) { border-color: var(--cyan); color: var(--cyan); }
.btn:disabled { opacity: .35; cursor: default; }
.btn:focus-visible { outline-offset: -2px; }

/*
  「不加密」。

  一开始把它压得很淡（无边框、灰字），想法是「有后果的选择不该看着像默认动作」——
  但压过头了：<b>找不到的退路等于没有退路</b>，用户会以为必须设密码才能导出。
  现在给它正常按钮的分量，只是用琥珀色标出「这条路是有代价的」，
  与绿色的「加密导出」区分开。真正说明后果的是它的 title 和按钮上那三个字。
*/
.btn.plain { border-color: rgb(var(--amber-rgb) / 40%); color: var(--amber); }
.btn.plain:hover { border-color: var(--amber); background: rgb(var(--amber-rgb) / 12%); color: var(--amber); }

.btn.primary { border-color: rgb(var(--green-rgb) / 45%); color: var(--green); }
.btn.primary:hover:not(:disabled) { background: rgb(var(--green-rgb) / 10%); border-color: var(--green); }
</style>
