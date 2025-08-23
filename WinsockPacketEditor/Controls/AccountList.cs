using AntdUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class AccountList : UserControl
    {
        private Form form;
        public BindingList<AccountInfo> lstAccount;

        #region//窗体事件

        public AccountList(Form _form)
        {
            InitializeComponent();
            this.form = _form;
        }

        private void AccountList_Load(object sender, EventArgs e)
        {
            this.InitTable_AccountList();
        }

        private void InitTable_AccountList()
        {
            tAccountList.Columns = new AntdUI.ColumnCollection {
                new AntdUI.ColumnCheck("IsCheck").SetFixed(),
                new AntdUI.Column("", "序号", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return ((this.pAccountList.Current - 1) * this.pAccountList.PageSize + rowindex + 1);
                    },
                }.SetFixed().SetLocalizationTitleID("Table.AccountList.Column.ID"),
                new AntdUI.Column("UserName", "用户名").SetSortOrder().SetLocalizationTitleID("Table.AccountList.Column."),
                new AntdUI.Column("IsOnLine", "状态")
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is AccountInfo ai)
                        {
                            if(ai.IsOnLine)
                            {
                                return new AntdUI.CellBadge(AntdUI.TState.Success, AntdUI.Localization.Get("Online", "在线"));
                            }
                            else
                            {
                                return new AntdUI.CellBadge(AntdUI.TState.Error, AntdUI.Localization.Get("Offline", "离线"));
                            }
                        }

                        return value;
                    },
                }.SetSortOrder().SetLocalizationTitleID("Table.AccountList.Column."),
                new AntdUI.Column("LimitLinks", "链接数", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is AccountInfo ai)
                        {
                            if(ai.IsLimitLinks)
                            {
                                return new AntdUI.CellTag(ai.LimitLinks.ToString(), AntdUI.TTypeMini.Info);
                            }
                            else
                            {
                                return new AntdUI.CellTag(AntdUI.Localization.Get("Unlimited", "无限制"), AntdUI.TTypeMini.Success);
                            }
                        }

                        return value;
                    },
                }.SetLocalizationTitleID("Table.AccountList.Column."),
                new AntdUI.Column("LimitDevices", "设备数", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is AccountInfo ai)
                        {
                            if(ai.IsLimitDevices)
                            {
                                return new AntdUI.CellTag(ai.LimitDevices.ToString(), AntdUI.TTypeMini.Info);
                            }
                            else
                            {
                                return new AntdUI.CellTag(AntdUI.Localization.Get("Unlimited", "无限制"), AntdUI.TTypeMini.Success);
                            }
                        }

                        return value;
                    },
                }.SetLocalizationTitleID("Table.AccountList.Column."),
                new AntdUI.Column("ExpiryTime", "过期时间").SetSortOrder().SetLocalizationTitleID("Table.AccountList.Column."),
                new AntdUI.Column("CellLinks", "操作")
                {
                    Render = (value, record, rowindex)=>
                    {
                        return new AntdUI.CellLink[]
                        {
                            new AntdUI.CellButton("bEdit", null, AntdUI.TTypeMini.Primary).SetIcon("EditOutlined"),
                            new AntdUI.CellButton("bLocation", null, AntdUI.TTypeMini.Warn).SetIcon("EnvironmentOutlined"),
                            new AntdUI.CellButton("bDelete", null, AntdUI.TTypeMini.Error).SetIcon("CloseOutlined"),
                        };
                    },
                }.SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.AccountList.Column."),
            };

            this.tAccountList.ColumnFont = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.pAccountList.PageSizeOptions = new int[] { 10, 20, 30, 50, 100 };
            this.tAccountList.Binding(GetPageData(this.pAccountList.Current, this.pAccountList.PageSize));
        }

        private void InitCalendar_ExpiryTime()
        {
            try
            {
                Dictionary<string, int> TimeCounts = new Dictionary<string, int>();

                foreach (AccountInfo ai in Operate.ProxyConfig.Account.lstAccountInfo)
                {
                    string ExpiryTime = ai.ExpiryTime.ToString("yyyy-MM-dd");

                    if (TimeCounts.ContainsKey(ExpiryTime))
                    {
                        TimeCounts[ExpiryTime]++;
                    }
                    else
                    {
                        TimeCounts.Add(ExpiryTime, 1);
                    }
                }

                dtpExpiryTime.BadgeAction = dates =>
                {
                    List<AntdUI.DateBadge> dbList = new List<AntdUI.DateBadge>();
                    foreach (var kvp in TimeCounts)
                    {
                        dbList.Add(new AntdUI.DateBadge(kvp.Key, kvp.Value));
                    }

                    return dbList;
                };
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private BindingList<AccountInfo> GetPageData(int current, int pageSize)
        {
            var list = new BindingList<AccountInfo>();

            try
            {
                if (this.lstAccount == null)
                {
                    this.lstAccount = Operate.ProxyConfig.Account.lstAccountInfo;
                }

                this.pAccountList.Total = this.lstAccount.Count;
                int start = Math.Abs(current - 1) * pageSize;

                for (int i = start; i < this.lstAccount.Count && i < start + pageSize; i++)
                {
                    list.Add(this.lstAccount[i]);
                }

                this.InitCalendar_ExpiryTime();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return list;
        }

        private void pAccountList_ValueChanged(object sender, PagePageEventArgs e)
        {
            this.tAccountList.Binding(GetPageData(e.Current, e.PageSize));
        }

        private string pAccountList_ShowTotalChanged(object sender, PagePageEventArgs e)
        {
            var sb = new StringBuilder();
            sb.Append(e.PageSize);
            sb.Append(" / ");
            sb.Append(e.Total);
            sb.Append(AntdUI.Localization.Get("Per", "条") + " ");
            sb.Append(e.PageTotal);
            sb.Append(AntdUI.Localization.Get("Page", "页"));
            return sb.ToString();
        }

        public void RefreshAccountList()
        {            
            this.tAccountList.Binding(GetPageData(this.pAccountList.Current, this.pAccountList.PageSize));
        }

        #endregion

        #region//账号列表 - 菜单

        private void mAccountList_SelectChanged(object sender, MenuSelectEventArgs e)
        {
            AntdUI.MenuItem miSelect = e.Value;
            this.mAccountList.USelect();

            switch (miSelect.ID)
            {
                case "miAdd":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this.form, new AccountEditForm(this.form, null))
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "miImport":

                    Operate.ProxyConfig.Account.LoadAccountList_Dialog(this.form);

                    break;

                case "miExport":

                    if (Operate.ProxyConfig.Account.lstAccountInfo.Count > 0)
                    {
                        Operate.ProxyConfig.Account.SaveAccount_Dialog(this.form, string.Empty, null);
                    }

                    break;

                case "miClear":

                    if (Operate.ProxyConfig.Account.lstAccountInfo.Count > 0)
                    {
                        Operate.ProxyConfig.Account.DeleteAccount_Dialog(this.form, null);
                    }

                    break;
            }
        }

        private void tAccountList_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            if (e.Record is AccountInfo ai)
            {
                switch (e.Btn.Id)
                {
                    case "bEdit":

                        AntdUI.Drawer.open(new AntdUI.Drawer.Config(this.form, new AccountEditForm(this.form, ai))
                        {
                            Align = AntdUI.TAlignMini.Right,
                            Mask = true,
                            MaskClosable = false,
                            DisplayDelay = 0,
                        });

                        break;

                    case "bLocation":

                        AntdUI.Drawer.open(new AntdUI.Drawer.Config(this.form, new LocationForm(this.form, ai))
                        {
                            Align = AntdUI.TAlignMini.Right,
                            Mask = true,
                            MaskClosable = false,
                            DisplayDelay = 0,
                        });

                        break;

                    case "bDelete":

                        List<AccountInfo> aiList = new List<AccountInfo>
                        {
                            ai
                        };

                        Operate.ProxyConfig.Account.DeleteAccount_Dialog(this.form, aiList);

                        break;
                }
            }
        }

        #endregion

        #region//账号列表 - 右键菜单

        private void tAccountList_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Operate.ProxyConfig.Account.lstAccountInfo.Count == 0)
                {
                    return;
                }

                AntdUI.ContextMenuStrip.open(new AntdUI.ContextMenuStrip.Config(tAccountList, (item) =>
                {
                    List<AccountInfo> aiList = new List<AccountInfo>();
                    foreach (AccountInfo ai in Operate.ProxyConfig.Account.lstAccountInfo)
                    {
                        if (ai.IsCheck)
                        {
                            aiList.Add(ai);
                        }
                    }

                    switch (item.ID)
                    {
                        case "ExpiryTime":

                            if (aiList.Count > 0)
                            {
                                AntdUI.Drawer.open(new AntdUI.Drawer.Config(this.form, new ExpiryTimeForm(this.form, aiList))
                                {
                                    Align = AntdUI.TAlignMini.Right,
                                    Mask = true,
                                    MaskClosable = false,
                                    DisplayDelay = 0,
                                });
                            }

                            break;

                        case "LimitLinks":

                            if (aiList.Count > 0)
                            {
                                AntdUI.Drawer.open(new AntdUI.Drawer.Config(this.form, new LimitLinksForm(this.form, aiList))
                                {
                                    Align = AntdUI.TAlignMini.Right,
                                    Mask = true,
                                    MaskClosable = false,
                                    DisplayDelay = 0,
                                });
                            }

                            break;

                        case "LimitDevices":

                            if (aiList.Count > 0)
                            {
                                AntdUI.Drawer.open(new AntdUI.Drawer.Config(this.form, new LimitDevicesForm(this.form, aiList))
                                {
                                    Align = AntdUI.TAlignMini.Right,
                                    Mask = true,
                                    MaskClosable = false,
                                    DisplayDelay = 0,
                                });
                            }

                            break;

                        case "Export":

                            if (aiList.Count > 0)
                            {
                                Operate.ProxyConfig.Account.SaveAccount_Dialog(this.form, string.Empty, aiList);
                            }

                            break;

                        case "Delete":

                            if (aiList.Count > 0)
                            {
                                Operate.ProxyConfig.Account.DeleteAccount_Dialog(this.form, aiList);
                            }

                            break;
                    }

                    this.tAccountList.SelectedIndex = -1;
                }, Operate.ProxyConfig.Account.GetCMS_AccountList()));
            }
        }

        #endregion

        #region//账号列表 - 搜索

        private void bSearchExpiryTime_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dtpExpiryTime.Value != null)
                {
                    if (this.dtpExpiryTime.Value.Count() == 2)
                    {
                        DateTime dtStart = this.dtpExpiryTime.Value[0];
                        DateTime dtEnd = this.dtpExpiryTime.Value[1];

                        this.lstAccount = Operate.ProxyConfig.Account.GetProxyAccount_ByExpireTime(dtStart, dtEnd);
                        this.RefreshAccountList();
                    }                    
                }                
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bReset_Click(object sender, EventArgs e)
        {
            this.dtpExpiryTime.Clear();
            this.lstAccount = Operate.ProxyConfig.Account.lstAccountInfo;
            this.RefreshAccountList();
        }

        private void txtSearchUserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13 && sender is Input input)
            {
                e.Handled = true;

                string UserName = this.txtSearchUserName.Text.Trim();

                if (string.IsNullOrEmpty(UserName))
                {
                    this.lstAccount = Operate.ProxyConfig.Account.lstAccountInfo;
                }
                else
                {
                    this.lstAccount = Operate.ProxyConfig.Account.GetAccount_ByUserName(UserName);
                }

                this.RefreshAccountList();
            }
        }

        #endregion        

        #region//保存账号列表（异步）

        public void SaveAccountList()
        {
            if (Operate.ProxyConfig.Account.NeedSave && !this.bgwAccountList.IsBusy)
            {
                Operate.ProxyConfig.Account.NeedSave = false;
                this.bgwAccountList.RunWorkerAsync();
            }
        }

        private void bgwAccountList_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Operate.ProxyConfig.Account.SaveAccountList_ToDB();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion        
    }
}
