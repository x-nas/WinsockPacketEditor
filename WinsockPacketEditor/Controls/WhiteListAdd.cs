using AntdUI;
using System;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class WhiteListAdd : UserControl
    {
        private Form form = null;

        #region//窗体事件

        public WhiteListAdd(Form form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void WhiteListAdd_Load(object sender, EventArgs e)
        {
            this.IPType_Changed();
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
                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "数据格式错误", TType.Error)
                {
                    LocalizationText = "WhiteListAdd.Error"
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

            Operate.ProxyConfig.Proxy.AddToWhiteList(IPString);
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
