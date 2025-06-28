using AntdUI;
using System;
using System.Drawing;
using System.Windows.Forms;
using WPE.Lib;
using WPE.Lib.Controls;

namespace WPE.InjectMode
{
    public partial class InjectModeForm : AntdUI.Window
    {
        private bool setcolor = false;

        #region//窗体事件

        public InjectModeForm()
        {
            InitializeComponent();            

            this.InitIsDark();
            this.InitForm();            
        }

        private void InjectModeForm_Load(object sender, EventArgs e)
        {
            Operate.SystemConfig.LoadSystemConfig_FromDB();
            Operate.SystemConfig.LoadInjectMode_FromDB();

            this.tabInjectMode.TabMenuVisible = false;
        }

        private void InjectModeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Operate.SystemConfig.SaveSystemList_ToDB();
            Operate.SystemConfig.SaveInjectMode_ToDB();
        }

        private void InitForm()
        {
            this.Text = "WPE x64 - " + AntdUI.Localization.Get("InjectModeForm", "注入模式");
            this.pageHeader.Text = "Winsock Packet Editor";
            this.pageHeader.SubText = Operate.SystemConfig.AssemblyVersion;

            this.mInjectMode.Collapsed = true;
            this.MenuCollapseChange();

            btn_global.Items.AddRange(
                new AntdUI.ISelectItem[]
                {
                    new AntdUI.SelectItem("中文", "zh-CN"),
                    new AntdUI.SelectItem("English", "en-US")
                });

            var lang = AntdUI.Localization.CurrentLanguage;
            if (lang.StartsWith("en"))
            {
                btn_global.SelectedValue = btn_global.Items[1];
            }
            else
            {
                btn_global.SelectedValue = btn_global.Items[0];
            }
        }

        private void InitIsDark()
        {
            if (AntdUI.Config.IsDark)
            {
                BackColor = Color.Black;
                ForeColor = Color.White;
            }
            else
            {
                BackColor = Color.White;
                ForeColor = Color.Black;
            }
        }

        #endregion

        #region//更换主题颜色

        private void colorTheme_ValueChanged(object sender, AntdUI.ColorEventArgs e)
        {
            setcolor = true;
            AntdUI.Style.SetPrimary(e.Value);
            Refresh();
        }

        #endregion

        #region//更换主题模式

        private void btn_mode_Click(object sender, EventArgs e)
        {
            if (setcolor)
            {
                var color = AntdUI.Style.Db.Primary;
                AntdUI.Config.IsDark = !AntdUI.Config.IsDark;
                Dark = AntdUI.Config.IsDark;
                AntdUI.Style.SetPrimary(color);
            }
            else
            {
                AntdUI.Config.IsDark = !AntdUI.Config.IsDark;
                Dark = AntdUI.Config.IsDark;
            }

            btn_mode.Toggle = Dark;

            if (Dark)
            {
                BackColor = Color.Black;
                ForeColor = Color.White;
            }
            else
            {
                BackColor = Color.White;
                ForeColor = Color.Black;
            }

            OnSizeChanged(e);
        }

        #endregion

        #region//切换语言

        private void btn_global_SelectedValueChanged(object sender, AntdUI.ObjectNEventArgs e)
        {
            if (e.Value is AntdUI.SelectItem value)
            {
                if (btn_global.Tag == value)
                {
                    return;
                }

                btn_global.Tag = value;
                btn_global.Loading = true;

                string lang = value.Tag.ToString();
                if (lang.StartsWith("en"))
                {
                    AntdUI.Localization.Provider = new Localizer();
                }
                else
                {
                    AntdUI.Localization.Provider = null;
                }

                AntdUI.Localization.SetLanguage(lang);
                this.Text = "WPE x64 - " + AntdUI.Localization.Get("InjectModeForm", "注入模式");
                Refresh();

                btn_global.Loading = false;
            }
        }

        #endregion

        #region//系统设置

        private void btn_setting_Click(object sender, EventArgs e)
        {
            var setting = new SystemSetting();
            if (AntdUI.Modal.open(this, AntdUI.Localization.Get("Setting", "设置"), setting) == DialogResult.OK)
            {
                AntdUI.Config.Animation = setting.Animation;
                AntdUI.Config.ShadowEnabled = setting.ShadowEnabled;
                AntdUI.Config.ShowInWindow = setting.ShowInWindow;
                AntdUI.Config.ScrollBarHide = setting.ScrollBarHide;
                AntdUI.Config.TextRenderingHighQuality = setting.TextRenderingHighQuality;
                if (AntdUI.Config.TextRenderingHighQuality == setting.TextRenderingHighQuality)
                {
                    return;
                }

                Refresh();
            }
        }



        #endregion

        #region//主菜单

        private void bMenuCollapse_Click(object sender, EventArgs e)
        {
            this.mInjectMode.Collapsed = !this.mInjectMode.Collapsed;
            this.MenuCollapseChange();
        }

        private void MenuCollapseChange()
        {
            if (this.mInjectMode.Collapsed)
            {
                this.mInjectMode.Width = this.tlpMenu.Width = this.mInjectMode.CollapseWidth;
                this.bMenuCollapse.IconSvg = "MenuUnfoldOutlined";
            }
            else
            {
                this.mInjectMode.Width = this.tlpMenu.Width = this.mInjectMode.CollapsedWidth;
                this.bMenuCollapse.IconSvg = "MenuFoldOutlined";
            }
        }

        private void mInjectMode_SelectChanged(object sender, AntdUI.MenuSelectEventArgs e)
        {
            AntdUI.MenuItem miSelect = e.Value;

            switch (miSelect.ID)
            {
                case "miPacketList":
                    this.tabInjectMode.SelectTab(0);
                    break;

                case "miStatistical":
                    this.tabInjectMode.SelectTab(1);
                    break;

                case "miComparison":
                    this.tabInjectMode.SelectTab(2);
                    break;

                case "miXOR":
                    this.tabInjectMode.SelectTab(3);
                    break;

                case "miTranscoding":
                    this.tabInjectMode.SelectTab(4);
                    break;

                case "miExtraction":
                    this.tabInjectMode.SelectTab(5);
                    break;

                case "miSystemLog":
                    this.tabInjectMode.SelectTab(6);
                    break;
            }
        }

        #endregion

        private void segmented_SelectIndexChanged(object sender, AntdUI.IntEventArgs e)
        {
            if (this.segmented.SelectIndex == 0)//过滤设置
            {
                AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new FilterSettingsForm())
                {
                    Align = AntdUI.TAlignMini.Right,
                    Mask = true,
                    MaskClosable = false,
                    DisplayDelay = 0,
                });
            }
            else if (this.segmented.SelectIndex == 1)//拦截设置
            {
                AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new HookSettingsForm())
                {
                    Align = AntdUI.TAlignMini.Right,
                    Mask = true,
                    MaskClosable = false,
                    DisplayDelay = 0,
                });
            }

            this.segmented.SelectIndex = -1;
        }

        
    }
}
