using AntdUI;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using WPE.Lib;

namespace WPE.InjectMode
{
    public partial class FilterEditForm : Form
    {
        private FilterInfo sfiSelect;
        private DataTable dtFilterNormal = new DataTable();
        private DataTable dtFilterAdvanced_Search = new DataTable();
        private DataTable dtFilterAdvanced_Modify_Head = new DataTable();
        private DataTable dtFilterAdvanced_Modify_Position = new DataTable();

        #region//初始化

        public FilterEditForm(Form form, FilterInfo fi)
        {
            InitializeComponent();

            if (fi == null)
            {
                string Title = AntdUI.Localization.Get("InjectModeForm.EditFilter.Error", "加载滤镜数据出错");
                string Content = AntdUI.Localization.Get("InjectModeForm.CheckSystemLog", "请检查系统日志");
                AntdUI.Notification.error(form, Title, Content, AntdUI.TAlignFrom.TR);
                this.Close();
            }
            else
            {
                this.sfiSelect = fi;
            }  
        }        

        private void FilterEditForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.tabFilterEdit.TabMenuVisible = false;
                this.tabFilterFrom.TabMenuVisible = false;
                this.InitTable_FilterNormal();
                this.InitTable_FilterAdvanced_Search();
                this.InitTable_FilterAdvanced_Modify_Head();
                this.InitTable_FilterAdvanced_Modify_Position();            
                this.InitProgressionPosition();
                this.InitFilterExecuteType();

                switch (sfiSelect.FMode)
                {
                    case Operate.FilterConfig.Filter.FilterMode.Normal:
                        this.rbFilterMode_Normal.Checked = true;
                        break;

                    case Operate.FilterConfig.Filter.FilterMode.Advanced:
                        this.rbFilterMode_Advanced.Checked = true;
                        break;
                }
                this.FilterModeChange();

                switch (sfiSelect.FAction)
                {
                    case Operate.FilterConfig.Filter.FilterAction.Replace:
                        this.rbFilterAction_Replace.Checked = true;
                        break;

                    case Operate.FilterConfig.Filter.FilterAction.Intercept:
                        this.rbFilterAction_Intercept.Checked = true;
                        break;

                    case Operate.FilterConfig.Filter.FilterAction.Change:
                        this.rbFilterAction_Change.Checked = true;
                        break;

                    case Operate.FilterConfig.Filter.FilterAction.NoModify_Display:
                        this.rbFilterAction_NoModify_Display.Checked = true;
                        break;

                    case Operate.FilterConfig.Filter.FilterAction.NoModify_NoDisplay:
                        this.rbFilterAction_NoModify_NoDisplay.Checked = true;
                        break;
                }

                switch (sfiSelect.FStartFrom)
                {
                    case Operate.FilterConfig.Filter.FilterStartFrom.Head:
                        this.rbFilterModifyFrom_Head.Checked = true;
                        break;

                    case Operate.FilterConfig.Filter.FilterStartFrom.Position:
                        this.rbFilterModifyFrom_Position.Checked = true;
                        break;
                }
                this.FilterModifyFromChange();

                this.cbFilterAction_Execute.Checked = sfiSelect.IsExecute;
                this.FilterAction_ExecuteChange();

                switch (sfiSelect.FEType)
                {
                    case Operate.FilterConfig.Filter.FilterExecuteType.Send:
                        this.cbbFilterAction_ExecuteType.SelectedIndex = 0;
                        break;

                    case Operate.FilterConfig.Filter.FilterExecuteType.Robot:
                        this.cbbFilterAction_ExecuteType.SelectedIndex = 1;
                        break;

                    default:
                        this.cbbFilterAction_ExecuteType.SelectedIndex = -1;
                        break;
                }
                this.FilterAction_ExecuteTypeChanged();

                this.cbFilter_AppointHeader.Checked = sfiSelect.AppointHeader;
                this.txtFilter_HeaderContent.Text = sfiSelect.HeaderContent;
                this.FilterAppointHeaderChange();

                this.cbFilter_AppointSocket.Checked = sfiSelect.AppointSocket;
                this.txtFilter_SocketContent.Text = sfiSelect.SocketContent;
                this.FilterAppointSocketChange();

