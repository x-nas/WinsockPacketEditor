<script setup lang="ts">
/*
  列表设置 —— 对应 WinForms 的 Controls/ListSetting。

  【比 WinForms 多一样】那边只管列显隐，自动清理是 ProxyList 工具条上的两个控件。
  这里并进同一个弹窗 —— 它们讲的是同一件事：这张表怎么显示、留多少。
  工具条上仍然显示当前值，只是改要到这里来。

  【比 WinForms 少三样】Id / 时间 / 数据不给关：
  Id 是取字节的钥匙（getPacketDetail 靠它），时间和数据是这张表的意义所在，
  关掉等于把列表变成一堆地址。WinForms 允许关，那是历史遗留，不照搬。

  ⚠️ 【列显隐按模式分两套】C# 侧 ProxyConfig.List.IsShow_*（代理，落 ProxyMode 表）与
  PacketConfig.List.IsShow_*（注入，落 InjectMode 表）是各自独立的十个字段。
  这个弹窗两种模式共用，靠 mode 决定读写哪一套 —— 不传就是代理。
  早先没分流，在注入模式关掉一列会把代理那张表的同名列也关掉。

  自动清理相反，它<b>本来就只有一套</b>（PacketConfig.List.AutoClear，代理列表沿用它），
  所以不跟着 mode 走。
*/
import { ref, watch } from 'vue'
import { call } from '../../bridge'
import { t, type Key } from '../../i18n'
import { listSetting } from '../../stores/runtime'
import SettingsModal from './SettingsModal.vue'

const props = withDefaults(defineProps<{ open: boolean; mode?: 'proxy' | 'inject' }>(), { mode: 'proxy' })
const emit = defineEmits<{ (e: 'update:open', v: boolean): void }>()

const busy = ref(false)
const error = ref('')

/** 可关的七列。顺序与表里一致，扫下来能对上。 */
const COLS: Array<{ key: keyof typeof form.value; label: Key }> = [
  { key: 'showSocket', label: 'col.socket' },
  { key: 'showType', label: 'col.type' },
  { key: 'showClientAddr', label: 'col.client' },
  { key: 'showClientLoc', label: 'col.clientLoc' },
  { key: 'showServerAddr', label: 'col.server' },
  { key: 'showServerLoc', label: 'col.serverLoc' },
  { key: 'showLen', label: 'col.len' },
]

/*
  ⚠️ <b>这一屏只管列显隐了</b>（2026-09-07）。

  自动清理搬去了数据页的工具条 —— 「设置摆在哪儿就代表它管哪张表」：
  日志那份一直在日志页的工具条上，而这一份原先藏在弹窗里，
  两个长得一模一样的「自动清理」谁都会以为是同一个（真被问过）。
  搬完之后这一屏与 WinForms 的「列表设置」一致，那边本来也只管列显隐。
*/
const form = ref({
  showSocket: true,
  showType: true,
  showClientAddr: true,
  showClientLoc: true,
  showServerAddr: true,
  showServerLoc: true,
  showLen: true,
})

watch(() => props.open, async (on) => {
  if (!on) return

  error.value = ''
  try {
    /*
      ⚠️ <b>只挑这七列，别把整个应答直接赋给 form</b>。
      getListSetting 仍然带着 autoClear / autoClearValue（数据页的工具条要拿初值），
      整个赋过来的话它们会混进 form，保存时又被原样推回共享状态，
      在这一屏留下两个没人显示、却会被写回去的字段。
    */
    const s = await call<any>('getListSetting', { mode: props.mode })
    for (const c of COLS) form.value[c.key] = !!s?.[c.key]
  } catch (e) {
    console.error('[set] 读取列表设置失败', e)
  }
})

async function save(): Promise<void> {
  busy.value = true
  error.value = ''

  try {
    const r = await call<any>('saveListSetting', { ...form.value, mode: props.mode })

    if (!r?.ok) {
      error.value = r?.error || ''
      return
    }

    /*
      列表要立刻按新的列显隐重画，所以把结果写进共享状态。
      ⚠️ 是<b>合并</b>不是整体替换 —— 那份状态里还有自动清理两项（工具条在用），
      整体替换会把它们抹成 undefined，工具条的勾选框当场变成未勾。
    */
    listSetting.value = { ...(listSetting.value as any), ...form.value }
    emit('update:open', false)
  } catch (e) {
    console.error('[set] 保存列表设置失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}
</script>

<template>
  <SettingsModal
    :open="props.open"
    :title="t('set.list')"
    subtitle="Packet Columns"
    :busy="busy"
    :error="error"
    @update:open="emit('update:open', $event)"
    @save="save"
  >    <div class="setf" style="--setf-k: 132px">

    <section class="sec">
    <div class="grp">{{ t('set.grp.cols') }}</div>
    <p class="hint">{{ t('set.colsHint') }}</p>

    <div class="cols">
      <button
        v-for="c in COLS"
        :key="c.key"
        class="chk"
        :class="{ on: form[c.key] }"
        @click="(form[c.key] as boolean) = !form[c.key]"
      ><i />{{ t(c.label) }}</button>
    </div>

    </section>
    </div>
  </SettingsModal>
</template>

<style scoped>

/* 七个列开关排成两列，比一行一个紧凑得多 */
.cols {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 8px 16px;
  padding: 0 20px 4px;
}

</style>
