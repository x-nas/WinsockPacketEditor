<script setup lang="ts">
/*
  远程映射的一条 —— 对应 WinForms 的 Controls/MapRemoteEdit。请求地址 → 映射地址，映射端可以是 https。
*/
import { computed, ref, watch } from 'vue'
import { call } from '../../bridge'
import type { MapRemoteRow } from '../../bridge/types'
import { t } from '../../i18n'
import CyberSelect from '../CyberSelect.vue'
import SettingsModal from './SettingsModal.vue'

const props = defineProps<{ target: MapRemoteRow | null | 'add' }>()
const emit = defineEmits<{ (e: 'close'): void }>()

const busy = ref(false)
const error = ref('')
const f = ref({ hostFrom: '', portFrom: 80, pathFrom: '', protocolTo: 0, hostTo: '', portTo: 80, pathTo: '' })

const isAdd = computed(() => props.target === 'add')
const title = computed(() => t('map.remote') + ' · ' + t(isAdd.value ? 'fw.add' : 'fw.edit'))
const PROTOS = [{ value: 0, label: 'http' }, { value: 1, label: 'https' }]

watch(() => props.target, (v) => {
  if (!v) return
  error.value = ''
  if (v === 'add') { f.value = { hostFrom: '', portFrom: 80, pathFrom: '', protocolTo: 0, hostTo: '', portTo: 80, pathTo: '' }; return }
  f.value = { hostFrom: v.HostFrom, portFrom: v.PortFrom, pathFrom: v.PathFrom, protocolTo: v.ProtocolTo, hostTo: v.HostTo, portTo: v.PortTo, pathTo: v.PathTo }
})

async function save(): Promise<void> {
  busy.value = true
  error.value = ''
  try {
    const r = await call<{ error: string }>('saveMapRemote', {
      id: isAdd.value ? '' : (props.target as MapRemoteRow).Id,
      protocolFrom: 0,
      hostFrom: f.value.hostFrom,
      portFrom: Math.trunc(f.value.portFrom || 0),
      pathFrom: f.value.pathFrom,
      protocolTo: f.value.protocolTo,
      hostTo: f.value.hostTo,
      portTo: Math.trunc(f.value.portTo || 0),
      pathTo: f.value.pathTo,
    })
    if (r?.error) { error.value = r.error; return }
    emit('close')
  } catch (e) {
    console.error('[map] 保存远程映射失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}
</script>

<template>
  <SettingsModal :open="!!props.target" :title="title" subtitle="Controls/MapRemoteEdit" :busy="busy" :error="error"
                 @update:open="!$event && emit('close')" @save="save">
    <div class="setf">
      <div class="grp">{{ t('map.reqAddr') }}</div>
      <div class="row">
        <div class="k">{{ t('map.host') }}</div>
        <div class="v">
          <span class="proto">http://</span>
          <input v-model="f.hostFrom" class="inp" spellcheck="false" placeholder="www.example.com">
          <span class="colon">:</span>
          <input v-model.number="f.portFrom" class="inp num" type="number" min="1" max="65535">
        </div>
      </div>
      <div class="row">
        <div class="k">{{ t('map.path') }}</div>
        <div class="v"><input v-model="f.pathFrom" class="inp" spellcheck="false" placeholder="/api/"></div>
      </div>

      <div class="grp">{{ t('map.mapAddr') }}</div>
      <div class="row">
        <div class="k">{{ t('map.host') }}</div>
        <div class="v">
          <CyberSelect v-model="f.protocolTo" :options="PROTOS" class="sel" />
          <input v-model="f.hostTo" class="inp" spellcheck="false" placeholder="127.0.0.1">
          <span class="colon">:</span>
          <input v-model.number="f.portTo" class="inp num" type="number" min="1" max="65535">
        </div>
      </div>
      <div class="row">
        <div class="k">{{ t('map.path') }}</div>
        <div class="v"><input v-model="f.pathTo" class="inp" spellcheck="false" placeholder="/api/"></div>
      </div>
      <p class="hint">{{ t('map.remoteEditHint') }}</p>
    </div>
  </SettingsModal>
</template>

<style scoped>
.proto, .colon { color: var(--dim); font-family: var(--mono); font-size: 12px; }
.sel { width: 92px; }
</style>
