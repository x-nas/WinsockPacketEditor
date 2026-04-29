namespace WinsockPacketEditor
{
    partial class NoticeEdit
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
            this.tlpNoticeEdit = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tlpServerInfo = new WinsockPacketEditor.TableLayoutPanelEx();
            this.txtNoticeMore = new AntdUI.Input();
            this.lNoticeMore = new AntdUI.Label();
            this.lNoticeContent = new AntdUI.Label();
            this.lNoticeType = new AntdUI.Label();
            this.lNoticeTitle = new AntdUI.Label();
            this.txtNoticeTitle = new AntdUI.Input();
            this.sNoticeType = new AntdUI.Select();
            this.txtNoticeContent = new AntdUI.Input();
            this.tlpNoticeEdit.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpServerInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpNoticeEdit
            // 
            this.tlpNoticeEdit.ColumnCount = 1;
            this.tlpNoticeEdit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpNoticeEdit.Controls.Add(this.tlpButton, 0, 1);
            this.tlpNoticeEdit.Controls.Add(this.tlpServerInfo, 0, 0);
            this.tlpNoticeEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpNoticeEdit.Location = new System.Drawing.Point(0, 0);
            this.tlpNoticeEdit.Margin = new System.Windows.Forms.Padding(0);
            this.tlpNoticeEdit.Name = "tlpNoticeEdit";
            this.tlpNoticeEdit.RowCount = 2;
            this.tlpNoticeEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpNoticeEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpNoticeEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpNoticeEdit.Size = new System.Drawing.Size(500, 500);
            this.tlpNoticeEdit.TabIndex = 4;
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
            this.tlpButton.Location = new System.Drawing.Point(0, 450);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(500, 50);
            this.tlpButton.TabIndex = 17;
            // 
            // bSave
            // 
            this.bSave.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.LocalizationText = "Save";
            this.bSave.Location = new System.Drawing.Point(178, 6);
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
            this.bExit.Location = new System.Drawing.Point(259, 6);
            this.bExit.Margin = new System.Windows.Forms.Padding(2);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(63, 37);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // tlpServerInfo
            // 
            this.tlpServerInfo.ColumnCount = 4;
            this.tlpServerInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpServerInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpServerInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpServerInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpServerInfo.Controls.Add(this.txtNoticeMore, 2, 3);
            this.tlpServerInfo.Controls.Add(this.lNoticeMore, 1, 3);
            this.tlpServerInfo.Controls.Add(this.lNoticeContent, 1, 2);
            this.tlpServerInfo.Controls.Add(this.lNoticeType, 1, 0);
            this.tlpServerInfo.Controls.Add(this.lNoticeTitle, 1, 1);
            this.tlpServerInfo.Controls.Add(this.txtNoticeTitle, 2, 1);
            this.tlpServerInfo.Controls.Add(this.sNoticeType, 2, 0);
            this.tlpServerInfo.Controls.Add(this.txtNoticeContent, 2, 2);
            this.tlpServerInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpServerInfo.Location = new System.Drawing.Point(0, 0);
            this.tlpServerInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpServerInfo.Name = "tlpServerInfo";
            this.tlpServerInfo.RowCount = 4;
            this.tlpServerInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpServerInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpServerInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpServerInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpServerInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpServerInfo.Size = new System.Drawing.Size(500, 450);
            this.tlpServerInfo.TabIndex = 1;
            // 
            // txtNoticeMore
            // 
            this.txtNoticeMore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNoticeMore.Location = new System.Drawing.Point(125, 412);
            this.txtNoticeMore.Margin = new System.Windows.Forms.Padding(2);
            this.txtNoticeMore.MaxLength = 20;
            this.txtNoticeMore.Name = "txtNoticeMore";
            this.txtNoticeMore.PlaceholderText = "请输入网址";
            this.txtNoticeMore.PrefixText = "http://";
            this.txtNoticeMore.Size = new System.Drawing.Size(353, 36);
            this.txtNoticeMore.TabIndex = 24;
            // 
            // lNoticeMore
            // 
            this.lNoticeMore.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lNoticeMore.LocalizationText = "ServerEditForm.NoticeMore";
            this.lNoticeMore.Location = new System.Drawing.Point(22, 412);
            this.lNoticeMore.Margin = new System.Windows.Forms.Padding(2);
            this.lNoticeMore.Name = "lNoticeMore";
            this.lNoticeMore.Size = new System.Drawing.Size(99, 36);
            this.lNoticeMore.TabIndex = 21;
            this.lNoticeMore.Text = "更多详情地址 :";
            this.lNoticeMore.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lNoticeContent
            // 
            this.lNoticeContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lNoticeContent.LocalizationText = "ServerEditForm.NoticeContent";
            this.lNoticeContent.Location = new System.Drawing.Point(22, 82);
            this.lNoticeContent.Margin = new System.Windows.Forms.Padding(2);
            this.lNoticeContent.Name = "lNoticeContent";
            this.lNoticeContent.Size = new System.Drawing.Size(99, 326);
            this.lNoticeContent.TabIndex = 20;
            this.lNoticeContent.Text = "内容 :";
            this.lNoticeContent.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lNoticeType
            // 
            this.lNoticeType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lNoticeType.LocalizationText = "NoticeEditForm.NoticeType";
            this.lNoticeType.Location = new System.Drawing.Point(22, 2);
            this.lNoticeType.Margin = new System.Windows.Forms.Padding(2);
            this.lNoticeType.Name = "lNoticeType";
            this.lNoticeType.Size = new System.Drawing.Size(99, 36);
            this.lNoticeType.TabIndex = 10;
            this.lNoticeType.Text = "公告类型 :";
            this.lNoticeType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lNoticeTitle
            // 
            this.lNoticeTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lNoticeTitle.LocalizationText = "ServerEditForm.NoticeTitle";
            this.lNoticeTitle.Location = new System.Drawing.Point(22, 42);
            this.lNoticeTitle.Margin = new System.Windows.Forms.Padding(2);
            this.lNoticeTitle.Name = "lNoticeTitle";
            this.lNoticeTitle.Size = new System.Drawing.Size(99, 36);
            this.lNoticeTitle.TabIndex = 11;
            this.lNoticeTitle.Text = "标题 :";
            this.lNoticeTitle.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtNoticeTitle
            // 
            this.txtNoticeTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNoticeTitle.Location = new System.Drawing.Point(125, 42);
            this.txtNoticeTitle.Margin = new System.Windows.Forms.Padding(2);
            this.txtNoticeTitle.MaxLength = 20;
            this.txtNoticeTitle.Name = "txtNoticeTitle";
            this.txtNoticeTitle.PlaceholderText = "请输入公告标题";
            this.txtNoticeTitle.Size = new System.Drawing.Size(353, 36);
            this.txtNoticeTitle.TabIndex = 13;
            this.txtNoticeTitle.TextChanged += new System.EventHandler(this.txtNoticeTitle_TextChanged);
            // 
            // sNoticeType
            // 
            this.sNoticeType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sNoticeType.List = true;
            this.sNoticeType.Location = new System.Drawing.Point(126, 3);
            this.sNoticeType.MaxCount = 5;
            this.sNoticeType.Name = "sNoticeType";
            this.sNoticeType.Size = new System.Drawing.Size(351, 34);
            this.sNoticeType.TabIndex = 25;
            // 
            // txtNoticeContent
            // 
            this.txtNoticeContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNoticeContent.Location = new System.Drawing.Point(126, 83);
            this.txtNoticeContent.Multiline = true;
            this.txtNoticeContent.Name = "txtNoticeContent";
            this.txtNoticeContent.PlaceholderText = "请输入公告内容";
            this.txtNoticeContent.Size = new System.Drawing.Size(351, 324);
            this.txtNoticeContent.TabIndex = 26;
            this.txtNoticeContent.TextChanged += new System.EventHandler(this.txtNoticeContent_TextChanged);
            // 
            // NoticeEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpNoticeEdit);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "NoticeEdit";
            this.Size = new System.Drawing.Size(500, 500);
            this.Load += new System.EventHandler(this.NoticeEdit_Load);
            this.tlpNoticeEdit.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.tlpServerInfo.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpNoticeEdit;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private TableLayoutPanelEx tlpServerInfo;
        private AntdUI.Input txtNoticeMore;
        private AntdUI.Label lNoticeMore;
        private AntdUI.Label lNoticeContent;
        private AntdUI.Label lNoticeType;
        private AntdUI.Label lNoticeTitle;
        private AntdUI.Input txtNoticeTitle;
        private AntdUI.Select sNoticeType;
        private AntdUI.Input txtNoticeContent;
    }
}
