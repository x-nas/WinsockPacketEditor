<script setup lang="ts">
/*
  WPC 配置 —— 对应 WinForms 的 Controls/WPCConfig（两个 Tab：服务器列表 / 公告列表）。

  这一页管的是下发给 WPE Proxy Cap 客户端的两样东西：
  可用节点（/ProxyCap/GetServerList，每台服务器带一组 Clash 规则）与公告（/ProxyCap/GetNoticeList）。
  两份列表都在 FeedPump 的推送流里（FeedList.Server / Notice），这里只读副本、只发动作；
  规则是服务器下面的嵌套列表，点「规则」按需单独取（RuleList.vue）。

  Tab 换成工具条上的分段按钮，表在下面换列；右键：置顶 / 上移 / 下移 / 置底 / 删除 + 全选 / 取消选择
  （照 GetCMS_List，没有复制 / 导出）。
*/
import { computed, ref, watch } from 'vue'
import { call } from '../../bridge'
import { FeedList, ListAction, type NoticeRow, type ServerRow } from '../../bridge/types'
import { t } from '../../i18n'
import { useList } from '../../stores/lists'
import { pushToast } from '../../stores/toast'
import { useRowPick } from '../../usePick'
import ContextMenu from '../ContextMenu.vue'
import { ICON, type MenuItem } from '../menu'
import ServerEdit from './ServerEdit.vue'
import NoticeEdit from './NoticeEdit.vue'
import RuleList from './RuleList.vue'

const tab = ref<'server' | 'notice'>('server')
const servers = useList<ServerRow>(FeedList.Server)
const notices = useList<NoticeRow>(FeedList.Notice)

const sp = useRowPick(servers, (r) => r.Id)
const np = useRowPick(notices, (r) => r.Id)
const pick = computed(() => (tab.value === 'server' ? sp : np))

watch(tab, () => { sp.clear(); np.clear() })

/* ── 弹窗 ── */
const serverEdit = ref<ServerRow | null | 'add'>(null)
const noticeEdit = ref<NoticeRow | null | 'add'>(null)
const ruleServer = ref<ServerRow | null>(null)

/* ── 动作 ── */

async function toggleServer(r: ServerRow): Promise<void> {
  try { await call('setServerEnable', { id: r.Id, enable: !r.IsEnable }) }
  catch (e) { console.error('[wpc] 切换启用失败', e) }
}

async function clearAll(): Promise<void> {
  try { await call(tab.value === 'server' ? 'clearServers' : 'clearNotices') }
  catch (e) { console.error('[wpc] 清空失败', e) }
}

async function del(id: string): Promise<void> {
  try {
    await call(tab.value === 'server' ? 'serverListAction' : 'noticeListAction', { action: ListAction.Delete, ids: [id] })
  } catch (e) {
    console.error('[wpc] 删除失败', e)
  }
}

/* ── 右键菜单 ── */

const menuAt = ref<{ x: number; y: number } | null>(null)

const menuItems = computed<MenuItem[]>(() => {
  const n = pick.value.picked.value.size
  const tag = n ? ' (' + n + ')' : ''
  return [
    { id: 'top', label: t('lst.top') + tag, icon: ICON.top },
    { divider: true },
    { id: 'up', label: t('lst.up') + tag, icon: ICON.up },
    { id: 'down', label: t('lst.down') + tag, icon: ICON.down },
    { divider: true },
    { id: 'bottom', label: t('lst.bottom') + tag, icon: ICON.bottom },
    { divider: true },
    { id: 'delete', label: t('lst.delete') + tag, icon: ICON.del, danger: true },
    { divider: true },
    { id: 'selectAll', label: t('pm.selectAll'), icon: ICON.list },
    { id: 'deselect', label: t('pm.deselect'), icon: ICON.del, disabled: !n },
  ]
})

const ACTION_OF: Record<string, ListAction> = {
  top: ListAction.Top, up: ListAction.Up, down: ListAction.Down, bottom: ListAction.Bottom, delete: ListAction.Delete,
}

async function onMenuPick(id: string): Promise<void> {
  const p = pick.value
  if (id === 'selectAll') { p.selectAll(); return }
  if (id === 'deselect') { p.clear(); return }
  if (p.picked.value.size === 0) { pushToast('warning', t('lst.needPick')); return }

  const action = ACTION_OF[id]
  if (action === undefined) return

  try {
    await call(tab.value === 'server' ? 'serverListAction' : 'noticeListAction', { action, ids: p.pickedIds.value })
  } catch (e) {
    console.error('[wpc] 列表操作失败', e)
  }
}

const NOTICE_TYPE = ['', 'wpc.nt1', 'wpc.nt2', 'wpc.nt3', 'wpc.nt4', 'wpc.nt5'] as const

function noticeType(n: number): string {
  const k = NOTICE_TYPE[n]
  return k ? t(k) : String(n)
}

