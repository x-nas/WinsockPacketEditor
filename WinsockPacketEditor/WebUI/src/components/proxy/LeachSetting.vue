<script setup lang="ts">
/*
  过滤设置 —— 对应 WinForms 的 Controls/LeachSetting。

  【与「滤镜」不是一回事】这里决定**收不收**这个封包，滤镜决定命中之后**怎么改**。
  过滤发生在入列表之前，所以它是控制列表刷屏最直接的手段。

  六个条件各自可开关、各带一个值；再加一个「类别」勾选表。
  「过滤方式」两个单选决定命中之后是只显示还是不显示。

  【类别按模式分流】WinForms 那一屏是 Inject / Proxy 两个页签共 12 个勾选框，
  同一份 FilterFunction。这里只出当前模式用得到的那一组 ——
  代理模式下摆着 WSASend 之类的选项没有意义（代理路径上根本不会产生那种类型的包）。

  注入那八个覆盖 WinSock 1.1 与 2.0 <b>两套</b>入口：
  CheckFilterFunction_ByPacketType 的映射表把 WS1_Send / WS2_Send 都指向同一个 Send 标志。

  ⚠️ 保存时<b>只送当前这一组</b>，C# 侧收不到的字段原样保留 ——
  否则在代理模式点一次保存就会把注入的八个类别一次清空
  （saveListSetting 当年就是这么把 12 个拦截开关洗成全开的）。
*/
import { computed, ref, watch } from 'vue'
import { call } from '../../bridge'
import { t, type Key } from '../../i18n'
import SettingsModal from './SettingsModal.vue'

const props = withDefaults(
  defineProps<{ open: boolean; mode?: 'proxy' | 'inject' }>(),
  { mode: 'proxy' },
)
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
  //注入模式的八个（WS1 与 WS2 共用一个标志）
  send: boolean; sendTo: boolean; recv: boolean; recvFrom: boolean
  wsaSend: boolean; wsaSendTo: boolean; wsaRecv: boolean; wsaRecvFrom: boolean
  //代理模式的四个
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
  send: true, sendTo: true, recv: true, recvFrom: true,
  wsaSend: true, wsaSendTo: true, wsaRecv: true, wsaRecvFrom: true,
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

/** 文案沿用封包类型那一组键，与列表「类型」列显示的字逐字一致。 */
const PROXY_TYPES: Array<{ key: keyof Form; label: Key }> = [
  { key: 'tcpReq', label: 'pt.tcpReq' },
  { key: 'tcpResp', label: 'pt.tcpResp' },
  { key: 'udpReq', label: 'pt.udpReq' },
  { key: 'udpResp', label: 'pt.udpResp' },
]

const INJECT_TYPES: Array<{ key: keyof Form; label: Key }> = [
  { key: 'send', label: 'pt.ws2Send' },
  { key: 'sendTo', label: 'pt.ws2SendTo' },
  { key: 'recv', label: 'pt.ws2Recv' },
  { key: 'recvFrom', label: 'pt.ws2RecvFrom' },
  { key: 'wsaSend', label: 'pt.wsaSend' },
  { key: 'wsaSendTo', label: 'pt.wsaSendTo' },
  { key: 'wsaRecv', label: 'pt.wsaRecv' },
  { key: 'wsaRecvFrom', label: 'pt.wsaRecvFrom' },
]

const TYPES = computed(() => (props.mode === 'inject' ? INJECT_TYPES : PROXY_TYPES))

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
    /*
      只送当前模式那一组类别。<b>另一组必须整个不出现在报文里</b>（不是送 false）——
      C# 侧按「字段存不存在」决定改不改，送 false 就是明确要求关掉。
    */
    const body: Record<string, unknown> = { ...f.value }
    for (const x of props.mode === 'inject' ? PROXY_TYPES : INJECT_TYPES) delete body[x.key]

    const r = await call<any>('saveLeachSetting', body)

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
    subtitle="Capture Filter"
    :busy="busy"
    :error="error"
    @update:open="emit('update:open', $event)"
    @save="save"
  >    <div class="setf" style="--setf-k: 132px">

    <p class="lead">{{ t('set.leach.lead') }}</p>

    <section class="sec">
    <div class="grp">{{ t('set.leach.mode') }}</div>

    <div class="modes">
      <button class="rd" :class="{ on: !f.notShow }" @click="f.notShow = false">
        <i />{{ t('set.leach.only') }}
      </button>
      <button class="rd" :class="{ on: f.notShow }" @click="f.notShow = true">
        <i />{{ t('set.leach.hide') }}
      </button>
    </div>
    </section>

    <section class="sec">
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
    </section>

    <section class="sec">
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
    </section>
    </div>
  </SettingsModal>
</template>

<style scoped>
.lead { margin: 12px 20px 0; font-size: var(--fs-body); color: var(--muted); line-height: 1.6; }

.warn {
  margin: 0 20px 8px;
  padding: 7px 11px;
  border: 1px solid rgb(var(--amber-rgb) / 32%);
  background: rgb(var(--amber-rgb) / 7%);
  font-size: var(--fs-small);
  color: var(--amber);
}

.modes { display: flex; gap: 22px; padding: 0 20px 4px; }

.types { display: flex; flex-wrap: wrap; gap: 8px 18px; }

.chk.k { justify-self: start; }

</style>
