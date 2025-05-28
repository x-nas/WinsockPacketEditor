using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using WPELibrary.Lib;

namespace WPELibrary
{
    public partial class Socket_SendListForm : Form
    {
        private Socket_SendInfo SSI;
        private string SendName = string.Empty;
        private BindingList<Socket_PacketInfo> SendCollection;
        private Socket_PacketInfo spiSelect;
        private readonly Socket_Send ss = new Socket_Send();

        #region//窗体事件

        public Socket_SendListForm(Socket_SendInfo ssi)
        {
            MultiLanguage.SetDefaultLanguage(MultiLanguage.DefaultLanguage);
            InitializeComponent();

            if (ssi != null)
            { 
                this.SSI = ssi;

                this.InitSendListForm();
                this.InitSendListDGV();
            }
        }

        private void SocketSendList_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.ss.StopSend();
        }

        private void dgvSendCollection_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvSendCollection.SelectedRows.Count > 0)
                {
                    int iSelectIndex = this.dgvSendCollection.SelectedRows[0].Index;

                    if (iSelectIndex >= 0 && iSelectIndex < this.SendCollection.Count)
                    {
                        this.spiSelect = this.SendCollection[iSelectIndex];
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion        

        #region//初始化

        private void InitSendListDGV()
        {
            try
            {
                dgvSendCollection.AutoGenerateColumns = false;
                dgvSendCollection.DataSource = this.SendCollection;
                dgvSendCollection.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvSendCollection, true, null);                
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void InitSendListForm()
        {
            try
            {
                string sSID = this.SSI.SID.ToString().ToUpper();
                this.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_48), sSID);

                this.SendName = this.SSI.SName;
                this.txtSendName.Text = this.SendName;
                this.cbSystemSocket.Checked = this.SSI.SSystemSocket;
                this.lSystemSocket.Text = Socket_Cache.System.SystemSocket.ToString();
                this.nudLoop_CNT.Value = this.SSI.SLoopCNT;
                this.nudLoop_INT.Value = this.SSI.SLoopINT;
                this.SendCollection = new BindingList<Socket_PacketInfo>(this.SSI.SCollection.ToList());
                this.rtbNotes.Text = this.SSI.SNotes;

                this.InitSend();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }          
        }

        private void InitSend()
        {
            try
            {
                this.ss.Worker.ProgressChanged -= this.Worker_ProgressChanged;
                this.ss.Worker.ProgressChanged += this.Worker_ProgressChanged;

                this.ss.Worker.RunWorkerCompleted -= this.Worker_RunWorkerCompleted;
                this.ss.Worker.RunWorkerCompleted += this.Worker_RunWorkerCompleted;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void dgvSendCollection_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dgvSendCollection.Columns["cID"].Index)
                {
                    e.Value = (e.RowIndex + 1).ToString();
                    e.FormattingApplied = true;
                }
                else if (e.ColumnIndex == dgvSendCollection.Columns["cType"].Index)
                {
                    string sType = dgvSendCollection.Rows[e.RowIndex].Cells["cType"].Value.ToString();

                    e.Value = Socket_Cache.SocketPacket.GetName_ByPacketType(Socket_Cache.SocketPacket.GetPacketType_ByString(sType));
                    e.FormattingApplied = true;
                }
                else if (e.ColumnIndex == dgvSendCollection.Columns["cData"].Index)
                {
                    byte[] buffer = (byte[])dgvSendCollection.Rows[e.RowIndex].Cells["cBuffer"].Value;

                    e.Value = Socket_Operation.BytesToString(Socket_Cache.SocketPacket.EncodingFormat.Hex, buffer);
                    e.FormattingApplied = true;
                }
                else if (e.ColumnIndex == dgvSendCollection.Columns["cLength"].Index)
                {
                    byte[] buffer = (byte[])dgvSendCollection.Rows[e.RowIndex].Cells["cBuffer"].Value;

                    e.Value = buffer.Length.ToString();
                    e.FormattingApplied = true;
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion                

        #region//保存按钮

        private void bSave_Click(object sender, EventArgs e)
        {
            try
            {
                string SName_New = this.txtSendName.Text.Trim();

                if (string.IsNullOrEmpty(SName_New))
                {
                    Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_19));
                    return;
                }

                bool SSystemSocket_New = this.cbSystemSocket.Checked;           
                int SLoopCNT_New = ((int)this.nudLoop_CNT.Value);
                int SLoopINT_New = ((int)this.nudLoop_INT.Value);
                string SNotes_New = this.rtbNotes.Text.Trim();

                Socket_Cache.Send.UpdateSend(this.SSI, SName_New, SSystemSocket_New, SLoopCNT_New, SLoopINT_New, this.SendCollection, SNotes_New);

                this.Close();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//关闭按钮

        private void bClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion        

        #region//执行按钮

        private void bExecute_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.SendCollection.Count > 0)
                {
                    if (this.CheckSendInfo())
                    {
                        if (!this.ss.Worker.IsBusy)
                        {
                            this.bExecute.Enabled = false;
                            this.bStop.Enabled = true;
                            this.tlpParameter.Enabled = false;

                            if (this.dgvSendCollection.ContextMenuStrip != null)
                            {
                                this.dgvSendCollection.ContextMenuStrip.Enabled = false;
                            }

                            bool bSystemSocket = this.cbSystemSocket.Checked;
                            int iLoopCNT = ((int)this.nudLoop_CNT.Value);
                            int iLoopINT = ((int)this.nudLoop_INT.Value);

                            ss.StartSend(this.SendName, bSystemSocket, iLoopCNT, iLoopINT, this.SendCollection);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void Worker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Cancelled)
                {
                    string sMsg = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_163), this.SendName);
                    Socket_Operation.ShowMessageBox(sMsg);
                }
                else if (e.Error != null)
                {
                    string sMsg = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_164), this.SendName, e.Error.Message);
                    Socket_Operation.ShowMessageBox(sMsg);
                }
                else
                {
                    string sMsg = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_165), this.SendName);
                    Socket_Operation.ShowMessageBox(sMsg);
                }

                this.bExecute.Enabled = true;
                this.bStop.Enabled = false;
                this.tlpParameter.Enabled = true;

                if (this.dgvSendCollection.ContextMenuStrip != null)
                {
                    this.dgvSendCollection.ContextMenuStrip.Enabled = true;
                }

                this.tlTotal_Send_CNT.Text = this.ss.Total_Send.ToString();
                this.tlSend_Success_CNT.Text = this.ss.Send_Success.ToString();
                this.tlSend_Fail_CNT.Text = this.ss.Send_Failure.ToString();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            try
            {
                int iIndex = e.ProgressPercentage;             

                this.tlTotal_Send_CNT.Text = this.ss.Total_Send.ToString();
                this.tlSend_Success_CNT.Text = this.ss.Send_Success.ToString();
                this.tlSend_Fail_CNT.Text = this.ss.Send_Failure.ToString();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//停止按钮

        private void bStop_Click(object sender, EventArgs e)
        {
            this.ss.StopSend();
        }        

        #endregion

        #region//检查发送设置

        private bool CheckSendInfo()
        {
            bool bReturn = true;

            try
            {
                if (this.cbSystemSocket.Checked)
                {
                    if (Socket_Cache.System.SystemSocket <= 0)
                    {
                        Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_49));
                        return false;
                    }                   
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return bReturn;
        }

        #endregion

        #region//发送集菜单

        private void cmsSendCollection_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string sItemText = e.ClickedItem.Name;
            cmsSendCollection.Close();

            try
            {
                if (this.dgvSendCollection.Rows.Count > 0)
                {
                    List<Socket_PacketInfo> spiList = Socket_Operation.GetSelectedSendCollection(this.dgvSendCollection, this.SendCollection);

                    if (spiList.Count > 0)
                    {
                        switch (sItemText)
                        {
                            case "cmsSendList_Send":

                                if (this.spiSelect != null)
                                {
                                    Socket_Operation.ShowSendForm(spiSelect);
                                }
                                
                                break;

                            case "cmsSendList_Top":
                                Socket_Cache.Send.UpdateSendCollection_ByListAction(this.SendCollection, Socket_Cache.System.ListAction.Top, spiList);
                                break;

                            case "cmsSendList_Up":
                                Socket_Cache.Send.UpdateSendCollection_ByListAction(this.SendCollection, Socket_Cache.System.ListAction.Up, spiList);
                                break;

                            case "cmsSendList_Down":
                                Socket_Cache.Send.UpdateSendCollection_ByListAction(this.SendCollection, Socket_Cache.System.ListAction.Down, spiList);
                                break;

                            case "cmsSendList_Bottom":
                                Socket_Cache.Send.UpdateSendCollection_ByListAction(this.SendCollection, Socket_Cache.System.ListAction.Bottom, spiList);
                                break;

                            case "cmsSendList_Delete":
                                Socket_Cache.Send.UpdateSendCollection_ByListAction(this.SendCollection, Socket_Cache.System.ListAction.Delete, spiList);
                                break;

                            case "cmsSendList_Export":
                                Socket_Cache.Send.UpdateSendCollection_ByListAction(this.SendCollection, Socket_Cache.System.ListAction.Export, spiList);
                                break;

                            case "cmsSendList_Import":
                                Socket_Cache.Send.UpdateSendCollection_ByListAction(this.SendCollection, Socket_Cache.System.ListAction.Import, spiList);
                                break;

                            case "cmsSendList_CleanUp":
                                Socket_Cache.Send.UpdateSendCollection_ByListAction(this.SendCollection, Socket_Cache.System.ListAction.CleanUp, spiList);
                                break;
                        }

                        this.dgvSendCollection.ClearSelection();

                        foreach (Socket_PacketInfo spi in spiList)
                        {
                            int iIndex = SendCollection.IndexOf(spi);

                            if (iIndex > -1 && iIndex < dgvSendCollection.RowCount)
                            {
                                this.dgvSendCollection.Rows[iIndex].Selected = true;
                                dgvSendCollection.FirstDisplayedScrollingRowIndex = iIndex;
                            }
                        }
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
