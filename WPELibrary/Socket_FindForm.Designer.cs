namespace WPELibrary
{
    partial class Socket_FindForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Socket_FindForm));
            this.tlpFindForm = new System.Windows.Forms.TableLayoutPanel();
            this.hexFind = new Be.Windows.Forms.HexBox();
            this.txtFind = new System.Windows.Forms.TextBox();
            this.tlpLine = new System.Windows.Forms.TableLayoutPanel();
            this.lFind = new System.Windows.Forms.Label();
            this.pLine = new System.Windows.Forms.Panel();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.pFindType = new System.Windows.Forms.Panel();
            this.tlpFindType = new System.Windows.Forms.TableLayoutPanel();
            this.rbHex = new System.Windows.Forms.RadioButton();
            this.rbString = new System.Windows.Forms.RadioButton();
            this.tlpFindForm.SuspendLayout();
            this.tlpLine.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.pFindType.SuspendLayout();
            this.tlpFindType.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpFindForm
            // 
            resources.ApplyResources(this.tlpFindForm, "tlpFindForm");
            this.tlpFindForm.Controls.Add(this.hexFind, 1, 3);
            this.tlpFindForm.Controls.Add(this.txtFind, 1, 2);
            this.tlpFindForm.Controls.Add(this.tlpLine, 1, 0);
            this.tlpFindForm.Controls.Add(this.tlpButton, 1, 4);
            this.tlpFindForm.Controls.Add(this.pFindType, 1, 1);
            this.tlpFindForm.Name = "tlpFindForm";
            // 
            // hexFind
            // 
            this.hexFind.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            // 
            // 
            // 
            this.hexFind.BuiltInContextMenu.CopyMenuItemImage = global::WPELibrary.Properties.Resources.CopyHS;
            this.hexFind.BuiltInContextMenu.CopyMenuItemText = resources.GetString("hexFind.BuiltInContextMenu.CopyMenuItemText");
            this.hexFind.BuiltInContextMenu.CutMenuItemImage = global::WPELibrary.Properties.Resources.CutHS;
            this.hexFind.BuiltInContextMenu.CutMenuItemText = resources.GetString("hexFind.BuiltInContextMenu.CutMenuItemText");
            this.hexFind.BuiltInContextMenu.PasteMenuItemImage = global::WPELibrary.Properties.Resources.PasteHS;
            this.hexFind.BuiltInContextMenu.PasteMenuItemText = resources.GetString("hexFind.BuiltInContextMenu.PasteMenuItemText");
            this.hexFind.BuiltInContextMenu.SelectAllMenuItemText = resources.GetString("hexFind.BuiltInContextMenu.SelectAllMenuItemText");
            resources.ApplyResources(this.hexFind, "hexFind");
            this.hexFind.InfoForeColor = System.Drawing.Color.Empty;
            this.hexFind.Name = "hexFind";
            this.hexFind.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            // 
            // txtFind
            // 
            resources.ApplyResources(this.txtFind, "txtFind");
            this.txtFind.Name = "txtFind";
            // 
            // tlpLine
            // 
            resources.ApplyResources(this.tlpLine, "tlpLine");
            this.tlpLine.Controls.Add(this.lFind, 0, 0);
            this.tlpLine.Controls.Add(this.pLine, 1, 0);
            this.tlpLine.Name = "tlpLine";
            // 
            // lFind
            // 
            resources.ApplyResources(this.lFind, "lFind");
            this.lFind.Name = "lFind";
            // 
            // pLine
            // 
            resources.ApplyResources(this.pLine, "pLine");
            this.pLine.BackColor = System.Drawing.Color.LightGray;
            this.pLine.Name = "pLine";
            // 
            // tlpButton
            // 
            resources.ApplyResources(this.tlpButton, "tlpButton");
            this.tlpButton.Controls.Add(this.btnCancel, 3, 1);
            this.tlpButton.Controls.Add(this.btnOK, 1, 1);
            this.tlpButton.Name = "tlpButton";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // pFindType
            // 
            this.pFindType.Controls.Add(this.tlpFindType);
            resources.ApplyResources(this.pFindType, "pFindType");
            this.pFindType.Name = "pFindType";
            // 
            // tlpFindType
            // 
            resources.ApplyResources(this.tlpFindType, "tlpFindType");
            this.tlpFindType.Controls.Add(this.rbHex, 1, 0);
            this.tlpFindType.Controls.Add(this.rbString, 0, 0);
            this.tlpFindType.Name = "tlpFindType";
            // 
            // rbHex
            // 
            resources.ApplyResources(this.rbHex, "rbHex");
            this.rbHex.Name = "rbHex";
            // 
            // rbString
            // 
            resources.ApplyResources(this.rbString, "rbString");
            this.rbString.Checked = true;
            this.rbString.Name = "rbString";
            this.rbString.TabStop = true;
            this.rbString.CheckedChanged += new System.EventHandler(this.rbString_CheckedChanged);
            // 
            // Socket_FindForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpFindForm);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Socket_FindForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.tlpFindForm.ResumeLayout(false);
            this.tlpFindForm.PerformLayout();
            this.tlpLine.ResumeLayout(false);
            this.tlpLine.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.pFindType.ResumeLayout(false);
            this.tlpFindType.ResumeLayout(false);
            this.tlpFindType.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpFindForm;
        private System.Windows.Forms.TableLayoutPanel tlpLine;
        private System.Windows.Forms.Label lFind;
        private System.Windows.Forms.Panel pLine;
        private System.Windows.Forms.TextBox txtFind;
        private Be.Windows.Forms.HexBox hexFind;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel pFindType;
        private System.Windows.Forms.TableLayoutPanel tlpFindType;
        private System.Windows.Forms.RadioButton rbHex;
        private System.Windows.Forms.RadioButton rbString;
    }
}