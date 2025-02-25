
namespace WPELibrary
{
    partial class Socket_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Socket_Form));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle26 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle25 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle27 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle28 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle29 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle30 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle31 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlpSocketForm = new System.Windows.Forms.TableLayoutPanel();
            this.ssSocketList = new System.Windows.Forms.StatusStrip();
            this.tlTotal = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlTotal_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSplit1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlFilterExecute = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlFilterExecute_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSplit4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlQueue = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlQueue_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSplit2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlFilterSocketList = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlFilterSocketList_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel9 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSend = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSend_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSplit3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlRecv = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlRecv_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSendTo = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSendTo_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlRecvFrom = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlRecvFrom_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel8 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlWSASend = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlWSASend_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel11 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlWSARecv = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlWSARecv_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel14 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlWSASendTo = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlWSASendTo_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel17 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlWSARecvFrom = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlWSARecvFrom_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.dgvSocketList = new System.Windows.Forms.DataGridView();
            this.cTypeImg = new System.Windows.Forms.DataGridViewImageColumn();
            this.cPacketID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cPacketType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cSocket = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cFrom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cTo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLen = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmsSocketList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsSocketList_Send = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsSocketList_tss1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsSocketList_SendList = new System.Windows.Forms.ToolStripMenuItem();
            this.tscbSendList = new System.Windows.Forms.ToolStripComboBox();
            this.cmsSocketList_FilterList = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsSocketList_SystemSocket = new System.Windows.Forms.ToolStripMenuItem();
            this.tss5 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsSocketList_ShowModified = new System.Windows.Forms.ToolStripMenuItem();
            this.tss6 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsSocketList_ToExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsSocketList_Comparison_A = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsSocketList_Comparison_B = new System.Windows.Forms.ToolStripMenuItem();
            this.tlpParameter = new System.Windows.Forms.TableLayoutPanel();
            this.gbHookButton_Search = new System.Windows.Forms.GroupBox();
            this.tlpSearch = new System.Windows.Forms.TableLayoutPanel();
            this.rbFromIndex = new System.Windows.Forms.RadioButton();
            this.rbFromHead = new System.Windows.Forms.RadioButton();
            this.tlpSearchButton = new System.Windows.Forms.TableLayoutPanel();
            this.bSearchNext = new System.Windows.Forms.Button();
            this.bSearch = new System.Windows.Forms.Button();
            this.tlpHookButton = new System.Windows.Forms.TableLayoutPanel();
            this.tlpHookButton_Start = new System.Windows.Forms.TableLayoutPanel();
            this.bStopHook = new System.Windows.Forms.Button();
            this.bStartHook = new System.Windows.Forms.Button();
            this.bCleanUp = new System.Windows.Forms.Button();
            this.tcSocketInfo = new System.Windows.Forms.TabControl();
            this.tcSocketInfo_FilterSet = new System.Windows.Forms.TabPage();
            this.tlpFilterSet = new System.Windows.Forms.TableLayoutPanel();
            this.txtCheckLength = new System.Windows.Forms.TextBox();
            this.txtCheckHead = new System.Windows.Forms.TextBox();
            this.cbCheckHead = new System.Windows.Forms.CheckBox();
            this.cbCheckSize = new System.Windows.Forms.CheckBox();
            this.rbFilter_Show = new System.Windows.Forms.RadioButton();
            this.rbFilter_NotShow = new System.Windows.Forms.RadioButton();
            this.txtCheckPort = new System.Windows.Forms.TextBox();
            this.cbCheckPort = new System.Windows.Forms.CheckBox();
            this.txtCheckData = new System.Windows.Forms.TextBox();
            this.cbCheckData = new System.Windows.Forms.CheckBox();
            this.cbCheckSocket = new System.Windows.Forms.CheckBox();
            this.cbCheckIP = new System.Windows.Forms.CheckBox();
            this.txtCheckSocket = new System.Windows.Forms.TextBox();
            this.txtCheckIP = new System.Windows.Forms.TextBox();
            this.tcSocketInfo_HookSet = new System.Windows.Forms.TabPage();
            this.tlpHookSet = new System.Windows.Forms.TableLayoutPanel();
            this.gbHookSet_WinsockWSA = new System.Windows.Forms.GroupBox();
            this.tlpHookSet_WinsockWSA = new System.Windows.Forms.TableLayoutPanel();
            this.cbHookWS2_RecvFrom = new System.Windows.Forms.CheckBox();
            this.cbHookWS2_Recv = new System.Windows.Forms.CheckBox();
            this.cbHookWS2_SendTo = new System.Windows.Forms.CheckBox();
            this.cbHookWS2_Send = new System.Windows.Forms.CheckBox();
            this.cbHookWSA_RecvFrom = new System.Windows.Forms.CheckBox();
            this.cbHookWSA_Recv = new System.Windows.Forms.CheckBox();
            this.cbHookWSA_SendTo = new System.Windows.Forms.CheckBox();
            this.cbHookWSA_Send = new System.Windows.Forms.CheckBox();
            this.gbHookSet_Winsock = new System.Windows.Forms.GroupBox();
            this.tlpHookSet_Winsock = new System.Windows.Forms.TableLayoutPanel();
            this.cbHookWS1_RecvFrom = new System.Windows.Forms.CheckBox();
            this.cbHookWS1_Recv = new System.Windows.Forms.CheckBox();
            this.cbHookWS1_SendTo = new System.Windows.Forms.CheckBox();
            this.cbHookWS1_Send = new System.Windows.Forms.CheckBox();
            this.tcSocketInfo_ListSet = new System.Windows.Forms.TabPage();
            this.tlpListSet = new System.Windows.Forms.TableLayoutPanel();
            this.gbListSet_LogList = new System.Windows.Forms.GroupBox();
            this.tlpListSet_LogList = new System.Windows.Forms.TableLayoutPanel();
            this.nudLogList_AutoClearValue = new System.Windows.Forms.NumericUpDown();
            this.cbLogList_AutoClear = new System.Windows.Forms.CheckBox();
            this.cbLogList_AutoRoll = new System.Windows.Forms.CheckBox();
            this.gbListSet_SocketList = new System.Windows.Forms.GroupBox();
            this.tlpListSet_SocketList = new System.Windows.Forms.TableLayoutPanel();
            this.nudSocketList_AutoClearValue = new System.Windows.Forms.NumericUpDown();
            this.cbSocketList_AutoClear = new System.Windows.Forms.CheckBox();
            this.cbSocketList_AutoRoll = new System.Windows.Forms.CheckBox();
            this.tcSocketInfo_HotKey = new System.Windows.Forms.TabPage();
            this.tlpHotKeySet = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.lHotKey_RobotList = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tcSocketInfo_SystemSet = new System.Windows.Forms.TabPage();
            this.tlpSystemSet = new System.Windows.Forms.TableLayoutPanel();
            this.gbSystemSet_ShowMode = new System.Windows.Forms.GroupBox();
            this.tlpSystemSet_ShowMode = new System.Windows.Forms.TableLayoutPanel();
            this.cbTopMost = new System.Windows.Forms.CheckBox();
            this.gbSystemSet_FilterSet = new System.Windows.Forms.GroupBox();
            this.tlpSystemSet_FilterSet = new System.Windows.Forms.TableLayoutPanel();
            this.rbFilterSet_Sequence = new System.Windows.Forms.RadioButton();
            this.rbFilterSet_Priority = new System.Windows.Forms.RadioButton();
            this.gbSystemSet_WorkMode = new System.Windows.Forms.GroupBox();
            this.tlpSystemSet_WorkMode = new System.Windows.Forms.TableLayoutPanel();
            this.cbWorkingMode_Speed = new System.Windows.Forms.CheckBox();
            this.tlpInformation = new System.Windows.Forms.TableLayoutPanel();
            this.tlpPacketInfo = new System.Windows.Forms.TableLayoutPanel();
            this.tcPacketInfo = new System.Windows.Forms.TabControl();
            this.tpPacketData = new System.Windows.Forms.TabPage();
            this.tlpPacketData = new System.Windows.Forms.TableLayoutPanel();
            this.tlpHexBox = new System.Windows.Forms.TableLayoutPanel();
            this.hbPacketData = new Be.Windows.Forms.HexBox();
            this.cmsHexBox = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsHexBox_Send = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsHexBox_tss1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsHexBox_SendList = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsHexBox_tscbSendList = new System.Windows.Forms.ToolStripComboBox();
            this.cmsHexBox_FilterList = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsHexBox_tss3 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsHexBox_CopyHex = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsHexBox_CopyText = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsHexBox_tss4 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsHexBox_Comparison_A = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsHexBox_Comparison_B = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsHexBox_tss5 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsHexBox_SelectAll = new System.Windows.Forms.ToolStripMenuItem();
            this.tpComparison = new System.Windows.Forms.TabPage();
            this.tlpComparison = new System.Windows.Forms.TableLayoutPanel();
            this.tlpComparison_Button = new System.Windows.Forms.TableLayoutPanel();
            this.bComparison_Exchange = new System.Windows.Forms.Button();
            this.bComparison_Clear = new System.Windows.Forms.Button();
            this.bComparison = new System.Windows.Forms.Button();
            this.lComparison_B = new System.Windows.Forms.Label();
            this.lComparison_A = new System.Windows.Forms.Label();
            this.pComparison_A = new System.Windows.Forms.Panel();
            this.rtbComparison_A = new System.Windows.Forms.RichTextBox();
            this.pComparison_B = new System.Windows.Forms.Panel();
            this.rtbComparison_B = new System.Windows.Forms.RichTextBox();
            this.pComparison_Result = new System.Windows.Forms.Panel();
            this.rtbComparison_Result = new System.Windows.Forms.RichTextBox();
            this.tpXOR = new System.Windows.Forms.TabPage();
            this.tlpPacketInfo_XOR = new System.Windows.Forms.TableLayoutPanel();
            this.hbXOR_To = new Be.Windows.Forms.HexBox();
            this.tlpPacketInfo_XOR_Button = new System.Windows.Forms.TableLayoutPanel();
            this.lXOR2 = new System.Windows.Forms.Label();
            this.bXOR_Clear = new System.Windows.Forms.Button();
            this.bXOR = new System.Windows.Forms.Button();
            this.lXOR = new System.Windows.Forms.Label();
            this.txtXOR = new System.Windows.Forms.TextBox();
            this.hbXOR_From = new Be.Windows.Forms.HexBox();
            this.tpEncoding = new System.Windows.Forms.TabPage();
            this.tlpPacketInfo_Encoding = new System.Windows.Forms.TableLayoutPanel();
            this.tlpPacketInfo_Encoding_Button = new System.Windows.Forms.TableLayoutPanel();
            this.bPacketInfo_Decoding = new System.Windows.Forms.Button();
            this.bPacketInfo_Encoding = new System.Windows.Forms.Button();
            this.pbEncoding = new System.Windows.Forms.PictureBox();
            this.tlpPacketInfo_Encoding_Result = new System.Windows.Forms.TableLayoutPanel();
            this.txtPacketInfo_Encoding_ANSIUnicode = new System.Windows.Forms.TextBox();
            this.lPacketInfo_Encoding_ANSIUnicode = new System.Windows.Forms.Label();
            this.txtPacketInfo_Encoding_ANSIUTF32 = new System.Windows.Forms.TextBox();
            this.lPacketInfo_Encoding_ANSIUTF32 = new System.Windows.Forms.Label();
            this.txtPacketInfo_Encoding_ANSIUTF16 = new System.Windows.Forms.TextBox();
            this.lPacketInfo_Encoding_ANSIASCII = new System.Windows.Forms.Label();
            this.txtPacketInfo_Encoding_Unicode = new System.Windows.Forms.TextBox();
            this.lPacketInfo_Encoding_Unicode = new System.Windows.Forms.Label();
            this.txtPacketInfo_Encoding_UTF32 = new System.Windows.Forms.TextBox();
            this.lPacketInfo_Encoding_UTF32 = new System.Windows.Forms.Label();
            this.txtPacketInfo_Encoding_UTF16 = new System.Windows.Forms.TextBox();
            this.lPacketInfo_Encoding_ASCII = new System.Windows.Forms.Label();
            this.txtPacketInfo_Encoding_ANSIbase64 = new System.Windows.Forms.TextBox();
            this.txtPacketInfo_Encoding_ANSIUTF8 = new System.Windows.Forms.TextBox();
            this.txtPacketInfo_Encoding_ANSIUTF7 = new System.Windows.Forms.TextBox();
            this.txtPacketInfo_Encoding_ANSIGBK = new System.Windows.Forms.TextBox();
            this.lPacketInfo_Encoding_ANSIbase64 = new System.Windows.Forms.Label();
            this.lPacketInfo_Encoding_ANSIUTF8 = new System.Windows.Forms.Label();
            this.lPacketInfo_Encoding_ANSIUTF7 = new System.Windows.Forms.Label();
            this.lPacketInfo_Encoding_UTF7 = new System.Windows.Forms.Label();
            this.lPacketInfo_Encoding_ANSIGBK = new System.Windows.Forms.Label();
            this.txtPacketInfo_Encoding_base64 = new System.Windows.Forms.TextBox();
            this.txtPacketInfo_Encoding_UTF8 = new System.Windows.Forms.TextBox();
            this.txtPacketInfo_Encoding_UTF7 = new System.Windows.Forms.TextBox();
            this.lPacketInfo_Encoding_base64 = new System.Windows.Forms.Label();
            this.lPacketInfo_Encoding_UTF8 = new System.Windows.Forms.Label();
            this.lPacketInfo_Encoding_Bytes = new System.Windows.Forms.Label();
            this.txtPacketInfo_Encoding_Bytes = new System.Windows.Forms.TextBox();
            this.pPacketInfo_Encoding = new System.Windows.Forms.Panel();
            this.rtbPacketInfo_Encoding = new System.Windows.Forms.RichTextBox();
            this.tpExtraction = new System.Windows.Forms.TabPage();
            this.tlpExtraction = new System.Windows.Forms.TableLayoutPanel();
            this.pExtraction = new System.Windows.Forms.Panel();
            this.rtbExtraction = new System.Windows.Forms.RichTextBox();
            this.cmsExtraction = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsExtraction_Export = new System.Windows.Forms.ToolStripMenuItem();
            this.tlpExtraction_Button = new System.Windows.Forms.TableLayoutPanel();
            this.bExtraction = new System.Windows.Forms.Button();
            this.cbbExtraction = new System.Windows.Forms.ComboBox();
            this.tpSystemLog = new System.Windows.Forms.TabPage();
            this.dgvLogList = new System.Windows.Forms.DataGridView();
            this.cLogID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLogTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cFuncName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLogContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmsLogList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsLogList_CleanUp = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsLogList_ToExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.tcAutomation = new System.Windows.Forms.TabControl();
            this.tpFilterList = new System.Windows.Forms.TabPage();
            this.tlpFilterList = new System.Windows.Forms.TableLayoutPanel();
            this.dgvFilterList = new System.Windows.Forms.DataGridView();
            this.cIsCheck = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cFName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmsFilterList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsFilterList_Top = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsFilterList_tss1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsFilterList_Up = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsFilterList_Down = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsFilterList_tss2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsFilterList_Bottom = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsFilterList_tss3 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsFilterList_Copy = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsFilterList_Export = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsFilterList_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.tsFilterList = new System.Windows.Forms.ToolStrip();
            this.tsFilterList_Load = new System.Windows.Forms.ToolStripButton();
            this.tsFilterList_Save = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsFilterList_SelectAll = new System.Windows.Forms.ToolStripButton();
            this.tsFilterList_SelectNo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsFilterList_Add = new System.Windows.Forms.ToolStripButton();
            this.tsFilterList_CleanUp = new System.Windows.Forms.ToolStripButton();
            this.tpSendList = new System.Windows.Forms.TabPage();
            this.tlpSendList = new System.Windows.Forms.TableLayoutPanel();
            this.dgvSendList = new System.Windows.Forms.DataGridView();
            this.cIsEnable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewImageColumn3 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmsSendList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsSendList_Top = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsSendList_Up = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsSendList_Down = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsSendList_Bottom = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsSendList_Copy = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsSendList_Export = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsSendList_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSendList = new System.Windows.Forms.ToolStrip();
            this.tsSendList_Load = new System.Windows.Forms.ToolStripButton();
            this.tsSendList_Save = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.tsSendList_Start = new System.Windows.Forms.ToolStripButton();
            this.tsSendList_Stop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.tsSendList_Add = new System.Windows.Forms.ToolStripButton();
            this.tsSendList_CleanUp = new System.Windows.Forms.ToolStripButton();
            this.tpRobotList = new System.Windows.Forms.TabPage();
            this.tlpRobotList = new System.Windows.Forms.TableLayoutPanel();
            this.dgvRobotList = new System.Windows.Forms.DataGridView();
            this.cRIsEnable = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.cImg = new System.Windows.Forms.DataGridViewImageColumn();
            this.cRName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmsRobotList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsRobotList_Top = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsRobotList_Split1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsRobotList_Up = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsRobotList_Down = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsRobotList_Split2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsRobotList_Bottom = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsRobotList_Split3 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsRobotList_Copy = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsRobotList_Export = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsRobotList_Delete = new System.Windows.Forms.ToolStripMenuItem();
            this.tsRobotList = new System.Windows.Forms.ToolStrip();
            this.tsRobotList_Load = new System.Windows.Forms.ToolStripButton();
            this.tsRobotList_Save = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsRobotList_Start = new System.Windows.Forms.ToolStripButton();
            this.tsRobotList_Stop = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.tsRobotList_Add = new System.Windows.Forms.ToolStripButton();
            this.tsRobotList_CleanUp = new System.Windows.Forms.ToolStripButton();
            this.ssProcessInfo = new System.Windows.Forms.StatusStrip();
            this.tsslProcessName = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslSplit2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslProcessInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslSplit1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslWinSock = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslSplit3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslTotalBytes = new System.Windows.Forms.ToolStripStatusLabel();
            this.tSocketInfo = new System.Windows.Forms.Timer(this.components);
            this.niWPE = new System.Windows.Forms.NotifyIcon(this.components);
            this.cmsIcon = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsIcon_Show = new System.Windows.Forms.ToolStripMenuItem();
            this.tss17 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsIcon_StartHook = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsIcon_StopHook = new System.Windows.Forms.ToolStripMenuItem();
            this.tss18 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsIcon_CleanUp = new System.Windows.Forms.ToolStripMenuItem();
            this.tss19 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsIcon_Exit = new System.Windows.Forms.ToolStripMenuItem();
            this.tSocketList = new System.Windows.Forms.Timer(this.components);
            this.ofdExtraction = new System.Windows.Forms.OpenFileDialog();
            this.sfdExtraction = new System.Windows.Forms.SaveFileDialog();
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.bgwSendList = new System.ComponentModel.BackgroundWorker();
            this.bgwRobotList = new System.ComponentModel.BackgroundWorker();
            this.tlpSocketForm.SuspendLayout();
            this.ssSocketList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSocketList)).BeginInit();
            this.cmsSocketList.SuspendLayout();
            this.tlpParameter.SuspendLayout();
            this.gbHookButton_Search.SuspendLayout();
            this.tlpSearch.SuspendLayout();
            this.tlpSearchButton.SuspendLayout();
            this.tlpHookButton.SuspendLayout();
            this.tlpHookButton_Start.SuspendLayout();
            this.tcSocketInfo.SuspendLayout();
            this.tcSocketInfo_FilterSet.SuspendLayout();
            this.tlpFilterSet.SuspendLayout();
            this.tcSocketInfo_HookSet.SuspendLayout();
            this.tlpHookSet.SuspendLayout();
            this.gbHookSet_WinsockWSA.SuspendLayout();
            this.tlpHookSet_WinsockWSA.SuspendLayout();
            this.gbHookSet_Winsock.SuspendLayout();
            this.tlpHookSet_Winsock.SuspendLayout();
            this.tcSocketInfo_ListSet.SuspendLayout();
            this.tlpListSet.SuspendLayout();
            this.gbListSet_LogList.SuspendLayout();
            this.tlpListSet_LogList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLogList_AutoClearValue)).BeginInit();
            this.gbListSet_SocketList.SuspendLayout();
            this.tlpListSet_SocketList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSocketList_AutoClearValue)).BeginInit();
            this.tcSocketInfo_HotKey.SuspendLayout();
            this.tlpHotKeySet.SuspendLayout();
            this.tcSocketInfo_SystemSet.SuspendLayout();
            this.tlpSystemSet.SuspendLayout();
            this.gbSystemSet_ShowMode.SuspendLayout();
            this.tlpSystemSet_ShowMode.SuspendLayout();
            this.gbSystemSet_FilterSet.SuspendLayout();
            this.tlpSystemSet_FilterSet.SuspendLayout();
            this.gbSystemSet_WorkMode.SuspendLayout();
            this.tlpSystemSet_WorkMode.SuspendLayout();
            this.tlpInformation.SuspendLayout();
            this.tlpPacketInfo.SuspendLayout();
            this.tcPacketInfo.SuspendLayout();
            this.tpPacketData.SuspendLayout();
            this.tlpPacketData.SuspendLayout();
            this.tlpHexBox.SuspendLayout();
            this.cmsHexBox.SuspendLayout();
            this.tpComparison.SuspendLayout();
            this.tlpComparison.SuspendLayout();
            this.tlpComparison_Button.SuspendLayout();
            this.pComparison_A.SuspendLayout();
            this.pComparison_B.SuspendLayout();
            this.pComparison_Result.SuspendLayout();
            this.tpXOR.SuspendLayout();
            this.tlpPacketInfo_XOR.SuspendLayout();
            this.tlpPacketInfo_XOR_Button.SuspendLayout();
            this.tpEncoding.SuspendLayout();
            this.tlpPacketInfo_Encoding.SuspendLayout();
            this.tlpPacketInfo_Encoding_Button.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbEncoding)).BeginInit();
            this.tlpPacketInfo_Encoding_Result.SuspendLayout();
            this.pPacketInfo_Encoding.SuspendLayout();
            this.tpExtraction.SuspendLayout();
            this.tlpExtraction.SuspendLayout();
            this.pExtraction.SuspendLayout();
            this.cmsExtraction.SuspendLayout();
            this.tlpExtraction_Button.SuspendLayout();
            this.tpSystemLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogList)).BeginInit();
            this.cmsLogList.SuspendLayout();
            this.tcAutomation.SuspendLayout();
            this.tpFilterList.SuspendLayout();
            this.tlpFilterList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilterList)).BeginInit();
            this.cmsFilterList.SuspendLayout();
            this.tsFilterList.SuspendLayout();
            this.tpSendList.SuspendLayout();
            this.tlpSendList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSendList)).BeginInit();
            this.cmsSendList.SuspendLayout();
            this.tsSendList.SuspendLayout();
            this.tpRobotList.SuspendLayout();
            this.tlpRobotList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRobotList)).BeginInit();
            this.cmsRobotList.SuspendLayout();
            this.tsRobotList.SuspendLayout();
            this.ssProcessInfo.SuspendLayout();
            this.cmsIcon.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpSocketForm
            // 
            resources.ApplyResources(this.tlpSocketForm, "tlpSocketForm");
            this.tlpSocketForm.Controls.Add(this.ssSocketList, 0, 1);
            this.tlpSocketForm.Controls.Add(this.dgvSocketList, 0, 2);
            this.tlpSocketForm.Controls.Add(this.tlpParameter, 0, 0);
            this.tlpSocketForm.Controls.Add(this.tlpInformation, 0, 3);
            this.tlpSocketForm.Controls.Add(this.ssProcessInfo, 0, 4);
            this.tlpSocketForm.Name = "tlpSocketForm";
            // 
            // ssSocketList
            // 
            resources.ApplyResources(this.ssSocketList, "ssSocketList");
            this.ssSocketList.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.ssSocketList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlTotal,
            this.tlTotal_CNT,
            this.tlSplit1,
            this.tlFilterExecute,
            this.tlFilterExecute_CNT,
            this.tlSplit4,
            this.tlQueue,
            this.tlQueue_CNT,
            this.tlSplit2,
            this.tlFilterSocketList,
            this.tlFilterSocketList_CNT,
            this.toolStripStatusLabel9,
            this.tlSend,
            this.tlSend_CNT,
            this.tlSplit3,
            this.tlRecv,
            this.tlRecv_CNT,
            this.toolStripStatusLabel2,
            this.tlSendTo,
            this.tlSendTo_CNT,
            this.toolStripStatusLabel5,
            this.tlRecvFrom,
            this.tlRecvFrom_CNT,
            this.toolStripStatusLabel8,
            this.tlWSASend,
            this.tlWSASend_CNT,
            this.toolStripStatusLabel11,
            this.tlWSARecv,
            this.tlWSARecv_CNT,
            this.toolStripStatusLabel14,
            this.tlWSASendTo,
            this.tlWSASendTo_CNT,
            this.toolStripStatusLabel17,
            this.tlWSARecvFrom,
            this.tlWSARecvFrom_CNT});
            this.ssSocketList.Name = "ssSocketList";
            this.ssSocketList.SizingGrip = false;
            // 
            // tlTotal
            // 
            resources.ApplyResources(this.tlTotal, "tlTotal");
            this.tlTotal.Name = "tlTotal";
            // 
            // tlTotal_CNT
            // 
            resources.ApplyResources(this.tlTotal_CNT, "tlTotal_CNT");
            this.tlTotal_CNT.Name = "tlTotal_CNT";
            // 
            // tlSplit1
            // 
            resources.ApplyResources(this.tlSplit1, "tlSplit1");
            this.tlSplit1.ForeColor = System.Drawing.Color.DarkGray;
            this.tlSplit1.Name = "tlSplit1";
            // 
            // tlFilterExecute
            // 
            resources.ApplyResources(this.tlFilterExecute, "tlFilterExecute");
            this.tlFilterExecute.Name = "tlFilterExecute";
            // 
            // tlFilterExecute_CNT
            // 
            resources.ApplyResources(this.tlFilterExecute_CNT, "tlFilterExecute_CNT");
            this.tlFilterExecute_CNT.Name = "tlFilterExecute_CNT";
            // 
            // tlSplit4
            // 
            resources.ApplyResources(this.tlSplit4, "tlSplit4");
            this.tlSplit4.ForeColor = System.Drawing.Color.DarkGray;
            this.tlSplit4.Name = "tlSplit4";
            // 
            // tlQueue
            // 
            resources.ApplyResources(this.tlQueue, "tlQueue");
            this.tlQueue.Name = "tlQueue";
            // 
            // tlQueue_CNT
            // 
            resources.ApplyResources(this.tlQueue_CNT, "tlQueue_CNT");
            this.tlQueue_CNT.Name = "tlQueue_CNT";
            // 
            // tlSplit2
            // 
            resources.ApplyResources(this.tlSplit2, "tlSplit2");
            this.tlSplit2.ForeColor = System.Drawing.Color.DarkGray;
            this.tlSplit2.Name = "tlSplit2";
            // 
            // tlFilterSocketList
            // 
            resources.ApplyResources(this.tlFilterSocketList, "tlFilterSocketList");
            this.tlFilterSocketList.Name = "tlFilterSocketList";
            // 
            // tlFilterSocketList_CNT
            // 
            resources.ApplyResources(this.tlFilterSocketList_CNT, "tlFilterSocketList_CNT");
            this.tlFilterSocketList_CNT.Name = "tlFilterSocketList_CNT";
            // 
            // toolStripStatusLabel9
            // 
            resources.ApplyResources(this.toolStripStatusLabel9, "toolStripStatusLabel9");
            this.toolStripStatusLabel9.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel9.Name = "toolStripStatusLabel9";
            // 
            // tlSend
            // 
            resources.ApplyResources(this.tlSend, "tlSend");
            this.tlSend.Name = "tlSend";
            // 
            // tlSend_CNT
            // 
            resources.ApplyResources(this.tlSend_CNT, "tlSend_CNT");
            this.tlSend_CNT.Name = "tlSend_CNT";
            // 
            // tlSplit3
            // 
            resources.ApplyResources(this.tlSplit3, "tlSplit3");
            this.tlSplit3.ForeColor = System.Drawing.Color.DarkGray;
            this.tlSplit3.Name = "tlSplit3";
            // 
            // tlRecv
            // 
            resources.ApplyResources(this.tlRecv, "tlRecv");
            this.tlRecv.Name = "tlRecv";
            // 
            // tlRecv_CNT
            // 
            resources.ApplyResources(this.tlRecv_CNT, "tlRecv_CNT");
            this.tlRecv_CNT.Name = "tlRecv_CNT";
            // 
            // toolStripStatusLabel2
            // 
            resources.ApplyResources(this.toolStripStatusLabel2, "toolStripStatusLabel2");
            this.toolStripStatusLabel2.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            // 
            // tlSendTo
            // 
            resources.ApplyResources(this.tlSendTo, "tlSendTo");
            this.tlSendTo.Name = "tlSendTo";
            // 
            // tlSendTo_CNT
            // 
            resources.ApplyResources(this.tlSendTo_CNT, "tlSendTo_CNT");
            this.tlSendTo_CNT.Name = "tlSendTo_CNT";
            // 
            // toolStripStatusLabel5
            // 
            resources.ApplyResources(this.toolStripStatusLabel5, "toolStripStatusLabel5");
            this.toolStripStatusLabel5.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            // 
            // tlRecvFrom
            // 
            resources.ApplyResources(this.tlRecvFrom, "tlRecvFrom");
            this.tlRecvFrom.Name = "tlRecvFrom";
            // 
            // tlRecvFrom_CNT
            // 
            resources.ApplyResources(this.tlRecvFrom_CNT, "tlRecvFrom_CNT");
            this.tlRecvFrom_CNT.Name = "tlRecvFrom_CNT";
            // 
            // toolStripStatusLabel8
            // 
            resources.ApplyResources(this.toolStripStatusLabel8, "toolStripStatusLabel8");
            this.toolStripStatusLabel8.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel8.Name = "toolStripStatusLabel8";
            // 
            // tlWSASend
            // 
            resources.ApplyResources(this.tlWSASend, "tlWSASend");
            this.tlWSASend.Name = "tlWSASend";
            // 
            // tlWSASend_CNT
            // 
            resources.ApplyResources(this.tlWSASend_CNT, "tlWSASend_CNT");
            this.tlWSASend_CNT.Name = "tlWSASend_CNT";
            // 
            // toolStripStatusLabel11
            // 
            resources.ApplyResources(this.toolStripStatusLabel11, "toolStripStatusLabel11");
            this.toolStripStatusLabel11.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel11.Name = "toolStripStatusLabel11";
            // 
            // tlWSARecv
            // 
            resources.ApplyResources(this.tlWSARecv, "tlWSARecv");
            this.tlWSARecv.Name = "tlWSARecv";
            // 
            // tlWSARecv_CNT
            // 
            resources.ApplyResources(this.tlWSARecv_CNT, "tlWSARecv_CNT");
            this.tlWSARecv_CNT.Name = "tlWSARecv_CNT";
            // 
            // toolStripStatusLabel14
            // 
            resources.ApplyResources(this.toolStripStatusLabel14, "toolStripStatusLabel14");
            this.toolStripStatusLabel14.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel14.Name = "toolStripStatusLabel14";
            // 
            // tlWSASendTo
            // 
            resources.ApplyResources(this.tlWSASendTo, "tlWSASendTo");
            this.tlWSASendTo.Name = "tlWSASendTo";
            // 
            // tlWSASendTo_CNT
            // 
            resources.ApplyResources(this.tlWSASendTo_CNT, "tlWSASendTo_CNT");
            this.tlWSASendTo_CNT.Name = "tlWSASendTo_CNT";
            // 
            // toolStripStatusLabel17
            // 
            resources.ApplyResources(this.toolStripStatusLabel17, "toolStripStatusLabel17");
            this.toolStripStatusLabel17.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel17.Name = "toolStripStatusLabel17";
            // 
            // tlWSARecvFrom
            // 
            resources.ApplyResources(this.tlWSARecvFrom, "tlWSARecvFrom");
            this.tlWSARecvFrom.Name = "tlWSARecvFrom";
            // 
            // tlWSARecvFrom_CNT
            // 
            resources.ApplyResources(this.tlWSARecvFrom_CNT, "tlWSARecvFrom_CNT");
            this.tlWSARecvFrom_CNT.Name = "tlWSARecvFrom_CNT";
            // 
            // dgvSocketList
            // 
            resources.ApplyResources(this.dgvSocketList, "dgvSocketList");
            this.dgvSocketList.AllowUserToAddRows = false;
            this.dgvSocketList.AllowUserToDeleteRows = false;
            this.dgvSocketList.AllowUserToResizeColumns = false;
            this.dgvSocketList.AllowUserToResizeRows = false;
            this.dgvSocketList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvSocketList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvSocketList.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle18.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            dataGridViewCellStyle18.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle18.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle18.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle18.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgvSocketList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle18;
            this.dgvSocketList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSocketList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cTypeImg,
            this.cPacketID,
            this.cPacketType,
            this.cSocket,
            this.cFrom,
            this.cTo,
            this.cLen,
            this.cData});
            this.dgvSocketList.ContextMenuStrip = this.cmsSocketList;
            dataGridViewCellStyle26.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle26.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle26.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            dataGridViewCellStyle26.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle26.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle26.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle26.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle26.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSocketList.DefaultCellStyle = dataGridViewCellStyle26;
            this.dgvSocketList.Name = "dgvSocketList";
            this.dgvSocketList.RowHeadersVisible = false;
            this.dgvSocketList.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.dgvSocketList.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvSocketList.RowTemplate.DefaultCellStyle.ForeColor = System.Drawing.Color.LimeGreen;
            this.dgvSocketList.RowTemplate.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(3);
            this.dgvSocketList.RowTemplate.Height = 23;
            this.dgvSocketList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSocketList.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvSocketList_CellFormatting);
            this.dgvSocketList.SelectionChanged += new System.EventHandler(this.dgvSocketInfo_SelectionChanged);
            // 
            // cTypeImg
            // 
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle19.NullValue = null;
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cTypeImg.DefaultCellStyle = dataGridViewCellStyle19;
            resources.ApplyResources(this.cTypeImg, "cTypeImg");
            this.cTypeImg.Image = global::WPELibrary.Properties.Resources.Info16;
            this.cTypeImg.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Zoom;
            this.cTypeImg.Name = "cTypeImg";
            this.cTypeImg.ReadOnly = true;
            this.cTypeImg.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // cPacketID
            // 
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cPacketID.DefaultCellStyle = dataGridViewCellStyle20;
            resources.ApplyResources(this.cPacketID, "cPacketID");
            this.cPacketID.Name = "cPacketID";
            this.cPacketID.ReadOnly = true;
            this.cPacketID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cPacketType
            // 
            this.cPacketType.DataPropertyName = "PacketType";
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cPacketType.DefaultCellStyle = dataGridViewCellStyle21;
            resources.ApplyResources(this.cPacketType, "cPacketType");
            this.cPacketType.Name = "cPacketType";
            this.cPacketType.ReadOnly = true;
            this.cPacketType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cSocket
            // 
            this.cSocket.DataPropertyName = "PacketSocket";
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cSocket.DefaultCellStyle = dataGridViewCellStyle22;
            resources.ApplyResources(this.cSocket, "cSocket");
            this.cSocket.Name = "cSocket";
            this.cSocket.ReadOnly = true;
            this.cSocket.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cFrom
            // 
            this.cFrom.DataPropertyName = "PacketFrom";
            dataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.cFrom.DefaultCellStyle = dataGridViewCellStyle23;
            resources.ApplyResources(this.cFrom, "cFrom");
            this.cFrom.Name = "cFrom";
            this.cFrom.ReadOnly = true;
            this.cFrom.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cTo
            // 
            this.cTo.DataPropertyName = "PacketTo";
            dataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.cTo.DefaultCellStyle = dataGridViewCellStyle24;
            resources.ApplyResources(this.cTo, "cTo");
            this.cTo.Name = "cTo";
            this.cTo.ReadOnly = true;
            this.cTo.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cLen
            // 
            this.cLen.DataPropertyName = "PacketLen";
            dataGridViewCellStyle25.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cLen.DefaultCellStyle = dataGridViewCellStyle25;
            resources.ApplyResources(this.cLen, "cLen");
            this.cLen.Name = "cLen";
            this.cLen.ReadOnly = true;
            this.cLen.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cData
            // 
            this.cData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cData.DataPropertyName = "PacketData";
            resources.ApplyResources(this.cData, "cData");
            this.cData.Name = "cData";
            this.cData.ReadOnly = true;
            this.cData.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cmsSocketList
            // 
            resources.ApplyResources(this.cmsSocketList, "cmsSocketList");
            this.cmsSocketList.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.cmsSocketList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsSocketList_Send,
            this.cmsSocketList_tss1,
            this.cmsSocketList_SendList,
            this.cmsSocketList_FilterList,
            this.toolStripSeparator6,
            this.cmsSocketList_SystemSocket,
            this.tss5,
            this.cmsSocketList_ShowModified,
            this.tss6,
            this.cmsSocketList_ToExcel,
            this.toolStripSeparator3,
            this.cmsSocketList_Comparison_A,
            this.cmsSocketList_Comparison_B});
            this.cmsSocketList.Name = "cmsSocketInfo";
            this.cmsSocketList.Opening += new System.ComponentModel.CancelEventHandler(this.cmsSocketList_Opening);
            this.cmsSocketList.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsSocketList_ItemClicked);
            // 
            // cmsSocketList_Send
            // 
            resources.ApplyResources(this.cmsSocketList_Send, "cmsSocketList_Send");
            this.cmsSocketList_Send.Image = global::WPELibrary.Properties.Resources.sent;
            this.cmsSocketList_Send.Name = "cmsSocketList_Send";
            // 
            // cmsSocketList_tss1
            // 
            resources.ApplyResources(this.cmsSocketList_tss1, "cmsSocketList_tss1");
            this.cmsSocketList_tss1.Name = "cmsSocketList_tss1";
            // 
            // cmsSocketList_SendList
            // 
            resources.ApplyResources(this.cmsSocketList_SendList, "cmsSocketList_SendList");
            this.cmsSocketList_SendList.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tscbSendList});
            this.cmsSocketList_SendList.Image = global::WPELibrary.Properties.Resources.addto;
            this.cmsSocketList_SendList.Name = "cmsSocketList_SendList";
            // 
            // tscbSendList
            // 
            resources.ApplyResources(this.tscbSendList, "tscbSendList");
            this.tscbSendList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tscbSendList.Name = "tscbSendList";
            this.tscbSendList.SelectedIndexChanged += new System.EventHandler(this.tscbSendList_SelectedIndexChanged);
            // 
            // cmsSocketList_FilterList
            // 
            resources.ApplyResources(this.cmsSocketList_FilterList, "cmsSocketList_FilterList");
            this.cmsSocketList_FilterList.Image = global::WPELibrary.Properties.Resources.addto;
            this.cmsSocketList_FilterList.Name = "cmsSocketList_FilterList";
            // 
            // toolStripSeparator6
            // 
            resources.ApplyResources(this.toolStripSeparator6, "toolStripSeparator6");
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            // 
            // cmsSocketList_SystemSocket
            // 
            resources.ApplyResources(this.cmsSocketList_SystemSocket, "cmsSocketList_SystemSocket");
            this.cmsSocketList_SystemSocket.Image = global::WPELibrary.Properties.Resources.eTree_true;
            this.cmsSocketList_SystemSocket.Name = "cmsSocketList_SystemSocket";
            // 
            // tss5
            // 
            resources.ApplyResources(this.tss5, "tss5");
            this.tss5.Name = "tss5";
            // 
            // cmsSocketList_ShowModified
            // 
            resources.ApplyResources(this.cmsSocketList_ShowModified, "cmsSocketList_ShowModified");
            this.cmsSocketList_ShowModified.Image = global::WPELibrary.Properties.Resources.Compare;
            this.cmsSocketList_ShowModified.Name = "cmsSocketList_ShowModified";
            // 
            // tss6
            // 
            resources.ApplyResources(this.tss6, "tss6");
            this.tss6.Name = "tss6";
            // 
            // cmsSocketList_ToExcel
            // 
            resources.ApplyResources(this.cmsSocketList_ToExcel, "cmsSocketList_ToExcel");
            this.cmsSocketList_ToExcel.Image = global::WPELibrary.Properties.Resources.saveas;
            this.cmsSocketList_ToExcel.Name = "cmsSocketList_ToExcel";
            // 
            // toolStripSeparator3
            // 
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            // 
            // cmsSocketList_Comparison_A
            // 
            resources.ApplyResources(this.cmsSocketList_Comparison_A, "cmsSocketList_Comparison_A");
            this.cmsSocketList_Comparison_A.Image = global::WPELibrary.Properties.Resources.A;
            this.cmsSocketList_Comparison_A.Name = "cmsSocketList_Comparison_A";
            // 
            // cmsSocketList_Comparison_B
            // 
            resources.ApplyResources(this.cmsSocketList_Comparison_B, "cmsSocketList_Comparison_B");
            this.cmsSocketList_Comparison_B.Image = global::WPELibrary.Properties.Resources.B;
            this.cmsSocketList_Comparison_B.Name = "cmsSocketList_Comparison_B";
            // 
            // tlpParameter
            // 
            resources.ApplyResources(this.tlpParameter, "tlpParameter");
            this.tlpParameter.Controls.Add(this.gbHookButton_Search, 1, 0);
            this.tlpParameter.Controls.Add(this.tlpHookButton, 2, 0);
            this.tlpParameter.Controls.Add(this.tcSocketInfo, 0, 0);
            this.tlpParameter.Name = "tlpParameter";
            // 
            // gbHookButton_Search
            // 
            resources.ApplyResources(this.gbHookButton_Search, "gbHookButton_Search");
            this.gbHookButton_Search.Controls.Add(this.tlpSearch);
            this.gbHookButton_Search.Name = "gbHookButton_Search";
            this.gbHookButton_Search.TabStop = false;
            // 
            // tlpSearch
            // 
            resources.ApplyResources(this.tlpSearch, "tlpSearch");
            this.tlpSearch.Controls.Add(this.rbFromIndex, 0, 1);
            this.tlpSearch.Controls.Add(this.rbFromHead, 0, 0);
            this.tlpSearch.Controls.Add(this.tlpSearchButton, 0, 2);
            this.tlpSearch.Name = "tlpSearch";
            // 
            // rbFromIndex
            // 
            resources.ApplyResources(this.rbFromIndex, "rbFromIndex");
            this.rbFromIndex.Name = "rbFromIndex";
            this.rbFromIndex.UseVisualStyleBackColor = true;
            // 
            // rbFromHead
            // 
            resources.ApplyResources(this.rbFromHead, "rbFromHead");
            this.rbFromHead.Checked = true;
            this.rbFromHead.Name = "rbFromHead";
            this.rbFromHead.TabStop = true;
            this.rbFromHead.UseVisualStyleBackColor = true;
            // 
            // tlpSearchButton
            // 
            resources.ApplyResources(this.tlpSearchButton, "tlpSearchButton");
            this.tlpSearchButton.Controls.Add(this.bSearchNext, 2, 0);
            this.tlpSearchButton.Controls.Add(this.bSearch, 0, 0);
            this.tlpSearchButton.Name = "tlpSearchButton";
            // 
            // bSearchNext
            // 
            resources.ApplyResources(this.bSearchNext, "bSearchNext");
            this.bSearchNext.FlatAppearance.BorderSize = 0;
            this.bSearchNext.Image = global::WPELibrary.Properties.Resources.Search16;
            this.bSearchNext.Name = "bSearchNext";
            this.bSearchNext.UseVisualStyleBackColor = true;
            this.bSearchNext.Click += new System.EventHandler(this.bSearchNext_Click);
            // 
            // bSearch
            // 
            resources.ApplyResources(this.bSearch, "bSearch");
            this.bSearch.FlatAppearance.BorderSize = 0;
            this.bSearch.Image = global::WPELibrary.Properties.Resources.Settings;
            this.bSearch.Name = "bSearch";
            this.bSearch.UseVisualStyleBackColor = true;
            this.bSearch.Click += new System.EventHandler(this.bSearch_Click);
            // 
            // tlpHookButton
            // 
            resources.ApplyResources(this.tlpHookButton, "tlpHookButton");
            this.tlpHookButton.Controls.Add(this.tlpHookButton_Start, 3, 1);
            this.tlpHookButton.Controls.Add(this.bCleanUp, 1, 1);
            this.tlpHookButton.Name = "tlpHookButton";
            // 
            // tlpHookButton_Start
            // 
            resources.ApplyResources(this.tlpHookButton_Start, "tlpHookButton_Start");
            this.tlpHookButton_Start.Controls.Add(this.bStopHook, 0, 2);
            this.tlpHookButton_Start.Controls.Add(this.bStartHook, 0, 0);
            this.tlpHookButton_Start.Name = "tlpHookButton_Start";
            // 
            // bStopHook
            // 
            resources.ApplyResources(this.bStopHook, "bStopHook");
            this.bStopHook.Image = global::WPELibrary.Properties.Resources.Stop16;
            this.bStopHook.Name = "bStopHook";
            this.bStopHook.UseVisualStyleBackColor = true;
            this.bStopHook.Click += new System.EventHandler(this.bStopHook_Click);
            // 
            // bStartHook
            // 
            resources.ApplyResources(this.bStartHook, "bStartHook");
            this.bStartHook.Image = global::WPELibrary.Properties.Resources.Play16;
            this.bStartHook.Name = "bStartHook";
            this.bStartHook.UseVisualStyleBackColor = true;
            this.bStartHook.Click += new System.EventHandler(this.bStartHook_Click);
            // 
            // bCleanUp
            // 
            resources.ApplyResources(this.bCleanUp, "bCleanUp");
            this.bCleanUp.Name = "bCleanUp";
            this.bCleanUp.UseVisualStyleBackColor = true;
            this.bCleanUp.Click += new System.EventHandler(this.bCleanUp_Click);
            // 
            // tcSocketInfo
            // 
            resources.ApplyResources(this.tcSocketInfo, "tcSocketInfo");
            this.tcSocketInfo.Controls.Add(this.tcSocketInfo_FilterSet);
            this.tcSocketInfo.Controls.Add(this.tcSocketInfo_HookSet);
            this.tcSocketInfo.Controls.Add(this.tcSocketInfo_ListSet);
            this.tcSocketInfo.Controls.Add(this.tcSocketInfo_HotKey);
            this.tcSocketInfo.Controls.Add(this.tcSocketInfo_SystemSet);
            this.tcSocketInfo.Name = "tcSocketInfo";
            this.tcSocketInfo.SelectedIndex = 0;
            // 
            // tcSocketInfo_FilterSet
            // 
            resources.ApplyResources(this.tcSocketInfo_FilterSet, "tcSocketInfo_FilterSet");
            this.tcSocketInfo_FilterSet.BackColor = System.Drawing.SystemColors.Control;
            this.tcSocketInfo_FilterSet.Controls.Add(this.tlpFilterSet);
            this.tcSocketInfo_FilterSet.Name = "tcSocketInfo_FilterSet";
            // 
            // tlpFilterSet
            // 
            resources.ApplyResources(this.tlpFilterSet, "tlpFilterSet");
            this.tlpFilterSet.Controls.Add(this.txtCheckLength, 1, 2);
            this.tlpFilterSet.Controls.Add(this.txtCheckHead, 7, 1);
            this.tlpFilterSet.Controls.Add(this.cbCheckHead, 6, 1);
            this.tlpFilterSet.Controls.Add(this.cbCheckSize, 0, 2);
            this.tlpFilterSet.Controls.Add(this.rbFilter_Show, 1, 0);
            this.tlpFilterSet.Controls.Add(this.rbFilter_NotShow, 0, 0);
            this.tlpFilterSet.Controls.Add(this.txtCheckPort, 4, 2);
            this.tlpFilterSet.Controls.Add(this.cbCheckPort, 3, 2);
            this.tlpFilterSet.Controls.Add(this.txtCheckData, 7, 2);
            this.tlpFilterSet.Controls.Add(this.cbCheckData, 6, 2);
            this.tlpFilterSet.Controls.Add(this.cbCheckSocket, 0, 1);
            this.tlpFilterSet.Controls.Add(this.cbCheckIP, 3, 1);
            this.tlpFilterSet.Controls.Add(this.txtCheckSocket, 1, 1);
            this.tlpFilterSet.Controls.Add(this.txtCheckIP, 4, 1);
            this.tlpFilterSet.Name = "tlpFilterSet";
            // 
            // txtCheckLength
            // 
            resources.ApplyResources(this.txtCheckLength, "txtCheckLength");
            this.txtCheckLength.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCheckLength.Name = "txtCheckLength";
            this.txtCheckLength.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCheckLength_KeyPress);
            // 
            // txtCheckHead
            // 
            resources.ApplyResources(this.txtCheckHead, "txtCheckHead");
            this.txtCheckHead.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCheckHead.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCheckHead.Name = "txtCheckHead";
            this.txtCheckHead.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCheckHead_KeyPress);
            // 
            // cbCheckHead
            // 
            resources.ApplyResources(this.cbCheckHead, "cbCheckHead");
            this.cbCheckHead.Name = "cbCheckHead";
            this.cbCheckHead.UseVisualStyleBackColor = true;
            // 
            // cbCheckSize
            // 
            resources.ApplyResources(this.cbCheckSize, "cbCheckSize");
            this.cbCheckSize.Name = "cbCheckSize";
            this.cbCheckSize.UseVisualStyleBackColor = true;
            // 
            // rbFilter_Show
            // 
            resources.ApplyResources(this.rbFilter_Show, "rbFilter_Show");
            this.rbFilter_Show.Name = "rbFilter_Show";
            this.rbFilter_Show.UseVisualStyleBackColor = true;
            // 
            // rbFilter_NotShow
            // 
            resources.ApplyResources(this.rbFilter_NotShow, "rbFilter_NotShow");
            this.rbFilter_NotShow.Checked = true;
            this.rbFilter_NotShow.Name = "rbFilter_NotShow";
            this.rbFilter_NotShow.TabStop = true;
            this.rbFilter_NotShow.UseVisualStyleBackColor = true;
            // 
            // txtCheckPort
            // 
            resources.ApplyResources(this.txtCheckPort, "txtCheckPort");
            this.txtCheckPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCheckPort.Name = "txtCheckPort";
            this.txtCheckPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCheckPort_KeyPress);
            // 
            // cbCheckPort
            // 
            resources.ApplyResources(this.cbCheckPort, "cbCheckPort");
            this.cbCheckPort.Name = "cbCheckPort";
            this.cbCheckPort.UseVisualStyleBackColor = true;
            // 
            // txtCheckData
            // 
            resources.ApplyResources(this.txtCheckData, "txtCheckData");
            this.txtCheckData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCheckData.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtCheckData.Name = "txtCheckData";
            this.txtCheckData.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCheckPacket_KeyPress);
            // 
            // cbCheckData
            // 
            resources.ApplyResources(this.cbCheckData, "cbCheckData");
            this.cbCheckData.Name = "cbCheckData";
            this.cbCheckData.UseVisualStyleBackColor = true;
            // 
            // cbCheckSocket
            // 
            resources.ApplyResources(this.cbCheckSocket, "cbCheckSocket");
            this.cbCheckSocket.Name = "cbCheckSocket";
            this.cbCheckSocket.UseVisualStyleBackColor = true;
            // 
            // cbCheckIP
            // 
            resources.ApplyResources(this.cbCheckIP, "cbCheckIP");
            this.cbCheckIP.Name = "cbCheckIP";
            this.cbCheckIP.UseVisualStyleBackColor = true;
            // 
            // txtCheckSocket
            // 
            resources.ApplyResources(this.txtCheckSocket, "txtCheckSocket");
            this.txtCheckSocket.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCheckSocket.Name = "txtCheckSocket";
            this.txtCheckSocket.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCheckSocket_KeyPress);
            // 
            // txtCheckIP
            // 
            resources.ApplyResources(this.txtCheckIP, "txtCheckIP");
            this.txtCheckIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCheckIP.Name = "txtCheckIP";
            this.txtCheckIP.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtCheckIP_KeyPress);
            // 
            // tcSocketInfo_HookSet
            // 
            resources.ApplyResources(this.tcSocketInfo_HookSet, "tcSocketInfo_HookSet");
            this.tcSocketInfo_HookSet.BackColor = System.Drawing.SystemColors.Control;
            this.tcSocketInfo_HookSet.Controls.Add(this.tlpHookSet);
            this.tcSocketInfo_HookSet.Name = "tcSocketInfo_HookSet";
            // 
            // tlpHookSet
            // 
            resources.ApplyResources(this.tlpHookSet, "tlpHookSet");
            this.tlpHookSet.Controls.Add(this.gbHookSet_WinsockWSA, 1, 0);
            this.tlpHookSet.Controls.Add(this.gbHookSet_Winsock, 0, 0);
            this.tlpHookSet.Name = "tlpHookSet";
            // 
            // gbHookSet_WinsockWSA
            // 
            resources.ApplyResources(this.gbHookSet_WinsockWSA, "gbHookSet_WinsockWSA");
            this.gbHookSet_WinsockWSA.Controls.Add(this.tlpHookSet_WinsockWSA);
            this.gbHookSet_WinsockWSA.Name = "gbHookSet_WinsockWSA";
            this.gbHookSet_WinsockWSA.TabStop = false;
            // 
            // tlpHookSet_WinsockWSA
            // 
            resources.ApplyResources(this.tlpHookSet_WinsockWSA, "tlpHookSet_WinsockWSA");
            this.tlpHookSet_WinsockWSA.Controls.Add(this.cbHookWS2_RecvFrom, 1, 1);
            this.tlpHookSet_WinsockWSA.Controls.Add(this.cbHookWS2_Recv, 0, 1);
            this.tlpHookSet_WinsockWSA.Controls.Add(this.cbHookWS2_SendTo, 1, 0);
            this.tlpHookSet_WinsockWSA.Controls.Add(this.cbHookWS2_Send, 0, 0);
            this.tlpHookSet_WinsockWSA.Controls.Add(this.cbHookWSA_RecvFrom, 3, 1);
            this.tlpHookSet_WinsockWSA.Controls.Add(this.cbHookWSA_Recv, 2, 1);
            this.tlpHookSet_WinsockWSA.Controls.Add(this.cbHookWSA_SendTo, 3, 0);
            this.tlpHookSet_WinsockWSA.Controls.Add(this.cbHookWSA_Send, 2, 0);
            this.tlpHookSet_WinsockWSA.Name = "tlpHookSet_WinsockWSA";
            // 
            // cbHookWS2_RecvFrom
            // 
            resources.ApplyResources(this.cbHookWS2_RecvFrom, "cbHookWS2_RecvFrom");
            this.cbHookWS2_RecvFrom.Checked = true;
            this.cbHookWS2_RecvFrom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHookWS2_RecvFrom.Name = "cbHookWS2_RecvFrom";
            this.cbHookWS2_RecvFrom.UseVisualStyleBackColor = true;
            // 
            // cbHookWS2_Recv
            // 
            resources.ApplyResources(this.cbHookWS2_Recv, "cbHookWS2_Recv");
            this.cbHookWS2_Recv.Checked = true;
            this.cbHookWS2_Recv.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHookWS2_Recv.Name = "cbHookWS2_Recv";
            this.cbHookWS2_Recv.UseVisualStyleBackColor = true;
            // 
            // cbHookWS2_SendTo
            // 
            resources.ApplyResources(this.cbHookWS2_SendTo, "cbHookWS2_SendTo");
            this.cbHookWS2_SendTo.Checked = true;
            this.cbHookWS2_SendTo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHookWS2_SendTo.Name = "cbHookWS2_SendTo";
            this.cbHookWS2_SendTo.UseVisualStyleBackColor = true;
            // 
            // cbHookWS2_Send
            // 
            resources.ApplyResources(this.cbHookWS2_Send, "cbHookWS2_Send");
            this.cbHookWS2_Send.Checked = true;
            this.cbHookWS2_Send.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHookWS2_Send.Name = "cbHookWS2_Send";
            this.cbHookWS2_Send.UseVisualStyleBackColor = true;
            // 
            // cbHookWSA_RecvFrom
            // 
            resources.ApplyResources(this.cbHookWSA_RecvFrom, "cbHookWSA_RecvFrom");
            this.cbHookWSA_RecvFrom.Checked = true;
            this.cbHookWSA_RecvFrom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHookWSA_RecvFrom.Name = "cbHookWSA_RecvFrom";
            this.cbHookWSA_RecvFrom.UseVisualStyleBackColor = true;
            // 
            // cbHookWSA_Recv
            // 
            resources.ApplyResources(this.cbHookWSA_Recv, "cbHookWSA_Recv");
            this.cbHookWSA_Recv.Checked = true;
            this.cbHookWSA_Recv.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHookWSA_Recv.Name = "cbHookWSA_Recv";
            this.cbHookWSA_Recv.UseVisualStyleBackColor = true;
            // 
            // cbHookWSA_SendTo
            // 
            resources.ApplyResources(this.cbHookWSA_SendTo, "cbHookWSA_SendTo");
            this.cbHookWSA_SendTo.Checked = true;
            this.cbHookWSA_SendTo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHookWSA_SendTo.Name = "cbHookWSA_SendTo";
            this.cbHookWSA_SendTo.UseVisualStyleBackColor = true;
            // 
            // cbHookWSA_Send
            // 
            resources.ApplyResources(this.cbHookWSA_Send, "cbHookWSA_Send");
            this.cbHookWSA_Send.Checked = true;
            this.cbHookWSA_Send.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHookWSA_Send.Name = "cbHookWSA_Send";
            this.cbHookWSA_Send.UseVisualStyleBackColor = true;
            // 
            // gbHookSet_Winsock
            // 
            resources.ApplyResources(this.gbHookSet_Winsock, "gbHookSet_Winsock");
            this.gbHookSet_Winsock.Controls.Add(this.tlpHookSet_Winsock);
            this.gbHookSet_Winsock.Name = "gbHookSet_Winsock";
            this.gbHookSet_Winsock.TabStop = false;
            // 
            // tlpHookSet_Winsock
            // 
            resources.ApplyResources(this.tlpHookSet_Winsock, "tlpHookSet_Winsock");
            this.tlpHookSet_Winsock.Controls.Add(this.cbHookWS1_RecvFrom, 1, 1);
            this.tlpHookSet_Winsock.Controls.Add(this.cbHookWS1_Recv, 0, 1);
            this.tlpHookSet_Winsock.Controls.Add(this.cbHookWS1_SendTo, 1, 0);
            this.tlpHookSet_Winsock.Controls.Add(this.cbHookWS1_Send, 0, 0);
            this.tlpHookSet_Winsock.Name = "tlpHookSet_Winsock";
            // 
            // cbHookWS1_RecvFrom
            // 
            resources.ApplyResources(this.cbHookWS1_RecvFrom, "cbHookWS1_RecvFrom");
            this.cbHookWS1_RecvFrom.Checked = true;
            this.cbHookWS1_RecvFrom.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHookWS1_RecvFrom.Name = "cbHookWS1_RecvFrom";
            this.cbHookWS1_RecvFrom.UseVisualStyleBackColor = true;
            // 
            // cbHookWS1_Recv
            // 
            resources.ApplyResources(this.cbHookWS1_Recv, "cbHookWS1_Recv");
            this.cbHookWS1_Recv.Checked = true;
            this.cbHookWS1_Recv.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHookWS1_Recv.Name = "cbHookWS1_Recv";
            this.cbHookWS1_Recv.UseVisualStyleBackColor = true;
            // 
            // cbHookWS1_SendTo
            // 
            resources.ApplyResources(this.cbHookWS1_SendTo, "cbHookWS1_SendTo");
            this.cbHookWS1_SendTo.Checked = true;
            this.cbHookWS1_SendTo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHookWS1_SendTo.Name = "cbHookWS1_SendTo";
            this.cbHookWS1_SendTo.UseVisualStyleBackColor = true;
            // 
            // cbHookWS1_Send
            // 
            resources.ApplyResources(this.cbHookWS1_Send, "cbHookWS1_Send");
            this.cbHookWS1_Send.Checked = true;
            this.cbHookWS1_Send.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbHookWS1_Send.Name = "cbHookWS1_Send";
            this.cbHookWS1_Send.UseVisualStyleBackColor = true;
            // 
            // tcSocketInfo_ListSet
            // 
            resources.ApplyResources(this.tcSocketInfo_ListSet, "tcSocketInfo_ListSet");
            this.tcSocketInfo_ListSet.BackColor = System.Drawing.SystemColors.Control;
            this.tcSocketInfo_ListSet.Controls.Add(this.tlpListSet);
            this.tcSocketInfo_ListSet.Name = "tcSocketInfo_ListSet";
            // 
            // tlpListSet
            // 
            resources.ApplyResources(this.tlpListSet, "tlpListSet");
            this.tlpListSet.Controls.Add(this.gbListSet_LogList, 1, 0);
            this.tlpListSet.Controls.Add(this.gbListSet_SocketList, 0, 0);
            this.tlpListSet.Name = "tlpListSet";
            // 
            // gbListSet_LogList
            // 
            resources.ApplyResources(this.gbListSet_LogList, "gbListSet_LogList");
            this.gbListSet_LogList.Controls.Add(this.tlpListSet_LogList);
            this.gbListSet_LogList.Name = "gbListSet_LogList";
            this.gbListSet_LogList.TabStop = false;
            // 
            // tlpListSet_LogList
            // 
            resources.ApplyResources(this.tlpListSet_LogList, "tlpListSet_LogList");
            this.tlpListSet_LogList.Controls.Add(this.nudLogList_AutoClearValue, 1, 1);
            this.tlpListSet_LogList.Controls.Add(this.cbLogList_AutoClear, 0, 1);
            this.tlpListSet_LogList.Controls.Add(this.cbLogList_AutoRoll, 0, 0);
            this.tlpListSet_LogList.Name = "tlpListSet_LogList";
            // 
            // nudLogList_AutoClearValue
            // 
            resources.ApplyResources(this.nudLogList_AutoClearValue, "nudLogList_AutoClearValue");
            this.nudLogList_AutoClearValue.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudLogList_AutoClearValue.Name = "nudLogList_AutoClearValue";
            this.nudLogList_AutoClearValue.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // cbLogList_AutoClear
            // 
            resources.ApplyResources(this.cbLogList_AutoClear, "cbLogList_AutoClear");
            this.cbLogList_AutoClear.Name = "cbLogList_AutoClear";
            this.cbLogList_AutoClear.UseVisualStyleBackColor = true;
            this.cbLogList_AutoClear.CheckedChanged += new System.EventHandler(this.cbLogList_AutoClear_CheckedChanged);
            // 
            // cbLogList_AutoRoll
            // 
            resources.ApplyResources(this.cbLogList_AutoRoll, "cbLogList_AutoRoll");
            this.cbLogList_AutoRoll.Name = "cbLogList_AutoRoll";
            this.cbLogList_AutoRoll.UseVisualStyleBackColor = true;
            // 
            // gbListSet_SocketList
            // 
            resources.ApplyResources(this.gbListSet_SocketList, "gbListSet_SocketList");
            this.gbListSet_SocketList.Controls.Add(this.tlpListSet_SocketList);
            this.gbListSet_SocketList.Name = "gbListSet_SocketList";
            this.gbListSet_SocketList.TabStop = false;
            // 
            // tlpListSet_SocketList
            // 
            resources.ApplyResources(this.tlpListSet_SocketList, "tlpListSet_SocketList");
            this.tlpListSet_SocketList.Controls.Add(this.nudSocketList_AutoClearValue, 1, 1);
            this.tlpListSet_SocketList.Controls.Add(this.cbSocketList_AutoClear, 0, 1);
            this.tlpListSet_SocketList.Controls.Add(this.cbSocketList_AutoRoll, 0, 0);
            this.tlpListSet_SocketList.Name = "tlpListSet_SocketList";
            // 
            // nudSocketList_AutoClearValue
            // 
            resources.ApplyResources(this.nudSocketList_AutoClearValue, "nudSocketList_AutoClearValue");
            this.nudSocketList_AutoClearValue.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudSocketList_AutoClearValue.Name = "nudSocketList_AutoClearValue";
            this.nudSocketList_AutoClearValue.Value = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            // 
            // cbSocketList_AutoClear
            // 
            resources.ApplyResources(this.cbSocketList_AutoClear, "cbSocketList_AutoClear");
            this.cbSocketList_AutoClear.Name = "cbSocketList_AutoClear";
            this.cbSocketList_AutoClear.UseVisualStyleBackColor = true;
            this.cbSocketList_AutoClear.CheckedChanged += new System.EventHandler(this.cbSocketList_AutoClear_CheckedChanged);
            // 
            // cbSocketList_AutoRoll
            // 
            resources.ApplyResources(this.cbSocketList_AutoRoll, "cbSocketList_AutoRoll");
            this.cbSocketList_AutoRoll.Name = "cbSocketList_AutoRoll";
            this.cbSocketList_AutoRoll.UseVisualStyleBackColor = true;
            // 
            // tcSocketInfo_HotKey
            // 
            resources.ApplyResources(this.tcSocketInfo_HotKey, "tcSocketInfo_HotKey");
            this.tcSocketInfo_HotKey.BackColor = System.Drawing.SystemColors.Control;
            this.tcSocketInfo_HotKey.Controls.Add(this.tlpHotKeySet);
            this.tcSocketInfo_HotKey.Name = "tcSocketInfo_HotKey";
            // 
            // tlpHotKeySet
            // 
            resources.ApplyResources(this.tlpHotKeySet, "tlpHotKeySet");
            this.tlpHotKeySet.Controls.Add(this.label2, 2, 0);
            this.tlpHotKeySet.Controls.Add(this.lHotKey_RobotList, 0, 0);
            this.tlpHotKeySet.Controls.Add(this.label1, 1, 0);
            this.tlpHotKeySet.Name = "tlpHotKeySet";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // lHotKey_RobotList
            // 
            resources.ApplyResources(this.lHotKey_RobotList, "lHotKey_RobotList");
            this.lHotKey_RobotList.Name = "lHotKey_RobotList";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // tcSocketInfo_SystemSet
            // 
            resources.ApplyResources(this.tcSocketInfo_SystemSet, "tcSocketInfo_SystemSet");
            this.tcSocketInfo_SystemSet.BackColor = System.Drawing.SystemColors.Control;
            this.tcSocketInfo_SystemSet.Controls.Add(this.tlpSystemSet);
            this.tcSocketInfo_SystemSet.Name = "tcSocketInfo_SystemSet";
            // 
            // tlpSystemSet
            // 
            resources.ApplyResources(this.tlpSystemSet, "tlpSystemSet");
            this.tlpSystemSet.Controls.Add(this.gbSystemSet_ShowMode, 0, 0);
            this.tlpSystemSet.Controls.Add(this.gbSystemSet_FilterSet, 2, 0);
            this.tlpSystemSet.Controls.Add(this.gbSystemSet_WorkMode, 1, 0);
            this.tlpSystemSet.Name = "tlpSystemSet";
            // 
            // gbSystemSet_ShowMode
            // 
            resources.ApplyResources(this.gbSystemSet_ShowMode, "gbSystemSet_ShowMode");
            this.gbSystemSet_ShowMode.Controls.Add(this.tlpSystemSet_ShowMode);
            this.gbSystemSet_ShowMode.Name = "gbSystemSet_ShowMode";
            this.gbSystemSet_ShowMode.TabStop = false;
            // 
            // tlpSystemSet_ShowMode
            // 
            resources.ApplyResources(this.tlpSystemSet_ShowMode, "tlpSystemSet_ShowMode");
            this.tlpSystemSet_ShowMode.Controls.Add(this.cbTopMost, 0, 0);
            this.tlpSystemSet_ShowMode.Name = "tlpSystemSet_ShowMode";
            // 
            // cbTopMost
            // 
            resources.ApplyResources(this.cbTopMost, "cbTopMost");
            this.cbTopMost.Name = "cbTopMost";
            this.cbTopMost.UseVisualStyleBackColor = true;
            this.cbTopMost.CheckedChanged += new System.EventHandler(this.cbTopMost_CheckedChanged);
            // 
            // gbSystemSet_FilterSet
            // 
            resources.ApplyResources(this.gbSystemSet_FilterSet, "gbSystemSet_FilterSet");
            this.gbSystemSet_FilterSet.Controls.Add(this.tlpSystemSet_FilterSet);
            this.gbSystemSet_FilterSet.Name = "gbSystemSet_FilterSet";
            this.gbSystemSet_FilterSet.TabStop = false;
            // 
            // tlpSystemSet_FilterSet
            // 
            resources.ApplyResources(this.tlpSystemSet_FilterSet, "tlpSystemSet_FilterSet");
            this.tlpSystemSet_FilterSet.Controls.Add(this.rbFilterSet_Sequence, 0, 1);
            this.tlpSystemSet_FilterSet.Controls.Add(this.rbFilterSet_Priority, 0, 0);
            this.tlpSystemSet_FilterSet.Name = "tlpSystemSet_FilterSet";
            // 
            // rbFilterSet_Sequence
            // 
            resources.ApplyResources(this.rbFilterSet_Sequence, "rbFilterSet_Sequence");
            this.rbFilterSet_Sequence.Name = "rbFilterSet_Sequence";
            this.rbFilterSet_Sequence.UseVisualStyleBackColor = true;
            // 
            // rbFilterSet_Priority
            // 
            resources.ApplyResources(this.rbFilterSet_Priority, "rbFilterSet_Priority");
            this.rbFilterSet_Priority.Checked = true;
            this.rbFilterSet_Priority.Name = "rbFilterSet_Priority";
            this.rbFilterSet_Priority.TabStop = true;
            this.rbFilterSet_Priority.UseVisualStyleBackColor = true;
            // 
            // gbSystemSet_WorkMode
            // 
            resources.ApplyResources(this.gbSystemSet_WorkMode, "gbSystemSet_WorkMode");
            this.gbSystemSet_WorkMode.Controls.Add(this.tlpSystemSet_WorkMode);
            this.gbSystemSet_WorkMode.Name = "gbSystemSet_WorkMode";
            this.gbSystemSet_WorkMode.TabStop = false;
            // 
            // tlpSystemSet_WorkMode
            // 
            resources.ApplyResources(this.tlpSystemSet_WorkMode, "tlpSystemSet_WorkMode");
            this.tlpSystemSet_WorkMode.Controls.Add(this.cbWorkingMode_Speed, 0, 0);
            this.tlpSystemSet_WorkMode.Name = "tlpSystemSet_WorkMode";
            // 
            // cbWorkingMode_Speed
            // 
            resources.ApplyResources(this.cbWorkingMode_Speed, "cbWorkingMode_Speed");
            this.cbWorkingMode_Speed.Name = "cbWorkingMode_Speed";
            this.cbWorkingMode_Speed.UseVisualStyleBackColor = true;
            // 
            // tlpInformation
            // 
            resources.ApplyResources(this.tlpInformation, "tlpInformation");
            this.tlpInformation.Controls.Add(this.tlpPacketInfo, 1, 0);
            this.tlpInformation.Controls.Add(this.tcAutomation, 0, 0);
            this.tlpInformation.Name = "tlpInformation";
            // 
            // tlpPacketInfo
            // 
            resources.ApplyResources(this.tlpPacketInfo, "tlpPacketInfo");
            this.tlpPacketInfo.Controls.Add(this.tcPacketInfo, 0, 0);
            this.tlpPacketInfo.Name = "tlpPacketInfo";
            // 
            // tcPacketInfo
            // 
            resources.ApplyResources(this.tcPacketInfo, "tcPacketInfo");
            this.tcPacketInfo.Controls.Add(this.tpPacketData);
            this.tcPacketInfo.Controls.Add(this.tpComparison);
            this.tcPacketInfo.Controls.Add(this.tpXOR);
            this.tcPacketInfo.Controls.Add(this.tpEncoding);
            this.tcPacketInfo.Controls.Add(this.tpExtraction);
            this.tcPacketInfo.Controls.Add(this.tpSystemLog);
            this.tcPacketInfo.Multiline = true;
            this.tcPacketInfo.Name = "tcPacketInfo";
            this.tcPacketInfo.SelectedIndex = 0;
            // 
            // tpPacketData
            // 
            resources.ApplyResources(this.tpPacketData, "tpPacketData");
            this.tpPacketData.Controls.Add(this.tlpPacketData);
            this.tpPacketData.Name = "tpPacketData";
            this.tpPacketData.UseVisualStyleBackColor = true;
            // 
            // tlpPacketData
            // 
            resources.ApplyResources(this.tlpPacketData, "tlpPacketData");
            this.tlpPacketData.BackColor = System.Drawing.SystemColors.Control;
            this.tlpPacketData.Controls.Add(this.tlpHexBox, 0, 0);
            this.tlpPacketData.Name = "tlpPacketData";
            // 
            // tlpHexBox
            // 
            resources.ApplyResources(this.tlpHexBox, "tlpHexBox");
            this.tlpHexBox.Controls.Add(this.hbPacketData, 0, 0);
            this.tlpHexBox.Name = "tlpHexBox";
            // 
            // hbPacketData
            // 
            resources.ApplyResources(this.hbPacketData, "hbPacketData");
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
            this.hbPacketData.BuiltInContextMenu.SelectAllMenuItemImage = global::WPELibrary.Properties.Resources.SelectAll;
            this.hbPacketData.BuiltInContextMenu.SelectAllMenuItemText = resources.GetString("hbPacketData.BuiltInContextMenu.SelectAllMenuItemText");
            this.hbPacketData.ColumnInfoVisible = true;
            this.hbPacketData.ContextMenuStrip = this.cmsHexBox;
            this.hbPacketData.LineInfoVisible = true;
            this.hbPacketData.Name = "hbPacketData";
            this.hbPacketData.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hbPacketData.StringViewVisible = true;
            this.hbPacketData.VScrollBarVisible = true;
            // 
            // cmsHexBox
            // 
            resources.ApplyResources(this.cmsHexBox, "cmsHexBox");
            this.cmsHexBox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsHexBox_Send,
            this.cmsHexBox_tss1,
            this.cmsHexBox_SendList,
            this.cmsHexBox_FilterList,
            this.cmsHexBox_tss3,
            this.cmsHexBox_CopyHex,
            this.cmsHexBox_CopyText,
            this.cmsHexBox_tss4,
            this.cmsHexBox_Comparison_A,
            this.cmsHexBox_Comparison_B,
            this.cmsHexBox_tss5,
            this.cmsHexBox_SelectAll});
            this.cmsHexBox.Name = "cmsHexBox";
            this.cmsHexBox.Opening += new System.ComponentModel.CancelEventHandler(this.cmsHexBox_Opening);
            this.cmsHexBox.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsHexBox_ItemClicked);
            // 
            // cmsHexBox_Send
            // 
            resources.ApplyResources(this.cmsHexBox_Send, "cmsHexBox_Send");
            this.cmsHexBox_Send.Image = global::WPELibrary.Properties.Resources.sent;
            this.cmsHexBox_Send.Name = "cmsHexBox_Send";
            // 
            // cmsHexBox_tss1
            // 
            resources.ApplyResources(this.cmsHexBox_tss1, "cmsHexBox_tss1");
            this.cmsHexBox_tss1.Name = "cmsHexBox_tss1";
            // 
            // cmsHexBox_SendList
            // 
            resources.ApplyResources(this.cmsHexBox_SendList, "cmsHexBox_SendList");
            this.cmsHexBox_SendList.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsHexBox_tscbSendList});
            this.cmsHexBox_SendList.Image = global::WPELibrary.Properties.Resources.addto;
            this.cmsHexBox_SendList.Name = "cmsHexBox_SendList";
            // 
            // cmsHexBox_tscbSendList
            // 
            resources.ApplyResources(this.cmsHexBox_tscbSendList, "cmsHexBox_tscbSendList");
            this.cmsHexBox_tscbSendList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmsHexBox_tscbSendList.Name = "cmsHexBox_tscbSendList";
            this.cmsHexBox_tscbSendList.SelectedIndexChanged += new System.EventHandler(this.cmsHexBox_tscbSendList_SelectedIndexChanged);
            // 
            // cmsHexBox_FilterList
            // 
            resources.ApplyResources(this.cmsHexBox_FilterList, "cmsHexBox_FilterList");
            this.cmsHexBox_FilterList.Image = global::WPELibrary.Properties.Resources.addto;
            this.cmsHexBox_FilterList.Name = "cmsHexBox_FilterList";
            // 
            // cmsHexBox_tss3
            // 
            resources.ApplyResources(this.cmsHexBox_tss3, "cmsHexBox_tss3");
            this.cmsHexBox_tss3.Name = "cmsHexBox_tss3";
            // 
            // cmsHexBox_CopyHex
            // 
            resources.ApplyResources(this.cmsHexBox_CopyHex, "cmsHexBox_CopyHex");
            this.cmsHexBox_CopyHex.Image = global::WPELibrary.Properties.Resources.copy;
            this.cmsHexBox_CopyHex.Name = "cmsHexBox_CopyHex";
            // 
            // cmsHexBox_CopyText
            // 
            resources.ApplyResources(this.cmsHexBox_CopyText, "cmsHexBox_CopyText");
            this.cmsHexBox_CopyText.Image = global::WPELibrary.Properties.Resources.copy;
            this.cmsHexBox_CopyText.Name = "cmsHexBox_CopyText";
            // 
            // cmsHexBox_tss4
            // 
            resources.ApplyResources(this.cmsHexBox_tss4, "cmsHexBox_tss4");
            this.cmsHexBox_tss4.Name = "cmsHexBox_tss4";
            // 
            // cmsHexBox_Comparison_A
            // 
            resources.ApplyResources(this.cmsHexBox_Comparison_A, "cmsHexBox_Comparison_A");
            this.cmsHexBox_Comparison_A.Image = global::WPELibrary.Properties.Resources.A;
            this.cmsHexBox_Comparison_A.Name = "cmsHexBox_Comparison_A";
            // 
            // cmsHexBox_Comparison_B
            // 
            resources.ApplyResources(this.cmsHexBox_Comparison_B, "cmsHexBox_Comparison_B");
            this.cmsHexBox_Comparison_B.Image = global::WPELibrary.Properties.Resources.B;
            this.cmsHexBox_Comparison_B.Name = "cmsHexBox_Comparison_B";
            // 
            // cmsHexBox_tss5
            // 
            resources.ApplyResources(this.cmsHexBox_tss5, "cmsHexBox_tss5");
            this.cmsHexBox_tss5.Name = "cmsHexBox_tss5";
            // 
            // cmsHexBox_SelectAll
            // 
            resources.ApplyResources(this.cmsHexBox_SelectAll, "cmsHexBox_SelectAll");
            this.cmsHexBox_SelectAll.Image = global::WPELibrary.Properties.Resources.SelectAll;
            this.cmsHexBox_SelectAll.Name = "cmsHexBox_SelectAll";
            // 
            // tpComparison
            // 
            resources.ApplyResources(this.tpComparison, "tpComparison");
            this.tpComparison.Controls.Add(this.tlpComparison);
            this.tpComparison.Name = "tpComparison";
            this.tpComparison.UseVisualStyleBackColor = true;
            // 
            // tlpComparison
            // 
            resources.ApplyResources(this.tlpComparison, "tlpComparison");
            this.tlpComparison.BackColor = System.Drawing.SystemColors.Control;
            this.tlpComparison.Controls.Add(this.tlpComparison_Button, 2, 0);
            this.tlpComparison.Controls.Add(this.lComparison_B, 1, 0);
            this.tlpComparison.Controls.Add(this.lComparison_A, 0, 0);
            this.tlpComparison.Controls.Add(this.pComparison_A, 0, 1);
            this.tlpComparison.Controls.Add(this.pComparison_B, 1, 1);
            this.tlpComparison.Controls.Add(this.pComparison_Result, 2, 1);
            this.tlpComparison.Name = "tlpComparison";
            // 
            // tlpComparison_Button
            // 
            resources.ApplyResources(this.tlpComparison_Button, "tlpComparison_Button");
            this.tlpComparison_Button.BackColor = System.Drawing.SystemColors.Control;
            this.tlpComparison_Button.Controls.Add(this.bComparison_Exchange, 5, 0);
            this.tlpComparison_Button.Controls.Add(this.bComparison_Clear, 1, 0);
            this.tlpComparison_Button.Controls.Add(this.bComparison, 3, 0);
            this.tlpComparison_Button.Name = "tlpComparison_Button";
            // 
            // bComparison_Exchange
            // 
            resources.ApplyResources(this.bComparison_Exchange, "bComparison_Exchange");
            this.bComparison_Exchange.Name = "bComparison_Exchange";
            this.bComparison_Exchange.UseVisualStyleBackColor = true;
            this.bComparison_Exchange.Click += new System.EventHandler(this.bComparison_Exchange_Click);
            // 
            // bComparison_Clear
            // 
            resources.ApplyResources(this.bComparison_Clear, "bComparison_Clear");
            this.bComparison_Clear.Name = "bComparison_Clear";
            this.bComparison_Clear.UseVisualStyleBackColor = true;
            this.bComparison_Clear.Click += new System.EventHandler(this.bComparison_Clear_Click);
            // 
            // bComparison
            // 
            resources.ApplyResources(this.bComparison, "bComparison");
            this.bComparison.Name = "bComparison";
            this.bComparison.UseVisualStyleBackColor = true;
            this.bComparison.Click += new System.EventHandler(this.bComparison_Click);
            // 
            // lComparison_B
            // 
            resources.ApplyResources(this.lComparison_B, "lComparison_B");
            this.lComparison_B.BackColor = System.Drawing.SystemColors.Control;
            this.lComparison_B.Name = "lComparison_B";
            // 
            // lComparison_A
            // 
            resources.ApplyResources(this.lComparison_A, "lComparison_A");
            this.lComparison_A.BackColor = System.Drawing.SystemColors.Control;
            this.lComparison_A.Name = "lComparison_A";
            // 
            // pComparison_A
            // 
            resources.ApplyResources(this.pComparison_A, "pComparison_A");
            this.pComparison_A.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pComparison_A.Controls.Add(this.rtbComparison_A);
            this.pComparison_A.Name = "pComparison_A";
            // 
            // rtbComparison_A
            // 
            resources.ApplyResources(this.rtbComparison_A, "rtbComparison_A");
            this.rtbComparison_A.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbComparison_A.Name = "rtbComparison_A";
            this.rtbComparison_A.TextChanged += new System.EventHandler(this.rtbComparison_A_TextChanged);
            // 
            // pComparison_B
            // 
            resources.ApplyResources(this.pComparison_B, "pComparison_B");
            this.pComparison_B.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pComparison_B.Controls.Add(this.rtbComparison_B);
            this.pComparison_B.Name = "pComparison_B";
            // 
            // rtbComparison_B
            // 
            resources.ApplyResources(this.rtbComparison_B, "rtbComparison_B");
            this.rtbComparison_B.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbComparison_B.Name = "rtbComparison_B";
            this.rtbComparison_B.TextChanged += new System.EventHandler(this.rtbComparison_B_TextChanged);
            // 
            // pComparison_Result
            // 
            resources.ApplyResources(this.pComparison_Result, "pComparison_Result");
            this.pComparison_Result.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pComparison_Result.Controls.Add(this.rtbComparison_Result);
            this.pComparison_Result.Name = "pComparison_Result";
            // 
            // rtbComparison_Result
            // 
            resources.ApplyResources(this.rtbComparison_Result, "rtbComparison_Result");
            this.rtbComparison_Result.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbComparison_Result.Name = "rtbComparison_Result";
            // 
            // tpXOR
            // 
            resources.ApplyResources(this.tpXOR, "tpXOR");
            this.tpXOR.Controls.Add(this.tlpPacketInfo_XOR);
            this.tpXOR.Name = "tpXOR";
            this.tpXOR.UseVisualStyleBackColor = true;
            // 
            // tlpPacketInfo_XOR
            // 
            resources.ApplyResources(this.tlpPacketInfo_XOR, "tlpPacketInfo_XOR");
            this.tlpPacketInfo_XOR.BackColor = System.Drawing.SystemColors.Control;
            this.tlpPacketInfo_XOR.Controls.Add(this.hbXOR_To, 0, 2);
            this.tlpPacketInfo_XOR.Controls.Add(this.tlpPacketInfo_XOR_Button, 0, 1);
            this.tlpPacketInfo_XOR.Controls.Add(this.hbXOR_From, 0, 0);
            this.tlpPacketInfo_XOR.Name = "tlpPacketInfo_XOR";
            // 
            // hbXOR_To
            // 
            resources.ApplyResources(this.hbXOR_To, "hbXOR_To");
            this.hbXOR_To.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            // 
            // 
            // 
            this.hbXOR_To.BuiltInContextMenu.CopyMenuItemImage = global::WPELibrary.Properties.Resources.CopyHS;
            this.hbXOR_To.BuiltInContextMenu.CopyMenuItemText = resources.GetString("hbXOR_To.BuiltInContextMenu.CopyMenuItemText");
            this.hbXOR_To.BuiltInContextMenu.CutMenuItemImage = global::WPELibrary.Properties.Resources.CutHS;
            this.hbXOR_To.BuiltInContextMenu.CutMenuItemText = resources.GetString("hbXOR_To.BuiltInContextMenu.CutMenuItemText");
            this.hbXOR_To.BuiltInContextMenu.PasteMenuItemImage = global::WPELibrary.Properties.Resources.PasteHS;
            this.hbXOR_To.BuiltInContextMenu.PasteMenuItemText = resources.GetString("hbXOR_To.BuiltInContextMenu.PasteMenuItemText");
            this.hbXOR_To.BuiltInContextMenu.SelectAllMenuItemText = resources.GetString("hbXOR_To.BuiltInContextMenu.SelectAllMenuItemText");
            this.hbXOR_To.ColumnInfoVisible = true;
            this.hbXOR_To.LineInfoVisible = true;
            this.hbXOR_To.Name = "hbXOR_To";
            this.hbXOR_To.ReadOnly = true;
            this.hbXOR_To.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hbXOR_To.VScrollBarVisible = true;
            // 
            // tlpPacketInfo_XOR_Button
            // 
            resources.ApplyResources(this.tlpPacketInfo_XOR_Button, "tlpPacketInfo_XOR_Button");
            this.tlpPacketInfo_XOR_Button.Controls.Add(this.lXOR2, 3, 0);
            this.tlpPacketInfo_XOR_Button.Controls.Add(this.bXOR_Clear, 6, 0);
            this.tlpPacketInfo_XOR_Button.Controls.Add(this.bXOR, 4, 0);
            this.tlpPacketInfo_XOR_Button.Controls.Add(this.lXOR, 1, 0);
            this.tlpPacketInfo_XOR_Button.Controls.Add(this.txtXOR, 2, 0);
            this.tlpPacketInfo_XOR_Button.Name = "tlpPacketInfo_XOR_Button";
            // 
            // lXOR2
            // 
            resources.ApplyResources(this.lXOR2, "lXOR2");
            this.lXOR2.Name = "lXOR2";
            // 
            // bXOR_Clear
            // 
            resources.ApplyResources(this.bXOR_Clear, "bXOR_Clear");
            this.bXOR_Clear.Name = "bXOR_Clear";
            this.bXOR_Clear.UseVisualStyleBackColor = true;
            this.bXOR_Clear.Click += new System.EventHandler(this.bXOR_Clear_Click);
            // 
            // bXOR
            // 
            resources.ApplyResources(this.bXOR, "bXOR");
            this.bXOR.Name = "bXOR";
            this.bXOR.UseVisualStyleBackColor = true;
            this.bXOR.Click += new System.EventHandler(this.bXOR_Click);
            // 
            // lXOR
            // 
            resources.ApplyResources(this.lXOR, "lXOR");
            this.lXOR.Name = "lXOR";
            // 
            // txtXOR
            // 
            resources.ApplyResources(this.txtXOR, "txtXOR");
            this.txtXOR.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtXOR.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtXOR.Name = "txtXOR";
            this.txtXOR.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtXOR_KeyPress);
            // 
            // hbXOR_From
            // 
            resources.ApplyResources(this.hbXOR_From, "hbXOR_From");
            this.hbXOR_From.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            // 
            // 
            // 
            this.hbXOR_From.BuiltInContextMenu.CopyMenuItemImage = global::WPELibrary.Properties.Resources.CopyHS;
            this.hbXOR_From.BuiltInContextMenu.CopyMenuItemText = resources.GetString("hbXOR_From.BuiltInContextMenu.CopyMenuItemText");
            this.hbXOR_From.BuiltInContextMenu.CutMenuItemImage = global::WPELibrary.Properties.Resources.CutHS;
            this.hbXOR_From.BuiltInContextMenu.CutMenuItemText = resources.GetString("hbXOR_From.BuiltInContextMenu.CutMenuItemText");
            this.hbXOR_From.BuiltInContextMenu.PasteMenuItemImage = global::WPELibrary.Properties.Resources.PasteHS;
            this.hbXOR_From.BuiltInContextMenu.PasteMenuItemText = resources.GetString("hbXOR_From.BuiltInContextMenu.PasteMenuItemText");
            this.hbXOR_From.BuiltInContextMenu.SelectAllMenuItemText = resources.GetString("hbXOR_From.BuiltInContextMenu.SelectAllMenuItemText");
            this.hbXOR_From.ColumnInfoVisible = true;
            this.hbXOR_From.LineInfoVisible = true;
            this.hbXOR_From.Name = "hbXOR_From";
            this.hbXOR_From.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hbXOR_From.VScrollBarVisible = true;
            // 
            // tpEncoding
            // 
            resources.ApplyResources(this.tpEncoding, "tpEncoding");
            this.tpEncoding.Controls.Add(this.tlpPacketInfo_Encoding);
            this.tpEncoding.Name = "tpEncoding";
            this.tpEncoding.UseVisualStyleBackColor = true;
            // 
            // tlpPacketInfo_Encoding
            // 
            resources.ApplyResources(this.tlpPacketInfo_Encoding, "tlpPacketInfo_Encoding");
            this.tlpPacketInfo_Encoding.BackColor = System.Drawing.SystemColors.Control;
            this.tlpPacketInfo_Encoding.Controls.Add(this.tlpPacketInfo_Encoding_Button, 1, 0);
            this.tlpPacketInfo_Encoding.Controls.Add(this.tlpPacketInfo_Encoding_Result, 2, 0);
            this.tlpPacketInfo_Encoding.Controls.Add(this.pPacketInfo_Encoding, 0, 0);
            this.tlpPacketInfo_Encoding.Name = "tlpPacketInfo_Encoding";
            // 
            // tlpPacketInfo_Encoding_Button
            // 
            resources.ApplyResources(this.tlpPacketInfo_Encoding_Button, "tlpPacketInfo_Encoding_Button");
            this.tlpPacketInfo_Encoding_Button.Controls.Add(this.bPacketInfo_Decoding, 1, 3);
            this.tlpPacketInfo_Encoding_Button.Controls.Add(this.bPacketInfo_Encoding, 1, 1);
            this.tlpPacketInfo_Encoding_Button.Controls.Add(this.pbEncoding, 1, 2);
            this.tlpPacketInfo_Encoding_Button.Name = "tlpPacketInfo_Encoding_Button";
            // 
            // bPacketInfo_Decoding
            // 
            resources.ApplyResources(this.bPacketInfo_Decoding, "bPacketInfo_Decoding");
            this.bPacketInfo_Decoding.Name = "bPacketInfo_Decoding";
            this.bPacketInfo_Decoding.UseVisualStyleBackColor = true;
            this.bPacketInfo_Decoding.Click += new System.EventHandler(this.bPacketInfo_Decoding_Click);
            // 
            // bPacketInfo_Encoding
            // 
            resources.ApplyResources(this.bPacketInfo_Encoding, "bPacketInfo_Encoding");
            this.bPacketInfo_Encoding.Name = "bPacketInfo_Encoding";
            this.bPacketInfo_Encoding.UseVisualStyleBackColor = true;
            this.bPacketInfo_Encoding.Click += new System.EventHandler(this.bPacketInfo_Encoding_Click);
            // 
            // pbEncoding
            // 
            resources.ApplyResources(this.pbEncoding, "pbEncoding");
            this.pbEncoding.Image = global::WPELibrary.Properties.Resources.ForwardArrow;
            this.pbEncoding.Name = "pbEncoding";
            this.pbEncoding.TabStop = false;
            // 
            // tlpPacketInfo_Encoding_Result
            // 
            resources.ApplyResources(this.tlpPacketInfo_Encoding_Result, "tlpPacketInfo_Encoding_Result");
            this.tlpPacketInfo_Encoding_Result.Controls.Add(this.txtPacketInfo_Encoding_ANSIUnicode, 3, 5);
            this.tlpPacketInfo_Encoding_Result.Controls.Add(this.lPacketInfo_Encoding_ANSIUnicode, 2, 5);
            this.tlpPacketInfo_Encoding_Result.Controls.Add(this.txtPacketInfo_Encoding_ANSIUTF32, 3, 4);
            this.tlpPacketInfo_Encoding_Result.Controls.Add(this.lPacketInfo_Encoding_ANSIUTF32, 2, 4);
            this.tlpPacketInfo_Encoding_Result.Controls.Add(this.txtPacketInfo_Encoding_ANSIUTF16, 3, 3);
            this.tlpPacketInfo_Encoding_Result.Controls.Add(this.lPacketInfo_Encoding_ANSIASCII, 2, 3);
            this.tlpPacketInfo_Encoding_Result.Controls.Add(this.txtPacketInfo_Encoding_Unicode, 1, 5);
            this.tlpPacketInfo_Encoding_Result.Controls.Add(this.lPacketInfo_Encoding_Unicode, 0, 5);
            this.tlpPacketInfo_Encoding_Result.Controls.Add(this.txtPacketInfo_Encoding_UTF32, 1, 4);
            this.tlpPacketInfo_Encoding_Result.Controls.Add(this.lPacketInfo_Encoding_UTF32, 0, 4);
            this.tlpPacketInfo_Encoding_Result.Controls.Add(this.txtPacketInfo_Encoding_UTF16, 1, 3);
            this.tlpPacketInfo_Encoding_Result.Controls.Add(this.lPacketInfo_Encoding_ASCII, 0, 3);
            this.tlpPacketInfo_Encoding_Result.Controls.Add(this.txtPacketInfo_Encoding_ANSIbase64, 3, 6);
            this.tlpPacketInfo_Encoding_Result.Controls.Add(this.txtPacketInfo_Encoding_ANSIUTF8, 3, 2);
            this.tlpPacketInfo_Encoding_Result.Controls.Add(this.txtPacketInfo_Encoding_ANSIUTF7, 3, 1);
            this.tlpPacketInfo_Encoding_Result.Controls.Add(this.txtPacketInfo_Encoding_ANSIGBK, 3, 0);
            this.tlpPacketInfo_Encoding_Result.Controls.Add(this.lPacketInfo_Encoding_ANSIbase64, 2, 6);
            this.tlpPacketInfo_Encoding_Result.Controls.Add(this.lPacketInfo_Encoding_ANSIUTF8, 2, 2);
            this.tlpPacketInfo_Encoding_Result.Controls.Add(this.lPacketInfo_Encoding_ANSIUTF7, 2, 1);
            this.tlpPacketInfo_Encoding_Result.Controls.Add(this.lPacketInfo_Encoding_UTF7, 0, 1);
            this.tlpPacketInfo_Encoding_Result.Controls.Add(this.lPacketInfo_Encoding_ANSIGBK, 2, 0);
            this.tlpPacketInfo_Encoding_Result.Controls.Add(this.txtPacketInfo_Encoding_base64, 1, 6);
            this.tlpPacketInfo_Encoding_Result.Controls.Add(this.txtPacketInfo_Encoding_UTF8, 1, 2);
            this.tlpPacketInfo_Encoding_Result.Controls.Add(this.txtPacketInfo_Encoding_UTF7, 1, 1);
            this.tlpPacketInfo_Encoding_Result.Controls.Add(this.lPacketInfo_Encoding_base64, 0, 6);
            this.tlpPacketInfo_Encoding_Result.Controls.Add(this.lPacketInfo_Encoding_UTF8, 0, 2);
            this.tlpPacketInfo_Encoding_Result.Controls.Add(this.lPacketInfo_Encoding_Bytes, 0, 0);
            this.tlpPacketInfo_Encoding_Result.Controls.Add(this.txtPacketInfo_Encoding_Bytes, 1, 0);
            this.tlpPacketInfo_Encoding_Result.Name = "tlpPacketInfo_Encoding_Result";
            // 
            // txtPacketInfo_Encoding_ANSIUnicode
            // 
            resources.ApplyResources(this.txtPacketInfo_Encoding_ANSIUnicode, "txtPacketInfo_Encoding_ANSIUnicode");
            this.txtPacketInfo_Encoding_ANSIUnicode.BackColor = System.Drawing.SystemColors.Window;
            this.txtPacketInfo_Encoding_ANSIUnicode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPacketInfo_Encoding_ANSIUnicode.Name = "txtPacketInfo_Encoding_ANSIUnicode";
            this.txtPacketInfo_Encoding_ANSIUnicode.ReadOnly = true;
            // 
            // lPacketInfo_Encoding_ANSIUnicode
            // 
            resources.ApplyResources(this.lPacketInfo_Encoding_ANSIUnicode, "lPacketInfo_Encoding_ANSIUnicode");
            this.lPacketInfo_Encoding_ANSIUnicode.Name = "lPacketInfo_Encoding_ANSIUnicode";
            // 
            // txtPacketInfo_Encoding_ANSIUTF32
            // 
            resources.ApplyResources(this.txtPacketInfo_Encoding_ANSIUTF32, "txtPacketInfo_Encoding_ANSIUTF32");
            this.txtPacketInfo_Encoding_ANSIUTF32.BackColor = System.Drawing.SystemColors.Window;
            this.txtPacketInfo_Encoding_ANSIUTF32.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPacketInfo_Encoding_ANSIUTF32.Name = "txtPacketInfo_Encoding_ANSIUTF32";
            this.txtPacketInfo_Encoding_ANSIUTF32.ReadOnly = true;
            // 
            // lPacketInfo_Encoding_ANSIUTF32
            // 
            resources.ApplyResources(this.lPacketInfo_Encoding_ANSIUTF32, "lPacketInfo_Encoding_ANSIUTF32");
            this.lPacketInfo_Encoding_ANSIUTF32.Name = "lPacketInfo_Encoding_ANSIUTF32";
            // 
            // txtPacketInfo_Encoding_ANSIUTF16
            // 
            resources.ApplyResources(this.txtPacketInfo_Encoding_ANSIUTF16, "txtPacketInfo_Encoding_ANSIUTF16");
            this.txtPacketInfo_Encoding_ANSIUTF16.BackColor = System.Drawing.SystemColors.Window;
            this.txtPacketInfo_Encoding_ANSIUTF16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPacketInfo_Encoding_ANSIUTF16.Name = "txtPacketInfo_Encoding_ANSIUTF16";
            this.txtPacketInfo_Encoding_ANSIUTF16.ReadOnly = true;
            // 
            // lPacketInfo_Encoding_ANSIASCII
            // 
            resources.ApplyResources(this.lPacketInfo_Encoding_ANSIASCII, "lPacketInfo_Encoding_ANSIASCII");
            this.lPacketInfo_Encoding_ANSIASCII.Name = "lPacketInfo_Encoding_ANSIASCII";
            // 
            // txtPacketInfo_Encoding_Unicode
            // 
            resources.ApplyResources(this.txtPacketInfo_Encoding_Unicode, "txtPacketInfo_Encoding_Unicode");
            this.txtPacketInfo_Encoding_Unicode.BackColor = System.Drawing.SystemColors.Window;
            this.txtPacketInfo_Encoding_Unicode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPacketInfo_Encoding_Unicode.Name = "txtPacketInfo_Encoding_Unicode";
            this.txtPacketInfo_Encoding_Unicode.ReadOnly = true;
            // 
            // lPacketInfo_Encoding_Unicode
            // 
            resources.ApplyResources(this.lPacketInfo_Encoding_Unicode, "lPacketInfo_Encoding_Unicode");
            this.lPacketInfo_Encoding_Unicode.Name = "lPacketInfo_Encoding_Unicode";
            // 
            // txtPacketInfo_Encoding_UTF32
            // 
            resources.ApplyResources(this.txtPacketInfo_Encoding_UTF32, "txtPacketInfo_Encoding_UTF32");
            this.txtPacketInfo_Encoding_UTF32.BackColor = System.Drawing.SystemColors.Window;
            this.txtPacketInfo_Encoding_UTF32.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPacketInfo_Encoding_UTF32.Name = "txtPacketInfo_Encoding_UTF32";
            this.txtPacketInfo_Encoding_UTF32.ReadOnly = true;
            // 
            // lPacketInfo_Encoding_UTF32
            // 
            resources.ApplyResources(this.lPacketInfo_Encoding_UTF32, "lPacketInfo_Encoding_UTF32");
            this.lPacketInfo_Encoding_UTF32.Name = "lPacketInfo_Encoding_UTF32";
            // 
            // txtPacketInfo_Encoding_UTF16
            // 
            resources.ApplyResources(this.txtPacketInfo_Encoding_UTF16, "txtPacketInfo_Encoding_UTF16");
            this.txtPacketInfo_Encoding_UTF16.BackColor = System.Drawing.SystemColors.Window;
            this.txtPacketInfo_Encoding_UTF16.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPacketInfo_Encoding_UTF16.Name = "txtPacketInfo_Encoding_UTF16";
            this.txtPacketInfo_Encoding_UTF16.ReadOnly = true;
            // 
            // lPacketInfo_Encoding_ASCII
            // 
            resources.ApplyResources(this.lPacketInfo_Encoding_ASCII, "lPacketInfo_Encoding_ASCII");
            this.lPacketInfo_Encoding_ASCII.Name = "lPacketInfo_Encoding_ASCII";
            // 
            // txtPacketInfo_Encoding_ANSIbase64
            // 
            resources.ApplyResources(this.txtPacketInfo_Encoding_ANSIbase64, "txtPacketInfo_Encoding_ANSIbase64");
            this.txtPacketInfo_Encoding_ANSIbase64.BackColor = System.Drawing.SystemColors.Window;
            this.txtPacketInfo_Encoding_ANSIbase64.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPacketInfo_Encoding_ANSIbase64.Name = "txtPacketInfo_Encoding_ANSIbase64";
            this.txtPacketInfo_Encoding_ANSIbase64.ReadOnly = true;
            // 
            // txtPacketInfo_Encoding_ANSIUTF8
            // 
            resources.ApplyResources(this.txtPacketInfo_Encoding_ANSIUTF8, "txtPacketInfo_Encoding_ANSIUTF8");
            this.txtPacketInfo_Encoding_ANSIUTF8.BackColor = System.Drawing.SystemColors.Window;
            this.txtPacketInfo_Encoding_ANSIUTF8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPacketInfo_Encoding_ANSIUTF8.Name = "txtPacketInfo_Encoding_ANSIUTF8";
            this.txtPacketInfo_Encoding_ANSIUTF8.ReadOnly = true;
            // 
            // txtPacketInfo_Encoding_ANSIUTF7
            // 
            resources.ApplyResources(this.txtPacketInfo_Encoding_ANSIUTF7, "txtPacketInfo_Encoding_ANSIUTF7");
            this.txtPacketInfo_Encoding_ANSIUTF7.BackColor = System.Drawing.SystemColors.Window;
            this.txtPacketInfo_Encoding_ANSIUTF7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPacketInfo_Encoding_ANSIUTF7.Name = "txtPacketInfo_Encoding_ANSIUTF7";
            this.txtPacketInfo_Encoding_ANSIUTF7.ReadOnly = true;
            // 
            // txtPacketInfo_Encoding_ANSIGBK
            // 
            resources.ApplyResources(this.txtPacketInfo_Encoding_ANSIGBK, "txtPacketInfo_Encoding_ANSIGBK");
            this.txtPacketInfo_Encoding_ANSIGBK.BackColor = System.Drawing.SystemColors.Window;
            this.txtPacketInfo_Encoding_ANSIGBK.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPacketInfo_Encoding_ANSIGBK.Name = "txtPacketInfo_Encoding_ANSIGBK";
            this.txtPacketInfo_Encoding_ANSIGBK.ReadOnly = true;
            // 
            // lPacketInfo_Encoding_ANSIbase64
            // 
            resources.ApplyResources(this.lPacketInfo_Encoding_ANSIbase64, "lPacketInfo_Encoding_ANSIbase64");
            this.lPacketInfo_Encoding_ANSIbase64.Name = "lPacketInfo_Encoding_ANSIbase64";
            // 
            // lPacketInfo_Encoding_ANSIUTF8
            // 
            resources.ApplyResources(this.lPacketInfo_Encoding_ANSIUTF8, "lPacketInfo_Encoding_ANSIUTF8");
            this.lPacketInfo_Encoding_ANSIUTF8.Name = "lPacketInfo_Encoding_ANSIUTF8";
            // 
            // lPacketInfo_Encoding_ANSIUTF7
            // 
            resources.ApplyResources(this.lPacketInfo_Encoding_ANSIUTF7, "lPacketInfo_Encoding_ANSIUTF7");
            this.lPacketInfo_Encoding_ANSIUTF7.Name = "lPacketInfo_Encoding_ANSIUTF7";
            // 
            // lPacketInfo_Encoding_UTF7
            // 
            resources.ApplyResources(this.lPacketInfo_Encoding_UTF7, "lPacketInfo_Encoding_UTF7");
            this.lPacketInfo_Encoding_UTF7.Name = "lPacketInfo_Encoding_UTF7";
            // 
            // lPacketInfo_Encoding_ANSIGBK
            // 
            resources.ApplyResources(this.lPacketInfo_Encoding_ANSIGBK, "lPacketInfo_Encoding_ANSIGBK");
            this.lPacketInfo_Encoding_ANSIGBK.Name = "lPacketInfo_Encoding_ANSIGBK";
            // 
            // txtPacketInfo_Encoding_base64
            // 
            resources.ApplyResources(this.txtPacketInfo_Encoding_base64, "txtPacketInfo_Encoding_base64");
            this.txtPacketInfo_Encoding_base64.BackColor = System.Drawing.SystemColors.Window;
            this.txtPacketInfo_Encoding_base64.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPacketInfo_Encoding_base64.Name = "txtPacketInfo_Encoding_base64";
            this.txtPacketInfo_Encoding_base64.ReadOnly = true;
            // 
            // txtPacketInfo_Encoding_UTF8
            // 
            resources.ApplyResources(this.txtPacketInfo_Encoding_UTF8, "txtPacketInfo_Encoding_UTF8");
            this.txtPacketInfo_Encoding_UTF8.BackColor = System.Drawing.SystemColors.Window;
            this.txtPacketInfo_Encoding_UTF8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPacketInfo_Encoding_UTF8.Name = "txtPacketInfo_Encoding_UTF8";
            this.txtPacketInfo_Encoding_UTF8.ReadOnly = true;
            // 
            // txtPacketInfo_Encoding_UTF7
            // 
            resources.ApplyResources(this.txtPacketInfo_Encoding_UTF7, "txtPacketInfo_Encoding_UTF7");
            this.txtPacketInfo_Encoding_UTF7.BackColor = System.Drawing.SystemColors.Window;
            this.txtPacketInfo_Encoding_UTF7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPacketInfo_Encoding_UTF7.Name = "txtPacketInfo_Encoding_UTF7";
            this.txtPacketInfo_Encoding_UTF7.ReadOnly = true;
            // 
            // lPacketInfo_Encoding_base64
            // 
            resources.ApplyResources(this.lPacketInfo_Encoding_base64, "lPacketInfo_Encoding_base64");
            this.lPacketInfo_Encoding_base64.Name = "lPacketInfo_Encoding_base64";
            // 
            // lPacketInfo_Encoding_UTF8
            // 
            resources.ApplyResources(this.lPacketInfo_Encoding_UTF8, "lPacketInfo_Encoding_UTF8");
            this.lPacketInfo_Encoding_UTF8.Name = "lPacketInfo_Encoding_UTF8";
            // 
            // lPacketInfo_Encoding_Bytes
            // 
            resources.ApplyResources(this.lPacketInfo_Encoding_Bytes, "lPacketInfo_Encoding_Bytes");
            this.lPacketInfo_Encoding_Bytes.Name = "lPacketInfo_Encoding_Bytes";
            // 
            // txtPacketInfo_Encoding_Bytes
            // 
            resources.ApplyResources(this.txtPacketInfo_Encoding_Bytes, "txtPacketInfo_Encoding_Bytes");
            this.txtPacketInfo_Encoding_Bytes.BackColor = System.Drawing.SystemColors.Window;
            this.txtPacketInfo_Encoding_Bytes.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPacketInfo_Encoding_Bytes.Name = "txtPacketInfo_Encoding_Bytes";
            this.txtPacketInfo_Encoding_Bytes.ReadOnly = true;
            // 
            // pPacketInfo_Encoding
            // 
            resources.ApplyResources(this.pPacketInfo_Encoding, "pPacketInfo_Encoding");
            this.pPacketInfo_Encoding.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pPacketInfo_Encoding.Controls.Add(this.rtbPacketInfo_Encoding);
            this.pPacketInfo_Encoding.Name = "pPacketInfo_Encoding";
            // 
            // rtbPacketInfo_Encoding
            // 
            resources.ApplyResources(this.rtbPacketInfo_Encoding, "rtbPacketInfo_Encoding");
            this.rtbPacketInfo_Encoding.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbPacketInfo_Encoding.Name = "rtbPacketInfo_Encoding";
            // 
            // tpExtraction
            // 
            resources.ApplyResources(this.tpExtraction, "tpExtraction");
            this.tpExtraction.BackColor = System.Drawing.SystemColors.Control;
            this.tpExtraction.Controls.Add(this.tlpExtraction);
            this.tpExtraction.Name = "tpExtraction";
            // 
            // tlpExtraction
            // 
            resources.ApplyResources(this.tlpExtraction, "tlpExtraction");
            this.tlpExtraction.Controls.Add(this.pExtraction, 0, 1);
            this.tlpExtraction.Controls.Add(this.tlpExtraction_Button, 0, 0);
            this.tlpExtraction.Name = "tlpExtraction";
            // 
            // pExtraction
            // 
            resources.ApplyResources(this.pExtraction, "pExtraction");
            this.pExtraction.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pExtraction.Controls.Add(this.rtbExtraction);
            this.pExtraction.Name = "pExtraction";
            // 
            // rtbExtraction
            // 
            resources.ApplyResources(this.rtbExtraction, "rtbExtraction");
            this.rtbExtraction.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbExtraction.ContextMenuStrip = this.cmsExtraction;
            this.rtbExtraction.Name = "rtbExtraction";
            // 
            // cmsExtraction
            // 
            resources.ApplyResources(this.cmsExtraction, "cmsExtraction");
            this.cmsExtraction.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsExtraction_Export});
            this.cmsExtraction.Name = "cmsExtraction";
            this.cmsExtraction.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsExtraction_ItemClicked);
            // 
            // cmsExtraction_Export
            // 
            resources.ApplyResources(this.cmsExtraction_Export, "cmsExtraction_Export");
            this.cmsExtraction_Export.Image = global::WPELibrary.Properties.Resources.export;
            this.cmsExtraction_Export.Name = "cmsExtraction_Export";
            // 
            // tlpExtraction_Button
            // 
            resources.ApplyResources(this.tlpExtraction_Button, "tlpExtraction_Button");
            this.tlpExtraction_Button.Controls.Add(this.bExtraction, 1, 0);
            this.tlpExtraction_Button.Controls.Add(this.cbbExtraction, 0, 0);
            this.tlpExtraction_Button.Name = "tlpExtraction_Button";
            // 
            // bExtraction
            // 
            resources.ApplyResources(this.bExtraction, "bExtraction");
            this.bExtraction.Name = "bExtraction";
            this.bExtraction.UseVisualStyleBackColor = true;
            this.bExtraction.Click += new System.EventHandler(this.bExtraction_Click);
            // 
            // cbbExtraction
            // 
            resources.ApplyResources(this.cbbExtraction, "cbbExtraction");
            this.cbbExtraction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbExtraction.FormattingEnabled = true;
            this.cbbExtraction.Items.AddRange(new object[] {
            resources.GetString("cbbExtraction.Items"),
            resources.GetString("cbbExtraction.Items1")});
            this.cbbExtraction.Name = "cbbExtraction";
            // 
            // tpSystemLog
            // 
            resources.ApplyResources(this.tpSystemLog, "tpSystemLog");
            this.tpSystemLog.Controls.Add(this.dgvLogList);
            this.tpSystemLog.Name = "tpSystemLog";
            this.tpSystemLog.UseVisualStyleBackColor = true;
            // 
            // dgvLogList
            // 
            resources.ApplyResources(this.dgvLogList, "dgvLogList");
            this.dgvLogList.AllowUserToAddRows = false;
            this.dgvLogList.AllowUserToDeleteRows = false;
            this.dgvLogList.AllowUserToResizeColumns = false;
            this.dgvLogList.AllowUserToResizeRows = false;
            this.dgvLogList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvLogList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvLogList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvLogList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvLogList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvLogList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLogList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cLogID,
            this.cLogTime,
            this.cFuncName,
            this.cLogContent});
            this.dgvLogList.ContextMenuStrip = this.cmsLogList;
            this.dgvLogList.MultiSelect = false;
            this.dgvLogList.Name = "dgvLogList";
            this.dgvLogList.ReadOnly = true;
            this.dgvLogList.RowHeadersVisible = false;
            this.dgvLogList.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvLogList.RowTemplate.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(3);
            this.dgvLogList.RowTemplate.Height = 23;
            this.dgvLogList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLogList.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvLogList_CellFormatting);
            // 
            // cLogID
            // 
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cLogID.DefaultCellStyle = dataGridViewCellStyle9;
            resources.ApplyResources(this.cLogID, "cLogID");
            this.cLogID.Name = "cLogID";
            this.cLogID.ReadOnly = true;
            // 
            // cLogTime
            // 
            this.cLogTime.DataPropertyName = "LogTime";
            dataGridViewCellStyle27.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cLogTime.DefaultCellStyle = dataGridViewCellStyle27;
            resources.ApplyResources(this.cLogTime, "cLogTime");
            this.cLogTime.Name = "cLogTime";
            this.cLogTime.ReadOnly = true;
            // 
            // cFuncName
            // 
            this.cFuncName.DataPropertyName = "FuncName";
            dataGridViewCellStyle28.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cFuncName.DefaultCellStyle = dataGridViewCellStyle28;
            resources.ApplyResources(this.cFuncName, "cFuncName");
            this.cFuncName.Name = "cFuncName";
            this.cFuncName.ReadOnly = true;
            // 
            // cLogContent
            // 
            this.cLogContent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cLogContent.DataPropertyName = "LogContent";
            resources.ApplyResources(this.cLogContent, "cLogContent");
            this.cLogContent.Name = "cLogContent";
            this.cLogContent.ReadOnly = true;
            // 
            // cmsLogList
            // 
            resources.ApplyResources(this.cmsLogList, "cmsLogList");
            this.cmsLogList.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.cmsLogList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsLogList_CleanUp,
            this.toolStripSeparator5,
            this.cmsLogList_ToExcel});
            this.cmsLogList.Name = "cmsLogList";
            this.cmsLogList.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsLogList_ItemClicked);
            // 
            // cmsLogList_CleanUp
            // 
            resources.ApplyResources(this.cmsLogList_CleanUp, "cmsLogList_CleanUp");
            this.cmsLogList_CleanUp.Image = global::WPELibrary.Properties.Resources.Trash_can16;
            this.cmsLogList_CleanUp.Name = "cmsLogList_CleanUp";
            // 
            // toolStripSeparator5
            // 
            resources.ApplyResources(this.toolStripSeparator5, "toolStripSeparator5");
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            // 
            // cmsLogList_ToExcel
            // 
            resources.ApplyResources(this.cmsLogList_ToExcel, "cmsLogList_ToExcel");
            this.cmsLogList_ToExcel.Image = global::WPELibrary.Properties.Resources.saveas;
            this.cmsLogList_ToExcel.Name = "cmsLogList_ToExcel";
            // 
            // tcAutomation
            // 
            resources.ApplyResources(this.tcAutomation, "tcAutomation");
            this.tcAutomation.Controls.Add(this.tpFilterList);
            this.tcAutomation.Controls.Add(this.tpSendList);
            this.tcAutomation.Controls.Add(this.tpRobotList);
            this.tcAutomation.Name = "tcAutomation";
            this.tcAutomation.SelectedIndex = 0;
            // 
            // tpFilterList
            // 
            resources.ApplyResources(this.tpFilterList, "tpFilterList");
            this.tpFilterList.BackColor = System.Drawing.SystemColors.Control;
            this.tpFilterList.Controls.Add(this.tlpFilterList);
            this.tpFilterList.Name = "tpFilterList";
            // 
            // tlpFilterList
            // 
            resources.ApplyResources(this.tlpFilterList, "tlpFilterList");
            this.tlpFilterList.Controls.Add(this.dgvFilterList, 0, 1);
            this.tlpFilterList.Controls.Add(this.tsFilterList, 0, 0);
            this.tlpFilterList.Name = "tlpFilterList";
            // 
            // dgvFilterList
            // 
            resources.ApplyResources(this.dgvFilterList, "dgvFilterList");
            this.dgvFilterList.AllowUserToAddRows = false;
            this.dgvFilterList.AllowUserToDeleteRows = false;
            this.dgvFilterList.AllowUserToResizeColumns = false;
            this.dgvFilterList.AllowUserToResizeRows = false;
            this.dgvFilterList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvFilterList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvFilterList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvFilterList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvFilterList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvFilterList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvFilterList.ColumnHeadersVisible = false;
            this.dgvFilterList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cIsCheck,
            this.cFName});
            this.dgvFilterList.ContextMenuStrip = this.cmsFilterList;
            this.dgvFilterList.MultiSelect = false;
            this.dgvFilterList.Name = "dgvFilterList";
            this.dgvFilterList.RowHeadersVisible = false;
            this.dgvFilterList.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Window;
            this.dgvFilterList.RowTemplate.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(3);
            this.dgvFilterList.RowTemplate.Height = 30;
            this.dgvFilterList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvFilterList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFilterList_CellContentClick);
            this.dgvFilterList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvFilterList_CellDoubleClick);
            // 
            // cIsCheck
            // 
            this.cIsCheck.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cIsCheck.DataPropertyName = "IsEnable";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.NullValue = false;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cIsCheck.DefaultCellStyle = dataGridViewCellStyle10;
            this.cIsCheck.FalseValue = "false";
            resources.ApplyResources(this.cIsCheck, "cIsCheck");
            this.cIsCheck.Name = "cIsCheck";
            this.cIsCheck.TrueValue = "true";
            // 
            // cFName
            // 
            this.cFName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cFName.DataPropertyName = "FName";
            resources.ApplyResources(this.cFName, "cFName");
            this.cFName.Name = "cFName";
            this.cFName.ReadOnly = true;
            // 
            // cmsFilterList
            // 
            resources.ApplyResources(this.cmsFilterList, "cmsFilterList");
            this.cmsFilterList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsFilterList_Top,
            this.cmsFilterList_tss1,
            this.cmsFilterList_Up,
            this.cmsFilterList_Down,
            this.cmsFilterList_tss2,
            this.cmsFilterList_Bottom,
            this.cmsFilterList_tss3,
            this.cmsFilterList_Copy,
            this.cmsFilterList_Export,
            this.cmsFilterList_Delete});
            this.cmsFilterList.Name = "cmsFilterList";
            this.cmsFilterList.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsFilterList_ItemClicked);
            // 
            // cmsFilterList_Top
            // 
            resources.ApplyResources(this.cmsFilterList_Top, "cmsFilterList_Top");
            this.cmsFilterList_Top.Image = global::WPELibrary.Properties.Resources.go_top;
            this.cmsFilterList_Top.Name = "cmsFilterList_Top";
            // 
            // cmsFilterList_tss1
            // 
            resources.ApplyResources(this.cmsFilterList_tss1, "cmsFilterList_tss1");
            this.cmsFilterList_tss1.Name = "cmsFilterList_tss1";
            // 
            // cmsFilterList_Up
            // 
            resources.ApplyResources(this.cmsFilterList_Up, "cmsFilterList_Up");
            this.cmsFilterList_Up.Name = "cmsFilterList_Up";
            // 
            // cmsFilterList_Down
            // 
            resources.ApplyResources(this.cmsFilterList_Down, "cmsFilterList_Down");
            this.cmsFilterList_Down.Image = global::WPELibrary.Properties.Resources.Down;
            this.cmsFilterList_Down.Name = "cmsFilterList_Down";
            // 
            // cmsFilterList_tss2
            // 
            resources.ApplyResources(this.cmsFilterList_tss2, "cmsFilterList_tss2");
            this.cmsFilterList_tss2.Name = "cmsFilterList_tss2";
            // 
            // cmsFilterList_Bottom
            // 
            resources.ApplyResources(this.cmsFilterList_Bottom, "cmsFilterList_Bottom");
            this.cmsFilterList_Bottom.Image = global::WPELibrary.Properties.Resources.go_bottom;
            this.cmsFilterList_Bottom.Name = "cmsFilterList_Bottom";
            // 
            // cmsFilterList_tss3
            // 
            resources.ApplyResources(this.cmsFilterList_tss3, "cmsFilterList_tss3");
            this.cmsFilterList_tss3.Name = "cmsFilterList_tss3";
            // 
            // cmsFilterList_Copy
            // 
            resources.ApplyResources(this.cmsFilterList_Copy, "cmsFilterList_Copy");
            this.cmsFilterList_Copy.Image = global::WPELibrary.Properties.Resources.copy;
            this.cmsFilterList_Copy.Name = "cmsFilterList_Copy";
            // 
            // cmsFilterList_Export
            // 
            resources.ApplyResources(this.cmsFilterList_Export, "cmsFilterList_Export");
            this.cmsFilterList_Export.Image = global::WPELibrary.Properties.Resources.saveas;
            this.cmsFilterList_Export.Name = "cmsFilterList_Export";
            // 
            // cmsFilterList_Delete
            // 
            resources.ApplyResources(this.cmsFilterList_Delete, "cmsFilterList_Delete");
            this.cmsFilterList_Delete.Image = global::WPELibrary.Properties.Resources.Delete;
            this.cmsFilterList_Delete.Name = "cmsFilterList_Delete";
            // 
            // tsFilterList
            // 
            resources.ApplyResources(this.tsFilterList, "tsFilterList");
            this.tsFilterList.BackColor = System.Drawing.SystemColors.Control;
            this.tsFilterList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsFilterList_Load,
            this.tsFilterList_Save,
            this.toolStripSeparator1,
            this.tsFilterList_SelectAll,
            this.tsFilterList_SelectNo,
            this.toolStripSeparator2,
            this.tsFilterList_Add,
            this.tsFilterList_CleanUp});
            this.tsFilterList.Name = "tsFilterList";
            // 
            // tsFilterList_Load
            // 
            resources.ApplyResources(this.tsFilterList_Load, "tsFilterList_Load");
            this.tsFilterList_Load.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsFilterList_Load.Image = global::WPELibrary.Properties.Resources.openHS;
            this.tsFilterList_Load.Margin = new System.Windows.Forms.Padding(3);
            this.tsFilterList_Load.Name = "tsFilterList_Load";
            this.tsFilterList_Load.Click += new System.EventHandler(this.tsFilterList_Load_Click);
            // 
            // tsFilterList_Save
            // 
            resources.ApplyResources(this.tsFilterList_Save, "tsFilterList_Save");
            this.tsFilterList_Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsFilterList_Save.Image = global::WPELibrary.Properties.Resources.save;
            this.tsFilterList_Save.Margin = new System.Windows.Forms.Padding(3);
            this.tsFilterList_Save.Name = "tsFilterList_Save";
            this.tsFilterList_Save.Click += new System.EventHandler(this.tsFilterList_Save_Click);
            // 
            // toolStripSeparator1
            // 
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            this.toolStripSeparator1.Margin = new System.Windows.Forms.Padding(1);
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            // 
            // tsFilterList_SelectAll
            // 
            resources.ApplyResources(this.tsFilterList_SelectAll, "tsFilterList_SelectAll");
            this.tsFilterList_SelectAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsFilterList_SelectAll.Image = global::WPELibrary.Properties.Resources.Select_true;
            this.tsFilterList_SelectAll.Margin = new System.Windows.Forms.Padding(3);
            this.tsFilterList_SelectAll.Name = "tsFilterList_SelectAll";
            this.tsFilterList_SelectAll.Click += new System.EventHandler(this.tsFilterList_SelectAll_Click);
            // 
            // tsFilterList_SelectNo
            // 
            resources.ApplyResources(this.tsFilterList_SelectNo, "tsFilterList_SelectNo");
            this.tsFilterList_SelectNo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsFilterList_SelectNo.Image = global::WPELibrary.Properties.Resources.Select_false;
            this.tsFilterList_SelectNo.Margin = new System.Windows.Forms.Padding(3);
            this.tsFilterList_SelectNo.Name = "tsFilterList_SelectNo";
            this.tsFilterList_SelectNo.Click += new System.EventHandler(this.tsFilterList_SelectNo_Click);
            // 
            // toolStripSeparator2
            // 
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            this.toolStripSeparator2.Margin = new System.Windows.Forms.Padding(1);
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            // 
            // tsFilterList_Add
            // 
            resources.ApplyResources(this.tsFilterList_Add, "tsFilterList_Add");
            this.tsFilterList_Add.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsFilterList_Add.Image = global::WPELibrary.Properties.Resources.Add;
            this.tsFilterList_Add.Margin = new System.Windows.Forms.Padding(3);
            this.tsFilterList_Add.Name = "tsFilterList_Add";
            this.tsFilterList_Add.Click += new System.EventHandler(this.tsFilterList_Add_Click);
            // 
            // tsFilterList_CleanUp
            // 
            resources.ApplyResources(this.tsFilterList_CleanUp, "tsFilterList_CleanUp");
            this.tsFilterList_CleanUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsFilterList_CleanUp.Image = global::WPELibrary.Properties.Resources.Trash_can16;
            this.tsFilterList_CleanUp.Margin = new System.Windows.Forms.Padding(3);
            this.tsFilterList_CleanUp.Name = "tsFilterList_CleanUp";
            this.tsFilterList_CleanUp.Click += new System.EventHandler(this.tsFilterList_CleanUp_Click);
            // 
            // tpSendList
            // 
            resources.ApplyResources(this.tpSendList, "tpSendList");
            this.tpSendList.BackColor = System.Drawing.SystemColors.Control;
            this.tpSendList.Controls.Add(this.tlpSendList);
            this.tpSendList.Name = "tpSendList";
            // 
            // tlpSendList
            // 
            resources.ApplyResources(this.tlpSendList, "tlpSendList");
            this.tlpSendList.Controls.Add(this.dgvSendList, 0, 1);
            this.tlpSendList.Controls.Add(this.tsSendList, 0, 0);
            this.tlpSendList.Name = "tlpSendList";
            // 
            // dgvSendList
            // 
            resources.ApplyResources(this.dgvSendList, "dgvSendList");
            this.dgvSendList.AllowUserToAddRows = false;
            this.dgvSendList.AllowUserToDeleteRows = false;
            this.dgvSendList.AllowUserToResizeColumns = false;
            this.dgvSendList.AllowUserToResizeRows = false;
            this.dgvSendList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvSendList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvSendList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvSendList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvSendList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvSendList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSendList.ColumnHeadersVisible = false;
            this.dgvSendList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cIsEnable,
            this.dataGridViewImageColumn3,
            this.dataGridViewTextBoxColumn1});
            this.dgvSendList.ContextMenuStrip = this.cmsSendList;
            this.dgvSendList.MultiSelect = false;
            this.dgvSendList.Name = "dgvSendList";
            this.dgvSendList.RowHeadersVisible = false;
            this.dgvSendList.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Window;
            this.dgvSendList.RowTemplate.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(3);
            this.dgvSendList.RowTemplate.Height = 25;
            this.dgvSendList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSendList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSendList_CellContentClick);
            this.dgvSendList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSendList_CellDoubleClick);
            // 
            // cIsEnable
            // 
            this.cIsEnable.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cIsEnable.DataPropertyName = "IsEnable";
            dataGridViewCellStyle29.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle29.NullValue = false;
            dataGridViewCellStyle29.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cIsEnable.DefaultCellStyle = dataGridViewCellStyle29;
            this.cIsEnable.FalseValue = "false";
            resources.ApplyResources(this.cIsEnable, "cIsEnable");
            this.cIsEnable.Name = "cIsEnable";
            this.cIsEnable.TrueValue = "true";
            // 
            // dataGridViewImageColumn3
            // 
            this.dataGridViewImageColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.dataGridViewImageColumn3, "dataGridViewImageColumn3");
            this.dataGridViewImageColumn3.Image = global::WPELibrary.Properties.Resources.sent;
            this.dataGridViewImageColumn3.Name = "dataGridViewImageColumn3";
            this.dataGridViewImageColumn3.ReadOnly = true;
            this.dataGridViewImageColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "SName";
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cmsSendList
            // 
            resources.ApplyResources(this.cmsSendList, "cmsSendList");
            this.cmsSendList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsSendList_Top,
            this.toolStripSeparator9,
            this.cmsSendList_Up,
            this.cmsSendList_Down,
            this.toolStripSeparator10,
            this.cmsSendList_Bottom,
            this.toolStripSeparator11,
            this.cmsSendList_Copy,
            this.cmsSendList_Export,
            this.cmsSendList_Delete});
            this.cmsSendList.Name = "cmsSendList";
            this.cmsSendList.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsSendList_ItemClicked);
            // 
            // cmsSendList_Top
            // 
            resources.ApplyResources(this.cmsSendList_Top, "cmsSendList_Top");
            this.cmsSendList_Top.Image = global::WPELibrary.Properties.Resources.go_top;
            this.cmsSendList_Top.Name = "cmsSendList_Top";
            // 
            // toolStripSeparator9
            // 
            resources.ApplyResources(this.toolStripSeparator9, "toolStripSeparator9");
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            // 
            // cmsSendList_Up
            // 
            resources.ApplyResources(this.cmsSendList_Up, "cmsSendList_Up");
            this.cmsSendList_Up.Image = global::WPELibrary.Properties.Resources.Up;
            this.cmsSendList_Up.Name = "cmsSendList_Up";
            // 
            // cmsSendList_Down
            // 
            resources.ApplyResources(this.cmsSendList_Down, "cmsSendList_Down");
            this.cmsSendList_Down.Image = global::WPELibrary.Properties.Resources.Down;
            this.cmsSendList_Down.Name = "cmsSendList_Down";
            // 
            // toolStripSeparator10
            // 
            resources.ApplyResources(this.toolStripSeparator10, "toolStripSeparator10");
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            // 
            // cmsSendList_Bottom
            // 
            resources.ApplyResources(this.cmsSendList_Bottom, "cmsSendList_Bottom");
            this.cmsSendList_Bottom.Image = global::WPELibrary.Properties.Resources.go_bottom;
            this.cmsSendList_Bottom.Name = "cmsSendList_Bottom";
            // 
            // toolStripSeparator11
            // 
            resources.ApplyResources(this.toolStripSeparator11, "toolStripSeparator11");
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            // 
            // cmsSendList_Copy
            // 
            resources.ApplyResources(this.cmsSendList_Copy, "cmsSendList_Copy");
            this.cmsSendList_Copy.Image = global::WPELibrary.Properties.Resources.copy;
            this.cmsSendList_Copy.Name = "cmsSendList_Copy";
            // 
            // cmsSendList_Export
            // 
            resources.ApplyResources(this.cmsSendList_Export, "cmsSendList_Export");
            this.cmsSendList_Export.Image = global::WPELibrary.Properties.Resources.saveas;
            this.cmsSendList_Export.Name = "cmsSendList_Export";
            // 
            // cmsSendList_Delete
            // 
            resources.ApplyResources(this.cmsSendList_Delete, "cmsSendList_Delete");
            this.cmsSendList_Delete.Image = global::WPELibrary.Properties.Resources.Delete;
            this.cmsSendList_Delete.Name = "cmsSendList_Delete";
            // 
            // tsSendList
            // 
            resources.ApplyResources(this.tsSendList, "tsSendList");
            this.tsSendList.BackColor = System.Drawing.SystemColors.Control;
            this.tsSendList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsSendList_Load,
            this.tsSendList_Save,
            this.toolStripSeparator8,
            this.tsSendList_Start,
            this.tsSendList_Stop,
            this.toolStripSeparator7,
            this.tsSendList_Add,
            this.tsSendList_CleanUp});
            this.tsSendList.Name = "tsSendList";
            // 
            // tsSendList_Load
            // 
            resources.ApplyResources(this.tsSendList_Load, "tsSendList_Load");
            this.tsSendList_Load.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsSendList_Load.Image = global::WPELibrary.Properties.Resources.openHS;
            this.tsSendList_Load.Margin = new System.Windows.Forms.Padding(3);
            this.tsSendList_Load.Name = "tsSendList_Load";
            this.tsSendList_Load.Click += new System.EventHandler(this.tsSendList_Load_Click);
            // 
            // tsSendList_Save
            // 
            resources.ApplyResources(this.tsSendList_Save, "tsSendList_Save");
            this.tsSendList_Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsSendList_Save.Image = global::WPELibrary.Properties.Resources.save;
            this.tsSendList_Save.Margin = new System.Windows.Forms.Padding(3);
            this.tsSendList_Save.Name = "tsSendList_Save";
            this.tsSendList_Save.Click += new System.EventHandler(this.tsSendList_Save_Click);
            // 
            // toolStripSeparator8
            // 
            resources.ApplyResources(this.toolStripSeparator8, "toolStripSeparator8");
            this.toolStripSeparator8.Margin = new System.Windows.Forms.Padding(1);
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            // 
            // tsSendList_Start
            // 
            resources.ApplyResources(this.tsSendList_Start, "tsSendList_Start");
            this.tsSendList_Start.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsSendList_Start.Image = global::WPELibrary.Properties.Resources.Start;
            this.tsSendList_Start.Margin = new System.Windows.Forms.Padding(3);
            this.tsSendList_Start.Name = "tsSendList_Start";
            this.tsSendList_Start.Click += new System.EventHandler(this.tsSendList_Start_Click);
            // 
            // tsSendList_Stop
            // 
            resources.ApplyResources(this.tsSendList_Stop, "tsSendList_Stop");
            this.tsSendList_Stop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsSendList_Stop.Image = global::WPELibrary.Properties.Resources.Stop;
            this.tsSendList_Stop.Margin = new System.Windows.Forms.Padding(3);
            this.tsSendList_Stop.Name = "tsSendList_Stop";
            this.tsSendList_Stop.Click += new System.EventHandler(this.tsSendList_Stop_Click);
            // 
            // toolStripSeparator7
            // 
            resources.ApplyResources(this.toolStripSeparator7, "toolStripSeparator7");
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            // 
            // tsSendList_Add
            // 
            resources.ApplyResources(this.tsSendList_Add, "tsSendList_Add");
            this.tsSendList_Add.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsSendList_Add.Image = global::WPELibrary.Properties.Resources.Add;
            this.tsSendList_Add.Margin = new System.Windows.Forms.Padding(3);
            this.tsSendList_Add.Name = "tsSendList_Add";
            this.tsSendList_Add.Click += new System.EventHandler(this.tsSendList_Add_Click);
            // 
            // tsSendList_CleanUp
            // 
            resources.ApplyResources(this.tsSendList_CleanUp, "tsSendList_CleanUp");
            this.tsSendList_CleanUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsSendList_CleanUp.Image = global::WPELibrary.Properties.Resources.Trash_can16;
            this.tsSendList_CleanUp.Margin = new System.Windows.Forms.Padding(3);
            this.tsSendList_CleanUp.Name = "tsSendList_CleanUp";
            this.tsSendList_CleanUp.Click += new System.EventHandler(this.tsSendList_CleanUp_Click);
            // 
            // tpRobotList
            // 
            resources.ApplyResources(this.tpRobotList, "tpRobotList");
            this.tpRobotList.BackColor = System.Drawing.SystemColors.Control;
            this.tpRobotList.Controls.Add(this.tlpRobotList);
            this.tpRobotList.Name = "tpRobotList";
            // 
            // tlpRobotList
            // 
            resources.ApplyResources(this.tlpRobotList, "tlpRobotList");
            this.tlpRobotList.Controls.Add(this.dgvRobotList, 0, 1);
            this.tlpRobotList.Controls.Add(this.tsRobotList, 0, 0);
            this.tlpRobotList.Name = "tlpRobotList";
            // 
            // dgvRobotList
            // 
            resources.ApplyResources(this.dgvRobotList, "dgvRobotList");
            this.dgvRobotList.AllowUserToAddRows = false;
            this.dgvRobotList.AllowUserToDeleteRows = false;
            this.dgvRobotList.AllowUserToResizeColumns = false;
            this.dgvRobotList.AllowUserToResizeRows = false;
            this.dgvRobotList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvRobotList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvRobotList.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvRobotList.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvRobotList.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvRobotList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRobotList.ColumnHeadersVisible = false;
            this.dgvRobotList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cRIsEnable,
            this.cImg,
            this.cRName});
            this.dgvRobotList.ContextMenuStrip = this.cmsRobotList;
            this.dgvRobotList.MultiSelect = false;
            this.dgvRobotList.Name = "dgvRobotList";
            this.dgvRobotList.RowHeadersVisible = false;
            this.dgvRobotList.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Window;
            this.dgvRobotList.RowTemplate.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(3);
            this.dgvRobotList.RowTemplate.Height = 25;
            this.dgvRobotList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRobotList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRobotList_CellContentClick);
            this.dgvRobotList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvRobotList_CellDoubleClick);
            // 
            // cRIsEnable
            // 
            this.cRIsEnable.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cRIsEnable.DataPropertyName = "IsEnable";
            dataGridViewCellStyle30.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle30.NullValue = false;
            dataGridViewCellStyle30.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cRIsEnable.DefaultCellStyle = dataGridViewCellStyle30;
            this.cRIsEnable.FalseValue = "false";
            resources.ApplyResources(this.cRIsEnable, "cRIsEnable");
            this.cRIsEnable.Name = "cRIsEnable";
            this.cRIsEnable.TrueValue = "true";
            // 
            // cImg
            // 
            this.cImg.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.cImg, "cImg");
            this.cImg.Image = global::WPELibrary.Properties.Resources.computer;
            this.cImg.Name = "cImg";
            this.cImg.ReadOnly = true;
            this.cImg.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // cRName
            // 
            this.cRName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cRName.DataPropertyName = "RName";
            resources.ApplyResources(this.cRName, "cRName");
            this.cRName.Name = "cRName";
            this.cRName.ReadOnly = true;
            this.cRName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cRName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cmsRobotList
            // 
            resources.ApplyResources(this.cmsRobotList, "cmsRobotList");
            this.cmsRobotList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsRobotList_Top,
            this.cmsRobotList_Split1,
            this.cmsRobotList_Up,
            this.cmsRobotList_Down,
            this.cmsRobotList_Split2,
            this.cmsRobotList_Bottom,
            this.cmsRobotList_Split3,
            this.cmsRobotList_Copy,
            this.cmsRobotList_Export,
            this.cmsRobotList_Delete});
            this.cmsRobotList.Name = "cmsRobotList";
            this.cmsRobotList.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsRobotList_ItemClicked);
            // 
            // cmsRobotList_Top
            // 
            resources.ApplyResources(this.cmsRobotList_Top, "cmsRobotList_Top");
            this.cmsRobotList_Top.Image = global::WPELibrary.Properties.Resources.go_top;
            this.cmsRobotList_Top.Name = "cmsRobotList_Top";
            // 
            // cmsRobotList_Split1
            // 
            resources.ApplyResources(this.cmsRobotList_Split1, "cmsRobotList_Split1");
            this.cmsRobotList_Split1.Name = "cmsRobotList_Split1";
            // 
            // cmsRobotList_Up
            // 
            resources.ApplyResources(this.cmsRobotList_Up, "cmsRobotList_Up");
            this.cmsRobotList_Up.Image = global::WPELibrary.Properties.Resources.Up;
            this.cmsRobotList_Up.Name = "cmsRobotList_Up";
            // 
            // cmsRobotList_Down
            // 
            resources.ApplyResources(this.cmsRobotList_Down, "cmsRobotList_Down");
            this.cmsRobotList_Down.Image = global::WPELibrary.Properties.Resources.Down;
            this.cmsRobotList_Down.Name = "cmsRobotList_Down";
            // 
            // cmsRobotList_Split2
            // 
            resources.ApplyResources(this.cmsRobotList_Split2, "cmsRobotList_Split2");
            this.cmsRobotList_Split2.Name = "cmsRobotList_Split2";
            // 
            // cmsRobotList_Bottom
            // 
            resources.ApplyResources(this.cmsRobotList_Bottom, "cmsRobotList_Bottom");
            this.cmsRobotList_Bottom.Image = global::WPELibrary.Properties.Resources.go_bottom;
            this.cmsRobotList_Bottom.Name = "cmsRobotList_Bottom";
            // 
            // cmsRobotList_Split3
            // 
            resources.ApplyResources(this.cmsRobotList_Split3, "cmsRobotList_Split3");
            this.cmsRobotList_Split3.Name = "cmsRobotList_Split3";
            // 
            // cmsRobotList_Copy
            // 
            resources.ApplyResources(this.cmsRobotList_Copy, "cmsRobotList_Copy");
            this.cmsRobotList_Copy.Image = global::WPELibrary.Properties.Resources.copy;
            this.cmsRobotList_Copy.Name = "cmsRobotList_Copy";
            // 
            // cmsRobotList_Export
            // 
            resources.ApplyResources(this.cmsRobotList_Export, "cmsRobotList_Export");
            this.cmsRobotList_Export.Image = global::WPELibrary.Properties.Resources.saveas;
            this.cmsRobotList_Export.Name = "cmsRobotList_Export";
            // 
            // cmsRobotList_Delete
            // 
            resources.ApplyResources(this.cmsRobotList_Delete, "cmsRobotList_Delete");
            this.cmsRobotList_Delete.Image = global::WPELibrary.Properties.Resources.Delete;
            this.cmsRobotList_Delete.Name = "cmsRobotList_Delete";
            // 
            // tsRobotList
            // 
            resources.ApplyResources(this.tsRobotList, "tsRobotList");
            this.tsRobotList.BackColor = System.Drawing.SystemColors.Control;
            this.tsRobotList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsRobotList_Load,
            this.tsRobotList_Save,
            this.toolStripSeparator4,
            this.tsRobotList_Start,
            this.tsRobotList_Stop,
            this.toolStripSeparator12,
            this.tsRobotList_Add,
            this.tsRobotList_CleanUp});
            this.tsRobotList.Name = "tsRobotList";
            // 
            // tsRobotList_Load
            // 
            resources.ApplyResources(this.tsRobotList_Load, "tsRobotList_Load");
            this.tsRobotList_Load.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsRobotList_Load.Image = global::WPELibrary.Properties.Resources.openHS;
            this.tsRobotList_Load.Margin = new System.Windows.Forms.Padding(3);
            this.tsRobotList_Load.Name = "tsRobotList_Load";
            this.tsRobotList_Load.Click += new System.EventHandler(this.tsRobotList_Load_Click);
            // 
            // tsRobotList_Save
            // 
            resources.ApplyResources(this.tsRobotList_Save, "tsRobotList_Save");
            this.tsRobotList_Save.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsRobotList_Save.Image = global::WPELibrary.Properties.Resources.save;
            this.tsRobotList_Save.Margin = new System.Windows.Forms.Padding(3);
            this.tsRobotList_Save.Name = "tsRobotList_Save";
            this.tsRobotList_Save.Click += new System.EventHandler(this.tsRobotList_Save_Click);
            // 
            // toolStripSeparator4
            // 
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            this.toolStripSeparator4.Margin = new System.Windows.Forms.Padding(1);
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            // 
            // tsRobotList_Start
            // 
            resources.ApplyResources(this.tsRobotList_Start, "tsRobotList_Start");
            this.tsRobotList_Start.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsRobotList_Start.Image = global::WPELibrary.Properties.Resources.Start;
            this.tsRobotList_Start.Margin = new System.Windows.Forms.Padding(3);
            this.tsRobotList_Start.Name = "tsRobotList_Start";
            this.tsRobotList_Start.Click += new System.EventHandler(this.tsRobotList_Start_Click);
            // 
            // tsRobotList_Stop
            // 
            resources.ApplyResources(this.tsRobotList_Stop, "tsRobotList_Stop");
            this.tsRobotList_Stop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsRobotList_Stop.Image = global::WPELibrary.Properties.Resources.Stop;
            this.tsRobotList_Stop.Margin = new System.Windows.Forms.Padding(3);
            this.tsRobotList_Stop.Name = "tsRobotList_Stop";
            this.tsRobotList_Stop.Click += new System.EventHandler(this.tsRobotList_Stop_Click);
            // 
            // toolStripSeparator12
            // 
            resources.ApplyResources(this.toolStripSeparator12, "toolStripSeparator12");
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            // 
            // tsRobotList_Add
            // 
            resources.ApplyResources(this.tsRobotList_Add, "tsRobotList_Add");
            this.tsRobotList_Add.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsRobotList_Add.Image = global::WPELibrary.Properties.Resources.Add;
            this.tsRobotList_Add.Margin = new System.Windows.Forms.Padding(3);
            this.tsRobotList_Add.Name = "tsRobotList_Add";
            this.tsRobotList_Add.Click += new System.EventHandler(this.tsRobotList_Add_Click);
            // 
            // tsRobotList_CleanUp
            // 
            resources.ApplyResources(this.tsRobotList_CleanUp, "tsRobotList_CleanUp");
            this.tsRobotList_CleanUp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsRobotList_CleanUp.Image = global::WPELibrary.Properties.Resources.Trash_can16;
            this.tsRobotList_CleanUp.Margin = new System.Windows.Forms.Padding(3);
            this.tsRobotList_CleanUp.Name = "tsRobotList_CleanUp";
            this.tsRobotList_CleanUp.Click += new System.EventHandler(this.tsRobotList_CleanUp_Click);
            // 
            // ssProcessInfo
            // 
            resources.ApplyResources(this.ssProcessInfo, "ssProcessInfo");
            this.ssProcessInfo.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslProcessName,
            this.tsslSplit2,
            this.tsslProcessInfo,
            this.tsslSplit1,
            this.tsslWinSock,
            this.tsslSplit3,
            this.tsslTotalBytes});
            this.ssProcessInfo.Name = "ssProcessInfo";
            // 
            // tsslProcessName
            // 
            resources.ApplyResources(this.tsslProcessName, "tsslProcessName");
            this.tsslProcessName.Name = "tsslProcessName";
            // 
            // tsslSplit2
            // 
            resources.ApplyResources(this.tsslSplit2, "tsslSplit2");
            this.tsslSplit2.ForeColor = System.Drawing.Color.DarkGray;
            this.tsslSplit2.Name = "tsslSplit2";
            // 
            // tsslProcessInfo
            // 
            resources.ApplyResources(this.tsslProcessInfo, "tsslProcessInfo");
            this.tsslProcessInfo.Name = "tsslProcessInfo";
            // 
            // tsslSplit1
            // 
            resources.ApplyResources(this.tsslSplit1, "tsslSplit1");
            this.tsslSplit1.ForeColor = System.Drawing.Color.DarkGray;
            this.tsslSplit1.Name = "tsslSplit1";
            // 
            // tsslWinSock
            // 
            resources.ApplyResources(this.tsslWinSock, "tsslWinSock");
            this.tsslWinSock.Name = "tsslWinSock";
            // 
            // tsslSplit3
            // 
            resources.ApplyResources(this.tsslSplit3, "tsslSplit3");
            this.tsslSplit3.ForeColor = System.Drawing.Color.DarkGray;
            this.tsslSplit3.Name = "tsslSplit3";
            // 
            // tsslTotalBytes
            // 
            resources.ApplyResources(this.tsslTotalBytes, "tsslTotalBytes");
            this.tsslTotalBytes.Name = "tsslTotalBytes";
            // 
            // tSocketInfo
            // 
            this.tSocketInfo.Interval = 1000;
            this.tSocketInfo.Tick += new System.EventHandler(this.tSocketInfo_Tick);
            // 
            // niWPE
            // 
            resources.ApplyResources(this.niWPE, "niWPE");
            this.niWPE.ContextMenuStrip = this.cmsIcon;
            this.niWPE.Click += new System.EventHandler(this.niWPE_Click);
            // 
            // cmsIcon
            // 
            resources.ApplyResources(this.cmsIcon, "cmsIcon");
            this.cmsIcon.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsIcon_Show,
            this.tss17,
            this.cmsIcon_StartHook,
            this.cmsIcon_StopHook,
            this.tss18,
            this.cmsIcon_CleanUp,
            this.tss19,
            this.cmsIcon_Exit});
            this.cmsIcon.Name = "cmsIcon";
            this.cmsIcon.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsIcon_ItemClicked);
            // 
            // cmsIcon_Show
            // 
            resources.ApplyResources(this.cmsIcon_Show, "cmsIcon_Show");
            this.cmsIcon_Show.Image = global::WPELibrary.Properties.Resources.Show;
            this.cmsIcon_Show.Name = "cmsIcon_Show";
            // 
            // tss17
            // 
            resources.ApplyResources(this.tss17, "tss17");
            this.tss17.Name = "tss17";
            // 
            // cmsIcon_StartHook
            // 
            resources.ApplyResources(this.cmsIcon_StartHook, "cmsIcon_StartHook");
            this.cmsIcon_StartHook.Image = global::WPELibrary.Properties.Resources.Play16;
            this.cmsIcon_StartHook.Name = "cmsIcon_StartHook";
            // 
            // cmsIcon_StopHook
            // 
            resources.ApplyResources(this.cmsIcon_StopHook, "cmsIcon_StopHook");
            this.cmsIcon_StopHook.Image = global::WPELibrary.Properties.Resources.Stop16;
            this.cmsIcon_StopHook.Name = "cmsIcon_StopHook";
            // 
            // tss18
            // 
            resources.ApplyResources(this.tss18, "tss18");
            this.tss18.Name = "tss18";
            // 
            // cmsIcon_CleanUp
            // 
            resources.ApplyResources(this.cmsIcon_CleanUp, "cmsIcon_CleanUp");
            this.cmsIcon_CleanUp.Image = global::WPELibrary.Properties.Resources.Trash_can16;
            this.cmsIcon_CleanUp.Name = "cmsIcon_CleanUp";
            // 
            // tss19
            // 
            resources.ApplyResources(this.tss19, "tss19");
            this.tss19.Name = "tss19";
            // 
            // cmsIcon_Exit
            // 
            resources.ApplyResources(this.cmsIcon_Exit, "cmsIcon_Exit");
            this.cmsIcon_Exit.Image = global::WPELibrary.Properties.Resources.exit;
            this.cmsIcon_Exit.Name = "cmsIcon_Exit";
            // 
            // tSocketList
            // 
            this.tSocketList.Interval = 10;
            this.tSocketList.Tick += new System.EventHandler(this.tSocketList_Tick);
            // 
            // ofdExtraction
            // 
            resources.ApplyResources(this.ofdExtraction, "ofdExtraction");
            // 
            // sfdExtraction
            // 
            resources.ApplyResources(this.sfdExtraction, "sfdExtraction");
            // 
            // dataGridViewImageColumn1
            // 
            dataGridViewCellStyle31.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle31.NullValue = null;
            dataGridViewCellStyle31.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewImageColumn1.DefaultCellStyle = dataGridViewCellStyle31;
            resources.ApplyResources(this.dataGridViewImageColumn1, "dataGridViewImageColumn1");
            this.dataGridViewImageColumn1.Image = global::WPELibrary.Properties.Resources.sent;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.ReadOnly = true;
            this.dataGridViewImageColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // dataGridViewImageColumn2
            // 
            this.dataGridViewImageColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.dataGridViewImageColumn2, "dataGridViewImageColumn2");
            this.dataGridViewImageColumn2.Image = global::WPELibrary.Properties.Resources.computer;
            this.dataGridViewImageColumn2.Name = "dataGridViewImageColumn2";
            this.dataGridViewImageColumn2.ReadOnly = true;
            this.dataGridViewImageColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // bgwSendList
            // 
            this.bgwSendList.WorkerSupportsCancellation = true;
            this.bgwSendList.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwSendList_DoWork);
            this.bgwSendList.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwSendList_RunWorkerCompleted);
            // 
            // bgwRobotList
            // 
            this.bgwRobotList.WorkerSupportsCancellation = true;
            this.bgwRobotList.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwRobotList_DoWork);
            this.bgwRobotList.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwRobotList_RunWorkerCompleted);
            // 
            // Socket_Form
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpSocketForm);
            this.DoubleBuffered = true;
            this.Name = "Socket_Form";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DLL_Form_FormClosed);
            this.Load += new System.EventHandler(this.Socket_Form_Load);
            this.Resize += new System.EventHandler(this.Socket_Form_Resize);
            this.tlpSocketForm.ResumeLayout(false);
            this.tlpSocketForm.PerformLayout();
            this.ssSocketList.ResumeLayout(false);
            this.ssSocketList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSocketList)).EndInit();
            this.cmsSocketList.ResumeLayout(false);
            this.tlpParameter.ResumeLayout(false);
            this.gbHookButton_Search.ResumeLayout(false);
            this.tlpSearch.ResumeLayout(false);
            this.tlpSearch.PerformLayout();
            this.tlpSearchButton.ResumeLayout(false);
            this.tlpHookButton.ResumeLayout(false);
            this.tlpHookButton_Start.ResumeLayout(false);
            this.tcSocketInfo.ResumeLayout(false);
            this.tcSocketInfo_FilterSet.ResumeLayout(false);
            this.tlpFilterSet.ResumeLayout(false);
            this.tlpFilterSet.PerformLayout();
            this.tcSocketInfo_HookSet.ResumeLayout(false);
            this.tlpHookSet.ResumeLayout(false);
            this.gbHookSet_WinsockWSA.ResumeLayout(false);
            this.tlpHookSet_WinsockWSA.ResumeLayout(false);
            this.tlpHookSet_WinsockWSA.PerformLayout();
            this.gbHookSet_Winsock.ResumeLayout(false);
            this.tlpHookSet_Winsock.ResumeLayout(false);
            this.tlpHookSet_Winsock.PerformLayout();
            this.tcSocketInfo_ListSet.ResumeLayout(false);
            this.tlpListSet.ResumeLayout(false);
            this.gbListSet_LogList.ResumeLayout(false);
            this.tlpListSet_LogList.ResumeLayout(false);
            this.tlpListSet_LogList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLogList_AutoClearValue)).EndInit();
            this.gbListSet_SocketList.ResumeLayout(false);
            this.tlpListSet_SocketList.ResumeLayout(false);
            this.tlpListSet_SocketList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSocketList_AutoClearValue)).EndInit();
            this.tcSocketInfo_HotKey.ResumeLayout(false);
            this.tlpHotKeySet.ResumeLayout(false);
            this.tlpHotKeySet.PerformLayout();
            this.tcSocketInfo_SystemSet.ResumeLayout(false);
            this.tlpSystemSet.ResumeLayout(false);
            this.gbSystemSet_ShowMode.ResumeLayout(false);
            this.tlpSystemSet_ShowMode.ResumeLayout(false);
            this.tlpSystemSet_ShowMode.PerformLayout();
            this.gbSystemSet_FilterSet.ResumeLayout(false);
            this.tlpSystemSet_FilterSet.ResumeLayout(false);
            this.tlpSystemSet_FilterSet.PerformLayout();
            this.gbSystemSet_WorkMode.ResumeLayout(false);
            this.tlpSystemSet_WorkMode.ResumeLayout(false);
            this.tlpSystemSet_WorkMode.PerformLayout();
            this.tlpInformation.ResumeLayout(false);
            this.tlpPacketInfo.ResumeLayout(false);
            this.tcPacketInfo.ResumeLayout(false);
            this.tpPacketData.ResumeLayout(false);
            this.tlpPacketData.ResumeLayout(false);
            this.tlpHexBox.ResumeLayout(false);
            this.cmsHexBox.ResumeLayout(false);
            this.tpComparison.ResumeLayout(false);
            this.tlpComparison.ResumeLayout(false);
            this.tlpComparison.PerformLayout();
            this.tlpComparison_Button.ResumeLayout(false);
            this.pComparison_A.ResumeLayout(false);
            this.pComparison_B.ResumeLayout(false);
            this.pComparison_Result.ResumeLayout(false);
            this.tpXOR.ResumeLayout(false);
            this.tlpPacketInfo_XOR.ResumeLayout(false);
            this.tlpPacketInfo_XOR_Button.ResumeLayout(false);
            this.tlpPacketInfo_XOR_Button.PerformLayout();
            this.tpEncoding.ResumeLayout(false);
            this.tlpPacketInfo_Encoding.ResumeLayout(false);
            this.tlpPacketInfo_Encoding_Button.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbEncoding)).EndInit();
            this.tlpPacketInfo_Encoding_Result.ResumeLayout(false);
            this.tlpPacketInfo_Encoding_Result.PerformLayout();
            this.pPacketInfo_Encoding.ResumeLayout(false);
            this.tpExtraction.ResumeLayout(false);
            this.tlpExtraction.ResumeLayout(false);
            this.pExtraction.ResumeLayout(false);
            this.cmsExtraction.ResumeLayout(false);
            this.tlpExtraction_Button.ResumeLayout(false);
            this.tpSystemLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogList)).EndInit();
            this.cmsLogList.ResumeLayout(false);
            this.tcAutomation.ResumeLayout(false);
            this.tpFilterList.ResumeLayout(false);
            this.tlpFilterList.ResumeLayout(false);
            this.tlpFilterList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFilterList)).EndInit();
            this.cmsFilterList.ResumeLayout(false);
            this.tsFilterList.ResumeLayout(false);
            this.tsFilterList.PerformLayout();
            this.tpSendList.ResumeLayout(false);
            this.tlpSendList.ResumeLayout(false);
            this.tlpSendList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSendList)).EndInit();
            this.cmsSendList.ResumeLayout(false);
            this.tsSendList.ResumeLayout(false);
            this.tsSendList.PerformLayout();
            this.tpRobotList.ResumeLayout(false);
            this.tlpRobotList.ResumeLayout(false);
            this.tlpRobotList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRobotList)).EndInit();
            this.cmsRobotList.ResumeLayout(false);
            this.tsRobotList.ResumeLayout(false);
            this.tsRobotList.PerformLayout();
            this.ssProcessInfo.ResumeLayout(false);
            this.ssProcessInfo.PerformLayout();
            this.cmsIcon.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpSocketForm;
        private System.Windows.Forms.TableLayoutPanel tlpParameter;
        private System.Windows.Forms.DataGridView dgvSocketList;
        private System.Windows.Forms.TableLayoutPanel tlpInformation;
        private System.Windows.Forms.TableLayoutPanel tlpPacketInfo;
        private System.Windows.Forms.TabControl tcPacketInfo;
        private System.Windows.Forms.TabPage tpSystemLog;
        private System.Windows.Forms.ContextMenuStrip cmsSocketList;
        private System.Windows.Forms.ToolStripSeparator tss5;
        private System.Windows.Forms.ToolStripMenuItem cmsSocketList_ToExcel;
        private System.Windows.Forms.ContextMenuStrip cmsLogList;
        private System.Windows.Forms.ToolStripMenuItem cmsLogList_CleanUp;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem cmsLogList_ToExcel;
        private System.Windows.Forms.Timer tSocketInfo;
        private System.Windows.Forms.TabPage tpPacketData;
        private System.Windows.Forms.TableLayoutPanel tlpPacketData;
        private System.Windows.Forms.TableLayoutPanel tlpHexBox;
        private Be.Windows.Forms.HexBox hbPacketData;
        private System.Windows.Forms.StatusStrip ssSocketList;
        private System.Windows.Forms.ToolStripStatusLabel tlTotal;
        private System.Windows.Forms.ToolStripStatusLabel tlTotal_CNT;
        private System.Windows.Forms.ToolStripStatusLabel tlSplit1;
        private System.Windows.Forms.ToolStripStatusLabel tlQueue;
        private System.Windows.Forms.ToolStripStatusLabel tlQueue_CNT;
        private System.Windows.Forms.ToolStripStatusLabel tlSplit2;
        private System.Windows.Forms.ToolStripStatusLabel tlSend;
        private System.Windows.Forms.ToolStripStatusLabel tlSend_CNT;
        private System.Windows.Forms.ToolStripStatusLabel tlSplit3;
        private System.Windows.Forms.ToolStripStatusLabel tlRecv;
        private System.Windows.Forms.ToolStripStatusLabel tlRecv_CNT;
        private System.Windows.Forms.ToolStripStatusLabel tlSplit4;
        private System.Windows.Forms.ToolStripStatusLabel tlFilterSocketList;
        private System.Windows.Forms.ToolStripStatusLabel tlFilterSocketList_CNT;
        private System.Windows.Forms.GroupBox gbHookButton_Search;
        private System.Windows.Forms.TableLayoutPanel tlpHookButton;
        private System.Windows.Forms.Button bCleanUp;
        private System.Windows.Forms.TableLayoutPanel tlpHookButton_Start;
        private System.Windows.Forms.Button bStopHook;
        private System.Windows.Forms.Button bStartHook;
        private System.Windows.Forms.TableLayoutPanel tlpSearch;
        private System.Windows.Forms.TableLayoutPanel tlpSearchButton;
        private System.Windows.Forms.Button bSearchNext;
        private System.Windows.Forms.Button bSearch;
        private System.Windows.Forms.StatusStrip ssProcessInfo;
        private System.Windows.Forms.ToolStripStatusLabel tsslProcessName;
        private System.Windows.Forms.ToolStripStatusLabel tsslSplit1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel tlSendTo;
        private System.Windows.Forms.ToolStripStatusLabel tlSendTo_CNT;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ToolStripStatusLabel tlRecvFrom;
        private System.Windows.Forms.ToolStripStatusLabel tlRecvFrom_CNT;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel8;
        private System.Windows.Forms.ToolStripStatusLabel tlWSASend;
        private System.Windows.Forms.ToolStripStatusLabel tlWSASend_CNT;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel11;
        private System.Windows.Forms.ToolStripStatusLabel tlWSARecv;
        private System.Windows.Forms.ToolStripStatusLabel tlWSARecv_CNT;
        private System.Windows.Forms.ToolStripStatusLabel tlWSASendTo;
        private System.Windows.Forms.ToolStripStatusLabel tlWSASendTo_CNT;
        private System.Windows.Forms.ToolStripStatusLabel tlWSARecvFrom;
        private System.Windows.Forms.ToolStripStatusLabel tlWSARecvFrom_CNT;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.ToolStripStatusLabel tsslTotalBytes;
        private System.Windows.Forms.ToolStripStatusLabel tsslWinSock;
        private System.Windows.Forms.ToolStripStatusLabel tsslSplit3;
        private System.Windows.Forms.ContextMenuStrip cmsIcon;
        private System.Windows.Forms.ToolStripMenuItem cmsIcon_Show;
        private System.Windows.Forms.ToolStripSeparator tss17;
        private System.Windows.Forms.ToolStripMenuItem cmsIcon_Exit;
        private System.Windows.Forms.ToolStripMenuItem cmsIcon_StartHook;
        private System.Windows.Forms.ToolStripMenuItem cmsIcon_StopHook;
        private System.Windows.Forms.ToolStripSeparator tss18;
        private System.Windows.Forms.ToolStripMenuItem cmsIcon_CleanUp;
        private System.Windows.Forms.ToolStripSeparator tss19;
        private System.Windows.Forms.ContextMenuStrip cmsHexBox;
        private System.Windows.Forms.ToolStripMenuItem cmsHexBox_Send;
        private System.Windows.Forms.ToolStripSeparator cmsHexBox_tss1;
        private System.Windows.Forms.ToolStripMenuItem cmsHexBox_FilterList;
        private System.Windows.Forms.ToolStripMenuItem cmsSocketList_Send;
        private System.Windows.Forms.ToolStripSeparator cmsSocketList_tss1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel9;
        private System.Windows.Forms.TabPage tpComparison;
        private System.Windows.Forms.TableLayoutPanel tlpComparison;
        private System.Windows.Forms.Label lComparison_A;
        private System.Windows.Forms.Label lComparison_B;
        private System.Windows.Forms.TableLayoutPanel tlpComparison_Button;
        private System.Windows.Forms.Button bComparison;
        private System.Windows.Forms.Button bComparison_Clear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem cmsSocketList_Comparison_A;
        private System.Windows.Forms.ToolStripMenuItem cmsSocketList_Comparison_B;
        private System.Windows.Forms.Button bComparison_Exchange;
        private System.Windows.Forms.DataGridView dgvLogList;
        private System.Windows.Forms.TabPage tpEncoding;
        private System.Windows.Forms.TableLayoutPanel tlpPacketInfo_Encoding;
        private System.Windows.Forms.TableLayoutPanel tlpPacketInfo_Encoding_Button;
        private System.Windows.Forms.Button bPacketInfo_Encoding;
        private System.Windows.Forms.Button bPacketInfo_Decoding;
        private System.Windows.Forms.TableLayoutPanel tlpPacketInfo_Encoding_Result;
        private System.Windows.Forms.Label lPacketInfo_Encoding_UTF8;
        private System.Windows.Forms.Label lPacketInfo_Encoding_Bytes;
        private System.Windows.Forms.Label lPacketInfo_Encoding_base64;
        private System.Windows.Forms.TextBox txtPacketInfo_Encoding_UTF8;
        private System.Windows.Forms.TextBox txtPacketInfo_Encoding_UTF7;
        private System.Windows.Forms.TextBox txtPacketInfo_Encoding_Bytes;
        private System.Windows.Forms.TextBox txtPacketInfo_Encoding_ANSIbase64;
        private System.Windows.Forms.TextBox txtPacketInfo_Encoding_ANSIUTF8;
        private System.Windows.Forms.TextBox txtPacketInfo_Encoding_ANSIUTF7;
        private System.Windows.Forms.TextBox txtPacketInfo_Encoding_ANSIGBK;
        private System.Windows.Forms.Label lPacketInfo_Encoding_ANSIbase64;
        private System.Windows.Forms.Label lPacketInfo_Encoding_ANSIUTF8;
        private System.Windows.Forms.Label lPacketInfo_Encoding_ANSIUTF7;
        private System.Windows.Forms.Label lPacketInfo_Encoding_UTF7;
        private System.Windows.Forms.Label lPacketInfo_Encoding_ANSIGBK;
        private System.Windows.Forms.TextBox txtPacketInfo_Encoding_base64;
        private System.Windows.Forms.TextBox txtPacketInfo_Encoding_UTF16;
        private System.Windows.Forms.Label lPacketInfo_Encoding_ASCII;
        private System.Windows.Forms.Label lPacketInfo_Encoding_UTF32;
        private System.Windows.Forms.TextBox txtPacketInfo_Encoding_UTF32;
        private System.Windows.Forms.Label lPacketInfo_Encoding_Unicode;
        private System.Windows.Forms.TextBox txtPacketInfo_Encoding_Unicode;
        private System.Windows.Forms.TextBox txtPacketInfo_Encoding_ANSIUTF16;
        private System.Windows.Forms.Label lPacketInfo_Encoding_ANSIASCII;
        private System.Windows.Forms.Label lPacketInfo_Encoding_ANSIUTF32;
        private System.Windows.Forms.TextBox txtPacketInfo_Encoding_ANSIUTF32;
        private System.Windows.Forms.Label lPacketInfo_Encoding_ANSIUnicode;
        private System.Windows.Forms.TextBox txtPacketInfo_Encoding_ANSIUnicode;
        private System.Windows.Forms.RadioButton rbFromHead;
        private System.Windows.Forms.RadioButton rbFromIndex;
        private System.Windows.Forms.ToolStripSeparator cmsHexBox_tss3;
        private System.Windows.Forms.ToolStripMenuItem cmsHexBox_CopyHex;
        private System.Windows.Forms.ToolStripMenuItem cmsHexBox_CopyText;
        private System.Windows.Forms.TabPage tpXOR;
        private System.Windows.Forms.TableLayoutPanel tlpPacketInfo_XOR;
        private System.Windows.Forms.TableLayoutPanel tlpPacketInfo_XOR_Button;
        private Be.Windows.Forms.HexBox hbXOR_From;
        private System.Windows.Forms.Button bXOR;
        private System.Windows.Forms.Button bXOR_Clear;
        private System.Windows.Forms.Label lXOR;
        private System.Windows.Forms.TextBox txtXOR;
        private Be.Windows.Forms.HexBox hbXOR_To;
        private System.Windows.Forms.Label lXOR2;
        private System.Windows.Forms.ToolStripSeparator cmsHexBox_tss4;
        private System.Windows.Forms.ToolStripMenuItem cmsHexBox_SelectAll;
        private System.Windows.Forms.Panel pComparison_A;
        private System.Windows.Forms.RichTextBox rtbComparison_A;
        private System.Windows.Forms.Panel pComparison_B;
        private System.Windows.Forms.RichTextBox rtbComparison_B;
        private System.Windows.Forms.Panel pComparison_Result;
        private System.Windows.Forms.RichTextBox rtbComparison_Result;
        private System.Windows.Forms.Panel pPacketInfo_Encoding;
        private System.Windows.Forms.RichTextBox rtbPacketInfo_Encoding;
        private System.Windows.Forms.ContextMenuStrip cmsFilterList;
        private System.Windows.Forms.ToolStripMenuItem cmsFilterList_Up;
        private System.Windows.Forms.ToolStripMenuItem cmsFilterList_Down;
        private System.Windows.Forms.ToolStripMenuItem cmsFilterList_Top;
        private System.Windows.Forms.ToolStripSeparator cmsFilterList_tss1;
        private System.Windows.Forms.ToolStripSeparator cmsFilterList_tss2;
        private System.Windows.Forms.ToolStripMenuItem cmsFilterList_Bottom;
        private System.Windows.Forms.ToolStripSeparator cmsFilterList_tss3;
        private System.Windows.Forms.ToolStripMenuItem cmsFilterList_Copy;
        private System.Windows.Forms.ToolStripMenuItem cmsFilterList_Delete;
        private System.Windows.Forms.ToolStripSeparator cmsHexBox_tss5;
        private System.Windows.Forms.ToolStripMenuItem cmsHexBox_Comparison_A;
        private System.Windows.Forms.ToolStripMenuItem cmsHexBox_Comparison_B;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel14;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel17;
        private System.Windows.Forms.ToolStripStatusLabel tlFilterExecute;
        private System.Windows.Forms.ToolStripStatusLabel tlFilterExecute_CNT;
        private System.Windows.Forms.TabControl tcSocketInfo;
        private System.Windows.Forms.TabPage tcSocketInfo_FilterSet;
        private System.Windows.Forms.TabPage tcSocketInfo_HookSet;
        private System.Windows.Forms.TableLayoutPanel tlpFilterSet;
        private System.Windows.Forms.CheckBox cbCheckSize;
        private System.Windows.Forms.RadioButton rbFilter_Show;
        private System.Windows.Forms.RadioButton rbFilter_NotShow;
        private System.Windows.Forms.TextBox txtCheckPort;
        private System.Windows.Forms.CheckBox cbCheckPort;
        private System.Windows.Forms.TextBox txtCheckData;
        private System.Windows.Forms.CheckBox cbCheckData;
        private System.Windows.Forms.CheckBox cbCheckSocket;
        private System.Windows.Forms.CheckBox cbCheckIP;
        private System.Windows.Forms.TextBox txtCheckSocket;
        private System.Windows.Forms.TextBox txtCheckIP;
        private System.Windows.Forms.TabPage tcSocketInfo_SystemSet;
        private System.Windows.Forms.TabPage tcSocketInfo_ListSet;
        private System.Windows.Forms.Timer tSocketList;
        private System.Windows.Forms.TableLayoutPanel tlpListSet;
        private System.Windows.Forms.GroupBox gbListSet_SocketList;
        private System.Windows.Forms.TableLayoutPanel tlpListSet_SocketList;
        private System.Windows.Forms.CheckBox cbSocketList_AutoRoll;
        private System.Windows.Forms.CheckBox cbSocketList_AutoClear;
        private System.Windows.Forms.NumericUpDown nudSocketList_AutoClearValue;
        private System.Windows.Forms.GroupBox gbListSet_LogList;
        private System.Windows.Forms.TableLayoutPanel tlpListSet_LogList;
        private System.Windows.Forms.NumericUpDown nudLogList_AutoClearValue;
        private System.Windows.Forms.CheckBox cbLogList_AutoClear;
        private System.Windows.Forms.CheckBox cbLogList_AutoRoll;
        private System.Windows.Forms.TableLayoutPanel tlpSystemSet;
        private System.Windows.Forms.GroupBox gbSystemSet_WorkMode;
        private System.Windows.Forms.TableLayoutPanel tlpSystemSet_WorkMode;
        private System.Windows.Forms.CheckBox cbWorkingMode_Speed;
        private System.Windows.Forms.GroupBox gbSystemSet_FilterSet;
        private System.Windows.Forms.TableLayoutPanel tlpSystemSet_FilterSet;
        private System.Windows.Forms.RadioButton rbFilterSet_Priority;
        private System.Windows.Forms.RadioButton rbFilterSet_Sequence;
        private System.Windows.Forms.TableLayoutPanel tlpHookSet;
        private System.Windows.Forms.GroupBox gbHookSet_Winsock;
        private System.Windows.Forms.TableLayoutPanel tlpHookSet_Winsock;
        private System.Windows.Forms.GroupBox gbHookSet_WinsockWSA;
        private System.Windows.Forms.TableLayoutPanel tlpHookSet_WinsockWSA;
        private System.Windows.Forms.CheckBox cbHookWSA_Send;
        private System.Windows.Forms.CheckBox cbHookWSA_SendTo;
        private System.Windows.Forms.CheckBox cbHookWSA_Recv;
        private System.Windows.Forms.CheckBox cbHookWSA_RecvFrom;
        private System.Windows.Forms.TabPage tpExtraction;
        private System.Windows.Forms.TableLayoutPanel tlpExtraction;
        private System.Windows.Forms.PictureBox pbEncoding;
        private System.Windows.Forms.ToolStripMenuItem cmsFilterList_Export;
        private System.Windows.Forms.CheckBox cbCheckHead;
        private System.Windows.Forms.TextBox txtCheckHead;
        private System.Windows.Forms.Panel pExtraction;
        private System.Windows.Forms.RichTextBox rtbExtraction;
        private System.Windows.Forms.TableLayoutPanel tlpExtraction_Button;
        private System.Windows.Forms.Button bExtraction;
        private System.Windows.Forms.ComboBox cbbExtraction;
        private System.Windows.Forms.OpenFileDialog ofdExtraction;
        private System.Windows.Forms.TextBox txtCheckLength;
        private System.Windows.Forms.ContextMenuStrip cmsExtraction;
        private System.Windows.Forms.ToolStripMenuItem cmsExtraction_Export;
        private System.Windows.Forms.SaveFileDialog sfdExtraction;
        private System.Windows.Forms.TabControl tcAutomation;
        private System.Windows.Forms.TabPage tpFilterList;
        private System.Windows.Forms.TabPage tpRobotList;
        private System.Windows.Forms.TableLayoutPanel tlpFilterList;
        private System.Windows.Forms.DataGridView dgvFilterList;
        private System.Windows.Forms.ToolStrip tsFilterList;
        private System.Windows.Forms.ToolStripButton tsFilterList_Load;
        private System.Windows.Forms.ToolStripButton tsFilterList_Save;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsFilterList_SelectAll;
        private System.Windows.Forms.ToolStripButton tsFilterList_SelectNo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsFilterList_Add;
        private System.Windows.Forms.ToolStripButton tsFilterList_CleanUp;
        private System.Windows.Forms.TableLayoutPanel tlpRobotList;
        private System.Windows.Forms.DataGridView dgvRobotList;
        private System.Windows.Forms.ToolStrip tsRobotList;
        private System.Windows.Forms.ToolStripButton tsRobotList_Load;
        private System.Windows.Forms.ToolStripButton tsRobotList_Save;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton tsRobotList_Add;
        private System.Windows.Forms.ToolStripButton tsRobotList_CleanUp;
        private System.Windows.Forms.ContextMenuStrip cmsRobotList;
        private System.Windows.Forms.ToolStripMenuItem cmsRobotList_Delete;
        private System.Windows.Forms.ToolStripMenuItem cmsSocketList_FilterList;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem cmsRobotList_Copy;
        private System.Windows.Forms.ToolStripMenuItem cmsRobotList_Export;
        private System.Windows.Forms.TabPage tcSocketInfo_HotKey;
        private System.Windows.Forms.TableLayoutPanel tlpHotKeySet;
        private System.Windows.Forms.Label lHotKey_RobotList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem cmsRobotList_Top;
        private System.Windows.Forms.ToolStripSeparator cmsRobotList_Split1;
        private System.Windows.Forms.ToolStripMenuItem cmsRobotList_Up;
        private System.Windows.Forms.ToolStripMenuItem cmsRobotList_Down;
        private System.Windows.Forms.ToolStripSeparator cmsRobotList_Split2;
        private System.Windows.Forms.ToolStripMenuItem cmsRobotList_Bottom;
        private System.Windows.Forms.ToolStripSeparator cmsRobotList_Split3;
        private System.Windows.Forms.ToolStripMenuItem cmsSocketList_ShowModified;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLogID;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLogTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn cFuncName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLogContent;
        private System.Windows.Forms.GroupBox gbSystemSet_ShowMode;
        private System.Windows.Forms.TableLayoutPanel tlpSystemSet_ShowMode;
        private System.Windows.Forms.CheckBox cbTopMost;
        private System.Windows.Forms.CheckBox cbHookWS2_RecvFrom;
        private System.Windows.Forms.CheckBox cbHookWS2_Recv;
        private System.Windows.Forms.CheckBox cbHookWS2_SendTo;
        private System.Windows.Forms.CheckBox cbHookWS2_Send;
        private System.Windows.Forms.CheckBox cbHookWS1_RecvFrom;
        private System.Windows.Forms.CheckBox cbHookWS1_Recv;
        private System.Windows.Forms.CheckBox cbHookWS1_SendTo;
        private System.Windows.Forms.CheckBox cbHookWS1_Send;
        private System.Windows.Forms.ToolStripStatusLabel tsslSplit2;
        private System.Windows.Forms.ToolStripStatusLabel tsslProcessInfo;
        private System.Windows.Forms.TabPage tpSendList;
        private System.Windows.Forms.TableLayoutPanel tlpSendList;
        private System.Windows.Forms.DataGridView dgvSendList;
        private System.Windows.Forms.ToolStrip tsSendList;
        private System.Windows.Forms.ToolStripButton tsSendList_Load;
        private System.Windows.Forms.ToolStripButton tsSendList_Save;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripButton tsSendList_Add;
        private System.Windows.Forms.ToolStripButton tsSendList_CleanUp;
        private System.Windows.Forms.ContextMenuStrip cmsSendList;
        private System.Windows.Forms.ToolStripMenuItem cmsSendList_Top;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem cmsSendList_Up;
        private System.Windows.Forms.ToolStripMenuItem cmsSendList_Down;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem cmsSendList_Bottom;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem cmsSendList_Copy;
        private System.Windows.Forms.ToolStripMenuItem cmsSendList_Export;
        private System.Windows.Forms.ToolStripMenuItem cmsSendList_Delete;
        private System.Windows.Forms.ToolStripMenuItem cmsSocketList_SystemSocket;
        private System.Windows.Forms.ToolStripSeparator tss6;
        private System.Windows.Forms.ToolStripMenuItem cmsSocketList_SendList;
        private System.Windows.Forms.ToolStripComboBox tscbSendList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripMenuItem cmsHexBox_SendList;
        private System.Windows.Forms.ToolStripComboBox cmsHexBox_tscbSendList;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cIsCheck;
        private System.Windows.Forms.DataGridViewTextBoxColumn cFName;
        private System.Windows.Forms.DataGridViewImageColumn cTypeImg;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPacketID;
        private System.Windows.Forms.DataGridViewTextBoxColumn cPacketType;
        private System.Windows.Forms.DataGridViewTextBoxColumn cSocket;
        private System.Windows.Forms.DataGridViewTextBoxColumn cFrom;
        private System.Windows.Forms.DataGridViewTextBoxColumn cTo;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLen;
        private System.Windows.Forms.DataGridViewTextBoxColumn cData;
        private System.Windows.Forms.NotifyIcon niWPE;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cIsEnable;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.ToolStripButton tsSendList_Start;
        private System.Windows.Forms.ToolStripButton tsSendList_Stop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.ComponentModel.BackgroundWorker bgwSendList;
        private System.Windows.Forms.ToolStripButton tsRobotList_Start;
        private System.Windows.Forms.ToolStripButton tsRobotList_Stop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.DataGridViewCheckBoxColumn cRIsEnable;
        private System.Windows.Forms.DataGridViewImageColumn cImg;
        private System.Windows.Forms.DataGridViewTextBoxColumn cRName;
        private System.ComponentModel.BackgroundWorker bgwRobotList;
    }
}