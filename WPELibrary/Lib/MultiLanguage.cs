using System;
using System.Threading;
using System.Windows.Forms;

namespace WPELibrary.Lib
{    
    public static class MultiLanguage
    {        
        public static string DefaultLanguage = "zh-CN";

        #region//加载语言（窗体）
        
        public static void LoadLanguage(Form form, Type formType)
        {
            try
            {
                if (form != null)
                {
                    System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(formType);
                    resources.ApplyResources(form, "$this");
                    Loading(form, resources);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }            
        }
        #endregion

        #region//加载语言（控件）
       
        private static void Loading(Control control, System.ComponentModel.ComponentResourceManager resources)
        {
            try
            {
                if (control is MenuStrip)
                {                    
                    resources.ApplyResources(control, control.Name);
                    MenuStrip ms = (MenuStrip)control;
                    if (ms.Items.Count > 0)
                    {
                        foreach (ToolStripMenuItem c in ms.Items)
                        {                            
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
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }
        }
        #endregion

        #region//遍历菜单
      
        private static void Loading(ToolStripMenuItem item, System.ComponentModel.ComponentResourceManager resources)
        {
            try
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
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }
        }
        #endregion

        #region//设置默认语言参数
        public static void SetDefaultLanguage(string lang)
        {
            try
            {
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
                MultiLanguage.DefaultLanguage = lang;

                Properties.Settings.Default.DefaultLanguage = lang;
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }
        }
        #endregion

        #region//获取默认语言参数
        public static string GetDefaultLanguage(string zhCN, string enUS)
        {
            string sReturn = "";

            try
            {
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
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }

            return sReturn;
        }
        #endregion
    }
}

