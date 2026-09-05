<script setup lang="ts">
/*
  中低频列表通道的验证面板（B9d）。

  这不是最终界面 —— 那 14 份列表各自的正式界面要一页一页做。
  这里只回答一个问题：<b>C# 那边一改，前端收到没有、收到的内容对不对</b>。

  没有这一屏，B9d 就是一堆「接了但验证不了」的代码 —— 而这正是 B9d 当初被推迟的原因，
  不该在同一个坑里再摔一次。
*/
import { computed, ref } from 'vue'
import { FeedList } from '../bridge/types'
import { useList } from '../stores/lists'

defineProps<{ open: boolean }>()
defineEmits<{ (e: 'update:open', v: boolean): void }>()

/** 14 份中低频列表，顺序与 C# 的 FeedPump.AllLists 一致。 */
const LISTS = [
  { key: FeedList.SelectProcess, name: '已选进程', hint: '注入模式选目标时变' },
  { key: FeedList.WhiteList, name: '白名单', hint: '防火墙设置' },
  { key: FeedList.BlackList, name: '黑名单', hint: '防火墙设置' },
  { key: FeedList.Account, name: '代理账号', hint: '账号增删改' },
  { key: FeedList.Auth, name: '认证记录', hint: '客户端连上来时追加' },
  { key: FeedList.MapLocal, name: '本地映射', hint: '映射设置' },
  { key: FeedList.MapRemote, name: '远程映射', hint: '映射设置' },
  { key: FeedList.Filter, name: '滤镜', hint: '滤镜增删改与排序' },
  { key: FeedList.Send, name: '发送列表', hint: '发送增删改' },
  { key: FeedList.Robot, name: '机器人', hint: '机器人增删改' },
  { key: FeedList.WareHouse, name: '封包仓库', hint: '仓库增删改' },
  { key: FeedList.AutoStores, name: '自动入库', hint: '入库规则' },
  { key: FeedList.Server, name: 'ProxyCap 节点', hint: '节点与规则' },
  { key: FeedList.Notice, name: 'ProxyCap 公告', hint: '公告增删改' },
] as const

const rows = LISTS.map((l) => ({ ...l, data: useList(l.key) }))

const selected = ref<number>(FeedList.Filter)

const current = computed(() => rows.find((r) => r.key === selected.value))

/** 把一行 DTO 摊平成「字段=值」，验证时看的是字段名对不对、值有没有错位。 */
function preview(row: any): string {
  if (!row) return ''
  return Object.keys(row)
    .map((k) => `${k}=${fmt(row[k])}`)
    .join('  ')
}

function fmt(v: unknown): string {
  if (v === null || v === undefined) return '∅'
  if (typeof v === 'string') return v === '' ? '""' : v
  return String(v)
}
</script>

<template>
  <a-drawer
    :open="open"
    title="列表通道验证 · B9d"
    placement="right"
    :width="860"
    @update:open="$emit('update:open', $event)"
  >
    <p class="tip">
      C# 侧订阅 <code>BindingList.ListChanged</code> 后整表推送。
      在主程序里增删改任意一份列表，这里的行数与内容应当立刻跟着变。
      <b>全是 0 行不一定是错的</b> —— 那些表本来就要先配置才有数据。
    </p>

    <div class="grid">
      <button
        v-for="r in rows"
        :key="r.key"
        class="chip"
        :class="{ on: selected === r.key }"
        @click="selected = r.key"
      >
        <span class="n">{{ r.name }}</span>
        <span class="c">{{ r.data.value.length }}</span>
      </button>
    </div>

    <template v-if="current">
      <div class="hd">
        {{ current.name }} · {{ current.data.value.length }} 行
        <span class="hint">{{ current.hint }}</span>
      </div>

      <div v-if="!current.data.value.length" class="empty">暂无数据</div>

      <ol v-else class="rows">
        <li v-for="(row, i) in current.data.value.slice(0, 100)" :key="i">
          {{ preview(row) }}
        </li>
      </ol>

      <p v-if="current.data.value.length > 100" class="tip">
        只列出前 100 行（共 {{ current.data.value.length }} 行）。
      </p>
    </template>
  </a-drawer>
</template>

<style scoped>
.tip {
  margin: 0 0 12px;
  color: var(--wpe-muted);
  font-size: 12px;
  line-height: 1.7;
}

.tip code {
  padding: 1px 5px;
  border-radius: 3px;
  background: var(--wpe-head);
  font-family: Consolas, monospace;
}

.grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 6px;
  margin-bottom: 16px;
}

.chip {
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: 8px;
  padding: 6px 10px;
  border: 1px solid var(--wpe-line);
  border-radius: 5px;
  background: var(--wpe-panel);
  color: var(--wpe-fg);
  font: inherit;
  font-size: 12px;
  cursor: pointer;
}

.chip.on {
  border-color: var(--wpe-accent);
  outline: 1px solid var(--wpe-accent);
}

.chip .c {
  font-family: Consolas, monospace;
  color: var(--wpe-muted);
}

.chip.on .c { color: var(--wpe-accent); }

.hd {
  margin-bottom: 8px;
  font-weight: 600;
}

.hd .hint {
  margin-left: 10px;
  font-weight: 400;
  font-size: 12px;
  color: var(--wpe-muted);
}

.empty {
  padding: 24px;
  text-align: center;
  color: var(--wpe-muted);
  border: 1px dashed var(--wpe-line);
  border-radius: 6px;
}

.rows {
  margin: 0;
  padding: 0 0 0 30px;
  max-height: 46vh;
  overflow: auto;
}

.rows li {
  padding: 3px 0;
  border-bottom: 1px solid var(--wpe-rowline);
  font-family: Consolas, 'Cascadia Mono', monospace;
  font-size: 11.5px;
  line-height: 1.6;
  word-break: break-all;
  user-select: text;
}
</style>
