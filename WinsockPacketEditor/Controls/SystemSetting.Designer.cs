namespace WinsockPacketEditor
{
    partial class SystemSetting
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
            this.tlpSystemSettings = new System.Windows.Forms.TableLayoutPanel();
            this.dFloatButton = new AntdUI.Divider();
            this.tlpFilterActionColor = new System.Windows.Forms.TableLayoutPanel();
            this.lForeColor = new AntdUI.Label();
            this.lBackColor = new AntdUI.Label();
            this.cChange_ForeColor = new AntdUI.ColorPicker();
            this.cChange_BackColor = new AntdUI.ColorPicker();
            this.lChange = new AntdUI.Label();
            this.cIntercept_ForeColor = new AntdUI.ColorPicker();
            this.cIntercept_BackColor = new AntdUI.ColorPicker();
            this.lIntercept = new AntdUI.Label();
            this.cRepalce_ForeColor = new AntdUI.ColorPicker();
            this.lReplace = new AntdUI.Label();
            this.cRepalce_BackColor = new AntdUI.ColorPicker();
            this.tlpFilterSet = new System.Windows.Forms.TableLayoutPanel();
            this.rbFilterSet_Sequence = new AntdUI.Radio();
            this.rbFilterSet_Priority = new AntdUI.Radio();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.cbSpeedMode = new AntdUI.Checkbox();
            this.dWorkMode = new AntdUI.Divider();
            this.dListExecute = new AntdUI.Divider();
            this.dFilterSet = new AntdUI.Divider();
            this.dFilterActionColor = new AntdUI.Divider();
            this.tlpListExecute = new System.Windows.Forms.TableLayoutPanel();
            this.rbListExecute_Sequence = new AntdUI.Radio();
            this.rbListExecute_Together = new AntdUI.Radio();
            this.switchFloatButton = new AntdUI.Switch();
            this.bReplaceReset = new AntdUI.Button();
            this.bInterceptReset = new AntdUI.Button();
            this.bChangeReset = new AntdUI.Button();
            this.tlpSystemSettings.SuspendLayout();
            this.tlpFilterActionColor.SuspendLayout();
            this.tlpFilterSet.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpListExecute.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpSystemSettings
            // 
            this.tlpSystemSettings.AutoSize = true;
            this.tlpSystemSettings.ColumnCount = 1;
            this.tlpSystemSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSystemSettings.Controls.Add(this.dFloatButton, 0, 3);
            this.tlpSystemSettings.Controls.Add(this.tlpFilterActionColor, 0, 11);
            this.tlpSystemSettings.Controls.Add(this.tlpFilterSet, 0, 9);
            this.tlpSystemSettings.Controls.Add(this.tlpButton, 0, 12);
            this.tlpSystemSettings.Controls.Add(this.cbSpeedMode, 0, 1);
            this.tlpSystemSettings.Controls.Add(this.dWorkMode, 0, 0);
            this.tlpSystemSettings.Controls.Add(this.dListExecute, 0, 6);
            this.tlpSystemSettings.Controls.Add(this.dFilterSet, 0, 8);
            this.tlpSystemSettings.Controls.Add(this.dFilterActionColor, 0, 10);
            this.tlpSystemSettings.Controls.Add(this.tlpListExecute, 0, 7);
            this.tlpSystemSettings.Controls.Add(this.switchFloatButton, 0, 4);
            this.tlpSystemSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSystemSettings.Location = new System.Drawing.Point(0, 0);
            this.tlpSystemSettings.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSystemSettings.Name = "tlpSystemSettings";
            this.tlpSystemSettings.RowCount = 13;
            this.tlpSystemSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSystemSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSystemSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpSystemSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSystemSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpSystemSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpSystemSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSystemSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tlpSystemSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSystemSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tlpSystemSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSystemSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSystemSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpSystemSettings.Size = new System.Drawing.Size(500, 750);
            this.tlpSystemSettings.TabIndex = 1;
            // 
            // dFloatButton
            // 
            this.dFloatButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dFloatButton.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dFloatButton.LocalizationText = "SystemSettingsForm.FloatingButton";
            this.dFloatButton.Location = new System.Drawing.Point(3, 100);
            this.dFloatButton.Name = "dFloatButton";
            this.dFloatButton.Orientation = AntdUI.TOrientation.Left;
            this.dFloatButton.Size = new System.Drawing.Size(494, 23);
            this.dFloatButton.TabIndex = 19;
            this.dFloatButton.Text = "悬浮按钮";
            // 
            // tlpFilterActionColor
            // 
            this.tlpFilterActionColor.ColumnCount = 7;
            this.tlpFilterActionColor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tlpFilterActionColor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpFilterActionColor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpFilterActionColor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpFilterActionColor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpFilterActionColor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpFilterActionColor.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterActionColor.Controls.Add(this.bChangeReset, 5, 3);
            this.tlpFilterActionColor.Controls.Add(this.bInterceptReset, 5, 2);
            this.tlpFilterActionColor.Controls.Add(this.bReplaceReset, 5, 1);
            this.tlpFilterActionColor.Controls.Add(this.lForeColor, 3, 0);
            this.tlpFilterActionColor.Controls.Add(this.lBackColor, 2, 0);
            this.tlpFilterActionColor.Controls.Add(this.cChange_ForeColor, 3, 3);
            this.tlpFilterActionColor.Controls.Add(this.cChange_BackColor, 2, 3);
            this.tlpFilterActionColor.Controls.Add(this.lChange, 1, 3);
            this.tlpFilterActionColor.Controls.Add(this.cIntercept_ForeColor, 3, 2);
            this.tlpFilterActionColor.Controls.Add(this.cIntercept_BackColor, 2, 2);
            this.tlpFilterActionColor.Controls.Add(this.lIntercept, 1, 2);
            this.tlpFilterActionColor.Controls.Add(this.cRepalce_ForeColor, 3, 1);
            this.tlpFilterActionColor.Controls.Add(this.lReplace, 1, 1);
            this.tlpFilterActionColor.Controls.Add(this.cRepalce_BackColor, 2, 1);
            this.tlpFilterActionColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterActionColor.Location = new System.Drawing.Point(0, 423);
            this.tlpFilterActionColor.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFilterActionColor.Name = "tlpFilterActionColor";
            this.tlpFilterActionColor.RowCount = 5;
            this.tlpFilterActionColor.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterActionColor.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterActionColor.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterActionColor.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterActionColor.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterActionColor.Size = new System.Drawing.Size(500, 267);
            this.tlpFilterActionColor.TabIndex = 18;
            // 
            // lForeColor
            // 
            this.lForeColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lForeColor.LocalizationText = "SystemSettingsForm.ForeColor";
            this.lForeColor.Location = new System.Drawing.Point(210, 3);
            this.lForeColor.Name = "lForeColor";
            this.lForeColor.Size = new System.Drawing.Size(144, 45);
            this.lForeColor.TabIndex = 46;
            this.lForeColor.Text = "文字颜色";
            this.lForeColor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lBackColor
            // 
            this.lBackColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lBackColor.LocalizationText = "SystemSettingsForm.BackColor";
            this.lBackColor.Location = new System.Drawing.Point(60, 3);
            this.lBackColor.Name = "lBackColor";
            this.lBackColor.Size = new System.Drawing.Size(144, 45);
            this.lBackColor.TabIndex = 44;
            this.lBackColor.Text = "背景颜色";
            this.lBackColor.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cChange_ForeColor
            // 
            this.cChange_ForeColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cChange_ForeColor.Location = new System.Drawing.Point(210, 158);
            this.cChange_ForeColor.Name = "cChange_ForeColor";
            this.cChange_ForeColor.ShowClose = true;
            this.cChange_ForeColor.ShowReset = true;
            this.cChange_ForeColor.ShowText = true;
            this.cChange_ForeColor.Size = new System.Drawing.Size(144, 46);
            this.cChange_ForeColor.TabIndex = 8;
            this.cChange_ForeColor.Value = System.Drawing.Color.Black;
            // 
            // cChange_BackColor
            // 
            this.cChange_BackColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cChange_BackColor.Location = new System.Drawing.Point(60, 158);
            this.cChange_BackColor.Name = "cChange_BackColor";
            this.cChange_BackColor.ShowClose = true;
            this.cChange_BackColor.ShowReset = true;
            this.cChange_BackColor.ShowText = true;
            this.cChange_BackColor.Size = new System.Drawing.Size(144, 46);
            this.cChange_BackColor.TabIndex = 7;
            this.cChange_BackColor.Value = System.Drawing.Color.DodgerBlue;
            // 
            // lChange
            // 
            this.lChange.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lChange.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lChange.LocalizationText = "StatisticalData.Change";
            this.lChange.Location = new System.Drawing.Point(13, 158);
            this.lChange.Name = "lChange";
            this.lChange.Size = new System.Drawing.Size(41, 46);
            this.lChange.TabIndex = 6;
            this.lChange.Text = "换包 :";
            // 
            // cIntercept_ForeColor
            // 
            this.cIntercept_ForeColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cIntercept_ForeColor.Location = new System.Drawing.Point(210, 106);
            this.cIntercept_ForeColor.Name = "cIntercept_ForeColor";
            this.cIntercept_ForeColor.ShowClose = true;
            this.cIntercept_ForeColor.ShowReset = true;
            this.cIntercept_ForeColor.ShowText = true;
            this.cIntercept_ForeColor.Size = new System.Drawing.Size(144, 46);
            this.cIntercept_ForeColor.TabIndex = 5;
            this.cIntercept_ForeColor.Value = System.Drawing.Color.White;
            // 
            // cIntercept_BackColor
            // 
            this.cIntercept_BackColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cIntercept_BackColor.Location = new System.Drawing.Point(60, 106);
            this.cIntercept_BackColor.Name = "cIntercept_BackColor";
            this.cIntercept_BackColor.ShowClose = true;
            this.cIntercept_BackColor.ShowReset = true;
            this.cIntercept_BackColor.ShowText = true;
            this.cIntercept_BackColor.Size = new System.Drawing.Size(144, 46);
            this.cIntercept_BackColor.TabIndex = 4;
            this.cIntercept_BackColor.Value = System.Drawing.Color.DarkRed;
            // 
            // lIntercept
            // 
            this.lIntercept.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lIntercept.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lIntercept.LocalizationText = "StatisticalData.Intercept";
            this.lIntercept.Location = new System.Drawing.Point(13, 106);
            this.lIntercept.Name = "lIntercept";
            this.lIntercept.Size = new System.Drawing.Size(41, 46);
            this.lIntercept.TabIndex = 3;
            this.lIntercept.Text = "拦截 :";
            // 
            // cRepalce_ForeColor
            // 
            this.cRepalce_ForeColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cRepalce_ForeColor.Location = new System.Drawing.Point(210, 54);
            this.cRepalce_ForeColor.Name = "cRepalce_ForeColor";
            this.cRepalce_ForeColor.ShowClose = true;
            this.cRepalce_ForeColor.ShowReset = true;
            this.cRepalce_ForeColor.ShowText = true;
            this.cRepalce_ForeColor.Size = new System.Drawing.Size(144, 46);
            this.cRepalce_ForeColor.TabIndex = 2;
            this.cRepalce_ForeColor.Value = System.Drawing.Color.Black;
            // 
            // lReplace
            // 
            this.lReplace.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lReplace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lReplace.LocalizationText = "StatisticalData.Replace";
            this.lReplace.Location = new System.Drawing.Point(13, 54);
            this.lReplace.Name = "lReplace";
            this.lReplace.Size = new System.Drawing.Size(41, 46);
            this.lReplace.TabIndex = 0;
            this.lReplace.Text = "替换 :";
            // 
            // cRepalce_BackColor
            // 
            this.cRepalce_BackColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cRepalce_BackColor.Location = new System.Drawing.Point(60, 54);
            this.cRepalce_BackColor.Name = "cRepalce_BackColor";
            this.cRepalce_BackColor.ShowClose = true;
            this.cRepalce_BackColor.ShowReset = true;
            this.cRepalce_BackColor.ShowText = true;
            this.cRepalce_BackColor.Size = new System.Drawing.Size(144, 46);
            this.cRepalce_BackColor.TabIndex = 1;
            this.cRepalce_BackColor.Value = System.Drawing.Color.Goldenrod;
            // 
            // tlpFilterSet
            // 
            this.tlpFilterSet.ColumnCount = 2;
            this.tlpFilterSet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpFilterSet.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpFilterSet.Controls.Add(this.rbFilterSet_Sequence, 1, 0);
            this.tlpFilterSet.Controls.Add(this.rbFilterSet_Priority, 0, 0);
            this.tlpFilterSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterSet.Location = new System.Drawing.Point(0, 324);
            this.tlpFilterSet.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFilterSet.Name = "tlpFilterSet";
            this.tlpFilterSet.RowCount = 2;
            this.tlpFilterSet.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterSet.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterSet.Size = new System.Drawing.Size(500, 70);
            this.tlpFilterSet.TabIndex = 17;
            // 
            // rbFilterSet_Sequence
            // 
            this.rbFilterSet_Sequence.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbFilterSet_Sequence.Checked = true;
            this.rbFilterSet_Sequence.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbFilterSet_Sequence.LocalizationText = "SystemSettingsForm.Order";
            this.rbFilterSet_Sequence.Location = new System.Drawing.Point(253, 3);
            this.rbFilterSet_Sequence.Name = "rbFilterSet_Sequence";
            this.rbFilterSet_Sequence.Size = new System.Drawing.Size(122, 42);
            this.rbFilterSet_Sequence.TabIndex = 2;
            this.rbFilterSet_Sequence.Text = "按顺序执行";
            // 
            // rbFilterSet_Priority
            // 
            this.rbFilterSet_Priority.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbFilterSet_Priority.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbFilterSet_Priority.LocalizationText = "SystemSettingsForm.Priority";
            this.rbFilterSet_Priority.Location = new System.Drawing.Point(3, 3);
            this.rbFilterSet_Priority.Name = "rbFilterSet_Priority";
            this.rbFilterSet_Priority.Size = new System.Drawing.Size(106, 42);
            this.rbFilterSet_Priority.TabIndex = 1;
            this.rbFilterSet_Priority.Text = "优先原则";
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
            this.tlpButton.Location = new System.Drawing.Point(0, 690);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(500, 60);
            this.tlpButton.TabIndex = 16;
            // 
            // bSave
            // 
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.LocalizationText = "Save";
            this.bSave.Location = new System.Drawing.Point(123, 7);
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
            this.bExit.LocalizationText = "Cancel";
            this.bExit.Location = new System.Drawing.Point(263, 7);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(114, 46);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // cbSpeedMode
            // 
            this.cbSpeedMode.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbSpeedMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbSpeedMode.LocalizationText = "SystemSettingsForm.SpeedMode";
            this.cbSpeedMode.Location = new System.Drawing.Point(3, 32);
            this.cbSpeedMode.Name = "cbSpeedMode";
            this.cbSpeedMode.Size = new System.Drawing.Size(106, 42);
            this.cbSpeedMode.TabIndex = 6;
            this.cbSpeedMode.Text = "极速模式";
            // 
            // dWorkMode
            // 
            this.dWorkMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dWorkMode.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dWorkMode.LocalizationText = "SystemSettingsForm.WorkMode";
            this.dWorkMode.Location = new System.Drawing.Point(3, 3);
            this.dWorkMode.Name = "dWorkMode";
            this.dWorkMode.Orientation = AntdUI.TOrientation.Left;
            this.dWorkMode.Size = new System.Drawing.Size(494, 23);
            this.dWorkMode.TabIndex = 1;
            this.dWorkMode.Text = "工作模式";
            // 
            // dListExecute
            // 
            this.dListExecute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dListExecute.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dListExecute.LocalizationText = "SystemSettingsForm.ListMode";
            this.dListExecute.Location = new System.Drawing.Point(3, 199);
            this.dListExecute.Name = "dListExecute";
            this.dListExecute.Orientation = AntdUI.TOrientation.Left;
            this.dListExecute.Size = new System.Drawing.Size(494, 23);
            this.dListExecute.TabIndex = 2;
            this.dListExecute.Text = "列表执行模式";
            // 
            // dFilterSet
            // 
            this.dFilterSet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dFilterSet.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dFilterSet.LocalizationText = "SystemSettingsForm.FilterMode";
            this.dFilterSet.Location = new System.Drawing.Point(3, 298);
            this.dFilterSet.Name = "dFilterSet";
            this.dFilterSet.Orientation = AntdUI.TOrientation.Left;
            this.dFilterSet.Size = new System.Drawing.Size(494, 23);
            this.dFilterSet.TabIndex = 3;
            this.dFilterSet.Text = "滤镜执行模式";
            // 
            // dFilterActionColor
            // 
            this.dFilterActionColor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dFilterActionColor.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dFilterActionColor.LocalizationText = "SystemSettingsForm.FilterAction";
            this.dFilterActionColor.Location = new System.Drawing.Point(3, 397);
            this.dFilterActionColor.Name = "dFilterActionColor";
            this.dFilterActionColor.Orientation = AntdUI.TOrientation.Left;
            this.dFilterActionColor.Size = new System.Drawing.Size(494, 23);
            this.dFilterActionColor.TabIndex = 4;
            this.dFilterActionColor.Text = "滤镜动作";
            // 
            // tlpListExecute
            // 
            this.tlpListExecute.ColumnCount = 2;
            this.tlpListExecute.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpListExecute.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpListExecute.Controls.Add(this.rbListExecute_Sequence, 1, 0);
            this.tlpListExecute.Controls.Add(this.rbListExecute_Together, 0, 0);
            this.tlpListExecute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpListExecute.Location = new System.Drawing.Point(0, 225);
            this.tlpListExecute.Margin = new System.Windows.Forms.Padding(0);
            this.tlpListExecute.Name = "tlpListExecute";
            this.tlpListExecute.RowCount = 2;
            this.tlpListExecute.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpListExecute.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpListExecute.Size = new System.Drawing.Size(500, 70);
            this.tlpListExecute.TabIndex = 15;
            // 
            // rbListExecute_Sequence
            // 
            this.rbListExecute_Sequence.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbListExecute_Sequence.Checked = true;
            this.rbListExecute_Sequence.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbListExecute_Sequence.LocalizationText = "SystemSettingsForm.Order";
            this.rbListExecute_Sequence.Location = new System.Drawing.Point(253, 3);
            this.rbListExecute_Sequence.Name = "rbListExecute_Sequence";
            this.rbListExecute_Sequence.Size = new System.Drawing.Size(122, 42);
            this.rbListExecute_Sequence.TabIndex = 2;
            this.rbListExecute_Sequence.Text = "按顺序执行";
            // 
            // rbListExecute_Together
            // 
            this.rbListExecute_Together.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbListExecute_Together.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbListExecute_Together.LocalizationText = "SystemSettingsForm.Simultaneously";
            this.rbListExecute_Together.Location = new System.Drawing.Point(3, 3);
            this.rbListExecute_Together.Name = "rbListExecute_Together";
            this.rbListExecute_Together.Size = new System.Drawing.Size(106, 42);
            this.rbListExecute_Together.TabIndex = 1;
            this.rbListExecute_Together.Text = "同时执行";
            // 
            // switchFloatButton
            // 
            this.switchFloatButton.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.switchFloatButton.Location = new System.Drawing.Point(6, 136);
            this.switchFloatButton.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            this.switchFloatButton.Name = "switchFloatButton";
            this.switchFloatButton.Size = new System.Drawing.Size(50, 30);
            this.switchFloatButton.TabIndex = 20;
            this.switchFloatButton.Text = "switch1";
            // 
            // bReplaceReset
            // 
            this.bReplaceReset.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bReplaceReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bReplaceReset.LocalizationText = "Reset";
            this.bReplaceReset.Location = new System.Drawing.Point(380, 54);
            this.bReplaceReset.Name = "bReplaceReset";
            this.bReplaceReset.Size = new System.Drawing.Size(60, 46);
            this.bReplaceReset.TabIndex = 50;
            this.bReplaceReset.Text = "还原";
            this.bReplaceReset.Type = AntdUI.TTypeMini.Success;
            this.bReplaceReset.Click += new System.EventHandler(this.bReplaceReset_Click);
            // 
            // bInterceptReset
            // 
            this.bInterceptReset.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bInterceptReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bInterceptReset.LocalizationText = "Reset";
            this.bInterceptReset.Location = new System.Drawing.Point(380, 106);
            this.bInterceptReset.Name = "bInterceptReset";
            this.bInterceptReset.Size = new System.Drawing.Size(60, 46);
            this.bInterceptReset.TabIndex = 52;
            this.bInterceptReset.Text = "还原";
            this.bInterceptReset.Type = AntdUI.TTypeMini.Success;
            this.bInterceptReset.Click += new System.EventHandler(this.bInterceptReset_Click);
            // 
            // bChangeReset
            // 
            this.bChangeReset.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bChangeReset.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bChangeReset.LocalizationText = "Reset";
            this.bChangeReset.Location = new System.Drawing.Point(380, 158);
            this.bChangeReset.Name = "bChangeReset";
            this.bChangeReset.Size = new System.Drawing.Size(60, 46);
            this.bChangeReset.TabIndex = 54;
            this.bChangeReset.Text = "还原";
            this.bChangeReset.Type = AntdUI.TTypeMini.Success;
            this.bChangeReset.Click += new System.EventHandler(this.bChangeReset_Click);
            // 
            // SystemSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpSystemSettings);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "SystemSetting";
            this.Size = new System.Drawing.Size(500, 750);
            this.Load += new System.EventHandler(this.SystemSetting_Load);
            this.tlpSystemSettings.ResumeLayout(false);
            this.tlpSystemSettings.PerformLayout();
            this.tlpFilterActionColor.ResumeLayout(false);
            this.tlpFilterActionColor.PerformLayout();
            this.tlpFilterSet.ResumeLayout(false);
            this.tlpFilterSet.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.tlpListExecute.ResumeLayout(false);
            this.tlpListExecute.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpSystemSettings;
        private AntdUI.Divider dFloatButton;
        private System.Windows.Forms.TableLayoutPanel tlpFilterActionColor;
        private System.Windows.Forms.TableLayoutPanel tlpFilterSet;
        private AntdUI.Radio rbFilterSet_Sequence;
        private AntdUI.Radio rbFilterSet_Priority;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private AntdUI.Checkbox cbSpeedMode;
        private AntdUI.Divider dWorkMode;
        private AntdUI.Divider dListExecute;
        private AntdUI.Divider dFilterSet;
        private AntdUI.Divider dFilterActionColor;
        private System.Windows.Forms.TableLayoutPanel tlpListExecute;
        private AntdUI.Radio rbListExecute_Sequence;
        private AntdUI.Radio rbListExecute_Together;
        private AntdUI.Switch switchFloatButton;
        private AntdUI.Label lReplace;
        private AntdUI.ColorPicker cRepalce_BackColor;
        private AntdUI.ColorPicker cRepalce_ForeColor;
        private AntdUI.ColorPicker cChange_ForeColor;
        private AntdUI.ColorPicker cChange_BackColor;
        private AntdUI.Label lChange;
        private AntdUI.ColorPicker cIntercept_ForeColor;
        private AntdUI.ColorPicker cIntercept_BackColor;
        private AntdUI.Label lIntercept;
        private AntdUI.Label lForeColor;
        private AntdUI.Label lBackColor;
        private AntdUI.Button bChangeReset;
        private AntdUI.Button bInterceptReset;
        private AntdUI.Button bReplaceReset;
    }
}
