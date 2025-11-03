namespace WinsockPacketEditor
{
    partial class PacketData
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
            AntdUI.Tabs.StyleLine styleLine1 = new AntdUI.Tabs.StyleLine();
            this.tabPacketData = new AntdUI.Tabs();
            this.tpSocket = new AntdUI.TabPage();
            this.hbPacketData = new Be.Windows.Forms.HexBox();
            this.tpHTTP = new AntdUI.TabPage();
            this.tlpHTTP = new WinsockPacketEditor.TableLayoutPanelEx();
            this.scintillaPacketData = new ScintillaNET.Scintilla();
            this.tabPacketData.SuspendLayout();
            this.tpSocket.SuspendLayout();
            this.tpHTTP.SuspendLayout();
            this.tlpHTTP.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPacketData
            // 
            this.tabPacketData.Controls.Add(this.tpHTTP);
            this.tabPacketData.Controls.Add(this.tpSocket);
            this.tabPacketData.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabPacketData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPacketData.Location = new System.Drawing.Point(0, 0);
            this.tabPacketData.Margin = new System.Windows.Forms.Padding(0);
            this.tabPacketData.Name = "tabPacketData";
            this.tabPacketData.Pages.Add(this.tpSocket);
            this.tabPacketData.Pages.Add(this.tpHTTP);
            this.tabPacketData.SelectedIndex = 1;
            this.tabPacketData.Size = new System.Drawing.Size(800, 300);
            this.tabPacketData.Style = styleLine1;
            this.tabPacketData.TabIndex = 0;
            this.tabPacketData.Text = "tabs1";
            // 
            // tpSocket
            // 
            this.tpSocket.Controls.Add(this.hbPacketData);
            this.tpSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpSocket.Location = new System.Drawing.Point(0, 24);
            this.tpSocket.Name = "tpSocket";
            this.tpSocket.Size = new System.Drawing.Size(800, 276);
            this.tpSocket.TabIndex = 0;
            this.tpSocket.Text = "tpSocket";
            // 
            // hbPacketData
            // 
            this.hbPacketData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.hbPacketData.ColumnInfoVisible = true;
            this.hbPacketData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hbPacketData.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hbPacketData.LineInfoVisible = true;
            this.hbPacketData.Location = new System.Drawing.Point(0, 0);
            this.hbPacketData.Margin = new System.Windows.Forms.Padding(0);
            this.hbPacketData.Name = "hbPacketData";
            this.hbPacketData.ReadOnly = true;
            this.hbPacketData.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hbPacketData.Size = new System.Drawing.Size(800, 276);
            this.hbPacketData.StringViewVisible = true;
            this.hbPacketData.TabIndex = 4;
            this.hbPacketData.VScrollBarVisible = true;
            this.hbPacketData.KeyDown += new System.Windows.Forms.KeyEventHandler(this.hbPacketData_KeyDown);
            this.hbPacketData.MouseDown += new System.Windows.Forms.MouseEventHandler(this.hbPacketData_MouseDown);
            // 
            // tpHTTP
            // 
            this.tpHTTP.Controls.Add(this.tlpHTTP);
            this.tpHTTP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpHTTP.Location = new System.Drawing.Point(0, 24);
            this.tpHTTP.Name = "tpHTTP";
            this.tpHTTP.Showed = true;
            this.tpHTTP.Size = new System.Drawing.Size(800, 276);
            this.tpHTTP.TabIndex = 1;
            this.tpHTTP.Text = "tpHTTP";
            // 
            // tlpHTTP
            // 
            this.tlpHTTP.ColumnCount = 1;
            this.tlpHTTP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpHTTP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpHTTP.Controls.Add(this.scintillaPacketData, 0, 0);
            this.tlpHTTP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpHTTP.Location = new System.Drawing.Point(0, 0);
            this.tlpHTTP.Margin = new System.Windows.Forms.Padding(0);
            this.tlpHTTP.Name = "tlpHTTP";
            this.tlpHTTP.RowCount = 1;
            this.tlpHTTP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpHTTP.Size = new System.Drawing.Size(800, 276);
            this.tlpHTTP.TabIndex = 0;
            // 
            // scintillaPacketData
            // 
            this.scintillaPacketData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.scintillaPacketData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scintillaPacketData.HScrollBar = false;
            this.scintillaPacketData.Location = new System.Drawing.Point(0, 0);
            this.scintillaPacketData.Margin = new System.Windows.Forms.Padding(0);
            this.scintillaPacketData.Name = "scintillaPacketData";
            this.scintillaPacketData.Size = new System.Drawing.Size(800, 276);
            this.scintillaPacketData.TabIndex = 0;
            // 
            // PacketData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabPacketData);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "PacketData";
            this.Size = new System.Drawing.Size(800, 300);
            this.Load += new System.EventHandler(this.PacketData_Load);
            this.tabPacketData.ResumeLayout(false);
            this.tpSocket.ResumeLayout(false);
            this.tpHTTP.ResumeLayout(false);
            this.tlpHTTP.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.Tabs tabPacketData;
        private AntdUI.TabPage tpSocket;
        private AntdUI.TabPage tpHTTP;
        private Be.Windows.Forms.HexBox hbPacketData;
        private TableLayoutPanelEx tlpHTTP;
        private ScintillaNET.Scintilla scintillaPacketData;
    }
}
