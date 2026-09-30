<script setup lang="ts">
import { nextTick, ref, watch } from 'vue'
import { call } from '../../bridge'
import CyberSelect from '../CyberSelect.vue'
import PacketVariableReferencePicker from './PacketVariableReferencePicker.vue'
import SettingsModal from '../proxy/SettingsModal.vue'
import type { Extractor, Variable } from '../../stores/extractor'

const props = defineProps<{ target: Extractor | null }>()
const emit = defineEmits<{ (e: 'close'): void; (e: 'saved'): void }>()

const f = ref<Extractor | null>(null)
const error = ref('')
const busy = ref(false)

const scopeOptions = [
  { value: 0, label: '全局（当前处理进程）' },
  { value: 1, label: '按 Socket 隔离' },
  { value: 2, label: '按 ProxySession 隔离' },
]
const typeOptions = [
  { value: 0, label: '整数' }, { value: 1, label: '浮点数' },
  { value: 2, label: '字节数组' }, { value: 3, label: '字符串' },
]

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
    Id: '00000000-0000-0000-0000-000000000000', Name: '变量 ' + n, Kind: kind, DataType: kind === 1 ? 2 : 3,
    Value: '', Extraction: { Offset: 0, Length: 1, RelativeToMatch: false, BigEndian: false, Signed: false, Encoding: 0 }, TtlSeconds: 0,
  }
}
function addVariable(kind: number): void { variableEdit.value = -1; variableDraft.value = newVariable(kind) }
function editVariable(index: number): void { if (!f.value) return; variableEdit.value = index; variableDraft.value = clone(f.value.Variables[index]) }
function saveVariable(): void { if (!f.value || !variableDraft.value || variableEdit.value === null) return; if (variableEdit.value < 0) f.value.Variables.push(variableDraft.value); else f.value.Variables.splice(variableEdit.value, 1, variableDraft.value); variableEdit.value = null; variableDraft.value = null }
function removeVariable(index: number): void { f.value?.Variables.splice(index, 1) }
function kindText(kind: number): string { return kind === 0 ? '简单值' : kind === 1 ? '从封包取值' : '表达式' }
function summary(v: Variable): string { return v.Kind === 1 ? (v.Extraction.RelativeToMatch ? '相对匹配点 · ' : '从包头 · ') + '偏移 ' + v.Extraction.Offset + ' · 长度 ' + v.Extraction.Length : (v.Value || '—') }
/** 后端为尚未取到的值返回「—」；编辑表统一按本页约定显示半角 -。 */
function currentValue(v: Variable): string { return v.CurrentValue && v.CurrentValue !== '—' ? v.CurrentValue : '-' }
function valuePlaceholder(type: number): string
{
  switch (type) {
    case 0: return '如：1000 或 -42'
    case 1: return '如：3.14159 或 -0.25'
    case 2: return '如：AA BB CC DD'
    default: return '如：你好，Hello'
  }
}

function expressionPlaceholder(type: number): string
{
  switch (type) {
    case 0: return '(${登录.积分} * 2 + 100) / 5'
    case 1: return '${状态.倍率} * 1.5 + 0.25'
    case 2: return "${封包.校验字节} + '0F 3E 22'"
    default: return "${登录.昵称} + '，欢迎登录'"
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
    if (!result.ok) { error.value = result.error || '保存失败'; return }
    emit('saved'); emit('close')
  } catch (e) { error.value = String(e) } finally { busy.value = false }
}
</script>

