<script setup lang="ts">
/*
  新增 / 编辑代理账号 —— 对应 WinForms 的 Controls/AccountEdit。

  【id 的三态】null = 关着；'' = 新增；其余 = 在改那一条。
  用一个 prop 表达是因为这两件事只差三样：标题、用户名可不可改、密码能不能留空。

  【密码是单独取的】AccountRow 里没有密码字段（那是加密串，不进推送流），
  打开时按 Id 走一次 getAccountPassword 拿明文；保存时传明文回去、C# 侧加密。
  编辑时留空 = 不动密码，这是 UpdateProxyAccount_ByAccountID 本来的语义。
*/
import { computed, ref, watch } from 'vue'
import { call } from '../../bridge'
import { FeedList, type AccountRow } from '../../bridge/types'
import { t } from '../../i18n'
import { useList } from '../../stores/lists'
import SettingsModal from './SettingsModal.vue'

const props = defineProps<{ id: string | null }>()
const emit = defineEmits<{ (e: 'close'): void }>()

const rows = useList<AccountRow>(FeedList.Account)

const busy = ref(false)
const error = ref('')

const isNew = computed(() => props.id === '')

interface Form {
  userName: string
  password: string
  isEnable: boolean
  isLimitLinks: boolean
  limitLinks: number
  isLimitDevices: boolean
  limitDevices: number
  isExpiry: boolean
  expiryTime: string
}

/** 新增时的默认值：启用、不限量、不过期 —— 与 WinForms 那张空表单一致。 */
function blank(): Form {
  return {
    userName: '',
    password: '',
    isEnable: true,
    isLimitLinks: false,
    limitLinks: 1,
    isLimitDevices: false,
    limitDevices: 1,
    isExpiry: false,
    expiryTime: defaultExpiry(),
  }
}

/*
  勾上「设置过期时间」时给一个能用的默认值（一个月后），而不是留空。
  <input type="datetime-local"> 只认 "yyyy-MM-ddTHH:mm"，得自己拼 ——
  toISOString() 会转成 UTC，直接用会把本地时间平移几个时区。
*/
function defaultExpiry(): string {
  const d = new Date()
  d.setMonth(d.getMonth() + 1)
  return toLocalInput(d)
}

function toLocalInput(d: Date): string {
  const p = (n: number) => String(n).padStart(2, '0')
  return d.getFullYear() + '-' + p(d.getMonth() + 1) + '-' + p(d.getDate())
    + 'T' + p(d.getHours()) + ':' + p(d.getMinutes())
}

const f = ref<Form>(blank())

watch(() => props.id, async (id) => {
  if (id === null) return

  error.value = ''
  f.value = blank()

  if (id === '') return

  const r = rows.value.find((x) => x.Id === id)

  if (r) {
    f.value = {
      userName: r.UserName,
      password: '',
      isEnable: r.IsEnable,
      isLimitLinks: r.IsLimitLinks,
      limitLinks: r.LimitLinks || 1,
      isLimitDevices: r.IsLimitDevices,
      limitDevices: r.LimitDevices || 1,
      isExpiry: r.IsExpiry,
      //推过来的是 "yyyy-MM-dd HH:mm:ss"，input 要 "yyyy-MM-ddTHH:mm"
      expiryTime: r.IsExpiry ? r.ExpiryTime.replace(' ', 'T').slice(0, 16) : defaultExpiry(),
    }
  }

  /*
    密码单独取。取不到就留空 —— 留空在编辑时本来就表示「不动密码」，
    所以这一步失败也不会把用户的密码冲掉。
  */
  try {
    const p = await call<{ password: string }>('getAccountPassword', { id })
    f.value.password = p?.password || ''
  } catch (e) {
    console.error('[acct] 读取密码失败', e)
  }
}, { immediate: true })

