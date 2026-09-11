<script setup lang="ts">
/*
  外部代理设置 —— 对应 WinForms 的 Controls/EXTProxySetting。
  WPE 自己的 SOCKS5 出口再套一层外部 SOCKS 代理：地址 / 端口、只对指定端口生效、需要认证。校验在 C# 侧（ValidateExtProxy）。
*/
import { ref, watch } from 'vue'
import { call } from '../../bridge'
import { t } from '../../i18n'
import { pushToast } from '../../stores/toast'
import SettingsModal from './SettingsModal.vue'

const props = defineProps<{ open: boolean }>()
const emit = defineEmits<{ (e: 'update:open', v: boolean): void }>()

interface Setting { Enable: boolean; IP: string; Port: number; AppointPort: boolean; AppointPortContent: string; Auth: boolean; UserName: string; PassWord: string }

const busy = ref(false)
const testing = ref(false)
const error = ref('')
const f = ref<Setting>({ Enable: false, IP: '127.0.0.1', Port: 8889, AppointPort: false, AppointPortContent: '', Auth: false, UserName: '', PassWord: '' })

watch(() => props.open, async (on) => {
  if (!on) return
  error.value = ''
  try { f.value = await call<Setting>('getExtProxySetting') }
  catch (e) { console.error('[xp] 读取外部代理设置失败', e) }
}, { immediate: true })

function payload() {
  return { enable: f.value.Enable, ip: f.value.IP, port: Math.trunc(f.value.Port || 0), appointPort: f.value.AppointPort, appointPortContent: f.value.AppointPortContent, auth: f.value.Auth, userName: f.value.UserName, passWord: f.value.PassWord }
}

async function test(): Promise<void> {
  testing.value = true
  try {
    const r = await call<{ error: string }>('testSocksProxy', { auth: f.value.Auth, ip: f.value.IP, port: Math.trunc(f.value.Port || 0), userName: f.value.UserName, passWord: f.value.PassWord })
    if (r?.error) pushToast('error', r.error)
    else pushToast('success', t('ps.connected'))
  } catch (e) {
    console.error('[xp] 检测失败', e)
  } finally {
    testing.value = false
  }
}

async function save(): Promise<void> {
  busy.value = true
  error.value = ''
  try {
    const r = await call<{ error: string }>('saveExtProxySetting', payload())
    if (r?.error) { error.value = r.error; return }
    emit('update:open', false)
  } catch (e) {
    console.error('[xp] 保存失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}
</script>

<template>
  <SettingsModal :open="props.open" :title="t('set.extproxy')" subtitle="Upstream Proxy" :busy="busy" :error="error"
                 @update:open="emit('update:open', $event)" @save="save">
    <div class="setf">
      <div class="swb">
      <div class="row">
        <div class="k">{{ t('xp.enable') }}</div>
        <div class="v">
          <button class="chk" :class="{ on: f.Enable }" @click="f.Enable = !f.Enable"><i />{{ t('set.speedModeOn') }}</button>
        </div>
      </div>
      <p class="hint">{{ t('xp.hint') }}</p>
      </div>

      <section class="sec">
      <div class="grp">{{ t('xp.grp') }}</div>
      <div class="row" :class="{ off: !f.Enable }">
        <div class="k">{{ t('xp.addr') }}</div>
        <div class="v">
          <input v-model="f.IP" class="inp" spellcheck="false" :disabled="!f.Enable" :placeholder="t('xp.addrPh')">
          <span class="colon">:</span>
          <input v-model.number="f.Port" class="inp num" type="number" min="1" max="65535" :disabled="!f.Enable">
          <button class="sbtn" :disabled="!f.Enable || testing" @click="test">{{ testing ? t('proxy.working') : t('ps.detect') }}</button>
        </div>
      </div>
      <div class="row" :class="{ off: !f.Enable }">
        <div class="k">{{ t('ps.appointPort') }}</div>
        <div class="v">
          <button class="chk" :class="{ on: f.AppointPort }" :disabled="!f.Enable" @click="f.AppointPort = !f.AppointPort"><i />{{ t('set.speedModeOn') }}</button>
          <input v-model="f.AppointPortContent" class="inp" spellcheck="false" :disabled="!f.Enable || !f.AppointPort" placeholder="80,8080,443,8443">
        </div>
      </div>
      <p class="hint">{{ t('xp.portHint') }}</p>
      <div class="row" :class="{ off: !f.Enable }">
        <div class="k">{{ t('ps.auth') }}</div>
        <div class="v">
          <button class="chk" :class="{ on: f.Auth }" :disabled="!f.Enable" @click="f.Auth = !f.Auth"><i />{{ t('set.speedModeOn') }}</button>
          <input v-model="f.UserName" class="inp sm" spellcheck="false" :disabled="!f.Enable || !f.Auth" :placeholder="t('ps.userPh')">
          <input v-model="f.PassWord" class="inp sm" type="password" :disabled="!f.Enable || !f.Auth" :placeholder="t('ps.passPh')">
        </div>
      </div>
      </section>
    </div>
  </SettingsModal>
</template>

<style scoped>
.colon { color: var(--dim); font-family: var(--mono); }
</style>
