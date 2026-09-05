using AntdUI;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class ProxyList : UserControl
    {
        private Form form;
        public bool SearchFromHead = true;
        private QuickList cQuickList = null;
        private PacketData controlPacketData = null;

        #region//窗体事件

        public ProxyList(Form form)
        {
            InitializeComponent();
            this.form = form;            
        }

        private void ProxyList_Load(object sender, EventArgs e)
        {
            try
            {
                this.InitMenu();
                this.InitTable_ProxyList();
                this.InitControl();
                this.Dark_Changed();
                this.SetColumnName_ProxyList();

                this.cbPacketList_AutoRoll.Checked = Operate.PacketConfig.List.AutoRoll;
                this.cbPacketList_AutoClear.Checked = Operate.PacketConfig.List.AutoClear;
                this.txtPacketList_AutoClear.Value = Operate.PacketConfig.List.AutoClear_Value;
                this.PacketList_AutoClear_Changed();

                //B9c：代理列表被自动清理时收拾右侧面板（清理动作本身在 Operate 里做）
                WinFormsUiFeed.Cleared += this.OnFeedCleared;
                this.Disposed += (s, ev) => WinFormsUiFeed.Cleared -= this.OnFeedCleared;

                Operate.DoLog(nameof(ProxyList_Load), Operate.ProcessConfig.GetInjectProcessName());
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(ProxyList_Load), ex);
            }            
        }

        private void OnFeedCleared(FeedList List)
        {
            if (List != FeedList.Proxy)
            {
                return;
            }

            try
            {
                this.controlPacketData?.CleanUp_PacketData();
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(OnFeedCleared), ex);
            }
        }

        private void InitMenu()
        {
            this.ddMenu.Items.AddRange(new AntdUI.SelectItem[]
            {
                new AntdUI.SelectItem("代理设置")
                {
                    Tag = "ProxySettings",
                    LocalizationText = "ProxyModeForm.ProxySettings",
                    IconSvg = "ShareAltOutlined",
                },
                new AntdUI.SelectItem("进程设置")
                {
                    Tag = "ProcessSettings",
                    LocalizationText = "ProxyModeForm.ProcessSettings",
                    IconSvg = "ProfileOutlined",
                },
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
                new AntdUI.SelectItem("映射设置")
                {
                    Tag = "MapSettings",
                    LocalizationText = "ProxyModeForm.MapSettings",
                    IconSvg = "BlockOutlined",
                },
                new AntdUI.SelectItem("外部代理设置")
                {
                    Tag = "ExternalProxySettings",
                    LocalizationText = "ProxyModeForm.ExternalProxySettings",
                    IconSvg = "CloudUploadOutlined",
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
                new AntdUI.SelectItem("远程管理设置")
                {
                    Tag = "RemoteMGTSettings",
                    LocalizationText = "ProxyModeForm.RemoteMGTSettings",
                    IconSvg = "CloudServerOutlined",
                },
                new AntdUI.SelectItem("防火墙设置")
                {
                    Tag = "FireWallSettings",
                    LocalizationText = "ProxyModeForm.FireWallSettings",
                    IconSvg = "FireOutlined",
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
            //PacketData
            if (this.splitterQuickList.InvokeRequired)
            {
                this.splitterQuickList.Invoke(new Action(() =>
                {
                    controlPacketData = new PacketData(this.form);
                    controlPacketData.Dock = DockStyle.Fill;
                    this.splitterQuickList.Panel2.Controls.Add(controlPacketData);
                }));
            }
            else
            {
                controlPacketData = new PacketData(this.form);
                controlPacketData.Dock = DockStyle.Fill;
                this.splitterQuickList.Panel2.Controls.Add(controlPacketData);
            }

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
                this.dgvProxyList.BackgroundColor = 
                    this.dgvProxyList.RowsDefaultCellStyle.BackColor = 
                    this.dgvProxyList.ColumnHeadersDefaultCellStyle.BackColor =
                    this.dgvProxyList.ColumnHeadersDefaultCellStyle.SelectionBackColor = UiTheme.Color_40;                

                this.dgvProxyList.ForeColor = Color.LimeGreen;
                this.dgvProxyList.ColumnHeadersDefaultCellStyle.ForeColor =
                    this.dgvProxyList.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Silver;                
            }
            else
            {
                this.dgvProxyList.BackgroundColor = 
                    this.dgvProxyList.RowsDefaultCellStyle.BackColor = 
                    this.dgvProxyList.ColumnHeadersDefaultCellStyle.BackColor =
                    this.dgvProxyList.ColumnHeadersDefaultCellStyle.SelectionBackColor = Color.White;
              
                this.dgvProxyList.ForeColor = Color.Green;
                this.dgvProxyList.ColumnHeadersDefaultCellStyle.ForeColor =
                    this.dgvProxyList.ColumnHeadersDefaultCellStyle.SelectionForeColor = Color.Black;                
            }

            this.controlPacketData?.Dark_Changed();
            this.cQuickList?.Dark_Changed();
        }

        public void RefreshProxyData()
        { 
            this.controlPacketData?.RefreshPacketData();
        }

        #endregion

        #region//初始化表格

        private void InitTable_ProxyList()
        {            
            this.dgvProxyList.AutoGenerateColumns = false;
            this.dgvProxyList.DataSource = Operate.ProxyConfig.List.lstProxyInfo;
            this.dgvProxyList.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvProxyList, true, null);
            this.dgvProxyList.ColumnHeadersDefaultCellStyle.Font = new Font("微软雅黑", 9, FontStyle.Bold);
        }

        private void dgvProxyList_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            try
            {
                var row = dgvProxyList.Rows[e.RowIndex];
                if (e.RowIndex < Operate.ProxyConfig.List.lstProxyInfo.Count)
                {
                    var filterAction = Operate.ProxyConfig.List.lstProxyInfo[e.RowIndex].FilterAction;
                    var colors = UiTheme.GetFilterColors(filterAction);
                    if (colors.HasValue)
                    {
                        row.DefaultCellStyle.ForeColor = colors.Value.ForeColor;
                        row.DefaultCellStyle.BackColor = colors.Value.BackColor;
                    }
                }

                switch (e.ColumnIndex)
                {
                    case int colIndex when colIndex == dgvProxyList.Columns["cID"].Index:
                        e.Value = (e.RowIndex + 1).ToString();
                        e.FormattingApplied = true;
                        break;

                    case int colIndex when colIndex == dgvProxyList.Columns["cTypeImg"].Index:
                        var packetTypeCell = row.Cells["cPacketType"];
                        if (packetTypeCell.Value != null)
                        {
                            e.Value = UiImages.GetImg_ByPacketType((Operate.PacketConfig.Packet.PacketType)packetTypeCell.Value);
                            e.FormattingApplied = true;
                        }
                        break;

                    case int colIndex when colIndex == dgvProxyList.Columns["cProxyTime"].Index:
                        if (e.Value is DateTime time)
                        {
                            e.Value = time.ToString("HH:mm:ss:fffffff");
                            e.FormattingApplied = true;
                        }
                        break;

                    case int colIndex when colIndex == dgvProxyList.Columns["cPacketType"].Index:
                        if (e.Value != null)
                        {
                            e.Value = Operate.PacketConfig.Packet.GetName_ByPacketType((Operate.PacketConfig.Packet.PacketType)e.Value);
                            e.FormattingApplied = true;
                        }
                        break;

                    case int colIndex when colIndex == dgvProxyList.Columns["cClientImg"].Index:
                        var clientLocationCell = row.Cells["cClientLocation"];
                        if (clientLocationCell.Value != null)
                        {
                            e.Value = UiImages.GetFlagByLocation(clientLocationCell.Value.ToString());
                            e.FormattingApplied = true;
                        }
                        break;

                    case int colIndex when colIndex == dgvProxyList.Columns["cServerImg"].Index:
                        var serverLocationCell = row.Cells["cServerLocation"];
                        if (serverLocationCell.Value != null)
                        {
                            e.Value = UiImages.GetFlagByLocation(serverLocationCell.Value.ToString());
                            e.FormattingApplied = true;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(dgvProxyList_CellFormatting), ex);
            }
        }

        public void SetColumnVisible_ProxyList()
        {
            this.dgvProxyList.SuspendLayout();

            AntdUI.Spin.open(this, new AntdUI.Spin.Config()
            {
                Radius = 6,
                Font = new Font("Microsoft YaHei UI", 9F),
            }, (config) =>
            {
                config.Text = AntdUI.Localization.Get("Loading", "正在加载...");

                Operate.SystemConfig.InvokeAction?.Invoke(() =>
                {
                    this.dgvProxyList.Columns[1].Visible = Operate.ProxyConfig.List.IsShow_ID;
                    this.dgvProxyList.Columns[2].Visible = Operate.ProxyConfig.List.IsShow_ProxyTime;
                    this.dgvProxyList.Columns[3].Visible = Operate.ProxyConfig.List.IsShow_PacketType;
                    this.dgvProxyList.Columns[4].Visible = Operate.ProxyConfig.List.IsShow_PacketSocket;
                    this.dgvProxyList.Columns[5].Visible = Operate.ProxyConfig.List.IsShow_ClientAddr;
                    this.dgvProxyList.Columns[6].Visible = Operate.ProxyConfig.List.IsShow_ClientAddr;
                    this.dgvProxyList.Columns[7].Visible = Operate.ProxyConfig.List.IsShow_ClientLocation;
                    this.dgvProxyList.Columns[8].Visible = Operate.ProxyConfig.List.IsShow_ServerAddr;
                    this.dgvProxyList.Columns[9].Visible = Operate.ProxyConfig.List.IsShow_ServerAddr;
                    this.dgvProxyList.Columns[10].Visible = Operate.ProxyConfig.List.IsShow_ServerLocation;
                    this.dgvProxyList.Columns[11].Visible = Operate.ProxyConfig.List.IsShow_PacketLen;
                    this.dgvProxyList.Columns[12].Visible = Operate.ProxyConfig.List.IsShow_PacketData;
                });
            }, () =>
            {
                Operate.SystemConfig.InvokeAction?.Invoke(() =>
                {
                    this.dgvProxyList.ResumeLayout();
                });
            });
        }

        public void SetColumnName_ProxyList()
        {
            this.dgvProxyList.SuspendLayout();

            AntdUI.Spin.open(this, new AntdUI.Spin.Config()
            {
                Radius = 6,
                Font = new Font("Microsoft YaHei UI", 9F),
            }, (config) =>
            {
                config.Text = AntdUI.Localization.Get("Loading", "正在加载...");

                Operate.SystemConfig.InvokeAction?.Invoke(() =>
                {
                    this.dgvProxyList.Columns[1].HeaderText = AntdUI.Localization.Get("Table.ProxyList.Column.ID", "序号");
                    this.dgvProxyList.Columns[2].HeaderText = AntdUI.Localization.Get("Table.ProxyList.Column.ProxyTime", "时间戳");
                    this.dgvProxyList.Columns[3].HeaderText = AntdUI.Localization.Get("Table.ProxyList.Column.PacketType", "类别");
                    this.dgvProxyList.Columns[4].HeaderText = AntdUI.Localization.Get("Table.ProxyList.Column.PacketSocket", "套接字");
                    this.dgvProxyList.Columns[6].HeaderText = AntdUI.Localization.Get("Table.ProxyList.Column.ClientAddr", "客户端地址");
                    this.dgvProxyList.Columns[7].HeaderText = AntdUI.Localization.Get("Table.ProxyList.Column.ClientLocation", "所属地");
                    this.dgvProxyList.Columns[9].HeaderText = AntdUI.Localization.Get("Table.ProxyList.Column.ServerDomain", "服务端地址");
                    this.dgvProxyList.Columns[10].HeaderText = AntdUI.Localization.Get("Table.ProxyList.Column.ServerLocation", "所属地");
                    this.dgvProxyList.Columns[11].HeaderText = AntdUI.Localization.Get("Table.ProxyList.Column.PacketLen", "长度");
                    this.dgvProxyList.Columns[12].HeaderText = AntdUI.Localization.Get("Table.ProxyList.Column.PacketData", "数据");
                });
            }, () =>
            {
                Operate.SystemConfig.InvokeAction?.Invoke(() =>
                {
                    this.dgvProxyList.ResumeLayout();
                });
            });            
        }

        private void dgvProxyList_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
            e.Cancel = true;

            Operate.DoLog(nameof(dgvProxyList_DataError), e.Exception.Message);
        }

        #endregion

        #region//代理列表 - 菜单

        private void bProxyStart_Click(object sender, EventArgs e)
        {
            bool bStart = false;
            AntdUI.Spin.open(this, new AntdUI.Spin.Config()
            {
                Radius = 6,
                Font = new Font("Microsoft YaHei UI", 9F),
            }, (config) =>
            {
                config.Text = AntdUI.Localization.Get("Loading", "正在加载...");
                //启停逻辑已搬进 Operate.ProxyConfig.Proxy（两套 UI 共用），这里只管按钮状态
                bStart = Operate.ProxyConfig.Proxy.StartProxy();
            }, () =>
            {
                if (bStart)
                {
                    this.bProxyStart.Enabled = false;
                    this.bProxyStop.Enabled = true;
                }
            });            
        }

        private void bProxyStop_Click(object sender, EventArgs e)
        {
            Operate.ProxyConfig.Proxy.StopProxy();

            this.bProxyStart.Enabled = true;
            this.bProxyStop.Enabled = false;
        }

        private void bProxyList_Clear_Click(object sender, EventArgs e)
        {
            this.CleanUp_ProxyList();
            this.CleanUp_ProxyListInfo();
            this.controlPacketData?.CleanUp_PacketData();

            if (this.form is InterfaceInfo.IProxyMode proxyForm)
            {
                proxyForm.CleanUp_LogList();
            }

            AntdUI.Message.open(new AntdUI.Message.Config(this.form, "已清空数据", TType.Warn)
            {
                LocalizationText = "InjectModeForm.Clear"
            });
        }

        private void bSearchPacket_Click(object sender, EventArgs e)
        {
            AntdUI.Drawer.open(new AntdUI.Drawer.Config(this.form, new SearchPacket(this.form))
            {                
                Align = AntdUI.TAlignMini.Top,
                Mask = false,
                DisplayDelay = 0,
                Padding = 5,
            });
        }

        private void ddMenu_SelectedValueChanged(object sender, ObjectNEventArgs e)
        {
            this.ddMenu.SelectedValue = null;

            switch (e.Value.ToString())
            {
                case "ProxySettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this.form, new ProxySetting(this.form))
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "ProcessSettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this.form, new ProcessSetting(this.form))
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

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

                case "MapSettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this.form, new MapSetting(this.form))
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "ExternalProxySettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this.form, new EXTProxySetting(this.form))
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

                case "RemoteMGTSettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this.form, new RemoteMGTSetting(this.form))
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "FireWallSettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this.form, new FireWallSetting(this.form))
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

        private void dgvProxyList_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //只响应鼠标左键：CellDoubleClick 的参数不带按键信息，故改用 CellMouseDoubleClick
            if (e.Button != MouseButtons.Left) return;

            if (e.RowIndex >= 0 && e.RowIndex < Operate.ProxyConfig.List.lstProxyInfo.Count)
            {
                UiDialogs.OpenPacketEdit(this.form, Operate.ProxyConfig.List.lstProxyInfo[e.RowIndex]);
            }
        }

        #endregion

        #region//代理列表 - 右键菜单

        private async void dgvProxyList_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                    if (e.Button == MouseButtons.Right)
                    {
                        if (Operate.ProxyConfig.List.lstProxyInfo.Count == 0)
                        {
                            return;
                        }

                        AntdUI.ContextMenuStrip.open(this.dgvProxyList, async item =>
                        {
                            List<ProxyInfo> piList = new List<ProxyInfo>();

                            for (int i = 0; i < dgvProxyList.Rows.Count; i++)
                            {
                                if (dgvProxyList.Rows[i].Selected)
                                {
                                    piList.Add(Operate.ProxyConfig.List.lstProxyInfo[i]);
                                }
                            }

                            string ListType = string.Empty;

                            switch (item.ID)
                            {
                                case "Edit":

                                    if (piList.Count > 0)
                                    {
                                        UiDialogs.OpenPacketEdit(this.form, piList[0]);
                                    }

                                    break;

                                case "Copy":

                                    if (piList.Count > 0)
                                    {
                                        StringBuilder sb = new StringBuilder();
                                        foreach (ProxyInfo pi in piList)
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
                                        bool bOK = Operate.FilterConfig.Filter.AddFilter_ByProxyInfo(piList[0], null);
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

                                    await Operate.ProxyConfig.List.SaveProxyList_Dialog(Operate.PacketConfig.Packet.InjectProcess, piList);

                                    break;

                                case "ToTextA":

                                    if (piList.Count > 0)
                                    {
                                        if (this.form is InterfaceInfo.IProxyMode proxyForm)
                                        {
                                            string TextA = string.Empty;
                                            foreach (ProxyInfo pi in piList)
                                            {
                                                TextA += Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, pi.PacketBuffer) + "\r\n";
                                            }

                                            proxyForm.SetTextA(TextA);

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
                                        if (this.form is InterfaceInfo.IProxyMode proxyForm)
                                        {
                                            string TextB = string.Empty;
                                            foreach (ProxyInfo pi in piList)
                                            {
                                                TextB += Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, pi.PacketBuffer) + "\r\n";
                                            }

                                            proxyForm.SetTextB(TextB);

                                            AntdUI.Message.open(new AntdUI.Message.Config(this.form, "已添加到文本B", TType.Success)
                                            {
                                                LocalizationText = "ToTextB"
                                            });
                                        }
                                    }

                                    break;

                                case "SelectAll":

                                    this.dgvProxyList.SelectAll();

                                    break;

                                case "DeSelect":

                                    this.dgvProxyList.ClearSelection();

                                    break;

                                default:

                                    if (piList.Count > 0)
                                    {
                                        if (item.Tag.ToString().Equals("ToSend"))
                                        {
                                            if (Guid.TryParse(item.ID, out Guid SID))
                                            {
                                                SendInfo si = Operate.SendConfig.Send.GetSend_ByGuid(SID);
                                                if (si != null && piList.Count > 0)
                                                {
                                                    if (Operate.SendConfig.Send.AddSendCollection_ByProxyInfo(SID, piList))
                                                    {
                                                        string sText = string.Format(AntdUI.Localization.Get("ToSendList.Success", "已添加到 : {0}"), item.Text);
                                                        AntdUI.Message.open(new AntdUI.Message.Config(this.form, sText, TType.Success));
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

                                            return;
                                        }

                                        if (item.Tag.ToString().Equals("ToWareHouse"))
                                        {
                                            if (Guid.TryParse(item.ID, out Guid WID))
                                            {
                                                WareHouseInfo whi = Operate.WareHouseConfig.WareHouse.GetWareHouse_ByGuid(WID);
                                                if (whi != null && piList.Count > 0)
                                                {
                                                    if (Operate.WareHouseConfig.WareHouse.AddStores_ByProxyInfo(WID, piList))
                                                    {
                                                        string sText = string.Format(AntdUI.Localization.Get("ToWareHouse.Success", "已添加到 : {0}"), item.Text);
                                                        AntdUI.Message.open(new AntdUI.Message.Config(this.form, sText, TType.Success));
                                                    }
                                                    else
                                                    {
                                                        AntdUI.Message.open(new AntdUI.Message.Config(this.form, "添加到仓库出错", TType.Error)
                                                        {
                                                            LocalizationText = "ToWareHouse.Error"
                                                        });
                                                    }
                                                }
                                            }

                                            return;
                                        }
                                    }

                                    break;
                            }
                        }, Operate.PacketConfig.List.GetCMS_PacketList().ToAntd());
                    }
            }
            catch (Exception ex)
            {
                //async void：await 之后抛出的异常不会被 WinForms 兜住，必须自己捕获
                Operate.DoLog(nameof(dgvProxyList_MouseClick), ex);
            }
        }

        #endregion        

        #region//清空数据

        private void CleanUp_ProxyListInfo()
        {
            Operate.ProxyConfig.Proxy.ProxyTotal_CNT = 0;
            Operate.ProxyConfig.Proxy.TCP_Req_CNT = 0;
            Operate.ProxyConfig.Proxy.TCP_Resp_CNT = 0;
            Operate.ProxyConfig.Proxy.UDP_Req_CNT = 0;
            Operate.ProxyConfig.Proxy.UDP_Resp_CNT = 0;
            Operate.FilterConfig.Filter.FilterExecute_CNT = 0;
            Operate.FilterConfig.Filter.FilterReplace_CNT = 0;
            Operate.FilterConfig.Filter.FilterChange_CNT = 0;
            Operate.FilterConfig.Filter.FilterIntercept_CNT = 0;
            Operate.FilterConfig.Filter.FilterDisplay_CNT = 0;
            Operate.FilterConfig.Filter.FilterNoDisplay_CNT = 0;
            Operate.ProxyConfig.Proxy.FilterProxy_CNT = 0;
            Operate.ProxyConfig.Proxy.Total_Request = 0;
            Operate.ProxyConfig.Proxy.Total_Response = 0;
        }

        private void CleanUp_ProxyList()
        {
            try
            {
                this.dgvProxyList.SuspendLayout();
                Operate.ProxyConfig.Queue.ClearProxyInfoQueue();
                Operate.ProxyConfig.List.ClearProxyInfo();
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(CleanUp_ProxyList), ex);
            }
            finally
            { 
                this.dgvProxyList.ResumeLayout();
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

        private void dgvProxyList_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvProxyList.SelectedRows.Count > 0)
                {
                    int selectedIndex = this.dgvProxyList.SelectedRows[0].Index;
                    if (selectedIndex >= 0 && selectedIndex < Operate.ProxyConfig.List.lstProxyInfo.Count)
                    {
                        Operate.ProxyConfig.List.Search_Index = selectedIndex;
                        Operate.ProxyConfig.List.piSelect = Operate.ProxyConfig.List.lstProxyInfo[selectedIndex];

                        this.controlPacketData?.RefreshPacketData();
                    }
                }                    
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(dgvProxyList_SelectionChanged), ex);
            }
        }

        #endregion
        
        #region//计时器 - 显示代理列表

        private void timerProxyList_Tick(object sender, EventArgs e)
        {
            try
            {
                this.timerProxyList.Stop();

                if (Operate.PacketConfig.List.AutoRoll && this.dgvProxyList.Rows.Count > 0 && dgvProxyList.Height > dgvProxyList.RowTemplate.Height)
                {
                    if (dgvProxyList.InvokeRequired)
                    {
                        dgvProxyList.Invoke(new Action(() =>
                        {
                            dgvProxyList.FirstDisplayedScrollingRowIndex = dgvProxyList.RowCount - 1;
                        }));
                    }
                    else
                    {
                        dgvProxyList.FirstDisplayedScrollingRowIndex = dgvProxyList.RowCount - 1;
                    }
                }

                //B9c：自动清理已移进 Operate.ProxyConfig.List.FlushToFeed，
                //这里只保留「清空后收拾右侧面板」，由 WinFormsUiFeed.Cleared 事件驱动（见 OnFeedCleared）。
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(timerProxyList_Tick), ex);
            }
            finally
            {
                this.timerProxyList.Start();
            }
        }

        #endregion

        #region//计时器 - 更新代理统计信息

        private void timerProxyListInfo_Tick(object sender, EventArgs e)
        {
            try
            {
                this.timerProxyListInfo.Stop();

                long ProxyTotal_CNT =
                    Operate.ProxyConfig.Proxy.TCP_Req_CNT +
                    Operate.ProxyConfig.Proxy.TCP_Resp_CNT +
                    Operate.ProxyConfig.Proxy.UDP_Req_CNT +
                    Operate.ProxyConfig.Proxy.UDP_Resp_CNT +
                    Operate.ProxyConfig.Proxy.HTTP_Req_CNT +
                    Operate.ProxyConfig.Proxy.HTTP_Resp_CNT;

                this.lProxyTotal_CNT.Text = ProxyTotal_CNT.ToString();
                this.lTCP_Req_CNT.Text = Operate.ProxyConfig.Proxy.TCP_Req_CNT.ToString();
                this.lTCP_Resp_CNT.Text = Operate.ProxyConfig.Proxy.TCP_Resp_CNT.ToString();
                this.lUDP_Req_CNT.Text = Operate.ProxyConfig.Proxy.UDP_Req_CNT.ToString();
                this.lUDP_Resp_CNT.Text = Operate.ProxyConfig.Proxy.UDP_Resp_CNT.ToString();
                this.lHTTP_Req_CNT.Text = Operate.ProxyConfig.Proxy.HTTP_Req_CNT.ToString();
                this.lHTTP_Resp_CNT.Text = Operate.ProxyConfig.Proxy.HTTP_Resp_CNT.ToString();
                this.lFilterExecute_CNT.Text = Operate.FilterConfig.Filter.FilterExecute_CNT.ToString();
                this.lProxyQueue_CNT.Text = Operate.ProxyConfig.Queue.qProxyInfo.Count.ToString();
                this.lFilterProxy_CNT.Text = Operate.ProxyConfig.Proxy.FilterProxy_CNT.ToString();
                this.lProxyTCP_CNT.Text = Operate.ProxyConfig.Proxy.ProxyServer?.SessionCount.ToString() ?? "0";
                this.lProxyUDP_CNT.Text = Operate.ProxyConfig.List.cdProxyUDP.Count.ToString();

                //三个统计字符串的计算已搬进 Operate（两套 UI 共用），这里只负责显示
                Operate.ProxyConfig.Proxy.RefreshStatInfo();

                this.lProxyAccount_CNT.Text = Operate.ProxyConfig.Proxy.ProxyOnLineInfo;
                this.lTotalBytes.Text = Operate.ProxyConfig.Proxy.ProxyBytesInfo;
                this.lProxySpeed.Text = Operate.ProxyConfig.Proxy.ProxySpeedInfo;

                Operate.ProxyConfig.Proxy.CloseUDPTimeOut();
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(timerProxyListInfo_Tick), ex);
            }
            finally
            {
                this.timerProxyListInfo.Start();
            }
        }

        #endregion

        #region//查找封包（异步）

        public void SearchProxyList(bool FromHead)
        {
            if (!this.bgwSearchProxyList.IsBusy)
            {
                this.SearchFromHead = FromHead;
                this.bgwSearchProxyList.RunWorkerAsync();
            }
        }        

        private void bgwSearchProxyList_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (Operate.ProxyConfig.List.lstProxyInfo.Count > 0 && Operate.PacketConfig.List.FindOptions.IsValid)
                {
                    if (this.SearchFromHead)
                    {
                        Operate.ProxyConfig.List.Search_Index = 0;
                    }                    

                    e.Result = Operate.PacketConfig.List.SearchForList<ProxyInfo>(Operate.ProxyConfig.List.Search_Index, false);
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(bgwSearchProxyList_DoWork), ex);
            }
        }

        private void bgwSearchProxyList_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                if (e.Error == null && !e.Cancelled && e.Result != null)
                {
                    if (int.TryParse(e.Result.ToString(), out int iSearchResultIndex))
                    {
                        if (iSearchResultIndex >= 0 && iSearchResultIndex < dgvProxyList.Rows.Count)
                        {
                            dgvProxyList.SuspendLayout();
                            dgvProxyList.FirstDisplayedScrollingRowIndex = iSearchResultIndex;
                            dgvProxyList.Rows[iSearchResultIndex].Selected = true;
                            dgvProxyList.CurrentCell = dgvProxyList.Rows[iSearchResultIndex].Cells[0];
                            dgvProxyList.ResumeLayout();

                            bool bFindNext = this.controlPacketData?.HexBox_FindNext() ?? false;
                            if (!bFindNext)
                            {
                                Operate.ProxyConfig.List.Search_Index += 1;
                                this.SearchProxyList(this.SearchFromHead);
                            }
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
                Operate.DoLog(nameof(bgwSearchProxyList_RunWorkerCompleted), ex);
            }
        }

        #endregion        

        #region//窗口保持最前

        private void cbTopMost_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.form.TopMost = this.cbTopMost.Checked;
        }

        #endregion
    }
}
