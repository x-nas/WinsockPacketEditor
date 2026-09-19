/// <reference types="vite/client" />

declare module '*.cs?raw' {
  const source: string
  export default source
}
