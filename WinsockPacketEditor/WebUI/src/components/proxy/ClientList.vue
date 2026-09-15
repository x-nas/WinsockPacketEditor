<script setup lang="ts">
/*
  客户端列表 —— 对应 WinForms 的 Controls/ClientList。

  一张认证记录表：谁、从哪个 IP、连了几条、开了几个设备、跑了多少流量、在线多久。
  数据全部来自 B9d 的推送通道（FeedPump 已经 hook 了 lstAuthInfo → FeedList.Auth），
  这里只读前端副本，不产生往返。

  【没有「认证结果」列】AuthInfo 全项目只有一处构造，AuthResult 硬编码为 true
  （Operate.RefreshAuthList），也没有任何地方把它改回 false ——
  这张表本来就是从<b>当前活动会话</b>建的，认证没过的连接根本走不到这里。
  一个恒为「通过」的列比空着更误导，与封包列表去掉恒为 0 的「会话」列同一个道理。
  DTO 里的 AuthResult 字段留着（那是与 C# 的契约）。

  【右键菜单：加白名单 / 加黑名单】菜单结构照 Operate 的 GetCMS_AuthList：
  白名单只有"永久"一档，黑名单是 1 小时 / 1 天 / 30 天 / 永久四档。
  真正落库的是桥方法 addIpRule。

  【不做虚拟滚动】在线客户端是"当前连着的人"，几十到几百量级，
  跟账号列表（几万个，拿来卖的）不是一个数量级。定高窗口那套在这里是白付复杂度。
*/
import { computed, onBeforeUnmount, onMounted, ref, watch } from 'vue'
import { call } from '../../bridge'
import { DOMAIN_TYPE, FeedList, type AuthRow } from '../../bridge/types'
import { flagSrc } from '../../flags'
import { t, type Key } from '../../i18n'
import { useList } from '../../stores/lists'
import { pushToast } from '../../stores/toast'
import ContextMenu from '../ContextMenu.vue'
import { ICON, type MenuItem } from '../menu'
import { ipKey, timeKey, useSort } from '../../useSort'
import { useScrollGutter } from '../../useScrollGutter'

const rows = useList<AuthRow>(FeedList.Auth)

/* 表头在滚动容器外，右内边距按实测的滚动条槽位补（WebView2 的覆盖式滚动条下是 0，写死 10 就会错位 10px） */
const bodyEl = ref<HTMLElement | null>(null)
const gutter = useScrollGutter(bodyEl)
const cbodyEl = ref<HTMLElement | null>(null)
const cgutter = useScrollGutter(cbodyEl)

/*
  表头排序。WinForms 那张表的八列里有六列带 SortMode，这里照着给同样六列
  （所属地与国旗那一列那边也不排，它是按 IP 查出来的附属信息）。

  <b>IP 不能按字符串排</b>：10.10.10.9 会跑到 10.10.10.10 后面，用 ipKey 转成数字。
  在线分钟是前端按认证时间现算的（onlineOf），排序直接排认证时间，等价且不必等那 20 秒的 tick。
*/
const sort = useSort<AuthRow>(rows, {
  time: (r) => timeKey(r.AuthTime),
  user: (r) => r.UserName || '',
  ip: (r) => ipKey(r.AuthIP),
  links: (r) => r.LinksNumber,
  devices: (r) => r.DevicesNumber,
  traffic: (r) => r.TrafficStatistics,
})

/** 表格显示的是排过的那一份；选中态按 AuthIP 认，与顺序无关。 */
const shown = sort.sorted

/*
  「客户端地」列宽可拖（2026-09-15 用户报：长地址看不全）。做法照防火墙名单 / 封包列表：
  手柄挂在表头格右边界、监听挂 window（快拖时鼠标会跑出那条 7px 窄条）、双击恢复默认，<b>不持久化</b>。

  其余列维持原来的 grid：IP 与设备标识仍是弹性列，拖宽时由它们让出空间。
  这张表的表头在滚动容器外面、不横向滚，所以上限按表头实际宽度算 ——
  固定列 476 + 两根弹性列下限 130 + 60 + 8 条 12px 间隙 + 左右内边距（右边含滚动条槽位），
  超过就会把整行撑出容器、表头与行错位。
  设备标识的下限只给 60：这一格不是「—」就是截断的指纹（全文在悬停提示里），
  原来的 110 让「客户端地」在 1280 窗口里最多只能拖到 234px，长地址照样看不全。
*/
const LOC_DEF = 150
const LOC_MIN = 80
const locW = ref(LOC_DEF)
const headEl = ref<HTMLElement | null>(null)