<template>
  <SettingsModal :open="!!f" title="编辑取值器" subtitle="PACKET EXTRACTOR" :width="850" :busy="busy" :error="error" @update:open="!$event && emit('close')" @save="save">
    <template v-if="f">
      <div class="setf ex list-page">
      <section class="sec">
        <h3><b>01</b> 基础信息</h3>
        <div class="form">
          <label>名称</label><input class="inp" v-model="f.Name" maxlength="64" />
          <label>作用域</label><CyberSelect v-model="f.Scope" :options="scopeOptions" />
          <label>备注</label><input class="inp" v-model="f.Description" maxlength="256" />
        </div>
      </section>

      <section class="sec vars">
        <h3><b>02</b> 变量 <span class="actions"><button class="sbtn" @click="refreshCurrentValues">刷新当前值</button><button class="sbtn" @click="addVariable(0)">简单值</button><button class="sbtn" @click="addVariable(1)">从封包取值</button><button class="sbtn" @click="addVariable(2)">表达式</button></span></h3>
        <div class="tbl vars-table"><div class="tbody"><div class="head vh"><span>序号</span><span>变量名称</span><span>类型</span><span>取值内容</span><span>当前值</span><span>TTL</span><span>操作</span></div>
          <div v-if="!f.Variables.length" class="empty">尚未添加变量。</div>
          <div v-for="(v, i) in f.Variables" v-else :key="v.Id || i" class="tr vh" @dblclick="editVariable(i)"><span>{{ i + 1 }}</span><span :title="v.Name">{{ v.Name }}</span><span>{{ kindText(v.Kind) }}</span><span :title="summary(v)">{{ summary(v) }}</span><span class="current" :title="currentValue(v)">{{ currentValue(v) }}</span><span>{{ v.TtlSeconds || '—' }}</span><span class="ops"><button class="op" title="编辑" @click.stop="editVariable(i)"><svg class="ico" viewBox="0 0 24 24"><path d="M4 20h4L20 8l-4-4L4 16z" /></svg></button><button class="op del" title="删除" @click.stop="removeVariable(i)"><svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg></button></span></div>
        </div></div>
      </section>
      </div>
    </template>
  </SettingsModal>
  <SettingsModal :open="!!variableDraft" title="编辑变量" subtitle="PACKET VARIABLE" :width="620"
                 hint="取值基准：未勾选从包头计算；勾选后从首次匹配位置计算。"
                 @update:open="!$event && (variableDraft = null)" @save="saveVariable">
    <div v-if="variableDraft" class="setf ve">
      <div class="grp">{{ kindText(variableDraft.Kind) }}</div>
      <div class="row"><div class="k">变量名称</div><div class="v"><input class="inp" v-model="variableDraft.Name" maxlength="64" /></div></div>
      <div class="row"><div class="k">数据类型</div><div class="v"><CyberSelect class="full" v-model="variableDraft.DataType" :options="typeOptions" /></div></div>
      <template v-if="variableDraft.Kind === 0">
        <div class="row"><div class="k">值</div><div class="v"><input class="inp" v-model="variableDraft.Value" :placeholder="valuePlaceholder(variableDraft.DataType)" /></div></div>
      </template>
      <template v-else-if="variableDraft.Kind === 1">
        <div class="row"><div class="k">取值基准</div><div class="v"><button class="chk" :class="{ on: variableDraft.Extraction.RelativeToMatch }" @click="variableDraft.Extraction.RelativeToMatch = !variableDraft.Extraction.RelativeToMatch"><i />相对滤镜匹配位置</button></div></div>
        <div class="row"><div class="k">起始偏移</div><div class="v"><input class="inp num" v-model.number="variableDraft.Extraction.Offset" inputmode="numeric" /></div></div>
        <div class="row"><div class="k">取值长度</div><div class="v"><input class="inp num" v-model.number="variableDraft.Extraction.Length" inputmode="numeric" /></div></div>
      </template>
      <template v-else>
        <p v-if="variableDraft.DataType === 0" class="expr-rule">整数：支持 <code>+ - * / %</code>、括号和引用；引用格式 <code>${取值器名称.变量名称}</code>。</p>
        <p v-else-if="variableDraft.DataType === 1" class="expr-rule">浮点数：支持 <code>+ - * / %</code>、括号和引用；引用的变量须为整数或浮点数。动态替换须指定字节格式，例如 <code>${计算.结果:f32le}</code>。</p>
        <p v-else-if="variableDraft.DataType === 2" class="expr-rule">字节数组：用 <code>+</code> 拼接变量与十六进制字节；数值变量须指定字节格式，插入器会自动填写。</p>
        <p v-else class="expr-rule">字符串：用 <code>+</code> 拼接任意变量与固定文本；字节数组会转为十六进制文本，固定文本可用单/双引号包住。</p>
        <div class="row expr-row">
          <div class="k">表达式</div>
          <div class="v">
            <div class="expr-input"><input ref="expressionInput" class="inp" v-model="variableDraft.Value" :placeholder="expressionPlaceholder(variableDraft.DataType)" /><PacketVariableReferencePicker :mode="variableDraft.DataType === 2 ? 'bytes' : 'expression'" :data-type="variableDraft.DataType" :extra-extractor="f" @insert="insertExpression" /></div>
            <p v-if="variableDraft.DataType === 0" class="hint expr-hint"><span class="expr-example">示例：<code>(${登录.积分} * 2 + 100) / 5</code></span></p>
            <p v-else-if="variableDraft.DataType === 1" class="hint expr-hint"><span class="expr-example">示例：<code>${状态.倍率} * 1.5 + 0.25</code></span></p>
            <p v-else-if="variableDraft.DataType === 2" class="hint expr-hint"><span class="expr-example">示例：<code>${封包.校验字节} + '0F 3E 22'</code></span></p>
            <p v-else class="hint expr-hint"><span class="expr-example">示例：<code>${登录.昵称} + '，欢迎登录'</code></span></p>
          </div>
        </div>
      </template>
      <div class="row"><div class="k">TTL（秒）</div><div class="v"><input class="inp num" v-model.number="variableDraft.TtlSeconds" inputmode="numeric" /><p class="hint">0 表示不自动过期</p></div></div>
    </div>
  </SettingsModal>
