using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data;
using System.IO;
using System.Drawing;
using WPELibrary.Lib;
using System.Threading;

namespace ProcessInjector
{
    public partial class ProcessList_Form : Form
    {
        #region//窗体加载
        public ProcessList_Form()
        {           
            InitializeComponent();            
        }

        private void ProcessList_Form_Load(object sender, EventArgs e)
        {
            this.GetProcess();
        }
        #endregion

        #region//获取所有进程
        public void GetProcess()
        {
            DataTable dtProcessList = new DataTable();
            dtProcessList.Columns.Add("ICO", typeof(Image));
            dtProcessList.Columns.Add("PName", typeof(string));
            dtProcessList.Columns.Add("PID", typeof(int));

            Process[] procesArr = Process.GetProcesses();
            int pCNT = procesArr.Length;

            foreach (Process p in procesArr)
            {
                string sPName = p.ProcessName;
                int iPID = p.Id;
                Image iICO = IconFromFile(p);

                DataRow dr = dtProcessList.NewRow();
                dr["ICO"] = iICO;
                dr["PName"] = sPName;
                dr["PID"] = iPID;
                dtProcessList.Rows.Add(dr);
            }

            DataView dv = dtProcessList.DefaultView;
            dv.Sort = "PName";
            dtProcessList = dv.ToTable();

            dgvProcessList.DataSource = dtProcessList;
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
                Socket_Operation.ShowMessageBox("请选择一个进程！");
            }
        }
        #endregion

        #region//刷新进程列表
        private void bRefresh_Click(object sender, EventArgs e)
        {
            DataTable dtClear = (DataTable)dgvProcessList.DataSource;
            dtClear.Rows.Clear();
            dgvProcessList.DataSource = dtClear;

            GetProcess();
        }
        #endregion

        #region//获取进程的图标
        private Image IconFromFile(Process p)
        {
            string filePath = "";
            Image image = null;

            try
            {
                filePath = p.MainModule.FileName.Replace(".ni.dll", ".dll");
            }
            catch
            {
                filePath = "";
            }

            try
            {
                var extractor = new IconExtractor(filePath);
                var icon = extractor.GetIcon(0);

                Icon[] splitIcons = IconUtil.Split(icon);

                Icon selectedIcon = null;

                foreach (var item in splitIcons)
                {
                    if (selectedIcon == null)
                    {
                        selectedIcon = item;
                    }
                    else
                    {
                        if (IconUtil.GetBitCount(item) > IconUtil.GetBitCount(selectedIcon))
                        {
                            selectedIcon = item;
                        }
                        else if (IconUtil.GetBitCount(item) == IconUtil.GetBitCount(selectedIcon) && item.Width > selectedIcon.Width)
                        {
                            selectedIcon = item;
                        }
                    }
                }

                return selectedIcon.ToBitmap();
            }
            catch (Exception)
            {
                //
            }

            try
            {
                image = Icon.ExtractAssociatedIcon(filePath)?.ToBitmap();
            }
            catch
            {
                image = new Icon(SystemIcons.Application, 256, 256).ToBitmap();
            }

            return image;
        }
        #endregion        

        #region//选择程序
        private void bCreate_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdCreate = new OpenFileDialog();
            ofdCreate.Title = "请选择要注入的应用程序";
            ofdCreate.Multiselect = false;
            ofdCreate.InitialDirectory = "";
            ofdCreate.Filter = "应用程序|*.exe|所有文件|*.*";
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
