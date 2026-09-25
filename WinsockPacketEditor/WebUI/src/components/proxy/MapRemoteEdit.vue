<script setup lang="ts">
/*
  远程映射的一条 —— 对应 WinForms 的 Controls/MapRemoteEdit。请求地址 → 映射地址。

  映射只在 SOCKS5 的明文 HTTP 路径上生效。两端显示协议下拉，是为了与本地映射保持
  一致的编辑体验；首期只有 HTTP 选项，不能制造不生效的 HTTPS 重定向规则。
  （HandleHttpConnect / ForwardData 的 DomainType.HTTP 分支），四个查询调用点
  一律传 MapProtocol.Http；ProtocolTypeTo 在任何数据路径上都<b>没有被读过</b> ——
  ConnectToTarget 开的是明文 TCP，ModifyRequestHostAndPath 拼的也是明文 HTTP 请求。
  所以这里曾经有过的「映射端 https」是个装饰项，选了不但不生效，
  还会把请求明文发到一个 TLS 端口上，2026-09-09 去掉了。
  （WinForms 的两个下拉本来就只有 "http" 一项，这个选项是外壳自己加出来的。）
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
const HTTP_ONLY = [{ value: 0, label: 'http://' }]
const f = ref({ protocolFrom: 0, hostFrom: '', portFrom: 80, pathFrom: '', protocolTo: 0, hostTo: '', portTo: 80, pathTo: '' })

const isAdd = computed(() => props.target === 'add')
const title = computed(() => t('map.remote') + ' · ' + t(isAdd.value ? 'fw.add' : 'fw.edit'))

watch(() => props.target, (v) => {
  if (!v) return
  error.value = ''
  if (v === 'add') { f.value = { protocolFrom: 0, hostFrom: '', portFrom: 80, pathFrom: '', protocolTo: 0, hostTo: '', portTo: 80, pathTo: '' }; return }
  // 老库可能遗留 HTTPS 值；远程映射并不支持它，编辑后统一归正为 HTTP。
  f.value = { protocolFrom: 0, hostFrom: v.HostFrom, portFrom: v.PortFrom, pathFrom: v.PathFrom, protocolTo: 0, hostTo: v.HostTo, portTo: v.PortTo, pathTo: v.PathTo }
})

async function save(): Promise<void> {
  busy.value = true
  error.value = ''
  try {
    const r = await call<{ error: string }>('saveMapRemote', {
      id: isAdd.value ? '' : (props.target as MapRemoteRow).Id,
      protocolFrom: f.value.protocolFrom,
      hostFrom: f.value.hostFrom,
      portFrom: Math.trunc(f.value.portFrom || 0),
      pathFrom: f.value.pathFrom,
      //两端都是 http。老库里可能存着 ProtocolTo=1（外壳早先能选 https），
      //改一次就归正，不做迁移 —— 那个值本来也没人读
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
  <SettingsModal :open="!!props.target" :title="title" subtitle="Remote Mapping" :busy="busy" :error="error"
                 @update:open="!$event && emit('close')" @save="save">
    <div class="setf">
      <div class="grp">{{ t('map.reqAddr') }}</div>
      <div class="row">
        <div class="k">{{ t('map.host') }}</div>
        <div class="v">
          <CyberSelect v-model="f.protocolFrom" class="proto" :options="HTTP_ONLY" />
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
          <CyberSelect v-model="f.protocolTo" class="proto" :options="HTTP_ONLY" />
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
.proto { width: 88px; }
.colon { color: var(--dim); font-family: var(--mono); font-size: var(--fs-body); }
</style>
