using System;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    static class Program
    {
        public static int PID = -1;
        public static string PNAME = string.Empty;
        public static string PATH = string.Empty;
        
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);             
               
                System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
                
                if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
                {
                    Application.Run(new Injector_Form());
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
    }
}