async function save(): Promise<void> {
  busy.value = true
  error.value = ''

  try {
    const r = await call<any>('saveAccount', {
      id: isNew.value ? '' : props.id,
      userName: f.value.userName,
      password: f.value.password,
      isEnable: f.value.isEnable,
      isLimitLinks: f.value.isLimitLinks,
      limitLinks: Number(f.value.limitLinks) || 0,
      isLimitDevices: f.value.isLimitDevices,
      limitDevices: Number(f.value.limitDevices) || 0,
      isExpiry: f.value.isExpiry,
      //补上秒，C# 侧走 DateTime.TryParse
      expiryTime: f.value.expiryTime ? f.value.expiryTime.replace('T', ' ') + ':00' : '',
    })

    if (!r?.ok) {
      error.value = r?.error || ''
      return
    }

    emit('close')
  } catch (e) {
    console.error('[acct] 保存账号失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}
</script>

<template>
  <SettingsModal
    :open="props.id !== null"
    :title="isNew ? t('acct.add') : t('acct.edit')"
    subtitle="Proxy Account"
    :busy="busy"
    :error="error"
    @update:open="emit('close')"
    @save="save"
  >
    <!-- 三段各一个组标题，不写整句说明 —— 这张表单的字段名本身已经说清了 -->
    <div class="grp">{{ t('acct.e.lead') }}</div>

    <div class="row">
      <div class="k">{{ t('acct.e.user') }}</div>
      <div class="v">
        <!-- 编辑时只读：用户名是登录凭据，改了等于换一个账号 -->
        <input v-model="f.userName" class="inp" spellcheck="false" :readonly="!isNew" autocomplete="off">
        <span v-if="!isNew" class="tip">{{ t('acct.e.userLocked') }}</span>
      </div>
    </div>

    <div class="row">
      <div class="k">{{ t('acct.e.pass') }}</div>
      <div class="v">
        <input v-model="f.password" class="inp" type="text" spellcheck="false" autocomplete="off">
        <span v-if="!isNew" class="tip">{{ t('acct.e.passKeep') }}</span>
      </div>
    </div>

    <div class="row">
      <div class="k" />
      <div class="v">
        <button class="chk" :class="{ on: f.isEnable }" @click="f.isEnable = !f.isEnable">
          <i />{{ t('acct.e.enable') }}
        </button>
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
        <span class="tip">{{ f.isLimitLinks ? '' : t('acct.unlimited') }}</span>
      </div>
    </div>

    <div class="row">
      <button class="chk k" :class="{ on: f.isLimitDevices }" @click="f.isLimitDevices = !f.isLimitDevices">
        <i />{{ t('col.devices') }}
      </button>
      <div class="v">
        <input v-model.number="f.limitDevices" class="inp num" type="number" min="1" max="10000"
               :disabled="!f.isLimitDevices">
        <span class="tip">{{ f.isLimitDevices ? '' : t('acct.unlimited') }}</span>
      </div>
    </div>

    <div class="grp">{{ t('col.expiry') }}</div>

    <div class="row">
      <button class="chk k" :class="{ on: f.isExpiry }" @click="f.isExpiry = !f.isExpiry">
        <i />{{ t('acct.e.expiry') }}
      </button>
      <div class="v">
        <input v-model="f.expiryTime" class="inp dt" type="datetime-local" :disabled="!f.isExpiry">
        <span class="tip">{{ f.isExpiry ? '' : t('acct.never') }}</span>
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

/* 第一个组标题紧跟弹窗标题栏，不需要那么大的上间距 */
.grp:first-child { margin-top: 14px; }

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

/*
  ⚠️ 这行提示<b>要换行，不能省略号</b>。中文写得下的一句，换成俄语 / 越南语常常长一倍
  （「100–500000. При достижении очищается весь список, а не только старые строки」），
  nowrap + ellipsis 会把后半句直接吃掉，而那半句正是要紧的部分。
  外面的 .setf .row > .v 已经是 flex-wrap: wrap，让它自己折下去即可。
*/
.tip { font-size: var(--fs-small); color: var(--dim); line-height: 1.5; }

/* 基样式在 style.css 的「勾选框 / 单选框」，这里只覆盖框线色与布局 */
.chk { --chk-ring: var(--border); }
.chk.k { justify-self: start; }

/* 基样式在 style.css 的 .inp，这里只补布局 */
.inp { flex: 1; min-width: 0; }

.inp:read-only { color: var(--muted); background: rgb(var(--inset-rgb) / 15%); }
.inp.num { flex: none; width: 110px; font-variant-numeric: tabular-nums; }
.inp.dt { flex: none; width: 200px; }

/* Chromium 的日期选择器图标默认是深色的，在黑底上几乎看不见 */
.inp.dt::-webkit-calendar-picker-indicator { filter: invert(0.7); cursor: pointer; }
</style>
