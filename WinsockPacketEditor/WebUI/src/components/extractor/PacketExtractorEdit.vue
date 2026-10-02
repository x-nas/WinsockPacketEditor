<script setup lang="ts">
import { computed, nextTick, ref, watch } from 'vue'
import { call } from '../../bridge'
import { t } from '../../i18n'
import CyberSelect from '../CyberSelect.vue'
import PacketVariableReferencePicker from './PacketVariableReferencePicker.vue'
import SettingsModal from '../proxy/SettingsModal.vue'
import type { Extractor, Variable } from '../../stores/extractor'

const props = defineProps<{ target: Extractor | null }>()
const emit = defineEmits<{ (e: 'close'): void; (e: 'saved'): void }>()

const f = ref<Extractor | null>(null)
const error = ref('')
const busy = ref(false)

const scopeOptions = computed(() => [
  { value: 0, label: t('pex.scopeGlobal') },
  { value: 1, label: t('pex.scopeSocket') },
  { value: 2, label: t('pex.scopeSession') },
])
const typeOptions = computed(() => [
  { value: 0, label: t('pex.typeInt') }, { value: 1, label: t('pex.typeFloat') },
  { value: 2, label: t('pex.typeBytes') }, { value: 3, label: t('pex.typeText') },
])

function clone<T>(value: T): T { return JSON.parse(JSON.stringify(value)) as T }

/*
  当前值属于运行时快照，不是编辑表单数据。弹窗打开后只同步这个字段：
  用户正在改名称 / 表达式 / 取值规则时，绝不能被后台刷新整份表单而覆盖。
*/
let refreshingCurrentValues = false
async function refreshCurrentValues(): Promise<void> {
  if (!f.value || refreshingCurrentValues) return
  refreshingCurrentValues = true
  const editingId = f.value.Id.toLowerCase()
  try {
    const rows = (await call<{ rows: Extractor[] }>('getPacketExtractors')).rows || []
    // 请求回来时窗口可能已经关闭，或已经切换到另一条取值器。
    if (!f.value || f.value.Id.toLowerCase() !== editingId) return
    const latest = rows.find(x => x.Id.toLowerCase() === editingId)
    if (!latest) return
    const values = new Map(latest.Variables.map(x => [x.Id.toLowerCase(), x.CurrentValue]))
    for (const variable of f.value.Variables) {
      const key = variable.Id.toLowerCase()
      if (values.has(key)) variable.CurrentValue = values.get(key)
    }
  } catch (e) {
    // 当前值只是显示快照；瞬时读取失败不打断用户的编辑，也不污染保存错误区。
    console.warn('[extractor] 刷新当前值失败', e)
  } finally { refreshingCurrentValues = false }
}
watch(() => props.target, (value) => {
  f.value = value ? clone(value) : null
  error.value = ''
  if (value) void refreshCurrentValues()
}, { immediate: true })

const variableEdit = ref<number | null>(null)
const variableDraft = ref<Variable | null>(null)
const expressionInput = ref<HTMLInputElement | null>(null)
function newVariable(kind: number): Variable {
  const n = (f.value?.Variables.length ?? 0) + 1
  return {
    // C# 端用 Guid；空字符串无法反序列化，Guid.Empty 会在保存时由后端分配正式 ID。
    Id: '00000000-0000-0000-0000-000000000000', Name: t('pex.variableName').replace('{0}', String(n)), Kind: kind, DataType: kind === 1 ? 2 : 3,
    Value: '', Extraction: { Offset: 0, Length: 1, RelativeToMatch: false, BigEndian: false, Signed: false, Encoding: 0 }, TtlSeconds: 0,
  }
}
function addVariable(kind: number): void { variableEdit.value = -1; variableDraft.value = newVariable(kind) }
function editVariable(index: number): void { if (!f.value) return; variableEdit.value = index; variableDraft.value = clone(f.value.Variables[index]) }
function saveVariable(): void { if (!f.value || !variableDraft.value || variableEdit.value === null) return; if (variableEdit.value < 0) f.value.Variables.push(variableDraft.value); else f.value.Variables.splice(variableEdit.value, 1, variableDraft.value); variableEdit.value = null; variableDraft.value = null }
function removeVariable(index: number): void { f.value?.Variables.splice(index, 1) }
function kindText(kind: number): string { return kind === 0 ? t('pex.kindSimple') : kind === 1 ? t('pex.kindCapture') : t('pex.kindExpression') }
function summary(v: Variable): string { return v.Kind === 1 ? t(v.Extraction.RelativeToMatch ? 'pex.summaryMatch' : 'pex.summaryHeader').replace('{0}', String(v.Extraction.Offset)).replace('{1}', String(v.Extraction.Length)) : (v.Value || '—') }
/** 后端为尚未取到的值返回「—」；编辑表统一按本页约定显示半角 -。 */
function currentValue(v: Variable): string { return v.CurrentValue && v.CurrentValue !== '—' ? v.CurrentValue : '-' }
function valuePlaceholder(type: number): string
{
  switch (type) {
    case 0: return t('pex.phInt')
    case 1: return t('pex.phFloat')
    case 2: return t('pex.phBytes')
    default: return t('pex.phText')
  }
}

