using System;
using System.Reflection;
using System.Windows.Forms;
using WPE.Lib;

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
                string UserName = Operate.ProxyConfig.ProxyAccount.GetProxyAccount_ByAccountID(this.AccountID).UserName;
                this.Text += " - " + UserName;
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void InitDGV()
        {
            try
            {
                dgvAccountLogin.AutoGenerateColumns = false;
                dgvAccountLogin.DataSource = Operate.ProxyConfig.ProxyAccount.LoadProxyAccount_LoginInfo_FromDB(this.AccountID);
                dgvAccountLogin.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvAccountLogin, true, null);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion
    }
}
