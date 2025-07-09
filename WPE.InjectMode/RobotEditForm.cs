using System.Windows.Forms;
using WPE.Lib;

namespace WPE.InjectMode
{
    public partial class RobotEditForm : Form
    {
        private InjectModeForm imForm;
        private RobotInfo riSelect;
        private readonly RobotExecute re = new RobotExecute();

        public RobotEditForm(InjectModeForm form, RobotInfo ri)
        {
            InitializeComponent();

            if (ri == null)
            {
                string Title = AntdUI.Localization.Get("InjectModeForm.EditRobot.Error", "加载机器人数据出错");
                string Content = AntdUI.Localization.Get("InjectModeForm.CheckSystemLog", "请检查系统日志");
                AntdUI.Notification.error(form, Title, Content, AntdUI.TAlignFrom.TR);
                this.Close();
            }
            else
            {
                this.riSelect = ri;
                this.imForm = form;
            }
        }

        private void RobotEditForm_Load(object sender, System.EventArgs e)
        {
            this.Text = AntdUI.Localization.Get("RobotEditForm", "编辑机器人");            
        }

        private void RobotEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.re.StopRobot();
        }
    }
}
