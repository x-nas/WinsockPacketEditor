namespace WPE.InjectMode
{
    partial class SendEditForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SendEditForm));
            this.tlpSendEdit = new System.Windows.Forms.TableLayoutPanel();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bStop = new AntdUI.Button();
            this.bExecute = new AntdUI.Button();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tlpSendCollectionInfo = new System.Windows.Forms.TableLayoutPanel();
            this.lSend_Fail_CNT = new AntdUI.Label();
            this.lSend_Success_CNT = new AntdUI.Label();
            this.lTotal_Send_CNT = new AntdUI.Label();
            this.label3 = new AntdUI.Label();
            this.label4 = new AntdUI.Label();
            this.lSend_Fail = new AntdUI.Label();
            this.lSend_Success = new AntdUI.Label();
            this.lTotal_Send = new AntdUI.Label();
            this.tSendCollection = new AntdUI.Table();
            this.txtNotes = new AntdUI.Input();
            this.tlpSendCollectionSettings = new System.Windows.Forms.TableLayoutPanel();
            this.pLoopINT = new AntdUI.Panel();
            this.tlpLoopINT = new System.Windows.Forms.TableLayoutPanel();
            this.nudLoopINT = new AntdUI.InputNumber();
            this.dLoopINT = new AntdUI.Divider();
            this.pLoopCNT = new AntdUI.Panel();
            this.tlpLoopCNT = new System.Windows.Forms.TableLayoutPanel();
            this.dLoopCNT = new AntdUI.Divider();
            this.nudLoopCNT = new AntdUI.InputNumber();
            this.pSendSocket = new AntdUI.Panel();
            this.tlpSendSocket = new System.Windows.Forms.TableLayoutPanel();
            this.dSendSocket = new AntdUI.Divider();
            this.cbSystemSocket = new AntdUI.Checkbox();
            this.pSendName = new AntdUI.Panel();
            this.tlpSendName = new System.Windows.Forms.TableLayoutPanel();
            this.dSendName = new AntdUI.Divider();
            this.txtSendName = new AntdUI.Input();
            this.tlpSendEdit.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpSendCollectionInfo.SuspendLayout();
            this.tlpSendCollectionSettings.SuspendLayout();
            this.pLoopINT.SuspendLayout();
            this.tlpLoopINT.SuspendLayout();
            this.pLoopCNT.SuspendLayout();
            this.tlpLoopCNT.SuspendLayout();
            this.pSendSocket.SuspendLayout();
            this.tlpSendSocket.SuspendLayout();
            this.pSendName.SuspendLayout();
            this.tlpSendName.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpSendEdit
            // 
            this.tlpSendEdit.ColumnCount = 1;
            this.tlpSendEdit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSendEdit.Controls.Add(this.tlpButton, 0, 4);
            this.tlpSendEdit.Controls.Add(this.tlpSendCollectionInfo, 0, 0);
            this.tlpSendEdit.Controls.Add(this.tSendCollection, 0, 1);
            this.tlpSendEdit.Controls.Add(this.txtNotes, 0, 3);
            this.tlpSendEdit.Controls.Add(this.tlpSendCollectionSettings, 0, 2);
            this.tlpSendEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSendEdit.Location = new System.Drawing.Point(0, 0);
            this.tlpSendEdit.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSendEdit.Name = "tlpSendEdit";
            this.tlpSendEdit.RowCount = 5;
            this.tlpSendEdit.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSendEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSendEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpSendEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpSendEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpSendEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpSendEdit.Size = new System.Drawing.Size(984, 761);
            this.tlpSendEdit.TabIndex = 0;
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
            this.tlpButton.TabIndex = 10;
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
            this.tlpSendCollectionInfo.Controls.Add(this.lSend_Fail_CNT, 7, 1);
            this.tlpSendCollectionInfo.Controls.Add(this.lSend_Success_CNT, 4, 1);
            this.tlpSendCollectionInfo.Controls.Add(this.lTotal_Send_CNT, 1, 1);
            this.tlpSendCollectionInfo.Controls.Add(this.label3, 5, 1);
            this.tlpSendCollectionInfo.Controls.Add(this.label4, 2, 1);
            this.tlpSendCollectionInfo.Controls.Add(this.lSend_Fail, 6, 1);
            this.tlpSendCollectionInfo.Controls.Add(this.lSend_Success, 3, 1);
            this.tlpSendCollectionInfo.Controls.Add(this.lTotal_Send, 0, 1);
            this.tlpSendCollectionInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSendCollectionInfo.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tlpSendCollectionInfo.Location = new System.Drawing.Point(0, 0);
            this.tlpSendCollectionInfo.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSendCollectionInfo.Name = "tlpSendCollectionInfo";
            this.tlpSendCollectionInfo.RowCount = 3;
            this.tlpSendCollectionInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSendCollectionInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlpSendCollectionInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpSendCollectionInfo.Size = new System.Drawing.Size(984, 30);
            this.tlpSendCollectionInfo.TabIndex = 6;
            // 
            // lSend_Fail_CNT
            // 
            this.lSend_Fail_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSend_Fail_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSend_Fail_CNT.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSend_Fail_CNT.Location = new System.Drawing.Point(217, 3);
            this.lSend_Fail_CNT.Name = "lSend_Fail_CNT";
            this.lSend_Fail_CNT.Size = new System.Drawing.Size(10, 24);
            this.lSend_Fail_CNT.TabIndex = 14;
            this.lSend_Fail_CNT.Text = "0";
            // 
            // lSend_Success_CNT
            // 
            this.lSend_Success_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSend_Success_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSend_Success_CNT.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSend_Success_CNT.Location = new System.Drawing.Point(147, 3);
            this.lSend_Success_CNT.Name = "lSend_Success_CNT";
            this.lSend_Success_CNT.Size = new System.Drawing.Size(10, 24);
            this.lSend_Success_CNT.TabIndex = 13;
            this.lSend_Success_CNT.Text = "0";
            // 
            // lTotal_Send_CNT
            // 
            this.lTotal_Send_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lTotal_Send_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lTotal_Send_CNT.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lTotal_Send_CNT.Location = new System.Drawing.Point(77, 3);
            this.lTotal_Send_CNT.Name = "lTotal_Send_CNT";
            this.lTotal_Send_CNT.Size = new System.Drawing.Size(10, 24);
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
            this.label3.Size = new System.Drawing.Size(6, 24);
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
            this.label4.Size = new System.Drawing.Size(6, 24);
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
            this.lSend_Fail.Size = new System.Drawing.Size(36, 24);
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
            this.lSend_Success.Size = new System.Drawing.Size(36, 24);
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
            this.lTotal_Send.Size = new System.Drawing.Size(68, 24);
            this.lTotal_Send.TabIndex = 5;
            this.lTotal_Send.Text = "发送总数:";
            // 
            // tSendCollection
            // 
            this.tSendCollection.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tSendCollection.Bordered = true;
            this.tSendCollection.CellImpactHeight = false;
            this.tSendCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tSendCollection.EmptyHeader = true;
            this.tSendCollection.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tSendCollection.Gap = 8;
            this.tSendCollection.GapCell = 0;
            this.tSendCollection.Location = new System.Drawing.Point(3, 33);
            this.tSendCollection.Name = "tSendCollection";
            this.tSendCollection.Size = new System.Drawing.Size(978, 465);
            this.tSendCollection.TabIndex = 7;
            // 
            // txtNotes
            // 
            this.txtNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNotes.Location = new System.Drawing.Point(3, 604);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.PlaceholderText = "备注信息";
            this.txtNotes.Size = new System.Drawing.Size(978, 94);
            this.txtNotes.TabIndex = 8;
            // 
            // tlpSendCollectionSettings
            // 
            this.tlpSendCollectionSettings.ColumnCount = 4;
            this.tlpSendCollectionSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpSendCollectionSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpSendCollectionSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpSendCollectionSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlpSendCollectionSettings.Controls.Add(this.pLoopINT, 3, 0);
            this.tlpSendCollectionSettings.Controls.Add(this.pLoopCNT, 2, 0);
            this.tlpSendCollectionSettings.Controls.Add(this.pSendSocket, 1, 0);
            this.tlpSendCollectionSettings.Controls.Add(this.pSendName, 0, 0);
            this.tlpSendCollectionSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSendCollectionSettings.Location = new System.Drawing.Point(0, 501);
            this.tlpSendCollectionSettings.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSendCollectionSettings.Name = "tlpSendCollectionSettings";
            this.tlpSendCollectionSettings.RowCount = 1;
            this.tlpSendCollectionSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSendCollectionSettings.Size = new System.Drawing.Size(984, 100);
            this.tlpSendCollectionSettings.TabIndex = 9;
            // 
            // pLoopINT
            // 
            this.pLoopINT.BorderWidth = 2F;
            this.pLoopINT.Controls.Add(this.tlpLoopINT);
            this.pLoopINT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pLoopINT.Location = new System.Drawing.Point(741, 3);
            this.pLoopINT.Name = "pLoopINT";
            this.pLoopINT.Radius = 10;
            this.pLoopINT.Size = new System.Drawing.Size(240, 94);
            this.pLoopINT.TabIndex = 13;
            // 
            // tlpLoopINT
            // 
            this.tlpLoopINT.BackColor = System.Drawing.Color.Transparent;
            this.tlpLoopINT.ColumnCount = 1;
            this.tlpLoopINT.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLoopINT.Controls.Add(this.nudLoopINT, 0, 1);
            this.tlpLoopINT.Controls.Add(this.dLoopINT, 0, 0);
            this.tlpLoopINT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpLoopINT.Location = new System.Drawing.Point(2, 2);
            this.tlpLoopINT.Margin = new System.Windows.Forms.Padding(0);
            this.tlpLoopINT.Name = "tlpLoopINT";
            this.tlpLoopINT.RowCount = 3;
            this.tlpLoopINT.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpLoopINT.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpLoopINT.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLoopINT.Size = new System.Drawing.Size(236, 90);
            this.tlpLoopINT.TabIndex = 0;
            // 
            // nudLoopINT
            // 
            this.nudLoopINT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudLoopINT.Location = new System.Drawing.Point(3, 39);
            this.nudLoopINT.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudLoopINT.Name = "nudLoopINT";
            this.nudLoopINT.Size = new System.Drawing.Size(230, 34);
            this.nudLoopINT.SuffixText = "毫秒";
            this.nudLoopINT.TabIndex = 2;
            this.nudLoopINT.Text = "100";
            this.nudLoopINT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudLoopINT.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // dLoopINT
            // 
            this.dLoopINT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dLoopINT.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dLoopINT.Location = new System.Drawing.Point(3, 3);
            this.dLoopINT.Name = "dLoopINT";
            this.dLoopINT.Orientation = AntdUI.TOrientation.Left;
            this.dLoopINT.Size = new System.Drawing.Size(230, 30);
            this.dLoopINT.TabIndex = 0;
            this.dLoopINT.Text = "发送间隔";
            // 
            // pLoopCNT
            // 
            this.pLoopCNT.BorderWidth = 2F;
            this.pLoopCNT.Controls.Add(this.tlpLoopCNT);
            this.pLoopCNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pLoopCNT.Location = new System.Drawing.Point(495, 3);
            this.pLoopCNT.Name = "pLoopCNT";
            this.pLoopCNT.Radius = 10;
            this.pLoopCNT.Size = new System.Drawing.Size(240, 94);
            this.pLoopCNT.TabIndex = 12;
            // 
            // tlpLoopCNT
            // 
            this.tlpLoopCNT.BackColor = System.Drawing.Color.Transparent;
            this.tlpLoopCNT.ColumnCount = 1;
            this.tlpLoopCNT.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLoopCNT.Controls.Add(this.dLoopCNT, 0, 0);
            this.tlpLoopCNT.Controls.Add(this.nudLoopCNT, 0, 1);
            this.tlpLoopCNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpLoopCNT.Location = new System.Drawing.Point(2, 2);
            this.tlpLoopCNT.Margin = new System.Windows.Forms.Padding(0);
            this.tlpLoopCNT.Name = "tlpLoopCNT";
            this.tlpLoopCNT.RowCount = 3;
            this.tlpLoopCNT.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpLoopCNT.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpLoopCNT.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLoopCNT.Size = new System.Drawing.Size(236, 90);
            this.tlpLoopCNT.TabIndex = 0;
            // 
            // dLoopCNT
            // 
            this.dLoopCNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dLoopCNT.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dLoopCNT.Location = new System.Drawing.Point(3, 3);
            this.dLoopCNT.Name = "dLoopCNT";
            this.dLoopCNT.Orientation = AntdUI.TOrientation.Left;
            this.dLoopCNT.Size = new System.Drawing.Size(230, 30);
            this.dLoopCNT.TabIndex = 0;
            this.dLoopCNT.Text = "循环次数";
            // 
            // nudLoopCNT
            // 
            this.nudLoopCNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudLoopCNT.Location = new System.Drawing.Point(3, 39);
            this.nudLoopCNT.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudLoopCNT.Name = "nudLoopCNT";
            this.nudLoopCNT.Size = new System.Drawing.Size(230, 40);
            this.nudLoopCNT.SuffixText = "次";
            this.nudLoopCNT.TabIndex = 1;
            this.nudLoopCNT.Text = "1";
            this.nudLoopCNT.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudLoopCNT.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // pSendSocket
            // 
            this.pSendSocket.BorderWidth = 2F;
            this.pSendSocket.Controls.Add(this.tlpSendSocket);
            this.pSendSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pSendSocket.Location = new System.Drawing.Point(249, 3);
            this.pSendSocket.Name = "pSendSocket";
            this.pSendSocket.Radius = 10;
            this.pSendSocket.Size = new System.Drawing.Size(240, 94);
            this.pSendSocket.TabIndex = 11;
            // 
            // tlpSendSocket
            // 
            this.tlpSendSocket.BackColor = System.Drawing.Color.Transparent;
            this.tlpSendSocket.ColumnCount = 1;
            this.tlpSendSocket.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSendSocket.Controls.Add(this.dSendSocket, 0, 0);
            this.tlpSendSocket.Controls.Add(this.cbSystemSocket, 0, 1);
            this.tlpSendSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSendSocket.Location = new System.Drawing.Point(2, 2);
            this.tlpSendSocket.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSendSocket.Name = "tlpSendSocket";
            this.tlpSendSocket.RowCount = 3;
            this.tlpSendSocket.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSendSocket.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSendSocket.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSendSocket.Size = new System.Drawing.Size(236, 90);
            this.tlpSendSocket.TabIndex = 0;
            // 
            // dSendSocket
            // 
            this.dSendSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dSendSocket.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dSendSocket.Location = new System.Drawing.Point(3, 3);
            this.dSendSocket.Name = "dSendSocket";
            this.dSendSocket.Orientation = AntdUI.TOrientation.Left;
            this.dSendSocket.Size = new System.Drawing.Size(230, 30);
            this.dSendSocket.TabIndex = 0;
            this.dSendSocket.Text = "套接字";
            // 
            // cbSystemSocket
            // 
            this.cbSystemSocket.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbSystemSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbSystemSocket.Location = new System.Drawing.Point(3, 39);
            this.cbSystemSocket.Name = "cbSystemSocket";
            this.cbSystemSocket.Size = new System.Drawing.Size(154, 42);
            this.cbSystemSocket.TabIndex = 1;
            this.cbSystemSocket.Text = "使用系统套接字";
            // 
            // pSendName
            // 
            this.pSendName.BorderWidth = 2F;
            this.pSendName.Controls.Add(this.tlpSendName);
            this.pSendName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pSendName.Location = new System.Drawing.Point(3, 3);
            this.pSendName.Name = "pSendName";
            this.pSendName.Radius = 10;
            this.pSendName.Size = new System.Drawing.Size(240, 94);
            this.pSendName.TabIndex = 10;
            // 
            // tlpSendName
            // 
            this.tlpSendName.BackColor = System.Drawing.Color.Transparent;
            this.tlpSendName.ColumnCount = 1;
            this.tlpSendName.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSendName.Controls.Add(this.dSendName, 0, 0);
            this.tlpSendName.Controls.Add(this.txtSendName, 0, 1);
            this.tlpSendName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSendName.Location = new System.Drawing.Point(2, 2);
            this.tlpSendName.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSendName.Name = "tlpSendName";
            this.tlpSendName.RowCount = 3;
            this.tlpSendName.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSendName.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSendName.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSendName.Size = new System.Drawing.Size(236, 90);
            this.tlpSendName.TabIndex = 0;
            // 
            // dSendName
            // 
            this.dSendName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dSendName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dSendName.Location = new System.Drawing.Point(3, 3);
            this.dSendName.Name = "dSendName";
            this.dSendName.Orientation = AntdUI.TOrientation.Left;
            this.dSendName.Size = new System.Drawing.Size(230, 30);
            this.dSendName.TabIndex = 0;
            this.dSendName.Text = "发送名称";
            // 
            // txtSendName
            // 
            this.txtSendName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSendName.Location = new System.Drawing.Point(3, 39);
            this.txtSendName.Name = "txtSendName";
            this.txtSendName.PlaceholderText = "请输入字符和数字";
            this.txtSendName.Size = new System.Drawing.Size(230, 40);
            this.txtSendName.TabIndex = 1;
            this.txtSendName.TextChanged += new System.EventHandler(this.txtSendName_TextChanged);
            // 
            // SendEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 761);
            this.Controls.Add(this.tlpSendEdit);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "SendEditForm";
            this.Text = "SendEditForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SendEditForm_FormClosing);
            this.Load += new System.EventHandler(this.SendEditForm_Load);
            this.tlpSendEdit.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.tlpSendCollectionInfo.ResumeLayout(false);
            this.tlpSendCollectionInfo.PerformLayout();
            this.tlpSendCollectionSettings.ResumeLayout(false);
            this.pLoopINT.ResumeLayout(false);
            this.tlpLoopINT.ResumeLayout(false);
            this.pLoopCNT.ResumeLayout(false);
            this.tlpLoopCNT.ResumeLayout(false);
            this.pSendSocket.ResumeLayout(false);
            this.tlpSendSocket.ResumeLayout(false);
            this.tlpSendSocket.PerformLayout();
            this.pSendName.ResumeLayout(false);
            this.tlpSendName.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpSendEdit;
        private System.Windows.Forms.TableLayoutPanel tlpSendCollectionInfo;
        private AntdUI.Label lSend_Fail_CNT;
        private AntdUI.Label lSend_Success_CNT;
        private AntdUI.Label lTotal_Send_CNT;
        private AntdUI.Label label3;
        private AntdUI.Label label4;
        private AntdUI.Label lSend_Fail;
        private AntdUI.Label lSend_Success;
        private AntdUI.Label lTotal_Send;
        private AntdUI.Table tSendCollection;
        private AntdUI.Input txtNotes;
        private System.Windows.Forms.TableLayoutPanel tlpSendCollectionSettings;
        private AntdUI.Panel pSendName;
        private System.Windows.Forms.TableLayoutPanel tlpSendName;
        private AntdUI.Divider dSendName;
        private AntdUI.Input txtSendName;
        private AntdUI.Panel pLoopINT;
        private System.Windows.Forms.TableLayoutPanel tlpLoopINT;
        private AntdUI.Divider dLoopINT;
        private AntdUI.Panel pLoopCNT;
        private System.Windows.Forms.TableLayoutPanel tlpLoopCNT;
        private AntdUI.Divider dLoopCNT;
        private AntdUI.InputNumber nudLoopCNT;
        private AntdUI.Panel pSendSocket;
        private System.Windows.Forms.TableLayoutPanel tlpSendSocket;
        private AntdUI.Divider dSendSocket;
        private AntdUI.Checkbox cbSystemSocket;
        private AntdUI.InputNumber nudLoopINT;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private AntdUI.Button bStop;
        private AntdUI.Button bExecute;
    }
}