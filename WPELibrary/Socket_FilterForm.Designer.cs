
namespace WPELibrary
{
    partial class Socket_FilterForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Socket_FilterForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlpFilterForm = new System.Windows.Forms.TableLayoutPanel();
            this.tlpFilterButton = new System.Windows.Forms.TableLayoutPanel();
            this.bFilterButton_Save = new System.Windows.Forms.Button();
            this.bFilterButton_Close = new System.Windows.Forms.Button();
            this.tcFilterInfo = new System.Windows.Forms.TabControl();
            this.tpNormal = new System.Windows.Forms.TabPage();
            this.dgvFilterNormal = new System.Windows.Forms.DataGridView();
            this.cmsDGV = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsDGV_Progression_Enable = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsDGV_Progression_Disable = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsDGV_Copy = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsDGV_Cut = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsDGV_Paste = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsDGV_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.tpAdvanced = new System.Windows.Forms.TabPage();
            this.tlpFilterAdvanced = new System.Windows.Forms.TableLayoutPanel();
            this.dgvFilterAdvanced_Modify_FromPosition = new System.Windows.Forms.DataGridView();
            this.dgvFilterAdvanced_Search = new System.Windows.Forms.DataGridView();
            this.dgvFilterAdvanced_Modify_FromHead = new System.Windows.Forms.DataGridView();
            this.tlpFilterParameter = new System.Windows.Forms.TableLayoutPanel();
            this.gbProgression = new System.Windows.Forms.GroupBox();
            this.tlpProgression = new System.Windows.Forms.TableLayoutPanel();
            this.nudProgressionStep = new System.Windows.Forms.NumericUpDown();
            this.lProgressionStep = new System.Windows.Forms.Label();
            this.gbFilterAppoint = new System.Windows.Forms.GroupBox();
            this.tlpFilterAppoint = new System.Windows.Forms.TableLayoutPanel();
            this.nudFilter_LengthContent = new System.Windows.Forms.NumericUpDown();
            this.cbFilter_AppointLength = new System.Windows.Forms.CheckBox();
            this.cbFilter_AppointSocket = new System.Windows.Forms.CheckBox();
            this.nudFilter_SocketContent = new System.Windows.Forms.NumericUpDown();
            this.gbFilterAction = new System.Windows.Forms.GroupBox();
            this.tlpFilterAction = new System.Windows.Forms.TableLayoutPanel();
            this.cbbFilterAction_Execute = new System.Windows.Forms.ComboBox();
            this.rbFilterAction_Intercept = new System.Windows.Forms.RadioButton();
            this.rbFilterAction_Replace = new System.Windows.Forms.RadioButton();
            this.rbFilterAction_NoModify_Display = new System.Windows.Forms.RadioButton();
            this.rbFilterAction_NoModify_NoDisplay = new System.Windows.Forms.RadioButton();
            this.cbFilterAction_Execute = new System.Windows.Forms.CheckBox();
            this.gbFilterFunction = new System.Windows.Forms.GroupBox();
            this.tlpFilterFunction = new System.Windows.Forms.TableLayoutPanel();
            this.cbFilterFunction_RecvFrom = new System.Windows.Forms.CheckBox();
            this.cbFilterFunction_Send = new System.Windows.Forms.CheckBox();
            this.cbFilterFunction_SendTo = new System.Windows.Forms.CheckBox();
            this.cbFilterFunction_Recv = new System.Windows.Forms.CheckBox();
            this.cbFilterFunction_WSASend = new System.Windows.Forms.CheckBox();
            this.cbFilterFunction_WSASendTo = new System.Windows.Forms.CheckBox();
            this.cbFilterFunction_WSARecv = new System.Windows.Forms.CheckBox();
            this.cbFilterFunction_WSARecvFrom = new System.Windows.Forms.CheckBox();
            this.tlpFilterNameAndAppoint = new System.Windows.Forms.TableLayoutPanel();
            this.gbFilterAppoint_Advance = new System.Windows.Forms.GroupBox();
            this.tlpFilterAppoint_Advance = new System.Windows.Forms.TableLayoutPanel();
            this.txtFilter_HeaderContent = new System.Windows.Forms.TextBox();
            this.cbFilter_AppointHeader = new System.Windows.Forms.CheckBox();
            this.gbFilterName = new System.Windows.Forms.GroupBox();
            this.tlpFilterName = new System.Windows.Forms.TableLayoutPanel();
            this.txtFilterName = new System.Windows.Forms.TextBox();
            this.tlpFilterModifyFromAndMode = new System.Windows.Forms.TableLayoutPanel();
            this.gbFilterMode = new System.Windows.Forms.GroupBox();
            this.tlpFilterMode = new System.Windows.Forms.TableLayoutPanel();
            this.rbFilterMode_Advanced = new System.Windows.Forms.RadioButton();
            this.rbFilterMode_Normal = new System.Windows.Forms.RadioButton();
            this.gbFilterModifyFrom = new System.Windows.Forms.GroupBox();
            this.tlpFilterModifyFrom = new System.Windows.Forms.TableLayoutPanel();
            this.rbFilterModifyFrom_Position = new System.Windows.Forms.RadioButton();
            this.rbFilterModifyFrom_Head = new System.Windows.Forms.RadioButton();
            this.tlpFilterForm.SuspendLayout();
            this.tlpFilterButton.SuspendLayout();
            this.tcFilterInfo.SuspendLayout();
            this.tpNormal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilterNormal)).BeginInit();
            this.cmsDGV.SuspendLayout();
            this.tpAdvanced.SuspendLayout();
            this.tlpFilterAdvanced.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilterAdvanced_Modify_FromPosition)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilterAdvanced_Search)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilterAdvanced_Modify_FromHead)).BeginInit();
            this.tlpFilterParameter.SuspendLayout();
            this.gbProgression.SuspendLayout();
            this.tlpProgression.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudProgressionStep)).BeginInit();
            this.gbFilterAppoint.SuspendLayout();
            this.tlpFilterAppoint.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFilter_LengthContent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFilter_SocketContent)).BeginInit();
            this.gbFilterAction.SuspendLayout();
            this.tlpFilterAction.SuspendLayout();
            this.gbFilterFunction.SuspendLayout();
            this.tlpFilterFunction.SuspendLayout();
            this.tlpFilterNameAndAppoint.SuspendLayout();
            this.gbFilterAppoint_Advance.SuspendLayout();
            this.tlpFilterAppoint_Advance.SuspendLayout();
            this.gbFilterName.SuspendLayout();
            this.tlpFilterName.SuspendLayout();
            this.tlpFilterModifyFromAndMode.SuspendLayout();
            this.gbFilterMode.SuspendLayout();
            this.tlpFilterMode.SuspendLayout();
            this.gbFilterModifyFrom.SuspendLayout();
            this.tlpFilterModifyFrom.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpFilterForm
            // 
            resources.ApplyResources(this.tlpFilterForm, "tlpFilterForm");
            this.tlpFilterForm.Controls.Add(this.tlpFilterButton, 0, 2);
            this.tlpFilterForm.Controls.Add(this.tcFilterInfo, 0, 0);
            this.tlpFilterForm.Controls.Add(this.tlpFilterParameter, 0, 1);
            this.tlpFilterForm.Name = "tlpFilterForm";
            // 
            // tlpFilterButton
            // 
            resources.ApplyResources(this.tlpFilterButton, "tlpFilterButton");
            this.tlpFilterButton.Controls.Add(this.bFilterButton_Save, 1, 0);
            this.tlpFilterButton.Controls.Add(this.bFilterButton_Close, 3, 0);
            this.tlpFilterButton.Name = "tlpFilterButton";
            // 
            // bFilterButton_Save
            // 
            resources.ApplyResources(this.bFilterButton_Save, "bFilterButton_Save");
            this.bFilterButton_Save.Name = "bFilterButton_Save";
            this.bFilterButton_Save.UseVisualStyleBackColor = true;
            this.bFilterButton_Save.Click += new System.EventHandler(this.bFilterButton_Save_Click);
            // 
            // bFilterButton_Close
            // 
            resources.ApplyResources(this.bFilterButton_Close, "bFilterButton_Close");
            this.bFilterButton_Close.Name = "bFilterButton_Close";
            this.bFilterButton_Close.UseVisualStyleBackColor = true;
            this.bFilterButton_Close.Click += new System.EventHandler(this.bFilterButton_Close_Click);
            // 
            // tcFilterInfo
            // 
            this.tcFilterInfo.Controls.Add(this.tpNormal);
            this.tcFilterInfo.Controls.Add(this.tpAdvanced);
            resources.ApplyResources(this.tcFilterInfo, "tcFilterInfo");
            this.tcFilterInfo.Name = "tcFilterInfo";
            this.tcFilterInfo.SelectedIndex = 0;
            // 
            // tpNormal
            // 
            this.tpNormal.Controls.Add(this.dgvFilterNormal);
            resources.ApplyResources(this.tpNormal, "tpNormal");
            this.tpNormal.Name = "tpNormal";
            this.tpNormal.UseVisualStyleBackColor = true;
            // 
            // dgvFilterNormal
            // 
            this.dgvFilterNormal.AllowUserToAddRows = false;
            this.dgvFilterNormal.AllowUserToDeleteRows = false;
            this.dgvFilterNormal.AllowUserToResizeColumns = false;
            this.dgvFilterNormal.AllowUserToResizeRows = false;
            this.dgvFilterNormal.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilterNormal.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvFilterNormal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFilterNormal.ContextMenuStrip = this.cmsDGV;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilterNormal.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.dgvFilterNormal, "dgvFilterNormal");
            this.dgvFilterNormal.MultiSelect = false;
            this.dgvFilterNormal.Name = "dgvFilterNormal";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilterNormal.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvFilterNormal.RowHeadersVisible = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgvFilterNormal.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvFilterNormal.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgvFilterNormal.RowTemplate.DefaultCellStyle.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilterNormal.RowTemplate.Height = 30;
            this.dgvFilterNormal.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DGV_CellFormatting);
            // 
            // cmsDGV
            // 
            this.cmsDGV.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsDGV_Progression_Enable,
            this.cmsDGV_Progression_Disable,
            this.toolStripSeparator1,
            this.cmsDGV_Copy,
            this.cmsDGV_Cut,
            this.cmsDGV_Paste,
            this.cmsDGV_Delete});
            this.cmsDGV.Name = "cmsDGV";
            resources.ApplyResources(this.cmsDGV, "cmsDGV");
            this.cmsDGV.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsDGV_ItemClicked);
            // 
            // cmsDGV_Progression_Enable
            // 
            this.cmsDGV_Progression_Enable.Image = global::WPELibrary.Properties.Resources.on;
            this.cmsDGV_Progression_Enable.Name = "cmsDGV_Progression_Enable";
            resources.ApplyResources(this.cmsDGV_Progression_Enable, "cmsDGV_Progression_Enable");
            // 
            // cmsDGV_Progression_Disable
            // 
            this.cmsDGV_Progression_Disable.Image = global::WPELibrary.Properties.Resources.off;
            this.cmsDGV_Progression_Disable.Name = "cmsDGV_Progression_Disable";
            resources.ApplyResources(this.cmsDGV_Progression_Disable, "cmsDGV_Progression_Disable");
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // cmsDGV_Copy
            // 
            this.cmsDGV_Copy.Image = global::WPELibrary.Properties.Resources.copy;
            this.cmsDGV_Copy.Name = "cmsDGV_Copy";
            resources.ApplyResources(this.cmsDGV_Copy, "cmsDGV_Copy");
            // 
            // cmsDGV_Cut
            // 
            this.cmsDGV_Cut.Image = global::WPELibrary.Properties.Resources.cut;
            this.cmsDGV_Cut.Name = "cmsDGV_Cut";
            resources.ApplyResources(this.cmsDGV_Cut, "cmsDGV_Cut");
            // 
            // cmsDGV_Paste
            // 
            this.cmsDGV_Paste.Image = global::WPELibrary.Properties.Resources.paste;
            resources.ApplyResources(this.cmsDGV_Paste, "cmsDGV_Paste");
            this.cmsDGV_Paste.Name = "cmsDGV_Paste";
            // 
            // cmsDGV_Delete
            // 
            this.cmsDGV_Delete.Image = global::WPELibrary.Properties.Resources.Delete;
            resources.ApplyResources(this.cmsDGV_Delete, "cmsDGV_Delete");
            this.cmsDGV_Delete.Name = "cmsDGV_Delete";
            // 
            // tpAdvanced
            // 
            this.tpAdvanced.Controls.Add(this.tlpFilterAdvanced);
            resources.ApplyResources(this.tpAdvanced, "tpAdvanced");
            this.tpAdvanced.Name = "tpAdvanced";
            this.tpAdvanced.UseVisualStyleBackColor = true;
            // 
            // tlpFilterAdvanced
            // 
            resources.ApplyResources(this.tlpFilterAdvanced, "tlpFilterAdvanced");
            this.tlpFilterAdvanced.Controls.Add(this.dgvFilterAdvanced_Modify_FromPosition, 0, 2);
            this.tlpFilterAdvanced.Controls.Add(this.dgvFilterAdvanced_Search, 0, 0);
            this.tlpFilterAdvanced.Controls.Add(this.dgvFilterAdvanced_Modify_FromHead, 0, 1);
            this.tlpFilterAdvanced.Name = "tlpFilterAdvanced";
            // 
            // dgvFilterAdvanced_Modify_FromPosition
            // 
            this.dgvFilterAdvanced_Modify_FromPosition.AllowUserToAddRows = false;
            this.dgvFilterAdvanced_Modify_FromPosition.AllowUserToDeleteRows = false;
            this.dgvFilterAdvanced_Modify_FromPosition.AllowUserToResizeColumns = false;
            this.dgvFilterAdvanced_Modify_FromPosition.AllowUserToResizeRows = false;
            this.dgvFilterAdvanced_Modify_FromPosition.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilterAdvanced_Modify_FromPosition.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvFilterAdvanced_Modify_FromPosition.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFilterAdvanced_Modify_FromPosition.ContextMenuStrip = this.cmsDGV;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilterAdvanced_Modify_FromPosition.DefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.dgvFilterAdvanced_Modify_FromPosition, "dgvFilterAdvanced_Modify_FromPosition");
            this.dgvFilterAdvanced_Modify_FromPosition.MultiSelect = false;
            this.dgvFilterAdvanced_Modify_FromPosition.Name = "dgvFilterAdvanced_Modify_FromPosition";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilterAdvanced_Modify_FromPosition.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvFilterAdvanced_Modify_FromPosition.RowHeadersVisible = false;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgvFilterAdvanced_Modify_FromPosition.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvFilterAdvanced_Modify_FromPosition.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgvFilterAdvanced_Modify_FromPosition.RowTemplate.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(3);
            this.dgvFilterAdvanced_Modify_FromPosition.RowTemplate.Height = 30;
            this.dgvFilterAdvanced_Modify_FromPosition.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DGV_CellFormatting);
            this.dgvFilterAdvanced_Modify_FromPosition.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvFilterAdvanced_Modify_FromPosition_ColumnAdded);
            // 
            // dgvFilterAdvanced_Search
            // 
            this.dgvFilterAdvanced_Search.AllowUserToAddRows = false;
            this.dgvFilterAdvanced_Search.AllowUserToDeleteRows = false;
            this.dgvFilterAdvanced_Search.AllowUserToResizeColumns = false;
            this.dgvFilterAdvanced_Search.AllowUserToResizeRows = false;
            this.dgvFilterAdvanced_Search.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilterAdvanced_Search.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvFilterAdvanced_Search.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFilterAdvanced_Search.ContextMenuStrip = this.cmsDGV;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle10.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilterAdvanced_Search.DefaultCellStyle = dataGridViewCellStyle10;
            resources.ApplyResources(this.dgvFilterAdvanced_Search, "dgvFilterAdvanced_Search");
            this.dgvFilterAdvanced_Search.MultiSelect = false;
            this.dgvFilterAdvanced_Search.Name = "dgvFilterAdvanced_Search";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilterAdvanced_Search.RowHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.dgvFilterAdvanced_Search.RowHeadersVisible = false;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgvFilterAdvanced_Search.RowsDefaultCellStyle = dataGridViewCellStyle12;
            this.dgvFilterAdvanced_Search.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgvFilterAdvanced_Search.RowTemplate.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(3);
            this.dgvFilterAdvanced_Search.RowTemplate.Height = 30;
            this.dgvFilterAdvanced_Search.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DGV_CellFormatting);
            // 
            // dgvFilterAdvanced_Modify_FromHead
            // 
            this.dgvFilterAdvanced_Modify_FromHead.AllowUserToAddRows = false;
            this.dgvFilterAdvanced_Modify_FromHead.AllowUserToDeleteRows = false;
            this.dgvFilterAdvanced_Modify_FromHead.AllowUserToResizeColumns = false;
            this.dgvFilterAdvanced_Modify_FromHead.AllowUserToResizeRows = false;
            this.dgvFilterAdvanced_Modify_FromHead.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle13.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle13.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilterAdvanced_Modify_FromHead.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            this.dgvFilterAdvanced_Modify_FromHead.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFilterAdvanced_Modify_FromHead.ContextMenuStrip = this.cmsDGV;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle14.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle14.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilterAdvanced_Modify_FromHead.DefaultCellStyle = dataGridViewCellStyle14;
            resources.ApplyResources(this.dgvFilterAdvanced_Modify_FromHead, "dgvFilterAdvanced_Modify_FromHead");
            this.dgvFilterAdvanced_Modify_FromHead.MultiSelect = false;
            this.dgvFilterAdvanced_Modify_FromHead.Name = "dgvFilterAdvanced_Modify_FromHead";
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle15.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle15.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle15.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilterAdvanced_Modify_FromHead.RowHeadersDefaultCellStyle = dataGridViewCellStyle15;
            this.dgvFilterAdvanced_Modify_FromHead.RowHeadersVisible = false;
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle16.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgvFilterAdvanced_Modify_FromHead.RowsDefaultCellStyle = dataGridViewCellStyle16;
            this.dgvFilterAdvanced_Modify_FromHead.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgvFilterAdvanced_Modify_FromHead.RowTemplate.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(3);
            this.dgvFilterAdvanced_Modify_FromHead.RowTemplate.Height = 30;
            this.dgvFilterAdvanced_Modify_FromHead.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.DGV_CellFormatting);
            // 
            // tlpFilterParameter
            // 
            resources.ApplyResources(this.tlpFilterParameter, "tlpFilterParameter");
            this.tlpFilterParameter.Controls.Add(this.gbProgression, 2, 1);
            this.tlpFilterParameter.Controls.Add(this.gbFilterAppoint, 0, 1);
            this.tlpFilterParameter.Controls.Add(this.gbFilterAction, 0, 0);
            this.tlpFilterParameter.Controls.Add(this.gbFilterFunction, 2, 0);
            this.tlpFilterParameter.Controls.Add(this.tlpFilterNameAndAppoint, 1, 0);
            this.tlpFilterParameter.Controls.Add(this.tlpFilterModifyFromAndMode, 1, 1);
            this.tlpFilterParameter.Name = "tlpFilterParameter";
            // 
            // gbProgression
            // 
            this.gbProgression.Controls.Add(this.tlpProgression);
            resources.ApplyResources(this.gbProgression, "gbProgression");
            this.gbProgression.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbProgression.Name = "gbProgression";
            this.gbProgression.TabStop = false;
            // 
            // tlpProgression
            // 
            resources.ApplyResources(this.tlpProgression, "tlpProgression");
            this.tlpProgression.Controls.Add(this.nudProgressionStep, 1, 0);
            this.tlpProgression.Controls.Add(this.lProgressionStep, 0, 0);
            this.tlpProgression.Name = "tlpProgression";
            // 
            // nudProgressionStep
            // 
            resources.ApplyResources(this.nudProgressionStep, "nudProgressionStep");
            this.nudProgressionStep.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudProgressionStep.Minimum = new decimal(new int[] {
            255,
            0,
            0,
            -2147483648});
            this.nudProgressionStep.Name = "nudProgressionStep";
            this.nudProgressionStep.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lProgressionStep
            // 
            resources.ApplyResources(this.lProgressionStep, "lProgressionStep");
            this.lProgressionStep.Name = "lProgressionStep";
            // 
            // gbFilterAppoint
            // 
            this.gbFilterAppoint.Controls.Add(this.tlpFilterAppoint);
            resources.ApplyResources(this.gbFilterAppoint, "gbFilterAppoint");
            this.gbFilterAppoint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbFilterAppoint.Name = "gbFilterAppoint";
            this.gbFilterAppoint.TabStop = false;
            // 
            // tlpFilterAppoint
            // 
            resources.ApplyResources(this.tlpFilterAppoint, "tlpFilterAppoint");
            this.tlpFilterAppoint.Controls.Add(this.nudFilter_LengthContent, 1, 1);
            this.tlpFilterAppoint.Controls.Add(this.cbFilter_AppointLength, 0, 1);
            this.tlpFilterAppoint.Controls.Add(this.cbFilter_AppointSocket, 0, 0);
            this.tlpFilterAppoint.Controls.Add(this.nudFilter_SocketContent, 1, 0);
            this.tlpFilterAppoint.Name = "tlpFilterAppoint";
            // 
            // nudFilter_LengthContent
            // 
            resources.ApplyResources(this.nudFilter_LengthContent, "nudFilter_LengthContent");
            this.nudFilter_LengthContent.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.nudFilter_LengthContent.Name = "nudFilter_LengthContent";
            // 
            // cbFilter_AppointLength
            // 
            resources.ApplyResources(this.cbFilter_AppointLength, "cbFilter_AppointLength");
            this.cbFilter_AppointLength.Name = "cbFilter_AppointLength";
            this.cbFilter_AppointLength.UseVisualStyleBackColor = true;
            this.cbFilter_AppointLength.CheckedChanged += new System.EventHandler(this.cbFilter_AppointLength_CheckedChanged);
            // 
            // cbFilter_AppointSocket
            // 
            resources.ApplyResources(this.cbFilter_AppointSocket, "cbFilter_AppointSocket");
            this.cbFilter_AppointSocket.Name = "cbFilter_AppointSocket";
            this.cbFilter_AppointSocket.UseVisualStyleBackColor = true;
            this.cbFilter_AppointSocket.CheckedChanged += new System.EventHandler(this.cbFilter_AppointSocket_CheckedChanged);
            // 
            // nudFilter_SocketContent
            // 
            resources.ApplyResources(this.nudFilter_SocketContent, "nudFilter_SocketContent");
            this.nudFilter_SocketContent.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.nudFilter_SocketContent.Name = "nudFilter_SocketContent";
            // 
            // gbFilterAction
            // 
            this.gbFilterAction.Controls.Add(this.tlpFilterAction);
            resources.ApplyResources(this.gbFilterAction, "gbFilterAction");
            this.gbFilterAction.Name = "gbFilterAction";
            this.gbFilterAction.TabStop = false;
            // 
            // tlpFilterAction
            // 
            resources.ApplyResources(this.tlpFilterAction, "tlpFilterAction");
            this.tlpFilterAction.Controls.Add(this.cbbFilterAction_Execute, 1, 3);
            this.tlpFilterAction.Controls.Add(this.rbFilterAction_Intercept, 0, 1);
            this.tlpFilterAction.Controls.Add(this.rbFilterAction_Replace, 0, 0);
            this.tlpFilterAction.Controls.Add(this.rbFilterAction_NoModify_Display, 1, 0);
            this.tlpFilterAction.Controls.Add(this.rbFilterAction_NoModify_NoDisplay, 1, 1);
            this.tlpFilterAction.Controls.Add(this.cbFilterAction_Execute, 0, 3);
            this.tlpFilterAction.Name = "tlpFilterAction";
            // 
            // cbbFilterAction_Execute
            // 
            resources.ApplyResources(this.cbbFilterAction_Execute, "cbbFilterAction_Execute");
            this.cbbFilterAction_Execute.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbFilterAction_Execute.FormattingEnabled = true;
            this.cbbFilterAction_Execute.Name = "cbbFilterAction_Execute";
            // 
            // rbFilterAction_Intercept
            // 
            resources.ApplyResources(this.rbFilterAction_Intercept, "rbFilterAction_Intercept");
            this.rbFilterAction_Intercept.Name = "rbFilterAction_Intercept";
            this.rbFilterAction_Intercept.UseVisualStyleBackColor = true;
            // 
            // rbFilterAction_Replace
            // 
            resources.ApplyResources(this.rbFilterAction_Replace, "rbFilterAction_Replace");
            this.rbFilterAction_Replace.Checked = true;
            this.rbFilterAction_Replace.Name = "rbFilterAction_Replace";
            this.rbFilterAction_Replace.TabStop = true;
            this.rbFilterAction_Replace.UseVisualStyleBackColor = true;
            // 
            // rbFilterAction_NoModify_Display
            // 
            resources.ApplyResources(this.rbFilterAction_NoModify_Display, "rbFilterAction_NoModify_Display");
            this.rbFilterAction_NoModify_Display.Name = "rbFilterAction_NoModify_Display";
            this.rbFilterAction_NoModify_Display.UseVisualStyleBackColor = true;
            // 
            // rbFilterAction_NoModify_NoDisplay
            // 
            resources.ApplyResources(this.rbFilterAction_NoModify_NoDisplay, "rbFilterAction_NoModify_NoDisplay");
            this.rbFilterAction_NoModify_NoDisplay.Name = "rbFilterAction_NoModify_NoDisplay";
            this.rbFilterAction_NoModify_NoDisplay.UseVisualStyleBackColor = true;
            // 
            // cbFilterAction_Execute
            // 
            resources.ApplyResources(this.cbFilterAction_Execute, "cbFilterAction_Execute");
            this.cbFilterAction_Execute.Name = "cbFilterAction_Execute";
            this.cbFilterAction_Execute.UseVisualStyleBackColor = true;
            this.cbFilterAction_Execute.CheckedChanged += new System.EventHandler(this.cbFilterAction_Execute_CheckedChanged);
            // 
            // gbFilterFunction
            // 
            this.gbFilterFunction.Controls.Add(this.tlpFilterFunction);
            resources.ApplyResources(this.gbFilterFunction, "gbFilterFunction");
            this.gbFilterFunction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbFilterFunction.Name = "gbFilterFunction";
            this.gbFilterFunction.TabStop = false;
            // 
            // tlpFilterFunction
            // 
            resources.ApplyResources(this.tlpFilterFunction, "tlpFilterFunction");
            this.tlpFilterFunction.Controls.Add(this.cbFilterFunction_RecvFrom, 0, 3);
            this.tlpFilterFunction.Controls.Add(this.cbFilterFunction_Send, 0, 0);
            this.tlpFilterFunction.Controls.Add(this.cbFilterFunction_SendTo, 0, 1);
            this.tlpFilterFunction.Controls.Add(this.cbFilterFunction_Recv, 0, 2);
            this.tlpFilterFunction.Controls.Add(this.cbFilterFunction_WSASend, 2, 0);
            this.tlpFilterFunction.Controls.Add(this.cbFilterFunction_WSASendTo, 2, 1);
            this.tlpFilterFunction.Controls.Add(this.cbFilterFunction_WSARecv, 2, 2);
            this.tlpFilterFunction.Controls.Add(this.cbFilterFunction_WSARecvFrom, 2, 3);
            this.tlpFilterFunction.Name = "tlpFilterFunction";
            // 
            // cbFilterFunction_RecvFrom
            // 
            resources.ApplyResources(this.cbFilterFunction_RecvFrom, "cbFilterFunction_RecvFrom");
            this.cbFilterFunction_RecvFrom.Checked = true;
            this.cbFilterFunction_RecvFrom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbFilterFunction_RecvFrom.Name = "cbFilterFunction_RecvFrom";
            this.cbFilterFunction_RecvFrom.UseVisualStyleBackColor = true;
            // 
            // cbFilterFunction_Send
            // 
            resources.ApplyResources(this.cbFilterFunction_Send, "cbFilterFunction_Send");
            this.cbFilterFunction_Send.Checked = true;
            this.cbFilterFunction_Send.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbFilterFunction_Send.Name = "cbFilterFunction_Send";
            this.cbFilterFunction_Send.UseVisualStyleBackColor = true;
            // 
            // cbFilterFunction_SendTo
            // 
            resources.ApplyResources(this.cbFilterFunction_SendTo, "cbFilterFunction_SendTo");
            this.cbFilterFunction_SendTo.Checked = true;
            this.cbFilterFunction_SendTo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbFilterFunction_SendTo.Name = "cbFilterFunction_SendTo";
            this.cbFilterFunction_SendTo.UseVisualStyleBackColor = true;
            // 
            // cbFilterFunction_Recv
            // 
            resources.ApplyResources(this.cbFilterFunction_Recv, "cbFilterFunction_Recv");
            this.cbFilterFunction_Recv.Checked = true;
            this.cbFilterFunction_Recv.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbFilterFunction_Recv.Name = "cbFilterFunction_Recv";
            this.cbFilterFunction_Recv.UseVisualStyleBackColor = true;
            // 
            // cbFilterFunction_WSASend
            // 
            resources.ApplyResources(this.cbFilterFunction_WSASend, "cbFilterFunction_WSASend");
            this.cbFilterFunction_WSASend.Checked = true;
            this.cbFilterFunction_WSASend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbFilterFunction_WSASend.Name = "cbFilterFunction_WSASend";
            this.cbFilterFunction_WSASend.UseVisualStyleBackColor = true;
            // 
            // cbFilterFunction_WSASendTo
            // 
            resources.ApplyResources(this.cbFilterFunction_WSASendTo, "cbFilterFunction_WSASendTo");
            this.cbFilterFunction_WSASendTo.Checked = true;
            this.cbFilterFunction_WSASendTo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbFilterFunction_WSASendTo.Name = "cbFilterFunction_WSASendTo";
            this.cbFilterFunction_WSASendTo.UseVisualStyleBackColor = true;
            // 
            // cbFilterFunction_WSARecv
            // 
            resources.ApplyResources(this.cbFilterFunction_WSARecv, "cbFilterFunction_WSARecv");
            this.cbFilterFunction_WSARecv.Checked = true;
            this.cbFilterFunction_WSARecv.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbFilterFunction_WSARecv.Name = "cbFilterFunction_WSARecv";
            this.cbFilterFunction_WSARecv.UseVisualStyleBackColor = true;
            // 
            // cbFilterFunction_WSARecvFrom
            // 
            resources.ApplyResources(this.cbFilterFunction_WSARecvFrom, "cbFilterFunction_WSARecvFrom");
            this.cbFilterFunction_WSARecvFrom.Checked = true;
            this.cbFilterFunction_WSARecvFrom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbFilterFunction_WSARecvFrom.Name = "cbFilterFunction_WSARecvFrom";
            this.cbFilterFunction_WSARecvFrom.UseVisualStyleBackColor = true;
            // 
            // tlpFilterNameAndAppoint
            // 
            resources.ApplyResources(this.tlpFilterNameAndAppoint, "tlpFilterNameAndAppoint");
            this.tlpFilterNameAndAppoint.Controls.Add(this.gbFilterAppoint_Advance, 0, 1);
            this.tlpFilterNameAndAppoint.Controls.Add(this.gbFilterName, 0, 0);
            this.tlpFilterNameAndAppoint.Name = "tlpFilterNameAndAppoint";
            // 
            // gbFilterAppoint_Advance
            // 
            this.gbFilterAppoint_Advance.Controls.Add(this.tlpFilterAppoint_Advance);
            resources.ApplyResources(this.gbFilterAppoint_Advance, "gbFilterAppoint_Advance");
            this.gbFilterAppoint_Advance.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbFilterAppoint_Advance.Name = "gbFilterAppoint_Advance";
            this.gbFilterAppoint_Advance.TabStop = false;
            // 
            // tlpFilterAppoint_Advance
            // 
            resources.ApplyResources(this.tlpFilterAppoint_Advance, "tlpFilterAppoint_Advance");
            this.tlpFilterAppoint_Advance.Controls.Add(this.txtFilter_HeaderContent, 1, 0);
            this.tlpFilterAppoint_Advance.Controls.Add(this.cbFilter_AppointHeader, 0, 0);
            this.tlpFilterAppoint_Advance.Name = "tlpFilterAppoint_Advance";
            // 
            // txtFilter_HeaderContent
            // 
            this.txtFilter_HeaderContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFilter_HeaderContent.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            resources.ApplyResources(this.txtFilter_HeaderContent, "txtFilter_HeaderContent");
            this.txtFilter_HeaderContent.Name = "txtFilter_HeaderContent";
            this.txtFilter_HeaderContent.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFilter_HeaderContent_KeyPress);
            // 
            // cbFilter_AppointHeader
            // 
            resources.ApplyResources(this.cbFilter_AppointHeader, "cbFilter_AppointHeader");
            this.cbFilter_AppointHeader.Name = "cbFilter_AppointHeader";
            this.cbFilter_AppointHeader.UseVisualStyleBackColor = true;
            this.cbFilter_AppointHeader.CheckedChanged += new System.EventHandler(this.cbFilter_AppointHeader_CheckedChanged);
            // 
            // gbFilterName
            // 
            this.gbFilterName.Controls.Add(this.tlpFilterName);
            resources.ApplyResources(this.gbFilterName, "gbFilterName");
            this.gbFilterName.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbFilterName.Name = "gbFilterName";
            this.gbFilterName.TabStop = false;
            // 
            // tlpFilterName
            // 
            resources.ApplyResources(this.tlpFilterName, "tlpFilterName");
            this.tlpFilterName.Controls.Add(this.txtFilterName, 0, 0);
            this.tlpFilterName.Name = "tlpFilterName";
            // 
            // txtFilterName
            // 
            this.txtFilterName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.txtFilterName, "txtFilterName");
            this.txtFilterName.Name = "txtFilterName";
            // 
            // tlpFilterModifyFromAndMode
            // 
            resources.ApplyResources(this.tlpFilterModifyFromAndMode, "tlpFilterModifyFromAndMode");
            this.tlpFilterModifyFromAndMode.Controls.Add(this.gbFilterMode, 1, 0);
            this.tlpFilterModifyFromAndMode.Controls.Add(this.gbFilterModifyFrom, 0, 0);
            this.tlpFilterModifyFromAndMode.Name = "tlpFilterModifyFromAndMode";
            // 
            // gbFilterMode
            // 
            this.gbFilterMode.Controls.Add(this.tlpFilterMode);
            resources.ApplyResources(this.gbFilterMode, "gbFilterMode");
            this.gbFilterMode.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbFilterMode.Name = "gbFilterMode";
            this.gbFilterMode.TabStop = false;
            // 
            // tlpFilterMode
            // 
            resources.ApplyResources(this.tlpFilterMode, "tlpFilterMode");
            this.tlpFilterMode.Controls.Add(this.rbFilterMode_Advanced, 0, 1);
            this.tlpFilterMode.Controls.Add(this.rbFilterMode_Normal, 0, 0);
            this.tlpFilterMode.Name = "tlpFilterMode";
            // 
            // rbFilterMode_Advanced
            // 
            resources.ApplyResources(this.rbFilterMode_Advanced, "rbFilterMode_Advanced");
            this.rbFilterMode_Advanced.Name = "rbFilterMode_Advanced";
            this.rbFilterMode_Advanced.UseVisualStyleBackColor = true;
            this.rbFilterMode_Advanced.CheckedChanged += new System.EventHandler(this.rbFilterMode_CheckedChanged);
            // 
            // rbFilterMode_Normal
            // 
            resources.ApplyResources(this.rbFilterMode_Normal, "rbFilterMode_Normal");
            this.rbFilterMode_Normal.Checked = true;
            this.rbFilterMode_Normal.Name = "rbFilterMode_Normal";
            this.rbFilterMode_Normal.TabStop = true;
            this.rbFilterMode_Normal.UseVisualStyleBackColor = true;
            this.rbFilterMode_Normal.CheckedChanged += new System.EventHandler(this.rbFilterMode_CheckedChanged);
            // 
            // gbFilterModifyFrom
            // 
            this.gbFilterModifyFrom.Controls.Add(this.tlpFilterModifyFrom);
            resources.ApplyResources(this.gbFilterModifyFrom, "gbFilterModifyFrom");
            this.gbFilterModifyFrom.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbFilterModifyFrom.Name = "gbFilterModifyFrom";
            this.gbFilterModifyFrom.TabStop = false;
            // 
            // tlpFilterModifyFrom
            // 
            resources.ApplyResources(this.tlpFilterModifyFrom, "tlpFilterModifyFrom");
            this.tlpFilterModifyFrom.Controls.Add(this.rbFilterModifyFrom_Position, 0, 1);
            this.tlpFilterModifyFrom.Controls.Add(this.rbFilterModifyFrom_Head, 0, 0);
            this.tlpFilterModifyFrom.Name = "tlpFilterModifyFrom";
            // 
            // rbFilterModifyFrom_Position
            // 
            resources.ApplyResources(this.rbFilterModifyFrom_Position, "rbFilterModifyFrom_Position");
            this.rbFilterModifyFrom_Position.Name = "rbFilterModifyFrom_Position";
            this.rbFilterModifyFrom_Position.UseVisualStyleBackColor = true;
            this.rbFilterModifyFrom_Position.CheckedChanged += new System.EventHandler(this.rbFilterModifyFrom_CheckedChanged);
            // 
            // rbFilterModifyFrom_Head
            // 
            resources.ApplyResources(this.rbFilterModifyFrom_Head, "rbFilterModifyFrom_Head");
            this.rbFilterModifyFrom_Head.Checked = true;
            this.rbFilterModifyFrom_Head.Name = "rbFilterModifyFrom_Head";
            this.rbFilterModifyFrom_Head.TabStop = true;
            this.rbFilterModifyFrom_Head.UseVisualStyleBackColor = true;
            this.rbFilterModifyFrom_Head.CheckedChanged += new System.EventHandler(this.rbFilterModifyFrom_CheckedChanged);
            // 
            // Socket_FilterForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpFilterForm);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Socket_FilterForm";
            this.TopMost = true;
            this.tlpFilterForm.ResumeLayout(false);
            this.tlpFilterButton.ResumeLayout(false);
            this.tcFilterInfo.ResumeLayout(false);
            this.tpNormal.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilterNormal)).EndInit();
            this.cmsDGV.ResumeLayout(false);
            this.tpAdvanced.ResumeLayout(false);
            this.tlpFilterAdvanced.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilterAdvanced_Modify_FromPosition)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilterAdvanced_Search)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilterAdvanced_Modify_FromHead)).EndInit();
            this.tlpFilterParameter.ResumeLayout(false);
            this.gbProgression.ResumeLayout(false);
            this.tlpProgression.ResumeLayout(false);
            this.tlpProgression.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudProgressionStep)).EndInit();
            this.gbFilterAppoint.ResumeLayout(false);
            this.tlpFilterAppoint.ResumeLayout(false);
            this.tlpFilterAppoint.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudFilter_LengthContent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFilter_SocketContent)).EndInit();
            this.gbFilterAction.ResumeLayout(false);
            this.tlpFilterAction.ResumeLayout(false);
            this.tlpFilterAction.PerformLayout();
            this.gbFilterFunction.ResumeLayout(false);
            this.tlpFilterFunction.ResumeLayout(false);
            this.tlpFilterFunction.PerformLayout();
            this.tlpFilterNameAndAppoint.ResumeLayout(false);
            this.gbFilterAppoint_Advance.ResumeLayout(false);
            this.tlpFilterAppoint_Advance.ResumeLayout(false);
            this.tlpFilterAppoint_Advance.PerformLayout();
            this.gbFilterName.ResumeLayout(false);
            this.tlpFilterName.ResumeLayout(false);
            this.tlpFilterName.PerformLayout();
            this.tlpFilterModifyFromAndMode.ResumeLayout(false);
            this.gbFilterMode.ResumeLayout(false);
            this.tlpFilterMode.ResumeLayout(false);
            this.tlpFilterMode.PerformLayout();
            this.gbFilterModifyFrom.ResumeLayout(false);
            this.tlpFilterModifyFrom.ResumeLayout(false);
            this.tlpFilterModifyFrom.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpFilterForm;
        private System.Windows.Forms.TabControl tcFilterInfo;
        private System.Windows.Forms.TabPage tpNormal;
        private System.Windows.Forms.DataGridView dgvFilterNormal;
        private System.Windows.Forms.TabPage tpAdvanced;
        private System.Windows.Forms.TableLayoutPanel tlpFilterAdvanced;
        private System.Windows.Forms.TableLayoutPanel tlpFilterParameter;
        private System.Windows.Forms.GroupBox gbFilterFunction;
        private System.Windows.Forms.TableLayoutPanel tlpFilterFunction;
        private System.Windows.Forms.CheckBox cbFilterFunction_RecvFrom;
        private System.Windows.Forms.CheckBox cbFilterFunction_Send;
        private System.Windows.Forms.CheckBox cbFilterFunction_SendTo;
        private System.Windows.Forms.CheckBox cbFilterFunction_Recv;
        private System.Windows.Forms.CheckBox cbFilterFunction_WSASend;
        private System.Windows.Forms.CheckBox cbFilterFunction_WSASendTo;
        private System.Windows.Forms.CheckBox cbFilterFunction_WSARecv;
        private System.Windows.Forms.CheckBox cbFilterFunction_WSARecvFrom;
        private System.Windows.Forms.DataGridView dgvFilterAdvanced_Modify_FromHead;
        private System.Windows.Forms.DataGridView dgvFilterAdvanced_Modify_FromPosition;
        private System.Windows.Forms.DataGridView dgvFilterAdvanced_Search;
        private System.Windows.Forms.TableLayoutPanel tlpFilterNameAndAppoint;
        private System.Windows.Forms.GroupBox gbFilterAppoint_Advance;
        private System.Windows.Forms.TableLayoutPanel tlpFilterAppoint_Advance;
        private System.Windows.Forms.TextBox txtFilter_HeaderContent;
        private System.Windows.Forms.GroupBox gbFilterName;
        private System.Windows.Forms.TableLayoutPanel tlpFilterName;
        private System.Windows.Forms.TextBox txtFilterName;
        private System.Windows.Forms.CheckBox cbFilter_AppointHeader;
        private System.Windows.Forms.ContextMenuStrip cmsDGV;
        private System.Windows.Forms.ToolStripMenuItem cmsDGV_Delete;
        private System.Windows.Forms.ToolStripMenuItem cmsDGV_Copy;
        private System.Windows.Forms.ToolStripMenuItem cmsDGV_Paste;
        private System.Windows.Forms.ToolStripMenuItem cmsDGV_Cut;
        private System.Windows.Forms.GroupBox gbFilterAction;
        private System.Windows.Forms.TableLayoutPanel tlpFilterAction;
        private System.Windows.Forms.RadioButton rbFilterAction_Intercept;
        private System.Windows.Forms.RadioButton rbFilterAction_Replace;
        private System.Windows.Forms.RadioButton rbFilterAction_NoModify_Display;
        private System.Windows.Forms.RadioButton rbFilterAction_NoModify_NoDisplay;
        private System.Windows.Forms.TableLayoutPanel tlpFilterModifyFromAndMode;
        private System.Windows.Forms.GroupBox gbFilterMode;
        private System.Windows.Forms.TableLayoutPanel tlpFilterMode;
        private System.Windows.Forms.RadioButton rbFilterMode_Advanced;
        private System.Windows.Forms.RadioButton rbFilterMode_Normal;
        private System.Windows.Forms.GroupBox gbFilterModifyFrom;
        private System.Windows.Forms.TableLayoutPanel tlpFilterModifyFrom;
        private System.Windows.Forms.RadioButton rbFilterModifyFrom_Position;
        private System.Windows.Forms.RadioButton rbFilterModifyFrom_Head;
        private System.Windows.Forms.GroupBox gbFilterAppoint;
        private System.Windows.Forms.TableLayoutPanel tlpFilterAppoint;
        private System.Windows.Forms.CheckBox cbFilter_AppointSocket;
        private System.Windows.Forms.CheckBox cbFilter_AppointLength;
        private System.Windows.Forms.NumericUpDown nudFilter_SocketContent;
        private System.Windows.Forms.NumericUpDown nudFilter_LengthContent;
        private System.Windows.Forms.TableLayoutPanel tlpFilterButton;
        private System.Windows.Forms.Button bFilterButton_Save;
        private System.Windows.Forms.Button bFilterButton_Close;
        private System.Windows.Forms.GroupBox gbProgression;
        private System.Windows.Forms.TableLayoutPanel tlpProgression;
        private System.Windows.Forms.NumericUpDown nudProgressionStep;
        private System.Windows.Forms.Label lProgressionStep;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem cmsDGV_Progression_Enable;
        private System.Windows.Forms.ToolStripMenuItem cmsDGV_Progression_Disable;
        private System.Windows.Forms.CheckBox cbFilterAction_Execute;
        private System.Windows.Forms.ComboBox cbbFilterAction_Execute;
    }
}