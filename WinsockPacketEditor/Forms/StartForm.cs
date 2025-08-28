using AntdUI;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace WinsockPacketEditor.Forms
{
    public partial class StartForm : BorderlessForm
    {
        private Color cEnter_Dark = Color.FromArgb(57, 57, 57);
        private Color cLeave_Dark = Color.FromArgb(50, 50, 50);
        private Color cEnter_Light = Color.FromArgb(251, 251, 251);
        private Color cLeave_Light = Color.Transparent;

        #region//窗体事件

        public StartForm()
        {
            InitializeComponent();
        }

        private void StartForm_Load(object sender, EventArgs e)
        {
            this.Text = "WPE x64 - " + AntdUI.Localization.Get("StartForm", "首页");
            Operate.SystemConfig.StartMode = Operate.SystemConfig.SystemMode.None;

            this.InitGlobal();
            this.Dark_Changed();
        }

        private void StartForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Operate.SystemConfig.SaveSystemConfig_ToDB();
        }

        private void InitGlobal()
        {
            var globals = new AntdUI.SelectItem[] {
                new AntdUI.SelectItem("中文","zh-CN"),
                new AntdUI.SelectItem("English","en-US")
            };

            btn_global.Items.AddRange(globals);

            var lang = AntdUI.Localization.CurrentLanguage;
            if (lang.StartsWith("en"))
            {
                btn_global.SelectedValue = globals[1].Tag;
            }
            else
            {
                btn_global.SelectedValue = globals[0].Tag;
            }
        }

        private void SelectedStartMode()
        {
            if (Operate.SystemConfig.StartMode != Operate.SystemConfig.SystemMode.None)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void SetPanelBack_Enter(AntdUI.Panel panel)
        {
            if (AntdUI.Config.IsDark)
            {
                panel.Back = this.cEnter_Dark;
            }
            else
            { 
                panel.Back = this.cEnter_Light;
            }
        }

        private void SetPanelBack_Leave(AntdUI.Panel panel)
        {
            if (AntdUI.Config.IsDark)
            {
                panel.Back = this.cLeave_Dark;
            }
            else
            {
                panel.Back = this.cLeave_Light;
            }
        }

        #endregion

        #region//切换语言

        private void btn_global_SelectedValueChanged(object sender, AntdUI.ObjectNEventArgs e)
        {
            if (e.Value is string lang)
            {
                btn_global.Loading = true;

                if (lang.StartsWith("en"))
                {
                    AntdUI.Localization.Provider = new Localizer();
                }
                else
                {
                    AntdUI.Localization.Provider = null;
                }

                AntdUI.Localization.SetLanguage(lang);
                Refresh();
                btn_global.Loading = false;
            }
        }

        #endregion

        #region//更换主题模式

        private void btn_mode_Click(object sender, EventArgs e)
        {
            AntdUI.Config.IsDark = !AntdUI.Config.IsDark;

            this.Dark_Changed();
            OnSizeChanged(e);
        }

        private void Dark_Changed()
        {
            Dark = AntdUI.Config.IsDark;
            btn_mode.Toggle = Dark;

            if (Dark)
            {
                BackColor = Color.FromArgb(30, 30, 30);
                ForeColor = Color.White;

                this.pRemoteMGT.Back = 
                    this.pInjectMode.Back = 
                    this.pProxyMode.Back = 
                    Color.FromArgb(50, 50, 50);
            }
            else
            {
                BackColor = Color.White;
                ForeColor = Color.Black;

                this.pRemoteMGT.Back =
                    this.pInjectMode.Back =
                    this.pProxyMode.Back =
                    Color.Transparent;
            }
        }

        #endregion

        #region//注入模式

        private void pInjectMode_MouseEnter(object sender, EventArgs e)
        {
            this.SetPanelBack_Enter(this.pInjectMode);
        }

        private void pInjectMode_MouseLeave(object sender, EventArgs e)
        {
            this.SetPanelBack_Leave(this.pInjectMode);
        }

        private void pInjectMode_Click(object sender, EventArgs e)
        {
            var ProcessList = new ProcessList(this);
            AntdUI.Modal.open(new AntdUI.Modal.Config(this, AntdUI.Localization.Get("ProcessList", "进程列表"), ProcessList)
            {
                Keyboard = false,
                MaskClosable = false,
                BtnHeight = 0,
            });

            this.SelectedStartMode();
        }

        #endregion

        #region//远程管理

        private void pRemoteMGT_MouseEnter(object sender, EventArgs e)
        {
            this.SetPanelBack_Enter(this.pRemoteMGT);
        }

        private void pRemoteMGT_MouseLeave(object sender, EventArgs e)
        {
            this.SetPanelBack_Leave(this.pRemoteMGT);
        }

        private void pRemoteMGT_Click(object sender, EventArgs e)
        {
            var RemoteMGT = new RemoteMGTSetting(this);
            AntdUI.Modal.open(new AntdUI.Modal.Config(this, AntdUI.Localization.Get("Setting", "设置"), RemoteMGT)
            {
                Keyboard = false,
                MaskClosable = false,
                BtnHeight = 0,
            });
        }

        #endregion

        #region//代理模式

        private void pProxyMode_MouseEnter(object sender, EventArgs e)
        {
            this.SetPanelBack_Enter(this.pProxyMode);
        }

        private void pProxyMode_MouseLeave(object sender, EventArgs e)
        {
            this.SetPanelBack_Leave(this.pProxyMode);
        }

        private void pProxyMode_Click(object sender, EventArgs e)
        {
            Operate.SystemConfig.StartMode = Operate.SystemConfig.SystemMode.Proxy;
            this.SelectedStartMode();
        }

        #endregion

        #region//了解更多

        private void bWPEWebSite_Click(object sender, EventArgs e)
        {
            var lang = AntdUI.Localization.CurrentLanguage;
            if (lang.StartsWith("en"))
            {
                Process.Start("https://www.wpe64.com/index_enUS.html");
            }
            else
            {
                Process.Start("https://www.wpe64.com/index.html");
            }
        }

        private void bTutorials_Click(object sender, EventArgs e)
        {
            var lang = AntdUI.Localization.CurrentLanguage;
            if (lang.StartsWith("en"))
            {
                Process.Start("https://www.wpe64.com/Tutorials/WinSockPacketEditor.pdf");
            }
            else
            {
                Process.Start("https://www.wpe64.com/Tutorials/WinSockPacketEditor.pdf");
            }            
        }

        private void bGitHub_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/x-nas/WinsockPacketEditor");
        }

        private void bGitee_Click(object sender, EventArgs e)
        {
            Process.Start("https://gitee.com/X-NAS/WinsockPacketEditor");
        }

        private void bQA_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/x-nas/WinsockPacketEditor/issues");
        }

        #endregion
    }
}
