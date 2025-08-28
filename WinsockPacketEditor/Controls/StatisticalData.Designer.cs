namespace WinsockPacketEditor
{
    partial class StatisticalData
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
            AntdUI.Tabs.StyleLine styleLine1 = new AntdUI.Tabs.StyleLine();
            this.tlpStatistical = new System.Windows.Forms.TableLayoutPanel();
            this.tabStatistical = new AntdUI.Tabs();
            this.tpFilter = new AntdUI.TabPage();
            this.tlpStatistical_Filter = new System.Windows.Forms.TableLayoutPanel();
            this.tlpStatistical_FilterButton = new System.Windows.Forms.TableLayoutPanel();
            this.bStatistical_Filter = new AntdUI.Button();
            this.lStatisticalFilter_Length = new AntdUI.Label();
            this.tlpStatistical_FilterTop = new System.Windows.Forms.TableLayoutPanel();
            this.lStatisticalFilter_Action = new AntdUI.Label();
            this.progressExecute = new AntdUI.Progress();
            this.lStatisticalFilter_Execute = new AntdUI.Label();
            this.tlpStatistical_FilterTop2 = new System.Windows.Forms.TableLayoutPanel();
            this.progressNoDisplay = new AntdUI.Progress();
            this.progressDisplay = new AntdUI.Progress();
            this.progressIntercept = new AntdUI.Progress();
            this.progressChange = new AntdUI.Progress();
            this.lNoDisplay = new AntdUI.Label();
            this.lDisplay = new AntdUI.Label();
            this.lIntercept = new AntdUI.Label();
            this.lChange = new AntdUI.Label();
            this.progressReplace = new AntdUI.Progress();
            this.lReplace = new AntdUI.Label();
            this.tStatisticalFilter = new AntdUI.Table();
            this.bgwStatistical = new System.ComponentModel.BackgroundWorker();
            this.tlpStatistical.SuspendLayout();
            this.tabStatistical.SuspendLayout();
            this.tpFilter.SuspendLayout();
            this.tlpStatistical_Filter.SuspendLayout();
            this.tlpStatistical_FilterButton.SuspendLayout();
            this.tlpStatistical_FilterTop.SuspendLayout();
            this.tlpStatistical_FilterTop2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpStatistical
            // 
            this.tlpStatistical.ColumnCount = 3;
            this.tlpStatistical.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpStatistical.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpStatistical.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpStatistical.Controls.Add(this.tabStatistical, 1, 1);
            this.tlpStatistical.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpStatistical.Location = new System.Drawing.Point(0, 0);
            this.tlpStatistical.Margin = new System.Windows.Forms.Padding(0);
            this.tlpStatistical.Name = "tlpStatistical";
            this.tlpStatistical.RowCount = 2;
            this.tlpStatistical.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpStatistical.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpStatistical.Size = new System.Drawing.Size(800, 800);
            this.tlpStatistical.TabIndex = 1;
            // 
            // tabStatistical
            // 
            this.tabStatistical.Controls.Add(this.tpFilter);
            this.tabStatistical.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tabStatistical.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabStatistical.Location = new System.Drawing.Point(33, 23);
            this.tabStatistical.Name = "tabStatistical";
            this.tabStatistical.Pages.Add(this.tpFilter);
            this.tabStatistical.Size = new System.Drawing.Size(734, 774);
            this.tabStatistical.Style = styleLine1;
            this.tabStatistical.TabIndex = 0;
            // 
            // tpFilter
            // 
            this.tpFilter.Controls.Add(this.tlpStatistical_Filter);
            this.tpFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpFilter.Location = new System.Drawing.Point(3, 33);
            this.tpFilter.Name = "tpFilter";
            this.tpFilter.Size = new System.Drawing.Size(728, 738);
            this.tpFilter.TabIndex = 0;
            this.tpFilter.Text = "tpFilter";
            // 
            // tlpStatistical_Filter
            // 
            this.tlpStatistical_Filter.ColumnCount = 1;
            this.tlpStatistical_Filter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpStatistical_Filter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpStatistical_Filter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpStatistical_Filter.Controls.Add(this.tlpStatistical_FilterButton, 0, 4);
            this.tlpStatistical_Filter.Controls.Add(this.lStatisticalFilter_Length, 0, 2);
            this.tlpStatistical_Filter.Controls.Add(this.tlpStatistical_FilterTop, 0, 0);
            this.tlpStatistical_Filter.Controls.Add(this.tStatisticalFilter, 0, 3);
            this.tlpStatistical_Filter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpStatistical_Filter.Location = new System.Drawing.Point(0, 0);
            this.tlpStatistical_Filter.Margin = new System.Windows.Forms.Padding(0);
            this.tlpStatistical_Filter.Name = "tlpStatistical_Filter";
            this.tlpStatistical_Filter.RowCount = 5;
            this.tlpStatistical_Filter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tlpStatistical_Filter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpStatistical_Filter.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpStatistical_Filter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpStatistical_Filter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpStatistical_Filter.Size = new System.Drawing.Size(728, 738);
            this.tlpStatistical_Filter.TabIndex = 0;
            // 
            // tlpStatistical_FilterButton
            // 
            this.tlpStatistical_FilterButton.ColumnCount = 3;
            this.tlpStatistical_FilterButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpStatistical_FilterButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlpStatistical_FilterButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpStatistical_FilterButton.Controls.Add(this.bStatistical_Filter, 1, 1);
            this.tlpStatistical_FilterButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpStatistical_FilterButton.Location = new System.Drawing.Point(0, 678);
            this.tlpStatistical_FilterButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpStatistical_FilterButton.Name = "tlpStatistical_FilterButton";
            this.tlpStatistical_FilterButton.RowCount = 3;
            this.tlpStatistical_FilterButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpStatistical_FilterButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpStatistical_FilterButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpStatistical_FilterButton.Size = new System.Drawing.Size(728, 60);
            this.tlpStatistical_FilterButton.TabIndex = 4;
            // 
            // bStatistical_Filter
            // 
            this.bStatistical_Filter.BackExtend = "135, #6253E1, #04BEFE";
            this.bStatistical_Filter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bStatistical_Filter.IconSvg = "SyncOutlined";
            this.bStatistical_Filter.LocalizationText = "Refresh";
            this.bStatistical_Filter.Location = new System.Drawing.Point(292, 8);
            this.bStatistical_Filter.Name = "bStatistical_Filter";
            this.bStatistical_Filter.Size = new System.Drawing.Size(144, 44);
            this.bStatistical_Filter.TabIndex = 1;
            this.bStatistical_Filter.Text = "刷新数据";
            this.bStatistical_Filter.Type = AntdUI.TTypeMini.Primary;
            this.bStatistical_Filter.Click += new System.EventHandler(this.bStatistical_Filter_Click);
            // 
            // lStatisticalFilter_Length
            // 
            this.lStatisticalFilter_Length.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lStatisticalFilter_Length.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lStatisticalFilter_Length.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lStatisticalFilter_Length.LocalizationText = "StatisticalData.Details";
            this.lStatisticalFilter_Length.Location = new System.Drawing.Point(3, 353);
            this.lStatisticalFilter_Length.Name = "lStatisticalFilter_Length";
            this.lStatisticalFilter_Length.Size = new System.Drawing.Size(86, 29);
            this.lStatisticalFilter_Length.TabIndex = 3;
            this.lStatisticalFilter_Length.Text = "明细数据";
            // 
            // tlpStatistical_FilterTop
            // 
            this.tlpStatistical_FilterTop.ColumnCount = 2;
            this.tlpStatistical_FilterTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tlpStatistical_FilterTop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tlpStatistical_FilterTop.Controls.Add(this.lStatisticalFilter_Action, 1, 0);
            this.tlpStatistical_FilterTop.Controls.Add(this.progressExecute, 0, 1);
            this.tlpStatistical_FilterTop.Controls.Add(this.lStatisticalFilter_Execute, 0, 0);
            this.tlpStatistical_FilterTop.Controls.Add(this.tlpStatistical_FilterTop2, 1, 1);
            this.tlpStatistical_FilterTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpStatistical_FilterTop.Location = new System.Drawing.Point(0, 0);
            this.tlpStatistical_FilterTop.Margin = new System.Windows.Forms.Padding(0);
            this.tlpStatistical_FilterTop.Name = "tlpStatistical_FilterTop";
            this.tlpStatistical_FilterTop.RowCount = 2;
            this.tlpStatistical_FilterTop.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpStatistical_FilterTop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpStatistical_FilterTop.Size = new System.Drawing.Size(728, 300);
            this.tlpStatistical_FilterTop.TabIndex = 1;
            // 
            // lStatisticalFilter_Action
            // 
            this.lStatisticalFilter_Action.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lStatisticalFilter_Action.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lStatisticalFilter_Action.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lStatisticalFilter_Action.LocalizationText = "StatisticalData.FilterAction";
            this.lStatisticalFilter_Action.Location = new System.Drawing.Point(294, 3);
            this.lStatisticalFilter_Action.Name = "lStatisticalFilter_Action";
            this.lStatisticalFilter_Action.Size = new System.Drawing.Size(86, 29);
            this.lStatisticalFilter_Action.TabIndex = 2;
            this.lStatisticalFilter_Action.Text = "滤镜动作";
            // 
            // progressExecute
            // 
            this.progressExecute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressExecute.Fill = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(189)))), ((int)(((byte)(233)))));
            this.progressExecute.Font = new System.Drawing.Font("微软雅黑", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.progressExecute.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(189)))), ((int)(((byte)(233)))));
            this.progressExecute.Location = new System.Drawing.Point(3, 38);
            this.progressExecute.Name = "progressExecute";
            this.progressExecute.Radius = 30;
            this.progressExecute.Shape = AntdUI.TShapeProgress.Circle;
            this.progressExecute.Size = new System.Drawing.Size(285, 259);
            this.progressExecute.TabIndex = 0;
            this.progressExecute.Text = "75%";
            this.progressExecute.UseSystemText = true;
            this.progressExecute.Value = 0.75F;
            // 
            // lStatisticalFilter_Execute
            // 
            this.lStatisticalFilter_Execute.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lStatisticalFilter_Execute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lStatisticalFilter_Execute.Font = new System.Drawing.Font("微软雅黑", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lStatisticalFilter_Execute.LocalizationText = "StatisticalData.FilterExecution";
            this.lStatisticalFilter_Execute.Location = new System.Drawing.Point(3, 3);
            this.lStatisticalFilter_Execute.Name = "lStatisticalFilter_Execute";
            this.lStatisticalFilter_Execute.Size = new System.Drawing.Size(86, 29);
            this.lStatisticalFilter_Execute.TabIndex = 1;
            this.lStatisticalFilter_Execute.Text = "滤镜执行";
            // 
            // tlpStatistical_FilterTop2
            // 
            this.tlpStatistical_FilterTop2.ColumnCount = 3;
            this.tlpStatistical_FilterTop2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpStatistical_FilterTop2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpStatistical_FilterTop2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpStatistical_FilterTop2.Controls.Add(this.progressNoDisplay, 2, 4);
            this.tlpStatistical_FilterTop2.Controls.Add(this.progressDisplay, 2, 3);
            this.tlpStatistical_FilterTop2.Controls.Add(this.progressIntercept, 2, 2);
            this.tlpStatistical_FilterTop2.Controls.Add(this.progressChange, 2, 1);
            this.tlpStatistical_FilterTop2.Controls.Add(this.lNoDisplay, 1, 4);
            this.tlpStatistical_FilterTop2.Controls.Add(this.lDisplay, 1, 3);
            this.tlpStatistical_FilterTop2.Controls.Add(this.lIntercept, 1, 2);
            this.tlpStatistical_FilterTop2.Controls.Add(this.lChange, 1, 1);
            this.tlpStatistical_FilterTop2.Controls.Add(this.progressReplace, 2, 0);
            this.tlpStatistical_FilterTop2.Controls.Add(this.lReplace, 1, 0);
            this.tlpStatistical_FilterTop2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpStatistical_FilterTop2.Location = new System.Drawing.Point(291, 35);
            this.tlpStatistical_FilterTop2.Margin = new System.Windows.Forms.Padding(0);
            this.tlpStatistical_FilterTop2.Name = "tlpStatistical_FilterTop2";
            this.tlpStatistical_FilterTop2.RowCount = 5;
            this.tlpStatistical_FilterTop2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpStatistical_FilterTop2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpStatistical_FilterTop2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpStatistical_FilterTop2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpStatistical_FilterTop2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tlpStatistical_FilterTop2.Size = new System.Drawing.Size(437, 265);
            this.tlpStatistical_FilterTop2.TabIndex = 3;
            // 
            // progressNoDisplay
            // 
            this.progressNoDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressNoDisplay.Fill = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(108)))), ((int)(((byte)(230)))));
            this.progressNoDisplay.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.progressNoDisplay.Location = new System.Drawing.Point(116, 215);
            this.progressNoDisplay.Name = "progressNoDisplay";
            this.progressNoDisplay.Size = new System.Drawing.Size(318, 47);
            this.progressNoDisplay.TabIndex = 13;
            this.progressNoDisplay.Text = "50%";
            this.progressNoDisplay.UseSystemText = true;
            this.progressNoDisplay.UseTextCenter = true;
            this.progressNoDisplay.Value = 0.5F;
            this.progressNoDisplay.ValueRatio = 1F;
            // 
            // progressDisplay
            // 
            this.progressDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressDisplay.Fill = System.Drawing.Color.FromArgb(((int)(((byte)(195)))), ((int)(((byte)(108)))), ((int)(((byte)(238)))));
            this.progressDisplay.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.progressDisplay.Location = new System.Drawing.Point(116, 162);
            this.progressDisplay.Name = "progressDisplay";
            this.progressDisplay.Size = new System.Drawing.Size(318, 47);
            this.progressDisplay.TabIndex = 12;
            this.progressDisplay.Text = "60%";
            this.progressDisplay.UseSystemText = true;
            this.progressDisplay.UseTextCenter = true;
            this.progressDisplay.Value = 0.6F;
            this.progressDisplay.ValueRatio = 1F;
            // 
            // progressIntercept
            // 
            this.progressIntercept.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressIntercept.Fill = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(108)))), ((int)(((byte)(233)))));
            this.progressIntercept.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.progressIntercept.Location = new System.Drawing.Point(116, 109);
            this.progressIntercept.Name = "progressIntercept";
            this.progressIntercept.Size = new System.Drawing.Size(318, 47);
            this.progressIntercept.TabIndex = 11;
            this.progressIntercept.Text = "70%";
            this.progressIntercept.UseSystemText = true;
            this.progressIntercept.UseTextCenter = true;
            this.progressIntercept.Value = 0.7F;
            this.progressIntercept.ValueRatio = 1F;
            // 
            // progressChange
            // 
            this.progressChange.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressChange.Fill = System.Drawing.Color.FromArgb(((int)(((byte)(103)))), ((int)(((byte)(114)))), ((int)(((byte)(228)))));
            this.progressChange.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.progressChange.Location = new System.Drawing.Point(116, 56);
            this.progressChange.Name = "progressChange";
            this.progressChange.Size = new System.Drawing.Size(318, 47);
            this.progressChange.TabIndex = 10;
            this.progressChange.Text = "80%";
            this.progressChange.UseSystemText = true;
            this.progressChange.UseTextCenter = true;
            this.progressChange.Value = 0.8F;
            this.progressChange.ValueRatio = 1F;
            // 
            // lNoDisplay
            // 
            this.lNoDisplay.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lNoDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lNoDisplay.LocalizationText = "StatisticalData.NoDisplay";
            this.lNoDisplay.Location = new System.Drawing.Point(53, 215);
            this.lNoDisplay.Name = "lNoDisplay";
            this.lNoDisplay.Size = new System.Drawing.Size(57, 47);
            this.lNoDisplay.TabIndex = 9;
            this.lNoDisplay.Text = "不显示 :";
            // 
            // lDisplay
            // 
            this.lDisplay.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lDisplay.LocalizationText = "StatisticalData.Display";
            this.lDisplay.Location = new System.Drawing.Point(53, 162);
            this.lDisplay.Name = "lDisplay";
            this.lDisplay.Size = new System.Drawing.Size(57, 47);
            this.lDisplay.TabIndex = 8;
            this.lDisplay.Text = "只显示 :";
            // 
            // lIntercept
            // 
            this.lIntercept.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lIntercept.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lIntercept.LocalizationText = "StatisticalData.Intercept";
            this.lIntercept.Location = new System.Drawing.Point(53, 109);
            this.lIntercept.Name = "lIntercept";
            this.lIntercept.Size = new System.Drawing.Size(41, 47);
            this.lIntercept.TabIndex = 7;
            this.lIntercept.Text = "拦截 :";
            // 
            // lChange
            // 
            this.lChange.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lChange.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lChange.LocalizationText = "StatisticalData.Change";
            this.lChange.Location = new System.Drawing.Point(53, 56);
            this.lChange.Name = "lChange";
            this.lChange.Size = new System.Drawing.Size(41, 47);
            this.lChange.TabIndex = 6;
            this.lChange.Text = "换包 :";
            // 
            // progressReplace
            // 
            this.progressReplace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressReplace.Fill = System.Drawing.Color.FromArgb(((int)(((byte)(74)))), ((int)(((byte)(74)))), ((int)(((byte)(255)))));
            this.progressReplace.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.progressReplace.Location = new System.Drawing.Point(116, 3);
            this.progressReplace.Name = "progressReplace";
            this.progressReplace.Size = new System.Drawing.Size(318, 47);
            this.progressReplace.TabIndex = 0;
            this.progressReplace.Text = "90%";
            this.progressReplace.UseSystemText = true;
            this.progressReplace.UseTextCenter = true;
            this.progressReplace.Value = 0.9F;
            this.progressReplace.ValueRatio = 1F;
            // 
            // lReplace
            // 
            this.lReplace.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lReplace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lReplace.LocalizationText = "StatisticalData.Replace";
            this.lReplace.Location = new System.Drawing.Point(53, 3);
            this.lReplace.Name = "lReplace";
            this.lReplace.Size = new System.Drawing.Size(41, 47);
            this.lReplace.TabIndex = 5;
            this.lReplace.Text = "替换 :";
            // 
            // tStatisticalFilter
            // 
            this.tStatisticalFilter.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tStatisticalFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tStatisticalFilter.EmptyHeader = true;
            this.tStatisticalFilter.Gap = 12;
            this.tStatisticalFilter.Location = new System.Drawing.Point(3, 388);
            this.tStatisticalFilter.Name = "tStatisticalFilter";
            this.tStatisticalFilter.Size = new System.Drawing.Size(722, 287);
            this.tStatisticalFilter.TabIndex = 2;
            this.tStatisticalFilter.Text = "table1";
            // 
            // bgwStatistical
            // 
            this.bgwStatistical.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwStatistical_DoWork);
            this.bgwStatistical.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwStatistical_RunWorkerCompleted);
            // 
            // StatisticalData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpStatistical);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "StatisticalData";
            this.Size = new System.Drawing.Size(800, 800);
            this.Load += new System.EventHandler(this.StatisticalData_Load);
            this.tlpStatistical.ResumeLayout(false);
            this.tabStatistical.ResumeLayout(false);
            this.tpFilter.ResumeLayout(false);
            this.tlpStatistical_Filter.ResumeLayout(false);
            this.tlpStatistical_Filter.PerformLayout();
            this.tlpStatistical_FilterButton.ResumeLayout(false);
            this.tlpStatistical_FilterTop.ResumeLayout(false);
            this.tlpStatistical_FilterTop.PerformLayout();
            this.tlpStatistical_FilterTop2.ResumeLayout(false);
            this.tlpStatistical_FilterTop2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpStatistical;
        private AntdUI.Tabs tabStatistical;
        private AntdUI.TabPage tpFilter;
        private System.Windows.Forms.TableLayoutPanel tlpStatistical_Filter;
        private System.Windows.Forms.TableLayoutPanel tlpStatistical_FilterButton;
        private AntdUI.Button bStatistical_Filter;
        private AntdUI.Label lStatisticalFilter_Length;
        private System.Windows.Forms.TableLayoutPanel tlpStatistical_FilterTop;
        private AntdUI.Label lStatisticalFilter_Action;
        private AntdUI.Progress progressExecute;
        private AntdUI.Label lStatisticalFilter_Execute;
        private System.Windows.Forms.TableLayoutPanel tlpStatistical_FilterTop2;
        private AntdUI.Progress progressNoDisplay;
        private AntdUI.Progress progressDisplay;
        private AntdUI.Progress progressIntercept;
        private AntdUI.Progress progressChange;
        private AntdUI.Label lNoDisplay;
        private AntdUI.Label lDisplay;
        private AntdUI.Label lIntercept;
        private AntdUI.Label lChange;
        private AntdUI.Progress progressReplace;
        private AntdUI.Label lReplace;
        private AntdUI.Table tStatisticalFilter;
        private System.ComponentModel.BackgroundWorker bgwStatistical;
    }
}
