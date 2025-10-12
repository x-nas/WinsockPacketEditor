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
        #region//窗体事件

        public ClientList()
        {
            InitializeComponent();
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

        #region//显示客户端列表

        public void RefreshClientList()
        {
            try
            {
                if (Operate.ProxyConfig.Proxy.ProxyServer == null)
                {
                    return;
                }

                this.treeClientList.PauseLayout = true;
                this.tAuthList.PauseLayout = true;

                #region //更新客户端列表

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

                        if (string.IsNullOrEmpty(RootName))
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
                    item.Remove();
                }

                #endregion

                #region//更新认证列表

                var AuthInfoToRemove = new List<(Guid AID, string AuthIP)>();
                var accountStatus = new Dictionary<Guid, bool>();

                foreach (AuthInfo ai in Operate.ProxyConfig.Account.cdAuthInfo.Values.ToList())
                {
                    if (ai == null) continue;

                    string clientIP = ai.AuthIP?.ToString() ?? string.Empty;
                    int linksNumber = Operate.ProxyConfig.Account.GetLinksNumber_ByAccountID(ai.AID, clientIP, this.treeClientList);
                    int devicesNumber = Operate.ProxyConfig.Account.GetDevicesNumber_ByAccountID(ai.AID);

                    var key = (ai.AID, ai.AuthIP);
                    if (Operate.ProxyConfig.Account.cdAuthInfo.TryGetValue(key, out var existingAi))
                    {
                        existingAi.LinksNumber = linksNumber;
                        existingAi.DevicesNumber = devicesNumber;

                        Operate.ProxyConfig.Account.cdAuthInfo.TryUpdate(key, existingAi, existingAi);
                    }

                    if (linksNumber == 0)
                    {
                        AuthInfoToRemove.Add((ai.AID, ai.AuthIP));
                    }

                    if (!accountStatus.ContainsKey(ai.AID))
                    {
                        accountStatus[ai.AID] = false;
                    }

                    if (linksNumber > 0)
                    {
                        accountStatus[ai.AID] = true;
                    }
                }

                foreach (var item in AuthInfoToRemove)
                {
                    Operate.ProxyConfig.Account.DeleteProxyAuthInfo_ByAIDAndIP(item.AID, item.AuthIP);
                }

                foreach (var account in accountStatus)
                {
                    Operate.ProxyConfig.Account.SetOnline_ByAccountID(account.Key, account.Value);
                }

                #endregion                
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(RefreshClientList), ex.Message);
            }
            finally
            {
                this.treeClientList.PauseLayout = false;
                Operate.ProxyConfig.List.ClientNumber = this.treeClientList.Items.Count();

                this.tAuthList.DataSource = Operate.ProxyConfig.Account.cdAuthInfo.Values;
                this.tAuthList.PauseLayout = false;                
            }
        }

        #endregion

        #region//显示认证列表

        public void RefreshAuthList()
        {
            this.tAuthList.DataSource = Operate.ProxyConfig.Account.cdAuthInfo.Values;
        }

        #endregion
    }
}
