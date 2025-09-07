namespace WinsockPacketEditor
{
    partial class RobotEdit
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
            this.tlpRobotEdit = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bStop = new AntdUI.Button();
            this.bExecute = new AntdUI.Button();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tlpRobotINST = new WinsockPacketEditor.TableLayoutPanelEx();
            this.cRobotINST = new AntdUI.Collapse();
            this.ciPacketINST = new AntdUI.CollapseItem();
            this.tlpPacketINST = new WinsockPacketEditor.TableLayoutPanelEx();
            this.pSYSSocket = new AntdUI.Panel();
            this.tlpSYSSocket2 = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpSYSSocket = new WinsockPacketEditor.TableLayoutPanelEx();
            this.lSelectFilter = new AntdUI.Label();
            this.rbSelectSocket = new AntdUI.Radio();
            this.rbSelectFilter = new AntdUI.Radio();
            this.bInsert_SYSSocket = new AntdUI.Button();
            this.rbSelectPacket = new AntdUI.Radio();
            this.lSelectPacket = new AntdUI.Label();
            this.nudSelectSocket = new AntdUI.InputNumber();
            this.dSYSSocket = new AntdUI.Divider();
            this.pPacketList = new AntdUI.Panel();
            this.tlpPacketList2 = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpPacketList = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bInsert_PacketList = new AntdUI.Button();
            this.lPacketList = new AntdUI.Label();
            this.dPacketList = new AntdUI.Divider();
            this.pSendList = new AntdUI.Panel();
            this.tlpSendList2 = new WinsockPacketEditor.TableLayoutPanelEx();
            this.dSendList = new AntdUI.Divider();
            this.tlpSendList = new WinsockPacketEditor.TableLayoutPanelEx();
            this.ddlSendList = new AntdUI.Select();
            this.bInsert_SendList = new AntdUI.Button();
            this.ciControlINST = new AntdUI.CollapseItem();
            this.tlpControlINST = new WinsockPacketEditor.TableLayoutPanelEx();
            this.pLoop = new AntdUI.Panel();
            this.tlpLoop = new WinsockPacketEditor.TableLayoutPanelEx();
            this.dLoop = new AntdUI.Divider();
            this.tlpLoop2 = new WinsockPacketEditor.TableLayoutPanelEx();
            this.nudLoop = new AntdUI.InputNumber();
            this.bInsert_LoopStart = new AntdUI.Button();
            this.bInsert_LoopEnd = new AntdUI.Button();
            this.pDelay = new AntdUI.Panel();
            this.tlpDelay = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpDelay2 = new WinsockPacketEditor.TableLayoutPanelEx();
            this.nudnudDelayRandom_To = new AntdUI.InputNumber();
            this.nudDelayFix = new AntdUI.InputNumber();
            this.rbDelayRandom = new AntdUI.Radio();
            this.bInsert_Delay = new AntdUI.Button();
            this.rbDelayFix = new AntdUI.Radio();
            this.nudnudDelayRandom_From = new AntdUI.InputNumber();
            this.dDelay = new AntdUI.Divider();
            this.ciKeyBoardINST = new AntdUI.CollapseItem();
            this.tlpKeyboardINST = new WinsockPacketEditor.TableLayoutPanelEx();
            this.pText = new AntdUI.Panel();
            this.tlpText = new WinsockPacketEditor.TableLayoutPanelEx();
            this.dText = new AntdUI.Divider();
            this.tlpText2 = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bInsert_Text = new AntdUI.Button();
            this.txtText = new AntdUI.Input();
            this.pKeyCombination = new AntdUI.Panel();
            this.tlpKeyCombination = new WinsockPacketEditor.TableLayoutPanelEx();
            this.dKeyCombination = new AntdUI.Divider();
            this.tlpKeyCombination2 = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bInsert_KeyCombination = new AntdUI.Button();
            this.txtKeyCombination = new WinsockPacketEditor.HotkeyTextBox();
            this.pKeyBoard = new AntdUI.Panel();
            this.tlpKey = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpKey2 = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bInsert_KeyBoard = new AntdUI.Button();
            this.lkey = new AntdUI.Label();
            this.lKeyType = new AntdUI.Label();
            this.txtKey = new AntdUI.Input();
            this.ddlKeyType = new AntdUI.Select();
            this.dkey = new AntdUI.Divider();
            this.ciMouseINST = new AntdUI.CollapseItem();
            this.tlpMouseINST = new WinsockPacketEditor.TableLayoutPanelEx();
            this.pMouseMove = new AntdUI.Panel();
            this.tlpMouseMove = new WinsockPacketEditor.TableLayoutPanelEx();
            this.divider8 = new AntdUI.Divider();
            this.tlpMouseMove2 = new WinsockPacketEditor.TableLayoutPanelEx();
            this.nudMouseMove_X = new AntdUI.InputNumber();
            this.rbMoveBy = new AntdUI.Radio();
            this.bInsert_MouseMove = new AntdUI.Button();
            this.nudMouseMove_Y = new AntdUI.InputNumber();
            this.rbMoveTo = new AntdUI.Radio();
            this.pMouseWheel = new AntdUI.Panel();
            this.tlpMouseWheel = new WinsockPacketEditor.TableLayoutPanelEx();
            this.dMouseWheel = new AntdUI.Divider();
            this.tlpMouseWheel2 = new WinsockPacketEditor.TableLayoutPanelEx();
            this.lWheelDistance = new AntdUI.Label();
            this.ddlMouseWheel = new AntdUI.Select();
            this.lMouseWheel = new AntdUI.Label();
            this.bInsert_MouseWheel = new AntdUI.Button();
            this.nudWheelDistance = new AntdUI.InputNumber();
            this.pMouseKey = new AntdUI.Panel();
            this.tlpMouseKey = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpMouseKey2 = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bInsert_MouseKey = new AntdUI.Button();
            this.lMouseKey = new AntdUI.Label();
            this.ddlMouseKey = new AntdUI.Select();
            this.dMouseKey = new AntdUI.Divider();
            this.pRobotINST = new AntdUI.Panel();
            this.tRobotInstruction = new AntdUI.Table();
            this.txtINSTLog = new AntdUI.Input();
            this.txtRobotName = new AntdUI.Input();
            this.tlpRobotEdit.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpRobotINST.SuspendLayout();
            this.cRobotINST.SuspendLayout();
            this.ciPacketINST.SuspendLayout();
            this.tlpPacketINST.SuspendLayout();
            this.pSYSSocket.SuspendLayout();
            this.tlpSYSSocket2.SuspendLayout();
            this.tlpSYSSocket.SuspendLayout();
            this.pPacketList.SuspendLayout();
            this.tlpPacketList2.SuspendLayout();
            this.tlpPacketList.SuspendLayout();
            this.pSendList.SuspendLayout();
            this.tlpSendList2.SuspendLayout();
            this.tlpSendList.SuspendLayout();
            this.ciControlINST.SuspendLayout();
            this.tlpControlINST.SuspendLayout();
            this.pLoop.SuspendLayout();
            this.tlpLoop.SuspendLayout();
            this.tlpLoop2.SuspendLayout();
            this.pDelay.SuspendLayout();
            this.tlpDelay.SuspendLayout();
            this.tlpDelay2.SuspendLayout();
            this.ciKeyBoardINST.SuspendLayout();
            this.tlpKeyboardINST.SuspendLayout();
            this.pText.SuspendLayout();
            this.tlpText.SuspendLayout();
            this.tlpText2.SuspendLayout();
            this.pKeyCombination.SuspendLayout();
            this.tlpKeyCombination.SuspendLayout();
            this.tlpKeyCombination2.SuspendLayout();
            this.pKeyBoard.SuspendLayout();
            this.tlpKey.SuspendLayout();
            this.tlpKey2.SuspendLayout();
            this.ciMouseINST.SuspendLayout();
            this.tlpMouseINST.SuspendLayout();
            this.pMouseMove.SuspendLayout();
            this.tlpMouseMove.SuspendLayout();
            this.tlpMouseMove2.SuspendLayout();
            this.pMouseWheel.SuspendLayout();
            this.tlpMouseWheel.SuspendLayout();
            this.tlpMouseWheel2.SuspendLayout();
            this.pMouseKey.SuspendLayout();
            this.tlpMouseKey.SuspendLayout();
            this.tlpMouseKey2.SuspendLayout();
            this.pRobotINST.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpRobotEdit
            // 
            this.tlpRobotEdit.ColumnCount = 1;
            this.tlpRobotEdit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRobotEdit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpRobotEdit.Controls.Add(this.tlpButton, 0, 1);
            this.tlpRobotEdit.Controls.Add(this.tlpRobotINST, 0, 0);
            this.tlpRobotEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRobotEdit.Location = new System.Drawing.Point(0, 0);
            this.tlpRobotEdit.Margin = new System.Windows.Forms.Padding(0);
            this.tlpRobotEdit.Name = "tlpRobotEdit";
            this.tlpRobotEdit.RowCount = 2;
            this.tlpRobotEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRobotEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpRobotEdit.Size = new System.Drawing.Size(1100, 700);
            this.tlpRobotEdit.TabIndex = 1;
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 9;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Controls.Add(this.bStop, 3, 1);
            this.tlpButton.Controls.Add(this.bExecute, 1, 1);
            this.tlpButton.Controls.Add(this.bSave, 5, 1);
            this.tlpButton.Controls.Add(this.bExit, 7, 1);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 640);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(1100, 60);
            this.tlpButton.TabIndex = 11;
            // 
            // bStop
            // 
            this.bStop.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bStop.BackExtend = "135, #6253E1, #04BEFE";
            this.bStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bStop.Enabled = false;
            this.bStop.IconSvg = "PauseCircleOutlined";
            this.bStop.LoadingWaveColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(87)))), ((int)(((byte)(34)))));
            this.bStop.LoadingWaveCount = 6;
            this.bStop.LoadingWaveSize = 6;
            this.bStop.LoadingWaveValue = 0.6F;
            this.bStop.LoadingWaveVertical = true;
            this.bStop.LocalizationText = "Stop";
            this.bStop.Location = new System.Drawing.Point(455, 7);
            this.bStop.Name = "bStop";
            this.bStop.Size = new System.Drawing.Size(82, 46);
            this.bStop.TabIndex = 3;
            this.bStop.Text = "停止";
            this.bStop.Type = AntdUI.TTypeMini.Primary;
            this.bStop.Click += new System.EventHandler(this.bStop_Click);
            // 
            // bExecute
            // 
            this.bExecute.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bExecute.BackExtend = "135, #6253E1, #04BEFE";
            this.bExecute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bExecute.IconSvg = "PlayCircleOutlined";
            this.bExecute.LoadingWaveColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(87)))), ((int)(((byte)(34)))));
            this.bExecute.LoadingWaveCount = 6;
            this.bExecute.LoadingWaveSize = 6;
            this.bExecute.LoadingWaveValue = 0.6F;
            this.bExecute.LoadingWaveVertical = true;
            this.bExecute.LocalizationText = "Execute";
            this.bExecute.Location = new System.Drawing.Point(347, 7);
            this.bExecute.Name = "bExecute";
            this.bExecute.Size = new System.Drawing.Size(82, 46);
            this.bExecute.TabIndex = 2;
            this.bExecute.Text = "执行";
            this.bExecute.Type = AntdUI.TTypeMini.Info;
            this.bExecute.Click += new System.EventHandler(this.bExecute_Click);
            // 
            // bSave
            // 
            this.bSave.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.LoadingWaveColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(87)))), ((int)(((byte)(34)))));
            this.bSave.LoadingWaveCount = 6;
            this.bSave.LoadingWaveSize = 6;
            this.bSave.LoadingWaveValue = 0.6F;
            this.bSave.LoadingWaveVertical = true;
            this.bSave.LocalizationText = "Save";
            this.bSave.Location = new System.Drawing.Point(563, 7);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(82, 46);
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
            this.bExit.Location = new System.Drawing.Point(671, 7);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(82, 46);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // tlpRobotINST
            // 
            this.tlpRobotINST.ColumnCount = 2;
            this.tlpRobotINST.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpRobotINST.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpRobotINST.Controls.Add(this.cRobotINST, 0, 0);
            this.tlpRobotINST.Controls.Add(this.pRobotINST, 1, 0);
            this.tlpRobotINST.Controls.Add(this.txtINSTLog, 1, 1);
            this.tlpRobotINST.Controls.Add(this.txtRobotName, 0, 1);
            this.tlpRobotINST.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRobotINST.Location = new System.Drawing.Point(0, 0);
            this.tlpRobotINST.Margin = new System.Windows.Forms.Padding(0);
            this.tlpRobotINST.Name = "tlpRobotINST";
            this.tlpRobotINST.RowCount = 2;
            this.tlpRobotINST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRobotINST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.tlpRobotINST.Size = new System.Drawing.Size(1100, 640);
            this.tlpRobotINST.TabIndex = 0;
            // 
            // cRobotINST
            // 
            this.cRobotINST.ContentPadding = new System.Drawing.Size(8, 8);
            this.cRobotINST.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cRobotINST.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cRobotINST.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cRobotINST.FontExpand = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold);
            this.cRobotINST.Gap = 3;
            this.cRobotINST.Items.Add(this.ciPacketINST);
            this.cRobotINST.Items.Add(this.ciControlINST);
            this.cRobotINST.Items.Add(this.ciKeyBoardINST);
            this.cRobotINST.Items.Add(this.ciMouseINST);
            this.cRobotINST.Location = new System.Drawing.Point(3, 3);
            this.cRobotINST.Name = "cRobotINST";
            this.cRobotINST.Size = new System.Drawing.Size(544, 584);
            this.cRobotINST.TabIndex = 9;
            this.cRobotINST.Unique = true;
            this.cRobotINST.UniqueFull = true;
            // 
            // ciPacketINST
            // 
            this.ciPacketINST.Controls.Add(this.tlpPacketINST);
            this.ciPacketINST.Expand = true;
            this.ciPacketINST.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ciPacketINST.LocalizationText = "RobotEditForm.{id}";
            this.ciPacketINST.Location = new System.Drawing.Point(11, 57);
            this.ciPacketINST.Name = "ciPacketINST";
            this.ciPacketINST.Size = new System.Drawing.Size(522, 369);
            this.ciPacketINST.TabIndex = 0;
            this.ciPacketINST.Text = "封包指令";
            // 
            // tlpPacketINST
            // 
            this.tlpPacketINST.ColumnCount = 1;
            this.tlpPacketINST.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketINST.Controls.Add(this.pSYSSocket, 0, 2);
            this.tlpPacketINST.Controls.Add(this.pPacketList, 0, 1);
            this.tlpPacketINST.Controls.Add(this.pSendList, 0, 0);
            this.tlpPacketINST.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPacketINST.Location = new System.Drawing.Point(0, 0);
            this.tlpPacketINST.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPacketINST.Name = "tlpPacketINST";
            this.tlpPacketINST.RowCount = 3;
            this.tlpPacketINST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tlpPacketINST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tlpPacketINST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketINST.Size = new System.Drawing.Size(522, 369);
            this.tlpPacketINST.TabIndex = 0;
            // 
            // pSYSSocket
            // 
            this.pSYSSocket.BorderWidth = 2F;
            this.pSYSSocket.Controls.Add(this.tlpSYSSocket2);
            this.pSYSSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pSYSSocket.Location = new System.Drawing.Point(3, 183);
            this.pSYSSocket.Name = "pSYSSocket";
            this.pSYSSocket.Radius = 10;
            this.pSYSSocket.Size = new System.Drawing.Size(516, 183);
            this.pSYSSocket.TabIndex = 13;
            // 
            // tlpSYSSocket2
            // 
            this.tlpSYSSocket2.BackColor = System.Drawing.Color.Transparent;
            this.tlpSYSSocket2.ColumnCount = 1;
            this.tlpSYSSocket2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSYSSocket2.Controls.Add(this.tlpSYSSocket, 0, 1);
            this.tlpSYSSocket2.Controls.Add(this.dSYSSocket, 0, 0);
            this.tlpSYSSocket2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSYSSocket2.Location = new System.Drawing.Point(2, 2);
            this.tlpSYSSocket2.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSYSSocket2.Name = "tlpSYSSocket2";
            this.tlpSYSSocket2.RowCount = 3;
            this.tlpSYSSocket2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSYSSocket2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSYSSocket2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSYSSocket2.Size = new System.Drawing.Size(512, 179);
            this.tlpSYSSocket2.TabIndex = 0;
            // 
            // tlpSYSSocket
            // 
            this.tlpSYSSocket.ColumnCount = 3;
            this.tlpSYSSocket.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSYSSocket.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSYSSocket.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSYSSocket.Controls.Add(this.lSelectFilter, 1, 1);
            this.tlpSYSSocket.Controls.Add(this.rbSelectSocket, 0, 2);
            this.tlpSYSSocket.Controls.Add(this.rbSelectFilter, 0, 1);
            this.tlpSYSSocket.Controls.Add(this.bInsert_SYSSocket, 2, 0);
            this.tlpSYSSocket.Controls.Add(this.rbSelectPacket, 0, 0);
            this.tlpSYSSocket.Controls.Add(this.lSelectPacket, 1, 0);
            this.tlpSYSSocket.Controls.Add(this.nudSelectSocket, 1, 2);
            this.tlpSYSSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSYSSocket.Location = new System.Drawing.Point(0, 31);
            this.tlpSYSSocket.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSYSSocket.Name = "tlpSYSSocket";
            this.tlpSYSSocket.RowCount = 4;
            this.tlpSYSSocket.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSYSSocket.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tlpSYSSocket.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tlpSYSSocket.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSYSSocket.Size = new System.Drawing.Size(512, 135);
            this.tlpSYSSocket.TabIndex = 3;
            // 
            // lSelectFilter
            // 
            this.lSelectFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSelectFilter.LocalizationText = "RobotEditForm.PacketINST.SetSSocket.Filter";
            this.lSelectFilter.Location = new System.Drawing.Point(55, 51);
            this.lSelectFilter.Name = "lSelectFilter";
            this.lSelectFilter.Size = new System.Drawing.Size(408, 39);
            this.lSelectFilter.TabIndex = 7;
            this.lSelectFilter.Text = "调用滤镜的套接字";
            // 
            // rbSelectSocket
            // 
            this.rbSelectSocket.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbSelectSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbSelectSocket.Location = new System.Drawing.Point(3, 96);
            this.rbSelectSocket.Name = "rbSelectSocket";
            this.rbSelectSocket.Size = new System.Drawing.Size(46, 39);
            this.rbSelectSocket.TabIndex = 5;
            this.rbSelectSocket.Text = "=";
            // 
            // rbSelectFilter
            // 
            this.rbSelectFilter.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbSelectFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbSelectFilter.Location = new System.Drawing.Point(3, 51);
            this.rbSelectFilter.Name = "rbSelectFilter";
            this.rbSelectFilter.Size = new System.Drawing.Size(46, 39);
            this.rbSelectFilter.TabIndex = 3;
            this.rbSelectFilter.Text = "=";
            // 
            // bInsert_SYSSocket
            // 
            this.bInsert_SYSSocket.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bInsert_SYSSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bInsert_SYSSocket.IconSvg = "ArrowRightOutlined";
            this.bInsert_SYSSocket.Location = new System.Drawing.Point(469, 3);
            this.bInsert_SYSSocket.Name = "bInsert_SYSSocket";
            this.bInsert_SYSSocket.Size = new System.Drawing.Size(40, 40);
            this.bInsert_SYSSocket.TabIndex = 1;
            this.bInsert_SYSSocket.Type = AntdUI.TTypeMini.Primary;
            this.bInsert_SYSSocket.Click += new System.EventHandler(this.bInsert_SYSSocket_Click);
            // 
            // rbSelectPacket
            // 
            this.rbSelectPacket.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbSelectPacket.Checked = true;
            this.rbSelectPacket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbSelectPacket.Location = new System.Drawing.Point(3, 3);
            this.rbSelectPacket.Name = "rbSelectPacket";
            this.rbSelectPacket.Size = new System.Drawing.Size(46, 42);
            this.rbSelectPacket.TabIndex = 2;
            this.rbSelectPacket.Text = "=";
            // 
            // lSelectPacket
            // 
            this.lSelectPacket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSelectPacket.LocalizationText = "RobotEditForm.PacketINST.SetSSocket.Select";
            this.lSelectPacket.Location = new System.Drawing.Point(55, 3);
            this.lSelectPacket.Name = "lSelectPacket";
            this.lSelectPacket.Size = new System.Drawing.Size(408, 42);
            this.lSelectPacket.TabIndex = 6;
            this.lSelectPacket.Text = "选中封包的套接字";
            // 
            // nudSelectSocket
            // 
            this.nudSelectSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudSelectSocket.Location = new System.Drawing.Point(55, 96);
            this.nudSelectSocket.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudSelectSocket.Name = "nudSelectSocket";
            this.nudSelectSocket.Size = new System.Drawing.Size(408, 39);
            this.nudSelectSocket.TabIndex = 8;
            this.nudSelectSocket.Text = "0";
            // 
            // dSYSSocket
            // 
            this.dSYSSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dSYSSocket.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dSYSSocket.LocalizationText = "RobotEditForm.PacketINST.SetSSocket";
            this.dSYSSocket.Location = new System.Drawing.Point(3, 3);
            this.dSYSSocket.Name = "dSYSSocket";
            this.dSYSSocket.Orientation = AntdUI.TOrientation.Left;
            this.dSYSSocket.Size = new System.Drawing.Size(506, 25);
            this.dSYSSocket.TabIndex = 0;
            this.dSYSSocket.Text = "设置 - 系统套接字";
            // 
            // pPacketList
            // 
            this.pPacketList.BorderWidth = 2F;
            this.pPacketList.Controls.Add(this.tlpPacketList2);
            this.pPacketList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pPacketList.Location = new System.Drawing.Point(3, 93);
            this.pPacketList.Name = "pPacketList";
            this.pPacketList.Radius = 10;
            this.pPacketList.Size = new System.Drawing.Size(516, 84);
            this.pPacketList.TabIndex = 12;
            // 
            // tlpPacketList2
            // 
            this.tlpPacketList2.BackColor = System.Drawing.Color.Transparent;
            this.tlpPacketList2.ColumnCount = 1;
            this.tlpPacketList2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketList2.Controls.Add(this.tlpPacketList, 0, 1);
            this.tlpPacketList2.Controls.Add(this.dPacketList, 0, 0);
            this.tlpPacketList2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPacketList2.Location = new System.Drawing.Point(2, 2);
            this.tlpPacketList2.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPacketList2.Name = "tlpPacketList2";
            this.tlpPacketList2.RowCount = 3;
            this.tlpPacketList2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpPacketList2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpPacketList2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketList2.Size = new System.Drawing.Size(512, 80);
            this.tlpPacketList2.TabIndex = 0;
            // 
            // tlpPacketList
            // 
            this.tlpPacketList.ColumnCount = 2;
            this.tlpPacketList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpPacketList.Controls.Add(this.bInsert_PacketList, 1, 0);
            this.tlpPacketList.Controls.Add(this.lPacketList, 0, 0);
            this.tlpPacketList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPacketList.Location = new System.Drawing.Point(0, 31);
            this.tlpPacketList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPacketList.Name = "tlpPacketList";
            this.tlpPacketList.RowCount = 1;
            this.tlpPacketList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketList.Size = new System.Drawing.Size(512, 45);
            this.tlpPacketList.TabIndex = 2;
            // 
            // bInsert_PacketList
            // 
            this.bInsert_PacketList.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bInsert_PacketList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bInsert_PacketList.IconSvg = "ArrowRightOutlined";
            this.bInsert_PacketList.Location = new System.Drawing.Point(469, 3);
            this.bInsert_PacketList.Name = "bInsert_PacketList";
            this.bInsert_PacketList.Size = new System.Drawing.Size(40, 40);
            this.bInsert_PacketList.TabIndex = 1;
            this.bInsert_PacketList.Type = AntdUI.TTypeMini.Primary;
            this.bInsert_PacketList.Click += new System.EventHandler(this.bInsert_PacketList_Click);
            // 
            // lPacketList
            // 
            this.lPacketList.AutoEllipsis = true;
            this.lPacketList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lPacketList.LocalizationText = "RobotEditForm.PacketINST.PacketList.Select";
            this.lPacketList.Location = new System.Drawing.Point(3, 3);
            this.lPacketList.Name = "lPacketList";
            this.lPacketList.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.lPacketList.Size = new System.Drawing.Size(460, 39);
            this.lPacketList.TabIndex = 2;
            this.lPacketList.Text = "封包列表中选中的封包";
            // 
            // dPacketList
            // 
            this.dPacketList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dPacketList.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dPacketList.LocalizationText = "RobotEditForm.PacketINST.PacketList";
            this.dPacketList.Location = new System.Drawing.Point(3, 3);
            this.dPacketList.Name = "dPacketList";
            this.dPacketList.Orientation = AntdUI.TOrientation.Left;
            this.dPacketList.Size = new System.Drawing.Size(506, 25);
            this.dPacketList.TabIndex = 0;
            this.dPacketList.Text = "发送 - 封包列表";
            // 
            // pSendList
            // 
            this.pSendList.BorderWidth = 2F;
            this.pSendList.Controls.Add(this.tlpSendList2);
            this.pSendList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pSendList.Location = new System.Drawing.Point(3, 3);
            this.pSendList.Name = "pSendList";
            this.pSendList.Radius = 10;
            this.pSendList.Size = new System.Drawing.Size(516, 84);
            this.pSendList.TabIndex = 11;
            // 
            // tlpSendList2
            // 
            this.tlpSendList2.BackColor = System.Drawing.Color.Transparent;
            this.tlpSendList2.ColumnCount = 1;
            this.tlpSendList2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSendList2.Controls.Add(this.dSendList, 0, 0);
            this.tlpSendList2.Controls.Add(this.tlpSendList, 0, 1);
            this.tlpSendList2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSendList2.Location = new System.Drawing.Point(2, 2);
            this.tlpSendList2.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSendList2.Name = "tlpSendList2";
            this.tlpSendList2.RowCount = 3;
            this.tlpSendList2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSendList2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSendList2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSendList2.Size = new System.Drawing.Size(512, 80);
            this.tlpSendList2.TabIndex = 0;
            // 
            // dSendList
            // 
            this.dSendList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dSendList.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dSendList.LocalizationText = "RobotEditForm.PacketINST.SendList";
            this.dSendList.Location = new System.Drawing.Point(3, 3);
            this.dSendList.Name = "dSendList";
            this.dSendList.Orientation = AntdUI.TOrientation.Left;
            this.dSendList.Size = new System.Drawing.Size(506, 25);
            this.dSendList.TabIndex = 0;
            this.dSendList.Text = "发送 - 发送列表";
            // 
            // tlpSendList
            // 
            this.tlpSendList.ColumnCount = 2;
            this.tlpSendList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSendList.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSendList.Controls.Add(this.ddlSendList, 0, 0);
            this.tlpSendList.Controls.Add(this.bInsert_SendList, 1, 0);
            this.tlpSendList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSendList.Location = new System.Drawing.Point(0, 31);
            this.tlpSendList.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSendList.Name = "tlpSendList";
            this.tlpSendList.RowCount = 1;
            this.tlpSendList.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSendList.Size = new System.Drawing.Size(512, 45);
            this.tlpSendList.TabIndex = 1;
            // 
            // ddlSendList
            // 
            this.ddlSendList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddlSendList.List = true;
            this.ddlSendList.LocalizationPlaceholderText = "PleaseSelect";
            this.ddlSendList.Location = new System.Drawing.Point(3, 3);
            this.ddlSendList.Name = "ddlSendList";
            this.ddlSendList.PlaceholderText = "请选择";
            this.ddlSendList.Size = new System.Drawing.Size(460, 39);
            this.ddlSendList.TabIndex = 0;
            // 
            // bInsert_SendList
            // 
            this.bInsert_SendList.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bInsert_SendList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bInsert_SendList.IconSvg = "ArrowRightOutlined";
            this.bInsert_SendList.Location = new System.Drawing.Point(469, 3);
            this.bInsert_SendList.Name = "bInsert_SendList";
            this.bInsert_SendList.Size = new System.Drawing.Size(40, 40);
            this.bInsert_SendList.TabIndex = 1;
            this.bInsert_SendList.Type = AntdUI.TTypeMini.Primary;
            this.bInsert_SendList.Click += new System.EventHandler(this.bInsert_SendList_Click);
            // 
            // ciControlINST
            // 
            this.ciControlINST.Controls.Add(this.tlpControlINST);
            this.ciControlINST.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ciControlINST.LocalizationText = "RobotEditForm.{id}";
            this.ciControlINST.Location = new System.Drawing.Point(-472, -369);
            this.ciControlINST.Name = "ciControlINST";
            this.ciControlINST.Size = new System.Drawing.Size(472, 369);
            this.ciControlINST.TabIndex = 1;
            this.ciControlINST.Text = "控制指令";
            // 
            // tlpControlINST
            // 
            this.tlpControlINST.ColumnCount = 1;
            this.tlpControlINST.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpControlINST.Controls.Add(this.pLoop, 0, 1);
            this.tlpControlINST.Controls.Add(this.pDelay, 0, 0);
            this.tlpControlINST.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpControlINST.Location = new System.Drawing.Point(0, 0);
            this.tlpControlINST.Margin = new System.Windows.Forms.Padding(0);
            this.tlpControlINST.Name = "tlpControlINST";
            this.tlpControlINST.RowCount = 3;
            this.tlpControlINST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tlpControlINST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpControlINST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpControlINST.Size = new System.Drawing.Size(472, 369);
            this.tlpControlINST.TabIndex = 0;
            // 
            // pLoop
            // 
            this.pLoop.BorderWidth = 2F;
            this.pLoop.Controls.Add(this.tlpLoop);
            this.pLoop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pLoop.Location = new System.Drawing.Point(3, 143);
            this.pLoop.Name = "pLoop";
            this.pLoop.Radius = 10;
            this.pLoop.Size = new System.Drawing.Size(466, 94);
            this.pLoop.TabIndex = 15;
            // 
            // tlpLoop
            // 
            this.tlpLoop.BackColor = System.Drawing.Color.Transparent;
            this.tlpLoop.ColumnCount = 1;
            this.tlpLoop.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLoop.Controls.Add(this.dLoop, 0, 0);
            this.tlpLoop.Controls.Add(this.tlpLoop2, 0, 1);
            this.tlpLoop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpLoop.Location = new System.Drawing.Point(2, 2);
            this.tlpLoop.Margin = new System.Windows.Forms.Padding(0);
            this.tlpLoop.Name = "tlpLoop";
            this.tlpLoop.RowCount = 3;
            this.tlpLoop.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpLoop.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpLoop.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLoop.Size = new System.Drawing.Size(462, 90);
            this.tlpLoop.TabIndex = 0;
            // 
            // dLoop
            // 
            this.dLoop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dLoop.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dLoop.LocalizationText = "RobotEditForm.ControlINST.Loop";
            this.dLoop.Location = new System.Drawing.Point(3, 3);
            this.dLoop.Name = "dLoop";
            this.dLoop.Orientation = AntdUI.TOrientation.Left;
            this.dLoop.Size = new System.Drawing.Size(456, 25);
            this.dLoop.TabIndex = 0;
            this.dLoop.Text = "循环";
            // 
            // tlpLoop2
            // 
            this.tlpLoop2.ColumnCount = 3;
            this.tlpLoop2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLoop2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpLoop2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpLoop2.Controls.Add(this.nudLoop, 0, 0);
            this.tlpLoop2.Controls.Add(this.bInsert_LoopStart, 1, 0);
            this.tlpLoop2.Controls.Add(this.bInsert_LoopEnd, 2, 0);
            this.tlpLoop2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpLoop2.Location = new System.Drawing.Point(0, 31);
            this.tlpLoop2.Margin = new System.Windows.Forms.Padding(0);
            this.tlpLoop2.Name = "tlpLoop2";
            this.tlpLoop2.RowCount = 2;
            this.tlpLoop2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpLoop2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLoop2.Size = new System.Drawing.Size(462, 50);
            this.tlpLoop2.TabIndex = 1;
            // 
            // nudLoop
            // 
            this.nudLoop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudLoop.LocalizationSuffixText = "Times";
            this.nudLoop.Location = new System.Drawing.Point(3, 3);
            this.nudLoop.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudLoop.Name = "nudLoop";
            this.nudLoop.Size = new System.Drawing.Size(306, 42);
            this.nudLoop.SuffixText = "次";
            this.nudLoop.TabIndex = 10;
            this.nudLoop.Text = "1";
            this.nudLoop.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudLoop.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // bInsert_LoopStart
            // 
            this.bInsert_LoopStart.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bInsert_LoopStart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bInsert_LoopStart.IconPosition = AntdUI.TAlignMini.Right;
            this.bInsert_LoopStart.IconSvg = "ArrowRightOutlined";
            this.bInsert_LoopStart.LocalizationText = "Begin";
            this.bInsert_LoopStart.Location = new System.Drawing.Point(315, 3);
            this.bInsert_LoopStart.Name = "bInsert_LoopStart";
            this.bInsert_LoopStart.Size = new System.Drawing.Size(69, 40);
            this.bInsert_LoopStart.TabIndex = 2;
            this.bInsert_LoopStart.Text = "开始";
            this.bInsert_LoopStart.Type = AntdUI.TTypeMini.Primary;
            this.bInsert_LoopStart.Click += new System.EventHandler(this.bInsert_LoopStart_Click);
            // 
            // bInsert_LoopEnd
            // 
            this.bInsert_LoopEnd.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bInsert_LoopEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bInsert_LoopEnd.IconPosition = AntdUI.TAlignMini.Right;
            this.bInsert_LoopEnd.IconSvg = "ArrowRightOutlined";
            this.bInsert_LoopEnd.LocalizationText = "End";
            this.bInsert_LoopEnd.Location = new System.Drawing.Point(390, 3);
            this.bInsert_LoopEnd.Name = "bInsert_LoopEnd";
            this.bInsert_LoopEnd.Size = new System.Drawing.Size(69, 40);
            this.bInsert_LoopEnd.TabIndex = 1;
            this.bInsert_LoopEnd.Text = "结束";
            this.bInsert_LoopEnd.Type = AntdUI.TTypeMini.Primary;
            this.bInsert_LoopEnd.Click += new System.EventHandler(this.bInsert_LoopEnd_Click);
            // 
            // pDelay
            // 
            this.pDelay.BorderWidth = 2F;
            this.pDelay.Controls.Add(this.tlpDelay);
            this.pDelay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pDelay.Location = new System.Drawing.Point(3, 3);
            this.pDelay.Name = "pDelay";
            this.pDelay.Radius = 10;
            this.pDelay.Size = new System.Drawing.Size(466, 134);
            this.pDelay.TabIndex = 14;
            // 
            // tlpDelay
            // 
            this.tlpDelay.BackColor = System.Drawing.Color.Transparent;
            this.tlpDelay.ColumnCount = 1;
            this.tlpDelay.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDelay.Controls.Add(this.tlpDelay2, 0, 1);
            this.tlpDelay.Controls.Add(this.dDelay, 0, 0);
            this.tlpDelay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpDelay.Location = new System.Drawing.Point(2, 2);
            this.tlpDelay.Margin = new System.Windows.Forms.Padding(0);
            this.tlpDelay.Name = "tlpDelay";
            this.tlpDelay.RowCount = 3;
            this.tlpDelay.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpDelay.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpDelay.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDelay.Size = new System.Drawing.Size(462, 130);
            this.tlpDelay.TabIndex = 0;
            // 
            // tlpDelay2
            // 
            this.tlpDelay2.ColumnCount = 4;
            this.tlpDelay2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpDelay2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpDelay2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpDelay2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpDelay2.Controls.Add(this.nudnudDelayRandom_To, 2, 1);
            this.tlpDelay2.Controls.Add(this.nudDelayFix, 1, 0);
            this.tlpDelay2.Controls.Add(this.rbDelayRandom, 0, 1);
            this.tlpDelay2.Controls.Add(this.bInsert_Delay, 3, 0);
            this.tlpDelay2.Controls.Add(this.rbDelayFix, 0, 0);
            this.tlpDelay2.Controls.Add(this.nudnudDelayRandom_From, 1, 1);
            this.tlpDelay2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpDelay2.Location = new System.Drawing.Point(0, 31);
            this.tlpDelay2.Margin = new System.Windows.Forms.Padding(0);
            this.tlpDelay2.Name = "tlpDelay2";
            this.tlpDelay2.RowCount = 3;
            this.tlpDelay2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpDelay2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tlpDelay2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpDelay2.Size = new System.Drawing.Size(462, 100);
            this.tlpDelay2.TabIndex = 3;
            // 
            // nudnudDelayRandom_To
            // 
            this.nudnudDelayRandom_To.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudnudDelayRandom_To.LocalizationSuffixText = "Millisecond";
            this.nudnudDelayRandom_To.Location = new System.Drawing.Point(245, 51);
            this.nudnudDelayRandom_To.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudnudDelayRandom_To.Name = "nudnudDelayRandom_To";
            this.nudnudDelayRandom_To.Size = new System.Drawing.Size(167, 39);
            this.nudnudDelayRandom_To.SuffixText = "毫秒";
            this.nudnudDelayRandom_To.TabIndex = 11;
            this.nudnudDelayRandom_To.Text = "100";
            this.nudnudDelayRandom_To.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudnudDelayRandom_To.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // nudDelayFix
            // 
            this.nudDelayFix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudDelayFix.LocalizationSuffixText = "Millisecond";
            this.nudDelayFix.Location = new System.Drawing.Point(72, 3);
            this.nudDelayFix.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudDelayFix.Name = "nudDelayFix";
            this.nudDelayFix.Size = new System.Drawing.Size(167, 42);
            this.nudDelayFix.SuffixText = "毫秒";
            this.nudDelayFix.TabIndex = 9;
            this.nudDelayFix.Text = "100";
            this.nudDelayFix.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudDelayFix.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // rbDelayRandom
            // 
            this.rbDelayRandom.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbDelayRandom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbDelayRandom.LocalizationText = "RobotEditForm.ControlINST.Delay.Random";
            this.rbDelayRandom.Location = new System.Drawing.Point(3, 51);
            this.rbDelayRandom.Name = "rbDelayRandom";
            this.rbDelayRandom.Size = new System.Drawing.Size(63, 39);
            this.rbDelayRandom.TabIndex = 5;
            this.rbDelayRandom.Text = "随机";
            this.rbDelayRandom.CheckedChanged += new AntdUI.BoolEventHandler(this.rbDelayRandom_CheckedChanged);
            // 
            // bInsert_Delay
            // 
            this.bInsert_Delay.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bInsert_Delay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bInsert_Delay.IconSvg = "ArrowRightOutlined";
            this.bInsert_Delay.Location = new System.Drawing.Point(418, 3);
            this.bInsert_Delay.Name = "bInsert_Delay";
            this.bInsert_Delay.Size = new System.Drawing.Size(40, 40);
            this.bInsert_Delay.TabIndex = 1;
            this.bInsert_Delay.Type = AntdUI.TTypeMini.Primary;
            this.bInsert_Delay.Click += new System.EventHandler(this.bInsert_Delay_Click);
            // 
            // rbDelayFix
            // 
            this.rbDelayFix.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.rbDelayFix.Checked = true;
            this.rbDelayFix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbDelayFix.LocalizationText = "RobotEditForm.ControlINST.Delay.Fixed";
            this.rbDelayFix.Location = new System.Drawing.Point(3, 3);
            this.rbDelayFix.Name = "rbDelayFix";
            this.rbDelayFix.Size = new System.Drawing.Size(63, 42);
            this.rbDelayFix.TabIndex = 2;
            this.rbDelayFix.Text = "定时";
            this.rbDelayFix.CheckedChanged += new AntdUI.BoolEventHandler(this.rbDelayFix_CheckedChanged);
            // 
            // nudnudDelayRandom_From
            // 
            this.nudnudDelayRandom_From.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudnudDelayRandom_From.LocalizationSuffixText = "";
            this.nudnudDelayRandom_From.Location = new System.Drawing.Point(72, 51);
            this.nudnudDelayRandom_From.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudnudDelayRandom_From.Name = "nudnudDelayRandom_From";
            this.nudnudDelayRandom_From.Size = new System.Drawing.Size(167, 39);
            this.nudnudDelayRandom_From.SuffixText = "-";
            this.nudnudDelayRandom_From.TabIndex = 8;
            this.nudnudDelayRandom_From.Text = "0";
            this.nudnudDelayRandom_From.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dDelay
            // 
            this.dDelay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dDelay.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dDelay.LocalizationText = "RobotEditForm.ControlINST.Delay";
            this.dDelay.Location = new System.Drawing.Point(3, 3);
            this.dDelay.Name = "dDelay";
            this.dDelay.Orientation = AntdUI.TOrientation.Left;
            this.dDelay.Size = new System.Drawing.Size(456, 25);
            this.dDelay.TabIndex = 0;
            this.dDelay.Text = "延迟";
            // 
            // ciKeyBoardINST
            // 
            this.ciKeyBoardINST.Controls.Add(this.tlpKeyboardINST);
            this.ciKeyBoardINST.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ciKeyBoardINST.LocalizationText = "RobotEditForm.{id}";
            this.ciKeyBoardINST.Location = new System.Drawing.Point(-472, -369);
            this.ciKeyBoardINST.Name = "ciKeyBoardINST";
            this.ciKeyBoardINST.Size = new System.Drawing.Size(472, 369);
            this.ciKeyBoardINST.TabIndex = 2;
            this.ciKeyBoardINST.Text = "键盘指令";
            // 
            // tlpKeyboardINST
            // 
            this.tlpKeyboardINST.ColumnCount = 1;
            this.tlpKeyboardINST.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpKeyboardINST.Controls.Add(this.pText, 0, 2);
            this.tlpKeyboardINST.Controls.Add(this.pKeyCombination, 0, 1);
            this.tlpKeyboardINST.Controls.Add(this.pKeyBoard, 0, 0);
            this.tlpKeyboardINST.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpKeyboardINST.Location = new System.Drawing.Point(0, 0);
            this.tlpKeyboardINST.Margin = new System.Windows.Forms.Padding(0);
            this.tlpKeyboardINST.Name = "tlpKeyboardINST";
            this.tlpKeyboardINST.RowCount = 4;
            this.tlpKeyboardINST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tlpKeyboardINST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpKeyboardINST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpKeyboardINST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpKeyboardINST.Size = new System.Drawing.Size(472, 369);
            this.tlpKeyboardINST.TabIndex = 1;
            // 
            // pText
            // 
            this.pText.BorderWidth = 2F;
            this.pText.Controls.Add(this.tlpText);
            this.pText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pText.Location = new System.Drawing.Point(3, 243);
            this.pText.Name = "pText";
            this.pText.Radius = 10;
            this.pText.Size = new System.Drawing.Size(466, 94);
            this.pText.TabIndex = 16;
            // 
            // tlpText
            // 
            this.tlpText.BackColor = System.Drawing.Color.Transparent;
            this.tlpText.ColumnCount = 1;
            this.tlpText.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpText.Controls.Add(this.dText, 0, 0);
            this.tlpText.Controls.Add(this.tlpText2, 0, 1);
            this.tlpText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpText.Location = new System.Drawing.Point(2, 2);
            this.tlpText.Margin = new System.Windows.Forms.Padding(0);
            this.tlpText.Name = "tlpText";
            this.tlpText.RowCount = 3;
            this.tlpText.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpText.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpText.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpText.Size = new System.Drawing.Size(462, 90);
            this.tlpText.TabIndex = 0;
            // 
            // dText
            // 
            this.dText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dText.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dText.LocalizationText = "RobotEditForm.KeyBoardINST.Text";
            this.dText.Location = new System.Drawing.Point(3, 3);
            this.dText.Name = "dText";
            this.dText.Orientation = AntdUI.TOrientation.Left;
            this.dText.Size = new System.Drawing.Size(456, 25);
            this.dText.TabIndex = 0;
            this.dText.Text = "文本";
            // 
            // tlpText2
            // 
            this.tlpText2.ColumnCount = 2;
            this.tlpText2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpText2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpText2.Controls.Add(this.bInsert_Text, 1, 0);
            this.tlpText2.Controls.Add(this.txtText, 0, 0);
            this.tlpText2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpText2.Location = new System.Drawing.Point(0, 31);
            this.tlpText2.Margin = new System.Windows.Forms.Padding(0);
            this.tlpText2.Name = "tlpText2";
            this.tlpText2.RowCount = 2;
            this.tlpText2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpText2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpText2.Size = new System.Drawing.Size(462, 50);
            this.tlpText2.TabIndex = 1;
            // 
            // bInsert_Text
            // 
            this.bInsert_Text.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bInsert_Text.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bInsert_Text.IconSvg = "ArrowRightOutlined";
            this.bInsert_Text.Location = new System.Drawing.Point(419, 3);
            this.bInsert_Text.Name = "bInsert_Text";
            this.bInsert_Text.Size = new System.Drawing.Size(40, 40);
            this.bInsert_Text.TabIndex = 1;
            this.bInsert_Text.Type = AntdUI.TTypeMini.Primary;
            this.bInsert_Text.Click += new System.EventHandler(this.bInsert_Text_Click);
            // 
            // txtText
            // 
            this.txtText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtText.LocalizationPlaceholderText = "Input.Text";
            this.txtText.Location = new System.Drawing.Point(3, 3);
            this.txtText.Name = "txtText";
            this.txtText.PlaceholderText = "请输入文本";
            this.txtText.Size = new System.Drawing.Size(410, 42);
            this.txtText.TabIndex = 2;
            // 
            // pKeyCombination
            // 
            this.pKeyCombination.BorderWidth = 2F;
            this.pKeyCombination.Controls.Add(this.tlpKeyCombination);
            this.pKeyCombination.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pKeyCombination.Location = new System.Drawing.Point(3, 143);
            this.pKeyCombination.Name = "pKeyCombination";
            this.pKeyCombination.Radius = 10;
            this.pKeyCombination.Size = new System.Drawing.Size(466, 94);
            this.pKeyCombination.TabIndex = 15;
            // 
            // tlpKeyCombination
            // 
            this.tlpKeyCombination.BackColor = System.Drawing.Color.Transparent;
            this.tlpKeyCombination.ColumnCount = 1;
            this.tlpKeyCombination.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpKeyCombination.Controls.Add(this.dKeyCombination, 0, 0);
            this.tlpKeyCombination.Controls.Add(this.tlpKeyCombination2, 0, 1);
            this.tlpKeyCombination.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpKeyCombination.Location = new System.Drawing.Point(2, 2);
            this.tlpKeyCombination.Margin = new System.Windows.Forms.Padding(0);
            this.tlpKeyCombination.Name = "tlpKeyCombination";
            this.tlpKeyCombination.RowCount = 3;
            this.tlpKeyCombination.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpKeyCombination.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpKeyCombination.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpKeyCombination.Size = new System.Drawing.Size(462, 90);
            this.tlpKeyCombination.TabIndex = 0;
            // 
            // dKeyCombination
            // 
            this.dKeyCombination.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dKeyCombination.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dKeyCombination.LocalizationText = "RobotEditForm.KeyBoardINST.CombinationKey";
            this.dKeyCombination.Location = new System.Drawing.Point(3, 3);
            this.dKeyCombination.Name = "dKeyCombination";
            this.dKeyCombination.Orientation = AntdUI.TOrientation.Left;
            this.dKeyCombination.Size = new System.Drawing.Size(456, 25);
            this.dKeyCombination.TabIndex = 0;
            this.dKeyCombination.Text = "组合按键";
            // 
            // tlpKeyCombination2
            // 
            this.tlpKeyCombination2.ColumnCount = 2;
            this.tlpKeyCombination2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpKeyCombination2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpKeyCombination2.Controls.Add(this.bInsert_KeyCombination, 1, 0);
            this.tlpKeyCombination2.Controls.Add(this.txtKeyCombination, 0, 0);
            this.tlpKeyCombination2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpKeyCombination2.Location = new System.Drawing.Point(0, 31);
            this.tlpKeyCombination2.Margin = new System.Windows.Forms.Padding(0);
            this.tlpKeyCombination2.Name = "tlpKeyCombination2";
            this.tlpKeyCombination2.RowCount = 2;
            this.tlpKeyCombination2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpKeyCombination2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpKeyCombination2.Size = new System.Drawing.Size(462, 50);
            this.tlpKeyCombination2.TabIndex = 1;
            // 
            // bInsert_KeyCombination
            // 
            this.bInsert_KeyCombination.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bInsert_KeyCombination.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bInsert_KeyCombination.IconSvg = "ArrowRightOutlined";
            this.bInsert_KeyCombination.Location = new System.Drawing.Point(419, 3);
            this.bInsert_KeyCombination.Name = "bInsert_KeyCombination";
            this.bInsert_KeyCombination.Size = new System.Drawing.Size(40, 40);
            this.bInsert_KeyCombination.TabIndex = 1;
            this.bInsert_KeyCombination.Type = AntdUI.TTypeMini.Primary;
            this.bInsert_KeyCombination.Click += new System.EventHandler(this.bInsert_KeyCombination_Click);
            // 
            // txtKeyCombination
            // 
            this.txtKeyCombination.BackColor = System.Drawing.SystemColors.Window;
            this.txtKeyCombination.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtKeyCombination.ForeColor = System.Drawing.Color.Black;
            this.txtKeyCombination.LocalizationPlaceholderText = "PressCombinationkey";
            this.txtKeyCombination.Location = new System.Drawing.Point(3, 3);
            this.txtKeyCombination.Name = "txtKeyCombination";
            this.txtKeyCombination.PlaceholderText = "请组合按键";
            this.txtKeyCombination.ReadOnly = true;
            this.txtKeyCombination.Size = new System.Drawing.Size(410, 42);
            this.txtKeyCombination.TabIndex = 2;
            this.txtKeyCombination.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pKeyBoard
            // 
            this.pKeyBoard.BorderWidth = 2F;
            this.pKeyBoard.Controls.Add(this.tlpKey);
            this.pKeyBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pKeyBoard.Location = new System.Drawing.Point(3, 3);
            this.pKeyBoard.Name = "pKeyBoard";
            this.pKeyBoard.Radius = 10;
            this.pKeyBoard.Size = new System.Drawing.Size(466, 134);
            this.pKeyBoard.TabIndex = 14;
            // 
            // tlpKey
            // 
            this.tlpKey.BackColor = System.Drawing.Color.Transparent;
            this.tlpKey.ColumnCount = 1;
            this.tlpKey.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpKey.Controls.Add(this.tlpKey2, 0, 1);
            this.tlpKey.Controls.Add(this.dkey, 0, 0);
            this.tlpKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpKey.Location = new System.Drawing.Point(2, 2);
            this.tlpKey.Margin = new System.Windows.Forms.Padding(0);
            this.tlpKey.Name = "tlpKey";
            this.tlpKey.RowCount = 3;
            this.tlpKey.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpKey.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpKey.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpKey.Size = new System.Drawing.Size(462, 130);
            this.tlpKey.TabIndex = 0;
            // 
            // tlpKey2
            // 
            this.tlpKey2.ColumnCount = 4;
            this.tlpKey2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tlpKey2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpKey2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpKey2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpKey2.Controls.Add(this.bInsert_KeyBoard, 3, 0);
            this.tlpKey2.Controls.Add(this.lkey, 1, 0);
            this.tlpKey2.Controls.Add(this.lKeyType, 1, 1);
            this.tlpKey2.Controls.Add(this.txtKey, 2, 0);
            this.tlpKey2.Controls.Add(this.ddlKeyType, 2, 1);
            this.tlpKey2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpKey2.Location = new System.Drawing.Point(0, 31);
            this.tlpKey2.Margin = new System.Windows.Forms.Padding(0);
            this.tlpKey2.Name = "tlpKey2";
            this.tlpKey2.RowCount = 3;
            this.tlpKey2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpKey2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tlpKey2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpKey2.Size = new System.Drawing.Size(462, 100);
            this.tlpKey2.TabIndex = 3;
            // 
            // bInsert_KeyBoard
            // 
            this.bInsert_KeyBoard.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bInsert_KeyBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bInsert_KeyBoard.IconSvg = "ArrowRightOutlined";
            this.bInsert_KeyBoard.Location = new System.Drawing.Point(419, 3);
            this.bInsert_KeyBoard.Name = "bInsert_KeyBoard";
            this.bInsert_KeyBoard.Size = new System.Drawing.Size(40, 40);
            this.bInsert_KeyBoard.TabIndex = 1;
            this.bInsert_KeyBoard.Type = AntdUI.TTypeMini.Primary;
            this.bInsert_KeyBoard.Click += new System.EventHandler(this.bInsert_KeyBoard_Click);
            // 
            // lkey
            // 
            this.lkey.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lkey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lkey.LocalizationText = "RobotEditForm.KeyBoardINST.Key";
            this.lkey.Location = new System.Drawing.Point(13, 3);
            this.lkey.Name = "lkey";
            this.lkey.Size = new System.Drawing.Size(34, 42);
            this.lkey.TabIndex = 12;
            this.lkey.Text = "按键 :";
            this.lkey.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lKeyType
            // 
            this.lKeyType.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lKeyType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lKeyType.LocalizationText = "RobotEditForm.KeyBoardINST.Key.Type";
            this.lKeyType.Location = new System.Drawing.Point(13, 51);
            this.lKeyType.Name = "lKeyType";
            this.lKeyType.Size = new System.Drawing.Size(34, 39);
            this.lKeyType.TabIndex = 13;
            this.lKeyType.Text = "类型 :";
            this.lKeyType.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtKey
            // 
            this.txtKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtKey.LocalizationPlaceholderText = "Presskey";
            this.txtKey.Location = new System.Drawing.Point(53, 3);
            this.txtKey.Name = "txtKey";
            this.txtKey.PlaceholderText = "请按键";
            this.txtKey.Size = new System.Drawing.Size(360, 42);
            this.txtKey.TabIndex = 14;
            this.txtKey.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtKey.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtKey_KeyDown);
            // 
            // ddlKeyType
            // 
            this.ddlKeyType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddlKeyType.List = true;
            this.ddlKeyType.LocalizationPlaceholderText = "PleaseSelect";
            this.ddlKeyType.Location = new System.Drawing.Point(53, 51);
            this.ddlKeyType.Name = "ddlKeyType";
            this.ddlKeyType.PlaceholderText = "请选择";
            this.ddlKeyType.Size = new System.Drawing.Size(360, 39);
            this.ddlKeyType.TabIndex = 15;
            this.ddlKeyType.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dkey
            // 
            this.dkey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dkey.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dkey.LocalizationText = "RobotEditForm.KeyBoardINST.Key";
            this.dkey.Location = new System.Drawing.Point(3, 3);
            this.dkey.Name = "dkey";
            this.dkey.Orientation = AntdUI.TOrientation.Left;
            this.dkey.Size = new System.Drawing.Size(456, 25);
            this.dkey.TabIndex = 0;
            this.dkey.Text = "按键";
            // 
            // ciMouseINST
            // 
            this.ciMouseINST.Controls.Add(this.tlpMouseINST);
            this.ciMouseINST.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ciMouseINST.LocalizationText = "RobotEditForm.{id}";
            this.ciMouseINST.Location = new System.Drawing.Point(-472, -369);
            this.ciMouseINST.Name = "ciMouseINST";
            this.ciMouseINST.Size = new System.Drawing.Size(472, 369);
            this.ciMouseINST.TabIndex = 3;
            this.ciMouseINST.Text = "鼠标指令";
            // 
            // tlpMouseINST
            // 
            this.tlpMouseINST.ColumnCount = 1;
            this.tlpMouseINST.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMouseINST.Controls.Add(this.pMouseMove, 0, 2);
            this.tlpMouseINST.Controls.Add(this.pMouseWheel, 0, 1);
            this.tlpMouseINST.Controls.Add(this.pMouseKey, 0, 0);
            this.tlpMouseINST.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMouseINST.Location = new System.Drawing.Point(0, 0);
            this.tlpMouseINST.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMouseINST.Name = "tlpMouseINST";
            this.tlpMouseINST.RowCount = 3;
            this.tlpMouseINST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tlpMouseINST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tlpMouseINST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMouseINST.Size = new System.Drawing.Size(472, 369);
            this.tlpMouseINST.TabIndex = 2;
            // 
            // pMouseMove
            // 
            this.pMouseMove.BorderWidth = 2F;
            this.pMouseMove.Controls.Add(this.tlpMouseMove);
            this.pMouseMove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pMouseMove.Location = new System.Drawing.Point(3, 233);
            this.pMouseMove.Name = "pMouseMove";
            this.pMouseMove.Radius = 10;
            this.pMouseMove.Size = new System.Drawing.Size(466, 133);
            this.pMouseMove.TabIndex = 16;
            // 
            // tlpMouseMove
            // 
            this.tlpMouseMove.BackColor = System.Drawing.Color.Transparent;
            this.tlpMouseMove.ColumnCount = 1;
            this.tlpMouseMove.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMouseMove.Controls.Add(this.divider8, 0, 0);
            this.tlpMouseMove.Controls.Add(this.tlpMouseMove2, 0, 1);
            this.tlpMouseMove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMouseMove.Location = new System.Drawing.Point(2, 2);
            this.tlpMouseMove.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMouseMove.Name = "tlpMouseMove";
            this.tlpMouseMove.RowCount = 3;
            this.tlpMouseMove.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMouseMove.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMouseMove.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMouseMove.Size = new System.Drawing.Size(462, 129);
            this.tlpMouseMove.TabIndex = 0;
            // 
            // divider8
            // 
            this.divider8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.divider8.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.divider8.Location = new System.Drawing.Point(3, 3);
            this.divider8.Name = "divider8";
            this.divider8.Orientation = AntdUI.TOrientation.Left;
            this.divider8.Size = new System.Drawing.Size(456, 25);
            this.divider8.TabIndex = 0;
            this.divider8.Text = "移动";
            // 
            // tlpMouseMove2
            // 
            this.tlpMouseMove2.ColumnCount = 3;
            this.tlpMouseMove2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMouseMove2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMouseMove2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMouseMove2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMouseMove2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpMouseMove2.Controls.Add(this.nudMouseMove_X, 0, 1);
            this.tlpMouseMove2.Controls.Add(this.rbMoveBy, 1, 0);
            this.tlpMouseMove2.Controls.Add(this.bInsert_MouseMove, 2, 0);
            this.tlpMouseMove2.Controls.Add(this.nudMouseMove_Y, 1, 1);
            this.tlpMouseMove2.Controls.Add(this.rbMoveTo, 0, 0);
            this.tlpMouseMove2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMouseMove2.Location = new System.Drawing.Point(0, 31);
            this.tlpMouseMove2.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMouseMove2.Name = "tlpMouseMove2";
            this.tlpMouseMove2.RowCount = 3;
            this.tlpMouseMove2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMouseMove2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMouseMove2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMouseMove2.Size = new System.Drawing.Size(462, 140);
            this.tlpMouseMove2.TabIndex = 1;
            // 
            // nudMouseMove_X
            // 
            this.nudMouseMove_X.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudMouseMove_X.LocalizationSuffixText = "Pixel";
            this.nudMouseMove_X.Location = new System.Drawing.Point(3, 49);
            this.nudMouseMove_X.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudMouseMove_X.Name = "nudMouseMove_X";
            this.nudMouseMove_X.PrefixText = "X :";
            this.nudMouseMove_X.Size = new System.Drawing.Size(202, 39);
            this.nudMouseMove_X.SuffixText = "像素";
            this.nudMouseMove_X.TabIndex = 19;
            this.nudMouseMove_X.Text = "0";
            this.nudMouseMove_X.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // rbMoveBy
            // 
            this.rbMoveBy.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbMoveBy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbMoveBy.LocalizationText = "MoveBy";
            this.rbMoveBy.Location = new System.Drawing.Point(211, 3);
            this.rbMoveBy.Name = "rbMoveBy";
            this.rbMoveBy.Size = new System.Drawing.Size(90, 36);
            this.rbMoveBy.TabIndex = 18;
            this.rbMoveBy.Text = "相对移动";
            // 
            // bInsert_MouseMove
            // 
            this.bInsert_MouseMove.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bInsert_MouseMove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bInsert_MouseMove.IconSvg = "ArrowRightOutlined";
            this.bInsert_MouseMove.Location = new System.Drawing.Point(419, 3);
            this.bInsert_MouseMove.Name = "bInsert_MouseMove";
            this.bInsert_MouseMove.Size = new System.Drawing.Size(40, 40);
            this.bInsert_MouseMove.TabIndex = 1;
            this.bInsert_MouseMove.Type = AntdUI.TTypeMini.Primary;
            this.bInsert_MouseMove.Click += new System.EventHandler(this.bInsert_MouseMove_Click);
            // 
            // nudMouseMove_Y
            // 
            this.nudMouseMove_Y.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudMouseMove_Y.LocalizationSuffixText = "Pixel";
            this.nudMouseMove_Y.Location = new System.Drawing.Point(211, 49);
            this.nudMouseMove_Y.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudMouseMove_Y.Name = "nudMouseMove_Y";
            this.nudMouseMove_Y.PrefixText = "Y :";
            this.nudMouseMove_Y.Size = new System.Drawing.Size(202, 39);
            this.nudMouseMove_Y.SuffixText = "像素";
            this.nudMouseMove_Y.TabIndex = 16;
            this.nudMouseMove_Y.Text = "0";
            this.nudMouseMove_Y.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // rbMoveTo
            // 
            this.rbMoveTo.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbMoveTo.Checked = true;
            this.rbMoveTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbMoveTo.LocalizationText = "MoveTo";
            this.rbMoveTo.Location = new System.Drawing.Point(3, 3);
            this.rbMoveTo.Name = "rbMoveTo";
            this.rbMoveTo.Size = new System.Drawing.Size(76, 36);
            this.rbMoveTo.TabIndex = 17;
            this.rbMoveTo.Text = "移动到";
            // 
            // pMouseWheel
            // 
            this.pMouseWheel.BorderWidth = 2F;
            this.pMouseWheel.Controls.Add(this.tlpMouseWheel);
            this.pMouseWheel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pMouseWheel.Location = new System.Drawing.Point(3, 93);
            this.pMouseWheel.Name = "pMouseWheel";
            this.pMouseWheel.Radius = 10;
            this.pMouseWheel.Size = new System.Drawing.Size(466, 134);
            this.pMouseWheel.TabIndex = 15;
            // 
            // tlpMouseWheel
            // 
            this.tlpMouseWheel.BackColor = System.Drawing.Color.Transparent;
            this.tlpMouseWheel.ColumnCount = 1;
            this.tlpMouseWheel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMouseWheel.Controls.Add(this.dMouseWheel, 0, 0);
            this.tlpMouseWheel.Controls.Add(this.tlpMouseWheel2, 0, 1);
            this.tlpMouseWheel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMouseWheel.Location = new System.Drawing.Point(2, 2);
            this.tlpMouseWheel.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMouseWheel.Name = "tlpMouseWheel";
            this.tlpMouseWheel.RowCount = 3;
            this.tlpMouseWheel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMouseWheel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMouseWheel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMouseWheel.Size = new System.Drawing.Size(462, 130);
            this.tlpMouseWheel.TabIndex = 0;
            // 
            // dMouseWheel
            // 
            this.dMouseWheel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dMouseWheel.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dMouseWheel.LocalizationText = "RobotEditForm.MouseINST.Wheel";
            this.dMouseWheel.Location = new System.Drawing.Point(3, 3);
            this.dMouseWheel.Name = "dMouseWheel";
            this.dMouseWheel.Orientation = AntdUI.TOrientation.Left;
            this.dMouseWheel.Size = new System.Drawing.Size(456, 25);
            this.dMouseWheel.TabIndex = 0;
            this.dMouseWheel.Text = "滚轮";
            // 
            // tlpMouseWheel2
            // 
            this.tlpMouseWheel2.ColumnCount = 4;
            this.tlpMouseWheel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tlpMouseWheel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMouseWheel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMouseWheel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMouseWheel2.Controls.Add(this.lWheelDistance, 1, 1);
            this.tlpMouseWheel2.Controls.Add(this.ddlMouseWheel, 2, 0);
            this.tlpMouseWheel2.Controls.Add(this.lMouseWheel, 1, 0);
            this.tlpMouseWheel2.Controls.Add(this.bInsert_MouseWheel, 3, 0);
            this.tlpMouseWheel2.Controls.Add(this.nudWheelDistance, 2, 1);
            this.tlpMouseWheel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMouseWheel2.Location = new System.Drawing.Point(0, 31);
            this.tlpMouseWheel2.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMouseWheel2.Name = "tlpMouseWheel2";
            this.tlpMouseWheel2.RowCount = 3;
            this.tlpMouseWheel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMouseWheel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tlpMouseWheel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMouseWheel2.Size = new System.Drawing.Size(462, 100);
            this.tlpMouseWheel2.TabIndex = 1;
            // 
            // lWheelDistance
            // 
            this.lWheelDistance.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lWheelDistance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lWheelDistance.LocalizationText = "RobotEditForm.MouseINST.Wheel.Distance";
            this.lWheelDistance.Location = new System.Drawing.Point(13, 51);
            this.lWheelDistance.Name = "lWheelDistance";
            this.lWheelDistance.Size = new System.Drawing.Size(34, 39);
            this.lWheelDistance.TabIndex = 15;
            this.lWheelDistance.Text = "距离 :";
            this.lWheelDistance.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ddlMouseWheel
            // 
            this.ddlMouseWheel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddlMouseWheel.List = true;
            this.ddlMouseWheel.LocalizationPlaceholderText = "PleaseSelect";
            this.ddlMouseWheel.Location = new System.Drawing.Point(53, 3);
            this.ddlMouseWheel.Name = "ddlMouseWheel";
            this.ddlMouseWheel.PlaceholderText = "请选择";
            this.ddlMouseWheel.PrefixSvg = "";
            this.ddlMouseWheel.Size = new System.Drawing.Size(360, 42);
            this.ddlMouseWheel.TabIndex = 14;
            this.ddlMouseWheel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // lMouseWheel
            // 
            this.lMouseWheel.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lMouseWheel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lMouseWheel.LocalizationText = "RobotEditForm.MouseINST.Wheel.Scroll";
            this.lMouseWheel.Location = new System.Drawing.Point(13, 3);
            this.lMouseWheel.Name = "lMouseWheel";
            this.lMouseWheel.Size = new System.Drawing.Size(34, 42);
            this.lMouseWheel.TabIndex = 13;
            this.lMouseWheel.Text = "滚动 :";
            this.lMouseWheel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // bInsert_MouseWheel
            // 
            this.bInsert_MouseWheel.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bInsert_MouseWheel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bInsert_MouseWheel.IconSvg = "ArrowRightOutlined";
            this.bInsert_MouseWheel.Location = new System.Drawing.Point(419, 3);
            this.bInsert_MouseWheel.Name = "bInsert_MouseWheel";
            this.bInsert_MouseWheel.Size = new System.Drawing.Size(40, 40);
            this.bInsert_MouseWheel.TabIndex = 1;
            this.bInsert_MouseWheel.Type = AntdUI.TTypeMini.Primary;
            this.bInsert_MouseWheel.Click += new System.EventHandler(this.bInsert_MouseWheel_Click);
            // 
            // nudWheelDistance
            // 
            this.nudWheelDistance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudWheelDistance.LocalizationSuffixText = "Pixel";
            this.nudWheelDistance.Location = new System.Drawing.Point(53, 51);
            this.nudWheelDistance.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudWheelDistance.Name = "nudWheelDistance";
            this.nudWheelDistance.Size = new System.Drawing.Size(360, 39);
            this.nudWheelDistance.SuffixText = "像素";
            this.nudWheelDistance.TabIndex = 16;
            this.nudWheelDistance.Text = "10";
            this.nudWheelDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudWheelDistance.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // pMouseKey
            // 
            this.pMouseKey.BorderWidth = 2F;
            this.pMouseKey.Controls.Add(this.tlpMouseKey);
            this.pMouseKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pMouseKey.Location = new System.Drawing.Point(3, 3);
            this.pMouseKey.Name = "pMouseKey";
            this.pMouseKey.Radius = 10;
            this.pMouseKey.Size = new System.Drawing.Size(466, 84);
            this.pMouseKey.TabIndex = 14;
            // 
            // tlpMouseKey
            // 
            this.tlpMouseKey.BackColor = System.Drawing.Color.Transparent;
            this.tlpMouseKey.ColumnCount = 1;
            this.tlpMouseKey.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMouseKey.Controls.Add(this.tlpMouseKey2, 0, 1);
            this.tlpMouseKey.Controls.Add(this.dMouseKey, 0, 0);
            this.tlpMouseKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMouseKey.Location = new System.Drawing.Point(2, 2);
            this.tlpMouseKey.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMouseKey.Name = "tlpMouseKey";
            this.tlpMouseKey.RowCount = 3;
            this.tlpMouseKey.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMouseKey.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMouseKey.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMouseKey.Size = new System.Drawing.Size(462, 80);
            this.tlpMouseKey.TabIndex = 0;
            // 
            // tlpMouseKey2
            // 
            this.tlpMouseKey2.ColumnCount = 4;
            this.tlpMouseKey2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.tlpMouseKey2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMouseKey2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMouseKey2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMouseKey2.Controls.Add(this.bInsert_MouseKey, 3, 0);
            this.tlpMouseKey2.Controls.Add(this.lMouseKey, 1, 0);
            this.tlpMouseKey2.Controls.Add(this.ddlMouseKey, 2, 0);
            this.tlpMouseKey2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMouseKey2.Location = new System.Drawing.Point(0, 31);
            this.tlpMouseKey2.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMouseKey2.Name = "tlpMouseKey2";
            this.tlpMouseKey2.RowCount = 2;
            this.tlpMouseKey2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMouseKey2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMouseKey2.Size = new System.Drawing.Size(462, 50);
            this.tlpMouseKey2.TabIndex = 3;
            // 
            // bInsert_MouseKey
            // 
            this.bInsert_MouseKey.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bInsert_MouseKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bInsert_MouseKey.IconSvg = "ArrowRightOutlined";
            this.bInsert_MouseKey.Location = new System.Drawing.Point(419, 3);
            this.bInsert_MouseKey.Name = "bInsert_MouseKey";
            this.bInsert_MouseKey.Size = new System.Drawing.Size(40, 40);
            this.bInsert_MouseKey.TabIndex = 1;
            this.bInsert_MouseKey.Type = AntdUI.TTypeMini.Primary;
            this.bInsert_MouseKey.Click += new System.EventHandler(this.bInsert_MouseKey_Click);
            // 
            // lMouseKey
            // 
            this.lMouseKey.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lMouseKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lMouseKey.LocalizationText = "RobotEditForm.MouseINST.Key";
            this.lMouseKey.Location = new System.Drawing.Point(13, 3);
            this.lMouseKey.Name = "lMouseKey";
            this.lMouseKey.Size = new System.Drawing.Size(34, 42);
            this.lMouseKey.TabIndex = 12;
            this.lMouseKey.Text = "按键 :";
            this.lMouseKey.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ddlMouseKey
            // 
            this.ddlMouseKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddlMouseKey.List = true;
            this.ddlMouseKey.LocalizationPlaceholderText = "PleaseSelect";
            this.ddlMouseKey.Location = new System.Drawing.Point(53, 3);
            this.ddlMouseKey.Name = "ddlMouseKey";
            this.ddlMouseKey.PlaceholderText = "请选择";
            this.ddlMouseKey.Size = new System.Drawing.Size(360, 42);
            this.ddlMouseKey.TabIndex = 13;
            this.ddlMouseKey.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dMouseKey
            // 
            this.dMouseKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dMouseKey.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dMouseKey.LocalizationText = "RobotEditForm.MouseINST.Key";
            this.dMouseKey.Location = new System.Drawing.Point(3, 3);
            this.dMouseKey.Name = "dMouseKey";
            this.dMouseKey.Orientation = AntdUI.TOrientation.Left;
            this.dMouseKey.Size = new System.Drawing.Size(456, 25);
            this.dMouseKey.TabIndex = 0;
            this.dMouseKey.Text = "按键";
            // 
            // pRobotINST
            // 
            this.pRobotINST.BorderWidth = 1F;
            this.pRobotINST.Controls.Add(this.tRobotInstruction);
            this.pRobotINST.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pRobotINST.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pRobotINST.Location = new System.Drawing.Point(553, 3);
            this.pRobotINST.Name = "pRobotINST";
            this.pRobotINST.Padding = new System.Windows.Forms.Padding(3);
            this.pRobotINST.Size = new System.Drawing.Size(544, 584);
            this.pRobotINST.TabIndex = 8;
            this.pRobotINST.Text = "panel1";
            // 
            // tRobotInstruction
            // 
            this.tRobotInstruction.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tRobotInstruction.CellImpactHeight = false;
            this.tRobotInstruction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tRobotInstruction.Font = new System.Drawing.Font("微软雅黑", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tRobotInstruction.Gap = 8;
            this.tRobotInstruction.GapCell = 0;
            this.tRobotInstruction.Gaps = new System.Drawing.Size(8, 8);
            this.tRobotInstruction.Location = new System.Drawing.Point(4, 4);
            this.tRobotInstruction.Name = "tRobotInstruction";
            this.tRobotInstruction.Size = new System.Drawing.Size(536, 576);
            this.tRobotInstruction.TabIndex = 3;
            this.tRobotInstruction.VisibleHeader = false;
            this.tRobotInstruction.CellClick += new AntdUI.Table.ClickEventHandler(this.tRobotINST_CellClick);
            // 
            // txtINSTLog
            // 
            this.txtINSTLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtINSTLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(119)))), ((int)(((byte)(255)))));
            this.txtINSTLog.LocalizationPrefixText = "RobotEditForm.Record";
            this.txtINSTLog.Location = new System.Drawing.Point(553, 593);
            this.txtINSTLog.Name = "txtINSTLog";
            this.txtINSTLog.PlaceholderText = "";
            this.txtINSTLog.PrefixText = "运行记录:";
            this.txtINSTLog.ReadOnly = true;
            this.txtINSTLog.Size = new System.Drawing.Size(544, 44);
            this.txtINSTLog.TabIndex = 7;
            this.txtINSTLog.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtRobotName
            // 
            this.txtRobotName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRobotName.LocalizationPlaceholderText = "Input.Text";
            this.txtRobotName.LocalizationPrefixText = "RobotEditForm.RName";
            this.txtRobotName.Location = new System.Drawing.Point(3, 593);
            this.txtRobotName.Name = "txtRobotName";
            this.txtRobotName.PlaceholderText = "请输入字符";
            this.txtRobotName.PrefixText = "机器人名称:";
            this.txtRobotName.Size = new System.Drawing.Size(544, 44);
            this.txtRobotName.TabIndex = 6;
            this.txtRobotName.TextChanged += new System.EventHandler(this.txtRobotName_TextChanged);
            // 
            // RobotEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpRobotEdit);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "RobotEdit";
            this.Size = new System.Drawing.Size(1100, 700);
            this.Load += new System.EventHandler(this.RobotEdit_Load);
            this.tlpRobotEdit.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.tlpRobotINST.ResumeLayout(false);
            this.cRobotINST.ResumeLayout(false);
            this.ciPacketINST.ResumeLayout(false);
            this.tlpPacketINST.ResumeLayout(false);
            this.pSYSSocket.ResumeLayout(false);
            this.tlpSYSSocket2.ResumeLayout(false);
            this.tlpSYSSocket.ResumeLayout(false);
            this.tlpSYSSocket.PerformLayout();
            this.pPacketList.ResumeLayout(false);
            this.tlpPacketList2.ResumeLayout(false);
            this.tlpPacketList.ResumeLayout(false);
            this.tlpPacketList.PerformLayout();
            this.pSendList.ResumeLayout(false);
            this.tlpSendList2.ResumeLayout(false);
            this.tlpSendList.ResumeLayout(false);
            this.tlpSendList.PerformLayout();
            this.ciControlINST.ResumeLayout(false);
            this.tlpControlINST.ResumeLayout(false);
            this.pLoop.ResumeLayout(false);
            this.tlpLoop.ResumeLayout(false);
            this.tlpLoop2.ResumeLayout(false);
            this.tlpLoop2.PerformLayout();
            this.pDelay.ResumeLayout(false);
            this.tlpDelay.ResumeLayout(false);
            this.tlpDelay2.ResumeLayout(false);
            this.tlpDelay2.PerformLayout();
            this.ciKeyBoardINST.ResumeLayout(false);
            this.tlpKeyboardINST.ResumeLayout(false);
            this.pText.ResumeLayout(false);
            this.tlpText.ResumeLayout(false);
            this.tlpText2.ResumeLayout(false);
            this.tlpText2.PerformLayout();
            this.pKeyCombination.ResumeLayout(false);
            this.tlpKeyCombination.ResumeLayout(false);
            this.tlpKeyCombination2.ResumeLayout(false);
            this.tlpKeyCombination2.PerformLayout();
            this.pKeyBoard.ResumeLayout(false);
            this.tlpKey.ResumeLayout(false);
            this.tlpKey2.ResumeLayout(false);
            this.tlpKey2.PerformLayout();
            this.ciMouseINST.ResumeLayout(false);
            this.tlpMouseINST.ResumeLayout(false);
            this.pMouseMove.ResumeLayout(false);
            this.tlpMouseMove.ResumeLayout(false);
            this.tlpMouseMove2.ResumeLayout(false);
            this.tlpMouseMove2.PerformLayout();
            this.pMouseWheel.ResumeLayout(false);
            this.tlpMouseWheel.ResumeLayout(false);
            this.tlpMouseWheel2.ResumeLayout(false);
            this.tlpMouseWheel2.PerformLayout();
            this.pMouseKey.ResumeLayout(false);
            this.tlpMouseKey.ResumeLayout(false);
            this.tlpMouseKey2.ResumeLayout(false);
            this.tlpMouseKey2.PerformLayout();
            this.pRobotINST.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpRobotEdit;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bStop;
        private AntdUI.Button bExecute;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private TableLayoutPanelEx tlpRobotINST;
        private AntdUI.Collapse cRobotINST;
        private AntdUI.CollapseItem ciPacketINST;
        private TableLayoutPanelEx tlpPacketINST;
        private AntdUI.Panel pSYSSocket;
        private TableLayoutPanelEx tlpSYSSocket2;
        private TableLayoutPanelEx tlpSYSSocket;
        private AntdUI.Label lSelectFilter;
        private AntdUI.Radio rbSelectSocket;
        private AntdUI.Radio rbSelectFilter;
        private AntdUI.Button bInsert_SYSSocket;
        private AntdUI.Radio rbSelectPacket;
        private AntdUI.Label lSelectPacket;
        private AntdUI.InputNumber nudSelectSocket;
        private AntdUI.Divider dSYSSocket;
        private AntdUI.Panel pPacketList;
        private TableLayoutPanelEx tlpPacketList2;
        private TableLayoutPanelEx tlpPacketList;
        private AntdUI.Button bInsert_PacketList;
        private AntdUI.Label lPacketList;
        private AntdUI.Divider dPacketList;
        private AntdUI.Panel pSendList;
        private TableLayoutPanelEx tlpSendList2;
        private AntdUI.Divider dSendList;
        private TableLayoutPanelEx tlpSendList;
        private AntdUI.Select ddlSendList;
        private AntdUI.Button bInsert_SendList;
        private AntdUI.CollapseItem ciControlINST;
        private TableLayoutPanelEx tlpControlINST;
        private AntdUI.Panel pLoop;
        private TableLayoutPanelEx tlpLoop;
        private AntdUI.Divider dLoop;
        private TableLayoutPanelEx tlpLoop2;
        private AntdUI.InputNumber nudLoop;
        private AntdUI.Button bInsert_LoopStart;
        private AntdUI.Button bInsert_LoopEnd;
        private AntdUI.Panel pDelay;
        private TableLayoutPanelEx tlpDelay;
        private TableLayoutPanelEx tlpDelay2;
        private AntdUI.InputNumber nudnudDelayRandom_To;
        private AntdUI.InputNumber nudDelayFix;
        private AntdUI.Radio rbDelayRandom;
        private AntdUI.Button bInsert_Delay;
        private AntdUI.Radio rbDelayFix;
        private AntdUI.InputNumber nudnudDelayRandom_From;
        private AntdUI.Divider dDelay;
        private AntdUI.CollapseItem ciKeyBoardINST;
        private TableLayoutPanelEx tlpKeyboardINST;
        private AntdUI.Panel pText;
        private TableLayoutPanelEx tlpText;
        private AntdUI.Divider dText;
        private TableLayoutPanelEx tlpText2;
        private AntdUI.Button bInsert_Text;
        private AntdUI.Input txtText;
        private AntdUI.Panel pKeyCombination;
        private TableLayoutPanelEx tlpKeyCombination;
        private AntdUI.Divider dKeyCombination;
        private TableLayoutPanelEx tlpKeyCombination2;
        private AntdUI.Button bInsert_KeyCombination;
        private HotkeyTextBox txtKeyCombination;
        private AntdUI.Panel pKeyBoard;
        private TableLayoutPanelEx tlpKey;
        private TableLayoutPanelEx tlpKey2;
        private AntdUI.Button bInsert_KeyBoard;
        private AntdUI.Label lkey;
        private AntdUI.Label lKeyType;
        private AntdUI.Input txtKey;
        private AntdUI.Select ddlKeyType;
        private AntdUI.Divider dkey;
        private AntdUI.CollapseItem ciMouseINST;
        private TableLayoutPanelEx tlpMouseINST;
        private AntdUI.Panel pMouseMove;
        private TableLayoutPanelEx tlpMouseMove;
        private AntdUI.Divider divider8;
        private TableLayoutPanelEx tlpMouseMove2;
        private AntdUI.InputNumber nudMouseMove_X;
        private AntdUI.Radio rbMoveBy;
        private AntdUI.Button bInsert_MouseMove;
        private AntdUI.InputNumber nudMouseMove_Y;
        private AntdUI.Radio rbMoveTo;
        private AntdUI.Panel pMouseWheel;
        private TableLayoutPanelEx tlpMouseWheel;
        private AntdUI.Divider dMouseWheel;
        private TableLayoutPanelEx tlpMouseWheel2;
        private AntdUI.Label lWheelDistance;
        private AntdUI.Select ddlMouseWheel;
        private AntdUI.Label lMouseWheel;
        private AntdUI.Button bInsert_MouseWheel;
        private AntdUI.InputNumber nudWheelDistance;
        private AntdUI.Panel pMouseKey;
        private TableLayoutPanelEx tlpMouseKey;
        private TableLayoutPanelEx tlpMouseKey2;
        private AntdUI.Button bInsert_MouseKey;
        private AntdUI.Label lMouseKey;
        private AntdUI.Select ddlMouseKey;
        private AntdUI.Divider dMouseKey;
        private AntdUI.Panel pRobotINST;
        private AntdUI.Table tRobotInstruction;
        private AntdUI.Input txtINSTLog;
        private AntdUI.Input txtRobotName;
    }
}
