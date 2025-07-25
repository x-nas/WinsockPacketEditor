using EasyHook;
using System;
using System.Reflection;
using WinsockPacketEditor;
using System.Windows.Forms;

namespace WPEHook
{
    public class Hook : IEntryPoint
    {
        #region//EasyHook        

        public Hook()
        {
            //
        }

        public Hook(RemoteHooking.IContext InContext, string ChannelName)
        {
            //
        }

        public unsafe void Run(RemoteHooking.IContext InContext, string ChannelName)
        {
            try
            {
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    User32.SetProcessDPIAware();
                }

                Operate.SystemConfig.LoadSystemConfig_FromDB();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new InjectModeForm());
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion
    }
}
