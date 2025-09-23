using System;
using System.Drawing;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class RobotListForm : AntdUI.Window, InterfaceInfo.IRobotList
    {
        private RobotList cRobotList = null;

        public RobotListForm()
        {
            Operate.RobotConfig.List.IsRobotListFormShow = true;

            InitializeComponent();

            Theme()
                .Light(Color.White, Color.Black)
                .Dark(Color.Black, Color.White)
                .Call(isDark =>
                {
                    this.Dark_Changed();
                });
        }

        private void RobotListForm_Load(object sender, EventArgs e)
        {
            this.Text = AntdUI.Localization.Get("RobotList", "机器人列表");

            cRobotList = new RobotList(this);
            cRobotList.Dock = DockStyle.Fill;
            this.tlpRobotList.Controls.Add(cRobotList);

            this.Dark_Changed();
        }

        private void RobotListForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Operate.RobotConfig.List.IsRobotListFormShow = false;
        }

        public void RefreshRobotList()
        {
            this.cRobotList?.RefreshRobotList();
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

            this.cRobotList?.Dark_Changed();
        }
    }
}
