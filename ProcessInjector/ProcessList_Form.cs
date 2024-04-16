using System;
using System.Windows.Forms;
using System.Data;
using System.IO;
using System.Drawing;
using ProcessInjector.Lib;
using WPELibrary.Lib;

namespace ProcessInjector
{
    public partial class ProcessList_Form : Form
    {
        private string sLanguage = "";

        #region//窗体加载
        public ProcessList_Form()
        {           
            InitializeComponent();
                        
            MultiLanguage.LoadLanguage(this, typeof(ProcessList_Form));
        }

        private void ProcessList_Form_Load(object sender, EventArgs e)
        {
            this.ShowProcess();
        }
        #endregion

        #region//显示所有进程
        public void ShowProcess()
        {
            dgvProcessList.DataSource = Process_Injector.GetProcess();
        }
        #endregion

        #region//选中某个进程
        private void bSelected_Click(object sender, EventArgs e)
        {
            if (dgvProcessList.SelectedRows.Count == 1)
            {
                Program.PID = (int)dgvProcessList.SelectedRows[0].Cells["cProcessID"].Value;
                Program.PNAME = dgvProcessList.SelectedRows[0].Cells["cProcessName"].Value.ToString();

                this.Close();
            }
            else
            {
                sLanguage = MultiLanguage.GetDefaultLanguage("请选择一个进程", "Please select a process");
                Socket_Operation.ShowMessageBox(sLanguage + "！");
            }
        }
        #endregion

        #region//刷新进程列表
        private void bRefresh_Click(object sender, EventArgs e)
        {
            DataTable dtClear = (DataTable)dgvProcessList.DataSource;
            dtClear.Rows.Clear();
            dgvProcessList.DataSource = dtClear;

            this.ShowProcess();
        }
        #endregion

        #region//选择程序
        private void bCreate_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdCreate = new OpenFileDialog();

            sLanguage = MultiLanguage.GetDefaultLanguage("请选择要注入的应用程序", "Please select the program to inject");
            ofdCreate.Title = sLanguage;
            ofdCreate.Multiselect = false;
            ofdCreate.InitialDirectory = "";
            sLanguage = MultiLanguage.GetDefaultLanguage("应用程序|*.exe|所有文件|*.*", "Program|*.exe|All Files|*.*");
            ofdCreate.Filter = sLanguage;
            ofdCreate.ShowDialog();

            Program.PID = -1;
            Program.PATH = ofdCreate.FileName;
            Program.PNAME = Path.GetFileName(Program.PATH);
            base.Close();
        }
        #endregion

        #region//列表样式
        private void dgvProcessList_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            dgvProcessList.ClearSelection();
        }

        private void dgvProcessList_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.RowIndex != -1 && e.ColumnIndex != -1)
                {
                    this.dgvProcessList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromName("Control");
                }
            }
        }

        private void dgvProcessList_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.RowIndex != -1 && e.ColumnIndex != -1)
                {
                    this.dgvProcessList.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromName("Window");
                }
            }
        }
        #endregion        
    }
}
