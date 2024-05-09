using ProcessInjector.Lib;
using System.Diagnostics;
using System.Windows.Forms;
using WPELibrary.Lib;

namespace ProcessInjector
{
    public partial class LanguageList_Form : Form
    {
        #region//初始化
        public LanguageList_Form()
        {
            InitializeComponent();

            try
            {
                switch (Properties.Settings.Default.DefaultLanguage)
                {
                    case "zh-CN":
                        this.rb_zhCN.Checked = true;
                        break;

                    case "en-US":
                        this.rb_enUS.Checked = true;
                        break;

                    default:
                        break;
                }
            }
            catch
            {
                //
            }        
        }
        #endregion

        #region//选择语言
        private void rb_zhCN_Click(object sender, System.EventArgs e)
        {            
            Process_Injector.SetDefaultLanguage("zh-CN");          
        }

        private void rb_enUS_Click(object sender, System.EventArgs e)
        {
            Process_Injector.SetDefaultLanguage("en-US");
        }
        #endregion

        #region//重启程序
        private void LanguageList_Form_FormClosed(object sender, FormClosedEventArgs e)
        {            
            Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_12));

            Application.Restart();
            Process.GetCurrentProcess().Kill();
        }
        #endregion
    }
}
