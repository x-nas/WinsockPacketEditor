namespace WinsockPacketEditor
{
    partial class HookSetting
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
            this.tlpHookSettings = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tabHookSettings = new AntdUI.Tabs();
            this.tpInjectMode = new AntdUI.TabPage();
            this.tlpInjectMode = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpWS2 = new WinsockPacketEditor.TableLayoutPanelEx();
            this.cbHookWSA_RecvFrom = new AntdUI.Checkbox();
            this.cbHookWSA_Recv = new AntdUI.Checkbox();
            this.cbHookWSA_SendTo = new AntdUI.Checkbox();
            this.cbHookWSA_Send = new AntdUI.Checkbox();
            this.cbHookWS2_RecvFrom = new AntdUI.Checkbox();
            this.cbHookWS2_Recv = new AntdUI.Checkbox();
            this.cbHookWS2_SendTo = new AntdUI.Checkbox();
            this.cbHookWS2_Send = new AntdUI.Checkbox();
            this.tlpWS1 = new WinsockPacketEditor.TableLayoutPanelEx();
            this.cbHookWS1_RecvFrom = new AntdUI.Checkbox();
            this.cbHookWS1_Recv = new AntdUI.Checkbox();
            this.cbHookWS1_SendTo = new AntdUI.Checkbox();
            this.cbHookWS1_Send = new AntdUI.Checkbox();
            this.dWS1 = new AntdUI.Divider();
            this.dWS2 = new AntdUI.Divider();
            this.tpProxyMode = new AntdUI.TabPage();
            this.tlpProxyMode = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpUDP = new WinsockPacketEditor.TableLayoutPanelEx();
            this.cbUDP_Resp = new AntdUI.Checkbox();
            this.cbUDP_Req = new AntdUI.Checkbox();
            this.tlpTCP = new WinsockPacketEditor.TableLayoutPanelEx();
            this.cbTCP_Resp = new AntdUI.Checkbox();
            this.cbTCP_Req = new AntdUI.Checkbox();
            this.dTCP = new AntdUI.Divider();
            this.dUDP = new AntdUI.Divider();
            this.dUnPack = new AntdUI.Divider();
            this.tlpUnPack = new WinsockPacketEditor.TableLayoutPanelEx();
            this.cbEnable_UnPack = new AntdUI.Checkbox();
            this.txtUnPack_Head = new AntdUI.Input();
            this.txtUnPack_Length = new AntdUI.Input();
            this.tlpHookSettings.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tabHookSettings.SuspendLayout();
            this.tpInjectMode.SuspendLayout();
            this.tlpInjectMode.SuspendLayout();
            this.tlpWS2.SuspendLayout();
            this.tlpWS1.SuspendLayout();
            this.tpProxyMode.SuspendLayout();
            this.tlpProxyMode.SuspendLayout();
            this.tlpUDP.SuspendLayout();
            this.tlpTCP.SuspendLayout();
            this.tlpUnPack.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpHookSettings
            // 
            this.tlpHookSettings.ColumnCount = 1;
            this.tlpHookSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpHookSettings.Controls.Add(this.tlpButton, 0, 1);
            this.tlpHookSettings.Controls.Add(this.tabHookSettings, 0, 0);
            this.tlpHookSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpHookSettings.Location = new System.Drawing.Point(0, 0);
            this.tlpHookSettings.Margin = new System.Windows.Forms.Padding(0);
            this.tlpHookSettings.Name = "tlpHookSettings";
            this.tlpHookSettings.RowCount = 2;
            this.tlpHookSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpHookSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tlpHookSettings.Size = new System.Drawing.Size(350, 700);
            this.tlpHookSettings.TabIndex = 1;
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
            this.tlpButton.TabIndex = 4;
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
            // tabHookSettings
            // 
            this.tabHookSettings.Controls.Add(this.tpProxyMode);
            this.tabHookSettings.Controls.Add(this.tpInjectMode);
            this.tabHookSettings.Cursor = System.Windows.Forms.Cursors.Default;
            this.tabHookSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabHookSettings.Location = new System.Drawing.Point(2, 2);
            this.tabHookSettings.Margin = new System.Windows.Forms.Padding(2);
            this.tabHookSettings.Name = "tabHookSettings";
            this.tabHookSettings.Pages.Add(this.tpInjectMode);
            this.tabHookSettings.Pages.Add(this.tpProxyMode);
            this.tabHookSettings.SelectedIndex = 1;
            this.tabHookSettings.Size = new System.Drawing.Size(346, 647);
            this.tabHookSettings.Style = styleLine1;
            this.tabHookSettings.TabIndex = 0;
            this.tabHookSettings.Text = "tabs1";
            // 
            // tpInjectMode
            // 
            this.tpInjectMode.Controls.Add(this.tlpInjectMode);
            this.tpInjectMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpInjectMode.Location = new System.Drawing.Point(0, 28);
            this.tpInjectMode.Margin = new System.Windows.Forms.Padding(2);
            this.tpInjectMode.Name = "tpInjectMode";
            this.tpInjectMode.Size = new System.Drawing.Size(346, 619);
            this.tpInjectMode.TabIndex = 0;
            this.tpInjectMode.Text = "tpInjectMode";
            // 
            // tlpInjectMode
            // 
            this.tlpInjectMode.ColumnCount = 1;
            this.tlpInjectMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpInjectMode.Controls.Add(this.tlpWS2, 0, 6);
            this.tlpInjectMode.Controls.Add(this.tlpWS1, 0, 2);
            this.tlpInjectMode.Controls.Add(this.dWS1, 0, 0);
            this.tlpInjectMode.Controls.Add(this.dWS2, 0, 4);
            this.tlpInjectMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpInjectMode.Location = new System.Drawing.Point(0, 0);
            this.tlpInjectMode.Margin = new System.Windows.Forms.Padding(0);
            this.tlpInjectMode.Name = "tlpInjectMode";
            this.tlpInjectMode.RowCount = 7;
            this.tlpInjectMode.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpInjectMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpInjectMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            this.tlpInjectMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpInjectMode.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpInjectMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpInjectMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpInjectMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpInjectMode.Size = new System.Drawing.Size(346, 619);
            this.tlpInjectMode.TabIndex = 1;
            // 
            // tlpWS2
            // 
            this.tlpWS2.ColumnCount = 2;
            this.tlpWS2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpWS2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpWS2.Controls.Add(this.cbHookWSA_RecvFrom, 1, 3);
            this.tlpWS2.Controls.Add(this.cbHookWSA_Recv, 0, 3);
            this.tlpWS2.Controls.Add(this.cbHookWSA_SendTo, 1, 2);
            this.tlpWS2.Controls.Add(this.cbHookWSA_Send, 0, 2);
            this.tlpWS2.Controls.Add(this.cbHookWS2_RecvFrom, 1, 1);
            this.tlpWS2.Controls.Add(this.cbHookWS2_Recv, 0, 1);
            this.tlpWS2.Controls.Add(this.cbHookWS2_SendTo, 1, 0);
            this.tlpWS2.Controls.Add(this.cbHookWS2_Send, 0, 0);
            this.tlpWS2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpWS2.Location = new System.Drawing.Point(0, 175);
            this.tlpWS2.Margin = new System.Windows.Forms.Padding(0);
            this.tlpWS2.Name = "tlpWS2";
            this.tlpWS2.RowCount = 5;
            this.tlpWS2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpWS2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpWS2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpWS2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpWS2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpWS2.Size = new System.Drawing.Size(346, 444);
            this.tlpWS2.TabIndex = 7;
            // 
            // cbHookWSA_RecvFrom
            // 
            this.cbHookWSA_RecvFrom.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbHookWSA_RecvFrom.LocalizationText = "HookSettingsForm.WSARecvFrom";
            this.cbHookWSA_RecvFrom.Location = new System.Drawing.Point(175, 110);
            this.cbHookWSA_RecvFrom.Margin = new System.Windows.Forms.Padding(2);
            this.cbHookWSA_RecvFrom.Name = "cbHookWSA_RecvFrom";
            this.cbHookWSA_RecvFrom.Size = new System.Drawing.Size(96, 32);
            this.cbHookWSA_RecvFrom.TabIndex = 8;
            this.cbHookWSA_RecvFrom.Text = "WSA接收自";
            // 
            // cbHookWSA_Recv
            // 
            this.cbHookWSA_Recv.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbHookWSA_Recv.LocalizationText = "HookSettingsForm.WSARecv";
            this.cbHookWSA_Recv.Location = new System.Drawing.Point(2, 110);
            this.cbHookWSA_Recv.Margin = new System.Windows.Forms.Padding(2);
            this.cbHookWSA_Recv.Name = "cbHookWSA_Recv";
            this.cbHookWSA_Recv.Size = new System.Drawing.Size(84, 32);
            this.cbHookWSA_Recv.TabIndex = 7;
            this.cbHookWSA_Recv.Text = "WSA接收";
            // 
            // cbHookWSA_SendTo
            // 
            this.cbHookWSA_SendTo.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbHookWSA_SendTo.LocalizationText = "HookSettingsForm.WSASendTo";
            this.cbHookWSA_SendTo.Location = new System.Drawing.Point(175, 74);
            this.cbHookWSA_SendTo.Margin = new System.Windows.Forms.Padding(2);
            this.cbHookWSA_SendTo.Name = "cbHookWSA_SendTo";
            this.cbHookWSA_SendTo.Size = new System.Drawing.Size(96, 32);
            this.cbHookWSA_SendTo.TabIndex = 6;
            this.cbHookWSA_SendTo.Text = "WSA发送到";
            // 
            // cbHookWSA_Send
            // 
            this.cbHookWSA_Send.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbHookWSA_Send.LocalizationText = "HookSettingsForm.WSASend";
            this.cbHookWSA_Send.Location = new System.Drawing.Point(2, 74);
            this.cbHookWSA_Send.Margin = new System.Windows.Forms.Padding(2);
            this.cbHookWSA_Send.Name = "cbHookWSA_Send";
            this.cbHookWSA_Send.Size = new System.Drawing.Size(84, 32);
            this.cbHookWSA_Send.TabIndex = 5;
            this.cbHookWSA_Send.Text = "WSA发送";
            // 
            // cbHookWS2_RecvFrom
            // 
            this.cbHookWS2_RecvFrom.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbHookWS2_RecvFrom.LocalizationText = "HookSettingsForm.RecvFrom";
            this.cbHookWS2_RecvFrom.Location = new System.Drawing.Point(175, 38);
            this.cbHookWS2_RecvFrom.Margin = new System.Windows.Forms.Padding(2);
            this.cbHookWS2_RecvFrom.Name = "cbHookWS2_RecvFrom";
            this.cbHookWS2_RecvFrom.Size = new System.Drawing.Size(68, 32);
            this.cbHookWS2_RecvFrom.TabIndex = 4;
            this.cbHookWS2_RecvFrom.Text = "接收自";
            // 
            // cbHookWS2_Recv
            // 
            this.cbHookWS2_Recv.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbHookWS2_Recv.LocalizationText = "HookSettingsForm.Recv";
            this.cbHookWS2_Recv.Location = new System.Drawing.Point(2, 38);
            this.cbHookWS2_Recv.Margin = new System.Windows.Forms.Padding(2);
            this.cbHookWS2_Recv.Name = "cbHookWS2_Recv";
            this.cbHookWS2_Recv.Size = new System.Drawing.Size(56, 32);
            this.cbHookWS2_Recv.TabIndex = 3;
            this.cbHookWS2_Recv.Text = "接收";
            // 
            // cbHookWS2_SendTo
            // 
            this.cbHookWS2_SendTo.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbHookWS2_SendTo.LocalizationText = "HookSettingsForm.SendTo";
            this.cbHookWS2_SendTo.Location = new System.Drawing.Point(175, 2);
            this.cbHookWS2_SendTo.Margin = new System.Windows.Forms.Padding(2);
            this.cbHookWS2_SendTo.Name = "cbHookWS2_SendTo";
            this.cbHookWS2_SendTo.Size = new System.Drawing.Size(68, 32);
            this.cbHookWS2_SendTo.TabIndex = 2;
            this.cbHookWS2_SendTo.Text = "发送到";
            // 
            // cbHookWS2_Send
            // 
            this.cbHookWS2_Send.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbHookWS2_Send.LocalizationText = "HookSettingsForm.Send";
            this.cbHookWS2_Send.Location = new System.Drawing.Point(2, 2);
            this.cbHookWS2_Send.Margin = new System.Windows.Forms.Padding(2);
            this.cbHookWS2_Send.Name = "cbHookWS2_Send";
            this.cbHookWS2_Send.Size = new System.Drawing.Size(56, 32);
            this.cbHookWS2_Send.TabIndex = 1;
            this.cbHookWS2_Send.Text = "发送";
            // 
            // tlpWS1
            // 
            this.tlpWS1.ColumnCount = 2;
            this.tlpWS1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpWS1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpWS1.Controls.Add(this.cbHookWS1_RecvFrom, 1, 1);
            this.tlpWS1.Controls.Add(this.cbHookWS1_Recv, 0, 1);
            this.tlpWS1.Controls.Add(this.cbHookWS1_SendTo, 1, 0);
            this.tlpWS1.Controls.Add(this.cbHookWS1_Send, 0, 0);
            this.tlpWS1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpWS1.Location = new System.Drawing.Point(0, 39);
            this.tlpWS1.Margin = new System.Windows.Forms.Padding(0);
            this.tlpWS1.Name = "tlpWS1";
            this.tlpWS1.RowCount = 3;
            this.tlpWS1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpWS1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpWS1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpWS1.Size = new System.Drawing.Size(346, 81);
            this.tlpWS1.TabIndex = 5;
            // 
            // cbHookWS1_RecvFrom
            // 
            this.cbHookWS1_RecvFrom.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbHookWS1_RecvFrom.LocalizationText = "HookSettingsForm.RecvFrom1";
            this.cbHookWS1_RecvFrom.Location = new System.Drawing.Point(175, 38);
            this.cbHookWS1_RecvFrom.Margin = new System.Windows.Forms.Padding(2);
            this.cbHookWS1_RecvFrom.Name = "cbHookWS1_RecvFrom";
            this.cbHookWS1_RecvFrom.Size = new System.Drawing.Size(89, 32);
            this.cbHookWS1_RecvFrom.TabIndex = 3;
            this.cbHookWS1_RecvFrom.Text = "接收自 1.1";
            // 
            // cbHookWS1_Recv
            // 
            this.cbHookWS1_Recv.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbHookWS1_Recv.LocalizationText = "HookSettingsForm.Recv1";
            this.cbHookWS1_Recv.Location = new System.Drawing.Point(2, 38);
            this.cbHookWS1_Recv.Margin = new System.Windows.Forms.Padding(2);
            this.cbHookWS1_Recv.Name = "cbHookWS1_Recv";
            this.cbHookWS1_Recv.Size = new System.Drawing.Size(77, 32);
            this.cbHookWS1_Recv.TabIndex = 2;
            this.cbHookWS1_Recv.Text = "接收 1.1";
            // 
            // cbHookWS1_SendTo
            // 
            this.cbHookWS1_SendTo.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbHookWS1_SendTo.LocalizationText = "HookSettingsForm.SendTo1";
            this.cbHookWS1_SendTo.Location = new System.Drawing.Point(175, 2);
            this.cbHookWS1_SendTo.Margin = new System.Windows.Forms.Padding(2);
            this.cbHookWS1_SendTo.Name = "cbHookWS1_SendTo";
            this.cbHookWS1_SendTo.Size = new System.Drawing.Size(89, 32);
            this.cbHookWS1_SendTo.TabIndex = 1;
            this.cbHookWS1_SendTo.Text = "发送到 1.1";
            // 
            // cbHookWS1_Send
            // 
            this.cbHookWS1_Send.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbHookWS1_Send.LocalizationText = "HookSettingsForm.Send1";
            this.cbHookWS1_Send.Location = new System.Drawing.Point(2, 2);
            this.cbHookWS1_Send.Margin = new System.Windows.Forms.Padding(2);
            this.cbHookWS1_Send.Name = "cbHookWS1_Send";
            this.cbHookWS1_Send.Size = new System.Drawing.Size(77, 32);
            this.cbHookWS1_Send.TabIndex = 0;
            this.cbHookWS1_Send.Text = "发送 1.1";
            // 
            // dWS1
            // 
            this.dWS1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dWS1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dWS1.Location = new System.Drawing.Point(2, 2);
            this.dWS1.Margin = new System.Windows.Forms.Padding(2);
            this.dWS1.Name = "dWS1";
            this.dWS1.Orientation = AntdUI.TOrientation.Left;
            this.dWS1.Size = new System.Drawing.Size(342, 19);
            this.dWS1.TabIndex = 4;
            this.dWS1.Text = "Winsock 1.1";
            // 
            // dWS2
            // 
            this.dWS2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dWS2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dWS2.Location = new System.Drawing.Point(2, 138);
            this.dWS2.Margin = new System.Windows.Forms.Padding(2);
            this.dWS2.Name = "dWS2";
            this.dWS2.Orientation = AntdUI.TOrientation.Left;
            this.dWS2.Size = new System.Drawing.Size(342, 19);
            this.dWS2.TabIndex = 6;
            this.dWS2.Text = "Winsock 2.0";
            // 
            // tpProxyMode
            // 
            this.tpProxyMode.Controls.Add(this.tlpProxyMode);
            this.tpProxyMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tpProxyMode.Location = new System.Drawing.Point(0, 28);
            this.tpProxyMode.Margin = new System.Windows.Forms.Padding(2);
            this.tpProxyMode.Name = "tpProxyMode";
            this.tpProxyMode.Showed = true;
            this.tpProxyMode.Size = new System.Drawing.Size(346, 619);
            this.tpProxyMode.TabIndex = 1;
            this.tpProxyMode.Text = "tpProxyMode";
            // 
            // tlpProxyMode
            // 
            this.tlpProxyMode.ColumnCount = 1;
            this.tlpProxyMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProxyMode.Controls.Add(this.dUnPack, 0, 8);
            this.tlpProxyMode.Controls.Add(this.tlpUDP, 0, 6);
            this.tlpProxyMode.Controls.Add(this.tlpTCP, 0, 2);
            this.tlpProxyMode.Controls.Add(this.dTCP, 0, 0);
            this.tlpProxyMode.Controls.Add(this.dUDP, 0, 4);
            this.tlpProxyMode.Controls.Add(this.tlpUnPack, 0, 10);
            this.tlpProxyMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpProxyMode.Location = new System.Drawing.Point(0, 0);
            this.tlpProxyMode.Margin = new System.Windows.Forms.Padding(0);
            this.tlpProxyMode.Name = "tlpProxyMode";
            this.tlpProxyMode.RowCount = 12;
            this.tlpProxyMode.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProxyMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tlpProxyMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpProxyMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tlpProxyMode.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProxyMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tlpProxyMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpProxyMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tlpProxyMode.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProxyMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 15F));
            this.tlpProxyMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.tlpProxyMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProxyMode.Size = new System.Drawing.Size(346, 619);
            this.tlpProxyMode.TabIndex = 2;
            // 
            // tlpUDP
            // 
            this.tlpUDP.ColumnCount = 2;
            this.tlpUDP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpUDP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpUDP.Controls.Add(this.cbUDP_Resp, 1, 0);
            this.tlpUDP.Controls.Add(this.cbUDP_Req, 0, 0);
            this.tlpUDP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpUDP.Location = new System.Drawing.Point(0, 141);
            this.tlpUDP.Margin = new System.Windows.Forms.Padding(0);
            this.tlpUDP.Name = "tlpUDP";
            this.tlpUDP.RowCount = 2;
            this.tlpUDP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpUDP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpUDP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpUDP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpUDP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpUDP.Size = new System.Drawing.Size(346, 50);
            this.tlpUDP.TabIndex = 7;
            // 
            // cbUDP_Resp
            // 
            this.cbUDP_Resp.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbUDP_Resp.LocalizationText = "Response";
            this.cbUDP_Resp.Location = new System.Drawing.Point(175, 2);
            this.cbUDP_Resp.Margin = new System.Windows.Forms.Padding(2);
            this.cbUDP_Resp.Name = "cbUDP_Resp";
            this.cbUDP_Resp.Size = new System.Drawing.Size(56, 32);
            this.cbUDP_Resp.TabIndex = 2;
            this.cbUDP_Resp.Text = "响应";
            // 
            // cbUDP_Req
            // 
            this.cbUDP_Req.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbUDP_Req.LocalizationText = "Request";
            this.cbUDP_Req.Location = new System.Drawing.Point(2, 2);
            this.cbUDP_Req.Margin = new System.Windows.Forms.Padding(2);
            this.cbUDP_Req.Name = "cbUDP_Req";
            this.cbUDP_Req.Size = new System.Drawing.Size(56, 32);
            this.cbUDP_Req.TabIndex = 1;
            this.cbUDP_Req.Text = "请求";
            // 
            // tlpTCP
            // 
            this.tlpTCP.ColumnCount = 2;
            this.tlpTCP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpTCP.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpTCP.Controls.Add(this.cbTCP_Resp, 1, 0);
            this.tlpTCP.Controls.Add(this.cbTCP_Req, 0, 0);
            this.tlpTCP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpTCP.Location = new System.Drawing.Point(0, 38);
            this.tlpTCP.Margin = new System.Windows.Forms.Padding(0);
            this.tlpTCP.Name = "tlpTCP";
            this.tlpTCP.RowCount = 2;
            this.tlpTCP.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpTCP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpTCP.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpTCP.Size = new System.Drawing.Size(346, 50);
            this.tlpTCP.TabIndex = 5;
            // 
            // cbTCP_Resp
            // 
            this.cbTCP_Resp.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbTCP_Resp.LocalizationText = "Response";
            this.cbTCP_Resp.Location = new System.Drawing.Point(175, 2);
            this.cbTCP_Resp.Margin = new System.Windows.Forms.Padding(2);
            this.cbTCP_Resp.Name = "cbTCP_Resp";
            this.cbTCP_Resp.Size = new System.Drawing.Size(56, 32);
            this.cbTCP_Resp.TabIndex = 1;
            this.cbTCP_Resp.Text = "响应";
            // 
            // cbTCP_Req
            // 
            this.cbTCP_Req.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbTCP_Req.LocalizationText = "Request";
            this.cbTCP_Req.Location = new System.Drawing.Point(2, 2);
            this.cbTCP_Req.Margin = new System.Windows.Forms.Padding(2);
            this.cbTCP_Req.Name = "cbTCP_Req";
            this.cbTCP_Req.Size = new System.Drawing.Size(56, 32);
            this.cbTCP_Req.TabIndex = 0;
            this.cbTCP_Req.Text = "请求";
            // 
            // dTCP
            // 
            this.dTCP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dTCP.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dTCP.LocalizationText = "HookSettingsForm.TCP";
            this.dTCP.Location = new System.Drawing.Point(2, 2);
            this.dTCP.Margin = new System.Windows.Forms.Padding(2);
            this.dTCP.Name = "dTCP";
            this.dTCP.Orientation = AntdUI.TOrientation.Left;
            this.dTCP.Size = new System.Drawing.Size(342, 19);
            this.dTCP.TabIndex = 4;
            this.dTCP.Text = "TCP 协议";
            // 
            // dUDP
            // 
            this.dUDP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dUDP.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dUDP.LocalizationText = "HookSettingsForm.UDP";
            this.dUDP.Location = new System.Drawing.Point(2, 105);
            this.dUDP.Margin = new System.Windows.Forms.Padding(2);
            this.dUDP.Name = "dUDP";
            this.dUDP.Orientation = AntdUI.TOrientation.Left;
            this.dUDP.Size = new System.Drawing.Size(342, 19);
            this.dUDP.TabIndex = 6;
            this.dUDP.Text = "UDP 协议";
            // 
            // dUnPack
            // 
            this.dUnPack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dUnPack.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dUnPack.LocalizationText = "HookSettingsForm.UDP";
            this.dUnPack.Location = new System.Drawing.Point(2, 208);
            this.dUnPack.Margin = new System.Windows.Forms.Padding(2);
            this.dUnPack.Name = "dUnPack";
            this.dUnPack.Orientation = AntdUI.TOrientation.Left;
            this.dUnPack.Size = new System.Drawing.Size(342, 19);
            this.dUnPack.TabIndex = 8;
            this.dUnPack.Text = "拆包设置";
            // 
            // tlpUnPack
            // 
            this.tlpUnPack.ColumnCount = 2;
            this.tlpUnPack.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpUnPack.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpUnPack.Controls.Add(this.txtUnPack_Length, 1, 1);
            this.tlpUnPack.Controls.Add(this.cbEnable_UnPack, 0, 0);
            this.tlpUnPack.Controls.Add(this.txtUnPack_Head, 1, 0);
            this.tlpUnPack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpUnPack.Location = new System.Drawing.Point(0, 244);
            this.tlpUnPack.Margin = new System.Windows.Forms.Padding(0);
            this.tlpUnPack.Name = "tlpUnPack";
            this.tlpUnPack.RowCount = 2;
            this.tlpUnPack.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpUnPack.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpUnPack.Size = new System.Drawing.Size(346, 80);
            this.tlpUnPack.TabIndex = 9;
            // 
            // cbEnable_UnPack
            // 
            this.cbEnable_UnPack.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.cbEnable_UnPack.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbEnable_UnPack.Location = new System.Drawing.Point(3, 3);
            this.cbEnable_UnPack.Name = "cbEnable_UnPack";
            this.cbEnable_UnPack.Size = new System.Drawing.Size(87, 32);
            this.cbEnable_UnPack.TabIndex = 0;
            this.cbEnable_UnPack.Text = "启用拆包 :";
            this.cbEnable_UnPack.CheckedChanged += new AntdUI.BoolEventHandler(this.cbEnable_UnPack_CheckedChanged);
            // 
            // txtUnPack_Head
            // 
            this.txtUnPack_Head.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUnPack_Head.Location = new System.Drawing.Point(95, 2);
            this.txtUnPack_Head.Margin = new System.Windows.Forms.Padding(2);
            this.txtUnPack_Head.Name = "txtUnPack_Head";
            this.txtUnPack_Head.PlaceholderText = "例如 01 00 00";
            this.txtUnPack_Head.PrefixText = "指定包头 :";
            this.txtUnPack_Head.Size = new System.Drawing.Size(249, 34);
            this.txtUnPack_Head.TabIndex = 1;
            // 
            // txtUnPack_Length
            // 
            this.txtUnPack_Length.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtUnPack_Length.Location = new System.Drawing.Point(95, 40);
            this.txtUnPack_Length.Margin = new System.Windows.Forms.Padding(2);
            this.txtUnPack_Length.Name = "txtUnPack_Length";
            this.txtUnPack_Length.PlaceholderText = "例如 4-5";
            this.txtUnPack_Length.PrefixText = "长度位置 :";
            this.txtUnPack_Length.Size = new System.Drawing.Size(249, 38);
            this.txtUnPack_Length.TabIndex = 2;
            // 
            // HookSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpHookSettings);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "HookSetting";
            this.Size = new System.Drawing.Size(350, 700);
            this.Load += new System.EventHandler(this.HookSetting_Load);
            this.tlpHookSettings.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.tabHookSettings.ResumeLayout(false);
            this.tpInjectMode.ResumeLayout(false);
            this.tlpInjectMode.ResumeLayout(false);
            this.tlpWS2.ResumeLayout(false);
            this.tlpWS2.PerformLayout();
            this.tlpWS1.ResumeLayout(false);
            this.tlpWS1.PerformLayout();
            this.tpProxyMode.ResumeLayout(false);
            this.tlpProxyMode.ResumeLayout(false);
            this.tlpUDP.ResumeLayout(false);
            this.tlpUDP.PerformLayout();
            this.tlpTCP.ResumeLayout(false);
            this.tlpTCP.PerformLayout();
            this.tlpUnPack.ResumeLayout(false);
            this.tlpUnPack.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpHookSettings;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private AntdUI.Tabs tabHookSettings;
        private AntdUI.TabPage tpInjectMode;
        private TableLayoutPanelEx tlpInjectMode;
        private TableLayoutPanelEx tlpWS2;
        private AntdUI.Checkbox cbHookWSA_RecvFrom;
        private AntdUI.Checkbox cbHookWSA_Recv;
        private AntdUI.Checkbox cbHookWSA_SendTo;
        private AntdUI.Checkbox cbHookWSA_Send;
        private AntdUI.Checkbox cbHookWS2_RecvFrom;
        private AntdUI.Checkbox cbHookWS2_Recv;
        private AntdUI.Checkbox cbHookWS2_SendTo;
        private AntdUI.Checkbox cbHookWS2_Send;
        private TableLayoutPanelEx tlpWS1;
        private AntdUI.Checkbox cbHookWS1_RecvFrom;
        private AntdUI.Checkbox cbHookWS1_Recv;
        private AntdUI.Checkbox cbHookWS1_SendTo;
        private AntdUI.Checkbox cbHookWS1_Send;
        private AntdUI.Divider dWS1;
        private AntdUI.Divider dWS2;
        private AntdUI.TabPage tpProxyMode;
        private TableLayoutPanelEx tlpProxyMode;
        private TableLayoutPanelEx tlpUDP;
        private AntdUI.Checkbox cbUDP_Resp;
        private AntdUI.Checkbox cbUDP_Req;
        private TableLayoutPanelEx tlpTCP;
        private AntdUI.Checkbox cbTCP_Resp;
        private AntdUI.Checkbox cbTCP_Req;
        private AntdUI.Divider dTCP;
        private AntdUI.Divider dUDP;
        private AntdUI.Divider dUnPack;
        private TableLayoutPanelEx tlpUnPack;
        private AntdUI.Checkbox cbEnable_UnPack;
        private AntdUI.Input txtUnPack_Head;
        private AntdUI.Input txtUnPack_Length;
    }
}
