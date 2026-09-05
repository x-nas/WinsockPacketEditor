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

const PROTO = ['http', 'https']

function urlOf(p: number, host: string, port: number, path: string): string {
  return (PROTO[p] ?? 'http') + '://' + host + ':' + port + (path || '')
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
  <SettingsModal :open="props.open" :title="t('set.map')" subtitle="Controls/MapSetting" :busy="busy" :error="error" :width="900"
                 @update:open="emit('update:open', $event)" @save="save">
    <div class="setf list-page ms">
      <!-- 本地映射 -->
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
            <span class="url" :title="urlOf(r.Protocol, r.Host, r.Port, r.RemotePath)">{{ urlOf(r.Protocol, r.Host, r.Port, r.RemotePath) }}</span>
            <span class="file" :title="r.LocalPath">{{ r.LocalPath }}</span>
            <span class="ops">
              <button class="op" :title="t('acct.op.edit')" @click.stop="localEdit = r"><svg class="ico" viewBox="0 0 24 24"><path d="M4 20h4L20 8l-4-4L4 16z" /></svg></button>
              <button class="op del" :title="t('acct.op.del')" @click.stop="action(false, r.Id, ListAction.Delete)"><svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg></button>
            </span>
          </div>
        </div>
      </div>

      <!-- 远程映射 -->
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
            <span class="url" :title="urlOf(r.ProtocolFrom, r.HostFrom, r.PortFrom, r.PathFrom)">{{ urlOf(r.ProtocolFrom, r.HostFrom, r.PortFrom, r.PathFrom) }}</span>
            <span class="url to" :title="urlOf(r.ProtocolTo, r.HostTo, r.PortTo, r.PathTo)">{{ urlOf(r.ProtocolTo, r.HostTo, r.PortTo, r.PathTo) }}</span>
            <span class="ops">
              <button class="op" :title="t('acct.op.edit')" @click.stop="remoteEdit = r"><svg class="ico" viewBox="0 0 24 24"><path d="M4 20h4L20 8l-4-4L4 16z" /></svg></button>
              <button class="op del" :title="t('acct.op.del')" @click.stop="action(true, r.Id, ListAction.Delete)"><svg class="ico" viewBox="0 0 24 24"><path d="M18 6L6 18M6 6l12 12" /></svg></button>
            </span>
          </div>
        </div>
      </div>
      <p class="hint">{{ t('map.saveHint') }}</p>
    </div>

    <ContextMenu :at="menuAt" :items="menuItems" @pick="onMenuPick" @close="menuAt = null" />
    <MapLocalEdit :target="localEdit" @close="localEdit = null" />
    <MapRemoteEdit :target="remoteEdit" @close="remoteEdit = null" />
  </SettingsModal>
</template>

<style scoped>
.lb { font-size: 11.5px; color: #8a94a6; }
.cnt { font-family: var(--mono); font-size: 11px; color: var(--muted); }
.tbl.dim .tbody { opacity: .55; }
.ms .tbody { max-height: 190px; }
.ms .head.hl, .ms .tr.hl { grid-template-columns: 50px minmax(200px, 1.2fr) minmax(200px, 1fr) 72px; }
.ms .head.hr, .ms .tr.hr { grid-template-columns: 50px minmax(200px, 1fr) minmax(200px, 1fr) 72px; }
.ms .head > span, .ms .tr > span { text-align: left; }
.url { font-family: var(--mono); font-size: 12px; color: var(--cyan); }
.url.to { color: #6ee7a8; }
.file { font-family: var(--mono); font-size: 12px; color: #94a3b8; }
</style>
