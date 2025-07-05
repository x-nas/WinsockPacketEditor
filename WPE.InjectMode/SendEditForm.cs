using AntdUI;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using WPE.Lib;

namespace WPE.InjectMode
{
    public partial class SendEditForm : Form
    {
        private InjectModeForm imForm;
        private SendInfo siSelect;
        private readonly SendExecute ss = new SendExecute();
        private BindingList<PacketInfo> SendCollection;

        #region//窗体事件

        public SendEditForm(InjectModeForm form, SendInfo si)
        {
            InitializeComponent();

            if (si == null)
            {
                string Title = AntdUI.Localization.Get("InjectModeForm.EditSend.Error", "加载发送数据出错");
                string Content = AntdUI.Localization.Get("InjectModeForm.CheckSystemLog", "请检查系统日志");
                AntdUI.Notification.error(form, Title, Content, AntdUI.TAlignFrom.TR);
                this.Close();
            }
            else
            {
                this.siSelect = si;
                this.imForm = form;
            }
        }

        private void SendEditForm_Load(object sender, EventArgs e)
        {
            this.txtSendName.Text = this.siSelect.SName;            
            this.cbSystemSocket.Checked = this.siSelect.SSystemSocket;
            this.nudLoopCNT.Value = this.siSelect.SLoopCNT;
            this.nudLoopINT.Value = this.siSelect.SLoopINT;
            this.SendCollection = new BindingList<PacketInfo>(this.siSelect.SCollection.ToList());
            this.txtNotes.Text = this.siSelect.SNotes;

            this.ss.Worker.ProgressChanged -= this.Worker_ProgressChanged;
            this.ss.Worker.ProgressChanged += this.Worker_ProgressChanged;
            this.ss.Worker.RunWorkerCompleted -= this.Worker_RunWorkerCompleted;
            this.ss.Worker.RunWorkerCompleted += this.Worker_RunWorkerCompleted;

            this.InitTable_SendCollection();            
        }

        private void SendEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.ss.StopSend();
        }

        private void InitTable_SendCollection()
        {
            tSendCollection.Columns = new AntdUI.ColumnCollection {
                new AntdUI.Column("", "序号", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return (rowindex + 1);
                    },
                }.SetFixed().SetLocalizationTitleID("Table.PacketList.Column."),                
                new AntdUI.Column("PacketType", "类别", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return Operate.PacketConfig.Packet.GetName_ByPacketType((Operate.PacketConfig.Packet.PacketType)value);
                    },
                }.SetLocalizationTitleID("Table.PacketList.Column."),
                new AntdUI.Column("PacketSocket", "套接字", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.PacketList.Column."),
                new AntdUI.Column("PacketFrom", "本机地址").SetLocalizationTitleID("Table.PacketList.Column."),
                new AntdUI.Column("PacketTo", "远端地址").SetLocalizationTitleID("Table.PacketList.Column."),
                new AntdUI.Column("PacketLen", "长度", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.PacketList.Column."),
                new AntdUI.Column("PacketData", "数据").SetLocalizationTitleID("Table.PacketList.Column."),
            };

            if (AntdUI.Config.IsDark)
            {
                this.tSendCollection.ColumnFore = Color.Silver;
                this.tSendCollection.ForeColor = Color.LimeGreen;
            }
            else
            {
                this.tSendCollection.ColumnFore = Color.Black;
                this.tSendCollection.ForeColor = Color.Green;
            }

            this.tSendCollection.ColumnFont = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.tSendCollection.Binding(this.SendCollection);
        }

