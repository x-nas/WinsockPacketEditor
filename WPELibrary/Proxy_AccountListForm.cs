using System;
using System.Reflection;
using System.Windows.Forms;
using WPELibrary.Lib;

namespace WPELibrary
{
    public partial class Proxy_AccountListForm : Form
    {
        #region//初始化

        public Proxy_AccountListForm()
        {
            InitializeComponent();
            this.InitDGV();
            this.ShowProxyAccountInfo();
        }        

        private void InitDGV()
        {
            try
            {
                dgvAccountList.AutoGenerateColumns = false;
                dgvAccountList.DataSource = Socket_Cache.ProxyAccount.lstProxyAccount;
                dgvAccountList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvAccountList, true, null);                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }        

        private void dgvAccountList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dgvAccountList.Columns["cAccountID"].Index)
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

        #region//新建

        private void bAccount_New_Click(object sender, EventArgs e)
        {
            Socket_Operation.ShowProxyAccountForm(-1);
        }

        #endregion

        #region//编辑

        private void bUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvAccountList.SelectedRows.Count > 0 && dgvAccountList.CurrentCell != null)
                {
                    int SelectedIndex = dgvAccountList.CurrentCell.RowIndex;

                    if (SelectedIndex > -1)
                    {
                        Socket_Operation.ShowProxyAccountForm(SelectedIndex);                        
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//删除

        private void bDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvAccountList.SelectedRows.Count > 0 && dgvAccountList.CurrentCell != null)
                {
                    int SelectedIndex = dgvAccountList.CurrentCell.RowIndex;

                    if (SelectedIndex > -1)
                    {
                        Socket_Cache.ProxyAccount.DeleteProxyAccount_ByAccountIndex_Dialog(SelectedIndex);                        
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//计时器

        private void tTimer_Tick(object sender, EventArgs e)
        {
            this.ShowProxyAccountInfo();
        }

        private void ShowProxyAccountInfo()
        {            
            this.tsslAccount_CNT.Text = Socket_Cache.ProxyAccount.lstProxyAccount.Count.ToString();
            this.tsslAccountEnable_CNT.Text = Socket_Operation.GetEnableProxyAccountCount().ToString();
            this.tsslAccountOnLine_CNT.Text = Socket_Operation.GetOnLineProxyAccountCount().ToString();
            this.tsslAccountExpiry_CNT.Text = Socket_Operation.GetExpiryProxyAccountCount().ToString();
            this.dgvAccountList.Refresh();
        }

        #endregion
    }
}
