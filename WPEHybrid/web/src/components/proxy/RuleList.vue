<script setup lang="ts">
/*
  某台 ProxyCap 服务器的规则集 —— 对应 WinForms 的 Controls/RuleList + RuleEdit（两个弹窗合成一个）。

  规则是 ServerInfo 下面的嵌套列表，不在推送流里，打开时按服务器 Id 取，每次改动后重取。
  下面那块编辑区既是「新增」也是「编辑」：点表里一行就把它装进去改，空着就是新增；
  新增时参数按分号拆开可以一次加多条（照 WinForms 的 RuleEdit.bSave）。
  规则类型的显示名（DOMAIN-SUFFIX 这种带横线的）由 C# 给，那是 Description 特性，前端不另抄一份。
*/
import { computed, ref, watch } from 'vue'
import { call } from '../../bridge'
import { ListAction, type ServerRow } from '../../bridge/types'
import { t } from '../../i18n'
import ContextMenu from '../ContextMenu.vue'
import CyberSelect from '../CyberSelect.vue'
import { ICON, type MenuItem } from '../menu'
import SettingsModal from './SettingsModal.vue'

interface RuleRow { Id: string; IsEnable: boolean; Type: number; TypeName: string; Argument: string; Action: number }
interface RuleType { Value: number; Name: string }

const props = defineProps<{ server: ServerRow | null }>()
const emit = defineEmits<{ (e: 'close'): void }>()

const rows = ref<RuleRow[]>([])
const types = ref<RuleType[]>([])
const busy = ref(false)
const error = ref('')

//编辑区：editing = null 是新增
const editing = ref<RuleRow | null>(null)
const f = ref({ enable: true, type: 0, argument: '', action: 2 })

const ACTIONS = [
  { value: 0, label: 'PROXY' },
  { value: 1, label: 'REJECT' },
  { value: 2, label: 'DIRECT' },
]

const typeOptions = computed(() => types.value.map((x) => ({ value: x.Value, label: x.Name })))
const title = computed(() => t('wpc.rulesOf') + ' · ' + (props.server?.Name ?? ''))

watch(() => props.server, async (s) => {
  if (!s) return
  error.value = ''
  editing.value = null
  f.value = { enable: true, type: 0, argument: '', action: 2 }
  try {
    if (!types.value.length) {
      const r = await call<{ rows: RuleType[] }>('getRuleTypes')
      types.value = r?.rows ?? []
    }
    await reload()
  } catch (e) {
    console.error('[wpc] 取规则失败', e)
  }
})

async function reload(): Promise<void> {
  if (!props.server) return
  const r = await call<{ rows: RuleRow[] }>('getServerRules', { sid: props.server.Id })
  rows.value = r?.rows ?? []
}

function load(r: RuleRow): void {
  editing.value = r
  f.value = { enable: r.IsEnable, type: r.Type, argument: r.Argument, action: r.Action }
}

function reset(): void {
  editing.value = null
  f.value = { enable: true, type: f.value.type, argument: '', action: f.value.action }
}

