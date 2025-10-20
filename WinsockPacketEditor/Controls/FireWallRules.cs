using AntdUI;
using System;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class FireWallRules : UserControl
    {
        private Form form = null;

        #region//窗体事件

        public FireWallRules(Form form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void FireWallRules_Load(object sender, EventArgs e)
        {
            this.cbAutoBlock_UnSupport.Checked = Operate.ProxyConfig.Proxy.FireWall_AutoBlock_UnSupport;
            this.cbAutoClear_Expiry.Checked = Operate.ProxyConfig.Proxy.FireWall_AutoClear_Expiry;
            this.nudAutoBlock_UnSupport.Value = Operate.ProxyConfig.Proxy.FireWall_AutoBlock_Minutes;
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            Operate.ProxyConfig.Proxy.FireWall_AutoBlock_UnSupport = this.cbAutoBlock_UnSupport.Checked;
            Operate.ProxyConfig.Proxy.FireWall_AutoClear_Expiry = this.cbAutoClear_Expiry.Checked;
            Operate.ProxyConfig.Proxy.FireWall_AutoBlock_Minutes = ((int)this.nudAutoBlock_UnSupport.Value);

            AntdUI.Message.open(new AntdUI.Message.Config(this.form, "防火墙规则保存成功", TType.Success)
            {
                LocalizationText = "FireWallSetting.Rules.Success"
            });

            this.Dispose();
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
