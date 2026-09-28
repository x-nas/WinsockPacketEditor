<script setup lang="ts">
/*
  代理设置 —— 对应 WinForms 的 Controls/ProxySetting。

  字段与那边逐项对齐：监听地址 / SOCKS5 / 身份认证 / 最大连接数 / HTTP / 系统代理 / 导出证书。

  【两处与 WinForms 不同，都是有意的】
  ① 校验在 C# 侧做（必须启用 SOCKS5、两个端口不能相同）。那是服务能不能起来的
     业务约束，前端再写一份就会有两套真相；这里只负责把错误显示出来。
  ② 保存时<b>顺带落库</b>。WinForms 靠关窗时统一 SaveProxyMode_ToDB，
     外壳没有那个时机，不落库的话改完端口重启就白改了。

  【系统代理开关立即生效，不等保存】与 WinForms 一致 ——
  它改的是注册表里的 Internet 设置，不是本程序的配置。
*/
import { computed, ref, watch } from 'vue'
import { call } from '../../bridge'
import { t } from '../../i18n'
import { socks5Addr } from '../../stores/runtime'
import CyberSelect from '../CyberSelect.vue'
import SettingsModal from './SettingsModal.vue'

const props = defineProps<{ open: boolean }>()
const emit = defineEmits<{ (e: 'update:open', v: boolean): void }>()

interface Setting {
  proxyIpAuto: boolean
  proxyIp: string
  localIps: string[]
  enableSocks5: boolean
  socks5Port: number
  enableAuth: boolean
  /** 只允许 WPC 客户端连接：普通 SOCKS5 客户端即使账号密码正确也被拒；要求 enableAuth */
  onlyWpc: boolean
  maxConnection: number
  /** 本机允许的最大连接数上限（= 可预留内存 ÷ 每连接缓冲），由 C# 按物理内存算 */
  maxConnectionCap: number
  maxConnectionDefault: number
  /** 每个连接预留多少 KB（SuperSocket 启动时按 连接数 × 这个值 一次性分配） */
  connBufferKB: number
  memoryGB: number
  enableSystemProxy: boolean
  running: boolean
}

interface HttpsCertificateStatus {
  Exists: boolean
  Trusted: boolean
  Subject: string
  Thumbprint: string
  Error: string
}

const s = ref<Setting | null>(null)
const httpsCert = ref<HttpsCertificateStatus | null>(null)
const certificateNote = ref('')
const certificateExportFormat = ref('cer')
const certificateExportFormats = computed(() => [
  { value: 'cer', label: t('cert.formatCer') }, { value: 'crt', label: t('cert.formatCrt') },
  { value: 'der', label: t('cert.formatDer') }, { value: 'pem', label: t('cert.formatPem') },
  { value: 'android', label: t('cert.formatAndroid') },
])
const busy = ref(false)
const error = ref('')

async function loadHttpsCertificate(): Promise<void> {
  try { httpsCert.value = await call<HttpsCertificateStatus>('getHttpsMappingCertificate') }
  catch (e) { console.error('[https-cert] 读取证书状态失败', e) }
}

/*
  最大连接数不是随手填的数字：服务启动时按「连接数 × 每连接缓冲」一次性预留内存。
  这里把估算与本机上限直接写在输入框旁边，超了保存时 C# 会拒绝（ProxySettingsForm.MaxConnection.Error）。
*/
const maxConnHint = computed(() => {
  if (!s.value) return ''
  const n = Number(s.value.maxConnection) || 0
  const mb = Math.round((n * s.value.connBufferKB) / 1024)
  return t('set.maxConnHint')
    .replace('{kb}', String(s.value.connBufferKB))
    .replace('{mb}', String(mb))
    .replace('{gb}', String(s.value.memoryGB))
    .replace('{cap}', String(s.value.maxConnectionCap))
})

watch(() => props.open, async (on) => {
  if (!on) return

  error.value = ''
  try {
    s.value = await call<Setting>('getProxySetting')
    await loadHttpsCertificate()
  } catch (e) {
    console.error('[set] 读取代理设置失败', e)
  }
})

/*
  服务在跑的时候不让改监听相关的项。

  改了也不会生效（SuperSocket 的 Setup 只在启动时读一次），
  而界面显示新值会让人以为已经换了端口 —— 那比不让改糟得多。
*/
const locked = computed(() => !!s.value?.running)

