using System;
using System.Reflection;
using System.Windows.Forms;
using WPE.Lib;

namespace WPE.InjectMode
{
    public partial class BackUpSettingsForm : Form
    {
        #region//窗体事件

        public BackUpSettingsForm()
        {
            InitializeComponent();
        }

        private void BackUpSettingsForm_Load(object sender, EventArgs e)
        {
            this.Text = AntdUI.Localization.Get("BackUpSettingsForm", "备份设置");
        }

        #endregion

        #region//退出

        private void bExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        #endregion

        #region//导入备份

        private void bImport_Click(object sender, EventArgs e)
        {
            Operate.SystemConfig.ImportSystemBackUp_Dialog();
        }

        #endregion

        #region//导出备份

        private void bExport_Click(object sender, EventArgs e)
        {
            try
            {
                string FileName = Operate.SystemConfig.AssemblyVersion;
                bool SystemConfig = this.cbBackUp_SystemConfig.Checked;
                bool ProxySet = this.cbBackUp_ProxySet.Checked;
                bool ProxyAccount = this.cbBackUp_ProxyAccount.Checked;
                bool ProxyMapping = this.cbBackUp_ProxyMapping.Checked;
                bool InjectionSet = this.cbBackUp_InjectSet.Checked;
                bool FilterList = this.cbBackUp_FilterList.Checked;
                bool SendList = this.cbBackUp_SendList.Checked;
                bool RobotList = this.cbBackUp_RobotList.Checked;

                Operate.SystemConfig.ExportSystemBackUp_Dialog(
                    FileName,
                    SystemConfig,
                    ProxySet,
                    ProxyAccount,
                    ProxyMapping,
                    InjectionSet,
                    FilterList,
                    SendList,
                    RobotList);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion
    }
}
