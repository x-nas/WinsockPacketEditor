using WinsockPacketEditor.Lib;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    partial class AboutFrom : Form
    {
        public AboutFrom()
        {
            InitializeComponent();           

            this.InitFrom();
        }

        #region//初始化关于信息

        private void InitFrom()
        { 
            this.labelVersion.Text = Process_Injector.AssemblyVersion;
            this.labelCopyright.Text = Process_Injector.AssemblyCopyright;
            this.labelCompanyName.Text = Process_Injector.AssemblyCompany;
        }

        #endregion

        #region//打开超链接

        //网站
        private void llSetup_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.x-nas.com:8888/WPE/index.html");
        }      

        //Github
        private void llGitHub_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/x-nas/WinsockPacketEditor");
        }

        //Gitee
        private void llGitee_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://gitee.com/X-NAS/WinsockPacketEditor");
        }

        #endregion      
    }
}
