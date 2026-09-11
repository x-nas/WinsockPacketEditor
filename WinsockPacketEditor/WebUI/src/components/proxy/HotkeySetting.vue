<script setup lang="ts">
/*
  快捷键设置 —— 对应 WinForms 的 Controls/HotKeySetting。

  12 个全局快捷键：1–10 触发发送列表 / 机器人列表里对应序号的那一条（「作用于」二选一），11 执行整份列表、12 停止。
  每一行「按键捕获框 + 注册」：与 WinForms 一样按下「注册」那一刻就向系统登记（RegisterHotKey），不等保存；
  组合键格式照 SystemConfig.ConvertHotkeyToString（"Ctrl + Alt + F1"），由 keys.ts 的 comboOf 拼。
  「保存」只管「作用于」这一项。
*/
import { ref, watch } from 'vue'
import { call } from '../../bridge'
import { t } from '../../i18n'
import { comboOf, isModifierEvent } from '../../keys'
import { refreshHotkey } from '../../stores/runtime'
import { pushToast } from '../../stores/toast'
import SettingsModal from './SettingsModal.vue'

const props = defineProps<{ open: boolean }>()
const emit = defineEmits<{ (e: 'update:open', v: boolean): void }>()

const busy = ref(false)
const error = ref('')
const type = ref(0)
const keys = ref<string[]>(Array(12).fill(''))
const saved = ref<string[]>(Array(12).fill(''))
const state = ref<Array<'' | 'ok' | 'bad'>>(Array(12).fill(''))

watch(() => props.open, async (on) => {
  if (!on) return
  error.value = ''
  state.value = Array(12).fill('')
  try {
    const r = await call<{ Type: number; Keys: string[] }>('getHotkeySetting')
    type.value = r?.Type ?? 0
    keys.value = (r?.Keys ?? []).map((k) => k ?? '')
    while (keys.value.length < 12) keys.value.push('')
    saved.value = [...keys.value]
  } catch (e) {
    console.error('[hk] 读取快捷键失败', e)
  }
}, { immediate: true })

function capture(e: KeyboardEvent, i: number): void {
  e.preventDefault()
  if (isModifierEvent(e)) return
  const c = comboOf(e)
  if (!c) return
  keys.value[i] = c
  keys.value = [...keys.value]
  state.value[i] = ''
  state.value = [...state.value]
}

async function register(i: number): Promise<void> {
  try {
    const r = await call<{ ok: boolean }>('registerHotkey', { index: i + 1, text: keys.value[i] })
    state.value[i] = r?.ok ? 'ok' : 'bad'
    state.value = [...state.value]
    if (r?.ok) { saved.value[i] = keys.value[i]; pushToast('success', t('hk.registered')); void refreshHotkey() }
    else pushToast('error', t('hk.registerFail'))
  } catch (e) {
    console.error('[hk] 注册失败', e)
  }
}

