using System;
using System.Threading;
using System.Windows.Forms;

namespace WPELibrary.Lib
{
    //用于编写与切换语言相关的变量和代码
    public static class MultiLanguage
    {
        //当前默认语言
        public static string DefaultLanguage = "zh-CN";        

        /// <summary>
        /// 加载语言
        /// </summary>
        /// <param name="form">加载语言的窗口</param>
        /// <param name="formType">窗口的类型</param>
        public static void LoadLanguage(Form form, Type formType)
        {
            if (form != null)
            {
                System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(formType);
                resources.ApplyResources(form, "$this");
                Loading(form, resources);
            }
        }

        /// <summary>
        /// 加载语言
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="resources">语言资源</param>
        private static void Loading(Control control, System.ComponentModel.ComponentResourceManager resources)
        {
            if (control is MenuStrip)
            {
                //将资源与控件对应
                resources.ApplyResources(control, control.Name);
                MenuStrip ms = (MenuStrip)control;
                if (ms.Items.Count > 0)
                {
                    foreach (ToolStripMenuItem c in ms.Items)
                    {
                        //遍历菜单
                        Loading(c, resources);
                    }
                }
            }

            foreach (Control c in control.Controls)
            {
                resources.ApplyResources(c, c.Name);
                Loading(c, resources);
            }
        }


        /// <summary>
        /// 遍历菜单
        /// </summary>
        /// <param name="item">菜单项</param>
        /// <param name="resources">语言资源</param>
        private static void Loading(ToolStripMenuItem item, System.ComponentModel.ComponentResourceManager resources)
        {
            if (item is ToolStripMenuItem)
            {
                resources.ApplyResources(item, item.Name);
                ToolStripMenuItem tsmi = (ToolStripMenuItem)item;
                if (tsmi.DropDownItems.Count > 0)
                {
                    foreach (ToolStripMenuItem c in tsmi.DropDownItems)
                    {
                        Loading(c, resources);
                    }
                }
            }
        }

        public static void SetDefaultLanguage(string lang)
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
            MultiLanguage.DefaultLanguage = lang;

            //Properties.Settings.Default.DefaultLanguage = lang;
            //Properties.Settings.Default.Save();
        }

        public static string GetDefaultLanguage(string zhCN, string enUS)
        {
            string sReturn = "";

            switch (DefaultLanguage)
            {
                case "zh-CN":
                    sReturn = zhCN;
                    break;

                case "en-US":
                    sReturn = enUS;
                    break;

                default:
                    sReturn = DefaultLanguage;
                    break;
            }

            return sReturn;
        }        
    }
}

