using System;
using System.Reflection;
using System.Windows.Forms;
using WPE.Lib;

namespace WPELibrary
{
    public partial class Proxy_MapRemoteListForm : Form
    {
        private BindingSource bindingSource = new BindingSource();

        #region//窗体事件

        public Proxy_MapRemoteListForm()
        {
            InitializeComponent();

            this.InitDGV();
        }

        private void Proxy_MapRemoteListForm_Load(object sender, EventArgs e)
        {
            Operate.ProxyConfig.ProxyMapping.IsShow_MapRemote = true;
        }

        private void Proxy_MapRemoteListForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Operate.ProxyConfig.ProxyMapping.IsShow_MapRemote = false;
        }

        #endregion

        #region//初始化DGV

        private void InitDGV()
        {
            bindingSource.DataSource = Operate.ProxyConfig.ProxyMapping.lstMapRemote;
            dgvMapRemote.AutoGenerateColumns = false;
            dgvMapRemote.DataSource = bindingSource;
            dgvMapRemote.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvMapRemote, true, null);
        }

        private void dgvMapRemote_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dgvMapRemote.Columns["cFromURL"].Index)
                {
                    string ProtocolType_From = dgvMapRemote.Rows[e.RowIndex].Cells["cProtocolType_From"].Value?.ToString()?.ToLower();
                    string Host_From = dgvMapRemote.Rows[e.RowIndex].Cells["cHost_From"].Value?.ToString();
                    string Port_From = dgvMapRemote.Rows[e.RowIndex].Cells["cPort_From"].Value?.ToString();
                    string Path_From = dgvMapRemote.Rows[e.RowIndex].Cells["cPath_From"].Value?.ToString();

                    if (!string.IsNullOrEmpty(ProtocolType_From) && !string.IsNullOrEmpty(Host_From) && !string.IsNullOrEmpty(Port_From))
                    {
                        string FromURL = ProtocolType_From + "://" + Host_From + ":" + Port_From + Path_From;
                        e.Value = FromURL;
                        e.FormattingApplied = true;
                    }
                }
                else if (e.ColumnIndex == dgvMapRemote.Columns["cToURL"].Index)
                {
                    string ProtocolType_To = dgvMapRemote.Rows[e.RowIndex].Cells["cProtocolType_To"].Value?.ToString()?.ToLower();
                    string Host_To = dgvMapRemote.Rows[e.RowIndex].Cells["cHost_To"].Value?.ToString();
                    string Port_To = dgvMapRemote.Rows[e.RowIndex].Cells["cPort_To"].Value?.ToString();
                    string Path_To = dgvMapRemote.Rows[e.RowIndex].Cells["cPath_To"].Value?.ToString();

                    if (!string.IsNullOrEmpty(ProtocolType_To) && !string.IsNullOrEmpty(Host_To) && !string.IsNullOrEmpty(Port_To))
                    {
                        string ToURL = ProtocolType_To + "://" + Host_To + ":" + Port_To + Path_To;
                        e.Value = ToURL;
                        e.FormattingApplied = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void dgvMapRemote_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int iSelectIndex = this.dgvMapRemote.SelectedRows[0].Index;
                if (iSelectIndex >= 0 && iSelectIndex < Operate.ProxyConfig.ProxyMapping.lstMapRemote.Count)
                {
                    Socket_Operation.ShowProxyMapRemoteForm(Operate.ProxyConfig.ProxyMapping.lstMapRemote[iSelectIndex]);
                    bindingSource.ResetBindings(false);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void dgvMapRemote_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgvMapRemote.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn && e.RowIndex >= 0)
                {
                    bool bCheck = Convert.ToBoolean(dgvMapRemote.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
                    dgvMapRemote.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = !bCheck;
                    Operate.ProxyConfig.ProxyMapping.lstMapRemote[e.RowIndex].IsEnable = !bCheck;
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
            Socket_Operation.ShowProxyMapRemoteForm(null);
            bindingSource.ResetBindings(false);
        }

        #endregion

        #region//移除

        private void bRemove_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvMapRemote.Rows.Count > 0 && this.dgvMapRemote.SelectedRows.Count > 0)
                {
                    int iSelectIndex = this.dgvMapRemote.SelectedRows[0].Index;
                    if (iSelectIndex >= 0 && iSelectIndex < Operate.ProxyConfig.ProxyMapping.lstMapRemote.Count)
                    {
                        Operate.ProxyConfig.ProxyMapping.DelMapRemote(Operate.ProxyConfig.ProxyMapping.lstMapRemote[iSelectIndex]);
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

        private void cmsMapRemote_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string sItemText = e.ClickedItem.Name;
            cmsMapRemote.Close();

            try
            {
                Proxy_MapRemote pmr = null;
                if (this.dgvMapRemote.Rows.Count > 0 && this.dgvMapRemote.SelectedRows.Count > 0)
                {
                    int iSelectIndex = this.dgvMapRemote.SelectedRows[0].Index;
                    if (iSelectIndex >= 0 && iSelectIndex < Operate.ProxyConfig.ProxyMapping.lstMapRemote.Count)
                    {
                        pmr = Operate.ProxyConfig.ProxyMapping.lstMapRemote[iSelectIndex];
                    }
                }

                switch (sItemText)
                {
                    case "cmsMapRemote_Top":

                        if (pmr != null)
                        {
                            Operate.ProxyConfig.ProxyMapping.UpdateMapRemote_ByListAction(Operate.SystemConfig.ListAction.Top, pmr);
                        }

                        break;

                    case "cmsMapRemote_Up":

                        if (pmr != null)
                        {
                            Operate.ProxyConfig.ProxyMapping.UpdateMapRemote_ByListAction(Operate.SystemConfig.ListAction.Up, pmr);
                        }

                        break;

                    case "cmsMapRemote_Down":

                        if (pmr != null)
                        {
                            Operate.ProxyConfig.ProxyMapping.UpdateMapRemote_ByListAction(Operate.SystemConfig.ListAction.Down, pmr);
                        }

                        break;

                    case "cmsMapRemote_Bottom":

                        if (pmr != null)
                        {
                            Operate.ProxyConfig.ProxyMapping.UpdateMapRemote_ByListAction(Operate.SystemConfig.ListAction.Bottom, pmr);
                        }

                        break;

                    case "cmsMapRemote_Import":

                        Operate.ProxyConfig.ProxyMapping.UpdateMapRemote_ByListAction(Operate.SystemConfig.ListAction.Import, pmr);
                        bindingSource.ResetBindings(false);

                        break;

                    case "cmsMapRemote_Export":

                        if (this.dgvMapRemote.Rows.Count > 0)
                        {
                            Operate.ProxyConfig.ProxyMapping.UpdateMapRemote_ByListAction(Operate.SystemConfig.ListAction.Export, pmr);
                        }

                        break;

                    case "cmsMapRemote_Clear":

                        if (this.dgvMapRemote.Rows.Count > 0)
                        {
                            Operate.ProxyConfig.ProxyMapping.UpdateMapRemote_ByListAction(Operate.SystemConfig.ListAction.CleanUp, pmr);
                            bindingSource.ResetBindings(false);
                        }

                        break;
                }

                this.dgvMapRemote.ClearSelection();

                int iIndex = Operate.ProxyConfig.ProxyMapping.lstMapRemote.IndexOf(pmr);
                if (iIndex > -1 && iIndex < this.dgvMapRemote.RowCount)
                {
                    this.dgvMapRemote.Rows[iIndex].Selected = true;
                    dgvMapRemote.FirstDisplayedScrollingRowIndex = iIndex;
                }

                this.dgvMapRemote.Refresh();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion
    }
}
