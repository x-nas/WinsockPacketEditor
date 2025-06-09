using System;
using System.Windows.Forms;
using WPELibrary.Lib;

namespace WPELibrary.TextComparison
{
    public partial class TextCompareForm : Form
    {
        #region//窗体事件

        public TextCompareForm(string TextA, string TextB)
        {
            InitializeComponent();

            this.rtbA.Text = TextA;
            this.rtbB.Text = TextB;            

            this.TextCompare();
        }

        private void TextCompareForm_Load(object sender, EventArgs e)
        {
            Socket_Cache.System.IsShow_TextCompare = true;
        }

        private void TextCompareForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Socket_Cache.System.IsShow_TextCompare = false;
        }

        private void rtbA_TextChanged(object sender, EventArgs e)
        {
            this.lA.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_33), rtbA.Text.Length);
        }

        private void rtbB_TextChanged(object sender, EventArgs e)
        {
            this.lB.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_34), rtbB.Text.Length);
        }

        #endregion

        #region//清空

        private void bClear_Click(object sender, EventArgs e)
        {
            this.rtbA.Clear();
            this.rtbB.Clear();
            this.rtbResult.Clear();
        }

        #endregion

        #region//交换

        private void bExchange_Click(object sender, EventArgs e)
        {
            string sText_A = this.rtbA.Text;
            string sText_B = this.rtbB.Text;

            this.rtbA.Text = sText_B;
            this.rtbB.Text = sText_A;
        }

        #endregion

        #region//对比（异步）

        private void bCompare_Click(object sender, EventArgs e)
        {
            this.TextCompare();
        }

        private async void TextCompare()
        {
            string TextA = this.rtbA.Text.Trim();
            string TextB = this.rtbB.Text.Trim();

            this.lA.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_33), TextA.Length);
            this.lB.Text = string.Format(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_34), TextB.Length);

            if (!string.IsNullOrEmpty(TextA) || !string.IsNullOrEmpty(TextB))
            {
                this.rtbResult.Clear();
                this.rtbResult.Text = MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_155);
                this.rtbResult.Rtf = await Socket_Operation.CompareData(this.Font, TextA, TextB);
            }            
        }

        #endregion        
    }
}