function expressionPlaceholder(type: number): string
{
  switch (type) {
    case 0: return t('pex.exInt')
    case 1: return t('pex.exFloat')
    case 2: return t('pex.exBytes')
    default: return t('pex.exText')
  }
}

/** 在输入框的当前光标处插入，按钮不会抢走已记录的选择区。 */
function insertExpression(text: string): void {
  if (!variableDraft.value) return
  const input = expressionInput.value
  const start = input?.selectionStart ?? variableDraft.value.Value.length
  const end = input?.selectionEnd ?? start
  variableDraft.value.Value = variableDraft.value.Value.slice(0, start) + text + variableDraft.value.Value.slice(end)
  void nextTick(() => { expressionInput.value?.focus(); expressionInput.value?.setSelectionRange(start + text.length, start + text.length) })
}

async function save(): Promise<void> {
  if (!f.value) return
  busy.value = true; error.value = ''
  try {
    const result = await call<{ ok: boolean; error?: string }>('savePacketExtractor', { extractor: f.value })
    if (!result.ok) { error.value = result.error || t('pex.saveFail'); return }
    emit('saved'); emit('close')
  } catch (e) { error.value = String(e) } finally { busy.value = false }
}
</script>

<template>
  <SettingsModal :open="!!f" :title="t('pex.editTitle')" subtitle="Controls/PacketExtractorEdit" :width="850" :busy="busy" :error="error" @update:open="!$event && emit('close')" @save="save">
    <template v-if="f">
      <div class="setf ex list-page">
      <section class="sec">
        <h3><b>01</b> {{ t('pex.secBasic') }}</h3>
        <div class="form">
          <label>{{ t('pex.name') }}</label><input class="inp" v-model="f.Name" maxlength="64" />
          <label>{{ t('pex.scope') }}</label><CyberSelect v-model="f.Scope" :options="scopeOptions" />
          <label>{{ t('pex.desc') }}</label><input class="inp" v-model="f.Description" maxlength="256" />
        </div>
      </section>

      <section class="sec vars">
        <h3><b>02</b> {{ t('pex.variables') }} <span class="actions"><button class="sbtn" @click="addVariable(0)">{{ t('pex.kindSimple') }}</button><button class="sbtn" @click="addVariable(1)">{{ t('pex.kindCapture') }}</button><button class="sbtn" @click="addVariable(2)">{{ t('pex.kindExpression') }}</button><button class="sbtn refresh-current" type="button" :title="t('pex.refreshCurrent')" :aria-label="t('pex.refreshCurrent')" @click="refreshCurrentValues"><svg class="ico" viewBox="0 0 24 24"><path d="M20 11a8 8 0 0 0-13.6-5.7L3 8" /><path d="M3 3v5h5" /><path d="M4 13a8 8 0 0 0 13.6 5.7L21 16" /><path d="M21 21v-5h-5" /></svg></button></span></h3>
        <div class="tbl vars-table"><div class="tbody"><div class="head vh"><span>{{ t('col.id') }}</span><span>{{ t('pex.colVarName') }}</span><span>{{ t('pex.colVarKind') }}</span><span>{{ t('pex.colValue') }}</span><span>{{ t('pex.colCurrent') }}</span><span>TTL</span><span>{{ t('col.ops') }}</span></div>
          <div v-if="!f.Variables.length" class="empty">{{ t('pex.noVars') }}</div>
          <div v-for="(v, i) in f.Variables" v-else :key="v.Id || i" class="tr vh" @dblclick="editVariable(i)"><span>{{ i + 1 }}</span><span :title="v.Name">{{ v.Name }}</span><span>{{ kindText(v.Kind) }}</span><span :title="summary(v)">{{ summary(v) }}</span><span class="current" :title="currentValue(v)">{{ currentValue(v) }}</span><span>{{ v.TtlSeconds || '—' }}</span><span class="ops"><button class="op" :title="t('acct.op.edit')" @click.stop="editVariable(i)"><svg class="ico" viewBox="0 0 24 24"><path d="M4 20h4L20 8l-4-4L4 16z" /></svg></button><button class="op del" :title="t('acct.op.del')" @click.stop="removeVariable(i)"><svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg></button></span></div>
        </div></div>
      </section>
      </div>
    </template>
  </SettingsModal>
  <SettingsModal :open="!!variableDraft" :title="t('pex.varEditTitle')" subtitle="Controls/PacketVariableEdit" :width="620"
                 :hint="t('pex.varHint')"
                 @update:open="!$event && (variableDraft = null)" @save="saveVariable">
    <div v-if="variableDraft" class="setf ve">
      <div class="grp">{{ kindText(variableDraft.Kind) }}</div>
      <div class="row"><div class="k">{{ t('pex.colVarName') }}</div><div class="v"><input class="inp" v-model="variableDraft.Name" maxlength="64" /></div></div>
      <div class="row"><div class="k">{{ t('pex.dataType') }}</div><div class="v"><CyberSelect class="full" v-model="variableDraft.DataType" :options="typeOptions" /></div></div>
      <template v-if="variableDraft.Kind === 0">
        <div class="row"><div class="k">{{ t('pex.value') }}</div><div class="v"><input class="inp" v-model="variableDraft.Value" :placeholder="valuePlaceholder(variableDraft.DataType)" /></div></div>
      </template>
      <template v-else-if="variableDraft.Kind === 1">
        <div class="row"><div class="k">{{ t('pex.baseline') }}</div><div class="v"><button class="chk" :class="{ on: variableDraft.Extraction.RelativeToMatch }" @click="variableDraft.Extraction.RelativeToMatch = !variableDraft.Extraction.RelativeToMatch"><i />{{ t('pex.relativeMatch') }}</button></div></div>
        <div class="row"><div class="k">{{ t('pex.startOffset') }}</div><div class="v"><input class="inp num" v-model.number="variableDraft.Extraction.Offset" inputmode="numeric" /></div></div>
        <div class="row"><div class="k">{{ t('pex.captureLength') }}</div><div class="v"><input class="inp num" v-model.number="variableDraft.Extraction.Length" inputmode="numeric" /></div></div>
      </template>
      <template v-else>
        <p v-if="variableDraft.DataType === 0" class="expr-rule">{{ t('pex.ruleInt1') }} <code>+ - * / %</code>{{ t('pex.ruleOpsAndRefs') }} <code>{{ t('pex.refFormat') }}</code>{{ t('pex.ruleEnd') }}</p>
        <p v-else-if="variableDraft.DataType === 1" class="expr-rule">{{ t('pex.ruleFloat1') }} <code>+ - * / %</code>{{ t('pex.ruleFloat2') }} <code>{{ t('pex.exFormat') }}</code>{{ t('pex.ruleEnd') }}</p>
        <p v-else-if="variableDraft.DataType === 2" class="expr-rule">{{ t('pex.ruleBytes1') }} <code>+</code> {{ t('pex.ruleBytes2') }}</p>
        <p v-else class="expr-rule">{{ t('pex.ruleText1') }} <code>+</code> {{ t('pex.ruleText2') }}</p>
        <div class="row expr-row">
          <div class="k">{{ t('pex.expression') }}</div>
          <div class="v">
            <div class="expr-input"><input ref="expressionInput" class="inp" v-model="variableDraft.Value" :placeholder="expressionPlaceholder(variableDraft.DataType)" /><PacketVariableReferencePicker :mode="variableDraft.DataType === 2 ? 'bytes' : 'expression'" :data-type="variableDraft.DataType" :extra-extractor="f" @insert="insertExpression" /></div>
            <p v-if="variableDraft.DataType === 0" class="hint expr-hint"><span class="expr-example">{{ t('pex.example') }}<code>{{ t('pex.exInt') }}</code></span></p>
            <p v-else-if="variableDraft.DataType === 1" class="hint expr-hint"><span class="expr-example">{{ t('pex.example') }}<code>{{ t('pex.exFloat') }}</code></span></p>
            <p v-else-if="variableDraft.DataType === 2" class="hint expr-hint"><span class="expr-example">{{ t('pex.example') }}<code>{{ t('pex.exBytes') }}</code></span></p>
            <p v-else class="hint expr-hint"><span class="expr-example">{{ t('pex.example') }}<code>{{ t('pex.exText') }}</code></span></p>
          </div>
        </div>
      </template>
      <div class="row"><div class="k">{{ t('pex.ttl') }}</div><div class="v"><input class="inp num" v-model.number="variableDraft.TtlSeconds" inputmode="numeric" /><p class="hint">{{ t('pex.ttlHint') }}</p></div></div>
    </div>
  </SettingsModal>