const gridCols = computed(
  () => `72px 116px minmax(130px, 1.2fr) ${locW.value}px 56px 56px 88px 88px minmax(60px, .9fr)`)

function locMax(): number {
  const w = headEl.value?.clientWidth ?? 0
  return Math.max(LOC_MIN, w - 14 - (14 + gutter.value) - 8 * 12 - 476 - (130 + 60))
}

let drag: { x: number; w: number } | null = null

function onDragMove(e: MouseEvent): void {
  if (!drag) return
  locW.value = Math.min(locMax(), Math.max(LOC_MIN, Math.round(drag.w + e.clientX - drag.x)))
}

function onDragUp(): void {
  drag = null
  window.removeEventListener('mousemove', onDragMove)
  window.removeEventListener('mouseup', onDragUp)
  document.body.classList.remove('col-resizing')
}

function startResize(e: MouseEvent): void {
  drag = { x: e.clientX, w: locW.value }
  //整页禁选 + 统一光标，否则拖动会把表头文字刷成选中态
  document.body.classList.add('col-resizing')
  window.addEventListener('mousemove', onDragMove)
  window.addEventListener('mouseup', onDragUp)
}

/** 双击手柄恢复默认宽度 —— 拖乱了不必去猜原来是多少。 */
function resetLoc(): void { locW.value = LOC_DEF }

//拖着列宽时切走这一页：监听与整页的 col-resizing 不收掉就一直挂着
onBeforeUnmount(() => { if (drag) onDragUp() })

/*
  「在线多久」是<b>算出来的</b>，不是推过来的（AuthRow 只有认证时刻）。
  推送只在认证列表本身变化时发生，所以要自己走表 —— 20 秒一拍：
  这一列的单位是分钟，再密没有意义，而每一拍都会让整张表重算一次。
*/
const now = ref(Date.now())
let tick = 0

onMounted(() => { tick = window.setInterval(() => { now.value = Date.now() }, 20000) })
onBeforeUnmount(() => window.clearInterval(tick))

/*
  "yyyy-MM-dd HH:mm:ss" —— Chromium 认这个格式，但它按<b>本地时区</b>解析，
  而 C# 那边给的也是本地时间（DateTime.Now），两边对得上。
  解析不出来就显示 0，不要显示 NaN。
*/
function onlineMinutes(r: AuthRow): number {
  const ms = Date.parse((r.AuthTime || '').replace(' ', 'T'))
  if (!Number.isFinite(ms)) return 0

  return Math.max(0, Math.floor((now.value - ms) / 60000))
}

/** 只取时分秒 —— 与 WinForms 的列 Render 一致（认证都发生在本次运行内）。 */
function hhmmss(s: string): string {
  return (s || '').slice(11) || s || ''
}

/** 与统计格子同一套换算，单位只到一位小数。 */
function bytes(v: number): string {
  let x = v || 0
  if (x < 1024) return x + ' B'

  const u = ['KB', 'MB', 'GB', 'TB']
  let i = -1
  while (x >= 1024 && i < u.length - 1) { x /= 1024; i++ }
  return x.toFixed(1) + ' ' + u[i]
}

/* ── 右键菜单 ────────────────────────────────────────────── */

const menuAt = ref<{ x: number; y: number } | null>(null)
const picked = ref<AuthRow | null>(null)

function openMenu(r: AuthRow, e: MouseEvent): void {
  picked.value = r
  menuAt.value = { x: e.clientX, y: e.clientY }
}

/*
  结构照 GetCMS_AuthList：白名单一档，黑名单四档放进二级菜单。

  <b>必须是 computed。</b>写成普通 const 的话 t() 只在 setup 时算一次，
  之后切语言这份数组不会重算 —— 菜单会一直停在打开时的那种语言，
  连关掉再开都没用（数组还是同一个）。账号列表 / 滤镜列表那三个菜单
  一直是 computed，这里是漏的一个。
*/
const MENU = computed<MenuItem[]>(() => [
  { id: 'white', label: t('cli.toWhite'), icon: ICON.eye },
  { divider: true },
  {
    id: 'black',
    label: t('cli.toBlack'),
    icon: ICON.eyeOff,
    danger: true,
    sub: [
      //三档有期限的用同一个计时器图标，永久用挂锁 —— 一眼分得出"会到期 / 不会"
      { id: 'b1h', label: t('cli.b1h'), icon: ICON.timer },
      { id: 'b1d', label: t('cli.b1d'), icon: ICON.timer },
      { id: 'b30d', label: t('cli.b30d'), icon: ICON.timer },
      { divider: true },
      { id: 'bever', label: t('cli.bever'), icon: ICON.ban, danger: true },
    ],
  },
])

