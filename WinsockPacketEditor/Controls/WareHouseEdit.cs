using AntdUI;
using Be.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class WareHouseEdit : UserControl
    {
        private Form form;
        private WareHouseInfo whiSelect;
        private BindingList<DataInfo> Stores;

        #region//窗体事件

        public WareHouseEdit(Form form, WareHouseInfo whiSelect)
        {
            InitializeComponent();
            this.form = form;
            this.whiSelect = whiSelect;
        }

        private void WareHouseEdit_Load(object sender, System.EventArgs e)
        {
            this.hbPacketData.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            this.txtWName.Text = this.whiSelect.WName;
            this.Stores = whiSelect.Stores;

            this.InitMenu();
            this.InitTable_Stores();
            this.Dark_Changed();

            if (this.Stores.Count > 0)
            { 
                this.tStores.SelectedIndex = 1;
            }
        }

        private void Dark_Changed()
        {
            if (AntdUI.Config.IsDark)
            {
                this.tStores.ColumnBack = Operate.SystemConfig.Color_35;
                this.tStores.ColumnFore = Color.Silver;
                this.tStores.ForeColor = Color.LimeGreen;

                this.hbPacketData.BackColor = Operate.SystemConfig.Color_40;
                this.hbPacketData.ForeColor = Color.Silver;
            }
            else
            {
                this.tStores.ColumnBack = null;
                this.tStores.ColumnFore = Color.Black;
                this.tStores.ForeColor = Color.Green;

                this.hbPacketData.BackColor = Color.White;
                this.hbPacketData.ForeColor = Color.Black;
            }
        }        

        private void InitTable_Stores()
        {
            tStores.Columns = new AntdUI.ColumnCollection {
                new AntdUI.ColumnCheck("IsCheck").SetFixed(),
                new AntdUI.Column("", "序号", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return (rowindex + 1);
                    },
                }.SetFixed().SetLocalizationTitleID("Table.PacketList.Column.ID"),
                new AntdUI.Column("PacketLen", "长度", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.PacketList.Column."),
                new AntdUI.Column("PacketBuffer", "数据")
                {
                    Render = (value, record, rowindex)=>
                    {
                        return Operate.PacketConfig.Packet.GetPacketData_Hex((byte[])value, Operate.PacketConfig.Packet.PacketData_MaxLen);
                    },
                }.SetLocalizationTitleID("Table.PacketList.Column."),
            };

            this.tStores.ColumnFont = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.tStores.Binding(this.Stores);
        }

        private void InitMenu()
        {
            this.ddMenu.Items.AddRange(new AntdUI.SelectItem[]
            {
                new AntdUI.SelectItem("导入数据")
                {
                    Tag = "Import",
                    LocalizationText = "SendEditForm.Import",
                    IconSvg = "FolderOpenOutlined",
                },
                new AntdUI.SelectItem("导出所有数据")
                {
                    Tag = "Export",
                    LocalizationText = "SendEditForm.Export",
                    IconSvg = "DeliveredProcedureOutlined",
                },
                new AntdUI.SelectItem("清空所有数据")
                {
                    Tag = "Clear",
                    LocalizationText = "SendEditForm.Clear",
                    IconSvg = "DeleteOutlined",
                },
            });
        }

        #endregion

        #region//仓储数据 - 菜单

        private void ddMenu_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            this.ddMenu.SelectedValue = null;

            switch (e.Value.ToString())
            {
                case "Import":

                    this.tStores.SuspendLayout();
                    Operate.WareHouseConfig.List.UpdateStores_ByListAction(this.form, this.Stores, Operate.SystemConfig.ListAction.Import, null);
                    this.tStores.ResumeLayout();                    

                    break;

                case "Export":

                    if (this.Stores.Count > 0)
                    {
                        Operate.WareHouseConfig.List.UpdateStores_ByListAction(this.form, this.Stores, Operate.SystemConfig.ListAction.Export, null);
                    }

                    break;

                case "Clear":

                    if (this.Stores.Count > 0)
                    {
                        Operate.WareHouseConfig.List.UpdateStores_ByListAction(this.form, this.Stores, Operate.SystemConfig.ListAction.CleanUp, null);
                    }

                    break;
            }
        }

        #endregion

        #region//仓储数据 - 右键菜单

        private void tStores_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (this.Stores.Count == 0)
                {
                    return;
                }

                AntdUI.ContextMenuStrip.open(new AntdUI.ContextMenuStrip.Config(tStores, (item) =>
                {
                    List<DataInfo> diList = new List<DataInfo>();
                    foreach (DataInfo di in this.Stores)
                    {
                        if (di.IsCheck)
                        {
                            diList.Add(di);
                        }                        
                    }

                    switch (item.ID)
                    {
                        case "Top":

                            if (diList.Count > 0)
                            {
                                Operate.WareHouseConfig.List.UpdateStores_ByListAction(this.form, this.Stores, Operate.SystemConfig.ListAction.Top, diList);
                            }

                            break;

                        case "Up":

                            if (diList.Count > 0)
                            {
                                Operate.WareHouseConfig.List.UpdateStores_ByListAction(this.form, this.Stores, Operate.SystemConfig.ListAction.Up, diList);
                            }

                            break;

                        case "Down":

                            if (diList.Count > 0)
                            {
                                Operate.WareHouseConfig.List.UpdateStores_ByListAction(this.form, this.Stores, Operate.SystemConfig.ListAction.Down, diList);
                            }

                            break;

                        case "Bottom":

                            if (diList.Count > 0)
                            {
                                Operate.WareHouseConfig.List.UpdateStores_ByListAction(this.form, this.Stores, Operate.SystemConfig.ListAction.Bottom, diList);
                            }

                            break;

                        case "Export":

                            if (diList.Count > 0)
                            {
                                Operate.WareHouseConfig.List.UpdateStores_ByListAction(this.form, this.Stores, Operate.SystemConfig.ListAction.Export, diList);
                            }

                            break;

                        case "Copy":

                            if (diList.Count > 0)
                            {
                                Operate.WareHouseConfig.List.UpdateStores_ByListAction(this.form, this.Stores, Operate.SystemConfig.ListAction.Copy, diList);
                                this.tStores.ScrollBar.ValueY = tStores.ScrollBar.MaxY;
                            }

                            break;

                        case "Delete":

                            if (diList.Count > 0)
                            {
                                Operate.WareHouseConfig.List.UpdateStores_ByListAction(this.form, this.Stores, Operate.SystemConfig.ListAction.Delete, diList);
                            }

                            break;
                    }

                    this.tStores.SelectedIndex = -1;
                }, Operate.SystemConfig.GetCMS_List()));
            }
        }

        #endregion

        #region//显示选中的封包数据

        private void tStores_SelectIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int selectedIndex = this.tStores.SelectedIndex - 1;
                if (selectedIndex >= 0 && selectedIndex < this.Stores.Count)
                {
                    DynamicByteProvider dbp = new DynamicByteProvider(this.Stores[selectedIndex].PacketBuffer);
                    this.hbPacketData.ByteProvider = dbp;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, System.EventArgs e)
        {
            try
            {
                string WName = this.txtWName.Text.Trim();
                if (string.IsNullOrEmpty(WName))
                {
                    this.txtWName.Status = AntdUI.TType.Error;

                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "仓库名称不能为空", TType.Error)
                    {
                        LocalizationText = "WareHouseEdit.WName.Empty"
                    });

                    return;
                }

                if (!this.whiSelect.WName.Equals(WName))
                {
                    this.whiSelect.WName = WName;
                }                

                this.Dispose();
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(bSave_Click), ex);
            }            
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
