using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class RuleList : UserControl
    {
        private ServerInfo siSelect;
        private Form form;

        #region//窗体事件

        public RuleList(Form form, ServerInfo si)
        {
            InitializeComponent();

            this.form = form;
            this.siSelect = si;
        }

        private void RuleList_Load(object sender, System.EventArgs e)
        {
            this.InitMenu();
            this.InitTable_RuleList();
            this.Dark_Changed();
        }

        private void InitMenu()
        {
            this.ddMenu.Items.Clear();

            this.ddMenu.Items.AddRange(
                new AntdUI.SelectItem[]
                {
                    new AntdUI.SelectItem("新增规则")
                    {
                        Tag = "RuleList_Add",
                        LocalizationText = "WPCConfig.RuleList.Add",
                        IconSvg = "SendOutlined",
                    },
                    new AntdUI.SelectItem("清空所有规则")
                    {
                        Tag = "RuleList_Clear",
                        LocalizationText = "WPCConfig.RuleList.Clear",
                        IconSvg = "DeleteOutlined",
                    },
                });
        }

        private void InitTable_RuleList()
        {
            tRuleList.Columns = new AntdUI.ColumnCollection {
                new AntdUI.ColumnSwitch("IsEnable", "启用", AntdUI.ColumnAlign.Center)
                {
                    Width = "60",
                    Call = (value, record, i_row, i_col) =>
                    {
                        return value;
                    }
                }.SetFixed().SetLocalizationTitleID("Table.WPCConfig.RuleList.Column."),
                new AntdUI.Column("RType", "类型")
                {
                    Render = (value, record, rowIndex) =>
                    {
                        if (value is RuleType ruleType)
                        {
                            return Operate.WPCConfig.ServerList.GetRuleTypeDescription(ruleType);
                        }

                        return value?.ToString();
                    }
                }.SetLocalizationTitleID("Table.WPCConfig.RuleList.Column."),
                new AntdUI.Column("RArgument", "参数").SetLocalizationTitleID("Table.WPCConfig.RuleList.Column."),
                new AntdUI.Column("RAction", "动作").SetLocalizationTitleID("Table.WPCConfig.RuleList.Column."),
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
                }.SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.WPCConfig.RuleList.Column."),
            };

            this.tRuleList.Binding(this.siSelect.ServerRInfo);
        }

        public void Dark_Changed()
        {
            if (AntdUI.Config.IsDark)
            {
                this.tRuleList.BackColor = Operate.SystemConfig.Color_40;
                this.tRuleList.ColumnBack = Operate.SystemConfig.Color_40;
            }
            else
            {
                this.tRuleList.BackColor = Color.White;
                this.tRuleList.ColumnBack = null;
            }
        }

        #endregion

        #region//规则列表 - 菜单

        private void ddMenu_SelectedValueChanged(object sender, AntdUI.ObjectNEventArgs e)
        {
            this.ddMenu.SelectedValue = null;

            switch (e.Value.ToString())
            {
                case "RuleList_Add":

                    Operate.WPCConfig.ServerList.OpenRuleEdit(this.form, this.siSelect, null);

                    break;

                case "RuleList_Clear":

                    if (this.siSelect.ServerRInfo.Count > 0)
                    {
                        Operate.WPCConfig.ServerList.CleanUpRuleList_Dialog(this.form, siSelect);
                    }

                    break;
            }
        }

        private void tRuleList_CellButtonClick(object sender, AntdUI.TableButtonEventArgs e)
        {
            if (e.Record is RuleInfo ri)
            {
                switch (e.Btn.Id)
                {
                    case "bEdit":

                        Operate.WPCConfig.ServerList.OpenRuleEdit(this.form, this.siSelect, ri);

                        break;

                    case "bDelete":

                        List<RuleInfo> riList = new List<RuleInfo>
                        {
                            ri
                        };

                        Operate.WPCConfig.ServerList.UpdateRuleList_ByListAction(this.form, this.siSelect, Operate.SystemConfig.ListAction.Delete, riList);

                        break;
                }
            }
        }

        private void tRuleList_CellDoubleClick(object sender, AntdUI.TableClickEventArgs e)
        {
            if (e.Record is RuleInfo ri)
            {
                Operate.WPCConfig.ServerList.OpenRuleEdit(this.form, this.siSelect, ri);
            }
        }

        #endregion

        #region//规则列表 - 右键菜单

        private void tRuleList_CellClick(object sender, AntdUI.TableClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (this.siSelect.ServerRInfo.Count == 0)
                {
                    return;
                }

                AntdUI.ContextMenuStrip.open(new AntdUI.ContextMenuStrip.Config(tRuleList, (item) =>
                {
                    List<RuleInfo> riList = new List<RuleInfo>();

                    foreach (int SelectIndex in this.tRuleList.SelectedIndexs)
                    {
                        riList.Add(this.siSelect.ServerRInfo[SelectIndex - 1]);
                    }

                    switch (item.ID)
                    {
                        case "Top":

                            if (riList.Count > 0)
                            {
                                Operate.WPCConfig.ServerList.UpdateRuleList_ByListAction(this.form, this.siSelect, Operate.SystemConfig.ListAction.Top, riList);
                            }

                            break;

                        case "Up":

                            if (riList.Count > 0)
                            {
                                Operate.WPCConfig.ServerList.UpdateRuleList_ByListAction(this.form, this.siSelect, Operate.SystemConfig.ListAction.Up, riList);
                            }

                            break;

                        case "Down":

                            if (riList.Count > 0)
                            {
                                Operate.WPCConfig.ServerList.UpdateRuleList_ByListAction(this.form, this.siSelect, Operate.SystemConfig.ListAction.Down, riList);
                            }

                            break;

                        case "Bottom":

                            if (riList.Count > 0)
                            {
                                Operate.WPCConfig.ServerList.UpdateRuleList_ByListAction(this.form, this.siSelect, Operate.SystemConfig.ListAction.Bottom, riList);
                            }

                            break;

                        case "Delete":

                            if (riList.Count > 0)
                            {
                                Operate.WPCConfig.ServerList.UpdateRuleList_ByListAction(this.form, this.siSelect, Operate.SystemConfig.ListAction.Delete, riList);
                            }

                            break;
                    }

                    this.tRuleList.SelectedIndex = -1;
                }, Operate.WPCConfig.GetCMS_List()));
            }
        }

        #endregion

        #region//确定

        private void bSure_Click(object sender, System.EventArgs e)
        {
            this.Dispose();
        }

        #endregion
    }
}
