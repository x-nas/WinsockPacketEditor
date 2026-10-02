<script setup lang="ts">
/*
  拦截设置 —— 对应 WinForms 的 Controls/HookSetting。

  两段：抓取方向（TCP / UDP 的请求与响应）+ 拆包。

  【两种模式各看一页】WinForms 是 tabHookSettings 按宿主窗体选页
  （HookSetting 26–33）：注入模式那一页是 12 个 WinSock 钩子
  （Send / SendTo / Recv / RecvFrom × WS1.1 / WS2.0 / WSA），
  代理模式那一页是四个方向 + 拆包。这里按 mode 挑一页显示，理由一样 ——
  代理模式的包不走那 12 个钩子，摆出来只会是永远不起作用的开关。

  ⚠️ 保存时<b>只送当前这一页</b>，另一组整个不出现在报文里（不是送 false）。
  C# 侧按「字段存不存在」决定改不改；送 false 的话在代理模式点一次保存
  就会把注入的 12 个钩子全关掉。

  【这四个开关的分量】它们直接决定包会不会被抓：

      if (HookTCP_Req) { DoFilter_SOCKS_TCP(...); }        // 抓包 + 跑滤镜 + 进列表
      else             { psSession.TargetSocket.Send(...); } // 直接转发，什么都不做

  关掉哪个方向，那个方向就既不进列表也不过滤镜 —— 所以下面给了一句醒目的说明，
  而不是让人自己去猜"为什么没数据了"。

  【两种存续方式，界面上必须说清】
    抓取方向  <b>不落库</b>，只在本次运行内有效，重启回到全开
    拆包      落库（ProxyMode 表）
  这不是我们选的，是源码就这样（HookTCP_* 在 Operate.cs 里只有初始化、没有读写路径）。
*/
import { computed, ref, watch } from 'vue'
import { call } from '../../bridge'
import { t, type Key } from '../../i18n'
import ContextMenu from '../ContextMenu.vue'
import { ICON, type MenuItem } from '../menu'
import SettingsModal from './SettingsModal.vue'
import UnpackRuleEdit, { type UnpackRuleRow } from './UnpackRuleEdit.vue'

const props = withDefaults(
  defineProps<{ open: boolean; mode?: 'proxy' | 'inject' }>(),
  { mode: 'proxy' },
)
const emit = defineEmits<{ (e: 'update:open', v: boolean): void }>()

const busy = ref(false)
const error = ref('')

interface Form {
  //注入模式那 12 个
  ws1Send: boolean; ws1SendTo: boolean; ws1Recv: boolean; ws1RecvFrom: boolean
  ws2Send: boolean; ws2SendTo: boolean; ws2Recv: boolean; ws2RecvFrom: boolean
  wsaSend: boolean; wsaSendTo: boolean; wsaRecv: boolean; wsaRecvFrom: boolean
  //代理模式那四个 + 拆包
  tcpReq: boolean
  tcpResp: boolean
  udpReq: boolean
  udpResp: boolean
  unpack: boolean
  unpackHead: string
  unpackLength: string
  unpackRules: UnpackRuleRow[]
}

const form = ref<Form>({
  ws1Send: true, ws1SendTo: true, ws1Recv: true, ws1RecvFrom: true,
  ws2Send: true, ws2SendTo: true, ws2Recv: true, ws2RecvFrom: true,
  wsaSend: true, wsaSendTo: true, wsaRecv: true, wsaRecvFrom: true,
  tcpReq: true,
  tcpResp: true,
  udpReq: true,
  udpResp: true,
  unpack: false,
  unpackHead: '01 00 00',
  unpackLength: '4-5',
  unpackRules: [{ Id: '', Name: t('set.hook.defaultRuleName'), IsEnable: true, Direction: 0, Header: '01 00 00', Length: '4-5' }],
})

