import { createApp } from 'vue'
import 'ant-design-vue/dist/reset.css'
import App from './App.vue'
import './style.css'
import { installTooltip } from './tooltip'

// 组件按需自动引入（见 vite.config.ts 的 unplugin-vue-components + AntDesignVueResolver），
// 不 app.use(Antd) 全量注册 —— 只打包实际用到的组件。
//接管原生 title，全项目 169 处 title= 一个字都不用改（见 tooltip.ts 的说明）
installTooltip()

createApp(App).mount('#app')
