<script setup lang="ts">
/*
  映射设置 —— 对应 WinForms 的 Controls/MapSetting（本地映射 / 远程映射两张表 + 各自的开关与菜单）。

  两份列表在 FeedPump 的推送流里（FeedList.MapLocal / MapRemote），这里只读副本、只发动作。
  表里没有主键，桥按模型上补的运行期 Id 收发。右键四个移动 + 删除，作用于开菜单的那一行（这两张表没有多选）。
  「保存」只管两个总开关；增删改在各自的动作里已经落库。
*/
import { computed, ref, watch } from 'vue'
import { call } from '../../bridge'
import { FeedList, ListAction, type MapLocalRow, type MapRemoteRow } from '../../bridge/types'
import { t } from '../../i18n'
import { useList } from '../../stores/lists'
import ContextMenu from '../ContextMenu.vue'
import { ICON, type MenuItem } from '../menu'
import SettingsModal from './SettingsModal.vue'
import MapLocalEdit from './MapLocalEdit.vue'
import MapRemoteEdit from './MapRemoteEdit.vue'

const props = defineProps<{ open: boolean }>()
const emit = defineEmits<{ (e: 'update:open', v: boolean): void }>()

const busy = ref(false)
const error = ref('')
const f = ref({ enableLocal: false, enableRemote: false })

const locals = useList<MapLocalRow>(FeedList.MapLocal)
const remotes = useList<MapRemoteRow>(FeedList.MapRemote)

const localEdit = ref<MapLocalRow | null | 'add'>(null)
const remoteEdit = ref<MapRemoteRow | null | 'add'>(null)

watch(() => props.open, async (on) => {
  if (!on) return
  error.value = ''
  try { f.value = await call<typeof f.value>('getMapSetting') }
  catch (e) { console.error('[map] 读取映射设置失败', e) }
}, { immediate: true })


// 协议一律是 http —— 运行期只有 DomainType.HTTP 那一支会查映射，见 MapRemoteEdit.vue 的文件头
function urlOf(host: string, port: number, path: string): string {
  return 'http://' + host + ':' + port + (path || '')
}

/*
  同一个地址拆成「主机」与「路径」两段给界面。

  ⚠️ 为什么不直接整串扔进一个 span：那样溢出时省略号切的是<b>尾部</b>，
  而一条映射规则里最能区分彼此的恰恰是路径的尾巴 ——
  `http://cdn.example.com:80/api/v1/config.j…` 等于什么都没说。
  拆开之后主机不参与收缩、路径从<b>头</b>那边省略（见样式里的 direction: rtl），
  于是长地址显示成 `http://cdn.example.com:80` + `…/v1/config.json`，两头都在。
*/
function hostOf(host: string, port: number): string {
  return 'http://' + host + ':' + port
}

function pathOf(path: string): string {
  return path || ''
}

async function toggle(remote: boolean, id: string, on: boolean): Promise<void> {
  try { await call('setMapEnable', { remote, id, enable: on }) }
  catch (e) { console.error('[map] 切换启用失败', e) }
}

async function action(remote: boolean, id: string, a: ListAction): Promise<void> {
  try { await call('mapAction', { remote, id, action: a }) }
  catch (e) { console.error('[map] 映射操作失败', e) }
}

async function command(remote: boolean, a: ListAction): Promise<void> {
  try { await call('mapCommand', { remote, action: a }) }
  catch (e) { console.error('[map] 整表操作失败', e) }
}

/* 右键 */
const menuAt = ref<{ x: number; y: number } | null>(null)
const menuRemote = ref(false)
const menuId = ref('')

const menuItems = computed<MenuItem[]>(() => {
  const list = menuRemote.value ? remotes.value : locals.value
  const i = list.findIndex((x) => x.Id === menuId.value)
  const first = i <= 0, last = i < 0 || i === list.length - 1
  return [
    { id: 'top', label: t('lst.top'), icon: ICON.top, disabled: first },
    { id: 'up', label: t('lst.up'), icon: ICON.up, disabled: first },
    { id: 'down', label: t('lst.down'), icon: ICON.down, disabled: last },
    { id: 'bottom', label: t('lst.bottom'), icon: ICON.bottom, disabled: last },
    { divider: true },
    { id: 'delete', label: t('lst.delete'), icon: ICON.del, danger: true },
  ]
})

const ACTION_OF: Record<string, ListAction> = { top: ListAction.Top, up: ListAction.Up, down: ListAction.Down, bottom: ListAction.Bottom, delete: ListAction.Delete }

function openMenu(e: MouseEvent, remote: boolean, id: string): void {
  menuRemote.value = remote
  menuId.value = id
  menuAt.value = { x: e.clientX, y: e.clientY }
}

function onMenuPick(id: string): void {
  const a = ACTION_OF[id]
  if (a !== undefined) void action(menuRemote.value, menuId.value, a)
}

