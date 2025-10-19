namespace WinsockPacketEditor
{
    partial class LeachSetting
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
            this.tlpFLeachSettings = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpContent = new WinsockPacketEditor.TableLayoutPanelEx();
            this.cbCheckType = new AntdUI.Checkbox();
            this.txtCheckData = new AntdUI.Input();
            this.cbCheckData = new AntdUI.Checkbox();
            this.txtCheckHead = new AntdUI.Input();
            this.cbCheckHead = new AntdUI.Checkbox();
            this.txtCheckPort = new AntdUI.Input();
            this.cbCheckPort = new AntdUI.Checkbox();
            this.txtCheckIP = new AntdUI.Input();
            this.cbCheckIP = new AntdUI.Checkbox();
            this.txtCheckLen = new AntdUI.Input();
            this.cbCheckLen = new AntdUI.Checkbox();
            this.cbCheckSocket = new AntdUI.Checkbox();
            this.txtCheckSocket = new AntdUI.Input();
            this.lIsShow = new AntdUI.Label();
            this.sIsShow = new AntdUI.Switch();
            this.tabPacketType = new AntdUI.Tabs();
            this.tpInject = new AntdUI.TabPage();
            this.tlpInject = new WinsockPacketEditor.TableLayoutPanelEx();
            this.cbSend = new AntdUI.Checkbox();
            this.cbWSARecvFrom = new AntdUI.Checkbox();
            this.cbRecvFrom = new AntdUI.Checkbox();
            this.cbWSARecv = new AntdUI.Checkbox();
            this.cbRecv = new AntdUI.Checkbox();
            this.cbWSASendTo = new AntdUI.Checkbox();
            this.cbSendTo = new AntdUI.Checkbox();
            this.cbWSASend = new AntdUI.Checkbox();
            this.tpProxy = new AntdUI.TabPage();
            this.tlpProxy = new WinsockPacketEditor.TableLayoutPanelEx();
            this.cbTCP_Req = new AntdUI.Checkbox();
            this.cbUDP_Resp = new AntdUI.Checkbox();
            this.cbUDP_Req = new AntdUI.Checkbox();
            this.cbTCP_Resp = new AntdUI.Checkbox();
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tlpFLeachSettings.SuspendLayout();
            this.tlpContent.SuspendLayout();
            this.tabPacketType.SuspendLayout();
            this.tpInject.SuspendLayout();
            this.tlpInject.SuspendLayout();
            this.tpProxy.SuspendLayout();
            this.tlpProxy.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpFLeachSettings
            // 
            this.tlpFLeachSettings.ColumnCount = 1;
            this.tlpFLeachSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFLeachSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlpFLeachSettings.Controls.Add(this.tlpContent, 0, 0);
            this.tlpFLeachSettings.Controls.Add(this.tlpButton, 0, 1);
            this.tlpFLeachSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFLeachSettings.Location = new System.Drawing.Point(0, 0);
            this.tlpFLeachSettings.Margin = new System.Windows.Forms.Padding(2);
            this.tlpFLeachSettings.Name = "tlpFLeachSettings";
            this.tlpFLeachSettings.RowCount = 2;
            this.tlpFLeachSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFLeachSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tlpFLeachSettings.Size = new System.Drawing.Size(350, 700);
            this.tlpFLeachSettings.TabIndex = 2;
            // 
            // tlpContent
            // 
            this.tlpContent.ColumnCount = 2;
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpContent.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpContent.Controls.Add(this.cbCheckType, 0, 8);
            this.tlpContent.Controls.Add(this.txtCheckData, 1, 7);
            this.tlpContent.Controls.Add(this.cbCheckData, 0, 7);
            this.tlpContent.Controls.Add(this.txtCheckHead, 1, 6);
            this.tlpContent.Controls.Add(this.cbCheckHead, 0, 6);
            this.tlpContent.Controls.Add(this.txtCheckPort, 1, 5);
            this.tlpContent.Controls.Add(this.cbCheckPort, 0, 5);
            this.tlpContent.Controls.Add(this.txtCheckIP, 1, 4);
            this.tlpContent.Controls.Add(this.cbCheckIP, 0, 4);
            this.tlpContent.Controls.Add(this.txtCheckLen, 1, 3);
            this.tlpContent.Controls.Add(this.cbCheckLen, 0, 3);
            this.tlpContent.Controls.Add(this.cbCheckSocket, 0, 2);
            this.tlpContent.Controls.Add(this.txtCheckSocket, 1, 2);
            this.tlpContent.Controls.Add(this.lIsShow, 0, 0);
            this.tlpContent.Controls.Add(this.sIsShow, 1, 0);
            this.tlpContent.Controls.Add(this.tabPacketType, 1, 8);
            this.tlpContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpContent.Location = new System.Drawing.Point(0, 0);
            this.tlpContent.Margin = new System.Windows.Forms.Padding(0);
            this.tlpContent.Name = "tlpContent";
            this.tlpContent.RowCount = 9;
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpContent.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpContent.Size = new System.Drawing.Size(350, 651);
            this.tlpContent.TabIndex = 1;
            // 
            // cbCheckType
            // 
            this.cbCheckType.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbCheckType.Dock = System.Windows.Forms.DockStyle.Top;
            this.cbCheckType.LocalizationText = "PacketType";
            this.cbCheckType.Location = new System.Drawing.Point(2, 287);
            this.cbCheckType.Margin = new System.Windows.Forms.Padding(2);
            this.cbCheckType.Name = "cbCheckType";
            this.cbCheckType.Size = new System.Drawing.Size(87, 32);
            this.cbCheckType.TabIndex = 15;
            this.cbCheckType.Text = "指定类别 :";
            this.cbCheckType.CheckedChanged += new AntdUI.BoolEventHandler(this.cbCheckType_CheckedChanged);
            // 
            // txtCheckData
            // 
            this.txtCheckData.AllowClear = true;
            this.txtCheckData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCheckData.LocalizationPlaceholderText = "HEXSemicolonDelimiter";
            this.txtCheckData.Location = new System.Drawing.Point(93, 247);
            this.txtCheckData.Margin = new System.Windows.Forms.Padding(2);
            this.txtCheckData.Name = "txtCheckData";
            this.txtCheckData.PlaceholderText = "十六进制带空格，支持 ; 分隔符";
            this.txtCheckData.Size = new System.Drawing.Size(255, 36);
            this.txtCheckData.TabIndex = 11;
            // 
            // cbCheckData
            // 
            this.cbCheckData.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbCheckData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCheckData.LocalizationText = "PacketContent";
            this.cbCheckData.Location = new System.Drawing.Point(2, 247);
            this.cbCheckData.Margin = new System.Windows.Forms.Padding(2);
            this.cbCheckData.Name = "cbCheckData";
            this.cbCheckData.Size = new System.Drawing.Size(87, 36);
            this.cbCheckData.TabIndex = 10;
            this.cbCheckData.Text = "指定内容 :";
            this.cbCheckData.CheckedChanged += new AntdUI.BoolEventHandler(this.cbCheckData_CheckedChanged);
            // 
            // txtCheckHead
            // 
            this.txtCheckHead.AllowClear = true;
            this.txtCheckHead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCheckHead.LocalizationPlaceholderText = "HEXSemicolonDelimiter";
            this.txtCheckHead.Location = new System.Drawing.Point(93, 207);
            this.txtCheckHead.Margin = new System.Windows.Forms.Padding(2);
            this.txtCheckHead.Name = "txtCheckHead";
            this.txtCheckHead.PlaceholderText = "十六进制带空格，支持 ; 分隔符";
            this.txtCheckHead.Size = new System.Drawing.Size(255, 36);
            this.txtCheckHead.TabIndex = 9;
            // 
            // cbCheckHead
            // 
            this.cbCheckHead.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbCheckHead.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCheckHead.LocalizationText = "PacketHead";
            this.cbCheckHead.Location = new System.Drawing.Point(2, 207);
            this.cbCheckHead.Margin = new System.Windows.Forms.Padding(2);
            this.cbCheckHead.Name = "cbCheckHead";
            this.cbCheckHead.Size = new System.Drawing.Size(87, 36);
            this.cbCheckHead.TabIndex = 8;
            this.cbCheckHead.Text = "指定包头 :";
            this.cbCheckHead.CheckedChanged += new AntdUI.BoolEventHandler(this.cbCheckHead_CheckedChanged);
            // 
            // txtCheckPort
            // 
            this.txtCheckPort.AllowClear = true;
            this.txtCheckPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCheckPort.LocalizationPlaceholderText = "SemicolonDelimiter";
            this.txtCheckPort.Location = new System.Drawing.Point(93, 167);
            this.txtCheckPort.Margin = new System.Windows.Forms.Padding(2);
            this.txtCheckPort.Name = "txtCheckPort";
            this.txtCheckPort.PlaceholderText = "支持 ; 分隔符";
            this.txtCheckPort.Size = new System.Drawing.Size(255, 36);
            this.txtCheckPort.TabIndex = 7;
            // 
            // cbCheckPort
            // 
            this.cbCheckPort.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbCheckPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCheckPort.LocalizationText = "Port";
            this.cbCheckPort.Location = new System.Drawing.Point(2, 167);
            this.cbCheckPort.Margin = new System.Windows.Forms.Padding(2);
            this.cbCheckPort.Name = "cbCheckPort";
            this.cbCheckPort.Size = new System.Drawing.Size(75, 36);
            this.cbCheckPort.TabIndex = 6;
            this.cbCheckPort.Text = "端口号 :";
            this.cbCheckPort.CheckedChanged += new AntdUI.BoolEventHandler(this.cbCheckPort_CheckedChanged);
            // 
            // txtCheckIP
            // 
            this.txtCheckIP.AllowClear = true;
            this.txtCheckIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCheckIP.LocalizationPlaceholderText = "SemicolonDelimiter";
            this.txtCheckIP.Location = new System.Drawing.Point(93, 127);
            this.txtCheckIP.Margin = new System.Windows.Forms.Padding(2);
            this.txtCheckIP.Name = "txtCheckIP";
            this.txtCheckIP.PlaceholderText = "支持 ; 分隔符";
            this.txtCheckIP.Size = new System.Drawing.Size(255, 36);
            this.txtCheckIP.TabIndex = 5;
            // 
            // cbCheckIP
            // 
            this.cbCheckIP.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbCheckIP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCheckIP.LocalizationText = "IPAddress";
            this.cbCheckIP.Location = new System.Drawing.Point(2, 127);
            this.cbCheckIP.Margin = new System.Windows.Forms.Padding(2);
            this.cbCheckIP.Name = "cbCheckIP";
            this.cbCheckIP.Size = new System.Drawing.Size(74, 36);
            this.cbCheckIP.TabIndex = 4;
            this.cbCheckIP.Text = "IP地址 :";
            this.cbCheckIP.CheckedChanged += new AntdUI.BoolEventHandler(this.cbCheckIP_CheckedChanged);
            // 
            // txtCheckLen
            // 
            this.txtCheckLen.AllowClear = true;
            this.txtCheckLen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCheckLen.LocalizationPlaceholderText = "LeachSetting.Length";
            this.txtCheckLen.Location = new System.Drawing.Point(93, 87);
            this.txtCheckLen.Margin = new System.Windows.Forms.Padding(2);
            this.txtCheckLen.Name = "txtCheckLen";
            this.txtCheckLen.PlaceholderText = "例如：0-99;100";
            this.txtCheckLen.Size = new System.Drawing.Size(255, 36);
            this.txtCheckLen.TabIndex = 3;
            // 
            // cbCheckLen
            // 
            this.cbCheckLen.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbCheckLen.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCheckLen.LocalizationText = "Length";
            this.cbCheckLen.Location = new System.Drawing.Point(2, 87);
            this.cbCheckLen.Margin = new System.Windows.Forms.Padding(2);
            this.cbCheckLen.Name = "cbCheckLen";
            this.cbCheckLen.Size = new System.Drawing.Size(63, 36);
            this.cbCheckLen.TabIndex = 2;
            this.cbCheckLen.Text = "长度 :";
            this.cbCheckLen.CheckedChanged += new AntdUI.BoolEventHandler(this.cbCheckLen_CheckedChanged);
            // 
            // cbCheckSocket
            // 
            this.cbCheckSocket.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbCheckSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbCheckSocket.LocalizationText = "Socket";
            this.cbCheckSocket.Location = new System.Drawing.Point(2, 47);
            this.cbCheckSocket.Margin = new System.Windows.Forms.Padding(2);
            this.cbCheckSocket.Name = "cbCheckSocket";
            this.cbCheckSocket.Size = new System.Drawing.Size(75, 36);
            this.cbCheckSocket.TabIndex = 0;
            this.cbCheckSocket.Text = "套接字 :";
            this.cbCheckSocket.CheckedChanged += new AntdUI.BoolEventHandler(this.cbCheckSocket_CheckedChanged);
            // 
            // txtCheckSocket
            // 
            this.txtCheckSocket.AllowClear = true;
            this.txtCheckSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtCheckSocket.LocalizationPlaceholderText = "SemicolonDelimiter";
            this.txtCheckSocket.Location = new System.Drawing.Point(93, 47);
            this.txtCheckSocket.Margin = new System.Windows.Forms.Padding(2);
            this.txtCheckSocket.Name = "txtCheckSocket";
            this.txtCheckSocket.PlaceholderText = "支持 ; 分隔符";
            this.txtCheckSocket.Size = new System.Drawing.Size(255, 36);
            this.txtCheckSocket.TabIndex = 12;
            // 
            // lIsShow
            // 
            this.lIsShow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lIsShow.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lIsShow.LocalizationText = "LeachSetting.IsShow";
            this.lIsShow.Location = new System.Drawing.Point(2, 2);
            this.lIsShow.Margin = new System.Windows.Forms.Padding(2);
            this.lIsShow.Name = "lIsShow";
            this.lIsShow.Size = new System.Drawing.Size(87, 26);
            this.lIsShow.TabIndex = 13;
            this.lIsShow.Text = "是否显示";
            this.lIsShow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sIsShow
            // 
            this.sIsShow.Checked = true;
            this.sIsShow.CheckedText = "是";
            this.sIsShow.LocalizationCheckedText = "Yes";
            this.sIsShow.LocalizationUnCheckedText = "No";
            this.sIsShow.Location = new System.Drawing.Point(93, 2);
            this.sIsShow.Margin = new System.Windows.Forms.Padding(2);
            this.sIsShow.Name = "sIsShow";
            this.sIsShow.Size = new System.Drawing.Size(49, 26);
            this.sIsShow.TabIndex = 14;
            this.sIsShow.UnCheckedText = "否";
            // 
            // tabPacketType
            // 
            this.tabPacketType.Controls.Add(this.tpProxy);
            this.tabPacketType.Controls.Add(this.tpInject);
            this.tabPacketType.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tabPacketType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabPacketType.Location = new System.Drawing.Point(94, 288);
            this.tabPacketType.Name = "tabPacketType";
            this.tabPacketType.Pages.Add(this.tpInject);
            this.tabPacketType.Pages.Add(this.tpProxy);
            this.tabPacketType.SelectedIndex = 1;
            this.tabPacketType.Size = new System.Drawing.Size(253, 360);
            this.tabPacketType.Style = styleLine1;
            this.tabPacketType.TabIndex = 16;
            // 
            // tpInject
            // 
            this.tpInject.Controls.Add(this.tlpInject);
            this.tpInject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpInject.Location = new System.Drawing.Point(0, 30);
            this.tpInject.Name = "tpInject";
            this.tpInject.Size = new System.Drawing.Size(253, 330);
            this.tpInject.TabIndex = 0;
            this.tpInject.Text = "Inject";
            // 
            // tlpInject
            // 
            this.tlpInject.ColumnCount = 2;
            this.tlpInject.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpInject.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpInject.Controls.Add(this.cbSend, 0, 0);
            this.tlpInject.Controls.Add(this.cbWSARecvFrom, 1, 3);
            this.tlpInject.Controls.Add(this.cbRecvFrom, 0, 3);
            this.tlpInject.Controls.Add(this.cbWSARecv, 1, 2);
            this.tlpInject.Controls.Add(this.cbRecv, 0, 2);
            this.tlpInject.Controls.Add(this.cbWSASendTo, 1, 1);
            this.tlpInject.Controls.Add(this.cbSendTo, 0, 1);
            this.tlpInject.Controls.Add(this.cbWSASend, 1, 0);
            this.tlpInject.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpInject.Location = new System.Drawing.Point(0, 0);
            this.tlpInject.Margin = new System.Windows.Forms.Padding(0);
            this.tlpInject.Name = "tlpInject";
            this.tlpInject.Padding = new System.Windows.Forms.Padding(2);
            this.tlpInject.RowCount = 5;
            this.tlpInject.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpInject.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpInject.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpInject.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpInject.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpInject.Size = new System.Drawing.Size(253, 330);
            this.tlpInject.TabIndex = 5;
            // 
            // cbSend
            // 
            this.cbSend.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbSend.LocalizationText = "Send";
            this.cbSend.Location = new System.Drawing.Point(4, 4);
            this.cbSend.Margin = new System.Windows.Forms.Padding(2);
            this.cbSend.Name = "cbSend";
            this.cbSend.Size = new System.Drawing.Size(56, 28);
            this.cbSend.TabIndex = 51;
            this.cbSend.Text = "发送";
            // 
            // cbWSARecvFrom
            // 
            this.cbWSARecvFrom.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbWSARecvFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbWSARecvFrom.LocalizationText = "WSARecvFrom";
            this.cbWSARecvFrom.Location = new System.Drawing.Point(128, 108);
            this.cbWSARecvFrom.Margin = new System.Windows.Forms.Padding(2);
            this.cbWSARecvFrom.Name = "cbWSARecvFrom";
            this.cbWSARecvFrom.Size = new System.Drawing.Size(100, 32);
            this.cbWSARecvFrom.TabIndex = 50;
            this.cbWSARecvFrom.Text = "WSA 接收自";
            // 
            // cbRecvFrom
            // 
            this.cbRecvFrom.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbRecvFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbRecvFrom.LocalizationText = "RecvFrom";
            this.cbRecvFrom.Location = new System.Drawing.Point(4, 108);
            this.cbRecvFrom.Margin = new System.Windows.Forms.Padding(2);
            this.cbRecvFrom.Name = "cbRecvFrom";
            this.cbRecvFrom.Size = new System.Drawing.Size(68, 32);
            this.cbRecvFrom.TabIndex = 48;
            this.cbRecvFrom.Text = "接收自";
            // 
            // cbWSARecv
            // 
            this.cbWSARecv.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbWSARecv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbWSARecv.LocalizationText = "WSARecv";
            this.cbWSARecv.Location = new System.Drawing.Point(128, 72);
            this.cbWSARecv.Margin = new System.Windows.Forms.Padding(2);
            this.cbWSARecv.Name = "cbWSARecv";
            this.cbWSARecv.Size = new System.Drawing.Size(88, 32);
            this.cbWSARecv.TabIndex = 47;
            this.cbWSARecv.Text = "WSA 接收";
            // 
            // cbRecv
            // 
            this.cbRecv.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbRecv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbRecv.LocalizationText = "Recv";
            this.cbRecv.Location = new System.Drawing.Point(4, 72);
            this.cbRecv.Margin = new System.Windows.Forms.Padding(2);
            this.cbRecv.Name = "cbRecv";
            this.cbRecv.Size = new System.Drawing.Size(56, 32);
            this.cbRecv.TabIndex = 45;
            this.cbRecv.Text = "接收";
            // 
            // cbWSASendTo
            // 
            this.cbWSASendTo.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbWSASendTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbWSASendTo.LocalizationText = "WSASendTo";
            this.cbWSASendTo.Location = new System.Drawing.Point(128, 36);
            this.cbWSASendTo.Margin = new System.Windows.Forms.Padding(2);
            this.cbWSASendTo.Name = "cbWSASendTo";
            this.cbWSASendTo.Size = new System.Drawing.Size(100, 32);
            this.cbWSASendTo.TabIndex = 44;
            this.cbWSASendTo.Text = "WSA 发送到";
            // 
            // cbSendTo
            // 
            this.cbSendTo.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbSendTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbSendTo.LocalizationText = "SendTo";
            this.cbSendTo.Location = new System.Drawing.Point(4, 36);
            this.cbSendTo.Margin = new System.Windows.Forms.Padding(2);
            this.cbSendTo.Name = "cbSendTo";
            this.cbSendTo.Size = new System.Drawing.Size(68, 32);
            this.cbSendTo.TabIndex = 42;
            this.cbSendTo.Text = "发送到";
            // 
            // cbWSASend
            // 
            this.cbWSASend.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbWSASend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbWSASend.LocalizationText = "WSASend";
            this.cbWSASend.Location = new System.Drawing.Point(128, 4);
            this.cbWSASend.Margin = new System.Windows.Forms.Padding(2);
            this.cbWSASend.Name = "cbWSASend";
            this.cbWSASend.Size = new System.Drawing.Size(88, 28);
            this.cbWSASend.TabIndex = 41;
            this.cbWSASend.Text = "WSA 发送";
            // 
            // tpProxy
            // 
            this.tpProxy.Controls.Add(this.tlpProxy);
            this.tpProxy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpProxy.Location = new System.Drawing.Point(0, 30);
            this.tpProxy.Name = "tpProxy";
            this.tpProxy.Size = new System.Drawing.Size(253, 330);
            this.tpProxy.TabIndex = 1;
            this.tpProxy.Text = "Proxy";
            // 
            // tlpProxy
            // 
            this.tlpProxy.ColumnCount = 2;
            this.tlpProxy.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpProxy.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpProxy.Controls.Add(this.cbTCP_Req, 0, 0);
            this.tlpProxy.Controls.Add(this.cbUDP_Resp, 1, 1);
            this.tlpProxy.Controls.Add(this.cbUDP_Req, 1, 0);
            this.tlpProxy.Controls.Add(this.cbTCP_Resp, 0, 1);
            this.tlpProxy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpProxy.Location = new System.Drawing.Point(0, 0);
            this.tlpProxy.Margin = new System.Windows.Forms.Padding(0);
            this.tlpProxy.Name = "tlpProxy";
            this.tlpProxy.Padding = new System.Windows.Forms.Padding(2);
            this.tlpProxy.RowCount = 3;
            this.tlpProxy.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProxy.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProxy.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProxy.Size = new System.Drawing.Size(253, 330);
            this.tlpProxy.TabIndex = 5;
            // 
            // cbTCP_Req
            // 
            this.cbTCP_Req.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbTCP_Req.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbTCP_Req.LocalizationText = "TCPReq";
            this.cbTCP_Req.Location = new System.Drawing.Point(4, 4);
            this.cbTCP_Req.Margin = new System.Windows.Forms.Padding(2);
            this.cbTCP_Req.Name = "cbTCP_Req";
            this.cbTCP_Req.Size = new System.Drawing.Size(82, 28);
            this.cbTCP_Req.TabIndex = 51;
            this.cbTCP_Req.Text = "TCP 请求";
            // 
            // cbUDP_Resp
            // 
            this.cbUDP_Resp.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbUDP_Resp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbUDP_Resp.LocalizationText = "UDPResp";
            this.cbUDP_Resp.Location = new System.Drawing.Point(128, 36);
            this.cbUDP_Resp.Margin = new System.Windows.Forms.Padding(2);
            this.cbUDP_Resp.Name = "cbUDP_Resp";
            this.cbUDP_Resp.Size = new System.Drawing.Size(85, 28);
            this.cbUDP_Resp.TabIndex = 44;
            this.cbUDP_Resp.Text = "UDP 响应";
            // 
            // cbUDP_Req
            // 
            this.cbUDP_Req.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbUDP_Req.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbUDP_Req.LocalizationText = "UDPReq";
            this.cbUDP_Req.Location = new System.Drawing.Point(128, 4);
            this.cbUDP_Req.Margin = new System.Windows.Forms.Padding(2);
            this.cbUDP_Req.Name = "cbUDP_Req";
            this.cbUDP_Req.Size = new System.Drawing.Size(85, 28);
            this.cbUDP_Req.TabIndex = 42;
            this.cbUDP_Req.Text = "UDP 请求";
            // 
            // cbTCP_Resp
            // 
            this.cbTCP_Resp.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbTCP_Resp.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbTCP_Resp.LocalizationText = "TCPResp";
            this.cbTCP_Resp.Location = new System.Drawing.Point(4, 36);
            this.cbTCP_Resp.Margin = new System.Windows.Forms.Padding(2);
            this.cbTCP_Resp.Name = "cbTCP_Resp";
            this.cbTCP_Resp.Size = new System.Drawing.Size(82, 28);
            this.cbTCP_Resp.TabIndex = 41;
            this.cbTCP_Resp.Text = "TCP 响应";
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 5;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Controls.Add(this.bSave, 1, 1);
            this.tlpButton.Controls.Add(this.bExit, 3, 1);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 651);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(350, 49);
            this.tlpButton.TabIndex = 2;
            // 
            // bSave
            // 
            this.bSave.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.LocalizationText = "Save";
            this.bSave.Location = new System.Drawing.Point(103, 6);
            this.bSave.Margin = new System.Windows.Forms.Padding(2);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(63, 37);
            this.bSave.TabIndex = 0;
            this.bSave.Text = "保存";
            this.bSave.Type = AntdUI.TTypeMini.Primary;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bExit
            // 
            this.bExit.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bExit.BackExtend = "135, #6253E1, #04BEFE";
            this.bExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bExit.IconSvg = "LogoutOutlined";
            this.bExit.LocalizationText = "Cancel";
            this.bExit.Location = new System.Drawing.Point(184, 6);
            this.bExit.Margin = new System.Windows.Forms.Padding(2);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(63, 37);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // LeachSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpFLeachSettings);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "LeachSetting";
            this.Size = new System.Drawing.Size(350, 700);
            this.Load += new System.EventHandler(this.LeachSetting_Load);
            this.tlpFLeachSettings.ResumeLayout(false);
            this.tlpContent.ResumeLayout(false);
            this.tlpContent.PerformLayout();
            this.tabPacketType.ResumeLayout(false);
            this.tpInject.ResumeLayout(false);
            this.tlpInject.ResumeLayout(false);
            this.tlpInject.PerformLayout();
            this.tpProxy.ResumeLayout(false);
            this.tlpProxy.ResumeLayout(false);
            this.tlpProxy.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpFLeachSettings;
        private TableLayoutPanelEx tlpContent;
        private AntdUI.Input txtCheckData;
        private AntdUI.Checkbox cbCheckData;
        private AntdUI.Input txtCheckHead;
        private AntdUI.Checkbox cbCheckHead;
        private AntdUI.Input txtCheckPort;
        private AntdUI.Checkbox cbCheckPort;
        private AntdUI.Input txtCheckIP;
        private AntdUI.Checkbox cbCheckIP;
        private AntdUI.Input txtCheckLen;
        private AntdUI.Checkbox cbCheckLen;
        private AntdUI.Checkbox cbCheckSocket;
        private AntdUI.Input txtCheckSocket;
        private AntdUI.Label lIsShow;
        private AntdUI.Switch sIsShow;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private AntdUI.Checkbox cbCheckType;
        private AntdUI.Tabs tabPacketType;
        private AntdUI.TabPage tpInject;
        private AntdUI.TabPage tpProxy;
        private TableLayoutPanelEx tlpProxy;
        private AntdUI.Checkbox cbTCP_Req;
        private AntdUI.Checkbox cbUDP_Resp;
        private AntdUI.Checkbox cbUDP_Req;
        private AntdUI.Checkbox cbTCP_Resp;
        private TableLayoutPanelEx tlpInject;
        private AntdUI.Checkbox cbSend;
        private AntdUI.Checkbox cbWSARecvFrom;
        private AntdUI.Checkbox cbRecvFrom;
        private AntdUI.Checkbox cbWSARecv;
        private AntdUI.Checkbox cbRecv;
        private AntdUI.Checkbox cbWSASendTo;
        private AntdUI.Checkbox cbSendTo;
        private AntdUI.Checkbox cbWSASend;
    }
}
