namespace WPELibrary.TextComparison
{
    partial class TextDuplicateForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextDuplicateForm));
            this.tlpTextCompare = new System.Windows.Forms.TableLayoutPanel();
            this.tlpTextCompare_Text = new System.Windows.Forms.TableLayoutPanel();
            this.lB = new System.Windows.Forms.Label();
            this.rtbB = new System.Windows.Forms.RichTextBox();
            this.rtbA = new System.Windows.Forms.RichTextBox();
            this.lA = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.nudMinLength = new System.Windows.Forms.NumericUpDown();
            this.bSearch = new System.Windows.Forms.Button();
            this.bRestore = new System.Windows.Forms.Button();
            this.lMinLength = new System.Windows.Forms.Label();
            this.tlpTextCompare.SuspendLayout();
            this.tlpTextCompare_Text.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinLength)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpTextCompare
            // 
            resources.ApplyResources(this.tlpTextCompare, "tlpTextCompare");
            this.tlpTextCompare.Controls.Add(this.tlpTextCompare_Text, 0, 0);
            this.tlpTextCompare.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tlpTextCompare.Name = "tlpTextCompare";
            // 
            // tlpTextCompare_Text
            // 
            resources.ApplyResources(this.tlpTextCompare_Text, "tlpTextCompare_Text");
            this.tlpTextCompare_Text.Controls.Add(this.lB, 1, 0);
            this.tlpTextCompare_Text.Controls.Add(this.rtbB, 1, 1);
            this.tlpTextCompare_Text.Controls.Add(this.rtbA, 0, 1);
            this.tlpTextCompare_Text.Controls.Add(this.lA, 0, 0);
            this.tlpTextCompare_Text.Name = "tlpTextCompare_Text";
            // 
            // lB
            // 
            resources.ApplyResources(this.lB, "lB");
            this.lB.Name = "lB";
            // 
            // rtbB
            // 
            resources.ApplyResources(this.rtbB, "rtbB");
            this.rtbB.Name = "rtbB";
            this.rtbB.TextChanged += new System.EventHandler(this.rtbB_TextChanged);
            // 
            // rtbA
            // 
            resources.ApplyResources(this.rtbA, "rtbA");
            this.rtbA.Name = "rtbA";
            this.rtbA.TextChanged += new System.EventHandler(this.rtbA_TextChanged);
            // 
            // lA
            // 
            resources.ApplyResources(this.lA, "lA");
            this.lA.Name = "lA";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.nudMinLength, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.bSearch, 4, 1);
            this.tableLayoutPanel1.Controls.Add(this.bRestore, 6, 1);
            this.tableLayoutPanel1.Controls.Add(this.lMinLength, 1, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // nudMinLength
            // 
            resources.ApplyResources(this.nudMinLength, "nudMinLength");
            this.nudMinLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMinLength.Name = "nudMinLength";
            this.nudMinLength.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // bSearch
            // 
            resources.ApplyResources(this.bSearch, "bSearch");
            this.bSearch.Name = "bSearch";
            this.bSearch.UseVisualStyleBackColor = true;
            this.bSearch.Click += new System.EventHandler(this.bSearch_Click);
            // 
            // bRestore
            // 
            resources.ApplyResources(this.bRestore, "bRestore");
            this.bRestore.Name = "bRestore";
            this.bRestore.UseVisualStyleBackColor = true;
            this.bRestore.Click += new System.EventHandler(this.bRestore_Click);
            // 
            // lMinLength
            // 
            resources.ApplyResources(this.lMinLength, "lMinLength");
            this.lMinLength.Name = "lMinLength";
            // 
            // TextDuplicateForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpTextCompare);
            this.DoubleBuffered = true;
            this.Name = "TextDuplicateForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TextDuplicateForm_FormClosed);
            this.Load += new System.EventHandler(this.TextDuplicateForm_Load);
            this.tlpTextCompare.ResumeLayout(false);
            this.tlpTextCompare_Text.ResumeLayout(false);
            this.tlpTextCompare_Text.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinLength)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpTextCompare;
        private System.Windows.Forms.TableLayoutPanel tlpTextCompare_Text;
        private System.Windows.Forms.Label lB;
        private System.Windows.Forms.RichTextBox rtbB;
        private System.Windows.Forms.RichTextBox rtbA;
        private System.Windows.Forms.Label lA;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button bSearch;
        private System.Windows.Forms.Button bRestore;
        private System.Windows.Forms.NumericUpDown nudMinLength;
        private System.Windows.Forms.Label lMinLength;
    }
}