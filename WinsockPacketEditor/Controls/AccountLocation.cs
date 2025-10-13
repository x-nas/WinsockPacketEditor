using AntdUI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class AccountLocation : UserControl
    {
        private Form form;
        private AccountInfo aiSelect;

        #region//窗体事件

        public AccountLocation(Form form, AccountInfo ai)
        {
            InitializeComponent();
            this.aiSelect = ai;
            this.form = form;
        }

        private void AccountLocation_Load(object sender, EventArgs e)
        {
            this.InitTable_Location();
        }

        #endregion

        #region//初始化表格

        private void InitTable_Location()
        {
            try
            {
                tLocation.Columns = new AntdUI.ColumnCollection 
                {
                    new AntdUI.Column("LoginTime", "登录时间").SetLocalizationTitleID("Table.Location.Column."),
                    new AntdUI.Column("LoginIP", "登录IP")
                    {
                        Render = (value, record, rowindex)=>
                        {
                            if(record is AccountIPInfo aii)
                            {
                                return new CellText(value?.ToString() ?? string.Empty)
                                {
                                    Prefix = Operate.SystemConfig.GetFlagByLocation(aii.IPLocation),
                                    IconRatio = 1.0F
                                };
                            }

                            return value;
                        },
                    }.SetLocalizationTitleID("Table.Location.Column."),
                    new AntdUI.Column("IPLocation", "所属地").SetLocalizationTitleID("Table.Location.Column."),
                };

                this.tLocation.ColumnFont = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
                this.tLocation.Binding(aiSelect.AIPInfo);
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(InitTable_Location), ex.Message);
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
