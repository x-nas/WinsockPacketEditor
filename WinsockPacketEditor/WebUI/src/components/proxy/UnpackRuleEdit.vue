<script setup lang="ts">
/* 一条 TCP 拆包规则；交互和映射规则编辑器保持一致。 */
import { computed, ref, watch } from 'vue'
import { t } from '../../i18n'
import CyberSelect from '../CyberSelect.vue'
import SettingsModal from './SettingsModal.vue'

export interface UnpackRuleRow {
  Id: string
  Name: string
  IsEnable: boolean
  Direction: number
  Header: string
  Length: string
}

const props = defineProps<{ target: UnpackRuleRow | null | 'add' }>()
const emit = defineEmits<{ (e: 'close'): void; (e: 'save', rule: UnpackRuleRow): void }>()

const error = ref('')
const f = ref<UnpackRuleRow>({ Id: '', Name: '', IsEnable: true, Direction: 0, Header: '01 00 00', Length: '4-5' })
const isAdd = computed(() => props.target === 'add')
const title = computed(() => t('set.hook.unpackRule') + ' · ' + t(isAdd.value ? 'fw.add' : 'fw.edit'))
//computed 而不是常量：语言切换时 t() 依赖的 ref 变化，选项要跟着重算
const DIRECTIONS = computed(() => [
  { value: 0, label: t('set.hook.ruleDirBoth') },
  { value: 1, label: t('pt.req') },
  { value: 2, label: t('pt.resp') },
])

watch(() => props.target, (v) => {
  if (!v) return
  error.value = ''
  f.value = v === 'add'
    ? { Id: '', Name: '', IsEnable: true, Direction: 0, Header: '', Length: '' }
    : { ...v }
})

function save(): void {
  const rule = { ...f.value, Name: f.value.Name.trim(), Header: f.value.Header.trim(), Length: f.value.Length.trim() }
  if (!rule.Name || !rule.Header || !rule.Length) { error.value = t('set.hook.ruleRequired'); return }
  emit('save', rule)
  emit('close')
}
</script>

<template>
  <SettingsModal :open="!!props.target" :title="title" subtitle="Packet Unpacking" :error="error"
                 @update:open="!$event && emit('close')" @save="save">
    <div class="setf">
      <div class="grp">{{ t('set.hook.grpRule') }}</div>
      <div class="row"><div class="k">{{ t('dec.name') }}</div><div class="v"><input v-model="f.Name" class="inp" maxlength="64" spellcheck="false" :placeholder="t('set.hook.ruleNamePh')"></div></div>
      <div class="row"><div class="k">{{ t('dec.direction') }}</div><div class="v"><CyberSelect :model-value="f.Direction" :options="DIRECTIONS" @update:model-value="f.Direction = Number($event)" /></div></div>
      <div class="grp">{{ t('set.hook.grpFrame') }}</div>
      <div class="row"><div class="k">{{ t('set.hook.head') }}</div><div class="v"><input v-model="f.Header" class="inp mono" spellcheck="false" :placeholder="t('set.hook.headPh')"></div></div>
      <div class="row"><div class="k">{{ t('set.hook.length') }}</div><div class="v"><input v-model="f.Length" class="inp mono" spellcheck="false" :placeholder="t('set.hook.lengthPh')"></div></div>
      <p class="hint">{{ t('set.hook.ruleHint') }}</p>
    </div>
  </SettingsModal>
</template>

<style scoped>
.mono { font-family: var(--mono); }
</style>
