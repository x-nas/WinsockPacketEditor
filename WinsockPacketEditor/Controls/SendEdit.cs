using AntdUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class SendEdit : UserControl
    {
        private Form form;
        private SendInfo siSelect;
        private readonly SendExecute se = new SendExecute();
        private BindingList<PacketInfo> SendCollection;

        #region//窗体事件

        public SendEdit(Form form, SendInfo si)
        {
            InitializeComponent();
            this.siSelect = si;
            this.form = form;
        }

        private void SendEdit_Load(object sender, EventArgs e)
        {
            this.txtSendName.Text = this.siSelect.SName;
            this.cbSystemSocket.Checked = this.siSelect.SSystemSocket;
            this.nudLoopCNT.Value = this.siSelect.SLoopCNT;
            this.nudLoopINT.Value = this.siSelect.SLoopINT;
            this.SendCollection = new BindingList<PacketInfo>(this.siSelect.SCollection.ToList());
            this.txtNotes.Text = this.siSelect.SNotes;

            this.se.Worker.ProgressChanged -= this.Worker_ProgressChanged;
            this.se.Worker.ProgressChanged += this.Worker_ProgressChanged;
            this.se.Worker.RunWorkerCompleted -= this.Worker_RunWorkerCompleted;
            this.se.Worker.RunWorkerCompleted += this.Worker_RunWorkerCompleted;

            this.InitTable_SendCollection();
            this.InitMenu();
            this.Dark_Changed();
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
                }.SetFixed().SetLocalizationTitleID("Table.PacketList.Column.ID"),
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
                new AntdUI.Column("CellLinks", "操作")
                {
                    Render = (value, record, rowindex)=>
                    {
                        return new AntdUI.CellLink[]
                        {
                            new AntdUI.CellButton("bEdit", null, AntdUI.TTypeMini.Primary).SetIcon("EditOutlined"),
                            new AntdUI.CellButton("bDelete", null, AntdUI.TTypeMini.Error).SetIcon("CloseOutlined"),
                        };
                    },
                }.SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.PacketList.Column."),
            };

            this.tSendCollection.ColumnFont = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.tSendCollection.Binding(this.SendCollection);
        }

        private void InitMenu()
        {
            this.ddMenu.Items.AddRange(new AntdUI.SelectItem[]
            {
                new AntdUI.SelectItem("导入发送集")
                {
                    Tag = "Import",
                    LocalizationText = "SendEditForm.Import",
                    IconSvg = "FolderOpenOutlined",
                },
                new AntdUI.SelectItem("导出所有发送集")
                {
                    Tag = "Export",
                    LocalizationText = "SendEditForm.Export",
                    IconSvg = "DeliveredProcedureOutlined",
                },
                new AntdUI.SelectItem("清空所有发送集")
                {
                    Tag = "Clear",
                    LocalizationText = "SendEditForm.Clear",
                    IconSvg = "DeleteOutlined",
                },
            });
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

        private void Dark_Changed()
        {
            if (AntdUI.Config.IsDark)
            {
                this.pSendName.Back = 
                    this.pSendSocket.Back = 
                    this.pLoopCNT.Back = 
                    this.pLoopINT.Back =
                    this.txtNotes.BackColor =
                    this.tSendCollection.ColumnBack =
                    Operate.SystemConfig.Color_35;

                this.tSendCollection.ColumnFore = Color.Silver;
                this.tSendCollection.ForeColor = Color.LimeGreen;
            }
            else
            {
                this.pSendName.Back =
                    this.pSendSocket.Back =
                    this.pLoopCNT.Back =
                    this.pLoopINT.Back =
                    this.txtNotes.BackColor =
                    this.tSendCollection.ColumnBack = null;

                this.tSendCollection.ColumnFore = Color.Black;
                this.tSendCollection.ForeColor = Color.Green;
            }
        }

        #endregion

        #region//执行发送（异步）

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int iIndex = e.ProgressPercentage;

            this.tSendCollection.SelectedIndex = iIndex + 1;
            this.tSendCollection.ScrollLine(iIndex + 1, true);

            this.lTotal_Send_CNT.Text = this.se.Total_Send.ToString();
            this.lSend_Success_CNT.Text = this.se.Send_Success.ToString();
            this.lSend_Fail_CNT.Text = this.se.Send_Failure.ToString();
        }

        private void Worker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            try
            {
                string sSendName = this.txtSendName.Text.Trim();

                if (e.Cancelled)
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "发送已停止", TType.Warn)
                    {
                        LocalizationText = "SendEditForm.Send.Stop",
                    });
                }
                else if (e.Error != null)
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "发生错误: " + e.Error.Message, TType.Error)
                    {
                        LocalizationText = "SendEditForm.Send.Error" + e.Error.Message
                    });
                }
                else
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "发送执行完毕", TType.Success)
                    {
                        LocalizationText = "SendEditForm.Send.Success"
                    });
                }

                this.bExecute.Loading = false;
                this.bStop.Enabled = false;
                this.tlpSendCollectionSettings.Enabled = true;
                this.tSendCollection.Enabled = true;

                this.lTotal_Send_CNT.Text = this.se.Total_Send.ToString();
                this.lSend_Success_CNT.Text = this.se.Send_Success.ToString();
                this.lSend_Fail_CNT.Text = this.se.Send_Failure.ToString();
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(Worker_RunWorkerCompleted), ex);
            }
        }

        #endregion

        #region//发送集 - 菜单

        private void tSendCollection_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            if (e.Record is PacketInfo pi)
            {
                switch (e.Btn.Id)
                {
                    case "bEdit":

                        Operate.PacketConfig.Packet.OpenPacketEdit(this.form, pi);

                        break;

                    case "bDelete":

                        List<PacketInfo> piList = new List<PacketInfo>
                        {
                            pi,
                        };

                        Operate.SendConfig.Send.UpdateSendCollection_ByListAction(this.form, this.SendCollection, Operate.SystemConfig.ListAction.Delete, piList);

                        break;
                }
            }
        }

        private void tSendCollection_CellDoubleClick(object sender, TableClickEventArgs e)
        {
            if (e.Record is PacketInfo pi)
            {
                Operate.PacketConfig.Packet.OpenPacketEdit(this.form, pi);
            }
        }

        private void ddMenu_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            this.ddMenu.SelectedValue = null;

            switch (e.Value.ToString())
            {
                case "Import":

                    Operate.SendConfig.Send.UpdateSendCollection_ByListAction(this.form, this.SendCollection, Operate.SystemConfig.ListAction.Import, this.SendCollection.ToList());

                    break;

                case "Export":

                    if (this.SendCollection.Count > 0)
                    {
                        Operate.SendConfig.Send.UpdateSendCollection_ByListAction(this.form, this.SendCollection, Operate.SystemConfig.ListAction.Export, this.SendCollection.ToList());
                    }

                    break;

                case "Clear":

                    if (this.SendCollection.Count > 0)
                    {
                        Operate.SendConfig.Send.UpdateSendCollection_ByListAction(this.form, this.SendCollection, Operate.SystemConfig.ListAction.CleanUp, this.SendCollection.ToList());
                    }

                    break;
            }
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
                                Operate.SendConfig.Send.UpdateSendCollection_ByListAction(this.form, this.SendCollection, Operate.SystemConfig.ListAction.Top, piList);
                            }

                            break;

                        case "cmsUp":

                            if (piList.Count > 0)
                            {
                                Operate.SendConfig.Send.UpdateSendCollection_ByListAction(this.form, this.SendCollection, Operate.SystemConfig.ListAction.Up, piList);
                            }

                            break;

                        case "cmsDown":

                            if (piList.Count > 0)
                            {
                                Operate.SendConfig.Send.UpdateSendCollection_ByListAction(this.form, this.SendCollection, Operate.SystemConfig.ListAction.Down, piList);
                            }

                            break;

                        case "cmsBottom":

                            if (piList.Count > 0)
                            {
                                Operate.SendConfig.Send.UpdateSendCollection_ByListAction(this.form, this.SendCollection, Operate.SystemConfig.ListAction.Bottom, piList);
                            }

                            break;                        

                        case "cmsCopy":

                            if (piList.Count > 0)
                            {
                                Operate.SendConfig.Send.UpdateSendCollection_ByListAction(this.form, this.SendCollection, Operate.SystemConfig.ListAction.Copy, piList);
                                this.tSendCollection.ScrollBar.ValueY = tSendCollection.ScrollBar.MaxY;
                            }

                            break;

                        case "cmsDelete":

                            if (piList.Count > 0)
                            {
                                Operate.SendConfig.Send.UpdateSendCollection_ByListAction(this.form, this.SendCollection, Operate.SystemConfig.ListAction.Delete, piList);
                            }

                            break;
                    }

                    this.tSendCollection.SelectedIndex = -1;
                },
                new AntdUI.IContextMenuStripItem[]
                {
                    new AntdUI.ContextMenuStripItem("置顶", "Ctrl+⬆")
                    {
                        ID = "cmsTop",
                        IconSvg = "VerticalAlignTopOutlined",
                        LocalizationText = "Top",
                    },
                    new AntdUI.ContextMenuStripItemDivider(),
                    new AntdUI.ContextMenuStripItem("向上移动", "Alt+⬆")
                    {
                        ID = "cmsUp",
                        IconSvg = "ArrowUpOutlined",
                        LocalizationText = "Up",
                    },
                    new AntdUI.ContextMenuStripItem("向下移动", "Alt+⬇")
                    {
                        ID = "cmsDown",
                        IconSvg = "ArrowDownOutlined",
                        LocalizationText = "Down",
                    },
                    new AntdUI.ContextMenuStripItemDivider(),
                    new AntdUI.ContextMenuStripItem("置底", "Ctrl+⬇")
                    {
                        ID = "cmsBottom",
                        IconSvg = "VerticalAlignBottomOutlined",
                        LocalizationText = "Bottom",
                    },
                    new AntdUI.ContextMenuStripItemDivider(),                    
                    new AntdUI.ContextMenuStripItem("复制")
                    {
                        ID = "cmsCopy",
                        IconSvg = "CopyOutlined",
                        LocalizationText = "Copy",
                    },
                    new AntdUI.ContextMenuStripItem("删除")
                    {
                        ID = "cmsDelete",
                        IconSvg = "CloseOutlined",
                        LocalizationText = "Delete",
                    },
                }));
            }
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

                if (this.cbSystemSocket.Checked)
                {
                    if (Operate.SystemConfig.SystemSocket <= 0)
                    {
                        AntdUI.Message.open(new AntdUI.Message.Config(this.form, "系统套接字未设置", TType.Error)
                        {
                            LocalizationText = "SendEditForm.SystemSocket.Error"
                        });

                        return;
                    }
                }

                if (!this.SaveSend())
                {
                    return;
                }

                if (!this.se.Worker.IsBusy)
                {
                    this.bExecute.Loading = true;
                    this.bStop.Enabled = true;
                    this.tlpSendCollectionSettings.Enabled = false;
                    this.tSendCollection.Enabled = false;

                    se.StartSend(siSelect);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(bExecute_Click), ex);
            }
        }

        #endregion

        #region//停止

        private void bStop_Click(object sender, EventArgs e)
        {
            this.se.StopSend();
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.SaveSend())
                {
                    if (this.form is InterfaceInfo.IProxyMode pmForm)
                    {
                        pmForm.RefreshSendList();
                    }
                    else if (this.form is InterfaceInfo.IInjectMode imForm)
                    {
                        imForm.RefreshSendList();
                    }

                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "发送保存成功", TType.Success)
                    {
                        LocalizationText = "SendEditForm.Success"
                    });

                    this.se.StopSend();
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(bSave_Click), ex);

                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "发送保存失败", TType.Error)
                {
                    LocalizationText = "SendEditForm.Error"
                });
            }
        }

        private bool SaveSend()
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtSendName.Text.Trim()))
                {
                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "发送名称为空", TType.Error)
                    {
                        LocalizationText = "SendEditForm.SendName.Empty"
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
                Operate.DoLog(nameof(SaveSend), ex);
            }

            return false;
        }

        #endregion

        #region//退出

        private void bExit_Click(object sender, EventArgs e)
        {
            this.se.StopSend();
            this.Dispose();
        }

        #endregion        
    }
}
