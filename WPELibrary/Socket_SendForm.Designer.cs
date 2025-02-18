
namespace WPELibrary
{
    partial class Socket_SendForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Socket_SendForm));
            this.tlpSendForm = new System.Windows.Forms.TableLayoutPanel();
            this.tlpPacketInfo = new System.Windows.Forms.TableLayoutPanel();
            this.txtIPTo = new System.Windows.Forms.TextBox();
            this.txtPacketType = new System.Windows.Forms.TextBox();
            this.txtIPFrom = new System.Windows.Forms.TextBox();
            this.lPacketType = new System.Windows.Forms.Label();
            this.lIPAddr = new System.Windows.Forms.Label();
            this.lPacketTime = new System.Windows.Forms.Label();
            this.txtPacketTime = new System.Windows.Forms.TextBox();
            this.pbSocketType = new System.Windows.Forms.PictureBox();
            this.tlpPacketData = new System.Windows.Forms.TableLayoutPanel();
            this.tsPacketData = new System.Windows.Forms.ToolStrip();
            this.tsPacketData_Cut = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.tsPacketData_Copy = new System.Windows.Forms.ToolStripSplitButton();
            this.tsPacketData_Copy_Copy = new System.Windows.Forms.ToolStripMenuItem();
            this.tsPacketData_Copy_CopyHex = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.tsPacketData_Paste = new System.Windows.Forms.ToolStripSplitButton();
            this.tsPacketData_Paste_Paste = new System.Windows.Forms.ToolStripMenuItem();
            this.tsPacketData_Paste_PasteHex = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.tsPacketData_Find = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator13 = new System.Windows.Forms.ToolStripSeparator();
            this.tsPacketData_FindNext = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator14 = new System.Windows.Forms.ToolStripSeparator();
            this.tscbEncoding = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator15 = new System.Windows.Forms.ToolStripSeparator();
            this.tscbPerLine = new System.Windows.Forms.ToolStripComboBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lChar_Value = new System.Windows.Forms.Label();
            this.lChar = new System.Windows.Forms.Label();
            this.lBits_Value = new System.Windows.Forms.Label();
            this.lBits = new System.Windows.Forms.Label();
            this.lDouble_Value = new System.Windows.Forms.Label();
            this.lDouble = new System.Windows.Forms.Label();
            this.lFloat_Value = new System.Windows.Forms.Label();
            this.lFloat = new System.Windows.Forms.Label();
            this.lUInt64_Value = new System.Windows.Forms.Label();
            this.lUInt64 = new System.Windows.Forms.Label();
            this.lInt64_Value = new System.Windows.Forms.Label();
            this.lInt64 = new System.Windows.Forms.Label();
            this.lUInt32_Value = new System.Windows.Forms.Label();
            this.lUInt32 = new System.Windows.Forms.Label();
            this.lInt32_Value = new System.Windows.Forms.Label();
            this.lInt32 = new System.Windows.Forms.Label();
            this.lUShort_Value = new System.Windows.Forms.Label();
            this.lUShort = new System.Windows.Forms.Label();
            this.lShort_Value = new System.Windows.Forms.Label();
            this.lShort = new System.Windows.Forms.Label();
            this.lByte_Value = new System.Windows.Forms.Label();
            this.lByte = new System.Windows.Forms.Label();
            this.hbPacketData = new Be.Windows.Forms.HexBox();
            this.cmsHexBox = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsHexBox_SendList = new System.Windows.Forms.ToolStripMenuItem();
            this.tscbSendList = new System.Windows.Forms.ToolStripComboBox();
            this.cmsHexBox_FilterList = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsHexBox_Split1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsHexBox_SelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.lHexBox_Position = new System.Windows.Forms.Label();
            this.ssSocketSend = new System.Windows.Forms.StatusStrip();
            this.tlSendTimes = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSendTimes_Value = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSplit = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSend_Success = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSend_Success_Value = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSend_Fail = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSend_Fail_Value = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlpParameter = new System.Windows.Forms.TableLayoutPanel();
            this.gbSendSocket = new System.Windows.Forms.GroupBox();
            this.tlpSendSocket = new System.Windows.Forms.TableLayoutPanel();
            this.nudSendSocket_Len = new System.Windows.Forms.NumericUpDown();
            this.lSendSocket_Len = new System.Windows.Forms.Label();
            this.lSendSocket_Socket = new System.Windows.Forms.Label();
            this.nudSendSocket_Socket = new System.Windows.Forms.NumericUpDown();
            this.gbSendType = new System.Windows.Forms.GroupBox();
            this.tlpSendType = new System.Windows.Forms.TableLayoutPanel();
            this.rbSendType_Times = new System.Windows.Forms.RadioButton();
            this.rbSendType_Continuously = new System.Windows.Forms.RadioButton();
            this.lSendType_Times = new System.Windows.Forms.Label();
            this.lSendType_Int = new System.Windows.Forms.Label();
            this.nudSendType_Interval = new System.Windows.Forms.NumericUpDown();
            this.nudSendType_Times = new System.Windows.Forms.NumericUpDown();
            this.gbSendStep = new System.Windows.Forms.GroupBox();
            this.tlpSendStepSet = new System.Windows.Forms.TableLayoutPanel();
            this.cbSendStep = new System.Windows.Forms.CheckBox();
            this.lSendStep_Position = new System.Windows.Forms.Label();
            this.lSendStep_Len_Value = new System.Windows.Forms.Label();
            this.nudSendStep_Len = new System.Windows.Forms.NumericUpDown();
            this.lSendStep_Len = new System.Windows.Forms.Label();
            this.lSendStep_Position_Value = new System.Windows.Forms.Label();
            this.nudSendStep_Position = new System.Windows.Forms.NumericUpDown();
            this.tlpButtons = new System.Windows.Forms.TableLayoutPanel();
            this.bSave = new System.Windows.Forms.Button();
            this.bClose = new System.Windows.Forms.Button();
            this.bSendStop = new System.Windows.Forms.Button();
            this.bSend = new System.Windows.Forms.Button();
            this.bgwSendPacket = new System.ComponentModel.BackgroundWorker();
            this.tlpSendForm.SuspendLayout();
            this.tlpPacketInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSocketType)).BeginInit();
            this.tlpPacketData.SuspendLayout();
            this.tsPacketData.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.cmsHexBox.SuspendLayout();
            this.ssSocketSend.SuspendLayout();
            this.tlpParameter.SuspendLayout();
            this.gbSendSocket.SuspendLayout();
            this.tlpSendSocket.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSendSocket_Len)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSendSocket_Socket)).BeginInit();
            this.gbSendType.SuspendLayout();
            this.tlpSendType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSendType_Interval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSendType_Times)).BeginInit();
            this.gbSendStep.SuspendLayout();
            this.tlpSendStepSet.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSendStep_Len)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSendStep_Position)).BeginInit();
            this.tlpButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpSendForm
            // 
            resources.ApplyResources(this.tlpSendForm, "tlpSendForm");
            this.tlpSendForm.Controls.Add(this.tlpPacketInfo, 0, 0);
            this.tlpSendForm.Controls.Add(this.tlpPacketData, 0, 1);
            this.tlpSendForm.Controls.Add(this.ssSocketSend, 0, 4);
            this.tlpSendForm.Controls.Add(this.tlpParameter, 0, 2);
            this.tlpSendForm.Controls.Add(this.tlpButtons, 0, 3);
            this.tlpSendForm.Name = "tlpSendForm";
            // 
            // tlpPacketInfo
            // 
            resources.ApplyResources(this.tlpPacketInfo, "tlpPacketInfo");
            this.tlpPacketInfo.Controls.Add(this.txtIPTo, 7, 0);
            this.tlpPacketInfo.Controls.Add(this.txtPacketType, 3, 0);
            this.tlpPacketInfo.Controls.Add(this.txtIPFrom, 5, 0);
            this.tlpPacketInfo.Controls.Add(this.lPacketType, 2, 0);
            this.tlpPacketInfo.Controls.Add(this.lIPAddr, 4, 0);
            this.tlpPacketInfo.Controls.Add(this.lPacketTime, 0, 0);
            this.tlpPacketInfo.Controls.Add(this.txtPacketTime, 1, 0);
            this.tlpPacketInfo.Controls.Add(this.pbSocketType, 6, 0);
            this.tlpPacketInfo.Name = "tlpPacketInfo";
            // 
            // txtIPTo
            // 
            resources.ApplyResources(this.txtIPTo, "txtIPTo");
            this.txtIPTo.Name = "txtIPTo";
            this.txtIPTo.ReadOnly = true;
            // 
            // txtPacketType
            // 
            resources.ApplyResources(this.txtPacketType, "txtPacketType");
            this.txtPacketType.Name = "txtPacketType";
            this.txtPacketType.ReadOnly = true;
            // 
            // txtIPFrom
            // 
            resources.ApplyResources(this.txtIPFrom, "txtIPFrom");
            this.txtIPFrom.Name = "txtIPFrom";
            this.txtIPFrom.ReadOnly = true;
            // 
            // lPacketType
            // 
            resources.ApplyResources(this.lPacketType, "lPacketType");
            this.lPacketType.Name = "lPacketType";
            // 
            // lIPAddr
            // 
            resources.ApplyResources(this.lIPAddr, "lIPAddr");
            this.lIPAddr.Name = "lIPAddr";
            // 
            // lPacketTime
            // 
            resources.ApplyResources(this.lPacketTime, "lPacketTime");
            this.lPacketTime.Name = "lPacketTime";
            // 
            // txtPacketTime
            // 
            resources.ApplyResources(this.txtPacketTime, "txtPacketTime");
            this.txtPacketTime.Name = "txtPacketTime";
            this.txtPacketTime.ReadOnly = true;
            // 
            // pbSocketType
            // 
            resources.ApplyResources(this.pbSocketType, "pbSocketType");
            this.pbSocketType.Image = global::WPELibrary.Properties.Resources.sent;
            this.pbSocketType.Name = "pbSocketType";
            this.pbSocketType.TabStop = false;
            // 
            // tlpPacketData
            // 
            resources.ApplyResources(this.tlpPacketData, "tlpPacketData");
            this.tlpPacketData.Controls.Add(this.tsPacketData, 0, 0);
            this.tlpPacketData.Controls.Add(this.tableLayoutPanel1, 1, 1);
            this.tlpPacketData.Controls.Add(this.hbPacketData, 0, 1);
            this.tlpPacketData.Controls.Add(this.lHexBox_Position, 1, 0);
            this.tlpPacketData.Name = "tlpPacketData";
            // 
            // tsPacketData
            // 
            this.tsPacketData.BackColor = System.Drawing.SystemColors.Control;
            this.tsPacketData.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.tsPacketData.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsPacketData_Cut,
            this.toolStripSeparator10,
            this.tsPacketData_Copy,
            this.toolStripSeparator11,
            this.tsPacketData_Paste,
            this.toolStripSeparator12,
            this.tsPacketData_Find,
            this.toolStripSeparator13,
            this.tsPacketData_FindNext,
            this.toolStripSeparator14,
            this.tscbEncoding,
            this.toolStripSeparator15,
            this.tscbPerLine});
            resources.ApplyResources(this.tsPacketData, "tsPacketData");
            this.tsPacketData.Name = "tsPacketData";
            // 
            // tsPacketData_Cut
            // 
            this.tsPacketData_Cut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsPacketData_Cut.Image = global::WPELibrary.Properties.Resources.cut;
            resources.ApplyResources(this.tsPacketData_Cut, "tsPacketData_Cut");
            this.tsPacketData_Cut.Name = "tsPacketData_Cut";
            this.tsPacketData_Cut.Click += new System.EventHandler(this.tsPacketData_Cut_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            resources.ApplyResources(this.toolStripSeparator10, "toolStripSeparator10");
            // 
            // tsPacketData_Copy
            // 
            this.tsPacketData_Copy.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsPacketData_Copy.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsPacketData_Copy_Copy,
            this.tsPacketData_Copy_CopyHex});
            this.tsPacketData_Copy.Image = global::WPELibrary.Properties.Resources.copy;
            resources.ApplyResources(this.tsPacketData_Copy, "tsPacketData_Copy");
            this.tsPacketData_Copy.Name = "tsPacketData_Copy";
            this.tsPacketData_Copy.ButtonClick += new System.EventHandler(this.tsPacketData_Copy_ButtonClick);
            // 
            // tsPacketData_Copy_Copy
            // 
            this.tsPacketData_Copy_Copy.Image = global::WPELibrary.Properties.Resources.copy;
            resources.ApplyResources(this.tsPacketData_Copy_Copy, "tsPacketData_Copy_Copy");
            this.tsPacketData_Copy_Copy.Name = "tsPacketData_Copy_Copy";
            this.tsPacketData_Copy_Copy.Click += new System.EventHandler(this.tsPacketData_Copy_Copy_Click);
            // 
            // tsPacketData_Copy_CopyHex
            // 
            this.tsPacketData_Copy_CopyHex.Image = global::WPELibrary.Properties.Resources.copy;
            resources.ApplyResources(this.tsPacketData_Copy_CopyHex, "tsPacketData_Copy_CopyHex");
            this.tsPacketData_Copy_CopyHex.Name = "tsPacketData_Copy_CopyHex";
            this.tsPacketData_Copy_CopyHex.Click += new System.EventHandler(this.tsPacketData_Copy_CopyHex_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            resources.ApplyResources(this.toolStripSeparator11, "toolStripSeparator11");
            // 
            // tsPacketData_Paste
            // 
            this.tsPacketData_Paste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsPacketData_Paste.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsPacketData_Paste_Paste,
            this.tsPacketData_Paste_PasteHex});
            this.tsPacketData_Paste.Image = global::WPELibrary.Properties.Resources.paste;
            resources.ApplyResources(this.tsPacketData_Paste, "tsPacketData_Paste");
            this.tsPacketData_Paste.Name = "tsPacketData_Paste";
            this.tsPacketData_Paste.ButtonClick += new System.EventHandler(this.tsPacketData_Paste_ButtonClick);
            // 
            // tsPacketData_Paste_Paste
            // 
            this.tsPacketData_Paste_Paste.Image = global::WPELibrary.Properties.Resources.paste;
            resources.ApplyResources(this.tsPacketData_Paste_Paste, "tsPacketData_Paste_Paste");
            this.tsPacketData_Paste_Paste.Name = "tsPacketData_Paste_Paste";
            this.tsPacketData_Paste_Paste.Click += new System.EventHandler(this.tsPacketData_Paste_Paste_Click);
            // 
            // tsPacketData_Paste_PasteHex
            // 
            this.tsPacketData_Paste_PasteHex.Image = global::WPELibrary.Properties.Resources.paste;
            resources.ApplyResources(this.tsPacketData_Paste_PasteHex, "tsPacketData_Paste_PasteHex");
            this.tsPacketData_Paste_PasteHex.Name = "tsPacketData_Paste_PasteHex";
            this.tsPacketData_Paste_PasteHex.Click += new System.EventHandler(this.tsPacketData_Paste_PasteHex_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            resources.ApplyResources(this.toolStripSeparator12, "toolStripSeparator12");
            // 
            // tsPacketData_Find
            // 
            this.tsPacketData_Find.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsPacketData_Find.Image = global::WPELibrary.Properties.Resources.FindHS;
            resources.ApplyResources(this.tsPacketData_Find, "tsPacketData_Find");
            this.tsPacketData_Find.Name = "tsPacketData_Find";
            this.tsPacketData_Find.Click += new System.EventHandler(this.tsPacketData_Find_Click);
            // 
            // toolStripSeparator13
            // 
            this.toolStripSeparator13.Name = "toolStripSeparator13";
            resources.ApplyResources(this.toolStripSeparator13, "toolStripSeparator13");
            // 
            // tsPacketData_FindNext
            // 
            this.tsPacketData_FindNext.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsPacketData_FindNext.Image = global::WPELibrary.Properties.Resources.FindNextHS;
            resources.ApplyResources(this.tsPacketData_FindNext, "tsPacketData_FindNext");
            this.tsPacketData_FindNext.Name = "tsPacketData_FindNext";
            this.tsPacketData_FindNext.Click += new System.EventHandler(this.tsPacketData_FindNext_Click);
            // 
            // toolStripSeparator14
            // 
            this.toolStripSeparator14.Name = "toolStripSeparator14";
            resources.ApplyResources(this.toolStripSeparator14, "toolStripSeparator14");
            // 
            // tscbEncoding
            // 
            this.tscbEncoding.BackColor = System.Drawing.SystemColors.Control;
            this.tscbEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.tscbEncoding, "tscbEncoding");
            this.tscbEncoding.Name = "tscbEncoding";
            this.tscbEncoding.SelectedIndexChanged += new System.EventHandler(this.tscbEncoding_SelectedIndexChanged);
            // 
            // toolStripSeparator15
            // 
            this.toolStripSeparator15.Name = "toolStripSeparator15";
            resources.ApplyResources(this.toolStripSeparator15, "toolStripSeparator15");
            // 
            // tscbPerLine
            // 
            this.tscbPerLine.BackColor = System.Drawing.SystemColors.Control;
            this.tscbPerLine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.tscbPerLine, "tscbPerLine");
            this.tscbPerLine.Items.AddRange(new object[] {
            resources.GetString("tscbPerLine.Items"),
            resources.GetString("tscbPerLine.Items1")});
            this.tscbPerLine.Name = "tscbPerLine";
            this.tscbPerLine.SelectedIndexChanged += new System.EventHandler(this.tscbPerLine_SelectedIndexChanged);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.lChar_Value, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lChar, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lBits_Value, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lBits, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lDouble_Value, 1, 10);
            this.tableLayoutPanel1.Controls.Add(this.lDouble, 0, 10);
            this.tableLayoutPanel1.Controls.Add(this.lFloat_Value, 1, 9);
            this.tableLayoutPanel1.Controls.Add(this.lFloat, 0, 9);
            this.tableLayoutPanel1.Controls.Add(this.lUInt64_Value, 1, 8);
            this.tableLayoutPanel1.Controls.Add(this.lUInt64, 0, 8);
            this.tableLayoutPanel1.Controls.Add(this.lInt64_Value, 1, 7);
            this.tableLayoutPanel1.Controls.Add(this.lInt64, 0, 7);
            this.tableLayoutPanel1.Controls.Add(this.lUInt32_Value, 1, 6);
            this.tableLayoutPanel1.Controls.Add(this.lUInt32, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.lInt32_Value, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.lInt32, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lUShort_Value, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lUShort, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lShort_Value, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.lShort, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lByte_Value, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lByte, 0, 2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // lChar_Value
            // 
            resources.ApplyResources(this.lChar_Value, "lChar_Value");
            this.lChar_Value.Name = "lChar_Value";
            // 
            // lChar
            // 
            resources.ApplyResources(this.lChar, "lChar");
            this.lChar.Name = "lChar";
            // 
            // lBits_Value
            // 
            resources.ApplyResources(this.lBits_Value, "lBits_Value");
            this.lBits_Value.Name = "lBits_Value";
            // 
            // lBits
            // 
            resources.ApplyResources(this.lBits, "lBits");
            this.lBits.Name = "lBits";
            // 
            // lDouble_Value
            // 
            resources.ApplyResources(this.lDouble_Value, "lDouble_Value");
            this.lDouble_Value.Name = "lDouble_Value";
            // 
            // lDouble
            // 
            resources.ApplyResources(this.lDouble, "lDouble");
            this.lDouble.Name = "lDouble";
            // 
            // lFloat_Value
            // 
            resources.ApplyResources(this.lFloat_Value, "lFloat_Value");
            this.lFloat_Value.Name = "lFloat_Value";
            // 
            // lFloat
            // 
            resources.ApplyResources(this.lFloat, "lFloat");
            this.lFloat.Name = "lFloat";
            // 
            // lUInt64_Value
            // 
            resources.ApplyResources(this.lUInt64_Value, "lUInt64_Value");
            this.lUInt64_Value.Name = "lUInt64_Value";
            // 
            // lUInt64
            // 
            resources.ApplyResources(this.lUInt64, "lUInt64");
            this.lUInt64.Name = "lUInt64";
            // 
            // lInt64_Value
            // 
            resources.ApplyResources(this.lInt64_Value, "lInt64_Value");
            this.lInt64_Value.Name = "lInt64_Value";
            // 
            // lInt64
            // 
            resources.ApplyResources(this.lInt64, "lInt64");
            this.lInt64.Name = "lInt64";
            // 
            // lUInt32_Value
            // 
            resources.ApplyResources(this.lUInt32_Value, "lUInt32_Value");
            this.lUInt32_Value.Name = "lUInt32_Value";
            // 
            // lUInt32
            // 
            resources.ApplyResources(this.lUInt32, "lUInt32");
            this.lUInt32.Name = "lUInt32";
            // 
            // lInt32_Value
            // 
            resources.ApplyResources(this.lInt32_Value, "lInt32_Value");
            this.lInt32_Value.Name = "lInt32_Value";
            // 
            // lInt32
            // 
            resources.ApplyResources(this.lInt32, "lInt32");
            this.lInt32.Name = "lInt32";
            // 
            // lUShort_Value
            // 
            resources.ApplyResources(this.lUShort_Value, "lUShort_Value");
            this.lUShort_Value.Name = "lUShort_Value";
            // 
            // lUShort
            // 
            resources.ApplyResources(this.lUShort, "lUShort");
            this.lUShort.Name = "lUShort";
            // 
            // lShort_Value
            // 
            resources.ApplyResources(this.lShort_Value, "lShort_Value");
            this.lShort_Value.Name = "lShort_Value";
            // 
            // lShort
            // 
            resources.ApplyResources(this.lShort, "lShort");
            this.lShort.Name = "lShort";
            // 
            // lByte_Value
            // 
            resources.ApplyResources(this.lByte_Value, "lByte_Value");
            this.lByte_Value.Name = "lByte_Value";
            // 
            // lByte
            // 
            resources.ApplyResources(this.lByte, "lByte");
            this.lByte.Name = "lByte";
            // 
            // hbPacketData
            // 
            this.hbPacketData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            // 
            // 
            // 
            this.hbPacketData.BuiltInContextMenu.CopyMenuItemImage = global::WPELibrary.Properties.Resources.CopyHS;
            this.hbPacketData.BuiltInContextMenu.CopyMenuItemText = resources.GetString("hbPacketData.BuiltInContextMenu.CopyMenuItemText");
            this.hbPacketData.BuiltInContextMenu.CutMenuItemImage = global::WPELibrary.Properties.Resources.CutHS;
            this.hbPacketData.BuiltInContextMenu.CutMenuItemText = resources.GetString("hbPacketData.BuiltInContextMenu.CutMenuItemText");
            this.hbPacketData.BuiltInContextMenu.PasteMenuItemImage = global::WPELibrary.Properties.Resources.PasteHS;
            this.hbPacketData.BuiltInContextMenu.PasteMenuItemText = resources.GetString("hbPacketData.BuiltInContextMenu.PasteMenuItemText");
            this.hbPacketData.BuiltInContextMenu.SelectAllMenuItemText = resources.GetString("hbPacketData.BuiltInContextMenu.SelectAllMenuItemText");
            this.hbPacketData.ColumnInfoVisible = true;
            this.hbPacketData.ContextMenuStrip = this.cmsHexBox;
            resources.ApplyResources(this.hbPacketData, "hbPacketData");
            this.hbPacketData.LineInfoVisible = true;
            this.hbPacketData.Name = "hbPacketData";
            this.hbPacketData.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hbPacketData.StringViewVisible = true;
            this.hbPacketData.UseFixedBytesPerLine = true;
            this.hbPacketData.VScrollBarVisible = true;
            this.hbPacketData.SelectionStartChanged += new System.EventHandler(this.hbPacketData_SelectionStartChanged);
            this.hbPacketData.SelectionLengthChanged += new System.EventHandler(this.hbPacketData_SelectionLengthChanged);
            this.hbPacketData.CurrentLineChanged += new System.EventHandler(this.hbPacketData_CurrentLineChanged);
            this.hbPacketData.CurrentPositionInLineChanged += new System.EventHandler(this.hbPacketData_CurrentPositionInLineChanged);
            this.hbPacketData.Copied += new System.EventHandler(this.hbPacketData_Copied);
            this.hbPacketData.CopiedHex += new System.EventHandler(this.hbPacketData_CopiedHex);
            // 
            // cmsHexBox
            // 
            this.cmsHexBox.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.cmsHexBox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsHexBox_SendList,
            this.cmsHexBox_FilterList,
            this.cmsHexBox_Split1,
            this.cmsHexBox_SelectAll});
            this.cmsHexBox.Name = "cmsSocketSend";
            resources.ApplyResources(this.cmsHexBox, "cmsHexBox");
            this.cmsHexBox.Opening += new System.ComponentModel.CancelEventHandler(this.cmsHexBox_Opening);
            this.cmsHexBox.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsHexBox_ItemClicked);
            // 
            // cmsHexBox_SendList
            // 
            this.cmsHexBox_SendList.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tscbSendList});
            this.cmsHexBox_SendList.Image = global::WPELibrary.Properties.Resources.addto;
            resources.ApplyResources(this.cmsHexBox_SendList, "cmsHexBox_SendList");
            this.cmsHexBox_SendList.Name = "cmsHexBox_SendList";
            // 
            // tscbSendList
            // 
            this.tscbSendList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tscbSendList.Name = "tscbSendList";
            resources.ApplyResources(this.tscbSendList, "tscbSendList");
            this.tscbSendList.SelectedIndexChanged += new System.EventHandler(this.tscbSendList_SelectedIndexChanged);
            // 
            // cmsHexBox_FilterList
            // 
            this.cmsHexBox_FilterList.Image = global::WPELibrary.Properties.Resources.addto;
            resources.ApplyResources(this.cmsHexBox_FilterList, "cmsHexBox_FilterList");
            this.cmsHexBox_FilterList.Name = "cmsHexBox_FilterList";
            // 
            // cmsHexBox_Split1
            // 
            this.cmsHexBox_Split1.Name = "cmsHexBox_Split1";
            resources.ApplyResources(this.cmsHexBox_Split1, "cmsHexBox_Split1");
            // 
            // cmsHexBox_SelectAll
            // 
            this.cmsHexBox_SelectAll.Image = global::WPELibrary.Properties.Resources.SelectAll;
            resources.ApplyResources(this.cmsHexBox_SelectAll, "cmsHexBox_SelectAll");
            this.cmsHexBox_SelectAll.Name = "cmsHexBox_SelectAll";
            // 
            // lHexBox_Position
            // 
            resources.ApplyResources(this.lHexBox_Position, "lHexBox_Position");
            this.lHexBox_Position.Name = "lHexBox_Position";
            // 
            // ssSocketSend
            // 
            resources.ApplyResources(this.ssSocketSend, "ssSocketSend");
            this.ssSocketSend.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.ssSocketSend.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlSendTimes,
            this.tlSendTimes_Value,
            this.tlSplit,
            this.tlSend_Success,
            this.tlSend_Success_Value,
            this.toolStripStatusLabel3,
            this.tlSend_Fail,
            this.tlSend_Fail_Value});
            this.ssSocketSend.Name = "ssSocketSend";
            // 
            // tlSendTimes
            // 
            this.tlSendTimes.Name = "tlSendTimes";
            resources.ApplyResources(this.tlSendTimes, "tlSendTimes");
            // 
            // tlSendTimes_Value
            // 
            resources.ApplyResources(this.tlSendTimes_Value, "tlSendTimes_Value");
            this.tlSendTimes_Value.Name = "tlSendTimes_Value";
            // 
            // tlSplit
            // 
            this.tlSplit.ForeColor = System.Drawing.Color.DarkGray;
            this.tlSplit.Name = "tlSplit";
            resources.ApplyResources(this.tlSplit, "tlSplit");
            // 
            // tlSend_Success
            // 
            this.tlSend_Success.Name = "tlSend_Success";
            resources.ApplyResources(this.tlSend_Success, "tlSend_Success");
            // 
            // tlSend_Success_Value
            // 
            resources.ApplyResources(this.tlSend_Success_Value, "tlSend_Success_Value");
            this.tlSend_Success_Value.ForeColor = System.Drawing.Color.Green;
            this.tlSend_Success_Value.Name = "tlSend_Success_Value";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            resources.ApplyResources(this.toolStripStatusLabel3, "toolStripStatusLabel3");
            // 
            // tlSend_Fail
            // 
            this.tlSend_Fail.Name = "tlSend_Fail";
            resources.ApplyResources(this.tlSend_Fail, "tlSend_Fail");
            // 
            // tlSend_Fail_Value
            // 
            resources.ApplyResources(this.tlSend_Fail_Value, "tlSend_Fail_Value");
            this.tlSend_Fail_Value.ForeColor = System.Drawing.Color.Red;
            this.tlSend_Fail_Value.Name = "tlSend_Fail_Value";
            // 
            // tlpParameter
            // 
            resources.ApplyResources(this.tlpParameter, "tlpParameter");
            this.tlpParameter.Controls.Add(this.gbSendSocket, 0, 0);
            this.tlpParameter.Controls.Add(this.gbSendType, 1, 0);
            this.tlpParameter.Controls.Add(this.gbSendStep, 2, 0);
            this.tlpParameter.Name = "tlpParameter";
            // 
            // gbSendSocket
            // 
            this.gbSendSocket.Controls.Add(this.tlpSendSocket);
            resources.ApplyResources(this.gbSendSocket, "gbSendSocket");
            this.gbSendSocket.Name = "gbSendSocket";
            this.gbSendSocket.TabStop = false;
            // 
            // tlpSendSocket
            // 
            resources.ApplyResources(this.tlpSendSocket, "tlpSendSocket");
            this.tlpSendSocket.Controls.Add(this.nudSendSocket_Len, 1, 1);
            this.tlpSendSocket.Controls.Add(this.lSendSocket_Len, 0, 1);
            this.tlpSendSocket.Controls.Add(this.lSendSocket_Socket, 0, 0);
            this.tlpSendSocket.Controls.Add(this.nudSendSocket_Socket, 1, 0);
            this.tlpSendSocket.Name = "tlpSendSocket";
            // 
            // nudSendSocket_Len
            // 
            resources.ApplyResources(this.nudSendSocket_Len, "nudSendSocket_Len");
            this.nudSendSocket_Len.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudSendSocket_Len.Name = "nudSendSocket_Len";
            this.nudSendSocket_Len.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lSendSocket_Len
            // 
            resources.ApplyResources(this.lSendSocket_Len, "lSendSocket_Len");
            this.lSendSocket_Len.Name = "lSendSocket_Len";
            // 
            // lSendSocket_Socket
            // 
            resources.ApplyResources(this.lSendSocket_Socket, "lSendSocket_Socket");
            this.lSendSocket_Socket.Name = "lSendSocket_Socket";
            // 
            // nudSendSocket_Socket
            // 
            resources.ApplyResources(this.nudSendSocket_Socket, "nudSendSocket_Socket");
            this.nudSendSocket_Socket.Maximum = new decimal(new int[] {
            9999999,
            0,
            0,
            0});
            this.nudSendSocket_Socket.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSendSocket_Socket.Name = "nudSendSocket_Socket";
            this.nudSendSocket_Socket.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // gbSendType
            // 
            this.gbSendType.Controls.Add(this.tlpSendType);
            resources.ApplyResources(this.gbSendType, "gbSendType");
            this.gbSendType.Name = "gbSendType";
            this.gbSendType.TabStop = false;
            // 
            // tlpSendType
            // 
            resources.ApplyResources(this.tlpSendType, "tlpSendType");
            this.tlpSendType.Controls.Add(this.rbSendType_Times, 0, 0);
            this.tlpSendType.Controls.Add(this.rbSendType_Continuously, 0, 1);
            this.tlpSendType.Controls.Add(this.lSendType_Times, 2, 0);
            this.tlpSendType.Controls.Add(this.lSendType_Int, 2, 1);
            this.tlpSendType.Controls.Add(this.nudSendType_Interval, 1, 1);
            this.tlpSendType.Controls.Add(this.nudSendType_Times, 1, 0);
            this.tlpSendType.Name = "tlpSendType";
            // 
            // rbSendType_Times
            // 
            resources.ApplyResources(this.rbSendType_Times, "rbSendType_Times");
            this.rbSendType_Times.Checked = true;
            this.rbSendType_Times.Name = "rbSendType_Times";
            this.rbSendType_Times.TabStop = true;
            this.rbSendType_Times.UseVisualStyleBackColor = true;
            this.rbSendType_Times.CheckedChanged += new System.EventHandler(this.rbSendType_Times_CheckedChanged);
            // 
            // rbSendType_Continuously
            // 
            resources.ApplyResources(this.rbSendType_Continuously, "rbSendType_Continuously");
            this.rbSendType_Continuously.Name = "rbSendType_Continuously";
            this.rbSendType_Continuously.UseVisualStyleBackColor = true;
            this.rbSendType_Continuously.CheckedChanged += new System.EventHandler(this.rbSendType_Continuously_CheckedChanged);
            // 
            // lSendType_Times
            // 
            resources.ApplyResources(this.lSendType_Times, "lSendType_Times");
            this.lSendType_Times.Name = "lSendType_Times";
            // 
            // lSendType_Int
            // 
            resources.ApplyResources(this.lSendType_Int, "lSendType_Int");
            this.lSendType_Int.Name = "lSendType_Int";
            // 
            // nudSendType_Interval
            // 
            resources.ApplyResources(this.nudSendType_Interval, "nudSendType_Interval");
            this.nudSendType_Interval.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudSendType_Interval.Maximum = new decimal(new int[] {
            999999999,
            0,
            0,
            0});
            this.nudSendType_Interval.Name = "nudSendType_Interval";
            this.nudSendType_Interval.Value = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            // 
            // nudSendType_Times
            // 
            resources.ApplyResources(this.nudSendType_Times, "nudSendType_Times");
            this.nudSendType_Times.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudSendType_Times.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSendType_Times.Name = "nudSendType_Times";
            this.nudSendType_Times.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // gbSendStep
            // 
            this.gbSendStep.Controls.Add(this.tlpSendStepSet);
            resources.ApplyResources(this.gbSendStep, "gbSendStep");
            this.gbSendStep.Name = "gbSendStep";
            this.gbSendStep.TabStop = false;
            // 
            // tlpSendStepSet
            // 
            resources.ApplyResources(this.tlpSendStepSet, "tlpSendStepSet");
            this.tlpSendStepSet.Controls.Add(this.cbSendStep, 0, 0);
            this.tlpSendStepSet.Controls.Add(this.lSendStep_Position, 1, 0);
            this.tlpSendStepSet.Controls.Add(this.lSendStep_Len_Value, 3, 1);
            this.tlpSendStepSet.Controls.Add(this.nudSendStep_Len, 2, 1);
            this.tlpSendStepSet.Controls.Add(this.lSendStep_Len, 1, 1);
            this.tlpSendStepSet.Controls.Add(this.lSendStep_Position_Value, 3, 0);
            this.tlpSendStepSet.Controls.Add(this.nudSendStep_Position, 2, 0);
            this.tlpSendStepSet.Name = "tlpSendStepSet";
            // 
            // cbSendStep
            // 
            resources.ApplyResources(this.cbSendStep, "cbSendStep");
            this.cbSendStep.Name = "cbSendStep";
            this.cbSendStep.UseVisualStyleBackColor = true;
            this.cbSendStep.CheckedChanged += new System.EventHandler(this.cbSendStep_CheckedChanged);
            // 
            // lSendStep_Position
            // 
            resources.ApplyResources(this.lSendStep_Position, "lSendStep_Position");
            this.lSendStep_Position.Name = "lSendStep_Position";
            // 
            // lSendStep_Len_Value
            // 
            resources.ApplyResources(this.lSendStep_Len_Value, "lSendStep_Len_Value");
            this.lSendStep_Len_Value.Name = "lSendStep_Len_Value";
            // 
            // nudSendStep_Len
            // 
            resources.ApplyResources(this.nudSendStep_Len, "nudSendStep_Len");
            this.nudSendStep_Len.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudSendStep_Len.Minimum = new decimal(new int[] {
            255,
            0,
            0,
            -2147483648});
            this.nudSendStep_Len.Name = "nudSendStep_Len";
            this.nudSendStep_Len.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSendStep_Len.ValueChanged += new System.EventHandler(this.nudStepLen_ValueChanged);
            // 
            // lSendStep_Len
            // 
            resources.ApplyResources(this.lSendStep_Len, "lSendStep_Len");
            this.lSendStep_Len.Name = "lSendStep_Len";
            // 
            // lSendStep_Position_Value
            // 
            resources.ApplyResources(this.lSendStep_Position_Value, "lSendStep_Position_Value");
            this.lSendStep_Position_Value.Name = "lSendStep_Position_Value";
            // 
            // nudSendStep_Position
            // 
            resources.ApplyResources(this.nudSendStep_Position, "nudSendStep_Position");
            this.nudSendStep_Position.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudSendStep_Position.Name = "nudSendStep_Position";
            this.nudSendStep_Position.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSendStep_Position.ValueChanged += new System.EventHandler(this.nudStepIndex_ValueChanged);
            // 
            // tlpButtons
            // 
            resources.ApplyResources(this.tlpButtons, "tlpButtons");
            this.tlpButtons.Controls.Add(this.bSave, 5, 0);
            this.tlpButtons.Controls.Add(this.bClose, 7, 0);
            this.tlpButtons.Controls.Add(this.bSendStop, 3, 0);
            this.tlpButtons.Controls.Add(this.bSend, 1, 0);
            this.tlpButtons.Name = "tlpButtons";
            // 
            // bSave
            // 
            resources.ApplyResources(this.bSave, "bSave");
            this.bSave.Name = "bSave";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bClose
            // 
            resources.ApplyResources(this.bClose, "bClose");
            this.bClose.Name = "bClose";
            this.bClose.UseVisualStyleBackColor = true;
            this.bClose.Click += new System.EventHandler(this.bClose_Click);
            // 
            // bSendStop
            // 
            resources.ApplyResources(this.bSendStop, "bSendStop");
            this.bSendStop.Name = "bSendStop";
            this.bSendStop.UseVisualStyleBackColor = true;
            this.bSendStop.Click += new System.EventHandler(this.bSendStop_Click);
            // 
            // bSend
            // 
            resources.ApplyResources(this.bSend, "bSend");
            this.bSend.Name = "bSend";
            this.bSend.UseVisualStyleBackColor = true;
            this.bSend.Click += new System.EventHandler(this.bSend_Click);
            // 
            // bgwSendPacket
            // 
            this.bgwSendPacket.WorkerSupportsCancellation = true;
            this.bgwSendPacket.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwSendPacket_DoWork);
            this.bgwSendPacket.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwSendPacket_RunWorkerCompleted);
            // 
            // Socket_SendForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpSendForm);
            this.DoubleBuffered = true;
            this.Name = "Socket_SendForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Socket_SendForm_FormClosing);
            this.Load += new System.EventHandler(this.Socket_SendForm_Load);
            this.tlpSendForm.ResumeLayout(false);
            this.tlpSendForm.PerformLayout();
            this.tlpPacketInfo.ResumeLayout(false);
            this.tlpPacketInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbSocketType)).EndInit();
            this.tlpPacketData.ResumeLayout(false);
            this.tlpPacketData.PerformLayout();
            this.tsPacketData.ResumeLayout(false);
            this.tsPacketData.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.cmsHexBox.ResumeLayout(false);
            this.ssSocketSend.ResumeLayout(false);
            this.ssSocketSend.PerformLayout();
            this.tlpParameter.ResumeLayout(false);
            this.gbSendSocket.ResumeLayout(false);
            this.tlpSendSocket.ResumeLayout(false);
            this.tlpSendSocket.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSendSocket_Len)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSendSocket_Socket)).EndInit();
            this.gbSendType.ResumeLayout(false);
            this.tlpSendType.ResumeLayout(false);
            this.tlpSendType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSendType_Interval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSendType_Times)).EndInit();
            this.gbSendStep.ResumeLayout(false);
            this.tlpSendStepSet.ResumeLayout(false);
            this.tlpSendStepSet.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSendStep_Len)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSendStep_Position)).EndInit();
            this.tlpButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpSendForm;
        private System.Windows.Forms.StatusStrip ssSocketSend;
        private System.Windows.Forms.ToolStripStatusLabel tlSendTimes;
        private System.Windows.Forms.ToolStripStatusLabel tlSendTimes_Value;
        private System.Windows.Forms.ToolStripStatusLabel tlSplit;
        private System.Windows.Forms.ToolStripStatusLabel tlSend_Success;
        private System.Windows.Forms.ToolStripStatusLabel tlSend_Success_Value;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel tlSend_Fail;
        private System.Windows.Forms.ToolStripStatusLabel tlSend_Fail_Value;
        private System.Windows.Forms.TableLayoutPanel tlpParameter;
        private System.Windows.Forms.GroupBox gbSendType;
        private System.Windows.Forms.GroupBox gbSendStep;
        private System.Windows.Forms.ContextMenuStrip cmsHexBox;
        private System.Windows.Forms.ToolStripSeparator cmsHexBox_Split1;
        private System.Windows.Forms.ToolStripMenuItem cmsHexBox_FilterList;
        private System.ComponentModel.BackgroundWorker bgwSendPacket;
        private System.Windows.Forms.TableLayoutPanel tlpPacketData;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lBits_Value;
        private System.Windows.Forms.Label lBits;
        private System.Windows.Forms.Label lDouble_Value;
        private System.Windows.Forms.Label lDouble;
        private System.Windows.Forms.Label lFloat_Value;
        private System.Windows.Forms.Label lFloat;
        private System.Windows.Forms.Label lUInt64_Value;
        private System.Windows.Forms.Label lUInt64;
        private System.Windows.Forms.Label lInt64_Value;
        private System.Windows.Forms.Label lInt64;
        private System.Windows.Forms.Label lUInt32_Value;
        private System.Windows.Forms.Label lUInt32;
        private System.Windows.Forms.Label lInt32_Value;
        private System.Windows.Forms.Label lInt32;
        private System.Windows.Forms.Label lUShort_Value;
        private System.Windows.Forms.Label lUShort;
        private System.Windows.Forms.Label lShort_Value;
        private System.Windows.Forms.Label lShort;
        private System.Windows.Forms.Label lByte_Value;
        private System.Windows.Forms.Label lByte;
        private Be.Windows.Forms.HexBox hbPacketData;
        private System.Windows.Forms.Label lHexBox_Position;
        private System.Windows.Forms.ToolStrip tsPacketData;
        private System.Windows.Forms.ToolStripButton tsPacketData_Cut;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripSplitButton tsPacketData_Copy;
        private System.Windows.Forms.ToolStripMenuItem tsPacketData_Copy_Copy;
        private System.Windows.Forms.ToolStripMenuItem tsPacketData_Copy_CopyHex;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripSplitButton tsPacketData_Paste;
        private System.Windows.Forms.ToolStripMenuItem tsPacketData_Paste_Paste;
        private System.Windows.Forms.ToolStripMenuItem tsPacketData_Paste_PasteHex;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripButton tsPacketData_Find;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator13;
        private System.Windows.Forms.ToolStripButton tsPacketData_FindNext;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator14;
        private System.Windows.Forms.ToolStripComboBox tscbEncoding;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator15;
        private System.Windows.Forms.ToolStripComboBox tscbPerLine;
        private System.Windows.Forms.Label lChar_Value;
        private System.Windows.Forms.Label lChar;
        private System.Windows.Forms.TableLayoutPanel tlpButtons;
        private System.Windows.Forms.Button bSend;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Button bClose;
        private System.Windows.Forms.Button bSendStop;
        private System.Windows.Forms.TableLayoutPanel tlpPacketInfo;
        private System.Windows.Forms.TextBox txtIPTo;
        private System.Windows.Forms.TextBox txtPacketType;
        private System.Windows.Forms.TextBox txtIPFrom;
        private System.Windows.Forms.Label lPacketType;
        private System.Windows.Forms.Label lIPAddr;
        private System.Windows.Forms.Label lPacketTime;
        private System.Windows.Forms.TextBox txtPacketTime;
        private System.Windows.Forms.PictureBox pbSocketType;
        private System.Windows.Forms.GroupBox gbSendSocket;
        private System.Windows.Forms.TableLayoutPanel tlpSendSocket;
        private System.Windows.Forms.NumericUpDown nudSendSocket_Len;
        private System.Windows.Forms.Label lSendSocket_Len;
        private System.Windows.Forms.Label lSendSocket_Socket;
        private System.Windows.Forms.NumericUpDown nudSendSocket_Socket;
        private System.Windows.Forms.ToolStripMenuItem cmsHexBox_SelectAll;
        private System.Windows.Forms.TableLayoutPanel tlpSendType;
        private System.Windows.Forms.RadioButton rbSendType_Continuously;
        private System.Windows.Forms.Label lSendType_Times;
        private System.Windows.Forms.Label lSendType_Int;
        private System.Windows.Forms.NumericUpDown nudSendType_Interval;
        private System.Windows.Forms.NumericUpDown nudSendType_Times;
        private System.Windows.Forms.TableLayoutPanel tlpSendStepSet;
        private System.Windows.Forms.CheckBox cbSendStep;
        private System.Windows.Forms.Label lSendStep_Position;
        private System.Windows.Forms.Label lSendStep_Len_Value;
        private System.Windows.Forms.NumericUpDown nudSendStep_Len;
        private System.Windows.Forms.Label lSendStep_Len;
        private System.Windows.Forms.Label lSendStep_Position_Value;
        private System.Windows.Forms.NumericUpDown nudSendStep_Position;
        private System.Windows.Forms.RadioButton rbSendType_Times;
        private System.Windows.Forms.ToolStripMenuItem cmsHexBox_SendList;
        private System.Windows.Forms.ToolStripComboBox tscbSendList;
    }
}