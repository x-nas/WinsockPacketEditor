using System;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Data;

namespace WPELibrary.Lib
{   
    public static class Socket_Operation
    {        
        public static bool bDoLog = true;
        public static bool bDoLog_Hook = false;        

        public static DataTable dtPacketFormat = new DataTable();         

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

        #region//数据格式转换

        #region//初始化封包数据格式
        public static void InitPacketFormat()
        {
            try
            {
                if (dtPacketFormat.Columns.Count == 0)
                {
                    dtPacketFormat.Columns.Add("Key", typeof(string));
                    dtPacketFormat.Columns.Add("Value", typeof(string));
                }

                if (dtPacketFormat.Rows.Count == 0)
                {
                    DataRow drHEX = dtPacketFormat.NewRow();

                    drHEX[0] = "HEX";
                    drHEX[1] = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_85);
                    dtPacketFormat.Rows.Add(drHEX);

                    DataRow drASCII = dtPacketFormat.NewRow();
                    drASCII[0] = "ASCII";
                    drASCII[1] = "ASCII";
                    dtPacketFormat.Rows.Add(drASCII);                  

                    DataRow drUnicode = dtPacketFormat.NewRow();
                    drUnicode[0] = "UNICODE";
                    drUnicode[1] = "UNICODE";
                    dtPacketFormat.Rows.Add(drUnicode);

                    DataRow drUTF7 = dtPacketFormat.NewRow();
                    drUTF7[0] = "UTF-7";
                    drUTF7[1] = "UTF-7";
                    dtPacketFormat.Rows.Add(drUTF7);

                    DataRow drUTF8 = dtPacketFormat.NewRow();
                    drUTF8[0] = "UTF-8";
                    drUTF8[1] = "UTF-8";
                    dtPacketFormat.Rows.Add(drUTF8);

                    DataRow drUTF16LE = dtPacketFormat.NewRow();
                    drUTF16LE[0] = "UTF-16-LE";
                    drUTF16LE[1] = "UTF-16（LE）";
                    dtPacketFormat.Rows.Add(drUTF16LE);

                    DataRow drUTF16BE = dtPacketFormat.NewRow();
                    drUTF16BE[0] = "UTF-16-BE";
                    drUTF16BE[1] = "UTF-16（BE）";
                    dtPacketFormat.Rows.Add(drUTF16BE);

                    DataRow drUTF32LE = dtPacketFormat.NewRow();
                    drUTF32LE[0] = "UTF-32-LE";
                    drUTF32LE[1] = "UTF-32（LE）";
                    dtPacketFormat.Rows.Add(drUTF32LE);

                    DataRow drUTF32BE = dtPacketFormat.NewRow();
                    drUTF32BE[0] = "UTF-32-BE";
                    drUTF32BE[1] = "UTF-32（BE）";
                    dtPacketFormat.Rows.Add(drUTF32BE);

                    DataRow drBIN = dtPacketFormat.NewRow();
                    drBIN[0] = "BIN";
                    drBIN[1] = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_86);
                    dtPacketFormat.Rows.Add(drBIN);

                    DataRow drDEC = dtPacketFormat.NewRow();
                    drDEC[0] = "DEC";
                    drDEC[1] = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_87);
                    dtPacketFormat.Rows.Add(drDEC);
                }
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }          
        }

        #endregion

        #region//字节转字符串
        public static string ByteToString(string EncodingFormat, byte[] buffer)
        {
            string sReturn = "";

            try
            {
                string sFormat = "";

                switch (EncodingFormat)
                {
                    case "HEX":

                        foreach (byte b in buffer)
                        {
                            sReturn += b.ToString("X2") + " ";
                        }
                        sReturn = sReturn.Trim();
                        break;

                    case "ASCII":

                        sFormat = Encoding.ASCII.GetString(buffer);
                        break;

                    case "UNICODE":

                        sFormat = Encoding.Unicode.GetString(buffer);
                        break;

                    case "UTF-7":

                        sFormat = Encoding.UTF7.GetString(buffer);
                        break;

                    case "UTF-8":

                        sFormat = Encoding.UTF8.GetString(buffer);
                        break;

                    case "UTF-16-LE":

                        sFormat = Encoding.Unicode.GetString(buffer);
                        break;

                    case "UTF-16-BE":

                        sFormat = Encoding.BigEndianUnicode.GetString(buffer);
                        break;

                    case "UTF-32-LE":

                        sFormat = Encoding.UTF32.GetString(buffer);
                        break;

                    case "UTF-32-BE":

                        byte[] copiedBuffer = new byte[buffer.Length];
                        Array.Copy(buffer, copiedBuffer, buffer.Length);
                        Array.Reverse(copiedBuffer);
                        sFormat = Encoding.UTF32.GetString(copiedBuffer);
                        break;

                    case "BIN":

                        foreach (byte b in buffer)
                        {
                            string strTemp = Convert.ToString(b, 2);
                            strTemp = strTemp.Insert(0, new string('0', 8 - strTemp.Length));
                            sReturn += strTemp + " ";
                        }
                        sReturn = sReturn.Trim();
                        break;

                    case "DEC":

                        foreach (byte n in buffer)
                        {
                            sReturn += n.ToString("D") + " ";
                        }
                        sReturn = sReturn.Trim();
                        break;

                    default:
                        sFormat = Encoding.Default.GetString(buffer);
                        break;
                }                

                foreach (char c in sFormat)
                {
                    if ((int)c > 31)
                    {
                        sReturn += c.ToString();
                    }
                    else
                    {
                        sReturn += "\u0001";
                    }
                }
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }

        #endregion

        #region//十六进制字符串转字节
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
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);

                return null;
            }
        }
        #endregion    

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
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return sReturn;
        }
        #endregion

        #region//是否显示封包（过滤条件）        

        public static bool ISShowSocketPacket_ByFilter(Socket_Packet sp)
        {
            try
            {
                //套接字
                if (Socket_Cache.Check_Socket)
                {
                    if (ISFilter_BySocket(sp.Socket))
                    {                        
                        return false;
                    }
                }

                //IP地址
                if (Socket_Cache.Check_IP)
                {                    
                    if (ISFilter_ByIP(sp.Socket))
                    {                        
                        return false;
                    }
                }

                //封包内容
                if (Socket_Cache.Check_Packet)
                {  
                    if (ISFilter_ByPacket(sp.Buffer))
                    {                        
                        return false;
                    }
                }

                //封包大小
                if (Socket_Cache.Check_Size)
                {
                    if (ISFilter_BySize(sp.Len))
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return true;
        }

        #region//检测套接字
        private static bool ISFilter_BySocket(int iSocket)
        {
            bool bReturn = false;

            try
            {
                if (!string.IsNullOrEmpty(Socket_Cache.txtCheck_Socket))
                {
                    string[] sSocketArr = Socket_Cache.txtCheck_Socket.Split(';');

                    foreach (string sSocket in sSocketArr)
                    {
                        int iSocketCheck = int.Parse(sSocket);

                        if (iSocket == iSocketCheck)
                        {
                            return true;
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }
        #endregion

        #region//检测IP地址
        private static bool ISFilter_ByIP(int iSocket)
        {
            bool bReturn = false;

            try
            {
                if (!string.IsNullOrEmpty(Socket_Cache.txtCheck_IP))
                {
                    string sIP_From = GetSocketIP(iSocket, Socket_Packet.IPType.From);
                    string sIP_To = GetSocketIP(iSocket, Socket_Packet.IPType.To);

                    string[] sIPArr = Socket_Cache.txtCheck_IP.Split(';');

                    foreach (string sIP in sIPArr)
                    {
                        if (sIP_From.IndexOf(sIP) >= 0 || sIP_To.IndexOf(sIP) >= 0)
                        {
                            return true;
                        }
                    }
                }                    
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }
        #endregion

        #region//检测封包内容
        private static bool ISFilter_ByPacket(byte[] bBuffer)
        {
            bool bReturn = false;

            try
            {
                if (!string.IsNullOrEmpty(Socket_Cache.txtCheck_Packet))
                {
                    string sPacket = ByteToString("HEX", bBuffer);

                    string[] sPacketArr = Socket_Cache.txtCheck_Packet.Split(';');

                    foreach (string sPacketCheck in sPacketArr)
                    {
                        if (sPacket.IndexOf(sPacketCheck) >= 0)
                        {
                            return true;
                        }
                    }
                }                    
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }
        #endregion

        #region//检测封包大小
        private static bool ISFilter_BySize(int iLength)
        {
            bool bReturn = false;

            try
            {
                if (iLength >= Socket_Cache.txtCheck_Size_From && iLength <= Socket_Cache.txtCheck_Size_To)
                {
                    bReturn = true;
                }
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }
        #endregion

        #endregion

        #region//搜索封包数据
        public static int SearchSocketList(string sSearchType, int iFromIndex, string sSearchData)
        {
            int iResult = -1;

            try
            {
                if (!string.IsNullOrEmpty(sSearchData))
                {
                    int iListCNT = Socket_Cache.SocketList.lstRecPacket.Count;

                    if (iListCNT > 0 && iFromIndex < iListCNT)
                    {
                        string sSearch = "";

                        for (int i = iFromIndex; i < iListCNT; i++)
                        {
                            byte[] bSearch = Socket_Cache.SocketList.lstRecPacket[i].Buffer;

                            sSearch = ByteToString(sSearchType, bSearch);

                            if (sSearch.IndexOf(sSearchData) >= 0)
                            {
                                iResult = i;
                                break;
                            }
                        }
                    }
                }                
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }          

            return iResult;
        }
        #endregion

        #region//检查是否合法的十六进制
        public static bool CheckHEX(object oHex)
        {
            try
            {
                if (oHex == null)
                { 
                    return false;
                }
                
                if (string.IsNullOrEmpty(oHex.ToString()))
                {
                    return false;
                }

                string sHex = oHex.ToString();

                if (sHex.Length != 2)
                {
                    return false;
                }

                string pattern = "^[A-Fa-f0-9]+$";
                return Regex.IsMatch(sHex, pattern);
            }
            catch (Exception ex) 
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);

                return false;
            }
        }
        #endregion

        #region//保存发送列表数据
        public static void SaveSendListToFile()
        {            
            int iSuccess = 0, iFail = 0;

            try
            {
                SaveFileDialog sfdSocketInfo = new SaveFileDialog();
                                
                sfdSocketInfo.Filter = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_70) + "（*.sp）|*.sp";
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
                                string sData = ByteToString("HEX", bBuffer);

                                string sSave = sIndex + "|" + sNote + "|" + sSocket + "|" + sIPTo + "|" + sLen + "|" + sData;

                                sw.WriteLine(sSave);

                                iSuccess++;
                            }
                            catch(Exception ex)
                            {
                                iFail++;
                                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                            }
                        }
                    }

                    sw.Flush();
                    sw.Close();
                    fs.Close();

                    ShowMessageBox(string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_71), iSuccess, iFail));                 
                }
            }
            catch (Exception ex)
            {                
                ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_72) + ex.Message);

                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

                ofdLoadSocket.Filter = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_70) + "（*.sp）|*.sp";
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

                            DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                        }                       
                    }

                    ShowMessageBox(string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_73), iSuccess, iFail));
                }            
            }
            catch (Exception ex)
            {
                ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_74) + ex.Message);

                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

                sfdSocketInfo.Filter = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_75) + "（*.fp）|*.fp";
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

                                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                            }                         
                        }
                    }

                    sw.Flush();
                    sw.Close();
                    fs.Close();
                                        
                    ShowMessageBox(string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_71), iSuccess, iFail));                
                }
            }
            catch (Exception ex)
            {                
                ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_72) + ex.Message);

                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                
                ofdLoadSocket.Filter = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_75) + "（*.fp）|*.fp";
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

                            DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                        }                     
                    }
                                        
                    ShowMessageBox(string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_73), iSuccess, iFail));
                }           
            }
            catch (Exception ex)
            {                
                ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_74) + ex.Message);

                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                
                sfdSaveToExcel.Title = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_76);

                if (sfdSaveToExcel.ShowDialog() == DialogResult.OK)
                {
                    Stream myStream = sfdSaveToExcel.OpenFile();
                    StreamWriter sw = new StreamWriter(myStream, Encoding.GetEncoding(-0));
                                        
                    string sColTitle = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_77);
                    sw.WriteLine(sColTitle);

                    foreach (Socket_Packet_Info spi in Socket_Cache.SocketList.lstRecPacket)
                    {
                        try
                        {
                            string sColValue = "";

                            string sIndex = spi.Index.ToString();
                            string sType = spi.Type.ToString();
                            string sSocket = spi.Socket.ToString();
                            string sFrom = spi.From;
                            string sTo = spi.To;
                            string sLen = spi.ResLen.ToString();
                            byte[] bBuff = spi.Buffer;
                            string sData = ByteToString("HEX", bBuff);

                            sColValue += sIndex + "\t" + sType + "\t" + sSocket + "\t" + sFrom + "\t" + sTo + "\t" + sLen + "\t" + sData + "\t";
                            sw.WriteLine(sColValue);

                            iSuccess++;
                        }
                        catch (Exception ex)
                        {
                            iFail++;

                            DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                        }
                    }

                    sw.Close();
                    myStream.Close();
                                        
                    ShowMessageBox(string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_71), iSuccess, iFail));
                }
            }
            catch(Exception ex)
            {                
                ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_72) + ex.Message);

                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                                
                sfdSaveToExcel.Title = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_76);

                if (sfdSaveToExcel.ShowDialog() == DialogResult.OK)
                {
                    Stream myStream = sfdSaveToExcel.OpenFile();
                    StreamWriter sw = new StreamWriter(myStream, Encoding.GetEncoding(-0));
                                        
                    string sColTitle = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_78);
                    sw.WriteLine(sColTitle);

                    foreach (Socket_Log_Info sl in Socket_Cache.LogList.lstRecLog)
                    {
                        try
                        {
                            string sColValue = "";

                            string sTime = sl.Time;
                            string sFuncName = sl.FuncName;
                            string sContent = sl.Content;

                            sColValue += sTime + "\t" + sFuncName + "\t" + sContent + "\t";
                            sw.WriteLine(sColValue);

                            iSuccess++;
                        }
                        catch (Exception ex)
                        {
                            iFail++;

                            DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                        }
                    }

                    sw.Close();
                    myStream.Close();
                                        
                    ShowMessageBox(string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_71), iSuccess, iFail));
                }
            }
            catch (Exception ex)
            {                
                ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_72) + ex.Message);
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        #endregion        

        #region//弹出对话框
        public static void ShowMessageBox(string sMessage)
        {
            try
            {                
                MessageBox.Show(sMessage, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_79), MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        public static DialogResult ShowSelectMessageBox(string sMessage)
        {
            DialogResult dr = new DialogResult();

            try
            {                
                dr = MessageBox.Show(sMessage, MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_79), MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return dr;
        }
        #endregion

        #region//记录日志
        public static void DoLog_HookInfo(string sFuncName, Socket_Packet.SocketType sType, int iSocket, int iLen, int iRes)
        {
            try
            {
                if (bDoLog_Hook)
                {
                    string sTypeName = sType.ToString();
                    string sLog = "[" + sTypeName + "]" + " - Socket:" + iSocket.ToString() + "，Packet:" + iRes.ToString() + "/" + iLen.ToString();
                    DoLog(sFuncName, sLog);
                }
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }           
        }

        public static void DoLog(string sFuncName, string sLogContent)
        {
            try
            {
                if (bDoLog)
                {  
                    Socket_Cache.LogQueue.LogToQueue(sFuncName, sLogContent);
                }
            }
            catch (Exception ex)
            {
                DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }           
        }
        #endregion
    }
}
