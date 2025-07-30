using System;
using System.Reflection;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public class HotkeyTextBox : AntdUI.Input
    {
        private Keys _currentKey = Keys.None;
        private bool _expectingKeyRelease = false;

        public HotkeyTextBox()
        {
            this.ReadOnly = true;            
            this.KeyDown += HotkeyTextBox_KeyDown;
            this.KeyUp += HotkeyTextBox_KeyUp;
            this.KeyPress += (s, e) => e.Handled = true;
            this.TabStop = true;
            this.HandShortcutKeys = true;
        }

        private void HotkeyTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (!this.Focused)
                {
                    return;
                }

                if (Operate.SystemConfig.IsModifierKey(e.KeyCode) && !_expectingKeyRelease)
                {
                    e.SuppressKeyPress = true;
                    return;
                }

                if (e.Modifiers != Keys.None && !Operate.SystemConfig.IsModifierKey(e.KeyCode))
                {
                    _currentKey = e.KeyCode | e.Modifiers;
                    this.Text = Operate.SystemConfig.ConvertHotkeyToString(_currentKey);
                    _expectingKeyRelease = true;
                    e.SuppressKeyPress = true;
                }
                else if (!Operate.SystemConfig.IsModifierKey(e.KeyCode))
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

                if (_expectingKeyRelease && !Operate.SystemConfig.IsModifierKey(e.KeyCode))
                {
                    _expectingKeyRelease = false;
                }

                if (Operate.SystemConfig.IsModifierKey(e.KeyCode))
                {
                    _expectingKeyRelease = false;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
    }
}
