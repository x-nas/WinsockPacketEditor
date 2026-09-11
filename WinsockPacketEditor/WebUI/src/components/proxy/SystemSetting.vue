<script setup lang="ts">
/*
  系统设置 —— 对应 WinForms 的 Controls/SystemSetting。

  三段：极速模式 / 列表执行模式 / 滤镜执行模式。

  【少了「悬浮按钮」那一项】WinForms 那边是 AntdUI 的 FormFloatButton，
  挂在窗体角上给 GitHub 与官网两个快捷入口。外壳没有这个东西 ——
  页脚已经有那两个链接，放个开关在这儿只会是拨了不动的假开关。
  配置本身没删，注入模式那套 UI 还在用。

  【四组配色不在这里】挪到代理数据页那条图例上，点色块就地改 ——
  挑颜色要看着它在表里的样子，隔着一个弹窗挑完再回去看是反的。

  【两个「执行模式」讲的不是一回事，别混】
    列表执行模式  SystemConfig.ListExecute   —— 发送 / 机器人列表里多条同时启用时，
                                               是一起跑还是一条跑完再跑下一条
    滤镜执行模式  FilterConfig.Filter.FilterExecute —— 一个封包命中多条滤镜时，
                                               只认第一条还是逐条叠加
  两者的枚举第二项都叫 Sequence，很容易看串。
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
  speedMode: boolean
  listExecute: number
  filterExecute: number
}

const form = ref<Form>({ speedMode: false, listExecute: 0, filterExecute: 0 })

watch(() => props.open, async (on) => {
  if (!on) return

  error.value = ''

  try {
    form.value = await call<Form>('getSystemSetting')
  } catch (e) {
    console.error('[set] 读取系统设置失败', e)
  }
})

async function save(): Promise<void> {
  busy.value = true
  error.value = ''

  try {
    const r = await call<{ ok: boolean; error: string }>('saveSystemSetting', { ...form.value })

    if (!r?.ok) {
      error.value = r?.error || ''
      return
    }

    emit('update:open', false)
  } catch (e) {
    console.error('[set] 保存系统设置失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}
</script>

<template>
  <SettingsModal
    :open="props.open"
    :title="t('set.system')"
    subtitle="Work Mode · Execution"
    :busy="busy"
    :error="error"
    @update:open="emit('update:open', $event)"
    @save="save"
  >    <div class="setf" style="--setf-k: 152px">

    <section class="sec">
    <div class="grp">{{ t('set.grp.workMode') }}</div>

    <div class="row">
      <div class="k">{{ t('set.speedMode') }}</div>
      <div class="v">
        <button class="chk" :class="{ on: form.speedMode }" @click="form.speedMode = !form.speedMode">
          <i />{{ t('set.speedModeOn') }}
        </button>
      </div>
    </div>

    <p class="hint">{{ t('set.speedModeHint') }}</p>
    </section>

    <section class="sec">
    <div class="grp">{{ t('set.grp.listExecute') }}</div>

    <div class="row">
      <div class="k">{{ t('set.listExecute') }}</div>
      <div class="v">
        <button class="rd" :class="{ on: form.listExecute === 0 }" @click="form.listExecute = 0">
          <i />{{ t('set.exec.together') }}
        </button>
        <button class="rd" :class="{ on: form.listExecute === 1 }" @click="form.listExecute = 1">
          <i />{{ t('set.exec.sequence') }}
        </button>
      </div>
    </div>

    <p class="hint">
      {{ form.listExecute === 0 ? t('set.exec.togetherHint') : t('set.exec.sequenceHint') }}
    </p>
    </section>

    <section class="sec">
    <div class="grp">{{ t('set.grp.filterExecute') }}</div>

    <div class="row">
      <div class="k">{{ t('set.filterExecute') }}</div>
      <div class="v">
        <button class="rd" :class="{ on: form.filterExecute === 0 }" @click="form.filterExecute = 0">
          <i />{{ t('set.exec.priority') }}
        </button>
        <button class="rd" :class="{ on: form.filterExecute === 1 }" @click="form.filterExecute = 1">
          <i />{{ t('set.exec.fseq') }}
        </button>
      </div>
    </div>

    <p class="hint">
      {{ form.filterExecute === 0 ? t('set.exec.priorityHint') : t('set.exec.fseqHint') }}
    </p>
    </section>
    </div>
  </SettingsModal>
</template>

<style scoped>

</style>
