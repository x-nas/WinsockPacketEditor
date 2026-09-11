<script setup lang="ts">
/*
  批量创建账号 —— 对应 WinForms 的 Controls/BatchAccounts。

  三步：设参数 → 预览 → 保存。预览这一步不是装饰：账号一旦落库就要挨个删，
  而随机密码与自动编号出来的东西，人得先看一眼才敢按保存。

  【生成规则在 C# 侧】用户名编号与随机密码走
  Operate.ProxyConfig.Account.BuildBatchAccounts，两套 UI 才是同一套规则。
  前端只持有生成出来的草稿数组，删行就是删元素 —— C# 侧不留状态，
  两边就不会出现「你删了我这儿还在」的错位。

  【密码是明文】草稿要给使用者看、导出的表格也要发出去，加密串对谁都没用。
  落库那一步才加密。
*/
import { computed, ref, watch } from 'vue'
import { call } from '../../bridge'
import { t } from '../../i18n'
import { pushToast } from '../../stores/toast'
import SettingsModal from './SettingsModal.vue'

const props = defineProps<{ open: boolean }>()
const emit = defineEmits<{ (e: 'update:open', v: boolean): void }>()

const busy = ref(false)
const error = ref('')

interface Draft { UserName: string; Password: string }

/** 参数区。上下限与 WinForms 那两个数字框一致（数量 1–999、密码 1–20）。 */
const f = ref({
  count: 10,
  /** 0 = 当前时间 + 序号，1 = 自定义前缀 + 序号 */
  rule: 0,
  prefix: '',
  passwordLength: 6,
  isLimitLinks: false,
  limitLinks: 1,
  isLimitDevices: false,
  limitDevices: 1,
  isExpiry: false,
  expiryTime: defaultExpiry(),
})

const rows = ref<Draft[]>([])
/** 与已有账号重名的用户名，预览时就标出来 —— 保存时才说就晚了。 */
const dup = ref<Set<string>>(new Set())

function defaultExpiry(): string {
  const d = new Date()
  d.setMonth(d.getMonth() + 1)
  const p = (n: number) => String(n).padStart(2, '0')
  return d.getFullYear() + '-' + p(d.getMonth() + 1) + '-' + p(d.getDate())
    + 'T' + p(d.getHours()) + ':' + p(d.getMinutes())
}

const dupCount = computed(() => rows.value.reduce((n, r) => n + (dup.value.has(r.UserName) ? 1 : 0), 0))

//每次打开都从零开始：上一批已经保存或放弃了，留着只会误按
watch(() => props.open, (on) => {
  if (!on) return
  error.value = ''
  rows.value = []
  dup.value = new Set()
})

/** 落库与导出共用的那半边参数。 */
function shared() {
  return {
    isLimitLinks: f.value.isLimitLinks,
    limitLinks: Number(f.value.limitLinks) || 0,
    isLimitDevices: f.value.isLimitDevices,
    limitDevices: Number(f.value.limitDevices) || 0,
    isExpiry: f.value.isExpiry,
    expiryTime: f.value.expiryTime ? f.value.expiryTime.replace('T', ' ') + ':00' : '',
  }
}

