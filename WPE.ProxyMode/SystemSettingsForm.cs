using AntdUI;
using System;
using System.Windows.Forms;
using WPE.Lib;

namespace WPE.ProxyMode
{
    public partial class SystemSettingsForm : Form
    {
        #region//窗体事件

        public SystemSettingsForm()
        {
            InitializeComponent();
        }

        private void SystemSettingsForm_Load(object sender, EventArgs e)
        {
            this.Text = AntdUI.Localization.Get("SystemSettingsForm", "系统设置");

            this.cbSpeedMode.Checked = Operate.ProxyConfig.Proxy.SpeedMode;
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            Operate.ProxyConfig.Proxy.SpeedMode = this.cbSpeedMode.Checked;

            AntdUI.Message.open(new AntdUI.Message.Config(this, "系统设置保存成功", TType.Success)
            {
                LocalizationText = "SystemSettingsForm.Success"
            });
        }

        #endregion

        #region//退出

        private void bExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        #endregion
    }
}
