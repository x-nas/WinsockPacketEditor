using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class TableLayoutPanelEx : TableLayoutPanel
    {
        public TableLayoutPanelEx()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | 
                ControlStyles.AllPaintingInWmPaint | 
                ControlStyles.SupportsTransparentBackColor, true);

            UpdateStyles();
        }
    }
}
