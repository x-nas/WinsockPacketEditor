using System;
using System.Data;

namespace WPELibrary.Lib
{
    public static class Filter_List
    {
        private static int FilterListNum = 10;
        private static string FilterLog = "";

        public static DataTable dtFilterList = new DataTable();

        #region//初始化
        public static void InitFilterList()
        {
            dtFilterList.Columns.Add("ISCheck", typeof(bool));
            dtFilterList.Columns.Add("FilterIndex", typeof(string));
            dtFilterList.Columns.Add("FilterName", typeof(string));
            dtFilterList.Columns.Add("FilterSearch", typeof(string));
            dtFilterList.Columns.Add("FilterModify", typeof(string));

            for (int i = 0; i < FilterListNum; i++)
            {
                DataRow drFilter = dtFilterList.NewRow();
                drFilter["ISCheck"] = false;
                drFilter["FilterIndex"] = i.ToString();
                drFilter["FilterName"] = "滤镜 " + (i + 1).ToString();
                drFilter["FilterSearch"] = "";
                drFilter["FilterModify"] = "";

                dtFilterList.Rows.Add(drFilter);
            }            
        }
        #endregion

        #region//执行滤镜
        public static void DoFilter(IntPtr ipBuff, int iLen)
        {
            try
            {
                byte[] bBuff = Socket_Operation.GetByteFromIntPtr(ipBuff, iLen);

                for (int i = 0;i < dtFilterList.Rows.Count;i ++)
                {
                    FilterLog = "";

                    if (Check_ISCheck(i))
                    {
                        FilterLog += "[滤镜 " + (i + 1).ToString() + "] - ";                        

                        if (Check_FilterSearch(i, bBuff))
                        {
                            string sFilterModify = dtFilterList.Rows[i]["FilterModify"].ToString().Trim();

                            if (!string.IsNullOrEmpty(sFilterModify))
                            {
                                string[] ssModify = sFilterModify.Split(',');

                                foreach (string sModify in ssModify)
                                {
                                    string[] sModifyValue = sModify.Split('|');
                                    int iIndex = int.Parse(sModifyValue[0].ToString().Trim());
                                    string sValue = sModifyValue[1].ToString().Trim();

                                    FilterLog += (iIndex + 1).ToString("000") + "：" + bBuff[iIndex].ToString("X2") + " => " + sValue + "，";

                                    bBuff[iIndex] = Socket_Operation.Hex_To_Byte(sValue)[0];
                                }

                                bool bSetOK = Socket_Operation.SetByteToIntPtr(bBuff, ipBuff, iLen);

                                if (bSetOK)
                                {
                                    FilterLog += "过滤完成！";
                                    Socket_Operation.DoLog(FilterLog);

                                    break;
                                }
                            }
                        }
                        else
                        {
                            FilterLog += "不匹配！";
                        }

                        Socket_Operation.DoLog(FilterLog);
                    }
                }                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }
        }
        #endregion

        #region//检查是否启用滤镜
        private static bool Check_ISCheck(int iFilterIndex)
        {
            bool bResult = bool.Parse(dtFilterList.Rows[iFilterIndex]["ISCheck"].ToString());

            return bResult;
        }
        #endregion

        #region//检查是否匹配搜索内容
        private static bool Check_FilterSearch(int iFilterIndex, byte[] bBuff)
        {
            bool bResult = true;           

            string sFilterSearch = dtFilterList.Rows[iFilterIndex]["FilterSearch"].ToString().Trim();

            if (!string.IsNullOrEmpty(sFilterSearch))
            {
                string[] ssSearch = sFilterSearch.Split(',');

                foreach (string sSearch in ssSearch)
                {
                    string[] sSearchValue = sSearch.Split('|');
                    int iIndex = int.Parse(sSearchValue[0].ToString().Trim());
                    string sValue = sSearchValue[1].ToString().Trim();

                    string sBufferValue = bBuff[iIndex].ToString("X2");

                    if (!sValue.Equals(sBufferValue))
                    {
                        FilterLog += (iIndex + 1).ToString("000") + "：" + sBufferValue + " / " + sValue + "，";

                        bResult = false;
                        break;
                    }
                }
            }
            else
            {
                FilterLog += "搜索条件";
                bResult = false;
            }

            return bResult;
        }
        #endregion
    }
}
