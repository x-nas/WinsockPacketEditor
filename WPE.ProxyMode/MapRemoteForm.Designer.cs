namespace WPE.ProxyMode
{
    partial class MapRemoteForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapRemoteForm));
            this.tlpMapRemote = new System.Windows.Forms.TableLayoutPanel();
            this.tlpMapTo = new System.Windows.Forms.TableLayoutPanel();
            this.lPathTo = new AntdUI.Label();
            this.lPortTo = new AntdUI.Label();
            this.lHostTo = new AntdUI.Label();
            this.lProtocolTo = new AntdUI.Label();
            this.ddlProtocolTo = new AntdUI.Select();
            this.txtHostTo = new AntdUI.Input();
            this.nudPortTo = new AntdUI.InputNumber();
            this.txtPathTo = new AntdUI.Input();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.dMapTo = new AntdUI.Divider();
            this.tlpMapFrom = new System.Windows.Forms.TableLayoutPanel();
            this.lPathFrom = new AntdUI.Label();
            this.lPortFrom = new AntdUI.Label();
            this.lHostFrom = new AntdUI.Label();
            this.lProtocolFrom = new AntdUI.Label();
            this.ddlProtocolFrom = new AntdUI.Select();
            this.txtHostFrom = new AntdUI.Input();
            this.nudPortFrom = new AntdUI.InputNumber();
            this.txtPathFrom = new AntdUI.Input();
            this.dMapFrom = new AntdUI.Divider();
            this.tlpMapRemote.SuspendLayout();
            this.tlpMapTo.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpMapFrom.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpMapRemote
            // 
            this.tlpMapRemote.ColumnCount = 1;
            this.tlpMapRemote.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMapRemote.Controls.Add(this.tlpMapTo, 0, 3);
            this.tlpMapRemote.Controls.Add(this.tlpButton, 0, 5);
            this.tlpMapRemote.Controls.Add(this.dMapTo, 0, 2);
            this.tlpMapRemote.Controls.Add(this.tlpMapFrom, 0, 1);
            this.tlpMapRemote.Controls.Add(this.dMapFrom, 0, 0);
            this.tlpMapRemote.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMapRemote.Location = new System.Drawing.Point(0, 0);
            this.tlpMapRemote.Name = "tlpMapRemote";
            this.tlpMapRemote.RowCount = 6;
            this.tlpMapRemote.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMapRemote.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tlpMapRemote.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMapRemote.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMapRemote.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMapRemote.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpMapRemote.Size = new System.Drawing.Size(484, 761);
            this.tlpMapRemote.TabIndex = 2;
            // 
            // tlpMapTo
            // 
            this.tlpMapTo.ColumnCount = 2;
            this.tlpMapTo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMapTo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMapTo.Controls.Add(this.lPathTo, 0, 3);
            this.tlpMapTo.Controls.Add(this.lPortTo, 0, 2);
            this.tlpMapTo.Controls.Add(this.lHostTo, 0, 1);
            this.tlpMapTo.Controls.Add(this.lProtocolTo, 0, 0);
            this.tlpMapTo.Controls.Add(this.ddlProtocolTo, 1, 0);
            this.tlpMapTo.Controls.Add(this.txtHostTo, 1, 1);
            this.tlpMapTo.Controls.Add(this.nudPortTo, 1, 2);
            this.tlpMapTo.Controls.Add(this.txtPathTo, 1, 3);
            this.tlpMapTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMapTo.Location = new System.Drawing.Point(0, 308);
            this.tlpMapTo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMapTo.Name = "tlpMapTo";
            this.tlpMapTo.Padding = new System.Windows.Forms.Padding(3);
            this.tlpMapTo.RowCount = 5;
            this.tlpMapTo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMapTo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMapTo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMapTo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMapTo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMapTo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMapTo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMapTo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMapTo.Size = new System.Drawing.Size(484, 250);
            this.tlpMapTo.TabIndex = 18;
            // 
            // lPathTo
            // 
            this.lPathTo.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lPathTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lPathTo.Location = new System.Drawing.Point(6, 159);
            this.lPathTo.Name = "lPathTo";
            this.lPathTo.Size = new System.Drawing.Size(32, 45);
            this.lPathTo.TabIndex = 16;
            this.lPathTo.Text = "路径";
            // 
            // lPortTo
            // 
            this.lPortTo.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lPortTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lPortTo.Location = new System.Drawing.Point(6, 108);
            this.lPortTo.Name = "lPortTo";
            this.lPortTo.Size = new System.Drawing.Size(32, 45);
            this.lPortTo.TabIndex = 14;
            this.lPortTo.Text = "端口";
            // 
            // lHostTo
            // 
            this.lHostTo.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lHostTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lHostTo.Location = new System.Drawing.Point(6, 57);
            this.lHostTo.Name = "lHostTo";
            this.lHostTo.Size = new System.Drawing.Size(64, 45);
            this.lHostTo.TabIndex = 12;
            this.lHostTo.Text = "主机地址";
            // 
            // lProtocolTo
            // 
            this.lProtocolTo.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lProtocolTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProtocolTo.Location = new System.Drawing.Point(6, 6);
            this.lProtocolTo.Name = "lProtocolTo";
            this.lProtocolTo.Size = new System.Drawing.Size(32, 45);
            this.lProtocolTo.TabIndex = 10;
            this.lProtocolTo.Text = "协议";
            // 
            // ddlProtocolTo
            // 
            this.ddlProtocolTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddlProtocolTo.Items.AddRange(new object[] {
            "http",
            "https"});
            this.ddlProtocolTo.List = true;
            this.ddlProtocolTo.Location = new System.Drawing.Point(76, 6);
            this.ddlProtocolTo.Name = "ddlProtocolTo";
            this.ddlProtocolTo.PlaceholderText = "请选择";
            this.ddlProtocolTo.Size = new System.Drawing.Size(402, 45);
            this.ddlProtocolTo.TabIndex = 11;
            this.ddlProtocolTo.SelectedIndexChanged += new AntdUI.IntEventHandler(this.ddlProtocolTo_SelectedIndexChanged);
            // 
            // txtHostTo
            // 
            this.txtHostTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHostTo.Location = new System.Drawing.Point(76, 57);
            this.txtHostTo.Name = "txtHostTo";
            this.txtHostTo.PrefixText = "http://";
            this.txtHostTo.Size = new System.Drawing.Size(402, 45);
            this.txtHostTo.TabIndex = 13;
            this.txtHostTo.TextChanged += new System.EventHandler(this.txtHostTo_TextChanged);
            // 
            // nudPortTo
            // 
            this.nudPortTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudPortTo.Location = new System.Drawing.Point(76, 108);
            this.nudPortTo.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPortTo.Name = "nudPortTo";
            this.nudPortTo.SelectionStart = 2;
            this.nudPortTo.Size = new System.Drawing.Size(402, 45);
            this.nudPortTo.TabIndex = 15;
            this.nudPortTo.Text = "80";
            this.nudPortTo.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // txtPathTo
            // 
            this.txtPathTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPathTo.Location = new System.Drawing.Point(76, 159);
            this.txtPathTo.Name = "txtPathTo";
            this.txtPathTo.PlaceholderText = "请填写映射路径";
            this.txtPathTo.Size = new System.Drawing.Size(402, 45);
            this.txtPathTo.TabIndex = 17;
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 5;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Controls.Add(this.bSave, 1, 1);
            this.tlpButton.Controls.Add(this.bExit, 3, 1);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 701);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(484, 60);
            this.tlpButton.TabIndex = 17;
            // 
            // bSave
            // 
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.Location = new System.Drawing.Point(115, 7);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(114, 46);
            this.bSave.TabIndex = 0;
            this.bSave.Text = "保存";
            this.bSave.Type = AntdUI.TTypeMini.Primary;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bExit
            // 
            this.bExit.BackExtend = "135, #6253E1, #04BEFE";
            this.bExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bExit.IconSvg = "LogoutOutlined";
            this.bExit.Location = new System.Drawing.Point(255, 7);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(114, 46);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // dMapTo
            // 
            this.dMapTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dMapTo.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dMapTo.Location = new System.Drawing.Point(3, 282);
            this.dMapTo.Name = "dMapTo";
            this.dMapTo.Orientation = AntdUI.TOrientation.Left;
            this.dMapTo.Size = new System.Drawing.Size(478, 23);
            this.dMapTo.TabIndex = 5;
            this.dMapTo.Text = "映射地址";
            // 
            // tlpMapFrom
            // 
            this.tlpMapFrom.ColumnCount = 2;
            this.tlpMapFrom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMapFrom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMapFrom.Controls.Add(this.lPathFrom, 0, 3);
            this.tlpMapFrom.Controls.Add(this.lPortFrom, 0, 2);
            this.tlpMapFrom.Controls.Add(this.lHostFrom, 0, 1);
            this.tlpMapFrom.Controls.Add(this.lProtocolFrom, 0, 0);
            this.tlpMapFrom.Controls.Add(this.ddlProtocolFrom, 1, 0);
            this.tlpMapFrom.Controls.Add(this.txtHostFrom, 1, 1);
            this.tlpMapFrom.Controls.Add(this.nudPortFrom, 1, 2);
            this.tlpMapFrom.Controls.Add(this.txtPathFrom, 1, 3);
            this.tlpMapFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMapFrom.Location = new System.Drawing.Point(0, 29);
            this.tlpMapFrom.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMapFrom.Name = "tlpMapFrom";
            this.tlpMapFrom.Padding = new System.Windows.Forms.Padding(3);
            this.tlpMapFrom.RowCount = 5;
            this.tlpMapFrom.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMapFrom.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMapFrom.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMapFrom.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMapFrom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMapFrom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMapFrom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMapFrom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMapFrom.Size = new System.Drawing.Size(484, 250);
            this.tlpMapFrom.TabIndex = 4;
            // 
            // lPathFrom
            // 
            this.lPathFrom.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lPathFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lPathFrom.Location = new System.Drawing.Point(6, 159);
            this.lPathFrom.Name = "lPathFrom";
            this.lPathFrom.Size = new System.Drawing.Size(32, 45);
            this.lPathFrom.TabIndex = 16;
            this.lPathFrom.Text = "路径";
            // 
            // lPortFrom
            // 
            this.lPortFrom.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lPortFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lPortFrom.Location = new System.Drawing.Point(6, 108);
            this.lPortFrom.Name = "lPortFrom";
            this.lPortFrom.Size = new System.Drawing.Size(32, 45);
            this.lPortFrom.TabIndex = 14;
            this.lPortFrom.Text = "端口";
            // 
            // lHostFrom
            // 
            this.lHostFrom.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lHostFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lHostFrom.Location = new System.Drawing.Point(6, 57);
            this.lHostFrom.Name = "lHostFrom";
            this.lHostFrom.Size = new System.Drawing.Size(64, 45);
            this.lHostFrom.TabIndex = 12;
            this.lHostFrom.Text = "主机地址";
            // 
            // lProtocolFrom
            // 
            this.lProtocolFrom.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lProtocolFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lProtocolFrom.Location = new System.Drawing.Point(6, 6);
            this.lProtocolFrom.Name = "lProtocolFrom";
            this.lProtocolFrom.Size = new System.Drawing.Size(32, 45);
            this.lProtocolFrom.TabIndex = 10;
            this.lProtocolFrom.Text = "协议";
            // 
            // ddlProtocolFrom
            // 
            this.ddlProtocolFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddlProtocolFrom.Items.AddRange(new object[] {
            "http"});
            this.ddlProtocolFrom.List = true;
            this.ddlProtocolFrom.Location = new System.Drawing.Point(76, 6);
            this.ddlProtocolFrom.Name = "ddlProtocolFrom";
            this.ddlProtocolFrom.PlaceholderText = "请选择";
            this.ddlProtocolFrom.Size = new System.Drawing.Size(402, 45);
            this.ddlProtocolFrom.TabIndex = 11;
            // 
            // txtHostFrom
            // 
            this.txtHostFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtHostFrom.Location = new System.Drawing.Point(76, 57);
            this.txtHostFrom.Name = "txtHostFrom";
            this.txtHostFrom.PrefixText = "http://";
            this.txtHostFrom.Size = new System.Drawing.Size(402, 45);
            this.txtHostFrom.TabIndex = 13;
            this.txtHostFrom.TextChanged += new System.EventHandler(this.txtHostFrom_TextChanged);
            // 
            // nudPortFrom
            // 
            this.nudPortFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudPortFrom.Location = new System.Drawing.Point(76, 108);
            this.nudPortFrom.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPortFrom.Name = "nudPortFrom";
            this.nudPortFrom.SelectionStart = 2;
            this.nudPortFrom.Size = new System.Drawing.Size(402, 45);
            this.nudPortFrom.TabIndex = 15;
            this.nudPortFrom.Text = "80";
            this.nudPortFrom.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // txtPathFrom
            // 
            this.txtPathFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPathFrom.Location = new System.Drawing.Point(76, 159);
            this.txtPathFrom.Name = "txtPathFrom";
            this.txtPathFrom.PlaceholderText = "请填写请求路径";
            this.txtPathFrom.Size = new System.Drawing.Size(402, 45);
            this.txtPathFrom.TabIndex = 17;
            // 
            // dMapFrom
            // 
            this.dMapFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dMapFrom.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dMapFrom.Location = new System.Drawing.Point(3, 3);
            this.dMapFrom.Name = "dMapFrom";
            this.dMapFrom.Orientation = AntdUI.TOrientation.Left;
            this.dMapFrom.Size = new System.Drawing.Size(478, 23);
            this.dMapFrom.TabIndex = 3;
            this.dMapFrom.Text = "请求地址";
            // 
            // MapRemoteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 761);
            this.Controls.Add(this.tlpMapRemote);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "MapRemoteForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MapRemoteForm";
            this.Load += new System.EventHandler(this.MapRemoteForm_Load);
            this.tlpMapRemote.ResumeLayout(false);
            this.tlpMapTo.ResumeLayout(false);
            this.tlpMapTo.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.tlpMapFrom.ResumeLayout(false);
            this.tlpMapFrom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMapRemote;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private AntdUI.Divider dMapTo;
        private System.Windows.Forms.TableLayoutPanel tlpMapFrom;
        private AntdUI.Label lPathFrom;
        private AntdUI.Label lPortFrom;
        private AntdUI.Label lHostFrom;
        private AntdUI.Label lProtocolFrom;
        private AntdUI.Select ddlProtocolFrom;
        private AntdUI.Input txtHostFrom;
        private AntdUI.InputNumber nudPortFrom;
        private AntdUI.Input txtPathFrom;
        private AntdUI.Divider dMapFrom;
        private System.Windows.Forms.TableLayoutPanel tlpMapTo;
        private AntdUI.Label lPathTo;
        private AntdUI.Label lPortTo;
        private AntdUI.Label lHostTo;
        private AntdUI.Label lProtocolTo;
        private AntdUI.Select ddlProtocolTo;
        private AntdUI.Input txtHostTo;
        private AntdUI.InputNumber nudPortTo;
        private AntdUI.Input txtPathTo;
    }
}