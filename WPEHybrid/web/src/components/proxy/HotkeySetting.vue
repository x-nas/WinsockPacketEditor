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
    if (r?.ok) { saved.value[i] = keys.value[i]; pushToast('success', t('hk.registered')) }
    else pushToast('error', t('hk.registerFail'))
  } catch (e) {
    console.error('[hk] 注册失败', e)
  }
}

async function save(): Promise<void> {
  busy.value = true
  try {
    await call('saveHotkeyType', { type: type.value })
    emit('update:open', false)
  } catch (e) {
    console.error('[hk] 保存失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}

function labelOf(i: number): string {
  if (i === 10) return t('hk.execute')
  if (i === 11) return t('hk.stop')
  return t('hk.key') + ' ' + (i + 1)
}
</script>

<template>
  <SettingsModal :open="props.open" :title="t('set.hotkey')" subtitle="Controls/HotKeySetting" :busy="busy" :error="error" :width="780"
                 @update:open="emit('update:open', $event)" @save="save">
    <div class="setf hk">
      <div class="row">
        <div class="k">{{ t('hk.applyTo') }}</div>
        <div class="v">
          <button class="rd" :class="{ on: type === 0 }" @click="type = 0"><i />{{ t('rb.e.swSend') }}</button>
          <button class="rd" :class="{ on: type === 1 }" @click="type = 1"><i />{{ t('rb.e.swRobot') }}</button>
        </div>
      </div>
      <p class="hint">{{ t('hk.hint') }}</p>

      <div class="grp">{{ t('hk.custom') }}</div>
      <div class="grid">
        <div v-for="i in 12" :key="i" class="hrow" :class="{ ctl: i > 10 }">
          <span class="kl">{{ labelOf(i - 1) }}</span>
          <input :value="keys[i - 1]" class="inp cap-key" :class="{ bad: state[i - 1] === 'bad' }" readonly :placeholder="t('hk.ph')" @keydown="capture($event, i - 1)">
          <span class="tg" :class="state[i - 1] === 'ok' ? 'ok' : state[i - 1] === 'bad' ? 'bad' : (keys[i - 1] !== saved[i - 1] ? 'amber' : 'dim')">
            {{ state[i - 1] === 'ok' ? t('hk.stOk') : state[i - 1] === 'bad' ? t('hk.stBad') : (keys[i - 1] !== saved[i - 1] ? t('hk.stChanged') : t('hk.stSaved')) }}
          </span>
          <button class="sbtn" :class="{ primary: keys[i - 1] !== saved[i - 1] }" :disabled="!keys[i - 1]" @click="register(i - 1)">{{ t('hk.register') }}</button>
        </div>
      </div>
      <p class="hint">{{ t('hk.registerHint') }}</p>
    </div>
  </SettingsModal>
</template>

<style scoped>
.grid { display: grid; grid-template-columns: 1fr 1fr; gap: 6px 16px; padding: 2px 20px 6px; }
/*
  两个固定列宽都乘 --setf-kx（App.vue 按语言给：方块字 1、拉丁 / 西里尔 1.3）——
  64 是按「快捷键 1 :」四个字定的，俄语的「Клавиша 10 :」要 71px，
  58 的状态标签装不下「Не назначено」。与设置弹窗标签列同一个令牌。
*/
.hrow {
  display: grid;
  grid-template-columns: calc(64px * var(--setf-kx, 1)) 1fr calc(58px * var(--setf-kx, 1)) auto;
  align-items: center;
  gap: 8px;
}
.hrow.ctl { border-top: 1px dashed var(--border); padding-top: 8px; margin-top: 4px; }
.kl { font-size: 12.5px; color: var(--muted); white-space: nowrap; overflow: hidden; text-overflow: ellipsis; }
.hk .tg { text-align: center; }
</style>
