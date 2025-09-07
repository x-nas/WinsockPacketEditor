namespace WinsockPacketEditor
{
    partial class ProcessList
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
            this.tlpSelectProcess = new TableLayoutPanelEx();
            this.pProcessList = new AntdUI.Panel();
            this.tProcessList = new AntdUI.Table();
            this.tlpButton = new TableLayoutPanelEx();
            this.bExit = new AntdUI.Button();
            this.bCreate = new AntdUI.Button();
            this.bRefresh = new AntdUI.Button();
            this.bInject = new AntdUI.Button();
            this.tlpSearch = new TableLayoutPanelEx();
            this.txtSearchProcess = new AntdUI.Input();
            this.txtSelectProcess = new AntdUI.Input();
            this.tlpSelectProcess.SuspendLayout();
            this.pProcessList.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpSearch.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpSelectProcess
            // 
            this.tlpSelectProcess.ColumnCount = 1;
            this.tlpSelectProcess.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSelectProcess.Controls.Add(this.pProcessList, 0, 1);
            this.tlpSelectProcess.Controls.Add(this.tlpButton, 0, 2);
            this.tlpSelectProcess.Controls.Add(this.tlpSearch, 0, 0);
            this.tlpSelectProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSelectProcess.Location = new System.Drawing.Point(0, 0);
            this.tlpSelectProcess.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSelectProcess.Name = "tlpSelectProcess";
            this.tlpSelectProcess.RowCount = 3;
            this.tlpSelectProcess.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpSelectProcess.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSelectProcess.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpSelectProcess.Size = new System.Drawing.Size(800, 500);
            this.tlpSelectProcess.TabIndex = 3;
            // 
            // pProcessList
            // 
            this.pProcessList.Controls.Add(this.tProcessList);
            this.pProcessList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pProcessList.Location = new System.Drawing.Point(3, 63);
            this.pProcessList.Name = "pProcessList";
            this.pProcessList.Size = new System.Drawing.Size(794, 374);
            this.pProcessList.TabIndex = 1;
            this.pProcessList.Text = "panel1";
            // 
            // tProcessList
            // 
            this.tProcessList.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tProcessList.CellImpactHeight = false;
            this.tProcessList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tProcessList.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tProcessList.Gap = 8;
            this.tProcessList.GapCell = 0;
            this.tProcessList.Gaps = new System.Drawing.Size(8, 8);
            this.tProcessList.Location = new System.Drawing.Point(0, 0);
            this.tProcessList.Name = "tProcessList";
            this.tProcessList.Radius = 6;
            this.tProcessList.Size = new System.Drawing.Size(794, 374);
            this.tProcessList.TabIndex = 0;
            this.tProcessList.CellClick += new AntdUI.Table.ClickEventHandler(this.tProcessList_CellClick);
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 9;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Controls.Add(this.bExit, 7, 0);
            this.tlpButton.Controls.Add(this.bCreate, 1, 0);
            this.tlpButton.Controls.Add(this.bRefresh, 3, 0);
            this.tlpButton.Controls.Add(this.bInject, 5, 0);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 440);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 2;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpButton.Size = new System.Drawing.Size(800, 60);
            this.tlpButton.TabIndex = 2;
            // 
            // bExit
            // 
            this.bExit.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bExit.BackExtend = "135, #6253E1, #04BEFE";
            this.bExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bExit.IconSvg = "LogoutOutlined";
            this.bExit.LocalizationText = "Cancel";
            this.bExit.Location = new System.Drawing.Point(542, 3);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(87, 46);
            this.bExit.TabIndex = 3;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // bCreate
            // 
            this.bCreate.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bCreate.BackExtend = "135, #6253E1, #04BEFE";
            this.bCreate.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bCreate.IconSvg = "SelectOutlined";
            this.bCreate.LocalizationText = "ProcessList.{id}";
            this.bCreate.Location = new System.Drawing.Point(171, 3);
            this.bCreate.Name = "bCreate";
            this.bCreate.Size = new System.Drawing.Size(119, 46);
            this.bCreate.TabIndex = 0;
            this.bCreate.Text = "选择程序";
            this.bCreate.Type = AntdUI.TTypeMini.Primary;
            this.bCreate.Click += new System.EventHandler(this.bCreate_Click);
            // 
            // bRefresh
            // 
            this.bRefresh.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bRefresh.BackExtend = "135, #6253E1, #04BEFE";
            this.bRefresh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bRefresh.IconSvg = "ReloadOutlined";
            this.bRefresh.LocalizationText = "ProcessList.{id}";
            this.bRefresh.Location = new System.Drawing.Point(316, 3);
            this.bRefresh.Name = "bRefresh";
            this.bRefresh.Size = new System.Drawing.Size(87, 46);
            this.bRefresh.TabIndex = 1;
            this.bRefresh.Text = "刷新";
            this.bRefresh.Type = AntdUI.TTypeMini.Primary;
            this.bRefresh.Click += new System.EventHandler(this.bRefresh_Click);
            // 
            // bInject
            // 
            this.bInject.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bInject.BackExtend = "135, #6253E1, #04BEFE";
            this.bInject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bInject.IconSvg = "AimOutlined";
            this.bInject.LocalizationText = "ProcessList.{id}";
            this.bInject.Location = new System.Drawing.Point(429, 3);
            this.bInject.Name = "bInject";
            this.bInject.Size = new System.Drawing.Size(87, 46);
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
            this.tlpSearch.Size = new System.Drawing.Size(800, 60);
            this.tlpSearch.TabIndex = 3;
            // 
            // txtSearchProcess
            // 
            this.txtSearchProcess.AllowClear = true;
            this.txtSearchProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSearchProcess.LocalizationPlaceholderText = "ProcessList.{id}";
            this.txtSearchProcess.Location = new System.Drawing.Point(403, 8);
            this.txtSearchProcess.Name = "txtSearchProcess";
            this.txtSearchProcess.PlaceholderText = "筛选进程列表";
            this.txtSearchProcess.PrefixSvg = "SearchOutlined";
            this.txtSearchProcess.Size = new System.Drawing.Size(394, 44);
            this.txtSearchProcess.TabIndex = 3;
            this.txtSearchProcess.TextChanged += new System.EventHandler(this.txtSearchProcess_TextChanged);
            // 
            // txtSelectProcess
            // 
            this.txtSelectProcess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSelectProcess.LocalizationPlaceholderText = "ProcessList.{id}";
            this.txtSelectProcess.Location = new System.Drawing.Point(3, 8);
            this.txtSelectProcess.Name = "txtSelectProcess";
            this.txtSelectProcess.PlaceholderText = "请选择一个进程或程序";
            this.txtSelectProcess.ReadOnly = true;
            this.txtSelectProcess.Round = true;
            this.txtSelectProcess.Size = new System.Drawing.Size(394, 44);
            this.txtSelectProcess.Status = AntdUI.TType.Error;
            this.txtSelectProcess.TabIndex = 2;
            this.txtSelectProcess.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtSelectProcess.TextChanged += new System.EventHandler(this.txtSelectProcess_TextChanged);
            // 
            // ProcessList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpSelectProcess);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "ProcessList";
            this.Size = new System.Drawing.Size(800, 500);
            this.Load += new System.EventHandler(this.ProcessList_Load);
            this.tlpSelectProcess.ResumeLayout(false);
            this.pProcessList.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.tlpSearch.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpSelectProcess;
        private AntdUI.Panel pProcessList;
        private AntdUI.Table tProcessList;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bCreate;
        private AntdUI.Button bRefresh;
        private AntdUI.Button bInject;
        private TableLayoutPanelEx tlpSearch;
        private AntdUI.Input txtSearchProcess;
        private AntdUI.Input txtSelectProcess;
        private AntdUI.Button bExit;
    }
}
