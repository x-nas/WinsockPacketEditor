namespace WinsockPacketEditor
{
    partial class PacketEdit
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
            this.tlpPacketEdit = new TableLayoutPanelEx();
            this.pPacketData = new AntdUI.Panel();
            this.hbPacketEdit = new Be.Windows.Forms.HexBox();
            this.tlpSendCollectionInfo = new TableLayoutPanelEx();
            this.lSend_Fail_CNT = new AntdUI.Label();
            this.lSend_Success_CNT = new AntdUI.Label();
            this.lTotal_Send_CNT = new AntdUI.Label();
            this.lSplit2 = new AntdUI.Label();
            this.lSplit1 = new AntdUI.Label();
            this.lSend_Fail = new AntdUI.Label();
            this.lSend_Success = new AntdUI.Label();
            this.lTotal_Send = new AntdUI.Label();
            this.tlpButton = new TableLayoutPanelEx();
            this.bStop = new AntdUI.Button();
            this.bSend = new AntdUI.Button();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tlpPacketSettings = new TableLayoutPanelEx();
            this.pProgression = new AntdUI.Panel();
            this.tlpProgression = new TableLayoutPanelEx();
            this.tableLayoutPanel4 = new TableLayoutPanelEx();
            this.nudProgressionCarry = new AntdUI.InputNumber();
            this.cbProgressionCarry = new AntdUI.Checkbox();
            this.cbProgressionPosition = new AntdUI.Checkbox();
            this.nudProgressionPosition = new AntdUI.InputNumber();
            this.nudProgressionStep = new AntdUI.InputNumber();
            this.dProgression = new AntdUI.Divider();
            this.pPacketSend = new AntdUI.Panel();
            this.tlpPacketSend = new TableLayoutPanelEx();
            this.tableLayoutPanel2 = new TableLayoutPanelEx();
            this.rbSendType_Continuously = new AntdUI.Radio();
            this.nudSendType_Times = new AntdUI.InputNumber();
            this.nudSendType_Interval = new AntdUI.InputNumber();
            this.rbSendType_Times = new AntdUI.Radio();
            this.dPacketSend = new AntdUI.Divider();
            this.pPacketSocket = new AntdUI.Panel();
            this.tlpPacketSocket = new TableLayoutPanelEx();
            this.tlpFilterProgression = new TableLayoutPanelEx();
            this.nudPacketLength = new AntdUI.InputNumber();
            this.lPacketLength = new AntdUI.Label();
            this.lPacketTo = new AntdUI.Label();
            this.nudPacketSocket = new AntdUI.InputNumber();
            this.lPacketSocket = new AntdUI.Label();
            this.txtPacketTo = new AntdUI.Input();
            this.dPacketSocket = new AntdUI.Divider();
            this.bgwSendPacket = new System.ComponentModel.BackgroundWorker();
            this.tlpPacketEdit.SuspendLayout();
            this.pPacketData.SuspendLayout();
            this.tlpSendCollectionInfo.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpPacketSettings.SuspendLayout();
            this.pProgression.SuspendLayout();
            this.tlpProgression.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.pPacketSend.SuspendLayout();
            this.tlpPacketSend.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.pPacketSocket.SuspendLayout();
            this.tlpPacketSocket.SuspendLayout();
            this.tlpFilterProgression.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpPacketEdit
            // 
            this.tlpPacketEdit.ColumnCount = 1;
            this.tlpPacketEdit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketEdit.Controls.Add(this.pPacketData, 0, 1);
            this.tlpPacketEdit.Controls.Add(this.tlpSendCollectionInfo, 0, 0);
            this.tlpPacketEdit.Controls.Add(this.tlpButton, 0, 3);
            this.tlpPacketEdit.Controls.Add(this.tlpPacketSettings, 0, 2);
            this.tlpPacketEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPacketEdit.Location = new System.Drawing.Point(0, 0);
            this.tlpPacketEdit.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPacketEdit.Name = "tlpPacketEdit";
            this.tlpPacketEdit.RowCount = 4;
            this.tlpPacketEdit.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpPacketEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            this.tlpPacketEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpPacketEdit.Size = new System.Drawing.Size(1000, 700);
            this.tlpPacketEdit.TabIndex = 1;
            // 
            // pPacketData
            // 
            this.pPacketData.BorderWidth = 2F;
            this.pPacketData.Controls.Add(this.hbPacketEdit);
            this.pPacketData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pPacketData.Location = new System.Drawing.Point(3, 48);
            this.pPacketData.Name = "pPacketData";
            this.pPacketData.Padding = new System.Windows.Forms.Padding(3);
            this.pPacketData.Radius = 10;
            this.pPacketData.Size = new System.Drawing.Size(994, 389);
            this.pPacketData.TabIndex = 49;
            this.pPacketData.Text = "panel3";
            // 
            // hbPacketEdit
            // 
            this.hbPacketEdit.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.hbPacketEdit.ColumnInfoVisible = true;
            this.hbPacketEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hbPacketEdit.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hbPacketEdit.LineInfoVisible = true;
            this.hbPacketEdit.Location = new System.Drawing.Point(5, 5);
            this.hbPacketEdit.Name = "hbPacketEdit";
            this.hbPacketEdit.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hbPacketEdit.Size = new System.Drawing.Size(984, 379);
            this.hbPacketEdit.StringViewVisible = true;
            this.hbPacketEdit.TabIndex = 1;
            this.hbPacketEdit.VScrollBarVisible = true;
            this.hbPacketEdit.CurrentPositionInLineChanged += new System.EventHandler(this.hbPacketEdit_CurrentPositionInLineChanged);
            this.hbPacketEdit.MouseDown += new System.Windows.Forms.MouseEventHandler(this.hbPacketEdit_MouseDown);
            // 
            // tlpSendCollectionInfo
            // 
            this.tlpSendCollectionInfo.ColumnCount = 9;
            this.tlpSendCollectionInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSendCollectionInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSendCollectionInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSendCollectionInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSendCollectionInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSendCollectionInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSendCollectionInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSendCollectionInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSendCollectionInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSendCollectionInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpSendCollectionInfo.Controls.Add(this.lSend_Fail_CNT, 7, 0);
            this.tlpSendCollectionInfo.Controls.Add(this.lSend_Success_CNT, 4, 0);
            this.tlpSendCollectionInfo.Controls.Add(this.lTotal_Send_CNT, 1, 0);
            this.tlpSendCollectionInfo.Controls.Add(this.lSplit2, 5, 0);
            this.tlpSendCollectionInfo.Controls.Add(this.lSplit1, 2, 0);
            this.tlpSendCollectionInfo.Controls.Add(this.lSend_Fail, 6, 0);
            this.tlpSendCollectionInfo.Controls.Add(this.lSend_Success, 3, 0);
            this.tlpSendCollectionInfo.Controls.Add(this.lTotal_Send, 0, 0);
            this.tlpSendCollectionInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSendCollectionInfo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tlpSendCollectionInfo.Location = new System.Drawing.Point(0, 0);
            this.tlpSendCollectionInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSendCollectionInfo.Name = "tlpSendCollectionInfo";
            this.tlpSendCollectionInfo.RowCount = 1;
            this.tlpSendCollectionInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSendCollectionInfo.Size = new System.Drawing.Size(1000, 45);
            this.tlpSendCollectionInfo.TabIndex = 16;
            // 
            // lSend_Fail_CNT
            // 
            this.lSend_Fail_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSend_Fail_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSend_Fail_CNT.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSend_Fail_CNT.ForeColor = System.Drawing.Color.Red;
            this.lSend_Fail_CNT.Location = new System.Drawing.Point(232, 3);
            this.lSend_Fail_CNT.Name = "lSend_Fail_CNT";
            this.lSend_Fail_CNT.Size = new System.Drawing.Size(10, 39);
            this.lSend_Fail_CNT.TabIndex = 14;
            this.lSend_Fail_CNT.Text = "0";
            // 
            // lSend_Success_CNT
            // 
            this.lSend_Success_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSend_Success_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSend_Success_CNT.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSend_Success_CNT.ForeColor = System.Drawing.Color.Green;
            this.lSend_Success_CNT.Location = new System.Drawing.Point(157, 3);
            this.lSend_Success_CNT.Name = "lSend_Success_CNT";
            this.lSend_Success_CNT.Size = new System.Drawing.Size(10, 39);
            this.lSend_Success_CNT.TabIndex = 13;
            this.lSend_Success_CNT.Text = "0";
            // 
            // lTotal_Send_CNT
            // 
            this.lTotal_Send_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lTotal_Send_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lTotal_Send_CNT.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lTotal_Send_CNT.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(119)))), ((int)(((byte)(255)))));
            this.lTotal_Send_CNT.Location = new System.Drawing.Point(82, 3);
            this.lTotal_Send_CNT.Name = "lTotal_Send_CNT";
            this.lTotal_Send_CNT.Size = new System.Drawing.Size(10, 39);
            this.lTotal_Send_CNT.TabIndex = 12;
            this.lTotal_Send_CNT.Text = "0";
            // 
            // lSplit2
            // 
            this.lSplit2.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSplit2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSplit2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSplit2.ForeColor = System.Drawing.Color.Silver;
            this.lSplit2.Location = new System.Drawing.Point(173, 3);
            this.lSplit2.Name = "lSplit2";
            this.lSplit2.Size = new System.Drawing.Size(6, 39);
            this.lSplit2.TabIndex = 9;
            this.lSplit2.Text = "|";
            this.lSplit2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lSplit1
            // 
            this.lSplit1.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSplit1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSplit1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSplit1.ForeColor = System.Drawing.Color.Silver;
            this.lSplit1.Location = new System.Drawing.Point(98, 3);
            this.lSplit1.Name = "lSplit1";
            this.lSplit1.Size = new System.Drawing.Size(6, 39);
            this.lSplit1.TabIndex = 8;
            this.lSplit1.Text = "|";
            this.lSplit1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lSend_Fail
            // 
            this.lSend_Fail.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSend_Fail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSend_Fail.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSend_Fail.LocalizationText = "SendEditForm.SendFailure";
            this.lSend_Fail.Location = new System.Drawing.Point(185, 3);
            this.lSend_Fail.Name = "lSend_Fail";
            this.lSend_Fail.Size = new System.Drawing.Size(41, 39);
            this.lSend_Fail.TabIndex = 7;
            this.lSend_Fail.Text = "失败 :";
            // 
            // lSend_Success
            // 
            this.lSend_Success.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSend_Success.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSend_Success.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSend_Success.LocalizationText = "SendEditForm.SendSuccess";
            this.lSend_Success.Location = new System.Drawing.Point(110, 3);
            this.lSend_Success.Name = "lSend_Success";
            this.lSend_Success.Size = new System.Drawing.Size(41, 39);
            this.lSend_Success.TabIndex = 6;
            this.lSend_Success.Text = "成功 :";
            // 
            // lTotal_Send
            // 
            this.lTotal_Send.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lTotal_Send.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lTotal_Send.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lTotal_Send.LocalizationText = "SendEditForm.TotalSend";
            this.lTotal_Send.Location = new System.Drawing.Point(3, 3);
            this.lTotal_Send.Name = "lTotal_Send";
            this.lTotal_Send.Size = new System.Drawing.Size(73, 39);
            this.lTotal_Send.TabIndex = 5;
            this.lTotal_Send.Text = "发送总数 :";
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
            this.tlpButton.Controls.Add(this.bSend, 1, 1);
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
            this.tlpButton.Size = new System.Drawing.Size(1000, 60);
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
            this.bStop.Location = new System.Drawing.Point(400, 7);
            this.bStop.Name = "bStop";
            this.bStop.Size = new System.Drawing.Size(87, 46);
            this.bStop.TabIndex = 3;
            this.bStop.Text = "停止";
            this.bStop.Type = AntdUI.TTypeMini.Info;
            this.bStop.Click += new System.EventHandler(this.bStop_Click);
            // 
            // bSend
            // 
            this.bSend.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bSend.BackExtend = "135, #6253E1, #04BEFE";
            this.bSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSend.IconSvg = "PlayCircleOutlined";
            this.bSend.LoadingWaveColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(87)))), ((int)(((byte)(34)))));
            this.bSend.LoadingWaveCount = 6;
            this.bSend.LoadingWaveSize = 6;
            this.bSend.LoadingWaveValue = 0.6F;
            this.bSend.LoadingWaveVertical = true;
            this.bSend.LocalizationText = "Send";
            this.bSend.Location = new System.Drawing.Point(287, 7);
            this.bSend.Name = "bSend";
            this.bSend.Size = new System.Drawing.Size(87, 46);
            this.bSend.TabIndex = 2;
            this.bSend.Text = "发送";
            this.bSend.Type = AntdUI.TTypeMini.Info;
            this.bSend.Click += new System.EventHandler(this.bSend_Click);
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
            this.bSave.Location = new System.Drawing.Point(513, 7);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(87, 46);
            this.bSave.TabIndex = 0;
            this.bSave.Text = "保存";
            this.bSave.Type = AntdUI.TTypeMini.Info;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bExit
            // 
            this.bExit.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.bExit.BackExtend = "135, #6253E1, #04BEFE";
            this.bExit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bExit.IconSvg = "LogoutOutlined";
            this.bExit.LocalizationText = "Cancel";
            this.bExit.Location = new System.Drawing.Point(626, 7);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(87, 46);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // tlpPacketSettings
            // 
            this.tlpPacketSettings.ColumnCount = 3;
            this.tlpPacketSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tlpPacketSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tlpPacketSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tlpPacketSettings.Controls.Add(this.pProgression, 2, 0);
            this.tlpPacketSettings.Controls.Add(this.pPacketSend, 1, 0);
            this.tlpPacketSettings.Controls.Add(this.pPacketSocket, 0, 0);
            this.tlpPacketSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPacketSettings.Location = new System.Drawing.Point(0, 440);
            this.tlpPacketSettings.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPacketSettings.Name = "tlpPacketSettings";
            this.tlpPacketSettings.RowCount = 1;
            this.tlpPacketSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketSettings.Size = new System.Drawing.Size(1000, 200);
            this.tlpPacketSettings.TabIndex = 13;
            // 
            // pProgression
            // 
            this.pProgression.BorderWidth = 2F;
            this.pProgression.Controls.Add(this.tlpProgression);
            this.pProgression.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pProgression.Location = new System.Drawing.Point(663, 3);
            this.pProgression.Name = "pProgression";
            this.pProgression.Radius = 10;
            this.pProgression.Size = new System.Drawing.Size(334, 194);
            this.pProgression.TabIndex = 13;
            // 
            // tlpProgression
            // 
            this.tlpProgression.BackColor = System.Drawing.Color.Transparent;
            this.tlpProgression.ColumnCount = 1;
            this.tlpProgression.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProgression.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tlpProgression.Controls.Add(this.dProgression, 0, 0);
            this.tlpProgression.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpProgression.Location = new System.Drawing.Point(2, 2);
            this.tlpProgression.Margin = new System.Windows.Forms.Padding(0);
            this.tlpProgression.Name = "tlpProgression";
            this.tlpProgression.RowCount = 2;
            this.tlpProgression.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpProgression.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpProgression.Size = new System.Drawing.Size(330, 190);
            this.tlpProgression.TabIndex = 0;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Controls.Add(this.nudProgressionCarry, 1, 2);
            this.tableLayoutPanel4.Controls.Add(this.cbProgressionCarry, 0, 2);
            this.tableLayoutPanel4.Controls.Add(this.cbProgressionPosition, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.nudProgressionPosition, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.nudProgressionStep, 1, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 36);
            this.tableLayoutPanel4.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.Padding = new System.Windows.Forms.Padding(3);
            this.tableLayoutPanel4.RowCount = 4;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(330, 154);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // nudProgressionCarry
            // 
            this.nudProgressionCarry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudProgressionCarry.LocalizationSuffixText = "FilterEditForm.StartFrom.Progression.Digits";
            this.nudProgressionCarry.Location = new System.Drawing.Point(118, 102);
            this.nudProgressionCarry.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudProgressionCarry.Name = "nudProgressionCarry";
            this.nudProgressionCarry.SelectionStart = 1;
            this.nudProgressionCarry.Size = new System.Drawing.Size(206, 42);
            this.nudProgressionCarry.SuffixText = "位数";
            this.nudProgressionCarry.TabIndex = 45;
            this.nudProgressionCarry.Text = "1";
            this.nudProgressionCarry.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudProgressionCarry.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cbProgressionCarry
            // 
            this.cbProgressionCarry.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbProgressionCarry.LocalizationText = "FilterEditForm.StartFrom.Progression.Carry";
            this.cbProgressionCarry.Location = new System.Drawing.Point(6, 102);
            this.cbProgressionCarry.Name = "cbProgressionCarry";
            this.cbProgressionCarry.Size = new System.Drawing.Size(106, 42);
            this.cbProgressionCarry.TabIndex = 44;
            this.cbProgressionCarry.Text = "进位递进";
            this.cbProgressionCarry.CheckedChanged += new AntdUI.BoolEventHandler(this.cbProgressionCarry_CheckedChanged);
            // 
            // cbProgressionPosition
            // 
            this.cbProgressionPosition.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbProgressionPosition.LocalizationText = "Enable";
            this.cbProgressionPosition.Location = new System.Drawing.Point(6, 6);
            this.cbProgressionPosition.Name = "cbProgressionPosition";
            this.cbProgressionPosition.Size = new System.Drawing.Size(106, 42);
            this.cbProgressionPosition.TabIndex = 40;
            this.cbProgressionPosition.Text = "启用递进";
            this.cbProgressionPosition.CheckedChanged += new AntdUI.BoolEventHandler(this.cbProgressionPosition_CheckedChanged);
            // 
            // nudProgressionPosition
            // 
            this.nudProgressionPosition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudProgressionPosition.LocalizationSuffixText = "Position";
            this.nudProgressionPosition.Location = new System.Drawing.Point(118, 6);
            this.nudProgressionPosition.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudProgressionPosition.Name = "nudProgressionPosition";
            this.nudProgressionPosition.ReadOnly = true;
            this.nudProgressionPosition.Size = new System.Drawing.Size(206, 42);
            this.nudProgressionPosition.SuffixText = "位置";
            this.nudProgressionPosition.TabIndex = 42;
            this.nudProgressionPosition.Text = "1";
            this.nudProgressionPosition.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudProgressionPosition.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudProgressionStep
            // 
            this.nudProgressionStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudProgressionStep.LocalizationSuffixText = "FilterEditForm.StartFrom.Progression.Step";
            this.nudProgressionStep.Location = new System.Drawing.Point(118, 54);
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
            this.nudProgressionStep.Size = new System.Drawing.Size(206, 42);
            this.nudProgressionStep.SuffixText = "步长";
            this.nudProgressionStep.TabIndex = 43;
            this.nudProgressionStep.Text = "1";
            this.nudProgressionStep.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudProgressionStep.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // dProgression
            // 
            this.dProgression.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dProgression.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dProgression.LocalizationText = "FilterEditForm.Progression";
            this.dProgression.Location = new System.Drawing.Point(3, 3);
            this.dProgression.Name = "dProgression";
            this.dProgression.Orientation = AntdUI.TOrientation.Left;
            this.dProgression.Size = new System.Drawing.Size(324, 30);
            this.dProgression.TabIndex = 0;
            this.dProgression.Text = "递进";
            // 
            // pPacketSend
            // 
            this.pPacketSend.BorderWidth = 2F;
            this.pPacketSend.Controls.Add(this.tlpPacketSend);
            this.pPacketSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pPacketSend.Location = new System.Drawing.Point(333, 3);
            this.pPacketSend.Name = "pPacketSend";
            this.pPacketSend.Radius = 10;
            this.pPacketSend.Size = new System.Drawing.Size(324, 194);
            this.pPacketSend.TabIndex = 12;
            // 
            // tlpPacketSend
            // 
            this.tlpPacketSend.BackColor = System.Drawing.Color.Transparent;
            this.tlpPacketSend.ColumnCount = 1;
            this.tlpPacketSend.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketSend.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tlpPacketSend.Controls.Add(this.dPacketSend, 0, 0);
            this.tlpPacketSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPacketSend.Location = new System.Drawing.Point(2, 2);
            this.tlpPacketSend.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPacketSend.Name = "tlpPacketSend";
            this.tlpPacketSend.RowCount = 2;
            this.tlpPacketSend.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpPacketSend.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketSend.Size = new System.Drawing.Size(320, 190);
            this.tlpPacketSend.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.rbSendType_Continuously, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.nudSendType_Times, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.nudSendType_Interval, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.rbSendType_Times, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 36);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.Padding = new System.Windows.Forms.Padding(3);
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(320, 154);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // rbSendType_Continuously
            // 
            this.rbSendType_Continuously.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbSendType_Continuously.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbSendType_Continuously.LocalizationText = "PacketEditForm.Send.Continuously";
            this.rbSendType_Continuously.Location = new System.Drawing.Point(6, 54);
            this.rbSendType_Continuously.Name = "rbSendType_Continuously";
            this.rbSendType_Continuously.Size = new System.Drawing.Size(106, 42);
            this.rbSendType_Continuously.TabIndex = 45;
            this.rbSendType_Continuously.Text = "连续发送";
            // 
            // nudSendType_Times
            // 
            this.nudSendType_Times.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudSendType_Times.LocalizationSuffixText = "Times";
            this.nudSendType_Times.Location = new System.Drawing.Point(118, 6);
            this.nudSendType_Times.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSendType_Times.Name = "nudSendType_Times";
            this.nudSendType_Times.SelectionStart = 1;
            this.nudSendType_Times.Size = new System.Drawing.Size(196, 42);
            this.nudSendType_Times.SuffixText = "次数";
            this.nudSendType_Times.TabIndex = 42;
            this.nudSendType_Times.Text = "1";
            this.nudSendType_Times.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudSendType_Times.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudSendType_Interval
            // 
            this.nudSendType_Interval.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudSendType_Interval.LocalizationSuffixText = "Millisecond";
            this.nudSendType_Interval.Location = new System.Drawing.Point(118, 54);
            this.nudSendType_Interval.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudSendType_Interval.Name = "nudSendType_Interval";
            this.nudSendType_Interval.Size = new System.Drawing.Size(196, 42);
            this.nudSendType_Interval.SuffixText = "毫秒";
            this.nudSendType_Interval.TabIndex = 43;
            this.nudSendType_Interval.Text = "100";
            this.nudSendType_Interval.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudSendType_Interval.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // rbSendType_Times
            // 
            this.rbSendType_Times.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbSendType_Times.Checked = true;
            this.rbSendType_Times.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbSendType_Times.LocalizationText = "PacketEditForm.Send.ByTime";
            this.rbSendType_Times.Location = new System.Drawing.Point(6, 6);
            this.rbSendType_Times.Name = "rbSendType_Times";
            this.rbSendType_Times.Size = new System.Drawing.Size(106, 42);
            this.rbSendType_Times.TabIndex = 44;
            this.rbSendType_Times.Text = "按次发送";
            this.rbSendType_Times.CheckedChanged += new AntdUI.BoolEventHandler(this.rbSendType_Times_CheckedChanged);
            // 
            // dPacketSend
            // 
            this.dPacketSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dPacketSend.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dPacketSend.LocalizationText = "Send";
            this.dPacketSend.Location = new System.Drawing.Point(3, 3);
            this.dPacketSend.Name = "dPacketSend";
            this.dPacketSend.Orientation = AntdUI.TOrientation.Left;
            this.dPacketSend.Size = new System.Drawing.Size(314, 30);
            this.dPacketSend.TabIndex = 0;
            this.dPacketSend.Text = "发送";
            // 
            // pPacketSocket
            // 
            this.pPacketSocket.BorderWidth = 2F;
            this.pPacketSocket.Controls.Add(this.tlpPacketSocket);
            this.pPacketSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pPacketSocket.Location = new System.Drawing.Point(3, 3);
            this.pPacketSocket.Name = "pPacketSocket";
            this.pPacketSocket.Radius = 10;
            this.pPacketSocket.Size = new System.Drawing.Size(324, 194);
            this.pPacketSocket.TabIndex = 11;
            // 
            // tlpPacketSocket
            // 
            this.tlpPacketSocket.BackColor = System.Drawing.Color.Transparent;
            this.tlpPacketSocket.ColumnCount = 1;
            this.tlpPacketSocket.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketSocket.Controls.Add(this.tlpFilterProgression, 0, 1);
            this.tlpPacketSocket.Controls.Add(this.dPacketSocket, 0, 0);
            this.tlpPacketSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPacketSocket.Location = new System.Drawing.Point(2, 2);
            this.tlpPacketSocket.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPacketSocket.Name = "tlpPacketSocket";
            this.tlpPacketSocket.RowCount = 2;
            this.tlpPacketSocket.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpPacketSocket.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketSocket.Size = new System.Drawing.Size(320, 190);
            this.tlpPacketSocket.TabIndex = 0;
            // 
            // tlpFilterProgression
            // 
            this.tlpFilterProgression.ColumnCount = 2;
            this.tlpFilterProgression.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpFilterProgression.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterProgression.Controls.Add(this.nudPacketLength, 1, 2);
            this.tlpFilterProgression.Controls.Add(this.lPacketLength, 0, 2);
            this.tlpFilterProgression.Controls.Add(this.lPacketTo, 0, 1);
            this.tlpFilterProgression.Controls.Add(this.nudPacketSocket, 1, 0);
            this.tlpFilterProgression.Controls.Add(this.lPacketSocket, 0, 0);
            this.tlpFilterProgression.Controls.Add(this.txtPacketTo, 1, 1);
            this.tlpFilterProgression.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterProgression.Location = new System.Drawing.Point(0, 36);
            this.tlpFilterProgression.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFilterProgression.Name = "tlpFilterProgression";
            this.tlpFilterProgression.Padding = new System.Windows.Forms.Padding(3);
            this.tlpFilterProgression.RowCount = 4;
            this.tlpFilterProgression.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterProgression.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterProgression.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterProgression.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterProgression.Size = new System.Drawing.Size(320, 154);
            this.tlpFilterProgression.TabIndex = 1;
            // 
            // nudPacketLength
            // 
            this.nudPacketLength.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudPacketLength.Location = new System.Drawing.Point(101, 96);
            this.nudPacketLength.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudPacketLength.Name = "nudPacketLength";
            this.nudPacketLength.ReadOnly = true;
            this.nudPacketLength.Size = new System.Drawing.Size(213, 39);
            this.nudPacketLength.TabIndex = 48;
            this.nudPacketLength.Text = "1";
            this.nudPacketLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudPacketLength.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lPacketLength
            // 
            this.lPacketLength.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lPacketLength.Dock = System.Windows.Forms.DockStyle.Right;
            this.lPacketLength.LocalizationText = "PacketEditForm.Length";
            this.lPacketLength.Location = new System.Drawing.Point(54, 96);
            this.lPacketLength.Name = "lPacketLength";
            this.lPacketLength.Size = new System.Drawing.Size(41, 39);
            this.lPacketLength.TabIndex = 47;
            this.lPacketLength.Text = "长度 :";
            this.lPacketLength.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lPacketTo
            // 
            this.lPacketTo.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lPacketTo.Dock = System.Windows.Forms.DockStyle.Right;
            this.lPacketTo.LocalizationText = "PacketEditForm.ToAddr";
            this.lPacketTo.Location = new System.Drawing.Point(22, 51);
            this.lPacketTo.Name = "lPacketTo";
            this.lPacketTo.Size = new System.Drawing.Size(73, 39);
            this.lPacketTo.TabIndex = 45;
            this.lPacketTo.Text = "远端地址 :";
            this.lPacketTo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudPacketSocket
            // 
            this.nudPacketSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudPacketSocket.Location = new System.Drawing.Point(101, 6);
            this.nudPacketSocket.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPacketSocket.Name = "nudPacketSocket";
            this.nudPacketSocket.SelectionStart = 1;
            this.nudPacketSocket.Size = new System.Drawing.Size(213, 39);
            this.nudPacketSocket.TabIndex = 42;
            this.nudPacketSocket.Text = "1";
            this.nudPacketSocket.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudPacketSocket.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lPacketSocket
            // 
            this.lPacketSocket.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lPacketSocket.Dock = System.Windows.Forms.DockStyle.Right;
            this.lPacketSocket.LocalizationText = "PacketEditForm.UseSocket";
            this.lPacketSocket.Location = new System.Drawing.Point(6, 6);
            this.lPacketSocket.Name = "lPacketSocket";
            this.lPacketSocket.Size = new System.Drawing.Size(89, 39);
            this.lPacketSocket.TabIndex = 44;
            this.lPacketSocket.Text = "使用套接字 :";
            this.lPacketSocket.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtPacketTo
            // 
            this.txtPacketTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPacketTo.Location = new System.Drawing.Point(101, 51);
            this.txtPacketTo.Name = "txtPacketTo";
            this.txtPacketTo.ReadOnly = true;
            this.txtPacketTo.Size = new System.Drawing.Size(213, 39);
            this.txtPacketTo.TabIndex = 46;
            this.txtPacketTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dPacketSocket
            // 
            this.dPacketSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dPacketSocket.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dPacketSocket.LocalizationText = "Socket";
            this.dPacketSocket.Location = new System.Drawing.Point(3, 3);
            this.dPacketSocket.Name = "dPacketSocket";
            this.dPacketSocket.Orientation = AntdUI.TOrientation.Left;
            this.dPacketSocket.Size = new System.Drawing.Size(314, 30);
            this.dPacketSocket.TabIndex = 0;
            this.dPacketSocket.Text = "套接字";
            // 
            // bgwSendPacket
            // 
            this.bgwSendPacket.WorkerReportsProgress = true;
            this.bgwSendPacket.WorkerSupportsCancellation = true;
            this.bgwSendPacket.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwSendPacket_DoWork);
            this.bgwSendPacket.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwSendPacket_ProgressChanged);
            this.bgwSendPacket.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwSendPacket_RunWorkerCompleted);
            // 
            // PacketEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpPacketEdit);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "PacketEdit";
            this.Size = new System.Drawing.Size(1000, 700);
            this.Load += new System.EventHandler(this.PacketEdit_Load);
            this.tlpPacketEdit.ResumeLayout(false);
            this.pPacketData.ResumeLayout(false);
            this.tlpSendCollectionInfo.ResumeLayout(false);
            this.tlpSendCollectionInfo.PerformLayout();
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
            this.tlpPacketSettings.ResumeLayout(false);
            this.pProgression.ResumeLayout(false);
            this.tlpProgression.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.pPacketSend.ResumeLayout(false);
            this.tlpPacketSend.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.pPacketSocket.ResumeLayout(false);
            this.tlpPacketSocket.ResumeLayout(false);
            this.tlpFilterProgression.ResumeLayout(false);
            this.tlpFilterProgression.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanelEx tlpPacketEdit;
        private AntdUI.Panel pPacketData;
        private Be.Windows.Forms.HexBox hbPacketEdit;
        private TableLayoutPanelEx tlpSendCollectionInfo;
        private AntdUI.Label lSend_Fail_CNT;
        private AntdUI.Label lSend_Success_CNT;
        private AntdUI.Label lTotal_Send_CNT;
        private AntdUI.Label lSplit2;
        private AntdUI.Label lSplit1;
        private AntdUI.Label lSend_Fail;
        private AntdUI.Label lSend_Success;
        private AntdUI.Label lTotal_Send;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bStop;
        private AntdUI.Button bSend;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private TableLayoutPanelEx tlpPacketSettings;
        private AntdUI.Panel pProgression;
        private TableLayoutPanelEx tlpProgression;
        private TableLayoutPanelEx tableLayoutPanel4;
        private AntdUI.InputNumber nudProgressionCarry;
        private AntdUI.Checkbox cbProgressionCarry;
        private AntdUI.Checkbox cbProgressionPosition;
        private AntdUI.InputNumber nudProgressionPosition;
        private AntdUI.InputNumber nudProgressionStep;
        private AntdUI.Divider dProgression;
        private AntdUI.Panel pPacketSend;
        private TableLayoutPanelEx tlpPacketSend;
        private TableLayoutPanelEx tableLayoutPanel2;
        private AntdUI.Radio rbSendType_Continuously;
        private AntdUI.InputNumber nudSendType_Times;
        private AntdUI.InputNumber nudSendType_Interval;
        private AntdUI.Radio rbSendType_Times;
        private AntdUI.Divider dPacketSend;
        private AntdUI.Panel pPacketSocket;
        private TableLayoutPanelEx tlpPacketSocket;
        private TableLayoutPanelEx tlpFilterProgression;
        private AntdUI.InputNumber nudPacketLength;
        private AntdUI.Label lPacketLength;
        private AntdUI.Label lPacketTo;
        private AntdUI.InputNumber nudPacketSocket;
        private AntdUI.Label lPacketSocket;
        private AntdUI.Input txtPacketTo;
        private AntdUI.Divider dPacketSocket;
        private System.ComponentModel.BackgroundWorker bgwSendPacket;
    }
}
