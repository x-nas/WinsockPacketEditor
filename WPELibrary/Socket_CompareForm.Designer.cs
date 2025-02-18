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
            this.hbModifiedData = new Be.Windows.Forms.HexBox();
            this.lModifiedData = new System.Windows.Forms.Label();
            this.lRawData = new System.Windows.Forms.Label();
            this.hbRawData = new Be.Windows.Forms.HexBox();
            this.pCompare = new System.Windows.Forms.Panel();
            this.rtbCompare = new System.Windows.Forms.RichTextBox();
            this.tlpSocketCompare.SuspendLayout();
            this.tlpPacketData.SuspendLayout();
            this.pCompare.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpSocketCompare
            // 
            resources.ApplyResources(this.tlpSocketCompare, "tlpSocketCompare");
            this.tlpSocketCompare.Controls.Add(this.tlpPacketData, 0, 0);
            this.tlpSocketCompare.Controls.Add(this.pCompare, 0, 1);
            this.tlpSocketCompare.Name = "tlpSocketCompare";
            // 
            // tlpPacketData
            // 
            resources.ApplyResources(this.tlpPacketData, "tlpPacketData");
            this.tlpPacketData.Controls.Add(this.hbModifiedData, 1, 1);
            this.tlpPacketData.Controls.Add(this.lModifiedData, 1, 0);
            this.tlpPacketData.Controls.Add(this.lRawData, 0, 0);
            this.tlpPacketData.Controls.Add(this.hbRawData, 0, 1);
            this.tlpPacketData.Name = "tlpPacketData";
            // 
            // hbModifiedData
            // 
            // 
            // 
            // 
            this.hbModifiedData.BuiltInContextMenu.CopyMenuItemImage = global::WPELibrary.Properties.Resources.copy;
            this.hbModifiedData.BuiltInContextMenu.CopyMenuItemText = resources.GetString("hbModifiedData.BuiltInContextMenu.CopyMenuItemText");
            this.hbModifiedData.BuiltInContextMenu.CutMenuItemImage = global::WPELibrary.Properties.Resources.cut;
            this.hbModifiedData.BuiltInContextMenu.CutMenuItemText = resources.GetString("hbModifiedData.BuiltInContextMenu.CutMenuItemText");
            this.hbModifiedData.BuiltInContextMenu.PasteMenuItemImage = global::WPELibrary.Properties.Resources.paste;
            this.hbModifiedData.BuiltInContextMenu.PasteMenuItemText = resources.GetString("hbModifiedData.BuiltInContextMenu.PasteMenuItemText");
            this.hbModifiedData.BuiltInContextMenu.SelectAllMenuItemImage = global::WPELibrary.Properties.Resources.SelectAll;
            this.hbModifiedData.BuiltInContextMenu.SelectAllMenuItemText = resources.GetString("hbModifiedData.BuiltInContextMenu.SelectAllMenuItemText");
            this.hbModifiedData.ColumnInfoVisible = true;
            resources.ApplyResources(this.hbModifiedData, "hbModifiedData");
            this.hbModifiedData.LineInfoVisible = true;
            this.hbModifiedData.Name = "hbModifiedData";
            this.hbModifiedData.ReadOnly = true;
            this.hbModifiedData.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hbModifiedData.StringViewVisible = true;
            this.hbModifiedData.VScrollBarVisible = true;
            // 
            // lModifiedData
            // 
            resources.ApplyResources(this.lModifiedData, "lModifiedData");
            this.lModifiedData.Name = "lModifiedData";
            // 
            // lRawData
            // 
            resources.ApplyResources(this.lRawData, "lRawData");
            this.lRawData.Name = "lRawData";
            // 
            // hbRawData
            // 
            // 
            // 
            // 
            this.hbRawData.BuiltInContextMenu.CopyMenuItemImage = global::WPELibrary.Properties.Resources.copy;
            this.hbRawData.BuiltInContextMenu.CopyMenuItemText = resources.GetString("hbRawData.BuiltInContextMenu.CopyMenuItemText");
            this.hbRawData.BuiltInContextMenu.CutMenuItemImage = global::WPELibrary.Properties.Resources.cut;
            this.hbRawData.BuiltInContextMenu.CutMenuItemText = resources.GetString("hbRawData.BuiltInContextMenu.CutMenuItemText");
            this.hbRawData.BuiltInContextMenu.PasteMenuItemImage = global::WPELibrary.Properties.Resources.paste;
            this.hbRawData.BuiltInContextMenu.PasteMenuItemText = resources.GetString("hbRawData.BuiltInContextMenu.PasteMenuItemText");
            this.hbRawData.BuiltInContextMenu.SelectAllMenuItemImage = global::WPELibrary.Properties.Resources.SelectAll;
            this.hbRawData.BuiltInContextMenu.SelectAllMenuItemText = resources.GetString("hbRawData.BuiltInContextMenu.SelectAllMenuItemText");
            this.hbRawData.ColumnInfoVisible = true;
            resources.ApplyResources(this.hbRawData, "hbRawData");
            this.hbRawData.LineInfoVisible = true;
            this.hbRawData.Name = "hbRawData";
            this.hbRawData.ReadOnly = true;
            this.hbRawData.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hbRawData.StringViewVisible = true;
            this.hbRawData.VScrollBarVisible = true;
            // 
            // pCompare
            // 
            this.pCompare.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pCompare.Controls.Add(this.rtbCompare);
            resources.ApplyResources(this.pCompare, "pCompare");
            this.pCompare.Name = "pCompare";
            // 
            // rtbCompare
            // 
            this.rtbCompare.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this.rtbCompare, "rtbCompare");
            this.rtbCompare.Name = "rtbCompare";
            this.rtbCompare.ReadOnly = true;
            // 
            // Socket_CompareForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpSocketCompare);
            this.DoubleBuffered = true;
            this.Name = "Socket_CompareForm";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Socket_CompareForm_Load);
            this.tlpSocketCompare.ResumeLayout(false);
            this.tlpPacketData.ResumeLayout(false);
            this.tlpPacketData.PerformLayout();
            this.pCompare.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpSocketCompare;
        private System.Windows.Forms.TableLayoutPanel tlpPacketData;
        private System.Windows.Forms.Label lModifiedData;
        private System.Windows.Forms.Label lRawData;
        private Be.Windows.Forms.HexBox hbRawData;
        private Be.Windows.Forms.HexBox hbModifiedData;
        private System.Windows.Forms.Panel pCompare;
        private System.Windows.Forms.RichTextBox rtbCompare;
    }
}