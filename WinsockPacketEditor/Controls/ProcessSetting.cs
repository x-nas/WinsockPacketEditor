using AntdUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class ProcessSetting : UserControl
    {
        private Form form = null;
        private List<ProcessInfo> lstProcessInfo = new List<ProcessInfo>();

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
                this.Dark_Changed();
                this.InitProcessList();
                this.ShowProcessList();

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

        private void InitProcessList()
        {
            tProcessList.Columns = new AntdUI.ColumnCollection {
                new AntdUI.ColumnCheck("IsCheck").SetFixed(),
                new AntdUI.Column("ICO", string.Empty, AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return new AntdUI.CellImage((Image)value)
                        {
                            Size = new Size(35, 35),
                        };
                    },
                }.SetLocalizationTitleID("Table.ProcessList.Column."),
                new AntdUI.Column("ProcessName", "进程名称").SetSortOrder().SetLocalizationTitleID("Table.ProcessList.Column."),
                new AntdUI.Column("ProcessID", "进程编号").SetSortOrder().SetLocalizationTitleID("Table.ProcessList.Column."),
                new AntdUI.Column("ProcessPath", "路径").SetLocalizationTitleID("Table.ProcessList.Column."),
            };
        }

        public void Dark_Changed()
        {
            if (AntdUI.Config.IsDark)
            {
                this.tProcessList.BackColor = Operate.SystemConfig.Color_40;
                this.tProcessList.ColumnBack = Operate.SystemConfig.Color_40;
            }
            else
            {
                this.tProcessList.BackColor = Color.White;
                this.tProcessList.ColumnBack = null;
            }
        }

        #endregion

        #region//显示所有进程

        private void ShowProcessList()
        {
            this.tProcessList.PauseLayout = true;

            AntdUI.Spin.open(this.tProcessList, new AntdUI.Spin.Config()
            {
                Radius = 6,
                Font = new Font("Microsoft YaHei UI", 9F),
            }, (config) =>
            {
                config.Text = AntdUI.Localization.Get("Loading", "正在加载...");
                this.lstProcessInfo = Operate.ProcessConfig.GetProcessList();
            }, () =>
            {
                if (Operate.ProxyConfig.Proxy.lstSelectProcessID.Count > 0 && this.lstProcessInfo.Count > 0)
                {
                    foreach (ProcessInfo pi in this.lstProcessInfo)
                    {
                        if (Operate.ProxyConfig.Proxy.lstSelectProcessID.Contains(pi.ProcessID))
                        {
                            pi.IsCheck = true;
                        }
                    }
                }

                this.tProcessList.DataSource = this.lstProcessInfo;
                this.tProcessList.SelectedIndex = -1;
                this.tProcessList.PauseLayout = false;
            });
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
            this.ShowProcessList();
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
                        LocalizationText = "ProcessSetting.Save.Error"
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
                    Operate.ProxyConfig.Proxy.lstSelectProcessID.Clear();
                    Operate.ProxyConfig.Proxy.syNet.RemoveAllProcesses();

                    foreach (ProcessInfo pi in this.lstProcessInfo)
                    {
                        if (pi.IsCheck)
                        {
                            Operate.ProxyConfig.Proxy.lstSelectProcessID.Add(pi.ProcessID);
                            Operate.ProxyConfig.Proxy.syNet.AddProcessPid(pi.ProcessID);
                        }                        
                    }

                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "进程设置保存成功", TType.Success)
                    {
                        LocalizationText = "ProcessSetting.Save.Success"
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
