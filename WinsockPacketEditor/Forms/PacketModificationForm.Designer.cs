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
            this.splitterModification = new AntdUI.Splitter();
            this.tlpPacketData = new System.Windows.Forms.TableLayoutPanel();
            this.txtModification_Result = new AntdUI.Input();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bExit = new AntdUI.Button();
            this.lPacketData_Raw = new AntdUI.Label();
            this.lPacketData_New = new AntdUI.Label();
            this.pPacketData_Raw = new AntdUI.Panel();
            this.hbPacketData_Raw = new Be.Windows.Forms.HexBox();
            this.pPacketData_New = new AntdUI.Panel();
            this.hbPacketData_New = new Be.Windows.Forms.HexBox();
            this.tlpModification.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitterModification)).BeginInit();
            this.splitterModification.Panel1.SuspendLayout();
            this.splitterModification.Panel2.SuspendLayout();
            this.splitterModification.SuspendLayout();
            this.tlpPacketData.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.pPacketData_Raw.SuspendLayout();
            this.pPacketData_New.SuspendLayout();
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
            this.splitterModification.Panel2.Controls.Add(this.txtModification_Result);
            this.splitterModification.Panel2MinSize = 0;
            this.splitterModification.Size = new System.Drawing.Size(978, 695);
            this.splitterModification.SplitterDistance = 500;
            this.splitterModification.SplitterSize = 80;
            this.splitterModification.SplitterWidth = 10;
            this.splitterModification.TabIndex = 2;
            // 
            // tlpPacketData
            // 
            this.tlpPacketData.ColumnCount = 2;
            this.tlpPacketData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpPacketData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpPacketData.Controls.Add(this.lPacketData_New, 1, 0);
            this.tlpPacketData.Controls.Add(this.lPacketData_Raw, 0, 0);
            this.tlpPacketData.Controls.Add(this.pPacketData_Raw, 0, 1);
            this.tlpPacketData.Controls.Add(this.pPacketData_New, 1, 1);
            this.tlpPacketData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPacketData.Location = new System.Drawing.Point(0, 0);
            this.tlpPacketData.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPacketData.Name = "tlpPacketData";
            this.tlpPacketData.RowCount = 2;
            this.tlpPacketData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpPacketData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketData.Size = new System.Drawing.Size(978, 500);
            this.tlpPacketData.TabIndex = 0;
            // 
            // txtModification_Result
            // 
            this.txtModification_Result.AutoScroll = true;
            this.txtModification_Result.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtModification_Result.Location = new System.Drawing.Point(0, 0);
            this.txtModification_Result.Multiline = true;
            this.txtModification_Result.Name = "txtModification_Result";
            this.txtModification_Result.ReadOnly = true;
            this.txtModification_Result.Size = new System.Drawing.Size(978, 185);
            this.txtModification_Result.TabIndex = 1;
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
            this.bExit.Location = new System.Drawing.Point(435, 7);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(114, 46);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // lPacketData_Raw
            // 
            this.lPacketData_Raw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lPacketData_Raw.Location = new System.Drawing.Point(3, 3);
            this.lPacketData_Raw.Name = "lPacketData_Raw";
            this.lPacketData_Raw.Size = new System.Drawing.Size(483, 40);
            this.lPacketData_Raw.TabIndex = 3;
            this.lPacketData_Raw.Text = "原始封包数据";
            this.lPacketData_Raw.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lPacketData_New
            // 
            this.lPacketData_New.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lPacketData_New.Location = new System.Drawing.Point(492, 3);
            this.lPacketData_New.Name = "lPacketData_New";
            this.lPacketData_New.Size = new System.Drawing.Size(483, 40);
            this.lPacketData_New.TabIndex = 4;
            this.lPacketData_New.Text = "修改后封包数据";
            this.lPacketData_New.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pPacketData_Raw
            // 
            this.pPacketData_Raw.BorderWidth = 1F;
            this.pPacketData_Raw.Controls.Add(this.hbPacketData_Raw);
            this.pPacketData_Raw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pPacketData_Raw.Location = new System.Drawing.Point(3, 49);
            this.pPacketData_Raw.Name = "pPacketData_Raw";
            this.pPacketData_Raw.Padding = new System.Windows.Forms.Padding(3);
            this.pPacketData_Raw.Size = new System.Drawing.Size(483, 448);
            this.pPacketData_Raw.TabIndex = 5;
            this.pPacketData_Raw.Text = "panel1";
            // 
            // hbPacketData_Raw
            // 
            this.hbPacketData_Raw.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.hbPacketData_Raw.ColumnInfoVisible = true;
            this.hbPacketData_Raw.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hbPacketData_Raw.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hbPacketData_Raw.LineInfoVisible = true;
            this.hbPacketData_Raw.Location = new System.Drawing.Point(4, 4);
            this.hbPacketData_Raw.Name = "hbPacketData_Raw";
            this.hbPacketData_Raw.ReadOnly = true;
            this.hbPacketData_Raw.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hbPacketData_Raw.Size = new System.Drawing.Size(475, 440);
            this.hbPacketData_Raw.StringViewVisible = true;
            this.hbPacketData_Raw.TabIndex = 2;
            this.hbPacketData_Raw.VScrollBarVisible = true;
            // 
            // pPacketData_New
            // 
            this.pPacketData_New.BorderWidth = 1F;
            this.pPacketData_New.Controls.Add(this.hbPacketData_New);
            this.pPacketData_New.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pPacketData_New.Location = new System.Drawing.Point(492, 49);
            this.pPacketData_New.Name = "pPacketData_New";
            this.pPacketData_New.Padding = new System.Windows.Forms.Padding(3);
            this.pPacketData_New.Size = new System.Drawing.Size(483, 448);
            this.pPacketData_New.TabIndex = 6;
            this.pPacketData_New.Text = "panel1";
            // 
            // hbPacketData_New
            // 
            this.hbPacketData_New.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.hbPacketData_New.ColumnInfoVisible = true;
            this.hbPacketData_New.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hbPacketData_New.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hbPacketData_New.LineInfoVisible = true;
            this.hbPacketData_New.Location = new System.Drawing.Point(4, 4);
            this.hbPacketData_New.Name = "hbPacketData_New";
            this.hbPacketData_New.ReadOnly = true;
            this.hbPacketData_New.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hbPacketData_New.Size = new System.Drawing.Size(475, 440);
            this.hbPacketData_New.StringViewVisible = true;
            this.hbPacketData_New.TabIndex = 3;
            this.hbPacketData_New.VScrollBarVisible = true;
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
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "PacketModificationForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PacketModificationForm";
            this.Load += new System.EventHandler(this.PacketModificationForm_Load);
            this.tlpModification.ResumeLayout(false);
            this.splitterModification.Panel1.ResumeLayout(false);
            this.splitterModification.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitterModification)).EndInit();
            this.splitterModification.ResumeLayout(false);
            this.tlpPacketData.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.pPacketData_Raw.ResumeLayout(false);
            this.pPacketData_New.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpModification;
        private AntdUI.Splitter splitterModification;
        private System.Windows.Forms.TableLayoutPanel tlpPacketData;
        private AntdUI.Input txtModification_Result;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private AntdUI.Button bExit;
        private AntdUI.Label lPacketData_New;
        private AntdUI.Label lPacketData_Raw;
        private AntdUI.Panel pPacketData_Raw;
        private Be.Windows.Forms.HexBox hbPacketData_Raw;
        private AntdUI.Panel pPacketData_New;
        private Be.Windows.Forms.HexBox hbPacketData_New;
    }
}