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
import SettingsModal from './SettingsModal.vue'

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
const PROXY_KEYS: Array<keyof Form> = ['tcpReq', 'tcpResp', 'udpReq', 'udpResp', 'unpack', 'unpackHead', 'unpackLength']

/** 有入口被关掉时才提示 —— 那是「看不到数据」的头号原因。 */
const anyInjectOff = computed(() => INJECT_KEYS.some((k) => !form.value[k]))

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
    @update:open="emit('update:open', $event)"
    @save="save"
  >    <div class="setf" style="--setf-k: 152px">

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

    <div class="row">
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

    <div class="row">
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

    <p class="hint">{{ t('set.hook.dirHint') }}</p>
    <!-- 关掉的方向连滤镜都不跑，这条比"少了几行"要紧，单独标出来 -->
    <p class="warn" v-if="!form.tcpReq || !form.tcpResp || !form.udpReq || !form.udpResp">
      {{ t('set.hook.offWarn') }}
    </p>
    <p class="hint">{{ t('set.hook.runOnly') }}</p>
    </section>

    <section class="sec">
    <div class="grp">{{ t('set.grp.unpack') }}</div>

    <div class="row">
      <div class="k">{{ t('set.hook.unpack') }}</div>
      <div class="v">
        <button class="chk" :class="{ on: form.unpack }" @click="form.unpack = !form.unpack">
          <i />{{ t('set.speedModeOn') }}
        </button>
      </div>
    </div>

    <p class="hint">{{ t('set.hook.unpackHint') }}</p>

    <div class="row">
      <div class="k">{{ t('set.hook.head') }}</div>
      <div class="v">
        <input v-model="form.unpackHead" class="inp" spellcheck="false"
               :disabled="!form.unpack" :placeholder="t('set.hook.headPh')">
      </div>
    </div>

    <div class="row">
      <div class="k">{{ t('set.hook.length') }}</div>
      <div class="v">
        <input v-model="form.unpackLength" class="inp num" spellcheck="false"
               :disabled="!form.unpack" :placeholder="t('set.hook.lengthPh')">
        <span class="tip">{{ t('set.hook.lengthHint') }}</span>
      </div>
    </div>
    </section>
    </template>
    </div>
  </SettingsModal>
</template>

<style scoped>

/* 有方向被关掉时才出现 —— 那是「看不到数据」的头号原因 */
.warn { padding: 0 20px; margin: 2px 0 4px; font-size: var(--fs-small); color: var(--amber); }

.tip { font-size: var(--fs-small); color: var(--dim2); }


</style>
