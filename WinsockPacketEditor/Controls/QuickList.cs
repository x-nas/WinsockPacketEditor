using AntdUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class QuickList : UserControl
    {
        private Form form = null;
        public QuickList(Form form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void QuickList_Load(object sender, EventArgs e)
        {
            this.InitFilterList();
        }

        private void InitFilterList()
        {
            tFilterList.Columns = new AntdUI.ColumnCollection {
                new AntdUI.ColumnSwitch("IsEnable", "启用", AntdUI.ColumnAlign.Center)
                {
                    Call = (value, record, i_row, i_col) =>
                    {
                        return value;
                    }
                }.SetFixed().SetWidth("Auto").SetLocalizationTitleID("Table.FilterList.Column."),
                new AntdUI.Column("FName", "滤镜名称").SetLocalizationTitleID("Table.FilterList.Column."),
            };

            this.tFilterList.Binding(Operate.FilterConfig.List.lstFilterInfo);
        }
    }
}