function short(u: string): string {
  return u ? u.replace(/^https?:\/\//, '') : '—'
}
</script>

<template>
  <div class="page list-page wpc">
    <div class="bar">
      <div class="seg">
        <button class="sg" :class="{ on: tab === 'server' }" @click="tab = 'server'">{{ t('wpc.servers') }} <b>{{ servers.length }}</b></button>
        <button class="sg" :class="{ on: tab === 'notice' }" @click="tab = 'notice'">{{ t('wpc.notices') }} <b>{{ notices.length }}</b></button>
      </div>

      <span class="sep" />

      <button v-if="tab === 'server'" class="btn primary" @click="serverEdit = 'add'">{{ t('wpc.addServer') }}</button>
      <button v-else class="btn primary" @click="noticeEdit = 'add'">{{ t('wpc.addNotice') }}</button>

      <!-- 放不下就省略号，完整的在悬停提示里；它同时替掉了原来那个 .grow，把清空按钮顶到最右 -->
      <span class="lb" :title="tab === 'server' ? t('wpc.serverHint') : t('wpc.noticeHint')">{{ tab === 'server' ? t('wpc.serverHint') : t('wpc.noticeHint') }}</span>

      <button class="btn danger" :disabled="tab === 'server' ? !servers.length : !notices.length" @click="clearAll">
        {{ tab === 'server' ? t('wpc.clearServers') : t('wpc.clearNotices') }}
      </button>
    </div>

    <div class="body">
      <!-- 服务器 -->
      <template v-if="tab === 'server'">
        <div class="head hs">
          <span class="no">{{ t('col.id') }}</span>
          <span class="ck">{{ t('col.enable') }}</span>
          <span class="name">{{ t('wpc.serverName') }}</span>
          <span class="addr">{{ t('wpc.serverAddr') }}</span>
          <span class="url">{{ t('wpc.forgotUrl') }}</span>
          <span class="url">{{ t('wpc.registerUrl') }}</span>
          <span class="url">{{ t('wpc.verifyUrl') }}</span>
          <span class="rules">{{ t('wpc.rules') }}</span>
          <span class="ops">{{ t('col.ops') }}</span>
        </div>

        <div v-if="!servers.length" class="empty">{{ t('wpc.emptyServers') }}</div>

        <div
          v-for="(r, i) in servers"
          v-else
          :key="r.Id"
          class="row hs"
          :class="{ off: !r.IsEnable, sel: sp.picked.value.has(r.Id) }"
          @click="sp.onRowClick(r, $event, i)"
          @contextmenu.prevent="menuAt = { x: $event.clientX, y: $event.clientY }"
          @dblclick="!($event.target as HTMLElement).closest('button') && (serverEdit = r)"
        >
          <span class="no">{{ i + 1 }}</span>
          <span class="ck"><button class="chk" :class="{ on: r.IsEnable }" :title="t('col.enable')" @click.stop="toggleServer(r)"><i /></button></span>
          <span class="name" :title="r.Name">{{ r.Name }}</span>
          <span class="addr">{{ r.IP }}<i>:</i>{{ r.Port }}</span>
          <span class="url" :title="r.ForgotURL">{{ short(r.ForgotURL) }}</span>
          <span class="url" :title="r.RegisterURL">{{ short(r.RegisterURL) }}</span>
          <span class="url" :title="r.VerifyURL">{{ short(r.VerifyURL) }}</span>
          <span class="rules" :class="{ none: !r.RuleCount }">{{ r.RuleCount }}</span>
          <span class="ops">
            <button class="op" :title="t('acct.op.edit')" @click.stop="serverEdit = r">
              <svg class="ico" viewBox="0 0 24 24"><path d="M4 20h4L20 8l-4-4L4 16z" /></svg>
            </button>
            <button class="op rule" :title="t('wpc.editRules')" @click.stop="ruleServer = r">
              <svg class="ico" viewBox="0 0 24 24"><path d="M4 12l16-8-6 16-2-6z" /></svg>
            </button>
            <button class="op del" :title="t('acct.op.del')" @click.stop="del(r.Id)">
              <svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg>
            </button>
          </span>
        </div>
      </template>

      <!-- 公告 -->
      <template v-else>
        <div class="head hn">
          <span class="no">{{ t('col.id') }}</span>
          <span class="ty">{{ t('col.type') }}</span>
          <span class="title">{{ t('wpc.noticeTitle') }}</span>
          <span class="more">{{ t('wpc.noticeMore') }}</span>
          <span class="time">{{ t('wpc.noticeTime') }}</span>
          <span class="ops">{{ t('col.ops') }}</span>
        </div>

        <div v-if="!notices.length" class="empty">{{ t('wpc.emptyNotices') }}</div>

        <div
          v-for="(r, i) in notices"
          v-else
          :key="r.Id"
          class="row hn"
          :class="{ sel: np.picked.value.has(r.Id) }"
          @click="np.onRowClick(r, $event, i)"
          @contextmenu.prevent="menuAt = { x: $event.clientX, y: $event.clientY }"
          @dblclick="!($event.target as HTMLElement).closest('button') && (noticeEdit = r)"
        >
          <span class="no">{{ i + 1 }}</span>
          <span class="ty"><i class="nt" :class="'n' + r.Type">{{ noticeType(r.Type) }}</i></span>
          <span class="title" :title="r.Content">{{ r.Title }}</span>
          <span class="more" :title="r.More">{{ short(r.More) }}</span>
          <span class="time">{{ r.Time }}</span>
          <span class="ops">
            <button class="op" :title="t('acct.op.edit')" @click.stop="noticeEdit = r">
              <svg class="ico" viewBox="0 0 24 24"><path d="M4 20h4L20 8l-4-4L4 16z" /></svg>
            </button>
            <button class="op del" :title="t('acct.op.del')" @click.stop="del(r.Id)">
              <svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg>
            </button>
          </span>
        </div>
      </template>
    </div>

    <ContextMenu :at="menuAt" :items="menuItems" @pick="onMenuPick" @close="menuAt = null" />

    <ServerEdit :target="serverEdit" @close="serverEdit = null" />
    <NoticeEdit :target="noticeEdit" @close="noticeEdit = null" />
    <RuleList :server="ruleServer" @close="ruleServer = null" />
  </div>
</template>

<style scoped>
.page { flex: 1; min-width: 0; min-height: 0; display: flex; flex-direction: column; gap: 8px; padding: 10px 12px 12px; }
.sep { width: 1px; height: 16px; background: var(--border); flex: none; }
/*
  ⚠️ `flex: 1 1 0`：假想宽度为 0，这句提示永远不会让工具条折行 —— 放不下就省略号。
  原来是默认的 `0 1 auto`，假想宽度 = 整句长度，125% 缩放下「清空所有服务器」被它挤到第二行（2026-09-11 改）。
*/
.lb { flex: 1 1 0; font-size: var(--fs-small); color: var(--dim2); white-space: nowrap; overflow: hidden; text-overflow: ellipsis; min-width: 0; }

.seg { display: inline-flex; border: 1px solid var(--border); flex: none; }
.sg { display: inline-flex; align-items: center; gap: 8px; padding: 8px 13px 8px; background: transparent; border: 0; color: var(--muted); font-family: var(--share); font-size: var(--btn-size); line-height: 1; letter-spacing: .12em; text-transform: uppercase; cursor: pointer; }
.sg b { font-family: var(--mono); font-weight: 400; font-size: var(--fs-small); color: var(--dim); }
.sg + .sg { border-left: 1px solid var(--border); }
.sg:hover { color: var(--gray); }
.sg.on { background: rgb(var(--cyan-rgb) / 10%); color: var(--cyan); }
.sg.on b { color: var(--cyan); }

.head.hs, .row.hs { grid-template-columns: 46px 50px minmax(120px, 1fr) 170px minmax(110px, 1fr) minmax(110px, 1fr) minmax(110px, 1fr) 60px 100px; gap: 8px; min-width: 980px; }
.head.hn, .row.hn { grid-template-columns: 46px 96px minmax(200px, 2fr) minmax(140px, 1fr) 160px 72px; gap: 8px; min-width: 760px; }

.head > span { overflow: hidden; text-overflow: ellipsis; }
.head > span, .row > span { text-align: center; }
/*
  ⚠️ 只有<b>变长标识</b>那两列左对齐：服务器名称、公告标题。
  其余全部居中 —— 含两张表的 URL 列（服务器的三列、公告的「更多」）：它们是等宽字体的短串
  （short() 已去掉 scheme），一竖列扫下来居中最省力；左对齐反而与旁边居中的列错开一档。
*/
.head > span.name, .row > span.name, .head > span.title, .row > span.title { text-align: left; }
.row > span { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }

.no { color: var(--dim); font-variant-numeric: tabular-nums; }
.name, .title { color: var(--gray); }
.addr { font-family: var(--mono); font-size: var(--fs-body); color: var(--cyan); }
.addr i { font-style: normal; color: var(--dim); margin: 0 1px; }
.url, .more { font-family: var(--mono); font-size: var(--fs-body); color: var(--dim3); }
.rules { font-family: var(--mono); color: var(--green); font-variant-numeric: tabular-nums; }
.rules.none { color: var(--dim); }
.time { font-family: var(--mono); font-size: var(--fs-body); color: var(--muted); font-variant-numeric: tabular-nums; }

/* 公告类型标签：配色照 WinForms 的 CellTag（活动情报蓝 / 维护说明黄 / 电竞赛事绿 / 限时商城紫 / 玩家社区蓝） */
.nt { display: inline-block; padding: 3px 7px 3px; border: 1px solid; font-family: var(--share); font-size: var(--fs-label); line-height: 1; letter-spacing: .06em; font-style: normal; }
.nt.n1, .nt.n5 { color: var(--nt1); border-color: rgb(var(--nt1-rgb) / 40%); background: rgb(var(--nt1-rgb) / 10%); }
.nt.n2 { color: var(--nt2); border-color: rgb(var(--nt2-rgb) / 40%); background: rgb(var(--nt2-rgb) / 10%); }
.nt.n3 { color: var(--nt3); border-color: rgb(var(--nt3-rgb) / 40%); background: rgb(var(--nt3-rgb) / 10%); }
.nt.n4 { color: var(--nt4); border-color: rgb(var(--nt4-rgb) / 40%); background: rgb(var(--nt4-rgb) / 10%); }

.op.rule { color: var(--amber); }
</style>
