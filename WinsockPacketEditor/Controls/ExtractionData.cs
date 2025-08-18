using AntdUI;
using System;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;

namespace WinsockPacketEditor
{
    public partial class ExtractionData : UserControl
    {
        private Form form;

        #region//窗体事件

        public ExtractionData(Form _form)
        {
            InitializeComponent();
            this.form = _form;
        }

        private void ExtractionData_Load(object sender, EventArgs e)
        {
            this.InitExtraction();
        }

        private void InitExtraction()
        {
            this.ddlExtraction.Items.Clear();
            this.ddlExtraction.Items.AddRange(new AntdUI.SelectItem[]
            {
                    new AntdUI.SelectItem("[ Charles XML 会话文件（.chlsx）] 提取 [ 十六进制数据 ]")
                    {
                        LocalizationText = "",
                    },
                    new AntdUI.SelectItem("[ FILT过滤器文件（.filt）] 提取 [ WPE64 滤镜文件（.sp）]")
                    {
                        LocalizationText = "",
                    },
            });

            this.ddlExtraction.SelectedIndex = 0;
            this.Extraction_Changed();
            this.udExtraction.UseAdmin();
        }

        #endregion

        #region//数据提取

        private void ddlExtraction_SelectedIndexChanged(object sender, IntEventArgs e)
        {
            this.Extraction_Changed();
        }

        private void Extraction_Changed()
        {
            if (this.ddlExtraction.SelectedIndex == 0)
            {
                this.udExtraction.Filter = AntdUI.Localization.Get("System.Charles", "Charles 会话文件") + "（*.chlsx）|*.chlsx";
            }
            else if (this.ddlExtraction.SelectedIndex == 1)
            {
                this.udExtraction.Filter = AntdUI.Localization.Get("System.FILT", "FILT 过滤器文件") + "（*.filt）|*.filt";
            }
        }

