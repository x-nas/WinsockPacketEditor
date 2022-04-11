using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace WPELibrary.Lib
{
    public static class Socket_Operation
    {        
        public static int CheckCNT = 0;

        public static bool Check_Size = false;
        public static bool Check_Socket = false;
        public static bool Check_IP = false;
        public static bool Check_Packet = false;
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

            foreach (byte n in buffer)
            {
                sReturn += n.ToString("X2") + " ";
            }

            sReturn = sReturn.Trim();

            return sReturn;
        }

        public static byte[] Hex_To_Byte(string hexString)
        {
            hexString = hexString.Replace(" ", "");

            if ((hexString.Length % 2) != 0)
            {
                hexString += " ";
            }

            byte[] returnBytes = new byte[hexString.Length / 2];

            for (int i = 0; i < returnBytes.Length; i++)
            {
                returnBytes[i] = System.Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return returnBytes;
        }

        public static string Byte_To_Bin(byte[] buffer)
        {
            string sReturn = "";

            foreach (byte n in buffer)
            {
                string strTemp = System.Convert.ToString(n, 2);
                strTemp = strTemp.Insert(0, new string('0', 8 - strTemp.Length));

                sReturn += strTemp + " ";
            }

            sReturn = sReturn.Trim();

            return sReturn;
        }

        public static string Byte_To_UTF8(byte[] buffer)
        {
            string sReturn = "";

            Encoding chs = Encoding.GetEncoding("utf-8");
            sReturn = chs.GetString(buffer);

            return sReturn;
        }

        public static string Byte_To_Unicode(byte[] buffer)
        {
            string sReturn = "";

            Encoding chs = Encoding.GetEncoding("utf-16");
            sReturn = chs.GetString(buffer);

            return sReturn;
        }

        public static string Byte_To_GB2312(byte[] buffer)
        {
            string sReturn = "";

            Encoding chs = Encoding.GetEncoding("gb2312");
            sReturn = chs.GetString(buffer);

            return sReturn;
        }

        public static string Byte_To_Default(byte[] buffer)
        {
            string sReturn = "";

            Encoding chs = Encoding.Default;
            sReturn = chs.GetString(buffer);

            return sReturn;
        }

        public static string Byte_To_Dec(byte[] buffer)
        {
            string sReturn = "";

            foreach (byte n in buffer)
            {
                sReturn += n.ToString("D") + " ";
            }

            return sReturn;
        }

        #endregion

        #region//获取指针地址指定长度的字节数据
        public static byte[] GetByteFromIntPtr(IntPtr ipBuff, int iLen)
        {
            byte[] bBuffer = new byte[iLen];
            Marshal.Copy(ipBuff, bBuffer, 0, iLen);

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
            string sReturn = "", sIP = "", sPort = "";

            sIP = GetAddr_IP(in_Addr);
            sPort = GetAddr_Port(NetPort);

            sReturn = sIP + ":" + sPort;

            return sReturn;
        }

        public static string GetSocketIP(int iSocket, Socket_Packet.IPType itIPType)
        {
            string sReturn = "", sIP = "", sPort = "";

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

            sIP = GetAddr_IP(saSocket.sin_addr);
            sPort = GetAddr_Port(saSocket.sin_port);

            sReturn = sIP + ":" + sPort;

            return sReturn;
        }

        //转换IP地址
        private static string GetAddr_IP(Socket_Packet.in_addr in_Addr)
        {
            string sReturn = "";

            sReturn = Marshal.PtrToStringAnsi(inet_ntoa(in_Addr));

            return sReturn;
        }

        //转换端口号
        private static string GetAddr_Port(ushort NetPort)
        {
            string sReturn = "";

            sReturn = ntohs(NetPort).ToString();

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
            catch
            {
                //
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
            catch
            {
                sReturn = "";
            }

            return sReturn;
        }
        #endregion

        #region//获取枚举类型的中文名
        public static string GetSocketType_CN(Socket_Packet.SocketType stType)
        {
            string sReturn = "";

            switch (stType)
            {
                case Socket_Packet.SocketType.Send:
                    sReturn = "发送";
                    break;
                case Socket_Packet.SocketType.WSASend:
                    sReturn = "WSA发送";
                    break;
                case Socket_Packet.SocketType.SendTo:
                    sReturn = "发送到";
                    break;
                case Socket_Packet.SocketType.Recv:
                    sReturn = "接收";
                    break;
                case Socket_Packet.SocketType.WSARecv:
                    sReturn = "WSA接收";
                    break;
                case Socket_Packet.SocketType.RecvFrom:
                    sReturn = "接收自";
                    break;
                case Socket_Packet.SocketType.Send_Interecept:
                    sReturn = "拦截-发送";
                    break;
                case Socket_Packet.SocketType.WSASend_Interecept:
                    sReturn = "拦截-WSA发送";
                    break;
                case Socket_Packet.SocketType.SendTo_Interecept:
                    sReturn = "拦截-发送到";
                    break;
                case Socket_Packet.SocketType.Recv_Interecept:
                    sReturn = "拦截-接收";
                    break;
                case Socket_Packet.SocketType.WSARecv_Interecept:
                    sReturn = "拦截-WSA接收";
                    break;
                case Socket_Packet.SocketType.RecvFrom_Interecept:
                    sReturn = "拦截-接收自";
                    break;
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
            catch
            {
                //
            }

            return sReturn;
        }
        #endregion

        #region//是否显示封包（过滤条件）        

        public static bool ISShow_SocketInfo(Socket_Packet s)
        {
            bool bReturn = true;

            int iSocket = s.Socket;
            byte[] bBuffer = s.Buffer;
            int iResLen = s.ResLen;

            string sIP_From = GetSocketIP(iSocket, Socket_Packet.IPType.From);
            string sIP_To = GetSocketIP(iSocket, Socket_Packet.IPType.To);

            //封包大小
            bool bISShow_BySize = ISShow_BySize(iResLen);
            if (!bISShow_BySize)
            {
                DoLog("[过滤封包大小] " + iResLen.ToString());
                return false;
            }

            //套接字
            bool bISShow_BySocket = ISShow_BySocket(iSocket);
            if (!bISShow_BySocket)
            {
                DoLog("[过滤套接字] " + iSocket.ToString());
                return false;
            }

            //IP地址
            bool bISShow_ByIP = ISShow_ByIP(sIP_From, sIP_To);
            if (!bISShow_ByIP)
            {
                DoLog("[过滤IP地址] " + sIP_From + " / " + sIP_To);
                return false;
            }

            //封包内容
            string sPacket = Byte_To_Hex(bBuffer);
            bool bISShow_ByPacket = ISShow_ByPacket(sPacket);
            if (!bISShow_ByPacket)
            {
                DoLog("[过滤封包内容] " + sPacket);
                return false;
            }

            return bReturn;
        }

        #region//检测封包大小
        private static bool ISShow_BySize(int iLength)
        {
            bool bReturn = true;

            if (Check_Size)
            {
                try
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
                catch
                {
                    //
                }
            }

            return bReturn;
        }
        #endregion

        #region//检测套接字
        private static bool ISShow_BySocket(int iSocket)
        {
            bool bReturn = true;

            if (Check_Socket)
            {
                try
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
                catch
                {
                    //
                }
            }

            return bReturn;
        }
        #endregion

        #region//检测IP地址
        private static bool ISShow_ByIP(string sIP_From, string sIP_To)
        {
            bool bReturn = true;

            if (Check_IP)
            {
                try
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
                catch
                {
                    //
                }
            }

            return bReturn;
        }
        #endregion

        #region//检测封包内容
        private static bool ISShow_ByPacket(string sPacket)
        {
            bool bReturn = true;

            if (Check_Packet)
            {
                try
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
                catch
                {
                    //
                }
            }

            return bReturn;
        }
        #endregion

        #endregion

        #region//搜索封包数据（十六进制）
        public static int SearchSocketListByHex(int iFrom, string sHex)
        {
            int iResult = -1;

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

            return iResult;
        }
        #endregion

        #region//检查套接字是否正确
        public static int CheckSocket(string sSocket)
        {
            int iResult = 0;

            if (!string.IsNullOrEmpty(sSocket))
            {
                try
                {
                    iResult = int.Parse(sSocket);

                    if (iResult < 0)
                    {
                        iResult = 0;
                    }
                }
                catch
                {
                    iResult = 0;
                }
            }            

            return iResult;
        }
        #endregion

        #region//保存发送列表数据
        public static void SaveListToFile()
        {            
            int iSuccess = 0, iFail = 0;
            SaveFileDialog sfdSocketInfo = new SaveFileDialog();
            sfdSocketInfo.Filter = "封包数据文件（*.sp）|*.sp";
            sfdSocketInfo.RestoreDirectory = true;

            if (sfdSocketInfo.ShowDialog() == DialogResult.OK)
            {
                try
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
                                string sNote = Socket_Cache.SocketSendList.dtSocketSendList.Rows[i]["备注"].ToString().Trim();
                                string sSocket = Socket_Cache.SocketSendList.dtSocketSendList.Rows[i]["套接字"].ToString().Trim();
                                string sIPTo = Socket_Cache.SocketSendList.dtSocketSendList.Rows[i]["目的地址"].ToString().Trim();
                                string sLen = Socket_Cache.SocketSendList.dtSocketSendList.Rows[i]["长度"].ToString().Trim();
                                byte[] bBuffer = (byte[])Socket_Cache.SocketSendList.dtSocketSendList.Rows[i]["字节"];
                                string sData = Byte_To_Hex(bBuffer);

                                string sSave = sIndex + "|" + sNote + "|" + sSocket + "|" + sIPTo + "|" + sLen + "|" + sData;

                                sw.WriteLine(sSave);

                                iSuccess ++;
                            }
                            catch
                            {
                                iFail ++;
                            }
                        }
                    }                    

                    sw.Flush();
                    sw.Close();
                    fs.Close();

                    ShowMessageBox("保存完毕，成功【" + iSuccess + "】失败【" + iFail + "】！");                    
                }
                catch (Exception ex)
                {
                    ShowMessageBox("保存失败！错误：" + ex.Message);                    
                }
            }                
        }
        #endregion

        #region//保存封包列表为Excel        
        public static void SaveSocketListToExcel()
        {
            try
            {
                SaveFileDialog sfdSaveToExcel = new SaveFileDialog();
                sfdSaveToExcel.Filter = "Execl files (*.xls)|*.xls";
                sfdSaveToExcel.FilterIndex = 0;
                sfdSaveToExcel.RestoreDirectory = true;
                sfdSaveToExcel.CreatePrompt = true;
                sfdSaveToExcel.Title = "保存为Excel文件";

                if (sfdSaveToExcel.ShowDialog() == DialogResult.OK)
                {
                    Stream myStream = sfdSaveToExcel.OpenFile();
                    StreamWriter sw = new StreamWriter(myStream, Encoding.GetEncoding(-0));

                    string sColTitle = "序号\t类别\t套接字\t源地址\t目的地址\t长度\t数据\t";
                    sw.WriteLine(sColTitle);

                    foreach (Socket_Packet_Info spi in Socket_Cache.SocketList.lstRecPacket)
                    {
                        string sColValue = "";

                        string sIndex = spi.Index.ToString();
                        string sType = GetSocketType_CN(spi.Type);;
                        string sSocket = spi.Socket.ToString();
                        string sFrom = spi.From;
                        string sTo = spi.To;
                        string sLen = spi.ResLen.ToString();
                        byte[] bBuff = spi.Buffer;
                        string sData = Byte_To_Hex(bBuff);

                        sColValue += sIndex + "\t" + sType + "\t" + sSocket + "\t" + sFrom + "\t" + sTo + "\t" + sLen + "\t" + sData + "\t";
                        sw.WriteLine(sColValue);
                    }

                    sw.Close();
                    myStream.Close();
                }
            }
            catch(Exception ex)
            {
                DoLog(ex.Message);
            }            
        }
        #endregion

        #region//保存日志列表为Excel        
        public static void SaveLogListToExcel()
        {
            try
            {
                SaveFileDialog sfdSaveToExcel = new SaveFileDialog();
                sfdSaveToExcel.Filter = "Execl files (*.xls)|*.xls";
                sfdSaveToExcel.FilterIndex = 0;
                sfdSaveToExcel.RestoreDirectory = true;
                sfdSaveToExcel.CreatePrompt = true;
                sfdSaveToExcel.Title = "保存为Excel文件";

                if (sfdSaveToExcel.ShowDialog() == DialogResult.OK)
                {
                    Stream myStream = sfdSaveToExcel.OpenFile();
                    StreamWriter sw = new StreamWriter(myStream, Encoding.GetEncoding(-0));

                    string sColTitle = "记录时间\t日志内容\t";
                    sw.WriteLine(sColTitle);

                    foreach (Socket_Log sl in Socket_Cache.LogList.lstRecLog)
                    {
                        string sColValue = "";

                        string sTime = sl.Time;
                        string sContent = sl.Content;

                        sColValue += sTime + "\t" + sContent + "\t";
                        sw.WriteLine(sColValue);
                    }

                    sw.Close();
                    myStream.Close();
                }
            }
            catch (Exception ex)
            {
                DoLog(ex.Message);
            }
        }
        #endregion

        #region//加载列表数据
        public static void LoadFileToList()
        {
            int iSuccess = 0, iFail = 0;

            OpenFileDialog ofdLoadSocket = new OpenFileDialog();
            ofdLoadSocket.Filter = "封包数据文件（*.sp）|*.sp";
            ofdLoadSocket.RestoreDirectory = true;

            try
            {
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

                            Socket_Cache.SocketSendList.SendList_Add(iIndex, sNote, iSocket, sIPTo, iResLen, sData, bBuffer);                            

                            iSuccess ++;
                        }
                        catch
                        {
                            iFail ++;
                        }
                    }
                }

                ShowMessageBox("加载完毕，成功【" + iSuccess + "】失败【" + iFail + "】！");                
            }
            catch (Exception ex)
            {
                ShowMessageBox("加载失败！错误：" + ex.Message);                
            }
        }

        #endregion

        #region//弹出对话框
        public static void ShowMessageBox(string sMessage)
        {            
            MessageBox.Show(sMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        #region//日志
        public static void DoLog_HookInfo(Socket_Packet.SocketType sType, int iSocket, int iLen, int iRes)
        {
            try
            {
                if (bDoLog_Hook)
                {
                    string sTypeCN = GetSocketType_CN(sType);

                    string sLog = "[" + sTypeCN + "]" + " - " + iSocket.ToString() + "，" + iRes.ToString() + " / " + iLen.ToString();
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
