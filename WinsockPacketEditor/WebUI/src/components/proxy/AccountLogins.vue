<script setup lang="ts">
/*
  账号登录情况 —— 对应 WinForms 的 Controls/AccountLocation。

  一个只读的明细表：这个账号从哪些 IP 登录过、什么时候、归属地是哪。
  数据是 AccountInfo.AIPInfo 这份嵌套列表，按 B9 的规则不进推送流
  （AccountRow 只留 LoginCount），所以打开时按账号 Id 单独取一次。
*/
import { ref, watch } from 'vue'
import { call } from '../../bridge'
import type { AccountRow } from '../../bridge/types'
import { flagSrc } from '../../flags'
import { t } from '../../i18n'
import SettingsModal from './SettingsModal.vue'

const props = defineProps<{ row: AccountRow | null }>()
const emit = defineEmits<{ (e: 'close'): void }>()

interface LoginRow {
  LoginTime: string
  LoginIP: string
  IPLocation: string
}

const rows = ref<LoginRow[]>([])
const busy = ref(false)

watch(() => props.row, async (r) => {
  if (!r) return

  rows.value = []
  busy.value = true

  try {
    const d = await call<{ rows: LoginRow[] }>('getAccountLogins', { id: r.Id })
    rows.value = d?.rows || []
  } catch (e) {
    console.error('[acct] 读取登录记录失败', e)
  } finally {
    busy.value = false
  }
})
</script>

<template>
  <SettingsModal
    :open="props.row !== null"
    :title="t('acct.lg.title')"
    :subtitle="props.row ? props.row.UserName : ''"
    :busy="busy"
    readonly
    @update:open="emit('close')"
  >
    <div class="tbl">
      <div class="head">
        <span>{{ t('acct.lg.time') }}</span><span>{{ t('acct.lg.ip') }}</span><span>{{ t('acct.lg.loc') }}</span>
      </div>

      <div v-if="!busy && !rows.length" class="empty">{{ t('acct.lg.empty') }}</div>

      <div v-for="(x, i) in rows" :key="i" class="row">
        <span class="tm">{{ x.LoginTime }}</span>
        <span class="ip">{{ x.LoginIP }}</span>
        <span class="loc">
          <!-- 国旗与封包列表同一套：16×16 的盒子 + object-fit: contain，三种源尺寸都不会压扁 -->
          <img v-if="x.IPLocation" class="flag" :src="flagSrc(x.IPLocation)" alt="">
          {{ x.IPLocation }}
        </span>
      </div>
    </div>
  </SettingsModal>
</template>

<style scoped>
.tbl {
  margin: 14px 20px 4px;
  border: 1px solid var(--border);
  background: rgb(var(--inset-rgb) / 30%);
  /* 登录记录可能很多，弹窗自身不长高，让这块自己滚 */
  max-height: 340px;
  overflow: auto;
}

.head,
.row {
  display: grid;
  grid-template-columns: 152px 130px 1fr;
  align-items: center;
  gap: 12px;
  padding: 0 12px;
  font-size: var(--fs-body);
}

.head {
  position: sticky;
  top: 0;
  z-index: 1;
  height: var(--th-h);
  background: var(--panel);
  border-bottom: 1px solid var(--border);
  font-family: var(--share);
  font-size: var(--th-size);
  letter-spacing: .14em;
  text-transform: uppercase;
  color: var(--th-fg);   /* 与全项目其它表头同一份，--muted 在 --panel 上只有 3.85:1 */
}

.row { height: 30px; color: var(--soft); }
.row:hover { background: rgb(var(--tint-rgb) / 4%); }

.empty { padding: 30px 0; text-align: center; color: var(--muted); font-size: var(--fs-body); }

.tm { color: var(--muted); font-variant-numeric: tabular-nums; }
.ip { color: var(--cyan); font-variant-numeric: tabular-nums; }

.loc {
  display: flex;
  align-items: center;
  gap: 6px;
  min-width: 0;
  overflow: hidden;
  text-overflow: ellipsis;
  white-space: nowrap;
}

.flag { width: 16px; height: 16px; object-fit: contain; flex: none; }

/*
  表头每格与数据格同名（对齐规则靠这个），于是数据列的字体 / 字号 / 颜色
  （.notes 12px、.ad / .dt 等宽字、.cnt 青色……）会一并漏进表头，看着就是「备注」「数据」比别的表头大。
  这里按格子把它们收回来：表头只认表头自己那一份。(0,2,1) 压得过任何单类名的列规则。
*/
.head > span {
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
</style>
