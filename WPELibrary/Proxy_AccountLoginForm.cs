using System;
using System.Reflection;
using System.Windows.Forms;
using WPELibrary.Lib;

namespace WPELibrary
{
    public partial class Proxy_AccountLoginForm : Form
    {
        private Guid AccountID = Guid.Empty;

        #region//初始化

        public Proxy_AccountLoginForm(Guid AID)
        {
            InitializeComponent();

            if (AID != Guid.Empty)
            { 
                this.AccountID = AID;

                this.InitForm();
                this.InitDGV();
            }
        }        

        private void InitForm()
        {
            try
            {
                this.Text += " - " + Socket_Cache.ProxyAccount.GetUserName_ByAccountID(this.AccountID);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void InitDGV()
        {
            try
            {
                dgvAccountLogin.AutoGenerateColumns = false;
                dgvAccountLogin.DataSource = Socket_Cache.ProxyAccount.LoadProxyAccount_LoginInfo_FromDB(this.AccountID);
                dgvAccountLogin.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvAccountLogin, true, null);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void dgvAccountLogin_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dgvAccountLogin.Columns["cID"].Index)
                {
                    e.Value = (e.RowIndex + 1).ToString();
                    e.FormattingApplied = true;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion
    }
}
