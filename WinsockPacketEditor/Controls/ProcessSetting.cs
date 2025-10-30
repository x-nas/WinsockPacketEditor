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

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            try
            {
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
