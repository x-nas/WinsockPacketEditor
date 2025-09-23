using AntdUI;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class FilterListForm : AntdUI.Window, InterfaceInfo.IFilterList
    {
        private FilterList cFilterList = null;

        public FilterListForm()
        {
            Operate.FilterConfig.List.IsFilterListFormShow = true;

            InitializeComponent();            

            Theme()
                .Light(Color.White, Color.Black)
                .Dark(Color.Black, Color.White)
                .Call(isDark => 
                {
                    this.Dark_Changed();
                });
        }

        private void FilterListForm_Load(object sender, EventArgs e)
        {
            this.Text = AntdUI.Localization.Get("FilterList", "滤镜列表");

            cFilterList = new FilterList(this);
            cFilterList.Dock = DockStyle.Fill;
            this.tlpFilterList.Controls.Add(cFilterList);

            this.Dark_Changed();
        }        

        private void FilterListForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Operate.FilterConfig.List.IsFilterListFormShow = false;
        }

        public void RefreshFilterList()
        {
            this.cFilterList?.RefreshFilterList();
        }

        private void Dark_Changed()
        {
            if (AntdUI.Config.IsDark)
            {
                BackColor = Operate.SystemConfig.Color_30;
                ForeColor = Color.White;
            }
            else
            {
                BackColor = Operate.SystemConfig.Color_250;
                ForeColor = Color.Black;
            }

            this.cFilterList?.Dark_Changed();
        }
    }
}
