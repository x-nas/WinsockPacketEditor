using AntdUI;
using DiffPlex.DiffBuilder.Model;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class ComparisonText : UserControl
    {
        public string TextA = string.Empty;
        public string TextB = string.Empty;

        #region//窗体事件

        public ComparisonText()
        {
            InitializeComponent();
        }

        private void ComparisonText_Load(object sender, EventArgs e)
        {
            this.InitComparison();
        }        

        private void InitComparison()
        {
            this.ddlComparisonType.Items.Clear();

            this.ddlComparisonType.Items.AddRange(new AntdUI.SelectItem[]
            {
                    new AntdUI.SelectItem("文本比较")
                    {
                        LocalizationText = "",
                    },
                    new AntdUI.SelectItem("文本查重")
                    {
                        LocalizationText = "",
                    },
            });

            this.ddlComparisonType.SelectedIndex = 0;
            this.ComparisonType_Changed();

            this.Comparison_A_Changed();
            this.Comparison_B_Changed();
        }

        private void txtComparison_A_TextChanged(object sender, EventArgs e)
        {
            this.Comparison_A_Changed();
        }

        private void Comparison_A_Changed()
        {
            string StringA = this.txtComparison_A.Text.Trim();
            if (string.IsNullOrEmpty(StringA))
            {
                this.txtComparison_A.Status = TType.Error;
            }
            else
            {
                this.txtComparison_A.Status = TType.Success;
            }

            this.lComparison_A.Text = string.Format(AntdUI.Localization.Get("System.TextA", "文本 A  ( 长度 {0} )"), StringA.Length);
        }

        private void txtComparison_B_TextChanged(object sender, EventArgs e)
        {
            this.Comparison_B_Changed();
        }

        private void Comparison_B_Changed()
        {
            string StringB = this.txtComparison_B.Text.Trim();
            if (string.IsNullOrEmpty(StringB))
            {
                this.txtComparison_B.Status = TType.Error;
            }
            else
            {
                this.txtComparison_B.Status = TType.Success;
            }

            this.lComparison_B.Text = string.Format(AntdUI.Localization.Get("System.TextB", "文本 B  ( 长度 {0} )"), StringB.Length);
        }

        private void ddlComparisonType_SelectedIndexChanged(object sender, IntEventArgs e)
        {
            this.ComparisonType_Changed();
        }

        private void ComparisonType_Changed()
        {
            if (this.ddlComparisonType.SelectedIndex == 0)
            {
                this.nudComparison_DuplicateNum.Enabled = false;
            }
            else if (this.ddlComparisonType.SelectedIndex == 1)
            {
                this.nudComparison_DuplicateNum.Enabled = true;
            }
        }

        public void SetTextA(string StringA)
        { 
            this.TextA = StringA;
            this.txtComparison_A.Text = this.TextA;
        }

        public void SetTextB(string StringB)
        {
            this.TextB = StringB;
            this.txtComparison_B.Text = this.TextB;
        }

        #endregion

        #region//分析文本

        private void bComparison_Click(object sender, EventArgs e)
        {
            if (this.ddlComparisonType.SelectedIndex == 0)
            {
                HighlightCharacterDifferences(this.txtComparison_A, this.txtComparison_B);
            }
            else
            {
                // 文本查重
            }
        }

        private void HighlightCharacterDifferences(AntdUI.Input box1, AntdUI.Input box2)
        {
            try
            {
                ResetStyles();

                string text1 = box1.Text;
                string text2 = box2.Text;

                int maxLength = Math.Max(text1.Length, text2.Length);
                text1 = text1.PadRight(maxLength, ' ');
                text2 = text2.PadRight(maxLength, ' ');

                for (int i = 0; i < maxLength; i++)
                {
                    ChangeType changeType = GetCharDiffType(text1, text2, i);

                    // 处理第一个文本框(input2) - 原始文本
                    if (i < text1.Length)
                    {
                        if (changeType == ChangeType.Deleted || changeType == ChangeType.Modified)
                        {
                            box1.SetStyle(i, 1,
                                        font: null,
                                        fore: Color.White,
                                        back: Color.FromArgb(220, 80, 80)); // 红色背景表示删除/修改
                        }
                    }

                    // 处理第二个文本框(input3) - 新文本
                    if (i < text2.Length)
                    {
                        if (changeType == ChangeType.Inserted || changeType == ChangeType.Modified)
                        {
                            box2.SetStyle(i, 1,
                                        font: null,
                                        fore: Color.White,
                                        back: Color.FromArgb(80, 180, 80)); // 绿色背景表示新增/修改
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        private static ChangeType GetCharDiffType(string str1, string str2, int position)
        {
            if (position >= str1.Length) return ChangeType.Inserted;
            if (position >= str2.Length) return ChangeType.Deleted;
            return str1[position] == str2[position] ? ChangeType.Unchanged : ChangeType.Modified;
        }

        private void ResetStyles()
        {
            this.txtComparison_A.ClearStyle();
            this.txtComparison_B.ClearStyle();
        }

        #endregion

        #region//还原

        private void bComparison_Reset_Click(object sender, EventArgs e)
        {
            ResetStyles();

            this.txtComparison_A.Text = this.TextA;
            this.txtComparison_B.Text = this.TextB;
        }

        #endregion

        #region//交换

        private void bComparison_Change_Click(object sender, EventArgs e)
        {
            ResetStyles();

            string sTextA = this.txtComparison_A.Text.Trim();
            string sTextB = this.txtComparison_B.Text.Trim();

            this.txtComparison_A.Text = sTextB;
            this.txtComparison_B.Text = sTextA;
        }

        #endregion

        #region//清空

        private void bComparison_Clean_Click(object sender, EventArgs e)
        {
            ResetStyles();

            this.txtComparison_A.Clear();
            this.txtComparison_B.Clear();
        }

        #endregion        
    }
}
