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
        public static string[] MutiLan_5 = { "当前内核版本: {0}", "Current kernel version: {0}" };
        public static string[] MutiLan_6 = { "请先选择要注入的进程", "Please select a process first" };
        public static string[] MutiLan_7 = { "开始注入目标进程 =>> {0}", "Start injecting target process =>> {0}" };
        public static string[] MutiLan_8 = { "目标进程是{0}位程序，已自动调用{0}位的注入模块", "The target process is a {0} bit program and has automatically called the {0} bit injection module" };
        public static string[] MutiLan_9 = { "已成功注入目标进程 =>> {0}[{1}]", "Successfully injected into the target process =>> {0}[{1}]" };
        public static string[] MutiLan_10 = { "注入完成，可关闭当前注入器或注入其它进程", "Injection completed, the current injector can be closed or inject other processes" };
        public static string[] MutiLan_11 = { "出现错误: {0}", "Error: {0}" };
        public static string[] MutiLan_12 = { "程序即将重新启动", "The program is about to restart" };
        public static string[] MutiLan_13 = { "请选择一个进程", "Please select a process" };
        public static string[] MutiLan_14 = { "请选择要注入的应用程序", "Please select the program to inject" };
        public static string[] MutiLan_15 = { "应用程序|*.exe|所有文件|*.*", "Program|*.exe|All Files|*.*" };
        public static string[] MutiLan_16 = { "滤镜 -【 序号 {0}】- {1} [{2}]", "Filter -【 ID {0}】- {1} [{2}]" };
        public static string[] MutiLan_17 = { "确定重置为上次保存时的数据吗?", "Are you sure to reset to the last saved data?" };
        public static string[] MutiLan_18 = { "确定保存当前数据吗?", "Are you sure to save the current data?" };
        public static string[] MutiLan_19 = { "名称不能为空!", "The name cannot be empty!" };
        public static string[] MutiLan_20 = { "目标进程: ", "Process: " };
        public static string[] MutiLan_21 = { "不是有效的异或值!", "Not a valid XOR value!" };
        public static string[] MutiLan_22 = { "只执行滤镜，不显示封包", "Only execute filters, No display" };
        public static string[] MutiLan_23 = { "没有找到匹配的内容！", "No matching content found!" };
        public static string[] MutiLan_24 = { "[ 行 {0}  列 {1}  位置 {2} ]", "[ Ln {0}  Col {1}  Pstn {2} ]" };
        public static string[] MutiLan_25 = { "查找内容", "Search for content" };
        public static string[] MutiLan_26 = { "查找", "Search" };
        public static string[] MutiLan_27 = { "机器人 {0}", "Robot {0}" };
        public static string[] MutiLan_28 = { "滤镜数据加载出错!", "Filter data loading error!" };
        public static string[] MutiLan_29 = { "两个文本相同", "The two texts are the same" };
        public static string[] MutiLan_30 = { "请正确设置查找内容！", "Please set the Find content correctly!" };
        public static string[] MutiLan_31 = { "已发送: {0}  已接收: {1}", "Sent: {0} Bytes  Received: {1} Bytes" };
        public static string[] MutiLan_32 = { "发送 - [{0}]", "Send - [{0}]" };
        public static string[] MutiLan_33 = { "文本 A  ( 长度 {0} )", "Text A  ( Length {0} )" };
        public static string[] MutiLan_34 = { "文本 B  ( 长度 {0} )", "Text B  ( Length {0} )" };
        public static string[] MutiLan_35 = { "加载系统配置成功！", "Load System Config successful!" };
        public static string[] MutiLan_36 = { "导出列表文件", "Export List File" };
        public static string[] MutiLan_37 = { "确定删除选中的数据吗", "Are you sure to delete the selected data" };
        public static string[] MutiLan_38 = { "确定删除所有数据吗", "Are you sure to delete all data" };
        public static string[] MutiLan_39 = { "开始拦截!", "Start Intercepting!" };
        public static string[] MutiLan_40 = { "结束拦截!", "Stop Intercepting!" };
        public static string[] MutiLan_41 = { "已开启极速模式!", "Speed mode has been activated!" };
        public static string[] MutiLan_42 = { "粘贴的不是有效的十六进制数据!", "The pasted data is not valid hex data!" };
        public static string[] MutiLan_43 = { "WPE64 - Winsock Packet Editor x64 - {0}", "WPE64 - Winsock Packet Editor x64 - {0}" };
        public static string[] MutiLan_44 = { "请正确设置递进位置", "Please set the progressive position correctly" };
        public static string[] MutiLan_45 = { "套接字设置错误", "Socket setting error" };
        public static string[] MutiLan_46 = { "封包数据错误", "Packet data error" };
        public static string[] MutiLan_47 = { "递进设置错误", "Progressive setting error" };
        public static string[] MutiLan_48 = { "发送列表 - ", "Send List - " };
        public static string[] MutiLan_49 = { "请正确设置套接字", "Please set the socket correctly" };
        public static string[] MutiLan_50 = { "滤镜 {0}", "Filter {0}" };
        public static string[] MutiLan_51 = { "[{0}] {1} | [{2}] 封包长度: {3}", "[{0}] {1} | [{2}] Packet Length: {3}" };
        public static string[] MutiLan_52 = { "{0} | 封包长度: {1} | {2}", "{0} | Packet Length: {1} | {2}" };
        public static string[] MutiLan_53 = { "匹配滤镜出错: ", "Matching filter error: " };
        public static string[] MutiLan_54 = { "发送", "Send" };
        public static string[] MutiLan_55 = { "接收", "Recv" };
        public static string[] MutiLan_56 = { "发送到", "SendTo" };
        public static string[] MutiLan_57 = { "接收自", "RecvFrom" };
        public static string[] MutiLan_58 = { "WSA发送", "WSASend" };
        public static string[] MutiLan_59 = { "WSA接收", "WSARecv" };
        public static string[] MutiLan_60 = { "WSA发送到", "WSASendTo" };
        public static string[] MutiLan_61 = { "WSA接收自", "WSARecvFromd" };
        public static string[] MutiLan_62 = { "{0} - 副本", "{0} - Copy" };
        public static string[] MutiLan_63 = { "优先执行满足条件的，不执行其它的", "Prioritize executing those that meet the conditions and do not execute others" };
        public static string[] MutiLan_64 = { "顺序执行所有满足条件的", "Execute all items that meet the conditions in sequence" };
        public static string[] MutiLan_65 = { "替换", "Replace" };
        public static string[] MutiLan_66 = { "拦截", "Intercept" };
        public static string[] MutiLan_67 = { "不修改-只显示", "No Modify-Display" };
        public static string[] MutiLan_68 = { "不修改-不显示", "No Modify-No Display" };
        public static string[] MutiLan_69 = { "[{0}] {1} | [{2}] 封包长度: {3} | 匹配数: {4}", "[{0}] {1} | [{2}] Packet Length: {3} | Match: {4}" };
        public static string[] MutiLan_70 = { "发送列表文件", "Send List File" };
        public static string[] MutiLan_71 = { "加载机器人列表成功! [未加密]", "Load robot list successful! [Unencrypted]" };
        public static string[] MutiLan_72 = { "加载机器人列表成功! [已加密]", "Load robot list successful! [Encrypted]" };
        public static string[] MutiLan_73 = { "{0} 句柄: {1}", "{0} Handle: {1}" };
        public static string[] MutiLan_74 = { "机器人数据文件", "Robot data file" };
        public static string[] MutiLan_75 = { "滤镜数据文件", "Filter data file" };
        public static string[] MutiLan_76 = { "保存为Excel文件", "Save as Excel file" };
        public static string[] MutiLan_77 = { "序号\t类别\t套接字\t源地址\t目的地址\t长度\t数据\t", "ID\tCategory\tSocket\tFrom Address\tTo Address\tLength\tData\t" };
        public static string[] MutiLan_78 = { "记录时间\t模块\t日志内容\t", "Log Time\tModule Name\tLog content\t" };
        public static string[] MutiLan_79 = { "提示", "Prompt" };
        public static string[] MutiLan_80 = { "加载滤镜列表成功! [未加密]", "Load filter list successful! [Unencrypted]" };
        public static string[] MutiLan_81 = { "加载滤镜列表成功! [已加密]", "Load filter list successful! [Encrypted]" };
        public static string[] MutiLan_82 = { "递进位置超出封包长度", "Progressive position exceeds packet length" };
        public static string[] MutiLan_83 = { "不是合法的十六进制数据", "Not legal hexadecimal data" };
        public static string[] MutiLan_84 = { "请选择需要发送的封包", "Please select the packets to sent" };
        public static string[] MutiLan_85 = { "十六进制", "Hexadecimal" };
        public static string[] MutiLan_86 = { "二进制", "Binary" };
        public static string[] MutiLan_87 = { "十进制", "Decimal" };
        public static string[] MutiLan_88 = { " 请输入密码, 此密码在导入列表文件时会要求输入验证!\r\n 如无需设置密码，请点击 [ 取消 ] 按钮!", " Please enter the password. This password will require verification when importing the list file.\r\n If you do not need to set a password, please click the [ Cancel ] button!" };
        public static string[] MutiLan_89 = { "密码不能为空!", "Password cannot be empty!" };
        public static string[] MutiLan_90 = { "导入列表文件", "Import List File" };
        public static string[] MutiLan_91 = { " 请输入密码", " Please enter the password" };
        public static string[] MutiLan_92 = { "加载失败: 密码错误!", "Failed to load: Password incorrect!" };
        public static string[] MutiLan_93 = { "机器人 -【 序号 {0}】- {1} [{2}]", "Robot -【 ID {0}】- {1} [{2}]" };
        public static string[] MutiLan_94 = { "发送", "Send" };
        public static string[] MutiLan_95 = { "延迟", "Delay" };
        public static string[] MutiLan_96 = { "循环开始", "Loop Start" };
        public static string[] MutiLan_97 = { "循环结束", "Loop End" };
        public static string[] MutiLan_98 = { "指令 {0}", "Inst {0}" };
        public static string[] MutiLan_99 = { "循环指令设置错误！", "Loop instruction setting error!" };
        public static string[] MutiLan_100 = { "加载发送列表成功! [未加密]", "Load send list successful! [Unencrypted]" };
        public static string[] MutiLan_101 = { "加载发送列表成功! [已加密]", "Load send list successful! [Encrypted]" };
        public static string[] MutiLan_102 = { "请参阅: {0}/tutorials.html 问题与解答!", "Please refer to the {0}/tutorials_enUS.html Q&A!" };

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

