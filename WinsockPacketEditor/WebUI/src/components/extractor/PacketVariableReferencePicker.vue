<script setup lang="ts">
/*
  取值器变量引用插入器。
  这里集中生成 ${取值器.变量[:格式]}，编辑器不再各自拼字符串，避免少括号、错名称或
  在动态替换中漏掉数值字节格式。template 模式下数值变量必须先选一个明确的编码格式。
*/
import { computed, ref } from 'vue'
import { t } from '../../i18n'
import { ensurePacketExtractors, extractorRows, type Variable } from '../../stores/extractor'
import { formatOptions } from './formats'
import SettingsModal from '../proxy/SettingsModal.vue'
import CyberSelect from '../CyberSelect.vue'

const props = withDefaults(defineProps<{
  /** expression：普通表达式；bytes：字节数组表达式；template：动态替换模板。 */
  mode?: 'expression' | 'bytes' | 'template'
  /** 表达式结果类型；候选类型与运行时的转换规则保持一致。 */
  dataType?: number | null
  /** 正在编辑但尚未保存的取值器；让新建的变量能立刻被同一编辑器引用。 */
  extraExtractor?: import('../../stores/extractor').Extractor | null
}>(), { mode: 'expression', dataType: null, extraExtractor: null })
const emit = defineEmits<{ (e: 'insert', text: string): void }>()

const open = ref(false)
const keyword = ref('')
const selectedFormats = ref<Record<string, string>>({})

const sources = computed(() => {
  if (!props.extraExtractor) return extractorRows.value
  // 用草稿替换同 GUID 的已保存版本：当前窗口新增/改名的变量无需先保存再打开插入器。
  return [...extractorRows.value.filter(x => x.Id !== props.extraExtractor!.Id), props.extraExtractor]
})
const variables = computed(() => sources.value
  .flatMap(extractor => extractor.Variables.map(variable => ({ extractor, variable })))
  .filter(x => props.dataType === null || props.dataType === 2 || props.dataType === 3 || (props.dataType === 1 ? x.variable.DataType === 0 || x.variable.DataType === 1 : x.variable.DataType === props.dataType))
  .filter(x => !keyword.value.trim() || (x.extractor.Name + ' ' + x.variable.Name).toLowerCase().includes(keyword.value.trim().toLowerCase())))

function typeName(type: number): string { return [t('pex.typeInt'), t('pex.typeFloat'), t('pex.typeBytes'), t('pex.typeText')][type] || t('pex.unknownType') }
function kindName(kind: number): string { return [t('pex.kindSimple'), t('pex.kindCapture'), t('pex.kindExpression')][kind] || t('pex.unknownType') }
/** 编辑中的草稿尚未走后端，因此简单值直接使用它的配置值作为当前值。 */
function currentValue(variable: Variable): string {
  if (variable.CurrentValue !== undefined && variable.CurrentValue !== '') return variable.CurrentValue
  return variable.Kind === 0 ? (variable.Value || '—') : '—'
}
const supportHint = computed(() => {
  if (props.mode === 'template') return t('pex.hintTemplate')
  switch (props.dataType) {
    case 0: return t('pex.hintInt')
    case 1: return t('pex.hintFloat')
    case 2: return t('pex.hintBytes')
    case 3: return t('pex.hintText')
    default: return t('pex.hintDefault')
  }
})
function formats(variable: Variable): Array<{ label: string; value: string }> {
  return formatOptions(variable.DataType)
}
function formatKey(extractorId: string, variableId: string): string { return extractorId + ':' + variableId }
function formatValue(extractorId: string, variable: Variable): string { return selectedFormats.value[formatKey(extractorId, variable.Id)] ?? formats(variable)[0].value }
function setFormat(extractorId: string, variableId: string, value: string | number): void { selectedFormats.value[formatKey(extractorId, variableId)] = String(value) }

async function show(): Promise<void> {
  if (open.value) return
  open.value = true
  keyword.value = ''
  await ensurePacketExtractors()
}
function insert(extractorName: string, variable: Variable, format = ''): void {
  emit('insert', '${' + extractorName + '.' + variable.Name + (format ? ':' + format : '') + '}')
  open.value = false
}
function needsFormat(variable: Variable): boolean { return props.mode === 'template' || (props.mode === 'bytes' && variable.DataType !== 2) }
function hasFormatChoice(variable: Variable): boolean { return needsFormat(variable) && formats(variable).length > 1 }
</script>

