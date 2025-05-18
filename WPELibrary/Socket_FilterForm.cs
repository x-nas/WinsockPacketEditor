using System;
using System.Drawing;
using System.Windows.Forms;
using WPELibrary.Lib;
using System.Reflection;
using System.Threading.Tasks;
using System.Text;

namespace WPELibrary
{
    public partial class Socket_FilterForm : Form
    {  
        private int FilterIndex = -1;
        private int LoadAllCount = 0;        
        private decimal FilterSocketContent = 1;        
        private decimal FilterPortContent = 1;
        private decimal FilterProgressionStep = 1;
        private string FilterName = string.Empty;
        private string FilterHeaderContent = string.Empty;
        private string FilterLengthContent = string.Empty;
        private string FilterProgressionPosition = string.Empty;
        private string FilterSearch = string.Empty;
        private string FilterModify = string.Empty;
        private bool IsExecute = false;
        private bool FilterAppointHeader = false;
        private bool FilterAppointSocket = false;
        private bool FilterAppointLength = false;
        private bool FilterAppointPort = false;
        private Socket_Cache.Filter.FilterMode FilterMode;
        private Socket_Cache.Filter.FilterAction FilterAction;
        private Socket_Cache.Filter.FilterExecuteType FilterExecuteType;
        private Guid SID = Guid.Empty;
        private Guid RID = Guid.Empty;
        private Socket_Cache.Filter.FilterFunction FilterFunction;        
        private Socket_Cache.Filter.FilterStartFrom FilterStartFrom;

        #region//窗体加载

