using AntdUI;
using System;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class SystemSetting : UserControl
    {
        private Form form;

        #region//窗体事件

        public SystemSetting(Form form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void SystemSetting_Load(object sender, EventArgs e)
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

            this.cRepalce_ForeColor.Value = Operate.FilterConfig.Filter.FilterActionForeColor_Replace;
            this.cRepalce_BackColor.Value = Operate.FilterConfig.Filter.FilterActionBackColor_Replace;
            this.cIntercept_ForeColor.Value = Operate.FilterConfig.Filter.FilterActionForeColor_Intercept;
            this.cIntercept_BackColor.Value = Operate.FilterConfig.Filter.FilterActionBackColor_Intercept;
            this.cChange_ForeColor.Value = Operate.FilterConfig.Filter.FilterActionForeColor_Change;
            this.cChange_BackColor.Value = Operate.FilterConfig.Filter.FilterActionBackColor_Change;
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            Operate.SystemConfig.IsShow_FloatButton = this.switchFloatButton.Checked;

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

            Operate.FilterConfig.Filter.FilterActionForeColor_Replace = this.cRepalce_ForeColor.Value;
            Operate.FilterConfig.Filter.FilterActionBackColor_Replace = this.cRepalce_BackColor.Value;
            Operate.FilterConfig.Filter.FilterActionForeColor_Intercept = this.cIntercept_ForeColor.Value;
            Operate.FilterConfig.Filter.FilterActionBackColor_Intercept = this.cIntercept_BackColor.Value;
            Operate.FilterConfig.Filter.FilterActionForeColor_Change = this.cChange_ForeColor.Value;
            Operate.FilterConfig.Filter.FilterActionBackColor_Change = this.cChange_BackColor.Value;

            AntdUI.Message.open(new AntdUI.Message.Config(this.form, "系统设置保存成功", TType.Success)
            {
                LocalizationText = "SystemSettingsForm.Success"
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
