namespace WPELibrary
{
    partial class Socket_CompareForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Socket_CompareForm));
            this.tlpSocketCompare = new System.Windows.Forms.TableLayoutPanel();
            this.tlpPacketData = new System.Windows.Forms.TableLayoutPanel();
            this.lRawData = new System.Windows.Forms.Label();
            this.lModifiedData = new System.Windows.Forms.Label();
            this.rtbCompare = new System.Windows.Forms.RichTextBox();
            this.hbRawData = new Be.Windows.Forms.HexBox();
            this.hbModifiedData = new Be.Windows.Forms.HexBox();
            this.tlpSocketCompare.SuspendLayout();
            this.tlpPacketData.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpSocketCompare
            // 
            this.tlpSocketCompare.ColumnCount = 1;
            this.tlpSocketCompare.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSocketCompare.Controls.Add(this.rtbCompare, 0, 1);
            this.tlpSocketCompare.Controls.Add(this.tlpPacketData, 0, 0);
            this.tlpSocketCompare.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSocketCompare.Location = new System.Drawing.Point(0, 0);
            this.tlpSocketCompare.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSocketCompare.Name = "tlpSocketCompare";
            this.tlpSocketCompare.RowCount = 2;
            this.tlpSocketCompare.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tlpSocketCompare.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tlpSocketCompare.Size = new System.Drawing.Size(1184, 611);
            this.tlpSocketCompare.TabIndex = 0;
            // 
            // tlpPacketData
            // 
            this.tlpPacketData.ColumnCount = 2;
            this.tlpPacketData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpPacketData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpPacketData.Controls.Add(this.hbModifiedData, 1, 1);
            this.tlpPacketData.Controls.Add(this.lModifiedData, 1, 0);
            this.tlpPacketData.Controls.Add(this.lRawData, 0, 0);
            this.tlpPacketData.Controls.Add(this.hbRawData, 0, 1);
            this.tlpPacketData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPacketData.Location = new System.Drawing.Point(0, 0);
            this.tlpPacketData.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPacketData.Name = "tlpPacketData";
            this.tlpPacketData.RowCount = 2;
            this.tlpPacketData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpPacketData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketData.Size = new System.Drawing.Size(1184, 366);
            this.tlpPacketData.TabIndex = 0;
            // 
            // lRawData
            // 
            this.lRawData.AutoSize = true;
            this.lRawData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lRawData.Location = new System.Drawing.Point(3, 3);
            this.lRawData.Margin = new System.Windows.Forms.Padding(3);
            this.lRawData.Name = "lRawData";
            this.lRawData.Size = new System.Drawing.Size(586, 24);
            this.lRawData.TabIndex = 0;
            this.lRawData.Text = "原始数据";
            this.lRawData.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lModifiedData
            // 
            this.lModifiedData.AutoSize = true;
            this.lModifiedData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lModifiedData.Location = new System.Drawing.Point(595, 3);
            this.lModifiedData.Margin = new System.Windows.Forms.Padding(3);
            this.lModifiedData.Name = "lModifiedData";
            this.lModifiedData.Size = new System.Drawing.Size(586, 24);
            this.lModifiedData.TabIndex = 1;
            this.lModifiedData.Text = "修改后数据";
            this.lModifiedData.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rtbCompare
            // 
            this.rtbCompare.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbCompare.Location = new System.Drawing.Point(3, 366);
            this.rtbCompare.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.rtbCompare.Name = "rtbCompare";
            this.rtbCompare.ReadOnly = true;
            this.rtbCompare.Size = new System.Drawing.Size(1178, 242);
            this.rtbCompare.TabIndex = 4;
            this.rtbCompare.Text = "";
            // 
            // hbRawData
            // 
            // 
            // 
            // 
            this.hbRawData.BuiltInContextMenu.CopyMenuItemImage = global::WPELibrary.Properties.Resources.copy;
            this.hbRawData.BuiltInContextMenu.CopyMenuItemText = "复制";
            this.hbRawData.BuiltInContextMenu.CutMenuItemImage = global::WPELibrary.Properties.Resources.cut;
            this.hbRawData.BuiltInContextMenu.CutMenuItemText = "剪切";
            this.hbRawData.BuiltInContextMenu.PasteMenuItemImage = global::WPELibrary.Properties.Resources.paste;
            this.hbRawData.BuiltInContextMenu.PasteMenuItemText = "粘贴";
            this.hbRawData.BuiltInContextMenu.SelectAllMenuItemImage = global::WPELibrary.Properties.Resources.SelectAll;
            this.hbRawData.BuiltInContextMenu.SelectAllMenuItemText = "全选";
            this.hbRawData.ColumnInfoVisible = true;
            this.hbRawData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hbRawData.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.hbRawData.LineInfoVisible = true;
            this.hbRawData.Location = new System.Drawing.Point(3, 33);
            this.hbRawData.Name = "hbRawData";
            this.hbRawData.ReadOnly = true;
            this.hbRawData.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hbRawData.Size = new System.Drawing.Size(586, 330);
            this.hbRawData.StringViewVisible = true;
            this.hbRawData.TabIndex = 2;
            this.hbRawData.VScrollBarVisible = true;
            // 
            // hbModifiedData
            // 
            // 
            // 
            // 
            this.hbModifiedData.BuiltInContextMenu.CopyMenuItemImage = global::WPELibrary.Properties.Resources.copy;
            this.hbModifiedData.BuiltInContextMenu.CopyMenuItemText = "复制";
            this.hbModifiedData.BuiltInContextMenu.CutMenuItemImage = global::WPELibrary.Properties.Resources.cut;
            this.hbModifiedData.BuiltInContextMenu.CutMenuItemText = "剪切";
            this.hbModifiedData.BuiltInContextMenu.PasteMenuItemImage = global::WPELibrary.Properties.Resources.paste;
            this.hbModifiedData.BuiltInContextMenu.PasteMenuItemText = "粘贴";
            this.hbModifiedData.BuiltInContextMenu.SelectAllMenuItemImage = global::WPELibrary.Properties.Resources.SelectAll;
            this.hbModifiedData.BuiltInContextMenu.SelectAllMenuItemText = "全选";
            this.hbModifiedData.ColumnInfoVisible = true;
            this.hbModifiedData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hbModifiedData.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F);
            this.hbModifiedData.LineInfoVisible = true;
            this.hbModifiedData.Location = new System.Drawing.Point(595, 33);
            this.hbModifiedData.Name = "hbModifiedData";
            this.hbModifiedData.ReadOnly = true;
            this.hbModifiedData.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hbModifiedData.Size = new System.Drawing.Size(586, 330);
            this.hbModifiedData.StringViewVisible = true;
            this.hbModifiedData.TabIndex = 3;
            this.hbModifiedData.VScrollBarVisible = true;
            // 
            // Socket_CompareForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1184, 611);
            this.Controls.Add(this.tlpSocketCompare);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Socket_CompareForm";
            this.Text = "封包数据修改情况";
            this.tlpSocketCompare.ResumeLayout(false);
            this.tlpPacketData.ResumeLayout(false);
            this.tlpPacketData.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpSocketCompare;
        private System.Windows.Forms.TableLayoutPanel tlpPacketData;
        private System.Windows.Forms.Label lModifiedData;
        private System.Windows.Forms.Label lRawData;
        private System.Windows.Forms.RichTextBox rtbCompare;
        private Be.Windows.Forms.HexBox hbRawData;
        private Be.Windows.Forms.HexBox hbModifiedData;
    }
}