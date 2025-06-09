namespace WPELibrary.TextComparison
{
    partial class TextCompareForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TextCompareForm));
            this.tlpTextCompare = new System.Windows.Forms.TableLayoutPanel();
            this.tlpTextCompare_Text = new System.Windows.Forms.TableLayoutPanel();
            this.lB = new System.Windows.Forms.Label();
            this.rtbB = new System.Windows.Forms.RichTextBox();
            this.rtbA = new System.Windows.Forms.RichTextBox();
            this.lA = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.bClear = new System.Windows.Forms.Button();
            this.bCompare = new System.Windows.Forms.Button();
            this.bExchange = new System.Windows.Forms.Button();
            this.rtbResult = new System.Windows.Forms.RichTextBox();
            this.tlpTextCompare.SuspendLayout();
            this.tlpTextCompare_Text.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpTextCompare
            // 
            resources.ApplyResources(this.tlpTextCompare, "tlpTextCompare");
            this.tlpTextCompare.Controls.Add(this.tlpTextCompare_Text, 0, 0);
            this.tlpTextCompare.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tlpTextCompare.Controls.Add(this.rtbResult, 0, 2);
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
            this.tableLayoutPanel1.Controls.Add(this.bClear, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.bCompare, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.bExchange, 5, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // bClear
            // 
            resources.ApplyResources(this.bClear, "bClear");
            this.bClear.Name = "bClear";
            this.bClear.UseVisualStyleBackColor = true;
            this.bClear.Click += new System.EventHandler(this.bClear_Click);
            // 
            // bCompare
            // 
            resources.ApplyResources(this.bCompare, "bCompare");
            this.bCompare.Name = "bCompare";
            this.bCompare.UseVisualStyleBackColor = true;
            this.bCompare.Click += new System.EventHandler(this.bCompare_Click);
            // 
            // bExchange
            // 
            resources.ApplyResources(this.bExchange, "bExchange");
            this.bExchange.Name = "bExchange";
            this.bExchange.UseVisualStyleBackColor = true;
            this.bExchange.Click += new System.EventHandler(this.bExchange_Click);
            // 
            // rtbResult
            // 
            resources.ApplyResources(this.rtbResult, "rtbResult");
            this.rtbResult.Name = "rtbResult";
            this.rtbResult.ReadOnly = true;
            // 
            // TextCompareForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpTextCompare);
            this.DoubleBuffered = true;
            this.Name = "TextCompareForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.TextCompareForm_FormClosed);
            this.Load += new System.EventHandler(this.TextCompareForm_Load);
            this.tlpTextCompare.ResumeLayout(false);
            this.tlpTextCompare_Text.ResumeLayout(false);
            this.tlpTextCompare_Text.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpTextCompare;
        private System.Windows.Forms.TableLayoutPanel tlpTextCompare_Text;
        private System.Windows.Forms.RichTextBox rtbB;
        private System.Windows.Forms.RichTextBox rtbA;
        private System.Windows.Forms.Label lB;
        private System.Windows.Forms.Label lA;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button bClear;
        private System.Windows.Forms.Button bCompare;
        private System.Windows.Forms.Button bExchange;
        private System.Windows.Forms.RichTextBox rtbResult;
    }
}