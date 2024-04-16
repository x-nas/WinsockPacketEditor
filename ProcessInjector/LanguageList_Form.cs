using ProcessInjector.Lib;
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
        #endregion

        #region//选择语言
        private void rb_zhCN_Click(object sender, System.EventArgs e)
        {            
            Process_Injector.SetDefaultLanguage("zh-CN");
            this.LoadAll();            
        }

        private void rb_enUS_Click(object sender, System.EventArgs e)
        {
            Process_Injector.SetDefaultLanguage("en-US");
            this.LoadAll();
        }
        #endregion

        #region//加载所有窗体语言
        private void LoadAll()
        {
            foreach (Form form in Application.OpenForms)
            {
                switch (form.Name)
                {
                    case "Injector_Form":
                        MultiLanguage.LoadLanguage(form, typeof(Injector_Form));
                        break;

                    case "LanguageList_Form":
                        MultiLanguage.LoadLanguage(form, typeof(LanguageList_Form));
                        break;                   

                    default:
                        break;
                }
            }
        }
        #endregion
    }
}
