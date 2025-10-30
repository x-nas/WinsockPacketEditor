namespace WinsockPacketEditor
{
    partial class ProcessSetting
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
            this.tlpProcessSetting = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bUninstallDriver = new AntdUI.Button();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tlpLoadDriver = new WinsockPacketEditor.TableLayoutPanelEx();
            this.label12 = new AntdUI.Label();
            this.label10 = new AntdUI.Label();
            this.label8 = new AntdUI.Label();
            this.rbWinDivert = new AntdUI.Radio();
            this.rbNFAPI = new AntdUI.Radio();
            this.label1 = new AntdUI.Label();
            this.rbProxifier = new AntdUI.Radio();
            this.label3 = new AntdUI.Label();
            this.transferProcessList = new AntdUI.Transfer();
            this.tlpProcessSetting.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpLoadDriver.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpProcessSetting
            // 
            this.tlpProcessSetting.ColumnCount = 1;
            this.tlpProcessSetting.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProcessSetting.Controls.Add(this.tlpButton, 0, 2);
            this.tlpProcessSetting.Controls.Add(this.tlpLoadDriver, 0, 0);
            this.tlpProcessSetting.Controls.Add(this.transferProcessList, 0, 1);
            this.tlpProcessSetting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpProcessSetting.Location = new System.Drawing.Point(0, 0);
            this.tlpProcessSetting.Margin = new System.Windows.Forms.Padding(0);
            this.tlpProcessSetting.Name = "tlpProcessSetting";
            this.tlpProcessSetting.RowCount = 3;
            this.tlpProcessSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            this.tlpProcessSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProcessSetting.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpProcessSetting.Size = new System.Drawing.Size(700, 700);
            this.tlpProcessSetting.TabIndex = 0;
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 7;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Controls.Add(this.bUninstallDriver, 1, 1);
            this.tlpButton.Controls.Add(this.bSave, 3, 1);
            this.tlpButton.Controls.Add(this.bExit, 5, 1);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 650);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 2;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.Size = new System.Drawing.Size(700, 50);
            this.tlpButton.TabIndex = 5;
            // 
            // bUninstallDriver
            // 
            this.bUninstallDriver.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bUninstallDriver.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bUninstallDriver.IconSvg = "UndoOutlined";
            this.bUninstallDriver.Location = new System.Drawing.Point(224, 11);
            this.bUninstallDriver.Name = "bUninstallDriver";
            this.bUninstallDriver.Size = new System.Drawing.Size(87, 36);
            this.bUninstallDriver.TabIndex = 15;
            this.bUninstallDriver.Text = "卸载驱动";
            this.bUninstallDriver.Type = AntdUI.TTypeMini.Warn;
            this.bUninstallDriver.Click += new System.EventHandler(this.bUninstallDriver_Click);
            // 
            // bSave
            // 
            this.bSave.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.LocalizationText = "Save";
            this.bSave.Location = new System.Drawing.Point(331, 10);
            this.bSave.Margin = new System.Windows.Forms.Padding(2);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(63, 38);
            this.bSave.TabIndex = 0;
            this.bSave.Text = "保存";
            this.bSave.Type = AntdUI.TTypeMini.Primary;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bExit
            // 
            this.bExit.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bExit.BackExtend = "135, #6253E1, #04BEFE";
            this.bExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bExit.IconSvg = "LogoutOutlined";
            this.bExit.LocalizationText = "Cancel";
            this.bExit.Location = new System.Drawing.Point(413, 10);
            this.bExit.Margin = new System.Windows.Forms.Padding(2);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(63, 38);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // tlpLoadDriver
            // 
            this.tlpLoadDriver.ColumnCount = 3;
            this.tlpLoadDriver.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpLoadDriver.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpLoadDriver.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLoadDriver.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpLoadDriver.Controls.Add(this.label12, 1, 3);
            this.tlpLoadDriver.Controls.Add(this.label10, 1, 2);
            this.tlpLoadDriver.Controls.Add(this.label8, 1, 1);
            this.tlpLoadDriver.Controls.Add(this.rbWinDivert, 0, 3);
            this.tlpLoadDriver.Controls.Add(this.rbNFAPI, 0, 2);
            this.tlpLoadDriver.Controls.Add(this.label1, 0, 0);
            this.tlpLoadDriver.Controls.Add(this.rbProxifier, 0, 1);
            this.tlpLoadDriver.Controls.Add(this.label3, 1, 0);
            this.tlpLoadDriver.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpLoadDriver.Location = new System.Drawing.Point(0, 0);
            this.tlpLoadDriver.Margin = new System.Windows.Forms.Padding(0);
            this.tlpLoadDriver.Name = "tlpLoadDriver";
            this.tlpLoadDriver.RowCount = 5;
            this.tlpLoadDriver.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpLoadDriver.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpLoadDriver.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpLoadDriver.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpLoadDriver.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLoadDriver.Size = new System.Drawing.Size(700, 180);
            this.tlpLoadDriver.TabIndex = 6;
            // 
            // label12
            // 
            this.label12.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label12.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label12.ForeColor = System.Drawing.Color.Red;
            this.label12.Location = new System.Drawing.Point(99, 121);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(154, 32);
            this.label12.TabIndex = 13;
            this.label12.Text = "不支持拦截 127.0.0.1 的数据";
            // 
            // label10
            // 
            this.label10.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.ForeColor = System.Drawing.Color.Red;
            this.label10.Location = new System.Drawing.Point(99, 83);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(401, 32);
            this.label10.TabIndex = 11;
            this.label10.Text = "每次限制 1000000 个 TCP 连接和 UDP 套接字，超限后需要重启才能继续\r\n";
            // 
            // label8
            // 
            this.label8.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(99, 45);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(195, 32);
            this.label8.TabIndex = 9;
            this.label8.Text = "不支持 UDP，不支持 32 位操作系统";
            // 
            // rbWinDivert
            // 
            this.rbWinDivert.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbWinDivert.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbWinDivert.Location = new System.Drawing.Point(3, 121);
            this.rbWinDivert.Name = "rbWinDivert";
            this.rbWinDivert.Size = new System.Drawing.Size(90, 32);
            this.rbWinDivert.TabIndex = 3;
            this.rbWinDivert.Text = "WinDivert";
            // 
            // rbNFAPI
            // 
            this.rbNFAPI.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbNFAPI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbNFAPI.Location = new System.Drawing.Point(3, 83);
            this.rbNFAPI.Name = "rbNFAPI";
            this.rbNFAPI.Size = new System.Drawing.Size(68, 32);
            this.rbNFAPI.TabIndex = 2;
            this.rbNFAPI.Text = "NFAPI";
            // 
            // label1
            // 
            this.label1.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 36);
            this.label1.TabIndex = 0;
            this.label1.Text = "驱动类型 :";
            // 
            // rbProxifier
            // 
            this.rbProxifier.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbProxifier.Checked = true;
            this.rbProxifier.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbProxifier.Location = new System.Drawing.Point(3, 45);
            this.rbProxifier.Name = "rbProxifier";
            this.rbProxifier.Size = new System.Drawing.Size(80, 32);
            this.rbProxifier.TabIndex = 1;
            this.rbProxifier.Text = "Proxifier";
            // 
            // label3
            // 
            this.label3.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(99, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(180, 36);
            this.label3.TabIndex = 4;
            this.label3.Text = "三个驱动三选一，选择后不能更改";
            // 
            // transferProcessList
            // 
            this.transferProcessList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.transferProcessList.Location = new System.Drawing.Point(3, 183);
            this.transferProcessList.Name = "transferProcessList";
            this.transferProcessList.Size = new System.Drawing.Size(694, 464);
            this.transferProcessList.TabIndex = 7;
            this.transferProcessList.Text = "transfer1";
            // 
            // ProcessSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpProcessSetting);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "ProcessSetting";
            this.Size = new System.Drawing.Size(700, 700);
            this.Load += new System.EventHandler(this.ProcessSetting_Load);
            this.tlpProcessSetting.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.tlpLoadDriver.ResumeLayout(false);
            this.tlpLoadDriver.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpProcessSetting;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private TableLayoutPanelEx tlpLoadDriver;
        private AntdUI.Label label1;
        private AntdUI.Radio rbProxifier;
        private AntdUI.Radio rbWinDivert;
        private AntdUI.Radio rbNFAPI;
        private AntdUI.Label label3;
        private AntdUI.Label label12;
        private AntdUI.Label label10;
        private AntdUI.Label label8;
        private AntdUI.Transfer transferProcessList;
        private AntdUI.Button bUninstallDriver;
    }
}
