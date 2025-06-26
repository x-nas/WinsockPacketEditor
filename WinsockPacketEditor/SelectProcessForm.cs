using AntdUI;
using EasyHook;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using WPE.Lib;

namespace WinsockPacketEditor
{
    public partial class SelectProcessForm : AntdUI.Window
    {
        private List<ProcessInfo> processList = new List<ProcessInfo>();

        #region//窗体加载

        public SelectProcessForm()
        {
            InitializeComponent();
            this.InitIsDark();
            this.InitTableColumns();
            this.InitLastInjection();
        }

        private void SelectProcessForm_Load(object sender, EventArgs e)
        {
            this.Text = "WPE x64 - " + AntdUI.Localization.Get("SelectProcessForm", "选择进程");
            this.pageHeader.Text = "WPE x64";
            this.pageHeader.SubText = Operate.SystemConfig.AssemblyVersion;            
            
            this.ShowProcessList();
        }

        private void SelectProcessForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Operate.SystemConfig.SaveSystemConfig_LastInjection_ToDB();
        }

        private void InitTableColumns()
        {
            tProcessList.Columns = new AntdUI.ColumnCollection {
                new AntdUI.Column("ICO", string.Empty, AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.ProcessList.Column."),
                new AntdUI.Column("ProcessName", "进程名称").SetSortOrder().SetLocalizationTitleID("Table.ProcessList.Column."),
                new AntdUI.Column("ProcessID", "进程编号", AntdUI.ColumnAlign.Center).SetSortOrder().SetLocalizationTitleID("Table.ProcessList.Column."),
                new AntdUI.Column("ProcessPath", "路径").SetLocalizationTitleID("Table.ProcessList.Column."),
            };
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

        #region//初始化上次注入信息

        private void InitLastInjection()
        {
            if (!string.IsNullOrEmpty(Operate.SystemConfig.LastInjection))
            {
                Process[] plProcess = Process.GetProcessesByName(Operate.SystemConfig.LastInjection);

                if (plProcess.Length > 0)
                {
                    Operate.SystemConfig.PID = plProcess[0].Id;
                    Operate.SystemConfig.PNAME = plProcess[0].ProcessName;

                    this.ShowSelectProcess();
                }
            }
        }

        #endregion

        #region//显示所有进程

        private void ShowProcessList()
        {
            AntdUI.Spin.open(this, new AntdUI.Spin.Config()
            {
                Radius = 6,
                Font = new Font("Microsoft YaHei UI", 12f),
            }, (config) =>
            {
                config.Text = AntdUI.Localization.Get("Loading", "正在加载...");
                processList = Operate.ProcessConfig.GetProcessList();
            }, () =>
            {
                this.tProcessList.DataSource = processList;
                this.tProcessList.SelectedIndex = -1;
            });
        }

        #endregion

        #region//显示选中的进程或者程序

        private void ShowSelectProcess()
        {
            if (Operate.SystemConfig.PID != -1 && Operate.SystemConfig.PNAME != string.Empty)
            {
                this.txtSelectProcess.Text = Operate.SystemConfig.PNAME + " [" + Operate.SystemConfig.PID + "]";
            }
            else if (Operate.SystemConfig.PID == -1 && !string.IsNullOrEmpty(Operate.SystemConfig.PNAME) && !string.IsNullOrEmpty(Operate.SystemConfig.PATH))
            {
                this.txtSelectProcess.Text = Operate.SystemConfig.PNAME;
            }
        }

        private void txtSelectProcess_TextChanged(object sender, EventArgs e)
        {
            string selectedProcess = this.txtSelectProcess.Text.Trim();
            if (string.IsNullOrEmpty(selectedProcess))
            {
                this.txtSelectProcess.Status = TType.Error;
            }
            else
            {
                this.txtSelectProcess.Status = TType.Success;
            }
        }

        #endregion

        #region//选择进程

        private void tProcessList_CellClick(object sender, TableClickEventArgs e)
        {
            int selectedIndex = tProcessList.SelectedIndex;
            if (selectedIndex > 0)
            {
                var row = tProcessList[selectedIndex - 1];
                if (row != null)
                {
                    Operate.SystemConfig.PID = (int)row["ProcessID"];
                    Operate.SystemConfig.PNAME = row["ProcessName"].ToString();

                    this.ShowSelectProcess();
                }
            }
        }

        #endregion

        #region//筛选进程

        private void txtSearchProcess_TextChanged(object sender, EventArgs e)
        {
            string sSearchText = this.txtSearchProcess.Text.Trim();
            if (string.IsNullOrEmpty(sSearchText))
            {
                this.tProcessList.DataSource = this.processList;
            }
            else
            {
                this.tProcessList.DataSource = processList
                    .Where(p => p.ProcessName.IndexOf(sSearchText, StringComparison.OrdinalIgnoreCase) >= 0)
                    .ToList();
                this.tProcessList.SelectedIndex = -1;
            }
        }

        #endregion        

        #region//选择程序

        private void bCreate_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdCreate = new OpenFileDialog();

            ofdCreate.Title = AntdUI.Localization.Get("SelectProcessForm.SelectProgram", "请选择要注入的应用程序");
            ofdCreate.Multiselect = false;
            ofdCreate.InitialDirectory = string.Empty;
            ofdCreate.Filter = AntdUI.Localization.Get("SelectProcessForm.ProgramFilter", "可执行文件 (*.exe)|*.exe|所有文件 (*.*)|*.*");
            ofdCreate.ShowDialog();

            Operate.SystemConfig.PID = -1;
            Operate.SystemConfig.PATH = ofdCreate.FileName;
            Operate.SystemConfig.PNAME = Path.GetFileName(Operate.SystemConfig.PATH);

            this.ShowSelectProcess();
        }

        #endregion

        #region//刷新

        private void bRefresh_Click(object sender, EventArgs e)
        {
            this.txtSelectProcess.Text = string.Empty;
            this.txtSearchProcess.Text = string.Empty;

            this.ShowProcessList();
        }        

        #endregion

        #region//注入

        private void bInject_Click(object sender, EventArgs e)
        {
            string selectedProcess = this.txtSelectProcess.Text.Trim();
            if (string.IsNullOrEmpty(selectedProcess))
            {
                AntdUI.Message.open(new AntdUI.Message.Config(this, "请选择一个进程或程序", TType.Error)
                {
                    LocalizationText = "SelectProcessForm.txtSelectProcess"
                });

                return;
            }

            try
            {
                string channelName = "WPE64";
                string injectionLibrary_x86 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Operate.SystemConfig.WPE64_DLL);
                string injectionLibrary_x64 = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Operate.SystemConfig.WPE64_DLL);

                var parameters = new Operate.SystemConfig.InjectionParameters
                {
                    Animation = AntdUI.Config.Animation,
                    ShadowEnabled = AntdUI.Config.ShadowEnabled,
                    ShowInWindow = AntdUI.Config.ShowInWindow,
                    ScrollBarHide = AntdUI.Config.ScrollBarHide,
                    TextRenderingHighQuality = AntdUI.Config.TextRenderingHighQuality,
                    Dark = AntdUI.Config.IsDark,
                    Lang = AntdUI.Localization.CurrentLanguage,
                };

                if (Operate.SystemConfig.PID > -1)
                {
                    RemoteHooking.Inject(Operate.SystemConfig.PID, injectionLibrary_x86, injectionLibrary_x64, channelName, parameters);
                }
                else
                {
                    RemoteHooking.CreateAndInject(Operate.SystemConfig.PATH, string.Empty, 0, injectionLibrary_x86, injectionLibrary_x64, out Operate.SystemConfig.PID, channelName, parameters);
                }

                Operate.SystemConfig.LastInjection = Operate.SystemConfig.PNAME;
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                AntdUI.Modal.open(new AntdUI.Modal.Config(this, AntdUI.Localization.Get("SelectProcessForm.InjectError", "注入失败"), "\r\n" + ex.Message + "\r\n\r\n")
                {
                    Icon = TType.Error,                    
                    CloseIcon = true,
                    Keyboard = false,
                    MaskClosable = false,
                    CancelText = null,
                    OkText = AntdUI.Localization.Get("SelectProcessForm.SearchOnWebSite", "查询 WPE64.com"),
                    OnButtonStyle = (id, btn) =>
                    {
                        btn.BackExtend = "135, #6253E1, #04BEFE";
                    },
                    OnOk = config =>
                    {
                        var lang = AntdUI.Localization.CurrentLanguage;
                        if (lang.StartsWith("en"))
                        {
                            Process.Start(Operate.SystemConfig.WebSite_Tutorials_EN);
                        }
                        else
                        {
                            Process.Start(Operate.SystemConfig.WebSite_Tutorials_CN);
                        }
                        
                        return false;
                    }
                });
            }
        }

        #endregion        
    }
}
