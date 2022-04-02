using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Data;
using System.IO;
using WPELibrary.Lib;

namespace ProcessInjector
{
    public partial class ProcessList_Form : Form
    {
        #region//窗体加载
        public ProcessList_Form()
        {
            InitializeComponent();            

            this.GetProcess();
        }
        #endregion

        #region//获取所有进程
        public void GetProcess()
        {
            DataTable dtProcess = new DataTable();
            dtProcess.Columns.Add("ProcessName", typeof(string));
            dtProcess.Columns.Add("PID", typeof(int));
            dtProcess.Columns.Add("RAM", typeof(long));

            lvProcessList.Items.Clear();

            Process[] procesArr = Process.GetProcesses();

            int pCNT = procesArr.Length;            

            foreach (Process p in procesArr)
            {
                DataRow dr = dtProcess.NewRow();
                dr[0] = p.ProcessName;
                dr[1] = p.Id;
                dr[2] = p.PrivateMemorySize64;

                dtProcess.Rows.Add(dr);
            }
            
            DataView dv = dtProcess.DefaultView;
            dv.Sort = "ProcessName";
            dtProcess = dv.ToTable();

            foreach (DataRow drSort in dtProcess.Rows)
            {
                ListViewItem obj = new ListViewItem();
                obj.Text = drSort[0].ToString();
                obj.SubItems.Add(drSort[1].ToString());
                obj.SubItems.Add(drSort[2].ToString());
                lvProcessList.Items.Add(obj);
            }            

            lProcessCNT.Text = "进程数：" + pCNT.ToString();
        }
        #endregion

        #region//选中某个进程
        private void bSelected_Click(object sender, EventArgs e)
        {
            if (lvProcessList.SelectedItems.Count == 1)
            {
                Program.PID = int.Parse(lvProcessList.SelectedItems[0].SubItems[1].Text.ToString().Trim());
                Program.PNAME = lvProcessList.SelectedItems[0].SubItems[0].Text.ToString().Trim();

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
            GetProcess();
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
    }
}
