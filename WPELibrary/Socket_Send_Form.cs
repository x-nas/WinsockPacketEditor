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
        private string sLanguage_UI = "";

        #region//窗体加载
        public Socket_Send_Form(int iSelectIndex)
        {
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

                sLanguage_UI = MultiLanguage.GetDefaultLanguage("发送封包 -【 序号", "Send -【 ID");
                this.Text = sLanguage_UI + Socket_Cache.SocketList.lstRecPacket[Select_Index].Index.ToString() + " 】- " + sInjectProcesName + " [" + iInjectProcessID.ToString() + "]";

                this.bSend.Enabled = true;
                this.bSendStop.Enabled = false;

                this.InitSocketSendInfo();
                this.ShowStepValue();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }            
        }

        #endregion

        #region//初始化数据表
        private void InitSocketSendDGV()
        {
            try
            {
                dgvSocketSend.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvSocketSend, true, null);

                int iColNum = Socket_Cache.SocketList.lstRecPacket[Select_Index].ResLen;

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
                        Width = 40,
                        MaxInputLength = 2
                    };
                    DataGridViewTextBoxColumn dgvColumn = dataGridViewTextBoxColumn;
                    dgvColumn.DefaultCellStyle.BackColor = Color.LightGoldenrodYellow;
                    dgvColumn.DefaultCellStyle.ForeColor = Color.RoyalBlue;

                    dgvSocketSend.Columns.Add(dgvColumn);
                }

                dgvSocketSend.RowHeadersWidth = 100;
                dgvSocketSend.Rows.Add();

                sLanguage_UI = MultiLanguage.GetDefaultLanguage("封包数据", "Data");
                dgvSocketSend.Rows[0].HeaderCell.Value = sLanguage_UI;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }
        }

        private void InitSocketSendInfo()
        {
            try
            {
                this.txtSend_Socket.Text = Socket_Cache.SocketList.lstRecPacket[Select_Index].Socket.ToString();
                this.txtSend_Len.Text = Socket_Cache.SocketList.lstRecPacket[Select_Index].ResLen.ToString();
                this.txtSend_IP.Text = Socket_Cache.SocketList.lstRecPacket[Select_Index].To.Split(':')[0];
                this.txtSend_Port.Text = Socket_Cache.SocketList.lstRecPacket[Select_Index].To.Split(':')[1];

                string sData = Socket_Operation.Byte_To_Hex(Socket_Cache.SocketList.lstRecPacket[Select_Index].Buffer);
                ShowSocketSendData(sData);
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
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
                Socket_Operation.DoLog(ex.Message);
            }
        }
        #endregion

        #region//发送按钮
        private void bSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.cbStep.Checked)
                {
                    string sStepIndex = this.lStepIndex_Value.Text.Trim();
                    string sStepLen = this.lStepLen_Value.Text.Trim();

                    if (string.IsNullOrEmpty(sStepIndex) || string.IsNullOrEmpty(sStepLen))
                    {
                        sLanguage_UI = MultiLanguage.GetDefaultLanguage("请正确设置递进位置!", "Please set the progressive position correctly!");
                        Socket_Operation.ShowMessageBox(sLanguage_UI);
                        return;
                    }
                }

                this.bSend.Enabled = false;
                this.bSendStop.Enabled = true;

                Send_CNT = 0;
                Send_Success_CNT = 0;
                Send_Fail_CNT = 0;

                if (!bgwSendPacket.IsBusy)
                {
                    bgwSendPacket.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }
        }
        #endregion

        #region//发送停止按钮
        private void bSendStop_Click(object sender, EventArgs e)
        {
            try
            {
                bgwSendPacket.CancelAsync();

                this.bSend.Enabled = true;
                this.bSendStop.Enabled = false;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }
        }
        #endregion        

        #region//获取封包数据
        private string GetSocketSendData()
        {
            string sResult = "";

            try
            {
                int iLen = int.Parse(this.txtSend_Len.Text.Trim());

                if (iLen > this.dgvSocketSend.Columns.Count)
                {
                    iLen = this.dgvSocketSend.Columns.Count;
                }

                for (int i = 0; i < iLen; i++)
                {
                    string sCell = this.dgvSocketSend.Rows[0].Cells[i].Value.ToString().Trim();

                    if (!string.IsNullOrEmpty(sCell))
                    {
                        sResult += sCell + " ";
                    }
                    else
                    {
                        sResult = "";
                        break;
                    }
                }

                sResult = sResult.Trim();
            }
            catch (Exception ex)
            {
                sResult = "";

                Socket_Operation.DoLog(ex.Message);
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

        private void dgvSocketSend_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int iCellIndex = this.dgvSocketSend.SelectedCells[0].ColumnIndex;
                this.nudStepIndex.Value = iCellIndex + 1;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }
        }

        private void ShowStepValue()
        {
            try
            {
                int iStepIndex = int.Parse(this.nudStepIndex.Value.ToString()) - 1;
                int iStepLen = int.Parse(this.nudStepLen.Value.ToString());

                if (this.dgvSocketSend.Rows[0].Cells[iStepIndex].Value != null)
                {
                    string sStepData = this.dgvSocketSend.Rows[0].Cells[iStepIndex].Value.ToString();

                    if (!string.IsNullOrEmpty(sStepData))
                    {
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
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
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
                Socket_Operation.DoLog(ex.Message);
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
                Socket_Operation.DoLog(ex.Message);
            }            
        }
        #endregion

        #region//计时器
        private void tSend_Tick(object sender, EventArgs e)
        {
            try
            {
                this.tlSendPacket_CNT.Text = this.Send_CNT.ToString();
                this.tlSend_Success_CNT.Text = this.Send_Success_CNT.ToString();
                this.tlSend_Fail_CNT.Text = this.Send_Fail_CNT.ToString();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
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

                switch (sItemText)
                {
                    case "tsmiBatchSend":

                        int iIndex = Socket_Cache.SocketList.lstRecPacket[Select_Index].Index;
                        int iSocket = Socket_Operation.CheckSocket(this.txtSend_Socket.Text.Trim());
                        string sIPTo = Socket_Cache.SocketList.lstRecPacket[Select_Index].To;
                        string sData = this.GetSocketSendData();
                        byte[] bBuff = Socket_Operation.Hex_To_Byte(sData);
                        int iResLen = bBuff.Length;

                        Socket_Cache.SocketSendList.AddSendList_New(iIndex, "", iSocket, sIPTo, iResLen, sData, bBuff);

                        if (Socket_Cache.SocketSendList.bShow_SendListForm)
                        {
                            Socket_SendList_Form sslForm = new Socket_SendList_Form();
                            sslForm.Show();
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }            
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
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }            
        }

        private void SendPacket()
        {
            try
            {
                int iSocket = Socket_Operation.CheckSocket(this.txtSend_Socket.Text.Trim());

                if (iSocket == 0)
                {
                    sLanguage_UI = MultiLanguage.GetDefaultLanguage("套接字设置错误!", "Socket setting error!");
                    Socket_Operation.ShowMessageBox(sLanguage_UI);
                    return;
                }

                string sSendData = this.GetSocketSendData();

                if (sSendData.Equals(""))
                {
                    sLanguage_UI = MultiLanguage.GetDefaultLanguage("封包数据错误!", "Packet data error!");
                    Socket_Operation.ShowMessageBox(sLanguage_UI);
                    return;
                }

                if (this.cbStep.Checked)
                {
                    if (this.lStepIndex_Value.Text.Equals("") || this.lStepLen_Value.Text.Equals(""))
                    {
                        sLanguage_UI = MultiLanguage.GetDefaultLanguage("递进设置错误!", "Progressive setting error!");
                        Socket_Operation.ShowMessageBox(sLanguage_UI);
                        return;
                    }
                }

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

                            Socket_Operation.DoLog(ex.Message);
                        }                        

                        Send_CNT++;
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }
        }
        #endregion        
    }
}
