using AntdUI;
using Be.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WPE.Lib;
using WPE.Lib.Controls;

namespace WPE.ProxyMode
{
    public partial class ProxyModeForm : Window
    {
        private bool setcolor = false;

        #region//窗体事件

        public ProxyModeForm()
        {
            InitializeComponent();
        }

        private void ProxyModeForm_Load(object sender, EventArgs e)
        {
            Operate.SystemConfig.MainHandle = this.Handle;
            Operate.SystemConfig.InvokeAction = action =>
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(action);
                }
                else
                {
                    action();
                }
            };

            this.Dark_Changed();
            this.InitForm();

            this.tabProxyMode.TabMenuVisible = false;
            this.mProxyMode.SelectIndex(0, true);

            this.pageHeader.Loading = true;
            AntdUI.Spin.open(this, AntdUI.Localization.Get("Loading", "正在加载..."), config =>
            {                
                Operate.SystemConfig.InitCPUAndMemoryCounter();
                Operate.SystemConfig.LoadSystemConfig_FromDB();
                Operate.SystemConfig.LoadInjectMode_FromDB();
                Operate.SystemConfig.LoadSystemList_FromDB();
                Operate.ProxyConfig.Account.LoadProxyAccountList_FromDB();
                Operate.ProxyConfig.Mapping.LoadProxyMapLocal_FromDB();
                Operate.ProxyConfig.Mapping.LoadProxyMapRemote_FromDB();
                Operate.SystemConfig.StartRemoteMGT();
            }, () =>
            {
                this.pageHeader.Loading = false;                
            });
        }

        private void ProxyModeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Operate.SystemConfig.StopRemoteMGT(this.RunMode);
            //Operate.SystemConfig.SaveSystemList_ToDB();
            //Operate.SystemConfig.SaveInjectMode_ToDB();
            //Operate.ProxyConfig.Account.SaveProxyAccountList_ToDB(this.RunMode);
            //Operate.ProxyConfig.Mapping.SaveProxyMapLocal_ToDB(this.RunMode);
            //Operate.ProxyConfig.Mapping.SaveProxyMapRemote_ToDB(this.RunMode);
        }

        private void InitForm()
        {
            this.Text = "WPE x64 - " + AntdUI.Localization.Get("ProxyModeForm", "代理模式");
            this.pageHeader.Text = "Winsock Packet Editor";
            this.pageHeader.SubText = Operate.SystemConfig.AssemblyVersion;

            this.mProxyMode.Collapsed = true;
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

            for (int i = 0; i < this.mProxyMode.Items.Count; i++)
            {
                this.mProxyMode.Items[i].BadgeBack = this.colorTheme.Value;
            }

            Operate.DoLog(MethodBase.GetCurrentMethod().Name, this.lProcessName.Text);
        }

        #endregion

        #region//更换主题颜色

        private void colorTheme_ValueChanged(object sender, AntdUI.ColorEventArgs e)
        {
            setcolor = true;
            AntdUI.Style.SetPrimary(e.Value);

            for (int i = 0; i < this.mProxyMode.Items.Count; i++)
            {
                this.mProxyMode.Items[i].BadgeBack = e.Value;
            }

            Refresh();
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
            if (setcolor)
            {
                var color = AntdUI.Style.Db.Primary;
                AntdUI.Style.SetPrimary(color);
            }

            Dark = AntdUI.Config.IsDark;
            btn_mode.Toggle = Dark;

            if (Dark)
            {
                BackColor = Color.FromArgb(30, 30, 30);
                ForeColor = Color.White;
            }
            else
            {
                BackColor = Color.White;
                ForeColor = Color.Black;
            }
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
                this.Text = "WPE x64 - " + AntdUI.Localization.Get("ProxyModeForm", "代理模式");
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
            this.mProxyMode.Collapsed = !this.mProxyMode.Collapsed;
            this.MenuCollapseChange();
        }

        private void MenuCollapseChange()
        {
            if (this.mProxyMode.Collapsed)
            {
                this.mProxyMode.Width = this.tlpMenu.Width = this.mProxyMode.CollapseWidth;
                this.bMenuCollapse.IconSvg = "MenuUnfoldOutlined";
            }
            else
            {
                this.mProxyMode.Width = this.tlpMenu.Width = this.mProxyMode.CollapsedWidth;
                this.bMenuCollapse.IconSvg = "MenuFoldOutlined";
            }
        }

        private void mProxyMode_SelectChanged(object sender, AntdUI.MenuSelectEventArgs e)
        {
            AntdUI.MenuItem miSelect = e.Value;

            switch (miSelect.ID)
            {
                case "miProxyList":
                    this.tabProxyMode.SelectTab("tpProxyList");
                    break;

                case "miClientList":
                    this.tabProxyMode.SelectTab("tpClientList");
                    break;

                case "miAccountList":
                    this.tabProxyMode.SelectTab("tpAccountList");
                    break;             

                case "miStatistical":
                    this.tabProxyMode.SelectTab("tpStatistical");                    
                    break;

                case "miSystemLog":
                    this.tabProxyMode.SelectTab("tpSystemLog");
                    break;
            }
        }

        #endregion

        #region//代理管理 - 菜单

        private void sProxyList_SelectIndexChanged(object sender, AntdUI.IntEventArgs e)
        {
            switch (this.sProxyList.SelectIndex)
            {
                //代理设置
                case 0:
                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new ProxySettingsForm())
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });
                    break;                

                //列表设置
                case 1:
                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new ListSettingsForm())
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });
                    break;

                //映射设置
                case 2:
                    //AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new HotKeyForm())
                    //{
                    //    Align = AntdUI.TAlignMini.Right,
                    //    Mask = true,
                    //    MaskClosable = false,
                    //    DisplayDelay = 0,
                    //});
                    break;

                //外部代理
                case 3:
                    //AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new BackUpSettingsForm())
                    //{
                    //    Align = AntdUI.TAlignMini.Right,
                    //    Mask = true,
                    //    MaskClosable = false,
                    //    DisplayDelay = 0,
                    //});
                    break;

                //系统设置
                case 4:
                    //AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new SystemSettingsForm(this))
                    //{
                    //    Align = AntdUI.TAlignMini.Right,
                    //    Mask = true,
                    //    MaskClosable = false,
                    //    DisplayDelay = 0,
                    //});
                    break;

                //清空数据
                case 5:                    

                    break;

                //代理
                case 6:

                    //if (this.StartHook)
                    //{
                    //    this.sPacketList.Items[8].IconSvg = "StopOutlined";
                    //    this.sPacketList.Items[8].Text = AntdUI.Localization.Get("InjectModeForm.StopHook", "停止拦截");
                    //    this.StartHook = false;

                    //    this.Start_Hook();
                    //}
                    //else
                    //{
                    //    this.sPacketList.Items[8].IconSvg = "PlayCircleFilled";
                    //    this.sPacketList.Items[8].Text = AntdUI.Localization.Get("InjectModeForm.StartHook", "开始拦截");
                    //    this.StartHook = true;

                    //    this.Stop_Hook();
                    //}

                    break;
            }

            this.sProxyList.SelectIndex = -1;
        }

        #endregion
    }
}
