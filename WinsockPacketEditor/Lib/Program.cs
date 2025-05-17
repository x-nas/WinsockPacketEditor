using System;
using System.Windows.Forms;
using WPELibrary;
using WPELibrary.Lib;
using WPELibrary.Lib.NativeMethods;

namespace WinsockPacketEditor
{
    static class Program
    {
        public static int PID = -1;
        public static string PNAME = string.Empty;
        public static string PATH = string.Empty;        

        #region//主函数

        [STAThread]

        static void Main()
        {
            try
            {
                if (Environment.OSVersion.Version.Major >= 6)
                {
                    User32.SetProcessDPIAware();
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);             
               
                System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);

                Socket_Cache.DataBase.InitDB();
                Socket_Cache.System.LoadSystemConfig_FromDB();

                if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
                {
                    SystemMode_Form systemMode_Form = new SystemMode_Form();

                    if (systemMode_Form.ShowDialog() == DialogResult.OK)
                    {
                        switch (Socket_Cache.System.StartMode)
                        { 
                            case Socket_Cache.System.SystemMode.Proxy:                                

                                Socket_Form socket_Form = new Socket_Form();
                                socket_Form.Show();

                                Application.Run(new SocketProxy_Form(socket_Form));

                                break;

                            case Socket_Cache.System.SystemMode.Process:        
                                
                                Application.Run(new Injector_Form());

                                break;
                        }                        
                    }
                }
                else
                {                    
                    System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                    startInfo.UseShellExecute = true;
                    startInfo.WorkingDirectory = Environment.CurrentDirectory;
                    startInfo.FileName = Application.ExecutablePath;
                    
                    startInfo.Verb = "runas";

                    try
                    {
                        System.Diagnostics.Process.Start(startInfo);
                    }
                    catch
                    {
                        return;
                    }
                   
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                string sError = ex.Message;
            }            
        }

        #endregion        
    }
}
