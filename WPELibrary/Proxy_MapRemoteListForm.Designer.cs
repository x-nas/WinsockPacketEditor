namespace WPELibrary
{
    partial class Proxy_MapRemoteListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Proxy_MapRemoteListForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.bAdd = new System.Windows.Forms.Button();
            this.bOK = new System.Windows.Forms.Button();
            this.tlpMapRemote_Button = new System.Windows.Forms.TableLayoutPanel();
            this.bRemove = new System.Windows.Forms.Button();
            this.cmsMapRemote_Clear = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsMapRemote_Export = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsMapRemote_Import = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsMapRemote_Bottom = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsMapRemote_Down = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsMapRemote_Up = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsMapRemote_Top = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsMapRemote = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.lMapRemote = new System.Windows.Forms.Label();
            this.dgvMapRemote = new System.Windows.Forms.DataGridView();
            this.cIsEnable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cProtocolType_From = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cProtocolType_To = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cHost_From = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cHost_To = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPort_From = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPort_To = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPath_From = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPath_To = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cFromURL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cToURL = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlpMapRemote = new System.Windows.Forms.TableLayoutPanel();
            this.tlpMapRemote_Button.SuspendLayout();
            this.cmsMapRemote.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMapRemote)).BeginInit();
            this.tlpMapRemote.SuspendLayout();
            this.SuspendLayout();
            // 
            // bAdd
            // 
            resources.ApplyResources(this.bAdd, "bAdd");
            this.bAdd.Name = "bAdd";
            this.bAdd.UseVisualStyleBackColor = true;
            this.bAdd.Click += new System.EventHandler(this.bAdd_Click);
            // 
            // bOK
            // 
            resources.ApplyResources(this.bOK, "bOK");
            this.bOK.Name = "bOK";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.bOK_Click);
            // 
            // tlpMapRemote_Button
            // 
            resources.ApplyResources(this.tlpMapRemote_Button, "tlpMapRemote_Button");
            this.tlpMapRemote_Button.Controls.Add(this.bAdd, 1, 0);
            this.tlpMapRemote_Button.Controls.Add(this.bRemove, 3, 0);
            this.tlpMapRemote_Button.Controls.Add(this.bOK, 7, 0);
            this.tlpMapRemote_Button.Name = "tlpMapRemote_Button";
            // 
            // bRemove
            // 
            resources.ApplyResources(this.bRemove, "bRemove");
            this.bRemove.Name = "bRemove";
            this.bRemove.UseVisualStyleBackColor = true;
            this.bRemove.Click += new System.EventHandler(this.bRemove_Click);
            // 
            // cmsMapRemote_Clear
            // 
            resources.ApplyResources(this.cmsMapRemote_Clear, "cmsMapRemote_Clear");
            this.cmsMapRemote_Clear.Image = global::WPELibrary.Properties.Resources.Trash_can16;
            this.cmsMapRemote_Clear.Name = "cmsMapRemote_Clear";
            // 
            // toolStripSeparator4
            // 
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            // 
            // cmsMapRemote_Export
            // 
            resources.ApplyResources(this.cmsMapRemote_Export, "cmsMapRemote_Export");
            this.cmsMapRemote_Export.Image = global::WPELibrary.Properties.Resources.save;
            this.cmsMapRemote_Export.Name = "cmsMapRemote_Export";
            // 
            // cmsMapRemote_Import
            // 
            resources.ApplyResources(this.cmsMapRemote_Import, "cmsMapRemote_Import");
            this.cmsMapRemote_Import.Image = global::WPELibrary.Properties.Resources.openHS;
            this.cmsMapRemote_Import.Name = "cmsMapRemote_Import";
            // 
            // toolStripSeparator3
            // 
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            // 
            // cmsMapRemote_Bottom
            // 
            resources.ApplyResources(this.cmsMapRemote_Bottom, "cmsMapRemote_Bottom");
            this.cmsMapRemote_Bottom.Image = global::WPELibrary.Properties.Resources.go_bottom;
            this.cmsMapRemote_Bottom.Name = "cmsMapRemote_Bottom";
            // 
            // toolStripSeparator2
            // 
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // cmsMapRemote_Down
            // 
            resources.ApplyResources(this.cmsMapRemote_Down, "cmsMapRemote_Down");
            this.cmsMapRemote_Down.Image = global::WPELibrary.Properties.Resources.Down;
            this.cmsMapRemote_Down.Name = "cmsMapRemote_Down";
            // 
            // cmsMapRemote_Up
            // 
            resources.ApplyResources(this.cmsMapRemote_Up, "cmsMapRemote_Up");
            this.cmsMapRemote_Up.Image = global::WPELibrary.Properties.Resources.Up;
            this.cmsMapRemote_Up.Name = "cmsMapRemote_Up";
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // cmsMapRemote_Top
            // 
            resources.ApplyResources(this.cmsMapRemote_Top, "cmsMapRemote_Top");
            this.cmsMapRemote_Top.Image = global::WPELibrary.Properties.Resources.go_top;
            this.cmsMapRemote_Top.Name = "cmsMapRemote_Top";
            // 
            // cmsMapRemote
            // 
            resources.ApplyResources(this.cmsMapRemote, "cmsMapRemote");
            this.cmsMapRemote.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsMapRemote_Top,
            this.toolStripSeparator1,
            this.cmsMapRemote_Up,
            this.cmsMapRemote_Down,
            this.toolStripSeparator2,
            this.cmsMapRemote_Bottom,
            this.toolStripSeparator3,
            this.cmsMapRemote_Import,
            this.cmsMapRemote_Export,
            this.toolStripSeparator4,
            this.cmsMapRemote_Clear});
            this.cmsMapRemote.Name = "cmsMapLocal";
            this.cmsMapRemote.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsMapRemote_ItemClicked);
            // 
            // lMapRemote
            // 
            resources.ApplyResources(this.lMapRemote, "lMapRemote");
            this.lMapRemote.Name = "lMapRemote";
            // 
            // dgvMapRemote
            // 
            resources.ApplyResources(this.dgvMapRemote, "dgvMapRemote");
            this.dgvMapRemote.AllowUserToAddRows = false;
            this.dgvMapRemote.AllowUserToDeleteRows = false;
            this.dgvMapRemote.AllowUserToResizeColumns = false;
            this.dgvMapRemote.AllowUserToResizeRows = false;
            this.dgvMapRemote.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvMapRemote.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvMapRemote.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvMapRemote.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvMapRemote.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMapRemote.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cIsEnable,
            this.cProtocolType_From,
            this.cProtocolType_To,
            this.cHost_From,
            this.cHost_To,
            this.cPort_From,
            this.cPort_To,
            this.cPath_From,
            this.cPath_To,
            this.cFromURL,
            this.cToURL});
            this.dgvMapRemote.ContextMenuStrip = this.cmsMapRemote;
            this.dgvMapRemote.MultiSelect = false;
            this.dgvMapRemote.Name = "dgvMapRemote";
            this.dgvMapRemote.RowHeadersVisible = false;
            this.dgvMapRemote.RowTemplate.Height = 23;
            this.dgvMapRemote.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMapRemote.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMapRemote_CellContentClick);
            this.dgvMapRemote.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvMapRemote_CellDoubleClick);
            this.dgvMapRemote.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvMapRemote_CellFormatting);
            // 
            // cIsEnable
            // 
            this.cIsEnable.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cIsEnable.DataPropertyName = "IsEnable";
            resources.ApplyResources(this.cIsEnable, "cIsEnable");
            this.cIsEnable.Name = "cIsEnable";
            this.cIsEnable.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // cProtocolType_From
            // 
            this.cProtocolType_From.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cProtocolType_From.DataPropertyName = "ProtocolType_From";
            resources.ApplyResources(this.cProtocolType_From, "cProtocolType_From");
            this.cProtocolType_From.Name = "cProtocolType_From";
            this.cProtocolType_From.ReadOnly = true;
            this.cProtocolType_From.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cProtocolType_From.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cProtocolType_To
            // 
            this.cProtocolType_To.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cProtocolType_To.DataPropertyName = "ProtocolType_To";
            resources.ApplyResources(this.cProtocolType_To, "cProtocolType_To");
            this.cProtocolType_To.Name = "cProtocolType_To";
            this.cProtocolType_To.ReadOnly = true;
            this.cProtocolType_To.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cProtocolType_To.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cHost_From
            // 
            this.cHost_From.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cHost_From.DataPropertyName = "Host_From";
            resources.ApplyResources(this.cHost_From, "cHost_From");
            this.cHost_From.Name = "cHost_From";
            this.cHost_From.ReadOnly = true;
            this.cHost_From.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cHost_From.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cHost_To
            // 
            this.cHost_To.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cHost_To.DataPropertyName = "Host_To";
            resources.ApplyResources(this.cHost_To, "cHost_To");
            this.cHost_To.Name = "cHost_To";
            this.cHost_To.ReadOnly = true;
            this.cHost_To.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cHost_To.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cPort_From
            // 
            this.cPort_From.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cPort_From.DataPropertyName = "Port_From";
            resources.ApplyResources(this.cPort_From, "cPort_From");
            this.cPort_From.Name = "cPort_From";
            this.cPort_From.ReadOnly = true;
            this.cPort_From.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cPort_From.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cPort_To
            // 
            this.cPort_To.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cPort_To.DataPropertyName = "Port_To";
            resources.ApplyResources(this.cPort_To, "cPort_To");
            this.cPort_To.Name = "cPort_To";
            this.cPort_To.ReadOnly = true;
            this.cPort_To.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cPort_To.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cPath_From
            // 
            this.cPath_From.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cPath_From.DataPropertyName = "Path_From";
            resources.ApplyResources(this.cPath_From, "cPath_From");
            this.cPath_From.Name = "cPath_From";
            this.cPath_From.ReadOnly = true;
            this.cPath_From.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cPath_From.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cPath_To
            // 
            this.cPath_To.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cPath_To.DataPropertyName = "Path_To";
            resources.ApplyResources(this.cPath_To, "cPath_To");
            this.cPath_To.Name = "cPath_To";
            this.cPath_To.ReadOnly = true;
            this.cPath_To.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cPath_To.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cFromURL
            // 
            this.cFromURL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cFromURL.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.cFromURL, "cFromURL");
            this.cFromURL.Name = "cFromURL";
            this.cFromURL.ReadOnly = true;
            this.cFromURL.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cFromURL.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cToURL
            // 
            this.cToURL.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.cToURL, "cToURL");
            this.cToURL.Name = "cToURL";
            this.cToURL.ReadOnly = true;
            this.cToURL.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cToURL.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tlpMapRemote
            // 
            resources.ApplyResources(this.tlpMapRemote, "tlpMapRemote");
            this.tlpMapRemote.Controls.Add(this.lMapRemote, 0, 0);
            this.tlpMapRemote.Controls.Add(this.dgvMapRemote, 0, 1);
            this.tlpMapRemote.Controls.Add(this.tlpMapRemote_Button, 0, 2);
            this.tlpMapRemote.Name = "tlpMapRemote";
            // 
            // Proxy_MapRemoteListForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpMapRemote);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Proxy_MapRemoteListForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Proxy_MapRemoteListForm_FormClosed);
            this.Load += new System.EventHandler(this.Proxy_MapRemoteListForm_Load);
            this.tlpMapRemote_Button.ResumeLayout(false);
            this.cmsMapRemote.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMapRemote)).EndInit();
            this.tlpMapRemote.ResumeLayout(false);
            this.tlpMapRemote.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button bAdd;
        private System.Windows.Forms.Button bOK;
        private System.Windows.Forms.TableLayoutPanel tlpMapRemote_Button;
        private System.Windows.Forms.Button bRemove;
        private System.Windows.Forms.ToolStripMenuItem cmsMapRemote_Clear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem cmsMapRemote_Export;
        private System.Windows.Forms.ToolStripMenuItem cmsMapRemote_Import;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem cmsMapRemote_Bottom;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem cmsMapRemote_Down;
        private System.Windows.Forms.ToolStripMenuItem cmsMapRemote_Up;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cmsMapRemote_Top;
        private System.Windows.Forms.ContextMenuStrip cmsMapRemote;
        private System.Windows.Forms.Label lMapRemote;
        private System.Windows.Forms.DataGridView dgvMapRemote;
        private System.Windows.Forms.TableLayoutPanel tlpMapRemote;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cIsEnable;
        private System.Windows.Forms.DataGridViewTextBoxColumn cProtocolType_From;
        private System.Windows.Forms.DataGridViewTextBoxColumn cProtocolType_To;
        private System.Windows.Forms.DataGridViewTextBoxColumn cHost_From;
        private System.Windows.Forms.DataGridViewTextBoxColumn cHost_To;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPort_From;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPort_To;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPath_From;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPath_To;
        private System.Windows.Forms.DataGridViewTextBoxColumn cFromURL;
        private System.Windows.Forms.DataGridViewTextBoxColumn cToURL;
    }
}