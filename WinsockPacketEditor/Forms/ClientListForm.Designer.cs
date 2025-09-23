namespace WinsockPacketEditor
{
    partial class ClientListForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientListForm));
            this.pageHeader = new AntdUI.PageHeader();
            this.timerClientList = new System.Windows.Forms.Timer(this.components);
            this.tlpClientList = new WinsockPacketEditor.TableLayoutPanelEx();
            this.SuspendLayout();
            // 
            // pageHeader
            // 
            this.pageHeader.BackColor = System.Drawing.Color.Transparent;
            this.pageHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pageHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pageHeader.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pageHeader.Icon = global::WinsockPacketEditor.Properties.Resources.wpe;
            this.pageHeader.LocalizationText = "ClientList";
            this.pageHeader.Location = new System.Drawing.Point(0, 0);
            this.pageHeader.Margin = new System.Windows.Forms.Padding(5);
            this.pageHeader.Name = "pageHeader";
            this.pageHeader.ShowButton = true;
            this.pageHeader.ShowIcon = true;
            this.pageHeader.Size = new System.Drawing.Size(1450, 40);
            this.pageHeader.SubText = "";
            this.pageHeader.TabIndex = 7;
            this.pageHeader.Text = "客户端列表";
            // 
            // timerClientList
            // 
            this.timerClientList.Enabled = true;
            this.timerClientList.Interval = 1000;
            this.timerClientList.Tick += new System.EventHandler(this.timerClientList_Tick);
            // 
            // tlpClientList
            // 
            this.tlpClientList.ColumnCount = 1;
            this.tlpClientList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpClientList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpClientList.Location = new System.Drawing.Point(0, 40);
            this.tlpClientList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpClientList.Name = "tlpClientList";
            this.tlpClientList.RowCount = 1;
            this.tlpClientList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpClientList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 560F));
            this.tlpClientList.Size = new System.Drawing.Size(1450, 760);
            this.tlpClientList.TabIndex = 8;
            // 
            // ClientListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1450, 800);
            this.Controls.Add(this.tlpClientList);
            this.Controls.Add(this.pageHeader);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "ClientListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ClientListForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ClientListForm_FormClosed);
            this.Load += new System.EventHandler(this.ClientListForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.PageHeader pageHeader;
        private TableLayoutPanelEx tlpClientList;
        private System.Windows.Forms.Timer timerClientList;
    }
}