<script setup lang="ts">
/*
  本地映射的一条 —— 对应 WinForms 的 Controls/MapLocalEdit。
  远端地址（协议 / 主机 / 端口 / 路径）→ 本地文件。本地文件走 C# 的原生文件框（浏览器拿不到完整路径），也能直接填。
  协议只有 http（那边的下拉两项都落到 Http），这里就不画下拉了。
*/
import { computed, ref, watch } from 'vue'
import { call } from '../../bridge'
import type { MapLocalRow } from '../../bridge/types'
import { t } from '../../i18n'
import SettingsModal from './SettingsModal.vue'

const props = defineProps<{ target: MapLocalRow | null | 'add' }>()
const emit = defineEmits<{ (e: 'close'): void }>()

const busy = ref(false)
const error = ref('')
const f = ref({ host: '', port: 80, remotePath: '', localPath: '' })

const isAdd = computed(() => props.target === 'add')
const title = computed(() => t('map.local') + ' · ' + t(isAdd.value ? 'fw.add' : 'fw.edit'))

watch(() => props.target, (v) => {
  if (!v) return
  error.value = ''
  if (v === 'add') { f.value = { host: '', port: 80, remotePath: '', localPath: '' }; return }
  f.value = { host: v.Host, port: v.Port, remotePath: v.RemotePath, localPath: v.LocalPath }
})

async function pick(): Promise<void> {
  try {
    const r = await call<{ path: string }>('pickLocalFile')
    if (r?.path) f.value.localPath = r.path
  } catch (e) {
    console.error('[map] 选文件失败', e)
  }
}

async function save(): Promise<void> {
  busy.value = true
  error.value = ''
  try {
    const r = await call<{ error: string }>('saveMapLocal', {
      id: isAdd.value ? '' : (props.target as MapLocalRow).Id,
      protocol: 0,
      host: f.value.host,
      port: Math.trunc(f.value.port || 0),
      remotePath: f.value.remotePath,
      localPath: f.value.localPath,
    })
    if (r?.error) { error.value = r.error; return }
    emit('close')
  } catch (e) {
    console.error('[map] 保存本地映射失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}
</script>

<template>
  <SettingsModal :open="!!props.target" :title="title" subtitle="Local Mapping" :busy="busy" :error="error"
                 @update:open="!$event && emit('close')" @save="save">
    <div class="setf">
      <div class="grp">{{ t('map.remoteAddr') }}</div>
      <div class="row">
        <div class="k">{{ t('map.host') }}</div>
        <div class="v">
          <span class="proto">http://</span>
          <input v-model="f.host" class="inp" spellcheck="false" placeholder="www.example.com">
          <span class="colon">:</span>
          <input v-model.number="f.port" class="inp num" type="number" min="1" max="65535">
        </div>
      </div>
      <div class="row">
        <div class="k">{{ t('map.path') }}</div>
        <div class="v"><input v-model="f.remotePath" class="inp" spellcheck="false" placeholder="/api/config.json"></div>
      </div>
      <div class="grp">{{ t('map.localFile') }}</div>
      <div class="row">
        <div class="k">{{ t('map.localFile') }}</div>
        <div class="v">
          <input v-model="f.localPath" class="inp" spellcheck="false" :placeholder="t('map.localFilePh')">
          <button class="sbtn" @click="pick">{{ t('ex.pick') }}</button>
        </div>
      </div>
      <p class="hint">{{ t('map.localEditHint') }}</p>
    </div>
  </SettingsModal>
</template>

<style scoped>
.proto, .colon { color: var(--dim); font-family: var(--mono); font-size: var(--fs-body); }
</style>
