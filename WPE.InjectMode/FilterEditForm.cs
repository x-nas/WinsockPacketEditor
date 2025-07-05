using AntdUI;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WPE.Lib;

namespace WPE.InjectMode
{
    public partial class FilterEditForm : Form
    {
        private InjectModeForm imForm;
        private FilterInfo fiSelect;
        private DataTable dtFilterNormal = new DataTable();
        private DataTable dtFilterAdvanced_Search = new DataTable();
        private DataTable dtFilterAdvanced_Modify_Head = new DataTable();
        private DataTable dtFilterAdvanced_Modify_Position = new DataTable();

        #region//初始化

        public FilterEditForm(InjectModeForm form, FilterInfo fi)
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
                this.fiSelect = fi;
                this.imForm = form;
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
                this.ShowFilterData();

                switch (fiSelect.FMode)
                {
                    case Operate.FilterConfig.Filter.FilterMode.Normal:
                        this.rbFilterMode_Normal.Checked = true;
                        break;

                    case Operate.FilterConfig.Filter.FilterMode.Advanced:
                        this.rbFilterMode_Advanced.Checked = true;
                        break;
                }
                this.FilterModeChange();

                switch (fiSelect.FAction)
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

                switch (fiSelect.FStartFrom)
                {
                    case Operate.FilterConfig.Filter.FilterStartFrom.Head:
                        this.rbFilterModifyFrom_Head.Checked = true;
                        break;

                    case Operate.FilterConfig.Filter.FilterStartFrom.Position:
                        this.rbFilterModifyFrom_Position.Checked = true;
                        break;
                }
                this.FilterModifyFromChange();

                this.cbFilterAction_Execute.Checked = fiSelect.IsExecute;
                this.FilterAction_ExecuteChange();

                switch (fiSelect.FEType)
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

                this.cbFilter_AppointHeader.Checked = fiSelect.AppointHeader;
                this.txtFilter_HeaderContent.Text = fiSelect.HeaderContent;
                this.FilterAppointHeaderChange();

                this.cbFilter_AppointSocket.Checked = fiSelect.AppointSocket;
                this.txtFilter_SocketContent.Text = fiSelect.SocketContent;
                this.FilterAppointSocketChange();

                this.cbFilter_AppointLength.Checked = fiSelect.AppointLength;
                this.txtFilter_LengthContent.Text = fiSelect.LengthContent;
                this.FilterAppointLengthChange();

                this.cbFilter_AppointPort.Checked = fiSelect.AppointPort;
                this.txtFilter_PortContent.Text = fiSelect.PortContent;
                this.FilterAppointPortChange();

                this.cbProgressionContinuous.Checked = fiSelect.IsProgressionContinuous;
                this.nudProgressionStep.Value = fiSelect.ProgressionStep;
                this.cbProgressionCarry.Checked = fiSelect.IsProgressionCarry;
                this.nudProgressionCarry.Value = fiSelect.ProgressionCarryNumber;
                this.ProgressionCarryChange();

                this.txtFilterName.Text = fiSelect.FName;
                this.cbFilterFunction_Send.Checked = fiSelect.FFunction.Send;
                this.cbFilterFunction_SendTo.Checked = fiSelect.FFunction.SendTo;
                this.cbFilterFunction_Recv.Checked = fiSelect.FFunction.Recv;
                this.cbFilterFunction_RecvFrom.Checked = fiSelect.FFunction.RecvFrom;
                this.cbFilterFunction_WSASend.Checked = fiSelect.FFunction.WSASend;
                this.cbFilterFunction_WSASendTo.Checked = fiSelect.FFunction.WSASendTo;
                this.cbFilterFunction_WSARecv.Checked = fiSelect.FFunction.WSARecv;
                this.cbFilterFunction_WSARecvFrom.Checked = fiSelect.FFunction.WSARecvFrom;
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
                string Title = (i + 1).ToString("D3");

                AntdUI.Column column = new AntdUI.Column(Title, Title, AntdUI.ColumnAlign.Center).SetWidth("50");
                columns.Add(column);

