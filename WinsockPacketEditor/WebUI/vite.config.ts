import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

// 构建产物直接输出到 C# 工程的 wwwroot，由 csproj 的 StageWwwroot 镜像进输出目录。
// base 用相对路径，配合 WebView2 的 https://app.wpe64.local/ 虚拟主机加载。
//
// 2.1.9 起不再用 ant-design-vue（界面全是自绘件），按需引入的 unplugin-vue-components 一并去掉；
// 组件一律在 <script setup> 里显式 import。
export default defineConfig({
  plugins: [vue()],
  base: './',
  build: {
    outDir: '../wwwroot',
    emptyOutDir: true,
    chunkSizeWarningLimit: 1500, // 本地加载，放宽体积告警
  },
  server: {
    port: 5173,
    strictPort: true, // 端口固定，ShellForm 的 DEBUG 分支写死了它
  },
})
