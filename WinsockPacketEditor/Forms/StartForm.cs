using AntdUI;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class StartForm : BorderlessForm
    {
        private string WebSiteURL = Operate.SystemConfig.WPE64_URL;
        private Color cEnter_Dark = UiTheme.Color_57;
        private Color cLeave_Dark = UiTheme.Color_50;
        private Color cEnter_Light = UiTheme.Color_250;
        private Color cLeave_Light = Color.Transparent;

        #region//窗体事件

        public StartForm()
        {
            //必须在 ShowBetaMessage 之前：该方法会弹 Modal
            UI.Attach(new WinFormsUiHost(this), new AntdL10n());
            UI.AttachFeed(WinFormsUiFeed.Instance);

            UiDialogs.ShowBetaMessage(this);
            InitializeComponent();
        }

        private void StartForm_Load(object sender, EventArgs e)
        {
            this.Text = "WPE x64 - " + AntdUI.Localization.Get("StartForm", "首页");
            Operate.SystemConfig.SelectMode = Operate.SystemConfig.SystemMode.None;

            this.InitGlobal();
            this.GetWebSiteURL();
            this.Dark_Changed();            
        }

        private void StartForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Operate.SystemConfig.SaveSystemConfig_ToDB();
        }

        private void InitGlobal()
        {
            /*
                六种语言 —— 与 Vue 外壳的 web/src/i18n/langs.ts、
                以及 ClassObject/L10n 的五张对照表<b>是同一份清单</b>，三处要一起改。

                显示名一律用<b>该语言自己的写法</b>：切到看不懂的语言时，
                「English」「日本語」这样的自称是唯一还认得出来的东西。
            */
            var globals = new AntdUI.SelectItem[] {
                new AntdUI.SelectItem("简体中文","zh-CN"),
                new AntdUI.SelectItem("繁體中文","zh-TW"),
                new AntdUI.SelectItem("English","en-US"),
                new AntdUI.SelectItem("日本語","ja-JP"),
                new AntdUI.SelectItem("한국어","ko-KR"),
                new AntdUI.SelectItem("Tiếng Việt","vi-VN"),
                new AntdUI.SelectItem("Русский","ru-RU")
            };

            btn_global.Items.AddRange(globals);

            /*
                只比前两位：库里可能存着 "en-GB" 这类值，没必要为此加一张别名表。

                ⚠️ 中文是例外 —— zh-CN 与 zh-TW 前两位相同，只比前缀会一律落到简体。
                所以先整串精确对一遍，对不上再退回前两位。
            */
            string lang = (AntdUI.Localization.CurrentLanguage ?? string.Empty).ToLowerInvariant();
            AntdUI.SelectItem picked = null;

            foreach (AntdUI.SelectItem it in globals)
            {
                if (lang == ((string)it.Tag).ToLowerInvariant())
                {
                    picked = it;
                    break;
                }
            }

            if (picked == null)
            {
                foreach (AntdUI.SelectItem it in globals)
                {
                    string tag = ((string)it.Tag).ToLowerInvariant();

                    if (lang.StartsWith(tag.Substring(0, 2)))
                    {
                        picked = it;
                        break;
                    }
                }
            }

            if (picked == null) { picked = globals[0]; }

            btn_global.SelectedValue = picked.Tag;
        }

        private void SelectedStartMode()
        {
            if (Operate.SystemConfig.SelectMode != Operate.SystemConfig.SystemMode.None)
            {
                this.DialogResult = DialogResult.OK;
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

                //写入唯一真源后统一应用，这样界面上切换的语言会随配置一起持久化
                UI.Prefs.Language = lang;
                WinFormsUiHost.ApplyLanguage();
                Refresh();
                btn_global.Loading = false;
            }
        }

        #endregion

        #region//更换主题模式

        private void btn_mode_Click(object sender, EventArgs e)
        {
            /*
                顶栏这个开关是<b>明确的选择</b>，所以要把「跟随系统」摘掉。
                不摘的话：在这里点成深色，下次进外壳仍显示「跟随系统」并按系统
                重新解析一遍 —— 用户刚做的选择被自己的配置吃掉了。
                WinForms 侧没有第三档，也不需要有；它只负责在被点到时说清「我选定了」。
            */
            UI.Prefs.FollowSystemTheme = false;
            UI.Prefs.IsDark = !UI.Prefs.IsDark;
            WinFormsUiHost.ApplyPrefs();

            this.Dark_Changed();
            OnSizeChanged(e);
        }

        private void Dark_Changed()
        {
            Dark = AntdUI.Config.IsDark;
            btn_mode.Toggle = Dark;

            if (Dark)
            {
                BackColor = UiTheme.Color_30;
                ForeColor = Color.White;

                this.pMultipleOpen.Back = 
                    this.pInjectMode.Back = 
                    this.pProxyMode.Back =
                    UiTheme.Color_50;
            }
            else
            {
                BackColor = Color.White;
                ForeColor = Color.Black;

                this.pMultipleOpen.Back =
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
            this.StartInjectMode();
        }

        private void InjectMode_Click(object sender, EventArgs e)
        {
            this.StartInjectMode();
        }

        private void lInject1_Click(object sender, EventArgs e)
        {
            this.StartInjectMode();
        }

        private void lInject2_Click(object sender, EventArgs e)
        {
            this.StartInjectMode();
        }

        private void StartInjectMode()
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

        #region//多开设置

        private void pMultipleOpen_MouseEnter(object sender, EventArgs e)
        {
            this.SetPanelBack_Enter(this.pMultipleOpen);
        }

        private void pMultipleOpen_MouseLeave(object sender, EventArgs e)
        {
            this.SetPanelBack_Leave(this.pMultipleOpen);
        }

        private void pMultipleOpen_Click(object sender, EventArgs e)
        {
            this.StartDataBaseSetting();
        }

        private void aMultipleOpen_Click(object sender, EventArgs e)
        {
            this.StartDataBaseSetting();
        }

        private void lMultipleOpen_Click(object sender, EventArgs e)
        {
            this.StartDataBaseSetting();
        }

        private void lMultipleOpenText_Click(object sender, EventArgs e)
        {
            this.StartDataBaseSetting();
        }

        private void StartDataBaseSetting()
        {
            var DataBaseSetting = new DataBaseSetting(this);
            AntdUI.Modal.open(new AntdUI.Modal.Config(this, AntdUI.Localization.Get("MultipleOpenSetting", "多开设置"), DataBaseSetting)
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
            this.StartProxyMode();
        }

        private void aProxyMode_Click(object sender, EventArgs e)
        {
            this.StartProxyMode();
        }

        private void lProxy1_Click(object sender, EventArgs e)
        {
            this.StartProxyMode();
        }

        private void lProxy2_Click(object sender, EventArgs e)
        {
            this.StartProxyMode();
        }

        private void StartProxyMode()
        {
            Operate.SystemConfig.SelectMode = Operate.SystemConfig.SystemMode.Proxy;
            this.SelectedStartMode();
            this.Close();
        }

        #endregion

        #region//了解更多

        private async void GetWebSiteURL()
        {
            try
            {
                    bool bOK = await Operate.SystemConfig.CheckWebSite(Operate.SystemConfig.WPE64_URL);

                    if (!bOK)
                    {
                        this.WebSiteURL = Operate.SystemConfig.WPE64_IP;
                    }
            }
            catch (Exception ex)
            {
                //async void：await 之后抛出的异常不会被 WinForms 兜住，必须自己捕获
                Operate.DoLog(nameof(GetWebSiteURL), ex);
            }
        }

        private void bWPEWebSite_Click(object sender, EventArgs e)
        {
            var lang = AntdUI.Localization.CurrentLanguage;
            if (lang.StartsWith("en"))
            {
                Process.Start(this.WebSiteURL + "/index_enUS.html");
            }
            else
            {
                Process.Start(this.WebSiteURL + "/index.html");
            }
        }

        private void bTutorials_Click(object sender, EventArgs e)
        {
            var lang = AntdUI.Localization.CurrentLanguage;
            if (lang.StartsWith("en"))
            {
                Process.Start(this.WebSiteURL + "/tutorials_enUS.html");
            }
            else
            {
                Process.Start(this.WebSiteURL + "/tutorials.html");
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
