<script setup lang="ts">
/*
  自动入库的一条规则 —— 对应 WinForms 的 Controls/AutoStoresEdit。

  两个字段：指定包头（十六进制）+ 入库到哪个仓库。

  【校验在 C# 侧】非空 / 是合法十六进制 / 仓库存在 / 包头不重复，都在 Operate.SaveAutoStores_Shell，
  前端只把错误显示出来。两边各写一份就有两套真相。
  （其中「不重复」WinForms 没查 —— 而库里 PacketHead 是事实上的唯一键，
  重复的第二条落库时会被静默丢掉。见那个方法上的说明。）

  【仓库下拉读的是前端副本】FeedList.WareHouse 一直在推，不必再问一次 C#。
*/
import { computed, ref, watch } from 'vue'
import { call } from '../../bridge'
import { FeedList, type AutoStoresRow, type WareHouseRow } from '../../bridge/types'
import { t } from '../../i18n'
import { useList } from '../../stores/lists'
import CyberSelect from '../CyberSelect.vue'
import SettingsModal from './SettingsModal.vue'

const props = defineProps<{
  /** null = 不开。'add' = 新增，其它值 = 要改的那条的 Id */
  target: string | null
  /** 编辑时带进来的当前值 */
  row: AutoStoresRow | null
}>()

const emit = defineEmits<{ (e: 'close'): void }>()

const houses = useList<WareHouseRow>(FeedList.WareHouse)
const houseOptions = computed(() => houses.value.map((h) => ({ value: h.Id.toUpperCase(), label: h.Name })))

const busy = ref(false)
const error = ref('')

const head = ref('')
const wid = ref('')

const isAdd = computed(() => props.target === 'add')
const title = computed(() => t(isAdd.value ? 'as.add' : 'as.edit'))

watch(() => props.target, (v) => {
  if (!v) return

  error.value = ''
  busy.value = false

  if (v === 'add') {
    head.value = ''
    //只有一个仓库就直接选上，省一步
    wid.value = houses.value.length === 1 ? houses.value[0].Id : ''
    return
  }

  head.value = props.row?.PacketHead ?? ''
  wid.value = (props.row?.WareHouseId ?? '').toUpperCase()
})

async function save(): Promise<void> {
  busy.value = true
  error.value = ''

  try {
    const r = await call<{ ok: boolean; error: string }>('saveAutoStores', {
      //新增时不带 id，C# 据此判断是新增还是改
      id: isAdd.value ? '' : props.target,
      head: head.value,
      wid: wid.value,
    })

    if (!r?.ok) {
      error.value = r?.error || ''
      return
    }

    emit('close')
  } catch (e) {
    console.error('[as] 保存规则失败', e)
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
    subtitle="Auto-Store Rule"
    :busy="busy"
    :error="error"
    @update:open="!$event && emit('close')"
    @save="save"
  >
    <div class="row">
      <div class="k">{{ t('as.head') }}</div>
      <div class="v">
        <input v-model="head" class="inp" type="text" spellcheck="false"
               :placeholder="t('as.headPh')" @keydown.enter="save">
      </div>
    </div>

    <p class="hint">{{ t('as.headHint') }}</p>

    <div class="row">
      <div class="k">{{ t('as.wareHouse') }}</div>
      <div class="v">
        <CyberSelect v-model="wid" class="sel" :options="houseOptions" :placeholder="t('as.pickWareHouse')" />
      </div>
    </div>

    <p v-if="!houses.length" class="hint warn">{{ t('as.noWareHouse') }}</p>
  </SettingsModal>
</template>

<style scoped>
.row {
  display: grid;
  grid-template-columns: 92px 1fr;
  align-items: center;
  gap: 12px;
  padding: 5px 20px;
  min-height: 32px;
}

.row > .k { font-size: var(--fs-body); color: var(--muted); }
.row > .v { display: flex; align-items: center; gap: 10px; min-width: 0; }

.hint { padding: 0 20px 0 124px; margin: 0 0 6px; font-size: var(--fs-small); color: var(--dim2); }
.hint.warn { color: var(--amber); padding-left: 20px; margin-top: 6px; }

/* 基样式在 style.css 的 .inp。这里填的是包头十六进制，加一点字距好数字节 */
.inp { flex: 1; min-width: 0; letter-spacing: .04em; }

/* 下拉是自绘的 CyberSelect，这里只给它在这一行里的宽度 */
.sel { flex: 1; min-width: 0; }
</style>
