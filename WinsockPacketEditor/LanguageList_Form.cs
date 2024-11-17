using WinsockPacketEditor.Lib;
using System.Diagnostics;
using System.Windows.Forms;
using WPELibrary.Lib;

namespace WinsockPacketEditor
{
    public partial class LanguageList_Form : Form
    {
        #region//初始化

        public LanguageList_Form()
        {
            InitializeComponent();

            this.InitSelectLanguage();                    
        }

        private void InitSelectLanguage()
        {
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

        #region//重启程序

        private void LanguageList_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                string SelectLanguage = string.Empty;

                if (this.rb_zhCN.Checked)
                {
                    SelectLanguage = "zh-CN";
                }
                else if (this.rb_enUS.Checked)
                {
                    SelectLanguage = "en-US";
                }

                if (!Properties.Settings.Default.DefaultLanguage.Equals(SelectLanguage))
                {
                    Process_Injector.SetDefaultLanguage(SelectLanguage);

                    Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_12));

                    Application.Restart();
                    Process.GetCurrentProcess().Kill();
                }
            }
            catch
            { 
                //
            }            
        }

        #endregion
    }
}