                dtFilterNormal.Columns.Add(Title, typeof(CellText));
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

                AntdUI.Column column = new AntdUI.Column(Title, Title, AntdUI.ColumnAlign.Center).SetWidth("50");
                columns.Add(column);

                dtFilterAdvanced_Search.Columns.Add(Title, typeof(CellText));
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

                AntdUI.Column column = new AntdUI.Column(Title, Title, AntdUI.ColumnAlign.Center).SetWidth("50");
                columns.Add(column);

                dtFilterAdvanced_Modify_Head.Columns.Add(Title, typeof(CellText));
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

                AntdUI.Column column = new AntdUI.Column(Title, Title, AntdUI.ColumnAlign.Center).SetWidth("50");
                columns.Add(column);

                dtFilterAdvanced_Modify_Position.Columns.Add(Title, typeof(CellText));
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

            if (Operate.SendConfig.List.lstSendInfo.Count > 0)
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
                if (Operate.SendConfig.List.lstSendInfo.Count > 0)
                {
                    var selectItems = Operate.SendConfig.List.lstSendInfo.Select(info => new SelectItem(info.SName, info)).ToArray();

                    this.cbbFilterAction_Execute.Items.Clear();
                    this.cbbFilterAction_Execute.Items.AddRange(selectItems);                    
                    this.cbbFilterAction_Execute.SelectedValue = Operate.SendConfig.Send.GetSend_ByGuid(fiSelect.SID);
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
                    var selectItems = Operate.RobotConfig.RobotList.lstRobot.Select(info => new SelectItem(info.RName, info)).ToArray();

                    this.cbbFilterAction_Execute.Items.Clear();
                    this.cbbFilterAction_Execute.Items.AddRange(selectItems);
                    this.cbbFilterAction_Execute.SelectedValue = Operate.RobotConfig.Robot.GeRobot_ByGuid(fiSelect.RID);
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
                if (!string.IsNullOrEmpty(fiSelect.ProgressionPosition))
                {
                    string[] slProgressionPosition = fiSelect.ProgressionPosition.Split(',');

                    foreach (string sPosition in slProgressionPosition)
                    {
                        if (!string.IsNullOrEmpty(sPosition))
                        {
                            if (int.TryParse(sPosition, out int iIndex))
                            {
                                switch (fiSelect.FMode)
                                {
                                    case Operate.FilterConfig.Filter.FilterMode.Normal:

                                        ((CellText)this.dtFilterNormal.Rows[1][iIndex]).Back = Color.DarkRed;                  

                                        break;

                                    case Operate.FilterConfig.Filter.FilterMode.Advanced:

                                        switch (fiSelect.FStartFrom)
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
            this.Filter_HeaderContent_Changed();
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
            this.Filter_SocketContent_Changed();
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
            this.Filter_LengthContent_Changed();
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
            this.Filter_PortContent_Changed();
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

        #region//滤镜设置合法性检测

        public bool CheckFilterIsValid()
        {
            bool bReturn = true;

            try
            {
                //滤镜名称
                if (string.IsNullOrEmpty(this.txtFilterName.Text.Trim()))
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this, "滤镜名称为空", TType.Error)
                    {
                        LocalizationText = "FilterEditForm.FilterName.Error"
                    });

                    return false;
                }

                //指定包头
                if (this.cbFilter_AppointHeader.Checked)
                {
                    if (string.IsNullOrEmpty(this.txtFilter_HeaderContent.Text.Trim()))
                    {
                        AntdUI.Message.open(new AntdUI.Message.Config(this, "指定包头数据错误", TType.Error)
                        {
                            LocalizationText = "FilterEditForm.AppointHeader.Error"
                        });

                        return false;
                    }
                    else
                    {
                        if (!Operate.SystemConfig.IsHexString(this.txtFilter_HeaderContent.Text.Trim()))
                        {
                            AntdUI.Message.open(new AntdUI.Message.Config(this, "指定包头数据错误", TType.Error)
                            {
                                LocalizationText = "FilterEditForm.AppointHeader.Error"
                            });

                            return false;
                        }
                    }
                }

                //指定套接字
                if (this.cbFilter_AppointSocket.Checked)
                {
                    if (string.IsNullOrEmpty(this.txtFilter_SocketContent.Text.Trim()))
                    {
                        AntdUI.Message.open(new AntdUI.Message.Config(this, "指定套接字错误", TType.Error)
                        {
                            LocalizationText = "FilterEditForm.AppointSocket.Error"
                        });

                        return false;
                    }
                    else
                    {
                        if (!Regex.IsMatch(this.txtFilter_SocketContent.Text.Trim(), @"^(\d+)(;\d+)*$"))
                        {
                            AntdUI.Message.open(new AntdUI.Message.Config(this, "指定套接字错误", TType.Error)
                            {
                                LocalizationText = "FilterEditForm.AppointSocket.Error"
                            });

                            return false;
                        }
                    }
                }

                //指定端口
                if (this.cbFilter_AppointPort.Checked)
                {
                    if (string.IsNullOrEmpty(this.txtFilter_PortContent.Text.Trim()))
                    {
                        AntdUI.Message.open(new AntdUI.Message.Config(this, "指定端口错误", TType.Error)
                        {
                            LocalizationText = "FilterEditForm.AppointPort.Error"
                        });

                        return false;
                    }
                    else
                    {
                        if (!Regex.IsMatch(this.txtFilter_PortContent.Text.Trim(), @"^(\d+[-;])*\d+$"))
                        {
                            AntdUI.Message.open(new AntdUI.Message.Config(this, "指定端口错误", TType.Error)
                            {
                                LocalizationText = "FilterEditForm.AppointPort.Error"
                            });

                            return false;
                        }
                    }
                }

                //指定长度
                if (this.cbFilter_AppointLength.Checked)
                {
                    if (string.IsNullOrEmpty(this.txtFilter_LengthContent.Text.Trim()))
                    {
                        AntdUI.Message.open(new AntdUI.Message.Config(this, "指定长度错误", TType.Error)
                        {
                            LocalizationText = "FilterEditForm.AppointLength.Error"
                        });

                        return false;
                    }
                    else
                    {
                        if (!Regex.IsMatch(this.txtFilter_LengthContent.Text.Trim(), @"^(\d+[-;])*\d+$"))
                        {
                            AntdUI.Message.open(new AntdUI.Message.Config(this, "指定长度错误", TType.Error)
                            {
                                LocalizationText = "FilterEditForm.AppointLength.Error"
                            });

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
                        for (int i = 0; i < this.dtFilterNormal.Columns.Count; i++)
                        {
                            if (dtFilterNormal.Rows[1][i] != null)
                            {
                                string sCheckValue = ((CellText)dtFilterNormal.Rows[1][i]).Text.Trim();
                                if (!string.IsNullOrEmpty(sCheckValue))
                                {
                                    iMaxIndex = i;
                                }
                            }
                        }

                        if (iMaxIndex == 0)
                        {
                            AntdUI.Message.open(new AntdUI.Message.Config(this, "换包数据错误", TType.Error)
                            {
                                LocalizationText = "FilterEditForm.Change.Error"
                            });

                            return false;
                        }

                        for (int i = 0; i < iMaxIndex; i++)
                        {
                            if (dtFilterNormal.Rows[1][i] != null)
                            {
                                string sCheckValue = ((CellText)dtFilterNormal.Rows[1][i]).Text.Trim();
                                if (string.IsNullOrEmpty(sCheckValue))
                                {
                                    AntdUI.Message.open(new AntdUI.Message.Config(this, "换包数据错误", TType.Error)
                                    {
                                        LocalizationText = "FilterEditForm.Change.Error"
                                    });

                                    return false;
                                }                                
                            }
                        }
                    }

                    //高级滤镜（从头开始）
                    if (this.rbFilterMode_Advanced.Checked && this.rbFilterModifyFrom_Head.Checked)
                    {
                        for (int i = 0; i < this.dtFilterAdvanced_Modify_Head.Columns.Count; i++)
                        {
                            if (dtFilterAdvanced_Modify_Head.Rows[0][i] != null)
                            {
                                string sCheckValue = ((CellText)dtFilterAdvanced_Modify_Head.Rows[0][i]).Text.Trim();
                                if (!string.IsNullOrEmpty(sCheckValue))
                                {
                                    iMaxIndex = i;
                                }
                            }
                        }

                        if (iMaxIndex == 0)
                        {
                            AntdUI.Message.open(new AntdUI.Message.Config(this, "换包数据错误", TType.Error)
                            {
                                LocalizationText = "FilterEditForm.Change.Error"
                            });

                            return false;
                        }

                        for (int i = 0; i < iMaxIndex; i++)
                        {
                            if (dtFilterAdvanced_Modify_Head.Rows[0][i] != null)
                            {
                                string sCheckValue = ((CellText)dtFilterAdvanced_Modify_Head.Rows[0][i]).Text.Trim();
                                if (string.IsNullOrEmpty(sCheckValue))
                                {
                                    AntdUI.Message.open(new AntdUI.Message.Config(this, "换包数据错误", TType.Error)
                                    {
                                        LocalizationText = "FilterEditForm.Change.Error"
                                    });

                                    return false;
                                }
                            }
                        }
                    }

                    //高级滤镜（从发现有连锁的位置）
                    if (this.rbFilterMode_Advanced.Checked && this.rbFilterModifyFrom_Position.Checked)
                    {
                        int iStartIndex = Operate.FilterConfig.Filter.FilterSize_MaxLen;

                        for (int i = iStartIndex; i < this.dtFilterAdvanced_Modify_Position.Columns.Count; i++)
                        {
                            if (dtFilterAdvanced_Modify_Position.Rows[0][i] != null)
                            {
                                string sCheckValue = ((CellText)dtFilterAdvanced_Modify_Position.Rows[0][i]).Text.Trim();
                                if (!string.IsNullOrEmpty(sCheckValue))
                                {
                                    iMaxIndex = i;
                                }
                            }
                        }

                        if (iMaxIndex == iStartIndex)
                        {
                            AntdUI.Message.open(new AntdUI.Message.Config(this, "换包数据错误", TType.Error)
                            {
                                LocalizationText = "FilterEditForm.Change.Error"
                            });

                            return false;
                        }

                        for (int i = iStartIndex; i < iMaxIndex; i++)
                        {
                            if (dtFilterAdvanced_Modify_Position.Rows[0][i] != null)
                            {
                                string sCheckValue = ((CellText)dtFilterAdvanced_Modify_Position.Rows[0][i]).Text.Trim();
                                if (string.IsNullOrEmpty(sCheckValue))
                                {
                                    AntdUI.Message.open(new AntdUI.Message.Config(this, "换包数据错误", TType.Error)
                                    {
                                        LocalizationText = "FilterEditForm.Change.Error"
                                    });

                                    return false;
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

            return bReturn;
        }

        private void txtFilterName_TextChanged(object sender, EventArgs e)
        {
            this.FilterName_Changed();
        }

        private void FilterName_Changed()
        {
            if (string.IsNullOrEmpty(this.txtFilterName.Text.Trim()))
            {
                this.txtFilterName.Status = TType.Error;
            }
            else
            {
                this.txtFilterName.Status = TType.Success;
            }
        }

        private void txtFilter_HeaderContent_TextChanged(object sender, EventArgs e)
        {
            this.Filter_HeaderContent_Changed();
        }

        private void Filter_HeaderContent_Changed()
        {
            if (this.cbFilter_AppointHeader.Checked)
            {
                if (string.IsNullOrEmpty(this.txtFilter_HeaderContent.Text.Trim()))
                {
                    this.txtFilter_HeaderContent.Status = TType.Error;
                }
                else
                {
                    this.txtFilter_HeaderContent.Status = TType.Success;
                }
            }
        }

        private void txtFilter_SocketContent_TextChanged(object sender, EventArgs e)
        {
            this.Filter_SocketContent_Changed();
        }

        private void Filter_SocketContent_Changed()
        {
            if (this.cbFilter_AppointSocket.Checked)
            {
                if (string.IsNullOrEmpty(this.txtFilter_SocketContent.Text.Trim()))
                {
                    this.txtFilter_SocketContent.Status = TType.Error;
                }
                else
                {
                    this.txtFilter_SocketContent.Status = TType.Success;
                }
            }
        }

        private void txtFilter_PortContent_TextChanged(object sender, EventArgs e)
        {
            this.Filter_PortContent_Changed();
        }

        private void Filter_PortContent_Changed()
        {
            if (this.cbFilter_AppointPort.Checked)
            {
                if (string.IsNullOrEmpty(this.txtFilter_PortContent.Text.Trim()))
                {
                    this.txtFilter_PortContent.Status = TType.Error;
                }
                else
                {
                    this.txtFilter_PortContent.Status = TType.Success;
                }
            }
        }

        private void txtFilter_LengthContent_TextChanged(object sender, EventArgs e)
        {
            this.Filter_LengthContent_Changed();
        }

        private void Filter_LengthContent_Changed()
        {
            if (this.cbFilter_AppointLength.Checked)
            {
                if (string.IsNullOrEmpty(this.txtFilter_LengthContent.Text.Trim()))
                {
                    this.txtFilter_LengthContent.Status = TType.Error;
                }
                else
                {
                    this.txtFilter_LengthContent.Status = TType.Success;
                }
            }
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

                        ((CellText)dtFilterEdit.Rows[RowIndex - 1][ColumnIndex]).Back = Color.DarkRed;
                        tFilterEdit.Refresh();

                        break;

                    case "cmsFilterEdit_Progression_Disable":

                        ((CellText)dtFilterEdit.Rows[RowIndex - 1][ColumnIndex]).Back = Color.Yellow;
                        tFilterEdit.Refresh();

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
                Point pMouse = tFilterNormal.PointToClient(MousePosition);
                tFilterNormal.HitTest(pMouse.X, pMouse.Y, out int RIndex, out int CIndex);

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
                Point pMouse = tFilterAdvanced_Search.PointToClient(MousePosition);
                tFilterAdvanced_Search.HitTest(pMouse.X, pMouse.Y, out int RIndex, out int CIndex);

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
                Point pMouse = tFilterAdvanced_Modify_Head.PointToClient(MousePosition);
                tFilterAdvanced_Modify_Head.HitTest(pMouse.X, pMouse.Y, out int RIndex, out int CIndex);

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
                Point pMouse = tFilterAdvanced_Modify_Position.PointToClient(MousePosition);
                tFilterAdvanced_Modify_Position.HitTest(pMouse.X, pMouse.Y, out int RIndex, out int CIndex);

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

        #region//显示滤镜数据

        private void ShowFilterData()
        {
            try
            {
                AntdUI.Spin.open(this, new AntdUI.Spin.Config()
                {
                    Radius = 6,
                    Font = new Font("Microsoft YaHei UI", 12f),
                }, (config) =>
                {
                    config.Text = AntdUI.Localization.Get("Loading", "正在加载...");

                    if (!string.IsNullOrEmpty(fiSelect.FSearch))
                    {
                        string[] sSearchAll = fiSelect.FSearch.Split(',');

                        foreach (string s in sSearchAll)
                        {
                            if (int.TryParse(s.Split('|')[0], out int iIndex))
                            {
                                string sValue = s.Split('|')[1];

                                if (iIndex < this.dtFilterNormal.Columns.Count)
                                {
                                    ((CellText)this.dtFilterNormal.Rows[0][iIndex]).Text = sValue;
                                }

                                if (iIndex < this.dtFilterAdvanced_Search.Columns.Count)
                                {
                                    ((CellText)this.dtFilterAdvanced_Search.Rows[0][iIndex]).Text = sValue;
                                }
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(fiSelect.FModify))
                    {
                        string[] sModifyAll = fiSelect.FModify.Split(',');
                        foreach (string s in sModifyAll)
                        {
                            if (int.TryParse(s.Split('|')[0], out int iIndex))
                            {
                                string sValue = s.Split('|')[1];

                                switch (fiSelect.FMode)
                                {
                                    case Operate.FilterConfig.Filter.FilterMode.Normal:

                                        if (iIndex < this.dtFilterNormal.Columns.Count)
                                        {
                                            ((CellText)this.dtFilterNormal.Rows[1][iIndex]).Text = sValue;
                                        }

                                        break;

                                    case Operate.FilterConfig.Filter.FilterMode.Advanced:

                                        switch (fiSelect.FStartFrom)
                                        {
                                            case Operate.FilterConfig.Filter.FilterStartFrom.Head:

                                                if (iIndex < this.dtFilterAdvanced_Modify_Head.Columns.Count)
                                                {
                                                    ((CellText)this.dtFilterAdvanced_Modify_Head.Rows[0][iIndex]).Text = sValue;
                                                }

                                                break;

                                            case Operate.FilterConfig.Filter.FilterStartFrom.Position:

                                                iIndex += Operate.FilterConfig.Filter.FilterSize_MaxLen;

                                                if (iIndex < this.dtFilterAdvanced_Modify_Position.Columns.Count)
                                                {
                                                    ((CellText)this.dtFilterAdvanced_Modify_Position.Rows[0][iIndex]).Text = sValue;
                                                }

                                                break;
                                        }

                                        break;
                                }
                            }
                        }
                    }

                }, () =>
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this, "滤镜数据加载完毕", TType.Success)
                    {
                        LocalizationText = "FilterEditForm.Load.Success"
                    });
                });                
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            if (!this.CheckFilterIsValid())
            {
                return;
            }

            try
            {
                string sFName_New = this.txtFilterName.Text.Trim();
                string sHeaderContent_New = string.Empty;
                string sLengthContent_New = string.Empty;
                string sSocketContent_New = string.Empty;
                string sPortContent_New = string.Empty;
                int iProgressionStep_New = 1;
                int iProgressionCarryNumber_New = 1;
                int iProgressionCount_New = 0;
                bool bIsExecute_New, bIsProgressionContinuous_New, bIsProgressionCarry_New;
                bool bAppointHeader_New, bAppointSocket_New, bAppointLength_New, bAppointPort_New;
                StringBuilder sbProgression = new StringBuilder();
                StringBuilder sbSearch = new StringBuilder();
                StringBuilder sbModify = new StringBuilder();

                Operate.FilterConfig.Filter.FilterMode FilterMode_New;
                Operate.FilterConfig.Filter.FilterAction FilterAction_New;
                Operate.FilterConfig.Filter.FilterExecuteType FilterExecuteType_New;
                Guid SID_New = Guid.Empty;
                Guid RID_New = Guid.Empty;
                Operate.FilterConfig.Filter.FilterFunction FilterFunction_New;
                Operate.FilterConfig.Filter.FilterStartFrom FilterStartFrom_New;

                bIsExecute_New = this.cbFilterAction_Execute.Checked;
                bAppointHeader_New = this.cbFilter_AppointHeader.Checked;
                bAppointSocket_New = this.cbFilter_AppointSocket.Checked;
                bAppointLength_New = this.cbFilter_AppointLength.Checked;
                bAppointPort_New = this.cbFilter_AppointPort.Checked;
                bIsProgressionContinuous_New = this.cbProgressionContinuous.Checked;
                bIsProgressionCarry_New = this.cbProgressionCarry.Checked;

                sHeaderContent_New = this.txtFilter_HeaderContent.Text.Trim();
                sLengthContent_New = this.txtFilter_LengthContent.Text.Trim();
                sSocketContent_New = this.txtFilter_SocketContent.Text.Trim();
                sPortContent_New = this.txtFilter_PortContent.Text.Trim();
                iProgressionStep_New = ((int)this.nudProgressionStep.Value);
                iProgressionCarryNumber_New = ((int)this.nudProgressionCarry.Value);

                if (rbFilterMode_Normal.Checked)
                {
                    FilterMode_New = Operate.FilterConfig.Filter.FilterMode.Normal;
                }
                else if (rbFilterMode_Advanced.Checked)
                {
                    FilterMode_New = Operate.FilterConfig.Filter.FilterMode.Advanced;
                }
                else
                {
                    FilterMode_New = Operate.FilterConfig.Filter.FilterMode.Normal;
                }

                if (rbFilterAction_Replace.Checked)
                {
                    FilterAction_New = Operate.FilterConfig.Filter.FilterAction.Replace;
                }
                else if (rbFilterAction_Intercept.Checked)
                {
                    FilterAction_New = Operate.FilterConfig.Filter.FilterAction.Intercept;
                }
                else if (rbFilterAction_Change.Checked)
                {
                    FilterAction_New = Operate.FilterConfig.Filter.FilterAction.Change;
                }
                else if (rbFilterAction_NoModify_Display.Checked)
                {
                    FilterAction_New = Operate.FilterConfig.Filter.FilterAction.NoModify_Display;
                }
                else if (rbFilterAction_NoModify_NoDisplay.Checked)
                {
                    FilterAction_New = Operate.FilterConfig.Filter.FilterAction.NoModify_NoDisplay;
                }
                else
                {
                    FilterAction_New = Operate.FilterConfig.Filter.FilterAction.NoModify_Display;
                }

                if (cbFilterAction_Execute.Checked)
                {
                    if (this.cbbFilterAction_ExecuteType.SelectedIndex == 0)
                    {
                        FilterExecuteType_New = Operate.FilterConfig.Filter.FilterExecuteType.Send;

                        if (cbbFilterAction_Execute.SelectedValue != null)
                        {
                            SID_New = ((SendInfo)cbbFilterAction_Execute.SelectedValue).SID;
                        }
                    }
                    else if (this.cbbFilterAction_ExecuteType.SelectedIndex == 1)
                    {
                        FilterExecuteType_New = Operate.FilterConfig.Filter.FilterExecuteType.Robot;

                        if (cbbFilterAction_Execute.SelectedValue != null)
                        {
                            RID_New = ((Socket_RobotInfo)cbbFilterAction_Execute.SelectedValue).RID;
                        }
                    }
                    else
                    {
                        FilterExecuteType_New = Operate.FilterConfig.Filter.FilterExecuteType.None;
                    }
                }
                else
                {
                    FilterExecuteType_New = Operate.FilterConfig.Filter.FilterExecuteType.None;
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
                    FilterStartFrom_New = Operate.FilterConfig.Filter.FilterStartFrom.Head;
                }
                else
                {
                    FilterStartFrom_New = Operate.FilterConfig.Filter.FilterStartFrom.Position;
                }

                switch (FilterMode_New)
                {
                    case Operate.FilterConfig.Filter.FilterMode.Normal:

                        for (int i = 0; i < this.dtFilterNormal.Columns.Count; i++)
                        {
                            CellText cell = (CellText)dtFilterNormal.Rows[1][i];
                            if (cell.Back == Color.DarkRed)
                            {
                                sbProgression.Append(i).Append(",");
                            }

                            if (dtFilterNormal.Rows[0][i] != null)
                            {
                                string sSearchValue = ((CellText)dtFilterNormal.Rows[0][i]).Text.Trim();
                                if (!String.IsNullOrEmpty(sSearchValue))
                                {
                                    sbSearch.Append(i).Append("|").Append(sSearchValue).Append(",");
                                }
                            }

                            if (dtFilterNormal.Rows[1][i] != null)
                            {
                                string sModifyValue = ((CellText)dtFilterNormal.Rows[1][i]).Text.Trim();
                                if (!String.IsNullOrEmpty(sModifyValue))
                                {
                                    sbModify.Append(i).Append("|").Append(sModifyValue).Append(",");
                                }
                            }
                        }

                        break;

                    case Operate.FilterConfig.Filter.FilterMode.Advanced:

                        for (int i = 0; i < this.dtFilterAdvanced_Search.Columns.Count; i++)
                        {
                            if (dtFilterAdvanced_Search.Rows[0][i] != null)
                            {
                                string sValue = ((CellText)dtFilterAdvanced_Search.Rows[0][i]).Text.Trim();
                                if (!String.IsNullOrEmpty(sValue))
                                {
                                    sbSearch.Append(i).Append("|").Append(sValue).Append(",");
                                }
                            }
                        }

                        switch (FilterStartFrom_New)
                        {
                            case Operate.FilterConfig.Filter.FilterStartFrom.Head:

                                for (int i = 0; i < this.dtFilterAdvanced_Modify_Head.Columns.Count; i++)
                                {
                                    CellText cell = (CellText)dtFilterAdvanced_Modify_Head.Rows[0][i];
                                    if (cell.Back == Color.DarkRed)
                                    {
                                        sbProgression.Append(i).Append(",");
                                    }

                                    if (dtFilterAdvanced_Modify_Head.Rows[0][i] != null)
                                    {
                                        string sValue = ((CellText)dtFilterAdvanced_Modify_Head.Rows[0][i]).Text.Trim();
                                        if (!String.IsNullOrEmpty(sValue))
                                        {
                                            sbModify.Append(i).Append("|").Append(sValue).Append(",");
                                        }
                                    }
                                }

                                break;

                            case Operate.FilterConfig.Filter.FilterStartFrom.Position:

                                for (int i = 0; i < this.dtFilterAdvanced_Modify_Position.Columns.Count; i++)
                                {
                                    if (int.TryParse(dtFilterAdvanced_Modify_Position.Columns[i].ColumnName, out int iIndex))
                                    {
                                        CellText cell = (CellText)dtFilterAdvanced_Modify_Position.Rows[0][i];
                                        if (cell.Back == Color.DarkRed)
                                        {
                                            sbProgression.Append(iIndex).Append(",");
                                        }

                                        if (dtFilterAdvanced_Modify_Position.Rows[0][i] != null)
                                        {
                                            string sValue = ((CellText)dtFilterAdvanced_Modify_Position.Rows[0][i]).Text.Trim();
                                            if (!String.IsNullOrEmpty(sValue))
                                            {
                                                sbModify.Append(iIndex).Append("|").Append(sValue).Append(",");
                                            }
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

                Operate.FilterConfig.Filter.UpdateFilter(
                    fiSelect,
                    sFName_New,
                    bAppointHeader_New,
                    sHeaderContent_New,
                    bAppointSocket_New,
                    sSocketContent_New,
                    bAppointLength_New,
                    sLengthContent_New,
                    bAppointPort_New,
                    sPortContent_New,
                    FilterMode_New,
                    FilterAction_New,
                    bIsExecute_New,
                    FilterExecuteType_New,
                    SID_New,
                    RID_New,
                    FilterFunction_New,
                    FilterStartFrom_New,
                    bIsProgressionContinuous_New,
                    iProgressionStep_New,
                    bIsProgressionCarry_New,
                    iProgressionCarryNumber_New,
                    sProgression_New,
                    iProgressionCount_New,
                    sSearch_New,
                    sModify_New);

                this.Close();
                this.imForm.RefreshFilterList();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);

                AntdUI.Message.open(new AntdUI.Message.Config(this, "滤镜保存出错", TType.Error)
                {
                    LocalizationText = "FilterEditForm.Save.Error"
                });
            }
        }

        #endregion

        #region//退出

        private void bExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        #endregion        
    }
}