        private void txtSendName_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtSendName.Text.Trim()))
            {
                this.txtSendName.Status = TType.Error;
            }
            else
            {
                this.txtSendName.Status = TType.Success;
            }
        }        

        #endregion

        #region//检查发送设置

        private bool CheckSendInfo()
        {
            if (this.cbSystemSocket.Checked)
            {
                if (Operate.SystemConfig.SystemSocket <= 0)
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this, "系统套接字未设置", TType.Success)
                    {
                        LocalizationText = "SystemSocket.Error"
                    });

                    return false;
                }
            }

            return true;
        }

        #endregion

        #region//执行

        private void bExecute_Click(object sender, EventArgs e)
        {
            if (this.SendCollection.Count == 0)
            { 
                return;
            }

            if (!this.CheckSendInfo())
            {
                return;
            }

            try
            {
                if (!this.ss.Worker.IsBusy)
                {
                    this.bExecute.Loading = true;
                    this.bStop.Enabled = true;
                    this.tlpSendCollectionSettings.Enabled = false;

                    //if (this.dgvSendCollection.ContextMenuStrip != null)
                    //{
                    //    this.dgvSendCollection.ContextMenuStrip.Enabled = false;
                    //}

                    string sSendName = this.txtSendName.Text.Trim();
                    bool bSystemSocket = this.cbSystemSocket.Checked;
                    int iLoopCNT = ((int)this.nudLoopCNT.Value);
                    int iLoopINT = ((int)this.nudLoopINT.Value);

                    ss.StartSend(sSendName, bSystemSocket, iLoopCNT, iLoopINT, this.SendCollection);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//执行发送（异步）

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int iIndex = e.ProgressPercentage;

            this.lTotal_Send_CNT.Text = this.ss.Total_Send.ToString();
            this.lSend_Success_CNT.Text = this.ss.Send_Success.ToString();
            this.lSend_Fail_CNT.Text = this.ss.Send_Failure.ToString();
        }

        private void Worker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            try
            {
                string sMsg = string.Empty;
                string sSendName = this.txtSendName.Text.Trim();

                if (e.Cancelled)
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this, "发送已停止", TType.Warn)
                    {
                        LocalizationText = "System.Send.Warn",
                    });
                }
                else if (e.Error != null)
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this, "发生错误: " + e.Error.Message, TType.Error)
                    {
                        LocalizationText = "System.Send.Error" + e.Error.Message
                    });
                }
                else
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this, "发送执行完毕", TType.Success)
                    {
                        LocalizationText = "System.Send.Success"
                    });
                }

                this.bExecute.Loading = false;
                this.bStop.Enabled = false;
                this.tlpSendCollectionSettings.Enabled = true;                

                //if (this.dgvSendCollection.ContextMenuStrip != null)
                //{
                //    this.dgvSendCollection.ContextMenuStrip.Enabled = true;
                //}

                this.lTotal_Send_CNT.Text = this.ss.Total_Send.ToString();
                this.lSend_Success_CNT.Text = this.ss.Send_Success.ToString();
                this.lSend_Fail_CNT.Text = this.ss.Send_Failure.ToString();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }        

        #endregion

        #region//停止

        private void bStop_Click(object sender, EventArgs e)
        {
            this.ss.StopSend();
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(this.txtSendName.Text.Trim()))
            {
                AntdUI.Message.open(new AntdUI.Message.Config(this, "发送名称为空", TType.Error)
                {
                    LocalizationText = "SendEditForm.SendName.Error"
                });

                return;
            }

            try
            {  
                string SName_New = this.txtSendName.Text.Trim();
                bool SSystemSocket_New = this.cbSystemSocket.Checked;
                int SLoopCNT_New = ((int)this.nudLoopCNT.Value);
                int SLoopINT_New = ((int)this.nudLoopINT.Value);
                string SNotes_New = this.txtNotes.Text.Trim();

                Operate.SendConfig.Send.UpdateSend(
                    this.siSelect, 
                    SName_New, 
                    SSystemSocket_New, 
                    SLoopCNT_New, 
                    SLoopINT_New, 
                    this.SendCollection, 
                    SNotes_New);

                this.Close();
                this.imForm.RefreshSendList();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//退出

        private void bExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        #endregion        
    }
}
