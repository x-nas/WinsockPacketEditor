<script setup lang="ts">
/*
  批量调整 —— 对应 WinForms 的 Controls/{ExpiryTime, LimitLinks, LimitDevices} 三个弹窗。

  三个合成一个：链接数与设备数的表单完全一样（一个开关 + 一个数字），
  过期时间多两组单选。拆成三份组件只会多两份要同步的样式。

  【过期时间是「加」不是「设」】WinForms 那边就是这个语义：
  给选中的账号各自加 N 小时/天，而不是把它们统一设成某个时刻 ——
  续期本来就是按人各自的到期日往后推。所以这里没有日期选择器。

  「基于原有时间」= 从各自现在的到期日往后加（已过期的会从过去的时间点算起，
  加完可能仍是过期的）；「基于当前时间」= 已过期的从此刻重新起算。
  这两条的差别只在已过期的账号上，界面里直说了。
*/
import { computed, ref, watch } from 'vue'
import { call } from '../../bridge'
import { t } from '../../i18n'
import SettingsModal from './SettingsModal.vue'

/*
  kind 三态直接写在 props 上，不 export 一个类型出去 ——
  <script setup> 里不能写 export（那正是 pages.ts / settings.ts 单独成文件的原因），
  而这个联合类型只有账号列表一处在用，为它再开一个文件不划算。
*/
const props = defineProps<{ kind: 'expiry' | 'links' | 'devices' | null; ids: string[] }>()
const emit = defineEmits<{ (e: 'close'): void }>()

const busy = ref(false)
const error = ref('')

//过期时间
const amount = ref(1)
/** 'h' | 'd' —— 天在这里换算成小时，C# 侧只认小时 */
const unit = ref<'h' | 'd'>('h')
/** 0 = 基于原有时间，1 = 基于当前时间。与 Operate.AdjustExpiryTime 的 AddType 一致 */
const addType = ref(0)

//链接数 / 设备数
const on = ref(true)
const value = ref(1)

const title = computed(() => {
  if (props.kind === 'expiry') return t('acct.adj.expiry')
  if (props.kind === 'devices') return t('acct.adj.devices')
  return t('acct.adj.links')
})

const subtitle = computed(() => {
  if (props.kind === 'expiry') return 'Batch · Expiry'
  if (props.kind === 'devices') return 'Batch · Devices'
  return 'Batch · Links'
})

//每次打开都回到默认值：上一次调的是别的一批账号，留着容易误按
watch(() => props.kind, (k) => {
  if (!k) return
  error.value = ''
  amount.value = 1
  unit.value = 'h'
  addType.value = 0
  on.value = true
  value.value = 1
})

async function save(): Promise<void> {
  busy.value = true
  error.value = ''

  try {
    const r = props.kind === 'expiry'
      ? await call<any>('adjustAccountExpiry', {
        ids: props.ids,
        hours: (Number(amount.value) || 0) * (unit.value === 'd' ? 24 : 1),
        addType: addType.value,
      })
      : await call<any>('adjustAccountLimit', {
        ids: props.ids,
        devices: props.kind === 'devices',
        on: on.value,
        value: Number(value.value) || 0,
      })

    if (!r?.ok) {
      error.value = r?.error || ''
      return
    }

    emit('close')
  } catch (e) {
    console.error('[acct] 批量调整失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}

</script>

<template>
  <SettingsModal
    :open="props.kind !== null"
    :title="title"
    :subtitle="subtitle"
    :busy="busy"
    :error="error"
    @update:open="emit('close')"
    @save="save"
  >
    <p class="lead">{{ t('acct.adj.scope').replace('{0}', String(props.ids.length)) }}</p>

    <!-- 过期时间：加多少 + 从哪算起 -->
    <template v-if="props.kind === 'expiry'">
      <div class="grp">{{ t('acct.adj.add') }}</div>

      <div class="row">
        <div class="k">{{ t('acct.adj.amount') }}</div>
        <div class="v">
          <input v-model.number="amount" class="inp num" type="number" min="1" max="99999999">
          <button class="rd" :class="{ on: unit === 'h' }" @click="unit = 'h'"><i />{{ t('acct.adj.hour') }}</button>
          <button class="rd" :class="{ on: unit === 'd' }" @click="unit = 'd'"><i />{{ t('acct.adj.day') }}</button>
        </div>
      </div>

      <div class="grp">{{ t('acct.adj.base') }}</div>

      <div class="modes">
        <button class="rd" :class="{ on: addType === 0 }" @click="addType = 0">
          <i />{{ t('acct.adj.fromExpiry') }}
        </button>
        <button class="rd" :class="{ on: addType === 1 }" @click="addType = 1">
          <i />{{ t('acct.adj.fromNow') }}
        </button>
      </div>

      <p class="hint">{{ t('acct.adj.baseHint') }}</p>
    </template>

    <!-- 链接数 / 设备数：一个开关 + 一个数字 -->
    <template v-else>
      <div class="grp">{{ title }}</div>

      <div class="row">
        <button class="chk k" :class="{ on }" @click="on = !on"><i />{{ t('acct.adj.limitOn') }}</button>
        <div class="v">
          <input v-model.number="value" class="inp num" type="number" min="1" max="10000" :disabled="!on">
          <span class="tip">{{ on ? '' : t('acct.unlimited') }}</span>
        </div>
      </div>
    </template>
  </SettingsModal>
</template>

<style scoped>
.lead { margin: 14px 20px 0; font-size: var(--fs-body); color: var(--cyan); }

.grp {
  font-family: var(--share);
  font-size: var(--fs-caption);
  letter-spacing: .26em;
  text-transform: uppercase;
  color: var(--dim);
  padding: 0 20px;
  margin: 16px 0 6px;
}

.hint { margin: 6px 20px 0; font-size: var(--fs-small); color: var(--dim); line-height: 1.6; }

.row {
  display: grid;
  grid-template-columns: 132px 1fr;
  align-items: center;
  gap: 12px;
  padding: 5px 20px;
  min-height: 32px;
}

.row > .k { font-size: var(--fs-body); color: var(--muted); }
.row > .v { display: flex; align-items: center; gap: 14px; min-width: 0; }

.modes { display: flex; gap: 22px; padding: 0 20px 4px; }
.tip { font-size: var(--fs-small); color: var(--dim); }

/* 基样式在 style.css 的「勾选框 / 单选框」，这里只覆盖框线色与布局 */
.chk, .rd { --chk-ring: var(--border); }
.chk.k { justify-self: start; }

/* 基样式在 style.css 的 .inp，这一屏没有需要覆盖的 */

.inp.num { width: 120px; font-variant-numeric: tabular-nums; }
</style>
