using AntdUI;
using System;
using System.ComponentModel;
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
            this.tabClientList.SelectTab(0);

            this.InitTable_AuthList();
            this.InitTable_ProxyLog();
            this.Dark_Changed();
        }

        private void InitTable_AuthList()
        {
            tAuthList.Columns = new AntdUI.ColumnCollection {
                new AntdUI.Column("AuthTime", "认证时间").SetSortOrder().SetLocalizationTitleID("Table.AuthList.Column."),
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
                                PrefixSvg = Operate.SystemConfig.GetSvgByLocation(ai.IPLocation),
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

        private void InitTable_ProxyLog()
        {
            tProxyLog.Columns = new AntdUI.ColumnCollection {
                new AntdUI.Column("LogTime", "时间戳")
                {
                    Render = (value, record, rowindex)=>
                    {
                        return ((DateTime)value).ToString("HH:mm:ss");
                    },
                }.SetLocalizationTitleID("Table.ProxyLog.Column."),
                new AntdUI.Column("UserName", "账号", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.ProxyLog.Column."),
                new AntdUI.Column("LoginIP", "IP地址")
                {
                    Render = (value, record, rowindex)=>
                    {
                        return new CellText(value?.ToString() ?? string.Empty)
                        {
                            PrefixSvg = Operate.SystemConfig.GetSvgByLocation(value.ToString()),
                            IconRatio = 1.0F
                        };
                    },
                }.SetLocalizationTitleID("Table.ProxyLog.Column."),
                new AntdUI.Column("LogContent", "日志内容").SetLocalizationTitleID("Table.ProxyLog.Column."),
            };

            this.tProxyLog.ColumnFont = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.tProxyLog.DataSource = Operate.LogConfig.List.lstProxyLogInfo;
        }

        public void Dark_Changed()
        {
            if (AntdUI.Config.IsDark)
            {
                this.tAuthList.ColumnBack = 
                    this.tProxyLog.ColumnBack = 
                    Color.FromArgb(35, 35, 35);
            }
            else
            {
                this.tAuthList.ColumnBack = 
                    this.tProxyLog.ColumnBack = null;
            }
        }

        public void RefreshClientList()
        {
            this.tAuthList.DataSource = Operate.ProxyConfig.Account.cdAuthInfo.Values;
            this.tProxyLog.Refresh();

            this.lAuthCount_Value.Text = Operate.ProxyConfig.Account.cdAuthInfo.Count.ToString();
            this.lLinksCount_Value.Text = Operate.ProxyConfig.Account.GetLinksCount_FromAuthList().ToString();
            this.lDevicesCount_Value.Text = Operate.ProxyConfig.Account.GetDevicesCount_FromAuthList().ToString();
        }

        public int GetClientNumber()
        {
            return this.treeClientList.Items.Count();
        }

        public void CleanUp_ProxyLogList()
        {
            Operate.LogConfig.List.ClearProxyLogList();
        }

        #endregion

        #region//显示客户端列表（异步）

        public void ShowClientList()
        {
            if (!this.bgwClientList.IsBusy)
            {
                this.treeClientList.PauseLayout = true;
                this.bgwClientList.RunWorkerAsync();
            }
        }

        private void bgwClientList_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var peList = Operate.ProxyConfig.List.lstProxyTCP.ToList();
                if (peList == null || peList.Count == 0)
                {
                    return;
                }

                foreach (ProxyTCP pe in peList)
                {
                    if (pe == null)
                    {
                        continue;
                    }

                    if (pe.TCP_Client == null)
                    {
                        Operate.ProxyConfig.List.lstProxyTCP.Remove(pe);
                        pe?.Dispose();
                        continue;
                    }

                    #region//更新客户端链接

                    if (pe.CommandType != Operate.ProxyConfig.Proxy.CommandType.Bind)
                    {
                        string ClientIP = Operate.ProxyConfig.Proxy.GetClientIPAddress(pe);
                        string ClientUserName = Operate.ProxyConfig.Account.GetUserName_ByAccountID(pe.AID);
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

                        string sChildName = pe.TCP_Client.Address;
                        if (string.IsNullOrEmpty(sChildName))
                        {
                            return;
                        }

                        AntdUI.TreeItem tiChild = Operate.SystemConfig.FindNodeByName(this.treeClientList, sChildName);
                        if (tiChild == null)
                        {
                            tiChild = new TreeItem(sChildName);
                            switch (pe.DomainType)
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

                                    break;
                            }
                            tiRoot.Sub.Add(tiChild);
                        }
                    }

                    #endregion

                    if (pe.TCP_Client.Socket == null)
                    {
                        #region//移除关闭的客户端链接                    

                        string ClientIP = Operate.ProxyConfig.Proxy.GetClientIPAddress(pe);
                        string ClientUserName = Operate.ProxyConfig.Account.GetUserName_ByAccountID(pe.AID);

                        if (string.IsNullOrEmpty(ClientUserName))
                        {
                            TreeItem tiChild = Operate.SystemConfig.FindNodeByName(this.treeClientList, pe.TCP_Client.Address);
                            if (tiChild != null)
                            {
                                this.treeClientList.Items.Remove(tiChild);
                            }
                            Operate.ProxyConfig.List.lstProxyTCP.Remove(pe);
                            pe?.Dispose();
                        }
                        else
                        {
                            string sRootName = Operate.ProxyConfig.Proxy.GetClientListName(ClientIP, ClientUserName);
                            if (string.IsNullOrEmpty(sRootName))
                            {
                                return;
                            }

                            TreeItem tiRoot = Operate.SystemConfig.FindNodeByName(this.treeClientList, sRootName);
                            if (tiRoot == null)
                            {
                                return;
                            }

                            TreeItem tiChild = Operate.SystemConfig.FindNodeByName(tiRoot.Sub, pe.TCP_Client.Address);
                            if (tiChild != null)
                            {
                                tiRoot.Sub.Remove(tiChild);
                            }

                            if (tiRoot.Sub.Count == 0)
                            {
                                this.treeClientList.Items.Remove(tiRoot);
                                Operate.ProxyConfig.Account.DeleteProxyAuthInfo_ByAIDAndIP(pe.AID, ClientIP);

                                if (pe.AID != null && pe.AID != Guid.Empty)
                                {
                                    Operate.ProxyConfig.Account.SetOnline_ByAccountID(pe.AID, false);
                                }
                            }

                            Operate.ProxyConfig.List.lstProxyTCP.Remove(pe);
                            pe?.Dispose();
                        }

                        #endregion
                    }
                }

                foreach (AuthInfo ai in Operate.ProxyConfig.Account.cdAuthInfo.Values)
                {
                    string clientIP = ai.AuthIP.ToString();
                    ai.LinksNumber = Operate.ProxyConfig.Account.GetLinksNumber_ByAccountID(ai.AID, clientIP, this.treeClientList);
                    ai.DevicesNumber = Operate.ProxyConfig.Account.GetDevicesNumber_ByAccountID(ai.AID);

                    var key = (ai.AID, ai.AuthIP);
                    Operate.ProxyConfig.Account.cdAuthInfo.AddOrUpdate(
                        key,
                        ai,
                        (_, existing) => ai
                    );
                }

                Operate.ProxyConfig.Proxy.CheckUDPTimeOut();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bgwClientList_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.treeClientList.PauseLayout = false;
                this.treeClientList.Refresh();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion        
    }
}
