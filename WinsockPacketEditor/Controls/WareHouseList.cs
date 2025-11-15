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
                new AntdUI.Column("Stores", "封包数量", AntdUI.ColumnAlign.Center)
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
                this.tWareHouseList.BackColor = Operate.SystemConfig.Color_40;
                this.tWareHouseList.ColumnBack = Operate.SystemConfig.Color_40;
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

        private void ddMenu_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            this.ddMenu.SelectedValue = null;

            switch (e.Value.ToString())
            {
                case "Add":

                    Operate.WareHouseConfig.WareHouse.AddWareHouse_New();
                    this.tWareHouseList.ScrollBar.ValueY = tWareHouseList.ScrollBar.MaxY;

                    break;

                case "Import":

                    Operate.WareHouseConfig.List.LoadWareHouseList_Dialog(this.form);

                    break;

                case "Export":

                    if (Operate.WareHouseConfig.List.lstWareHouseInfo.Count > 0)
                    {
                        Operate.WareHouseConfig.List.SaveWareHouseList_Dialog(this.form, string.Empty, null);
                    }

                    break;

                case "Clear":

                    if (Operate.WareHouseConfig.List.lstWareHouseInfo.Count > 0)
                    {
                        Operate.WareHouseConfig.List.CleanUpWareHouseList_Dialog(this.form);
                    }

                    break;
            }
        }

        private void tWareHouseList_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            if (e.Record is WareHouseInfo whi)
            {
                switch (e.Btn.Id)
                {
                    case "bEdit":

                        Operate.WareHouseConfig.WareHouse.OpenWareHouseEdit(this.form, whi);

                        break;

                    case "bDelete":

                        List<WareHouseInfo> whiList = new List<WareHouseInfo>
                        {
                            whi
                        };

                        Operate.WareHouseConfig.List.UpdateWareHouseList_ByListAction(this.form, Operate.SystemConfig.ListAction.Delete, whiList);

                        break;
                }
            }
        }

        private void tWareHouseList_CellDoubleClick(object sender, TableClickEventArgs e)
        {
            if (e.Record is WareHouseInfo whi)
            {
                Operate.WareHouseConfig.WareHouse.OpenWareHouseEdit(this.form, whi);
            }
        }

        #endregion

        

        
    }
}