                this.cbFilter_AppointLength.Checked = sfiSelect.AppointLength;
                this.txtFilter_LengthContent.Text = sfiSelect.LengthContent;
                this.FilterAppointLengthChange();

                this.cbFilter_AppointPort.Checked = sfiSelect.AppointPort;
                this.txtFilter_PortContent.Text = sfiSelect.PortContent;
                this.FilterAppointPortChange();

                this.cbProgressionContinuous.Checked = sfiSelect.IsProgressionContinuous;
                this.nudProgressionStep.Value = sfiSelect.ProgressionStep;
                this.cbProgressionCarry.Checked = sfiSelect.IsProgressionCarry;
                this.nudProgressionCarry.Value = sfiSelect.ProgressionCarryNumber;
                this.ProgressionCarryChange();

                this.txtFilterName.Text = sfiSelect.FName;
                this.cbFilterFunction_Send.Checked = sfiSelect.FFunction.Send;
                this.cbFilterFunction_SendTo.Checked = sfiSelect.FFunction.SendTo;
                this.cbFilterFunction_Recv.Checked = sfiSelect.FFunction.Recv;
                this.cbFilterFunction_RecvFrom.Checked = sfiSelect.FFunction.RecvFrom;
                this.cbFilterFunction_WSASend.Checked = sfiSelect.FFunction.WSASend;
                this.cbFilterFunction_WSASendTo.Checked = sfiSelect.FFunction.WSASendTo;
                this.cbFilterFunction_WSARecv.Checked = sfiSelect.FFunction.WSARecv;
                this.cbFilterFunction_WSARecvFrom.Checked = sfiSelect.FFunction.WSARecvFrom;
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void InitTable_FilterNormal()
        {
            int rowCount = 2;
            var columns = new AntdUI.ColumnCollection();

            for (int i = 0; i < Operate.FilterConfig.Filter.FilterSize_MaxLen; i++)
            {
                string title = (i + 1).ToString("D3");
                string key = "col" + title;

                AntdUI.Column column = new AntdUI.Column(key, title, AntdUI.ColumnAlign.Center).SetWidth("50");
                columns.Add(column);

                dtFilterNormal.Columns.Add(key, typeof(CellText));
            }
            tFilterNormal.Columns = columns;

            Color color;
            for (int row = 0; row < rowCount; row++)
            {
                if (row == 0)
                {
                    color = Color.LightYellow;
                }
                else
                {
                    color = Color.Yellow;
                }

                DataRow dr = dtFilterNormal.NewRow();
                for (int col = 0; col < dtFilterNormal.Columns.Count; col++)
                {
                    dr[col] = new CellText(string.Empty)
                    {
                        Back = color,
                        Fore = Color.RoyalBlue,
                    };
                }

                dtFilterNormal.Rows.Add(dr);
            }

            tFilterNormal.DataSource = dtFilterNormal;
        }

        private void InitTable_FilterAdvanced_Search()
        {
            var columns = new AntdUI.ColumnCollection();

            for (int i = 0; i < Operate.FilterConfig.Filter.FilterSize_MaxLen; i++)
            {
                string Title = (i + 1).ToString("D3");
                string Key = "col" + Title;

                AntdUI.Column column = new AntdUI.Column(Key, Title, AntdUI.ColumnAlign.Center).SetWidth("50");
                columns.Add(column);

                dtFilterAdvanced_Search.Columns.Add(Key, typeof(CellText));
            }
            tFilterAdvanced_Search.Columns = columns;

            DataRow dr = dtFilterAdvanced_Search.NewRow();
            for (int col = 0; col < dtFilterAdvanced_Search.Columns.Count; col++)
            {
                dr[col] = new CellText(string.Empty)
                {
                    Back = Color.LightYellow,
                    Fore = Color.RoyalBlue,
                };
            }

            dtFilterAdvanced_Search.Rows.Add(dr);
            tFilterAdvanced_Search.DataSource = dtFilterAdvanced_Search;
        }

