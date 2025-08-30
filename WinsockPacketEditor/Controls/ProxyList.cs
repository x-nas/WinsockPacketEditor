using AntdUI;
using Be.Windows.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class ProxyList : UserControl
    {
        private Form form;
        public bool SearchFromHead = true;

        #region//窗体事件

        public ProxyList(Form form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void ProxyList_Load(object sender, EventArgs e)
        {
            this.hbProxyData.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();

            this.InitTable_ProxyList();
            this.Dark_Changed();
        }

        private void Dark_Changed()
        {
            if (AntdUI.Config.IsDark)
            {
                this.tProxyList.BackColor = Operate.SystemConfig.Color_40;
                this.tProxyList.ColumnBack = Operate.SystemConfig.Color_40;
                this.tProxyList.ColumnFore = Color.Silver;
                this.tProxyList.ForeColor = Color.Lime;

                this.pPacketData.Back = Operate.SystemConfig.Color_40;
                this.hbProxyData.BackColor = Operate.SystemConfig.Color_40;
                this.hbProxyData.ForeColor = Color.Silver;
            }
            else
            {
                this.tProxyList.BackColor = Color.White;
                this.tProxyList.ColumnBack = Color.White;
                this.tProxyList.ColumnFore = Color.Black;
                this.tProxyList.ForeColor = Color.Green;

                this.pPacketData.Back = Color.White;
                this.hbProxyData.BackColor = Color.White;
                this.hbProxyData.ForeColor = Color.Black;
            }
        }

        public void RefreshProxyData()
        {
            if (Operate.ProxyConfig.List.piSelect != null)
            {
                DynamicByteProvider dbp = new DynamicByteProvider(Operate.ProxyConfig.List.piSelect.PacketBuffer);
                hbProxyData.ByteProvider = dbp;
            }
        }

        #endregion

        #region//初始化表格

        private void InitTable_ProxyList()
        {
            tProxyList.Columns = new AntdUI.ColumnCollection {
                new AntdUI.Column(string.Empty, string.Empty, AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is ProxyInfo pi)
                        {
                            return new AntdUI.CellImage(Operate.PacketConfig.Packet.GetImg_ByPacketType(pi.PacketType));
                        }

                        return value;
                    },
                }.SetFixed(),
                new AntdUI.Column("", "序号", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return (rowindex + 1);
                    },
                }.SetFixed().SetLocalizationTitleID("Table.ProxyList.Column.ID"),
                new AntdUI.Column("ProxyTime", "时间戳", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return ((DateTime)value).ToString("HH:mm:ss:fffffff");
                    },
                }.SetLocalizationTitleID("Table.ProxyList.Column."),
                new AntdUI.Column("PacketType", "类别", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return Operate.PacketConfig.Packet.GetName_ByPacketType((Operate.PacketConfig.Packet.PacketType)value);
                    },
                }.SetLocalizationTitleID("Table.ProxyList.Column."),
                new AntdUI.Column("PacketSocket", "套接字", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.ProxyList.Column."),
                new AntdUI.Column("ClientAddr", "客户端地址")
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is ProxyInfo pi)
                        {
                            return new CellText(value?.ToString() ?? string.Empty)
                            {
                                PrefixSvg = Operate.SystemConfig.GetSvgByLocation(pi.ClientLocation),
                                IconRatio = 1.0F
                            };
                        }

                        return value;
                    },
                }.SetLocalizationTitleID("Table.ProxyList.Column."),
                new AntdUI.Column("ClientLocation", "所属地").SetWidth("100").SetLocalizationTitleID("Table.ProxyList.Column."),
                new AntdUI.Column("ServerDomain", "服务端地址")
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is ProxyInfo pi)
                        {
                            return new CellText(value?.ToString() ?? string.Empty)
                            {
                                PrefixSvg = Operate.SystemConfig.GetSvgByLocation(pi.ServerLocation),
                                IconRatio = 1.0F
                            };
                        }

                        return value;
                    },
                }.SetLocalizationTitleID("Table.ProxyList.Column."),
                new AntdUI.Column("ServerLocation", "所属地").SetWidth("100").SetLocalizationTitleID("Table.ProxyList.Column."),
                new AntdUI.Column("PacketLen", "长度", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.ProxyList.Column."),
                new AntdUI.Column("PacketData", "数据").SetLocalizationTitleID("Table.ProxyList.Column."),
            };

            this.tProxyList.ColumnFont = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.tProxyList.DataSource = Operate.ProxyConfig.List.lstProxyInfo;
        }

        public void SetColumnVisible_ProxyList()
        {
            try
            {
                this.tProxyList.Columns[1].Visible = Operate.ProxyConfig.List.IsShow_ID;
                this.tProxyList.Columns[2].Visible = Operate.ProxyConfig.List.IsShow_ProxyTime;
                this.tProxyList.Columns[3].Visible = Operate.ProxyConfig.List.IsShow_PacketType;
                this.tProxyList.Columns[4].Visible = Operate.ProxyConfig.List.IsShow_PacketSocket;
                this.tProxyList.Columns[5].Visible = Operate.ProxyConfig.List.IsShow_ClientAddr;
                this.tProxyList.Columns[6].Visible = Operate.ProxyConfig.List.IsShow_ClientLocation;
                this.tProxyList.Columns[7].Visible = Operate.ProxyConfig.List.IsShow_ServerAddr;
                this.tProxyList.Columns[8].Visible = Operate.ProxyConfig.List.IsShow_ServerLocation;
                this.tProxyList.Columns[9].Visible = Operate.ProxyConfig.List.IsShow_PacketLen;
                this.tProxyList.Columns[10].Visible = Operate.ProxyConfig.List.IsShow_PacketData;

            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private Table.CellStyleInfo tProxyList_SetRowStyle(object sender, TableSetRowStyleEventArgs e)
        {
            try
            {
                int index = e.RowIndex - 1;
                if (index > -1 && index < Operate.ProxyConfig.List.lstProxyInfo.Count)
                {
                    ProxyInfo pi = Operate.ProxyConfig.List.lstProxyInfo[index];
                    if (pi != null)
                    {
                        switch (pi.FilterAction)
                        {
                            case Operate.FilterConfig.Filter.FilterAction.Replace:

                                return new AntdUI.Table.CellStyleInfo
                                {
                                    ForeColor = Operate.FilterConfig.Filter.FilterActionForeColor_Replace,
                                    BackColor = Operate.FilterConfig.Filter.FilterActionBackColor_Replace,
                                };

                            case Operate.FilterConfig.Filter.FilterAction.Intercept:

                                return new AntdUI.Table.CellStyleInfo
                                {
                                    ForeColor = Operate.FilterConfig.Filter.FilterActionForeColor_Intercept,
                                    BackColor = Operate.FilterConfig.Filter.FilterActionBackColor_Intercept,
                                };

                            case Operate.FilterConfig.Filter.FilterAction.Change:

                                return new AntdUI.Table.CellStyleInfo
                                {
                                    ForeColor = Operate.FilterConfig.Filter.FilterActionForeColor_Change,
                                    BackColor = Operate.FilterConfig.Filter.FilterActionBackColor_Change,
                                };

                            default:
                                return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }

            return null;
        }

        #endregion

        #region//代理列表 - 菜单

        private void bProxyStart_Click(object sender, EventArgs e)
        {
            this.bProxyStart.Enabled = false;
            this.bProxyStop.Enabled = true;

            this.Start_Proxy();
        }

        private void bProxyStop_Click(object sender, EventArgs e)
        {
            this.bProxyStart.Enabled = true;
            this.bProxyStop.Enabled = false;

            this.Stop_Proxy();
        }

        private void bProxyList_Clear_Click(object sender, EventArgs e)
        {
            this.CleanUp_ProxyList();
            this.CleanUp_ProxyListInfo();
            this.CleanUp_HexBox();

            if (this.form is InterfaceInfo.IProxyMode proxyForm)
            {
                proxyForm.CleanUp_LogList();
            }

            AntdUI.Message.open(new AntdUI.Message.Config(this.form, "已清空数据", TType.Warn)
            {
                LocalizationText = "InjectModeForm.Clear"
            });
        }

        private void mProxyList_SelectChanged(object sender, MenuSelectEventArgs e)
        {
            AntdUI.MenuItem miSelect = e.Value;
            this.mProxyList.USelect();

            switch (miSelect.ID)
            {
                case "miProxyListSearch":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this.form, new SearchPacketForm(this.form)
                    {
                        Size = new Size(1000, 100),
                    })
                    {
                        Align = AntdUI.TAlignMini.Top,
                        Mask = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "miProxySettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this.form, new ProxySettingsForm(this.form))
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "miFilterSettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this.form, new FilterSettingsForm(this.form))
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "miHookSettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this.form, new HookSettingsForm(this.form))
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "miListSettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this.form, new ListSettingsForm(this.form))
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "miMapSettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this.form, new MapSettingsForm(this.form))
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "miExternalProxySettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this.form, new EXTProxySettingsForm(this.form))
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "miHotKeySettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this.form, new HotKeyForm(this.form))
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "miBackUpSettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this.form, new BackUpSettingsForm(this.form))
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;

                case "miSystemSettings":

                    AntdUI.Drawer.open(new AntdUI.Drawer.Config(this.form, new SystemSettingsForm(this.form))
                    {
                        Align = AntdUI.TAlignMini.Right,
                        Mask = true,
                        MaskClosable = false,
                        DisplayDelay = 0,
                    });

                    break;
            }
        }

        #endregion

        #region//代理列表 - 右键菜单

        private void tProxyList_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Operate.ProxyConfig.List.lstProxyInfo.Count == 0)
                {
                    return;
                }

                AntdUI.ContextMenuStrip.open(tProxyList, item =>
                {
                    List<ProxyInfo> piList = new List<ProxyInfo>();

                    foreach (int SelectIndex in this.tProxyList.SelectedIndexs)
                    {
                        piList.Add(Operate.ProxyConfig.List.lstProxyInfo[SelectIndex - 1]);
                    }

                    switch (item.ID)
                    {
                        case "Edit":

                            if (piList.Count > 0)
                            {
                                var PacketEdit = new PacketEdit(this.form, piList[0]);
                                AntdUI.Modal.open(new AntdUI.Modal.Config(this.form, AntdUI.Localization.Get("PacketEditForm", "封包编辑"), PacketEdit)
                                {
                                    Keyboard = false,
                                    MaskClosable = false,
                                    BtnHeight = 0,
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

                            Operate.ProxyConfig.List.SaveProxyList_Dialog(this.form, this.tProxyList, Operate.PacketConfig.Packet.InjectProcess, piList);

                            break;

                        case "ToTextA":

                            if (piList.Count > 0)
                            {
                                if (this.form is InterfaceInfo.IProxyMode proxyForm)
                                {
                                    string TextA = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, piList[0].PacketBuffer);
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
                                    string TextB = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, piList[0].PacketBuffer);
                                    proxyForm.SetTextB(TextB);

                                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "已添加到文本B", TType.Success)
                                    {
                                        LocalizationText = "ToTextB"
                                    });
                                }
                            }

                            break;

                        case "DeSelect":

                            this.tProxyList.SelectedIndex = -1;

                            break;

                        default:

                            if (piList.Count > 0)
                            {
                                if (Guid.TryParse(item.ID, out Guid SID))
                                {
                                    SendInfo si = Operate.SendConfig.Send.GetSend_ByGuid(SID);
                                    if (si != null && piList.Count > 0)
                                    {
                                        if (Operate.SendConfig.Send.AddSendCollection_ByProxyInfo(SID, piList))
                                        {
                                            string sText = string.Format(AntdUI.Localization.Get("ToSendList.Success", "已添加到: {0}"), item.Text);
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
                            }

                            break;
                    }
                }, Operate.PacketConfig.List.GetCMS_PacketList());
            }
        }

        #endregion

        #region//代理数据 - 右键菜单

        private void hbProxyData_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                DynamicByteProvider dbp = hbProxyData.ByteProvider as DynamicByteProvider;
                if (dbp == null || dbp.Bytes.Count == 0)
                {
                    return;
                }

                AntdUI.ContextMenuStrip.open(new AntdUI.ContextMenuStrip.Config(hbProxyData, (item) =>
                {
                    switch (item.ID)
                    {
                        case "Edit":

                            if (Operate.PacketConfig.List.piSelect != null)
                            {
                                var PacketEdit = new PacketEdit(this.form, Operate.ProxyConfig.List.piSelect);
                                AntdUI.Modal.open(new AntdUI.Modal.Config(this.form, AntdUI.Localization.Get("PacketEditForm", "封包编辑"), PacketEdit)
                                {
                                    Keyboard = false,
                                    MaskClosable = false,
                                    BtnHeight = 0,
                                });
                            }

                            break;

                        case "ToFilterList":

                            if (Operate.ProxyConfig.List.piSelect != null)
                            {
                                bool bOK = false;
                                if (this.hbProxyData.CanCopy())
                                {
                                    this.hbProxyData.CopyHex();
                                    byte[] bBufferCopy = Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, Clipboard.GetText());
                                    bOK = Operate.FilterConfig.Filter.AddFilter_ByProxyInfo(Operate.ProxyConfig.List.piSelect, bBufferCopy);
                                }
                                else
                                {
                                    bOK = Operate.FilterConfig.Filter.AddFilter_ByProxyInfo(Operate.ProxyConfig.List.piSelect, dbp.Bytes.ToArray());
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

                            this.hbProxyData.Copy();

                            break;

                        case "Copy_Hex":

                            this.hbProxyData.CopyHex();

                            break;

                        case "ToTextA":

                            string StringA = string.Empty;
                            if (this.hbProxyData.CanCopy())
                            {
                                this.hbProxyData.CopyHex();
                                StringA = Clipboard.GetText();
                            }
                            else
                            {
                                StringA = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, dbp.Bytes.ToArray());
                            }

                            if (this.form is InterfaceInfo.IProxyMode proxyFormA)
                            {
                                proxyFormA.SetTextA(StringA);

                                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "已添加到文本A", TType.Success)
                                {
                                    LocalizationText = "ToTextA"
                                });
                            }

                            break;

                        case "ToTextB":

                            string StringB = string.Empty;
                            if (this.hbProxyData.CanCopy())
                            {
                                this.hbProxyData.CopyHex();
                                StringB = Clipboard.GetText();
                            }
                            else
                            {
                                StringB = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, dbp.Bytes.ToArray());
                            }

                            if (this.form is InterfaceInfo.IProxyMode proxyFormB)
                            {
                                proxyFormB.SetTextB(StringB);

                                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "已添加到文本B", TType.Success)
                                {
                                    LocalizationText = "ToTextB"
                                });
                            }

                            break;

                        case "SelectAll":

                            this.hbProxyData.SelectAll();

                            break;

                        default:

                            if (Operate.ProxyConfig.List.piSelect == null)
                            {
                                return;
                            }

                            if (Guid.TryParse(item.ID, out Guid SID))
                            {
                                SendInfo si = Operate.SendConfig.Send.GetSend_ByGuid(SID);
                                if (si != null)
                                {
                                    byte[] bBuffer = null;
                                    if (this.hbProxyData.CanCopy())
                                    {
                                        this.hbProxyData.CopyHex();
                                        bBuffer = Operate.SystemConfig.StringToBytes(Operate.PacketConfig.Packet.EncodingFormat.Hex, Clipboard.GetText());
                                    }
                                    else
                                    {
                                        bBuffer = dbp.Bytes.ToArray();
                                    }

                                    List<ProxyInfo> piList = new List<ProxyInfo>
                                    {
                                        new ProxyInfo
                                        {
                                            PacketSocket = Operate.ProxyConfig.List.piSelect.PacketSocket,
                                            PacketType = Operate.ProxyConfig.List.piSelect.PacketType,
                                            ClientAddr = Operate.ProxyConfig.List.piSelect.ClientAddr,
                                            ServerAddr = Operate.ProxyConfig.List.piSelect.ServerAddr,
                                            PacketBuffer = bBuffer,
                                            PacketLen = bBuffer.Length,
                                            PacketData = Operate.PacketConfig.Packet.GetPacketData_Hex(bBuffer, Operate.PacketConfig.Packet.PacketData_MaxLen),
                                        }
                                    };

                                    if (Operate.SendConfig.Send.AddSendCollection_ByProxyInfo(SID, piList))
                                    {
                                        string sText = string.Format(AntdUI.Localization.Get("ToSendList.Success", "已添加到: {0}"), item.Text);
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

                            break;
                    }
                }, Operate.PacketConfig.Packet.GetCMS_PacketData(this.hbProxyData)));
            }
        }

        #endregion

        #region//开始代理

        private void Start_Proxy()
        {
            try
            {
                Operate.ProxyConfig.Proxy.IsListening = true;

                if (Operate.ProxyConfig.Proxy.ProxyIP_Auto)
                {
                    Operate.ProxyConfig.Proxy.ProxyTCP_IP = IPAddress.Any;
                    Operate.ProxyConfig.Proxy.ProxyUDP_IP = Operate.ProxyConfig.Proxy.ProxyServerIP[0];
                }
                else
                {
                    if (IPAddress.TryParse(Operate.ProxyConfig.Proxy.ProxyIP, out IPAddress proxyIP))
                    {
                        Operate.ProxyConfig.Proxy.ProxyTCP_IP = proxyIP;
                        Operate.ProxyConfig.Proxy.ProxyUDP_IP = proxyIP;
                    }
                    else
                    {
                        Operate.ProxyConfig.Proxy.ProxyTCP_IP = IPAddress.Any;
                        Operate.ProxyConfig.Proxy.ProxyUDP_IP = Operate.ProxyConfig.Proxy.ProxyServerIP[0];
                    }
                }

                string sProxyIP = string.Format(AntdUI.Localization.Get("ProxyModeForm.ProxyServerIP", "代理服务器IP地址: TCP [{0}] UDP [{1}]"), Operate.ProxyConfig.Proxy.ProxyTCP_IP, Operate.ProxyConfig.Proxy.ProxyUDP_IP);
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, sProxyIP);

                if (Operate.ProxyConfig.Proxy.Enable_Auth)
                {
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, AntdUI.Localization.Get("ProxyModeForm.ProxyServer.Auth", "已启用代理服务身份认证"));
                }

                if (Operate.ProxyConfig.Proxy.Enable_ExternalProxy)
                {
                    Operate.DoLog(MethodBase.GetCurrentMethod().Name, AntdUI.Localization.Get("ProxyModeForm.ProxyServer.EXTProxy", "已启用外部 SOCKS5 代理"));
                }

                if (Operate.ProxyConfig.Proxy.ProxyServer == null)
                {
                    InitializeServerSocket();
                }

                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "开始 SOCKS5 代理", TType.Success)
                {
                    LocalizationText = "ProxyModeForm.StartProxy"
                });
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void InitializeServerSocket()
        {
            try
            {
                Operate.ProxyConfig.Proxy.ProxyServer?.Close();
                Operate.ProxyConfig.Proxy.ProxyServer?.Dispose();

                IPEndPoint ep = new IPEndPoint(Operate.ProxyConfig.Proxy.ProxyTCP_IP, Operate.ProxyConfig.Proxy.ProxyPort);
                Operate.ProxyConfig.Proxy.ProxyServer = new Socket(ep.AddressFamily, SocketType.Stream, ProtocolType.Tcp)
                {
                    NoDelay = true,
                    LingerState = new LingerOption(false, 0),
                    ExclusiveAddressUse = false
                };

                Operate.ProxyConfig.Proxy.ProxyServer.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);

                Operate.ProxyConfig.Proxy.ProxyServer.Bind(ep);
                Operate.ProxyConfig.Proxy.ProxyServer.Listen(backlog: 1000);

                AcceptClients();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void AcceptClients()
        {
            try
            {
                if (Operate.ProxyConfig.Proxy.IsListening && Operate.ProxyConfig.Proxy.ProxyServer != null)
                {
                    var acceptArgs = new SocketAsyncEventArgs();
                    acceptArgs.Completed += AcceptCompleted;

                    if (!Operate.ProxyConfig.Proxy.ProxyServer.AcceptAsync(acceptArgs))
                    {
                        AcceptCompleted(null, acceptArgs);
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                // Socket已关闭，正常退出
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                Task.Delay(5000).ContinueWith(_ => AcceptClients());
            }
        }

        private async void AcceptCompleted(object sender, SocketAsyncEventArgs e)
        {
            try
            {
                if (e.SocketError == SocketError.Success && Operate.ProxyConfig.Proxy.IsListening && e.AcceptSocket != null)
                {
                    await Operate.ProxyConfig.Proxy.HandleClient(e.AcceptSocket);

                    e.AcceptSocket = null;

                    if (Operate.ProxyConfig.Proxy.IsListening)
                    {
                        if (!Operate.ProxyConfig.Proxy.ProxyServer.AcceptAsync(e))
                        {
                            AcceptCompleted(null, e);
                        }
                    }
                    else
                    {
                        e.Dispose();
                    }
                }
                else
                {
                    e.Dispose();
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                e.Dispose();
            }
        }

        #endregion

        #region//停止代理

        private void Stop_Proxy()
        {
            try
            {
                Operate.ProxyConfig.Proxy.IsListening = false;

                if (Operate.ProxyConfig.Proxy.ProxyServer != null)
                {
                    try
                    {
                        Operate.ProxyConfig.Proxy.ProxyServer.Close();
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
                    }
                    finally
                    {
                        Operate.ProxyConfig.Proxy.ProxyServer = null;
                    }
                }

                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "停止 SOCKS5 代理", TType.Error)
                {
                    LocalizationText = "ProxyModeForm.StopProxy"
                });
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Operate.ProxyConfig.Queue.ResetProxyInfoQueue();
                Operate.ProxyConfig.List.lstProxyInfo.Clear();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void CleanUp_HexBox()
        {
            if (hbProxyData.InvokeRequired)
            {
                hbProxyData.Invoke(new Action(CleanUp_HexBox));
                return;
            }

            if (hbProxyData.ByteProvider != null)
            {
                IDisposable byteProvider = hbProxyData.ByteProvider as IDisposable;

                if (byteProvider != null)
                {
                    byteProvider.Dispose();
                }

                hbProxyData.ByteProvider = null;
            }
        }

        #endregion

        #region//显示选中的封包数据

        private void tProxyList_SelectIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int selectedIndex = tProxyList.SelectedIndex - 1;
                if (selectedIndex >= 0 && selectedIndex < Operate.ProxyConfig.List.lstProxyInfo.Count)
                {
                    Operate.ProxyConfig.List.Search_Index = selectedIndex;
                    Operate.ProxyConfig.List.piSelect = Operate.ProxyConfig.List.lstProxyInfo[selectedIndex];

                    DynamicByteProvider dbp = new DynamicByteProvider(Operate.ProxyConfig.List.piSelect.PacketBuffer);
                    hbProxyData.ByteProvider = dbp;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//显示代理列表

        public void RefreshProxyList()
        {
            if (tProxyList.InvokeRequired)
            {
                tProxyList.BeginInvoke(new Action(() => this.tProxyList.Refresh()));
            }
            else
            {
                this.tProxyList.Refresh();
            }

            if (Operate.ProxyConfig.List.AutoRoll)
            {
                tProxyList.ScrollBar.ValueY = tProxyList.ScrollBar.MaxY;
            }

            if (Operate.ProxyConfig.List.AutoClear)
            {
                if (Operate.ProxyConfig.List.lstProxyInfo.Count > Operate.ProxyConfig.List.AutoClear_Value)
                {
                    this.CleanUp_ProxyList();
                    this.CleanUp_HexBox();
                }
            }

            this.ShowProxyInfo();
        }

        #endregion

        #region//显示代理信息

        private void ShowProxyInfo()
        {
            try
            {
                long ProxyTotal_CNT =
                    Operate.ProxyConfig.Proxy.TCP_Req_CNT +
                    Operate.ProxyConfig.Proxy.TCP_Resp_CNT +
                    Operate.ProxyConfig.Proxy.UDP_Req_CNT +
                    Operate.ProxyConfig.Proxy.UDP_Resp_CNT;

                this.lProxyTotal_CNT.Text = ProxyTotal_CNT.ToString();
                this.lTCP_Req_CNT.Text = Operate.ProxyConfig.Proxy.TCP_Req_CNT.ToString();
                this.lTCP_Resp_CNT.Text = Operate.ProxyConfig.Proxy.TCP_Resp_CNT.ToString();
                this.lUDP_Req_CNT.Text = Operate.ProxyConfig.Proxy.UDP_Req_CNT.ToString();
                this.lUDP_Resp_CNT.Text = Operate.ProxyConfig.Proxy.UDP_Resp_CNT.ToString();
                this.lFilterExecute_CNT.Text = Operate.FilterConfig.Filter.FilterExecute_CNT.ToString();
                this.lProxyQueue_CNT.Text = Operate.ProxyConfig.Queue.qProxyInfo.Count.ToString();
                this.lFilterProxy_CNT.Text = Operate.ProxyConfig.Proxy.FilterProxy_CNT.ToString();
                this.lProxyTCP_CNT.Text = Operate.ProxyConfig.List.lstProxyTCP.Count.ToString();
                this.lProxyUDP_CNT.Text = Operate.ProxyConfig.List.cdProxyUDP.Count.ToString();

                Operate.ProxyConfig.Proxy.ProxyOnLineInfo = string.Format(
                        "{0}/{1}",
                        Operate.ProxyConfig.Account.GetOnLineProxyAccountCount(Operate.ProxyConfig.Account.lstAccountInfo),
                        Operate.ProxyConfig.Account.lstAccountInfo.Count);
                this.lProxyAccount_CNT.Text = Operate.ProxyConfig.Proxy.ProxyOnLineInfo;

                Operate.ProxyConfig.Proxy.ProxyBytesInfo = string.Format(
                    AntdUI.Localization.Get("ProxyModeForm.ProxyBytesInfo", "请求: {0}  响应: {1}"),
                    Operate.SystemConfig.GetDisplayBytes(Operate.ProxyConfig.Proxy.Total_Request),
                    Operate.SystemConfig.GetDisplayBytes(Operate.ProxyConfig.Proxy.Total_Response));
                this.lTotalBytes.Text = Operate.ProxyConfig.Proxy.ProxyBytesInfo;

                decimal dUplink = Operate.ProxyConfig.Proxy.ProxySpeed_Uplink / 1024;
                Operate.ProxyConfig.Proxy.ProxySpeed_Uplink = 0;
                decimal dDownlink = Operate.ProxyConfig.Proxy.ProxySpeed_Downlink / 1024;
                Operate.ProxyConfig.Proxy.ProxySpeed_Downlink = 0;

                Operate.ProxyConfig.Proxy.ProxySpeedInfo = string.Format(
                    AntdUI.Localization.Get("ProxyModeForm.ProxySpeedInfo", "上行: {0} KB/s  下行: {1} KB/s"),
                    dUplink.ToString("0.00"),
                    dDownlink.ToString("0.00"));
                this.lProxySpeed.Text = Operate.ProxyConfig.Proxy.ProxySpeedInfo;
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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

        public void HexBox_FindNext()
        {
            try
            {
                if (Operate.PacketConfig.List.FindOptions.IsValid)
                {
                    long res = this.hbProxyData.Find(Operate.PacketConfig.List.FindOptions);

                    if (res == -1)
                    {
                        Operate.ProxyConfig.List.Search_Index += 1;
                        this.SearchProxyList(this.SearchFromHead);
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bgwSearchProxyList_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                if (Operate.ProxyConfig.List.lstProxyInfo.Count > 0)
                {
                    if (Operate.PacketConfig.List.FindOptions.IsValid)
                    {
                        byte[] bSearchContent = null;
                        FindType fType = Operate.PacketConfig.List.FindOptions.Type;
                        Operate.PacketConfig.Packet.EncodingFormat efFormat = new Operate.PacketConfig.Packet.EncodingFormat();

                        switch (fType)
                        {
                            case FindType.Text:
                                efFormat = Operate.PacketConfig.Packet.EncodingFormat.UTF7;
                                bSearchContent = Operate.SystemConfig.StringToBytes(efFormat, Operate.PacketConfig.List.FindOptions.Text);
                                break;

                            case FindType.Hex:
                                efFormat = Operate.PacketConfig.Packet.EncodingFormat.Hex;
                                bSearchContent = Operate.PacketConfig.List.FindOptions.Hex;
                                break;
                        }

                        if (this.SearchFromHead)
                        {
                            Operate.ProxyConfig.List.Search_Index = 0;
                        }

                        e.Result = Operate.ProxyConfig.List.SearchForProxyList(Operate.ProxyConfig.List.Search_Index, bSearchContent);
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                        if (iSearchResultIndex >= 0)
                        {
                            this.tProxyList.SelectedIndex = iSearchResultIndex + 1;
                            this.tProxyList.ScrollLine(iSearchResultIndex + 1, true);
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
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion                                                
    }
}
