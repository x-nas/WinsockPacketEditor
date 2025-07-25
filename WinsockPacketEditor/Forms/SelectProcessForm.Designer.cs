namespace WinsockPacketEditor
{
    partial class SelectProcessForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectProcessForm));
            this.pageHeader = new AntdUI.PageHeader();
            this.tlpSelectProcess = new System.Windows.Forms.TableLayoutPanel();
            this.pProcessList = new AntdUI.Panel();
            this.tProcessList = new AntdUI.Table();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bCreate = new AntdUI.Button();
            this.bRefresh = new AntdUI.Button();
            this.bInject = new AntdUI.Button();
            this.tlpSearch = new System.Windows.Forms.TableLayoutPanel();
            this.txtSelectProcess = new AntdUI.Input();
            this.txtSearchProcess = new AntdUI.Input();
            this.tlpSelectProcess.SuspendLayout();
            this.pProcessList.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // pageHeader
            // 
            this.pageHeader.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pageHeader.DividerMargin = 3;
            this.pageHeader.DividerShow = true;
            this.pageHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pageHeader.Icon = global::WinsockPacketEditor.Properties.Resources.wpe;
            this.pageHeader.Location = new System.Drawing.Point(0, 0);
            this.pageHeader.MaximizeBox = false;
            this.pageHeader.MinimizeBox = false;
            this.pageHeader.Name = "pageHeader";
            this.pageHeader.ShowButton = true;
            this.pageHeader.ShowIcon = true;
            this.pageHeader.Size = new System.Drawing.Size(1100, 40);
            this.pageHeader.SubText = "2.0.0.0";
            this.pageHeader.TabIndex = 1;
            this.pageHeader.Text = "WPE x64";
            // 
            // tlpSelectProcess
            // 
            this.tlpSelectProcess.ColumnCount = 1;
            this.tlpSelectProcess.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSelectProcess.Controls.Add(this.pProcessList, 0, 1);
            this.tlpSelectProcess.Controls.Add(this.tlpButton, 0, 2);
            this.tlpSelectProcess.Controls.Add(this.tlpSearch, 0, 0);
            this.tlpSelectProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSelectProcess.Location = new System.Drawing.Point(0, 40);
            this.tlpSelectProcess.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSelectProcess.Name = "tlpSelectProcess";
            this.tlpSelectProcess.RowCount = 3;
            this.tlpSelectProcess.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpSelectProcess.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSelectProcess.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpSelectProcess.Size = new System.Drawing.Size(1100, 560);
            this.tlpSelectProcess.TabIndex = 2;
            // 
            // pProcessList
            // 
            this.pProcessList.Controls.Add(this.tProcessList);
            this.pProcessList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pProcessList.Location = new System.Drawing.Point(3, 63);
            this.pProcessList.Name = "pProcessList";
            this.pProcessList.Size = new System.Drawing.Size(1094, 434);
            this.pProcessList.TabIndex = 1;
            this.pProcessList.Text = "panel1";
            // 
            // tProcessList
            // 
            this.tProcessList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tProcessList.CellImpactHeight = false;
            this.tProcessList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tProcessList.Empty = false;
            this.tProcessList.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tProcessList.Gap = 8;
            this.tProcessList.GapCell = 0;
            this.tProcessList.Location = new System.Drawing.Point(0, 0);
            this.tProcessList.Name = "tProcessList";
            this.tProcessList.Radius = 6;
            this.tProcessList.Size = new System.Drawing.Size(1094, 434);
            this.tProcessList.TabIndex = 0;
            this.tProcessList.CellClick += new AntdUI.Table.ClickEventHandler(this.tProcessList_CellClick);
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 7;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Controls.Add(this.bCreate, 1, 0);
            this.tlpButton.Controls.Add(this.bRefresh, 3, 0);
            this.tlpButton.Controls.Add(this.bInject, 5, 0);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 500);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 2;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.Size = new System.Drawing.Size(1100, 60);
            this.tlpButton.TabIndex = 2;
            // 
            // bCreate
            // 
            this.bCreate.BackExtend = "135, #6253E1, #04BEFE";
            this.bCreate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bCreate.IconSvg = "SelectOutlined";
            this.bCreate.LocalizationText = "SelectProcessForm.{id}";
            this.bCreate.Location = new System.Drawing.Point(338, 3);
            this.bCreate.Name = "bCreate";
            this.bCreate.Size = new System.Drawing.Size(144, 46);
            this.bCreate.TabIndex = 0;
            this.bCreate.Text = "选择程序";
            this.bCreate.Type = AntdUI.TTypeMini.Primary;
            this.bCreate.Click += new System.EventHandler(this.bCreate_Click);
            // 
            // bRefresh
            // 
            this.bRefresh.BackExtend = "135, #6253E1, #04BEFE";
            this.bRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bRefresh.IconSvg = "ReloadOutlined";
            this.bRefresh.LocalizationText = "SelectProcessForm.{id}";
            this.bRefresh.Location = new System.Drawing.Point(508, 3);
            this.bRefresh.Name = "bRefresh";
            this.bRefresh.Size = new System.Drawing.Size(114, 46);
            this.bRefresh.TabIndex = 1;
            this.bRefresh.Text = "刷新";
            this.bRefresh.Type = AntdUI.TTypeMini.Primary;
            this.bRefresh.Click += new System.EventHandler(this.bRefresh_Click);
            // 
            // bInject
            // 
            this.bInject.BackExtend = "135, #6253E1, #04BEFE";
            this.bInject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bInject.IconSvg = "AimOutlined";
            this.bInject.LocalizationText = "SelectProcessForm.{id}";
            this.bInject.Location = new System.Drawing.Point(648, 3);
            this.bInject.Name = "bInject";
            this.bInject.Size = new System.Drawing.Size(114, 46);
            this.bInject.TabIndex = 2;
            this.bInject.Text = "注入";
            this.bInject.Type = AntdUI.TTypeMini.Primary;
            this.bInject.Click += new System.EventHandler(this.bInject_Click);
            // 
            // tlpSearch
            // 
            this.tlpSearch.ColumnCount = 2;
            this.tlpSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSearch.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSearch.Controls.Add(this.txtSearchProcess, 1, 1);
            this.tlpSearch.Controls.Add(this.txtSelectProcess, 0, 1);
            this.tlpSearch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSearch.Location = new System.Drawing.Point(0, 0);
            this.tlpSearch.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSearch.Name = "tlpSearch";
            this.tlpSearch.RowCount = 3;
            this.tlpSearch.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSearch.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpSearch.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSearch.Size = new System.Drawing.Size(1100, 60);
            this.tlpSearch.TabIndex = 3;
            // 
            // txtSelectProcess
            // 
            this.txtSelectProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSelectProcess.LocalizationPlaceholderText = "SelectProcessForm.{id}";
            this.txtSelectProcess.Location = new System.Drawing.Point(3, 8);
            this.txtSelectProcess.Name = "txtSelectProcess";
            this.txtSelectProcess.PlaceholderText = "请选择一个进程或程序";
            this.txtSelectProcess.ReadOnly = true;
            this.txtSelectProcess.Round = true;
            this.txtSelectProcess.Size = new System.Drawing.Size(544, 44);
            this.txtSelectProcess.Status = AntdUI.TType.Error;
            this.txtSelectProcess.TabIndex = 2;
            this.txtSelectProcess.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSelectProcess.TextChanged += new System.EventHandler(this.txtSelectProcess_TextChanged);
            // 
            // txtSearchProcess
            // 
            this.txtSearchProcess.AllowClear = true;
            this.txtSearchProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearchProcess.LocalizationPlaceholderText = "SelectProcessForm.{id}";
            this.txtSearchProcess.Location = new System.Drawing.Point(553, 8);
            this.txtSearchProcess.Name = "txtSearchProcess";
            this.txtSearchProcess.PlaceholderText = "筛选进程列表";
            this.txtSearchProcess.PrefixSvg = "SearchOutlined";
            this.txtSearchProcess.Size = new System.Drawing.Size(544, 44);
            this.txtSearchProcess.TabIndex = 3;
            this.txtSearchProcess.TextChanged += new System.EventHandler(this.txtSearchProcess_TextChanged);
            // 
            // SelectProcessForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1100, 600);
            this.Controls.Add(this.tlpSelectProcess);
            this.Controls.Add(this.pageHeader);
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.Name = "SelectProcessForm";
            this.Resizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WPE x64";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SelectProcessForm_FormClosing);
            this.Load += new System.EventHandler(this.SelectProcessForm_Load);
            this.tlpSelectProcess.ResumeLayout(false);
            this.pProcessList.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.tlpSearch.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.PageHeader pageHeader;
        private System.Windows.Forms.TableLayoutPanel tlpSelectProcess;
        private AntdUI.Panel pProcessList;
        private AntdUI.Table tProcessList;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private AntdUI.Button bCreate;
        private AntdUI.Button bRefresh;
        private AntdUI.Button bInject;
        private System.Windows.Forms.TableLayoutPanel tlpSearch;
        private AntdUI.Input txtSelectProcess;
        private AntdUI.Input txtSearchProcess;
    }
}