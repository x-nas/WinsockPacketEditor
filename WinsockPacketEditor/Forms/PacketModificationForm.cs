using AntdUI;
using DiffPlex.DiffBuilder.Model;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class PacketModificationForm : Form
    {
        private Form form;
        private PacketInfo packetInfo = null;
        private ProxyInfo proxyInfo = null;

        #region//窗体事件

        public PacketModificationForm(Form form, PacketInfo packetInfo)
        {
            InitializeComponent();

            this.packetInfo = packetInfo;
            this.form = form;
        }

        public PacketModificationForm(Form form, ProxyInfo proxyInfo)
        {
            InitializeComponent();

            this.proxyInfo = proxyInfo;
            this.form = form;
        }

        private void PacketModificationForm_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = AntdUI.Localization.Get("PacketModificationForm", "封包修改");
                this.InitTable_Comparison();

                switch (Operate.SystemConfig.StartMode)
                {
                    case Operate.SystemConfig.SystemMode.Process:

                        this.lPacketData_Raw.Text = string.Format(AntdUI.Localization.Get("System.PacketDataRaw", "原始封包数据  ( 长度 {0} )"), this.packetInfo.RawBuffer.Length);
                        this.lPacketData_New.Text = string.Format(AntdUI.Localization.Get("System.PacketDataNew", "修改后封包数据  ( 长度 {0} )"), this.packetInfo.PacketBuffer.Length);

                        if (this.packetInfo.RawBuffer.Length > 0)
                        {
                            this.txtPacketData_Raw.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, this.packetInfo.RawBuffer);
                        }

                        if (this.packetInfo.PacketBuffer.Length > 0)
                        {
                            this.txtPacketData_New.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, this.packetInfo.PacketBuffer);
                        }

                        break;

                    case Operate.SystemConfig.SystemMode.Proxy:

                        this.lPacketData_Raw.Text = string.Format(AntdUI.Localization.Get("System.PacketDataRaw", "原始封包数据  ( 长度 {0} )"), this.proxyInfo.RawBuffer.Length);
                        this.lPacketData_New.Text = string.Format(AntdUI.Localization.Get("System.PacketDataNew", "修改后封包数据  ( 长度 {0} )"), this.proxyInfo.PacketBuffer.Length);

                        if (this.proxyInfo.RawBuffer.Length > 0)
                        {
                            this.txtPacketData_Raw.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, this.proxyInfo.RawBuffer);
                        }

                        if (this.proxyInfo.PacketBuffer.Length > 0)
                        {
                            this.txtPacketData_New.Text = Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, this.proxyInfo.PacketBuffer);
                        }

                        break;
                }

                this.tPacketModification.DataSource = Operate.SystemConfig.CompareText(this.txtPacketData_Raw, this.txtPacketData_New);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void InitTable_Comparison()
        {
            tPacketModification.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("", "序号", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return (rowindex + 1);
                    },
                }.SetFixed().SetLocalizationTitleID("Table.ComparisonText.Column."),
                new AntdUI.Column("Position", "位置", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.ComparisonText.Column."),
                new AntdUI.Column("ValueA", "原值", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.ComparisonText.Column."),
                new AntdUI.Column("ValueB", "新值", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.ComparisonText.Column."),
                new AntdUI.Column("ChangeType", "变更类型", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is Operate.SystemConfig.DifferenceItem di)
                        {
                            switch (di.ChangeType)
                            {
                                case ChangeType.Inserted: return new CellTag("新增", TTypeMini.Success);
                                case ChangeType.Deleted: return new CellTag("删除", TTypeMini.Error);
                                case ChangeType.Modified: return new CellTag("修改", TTypeMini.Warn);
                                default: return new CellTag("相同", TTypeMini.Info);
                            }
                        }

                        return value;
                    },
                }.SetLocalizationTitleID("Table.ComparisonText.Column."),
            };

            this.tPacketModification.ColumnFont = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
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