const HOURS: Record<string, number> = { b1h: 1, b1d: 24, b30d: 24 * 30, bever: 0 }

async function onMenuPick(id: string): Promise<void> {
  const r = picked.value
  if (!r) return

  const ip = (r.AuthIP || '').trim()

  if (!ip) {
    pushToast('warning', t('cli.noIp'))
    return
  }

  //父项「加入黑名单」本身不是动作，只有它的四个子项才是
  if (id === 'black') return

  const black = id !== 'white'

  try {
    const res = await call<{ ok: boolean; error: string }>('addIpRule', {
      ip,
      black,
      hours: black ? HOURS[id] ?? 0 : 0,
    })

    if (!res?.ok) {
      pushToast('error', res?.error || '')
      return
    }

    pushToast('success', ip + ' ' + t(black ? 'cli.blackOk' : 'cli.whiteOk'))
  } catch (e) {
    console.error('[cli] 加入名单失败', e)
    pushToast('error', String(e))
  }
}

/* ── 选中客户端 → 它当前开着的连接 ────────────────────────── */

/*
  WinForms 那一屏是「左边一棵树 + 右边一张认证表」，树的<b>根</b>是哪些客户端连着、
  <b>叶子</b>才是每条连接。

  这里没照搬那棵树：它的根与右边那张表画的是同一份数据（都按 IP + 账号分组），
  一屏里摆两遍就是重复。改成「选中表里一行，下面列出这个客户端的连接」——
  一份客户端清单，多出来的信息（连接明细）挂在它下面，与代理数据页
  「上面一张表、下面十六进制」是同一种排法。

  连接是现算的（从 ProxyServer 的活动会话取），没有推送通道，所以按秒轮询；
  只在这一页挂着时才轮，切走就停。
*/
interface ConnRow {
  ClientIP: string
  ClientPort: number
  Target: string
  DomainType: number
  ServerAddress: string
  /** SOCKS5 UDP ASSOCIATE 的那条控制连接（C# 的 ClientConnRow.Udp）。 */
  Udp?: boolean
  /** WPC 的控制连接（C# 的 ClientConnRow.Wpc）：常驻、不连任何目标，所以目标与出口都是空的。 */
  Wpc?: boolean
}

/** DomainType 的 0 —— 端口认不出应用层协议的普通 TCP（C# 的 DomainType.Socket）。 */
const DOMAIN_SOCKET = 0

/*
  DOMAIN_TYPE 存的是 <b>i18n 键</b>（'dt.https' 这种），不是可显示的文字 ——
  与 PACKET_TYPE 同理，见 bridge/types.ts。直接渲染出来就是一串键名。
  照 PacketList 的 protoText 那样先展平成语言相关的普通对象。
*/
const protoText = computed<Record<number, string>>(() => {
  const m: Record<number, string> = {}
  for (const k of Object.keys(DOMAIN_TYPE)) {
    m[+k] = t(DOMAIN_TYPE[+k] as Key)
  }
  return m
})

/*
  「实际出口」跟「目标」不一样吗？

  ClientAddress（Target）是客户端<b>请求的</b>目标，只在解析 SOCKS5 请求时写一次。
  ServerAddress 是<b>实际连过去的</b>地址，会被改写：
    · 走外部代理时 → 上游代理的 IP:端口（Operate.cs:5276）
    · 命中远程映射规则时 → 规则里的 HostTo:PortTo（Operate.cs:5589）
  平时两者是同一个字符串（两个方法的拼法完全一样），所以只在不同的时候才显示。
*/
function diffVia(c: ConnRow): boolean {
  return !!c.ServerAddress && c.ServerAddress !== c.Target
}

const selectedIp = ref<string | null>(null)
const conns = ref<ConnRow[]>([])
let connTimer = 0

