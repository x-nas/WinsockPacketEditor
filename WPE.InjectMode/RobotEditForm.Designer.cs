namespace WPE.InjectMode
{
    partial class RobotEditForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RobotEditForm));
            this.tlpRobotEdit = new System.Windows.Forms.TableLayoutPanel();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bStop = new AntdUI.Button();
            this.bExecute = new AntdUI.Button();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tlpRobotINST = new System.Windows.Forms.TableLayoutPanel();
            this.cRobotINST = new AntdUI.Collapse();
            this.ciPacketINST = new AntdUI.CollapseItem();
            this.tlpPacketINST = new System.Windows.Forms.TableLayoutPanel();
            this.pSYSSocket = new AntdUI.Panel();
            this.tlpSYSSocket2 = new System.Windows.Forms.TableLayoutPanel();
            this.tlpSYSSocket = new System.Windows.Forms.TableLayoutPanel();
            this.lSelectFilter = new AntdUI.Label();
            this.rbSelectSocket = new AntdUI.Radio();
            this.rbSelectFilter = new AntdUI.Radio();
            this.bInsert_SYSSocket = new AntdUI.Button();
            this.rbSelectPacket = new AntdUI.Radio();
            this.lSelectPacket = new AntdUI.Label();
            this.nudSelectSocket = new AntdUI.InputNumber();
            this.dSYSSocket = new AntdUI.Divider();
            this.pPacketList = new AntdUI.Panel();
            this.tlpPacketList2 = new System.Windows.Forms.TableLayoutPanel();
            this.tlpPacketList = new System.Windows.Forms.TableLayoutPanel();
            this.bInsert_PacketList = new AntdUI.Button();
            this.lPacketList = new AntdUI.Label();
            this.dPacketList = new AntdUI.Divider();
            this.pSendList = new AntdUI.Panel();
            this.tlpSendList2 = new System.Windows.Forms.TableLayoutPanel();
            this.dSendList = new AntdUI.Divider();
            this.tlpSendList = new System.Windows.Forms.TableLayoutPanel();
            this.ddlSendList = new AntdUI.Select();
            this.bInsert_SendList = new AntdUI.Button();
            this.ciControlINST = new AntdUI.CollapseItem();
            this.tlpControlINST = new System.Windows.Forms.TableLayoutPanel();
            this.pLoop = new AntdUI.Panel();
            this.tlpLoop = new System.Windows.Forms.TableLayoutPanel();
            this.dLoop = new AntdUI.Divider();
            this.tlpLoop2 = new System.Windows.Forms.TableLayoutPanel();
            this.nudLoop = new AntdUI.InputNumber();
            this.bInsert_LoopStart = new AntdUI.Button();
            this.bInsert_LoopEnd = new AntdUI.Button();
            this.pDelay = new AntdUI.Panel();
            this.tlpDelay = new System.Windows.Forms.TableLayoutPanel();
            this.tlpDelay2 = new System.Windows.Forms.TableLayoutPanel();
            this.nudnudDelayRandom_To = new AntdUI.InputNumber();
            this.nudDelayFix = new AntdUI.InputNumber();
            this.rbDelayRandom = new AntdUI.Radio();
            this.bInsert_Delay = new AntdUI.Button();
            this.rbDelayFix = new AntdUI.Radio();
            this.nudnudDelayRandom_From = new AntdUI.InputNumber();
            this.dDelay = new AntdUI.Divider();
            this.ciKeyBoardINST = new AntdUI.CollapseItem();
            this.tlpKeyboardINST = new System.Windows.Forms.TableLayoutPanel();
            this.pText = new AntdUI.Panel();
            this.tlpText = new System.Windows.Forms.TableLayoutPanel();
            this.dText = new AntdUI.Divider();
            this.tlpText2 = new System.Windows.Forms.TableLayoutPanel();
            this.bInsert_Text = new AntdUI.Button();
            this.txtText = new AntdUI.Input();
            this.pKeyCombination = new AntdUI.Panel();
            this.tlpKeyCombination = new System.Windows.Forms.TableLayoutPanel();
            this.dKeyCombination = new AntdUI.Divider();
            this.tlpKeyCombination2 = new System.Windows.Forms.TableLayoutPanel();
            this.bInsert_KeyCombination = new AntdUI.Button();
            this.txtKeyCombination = new WPE.Lib.Controls.HotkeyTextBox();
            this.pKeyBoard = new AntdUI.Panel();
            this.tlpKey = new System.Windows.Forms.TableLayoutPanel();
            this.tlpKey2 = new System.Windows.Forms.TableLayoutPanel();
            this.bInsert_KeyBoard = new AntdUI.Button();
            this.lkey = new AntdUI.Label();
            this.lKeyType = new AntdUI.Label();
            this.txtKey = new AntdUI.Input();
            this.ddlKeyType = new AntdUI.Select();
            this.dkey = new AntdUI.Divider();
            this.ciMouseINST = new AntdUI.CollapseItem();
            this.tlpMouseINST = new System.Windows.Forms.TableLayoutPanel();
            this.pMouseMove = new AntdUI.Panel();
            this.tlpMouseMove = new System.Windows.Forms.TableLayoutPanel();
            this.divider8 = new AntdUI.Divider();
            this.tlpMouseMove2 = new System.Windows.Forms.TableLayoutPanel();
            this.nudMouseMove_X = new AntdUI.InputNumber();
            this.rbMoveBy = new AntdUI.Radio();
            this.lMouseMove_Y = new AntdUI.Label();
            this.lMouseMove_X = new AntdUI.Label();
            this.bInsert_MouseMove = new AntdUI.Button();
            this.nudMouseMove_Y = new AntdUI.InputNumber();
            this.rbMoveTo = new AntdUI.Radio();
            this.pMouseWheel = new AntdUI.Panel();
            this.tlpMouseWheel = new System.Windows.Forms.TableLayoutPanel();
            this.dMouseWheel = new AntdUI.Divider();
            this.tlpMouseWheel2 = new System.Windows.Forms.TableLayoutPanel();
            this.lWheelDistance = new AntdUI.Label();
            this.ddlMouseWheel = new AntdUI.Select();
            this.lMouseWheel = new AntdUI.Label();
            this.bInsert_MouseWheel = new AntdUI.Button();
            this.nudWheelDistance = new AntdUI.InputNumber();
            this.pMouseKey = new AntdUI.Panel();
            this.tlpMouseKey = new System.Windows.Forms.TableLayoutPanel();
            this.tlpMouseKey2 = new System.Windows.Forms.TableLayoutPanel();
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
            this.tlpRobotEdit.Controls.Add(this.tlpButton, 0, 1);
            this.tlpRobotEdit.Controls.Add(this.tlpRobotINST, 0, 0);
            this.tlpRobotEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpRobotEdit.Location = new System.Drawing.Point(0, 0);
            this.tlpRobotEdit.Margin = new System.Windows.Forms.Padding(0);
            this.tlpRobotEdit.Name = "tlpRobotEdit";
            this.tlpRobotEdit.RowCount = 2;
            this.tlpRobotEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpRobotEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpRobotEdit.Size = new System.Drawing.Size(984, 761);
            this.tlpRobotEdit.TabIndex = 0;
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 9;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Controls.Add(this.bStop, 3, 1);
            this.tlpButton.Controls.Add(this.bExecute, 1, 1);
            this.tlpButton.Controls.Add(this.bSave, 5, 1);
            this.tlpButton.Controls.Add(this.bExit, 7, 1);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 701);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(984, 60);
            this.tlpButton.TabIndex = 11;
            // 
            // bStop
            // 
            this.bStop.BackExtend = "135, #6253E1, #04BEFE";
            this.bStop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bStop.Enabled = false;
            this.bStop.IconSvg = "PauseCircleOutlined";
            this.bStop.LoadingWaveColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(87)))), ((int)(((byte)(34)))));
            this.bStop.LoadingWaveCount = 6;
            this.bStop.LoadingWaveSize = 6;
            this.bStop.LoadingWaveValue = 0.6F;
            this.bStop.LoadingWaveVertical = true;
            this.bStop.Location = new System.Drawing.Point(365, 7);
            this.bStop.Name = "bStop";
            this.bStop.Size = new System.Drawing.Size(114, 46);
            this.bStop.TabIndex = 3;
            this.bStop.Text = "停止";
            this.bStop.Type = AntdUI.TTypeMini.Primary;
            this.bStop.Click += new System.EventHandler(this.bStop_Click);
            // 
            // bExecute
            // 
            this.bExecute.BackExtend = "135, #6253E1, #04BEFE";
            this.bExecute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bExecute.IconSvg = "PlayCircleOutlined";
            this.bExecute.LoadingWaveColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(87)))), ((int)(((byte)(34)))));
            this.bExecute.LoadingWaveCount = 6;
            this.bExecute.LoadingWaveSize = 6;
            this.bExecute.LoadingWaveValue = 0.6F;
            this.bExecute.LoadingWaveVertical = true;
            this.bExecute.Location = new System.Drawing.Point(225, 7);
            this.bExecute.Name = "bExecute";
            this.bExecute.Size = new System.Drawing.Size(114, 46);
            this.bExecute.TabIndex = 2;
            this.bExecute.Text = "执行";
            this.bExecute.Type = AntdUI.TTypeMini.Info;
            this.bExecute.Click += new System.EventHandler(this.bExecute_Click);
            // 
            // bSave
            // 
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.LoadingWaveColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(87)))), ((int)(((byte)(34)))));
            this.bSave.LoadingWaveCount = 6;
            this.bSave.LoadingWaveSize = 6;
            this.bSave.LoadingWaveValue = 0.6F;
            this.bSave.LoadingWaveVertical = true;
            this.bSave.Location = new System.Drawing.Point(505, 7);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(114, 46);
            this.bSave.TabIndex = 0;
            this.bSave.Text = "保存";
            this.bSave.Type = AntdUI.TTypeMini.Primary;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bExit
            // 
            this.bExit.BackExtend = "135, #6253E1, #04BEFE";
            this.bExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bExit.IconSvg = "LogoutOutlined";
            this.bExit.Location = new System.Drawing.Point(645, 7);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(114, 46);
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
            this.tlpRobotINST.Size = new System.Drawing.Size(984, 701);
            this.tlpRobotINST.TabIndex = 0;
            // 
            // cRobotINST
            // 
            this.cRobotINST.ContentPadding = new System.Drawing.Size(8, 8);
            this.cRobotINST.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cRobotINST.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cRobotINST.Gap = 3;
            this.cRobotINST.Items.Add(this.ciPacketINST);
            this.cRobotINST.Items.Add(this.ciControlINST);
            this.cRobotINST.Items.Add(this.ciKeyBoardINST);
            this.cRobotINST.Items.Add(this.ciMouseINST);
            this.cRobotINST.Location = new System.Drawing.Point(3, 3);
            this.cRobotINST.Name = "cRobotINST";
            this.cRobotINST.Size = new System.Drawing.Size(486, 645);
            this.cRobotINST.TabIndex = 9;
            this.cRobotINST.Unique = true;
            this.cRobotINST.UniqueFull = true;
            // 
            // ciPacketINST
            // 
            this.ciPacketINST.Controls.Add(this.tlpPacketINST);
            this.ciPacketINST.Expand = true;
            this.ciPacketINST.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ciPacketINST.Location = new System.Drawing.Point(11, 57);
            this.ciPacketINST.Name = "ciPacketINST";
            this.ciPacketINST.Size = new System.Drawing.Size(464, 430);
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
            this.tlpPacketINST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpPacketINST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpPacketINST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpPacketINST.Size = new System.Drawing.Size(464, 430);
            this.tlpPacketINST.TabIndex = 0;
            // 
            // pSYSSocket
            // 
            this.pSYSSocket.BorderWidth = 2F;
            this.pSYSSocket.Controls.Add(this.tlpSYSSocket2);
            this.pSYSSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pSYSSocket.Location = new System.Drawing.Point(3, 217);
            this.pSYSSocket.Name = "pSYSSocket";
            this.pSYSSocket.Radius = 10;
            this.pSYSSocket.Size = new System.Drawing.Size(458, 210);
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
            this.tlpSYSSocket2.Size = new System.Drawing.Size(454, 206);
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
            this.tlpSYSSocket.Size = new System.Drawing.Size(454, 135);
            this.tlpSYSSocket.TabIndex = 3;
            // 
            // lSelectFilter
            // 
            this.lSelectFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSelectFilter.Location = new System.Drawing.Point(57, 51);
            this.lSelectFilter.Name = "lSelectFilter";
            this.lSelectFilter.Size = new System.Drawing.Size(346, 39);
            this.lSelectFilter.TabIndex = 7;
            this.lSelectFilter.Text = "调用滤镜的套接字";
            // 
            // rbSelectSocket
            // 
            this.rbSelectSocket.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbSelectSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbSelectSocket.Location = new System.Drawing.Point(3, 96);
            this.rbSelectSocket.Name = "rbSelectSocket";
            this.rbSelectSocket.Size = new System.Drawing.Size(48, 38);
            this.rbSelectSocket.TabIndex = 5;
            this.rbSelectSocket.Text = "=";
            // 
            // rbSelectFilter
            // 
            this.rbSelectFilter.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbSelectFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbSelectFilter.Location = new System.Drawing.Point(3, 51);
            this.rbSelectFilter.Name = "rbSelectFilter";
            this.rbSelectFilter.Size = new System.Drawing.Size(48, 38);
            this.rbSelectFilter.TabIndex = 3;
            this.rbSelectFilter.Text = "=";
            // 
            // bInsert_SYSSocket
            // 
            this.bInsert_SYSSocket.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bInsert_SYSSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bInsert_SYSSocket.IconSvg = "ArrowRightOutlined";
            this.bInsert_SYSSocket.Location = new System.Drawing.Point(409, 3);
            this.bInsert_SYSSocket.Name = "bInsert_SYSSocket";
            this.bInsert_SYSSocket.Size = new System.Drawing.Size(42, 42);
            this.bInsert_SYSSocket.TabIndex = 1;
            this.bInsert_SYSSocket.Type = AntdUI.TTypeMini.Primary;
            // 
            // rbSelectPacket
            // 
            this.rbSelectPacket.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbSelectPacket.Checked = true;
            this.rbSelectPacket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbSelectPacket.Location = new System.Drawing.Point(3, 3);
            this.rbSelectPacket.Name = "rbSelectPacket";
            this.rbSelectPacket.Size = new System.Drawing.Size(48, 38);
            this.rbSelectPacket.TabIndex = 2;
            this.rbSelectPacket.Text = "=";
            // 
            // lSelectPacket
            // 
            this.lSelectPacket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSelectPacket.Location = new System.Drawing.Point(57, 3);
            this.lSelectPacket.Name = "lSelectPacket";
            this.lSelectPacket.Size = new System.Drawing.Size(346, 42);
            this.lSelectPacket.TabIndex = 6;
            this.lSelectPacket.Text = "选中封包的套接字";
            // 
            // nudSelectSocket
            // 
            this.nudSelectSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudSelectSocket.Location = new System.Drawing.Point(57, 96);
            this.nudSelectSocket.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudSelectSocket.Name = "nudSelectSocket";
            this.nudSelectSocket.SelectionStart = 1;
            this.nudSelectSocket.Size = new System.Drawing.Size(346, 39);
            this.nudSelectSocket.TabIndex = 8;
            this.nudSelectSocket.Text = "0";
            // 
            // dSYSSocket
            // 
            this.dSYSSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dSYSSocket.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dSYSSocket.Location = new System.Drawing.Point(3, 3);
            this.dSYSSocket.Name = "dSYSSocket";
            this.dSYSSocket.Orientation = AntdUI.TOrientation.Left;
            this.dSYSSocket.Size = new System.Drawing.Size(448, 25);
            this.dSYSSocket.TabIndex = 0;
            this.dSYSSocket.Text = "设置 - 系统套接字";
            // 
            // pPacketList
            // 
            this.pPacketList.BorderWidth = 2F;
            this.pPacketList.Controls.Add(this.tlpPacketList2);
            this.pPacketList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pPacketList.Location = new System.Drawing.Point(3, 110);
            this.pPacketList.Name = "pPacketList";
            this.pPacketList.Radius = 10;
            this.pPacketList.Size = new System.Drawing.Size(458, 101);
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
            this.tlpPacketList2.Size = new System.Drawing.Size(454, 97);
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
            this.tlpPacketList.Size = new System.Drawing.Size(454, 45);
            this.tlpPacketList.TabIndex = 2;
            // 
            // bInsert_PacketList
            // 
            this.bInsert_PacketList.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bInsert_PacketList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bInsert_PacketList.IconSvg = "ArrowRightOutlined";
            this.bInsert_PacketList.Location = new System.Drawing.Point(409, 3);
            this.bInsert_PacketList.Name = "bInsert_PacketList";
            this.bInsert_PacketList.Size = new System.Drawing.Size(42, 42);
            this.bInsert_PacketList.TabIndex = 1;
            this.bInsert_PacketList.Type = AntdUI.TTypeMini.Primary;
            // 
            // lPacketList
            // 
            this.lPacketList.AutoEllipsis = true;
            this.lPacketList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lPacketList.Location = new System.Drawing.Point(3, 3);
            this.lPacketList.Name = "lPacketList";
            this.lPacketList.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.lPacketList.Size = new System.Drawing.Size(400, 39);
            this.lPacketList.TabIndex = 2;
            this.lPacketList.Text = "封包列表中选中的封包";
            // 
            // dPacketList
            // 
            this.dPacketList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dPacketList.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dPacketList.Location = new System.Drawing.Point(3, 3);
            this.dPacketList.Name = "dPacketList";
            this.dPacketList.Orientation = AntdUI.TOrientation.Left;
            this.dPacketList.Size = new System.Drawing.Size(448, 25);
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
            this.pSendList.Size = new System.Drawing.Size(458, 101);
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
            this.tlpSendList2.Size = new System.Drawing.Size(454, 97);
            this.tlpSendList2.TabIndex = 0;
            // 
            // dSendList
            // 
            this.dSendList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dSendList.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dSendList.Location = new System.Drawing.Point(3, 3);
            this.dSendList.Name = "dSendList";
            this.dSendList.Orientation = AntdUI.TOrientation.Left;
            this.dSendList.Size = new System.Drawing.Size(448, 25);
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
            this.tlpSendList.Size = new System.Drawing.Size(454, 45);
            this.tlpSendList.TabIndex = 1;
            // 
            // ddlSendList
            // 
            this.ddlSendList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddlSendList.Location = new System.Drawing.Point(3, 3);
            this.ddlSendList.Name = "ddlSendList";
            this.ddlSendList.PlaceholderText = "请选择";
            this.ddlSendList.Size = new System.Drawing.Size(400, 39);
            this.ddlSendList.TabIndex = 0;
            // 
            // bInsert_SendList
            // 
            this.bInsert_SendList.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bInsert_SendList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bInsert_SendList.IconSvg = "ArrowRightOutlined";
            this.bInsert_SendList.Location = new System.Drawing.Point(409, 3);
            this.bInsert_SendList.Name = "bInsert_SendList";
            this.bInsert_SendList.Size = new System.Drawing.Size(42, 42);
            this.bInsert_SendList.TabIndex = 1;
            this.bInsert_SendList.Type = AntdUI.TTypeMini.Primary;
            // 
            // ciControlINST
            // 
            this.ciControlINST.Controls.Add(this.tlpControlINST);
            this.ciControlINST.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ciControlINST.Location = new System.Drawing.Point(-464, -430);
            this.ciControlINST.Name = "ciControlINST";
            this.ciControlINST.Size = new System.Drawing.Size(464, 430);
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
            this.tlpControlINST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tlpControlINST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpControlINST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tlpControlINST.Size = new System.Drawing.Size(464, 430);
            this.tlpControlINST.TabIndex = 0;
            // 
            // pLoop
            // 
            this.pLoop.BorderWidth = 2F;
            this.pLoop.Controls.Add(this.tlpLoop);
            this.pLoop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pLoop.Location = new System.Drawing.Point(3, 153);
            this.pLoop.Name = "pLoop";
            this.pLoop.Radius = 10;
            this.pLoop.Size = new System.Drawing.Size(458, 101);
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
            this.tlpLoop.Size = new System.Drawing.Size(454, 97);
            this.tlpLoop.TabIndex = 0;
            // 
            // dLoop
            // 
            this.dLoop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dLoop.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dLoop.Location = new System.Drawing.Point(3, 3);
            this.dLoop.Name = "dLoop";
            this.dLoop.Orientation = AntdUI.TOrientation.Left;
            this.dLoop.Size = new System.Drawing.Size(448, 25);
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
            this.tlpLoop2.Size = new System.Drawing.Size(454, 50);
            this.tlpLoop2.TabIndex = 1;
            // 
            // nudLoop
            // 
            this.nudLoop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudLoop.Location = new System.Drawing.Point(3, 3);
            this.nudLoop.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudLoop.Name = "nudLoop";
            this.nudLoop.SelectionStart = 1;
            this.nudLoop.Size = new System.Drawing.Size(282, 42);
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
            this.bInsert_LoopStart.Location = new System.Drawing.Point(291, 3);
            this.bInsert_LoopStart.Name = "bInsert_LoopStart";
            this.bInsert_LoopStart.Size = new System.Drawing.Size(77, 42);
            this.bInsert_LoopStart.TabIndex = 2;
            this.bInsert_LoopStart.Text = "开始";
            this.bInsert_LoopStart.Type = AntdUI.TTypeMini.Primary;
            // 
            // bInsert_LoopEnd
            // 
            this.bInsert_LoopEnd.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bInsert_LoopEnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bInsert_LoopEnd.IconPosition = AntdUI.TAlignMini.Right;
            this.bInsert_LoopEnd.IconSvg = "ArrowRightOutlined";
            this.bInsert_LoopEnd.Location = new System.Drawing.Point(374, 3);
            this.bInsert_LoopEnd.Name = "bInsert_LoopEnd";
            this.bInsert_LoopEnd.Size = new System.Drawing.Size(77, 42);
            this.bInsert_LoopEnd.TabIndex = 1;
            this.bInsert_LoopEnd.Text = "结束";
            this.bInsert_LoopEnd.Type = AntdUI.TTypeMini.Primary;
            // 
            // pDelay
            // 
            this.pDelay.BorderWidth = 2F;
            this.pDelay.Controls.Add(this.tlpDelay);
            this.pDelay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pDelay.Location = new System.Drawing.Point(3, 3);
            this.pDelay.Name = "pDelay";
            this.pDelay.Radius = 10;
            this.pDelay.Size = new System.Drawing.Size(458, 144);
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
            this.tlpDelay.Size = new System.Drawing.Size(454, 140);
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
            this.tlpDelay2.Size = new System.Drawing.Size(454, 100);
            this.tlpDelay2.TabIndex = 3;
            // 
            // nudnudDelayRandom_To
            // 
            this.nudnudDelayRandom_To.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudnudDelayRandom_To.Location = new System.Drawing.Point(241, 51);
            this.nudnudDelayRandom_To.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudnudDelayRandom_To.Name = "nudnudDelayRandom_To";
            this.nudnudDelayRandom_To.SelectionStart = 3;
            this.nudnudDelayRandom_To.Size = new System.Drawing.Size(161, 39);
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
            this.nudDelayFix.Location = new System.Drawing.Point(74, 3);
            this.nudDelayFix.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudDelayFix.Name = "nudDelayFix";
            this.nudDelayFix.SelectionStart = 3;
            this.nudDelayFix.Size = new System.Drawing.Size(161, 42);
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
            this.rbDelayRandom.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbDelayRandom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbDelayRandom.Location = new System.Drawing.Point(3, 51);
            this.rbDelayRandom.Name = "rbDelayRandom";
            this.rbDelayRandom.Size = new System.Drawing.Size(65, 38);
            this.rbDelayRandom.TabIndex = 5;
            this.rbDelayRandom.Text = "随机";
            // 
            // bInsert_Delay
            // 
            this.bInsert_Delay.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bInsert_Delay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bInsert_Delay.IconSvg = "ArrowRightOutlined";
            this.bInsert_Delay.Location = new System.Drawing.Point(408, 3);
            this.bInsert_Delay.Name = "bInsert_Delay";
            this.bInsert_Delay.Size = new System.Drawing.Size(42, 42);
            this.bInsert_Delay.TabIndex = 1;
            this.bInsert_Delay.Type = AntdUI.TTypeMini.Primary;
            // 
            // rbDelayFix
            // 
            this.rbDelayFix.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbDelayFix.Checked = true;
            this.rbDelayFix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbDelayFix.Location = new System.Drawing.Point(3, 3);
            this.rbDelayFix.Name = "rbDelayFix";
            this.rbDelayFix.Size = new System.Drawing.Size(65, 38);
            this.rbDelayFix.TabIndex = 2;
            this.rbDelayFix.Text = "定时";
            // 
            // nudnudDelayRandom_From
            // 
            this.nudnudDelayRandom_From.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudnudDelayRandom_From.Location = new System.Drawing.Point(74, 51);
            this.nudnudDelayRandom_From.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudnudDelayRandom_From.Name = "nudnudDelayRandom_From";
            this.nudnudDelayRandom_From.SelectionStart = 1;
            this.nudnudDelayRandom_From.Size = new System.Drawing.Size(161, 39);
            this.nudnudDelayRandom_From.SuffixText = "-";
            this.nudnudDelayRandom_From.TabIndex = 8;
            this.nudnudDelayRandom_From.Text = "0";
            this.nudnudDelayRandom_From.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dDelay
            // 
            this.dDelay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dDelay.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dDelay.Location = new System.Drawing.Point(3, 3);
            this.dDelay.Name = "dDelay";
            this.dDelay.Orientation = AntdUI.TOrientation.Left;
            this.dDelay.Size = new System.Drawing.Size(448, 25);
            this.dDelay.TabIndex = 0;
            this.dDelay.Text = "延迟";
            // 
            // ciKeyBoardINST
            // 
            this.ciKeyBoardINST.Controls.Add(this.tlpKeyboardINST);
            this.ciKeyBoardINST.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ciKeyBoardINST.Location = new System.Drawing.Point(-464, -430);
            this.ciKeyBoardINST.Name = "ciKeyBoardINST";
            this.ciKeyBoardINST.Size = new System.Drawing.Size(464, 430);
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
            this.tlpKeyboardINST.RowCount = 3;
            this.tlpKeyboardINST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tlpKeyboardINST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tlpKeyboardINST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tlpKeyboardINST.Size = new System.Drawing.Size(464, 430);
            this.tlpKeyboardINST.TabIndex = 1;
            // 
            // pText
            // 
            this.pText.BorderWidth = 2F;
            this.pText.Controls.Add(this.tlpText);
            this.pText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pText.Location = new System.Drawing.Point(3, 304);
            this.pText.Name = "pText";
            this.pText.Radius = 10;
            this.pText.Size = new System.Drawing.Size(458, 123);
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
            this.tlpText.Size = new System.Drawing.Size(454, 119);
            this.tlpText.TabIndex = 0;
            // 
            // dText
            // 
            this.dText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dText.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dText.Location = new System.Drawing.Point(3, 3);
            this.dText.Name = "dText";
            this.dText.Orientation = AntdUI.TOrientation.Left;
            this.dText.Size = new System.Drawing.Size(448, 25);
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
            this.tlpText2.Size = new System.Drawing.Size(454, 50);
            this.tlpText2.TabIndex = 1;
            // 
            // bInsert_Text
            // 
            this.bInsert_Text.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bInsert_Text.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bInsert_Text.IconSvg = "ArrowRightOutlined";
            this.bInsert_Text.Location = new System.Drawing.Point(409, 3);
            this.bInsert_Text.Name = "bInsert_Text";
            this.bInsert_Text.Size = new System.Drawing.Size(42, 42);
            this.bInsert_Text.TabIndex = 1;
            this.bInsert_Text.Type = AntdUI.TTypeMini.Primary;
            // 
            // txtText
            // 
            this.txtText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtText.Location = new System.Drawing.Point(3, 3);
            this.txtText.Name = "txtText";
            this.txtText.PlaceholderText = "请输入文本";
            this.txtText.Size = new System.Drawing.Size(400, 42);
            this.txtText.TabIndex = 2;
            // 
            // pKeyCombination
            // 
            this.pKeyCombination.BorderWidth = 2F;
            this.pKeyCombination.Controls.Add(this.tlpKeyCombination);
            this.pKeyCombination.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pKeyCombination.Location = new System.Drawing.Point(3, 175);
            this.pKeyCombination.Name = "pKeyCombination";
            this.pKeyCombination.Radius = 10;
            this.pKeyCombination.Size = new System.Drawing.Size(458, 123);
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
            this.tlpKeyCombination.Size = new System.Drawing.Size(454, 119);
            this.tlpKeyCombination.TabIndex = 0;
            // 
            // dKeyCombination
            // 
            this.dKeyCombination.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dKeyCombination.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dKeyCombination.Location = new System.Drawing.Point(3, 3);
            this.dKeyCombination.Name = "dKeyCombination";
            this.dKeyCombination.Orientation = AntdUI.TOrientation.Left;
            this.dKeyCombination.Size = new System.Drawing.Size(448, 25);
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
            this.tlpKeyCombination2.Size = new System.Drawing.Size(454, 50);
            this.tlpKeyCombination2.TabIndex = 1;
            // 
            // bInsert_KeyCombination
            // 
            this.bInsert_KeyCombination.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bInsert_KeyCombination.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bInsert_KeyCombination.IconSvg = "ArrowRightOutlined";
            this.bInsert_KeyCombination.Location = new System.Drawing.Point(409, 3);
            this.bInsert_KeyCombination.Name = "bInsert_KeyCombination";
            this.bInsert_KeyCombination.Size = new System.Drawing.Size(42, 42);
            this.bInsert_KeyCombination.TabIndex = 1;
            this.bInsert_KeyCombination.Type = AntdUI.TTypeMini.Primary;
            // 
            // txtKeyCombination
            // 
            this.txtKeyCombination.BackColor = System.Drawing.SystemColors.Window;
            this.txtKeyCombination.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtKeyCombination.ForeColor = System.Drawing.Color.Black;
            this.txtKeyCombination.Location = new System.Drawing.Point(3, 3);
            this.txtKeyCombination.Name = "txtKeyCombination";
            this.txtKeyCombination.PlaceholderText = "请组合按键";
            this.txtKeyCombination.ReadOnly = true;
            this.txtKeyCombination.Size = new System.Drawing.Size(400, 42);
            this.txtKeyCombination.TabIndex = 2;
            // 
            // pKeyBoard
            // 
            this.pKeyBoard.BorderWidth = 2F;
            this.pKeyBoard.Controls.Add(this.tlpKey);
            this.pKeyBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pKeyBoard.Location = new System.Drawing.Point(3, 3);
            this.pKeyBoard.Name = "pKeyBoard";
            this.pKeyBoard.Radius = 10;
            this.pKeyBoard.Size = new System.Drawing.Size(458, 166);
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
            this.tlpKey.Size = new System.Drawing.Size(454, 162);
            this.tlpKey.TabIndex = 0;
            // 
            // tlpKey2
            // 
            this.tlpKey2.ColumnCount = 3;
            this.tlpKey2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpKey2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpKey2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpKey2.Controls.Add(this.bInsert_KeyBoard, 2, 0);
            this.tlpKey2.Controls.Add(this.lkey, 0, 0);
            this.tlpKey2.Controls.Add(this.lKeyType, 0, 1);
            this.tlpKey2.Controls.Add(this.txtKey, 1, 0);
            this.tlpKey2.Controls.Add(this.ddlKeyType, 1, 1);
            this.tlpKey2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpKey2.Location = new System.Drawing.Point(0, 31);
            this.tlpKey2.Margin = new System.Windows.Forms.Padding(0);
            this.tlpKey2.Name = "tlpKey2";
            this.tlpKey2.RowCount = 3;
            this.tlpKey2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpKey2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tlpKey2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpKey2.Size = new System.Drawing.Size(454, 100);
            this.tlpKey2.TabIndex = 3;
            // 
            // bInsert_KeyBoard
            // 
            this.bInsert_KeyBoard.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bInsert_KeyBoard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bInsert_KeyBoard.IconSvg = "ArrowRightOutlined";
            this.bInsert_KeyBoard.Location = new System.Drawing.Point(409, 3);
            this.bInsert_KeyBoard.Name = "bInsert_KeyBoard";
            this.bInsert_KeyBoard.Size = new System.Drawing.Size(42, 42);
            this.bInsert_KeyBoard.TabIndex = 1;
            this.bInsert_KeyBoard.Type = AntdUI.TTypeMini.Primary;
            // 
            // lkey
            // 
            this.lkey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lkey.Location = new System.Drawing.Point(3, 3);
            this.lkey.Name = "lkey";
            this.lkey.Size = new System.Drawing.Size(75, 42);
            this.lkey.TabIndex = 12;
            this.lkey.Text = "按键";
            this.lkey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lKeyType
            // 
            this.lKeyType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lKeyType.Location = new System.Drawing.Point(3, 51);
            this.lKeyType.Name = "lKeyType";
            this.lKeyType.Size = new System.Drawing.Size(75, 39);
            this.lKeyType.TabIndex = 13;
            this.lKeyType.Text = "类型";
            this.lKeyType.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtKey
            // 
            this.txtKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtKey.Location = new System.Drawing.Point(84, 3);
            this.txtKey.Name = "txtKey";
            this.txtKey.PlaceholderText = "请按键";
            this.txtKey.Size = new System.Drawing.Size(319, 42);
            this.txtKey.TabIndex = 14;
            // 
            // ddlKeyType
            // 
            this.ddlKeyType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddlKeyType.Location = new System.Drawing.Point(84, 51);
            this.ddlKeyType.Name = "ddlKeyType";
            this.ddlKeyType.PlaceholderText = "请选择";
            this.ddlKeyType.Size = new System.Drawing.Size(319, 39);
            this.ddlKeyType.TabIndex = 15;
            // 
            // dkey
            // 
            this.dkey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dkey.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dkey.Location = new System.Drawing.Point(3, 3);
            this.dkey.Name = "dkey";
            this.dkey.Orientation = AntdUI.TOrientation.Left;
            this.dkey.Size = new System.Drawing.Size(448, 25);
            this.dkey.TabIndex = 0;
            this.dkey.Text = "按键";
            // 
            // ciMouseINST
            // 
            this.ciMouseINST.Controls.Add(this.tlpMouseINST);
            this.ciMouseINST.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ciMouseINST.Location = new System.Drawing.Point(-464, -430);
            this.ciMouseINST.Name = "ciMouseINST";
            this.ciMouseINST.Size = new System.Drawing.Size(464, 430);
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
            this.tlpMouseINST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpMouseINST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tlpMouseINST.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tlpMouseINST.Size = new System.Drawing.Size(464, 430);
            this.tlpMouseINST.TabIndex = 2;
            // 
            // pMouseMove
            // 
            this.pMouseMove.BorderWidth = 2F;
            this.pMouseMove.Controls.Add(this.tlpMouseMove);
            this.pMouseMove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pMouseMove.Location = new System.Drawing.Point(3, 251);
            this.pMouseMove.Name = "pMouseMove";
            this.pMouseMove.Radius = 10;
            this.pMouseMove.Size = new System.Drawing.Size(458, 176);
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
            this.tlpMouseMove.Size = new System.Drawing.Size(454, 172);
            this.tlpMouseMove.TabIndex = 0;
            // 
            // divider8
            // 
            this.divider8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.divider8.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.divider8.Location = new System.Drawing.Point(3, 3);
            this.divider8.Name = "divider8";
            this.divider8.Orientation = AntdUI.TOrientation.Left;
            this.divider8.Size = new System.Drawing.Size(448, 25);
            this.divider8.TabIndex = 0;
            this.divider8.Text = "移动";
            // 
            // tlpMouseMove2
            // 
            this.tlpMouseMove2.ColumnCount = 3;
            this.tlpMouseMove2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMouseMove2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMouseMove2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMouseMove2.Controls.Add(this.nudMouseMove_X, 1, 1);
            this.tlpMouseMove2.Controls.Add(this.rbMoveBy, 1, 0);
            this.tlpMouseMove2.Controls.Add(this.lMouseMove_Y, 0, 2);
            this.tlpMouseMove2.Controls.Add(this.lMouseMove_X, 0, 1);
            this.tlpMouseMove2.Controls.Add(this.bInsert_MouseMove, 2, 0);
            this.tlpMouseMove2.Controls.Add(this.nudMouseMove_Y, 1, 2);
            this.tlpMouseMove2.Controls.Add(this.rbMoveTo, 0, 0);
            this.tlpMouseMove2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMouseMove2.Location = new System.Drawing.Point(0, 31);
            this.tlpMouseMove2.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMouseMove2.Name = "tlpMouseMove2";
            this.tlpMouseMove2.RowCount = 4;
            this.tlpMouseMove2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMouseMove2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tlpMouseMove2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tlpMouseMove2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMouseMove2.Size = new System.Drawing.Size(454, 140);
            this.tlpMouseMove2.TabIndex = 1;
            // 
            // nudMouseMove_X
            // 
            this.nudMouseMove_X.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudMouseMove_X.Location = new System.Drawing.Point(87, 51);
            this.nudMouseMove_X.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudMouseMove_X.Name = "nudMouseMove_X";
            this.nudMouseMove_X.SelectionStart = 1;
            this.nudMouseMove_X.Size = new System.Drawing.Size(316, 39);
            this.nudMouseMove_X.SuffixText = "值";
            this.nudMouseMove_X.TabIndex = 19;
            this.nudMouseMove_X.Text = "0";
            this.nudMouseMove_X.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // rbMoveBy
            // 
            this.rbMoveBy.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbMoveBy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbMoveBy.Location = new System.Drawing.Point(87, 3);
            this.rbMoveBy.Name = "rbMoveBy";
            this.rbMoveBy.Size = new System.Drawing.Size(92, 38);
            this.rbMoveBy.TabIndex = 18;
            this.rbMoveBy.Text = "相对移动";
            // 
            // lMouseMove_Y
            // 
            this.lMouseMove_Y.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lMouseMove_Y.Location = new System.Drawing.Point(3, 96);
            this.lMouseMove_Y.Name = "lMouseMove_Y";
            this.lMouseMove_Y.Size = new System.Drawing.Size(78, 39);
            this.lMouseMove_Y.TabIndex = 15;
            this.lMouseMove_Y.Text = "Y坐标";
            this.lMouseMove_Y.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lMouseMove_X
            // 
            this.lMouseMove_X.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lMouseMove_X.Location = new System.Drawing.Point(3, 51);
            this.lMouseMove_X.Name = "lMouseMove_X";
            this.lMouseMove_X.Size = new System.Drawing.Size(78, 39);
            this.lMouseMove_X.TabIndex = 13;
            this.lMouseMove_X.Text = "X坐标";
            this.lMouseMove_X.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bInsert_MouseMove
            // 
            this.bInsert_MouseMove.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bInsert_MouseMove.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bInsert_MouseMove.IconSvg = "ArrowRightOutlined";
            this.bInsert_MouseMove.Location = new System.Drawing.Point(409, 3);
            this.bInsert_MouseMove.Name = "bInsert_MouseMove";
            this.bInsert_MouseMove.Size = new System.Drawing.Size(42, 42);
            this.bInsert_MouseMove.TabIndex = 1;
            this.bInsert_MouseMove.Type = AntdUI.TTypeMini.Primary;
            // 
            // nudMouseMove_Y
            // 
            this.nudMouseMove_Y.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudMouseMove_Y.Location = new System.Drawing.Point(87, 96);
            this.nudMouseMove_Y.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudMouseMove_Y.Name = "nudMouseMove_Y";
            this.nudMouseMove_Y.SelectionStart = 1;
            this.nudMouseMove_Y.Size = new System.Drawing.Size(316, 39);
            this.nudMouseMove_Y.SuffixText = "值";
            this.nudMouseMove_Y.TabIndex = 16;
            this.nudMouseMove_Y.Text = "0";
            this.nudMouseMove_Y.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // rbMoveTo
            // 
            this.rbMoveTo.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbMoveTo.Checked = true;
            this.rbMoveTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbMoveTo.Location = new System.Drawing.Point(3, 3);
            this.rbMoveTo.Name = "rbMoveTo";
            this.rbMoveTo.Size = new System.Drawing.Size(78, 38);
            this.rbMoveTo.TabIndex = 17;
            this.rbMoveTo.Text = "移动到";
            // 
            // pMouseWheel
            // 
            this.pMouseWheel.BorderWidth = 2F;
            this.pMouseWheel.Controls.Add(this.tlpMouseWheel);
            this.pMouseWheel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pMouseWheel.Location = new System.Drawing.Point(3, 103);
            this.pMouseWheel.Name = "pMouseWheel";
            this.pMouseWheel.Radius = 10;
            this.pMouseWheel.Size = new System.Drawing.Size(458, 142);
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
            this.tlpMouseWheel.Size = new System.Drawing.Size(454, 138);
            this.tlpMouseWheel.TabIndex = 0;
            // 
            // dMouseWheel
            // 
            this.dMouseWheel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dMouseWheel.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dMouseWheel.Location = new System.Drawing.Point(3, 3);
            this.dMouseWheel.Name = "dMouseWheel";
            this.dMouseWheel.Orientation = AntdUI.TOrientation.Left;
            this.dMouseWheel.Size = new System.Drawing.Size(448, 25);
            this.dMouseWheel.TabIndex = 0;
            this.dMouseWheel.Text = "滚轮";
            // 
            // tlpMouseWheel2
            // 
            this.tlpMouseWheel2.ColumnCount = 3;
            this.tlpMouseWheel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMouseWheel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMouseWheel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMouseWheel2.Controls.Add(this.lWheelDistance, 0, 1);
            this.tlpMouseWheel2.Controls.Add(this.ddlMouseWheel, 1, 0);
            this.tlpMouseWheel2.Controls.Add(this.lMouseWheel, 0, 0);
            this.tlpMouseWheel2.Controls.Add(this.bInsert_MouseWheel, 2, 0);
            this.tlpMouseWheel2.Controls.Add(this.nudWheelDistance, 1, 1);
            this.tlpMouseWheel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMouseWheel2.Location = new System.Drawing.Point(0, 31);
            this.tlpMouseWheel2.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMouseWheel2.Name = "tlpMouseWheel2";
            this.tlpMouseWheel2.RowCount = 3;
            this.tlpMouseWheel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMouseWheel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tlpMouseWheel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMouseWheel2.Size = new System.Drawing.Size(454, 100);
            this.tlpMouseWheel2.TabIndex = 1;
            // 
            // lWheelDistance
            // 
            this.lWheelDistance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lWheelDistance.Location = new System.Drawing.Point(3, 51);
            this.lWheelDistance.Name = "lWheelDistance";
            this.lWheelDistance.Size = new System.Drawing.Size(75, 39);
            this.lWheelDistance.TabIndex = 15;
            this.lWheelDistance.Text = "距离";
            this.lWheelDistance.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ddlMouseWheel
            // 
            this.ddlMouseWheel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddlMouseWheel.Location = new System.Drawing.Point(84, 3);
            this.ddlMouseWheel.Name = "ddlMouseWheel";
            this.ddlMouseWheel.PlaceholderText = "请选择";
            this.ddlMouseWheel.Size = new System.Drawing.Size(319, 42);
            this.ddlMouseWheel.TabIndex = 14;
            // 
            // lMouseWheel
            // 
            this.lMouseWheel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lMouseWheel.Location = new System.Drawing.Point(3, 3);
            this.lMouseWheel.Name = "lMouseWheel";
            this.lMouseWheel.Size = new System.Drawing.Size(75, 42);
            this.lMouseWheel.TabIndex = 13;
            this.lMouseWheel.Text = "滚动";
            this.lMouseWheel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // bInsert_MouseWheel
            // 
            this.bInsert_MouseWheel.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bInsert_MouseWheel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bInsert_MouseWheel.IconSvg = "ArrowRightOutlined";
            this.bInsert_MouseWheel.Location = new System.Drawing.Point(409, 3);
            this.bInsert_MouseWheel.Name = "bInsert_MouseWheel";
            this.bInsert_MouseWheel.Size = new System.Drawing.Size(42, 42);
            this.bInsert_MouseWheel.TabIndex = 1;
            this.bInsert_MouseWheel.Type = AntdUI.TTypeMini.Primary;
            // 
            // nudWheelDistance
            // 
            this.nudWheelDistance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudWheelDistance.Location = new System.Drawing.Point(84, 51);
            this.nudWheelDistance.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudWheelDistance.Name = "nudWheelDistance";
            this.nudWheelDistance.SelectionStart = 2;
            this.nudWheelDistance.Size = new System.Drawing.Size(319, 39);
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
            this.pMouseKey.Size = new System.Drawing.Size(458, 94);
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
            this.tlpMouseKey.Size = new System.Drawing.Size(454, 90);
            this.tlpMouseKey.TabIndex = 0;
            // 
            // tlpMouseKey2
            // 
            this.tlpMouseKey2.ColumnCount = 3;
            this.tlpMouseKey2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMouseKey2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMouseKey2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpMouseKey2.Controls.Add(this.bInsert_MouseKey, 2, 0);
            this.tlpMouseKey2.Controls.Add(this.lMouseKey, 0, 0);
            this.tlpMouseKey2.Controls.Add(this.ddlMouseKey, 1, 0);
            this.tlpMouseKey2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMouseKey2.Location = new System.Drawing.Point(0, 31);
            this.tlpMouseKey2.Margin = new System.Windows.Forms.Padding(0);
            this.tlpMouseKey2.Name = "tlpMouseKey2";
            this.tlpMouseKey2.RowCount = 2;
            this.tlpMouseKey2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpMouseKey2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMouseKey2.Size = new System.Drawing.Size(454, 50);
            this.tlpMouseKey2.TabIndex = 3;
            // 
            // bInsert_MouseKey
            // 
            this.bInsert_MouseKey.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.bInsert_MouseKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bInsert_MouseKey.IconSvg = "ArrowRightOutlined";
            this.bInsert_MouseKey.Location = new System.Drawing.Point(409, 3);
            this.bInsert_MouseKey.Name = "bInsert_MouseKey";
            this.bInsert_MouseKey.Size = new System.Drawing.Size(42, 42);
            this.bInsert_MouseKey.TabIndex = 1;
            this.bInsert_MouseKey.Type = AntdUI.TTypeMini.Primary;
            // 
            // lMouseKey
            // 
            this.lMouseKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lMouseKey.Location = new System.Drawing.Point(3, 3);
            this.lMouseKey.Name = "lMouseKey";
            this.lMouseKey.Size = new System.Drawing.Size(75, 42);
            this.lMouseKey.TabIndex = 12;
            this.lMouseKey.Text = "按键";
            this.lMouseKey.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ddlMouseKey
            // 
            this.ddlMouseKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddlMouseKey.Location = new System.Drawing.Point(84, 3);
            this.ddlMouseKey.Name = "ddlMouseKey";
            this.ddlMouseKey.PlaceholderText = "请选择";
            this.ddlMouseKey.Size = new System.Drawing.Size(319, 42);
            this.ddlMouseKey.TabIndex = 13;
            // 
            // dMouseKey
            // 
            this.dMouseKey.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dMouseKey.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dMouseKey.Location = new System.Drawing.Point(3, 3);
            this.dMouseKey.Name = "dMouseKey";
            this.dMouseKey.Orientation = AntdUI.TOrientation.Left;
            this.dMouseKey.Size = new System.Drawing.Size(448, 25);
            this.dMouseKey.TabIndex = 0;
            this.dMouseKey.Text = "按键";
            // 
            // pRobotINST
            // 
            this.pRobotINST.BorderWidth = 1F;
            this.pRobotINST.Controls.Add(this.tRobotInstruction);
            this.pRobotINST.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pRobotINST.Font = new System.Drawing.Font("微软雅黑", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pRobotINST.Location = new System.Drawing.Point(495, 3);
            this.pRobotINST.Name = "pRobotINST";
            this.pRobotINST.Padding = new System.Windows.Forms.Padding(3);
            this.pRobotINST.Size = new System.Drawing.Size(486, 645);
            this.pRobotINST.TabIndex = 8;
            this.pRobotINST.Text = "panel1";
            // 
            // tRobotInstruction
            // 
            this.tRobotInstruction.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tRobotInstruction.CellImpactHeight = false;
            this.tRobotInstruction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tRobotInstruction.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tRobotInstruction.Gap = 8;
            this.tRobotInstruction.GapCell = 0;
            this.tRobotInstruction.Location = new System.Drawing.Point(4, 4);
            this.tRobotInstruction.Name = "tRobotInstruction";
            this.tRobotInstruction.Size = new System.Drawing.Size(478, 637);
            this.tRobotInstruction.TabIndex = 3;
            this.tRobotInstruction.VisibleHeader = false;
            this.tRobotInstruction.CellClick += new AntdUI.Table.ClickEventHandler(this.tRobotINST_CellClick);
            // 
            // txtINSTLog
            // 
            this.txtINSTLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtINSTLog.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(119)))), ((int)(((byte)(255)))));
            this.txtINSTLog.Location = new System.Drawing.Point(495, 654);
            this.txtINSTLog.Name = "txtINSTLog";
            this.txtINSTLog.PlaceholderText = "";
            this.txtINSTLog.PrefixText = "运行记录:";
            this.txtINSTLog.ReadOnly = true;
            this.txtINSTLog.Size = new System.Drawing.Size(486, 44);
            this.txtINSTLog.TabIndex = 7;
            this.txtINSTLog.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtRobotName
            // 
            this.txtRobotName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtRobotName.Location = new System.Drawing.Point(3, 654);
            this.txtRobotName.Name = "txtRobotName";
            this.txtRobotName.PlaceholderText = "请输入名称";
            this.txtRobotName.PrefixText = "名称:";
            this.txtRobotName.Size = new System.Drawing.Size(486, 44);
            this.txtRobotName.TabIndex = 6;
            this.txtRobotName.TextChanged += new System.EventHandler(this.txtRobotName_TextChanged);
            // 
            // RobotEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 761);
            this.Controls.Add(this.tlpRobotEdit);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "RobotEditForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "RobotEditForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RobotEditForm_FormClosing);
            this.Load += new System.EventHandler(this.RobotEditForm_Load);
            this.tlpRobotEdit.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
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

        private System.Windows.Forms.TableLayoutPanel tlpRobotEdit;
        private System.Windows.Forms.TableLayoutPanel tlpRobotINST;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private AntdUI.Button bStop;
        private AntdUI.Button bExecute;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private AntdUI.Panel pRobotINST;
        private AntdUI.Table tRobotInstruction;
        private AntdUI.Input txtINSTLog;
        private AntdUI.Input txtRobotName;
        private AntdUI.Collapse cRobotINST;
        private AntdUI.CollapseItem ciPacketINST;
        private System.Windows.Forms.TableLayoutPanel tlpPacketINST;
        private AntdUI.Panel pSYSSocket;
        private System.Windows.Forms.TableLayoutPanel tlpSYSSocket2;
        private System.Windows.Forms.TableLayoutPanel tlpSYSSocket;
        private AntdUI.Label lSelectFilter;
        private AntdUI.Radio rbSelectSocket;
        private AntdUI.Radio rbSelectFilter;
        private AntdUI.Button bInsert_SYSSocket;
        private AntdUI.Radio rbSelectPacket;
        private AntdUI.Label lSelectPacket;
        private AntdUI.InputNumber nudSelectSocket;
        private AntdUI.Divider dSYSSocket;
        private AntdUI.Panel pPacketList;
        private System.Windows.Forms.TableLayoutPanel tlpPacketList2;
        private System.Windows.Forms.TableLayoutPanel tlpPacketList;
        private AntdUI.Button bInsert_PacketList;
        private AntdUI.Label lPacketList;
        private AntdUI.Divider dPacketList;
        private AntdUI.Panel pSendList;
        private System.Windows.Forms.TableLayoutPanel tlpSendList2;
        private AntdUI.Divider dSendList;
        private System.Windows.Forms.TableLayoutPanel tlpSendList;
        private AntdUI.Select ddlSendList;
        private AntdUI.Button bInsert_SendList;
        private AntdUI.CollapseItem ciControlINST;
        private System.Windows.Forms.TableLayoutPanel tlpControlINST;
        private AntdUI.Panel pLoop;
        private System.Windows.Forms.TableLayoutPanel tlpLoop;
        private AntdUI.Divider dLoop;
        private System.Windows.Forms.TableLayoutPanel tlpLoop2;
        private AntdUI.InputNumber nudLoop;
        private AntdUI.Button bInsert_LoopStart;
        private AntdUI.Button bInsert_LoopEnd;
        private AntdUI.Panel pDelay;
        private System.Windows.Forms.TableLayoutPanel tlpDelay;
        private System.Windows.Forms.TableLayoutPanel tlpDelay2;
        private AntdUI.InputNumber nudnudDelayRandom_To;
        private AntdUI.InputNumber nudDelayFix;
        private AntdUI.Radio rbDelayRandom;
        private AntdUI.Button bInsert_Delay;
        private AntdUI.Radio rbDelayFix;
        private AntdUI.InputNumber nudnudDelayRandom_From;
        private AntdUI.Divider dDelay;
        private AntdUI.CollapseItem ciKeyBoardINST;
        private System.Windows.Forms.TableLayoutPanel tlpKeyboardINST;
        private AntdUI.Panel pText;
        private System.Windows.Forms.TableLayoutPanel tlpText;
        private AntdUI.Divider dText;
        private System.Windows.Forms.TableLayoutPanel tlpText2;
        private AntdUI.Button bInsert_Text;
        private AntdUI.Input txtText;
        private AntdUI.Panel pKeyCombination;
        private System.Windows.Forms.TableLayoutPanel tlpKeyCombination;
        private AntdUI.Divider dKeyCombination;
        private System.Windows.Forms.TableLayoutPanel tlpKeyCombination2;
        private AntdUI.Button bInsert_KeyCombination;
        private Lib.Controls.HotkeyTextBox txtKeyCombination;
        private AntdUI.Panel pKeyBoard;
        private System.Windows.Forms.TableLayoutPanel tlpKey;
        private System.Windows.Forms.TableLayoutPanel tlpKey2;
        private AntdUI.Button bInsert_KeyBoard;
        private AntdUI.Label lkey;
        private AntdUI.Label lKeyType;
        private AntdUI.Input txtKey;
        private AntdUI.Select ddlKeyType;
        private AntdUI.Divider dkey;
        private AntdUI.CollapseItem ciMouseINST;
        private System.Windows.Forms.TableLayoutPanel tlpMouseINST;
        private AntdUI.Panel pMouseMove;
        private System.Windows.Forms.TableLayoutPanel tlpMouseMove;
        private AntdUI.Divider divider8;
        private System.Windows.Forms.TableLayoutPanel tlpMouseMove2;
        private AntdUI.InputNumber nudMouseMove_X;
        private AntdUI.Radio rbMoveBy;
        private AntdUI.Label lMouseMove_Y;
        private AntdUI.Label lMouseMove_X;
        private AntdUI.Button bInsert_MouseMove;
        private AntdUI.InputNumber nudMouseMove_Y;
        private AntdUI.Radio rbMoveTo;
        private AntdUI.Panel pMouseWheel;
        private System.Windows.Forms.TableLayoutPanel tlpMouseWheel;
        private AntdUI.Divider dMouseWheel;
        private System.Windows.Forms.TableLayoutPanel tlpMouseWheel2;
        private AntdUI.Label lWheelDistance;
        private AntdUI.Select ddlMouseWheel;
        private AntdUI.Label lMouseWheel;
        private AntdUI.Button bInsert_MouseWheel;
        private AntdUI.InputNumber nudWheelDistance;
        private AntdUI.Panel pMouseKey;
        private System.Windows.Forms.TableLayoutPanel tlpMouseKey;
        private System.Windows.Forms.TableLayoutPanel tlpMouseKey2;
        private AntdUI.Button bInsert_MouseKey;
        private AntdUI.Label lMouseKey;
        private AntdUI.Select ddlMouseKey;
        private AntdUI.Divider dMouseKey;
    }
}