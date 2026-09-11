/*
  浏览器按键 → .NET System.Windows.Forms.Keys 的成员名。

  机器人的键盘指令存的是 Keys 枚举的名字（WinForms 里 txtKey_KeyDown 写的是 e.KeyCode.ToString()），
  执行时 RobotExecute 用 Enum.TryParse<Keys> 解回去，所以这边必须吐出<b>一模一样的名字</b>：
  字母是 "A"，数字是 "D1"，回车是 "Return"，退格是 "Back"，Ctrl 是 "ControlKey"，Alt 是 "Menu"……
  按 KeyboardEvent.code（物理键）而不是 key：key 会跟着 Shift / 输入法变（"a" / "A" / "ａ"），code 不会。
*/

const CODE_MAP: Record<string, string> = {
  Enter: 'Return', NumpadEnter: 'Return', Escape: 'Escape', Space: 'Space', Tab: 'Tab', Backspace: 'Back',
  Delete: 'Delete', Insert: 'Insert', Home: 'Home', End: 'End', PageUp: 'PageUp', PageDown: 'PageDown',
  ArrowUp: 'Up', ArrowDown: 'Down', ArrowLeft: 'Left', ArrowRight: 'Right',
  ShiftLeft: 'ShiftKey', ShiftRight: 'ShiftKey', ControlLeft: 'ControlKey', ControlRight: 'ControlKey',
  AltLeft: 'Menu', AltRight: 'Menu', MetaLeft: 'LWin', MetaRight: 'RWin', ContextMenu: 'Apps',
  CapsLock: 'Capital', NumLock: 'NumLock', ScrollLock: 'Scroll', PrintScreen: 'PrintScreen', Pause: 'Pause',
  NumpadAdd: 'Add', NumpadSubtract: 'Subtract', NumpadMultiply: 'Multiply', NumpadDivide: 'Divide', NumpadDecimal: 'Decimal',
  Minus: 'OemMinus', Equal: 'Oemplus', BracketLeft: 'OemOpenBrackets', BracketRight: 'Oem6', Backslash: 'Oem5',
  Semicolon: 'Oem1', Quote: 'Oem7', Comma: 'Oemcomma', Period: 'OemPeriod', Slash: 'OemQuestion', Backquote: 'Oemtilde',
}

/** 单个按键的 Keys 名；认不出来返回空串。 */
export function keyNameOf(e: KeyboardEvent): string {
  const c = e.code
  if (/^Key[A-Z]$/.test(c)) return c.slice(3)
  if (/^Digit[0-9]$/.test(c)) return 'D' + c.slice(5)
  if (/^Numpad[0-9]$/.test(c)) return 'NumPad' + c.slice(6)
  if (/^F([1-9]|1[0-9]|2[0-4])$/.test(c)) return c
  return CODE_MAP[c] ?? ''
}

export function isModifierEvent(e: KeyboardEvent): boolean {
  return e.key === 'Shift' || e.key === 'Control' || e.key === 'Alt' || e.key === 'Meta'
}

/*
  组合按键。格式照 C# 的 SystemConfig.ConvertHotkeyToString（WinForms 的 HotkeyTextBox 显示的就是它）：
  "Ctrl + Alt + Shift + A"，修饰键按 Ctrl / Alt / Shift 的固定顺序，数字写 0–9、小键盘写 NumPad0–9，
  其余用 Keys 的名字。两套 UI 存的串必须长得一样 —— 执行端（RobotExecute.TryGetVirtualKey）就是按这个写法解的。
  没有修饰键、或者按下的本身就是修饰键，返回空串（等下一次）。
*/
export function comboOf(e: KeyboardEvent): string {
  if (isModifierEvent(e)) return ''
  if (!(e.ctrlKey || e.altKey || e.shiftKey)) return ''

  const name = keyNameOf(e)
  if (!name) return ''

  const main = /^D[0-9]$/.test(name) ? name.slice(1) : name
  const parts: string[] = []
  if (e.ctrlKey) parts.push('Ctrl')
  if (e.altKey) parts.push('Alt')
  if (e.shiftKey) parts.push('Shift')
  parts.push(main)
  return parts.join(' + ')
}