        public Socket_FilterForm(int FIndex)
        {
            try
            {
                MultiLanguage.SetDefaultLanguage(MultiLanguage.DefaultLanguage);
                InitializeComponent();                

                if (FIndex > -1)
                {
                    this.FilterIndex = FIndex;

                    this.InitFrom();
                    this.InitDGV();

                    if (!this.bgwFilterInfo.IsBusy)
                    {
                        this.bFilterButton_Save.Enabled = false;
                        this.tcFilterInfo.Enabled = false;
                        this.lFilterInfo.Visible = true;

                        this.bgwFilterInfo.RunWorkerAsync();
                    }                    
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

        #region//初始化

        private void InitFrom()
        {
            try
            {
                string sFID = Socket_Cache.FilterList.lstFilter[FilterIndex].FID.ToString().ToUpper();
                this.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_16), (FilterIndex + 1), sFID);
                this.lFilterInfo.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_175), "0");

                this.FilterName = Socket_Cache.FilterList.lstFilter[FilterIndex].FName;
                this.FilterAppointHeader = Socket_Cache.FilterList.lstFilter[FilterIndex].AppointHeader;
                this.FilterHeaderContent = Socket_Cache.FilterList.lstFilter[FilterIndex].HeaderContent;
                this.FilterAppointSocket = Socket_Cache.FilterList.lstFilter[FilterIndex].AppointSocket;
                this.FilterSocketContent = Socket_Cache.FilterList.lstFilter[FilterIndex].SocketContent;
                this.FilterAppointLength = Socket_Cache.FilterList.lstFilter[FilterIndex].AppointLength;
                this.FilterLengthContent = Socket_Cache.FilterList.lstFilter[FilterIndex].LengthContent;
                this.FilterAppointPort = Socket_Cache.FilterList.lstFilter[FilterIndex].AppointPort;
                this.FilterPortContent = Socket_Cache.FilterList.lstFilter[FilterIndex].PortContent;
                this.FilterMode = Socket_Cache.FilterList.lstFilter[FilterIndex].FMode;
                this.FilterAction = Socket_Cache.FilterList.lstFilter[FilterIndex].FAction;
                this.IsExecute = Socket_Cache.FilterList.lstFilter[FilterIndex].IsExecute;
                this.FilterExecuteType = Socket_Cache.FilterList.lstFilter[FilterIndex].FEType;
                this.SID = Socket_Cache.FilterList.lstFilter[FilterIndex].SID;
                this.RID = Socket_Cache.FilterList.lstFilter[FilterIndex].RID;
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

                    case Socket_Cache.Filter.FilterAction.Change:
                        this.rbFilterAction_Change.Checked = true;
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

                this.cbFilterAction_Execute.Checked = IsExecute;
                this.FilterAction_ExecuteChange();

                switch (FilterExecuteType)
                { 
                    case Socket_Cache.Filter.FilterExecuteType.Send:
                        this.cbbFilterAction_ExecuteType.SelectedIndex = 0;
                        break;

                    case Socket_Cache.Filter.FilterExecuteType.Robot:
                        this.cbbFilterAction_ExecuteType.SelectedIndex = 1;
                        break;
                }
                this.FilterAction_ExecuteTypeChanged();

                this.cbFilter_AppointHeader.Checked = FilterAppointHeader;
                this.txtFilter_HeaderContent.Text = FilterHeaderContent;
                this.FilterAppointHeaderChange();

                this.cbFilter_AppointSocket.Checked = FilterAppointSocket;
                this.nudFilter_SocketContent.Value = FilterSocketContent;
                this.FilterAppointSocketChange();

                this.cbFilter_AppointLength.Checked = FilterAppointLength;
                if (!string.IsNullOrEmpty(this.FilterLengthContent))
                {
                    if (this.FilterLengthContent.Contains("-"))
                    {
                        string[] sLengthContent = this.FilterLengthContent.Split('-');

                        if (int.TryParse(sLengthContent[0], out int iLenFrom))
                        {
                            this.nudFilter_LengthContent_From.Value = iLenFrom;
                        }

                        if (int.TryParse(sLengthContent[1], out int iLenTo))
                        {
                            this.nudFilter_LengthContent_To.Value = iLenTo;
                        }
                    }
                    else
                    {
                        if (int.TryParse(this.FilterLengthContent, out int iLength))
                        {
                            this.nudFilter_LengthContent_From.Value = iLength;
                            this.nudFilter_LengthContent_To.Value = iLength;
                        }
                    }                    
                }                
                this.FilterAppointLengthChange();

                this.cbFilter_AppointPort.Checked = FilterAppointPort;
                this.nudFilter_PortContent.Value = FilterPortContent;
                this.FilterAppointPortChange();

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
                this.InitDGV_Normal_ByAdvance();

                this.dgvFilterAdvanced_Search.Height = this.tlpFilterAdvanced.Height / 2;
                this.dgvFilterAdvanced_Modify_FromHead.Height = this.tlpFilterAdvanced.Height / 2;
                this.dgvFilterAdvanced_Modify_FromPosition.Height = this.tlpFilterAdvanced.Height / 2;                

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
                    DataGridViewTextBoxColumn dgv = Socket_Operation.InitDGVColumn(i + 1, Color.RoyalBlue, Color.LightYellow);
                    dgvFilterNormal.Columns.Add(dgv);
                    dgv.Width = dgv.GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true) + 5;
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

        private void InitDGV_Normal_ByAdvance()
        {
            try
            {
                int iWidth = this.dgvFilterAdvanced_Search.Columns[0].Width;

                for (int i = 0; i < this.dgvFilterNormal.Columns.Count; i++)
                {
                    this.dgvFilterNormal.Columns[i].Width = iWidth;
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
                    DataGridViewTextBoxColumn dgv = Socket_Operation.InitDGVColumn(i + 1, Color.RoyalBlue, Color.LightYellow);
                    dgvFilterAdvanced_Search.Columns.Add(dgv);
                    dgv.Width = dgv.GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true) + 5;
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
                    DataGridViewTextBoxColumn dgv = Socket_Operation.InitDGVColumn(i + 1, Color.RoyalBlue, Color.Yellow);
                    dgvFilterAdvanced_Modify_FromHead.Columns.Add(dgv);
                    dgv.Width = dgv.GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true) + 5;
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

                int iSize = Socket_Cache.Filter.FilterSize_MaxLen;

                for (int i = -iSize; i < iSize; i++)
                {
                    DataGridViewTextBoxColumn dgv = Socket_Operation.InitDGVColumn(i, Color.RoyalBlue, Color.Yellow);
                    dgvFilterAdvanced_Modify_FromPosition.Columns.Add(dgv);
                    dgv.Width = dgv.GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true) + 5;
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

                                                iIndex += Socket_Cache.Filter.FilterSize_MaxLen;

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

        #region//滤镜动作-执行

        private void cbFilterAction_Execute_CheckedChanged(object sender, EventArgs e)
        {
            this.FilterAction_ExecuteChange();
        }      

        private void FilterAction_ExecuteChange()
        {
            this.cbbFilterAction_Execute.Enabled = this.cbbFilterAction_ExecuteType.Enabled = cbFilterAction_Execute.Checked;            
        }

        private void cbbFilterAction_ExecuteType_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.FilterAction_ExecuteTypeChanged();
        }

