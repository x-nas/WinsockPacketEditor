using AntdUI;
using DiffPlex.DiffBuilder.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class PacketModification : UserControl
    {
        private Form form;
        private PacketInfo packetInfo = null;
        private ProxyInfo proxyInfo = null;

        #region//窗体事件

        public PacketModification(Form form, PacketInfo packetInfo)
        {
            InitializeComponent();
            this.packetInfo = packetInfo;
            this.form = form;
        }

        public PacketModification(Form form, ProxyInfo proxyInfo)
        {
            InitializeComponent();
            this.proxyInfo = proxyInfo;
            this.form = form;
        }

        private void PacketModification_Load(object sender, EventArgs e)
        {
            try
            {
                switch (Operate.SystemConfig.StartMode)
                {
                    case Operate.SystemConfig.SystemMode.Process:                        

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

                this.InitTable_Comparison();
                this.Dark_Changed();
                this.SetPacketInfo();

                List<Operate.SystemConfig.DifferenceItem> diResult = null;

                AntdUI.Spin.open(this, new AntdUI.Spin.Config()
                {
                    Radius = 6,
                    Font = new Font("Microsoft YaHei UI", 12f),
                }, (config) =>
                {
                    config.Text = AntdUI.Localization.Get("Loading", "正在加载...");
                    diResult = Operate.SystemConfig.CompareText(this.txtPacketData_Raw, this.txtPacketData_New);

                }, () =>
                {
                    this.tPacketModification.DataSource = diResult;
                });                
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
                }.SetFixed().SetLocalizationTitleID("Table.ComparisonText.Column.ID"),
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
                                case ChangeType.Inserted: return new CellTag(AntdUI.Localization.Get("Inserted", "新增"), TTypeMini.Success);
                                case ChangeType.Deleted: return new CellTag(AntdUI.Localization.Get("Deleted", "删除"), TTypeMini.Error);
                                case ChangeType.Modified: return new CellTag(AntdUI.Localization.Get("Modified", "修改"), TTypeMini.Warn);
                                default: return new CellTag(AntdUI.Localization.Get("Same", "相同"), TTypeMini.Info);
                            }
                        }

                        return value;
                    },
                }.SetLocalizationTitleID("Table.ComparisonText.Column."),
            };

            this.tPacketModification.ColumnFont = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
        }

        private void Dark_Changed()
        {
            if (AntdUI.Config.IsDark)
            {
                this.txtPacketData_Raw.BackColor = 
                    this.txtPacketData_New.BackColor = 
                    Operate.SystemConfig.Color_30;
                
            }
            else
            {
                this.txtPacketData_Raw.BackColor =
                    this.txtPacketData_New.BackColor = null;
            }
        }

        private void SetPacketInfo()
        {
            string RawInfo = AntdUI.Localization.Get("PacketModificationForm.Raw", "原始封包数据 ( 长度 {0} )");
            string ModifiedInfo = AntdUI.Localization.Get("PacketModificationForm.Modified", "修改后封包数据 ( 长度 {0} )");

            switch (Operate.SystemConfig.StartMode)
            {
                case Operate.SystemConfig.SystemMode.Process:

                    this.lPacketData_Raw.Text = string.Format(RawInfo, this.packetInfo.RawBuffer.Length);
                    this.lPacketData_New.Text = string.Format(ModifiedInfo, this.packetInfo.PacketBuffer.Length);

                    break;

                case Operate.SystemConfig.SystemMode.Proxy:

                    this.lPacketData_Raw.Text = string.Format(RawInfo, this.proxyInfo.RawBuffer.Length);
                    this.lPacketData_New.Text = string.Format(ModifiedInfo, this.proxyInfo.PacketBuffer.Length);

                    break;
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
