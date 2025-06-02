using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using WPELibrary.Lib;

namespace WPELibrary
{
    public partial class Proxy_AccountListForm : Form
    {
        private int currentPage = 1;
        private int pageSize = 10;        
        private string SearchType = string.Empty;
        private string Search_UserName = string.Empty;
        private DateTime Search_ExpireFrom, Search_ExpireTo;
        private BindingList<Proxy_AccountInfo> allData = new BindingList<Proxy_AccountInfo>();
        private List<Proxy_AccountInfo> pageData = new List<Proxy_AccountInfo>();

        #region//初始化

        public Proxy_AccountListForm()
        {
            InitializeComponent();

            this.InitDGV();
            this.LoadData("UserName");
            this.InitForm();
        }        

        private void InitDGV()
        {
            dgvAccountList.AutoGenerateColumns = false;            
            dgvAccountList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvAccountList, true, null);
        }

        private void InitForm()
        { 
            this.cbbPageSize.SelectedIndex = 0;
            this.PageSize_Changed();

            this.InitExpireTime();
        }

        private void InitExpireTime()
        {
            try
            {
                DateTime Now = DateTime.Now;
                this.dtpExpireFrom.Value = Now.AddMonths(-1);
                this.dtpExpireTo.Value = Now;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void Proxy_AccountListForm_Load(object sender, EventArgs e)
        {
            Socket_Cache.ProxyAccount.IsShow = true;            
        }

        private void Proxy_AccountListForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Socket_Cache.ProxyAccount.IsShow = false;
        }

        #endregion

        #region//搜索并显示账号列表（异步）

        private void LoadData(string sType)
        {
            try
            {
                if (!bgwAccountList.IsBusy)
                {
                    if (string.IsNullOrEmpty(sType))
                    {
                        if (string.IsNullOrEmpty(this.SearchType))
                        {
                            this.SearchType = "UserName_All";
                        }                        
                    }
                    else
                    { 
                        this.SearchType = sType;
                    }

                    bgwAccountList.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        private void bgwAccountList_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                switch (SearchType)
                {
                    case "UserName":
                        e.Result = Socket_Cache.ProxyAccount.GetProxyAccount_ByUserName(this.Search_UserName);
                        break;

                    case "UserName_All":
                        e.Result = Socket_Cache.ProxyAccount.lstProxyAccount;
                        break;

                    case "IsEnable_True":
                        e.Result = Socket_Cache.ProxyAccount.GetProxyAccount_ByIsEnable(true);
                        break;

                    case "IsEnable_False":
                        e.Result = Socket_Cache.ProxyAccount.GetProxyAccount_ByIsEnable(false);
                        break;

                    case "IsOnLine_True":
                        e.Result = Socket_Cache.ProxyAccount.GetProxyAccount_ByIsOnLine(true);
                        break;

                    case "IsOnLine_False":
                        e.Result = Socket_Cache.ProxyAccount.GetProxyAccount_ByIsOnLine(false);
                        break;

                    case "IsExpiry_True":
                        e.Result = Socket_Cache.ProxyAccount.GetProxyAccount_ByIsExpiry(true);
                        break;

                    case "IsExpiry_False":
                        e.Result = Socket_Cache.ProxyAccount.GetProxyAccount_ByIsExpiry(false);
                        break;

                    case "Expire_Time":
                        e.Result = Socket_Cache.ProxyAccount.GetProxyAccount_ByExpireTime(this.Search_ExpireFrom, this.Search_ExpireTo);
                        break;

                    default:
                        e.Result = Socket_Cache.ProxyAccount.lstProxyAccount;
                        break;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        private void bgwAccountList_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.allData = e.Result as BindingList<Proxy_AccountInfo>;
                this.currentPage = 1;
                this.ShowAccountList();
                this.ShowProxyAccountInfo();                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        private void ShowAccountList()
        {
            try
            {
                int totalRecords = this.allData.Count;
                int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);
                int start = (currentPage - 1) * pageSize;
                int end = Math.Min(start + pageSize, totalRecords);

                pageData = allData.Skip(start).Take(pageSize).ToList();

                dgvAccountList.DataSource = this.pageData;

                for (int i = 0; i < dgvAccountList.Rows.Count; i++)
                {
                    dgvAccountList.Rows[i].Cells["cID"].Value = start + i + 1;
                }

                if (totalPages == 0)
                {
                    lblTotalPages.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_32), 0, 0);
                }
                else
                {
                    lblTotalPages.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_32), currentPage, totalPages);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void ShowProxyAccountInfo()
        {
            this.tsslAccount_CNT.Text = allData.Count.ToString();
            this.tsslAccountEnable_CNT.Text = Socket_Operation.GetEnableProxyAccountCount(allData).ToString();
            this.tsslAccountOnLine_CNT.Text = Socket_Operation.GetOnLineProxyAccountCount(allData).ToString();
            this.tsslAccountExpiry_CNT.Text = Socket_Operation.GetExpiryProxyAccountCount(allData).ToString();            
        }

        #endregion

        #region//分页

        private void cbbPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.PageSize_Changed();
            this.currentPage = 1;
            this.ShowAccountList();
        }

        private void PageSize_Changed()
        {
            if (this.cbbPageSize.SelectedIndex == 0)
            { 
                this.pageSize = 10;
            }
            else if (this.cbbPageSize.SelectedIndex == 1)
            {
                this.pageSize = 20;
            }
            else if (this.cbbPageSize.SelectedIndex == 2)
            {
                this.pageSize = 50;
            }
            else if (this.cbbPageSize.SelectedIndex == 3)
            {
                this.pageSize = 100;
            }
        }

        private void bFirst_Click(object sender, EventArgs e)
        {
            currentPage = 1;
            ShowAccountList();
        }

        private void bPrevious_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                ShowAccountList();
            }
        }

