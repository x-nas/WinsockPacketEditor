using System;
using System.Reflection;
using System.Windows.Forms;
using WPE.Lib;

namespace WPELibrary
{
    public partial class Proxy_MapLocalListForm : Form
    {
        private BindingSource bindingSource = new BindingSource();

        #region//窗体事件

        public Proxy_MapLocalListForm()
        {
            InitializeComponent();     

            this.InitDGV();
        }

        private void Proxy_MapLocalForm_Load(object sender, EventArgs e)
        {
            Operate.ProxyConfig.ProxyMapping.IsShow_MapLocal = true;
        }

        private void Proxy_MapLocalForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Operate.ProxyConfig.ProxyMapping.IsShow_MapLocal = false;
        }

        #endregion

        #region//初始化DGV

        private void InitDGV()
        {
            bindingSource.DataSource = Operate.ProxyConfig.ProxyMapping.lstMapLocal;
            dgvMapLocal.AutoGenerateColumns = false;
            dgvMapLocal.DataSource = bindingSource;
            dgvMapLocal.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvMapLocal, true, null);            
        }

        private void dgvMapLocal_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dgvMapLocal.Columns["cURL"].Index)
                {
                    string ProtocolType = dgvMapLocal.Rows[e.RowIndex].Cells["cProtocolType"].Value?.ToString()?.ToLower();
                    string Host = dgvMapLocal.Rows[e.RowIndex].Cells["cHost"].Value?.ToString();
                    string Port = dgvMapLocal.Rows[e.RowIndex].Cells["cPort"].Value?.ToString();
                    string RemotePath = dgvMapLocal.Rows[e.RowIndex].Cells["cRemotePath"].Value?.ToString();

                    if (!string.IsNullOrEmpty(ProtocolType) && !string.IsNullOrEmpty(Host) && !string.IsNullOrEmpty(Port))
                    {
                        string RemoteURL = ProtocolType + "://" + Host + ":" + Port + RemotePath;
                        e.Value = RemoteURL;
                        e.FormattingApplied = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void dgvMapLocal_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int iSelectIndex = this.dgvMapLocal.SelectedRows[0].Index;
                if (iSelectIndex >= 0 && iSelectIndex < Operate.ProxyConfig.ProxyMapping.lstMapLocal.Count)
                {
                    Socket_Operation.ShowProxyMapLocalForm(Operate.ProxyConfig.ProxyMapping.lstMapLocal[iSelectIndex]);
                    bindingSource.ResetBindings(false);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void dgvMapLocal_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvMapLocal.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn && e.RowIndex >= 0)
                {
                    bool bCheck = Convert.ToBoolean(dgvMapLocal.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    dgvMapLocal.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = !bCheck;
                    Operate.ProxyConfig.ProxyMapping.lstMapLocal[e.RowIndex].IsEnable = !bCheck;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion        

        #region//新增

        private void bAdd_Click(object sender, EventArgs e)
        {
            Socket_Operation.ShowProxyMapLocalForm(null);
            bindingSource.ResetBindings(false);
        }

        #endregion

        #region//移除

        private void bRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvMapLocal.Rows.Count > 0 && this.dgvMapLocal.SelectedRows.Count > 0)
                {
                    int iSelectIndex = this.dgvMapLocal.SelectedRows[0].Index;
                    if (iSelectIndex >= 0 && iSelectIndex < Operate.ProxyConfig.ProxyMapping.lstMapLocal.Count)
                    {
                        Operate.ProxyConfig.ProxyMapping.DelMapLocal(Operate.ProxyConfig.ProxyMapping.lstMapLocal[iSelectIndex]);
                        bindingSource.ResetBindings(false);
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }             
        }

        #endregion        

        #region//确定

        private void bOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region//右键菜单

        private void cmsMapLocal_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string sItemText = e.ClickedItem.Name;
            cmsMapLocal.Close();

            try
            {
                Proxy_MapLocal pml = null;
                if (this.dgvMapLocal.Rows.Count > 0 && this.dgvMapLocal.SelectedRows.Count > 0)
                {
                    int iSelectIndex = this.dgvMapLocal.SelectedRows[0].Index;
                    if (iSelectIndex >= 0 && iSelectIndex < Operate.ProxyConfig.ProxyMapping.lstMapLocal.Count)
                    {
                        pml = Operate.ProxyConfig.ProxyMapping.lstMapLocal[iSelectIndex];
                    }
                }                              

                switch (sItemText)
                {
                    case "cmsMapLocal_Top":

                        if (pml != null)
                        {
                            Operate.ProxyConfig.ProxyMapping.UpdateMapLocal_ByListAction(Operate.SystemConfig.ListAction.Top, pml);
                        }

                        break;

                    case "cmsMapLocal_Up":

                        if (pml != null)
                        {
                            Operate.ProxyConfig.ProxyMapping.UpdateMapLocal_ByListAction(Operate.SystemConfig.ListAction.Up, pml);
                        }
                        
                        break;

                    case "cmsMapLocal_Down":

                        if (pml != null)
                        {
                            Operate.ProxyConfig.ProxyMapping.UpdateMapLocal_ByListAction(Operate.SystemConfig.ListAction.Down, pml);
                        }
                        
                        break;

                    case "cmsMapLocal_Bottom":

                        if (pml != null)
                        {
                            Operate.ProxyConfig.ProxyMapping.UpdateMapLocal_ByListAction(Operate.SystemConfig.ListAction.Bottom, pml);
                        }
                        
                        break;

                    case "cmsMapLocal_Import":

                        Operate.ProxyConfig.ProxyMapping.UpdateMapLocal_ByListAction(Operate.SystemConfig.ListAction.Import, pml);
                        bindingSource.ResetBindings(false);

                        break;

                    case "cmsMapLocal_Export":

                        if (this.dgvMapLocal.Rows.Count > 0)
                        {
                            Operate.ProxyConfig.ProxyMapping.UpdateMapLocal_ByListAction(Operate.SystemConfig.ListAction.Export, pml);
                        }
                        
                        break;

                    case "cmsMapLocal_Clear":

                        if (this.dgvMapLocal.Rows.Count > 0)
                        {
                            Operate.ProxyConfig.ProxyMapping.UpdateMapLocal_ByListAction(Operate.SystemConfig.ListAction.CleanUp, pml);
                            bindingSource.ResetBindings(false);
                        }
                        
                        break;
                }

                this.dgvMapLocal.ClearSelection();

                int iIndex = Operate.ProxyConfig.ProxyMapping.lstMapLocal.IndexOf(pml);
                if (iIndex > -1 && iIndex < this.dgvMapLocal.RowCount)
                {
                    this.dgvMapLocal.Rows[iIndex].Selected = true;
                    dgvMapLocal.FirstDisplayedScrollingRowIndex = iIndex;
                }

                this.dgvMapLocal.Refresh();                 
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion
    }
}
