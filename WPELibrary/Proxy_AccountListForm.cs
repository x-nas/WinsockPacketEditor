using System;
using System.ComponentModel;
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

        private void Proxy_AccountListForm_Load(object sender, EventArgs e)
        {
            Socket_Cache.ProxyAccount.IsShow = true;
        }

        private void Proxy_AccountListForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Socket_Cache.ProxyAccount.IsShow = false;
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
            Socket_Operation.ShowProxyAccountForm(Guid.Empty);
        }

        #endregion

        #region//编辑

        private void dgvAccountList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvAccountList.SelectedRows.Count > 0 && dgvAccountList.CurrentRow != null)
                {
                    Guid AID = (Guid)dgvAccountList.Rows[dgvAccountList.CurrentRow.Index].Cells["cAID"].Value;

                    if (AID != null && AID != Guid.Empty)
                    {
                        Socket_Operation.ShowProxyAccountForm(AID);
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
                    Guid[] glAID = this.GetSelectedAID();

                    if (glAID.Length > 0)
                    {
                        Socket_Cache.ProxyAccount.DeleteProxyAccount_Dialog(glAID);
                        this.Search_UserName();
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//导出

        private void bExport_Click(object sender, EventArgs e)
        {
            Guid[] glAID = this.GetSelectedAID();

            if (glAID.Length > 0)
            {
                Socket_Cache.ProxyAccount.SaveProxyAccountList_Dialog(string.Empty, glAID);
            }            
        }

        #endregion

        #region//导入

        private void bImport_Click(object sender, EventArgs e)
        {
            Socket_Cache.ProxyAccount.LoadProxyAccountList_Dialog();
        }

        #endregion

        #region//查找 - 用户名

        private void bSearch_UserName_Click(object sender, EventArgs e)
        {
            this.Search_UserName();
        }

        private void Search_UserName()
        {
            try
            {
                string UserName = this.txtSearch_UserName.Text.Trim();

                if (string.IsNullOrEmpty(UserName))
                {
                    this.dgvAccountList.DataSource = Socket_Cache.ProxyAccount.lstProxyAccount;
                }
                else
                {
                    BindingList<Proxy_AccountInfo> pai = Socket_Cache.ProxyAccount.GetProxyAccount_ByUserName(UserName);
                    this.dgvAccountList.DataSource = pai;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//查找 - 状态

        private void cbIs_CheckedChanged(object sender, EventArgs e)
        {
            this.Search_State();
        }

        private void cbbSearch_State_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Search_State();
        }

        private void Search_State()
        {
            try
            {
                if (this.cbbSearch_State.SelectedIndex == 0)
                {
                    this.dgvAccountList.DataSource = Socket_Cache.ProxyAccount.GetProxyAccount_ByIsEnable(this.cbIs.Checked);                 
                }
                else if (this.cbbSearch_State.SelectedIndex == 1)
                {
                    this.dgvAccountList.DataSource = Socket_Cache.ProxyAccount.GetProxyAccount_ByIsOnLine(this.cbIs.Checked);
                }
                else if (this.cbbSearch_State.SelectedIndex == 2)
                {
                    this.dgvAccountList.DataSource = Socket_Cache.ProxyAccount.GetProxyAccount_ByIsExpiry(this.cbIs.Checked);
                }                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        #endregion

        #region//获取所有选中的账号ID

        private Guid[] GetSelectedAID()
        {
            Guid[] glAID = new Guid[dgvAccountList.Rows.Count];

            int index = 0;
            for (int i = 0; i < dgvAccountList.Rows.Count; i++)
            {
                if (dgvAccountList.Rows[i].Selected)
                {
                    glAID[index] = (Guid)dgvAccountList.Rows[i].Cells["cAID"].Value;
                    index++;
                }
            }

            if (index < glAID.Length)
            {
                Array.Resize(ref glAID, index);
            }

            return glAID;
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
