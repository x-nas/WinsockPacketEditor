using System;
using System.Windows.Forms;
using System.Diagnostics;
using EasyHook;
using WPELibrary.Lib;
using System.Reflection;

namespace WPELibrary
{
    public partial class Socket_SendList_Form : Form
    {
        private string sLanguage = "";

        #region//窗体加载
        public Socket_SendList_Form()
        {
            InitializeComponent();

            Socket_Cache.SocketSendList.bShow_SendListForm = false;

            this.InitSendListDGV();
        }

        private void SocketSendList_Form_Load(object sender, EventArgs e)
        {
            this.InitSendListForm();            
        }
        #endregion

        #region//关闭窗体后
        private void SocketSendList_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                Socket_Cache.SocketSendList.bShow_SendListForm = true;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }            
        }
        #endregion

        #region//初始化窗体
        private void InitSendListForm()
        {
            try
            {
                this.pbLoading.Top = (ClientRectangle.Height - pbLoading.Height) / 2;
                this.pbLoading.Left = (ClientRectangle.Width - pbLoading.Width) / 2;
                this.pbLoading.Visible = false;

                string sInjectProcesName = Process.GetCurrentProcess().ProcessName;
                int iInjectProcessID = RemoteHooking.GetCurrentProcessId();

                sLanguage = MultiLanguage.GetDefaultLanguage("发送列表 - ", "Send List - ");
                this.Text = sLanguage + sInjectProcesName + " [" + iInjectProcessID.ToString() + "]";

                this.bSendList.Enabled = true;
                this.bSendListStop.Enabled = false;

                if (Socket_Cache.SocketSendList.UseSocket > 0)
                {
                    this.txtUseSocket.Text = Socket_Cache.SocketSendList.UseSocket.ToString();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }          
        }
        #endregion

        #region//初始化数据表
        private void InitSendListDGV()
        {
            try
            {
                dgvSendList.AutoGenerateColumns = false;
                dgvSendList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvSendList, true, null);
                dgvSendList.DataSource = Socket_Cache.SocketSendList.dtSocketSendList;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }
        }
        #endregion

        #region//右键菜单
        private void cmsSendList_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            try
            {
                string sItemText = e.ClickedItem.Name;
                this.cmsSendList.Close();

                switch (sItemText)
                {
                    case "tsmiDeleteSendList":

                        if (this.dgvSendList.SelectedRows.Count == 1)
                        {
                            int iSelectIndex = this.dgvSendList.SelectedRows[0].Index;
                            Socket_Cache.SocketSendList.DeleteSendList_ByIndex(iSelectIndex);
                        }

                        break;

                    case "tsmiClear":

                        Socket_Cache.SocketSendList.SendListClear();

                        break;

                    case "tsmiSaveSendList":

                        if (dgvSendList.Rows.Count > 0)
                        {
                            Socket_Operation.SaveSendListToFile();
                        }

                        break;

                    case "tsmiLoadSendList":

                        Socket_Operation.LoadFileToSendList();

                        break;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }       
        }
        #endregion

        #region//发送封包列表（异步）
        private void bSend_Click(object sender, EventArgs e)
        {
            try
            {
                bool bSocketOK = true;

                if (this.cbUseSocket.Checked)
                {
                    int iCheckSocket = Socket_Operation.CheckSocket(this.txtUseSocket.Text.ToString().Trim());

                    if (iCheckSocket > 0)
                    {
                        bSocketOK = true;
                    }
                    else
                    {
                        bSocketOK = false;
                    }
                }

                if (bSocketOK)
                {
                    this.bSendList.Enabled = false;
                    this.bSendListStop.Enabled = true;
                    this.cbUseSocket.Enabled = false;
                    this.txtUseSocket.Enabled = false;

                    Socket_Cache.SocketSendList.Loop_Send_CNT = 0;
                    Socket_Cache.SocketSendList.SendList_Success_CNT = 0;
                    Socket_Cache.SocketSendList.SendList_Fail_CNT = 0;
                    Socket_Cache.SocketSendList.Loop_CNT = ((int)this.nudLoop_CNT.Value);
                    Socket_Cache.SocketSendList.Loop_Int = ((int)this.nudLoop_Int.Value);

                    if (!bgwSendList.IsBusy)
                    {
                        bgwSendList.RunWorkerAsync();
                    }
                }
                else
                {
                    sLanguage = MultiLanguage.GetDefaultLanguage("请正确设置套接字!", "Please set the socket correctly!");
                    Socket_Operation.ShowMessageBox(sLanguage);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }
        }
        
        private void bgwSendList_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                for (int i = 0; i < Socket_Cache.SocketSendList.Loop_CNT; i++)
                {
                    for (int j = 0; j < dgvSendList.Rows.Count; j++)
                    {
                        if (bgwSendList.CancellationPending)
                        {
                            return;
                        }
                        else
                        {
                            if (this.dgvSendList.Rows[j].Cells["cCheck"].Value != null)
                            {
                                if (this.dgvSendList.Rows[j].Cells["cCheck"].Value.ToString() == "1")
                                {
                                    int iSocket = 0;

                                    if (this.cbUseSocket.Checked)
                                    {
                                        iSocket = Socket_Operation.CheckSocket(this.txtUseSocket.Text.ToString().Trim());
                                    }
                                    else
                                    {
                                        iSocket = Socket_Operation.CheckSocket(this.dgvSendList.Rows[j].Cells["cSocket"].Value.ToString().Trim());
                                    }

                                    Socket_Cache.SocketSendList.SendPacketList_ByIndex(iSocket, j);

                                    Socket_Cache.SocketSendList.Loop_Send_CNT++;
                                }
                            }
                        }
                    }
                    
                    int iLoop_UnSend = Socket_Cache.SocketSendList.Loop_CNT - Socket_Cache.SocketSendList.Loop_Send_CNT;

                    if (iLoop_UnSend > 0)
                    {
                        this.nudLoop_CNT.Value = iLoop_UnSend;
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }            

            this.bSendList.Enabled = true;
            this.bSendListStop.Enabled = false;
            this.cbUseSocket.Enabled = true;
            this.txtUseSocket.Enabled = true;
        }
        #endregion        

        #region//停止按钮
        private void bSendStop_Click(object sender, EventArgs e)
        {
            try
            {
                bgwSendList.CancelAsync();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }            

            this.bSendList.Enabled = true;
            this.bSendListStop.Enabled = false;
            this.cbUseSocket.Enabled = true;
            this.txtUseSocket.Enabled = true;
        }
        #endregion

        #region//计时器
        private void tSendList_Tick(object sender, EventArgs e)
        {
            try
            {
                this.tlLoop_Send_CNT.Text = Socket_Cache.SocketSendList.Loop_Send_CNT.ToString();
                this.tlSendList_Success_CNT.Text = Socket_Cache.SocketSendList.SendList_Success_CNT.ToString();
                this.tlSendList_Fail_CNT.Text = Socket_Cache.SocketSendList.SendList_Fail_CNT.ToString();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(ex.Message);
            }            
        }
        #endregion       

        #region//全选/取消
        private void cbSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                string sSelect = "0";

                if (this.cbSelectAll.Checked)
                {
                    sSelect = "1";
                }

                for (int i = 0; i < this.dgvSendList.Rows.Count; i++)
                {
                    this.dgvSendList.Rows[i].Cells["cCheck"].Value = sSelect;
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
