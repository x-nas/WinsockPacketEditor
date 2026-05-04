using AntdUI;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class WPCConfig : UserControl
    {
        private Form form;

        #region//窗体事件

        public WPCConfig(Form form)
        {
            this.form = form;
            InitializeComponent();
        }

        private void WPCConfig_Load(object sender, System.EventArgs e)
        {
            this.tabWPCConfig.SelectedIndex = 0;

            this.InitMenu();
            this.InitTable_ServerList();
            this.InitTable_NoticeList();
            this.Dark_Changed();
        }

        private void tabWPCConfig_SelectedIndexChanged(object sender, AntdUI.IntEventArgs e)
        {
            this.InitMenu();
        }

        private void InitMenu()
        {
            this.ddMenu.Items.Clear();

            if (this.tabWPCConfig.SelectedTab == this.tpServerList)
            {
                this.ddMenu.Items.AddRange(new AntdUI.SelectItem[]
                {
                    new AntdUI.SelectItem("新增服务器")
                    {
                        Tag = "ServerList_Add",
                        LocalizationText = "WPCConfig.ServerList.Add",
                        IconSvg = "CloudServerOutlined",
                    },
                    new AntdUI.SelectItem("清空所有服务器")
                    {
                        Tag = "ServerList_Clear",
                        LocalizationText = "WPCConfig.ServerList.Clear",
                        IconSvg = "DeleteOutlined",
                    },
                });
            }
            else if (this.tabWPCConfig.SelectedTab == this.tpNoticeList)
            {
                this.ddMenu.Items.AddRange(new AntdUI.SelectItem[]
                {
                    new AntdUI.SelectItem("新增公告")
                    {
                        Tag = "NoticeList_Add",
                        LocalizationText = "WPCConfig.NoticeList.Add",
                        IconSvg = "NotificationOutlined",
                    },
                    new AntdUI.SelectItem("清空所有公告")
                    {
                        Tag = "NoticeList_Clear",
                        LocalizationText = "WPCConfig.NoticeList.Clear",
                        IconSvg = "DeleteOutlined",
                    },
                });
            }
        }

        private void InitTable_ServerList()
        {
            this.tServerList.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.ColumnSwitch("IsEnable", "启用", AntdUI.ColumnAlign.Center)
                {
                    Width = "60",
                    Call = (value, record, i_row, i_col) =>
                    {
                        return value;
                    }
                }.SetFixed().SetLocalizationTitleID("Table.WPCConfig.ServerList.Column."),
                new AntdUI.Column("ServerName", "服务器名称").SetLocalizationTitleID("Table.WPCConfig.ServerList.Column."),
                new AntdUI.Column("ServerIP", "服务器IP").SetLocalizationTitleID("Table.WPCConfig.ServerList.Column."),
                new AntdUI.Column("ServerPort", "服务器端口", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.WPCConfig.ServerList.Column."),
                new AntdUI.Column("ForgotURL", "找回密码地址", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.WPCConfig.ServerList.Column."),
                new AntdUI.Column("RegisterURL", "立即注册地址", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.WPCConfig.ServerList.Column."),
                new AntdUI.Column("VerifyURL", "验证地址", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.WPCConfig.ServerList.Column."),
                new AntdUI.Column("CellLinks", "操作")
                {
                    Render = (value, record, rowindex)=>
                    {
                        return new AntdUI.CellLink[]
                        {
                            new AntdUI.CellButton("bEdit", null, AntdUI.TTypeMini.Primary).SetIcon("EditOutlined"),
                            new AntdUI.CellButton("bRule", null, AntdUI.TTypeMini.Warn).SetIcon("SendOutlined"),
                            new AntdUI.CellButton("bDelete", null, AntdUI.TTypeMini.Error).SetIcon("CloseOutlined"),
                        };
                    },
                }.SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.WPCConfig.ServerList.Column."),
            };

            this.tServerList.Binding(Operate.WPCConfig.ServerList.lstServerInfo);
        }

        private void InitTable_NoticeList()
        {
            this.tNoticeList.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("NoticeType", "类型", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(int.TryParse(value.ToString(), out int NoticeType))
                        {
                            CellTag cellTag = new CellTag();
                            cellTag.BorderWidth = 0;

                            if(NoticeType == 1)
                            {
                                cellTag.Text = AntdUI.Localization.Get("WPCConfig.NoticeList.NoticeType_1", " 活动情报 ");
                                cellTag.Fore = Color.FromArgb(56, 189, 248);
                                cellTag.Back = Color.FromArgb(51, 56, 189, 248);                                
                            }
                            else if(NoticeType == 2)
                            {
                                cellTag.Text = AntdUI.Localization.Get("WPCConfig.NoticeList.NoticeType_2", " 维护说明 ");
                                cellTag.Fore = Color.FromArgb(251, 191, 36);
                                cellTag.Back = Color.FromArgb(51, 251, 191, 36);
                            }
                            else if(NoticeType == 3)
                            {
                                cellTag.Text = AntdUI.Localization.Get("WPCConfig.NoticeList.NoticeType_3", " 电竞赛事 ");
                                cellTag.Fore = Color.FromArgb(52, 211, 153);
                                cellTag.Back = Color.FromArgb(51, 52, 211, 153);
                            }
                            else if(NoticeType == 4)
                            {
                                cellTag.Text = AntdUI.Localization.Get("WPCConfig.NoticeList.NoticeType_4", " 限时商城 ");
                                cellTag.Fore = Color.FromArgb(192, 132, 252);
                                cellTag.Back = Color.FromArgb(51, 192, 132, 252);
                            }
                            else if(NoticeType == 5)
                            {
                                cellTag.Text = AntdUI.Localization.Get("WPCConfig.NoticeList.NoticeType_5", " 玩家社区 ");
                                cellTag.Fore = Color.FromArgb(56, 189, 248);
                                cellTag.Back = Color.FromArgb(51, 56, 189, 248);
                            }

                            return cellTag;
                        }

                        return null;
                    },
                }.SetLocalizationTitleID("Table.WPCConfig.NoticeList.Column."),
                new AntdUI.Column("NoticeTitle", "标题").SetLocalizationTitleID("Table.WPCConfig.NoticeList.Column."),                
                new AntdUI.Column("NoticeMore", "更多详情链接", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.WPCConfig.NoticeList.Column."),
                new AntdUI.Column("NoticeTime", "发布时间", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.WPCConfig.NoticeList.Column."),
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
                }.SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.WPCConfig.NoticeList.Column."),
            };

            this.tNoticeList.Binding(Operate.WPCConfig.NoticeList.lstNoticeInfo);
        }

        public void Dark_Changed()
        {
            if (AntdUI.Config.IsDark)
            {
                this.tServerList.BackColor = Operate.SystemConfig.Color_40;
                this.tServerList.ColumnBack = Operate.SystemConfig.Color_40;
            }
            else
            {
                this.tServerList.BackColor = Color.White;
                this.tServerList.ColumnBack = null;
            }
        }

        #endregion

        #region//列表 - 菜单

        private void ddMenu_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            this.ddMenu.SelectedValue = null;

            switch (e.Value.ToString())
            {
                case "ServerList_Add":

                    Operate.WPCConfig.ServerList.OpenServerEdit(this.form, null);

                    break;

                case "NoticeList_Add":

                    Operate.WPCConfig.NoticeList.OpenNoticeEdit(this.form, null);

                    break;

                case "ServerList_Clear":

                    if (Operate.WPCConfig.ServerList.lstServerInfo.Count > 0)
                    {
                        Operate.WPCConfig.ServerList.CleanUpServerList_Dialog(this.form);
                    }

                    break;

                case "NoticeList_Clear":

                    if (Operate.WPCConfig.NoticeList.lstNoticeInfo.Count > 0)
                    {
                        Operate.WPCConfig.NoticeList.CleanUpNoticeList_Dialog(this.form);
                    }

                    break;
            }
        }

        private void tServerList_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            if (e.Record is ServerInfo si)
            {
                switch (e.Btn.Id)
                {
                    case "bEdit":

                        Operate.WPCConfig.ServerList.OpenServerEdit(this.form, si);

                        break;

                    case "bRule":

                        Operate.WPCConfig.ServerList.OpenRuleList(this.form, si);

                        break;

                    case "bDelete":

                        List<ServerInfo> siList = new List<ServerInfo>
                        {
                            si
                        };

                        Operate.WPCConfig.ServerList.UpdateServerList_ByListAction(this.form, Operate.SystemConfig.ListAction.Delete, siList);

                        break;
                }
            }
        }

        private void tNoticeList_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            if (e.Record is NoticeInfo ni)
            {
                switch (e.Btn.Id)
                {
                    case "bEdit":

                        Operate.WPCConfig.NoticeList.OpenNoticeEdit(this.form, ni);

                        break;

                    case "bDelete":

                        List<NoticeInfo> niList = new List<NoticeInfo>
                        {
                            ni
                        };

                        Operate.WPCConfig.NoticeList.UpdateNoticeList_ByListAction(this.form, Operate.SystemConfig.ListAction.Delete, niList);

                        break;
                }
            }
        }

        private void tServerList_CellDoubleClick(object sender, TableClickEventArgs e)
        {
            if (e.Record is ServerInfo si)
            {
                Operate.WPCConfig.ServerList.OpenServerEdit(this.form, si);
            }
        }

        private void tNoticeList_CellDoubleClick(object sender, TableClickEventArgs e)
        {
            if (e.Record is NoticeInfo ni)
            {
                Operate.WPCConfig.NoticeList.OpenNoticeEdit(this.form, ni);
            }
        }

        #endregion

        #region//服务器列表 - 右键菜单

        private void tServerList_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Operate.WPCConfig.ServerList.lstServerInfo.Count == 0)
                {
                    return;
                }

                AntdUI.ContextMenuStrip.open(new AntdUI.ContextMenuStrip.Config(tServerList, (item) =>
                {
                    List<ServerInfo> siList = new List<ServerInfo>();

                    foreach (int SelectIndex in this.tServerList.SelectedIndexs)
                    {
                        siList.Add(Operate.WPCConfig.ServerList.lstServerInfo[SelectIndex - 1]);
                    }

                    switch (item.ID)
                    {
                        case "Top":

                            if (siList.Count > 0)
                            {
                                Operate.WPCConfig.ServerList.UpdateServerList_ByListAction(this.form, Operate.SystemConfig.ListAction.Top, siList);
                            }

                            break;

                        case "Up":

                            if (siList.Count > 0)
                            {
                                Operate.WPCConfig.ServerList.UpdateServerList_ByListAction(this.form, Operate.SystemConfig.ListAction.Up, siList);
                            }

                            break;

                        case "Down":

                            if (siList.Count > 0)
                            {
                                Operate.WPCConfig.ServerList.UpdateServerList_ByListAction(this.form, Operate.SystemConfig.ListAction.Down, siList);
                            }

                            break;

                        case "Bottom":

                            if (siList.Count > 0)
                            {
                                Operate.WPCConfig.ServerList.UpdateServerList_ByListAction(this.form, Operate.SystemConfig.ListAction.Bottom, siList);
                            }

                            break;                        

                        case "Delete":

                            if (siList.Count > 0)
                            {
                                Operate.WPCConfig.ServerList.UpdateServerList_ByListAction(this.form, Operate.SystemConfig.ListAction.Delete, siList);
                            }

                            break;
                    }

                    this.tServerList.SelectedIndex = -1;
                }, Operate.WPCConfig.GetCMS_List()));
            }
        }

        private void tNoticeList_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Operate.WPCConfig.NoticeList.lstNoticeInfo.Count == 0)
                {
                    return;
                }

                AntdUI.ContextMenuStrip.open(new AntdUI.ContextMenuStrip.Config(tNoticeList, (item) =>
                {
                    List<NoticeInfo> niList = new List<NoticeInfo>();

                    foreach (int SelectIndex in this.tNoticeList.SelectedIndexs)
                    {
                        niList.Add(Operate.WPCConfig.NoticeList.lstNoticeInfo[SelectIndex - 1]);
                    }

                    switch (item.ID)
                    {
                        case "Top":

                            if (niList.Count > 0)
                            {
                                Operate.WPCConfig.NoticeList.UpdateNoticeList_ByListAction(this.form, Operate.SystemConfig.ListAction.Top, niList);
                            }

                            break;

                        case "Up":

                            if (niList.Count > 0)
                            {
                                Operate.WPCConfig.NoticeList.UpdateNoticeList_ByListAction(this.form, Operate.SystemConfig.ListAction.Up, niList);
                            }

                            break;

                        case "Down":

                            if (niList.Count > 0)
                            {
                                Operate.WPCConfig.NoticeList.UpdateNoticeList_ByListAction(this.form, Operate.SystemConfig.ListAction.Down, niList);
                            }

                            break;

                        case "Bottom":

                            if (niList.Count > 0)
                            {
                                Operate.WPCConfig.NoticeList.UpdateNoticeList_ByListAction(this.form, Operate.SystemConfig.ListAction.Bottom, niList);
                            }

                            break;

                        case "Delete":

                            if (niList.Count > 0)
                            {
                                Operate.WPCConfig.NoticeList.UpdateNoticeList_ByListAction(this.form, Operate.SystemConfig.ListAction.Delete, niList);
                            }

                            break;
                    }

                    this.tNoticeList.SelectedIndex = -1;
                }, Operate.WPCConfig.GetCMS_List()));
            }
        }

        #endregion
    }
}
