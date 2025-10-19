using AntdUI;
using System;
using System.Drawing;
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
            this.cbSpeedMode.Checked = Operate.SystemConfig.SpeedMode;
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

            this.cRepalce_ForeColor.Value = Operate.FilterConfig.Filter.FilterReplace_ForeColor;
            this.cRepalce_BackColor.Value = Operate.FilterConfig.Filter.FilterReplace_BackColor;
            this.cIntercept_ForeColor.Value = Operate.FilterConfig.Filter.FilterIntercept_ForeColor;
            this.cIntercept_BackColor.Value = Operate.FilterConfig.Filter.FilterIntercept_BackColor;
            this.cChange_ForeColor.Value = Operate.FilterConfig.Filter.FilterChange_ForeColor;
            this.cChange_BackColor.Value = Operate.FilterConfig.Filter.FilterChange_BackColor;
        }

        #endregion

        #region//还原颜色

        private void bReplaceReset_Click(object sender, EventArgs e)
        {
            this.cRepalce_ForeColor.Value = Color.Black;
            this.cRepalce_BackColor.Value = Color.Goldenrod;
        }

        private void bInterceptReset_Click(object sender, EventArgs e)
        {
            this.cIntercept_ForeColor.Value = Color.White;
            this.cIntercept_BackColor.Value = Color.DarkRed;
        }

        private void bChangeReset_Click(object sender, EventArgs e)
        {
            this.cChange_ForeColor.Value = Color.Black;
            this.cChange_BackColor.Value = Color.DodgerBlue;
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            try
            {
                Operate.SystemConfig.SpeedMode = this.cbSpeedMode.Checked;
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
                
                Operate.FilterConfig.Filter.FilterReplace_ForeColor = this.cRepalce_ForeColor.Value;
                Operate.FilterConfig.Filter.FilterReplace_BackColor = this.cRepalce_BackColor.Value;
                Operate.FilterConfig.Filter.FilterIntercept_ForeColor = this.cIntercept_ForeColor.Value;
                Operate.FilterConfig.Filter.FilterIntercept_BackColor = this.cIntercept_BackColor.Value;
                Operate.FilterConfig.Filter.FilterChange_ForeColor = this.cChange_ForeColor.Value;
                Operate.FilterConfig.Filter.FilterChange_BackColor = this.cChange_BackColor.Value;                

                if (this.form is InterfaceInfo.IInjectMode injectForm)
                {
                    injectForm.InitFloatButton();
                }
                else if (this.form is InterfaceInfo.IProxyMode proxyForm)
                {
                    proxyForm.InitFloatButton();
                }                

                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "系统设置保存成功", TType.Success)
                {
                    LocalizationText = "SystemSettingsForm.Success"
                });

                this.Dispose();
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(bSave_Click), ex.Message);
            }            
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