        private void udExtraction_DragChanged(object sender, StringsEventArgs e)
        {
            try
            {
                string FilePath = e.Value[0];

                if (!string.IsNullOrEmpty(FilePath))
                {
                    if (File.Exists(FilePath))
                    {
                        switch (this.ddlExtraction.SelectedIndex)
                        {
                            case 0:

                                #region//Charles XML 会话文件

                                try
                                {
                                    XDocument xdoc_Charles = new XDocument();
                                    xdoc_Charles = XDocument.Load(FilePath);

                                    XElement xeRoot_Charles = xdoc_Charles.Descendants("response").FirstOrDefault();

                                    if (xeRoot_Charles != null)
                                    {
                                        if (xeRoot_Charles.Element("body") != null)
                                        {
                                            string sBody = xeRoot_Charles.Element("body").Value;

                                            byte[] bBody = Convert.FromBase64String(sBody);
                                            this.txtExtraction.Text = BitConverter.ToString(bBody).Replace("-", " ");
                                        }
                                    }
                                }
                                catch
                                {
                                    //                            
                                }

                                #endregion

                                break;

                            case 1:

                                #region//FILT 过滤器文件

                                string[] lines = File.ReadAllLines(FilePath, Encoding.Default);

                                XDocument xdoc_Filt = new XDocument
                                {
                                    Declaration = new XDeclaration("1.0", "utf-8", "yes")
                                };

                                XElement xeRoot_Filt = new XElement("FilterList");
                                xdoc_Filt.Add(xeRoot_Filt);

                                foreach (string line in lines)
                                {
                                    if (line.IndexOf("￥") >= 0)
                                    {
                                        string[] slFilter = line.Split('￥');

                                        if (slFilter.Length == 35)
                                        {
                                            string s0 = slFilter[0].ToString();//是否指定长度 bool （真，假）
                                            string s1 = slFilter[1].ToString();//指定长度 int
                                            string s2 = slFilter[2].ToString();//是否指定套接字 bool （真，假）
                                            string s3 = slFilter[3].ToString();//套接字 int
                                            string s4 = slFilter[4].ToString();//是否指定包头 bool （真，假）
                                            string s5 = slFilter[5].ToString();//包头 string (十六进制不带空格)
                                            string s6 = slFilter[6].ToString();//未知 bool （真，假）
                                            string s7 = slFilter[7].ToString();//未知 int 0
                                            string s8 = slFilter[8].ToString();//未知 int 0
                                            string s9 = slFilter[9].ToString();//是否替换 bool （真，假）
                                            string s10 = slFilter[10].ToString();//是否拦截 bool （真，假）
                                            string s11 = slFilter[11].ToString();//是否不可视 bool （真，假）
                                            string s12 = slFilter[12].ToString();//步长 int
                                            string s13 = slFilter[13].ToString();//过滤器名称 string
                                            string s14 = slFilter[14].ToString();//发送 bool （1，0）
                                            string s15 = slFilter[15].ToString();//接收 bool （1，0）
                                            string s16 = slFilter[16].ToString();//发送到 bool （1，0）
                                            string s17 = slFilter[17].ToString();//接收自 bool （1，0）
                                            string s18 = slFilter[18].ToString();//WSA发送 bool （1，0）
                                            string s19 = slFilter[19].ToString();//WSA接收 bool （1，0）
                                            string s20 = slFilter[20].ToString();//WSA发送到 bool （1，0）
                                            string s21 = slFilter[21].ToString();//未知 -1
                                            string s22 = slFilter[22].ToString();//普通模式 bool （真，假）
                                            string s23 = slFilter[23].ToString();//高级模式 bool （真，假）
                                            string s24 = slFilter[24].ToString();//数据包开头 bool （真，假）
                                            string s25 = slFilter[25].ToString();//自发式连锁位 bool （真，假）
                                            string s26 = slFilter[26].ToString();//普通-搜索 string （列Index（支持负数）$十六进制数值不带空格$数据个数$）
                                            string s27 = slFilter[27].ToString();//普通-修改 string（列Index（支持负数）$十六进制数值不带空格$数据个数$）
                                            string s28 = slFilter[28].ToString();//高级-搜索 string（列Index（支持负数）$十六进制数值不带空格$数据个数$）
                                            string s29 = slFilter[29].ToString();//高级-修改 string（列Index（支持负数）$十六进制数值不带空格$数据个数$）
                                            string s30 = slFilter[30].ToString();//递进 bool （真，假）
                                            string s31 = slFilter[31].ToString();//普通-修改-递进 string（列Index（支持负数）$十六进制数值不带空格$数据个数$）
                                            string s32 = slFilter[32].ToString();//高级-修改-递进 string（列Index（支持负数）$十六进制数值不带空格$数据个数$）
                                            string s33 = slFilter[33].ToString();//未知 1

                                            string sIsEnable = bool.FalseString;
                                            string sFID = Guid.NewGuid().ToString();
                                            string sFName = s13;
                                            string sIsExecute = bool.FalseString;
                                            string sRID = Guid.Empty.ToString();
                                            string sFAppointHeader = Operate.SystemConfig.GetBoolFromChineseString(s4).ToString();
                                            string sFHeaderContent = s5;
                                            string sFAppointSocket = Operate.SystemConfig.GetBoolFromChineseString(s2).ToString();
                                            string sFSocketContent = s3;
                                            string sFAppointLength = Operate.SystemConfig.GetBoolFromChineseString(s0).ToString();
                                            string sFLengthContent = s1;

                                            Operate.FilterConfig.Filter.FilterMode FMode = new Operate.FilterConfig.Filter.FilterMode();
                                            if (Operate.SystemConfig.GetBoolFromChineseString(s22) == true)
                                            {
                                                FMode = Operate.FilterConfig.Filter.FilterMode.Normal;
                                            }
                                            else if (Operate.SystemConfig.GetBoolFromChineseString(s23) == true)
                                            {
                                                FMode = Operate.FilterConfig.Filter.FilterMode.Advanced;
                                            }
                                            string sFMode = ((int)FMode).ToString();

                                            Operate.FilterConfig.Filter.FilterAction FAction = new Operate.FilterConfig.Filter.FilterAction();
                                            if (Operate.SystemConfig.GetBoolFromChineseString(s9) == true)
                                            {
                                                FAction = Operate.FilterConfig.Filter.FilterAction.Replace;
                                            }
                                            else if (Operate.SystemConfig.GetBoolFromChineseString(s10) == true)
                                            {
                                                FAction = Operate.FilterConfig.Filter.FilterAction.Intercept;
                                            }
                                            else if (Operate.SystemConfig.GetBoolFromChineseString(s11) == true)
                                            {
                                                FAction = Operate.FilterConfig.Filter.FilterAction.NoModify_NoDisplay;
                                            }
                                            else
                                            {
                                                FAction = Operate.FilterConfig.Filter.FilterAction.NoModify_Display;
                                            }
                                            string sFAction = ((int)FAction).ToString();

                                            bool bSend = Convert.ToBoolean(int.Parse(s14));
                                            bool bRecv = Convert.ToBoolean(int.Parse(s15));
                                            bool bSendTo = Convert.ToBoolean(int.Parse(s16));
                                            bool bRecvFrom = Convert.ToBoolean(int.Parse(s17));
                                            bool bWSASend = Convert.ToBoolean(int.Parse(s18));
                                            bool bWSARecv = Convert.ToBoolean(int.Parse(s19));
                                            bool bWSASendTo = Convert.ToBoolean(int.Parse(s20));
                                            bool bWSARecvFrom = false;

                                            Operate.FilterConfig.Filter.FilterFunction filterFunction =
                                                new Operate.FilterConfig.Filter.FilterFunction(
                                                    bSend,
                                                    bSendTo,
                                                    bRecv,
                                                    bRecvFrom,
                                                    bWSASend,
                                                    bWSASendTo,
                                                    bWSARecv,
                                                    bWSARecvFrom,
                                                    false,
                                                    false,
                                                    false,
                                                    false);
                                            string sFFunction = Operate.FilterConfig.Filter.GetFilterFunctionString(filterFunction);

                                            Operate.FilterConfig.Filter.FilterStartFrom FStartFrom = new Operate.FilterConfig.Filter.FilterStartFrom();
                                            if (Operate.SystemConfig.GetBoolFromChineseString(s24) == true)
                                            {
                                                FStartFrom = Operate.FilterConfig.Filter.FilterStartFrom.Head;
                                            }
                                            else if (Operate.SystemConfig.GetBoolFromChineseString(s25) == true)
                                            {
                                                FStartFrom = Operate.FilterConfig.Filter.FilterStartFrom.Position;
                                            }
                                            string sFStartFrom = ((int)FStartFrom).ToString();

                                            string sFProgressionStep = s12;
                                            string sFProgressionPosition = string.Empty;

                                            string sFSearch = string.Empty;
                                            string sFModify = string.Empty;
                                            if (FMode == Operate.FilterConfig.Filter.FilterMode.Normal)
                                            {
                                                sFProgressionPosition = Operate.SystemConfig.ConvertFILTString(s31, false);
                                                sFSearch = Operate.SystemConfig.ConvertFILTString(s26, false);
                                                sFModify = Operate.SystemConfig.ConvertFILTString(s27, false);
                                            }
                                            else if (FMode == Operate.FilterConfig.Filter.FilterMode.Advanced)
                                            {
                                                sFProgressionPosition = Operate.SystemConfig.ConvertFILTString(s32, false);
                                                sFSearch = Operate.SystemConfig.ConvertFILTString(s28, false);

                                                if (FStartFrom == Operate.FilterConfig.Filter.FilterStartFrom.Position)
                                                {
                                                    sFModify = Operate.SystemConfig.ConvertFILTString(s29, true);
                                                }
                                                else
                                                {
                                                    sFModify = Operate.SystemConfig.ConvertFILTString(s29, false);
                                                }
                                            }

                                            XElement xeFilter =
                                                new XElement("Filter",
                                                new XElement("IsEnable", sIsEnable),
                                                new XElement("ID", sFID),
                                                new XElement("Name", sFName),
                                                new XElement("AppointHeader", sFAppointHeader),
                                                new XElement("HeaderContent", sFHeaderContent),
                                                new XElement("AppointSocket", sFAppointSocket),
                                                new XElement("SocketContent", sFSocketContent),
                                                new XElement("AppointLength", sFAppointLength),
                                                new XElement("LengthContent", sFLengthContent),
                                                new XElement("Mode", sFMode),
                                                new XElement("Action", sFAction),
                                                new XElement("IsExecute", sIsExecute),
                                                new XElement("RobotID", sRID),
                                                new XElement("Function", sFFunction),
                                                new XElement("StartFrom", sFStartFrom),
                                                new XElement("ProgressionStep", sFProgressionStep),
                                                new XElement("ProgressionPosition", sFProgressionPosition),
                                                new XElement("Search", sFSearch),
                                                new XElement("Modify", sFModify)
                                                );

                                            xeRoot_Filt.Add(xeFilter);
                                        }

                                    }
                                }

                                this.txtExtraction.Text = xdoc_Filt.Declaration.ToString() + "\r\n" + xdoc_Filt.ToString();

                                #endregion

                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bExtraction_Click(object sender, EventArgs e)
        {
            try
            {
                string sFileContent = this.txtExtraction.Text.Trim();
                if (string.IsNullOrEmpty(sFileContent))
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "提取数据为空", TType.Error)
                    {
                        LocalizationText = "System.Extraction.Empty"
                    });

                    return;
                }

                SaveFileDialog sfdExtraction = new SaveFileDialog();

                switch (this.ddlExtraction.SelectedIndex)
                {
                    case 0:

                        sfdExtraction.Filter = "TXT（*.txt）|*.txt";

                        break;

                    case 1:

                        sfdExtraction.Filter = AntdUI.Localization.Get("FilterListFile", "滤镜列表文件") + "（*.fp）|*.fp";

                        break;
                }

                if (sfdExtraction.ShowDialog() == DialogResult.OK)
                {
                    string FilePath = sfdExtraction.FileName;
                    if (!string.IsNullOrEmpty(FilePath))
                    {
                        File.WriteAllText(FilePath, sFileContent);

                        string Title = AntdUI.Localization.Get("System.Extraction.Success", "数据提取成功");
                        AntdUI.Notification.success(this.form, Title, FilePath, AntdUI.TAlignFrom.TR);
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, Title + ": " + FilePath);
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion        
    }
}