async function save(): Promise<void> {
  busy.value = true
  error.value = ''
  try {
    await call('saveMapSetting', { ...f.value })
    emit('update:open', false)
  } catch (e) {
    console.error('[map] 保存失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}
</script>

<template>
  <SettingsModal :open="props.open" :title="t('set.map')" subtitle="Address Mapping" :busy="busy" :error="error" :width="900"
                 @update:open="emit('update:open', $event)" @save="save">
    <div class="setf list-page ms">
      <!--
        ⚠️ 这句要放在最上面。它是「用这个面板之前就该知道」的前提，
        原先摆在底部，要滚过两张表才看得见 —— 而那两张表是会长的，行一多更看不到。
      -->
      <p class="hint warn top">{{ t('map.httpOnly') }}</p>

      <!-- 本地映射 -->
      <section class="sec">
      <div class="grp">{{ t('map.local') }}</div>
      <div class="row">
        <div class="k">{{ t('col.enable') }}</div>
        <div class="v">
          <button class="chk" :class="{ on: f.enableLocal }" @click="f.enableLocal = !f.enableLocal"><i />{{ t('map.enableLocal') }}</button>
          <span class="lb">{{ t('map.localHint') }}</span>
        </div>
      </div>
      <div class="tbl" :class="{ dim: !f.enableLocal }">
        <div class="tbar">
          <button class="sbtn primary" @click="localEdit = 'add'">{{ t('fw.add') }}</button>
          <button class="sbtn" @click="command(false, ListAction.Import)">{{ t('map.import') }}</button>
          <button class="sbtn" :disabled="!locals.length" @click="command(false, ListAction.Export)">{{ t('map.export') }}</button>
          <span class="grow" />
          <span class="cnt">{{ locals.length }}</span>
          <button class="sbtn danger" :disabled="!locals.length" @click="command(false, ListAction.CleanUp)">{{ t('rb.clearAll') }}</button>
        </div>
        <div class="tbody">
          <div class="head hl">
            <span class="ck">{{ t('col.enable') }}</span>
            <span class="url">{{ t('map.remoteAddr') }}</span>
            <span class="file">{{ t('map.localFile') }}</span>
            <span class="ops">{{ t('col.ops') }}</span>
          </div>
          <div v-if="!locals.length" class="empty">{{ t('map.emptyLocal') }}</div>
          <div v-for="r in locals" v-else :key="r.Id" class="tr hl" :class="{ off: !r.IsEnable }"
               @contextmenu.prevent="openMenu($event, false, r.Id)" @dblclick="!($event.target as HTMLElement).closest('button') && (localEdit = r)">
            <span class="ck"><button class="chk" :class="{ on: r.IsEnable }" @click.stop="toggle(false, r.Id, !r.IsEnable)"><i /></button></span>
            <span class="url" :title="urlOf(r.Host, r.Port, r.RemotePath)"><bdi class="uh">{{ hostOf(r.Host, r.Port) }}</bdi><bdi class="up"><span dir="ltr">{{ pathOf(r.RemotePath) }}</span></bdi></span>
            <span class="file" :title="r.LocalPath">{{ r.LocalPath }}</span>
            <span class="ops">
              <button class="op" :title="t('acct.op.edit')" @click.stop="localEdit = r"><svg class="ico" viewBox="0 0 24 24"><path d="M4 20h4L20 8l-4-4L4 16z" /></svg></button>
              <button class="op del" :title="t('acct.op.del')" @click.stop="action(false, r.Id, ListAction.Delete)"><svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg></button>
            </span>
          </div>
        </div>
      </div>
      </section>

      <!-- 远程映射 -->
      <section class="sec">
      <div class="grp">{{ t('map.remote') }}</div>
      <div class="row">
        <div class="k">{{ t('col.enable') }}</div>
        <div class="v">
          <button class="chk" :class="{ on: f.enableRemote }" @click="f.enableRemote = !f.enableRemote"><i />{{ t('map.enableRemote') }}</button>
          <span class="lb">{{ t('map.remoteHint') }}</span>
        </div>
      </div>
      <div class="tbl" :class="{ dim: !f.enableRemote }">
        <div class="tbar">
          <button class="sbtn primary" @click="remoteEdit = 'add'">{{ t('fw.add') }}</button>
          <button class="sbtn" @click="command(true, ListAction.Import)">{{ t('map.import') }}</button>
          <button class="sbtn" :disabled="!remotes.length" @click="command(true, ListAction.Export)">{{ t('map.export') }}</button>
          <span class="grow" />
          <span class="cnt">{{ remotes.length }}</span>
          <button class="sbtn danger" :disabled="!remotes.length" @click="command(true, ListAction.CleanUp)">{{ t('rb.clearAll') }}</button>
        </div>
        <div class="tbody">
          <div class="head hr">
            <span class="ck">{{ t('col.enable') }}</span>
            <span class="url">{{ t('map.reqAddr') }}</span>
            <span class="url">{{ t('map.mapAddr') }}</span>
            <span class="ops">{{ t('col.ops') }}</span>
          </div>
          <div v-if="!remotes.length" class="empty">{{ t('map.emptyRemote') }}</div>
          <div v-for="r in remotes" v-else :key="r.Id" class="tr hr" :class="{ off: !r.IsEnable }"
               @contextmenu.prevent="openMenu($event, true, r.Id)" @dblclick="!($event.target as HTMLElement).closest('button') && (remoteEdit = r)">
            <span class="ck"><button class="chk" :class="{ on: r.IsEnable }" @click.stop="toggle(true, r.Id, !r.IsEnable)"><i /></button></span>
            <span class="url" :title="urlOf(r.HostFrom, r.PortFrom, r.PathFrom)"><bdi class="uh">{{ hostOf(r.HostFrom, r.PortFrom) }}</bdi><bdi class="up"><span dir="ltr">{{ pathOf(r.PathFrom) }}</span></bdi></span>
            <span class="url to" :title="urlOf(r.HostTo, r.PortTo, r.PathTo)"><bdi class="uh">{{ hostOf(r.HostTo, r.PortTo) }}</bdi><bdi class="up"><span dir="ltr">{{ pathOf(r.PathTo) }}</span></bdi></span>
            <span class="ops">
              <button class="op" :title="t('acct.op.edit')" @click.stop="remoteEdit = r"><svg class="ico" viewBox="0 0 24 24"><path d="M4 20h4L20 8l-4-4L4 16z" /></svg></button>
              <button class="op del" :title="t('acct.op.del')" @click.stop="action(true, r.Id, ListAction.Delete)"><svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg></button>
            </span>
          </div>
        </div>
      </div>
      </section>
      <p class="hint">{{ t('map.saveHint') }}</p>
    </div>

    <ContextMenu :at="menuAt" :items="menuItems" @pick="onMenuPick" @close="menuAt = null" />
    <MapLocalEdit :target="localEdit" @close="localEdit = null" />
    <MapRemoteEdit :target="remoteEdit" @close="remoteEdit = null" />
  </SettingsModal>
</template>

<style scoped>
/* 顶到面板第一行时，.setf .hint 那点 margin-top 不够，单独给一档 */
.hint.top { margin: 12px 0 10px; }

.lb { font-size: var(--fs-small); color: var(--dim2); }
.cnt { font-family: var(--mono); font-size: var(--fs-small); color: var(--muted); }
.tbl.dim .tbody { opacity: .55; }
.ms .tbody { max-height: 190px; }
/* ⚠️ 操作列 72 → 96：俄语的「Действия」实测要 92px，72 下会被省略号截掉（七种语言里只有它超） */
.ms .head.hl, .ms .tr.hl { grid-template-columns: 50px minmax(200px, 1.2fr) minmax(200px, 1fr) 100px; }
.ms .head.hr, .ms .tr.hr { grid-template-columns: 50px minmax(200px, 1fr) minmax(200px, 1fr) 100px; }
.ms .head > span, .ms .tr > span { text-align: left; }
/*
  ── 地址：主机不收缩，路径从头省略 ──────────────────

  ⚠️ <b>.up 的 direction: rtl 是这套写法的关键</b>：它让省略号出现在<b>左</b>边，
  于是被切掉的是路径的头、留下的是尾（`…/v1/config.json`）。
  外面包 <bdi> 是必须的 —— 它把这一段隔离成独立的双向文本运行，
  不然 rtl 会把结尾的标点（`/` `?` `=`）甩到字符串另一头去。

  ⚠️ 主机那段 <b>flex: none</b>：宁可让路径少显示几个字符，也别把
  `http://cdn.example.com:80` 截成 `http://cdn.exa…` —— 那样两条规则就分不出是不是同一台主机了。
*/
.url { display: flex; align-items: baseline; min-width: 0; font-family: var(--mono); font-size: var(--fs-body); color: var(--cyan); }
.url.to { color: var(--acc-green2); }
.uh { flex: none; }
.up { flex: 0 1 auto; min-width: 0; overflow: hidden; text-overflow: ellipsis; white-space: nowrap; direction: rtl; }
/*
  ⚠️ 里面这层 <span dir="ltr"> 不能省。

  光有 direction: rtl 的话，路径开头那个 <b>/</b> 是「中性字符」，会被双向算法
  搬到字符串的<b>另一头</b> —— 实测显示成 `http://cdn.example.com:80config.json/`，
  斜杠跑到末尾去了。把整段路径圈成一个 LTR 运行，字符顺序就锁住了，
  而 rtl 只剩下它唯一的用处：让省略号出现在<b>左</b>边。
*/
.up > span { unicode-bidi: isolate; }
.file { font-family: var(--mono); font-size: var(--fs-body); color: var(--dim3); }
</style>
