using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using WPELibrary.Lib;
using EasyHook;
using System.Reflection;

namespace WPELibrary
{
    public partial class Socket_Filter_Form : Form
    {
        private int FilterNum = -1;
        private int FilterIndex = -1;

        #region//窗体加载
        public Socket_Filter_Form(int FNum)
        {
            InitializeComponent();

            this.FilterNum = FNum;
            this.FilterIndex = Socket_Cache.SocketFilterList.GetFilterIndex_ByFilterNum(FilterNum);

            this.InitDGV();
        }

        private void Filter_Form_Load(object sender, EventArgs e)
        {
            this.InitForm();
            this.ShowFilterInfo();
        }
        #endregion

        #region//初始化窗体
        private void InitForm()
        {
            int iInjectProcessID = RemoteHooking.GetCurrentProcessId();
            string sInjectProcesName = Process.GetCurrentProcess().ProcessName;
            
            this.Text = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_16) + FilterNum + " 】- " + sInjectProcesName + " [" + iInjectProcessID.ToString() + "]";
            this.txtFilterName.Text = Socket_Cache.SocketFilterList.lstFilter[FilterIndex].FName.ToString();
        }
        #endregion

        #region//初始化数据表
        private void InitDGV()
        {
            dgvFilter.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvFilter, true, null);

            for (int i = 0; i < Socket_Cache.SocketFilterList.FilterLen_MAX; i++)
            {
                DataGridViewTextBoxColumn dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn()
                {
                    Name = "col" + (i + 1).ToString("000"),
                    HeaderText = (i + 1).ToString("000"),
                    Width = 50,
                    MaxInputLength = 2
                };

                DataGridViewTextBoxColumn dgvColumn = dataGridViewTextBoxColumn;
                dgvColumn.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                dgvColumn.DefaultCellStyle.ForeColor = Color.RoyalBlue;

                dgvFilter.Columns.Add(dgvColumn);
            }            

            if (dgvFilter.Rows.Count == 0)
            {
                dgvFilter.RowHeadersWidth = 120;

                dgvFilter.Rows.Add();
                dgvFilter.Rows.Add();
                
                dgvFilter.Rows[Socket_Cache.SocketFilterList.SearchRowIndex].HeaderCell.Value = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_17);
                dgvFilter.Rows[Socket_Cache.SocketFilterList.ModifyRowIndex].HeaderCell.Value = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_18);
            }            
        }
        #endregion

        #region//显示滤镜内容
        private void ShowFilterInfo()
        {
            string sSearch = Socket_Cache.SocketFilterList.lstFilter[FilterIndex].FSearch.ToString().Trim();
            string sModify = Socket_Cache.SocketFilterList.lstFilter[FilterIndex].FModify.ToString().Trim();

            if (!string.IsNullOrEmpty(sSearch))
            {
                string[] sSearchAll = sSearch.Split(',');
                foreach (string sText in sSearchAll)
                {
                    string[] sSplitText = sText.Split('-');
                    int iIndex = int.Parse(sSplitText[0].ToString().Trim());
                    string sValue = sSplitText[1].ToString().Trim();
                    this.dgvFilter.Rows[Socket_Cache.SocketFilterList.SearchRowIndex].Cells[iIndex].Value = sValue;
                }
            }

            if (!string.IsNullOrEmpty(sModify))
            {
                string[] sModifyAll = sModify.Split(',');
                foreach (string sText in sModifyAll)
                {
                    string[] sSplitText = sText.Split('-');
                    int iIndex = int.Parse(sSplitText[0].ToString().Trim());
                    string sValue = sSplitText[1].ToString().Trim();
                    this.dgvFilter.Rows[Socket_Cache.SocketFilterList.ModifyRowIndex].Cells[iIndex].Value = sValue;
                }
            }
        }

        #endregion

        #region//重置按钮
        private void bReset_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dgvFilter.Rows.Count; i++)
            {
                for (int j = 0; j < this.dgvFilter.Columns.Count; j++)
                {
                    dgvFilter.Rows[i].Cells[j].Value = "";
                }
            }

            this.txtFilterName.Text = Socket_Cache.SocketFilterList.lstFilter[FilterIndex].FName.ToString();
        }
        #endregion

        #region//应用按钮
        private void bApply_Click(object sender, EventArgs e)
        {  
            string sSearch_New = "";
            string sModify_New = "";

            string sFName_New = this.txtFilterName.Text.Trim();

            if (!string.IsNullOrEmpty(sFName_New))
            {
                for (int i = 0; i < this.dgvFilter.Columns.Count; i++)
                {
                    string sSearchValue;

                    try
                    {
                        sSearchValue = dgvFilter.Rows[Socket_Cache.SocketFilterList.SearchRowIndex].Cells[i].Value.ToString().Trim();
                    }
                    catch
                    {
                        sSearchValue = "";
                    }

                    if (!String.IsNullOrEmpty(sSearchValue))
                    {
                        sSearch_New += i.ToString() + "-" + sSearchValue + ",";
                    }

                    string sModifyValue;

                    try
                    {
                        sModifyValue = dgvFilter.Rows[Socket_Cache.SocketFilterList.ModifyRowIndex].Cells[i].Value.ToString().Trim();
                    }
                    catch
                    {
                        sModifyValue = "";
                    }

                    if (!String.IsNullOrEmpty(sModifyValue))
                    {
                        sModify_New += i.ToString() + "-" + sModifyValue + ",";
                    }                 
                }

                sSearch_New = sSearch_New.TrimEnd(',');
                sModify_New = sModify_New.TrimEnd(',');

                Socket_Cache.SocketFilterList.UpdateFilter_ByFilterNum(FilterNum, sFName_New, sSearch_New, sModify_New);             

                this.Close();
            }
            else
            {                
                Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_19));                
            }            
        }
        #endregion

        #region//黏贴封包数据
        private void dgvFilterList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                string sClipboardText = Clipboard.GetText().Trim();
                ShowSocketSendData(sClipboardText);
            }
        }        

        private void ShowSocketSendData(string sData)
        {
            int iRow = dgvFilter.CurrentCell.RowIndex;
            int iCol = dgvFilter.CurrentCell.ColumnIndex;

            string[] DataCells = sData.Split(' ');
            for (int i = 0; i < DataCells.Length; i++)
            {
                if (iCol + i < this.dgvFilter.ColumnCount)
                {
                    dgvFilter[iCol + i, iRow].Value = Convert.ChangeType(DataCells[i], dgvFilter[iCol + i, iRow].ValueType);
                }
                else
                {
                    break;
                }
            }
        }
        #endregion        
    }
}
