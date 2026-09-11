// 右键菜单的一项。ContextMenu.vue 与所有用它的列表共用。
//
// 单独成文件的理由与 proxy/pages.ts、proxy/settings.ts 一样：
// <script setup> 里不能写 export，而这个类型两侧都要用。
//
// 字段与 C# 的 ClassObject/Ui/MenuNode.cs 对齐（那是 Operate 侧描述右键菜单的类型），
// 但<b>不是</b>它的 DTO —— 目前各页的菜单都在前端直接写，没走桥。
// 将来若要把 Operate 的 GetCMS_* 接过来，照着 MenuNode 的字段名映射即可。

/*
  ── 菜单图标的唯一一份 ──────────────────────────────────────

  之前每个菜单各自内联 SVG path 串，结果同一个动作画成了两个样子：
  「复制」有 `M5 15V5h10` 和 `M5 15V5a1 1 0 0 1 1-1h9` 两版，
  「删除」有 `l1 13h8l1-13` 和 `l1 12h6l1-12` 两版 —— 都是抄来抄去时手改的。

  集中到这里之后，同一个概念只有一份，改一次全站跟着变。

  【统一的画法】全部是<b>描边</b>（不填色）、viewBox 0 0 24 24、
  线宽由 ContextMenu 的 .ico 统一给（stroke-width: 1.6），
  所以这里只写形状，不写 stroke / fill / width。
*/
export const ICON = {
  //列表顺序
  top: '<path d="M5 4h14M12 20V9M7 13l5-5 5 5"/>',
  up: '<path d="M12 19V8M7 13l5-5 5 5"/>',
  down: '<path d="M12 5v11M7 11l5 5 5-5"/>',
  bottom: '<path d="M5 20h14M12 4v11M7 10l5 5 5-5"/>',

  //编辑（封包列表右键 / 各列表行内的那支笔是同一枚）
  edit: '<path d="M4 20h4L20 8l-4-4L4 16z"/>',

  //复制的两种形态：文本 = 一个大写 T；十六进制 = 「0x」
  text: '<path d="M5 6h14M12 6v13M9 19h6"/>',
  hex: '<ellipse cx="8" cy="12" rx="3.5" ry="5.5"/><path d="M14 8l6 8M20 8l-6 8"/>',

  //剪贴板与增删
  copy: '<rect x="9" y="9" width="11" height="11" rx="1"/><path d="M5 15V5a1 1 0 0 1 1-1h9"/>',
  cut: '<circle cx="6" cy="18" r="2.5"/><circle cx="18" cy="18" r="2.5"/><path d="M7.5 16 19 4M16.5 16 5 4"/>',
  paste: '<rect x="6" y="4" width="12" height="16" rx="1"/><path d="M9 4V3h6v1"/>',
  //粘贴的两种形态：剪贴板里写一个 T / 一个 x
  pasteText: '<rect x="6" y="4" width="12" height="16" rx="1"/><path d="M9 4V3h6v1M9.5 10h5M12 10v6"/>',
  pasteHex: '<rect x="6" y="4" width="12" height="16" rx="1"/><path d="M9 4V3h6v1M9.5 10l5 6M14.5 10l-5 6"/>',
  del: '<path d="M5 7h14M9 7V5h6v2M7 7l1 13h8l1-13"/>',
  clear: '<path d="M18 6L6 18M6 6l12 12"/>',
  save: '<path d="M5 4h10l4 4v11a1 1 0 0 1-1 1H5a1 1 0 0 1-1-1V5a1 1 0 0 1 1-1z M8 20v-6h8v6"/>',

  //标记
  plus: '<path d="M5 12h14M12 5v14" transform="rotate(45 12 12)"/>',
  minus: '<path d="M5 12h14"/>',
  step: '<path d="M4 18l6-6 4 4 6-8"/>',
  dice: '<circle cx="12" cy="12" r="8"/><path d="M9 9h.01M15 15h.01M15 9h.01M9 15h.01"/>',

  //账号
  clock: '<circle cx="12" cy="12" r="8"/><path d="M12 8v4l3 2"/>',
  link: '<path d="M9 15l6-6M7 17a4 4 0 0 1 0-6l2-2M17 7a4 4 0 0 1 0 6l-2 2"/>',
  device: '<rect x="7" y="3" width="10" height="18" rx="2"/><path d="M11 18h2"/>',

  //名单
  eye: '<path d="M2 12s4-7 10-7 10 7 10 7-4 7-10 7-10-7-10-7z"/><circle cx="12" cy="12" r="3"/>',
  eyeOff: '<path d="M2 12s4-7 10-7 10 7 10 7-4 7-10 7-10-7-10-7z"/><path d="M3 3l18 18"/>',
  //时长：表盘 + 指针，三档共用；永久用挂锁，一眼分得出"有期限 / 无期限"
  timer: '<circle cx="12" cy="13" r="7"/><path d="M12 10v3l2 2M9 3h6"/>',
  ban: '<rect x="5" y="11" width="14" height="9" rx="1"/><path d="M8 11V8a4 4 0 0 1 8 0v3"/>',
  //批量操作（账号列表的「批量调整」父项）
  list: '<path d="M4 6h16M4 12h16M4 18h10"/>',

  /* 下面四枚与侧栏 pages.ts 里对应页的图标是同一份 */
  send: '<path d="M4 12l16-8-6 16-2-6z"/>',
  house: '<path d="M3 20V9l9-5 9 5v11"/><path d="M2 20h20"/><path d="M8 20v-7h8v7"/><path d="M8 16.5h8"/>',
  filter: '<path d="M3 5h18l-7 8v6l-4 2v-8z"/>',
  check: '<rect x="4" y="4" width="16" height="16" rx="1"/><path d="M8 12l3 3 5-6"/>',

  /*
    快捷面板的右键菜单要的五枚。
    ⚠️ 别拿现成的凑：`plus` 是<b>旋转 45° 的加号</b>（画出来是个叉，滤镜编辑用它当「排除」标记），
    `ban` 是<b>挂锁</b>（客户端列表的「永久屏蔽」用），两个都不是名字听上去的那个意思。
  */
  add: '<path d="M5 12h14M12 5v14"/>',
  //与 check 成对：同一个方框，一个打勾一个空着 —— 「全部启用 / 全部禁用」并排时一眼分得出
  uncheck: '<rect x="4" y="4" width="16" height="16" rx="1"/>',
  //重置计数：回转箭头，对应 WinForms 那三处的 UndoOutlined
  undo: '<path d="M4 9h10a5 5 0 0 1 0 10H8M4 9l4-4M4 9l4 4"/>',
  //执行 / 停止：实心三角与方块，与状态条上那两个按钮同一种画法
  play: '<path d="M8 5l11 7-11 7z"/>',
  //语言：地球。标题栏那个下拉里，当前语言画 check，其余画它
  globe: '<circle cx="12" cy="12" r="9"/><path d="M3 12h18M12 3a14 14 0 0 1 0 18 14 14 0 0 1 0-18"/>',
  stop: '<rect x="6" y="6" width="12" height="12" rx="1"/>',
} as const

export interface MenuItem {
  /** 分隔线：只写 { divider: true } */
  divider?: boolean
  id?: string
  label?: string
  /** SVG path 串，内联进 <svg viewBox="0 0 24 24">。与侧栏 pages.ts 同一种写法 */
  icon?: string
  disabled?: boolean
  /** 危险操作（删除类），画成红的 */
  danger?: boolean
  /** 二级菜单 */
  sub?: MenuItem[]
}
