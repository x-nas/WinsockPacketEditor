using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace WPE.Lib.Controls
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

                if (IsModifierKey(e.KeyCode) && !_expectingKeyRelease)
                {
                    e.SuppressKeyPress = true;
                    return;
                }

                if (e.Modifiers != Keys.None && !IsModifierKey(e.KeyCode))
                {
                    _currentKey = e.KeyCode | e.Modifiers;
                    this.Text = ConvertHotkeyToString(_currentKey);
                    _expectingKeyRelease = true;
                    e.SuppressKeyPress = true;
                }
                else if (!IsModifierKey(e.KeyCode))
                {
                    _currentKey = Keys.None;
                    this.Text = "";
                    e.SuppressKeyPress = true;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void HotkeyTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (!this.Focused)
                {
                    return;
                } 

                if (_expectingKeyRelease && !IsModifierKey(e.KeyCode))
                {
                    _expectingKeyRelease = false;
                }

                if (IsModifierKey(e.KeyCode))
                {
                    _expectingKeyRelease = false;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                {
                    result += "Ctrl + ";
                }

                if ((key & Keys.Alt) == Keys.Alt)
                {
                    result += "Alt + ";
                }

                if ((key & Keys.Shift) == Keys.Shift)
                {
                    result += "Shift + ";
                }                    

                Keys mainKey = key & Keys.KeyCode;

                if (mainKey >= Keys.D0 && mainKey <= Keys.D9)
                {
                    result += ((char)('0' + (mainKey - Keys.D0))).ToString();
                }
                else if (mainKey >= Keys.NumPad0 && mainKey <= Keys.NumPad9)
                {
                    result += "NumPad" + (mainKey - Keys.NumPad0);
                }
                else
                {
                    result += mainKey.ToString();
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    if (bOK)
                    {
                        this.ForeColor = SystemColors.WindowText;
                    }
                    else
                    {
                        this.ForeColor = Color.DarkRed;
                    }

                    return bOK;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return false;
        }

        private bool RegisterRecordedHotkey(int KeyID)
        {
            try
            {
                if (_currentKey != Keys.None && Operate.SystemConfig.MainHandle != IntPtr.Zero)
                {
                    if (KeyID != 0)
                    {
                        NativeMethods.User32.UnregisterHotKey(Operate.SystemConfig.MainHandle, KeyID);
                    }

                    uint modifiers = 0;
                    if ((_currentKey & Keys.Control) == Keys.Control)
                    {
                        modifiers |= 0x0002; // MOD_CONTROL
                    }

                    if ((_currentKey & Keys.Alt) == Keys.Alt)
                    {
                        modifiers |= 0x0001; // MOD_ALT
                    }

                    if ((_currentKey & Keys.Shift) == Keys.Shift)
                    {
                        modifiers |= 0x0004; // MOD_SHIFT
                    }                        

                    uint vk = 0;

                    if (_currentKey >= Keys.NumPad0 && _currentKey <= Keys.NumPad9)
                    {
                        vk = (uint)(_currentKey - Keys.NumPad0 + 0x60);
                    }
                    else
                    {
                        vk = (uint)(_currentKey & Keys.KeyCode);
                    }

                    return NativeMethods.User32.RegisterHotKey(Operate.SystemConfig.MainHandle, KeyID, modifiers, vk);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    {
                        result |= Keys.Control;
                    }
                    else if (key.Equals("Alt", StringComparison.OrdinalIgnoreCase))
                    {
                        result |= Keys.Alt;
                    }
                    else if (key.Equals("Shift", StringComparison.OrdinalIgnoreCase))
                    {
                        result |= Keys.Shift;
                    }                        
                    else
                    {
                        if (key.Length == 1 && char.IsDigit(key[0]))
                        {
                            result |= (Keys)((int)Keys.D0 + (key[0] - '0'));
                        }
                        else if (key.StartsWith("NumPad", StringComparison.OrdinalIgnoreCase) && int.TryParse(key.Substring(6), out int numpadNum) && numpadNum >= 0 && numpadNum <= 9)
                        {
                            result |= (Keys)((int)Keys.NumPad0 + numpadNum);
                        }
                        else if (key.StartsWith("F", StringComparison.OrdinalIgnoreCase) && int.TryParse(key.Substring(1), out int fNum) && fNum >= 1 && fNum <= 24)
                        {
                            result |= (Keys)((int)Keys.F1 + fNum - 1);
                        }
                        else if (Enum.TryParse<Keys>(key, true, out Keys keyValue))
                        {
                            result |= keyValue;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return result;
        }
    }
}
