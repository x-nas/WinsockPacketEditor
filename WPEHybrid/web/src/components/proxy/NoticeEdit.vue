<script setup lang="ts">
/*
  ProxyCap 公告的新增 / 编辑 —— 对应 WinForms 的 Controls/NoticeEdit。
  五种类型照那边的下拉；发布时间由 C# 取当下（编辑一次就刷新一次，WinForms 也是这样）。
*/
import { computed, ref, watch } from 'vue'
import { call } from '../../bridge'
import type { NoticeRow } from '../../bridge/types'
import { t } from '../../i18n'
import CyberSelect from '../CyberSelect.vue'
import SettingsModal from './SettingsModal.vue'

const props = defineProps<{ target: NoticeRow | null | 'add' }>()
const emit = defineEmits<{ (e: 'close'): void }>()

const busy = ref(false)
const error = ref('')
const f = ref({ type: 1, title: '', content: '', more: '' })

const isAdd = computed(() => props.target === 'add')
const title = computed(() => t('wpc.notices') + ' · ' + t(isAdd.value ? 'fw.add' : 'fw.edit'))

const typeOptions = computed(() => [1, 2, 3, 4, 5].map((n) => ({ value: n, label: t(('wpc.nt' + n) as 'wpc.nt1') })))

watch(() => props.target, (v) => {
  if (!v) return
  error.value = ''
  if (v === 'add') { f.value = { type: 1, title: '', content: '', more: '' }; return }
  f.value = { type: v.Type, title: v.Title, content: v.Content, more: v.More }
})

async function save(): Promise<void> {
  busy.value = true
  error.value = ''
  try {
    const r = await call<{ error: string }>('saveNotice', { id: isAdd.value ? '' : (props.target as NoticeRow).Id, ...f.value })
    if (r?.error) { error.value = r.error; return }
    emit('close')
  } catch (e) {
    console.error('[wpc] 保存公告失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}
</script>

<template>
  <SettingsModal :open="!!props.target" :title="title" subtitle="Client Notice" :busy="busy" :error="error"
                 @update:open="!$event && emit('close')" @save="save">
    <div class="setf">
      <div class="row">
        <div class="k">{{ t('wpc.noticeType') }}</div>
        <div class="v"><CyberSelect v-model="f.type" :options="typeOptions" class="sel" /></div>
      </div>
      <div class="row">
        <div class="k">{{ t('wpc.noticeTitle') }}</div>
        <div class="v"><input v-model="f.title" class="inp" spellcheck="false" :placeholder="t('wpc.noticeTitlePh')"></div>
      </div>
      <div class="row top">
        <div class="k">{{ t('wpc.noticeContent') }}</div>
        <div class="v"><textarea v-model="f.content" class="inp" spellcheck="false" :placeholder="t('wpc.noticeContentPh')" /></div>
      </div>
      <div class="row">
        <div class="k">{{ t('wpc.noticeMore') }}</div>
        <div class="v"><input v-model="f.more" class="inp" spellcheck="false" placeholder="https://"></div>
      </div>
      <p class="hint">{{ t('wpc.noticeTimeHint') }}</p>
    </div>
  </SettingsModal>
</template>

<style scoped>
.sel { width: 200px; }
.row.top { align-items: start; }
.row.top > .k { padding-top: 6px; }
</style>
