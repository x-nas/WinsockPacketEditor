namespace WPE.InjectMode
{
    partial class PacketEditForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PacketEditForm));
            this.tlpPacketEdit = new System.Windows.Forms.TableLayoutPanel();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bStop = new AntdUI.Button();
            this.bSend = new AntdUI.Button();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tlpPacketData = new System.Windows.Forms.TableLayoutPanel();
            this.tlpSendCollectionInfo = new System.Windows.Forms.TableLayoutPanel();
            this.ddlEncoding = new AntdUI.Select();
            this.lSend_Fail_CNT = new AntdUI.Label();
            this.lSend_Success_CNT = new AntdUI.Label();
            this.lTotal_Send_CNT = new AntdUI.Label();
            this.label3 = new AntdUI.Label();
            this.label4 = new AntdUI.Label();
            this.lSend_Fail = new AntdUI.Label();
            this.lSend_Success = new AntdUI.Label();
            this.lTotal_Send = new AntdUI.Label();
            this.pBitInfo = new AntdUI.Panel();
            this.tlpBitInfo = new System.Windows.Forms.TableLayoutPanel();
            this.lDouble = new AntdUI.Label();
            this.lFloat = new AntdUI.Label();
            this.lUInt64 = new AntdUI.Label();
            this.lInt64 = new AntdUI.Label();
            this.lUInt32 = new AntdUI.Label();
            this.lInt32 = new AntdUI.Label();
            this.lUShort = new AntdUI.Label();
            this.lShort = new AntdUI.Label();
            this.lByte = new AntdUI.Label();
            this.lChar = new AntdUI.Label();
            this.lBits = new AntdUI.Label();
            this.tlpPacketSettings = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new AntdUI.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.nudProgressionCarry = new AntdUI.InputNumber();
            this.cbProgressionCarry = new AntdUI.Checkbox();
            this.cbProgressionPosition = new AntdUI.Checkbox();
            this.nudProgressionPosition = new AntdUI.InputNumber();
            this.nudProgressionStep = new AntdUI.InputNumber();
            this.divider2 = new AntdUI.Divider();
            this.panel1 = new AntdUI.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.rbSendType_Continuously = new AntdUI.Radio();
            this.nudSendType_Times = new AntdUI.InputNumber();
            this.nudSendType_Interval = new AntdUI.InputNumber();
            this.rbSendType_Times = new AntdUI.Radio();
            this.divider1 = new AntdUI.Divider();
            this.pFilterProgression = new AntdUI.Panel();
            this.tlpFilterProgression2 = new System.Windows.Forms.TableLayoutPanel();
            this.tlpFilterProgression = new System.Windows.Forms.TableLayoutPanel();
            this.nudPacketLength = new AntdUI.InputNumber();
            this.label7 = new AntdUI.Label();
            this.label5 = new AntdUI.Label();
            this.nudPacketSocket = new AntdUI.InputNumber();
            this.label1 = new AntdUI.Label();
            this.txtIPTo = new AntdUI.Input();
            this.dFilterProgression = new AntdUI.Divider();
            this.lHexBox_Position = new AntdUI.Label();
            this.lBits_Value = new AntdUI.Label();
            this.lChar_Value = new AntdUI.Label();
            this.lByte_Value = new AntdUI.Label();
            this.lShort_Value = new AntdUI.Label();
            this.lUShort_Value = new AntdUI.Label();
            this.lInt32_Value = new AntdUI.Label();
            this.lUInt32_Value = new AntdUI.Label();
            this.lInt64_Value = new AntdUI.Label();
            this.lUInt64_Value = new AntdUI.Label();
            this.lFloat_Value = new AntdUI.Label();
            this.lDouble_Value = new AntdUI.Label();
            this.pPacketData = new AntdUI.Panel();
            this.hbPacketData = new Be.Windows.Forms.HexBox();
            this.tlpPacketEdit.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpPacketData.SuspendLayout();
            this.tlpSendCollectionInfo.SuspendLayout();
            this.pBitInfo.SuspendLayout();
            this.tlpBitInfo.SuspendLayout();
            this.tlpPacketSettings.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.pFilterProgression.SuspendLayout();
            this.tlpFilterProgression2.SuspendLayout();
            this.tlpFilterProgression.SuspendLayout();
            this.pPacketData.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpPacketEdit
            // 
            this.tlpPacketEdit.ColumnCount = 1;
            this.tlpPacketEdit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketEdit.Controls.Add(this.tlpButton, 0, 2);
            this.tlpPacketEdit.Controls.Add(this.tlpPacketData, 0, 0);
            this.tlpPacketEdit.Controls.Add(this.tlpPacketSettings, 0, 1);
            this.tlpPacketEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPacketEdit.Location = new System.Drawing.Point(0, 0);
            this.tlpPacketEdit.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPacketEdit.Name = "tlpPacketEdit";
            this.tlpPacketEdit.RowCount = 3;
            this.tlpPacketEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tlpPacketEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tlpPacketEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpPacketEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpPacketEdit.Size = new System.Drawing.Size(984, 761);
            this.tlpPacketEdit.TabIndex = 0;
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
            this.tlpButton.Controls.Add(this.bSend, 1, 1);
            this.tlpButton.Controls.Add(this.bSave, 5, 1);
            this.tlpButton.Controls.Add(this.bExit, 7, 1);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 700);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(984, 61);
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
            this.bStop.Type = AntdUI.TTypeMini.Info;
            // 
            // bSend
            // 
            this.bSend.BackExtend = "135, #6253E1, #04BEFE";
            this.bSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSend.IconSvg = "PlayCircleOutlined";
            this.bSend.LoadingWaveColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(87)))), ((int)(((byte)(34)))));
            this.bSend.LoadingWaveCount = 6;
            this.bSend.LoadingWaveSize = 6;
            this.bSend.LoadingWaveValue = 0.6F;
            this.bSend.LoadingWaveVertical = true;
            this.bSend.Location = new System.Drawing.Point(225, 7);
            this.bSend.Name = "bSend";
            this.bSend.Size = new System.Drawing.Size(114, 46);
            this.bSend.TabIndex = 2;
            this.bSend.Text = "发送";
            this.bSend.Type = AntdUI.TTypeMini.Info;
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
            this.bSave.Type = AntdUI.TTypeMini.Info;
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
            // tlpPacketData
            // 
            this.tlpPacketData.ColumnCount = 2;
            this.tlpPacketData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketData.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 250F));
            this.tlpPacketData.Controls.Add(this.tlpSendCollectionInfo, 0, 0);
            this.tlpPacketData.Controls.Add(this.pBitInfo, 1, 1);
            this.tlpPacketData.Controls.Add(this.lHexBox_Position, 1, 0);
            this.tlpPacketData.Controls.Add(this.pPacketData, 0, 1);
            this.tlpPacketData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPacketData.Location = new System.Drawing.Point(0, 0);
            this.tlpPacketData.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPacketData.Name = "tlpPacketData";
            this.tlpPacketData.RowCount = 2;
            this.tlpPacketData.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpPacketData.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketData.Size = new System.Drawing.Size(984, 490);
            this.tlpPacketData.TabIndex = 12;
            // 
            // tlpSendCollectionInfo
            // 
            this.tlpSendCollectionInfo.ColumnCount = 10;
            this.tlpSendCollectionInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSendCollectionInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSendCollectionInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSendCollectionInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSendCollectionInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSendCollectionInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSendCollectionInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSendCollectionInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSendCollectionInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSendCollectionInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 300F));
            this.tlpSendCollectionInfo.Controls.Add(this.ddlEncoding, 9, 0);
            this.tlpSendCollectionInfo.Controls.Add(this.lSend_Fail_CNT, 7, 0);
            this.tlpSendCollectionInfo.Controls.Add(this.lSend_Success_CNT, 4, 0);
            this.tlpSendCollectionInfo.Controls.Add(this.lTotal_Send_CNT, 1, 0);
            this.tlpSendCollectionInfo.Controls.Add(this.label3, 5, 0);
            this.tlpSendCollectionInfo.Controls.Add(this.label4, 2, 0);
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
            this.tlpSendCollectionInfo.Size = new System.Drawing.Size(734, 45);
            this.tlpSendCollectionInfo.TabIndex = 15;
            // 
            // ddlEncoding
            // 
            this.ddlEncoding.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddlEncoding.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ddlEncoding.Location = new System.Drawing.Point(437, 3);
            this.ddlEncoding.Name = "ddlEncoding";
            this.ddlEncoding.Size = new System.Drawing.Size(294, 39);
            this.ddlEncoding.TabIndex = 16;
            this.ddlEncoding.SelectedIndexChanged += new AntdUI.IntEventHandler(this.ddlEncoding_SelectedIndexChanged);
            // 
            // lSend_Fail_CNT
            // 
            this.lSend_Fail_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSend_Fail_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSend_Fail_CNT.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSend_Fail_CNT.ForeColor = System.Drawing.Color.DarkRed;
            this.lSend_Fail_CNT.Location = new System.Drawing.Point(217, 3);
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
            this.lSend_Success_CNT.Location = new System.Drawing.Point(147, 3);
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
            this.lTotal_Send_CNT.ForeColor = System.Drawing.Color.Blue;
            this.lTotal_Send_CNT.Location = new System.Drawing.Point(77, 3);
            this.lTotal_Send_CNT.Name = "lTotal_Send_CNT";
            this.lTotal_Send_CNT.Size = new System.Drawing.Size(10, 39);
            this.lTotal_Send_CNT.TabIndex = 12;
            this.lTotal_Send_CNT.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.Silver;
            this.label3.Location = new System.Drawing.Point(163, 3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(6, 39);
            this.label3.TabIndex = 9;
            this.label3.Text = "|";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Silver;
            this.label4.Location = new System.Drawing.Point(93, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(6, 39);
            this.label4.TabIndex = 8;
            this.label4.Text = "|";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lSend_Fail
            // 
            this.lSend_Fail.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSend_Fail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSend_Fail.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSend_Fail.Location = new System.Drawing.Point(175, 3);
            this.lSend_Fail.Name = "lSend_Fail";
            this.lSend_Fail.Size = new System.Drawing.Size(36, 39);
            this.lSend_Fail.TabIndex = 7;
            this.lSend_Fail.Text = "失败:";
            // 
            // lSend_Success
            // 
            this.lSend_Success.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSend_Success.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSend_Success.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSend_Success.Location = new System.Drawing.Point(105, 3);
            this.lSend_Success.Name = "lSend_Success";
            this.lSend_Success.Size = new System.Drawing.Size(36, 39);
            this.lSend_Success.TabIndex = 6;
            this.lSend_Success.Text = "成功:";
            // 
            // lTotal_Send
            // 
            this.lTotal_Send.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lTotal_Send.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lTotal_Send.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lTotal_Send.Location = new System.Drawing.Point(3, 3);
            this.lTotal_Send.Name = "lTotal_Send";
            this.lTotal_Send.Size = new System.Drawing.Size(68, 39);
            this.lTotal_Send.TabIndex = 5;
            this.lTotal_Send.Text = "发送总数:";
            // 
            // pBitInfo
            // 
            this.pBitInfo.BorderWidth = 2F;
            this.pBitInfo.Controls.Add(this.tlpBitInfo);
            this.pBitInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pBitInfo.Location = new System.Drawing.Point(737, 48);
            this.pBitInfo.Name = "pBitInfo";
            this.pBitInfo.Radius = 10;
            this.pBitInfo.Size = new System.Drawing.Size(244, 439);
            this.pBitInfo.TabIndex = 46;
            // 
            // tlpBitInfo
            // 
            this.tlpBitInfo.BackColor = System.Drawing.Color.Transparent;
            this.tlpBitInfo.ColumnCount = 2;
            this.tlpBitInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpBitInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBitInfo.Controls.Add(this.lDouble_Value, 1, 10);
            this.tlpBitInfo.Controls.Add(this.lFloat_Value, 1, 9);
            this.tlpBitInfo.Controls.Add(this.lUInt64_Value, 1, 8);
            this.tlpBitInfo.Controls.Add(this.lInt64_Value, 1, 7);
            this.tlpBitInfo.Controls.Add(this.lUInt32_Value, 1, 6);
            this.tlpBitInfo.Controls.Add(this.lInt32_Value, 1, 5);
            this.tlpBitInfo.Controls.Add(this.lUShort_Value, 1, 4);
            this.tlpBitInfo.Controls.Add(this.lShort_Value, 1, 3);
            this.tlpBitInfo.Controls.Add(this.lByte_Value, 1, 2);
            this.tlpBitInfo.Controls.Add(this.lChar_Value, 1, 1);
            this.tlpBitInfo.Controls.Add(this.lDouble, 0, 10);
            this.tlpBitInfo.Controls.Add(this.lFloat, 0, 9);
            this.tlpBitInfo.Controls.Add(this.lUInt64, 0, 8);
            this.tlpBitInfo.Controls.Add(this.lInt64, 0, 7);
            this.tlpBitInfo.Controls.Add(this.lUInt32, 0, 6);
            this.tlpBitInfo.Controls.Add(this.lInt32, 0, 5);
            this.tlpBitInfo.Controls.Add(this.lUShort, 0, 4);
            this.tlpBitInfo.Controls.Add(this.lShort, 0, 3);
            this.tlpBitInfo.Controls.Add(this.lByte, 0, 2);
            this.tlpBitInfo.Controls.Add(this.lChar, 0, 1);
            this.tlpBitInfo.Controls.Add(this.lBits, 0, 0);
            this.tlpBitInfo.Controls.Add(this.lBits_Value, 1, 0);
            this.tlpBitInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBitInfo.Location = new System.Drawing.Point(2, 2);
            this.tlpBitInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpBitInfo.Name = "tlpBitInfo";
            this.tlpBitInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tlpBitInfo.RowCount = 12;
            this.tlpBitInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBitInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBitInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBitInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBitInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBitInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBitInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBitInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBitInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBitInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBitInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBitInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpBitInfo.Size = new System.Drawing.Size(240, 435);
            this.tlpBitInfo.TabIndex = 2;
            // 
            // lDouble
            // 
            this.lDouble.AutoEllipsis = true;
            this.lDouble.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lDouble.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lDouble.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lDouble.Location = new System.Drawing.Point(6, 366);
            this.lDouble.Name = "lDouble";
            this.lDouble.Size = new System.Drawing.Size(60, 30);
            this.lDouble.TabIndex = 10;
            this.lDouble.Text = "Double:";
            // 
            // lFloat
            // 
            this.lFloat.AutoEllipsis = true;
            this.lFloat.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lFloat.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lFloat.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lFloat.Location = new System.Drawing.Point(6, 330);
            this.lFloat.Name = "lFloat";
            this.lFloat.Size = new System.Drawing.Size(42, 30);
            this.lFloat.TabIndex = 9;
            this.lFloat.Text = "Float:";
            // 
            // lUInt64
            // 
            this.lUInt64.AutoEllipsis = true;
            this.lUInt64.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lUInt64.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lUInt64.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lUInt64.Location = new System.Drawing.Point(6, 294);
            this.lUInt64.Name = "lUInt64";
            this.lUInt64.Size = new System.Drawing.Size(56, 30);
            this.lUInt64.TabIndex = 8;
            this.lUInt64.Text = "UInt64:";
            // 
            // lInt64
            // 
            this.lInt64.AutoEllipsis = true;
            this.lInt64.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lInt64.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lInt64.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lInt64.Location = new System.Drawing.Point(6, 258);
            this.lInt64.Name = "lInt64";
            this.lInt64.Size = new System.Drawing.Size(44, 30);
            this.lInt64.TabIndex = 7;
            this.lInt64.Text = "Int64:";
            // 
            // lUInt32
            // 
            this.lUInt32.AutoEllipsis = true;
            this.lUInt32.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lUInt32.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lUInt32.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lUInt32.Location = new System.Drawing.Point(6, 222);
            this.lUInt32.Name = "lUInt32";
            this.lUInt32.Size = new System.Drawing.Size(56, 30);
            this.lUInt32.TabIndex = 6;
            this.lUInt32.Text = "UInt32:";
            // 
            // lInt32
            // 
            this.lInt32.AutoEllipsis = true;
            this.lInt32.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lInt32.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lInt32.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lInt32.Location = new System.Drawing.Point(6, 186);
            this.lInt32.Name = "lInt32";
            this.lInt32.Size = new System.Drawing.Size(44, 30);
            this.lInt32.TabIndex = 5;
            this.lInt32.Text = "Int32:";
            // 
            // lUShort
            // 
            this.lUShort.AutoEllipsis = true;
            this.lUShort.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lUShort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lUShort.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lUShort.Location = new System.Drawing.Point(6, 150);
            this.lUShort.Name = "lUShort";
            this.lUShort.Size = new System.Drawing.Size(58, 30);
            this.lUShort.TabIndex = 4;
            this.lUShort.Text = "UShort:";
            // 
            // lShort
            // 
            this.lShort.AutoEllipsis = true;
            this.lShort.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lShort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lShort.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lShort.Location = new System.Drawing.Point(6, 114);
            this.lShort.Name = "lShort";
            this.lShort.Size = new System.Drawing.Size(46, 30);
            this.lShort.TabIndex = 3;
            this.lShort.Text = "Short:";
            // 
            // lByte
            // 
            this.lByte.AutoEllipsis = true;
            this.lByte.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lByte.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lByte.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lByte.Location = new System.Drawing.Point(6, 78);
            this.lByte.Name = "lByte";
            this.lByte.Size = new System.Drawing.Size(38, 30);
            this.lByte.TabIndex = 2;
            this.lByte.Text = "Byte:";
            // 
            // lChar
            // 
            this.lChar.AutoEllipsis = true;
            this.lChar.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lChar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lChar.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lChar.Location = new System.Drawing.Point(6, 42);
            this.lChar.Name = "lChar";
            this.lChar.Size = new System.Drawing.Size(40, 30);
            this.lChar.TabIndex = 1;
            this.lChar.Text = "Char:";
            // 
            // lBits
            // 
            this.lBits.AutoEllipsis = true;
            this.lBits.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lBits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lBits.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lBits.Location = new System.Drawing.Point(6, 6);
            this.lBits.Name = "lBits";
            this.lBits.Size = new System.Drawing.Size(32, 30);
            this.lBits.TabIndex = 0;
            this.lBits.Text = "Bits:";
            // 
            // tlpPacketSettings
            // 
            this.tlpPacketSettings.ColumnCount = 3;
            this.tlpPacketSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tlpPacketSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tlpPacketSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tlpPacketSettings.Controls.Add(this.panel2, 2, 0);
            this.tlpPacketSettings.Controls.Add(this.panel1, 1, 0);
            this.tlpPacketSettings.Controls.Add(this.pFilterProgression, 0, 0);
            this.tlpPacketSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpPacketSettings.Location = new System.Drawing.Point(0, 490);
            this.tlpPacketSettings.Margin = new System.Windows.Forms.Padding(0);
            this.tlpPacketSettings.Name = "tlpPacketSettings";
            this.tlpPacketSettings.RowCount = 1;
            this.tlpPacketSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpPacketSettings.Size = new System.Drawing.Size(984, 210);
            this.tlpPacketSettings.TabIndex = 13;
            // 
            // panel2
            // 
            this.panel2.BorderWidth = 2F;
            this.panel2.Controls.Add(this.tableLayoutPanel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(651, 3);
            this.panel2.Name = "panel2";
            this.panel2.Radius = 10;
            this.panel2.Size = new System.Drawing.Size(330, 204);
            this.panel2.TabIndex = 13;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel3.ColumnCount = 1;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Controls.Add(this.tableLayoutPanel4, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.divider2, 0, 0);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel3.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(326, 200);
            this.tableLayoutPanel3.TabIndex = 0;
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
            this.tableLayoutPanel4.Size = new System.Drawing.Size(326, 164);
            this.tableLayoutPanel4.TabIndex = 1;
            // 
            // nudProgressionCarry
            // 
            this.nudProgressionCarry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudProgressionCarry.Location = new System.Drawing.Point(118, 102);
            this.nudProgressionCarry.Name = "nudProgressionCarry";
            this.nudProgressionCarry.Size = new System.Drawing.Size(202, 42);
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
            this.nudProgressionPosition.Location = new System.Drawing.Point(118, 6);
            this.nudProgressionPosition.Name = "nudProgressionPosition";
            this.nudProgressionPosition.ReadOnly = true;
            this.nudProgressionPosition.Size = new System.Drawing.Size(202, 42);
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
            this.nudProgressionStep.Location = new System.Drawing.Point(118, 54);
            this.nudProgressionStep.Name = "nudProgressionStep";
            this.nudProgressionStep.Size = new System.Drawing.Size(202, 42);
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
            // divider2
            // 
            this.divider2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.divider2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.divider2.Location = new System.Drawing.Point(3, 3);
            this.divider2.Name = "divider2";
            this.divider2.Orientation = AntdUI.TOrientation.Left;
            this.divider2.Size = new System.Drawing.Size(320, 30);
            this.divider2.TabIndex = 0;
            this.divider2.Text = "递进";
            // 
            // panel1
            // 
            this.panel1.BorderWidth = 2F;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(327, 3);
            this.panel1.Name = "panel1";
            this.panel1.Radius = 10;
            this.panel1.Size = new System.Drawing.Size(318, 204);
            this.panel1.TabIndex = 12;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.Transparent;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.divider1, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(2, 2);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(314, 200);
            this.tableLayoutPanel1.TabIndex = 0;
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
            this.tableLayoutPanel2.Size = new System.Drawing.Size(314, 164);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // rbSendType_Continuously
            // 
            this.rbSendType_Continuously.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbSendType_Continuously.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbSendType_Continuously.Location = new System.Drawing.Point(6, 54);
            this.rbSendType_Continuously.Name = "rbSendType_Continuously";
            this.rbSendType_Continuously.Size = new System.Drawing.Size(106, 42);
            this.rbSendType_Continuously.TabIndex = 45;
            this.rbSendType_Continuously.Text = "连续发送";
            // 
            // nudSendType_Times
            // 
            this.nudSendType_Times.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudSendType_Times.Location = new System.Drawing.Point(118, 6);
            this.nudSendType_Times.Name = "nudSendType_Times";
            this.nudSendType_Times.Size = new System.Drawing.Size(190, 42);
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
            this.nudSendType_Interval.Location = new System.Drawing.Point(118, 54);
            this.nudSendType_Interval.Name = "nudSendType_Interval";
            this.nudSendType_Interval.Size = new System.Drawing.Size(190, 42);
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
            this.rbSendType_Times.Location = new System.Drawing.Point(6, 6);
            this.rbSendType_Times.Name = "rbSendType_Times";
            this.rbSendType_Times.Size = new System.Drawing.Size(106, 42);
            this.rbSendType_Times.TabIndex = 44;
            this.rbSendType_Times.Text = "按次发送";
            this.rbSendType_Times.CheckedChanged += new AntdUI.BoolEventHandler(this.rbSendType_Times_CheckedChanged);
            // 
            // divider1
            // 
            this.divider1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.divider1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.divider1.Location = new System.Drawing.Point(3, 3);
            this.divider1.Name = "divider1";
            this.divider1.Orientation = AntdUI.TOrientation.Left;
            this.divider1.Size = new System.Drawing.Size(308, 30);
            this.divider1.TabIndex = 0;
            this.divider1.Text = "发送";
            // 
            // pFilterProgression
            // 
            this.pFilterProgression.BorderWidth = 2F;
            this.pFilterProgression.Controls.Add(this.tlpFilterProgression2);
            this.pFilterProgression.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pFilterProgression.Location = new System.Drawing.Point(3, 3);
            this.pFilterProgression.Name = "pFilterProgression";
            this.pFilterProgression.Radius = 10;
            this.pFilterProgression.Size = new System.Drawing.Size(318, 204);
            this.pFilterProgression.TabIndex = 11;
            // 
            // tlpFilterProgression2
            // 
            this.tlpFilterProgression2.BackColor = System.Drawing.Color.Transparent;
            this.tlpFilterProgression2.ColumnCount = 1;
            this.tlpFilterProgression2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterProgression2.Controls.Add(this.tlpFilterProgression, 0, 1);
            this.tlpFilterProgression2.Controls.Add(this.dFilterProgression, 0, 0);
            this.tlpFilterProgression2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterProgression2.Location = new System.Drawing.Point(2, 2);
            this.tlpFilterProgression2.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFilterProgression2.Name = "tlpFilterProgression2";
            this.tlpFilterProgression2.RowCount = 2;
            this.tlpFilterProgression2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterProgression2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterProgression2.Size = new System.Drawing.Size(314, 200);
            this.tlpFilterProgression2.TabIndex = 0;
            // 
            // tlpFilterProgression
            // 
            this.tlpFilterProgression.ColumnCount = 2;
            this.tlpFilterProgression.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpFilterProgression.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterProgression.Controls.Add(this.nudPacketLength, 1, 2);
            this.tlpFilterProgression.Controls.Add(this.label7, 0, 2);
            this.tlpFilterProgression.Controls.Add(this.label5, 0, 1);
            this.tlpFilterProgression.Controls.Add(this.nudPacketSocket, 1, 0);
            this.tlpFilterProgression.Controls.Add(this.label1, 0, 0);
            this.tlpFilterProgression.Controls.Add(this.txtIPTo, 1, 1);
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
            this.tlpFilterProgression.Size = new System.Drawing.Size(314, 164);
            this.tlpFilterProgression.TabIndex = 1;
            // 
            // nudPacketLength
            // 
            this.nudPacketLength.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudPacketLength.Enabled = false;
            this.nudPacketLength.Location = new System.Drawing.Point(92, 96);
            this.nudPacketLength.Name = "nudPacketLength";
            this.nudPacketLength.ReadOnly = true;
            this.nudPacketLength.Size = new System.Drawing.Size(216, 39);
            this.nudPacketLength.TabIndex = 48;
            this.nudPacketLength.Text = "1";
            this.nudPacketLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudPacketLength.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(6, 96);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 39);
            this.label7.TabIndex = 47;
            this.label7.Text = "长度";
            // 
            // label5
            // 
            this.label5.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(6, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(64, 39);
            this.label5.TabIndex = 45;
            this.label5.Text = "远端地址";
            // 
            // nudPacketSocket
            // 
            this.nudPacketSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudPacketSocket.Location = new System.Drawing.Point(92, 6);
            this.nudPacketSocket.Name = "nudPacketSocket";
            this.nudPacketSocket.Size = new System.Drawing.Size(216, 39);
            this.nudPacketSocket.TabIndex = 42;
            this.nudPacketSocket.Text = "1";
            this.nudPacketSocket.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudPacketSocket.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(6, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 39);
            this.label1.TabIndex = 44;
            this.label1.Text = "使用套接字";
            // 
            // txtIPTo
            // 
            this.txtIPTo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtIPTo.Enabled = false;
            this.txtIPTo.Location = new System.Drawing.Point(92, 51);
            this.txtIPTo.Name = "txtIPTo";
            this.txtIPTo.ReadOnly = true;
            this.txtIPTo.Size = new System.Drawing.Size(216, 39);
            this.txtIPTo.TabIndex = 46;
            this.txtIPTo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // dFilterProgression
            // 
            this.dFilterProgression.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dFilterProgression.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dFilterProgression.Location = new System.Drawing.Point(3, 3);
            this.dFilterProgression.Name = "dFilterProgression";
            this.dFilterProgression.Orientation = AntdUI.TOrientation.Left;
            this.dFilterProgression.Size = new System.Drawing.Size(308, 30);
            this.dFilterProgression.TabIndex = 0;
            this.dFilterProgression.Text = "套接字";
            // 
            // lHexBox_Position
            // 
            this.lHexBox_Position.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lHexBox_Position.Location = new System.Drawing.Point(737, 3);
            this.lHexBox_Position.Name = "lHexBox_Position";
            this.lHexBox_Position.Size = new System.Drawing.Size(244, 39);
            this.lHexBox_Position.TabIndex = 47;
            this.lHexBox_Position.Text = "lHexBox_Position";
            this.lHexBox_Position.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lBits_Value
            // 
            this.lBits_Value.AutoEllipsis = true;
            this.lBits_Value.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lBits_Value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lBits_Value.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lBits_Value.Location = new System.Drawing.Point(72, 6);
            this.lBits_Value.Name = "lBits_Value";
            this.lBits_Value.Size = new System.Drawing.Size(68, 30);
            this.lBits_Value.TabIndex = 11;
            this.lBits_Value.Text = "Bits_Value";
            // 
            // lChar_Value
            // 
            this.lChar_Value.AutoEllipsis = true;
            this.lChar_Value.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lChar_Value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lChar_Value.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lChar_Value.Location = new System.Drawing.Point(72, 42);
            this.lChar_Value.Name = "lChar_Value";
            this.lChar_Value.Size = new System.Drawing.Size(75, 30);
            this.lChar_Value.TabIndex = 12;
            this.lChar_Value.Text = "Char_Value";
            // 
            // lByte_Value
            // 
            this.lByte_Value.AutoEllipsis = true;
            this.lByte_Value.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lByte_Value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lByte_Value.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lByte_Value.Location = new System.Drawing.Point(72, 78);
            this.lByte_Value.Name = "lByte_Value";
            this.lByte_Value.Size = new System.Drawing.Size(74, 30);
            this.lByte_Value.TabIndex = 13;
            this.lByte_Value.Text = "Byte_Value";
            // 
            // lShort_Value
            // 
            this.lShort_Value.AutoEllipsis = true;
            this.lShort_Value.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lShort_Value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lShort_Value.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lShort_Value.Location = new System.Drawing.Point(72, 114);
            this.lShort_Value.Name = "lShort_Value";
            this.lShort_Value.Size = new System.Drawing.Size(80, 30);
            this.lShort_Value.TabIndex = 14;
            this.lShort_Value.Text = "Short_Value";
            // 
            // lUShort_Value
            // 
            this.lUShort_Value.AutoEllipsis = true;
            this.lUShort_Value.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lUShort_Value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lUShort_Value.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lUShort_Value.Location = new System.Drawing.Point(72, 150);
            this.lUShort_Value.Name = "lUShort_Value";
            this.lUShort_Value.Size = new System.Drawing.Size(91, 30);
            this.lUShort_Value.TabIndex = 15;
            this.lUShort_Value.Text = "UShort_Value";
            // 
            // lInt32_Value
            // 
            this.lInt32_Value.AutoEllipsis = true;
            this.lInt32_Value.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lInt32_Value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lInt32_Value.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lInt32_Value.Location = new System.Drawing.Point(72, 186);
            this.lInt32_Value.Name = "lInt32_Value";
            this.lInt32_Value.Size = new System.Drawing.Size(79, 30);
            this.lInt32_Value.TabIndex = 16;
            this.lInt32_Value.Text = "Int32_Value";
            // 
            // lUInt32_Value
            // 
            this.lUInt32_Value.AutoEllipsis = true;
            this.lUInt32_Value.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lUInt32_Value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lUInt32_Value.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lUInt32_Value.Location = new System.Drawing.Point(72, 222);
            this.lUInt32_Value.Name = "lUInt32_Value";
            this.lUInt32_Value.Size = new System.Drawing.Size(89, 30);
            this.lUInt32_Value.TabIndex = 17;
            this.lUInt32_Value.Text = "UInt32_Value";
            // 
            // lInt64_Value
            // 
            this.lInt64_Value.AutoEllipsis = true;
            this.lInt64_Value.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lInt64_Value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lInt64_Value.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lInt64_Value.Location = new System.Drawing.Point(72, 258);
            this.lInt64_Value.Name = "lInt64_Value";
            this.lInt64_Value.Size = new System.Drawing.Size(79, 30);
            this.lInt64_Value.TabIndex = 18;
            this.lInt64_Value.Text = "Int64_Value";
            // 
            // lUInt64_Value
            // 
            this.lUInt64_Value.AutoEllipsis = true;
            this.lUInt64_Value.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lUInt64_Value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lUInt64_Value.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lUInt64_Value.Location = new System.Drawing.Point(72, 294);
            this.lUInt64_Value.Name = "lUInt64_Value";
            this.lUInt64_Value.Size = new System.Drawing.Size(89, 30);
            this.lUInt64_Value.TabIndex = 19;
            this.lUInt64_Value.Text = "UInt64_Value";
            // 
            // lFloat_Value
            // 
            this.lFloat_Value.AutoEllipsis = true;
            this.lFloat_Value.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lFloat_Value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lFloat_Value.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lFloat_Value.Location = new System.Drawing.Point(72, 330);
            this.lFloat_Value.Name = "lFloat_Value";
            this.lFloat_Value.Size = new System.Drawing.Size(77, 30);
            this.lFloat_Value.TabIndex = 20;
            this.lFloat_Value.Text = "Float_Value";
            // 
            // lDouble_Value
            // 
            this.lDouble_Value.AutoEllipsis = true;
            this.lDouble_Value.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lDouble_Value.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lDouble_Value.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lDouble_Value.Location = new System.Drawing.Point(72, 366);
            this.lDouble_Value.Name = "lDouble_Value";
            this.lDouble_Value.Size = new System.Drawing.Size(93, 30);
            this.lDouble_Value.TabIndex = 21;
            this.lDouble_Value.Text = "Double_Value";
            // 
            // pPacketData
            // 
            this.pPacketData.BorderWidth = 2F;
            this.pPacketData.Controls.Add(this.hbPacketData);
            this.pPacketData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pPacketData.Location = new System.Drawing.Point(3, 48);
            this.pPacketData.Name = "pPacketData";
            this.pPacketData.Padding = new System.Windows.Forms.Padding(3);
            this.pPacketData.Radius = 10;
            this.pPacketData.Size = new System.Drawing.Size(728, 439);
            this.pPacketData.TabIndex = 48;
            this.pPacketData.Text = "panel3";
            // 
            // hbPacketData
            // 
            this.hbPacketData.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.hbPacketData.ColumnInfoVisible = true;
            this.hbPacketData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hbPacketData.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hbPacketData.LineInfoVisible = true;
            this.hbPacketData.Location = new System.Drawing.Point(5, 5);
            this.hbPacketData.Name = "hbPacketData";
            this.hbPacketData.ShadowSelectionColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(60)))), ((int)(((byte)(188)))), ((int)(((byte)(255)))));
            this.hbPacketData.Size = new System.Drawing.Size(718, 429);
            this.hbPacketData.StringViewVisible = true;
            this.hbPacketData.TabIndex = 1;
            this.hbPacketData.VScrollBarVisible = true;
            this.hbPacketData.MouseDown += new System.Windows.Forms.MouseEventHandler(this.hbPacketData_MouseDown);
            // 
            // PacketEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 761);
            this.Controls.Add(this.tlpPacketEdit);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "PacketEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PacketEditForm";
            this.Load += new System.EventHandler(this.PacketEditForm_Load);
            this.tlpPacketEdit.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.tlpPacketData.ResumeLayout(false);
            this.tlpSendCollectionInfo.ResumeLayout(false);
            this.tlpSendCollectionInfo.PerformLayout();
            this.pBitInfo.ResumeLayout(false);
            this.tlpBitInfo.ResumeLayout(false);
            this.tlpBitInfo.PerformLayout();
            this.tlpPacketSettings.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.pFilterProgression.ResumeLayout(false);
            this.tlpFilterProgression2.ResumeLayout(false);
            this.tlpFilterProgression.ResumeLayout(false);
            this.tlpFilterProgression.PerformLayout();
            this.pPacketData.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpPacketEdit;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private AntdUI.Button bStop;
        private AntdUI.Button bSend;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private System.Windows.Forms.TableLayoutPanel tlpPacketData;
        private System.Windows.Forms.TableLayoutPanel tlpPacketSettings;
        private AntdUI.Panel panel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private AntdUI.Checkbox cbProgressionPosition;
        private AntdUI.InputNumber nudProgressionPosition;
        private AntdUI.InputNumber nudProgressionStep;
        private AntdUI.Divider divider2;
        private AntdUI.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private AntdUI.InputNumber nudSendType_Times;
        private AntdUI.InputNumber nudSendType_Interval;
        private AntdUI.Divider divider1;
        private AntdUI.Panel pFilterProgression;
        private System.Windows.Forms.TableLayoutPanel tlpFilterProgression2;
        private System.Windows.Forms.TableLayoutPanel tlpFilterProgression;
        private AntdUI.InputNumber nudPacketSocket;
        private AntdUI.Divider dFilterProgression;
        private AntdUI.Label label1;
        private AntdUI.InputNumber nudPacketLength;
        private AntdUI.Label label7;
        private AntdUI.Label label5;
        private AntdUI.Input txtIPTo;
        private AntdUI.Radio rbSendType_Times;
        private AntdUI.Radio rbSendType_Continuously;
        private AntdUI.InputNumber nudProgressionCarry;
        private AntdUI.Checkbox cbProgressionCarry;
        private System.Windows.Forms.TableLayoutPanel tlpSendCollectionInfo;
        private AntdUI.Label lSend_Fail_CNT;
        private AntdUI.Label lSend_Success_CNT;
        private AntdUI.Label lTotal_Send_CNT;
        private AntdUI.Label label3;
        private AntdUI.Label label4;
        private AntdUI.Label lSend_Fail;
        private AntdUI.Label lSend_Success;
        private AntdUI.Label lTotal_Send;
        private AntdUI.Panel pBitInfo;
        private System.Windows.Forms.TableLayoutPanel tlpBitInfo;
        private AntdUI.Label lDouble;
        private AntdUI.Label lFloat;
        private AntdUI.Label lUInt64;
        private AntdUI.Label lInt64;
        private AntdUI.Label lUInt32;
        private AntdUI.Label lInt32;
        private AntdUI.Label lUShort;
        private AntdUI.Label lShort;
        private AntdUI.Label lByte;
        private AntdUI.Label lChar;
        private AntdUI.Label lBits;
        private AntdUI.Select ddlEncoding;
        private AntdUI.Label lHexBox_Position;
        private AntdUI.Label lDouble_Value;
        private AntdUI.Label lFloat_Value;
        private AntdUI.Label lUInt64_Value;
        private AntdUI.Label lInt64_Value;
        private AntdUI.Label lUInt32_Value;
        private AntdUI.Label lInt32_Value;
        private AntdUI.Label lUShort_Value;
        private AntdUI.Label lShort_Value;
        private AntdUI.Label lByte_Value;
        private AntdUI.Label lChar_Value;
        private AntdUI.Label lBits_Value;
        private AntdUI.Panel pPacketData;
        private Be.Windows.Forms.HexBox hbPacketData;
    }
}