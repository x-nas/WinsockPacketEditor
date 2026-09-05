declare module '*.vue' {
  import type { DefineComponent } from 'vue'
  const component: DefineComponent<{}, {}, any>
  export default component
}

/*
  静态资源的类型声明。

  Vite 会把 import 进来的图片转成 URL 字符串，但 TypeScript 不知道这回事 ——
  没有这段声明，`import wpeLogo from './assets/wpe.png'` 会报 TS2307。
  正规做法是引 vite/client 的类型，但那会把 import.meta.env 等一整套也带进来；
  这里只用到图片，声明这几个就够。
*/
declare module '*.png' {
  const src: string
  export default src
}

declare module '*.svg' {
  const src: string
  export default src
}

declare module '*.ico' {
  const src: string
  export default src
}
