using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class WareHouseEdit : UserControl
    {
        private Form form;
        private WareHouseInfo whiSelect;
        private BindingList<DataInfo> Stores;

        public WareHouseEdit(Form form, WareHouseInfo whiSelect)
        {
            InitializeComponent();
            this.form = form;
            this.whiSelect = whiSelect;
        }

        private void WareHouseEdit_Load(object sender, System.EventArgs e)
        {
            this.txtWName.Text = this.whiSelect.WName;
            this.Stores = whiSelect.Stores;

            this.InitTable_Stores();
        }

        private void InitTable_Stores()
        {
            tStores.Columns = new AntdUI.ColumnCollection {
                new AntdUI.Column("", "序号", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return (rowindex + 1);
                    },
                }.SetFixed().SetLocalizationTitleID("Table.PacketList.Column.ID"),
                new AntdUI.Column("PacketBuffer", "长度", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return ((byte[])value).Length;
                    },
                }.SetLocalizationTitleID("Table.PacketList.Column."),
                new AntdUI.Column("PacketBuffer", "数据")
                {
                    Render = (value, record, rowindex)=>
                    {
                        return Operate.SystemConfig.BytesToString(Operate.PacketConfig.Packet.EncodingFormat.Hex, (byte[])value);
                    },
                }.SetLocalizationTitleID("Table.PacketList.Column."),
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

            this.tStores.ColumnFont = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.tStores.Binding(this.Stores);
        }

        #region//保存

        private void bSave_Click(object sender, System.EventArgs e)
        {
            this.Dispose();
        }

        #endregion

        #region//退出

        private void bExit_Click(object sender, System.EventArgs e)
        {
            this.Dispose();
        }

        #endregion
    }
}