        private void InitTable_FilterAdvanced_Modify_Head()
        {
            var columns = new AntdUI.ColumnCollection();

            for (int i = 0; i < Operate.FilterConfig.Filter.FilterSize_MaxLen; i++)
            {
                string Title = (i + 1).ToString("D3");
                string Key = "col" + Title;

                AntdUI.Column column = new AntdUI.Column(Key, Title, AntdUI.ColumnAlign.Center).SetWidth("50");
                columns.Add(column);

                dtFilterAdvanced_Modify_Head.Columns.Add(Key, typeof(CellText));
            }
            tFilterAdvanced_Modify_Head.Columns = columns;

            DataRow dr = dtFilterAdvanced_Modify_Head.NewRow();
            for (int col = 0; col < dtFilterAdvanced_Modify_Head.Columns.Count; col++)
            {
                dr[col] = new CellText(string.Empty)
                {
                    Back = Color.Yellow,
                    Fore = Color.RoyalBlue,
                };
            }

            dtFilterAdvanced_Modify_Head.Rows.Add(dr);
            tFilterAdvanced_Modify_Head.DataSource = dtFilterAdvanced_Modify_Head;
        }

        private void InitTable_FilterAdvanced_Modify_Position()
        {
            var columns = new AntdUI.ColumnCollection();

            int iSize = Operate.FilterConfig.Filter.FilterSize_MaxLen;
            for (int i = -iSize; i < iSize; i++)
            {
                string Title = i.ToString("D3");
                string Key = "col" + Title;

                AntdUI.Column column = new AntdUI.Column(Key, Title, AntdUI.ColumnAlign.Center).SetWidth("50");
                columns.Add(column);

                dtFilterAdvanced_Modify_Position.Columns.Add(Key, typeof(CellText));
            }
            tFilterAdvanced_Modify_Position.Columns = columns;

            DataRow dr = dtFilterAdvanced_Modify_Position.NewRow();
            for (int col = 0; col < dtFilterAdvanced_Modify_Position.Columns.Count; col++)
            {
                dr[col] = new CellText(string.Empty)
                {
                    Back = Color.Yellow,
                    Fore = Color.RoyalBlue,
                };
            }

            dtFilterAdvanced_Modify_Position.Rows.Add(dr);
            tFilterAdvanced_Modify_Position.DataSource = dtFilterAdvanced_Modify_Position;
            tFilterAdvanced_Modify_Position.ScrollBar.ValueX = iSize * 50;
        }        
        
        private void InitFilterExecuteType()
        { 
            this.cbbFilterAction_ExecuteType.Items.Clear();

            if (Operate.SendConfig.SendList.lstSend.Count > 0)
            {
                this.cbbFilterAction_ExecuteType.Items.Add(new SelectItem("发送列表")
                {
                    Online = 1,
                });
            }
            else
            {
                this.cbbFilterAction_ExecuteType.Items.Add(new SelectItem("发送列表")
                {
                    Enable = false,
                });
            }

            this.cbbFilterAction_ExecuteType.Items.Add(new DividerSelectItem());            

            if (Operate.RobotConfig.RobotList.lstRobot.Count > 0)
            {
                this.cbbFilterAction_ExecuteType.Items.Add(new SelectItem("机器人列表")
                {
                    Online = 1,
                });
            }
            else
            {
                this.cbbFilterAction_ExecuteType.Items.Add(new SelectItem("机器人列表")
                {
                    Enable = false,
                });
            }
        }

