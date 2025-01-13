using System;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Drawing;
using WPELibrary.Lib;
using System.Reflection;
using System.Threading.Tasks;

namespace WinsockPacketEditor
{
    public partial class ProcessList_Form : Form
    {
        #region//窗体加载

        public ProcessList_Form()
        {           
            InitializeComponent();
            this.InitDGV();            
        }

        private void InitDGV()
        {
            dgvProcessList.AutoGenerateColumns = false;
            dgvProcessList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvProcessList, true, null);
        }

        private async void ProcessList_Form_Load(object sender, EventArgs e)
        {
            await this.ShowProcessList();
        }

        #endregion      

        #region//显示所有进程（异步）

        private async Task ShowProcessList()
        {
            try
            {
                this.bCreate.Enabled = false;
                this.bRefresh.Enabled = false;
                this.bSelected.Enabled = false;
                this.txtProcessSearch.Enabled = false;

                this.pbLoading.Visible = true;
                this.dgvProcessList.Visible = false;

                this.dgvProcessList.DataSource = await Socket_Operation.GetProcess();

                this.bCreate.Enabled = true;
                this.bRefresh.Enabled = true;
                this.bSelected.Enabled = true;
                this.txtProcessSearch.Enabled = true;

                this.pbLoading.Visible = false;
                this.dgvProcessList.Visible = true;             
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//选中某个进程

        private void bSelected_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvProcessList.SelectedRows.Count == 1)
                {
                    Program.PID = (int)dgvProcessList.SelectedRows[0].Cells["cProcessID"].Value;
                    Program.PNAME = dgvProcessList.SelectedRows[0].Cells["cProcessName"].Value.ToString();

                    this.Close();
                }
                else
                {                    
                    Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_13));
                }
            }
            catch
            {
                //
            }
        }

        #endregion

        #region//刷新进程列表

        private async void bRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtClear = (DataTable)dgvProcessList.DataSource;
                dtClear.Rows.Clear();
                dgvProcessList.DataSource = dtClear;

                this.txtProcessSearch.Text = string.Empty;
                await this.ShowProcessList();
            }
            catch
            {
                //
            }
        }

        #endregion

        #region//选择程序

        private void bCreate_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofdCreate = new OpenFileDialog();
                                
                ofdCreate.Title = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_14);
                ofdCreate.Multiselect = false;
                ofdCreate.InitialDirectory = string.Empty;                                
                ofdCreate.Filter = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_15);
                ofdCreate.ShowDialog();

                Program.PID = -1;
                Program.PATH = ofdCreate.FileName;
                Program.PNAME = Path.GetFileName(Program.PATH);
                base.Close();
            }
            catch
            {
                //
            }
        }

        #endregion

        #region//列表样式

        private void dgvProcessList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvProcessList.ClearSelection();
        }

        private void dgvProcessList_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    if (e.RowIndex != -1 && e.ColumnIndex != -1)
                    {
                        this.dgvProcessList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromName("Control");
                    }
                }
            }
            catch
            {
                //
            }            
        }

        private void dgvProcessList_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    if (e.RowIndex != -1 && e.ColumnIndex != -1)
                    {
                        this.dgvProcessList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromName("Window");
                    }
                }
            }
            catch
            {
                //
            }
        }

        #endregion

        #region//筛选进程

        private void txtProcessSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string sSearchText = this.txtProcessSearch.Text.Trim();

                if (String.IsNullOrEmpty(sSearchText))
                {
                    dgvProcessList.DataSource = Socket_Operation.ProcessTable;
                }
                else
                {
                    DataView dvSearch = new DataView(Socket_Operation.ProcessTable);
                    dvSearch.RowFilter = "PName like '" + sSearchText + "%'";
                    dgvProcessList.DataSource = dvSearch.ToTable();
                }
            }
            catch
            {
                //
            }                    
        }

        #endregion
    }
}
