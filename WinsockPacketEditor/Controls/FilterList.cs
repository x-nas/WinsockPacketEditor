using AntdUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class FilterList : UserControl
    {
        private Form form;

        #region//窗体事件

        public FilterList(Form _form)
        {
            InitializeComponent();
            this.form = _form;
        }

        private void FilterList_Load(object sender, EventArgs e)
        {
            this.InitMenu();
            this.InitTable_FilterList();
            this.Dark_Changed();
        }

        private void InitTable_FilterList()
        {
            tFilterList.Columns = new AntdUI.ColumnCollection {
                new AntdUI.ColumnSwitch("IsEnable", "启用", AntdUI.ColumnAlign.Center)
                {
                    Width = "60",
                    Call = (value, record, i_row, i_col) =>
                    {
                        return value;
                    }
                }.SetFixed().SetLocalizationTitleID("Table.FilterList.Column."),
                new AntdUI.Column("FName", "滤镜名称").SetLocalizationTitleID("Table.FilterList.Column."),
                new AntdUI.Column("Status", "状态")
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is FilterInfo fi)
                        {
                            if(fi.IsEnable)
                            {
                                if(fi.ExecutionCount > 0)
                                {
                                    return new AntdUI.CellBadge(AntdUI.TState.Processing, AntdUI.Localization.Get("Working", "处理中"));
                                }
                                else
                                {
                                    return new AntdUI.CellBadge(AntdUI.TState.Success, AntdUI.Localization.Get("Enable", "启用"));
                                }
                            }
                            else
                            {
                                return new AntdUI.CellBadge(AntdUI.TState.Error, AntdUI.Localization.Get("Disable", "禁用"));
                            }
                        }

                        return value;
                    },
                }.SetLocalizationTitleID("Table.FilterList.Column."),
                new AntdUI.Column("FAction", "动作")
                {
                    Render = (value, record, rowindex)=>
                    {
                        switch((Operate.FilterConfig.Filter.FilterAction)value)
                        {
                            case Operate.FilterConfig.Filter.FilterAction.Replace:
                                return AntdUI.Localization.Get("Replace", "替换");

                            case Operate.FilterConfig.Filter.FilterAction.Change:
                                return AntdUI.Localization.Get("Change", "换包");

                            case Operate.FilterConfig.Filter.FilterAction.Intercept:
                                return AntdUI.Localization.Get("Intercept", "拦截");

                            case Operate.FilterConfig.Filter.FilterAction.NoModify_NoDisplay:
                                return AntdUI.Localization.Get("NoModifyNoDisplay", "不修改不显示");

                            case Operate.FilterConfig.Filter.FilterAction.NoModify_Display:
                                return AntdUI.Localization.Get("NoModifyDisplay", "不修改只显示");

                            default:
                                return value;
                        }
                    },
                }.SetLocalizationTitleID("Table.FilterList.Column."),
                new AntdUI.Column("ExecutionCount", "执行次数", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return new AntdUI.CellText(value.ToString())
                        {
                            Fore = Color.FromArgb(22, 119, 255),
                        };
                    },
                }.SetLocalizationTitleID("Table.FilterList.Column."),
                new AntdUI.Column("Appoint", "指定类型")
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is FilterInfo fi)
                        {
                            List<CellTag> ctList = new List<CellTag>();

                            if(fi.AppointHeader)
                            {
                                ctList.Add(new AntdUI.CellTag(AntdUI.Localization.Get("Head", "包头") , AntdUI.TTypeMini.Success));
                            }

                            if(fi.AppointSocket)
                            {
                                ctList.Add(new AntdUI.CellTag(AntdUI.Localization.Get("Socket", "套接字"), AntdUI.TTypeMini.Warn));
                            }

                            if(fi.AppointPort)
                            {
                                ctList.Add(new AntdUI.CellTag(AntdUI.Localization.Get("Port", "端口"), AntdUI.TTypeMini.Default));
                            }

                            if(fi.AppointLength)
                            {
                                ctList.Add(new AntdUI.CellTag(AntdUI.Localization.Get("Length", "长度"), AntdUI.TTypeMini.Primary));
                            }

                            if(ctList.Count > 0)
                            {
                                AntdUI.CellTag[] cellTags = new AntdUI.CellTag[ctList.Count];
                                for(int i = 0; i < ctList.Count; i++)
                                {
                                    cellTags[i] = ctList[i];
                                }

                                return cellTags;
                            }
                            else
                            {
                                return null;
                            }
                        }

                        return null;
                    },
                }.SetLocalizationTitleID("Table.FilterList.Column."),
                new AntdUI.Column("Progression", "递进")
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is FilterInfo fi)
                        {
                            List<CellTag> ctList = new List<CellTag>();

                            if(!string.IsNullOrEmpty(fi.ProgressionPosition))
                            {
                                ctList.Add(new AntdUI.CellTag(AntdUI.Localization.Get("Enable", "启用"), AntdUI.TTypeMini.Error));
                            }

                            if(fi.IsProgressionContinuous)
                            {
                                ctList.Add(new AntdUI.CellTag(AntdUI.Localization.Get("Continuous", "连续"), AntdUI.TTypeMini.Success));
                            }

                            if(fi.IsProgressionCarry)
                            {
                                ctList.Add(new AntdUI.CellTag(AntdUI.Localization.Get("Carry", "进位"), AntdUI.TTypeMini.Warn));
                            }

                            if(ctList.Count > 0)
                            {
                                AntdUI.CellTag[] cellTags = new AntdUI.CellTag[ctList.Count];
                                for(int i = 0; i < ctList.Count; i++)
                                {
                                    cellTags[i] = ctList[i];
                                }

                                return cellTags;
                            }
                            else
                            {
                                return null;
                            }
                        }

                        return null;
                    },
                }.SetLocalizationTitleID("Table.FilterList.Column."),
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
                }.SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.FilterList.Column."),
            };

            this.tFilterList.Binding(Operate.FilterConfig.List.lstFilterInfo);
        }

        private void InitMenu()
        {
            this.ddMenu.Items.AddRange(new AntdUI.SelectItem[]
            {
                new AntdUI.SelectItem("新增滤镜")
                {
                    Tag = "Add",
                    LocalizationText = "FilterList.Add",
                    IconSvg = "FilterOutlined",
                },
                new AntdUI.SelectItem("导入滤镜列表")
                {
                    Tag = "Import",
                    LocalizationText = "FilterList.Import",
                    IconSvg = "FolderOpenOutlined",
                },
                new AntdUI.SelectItem("导出所有滤镜")
                {
                    Tag = "Export",
                    LocalizationText = "FilterList.Export",
                    IconSvg = "DeliveredProcedureOutlined",
                },
                new AntdUI.SelectItem("清空所有滤镜")
                {
                    Tag = "Clear",
                    LocalizationText = "FilterList.Clear",
                    IconSvg = "DeleteOutlined",
                },
            });
        }

        public void Dark_Changed()
        {
            if (AntdUI.Config.IsDark)
            {
                this.tFilterList.BackColor = Operate.SystemConfig.Color_40;
                this.tFilterList.ColumnBack = Operate.SystemConfig.Color_40;
            }
            else
            {
                this.tFilterList.BackColor = Color.White;
                this.tFilterList.ColumnBack = null;
            }
        }

        public void RefreshFilterList()
        {
            this.tFilterList.Refresh();
        }

        #endregion

        #region//滤镜列表 - 菜单

        private void bFilterList_Reset_Click(object sender, EventArgs e)
        {
            Operate.FilterConfig.List.InitFilterList_Count();
        }

        private void bEnableAll_Click(object sender, EventArgs e)
        {
            foreach (FilterInfo fi in Operate.FilterConfig.List.lstFilterInfo)
            {
                fi.IsEnable = true;
            }
        }

        private void bDisableAll_Click(object sender, EventArgs e)
        {
            foreach (FilterInfo fi in Operate.FilterConfig.List.lstFilterInfo)
            {
                fi.IsEnable = false;
            }
        }

        private void ddMenu_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            this.ddMenu.SelectedValue = null;

            switch (e.Value.ToString())
            {
                case "Add":

                    Operate.FilterConfig.Filter.AddFilter_New();
                    this.tFilterList.ScrollBar.ValueY = tFilterList.ScrollBar.MaxY;

                    break;

                case "Import":

                    Operate.FilterConfig.List.LoadFilterList_Dialog(this.form);

                    break;

                case "Export":

                    if (Operate.FilterConfig.List.lstFilterInfo.Count > 0)
                    {
                        Operate.FilterConfig.List.SaveFilterList_Dialog(this.form, string.Empty, null);
                    }

                    break;

                case "Clear":

                    if (Operate.FilterConfig.List.lstFilterInfo.Count > 0)
                    {
                        Operate.FilterConfig.List.CleanUpFilterList_Dialog(this.form);
                    }

                    break;
            }
        }

        private void tFilterList_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            if (e.Record is FilterInfo fi)
            {
                switch (e.Btn.Id)
                {
                    case "bEdit":

                        this.OpenFilterEdit(fi);

                        break;

                    case "bDelete":

                        List<FilterInfo> fiList = new List<FilterInfo>();
                        fiList.Add(fi);
                        Operate.FilterConfig.List.UpdateFilterList_ByListAction(this.form, Operate.SystemConfig.ListAction.Delete, fiList);

                        break;
                }
            }
        }

        private void tFilterList_CellDoubleClick(object sender, TableClickEventArgs e)
        {
            if (e.Record is FilterInfo fi)
            {
                this.OpenFilterEdit(fi);
            }
        }

        private void OpenFilterEdit(FilterInfo fi)
        {
            var FilterEdit = new FilterEdit(this.form, fi);
            AntdUI.Modal.open(new AntdUI.Modal.Config(this.form, AntdUI.Localization.Get("FilterEditForm", "滤镜编辑"), FilterEdit)
            {
                Keyboard = false,
                MaskClosable = false,
                BtnHeight = 0,
            });
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
    }
}
