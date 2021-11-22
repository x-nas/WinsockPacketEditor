using System;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;
using System.Data;
using System.Diagnostics;
using EasyHook;

namespace WPELibrary
{
    public partial class SocketBatchSend_Form : Form
    {
        private Lib.WinSockHook ws = new Lib.WinSockHook();
        private Lib.SocketOperation so = new Lib.SocketOperation();

        private int SendBatchCNT = 0;
        private int Send_Success_CNT = 0;
        private int Send_Fail_CNT = 0;

        public SocketBatchSend_Form()
        {
            InitializeComponent();
        }

        //窗体加载
        private void SocketBatchSend_Form_Load(object sender, EventArgs e)
        {
            string sInjectProcesName = Process.GetCurrentProcess().ProcessName;
            int iInjectProcessID = RemoteHooking.GetCurrentProcessId();

            this.Text = "发送列表 - " + sInjectProcesName + " [" + iInjectProcessID.ToString() + "]";

            Lib.SocketSend.bHasBatchSendForm = true;

            this.dgBatchSend.AutoGenerateColumns = false;
            this.bSend.Enabled = true;
            this.bSendStop.Enabled = false;

            this.InitBatchSendSocketInfo();
        }

        //初始化
        private void InitBatchSendSocketInfo()
        {
            if (Lib.SocketSend.iUseSocket > 0)
            {
                this.txtUseSocket.Text = Lib.SocketSend.iUseSocket.ToString();
            }

            dgBatchSend.DataSource = Lib.SocketSend.dtSocketBatchSend;
        }

        //右键菜单
        private void cmsBatchSend_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string sItemText = e.ClickedItem.Text;

            if (sItemText.Equals("从列表中移除"))
            {
                if (this.dgBatchSend.SelectedRows.Count == 1)
                {
                    int iSelectIndex = this.dgBatchSend.SelectedRows[0].Index;

                    Lib.SocketSend.dtSocketBatchSend.Rows[iSelectIndex].Delete();
                }
            }
            else if (sItemText.Equals("清空发送列表"))
            {
                Lib.SocketSend.dtSocketBatchSend.Rows.Clear();
            }            
            else
            {
                //
            }
        }

        //发送按钮
        private void bSend_Click(object sender, EventArgs e)
        {
            bool bSocketOK = true;

            if (this.cbUseSocket.Checked)
            {
                int iUseSocket = 0;
                string sUseSocket = this.txtUseSocket.Text.Trim();

                if (string.IsNullOrEmpty(sUseSocket))
                {
                    bSocketOK = false;
                }
                else
                {
                    try
                    {
                        iUseSocket = int.Parse(sUseSocket);

                        if (iUseSocket <= 0)
                        {
                            bSocketOK = false;
                        }
                    }
                    catch
                    {
                        bSocketOK = false;
                    }
                }               
            }

            if (bSocketOK)
            {
                this.bSend.Enabled = false;
                this.bSendStop.Enabled = true;

                this.cbUseSocket.Enabled = false;
                this.txtUseSocket.Enabled = false;

                SendBatchCNT = 0;
                Send_Success_CNT = 0;
                Send_Fail_CNT = 0;

                if (!bgwSendPacket.IsBusy)
                {
                    bgwSendPacket.RunWorkerAsync();
                }
            }
            else
            {
                MessageBox.Show("请正确设置套接字！");
            }
        }

        //发送停止按钮
        private void bSendStop_Click(object sender, EventArgs e)
        {
            bgwSendPacket.CancelAsync();
            this.bSend.Enabled = true;
            this.bSendStop.Enabled = false;
        }

        //计时器
        private void tSend_Tick(object sender, EventArgs e)
        {
            this.tlSendBatch_CNT.Text = this.SendBatchCNT.ToString();
            this.tlSend_Success_CNT.Text = this.Send_Success_CNT.ToString();
            this.tlSend_Fail_CNT.Text = this.Send_Fail_CNT.ToString();
        }

        //循环发送封包列表
        public void SendPacket()
        {
            try
            {
                int iSendCNT = int.Parse(this.txtSend_CNT.Text.Trim());
                int iSendInt = int.Parse(this.txtSend_Int.Text.Trim());

                //循环发送次数
                for (int iSC = 0; iSC < iSendCNT; iSC++)
                {
                    //循环发送列表
                    for (int iBS = 0; iBS < this.dgBatchSend.Rows.Count; iBS++)
                    {
                        if (bgwSendPacket.CancellationPending)
                        {
                            //停止发送
                            return;
                        }
                        else
                        {
                            //选择框
                            bool bCheck = false;
                            if (this.dgBatchSend.Rows[iBS].Cells["cCheck"].Value != null)
                            {
                                if (this.dgBatchSend.Rows[iBS].Cells["cCheck"].Value.ToString() == "1")
                                {
                                    bCheck = true;
                                }
                            }

                            if (bCheck)
                            {
                                //套接字
                                int iSendSocket = 0;
                                if (this.cbUseSocket.Checked)
                                {
                                    iSendSocket = int.Parse(this.txtUseSocket.Text.Trim());
                                }
                                else
                                {
                                    iSendSocket = int.Parse(this.dgBatchSend.Rows[iBS].Cells["cSocket"].Value.ToString());
                                }

                                //长度
                                int iSendLen = int.Parse(this.dgBatchSend.Rows[iBS].Cells["cLen"].Value.ToString());

                                //字节
                                byte[] bSendBuffer = (byte[])Lib.SocketSend.dtSocketBatchSend.Rows[iBS]["字节"];

                                if (iSendSocket > 0 && bSendBuffer.Length > 0)
                                {
                                    IntPtr ipSend = Marshal.AllocHGlobal(bSendBuffer.Length);
                                    Marshal.Copy(bSendBuffer, 0, ipSend, bSendBuffer.Length);

                                    bool bReturn = ws.SendPacket(iSendSocket, ipSend, bSendBuffer.Length);

                                    if (bReturn)
                                    {
                                        Send_Success_CNT++;
                                    }
                                    else
                                    {
                                        Send_Fail_CNT++;
                                    }

                                    Thread.Sleep(iSendInt);
                                }
                            }
                        }
                    }                    

                    SendBatchCNT++;
                    int iSendLeft = iSendCNT - SendBatchCNT;

                    if (iSendLeft > 0)
                    {
                        this.txtSend_CNT.Text = iSendLeft.ToString();
                    }
                }
            }
            catch(Exception ex)
            {
                string sError = ex.Message;             
            }
        }

