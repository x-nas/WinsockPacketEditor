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

                //配置只落到 UI.Prefs，这里把主题与语言真正应用到 AntdUI（必须早于任何窗体创建）
                WinFormsUiHost.ApplyAll();

                //登记密码框等表单弹窗的渲染方式，否则 UI.Prompt 取不到工厂、一律返回 null
                UiDialogs.RegisterPrompts();

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
