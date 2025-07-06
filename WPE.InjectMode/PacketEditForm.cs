using System;
using System.Windows.Forms;

namespace WPE.InjectMode
{
    public partial class PacketEditForm : Form
    {
        public PacketEditForm()
        {
            InitializeComponent();
        }

        private void bExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }
    }
}
