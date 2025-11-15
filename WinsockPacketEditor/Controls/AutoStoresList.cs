using AntdUI;
using System;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class AutoStoresList : UserControl
    {
        private Form form;

        #region//窗体事件

        public AutoStoresList(Form form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void AutoStores_Load(object sender, System.EventArgs e)
        {
            this.cbEnable_AutoStores.Checked = Operate.WareHouseConfig.WareHouse.Enable_AutoStores;

            this.InitMenu();
            this.InitTable_AutoStores();
            this.Enable_AutoStores_Changed();
        }

        private void InitMenu()
        {
            this.ddMenu.Items.AddRange(new AntdUI.SelectItem[]
            {
                new AntdUI.SelectItem("新增")
                {
                    Tag = "Add",
                    LocalizationText = "AutoStores.Add",
                    IconSvg = "ShoppingCartOutlined",
                },
                new AntdUI.SelectItem("导入自动入库")
                {
                    Tag = "Import",
                    LocalizationText = "AutoStores.Import",
                    IconSvg = "FolderOpenOutlined",
                },
                new AntdUI.SelectItem("导出自动入库")
                {
                    Tag = "Export",
                    LocalizationText = "AutoStores.Export",
                    IconSvg = "DeliveredProcedureOutlined",
                },
                new AntdUI.SelectItem("清空自动入库")
                {
                    Tag = "Clear",
                    LocalizationText = "AutoStores.Clear",
                    IconSvg = "DeleteOutlined",
                },
            });
        }

        private void InitTable_AutoStores()
        {
            tAutoStores.Columns = new AntdUI.ColumnCollection {
                new AntdUI.ColumnCheck("IsEnable").SetFixed(),
                new AntdUI.Column("PacketHead", "指定包头").SetLocalizationTitleID("Table.AutoStores.Column."),
                new AntdUI.Column("WID", "仓库名称")
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is AutoStoresInfo asi)
                        {
                            return Operate.WareHouseConfig.WareHouse.GetWareHouseName_ByGuid(asi.WID);
                        }

                        return null;
                    },
                }.SetLocalizationTitleID("Table.AutoStores.Column."),
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
                }.SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.AutoStores.Column."),
            };

            this.tAutoStores.Binding(Operate.WareHouseConfig.List.lstAutoStoresInfo);
        }

        public void RefreshAutoStores()
        {
            this.tAutoStores.Refresh();
        }

        #endregion

        #region//启用自动入库

        private void cbEnable_AutoStores_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            this.Enable_AutoStores_Changed();
        }

        private void Enable_AutoStores_Changed()
        { 
            this.ddMenu.Enabled = this.tAutoStores.Enabled = this.cbEnable_AutoStores.Checked;
        }

        #endregion

        #region//自动入库 - 菜单

        private void ddMenu_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            this.ddMenu.SelectedValue = null;

            switch (e.Value.ToString())
            {
                case "Add":

                    Operate.WareHouseConfig.WareHouse.OpenAutoStoresEdit(this.form, this, null);

                    break;

                case "Import":

                    Operate.WareHouseConfig.List.UpdateAutoStores_ByListAction(this.form, Operate.SystemConfig.ListAction.Import, null);

                    break;

                case "Export":

                    if (Operate.WareHouseConfig.List.lstAutoStoresInfo.Count > 0)
                    {
                        Operate.WareHouseConfig.List.UpdateAutoStores_ByListAction(this.form, Operate.SystemConfig.ListAction.Export, null);
                    }

                    break;

                case "Clear":

                    if (Operate.WareHouseConfig.List.lstAutoStoresInfo.Count > 0)
                    {
                        Operate.WareHouseConfig.List.UpdateAutoStores_ByListAction(this.form, Operate.SystemConfig.ListAction.CleanUp, null);
                    }

                    break;
            }
        }

        private void tAutoStores_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            if (e.Record is AutoStoresInfo asi)
            {
                switch (e.Btn.Id)
                {
                    case "bEdit":

                        Operate.WareHouseConfig.WareHouse.OpenAutoStoresEdit(this.form, this, asi);

                        break;

                    case "bDelete":

                        Operate.WareHouseConfig.WareHouse.DeleteAutoStores_Dialog(this.form, asi);

                        break;
                }
            }
        }

        private void tAutoStores_CellDoubleClick(object sender, TableClickEventArgs e)
        {
            if (e.Record is AutoStoresInfo asi)
            {
                Operate.WareHouseConfig.WareHouse.OpenAutoStoresEdit(this.form, this, asi);
            }
        }

        #endregion

        #region//自动入库 - 右键菜单

        private void tAutoStores_CellClick(object sender, TableClickEventArgs e)
        {
            try
            {
                if (e.Button == MouseButtons.Right)
                {
                    if (Operate.WareHouseConfig.List.lstAutoStoresInfo.Count == 0)
                    {
                        return;
                    }

                    if (e.Record is AutoStoresInfo asi)
                    {
                        AntdUI.ContextMenuStrip.open(new AntdUI.ContextMenuStrip.Config(tAutoStores, (item) =>
                        {
                            switch (item.ID)
                            {
                                case "Top":

                                    Operate.WareHouseConfig.List.UpdateAutoStores_ByListAction(this.form, Operate.SystemConfig.ListAction.Top, asi);

                                    break;

                                case "Up":

                                    Operate.WareHouseConfig.List.UpdateAutoStores_ByListAction(this.form, Operate.SystemConfig.ListAction.Up, asi);

                                    break;

                                case "Down":

                                    Operate.WareHouseConfig.List.UpdateAutoStores_ByListAction(this.form, Operate.SystemConfig.ListAction.Down, asi);

                                    break;

                                case "Bottom":

                                    Operate.WareHouseConfig.List.UpdateAutoStores_ByListAction(this.form, Operate.SystemConfig.ListAction.Bottom, asi);

                                    break;
                            }

                            this.tAutoStores.SelectedIndex = -1;
                        }, Operate.WareHouseConfig.List.GetCMS_AutoStores()));
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(tAutoStores_CellClick), ex);
            }            
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, System.EventArgs e)
        {
            Operate.WareHouseConfig.WareHouse.Enable_AutoStores = this.cbEnable_AutoStores.Checked;

            AntdUI.Message.open(new AntdUI.Message.Config(this.form, "自动入库保存成功", TType.Success)
            {
                LocalizationText = "AutoStoresList.Success"
            });

            this.Dispose();
        }

        #endregion

        #region//退出

        private void bExit_Click(object sender, System.EventArgs e)
        {
            this.Dispose();
        }

        #endregion
    }
}
