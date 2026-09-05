<script setup lang="ts">
/*
  过滤设置 —— 对应 WinForms 的 Controls/LeachSetting。

  【与「滤镜」不是一回事】这里决定**收不收**这个封包，滤镜决定命中之后**怎么改**。
  过滤发生在入列表之前，所以它是控制列表刷屏最直接的手段。

  六个条件各自可开关、各带一个值；再加一个「类别」勾选表。
  「过滤方式」两个单选决定命中之后是只显示还是不显示。

  只出代理模式用得到的四个类别（TCP/UDP 各请求响应）。
  注入模式那八个（Send / Recv / WSA*）留给将来 IPC 改造 —— 现在出来也没法用，
  而且 C# 侧保存时是取出结构体改完再写回，那八个原样保留、不会被清掉。
*/
import { computed, ref, watch } from 'vue'
import { call } from '../../bridge'
import { t, type Key } from '../../i18n'
import SettingsModal from './SettingsModal.vue'

const props = defineProps<{ open: boolean }>()
const emit = defineEmits<{ (e: 'update:open', v: boolean): void }>()

const busy = ref(false)
const error = ref('')

interface Form {
  notShow: boolean
  checkSocket: boolean; socketValue: string
  checkIP: boolean; ipValue: string
  checkPort: boolean; portValue: string
  checkHead: boolean; headValue: string
  checkData: boolean; dataValue: string
  checkLen: boolean; lenValue: string
  checkType: boolean
  tcpReq: boolean; tcpResp: boolean; udpReq: boolean; udpResp: boolean
}

const f = ref<Form>({
  notShow: false,
  checkSocket: false, socketValue: '',
  checkIP: false, ipValue: '',
  checkPort: false, portValue: '',
  checkHead: false, headValue: '',
  checkData: false, dataValue: '',
  checkLen: false, lenValue: '',
  checkType: false,
  tcpReq: true, tcpResp: true, udpReq: true, udpResp: true,
})

/** 六个「开关 + 值」的条件，排版完全一样，用一张表驱动。 */
const CONDS: Array<{ on: keyof Form; val: keyof Form; label: Key; ph: Key }> = [
  { on: 'checkSocket', val: 'socketValue', label: 'set.leach.socket', ph: 'set.leach.socketPh' },
  { on: 'checkIP', val: 'ipValue', label: 'set.leach.ip', ph: 'set.leach.ipPh' },
  { on: 'checkPort', val: 'portValue', label: 'set.leach.port', ph: 'set.leach.portPh' },
  { on: 'checkHead', val: 'headValue', label: 'set.leach.head', ph: 'set.leach.headPh' },
  { on: 'checkData', val: 'dataValue', label: 'set.leach.data', ph: 'set.leach.dataPh' },
  { on: 'checkLen', val: 'lenValue', label: 'set.leach.len', ph: 'set.leach.lenPh' },
]

const TYPES: Array<{ key: keyof Form; label: Key }> = [
  { key: 'tcpReq', label: 'pt.tcpReq' },
  { key: 'tcpResp', label: 'pt.tcpResp' },
  { key: 'udpReq', label: 'pt.udpReq' },
  { key: 'udpResp', label: 'pt.udpResp' },
]

/** 一个条件都没开 = 过滤不生效，界面上说清楚，省得以为设了没用。 */
const anyOn = computed(() =>
  f.value.checkSocket || f.value.checkIP || f.value.checkPort
  || f.value.checkHead || f.value.checkData || f.value.checkLen || f.value.checkType)

watch(() => props.open, async (on) => {
  if (!on) return

  error.value = ''
  try {
    f.value = await call<Form>('getLeachSetting')
  } catch (e) {
    console.error('[set] 读取过滤设置失败', e)
  }
})

async function save(): Promise<void> {
  busy.value = true
  error.value = ''

  try {
    const r = await call<any>('saveLeachSetting', { ...f.value })

    if (!r?.ok) {
      error.value = r?.error || ''
      return
    }

    emit('update:open', false)
  } catch (e) {
    console.error('[set] 保存过滤设置失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}
</script>

<template>
  <SettingsModal
    :open="props.open"
    :title="t('set.leach')"
    subtitle="Controls/LeachSetting"
    :busy="busy"
    :error="error"
    @update:open="emit('update:open', $event)"
    @save="save"
  >    <div class="setf" style="--setf-k: 132px">

    <p class="lead">{{ t('set.leach.lead') }}</p>

    <div class="grp">{{ t('set.leach.mode') }}</div>

    <div class="modes">
      <button class="rd" :class="{ on: !f.notShow }" @click="f.notShow = false">
        <i />{{ t('set.leach.only') }}
      </button>
      <button class="rd" :class="{ on: f.notShow }" @click="f.notShow = true">
        <i />{{ t('set.leach.hide') }}
      </button>
    </div>

    <div class="grp">{{ t('set.leach.conds') }}</div>

    <p v-if="!anyOn" class="warn">{{ t('set.leach.none') }}</p>

    <div v-for="c in CONDS" :key="c.on" class="row">
      <button class="chk k" :class="{ on: f[c.on] }" @click="(f[c.on] as boolean) = !f[c.on]">
        <i />{{ t(c.label) }}
      </button>
      <input
        v-model="(f[c.val] as string)"
        class="inp"
        spellcheck="false"
        :disabled="!f[c.on]"
        :placeholder="t(c.ph)"
      >
    </div>

    <div class="grp">{{ t('set.leach.types') }}</div>

    <div class="row">
      <button class="chk k" :class="{ on: f.checkType }" @click="f.checkType = !f.checkType">
        <i />{{ t('set.leach.byType') }}
      </button>
      <div class="types">
        <button
          v-for="x in TYPES"
          :key="x.key"
          class="chk"
          :class="{ on: f[x.key] }"
          :disabled="!f.checkType"
          @click="(f[x.key] as boolean) = !f[x.key]"
        ><i />{{ t(x.label) }}</button>
      </div>
    </div>
    </div>
  </SettingsModal>
</template>

<style scoped>
.lead { margin: 12px 20px 0; font-size: 12.5px; color: var(--muted); line-height: 1.6; }

.warn {
  margin: 0 20px 8px;
  padding: 7px 11px;
  border: 1px solid rgb(234 179 8 / 32%);
  background: rgb(234 179 8 / 7%);
  font-size: 11.5px;
  color: var(--amber);
}

.modes { display: flex; gap: 22px; padding: 0 20px 4px; }

.types { display: flex; flex-wrap: wrap; gap: 8px 18px; }

.chk.k { justify-self: start; }

</style>
