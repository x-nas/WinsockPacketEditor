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
        bool bFinishInit = false;
        private string FilterName = "";
        private int FilterNum = -1, FilterIndex = -1;
        private string FilterSearch = "", FilterModify = "";
        private int FilterSearchLen = -1, FilterModifyLen = -1;
        private Socket_Filter_Info.FilterDGVType FilterDGVType_Init;
        private Socket_Filter_Info.FilterMode FilterMode;        
        private Socket_Filter_Info.StartFrom FilterStartFrom;

        #region//窗体加载

        public Socket_Filter_Form(int FNum)
        {
            try
            {
                MultiLanguage.SetDefaultLanguage(MultiLanguage.DefaultLanguage);
                InitializeComponent();

                dgvFilter_Normal.AutoGenerateColumns = false;
                dgvFilter_Normal.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvFilter_Normal, true, null);
                dgvFilter_Advanced_Search.AutoGenerateColumns = false;
                dgvFilter_Advanced_Search.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvFilter_Advanced_Search, true, null);
                dgvFilter_Advanced_Modify.AutoGenerateColumns = false;
                dgvFilter_Advanced_Modify.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvFilter_Advanced_Modify, true, null);

                this.FilterNum = FNum;
                this.FilterIndex = Socket_Cache.SocketFilterList.GetFilterIndex_ByFilterNum(FilterNum);
                int iInjectProcessID = RemoteHooking.GetCurrentProcessId();
                string sInjectProcesName = Process.GetCurrentProcess().ProcessName;
                this.Text = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_16) + FilterNum + " 】- " + sInjectProcesName + " [" + iInjectProcessID.ToString() + "]";

                this.InitFrom();
                this.InitDGVAndShowFilterInfo(Socket_Filter_Info.FilterDGVType.All);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }        

        private void InitFrom()
        {
            try
            {
                if (FilterIndex >= 0)
                {
                    this.FilterName = Socket_Cache.SocketFilterList.lstFilter[FilterIndex].FName.ToString();
                    this.FilterMode = Socket_Cache.SocketFilterList.lstFilter[FilterIndex].FMode;                    
                    this.FilterStartFrom = Socket_Cache.SocketFilterList.lstFilter[FilterIndex].FStartFrom;
                    this.FilterSearch = Socket_Cache.SocketFilterList.lstFilter[FilterIndex].FSearch.ToString().Trim();
                    this.FilterSearchLen = Socket_Cache.SocketFilterList.lstFilter[FilterIndex].FSearchLen;
                    this.FilterModify = Socket_Cache.SocketFilterList.lstFilter[FilterIndex].FModify.ToString().Trim();
                    this.FilterModifyLen = Socket_Cache.SocketFilterList.lstFilter[FilterIndex].FModifyLen;

                    this.txtFilterName.Text = FilterName;

                    switch (FilterMode)
                    {
                        case Socket_Filter_Info.FilterMode.Normal:
                            this.rbNormal.Checked = true;
                            break;

                        case Socket_Filter_Info.FilterMode.Advanced:
                            this.rbAdvanced.Checked = true;
                            break;
                    }                    

                    switch (FilterStartFrom)
                    {
                        case Socket_Filter_Info.StartFrom.Head:
                            this.rbFromHead.Checked = true;
                            break;

                        case Socket_Filter_Info.StartFrom.Position:
                            this.rbFromPosition.Checked = true;
                            break;
                    }

                    this.FilterModeChange();                    
                    this.PacketLengthChange();                    

                    this.nudPacketLength.Value = FilterSearchLen;
                    this.nudModifyLength.Value = FilterModifyLen;
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

        #endregion

        #region//重置按钮
        private void bReset_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = Socket_Operation.ShowSelectMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_17));

                if (dr.Equals(DialogResult.OK))
                {
                    this.InitFrom();
                    this.InitDGVAndShowFilterInfo(Socket_Filter_Info.FilterDGVType.All);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        #endregion

        #region//保存按钮
        private void bSave_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dr = Socket_Operation.ShowSelectMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_18));

                if (dr.Equals(DialogResult.OK))
                {
                    string sSearch_New = "", sModify_New = "";
                    int iSearchLen_New = 0, iModifyLen_New = 0;
                    Socket_Filter_Info.FilterMode FMode_New;                    
                    Socket_Filter_Info.StartFrom FStartFrom_New;

                    string sFName_New = this.txtFilterName.Text.Trim();

                    if (!string.IsNullOrEmpty(sFName_New))
                    {  
                        if (rbNormal.Checked)
                        {
                            FMode_New = Socket_Filter_Info.FilterMode.Normal;                                                       
                        }
                        else
                        {
                            FMode_New = Socket_Filter_Info.FilterMode.Advanced;                            
                        }                     

                        if (rbFromHead.Checked)
                        {
                            FStartFrom_New = Socket_Filter_Info.StartFrom.Head;
                        }
                        else
                        {
                            FStartFrom_New = Socket_Filter_Info.StartFrom.Position;
                        }

                        switch (FMode_New)
                        {
                            case Socket_Filter_Info.FilterMode.Normal:

                                for (int i = 0; i < this.dgvFilter_Normal.Columns.Count; i++)
                                {
                                    string sSearchValue;

                                    try
                                    {
                                        sSearchValue = dgvFilter_Normal.Rows[Socket_Cache.SocketFilterList.FilterNormal_SearchRowIndex].Cells[i].Value.ToString().Trim();
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
                                        sModifyValue = dgvFilter_Normal.Rows[Socket_Cache.SocketFilterList.FilterNormal_ModifyRowIndex].Cells[i].Value.ToString().Trim();
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

                                break;

                            case Socket_Filter_Info.FilterMode.Advanced:

                                for (int i = 0; i < this.dgvFilter_Advanced_Search.Columns.Count; i++)
                                {
                                    string sSearchValue;

                                    try
                                    {
                                        sSearchValue = dgvFilter_Advanced_Search.Rows[Socket_Cache.SocketFilterList.FilterAdvanced_SearchRowIndex].Cells[i].Value.ToString().Trim();
                                    }
                                    catch
                                    {
                                        sSearchValue = "";
                                    }

                                    if (!String.IsNullOrEmpty(sSearchValue))
                                    {
                                        sSearch_New += i.ToString() + "-" + sSearchValue + ",";
                                    }                                    
                                }

                                for (int i = 0; i < this.dgvFilter_Advanced_Modify.Columns.Count; i++)
                                {
                                    string sModifyValue;

                                    try
                                    {
                                        sModifyValue = dgvFilter_Advanced_Modify.Rows[Socket_Cache.SocketFilterList.FilterAdvanced_ModifyRowIndex].Cells[i].Value.ToString().Trim();
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
                                
                                break;                            
                        }

                        sSearch_New = sSearch_New.TrimEnd(',');
                        sModify_New = sModify_New.TrimEnd(',');

                        iSearchLen_New = int.Parse(this.nudPacketLength.Value.ToString());
                        iModifyLen_New = int.Parse(this.nudModifyLength.Value.ToString());

                        Socket_Cache.SocketFilterList.UpdateFilter_ByFilterNum(FilterNum, sFName_New, FMode_New, FStartFrom_New, sSearch_New, iSearchLen_New, sModify_New, iModifyLen_New);

                        this.Close();
                    }
                    else
                    {
                        Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_19));
                    }
                }
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
                if (rbNormal.Checked)
                {
                    this.tpNormal.Parent = this.tcFilterInfo;
                    this.tpAdvanced.Parent = null;

                    this.gbStartModify.Enabled = false;
                    this.gbModifyLength.Enabled = false;
                }
                else if (rbAdvanced.Checked)
                {
                    this.tpNormal.Parent = null;
                    this.tpAdvanced.Parent = this.tcFilterInfo;

                    this.gbStartModify.Enabled = true;
                    this.gbModifyLength.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//黏贴封包数据
        private void dgvFilterList_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Control && e.KeyCode == Keys.V)
                {
                    string sDGVName = ((DataGridView)sender).Name.Trim();
                    string sClipboardText = Clipboard.GetText().Trim();

                    ShowSocketSendData(sDGVName, sClipboardText);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }        

        private void ShowSocketSendData(string sDGVName, string sData)
        {
            try
            {
                int iRow = 0, iCol = 0;
                string[] DataCells = sData.Split(' ');                

                switch (sDGVName)
                {
                    case "dgvFilter_Normal":

                        iRow = dgvFilter_Normal.CurrentCell.RowIndex;
                        iCol = dgvFilter_Normal.CurrentCell.ColumnIndex;

                        for (int i = 0; i < DataCells.Length; i++)
                        {
                            if (iCol + i < this.dgvFilter_Normal.ColumnCount)
                            {
                                dgvFilter_Normal[iCol + i, iRow].Value = Convert.ChangeType(DataCells[i], dgvFilter_Normal[iCol + i, iRow].ValueType);
                            }
                            else
                            {
                                break;
                            }
                        }

                        break;

                    case "dgvFilter_Advanced_Search":

                        iRow = dgvFilter_Advanced_Search.CurrentCell.RowIndex;
                        iCol = dgvFilter_Advanced_Search.CurrentCell.ColumnIndex;

                        for (int i = 0; i < DataCells.Length; i++)
                        {
                            if (iCol + i < this.dgvFilter_Advanced_Search.ColumnCount)
                            {
                                dgvFilter_Advanced_Search[iCol + i, iRow].Value = Convert.ChangeType(DataCells[i], dgvFilter_Advanced_Search[iCol + i, iRow].ValueType);
                            }
                            else
                            {
                                break;
                            }
                        }

                        break;

                    case "dgvFilter_Advanced_Modify":

                        iRow = dgvFilter_Advanced_Modify.CurrentCell.RowIndex;
                        iCol = dgvFilter_Advanced_Modify.CurrentCell.ColumnIndex;

                        for (int i = 0; i < DataCells.Length; i++)
                        {
                            if (iCol + i < this.dgvFilter_Advanced_Modify.ColumnCount)
                            {
                                dgvFilter_Advanced_Modify[iCol + i, iRow].Value = Convert.ChangeType(DataCells[i], dgvFilter_Advanced_Modify[iCol + i, iRow].ValueType);
                            }
                            else
                            {
                                break;
                            }
                        }

                        break;

                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        #endregion

        #region//封包大小

        private void rbPacketLength_CheckedChanged(object sender, EventArgs e)
        {
            this.PacketLengthChange();
        }

        private void bPacketLength_Click(object sender, EventArgs e)
        {
            try
            {
                int iPacketLength_New = int.Parse(nudPacketLength.Value.ToString());

                if (iPacketLength_New != this.dgvFilter_Normal.ColumnCount)
                {
                    this.FilterSearchLen = iPacketLength_New;
                    this.InitDGVAndShowFilterInfo(Socket_Filter_Info.FilterDGVType.Search);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void PacketLengthChange()
        {
            try
            {
                if (rbPacketLength_NoModify.Checked)
                {
                    this.nudPacketLength.Enabled = false;
                    this.bPacketLength.Enabled = false;

                }
                else if (rbPacketLength_Custom.Checked)
                {
                    this.nudPacketLength.Enabled = true;
                    this.bPacketLength.Enabled = true;

                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//修改长度

        private void bModifyLength_Click(object sender, EventArgs e)
        {
            try
            {
                this.FilterModifyLen = int.Parse(nudModifyLength.Value.ToString());

                this.InitDGVAndShowFilterInfo(Socket_Filter_Info.FilterDGVType.Modify);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//修改起始于切换

        private void rbFromHead_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFromHead.Checked && bFinishInit)
            {
                this.StartFromChange();
            }
        }

        private void rbFromPosition_CheckedChanged(object sender, EventArgs e)
        {
            if (rbFromPosition.Checked && bFinishInit)
            {
                this.StartFromChange();
            }
        }        

        private void StartFromChange()
        {
            try
            {
                this.InitDGVAndShowFilterInfo(Socket_Filter_Info.FilterDGVType.Modify);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//初始化数据表

        private void InitDGV_Search(int FilterLength)
        {
            try
            {
                this.dgvFilter_Normal.Columns.Clear();

                for (int i = 0; i < FilterLength; i++)
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
                    dgvFilter_Normal.Columns.Add(dgvColumn);
                }

                if (dgvFilter_Normal.Rows.Count == 0)
                {
                    dgvFilter_Normal.Rows.Add();
                    dgvFilter_Normal.Rows.Add();
                }

                this.dgvFilter_Advanced_Search.Columns.Clear();

                for (int i = 0; i < FilterLength; i++)
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
                    dgvFilter_Advanced_Search.Columns.Add(dgvColumn);
                }

                if (dgvFilter_Advanced_Search.Rows.Count == 0)
                {
                    dgvFilter_Advanced_Search.Rows.Add();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void InitDGV_Modify(int FilterLength)
        {
            try
            {
                this.dgvFilter_Advanced_Modify.Columns.Clear();

                if (rbFromHead.Checked)
                {
                    for (int i = 0; i < FilterLength; i++)
                    {
                        DataGridViewTextBoxColumn dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn()
                        {
                            Name = "col" + (i + 1).ToString("000"),
                            HeaderText = (i + 1).ToString("000"),
                            Width = 50,
                            MaxInputLength = 2
                        };

                        DataGridViewTextBoxColumn dgvColumn = dataGridViewTextBoxColumn;
                        dgvColumn.DefaultCellStyle.BackColor = Color.Yellow;
                        dgvColumn.DefaultCellStyle.ForeColor = Color.RoyalBlue;

                        dgvFilter_Advanced_Modify.Columns.Add(dgvColumn);
                    }
                }
                else
                {
                    for (int i = -FilterLength; i < FilterLength; i++)
                    {
                        DataGridViewTextBoxColumn dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn()
                        {
                            Name = "col" + i.ToString("000"),
                            HeaderText = i.ToString("000"),
                            Width = 50,
                            MaxInputLength = 2
                        };

                        DataGridViewTextBoxColumn dgvColumn = dataGridViewTextBoxColumn;
                        dgvColumn.DefaultCellStyle.BackColor = Color.Yellow;
                        dgvColumn.DefaultCellStyle.ForeColor = Color.RoyalBlue;

                        dgvFilter_Advanced_Modify.Columns.Add(dgvColumn);
                    }
                }

                if (dgvFilter_Advanced_Modify.Rows.Count == 0)
                {
                    dgvFilter_Advanced_Modify.Rows.Add();
                }

                foreach (DataGridViewColumn column in dgvFilter_Advanced_Modify.Columns)
                {
                    if (column.Name == "col000")
                    {
                        dgvFilter_Advanced_Modify.CurrentCell = dgvFilter_Advanced_Modify.Rows[0].Cells[column.Index];                        
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void dgvFilter_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            try
            {
                e.Column.FillWeight = 1;
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

                    foreach (string sText in sSearchAll)
                    {
                        string[] sSplitText = sText.Split('-');
                        int iIndex = int.Parse(sSplitText[0].ToString().Trim());
                        string sValue = sSplitText[1].ToString().Trim();

                        if (this.dgvFilter_Normal.Rows.Count == 2)
                        {
                            if (iIndex < this.dgvFilter_Normal.Rows[Socket_Cache.SocketFilterList.FilterNormal_SearchRowIndex].Cells.Count)
                            {
                                this.dgvFilter_Normal.Rows[Socket_Cache.SocketFilterList.FilterNormal_SearchRowIndex].Cells[iIndex].Value = sValue;
                            }
                        }

                        if (this.dgvFilter_Advanced_Search.Rows.Count == 1)
                        {
                            if (iIndex < this.dgvFilter_Advanced_Search.Rows[Socket_Cache.SocketFilterList.FilterAdvanced_SearchRowIndex].Cells.Count)
                            {
                                this.dgvFilter_Advanced_Search.Rows[Socket_Cache.SocketFilterList.FilterAdvanced_SearchRowIndex].Cells[iIndex].Value = sValue;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message + " - Search");
            }

            try
            {
                if (!string.IsNullOrEmpty(FilterModify))
                {
                    string[] sModifyAll = FilterModify.Split(',');

                    foreach (string sText in sModifyAll)
                    {
                        string[] sSplitText = sText.Split('-');
                        int iIndex = int.Parse(sSplitText[0].ToString().Trim());
                        string sValue = sSplitText[1].ToString().Trim();

                        switch (FilterMode)
                        {
                            case Socket_Filter_Info.FilterMode.Normal:

                                if (iIndex < this.dgvFilter_Normal.Rows[Socket_Cache.SocketFilterList.FilterNormal_ModifyRowIndex].Cells.Count)
                                {
                                    this.dgvFilter_Normal.Rows[Socket_Cache.SocketFilterList.FilterNormal_ModifyRowIndex].Cells[iIndex].Value = sValue;
                                }

                                break;

                            case Socket_Filter_Info.FilterMode.Advanced:

                                if (iIndex < this.dgvFilter_Advanced_Modify.Rows[Socket_Cache.SocketFilterList.FilterAdvanced_ModifyRowIndex].Cells.Count)
                                {
                                    this.dgvFilter_Advanced_Modify.Rows[Socket_Cache.SocketFilterList.FilterAdvanced_ModifyRowIndex].Cells[iIndex].Value = sValue;
                                }

                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message + " - Modify");
            }
        }

        #endregion

        #region//初始化数据表并显示滤镜内容（异步）

        private void InitDGVAndShowFilterInfo(Socket_Filter_Info.FilterDGVType FDGVType)
        {
            try
            {
                this.pbLoading.Visible = true;
                this.tcFilterInfo.Visible = false;
                this.tlpFilterInfo_Function.Enabled = false;

                if (!bgwFilterInfo.IsBusy)
                {
                    this.FilterDGVType_Init = FDGVType;

                    bgwFilterInfo.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }        

        private void bgwFilterInfo_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                switch (this.FilterDGVType_Init)
                {
                    case Socket_Filter_Info.FilterDGVType.Search:

                        this.InitDGV_Search(FilterSearchLen);

                        break;

                    case Socket_Filter_Info.FilterDGVType.Modify:

                        this.InitDGV_Modify(FilterModifyLen);

                        break;

                    case Socket_Filter_Info.FilterDGVType.All:

                        this.InitDGV_Search(FilterSearchLen);
                        this.InitDGV_Modify(FilterModifyLen);

                        break;
                }
                
                this.ShowFilterInfo();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }        

        private void bgwFilterInfo_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.pbLoading.Visible = false;
                this.tcFilterInfo.Visible = true;
                this.tlpFilterInfo_Function.Enabled = true;

                this.bFinishInit = true;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }        

        #endregion
    }
}