async function save(): Promise<void> {
  busy.value = true
  try {
    await call('saveHotkeyType', { type: type.value })
    //快捷面板底部那一条跟着变
    void refreshHotkey()
    emit('update:open', false)
  } catch (e) {
    console.error('[hk] 保存失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}

/*
  每行的状态灯（原来是一枚写着「生效中 / 未注册 / 失败」的标签，按要求换成灯，省出一列宽度给组合键）：

    绿  生效中 —— 已向系统登记（刚注册成功的也是这一档）
    黄  未注册 —— 组合键改过了还没按「注册」，旧的那个仍在生效
    红  注册失败 —— 多半是被别的程序占了
    灰  未设置 —— 这一格是空的

  文字进悬停提示与 aria-label：灯只有颜色，读屏与色弱用户要靠那句话。
  ⚠️ 判断顺序与原来那枚标签一致：失败优先，其次「改过没注册」，再次空，最后才是生效。
*/
type Lamp = 'on' | 'pending' | 'bad' | 'none'

function lampOf(i: number): Lamp {
  if (state.value[i] === 'bad') return 'bad'
  if (keys.value[i] !== saved.value[i]) return 'pending'
  if (!keys.value[i]) return 'none'
  return 'on'
}

const LAMP_TIP: Record<Lamp, 'hk.lampOn' | 'hk.lampPending' | 'hk.lampBad' | 'hk.lampNone'> = {
  on: 'hk.lampOn', pending: 'hk.lampPending', bad: 'hk.lampBad', none: 'hk.lampNone',
}

function labelOf(i: number): string {
  if (i === 10) return t('hk.execute')
  if (i === 11) return t('hk.stop')
  return t('hk.key') + ' ' + (i + 1)
}
</script>

<template>
  <!-- ⚠️ 780 → 824：分区卡的左右外边距 + 边框吃掉 42px，俄语那 12 行原本正好卡在 714px 上 -->
  <SettingsModal :open="props.open" :title="t('set.hotkey')" subtitle="Shortcut Keys" :busy="busy" :error="error" :width="824"
                 @update:open="emit('update:open', $event)" @save="save">
    <div class="setf hk">
      <div class="swb">
      <div class="row">
        <div class="k">{{ t('hk.applyTo') }}</div>
        <div class="v">
          <button class="rd" :class="{ on: type === 0 }" @click="type = 0"><i />{{ t('rb.e.swSend') }}</button>
          <button class="rd" :class="{ on: type === 1 }" @click="type = 1"><i />{{ t('rb.e.swRobot') }}</button>
        </div>
      </div>
      <p class="hint">{{ t('hk.hint') }}</p>
      </div>

      <section class="sec">
      <div class="grp">{{ t('hk.custom') }}</div>
      <div class="grid">
        <div v-for="i in 12" :key="i" class="hrow" :class="{ ctl: i > 10 }">
          <span class="lamp" :class="lampOf(i - 1)" role="img" :title="t(LAMP_TIP[lampOf(i - 1)])" :aria-label="t(LAMP_TIP[lampOf(i - 1)])"><i /></span>
          <span class="kl">{{ labelOf(i - 1) }}</span>
          <input :value="keys[i - 1]" class="inp cap-key" :class="{ bad: state[i - 1] === 'bad' }" readonly :placeholder="t('hk.ph')" @keydown="capture($event, i - 1)">
          <button class="sbtn" :class="{ primary: keys[i - 1] !== saved[i - 1] }" :disabled="!keys[i - 1]" @click="register(i - 1)">{{ t('hk.register') }}</button>
        </div>
      </div>
      <p class="hint">{{ t('hk.registerHint') }}</p>
      </section>
    </div>
  </SettingsModal>
</template>

<style scoped>
.grid { display: grid; grid-template-columns: 1fr 1fr; gap: 6px 16px; padding: 2px 20px 6px; }
/*
  标签列宽乘 --setf-kx（App.vue 按语言给：方块字 1、拉丁 / 西里尔 1.3）——
  64 是按「快捷键 1 :」四个字定的，俄语的「Клавиша 10 :」要 71px。与设置弹窗标签列同一个令牌。
  状态那一列原来是 58 × kx 的文字标签，换成灯之后定宽 14px、不跟语言走，省下的宽度都给了组合键那一格。
  灯排在<b>每行最前面</b>（2026-09-11 按要求从组合键后面挪过来）：一竖列灯对齐成一条，扫一眼就看得出哪几个没生效。
*/
.hrow {
  display: grid;
  grid-template-columns: 14px calc(64px * var(--setf-kx, 1)) 1fr auto;
  align-items: center;
  gap: 8px;
}
.hrow.ctl { border-top: 1px dashed var(--border); padding-top: 8px; margin-top: 4px; }
.kl { font-size: var(--fs-body); color: var(--muted); white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }
/*
  状态灯：8px 实心圆 + 同色辉光，与状态条上那颗运行灯同一套语汇。
  灰（未设置）是空心的 —— 「没有东西」不该画成一颗亮着的灯。
  外层 14px 定宽定高，灯在里面居中；悬停提示认的是外层这个 span。
*/
.lamp { width: 14px; height: 14px; display: flex; align-items: center; justify-content: center; cursor: default; }
.lamp i { width: 8px; height: 8px; border-radius: 50%; display: block; }
.lamp.on i { background: var(--green); box-shadow: 0 0 6px rgb(var(--green-rgb) / 70%); }
.lamp.pending i { background: var(--amber); box-shadow: 0 0 6px rgb(var(--amber-rgb) / 60%); }
.lamp.bad i { background: var(--danger); box-shadow: 0 0 6px rgb(var(--danger-rgb) / 70%); }
.lamp.none i { border: 1px solid var(--dim3); }
</style>
