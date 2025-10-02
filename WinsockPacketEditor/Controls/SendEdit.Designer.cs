namespace WinsockPacketEditor
{
    partial class SendEdit
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
            this.tlpSendEdit = new WinsockPacketEditor.TableLayoutPanelEx();
            this.tlpButton = new WinsockPacketEditor.TableLayoutPanelEx();
            this.bStop = new AntdUI.Button();
            this.bExecute = new AntdUI.Button();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tlpSendCollectionInfo = new WinsockPacketEditor.TableLayoutPanelEx();
            this.ddMenu = new AntdUI.Dropdown();
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
            this.tlpSendCollectionSettings = new WinsockPacketEditor.TableLayoutPanelEx();
            this.pLoopINT = new AntdUI.Panel();
            this.tlpLoopINT = new WinsockPacketEditor.TableLayoutPanelEx();
            this.nudLoopINT = new AntdUI.InputNumber();
            this.dLoopINT = new AntdUI.Divider();
            this.pLoopCNT = new AntdUI.Panel();
            this.tlpLoopCNT = new WinsockPacketEditor.TableLayoutPanelEx();
            this.dLoopCNT = new AntdUI.Divider();
            this.nudLoopCNT = new AntdUI.InputNumber();
            this.pSendSocket = new AntdUI.Panel();
            this.tlpSendSocket = new WinsockPacketEditor.TableLayoutPanelEx();
            this.dSendSocket = new AntdUI.Divider();
            this.cbSystemSocket = new AntdUI.Checkbox();
            this.pSendName = new AntdUI.Panel();
            this.tlpSendName = new WinsockPacketEditor.TableLayoutPanelEx();
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
            this.tlpSendEdit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
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
            this.tlpSendEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpSendEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSendEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            this.tlpSendEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            this.tlpSendEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tlpSendEdit.Size = new System.Drawing.Size(1100, 700);
            this.tlpSendEdit.TabIndex = 1;
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 9;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Controls.Add(this.bStop, 3, 1);
            this.tlpButton.Controls.Add(this.bExecute, 1, 1);
            this.tlpButton.Controls.Add(this.bSave, 5, 1);
            this.tlpButton.Controls.Add(this.bExit, 7, 1);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 651);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(1100, 49);
            this.tlpButton.TabIndex = 10;
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
            this.bStop.Location = new System.Drawing.Point(478, 6);
            this.bStop.Margin = new System.Windows.Forms.Padding(2);
            this.bStop.Name = "bStop";
            this.bStop.Size = new System.Drawing.Size(63, 37);
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
            this.bExecute.Location = new System.Drawing.Point(397, 6);
            this.bExecute.Margin = new System.Windows.Forms.Padding(2);
            this.bExecute.Name = "bExecute";
            this.bExecute.Size = new System.Drawing.Size(63, 37);
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
            this.bSave.Location = new System.Drawing.Point(559, 6);
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
            this.bExit.Location = new System.Drawing.Point(640, 6);
            this.bExit.Margin = new System.Windows.Forms.Padding(2);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(63, 37);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
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
            this.tlpSendCollectionInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpSendCollectionInfo.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 14F));
            this.tlpSendCollectionInfo.Controls.Add(this.ddMenu, 9, 0);
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
            this.tlpSendCollectionInfo.RowCount = 2;
            this.tlpSendCollectionInfo.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpSendCollectionInfo.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSendCollectionInfo.Size = new System.Drawing.Size(1100, 40);
            this.tlpSendCollectionInfo.TabIndex = 6;
            // 
            // ddMenu
            // 
            this.ddMenu.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.ddMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ddMenu.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ddMenu.Ghost = true;
            this.ddMenu.IconRatio = 1F;
            this.ddMenu.IconSvg = "PlusOutlined";
            this.ddMenu.Location = new System.Drawing.Point(1066, 2);
            this.ddMenu.Margin = new System.Windows.Forms.Padding(2);
            this.ddMenu.MaxCount = 10;
            this.ddMenu.Name = "ddMenu";
            this.ddMenu.Placement = AntdUI.TAlignFrom.BR;
            this.ddMenu.Size = new System.Drawing.Size(32, 37);
            this.ddMenu.TabIndex = 18;
            this.ddMenu.Trigger = AntdUI.Trigger.Hover;
            this.ddMenu.WaveSize = 0;
            this.ddMenu.SelectedValueChanged += new AntdUI.ObjectNEventHandler(this.ddMenu_SelectedValueChanged);
            // 
            // lSend_Fail_CNT
            // 
            this.lSend_Fail_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSend_Fail_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSend_Fail_CNT.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSend_Fail_CNT.ForeColor = System.Drawing.Color.Red;
            this.lSend_Fail_CNT.Location = new System.Drawing.Point(173, 2);
            this.lSend_Fail_CNT.Margin = new System.Windows.Forms.Padding(2);
            this.lSend_Fail_CNT.Name = "lSend_Fail_CNT";
            this.lSend_Fail_CNT.Size = new System.Drawing.Size(8, 37);
            this.lSend_Fail_CNT.TabIndex = 14;
            this.lSend_Fail_CNT.Text = "0";
            // 
            // lSend_Success_CNT
            // 
            this.lSend_Success_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSend_Success_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSend_Success_CNT.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSend_Success_CNT.ForeColor = System.Drawing.Color.Green;
            this.lSend_Success_CNT.Location = new System.Drawing.Point(117, 2);
            this.lSend_Success_CNT.Margin = new System.Windows.Forms.Padding(2);
            this.lSend_Success_CNT.Name = "lSend_Success_CNT";
            this.lSend_Success_CNT.Size = new System.Drawing.Size(8, 37);
            this.lSend_Success_CNT.TabIndex = 13;
            this.lSend_Success_CNT.Text = "0";
            // 
            // lTotal_Send_CNT
            // 
            this.lTotal_Send_CNT.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lTotal_Send_CNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lTotal_Send_CNT.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lTotal_Send_CNT.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(119)))), ((int)(((byte)(255)))));
            this.lTotal_Send_CNT.Location = new System.Drawing.Point(61, 2);
            this.lTotal_Send_CNT.Margin = new System.Windows.Forms.Padding(2);
            this.lTotal_Send_CNT.Name = "lTotal_Send_CNT";
            this.lTotal_Send_CNT.Size = new System.Drawing.Size(8, 37);
            this.lTotal_Send_CNT.TabIndex = 12;
            this.lTotal_Send_CNT.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.Silver;
            this.label3.Location = new System.Drawing.Point(129, 2);
            this.label3.Margin = new System.Windows.Forms.Padding(2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(5, 37);
            this.label3.TabIndex = 9;
            this.label3.Text = "|";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Silver;
            this.label4.Location = new System.Drawing.Point(73, 2);
            this.label4.Margin = new System.Windows.Forms.Padding(2);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(5, 37);
            this.label4.TabIndex = 8;
            this.label4.Text = "|";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lSend_Fail
            // 
            this.lSend_Fail.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSend_Fail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSend_Fail.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSend_Fail.LocalizationText = "SendEditForm.SendFailure";
            this.lSend_Fail.Location = new System.Drawing.Point(138, 2);
            this.lSend_Fail.Margin = new System.Windows.Forms.Padding(2);
            this.lSend_Fail.Name = "lSend_Fail";
            this.lSend_Fail.Size = new System.Drawing.Size(31, 37);
            this.lSend_Fail.TabIndex = 7;
            this.lSend_Fail.Text = "失败 :";
            // 
            // lSend_Success
            // 
            this.lSend_Success.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lSend_Success.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lSend_Success.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lSend_Success.LocalizationText = "SendEditForm.SendSuccess";
            this.lSend_Success.Location = new System.Drawing.Point(82, 2);
            this.lSend_Success.Margin = new System.Windows.Forms.Padding(2);
            this.lSend_Success.Name = "lSend_Success";
            this.lSend_Success.Size = new System.Drawing.Size(31, 37);
            this.lSend_Success.TabIndex = 6;
            this.lSend_Success.Text = "成功 :";
            // 
            // lTotal_Send
            // 
            this.lTotal_Send.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.lTotal_Send.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lTotal_Send.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lTotal_Send.LocalizationText = "SendEditForm.TotalSend";
            this.lTotal_Send.Location = new System.Drawing.Point(2, 2);
            this.lTotal_Send.Margin = new System.Windows.Forms.Padding(2);
            this.lTotal_Send.Name = "lTotal_Send";
            this.lTotal_Send.Size = new System.Drawing.Size(55, 37);
            this.lTotal_Send.TabIndex = 5;
            this.lTotal_Send.Text = "发送总数 :";
            // 
            // tSendCollection
            // 
            this.tSendCollection.AutoSizeColumnsMode = AntdUI.ColumnsMode.Fill;
            this.tSendCollection.Bordered = true;
            this.tSendCollection.CellImpactHeight = false;
            this.tSendCollection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tSendCollection.EmptyHeader = true;
            this.tSendCollection.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tSendCollection.Gap = 10;
            this.tSendCollection.GapCell = 5;
            this.tSendCollection.Gaps = new System.Drawing.Size(10, 10);
            this.tSendCollection.Location = new System.Drawing.Point(2, 42);
            this.tSendCollection.Margin = new System.Windows.Forms.Padding(2);
            this.tSendCollection.MultipleRows = true;
            this.tSendCollection.Name = "tSendCollection";
            this.tSendCollection.Size = new System.Drawing.Size(1096, 445);
            this.tSendCollection.TabIndex = 7;
            this.tSendCollection.CellClick += new AntdUI.Table.ClickEventHandler(this.tSendCollection_CellClick);
            this.tSendCollection.CellButtonClick += new AntdUI.Table.ClickButtonEventHandler(this.tSendCollection_CellButtonClick);
            this.tSendCollection.CellDoubleClick += new AntdUI.Table.ClickEventHandler(this.tSendCollection_CellDoubleClick);
            // 
            // txtNotes
            // 
            this.txtNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNotes.LocalizationPlaceholderText = "SendEditForm.Remarks";
            this.txtNotes.Location = new System.Drawing.Point(2, 572);
            this.txtNotes.Margin = new System.Windows.Forms.Padding(2);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.PlaceholderText = "备注信息";
            this.txtNotes.Size = new System.Drawing.Size(1096, 77);
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
            this.tlpSendCollectionSettings.Location = new System.Drawing.Point(0, 489);
            this.tlpSendCollectionSettings.Margin = new System.Windows.Forms.Padding(0);
            this.tlpSendCollectionSettings.Name = "tlpSendCollectionSettings";
            this.tlpSendCollectionSettings.RowCount = 1;
            this.tlpSendCollectionSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSendCollectionSettings.Size = new System.Drawing.Size(1100, 81);
            this.tlpSendCollectionSettings.TabIndex = 9;
            // 
            // pLoopINT
            // 
            this.pLoopINT.BorderWidth = 2F;
            this.pLoopINT.Controls.Add(this.tlpLoopINT);
            this.pLoopINT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pLoopINT.Location = new System.Drawing.Point(827, 2);
            this.pLoopINT.Margin = new System.Windows.Forms.Padding(2);
            this.pLoopINT.Name = "pLoopINT";
            this.pLoopINT.Radius = 10;
            this.pLoopINT.Size = new System.Drawing.Size(271, 77);
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
            this.tlpLoopINT.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpLoopINT.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpLoopINT.Size = new System.Drawing.Size(267, 73);
            this.tlpLoopINT.TabIndex = 0;
            // 
            // nudLoopINT
            // 
            this.nudLoopINT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudLoopINT.LocalizationSuffixText = "Millisecond";
            this.nudLoopINT.Location = new System.Drawing.Point(2, 30);
            this.nudLoopINT.Margin = new System.Windows.Forms.Padding(2);
            this.nudLoopINT.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nudLoopINT.Minimum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudLoopINT.Name = "nudLoopINT";
            this.nudLoopINT.Size = new System.Drawing.Size(263, 32);
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
            this.dLoopINT.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dLoopINT.LocalizationText = "SendEditForm.SendInterval";
            this.dLoopINT.Location = new System.Drawing.Point(2, 2);
            this.dLoopINT.Margin = new System.Windows.Forms.Padding(2);
            this.dLoopINT.Name = "dLoopINT";
            this.dLoopINT.Orientation = AntdUI.TOrientation.Left;
            this.dLoopINT.Size = new System.Drawing.Size(263, 24);
            this.dLoopINT.TabIndex = 0;
            this.dLoopINT.Text = "发送间隔";
            // 
            // pLoopCNT
            // 
            this.pLoopCNT.BorderWidth = 2F;
            this.pLoopCNT.Controls.Add(this.tlpLoopCNT);
            this.pLoopCNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pLoopCNT.Location = new System.Drawing.Point(552, 2);
            this.pLoopCNT.Margin = new System.Windows.Forms.Padding(2);
            this.pLoopCNT.Name = "pLoopCNT";
            this.pLoopCNT.Radius = 10;
            this.pLoopCNT.Size = new System.Drawing.Size(271, 77);
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
            this.tlpLoopCNT.Size = new System.Drawing.Size(267, 73);
            this.tlpLoopCNT.TabIndex = 0;
            // 
            // dLoopCNT
            // 
            this.dLoopCNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dLoopCNT.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dLoopCNT.LocalizationText = "SendEditForm.LoopCount";
            this.dLoopCNT.Location = new System.Drawing.Point(2, 2);
            this.dLoopCNT.Margin = new System.Windows.Forms.Padding(2);
            this.dLoopCNT.Name = "dLoopCNT";
            this.dLoopCNT.Orientation = AntdUI.TOrientation.Left;
            this.dLoopCNT.Size = new System.Drawing.Size(263, 24);
            this.dLoopCNT.TabIndex = 0;
            this.dLoopCNT.Text = "循环次数";
            // 
            // nudLoopCNT
            // 
            this.nudLoopCNT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudLoopCNT.LocalizationSuffixText = "Times";
            this.nudLoopCNT.Location = new System.Drawing.Point(2, 30);
            this.nudLoopCNT.Margin = new System.Windows.Forms.Padding(2);
            this.nudLoopCNT.Maximum = new decimal(new int[] {
            99999999,
            0,
            0,
            0});
            this.nudLoopCNT.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudLoopCNT.Name = "nudLoopCNT";
            this.nudLoopCNT.SelectionStart = 1;
            this.nudLoopCNT.Size = new System.Drawing.Size(263, 32);
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
            this.pSendSocket.Location = new System.Drawing.Point(277, 2);
            this.pSendSocket.Margin = new System.Windows.Forms.Padding(2);
            this.pSendSocket.Name = "pSendSocket";
            this.pSendSocket.Radius = 10;
            this.pSendSocket.Size = new System.Drawing.Size(271, 77);
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
            this.tlpSendSocket.Size = new System.Drawing.Size(267, 73);
            this.tlpSendSocket.TabIndex = 0;
            // 
            // dSendSocket
            // 
            this.dSendSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dSendSocket.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dSendSocket.LocalizationText = "Socket";
            this.dSendSocket.Location = new System.Drawing.Point(2, 2);
            this.dSendSocket.Margin = new System.Windows.Forms.Padding(2);
            this.dSendSocket.Name = "dSendSocket";
            this.dSendSocket.Orientation = AntdUI.TOrientation.Left;
            this.dSendSocket.Size = new System.Drawing.Size(263, 24);
            this.dSendSocket.TabIndex = 0;
            this.dSendSocket.Text = "套接字";
            // 
            // cbSystemSocket
            // 
            this.cbSystemSocket.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbSystemSocket.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbSystemSocket.LocalizationText = "SendEditForm.UseSSocket";
            this.cbSystemSocket.Location = new System.Drawing.Point(2, 30);
            this.cbSystemSocket.Margin = new System.Windows.Forms.Padding(2);
            this.cbSystemSocket.Name = "cbSystemSocket";
            this.cbSystemSocket.Size = new System.Drawing.Size(116, 32);
            this.cbSystemSocket.TabIndex = 1;
            this.cbSystemSocket.Text = "使用系统套接字";
            // 
            // pSendName
            // 
            this.pSendName.BorderWidth = 2F;
            this.pSendName.Controls.Add(this.tlpSendName);
            this.pSendName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pSendName.Location = new System.Drawing.Point(2, 2);
            this.pSendName.Margin = new System.Windows.Forms.Padding(2);
            this.pSendName.Name = "pSendName";
            this.pSendName.Radius = 10;
            this.pSendName.Size = new System.Drawing.Size(271, 77);
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
            this.tlpSendName.Size = new System.Drawing.Size(267, 73);
            this.tlpSendName.TabIndex = 0;
            // 
            // dSendName
            // 
            this.dSendName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dSendName.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dSendName.LocalizationText = "Table.SendList.Column.SName";
            this.dSendName.Location = new System.Drawing.Point(2, 2);
            this.dSendName.Margin = new System.Windows.Forms.Padding(2);
            this.dSendName.Name = "dSendName";
            this.dSendName.Orientation = AntdUI.TOrientation.Left;
            this.dSendName.Size = new System.Drawing.Size(263, 24);
            this.dSendName.TabIndex = 0;
            this.dSendName.Text = "发送名称";
            // 
            // txtSendName
            // 
            this.txtSendName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtSendName.LocalizationPlaceholderText = "Input.Text";
            this.txtSendName.Location = new System.Drawing.Point(2, 30);
            this.txtSendName.Margin = new System.Windows.Forms.Padding(2);
            this.txtSendName.Name = "txtSendName";
            this.txtSendName.PlaceholderText = "请输入字符";
            this.txtSendName.Size = new System.Drawing.Size(263, 32);
            this.txtSendName.TabIndex = 1;
            this.txtSendName.TextChanged += new System.EventHandler(this.txtSendName_TextChanged);
            // 
            // SendEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpSendEdit);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "SendEdit";
            this.Size = new System.Drawing.Size(1100, 700);
            this.Load += new System.EventHandler(this.SendEdit_Load);
            this.tlpSendEdit.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.tlpButton.PerformLayout();
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

        private TableLayoutPanelEx tlpSendEdit;
        private TableLayoutPanelEx tlpButton;
        private AntdUI.Button bStop;
        private AntdUI.Button bExecute;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private TableLayoutPanelEx tlpSendCollectionInfo;
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
        private TableLayoutPanelEx tlpSendCollectionSettings;
        private AntdUI.Panel pLoopINT;
        private TableLayoutPanelEx tlpLoopINT;
        private AntdUI.InputNumber nudLoopINT;
        private AntdUI.Divider dLoopINT;
        private AntdUI.Panel pLoopCNT;
        private TableLayoutPanelEx tlpLoopCNT;
        private AntdUI.Divider dLoopCNT;
        private AntdUI.InputNumber nudLoopCNT;
        private AntdUI.Panel pSendSocket;
        private TableLayoutPanelEx tlpSendSocket;
        private AntdUI.Divider dSendSocket;
        private AntdUI.Checkbox cbSystemSocket;
        private AntdUI.Panel pSendName;
        private TableLayoutPanelEx tlpSendName;
        private AntdUI.Divider dSendName;
        private AntdUI.Input txtSendName;
        private AntdUI.Dropdown ddMenu;
    }
}
