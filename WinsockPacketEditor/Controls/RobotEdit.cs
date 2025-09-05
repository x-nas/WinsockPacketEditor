using AntdUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class RobotEdit : UserControl
    {
        private Form form;
        private RobotInfo riSelect;
        private BindingList<InstructionInfo> RInstruction;
        private readonly RobotExecute re = new RobotExecute();

        #region//窗体事件

        public RobotEdit(Form form, RobotInfo ri)
        {
            InitializeComponent();
            this.riSelect = ri;
            this.form = form;
        }

        private void RobotEdit_Load(object sender, EventArgs e)
        {
            this.ciPacketINST.Expand = false;

            this.txtKeyCombination.BackColor = null;
            this.txtKeyCombination.ForeColor = null;

            this.txtRobotName.Text = riSelect.RName;
            this.RInstruction = new BindingList<InstructionInfo>(this.riSelect.RInstruction.ToList());

            this.re.Worker.ProgressChanged -= this.Worker_ProgressChanged;
            this.re.Worker.ProgressChanged += this.Worker_ProgressChanged;
            this.re.Worker.RunWorkerCompleted -= this.Worker_RunWorkerCompleted;
            this.re.Worker.RunWorkerCompleted += this.Worker_RunWorkerCompleted;

            this.InitTable_RobotINST();
            this.InitDDL();
            this.DelayFix_Changed();
            this.DelayRandom_Changed();
            this.Dark_Changed();
        }

        private void InitTable_RobotINST()
        {
            tRobotInstruction.Columns = new AntdUI.ColumnCollection {
                new AntdUI.Column("", "Inst")
                {
                    Render = (value, record, rowindex)=>
                    {
                        string sINST = string.Format(AntdUI.Localization.Get("RobotEditForm.INST", "指令 {0}"), rowindex + 1);
                        return new AntdUI.CellText(sINST)
                        {
                            Fore = Color.FromArgb(22, 119, 255),
                        };
                    },
                }.SetFixed().SetLocalizationTitleID("Table.RobotINST.Column."),
                new AntdUI.Column("InstType", "Type")
                {
                    Render = (value, record, rowindex)=>
                    {
                        var typeName = Operate.RobotConfig.Robot.GetName_ByInstructionType((Operate.RobotConfig.Robot.InstructionType)value);
                        return new AntdUI.CellText(typeName)
                        {
                            Fore = Operate.RobotConfig.Robot.GetColor_ByInstructionType((Operate.RobotConfig.Robot.InstructionType)value),
                        };
                    },
                }.SetFixed().SetLocalizationTitleID("Table.RobotINST.Column."),
                new AntdUI.Column("InstContent", "Content")
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is InstructionInfo ii)
                        {
                            return Operate.RobotConfig.Robot.GetContentString_ByInstructionType(ii.InstType, value.ToString());
                        }

                        return null;
                    },
                }.SetLocalizationTitleID("Table.RobotINST.Column."),
            };

            this.tRobotInstruction.ColumnFont = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.tRobotInstruction.Binding(this.RInstruction);
        }

        private void InitDDL()
        {
            try
            {
                if (Operate.SendConfig.List.lstSendInfo.Count > 0)
                {
                    var selectItems = Operate.SendConfig.List.lstSendInfo.Select(info => new SelectItem(info.SName, info)).ToArray();

                    this.ddlSendList.Items.Clear();
                    this.ddlSendList.Items.AddRange(selectItems);
                    this.ddlSendList.SelectedIndex = 0;
                }

                this.ddlKeyType.Items.Clear();
                this.ddlKeyType.Items.AddRange(new AntdUI.SelectItem[]
                {
                    new AntdUI.SelectItem("按键")
                    {
                        LocalizationText = "RobotEditForm.KeyBoardINST.KeyPress",
                    },
                    new AntdUI.SelectItem("按下")
                    {
                        LocalizationText = "RobotEditForm.KeyBoardINST.KeyDown",
                    },
                    new AntdUI.SelectItem("弹起")
                    {
                        LocalizationText = "RobotEditForm.KeyBoardINST.KeyUp",
                    },
                });
                this.ddlKeyType.SelectedIndex = 0;

                this.ddlMouseKey.Items.Clear();
                this.ddlMouseKey.Items.AddRange(new AntdUI.SelectItem[]
                {
                    new AntdUI.SelectItem("左键单击")
                    {
                        LocalizationText = "RobotEditForm.INST.LeftClick",
                    },
                    new AntdUI.SelectItem("右键单击")
                    {
                        LocalizationText = "RobotEditForm.INST.RightClick",
                    },
                    new AntdUI.SelectItem("左键双击")
                    {
                        LocalizationText = "RobotEditForm.INST.LeftDBClick",
                    },
                    new AntdUI.SelectItem("右键双击")
                    {
                        LocalizationText = "RobotEditForm.INST.RightDBClick",
                    },
                    new AntdUI.SelectItem("左键按下")
                    {
                        LocalizationText = "RobotEditForm.INST.LeftDown",
                    },
                    new AntdUI.SelectItem("左键弹起")
                    {
                        LocalizationText = "RobotEditForm.INST.LeftUp",
                    },
                    new AntdUI.SelectItem("右键按下")
                    {
                        LocalizationText = "RobotEditForm.INST.RightDown",
                    },
                    new AntdUI.SelectItem("右键弹起")
                    {
                        LocalizationText = "RobotEditForm.INST.RightUp",
                    },
                });
                this.ddlMouseKey.SelectedIndex = 0;

                this.ddlMouseWheel.Items.Clear();
                this.ddlMouseWheel.Items.AddRange(new AntdUI.SelectItem[]
                {
                    new AntdUI.SelectItem("向上")
                    {
                        LocalizationText = "Up",
                    },
                    new AntdUI.SelectItem("向下")
                    {
                        LocalizationText = "Down",
                    },
                });
                this.ddlMouseWheel.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void txtRobotName_TextChanged(object sender, System.EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtRobotName.Text.Trim()))
            {
                this.txtRobotName.Status = AntdUI.TType.Error;
            }
            else
            {
                this.txtRobotName.Status = AntdUI.TType.Success;
            }
        }

        private void Dark_Changed()
        {
            if (AntdUI.Config.IsDark)
            {
                this.pSendList.Back =
                    this.pPacketList.Back =
                    this.pSYSSocket.Back =
                    this.pDelay.Back =
                    this.pLoop.Back =
                    this.pKeyBoard.Back =
                    this.pKeyCombination.Back = 
                    this.pText.Back =
                    this.pMouseKey.Back =
                    this.pMouseWheel.Back =
                    this.pMouseMove.Back =
                    Operate.SystemConfig.Color_35;
            }
            else
            {
                this.pSendList.Back =
                    this.pPacketList.Back =
                    this.pSYSSocket.Back =
                    this.pDelay.Back =
                    this.pLoop.Back =
                    this.pKeyBoard.Back =
                    this.pKeyCombination.Back =
                    this.pText.Back =
                    this.pMouseKey.Back =
                    this.pMouseWheel.Back =
                    this.pMouseMove.Back = null;
            }
        }

        #endregion

        #region//添加指令

        public void AddInstruction(Operate.RobotConfig.Robot.InstructionType instructionType, string sContent)
        {
            try
            {
                InstructionInfo ii = new InstructionInfo(instructionType, sContent);

                if (this.tRobotInstruction.SelectedIndex != -1)
                {
                    this.RInstruction.Insert(this.tRobotInstruction.SelectedIndex, ii);
                }
                else
                {
                    this.RInstruction.Add(ii);
                    this.tRobotInstruction.ScrollBar.ValueY = this.tRobotInstruction.ScrollBar.MaxY;
                }

                this.tRobotInstruction.Refresh();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//封包指令 - 发送 - 发送列表

        private void bInsert_SendList_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.ddlSendList.SelectedValue != null)
                {
                    string sContent = ((SendInfo)ddlSendList.SelectedValue).SID.ToString().ToUpper();
                    this.AddInstruction(Operate.RobotConfig.Robot.InstructionType.SendSendList, sContent);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }


        #endregion

        #region//封包指令 - 发送 - 封包列表

        private void bInsert_PacketList_Click(object sender, EventArgs e)
        {
            this.AddInstruction(Operate.RobotConfig.Robot.InstructionType.SendPacketList, string.Empty);
        }


        #endregion

        #region//封包指令 - 设置 - 系统套接字

        private void bInsert_SYSSocket_Click(object sender, EventArgs e)
        {
            try
            {
                string sContent = string.Empty;
                if (this.rbSelectPacket.Checked)
                {
                    sContent = "PacketConfig.List";
                }
                else if (this.rbSelectFilter.Checked)
                {
                    sContent = "FilterSocket";
                }
                else if (this.rbSelectSocket.Checked)
                {
                    string sSocket = this.nudSelectSocket.Value.ToString();
                    sContent = "Customize" + "|" + sSocket;
                }

                this.AddInstruction(Operate.RobotConfig.Robot.InstructionType.SetSystemSocket, sContent);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }


        #endregion

        #region//封包指令 - 延迟

        private void rbDelayFix_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.DelayFix_Changed();
        }

        private void DelayFix_Changed()
        {
            this.nudDelayFix.Enabled = this.rbDelayFix.Checked;
        }

        private void rbDelayRandom_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.DelayRandom_Changed();
        }

        private void DelayRandom_Changed()
        {
            this.nudnudDelayRandom_From.Enabled =
                this.nudnudDelayRandom_To.Enabled =
                this.rbDelayRandom.Checked;
        }

        private void bInsert_Delay_Click(object sender, EventArgs e)
        {
            try
            {
                string sContent = string.Empty;
                if (this.rbDelayFix.Checked)
                {
                    int Delay = ((int)this.nudDelayFix.Value);
                    sContent = Delay.ToString();
                }
                else
                {
                    int DelayFrom = ((int)this.nudnudDelayRandom_From.Value);
                    int DelayTo = ((int)this.nudnudDelayRandom_To.Value);
                    sContent = DelayFrom.ToString() + "-" + DelayTo.ToString();
                }

                this.AddInstruction(Operate.RobotConfig.Robot.InstructionType.Delay, sContent);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//封包指令 - 循环

        private void bInsert_LoopStart_Click(object sender, EventArgs e)
        {
            string sContent = this.nudLoop.Text.Trim();
            this.AddInstruction(Operate.RobotConfig.Robot.InstructionType.LoopStart, sContent);
        }

        private void bInsert_LoopEnd_Click(object sender, EventArgs e)
        {
            this.AddInstruction(Operate.RobotConfig.Robot.InstructionType.LoopEnd, string.Empty);
        }

        #endregion

        #region//键盘指令 - 按键

        private void bInsert_KeyBoard_Click(object sender, EventArgs e)
        {
            try
            {
                string KeyCode = this.txtKey.Text.Trim();
                if (!string.IsNullOrEmpty(KeyCode))
                {
                    Operate.RobotConfig.Robot.KeyBoardType kbType = new Operate.RobotConfig.Robot.KeyBoardType();

                    switch (this.ddlKeyType.SelectedIndex)
                    {
                        case 0:
                            kbType = Operate.RobotConfig.Robot.KeyBoardType.Press;
                            break;

                        case 1:
                            kbType = Operate.RobotConfig.Robot.KeyBoardType.Down;
                            break;

                        case 2:
                            kbType = Operate.RobotConfig.Robot.KeyBoardType.Up;
                            break;
                    }

                    string sContent = kbType + "|" + KeyCode;
                    this.AddInstruction(Operate.RobotConfig.Robot.InstructionType.KeyBoard, sContent);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void txtKey_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                e.SuppressKeyPress = true;
                e.Handled = true;
                this.txtKey.Text = e.KeyCode.ToString();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//键盘指令 - 按键组合

        private void bInsert_KeyCombination_Click(object sender, EventArgs e)
        {
            try
            {
                string KeyCode = this.txtKeyCombination.Text.Trim();

                if (!string.IsNullOrEmpty(KeyCode))
                {
                    Operate.RobotConfig.Robot.KeyBoardType kbType = Operate.RobotConfig.Robot.KeyBoardType.Combine;
                    string sContent = kbType + "|" + KeyCode;

                    this.AddInstruction(Operate.RobotConfig.Robot.InstructionType.KeyBoard, sContent);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//键盘指令 - 文本

        private void bInsert_Text_Click(object sender, EventArgs e)
        {
            try
            {
                string KeyCode = this.txtText.Text.Trim();
                if (!string.IsNullOrEmpty(KeyCode))
                {
                    Operate.RobotConfig.Robot.KeyBoardType kbType = Operate.RobotConfig.Robot.KeyBoardType.Text;
                    string sContent = kbType + "|" + KeyCode;

                    this.AddInstruction(Operate.RobotConfig.Robot.InstructionType.KeyBoard, sContent);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//鼠标指令 - 按键

        private void bInsert_MouseKey_Click(object sender, EventArgs e)
        {
            try
            {
                Operate.RobotConfig.Robot.MouseType mType = new Operate.RobotConfig.Robot.MouseType();

                switch (this.ddlMouseKey.SelectedIndex)
                {
                    case 0:
                        mType = Operate.RobotConfig.Robot.MouseType.LeftClick;
                        break;

                    case 1:
                        mType = Operate.RobotConfig.Robot.MouseType.RightClick;
                        break;

                    case 2:
                        mType = Operate.RobotConfig.Robot.MouseType.LeftDBClick;
                        break;

                    case 3:
                        mType = Operate.RobotConfig.Robot.MouseType.RightDBClick;
                        break;

                    case 4:
                        mType = Operate.RobotConfig.Robot.MouseType.LeftDown;
                        break;

                    case 5:
                        mType = Operate.RobotConfig.Robot.MouseType.LeftUp;
                        break;

                    case 6:
                        mType = Operate.RobotConfig.Robot.MouseType.RightDown;
                        break;

                    case 7:
                        mType = Operate.RobotConfig.Robot.MouseType.RightUp;
                        break;
                }

                string sContent = mType + "|" + string.Empty;
                this.AddInstruction(Operate.RobotConfig.Robot.InstructionType.Mouse, sContent);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//鼠标指令 - 滚轮

        private void bInsert_MouseWheel_Click(object sender, EventArgs e)
        {
            try
            {
                Operate.RobotConfig.Robot.MouseType mType = new Operate.RobotConfig.Robot.MouseType();
                switch (this.ddlMouseWheel.SelectedIndex)
                {
                    case 0:
                        mType = Operate.RobotConfig.Robot.MouseType.WheelUp;
                        break;

                    case 1:
                        mType = Operate.RobotConfig.Robot.MouseType.WheelDown;
                        break;
                }

                string sContent = mType + "|" + this.nudWheelDistance.Text.Trim();
                this.AddInstruction(Operate.RobotConfig.Robot.InstructionType.Mouse, sContent);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//鼠标指令 - 移动

        private void bInsert_MouseMove_Click(object sender, EventArgs e)
        {
            try
            {
                Operate.RobotConfig.Robot.MouseType mType = new Operate.RobotConfig.Robot.MouseType();
                if (this.rbMoveTo.Checked)
                {
                    mType = Operate.RobotConfig.Robot.MouseType.MoveTo;
                }
                else
                {
                    mType = Operate.RobotConfig.Robot.MouseType.MoveBy;
                }

                string sContent = mType + "|" + this.nudMouseMove_X.Text.Trim() + ", " + this.nudMouseMove_Y.Text.Trim();
                this.AddInstruction(Operate.RobotConfig.Robot.InstructionType.Mouse, sContent);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//指令集 - 右键菜单

        private void tRobotINST_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (this.RInstruction.Count == 0)
                {
                    return;
                }

                AntdUI.ContextMenuStrip.open(new AntdUI.ContextMenuStrip.Config(tRobotInstruction, (item) =>
                {
                    List<InstructionInfo> iiList = new List<InstructionInfo>();
                    if (this.tRobotInstruction.SelectedIndex != -1)
                    {
                        iiList.Add(this.RInstruction[this.tRobotInstruction.SelectedIndex - 1]);
                    }

                    switch (item.ID)
                    {
                        case "Top":

                            if (iiList.Count > 0)
                            {
                                Operate.RobotConfig.Robot.UpdateInstruction_ByListAction(this.form, Operate.SystemConfig.ListAction.Top, this.RInstruction, iiList);
                            }

                            break;

                        case "Up":

                            if (iiList.Count > 0)
                            {
                                Operate.RobotConfig.Robot.UpdateInstruction_ByListAction(this.form, Operate.SystemConfig.ListAction.Up, this.RInstruction, iiList);
                            }

                            break;

                        case "Down":

                            if (iiList.Count > 0)
                            {
                                Operate.RobotConfig.Robot.UpdateInstruction_ByListAction(this.form, Operate.SystemConfig.ListAction.Down, this.RInstruction, iiList);
                            }

                            break;

                        case "Bottom":

                            if (iiList.Count > 0)
                            {
                                Operate.RobotConfig.Robot.UpdateInstruction_ByListAction(this.form, Operate.SystemConfig.ListAction.Bottom, this.RInstruction, iiList);
                            }

                            break;

                        case "Delete":

                            if (iiList.Count > 0)
                            {
                                Operate.RobotConfig.Robot.UpdateInstruction_ByListAction(this.form, Operate.SystemConfig.ListAction.Delete, this.RInstruction, iiList);
                            }

                            break;

                        case "ClearUp":

                            Operate.RobotConfig.Robot.UpdateInstruction_ByListAction(this.form, Operate.SystemConfig.ListAction.CleanUp, this.RInstruction, iiList);

                            break;
                    }

                    this.tRobotInstruction.SelectedIndex = -1;
                }, Operate.RobotConfig.Robot.GetCMS_RobotInstruction()));
            }
        }

        #endregion

        #region//执行

        private void bExecute_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.RInstruction.Count == 0)
                {
                    return;
                }

                if (!this.SaveRobot())
                {
                    return;
                }

                if (!this.re.Worker.IsBusy)
                {
                    this.txtINSTLog.Clear();

                    this.bExecute.Loading = true;
                    this.bStop.Enabled = true;
                    this.cRobotINST.Enabled = false;
                    this.tRobotInstruction.Enabled = false;

                    re.StartRobot(this.riSelect, null);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//执行机器人（异步）

        private void Worker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "机器人已停止", TType.Warn)
                    {
                        LocalizationText = "RobotEditForm.Robot.Stop",
                    });
                }
                else if (e.Error != null)
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "发生错误: " + e.Error.Message, TType.Error)
                    {
                        LocalizationText = "RobotEditForm.Robot.Error" + e.Error.Message,
                    });
                }
                else
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "机器人执行完毕", TType.Success)
                    {
                        LocalizationText = "RobotEditForm.Robot.Success",
                    });
                }

                this.bExecute.Loading = false;
                this.bStop.Enabled = false;
                this.cRobotINST.Enabled = true;
                this.tRobotInstruction.Enabled = true;
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void Worker_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            int iIndex = e.ProgressPercentage;

            this.tRobotInstruction.SelectedIndex = iIndex + 1;
            this.tRobotInstruction.ScrollLine(iIndex + 1, true);

            this.txtINSTLog.AppendText((iIndex + 1).ToString() + ", ");
        }

        #endregion        

        #region//停止

        private void bStop_Click(object sender, EventArgs e)
        {
            this.re.StopRobot();
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.SaveRobot())
                {
                    return;
                }

                switch (Operate.SystemConfig.StartMode)
                {
                    case Operate.SystemConfig.SystemMode.Process:

                        ((InterfaceInfo.IInjectMode)form).RefreshRobotList();

                        break;

                    case Operate.SystemConfig.SystemMode.Proxy:

                        ((InterfaceInfo.IProxyMode)form).RefreshRobotList();

                        break;
                }

                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "机器人保存成功", TType.Success)
                {
                    LocalizationText = "RobotEditForm.Success"
                });

                this.re.StopRobot();
                this.Dispose();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);

                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "机器人保存失败", TType.Error)
                {
                    LocalizationText = "RobotEditForm.Error"
                });
            }
        }

        private bool SaveRobot()
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtRobotName.Text.Trim()))
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "机器人名称为空", TType.Error)
                    {
                        LocalizationText = "RobotEditForm.RName.Empty"
                    });

                    return false;
                }

                if (this.RInstruction.Count > 0)
                {
                    int iReturn = Operate.RobotConfig.Robot.CheckRobotInstruction(this.form, this.RInstruction);
                    if (iReturn > -1 && iReturn < tRobotInstruction.ToDataTable().Rows.Count)
                    {
                        this.tRobotInstruction.SelectedIndex = iReturn + 1;
                        this.tRobotInstruction.ScrollLine(iReturn, true);

                        return false;
                    }
                }

                string RName_New = this.txtRobotName.Text.Trim();
                Operate.RobotConfig.Robot.UpdateRobot(riSelect, RName_New, this.RInstruction);

                return true;
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return false;
        }

        #endregion

        #region//退出

        private void bExit_Click(object sender, System.EventArgs e)
        {
            this.re.StopRobot();
            this.Dispose();
        }

        #endregion        
    }
}