</template>

<style scoped>
.sec{margin:0 0 14px;border:1px solid var(--border);border-left-color:var(--cyan);background:rgb(var(--inset-rgb) / 18%)}
.sec h3{display:flex;align-items:center;gap:12px;margin:0;padding:10px 18px;background:var(--bar);font-size:var(--fs-body);color:var(--gray)}
.sec h3 b{padding:2px 5px;border:1px solid var(--cyan);color:var(--cyan);font:var(--fs-small) var(--mono)}
.actions{display:flex;gap:8px;margin-left:auto}.actions .sbtn{height:28px;padding:0 10px}.actions .refresh-current{width:28px;padding:0;display:grid;place-items:center}.actions .refresh-current .ico{width:15px;height:15px}
.form{display:grid;grid-template-columns:140px minmax(0,1fr);gap:10px 14px;align-items:center;padding:12px 18px}.form>label{color:var(--dim);text-align:left}.form .inp{width:100%;box-sizing:border-box}.form small{grid-column:2;color:var(--muted);margin-top:-6px}.cs{width:100%}
/* 用户（2026-09-29）：02 变量表要撑满整张卡 —— 卡底那 8px 内边距（.setf .sec 的默认）归零；表的底边框也让掉，否则与卡自己的下边框叠成 2px 双线。表本来就通栏（左右无边框 / 无外边距），上下贴齐后读成卡里的一整条带。 */
.ex .vars{display:flex;min-height:0;padding-bottom:0;flex-direction:column}.ex .vars .vars-table{display:flex;flex:1;min-height:0;margin:0!important;border-bottom:0;flex-direction:column}.ex .vars .vars-table .tbody{display:flex;min-height:0;max-height:none;flex:1;flex-direction:column;overflow-y:auto}.ex .vars .vars-table .tbody .tr,.ex .vars .vars-table .tbody .head{flex:none}.vh{grid-template-columns:50px minmax(100px,1fr) 100px minmax(130px,1.3fr) 80px 60px 70px}.vh>span{overflow:hidden;text-overflow:ellipsis;white-space:nowrap}.vh>span:nth-child(1),.vh>span:nth-child(3),.vh>span:nth-child(6),.vh>span:nth-child(7){text-align:center}.vh .current{font-family:var(--mono);color:var(--text)}.ve .full{display:flex;width:100%}.ve .hint{margin:5px 0 0;color:var(--dim2);font-size:var(--fs-small)}.ve .expr-rule{margin:5px 20px;padding-left:calc(var(--setf-k, 112px) * var(--setf-kx, 1) + 12px);color:var(--dim2);font-size:var(--fs-small);line-height:1.65}.ve .expr-rule code,.ve .expr-hint code{color:var(--cyan);font-family:var(--mono)}.ve .expr-row{align-items:start}.ve .expr-row>.k{height:36px;padding:0;display:flex;align-items:center;transform:translateY(-2px)}.ve .expr-row>.v{gap:0}.ve .expr-input{display:flex;gap:8px;width:100%}.ve .expr-input .inp{min-width:0;flex:1}.ve .expr-hint{flex:0 0 100%;padding:0;margin:10px 0 0;line-height:1.65}.ve .expr-example{display:block}
</style>
