using System;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using WPELibrary.Lib;
using EasyHook;
using System.Reflection;

namespace WPELibrary
{
    public partial class Socket_FilterForm : Form
    {
        private int FilterNum = -1;
        private int FilterIndex = -1;
        private int FilterModifyCNT = 0;
        private string FilterName = string.Empty;
        private string FilterHeaderContent = string.Empty;
        private string FilterSearch = string.Empty;
        private string FilterModify = string.Empty;
        private bool FilterAppointHeader = false;
        private Socket_Cache.Filter.FilterMode FilterMode;
        private Socket_Cache.Filter.FilterAction FilterAction;
        private Socket_Cache.Filter.FilterFunction FilterFunction;        
        private Socket_Cache.Filter.FilterStartFrom FilterStartFrom;

        #region//窗体加载

        public Socket_FilterForm(int FNum)
        {
            try
            {
                MultiLanguage.SetDefaultLanguage(MultiLanguage.DefaultLanguage);
                InitializeComponent();

                this.FilterNum = FNum;

                this.InitFrom();
                this.InitDGV();
                this.ShowFilterInfo();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//初始化

        private void InitFrom()
        {
            try
            {
                int iInjectProcessID = RemoteHooking.GetCurrentProcessId();
                string sInjectProcesName = Process.GetCurrentProcess().ProcessName;
                this.Text = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_16) + FilterNum + " 】- " + sInjectProcesName + " [" + iInjectProcessID.ToString() + "]";

                this.FilterIndex = Socket_Cache.FilterList.GetFilterIndex_ByFilterNum(FilterNum);

                if (FilterIndex >= 0)
                {
                    this.FilterName = Socket_Cache.FilterList.lstFilter[FilterIndex].FName;
                    this.FilterAppointHeader = Socket_Cache.FilterList.lstFilter[FilterIndex].AppointHeader;
                    this.FilterHeaderContent = Socket_Cache.FilterList.lstFilter[FilterIndex].HeaderContent;
                    this.FilterMode = Socket_Cache.FilterList.lstFilter[FilterIndex].FMode;
                    this.FilterAction = Socket_Cache.FilterList.lstFilter[FilterIndex].FAction;
                    this.FilterFunction = Socket_Cache.FilterList.lstFilter[FilterIndex].FFunction;
                    this.FilterStartFrom = Socket_Cache.FilterList.lstFilter[FilterIndex].FStartFrom;
                    this.FilterModifyCNT = Socket_Cache.FilterList.lstFilter[FilterIndex].FModifyCNT;
                    this.FilterSearch = Socket_Cache.FilterList.lstFilter[FilterIndex].FSearch;
                    this.FilterModify = Socket_Cache.FilterList.lstFilter[FilterIndex].FModify;                    

                    switch (FilterMode)
                    {
                        case Socket_Cache.Filter.FilterMode.Normal:
                            this.rbFilterMode_Normal.Checked = true;
                            break;

                        case Socket_Cache.Filter.FilterMode.Advanced:
                            this.rbFilterMode_Advanced.Checked = true;
                            break;
                    }
                    this.FilterModeChange();

                    switch (FilterAction)
                    {
                        case Socket_Cache.Filter.FilterAction.Replace:
                            this.rbFilterAction_Replace.Checked = true;
                            break;

                        case Socket_Cache.Filter.FilterAction.Intercept:
                            this.rbFilterAction_Intercept.Checked = true;
                            break;
                    }

                    switch (FilterStartFrom)
                    {
                        case Socket_Cache.Filter.FilterStartFrom.Head:
                            this.rbFilterModifyFrom_Head.Checked = true;
                            break;

                        case Socket_Cache.Filter.FilterStartFrom.Position:
                            this.rbFilterModifyFrom_Position.Checked = true;
                            break;
                    }
                    this.FilterModifyFromChange();

                    this.cbFilter_AppointHeader.Checked = FilterAppointHeader;
                    this.FilterAppointHeaderChange();

                    this.txtFilterName.Text = FilterName;                    
                    this.txtFilter_HeaderContent.Text = FilterHeaderContent;
                    
                    this.nudFilterSet_ModifyTimes.Value = FilterModifyCNT;
                    
                    this.cbFilterFunction_Send.Checked = FilterFunction.Send;
                    this.cbFilterFunction_SendTo.Checked = FilterFunction.SendTo;
                    this.cbFilterFunction_Recv.Checked = FilterFunction.Recv;
                    this.cbFilterFunction_RecvFrom.Checked = FilterFunction.RecvFrom;
                    this.cbFilterFunction_WSASend.Checked = FilterFunction.WSASend;
                    this.cbFilterFunction_WSASendTo.Checked = FilterFunction.WSASendTo;
                    this.cbFilterFunction_WSARecv.Checked = FilterFunction.WSARecv;
                    this.cbFilterFunction_WSARecvFrom.Checked = FilterFunction.WSARecvFrom;
                }
                else
                {
                    Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_28));
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void InitDGV_Normal()
        {
            try
            {
                this.dgvFilterNormal.Columns.Clear();

                for (int i = 0; i < Socket_Cache.Filter.FilterSize_MaxLen; i++)
                {
                    DataGridViewTextBoxColumn dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn()
                    {
                        Name = "col" + (i + 1).ToString("000"),
                        HeaderText = (i + 1).ToString("000"),
                        Width = 50,
                        MaxInputLength = 2
                    };

                    DataGridViewTextBoxColumn dgvColumn = dataGridViewTextBoxColumn;
                    dgvColumn.DefaultCellStyle.ForeColor = Color.RoyalBlue;
                    dgvColumn.DefaultCellStyle.BackColor = Color.LightYellow;
                    dgvFilterNormal.Columns.Add(dgvColumn);                 
                }

                if (dgvFilterNormal.Rows.Count == 0)
                {
                    dgvFilterNormal.Rows.Add();
                    dgvFilterNormal.Rows.Add();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void InitDGV_Advanced_Search()
        {
            try
            {
                this.dgvFilterAdvanced_Search.Columns.Clear();

                for (int i = 0; i < Socket_Cache.Filter.FilterSize_MaxLen; i++)
                {
                    DataGridViewTextBoxColumn dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn()
                    {
                        Name = "col" + (i + 1).ToString("000"),
                        HeaderText = (i + 1).ToString("000"),
                        Width = 50,
                        MaxInputLength = 2
                    };

                    DataGridViewTextBoxColumn dgvColumn = dataGridViewTextBoxColumn;
                    dgvColumn.DefaultCellStyle.ForeColor = Color.RoyalBlue;
                    dgvColumn.DefaultCellStyle.BackColor = Color.LightYellow;
                    dgvFilterAdvanced_Search.Columns.Add(dgvColumn);
                }

                if (dgvFilterAdvanced_Search.Rows.Count == 0)
                {
                    dgvFilterAdvanced_Search.Rows.Add();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void InitDGV_Advanced_Modify_Head()
        {
            try
            {
                this.dgvFilterAdvanced_Modify_FromHead.Columns.Clear();                

                for (int i = 0; i < Socket_Cache.Filter.FilterSize_MaxLen; i++)
                {
                    DataGridViewTextBoxColumn dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn()
                    {
                        Name = "col" + (i + 1).ToString("000"),
                        HeaderText = (i + 1).ToString("000"),
                        Width = 50,
                        MaxInputLength = 2
                    };

                    DataGridViewTextBoxColumn dgvColumn = dataGridViewTextBoxColumn;
                    dgvColumn.DefaultCellStyle.ForeColor = Color.RoyalBlue;
                    dgvColumn.DefaultCellStyle.BackColor = Color.Yellow;
                    dgvFilterAdvanced_Modify_FromHead.Columns.Add(dgvColumn);
                }

                if (dgvFilterAdvanced_Modify_FromHead.Rows.Count == 0)
                {
                    dgvFilterAdvanced_Modify_FromHead.Rows.Add();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void InitDGV_Advanced_Modify_Position()
        {
            try
            {
                this.dgvFilterAdvanced_Modify_FromPosition.Columns.Clear();

                int iSize = Socket_Cache.Filter.FilterSize_MaxLen / 2;

                for (int i = -iSize; i <= iSize; i++)
                {
                    DataGridViewTextBoxColumn dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn()
                    {
                        Name = "col" + i.ToString("000"),
                        HeaderText = i.ToString("000"),
                        Width = 50,
                        MaxInputLength = 2
                    };

                    DataGridViewTextBoxColumn dgvColumn = dataGridViewTextBoxColumn;
                    dgvColumn.DefaultCellStyle.ForeColor = Color.RoyalBlue;
                    dgvColumn.DefaultCellStyle.BackColor = Color.Yellow;
                    dgvFilterAdvanced_Modify_FromPosition.Columns.Add(dgvColumn);
                }

                if (dgvFilterAdvanced_Modify_FromPosition.Rows.Count == 0)
                {
                    dgvFilterAdvanced_Modify_FromPosition.Rows.Add();
                }

                foreach (DataGridViewColumn column in dgvFilterAdvanced_Modify_FromPosition.Columns)
                {
                    if (column.Name == "col000")
                    {
                        dgvFilterAdvanced_Modify_FromPosition.CurrentCell = dgvFilterAdvanced_Modify_FromPosition.Rows[0].Cells[column.Index];
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void InitDGV()
        {
            try
            {
                dgvFilterNormal.AutoGenerateColumns = false;
                dgvFilterNormal.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvFilterNormal, true, null);

                dgvFilterAdvanced_Search.AutoGenerateColumns = false;
                dgvFilterAdvanced_Search.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvFilterAdvanced_Search, true, null);

                dgvFilterAdvanced_Modify_FromHead.AutoGenerateColumns = false;
                dgvFilterAdvanced_Modify_FromHead.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvFilterAdvanced_Modify_FromHead, true, null);

                dgvFilterAdvanced_Modify_FromPosition.AutoGenerateColumns = false;
                dgvFilterAdvanced_Modify_FromPosition.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvFilterAdvanced_Modify_FromPosition, true, null);

                this.InitDGV_Normal();
                this.InitDGV_Advanced_Search();
                this.InitDGV_Advanced_Modify_Head();
                this.InitDGV_Advanced_Modify_Position();

                this.dgvFilterAdvanced_Search.Height = 85;
                this.dgvFilterAdvanced_Modify_FromHead.Height = 85;
                this.dgvFilterAdvanced_Modify_FromPosition.Height = 85;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }        

        #endregion

        #region//模式切换

        private void rbFilterMode_CheckedChanged(object sender, EventArgs e)
        {
            this.FilterModeChange();
        }

        private void FilterModeChange()
        {
            try
            {
                if (rbFilterMode_Normal.Checked)
                {
                    this.tpNormal.Parent = this.tcFilterInfo;
                    this.tpAdvanced.Parent = null;

                    this.gbFilterModifyFrom.Enabled = false;
                    this.gbFilterSet.Enabled = false;
                }
                else if (rbFilterMode_Advanced.Checked)
                {
                    this.tpNormal.Parent = null;
                    this.tpAdvanced.Parent = this.tcFilterInfo;

                    this.gbFilterModifyFrom.Enabled = true;

                    if (rbFilterModifyFrom_Position.Checked)
                    {
                        this.gbFilterSet.Enabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//修改起始于切换

        private void rbFilterModifyFrom_CheckedChanged(object sender, EventArgs e)
        {
            this.FilterModifyFromChange();
        }

        private void FilterModifyFromChange()
        {
            try
            {
                if (rbFilterModifyFrom_Head.Checked)
                {
                    this.gbFilterSet.Enabled = false;

                    this.dgvFilterAdvanced_Modify_FromHead.Visible = true;
                    this.dgvFilterAdvanced_Modify_FromPosition.Visible = false;
                }
                else if (rbFilterModifyFrom_Position.Checked)
                {
                    this.gbFilterSet.Enabled = true;

                    this.dgvFilterAdvanced_Modify_FromHead.Visible = false;
                    this.dgvFilterAdvanced_Modify_FromPosition.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//高级指定

        private void txtFilter_HeaderContent_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (!Socket_Operation.CheckTextInput_IsHex(e.KeyChar))
                {
                    e.Handled = true;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void cbFilter_AppointHeader_CheckedChanged(object sender, EventArgs e)
        {
            this.FilterAppointHeaderChange();
        }

        private void FilterAppointHeaderChange()
        {
            try
            {
                if (this.cbFilter_AppointHeader.Checked)
                {
                    this.txtFilter_HeaderContent.Enabled = true;
                }
                else
                {
                    this.txtFilter_HeaderContent.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//显示滤镜内容

        private void ShowFilterInfo()
        {
            try
            {
                if (!string.IsNullOrEmpty(FilterSearch))
                {
                    string[] sSearchAll = FilterSearch.Split(',');

                    foreach (string s in sSearchAll)
                    {
                        int iIndex = int.Parse(s.Split('-')[0]);
                        string sValue = s.Split('-')[1];

                        if (this.dgvFilterNormal.Rows.Count == 2)
                        {
                            if (iIndex < this.dgvFilterNormal.Rows[0].Cells.Count)
                            {
                                this.dgvFilterNormal.Rows[0].Cells[iIndex].Value = sValue;
                            }
                        }

                        if (this.dgvFilterAdvanced_Search.Rows.Count == 1)
                        {
                            if (iIndex < this.dgvFilterAdvanced_Search.Rows[0].Cells.Count)
                            {
                                this.dgvFilterAdvanced_Search.Rows[0].Cells[iIndex].Value = sValue;
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(FilterModify))
                {
                    string[] sModifyAll = FilterModify.Split(',');

                    foreach (string s in sModifyAll)
                    {
                        int iIndex = int.Parse(s.Split('-')[0]);
                        string sValue = s.Split('-')[1];

                        switch (FilterMode)
                        {
                            case Socket_Cache.Filter.FilterMode.Normal:

                                if (this.dgvFilterNormal.Rows.Count == 2)
                                {
                                    if (iIndex < this.dgvFilterNormal.Rows[1].Cells.Count)
                                    {
                                        this.dgvFilterNormal.Rows[1].Cells[iIndex].Value = sValue;
                                    }
                                }

                                break;

                            case Socket_Cache.Filter.FilterMode.Advanced:

                                switch (FilterStartFrom)
                                {
                                    case Socket_Cache.Filter.FilterStartFrom.Head:

                                        if (this.dgvFilterAdvanced_Modify_FromHead.Rows.Count == 1)
                                        {
                                            if (iIndex < this.dgvFilterAdvanced_Modify_FromHead.Rows[0].Cells.Count)
                                            {
                                                this.dgvFilterAdvanced_Modify_FromHead.Rows[0].Cells[iIndex].Value = sValue;
                                            }
                                        }

                                        break;

                                    case Socket_Cache.Filter.FilterStartFrom.Position:

                                        if (this.dgvFilterAdvanced_Modify_FromPosition.Rows.Count == 1)
                                        {
                                            if (iIndex < this.dgvFilterAdvanced_Modify_FromPosition.Rows[0].Cells.Count)
                                            {
                                                this.dgvFilterAdvanced_Modify_FromPosition.Rows[0].Cells[iIndex].Value = sValue;
                                            }
                                        }  

                                        break;
                                }                            

                                break;
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void DGV_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.Value != null)
                {
                    e.Value = e.Value.ToString().ToUpper();
                    e.FormattingApplied = true;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//关闭按钮

        private void bFilterButton_Close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region//保存按钮       

        private void bFilterButton_Save_Click(object sender, EventArgs e)
        {
            try
            {
                string sFName_New = string.Empty;
                string sHeaderContent_New = string.Empty;
                string sSearch_New = string.Empty;
                string sModify_New = string.Empty;

                bool bAppointHeader;

                Socket_Cache.Filter.FilterMode FilterMode_New;
                Socket_Cache.Filter.FilterAction FilterAction_New;
                Socket_Cache.Filter.FilterFunction FilterFunction_New;
                Socket_Cache.Filter.FilterStartFrom FilterStartFrom_New;                

                sFName_New = this.txtFilterName.Text.Trim();

                if (string.IsNullOrEmpty(sFName_New))
                {
                    Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_19));
                    return;
                }

                bAppointHeader = this.cbFilter_AppointHeader.Checked;

                sHeaderContent_New = this.txtFilter_HeaderContent.Text.Trim();

                if (rbFilterMode_Normal.Checked)
                {
                    FilterMode_New = Socket_Cache.Filter.FilterMode.Normal;
                }
                else
                {
                    FilterMode_New = Socket_Cache.Filter.FilterMode.Advanced;
                }

                if (rbFilterAction_Replace.Checked)
                {
                    FilterAction_New = Socket_Cache.Filter.FilterAction.Replace;
                }
                else
                {
                    FilterAction_New = Socket_Cache.Filter.FilterAction.Intercept;
                }

                FilterFunction_New.Send = this.cbFilterFunction_Send.Checked;
                FilterFunction_New.SendTo = this.cbFilterFunction_SendTo.Checked;
                FilterFunction_New.Recv = this.cbFilterFunction_Recv.Checked;
                FilterFunction_New.RecvFrom = this.cbFilterFunction_RecvFrom.Checked;
                FilterFunction_New.WSASend = this.cbFilterFunction_WSASend.Checked;
                FilterFunction_New.WSASendTo = this.cbFilterFunction_WSASendTo.Checked;
                FilterFunction_New.WSARecv = this.cbFilterFunction_WSARecv.Checked;
                FilterFunction_New.WSARecvFrom = this.cbFilterFunction_WSARecvFrom.Checked;

                if (rbFilterModifyFrom_Head.Checked)
                {
                    FilterStartFrom_New = Socket_Cache.Filter.FilterStartFrom.Head;
                }
                else
                {
                    FilterStartFrom_New = Socket_Cache.Filter.FilterStartFrom.Position;
                }

                switch (FilterMode_New)
                {
                    case Socket_Cache.Filter.FilterMode.Normal:

                        for (int i = 0; i < this.dgvFilterNormal.Columns.Count; i++)
                        {
                            string sSearchValue = string.Empty;
                            string sModifyValue = string.Empty;

                            if (dgvFilterNormal.Rows[0].Cells[i].Value != null)
                            {
                                sSearchValue = dgvFilterNormal.Rows[0].Cells[i].Value.ToString().Trim();
                            }

                            if (!String.IsNullOrEmpty(sSearchValue))
                            {
                                sSearch_New += i.ToString() + "-" + sSearchValue + ",";
                            }

                            if (dgvFilterNormal.Rows[1].Cells[i].Value != null)
                            {
                                sModifyValue = dgvFilterNormal.Rows[1].Cells[i].Value.ToString().Trim();
                            }

                            if (!String.IsNullOrEmpty(sModifyValue))
                            {
                                sModify_New += i.ToString() + "-" + sModifyValue + ",";
                            }
                        }                    

                        break;

                    case Socket_Cache.Filter.FilterMode.Advanced:

                        for (int i = 0; i < this.dgvFilterAdvanced_Search.Columns.Count; i++)
                        {
                            string sValue = string.Empty;

                            if (dgvFilterAdvanced_Search.Rows[0].Cells[i].Value != null)
                            {
                                sValue = dgvFilterAdvanced_Search.Rows[0].Cells[i].Value.ToString().Trim();
                            }

                            if (!String.IsNullOrEmpty(sValue))
                            {
                                sSearch_New += i.ToString() + "-" + sValue + ",";
                            }
                        }

                        switch (FilterStartFrom_New)
                        { 
                            case Socket_Cache.Filter.FilterStartFrom.Head:

                                for (int i = 0; i < this.dgvFilterAdvanced_Modify_FromHead.Columns.Count; i++)
                                {
                                    string sValue = string.Empty;

                                    if (dgvFilterAdvanced_Modify_FromHead.Rows[0].Cells[i].Value != null)
                                    {
                                        sValue = dgvFilterAdvanced_Modify_FromHead.Rows[0].Cells[i].Value.ToString().Trim();
                                    }

                                    if (!String.IsNullOrEmpty(sValue))
                                    {
                                        sModify_New += i.ToString() + "-" + sValue + ",";
                                    }
                                }

                                break;

                            case Socket_Cache.Filter.FilterStartFrom.Position:

                                for (int i = 0; i < this.dgvFilterAdvanced_Modify_FromPosition.Columns.Count; i++)
                                {
                                    string sValue = string.Empty;

                                    if (dgvFilterAdvanced_Modify_FromPosition.Rows[0].Cells[i].Value != null)
                                    {
                                        sValue = dgvFilterAdvanced_Modify_FromPosition.Rows[0].Cells[i].Value.ToString().Trim();
                                    }

                                    if (!String.IsNullOrEmpty(sValue))
                                    {
                                        sModify_New += i.ToString() + "-" + sValue + ",";
                                    }
                                }

                                break;
                        }

                        break;
                }

                sSearch_New = sSearch_New.TrimEnd(',');
                sModify_New = sModify_New.TrimEnd(',');

                int iFModifyCNT_New = int.Parse(this.nudFilterSet_ModifyTimes.Value.ToString());                

                Socket_Cache.FilterList.UpdateFilter_ByFilterNum(FilterNum, sFName_New, bAppointHeader, sHeaderContent_New, FilterMode_New, FilterAction_New, FilterFunction_New, FilterStartFrom_New, iFModifyCNT_New, sSearch_New, sModify_New);                

                this.Close();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion        

        #region//右键菜单

        private void cmsDGV_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string sDGVName = ((ContextMenuStrip)sender).SourceControl.Name;
            string sItemText = e.ClickedItem.Name;

            cmsDGV.Close();

            try
            {
                string sCellText = string.Empty;

                DataGridView dgv = new DataGridView();

                switch (sDGVName)
                {
                    case "dgvFilterNormal":
                        dgv = this.dgvFilterNormal;
                        break;

                    case "dgvFilterAdvanced_Search":
                        dgv = this.dgvFilterAdvanced_Search;
                        break;

                    case "dgvFilterAdvanced_Modify_FromHead":
                        dgv = this.dgvFilterAdvanced_Modify_FromHead;
                        break;

                    case "dgvFilterAdvanced_Modify_FromPosition":
                        dgv = this.dgvFilterAdvanced_Modify_FromPosition;
                        break;
                }

                switch (sItemText)
                {
                    case "cmsDGV_Copy":

                        if (dgv.CurrentCell.Value != null)
                        {
                            sCellText = dgv.CurrentCell.Value.ToString();
                            Clipboard.SetText(sCellText);
                        }
                        
                        break;

                    case "cmsDGV_Cut":

                        if (dgv.CurrentCell.Value != null)
                        {
                            sCellText = dgv.CurrentCell.Value.ToString();
                            Clipboard.SetText(sCellText);
                            dgv.CurrentCell.Value = null;
                        }
                        
                        break;

                    case "cmsDGV_Paste":

                        string sClipboardText = Clipboard.GetText().Trim();
                        PastePacketData(dgv, sClipboardText);

                        break;

                    case "cmsDGV_Delete":

                        dgv.CurrentCell.Value = null;

                        break;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void PastePacketData(DataGridView dgv, string sData)
        {
            try
            {
                if (!string.IsNullOrEmpty(sData) && Socket_Operation.IsHexString(sData))
                {
                    string[] DataCells = sData.Split(' ');

                    int iRow = dgv.CurrentCell.RowIndex;
                    int iCol = dgv.CurrentCell.ColumnIndex;

                    for (int i = 0; i < DataCells.Length; i++)
                    {
                        if (iCol + i < dgv.ColumnCount)
                        {
                            dgv[iCol + i, iRow].Value = Convert.ChangeType(DataCells[i].ToUpper(), dgv[iCol + i, iRow].ValueType);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                else
                {  
                    Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_42));
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
