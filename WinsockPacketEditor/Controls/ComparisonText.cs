using AntdUI;
using DiffPlex.DiffBuilder.Model;
using System;
using System.Collections.Generic;
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
            this.tabComparisonText.SelectTab(0);

            this.InitTable_Comparison();
            this.InitTable_Duplicate();
            this.Dark_Changed();
            this.SetTextInfo();
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
                }.SetFixed().SetLocalizationTitleID("Table.Comparison.Column.ID"),
                new AntdUI.Column("Position", "位置", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Comparison.Column."),
                new AntdUI.Column("ValueA", "A 值", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Comparison.Column."),
                new AntdUI.Column("ValueB", "B 值", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Comparison.Column."),
                new AntdUI.Column("ChangeType", "变更类型", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is Operate.SystemConfig.DifferenceItem di)
                        {
                            switch (di.ChangeType)
                            {
                                case ChangeType.Inserted: return new CellTag(AntdUI.Localization.Get("Inserted", "新增"), TTypeMini.Success);
                                case ChangeType.Deleted: return new CellTag(AntdUI.Localization.Get("Deleted", "删除"), TTypeMini.Error);
                                case ChangeType.Modified: return new CellTag(AntdUI.Localization.Get("Modified", "修改"), TTypeMini.Warn);
                                default: return new CellTag(AntdUI.Localization.Get("Same", "相同"), TTypeMini.Info);
                            }
                        }

                        return value;
                    },
                }.SetLocalizationTitleID("Table.Comparison.Column."),
                new AntdUI.Column("CellLinks", "操作")
                {
                    Render = (value, record, rowindex)=>
                    {
                        return new AntdUI.CellLink[]
                        {
                            new AntdUI.CellButton("bLocation", null, AntdUI.TTypeMini.Warn).SetIcon("EnvironmentOutlined"),                            
                        };
                    },
                }.SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.Comparison.Column."),
            };

            this.tComparison.ColumnFont = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
        }

        private void InitTable_Duplicate()
        {
            tDuplicate.Columns = new AntdUI.ColumnCollection
            {
                new AntdUI.Column("", "序号", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return (rowindex + 1);
                    },
                }.SetFixed().SetLocalizationTitleID("Table.Duplicate.Column.ID"),
                new AntdUI.Column("Sequence", "重复值").SetWidth("500").SetLineBreak(true).SetLocalizationTitleID("Table.Duplicate.Column."),
                new AntdUI.Column("Length", "长度", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Duplicate.Column."),
                new AntdUI.Column("CountInA", "A 次数", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Duplicate.Column."),                
                new AntdUI.Column("CountInB", "B 次数", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.Duplicate.Column."),
                new AntdUI.Column("PositionsInA", "A 位置")
                {
                    Render = (value, record, rowIndex) =>
                    {
                        if (value is List<int> intList)
                        {
                            return string.Join(", ", intList);
                        }
                        return value;
                    }
                }.SetLocalizationTitleID("Table.Duplicate.Column."),
                new AntdUI.Column("PositionsInB", "B 位置")
                {
                    Render = (value, record, rowIndex) =>
                    {
                        if (value is List<int> intList)
                        {
                            return string.Join(", ", intList);
                        }
                        return value;
                    }
                }.SetLocalizationTitleID("Table.Duplicate.Column."),
            };

            this.tDuplicate.ColumnFont = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
        }

        public void SetTextInfo()
        {
            this.lComparison_A.Text = string.Format(AntdUI.Localization.Get("ComparisonText.TextA", "文本 A  ( 长度 {0} )"), this.txtComparison_A.Text.Length);
            this.lComparison_B.Text = string.Format(AntdUI.Localization.Get("ComparisonText.TextB", "文本 B  ( 长度 {0} )"), this.txtComparison_B.Text.Length);
            this.lDuplicate_A.Text = string.Format(AntdUI.Localization.Get("ComparisonText.TextA", "文本 A  ( 长度 {0} )"), this.txtDuplicate_A.Text.Length);
            this.lDuplicate_B.Text = string.Format(AntdUI.Localization.Get("ComparisonText.TextB", "文本 B  ( 长度 {0} )"), this.txtDuplicate_B.Text.Length);
        }

        public void Dark_Changed()
        {
            if (AntdUI.Config.IsDark)
            {
                this.txtComparison_A.BackColor = 
                    this.txtComparison_B.BackColor = 
                    this.txtDuplicate_A.BackColor = 
                    this.txtDuplicate_B.BackColor =
                    Operate.SystemConfig.Color_40;

                this.tComparison.BackColor = Operate.SystemConfig.Color_40;
                this.tComparison.ColumnBack = Operate.SystemConfig.Color_40;

                this.tDuplicate.BackColor = Operate.SystemConfig.Color_40;
                this.tDuplicate.ColumnBack = Operate.SystemConfig.Color_40;
            }
            else
            {
                this.txtComparison_A.BackColor =
                    this.txtComparison_B.BackColor =
                    this.txtDuplicate_A.BackColor =
                    this.txtDuplicate_B.BackColor = null;

                this.tComparison.BackColor = Color.White;
                this.tComparison.ColumnBack = null;

                this.tDuplicate.BackColor = Color.White;
                this.tDuplicate.ColumnBack = null;
            }
        }

        public void SetTextA(string StringA)
        { 
            this.TextA = StringA;
            this.txtComparison_A.Text = this.TextA;
            this.txtDuplicate_A.Text = this.TextA;
        }

        public void SetTextB(string StringB)
        {
            this.TextB = StringB;
            this.txtComparison_B.Text = this.TextB;
            this.txtDuplicate_B.Text = this.TextB;
        }

        private void txtComparison_A_TextChanged(object sender, EventArgs e)
        {
            this.lComparison_A.Text = string.Format(AntdUI.Localization.Get("ComparisonText.TextA", "文本 A  ( 长度 {0} )"), this.txtComparison_A.Text.Length);
        }

        private void txtComparison_B_TextChanged(object sender, EventArgs e)
        {
            this.lComparison_B.Text = string.Format(AntdUI.Localization.Get("ComparisonText.TextB", "文本 B  ( 长度 {0} )"), this.txtComparison_B.Text.Length);
        }

        private void txtDuplicate_A_TextChanged(object sender, EventArgs e)
        {
            this.lDuplicate_A.Text = string.Format(AntdUI.Localization.Get("ComparisonText.TextA", "文本 A  ( 长度 {0} )"), this.txtDuplicate_A.Text.Length);
        }

        private void txtDuplicate_B_TextChanged(object sender, EventArgs e)
        {
            this.lDuplicate_B.Text = string.Format(AntdUI.Localization.Get("ComparisonText.TextB", "文本 B  ( 长度 {0} )"), this.txtDuplicate_B.Text.Length);
        }

        private void tComparison_CellButtonClick(object sender, TableButtonEventArgs e)
        {
            if (e.Record is Operate.SystemConfig.DifferenceItem di)
            {
                switch (e.Btn.Id)
                {
                    case "bLocation":

                        this.ScrollToPosition(di.Position, di.Position);

                        break;
                }
            }
        }

        private void tComparison_CellDoubleClick(object sender, TableClickEventArgs e)
        {
            if (e.Record is Operate.SystemConfig.DifferenceItem di)
            {
                this.ScrollToPosition(di.Position, di.Position);
            }
        }

        #endregion        

        #region //文本比较

        private void txtComparisonRegex_TextChanged(object sender, EventArgs e)
        {
            Operate.SystemConfig.FindRegexMatches(this.txtComparisonRegex.Text, this.txtComparison_A, this.txtComparison_B);
        }

        private void bComparisonRegex_Click(object sender, EventArgs e)
        {
            Operate.SystemConfig.LeachRegexMatches(this.txtComparisonRegex.Text, this.txtComparison_A, this.txtComparison_B);
        }

        private void bComparison_Click(object sender, EventArgs e)
        {
            try
            {
                List<Operate.SystemConfig.DifferenceItem> diResult = null;

                this.txtComparison_A.ClearStyle();
                this.txtComparison_B.ClearStyle();

                AntdUI.Spin.open(this, new AntdUI.Spin.Config()
                {
                    Radius = 6,
                    Font = new Font("Microsoft YaHei UI", 12f),
                }, (config) =>
                {
                    config.Text = AntdUI.Localization.Get("Loading", "正在加载...");
                    diResult = Operate.SystemConfig.CompareText(this.txtComparison_A, this.txtComparison_B);

                }, () =>
                {
                    this.tComparison.DataSource = diResult;
                });
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(bComparison_Click), ex.Message);
            }            
        }

        #endregion

        #region//文本查重

        private void txtRegex_TextChanged(object sender, EventArgs e)
        {
            Operate.SystemConfig.FindRegexMatches(this.txtDuplicateRegex.Text, this.txtDuplicate_A, this.txtDuplicate_B);
        }

        private void bDuplicateRegex_Click(object sender, EventArgs e)
        {
            Operate.SystemConfig.LeachRegexMatches(this.txtDuplicateRegex.Text, this.txtDuplicate_A, this.txtDuplicate_B);
        }

        private void bDuplicate_Click(object sender, EventArgs e)
        {
            try
            {
                string StringA = this.txtDuplicate_A.Text.Trim();
                string StringB = this.txtDuplicate_B.Text.Trim();
                int minBytes = (int)nudDuplicate.Value;

                (string TextA, string TextB, List<Operate.SystemConfig.DuplicateInfo> Duplicates) diResult = (TextA: "", TextB: "", Duplicates: new List<Operate.SystemConfig.DuplicateInfo>());

                AntdUI.Spin.open(this, new AntdUI.Spin.Config()
                {
                    Radius = 6,
                    Font = new Font("Microsoft YaHei UI", 12f),
                }, (config) =>
                {
                    config.Text = AntdUI.Localization.Get("Loading", "正在加载...");
                    diResult = Operate.SystemConfig.ComparePackets(StringA, StringB, minBytes);

                }, () =>
                {
                    this.txtDuplicate_A.Text = Operate.SystemConfig.FormatHex(diResult.TextA);
                    this.txtDuplicate_B.Text = Operate.SystemConfig.FormatHex(diResult.TextB);
                    this.tDuplicate.DataSource = diResult.Duplicates;
                });
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(bDuplicate_Click), ex.Message);
            }            
        }

        #endregion

        #region//跳转到指定位置

        private void ScrollToPosition(int PositionA, int PositionB)
        {
            this.txtComparison_A.SelectionStart = PositionA;
            this.txtComparison_A.ScrollToCaret();

            this.txtComparison_B.SelectionStart = PositionB;
            this.txtComparison_B.ScrollToCaret();
        }

        #endregion

        #region//清除

        private void bComparison_Clear_Click(object sender, EventArgs e)
        {
            this.txtComparison_A.ClearStyle();
            this.txtComparison_B.ClearStyle();
            this.txtComparison_A.Clear();
            this.txtComparison_B.Clear();
            this.txtComparisonRegex.Clear();
        }

        private void bDuplicate_Clear_Click(object sender, EventArgs e)
        {
            this.txtDuplicate_A.ClearStyle();
            this.txtDuplicate_B.ClearStyle();
            this.txtDuplicate_A.Clear();
            this.txtDuplicate_B.Clear();
            this.txtDuplicateRegex.Clear();
        }

        #endregion

        #region//暂存文本

        private void bStore_Click(object sender, EventArgs e)
        {
            if (this.tabComparisonText.SelectedIndex == 0)
            {
                this.TextA = this.txtComparison_A.Text;
                this.TextB = this.txtComparison_B.Text;
            }
            else if (this.tabComparisonText.SelectedIndex == 1)
            {
                this.TextA = this.txtDuplicate_A.Text;
                this.TextB = this.txtDuplicate_B.Text;
            }
        }

        #endregion

        #region//还原

        private void bReset_Click(object sender, EventArgs e)
        {
            if (this.tabComparisonText.SelectedIndex == 0)
            {
                this.txtComparison_A.Text = this.TextA;
                this.txtComparison_B.Text = this.TextB;
            }
            else if (this.tabComparisonText.SelectedIndex == 1)
            {
                this.txtDuplicate_A.Text = this.TextA;
                this.txtDuplicate_B.Text = this.TextB;
            }
        }

        #endregion
    }
}
