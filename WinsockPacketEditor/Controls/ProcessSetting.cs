using AntdUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class ProcessSetting : UserControl
    {
        private Form form = null;

        #region//窗体事件

        public ProcessSetting(Form form)
        {
            InitializeComponent();
            this.form = form;            
        }

        private void ProcessSetting_Load(object sender, EventArgs e)
        {
            try
            {
                this.InitProcessList();

                if (Operate.ProxyConfig.Proxy.IsLoadDriver)
                {                    
                    this.tlpLoadDriver.Enabled = false;
                    this.bUninstallDriver.Enabled = true;
                }
                else
                {                    
                    this.tlpLoadDriver.Enabled = true;
                    this.bUninstallDriver.Enabled = false;
                }

                this.cbMustTCP_Auth.Checked = Operate.ProxyConfig.Proxy.MustTCP_Auth;
                this.MustTCP_Auth_Changed();

                this.txtMustTCP_IP.Text = Operate.ProxyConfig.Proxy.MustTCP_IP;
                this.txtMustTCP_Port.Value = Operate.ProxyConfig.Proxy.MustTCP_Port;
                this.txtMustTCP_UserName.Text = Operate.ProxyConfig.Proxy.MustTCP_UserName;
                this.txtMustTCP_PassWord.Text = Operate.ProxyConfig.Proxy.MustTCP_PassWord;

                this.transferProcessList.SourceTitle = AntdUI.Localization.Get("ProcessSetting.SourceTitle", "进程列表");
                this.transferProcessList.TargetTitle = AntdUI.Localization.Get("ProcessSetting.TargetTitle", "拦截列表");                
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(ProcessSetting_Load), ex.Message);
            }                        
        }

        #endregion

        #region//初始化进程列表

        private void InitProcessList()
        {
            try
            {
                List<AntdUI.TransferItem> lstProcess = new List<AntdUI.TransferItem>();

                AntdUI.Spin.open(this, new AntdUI.Spin.Config()
                {
                    Radius = 6,
                    Font = new Font("Microsoft YaHei UI", 9F),
                }, (config) =>
                {
                    config.Text = AntdUI.Localization.Get("Loading", "正在加载...");                    
                    var processList = Operate.ProcessConfig.GetProcessList();

                    foreach (var process in processList)
                    {
                        AntdUI.TransferItem tiProcess = new AntdUI.TransferItem()
                        {
                            Text = string.Format("{0} [{1}]", process.ProcessName, process.ProcessID),
                            Value = process
                        };

                        lstProcess.Add(tiProcess);

                        if (Operate.ProxyConfig.Proxy.lstSelectProcess.Count > 0)
                        {
                            var existingItem = Operate.ProxyConfig.Proxy.lstSelectProcess.FirstOrDefault(x => x.Text == tiProcess.Text);
                            if (existingItem != null)
                            {
                                tiProcess.IsTarget = true;
                            }
                        }                        
                    }
                    
                }, () =>
                {
                    this.transferProcessList.Items = lstProcess;
                    this.transferProcessList.Reload();
                });
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(InitProcessList), ex.Message);
            }
        }

        #endregion

        #region//卸载驱动

        private void bUninstallDriver_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region//需要认证

        private void cbMustTCP_Auth_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.MustTCP_Auth_Changed();
        }

        private void MustTCP_Auth_Changed()
        {
            this.txtMustTCP_UserName.Enabled = this.txtMustTCP_PassWord.Enabled = this.cbMustTCP_Auth.Checked;
        }

        #endregion

        #region//刷新进程

        private void bRefresh_Click(object sender, EventArgs e)
        {
            this.InitProcessList();
        }

        #endregion

        #region//数据完整性检查

        private bool CheckSetting()
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtMustTCP_IP.Text.Trim()))
                {
                    this.txtMustTCP_IP.Status = TType.Error;
                    return false;
                }
                else
                {
                    this.txtMustTCP_IP.Status = TType.Success;
                }

                if (this.cbMustTCP_Auth.Checked)
                {
                    if (string.IsNullOrEmpty(this.txtMustTCP_UserName.Text.Trim()))
                    {
                        this.txtMustTCP_UserName.Status = TType.Error;
                        return false;
                    }
                    else
                    {
                        this.txtMustTCP_UserName.Status = TType.Success;
                    }

                    if (string.IsNullOrEmpty(this.txtMustTCP_PassWord.Text.Trim()))
                    {
                        this.txtMustTCP_PassWord.Status = TType.Error;
                        return false;
                    }
                    else
                    {
                        this.txtMustTCP_PassWord.Status = TType.Success;
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(CheckSetting), ex.Message);
            }

            return false;
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.CheckSetting())
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "保存失败，请检查数据设置", TType.Error)
                    {
                        LocalizationText = "ProcessSetting.LoadDriver.Error"
                    });

                    return;
                }

                if (!Operate.ProxyConfig.Proxy.IsLoadDriver)
                {                    
                    if (this.rbProxifier.Checked)
                    {
                        Operate.ProxyConfig.Proxy.IsLoadDriver = Operate.ProxyConfig.Proxy.syNet.LoadDriver(0);
                    }
                    else if (this.rbNFAPI.Checked)
                    {
                        Operate.ProxyConfig.Proxy.IsLoadDriver = Operate.ProxyConfig.Proxy.syNet.LoadDriver(1);
                    }
                    else
                    {
                        Operate.ProxyConfig.Proxy.IsLoadDriver = Operate.ProxyConfig.Proxy.syNet.LoadDriver(2);
                    }                    
                }

                if (Operate.ProxyConfig.Proxy.IsLoadDriver)
                {
                    var targetItems = this.transferProcessList.GetTargetItems();
                    Operate.ProxyConfig.Proxy.lstSelectProcess = targetItems.Count > 0
                        ? targetItems
                        : new List<TransferItem>();

                    Operate.ProxyConfig.Proxy.syNet.RemoveAllProcesses();
                    foreach (TransferItem item in Operate.ProxyConfig.Proxy.lstSelectProcess)
                    {
                        if (item.Value is ProcessInfo pi)
                        {
                            Operate.ProxyConfig.Proxy.syNet.AddProcessPid(pi.ProcessID);
                        }
                    }

                    Operate.ProxyConfig.Proxy.MustTCP_Auth = this.cbMustTCP_Auth.Checked;
                    Operate.ProxyConfig.Proxy.MustTCP_IP = this.txtMustTCP_IP.Text.Trim();
                    Operate.ProxyConfig.Proxy.MustTCP_Port = ((ushort)this.txtMustTCP_Port.Value);
                    Operate.ProxyConfig.Proxy.MustTCP_UserName = this.txtMustTCP_UserName.Text.Trim();
                    Operate.ProxyConfig.Proxy.MustTCP_PassWord = this.txtMustTCP_PassWord.Text.Trim();

                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "进程设置保存成功", TType.Success)
                    {
                        LocalizationText = "ProcessSetting.Success"
                    });

                    this.Dispose();
                }
                else
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "加载驱动失败, 请检查是否管理员权限运行", TType.Error)
                    {
                        LocalizationText = "ProcessSetting.LoadDriver.Error"
                    });
                }
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
