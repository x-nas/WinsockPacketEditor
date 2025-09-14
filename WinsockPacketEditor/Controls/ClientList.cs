using AntdUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
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
                }.SetLocalizationTitleID("Table.AuthList.Column."),
                new AntdUI.Column("AID", "账号", AntdUI.ColumnAlign.Center)
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

            this.tAuthList.ColumnFont = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.tAuthList.DataSource = Operate.ProxyConfig.Account.cdAuthInfo.Values;
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

        public int GetClientNumber()
        {
            return this.treeClientList.Items.Count();
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

                #region//更新客户端列表

                this.treeClientList.PauseLayout = true;

                foreach (var rootItem in treeClientList.Items)
                {
                    rootItem.Sub.Clear();
                }

                var sessions = Operate.ProxyConfig.Proxy.ProxyServer.GetAllSessions();
                var SessionList = sessions?.ToList() ?? new List<ProxySession>();

                foreach (ProxySession Session in SessionList)
                {
                    #region//更新客户端链接

                    if (Session.CommandType != Operate.ProxyConfig.Proxy.CommandType.Bind)
                    {
                        string ClientIP = Session.ClientIP;
                        string ClientUserName = Operate.ProxyConfig.Account.GetUserName_ByAccountID(Session.AID);
                        string sRootName = Operate.ProxyConfig.Proxy.GetClientListName(ClientIP, ClientUserName);

                        if (string.IsNullOrEmpty(sRootName))
                        {
                            return;
                        }

                        AntdUI.TreeItem tiRoot = Operate.SystemConfig.FindNodeByName(this.treeClientList, sRootName);
                        if (tiRoot == null)
                        {
                            tiRoot = new TreeItem(sRootName)
                            {
                                IconSvg = "DesktopOutlined",
                            };
                            this.treeClientList.Items.Add(tiRoot);
                        }

                        string sChildName = Session.ClientAddress;
                        if (string.IsNullOrEmpty(sChildName))
                        {
                            return;
                        }

                        AntdUI.TreeItem tiChild = Operate.SystemConfig.FindNodeByName(this.treeClientList, sChildName);
                        if (tiChild == null)
                        {
                            tiChild = new TreeItem(sChildName);
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

                    #endregion
                }

                foreach (var rootItem in treeClientList.Items)
                {
                    if (rootItem.Sub.Count == 0)
                    {
                        rootItem.Remove();
                    }
                }

                this.treeClientList.PauseLayout = false;

                #endregion

                #region//更新认证列表

                foreach (AuthInfo ai in Operate.ProxyConfig.Account.cdAuthInfo.Values)
                {
                    string clientIP = ai.AuthIP.ToString();
                    ai.LinksNumber = Operate.ProxyConfig.Account.GetLinksNumber_ByAccountID(ai.AID, clientIP, this.treeClientList);
                    ai.DevicesNumber = Operate.ProxyConfig.Account.GetDevicesNumber_ByAccountID(ai.AID);

                    var keyAdd = (ai.AID, ai.AuthIP);
                    Operate.ProxyConfig.Account.cdAuthInfo.TryUpdate(keyAdd, ai, ai);
                }

                foreach (AuthInfo ai in Operate.ProxyConfig.Account.cdAuthInfo.Values)
                {
                    if (ai.LinksNumber == 0)
                    {
                        Operate.ProxyConfig.Account.DeleteProxyAuthInfo_ByAIDAndIP(ai.AID, ai.AuthIP);

                        int count = Operate.ProxyConfig.Account.cdAuthInfo.Count(kv => kv.Key.AID == ai.AID);
                        if (count == 0)
                        {
                            Operate.ProxyConfig.Account.SetOnline_ByAccountID(ai.AID, false);
                        }
                    }
                }

                #endregion                
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
            finally
            {
                this.treeClientList.PauseLayout = false;
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