async function loadConns(): Promise<void> {
  const ip = selectedIp.value

  if (!ip) {
    conns.value = []
    return
  }

  try {
    const r = await call<{ items: ConnRow[] }>('getClientConnections', { ip })

    //选中行在这一次往返期间被切走了，落后的结果要丢掉
    if (selectedIp.value !== ip) return

    conns.value = r?.items || []
  } catch (e) {
    console.error('[cli] 取连接失败', e)
  }
}

function pick(r: AuthRow): void {
  selectedIp.value = r.AuthIP
  void loadConns()
}

onMounted(() => { connTimer = window.setInterval(loadConns, 1000) })
onBeforeUnmount(() => window.clearInterval(connTimer))

/** 选中的那个客户端还在不在表里 —— 断开之后要把下半部收干净。 */
watch(rows, () => {
  if (selectedIp.value && !rows.value.some((x) => x.AuthIP === selectedIp.value)) {
    selectedIp.value = null
    conns.value = []
  }
})

</script>

<template>
  <div class="page">
    <!--
      没有标题条：页名与在线数侧栏那一项已经在显示，
      「右键可加入名单」也不值得占一整行 —— 表格直接顶到上边。
    -->
    <!-- 带 .so 的这几格可点排序；所属地与在线分钟不排（一个是附属信息、一个跟着认证时间走）-->
    <div ref="headEl" class="head" :style="{ paddingRight: (14 + gutter) + 'px', gridTemplateColumns: gridCols }">
      <span class="so" :class="{ on: sort.active('time') }" @click="sort.toggle('time')">{{ t('cli.authTime') }}<i class="ar">{{ sort.mark('time') }}</i></span>
      <span class="so" :class="{ on: sort.active('user') }" @click="sort.toggle('user')">{{ t('col.user') }}<i class="ar">{{ sort.mark('user') }}</i></span>
      <span class="so" :class="{ on: sort.active('ip') }" @click="sort.toggle('ip')">{{ t('cli.ip') }}<i class="ar">{{ sort.mark('ip') }}</i></span>
      <span class="rz">
        {{ t('col.clientLoc') }}
        <i class="grip" :title="t('col.resizeHint')"
           @mousedown.prevent.stop="startResize($event)"
           @dblclick.prevent.stop="resetLoc()" />
      </span>
      <span class="so" :class="{ on: sort.active('links') }" @click="sort.toggle('links')">{{ t('cli.links') }}<i class="ar">{{ sort.mark('links') }}</i></span>
      <span class="so" :class="{ on: sort.active('devices') }" @click="sort.toggle('devices')">{{ t('cli.devices') }}<i class="ar">{{ sort.mark('devices') }}</i></span>
      <span class="so" :class="{ on: sort.active('traffic') }" @click="sort.toggle('traffic')">{{ t('cli.traffic') }}<i class="ar">{{ sort.mark('traffic') }}</i></span>
      <span>{{ t('cli.online') }}</span>
      <span>{{ t('cli.device') }}</span>
    </div>

    <div ref="bodyEl" class="body">
      <div v-if="!rows.length" class="empty">{{ t('cli.empty') }}</div>

      <div
        v-for="(r, i) in shown"
        v-else
        :key="r.AccountId + '|' + r.AuthIP + '|' + i"
        class="row"
        :class="{ sel: r.AuthIP === selectedIp }"
        :style="{ gridTemplateColumns: gridCols }"
        @click="pick(r)"
        @contextmenu.prevent="openMenu(r, $event)"
      >
        <span class="tm">{{ hhmmss(r.AuthTime) }}</span>
        <span class="user">{{ r.UserName }}</span>
        <span class="ip">{{ r.AuthIP }}</span>
        <!--
          国旗 / 局域网图标跟着<b>所属地</b>走，不跟 IP —— 与封包列表一致
          （那边 flag: true 也是挂在 ClientLocation / ServerLocation 两列上）。
          它描述的本来就是"这个地址属于哪儿"，挂在 IP 上是我这一页搞反了。
        -->
        <!-- 省略号挂在内层 .t：.loc 是 flex 容器，text-overflow 对它本身不起作用；拖不够宽时悬停看全文 -->
        <span class="loc" :title="r.IPLocation">
          <img class="flag" :src="flagSrc(r.IPLocation)" alt="" width="16" height="16"
               loading="eager" decoding="sync">
          <span class="t">{{ r.IPLocation }}</span>
        </span>
        <span class="num">{{ r.LinksNumber }}</span>
        <span class="num">{{ r.DevicesNumber }}</span>
        <span class="num">{{ bytes(r.TrafficStatistics) }}</span>
        <span class="num">{{ onlineMinutes(r) }}</span>
        <!--
          设备标识只有 WPC 有，普通 SOCKS5 客户端是「—」—— 这一格不是「—」就是 WPC，所以不另设「客户端」列。
          WPC 的版本（C# 的 Client 字段，"WPC 1.0"）放在悬停提示里，与指纹全文一起。
        -->
        <span class="dev" :class="{ wpc: r.DeviceId }" :title="r.DeviceId ? r.Client + ' · ' + r.DeviceId : ''">{{ r.DeviceId || '—' }}</span>
      </div>
    </div>

    <!--
      下半部：选中客户端的连接明细（WinForms 那棵树的叶子）。
      与代理数据页「上面一张表、下面十六进制」是同一种排法。
    -->
    <div class="conn">
      <div class="cbar">
        <span class="ctl">{{ t('cli.conns') }}</span>
        <span v-if="selectedIp" class="cip">{{ selectedIp }}</span>
        <span v-if="selectedIp" class="cnt">{{ conns.length }}</span>
        <!-- 没选中时的提示只放正文那一处，标题栏不重复 -->
      </div>

      <!--
        没有表头的话这几列根本看不出是什么（源端口 / 目标 / 协议 / 实际出口），
        尤其最后一列在绝大多数情况下与「目标」一模一样。

        ⚠️ <b>表头在滚动容器外面</b>，与上半部那张表同一个做法 —— 它不需要 sticky，
        配色也就能跟着用 --panel（与上半部的表头一致）。
        这张表<b>列宽是固定 grid、不可拖、也不横向滚</b>，所以「表头必须与行同容器」
        那条约束（封包列表 / 防火墙名单那种）在这里不成立。
      -->
      <div v-if="selectedIp && conns.length" class="chead" :style="{ paddingRight: (14 + cgutter) + 'px' }">
        <span>{{ t('cli.srcPort') }}</span>
        <span />
        <span>{{ t('cli.target') }}</span>
        <span>{{ t('col.proto') }}</span>
        <span :title="t('cli.viaHint')">{{ t('cli.via') }}</span>
      </div>

      <div ref="cbodyEl" class="cbody">
        <div v-if="!selectedIp" class="empty sm">{{ t('cli.pickHint') }}</div>
        <div v-else-if="!conns.length" class="empty sm">{{ t('cli.noConn') }}</div>

        <template v-else>

        <div v-for="(c, i) in conns" :key="c.ClientPort + '|' + i" class="crow">
          <span class="cport">:{{ c.ClientPort }}</span>
          <span class="carrow">→</span>
          <!--
            WPC 控制连接：目标、协议、出口三格都写明它是什么，而不是「— · 套接字 · —」——
            那样看着像一条坏掉的连接（2026-09-15 用户问过）。
          -->
          <template v-if="c.Wpc">
            <span class="ctarget wpc" :title="t('cli.wpcCtlTip')">{{ t('cli.wpcCtl') }}</span>
            <span class="cproto wpc" :title="t('cli.wpcCtlTip')">WPC</span>
            <span class="csrv same">—</span>
          </template>
          <template v-else>
          <span class="ctarget">{{ c.Target || '—' }}</span>
          <!--
            UDP 关联（SOCKS5 UDP ASSOCIATE）单独写「UDP」：它的 DomainType 是按端口猜的
            （53 → 套接字），照写就是「协议：套接字」，看不出这其实是一路 UDP。
            「套接字」＝ 端口认不出应用层协议的普通 TCP 连接，挂一句提示说清楚。
          -->
          <span v-if="c.Udp" class="cproto" :title="t('cli.udpTip')">UDP</span>
          <span v-else class="cproto" :title="c.DomainType === DOMAIN_SOCKET ? t('cli.socketTip') : undefined">
            {{ protoText[c.DomainType] || c.DomainType }}
          </span>
          <!--
            实际出口。相同时<b>写「直连」而不是一条短横</b> ——
            短横看着像"没取到数据"，而这里的事实是"没有被转走"，是有内容的。
            地址原样并排写两遍才是噪声；真正要一眼看见的是它<b>不一样</b>的那两种情况：
            走了外部代理（ServerAddress = 上游代理地址），或命中了远程映射规则。
          -->
          <!--
            UDP 关联没有「连过去的地址」—— 数据报每一个都可以发往不同的目标，
            ServerAddress 从来不写。原来画成一条短横，看着像「没取到」，照实写成「UDP 中继」。
          -->
          <span v-if="c.Udp" class="csrv same" :title="t('cli.udpTip')">{{ t('cli.udpRelay') }}</span>
          <span v-else class="csrv" :class="{ same: !diffVia(c) }">
            {{ diffVia(c) ? c.ServerAddress : (c.ServerAddress ? t('cli.direct') : '—') }}
          </span>
          </template>
        </div>
        </template>
      </div>
    </div>

    <ContextMenu :at="menuAt" :items="MENU" @pick="onMenuPick" @close="menuAt = null" />
  </div>
