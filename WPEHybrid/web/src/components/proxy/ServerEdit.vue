<script setup lang="ts">
/*
  ProxyCap 服务器的新增 / 编辑 —— 对应 WinForms 的 Controls/ServerEdit。校验在 C# 侧（SaveServer_Shell）。
*/
import { computed, ref, watch } from 'vue'
import { call } from '../../bridge'
import type { ServerRow } from '../../bridge/types'
import { t } from '../../i18n'
import SettingsModal from './SettingsModal.vue'

const props = defineProps<{ target: ServerRow | null | 'add' }>()
const emit = defineEmits<{ (e: 'close'): void }>()

const busy = ref(false)
const error = ref('')
const f = ref({ enable: true, name: '', ip: '', port: 1080, forgotUrl: '', registerUrl: '', verifyUrl: '' })

const isAdd = computed(() => props.target === 'add')
const title = computed(() => t('wpc.servers') + ' · ' + t(isAdd.value ? 'fw.add' : 'fw.edit'))

watch(() => props.target, (v) => {
  if (!v) return
  error.value = ''
  if (v === 'add') { f.value = { enable: true, name: '', ip: '', port: 1080, forgotUrl: '', registerUrl: '', verifyUrl: '' }; return }
  f.value = { enable: v.IsEnable, name: v.Name, ip: v.IP, port: v.Port, forgotUrl: v.ForgotURL, registerUrl: v.RegisterURL, verifyUrl: v.VerifyURL }
})

async function save(): Promise<void> {
  busy.value = true
  error.value = ''
  try {
    const r = await call<{ error: string }>('saveServer', {
      id: isAdd.value ? '' : (props.target as ServerRow).Id,
      ...f.value,
      port: Math.trunc(f.value.port || 0),
    })
    if (r?.error) { error.value = r.error; return }
    emit('close')
  } catch (e) {
    console.error('[wpc] 保存服务器失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}
</script>

<template>
  <SettingsModal :open="!!props.target" :title="title" subtitle="Proxy Node" :busy="busy" :error="error"
                 @update:open="!$event && emit('close')" @save="save">
    <div class="setf">
      <div class="row">
        <div class="k">{{ t('col.enable') }}</div>
        <div class="v"><button class="chk" :class="{ on: f.enable }" @click="f.enable = !f.enable"><i />{{ t('set.speedModeOn') }}</button></div>
      </div>
      <div class="row">
        <div class="k">{{ t('wpc.serverName') }}</div>
        <div class="v"><input v-model="f.name" class="inp" spellcheck="false" :placeholder="t('wpc.serverNamePh')"></div>
      </div>
      <div class="row">
        <div class="k">{{ t('wpc.serverAddr') }}</div>
        <div class="v">
          <input v-model="f.ip" class="inp" spellcheck="false" placeholder="192.168.1.10">
          <span class="colon">:</span>
          <input v-model.number="f.port" class="inp num" type="number" min="1" max="65535">
        </div>
      </div>
      <div class="grp">{{ t('wpc.grp.urls') }}</div>
      <div class="row">
        <div class="k">{{ t('wpc.forgotUrl') }}</div>
        <div class="v"><input v-model="f.forgotUrl" class="inp" spellcheck="false" placeholder="https://"></div>
      </div>
      <div class="row">
        <div class="k">{{ t('wpc.registerUrl') }}</div>
        <div class="v"><input v-model="f.registerUrl" class="inp" spellcheck="false" placeholder="https://"></div>
      </div>
      <div class="row">
        <div class="k">{{ t('wpc.verifyUrl') }}</div>
        <div class="v"><input v-model="f.verifyUrl" class="inp" spellcheck="false" placeholder="https://"></div>
      </div>
      <p class="hint">{{ t('wpc.urlHint') }}</p>
    </div>
  </SettingsModal>
</template>

<style scoped>
.colon { color: var(--dim); font-family: var(--mono); }
</style>
