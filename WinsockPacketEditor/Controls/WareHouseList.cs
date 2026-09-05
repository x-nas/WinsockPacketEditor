using System;
using AntdUI;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class WareHouseList : UserControl
    {
        private Form form;

        #region//窗体事件

        public WareHouseList(Form form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void WareHouseList_Load(object sender, System.EventArgs e)
        {
            this.InitMenu();
            this.InitTable_WareHouseList();
            this.Dark_Changed();
        }

        private void InitTable_WareHouseList()
        {
            tWareHouseList.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("WName", "仓库名称").SetLocalizationTitleID("Table.WareHouseList.Column."),
                new AntdUI.Column("Stores", "仓储数量", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is WareHouseInfo whi)
                        {
                            return new AntdUI.CellText(whi.Stores.Count.ToString())
                            {
                                Fore = Color.FromArgb(22, 119, 255),
                            };
                        }

                        return null;                        
                    },
                }.SetLocalizationTitleID("Table.WareHouseList.Column."),
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
                }.SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.WareHouseList.Column."),
            };

            this.tWareHouseList.Binding(Operate.WareHouseConfig.List.lstWareHouseInfo);
        }

        private void InitMenu()
        {
            this.ddMenu.Items.AddRange(new AntdUI.SelectItem[]
            {
                new AntdUI.SelectItem("新增仓库")
                {
                    Tag = "Add",
                    LocalizationText = "WareHouseList.Add",
                    IconSvg = "BankOutlined",
                },
                new AntdUI.SelectItem("导入仓库列表")
                {
                    Tag = "Import",
                    LocalizationText = "WareHouseList.Import",
                    IconSvg = "FolderOpenOutlined",
                },
                new AntdUI.SelectItem("导出所有仓库")
                {
                    Tag = "Export",
                    LocalizationText = "WareHouseList.Export",
                    IconSvg = "DeliveredProcedureOutlined",
                },
                new AntdUI.SelectItem("清空所有仓库")
                {
                    Tag = "Clear",
                    LocalizationText = "WareHouseList.Clear",
                    IconSvg = "DeleteOutlined",
                },
            });
        }

        public void Dark_Changed()
        {
            if (AntdUI.Config.IsDark)
            {
                this.tWareHouseList.BackColor = UiTheme.Color_40;
                this.tWareHouseList.ColumnBack = UiTheme.Color_40;
            }
            else
            {
                this.tWareHouseList.BackColor = Color.White;
                this.tWareHouseList.ColumnBack = null;
            }
        }

        public void RefreshWareHouseList()
        {
            this.tWareHouseList.Refresh();
        }

        #endregion

        #region//仓库列表 - 菜单

        private void bAutoStores_Click(object sender, System.EventArgs e)
        {
            AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("Setting", "设置"), new AutoStoresList(this.form))
            {
                Keyboard = false,
                MaskClosable = false,
                BtnHeight = 0,
            });
        }

        private async void ddMenu_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            try
            {
                    this.ddMenu.SelectedValue = null;

                    switch (e.Value.ToString())
                    {
                        case "Add":

                            Operate.WareHouseConfig.WareHouse.AddWareHouse_New();
                            this.tWareHouseList.ScrollBar.ValueY = tWareHouseList.ScrollBar.MaxY;

                            break;

                        case "Import":

                            await Operate.WareHouseConfig.List.LoadWareHouseList_Dialog();

                            break;

                        case "Export":

                            if (Operate.WareHouseConfig.List.lstWareHouseInfo.Count > 0)
                            {
                                await Operate.WareHouseConfig.List.SaveWareHouseList_Dialog(string.Empty, null);
                            }

                            break;

                        case "Clear":

                            if (Operate.WareHouseConfig.List.lstWareHouseInfo.Count > 0)
                            {
                                await Operate.WareHouseConfig.List.CleanUpWareHouseList_Dialog();
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

        private async void tWareHouseList_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            try
            {
                    if (e.Record is WareHouseInfo whi)
                    {
                        switch (e.Btn.Id)
                        {
                            case "bEdit":

                                UiDialogs.OpenWareHouseEdit(this.form, whi);

                                break;

                            case "bDelete":

                                List<WareHouseInfo> whiList = new List<WareHouseInfo>
                                {
                                    whi
                                };

                                await Operate.WareHouseConfig.List.UpdateWareHouseList_ByListAction(Operate.SystemConfig.ListAction.Delete, whiList);

                                break;
                        }
                    }
            }
            catch (Exception ex)
            {
                //async void：await 之后抛出的异常不会被 WinForms 兜住，必须自己捕获
                Operate.DoLog(nameof(tWareHouseList_CellButtonClick), ex);
            }
        }

        private void tWareHouseList_CellDoubleClick(object sender, TableClickEventArgs e)
        {
            //只响应鼠标左键：AntdUI.Table 对任意鼠标键的双击都会抛 CellDoubleClick
            if (e.Button != MouseButtons.Left) return;

            if (e.Record is WareHouseInfo whi)
            {
                UiDialogs.OpenWareHouseEdit(this.form, whi);
            }
        }

        #endregion

        #region//仓库列表 - 右键菜单

        private async void tWareHouseList_CellClick(object sender, TableClickEventArgs e)
        {
            try
            {
                    if (e.Button == MouseButtons.Right)
                    {
                        if (Operate.WareHouseConfig.List.lstWareHouseInfo.Count == 0)
                        {
                            return;
                        }

                        AntdUI.ContextMenuStrip.open(new AntdUI.ContextMenuStrip.Config(tWareHouseList, async (item) =>
                        {
                            List<WareHouseInfo> whiList = new List<WareHouseInfo>();
                            foreach (int SelectIndex in this.tWareHouseList.SelectedIndexs)
                            {
                                whiList.Add(Operate.WareHouseConfig.List.lstWareHouseInfo[SelectIndex - 1]);
                            }

                            switch (item.ID)
                            {
                                case "Top":

                                    if (whiList.Count > 0)
                                    {
                                        await Operate.WareHouseConfig.List.UpdateWareHouseList_ByListAction(Operate.SystemConfig.ListAction.Top, whiList);
                                    }

                                    break;

                                case "Up":

                                    if (whiList.Count > 0)
                                    {
                                        await Operate.WareHouseConfig.List.UpdateWareHouseList_ByListAction(Operate.SystemConfig.ListAction.Up, whiList);
                                    }

                                    break;

                                case "Down":

                                    if (whiList.Count > 0)
                                    {
                                        await Operate.WareHouseConfig.List.UpdateWareHouseList_ByListAction(Operate.SystemConfig.ListAction.Down, whiList);
                                    }

                                    break;

                                case "Bottom":

                                    if (whiList.Count > 0)
                                    {
                                        await Operate.WareHouseConfig.List.UpdateWareHouseList_ByListAction(Operate.SystemConfig.ListAction.Bottom, whiList);
                                    }

                                    break;

                                case "Copy":

                                    if (whiList.Count > 0)
                                    {
                                        await Operate.WareHouseConfig.List.UpdateWareHouseList_ByListAction(Operate.SystemConfig.ListAction.Copy, whiList);
                                        this.tWareHouseList.ScrollBar.ValueY = tWareHouseList.ScrollBar.MaxY;
                                    }

                                    break;

                                case "Export":

                                    if (whiList.Count > 0)
                                    {
                                        await Operate.WareHouseConfig.List.UpdateWareHouseList_ByListAction(Operate.SystemConfig.ListAction.Export, whiList);
                                    }

                                    break;

                                case "Delete":

                                    if (whiList.Count > 0)
                                    {
                                        await Operate.WareHouseConfig.List.UpdateWareHouseList_ByListAction(Operate.SystemConfig.ListAction.Delete, whiList);
                                    }

                                    break;
                            }

                            this.tWareHouseList.SelectedIndex = -1;
                        }, Operate.SystemConfig.GetCMS_List().ToAntd()));
                    }
            }
            catch (Exception ex)
            {
                //async void：await 之后抛出的异常不会被 WinForms 兜住，必须自己捕获
                Operate.DoLog(nameof(tWareHouseList_CellClick), ex);
            }
        }

        #endregion
    }
}