</template>

<style scoped>
.page { display: flex; flex-direction: column; min-height: 0; height: 100%; }


.cnt {
  font-family: var(--share);
  font-size: var(--fs-caption);
  letter-spacing: .1em;
  padding: 1px 8px;
  border: 1px solid var(--border);
  color: var(--muted);
}


/*
  九列。列宽比着封包列表那套给：时间与数字定宽，IP 与所属地弹性。
  表头与行共用同一份 grid-template-columns，改一处必须改两处 —— 所以合并成一条规则。
*/
/*
  行高与表头高<b>写死</b>，与账号列表 / 滤镜列表逐像素一致（表头 30、行 34）。
  原来是靠 line-height 撑出来的，结果这一页的表头比别处矮一截。
*/
.head,
.row {
  display: grid;
  /*
    九列。1280 窗口下内容区约 1036 CSS 宽，定宽 + 弹性列下限 + 间距要压在这个数以内：
    72+116+56+56+88+88 = 476，弹性下限 130+96+110 = 336，间距 8×12 = 96，内边距 38 → 946。
    「在线 (分钟)」那格 88px 是按表头字体（10px + .14em 字距）量的，再窄表头就被截。
  */
  /* 兜底值；实际列宽由模板里的 gridCols 内联给（「客户端地」可拖，见脚本里的 locW） */
  grid-template-columns: 72px 116px minmax(130px, 1.2fr) 150px 56px 56px 88px 88px minmax(60px, .9fr);
  align-items: center;
  gap: 12px;
  padding: 0 14px;
  font-size: var(--fs-body);
}