/** 注入模式那一页的三组，文案沿用封包类型那一族键（与过滤设置的类别同一套口径）。 */
const WS_GROUPS: Array<{ cap: Key; items: Array<{ key: keyof Form; label: Key }> }> = [
  {
    cap: 'set.grp.hookWs1',
    items: [
      { key: 'ws1Send', label: 'pt.ws2Send' },
      { key: 'ws1SendTo', label: 'pt.ws2SendTo' },
      { key: 'ws1Recv', label: 'pt.ws2Recv' },
      { key: 'ws1RecvFrom', label: 'pt.ws2RecvFrom' },
    ],
  },
  {
    cap: 'set.grp.hookWs2',
    items: [
      { key: 'ws2Send', label: 'pt.ws2Send' },
      { key: 'ws2SendTo', label: 'pt.ws2SendTo' },
      { key: 'ws2Recv', label: 'pt.ws2Recv' },
      { key: 'ws2RecvFrom', label: 'pt.ws2RecvFrom' },
    ],
  },
  {
    cap: 'set.grp.hookWsa',
    items: [
      { key: 'wsaSend', label: 'pt.wsaSend' },
      { key: 'wsaSendTo', label: 'pt.wsaSendTo' },
      { key: 'wsaRecv', label: 'pt.wsaRecv' },
      { key: 'wsaRecvFrom', label: 'pt.wsaRecvFrom' },
    ],
  },
]

const INJECT_KEYS = WS_GROUPS.flatMap((g) => g.items.map((x) => x.key))
const PROXY_KEYS: Array<keyof Form> = ['tcpReq', 'tcpResp', 'udpReq', 'udpResp', 'unpack', 'unpackHead', 'unpackLength', 'unpackRules']

const unpackEdit = ref<UnpackRuleRow | null | 'add'>(null)
function saveUnpackRule(rule: UnpackRuleRow): void {
  if (unpackEdit.value === 'add') { form.value.unpackRules.push(rule); return }
  const i = form.value.unpackRules.findIndex((x) => x.Id === rule.Id)
  if (i >= 0) form.value.unpackRules.splice(i, 1, rule)
}
function delUnpackRule(i: number): void { form.value.unpackRules.splice(i, 1) }
function dirText(v: number): string { return v === 1 ? t('pt.req') : (v === 2 ? t('pt.resp') : t('set.hook.ruleDirBoth')) }

/*
  拆包规则仍在当前设置草稿中：右键排序 / 删除先改草稿，按「保存」才一并落库。
  菜单项目、禁用边界和视觉组件均与映射设置保持一致。
*/
const unpackMenuAt = ref<{ x: number; y: number } | null>(null)
const unpackMenuIndex = ref(-1)
const unpackMenuItems = computed<MenuItem[]>(() => {
  const i = unpackMenuIndex.value
  const lastIndex = form.value.unpackRules.length - 1
  const first = i <= 0
  const last = i < 0 || i === lastIndex
  return [
    { id: 'top', label: t('lst.top'), icon: ICON.top, disabled: first },
    { id: 'up', label: t('lst.up'), icon: ICON.up, disabled: first },
    { id: 'down', label: t('lst.down'), icon: ICON.down, disabled: last },
    { id: 'bottom', label: t('lst.bottom'), icon: ICON.bottom, disabled: last },
    { divider: true },
    { id: 'delete', label: t('lst.delete'), icon: ICON.del, danger: true },
  ]
})

function openUnpackMenu(e: MouseEvent, index: number): void {
  unpackMenuIndex.value = index
  unpackMenuAt.value = { x: e.clientX, y: e.clientY }
}

function onUnpackMenuPick(id: string): void {
  const i = unpackMenuIndex.value
  const rules = form.value.unpackRules
  if (i < 0 || i >= rules.length) return

  switch (id) {
    case 'top': rules.unshift(rules.splice(i, 1)[0]); break
    case 'up': [rules[i - 1], rules[i]] = [rules[i], rules[i - 1]]; break
    case 'down': [rules[i], rules[i + 1]] = [rules[i + 1], rules[i]]; break
    case 'bottom': rules.push(rules.splice(i, 1)[0]); break
    case 'delete': delUnpackRule(i); break
  }
}

async function unpackCommand(action: number): Promise<void> {
  try {
    const r = await call<{ rules?: UnpackRuleRow[] }>('unpackRulesCommand', { action, rules: form.value.unpackRules })
    if (r?.rules) form.value.unpackRules = r.rules
  } catch (e) { console.error('[set] 拆包规则操作失败', e) }
}

/** 有入口被关掉时才提示 —— 那是「看不到数据」的头号原因。 */
const anyInjectOff = computed(() => INJECT_KEYS.some((k) => !form.value[k]))
const proxyDirectionWarning = computed(() =>
  !form.value.tcpReq || !form.value.tcpResp || !form.value.udpReq || !form.value.udpResp
    ? t('set.hook.offWarn')
    : '',
)

