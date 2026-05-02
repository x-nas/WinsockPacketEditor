namespace WinsockPacketEditor
{
    partial class RuleList
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tlpRuleList = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bSure = new AntdUI.Button();
            this.tRuleList = new AntdUI.Table();
            this.tlpTop = new WinsockPacketEditor.TableLayoutPanelEx();
            this.ddMenu = new AntdUI.Dropdown();
            this.tlpRuleList.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRuleList
            // 
            this.tlpRuleList.ColumnCount = 1;
            this.tlpRuleList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRuleList.Controls.Add(this.tlpButton, 0, 2);
            this.tlpRuleList.Controls.Add(this.tRuleList, 0, 1);
            this.tlpRuleList.Controls.Add(this.tlpTop, 0, 0);
            this.tlpRuleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRuleList.Location = new System.Drawing.Point(0, 0);
            this.tlpRuleList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpRuleList.Name = "tlpRuleList";
            this.tlpRuleList.RowCount = 3;
            this.tlpRuleList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpRuleList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRuleList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpRuleList.Size = new System.Drawing.Size(800, 500);
            this.tlpRuleList.TabIndex = 1;
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 3;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.Controls.Add(this.bSure, 1, 1);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 450);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(800, 50);
            this.tlpButton.TabIndex = 18;
            // 
            // bSure
            // 
            this.bSure.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSure.BackExtend = "135, #6253E1, #04BEFE";
            this.bSure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSure.IconSvg = "SaveOutlined";
            this.bSure.LocalizationText = "Save";
            this.bSure.Location = new System.Drawing.Point(715, 6);
            this.bSure.Margin = new System.Windows.Forms.Padding(2);
            this.bSure.Name = "bSure";
            this.bSure.Size = new System.Drawing.Size(63, 37);
            this.bSure.TabIndex = 0;
            this.bSure.Text = "保存";
            this.bSure.Type = AntdUI.TTypeMini.Primary;
            this.bSure.Click += new System.EventHandler(this.bSure_Click);
            // 
            // tRuleList
            // 
            this.tRuleList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tRuleList.CellImpactHeight = false;
            this.tRuleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tRuleList.Gap = 10;
            this.tRuleList.GapCell = 5;
            this.tRuleList.Gaps = new System.Drawing.Size(10, 10);
            this.tRuleList.Location = new System.Drawing.Point(2, 42);
            this.tRuleList.Margin = new System.Windows.Forms.Padding(2);
            this.tRuleList.MultipleRows = true;
            this.tRuleList.Name = "tRuleList";
            this.tRuleList.Size = new System.Drawing.Size(796, 406);
            this.tRuleList.SwitchSize = 12;
            this.tRuleList.TabIndex = 6;
            this.tRuleList.CellClick += new AntdUI.Table.ClickEventHandler(this.tRuleList_CellClick);
            this.tRuleList.CellButtonClick += new AntdUI.Table.ClickButtonEventHandler(this.tRuleList_CellButtonClick);
            this.tRuleList.CellDoubleClick += new AntdUI.Table.ClickEventHandler(this.tRuleList_CellDoubleClick);
            // 
            // tlpTop
            // 
            this.tlpTop.ColumnCount = 2;
            this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpTop.Controls.Add(this.ddMenu, 1, 0);
            this.tlpTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTop.Location = new System.Drawing.Point(0, 0);
            this.tlpTop.Margin = new System.Windows.Forms.Padding(0);
            this.tlpTop.Name = "tlpTop";
            this.tlpTop.RowCount = 1;
            this.tlpTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTop.Size = new System.Drawing.Size(800, 40);
            this.tlpTop.TabIndex = 5;
            // 
            // ddMenu
            // 
            this.ddMenu.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.ddMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddMenu.Ghost = true;
            this.ddMenu.IconRatio = 1F;
            this.ddMenu.IconSvg = "PlusOutlined";
            this.ddMenu.Location = new System.Drawing.Point(766, 2);
            this.ddMenu.Margin = new System.Windows.Forms.Padding(2);
            this.ddMenu.MaxCount = 10;
            this.ddMenu.Name = "ddMenu";
            this.ddMenu.Placement = AntdUI.TAlignFrom.BR;
            this.ddMenu.Size = new System.Drawing.Size(32, 36);
            this.ddMenu.TabIndex = 12;
            this.ddMenu.Trigger = AntdUI.Trigger.Hover;
            this.ddMenu.WaveSize = 0;
            this.ddMenu.SelectedValueChanged += new AntdUI.ObjectNEventHandler(this.ddMenu_SelectedValueChanged);
            // 
            // RuleList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpRuleList);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "RuleList";
            this.Size = new System.Drawing.Size(800, 500);
            this.Load += new System.EventHandler(this.RuleList_Load);
            this.tlpRuleList.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.tlpTop.ResumeLayout(false);
            this.tlpTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpRuleList;
        private AntdUI.Table tRuleList;
        private TableLayoutPanelEx tlpTop;
        private AntdUI.Dropdown ddMenu;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bSure;
    }
}
