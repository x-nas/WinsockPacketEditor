using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using WPELibrary.Lib;
using System.Data;
using System.Diagnostics;
using EasyHook;

namespace WPELibrary
{
    public partial class SocketSend_Form : Form
    {  
        private int SendPacketCNT = 0;
        private int Send_Success_CNT = 0;
        private int Send_Fail_CNT = 0;

        private WinSockHook ws = new WinSockHook();
        private SocketOperation so = new SocketOperation();

        public string Send_Index, Send_Socket, Send_Len, Send_IPTo;
        public byte[] Send_Byte;        

        public SocketSend_Form()
        {
            InitializeComponent();            
        }

        //窗体加载
        private void SocketSend_Form_Load(object sender, EventArgs e)
        {
            string sInjectProcesName = Process.GetCurrentProcess().ProcessName;
            int iInjectProcessID = RemoteHooking.GetCurrentProcessId();

            this.Text = "发送封包 -【 序号 " + this.Send_Index + " 】- " + sInjectProcesName + " [" + iInjectProcessID.ToString() + "]";

            this.bSend.Enabled = true;
            this.bSendStop.Enabled = false;

            this.InitSendSocketInfo();
            this.ShowStepValue();
        }

        //初始化
        private void InitSendSocketInfo()
        {
            try
            {
                this.txtSend_Socket.Text = Send_Socket;
                this.txtSend_Len.Text = Send_Len;
                this.txtSend_IP.Text = Send_IPTo.Split(':')[0];
                this.txtSend_Port.Text = Send_IPTo.Split(':')[1];
                this.rtbSocketSend_Data.Text = so.Byte_To_Hex(Send_Byte);
            }
            catch (Exception ex)
            {
                //         
            }            
        }

        //发送按钮
        private void bSend_Click(object sender, EventArgs e)
        {
            if (this.cbStep.Checked)
            {
                string sStepIndex = this.lStepIndex.Text.Trim();
                string sStepLen = this.lStepLen.Text.Trim();

                if (string.IsNullOrEmpty(sStepIndex) || string.IsNullOrEmpty(sStepLen))
                {
                    MessageBox.Show("请正确设置递进位置!");
                    return;
                }
            }            

            this.bSend.Enabled = false;
            this.bSendStop.Enabled = true;

            SendPacketCNT = 0;
            Send_Success_CNT = 0;
            Send_Fail_CNT = 0;

            if (!bgwSendPacket.IsBusy)
            {
                bgwSendPacket.RunWorkerAsync();
            }
        }

        //发送停止按钮
        private void bSendStop_Click(object sender, EventArgs e)
        {
            bgwSendPacket.CancelAsync();
            this.bSend.Enabled = true;
            this.bSendStop.Enabled = false;
        }

        //右键菜单
        private void cmsSocketSend_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string sItemText = e.ClickedItem.Text;

            if (sItemText.Equals("添加到发送列表"))
            {

                string sSendSocket = this.txtSend_Socket.Text.Trim();
                string sSendLen = this.txtSend_Len.Text.Trim();
                string sSendPacket = this.rtbSocketSend_Data.Text.Trim();
                string sSendCNT = this.txtSend_CNT.Text.Trim();
                string sSendInt = this.txtSend_Int.Text.Trim();
                byte[] bBuff = so.Hex_To_Byte(sSendPacket);

                DataRow dr = SocketSend.dtSocketBatchSend.NewRow();
                dr[0] = int.Parse(Send_Index);
                dr[1] = "";
                dr[2] = int.Parse(sSendSocket);
                dr[3] = Send_IPTo;
                dr[4] = bBuff.Length;
                dr[5] = sSendPacket;
                dr[6] = bBuff;
                SocketSend.dtSocketBatchSend.Rows.Add(dr);

                if (!SocketSend.bHasBatchSendForm)
                {
                    SocketBatchSend_Form sbsForm = new SocketBatchSend_Form();
                    sbsForm.Show();
                }
            }
        }

        //递进位置
        private void nudStepIndex_ValueChanged(object sender, EventArgs e)
        {
            this.ShowStepValue();                        
        }

        //递进步长
        private void nudStepLen_ValueChanged(object sender, EventArgs e)
        {
            this.ShowStepValue();
        }

        //显示递进数据
        private void ShowStepValue()
        {            
            int iStepIndex = int.Parse(this.nudStepIndex.Value.ToString()) - 1;            
            int iStepLen = int.Parse(this.nudStepLen.Value.ToString());
            string sData = this.rtbSocketSend_Data.Text.Trim();

            string sStepIndex = so.GetValueByIndex_HEX(sData, iStepIndex);
            string sStepLen = so.GetValueByLen_HEX(sStepIndex, iStepLen);          

            this.lStepIndex.Text = sStepIndex;
            this.lStepLen.Text = sStepLen;
        }        

        //计时器
        private void tSend_Tick(object sender, EventArgs e)
        {
            this.tlSendPacket_CNT.Text = this.SendPacketCNT.ToString();
            this.tlSend_Success_CNT.Text = this.Send_Success_CNT.ToString();
            this.tlSend_Fail_CNT.Text = this.Send_Fail_CNT.ToString();
        }

        //发送封包
        public void SendPacket()
        {
            string sSendSocket = this.txtSend_Socket.Text.Trim();
            string sSendLen = this.txtSend_Len.Text.Trim();
            string sSendPacket = this.rtbSocketSend_Data.Text;
            string sSendCNT = this.txtSend_CNT.Text.Trim();
            string sSendInt = this.txtSend_Int.Text.Trim();

            int iCNT = int.Parse(sSendCNT);
            int iInt = int.Parse(sSendInt);

            for (int i = 0; i < iCNT; i++)
            {
                if (bgwSendPacket.CancellationPending)
                {
                    return;
                }
                else
                {
                    try
                    {
                        int iSocket = int.Parse(sSendSocket);
                        int iLen = int.Parse(sSendLen);

                        if (this.cbStep.Checked)
                        {
                            int iStepIndex = int.Parse(this.nudStepIndex.Value.ToString()) - 1;
                            int iStepLen = int.Parse(this.nudStepLen.Value.ToString());

                            sSendPacket = so.ReplaceValueByIndexAndLen_HEX(sSendPacket, iStepIndex, iStepLen);

                            if (string.IsNullOrEmpty(sSendPacket))
                            {
                                Send_Fail_CNT++;
                                return;
                            }
                        }

                        byte[] bBuff = so.Hex_To_Byte(sSendPacket);

                        IntPtr ipSend = Marshal.AllocHGlobal(bBuff.Length);
                        Marshal.Copy(bBuff, 0, ipSend, bBuff.Length);

                        if (iSocket > 0 && bBuff.Length > 0)
                        {
                            bool bReturn = ws.SendPacket(iSocket, ipSend, bBuff.Length);

                            if (bReturn)
                            {
                                Send_Success_CNT++;
                            }
                            else
                            {
                                Send_Fail_CNT++;
                            }
                            
                            int iSendLeft = iCNT - SendPacketCNT;

                            if (iSendLeft > 0)
                            {
                                this.txtSend_CNT.Text = iSendLeft.ToString();
                            }

                            Thread.Sleep(iInt);
                        }
                    }
                    catch
                    {
                        Send_Fail_CNT++;
                    }

                    SendPacketCNT++;
                }
            }
                
        }

        //多线程
        private void bgwSendPacket_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {           
            SendPacket();

            this.bSend.Enabled = true;
            this.bSendStop.Enabled = false;
        }
    }
}
