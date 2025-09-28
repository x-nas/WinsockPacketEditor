using AntdUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class RobotList : UserControl
    {
        private Form form;

        #region//窗体事件

        public RobotList(Form _form)
        {
            InitializeComponent();
            this.form = _form;
        }

        private void RobotList_Load(object sender, EventArgs e)
        {
            this.InitMenu();
            this.InitTable_RobotList();
            this.Dark_Changed();
        }

        private void InitTable_RobotList()
        {
            tRobotList.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.ColumnSwitch("IsEnable", "启用", AntdUI.ColumnAlign.Center)
                {
                    Width = "60",
                    Call = (value, record, i_row, i_col) =>
                    {
                        return value;
                    }
                }.SetFixed().SetLocalizationTitleID("Table.RobotList.Column."),
                new AntdUI.Column("RName", "机器人名称").SetLocalizationTitleID("Table.RobotList.Column."),
                new AntdUI.Column("Status", "状态")
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is RobotInfo ri)
                        {
                            AntdUI.CellBadge cellBadge = null;

                            if(ri.IsEnable)
                            {
                                cellBadge = new AntdUI.CellBadge(AntdUI.TState.Success, AntdUI.Localization.Get("Enable", "启用"));
                                if(ri.ExecutionCount > 0)
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
                }.SetLocalizationTitleID("Table.RobotList.Column."),
                new AntdUI.Column("ExecutionCount", "执行次数", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return new AntdUI.CellText(value.ToString())
                        {
                            Fore = Color.FromArgb(22, 119, 255),
                        };
                    },
                }.SetLocalizationTitleID("Table.RobotList.Column."),
                new AntdUI.Column("RInstruction", "指令条数", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is RobotInfo ri)
                        {
                            return new AntdUI.CellText(ri.RInstruction.Count.ToString())
                            {
                                Fore = Color.Green,
                            };
                        }

                        return null;                        
                    },
                }.SetLocalizationTitleID("Table.RobotList.Column."),
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
                }.SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.RobotList.Column."),
            };

            this.tRobotList.Binding(Operate.RobotConfig.List.lstRobotInfo);
        }

        private void InitMenu()
        {
            this.ddMenu.Items.AddRange(new AntdUI.SelectItem[]
            {
                new AntdUI.SelectItem("新增机器人")
                {
                    Tag = "Add",
                    LocalizationText = "RobotList.Add",
                    IconSvg = "RobotOutlined",
                },
                new AntdUI.SelectItem("导入机器人列表")
                {
                    Tag = "Import",
                    LocalizationText = "RobotList.Import",
                    IconSvg = "FolderOpenOutlined",
                },
                new AntdUI.SelectItem("导出所有机器人")
                {
                    Tag = "Export",
                    LocalizationText = "RobotList.Export",
                    IconSvg = "DeliveredProcedureOutlined",
                },
                new AntdUI.SelectItem("清空所有机器人")
                {
                    Tag = "Clear",
                    LocalizationText = "RobotList.Clear",
                    IconSvg = "DeleteOutlined",
                },
            });
        }

        public void Dark_Changed()
        {
            if (AntdUI.Config.IsDark)
            {
                this.tRobotList.BackColor = Operate.SystemConfig.Color_40;
                this.tRobotList.ColumnBack = Operate.SystemConfig.Color_40;
            }
            else
            {
                this.tRobotList.BackColor = Color.White;
                this.tRobotList.ColumnBack = null;
            }
        }

        public void RefreshRobotList()
        {
            this.tRobotList.Refresh();
        }

        #endregion

        #region//机器人列表 - 菜单

        private void bEnableAll_Click(object sender, EventArgs e)
        {
            foreach (RobotInfo ri in Operate.RobotConfig.List.lstRobotInfo)
            {
                ri.IsEnable = true;
            }
        }

        private void bDisableAll_Click(object sender, EventArgs e)
        {
            foreach (RobotInfo ri in Operate.RobotConfig.List.lstRobotInfo)
            {
                ri.IsEnable = false;
            }
        }

        private void bRobotList_Reset_Click(object sender, EventArgs e)
        {
            Operate.RobotConfig.List.InitRobotList_Count();
        }

        private void bRobotList_Start_Click(object sender, EventArgs e)
        {
            if (Operate.RobotConfig.List.lstRobotInfo.Count > 0)
            {
                if (!this.bgwRobotList.IsBusy)
                {
                    this.bRobotList_Start.Enabled = false;
                    this.bRobotList_Stop.Enabled = true;
                    this.tRobotList.Enabled = false;

                    Operate.RobotConfig.List.lstRobotExecute.Clear();

                    this.bgwRobotList.RunWorkerAsync();
                }
            }
        }

        private void bRobotList_Stop_Click(object sender, EventArgs e)
        {
            this.bgwRobotList.CancelAsync();
        }

        private void ddMenu_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            this.ddMenu.SelectedValue = null;

            switch (e.Value.ToString())
            {
                case "Add":

                    Operate.RobotConfig.Robot.AddRobot_New();
                    this.tRobotList.ScrollBar.ValueY = tRobotList.ScrollBar.MaxY;

                    break;

                case "Import":

                    Operate.RobotConfig.List.LoadRobotList_Dialog(this.form);

                    break;

                case "Export":

                    if (Operate.RobotConfig.List.lstRobotInfo.Count > 0)
                    {
                        Operate.RobotConfig.List.SaveRobotList_Dialog(this.form, string.Empty, null);
                    }

                    break;

                case "Clear":

                    if (Operate.RobotConfig.List.lstRobotInfo.Count > 0)
                    {
                        Operate.RobotConfig.List.CleanUpRobotList_Dialog(this.form);
                    }

                    break;
            }
        }

        private void tRobotList_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            if (e.Record is RobotInfo ri)
            {
                switch (e.Btn.Id)
                {
                    case "bEdit":

                        Operate.RobotConfig.Robot.OpenRobotEdit(this.form, ri);

                        break;

                    case "bDelete":

                        List<RobotInfo> riList = new List<RobotInfo>
                        {
                            ri,
                        };

                        Operate.RobotConfig.List.UpdateRobotList_ByListAction(this.form, Operate.SystemConfig.ListAction.Delete, riList);

                        break;
                }
            }
        }

        private void tRobotList_CellDoubleClick(object sender, TableClickEventArgs e)
        {
            if (e.Record is RobotInfo ri)
            {
                Operate.RobotConfig.Robot.OpenRobotEdit(this.form, ri);
            }                
        }        

        #endregion

        #region//机器人列表 - 右键菜单

        private void tRobotList_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Operate.RobotConfig.List.lstRobotInfo.Count == 0)
                {
                    return;
                }

                AntdUI.ContextMenuStrip.open(new AntdUI.ContextMenuStrip.Config(tRobotList, (item) =>
                {
                    List<RobotInfo> riList = new List<RobotInfo>();

                    foreach (int SelectIndex in this.tRobotList.SelectedIndexs)
                    {
                        riList.Add(Operate.RobotConfig.List.lstRobotInfo[SelectIndex - 1]);
                    }

                    switch (item.ID)
                    {
                        case "Top":

                            if (riList.Count > 0)
                            {
                                Operate.RobotConfig.List.UpdateRobotList_ByListAction(this.form, Operate.SystemConfig.ListAction.Top, riList);
                            }

                            break;

                        case "Up":

                            if (riList.Count > 0)
                            {
                                Operate.RobotConfig.List.UpdateRobotList_ByListAction(this.form, Operate.SystemConfig.ListAction.Up, riList);
                            }

                            break;

                        case "Down":

                            if (riList.Count > 0)
                            {
                                Operate.RobotConfig.List.UpdateRobotList_ByListAction(this.form, Operate.SystemConfig.ListAction.Down, riList);
                            }

                            break;

                        case "Bottom":

                            if (riList.Count > 0)
                            {
                                Operate.RobotConfig.List.UpdateRobotList_ByListAction(this.form, Operate.SystemConfig.ListAction.Bottom, riList);
                            }

                            break;

                        case "Copy":

                            if (riList.Count > 0)
                            {
                                Operate.RobotConfig.List.UpdateRobotList_ByListAction(this.form, Operate.SystemConfig.ListAction.Copy, riList);
                                this.tRobotList.ScrollBar.ValueY = tRobotList.ScrollBar.MaxY;
                            }

                            break;

                        case "Export":

                            if (riList.Count > 0)
                            {
                                Operate.RobotConfig.List.UpdateRobotList_ByListAction(this.form, Operate.SystemConfig.ListAction.Export, riList);
                            }

                            break;

                        case "Delete":

                            if (riList.Count > 0)
                            {
                                Operate.RobotConfig.List.UpdateRobotList_ByListAction(this.form, Operate.SystemConfig.ListAction.Delete, riList);
                            }

                            break;
                    }

                    this.tRobotList.SelectedIndex = -1;
                }, Operate.SystemConfig.GetCMS_List()));
            }
        }

        #endregion

        #region//执行机器人列表（异步）

        private void bgwRobotList_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                foreach (RobotInfo ri in Operate.RobotConfig.List.lstRobotInfo)
                {
                    if (ri.IsEnable)
                    {
                        RobotExecute re = Operate.RobotConfig.Robot.DoRobot(ri.RID, null);
                        if (re != null)
                        {
                            if (Operate.SystemConfig.ListExecute == Operate.SystemConfig.Execute.Together)
                            {
                                Operate.RobotConfig.List.lstRobotExecute.Add(re);
                            }
                            else
                            {
                                while (re.Worker.IsBusy)
                                {
                                    if (this.bgwRobotList.CancellationPending)
                                    {
                                        re.StopRobot();

                                        e.Cancel = true;
                                        return;
                                    }

                                    Thread.Sleep(100);
                                }
                            }
                        }
                    }
                }

                while (Operate.RobotConfig.List.lstRobotExecute.Count > 0)
                {
                    foreach (RobotExecute re in Operate.RobotConfig.List.lstRobotExecute.ToList())
                    {
                        if (this.bgwRobotList.CancellationPending)
                        {
                            re.StopRobot();
                        }

                        if (!re.Worker.IsBusy)
                        {
                            Operate.RobotConfig.List.lstRobotExecute.Remove(re);
                        }
                    }

                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(bgwRobotList_DoWork), ex.Message);
            }
        }

        private void bgwRobotList_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            this.bRobotList_Start.Enabled = true;
            this.bRobotList_Stop.Enabled = false;
            this.tRobotList.Enabled = true;
        }

        #endregion        
    }
}
