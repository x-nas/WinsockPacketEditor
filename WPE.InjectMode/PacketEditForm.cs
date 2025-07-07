using AntdUI;
using Be.Windows.Forms;
using System;
using System.Reflection;
using System.Windows.Forms;
using WPE.Lib;

namespace WPE.InjectMode
{
    public partial class PacketEditForm : Form
    {
        private InjectModeForm imForm;
        private PacketInfo piSelect;

        #region//窗体事件

        public PacketEditForm(InjectModeForm form, PacketInfo pi)
        {
            InitializeComponent();

            if (pi == null)
            {
                string Title = AntdUI.Localization.Get("PacketEditForm.LoadError", "加载封包数据出错");
                string Content = AntdUI.Localization.Get("InjectModeForm.CheckSystemLog", "请检查系统日志");
                AntdUI.Notification.error(form, Title, Content, AntdUI.TAlignFrom.TR);
                this.Close();
            }
            else
            {
                this.piSelect = pi;
                this.imForm = form;
            }
        }

        private void PacketEditForm_Load(object sender, EventArgs e)
        {
            this.Text = AntdUI.Localization.Get("PacketEditForm", "编辑封包");

            this.hbPacketData.ContextMenuStrip = new System.Windows.Forms.ContextMenuStrip();
            this.nudPacketSocket.Value = this.piSelect.PacketSocket;
            this.txtIPTo.Text = this.piSelect.PacketTo;            

            DynamicByteProvider dbp = new DynamicByteProvider(this.piSelect.PacketBuffer);
            dbp.Changed += new EventHandler(ByteProvider_Changed);
            dbp.LengthChanged += new EventHandler(ByteProvider_LengthChanged);
            hbPacketData.ByteProvider = dbp;

            DefaultByteCharConverter defConverter = new DefaultByteCharConverter();
            EbcdicByteCharProvider ebcdicConverter = new EbcdicByteCharProvider();

            this.ddlEncoding.Items.Clear();
            this.ddlEncoding.Items.Add(defConverter);
            this.ddlEncoding.Items.Add(ebcdicConverter);
            this.ddlEncoding.SelectedIndex = 0;            

            this.HexBox_LinePositionChanged();
            this.HexBox_UpdatePacketLen();
            this.HexBox_ManageAbility();
            this.ProgressionPosition_Change();
        }

        #endregion        

        #region//发送类型

        private void rbSendType_Times_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.SendType_Changed();
        }

        private void SendType_Changed()
        {
            this.nudSendType_Times.Enabled = this.rbSendType_Times.Checked;
        }

        #endregion

        #region//递进

