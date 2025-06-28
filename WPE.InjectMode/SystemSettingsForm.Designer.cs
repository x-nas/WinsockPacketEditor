namespace WPE.InjectMode
{
    partial class SystemSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SystemSettingsForm));
            this.tlpSystemSettings = new System.Windows.Forms.TableLayoutPanel();
            this.dWorkMode = new AntdUI.Divider();
            this.dListExecute = new AntdUI.Divider();
            this.dFilterSet = new AntdUI.Divider();
            this.dFilterActionColor = new AntdUI.Divider();
            this.cbSpeedMode = new AntdUI.Checkbox();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.rbListExecute_Together = new AntdUI.Radio();
            this.tlpListExecute = new System.Windows.Forms.TableLayoutPanel();
            this.rbListExecute_Sequence = new AntdUI.Radio();
            this.tlpFilterSet = new System.Windows.Forms.TableLayoutPanel();
            this.rbFilterSet_Sequence = new AntdUI.Radio();
            this.rbFilterSet_Priority = new AntdUI.Radio();
            this.tlpFilterActionColor = new System.Windows.Forms.TableLayoutPanel();
            this.lFAColor_Replace = new AntdUI.Label();
            this.lFAColor_Intercept = new AntdUI.Label();
            this.lFAColor_Change = new AntdUI.Label();
            this.lFAColor_Other = new AntdUI.Label();
            this.tlpSystemSettings.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpListExecute.SuspendLayout();
            this.tlpFilterSet.SuspendLayout();
            this.tlpFilterActionColor.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpSystemSettings
            // 
            this.tlpSystemSettings.AutoSize = true;
            this.tlpSystemSettings.ColumnCount = 1;
            this.tlpSystemSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSystemSettings.Controls.Add(this.tlpFilterActionColor, 0, 8);
            this.tlpSystemSettings.Controls.Add(this.tlpFilterSet, 0, 6);
            this.tlpSystemSettings.Controls.Add(this.tlpButton, 0, 9);
            this.tlpSystemSettings.Controls.Add(this.cbSpeedMode, 0, 1);
            this.tlpSystemSettings.Controls.Add(this.dWorkMode, 0, 0);
            this.tlpSystemSettings.Controls.Add(this.dListExecute, 0, 3);
            this.tlpSystemSettings.Controls.Add(this.dFilterSet, 0, 5);
            this.tlpSystemSettings.Controls.Add(this.dFilterActionColor, 0, 7);
            this.tlpSystemSettings.Controls.Add(this.tlpListExecute, 0, 4);
            this.tlpSystemSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSystemSettings.Location = new System.Drawing.Point(0, 0);
            this.tlpSystemSettings.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSystemSettings.Name = "tlpSystemSettings";
            this.tlpSystemSettings.RowCount = 10;
            this.tlpSystemSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSystemSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSystemSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpSystemSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSystemSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tlpSystemSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSystemSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tlpSystemSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSystemSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSystemSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpSystemSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpSystemSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpSystemSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpSystemSettings.Size = new System.Drawing.Size(484, 461);
            this.tlpSystemSettings.TabIndex = 0;
            // 
            // dWorkMode
            // 
            this.dWorkMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dWorkMode.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dWorkMode.Location = new System.Drawing.Point(3, 3);
            this.dWorkMode.Name = "dWorkMode";
            this.dWorkMode.Orientation = AntdUI.TOrientation.Left;
            this.dWorkMode.Size = new System.Drawing.Size(478, 23);
            this.dWorkMode.TabIndex = 1;
            this.dWorkMode.Text = "工作模式";
            // 
            // dListExecute
            // 
            this.dListExecute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dListExecute.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dListExecute.Location = new System.Drawing.Point(3, 100);
            this.dListExecute.Name = "dListExecute";
            this.dListExecute.Orientation = AntdUI.TOrientation.Left;
            this.dListExecute.Size = new System.Drawing.Size(478, 23);
            this.dListExecute.TabIndex = 2;
            this.dListExecute.Text = "列表执行模式";
            // 
            // dFilterSet
            // 
            this.dFilterSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dFilterSet.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dFilterSet.Location = new System.Drawing.Point(3, 199);
            this.dFilterSet.Name = "dFilterSet";
            this.dFilterSet.Orientation = AntdUI.TOrientation.Left;
            this.dFilterSet.Size = new System.Drawing.Size(478, 23);
            this.dFilterSet.TabIndex = 3;
            this.dFilterSet.Text = "滤镜执行模式";
            // 
            // dFilterActionColor
            // 
            this.dFilterActionColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dFilterActionColor.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dFilterActionColor.Location = new System.Drawing.Point(3, 298);
            this.dFilterActionColor.Name = "dFilterActionColor";
            this.dFilterActionColor.Orientation = AntdUI.TOrientation.Left;
            this.dFilterActionColor.Size = new System.Drawing.Size(478, 23);
            this.dFilterActionColor.TabIndex = 4;
            this.dFilterActionColor.Text = "滤镜动作";
            // 
            // cbSpeedMode
            // 
            this.cbSpeedMode.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbSpeedMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbSpeedMode.Location = new System.Drawing.Point(3, 32);
            this.cbSpeedMode.Name = "cbSpeedMode";
            this.cbSpeedMode.Size = new System.Drawing.Size(106, 42);
            this.cbSpeedMode.TabIndex = 6;
            this.cbSpeedMode.Text = "极速模式";
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
            this.tlpButton.Location = new System.Drawing.Point(0, 401);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(484, 60);
            this.tlpButton.TabIndex = 16;
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
            // rbListExecute_Together
            // 
            this.rbListExecute_Together.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbListExecute_Together.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbListExecute_Together.Location = new System.Drawing.Point(3, 3);
            this.rbListExecute_Together.Name = "rbListExecute_Together";
            this.rbListExecute_Together.Size = new System.Drawing.Size(106, 42);
            this.rbListExecute_Together.TabIndex = 1;
            this.rbListExecute_Together.Text = "同时执行";
            // 
            // tlpListExecute
            // 
            this.tlpListExecute.ColumnCount = 2;
            this.tlpListExecute.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpListExecute.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpListExecute.Controls.Add(this.rbListExecute_Sequence, 1, 0);
            this.tlpListExecute.Controls.Add(this.rbListExecute_Together, 0, 0);
            this.tlpListExecute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpListExecute.Location = new System.Drawing.Point(0, 126);
            this.tlpListExecute.Margin = new System.Windows.Forms.Padding(0);
            this.tlpListExecute.Name = "tlpListExecute";
            this.tlpListExecute.RowCount = 2;
            this.tlpListExecute.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpListExecute.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpListExecute.Size = new System.Drawing.Size(484, 70);
            this.tlpListExecute.TabIndex = 15;
            // 
            // rbListExecute_Sequence
            // 
            this.rbListExecute_Sequence.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbListExecute_Sequence.Checked = true;
            this.rbListExecute_Sequence.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbListExecute_Sequence.Location = new System.Drawing.Point(245, 3);
            this.rbListExecute_Sequence.Name = "rbListExecute_Sequence";
            this.rbListExecute_Sequence.Size = new System.Drawing.Size(122, 42);
            this.rbListExecute_Sequence.TabIndex = 2;
            this.rbListExecute_Sequence.Text = "按顺序执行";
            // 
            // tlpFilterSet
            // 
            this.tlpFilterSet.ColumnCount = 2;
            this.tlpFilterSet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpFilterSet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpFilterSet.Controls.Add(this.rbFilterSet_Sequence, 1, 0);
            this.tlpFilterSet.Controls.Add(this.rbFilterSet_Priority, 0, 0);
            this.tlpFilterSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterSet.Location = new System.Drawing.Point(0, 225);
            this.tlpFilterSet.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFilterSet.Name = "tlpFilterSet";
            this.tlpFilterSet.RowCount = 2;
            this.tlpFilterSet.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterSet.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterSet.Size = new System.Drawing.Size(484, 70);
            this.tlpFilterSet.TabIndex = 17;
            // 
            // rbFilterSet_Sequence
            // 
            this.rbFilterSet_Sequence.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbFilterSet_Sequence.Checked = true;
            this.rbFilterSet_Sequence.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbFilterSet_Sequence.Location = new System.Drawing.Point(245, 3);
            this.rbFilterSet_Sequence.Name = "rbFilterSet_Sequence";
            this.rbFilterSet_Sequence.Size = new System.Drawing.Size(122, 42);
            this.rbFilterSet_Sequence.TabIndex = 2;
            this.rbFilterSet_Sequence.Text = "按顺序执行";
            // 
            // rbFilterSet_Priority
            // 
            this.rbFilterSet_Priority.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbFilterSet_Priority.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbFilterSet_Priority.Location = new System.Drawing.Point(3, 3);
            this.rbFilterSet_Priority.Name = "rbFilterSet_Priority";
            this.rbFilterSet_Priority.Size = new System.Drawing.Size(106, 42);
            this.rbFilterSet_Priority.TabIndex = 1;
            this.rbFilterSet_Priority.Text = "优先原则";
            // 
            // tlpFilterActionColor
            // 
            this.tlpFilterActionColor.ColumnCount = 9;
            this.tlpFilterActionColor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpFilterActionColor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tlpFilterActionColor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpFilterActionColor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tlpFilterActionColor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpFilterActionColor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tlpFilterActionColor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpFilterActionColor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tlpFilterActionColor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpFilterActionColor.Controls.Add(this.lFAColor_Other, 7, 1);
            this.tlpFilterActionColor.Controls.Add(this.lFAColor_Change, 5, 1);
            this.tlpFilterActionColor.Controls.Add(this.lFAColor_Intercept, 3, 1);
            this.tlpFilterActionColor.Controls.Add(this.lFAColor_Replace, 1, 1);
            this.tlpFilterActionColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterActionColor.Location = new System.Drawing.Point(0, 324);
            this.tlpFilterActionColor.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFilterActionColor.Name = "tlpFilterActionColor";
            this.tlpFilterActionColor.RowCount = 3;
            this.tlpFilterActionColor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tlpFilterActionColor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tlpFilterActionColor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterActionColor.Size = new System.Drawing.Size(484, 77);
            this.tlpFilterActionColor.TabIndex = 18;
            // 
            // lFAColor_Replace
            // 
            this.lFAColor_Replace.BackColor = System.Drawing.Color.Goldenrod;
            this.lFAColor_Replace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lFAColor_Replace.ForeColor = System.Drawing.Color.Black;
            this.lFAColor_Replace.Location = new System.Drawing.Point(35, 18);
            this.lFAColor_Replace.Name = "lFAColor_Replace";
            this.lFAColor_Replace.Size = new System.Drawing.Size(84, 39);
            this.lFAColor_Replace.TabIndex = 0;
            this.lFAColor_Replace.Text = "替换";
            this.lFAColor_Replace.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lFAColor_Intercept
            // 
            this.lFAColor_Intercept.BackColor = System.Drawing.Color.DarkRed;
            this.lFAColor_Intercept.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lFAColor_Intercept.ForeColor = System.Drawing.Color.White;
            this.lFAColor_Intercept.Location = new System.Drawing.Point(145, 18);
            this.lFAColor_Intercept.Name = "lFAColor_Intercept";
            this.lFAColor_Intercept.Size = new System.Drawing.Size(84, 39);
            this.lFAColor_Intercept.TabIndex = 2;
            this.lFAColor_Intercept.Text = "拦截";
            this.lFAColor_Intercept.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lFAColor_Change
            // 
            this.lFAColor_Change.BackColor = System.Drawing.Color.DodgerBlue;
            this.lFAColor_Change.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lFAColor_Change.ForeColor = System.Drawing.Color.Black;
            this.lFAColor_Change.Location = new System.Drawing.Point(255, 18);
            this.lFAColor_Change.Name = "lFAColor_Change";
            this.lFAColor_Change.Size = new System.Drawing.Size(84, 39);
            this.lFAColor_Change.TabIndex = 3;
            this.lFAColor_Change.Text = "换包";
            this.lFAColor_Change.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lFAColor_Other
            // 
            this.lFAColor_Other.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.lFAColor_Other.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lFAColor_Other.ForeColor = System.Drawing.Color.LimeGreen;
            this.lFAColor_Other.Location = new System.Drawing.Point(365, 18);
            this.lFAColor_Other.Name = "lFAColor_Other";
            this.lFAColor_Other.Size = new System.Drawing.Size(84, 39);
            this.lFAColor_Other.TabIndex = 4;
            this.lFAColor_Other.Text = "其它";
            this.lFAColor_Other.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SystemSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 461);
            this.Controls.Add(this.tlpSystemSettings);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5, 5, 5, 5);
            this.Name = "SystemSettingsForm";
            this.Text = "SystemSettingsForm";
            this.Load += new System.EventHandler(this.SystemSettingsForm_Load);
            this.tlpSystemSettings.ResumeLayout(false);
            this.tlpSystemSettings.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.tlpListExecute.ResumeLayout(false);
            this.tlpListExecute.PerformLayout();
            this.tlpFilterSet.ResumeLayout(false);
            this.tlpFilterSet.PerformLayout();
            this.tlpFilterActionColor.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpSystemSettings;
        private AntdUI.Divider dWorkMode;
        private AntdUI.Divider dListExecute;
        private AntdUI.Divider dFilterSet;
        private AntdUI.Divider dFilterActionColor;
        private AntdUI.Checkbox cbSpeedMode;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private System.Windows.Forms.TableLayoutPanel tlpFilterSet;
        private AntdUI.Radio rbFilterSet_Sequence;
        private AntdUI.Radio rbFilterSet_Priority;
        private System.Windows.Forms.TableLayoutPanel tlpListExecute;
        private AntdUI.Radio rbListExecute_Sequence;
        private AntdUI.Radio rbListExecute_Together;
        private System.Windows.Forms.TableLayoutPanel tlpFilterActionColor;
        private AntdUI.Label lFAColor_Other;
        private AntdUI.Label lFAColor_Change;
        private AntdUI.Label lFAColor_Intercept;
        private AntdUI.Label lFAColor_Replace;
    }
}