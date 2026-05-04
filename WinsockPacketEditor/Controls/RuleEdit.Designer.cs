namespace WinsockPacketEditor
{
    partial class RuleEdit
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
            this.tlpRuleEdit = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tlpRuleInfo = new WinsockPacketEditor.TableLayoutPanelEx();
            this.ddlRuleAction = new AntdUI.Select();
            this.lRuleAction = new AntdUI.Label();
            this.lRuleType = new AntdUI.Label();
            this.lRuleArgument = new AntdUI.Label();
            this.txtRuleArgument = new AntdUI.Input();
            this.ddlRuleType = new AntdUI.Select();
            this.cbIsEnable = new AntdUI.Checkbox();
            this.tlpRuleEdit.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpRuleInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRuleEdit
            // 
            this.tlpRuleEdit.ColumnCount = 1;
            this.tlpRuleEdit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRuleEdit.Controls.Add(this.tlpButton, 0, 2);
            this.tlpRuleEdit.Controls.Add(this.tlpRuleInfo, 0, 1);
            this.tlpRuleEdit.Controls.Add(this.cbIsEnable, 0, 0);
            this.tlpRuleEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRuleEdit.Location = new System.Drawing.Point(0, 0);
            this.tlpRuleEdit.Margin = new System.Windows.Forms.Padding(0);
            this.tlpRuleEdit.Name = "tlpRuleEdit";
            this.tlpRuleEdit.RowCount = 3;
            this.tlpRuleEdit.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRuleEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRuleEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpRuleEdit.Size = new System.Drawing.Size(600, 250);
            this.tlpRuleEdit.TabIndex = 4;
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
            this.tlpButton.Location = new System.Drawing.Point(0, 200);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(600, 50);
            this.tlpButton.TabIndex = 17;
            // 
            // bSave
            // 
            this.bSave.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.LocalizationText = "Save";
            this.bSave.Location = new System.Drawing.Point(228, 6);
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
            this.bExit.Location = new System.Drawing.Point(309, 6);
            this.bExit.Margin = new System.Windows.Forms.Padding(2);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(63, 37);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // tlpRuleInfo
            // 
            this.tlpRuleInfo.ColumnCount = 4;
            this.tlpRuleInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpRuleInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpRuleInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRuleInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpRuleInfo.Controls.Add(this.ddlRuleAction, 2, 2);
            this.tlpRuleInfo.Controls.Add(this.lRuleAction, 1, 2);
            this.tlpRuleInfo.Controls.Add(this.lRuleType, 1, 0);
            this.tlpRuleInfo.Controls.Add(this.lRuleArgument, 1, 1);
            this.tlpRuleInfo.Controls.Add(this.txtRuleArgument, 2, 1);
            this.tlpRuleInfo.Controls.Add(this.ddlRuleType, 2, 0);
            this.tlpRuleInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRuleInfo.Location = new System.Drawing.Point(0, 36);
            this.tlpRuleInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpRuleInfo.Name = "tlpRuleInfo";
            this.tlpRuleInfo.RowCount = 4;
            this.tlpRuleInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRuleInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRuleInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpRuleInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRuleInfo.Size = new System.Drawing.Size(600, 164);
            this.tlpRuleInfo.TabIndex = 1;
            // 
            // ddlRuleAction
            // 
            this.ddlRuleAction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddlRuleAction.List = true;
            this.ddlRuleAction.LocalizationPlaceholderText = "WPCConfig.RuleList.RuleAction.Input";
            this.ddlRuleAction.Location = new System.Drawing.Point(57, 90);
            this.ddlRuleAction.Margin = new System.Windows.Forms.Padding(2);
            this.ddlRuleAction.Name = "ddlRuleAction";
            this.ddlRuleAction.PlaceholderText = "请选择规则动作";
            this.ddlRuleAction.Size = new System.Drawing.Size(521, 40);
            this.ddlRuleAction.TabIndex = 22;
            // 
            // lRuleAction
            // 
            this.lRuleAction.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lRuleAction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lRuleAction.LocalizationText = "WPCConfig.RuleList.RuleAction";
            this.lRuleAction.Location = new System.Drawing.Point(22, 90);
            this.lRuleAction.Margin = new System.Windows.Forms.Padding(2);
            this.lRuleAction.Name = "lRuleAction";
            this.lRuleAction.Size = new System.Drawing.Size(31, 40);
            this.lRuleAction.TabIndex = 20;
            this.lRuleAction.Text = "动作 :";
            this.lRuleAction.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lRuleType
            // 
            this.lRuleType.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lRuleType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lRuleType.LocalizationText = "WPCConfig.RuleList.RuleType";
            this.lRuleType.Location = new System.Drawing.Point(22, 2);
            this.lRuleType.Margin = new System.Windows.Forms.Padding(2);
            this.lRuleType.Name = "lRuleType";
            this.lRuleType.Size = new System.Drawing.Size(31, 40);
            this.lRuleType.TabIndex = 10;
            this.lRuleType.Text = "类型 :";
            this.lRuleType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lRuleArgument
            // 
            this.lRuleArgument.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lRuleArgument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lRuleArgument.LocalizationText = "WPCConfig.RuleList.RuleArgument";
            this.lRuleArgument.Location = new System.Drawing.Point(22, 46);
            this.lRuleArgument.Margin = new System.Windows.Forms.Padding(2);
            this.lRuleArgument.Name = "lRuleArgument";
            this.lRuleArgument.Size = new System.Drawing.Size(31, 40);
            this.lRuleArgument.TabIndex = 11;
            this.lRuleArgument.Text = "参数 :";
            this.lRuleArgument.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtRuleArgument
            // 
            this.txtRuleArgument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRuleArgument.LocalizationPlaceholderText = "WPCConfig.RuleList.RuleArgument.Input";
            this.txtRuleArgument.Location = new System.Drawing.Point(57, 46);
            this.txtRuleArgument.Margin = new System.Windows.Forms.Padding(2);
            this.txtRuleArgument.MaxLength = 1000;
            this.txtRuleArgument.Name = "txtRuleArgument";
            this.txtRuleArgument.PlaceholderText = "请输入规则参数";
            this.txtRuleArgument.Size = new System.Drawing.Size(521, 40);
            this.txtRuleArgument.TabIndex = 13;
            // 
            // ddlRuleType
            // 
            this.ddlRuleType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddlRuleType.List = true;
            this.ddlRuleType.Location = new System.Drawing.Point(57, 2);
            this.ddlRuleType.Margin = new System.Windows.Forms.Padding(2);
            this.ddlRuleType.Name = "ddlRuleType";
            this.ddlRuleType.PlaceholderText = "";
            this.ddlRuleType.Size = new System.Drawing.Size(521, 40);
            this.ddlRuleType.TabIndex = 21;
            // 
            // cbIsEnable
            // 
            this.cbIsEnable.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbIsEnable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbIsEnable.LocalizationText = "Enable";
            this.cbIsEnable.Location = new System.Drawing.Point(2, 2);
            this.cbIsEnable.Margin = new System.Windows.Forms.Padding(2);
            this.cbIsEnable.Name = "cbIsEnable";
            this.cbIsEnable.Size = new System.Drawing.Size(56, 32);
            this.cbIsEnable.TabIndex = 18;
            this.cbIsEnable.Text = "启用";
            this.cbIsEnable.CheckedChanged += new AntdUI.BoolEventHandler(this.cbIsEnable_CheckedChanged);
            // 
            // RuleEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpRuleEdit);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "RuleEdit";
            this.Size = new System.Drawing.Size(600, 250);
            this.Load += new System.EventHandler(this.RuleEdit_Load);
            this.tlpRuleEdit.ResumeLayout(false);
            this.tlpRuleEdit.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.tlpRuleInfo.ResumeLayout(false);
            this.tlpRuleInfo.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpRuleEdit;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private TableLayoutPanelEx tlpRuleInfo;
        private AntdUI.Label lRuleAction;
        private AntdUI.Label lRuleType;
        private AntdUI.Label lRuleArgument;
        private AntdUI.Input txtRuleArgument;
        private AntdUI.Checkbox cbIsEnable;
        private AntdUI.Select ddlRuleType;
        private AntdUI.Select ddlRuleAction;
    }
}
