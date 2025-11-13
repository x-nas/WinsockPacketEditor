using AntdUI;
using Be.Windows.Forms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class PacketData : UserControl
    {
        private Form form = null;

        #region//窗体事件

        public PacketData(Form form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void PacketData_Load(object sender, EventArgs e)
        {
            this.tabPacketData.TabMenuVisible = false;
            this.hbPacketData.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();

            this.Dark_Changed();
        }

        public void Dark_Changed()
        {
            if (AntdUI.Config.IsDark)
            {
                this.hbPacketData.BackColor = Operate.SystemConfig.Color_40;
                this.hbPacketData.ForeColor = Color.Silver;
            }
            else
            {
                this.hbPacketData.BackColor = Color.White;
                this.hbPacketData.ForeColor = Color.Black;
            }
        }

        #endregion

        #region//显示选中的封包数据

        public void RefreshPacketData()
        {
            try
            {
                if (this.form is InterfaceInfo.IInjectMode)
                {
                    if (Operate.PacketConfig.List.piSelect != null)
                    {
                        this.tabPacketData.SelectTab("tpHex");

                        DynamicByteProvider dbp = new DynamicByteProvider(Operate.PacketConfig.List.piSelect.PacketBuffer);
                        this.hbPacketData.ByteProvider = dbp;
                    }
                }
                else if (this.form is InterfaceInfo.IProxyMode proxyForm)
                {
                    if (Operate.ProxyConfig.List.piSelect != null)
                    {
                        if (Operate.ProxyConfig.List.piSelect.PacketType == Operate.PacketConfig.Packet.PacketType.HTTP_Req ||
                            Operate.ProxyConfig.List.piSelect.PacketType == Operate.PacketConfig.Packet.PacketType.HTTP_Resp ||
                            Operate.ProxyConfig.List.piSelect.PacketType == Operate.PacketConfig.Packet.PacketType.HTTPS_Req ||
                            Operate.ProxyConfig.List.piSelect.PacketType == Operate.PacketConfig.Packet.PacketType.HTTPS_Resp)
                        {
                            this.tabPacketData.SelectTab("tpText");
                            this.ShowText(Operate.ProxyConfig.List.piSelect.PacketData);                            
                        }
                        else
                        {
                            this.tabPacketData.SelectTab("tpHex");

                            DynamicByteProvider dbp = new DynamicByteProvider(Operate.ProxyConfig.List.piSelect.PacketBuffer);
                            this.hbPacketData.ByteProvider = dbp;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(RefreshPacketData), ex);
            }            
        }

        private void ShowText(string PacketData)
        {
            try
            {
                this.txtText.SuspendLayout();
                this.txtText.Clear();

                AntdUI.Spin.open(this.form, new AntdUI.Spin.Config()
                {
                    Radius = 6,
                    Font = new Font("Microsoft YaHei UI", 9F),
                }, (config) =>
                {
                    config.Text = AntdUI.Localization.Get("Loading", "正在加载...");
                    this.txtText.Text = PacketData;

                }, () =>
                {
                    this.txtText.ResumeLayout();
                });
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(ShowText), ex);
            }            
        }

        #endregion

        #region//查找封包数据

        public bool HexBox_FindNext()
        {
            try
            {
                if (Operate.PacketConfig.List.FindOptions.IsValid)
                {
                    if (Operate.PacketConfig.List.FindOptions.Type == FindType.Hex && Operate.PacketConfig.List.FindOptions.Hex.Length == 0)
                    {
                        return false;
                    }

                    long res = this.hbPacketData.Find(Operate.PacketConfig.List.FindOptions);
                    if (res == -1)
                    {
                        return false;
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(HexBox_FindNext), ex);
            }

            return false;
        }

        #endregion

        #region//清除数据

        public void CleanUp_PacketData()
        {
            try
            {
                this.txtText.Clear();

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
            catch (Exception ex)
            {
                Operate.DoLog(nameof(CleanUp_PacketData), ex);
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

                            if (this.form is InterfaceInfo.IInjectMode)
                            {
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
                            }
                            else if (this.form is InterfaceInfo.IProxyMode proxyForm)
                            {
                                if (Operate.ProxyConfig.List.piSelect != null)
                                {
                                    var PacketEdit = new PacketEdit(this.form, Operate.ProxyConfig.List.piSelect);
                                    AntdUI.Modal.open(new AntdUI.Modal.Config(this.form, AntdUI.Localization.Get("PacketEditForm", "封包编辑"), PacketEdit)
                                    {
                                        Keyboard = false,
                                        MaskClosable = false,
                                        BtnHeight = 0,
                                    });
                                }
                            }                            

                            break;

                        case "ToFilterList":

                            if (this.form is InterfaceInfo.IInjectMode)
                            {
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
                            }
                            else if (this.form is InterfaceInfo.IProxyMode proxyForm)
                            {
                                if (Operate.ProxyConfig.List.piSelect != null)
                                {
                                    bool bOK = false;
                                    if (this.hbPacketData.CanCopy())
                                    {
                                        this.hbPacketData.CopyHex();
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
                            }
                            else if (this.form is InterfaceInfo.IProxyMode proxyFormA)
                            {
                                proxyFormA.SetTextA(StringA);                                
                            }

                            AntdUI.Message.open(new AntdUI.Message.Config(this.form, "已添加到文本A", TType.Success)
                            {
                                LocalizationText = "ToTextA"
                            });

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
                            }
                            else if (this.form is InterfaceInfo.IProxyMode proxyFormB)
                            {
                                proxyFormB.SetTextB(StringB);
                            }

                            AntdUI.Message.open(new AntdUI.Message.Config(this.form, "已添加到文本B", TType.Success)
                            {
                                LocalizationText = "ToTextB"
                            });

                            break;

                        case "SelectAll":

                            this.hbPacketData.SelectAll();

                            break;

                        default:

                            if (this.form is InterfaceInfo.IInjectMode)
                            {
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
                            }
                            else if (this.form is InterfaceInfo.IProxyMode proxyForm)
                            {
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
                                        if (this.hbPacketData.CanCopy())
                                        {
                                            this.hbPacketData.CopyHex();
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
                            }                            

                            break;
                    }
                }, Operate.PacketConfig.Packet.GetCMS_PacketData(this.hbPacketData)));
            }
        }

        #endregion
    }
}
