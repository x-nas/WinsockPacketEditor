using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WPELibrary.Lib
{
    public static class Socket_Operation
    {
        private static string sLanguage_UI = "";

        public static int CheckCNT = 0;

        public static bool IsCheck_Size = false;
        public static bool IsCheck_Socket = false;
        public static bool IsCheck_IP = false;
        public static bool IsCheck_Packet = false;
        public static bool bDoLog = true;
        public static bool bDoLog_Hook = false;

        public static string Check_Size_From = "";
        public static string Check_Size_To = "";
        public static string Check_Socket_txt = "";
        public static string Check_IP_txt = "";
        public static string Check_Packet_txt = "";

        #region//ws2_32.dll API        

        [DllImport("ws2_32.dll")]
        private static extern int getsockname(int s, ref Socket_Packet.sockaddr Address, ref int namelen);

        [DllImport("ws2_32.dll")]
        private static extern int getpeername(int s, ref Socket_Packet.sockaddr Address, ref int namelen);

        [DllImport("ws2_32.dll")]
        private static extern IntPtr inet_ntoa(Socket_Packet.in_addr a);

        [DllImport("ws2_32.dll")]
        private static extern ushort ntohs(ushort netshort);

        #endregion

        #region//进制转换        

        public static string Byte_To_Hex(byte[] buffer)
        {
            string sReturn = "";

            try
            {
                foreach (byte n in buffer)
                {
                    sReturn += n.ToString("X2") + " ";
                }

                sReturn = sReturn.Trim();
            }
            catch (Exception ex)
            {
                DoLog(ex.Message);
            }

            return sReturn;
        }

        public static byte[] Hex_To_Byte(string hexString)
        {
            try
            {
                hexString = hexString.Replace(" ", "");

                if ((hexString.Length % 2) != 0)
                {
                    hexString += " ";
                }

                byte[] returnBytes = new byte[hexString.Length / 2];

                for (int i = 0; i < returnBytes.Length; i++)
                {
                    returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
                }

                return returnBytes;
            }
            catch (Exception ex)
            {
                DoLog(ex.Message);

                return null;
            }            
        }

        public static string Byte_To_Bin(byte[] buffer)
        {
            string sReturn = "";

            try
            {
                foreach (byte n in buffer)
                {
                    string strTemp = System.Convert.ToString(n, 2);
                    strTemp = strTemp.Insert(0, new string('0', 8 - strTemp.Length));

                    sReturn += strTemp + " ";
                }

                sReturn = sReturn.Trim();
            }
            catch (Exception ex)
            {
                DoLog(ex.Message);
            }

            return sReturn;
        }

        public static string Byte_To_UTF8(byte[] buffer)
        {
            string sReturn = "";

            try
            {
                Encoding chs = Encoding.GetEncoding("utf-8");
                sReturn = chs.GetString(buffer);
            }
            catch (Exception ex)
            {
                DoLog(ex.Message);
            }

            return sReturn;
        }

        public static string Byte_To_Unicode(byte[] buffer)
        {
            string sReturn = "";

            try
            {
                Encoding chs = Encoding.GetEncoding("utf-16");
                sReturn = chs.GetString(buffer);
            }
            catch (Exception ex)
            {
                DoLog(ex.Message);
            }

            return sReturn;
        }

        public static string Byte_To_Ascii(byte[] buffer)
        {
            string sReturn = "";

            try
            {
                Encoding chs = Encoding.GetEncoding("Ascii");
                sReturn = chs.GetString(buffer);
            }
            catch (Exception ex)
            {
                DoLog(ex.Message);
            }

            return sReturn;
        }

        public static string Byte_To_GB2312(byte[] buffer)
        {
            string sReturn = "";

            try
            {
                Encoding chs = Encoding.GetEncoding("gb2312");
                sReturn = chs.GetString(buffer);
            }
            catch (Exception ex)
            {
                DoLog(ex.Message);
            }

            return sReturn;
        }

        public static string Byte_To_Default(byte[] buffer)
        {
            string sReturn = "";

            try
            {
                Encoding chs = Encoding.Default;
                sReturn = chs.GetString(buffer);
            }
            catch (Exception ex)
            {
                DoLog(ex.Message);
            }

            return sReturn;
        }

        public static string Byte_To_Dec(byte[] buffer)
        {
            string sReturn = "";

            try
            {
                foreach (byte n in buffer)
                {
                    sReturn += n.ToString("D") + " ";
                }
            }
            catch (Exception ex)
            {
                DoLog(ex.Message);
            }

            return sReturn;
        }

        #endregion

        #region//获取指针地址指定长度的字节数据
        public static byte[] GetByteFromIntPtr(IntPtr ipBuff, int iLen)
        {
            byte[] bBuffer = new byte[iLen];

            try
            {
                Marshal.Copy(ipBuff, bBuffer, 0, iLen);
            }
            catch (Exception ex)
            {
                DoLog(ex.Message);
            }

            return bBuffer;
        }
        #endregion

        #region//设置指针地址位置的字节数据
        public static bool SetByteToIntPtr(byte[] bBuffer, IntPtr ipBuff, int iLen)
        {
            bool bResult = false;

            try
            {
                Marshal.Copy(bBuffer, 0, ipBuff, iLen);
                bResult = true;
            }
            catch
            {
                bResult = false;
            }

            return bResult;
        }
        #endregion

        #region//获取套接字对应的IP地址和端口
        public static string GetSocketIP(Socket_Packet.in_addr in_Addr, ushort NetPort)
        {
            string sReturn = "";

            try
            {
                string sIP = GetAddr_IP(in_Addr);
                string sPort = GetAddr_Port(NetPort);

                sReturn = sIP + ":" + sPort;
            }
            catch (Exception ex)
            {
                DoLog(ex.Message);
            }

            return sReturn;
        }

        public static string GetSocketIP(int iSocket, Socket_Packet.IPType itIPType)
        {
            string sReturn = "";

            try
            {
                Socket_Packet.sockaddr saSocket = new Socket_Packet.sockaddr();
                int iLen = Marshal.SizeOf(saSocket);

                if (itIPType.Equals(Socket_Packet.IPType.From))
                {
                    getsockname(iSocket, ref saSocket, ref iLen);
                }
                else if (itIPType.Equals(Socket_Packet.IPType.To))
                {
                    getpeername(iSocket, ref saSocket, ref iLen);
                }
                else
                {
                    //
                }

                string sIP = GetAddr_IP(saSocket.sin_addr);
                string sPort = GetAddr_Port(saSocket.sin_port);

                sReturn = sIP + ":" + sPort;
            }
            catch (Exception ex)
            {
                DoLog(ex.Message);
            }

            return sReturn;
        }

        //转换IP地址
        private static string GetAddr_IP(Socket_Packet.in_addr in_Addr)
        {
            string sReturn = "";

            try
            {
                sReturn = Marshal.PtrToStringAnsi(inet_ntoa(in_Addr));
            }
            catch (Exception ex)
            {
                DoLog(ex.Message);
            }

            return sReturn;
        }

        //转换端口号
        private static string GetAddr_Port(ushort NetPort)
        {
            string sReturn = "";

            try
            {
                sReturn = ntohs(NetPort).ToString();
            }
            catch (Exception ex)
            {
                DoLog(ex.Message);
            }

            return sReturn;
        }
        #endregion

        #region//获取指定位置的16进制数据值
        public static string GetValueByIndex_HEX(string sHex, int iIndex)
        {
            string sReturn = "";

            try
            {
                string[] slHex = sHex.Split(' ');

                if (slHex.Length > iIndex)
                {
                    sReturn = slHex[iIndex];
                }
            }
            catch (Exception ex)
            {
                DoLog(ex.Message);
            }

            return sReturn;
        }
        #endregion

        #region//获取指定步长的16进制数据值
        public static string GetValueByLen_HEX(string sHex, int iLen)
        {
            string sReturn = "";

            try
            {
                int iValue_Dec = Convert.ToInt32(sHex, 16);
                iValue_Dec += iLen;

                sReturn = iValue_Dec.ToString("X2");
            }
            catch (Exception ex)
            {
                DoLog(ex.Message);
            }

            return sReturn;
        }
        #endregion

        #region//获取枚举类型的名称
        public static string GetSocketType_Name(Socket_Packet.SocketType stType)
        {
            string sReturn = "";

            try
            {
                switch (stType)
                {
                    case Socket_Packet.SocketType.Send:
                        sLanguage_UI = MultiLanguage.GetDefaultLanguage("发送", "Send");
                        sReturn = sLanguage_UI;
                        break;
                    case Socket_Packet.SocketType.WSASend:
                        sLanguage_UI = MultiLanguage.GetDefaultLanguage("WSA发送", "WSend");
                        sReturn = sLanguage_UI;
                        break;
                    case Socket_Packet.SocketType.SendTo:
                        sLanguage_UI = MultiLanguage.GetDefaultLanguage("发送到", "SendTo");
                        sReturn = sLanguage_UI;
                        break;
                    case Socket_Packet.SocketType.Recv:
                        sLanguage_UI = MultiLanguage.GetDefaultLanguage("接收", "Recv");
                        sReturn = sLanguage_UI;
                        break;
                    case Socket_Packet.SocketType.WSARecv:
                        sLanguage_UI = MultiLanguage.GetDefaultLanguage("WSA接收", "WRecv");
                        sReturn = sLanguage_UI;
                        break;
                    case Socket_Packet.SocketType.RecvFrom:
                        sLanguage_UI = MultiLanguage.GetDefaultLanguage("接收自", "RecvF");
                        sReturn = sLanguage_UI;
                        break;
                    case Socket_Packet.SocketType.Send_Interecept:
                        sLanguage_UI = MultiLanguage.GetDefaultLanguage("拦截-发送", "B-Send");
                        sReturn = sLanguage_UI;
                        break;
                    case Socket_Packet.SocketType.WSASend_Interecept:
                        sLanguage_UI = MultiLanguage.GetDefaultLanguage("拦截-WSA发送", "B-WSend");
                        sReturn = sLanguage_UI;
                        break;
                    case Socket_Packet.SocketType.SendTo_Interecept:
                        sLanguage_UI = MultiLanguage.GetDefaultLanguage("拦截-发送到", "B-SendTo");
                        sReturn = sLanguage_UI;
                        break;
                    case Socket_Packet.SocketType.Recv_Interecept:
                        sLanguage_UI = MultiLanguage.GetDefaultLanguage("拦截-接收", "B-Recv");
                        sReturn = sLanguage_UI;
                        break;
                    case Socket_Packet.SocketType.WSARecv_Interecept:
                        sLanguage_UI = MultiLanguage.GetDefaultLanguage("拦截-WSA接收", "B-WRecv");
                        sReturn = sLanguage_UI;
                        break;
                    case Socket_Packet.SocketType.RecvFrom_Interecept:
                        sLanguage_UI = MultiLanguage.GetDefaultLanguage("拦截-接收自", "B-RecvF");
                        sReturn = sLanguage_UI;
                        break;
                }
            }
            catch (Exception ex)
            {
                DoLog(ex.Message);
            }

            return sReturn;
        }
        #endregion

        #region//替换封包指定位置的16进制数据值
        public static string ReplaceValueByIndexAndLen_HEX(string sHex, int iIndex, int iLen)
        {
            string sReturn = "";

            try
            {
                string sOldValue = GetValueByIndex_HEX(sHex, iIndex);
                string sNewValue = GetValueByLen_HEX(sOldValue, iLen);

                int iStartIndex = iIndex * 3;

                sHex = sHex.Remove(iStartIndex, 2);
                sHex = sHex.Insert(iStartIndex, sNewValue);

                sReturn = sHex;
            }
            catch (Exception ex)
            {
                DoLog(ex.Message);
            }

            return sReturn;
        }
        #endregion

        #region//是否显示封包（过滤条件）        

        public static bool ISShow_SocketInfo(Socket_Packet s)
        {
            bool bReturn = true;

            try
            {
                int iSocket = s.Socket;
                byte[] bBuffer = s.Buffer;
                int iResLen = s.ResLen;

                string sIP_From = GetSocketIP(iSocket, Socket_Packet.IPType.From);
                string sIP_To = GetSocketIP(iSocket, Socket_Packet.IPType.To);

                //封包大小
                bool bISShow_BySize = ISShow_BySize(iResLen);
                if (!bISShow_BySize)
                {
                    sLanguage_UI = MultiLanguage.GetDefaultLanguage("[过滤封包大小] ", "[Filter packet size] ");
                    DoLog(sLanguage_UI + iResLen.ToString());
                    return false;
                }

                //套接字
                bool bISShow_BySocket = ISShow_BySocket(iSocket);
                if (!bISShow_BySocket)
                {
                    sLanguage_UI = MultiLanguage.GetDefaultLanguage("[过滤套接字] ", "[Filter sockets] ");
                    DoLog(sLanguage_UI + iSocket.ToString());
                    return false;
                }

                //IP地址
                bool bISShow_ByIP = ISShow_ByIP(sIP_From, sIP_To);
                if (!bISShow_ByIP)
                {
                    sLanguage_UI = MultiLanguage.GetDefaultLanguage("[过滤IP地址] ", "[Filter IP addresses] ");
                    DoLog(sLanguage_UI + sIP_From + " / " + sIP_To);
                    return false;
                }

                //封包内容
                string sPacket = Byte_To_Hex(bBuffer);
                bool bISShow_ByPacket = ISShow_ByPacket(sPacket);
                if (!bISShow_ByPacket)
                {
                    sLanguage_UI = MultiLanguage.GetDefaultLanguage("[过滤封包内容] ", "[Filter packet content] ");
                    DoLog(sLanguage_UI + sPacket);
                    return false;
                }
            }
            catch (Exception ex)
            {
                DoLog(ex.Message);
            }

            return bReturn;
        }

        #region//检测封包大小
        private static bool ISShow_BySize(int iLength)
        {
            bool bReturn = true;

            try
            {
                if (IsCheck_Size)
                {
                    int iFrom = int.Parse(Check_Size_From);
                    int iTo = int.Parse(Check_Size_To);

                    if (iLength >= iFrom && iLength <= iTo)
                    {
                        bReturn = true;
                    }
                    else
                    {
                        bReturn = false;
                    }
                }
            }
            catch (Exception ex)
            {
                DoLog(ex.Message);
            }

            return bReturn;
        }
        #endregion

        #region//检测套接字
        private static bool ISShow_BySocket(int iSocket)
        {
            bool bReturn = true;

            try
            {
                if (IsCheck_Socket)
                {
                    string[] sSocketArr = Check_Socket_txt.Split(';');

                    foreach (string sSocket in sSocketArr)
                    {
                        int iSocketCheck = int.Parse(sSocket);

                        if (iSocket == iSocketCheck)
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DoLog(ex.Message);
            }

            return bReturn;
        }
        #endregion

        #region//检测IP地址
        private static bool ISShow_ByIP(string sIP_From, string sIP_To)
        {
            bool bReturn = true;

            try
            {
                if (IsCheck_IP)
                {
                    string[] sIPArr = Check_IP_txt.Split(';');

                    foreach (string sIP in sIPArr)
                    {
                        if (sIP_From.IndexOf(sIP) >= 0 || sIP_To.IndexOf(sIP) >= 0)
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DoLog(ex.Message);
            }

            return bReturn;
        }
        #endregion

        #region//检测封包内容
        private static bool ISShow_ByPacket(string sPacket)
        {
            bool bReturn = true;

            try
            {
                if (IsCheck_Packet)
                {
                    string[] sPacketArr = Check_Packet_txt.Split(';');

                    foreach (string sPacketCheck in sPacketArr)
                    {
                        if (sPacket.IndexOf(sPacketCheck) >= 0)
                        {
                            return false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DoLog(ex.Message);
            }

            return bReturn;
        }
        #endregion

        #endregion

        #region//搜索封包数据（十六进制）
        public static int SearchSocketListByHex(int iFrom, string sHex)
        {
            int iResult = -1;

            try
            {
                if (!string.IsNullOrEmpty(sHex))
                {
                    int iListCNT = Socket_Cache.SocketList.lstRecPacket.Count;

                    if (iListCNT > 0)
                    {
                        if (iFrom < iListCNT)
                        {
                            for (int i = iFrom; i < iListCNT; i++)
                            {
                                byte[] bSearch = Socket_Cache.SocketList.lstRecPacket[i].Buffer;
                                string sSearch = Byte_To_Hex(bSearch);

                                if (sSearch.IndexOf(sHex) >= 0)
                                {
                                    iResult = i;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DoLog(ex.Message);
            }

            return iResult;
        }
        #endregion

        #region//检查套接字是否正确
        public static int CheckSocket(string sSocket)
        {
            int iResult = 0;

            try
            {
                if (!string.IsNullOrEmpty(sSocket))
                {
                    iResult = int.Parse(sSocket);

                    if (iResult < 0)
                    {
                        iResult = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                DoLog(ex.Message);
            }          

            return iResult;
        }
        #endregion

        #region//保存发送列表数据
        public static void SaveSendListToFile()
        {            
            int iSuccess = 0, iFail = 0;

            try
            {
                SaveFileDialog sfdSocketInfo = new SaveFileDialog();

                sLanguage_UI = MultiLanguage.GetDefaultLanguage("封包数据文件", "Package data file");
                sfdSocketInfo.Filter = sLanguage_UI + "（*.sp）|*.sp";
                sfdSocketInfo.RestoreDirectory = true;

                if (sfdSocketInfo.ShowDialog() == DialogResult.OK)
                {
                    FileStream fs = new FileStream(sfdSocketInfo.FileName, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);

                    if (Socket_Cache.SocketSendList.dtSocketSendList.Rows.Count > 0)
                    {
                        for (int i = 0; i < Socket_Cache.SocketSendList.dtSocketSendList.Rows.Count; i++)
                        {
                            try
                            {
                                string sIndex = (i + 1).ToString();
                                string sNote = Socket_Cache.SocketSendList.dtSocketSendList.Rows[i]["Remark"].ToString().Trim();
                                string sSocket = Socket_Cache.SocketSendList.dtSocketSendList.Rows[i]["Socket"].ToString().Trim();
                                string sIPTo = Socket_Cache.SocketSendList.dtSocketSendList.Rows[i]["ToAddress"].ToString().Trim();
                                string sLen = Socket_Cache.SocketSendList.dtSocketSendList.Rows[i]["Len"].ToString().Trim();
                                byte[] bBuffer = (byte[])Socket_Cache.SocketSendList.dtSocketSendList.Rows[i]["Bytes"];
                                string sData = Byte_To_Hex(bBuffer);

                                string sSave = sIndex + "|" + sNote + "|" + sSocket + "|" + sIPTo + "|" + sLen + "|" + sData;

                                sw.WriteLine(sSave);

                                iSuccess++;
                            }
                            catch(Exception ex)
                            {
                                iFail++;
                                DoLog(ex.Message);
                            }
                        }
                    }

                    sw.Flush();
                    sw.Close();
                    fs.Close();

                    sLanguage_UI = MultiLanguage.GetDefaultLanguage("保存完毕，成功【{0}】失败【{1}】！", "Save completed, successful【{0}】failed【{1}】!");
                    ShowMessageBox(string.Format(sLanguage_UI, iSuccess, iFail));                 
                }
            }
            catch (Exception ex)
            {
                sLanguage_UI = MultiLanguage.GetDefaultLanguage("保存失败！错误：", "Save failed! Error:");
                ShowMessageBox(sLanguage_UI + ex.Message);

                DoLog(ex.Message);
            }                            
        }
        #endregion

        #region//加载发送列表数据
        public static void LoadFileToSendList()
        {
            int iSuccess = 0, iFail = 0;

            try
            {
                OpenFileDialog ofdLoadSocket = new OpenFileDialog();

                sLanguage_UI = MultiLanguage.GetDefaultLanguage("封包数据文件", "Package data file");
                ofdLoadSocket.Filter = sLanguage_UI + "（*.sp）|*.sp";
                ofdLoadSocket.RestoreDirectory = true;

                ofdLoadSocket.ShowDialog();
                string filePath = ofdLoadSocket.FileName;

                if (!string.IsNullOrEmpty(filePath))
                {
                    string[] slSocket = File.ReadAllLines(filePath, Encoding.UTF8);

                    Socket_Cache.SocketSendList.dtSocketSendList.Rows.Clear();

                    foreach (string sSocketTemp in slSocket)
                    {
                        try
                        {
                            string[] ss = sSocketTemp.Split('|');

                            int iIndex = int.Parse(ss[0]);
                            string sNote = ss[1];
                            int iSocket = int.Parse(ss[2]);
                            string sIPTo = ss[3];
                            int iResLen = int.Parse(ss[4]);
                            string sData = ss[5];

                            byte[] bBuffer = Hex_To_Byte(sData);

                            Socket_Cache.SocketSendList.AddSendList_New(iIndex, sNote, iSocket, sIPTo, iResLen, sData, bBuffer);

                            iSuccess++;
                        }
                        catch (Exception ex)
                        {
                            iFail++;

                            DoLog(ex.Message);
                        }                       
                    }

                    sLanguage_UI = MultiLanguage.GetDefaultLanguage("加载完毕，成功【{0}】失败【{1}】！", "Load completed, successful【{0}】failed【{1}】!");
                    ShowMessageBox(string.Format(sLanguage_UI, iSuccess, iFail));
                }            
            }
            catch (Exception ex)
            {
                sLanguage_UI = MultiLanguage.GetDefaultLanguage("加载失败！错误：", "Load failed! Error:");
                ShowMessageBox(sLanguage_UI + ex.Message);

                DoLog(ex.Message);
            }
        }

        #endregion

        #region//保存滤镜列表数据
        public static void SaveFilterListToFile()
        {
            int iSuccess = 0, iFail = 0;

            try
            {
                SaveFileDialog sfdSocketInfo = new SaveFileDialog();

                sLanguage_UI = MultiLanguage.GetDefaultLanguage("滤镜数据文件", "Filter data file");
                sfdSocketInfo.Filter = sLanguage_UI + "（*.fp）|*.fp";
                sfdSocketInfo.RestoreDirectory = true;

                if (sfdSocketInfo.ShowDialog() == DialogResult.OK)
                {
                    FileStream fs = new FileStream(sfdSocketInfo.FileName, FileMode.Create);
                    StreamWriter sw = new StreamWriter(fs);

                    if (Socket_Cache.SocketFilterList.lstFilter.Count > 0)
                    {
                        for (int i = 0; i < Socket_Cache.SocketFilterList.lstFilter.Count; i++)
                        {
                            try
                            {
                                string sFNum = Socket_Cache.SocketFilterList.lstFilter[i].FNum.ToString();
                                string sFName = Socket_Cache.SocketFilterList.lstFilter[i].FName.ToString();
                                string sFSearch = Socket_Cache.SocketFilterList.lstFilter[i].FSearch.ToString();
                                string sModify = Socket_Cache.SocketFilterList.lstFilter[i].FModify.ToString();

                                string sSave = sFNum + "|" + sFName + "|" + sFSearch + "|" + sModify;

                                sw.WriteLine(sSave);

                                iSuccess++;
                            }
                            catch (Exception ex)
                            {
                                iFail++;

                                DoLog(ex.Message);
                            }                         
                        }
                    }

                    sw.Flush();
                    sw.Close();
                    fs.Close();

                    sLanguage_UI = MultiLanguage.GetDefaultLanguage("保存完毕，成功【{0}】失败【{1}】！", "Save completed, successful【{0}】failed【{1}】!");
                    ShowMessageBox(string.Format(sLanguage_UI, iSuccess, iFail));                
                }
            }
            catch (Exception ex)
            {
                sLanguage_UI = MultiLanguage.GetDefaultLanguage("保存失败！错误：", "Save failed! Error:");
                ShowMessageBox(sLanguage_UI + ex.Message);

                DoLog(ex.Message);
            }
        }
        #endregion

        #region//加载滤镜列表数据
        public static void LoadFileToFilterList()
        {
            int iSuccess = 0, iFail = 0;

            try
            {
                OpenFileDialog ofdLoadSocket = new OpenFileDialog();

                sLanguage_UI = MultiLanguage.GetDefaultLanguage("滤镜数据文件", "Filter data file");
                ofdLoadSocket.Filter = sLanguage_UI + "（*.fp）|*.fp";
                ofdLoadSocket.RestoreDirectory = true;

                ofdLoadSocket.ShowDialog();
                string filePath = ofdLoadSocket.FileName;

                if (!string.IsNullOrEmpty(filePath))
                {
                    string[] slFilter = File.ReadAllLines(filePath, Encoding.UTF8);

                    Socket_Cache.SocketFilterList.FilterListClear();

                    foreach (string sFilterTemp in slFilter)
                    {
                        try
                        {
                            string[] ss = sFilterTemp.Split('|');

                            int iFNum = int.Parse(ss[0]);
                            string sFName = ss[1];
                            string sFSearch = ss[2];
                            string sFModify = ss[3];

                            Socket_Cache.SocketFilterList.AddFilter_New(iFNum, false, sFName, sFSearch, sFModify);

                            iSuccess++;
                        }
                        catch (Exception ex)
                        {
                            iFail++;

                            DoLog(ex.Message);
                        }                     
                    }

                    sLanguage_UI = MultiLanguage.GetDefaultLanguage("加载完毕，成功【{0}】失败【{1}】！", "Load completed, successful【{0}】failed【{1}】!");
                    ShowMessageBox(string.Format(sLanguage_UI, iSuccess, iFail));
                }           
            }
            catch (Exception ex)
            {
                sLanguage_UI = MultiLanguage.GetDefaultLanguage("加载失败！错误：", "Load failed! Error:");
                ShowMessageBox(sLanguage_UI + ex.Message);

                DoLog(ex.Message);
            }
        }

        #endregion

        #region//保存封包列表为Excel
        public static void SaveSocketListToExcel()
        {
            int iSuccess = 0, iFail = 0;

            try
            {
                SaveFileDialog sfdSaveToExcel = new SaveFileDialog();
                sfdSaveToExcel.Filter = "Execl files (*.xls)|*.xls";
                sfdSaveToExcel.FilterIndex = 0;
                sfdSaveToExcel.RestoreDirectory = true;
                sfdSaveToExcel.CreatePrompt = true;

                sLanguage_UI = MultiLanguage.GetDefaultLanguage("保存为Excel文件", "Save as Excel file");
                sfdSaveToExcel.Title = sLanguage_UI;

                if (sfdSaveToExcel.ShowDialog() == DialogResult.OK)
                {
                    Stream myStream = sfdSaveToExcel.OpenFile();
                    StreamWriter sw = new StreamWriter(myStream, Encoding.GetEncoding(-0));

                    sLanguage_UI = MultiLanguage.GetDefaultLanguage("序号\t类别\t套接字\t源地址\t目的地址\t长度\t数据\t", "ID\tCategory\tSocket\tFrom Address\tTo Address\tLength\tData\t");
                    string sColTitle = sLanguage_UI;
                    sw.WriteLine(sColTitle);

                    foreach (Socket_Packet_Info spi in Socket_Cache.SocketList.lstRecPacket)
                    {
                        try
                        {
                            string sColValue = "";

                            string sIndex = spi.Index.ToString();
                            string sType = GetSocketType_Name(spi.Type); ;
                            string sSocket = spi.Socket.ToString();
                            string sFrom = spi.From;
                            string sTo = spi.To;
                            string sLen = spi.ResLen.ToString();
                            byte[] bBuff = spi.Buffer;
                            string sData = Byte_To_Hex(bBuff);

                            sColValue += sIndex + "\t" + sType + "\t" + sSocket + "\t" + sFrom + "\t" + sTo + "\t" + sLen + "\t" + sData + "\t";
                            sw.WriteLine(sColValue);

                            iSuccess++;
                        }
                        catch (Exception ex)
                        {
                            iFail++;

                            DoLog(ex.Message);
                        }
                    }

                    sw.Close();
                    myStream.Close();

                    sLanguage_UI = MultiLanguage.GetDefaultLanguage("保存完毕，成功【{0}】失败【{1}】！", "Save completed, successful【{0}】failed【{1}】!");
                    ShowMessageBox(string.Format(sLanguage_UI, iSuccess, iFail));
                }
            }
            catch(Exception ex)
            {
                sLanguage_UI = MultiLanguage.GetDefaultLanguage("保存失败！错误：", "Save failed! Error:");
                ShowMessageBox(sLanguage_UI + ex.Message);

                DoLog(ex.Message);
            }            
        }
        #endregion

        #region//保存日志列表为Excel        
        public static void SaveLogListToExcel()
        {
            int iSuccess = 0, iFail = 0;

            try
            {
                SaveFileDialog sfdSaveToExcel = new SaveFileDialog();
                sfdSaveToExcel.Filter = "Execl files (*.xls)|*.xls";
                sfdSaveToExcel.FilterIndex = 0;
                sfdSaveToExcel.RestoreDirectory = true;
                sfdSaveToExcel.CreatePrompt = true;

                sLanguage_UI = MultiLanguage.GetDefaultLanguage("保存为Excel文件", "Save as Excel file");
                sfdSaveToExcel.Title = sLanguage_UI;

                if (sfdSaveToExcel.ShowDialog() == DialogResult.OK)
                {
                    Stream myStream = sfdSaveToExcel.OpenFile();
                    StreamWriter sw = new StreamWriter(myStream, Encoding.GetEncoding(-0));

                    sLanguage_UI = MultiLanguage.GetDefaultLanguage("记录时间\t日志内容\t", "Log Time\tLog content\t");
                    string sColTitle = sLanguage_UI;
                    sw.WriteLine(sColTitle);

                    foreach (Socket_Log_Info sl in Socket_Cache.LogList.lstRecLog)
                    {
                        try
                        {
                            string sColValue = "";

                            string sTime = sl.Time;
                            string sContent = sl.Content;

                            sColValue += sTime + "\t" + sContent + "\t";
                            sw.WriteLine(sColValue);

                            iSuccess++;
                        }
                        catch (Exception ex)
                        {
                            iFail++;

                            DoLog(ex.Message);
                        }
                    }

                    sw.Close();
                    myStream.Close();

                    sLanguage_UI = MultiLanguage.GetDefaultLanguage("保存完毕，成功【{0}】失败【{1}】！", "Save completed, successful【{0}】failed【{1}】!");
                    ShowMessageBox(string.Format(sLanguage_UI, iSuccess, iFail));
                }
            }
            catch (Exception ex)
            {
                sLanguage_UI = MultiLanguage.GetDefaultLanguage("保存失败！错误：", "Save failed! Error:");
                ShowMessageBox(sLanguage_UI + ex.Message);

                DoLog(ex.Message);
            }
        }
        #endregion        

        #region//弹出对话框
        public static void ShowMessageBox(string sMessage)
        {
            try
            {
                sLanguage_UI = MultiLanguage.GetDefaultLanguage("提示", "Prompt");
                MessageBox.Show(sMessage, sLanguage_UI, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                DoLog(ex.Message);
            }            
        }

        public static DialogResult ShowSelectMessageBox(string sMessage)
        {
            DialogResult dr = new DialogResult();

            try
            {
                sLanguage_UI = MultiLanguage.GetDefaultLanguage("提示", "Prompt");
                dr = MessageBox.Show(sMessage, sLanguage_UI, MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            }
            catch (Exception ex)
            {
                DoLog(ex.Message);
            }

            return dr;
        }
        #endregion

        #region//记录日志
        public static void DoLog_HookInfo(Socket_Packet.SocketType sType, int iSocket, int iLen, int iRes)
        {
            try
            {
                if (bDoLog_Hook)
                {
                    string sTypeName = GetSocketType_Name(sType);
                    string sLog = "[" + sTypeName + "]" + " - Socket:" + iSocket.ToString() + "，Packet:" + iRes.ToString() + "/" + iLen.ToString();
                    DoLog(sLog);
                }
            }
            catch (Exception ex)
            {
                DoLog(ex.Message);
            }           
        }

        public static void DoLog(string sLogContent)
        {
            try
            {
                if (bDoLog)
                {
                    Socket_Cache.LogQueue.LogToQueue(sLogContent);
                }
            }
            catch (Exception ex)
            {
                DoLog(ex.Message);
            }           
        }
        #endregion
    }
}
