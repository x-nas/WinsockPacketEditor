using System;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class BackUpSetting : UserControl
    {
        private Form form;

        #region//窗体事件

        public BackUpSetting(Form form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void BackUpSetting_Load(object sender, EventArgs e)
        {
            this.Text = AntdUI.Localization.Get("BackUpSettingsForm", "备份设置");
        }

        #endregion        

        #region//导入备份

        private async void bImport_Click(object sender, EventArgs e)
        {
            try
            {
                    await Operate.SystemConfig.ImportSystemBackUp_Dialog(this.form);

                    //备份里可能带着主题与语言，导入后要把 UI.Prefs 真正应用到 AntdUI
                    WinFormsUiHost.ApplyAll();
            }
            catch (Exception ex)
            {
                //async void：await 之后抛出的异常不会被 WinForms 兜住，必须自己捕获
                Operate.DoLog(nameof(bImport_Click), ex);
            }
        }

        #endregion

        #region//导出备份

        private async void bExport_Click(object sender, EventArgs e)
        {
            try
            {
                string FileName = Operate.SystemConfig.AssemblyVersion;
                bool SystemConfig = this.cbBackUp_SystemConfig.Checked;
                bool ProxySet = this.cbBackUp_ProxySet.Checked;
                bool ProxyAccount = this.cbBackUp_ProxyAccount.Checked;
                bool WhiteList = this.cbBackUp_WhiteList.Checked;
                bool BlackList = this.cbBackUp_BlackList.Checked;
                bool ProxyMapping = this.cbBackUp_ProxyMapping.Checked;
                bool InjectionSet = this.cbBackUp_InjectSet.Checked;
                bool FilterList = this.cbBackUp_FilterList.Checked;
                bool SendList = this.cbBackUp_SendList.Checked;
                bool RobotList = this.cbBackUp_RobotList.Checked;

                await Operate.SystemConfig.ExportSystemBackUp_Dialog(FileName,
                    SystemConfig,
                    ProxySet,
                    ProxyAccount,
                    WhiteList,
                    BlackList,
                    ProxyMapping,
                    InjectionSet,
                    FilterList,
                    SendList,
                    RobotList);
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(bExport_Click), ex);
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
