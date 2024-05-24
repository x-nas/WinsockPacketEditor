using ProcessInjector.Lib;
using System;
using System.Windows.Forms;
using WPELibrary.Lib;

namespace ProcessInjector
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
            this.Text = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_3);

            this.labelProductName.Text = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_23);

            this.labelVersion.Text = String.Format("版本 {0}", Process_Injector.AssemblyVersion);

            this.labelCopyright.Text = Process_Injector.AssemblyCopyright;

            this.labelCompanyName.Text = Process_Injector.AssemblyCompany;

            this.rtbDescription.Text = "";

            //安装下载
            this.rtbDescription.Text += MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_24);
            this.rtbDescription.Text += "https://www.x-nas.com/wpe/publish.htm" + "\r\n";

            //使用说明
            this.rtbDescription.Text += MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_25);
            this.rtbDescription.Text += "https://www.52pojie.cn/thread-1446781-1-1.html" + "\r\n";

            //Github
            this.rtbDescription.Text += "GitHub: ";
            this.rtbDescription.Text += "https://github.com/x-nas/WinPacketsEdit" + "\r\n";

            //Gitee
            this.rtbDescription.Text += "Gitee: ";
            this.rtbDescription.Text += "https://gitee.com/X-NAS/WinPacketsEdit" + "\r\n";

            //联系作者
            this.rtbDescription.Text += MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_26);
            this.rtbDescription.Text += "MailTo:263659@qq.com" + "\r\n";
        }

        #endregion

        #region//打开超链接

        private void rtbDescription_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }

        #endregion
    }
}
