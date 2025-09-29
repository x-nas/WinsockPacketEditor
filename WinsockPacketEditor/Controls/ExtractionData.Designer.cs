namespace WinsockPacketEditor
{
    partial class ExtractionData
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
            this.splitterExtractionData = new AntdUI.Splitter();
            this.tlpExtractionSelectFile = new WinsockPacketEditor.TableLayoutPanelEx();
            this.ddlExtraction = new AntdUI.Select();
            this.udExtraction = new AntdUI.UploadDragger();
            this.tlpData = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpExtractionButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bExtraction = new AntdUI.Button();
            this.txtExtraction = new AntdUI.Input();
            this.tlpExtractionData = new WinsockPacketEditor.TableLayoutPanelEx();
            ((System.ComponentModel.ISupportInitialize)(this.splitterExtractionData)).BeginInit();
            this.splitterExtractionData.Panel1.SuspendLayout();
            this.splitterExtractionData.Panel2.SuspendLayout();
            this.splitterExtractionData.SuspendLayout();
            this.tlpExtractionSelectFile.SuspendLayout();
            this.tlpData.SuspendLayout();
            this.tlpExtractionButton.SuspendLayout();
            this.tlpExtractionData.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitterExtractionData
            // 
            this.splitterExtractionData.CollapsePanel = AntdUI.Splitter.ADCollapsePanel.Panel1;
            this.splitterExtractionData.Cursor = System.Windows.Forms.Cursors.Default;
            this.splitterExtractionData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitterExtractionData.Location = new System.Drawing.Point(3, 3);
            this.splitterExtractionData.Name = "splitterExtractionData";
            this.splitterExtractionData.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitterExtractionData.Panel1
            // 
            this.splitterExtractionData.Panel1.Controls.Add(this.tlpExtractionSelectFile);
            this.splitterExtractionData.Panel1MinSize = 0;
            // 
            // splitterExtractionData.Panel2
            // 
            this.splitterExtractionData.Panel2.Controls.Add(this.tlpData);
            this.splitterExtractionData.Panel2MinSize = 0;
            this.splitterExtractionData.Size = new System.Drawing.Size(1094, 794);
            this.splitterExtractionData.SplitterDistance = 400;
            this.splitterExtractionData.SplitterSize = 80;
            this.splitterExtractionData.SplitterWidth = 5;
            this.splitterExtractionData.TabIndex = 4;
            // 
            // tlpExtractionSelectFile
            // 
            this.tlpExtractionSelectFile.ColumnCount = 1;
            this.tlpExtractionSelectFile.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpExtractionSelectFile.Controls.Add(this.ddlExtraction, 0, 0);
            this.tlpExtractionSelectFile.Controls.Add(this.udExtraction, 0, 1);
            this.tlpExtractionSelectFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpExtractionSelectFile.Location = new System.Drawing.Point(0, 0);
            this.tlpExtractionSelectFile.Margin = new System.Windows.Forms.Padding(0);
            this.tlpExtractionSelectFile.Name = "tlpExtractionSelectFile";
            this.tlpExtractionSelectFile.RowCount = 2;
            this.tlpExtractionSelectFile.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpExtractionSelectFile.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpExtractionSelectFile.Size = new System.Drawing.Size(1094, 400);
            this.tlpExtractionSelectFile.TabIndex = 1;
            // 
            // ddlExtraction
            // 
            this.ddlExtraction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddlExtraction.List = true;
            this.ddlExtraction.LocalizationPlaceholderText = "ExtractionData.ExtractionType";
            this.ddlExtraction.Location = new System.Drawing.Point(2, 2);
            this.ddlExtraction.Margin = new System.Windows.Forms.Padding(2);
            this.ddlExtraction.Name = "ddlExtraction";
            this.ddlExtraction.PlaceholderText = "请选择提取类型";
            this.ddlExtraction.Size = new System.Drawing.Size(1090, 36);
            this.ddlExtraction.TabIndex = 2;
            this.ddlExtraction.SelectedIndexChanged += new AntdUI.IntEventHandler(this.ddlExtraction_SelectedIndexChanged);
            // 
            // udExtraction
            // 
            this.udExtraction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.udExtraction.LocalizationText = "ExtractionData.DragFiles";
            this.udExtraction.Location = new System.Drawing.Point(2, 42);
            this.udExtraction.Margin = new System.Windows.Forms.Padding(2);
            this.udExtraction.Multiselect = false;
            this.udExtraction.Name = "udExtraction";
            this.udExtraction.Size = new System.Drawing.Size(1090, 356);
            this.udExtraction.TabIndex = 1;
            this.udExtraction.Text = "单击或拖动文件到此区域进行数据提取";
            this.udExtraction.TextDesc = "";
            this.udExtraction.DragChanged += new AntdUI.IControl.DragEventHandler(this.udExtraction_DragChanged);
            // 
            // tlpData
            // 
            this.tlpData.ColumnCount = 1;
            this.tlpData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpData.Controls.Add(this.tlpExtractionButton, 0, 1);
            this.tlpData.Controls.Add(this.txtExtraction, 0, 0);
            this.tlpData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpData.Location = new System.Drawing.Point(0, 0);
            this.tlpData.Margin = new System.Windows.Forms.Padding(0);
            this.tlpData.Name = "tlpData";
            this.tlpData.RowCount = 2;
            this.tlpData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tlpData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpData.Size = new System.Drawing.Size(1094, 389);
            this.tlpData.TabIndex = 1;
            // 
            // tlpExtractionButton
            // 
            this.tlpExtractionButton.ColumnCount = 3;
            this.tlpExtractionButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpExtractionButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpExtractionButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpExtractionButton.Controls.Add(this.bExtraction, 1, 1);
            this.tlpExtractionButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpExtractionButton.Location = new System.Drawing.Point(0, 340);
            this.tlpExtractionButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpExtractionButton.Name = "tlpExtractionButton";
            this.tlpExtractionButton.RowCount = 3;
            this.tlpExtractionButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpExtractionButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpExtractionButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpExtractionButton.Size = new System.Drawing.Size(1094, 49);
            this.tlpExtractionButton.TabIndex = 2;
            // 
            // bExtraction
            // 
            this.bExtraction.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bExtraction.BackExtend = "135, #6253E1, #04BEFE";
            this.bExtraction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bExtraction.IconSvg = "SaveOutlined";
            this.bExtraction.LocalizationText = "Extraction";
            this.bExtraction.Location = new System.Drawing.Point(503, 6);
            this.bExtraction.Margin = new System.Windows.Forms.Padding(2);
            this.bExtraction.Name = "bExtraction";
            this.bExtraction.Size = new System.Drawing.Size(87, 36);
            this.bExtraction.TabIndex = 1;
            this.bExtraction.Text = "生成文件";
            this.bExtraction.Type = AntdUI.TTypeMini.Primary;
            this.bExtraction.Click += new System.EventHandler(this.bExtraction_Click);
            // 
            // txtExtraction
            // 
            this.txtExtraction.AutoScroll = true;
            this.txtExtraction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtExtraction.Location = new System.Drawing.Point(0, 0);
            this.txtExtraction.Margin = new System.Windows.Forms.Padding(0);
            this.txtExtraction.Multiline = true;
            this.txtExtraction.Name = "txtExtraction";
            this.txtExtraction.ReadOnly = true;
            this.txtExtraction.Size = new System.Drawing.Size(1094, 340);
            this.txtExtraction.TabIndex = 1;
            // 
            // tlpExtractionData
            // 
            this.tlpExtractionData.ColumnCount = 1;
            this.tlpExtractionData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpExtractionData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpExtractionData.Controls.Add(this.splitterExtractionData, 0, 0);
            this.tlpExtractionData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpExtractionData.Location = new System.Drawing.Point(0, 0);
            this.tlpExtractionData.Margin = new System.Windows.Forms.Padding(0);
            this.tlpExtractionData.Name = "tlpExtractionData";
            this.tlpExtractionData.RowCount = 1;
            this.tlpExtractionData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpExtractionData.Size = new System.Drawing.Size(1100, 800);
            this.tlpExtractionData.TabIndex = 0;
            // 
            // ExtractionData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpExtractionData);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ExtractionData";
            this.Size = new System.Drawing.Size(1100, 800);
            this.Load += new System.EventHandler(this.ExtractionData_Load);
            this.splitterExtractionData.Panel1.ResumeLayout(false);
            this.splitterExtractionData.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitterExtractionData)).EndInit();
            this.splitterExtractionData.ResumeLayout(false);
            this.tlpExtractionSelectFile.ResumeLayout(false);
            this.tlpData.ResumeLayout(false);
            this.tlpExtractionButton.ResumeLayout(false);
            this.tlpExtractionButton.PerformLayout();
            this.tlpExtractionData.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AntdUI.Splitter splitterExtractionData;
        private TableLayoutPanelEx tlpExtractionSelectFile;
        private AntdUI.Select ddlExtraction;
        private AntdUI.UploadDragger udExtraction;
        private TableLayoutPanelEx tlpData;
        private TableLayoutPanelEx tlpExtractionButton;
        private AntdUI.Button bExtraction;
        private AntdUI.Input txtExtraction;
        private TableLayoutPanelEx tlpExtractionData;
    }
}