        private void bNext_Click(object sender, EventArgs e)
        {
            int totalRecords = allData.Count;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            if (currentPage < totalPages)
            {
                currentPage++;
                ShowAccountList();
            }
        }

        private void bLast_Click(object sender, EventArgs e)
        {
            int totalRecords = allData.Count;
            int totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            currentPage = totalPages;
            ShowAccountList();
        }

        #endregion

        #region//全选

        private void bSelectAll_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in this.dgvAccountList.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        row.Selected = true;
                    }
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
            this.LoadData(string.Empty);
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
                    List<Guid> gList = Socket_Operation.GetSelectedProxyAccount(this.dgvAccountList);

                    if (gList.Count > 0)
                    {
                        Socket_Cache.ProxyAccount.DeleteProxyAccount_Dialog(gList);
                        this.LoadData(string.Empty);
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//加时

        private void bAddTime_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvAccountList.SelectedRows.Count > 0 && dgvAccountList.CurrentCell != null)
                {
                    List<Guid> gList = Socket_Operation.GetSelectedProxyAccount(this.dgvAccountList);                    

                    if (gList.Count > 0)
                    {
                        Socket_Operation.ShowAccountTimeForm(gList);
                        this.LoadData(string.Empty);
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
            try
            {
                List<Guid> gList = Socket_Operation.GetSelectedProxyAccount(this.dgvAccountList);

                if (gList.Count > 0)
                {
                    Socket_Cache.ProxyAccount.SaveProxyAccountList_Dialog(string.Empty, gList);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//导入

        private void bImport_Click(object sender, EventArgs e)
        {
            Socket_Cache.ProxyAccount.LoadProxyAccountList_Dialog();
            this.LoadData(string.Empty);
        }

        #endregion

        #region//查找 - 用户名

        private void bSearch_UserName_Click(object sender, EventArgs e)
        {
            string sType = string.Empty;
            this.Search_UserName = this.txtSearch_UserName.Text.Trim();

            if (string.IsNullOrEmpty(this.Search_UserName))
            {
                sType = "UserName_All";
            }
            else
            {
                sType = "UserName";
            }

            this.LoadData(sType);
        }

        #endregion

        #region//查找 - 状态

        private void bSearch_State_Click(object sender, EventArgs e)
        {
            string sType = string.Empty;

            if (this.cbbSearch_State.SelectedIndex == 0)
            {
                sType = "IsEnable_True";
            }
            else if (this.cbbSearch_State.SelectedIndex == 1)
            {
                sType = "IsEnable_False";
            }
            else if (this.cbbSearch_State.SelectedIndex == 2)
            {
                sType = "IsOnLine_True";
            }
            else if (this.cbbSearch_State.SelectedIndex == 3)
            {
                sType = "IsOnLine_False";
            }
            else if (this.cbbSearch_State.SelectedIndex == 4)
            {
                sType = "IsExpiry_True";
            }
            else if (this.cbbSearch_State.SelectedIndex == 5)
            {
                sType = "IsExpiry_False";
            }
            else
            {
                sType = "UserName_All";
            }

            this.LoadData(sType);
        }

        #endregion

        #region//查找 - 过期时间

        private void bSearch_Expire_Click(object sender, EventArgs e)
        {
            this.Search_ExpireFrom = this.dtpExpireFrom.Value;
            this.Search_ExpireTo = this.dtpExpireTo.Value;

            if (this.Search_ExpireFrom < this.Search_ExpireTo)
            {
                this.LoadData("Expire_Time");
            }
        }        

        #endregion

        #region//右键菜单

        private void cmsAccountList_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string sItemText = e.ClickedItem.Name;
            cmsAccountList.Close();

            try
            {
                if (dgvAccountList.Rows.Count > 0)
                {
                    if (dgvAccountList.SelectedRows.Count > 0 && dgvAccountList.CurrentCell != null)
                    {
                        List<Guid> gList = Socket_Operation.GetSelectedProxyAccount(this.dgvAccountList);

                        if (gList.Count > 0)
                        {
                            switch (sItemText)
                            {
                                case "cmsAccountList_LoginInfo":

                                    Socket_Operation.ShowAccountLoginForm(gList[0]);

                                    break;

                                case "cmsAccountList_Export":

                                    List<Guid> gExport = new List<Guid>();

                                    foreach (Proxy_AccountInfo pai in this.allData)
                                    {
                                        gExport.Add(pai.AID);
                                    }

                                    Socket_Cache.ProxyAccount.SaveProxyAccountList_Dialog(string.Empty, gExport);

                                    break;

                                case "cmsAccountList_Clear":

                                    List<Guid> gClear = new List<Guid>();

                                    foreach (Proxy_AccountInfo pai in this.allData)
                                    {
                                        gClear.Add(pai.AID);
                                    }

                                    if (gClear.Count > 0)
                                    {
                                        Socket_Cache.ProxyAccount.DeleteProxyAccount_Dialog(gClear);
                                        this.LoadData(string.Empty);
                                    }                                    

                                    break;

                                case "cmsAccountList_AddTime":

                                    List<Guid> gAddTime = new List<Guid>();

                                    foreach (Proxy_AccountInfo pai in this.allData)
                                    {
                                        gAddTime.Add(pai.AID);
                                    }

                                    if (gAddTime.Count > 0)
                                    {
                                        Socket_Operation.ShowAccountTimeForm(gAddTime);
                                        this.LoadData(string.Empty);
                                    }

                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion        
    }
}
