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
