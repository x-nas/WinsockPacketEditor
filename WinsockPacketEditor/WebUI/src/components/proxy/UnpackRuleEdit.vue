<script setup lang="ts">
/* 一条 TCP 拆包规则；交互和映射规则编辑器保持一致。 */
import { computed, ref, watch } from 'vue'
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
const title = computed(() => '拆包规则 · ' + (isAdd.value ? '新增' : '编辑'))
const DIRECTIONS = [{ value: 0, label: '双向' }, { value: 1, label: '请求' }, { value: 2, label: '响应' }]

watch(() => props.target, (v) => {
  if (!v) return
  error.value = ''
  f.value = v === 'add'
    ? { Id: '', Name: '', IsEnable: true, Direction: 0, Header: '', Length: '' }
    : { ...v }
})

function save(): void {
  const rule = { ...f.value, Name: f.value.Name.trim(), Header: f.value.Header.trim(), Length: f.value.Length.trim() }
  if (!rule.Name || !rule.Header || !rule.Length) { error.value = '请填写名称、包头特征和长度字段位置'; return }
  emit('save', rule)
  emit('close')
}
</script>

<template>
  <SettingsModal :open="!!props.target" :title="title" subtitle="Packet Unpacking" :error="error"
                 @update:open="!$event && emit('close')" @save="save">
    <div class="setf">
      <div class="grp">规则</div>
      <div class="row"><div class="k">名称</div><div class="v"><input v-model="f.Name" class="inp" maxlength="64" spellcheck="false" placeholder="请输入名称"></div></div>
      <div class="row"><div class="k">方向</div><div class="v"><CyberSelect :model-value="f.Direction" :options="DIRECTIONS" @update:model-value="f.Direction = Number($event)" /></div></div>
      <div class="grp">帧格式</div>
      <div class="row"><div class="k">包头特征</div><div class="v"><input v-model="f.Header" class="inp mono" spellcheck="false" placeholder="01 00 00"></div></div>
      <div class="row"><div class="k">长度字段位置</div><div class="v"><input v-model="f.Length" class="inp mono" spellcheck="false" placeholder="4-5"></div></div>
      <p class="hint">长度字段位置从 1 开始编号，按大端解释并表示整个包长度；首个有效帧会固定该 TCP 方向所命中的规则。</p>
    </div>
  </SettingsModal>
</template>

<style scoped>
.mono { font-family: var(--mono); }
</style>
