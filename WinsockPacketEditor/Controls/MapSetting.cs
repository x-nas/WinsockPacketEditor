using AntdUI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class MapSetting : UserControl
    {
        private Form form;

        #region//窗体事件

        public MapSetting(Form form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void MapSetting_Load(object sender, EventArgs e)
        {
            this.Text = AntdUI.Localization.Get("MapSettingsForm", "映射设置");

            this.cbEnable_MapLocal.Checked = Operate.ProxyConfig.Mapping.Enable_MapLocal;
            this.cbEnable_MapRemote.Checked = Operate.ProxyConfig.Mapping.Enable_MapRemote;
            
            this.InitTable_MapLocal();
            this.InitTable_MapRemote();
            this.InitMenu_MapLocal();
            this.InitMenu_MapRemote();
            this.EnableMapLocal_Changed();
            this.EnableMapRemote_Changed();
        }

        private void InitMenu_MapLocal()
        {
            this.ddMenu_MapLocal.Items.AddRange(new AntdUI.SelectItem[]
            {
                new AntdUI.SelectItem("新增")
                {
                    Tag = "Add",
                    LocalizationText = "MapSettingsForm.MapLocal.Add",
                    IconSvg = "BlockOutlined",
                },
                new AntdUI.SelectItem("导入本地映射")
                {
                    Tag = "Import",
                    LocalizationText = "MapSettingsForm.MapLocal.Import",
                    IconSvg = "FolderOpenOutlined",
                },
                new AntdUI.SelectItem("导出本地映射")
                {
                    Tag = "Export",
                    LocalizationText = "MapSettingsForm.MapLocal.Export",
                    IconSvg = "DeliveredProcedureOutlined",
                },
                new AntdUI.SelectItem("清空本地映射")
                {
                    Tag = "Clear",
                    LocalizationText = "MapSettingsForm.MapLocal.Clear",
                    IconSvg = "DeleteOutlined",
                },
            });
        }

        private void InitMenu_MapRemote()
        {
            this.ddMenu_MapRemote.Items.AddRange(new AntdUI.SelectItem[]
            {
                new AntdUI.SelectItem("新增")
                {
                    Tag = "Add",
                    LocalizationText = "MapSettingsForm.MapRemote.Add",
                    IconSvg = "BlockOutlined",
                },
                new AntdUI.SelectItem("导入远程映射")
                {
                    Tag = "Import",
                    LocalizationText = "MapSettingsForm.MapRemote.Import",
                    IconSvg = "FolderOpenOutlined",
                },
                new AntdUI.SelectItem("导出远程映射")
                {
                    Tag = "Export",
                    LocalizationText = "MapSettingsForm.MapRemote.Export",
                    IconSvg = "DeliveredProcedureOutlined",
                },
                new AntdUI.SelectItem("清空远程映射")
                {
                    Tag = "Clear",
                    LocalizationText = "MapSettingsForm.MapRemote.Clear",
                    IconSvg = "DeleteOutlined",
                },
            });
        }

        public void RefreshMapLocal()
        { 
            this.tMapLocal.Refresh();
        }

        public void RefreshMapRemote()
        {
            this.tMapRemote.Refresh();
        }

        #endregion

        #region//初始化数据表

        private void InitTable_MapLocal()
        {
            tMapLocal.Columns = new AntdUI.ColumnCollection {
                new AntdUI.ColumnCheck("IsEnable").SetFixed(),
                new AntdUI.Column("RemotePath", "远程地址")
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is MapLocal ml)
                        {
                            return $"{ml.ProtocolType.ToString().ToLower()}://{ml.Host}:{ml.Port}{ml.RemotePath}";
                        }

                        return value;
                    },
                }.SetLocalizationTitleID("Table.MapLocal.Column."),
                new AntdUI.Column("LocalPath", "本地文件").SetLocalizationTitleID("Table.MapLocal.Column."),
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
                }.SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.MapLocal.Column."),
            };

            this.tMapLocal.ColumnFont = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.tMapLocal.Binding(Operate.ProxyConfig.Mapping.lstMapLocal);
        }

        private void InitTable_MapRemote()
        {
            tMapRemote.Columns = new AntdUI.ColumnCollection {
                new AntdUI.ColumnCheck("IsEnable").SetFixed(),
                new AntdUI.Column("HostFrom", "请求地址")
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is MapRemote mr)
                        {
                            return $"{mr.ProtocolTypeFrom.ToString().ToLower()}://{mr.HostFrom}:{mr.PortFrom}{mr.PathFrom}";
                        }

                        return value;
                    },
                }.SetLocalizationTitleID("Table.MapRemote.Column."),
                new AntdUI.Column("HostTo", "映射地址")
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is MapRemote mr)
                        {
                            return $"{mr.ProtocolTypeTo.ToString().ToLower()}://{mr.HostTo}:{mr.PortTo}{mr.PathTo}";
                        }

                        return value;
                    },
                }.SetLocalizationTitleID("Table.MapRemote.Column."),
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
                }.SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.MapRemote.Column."),
            };

            this.tMapRemote.ColumnFont = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.tMapRemote.Binding(Operate.ProxyConfig.Mapping.lstMapRemote);
        }

        #endregion

        #region//启用本地映射

        private void cbEnable_MapLocal_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.EnableMapLocal_Changed();
        }

        private void EnableMapLocal_Changed()
        {
            this.tMapLocal.Enabled = this.ddMenu_MapLocal.Enabled = this.cbEnable_MapLocal.Checked;
        }

        #endregion        

        #region//启用远程映射

        private void cbEnable_MapRemote_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.EnableMapRemote_Changed();
        }

        private void EnableMapRemote_Changed()
        {
            this.tMapRemote.Enabled = this.ddMenu_MapRemote.Enabled = this.cbEnable_MapRemote.Checked;
        }

        #endregion        

        #region//本地映射 - 菜单

        private void ddMenu_MapLocal_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            this.ddMenu_MapLocal.SelectedValue = null;

            switch (e.Value.ToString())
            {
                case "Add":

                    var MapLocalEdit = new MapLocalEdit(this.form, this, null);
                    AntdUI.Modal.open(new AntdUI.Modal.Config(this.form, AntdUI.Localization.Get("MapLocalForm", "本地映射编辑"), MapLocalEdit)
                    {
                        Keyboard = false,
                        MaskClosable = false,
                        BtnHeight = 0,
                    });

                    break;

                case "Import":

                    Operate.ProxyConfig.Mapping.UpdateMapLocal_ByListAction(this.form, Operate.SystemConfig.ListAction.Import, null);

                    break;

                case "Export":

                    if (Operate.ProxyConfig.Mapping.lstMapLocal.Count > 0)
                    {
                        Operate.ProxyConfig.Mapping.UpdateMapLocal_ByListAction(this.form, Operate.SystemConfig.ListAction.Export, null);
                    }

                    break;

                case "Clear":

                    if (Operate.ProxyConfig.Mapping.lstMapLocal.Count > 0)
                    {
                        Operate.ProxyConfig.Mapping.UpdateMapLocal_ByListAction(this.form, Operate.SystemConfig.ListAction.CleanUp, null);
                    }

                    break;
            }
        }

        private void tMapLocal_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            if (e.Record is MapLocal ml)
            {
                switch (e.Btn.Id)
                {
                    case "bEdit":

                        this.OpenMapLocalEdit(ml);

                        break;

                    case "bDelete":

                        Operate.ProxyConfig.Mapping.DeleteMapLocal_Dialog(this.form, ml);

                        break;
                }
            }
        }

        private void tMapLocal_CellDoubleClick(object sender, TableClickEventArgs e)
        {
            if (e.Record is MapLocal ml)
            {
                this.OpenMapLocalEdit(ml);
            }
        }

        private void OpenMapLocalEdit(MapLocal ml)
        {
            var MapLocalEdit = new MapLocalEdit(this.form, this, ml);
            AntdUI.Modal.open(new AntdUI.Modal.Config(this.form, AntdUI.Localization.Get("MapLocalForm", "本地映射编辑"), MapLocalEdit)
            {
                Keyboard = false,
                MaskClosable = false,
                BtnHeight = 0,
            });
        }

        #endregion

        #region//本地映射 - 右键菜单

        private void tMapLocal_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Operate.ProxyConfig.Mapping.lstMapLocal.Count == 0)
                {
                    return;
                }

                if (e.Record is MapLocal ml)
                {
                    AntdUI.ContextMenuStrip.open(new AntdUI.ContextMenuStrip.Config(tMapLocal, (item) =>
                    {
                        switch (item.ID)
                        {
                            case "Top":

                                Operate.ProxyConfig.Mapping.UpdateMapLocal_ByListAction(this.form, Operate.SystemConfig.ListAction.Top, ml);

                                break;

                            case "Up":

                                Operate.ProxyConfig.Mapping.UpdateMapLocal_ByListAction(this.form, Operate.SystemConfig.ListAction.Up, ml);

                                break;

                            case "Down":

                                Operate.ProxyConfig.Mapping.UpdateMapLocal_ByListAction(this.form, Operate.SystemConfig.ListAction.Down, ml);

                                break;

                            case "Bottom":

                                Operate.ProxyConfig.Mapping.UpdateMapLocal_ByListAction(this.form, Operate.SystemConfig.ListAction.Bottom, ml);

                                break;
                        }

                        this.tMapLocal.SelectedIndex = -1;
                    }, Operate.ProxyConfig.Mapping.GetCMS_Mapping()));
                }
            }
        }

        #endregion

        #region//远程映射 - 菜单

        private void ddMenu_MapRemote_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            this.ddMenu_MapRemote.SelectedValue = null;

            switch (e.Value.ToString())
            {
                case "Add":

                    var MapRemoteEdit = new MapRemoteEdit(this.form, this, null);
                    AntdUI.Modal.open(new AntdUI.Modal.Config(this.form, AntdUI.Localization.Get("MapRemoteForm", "远程映射编辑"), MapRemoteEdit)
                    {
                        Keyboard = false,
                        MaskClosable = false,
                        BtnHeight = 0,
                    });

                    break;

                case "Import":

                    Operate.ProxyConfig.Mapping.UpdateMapRemote_ByListAction(this.form, Operate.SystemConfig.ListAction.Import, null);

                    break;

                case "Export":

                    if (Operate.ProxyConfig.Mapping.lstMapRemote.Count > 0)
                    {
                        Operate.ProxyConfig.Mapping.UpdateMapRemote_ByListAction(this.form, Operate.SystemConfig.ListAction.Export, null);
                    }

                    break;

                case "Clear":

                    if (Operate.ProxyConfig.Mapping.lstMapRemote.Count > 0)
                    {
                        Operate.ProxyConfig.Mapping.UpdateMapRemote_ByListAction(this.form, Operate.SystemConfig.ListAction.CleanUp, null);
                    }

                    break;
            }
        }
                
        private void tMapRemote_CellButtonUp(object sender, TableButtonEventArgs e)
        {
            if (e.Record is MapRemote mr)
            {
                switch (e.Btn.Id)
                {
                    case "bEdit":

                        this.OpenMapRemoteEdit(mr);

                        break;

                    case "bDelete":

                        Operate.ProxyConfig.Mapping.DeleteMapRemote_Dialog(this.form, mr);

                        break;
                }
            }
        }

        private void tMapRemote_CellDoubleClick(object sender, TableClickEventArgs e)
        {
            if (e.Record is MapRemote mr)
            {
                this.OpenMapRemoteEdit(mr);
            }
        }

        private void OpenMapRemoteEdit(MapRemote mr)
        {
            var MapRemoteEdit = new MapRemoteEdit(this.form, this, mr);
            AntdUI.Modal.open(new AntdUI.Modal.Config(this.form, AntdUI.Localization.Get("MapRemoteForm", "远程映射编辑"), MapRemoteEdit)
            {
                Keyboard = false,
                MaskClosable = false,
                BtnHeight = 0,
            });
        }

        #endregion

        #region//远程映射 - 右键菜单

        private void tMapRemote_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Operate.ProxyConfig.Mapping.lstMapRemote.Count == 0)
                {
                    return;
                }

                if (e.Record is MapRemote mr)
                {
                    AntdUI.ContextMenuStrip.open(new AntdUI.ContextMenuStrip.Config(tMapRemote, (item) =>
                    {
                        switch (item.ID)
                        {
                            case "Top":

                                Operate.ProxyConfig.Mapping.UpdateMapRemote_ByListAction(this.form, Operate.SystemConfig.ListAction.Top, mr);

                                break;

                            case "Up":

                                Operate.ProxyConfig.Mapping.UpdateMapRemote_ByListAction(this.form, Operate.SystemConfig.ListAction.Up, mr);

                                break;

                            case "Down":

                                Operate.ProxyConfig.Mapping.UpdateMapRemote_ByListAction(this.form, Operate.SystemConfig.ListAction.Down, mr);

                                break;

                            case "Bottom":

                                Operate.ProxyConfig.Mapping.UpdateMapRemote_ByListAction(this.form, Operate.SystemConfig.ListAction.Bottom, mr);

                                break;
                        }

                        this.tMapRemote.SelectedIndex = -1;
                    }, Operate.ProxyConfig.Mapping.GetCMS_Mapping()));
                }
            }
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            Operate.ProxyConfig.Mapping.Enable_MapLocal = this.cbEnable_MapLocal.Checked;
            Operate.ProxyConfig.Mapping.Enable_MapRemote = this.cbEnable_MapRemote.Checked;

            AntdUI.Message.open(new AntdUI.Message.Config(this.form, "映射设置保存成功", TType.Success)
            {
                LocalizationText = "MapSettingsForm.Success"
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
