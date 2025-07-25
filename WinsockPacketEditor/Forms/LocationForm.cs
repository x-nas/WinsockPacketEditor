using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class LocationForm : Form
    {
        private AccountInfo aiSelect;

        #region//窗体事件

        public LocationForm(ProxyModeForm form, AccountInfo ai)
        {
            InitializeComponent();

            if (ai == null)
            {
                string Title = AntdUI.Localization.Get("LocationForm.Error", "加载账号数据出错");
                string Content = AntdUI.Localization.Get("System.CheckSystemLog", "请检查系统日志");
                AntdUI.Notification.error(form, Title, Content, AntdUI.TAlignFrom.TR);
                this.Close();
            }
            else
            {
                this.aiSelect = ai;
            }
        }

        private void LocationForm_Load(object sender, EventArgs e)
        {
            this.Text = AntdUI.Localization.Get("LocationForm", "登录情况");

            this.InitTable_Location();
        }

        #endregion

        #region//初始化表格

        private void InitTable_Location()
        {
            tLocation.Columns = new AntdUI.ColumnCollection {
                new AntdUI.Column("LoginTime", "登录时间").SetLocalizationTitleID("Table.AccountList.Column."),
                new AntdUI.Column("LoginIP", "登录IP").SetLocalizationTitleID("Table.AccountList.Column."),
                new AntdUI.Column("IPLocation", "IP所属地").SetLocalizationTitleID("Table.AccountList.Column."),
            };

            this.tLocation.ColumnFont = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.tLocation.DataSource = Operate.ProxyConfig.Account.LoadAccountLocation_FromDB(this.aiSelect.AID);
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
