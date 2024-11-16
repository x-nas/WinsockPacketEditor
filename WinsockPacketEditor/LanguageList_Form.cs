using WinsockPacketEditor.Lib;
using System.Diagnostics;
using System.Windows.Forms;
using WPELibrary.Lib;

namespace WinsockPacketEditor
{
    public partial class LanguageList_Form : Form
    {
        private string SelectLanguage = "zh-CN";

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
            this.SelectLanguage = "zh-CN";
        }

        private void rb_enUS_Click(object sender, System.EventArgs e)
        {
            this.SelectLanguage = "en-US";
        }

        #endregion

        #region//重启程序

        private void LanguageList_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
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
