using AntdUI;
using Be.Windows.Forms;
using EasyHook;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class PacketList : UserControl
    {
        private Form form;
        private bool bWakeUp = true;
        private bool SearchFromHead = true;
        private QuickList cQuickList = null;
        private readonly WinSockHook ws = new WinSockHook();

        #region//窗体事件

        public PacketList(Form form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void PacketList_Load(object sender, EventArgs e)
        {
            this.lProcessName.Text = Operate.ProcessConfig.GetInjectProcessName();
            this.lModuleName.Text = Operate.ProcessConfig.GetInjectModuleName();
            this.lWinsockInfo.Text = Operate.ProcessConfig.GetInjectWinsockInfo();
            this.lSpeedInfo.Text = Operate.PacketConfig.Packet.GetPacketSpeedInfo();
            this.hbPacketData.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();

            this.InitMenu();
            this.InitTable_PacketList();
            this.InitControl();
            this.Dark_Changed();
            this.SetColumnName_PacketList();

            this.cbPacketList_AutoRoll.Checked = Operate.PacketConfig.List.AutoRoll;
            this.cbPacketList_AutoClear.Checked = Operate.PacketConfig.List.AutoClear;
            this.txtPacketList_AutoClear.Value = Operate.PacketConfig.List.AutoClear_Value;
            this.PacketList_AutoClear_Changed();

            Operate.DoLog(nameof(PacketList_Load), this.lProcessName.Text);
        }

        private void InitMenu()
        {
            this.ddMenu.Items.AddRange(new AntdUI.SelectItem[]
            {
                new AntdUI.SelectItem("过滤设置")
                {
                    Tag = "LeachSettings",
                    LocalizationText = "ProxyModeForm.LeachSettings",
                    IconSvg = "FilterOutlined",
                },
                new AntdUI.SelectItem("拦截设置")
                {
                    Tag = "HookSettings",
                    LocalizationText = "ProxyModeForm.HookSettings",
                    IconSvg = "AimOutlined",
                },
                new AntdUI.SelectItem("列表设置")
                {
                    Tag = "ListSettings",
                    LocalizationText = "ProxyModeForm.ListSettings",
                    IconSvg = "OrderedListOutlined",
                },
                new AntdUI.SelectItem("快捷键设置")
                {
                    Tag = "HotKeySettings",
                    LocalizationText = "ProxyModeForm.HotKeySettings",
                    IconSvg = "GoldOutlined",
                },
                new AntdUI.SelectItem("备份设置")
                {
                    Tag = "BackUpSettings",
                    LocalizationText = "ProxyModeForm.BackUpSettings",
                    IconSvg = "DeliveredProcedureOutlined",
                },
                new AntdUI.SelectItem("系统设置")
                {
                    Tag = "SystemSettings",
                    LocalizationText = "ProxyModeForm.SystemSettings",
                    IconSvg = "SettingOutlined",
                },
            });
        }

        private void InitControl()
        {
            //QuickList
            if (this.splitterQuickList.InvokeRequired)
            {
                this.splitterQuickList.Invoke(new Action(() =>
                {
                    cQuickList = new QuickList(this.form);
                    cQuickList.Dock = DockStyle.Fill;
                    this.splitterQuickList.Panel1.Controls.Add(cQuickList);
                }));
            }
            else
            {
                cQuickList = new QuickList(this.form);
                cQuickList.Dock = DockStyle.Fill;
                this.splitterQuickList.Panel1.Controls.Add(cQuickList);
            }
        }

        public void Dark_Changed()
        {
            if (AntdUI.Config.IsDark)
            {
                this.dgvPacketList.BackgroundColor =
                    this.dgvPacketList.RowsDefaultCellStyle.BackColor =
                    this.dgvPacketList.ColumnHeadersDefaultCellStyle.BackColor = Operate.SystemConfig.Color_40;

                this.dgvPacketList.ForeColor = Color.LimeGreen;
                this.dgvPacketList.ColumnHeadersDefaultCellStyle.ForeColor = Color.Silver;

                this.hbPacketData.BackColor = Operate.SystemConfig.Color_40;
                this.hbPacketData.ForeColor = Color.Silver;
            }
            else
            {
                this.dgvPacketList.BackgroundColor =
                    this.dgvPacketList.RowsDefaultCellStyle.BackColor =
                    this.dgvPacketList.ColumnHeadersDefaultCellStyle.BackColor = Color.White;

                this.dgvPacketList.ForeColor = Color.Green;
                this.dgvPacketList.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;

                this.hbPacketData.BackColor = Color.White;
                this.hbPacketData.ForeColor = Color.Black;
            }
        }

        public void RefreshPacketData()
        {
            if (Operate.PacketConfig.List.piSelect != null)
            {
                DynamicByteProvider dbp = new DynamicByteProvider(Operate.PacketConfig.List.piSelect.PacketBuffer);
                hbPacketData.ByteProvider = dbp;
            }
        }

        #endregion

        #region//初始化数据表

        private void InitTable_PacketList()
        {
            this.dgvPacketList.AutoGenerateColumns = false;
            this.dgvPacketList.DataSource = Operate.PacketConfig.List.lstPacketInfo;
            this.dgvPacketList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvPacketList, true, null);
            this.dgvPacketList.ColumnHeadersDefaultCellStyle.Font = new Font("微软雅黑", 9, FontStyle.Bold);
        }

        private void dgvPacketList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            try
            {
                var row = dgvPacketList.Rows[e.RowIndex];
                if (e.RowIndex < Operate.PacketConfig.List.lstPacketInfo.Count)
                {
                    var filterAction = Operate.PacketConfig.List.lstPacketInfo[e.RowIndex].FilterAction;
                    var colors = Operate.SystemConfig.GetFilterColors(filterAction);
                    if (colors.HasValue)
                    {
                        row.DefaultCellStyle.ForeColor = colors.Value.ForeColor;
                        row.DefaultCellStyle.BackColor = colors.Value.BackColor;
                    }
                }

                switch (e.ColumnIndex)
                {
                    case int colIndex when colIndex == dgvPacketList.Columns["cID"].Index:
                        e.Value = (e.RowIndex + 1).ToString();
                        e.FormattingApplied = true;
                        break;

                    case int colIndex when colIndex == dgvPacketList.Columns["cTypeImg"].Index:
                        var packetTypeCell = row.Cells["cPacketType"];
                        if (packetTypeCell.Value != null)
                        {
                            e.Value = Operate.PacketConfig.Packet.GetImg_ByPacketType((Operate.PacketConfig.Packet.PacketType)packetTypeCell.Value);
                            e.FormattingApplied = true;
                        }
                        break;

                    case int colIndex when colIndex == dgvPacketList.Columns["cPacketTime"].Index:
                        if (e.Value is DateTime time)
                        {
                            e.Value = time.ToString("HH:mm:ss:fffffff");
                            e.FormattingApplied = true;
                        }
                        break;

                    case int colIndex when colIndex == dgvPacketList.Columns["cPacketType"].Index:
                        if (e.Value != null)
                        {
                            e.Value = Operate.PacketConfig.Packet.GetName_ByPacketType((Operate.PacketConfig.Packet.PacketType)e.Value);
                            e.FormattingApplied = true;
                        }
                        break;

                    case int colIndex when colIndex == dgvPacketList.Columns["cFromImg"].Index:
                        var clientLocationCell = row.Cells["cFromLocation"];
                        if (clientLocationCell.Value != null)
                        {
                            e.Value = Operate.SystemConfig.GetFlagByLocation(clientLocationCell.Value.ToString());
                            e.FormattingApplied = true;
                        }
                        break;

                    case int colIndex when colIndex == dgvPacketList.Columns["cToImg"].Index:
                        var serverLocationCell = row.Cells["cToLocation"];
                        if (serverLocationCell.Value != null)
                        {
                            e.Value = Operate.SystemConfig.GetFlagByLocation(serverLocationCell.Value.ToString());
                            e.FormattingApplied = true;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(dgvPacketList_CellFormatting), ex.Message);
            }
        }

        public void SetColumnVisible_PacketList()
        {
            this.dgvPacketList.SuspendLayout();

            AntdUI.Spin.open(this, new AntdUI.Spin.Config()
            {
                Radius = 6,
                Font = new Font("Microsoft YaHei UI", 9F),
            }, (config) =>
            {
                config.Text = AntdUI.Localization.Get("Loading", "正在加载...");

                Operate.SystemConfig.InvokeAction?.Invoke(() =>
                {
                    this.dgvPacketList.Columns[1].Visible = Operate.PacketConfig.List.IsShow_ID;
                    this.dgvPacketList.Columns[2].Visible = Operate.PacketConfig.List.IsShow_ProxyTime;
                    this.dgvPacketList.Columns[3].Visible = Operate.PacketConfig.List.IsShow_PacketType;
                    this.dgvPacketList.Columns[4].Visible = Operate.PacketConfig.List.IsShow_PacketSocket;
                    this.dgvPacketList.Columns[5].Visible = Operate.PacketConfig.List.IsShow_ClientAddr;
                    this.dgvPacketList.Columns[6].Visible = Operate.PacketConfig.List.IsShow_ClientAddr;
                    this.dgvPacketList.Columns[7].Visible = Operate.PacketConfig.List.IsShow_ClientLocation;
                    this.dgvPacketList.Columns[8].Visible = Operate.PacketConfig.List.IsShow_ServerAddr;
                    this.dgvPacketList.Columns[9].Visible = Operate.PacketConfig.List.IsShow_ServerAddr;
                    this.dgvPacketList.Columns[10].Visible = Operate.PacketConfig.List.IsShow_ServerLocation;
                    this.dgvPacketList.Columns[11].Visible = Operate.PacketConfig.List.IsShow_PacketLen;
                    this.dgvPacketList.Columns[12].Visible = Operate.PacketConfig.List.IsShow_PacketData;
                });
            }, () =>
            {
                Operate.SystemConfig.InvokeAction?.Invoke(() =>
                {
                    this.dgvPacketList.ResumeLayout();
                });
            });
        }

        public void SetColumnName_PacketList()
        {
            this.dgvPacketList.SuspendLayout();

            AntdUI.Spin.open(this, new AntdUI.Spin.Config()
            {
                Radius = 6,
                Font = new Font("Microsoft YaHei UI", 9F),
            }, (config) =>
            {
                config.Text = AntdUI.Localization.Get("Loading", "正在加载...");

                Operate.SystemConfig.InvokeAction?.Invoke(() =>
                {
                    this.dgvPacketList.Columns[1].HeaderText = AntdUI.Localization.Get("Table.PacketList.Column.ID", "序号");
                    this.dgvPacketList.Columns[2].HeaderText = AntdUI.Localization.Get("Table.PacketList.Column.PacketTime", "时间戳");
                    this.dgvPacketList.Columns[3].HeaderText = AntdUI.Localization.Get("Table.PacketList.Column.PacketType", "类别");
                    this.dgvPacketList.Columns[4].HeaderText = AntdUI.Localization.Get("Table.PacketList.Column.PacketSocket", "套接字");
                    this.dgvPacketList.Columns[6].HeaderText = AntdUI.Localization.Get("Table.PacketList.Column.PacketFrom", "本机地址");
                    this.dgvPacketList.Columns[7].HeaderText = AntdUI.Localization.Get("Table.PacketList.Column.FromLocation", "所属地");
                    this.dgvPacketList.Columns[9].HeaderText = AntdUI.Localization.Get("Table.PacketList.Column.PacketTo", "远端地址");
                    this.dgvPacketList.Columns[10].HeaderText = AntdUI.Localization.Get("Table.PacketList.Column.ToLocation", "所属地");
                    this.dgvPacketList.Columns[11].HeaderText = AntdUI.Localization.Get("Table.PacketList.Column.PacketLen", "长度");
                    this.dgvPacketList.Columns[12].HeaderText = AntdUI.Localization.Get("Table.PacketList.Column.PacketData", "数据");
                });
            }, () =>
            {
                Operate.SystemConfig.InvokeAction?.Invoke(() =>
                {
                    this.dgvPacketList.ResumeLayout();
                });
            });
        }

        #endregion

        #region//封包列表 - 菜单

        private void bHookStart_Click(object sender, EventArgs e)
        {
            this.bHookStart.Enabled = false;
            this.bHookStop.Enabled = true;

            this.Start_Hook();
        }

        private void bHookStop_Click(object sender, EventArgs e)
        {
            this.bHookStart.Enabled = true;
            this.bHookStop.Enabled = false;

            this.Stop_Hook();
        }

        private void bPacketList_Clear_Click(object sender, EventArgs e)
        {
            this.CleanUp_PacketListInfo();
            this.CleanUp_PacketList();
            this.CleanUp_HexBox();

            if (this.form is InterfaceInfo.IInjectMode injectForm)
            {
                injectForm.CleanUp_LogList();
            }

            AntdUI.Message.open(new AntdUI.Message.Config(this.form, "已清空数据", TType.Warn)
            {
                LocalizationText = "ClearedData"
            });
        }

        private void bSearchPacket_Click(object sender, EventArgs e)
        {
            AntdUI.Drawer.open(new AntdUI.Drawer.Config(this.form, new SearchPacket(this.form))
            {
                Align = AntdUI.TAlignMini.Top,
                Mask = false,
                DisplayDelay = 0,
            });
        }

        private void ddMenu_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            this.ddMenu.SelectedValue = null;

            switch (e.Value.ToString())
            {
                case "LeachSettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this.form, new LeachSetting(this.form))
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "HookSettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this.form, new HookSetting(this.form))
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "ListSettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this.form, new ListSetting(this.form))
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "HotKeySettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this.form, new HotKeySetting(this.form))
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "BackUpSettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this.form, new BackUpSetting(this.form))
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "SystemSettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this.form, new SystemSetting(this.form))
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;
            }
        }

        private void dgvPacketList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < Operate.PacketConfig.List.lstPacketInfo.Count)
            {
                Operate.PacketConfig.Packet.OpenPacketEdit(this.form, Operate.PacketConfig.List.lstPacketInfo[e.RowIndex]);
            }
        }

        #endregion

        #region//封包列表 - 右键菜单

        private void dgvPacketList_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Operate.PacketConfig.List.lstPacketInfo.Count == 0)
                {
                    return;
                }

                AntdUI.ContextMenuStrip.open(this.dgvPacketList, item =>
                {
                    List<PacketInfo> piList = new List<PacketInfo>();

                    for (int i = 0; i < dgvPacketList.Rows.Count; i++)
                    {
                        if (dgvPacketList.Rows[i].Selected)
                        {
                            piList.Add(Operate.PacketConfig.List.lstPacketInfo[i]);
                        }
                    }

                    switch (item.ID)
                    {
                        case "Edit":

                            if (piList.Count > 0)
                            {
                                Operate.PacketConfig.Packet.OpenPacketEdit(this.form, piList[0]);
                            }

                            break;

                        case "Copy":

                            if (piList.Count > 0)
                            {
                                StringBuilder sb = new StringBuilder();
                                foreach (PacketInfo pi in piList)
                                {
                                    string hexString = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, pi.PacketBuffer);
                                    sb.AppendLine(hexString);
                                }

                                Clipboard.SetText(sb.ToString());

                                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "已复制到剪贴板", TType.Success)
                                {
                                    LocalizationText = "CopyToClipboard"
                                });
                            }

                            break;

                        case "ToFilterList":

                            if (piList.Count > 0)
                            {
                                bool bOK = Operate.FilterConfig.Filter.AddFilter_ByPacketInfo(piList[0], null);
                                if (bOK)
                                {
                                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "添加到滤镜列表成功", TType.Success)
                                    {
                                        LocalizationText = "ToFilterList.Success"
                                    });
                                }
                                else
                                {
                                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "添加到滤镜列表失败", TType.Error)
                                    {
                                        LocalizationText = "ToFilterList.Error"
                                    });
                                }
                            }

                            break;

                        case "SYSSocket":

                            if (piList.Count > 0)
                            {
                                Operate.SystemConfig.SystemSocket = piList[0].PacketSocket;

                                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "设置系统套接字完成", TType.Success)
                                {
                                    LocalizationText = "SSocket.Success"
                                });
                            }

                            break;

                        case "PacketModification":

                            if (piList.Count > 0)
                            {
                                var PacketModification = new PacketModification(this.form, piList[0]);
                                AntdUI.Modal.open(new AntdUI.Modal.Config(this.form, AntdUI.Localization.Get("PacketModificationForm", "封包数据对比"), PacketModification)
                                {
                                    Keyboard = false,
                                    MaskClosable = false,
                                    BtnHeight = 0,
                                });
                            }

                            break;

                        case "ToExcel":

                            Operate.PacketConfig.List.SavePacketList_Dialog(this.form, Operate.PacketConfig.Packet.InjectProcess, piList);

                            break;

                        case "ToTextA":

                            if (piList.Count > 0)
                            {
                                if (this.form is InterfaceInfo.IInjectMode injectForm)
                                {
                                    string TextA = string.Empty;
                                    foreach (PacketInfo pi in piList)
                                    {
                                        TextA += Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, pi.PacketBuffer) + "\r\n";
                                    }

                                    injectForm.SetTextA(TextA);

                                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "已添加到文本A", TType.Success)
                                    {
                                        LocalizationText = "ToTextA"
                                    });
                                }
                            }

                            break;

                        case "ToTextB":

                            if (piList.Count > 0)
                            {
                                if (this.form is InterfaceInfo.IInjectMode injectForm)
                                {
                                    string TextB = string.Empty;
                                    foreach (PacketInfo pi in piList)
                                    {
                                        TextB += Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, pi.PacketBuffer) + "\r\n";
                                    }

                                    injectForm.SetTextB(TextB);

                                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "已添加到文本B", TType.Success)
                                    {
                                        LocalizationText = "ToTextB"
                                    });
                                }
                            }

                            break;

                        case "SelectAll":

                            this.dgvPacketList.SelectAll();

                            break;

                        case "DeSelect":

                            this.dgvPacketList.ClearSelection();

                            break;

                        default:

                            if (piList.Count > 0)
                            {
                                if (Guid.TryParse(item.ID, out Guid SID))
                                {
                                    SendInfo si = Operate.SendConfig.Send.GetSend_ByGuid(SID);
                                    if (si != null && piList.Count > 0)
                                    {
                                        if (Operate.SendConfig.Send.AddSendCollection_ByPacketInfo(SID, piList))
                                        {
                                            AntdUI.Message.open(new AntdUI.Message.Config(this.form, "已添加到 " + item.Text, TType.Success)
                                            {
                                                LocalizationText = "ToSendList.Success"
                                            });
                                        }
                                        else
                                        {
                                            AntdUI.Message.open(new AntdUI.Message.Config(this.form, "添加到发送列表出错", TType.Error)
                                            {
                                                LocalizationText = "ToSendList.Error"
                                            });
                                        }
                                    }
                                }
                            }

                            break;
                    }
                }, Operate.PacketConfig.List.GetCMS_PacketList());
            }
        }

        #endregion

        #region//封包数据 - 右键菜单

        private void hbPacketData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.C && this.hbPacketData.CanCopy())
            {
                e.Handled = true;
                this.hbPacketData.CopyHex();

                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "已复制到剪贴板", TType.Success)
                {
                    LocalizationText = "CopyToClipboard"
                });
            }
        }

        private void hbPacketData_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DynamicByteProvider dbp = hbPacketData.ByteProvider as DynamicByteProvider;
                if (dbp == null || dbp.Bytes.Count == 0)
                {
                    return;
                }

                AntdUI.ContextMenuStrip.open(new AntdUI.ContextMenuStrip.Config(hbPacketData, (item) =>
                {
                    switch (item.ID)
                    {
                        case "Edit":

                            if (Operate.PacketConfig.List.piSelect != null)
                            {
                                var PacketEdit = new PacketEdit(this.form, Operate.PacketConfig.List.piSelect);
                                AntdUI.Modal.open(new AntdUI.Modal.Config(this.form, AntdUI.Localization.Get("PacketEditForm", "封包编辑"), PacketEdit)
                                {
                                    Keyboard = false,
                                    MaskClosable = false,
                                    BtnHeight = 0,
                                });
                            }

                            break;

                        case "ToFilterList":

                            if (Operate.PacketConfig.List.piSelect != null)
                            {
                                bool bOK = false;
                                if (this.hbPacketData.CanCopy())
                                {
                                    this.hbPacketData.CopyHex();
                                    byte[] bBufferCopy = Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, Clipboard.GetText());
                                    bOK = Operate.FilterConfig.Filter.AddFilter_ByPacketInfo(Operate.PacketConfig.List.piSelect, bBufferCopy);
                                }
                                else
                                {
                                    bOK = Operate.FilterConfig.Filter.AddFilter_ByPacketInfo(Operate.PacketConfig.List.piSelect, dbp.Bytes.ToArray());
                                }

                                if (bOK)
                                {
                                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "已添加到滤镜列表", TType.Success)
                                    {
                                        LocalizationText = "ToFilterList.Success"
                                    });
                                }
                                else
                                {
                                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "添加到滤镜列表出错", TType.Error)
                                    {
                                        LocalizationText = "ToFilterList.Error"
                                    });
                                }
                            }

                            break;

                        case "Copy_Text":

                            this.hbPacketData.Copy();

                            AntdUI.Message.open(new AntdUI.Message.Config(this.form, "已复制到剪贴板", TType.Success)
                            {
                                LocalizationText = "CopyToClipboard"
                            });

                            break;

                        case "Copy_Hex":

                            this.hbPacketData.CopyHex();

                            AntdUI.Message.open(new AntdUI.Message.Config(this.form, "已复制到剪贴板", TType.Success)
                            {
                                LocalizationText = "CopyToClipboard"
                            });

                            break;

                        case "ToTextA":

                            string StringA = string.Empty;
                            if (this.hbPacketData.CanCopy())
                            {
                                this.hbPacketData.CopyHex();
                                StringA = Clipboard.GetText();
                            }
                            else
                            {
                                StringA = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, dbp.Bytes.ToArray());
                            }

                            if (this.form is InterfaceInfo.IInjectMode injectFormA)
                            {
                                injectFormA.SetTextA(StringA);

                                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "已添加到文本A", TType.Success)
                                {
                                    LocalizationText = "ToTextA"
                                });
                            }

                            break;

                        case "ToTextB":

                            string StringB = string.Empty;
                            if (this.hbPacketData.CanCopy())
                            {
                                this.hbPacketData.CopyHex();
                                StringB = Clipboard.GetText();
                            }
                            else
                            {
                                StringB = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, dbp.Bytes.ToArray());
                            }

                            if (this.form is InterfaceInfo.IInjectMode injectFormB)
                            {
                                injectFormB.SetTextB(StringB);

                                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "已添加到文本B", TType.Success)
                                {
                                    LocalizationText = "ToTextB"
                                });
                            }

                            break;

                        case "SelectAll":

                            this.hbPacketData.SelectAll();

                            break;

                        default:

                            if (Operate.PacketConfig.List.piSelect == null)
                            {
                                return;
                            }

                            if (Guid.TryParse(item.ID, out Guid SID))
                            {
                                SendInfo si = Operate.SendConfig.Send.GetSend_ByGuid(SID);
                                if (si != null)
                                {
                                    byte[] bBuffer = null;
                                    if (this.hbPacketData.CanCopy())
                                    {
                                        this.hbPacketData.CopyHex();
                                        bBuffer = Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, Clipboard.GetText());
                                    }
                                    else
                                    {
                                        bBuffer = dbp.Bytes.ToArray();
                                    }

                                    List<PacketInfo> piList = new List<PacketInfo>
                                    {
                                        new PacketInfo
                                        {
                                            PacketSocket = Operate.PacketConfig.List.piSelect.PacketSocket,
                                            PacketType = Operate.PacketConfig.List.piSelect.PacketType,
                                            PacketFrom = Operate.PacketConfig.List.piSelect.PacketFrom,
                                            PacketTo = Operate.PacketConfig.List.piSelect.PacketTo,
                                            PacketBuffer = bBuffer,
                                            PacketLen = bBuffer.Length,
                                            PacketData = Operate.PacketConfig.Packet.GetPacketData_Hex(bBuffer, Operate.PacketConfig.Packet.PacketData_MaxLen),
                                        }
                                    };

                                    if (Operate.SendConfig.Send.AddSendCollection_ByPacketInfo(SID, piList))
                                    {
                                        AntdUI.Message.open(new AntdUI.Message.Config(this.form, "已添加到 " + item.Text, TType.Success)
                                        {
                                            LocalizationText = "ToSendList.Success"
                                        });
                                    }
                                    else
                                    {
                                        AntdUI.Message.open(new AntdUI.Message.Config(this.form, "添加到发送列表出错", TType.Error)
                                        {
                                            LocalizationText = "ToSendList.Error"
                                        });
                                    }
                                }
                            }

                            break;
                    }
                }, Operate.PacketConfig.Packet.GetCMS_PacketData(this.hbPacketData)));
            }
        }

        #endregion                       

        #region//开始拦截

        private void Start_Hook()
        {
            try
            {
                ws.StartHook();

                if (bWakeUp)
                {
                    RemoteHooking.WakeUpProcess();
                    this.bWakeUp = false;
                }

                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "开始拦截", TType.Success)
                {
                    LocalizationText = "InjectModeForm.StartHook"
                });
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(Start_Hook), ex.Message);
            }
        }

        #endregion

        #region//停止拦截        

        private void Stop_Hook()
        {
            try
            {
                ws.StopHook();

                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "停止拦截", TType.Error)
                {
                    LocalizationText = "InjectModeForm.StopHook"
                });
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(Stop_Hook), ex.Message);
            }
        }

        #endregion

        #region//清理数据

        private void CleanUp_PacketListInfo()
        {
            try
            {
                Operate.PacketConfig.Packet.TotalPackets = 0;
                Operate.PacketConfig.Packet.Total_SendBytes = 0;
                Operate.PacketConfig.Packet.Total_RecvBytes = 0;
                Operate.FilterConfig.Filter.FilterExecute_CNT = 0;
                Operate.FilterConfig.Filter.FilterReplace_CNT = 0;
                Operate.FilterConfig.Filter.FilterChange_CNT = 0;
                Operate.FilterConfig.Filter.FilterIntercept_CNT = 0;
                Operate.FilterConfig.Filter.FilterDisplay_CNT = 0;
                Operate.FilterConfig.Filter.FilterNoDisplay_CNT = 0;
                Operate.PacketConfig.Packet.FilterPacket_CNT = 0;
                Operate.PacketConfig.Packet.Send_CNT = 0;
                Operate.PacketConfig.Packet.Recv_CNT = 0;
                Operate.PacketConfig.Packet.SendTo_CNT = 0;
                Operate.PacketConfig.Packet.RecvFrom_CNT = 0;
                Operate.PacketConfig.Packet.WSASend_CNT = 0;
                Operate.PacketConfig.Packet.WSARecv_CNT = 0;
                Operate.PacketConfig.Packet.WSASendTo_CNT = 0;
                Operate.PacketConfig.Packet.WSARecvFrom_CNT = 0;
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(CleanUp_PacketListInfo), ex.Message);
            }
        }

        private void CleanUp_PacketList()
        {
            try
            {
                this.dgvPacketList.SuspendLayout();
                Operate.PacketConfig.Queue.ClearPacketQueue();
                Operate.PacketConfig.List.lstPacketInfo.Clear();
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(CleanUp_PacketList), ex.Message);
            }
            finally
            {
                this.dgvPacketList.ResumeLayout();
            }
        }

        private void CleanUp_HexBox()
        {
            if (hbPacketData.ByteProvider != null)
            {
                IDisposable byteProvider = hbPacketData.ByteProvider as IDisposable;

                if (byteProvider != null)
                {
                    byteProvider.Dispose();
                }

                hbPacketData.ByteProvider = null;
            }
        }

        #endregion

        #region//自动清理

        private void cbPacketList_AutoClear_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.PacketList_AutoClear_Changed();

            Operate.PacketConfig.List.AutoClear = this.cbPacketList_AutoClear.Checked;            
        }

        private void txtPacketList_AutoClear_ValueChanged(object sender, DecimalEventArgs e)
        {
            Operate.PacketConfig.List.AutoClear_Value = this.txtPacketList_AutoClear.Value;
        }

        private void PacketList_AutoClear_Changed()
        {
            this.txtPacketList_AutoClear.Enabled = this.cbPacketList_AutoClear.Checked;            
        }

        #endregion

        #region//自动滚动

        private void cbPacketList_AutoRoll_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.PacketList_AutoRoll_Changed();
        }

        private void PacketList_AutoRoll_Changed()
        {
            Operate.PacketConfig.List.AutoRoll = this.cbPacketList_AutoRoll.Checked;
        }

        #endregion

        #region//显示选中的封包数据

        private void dgvPacketList_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvPacketList.SelectedRows.Count > 0)
                {
                    int selectedIndex = this.dgvPacketList.SelectedRows[0].Index;
                    if (selectedIndex >= 0 && selectedIndex < Operate.PacketConfig.List.lstPacketInfo.Count)
                    {
                        Operate.PacketConfig.List.Search_Index = selectedIndex;
                        Operate.PacketConfig.List.piSelect = Operate.PacketConfig.List.lstPacketInfo[selectedIndex];

                        DynamicByteProvider dbp = new DynamicByteProvider(Operate.PacketConfig.List.piSelect.PacketBuffer);
                        hbPacketData.ByteProvider = dbp;
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(dgvPacketList_SelectionChanged), ex.Message);
            }
        }

        #endregion

        #region//显示封包列表

        public void RefreshPacketList()
        {
            try
            {
                if (Operate.PacketConfig.List.AutoRoll && this.dgvPacketList.Rows.Count > 0 && dgvPacketList.Height > dgvPacketList.RowTemplate.Height)
                {
                    if (dgvPacketList.InvokeRequired)
                    {
                        dgvPacketList.Invoke(new Action(() =>
                        {
                            dgvPacketList.FirstDisplayedScrollingRowIndex = dgvPacketList.RowCount - 1;
                        }));
                    }
                    else
                    {
                        dgvPacketList.FirstDisplayedScrollingRowIndex = dgvPacketList.RowCount - 1;
                    }
                }

                if (Operate.PacketConfig.List.AutoClear)
                {
                    if (Operate.PacketConfig.List.lstPacketInfo.Count > Operate.PacketConfig.List.AutoClear_Value)
                    {
                        this.CleanUp_PacketList();
                        this.CleanUp_HexBox();
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(RefreshPacketList), ex.Message);
            }
        }

        #endregion

        #region//显示注入信息

        public void ShowInjectInfo()
        {
            this.lTotal_CNT.Text = Operate.PacketConfig.Packet.TotalPackets.ToString();
            this.lFilterExecute_CNT.Text = Operate.FilterConfig.Filter.FilterExecute_CNT.ToString();
            this.lQueue_CNT.Text = Operate.PacketConfig.Queue.cqPacketInfo.Count.ToString();
            this.lFilterPacket_CNT.Text = Operate.PacketConfig.Packet.FilterPacket_CNT.ToString();
            this.lSend_CNT.Text = Operate.PacketConfig.Packet.Send_CNT.ToString();
            this.lRecv_CNT.Text = Operate.PacketConfig.Packet.Recv_CNT.ToString();
            this.lSendTo_CNT.Text = Operate.PacketConfig.Packet.SendTo_CNT.ToString();
            this.lRecvFrom_CNT.Text = Operate.PacketConfig.Packet.RecvFrom_CNT.ToString();
            this.lWSASend_CNT.Text = Operate.PacketConfig.Packet.WSASend_CNT.ToString();
            this.lWSARecv_CNT.Text = Operate.PacketConfig.Packet.WSARecv_CNT.ToString();
            this.lWSASendTo_CNT.Text = Operate.PacketConfig.Packet.WSASendTo_CNT.ToString();
            this.lWSARecvFrom_CNT.Text = Operate.PacketConfig.Packet.WSARecvFrom_CNT.ToString();
            this.lSpeedInfo.Text = Operate.PacketConfig.Packet.GetPacketSpeedInfo();
        }

        #endregion

        #region//查找封包（异步）

        public void SearchPacketList(bool FromHead)
        {
            if (!this.bgwSearchPacketList.IsBusy)
            {
                this.SearchFromHead = FromHead;
                this.bgwSearchPacketList.RunWorkerAsync();
            }
        }

        private void HexBox_FindNext()
        {
            try
            {
                if (Operate.PacketConfig.List.FindOptions.IsValid)
                {
                    if (Operate.PacketConfig.List.FindOptions.Type == FindType.Hex && Operate.PacketConfig.List.FindOptions.Hex.Length == 0)
                    {
                        return;
                    }

                    long res = this.hbPacketData.Find(Operate.PacketConfig.List.FindOptions);

                    if (res == -1)
                    {
                        Operate.PacketConfig.List.Search_Index += 1;
                        this.SearchPacketList(this.SearchFromHead);
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(HexBox_FindNext), ex.Message);
            }
        }

        private void bgwSearchPacketList_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            try
            {
                if (Operate.PacketConfig.List.lstPacketInfo.Count > 0 && Operate.PacketConfig.List.FindOptions.IsValid)
                {
                    if (this.SearchFromHead)
                    {
                        Operate.PacketConfig.List.Search_Index = 0;
                    }                    

                    e.Result = Operate.PacketConfig.List.SearchForList<PacketInfo>(Operate.PacketConfig.List.Search_Index, true);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(bgwSearchPacketList_DoWork), ex.Message);
            }
        }

        private void bgwSearchPacketList_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null && !e.Cancelled && e.Result != null)
                {
                    if (int.TryParse(e.Result.ToString(), out int iSearchResultIndex))
                    {
                        if (iSearchResultIndex >= 0 && iSearchResultIndex < dgvPacketList.Rows.Count)
                        {
                            dgvPacketList.SuspendLayout();
                            dgvPacketList.FirstDisplayedScrollingRowIndex = iSearchResultIndex;
                            dgvPacketList.Rows[iSearchResultIndex].Selected = true;
                            dgvPacketList.CurrentCell = dgvPacketList.Rows[iSearchResultIndex].Cells[0];
                            dgvPacketList.ResumeLayout();

                            this.HexBox_FindNext();
                        }
                        else
                        {
                            string NoMatch = AntdUI.Localization.Get("SearchPacketForm.NoMatch", "没有匹配的封包");
                            AntdUI.Modal.open(new AntdUI.Modal.Config(this.form, AntdUI.Localization.Get("SearchPacketForm", "查找封包"), "\r\n" + NoMatch + "\r\n\r\n")
                            {
                                Icon = TType.Info,
                                Keyboard = false,
                                MaskClosable = false,
                                CancelText = null,
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(bgwSearchPacketList_RunWorkerCompleted), ex.Message);
            }
        }

        #endregion        
    }
}
