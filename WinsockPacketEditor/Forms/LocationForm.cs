using AntdUI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class LocationForm : Form
    {
        private AccountInfo aiSelect;

        #region//窗体事件

        public LocationForm(Form form, AccountInfo ai)
        {
            InitializeComponent();
            this.aiSelect = ai;
        }

        private void LocationForm_Load(object sender, EventArgs e)
        {
            this.Text = AntdUI.Localization.Get("LocationForm", "登录情况");
            this.InitTable_Location();
        }

        #endregion

        #region//初始化表格

        private void InitTable_Location()
        {
            tLocation.Columns = new AntdUI.ColumnCollection {
                new AntdUI.Column("LoginTime", "登录时间").SetLocalizationTitleID("Table.Location.Column."),
                new AntdUI.Column("LoginIP", "登录IP")
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is AccountIPInfo aii)
                        {
                            return new CellText(value?.ToString() ?? string.Empty)
                            {
                                PrefixSvg = Operate.SystemConfig.GetSvgByLocation(aii.IPLocation),
                                IconRatio = 1.0F
                            };
                        }

                        return value;
                    },
                }.SetLocalizationTitleID("Table.Location.Column."),
                new AntdUI.Column("IPLocation", "所属地").SetLocalizationTitleID("Table.Location.Column."),
            };

            this.tLocation.ColumnFont = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.tLocation.Binding(aiSelect.AIPInfo);
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
