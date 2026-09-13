<script setup lang="ts">
/*
  远程管理设置 —— 对应 WinForms 的 Controls/RemoteMGTSetting。
  内嵌的 OWIN 管理台：选本机哪个 IP、端口、管理员账号密码；保存即起 / 停（C# 的 StartRemoteMGT / StopRemoteMGT）。
  地址预览可以点 —— 走 openExternal 交给系统浏览器。
*/
import { computed, ref, watch } from 'vue'
import { call } from '../../bridge'
import { t } from '../../i18n'
import CyberSelect from '../CyberSelect.vue'
import SettingsModal from './SettingsModal.vue'

const props = defineProps<{ open: boolean }>()
const emit = defineEmits<{ (e: 'update:open', v: boolean): void }>()

interface Setting { IsRemote: boolean; IP: string; IPs: string[]; Port: number; UserName: string; PassWord: string; Running: boolean; IPMissing?: boolean }

const busy = ref(false)
const error = ref('')
const f = ref<Setting>({ IsRemote: false, IP: '127.0.0.1', IPs: ['127.0.0.1'], Port: 88, UserName: '', PassWord: '', Running: false })

watch(() => props.open, async (on) => {
  if (!on) return
  error.value = ''
  try { f.value = await call<Setting>('getRemoteSetting') }
  catch (e) { console.error('[rm] 读取远程管理设置失败', e) }
}, { immediate: true })

/*
  保存的地址不在本机网卡上（换了网络 / DHCP 重新分配）时，C# 把它原样放在 IPs 最前面并标 IPMissing。
  原来是悄悄换成第一个网卡的 IP 显示 —— 服务却仍按旧地址去绑，于是起不来，这里看着却一切正常。
  现在照实显示，并在下面给一句提示；用户选一个别的地址再保存即可。
*/
const missingIp = computed(() => (f.value.IPMissing ? f.value.IPs[0] : ''))

//下拉里不加标注：200px 宽放不下（会截成「（不在…」），下面那句提示已经点了名
const ipOptions = computed(() => f.value.IPs.map((ip) => ({ value: ip, label: ip })))
const url = computed(() => 'http://' + f.value.IP + ':' + Math.trunc(f.value.Port || 0))

function openUrl(): void {
  call('openExternal', { url: url.value }).catch(() => {})
}

async function save(): Promise<void> {
  busy.value = true
  error.value = ''
  try {
    const r = await call<{ error: string; running: boolean }>('saveRemoteSetting', {
      isRemote: f.value.IsRemote, ip: f.value.IP, port: Math.trunc(f.value.Port || 0), userName: f.value.UserName, passWord: f.value.PassWord,
    })
    if (r?.error) { error.value = r.error; return }
    emit('update:open', false)
  } catch (e) {
    console.error('[rm] 保存失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}
</script>

<template>
  <SettingsModal :open="props.open" :title="t('set.remote')" subtitle="Remote Console" :busy="busy" :error="error"
                 @update:open="emit('update:open', $event)" @save="save">
    <div class="setf">
      <div class="swb">
      <div class="row">
        <div class="k">{{ t('rm.enable') }}</div>
        <div class="v">
          <button class="chk" :class="{ on: f.IsRemote }" @click="f.IsRemote = !f.IsRemote"><i />{{ t('set.speedModeOn') }}</button>
          <span class="tg" :class="f.Running ? 'ok' : 'dim'">{{ f.Running ? t('rm.running') : t('rm.stopped') }}</span>
        </div>
      </div>
      <p class="hint">{{ t('rm.hint') }}</p>
      </div>

      <section class="sec">
      <div class="grp">{{ t('rm.grp') }}</div>
      <div class="row" :class="{ off: !f.IsRemote }">
        <div class="k">{{ t('rm.listen') }}</div>
        <div class="v">
          <CyberSelect v-model="f.IP" :options="ipOptions" :disabled="!f.IsRemote" class="sel" />
          <span class="colon">:</span>
          <input v-model.number="f.Port" class="inp num" type="number" min="1" max="65535" :disabled="!f.IsRemote">
        </div>
      </div>
      <div class="row" :class="{ off: !f.IsRemote }">
        <div class="k">{{ t('rm.admin') }}</div>
        <div class="v">
          <input v-model="f.UserName" class="inp sm" spellcheck="false" :disabled="!f.IsRemote" :placeholder="t('rm.userPh')">
          <input v-model="f.PassWord" class="inp sm" type="password" :disabled="!f.IsRemote" :placeholder="t('ps.passPh')">
        </div>
      </div>
      <div class="row" :class="{ off: !f.IsRemote }">
        <div class="k">{{ t('rm.url') }}</div>
        <div class="v"><span class="link" @click="f.IsRemote && openUrl()">{{ url }}</span></div>
      </div>
      <p v-if="missingIp && f.IP === missingIp" class="hint warn">{{ t('rm.ipMissing').replace('{0}', missingIp) }}</p>
      <p class="hint">{{ t('rm.saveHint') }}</p>
      </section>
    </div>
  </SettingsModal>
</template>

<style scoped>
.colon { color: var(--dim); font-family: var(--mono); }
.sel { width: 200px; }
</style>
