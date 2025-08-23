namespace WinsockPacketEditor
{
    partial class PacketModificationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PacketModificationForm));
            this.tlpModification = new System.Windows.Forms.TableLayoutPanel();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bExit = new AntdUI.Button();
            this.splitterModification = new AntdUI.Splitter();
            this.tlpPacketData = new System.Windows.Forms.TableLayoutPanel();
            this.txtPacketData_New = new AntdUI.Input();
            this.lPacketData_New = new AntdUI.Label();
            this.lPacketData_Raw = new AntdUI.Label();
            this.txtPacketData_Raw = new AntdUI.Input();
            this.tPacketModification = new AntdUI.Table();
            this.tlpModification.SuspendLayout();
            this.tlpButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitterModification)).BeginInit();
            this.splitterModification.Panel1.SuspendLayout();
            this.splitterModification.Panel2.SuspendLayout();
            this.splitterModification.SuspendLayout();
            this.tlpPacketData.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpModification
            // 
            this.tlpModification.ColumnCount = 1;
            this.tlpModification.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpModification.Controls.Add(this.tlpButton, 0, 1);
            this.tlpModification.Controls.Add(this.splitterModification, 0, 0);
            this.tlpModification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpModification.Location = new System.Drawing.Point(0, 0);
            this.tlpModification.Margin = new System.Windows.Forms.Padding(0);
            this.tlpModification.Name = "tlpModification";
            this.tlpModification.RowCount = 2;
            this.tlpModification.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpModification.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpModification.Size = new System.Drawing.Size(984, 761);
            this.tlpModification.TabIndex = 0;
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 3;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.Controls.Add(this.bExit, 1, 1);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 701);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(984, 60);
            this.tlpButton.TabIndex = 17;
            // 
            // bExit
            // 
            this.bExit.BackExtend = "135, #6253E1, #04BEFE";
            this.bExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bExit.IconSvg = "LogoutOutlined";
            this.bExit.LocalizationText = "Cancel";
            this.bExit.Location = new System.Drawing.Point(435, 7);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(114, 46);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // splitterModification
            // 
            this.splitterModification.CollapsePanel = AntdUI.Splitter.ADCollapsePanel.Panel1;
            this.splitterModification.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitterModification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitterModification.Location = new System.Drawing.Point(3, 3);
            this.splitterModification.Name = "splitterModification";
            this.splitterModification.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitterModification.Panel1
            // 
            this.splitterModification.Panel1.Controls.Add(this.tlpPacketData);
            this.splitterModification.Panel1MinSize = 0;
            // 
            // splitterModification.Panel2
            // 
            this.splitterModification.Panel2.Controls.Add(this.tPacketModification);
            this.splitterModification.Panel2MinSize = 0;
            this.splitterModification.Size = new System.Drawing.Size(978, 695);
            this.splitterModification.SplitterDistance = 400;
            this.splitterModification.SplitterSize = 80;
            this.splitterModification.SplitterWidth = 10;
            this.splitterModification.TabIndex = 2;
            // 
            // tlpPacketData
            // 
            this.tlpPacketData.ColumnCount = 2;
            this.tlpPacketData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpPacketData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpPacketData.Controls.Add(this.txtPacketData_New, 1, 1);
            this.tlpPacketData.Controls.Add(this.lPacketData_New, 1, 0);
            this.tlpPacketData.Controls.Add(this.lPacketData_Raw, 0, 0);
            this.tlpPacketData.Controls.Add(this.txtPacketData_Raw, 0, 1);
            this.tlpPacketData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPacketData.Location = new System.Drawing.Point(0, 0);
            this.tlpPacketData.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPacketData.Name = "tlpPacketData";
            this.tlpPacketData.RowCount = 2;
            this.tlpPacketData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpPacketData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketData.Size = new System.Drawing.Size(978, 400);
            this.tlpPacketData.TabIndex = 0;
            // 
            // txtPacketData_New
            // 
            this.txtPacketData_New.AutoScroll = true;
            this.txtPacketData_New.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPacketData_New.Location = new System.Drawing.Point(492, 49);
            this.txtPacketData_New.Multiline = true;
            this.txtPacketData_New.Name = "txtPacketData_New";
            this.txtPacketData_New.Size = new System.Drawing.Size(483, 348);
            this.txtPacketData_New.TabIndex = 6;
            // 
            // lPacketData_New
            // 
            this.lPacketData_New.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lPacketData_New.LocalizationText = "PacketModificationForm.Modified";
            this.lPacketData_New.Location = new System.Drawing.Point(492, 3);
            this.lPacketData_New.Name = "lPacketData_New";
            this.lPacketData_New.Size = new System.Drawing.Size(483, 40);
            this.lPacketData_New.TabIndex = 4;
            this.lPacketData_New.Text = "修改后封包数据";
            this.lPacketData_New.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lPacketData_Raw
            // 
            this.lPacketData_Raw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lPacketData_Raw.LocalizationText = "PacketModificationForm.Raw";
            this.lPacketData_Raw.Location = new System.Drawing.Point(3, 3);
            this.lPacketData_Raw.Name = "lPacketData_Raw";
            this.lPacketData_Raw.Size = new System.Drawing.Size(483, 40);
            this.lPacketData_Raw.TabIndex = 3;
            this.lPacketData_Raw.Text = "原始封包数据";
            this.lPacketData_Raw.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPacketData_Raw
            // 
            this.txtPacketData_Raw.AutoScroll = true;
            this.txtPacketData_Raw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPacketData_Raw.Location = new System.Drawing.Point(3, 49);
            this.txtPacketData_Raw.Multiline = true;
            this.txtPacketData_Raw.Name = "txtPacketData_Raw";
            this.txtPacketData_Raw.Size = new System.Drawing.Size(483, 348);
            this.txtPacketData_Raw.TabIndex = 5;
            // 
            // tPacketModification
            // 
            this.tPacketModification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tPacketModification.Gap = 12;
            this.tPacketModification.Location = new System.Drawing.Point(0, 0);
            this.tPacketModification.Name = "tPacketModification";
            this.tPacketModification.Size = new System.Drawing.Size(978, 285);
            this.tPacketModification.TabIndex = 0;
            this.tPacketModification.Text = "table1";
            // 
            // PacketModificationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 761);
            this.Controls.Add(this.tlpModification);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "PacketModificationForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PacketModificationForm";
            this.Load += new System.EventHandler(this.PacketModificationForm_Load);
            this.tlpModification.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.splitterModification.Panel1.ResumeLayout(false);
            this.splitterModification.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitterModification)).EndInit();
            this.splitterModification.ResumeLayout(false);
            this.tlpPacketData.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpModification;
        private AntdUI.Splitter splitterModification;
        private System.Windows.Forms.TableLayoutPanel tlpPacketData;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private AntdUI.Button bExit;
        private AntdUI.Label lPacketData_New;
        private AntdUI.Label lPacketData_Raw;
        private AntdUI.Input txtPacketData_Raw;
        private AntdUI.Input txtPacketData_New;
        private AntdUI.Table tPacketModification;
    }
}