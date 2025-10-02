using AntdUI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class BatchAccounts : UserControl
    {
        private Form form = null;
        private BindingList<AccountInfo> lstBatchAccounts = new BindingList<AccountInfo>();

        #region//窗体事件

        public BatchAccounts(Form form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void BatchAccounts_Load(object sender, EventArgs e)
        {
            this.dpExpiryTime.Value = DateTime.Now;

            this.InitBatchAccounts();
            this.InitAccountRule();
            this.LimitLinks_Changed();
            this.LimitDevices_Changed();
            this.ExpiryTime_Changed();
        }

        private void InitAccountRule()
        {
            this.ddlAccountRule.Items.Clear();
            this.ddlAccountRule.Items.AddRange(new AntdUI.SelectItem[]
            {
                new AntdUI.SelectItem("当前时间 + 序号")
                {
                    LocalizationText = "BatchAccounts.TimeID",
                },
            });

            this.ddlAccountRule.SelectedIndex = 0;
        }

        private void InitBatchAccounts()
        {
            tBatchAccounts.Columns = new AntdUI.ColumnCollection {
                new AntdUI.Column("", "序号", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return (rowindex + 1);
                    },
                }.SetFixed().SetLocalizationTitleID("Table.AccountList.Column.ID"),
                new AntdUI.Column("UserName", "用户名").SetLocalizationTitleID("Table.AccountList.Column."),
                new AntdUI.Column("Password", "密码").SetLocalizationTitleID("Table.AccountList.Column."),
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
                new AntdUI.Column("ExpiryTime", "过期时间").SetLocalizationTitleID("Table.AccountList.Column."),
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
                }.SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.AccountList.Column."),
            };

            this.tBatchAccounts.Binding(this.lstBatchAccounts);
        }

        private void tBatchAccounts_CellDoubleClick(object sender, AntdUI.TableClickEventArgs e)
        {
            if (e.Record is AccountInfo ai)
            {
                Operate.ProxyConfig.Account.OpenAccountEdit(this.form, ai);
            }
        }

        private void tBatchAccounts_CellButtonClick(object sender, AntdUI.TableButtonEventArgs e)
        {
            if (e.Record is AccountInfo ai)
            {
                switch (e.Btn.Id)
                {
                    case "bEdit":

                        Operate.ProxyConfig.Account.OpenAccountEdit(this.form, ai);

                        break;

                    case "bDelete":

                        AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("AccountList", "账号列表"), "\r\n" + AntdUI.Localization.Get("SureToDelete", "确定删除数据吗?") + "\r\n\r\n")
                        {
                            Icon = TType.Warn,
                            Keyboard = false,
                            MaskClosable = false,
                            OnOk = config =>
                            {
                                this.lstBatchAccounts.Remove(ai);

                                if (this.tBatchAccounts.InvokeRequired)
                                {
                                    this.tBatchAccounts.Invoke(new Action(() =>
                                    {
                                        this.tBatchAccounts.Refresh();
                                    }));
                                }
                                else
                                {
                                    this.tBatchAccounts.Refresh();
                                }

                                return true;
                            }
                        });

                        break;
                }
            }
        }

        #endregion

        #region//限制链接数

        private void cbLimitLinks_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            this.LimitLinks_Changed();
        }

        private void LimitLinks_Changed()
        {
            this.nudLimitLinks.Enabled = this.cbLimitLinks.Checked;
        }

        #endregion

        #region//限制设备数

        private void cbLimitDevices_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            this.LimitDevices_Changed();
        }

        private void LimitDevices_Changed()
        {
            this.nudLimitDevices.Enabled = this.cbLimitDevices.Checked;
        }

        #endregion

        #region//过期时间

        private void cbExpiryTime_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            this.ExpiryTime_Changed();
        }

        private void ExpiryTime_Changed()
        {
            this.dpExpiryTime.Enabled = this.cbExpiryTime.Checked;
        }

        #endregion

        #region//预览

        private void bPreview_Click(object sender, EventArgs e)
        {
            try
            {                
                this.tBatchAccounts.PauseLayout = true;
                this.lstBatchAccounts.Clear();

                AntdUI.Spin.open(this, new AntdUI.Spin.Config()
                {
                    Radius = 6,
                    Font = new Font("Microsoft YaHei UI", 9F),
                }, (config) =>
                {
                    config.Text = AntdUI.Localization.Get("Loading", "正在加载...");

                    int AccountNum = ((int)this.nudAccountNum.Value);

                    DateTime dtNow = DateTime.Now;
                    string AccountHead = DateTime.Now.ToString("HHmmss");
                    int PasswordLength = ((int)this.nudPasswordLength.Value);

                    for (int i = 1; i <= AccountNum; i++)
                    {
                        AccountInfo ai = new AccountInfo()
                        {
                            AID = Guid.NewGuid(),
                            IsEnable = true,
                            Password = Operate.ProxyConfig.Account.RandomPassword(PasswordLength),
                            LimitLinks = (int)this.nudLimitLinks.Value,
                            LimitDevices = (int)this.nudLimitDevices.Value,
                            CreateTime = dtNow,
                        };

                        if (this.ddlAccountRule.SelectedIndex == 0)
                        {
                            ai.UserName = AccountHead + i.ToString("D3");
                        }
                        else
                        {
                            //其他规则
                        }

                        ai.IsLimitLinks = this.cbLimitLinks.Checked;
                        ai.IsLimitDevices = this.cbLimitDevices.Checked;
                        ai.IsExpiry = this.cbExpiryTime.Checked;

                        if (ai.IsExpiry)
                        {
                            ai.ExpiryTime = this.dpExpiryTime.Value.Value;
                        }
                        else
                        {
                            ai.ExpiryTime = this.dpExpiryTime.MaxDate.Value;
                        }

                        this.lstBatchAccounts.Add(ai);
                    }
                }, () =>
                {
                    this.tBatchAccounts.PauseLayout = false;
                });
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(bPreview_Click), ex.Message);
            }            
        }

        #endregion        

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (AccountInfo ai in this.lstBatchAccounts)
                {
                    Operate.ProxyConfig.Account.lstAccountInfo.Add(ai);
                }

                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "批量创建账号成功", TType.Success)
                {
                    LocalizationText = ""
                });

                if (this.form is InterfaceInfo.IProxyMode pmForm)
                {
                    Operate.ProxyConfig.Account.NeedSave = true;
                    pmForm.RefreshAccountList();
                }

                this.Dispose();
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(bSave_Click), ex.Message);
            }
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