        //多线程
        private void bgwSendPacket_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            SendPacket();

            this.bSend.Enabled = true;
            this.bSendStop.Enabled = false;

            this.cbUseSocket.Enabled = true;
            this.txtUseSocket.Enabled = true;
        }

        //关闭窗体后
        private void SocketBatchSend_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            Lib.SocketSend.bHasBatchSendForm = false;
        }      

        //保存此列表数据
        private void bSaveSocket_Click(object sender, EventArgs e)
        {
            int iSuccess = 0, iFail = 0;

            if (this.dgBatchSend.Rows.Count > 0)
            {
                if (sfdSaveSocket.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        FileStream fs = new FileStream(sfdSaveSocket.FileName, FileMode.Create);
                        StreamWriter sw = new StreamWriter(fs);

                        for (int i = 0; i < this.dgBatchSend.Rows.Count; i++)
                        {
                            try
                            {
                                byte[] bBuffer = (byte[])Lib.SocketSend.dtSocketBatchSend.Rows[i]["字节"];
                                
                                string sIndex = (i + 1).ToString();
                                string sNote = this.dgBatchSend.Rows[i].Cells["cNote"].Value.ToString().Trim();
                                string sSocket = this.dgBatchSend.Rows[i].Cells["cSocket"].Value.ToString().Trim();
                                string sIPTo = this.dgBatchSend.Rows[i].Cells["cIPTo"].Value.ToString().Trim();
                                string sLen = this.dgBatchSend.Rows[i].Cells["cLen"].Value.ToString().Trim();
                                string sData = so.Byte_To_Hex(bBuffer);

                                string sSave = sIndex + "|" + sNote + "|" + sSocket + "|" + sIPTo + "|" + sLen + "|" + sData;

                                sw.WriteLine(sSave);

                                iSuccess++;
                            }
                            catch
                            {
                                iFail++;
                            }                            
                        }

                        sw.Flush();
                        sw.Close();
                        fs.Close();

                        MessageBox.Show("保存完毕，成功【" + iSuccess + "】失败【" + iFail + "】！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("保存失败！错误：" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }                    
                }
            }
        }

        //加载发送列表
        private void bLoadSocket_Click(object sender, EventArgs e)
        {
            int iSuccess = 0, iFail = 0;

            try
            {
                this.ofdLoadSocket.ShowDialog();
                string filePath = this.ofdLoadSocket.FileName;

                if (!string.IsNullOrEmpty(filePath))
                {
                    string[] slSocket = File.ReadAllLines(filePath, Encoding.UTF8);

                    Lib.SocketSend.dtSocketBatchSend.Rows.Clear();

                    foreach (string sSocketTemp in slSocket)
                    {
                        try
                        {
                            string[] ss = sSocketTemp.Split('|');

                            string sIndex = ss[0];
                            string sNote = ss[1];
                            string sSocket = ss[2];
                            string sIPTo = ss[3];
                            string sLen = ss[4];
                            string sData = ss[5];

                            byte[] bBuffer = so.Hex_To_Byte(sData);

                            DataRow dr = Lib.SocketSend.dtSocketBatchSend.NewRow();
                            dr[0] = int.Parse(sIndex);
                            dr[1] = sNote;
                            dr[2] = int.Parse(sSocket);
                            dr[3] = sIPTo;
                            dr[4] = sLen;
                            dr[5] = sData;
                            dr[6] = bBuffer;
                            Lib.SocketSend.dtSocketBatchSend.Rows.Add(dr);

                            iSuccess++;
                        }
                        catch
                        {
                            iFail++;
                        }                        
                    }
                }

                MessageBox.Show("加载完毕，成功【" + iSuccess + "】失败【" + iFail + "】！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("加载失败！错误：" + ex.Message, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }                        
        }

        //全选/取消
        private void cbSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            string sSelect = "0";
            if (this.cbSelectAll.Checked)
            {
                sSelect = "1";
            }

            for (int i = 0;i < this.dgBatchSend.Rows.Count;i ++)
            {
                this.dgBatchSend.Rows[i].Cells["cCheck"].Value = sSelect;
            }
        }
    }
}
