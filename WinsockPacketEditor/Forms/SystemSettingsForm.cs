using AntdUI;
using System;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class SystemSettingsForm : Form
    {
        private Form form;

        #region//窗体事件

        public SystemSettingsForm(Form form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void SystemSettingsForm_Load(object sender, EventArgs e)
        {
            this.Text = AntdUI.Localization.Get("SystemSettingsForm", "系统设置");

            switch (Operate.SystemConfig.StartMode)
            {
                case Operate.SystemConfig.SystemMode.Process:

                    this.cbSpeedMode.Checked = Operate.PacketConfig.Packet.SpeedMode;

                    break;

                case Operate.SystemConfig.SystemMode.Proxy:

                    this.cbSpeedMode.Checked = Operate.ProxyConfig.Proxy.SpeedMode;

                    break;
            }
            
            this.switchFloatButton.Checked = Operate.SystemConfig.IsShow_FloatButton;

            switch (Operate.SystemConfig.ListExecute)
            {
                case Operate.SystemConfig.Execute.Together:
                    this.rbListExecute_Together.Checked = true;
                    break;

                case Operate.SystemConfig.Execute.Sequence:
                    this.rbListExecute_Sequence.Checked = true;
                    break;
            }

            switch (Operate.FilterConfig.Filter.FilterExecute)
            {
                case Operate.FilterConfig.Filter.Execute.Priority:
                    this.rbFilterSet_Priority.Checked = true;
                    break;

                case Operate.FilterConfig.Filter.Execute.Sequence:
                    this.rbFilterSet_Sequence.Checked = true;
                    break;
            }

            this.lFAColor_Replace.ForeColor = Operate.FilterConfig.Filter.FilterActionForeColor_Replace;
            this.lFAColor_Replace.BackColor = Operate.FilterConfig.Filter.FilterActionBackColor_Replace;

            this.lFAColor_Intercept.ForeColor = Operate.FilterConfig.Filter.FilterActionForeColor_Intercept;
            this.lFAColor_Intercept.BackColor = Operate.FilterConfig.Filter.FilterActionBackColor_Intercept;

            this.lFAColor_Change.ForeColor = Operate.FilterConfig.Filter.FilterActionForeColor_Change;
            this.lFAColor_Change.BackColor = Operate.FilterConfig.Filter.FilterActionBackColor_Change;

            this.lFAColor_Other.ForeColor = Operate.FilterConfig.Filter.FilterActionForeColor_Other;
            this.lFAColor_Other.BackColor = Operate.FilterConfig.Filter.FilterActionBackColor_Other;
        }

        #endregion

        #region//退出

        private void bExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            switch (Operate.SystemConfig.StartMode)
            {
                case Operate.SystemConfig.SystemMode.Process:

                    Operate.PacketConfig.Packet.SpeedMode = this.cbSpeedMode.Checked;
                    ((InterfaceInfo.IInjectMode)form).InitFloatButton();

                    break;

                case Operate.SystemConfig.SystemMode.Proxy:

                    Operate.ProxyConfig.Proxy.SpeedMode = this.cbSpeedMode.Checked;
                    ((InterfaceInfo.IProxyMode)form).InitFloatButton();

                    break;
            }

            if (this.rbListExecute_Together.Checked)
            {
                Operate.SystemConfig.ListExecute = Operate.SystemConfig.Execute.Together;
            }
            else
            {
                Operate.SystemConfig.ListExecute = Operate.SystemConfig.Execute.Sequence;
            }

            if (this.rbFilterSet_Priority.Checked)
            {
                Operate.FilterConfig.Filter.FilterExecute = Operate.FilterConfig.Filter.Execute.Priority;
            }
            else
            {
                Operate.FilterConfig.Filter.FilterExecute = Operate.FilterConfig.Filter.Execute.Sequence;
            }

            Operate.SystemConfig.IsShow_FloatButton = this.switchFloatButton.Checked;

            AntdUI.Message.open(new AntdUI.Message.Config(this, "系统设置保存成功", TType.Success)
            {
                LocalizationText = "SystemSettingsForm.Success"
            });
        }

        #endregion
    }
}
