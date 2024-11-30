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
        private int FilterIndex = -1;
        private decimal FilterSocketContent = 1;
        private decimal FilterLengthContent = 1;
        private decimal FilterProgressionStep = 1;
        private string FilterName = string.Empty;
        private string FilterHeaderContent = string.Empty;
        private string FilterProgressionPosition = string.Empty;
        private string FilterSearch = string.Empty;
        private string FilterModify = string.Empty;
        private bool FilterAppointHeader = false;
        private bool FilterAppointSocket = false;
        private bool FilterAppointLength = false;
        private Socket_Cache.Filter.FilterMode FilterMode;
        private Socket_Cache.Filter.FilterAction FilterAction;
        private Socket_Cache.Filter.FilterFunction FilterFunction;        
        private Socket_Cache.Filter.FilterStartFrom FilterStartFrom;

        #region//窗体加载

        public Socket_FilterForm(int FIndex)
        {
            try
            {
                MultiLanguage.SetDefaultLanguage(MultiLanguage.DefaultLanguage);
                InitializeComponent();

                this.FilterIndex = FIndex;

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
                this.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_16), (FilterIndex + 1), sInjectProcesName, iInjectProcessID);

                if (this.FilterIndex > -1)
                {
                    this.FilterName = Socket_Cache.FilterList.lstFilter[FilterIndex].FName;
                    this.FilterAppointHeader = Socket_Cache.FilterList.lstFilter[FilterIndex].AppointHeader;
                    this.FilterHeaderContent = Socket_Cache.FilterList.lstFilter[FilterIndex].HeaderContent;
                    this.FilterAppointSocket = Socket_Cache.FilterList.lstFilter[FilterIndex].AppointSocket;
                    this.FilterSocketContent = Socket_Cache.FilterList.lstFilter[FilterIndex].SocketContent;
                    this.FilterAppointLength = Socket_Cache.FilterList.lstFilter[FilterIndex].AppointLength;
                    this.FilterLengthContent = Socket_Cache.FilterList.lstFilter[FilterIndex].LengthContent;
                    this.FilterMode = Socket_Cache.FilterList.lstFilter[FilterIndex].FMode;
                    this.FilterAction = Socket_Cache.FilterList.lstFilter[FilterIndex].FAction;
                    this.FilterFunction = Socket_Cache.FilterList.lstFilter[FilterIndex].FFunction;
                    this.FilterStartFrom = Socket_Cache.FilterList.lstFilter[FilterIndex].FStartFrom;
                    this.FilterProgressionStep = Socket_Cache.FilterList.lstFilter[FilterIndex].ProgressionStep;
                    this.FilterProgressionPosition = Socket_Cache.FilterList.lstFilter[FilterIndex].ProgressionPosition;
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

                        case Socket_Cache.Filter.FilterAction.NoModify_Display:
                            this.rbFilterAction_NoModify_Display.Checked = true;
                            break;

                        case Socket_Cache.Filter.FilterAction.NoModify_NoDisplay:
                            this.rbFilterAction_NoModify_NoDisplay.Checked = true;
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
                    this.txtFilter_HeaderContent.Text = FilterHeaderContent;
                    this.FilterAppointHeaderChange();

                    this.cbFilter_AppointSocket.Checked = FilterAppointSocket;
                    this.nudFilter_SocketContent.Value = FilterSocketContent;
                    this.FilterAppointSocketChange();

                    this.cbFilter_AppointLength.Checked = FilterAppointLength;
                    this.nudFilter_LengthContent.Value = FilterLengthContent;
                    this.FilterAppointLengthChange();

                    this.nudProgressionStep.Value = FilterProgressionStep;                    

                    this.txtFilterName.Text = FilterName;
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

                if (dgvFilterAdvanced_Modify_FromPosition.Rows[0].Cells["col000"] != null)
                {
                    dgvFilterAdvanced_Modify_FromPosition.FirstDisplayedCell = dgvFilterAdvanced_Modify_FromPosition.Rows[0].Cells["col000"];
                }                

                this.InitProgressionPosition();
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
                if (this.dgvFilterNormal.Columns.Count > 0)
                {
                    this.dgvFilterNormal.Columns.Clear();
                }                

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
                if (this.dgvFilterAdvanced_Search.Columns.Count > 0)
                {
                    this.dgvFilterAdvanced_Search.Columns.Clear();
                }                

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
                if (this.dgvFilterAdvanced_Modify_FromHead.Columns.Count > 0)
                {
                    this.dgvFilterAdvanced_Modify_FromHead.Columns.Clear();
                }                                

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
                if (this.dgvFilterAdvanced_Modify_FromPosition.Columns.Count > 0)
                {
                    this.dgvFilterAdvanced_Modify_FromPosition.Columns.Clear();
                }

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
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void InitProgressionPosition()
        {
            try
            {
                if (!string.IsNullOrEmpty(FilterProgressionPosition))
                {
                    string[] slProgressionPosition = FilterProgressionPosition.Split(',');

                    foreach (string sPosition in slProgressionPosition)
                    {
                        if (!string.IsNullOrEmpty(sPosition))
                        {
                            if (int.TryParse(sPosition, out int iIndex))
                            {
                                switch (FilterMode)
                                {
                                    case Socket_Cache.Filter.FilterMode.Normal:

                                        if (dgvFilterNormal.Rows.Count == 2 && dgvFilterNormal.Columns.Count > iIndex)
                                        {
                                            this.dgvFilterNormal.Rows[1].Cells[iIndex].Style.BackColor = Color.DarkRed;
                                        }                                        

                                        break;

                                    case Socket_Cache.Filter.FilterMode.Advanced:

                                        switch (FilterStartFrom)
                                        {
                                            case Socket_Cache.Filter.FilterStartFrom.Head:

                                                if (dgvFilterAdvanced_Modify_FromHead.Rows.Count == 1 && dgvFilterAdvanced_Modify_FromHead.Columns.Count > iIndex)
                                                {
                                                    this.dgvFilterAdvanced_Modify_FromHead.Rows[0].Cells[iIndex].Style.BackColor = Color.DarkRed;
                                                }                                                

                                                break;

                                            case Socket_Cache.Filter.FilterStartFrom.Position:

                                                if (dgvFilterAdvanced_Modify_FromPosition.Rows.Count == 1 && dgvFilterAdvanced_Modify_FromPosition.Columns.Count > iIndex)
                                                {
                                                    this.dgvFilterAdvanced_Modify_FromPosition.Rows[0].Cells[iIndex].Style.BackColor = Color.DarkRed;
                                                }                                                

                                                break;
                                        }

                                        break;
                                }                                
                            }
                        }
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
                if (rbFilterMode_Normal.Checked)
                {
                    this.tpNormal.Parent = this.tcFilterInfo;
                    this.tpAdvanced.Parent = null;

                    this.gbFilterModifyFrom.Enabled = false;                    
                }
                else if (rbFilterMode_Advanced.Checked)
                {
                    this.tpNormal.Parent = null;
                    this.tpAdvanced.Parent = this.tcFilterInfo;

                    this.gbFilterModifyFrom.Enabled = true;
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
                    this.dgvFilterAdvanced_Modify_FromHead.Visible = true;
                    this.dgvFilterAdvanced_Modify_FromPosition.Visible = false;
                }
                else if (rbFilterModifyFrom_Position.Checked)
                {
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

        #region//指定类型        

        private void cbFilter_AppointHeader_CheckedChanged(object sender, EventArgs e)
        {
            this.FilterAppointHeaderChange();
        }

        private void cbFilter_AppointSocket_CheckedChanged(object sender, EventArgs e)
        {
            this.FilterAppointSocketChange();
        }

        private void cbFilter_AppointLength_CheckedChanged(object sender, EventArgs e)
        {
            this.FilterAppointLengthChange();
        }

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

        private void FilterAppointSocketChange()
        {
            try
            {
                if (this.cbFilter_AppointSocket.Checked)
                {
                    this.nudFilter_SocketContent.Enabled = true;
                }
                else
                {
                    this.nudFilter_SocketContent.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void FilterAppointLengthChange()
        {
            try
            {
                if (this.cbFilter_AppointLength.Checked)
                {
                    this.nudFilter_LengthContent.Enabled = true;
                }
                else
                {
                    this.nudFilter_LengthContent.Enabled = false;
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
                string sFName_New = this.txtFilterName.Text.Trim();
                if (string.IsNullOrEmpty(sFName_New))
                {
                    Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_19));
                    return;
                }

                string sHeaderContent_New = string.Empty;
                decimal dSocketContent_New = 0;
                decimal dLengthContent_New = 0;
                decimal dProgressionStep_New = 0;
                string sProgression_New = string.Empty;
                string sSearch_New = string.Empty;
                string sModify_New = string.Empty;
                bool bAppointHeader, bAppointSocket, bAppointLength;

                Socket_Cache.Filter.FilterMode FilterMode_New;
                Socket_Cache.Filter.FilterAction FilterAction_New;
                Socket_Cache.Filter.FilterFunction FilterFunction_New;
                Socket_Cache.Filter.FilterStartFrom FilterStartFrom_New;

                bAppointHeader = this.cbFilter_AppointHeader.Checked;
                bAppointSocket = this.cbFilter_AppointSocket.Checked;
                bAppointLength = this.cbFilter_AppointLength.Checked;

                sHeaderContent_New = this.txtFilter_HeaderContent.Text.Trim();
                dSocketContent_New = this.nudFilter_SocketContent.Value;
                dLengthContent_New = this.nudFilter_LengthContent.Value;
                dProgressionStep_New = this.nudProgressionStep.Value;

                if (rbFilterMode_Normal.Checked)
                {
                    FilterMode_New = Socket_Cache.Filter.FilterMode.Normal;
                }
                else if (rbFilterMode_Advanced.Checked)
                {
                    FilterMode_New = Socket_Cache.Filter.FilterMode.Advanced;
                }
                else
                {
                    FilterMode_New = Socket_Cache.Filter.FilterMode.Normal;
                }

                if (rbFilterAction_Replace.Checked)
                {
                    FilterAction_New = Socket_Cache.Filter.FilterAction.Replace;
                }
                else if (rbFilterAction_Intercept.Checked)
                {
                    FilterAction_New = Socket_Cache.Filter.FilterAction.Intercept;
                }
                else if (rbFilterAction_NoModify_Display.Checked)
                {
                    FilterAction_New = Socket_Cache.Filter.FilterAction.NoModify_Display;
                }
                else if (rbFilterAction_NoModify_NoDisplay.Checked)
                {
                    FilterAction_New = Socket_Cache.Filter.FilterAction.NoModify_NoDisplay;
                }
                else
                {
                    FilterAction_New = Socket_Cache.Filter.FilterAction.NoModify_Display;
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
                            if (dgvFilterNormal.Rows[1].Cells[i].Style.BackColor == Color.DarkRed)
                            {
                                sProgression_New += i.ToString() + ",";
                            }

                            if (dgvFilterNormal.Rows[0].Cells[i].Value != null)
                            {
                                string sSearchValue = dgvFilterNormal.Rows[0].Cells[i].Value.ToString().Trim();

                                if (!String.IsNullOrEmpty(sSearchValue))
                                {
                                    sSearch_New += i.ToString() + "-" + sSearchValue + ",";
                                }
                            }

                            if (dgvFilterNormal.Rows[1].Cells[i].Value != null)
                            {
                                string sModifyValue = dgvFilterNormal.Rows[1].Cells[i].Value.ToString().Trim();

                                if (!String.IsNullOrEmpty(sModifyValue))
                                {
                                    sModify_New += i.ToString() + "-" + sModifyValue + ",";
                                }
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
                                    if (dgvFilterAdvanced_Modify_FromHead.Rows[0].Cells[i].Style.BackColor == Color.DarkRed)
                                    {
                                        sProgression_New += i.ToString() + ",";
                                    }

                                    if (dgvFilterAdvanced_Modify_FromHead.Rows[0].Cells[i].Value != null)
                                    {
                                        string sValue = dgvFilterAdvanced_Modify_FromHead.Rows[0].Cells[i].Value.ToString().Trim();

                                        if (!String.IsNullOrEmpty(sValue))
                                        {
                                            sModify_New += i.ToString() + "-" + sValue + ",";
                                        }
                                    }
                                }

                                break;

                            case Socket_Cache.Filter.FilterStartFrom.Position:

                                for (int i = 0; i < this.dgvFilterAdvanced_Modify_FromPosition.Columns.Count; i++)
                                {
                                    if (dgvFilterAdvanced_Modify_FromPosition.Rows[0].Cells[i].Style.BackColor == Color.DarkRed)
                                    {
                                        sProgression_New += i.ToString() + ",";
                                    }

                                    if (dgvFilterAdvanced_Modify_FromPosition.Rows[0].Cells[i].Value != null)
                                    {
                                        string sValue = dgvFilterAdvanced_Modify_FromPosition.Rows[0].Cells[i].Value.ToString().Trim();

                                        if (!String.IsNullOrEmpty(sValue))
                                        {
                                            sModify_New += i.ToString() + "-" + sValue + ",";
                                        }
                                    }
                                }

                                break;
                        }

                        break;
                }

                sProgression_New = sProgression_New.TrimEnd(',');
                sSearch_New = sSearch_New.TrimEnd(',');
                sModify_New = sModify_New.TrimEnd(',');

                Socket_Cache.Filter.UpdateFilter_ByFilterIndex(
                    this.FilterIndex, 
                    sFName_New, 
                    bAppointHeader, 
                    sHeaderContent_New, 
                    bAppointSocket, 
                    dSocketContent_New, 
                    bAppointLength, 
                    dLengthContent_New, 
                    FilterMode_New, 
                    FilterAction_New, 
                    FilterFunction_New, 
                    FilterStartFrom_New,
                    dProgressionStep_New,
                    sProgression_New,
                    sSearch_New, 
                    sModify_New);                

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

                    case "cmsDGV_Progression_Enable":

                        if (dgv.Name.Equals("dgvFilterAdvanced_Search"))
                        {
                            Socket_Operation.ShowMessageBox("搜索位置不可设置递进！");
                            break;
                        }

                        if (dgv.Name.Equals("dgvFilterNormal"))
                        {
                            int iRowIndex = dgv.CurrentCell.RowIndex;

                            if (iRowIndex == 0)
                            {
                                Socket_Operation.ShowMessageBox("搜索位置不可设置递进！");
                                break;
                            }
                        }

                        dgv.CurrentCell.Style.BackColor = Color.DarkRed;
                        dgv.CurrentCell.Selected = false;

                        break;

                    case "cmsDGV_Progression_Disable":

                        dgv.CurrentCell.Style.BackColor = dgv.Rows[0].DefaultCellStyle.BackColor;
                        dgv.CurrentCell.Selected = false;

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
