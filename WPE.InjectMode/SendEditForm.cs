using AntdUI;
using System;
using System.Collections.Generic;
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
            this.Text = AntdUI.Localization.Get("SendEditForm", "编辑发送");

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
            try
            {
                if (this.SendCollection.Count == 0)
                {
                    return;
                }

                if (!this.SaveSend())
                {
                    return;
                }

                if (!this.ss.Worker.IsBusy)
                {
                    this.bExecute.Loading = true;
                    this.bStop.Enabled = true;
                    this.tlpSendCollectionSettings.Enabled = false;
                    this.tSendCollection.Enabled = false;

                    ss.StartSend(siSelect);
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
            
            this.tSendCollection.SelectedIndex = iIndex + 1;
            this.tSendCollection.ScrollLine(iIndex + 1, true);

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
                this.tSendCollection.Enabled = true;

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

        #region//发送集 - 菜单

        private void sSendCollection_SelectIndexChanged(object sender, IntEventArgs e)
        {
            switch (this.sSendCollection.SelectIndex)
            {
                //导入
                case 0:

                    Operate.SendConfig.Send.UpdateSendCollection_ByListAction(this, this.SendCollection, Operate.SystemConfig.ListAction.Import, this.SendCollection.ToList());

                    break;

                //导出
                case 1:

                    if (this.SendCollection.Count > 0)
                    {
                        Operate.SendConfig.Send.UpdateSendCollection_ByListAction(this, this.SendCollection, Operate.SystemConfig.ListAction.Export, this.SendCollection.ToList());
                    }

                    break;

                //清空
                case 2:

                    if (this.SendCollection.Count > 0)
                    {
                        Operate.SendConfig.Send.UpdateSendCollection_ByListAction(this, this.SendCollection, Operate.SystemConfig.ListAction.CleanUp, this.SendCollection.ToList());
                    }

                    break;
            }

            this.sSendCollection.SelectIndex = -1;
        }

        #endregion

        #region//发送集 - 右键菜单

        private void tSendCollection_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (this.SendCollection.Count == 0)
                {
                    return;
                }

                AntdUI.ContextMenuStrip.open(new AntdUI.ContextMenuStrip.Config(tSendCollection, (item) =>
                {
                    List<PacketInfo> piList = new List<PacketInfo>();

                    foreach (int SelectIndex in this.tSendCollection.SelectedIndexs)
                    {
                        piList.Add(this.SendCollection[SelectIndex - 1]);
                    }

                    switch (item.ID)
                    {
                        case "cmsTop":

                            if (piList.Count > 0)
                            {
                                Operate.SendConfig.Send.UpdateSendCollection_ByListAction(this, this.SendCollection, Operate.SystemConfig.ListAction.Top, piList);
                            }

                            break;

                        case "cmsUp":

                            if (piList.Count > 0)
                            {
                                Operate.SendConfig.Send.UpdateSendCollection_ByListAction(this, this.SendCollection, Operate.SystemConfig.ListAction.Up, piList);
                            }

                            break;

                        case "cmsDown":

                            if (piList.Count > 0)
                            {
                                Operate.SendConfig.Send.UpdateSendCollection_ByListAction(this, this.SendCollection, Operate.SystemConfig.ListAction.Down, piList);
                            }

                            break;

                        case "cmsBottom":

                            if (piList.Count > 0)
                            {
                                Operate.SendConfig.Send.UpdateSendCollection_ByListAction(this, this.SendCollection, Operate.SystemConfig.ListAction.Bottom, piList);
                            }

                            break;

                        case "cmsEdit":

                            if (piList.Count > 0)
                            {
                                AntdUI.Drawer.open(new AntdUI.Drawer.Config(this, new PacketEditForm(this.imForm, piList[0]))
                                {
                                    Align = AntdUI.TAlignMini.Right,
                                    Mask = true,
                                    MaskClosable = false,
                                    DisplayDelay = 0,
                                });
                            }

                            break;

                        case "cmsCopy":

                            if (piList.Count > 0)
                            {
                                Operate.SendConfig.Send.UpdateSendCollection_ByListAction(this, this.SendCollection, Operate.SystemConfig.ListAction.Copy, piList);
                                this.tSendCollection.ScrollBar.ValueY = tSendCollection.ScrollBar.MaxY;
                            }

                            break;

                        case "cmsDelete":

                            if (piList.Count > 0)
                            {
                                Operate.SendConfig.Send.UpdateSendCollection_ByListAction(this, this.SendCollection, Operate.SystemConfig.ListAction.Delete, piList);
                            }

                            break;                        
                    }

                    this.tSendCollection.SelectedIndex = -1;
                },
                new AntdUI.IContextMenuStripItem[]
                {
                    new AntdUI.ContextMenuStripItem("置顶", "Ctrl+向上键")
                {
                    ID = "cmsTop",
                    IconSvg = "VerticalAlignTopOutlined",
                    LocalizationText = "System.cms.Top",
                },
                    new AntdUI.ContextMenuStripItemDivider(),
                    new AntdUI.ContextMenuStripItem("向上移动", "Alt+向上键")
                {
                    ID = "cmsUp",
                    IconSvg = "ArrowUpOutlined",
                },
                    new AntdUI.ContextMenuStripItem("向下移动", "Alt+向下键")
                {
                    ID = "cmsDown",
                    IconSvg = "ArrowDownOutlined",
                },
                    new AntdUI.ContextMenuStripItemDivider(),
                    new AntdUI.ContextMenuStripItem("置底", "Ctrl+向下键")
                {
                    ID = "cmsBottom",
                    IconSvg = "VerticalAlignBottomOutlined",
                },
                    new AntdUI.ContextMenuStripItemDivider(),
                    new AntdUI.ContextMenuStripItem("编辑")
                {
                    ID = "cmsEdit",
                    IconSvg = "EditOutlined",
                },
                    new AntdUI.ContextMenuStripItem("复制")
                {
                    ID = "cmsCopy",
                    IconSvg = "CopyOutlined",
                },
                    new AntdUI.ContextMenuStripItem("删除")
                {
                    ID = "cmsDelete",
                    IconSvg = "CloseOutlined",
                },                    
                }));
            }
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            if (this.SaveSend())
            {
                this.Close();
                this.imForm.RefreshSendList();
            }
        }

        private bool SaveSend()
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtSendName.Text.Trim()))
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this, "发送名称为空", TType.Error)
                    {
                        LocalizationText = "SendEditForm.SendName.Error"
                    });

                    return false;
                }

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

                return true;
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return false;
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
