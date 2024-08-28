using System;
using System.Reflection;
using System.Threading;

namespace WPELibrary.Lib
{    
    public static class MultiLanguage
    {        
        public static string DefaultLanguage = "zh-CN";

        #region//多语言

        public static string[] MutiLan_1 = { "选择进程", "Select Process" };
        public static string[] MutiLan_2 = { "注入进程", "Inject Process" };
        public static string[] MutiLan_3 = { "关于", "about" };
        public static string[] MutiLan_4 = { "选择语言", "Language" };
        public static string[] MutiLan_5 = { "当前内核版本", "Current kernel version" };
        public static string[] MutiLan_6 = { "请先选择要注入的进程", "Please select a process first" };
        public static string[] MutiLan_7 = { "开始注入目标进程", "Start injecting target process" };
        public static string[] MutiLan_8 = { "目标进程是{0}位程序，已自动调用{0}位的注入模块", "The target process is a {0} bit program and has automatically called the {0} bit injection module" };
        public static string[] MutiLan_9 = { "已成功注入目标进程", "Successfully injected into the target process" };
        public static string[] MutiLan_10 = { "注入完成，可关闭当前注入器或注入其它进程", "Injection completed, the current injector can be closed or inject other processes" };
        public static string[] MutiLan_11 = { "出现错误", "Error" };
        public static string[] MutiLan_12 = { "程序即将重新启动", "The program is about to restart" };
        public static string[] MutiLan_13 = { "请选择一个进程", "Please select a process" };
        public static string[] MutiLan_14 = { "请选择要注入的应用程序", "Please select the program to inject" };
        public static string[] MutiLan_15 = { "应用程序|*.exe|所有文件|*.*", "Program|*.exe|All Files|*.*" };
        public static string[] MutiLan_16 = { "滤镜 -【 序号", "Filter -【 ID" };
        public static string[] MutiLan_17 = { "确定重置为上次保存时的数据吗?", "Are you sure to reset to the last saved data?" };
        public static string[] MutiLan_18 = { "确定保存当前数据吗?", "Are you sure to save the current data?" };
        public static string[] MutiLan_19 = { "滤镜名称不能为空", "Filter name cannot be empty" };
        public static string[] MutiLan_20 = { "目标进程：", "Process: " };
        public static string[] MutiLan_21 = { "初始化数据表完成", "Initializing data table completed" };
        public static string[] MutiLan_22 = { "设置拦截参数完成", "Setting interception parameters completed" };
        public static string[] MutiLan_23 = { "网络封包拦截器", "Winsock Packet Editor" };
        public static string[] MutiLan_24 = { "下载安装：", "DownLoad Setup: " };
        public static string[] MutiLan_25 = { "使用说明：", "Instructions: " };
        public static string[] MutiLan_26 = { "联系作者：", "Contact author: " };
        public static string[] MutiLan_27 = { "滤镜 {0} 添加完成", "Filter {0} added completed" };
        public static string[] MutiLan_28 = { "滤镜数据加载出错!", "Filter data loading error!" };
        //public static string[] MutiLan_29 = { "显示-接收, ", "Show-Recv, " };
        //public static string[] MutiLan_30 = { "显示-接收自, ", "Show-RecvFrom, " };
        //public static string[] MutiLan_31 = { "过滤-封包长度, ", "Filter-SocketSize, " };
        //public static string[] MutiLan_32 = { "过滤-套接字, ", "Filter-Socket, " };
        //public static string[] MutiLan_33 = { "过滤-IP地址, ", "Filter-IP Address, " };
        //public static string[] MutiLan_34 = { "过滤-封包内容, ", "Filter-Socket Packet, " };
        public static string[] MutiLan_35 = { "开始拦截！", "Start Intercepting!" };
        public static string[] MutiLan_36 = { "结束拦截！", "Stop Interception!" };
        public static string[] MutiLan_37 = { "确定删除选中的滤镜吗", "Are you sure to delete the selected filter" };
        public static string[] MutiLan_38 = { "确定删除所有数据吗", "Are you sure to delete all data" };
        public static string[] MutiLan_39 = { "没有搜索到数据", "No data found in search" };
        public static string[] MutiLan_40 = { "请输入搜索内容", "Please enter search content" };
        //public static string[] MutiLan_41 = { "搜索完成!", "Search completed!" };
        public static string[] MutiLan_42 = { "发送封包 -【 序号", "Send -【 ID" };
        //public static string[] MutiLan_43 = { "", "" };
        public static string[] MutiLan_44 = { "请正确设置递进位置", "Please set the progressive position correctly" };
        public static string[] MutiLan_45 = { "套接字设置错误", "Socket setting error" };
        public static string[] MutiLan_46 = { "封包数据错误", "Packet data error" };
        public static string[] MutiLan_47 = { "递进设置错误", "Progressive setting error" };
        public static string[] MutiLan_48 = { "发送列表 - ", "Send List - " };
        public static string[] MutiLan_49 = { "请正确设置套接字", "Please set the socket correctly" };
        public static string[] MutiLan_50 = { "滤镜", "Filter" };
        public static string[] MutiLan_51 = { "执行滤镜成功: ", "Filter execution successful: " };
        public static string[] MutiLan_52 = { "执行滤镜出错: ", "Error executing filter: " };
        public static string[] MutiLan_53 = { "匹配滤镜出错: ", "Matching filter error: " };
        //public static string[] MutiLan_54 = { "发送", "Send" };
        //public static string[] MutiLan_55 = { "WSA发送", "WSend" };
        //public static string[] MutiLan_56 = { "发送到", "SendTo" };
        //public static string[] MutiLan_57 = { "接收", "Recv" };
        //public static string[] MutiLan_58 = { "WSA接收", "WRecv" };
        //public static string[] MutiLan_59 = { "接收自", "RecvF" };
        //public static string[] MutiLan_60 = { "拦截-发送", "B-Send" };
        //public static string[] MutiLan_61 = { "拦截-WSA发送", "B-WSend" };
        //public static string[] MutiLan_62 = { "拦截-发送到", "B-SendTo" };
        //public static string[] MutiLan_63 = { "拦截-接收", "B-Recv" };
        //public static string[] MutiLan_64 = { "拦截-WSA接收", "B-WRecv" };
        //public static string[] MutiLan_65 = { "拦截-接收自", "B-RecvF" };
        //public static string[] MutiLan_66 = { "[过滤封包大小] ", "[Filter packet size] " };
        //public static string[] MutiLan_67 = { "[过滤套接字] ", "[Filter sockets] " };
        //public static string[] MutiLan_68 = { "[过滤IP地址] ", "[Filter IP addresses] " };
        //public static string[] MutiLan_69 = { "[过滤封包内容] ", "[Filter packet content] " };
        public static string[] MutiLan_70 = { "封包数据文件", "Package data file" };
        public static string[] MutiLan_71 = { "保存完毕，成功【{0}】失败【{1}】", "Save completed, successful【{0}】failed【{1}】" };
        public static string[] MutiLan_72 = { "保存失败！错误：", "Save failed! Error: " };
        public static string[] MutiLan_73 = { "加载完毕，成功【{0}】失败【{1}】", "Load completed, successful【{0}】failed【{1}】" };
        public static string[] MutiLan_74 = { "加载失败！错误：", "Load failed! Error: " };
        public static string[] MutiLan_75 = { "滤镜数据文件", "Filter data file" };
        public static string[] MutiLan_76 = { "保存为Excel文件", "Save as Excel file" };
        public static string[] MutiLan_77 = { "序号\t类别\t套接字\t源地址\t目的地址\t长度\t数据\t", "ID\tCategory\tSocket\tFrom Address\tTo Address\tLength\tData\t" };
        public static string[] MutiLan_78 = { "记录时间\t模块\t日志内容\t", "Log Time\tModule Name\tLog content\t" };
        public static string[] MutiLan_79 = { "提示", "Prompt" };
        public static string[] MutiLan_80 = { "发送封包成功", "Successfully sent packet" };
        public static string[] MutiLan_81 = { "发送封包失败", "Sending packet failed" };
        public static string[] MutiLan_82 = { "递进位置超出封包长度", "Progressive position exceeds packet length" };
        public static string[] MutiLan_83 = { "不是合法的十六进制数据", "Not legal hexadecimal data" };
        public static string[] MutiLan_84 = { "请选择需要发送的封包", "Please select the packets to sent" };
        public static string[] MutiLan_85 = { "十六进制", "Hexadecimal" };
        public static string[] MutiLan_86 = { "二进制", "Binary" };
        public static string[] MutiLan_87 = { "十进制", "Decimal" };

        #endregion

        #region//设置默认语言参数
        public static void SetDefaultLanguage(string lang)
        {
            try
            {
                Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(lang);
                MultiLanguage.DefaultLanguage = lang;                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);                
            }
        }
        #endregion

        #region//获取默认语言参数
        public static string GetDefaultLanguage(string[] MutiLanID)
        {
            string sReturn = string.Empty;

            try
            {
                switch (DefaultLanguage)
                {
                    case "zh-CN":
                        sReturn = MutiLanID[0].ToString();
                        break;

                    case "en-US":
                        sReturn = MutiLanID[1].ToString();
                        break;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }
        #endregion
    }
}

