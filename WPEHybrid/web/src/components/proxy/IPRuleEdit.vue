<script setup lang="ts">
/*
  白 / 黑名单的一条 —— 对应 WinForms 的 Controls/WhiteListEdit 与 BlackListEdit。

  【一个组件顶两个】那两个 UserControl 各 260 行、逐行几乎相同，只有取哪张表不一样。
  这里靠 black 参数分流 —— 复制一份出来只会让以后每改一处都要改两遍。

  【IP 段是拼出来的，不是两个字段】源模型只有一个 IPAddress 串：
  单个 IP 就是 "1.2.3.4"，段是 "1.2.3.4-1.2.3.9"。所以界面上给两个输入框，
  提交时按这个格式拼起来 —— 与 WhiteListEdit 的 bSave 一致。

  【校验在 C# 侧】IsValidIPv4 与"是否已在名单里"都在 Operate.SaveIPRule，
  前端只把错误显示出来。两边各写一份就有两套真相。
*/
import { computed, ref, watch } from 'vue'
import { call } from '../../bridge'
import { t } from '../../i18n'
import SettingsModal from './SettingsModal.vue'

const props = defineProps<{
  /** null = 不开。'add' = 新增，其它值 = 要改的那条的 IP */
  target: string | null
  black: boolean
  /** 编辑时带进来的当前值 */
  isExpiry: boolean
  expiry: string
}>()

const emit = defineEmits<{
  (e: 'close'): void
  (e: 'saved'): void
}>()

const busy = ref(false)
const error = ref('')

const isRange = ref(false)
const single = ref('')
const from = ref('')
const to = ref('')
const hasExpiry = ref(false)
const expiry = ref('')

const isAdd = computed(() => props.target === 'add')

const title = computed(() =>
  t(props.black ? 'fw.blackList' : 'fw.whiteList') + ' · ' + t(isAdd.value ? 'fw.add' : 'fw.edit'))

/** 到期时间输入框要 "yyyy-MM-ddTHH:mm"，C# 给的是 "yyyy-MM-dd HH:mm:ss"。 */
function toLocalInput(s: string): string {
  const v = (s || '').trim().replace(' ', 'T')
  return v.length >= 16 ? v.slice(0, 16) : ''
}

watch(() => props.target, (v) => {
  if (!v) return

  error.value = ''
  busy.value = false

  if (v === 'add') {
    isRange.value = false
    single.value = ''
    from.value = ''
    to.value = ''
    hasExpiry.value = false
    //默认给一天后，比空着好填
    expiry.value = toLocalInput(new Date(Date.now() + 86400000).toISOString().replace('T', ' '))
    return
  }

  //编辑：把 "起-止" 拆回两个框
  const parts = v.split('-')
  isRange.value = parts.length === 2
  single.value = isRange.value ? '' : v
  from.value = isRange.value ? parts[0].trim() : ''
  to.value = isRange.value ? parts[1].trim() : ''
  hasExpiry.value = props.isExpiry
  expiry.value = toLocalInput(props.expiry)
})

async function save(): Promise<void> {
  busy.value = true
  error.value = ''

  const ip = isRange.value
    ? from.value.trim() + '-' + to.value.trim()
    : single.value.trim()

  try {
    const r = await call<{ ok: boolean; error: string }>('saveIPRule', {
      black: props.black,
      //新增时不带 oldIp，C# 据此判断是新增还是改
      oldIp: isAdd.value ? '' : props.target,
      ip,
      isExpiry: hasExpiry.value,
      //<input type="datetime-local"> 给的是 "yyyy-MM-ddTHH:mm"，C# 的 DateTime.TryParse 认
      expiry: hasExpiry.value ? expiry.value.replace('T', ' ') : '',
    })

    if (!r?.ok) {
      error.value = r?.error || ''
      return
    }

    emit('saved')
    emit('close')
  } catch (e) {
    console.error('[fw] 保存名单失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}
</script>

<template>
  <SettingsModal
    :open="!!props.target"
    :title="title"
    subtitle="IP Rule"
    :busy="busy"
    :error="error"
    @update:open="!$event && emit('close')"
    @save="save"
  >    <div class="setf" style="--setf-k: 92px">

    <div class="row">
      <div class="k">{{ t('fw.ipKind') }}</div>
      <div class="v">
        <button class="rd" :class="{ on: !isRange }" @click="isRange = false">
          <i />{{ t('fw.single') }}
        </button>
        <button class="rd" :class="{ on: isRange }" @click="isRange = true">
          <i />{{ t('fw.range') }}
        </button>
      </div>
    </div>

    <div class="row">
      <div class="k">{{ t('cli.ip') }}</div>
      <div class="v">
        <input v-if="!isRange" v-model="single" class="inp" spellcheck="false" placeholder="192.168.1.100">
        <template v-else>
          <input v-model="from" class="inp" spellcheck="false" placeholder="192.168.1.1">
          <span class="dash">—</span>
          <input v-model="to" class="inp" spellcheck="false" placeholder="192.168.1.254">
        </template>
      </div>
    </div>

    <div class="row">
      <div class="k">{{ t('fw.expiry') }}</div>
      <div class="v">
        <button class="chk" :class="{ on: hasExpiry }" @click="hasExpiry = !hasExpiry">
          <i />{{ t('fw.hasExpiry') }}
        </button>
        <input v-model="expiry" class="inp dt" type="datetime-local" :disabled="!hasExpiry">
      </div>
    </div>

    <p class="hint">{{ t('fw.expiryHint') }}</p>
    </div>
  </SettingsModal>
</template>

<style scoped>

.dash { color: var(--dim); }

.inp.dt { flex: none; width: 200px; }

/*
  日期框的日历按钮在深色底上默认是黑的，几乎看不见 —— 反相提亮。
  与账号列表的到期区间同一处理。
*/
.inp.dt::-webkit-calendar-picker-indicator { filter: invert(.7); cursor: pointer; }
</style>