watch(() => props.open, async (on) => {
  if (!on) return

  error.value = ''

  try {
    form.value = await call<Form>('getHookSetting')
  } catch (e) {
    console.error('[set] 读取拦截设置失败', e)
  }
})

async function save(): Promise<void> {
  busy.value = true
  error.value = ''

  try {
    /*
      只送当前这一页。<b>另一组必须整个不出现在报文里</b>（不是送 false）——
      C# 侧按「字段存不存在」决定改不改。
    */
    const body: Record<string, unknown> = { ...form.value }
    for (const k of props.mode === 'inject' ? PROXY_KEYS : INJECT_KEYS) delete body[k]

    const r = await call<{ ok: boolean; error: string }>('saveHookSetting', body)

    if (!r?.ok) {
      error.value = r?.error || ''
      return
    }

    emit('update:open', false)
  } catch (e) {
    console.error('[set] 保存拦截设置失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}
</script>

<template>
  <SettingsModal
    :open="props.open"
    :title="t('set.hook')"
    subtitle="Interception"
    :busy="busy"
    :error="error"
    :hint="props.mode === 'proxy' ? proxyDirectionWarning : undefined"
    :width="900"
    @update:open="emit('update:open', $event)"
    @save="save"
  >    <div class="setf list-page" style="--setf-k: 152px">

    <!-- ── 注入模式：12 个 WinSock 钩子 ────────────────────── -->
    <template v-if="props.mode === 'inject'">
      <section class="sec">
      <div class="grp">{{ t('set.grp.hookDir') }}</div>

      <!-- 组名放进标签列：三组各四个入口，右边一行排开正好 -->
      <div v-for="g in WS_GROUPS" :key="g.cap" class="row">
        <div class="k">{{ t(g.cap) }}</div>
        <div class="v">
          <button
            v-for="x in g.items"
            :key="x.key"
            class="chk"
            :class="{ on: form[x.key] }"
            @click="(form[x.key] as boolean) = !form[x.key]"
          ><i />{{ t(x.label) }}</button>
        </div>
      </div>

      <p class="hint">{{ t('set.hook.injectHint') }}</p>
      </section>
      <p v-if="anyInjectOff" class="warn">{{ t('set.hook.injectWarn') }}</p>
    </template>

    <!-- ── 代理模式：四个方向 + 拆包 ───────────────────────── -->
    <template v-else>
    <section class="sec">
    <div class="grp">{{ t('set.grp.hookDir') }}</div>

    <div class="row direction-row">
      <div class="k">{{ t('set.hook.tcp') }}</div>
      <div class="v">
        <button class="chk" :class="{ on: form.tcpReq }" @click="form.tcpReq = !form.tcpReq">
          <i />{{ t('pt.req') }}
        </button>
        <button class="chk" :class="{ on: form.tcpResp }" @click="form.tcpResp = !form.tcpResp">
          <i />{{ t('pt.resp') }}
        </button>
      </div>
    </div>
    <div class="row direction-row">
      <div class="k">{{ t('set.hook.udp') }}</div>
      <div class="v">
        <button class="chk" :class="{ on: form.udpReq }" @click="form.udpReq = !form.udpReq">
          <i />{{ t('pt.req') }}
        </button>
        <button class="chk" :class="{ on: form.udpResp }" @click="form.udpResp = !form.udpResp">
          <i />{{ t('pt.resp') }}
        </button>
      </div>
    </div>

    <p class="hint">{{ t('set.hook.dirHint') }}　{{ t('set.hook.runOnly') }}</p>
    </section>

    <section class="sec unpack">
    <div class="grp">{{ t('set.grp.unpack') }}</div>

    <div class="row unpack-switch">
      <div class="k">{{ t('set.hook.unpack') }}</div>
      <div class="v">
        <button class="chk" :class="{ on: form.unpack }" @click="form.unpack = !form.unpack">
          <i />{{ t('set.speedModeOn') }}
        </button>
      </div>
    </div>
    <p class="hint">{{ t('set.hook.unpackHint') }}</p>

    <div class="tbl urules">
      <div class="tbar">
        <button class="sbtn primary" @click="unpackEdit = 'add'">{{ t('fw.add') }}</button>
        <span class="grow" /><span class="cnt">{{ form.unpackRules.length }}</span>
        <button class="sbtn" @click="unpackCommand(8)">{{ t('map.import') }}</button>
        <button class="sbtn" :disabled="!form.unpackRules.length" @click="unpackCommand(5)">{{ t('map.export') }}</button>
        <button class="sbtn danger" :disabled="!form.unpackRules.length" @click="unpackCommand(7)">{{ t('rb.clearAll') }}</button>
      </div>
      <div class="tbody">
        <div class="head ur">
          <span class="ck">{{ t('col.enable') }}</span>
          <span>{{ t('dec.name') }}</span>
          <span>{{ t('dec.direction') }}</span>
          <span>{{ t('set.hook.head') }}</span>
          <span>{{ t('set.hook.length') }}</span>
          <span class="ops">{{ t('col.ops') }}</span>
        </div>
        <div v-if="!form.unpackRules.length" class="empty">{{ t('set.hook.unpackEmpty') }}</div>
        <div v-for="(r, i) in form.unpackRules" v-else :key="r.Id || i" class="tr ur" :class="{ off: !r.IsEnable }"
             @contextmenu.prevent="openUnpackMenu($event, i)"
             @dblclick="!($event.target as HTMLElement).closest('button') && (unpackEdit = r)">
          <span class="ck"><button class="chk" :class="{ on: r.IsEnable }" @click.stop="r.IsEnable = !r.IsEnable"><i /></button></span>
          <span :title="r.Name">{{ r.Name }}</span>
          <span>{{ dirText(r.Direction) }}</span>
          <span class="mono" :title="r.Header">{{ r.Header }}</span>
          <span class="mono" :title="r.Length">{{ r.Length }}</span>
          <span class="ops">
            <button class="op" :title="t('acct.op.edit')" @click.stop="unpackEdit = r"><svg class="ico" viewBox="0 0 24 24"><path d="M4 20h4L20 8l-4-4L4 16z" /></svg></button>
            <button class="op del" :title="t('acct.op.del')" @click.stop="delUnpackRule(i)"><svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg></button>
          </span>
        </div>
      </div>
    </div>
    </section>
    <UnpackRuleEdit :target="unpackEdit" @save="saveUnpackRule" @close="unpackEdit = null" />
    <ContextMenu :at="unpackMenuAt" :items="unpackMenuItems" @pick="onUnpackMenuPick" @close="unpackMenuAt = null" />
    </template>
    </div>
  </SettingsModal>
</template>

<style scoped>

/*
  用户（2026-10-02）：02 拆包表的下沿与卡片下边框之间留着一小段空隙 ——
  那是 .setf .sec 默认的 8px padding-bottom，外加表自己那 1px 下边框。
  与 2026-09-29 取值器编辑的 02 变量表同一条口径：
  卡底内边距归零、表的底边框让掉（否则与卡自己的下边框叠成双线），
  表高改为跟着行数自适应 —— 顶掉 .setf .tbl .tbody 那 260px 的上限，
  行多时整张弹窗长高、由弹窗内容区自己滚，而不是表内憋出一条小滚动区。
*/
.urules { margin-top: 10px; margin-bottom: 0; }
.sec.unpack { display: flex; flex-direction: column; min-height: 0; padding-bottom: 0; }
.unpack .urules { flex: 1; min-height: 0; display: flex; flex-direction: column; border-bottom: 0; }
.unpack .urules .tbody { flex: 1; min-height: 0; max-height: none; overflow-y: auto; }
.urules .head.ur, .urules .tr.ur { grid-template-columns: 50px minmax(110px, .8fr) 82px minmax(150px, 1fr) 100px 100px; }
.urules .tr > span { min-width: 0; }
.urules .mono, .cnt { font-family: var(--mono); }
.setf .direction-row { grid-template-columns: max-content max-content; gap: 10px; }
.setf .unpack-switch { grid-template-columns: max-content max-content; gap: 10px; }

/* 有方向被关掉时才出现 —— 那是「看不到数据」的头号原因 */
.warn { padding: 0 20px; margin: 2px 0 4px; font-size: var(--fs-small); color: var(--amber); }

.tip { font-size: var(--fs-small); color: var(--dim2); }


</style>
