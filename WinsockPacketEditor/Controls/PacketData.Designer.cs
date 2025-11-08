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
            this.tpText = new AntdUI.TabPage();
            this.tpHex = new AntdUI.TabPage();
            this.hbPacketData = new Be.Windows.Forms.HexBox();
            this.txtText = new AntdUI.Input();
            this.tabPacketData.SuspendLayout();
            this.tpText.SuspendLayout();
            this.tpHex.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPacketData
            // 
            this.tabPacketData.Controls.Add(this.tpText);
            this.tabPacketData.Controls.Add(this.tpHex);
            this.tabPacketData.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tabPacketData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPacketData.Location = new System.Drawing.Point(0, 0);
            this.tabPacketData.Margin = new System.Windows.Forms.Padding(0);
            this.tabPacketData.Name = "tabPacketData";
            this.tabPacketData.Pages.Add(this.tpHex);
            this.tabPacketData.Pages.Add(this.tpText);
            this.tabPacketData.SelectedIndex = 1;
            this.tabPacketData.Size = new System.Drawing.Size(800, 300);
            this.tabPacketData.Style = styleLine1;
            this.tabPacketData.TabIndex = 0;
            this.tabPacketData.Text = "tabs1";
            // 
            // tpText
            // 
            this.tpText.Controls.Add(this.txtText);
            this.tpText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpText.Location = new System.Drawing.Point(0, 24);
            this.tpText.Name = "tpText";
            this.tpText.Showed = true;
            this.tpText.Size = new System.Drawing.Size(800, 276);
            this.tpText.TabIndex = 1;
            this.tpText.Text = "tpText";
            // 
            // tpHex
            // 
            this.tpHex.Controls.Add(this.hbPacketData);
            this.tpHex.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpHex.Location = new System.Drawing.Point(0, 24);
            this.tpHex.Name = "tpHex";
            this.tpHex.Size = new System.Drawing.Size(800, 276);
            this.tpHex.TabIndex = 0;
            this.tpHex.Text = "tpHex";
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
            // txtText
            // 
            this.txtText.AutoScroll = true;
            this.txtText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtText.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtText.Location = new System.Drawing.Point(0, 0);
            this.txtText.Margin = new System.Windows.Forms.Padding(0);
            this.txtText.Multiline = true;
            this.txtText.Name = "txtText";
            this.txtText.Radius = 0;
            this.txtText.ReadOnly = true;
            this.txtText.Size = new System.Drawing.Size(800, 276);
            this.txtText.TabIndex = 1;
            this.txtText.WaveSize = 0;
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
            this.tpText.ResumeLayout(false);
            this.tpHex.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.Tabs tabPacketData;
        private AntdUI.TabPage tpHex;
        private AntdUI.TabPage tpText;
        private Be.Windows.Forms.HexBox hbPacketData;
        private AntdUI.Input txtText;
    }
}