async function preview(): Promise<void> {
  busy.value = true
  error.value = ''

  try {
    const r = await call<{ ok: boolean; error: string; rows: Draft[]; duplicates: string[] }>(
      'previewBatchAccounts',
      {
        count: Number(f.value.count) || 1,
        rule: f.value.rule,
        prefix: f.value.prefix,
        passwordLength: Number(f.value.passwordLength) || 6,
      },
    )

    if (!r?.ok) {
      error.value = r?.error || ''
      return
    }

    rows.value = r.rows || []
    dup.value = new Set(r.duplicates || [])
  } catch (e) {
    console.error('[acct] 生成批量账号失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}

function drop(i: number): void {
  rows.value.splice(i, 1)
}

async function save(): Promise<void> {
  if (!rows.value.length) {
    error.value = t('acct.b.needPreview')
    return
  }

  busy.value = true
  error.value = ''

  try {
    const r = await call<{ ok: boolean; added: number; skipped: number; error: string }>(
      'saveBatchAccounts', { rows: rows.value, ...shared() },
    )

    if (!r?.ok) {
      error.value = r?.error || ''
      return
    }

    //跳过的都是重名 —— 说清楚，别让人以为全建上了
    pushToast(
      r.skipped ? 'warning' : 'success',
      r.skipped
        ? t('acct.b.savedSome').replace('{0}', String(r.added)).replace('{1}', String(r.skipped))
        : t('acct.b.saved').replace('{0}', String(r.added)),
    )

    emit('update:open', false)
  } catch (e) {
    console.error('[acct] 批量保存失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}

async function exportRows(): Promise<void> {
  try {
    await call('exportBatchAccounts', { rows: rows.value, ...shared() })
  } catch (e) {
    console.error('[acct] 导出批量账号失败', e)
  }
}
</script>

<template>
  <SettingsModal
    :open="props.open"
    :title="t('acct.batch')"
    subtitle="Batch Create"
    :busy="busy"
    :error="error"
    @update:open="emit('update:open', $event)"
    @save="save"
  >
    <div class="grp">{{ t('acct.b.rule') }}</div>

    <div class="row">
      <div class="k">{{ t('acct.b.naming') }}</div>
      <div class="v">
        <button class="rd" :class="{ on: f.rule === 0 }" @click="f.rule = 0"><i />{{ t('acct.b.byTime') }}</button>
        <button class="rd" :class="{ on: f.rule === 1 }" @click="f.rule = 1"><i />{{ t('acct.b.byPrefix') }}</button>
        <input v-model="f.prefix" class="inp pf" spellcheck="false" :disabled="f.rule !== 1"
               :placeholder="t('acct.b.prefixPh')">
      </div>
    </div>

    <div class="row">
      <div class="k">{{ t('acct.b.count') }}</div>
      <div class="v">
        <input v-model.number="f.count" class="inp num" type="number" min="1" max="999">
        <span class="k2">{{ t('acct.b.passLen') }}</span>
        <input v-model.number="f.passwordLength" class="inp num" type="number" min="1" max="20">
      </div>
    </div>

    <div class="grp">{{ t('acct.e.limits') }}</div>

    <div class="row">
      <button class="chk k" :class="{ on: f.isLimitLinks }" @click="f.isLimitLinks = !f.isLimitLinks">
        <i />{{ t('col.links') }}
      </button>
      <div class="v">
        <input v-model.number="f.limitLinks" class="inp num" type="number" min="1" max="10000"
               :disabled="!f.isLimitLinks">
        <button class="chk" :class="{ on: f.isLimitDevices }" @click="f.isLimitDevices = !f.isLimitDevices">
          <i />{{ t('col.devices') }}
        </button>
        <input v-model.number="f.limitDevices" class="inp num" type="number" min="1" max="10000"
               :disabled="!f.isLimitDevices">
      </div>
    </div>

    <div class="row">
      <button class="chk k" :class="{ on: f.isExpiry }" @click="f.isExpiry = !f.isExpiry">
        <i />{{ t('acct.e.expiry') }}
      </button>
      <div class="v">
        <input v-model="f.expiryTime" class="inp dt" type="datetime-local" :disabled="!f.isExpiry">
        <span class="tip">{{ f.isExpiry ? '' : t('acct.never') }}</span>
      </div>
    </div>

    <div class="grp bar">
      <span>{{ t('acct.b.preview') }}</span>
      <span v-if="rows.length" class="n">{{ rows.length }}</span>
      <span class="grow" />
      <button class="mini" :disabled="busy" @click="preview">{{ t('acct.b.gen') }}</button>
      <button class="mini" :disabled="!rows.length" @click="exportRows">{{ t('acct.export') }}</button>
    </div>

    <p v-if="dupCount" class="warn">{{ t('acct.b.dup').replace('{0}', String(dupCount)) }}</p>

    <div class="tbl">
      <div class="head">
        <span>{{ t('col.id') }}</span><span>{{ t('col.user') }}</span>
        <span>{{ t('acct.e.pass') }}</span><span />
      </div>

      <div v-if="!rows.length" class="empty">{{ t('acct.b.needPreview') }}</div>

      <!-- key 用用户名：删中间一行时按下标 key 会让后面每行都重画一遍 -->
      <div v-for="(r, i) in rows" :key="r.UserName" class="drow" :class="{ dup: dup.has(r.UserName) }">
        <span class="no">{{ i + 1 }}</span>
        <span class="user">{{ r.UserName }}</span>
        <span class="pw">{{ r.Password }}</span>
        <button class="op del" :title="t('acct.op.del')" @click="drop(i)">
          <svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg>
        </button>
      </div>
    </div>
  </SettingsModal>
</template>

<style scoped>
.grp {
  font-family: var(--share);
  font-size: var(--fs-caption);
  letter-spacing: .26em;
  text-transform: uppercase;
  color: var(--dim);
  padding: 0 20px;
  margin: 16px 0 6px;
}

.grp:first-child { margin-top: 14px; }

/* 预览这一段的组标题右边挂两个动作按钮 */
.grp.bar { display: flex; align-items: center; gap: 10px; }
.grp.bar .grow { flex: 1; }
.grp.bar .n { color: var(--cyan); letter-spacing: .1em; }

/* 小按钮的样式在 style.css 的 .mini */

.warn {
  margin: 0 20px 6px;
  padding: 6px 11px;
  border: 1px solid rgb(var(--amber-rgb) / 32%);
  background: rgb(var(--amber-rgb) / 7%);
  font-size: var(--fs-small);
  color: var(--amber);
}

.row {
  display: grid;
  grid-template-columns: 132px 1fr;
  align-items: center;
  gap: 12px;
  padding: 5px 20px;
  min-height: 32px;
}

.row > .k { font-size: var(--fs-body); color: var(--muted); }
.row > .v { display: flex; align-items: center; gap: 10px; min-width: 0; }

.k2 { font-size: var(--fs-body); color: var(--muted); margin-left: 8px; white-space: nowrap; }
.tip { font-size: var(--fs-small); color: var(--dim); }

/* 基样式在 style.css 的「勾选框 / 单选框」，这里只覆盖框线色与布局 */
.chk, .rd { --chk-ring: var(--border); }
.chk.k { justify-self: start; }

/* 基样式在 style.css 的 .inp，这里只补布局 */
.inp { min-width: 0; }

.inp.num { width: 84px; font-variant-numeric: tabular-nums; }
.inp.dt { width: 200px; }
.inp.pf { flex: 1; }
.inp.dt::-webkit-calendar-picker-indicator { filter: invert(0.7); cursor: pointer; }

.tbl {
  margin: 0 20px 4px;
  border: 1px solid var(--border);
  background: rgb(var(--inset-rgb) / 30%);
  /* 999 条也不该把弹窗撑到屏幕外，这块自己滚 */
  max-height: 220px;
  overflow: auto;
}

.head,
.drow {
  display: grid;
  grid-template-columns: 46px minmax(100px, 1fr) minmax(100px, 1fr) 30px;
  align-items: center;
  gap: 10px;
  padding: 0 10px;
  font-size: var(--fs-body);
}

.head {
  position: sticky;
  top: 0;
  z-index: 1;
  height: var(--th-h);
  background: var(--panel);
  border-bottom: 1px solid var(--border);
  /* 与全项目其它表头同一份：10px · var(--th-fg)（原先 9px + --muted，比别的表小一号也暗一档）*/
  font-family: var(--share);
  font-size: var(--th-size);
  letter-spacing: .14em;
  text-transform: uppercase;
  color: var(--th-fg);
}

.drow { height: 26px; color: var(--soft); }
.drow:hover { background: rgb(var(--tint-rgb) / 4%); }

/* 重名的这一条落库时会被挡下，先标出来 */
.drow.dup .user { color: var(--amber); text-decoration: line-through; }

.empty { padding: 24px 0; text-align: center; color: var(--dim); font-size: var(--fs-body); }

.no { color: var(--dim); font-variant-numeric: tabular-nums; text-align: center; }
.user { color: var(--gray); overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
.pw { color: var(--green); overflow: hidden; text-overflow: ellipsis; white-space: nowrap; user-select: text; }

.op {
  display: inline-flex;
  align-items: center;
  justify-content: center;
  width: 22px;
  height: 20px;
  padding: 0;
  background: transparent;
  border: 1px solid transparent;
  color: var(--dim);
  cursor: pointer;
}

.op:hover { border-color: var(--danger); color: var(--danger); }
.op:focus-visible { outline-offset: -2px; }
.op .ico { width: 12px; height: 12px; fill: none; stroke: currentColor; stroke-width: 1.8; }

/*
  表头每格与数据格同名（对齐规则靠这个），于是数据列的字体 / 字号 / 颜色
  （.notes 12px、.ad / .dt 等宽字、.cnt 青色……）会一并漏进表头，看着就是「备注」「数据」比别的表头大。
  这里按格子把它们收回来：表头只认表头自己那一份。(0,2,1) 压得过任何单类名的列规则。
*/
.head > span {
  /*
    表头字形偏上 2px（像素级实测：Share Tech Mono 10.5px 在 30px 表头里，中文 / 英文的墨迹中心都在盒中心上方约 2px）。
    格子是 grid / flex 项，内边距只在上面补 4px 就把内容框中心压下 2px，对文本与 flex 居中的格子都成立。
    改字号 / 字体 / 表头高度后要重新量（SVG foreignObject 逐行扫像素那套；量尺页 dev-headers.html 已于 2.1.9 随开发工具删除，要用从 git 历史取回）。
  */
  padding-top: 4px;
  font-family: inherit;
  font-size: inherit;
  font-weight: inherit;
  letter-spacing: inherit;
  text-transform: inherit;
  color: inherit;
}
</style>