async function save(): Promise<void> {
  if (!s.value) return

  busy.value = true
  error.value = ''

  try {
    const r = await call<any>('saveProxySetting', {
      proxyIpAuto: s.value.proxyIpAuto,
      proxyIp: s.value.proxyIp,
      enableSocks5: s.value.enableSocks5,
      socks5Port: Number(s.value.socks5Port),
      enableAuth: s.value.enableAuth,
      onlyWpc: s.value.enableAuth && s.value.onlyWpc,
      maxConnection: Number(s.value.maxConnection),
    })

    if (!r?.ok) {
      error.value = r?.error || ''
      return
    }

    // 监听地址可能变了，状态栏与运行状态条都在读它
    if (r.socks5Addr) socks5Addr.value = r.socks5Addr

    emit('update:open', false)
  } catch (e) {
    console.error('[set] 保存代理设置失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}

async function toggleSystemProxy(): Promise<void> {
  if (!s.value) return

  const next = !s.value.enableSystemProxy
  try {
    const r = await call<any>('setSystemProxy', { enable: next })
    s.value.enableSystemProxy = !!r?.enabled
  } catch (e) {
    console.error('[set] 切换系统代理失败', e)
  }
}

async function certificateAction(action: 'create' | 'trust' | 'untrust' | 'export' | 'delete'): Promise<void> {
  const methods = {
    create: 'createHttpsMappingCertificate', trust: 'trustHttpsMappingCertificate', untrust: 'untrustHttpsMappingCertificate',
    export: 'exportHttpsMappingCertificate', delete: 'deleteHttpsMappingCertificate',
  }
  try {
    certificateNote.value = ''
    const r = await call<any>(methods[action], action === 'export' ? { format: certificateExportFormat.value } : {})
    if (!r?.ok && !r?.cancelled) error.value = r?.error || t('cert.error')
    else if (action === 'export' && r?.path) certificateNote.value = t('cert.exported').replace('{0}', r.path)
    await loadHttpsCertificate()
  } catch (e) {
    console.error('[https-cert] 证书操作失败', e)
    error.value = String(e)
  }
}

</script>

<template>
  <SettingsModal
    :open="props.open"
    :title="t('set.proxy')"
    subtitle="Proxy Listener"
    :busy="busy"
    :error="error"
    @update:open="emit('update:open', $event)"
    @save="save"
  >    <div class="setf" style="--setf-k: 132px">

    <template v-if="s">
      <p v-if="locked" class="lock">{{ t('set.lockedHint') }}</p>

      <!-- 监听地址 -->
      <section class="sec">
      <div class="grp">{{ t('set.grp.addr') }}</div>

      <div class="row">
        <div class="k">{{ t('set.autoIp') }}</div>
        <div class="v">
          <button class="chk" :class="{ on: s.proxyIpAuto }" :disabled="locked"
                  @click="s.proxyIpAuto = !s.proxyIpAuto"><i />{{ t('set.autoDetect') }}</button>
        </div>
      </div>

      <div class="row">
        <div class="k">{{ t('set.appointIp') }}</div>
        <div class="v">
          <CyberSelect v-model="s.proxyIp" class="sel" :options="s.localIps.map((ip) => ({ value: ip, label: ip }))" :disabled="s.proxyIpAuto || locked" />
        </div>
      </div>

      </section>
      <!-- SOCKS5 -->
      <section class="sec">
      <div class="grp">{{ t('set.grp.socks') }}</div>

      <div class="row">
        <div class="k">{{ t('set.enableSocks') }}</div>
        <div class="v">
          <button class="chk" :class="{ on: s.enableSocks5 }" :disabled="locked"
                  @click="s.enableSocks5 = !s.enableSocks5"><i />SOCKS5</button>
          <span class="tip">{{ t('set.socksRequired') }}</span>
        </div>
      </div>

      <div class="row">
        <div class="k">{{ t('set.socksPort') }}</div>
        <div class="v">
          <input v-model.number="s.socks5Port" class="inp num" type="number" min="1" max="65535" :disabled="!s.enableSocks5 || locked">
        </div>
      </div>

      <!--
        身份认证。开着时客户端必须提供代理账号的用户名密码才能连上；
        账号在「账号列表」页管理。
      -->
      <div class="row">
        <div class="k">{{ t('set.enableAuth') }}</div>
        <div class="v">
          <button class="chk" :class="{ on: s.enableAuth }" :disabled="locked"
                  @click="s.enableAuth = !s.enableAuth"><i />{{ t('set.authUserPass') }}</button>
          <span class="tip warn">{{ s.enableAuth ? t('set.authOnHint') : t('set.authOffHint') }}</span>
        </div>
      </div>

      <!--
        只允许 WPC 客户端。WPC 经 SOCKS5 端口上的私有方法注册设备后拿令牌连接，
        普通 SOCKS5 客户端拿着正确的账号密码也进不来。它依赖账号认证，认证关着时置灰。
      -->
      <div class="row">
        <div class="k">{{ t('set.onlyWpc') }}</div>
        <div class="v">
          <button class="chk" :class="{ on: s.enableAuth && s.onlyWpc }" :disabled="!s.enableAuth || locked"
                  @click="s.onlyWpc = !s.onlyWpc"><i />WPC</button>
          <span class="tip">{{ t('set.onlyWpcHint') }}</span>
        </div>
      </div>

      <div class="row">
        <div class="k">{{ t('set.maxConn') }}</div>
        <div class="v">
          <input v-model.number="s.maxConnection" class="inp num" type="number" min="1" :max="s.maxConnectionCap" :disabled="locked">
          <span class="tip" :class="{ warn: Number(s.maxConnection) > s.maxConnectionCap }">{{ maxConnHint }}</span>
        </div>
      </div>

      </section>
      <!-- 系统代理 -->
      <section class="sec">
      <div class="grp">{{ t('set.grp.system') }}</div>

      <div class="row">
        <div class="k">{{ t('set.systemProxy') }}</div>
        <div class="v">
          <button class="sw" :class="{ on: s.enableSystemProxy }" role="switch"
                  :aria-checked="s.enableSystemProxy" @click="toggleSystemProxy"><i /></button>
          <span class="tip">{{ t('set.systemProxyHint') }}</span>
        </div>
      </div>

      </section>

      <!-- HTTPS 映射根证书。是否尝试 MITM 由已启用的 HTTPS 本地映射规则决定，不设独立开关。 -->
      <section class="sec https-cert">
      <div class="grp">{{ t('cert.title') }}</div>
      <div class="row">
        <div class="k">{{ t('cert.root') }}</div>
        <div class="v cert-actions">
          <template v-if="httpsCert?.Exists">
            <span class="tip" :class="{ warn: !httpsCert.Trusted }">{{ httpsCert.Trusted ? t('cert.trusted') : t('cert.untrusted') }}</span>
            <button v-if="!httpsCert.Trusted" class="sbtn primary" @click="certificateAction('trust')">{{ t('cert.trust') }}</button>
            <button v-else class="sbtn" @click="certificateAction('untrust')">{{ t('cert.untrust') }}</button>
            <button class="sbtn danger" @click="certificateAction('delete')">{{ t('cert.delete') }}</button>
          </template>
          <template v-else>
            <span class="tip warn">{{ t('cert.createHint') }}</span>
            <button class="sbtn primary" @click="certificateAction('create')">{{ t('cert.create') }}</button>
          </template>
        </div>
      </div>
      <div v-if="httpsCert?.Exists" class="row">
        <div class="k">{{ t('cert.format') }}</div>
        <div class="v cert-actions">
          <CyberSelect v-model="certificateExportFormat" class="cert-format" :options="certificateExportFormats" />
          <button class="sbtn" @click="certificateAction('export')">{{ t('cert.export') }}</button>
        </div>
      </div>
      <div v-if="httpsCert?.Error" class="row"><div class="k">状态</div><div class="v"><span class="tip warn">{{ httpsCert.Error }}</span></div></div>
      <div v-if="certificateNote" class="row"><div class="k">{{ t('cert.result') }}</div><div class="v"><span class="tip">{{ certificateNote }}</span></div></div>
      <p class="hint">{{ t('cert.hint') }}</p>
      </section>
    </template>

    <div v-else class="loading">{{ t('hex.loading') }}</div>
    </div>
  </SettingsModal>
</template>

<style scoped>
.loading { padding: 40px 0; text-align: center; color: var(--muted); font-size: var(--fs-body); }

.lock {
  margin: 10px 20px 4px;
  padding: 8px 12px;
  border: 1px solid rgb(var(--amber-rgb) / 32%);
  background: rgb(var(--amber-rgb) / 7%);
  font-size: var(--fs-small);
  color: var(--amber);
}

/*
  ⚠️ 这行提示<b>要换行，不能省略号</b>。中文写得下的一句，换成俄语 / 越南语常常长一倍
  （「100–500000. При достижении очищается весь список, а не только старые строки」），
  nowrap + ellipsis 会把后半句直接吃掉，而那半句正是要紧的部分。
  外面的 .setf .row > .v 已经是 flex-wrap: wrap，让它自己折下去即可。
*/
.tip {
  font-size: var(--fs-small);
  color: var(--dim);
  min-width: 0;
  line-height: 1.5;
}
.tip.warn { color: var(--amber); }

/* 与其它设置下拉共用 CyberSelect，仅限定证书格式这列的可读宽度。 */
.cert-format { width: 150px; }

/* 下拉是自绘的 CyberSelect，这里只给宽度 */
.sel { min-width: 190px; }

/* 开关：系统代理是即时生效的，用滑动开关而不是勾选框，形态上就与「要按保存」的项区分开 */
.sw {
  width: 38px;
  height: 20px;
  padding: 0;
  border: 1px solid var(--border);
  background: rgb(var(--inset-rgb) / 30%);
  cursor: pointer;
  position: relative;
  transition: .15s;
}

.sw i { position: absolute; top: 2px; left: 2px; width: 14px; height: 14px; background: var(--muted); transition: .15s; }
.sw.on { border-color: var(--green); background: rgb(var(--green-rgb) / 15%); }
.sw.on i { left: 20px; background: var(--green); box-shadow: 0 0 6px var(--green); }
.sw:focus-visible { outline-offset: 2px; }

/* 小按钮的样式在 style.css 的 .mini */

</style>