<template>
  <span class="ref-picker">
    <button type="button" class="ref-open" :title="t('pex.refOpenTitle')" @mousedown.prevent @click="show">{{ t('pex.refOpen') }}</button>
  </span>
  <SettingsModal :open="open" :title="t('pex.refTitle')" subtitle="Controls/PacketVariableReferencePicker" :width="820" readonly :cancel-text="t('dlg.close')" :hint="supportHint" @update:open="open = $event">
    <div class="ref-dialog">
      <input v-model="keyword" class="inp ref-search" :placeholder="t('pex.refSearch')" autofocus />
      <div class="ref-table">
        <div class="ref-head"><span>{{ t('pex.refExtractor') }}</span><span>{{ t('pex.colVarName') }}</span><span>{{ t('pex.refKindName') }}</span><span>{{ t('pex.dataType') }}</span><span>{{ t('pex.colCurrent') }}</span><span /></div>
        <div v-if="!variables.length" class="ref-empty">{{ t('pex.refEmpty') }}</div>
        <div v-for="item in variables" v-else :key="item.extractor.Id + item.variable.Id" class="ref-row" :class="{ off: !item.extractor.IsEnable && item.extractor.Id !== extraExtractor?.Id }">
          <span :title="item.extractor.Name">{{ item.extractor.Name }}</span><span class="ref-var" :title="item.variable.Name">{{ item.variable.Name }}</span><span class="ref-kind">{{ kindName(item.variable.Kind) }}</span>
          <span class="ref-type">{{ !item.extractor.IsEnable && item.extractor.Id !== extraExtractor?.Id ? t('pex.disabled') : typeName(item.variable.DataType) }}</span><span class="ref-value" :title="currentValue(item.variable)">{{ currentValue(item.variable) }}</span>
          <span class="ref-actions"><template v-if="hasFormatChoice(item.variable)"><CyberSelect class="ref-format" :model-value="formatValue(item.extractor.Id, item.variable)" :options="formats(item.variable)" :disabled="!item.extractor.IsEnable && item.extractor.Id !== extraExtractor?.Id" @update:model-value="setFormat(item.extractor.Id, item.variable.Id, $event)" /><button type="button" class="ref-add" :disabled="!item.extractor.IsEnable && item.extractor.Id !== extraExtractor?.Id" @click="insert(item.extractor.Name, item.variable, formatValue(item.extractor.Id, item.variable))">{{ t('pex.insert') }}</button></template><button v-else type="button" class="ref-add" :disabled="!item.extractor.IsEnable && item.extractor.Id !== extraExtractor?.Id" @click="insert(item.extractor.Name, item.variable, needsFormat(item.variable) ? formats(item.variable)[0].value : '')">{{ t('pex.insert') }}</button></span>
        </div>
      </div>
    </div>
  </SettingsModal>
</template>

<style scoped>
.ref-picker{display:inline-flex;flex:0 0 auto;align-self:center}.ref-open{box-sizing:border-box!important;align-self:center!important;min-height:0!important;width:70px!important;height:28px!important;padding:0!important;border:1px solid rgb(var(--cyan-rgb) / 68%);background:rgb(var(--cyan-rgb) / 7%);color:var(--cyan);font:10px/1 var(--mono)!important;white-space:nowrap}.ref-open:hover{background:rgb(var(--cyan-rgb) / 15%);border-color:var(--cyan)}.ref-dialog{padding:2px 2px 0}.ref-search{box-sizing:border-box;width:100%;margin-bottom:10px}.ref-table{border:1px solid var(--border);max-height:450px;overflow:auto}.ref-head,.ref-row{display:grid;grid-template-columns:minmax(76px,.55fr) minmax(94px,.75fr) 82px 68px minmax(98px,.85fr) minmax(176px,1.25fr);align-items:center;gap:10px;padding:0 12px}.ref-head{height:35px;background:var(--bar);color:var(--dim);font:var(--fs-small) var(--mono)}.ref-row{min-height:39px;border-top:1px solid var(--border);color:var(--gray)}.ref-row>span{min-width:0;overflow:hidden;text-overflow:ellipsis;white-space:nowrap}.ref-row.off{opacity:.45}.ref-var{color:var(--cyan)}.ref-kind,.ref-type{color:var(--dim);font-size:var(--fs-small)}.ref-value{color:var(--text);font:var(--fs-small) var(--mono)}.ref-actions{display:flex;align-items:center;justify-content:flex-end;gap:6px;overflow:visible!important;white-space:normal!important}.ref-format{width:108px;flex:0 0 108px}.ref-add{height:25px;padding:0 8px;border:1px solid rgb(var(--amber-rgb) / 65%);background:rgb(var(--amber-rgb) / 7%);color:var(--amber);font:var(--fs-small) var(--mono);cursor:pointer}.ref-add:hover{border-color:var(--amber);background:rgb(var(--amber-rgb) / 15%)}.ref-add:disabled{cursor:not-allowed}.ref-empty{margin:0;padding:18px 12px;color:var(--dim);font-size:var(--fs-small);line-height:1.55}
</style>