.head {
  flex: none;
  height: var(--th-h);
  background: var(--panel);
  border-bottom: 1px solid var(--border);
  /* 右内边距由模板按实测的滚动条宽度补（useScrollGutter，没有滚动条时是 0），这里只是没量到之前的兜底 */
  padding-right: 14px;
  font-family: var(--share);
  font-size: var(--th-size);
  letter-spacing: .14em;
  text-transform: uppercase;
  /*
    颜色比另外两张表亮（那边是 --muted，对底色只有 3.85:1）。
    10px 的小字压在 3.85 上本来就吃力 —— 这一点在十六进制那张表上已经反馈过一次，
    不在新表里重复。要统一的话该是把另外两张一起提上来，不是把这张压下去。
  */
  color: var(--th-fg);
}

/* 表头全部居中；内容里只有名称与地址两列靠左 —— 与滤镜日志同一条口径 */
.head > span,
.row > span { text-align: center; }

/* 表头不折行，理由同下面连接表的 .chead —— 折一次整条栏就高一倍 */
.head > span { white-space: nowrap; overflow: hidden; }

.head > span:nth-child(2),
.head > span:nth-child(3),
.head > span:nth-child(4),
.row > span:nth-child(2),
.row > span:nth-child(3),
.row > span:nth-child(4) { text-align: left; }

/*
  ⚠️ <b>不写 scrollbar-gutter: stable</b>（2026-09-15 去掉）：它在没有滚动条时也在右边留一条空槽，
  行（连同选中的底色）停在槽位前、表头却因为补了 gutter 铺到最右，看着就是「行没铺满」。
  表头的右内边距由 useScrollGutter 实测补：滚动条出现时内容框变窄、ResizeObserver 触发重量，两边照样对得齐。
*/
.body { flex: 1; min-height: 0; overflow-y: auto; }

