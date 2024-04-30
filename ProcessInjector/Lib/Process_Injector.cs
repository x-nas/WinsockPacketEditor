using System;
using System.Data;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Threading;
using WPELibrary.Lib;
using System.Deployment.Application;

namespace ProcessInjector.Lib
{
    internal class Process_Injector
    {
        #region//DLL 引用

        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool IsWow64Process([In] IntPtr process, [Out] out bool wow64Process);

        #endregion

        #region//判断是否为64位的进程
        public static bool IsWin64Process(int ProcessID)
        {
            bool bReturn = false;

            try
            {
                Process pProcess = Process.GetProcessById(ProcessID);

                if (pProcess != null)
                {
                    if ((Environment.OSVersion.Version.Major > 5) || ((Environment.OSVersion.Version.Major == 5) && (Environment.OSVersion.Version.Minor >= 1)))
                    {
                        bool retVal;
                        IsWow64Process(pProcess.Handle, out retVal);
                        bReturn = !retVal;
                    }
                }
            }
            catch
            {
                //
            }            

            return bReturn;
        }
        #endregion

        #region//获取所有进程
        public static DataTable GetProcess()
        {
            DataTable dtProcessList = new DataTable();
            dtProcessList.Columns.Add("ICO", typeof(Image));
            dtProcessList.Columns.Add("PName", typeof(string));
            dtProcessList.Columns.Add("PID", typeof(int));

            try
            {
                Process[] procesArr = Process.GetProcesses();
                int pCNT = procesArr.Length;

                foreach (Process p in procesArr)
                {
                    string sPName = p.ProcessName;
                    int iPID = p.Id;
                    Image iICO = IconFromFile(p);

                    DataRow dr = dtProcessList.NewRow();
                    dr["ICO"] = iICO;
                    dr["PName"] = sPName;
                    dr["PID"] = iPID;
                    dtProcessList.Rows.Add(dr);
                }

                DataView dv = dtProcessList.DefaultView;
                dv.Sort = "PName";
                dtProcessList = dv.ToTable();
            }
            catch
            {
                //
            }

            return dtProcessList;
        }
        #endregion

        #region//获取进程的图标
        private static Image IconFromFile(Process p)
        {
            string filePath = "";
            Image image = null;

            try
            {
                filePath = p.MainModule.FileName.Replace(".ni.dll", ".dll");
            }
            catch
            {
                filePath = "";
            }

            try
            {
                var extractor = new IconExtractor(filePath);
                var icon = extractor.GetIcon(0);

                Icon[] splitIcons = IconUtil.Split(icon);

                Icon selectedIcon = null;

                foreach (var item in splitIcons)
                {
                    if (selectedIcon == null)
                    {
                        selectedIcon = item;
                    }
                    else
                    {
                        if (IconUtil.GetBitCount(item) > IconUtil.GetBitCount(selectedIcon))
                        {
                            selectedIcon = item;
                        }
                        else if (IconUtil.GetBitCount(item) == IconUtil.GetBitCount(selectedIcon) && item.Width > selectedIcon.Width)
                        {
                            selectedIcon = item;
                        }
                    }
                }

                return selectedIcon.ToBitmap();
            }
            catch
            {
                //
            }

            try
            {
                image = Icon.ExtractAssociatedIcon(filePath)?.ToBitmap();
            }
            catch
            {
                image = new Icon(SystemIcons.Application, 256, 256).ToBitmap();
            }

            return image;
        }
        #endregion

        #region//设置默认语言
        public static void SetDefaultLanguage(string lang)
        {
            try
            {
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
                MultiLanguage.DefaultLanguage = lang;
                Properties.Settings.Default.DefaultLanguage = lang;
                Properties.Settings.Default.Save();
            }
            catch
            {
                //
            }          
        }
        #endregion

        #region//获取发布版本号
        public static string GetVersion()
        {
            string sReturn = "";

            try
            {
                if (ApplicationDeployment.IsNetworkDeployed)
                {
                    sReturn = ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
                }
                else
                {
                    sReturn = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                }
            }
            catch
            {
                //
            }

            return sReturn;            
        }
        #endregion
    }
}
