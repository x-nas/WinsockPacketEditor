namespace WPELibrary
{
    partial class Proxy_AccountListForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Proxy_AccountListForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlpProxyAccountList = new System.Windows.Forms.TableLayoutPanel();
            this.ssProxyAccount = new System.Windows.Forms.StatusStrip();
            this.tsslAccount = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslAccount_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslSplit = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslAccountEnable = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslAccountEnable_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslSplit1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslAccountExpiry = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslAccountExpiry_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslSplit2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslAccountOnLine = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslAccountOnLine_CNT = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlpAccountList = new System.Windows.Forms.TableLayoutPanel();
            this.dgvAccountList = new System.Windows.Forms.DataGridView();
            this.cID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cAID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cUserName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLoginTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLoginIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLimitLinks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cLimitDevices = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cIPLocation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cExpiryTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmsAccountList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmsAccountList_LoginInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsAccountList_AddTime = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsAccountList_Links = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsAccountList_Devices = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsAccountList_Export = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmsAccountList_Clear = new System.Windows.Forms.ToolStripMenuItem();
            this.tlpAccountButton = new System.Windows.Forms.TableLayoutPanel();
            this.bAddTime = new System.Windows.Forms.Button();
            this.bSelectAll = new System.Windows.Forms.Button();
            this.bDelete = new System.Windows.Forms.Button();
            this.bAccount_New = new System.Windows.Forms.Button();
            this.bExport = new System.Windows.Forms.Button();
            this.bImport = new System.Windows.Forms.Button();
            this.tlpAccountListButton = new System.Windows.Forms.TableLayoutPanel();
            this.bFirst = new System.Windows.Forms.Button();
            this.bPrevious = new System.Windows.Forms.Button();
            this.bNext = new System.Windows.Forms.Button();
            this.bLast = new System.Windows.Forms.Button();
            this.lblTotalPages = new System.Windows.Forms.Label();
            this.cbbPageSize = new System.Windows.Forms.ComboBox();
            this.tlpSearchAccount = new System.Windows.Forms.TableLayoutPanel();
            this.gSearch_Expire = new System.Windows.Forms.GroupBox();
            this.tlpSearch_Expire = new System.Windows.Forms.TableLayoutPanel();
            this.bSearch_Expire = new System.Windows.Forms.Button();
            this.dtpExpireFrom = new System.Windows.Forms.DateTimePicker();
            this.dtpExpireTo = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.gbSearch_UserName = new System.Windows.Forms.GroupBox();
            this.tlpSearch_UserName = new System.Windows.Forms.TableLayoutPanel();
            this.txtSearch_UserName = new System.Windows.Forms.TextBox();
            this.bSearch_UserName = new System.Windows.Forms.Button();
            this.gbSearch_State = new System.Windows.Forms.GroupBox();
            this.tlpSearch_State = new System.Windows.Forms.TableLayoutPanel();
            this.cbbSearch_State = new System.Windows.Forms.ComboBox();
            this.bSearch_State = new System.Windows.Forms.Button();
            this.bgwAccountList = new System.ComponentModel.BackgroundWorker();
            this.tlpProxyAccountList.SuspendLayout();
            this.ssProxyAccount.SuspendLayout();
            this.tlpAccountList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccountList)).BeginInit();
            this.cmsAccountList.SuspendLayout();
            this.tlpAccountButton.SuspendLayout();
            this.tlpAccountListButton.SuspendLayout();
            this.tlpSearchAccount.SuspendLayout();
            this.gSearch_Expire.SuspendLayout();
            this.tlpSearch_Expire.SuspendLayout();
            this.gbSearch_UserName.SuspendLayout();
            this.tlpSearch_UserName.SuspendLayout();
            this.gbSearch_State.SuspendLayout();
            this.tlpSearch_State.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpProxyAccountList
            // 
            resources.ApplyResources(this.tlpProxyAccountList, "tlpProxyAccountList");
            this.tlpProxyAccountList.Controls.Add(this.ssProxyAccount, 0, 1);
            this.tlpProxyAccountList.Controls.Add(this.tlpAccountList, 0, 2);
            this.tlpProxyAccountList.Controls.Add(this.tlpSearchAccount, 0, 0);
            this.tlpProxyAccountList.Name = "tlpProxyAccountList";
            // 
            // ssProxyAccount
            // 
            resources.ApplyResources(this.ssProxyAccount, "ssProxyAccount");
            this.ssProxyAccount.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslAccount,
            this.tsslAccount_CNT,
            this.tsslSplit,
            this.tsslAccountEnable,
            this.tsslAccountEnable_CNT,
            this.tsslSplit1,
            this.tsslAccountExpiry,
            this.tsslAccountExpiry_CNT,
            this.tsslSplit2,
            this.tsslAccountOnLine,
            this.tsslAccountOnLine_CNT});
            this.ssProxyAccount.Name = "ssProxyAccount";
            this.ssProxyAccount.SizingGrip = false;
            // 
            // tsslAccount
            // 
            this.tsslAccount.Name = "tsslAccount";
            resources.ApplyResources(this.tsslAccount, "tsslAccount");
            // 
            // tsslAccount_CNT
            // 
            resources.ApplyResources(this.tsslAccount_CNT, "tsslAccount_CNT");
            this.tsslAccount_CNT.Name = "tsslAccount_CNT";
            // 
            // tsslSplit
            // 
            resources.ApplyResources(this.tsslSplit, "tsslSplit");
            this.tsslSplit.Name = "tsslSplit";
            // 
            // tsslAccountEnable
            // 
            this.tsslAccountEnable.Name = "tsslAccountEnable";
            resources.ApplyResources(this.tsslAccountEnable, "tsslAccountEnable");
            // 
            // tsslAccountEnable_CNT
            // 
            resources.ApplyResources(this.tsslAccountEnable_CNT, "tsslAccountEnable_CNT");
            this.tsslAccountEnable_CNT.Name = "tsslAccountEnable_CNT";
            // 
            // tsslSplit1
            // 
            resources.ApplyResources(this.tsslSplit1, "tsslSplit1");
            this.tsslSplit1.Name = "tsslSplit1";
            // 
            // tsslAccountExpiry
            // 
            this.tsslAccountExpiry.Name = "tsslAccountExpiry";
            resources.ApplyResources(this.tsslAccountExpiry, "tsslAccountExpiry");
            // 
            // tsslAccountExpiry_CNT
            // 
            resources.ApplyResources(this.tsslAccountExpiry_CNT, "tsslAccountExpiry_CNT");
            this.tsslAccountExpiry_CNT.Name = "tsslAccountExpiry_CNT";
            // 
            // tsslSplit2
            // 
            resources.ApplyResources(this.tsslSplit2, "tsslSplit2");
            this.tsslSplit2.Name = "tsslSplit2";
            // 
            // tsslAccountOnLine
            // 
            this.tsslAccountOnLine.Name = "tsslAccountOnLine";
            resources.ApplyResources(this.tsslAccountOnLine, "tsslAccountOnLine");
            // 
            // tsslAccountOnLine_CNT
            // 
            resources.ApplyResources(this.tsslAccountOnLine_CNT, "tsslAccountOnLine_CNT");
            this.tsslAccountOnLine_CNT.Name = "tsslAccountOnLine_CNT";
            // 
            // tlpAccountList
            // 
            resources.ApplyResources(this.tlpAccountList, "tlpAccountList");
            this.tlpAccountList.Controls.Add(this.dgvAccountList, 0, 0);
            this.tlpAccountList.Controls.Add(this.tlpAccountButton, 1, 0);
            this.tlpAccountList.Controls.Add(this.tlpAccountListButton, 0, 1);
            this.tlpAccountList.Name = "tlpAccountList";
            // 
            // dgvAccountList
            // 
            this.dgvAccountList.AllowUserToAddRows = false;
            this.dgvAccountList.AllowUserToDeleteRows = false;
            this.dgvAccountList.AllowUserToResizeColumns = false;
            this.dgvAccountList.AllowUserToResizeRows = false;
            this.dgvAccountList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvAccountList.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvAccountList.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAccountList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvAccountList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAccountList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.cID,
            this.cAID,
            this.cUserName,
            this.cLoginTime,
            this.cLoginIP,
            this.cLimitLinks,
            this.cLimitDevices,
            this.cIPLocation,
            this.cExpiryTime});
            this.dgvAccountList.ContextMenuStrip = this.cmsAccountList;
            resources.ApplyResources(this.dgvAccountList, "dgvAccountList");
            this.dgvAccountList.Name = "dgvAccountList";
            this.dgvAccountList.ReadOnly = true;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("微软雅黑", 9F);
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAccountList.RowHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvAccountList.RowHeadersVisible = false;
            dataGridViewCellStyle11.Padding = new System.Windows.Forms.Padding(3);
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAccountList.RowsDefaultCellStyle = dataGridViewCellStyle11;
            this.dgvAccountList.RowTemplate.Height = 23;
            this.dgvAccountList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAccountList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAccountList_CellDoubleClick);
            // 
            // cID
            // 
            this.cID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cID.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.cID, "cID");
            this.cID.Name = "cID";
            this.cID.ReadOnly = true;
            this.cID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cAID
            // 
            this.cAID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cAID.DataPropertyName = "AID";
            resources.ApplyResources(this.cAID, "cAID");
            this.cAID.Name = "cAID";
            this.cAID.ReadOnly = true;
            this.cAID.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cAID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cUserName
            // 
            this.cUserName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cUserName.DataPropertyName = "UserName";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cUserName.DefaultCellStyle = dataGridViewCellStyle3;
            resources.ApplyResources(this.cUserName, "cUserName");
            this.cUserName.Name = "cUserName";
            this.cUserName.ReadOnly = true;
            this.cUserName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cUserName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cLoginTime
            // 
            this.cLoginTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cLoginTime.DataPropertyName = "LoginTime";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.Format = "g";
            dataGridViewCellStyle4.NullValue = null;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cLoginTime.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.cLoginTime, "cLoginTime");
            this.cLoginTime.Name = "cLoginTime";
            this.cLoginTime.ReadOnly = true;
            this.cLoginTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cLoginTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cLoginIP
            // 
            this.cLoginIP.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cLoginIP.DataPropertyName = "LoginIP";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cLoginIP.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.cLoginIP, "cLoginIP");
            this.cLoginIP.Name = "cLoginIP";
            this.cLoginIP.ReadOnly = true;
            this.cLoginIP.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cLoginIP.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cLimitLinks
            // 
            this.cLimitLinks.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cLimitLinks.DefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.cLimitLinks, "cLimitLinks");
            this.cLimitLinks.Name = "cLimitLinks";
            this.cLimitLinks.ReadOnly = true;
            this.cLimitLinks.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cLimitLinks.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cLimitDevices
            // 
            this.cLimitDevices.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cLimitDevices.DefaultCellStyle = dataGridViewCellStyle7;
            resources.ApplyResources(this.cLimitDevices, "cLimitDevices");
            this.cLimitDevices.Name = "cLimitDevices";
            this.cLimitDevices.ReadOnly = true;
            this.cLimitDevices.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cLimitDevices.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cIPLocation
            // 
            this.cIPLocation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.cIPLocation.DataPropertyName = "IPLocation";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cIPLocation.DefaultCellStyle = dataGridViewCellStyle8;
            resources.ApplyResources(this.cIPLocation, "cIPLocation");
            this.cIPLocation.Name = "cIPLocation";
            this.cIPLocation.ReadOnly = true;
            this.cIPLocation.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cIPLocation.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cExpiryTime
            // 
            this.cExpiryTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.cExpiryTime.DataPropertyName = "ExpiryTime";
            dataGridViewCellStyle9.Format = "g";
            dataGridViewCellStyle9.NullValue = null;
            this.cExpiryTime.DefaultCellStyle = dataGridViewCellStyle9;
            resources.ApplyResources(this.cExpiryTime, "cExpiryTime");
            this.cExpiryTime.Name = "cExpiryTime";
            this.cExpiryTime.ReadOnly = true;
            this.cExpiryTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.cExpiryTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // cmsAccountList
            // 
            this.cmsAccountList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmsAccountList_LoginInfo,
            this.toolStripSeparator1,
            this.cmsAccountList_AddTime,
            this.cmsAccountList_Links,
            this.cmsAccountList_Devices,
            this.toolStripSeparator3,
            this.cmsAccountList_Export,
            this.toolStripSeparator2,
            this.cmsAccountList_Clear});
            this.cmsAccountList.Name = "cmsAccountList";
            resources.ApplyResources(this.cmsAccountList, "cmsAccountList");
            this.cmsAccountList.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.cmsAccountList_ItemClicked);
            // 
            // cmsAccountList_LoginInfo
            // 
            this.cmsAccountList_LoginInfo.Image = global::WPELibrary.Properties.Resources.computer;
            resources.ApplyResources(this.cmsAccountList_LoginInfo, "cmsAccountList_LoginInfo");
            this.cmsAccountList_LoginInfo.Name = "cmsAccountList_LoginInfo";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // cmsAccountList_AddTime
            // 
            this.cmsAccountList_AddTime.Image = global::WPELibrary.Properties.Resources.SelectAll;
            resources.ApplyResources(this.cmsAccountList_AddTime, "cmsAccountList_AddTime");
            this.cmsAccountList_AddTime.Name = "cmsAccountList_AddTime";
            // 
            // cmsAccountList_Links
            // 
            this.cmsAccountList_Links.Image = global::WPELibrary.Properties.Resources.SelectAll;
            resources.ApplyResources(this.cmsAccountList_Links, "cmsAccountList_Links");
            this.cmsAccountList_Links.Name = "cmsAccountList_Links";
            // 
            // cmsAccountList_Devices
            // 
            this.cmsAccountList_Devices.Image = global::WPELibrary.Properties.Resources.SelectAll;
            resources.ApplyResources(this.cmsAccountList_Devices, "cmsAccountList_Devices");
            this.cmsAccountList_Devices.Name = "cmsAccountList_Devices";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // cmsAccountList_Export
            // 
            this.cmsAccountList_Export.Image = global::WPELibrary.Properties.Resources.save;
            resources.ApplyResources(this.cmsAccountList_Export, "cmsAccountList_Export");
            this.cmsAccountList_Export.Name = "cmsAccountList_Export";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // cmsAccountList_Clear
            // 
            this.cmsAccountList_Clear.Image = global::WPELibrary.Properties.Resources.Trash_can16;
            resources.ApplyResources(this.cmsAccountList_Clear, "cmsAccountList_Clear");
            this.cmsAccountList_Clear.Name = "cmsAccountList_Clear";
            // 
            // tlpAccountButton
            // 
            resources.ApplyResources(this.tlpAccountButton, "tlpAccountButton");
            this.tlpAccountButton.Controls.Add(this.bAddTime, 1, 7);
            this.tlpAccountButton.Controls.Add(this.bSelectAll, 1, 1);
            this.tlpAccountButton.Controls.Add(this.bDelete, 1, 5);
            this.tlpAccountButton.Controls.Add(this.bAccount_New, 1, 3);
            this.tlpAccountButton.Controls.Add(this.bExport, 1, 9);
            this.tlpAccountButton.Controls.Add(this.bImport, 1, 11);
            this.tlpAccountButton.Name = "tlpAccountButton";
            // 
            // bAddTime
            // 
            resources.ApplyResources(this.bAddTime, "bAddTime");
            this.bAddTime.Name = "bAddTime";
            this.bAddTime.UseVisualStyleBackColor = true;
            this.bAddTime.Click += new System.EventHandler(this.bAddTime_Click);
            // 
            // bSelectAll
            // 
            resources.ApplyResources(this.bSelectAll, "bSelectAll");
            this.bSelectAll.Name = "bSelectAll";
            this.bSelectAll.UseVisualStyleBackColor = true;
            this.bSelectAll.Click += new System.EventHandler(this.bSelectAll_Click);
            // 
            // bDelete
            // 
            resources.ApplyResources(this.bDelete, "bDelete");
            this.bDelete.Name = "bDelete";
            this.bDelete.UseVisualStyleBackColor = true;
            this.bDelete.Click += new System.EventHandler(this.bDelete_Click);
            // 
            // bAccount_New
            // 
            resources.ApplyResources(this.bAccount_New, "bAccount_New");
            this.bAccount_New.Name = "bAccount_New";
            this.bAccount_New.UseVisualStyleBackColor = true;
            this.bAccount_New.Click += new System.EventHandler(this.bAccount_New_Click);
            // 
            // bExport
            // 
            resources.ApplyResources(this.bExport, "bExport");
            this.bExport.Name = "bExport";
            this.bExport.UseVisualStyleBackColor = true;
            this.bExport.Click += new System.EventHandler(this.bExport_Click);
            // 
            // bImport
            // 
            resources.ApplyResources(this.bImport, "bImport");
            this.bImport.Name = "bImport";
            this.bImport.UseVisualStyleBackColor = true;
            this.bImport.Click += new System.EventHandler(this.bImport_Click);
            // 
            // tlpAccountListButton
            // 
            resources.ApplyResources(this.tlpAccountListButton, "tlpAccountListButton");
            this.tlpAccountListButton.Controls.Add(this.bFirst, 3, 0);
            this.tlpAccountListButton.Controls.Add(this.bPrevious, 5, 0);
            this.tlpAccountListButton.Controls.Add(this.bNext, 7, 0);
            this.tlpAccountListButton.Controls.Add(this.bLast, 9, 0);
            this.tlpAccountListButton.Controls.Add(this.lblTotalPages, 11, 0);
            this.tlpAccountListButton.Controls.Add(this.cbbPageSize, 1, 0);
            this.tlpAccountListButton.Name = "tlpAccountListButton";
            // 
            // bFirst
            // 
            resources.ApplyResources(this.bFirst, "bFirst");
            this.bFirst.Name = "bFirst";
            this.bFirst.UseVisualStyleBackColor = true;
            this.bFirst.Click += new System.EventHandler(this.bFirst_Click);
            // 
            // bPrevious
            // 
            resources.ApplyResources(this.bPrevious, "bPrevious");
            this.bPrevious.Name = "bPrevious";
            this.bPrevious.UseVisualStyleBackColor = true;
            this.bPrevious.Click += new System.EventHandler(this.bPrevious_Click);
            // 
            // bNext
            // 
            resources.ApplyResources(this.bNext, "bNext");
            this.bNext.Name = "bNext";
            this.bNext.UseVisualStyleBackColor = true;
            this.bNext.Click += new System.EventHandler(this.bNext_Click);
            // 
            // bLast
            // 
            resources.ApplyResources(this.bLast, "bLast");
            this.bLast.Name = "bLast";
            this.bLast.UseVisualStyleBackColor = true;
            this.bLast.Click += new System.EventHandler(this.bLast_Click);
            // 
            // lblTotalPages
            // 
            resources.ApplyResources(this.lblTotalPages, "lblTotalPages");
            this.lblTotalPages.Name = "lblTotalPages";
            // 
            // cbbPageSize
            // 
            resources.ApplyResources(this.cbbPageSize, "cbbPageSize");
            this.cbbPageSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbPageSize.FormattingEnabled = true;
            this.cbbPageSize.Items.AddRange(new object[] {
            resources.GetString("cbbPageSize.Items"),
            resources.GetString("cbbPageSize.Items1"),
            resources.GetString("cbbPageSize.Items2"),
            resources.GetString("cbbPageSize.Items3")});
            this.cbbPageSize.Name = "cbbPageSize";
            this.cbbPageSize.SelectedIndexChanged += new System.EventHandler(this.cbbPageSize_SelectedIndexChanged);
            // 
            // tlpSearchAccount
            // 
            resources.ApplyResources(this.tlpSearchAccount, "tlpSearchAccount");
            this.tlpSearchAccount.Controls.Add(this.gSearch_Expire, 2, 0);
            this.tlpSearchAccount.Controls.Add(this.gbSearch_UserName, 0, 0);
            this.tlpSearchAccount.Controls.Add(this.gbSearch_State, 1, 0);
            this.tlpSearchAccount.Name = "tlpSearchAccount";
            // 
            // gSearch_Expire
            // 
            this.gSearch_Expire.Controls.Add(this.tlpSearch_Expire);
            resources.ApplyResources(this.gSearch_Expire, "gSearch_Expire");
            this.gSearch_Expire.Name = "gSearch_Expire";
            this.gSearch_Expire.TabStop = false;
            // 
            // tlpSearch_Expire
            // 
            resources.ApplyResources(this.tlpSearch_Expire, "tlpSearch_Expire");
            this.tlpSearch_Expire.Controls.Add(this.bSearch_Expire, 4, 1);
            this.tlpSearch_Expire.Controls.Add(this.dtpExpireFrom, 0, 1);
            this.tlpSearch_Expire.Controls.Add(this.dtpExpireTo, 2, 1);
            this.tlpSearch_Expire.Controls.Add(this.label1, 1, 1);
            this.tlpSearch_Expire.Name = "tlpSearch_Expire";
            // 
            // bSearch_Expire
            // 
            resources.ApplyResources(this.bSearch_Expire, "bSearch_Expire");
            this.bSearch_Expire.Name = "bSearch_Expire";
            this.bSearch_Expire.UseVisualStyleBackColor = true;
            this.bSearch_Expire.Click += new System.EventHandler(this.bSearch_Expire_Click);
            // 
            // dtpExpireFrom
            // 
            resources.ApplyResources(this.dtpExpireFrom, "dtpExpireFrom");
            this.dtpExpireFrom.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpExpireFrom.Name = "dtpExpireFrom";
            this.dtpExpireFrom.ShowUpDown = true;
            // 
            // dtpExpireTo
            // 
            resources.ApplyResources(this.dtpExpireTo, "dtpExpireTo");
            this.dtpExpireTo.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpExpireTo.Name = "dtpExpireTo";
            this.dtpExpireTo.ShowUpDown = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // gbSearch_UserName
            // 
            this.gbSearch_UserName.Controls.Add(this.tlpSearch_UserName);
            resources.ApplyResources(this.gbSearch_UserName, "gbSearch_UserName");
            this.gbSearch_UserName.Name = "gbSearch_UserName";
            this.gbSearch_UserName.TabStop = false;
            // 
            // tlpSearch_UserName
            // 
            resources.ApplyResources(this.tlpSearch_UserName, "tlpSearch_UserName");
            this.tlpSearch_UserName.Controls.Add(this.txtSearch_UserName, 0, 1);
            this.tlpSearch_UserName.Controls.Add(this.bSearch_UserName, 2, 1);
            this.tlpSearch_UserName.Name = "tlpSearch_UserName";
            // 
            // txtSearch_UserName
            // 
            resources.ApplyResources(this.txtSearch_UserName, "txtSearch_UserName");
            this.txtSearch_UserName.Name = "txtSearch_UserName";
            // 
            // bSearch_UserName
            // 
            resources.ApplyResources(this.bSearch_UserName, "bSearch_UserName");
            this.bSearch_UserName.Name = "bSearch_UserName";
            this.bSearch_UserName.UseVisualStyleBackColor = true;
            this.bSearch_UserName.Click += new System.EventHandler(this.bSearch_UserName_Click);
            // 
            // gbSearch_State
            // 
            this.gbSearch_State.Controls.Add(this.tlpSearch_State);
            resources.ApplyResources(this.gbSearch_State, "gbSearch_State");
            this.gbSearch_State.Name = "gbSearch_State";
            this.gbSearch_State.TabStop = false;
            // 
            // tlpSearch_State
            // 
            resources.ApplyResources(this.tlpSearch_State, "tlpSearch_State");
            this.tlpSearch_State.Controls.Add(this.cbbSearch_State, 0, 1);
            this.tlpSearch_State.Controls.Add(this.bSearch_State, 2, 1);
            this.tlpSearch_State.Name = "tlpSearch_State";
            // 
            // cbbSearch_State
            // 
            resources.ApplyResources(this.cbbSearch_State, "cbbSearch_State");
            this.cbbSearch_State.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbSearch_State.FormattingEnabled = true;
            this.cbbSearch_State.Items.AddRange(new object[] {
            resources.GetString("cbbSearch_State.Items"),
            resources.GetString("cbbSearch_State.Items1"),
            resources.GetString("cbbSearch_State.Items2"),
            resources.GetString("cbbSearch_State.Items3"),
            resources.GetString("cbbSearch_State.Items4"),
            resources.GetString("cbbSearch_State.Items5"),
            resources.GetString("cbbSearch_State.Items6"),
            resources.GetString("cbbSearch_State.Items7"),
            resources.GetString("cbbSearch_State.Items8"),
            resources.GetString("cbbSearch_State.Items9")});
            this.cbbSearch_State.Name = "cbbSearch_State";
            // 
            // bSearch_State
            // 
            resources.ApplyResources(this.bSearch_State, "bSearch_State");
            this.bSearch_State.Name = "bSearch_State";
            this.bSearch_State.UseVisualStyleBackColor = true;
            this.bSearch_State.Click += new System.EventHandler(this.bSearch_State_Click);
            // 
            // bgwAccountList
            // 
            this.bgwAccountList.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwAccountList_DoWork);
            this.bgwAccountList.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwAccountList_RunWorkerCompleted);
            // 
            // Proxy_AccountListForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.Controls.Add(this.tlpProxyAccountList);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Proxy_AccountListForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Proxy_AccountListForm_FormClosed);
            this.Load += new System.EventHandler(this.Proxy_AccountListForm_Load);
            this.tlpProxyAccountList.ResumeLayout(false);
            this.tlpProxyAccountList.PerformLayout();
            this.ssProxyAccount.ResumeLayout(false);
            this.ssProxyAccount.PerformLayout();
            this.tlpAccountList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccountList)).EndInit();
            this.cmsAccountList.ResumeLayout(false);
            this.tlpAccountButton.ResumeLayout(false);
            this.tlpAccountListButton.ResumeLayout(false);
            this.tlpAccountListButton.PerformLayout();
            this.tlpSearchAccount.ResumeLayout(false);
            this.gSearch_Expire.ResumeLayout(false);
            this.tlpSearch_Expire.ResumeLayout(false);
            this.tlpSearch_Expire.PerformLayout();
            this.gbSearch_UserName.ResumeLayout(false);
            this.tlpSearch_UserName.ResumeLayout(false);
            this.tlpSearch_UserName.PerformLayout();
            this.gbSearch_State.ResumeLayout(false);
            this.tlpSearch_State.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpProxyAccountList;
        private System.Windows.Forms.StatusStrip ssProxyAccount;
        private System.Windows.Forms.TableLayoutPanel tlpAccountList;
        private System.Windows.Forms.DataGridView dgvAccountList;
        private System.Windows.Forms.TableLayoutPanel tlpAccountButton;
        private System.Windows.Forms.Button bAccount_New;
        private System.Windows.Forms.Button bDelete;
        private System.Windows.Forms.ToolStripStatusLabel tsslAccount;
        private System.Windows.Forms.ToolStripStatusLabel tsslAccount_CNT;
        private System.Windows.Forms.ToolStripStatusLabel tsslSplit;
        private System.Windows.Forms.ToolStripStatusLabel tsslAccountEnable;
        private System.Windows.Forms.ToolStripStatusLabel tsslAccountEnable_CNT;
        private System.Windows.Forms.ToolStripStatusLabel tsslSplit2;
        private System.Windows.Forms.ToolStripStatusLabel tsslAccountExpiry;
        private System.Windows.Forms.ToolStripStatusLabel tsslAccountExpiry_CNT;
        private System.Windows.Forms.ToolStripStatusLabel tsslSplit1;
        private System.Windows.Forms.ToolStripStatusLabel tsslAccountOnLine;
        private System.Windows.Forms.ToolStripStatusLabel tsslAccountOnLine_CNT;
        private System.Windows.Forms.TableLayoutPanel tlpSearchAccount;
        private System.Windows.Forms.GroupBox gbSearch_State;
        private System.Windows.Forms.GroupBox gbSearch_UserName;
        private System.Windows.Forms.TableLayoutPanel tlpSearch_UserName;
        private System.Windows.Forms.TextBox txtSearch_UserName;
        private System.Windows.Forms.Button bSearch_UserName;
        private System.Windows.Forms.TableLayoutPanel tlpSearch_State;
        private System.Windows.Forms.Button bExport;
        private System.Windows.Forms.Button bImport;
        private System.Windows.Forms.ComboBox cbbSearch_State;
        private System.Windows.Forms.ContextMenuStrip cmsAccountList;
        private System.Windows.Forms.ToolStripMenuItem cmsAccountList_LoginInfo;
        private System.ComponentModel.BackgroundWorker bgwAccountList;
        private System.Windows.Forms.TableLayoutPanel tlpAccountListButton;
        private System.Windows.Forms.Button bFirst;
        private System.Windows.Forms.Button bPrevious;
        private System.Windows.Forms.Button bNext;
        private System.Windows.Forms.Button bLast;
        private System.Windows.Forms.Label lblTotalPages;
        private System.Windows.Forms.Button bSelectAll;
        private System.Windows.Forms.ToolStripMenuItem cmsAccountList_Export;
        private System.Windows.Forms.ComboBox cbbPageSize;
        private System.Windows.Forms.Button bSearch_State;
        private System.Windows.Forms.GroupBox gSearch_Expire;
        private System.Windows.Forms.TableLayoutPanel tlpSearch_Expire;
        private System.Windows.Forms.Button bSearch_Expire;
        private System.Windows.Forms.DateTimePicker dtpExpireFrom;
        private System.Windows.Forms.DateTimePicker dtpExpireTo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem cmsAccountList_Clear;
        private System.Windows.Forms.Button bAddTime;
        private System.Windows.Forms.ToolStripMenuItem cmsAccountList_AddTime;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.DataGridViewTextBoxColumn cID;
        private System.Windows.Forms.DataGridViewTextBoxColumn cAID;
        private System.Windows.Forms.DataGridViewTextBoxColumn cUserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLoginTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLoginIP;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLimitLinks;
        private System.Windows.Forms.DataGridViewTextBoxColumn cLimitDevices;
        private System.Windows.Forms.DataGridViewTextBoxColumn cIPLocation;
        private System.Windows.Forms.DataGridViewTextBoxColumn cExpiryTime;
        private System.Windows.Forms.ToolStripMenuItem cmsAccountList_Devices;
        private System.Windows.Forms.ToolStripMenuItem cmsAccountList_Links;
    }
}