.empty { padding: 40px 0; text-align: center; color: var(--muted); font-size: var(--fs-body); }

.row { height: 34px; border-bottom: 1px solid rgb(var(--border-rgb) / 45%); color: var(--soft); cursor: pointer; }
.row:hover { background: rgb(var(--tint-rgb) / 4%); }

/* 选中行：左侧一道青色标，与下半部的连接明细呼应 */
.row.sel { background: rgb(var(--cyan-rgb) / 10%); box-shadow: inset 2px 0 0 var(--cyan); }
.row > span { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }

.tm { color: var(--muted); font-variant-numeric: tabular-nums; }
.user { color: var(--gray); }
/* 国旗与文字同格：省一列，滚动时图和文永远对得上（与封包列表同一个理由）*/
.loc { display: flex; align-items: center; gap: 6px; color: var(--dim3); }
.loc .t { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; min-width: 0; }

/* 可拖宽的那一格：手柄贴在格子右边界上，格子要能定位；不能 overflow: hidden，否则伸进间隙的半条手柄被裁掉 */
.head > span.rz { position: relative; overflow: visible; }

/*
  手柄与防火墙名单同一个样子：宽 7px（1px 的靶子点不中），跨在列与列的间隙里，
  hover 时显一条青线给反馈。
*/
.grip {
  position: absolute;
  top: 0;
  right: -9px;
  width: 7px;
  height: 100%;
  cursor: col-resize;
  z-index: 3;
}

.grip:hover::after {
  content: "";
  position: absolute;
  top: 4px;
  bottom: 4px;
  left: 3px;
  width: 1px;
  background: var(--cyan);
  box-shadow: 0 0 4px var(--cyan);
}
.num { color: var(--dim3); font-variant-numeric: tabular-nums; }
.dev { color: var(--muted); font-family: var(--mono); overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }
.dev.wpc { color: var(--green); }

.ip { color: var(--cyan); }

/* 三种原始尺寸（16×12 国旗 / 20×20 组织旗 / 16×16 本地），定方框 + contain 才不会被压扁 */
.flag { width: 16px; height: 16px; object-fit: contain; flex: none; }


/* ── 下半部：连接明细 ── */

/*
  按比例分，不是定高。

  【为什么不定高】原来写死 176px，窗口拉大时多出来的高度全给了上面那张表 ——
  而客户端通常只有几个（截图里就一行），连接却常有几十条，越拉大越不合理。
  现在占 40%，跟着窗口一起长。

  【为什么不按内容撑开】连接数每秒都在变，高度跟着内容跳会让上面的表一直上下抖。
  按比例算出来的高度只跟窗口有关，稳定 —— 这条约束仍然守着。

  min-height 兜住窗口很矮的情况：40% 再小也至少留得下四五行。
*/
.conn {
  flex: 0 0 40%;
  min-height: 176px;
  display: flex;
  flex-direction: column;
  border-top: 1px solid var(--border);
  background: var(--card);
}

.cbar {
  flex: none;
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 6px 14px;
  background: var(--panel);
  border-bottom: 1px solid var(--border);
}

.ctl {
  position: relative;
  top: 1px;   /* 实测偏高 1.3px（2026-09-13 字体度量覆写后按 100% 缩放实测重调） */
  font-family: var(--share);
  font-size: var(--fs-label);
  letter-spacing: .14em;
  text-transform: uppercase;
  color: var(--dim2);
}

.cip { font-family: var(--mono); font-size: var(--fs-body); color: var(--cyan); }

/*
  表头在这个容器<b>外面</b>：一出现滚动条，行的可用宽度就比表头少一个槽位，
  两边的 grid 各按各的宽度算，后面几列就错开（2026-09-09 两张表都撞过）。

  以前靠 scrollbar-gutter: stable 恒留槽位 + 表头写死同宽的右内边距；
  2026-09-15 起改成<b>不留槽位</b>、表头右内边距按 useScrollGutter 实测补 ——
  恒留槽位会让没有滚动条时的行停在槽位前，看着像没铺满（原因同上面的 .body）。
  滚动条出现 / 消失时内容框变宽变窄，ResizeObserver 触发重量，照样对得齐。
*/
.cbody {
  flex: 1;
  min-height: 0;
  overflow-y: auto;
  padding: 0 0 4px;
}