        private void InitSendInfo()
        {
            try
            {
                if (Operate.SendConfig.SendList.lstSend.Count > 0)
                {
                    this.cbbFilterAction_Execute.Items.Clear();
                    this.cbbFilterAction_Execute.Items.AddRange(Operate.SendConfig.SendList.lstSend.ToArray());
                    
                    this.cbbFilterAction_Execute.SelectedValue = sfiSelect.SID;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void InitRobotInfo()
        {
            try
            {
                if (Operate.RobotConfig.RobotList.lstRobot.Count > 0)
                {
                    this.cbbFilterAction_Execute.Items.Clear();
                    this.cbbFilterAction_Execute.Items.AddRange(Operate.RobotConfig.RobotList.lstRobot.ToArray());

                    this.cbbFilterAction_Execute.SelectedValue = sfiSelect.RID;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void InitProgressionPosition()
        {
            try
            {
                if (!string.IsNullOrEmpty(sfiSelect.ProgressionPosition))
                {
                    string[] slProgressionPosition = sfiSelect.ProgressionPosition.Split(',');

                    foreach (string sPosition in slProgressionPosition)
                    {
                        if (!string.IsNullOrEmpty(sPosition))
                        {
                            if (int.TryParse(sPosition, out int iIndex))
                            {
                                switch (sfiSelect.FMode)
                                {
                                    case Operate.FilterConfig.Filter.FilterMode.Normal:

                                        ((CellText)this.dtFilterNormal.Rows[1][iIndex]).Back = Color.DarkRed;                  

                                        break;

                                    case Operate.FilterConfig.Filter.FilterMode.Advanced:

                                        switch (sfiSelect.FStartFrom)
                                        {
                                            case Operate.FilterConfig.Filter.FilterStartFrom.Head:

                                                ((CellText)this.dtFilterAdvanced_Modify_Head.Rows[0][iIndex]).Back = Color.DarkRed;

                                                break;

                                            case Operate.FilterConfig.Filter.FilterStartFrom.Position:

                                                iIndex += Operate.FilterConfig.Filter.FilterSize_MaxLen;
                                                ((CellText)this.dtFilterAdvanced_Modify_Position.Rows[0][iIndex]).Back = Color.DarkRed;

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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion        

        #region//验证并处理十六进制字符输入

        public void VerifyHexChar(InputVerifyCharEventArgs verifyArgs)
        {
            char c = verifyArgs.Char;
            if (c == '\b')
            {
                verifyArgs.Result = true;
                return;
            }

            if (char.IsDigit(c))
            {
                verifyArgs.Result = true;
            }
            else if (c >= 'A' && c <= 'F')
            {
                verifyArgs.Result = true;
            }
            else if (c >= 'a' && c <= 'f')
            {
                verifyArgs.ReplaceText = c.ToString().ToUpper();
                verifyArgs.Result = true;
            }
            else
            {
                verifyArgs.Result = false;
            }
        }

        public bool ValidateHexValueAndShowMessage(string ValidateHex)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(ValidateHex, "^[0-9A-F]{2}$"))
            {
                AntdUI.Message.open(new AntdUI.Message.Config(this, "请输入有效的十六进制数值", TType.Error)
                {
                    LocalizationText = "InvalidHex"
                });

                return false;
            }

            return true;
        }

        private void tFilterNormal_CellBeginEditInputStyle(object sender, TableBeginEditInputStyleEventArgs e)
        {
            e.Input.MaxLength = 2;
            e.Input.VerifyChar += (inputSender, verifyArgs) =>
            {
                VerifyHexChar(verifyArgs);
            };
        }

        private void tFilterAdvanced_Search_CellBeginEditInputStyle(object sender, TableBeginEditInputStyleEventArgs e)
        {
            e.Input.MaxLength = 2;
            e.Input.VerifyChar += (inputSender, verifyArgs) =>
            {
                VerifyHexChar(verifyArgs);
            };
        }

        private void tFilterAdvanced_Modify_Head_CellBeginEditInputStyle(object sender, TableBeginEditInputStyleEventArgs e)
        {
            e.Input.MaxLength = 2;
            e.Input.VerifyChar += (inputSender, verifyArgs) =>
            {
                VerifyHexChar(verifyArgs);
            };
        }

        private void tFilterAdvanced_Modify_Position_CellBeginEditInputStyle(object sender, TableBeginEditInputStyleEventArgs e)
        {
            e.Input.MaxLength = 2;
            e.Input.VerifyChar += (inputSender, verifyArgs) =>
            {
                VerifyHexChar(verifyArgs);
            };
        }

        private bool tFilterNormal_CellEndEdit(object sender, TableEndEditEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Value))
            {
                return true;
            }

            return ValidateHexValueAndShowMessage(e.Value);
        }

        private bool tFilterAdvanced_Search_CellEndEdit(object sender, TableEndEditEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Value))
            {
                return true;
            }

            return ValidateHexValueAndShowMessage(e.Value);
        }

        private bool tFilterAdvanced_Modify_Head_CellEndEdit(object sender, TableEndEditEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Value))
            {
                return true;
            }

            return ValidateHexValueAndShowMessage(e.Value);
        }

        private bool tFilterAdvanced_Modify_Position_CellEndEdit(object sender, TableEndEditEventArgs e)
        {
            if (string.IsNullOrEmpty(e.Value))
            {
                return true;
            }

            return ValidateHexValueAndShowMessage(e.Value);
        }

        #endregion

        #region//模式切换

        private void rbFilterMode_Normal_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.FilterModeChange();
        }

        private void FilterModeChange()
        {
            if (rbFilterMode_Normal.Checked)
            {
                this.tabFilterEdit.SelectTab("tpNormal");
                this.pFilterModifyFrom.Enabled = false;
            }
            else if (rbFilterMode_Advanced.Checked)
            {
                this.tabFilterEdit.SelectTab("tpAdvance");
                this.pFilterModifyFrom.Enabled = true;
            }
        }

        #endregion

        #region//修改起始于切换

        private void rbFilterModifyFrom_Head_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.FilterModifyFromChange();
        }

        private void FilterModifyFromChange()
        {
            if (rbFilterModifyFrom_Head.Checked)
            {
                this.tabFilterFrom.SelectTab("tpFromHead");
            }
            else if (rbFilterModifyFrom_Position.Checked)
            {
                this.tabFilterFrom.SelectTab("tpFromPosition");
            }
        }

        #endregion

        #region//执行

        private void cbFilterAction_Execute_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.FilterAction_ExecuteChange();
        }

        private void FilterAction_ExecuteChange()
        {
            this.cbbFilterAction_Execute.Enabled = this.cbbFilterAction_ExecuteType.Enabled = cbFilterAction_Execute.Checked;
        }

        private void cbbFilterAction_ExecuteType_SelectedIndexChanged(object sender, IntEventArgs e)
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//指定包头

        private void cbFilter_AppointHeader_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.FilterAppointHeaderChange();
        }

        private void FilterAppointHeaderChange()
        {
            this.txtFilter_HeaderContent.Enabled = this.cbFilter_AppointHeader.Checked;
        }

        #endregion

        #region//指定套接字

        private void cbFilter_AppointSocket_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.FilterAppointSocketChange();
        }

        private void FilterAppointSocketChange()
        {
            this.txtFilter_SocketContent.Enabled = this.cbFilter_AppointSocket.Checked;
        }

        #endregion

        #region//指定长度

        private void cbFilter_AppointLength_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.FilterAppointLengthChange();
        }

