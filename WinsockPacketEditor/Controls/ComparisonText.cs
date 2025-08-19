using AntdUI;
using DiffPlex.DiffBuilder.Model;
using System;
using System.Drawing;
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
            this.InitTable_Comparison();
            this.InitComparison();
        }

        private void InitTable_Comparison()
        {
            tComparison.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("", "序号", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return (rowindex + 1);
                    },
                }.SetFixed().SetLocalizationTitleID("Table.ComparisonText.Column."),
                new AntdUI.Column("Position", "位置", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.ComparisonText.Column."),
                new AntdUI.Column("ValueA", "A值", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.ComparisonText.Column."),
                new AntdUI.Column("ValueB", "B值", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.ComparisonText.Column."),
                new AntdUI.Column("ChangeType", "变更类型", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is Operate.SystemConfig.DifferenceItem di)
                        {
                            switch (di.ChangeType)
                            {
                                case ChangeType.Inserted: return new CellTag("新增", TTypeMini.Success);
                                case ChangeType.Deleted: return new CellTag("删除", TTypeMini.Error);
                                case ChangeType.Modified: return new CellTag("修改", TTypeMini.Warn);
                                default: return new CellTag("相同", TTypeMini.Info);
                            }
                        }

                        return value;
                    },
                }.SetLocalizationTitleID("Table.ComparisonText.Column."),
            };

            this.tComparison.ColumnFont = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
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

        #region //分析文本

        private void bComparison_Click(object sender, EventArgs e)
        {
            this.ResetStyles();

            if (this.ddlComparisonType.SelectedIndex == 0)
            {
                this.tComparison.DataSource = Operate.SystemConfig.CompareText(this.txtComparison_A, this.txtComparison_B);
            }
            else
            {
                this.TextA = this.txtComparison_A.Text.Trim();
                this.TextB = this.txtComparison_B.Text.Trim();
                int minBytes = (int)nudComparison_DuplicateNum.Value;
                var results = Operate.SystemConfig.ComparePackets(this.TextA, this.TextB, minBytes);

                this.txtComparison_A.Text = Operate.SystemConfig.FormatHex(results.TextA);
                this.txtComparison_B.Text = Operate.SystemConfig.FormatHex(results.TextB);
            }
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
