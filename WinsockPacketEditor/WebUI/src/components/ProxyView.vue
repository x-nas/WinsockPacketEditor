<script setup lang="ts">
/*
  代理模式的外壳 —— 对应 WinForms 的 Forms/ProxyModeForm。

  那边是「一个 119px 的 Menu（14 项）+ 一个 Tabs（14 页）」两套同步的导航；
  这里只保留侧栏一套，与官网 WPEWeb 的 .sidebar 一致。
  页面定义（顺序 / 分组 / 图标）集中在 proxy/pages.ts，与注入模式共用一份。
*/
import { onBeforeUnmount, onMounted, ref, watch } from 'vue'
import { call } from '../bridge'
import { loadCountryTable } from '../flags'
import { attachListFeed } from '../stores/lists'
import { gotoPage } from '../stores/runtime'
import ProxySide from './proxy/ProxySide.vue'
import ProxyData from './proxy/ProxyData.vue'
import AccountList from './proxy/AccountList.vue'
import ClientList from './proxy/ClientList.vue'
import FilterList from './proxy/FilterList.vue'
import SendList from './proxy/SendList.vue'
import WareHouseList from './proxy/WareHouseList.vue'
import RobotList from './proxy/RobotList.vue'
import TextCompare from './proxy/TextCompare.vue'
import XorCalc from './proxy/XorCalc.vue'
import Transcode from './proxy/Transcode.vue'
import ExtractData from './proxy/ExtractData.vue'
import StatData from './proxy/StatData.vue'
import WpcConfig from './proxy/WpcConfig.vue'
import SystemLog from './proxy/SystemLog.vue'
import { PAGES, type PageKey } from './proxy/pages'

const page = ref<PageKey>('data')

//别处（封包列表右键「添加到文本 A / B」）要求切页：切完清掉，下次还能再切同一页
watch(gotoPage, (k) => {
  if (!k) return
  if (PAGES.some((p) => p.key === k)) page.value = k as PageKey
  gotoPage.value = null
})

let detach: (() => void) | null = null

/*
  14 份中低频列表的接收端在这里挂，而不是在 ProxyData 里 ——
  侧栏的计数用得着它，切到别的页时也不该断掉。

  【顺序要紧】先订阅再通知 C#。反过来的话，C# 那边加载完立刻标脏，
  10ms 后的搬运拍就把整表推出来了，而此刻还没人订阅 —— 那一批就丢了，
  界面上是一片空，直到下一次有人改动列表才恢复。
*/
onMounted(() => {
  detach = attachListFeed()

  /*
    告诉 C# 进代理模式了：它会加载 14 份列表并强制整体推一次。
    WinForms 是进 ProxyModeForm 时做同一件事（那个 Spin 遮罩里那一串）。
  */
  call('enterProxyMode').catch((e) => console.error('[proxy] 进入代理模式失败', e))

  //国旗用的中文国名对照表，整个会话取一次（约 4KB）
  void loadCountryTable()
})
onBeforeUnmount(() => detach?.())
</script>

<template>
  <div class="proxy">
    <ProxySide :current="page" @go="page = $event" />

    <!--
      只有代理数据用 v-show 保活，其余用占位。
      保活是必须的：切走再切回来若重新挂载，PacketList 的滚动位置、
      选中行、以及 attachPacketFeed 的订阅都会重来一遍。
    -->
    <ProxyData v-show="page === 'data'" />

    <!--
      日志页也保活：它自己维护一份 2000 条的环形缓冲，切走再切回来若重新挂载，
      之前的日志就全没了 —— 而排查问题时最需要的恰恰是「刚才那几条」。
    -->
    <SystemLog v-show="page === 'log'" />

    <!--
      账号列表相反，用 v-if 按需挂载：它没有需要保住的运行态
      （列表在 stores/lists，搜索框空着才是常态），而 onMounted 里要问一次
      「身份认证开着没」—— 每次进来重新问反而是对的，那个开关随时可能被改。
    -->
    <AccountList v-if="page === 'account'" />

    <!--
      滤镜列表同样 v-if：没有要保住的运行态（列表在 stores/lists，
      搜索框空着才是常态），重新挂载反而能把选中集清干净。
    -->
    <FilterList v-if="page === 'filter'" />

    <!-- 客户端列表同样 v-if：整表由 FeedList.Auth 推，没有要保住的运行态 -->
    <ClientList v-if="page === 'client'" />

    <!-- 发送列表：运行态（在跑没在跑）由 C# 每秒推，重新挂载会自己对上 -->
    <SendList v-if="page === 'send'" />

    <!-- 仓库列表同样 v-if：整表由 FeedList.WareHouse 推，没有要保住的运行态 -->
    <WareHouseList v-if="page === 'warehouse'" />

    <!-- 机器人列表：运行态由 C# 每秒推（robot:running），重新挂载会自己对上 -->
    <RobotList v-if="page === 'robot'" />

    <!-- 四个工具页：状态都在 stores/tools 里，v-if 销毁再挂回来内容还在 -->
    <TextCompare v-if="page === 'diff'" />
    <XorCalc v-if="page === 'xor'" />
    <Transcode v-if="page === 'transcode'" />
    <ExtractData v-if="page === 'extract'" />

    <!-- 统计数据（页面开着时每秒刷）与 WPC 配置（两份列表都在推送流里） -->
    <StatData v-if="page === 'stat'" />
    <WpcConfig v-if="page === 'wpc'" />
  </div>
</template>

<style scoped>
.proxy {
  position: relative;
  z-index: 10;
  flex: 1;
  min-height: 0;
  display: grid;
  grid-template-columns: var(--side-w, 196px) 1fr;
}
</style>
