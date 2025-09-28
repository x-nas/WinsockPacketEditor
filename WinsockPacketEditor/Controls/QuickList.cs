using AntdUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class QuickList : UserControl
    {
        private Form form = null;

        #region//窗体事件

        public QuickList(Form form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void QuickList_Load(object sender, EventArgs e)
        {
            this.tabQuickList.SelectedIndex = 0;

            this.InitFilterList();
            this.InitSendList();
            this.InitRobotList();
        }

        private void InitFilterList()
        {
            tFilterList.Columns = new AntdUI.ColumnCollection {
                new AntdUI.ColumnSwitch("IsEnable", "启用", AntdUI.ColumnAlign.Center)
                {
                    Call = (value, record, i_row, i_col) =>
                    {
                        return value;
                    }
                }.SetFixed().SetWidth("Auto").SetLocalizationTitleID("Table.FilterList.Column."),
                new AntdUI.Column("FName", "滤镜名称").SetLocalizationTitleID("Table.FilterList.Column."),
                new AntdUI.Column("ExecutionCount", "执行次数", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return new AntdUI.CellText(value.ToString())
                        {
                            Fore = Color.FromArgb(22, 119, 255),
                        };
                    },
                }.SetWidth("Auto").SetLocalizationTitleID("Table.FilterList.Column."),
            };

            this.tFilterList.Binding(Operate.FilterConfig.List.lstFilterInfo);
        }

        private void InitSendList()
        {
            tSendList.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.ColumnSwitch("IsEnable", "启用", AntdUI.ColumnAlign.Center)
                {
                    Call = (value, record, i_row, i_col) =>
                    {
                        return value;
                    }
                }.SetFixed().SetWidth("Auto").SetLocalizationTitleID("Table.SendList.Column."),
                new AntdUI.Column("SName", "发送名称").SetLocalizationTitleID("Table.SendList.Column."),
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
            };

            this.tSendList.Binding(Operate.SendConfig.List.lstSendInfo);
        }

        private void InitRobotList()
        {
            tRobotList.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.ColumnSwitch("IsEnable", "启用", AntdUI.ColumnAlign.Center)
                {
                    Call = (value, record, i_row, i_col) =>
                    {
                        return value;
                    }
                }.SetFixed().SetWidth("Auto").SetLocalizationTitleID("Table.RobotList.Column."),
                new AntdUI.Column("RName", "机器人名称").SetLocalizationTitleID("Table.RobotList.Column."),
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
            };

            this.tRobotList.Binding(Operate.RobotConfig.List.lstRobotInfo);
        }

        #endregion

        #region//滤镜列表

        private void tFilterList_CellDoubleClick(object sender, AntdUI.TableClickEventArgs e)
        {
            if (e.Record is FilterInfo fi)
            {
                Operate.FilterConfig.Filter.OpenFilterEdit(this.form, fi);
            }
        }        

        private void bFilterList_EnableAll_Click(object sender, EventArgs e)
        {
            foreach (FilterInfo fi in Operate.FilterConfig.List.lstFilterInfo)
            {
                fi.IsEnable = true;
            }
        }

        private void bFilterList_DisableAll_Click(object sender, EventArgs e)
        {
            foreach (FilterInfo fi in Operate.FilterConfig.List.lstFilterInfo)
            {
                fi.IsEnable = false;
            }
        }

        private void bFilterList_ResetCount_Click(object sender, EventArgs e)
        {
            Operate.FilterConfig.List.InitFilterList_Count();
        }

        private void bFilterList_Add_Click(object sender, EventArgs e)
        {
            Operate.FilterConfig.Filter.AddFilter_New();
            this.tFilterList.ScrollBar.ValueY = tFilterList.ScrollBar.MaxY;
        }

        private void bFilterList_Delete_Click(object sender, EventArgs e)
        {
            if (Operate.FilterConfig.List.lstFilterInfo.Count > 0)
            {
                Operate.FilterConfig.List.CleanUpFilterList_Dialog(this.form);
            }
        }

        #endregion

        #region//滤镜列表 - 右键菜单

        private void tFilterList_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Operate.FilterConfig.List.lstFilterInfo.Count == 0)
                {
                    return;
                }

                AntdUI.ContextMenuStrip.open(new AntdUI.ContextMenuStrip.Config(tFilterList, (item) =>
                {
                    List<FilterInfo> fiList = new List<FilterInfo>();

                    foreach (int SelectIndex in this.tFilterList.SelectedIndexs)
                    {
                        fiList.Add(Operate.FilterConfig.List.lstFilterInfo[SelectIndex - 1]);
                    }

                    switch (item.ID)
                    {
                        case "Top":

                            if (fiList.Count > 0)
                            {
                                Operate.FilterConfig.List.UpdateFilterList_ByListAction(this.form, Operate.SystemConfig.ListAction.Top, fiList);
                            }

                            break;

                        case "Up":

                            if (fiList.Count > 0)
                            {
                                Operate.FilterConfig.List.UpdateFilterList_ByListAction(this.form, Operate.SystemConfig.ListAction.Up, fiList);
                            }

                            break;

                        case "Down":

                            if (fiList.Count > 0)
                            {
                                Operate.FilterConfig.List.UpdateFilterList_ByListAction(this.form, Operate.SystemConfig.ListAction.Down, fiList);
                            }

                            break;

                        case "Bottom":

                            if (fiList.Count > 0)
                            {
                                Operate.FilterConfig.List.UpdateFilterList_ByListAction(this.form, Operate.SystemConfig.ListAction.Bottom, fiList);
                            }

                            break;

                        case "Copy":

                            if (fiList.Count > 0)
                            {
                                Operate.FilterConfig.List.UpdateFilterList_ByListAction(this.form, Operate.SystemConfig.ListAction.Copy, fiList);
                                this.tFilterList.ScrollBar.ValueY = tFilterList.ScrollBar.MaxY;
                            }

                            break;

                        case "Export":

                            if (fiList.Count > 0)
                            {
                                Operate.FilterConfig.List.UpdateFilterList_ByListAction(this.form, Operate.SystemConfig.ListAction.Export, fiList);
                            }

                            break;

                        case "Delete":

                            if (fiList.Count > 0)
                            {
                                Operate.FilterConfig.List.UpdateFilterList_ByListAction(this.form, Operate.SystemConfig.ListAction.Delete, fiList);
                            }

                            break;
                    }

                    this.tFilterList.SelectedIndex = -1;
                }, Operate.SystemConfig.GetCMS_List()));
            }
        }

        #endregion        

        #region//发送列表

        private void bSendList_EnableAll_Click(object sender, EventArgs e)
        {
            foreach (SendInfo si in Operate.SendConfig.List.lstSendInfo)
            {
                si.IsEnable = true;
            }
        }

        private void tlpSendList_DisableAll_Click(object sender, EventArgs e)
        {
            foreach (SendInfo si in Operate.SendConfig.List.lstSendInfo)
            {
                si.IsEnable = false;
            }
        }

        private void bSendList_Execute_Click(object sender, EventArgs e)
        {

        }

        private void bSendList_Stop_Click(object sender, EventArgs e)
        {

        }

        private void tlpSendList_ResetCount_Click(object sender, EventArgs e)
        {
            Operate.SendConfig.List.InitSendList_Count();
        }

        private void tlpSendList_Add_Click(object sender, EventArgs e)
        {
            Operate.SendConfig.Send.AddSend_New();
            this.tSendList.ScrollBar.ValueY = tSendList.ScrollBar.MaxY;
        }

        private void tlpSendList_Delete_Click(object sender, EventArgs e)
        {
            if (Operate.SendConfig.List.lstSendInfo.Count > 0)
            {
                Operate.SendConfig.List.CleanUpSendList_Dialog(this.form);
            }
        }

        private void tSendList_CellDoubleClick(object sender, AntdUI.TableClickEventArgs e)
        {
            if (e.Record is SendInfo si)
            {
                Operate.SendConfig.Send.OpenSendEdit(this.form, si);
            }
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

        #region//机器人列表

        private void tRobotList_CellDoubleClick(object sender, TableClickEventArgs e)
        {
            if (e.Record is RobotInfo ri)
            {
                Operate.RobotConfig.Robot.OpenRobotEdit(this.form, ri);
            }
        }

        private void bRobotList_EnableAll_Click(object sender, EventArgs e)
        {
            foreach (RobotInfo ri in Operate.RobotConfig.List.lstRobotInfo)
            {
                ri.IsEnable = true;
            }
        }

        private void bRobotList_DisableAll_Click(object sender, EventArgs e)
        {
            foreach (RobotInfo ri in Operate.RobotConfig.List.lstRobotInfo)
            {
                ri.IsEnable = false;
            }
        }

        private void bRobotList_Execute_Click(object sender, EventArgs e)
        {

        }

        private void bRobotList_Stop_Click(object sender, EventArgs e)
        {

        }

        private void bRobotList_ResetCount_Click(object sender, EventArgs e)
        {
            Operate.RobotConfig.List.InitRobotList_Count();
        }

        private void bRobotList_Add_Click(object sender, EventArgs e)
        {
            Operate.RobotConfig.Robot.AddRobot_New();
            this.tRobotList.ScrollBar.ValueY = tRobotList.ScrollBar.MaxY;
        }

        private void bRobotList_Delete_Click(object sender, EventArgs e)
        {
            if (Operate.RobotConfig.List.lstRobotInfo.Count > 0)
            {
                Operate.RobotConfig.List.CleanUpRobotList_Dialog(this.form);
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
    }
}
