using AntdUI;
using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;
using WPE.InjectMode;
using WPE.Lib;
using WPE.Lib.NativeMethods;

namespace WinsockPacketEditor
{
    static class Program
    {
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

                Operate.DataBase.InitDB();
                Operate.SystemConfig.LoadSystemConfig_FromDB();                

                System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
                System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);

                if (principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator))
                {
                    StartForm startForm = new StartForm();

                    if (startForm.ShowDialog() == DialogResult.OK)
                    {
                        switch (Operate.SystemConfig.StartMode)
                        { 
                            case Operate.SystemConfig.SystemMode.Proxy:

                                ThreadPool.SetMinThreads(100, 100);
                                ThreadPool.SetMaxThreads(Environment.ProcessorCount * 2, 1000);

                                InjectModeForm imForm = new InjectModeForm();
                                imForm.Show();

                                Application.Run(new SocketProxy_Form(imForm));

                                break;

                            case Operate.SystemConfig.SystemMode.Process:

                                //Application.Run(new InjectMode_Form());

                                break;
                        }                        
                    }
                }
                else
                {
                    #region//如果没有管理员权限，则重新以管理员身份运行

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

                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "错误 Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);                
            }            
        }

        #endregion        
    }
}
