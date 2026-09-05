using AntdUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class SendList : UserControl
    {
        private Form form;

        #region//窗体事件

        public SendList(Form form)
        {
            InitializeComponent();
            this.form = form;
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
                    Width = "60",
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
                this.tSendList.BackColor = UiTheme.Color_40;
                this.tSendList.ColumnBack = UiTheme.Color_40;
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
                if (!Operate.SendConfig.List.bgwSendList.IsBusy)
                {
                    this.bSendList_Start.Enabled = false;
                    this.bSendList_Stop.Enabled = true;
                    this.tSendList.Enabled = false;

                    Operate.SendConfig.List.bgwSendList.RunWorkerCompleted -= bgwSendList_RunWorkerCompleted;
                    Operate.SendConfig.List.bgwSendList.RunWorkerCompleted += bgwSendList_RunWorkerCompleted;
                    Operate.SendConfig.List.StartSendList();
                }
            }
        }

        private void bgwSendList_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            this.bSendList_Start.Enabled = true;
            this.bSendList_Stop.Enabled = false;
            this.tSendList.Enabled = true;
        }

        private void bSendList_Stop_Click(object sender, EventArgs e)
        {
            Operate.SendConfig.List.StopSendList();
        }

        private async void ddMenu_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            try
            {
                    this.ddMenu.SelectedValue = null;

                    switch (e.Value.ToString())
                    {
                        case "Add":

                            Operate.SendConfig.Send.AddSend_New();
                            this.tSendList.ScrollBar.ValueY = tSendList.ScrollBar.MaxY;

                            break;

                        case "Import":

                            await Operate.SendConfig.List.LoadSendList_Dialog();

                            break;

                        case "Export":

                            if (Operate.SendConfig.List.lstSendInfo.Count > 0)
                            {
                                await Operate.SendConfig.List.SaveSendList_Dialog(string.Empty, null);
                            }

                            break;

                        case "Clear":

                            if (Operate.SendConfig.List.lstSendInfo.Count > 0)
                            {
                                await Operate.SendConfig.List.CleanUpSendList_Dialog();
                            }

                            break;
                    }
            }
            catch (Exception ex)
            {
                //async void：await 之后抛出的异常不会被 WinForms 兜住，必须自己捕获
                Operate.DoLog(nameof(ddMenu_SelectedValueChanged), ex);
            }
        }

        private async void tSendList_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            try
            {
                    if (e.Record is SendInfo si)
                    {
                        switch (e.Btn.Id)
                        {
                            case "bEdit":

                                UiDialogs.OpenSendEdit(this.form, si);

                                break;

                            case "bDelete":

                                List<SendInfo> siList = new List<SendInfo>
                                {
                                    si
                                };

                                await Operate.SendConfig.List.UpdateSendList_ByListAction(Operate.SystemConfig.ListAction.Delete, siList);

                                break;
                        }
                    }
            }
            catch (Exception ex)
            {
                //async void：await 之后抛出的异常不会被 WinForms 兜住，必须自己捕获
                Operate.DoLog(nameof(tSendList_CellButtonClick), ex);
            }
        }

        private void tSendList_CellDoubleClick(object sender, TableClickEventArgs e)
        {
            //只响应鼠标左键：AntdUI.Table 对任意鼠标键的双击都会抛 CellDoubleClick
            if (e.Button != MouseButtons.Left) return;

            if (e.Record is SendInfo si)
            {
                UiDialogs.OpenSendEdit(this.form, si);
            }                
        }        

        #endregion

        #region//发送列表 - 右键菜单

        private async void tSendList_CellClick(object sender, TableClickEventArgs e)
        {
            try
            {
                    if (e.Button == MouseButtons.Right)
                    {
                        if (Operate.SendConfig.List.lstSendInfo.Count == 0)
                        {
                            return;
                        }

                        AntdUI.ContextMenuStrip.open(new AntdUI.ContextMenuStrip.Config(tSendList, async (item) =>
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
                                        await Operate.SendConfig.List.UpdateSendList_ByListAction(Operate.SystemConfig.ListAction.Top, siList);
                                    }

                                    break;

                                case "Up":

                                    if (siList.Count > 0)
                                    {
                                        await Operate.SendConfig.List.UpdateSendList_ByListAction(Operate.SystemConfig.ListAction.Up, siList);
                                    }

                                    break;

                                case "Down":

                                    if (siList.Count > 0)
                                    {
                                        await Operate.SendConfig.List.UpdateSendList_ByListAction(Operate.SystemConfig.ListAction.Down, siList);
                                    }

                                    break;

                                case "Bottom":

                                    if (siList.Count > 0)
                                    {
                                        await Operate.SendConfig.List.UpdateSendList_ByListAction(Operate.SystemConfig.ListAction.Bottom, siList);
                                    }

                                    break;

                                case "Copy":

                                    if (siList.Count > 0)
                                    {
                                        await Operate.SendConfig.List.UpdateSendList_ByListAction(Operate.SystemConfig.ListAction.Copy, siList);
                                        this.tSendList.ScrollBar.ValueY = tSendList.ScrollBar.MaxY;
                                    }

                                    break;

                                case "Export":

                                    if (siList.Count > 0)
                                    {
                                        await Operate.SendConfig.List.UpdateSendList_ByListAction(Operate.SystemConfig.ListAction.Export, siList);
                                    }

                                    break;

                                case "Delete":

                                    if (siList.Count > 0)
                                    {
                                        await Operate.SendConfig.List.UpdateSendList_ByListAction(Operate.SystemConfig.ListAction.Delete, siList);
                                    }

                                    break;
                            }

                            this.tSendList.SelectedIndex = -1;
                        }, Operate.SystemConfig.GetCMS_List().ToAntd()));
                    }
            }
            catch (Exception ex)
            {
                //async void：await 之后抛出的异常不会被 WinForms 兜住，必须自己捕获
                Operate.DoLog(nameof(tSendList_CellClick), ex);
            }
        }

        #endregion        
    }
}
