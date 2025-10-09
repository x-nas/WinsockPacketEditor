using AntdUI;
using System;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class DataBaseSetting : UserControl
    {
        private Form form = null;

        #region//窗体事件

        public DataBaseSetting(Form form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void DataBaseSetting_Load(object sender, EventArgs e)
        {
            this.txtDataBasePath.Text = Operate.DataBase.dbPath;
            this.txtDataBaseVersion.Text = Operate.DataBase.dbName;
        }

        #endregion

        #region//选择路径

        private void bSelectPath_Click(object sender, EventArgs e)
        {
            try
            {
                var dialog = new AntdUI.FolderBrowserDialog();
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    this.txtDataBasePath.Text = dialog.DirectoryPath;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(bSelectPath_Click), ex.Message);
            }
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            string DBPath = this.txtDataBasePath.Text.Trim();

            if (string.IsNullOrEmpty(DBPath))
            {
                this.txtDataBasePath.Status = TType.Error;
                return;
            }

            Operate.DataBase.dbPath = DBPath;
            Operate.DataBase.InitDB();

            AntdUI.Message.open(new AntdUI.Message.Config(this.form, "数据库配置已保存", TType.Success)
            {
                LocalizationText = "DataBaseSetting.Success"
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