async function saveRule(): Promise<void> {
  if (!props.server) return
  busy.value = true
  error.value = ''
  try {
    const r = await call<{ error: string }>('saveServerRule', {
      sid: props.server.Id,
      id: editing.value?.Id ?? '',
      enable: f.value.enable,
      type: f.value.type,
      argument: f.value.argument,
      ruleAction: f.value.action,
    })
    if (r?.error) { error.value = r.error; return }
    reset()
    await reload()
  } catch (e) {
    console.error('[wpc] 保存规则失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}

async function toggle(r: RuleRow): Promise<void> {
  if (!props.server) return
  try {
    await call('setServerRuleEnable', { sid: props.server.Id, id: r.Id, enable: !r.IsEnable })
    await reload()
  } catch (e) {
    console.error('[wpc] 切换规则失败', e)
  }
}

async function action(id: string, a: ListAction): Promise<void> {
  if (!props.server) return
  try {
    await call('serverRuleAction', { sid: props.server.Id, action: a, ids: [id] })
    if (editing.value?.Id === id && a === ListAction.Delete) reset()
    await reload()
  } catch (e) {
    console.error('[wpc] 规则操作失败', e)
  }
}

async function clearAll(): Promise<void> {
  if (!props.server) return
  try {
    await call('clearServerRules', { sid: props.server.Id })
    reset()
    await reload()
  } catch (e) {
    console.error('[wpc] 清空规则失败', e)
  }
}

/* 右键：四个移动 + 删除，作用于开菜单的那一行（与自动入库的规则表同一口径，这张表没有多选） */
const menuAt = ref<{ x: number; y: number } | null>(null)
const menuRow = ref<RuleRow | null>(null)

const menuItems = computed<MenuItem[]>(() => {
  const i = rows.value.findIndex((x) => x.Id === menuRow.value?.Id)
  const first = i <= 0, last = i < 0 || i === rows.value.length - 1
  return [
    { id: 'top', label: t('lst.top'), icon: ICON.top, disabled: first },
    { id: 'up', label: t('lst.up'), icon: ICON.up, disabled: first },
    { id: 'down', label: t('lst.down'), icon: ICON.down, disabled: last },
    { id: 'bottom', label: t('lst.bottom'), icon: ICON.bottom, disabled: last },
    { divider: true },
    { id: 'delete', label: t('lst.delete'), icon: ICON.del, danger: true },
  ]
})

const ACTION_OF: Record<string, ListAction> = {
  top: ListAction.Top, up: ListAction.Up, down: ListAction.Down, bottom: ListAction.Bottom, delete: ListAction.Delete,
}

function onMenuPick(id: string): void {
  const r = menuRow.value
  const a = ACTION_OF[id]
  if (!r || a === undefined) return
  void action(r.Id, a)
}

function openMenu(e: MouseEvent, r: RuleRow): void {
  menuRow.value = r
  menuAt.value = { x: e.clientX, y: e.clientY }
}

function close(): void {
  if (!busy.value) emit('close')
}

const ACTION_LABEL = ['PROXY', 'REJECT', 'DIRECT']
</script>

<template>
  <SettingsModal :open="!!props.server" :title="title" subtitle="Node Rules" :busy="busy" :error="error" readonly :width="860"
                 @update:open="!$event && close()">
    <div class="setf list-page rl">
      <!-- 编辑区 -->
      <div class="grp">{{ editing ? t('wpc.ruleEdit') : t('wpc.ruleAdd') }}</div>
      <div class="row">
        <div class="k">{{ t('wpc.ruleType') }}</div>
        <div class="v">
          <CyberSelect v-model="f.type" :options="typeOptions" class="dd" />
          <span class="lb">{{ t('wpc.ruleAction') }}</span>
          <CyberSelect v-model="f.action" :options="ACTIONS" class="dd sm" />
          <button class="chk" :class="{ on: f.enable }" @click="f.enable = !f.enable"><i />{{ t('col.enable') }}</button>
        </div>
      </div>
      <div class="row">
        <div class="k">{{ t('wpc.ruleArg') }}</div>
        <div class="v">
          <input v-model="f.argument" class="inp" spellcheck="false" :placeholder="editing ? '' : t('wpc.ruleArgPh')" @keydown.enter="saveRule">
          <button class="sbtn primary" :disabled="busy" @click="saveRule">{{ editing ? t('set.save') : t('wpc.ruleInsert') }}</button>
          <button v-if="editing" class="sbtn" @click="reset">{{ t('wpc.ruleNew') }}</button>
        </div>
      </div>
      <p class="hint">{{ t('wpc.ruleHint') }}</p>

      <!-- 规则表 -->
      <div class="tbl">
        <div class="tbar">
          <span class="cnt">{{ rows.length }} {{ t('wpc.ruleCount') }}</span>
          <span class="grow" />
          <button class="sbtn danger" :disabled="!rows.length" @click="clearAll">{{ t('wpc.clearRules') }}</button>
        </div>
        <div class="tbody">
          <div class="head">
            <span class="no">{{ t('col.id') }}</span>
            <span class="ck">{{ t('col.enable') }}</span>
            <span class="ty">{{ t('col.type') }}</span>
            <span class="arg">{{ t('wpc.ruleArg') }}</span>
            <span class="act">{{ t('wpc.ruleAction') }}</span>
            <span class="ops">{{ t('col.ops') }}</span>
          </div>
          <div v-if="!rows.length" class="empty">{{ t('wpc.emptyRules') }}</div>
          <div
            v-for="(r, i) in rows"
            v-else
            :key="r.Id"
            class="tr"
            :class="{ off: !r.IsEnable, sel: editing?.Id === r.Id }"
            @click="load(r)"
            @contextmenu.prevent="openMenu($event, r)"
          >
            <span class="no">{{ i + 1 }}</span>
            <span class="ck"><button class="chk" :class="{ on: r.IsEnable }" @click.stop="toggle(r)"><i /></button></span>
            <span class="ty">{{ r.TypeName }}</span>
            <span class="arg" :title="r.Argument">{{ r.Argument || '—' }}</span>
            <span class="act" :class="'a' + r.Action">{{ ACTION_LABEL[r.Action] ?? r.Action }}</span>
            <span class="ops">
              <button class="op del" :title="t('acct.op.del')" @click.stop="action(r.Id, ListAction.Delete)">
                <svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg>
              </button>
            </span>
          </div>
        </div>
      </div>
    </div>

    <ContextMenu :at="menuAt" :items="menuItems" @pick="onMenuPick" @close="menuAt = null" />
  </SettingsModal>
</template>

<style scoped>
/*
  ⚠️ 两个下拉的宽度类叫 .dd 不叫 .sel —— 这张表的行选中态<b>也</b>是 .sel
  （`style.css` 的 `.setf .tbl .tr.sel`）。写成裸 .sel 会同特异度命中那一行，
  把整行压成 190px，参数列的 minmax(160px, 1fr) 塌回下限、后面几列一起左移。
*/
.dd { width: 190px; }
.dd.sm { width: 120px; }
.lb { font-size: var(--fs-body); color: var(--muted); }
.cnt { font-family: var(--share); font-size: var(--fs-label); letter-spacing: .1em; text-transform: uppercase; color: var(--muted); }

.rl .head, .rl .tr { grid-template-columns: 46px 50px 150px minmax(160px, 1fr) 80px 44px; }
.rl .head > span, .rl .tr > span { text-align: center; }
.rl .head > span.arg, .rl .tr > span.arg, .rl .head > span.ty, .rl .tr > span.ty { text-align: left; }
.rl .tbody { max-height: 320px; }

.no { color: var(--dim); font-variant-numeric: tabular-nums; }
.ty { font-family: var(--mono); font-size: var(--fs-body); color: var(--cyan); }
.arg { font-family: var(--mono); font-size: var(--fs-body); color: var(--acc-green2); }
.act { font-family: var(--share); font-size: var(--fs-label); letter-spacing: .08em; }
.act.a0 { color: var(--cyan); }
.act.a1 { color: var(--danger); }
.act.a2 { color: var(--green); }
</style>
