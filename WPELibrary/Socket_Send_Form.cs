using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using WPELibrary.Lib;
using System.Diagnostics;
using System.Drawing;
using EasyHook;
using System.Reflection;

namespace WPELibrary
{
    public partial class Socket_Send_Form : Form
    {
        private int Loop_CNT = 0;
        private int Loop_Int = 0;
        private int Send_CNT = 0;
        private int Send_Success_CNT = 0;
        private int Send_Fail_CNT = 0;
        private int Select_Index = 0;
        private string Select_Hex = "";

        #region//窗体加载

        public Socket_Send_Form(int iSelectIndex)
        {
            MultiLanguage.SetDefaultLanguage(MultiLanguage.DefaultLanguage);

            InitializeComponent();

            this.Select_Index = iSelectIndex;
            this.InitSocketSendDGV();
        }

        private void SocketSend_Form_Load(object sender, EventArgs e)
        {
            try
            {
                string sInjectProcesName = Process.GetCurrentProcess().ProcessName;
                int iInjectProcessID = RemoteHooking.GetCurrentProcessId();

                this.Text = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_42) + Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketIndex.ToString() + " 】- " + sInjectProcesName + " [" + iInjectProcessID.ToString() + "]";

                this.bSend.Enabled = true;
                this.bSendStop.Enabled = false;                

                this.InitSocketSendInfo();
                this.ShowStepValue();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        #endregion

        #region//初始化数据表

        private void InitSocketSendDGV()
        {
            try
            {
                dgvSocketSend.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvSocketSend, true, null);

                int iColNum = Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketLen;

                if (iColNum >= 65535)
                {
                    iColNum = 65534;
                }

                for (int i = 0; i < iColNum; i++)
                {
                    DataGridViewTextBoxColumn dataGridViewTextBoxColumn = new DataGridViewTextBoxColumn()
                    {
                        Name = "col" + (i + 1).ToString("000"),
                        HeaderText = (i + 1).ToString("000"),
                        Width = 50,
                        MaxInputLength = 2
                    };

                    DataGridViewTextBoxColumn dgvColumn = dataGridViewTextBoxColumn;
                    dgvColumn.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                    dgvColumn.DefaultCellStyle.ForeColor = Color.RoyalBlue;

                    dgvSocketSend.Columns.Add(dgvColumn);
                }
                
                dgvSocketSend.Rows.Add();                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void InitSocketSendInfo()
        {
            try
            {
                this.txtSend_Socket.Text = Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketSocket.ToString();
                this.txtSend_Len.Text = Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketLen.ToString();
                this.txtSend_IP.Text = Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketTo.Split(':')[0];
                this.txtSend_Port.Text = Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketTo.Split(':')[1];

                string sData = Socket_Operation.ByteToString("HEX", Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketBuffer);

                ShowSocketSendData(sData);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void dgvSocketSend_ColumnAdded(object sender, DataGridViewColumnEventArgs e)
        {
            try
            {
                e.Column.FillWeight = 1;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//发送按钮

        private void bSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CheckSendPacket())
                {
                    if (this.cbStep.Checked)
                    {
                        string sStepIndex = this.lStepIndex_Value.Text.Trim();
                        string sStepLen = this.lStepLen_Value.Text.Trim();

                        if (string.IsNullOrEmpty(sStepIndex) || string.IsNullOrEmpty(sStepLen))
                        {
                            Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_44));
                            return;
                        }
                    }

                    this.bSend.Enabled = false;
                    this.bSendStop.Enabled = true;

                    this.gbSendForm1.Enabled = false;
                    this.gbSendForm2.Enabled = false;
                    this.gbSendForm3.Enabled = false;
                    this.gbSendForm4.Enabled = false;

                    Send_CNT = 0;
                    Send_Success_CNT = 0;
                    Send_Fail_CNT = 0;

                    if (!bgwSendPacket.IsBusy)
                    {
                        bgwSendPacket.RunWorkerAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//停止按钮

        private void bSendStop_Click(object sender, EventArgs e)
        {
            try
            {
                bgwSendPacket.CancelAsync();

                this.bSend.Enabled = true;
                this.bSendStop.Enabled = false;

                this.gbSendForm1.Enabled = true;
                this.gbSendForm2.Enabled = true;
                this.gbSendForm3.Enabled = true;
                this.gbSendForm4.Enabled = true;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion        

        #region//获取发送数据

        private string GetSendData()
        {
            string sResult = "";

            try
            {
                int iLen = int.Parse(this.txtSend_Len.Text.Trim());

                if (iLen > this.dgvSocketSend.Columns.Count)
                {
                    iLen = this.dgvSocketSend.Columns.Count;
                }

                string sCell = "";

                for (int i = 0; i < iLen; i++)
                {
                    if (this.dgvSocketSend.Rows[0].Cells[i].Value != null)
                    {
                        if (!string.IsNullOrEmpty(this.dgvSocketSend.Rows[0].Cells[i].Value.ToString().Trim()))
                        {
                            sCell = this.dgvSocketSend.Rows[0].Cells[i].Value.ToString().Trim();

                            sResult += sCell + " ";
                        }
                        else
                        {
                            return "";
                        }
                    }
                    else
                    {
                        return "";
                    }
                }

                sResult = sResult.Trim();
            }
            catch (Exception ex)
            {                
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                return "";
            }          

            return sResult;
        }

        #endregion

        #region//显示递进数据

        private void nudStepIndex_ValueChanged(object sender, EventArgs e)
        {
            this.ShowStepValue();                        
        }
     
        private void nudStepLen_ValueChanged(object sender, EventArgs e)
        {
            this.ShowStepValue();
        }

        private void dgvSocketSend_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                object value = dgvSocketSend.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                if (!Socket_Operation.CheckHEX(value))
                {
                    Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_83));
                    dgvSocketSend.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = this.Select_Hex;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void dgvSocketSend_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                this.Select_Hex = this.dgvSocketSend.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString().Trim();
                this.nudStepIndex.Value = e.ColumnIndex + 1;

                this.ShowStepValue();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void ShowStepValue()
        {
            try
            {
                if (this.nudStepIndex.Value > Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketLen)
                {
                    Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_82));

                    this.nudStepIndex.Value = Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketLen;
                }
                else
                {
                    int iStepIndex = int.Parse(this.nudStepIndex.Value.ToString()) - 1;
                    int iStepLen = int.Parse(this.nudStepLen.Value.ToString());

                    if (this.dgvSocketSend.Rows[0].Cells[iStepIndex].Value != null)
                    {
                        if (!string.IsNullOrEmpty(this.dgvSocketSend.Rows[0].Cells[iStepIndex].Value.ToString().Trim()))
                        {
                            string sStepData = this.dgvSocketSend.Rows[0].Cells[iStepIndex].Value.ToString().Trim();
                            string sStepLenData = Socket_Operation.GetValueByLen_HEX(sStepData, iStepLen);

                            this.lStepIndex_Value.Text = sStepData;
                            this.lStepLen_Value.Text = sStepLenData;
                        }
                        else
                        {
                            this.lStepIndex_Value.Text = "";
                            this.lStepLen_Value.Text = "";
                        }
                    }
                    else
                    {
                        this.lStepIndex_Value.Text = "";
                        this.lStepLen_Value.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        #endregion

        #region//黏贴封包数据

        private void dgvSocketSend_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Control && e.KeyCode == Keys.V)
                {
                    string sClipboardText = Clipboard.GetText().Trim();
                    ShowSocketSendData(sClipboardText);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        private void ShowSocketSendData(string sData)
        {
            try
            {
                int iRow = dgvSocketSend.CurrentCell.RowIndex;
                int iCol = dgvSocketSend.CurrentCell.ColumnIndex;

                string[] DataCells = sData.Split(' ');
                for (int i = 0; i < DataCells.Length; i++)
                {
                    if (iCol + i < this.dgvSocketSend.ColumnCount)
                    {
                        dgvSocketSend[iCol + i, iRow].Value = Convert.ChangeType(DataCells[i], dgvSocketSend[iCol + i, iRow].ValueType);
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        #endregion        

        #region//右键菜单

        private void cmsSocketSend_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                string sItemText = e.ClickedItem.Name;
                this.cmsSocketSend.Close();

                int iIndex = Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketIndex;
                string sData = this.GetSendData();

                switch (sItemText)
                {
                    //添加到发送列表
                    case "tsmiBatchSend":
                        
                        int iSocket = Socket_Operation.CheckSocket(this.txtSend_Socket.Text.Trim());
                        string sIPTo = Socket_Cache.SocketList.lstRecPacket[Select_Index].PacketTo;                        
                        byte[] bBuff = Socket_Operation.Hex_To_Byte(sData);
                        int iResLen = bBuff.Length;

                        Socket_Cache.SendList.AddSendList_New(iIndex, "", iSocket, sIPTo, iResLen, sData, bBuff);

                        if (Socket_Cache.SendList.bShow_SendListForm)
                        {
                            Socket_SendList_Form sslForm = new Socket_SendList_Form();
                            sslForm.Show();
                        }

                        break;

                    //添加到滤镜列表
                    case "tsmiAddToFilter":
                        
                        string sFName = Process.GetCurrentProcess().ProcessName.Trim() + " [" + iIndex.ToString() + "]";
                        Socket_FilterInfo.FilterMode FMode = Socket_FilterInfo.FilterMode.Normal;                        
                        Socket_FilterInfo.StartFrom FStartFrom = Socket_FilterInfo.StartFrom.Head;
                        int iFModifyCNT = 1;
                        string sFSearch = Socket_Operation.GetFilterString_ByHEX(sData);
                        int iFSearchLen = int.Parse(this.txtSend_Len.Text.Trim());                        

                        Socket_Cache.FilterList.AddFilter_New(sFName, FMode, FStartFrom, iFModifyCNT, sFSearch, iFSearchLen, "", iFSearchLen, false);

                        Socket_Operation.ShowMessageBox(String.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_27), sFName));

                        break;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        #endregion

        #region//检查发送数据

        private bool CheckSendPacket()
        { 
            try
            {
                int iSocket = Socket_Operation.CheckSocket(this.txtSend_Socket.Text.Trim());

                if (iSocket == 0)
                {
                    Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_45));
                    return false;
                }

                string sSendData = this.GetSendData();

                if (sSendData.Equals(""))
                {
                    Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_46));
                    return false;
                }

                if (this.cbStep.Checked)
                {
                    if (this.lStepIndex_Value.Text.Equals("") || this.lStepLen_Value.Text.Equals(""))
                    {
                        Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_47));
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                return false;
            }

            return true;
        }

        #endregion

        #region//发送封包（异步）

        private void bgwSendPacket_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            SendPacket();            
        }

        private void bgwSendPacket_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.bSend.Enabled = true;
                this.bSendStop.Enabled = false;

                this.gbSendForm1.Enabled = true;
                this.gbSendForm2.Enabled = true;
                this.gbSendForm3.Enabled = true;
                this.gbSendForm4.Enabled = true;

                this.tlSendPacket_CNT.Text = this.Send_CNT.ToString();
                this.tlSend_Success_CNT.Text = this.Send_Success_CNT.ToString();                
                this.tlSend_Fail_CNT.Text = this.Send_Fail_CNT.ToString();                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        private void SendPacket()
        {
            try
            {
                int iSocket = Socket_Operation.CheckSocket(this.txtSend_Socket.Text.Trim());
                string sSendData = this.GetSendData();

                Loop_CNT = (int)this.nudLoop_CNT.Value;
                Loop_Int = (int)this.nudLoop_Int.Value;

                for (int i = 0; i < Loop_CNT; i++)
                {
                    if (bgwSendPacket.CancellationPending)
                    {
                        return;
                    }
                    else
                    {
                        try
                        {
                            if (this.cbStep.Checked)
                            {
                                int iStepIndex = int.Parse(this.nudStepIndex.Value.ToString()) - 1;
                                int iStepLen = int.Parse(this.nudStepLen.Value.ToString());

                                sSendData = Socket_Operation.ReplaceValueByIndexAndLen_HEX(sSendData, iStepIndex, iStepLen);

                                if (string.IsNullOrEmpty(sSendData))
                                {
                                    Send_Fail_CNT++;
                                    return;
                                }
                            }

                            byte[] bBuff = Socket_Operation.Hex_To_Byte(sSendData);

                            IntPtr ipSend = Marshal.AllocHGlobal(bBuff.Length);
                            Marshal.Copy(bBuff, 0, ipSend, bBuff.Length);

                            if (bBuff.Length > 0)
                            {
                                bool bReturn = WinSockHook.SendPacket(iSocket, ipSend, bBuff.Length);

                                if (bReturn)
                                {
                                    Send_Success_CNT++;
                                }
                                else
                                {
                                    Send_Fail_CNT++;
                                }

                                int iLoop_UnSend = Loop_CNT - Send_CNT;

                                if (iLoop_UnSend > 0)
                                {
                                    this.nudLoop_CNT.Value = iLoop_UnSend;
                                }

                                Thread.Sleep(Loop_Int);
                            }
                        }
                        catch (Exception ex)
                        {
                            Send_Fail_CNT++;

                            Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                        }                        

                        Send_CNT++;
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion
    }
}
