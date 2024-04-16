using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using WPELibrary.Lib;
using EasyHook;

namespace WPELibrary
{
    public partial class Filter_Form : Form
    {
        public int Filter_Index = 0;

        private int iSearchRowIndex = 0;
        private int iModifyRowIndex = 1;
        private int iColNum = 500;
        private string sLanguage = "";

        #region//窗体加载
        public Filter_Form()
        {
            InitializeComponent();
            this.Init_dgvFilter();
        }

        private void Filter_Form_Load(object sender, EventArgs e)
        {
            string sInjectProcesName = Process.GetCurrentProcess().ProcessName;
            int iFilterIndex = Filter_Index + 1;
            int iInjectProcessID = RemoteHooking.GetCurrentProcessId();

            sLanguage = MultiLanguage.GetDefaultLanguage("滤镜 -【 序号 ", "Filter -【 Num ");

            this.Text = sLanguage + iFilterIndex + " 】- " + sInjectProcesName + " [" + iInjectProcessID.ToString() + "]";

            this.txtFilterName.Text = Filter_List.dtFilterList.Rows[Filter_Index]["FilterName"].ToString().Trim();

            this.SetFilterInfo(Filter_Index);            
        }
        #endregion

        #region//初始化
        private void Init_dgvFilter()
        {
            for (int i = 0; i < iColNum; i++)
            {
                DataGridViewTextBoxColumn dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn()
                {
                    Name = "col" + (i + 1).ToString("000"),
                    HeaderText = (i + 1).ToString("000"),
                    Width = 40,
                    MaxInputLength = 2
                };

                DataGridViewTextBoxColumn dgvColumn = dataGridViewTextBoxColumn;
                dgvColumn.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                dgvColumn.DefaultCellStyle.ForeColor = Color.RoyalBlue;

                dgvFilter.Columns.Add(dgvColumn);
            }            

            if (dgvFilter.Rows.Count == 0)
            {
                dgvFilter.RowHeadersWidth = 100;

                dgvFilter.Rows.Add();
                dgvFilter.Rows.Add();

                sLanguage = MultiLanguage.GetDefaultLanguage("搜索封包", "Search");
                dgvFilter.Rows[iSearchRowIndex].HeaderCell.Value = sLanguage;

                sLanguage = MultiLanguage.GetDefaultLanguage("修改封包", "Modify");
                dgvFilter.Rows[iModifyRowIndex].HeaderCell.Value = sLanguage;
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

            this.txtFilterName.Text = Filter_List.dtFilterList.Rows[Filter_Index]["FilterName"].ToString().Trim();
        }
        #endregion

        #region//应用按钮
        private void bApply_Click(object sender, EventArgs e)
        {           
            string sSearchValue, sModifyValue;
            string sAllpy_SearchValue = "";
            string sApply_ModifyValue = "";
            string sApply_FilterName = this.txtFilterName.Text.Trim();

            if (string.IsNullOrEmpty(sApply_FilterName))
            {
                sLanguage = MultiLanguage.GetDefaultLanguage("滤镜名称不能为空！", "Filter name cannot be empty!");
                Socket_Operation.ShowMessageBox(sLanguage);
                return;
            }

            for (int i = 0; i < this.dgvFilter.Columns.Count; i++)
            {
                if (dgvFilter.Rows[iSearchRowIndex].Cells[i].Value != null)
                {
                    sSearchValue = dgvFilter.Rows[iSearchRowIndex].Cells[i].Value.ToString().Trim();

                    if (!String.IsNullOrEmpty(sSearchValue))
                    {
                        sAllpy_SearchValue += i.ToString() + "|" + sSearchValue + ",";
                    }
                }

                if (dgvFilter.Rows[iModifyRowIndex].Cells[i].Value != null)
                {
                    sModifyValue = dgvFilter.Rows[iModifyRowIndex].Cells[i].Value.ToString().Trim();

                    if (!String.IsNullOrEmpty(sModifyValue))
                    {
                        sApply_ModifyValue += i.ToString() + "|" + sModifyValue + ",";
                    }
                }
            }

            sAllpy_SearchValue = sAllpy_SearchValue.TrimEnd(',');
            sApply_ModifyValue = sApply_ModifyValue.TrimEnd(',');

            Filter_List.dtFilterList.Rows[Filter_Index]["FilterName"] = sApply_FilterName;
            Filter_List.dtFilterList.Rows[Filter_Index]["FilterSearch"] = sAllpy_SearchValue;
            Filter_List.dtFilterList.Rows[Filter_Index]["FilterModify"] = sApply_ModifyValue;

            this.Close();
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

        #region//设置滤镜内容
        private void SetFilterInfo(int iFilterIndex)
        {
            string sSearch = Filter_List.dtFilterList.Rows[Filter_Index]["FilterSearch"].ToString().Trim();
            string sModify = Filter_List.dtFilterList.Rows[Filter_Index]["FilterModify"].ToString().Trim();

            if (!sSearch.Equals("") && !sModify.Equals(""))
            {
                string[] sSearchAll = sSearch.Split(',');
                foreach (string sText in sSearchAll)
                {
                    string[] sSplitText = sText.Split('|');
                    int iIndex = int.Parse(sSplitText[0].ToString().Trim());
                    string sValue = sSplitText[1].ToString().Trim();
                    this.dgvFilter.Rows[iSearchRowIndex].Cells[iIndex].Value = sValue;                    
                }

                string[] sModifyAll = sModify.Split(',');
                foreach (string sText in sModifyAll)
                {
                    string[] sSplitText = sText.Split('|');
                    int iIndex = int.Parse(sSplitText[0].ToString().Trim());
                    string sValue = sSplitText[1].ToString().Trim();
                    this.dgvFilter.Rows[iModifyRowIndex].Cells[iIndex].Value = sValue;
                }
            }
        }

        #endregion
    }
}
