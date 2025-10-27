using AntdUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class ClientList : UserControl
    {
        private Form form = null;

        #region//窗体事件

        public ClientList(Form form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void ClientList_Load(object sender, EventArgs e)
        {
            this.InitTable_AuthList();            
            this.Dark_Changed();
        }

        private void InitTable_AuthList()
        {
            tAuthList.Columns = new AntdUI.ColumnCollection {
                new AntdUI.Column("AuthTime", "认证时间")
                {
                    Render = (value, record, rowindex)=>
                    {
                        return ((DateTime)value).ToString("HH:mm:ss");
                    },
                }.SetSortOrder().SetLocalizationTitleID("Table.AuthList.Column."),
                new AntdUI.Column("AID", "账号")
                {
                    Render = (value, record, rowindex)=>
                    {
                        return Operate.ProxyConfig.Account.GetUserName_ByAccountID((Guid)value);
                    },
                }.SetSortOrder().SetLocalizationTitleID("Table.AuthList.Column."),
                new AntdUI.Column("AuthIP", "IP地址")
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is AuthInfo ai)
                        {
                            return new CellText(value?.ToString() ?? string.Empty)
                            {
                                Prefix = Operate.SystemConfig.GetFlagByLocation(ai.IPLocation),
                                IconRatio = 1.0F
                            };
                        }

                        return value;
                    },
                }.SetLocalizationTitleID("Table.AuthList.Column."),
                new AntdUI.Column("IPLocation", "所属地").SetLocalizationTitleID("Table.AuthList.Column."),
                new AntdUI.Column("LinksNumber", "链接数", AntdUI.ColumnAlign.Center).SetSortOrder().SetLocalizationTitleID("Table.AuthList.Column."),
                new AntdUI.Column("DevicesNumber", "设备数", AntdUI.ColumnAlign.Center).SetSortOrder().SetLocalizationTitleID("Table.AuthList.Column."),
                new AntdUI.Column("AuthResult", "认证结果", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        if((bool)value)
                        {
                            return new CellTag("通过", TTypeMini.Success);
                        }
                        else
                        {
                            return new CellTag("失败", TTypeMini.Error);
                        }
                    },
                }.SetLocalizationTitleID("Table.AuthList.Column."),
            };

            this.tAuthList.ColumnFont = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.tAuthList.Binding(Operate.ProxyConfig.Account.lstAuthInfo);
        }        

        public void Dark_Changed()
        {
            if (AntdUI.Config.IsDark)
            {
                this.treeClientList.BackColor = Operate.SystemConfig.Color_40;

                this.tAuthList.BackColor = Operate.SystemConfig.Color_40;
                this.tAuthList.ColumnBack = Operate.SystemConfig.Color_40;
            }
            else
            {
                this.treeClientList.BackColor = Color.White;

                this.tAuthList.BackColor = Color.White;
                this.tAuthList.ColumnBack = null;                    
            }
        }

        #endregion

        #region//计时器

        private async void timerClientList_Tick(object sender, EventArgs e)
        {
            this.timerClientList.Stop();

            try
            {
                if (Operate.ProxyConfig.Proxy.ProxyServer == null || Operate.ProxyConfig.Proxy.ProxyServer.SessionCount == 0)
                {
                    this.treeClientList.Items.Clear();
                    Operate.ProxyConfig.Account.ClearAuthInfo();

                    return;
                }

                this.tAuthList.PauseLayout = true;

                this.UpdateClientList();
                await Operate.ProxyConfig.Account.UpdateAuthList();
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(timerClientList_Tick), ex.Message);
            }
            finally
            {
                Operate.ProxyConfig.List.ClientNumber = this.treeClientList.Items.Count();
                this.tAuthList.PauseLayout = false;
                this.timerClientList.Start();
            }
        }

        #endregion

        #region//更新客户端列表

        private void UpdateClientList()
        {
            try
            {
                this.treeClientList.PauseLayout = true;

                foreach (var rootItem in treeClientList.Items)
                {
                    rootItem.Sub.Clear();
                }

                var sessions = Operate.ProxyConfig.Proxy.ProxyServer.GetAllSessions();
                var SessionList = sessions?.ToList() ?? new List<ProxySession>();

                foreach (ProxySession Session in SessionList)
                {
                    if (Session.CommandType != Operate.ProxyConfig.Proxy.CommandType.Bind)
                    {
                        string RootName = Session.ClientIP;
                        string RootSubTitle = Operate.ProxyConfig.Account.GetUserName_ByAccountID(Session.AID);

                        if (string.IsNullOrEmpty(RootName) || string.IsNullOrEmpty(RootSubTitle))
                        {
                            continue;
                        }

                        AntdUI.TreeItem tiRoot = Operate.SystemConfig.FindNodeByName(this.treeClientList, RootName, RootSubTitle);
                        if (tiRoot == null)
                        {
                            tiRoot = new TreeItem(RootName)
                            {
                                IconSvg = "DesktopOutlined",
                                SubTitle = RootSubTitle,
                            };

                            this.treeClientList.Items.Add(tiRoot);
                        }

                        string sChildName = Session.ClientAddress;
                        if (string.IsNullOrEmpty(sChildName))
                        {
                            continue;
                        }

                        string ChildSubTitle = Session.ClientPort.ToString();
                        AntdUI.TreeItem tiChild = Operate.SystemConfig.FindNodeByName(this.treeClientList, sChildName, ChildSubTitle);

                        if (tiChild == null)
                        {
                            tiChild = new TreeItem(sChildName);
                            tiChild.SubTitle = ChildSubTitle;

                            switch (Session.DomainType)
                            {
                                case Operate.ProxyConfig.Proxy.DomainType.Http:
                                    tiChild.IconSvg = "IeOutlined";
                                    break;

                                case Operate.ProxyConfig.Proxy.DomainType.Https:
                                    tiChild.IconSvg = "LockOutlined";
                                    break;

                                case Operate.ProxyConfig.Proxy.DomainType.Socket:
                                    tiChild.IconSvg = "ApiOutlined";
                                    break;

                                case Operate.ProxyConfig.Proxy.DomainType.External:
                                    tiChild.IconSvg = "CloudUploadOutlined";
                                    break;
                            }

                            tiRoot.Sub.Add(tiChild);
                        }
                    }
                }

                var TreeItemToRemove = new List<AntdUI.TreeItem>();
                foreach (var rootItem in treeClientList.Items)
                {
                    if (rootItem.Sub.Count == 0)
                    {
                        TreeItemToRemove.Add(rootItem);
                    }
                }

                foreach (var item in TreeItemToRemove)
                {
                    if (item != null)
                    {
                        item.Remove();
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(UpdateClientList), ex.Message);
            }
            finally
            {
                this.treeClientList.PauseLayout = false;
            }
        }

        #endregion

        #region//认证列表 - 右键菜单

        private void AddToWhiteList_ByDateTime(string IPAddress, bool IsExpiry, DateTime ExpiryTime)
        {
            Operate.ProxyConfig.Proxy.AddToWhiteList(IPAddress, false, Operate.SystemConfig.MaxDateTime, DateTime.Now);

            AntdUI.Message.open(new AntdUI.Message.Config(this.form, IPAddress + " " + "已加入到白名单", TType.Success)
            {
                LocalizationText = IPAddress + " " + "FireWallSetting.WhiteList.Add"
            });
        }

        private void AddToBlackList_ByDateTime(string IPAddress, bool IsExpiry, DateTime ExpiryTime)
        {
            Operate.ProxyConfig.Proxy.AddToBlackList(IPAddress, IsExpiry, ExpiryTime, DateTime.Now);

            AntdUI.Message.open(new AntdUI.Message.Config(this.form, IPAddress + " " + "已加入到黑名单", TType.Success)
            {
                LocalizationText = IPAddress + " " + "FireWallSetting.BlackList.Add"
            });
        }

        private void tAuthList_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Operate.ProxyConfig.Account.lstAuthInfo.Count == 0)
                {
                    return;
                }

                int SelectedIndex = this.tAuthList.SelectedIndex;
                if (SelectedIndex == -1 || SelectedIndex > Operate.ProxyConfig.Account.lstAuthInfo.Count)
                {
                    return;
                }

                string AuthIP = Operate.ProxyConfig.Account.lstAuthInfo.ElementAt(SelectedIndex - 1).AuthIP;

                AntdUI.ContextMenuStrip.open(tAuthList, item =>
                {
                    switch (item.ID)
                    {
                        case "WhiteList_Permanent":

                            this.AddToWhiteList_ByDateTime(AuthIP, false, Operate.SystemConfig.MaxDateTime);

                            break;

                        case "BlackList_1Hour":

                            this.AddToBlackList_ByDateTime(AuthIP, true, DateTime.Now.AddHours(1));

                            break;

                        case "BlackList_1Day":

                            this.AddToBlackList_ByDateTime(AuthIP, true, DateTime.Now.AddDays(1));

                            break;

                        case "BlackList_30Day":

                            this.AddToBlackList_ByDateTime(AuthIP, true, DateTime.Now.AddDays(30));

                            break;

                        case "BlackList_Permanent":

                            this.AddToBlackList_ByDateTime(AuthIP, false, Operate.SystemConfig.MaxDateTime);

                            break;
                    }
                }, Operate.ProxyConfig.Account.GetCMS_AuthList());
            }
        }

        #endregion        
    }
}
