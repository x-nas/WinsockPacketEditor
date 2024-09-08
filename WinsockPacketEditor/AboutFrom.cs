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

        //在线安装版
        private void llSetup_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.x-nas.com/wpe/publish.htm");
        }

        //离线打包版
        private void llOffline_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://pan.baidu.com/s/1xB8TT395PeIpCjP5bIu42Q?pwd=wspe");
        }

        //使用说明
        private void llInstructions_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://pan.baidu.com/s/1jbCv9fhO25WnWSF2kB078w?pwd=wspe");
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

        //联系作者
        private void llContact_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("MailTo:263659@qq.com");
        }

        #endregion      
    }
}
