namespace WinsockPacketEditor
{
    partial class XORCalculation
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
            this.tlpXOR = new TableLayoutPanelEx();
            this.tlpPacketInfo_XOR_Button = new TableLayoutPanelEx();
            this.lXOR = new AntdUI.Label();
            this.bXOR = new AntdUI.Button();
            this.bXOR_ClearUp = new AntdUI.Button();
            this.txtXOR = new AntdUI.Input();
            this.pXOR_To = new AntdUI.Panel();
            this.hbXOR_To = new Be.Windows.Forms.HexBox();
            this.pXOR_From = new AntdUI.Panel();
            this.hbXOR_From = new Be.Windows.Forms.HexBox();
            this.tlpXOR.SuspendLayout();
            this.tlpPacketInfo_XOR_Button.SuspendLayout();
            this.pXOR_To.SuspendLayout();
            this.pXOR_From.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpXOR
            // 
            this.tlpXOR.ColumnCount = 3;
            this.tlpXOR.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpXOR.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpXOR.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpXOR.Controls.Add(this.tlpPacketInfo_XOR_Button, 1, 2);
            this.tlpXOR.Controls.Add(this.pXOR_To, 1, 3);
            this.tlpXOR.Controls.Add(this.pXOR_From, 1, 1);
            this.tlpXOR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpXOR.Location = new System.Drawing.Point(0, 0);
            this.tlpXOR.Margin = new System.Windows.Forms.Padding(0);
            this.tlpXOR.Name = "tlpXOR";
            this.tlpXOR.RowCount = 4;
            this.tlpXOR.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpXOR.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpXOR.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpXOR.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpXOR.Size = new System.Drawing.Size(800, 800);
            this.tlpXOR.TabIndex = 2;
            // 
            // tlpPacketInfo_XOR_Button
            // 
            this.tlpPacketInfo_XOR_Button.ColumnCount = 4;
            this.tlpPacketInfo_XOR_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPacketInfo_XOR_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketInfo_XOR_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPacketInfo_XOR_Button.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPacketInfo_XOR_Button.Controls.Add(this.lXOR, 0, 1);
            this.tlpPacketInfo_XOR_Button.Controls.Add(this.bXOR, 2, 1);
            this.tlpPacketInfo_XOR_Button.Controls.Add(this.bXOR_ClearUp, 3, 1);
            this.tlpPacketInfo_XOR_Button.Controls.Add(this.txtXOR, 1, 1);
            this.tlpPacketInfo_XOR_Button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPacketInfo_XOR_Button.Location = new System.Drawing.Point(30, 380);
            this.tlpPacketInfo_XOR_Button.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPacketInfo_XOR_Button.Name = "tlpPacketInfo_XOR_Button";
            this.tlpPacketInfo_XOR_Button.RowCount = 3;
            this.tlpPacketInfo_XOR_Button.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpPacketInfo_XOR_Button.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpPacketInfo_XOR_Button.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpPacketInfo_XOR_Button.Size = new System.Drawing.Size(740, 60);
            this.tlpPacketInfo_XOR_Button.TabIndex = 2;
            // 
            // lXOR
            // 
            this.lXOR.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lXOR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lXOR.LocalizationText = "XORCalculation.XORValue";
            this.lXOR.Location = new System.Drawing.Point(3, 7);
            this.lXOR.Name = "lXOR";
            this.lXOR.Size = new System.Drawing.Size(176, 46);
            this.lXOR.TabIndex = 6;
            this.lXOR.Text = "异或值（支持循环异或）";
            // 
            // bXOR
            // 
            this.bXOR.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bXOR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bXOR.IconSvg = "BuildFilled";
            this.bXOR.LocalizationText = "XOR";
            this.bXOR.Location = new System.Drawing.Point(557, 7);
            this.bXOR.Name = "bXOR";
            this.bXOR.Size = new System.Drawing.Size(87, 46);
            this.bXOR.TabIndex = 7;
            this.bXOR.Text = "计算";
            this.bXOR.Type = AntdUI.TTypeMini.Primary;
            this.bXOR.Click += new System.EventHandler(this.bXOR_Click);
            // 
            // bXOR_ClearUp
            // 
            this.bXOR_ClearUp.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bXOR_ClearUp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bXOR_ClearUp.IconSvg = "DeleteOutlined";
            this.bXOR_ClearUp.LocalizationText = "Clear";
            this.bXOR_ClearUp.Location = new System.Drawing.Point(650, 7);
            this.bXOR_ClearUp.Name = "bXOR_ClearUp";
            this.bXOR_ClearUp.Size = new System.Drawing.Size(87, 46);
            this.bXOR_ClearUp.TabIndex = 8;
            this.bXOR_ClearUp.Text = "清空";
            this.bXOR_ClearUp.Type = AntdUI.TTypeMini.Warn;
            this.bXOR_ClearUp.Click += new System.EventHandler(this.bXOR_ClearUp_Click);
            // 
            // txtXOR
            // 
            this.txtXOR.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtXOR.LocalizationPlaceholderText = "HexWithSpaces";
            this.txtXOR.Location = new System.Drawing.Point(185, 7);
            this.txtXOR.Name = "txtXOR";
            this.txtXOR.PlaceholderText = "请输入十六进制带空格";
            this.txtXOR.Size = new System.Drawing.Size(366, 46);
            this.txtXOR.TabIndex = 9;
            this.txtXOR.TextChanged += new System.EventHandler(this.txtXOR_TextChanged);
            // 
            // pXOR_To
            // 
            this.pXOR_To.BorderWidth = 1F;
            this.pXOR_To.Controls.Add(this.hbXOR_To);
            this.pXOR_To.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pXOR_To.Location = new System.Drawing.Point(33, 443);
            this.pXOR_To.Name = "pXOR_To";
            this.pXOR_To.Padding = new System.Windows.Forms.Padding(3);
            this.pXOR_To.Size = new System.Drawing.Size(734, 354);
            this.pXOR_To.TabIndex = 1;
            this.pXOR_To.Text = "panel2";
            // 
            // hbXOR_To
            // 
            this.hbXOR_To.BorderStyle = System.Windows.Forms.BorderStyle.None;
            // 
            // 
            // 
            this.hbXOR_To.BuiltInContextMenu.CopyMenuItemText = "复制";
            this.hbXOR_To.BuiltInContextMenu.CutMenuItemText = "剪切";
            this.hbXOR_To.BuiltInContextMenu.PasteMenuItemText = "粘贴";
            this.hbXOR_To.BuiltInContextMenu.SelectAllMenuItemText = "全选";
            this.hbXOR_To.ColumnInfoVisible = true;
            this.hbXOR_To.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hbXOR_To.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hbXOR_To.LineInfoVisible = true;
            this.hbXOR_To.Location = new System.Drawing.Point(4, 4);
            this.hbXOR_To.Name = "hbXOR_To";
            this.hbXOR_To.ReadOnly = true;
            this.hbXOR_To.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hbXOR_To.Size = new System.Drawing.Size(726, 346);
            this.hbXOR_To.TabIndex = 3;
            this.hbXOR_To.VScrollBarVisible = true;
            this.hbXOR_To.MouseDown += new System.Windows.Forms.MouseEventHandler(this.hbXOR_To_MouseDown);
            // 
            // pXOR_From
            // 
            this.pXOR_From.BorderWidth = 1F;
            this.pXOR_From.Controls.Add(this.hbXOR_From);
            this.pXOR_From.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pXOR_From.Location = new System.Drawing.Point(33, 23);
            this.pXOR_From.Name = "pXOR_From";
            this.pXOR_From.Padding = new System.Windows.Forms.Padding(3);
            this.pXOR_From.Size = new System.Drawing.Size(734, 354);
            this.pXOR_From.TabIndex = 0;
            this.pXOR_From.Text = "panel1";
            // 
            // hbXOR_From
            // 
            this.hbXOR_From.BorderStyle = System.Windows.Forms.BorderStyle.None;
            // 
            // 
            // 
            this.hbXOR_From.BuiltInContextMenu.CopyMenuItemText = "复制";
            this.hbXOR_From.BuiltInContextMenu.CutMenuItemText = "剪切";
            this.hbXOR_From.BuiltInContextMenu.PasteMenuItemText = "粘贴";
            this.hbXOR_From.BuiltInContextMenu.SelectAllMenuItemText = "全选";
            this.hbXOR_From.ColumnInfoVisible = true;
            this.hbXOR_From.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hbXOR_From.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hbXOR_From.LineInfoVisible = true;
            this.hbXOR_From.Location = new System.Drawing.Point(4, 4);
            this.hbXOR_From.Name = "hbXOR_From";
            this.hbXOR_From.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hbXOR_From.Size = new System.Drawing.Size(726, 346);
            this.hbXOR_From.TabIndex = 2;
            this.hbXOR_From.VScrollBarVisible = true;
            this.hbXOR_From.MouseDown += new System.Windows.Forms.MouseEventHandler(this.hbXOR_From_MouseDown);
            // 
            // XORCalculation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpXOR);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "XORCalculation";
            this.Size = new System.Drawing.Size(800, 800);
            this.Load += new System.EventHandler(this.XORCalculation_Load);
            this.tlpXOR.ResumeLayout(false);
            this.tlpPacketInfo_XOR_Button.ResumeLayout(false);
            this.tlpPacketInfo_XOR_Button.PerformLayout();
            this.pXOR_To.ResumeLayout(false);
            this.pXOR_From.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpXOR;
        private TableLayoutPanelEx tlpPacketInfo_XOR_Button;
        private AntdUI.Label lXOR;
        private AntdUI.Button bXOR;
        private AntdUI.Button bXOR_ClearUp;
        private AntdUI.Input txtXOR;
        private AntdUI.Panel pXOR_To;
        private Be.Windows.Forms.HexBox hbXOR_To;
        private AntdUI.Panel pXOR_From;
        private Be.Windows.Forms.HexBox hbXOR_From;
    }
}