        private void FilterAction_ExecuteTypeChanged()
        {
            try
            {
                if (this.cbbFilterAction_ExecuteType.SelectedIndex == 0)
                {
                    this.InitSendInfo();
                }
                else
                {
                    this.InitRobotInfo();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void InitSendInfo()
        {
            try
            {
                if (Socket_Cache.SendList.lstSend.Count > 0)
                {
                    cbbFilterAction_Execute.DataSource = Socket_Cache.SendList.lstSend;
                    cbbFilterAction_Execute.DisplayMember = "SName";
                    cbbFilterAction_Execute.ValueMember = "SID";

                    this.cbbFilterAction_Execute.SelectedValue = this.SID;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void InitRobotInfo()
        {
            try
            {
                if (Socket_Cache.RobotList.lstRobot.Count > 0)
                {
                    cbbFilterAction_Execute.DataSource = Socket_Cache.RobotList.lstRobot;
                    cbbFilterAction_Execute.DisplayMember = "RName";
                    cbbFilterAction_Execute.ValueMember = "RID";

                    this.cbbFilterAction_Execute.SelectedValue = this.RID;
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

        #region//指定包头

        private void cbFilter_AppointHeader_CheckedChanged(object sender, EventArgs e)
        {
            this.FilterAppointHeaderChange();
        }

        private void FilterAppointHeaderChange()
        {
            this.txtFilter_HeaderContent.Enabled = this.cbFilter_AppointHeader.Checked;
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

        #endregion

        #region//指定套接字

        private void cbFilter_AppointSocket_CheckedChanged(object sender, EventArgs e)
        {
            this.FilterAppointSocketChange();
        }

        private void FilterAppointSocketChange()
        {
            this.nudFilter_SocketContent.Enabled = this.cbFilter_AppointSocket.Checked;
        }

        #endregion

        #region//指定长度

        private void cbFilter_AppointLength_CheckedChanged(object sender, EventArgs e)
        {
            this.FilterAppointLengthChange();
        }

        private void FilterAppointLengthChange()
        {
            this.nudFilter_LengthContent_From.Enabled = this.nudFilter_LengthContent_To.Enabled = this.cbFilter_AppointLength.Checked;
        }

        #endregion

        #region//指定端口

        private void cbFilter_AppointPort_CheckedChanged(object sender, EventArgs e)
        {
            this.FilterAppointPortChange();
        }

        private void FilterAppointPortChange()
        {
            this.nudFilter_PortContent.Enabled = this.cbFilter_AppointPort.Checked;
        }

        #endregion

        #endregion

        #region//显示滤镜内容（异步）

        private void bgwFilterInfo_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            this.ShowFilterInfo();
        }

        private void bgwFilterInfo_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            this.lFilterInfo.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_175), e.ProgressPercentage.ToString());
        }

        private void bgwFilterInfo_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            this.lFilterInfo.Visible = false;
            this.tcFilterInfo.Enabled = true;
            this.bFilterButton_Save.Enabled = true;
        }

        private void ShowFilterInfo()
        {
            try
            {
                if (!string.IsNullOrEmpty(FilterSearch))
                {
                    string[] sSearchAll = FilterSearch.Split(',');

                    foreach (string s in sSearchAll)
                    {
                        if (int.TryParse(s.Split('|')[0], out int iIndex))
                        {
                            string sValue = s.Split('|')[1];

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
                }

                if (!string.IsNullOrEmpty(FilterModify))
                {
                    string[] sModifyAll = FilterModify.Split(',');
                    this.LoadAllCount = sModifyAll.Length;

                    int LoadCount = 0;
                    foreach (string s in sModifyAll)
                    {
                        if (int.TryParse(s.Split('|')[0], out int iIndex))
                        {
                            string sValue = s.Split('|')[1];

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
                                                iIndex += Socket_Cache.Filter.FilterSize_MaxLen;

                                                if (iIndex < this.dgvFilterAdvanced_Modify_FromPosition.Rows[0].Cells.Count)
                                                {                                                    
                                                    this.dgvFilterAdvanced_Modify_FromPosition.Rows[0].Cells[iIndex].Value = sValue;

                                                    LoadCount++;
                                                    this.bgwFilterInfo.ReportProgress(LoadCount * 100 / LoadAllCount);
                                                }
                                            }

                                            break;
                                    }

                                    break;
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

        #region//粘贴数据（异步）

        private void dgvFilterNormal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                string sClipboardText = Clipboard.GetText().Trim();
                this.PastePacketData(dgvFilterNormal, sClipboardText);
            }
        }

        private void dgvFilterAdvanced_Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                string sClipboardText = Clipboard.GetText().Trim();
                this.PastePacketData(dgvFilterAdvanced_Search, sClipboardText);
            }
        }

        private void dgvFilterAdvanced_Modify_FromHead_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                string sClipboardText = Clipboard.GetText().Trim();
                this.PastePacketData(dgvFilterAdvanced_Modify_FromHead, sClipboardText);
            }
        }

        private void dgvFilterAdvanced_Modify_FromPosition_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                string sClipboardText = Clipboard.GetText().Trim();
                this.PastePacketData(dgvFilterAdvanced_Modify_FromPosition, sClipboardText);
            }
        }

        private async void PastePacketData(DataGridView dgv, string sData)
        {
            this.bFilterButton_Save.Enabled = false;

            await Task.Run(() =>
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
            });

            this.bFilterButton_Save.Enabled = true;
        }

        #endregion

        #region//滤镜设置合法性检测

        public bool CheckFilterIsValid()
        {
            bool bReturn = true;

            try
            {
                string sCheckValue = string.Empty;

                //滤镜名称
                sCheckValue = this.txtFilterName.Text.Trim();
                if (string.IsNullOrEmpty(sCheckValue))
                {
                    Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_19));
                    return false;
                }

                //普通滤镜
                for (int i = 0; i < this.dgvFilterNormal.Columns.Count; i++)
                {
                    if (dgvFilterNormal.Rows[0].Cells[i].Value != null)
                    {
                        sCheckValue = dgvFilterNormal.Rows[0].Cells[i].Value.ToString().Trim();
                        if (!Socket_Operation.IsValidFilterString(sCheckValue))
                        {
                            Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_83));
                            return false;
                        }
                    }

