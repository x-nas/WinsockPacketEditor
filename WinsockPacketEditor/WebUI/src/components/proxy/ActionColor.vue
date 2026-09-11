<script setup lang="ts">
/*
  改一组滤镜动作的配色。入口是代理数据页那条图例 —— 点哪个色块改哪个。

  【为什么不放进系统设置】挑颜色要看着它在表里的样子。
  隔着一个弹窗挑完、关掉、再回列表看效果，一轮要三步，不满意还得再来一轮；
  就地改则是"点开就在它旁边，改完当场看到"。

  【一次只提交这一组】桥那边是 saveActionColor(action, fore, back)。
  这个弹窗手里只有这一组的值，若按整表回写，就得先把另外三组取回来再原样送回去 ——
  中间任何一次遗漏都会把别处改过的颜色悄悄盖掉。
*/
import { computed, ref, watch } from 'vue'
import { call } from '../../bridge'
import { t, type Key } from '../../i18n'
import SettingsModal from './SettingsModal.vue'

/** 出厂值，与 ClassObject/Ui/UiPrefs.cs 的默认值逐值一致（改那边这里也要改）。 */
const RESET: Record<string, [string, string]> = {
  replace: ['#FFB0F8', '#4E1C56'],
  change: ['#F9D86F', '#4E3D0D'],
  intercept: ['#FFA5B8', '#511C2B'],
  display: ['#8AEAFF', '#0D4353'],
}

const LABEL: Record<string, Key> = {
  replace: 'proxy.act.replace',
  change: 'proxy.act.change',
  intercept: 'proxy.act.intercept',
  display: 'proxy.act.display',
}

const props = defineProps<{
  /** 要改哪一组；null = 不开 */
  action: string | null
  fore: string
  back: string
}>()

const emit = defineEmits<{
  (e: 'close'): void
  (e: 'saved'): void
}>()

//t() 只收键、不做插值，标题在这里拼
const title = computed(() =>
  t('set.colorTitle') + (props.action ? ' · ' + t(LABEL[props.action]) : ''))

const busy = ref(false)
const error = ref('')
const fore = ref('#000000')
const back = ref('#000000')

//每次打开都从当前实际配色起步，不留上一次没保存的改动
watch(() => props.action, (a) => {
  if (!a) return

  error.value = ''
  fore.value = props.fore
  back.value = props.back
})

/*
  <input type="color"> 只认小写 "#rrggbb"，而 C# 的 RgbColor.Hex 出的是大写。
  大小写它一律照收，但回读永远给小写 —— 不统一的话每次打开都像"值自己变了"。
*/
function norm(v: string): string {
  return (v || '#000000').toLowerCase()
}

function reset(): void {
  const d = RESET[props.action || '']
  if (!d) return

  fore.value = d[0]
  back.value = d[1]
}

async function save(): Promise<void> {
  busy.value = true
  error.value = ''

  try {
    const r = await call<{ ok: boolean; error: string }>('saveActionColor', {
      action: props.action,
      fore: fore.value,
      back: back.value,
    })

    if (!r?.ok) {
      error.value = r?.error || ''
      return
    }

    emit('saved')
    emit('close')
  } catch (e) {
    console.error('[color] 保存动作配色失败', e)
    error.value = String(e)
  } finally {
    busy.value = false
  }
}
</script>

<template>
  <SettingsModal
    :open="!!props.action"
    :title="title"
    subtitle="Action Colors"
    :busy="busy"
    :error="error"
    @update:open="!$event && emit('close')"
    @save="save"
  >    <div class="setf" style="--setf-k: 72px">

    <!--
      预览就是这一行在封包列表里的样子 —— 两个色块并排看不出
      「这两个颜色配在一起是什么样」，而那才是要挑的东西。
    -->
    <div class="prevbox">
      <div class="prev" :style="{ color: fore, background: back }">
        <span>0001</span><span>14:45:44</span><span>TCP {{ t('set.colorPrevType') }}</span>
        <span>10.10.10.8:60174</span><span>middledata.example.com:443</span>
      </div>
    </div>

    <div class="row">
      <div class="k">{{ t('set.foreColor') }}</div>
      <div class="v">
        <input type="color" :value="norm(fore)"
               @input="fore = ($event.target as HTMLInputElement).value">
        <span class="hexv">{{ fore.toUpperCase() }}</span>
      </div>
    </div>

    <div class="row">
      <div class="k">{{ t('set.backColor') }}</div>
      <div class="v">
        <input type="color" :value="norm(back)"
               @input="back = ($event.target as HTMLInputElement).value">
        <span class="hexv">{{ back.toUpperCase() }}</span>
        <!-- 琥珀色：还原会把改了的两个色一起扔掉，是个提醒级动作，与仓库列表的「自动入库」同一套 -->
        <button class="mini warn" @click="reset">{{ t('set.resetColor') }}</button>
      </div>
    </div>
    </div>
  </SettingsModal>
</template>

<style scoped>
.prevbox { padding: 14px 20px 6px; }

/* 列宽比着封包列表那几列给，看着才像真的一行 */
.prev {
  display: grid;
  grid-template-columns: 52px 92px 74px 1fr 1fr;
  gap: 10px;
  align-items: center;
  padding: 4px 8px;
  font-family: var(--mono);
  font-size: var(--fs-body);
}

.prev > span { overflow: hidden; text-overflow: ellipsis; white-space: nowrap; }

.hexv {
  font-family: var(--mono);
  font-size: var(--fs-body);
  color: var(--dim2);
  font-variant-numeric: tabular-nums;
}

/*
  原生取色器。各浏览器的默认外框差别很大（内边距、圆角、边框都不一样），
  所以把它清掉，只留一个我们自己画的方块。
*/
input[type="color"] {
  width: 44px;
  height: 24px;
  padding: 0;
  border: 1px solid var(--border);
  background: transparent;
  cursor: pointer;
  flex: none;
}

input[type="color"]::-webkit-color-swatch-wrapper { padding: 2px; }
input[type="color"]::-webkit-color-swatch { border: 0; }

/* 小按钮在 style.css 的 .mini，这里只补布局 */
.mini { margin-left: auto; }

</style>
