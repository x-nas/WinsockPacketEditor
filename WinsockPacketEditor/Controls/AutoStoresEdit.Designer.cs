namespace WinsockPacketEditor
{
    partial class AutoStoresEdit
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
            this.tlpAutoStoresEdit = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tlpSettingInfo = new WinsockPacketEditor.TableLayoutPanelEx();
            this.lWareHouse = new AntdUI.Label();
            this.lPacketHead = new AntdUI.Label();
            this.txtPacketHead = new AntdUI.Input();
            this.ddlWareHouse = new AntdUI.Select();
            this.tlpAutoStoresEdit.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpSettingInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpAutoStoresEdit
            // 
            this.tlpAutoStoresEdit.ColumnCount = 1;
            this.tlpAutoStoresEdit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAutoStoresEdit.Controls.Add(this.tlpButton, 0, 1);
            this.tlpAutoStoresEdit.Controls.Add(this.tlpSettingInfo, 0, 0);
            this.tlpAutoStoresEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpAutoStoresEdit.Location = new System.Drawing.Point(0, 0);
            this.tlpAutoStoresEdit.Margin = new System.Windows.Forms.Padding(0);
            this.tlpAutoStoresEdit.Name = "tlpAutoStoresEdit";
            this.tlpAutoStoresEdit.RowCount = 2;
            this.tlpAutoStoresEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAutoStoresEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpAutoStoresEdit.Size = new System.Drawing.Size(350, 200);
            this.tlpAutoStoresEdit.TabIndex = 0;
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 5;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Controls.Add(this.bSave, 1, 1);
            this.tlpButton.Controls.Add(this.bExit, 3, 1);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 150);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(350, 50);
            this.tlpButton.TabIndex = 18;
            // 
            // bSave
            // 
            this.bSave.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.LocalizationText = "Save";
            this.bSave.Location = new System.Drawing.Point(103, 6);
            this.bSave.Margin = new System.Windows.Forms.Padding(2);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(63, 37);
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
            this.bExit.Location = new System.Drawing.Point(184, 6);
            this.bExit.Margin = new System.Windows.Forms.Padding(2);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(63, 37);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // tlpSettingInfo
            // 
            this.tlpSettingInfo.ColumnCount = 2;
            this.tlpSettingInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSettingInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSettingInfo.Controls.Add(this.lWareHouse, 0, 2);
            this.tlpSettingInfo.Controls.Add(this.lPacketHead, 0, 1);
            this.tlpSettingInfo.Controls.Add(this.txtPacketHead, 1, 1);
            this.tlpSettingInfo.Controls.Add(this.ddlWareHouse, 1, 2);
            this.tlpSettingInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSettingInfo.Location = new System.Drawing.Point(0, 0);
            this.tlpSettingInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSettingInfo.Name = "tlpSettingInfo";
            this.tlpSettingInfo.RowCount = 4;
            this.tlpSettingInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tlpSettingInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSettingInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSettingInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSettingInfo.Size = new System.Drawing.Size(350, 150);
            this.tlpSettingInfo.TabIndex = 19;
            // 
            // lWareHouse
            // 
            this.lWareHouse.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lWareHouse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lWareHouse.LocalizationText = "WareHouse.Name";
            this.lWareHouse.Location = new System.Drawing.Point(3, 57);
            this.lWareHouse.Name = "lWareHouse";
            this.lWareHouse.Size = new System.Drawing.Size(55, 33);
            this.lWareHouse.TabIndex = 2;
            this.lWareHouse.Text = "选择仓库 :";
            // 
            // lPacketHead
            // 
            this.lPacketHead.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lPacketHead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lPacketHead.LocalizationText = "AutoStores.PacketHead";
            this.lPacketHead.Location = new System.Drawing.Point(3, 18);
            this.lPacketHead.Name = "lPacketHead";
            this.lPacketHead.Size = new System.Drawing.Size(55, 33);
            this.lPacketHead.TabIndex = 0;
            this.lPacketHead.Text = "指定包头 :";
            // 
            // txtPacketHead
            // 
            this.txtPacketHead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPacketHead.LocalizationPlaceholderText = "HexWithSpaces";
            this.txtPacketHead.Location = new System.Drawing.Point(63, 17);
            this.txtPacketHead.Margin = new System.Windows.Forms.Padding(2);
            this.txtPacketHead.Name = "txtPacketHead";
            this.txtPacketHead.PlaceholderText = "请输入十六进制和空格";
            this.txtPacketHead.Size = new System.Drawing.Size(285, 35);
            this.txtPacketHead.TabIndex = 1;
            // 
            // ddlWareHouse
            // 
            this.ddlWareHouse.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddlWareHouse.List = true;
            this.ddlWareHouse.LocalizationPlaceholderText = "PleaseSelect";
            this.ddlWareHouse.Location = new System.Drawing.Point(63, 56);
            this.ddlWareHouse.Margin = new System.Windows.Forms.Padding(2);
            this.ddlWareHouse.Name = "ddlWareHouse";
            this.ddlWareHouse.PlaceholderText = "请选择";
            this.ddlWareHouse.Size = new System.Drawing.Size(285, 35);
            this.ddlWareHouse.TabIndex = 3;
            // 
            // AutoStoresEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpAutoStoresEdit);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AutoStoresEdit";
            this.Size = new System.Drawing.Size(350, 200);
            this.Load += new System.EventHandler(this.AutoStoresEdit_Load);
            this.tlpAutoStoresEdit.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.tlpSettingInfo.ResumeLayout(false);
            this.tlpSettingInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpAutoStoresEdit;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private TableLayoutPanelEx tlpSettingInfo;
        private AntdUI.Label lWareHouse;
        private AntdUI.Label lPacketHead;
        private AntdUI.Input txtPacketHead;
        private AntdUI.Select ddlWareHouse;
    }
}
