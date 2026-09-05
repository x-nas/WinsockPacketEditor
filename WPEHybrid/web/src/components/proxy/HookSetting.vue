<script setup lang="ts">
/*
  拦截设置 —— 对应 WinForms 的 Controls/HookSetting。

  两段：抓取方向（TCP / UDP 的请求与响应）+ 拆包。

  【只出代理那四个】WinForms 是 tabHookSettings 按宿主窗体选页
  （HookSetting 26–33），注入模式那 12 个 WinSock 钩子（Send / SendTo / Recv /
  RecvFrom × WS1.1 / WS2.0 / WSA）在另一页。与滤镜编辑的「作用域」同一个道理：
  代理模式的包不走那些钩子，摆出来只会是永远不起作用的开关。

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
import { ref, watch } from 'vue'
import { call } from '../../bridge'
import { t } from '../../i18n'
import SettingsModal from './SettingsModal.vue'

const props = defineProps<{ open: boolean }>()
const emit = defineEmits<{ (e: 'update:open', v: boolean): void }>()

const busy = ref(false)
const error = ref('')

interface Form {
  tcpReq: boolean
  tcpResp: boolean
  udpReq: boolean
  udpResp: boolean
  unpack: boolean
  unpackHead: string
  unpackLength: string
}

const form = ref<Form>({
  tcpReq: true,
  tcpResp: true,
  udpReq: true,
  udpResp: true,
  unpack: false,
  unpackHead: '01 00 00',
  unpackLength: '4-5',
})

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
    const r = await call<{ ok: boolean; error: string }>('saveHookSetting', { ...form.value })

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
    subtitle="Controls/HookSetting"
    :busy="busy"
    :error="error"
    @update:open="emit('update:open', $event)"
    @save="save"
  >    <div class="setf" style="--setf-k: 152px">

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
    </div>
  </SettingsModal>
</template>

<style scoped>

/* 有方向被关掉时才出现 —— 那是「看不到数据」的头号原因 */
.warn { padding: 0 20px; margin: 2px 0 4px; font-size: 11.5px; color: var(--amber); }

.tip { font-size: 11.5px; color: #8a94a6; }

</style>
