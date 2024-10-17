
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
            this.tlpFilterInfo_Function = new System.Windows.Forms.TableLayoutPanel();
            this.gbStartModify = new System.Windows.Forms.GroupBox();
            this.tlpFilterModifyFrom = new System.Windows.Forms.TableLayoutPanel();
            this.rbFromPosition = new System.Windows.Forms.RadioButton();
            this.rbFromHead = new System.Windows.Forms.RadioButton();
            this.tlpFilterButton = new System.Windows.Forms.TableLayoutPanel();
            this.bSave = new System.Windows.Forms.Button();
            this.bReset = new System.Windows.Forms.Button();
            this.gbFilterName = new System.Windows.Forms.GroupBox();
            this.tlpFilterName = new System.Windows.Forms.TableLayoutPanel();
            this.txtFilterName = new System.Windows.Forms.TextBox();
            this.gbFilterMode = new System.Windows.Forms.GroupBox();
            this.tlpFilterMode = new System.Windows.Forms.TableLayoutPanel();
            this.rbAdvanced = new System.Windows.Forms.RadioButton();
            this.rbNormal = new System.Windows.Forms.RadioButton();
            this.gbPacketLength = new System.Windows.Forms.GroupBox();
            this.tlpPacketLength = new System.Windows.Forms.TableLayoutPanel();
            this.bPacketLength = new System.Windows.Forms.Button();
            this.nudPacketLength = new System.Windows.Forms.NumericUpDown();
            this.rbPacketLength_Custom = new System.Windows.Forms.RadioButton();
            this.rbPacketLength_NoModify = new System.Windows.Forms.RadioButton();
            this.tlpModifyFunction = new System.Windows.Forms.TableLayoutPanel();
            this.gbModifyLength = new System.Windows.Forms.GroupBox();
            this.tlpSearchLength = new System.Windows.Forms.TableLayoutPanel();
            this.bModifyLength = new System.Windows.Forms.Button();
            this.nudModifyLength = new System.Windows.Forms.NumericUpDown();
            this.gbModifyCNT = new System.Windows.Forms.GroupBox();
            this.tlpModifyCNT = new System.Windows.Forms.TableLayoutPanel();
            this.nudModifyCNT = new System.Windows.Forms.NumericUpDown();
            this.bgwFilterInfo = new System.ComponentModel.BackgroundWorker();
            this.tlpFilterInfo = new System.Windows.Forms.TableLayoutPanel();
            this.tcFilterInfo = new System.Windows.Forms.TabControl();
            this.tpNormal = new System.Windows.Forms.TabPage();
            this.dgvFilter_Normal = new System.Windows.Forms.DataGridView();
            this.tpAdvanced = new System.Windows.Forms.TabPage();
            this.tlpFilterAdvanced = new System.Windows.Forms.TableLayoutPanel();
            this.dgvFilter_Advanced_Modify = new System.Windows.Forms.DataGridView();
            this.dgvFilter_Advanced_Search = new System.Windows.Forms.DataGridView();
            this.pbLoading = new System.Windows.Forms.PictureBox();
            this.tlpFilterInfo_Function.SuspendLayout();
            this.gbStartModify.SuspendLayout();
            this.tlpFilterModifyFrom.SuspendLayout();
            this.tlpFilterButton.SuspendLayout();
            this.gbFilterName.SuspendLayout();
            this.tlpFilterName.SuspendLayout();
            this.gbFilterMode.SuspendLayout();
            this.tlpFilterMode.SuspendLayout();
            this.gbPacketLength.SuspendLayout();
            this.tlpPacketLength.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPacketLength)).BeginInit();
            this.tlpModifyFunction.SuspendLayout();
            this.gbModifyLength.SuspendLayout();
            this.tlpSearchLength.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudModifyLength)).BeginInit();
            this.gbModifyCNT.SuspendLayout();
            this.tlpModifyCNT.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudModifyCNT)).BeginInit();
            this.tlpFilterInfo.SuspendLayout();
            this.tcFilterInfo.SuspendLayout();
            this.tpNormal.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilter_Normal)).BeginInit();
            this.tpAdvanced.SuspendLayout();
            this.tlpFilterAdvanced.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilter_Advanced_Modify)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilter_Advanced_Search)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoading)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpFilterInfo_Function
            // 
            resources.ApplyResources(this.tlpFilterInfo_Function, "tlpFilterInfo_Function");
            this.tlpFilterInfo_Function.Controls.Add(this.gbStartModify, 1, 1);
            this.tlpFilterInfo_Function.Controls.Add(this.tlpFilterButton, 0, 1);
            this.tlpFilterInfo_Function.Controls.Add(this.gbFilterName, 1, 0);
            this.tlpFilterInfo_Function.Controls.Add(this.gbFilterMode, 0, 0);
            this.tlpFilterInfo_Function.Controls.Add(this.gbPacketLength, 2, 0);
            this.tlpFilterInfo_Function.Controls.Add(this.tlpModifyFunction, 2, 1);
            this.tlpFilterInfo_Function.Name = "tlpFilterInfo_Function";
            // 
            // gbStartModify
            // 
            this.gbStartModify.Controls.Add(this.tlpFilterModifyFrom);
            resources.ApplyResources(this.gbStartModify, "gbStartModify");
            this.gbStartModify.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbStartModify.Name = "gbStartModify";
            this.gbStartModify.TabStop = false;
            // 
            // tlpFilterModifyFrom
            // 
            resources.ApplyResources(this.tlpFilterModifyFrom, "tlpFilterModifyFrom");
            this.tlpFilterModifyFrom.Controls.Add(this.rbFromPosition, 1, 1);
            this.tlpFilterModifyFrom.Controls.Add(this.rbFromHead, 1, 0);
            this.tlpFilterModifyFrom.Name = "tlpFilterModifyFrom";
            // 
            // rbFromPosition
            // 
            resources.ApplyResources(this.rbFromPosition, "rbFromPosition");
            this.rbFromPosition.Name = "rbFromPosition";
            this.rbFromPosition.UseVisualStyleBackColor = true;
            this.rbFromPosition.CheckedChanged += new System.EventHandler(this.rbStartFrom_CheckedChanged);
            // 
            // rbFromHead
            // 
            resources.ApplyResources(this.rbFromHead, "rbFromHead");
            this.rbFromHead.Checked = true;
            this.rbFromHead.Name = "rbFromHead";
            this.rbFromHead.TabStop = true;
            this.rbFromHead.UseVisualStyleBackColor = true;
            this.rbFromHead.CheckedChanged += new System.EventHandler(this.rbStartFrom_CheckedChanged);
            // 
            // tlpFilterButton
            // 
            resources.ApplyResources(this.tlpFilterButton, "tlpFilterButton");
            this.tlpFilterButton.Controls.Add(this.bSave, 3, 1);
            this.tlpFilterButton.Controls.Add(this.bReset, 1, 1);
            this.tlpFilterButton.Name = "tlpFilterButton";
            // 
            // bSave
            // 
            resources.ApplyResources(this.bSave, "bSave");
            this.bSave.Name = "bSave";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bReset
            // 
            resources.ApplyResources(this.bReset, "bReset");
            this.bReset.Name = "bReset";
            this.bReset.UseVisualStyleBackColor = true;
            this.bReset.Click += new System.EventHandler(this.bReset_Click);
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
            this.tlpFilterName.Controls.Add(this.txtFilterName, 1, 1);
            this.tlpFilterName.Name = "tlpFilterName";
            // 
            // txtFilterName
            // 
            resources.ApplyResources(this.txtFilterName, "txtFilterName");
            this.txtFilterName.Name = "txtFilterName";
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
            this.tlpFilterMode.Controls.Add(this.rbAdvanced, 0, 1);
            this.tlpFilterMode.Controls.Add(this.rbNormal, 0, 0);
            this.tlpFilterMode.Name = "tlpFilterMode";
            // 
            // rbAdvanced
            // 
            resources.ApplyResources(this.rbAdvanced, "rbAdvanced");
            this.rbAdvanced.Name = "rbAdvanced";
            this.rbAdvanced.UseVisualStyleBackColor = true;
            this.rbAdvanced.CheckedChanged += new System.EventHandler(this.rbFilterMode_CheckedChanged);
            // 
            // rbNormal
            // 
            resources.ApplyResources(this.rbNormal, "rbNormal");
            this.rbNormal.Checked = true;
            this.rbNormal.Name = "rbNormal";
            this.rbNormal.TabStop = true;
            this.rbNormal.UseVisualStyleBackColor = true;
            this.rbNormal.CheckedChanged += new System.EventHandler(this.rbFilterMode_CheckedChanged);
            // 
            // gbPacketLength
            // 
            this.gbPacketLength.Controls.Add(this.tlpPacketLength);
            resources.ApplyResources(this.gbPacketLength, "gbPacketLength");
            this.gbPacketLength.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbPacketLength.Name = "gbPacketLength";
            this.gbPacketLength.TabStop = false;
            // 
            // tlpPacketLength
            // 
            resources.ApplyResources(this.tlpPacketLength, "tlpPacketLength");
            this.tlpPacketLength.Controls.Add(this.bPacketLength, 5, 1);
            this.tlpPacketLength.Controls.Add(this.nudPacketLength, 2, 1);
            this.tlpPacketLength.Controls.Add(this.rbPacketLength_Custom, 1, 1);
            this.tlpPacketLength.Controls.Add(this.rbPacketLength_NoModify, 1, 0);
            this.tlpPacketLength.Name = "tlpPacketLength";
            // 
            // bPacketLength
            // 
            resources.ApplyResources(this.bPacketLength, "bPacketLength");
            this.bPacketLength.Name = "bPacketLength";
            this.bPacketLength.UseVisualStyleBackColor = true;
            this.bPacketLength.Click += new System.EventHandler(this.bPacketLength_Click);
            // 
            // nudPacketLength
            // 
            resources.ApplyResources(this.nudPacketLength, "nudPacketLength");
            this.nudPacketLength.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudPacketLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPacketLength.Name = "nudPacketLength";
            this.nudPacketLength.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // rbPacketLength_Custom
            // 
            resources.ApplyResources(this.rbPacketLength_Custom, "rbPacketLength_Custom");
            this.rbPacketLength_Custom.Name = "rbPacketLength_Custom";
            this.rbPacketLength_Custom.UseVisualStyleBackColor = true;
            this.rbPacketLength_Custom.CheckedChanged += new System.EventHandler(this.rbPacketLength_CheckedChanged);
            // 
            // rbPacketLength_NoModify
            // 
            resources.ApplyResources(this.rbPacketLength_NoModify, "rbPacketLength_NoModify");
            this.rbPacketLength_NoModify.Checked = true;
            this.rbPacketLength_NoModify.Name = "rbPacketLength_NoModify";
            this.rbPacketLength_NoModify.TabStop = true;
            this.rbPacketLength_NoModify.UseVisualStyleBackColor = true;
            this.rbPacketLength_NoModify.CheckedChanged += new System.EventHandler(this.rbPacketLength_CheckedChanged);
            // 
            // tlpModifyFunction
            // 
            resources.ApplyResources(this.tlpModifyFunction, "tlpModifyFunction");
            this.tlpModifyFunction.Controls.Add(this.gbModifyLength, 1, 0);
            this.tlpModifyFunction.Controls.Add(this.gbModifyCNT, 0, 0);
            this.tlpModifyFunction.Name = "tlpModifyFunction";
            // 
            // gbModifyLength
            // 
            this.gbModifyLength.Controls.Add(this.tlpSearchLength);
            resources.ApplyResources(this.gbModifyLength, "gbModifyLength");
            this.gbModifyLength.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.gbModifyLength.Name = "gbModifyLength";
            this.gbModifyLength.TabStop = false;
            // 
            // tlpSearchLength
            // 
            resources.ApplyResources(this.tlpSearchLength, "tlpSearchLength");
            this.tlpSearchLength.Controls.Add(this.bModifyLength, 1, 2);
            this.tlpSearchLength.Controls.Add(this.nudModifyLength, 1, 0);
            this.tlpSearchLength.Name = "tlpSearchLength";
            // 
            // bModifyLength
            // 
            resources.ApplyResources(this.bModifyLength, "bModifyLength");
            this.bModifyLength.Name = "bModifyLength";
            this.bModifyLength.UseVisualStyleBackColor = true;
            this.bModifyLength.Click += new System.EventHandler(this.bModifyLength_Click);
            // 
            // nudModifyLength
            // 
            this.nudModifyLength.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.nudModifyLength, "nudModifyLength");
            this.nudModifyLength.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudModifyLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudModifyLength.Name = "nudModifyLength";
            this.nudModifyLength.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // gbModifyCNT
            // 
            this.gbModifyCNT.Controls.Add(this.tlpModifyCNT);
            this.gbModifyCNT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            resources.ApplyResources(this.gbModifyCNT, "gbModifyCNT");
            this.gbModifyCNT.Name = "gbModifyCNT";
            this.gbModifyCNT.TabStop = false;
            // 
            // tlpModifyCNT
            // 
            resources.ApplyResources(this.tlpModifyCNT, "tlpModifyCNT");
            this.tlpModifyCNT.Controls.Add(this.nudModifyCNT, 1, 1);
            this.tlpModifyCNT.Name = "tlpModifyCNT";
            // 
            // nudModifyCNT
            // 
            this.nudModifyCNT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.nudModifyCNT, "nudModifyCNT");
            this.nudModifyCNT.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.nudModifyCNT.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudModifyCNT.Name = "nudModifyCNT";
            this.nudModifyCNT.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // bgwFilterInfo
            // 
            this.bgwFilterInfo.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwFilterInfo_DoWork);
            this.bgwFilterInfo.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwFilterInfo_RunWorkerCompleted);
            // 
            // tlpFilterInfo
            // 
            resources.ApplyResources(this.tlpFilterInfo, "tlpFilterInfo");
            this.tlpFilterInfo.Controls.Add(this.tcFilterInfo, 0, 0);
            this.tlpFilterInfo.Controls.Add(this.pbLoading, 0, 1);
            this.tlpFilterInfo.Name = "tlpFilterInfo";
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
            this.tpNormal.Controls.Add(this.dgvFilter_Normal);
            resources.ApplyResources(this.tpNormal, "tpNormal");
            this.tpNormal.Name = "tpNormal";
            this.tpNormal.UseVisualStyleBackColor = true;
            // 
            // dgvFilter_Normal
            // 
            this.dgvFilter_Normal.AllowUserToAddRows = false;
            this.dgvFilter_Normal.AllowUserToDeleteRows = false;
            this.dgvFilter_Normal.AllowUserToResizeColumns = false;
            this.dgvFilter_Normal.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilter_Normal.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvFilter_Normal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilter_Normal.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.dgvFilter_Normal, "dgvFilter_Normal");
            this.dgvFilter_Normal.MultiSelect = false;
            this.dgvFilter_Normal.Name = "dgvFilter_Normal";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilter_Normal.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvFilter_Normal.RowHeadersVisible = false;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgvFilter_Normal.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvFilter_Normal.RowTemplate.Height = 30;
            this.dgvFilter_Normal.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvFilter_ColumnAdded);
            this.dgvFilter_Normal.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvFilterList_KeyDown);
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
            this.tlpFilterAdvanced.Controls.Add(this.dgvFilter_Advanced_Modify, 0, 1);
            this.tlpFilterAdvanced.Controls.Add(this.dgvFilter_Advanced_Search, 0, 0);
            this.tlpFilterAdvanced.Name = "tlpFilterAdvanced";
            // 
            // dgvFilter_Advanced_Modify
            // 
            this.dgvFilter_Advanced_Modify.AllowUserToAddRows = false;
            this.dgvFilter_Advanced_Modify.AllowUserToDeleteRows = false;
            this.dgvFilter_Advanced_Modify.AllowUserToResizeColumns = false;
            this.dgvFilter_Advanced_Modify.AllowUserToResizeRows = false;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilter_Advanced_Modify.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvFilter_Advanced_Modify.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilter_Advanced_Modify.DefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.dgvFilter_Advanced_Modify, "dgvFilter_Advanced_Modify");
            this.dgvFilter_Advanced_Modify.MultiSelect = false;
            this.dgvFilter_Advanced_Modify.Name = "dgvFilter_Advanced_Modify";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilter_Advanced_Modify.RowHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvFilter_Advanced_Modify.RowHeadersVisible = false;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgvFilter_Advanced_Modify.RowsDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvFilter_Advanced_Modify.RowTemplate.Height = 30;
            this.dgvFilter_Advanced_Modify.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvFilter_ColumnAdded);
            this.dgvFilter_Advanced_Modify.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvFilterList_KeyDown);
            // 
            // dgvFilter_Advanced_Search
            // 
            this.dgvFilter_Advanced_Search.AllowUserToAddRows = false;
            this.dgvFilter_Advanced_Search.AllowUserToDeleteRows = false;
            this.dgvFilter_Advanced_Search.AllowUserToResizeColumns = false;
            this.dgvFilter_Advanced_Search.AllowUserToResizeRows = false;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilter_Advanced_Search.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvFilter_Advanced_Search.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilter_Advanced_Search.DefaultCellStyle = dataGridViewCellStyle10;
            resources.ApplyResources(this.dgvFilter_Advanced_Search, "dgvFilter_Advanced_Search");
            this.dgvFilter_Advanced_Search.MultiSelect = false;
            this.dgvFilter_Advanced_Search.Name = "dgvFilter_Advanced_Search";
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFilter_Advanced_Search.RowHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.dgvFilter_Advanced_Search.RowHeadersVisible = false;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dgvFilter_Advanced_Search.RowsDefaultCellStyle = dataGridViewCellStyle12;
            this.dgvFilter_Advanced_Search.RowTemplate.Height = 30;
            this.dgvFilter_Advanced_Search.ColumnAdded += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dgvFilter_ColumnAdded);
            this.dgvFilter_Advanced_Search.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvFilterList_KeyDown);
            // 
            // pbLoading
            // 
            resources.ApplyResources(this.pbLoading, "pbLoading");
            this.pbLoading.Image = global::WPELibrary.Properties.Resources.loading;
            this.pbLoading.Name = "pbLoading";
            this.pbLoading.TabStop = false;
            // 
            // Socket_FilterForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpFilterInfo);
            this.Controls.Add(this.tlpFilterInfo_Function);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Socket_FilterForm";
            this.tlpFilterInfo_Function.ResumeLayout(false);
            this.gbStartModify.ResumeLayout(false);
            this.tlpFilterModifyFrom.ResumeLayout(false);
            this.tlpFilterModifyFrom.PerformLayout();
            this.tlpFilterButton.ResumeLayout(false);
            this.gbFilterName.ResumeLayout(false);
            this.tlpFilterName.ResumeLayout(false);
            this.tlpFilterName.PerformLayout();
            this.gbFilterMode.ResumeLayout(false);
            this.tlpFilterMode.ResumeLayout(false);
            this.tlpFilterMode.PerformLayout();
            this.gbPacketLength.ResumeLayout(false);
            this.tlpPacketLength.ResumeLayout(false);
            this.tlpPacketLength.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPacketLength)).EndInit();
            this.tlpModifyFunction.ResumeLayout(false);
            this.gbModifyLength.ResumeLayout(false);
            this.tlpSearchLength.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudModifyLength)).EndInit();
            this.gbModifyCNT.ResumeLayout(false);
            this.tlpModifyCNT.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudModifyCNT)).EndInit();
            this.tlpFilterInfo.ResumeLayout(false);
            this.tcFilterInfo.ResumeLayout(false);
            this.tpNormal.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilter_Normal)).EndInit();
            this.tpAdvanced.ResumeLayout(false);
            this.tlpFilterAdvanced.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilter_Advanced_Modify)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilter_Advanced_Search)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbLoading)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpFilterInfo_Function;
        private System.Windows.Forms.GroupBox gbStartModify;
        private System.Windows.Forms.TableLayoutPanel tlpFilterButton;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Button bReset;
        private System.Windows.Forms.GroupBox gbFilterName;
        private System.Windows.Forms.GroupBox gbFilterMode;
        private System.Windows.Forms.TableLayoutPanel tlpFilterModifyFrom;
        private System.Windows.Forms.RadioButton rbFromPosition;
        private System.Windows.Forms.RadioButton rbFromHead;
        private System.Windows.Forms.TableLayoutPanel tlpFilterMode;
        private System.Windows.Forms.RadioButton rbAdvanced;
        private System.Windows.Forms.RadioButton rbNormal;
        private System.Windows.Forms.GroupBox gbPacketLength;
        private System.Windows.Forms.TableLayoutPanel tlpPacketLength;
        private System.Windows.Forms.RadioButton rbPacketLength_Custom;
        private System.Windows.Forms.RadioButton rbPacketLength_NoModify;
        private System.Windows.Forms.NumericUpDown nudPacketLength;
        private System.Windows.Forms.TableLayoutPanel tlpFilterName;
        private System.Windows.Forms.TextBox txtFilterName;
        private System.Windows.Forms.Button bPacketLength;
        private System.ComponentModel.BackgroundWorker bgwFilterInfo;
        private System.Windows.Forms.TableLayoutPanel tlpFilterInfo;
        private System.Windows.Forms.PictureBox pbLoading;
        private System.Windows.Forms.TabControl tcFilterInfo;
        private System.Windows.Forms.TabPage tpNormal;
        private System.Windows.Forms.DataGridView dgvFilter_Normal;
        private System.Windows.Forms.TabPage tpAdvanced;
        private System.Windows.Forms.TableLayoutPanel tlpFilterAdvanced;
        private System.Windows.Forms.DataGridView dgvFilter_Advanced_Modify;
        private System.Windows.Forms.DataGridView dgvFilter_Advanced_Search;
        private System.Windows.Forms.TableLayoutPanel tlpModifyFunction;
        private System.Windows.Forms.GroupBox gbModifyLength;
        private System.Windows.Forms.TableLayoutPanel tlpSearchLength;
        private System.Windows.Forms.Button bModifyLength;
        private System.Windows.Forms.NumericUpDown nudModifyLength;
        private System.Windows.Forms.GroupBox gbModifyCNT;
        private System.Windows.Forms.TableLayoutPanel tlpModifyCNT;
        private System.Windows.Forms.NumericUpDown nudModifyCNT;
    }
}