using System;
using System.Windows.Forms;
using System.Diagnostics;
using EasyHook;
using WPELibrary.Lib;
using System.Reflection;

namespace WPELibrary
{
    public partial class Socket_SendListForm : Form
    {
        #region//窗体加载

        public Socket_SendListForm()
        {
            MultiLanguage.SetDefaultLanguage(MultiLanguage.DefaultLanguage);

            InitializeComponent();

            Socket_Cache.SendList.bShow_SendListForm = false;

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
                Socket_Cache.SendList.bShow_SendListForm = true;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        #endregion

        #region//初始化

        private void InitSendListForm()
        {
            try
            {
                string sInjectProcesName = Process.GetCurrentProcess().ProcessName;
                int iInjectProcessID = RemoteHooking.GetCurrentProcessId();

                this.Text = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_48) + sInjectProcesName + " [" + iInjectProcessID.ToString() + "]";

                this.bSendList.Enabled = true;
                this.bSendListStop.Enabled = false;

                if (Socket_Cache.SendList.UseSocket > 0)
                {
                    this.nudSocket.Value = Socket_Cache.SendList.UseSocket;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                dgvSendList.DataSource = Socket_Cache.SendList.dtSocketSendList;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                            Socket_Cache.SendList.DeleteSendList_ByIndex(iSelectIndex);
                        }

                        break;

                    case "tsmiClear":

                        Socket_Cache.SendList.SendListClear();

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
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }       
        }
        #endregion

        #region//开始发送

        private void bSend_Click(object sender, EventArgs e)
        {
            try
            {
                int iCheckedSendPacket = 0;

                for (int i = 0; i < dgvSendList.Rows.Count; i++)
                {
                    if (this.dgvSendList.Rows[i].Cells["cCheck"].Value != null)
                    {
                        if (this.dgvSendList.Rows[i].Cells["cCheck"].Value.ToString() == "1")
                        {
                            iCheckedSendPacket++;
                        }
                    }
                }

                if (iCheckedSendPacket > 0)
                {
                    bool bSocketOK = true;

                    if (this.cbUseSocket.Checked)
                    {
                        int iCheckSocket = (int)this.nudSocket.Value;

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
                        Socket_Cache.SendList.Loop_Send_CNT = 0;
                        Socket_Cache.SendList.SendList_Success_CNT = 0;
                        Socket_Cache.SendList.SendList_Fail_CNT = 0;
                        Socket_Cache.SendList.Loop_CNT = ((int)this.nudLoop_CNT.Value);
                        Socket_Cache.SendList.Loop_Int = ((int)this.nudLoop_Int.Value);

                        this.tlLoop_Send_CNT.Text = Socket_Cache.SendList.Loop_Send_CNT.ToString();
                        this.tlSendList_Success_CNT.Text = Socket_Cache.SendList.SendList_Success_CNT.ToString();
                        this.tlSendList_Fail_CNT.Text = Socket_Cache.SendList.SendList_Fail_CNT.ToString();

                        if (!bgwSendList.IsBusy)
                        {
                            bgwSendList.RunWorkerAsync();
                        }

                        this.bSendList.Enabled = false;
                        this.bSendListStop.Enabled = true;
                        this.gbSendListForm1.Enabled = false;
                        this.gbSendListForm2.Enabled = false;
                        this.gbSendListForm3.Enabled = false;
                        this.gbSendListForm4.Enabled = false;
                    }
                    else
                    {
                        Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_49));
                    }
                }
                else
                {
                    Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_84));
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
                bgwSendList.CancelAsync();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }
        #endregion        

        #region//发送封包列表（异步）

        private void bgwSendList_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                for (int i = 0; i < Socket_Cache.SendList.Loop_CNT; i++)
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
                                        iSocket = (int)this.nudSocket.Value;
                                    }
                                    else
                                    {
                                        iSocket = (int)this.dgvSendList.Rows[j].Cells["cSocket"].Value;
                                    }

                                    Socket_Cache.SendList.SendPacketList_ByIndex(iSocket, j);
                                    Socket_Cache.SendList.Loop_Send_CNT++;
                                }
                            }
                        }
                    }

                    if (this.nudLoop_CNT.Value > 1)
                    {
                        this.nudLoop_CNT.Value -= 1;
                    }                    

                    this.tlLoop_Send_CNT.Text = Socket_Cache.SendList.Loop_Send_CNT.ToString();
                    this.tlSendList_Success_CNT.Text = Socket_Cache.SendList.SendList_Success_CNT.ToString();
                    this.tlSendList_Fail_CNT.Text = Socket_Cache.SendList.SendList_Fail_CNT.ToString();
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        private void bgwSendList_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.bSendList.Enabled = true;
                this.bSendListStop.Enabled = false;
                this.gbSendListForm1.Enabled = true;
                this.gbSendListForm2.Enabled = true;
                this.gbSendListForm3.Enabled = true;
                this.gbSendListForm4.Enabled = true;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion
    }
}