.empty.sm { padding: 26px 0; font-size: var(--fs-body); }

/* 一条连接一行：源端口 → 目标，后面跟协议与出口 */
.crow {
  display: grid;
  grid-template-columns: 48px 16px minmax(180px, 1.4fr) 84px minmax(140px, 1fr);
  align-items: center;
  gap: 10px;
  padding: 1px 14px;
  font-family: var(--mono);
  font-size: var(--fs-body);
  line-height: 1.85;
}

/* 表头与行共用同一份列宽，所以只写一次网格；这里只补表头自己的样子 */
.chead {
  display: grid;
  grid-template-columns: 48px 16px minmax(180px, 1.4fr) 84px minmax(140px, 1fr);
  align-items: center;
  gap: 10px;
  padding: 0 14px;
  height: var(--th-h);

  /*
    ⚠️ 配色与<b>上半部那张表的 .head 逐条一致</b>（--panel + 下边框）——
    两张表上下并排，表头长得不一样一眼就看得出来。

    ⚠️ <b>右内边距要补上滚动条占掉的宽度</b>：表头在 .cbody 外面，滚动条出现时不补就比行宽。
    补多少由模板按实测给（useScrollGutter，没有滚动条或覆盖式滚动条时是 0）；这里的 14 只是挂载前的兜底。
  */
  background: var(--panel);
  border-bottom: 1px solid var(--border);
  padding-right: 14px;
  /* 与全项目其它表头同一份（10px · .14em · var(--th-fg)），不因为是子表就小半号 */
  font-family: var(--share);
  font-size: var(--th-size);
  letter-spacing: .14em;
  text-transform: uppercase;
  color: var(--th-fg);
}

.chead > span:nth-child(4) { text-align: center; }

/*
  表头一律不折行。折行会把整条标签栏撑高一倍，而这一条紧贴着表格 ——
  高度一变，下面每一行都跟着往下挪。宁可溢出也不折。
*/
.chead > span { white-space: nowrap; overflow: hidden; }

.crow:hover { background: rgb(var(--tint-rgb) / 4%); }
.crow > span { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }

/* 表头默认就靠左，这里不必再写；等宽数字让端口位数不齐时也不会左右晃 */
.cport { color: var(--muted); font-variant-numeric: tabular-nums; }

/*
  箭头一直是在自己那一格里居中的（量过：左右各 5px）。看着偏右是因为
  <b>端口那一列太宽</b> —— 74px 而 ":10169" 只有 43px，靠左之后尾部空出 31px，
  于是箭头离端口 49px、离目标只有 15px。把列收到 48px（贴着内容），
  两边变成 23 / 15，看着才像"在中间"。
*/
.carrow { color: var(--dim); text-align: center; }
.ctarget { color: var(--bright); }
.csrv { color: var(--dim3); }

/* 与目标相同时只是一条短横，压暗，别让它看着像有内容 */
.csrv.same { color: var(--dim); }

/*
  协议：与封包列表的协议列同一套 —— 居中的普通文字，一个暗色，没有边框。

  原来画成了带边框的小标签，看着像个按钮（可点的东西），而它只是个只读的值；
  另外那五种按类型分色也去掉了：封包列表里只有「类型」那一格按取值变色
  （请求 / 响应是最值得一眼分辨的语义），协议本身不承担那个作用。
*/
.cproto { text-align: center; color: var(--muted); }

/* WPC 控制连接：绿色，与上半部「设备标识」那一格是 WPC 时同一个颜色 */
.ctarget.wpc,
.cproto.wpc { color: var(--green); }

/*
  两张表共用。
  表头每格与数据格同名（对齐规则靠这个），于是数据列的字体 / 字号 / 颜色
  （.notes 12px、.ad / .dt 等宽字、.cnt 青色……）会一并漏进表头，看着就是「备注」「数据」比别的表头大。
  这里按格子把它们收回来：表头只认表头自己那一份。(0,2,1) 压得过任何单类名的列规则。
*/
.head > span,
.chead > span {
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

/*
  上面那条 color: inherit 是 (0,2,1)，把 style.css 的 .so.on（0,2,0）盖掉了 ——
  排序中的那一列一直没点亮，只剩一个箭头。这里再抬一级把青色还回来。
*/
.head > span.so.on { color: var(--cyan); }
</style>
