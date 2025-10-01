using AntdUI;
using EasyHook;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class ProcessList : UserControl
    {        
        private IntPtr ipMouseHook = IntPtr.Zero;
        private IntPtr ipKeyHook = IntPtr.Zero;
        private IntPtr lastHoverHwnd = IntPtr.Zero;
        private bool isPickingWindow = false;
        private Form form = null;
        private Form currentTooltip;
        private Timer tooltipTimer;
        private Point pendingScreenPoint;
        private string pendingText;
        private User32.HookProc mProc;
        private User32.HookProc kProc;
        private List<ProcessInfo> processList = new List<ProcessInfo>();

        #region//窗体事件

        public ProcessList(Form form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void ProcessList_Load(object sender, EventArgs e)
        {
            this.InitProcessList();
            this.InitLastInjection();
            this.Dark_Changed();
            this.ShowProcessList();
        }

        private void InitProcessList()
        {
            tProcessList.Columns = new AntdUI.ColumnCollection {
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

        protected override void OnHandleDestroyed(EventArgs e)
        {
            this.UnHook();
            base.OnHandleDestroyed(e);
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
            this.tProcessList.PauseLayout = true;

            AntdUI.Spin.open(this.tProcessList, new AntdUI.Spin.Config()
            {
                Radius = 6,
                Font = new Font("Microsoft YaHei UI", 9F),
            }, (config) =>
            {
                config.Text = AntdUI.Localization.Get("Loading", "正在加载...");
                processList = Operate.ProcessConfig.GetProcessList();
            }, () =>
            {
                this.tProcessList.DataSource = processList;
                this.tProcessList.SelectedIndex = -1;
                this.tProcessList.PauseLayout = false;
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

        #region//选择窗体        

        private void bSelectForm_Click(object sender, EventArgs e)
        {
            try
            {
                this.isPickingWindow = true;

                this.mProc = new User32.HookProc(MouseHook);
                this.kProc = new User32.HookProc(KeyBoardHook);

                if (this.mProc != null)
                {
                    using (Process curProcess = Process.GetCurrentProcess())
                    using (ProcessModule curModule = curProcess.MainModule)
                    {
                        this.ipMouseHook = User32.SetWindowsHookEx(User32.WH_MOUSE_LL, this.mProc, Kernel32.GetModuleHandle(curModule.ModuleName), 0);
                    }
                }

                if (this.kProc != null)
                {
                    using (Process curProc = Process.GetCurrentProcess())
                    using (ProcessModule curMod = curProc.MainModule)
                    {
                        this.ipKeyHook = User32.SetWindowsHookEx(User32.WH_KEYBOARD_LL, this.kProc, Kernel32.GetModuleHandle(curMod.ModuleName), 0);
                    }
                }

                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "请选择一个窗体", TType.Success)
                {
                    LocalizationText = "ProcessList.SelectForm"
                });
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(bSelectForm_Click), ex.Message);
                this.UnHook();
            }
        }

        private IntPtr MouseHook(int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (nCode >= 0 && this.isPickingWindow)
                {
                    User32.MSLLHOOKSTRUCT hookStruct = Marshal.PtrToStructure<User32.MSLLHOOKSTRUCT>(lParam);
                    User32.POINT point = hookStruct.pt;

                    if (wParam == (IntPtr)User32.WM_MOUSEMOVE)
                    {
                        IntPtr hWnd = User32.WindowFromPoint(point);
                        if (hWnd != IntPtr.Zero && hWnd != lastHoverHwnd)
                        {
                            lastHoverHwnd = hWnd;

                            int processId;
                            User32.GetWindowThreadProcessId(hWnd, out processId);

                            string processInfo = GetProcessInfo(processId, hWnd);

                            this.BeginInvoke((MethodInvoker)delegate {
                                ShowProcessToolTip(new Point(point.x, point.y), processInfo);
                            });
                        }
                    }

                    if (wParam == (IntPtr)User32.WM_LBUTTONDOWN)
                    {
                        IntPtr hWnd = User32.WindowFromPoint(point);
                        if (hWnd != IntPtr.Zero)
                        {
                            this.BeginInvoke((MethodInvoker)delegate {
                                this.isPickingWindow = false;
                                this.UnHook();

                                int processId;
                                User32.GetWindowThreadProcessId(hWnd, out processId);

                                var proc = Process.GetProcessById(processId);
                                if (proc != null)
                                {
                                    Operate.SystemConfig.PID = processId;
                                    Operate.SystemConfig.PNAME = proc.ProcessName;

                                    this.ShowSelectProcess();
                                    this.DoInject();
                                }
                            });
                        }
                    }
                }

                return User32.CallNextHookEx(this.ipMouseHook, nCode, wParam, lParam);
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(MouseHook), ex.Message);
            }

            return IntPtr.Zero;
        }

        private IntPtr KeyBoardHook(int nCode, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (nCode >= 0 && this.isPickingWindow)
                {
                    var kb = Marshal.PtrToStructure<User32.KBDLLHOOKSTRUCT>(lParam);
                    if (kb.vkCode == (uint)Keys.Escape)
                    {
                        this.BeginInvoke((MethodInvoker)delegate
                        {
                            UnHook();
                        });

                        return (IntPtr)1;
                    }
                }
                return User32.CallNextHookEx(ipKeyHook, nCode, wParam, lParam);
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(KeyBoardHook), ex.Message);
            }

            return User32.CallNextHookEx(ipKeyHook, nCode, wParam, lParam);
        }

        private string GetProcessInfo(int processId, IntPtr hWnd)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            try
            {
                Process process = Process.GetProcessById(processId);
                string windowTitle = GetWindowTitle(hWnd);

                sb.Append(AntdUI.Localization.Get("ProcessList.Process", "进程 : ")).AppendLine(process.ProcessName)
                  .Append(AntdUI.Localization.Get("ProcessList.PID", "PID : ")).AppendLine(processId.ToString())
                  .Append(AntdUI.Localization.Get("ProcessList.Title", "标题 : ")).AppendLine(windowTitle)
                  .Append(AntdUI.Localization.Get("ProcessList.Path", "路径 : ")).AppendLine(process.MainModule?.FileName ?? string.Empty);
            }
            catch
            {
                sb.Append(AntdUI.Localization.Get("ProcessList.PID", "PID : ")).AppendLine(processId.ToString())
                  .Append(AntdUI.Localization.Get("ProcessList.Handle", "窗口句柄 : ")).AppendLine(hWnd.ToString())
                  .AppendLine(AntdUI.Localization.Get("ProcessList.NoInfo", "(无法获取详细信息)"));
            }

            return sb.ToString();
        }

        private string GetWindowTitle(IntPtr hWnd)
        {
            try
            {
                System.Text.StringBuilder title = new System.Text.StringBuilder(256);
                if (User32.GetWindowText(hWnd, title, title.Capacity) > 0)
                {
                    return title.ToString();
                }
            }
            catch
            {
                // 忽略错误
            }

            return AntdUI.Localization.Get("ProcessList.NoTitle", "无标题");
        }

        private void ShowProcessToolTip(Point screenPoint, string text)
        {
            try
            {
                tooltipTimer?.Stop();

                if (currentTooltip != null && !currentTooltip.IsDisposed)
                {
                    currentTooltip.Close();
                    currentTooltip = null;
                }

                pendingScreenPoint = screenPoint;
                pendingText = text;

                if (tooltipTimer == null)
                {
                    tooltipTimer = new Timer();
                    tooltipTimer.Interval = 200;
                    tooltipTimer.Tick += TooltipTimer_Tick;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(ShowProcessToolTip), ex.Message);
            }
            finally
            {
                tooltipTimer.Start();
            }
        }

        private void TooltipTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                tooltipTimer?.Stop();

                Point formPoint = this.PointToClient(pendingScreenPoint);

                var config = new AntdUI.Tooltip.Config(this, pendingText)
                {
                    Offset = new Rectangle(formPoint.X, formPoint.Y, 0, 0),
                    ArrowAlign = TAlign.Top,
                    Font = this.Font
                };

                currentTooltip = AntdUI.Tooltip.open(config);
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(TooltipTimer_Tick), ex.Message);
            }            
        }

        private void UnHook()
        {
            try
            {
                tooltipTimer?.Stop();
                tooltipTimer?.Dispose();
                tooltipTimer = null;

                currentTooltip?.Close();
                currentTooltip = null;

                if (this.ipMouseHook != IntPtr.Zero)
                {
                    User32.UnhookWindowsHookEx(this.ipMouseHook);
                    this.ipMouseHook = IntPtr.Zero;
                }

                if (ipKeyHook != IntPtr.Zero)
                {
                    User32.UnhookWindowsHookEx(this.ipKeyHook);
                    ipKeyHook = IntPtr.Zero;
                }

                this.lastHoverHwnd = IntPtr.Zero;
                this.isPickingWindow = false;
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(UnHook), ex.Message);
            }
        }

        #endregion

        #region//选择程序

        private void bCreate_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofdCreate = new OpenFileDialog();

            ofdCreate.Title = AntdUI.Localization.Get("ProcessList.SelectProgram", "请选择要注入的应用程序");
            ofdCreate.Multiselect = false;
            ofdCreate.InitialDirectory = string.Empty;
            ofdCreate.Filter = AntdUI.Localization.Get("ProcessList.ProgramFilter", "可执行文件 (*.exe)|*.exe|所有文件 (*.*)|*.*");
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
                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "请选择一个进程或程序", TType.Error)
                {
                    LocalizationText = "ProcessList.txtSelectProcess"
                });

                return;
            }

            this.DoInject();
        }

        private void DoInject()
        {
            try
            {
                string channelName = "WPE64";
                string injectionLibrary_x86 = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), Operate.SystemConfig.WPE64_DLL);
                string injectionLibrary_x64 = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), Operate.SystemConfig.WPE64_DLL);

                if (Operate.SystemConfig.PID > -1)
                {
                    RemoteHooking.Inject(Operate.SystemConfig.PID, injectionLibrary_x86, injectionLibrary_x64, channelName);
                }
                else
                {
                    RemoteHooking.CreateAndInject(Operate.SystemConfig.PATH, string.Empty, 0, injectionLibrary_x86, injectionLibrary_x64, out Operate.SystemConfig.PID, channelName);
                }

                Operate.SystemConfig.LastInjection = Operate.SystemConfig.PNAME;
                Operate.SystemConfig.StartMode = Operate.SystemConfig.SystemMode.Process;
                Operate.SystemConfig.SaveSystemConfig_LastInjection_ToDB();

                this.Dispose();
            }
            catch (Exception ex)
            {
                AntdUI.Modal.open(new AntdUI.Modal.Config(this.form, AntdUI.Localization.Get("ProcessList.InjectError", "注入失败"), "\r\n" + ex.Message + "\r\n\r\n")
                {
                    Icon = TType.Error,
                    CloseIcon = true,
                    Keyboard = false,
                    MaskClosable = false,
                    CancelText = null,
                    OkText = AntdUI.Localization.Get("ProcessList.SearchOnWebSite", "查询 WPE64.com"),
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

        #region//退出

        private void bExit_Click(object sender, EventArgs e)
        {
            this.UnHook();
            this.Dispose();
        }

        #endregion        
    }
}