        private void cbProgressionCarry_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.ProgressionCarry_Change();
        }

        private void ProgressionCarry_Change()
        {
            this.nudProgressionCarry.Enabled = this.cbProgressionCarry.Checked;
        }

        private void cbProgressionPosition_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.ProgressionPosition_Change();
        }

        private void ProgressionPosition_Change()
        {
            this.cbProgressionCarry.Enabled =
                this.nudProgressionPosition.Enabled =
                this.nudProgressionStep.Enabled =
                this.nudProgressionCarry.Enabled =
                this.cbProgressionPosition.Checked;

            if (this.cbProgressionPosition.Checked)
            {
                this.ProgressionCarry_Change();
            }
        }

        #endregion

        #region//编辑器事件

        private void ByteProvider_Changed(object sender, EventArgs e)
        {
            this.HexBox_ManageAbility();
        }

        private void ByteProvider_LengthChanged(object sender, EventArgs e)
        {
            this.HexBox_UpdatePacketLen();
        }

        private void HexBox_ManageAbility()
        {
            try
            {
                if (hbPacketData.ByteProvider == null)
                {
                    this.bSave.Enabled = false;
                    //tsPacketData_Find.Enabled = false;
                    //tsPacketData_FindNext.Enabled = false;
                    ddlEncoding.Enabled = false;
                }
                else
                {
                    this.bSave.Enabled = true;
                    //tsPacketData_Find.Enabled = true;
                    //tsPacketData_FindNext.Enabled = true;
                    ddlEncoding.Enabled = true;
                }

                HexBox_ManageAbilityForCopyAndPaste();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void HexBox_UpdatePacketLen()
        {
            this.nudPacketLength.Value = this.hbPacketData.ByteProvider.Length;
        }

        private void HexBox_ManageAbilityForCopyAndPaste()
        {
            //tsPacketData_Copy.Enabled = hbPacketData.CanCopy();
            //tsPacketData_Cut.Enabled = hbPacketData.CanCut();
            //tsPacketData_Paste.Enabled = hbPacketData.CanPaste();
            //tsPacketData_Paste_PasteHex.Enabled = hbPacketData.CanPasteHex();
        }

        private void HexBox_LinePositionChanged()
        {
            try
            {
                int iSelectIndex = (int)hbPacketData.SelectionStart;
                this.lHexBox_Position.Text = string.Format(AntdUI.Localization.Get("PacketEditForm.Position", "[ 行 {0}  列 {1}  位置 {2} ]"), hbPacketData.CurrentLine, hbPacketData.CurrentPositionInLine, iSelectIndex);

                if (hbPacketData.ByteProvider != null && hbPacketData.ByteProvider.Length > hbPacketData.SelectionStart)
                {
                    byte bSelected = hbPacketData.ByteProvider.ReadByte(hbPacketData.SelectionStart);

                    BitInfo bitInfo = new BitInfo(bSelected, hbPacketData.SelectionStart);

                    if (bitInfo != null)
                    {
                        long start = hbPacketData.SelectionStart;
                        long selected = hbPacketData.SelectionLength;

                        if (selected == 0 || selected > 8)
                        {
                            selected = 8;
                        }

                        long last = hbPacketData.ByteProvider.Length;
                        long end = Math.Min(start + selected, last);

                        byte[] buffer64 = new byte[8];
                        int iBuffIndex = 0;

                        for (long i = start; i < end; i++)
                        {
                            buffer64[iBuffIndex] = hbPacketData.ByteProvider.ReadByte(i);
                            iBuffIndex++;
                        }
                        
                        this.lBits_Value.Text = bitInfo.ToString();
                        this.lChar_Value.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Char, buffer64);
                        this.lByte_Value.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Byte, buffer64);
                        this.lShort_Value.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Short, buffer64);
                        this.lUShort_Value.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UShort, buffer64);
                        this.lInt32_Value.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Int32, buffer64);
                        this.lUInt32_Value.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UInt32, buffer64);
                        this.lInt64_Value.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Int64, buffer64);
                        this.lUInt64_Value.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.UInt64, buffer64);
                        this.lFloat_Value.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Float, buffer64);
                        this.lDouble_Value.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Double, buffer64);
                    }
                }                
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #region//编码

        private void ddlEncoding_SelectedIndexChanged(object sender, IntEventArgs e)
        {
            hbPacketData.ByteCharConverter = ddlEncoding.SelectedValue as IByteCharConverter;
            this.hbPacketData.Focus();
        }

        #endregion

        #endregion

        #region//右键菜单

        private void hbPacketData_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                AntdUI.ContextMenuStrip.open(new AntdUI.ContextMenuStrip.Config(hbPacketData, (item) =>
                {
                    switch (item.ID)
                    {
                        case "cmsTop":

                            

                            break;                        
                    }
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
            try
            {
                if (hbPacketData.ByteProvider != null)
                {
                    DynamicByteProvider dbp = hbPacketData.ByteProvider as DynamicByteProvider;
                    if (dbp != null)
                    {
                        dbp.ApplyChanges();
                        byte[] bNewBuff = dbp.Bytes.ToArray();

                        this.piSelect.PacketSocket = ((int)this.nudPacketSocket.Value);
                        this.piSelect.PacketBuffer = bNewBuff;
                        this.piSelect.PacketLen = bNewBuff.Length;
                        this.piSelect.PacketData = Operate.PacketConfig.Packet.GetPacketData_Hex(bNewBuff, Operate.PacketConfig.Packet.PacketData_MaxLen);

                        this.Close();
                    }
                    else
                    {
                        AntdUI.Message.open(new AntdUI.Message.Config(this, "封包数据为空", TType.Error)
                        {
                            LocalizationText = "PacketEditForm.Save.Empty"
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);

                AntdUI.Message.open(new AntdUI.Message.Config(this, "封包保存出错", TType.Error)
                {
                    LocalizationText = "PacketEditForm.Save.Error"
                });
            }
            finally
            {
                HexBox_ManageAbility();
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
