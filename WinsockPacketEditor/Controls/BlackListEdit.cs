using AntdUI;
using System;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class BlackListEdit : UserControl
    {
        private Form form = null;
        private FireWallSetting fwForm = null;
        private BlackListInfo bliSelect = null;

        #region//窗体事件

        public BlackListEdit(Form form, FireWallSetting fwForm, BlackListInfo bliSelect)
        {
            InitializeComponent();
            this.form = form;
            this.fwForm = fwForm;
            this.bliSelect = bliSelect;
        }

        private void BlackListEdit_Load(object sender, EventArgs e)
        {
            this.dtpExpiryTime.Value = DateTime.Now;

            if (this.bliSelect != null)
            {
                if (this.bliSelect.IPAddress.Contains("-"))
                {
                    this.rbIPRange.Checked = true;
                    string[] IPRange = this.bliSelect.IPAddress.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
                    if (IPRange.Length == 2)
                    {
                        this.txtIPRangeFrom.Text = IPRange[0];
                        this.txtIPRangeTo.Text = IPRange[1];
                    }                    
                }
                else
                {
                    this.rbSingleIP.Checked = true;
                    this.txtSingleIP.Text = this.bliSelect.IPAddress;
                }

                this.cbExpiryTime.Checked = this.bliSelect.IsExpiry;
                if (this.bliSelect.ExpiryTime > this.dtpExpiryTime.MaxDate)
                {
                    this.dtpExpiryTime.Value = this.dtpExpiryTime.MaxDate;
                }
                else
                {
                    this.dtpExpiryTime.Value = bliSelect.ExpiryTime;
                }
            }

            this.IPType_Changed();
            this.ExpiryTime_Changed();
        }

        #endregion

        #region//选择IP类型

        private void rbSingleIP_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            this.IPType_Changed();
        }

        private void rbIPRange_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            this.IPType_Changed();
        }

        private void IPType_Changed()
        {
            this.txtSingleIP.Enabled = this.rbSingleIP.Checked;
            this.txtIPRangeFrom.Enabled = this.txtIPRangeTo.Enabled = this.rbIPRange.Checked;
        }

        #endregion        

        #region//过期时间

        private void cbExpiryTime_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.ExpiryTime_Changed();
        }

        private void ExpiryTime_Changed()
        {
            this.dtpExpiryTime.Enabled = this.cbExpiryTime.Checked;
        }

        #endregion        

        #region//检查保存设置

        private bool CheckSave()
        {
            if (this.rbSingleIP.Checked)
            {
                string SingleIP = this.txtSingleIP.Text.Trim();
                if (string.IsNullOrEmpty(SingleIP))
                {
                    this.txtSingleIP.Status = TType.Error;
                    return false;
                }
                else
                {
                    this.txtSingleIP.Status = TType.Success;
                }

                if (!Operate.SystemConfig.IsValidIPv4(SingleIP))
                {
                    this.txtSingleIP.Status = TType.Error;
                    return false;
                }
                else
                {
                    this.txtSingleIP.Status = TType.Success;
                }
            }
            else
            {
                string IPRangeFrom = this.txtIPRangeFrom.Text.Trim();
                if (string.IsNullOrEmpty(IPRangeFrom))
                {
                    this.txtIPRangeFrom.Status = TType.Error;
                    return false;
                }
                else
                {
                    this.txtIPRangeFrom.Status = TType.Success;
                }

                if (!Operate.SystemConfig.IsValidIPv4(IPRangeFrom))
                {
                    this.txtIPRangeFrom.Status = TType.Error;
                    return false;
                }
                else
                {
                    this.txtIPRangeFrom.Status = TType.Success;
                }

                string IPRangeTo = this.txtIPRangeTo.Text.Trim();
                if (string.IsNullOrEmpty(IPRangeTo))
                {
                    this.txtIPRangeTo.Status = TType.Error;
                    return false;
                }
                else
                {
                    this.txtIPRangeTo.Status = TType.Success;
                }

                if (!Operate.SystemConfig.IsValidIPv4(IPRangeTo))
                {
                    this.txtIPRangeTo.Status = TType.Error;
                    return false;
                }
                else
                {
                    this.txtIPRangeTo.Status = TType.Success;
                }
            }

            return true;
        }

        #endregion        

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            if (!this.CheckSave())
            {
                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "IP地址错误", TType.Error)
                {
                    LocalizationText = "FireWallSetting.IPAddress.Error"
                });

                return;
            }

            string IPString = string.Empty;
            if (this.rbSingleIP.Checked)
            {
                IPString = this.txtSingleIP.Text.Trim();
            }
            else
            {
                IPString = this.txtIPRangeFrom.Text.Trim() + "-" + this.txtIPRangeTo.Text.Trim();
            }

            bool IsExpiry = this.cbExpiryTime.Checked;
            DateTime ExpiryTime;
            if (IsExpiry)
            {
                ExpiryTime = this.dtpExpiryTime.Value.Value;
            }
            else
            {
                ExpiryTime = this.dtpExpiryTime.MaxDate.Value;
            }

            if (this.bliSelect != null)
            {
                if (!this.bliSelect.IPAddress.Equals(IPString))
                {
                    if (Operate.ProxyConfig.Proxy.IsExistsInBlackList(IPString))
                    {
                        AntdUI.Message.open(new AntdUI.Message.Config(this.form, "此IP地址已存在", TType.Error)
                        {
                            LocalizationText = "FireWallSetting.IPAddress.Exists"
                        });

                        return;
                    }
                }

                Operate.ProxyConfig.Proxy.UpdateBlackList(this.bliSelect, IPString, IsExpiry, ExpiryTime);
            }
            else
            {
                if (Operate.ProxyConfig.Proxy.IsExistsInBlackList(IPString))
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "此IP地址已存在", TType.Error)
                    {
                        LocalizationText = "FireWallSetting.IPAddress.Exists"
                    });

                    return;
                }

                Operate.ProxyConfig.Proxy.AddToBlackList(IPString, IsExpiry, ExpiryTime);
            }

            AntdUI.Message.open(new AntdUI.Message.Config(this.form, "黑名单保存成功", TType.Success)
            {
                LocalizationText = "FireWallSetting.BlackList.Save.Success"
            });

            this.fwForm.RefreshBlackList();
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
