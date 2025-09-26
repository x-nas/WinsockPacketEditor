using AntdUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class SendList : UserControl
    {
        private Form form;

        #region//窗体事件

        public SendList(Form _form)
        {
            InitializeComponent();
            this.form = _form;
        }

        private void SendList_Load(object sender, EventArgs e)
        {
            this.InitMenu();
            this.InitTable_SendList();
            this.Dark_Changed();
        }

        private void InitTable_SendList()
        {
            tSendList.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.ColumnSwitch("IsEnable", "启用", AntdUI.ColumnAlign.Center)
                {
                    Width = "80",
                    Call = (value, record, i_row, i_col) =>
                    {
                        return value;
                    }
                }.SetFixed().SetLocalizationTitleID("Table.SendList.Column."),
                new AntdUI.Column("SName", "发送名称").SetLocalizationTitleID("Table.SendList.Column."),
                new AntdUI.Column("Status", "状态")
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is SendInfo si)
                        {
                            AntdUI.CellBadge cellBadge = null;

                            if(si.IsEnable)
                            {
                                cellBadge = new AntdUI.CellBadge(AntdUI.TState.Success, AntdUI.Localization.Get("Enable", "启用"));
                                if(si.ExecutionCount > 0)
                                {
                                    cellBadge = new AntdUI.CellBadge(AntdUI.TState.Processing, AntdUI.Localization.Get("Working", "处理中"));
                                }
                            }
                            else
                            {
                                cellBadge = new AntdUI.CellBadge(AntdUI.TState.Error, AntdUI.Localization.Get("Disable", "禁用"));
                            }

                            return cellBadge;
                        }

                        return null;
                    },
                }.SetLocalizationTitleID("Table.SendList.Column."),
                new AntdUI.Column("ExecutionCount", "执行次数", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return new AntdUI.CellText(value.ToString())
                        {
                            Fore = Color.FromArgb(22, 119, 255),
                        };
                    },
                }.SetLocalizationTitleID("Table.SendList.Column."),
                new AntdUI.Column("ExecutionSuccess", "成功次数", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return new AntdUI.CellText(value.ToString())
                        {
                            Fore = Color.Green,
                        };
                    },
                }.SetLocalizationTitleID("Table.SendList.Column."),
                new AntdUI.Column("ExecutionFail", "失败次数", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return new AntdUI.CellText(value.ToString())
                        {
                            Fore = Color.Red,
                        };
                    },
                }.SetLocalizationTitleID("Table.SendList.Column."),
                new AntdUI.Column("SSystemSocket", "套接字", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        if((bool)value)
                        {
                            return new CellTag(Operate.SystemConfig.SystemSocket.ToString(), TTypeMini.Error);
                        }
                        else
                        {
                            return new CellTag(AntdUI.Localization.Get("Customize", "自定义"), TTypeMini.Success);
                        }
                    },
                }.SetLocalizationTitleID("Table.SendList.Column."),
                new AntdUI.Column("SLoopCNT", "循环")
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is SendInfo si)
                        {
                            return new CellTag[]
                            {
                                new CellTag(si.SLoopCNT.ToString() + " " + AntdUI.Localization.Get("Count", "次"), TTypeMini.Success),
                                new CellTag(AntdUI.Localization.Get("Interval", "间隔") + " " + si.SLoopINT.ToString() + " " + AntdUI.Localization.Get("Millisecond", "毫秒"), TTypeMini.Warn)
                            };
                        }

                        return null;
                    },
                }.SetLocalizationTitleID("Table.SendList.Column."),
                new Column("SNotes", "备注")
                {
                    LineBreak = true,
                }.SetLocalizationTitleID("Table.SendList.Column."),
                new AntdUI.Column("CellLinks", "操作")
                {
                    Render = (value, record, rowindex)=>
                    {
                        return new AntdUI.CellLink[]
                        {
                            new AntdUI.CellButton("bEdit", null, AntdUI.TTypeMini.Primary).SetIcon("EditOutlined"),
                            new AntdUI.CellButton("bDelete", null, AntdUI.TTypeMini.Error).SetIcon("CloseOutlined"),
                        };
                    },
                }.SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.SendList.Column."),
            };

            this.tSendList.ColumnFont = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.tSendList.Binding(Operate.SendConfig.List.lstSendInfo);
        }

        private void InitMenu()
        {
            this.ddMenu.Items.AddRange(new AntdUI.SelectItem[]
            {
                new AntdUI.SelectItem("新增发送")
                {
                    Tag = "Add",
                    LocalizationText = "SendList.Add",
                    IconSvg = "SendOutlined",
                },
                new AntdUI.SelectItem("导入发送列表")
                {
                    Tag = "Import",
                    LocalizationText = "SendList.Import",
                    IconSvg = "FolderOpenOutlined",
                },
                new AntdUI.SelectItem("导出所有发送")
                {
                    Tag = "Export",
                    LocalizationText = "SendList.Export",
                    IconSvg = "DeliveredProcedureOutlined",
                },
                new AntdUI.SelectItem("清空所有发送")
                {
                    Tag = "Clear",
                    LocalizationText = "SendList.Clear",
                    IconSvg = "DeleteOutlined",
                },
            });
        }

        public void Dark_Changed()
        {
            if (AntdUI.Config.IsDark)
            {
                this.tSendList.BackColor = Operate.SystemConfig.Color_40;
                this.tSendList.ColumnBack = Operate.SystemConfig.Color_40;
            }
            else
            {
                this.tSendList.BackColor = Color.White;
                this.tSendList.ColumnBack = null;
            }
        }

        public void RefreshSendList()
        {
            this.tSendList.Refresh();
        }

        #endregion

        #region//发送列表 - 菜单

        private void bEnableAll_Click(object sender, EventArgs e)
        {
            foreach (SendInfo si in Operate.SendConfig.List.lstSendInfo)
            {
                si.IsEnable = true;
            }
        }

        private void bDisableAll_Click(object sender, EventArgs e)
        {
            foreach (SendInfo si in Operate.SendConfig.List.lstSendInfo)
            {
                si.IsEnable = false;
            }
        }

        private void bSendList_Reset_Click(object sender, EventArgs e)
        {
            Operate.SendConfig.List.InitSendList_Count();
        }

        private void bSendList_Start_Click(object sender, EventArgs e)
        {
            if (Operate.SendConfig.List.lstSendInfo.Count > 0)
            {
                if (!this.bgwSendList.IsBusy)
                {
                    this.bSendList_Start.Enabled = false;
                    this.bSendList_Stop.Enabled = true;
                    this.tSendList.Enabled = false;

                    Operate.SendConfig.List.lstSendExecute.Clear();

                    this.bgwSendList.RunWorkerAsync();
                }
            }
        }

        private void bSendList_Stop_Click(object sender, EventArgs e)
        {
            this.bgwSendList.CancelAsync();
        }

        private void ddMenu_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            this.ddMenu.SelectedValue = null;

            switch (e.Value.ToString())
            {
                case "Add":

                    Operate.SendConfig.Send.AddSend_New();
                    this.tSendList.ScrollBar.ValueY = tSendList.ScrollBar.MaxY;

                    break;

                case "Import":

                    Operate.SendConfig.List.LoadSendList_Dialog(this.form);

                    break;

                case "Export":

                    if (Operate.SendConfig.List.lstSendInfo.Count > 0)
                    {
                        Operate.SendConfig.List.SaveSendList_Dialog(this.form, string.Empty, null);
                    }

                    break;

                case "Clear":

                    if (Operate.SendConfig.List.lstSendInfo.Count > 0)
                    {
                        Operate.SendConfig.List.CleanUpSendList_Dialog(this.form);
                    }

                    break;
            }
        }

        private void tSendList_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            if (e.Record is SendInfo si)
            {
                switch (e.Btn.Id)
                {
                    case "bEdit":

                        this.OpenSendEdit(si);

                        break;

                    case "bDelete":

                        List<SendInfo> siList = new List<SendInfo>
                        {
                            si
                        };

                        Operate.SendConfig.List.UpdateSendList_ByListAction(this.form, Operate.SystemConfig.ListAction.Delete, siList);

                        break;
                }
            }
        }

        private void tSendList_CellDoubleClick(object sender, TableClickEventArgs e)
        {
            if (e.Record is SendInfo si)
            {
                this.OpenSendEdit(si);
            }                
        }

        private void OpenSendEdit(SendInfo si)
        {
            var SendEdit = new SendEdit(this.form, si);
            AntdUI.Modal.open(new AntdUI.Modal.Config(this.form, AntdUI.Localization.Get("SendEditForm", "发送编辑"), SendEdit)
            {
                Keyboard = false,
                MaskClosable = false,
                BtnHeight = 0,
            });
        }

        #endregion

        #region//发送列表 - 右键菜单

        private void tSendList_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Operate.SendConfig.List.lstSendInfo.Count == 0)
                {
                    return;
                }

                AntdUI.ContextMenuStrip.open(new AntdUI.ContextMenuStrip.Config(tSendList, (item) =>
                {
                    List<SendInfo> siList = new List<SendInfo>();

                    foreach (int SelectIndex in this.tSendList.SelectedIndexs)
                    {
                        siList.Add(Operate.SendConfig.List.lstSendInfo[SelectIndex - 1]);
                    }

                    switch (item.ID)
                    {
                        case "Top":

                            if (siList.Count > 0)
                            {
                                Operate.SendConfig.List.UpdateSendList_ByListAction(this.form, Operate.SystemConfig.ListAction.Top, siList);
                            }

                            break;

                        case "Up":

                            if (siList.Count > 0)
                            {
                                Operate.SendConfig.List.UpdateSendList_ByListAction(this.form, Operate.SystemConfig.ListAction.Up, siList);
                            }

                            break;

                        case "Down":

                            if (siList.Count > 0)
                            {
                                Operate.SendConfig.List.UpdateSendList_ByListAction(this.form, Operate.SystemConfig.ListAction.Down, siList);
                            }

                            break;

                        case "Bottom":

                            if (siList.Count > 0)
                            {
                                Operate.SendConfig.List.UpdateSendList_ByListAction(this.form, Operate.SystemConfig.ListAction.Bottom, siList);
                            }

                            break;

                        case "Copy":

                            if (siList.Count > 0)
                            {
                                Operate.SendConfig.List.UpdateSendList_ByListAction(this.form, Operate.SystemConfig.ListAction.Copy, siList);
                                this.tSendList.ScrollBar.ValueY = tSendList.ScrollBar.MaxY;
                            }

                            break;

                        case "Export":

                            if (siList.Count > 0)
                            {
                                Operate.SendConfig.List.UpdateSendList_ByListAction(this.form, Operate.SystemConfig.ListAction.Export, siList);
                            }

                            break;

                        case "Delete":

                            if (siList.Count > 0)
                            {
                                Operate.SendConfig.List.UpdateSendList_ByListAction(this.form, Operate.SystemConfig.ListAction.Delete, siList);
                            }

                            break;
                    }

                    this.tSendList.SelectedIndex = -1;
                }, Operate.SystemConfig.GetCMS_List()));
            }
        }

        #endregion

        #region//执行发送列表（异步）

        private void bgwSendList_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                for (int index = 0; index < Operate.SendConfig.List.lstSendInfo.Count; index++)
                {
                    SendInfo si = Operate.SendConfig.List.lstSendInfo[index];
                    if (si.IsEnable)
                    {
                        SendExecute se = Operate.SendConfig.Send.DoSend(si.SID);
                        if (se != null)
                        {
                            if (Operate.SystemConfig.ListExecute == Operate.SystemConfig.Execute.Together)
                            {
                                Operate.SendConfig.List.lstSendExecute.Add(se);
                            }
                            else
                            {
                                while (se.Worker.IsBusy)
                                {
                                    if (this.bgwSendList.CancellationPending)
                                    {
                                        se.StopSend();

                                        e.Cancel = true;
                                        return;
                                    }

                                    Thread.Sleep(10);
                                }
                            }
                        }
                    }
                }

                while (Operate.SendConfig.List.lstSendExecute.Count > 0)
                {
                    foreach (SendExecute se in Operate.SendConfig.List.lstSendExecute.ToList())
                    {
                        if (this.bgwSendList.CancellationPending)
                        {
                            se.StopSend();
                        }

                        if (!se.Worker.IsBusy)
                        {
                            Operate.SendConfig.List.lstSendExecute.Remove(se);
                        }
                    }

                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(bgwSendList_DoWork), ex.Message);
            }
        }

        private void bgwSendList_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            this.bSendList_Start.Enabled = true;
            this.bSendList_Stop.Enabled = false;
            this.tSendList.Enabled = true;
        }

        #endregion        
    }
}