                    if (dgvFilterNormal.Rows[1].Cells[i].Value != null)
                    {
                        sCheckValue = dgvFilterNormal.Rows[1].Cells[i].Value.ToString().Trim();
                        if (!Socket_Operation.IsValidFilterString(sCheckValue))
                        {
                            Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_83));
                            return false;
                        }
                    }
                }

                //高级滤镜（搜索）
                for (int i = 0; i < this.dgvFilterAdvanced_Search.Columns.Count; i++)
                {
                    if (dgvFilterAdvanced_Search.Rows[0].Cells[i].Value != null)
                    {
                        sCheckValue = dgvFilterAdvanced_Search.Rows[0].Cells[i].Value.ToString().Trim();
                        if (!Socket_Operation.IsValidFilterString(sCheckValue))
                        {
                            Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_83));
                            return false;
                        }
                    }
                }

                //高级滤镜（修改 - 从头开始）
                for (int i = 0; i < this.dgvFilterAdvanced_Modify_FromHead.Columns.Count; i++)
                {
                    if (dgvFilterAdvanced_Modify_FromHead.Rows[0].Cells[i].Value != null)
                    {
                        sCheckValue = dgvFilterAdvanced_Modify_FromHead.Rows[0].Cells[i].Value.ToString().Trim();
                        if (!Socket_Operation.IsValidFilterString(sCheckValue))
                        {
                            Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_83));
                            return false;
                        }
                    }
                }

                //高级滤镜（修改 - 自发现有连锁的位置）
                for (int i = 0; i < this.dgvFilterAdvanced_Modify_FromPosition.Columns.Count; i++)
                {
                    if (dgvFilterAdvanced_Modify_FromPosition.Rows[0].Cells[i].Value != null)
                    {
                        sCheckValue = dgvFilterAdvanced_Modify_FromPosition.Rows[0].Cells[i].Value.ToString().Trim();
                        if (!Socket_Operation.IsValidFilterString(sCheckValue))
                        {
                            Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_83));
                            return false;
                        }
                    }
                }

                //换包（数据完整度检测）
                if (this.rbFilterAction_Change.Checked)
                {
                    int iMaxIndex = 0;

                    //普通滤镜
                    if (this.rbFilterMode_Normal.Checked)
                    {
                        for (int i = 0; i < this.dgvFilterNormal.Columns.Count; i++)
                        {
                            if (dgvFilterNormal.Rows[1].Cells[i].Value != null)
                            {
                                sCheckValue = dgvFilterNormal.Rows[1].Cells[i].Value.ToString().Trim();
                                if (!string.IsNullOrEmpty(sCheckValue))
                                {
                                    iMaxIndex = i;
                                }
                            }
                        }

                        if (iMaxIndex == 0)
                        {
                            Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_174));
                            return false;
                        }

                        for (int i = 0; i < iMaxIndex; i++)
                        {
                            if (dgvFilterNormal.Rows[1].Cells[i].Value == null)
                            {
                                Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_174));
                                return false;
                            }
                        }
                    }

                    //高级滤镜（从头开始）
                    if (this.rbFilterMode_Advanced.Checked && this.rbFilterModifyFrom_Head.Checked)
                    {
                        for (int i = 0; i < this.dgvFilterAdvanced_Modify_FromHead.Columns.Count; i++)
                        {
                            if (dgvFilterAdvanced_Modify_FromHead.Rows[0].Cells[i].Value != null)
                            {
                                sCheckValue = dgvFilterAdvanced_Modify_FromHead.Rows[0].Cells[i].Value.ToString().Trim();
                                if (!string.IsNullOrEmpty(sCheckValue))
                                {
                                    iMaxIndex = i;
                                }
                            }
                        }

                        if (iMaxIndex == 0)
                        {
                            Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_174));
                            return false;
                        }

                        for (int i = 0; i < iMaxIndex; i++)
                        {
                            if (dgvFilterAdvanced_Modify_FromHead.Rows[0].Cells[i].Value == null)
                            {
                                Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_174));
                                return false;
                            }
                        }
                    }

                    //高级滤镜（从发现有连锁的位置）
                    if (this.rbFilterMode_Advanced.Checked && this.rbFilterModifyFrom_Position.Checked)
                    {
                        int iStartIndex = Socket_Cache.Filter.FilterSize_MaxLen;

                        for (int i = iStartIndex; i < this.dgvFilterAdvanced_Modify_FromPosition.Columns.Count; i++)
                        {
                            if (dgvFilterAdvanced_Modify_FromPosition.Rows[0].Cells[i].Value != null)
                            {
                                sCheckValue = dgvFilterAdvanced_Modify_FromPosition.Rows[0].Cells[i].Value.ToString().Trim();
                                if (!string.IsNullOrEmpty(sCheckValue))
                                {
                                    iMaxIndex = i;
                                }
                            }
                        }

                        if (iMaxIndex == iStartIndex)
                        {
                            Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_174));
                            return false;
                        }

                        for (int i = iStartIndex; i < iMaxIndex; i++)
                        {
                            if (dgvFilterAdvanced_Modify_FromPosition.Rows[0].Cells[i].Value == null)
                            {
                                Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_174));
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
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
                if (this.CheckFilterIsValid())
                {
                    string sFName_New = this.txtFilterName.Text.Trim();
                    string sHeaderContent_New = string.Empty;
                    string sLengthContent_New = string.Empty;
                    decimal dSocketContent_New = 0;                    
                    decimal dPortContent_New = 0;
                    decimal dProgressionStep_New = 0;                    
                    int iProgressionCount = 0;                    
                    bool bIsExecute;
                    bool bAppointHeader, bAppointSocket, bAppointLength, bAppointPort;
                    StringBuilder sbProgression = new StringBuilder();
                    StringBuilder sbSearch = new StringBuilder();
                    StringBuilder sbModify = new StringBuilder();

                    Socket_Cache.Filter.FilterMode FilterMode_New;
                    Socket_Cache.Filter.FilterAction FilterAction_New;
                    Socket_Cache.Filter.FilterExecuteType FilterExecuteType_New;
                    Guid SID_New = Guid.Empty;
                    Guid RID_New = Guid.Empty;
                    Socket_Cache.Filter.FilterFunction FilterFunction_New;
                    Socket_Cache.Filter.FilterStartFrom FilterStartFrom_New;

                    bIsExecute = this.cbFilterAction_Execute.Checked;
                    bAppointHeader = this.cbFilter_AppointHeader.Checked;
                    bAppointSocket = this.cbFilter_AppointSocket.Checked;
                    bAppointLength = this.cbFilter_AppointLength.Checked;
                    bAppointPort = this.cbFilter_AppointPort.Checked;

                    sHeaderContent_New = this.txtFilter_HeaderContent.Text.Trim();
                    sLengthContent_New = this.nudFilter_LengthContent_From.Value.ToString() + "-" + this.nudFilter_LengthContent_To.Value.ToString();
                    dSocketContent_New = this.nudFilter_SocketContent.Value;                    
                    dPortContent_New = this.nudFilter_PortContent.Value;
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
                    else if (rbFilterAction_Change.Checked)
                    {
                        FilterAction_New = Socket_Cache.Filter.FilterAction.Change;
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

                    if (cbFilterAction_Execute.Checked)
                    {
                        if (this.cbbFilterAction_ExecuteType.SelectedIndex == 0)
                        {
                            FilterExecuteType_New = Socket_Cache.Filter.FilterExecuteType.Send;

                            if (cbbFilterAction_Execute.SelectedValue != null)
                            {
                                SID_New = (Guid)cbbFilterAction_Execute.SelectedValue;
                            }
                        }
                        else if (this.cbbFilterAction_ExecuteType.SelectedIndex == 1)
                        {
                            FilterExecuteType_New = Socket_Cache.Filter.FilterExecuteType.Robot;

                            if (cbbFilterAction_Execute.SelectedValue != null)
                            {
                                RID_New = (Guid)cbbFilterAction_Execute.SelectedValue;
                            }
                        }
                        else
                        {
                            FilterExecuteType_New = new Socket_Cache.Filter.FilterExecuteType();
                        }
                    }
                    else
                    {
                        FilterExecuteType_New = new Socket_Cache.Filter.FilterExecuteType();
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
                                    sbProgression.Append(i).Append(",");
                                }

                                if (dgvFilterNormal.Rows[0].Cells[i].Value != null)
                                {
                                    string sSearchValue = dgvFilterNormal.Rows[0].Cells[i].Value.ToString().Trim();

                                    if (!String.IsNullOrEmpty(sSearchValue))
                                    {
                                        sbSearch.Append(i).Append("|").Append(sSearchValue).Append(",");
                                    }
                                }

                                if (dgvFilterNormal.Rows[1].Cells[i].Value != null)
                                {
                                    string sModifyValue = dgvFilterNormal.Rows[1].Cells[i].Value.ToString().Trim();

                                    if (!String.IsNullOrEmpty(sModifyValue))
                                    {
                                        sbModify.Append(i).Append("|").Append(sModifyValue).Append(",");
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
                                    sbSearch.Append(i).Append("|").Append(sValue).Append(",");
                                }
                            }

                            switch (FilterStartFrom_New)
                            {
                                case Socket_Cache.Filter.FilterStartFrom.Head:

                                    for (int i = 0; i < this.dgvFilterAdvanced_Modify_FromHead.Columns.Count; i++)
                                    {
                                        if (dgvFilterAdvanced_Modify_FromHead.Rows[0].Cells[i].Style.BackColor == Color.DarkRed)
                                        {
                                            sbProgression.Append(i).Append(",");
                                        }

                                        if (dgvFilterAdvanced_Modify_FromHead.Rows[0].Cells[i].Value != null)
                                        {
                                            string sValue = dgvFilterAdvanced_Modify_FromHead.Rows[0].Cells[i].Value.ToString().Trim();

                                            if (!String.IsNullOrEmpty(sValue))
                                            {
                                                sbModify.Append(i).Append("|").Append(sValue).Append(",");
                                            }
                                        }
                                    }

                                    break;

                                case Socket_Cache.Filter.FilterStartFrom.Position:

                                    for (int i = 0; i < this.dgvFilterAdvanced_Modify_FromPosition.Columns.Count; i++)
                                    {
                                        int iIndex = int.Parse(dgvFilterAdvanced_Modify_FromPosition.Rows[0].Cells[i].OwningColumn.HeaderText);

                                        if (dgvFilterAdvanced_Modify_FromPosition.Rows[0].Cells[i].Style.BackColor == Color.DarkRed)
                                        {                                            
                                            sbProgression.Append(iIndex).Append(",");
                                        }

                                        if (dgvFilterAdvanced_Modify_FromPosition.Rows[0].Cells[i].Value != null)
                                        {
                                            string sValue = dgvFilterAdvanced_Modify_FromPosition.Rows[0].Cells[i].Value.ToString().Trim();

                                            if (!String.IsNullOrEmpty(sValue))
                                            {
                                                sbModify.Append(iIndex).Append("|").Append(sValue).Append(",");
                                            }
                                        }
                                    }

                                    break;
                            }

                            break;
                    }

                    string sProgression_New = sbProgression.ToString().TrimEnd(',');
                    string sSearch_New = sbSearch.ToString().TrimEnd(',');
                    string sModify_New = sbModify.ToString().TrimEnd(',');

                    Socket_Cache.Filter.UpdateFilter_ByFilterIndex(
                        this.FilterIndex,
                        sFName_New,
                        bAppointHeader,
                        sHeaderContent_New,
                        bAppointSocket,
                        dSocketContent_New,
                        bAppointLength,
                        sLengthContent_New,
                        bAppointPort,
                        dPortContent_New,
                        FilterMode_New,
                        FilterAction_New,
                        bIsExecute,
                        FilterExecuteType_New,
                        SID_New,
                        RID_New,
                        FilterFunction_New,
                        FilterStartFrom_New,
                        dProgressionStep_New,
                        sProgression_New,
                        iProgressionCount,
                        sSearch_New,
                        sModify_New);

                    this.Close();
                }                
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
                        this.PastePacketData(dgv, sClipboardText);

                        break;

                    case "cmsDGV_Delete":

                        dgv.CurrentCell.Value = null;

                        break;

                    case "cmsDGV_Progression_Enable":

                        if (dgv.Name.Equals("dgvFilterAdvanced_Search"))
                        {
                            Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_73));
                            break;
                        }

                        if (dgv.Name.Equals("dgvFilterNormal"))
                        {
                            int iRowIndex = dgv.CurrentCell.RowIndex;

                            if (iRowIndex == 0)
                            {
                                Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_73));
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

        #endregion        
    }
}
