namespace WinsockPacketEditor
{
    partial class FireWallSetting
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
            this.tlpFireWallSetting = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tableLayoutPanelEx1 = new WinsockPacketEditor.TableLayoutPanelEx();
            this.lBlackList = new AntdUI.Label();
            this.bBlackList = new AntdUI.Button();
            this.tlpWhiteList = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bWhiteList = new AntdUI.Button();
            this.lWhiteList = new AntdUI.Label();
            this.tBlackList = new AntdUI.Table();
            this.tWhiteList = new AntdUI.Table();
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tlpEnableFireWall = new WinsockPacketEditor.TableLayoutPanelEx();
            this.rbBlackListMode = new AntdUI.Radio();
            this.rbWhiteListMode = new AntdUI.Radio();
            this.cbEnableFireWall = new AntdUI.Checkbox();
            this.tlpFireWallSetting.SuspendLayout();
            this.tableLayoutPanelEx1.SuspendLayout();
            this.tlpWhiteList.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpEnableFireWall.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpFireWallSetting
            // 
            this.tlpFireWallSetting.ColumnCount = 1;
            this.tlpFireWallSetting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFireWallSetting.Controls.Add(this.tableLayoutPanelEx1, 0, 3);
            this.tlpFireWallSetting.Controls.Add(this.tlpWhiteList, 0, 1);
            this.tlpFireWallSetting.Controls.Add(this.tBlackList, 0, 4);
            this.tlpFireWallSetting.Controls.Add(this.tWhiteList, 0, 2);
            this.tlpFireWallSetting.Controls.Add(this.tlpButton, 0, 5);
            this.tlpFireWallSetting.Controls.Add(this.tlpEnableFireWall, 0, 0);
            this.tlpFireWallSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFireWallSetting.Location = new System.Drawing.Point(0, 0);
            this.tlpFireWallSetting.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFireWallSetting.Name = "tlpFireWallSetting";
            this.tlpFireWallSetting.RowCount = 6;
            this.tlpFireWallSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpFireWallSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpFireWallSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpFireWallSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpFireWallSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpFireWallSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpFireWallSetting.Size = new System.Drawing.Size(700, 700);
            this.tlpFireWallSetting.TabIndex = 0;
            // 
            // tableLayoutPanelEx1
            // 
            this.tableLayoutPanelEx1.ColumnCount = 3;
            this.tableLayoutPanelEx1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEx1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelEx1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanelEx1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelEx1.Controls.Add(this.lBlackList, 0, 1);
            this.tableLayoutPanelEx1.Controls.Add(this.bBlackList, 2, 1);
            this.tableLayoutPanelEx1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelEx1.Location = new System.Drawing.Point(0, 350);
            this.tableLayoutPanelEx1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanelEx1.Name = "tableLayoutPanelEx1";
            this.tableLayoutPanelEx1.RowCount = 3;
            this.tableLayoutPanelEx1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelEx1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelEx1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanelEx1.Size = new System.Drawing.Size(700, 50);
            this.tableLayoutPanelEx1.TabIndex = 15;
            // 
            // lBlackList
            // 
            this.lBlackList.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lBlackList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lBlackList.Location = new System.Drawing.Point(3, 9);
            this.lBlackList.Name = "lBlackList";
            this.lBlackList.Size = new System.Drawing.Size(60, 32);
            this.lBlackList.TabIndex = 15;
            this.lBlackList.Text = "黑名单列表";
            // 
            // bBlackList
            // 
            this.bBlackList.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bBlackList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bBlackList.Ghost = true;
            this.bBlackList.IconRatio = 1F;
            this.bBlackList.IconSvg = "PlusOutlined";
            this.bBlackList.Location = new System.Drawing.Point(665, 9);
            this.bBlackList.Name = "bBlackList";
            this.bBlackList.Size = new System.Drawing.Size(32, 32);
            this.bBlackList.TabIndex = 13;
            this.bBlackList.WaveSize = 0;
            // 
            // tlpWhiteList
            // 
            this.tlpWhiteList.ColumnCount = 3;
            this.tlpWhiteList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpWhiteList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpWhiteList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpWhiteList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpWhiteList.Controls.Add(this.bWhiteList, 2, 1);
            this.tlpWhiteList.Controls.Add(this.lWhiteList, 0, 1);
            this.tlpWhiteList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpWhiteList.Location = new System.Drawing.Point(0, 50);
            this.tlpWhiteList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpWhiteList.Name = "tlpWhiteList";
            this.tlpWhiteList.RowCount = 3;
            this.tlpWhiteList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpWhiteList.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpWhiteList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpWhiteList.Size = new System.Drawing.Size(700, 50);
            this.tlpWhiteList.TabIndex = 13;
            // 
            // bWhiteList
            // 
            this.bWhiteList.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bWhiteList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bWhiteList.Ghost = true;
            this.bWhiteList.IconRatio = 1F;
            this.bWhiteList.IconSvg = "PlusOutlined";
            this.bWhiteList.Location = new System.Drawing.Point(665, 9);
            this.bWhiteList.Name = "bWhiteList";
            this.bWhiteList.Size = new System.Drawing.Size(32, 32);
            this.bWhiteList.TabIndex = 13;
            this.bWhiteList.WaveSize = 0;
            // 
            // lWhiteList
            // 
            this.lWhiteList.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lWhiteList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lWhiteList.Location = new System.Drawing.Point(3, 9);
            this.lWhiteList.Name = "lWhiteList";
            this.lWhiteList.Size = new System.Drawing.Size(60, 32);
            this.lWhiteList.TabIndex = 14;
            this.lWhiteList.Text = "白名单列表";
            // 
            // tBlackList
            // 
            this.tBlackList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tBlackList.Bordered = true;
            this.tBlackList.CellImpactHeight = false;
            this.tBlackList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tBlackList.EmptyHeader = true;
            this.tBlackList.Gap = 8;
            this.tBlackList.GapCell = 5;
            this.tBlackList.Gaps = new System.Drawing.Size(8, 8);
            this.tBlackList.Location = new System.Drawing.Point(2, 402);
            this.tBlackList.Margin = new System.Windows.Forms.Padding(2);
            this.tBlackList.Name = "tBlackList";
            this.tBlackList.Size = new System.Drawing.Size(696, 246);
            this.tBlackList.TabIndex = 10;
            // 
            // tWhiteList
            // 
            this.tWhiteList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tWhiteList.Bordered = true;
            this.tWhiteList.CellImpactHeight = false;
            this.tWhiteList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tWhiteList.EmptyHeader = true;
            this.tWhiteList.Gap = 8;
            this.tWhiteList.GapCell = 5;
            this.tWhiteList.Gaps = new System.Drawing.Size(8, 8);
            this.tWhiteList.Location = new System.Drawing.Point(2, 102);
            this.tWhiteList.Margin = new System.Windows.Forms.Padding(2);
            this.tWhiteList.Name = "tWhiteList";
            this.tWhiteList.Size = new System.Drawing.Size(696, 246);
            this.tWhiteList.TabIndex = 9;
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 5;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Controls.Add(this.bSave, 1, 1);
            this.tlpButton.Controls.Add(this.bExit, 3, 1);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 650);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(700, 50);
            this.tlpButton.TabIndex = 4;
            // 
            // bSave
            // 
            this.bSave.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.LocalizationText = "Save";
            this.bSave.Location = new System.Drawing.Point(278, 6);
            this.bSave.Margin = new System.Windows.Forms.Padding(2);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(63, 37);
            this.bSave.TabIndex = 0;
            this.bSave.Text = "保存";
            this.bSave.Type = AntdUI.TTypeMini.Primary;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bExit
            // 
            this.bExit.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bExit.BackExtend = "135, #6253E1, #04BEFE";
            this.bExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bExit.IconSvg = "LogoutOutlined";
            this.bExit.LocalizationText = "Cancel";
            this.bExit.Location = new System.Drawing.Point(359, 6);
            this.bExit.Margin = new System.Windows.Forms.Padding(2);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(63, 37);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // tlpEnableFireWall
            // 
            this.tlpEnableFireWall.ColumnCount = 4;
            this.tlpEnableFireWall.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpEnableFireWall.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpEnableFireWall.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpEnableFireWall.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpEnableFireWall.Controls.Add(this.rbBlackListMode, 2, 1);
            this.tlpEnableFireWall.Controls.Add(this.rbWhiteListMode, 1, 1);
            this.tlpEnableFireWall.Controls.Add(this.cbEnableFireWall, 0, 1);
            this.tlpEnableFireWall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpEnableFireWall.Location = new System.Drawing.Point(0, 0);
            this.tlpEnableFireWall.Margin = new System.Windows.Forms.Padding(0);
            this.tlpEnableFireWall.Name = "tlpEnableFireWall";
            this.tlpEnableFireWall.RowCount = 3;
            this.tlpEnableFireWall.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpEnableFireWall.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpEnableFireWall.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpEnableFireWall.Size = new System.Drawing.Size(700, 50);
            this.tlpEnableFireWall.TabIndex = 16;
            // 
            // rbBlackListMode
            // 
            this.rbBlackListMode.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbBlackListMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbBlackListMode.Location = new System.Drawing.Point(211, 9);
            this.rbBlackListMode.Name = "rbBlackListMode";
            this.rbBlackListMode.Size = new System.Drawing.Size(92, 32);
            this.rbBlackListMode.TabIndex = 17;
            this.rbBlackListMode.Text = "黑名单模式";
            // 
            // rbWhiteListMode
            // 
            this.rbWhiteListMode.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbWhiteListMode.Checked = true;
            this.rbWhiteListMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbWhiteListMode.Location = new System.Drawing.Point(113, 9);
            this.rbWhiteListMode.Name = "rbWhiteListMode";
            this.rbWhiteListMode.Size = new System.Drawing.Size(92, 32);
            this.rbWhiteListMode.TabIndex = 16;
            this.rbWhiteListMode.Text = "白名单模式";
            // 
            // cbEnableFireWall
            // 
            this.cbEnableFireWall.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbEnableFireWall.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbEnableFireWall.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbEnableFireWall.Location = new System.Drawing.Point(3, 9);
            this.cbEnableFireWall.Name = "cbEnableFireWall";
            this.cbEnableFireWall.Size = new System.Drawing.Size(104, 32);
            this.cbEnableFireWall.TabIndex = 15;
            this.cbEnableFireWall.Text = "启用连接控制";
            this.cbEnableFireWall.CheckedChanged += new AntdUI.BoolEventHandler(this.cbEnableFireWall_CheckedChanged);
            // 
            // FireWallSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpFireWallSetting);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FireWallSetting";
            this.Size = new System.Drawing.Size(700, 700);
            this.Load += new System.EventHandler(this.FireWallSetting_Load);
            this.tlpFireWallSetting.ResumeLayout(false);
            this.tableLayoutPanelEx1.ResumeLayout(false);
            this.tableLayoutPanelEx1.PerformLayout();
            this.tlpWhiteList.ResumeLayout(false);
            this.tlpWhiteList.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.tlpEnableFireWall.ResumeLayout(false);
            this.tlpEnableFireWall.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpFireWallSetting;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private AntdUI.Table tBlackList;
        private AntdUI.Table tWhiteList;
        private TableLayoutPanelEx tlpWhiteList;
        private AntdUI.Button bWhiteList;
        private TableLayoutPanelEx tableLayoutPanelEx1;
        private AntdUI.Button bBlackList;
        private TableLayoutPanelEx tlpEnableFireWall;
        private AntdUI.Radio rbBlackListMode;
        private AntdUI.Radio rbWhiteListMode;
        private AntdUI.Checkbox cbEnableFireWall;
        private AntdUI.Label lWhiteList;
        private AntdUI.Label lBlackList;
    }
}