</template>

<style scoped>
.sec{margin:0 0 14px;border:1px solid var(--border);border-left-color:var(--cyan);background:rgb(var(--inset-rgb) / 18%)}
.sec h3{display:flex;align-items:center;gap:12px;margin:0;padding:10px 18px;background:var(--bar);font-size:var(--fs-body);color:var(--gray)}
.sec h3 b{padding:2px 5px;border:1px solid var(--cyan);color:var(--cyan);font:var(--fs-small) var(--mono)}
.actions{display:flex;gap:8px;margin-left:auto}.actions .sbtn{height:28px;padding:0 10px}
.form{display:grid;grid-template-columns:140px minmax(0,1fr);gap:10px 14px;align-items:center;padding:12px 18px}.form>label{color:var(--dim);text-align:left}.form .inp{width:100%;box-sizing:border-box}.form small{grid-column:2;color:var(--muted);margin-top:-6px}.cs{width:100%}
/* 用户（2026-09-29）：02 变量表要撑满整张卡 —— 卡底那 8px 内边距（.setf .sec 的默认）归零；表的底边框也让掉，否则与卡自己的下边框叠成 2px 双线。表本来就通栏（左右无边框 / 无外边距），上下贴齐后读成卡里的一整条带。 */
.ex .vars{display:flex;min-height:0;padding-bottom:0;flex-direction:column}.ex .vars .vars-table{display:flex;flex:1;min-height:0;margin:0!important;border-bottom:0;flex-direction:column}.ex .vars .vars-table .tbody{display:flex;min-height:0;max-height:none;flex:1;flex-direction:column;overflow-y:auto}.ex .vars .vars-table .tbody .tr,.ex .vars .vars-table .tbody .head{flex:none}.vh{grid-template-columns:50px minmax(100px,1fr) 100px minmax(130px,1.3fr) 80px 60px 70px}.vh>span{overflow:hidden;text-overflow:ellipsis;white-space:nowrap}.vh>span:nth-child(1),.vh>span:nth-child(3),.vh>span:nth-child(6),.vh>span:nth-child(7){text-align:center}.vh .current{font-family:var(--mono);color:var(--text)}.ve .full{display:flex;width:100%}.ve .hint{margin:5px 0 0;color:var(--dim2);font-size:var(--fs-small)}.ve .expr-rule{margin:5px 20px;padding-left:calc(var(--setf-k, 112px) * var(--setf-kx, 1) + 12px);color:var(--dim2);font-size:var(--fs-small);line-height:1.65}.ve .expr-rule code,.ve .expr-hint code{color:var(--cyan);font-family:var(--mono)}.ve .expr-row{align-items:start}.ve .expr-row>.k{height:36px;padding:0;display:flex;align-items:center;transform:translateY(-2px)}.ve .expr-row>.v{gap:0}.ve .expr-input{display:flex;gap:8px;width:100%}.ve .expr-input .inp{min-width:0;flex:1}.ve .expr-hint{flex:0 0 100%;padding:0;margin:10px 0 0;line-height:1.65}.ve .expr-example{display:block}
</style>
