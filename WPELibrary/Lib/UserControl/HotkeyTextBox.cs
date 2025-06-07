using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using WPELibrary.Lib.NativeMethods;

namespace WPELibrary.Lib.UserControl
{
    public class HotkeyTextBox : TextBox
    {
        private Keys _currentKey = Keys.None;
        private bool _expectingKeyRelease = false;        

        public HotkeyTextBox()
        {
            this.ReadOnly = true;
            this.BackColor = SystemColors.Window;
            this.KeyDown += HotkeyTextBox_KeyDown;
            this.KeyUp += HotkeyTextBox_KeyUp;
            this.KeyPress += (s, e) => e.Handled = true;
            this.TabStop = true;
            this.ShortcutsEnabled = false;
        }

        private void HotkeyTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (!this.Focused)
                {
                    return;
                }

                // 忽略单独的修饰键按下
                if (IsModifierKey(e.KeyCode) && !_expectingKeyRelease)
                {
                    e.SuppressKeyPress = true;
                    return;
                }

                // 如果是组合键（修饰键+其他键）
                if (e.Modifiers != Keys.None && !IsModifierKey(e.KeyCode))
                {
                    _currentKey = e.KeyCode | e.Modifiers;
                    this.Text = ConvertHotkeyToString(_currentKey);
                    _expectingKeyRelease = true;
                    e.SuppressKeyPress = true;
                }
                // 如果是非组合键（单独按键）
                else if (!IsModifierKey(e.KeyCode))
                {
                    _currentKey = Keys.None;
                    this.Text = "";
                    e.SuppressKeyPress = true;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        private void HotkeyTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                // 确保当前控件有焦点时才处理
                if (!this.Focused) return;

                if (_expectingKeyRelease && !IsModifierKey(e.KeyCode))
                {
                    _expectingKeyRelease = false;
                    // 仅记录按键，不自动注册
                }

                // 重要：清除修饰键状态
                if (IsModifierKey(e.KeyCode))
                {
                    _expectingKeyRelease = false;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }        

        private bool IsModifierKey(Keys key)
        {
            return key == Keys.ControlKey ||
                   key == Keys.LControlKey ||
                   key == Keys.RControlKey ||
                   key == Keys.Menu ||
                   key == Keys.LMenu ||
                   key == Keys.RMenu ||
                   key == Keys.ShiftKey ||
                   key == Keys.LShiftKey ||
                   key == Keys.RShiftKey;
        }

        private string ConvertHotkeyToString(Keys key)
        {
            string result = "";

            try
            {
                if ((key & Keys.Control) == Keys.Control)
                    result += "Ctrl + ";
                if ((key & Keys.Alt) == Keys.Alt)
                    result += "Alt + ";
                if ((key & Keys.Shift) == Keys.Shift)
                    result += "Shift + ";

                Keys mainKey = key & Keys.KeyCode;

                if (mainKey >= Keys.D0 && mainKey <= Keys.D9)
                {
                    result += ((char)('0' + (mainKey - Keys.D0))).ToString();
                }
                else if (mainKey >= Keys.NumPad0 && mainKey <= Keys.NumPad9)
                {
                    result += ((char)('0' + (mainKey - Keys.NumPad0))).ToString();
                }
                else
                {
                    result += mainKey.ToString();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            

            return result;
        }

        public bool RegisterHotkeyFromText(int KeyID)
        {
            try
            {
                if (string.IsNullOrEmpty(this.Text))
                    return false;

                Keys parsedKey = ParseHotkeyString(this.Text);
                if (parsedKey != Keys.None)
                {
                    _currentKey = parsedKey;

                    bool bOK = RegisterRecordedHotkey(KeyID);
                    if (!bOK)
                    {
                        this.ForeColor = Color.DarkRed;
                    }

                    return bOK;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            
            return false;
        }

        private bool RegisterRecordedHotkey(int KeyID)
        {
            try
            {
                if (_currentKey != Keys.None && Socket_Cache.System.MainHandle != IntPtr.Zero)
                {
                    // 先取消之前注册的热键
                    if (KeyID != 0)
                    {
                        User32.UnregisterHotKey(Socket_Cache.System.MainHandle, KeyID);
                    }

                    // 准备注册参数
                    uint modifiers = 0;
                    if ((_currentKey & Keys.Control) == Keys.Control)
                        modifiers |= 0x0002; // MOD_CONTROL
                    if ((_currentKey & Keys.Alt) == Keys.Alt)
                        modifiers |= 0x0001; // MOD_ALT
                    if ((_currentKey & Keys.Shift) == Keys.Shift)
                        modifiers |= 0x0004; // MOD_SHIFT

                    uint vk = (uint)(_currentKey & Keys.KeyCode);

                    return User32.RegisterHotKey(Socket_Cache.System.MainHandle, KeyID, modifiers, vk);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            

            return false;
        }

        private Keys ParseHotkeyString(string hotkeyString)
        {
            Keys result = Keys.None;

            try
            {
                string[] parts = hotkeyString.Split(new[] { '+' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (string part in parts)
                {
                    string key = part.Trim();

                    if (key.Equals("Ctrl", StringComparison.OrdinalIgnoreCase))
                        result |= Keys.Control;
                    else if (key.Equals("Alt", StringComparison.OrdinalIgnoreCase))
                        result |= Keys.Alt;
                    else if (key.Equals("Shift", StringComparison.OrdinalIgnoreCase))
                        result |= Keys.Shift;
                    else
                    {
                        // 处理数字键
                        if (key.Length == 1 && char.IsDigit(key[0]))
                        {
                            result |= (Keys)((int)Keys.D0 + (key[0] - '0'));
                        }
                        // 处理功能键
                        else if (key.StartsWith("F", StringComparison.OrdinalIgnoreCase) &&
                                 int.TryParse(key.Substring(1), out int fNum) &&
                                 fNum >= 1 && fNum <= 24)
                        {
                            result |= (Keys)((int)Keys.F1 + fNum - 1);
                        }
                        // 处理其他键
                        else if (Enum.TryParse<Keys>(key, true, out Keys keyValue))
                        {
                            result |= keyValue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return result;
        }
    }
}
