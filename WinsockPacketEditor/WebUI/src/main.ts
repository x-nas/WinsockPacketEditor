import { createApp } from 'vue'
import './reset.css'
import App from './App.vue'
import './style.css'
import { installTooltip } from './tooltip'

//接管原生 title，全项目 169 处 title= 一个字都不用改（见 tooltip.ts 的说明）
installTooltip()

createApp(App).mount('#app')
