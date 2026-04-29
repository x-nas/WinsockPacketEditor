using System;
using System.Windows.Forms;

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
                if (!Operate.SystemConfig.IsAdministrator())
                {
                    Operate.SystemConfig.RestartAsAdmin();
                    return;
                }

                if (Environment.OSVersion.Version.Major >= 6)
                {
                    User32.SetProcessDPIAware();
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                Operate.DataBase.InitDB();
                Operate.SystemConfig.LoadSystemConfig_FromDB();

                StartForm sfForm = new StartForm();
                if (sfForm.ShowDialog() == DialogResult.OK)
                {
                    if (Operate.SystemConfig.SelectMode == Operate.SystemConfig.SystemMode.Proxy)
                    {
                        Application.Run(new ProxyModeForm());
                    }
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
