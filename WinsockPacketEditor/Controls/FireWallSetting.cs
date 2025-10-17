using AntdUI;
using System;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class FireWallSetting : UserControl
    {
        private Form form = null;

        #region//窗体事件

        public FireWallSetting(Form form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void FireWallSetting_Load(object sender, EventArgs e)
        {
            this.InitTable_WhiteList();
            this.InitTable_BlackList();
            this.InitMenu_WhiteList();
            this.InitMenu_BlackList();

            this.cbEnableFireWall.Checked = Operate.ProxyConfig.Proxy.EnableFireWall;
            this.EnableFireWall_Changed();

            if (Operate.ProxyConfig.Proxy.WhiteListMode)
            {
                this.rbWhiteListMode.Checked = true;
            }
            else
            {
                this.rbBlackListMode.Checked = true;
            }
        }

        private void InitMenu_WhiteList()
        {
            this.ddMenu_WhiteList.Items.AddRange(new AntdUI.SelectItem[]
            {
                new AntdUI.SelectItem("新增")
                {
                    Tag = "Add",
                    LocalizationText = "MapSettingsForm.MapLocal.Add",
                    IconSvg = "DesktopOutlined",
                },
                new AntdUI.SelectItem("导入白名单")
                {
                    Tag = "Import",
                    LocalizationText = "MapSettingsForm.MapLocal.Import",
                    IconSvg = "FolderOpenOutlined",
                },
                new AntdUI.SelectItem("导出白名单")
                {
                    Tag = "Export",
                    LocalizationText = "MapSettingsForm.MapLocal.Export",
                    IconSvg = "DeliveredProcedureOutlined",
                },
                new AntdUI.SelectItem("清空白名单")
                {
                    Tag = "Clear",
                    LocalizationText = "MapSettingsForm.MapLocal.Clear",
                    IconSvg = "DeleteOutlined",
                },
            });
        }

        private void InitMenu_BlackList()
        {
            this.ddMenu_BlackList.Items.AddRange(new AntdUI.SelectItem[]
            {
                new AntdUI.SelectItem("新增")
                {
                    Tag = "Add",
                    LocalizationText = "MapSettingsForm.MapLocal.Add",
                    IconSvg = "DesktopOutlined",
                },
                new AntdUI.SelectItem("导入黑名单")
                {
                    Tag = "Import",
                    LocalizationText = "MapSettingsForm.MapLocal.Import",
                    IconSvg = "FolderOpenOutlined",
                },
                new AntdUI.SelectItem("导出黑名单")
                {
                    Tag = "Export",
                    LocalizationText = "MapSettingsForm.MapLocal.Export",
                    IconSvg = "DeliveredProcedureOutlined",
                },
                new AntdUI.SelectItem("清空黑名单")
                {
                    Tag = "Clear",
                    LocalizationText = "MapSettingsForm.MapLocal.Clear",
                    IconSvg = "DeleteOutlined",
                },
            });
        }

        public void RefreshWhiteList()
        {
            this.tWhiteList.Refresh();
        }

        public void RefreshBlackList()
        {
            this.tBlackList.Refresh();
        }

        #endregion

        #region//初始化数据表

        private void InitTable_WhiteList()
        {
            tWhiteList.Columns = new AntdUI.ColumnCollection {
                new AntdUI.Column("IPAddress", "IP地址").SetFixed().SetLocalizationTitleID("Table.WhiteList.Column."),
                new AntdUI.Column("IPLocation", "所属地").SetLocalizationTitleID("Table.WhiteList.Column."),
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
                }.SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.WhiteList.Column."),
            };

            this.tWhiteList.Binding(Operate.ProxyConfig.Proxy.lstWhiteList);
        }

        private void InitTable_BlackList()
        {
            tBlackList.Columns = new AntdUI.ColumnCollection {
                new AntdUI.Column("IPAddress", "IP地址").SetFixed().SetLocalizationTitleID("Table.BlackList.Column."),
                new AntdUI.Column("IPLocation", "所属地").SetLocalizationTitleID("Table.BlackList.Column."),
                new AntdUI.Column("ExpiryTime", "过期时间").SetSortOrder().SetLocalizationTitleID("Table.BlackList.Column."),
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
                }.SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.BlackList.Column."),
            };

            this.tBlackList.Binding(Operate.ProxyConfig.Proxy.lstBlackList);
        }

        #endregion

        #region//启用连接控制

        private void cbEnableFireWall_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            this.EnableFireWall_Changed();
        }

        private void EnableFireWall_Changed()
        { 
            this.rbWhiteListMode.Enabled = 
                this.rbBlackListMode.Enabled = 
                this.tWhiteList.Enabled =
                this.tBlackList.Enabled =
                this.ddMenu_WhiteList.Enabled =
                this.ddMenu_BlackList.Enabled =                
                this.cbEnableFireWall.Checked;
        }

        #endregion

        #region//白名单 - 菜单

        private void ddMenu_WhiteList_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            this.ddMenu_WhiteList.SelectedValue = null;

            switch (e.Value.ToString())
            {
                case "Add":

                    Operate.ProxyConfig.Proxy.OpenWhiteListEdit(this.form, this, null);

                    break;

                case "Import":

                    Operate.ProxyConfig.Mapping.UpdateMapLocal_ByListAction(this.form, Operate.SystemConfig.ListAction.Import, null);

                    break;

                case "Export":

                    //if (Operate.ProxyConfig.Mapping.lstMapLocal.Count > 0)
                    //{
                    //    Operate.ProxyConfig.Mapping.UpdateMapLocal_ByListAction(this.form, Operate.SystemConfig.ListAction.Export, null);
                    //}

                    break;

                case "Clear":

                    //if (Operate.ProxyConfig.Mapping.lstMapLocal.Count > 0)
                    //{
                    //    Operate.ProxyConfig.Mapping.UpdateMapLocal_ByListAction(this.form, Operate.SystemConfig.ListAction.CleanUp, null);
                    //}

                    break;
            }
        }

        private void tWhiteList_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            if (e.Record is WhiteListInfo wli)
            {
                switch (e.Btn.Id)
                {
                    case "bEdit":

                        Operate.ProxyConfig.Proxy.OpenWhiteListEdit(this.form, this, wli);

                        break;

                    case "bDelete":

                        //Operate.ProxyConfig.Mapping.DeleteMapLocal_Dialog(this.form, ml);

                        break;
                }
            }
        }

        private void tWhiteList_CellDoubleClick(object sender, TableClickEventArgs e)
        {
            if (e.Record is WhiteListInfo wli)
            {
                Operate.ProxyConfig.Proxy.OpenWhiteListEdit(this.form, this, wli);
            }
        }

        #endregion

        #region//黑名单 - 菜单



        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            Operate.ProxyConfig.Proxy.EnableFireWall = this.cbEnableFireWall.Checked;
            Operate.ProxyConfig.Proxy.WhiteListMode = this.rbWhiteListMode.Checked;

            AntdUI.Message.open(new AntdUI.Message.Config(this.form, "防火墙设置保存成功", TType.Success)
            {
                LocalizationText = "FireWallSetting.Success"
            });

            this.Dispose();
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
