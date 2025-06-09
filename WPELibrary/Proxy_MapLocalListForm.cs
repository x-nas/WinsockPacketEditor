using System;
using System.Reflection;
using System.Windows.Forms;
using WPELibrary.Lib;

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
            Socket_Cache.ProxyMapping.IsShow = true;
        }

        private void Proxy_MapLocalForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Socket_Cache.ProxyMapping.IsShow = false;
        }

        #endregion

        #region//初始化DGV

        private void InitDGV()
        {
            bindingSource.DataSource = Socket_Cache.ProxyMapping.lstMapLocal;
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
                    string ProtocolType = dgvMapLocal.Rows[e.RowIndex].Cells["cProtocolType"].Value.ToString().ToLower();
                    string Host = dgvMapLocal.Rows[e.RowIndex].Cells["cHost"].Value.ToString();
                    string Port = dgvMapLocal.Rows[e.RowIndex].Cells["cPort"].Value.ToString();
                    string RemotePath = dgvMapLocal.Rows[e.RowIndex].Cells["cRemotePath"].Value.ToString();
                    string RemoteURL = ProtocolType + "://" + Host + ": " + Port + RemotePath;
                    e.Value = RemoteURL;
                    e.FormattingApplied = true;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void dgvMapLocal_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int iSelectIndex = this.dgvMapLocal.SelectedRows[0].Index;
                if (iSelectIndex >= 0 && iSelectIndex < Socket_Cache.ProxyMapping.lstMapLocal.Count)
                {
                    Socket_Operation.ShowProxyMapLocalForm(Socket_Cache.ProxyMapping.lstMapLocal[iSelectIndex]);
                    bindingSource.ResetBindings(false);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                    Socket_Cache.ProxyMapping.lstMapLocal[e.RowIndex].IsEnable = !bCheck;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog_Proxy(MethodBase.GetCurrentMethod().Name, ex.Message);
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
            int iSelectIndex = this.dgvMapLocal.SelectedRows[0].Index;
            if (iSelectIndex >= 0 && iSelectIndex < Socket_Cache.ProxyMapping.lstMapLocal.Count)
            {
                Socket_Cache.ProxyMapping.DelMapLocal(Socket_Cache.ProxyMapping.lstMapLocal[iSelectIndex]);
                bindingSource.ResetBindings(false);
            }            
        }

        #endregion        

        #region//确定

        private void bOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion
    }
}
