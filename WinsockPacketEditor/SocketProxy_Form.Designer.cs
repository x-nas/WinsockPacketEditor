namespace WinsockPacketEditor
{
    partial class SocketProxy_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SocketProxy_Form));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlProxyTotal = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlpSocketProxy_Parameter = new System.Windows.Forms.TableLayoutPanel();
            this.gbSystemProxy = new System.Windows.Forms.GroupBox();
            this.tlpSystemProxy = new System.Windows.Forms.TableLayoutPanel();
            this.cbEnable_SystemProxy = new System.Windows.Forms.CheckBox();
            this.tcSocketProxySet = new System.Windows.Forms.TabControl();
            this.tpProxySet = new System.Windows.Forms.TabPage();
            this.tlpProxySet = new System.Windows.Forms.TableLayoutPanel();
            this.gbProxyIP = new System.Windows.Forms.GroupBox();
            this.tlpProxyIP = new System.Windows.Forms.TableLayoutPanel();
            this.cbbProxyIP_Appoint = new System.Windows.Forms.ComboBox();
            this.cbProxyIP_Auto = new System.Windows.Forms.CheckBox();
            this.gbProxyType = new System.Windows.Forms.GroupBox();
            this.tlpProxyType = new System.Windows.Forms.TableLayoutPanel();
            this.nudProxyPort = new System.Windows.Forms.NumericUpDown();
            this.cbEnable_SOCKS5 = new System.Windows.Forms.CheckBox();
            this.lSOCKS_Port = new System.Windows.Forms.Label();
            this.tpAuthSet = new System.Windows.Forms.TabPage();
            this.tlpAuthSet = new System.Windows.Forms.TableLayoutPanel();
            this.gbAuthSet = new System.Windows.Forms.GroupBox();
            this.tlpAuthSet_Auth = new System.Windows.Forms.TableLayoutPanel();
            this.txtAuth_PassWord = new System.Windows.Forms.TextBox();
            this.lAuth_PassWord = new System.Windows.Forms.Label();
            this.cbEnable_Auth = new System.Windows.Forms.CheckBox();
            this.lAuth_UserName = new System.Windows.Forms.Label();
            this.txtAuth_UserName = new System.Windows.Forms.TextBox();
            this.tpListSet = new System.Windows.Forms.TabPage();
            this.tlpListSet = new System.Windows.Forms.TableLayoutPanel();
            this.gbClientList = new System.Windows.Forms.GroupBox();
            this.tlpClientList = new System.Windows.Forms.TableLayoutPanel();
            this.cbDeleteClosed = new System.Windows.Forms.CheckBox();
            this.gbProxyList = new System.Windows.Forms.GroupBox();
            this.tlpProxyList = new System.Windows.Forms.TableLayoutPanel();
            this.cbNoRecordData = new System.Windows.Forms.CheckBox();
            this.gbListSet_LogList = new System.Windows.Forms.GroupBox();
            this.tlpListSet_LogList = new System.Windows.Forms.TableLayoutPanel();
            this.nudLogList_AutoClearValue = new System.Windows.Forms.NumericUpDown();
            this.cbLogList_AutoClear = new System.Windows.Forms.CheckBox();
            this.cbLogList_AutoRoll = new System.Windows.Forms.CheckBox();
            this.tpExternalProxy = new System.Windows.Forms.TabPage();
            this.tlpExternalProxy = new System.Windows.Forms.TableLayoutPanel();
            this.lHttpsProxy = new System.Windows.Forms.Label();
            this.txtEXTHttpsPort = new System.Windows.Forms.TextBox();
            this.txtEXTHttpsIP = new System.Windows.Forms.TextBox();
            this.txtEXTHttpPort = new System.Windows.Forms.TextBox();
            this.cbEnable_EXTHttps = new System.Windows.Forms.CheckBox();
            this.cbEnable_EXTHttp = new System.Windows.Forms.CheckBox();
            this.txtEXTHttpIP = new System.Windows.Forms.TextBox();
            this.lHttpProxy = new System.Windows.Forms.Label();
            this.lHttpPort = new System.Windows.Forms.Label();
            this.lHttpsPort = new System.Windows.Forms.Label();
            this.txtAppointHttpPort = new System.Windows.Forms.TextBox();
            this.txtAppointHttpsPort = new System.Windows.Forms.TextBox();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bCleanUp = new System.Windows.Forms.Button();
            this.tlpButton_Start = new System.Windows.Forms.TableLayoutPanel();
            this.bStop = new System.Windows.Forms.Button();
            this.bStart = new System.Windows.Forms.Button();
            this.tvProxyData = new System.Windows.Forms.TreeView();
            this.ilSocketProxy = new System.Windows.Forms.ImageList(this.components);
            this.tpProxyList = new System.Windows.Forms.TabPage();
            this.tcProxyInfo = new System.Windows.Forms.TabControl();
            this.ssSocketProxy = new System.Windows.Forms.StatusStrip();
            this.tlProxyTotal_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlSplit1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlProxyTCP = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlProxyTCP_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlProxyUDP = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlProxyUDP_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel6 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlProxyCache = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlProxyCache_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlProxyLinks = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlProxyLinks_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslTotalBytes = new System.Windows.Forms.ToolStripStatusLabel();
            this.tcProxyData = new System.Windows.Forms.TabControl();
            this.tpData = new System.Windows.Forms.TabPage();
            this.hbData = new Be.Windows.Forms.HexBox();
            this.tlpSocketProxy_Data = new System.Windows.Forms.TableLayoutPanel();
            this.tcSocketProxy_Log = new System.Windows.Forms.TabControl();
            this.tpLog = new System.Windows.Forms.TabPage();
            this.dgvLogList = new System.Windows.Forms.DataGridView();
            this.cLogID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLogTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cFuncName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLogContent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tcClientInfo = new System.Windows.Forms.TabControl();
            this.tpClientList = new System.Windows.Forms.TabPage();
            this.tvProxyInfo = new System.Windows.Forms.TreeView();
            this.tlpSocketProxy = new System.Windows.Forms.TableLayoutPanel();
            this.tSocketProxy = new System.Windows.Forms.Timer(this.components);
            this.tCheckProxyState = new System.Windows.Forms.Timer(this.components);
            this.tlpSocketProxy_Parameter.SuspendLayout();
            this.gbSystemProxy.SuspendLayout();
            this.tlpSystemProxy.SuspendLayout();
            this.tcSocketProxySet.SuspendLayout();
            this.tpProxySet.SuspendLayout();
            this.tlpProxySet.SuspendLayout();
            this.gbProxyIP.SuspendLayout();
            this.tlpProxyIP.SuspendLayout();
            this.gbProxyType.SuspendLayout();
            this.tlpProxyType.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudProxyPort)).BeginInit();
            this.tpAuthSet.SuspendLayout();
            this.tlpAuthSet.SuspendLayout();
            this.gbAuthSet.SuspendLayout();
            this.tlpAuthSet_Auth.SuspendLayout();
            this.tpListSet.SuspendLayout();
            this.tlpListSet.SuspendLayout();
            this.gbClientList.SuspendLayout();
            this.tlpClientList.SuspendLayout();
            this.gbProxyList.SuspendLayout();
            this.tlpProxyList.SuspendLayout();
            this.gbListSet_LogList.SuspendLayout();
            this.tlpListSet_LogList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLogList_AutoClearValue)).BeginInit();
            this.tpExternalProxy.SuspendLayout();
            this.tlpExternalProxy.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpButton_Start.SuspendLayout();
            this.tpProxyList.SuspendLayout();
            this.tcProxyInfo.SuspendLayout();
            this.ssSocketProxy.SuspendLayout();
            this.tcProxyData.SuspendLayout();
            this.tpData.SuspendLayout();
            this.tlpSocketProxy_Data.SuspendLayout();
            this.tcSocketProxy_Log.SuspendLayout();
            this.tpLog.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogList)).BeginInit();
            this.tcClientInfo.SuspendLayout();
            this.tpClientList.SuspendLayout();
            this.tlpSocketProxy.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlProxyTotal
            // 
            this.tlProxyTotal.Name = "tlProxyTotal";
            resources.ApplyResources(this.tlProxyTotal, "tlProxyTotal");
            // 
            // tlpSocketProxy_Parameter
            // 
            resources.ApplyResources(this.tlpSocketProxy_Parameter, "tlpSocketProxy_Parameter");
            this.tlpSocketProxy_Parameter.Controls.Add(this.gbSystemProxy, 1, 0);
            this.tlpSocketProxy_Parameter.Controls.Add(this.tcSocketProxySet, 0, 0);
            this.tlpSocketProxy_Parameter.Controls.Add(this.tlpButton, 2, 0);
            this.tlpSocketProxy_Parameter.Name = "tlpSocketProxy_Parameter";
            // 
            // gbSystemProxy
            // 
            this.gbSystemProxy.Controls.Add(this.tlpSystemProxy);
            resources.ApplyResources(this.gbSystemProxy, "gbSystemProxy");
            this.gbSystemProxy.Name = "gbSystemProxy";
            this.gbSystemProxy.TabStop = false;
            // 
            // tlpSystemProxy
            // 
            resources.ApplyResources(this.tlpSystemProxy, "tlpSystemProxy");
            this.tlpSystemProxy.Controls.Add(this.cbEnable_SystemProxy, 1, 1);
            this.tlpSystemProxy.Name = "tlpSystemProxy";
            // 
            // cbEnable_SystemProxy
            // 
            resources.ApplyResources(this.cbEnable_SystemProxy, "cbEnable_SystemProxy");
            this.cbEnable_SystemProxy.Name = "cbEnable_SystemProxy";
            this.cbEnable_SystemProxy.UseVisualStyleBackColor = true;
            this.cbEnable_SystemProxy.CheckedChanged += new System.EventHandler(this.cbEnable_SystemProxy_CheckedChanged);
            // 
            // tcSocketProxySet
            // 
            this.tcSocketProxySet.Controls.Add(this.tpProxySet);
            this.tcSocketProxySet.Controls.Add(this.tpAuthSet);
            this.tcSocketProxySet.Controls.Add(this.tpListSet);
            this.tcSocketProxySet.Controls.Add(this.tpExternalProxy);
            resources.ApplyResources(this.tcSocketProxySet, "tcSocketProxySet");
            this.tcSocketProxySet.Name = "tcSocketProxySet";
            this.tcSocketProxySet.SelectedIndex = 0;
            // 
            // tpProxySet
            // 
            this.tpProxySet.BackColor = System.Drawing.SystemColors.Control;
            this.tpProxySet.Controls.Add(this.tlpProxySet);
            resources.ApplyResources(this.tpProxySet, "tpProxySet");
            this.tpProxySet.Name = "tpProxySet";
            // 
            // tlpProxySet
            // 
            resources.ApplyResources(this.tlpProxySet, "tlpProxySet");
            this.tlpProxySet.Controls.Add(this.gbProxyIP, 0, 0);
            this.tlpProxySet.Controls.Add(this.gbProxyType, 1, 0);
            this.tlpProxySet.Name = "tlpProxySet";
            // 
            // gbProxyIP
            // 
            this.gbProxyIP.Controls.Add(this.tlpProxyIP);
            resources.ApplyResources(this.gbProxyIP, "gbProxyIP");
            this.gbProxyIP.Name = "gbProxyIP";
            this.gbProxyIP.TabStop = false;
            // 
            // tlpProxyIP
            // 
            resources.ApplyResources(this.tlpProxyIP, "tlpProxyIP");
            this.tlpProxyIP.Controls.Add(this.cbbProxyIP_Appoint, 1, 1);
            this.tlpProxyIP.Controls.Add(this.cbProxyIP_Auto, 0, 1);
            this.tlpProxyIP.Name = "tlpProxyIP";
            // 
            // cbbProxyIP_Appoint
            // 
            resources.ApplyResources(this.cbbProxyIP_Appoint, "cbbProxyIP_Appoint");
            this.cbbProxyIP_Appoint.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbProxyIP_Appoint.FormattingEnabled = true;
            this.cbbProxyIP_Appoint.Name = "cbbProxyIP_Appoint";
            // 
            // cbProxyIP_Auto
            // 
            resources.ApplyResources(this.cbProxyIP_Auto, "cbProxyIP_Auto");
            this.cbProxyIP_Auto.Checked = true;
            this.cbProxyIP_Auto.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbProxyIP_Auto.Name = "cbProxyIP_Auto";
            this.cbProxyIP_Auto.UseVisualStyleBackColor = true;
            this.cbProxyIP_Auto.CheckedChanged += new System.EventHandler(this.cbProxyIP_Auto_CheckedChanged);
            // 
            // gbProxyType
            // 
            this.gbProxyType.Controls.Add(this.tlpProxyType);
            resources.ApplyResources(this.gbProxyType, "gbProxyType");
            this.gbProxyType.Name = "gbProxyType";
            this.gbProxyType.TabStop = false;
            // 
            // tlpProxyType
            // 
            resources.ApplyResources(this.tlpProxyType, "tlpProxyType");
            this.tlpProxyType.Controls.Add(this.nudProxyPort, 2, 1);
            this.tlpProxyType.Controls.Add(this.cbEnable_SOCKS5, 0, 1);
            this.tlpProxyType.Controls.Add(this.lSOCKS_Port, 1, 1);
            this.tlpProxyType.Name = "tlpProxyType";
            // 
            // nudProxyPort
            // 
            this.nudProxyPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.nudProxyPort, "nudProxyPort");
            this.nudProxyPort.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.nudProxyPort.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudProxyPort.Name = "nudProxyPort";
            this.nudProxyPort.Value = new decimal(new int[] {
            8899,
            0,
            0,
            0});
            // 
            // cbEnable_SOCKS5
            // 
            resources.ApplyResources(this.cbEnable_SOCKS5, "cbEnable_SOCKS5");
            this.cbEnable_SOCKS5.Checked = true;
            this.cbEnable_SOCKS5.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnable_SOCKS5.Name = "cbEnable_SOCKS5";
            this.cbEnable_SOCKS5.UseVisualStyleBackColor = true;
            // 
            // lSOCKS_Port
            // 
            resources.ApplyResources(this.lSOCKS_Port, "lSOCKS_Port");
            this.lSOCKS_Port.Name = "lSOCKS_Port";
            // 
            // tpAuthSet
            // 
            this.tpAuthSet.BackColor = System.Drawing.SystemColors.Control;
            this.tpAuthSet.Controls.Add(this.tlpAuthSet);
            resources.ApplyResources(this.tpAuthSet, "tpAuthSet");
            this.tpAuthSet.Name = "tpAuthSet";
            // 
            // tlpAuthSet
            // 
            resources.ApplyResources(this.tlpAuthSet, "tlpAuthSet");
            this.tlpAuthSet.Controls.Add(this.gbAuthSet, 0, 0);
            this.tlpAuthSet.Name = "tlpAuthSet";
            // 
            // gbAuthSet
            // 
            this.gbAuthSet.Controls.Add(this.tlpAuthSet_Auth);
            resources.ApplyResources(this.gbAuthSet, "gbAuthSet");
            this.gbAuthSet.Name = "gbAuthSet";
            this.gbAuthSet.TabStop = false;
            // 
            // tlpAuthSet_Auth
            // 
            resources.ApplyResources(this.tlpAuthSet_Auth, "tlpAuthSet_Auth");
            this.tlpAuthSet_Auth.Controls.Add(this.txtAuth_PassWord, 4, 1);
            this.tlpAuthSet_Auth.Controls.Add(this.lAuth_PassWord, 3, 1);
            this.tlpAuthSet_Auth.Controls.Add(this.cbEnable_Auth, 0, 1);
            this.tlpAuthSet_Auth.Controls.Add(this.lAuth_UserName, 1, 1);
            this.tlpAuthSet_Auth.Controls.Add(this.txtAuth_UserName, 2, 1);
            this.tlpAuthSet_Auth.Name = "tlpAuthSet_Auth";
            // 
            // txtAuth_PassWord
            // 
            this.txtAuth_PassWord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.txtAuth_PassWord, "txtAuth_PassWord");
            this.txtAuth_PassWord.Name = "txtAuth_PassWord";
            // 
            // lAuth_PassWord
            // 
            resources.ApplyResources(this.lAuth_PassWord, "lAuth_PassWord");
            this.lAuth_PassWord.Name = "lAuth_PassWord";
            // 
            // cbEnable_Auth
            // 
            resources.ApplyResources(this.cbEnable_Auth, "cbEnable_Auth");
            this.cbEnable_Auth.Name = "cbEnable_Auth";
            this.cbEnable_Auth.UseVisualStyleBackColor = true;
            this.cbEnable_Auth.CheckedChanged += new System.EventHandler(this.cbEnable_Auth_CheckedChanged);
            // 
            // lAuth_UserName
            // 
            resources.ApplyResources(this.lAuth_UserName, "lAuth_UserName");
            this.lAuth_UserName.Name = "lAuth_UserName";
            // 
            // txtAuth_UserName
            // 
            this.txtAuth_UserName.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.txtAuth_UserName, "txtAuth_UserName");
            this.txtAuth_UserName.Name = "txtAuth_UserName";
            // 
            // tpListSet
            // 
            this.tpListSet.BackColor = System.Drawing.SystemColors.Control;
            this.tpListSet.Controls.Add(this.tlpListSet);
            resources.ApplyResources(this.tpListSet, "tpListSet");
            this.tpListSet.Name = "tpListSet";
            // 
            // tlpListSet
            // 
            resources.ApplyResources(this.tlpListSet, "tlpListSet");
            this.tlpListSet.Controls.Add(this.gbClientList, 1, 0);
            this.tlpListSet.Controls.Add(this.gbProxyList, 0, 0);
            this.tlpListSet.Controls.Add(this.gbListSet_LogList, 2, 0);
            this.tlpListSet.Name = "tlpListSet";
            // 
            // gbClientList
            // 
            this.gbClientList.Controls.Add(this.tlpClientList);
            resources.ApplyResources(this.gbClientList, "gbClientList");
            this.gbClientList.Name = "gbClientList";
            this.gbClientList.TabStop = false;
            // 
            // tlpClientList
            // 
            resources.ApplyResources(this.tlpClientList, "tlpClientList");
            this.tlpClientList.Controls.Add(this.cbDeleteClosed, 0, 1);
            this.tlpClientList.Name = "tlpClientList";
            // 
            // cbDeleteClosed
            // 
            resources.ApplyResources(this.cbDeleteClosed, "cbDeleteClosed");
            this.cbDeleteClosed.Name = "cbDeleteClosed";
            this.cbDeleteClosed.UseVisualStyleBackColor = true;
            // 
            // gbProxyList
            // 
            this.gbProxyList.Controls.Add(this.tlpProxyList);
            resources.ApplyResources(this.gbProxyList, "gbProxyList");
            this.gbProxyList.Name = "gbProxyList";
            this.gbProxyList.TabStop = false;
            // 
            // tlpProxyList
            // 
            resources.ApplyResources(this.tlpProxyList, "tlpProxyList");
            this.tlpProxyList.Controls.Add(this.cbNoRecordData, 0, 1);
            this.tlpProxyList.Name = "tlpProxyList";
            // 
            // cbNoRecordData
            // 
            resources.ApplyResources(this.cbNoRecordData, "cbNoRecordData");
            this.cbNoRecordData.Name = "cbNoRecordData";
            this.cbNoRecordData.UseVisualStyleBackColor = true;
            // 
            // gbListSet_LogList
            // 
            this.gbListSet_LogList.Controls.Add(this.tlpListSet_LogList);
            resources.ApplyResources(this.gbListSet_LogList, "gbListSet_LogList");
            this.gbListSet_LogList.Name = "gbListSet_LogList";
            this.gbListSet_LogList.TabStop = false;
            // 
            // tlpListSet_LogList
            // 
            resources.ApplyResources(this.tlpListSet_LogList, "tlpListSet_LogList");
            this.tlpListSet_LogList.Controls.Add(this.nudLogList_AutoClearValue, 2, 1);
            this.tlpListSet_LogList.Controls.Add(this.cbLogList_AutoClear, 1, 1);
            this.tlpListSet_LogList.Controls.Add(this.cbLogList_AutoRoll, 0, 1);
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
            this.cbLogList_AutoRoll.Checked = true;
            this.cbLogList_AutoRoll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbLogList_AutoRoll.Name = "cbLogList_AutoRoll";
            this.cbLogList_AutoRoll.UseVisualStyleBackColor = true;
            // 
            // tpExternalProxy
            // 
            this.tpExternalProxy.BackColor = System.Drawing.SystemColors.Control;
            this.tpExternalProxy.Controls.Add(this.tlpExternalProxy);
            resources.ApplyResources(this.tpExternalProxy, "tpExternalProxy");
            this.tpExternalProxy.Name = "tpExternalProxy";
            // 
            // tlpExternalProxy
            // 
            resources.ApplyResources(this.tlpExternalProxy, "tlpExternalProxy");
            this.tlpExternalProxy.Controls.Add(this.lHttpsProxy, 2, 2);
            this.tlpExternalProxy.Controls.Add(this.txtEXTHttpsPort, 3, 2);
            this.tlpExternalProxy.Controls.Add(this.txtEXTHttpsIP, 1, 2);
            this.tlpExternalProxy.Controls.Add(this.txtEXTHttpPort, 3, 1);
            this.tlpExternalProxy.Controls.Add(this.cbEnable_EXTHttps, 0, 2);
            this.tlpExternalProxy.Controls.Add(this.cbEnable_EXTHttp, 0, 1);
            this.tlpExternalProxy.Controls.Add(this.txtEXTHttpIP, 1, 1);
            this.tlpExternalProxy.Controls.Add(this.lHttpProxy, 2, 1);
            this.tlpExternalProxy.Controls.Add(this.lHttpPort, 4, 1);
            this.tlpExternalProxy.Controls.Add(this.lHttpsPort, 4, 2);
            this.tlpExternalProxy.Controls.Add(this.txtAppointHttpPort, 5, 1);
            this.tlpExternalProxy.Controls.Add(this.txtAppointHttpsPort, 5, 2);
            this.tlpExternalProxy.Name = "tlpExternalProxy";
            // 
            // lHttpsProxy
            // 
            resources.ApplyResources(this.lHttpsProxy, "lHttpsProxy");
            this.lHttpsProxy.Name = "lHttpsProxy";
            // 
            // txtEXTHttpsPort
            // 
            this.txtEXTHttpsPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.txtEXTHttpsPort, "txtEXTHttpsPort");
            this.txtEXTHttpsPort.Name = "txtEXTHttpsPort";
            // 
            // txtEXTHttpsIP
            // 
            this.txtEXTHttpsIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.txtEXTHttpsIP, "txtEXTHttpsIP");
            this.txtEXTHttpsIP.Name = "txtEXTHttpsIP";
            // 
            // txtEXTHttpPort
            // 
            this.txtEXTHttpPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.txtEXTHttpPort, "txtEXTHttpPort");
            this.txtEXTHttpPort.Name = "txtEXTHttpPort";
            // 
            // cbEnable_EXTHttps
            // 
            resources.ApplyResources(this.cbEnable_EXTHttps, "cbEnable_EXTHttps");
            this.cbEnable_EXTHttps.Name = "cbEnable_EXTHttps";
            this.cbEnable_EXTHttps.UseVisualStyleBackColor = true;
            this.cbEnable_EXTHttps.CheckedChanged += new System.EventHandler(this.cbEnableHttpsProxy_CheckedChanged);
            // 
            // cbEnable_EXTHttp
            // 
            resources.ApplyResources(this.cbEnable_EXTHttp, "cbEnable_EXTHttp");
            this.cbEnable_EXTHttp.Name = "cbEnable_EXTHttp";
            this.cbEnable_EXTHttp.UseVisualStyleBackColor = true;
            this.cbEnable_EXTHttp.CheckedChanged += new System.EventHandler(this.cbEnableHttpProxy_CheckedChanged);
            // 
            // txtEXTHttpIP
            // 
            this.txtEXTHttpIP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.txtEXTHttpIP, "txtEXTHttpIP");
            this.txtEXTHttpIP.Name = "txtEXTHttpIP";
            // 
            // lHttpProxy
            // 
            resources.ApplyResources(this.lHttpProxy, "lHttpProxy");
            this.lHttpProxy.Name = "lHttpProxy";
            // 
            // lHttpPort
            // 
            resources.ApplyResources(this.lHttpPort, "lHttpPort");
            this.lHttpPort.Name = "lHttpPort";
            // 
            // lHttpsPort
            // 
            resources.ApplyResources(this.lHttpsPort, "lHttpsPort");
            this.lHttpsPort.Name = "lHttpsPort";
            // 
            // txtAppointHttpPort
            // 
            this.txtAppointHttpPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.txtAppointHttpPort, "txtAppointHttpPort");
            this.txtAppointHttpPort.Name = "txtAppointHttpPort";
            // 
            // txtAppointHttpsPort
            // 
            this.txtAppointHttpsPort.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.txtAppointHttpsPort, "txtAppointHttpsPort");
            this.txtAppointHttpsPort.Name = "txtAppointHttpsPort";
            // 
            // tlpButton
            // 
            resources.ApplyResources(this.tlpButton, "tlpButton");
            this.tlpButton.Controls.Add(this.bCleanUp, 1, 1);
            this.tlpButton.Controls.Add(this.tlpButton_Start, 2, 1);
            this.tlpButton.Name = "tlpButton";
            // 
            // bCleanUp
            // 
            resources.ApplyResources(this.bCleanUp, "bCleanUp");
            this.bCleanUp.Name = "bCleanUp";
            this.bCleanUp.UseVisualStyleBackColor = true;
            this.bCleanUp.Click += new System.EventHandler(this.bCleanUp_Click);
            // 
            // tlpButton_Start
            // 
            resources.ApplyResources(this.tlpButton_Start, "tlpButton_Start");
            this.tlpButton_Start.Controls.Add(this.bStop, 1, 2);
            this.tlpButton_Start.Controls.Add(this.bStart, 1, 0);
            this.tlpButton_Start.Name = "tlpButton_Start";
            // 
            // bStop
            // 
            resources.ApplyResources(this.bStop, "bStop");
            this.bStop.Image = global::WinsockPacketEditor.Properties.Resources.Stop16;
            this.bStop.Name = "bStop";
            this.bStop.UseVisualStyleBackColor = true;
            this.bStop.Click += new System.EventHandler(this.bStop_Click);
            // 
            // bStart
            // 
            resources.ApplyResources(this.bStart, "bStart");
            this.bStart.Image = global::WinsockPacketEditor.Properties.Resources.Play16;
            this.bStart.Name = "bStart";
            this.bStart.UseVisualStyleBackColor = true;
            this.bStart.Click += new System.EventHandler(this.bStart_Click);
            // 
            // tvProxyData
            // 
            resources.ApplyResources(this.tvProxyData, "tvProxyData");
            this.tvProxyData.ImageList = this.ilSocketProxy;
            this.tvProxyData.Name = "tvProxyData";
            this.tvProxyData.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvSocketProxy_AfterSelect);
            // 
            // ilSocketProxy
            // 
            this.ilSocketProxy.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilSocketProxy.ImageStream")));
            this.ilSocketProxy.TransparentColor = System.Drawing.Color.Transparent;
            this.ilSocketProxy.Images.SetKeyName(0, "Socket.png");
            this.ilSocketProxy.Images.SetKeyName(1, "unknown.png");
            this.ilSocketProxy.Images.SetKeyName(2, "Request.png");
            this.ilSocketProxy.Images.SetKeyName(3, "Response.png");
            this.ilSocketProxy.Images.SetKeyName(4, "computer.png");
            this.ilSocketProxy.Images.SetKeyName(5, "pass.png");
            this.ilSocketProxy.Images.SetKeyName(6, "fail.png");
            this.ilSocketProxy.Images.SetKeyName(7, "http.png");
            this.ilSocketProxy.Images.SetKeyName(8, "js.png");
            // 
            // tpProxyList
            // 
            this.tpProxyList.Controls.Add(this.tvProxyData);
            resources.ApplyResources(this.tpProxyList, "tpProxyList");
            this.tpProxyList.Name = "tpProxyList";
            this.tpProxyList.UseVisualStyleBackColor = true;
            // 
            // tcProxyInfo
            // 
            this.tcProxyInfo.Controls.Add(this.tpProxyList);
            resources.ApplyResources(this.tcProxyInfo, "tcProxyInfo");
            this.tcProxyInfo.Name = "tcProxyInfo";
            this.tcProxyInfo.SelectedIndex = 0;
            // 
            // ssSocketProxy
            // 
            resources.ApplyResources(this.ssSocketProxy, "ssSocketProxy");
            this.ssSocketProxy.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlProxyTotal,
            this.tlProxyTotal_CNT,
            this.tlSplit1,
            this.tlProxyTCP,
            this.tlProxyTCP_CNT,
            this.toolStripStatusLabel3,
            this.tlProxyUDP,
            this.tlProxyUDP_CNT,
            this.toolStripStatusLabel6,
            this.tlProxyCache,
            this.tlProxyCache_CNT,
            this.toolStripStatusLabel4,
            this.tlProxyLinks,
            this.tlProxyLinks_CNT,
            this.toolStripStatusLabel5,
            this.tsslTotalBytes});
            this.ssSocketProxy.Name = "ssSocketProxy";
            // 
            // tlProxyTotal_CNT
            // 
            resources.ApplyResources(this.tlProxyTotal_CNT, "tlProxyTotal_CNT");
            this.tlProxyTotal_CNT.Name = "tlProxyTotal_CNT";
            // 
            // tlSplit1
            // 
            this.tlSplit1.ForeColor = System.Drawing.Color.DarkGray;
            this.tlSplit1.Name = "tlSplit1";
            resources.ApplyResources(this.tlSplit1, "tlSplit1");
            // 
            // tlProxyTCP
            // 
            this.tlProxyTCP.Name = "tlProxyTCP";
            resources.ApplyResources(this.tlProxyTCP, "tlProxyTCP");
            // 
            // tlProxyTCP_CNT
            // 
            resources.ApplyResources(this.tlProxyTCP_CNT, "tlProxyTCP_CNT");
            this.tlProxyTCP_CNT.Name = "tlProxyTCP_CNT";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            resources.ApplyResources(this.toolStripStatusLabel3, "toolStripStatusLabel3");
            // 
            // tlProxyUDP
            // 
            this.tlProxyUDP.Name = "tlProxyUDP";
            resources.ApplyResources(this.tlProxyUDP, "tlProxyUDP");
            // 
            // tlProxyUDP_CNT
            // 
            resources.ApplyResources(this.tlProxyUDP_CNT, "tlProxyUDP_CNT");
            this.tlProxyUDP_CNT.Name = "tlProxyUDP_CNT";
            // 
            // toolStripStatusLabel6
            // 
            this.toolStripStatusLabel6.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel6.Name = "toolStripStatusLabel6";
            resources.ApplyResources(this.toolStripStatusLabel6, "toolStripStatusLabel6");
            // 
            // tlProxyCache
            // 
            this.tlProxyCache.Name = "tlProxyCache";
            resources.ApplyResources(this.tlProxyCache, "tlProxyCache");
            // 
            // tlProxyCache_CNT
            // 
            resources.ApplyResources(this.tlProxyCache_CNT, "tlProxyCache_CNT");
            this.tlProxyCache_CNT.Name = "tlProxyCache_CNT";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            resources.ApplyResources(this.toolStripStatusLabel4, "toolStripStatusLabel4");
            // 
            // tlProxyLinks
            // 
            this.tlProxyLinks.Name = "tlProxyLinks";
            resources.ApplyResources(this.tlProxyLinks, "tlProxyLinks");
            // 
            // tlProxyLinks_CNT
            // 
            resources.ApplyResources(this.tlProxyLinks_CNT, "tlProxyLinks_CNT");
            this.tlProxyLinks_CNT.Name = "tlProxyLinks_CNT";
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.ForeColor = System.Drawing.Color.DarkGray;
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            resources.ApplyResources(this.toolStripStatusLabel5, "toolStripStatusLabel5");
            // 
            // tsslTotalBytes
            // 
            this.tsslTotalBytes.Name = "tsslTotalBytes";
            resources.ApplyResources(this.tsslTotalBytes, "tsslTotalBytes");
            // 
            // tcProxyData
            // 
            this.tcProxyData.Controls.Add(this.tpData);
            resources.ApplyResources(this.tcProxyData, "tcProxyData");
            this.tcProxyData.Name = "tcProxyData";
            this.tcProxyData.SelectedIndex = 0;
            // 
            // tpData
            // 
            this.tpData.Controls.Add(this.hbData);
            resources.ApplyResources(this.tpData, "tpData");
            this.tpData.Name = "tpData";
            this.tpData.UseVisualStyleBackColor = true;
            // 
            // hbData
            // 
            this.hbData.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            // 
            // 
            // 
            this.hbData.BuiltInContextMenu.CopyMenuItemImage = global::WinsockPacketEditor.Properties.Resources.copy;
            this.hbData.BuiltInContextMenu.CopyMenuItemText = resources.GetString("hbData.BuiltInContextMenu.CopyMenuItemText");
            this.hbData.BuiltInContextMenu.CutMenuItemImage = global::WinsockPacketEditor.Properties.Resources.cut;
            this.hbData.BuiltInContextMenu.CutMenuItemText = resources.GetString("hbData.BuiltInContextMenu.CutMenuItemText");
            this.hbData.BuiltInContextMenu.PasteMenuItemImage = global::WinsockPacketEditor.Properties.Resources.paste;
            this.hbData.BuiltInContextMenu.PasteMenuItemText = resources.GetString("hbData.BuiltInContextMenu.PasteMenuItemText");
            this.hbData.BuiltInContextMenu.SelectAllMenuItemImage = global::WinsockPacketEditor.Properties.Resources.SelectAll;
            this.hbData.BuiltInContextMenu.SelectAllMenuItemText = resources.GetString("hbData.BuiltInContextMenu.SelectAllMenuItemText");
            this.hbData.ColumnInfoVisible = true;
            resources.ApplyResources(this.hbData, "hbData");
            this.hbData.LineInfoVisible = true;
            this.hbData.Name = "hbData";
            this.hbData.ReadOnly = true;
            this.hbData.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hbData.StringViewVisible = true;
            this.hbData.VScrollBarVisible = true;
            // 
            // tlpSocketProxy_Data
            // 
            resources.ApplyResources(this.tlpSocketProxy_Data, "tlpSocketProxy_Data");
            this.tlpSocketProxy_Data.Controls.Add(this.tcSocketProxy_Log, 1, 1);
            this.tlpSocketProxy_Data.Controls.Add(this.tcProxyData, 1, 0);
            this.tlpSocketProxy_Data.Controls.Add(this.tcProxyInfo, 0, 0);
            this.tlpSocketProxy_Data.Controls.Add(this.tcClientInfo, 0, 1);
            this.tlpSocketProxy_Data.Name = "tlpSocketProxy_Data";
            // 
            // tcSocketProxy_Log
            // 
            this.tcSocketProxy_Log.Controls.Add(this.tpLog);
            resources.ApplyResources(this.tcSocketProxy_Log, "tcSocketProxy_Log");
            this.tcSocketProxy_Log.Name = "tcSocketProxy_Log";
            this.tcSocketProxy_Log.SelectedIndex = 0;
            // 
            // tpLog
            // 
            this.tpLog.Controls.Add(this.dgvLogList);
            resources.ApplyResources(this.tpLog, "tpLog");
            this.tpLog.Name = "tpLog";
            this.tpLog.UseVisualStyleBackColor = true;
            // 
            // dgvLogList
            // 
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
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9F);
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
            resources.ApplyResources(this.dgvLogList, "dgvLogList");
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
            this.cLogID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cLogID.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.cLogID, "cLogID");
            this.cLogID.Name = "cLogID";
            this.cLogID.ReadOnly = true;
            this.cLogID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cLogID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cLogTime
            // 
            this.cLogTime.DataPropertyName = "LogTime";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cLogTime.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.cLogTime, "cLogTime");
            this.cLogTime.Name = "cLogTime";
            this.cLogTime.ReadOnly = true;
            this.cLogTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cLogTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cFuncName
            // 
            this.cFuncName.DataPropertyName = "FuncName";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.cFuncName.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.cFuncName, "cFuncName");
            this.cFuncName.Name = "cFuncName";
            this.cFuncName.ReadOnly = true;
            this.cFuncName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cFuncName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cLogContent
            // 
            this.cLogContent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cLogContent.DataPropertyName = "LogContent";
            resources.ApplyResources(this.cLogContent, "cLogContent");
            this.cLogContent.Name = "cLogContent";
            this.cLogContent.ReadOnly = true;
            this.cLogContent.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cLogContent.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // tcClientInfo
            // 
            this.tcClientInfo.Controls.Add(this.tpClientList);
            resources.ApplyResources(this.tcClientInfo, "tcClientInfo");
            this.tcClientInfo.Name = "tcClientInfo";
            this.tcClientInfo.SelectedIndex = 0;
            // 
            // tpClientList
            // 
            this.tpClientList.BackColor = System.Drawing.SystemColors.Control;
            this.tpClientList.Controls.Add(this.tvProxyInfo);
            resources.ApplyResources(this.tpClientList, "tpClientList");
            this.tpClientList.Name = "tpClientList";
            // 
            // tvProxyInfo
            // 
            resources.ApplyResources(this.tvProxyInfo, "tvProxyInfo");
            this.tvProxyInfo.ImageList = this.ilSocketProxy;
            this.tvProxyInfo.Name = "tvProxyInfo";
            // 
            // tlpSocketProxy
            // 
            resources.ApplyResources(this.tlpSocketProxy, "tlpSocketProxy");
            this.tlpSocketProxy.Controls.Add(this.tlpSocketProxy_Data, 0, 1);
            this.tlpSocketProxy.Controls.Add(this.tlpSocketProxy_Parameter, 0, 0);
            this.tlpSocketProxy.Controls.Add(this.ssSocketProxy, 0, 2);
            this.tlpSocketProxy.Name = "tlpSocketProxy";
            // 
            // tSocketProxy
            // 
            this.tSocketProxy.Interval = 50;
            this.tSocketProxy.Tick += new System.EventHandler(this.tSocketProxy_Tick);
            // 
            // tCheckProxyState
            // 
            this.tCheckProxyState.Interval = 1000;
            this.tCheckProxyState.Tick += new System.EventHandler(this.tCheckProxyState_Tick);
            // 
            // SocketProxy_Form
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpSocketProxy);
            this.DoubleBuffered = true;
            this.Name = "SocketProxy_Form";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SocketProxy_Form_FormClosing);
            this.Load += new System.EventHandler(this.SocketProxy_Form_Load);
            this.tlpSocketProxy_Parameter.ResumeLayout(false);
            this.gbSystemProxy.ResumeLayout(false);
            this.tlpSystemProxy.ResumeLayout(false);
            this.tlpSystemProxy.PerformLayout();
            this.tcSocketProxySet.ResumeLayout(false);
            this.tpProxySet.ResumeLayout(false);
            this.tlpProxySet.ResumeLayout(false);
            this.gbProxyIP.ResumeLayout(false);
            this.tlpProxyIP.ResumeLayout(false);
            this.tlpProxyIP.PerformLayout();
            this.gbProxyType.ResumeLayout(false);
            this.tlpProxyType.ResumeLayout(false);
            this.tlpProxyType.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudProxyPort)).EndInit();
            this.tpAuthSet.ResumeLayout(false);
            this.tlpAuthSet.ResumeLayout(false);
            this.gbAuthSet.ResumeLayout(false);
            this.tlpAuthSet_Auth.ResumeLayout(false);
            this.tlpAuthSet_Auth.PerformLayout();
            this.tpListSet.ResumeLayout(false);
            this.tlpListSet.ResumeLayout(false);
            this.gbClientList.ResumeLayout(false);
            this.tlpClientList.ResumeLayout(false);
            this.tlpClientList.PerformLayout();
            this.gbProxyList.ResumeLayout(false);
            this.tlpProxyList.ResumeLayout(false);
            this.tlpProxyList.PerformLayout();
            this.gbListSet_LogList.ResumeLayout(false);
            this.tlpListSet_LogList.ResumeLayout(false);
            this.tlpListSet_LogList.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLogList_AutoClearValue)).EndInit();
            this.tpExternalProxy.ResumeLayout(false);
            this.tlpExternalProxy.ResumeLayout(false);
            this.tlpExternalProxy.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.tlpButton_Start.ResumeLayout(false);
            this.tpProxyList.ResumeLayout(false);
            this.tcProxyInfo.ResumeLayout(false);
            this.ssSocketProxy.ResumeLayout(false);
            this.ssSocketProxy.PerformLayout();
            this.tcProxyData.ResumeLayout(false);
            this.tpData.ResumeLayout(false);
            this.tlpSocketProxy_Data.ResumeLayout(false);
            this.tcSocketProxy_Log.ResumeLayout(false);
            this.tpLog.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLogList)).EndInit();
            this.tcClientInfo.ResumeLayout(false);
            this.tpClientList.ResumeLayout(false);
            this.tlpSocketProxy.ResumeLayout(false);
            this.tlpSocketProxy.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripStatusLabel tlProxyTotal;
        private System.Windows.Forms.TableLayoutPanel tlpSocketProxy_Parameter;
        private System.Windows.Forms.TreeView tvProxyData;
        private System.Windows.Forms.ImageList ilSocketProxy;
        private System.Windows.Forms.TabPage tpProxyList;
        private System.Windows.Forms.TabControl tcProxyInfo;
        private System.Windows.Forms.StatusStrip ssSocketProxy;
        private System.Windows.Forms.TabControl tcProxyData;
        private System.Windows.Forms.TabPage tpData;
        private System.Windows.Forms.TableLayoutPanel tlpSocketProxy_Data;
        private System.Windows.Forms.TableLayoutPanel tlpSocketProxy;
        private System.Windows.Forms.Timer tSocketProxy;
        private Be.Windows.Forms.HexBox hbData;
        private System.Windows.Forms.TabControl tcSocketProxySet;
        private System.Windows.Forms.TabPage tpProxySet;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private System.Windows.Forms.Button bCleanUp;
        private System.Windows.Forms.TableLayoutPanel tlpButton_Start;
        private System.Windows.Forms.Button bStop;
        private System.Windows.Forms.Button bStart;
        private System.Windows.Forms.TabPage tpListSet;
        private System.Windows.Forms.TableLayoutPanel tlpListSet;
        private System.Windows.Forms.GroupBox gbListSet_LogList;
        private System.Windows.Forms.TableLayoutPanel tlpListSet_LogList;
        private System.Windows.Forms.NumericUpDown nudLogList_AutoClearValue;
        private System.Windows.Forms.CheckBox cbLogList_AutoClear;
        private System.Windows.Forms.CheckBox cbLogList_AutoRoll;
        private System.Windows.Forms.TableLayoutPanel tlpProxySet;
        private System.Windows.Forms.TabControl tcSocketProxy_Log;
        private System.Windows.Forms.TabPage tpLog;
        private System.Windows.Forms.DataGridView dgvLogList;
        private System.Windows.Forms.TabControl tcClientInfo;
        private System.Windows.Forms.TabPage tpClientList;
        private System.Windows.Forms.TreeView tvProxyInfo;
        private System.Windows.Forms.GroupBox gbProxyType;
        private System.Windows.Forms.TableLayoutPanel tlpProxyType;
        private System.Windows.Forms.CheckBox cbEnable_SOCKS5;
        private System.Windows.Forms.GroupBox gbSystemProxy;
        private System.Windows.Forms.TableLayoutPanel tlpSystemProxy;
        private System.Windows.Forms.CheckBox cbEnable_SystemProxy;
        private System.Windows.Forms.Timer tCheckProxyState;
        private System.Windows.Forms.ToolStripStatusLabel tsslTotalBytes;
        private System.Windows.Forms.GroupBox gbProxyList;
        private System.Windows.Forms.TableLayoutPanel tlpProxyList;
        private System.Windows.Forms.GroupBox gbClientList;
        private System.Windows.Forms.TableLayoutPanel tlpClientList;
        private System.Windows.Forms.CheckBox cbDeleteClosed;
        private System.Windows.Forms.CheckBox cbNoRecordData;
        private System.Windows.Forms.NumericUpDown nudProxyPort;
        private System.Windows.Forms.Label lSOCKS_Port;
        private System.Windows.Forms.TabPage tpAuthSet;
        private System.Windows.Forms.TableLayoutPanel tlpAuthSet;
        private System.Windows.Forms.GroupBox gbProxyIP;
        private System.Windows.Forms.TableLayoutPanel tlpProxyIP;
        private System.Windows.Forms.GroupBox gbAuthSet;
        private System.Windows.Forms.TableLayoutPanel tlpAuthSet_Auth;
        private System.Windows.Forms.TextBox txtAuth_PassWord;
        private System.Windows.Forms.Label lAuth_PassWord;
        private System.Windows.Forms.CheckBox cbEnable_Auth;
        private System.Windows.Forms.Label lAuth_UserName;
        private System.Windows.Forms.TextBox txtAuth_UserName;
        private System.Windows.Forms.CheckBox cbProxyIP_Auto;
        private System.Windows.Forms.ToolStripStatusLabel tlProxyTotal_CNT;
        private System.Windows.Forms.ToolStripStatusLabel tlSplit1;
        private System.Windows.Forms.ToolStripStatusLabel tlProxyTCP;
        private System.Windows.Forms.ToolStripStatusLabel tlProxyTCP_CNT;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel tlProxyUDP;
        private System.Windows.Forms.ToolStripStatusLabel tlProxyUDP_CNT;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel6;
        private System.Windows.Forms.ToolStripStatusLabel tlProxyCache;
        private System.Windows.Forms.ToolStripStatusLabel tlProxyCache_CNT;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel tlProxyLinks;
        private System.Windows.Forms.ToolStripStatusLabel tlProxyLinks_CNT;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ComboBox cbbProxyIP_Appoint;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLogID;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLogTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn cFuncName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLogContent;
        private System.Windows.Forms.TabPage tpExternalProxy;
        private System.Windows.Forms.TableLayoutPanel tlpExternalProxy;
        private System.Windows.Forms.CheckBox cbEnable_EXTHttps;
        private System.Windows.Forms.CheckBox cbEnable_EXTHttp;
        private System.Windows.Forms.Label lHttpsProxy;
        private System.Windows.Forms.TextBox txtEXTHttpsPort;
        private System.Windows.Forms.TextBox txtEXTHttpsIP;
        private System.Windows.Forms.TextBox txtEXTHttpPort;
        private System.Windows.Forms.TextBox txtEXTHttpIP;
        private System.Windows.Forms.Label lHttpProxy;
        private System.Windows.Forms.Label lHttpPort;
        private System.Windows.Forms.Label lHttpsPort;
        private System.Windows.Forms.TextBox txtAppointHttpPort;
        private System.Windows.Forms.TextBox txtAppointHttpsPort;
    }
}