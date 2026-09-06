import { ref } from 'vue'
import { call } from '../bridge'

/*
  深色 / 浅色。

  【真源在 C#】用的是已有的 UI.Prefs.IsDark —— WinForms 侧顶栏那个暗色开关
  存的就是它（SystemConfig 表的 IsDark 列，备份 XML 里也带着）。
  外壳另起一份的话，两套 UI 会各记各的，用户在 WinForms 里切了、
  进外壳又变回去，而且备份导入之后两边对不上。

  【浅色怎么实现】不是反相，是同一套设计换一组令牌值 ——
  版式 / 间距 / 边框 / 四角标记 / 网格底纹 / 游走亮带全部保留，
  只有颜色换掉。规则都在 style.css 的 :root[data-theme="light"] 里。

  ⚠️ <b>属性写在 <html> 上，不是 .win 上。</b>右键菜单、提示浮层、确认框
  都是 Teleport 到 body 的，挂在 .win 上它们拿不到令牌，
  会出现「页面浅色、右键菜单还是深色」。
*/
export type Theme = 'dark' | 'light'

export const theme = ref<Theme>('dark')

function applyDocumentTheme(): void {
  try {
    /*
      深色不写属性、浅色才写 —— :root 上那份定义本来就是深色，
      两边都写等于要维护两个入口。
    */
    if (theme.value === 'light') document.documentElement.setAttribute('data-theme', 'light')
    else document.documentElement.removeAttribute('data-theme')
  } catch {
    /* 非浏览器环境，忽略 */
  }
}

/** 用 C# 给的初值设定主题。启动时调一次，不回写 —— 值就是从那边来的。 */
export function initTheme(isDark: boolean | undefined | null): void {
  //认不出来一律深色：这套皮肤是照深色设计的，深色是默认
  theme.value = isDark === false ? 'light' : 'dark'
  applyDocumentTheme()
}

/**
 * 切主题。
 *
 * 先改本地再推 C#：本地这一步是同步的，界面立刻就变；
 * 落库那一步万一失败也只是「这次没记住」，不该让界面卡在旧主题上。
 * 与 setLang 同一条路数。
 */
export async function setTheme(next: Theme): Promise<void> {
  /*
    ⚠️ 属性<b>无条件</b>同步一次，再判要不要往下走。
    先判等再 apply 的话，一旦 ref 与 <html> 上的属性对不上（备份导入、
    或别处直接改了 DOM），点当前这一项永远修不回来 —— 而那正是用户
    会去点的那一下。同步一次几乎不要钱。
  */
  applyDocumentTheme()

  if (next === theme.value) return

  theme.value = next
  applyDocumentTheme()

  try {
    await call('setAppearance', { isDark: next === 'dark' })
  } catch (e) {
    console.error('[theme] 主题未能写回 C#，本次切换不会被记住', e)
  }
}