        private void FilterAppointLengthChange()
        {
            this.txtFilter_LengthContent.Enabled = this.cbFilter_AppointLength.Checked;
        }

        #endregion

        #region//指定端口

        private void cbFilter_AppointPort_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.FilterAppointPortChange();
        }

        private void FilterAppointPortChange()
        {
            this.txtFilter_PortContent.Enabled = this.cbFilter_AppointPort.Checked;
        }

        #endregion

        #region//进位递进

        private void cbProgressionCarry_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.ProgressionCarryChange();
        }

        private void ProgressionCarryChange()
        {
            this.nudProgressionCarry.Enabled = this.cbProgressionCarry.Checked;
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {

            
        }

        #endregion

        #region//退出

        private void bExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }


        #endregion

        #region//右键菜单

        private void InitCMS(AntdUI.Table tFilterEdit, DataTable dtFilterEdit, int RowIndex, int ColumnIndex)
        {
            AntdUI.IContextMenuStripItem[] menulist = { };

            if (RowIndex == 2 || tFilterEdit == tFilterAdvanced_Modify_Head || tFilterEdit == tFilterAdvanced_Modify_Position)
            {
                menulist = new AntdUI.IContextMenuStripItem[]
                {
                    new AntdUI.ContextMenuStripItem("启用递进")
                {
                    ID = "cmsFilterEdit_Progression_Enable",
                    IconSvg = "CheckSquareFilled",
                    LocalizationText = "InjectModeForm.cmsFilterList.Top",
                },
                    new AntdUI.ContextMenuStripItem("取消递进")
                {
                    ID = "cmsFilterEdit_Progression_Disable",
                    IconSvg = "CloseSquareOutlined",
                },
                    new AntdUI.ContextMenuStripItemDivider(),
                    new AntdUI.ContextMenuStripItem("复制", "Ctrl+C")
                {
                ID = "cmsFilterEdit_Copy",
                    IconSvg = "CopyOutlined",
                },
                    new AntdUI.ContextMenuStripItem("剪切", "Ctrl+X")
                {
                    ID = "cmsFilterEdit_Cut",
                    IconSvg = "ScissorOutlined",
                },
                    new AntdUI.ContextMenuStripItem("粘贴", "Ctrl+V")
                {
                    ID = "cmsFilterEdit_Paste",
                    IconSvg = "SnippetsOutlined",
                },
                    new AntdUI.ContextMenuStripItem("删除")
                {
                    ID = "cmsFilterEdit_Delete",
                    IconSvg = "DeleteOutlined",
                },
                };
            }
            else
            {
                menulist = new AntdUI.IContextMenuStripItem[]
                {
                    new AntdUI.ContextMenuStripItem("复制", "Ctrl+C")
                {
                ID = "cmsFilterEdit_Copy",
                    IconSvg = "CopyOutlined",
                },
                    new AntdUI.ContextMenuStripItem("剪切", "Ctrl+X")
                {
                    ID = "cmsFilterEdit_Cut",
                    IconSvg = "ScissorOutlined",
                },
                    new AntdUI.ContextMenuStripItem("粘贴", "Ctrl+V")
                {
                    ID = "cmsFilterEdit_Paste",
                    IconSvg = "SnippetsOutlined",
                },
                    new AntdUI.ContextMenuStripItem("删除")
                {
                    ID = "cmsFilterEdit_Delete",
                    IconSvg = "DeleteOutlined",
                },
                };
            }

            AntdUI.ContextMenuStrip.open(tFilterEdit, item =>
            {
                string CellValue = string.Empty;
                switch (item.ID)
                {
                    case "cmsFilterEdit_Progression_Enable":

                        if (RowIndex == 2)
                        {
                            ((CellText)dtFilterEdit.Rows[RowIndex - 1][ColumnIndex]).Back = Color.DarkRed;
                            tFilterEdit.Refresh();
                        }

                        break;

                    case "cmsFilterEdit_Progression_Disable":

                        if (RowIndex == 2)
                        {
                            ((CellText)dtFilterEdit.Rows[RowIndex - 1][ColumnIndex]).Back = Color.Yellow;
                            tFilterEdit.Refresh();
                        }

                        break;

                    case "cmsFilterEdit_Copy":

                        CellValue = ((CellText)dtFilterEdit.Rows[RowIndex - 1][ColumnIndex]).Text;

                        if (!string.IsNullOrEmpty(CellValue))
                        {
                            Clipboard.SetText(CellValue);
                        }

                        break;

                    case "cmsFilterEdit_Cut":

                        CellValue = ((CellText)dtFilterEdit.Rows[RowIndex - 1][ColumnIndex]).Text;

                        if (!string.IsNullOrEmpty(CellValue))
                        {
                            Clipboard.SetText(CellValue);
                            ((CellText)dtFilterEdit.Rows[RowIndex - 1][ColumnIndex]).Text = string.Empty;
                        }

                        break;

                    case "cmsFilterEdit_Paste":

                        string sClipboardText = Clipboard.GetText().Trim();
                        this.PastePacketData(tFilterEdit, dtFilterEdit, RowIndex - 1, ColumnIndex, sClipboardText);                        

                        break;

                    case "cmsFilterEdit_Delete":

                        ((CellText)dtFilterEdit.Rows[RowIndex - 1][ColumnIndex]).Text = string.Empty;

                        break;
                }
            }, menulist);
        }

        private void tFilterNormal_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                InitCMS(tFilterNormal, dtFilterNormal, e.RowIndex, e.ColumnIndex);                
            }
        }

        private void tFilterAdvanced_Search_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                InitCMS(tFilterAdvanced_Search, dtFilterAdvanced_Search, e.RowIndex, e.ColumnIndex);
            }
        }

        private void tFilterAdvanced_Modify_Head_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                InitCMS(tFilterAdvanced_Modify_Head, dtFilterAdvanced_Modify_Head, e.RowIndex, e.ColumnIndex);
            }
        }        

        private void tFilterAdvanced_Modify_Position_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                InitCMS(tFilterAdvanced_Modify_Position, dtFilterAdvanced_Modify_Position, e.RowIndex, e.ColumnIndex);
            }
        }

        #endregion

        #region//粘贴数据

        private void tFilterNormal_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                Point pMouse = PointToClient(MousePosition);
                tFilterNormal.HitTest(pMouse.X, pMouse.Y, out int RIndex, out int CIndex);

                MessageBox.Show(pMouse.X + " | " + pMouse.Y + " - " + RIndex + " | " + CIndex);

                if (RIndex > 0 && CIndex > -1)
                {
                    string sClipboardText = Clipboard.GetText().Trim();
                    this.PastePacketData(tFilterNormal, dtFilterNormal, RIndex - 1, CIndex, sClipboardText);
                }
            }
        }

        private void tFilterAdvanced_Search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                Point pMouse = PointToClient(MousePosition);
                tFilterAdvanced_Search.HitTest(pMouse.X, pMouse.Y, out int RIndex, out int CIndex);

                MessageBox.Show(pMouse.X + " | " + pMouse.Y + " - " + RIndex + " | " + CIndex);

                if (RIndex > 0 && CIndex > -1)
                {
                    string sClipboardText = Clipboard.GetText().Trim();
                    this.PastePacketData(tFilterAdvanced_Search, dtFilterAdvanced_Search, RIndex - 1, CIndex, sClipboardText);
                }
            }
        }

        private void tFilterAdvanced_Modify_Head_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                Point pMouse = PointToClient(MousePosition);
                tFilterAdvanced_Modify_Head.HitTest(pMouse.X, pMouse.Y, out int RIndex, out int CIndex);

                MessageBox.Show(pMouse.X + " | " + pMouse.Y + " - " + RIndex + " | " + CIndex);

                if (RIndex > 0 && CIndex > -1)
                {
                    string sClipboardText = Clipboard.GetText().Trim();
                    this.PastePacketData(tFilterAdvanced_Modify_Head, dtFilterAdvanced_Modify_Head, RIndex - 1, CIndex, sClipboardText);
                }
            }
        }

        private void tFilterAdvanced_Modify_Position_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.V)
            {
                Point pMouse = PointToClient(MousePosition);
                tFilterAdvanced_Modify_Position.HitTest(pMouse.X, pMouse.Y, out int RIndex, out int CIndex);

                MessageBox.Show(pMouse.X + " | " + pMouse.Y + " - " + RIndex + " | " + CIndex);

                if (RIndex > 0 && CIndex > -1)
                {
                    string sClipboardText = Clipboard.GetText().Trim();
                    this.PastePacketData(tFilterAdvanced_Modify_Position, dtFilterAdvanced_Modify_Position, RIndex - 1, CIndex, sClipboardText);
                }                    
            }
        }

        private void PastePacketData(AntdUI.Table tTable, DataTable dt, int RIndex, int CIndex, string sData)
        {
            try
            {
                bool bOK = false;
                tTable.Spin(AntdUI.Localization.Get("Loading", "正在加载..."), config =>
                {
                    this.bSave.Loading = true;

                    if (!string.IsNullOrEmpty(sData) && Operate.SystemConfig.IsHexString(sData))
                    {
                        string[] DataCells = sData.Split(' ');
                        for (int i = 0; i < DataCells.Length; i++)
                        {
                            if (CIndex + i < dt.Columns.Count)
                            {
                                ((CellText)dt.Rows[RIndex][CIndex + i]).Text = DataCells[i].ToUpper();
                            }
                            else
                            {
                                break;
                            }
                        }

                        bOK = true;
                    }
                    else
                    {
                        AntdUI.Message.open(new AntdUI.Message.Config(this, "请输入有效的十六进制数值", TType.Error)
                        {
                            LocalizationText = "InvalidHex"
                        });
                    }
                }, () =>
                {
                    if (bOK) 
                    {
                        AntdUI.Message.open(new AntdUI.Message.Config(this, "数据粘贴完毕", TType.Success)
                        {
                            LocalizationText = "FilterEditForm.Paste.Success"
                        });
                    }

                    this.bSave.Loading = false;
                });
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(PastePacketData), ex.Message);
            }
        }

        #endregion        

        

        
    }
}
