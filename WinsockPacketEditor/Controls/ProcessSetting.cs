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

                switch (Operate.ProxyConfig.Proxy.DriverType)
                {
                    case 0:
                        this.rbProxifier.Checked = true;
                        break;
                    
                    case 1:
                        this.rbNFAPI.Checked = true;
                        break; 
                    
                    case 2:
                        this.rbWinDivert.Checked = true;
                        break;                    
                }

                this.transferProcessList.SourceTitle = AntdUI.Localization.Get("ProcessSetting.SourceTitle", "进程列表");
                this.transferProcessList.TargetTitle = AntdUI.Localization.Get("ProcessSetting.TargetTitle", "拦截列表");
                this.ttcLoadDriver.SetTip(this.rbNFAPI, AntdUI.Localization.Get("ProcessSetting.NFAPI", "限制 1000000 个 TCP 连接和 UDP 套接字\r\n超过此限制后，需要重启才能继续拦截"));
                this.ttcLoadDriver.SetTip(this.rbProxifier, AntdUI.Localization.Get("ProcessSetting.Proxifier", "不支持 UDP, 不支持32位操作系统"));
                this.ttcLoadDriver.SetTip(this.rbWinDivert, AntdUI.Localization.Get("ProcessSetting.WinDivert", "不支持拦截 127.0.0.1 数据"));

                this.rbNFAPI.Enabled = this.rbProxifier.Enabled = this.rbWinDivert.Enabled = !Operate.ProxyConfig.Proxy.IsLoadDriver;
                this.bUninstallDriver.Enabled = Operate.ProxyConfig.Proxy.IsLoadDriver;
                this.txtMustTCP_IP.Text = Operate.ProxyConfig.Proxy.MustTCP_IP;
                this.nudMustTCP_Port.Value = Operate.ProxyConfig.Proxy.MustTCP_Port;
                this.cbMustTCP_AppointPort.Checked = Operate.ProxyConfig.Proxy.MustTCP_AppointPort;
                this.txtMustTCP_AppointPort.Text = Operate.ProxyConfig.Proxy.MustTCP_AppointPortContent;
                this.txtMustTCP_UserName.Text = Operate.ProxyConfig.Proxy.MustTCP_UserName;
                this.txtMustTCP_PassWord.Text = Operate.ProxyConfig.Proxy.MustTCP_PassWord;
                this.cbMustTCP_Auth.Checked = Operate.ProxyConfig.Proxy.MustTCP_Auth;               

                this.MustTCP_Auth_Changed();
                this.MustTCP_AppointPort_Changed();
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

        #region//检测

        private async void bMustTCP_Detection_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.CheckSetting())
                {
                    return;
                }

                this.bMustTCP_Detection.Loading = true;

                bool Result = await Operate.ProxyConfig.Proxy.DetectionExternalProxy(
                    this.form,
                    this.txtMustTCP_IP.Text.Trim(),
                    ((ushort)this.nudMustTCP_Port.Value),
                    this.cbMustTCP_Auth.Checked,
                    this.txtMustTCP_UserName.Text.Trim(),
                    this.txtMustTCP_PassWord.Text.Trim());

                if (Result)
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "代理服务器连接成功", TType.Success)
                    {
                        LocalizationText = "EXTProxySettingsForm.Connection"
                    });
                }

                this.bMustTCP_Detection.Loading = false;
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(bMustTCP_Detection_Click), ex.Message);
            }            
        }

        #endregion

        #region//卸载驱动

        private void bUninstallDriver_Click(object sender, EventArgs e)
        {
            try
            {
                AntdUI.Modal.open(new AntdUI.Modal.Config(form, AntdUI.Localization.Get("UninstallDriver", "卸载驱动"), "\r\n" + AntdUI.Localization.Get("UninstallDriver.Alert", "卸载驱动会立即重启电脑，若非必要请勿卸载!") + "\r\n\r\n")
                {
                    Icon = TType.Warn,
                    Keyboard = false,
                    MaskClosable = false,
                    OnOk = config =>
                    {
                        Operate.ProxyConfig.Proxy.syNet.UnDriver();
                        return true;
                    }
                });
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(bUninstallDriver_Click), ex.Message);
            }
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

        #region//指定端口

        private void cbMustTCP_AppointPort_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.MustTCP_AppointPort_Changed();
        }

        private void MustTCP_AppointPort_Changed()
        {
            this.txtMustTCP_AppointPort.Enabled = this.cbMustTCP_AppointPort.Checked;
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

                return true;
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

                Operate.ProxyConfig.Proxy.MustTCP_IP = this.txtMustTCP_IP.Text.Trim();
                Operate.ProxyConfig.Proxy.MustTCP_Port = ((ushort)this.nudMustTCP_Port.Value);
                Operate.ProxyConfig.Proxy.MustTCP_AppointPort = this.cbMustTCP_AppointPort.Checked;
                Operate.ProxyConfig.Proxy.MustTCP_AppointPortContent = this.txtMustTCP_AppointPort.Text.Trim();
                Operate.ProxyConfig.Proxy.MustTCP_Auth = this.cbMustTCP_Auth.Checked;
                Operate.ProxyConfig.Proxy.MustTCP_UserName = this.txtMustTCP_UserName.Text.Trim();
                Operate.ProxyConfig.Proxy.MustTCP_PassWord = this.txtMustTCP_PassWord.Text.Trim();

                if (!Operate.ProxyConfig.Proxy.IsLoadDriver)
                {                    
                    if (this.rbProxifier.Checked)
                    {
                        Operate.ProxyConfig.Proxy.DriverType = 0;                        
                    }
                    else if (this.rbNFAPI.Checked)
                    {
                        Operate.ProxyConfig.Proxy.DriverType = 1;
                    }
                    else
                    {
                        Operate.ProxyConfig.Proxy.DriverType = 2;
                    }

                    Operate.ProxyConfig.Proxy.IsLoadDriver = Operate.ProxyConfig.Proxy.syNet.LoadDriver(Operate.ProxyConfig.Proxy.DriverType);
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
