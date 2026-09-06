<script setup lang="ts">
/*
  列表设置 —— 对应 WinForms 的 Controls/ListSetting。

  【比 WinForms 多一样】那边只管列显隐，自动清理是 ProxyList 工具条上的两个控件。
  这里并进同一个弹窗 —— 它们讲的是同一件事：这张表怎么显示、留多少。
  工具条上仍然显示当前值，只是改要到这里来。

  【比 WinForms 少三样】Id / 时间 / 数据不给关：
  Id 是取字节的钥匙（getPacketDetail 靠它），时间和数据是这张表的意义所在，
  关掉等于把列表变成一堆地址。WinForms 允许关，那是历史遗留，不照搬。
*/
import { ref, watch } from 'vue'
import { call } from '../../bridge'
import { t, type Key } from '../../i18n'
import { listSetting } from '../../stores/runtime'
import SettingsModal from './SettingsModal.vue'

const props = defineProps<{ open: boolean }>()
const emit = defineEmits<{ (e: 'update:open', v: boolean): void }>()

const busy = ref(false)
const error = ref('')

/** 可关的七列。顺序与表里一致，扫下来能对上。 */
const COLS: Array<{ key: keyof typeof form.value; label: Key }> = [
  { key: 'showSocket', label: 'col.socket' },
  { key: 'showType', label: 'col.type' },
  { key: 'showClientAddr', label: 'col.client' },
  { key: 'showClientLoc', label: 'col.clientLoc' },
  { key: 'showServerAddr', label: 'col.server' },
  { key: 'showServerLoc', label: 'col.serverLoc' },
  { key: 'showLen', label: 'col.len' },
]

const form = ref({
  showSocket: true,
  showType: true,
  showClientAddr: true,
  showClientLoc: true,
  showServerAddr: true,
  showServerLoc: true,
  showLen: true,
  autoClear: true,
  autoClearValue: 5000,
})

watch(() => props.open, async (on) => {
  if (!on) return

  error.value = ''
  try {
    form.value = await call<typeof form.value>('getListSetting')
  } catch (e) {
    console.error('[set] 读取列表设置失败', e)
  }
})

async function save(): Promise<void> {
  busy.value = true
  error.value = ''

  try {
    const r = await call<any>('saveListSetting', {
      ...form.value,
      autoClearValue: Number(form.value.autoClearValue),
    })

    if (!r?.ok) {
      error.value = r?.error || ''
      return
    }

    //列表要立刻按新的列显隐重画，所以把结果写进共享状态
    listSetting.value = { ...form.value, autoClearValue: Number(form.value.autoClearValue) }
    emit('update:open', false)
  } catch (e) {
    console.error('[set] 保存列表设置失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}
</script>

<template>
  <SettingsModal
    :open="props.open"
    :title="t('set.list')"
    subtitle="Controls/ListSetting"
    :busy="busy"
    :error="error"
    @update:open="emit('update:open', $event)"
    @save="save"
  >    <div class="setf" style="--setf-k: 132px">

    <div class="grp">{{ t('set.grp.cols') }}</div>
    <p class="hint">{{ t('set.colsHint') }}</p>

    <div class="cols">
      <button
        v-for="c in COLS"
        :key="c.key"
        class="chk"
        :class="{ on: form[c.key] }"
        @click="(form[c.key] as boolean) = !form[c.key]"
      ><i />{{ t(c.label) }}</button>
    </div>

    <div class="grp">{{ t('set.grp.autoClear') }}</div>

    <div class="row">
      <div class="k">{{ t('proxy.autoClear') }}</div>
      <div class="v">
        <button class="chk" :class="{ on: form.autoClear }" @click="form.autoClear = !form.autoClear">
          <i />{{ t('set.autoClearOn') }}
        </button>
      </div>
    </div>

    <div class="row">
      <div class="k">{{ t('set.keepRows') }}</div>
      <div class="v">
        <input v-model.number="form.autoClearValue" class="inp num" type="number"
               min="100" max="500000" :disabled="!form.autoClear">
        <span class="tip">{{ t('set.keepRowsHint') }}</span>
      </div>
    </div>
    </div>
  </SettingsModal>
</template>

<style scoped>

/* 七个列开关排成两列，比一行一个紧凑得多 */
.cols {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 8px 16px;
  padding: 0 20px 4px;
}

/*
  ⚠️ 这行提示<b>要换行，不能省略号</b>。中文写得下的一句，换成俄语 / 越南语常常长一倍
  （「100–500000. При достижении очищается весь список, а не только старые строки」），
  nowrap + ellipsis 会把后半句直接吃掉，而那半句正是要紧的部分。
  外面的 .setf .row > .v 已经是 flex-wrap: wrap，让它自己折下去即可。
*/
.tip { font-size: 11px; color: #4b5563; line-height: 1.5; flex: 1 1 100%; }

</style>
