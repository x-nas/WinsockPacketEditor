namespace WinsockPacketEditor
{
    partial class AccountListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccountListForm));
            this.pageHeader = new AntdUI.PageHeader();
            this.tlpAccountList = new WinsockPacketEditor.TableLayoutPanelEx();
            this.SuspendLayout();
            // 
            // pageHeader
            // 
            this.pageHeader.BackColor = System.Drawing.Color.Transparent;
            this.pageHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pageHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pageHeader.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pageHeader.Icon = global::WinsockPacketEditor.Properties.Resources.wpe;
            this.pageHeader.LocalizationText = "AccountList";
            this.pageHeader.Location = new System.Drawing.Point(0, 0);
            this.pageHeader.Margin = new System.Windows.Forms.Padding(5);
            this.pageHeader.Name = "pageHeader";
            this.pageHeader.ShowButton = true;
            this.pageHeader.ShowIcon = true;
            this.pageHeader.Size = new System.Drawing.Size(1450, 40);
            this.pageHeader.SubText = "";
            this.pageHeader.TabIndex = 7;
            this.pageHeader.Text = "账号列表";
            // 
            // tlpAccountList
            // 
            this.tlpAccountList.ColumnCount = 1;
            this.tlpAccountList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAccountList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpAccountList.Location = new System.Drawing.Point(0, 40);
            this.tlpAccountList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpAccountList.Name = "tlpAccountList";
            this.tlpAccountList.RowCount = 1;
            this.tlpAccountList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAccountList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 560F));
            this.tlpAccountList.Size = new System.Drawing.Size(1450, 760);
            this.tlpAccountList.TabIndex = 8;
            // 
            // AccountListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1450, 800);
            this.Controls.Add(this.tlpAccountList);
            this.Controls.Add(this.pageHeader);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "AccountListForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AccountListForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AccountListForm_FormClosed);
            this.Load += new System.EventHandler(this.AccountListForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.PageHeader pageHeader;
        private TableLayoutPanelEx tlpAccountList;
    }
}