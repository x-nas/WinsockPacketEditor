using EasyHook;
using System;
using System.Windows.Forms;
using WinsockPacketEditor;

namespace WPEHook
{
    public class Hook : IEntryPoint
    {
        #region//EasyHook        

        public Hook()
        {
            //
        }

        public Hook(RemoteHooking.IContext InContext, string ChannelName, Operate.SystemConfig.InjectionParameters ipParameters)
        {
            //
        }

        public void Run(RemoteHooking.IContext InContext, string ChannelName, Operate.SystemConfig.InjectionParameters ipParameters)
        {
            try
            {
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    User32.SetProcessDPIAware();
                }

                if (ipParameters != null)
                {
                    string DBPath = ipParameters.DataBasePath;
                    if (!string.IsNullOrEmpty(DBPath))
                    {
                        Operate.DataBase.dbPath = DBPath;
                    }
                }

                Operate.SystemConfig.LoadSystemConfig_FromDB();

                //配置只落到 UI.Prefs，这里把主题与语言真正应用到 AntdUI（必须早于任何窗体创建）
                WinFormsUiHost.ApplyAll();

                //登记密码框等表单弹窗的渲染方式，否则 UI.Prompt 取不到工厂、一律返回 null
                UiDialogs.RegisterPrompts();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new InjectModeForm());
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(Run), ex);
            }
        }

        #endregion
    }
}
