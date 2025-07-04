namespace WPE.InjectMode
{
    partial class FilterEditForm
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
            AntdUI.Tabs.StyleLine styleLine2 = new AntdUI.Tabs.StyleLine();
            AntdUI.Tabs.StyleLine styleLine1 = new AntdUI.Tabs.StyleLine();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilterEditForm));
            this.tlpFilterEdit = new System.Windows.Forms.TableLayoutPanel();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.tabFilterEdit = new AntdUI.Tabs();
            this.tpNormal = new AntdUI.TabPage();
            this.tlpNormal = new System.Windows.Forms.TableLayoutPanel();
            this.tFilterNormal = new AntdUI.Table();
            this.tpAdvance = new AntdUI.TabPage();
            this.tlpAdvance = new System.Windows.Forms.TableLayoutPanel();
            this.tFilterAdvanced_Search = new AntdUI.Table();
            this.tabFilterFrom = new AntdUI.Tabs();
            this.tpFromHead = new AntdUI.TabPage();
            this.tFilterAdvanced_Modify_Head = new AntdUI.Table();
            this.tpFromPosition = new AntdUI.TabPage();
            this.tFilterAdvanced_Modify_Position = new AntdUI.Table();
            this.tlpFilterSettings = new System.Windows.Forms.TableLayoutPanel();
            this.pFilterProgression = new AntdUI.Panel();
            this.tlpFilterProgression2 = new System.Windows.Forms.TableLayoutPanel();
            this.tlpFilterProgression = new System.Windows.Forms.TableLayoutPanel();
            this.cbProgressionContinuous = new AntdUI.Checkbox();
            this.cbProgressionCarry = new AntdUI.Checkbox();
            this.nudProgressionStep = new AntdUI.InputNumber();
            this.nudProgressionCarry = new AntdUI.InputNumber();
            this.label1 = new AntdUI.Label();
            this.label5 = new AntdUI.Label();
            this.dFilterProgression = new AntdUI.Divider();
            this.pFilterAppoint = new AntdUI.Panel();
            this.tlpFilterAppoint2 = new System.Windows.Forms.TableLayoutPanel();
            this.tlpFilterAppoint = new System.Windows.Forms.TableLayoutPanel();
            this.txtFilter_LengthContent = new AntdUI.Input();
            this.cbFilter_AppointLength = new AntdUI.Checkbox();
            this.cbFilter_AppointSocket = new AntdUI.Checkbox();
            this.cbFilter_AppointPort = new AntdUI.Checkbox();
            this.txtFilter_SocketContent = new AntdUI.Input();
            this.txtFilter_PortContent = new AntdUI.Input();
            this.dFilterAppoint = new AntdUI.Divider();
            this.pFilterFunction = new AntdUI.Panel();
            this.tlpFilterFunction = new System.Windows.Forms.TableLayoutPanel();
            this.tlpFilterFunction2 = new System.Windows.Forms.TableLayoutPanel();
            this.cbFilterFunction_Send = new AntdUI.Checkbox();
            this.cbFilterFunction_WSARecvFrom = new AntdUI.Checkbox();
            this.cbFilterFunction_RecvFrom = new AntdUI.Checkbox();
            this.cbFilterFunction_WSARecv = new AntdUI.Checkbox();
            this.cbFilterFunction_Recv = new AntdUI.Checkbox();
            this.cbFilterFunction_WSASendTo = new AntdUI.Checkbox();
            this.cbFilterFunction_SendTo = new AntdUI.Checkbox();
            this.cbFilterFunction_WSASend = new AntdUI.Checkbox();
            this.dFilterFunction = new AntdUI.Divider();
            this.pFilterAction = new AntdUI.Panel();
            this.tlpFilterAction = new System.Windows.Forms.TableLayoutPanel();
            this.tlpFilterAction2 = new System.Windows.Forms.TableLayoutPanel();
            this.rbFilterAction_Replace = new AntdUI.Radio();
            this.rbFilterAction_Change = new AntdUI.Radio();
            this.rbFilterAction_NoModify_Display = new AntdUI.Radio();
            this.rbFilterAction_Intercept = new AntdUI.Radio();
            this.rbFilterAction_NoModify_NoDisplay = new AntdUI.Radio();
            this.cbFilterAction_Execute = new AntdUI.Checkbox();
            this.cbbFilterAction_ExecuteType = new AntdUI.Select();
            this.cbbFilterAction_Execute = new AntdUI.Select();
            this.dFilterAction = new AntdUI.Divider();
            this.tlpFilterFromAndMode = new System.Windows.Forms.TableLayoutPanel();
            this.pFilterMode = new AntdUI.Panel();
            this.tlpFilterMode = new System.Windows.Forms.TableLayoutPanel();
            this.rbFilterMode_Advanced = new AntdUI.Radio();
            this.dFilterMode = new AntdUI.Divider();
            this.rbFilterMode_Normal = new AntdUI.Radio();
            this.pFilterModifyFrom = new AntdUI.Panel();
            this.tlpFilterModifyFrom = new System.Windows.Forms.TableLayoutPanel();
            this.rbFilterModifyFrom_Position = new AntdUI.Radio();
            this.rbFilterModifyFrom_Head = new AntdUI.Radio();
            this.dFilterModifyFrom = new AntdUI.Divider();
            this.tlpFilterNameAndAppoint = new System.Windows.Forms.TableLayoutPanel();
            this.pFilterAppoint_Advance = new AntdUI.Panel();
            this.tlpFilterAppoint_Advance = new System.Windows.Forms.TableLayoutPanel();
            this.tlpFilterAppoint_Advance2 = new System.Windows.Forms.TableLayoutPanel();
            this.cbFilter_AppointHeader = new AntdUI.Checkbox();
            this.dFilterAppoint_Advance = new AntdUI.Divider();
            this.pFilterName = new AntdUI.Panel();
            this.tlpFilterName = new System.Windows.Forms.TableLayoutPanel();
            this.dFilterName = new AntdUI.Divider();
            this.txtFilterName = new AntdUI.Input();
            this.txtFilter_HeaderContent = new AntdUI.Input();
            this.tlpFilterEdit.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tabFilterEdit.SuspendLayout();
            this.tpNormal.SuspendLayout();
            this.tlpNormal.SuspendLayout();
            this.tpAdvance.SuspendLayout();
            this.tlpAdvance.SuspendLayout();
            this.tabFilterFrom.SuspendLayout();
            this.tpFromHead.SuspendLayout();
            this.tpFromPosition.SuspendLayout();
            this.tlpFilterSettings.SuspendLayout();
            this.pFilterProgression.SuspendLayout();
            this.tlpFilterProgression2.SuspendLayout();
            this.tlpFilterProgression.SuspendLayout();
            this.pFilterAppoint.SuspendLayout();
            this.tlpFilterAppoint2.SuspendLayout();
            this.tlpFilterAppoint.SuspendLayout();
            this.pFilterFunction.SuspendLayout();
            this.tlpFilterFunction.SuspendLayout();
            this.tlpFilterFunction2.SuspendLayout();
            this.pFilterAction.SuspendLayout();
            this.tlpFilterAction.SuspendLayout();
            this.tlpFilterAction2.SuspendLayout();
            this.tlpFilterFromAndMode.SuspendLayout();
            this.pFilterMode.SuspendLayout();
            this.tlpFilterMode.SuspendLayout();
            this.pFilterModifyFrom.SuspendLayout();
            this.tlpFilterModifyFrom.SuspendLayout();
            this.tlpFilterNameAndAppoint.SuspendLayout();
            this.pFilterAppoint_Advance.SuspendLayout();
            this.tlpFilterAppoint_Advance.SuspendLayout();
            this.tlpFilterAppoint_Advance2.SuspendLayout();
            this.pFilterName.SuspendLayout();
            this.tlpFilterName.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpFilterEdit
            // 
            this.tlpFilterEdit.ColumnCount = 1;
            this.tlpFilterEdit.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterEdit.Controls.Add(this.tlpButton, 0, 2);
            this.tlpFilterEdit.Controls.Add(this.tabFilterEdit, 0, 0);
            this.tlpFilterEdit.Controls.Add(this.tlpFilterSettings, 0, 1);
            this.tlpFilterEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterEdit.Location = new System.Drawing.Point(0, 0);
            this.tlpFilterEdit.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFilterEdit.Name = "tlpFilterEdit";
            this.tlpFilterEdit.RowCount = 3;
            this.tlpFilterEdit.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpFilterEdit.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpFilterEdit.Size = new System.Drawing.Size(984, 761);
            this.tlpFilterEdit.TabIndex = 1;
            // 
            // tlpButton
            // 
            this.tlpButton.ColumnCount = 5;
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 120F));
            this.tlpButton.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Controls.Add(this.bSave, 1, 1);
            this.tlpButton.Controls.Add(this.bExit, 3, 1);
            this.tlpButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpButton.Location = new System.Drawing.Point(0, 701);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(984, 60);
            this.tlpButton.TabIndex = 4;
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
            this.bSave.Location = new System.Drawing.Point(365, 7);
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
            this.bExit.Location = new System.Drawing.Point(505, 7);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(114, 46);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // tabFilterEdit
            // 
            this.tabFilterEdit.Controls.Add(this.tpNormal);
            this.tabFilterEdit.Controls.Add(this.tpAdvance);
            this.tabFilterEdit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tabFilterEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabFilterEdit.Location = new System.Drawing.Point(3, 3);
            this.tabFilterEdit.Name = "tabFilterEdit";
            this.tabFilterEdit.Pages.Add(this.tpNormal);
            this.tabFilterEdit.Pages.Add(this.tpAdvance);
            this.tabFilterEdit.SelectedIndex = 1;
            this.tabFilterEdit.Size = new System.Drawing.Size(978, 250);
            this.tabFilterEdit.Style = styleLine2;
            this.tabFilterEdit.TabIndex = 0;
            // 
            // tpNormal
            // 
            this.tpNormal.Controls.Add(this.tlpNormal);
            this.tpNormal.Location = new System.Drawing.Point(-972, -214);
            this.tpNormal.Name = "tpNormal";
            this.tpNormal.Size = new System.Drawing.Size(972, 214);
            this.tpNormal.TabIndex = 1;
            this.tpNormal.Text = "Normal";
            // 
            // tlpNormal
            // 
            this.tlpNormal.ColumnCount = 1;
            this.tlpNormal.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpNormal.Controls.Add(this.tFilterNormal, 0, 0);
            this.tlpNormal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpNormal.Location = new System.Drawing.Point(0, 0);
            this.tlpNormal.Margin = new System.Windows.Forms.Padding(0);
            this.tlpNormal.Name = "tlpNormal";
            this.tlpNormal.RowCount = 1;
            this.tlpNormal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpNormal.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 214F));
            this.tlpNormal.Size = new System.Drawing.Size(972, 214);
            this.tlpNormal.TabIndex = 0;
            // 
            // tFilterNormal
            // 
            this.tFilterNormal.Bordered = true;
            this.tFilterNormal.CellImpactHeight = true;
            this.tFilterNormal.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tFilterNormal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tFilterNormal.EditInputStyle = AntdUI.TEditInputStyle.Full;
            this.tFilterNormal.EditMode = AntdUI.TEditMode.DoubleClick;
            this.tFilterNormal.Gap = 8;
            this.tFilterNormal.GapCell = 0;
            this.tFilterNormal.Location = new System.Drawing.Point(3, 3);
            this.tFilterNormal.Name = "tFilterNormal";
            this.tFilterNormal.RowHeight = 40;
            this.tFilterNormal.RowSelectedBg = System.Drawing.Color.Transparent;
            this.tFilterNormal.Size = new System.Drawing.Size(966, 208);
            this.tFilterNormal.TabIndex = 1;
            this.tFilterNormal.CellClick += new AntdUI.Table.ClickEventHandler(this.tFilterNormal_CellClick);
            this.tFilterNormal.CellBeginEditInputStyle += new AntdUI.Table.BeginEditInputStyleEventHandler(this.tFilterNormal_CellBeginEditInputStyle);
            this.tFilterNormal.CellEndEdit += new AntdUI.Table.EndEditEventHandler(this.tFilterNormal_CellEndEdit);
            this.tFilterNormal.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tFilterNormal_KeyDown);
            // 
            // tpAdvance
            // 
            this.tpAdvance.Controls.Add(this.tlpAdvance);
            this.tpAdvance.Location = new System.Drawing.Point(3, 33);
            this.tpAdvance.Name = "tpAdvance";
            this.tpAdvance.Size = new System.Drawing.Size(972, 214);
            this.tpAdvance.TabIndex = 0;
            this.tpAdvance.Text = "Advance";
            // 
            // tlpAdvance
            // 
            this.tlpAdvance.ColumnCount = 1;
            this.tlpAdvance.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpAdvance.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpAdvance.Controls.Add(this.tFilterAdvanced_Search, 0, 0);
            this.tlpAdvance.Controls.Add(this.tabFilterFrom, 0, 1);
            this.tlpAdvance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpAdvance.Location = new System.Drawing.Point(0, 0);
            this.tlpAdvance.Margin = new System.Windows.Forms.Padding(0);
            this.tlpAdvance.Name = "tlpAdvance";
            this.tlpAdvance.RowCount = 2;
            this.tlpAdvance.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpAdvance.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpAdvance.Size = new System.Drawing.Size(972, 214);
            this.tlpAdvance.TabIndex = 0;
            // 
            // tFilterAdvanced_Search
            // 
            this.tFilterAdvanced_Search.Bordered = true;
            this.tFilterAdvanced_Search.CellImpactHeight = true;
            this.tFilterAdvanced_Search.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tFilterAdvanced_Search.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tFilterAdvanced_Search.EditInputStyle = AntdUI.TEditInputStyle.Full;
            this.tFilterAdvanced_Search.EditMode = AntdUI.TEditMode.DoubleClick;
            this.tFilterAdvanced_Search.Gap = 8;
            this.tFilterAdvanced_Search.GapCell = 0;
            this.tFilterAdvanced_Search.Location = new System.Drawing.Point(3, 3);
            this.tFilterAdvanced_Search.Name = "tFilterAdvanced_Search";
            this.tFilterAdvanced_Search.RowHeight = 40;
            this.tFilterAdvanced_Search.RowSelectedBg = System.Drawing.Color.Transparent;
            this.tFilterAdvanced_Search.Size = new System.Drawing.Size(966, 101);
            this.tFilterAdvanced_Search.TabIndex = 1;
            this.tFilterAdvanced_Search.CellClick += new AntdUI.Table.ClickEventHandler(this.tFilterAdvanced_Search_CellClick);
            this.tFilterAdvanced_Search.CellBeginEditInputStyle += new AntdUI.Table.BeginEditInputStyleEventHandler(this.tFilterAdvanced_Search_CellBeginEditInputStyle);
            this.tFilterAdvanced_Search.CellEndEdit += new AntdUI.Table.EndEditEventHandler(this.tFilterAdvanced_Search_CellEndEdit);
            this.tFilterAdvanced_Search.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tFilterAdvanced_Search_KeyDown);
            // 
            // tabFilterFrom
            // 
            this.tabFilterFrom.Controls.Add(this.tpFromHead);
            this.tabFilterFrom.Controls.Add(this.tpFromPosition);
            this.tabFilterFrom.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tabFilterFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabFilterFrom.Location = new System.Drawing.Point(3, 110);
            this.tabFilterFrom.Name = "tabFilterFrom";
            this.tabFilterFrom.Pages.Add(this.tpFromHead);
            this.tabFilterFrom.Pages.Add(this.tpFromPosition);
            this.tabFilterFrom.Size = new System.Drawing.Size(966, 101);
            this.tabFilterFrom.Style = styleLine1;
            this.tabFilterFrom.TabIndex = 2;
            this.tabFilterFrom.Text = "tabs1";
            // 
            // tpFromHead
            // 
            this.tpFromHead.Controls.Add(this.tFilterAdvanced_Modify_Head);
            this.tpFromHead.Location = new System.Drawing.Point(3, 33);
            this.tpFromHead.Name = "tpFromHead";
            this.tpFromHead.Size = new System.Drawing.Size(960, 65);
            this.tpFromHead.TabIndex = 0;
            this.tpFromHead.Text = "Head";
            // 
            // tFilterAdvanced_Modify_Head
            // 
            this.tFilterAdvanced_Modify_Head.Bordered = true;
            this.tFilterAdvanced_Modify_Head.CellImpactHeight = true;
            this.tFilterAdvanced_Modify_Head.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tFilterAdvanced_Modify_Head.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tFilterAdvanced_Modify_Head.EditInputStyle = AntdUI.TEditInputStyle.Full;
            this.tFilterAdvanced_Modify_Head.EditMode = AntdUI.TEditMode.DoubleClick;
            this.tFilterAdvanced_Modify_Head.Gap = 8;
            this.tFilterAdvanced_Modify_Head.GapCell = 0;
            this.tFilterAdvanced_Modify_Head.Location = new System.Drawing.Point(0, 0);
            this.tFilterAdvanced_Modify_Head.Name = "tFilterAdvanced_Modify_Head";
            this.tFilterAdvanced_Modify_Head.RowHeight = 40;
            this.tFilterAdvanced_Modify_Head.RowSelectedBg = System.Drawing.Color.Transparent;
            this.tFilterAdvanced_Modify_Head.Size = new System.Drawing.Size(960, 65);
            this.tFilterAdvanced_Modify_Head.TabIndex = 2;
            this.tFilterAdvanced_Modify_Head.CellClick += new AntdUI.Table.ClickEventHandler(this.tFilterAdvanced_Modify_Head_CellClick);
            this.tFilterAdvanced_Modify_Head.CellBeginEditInputStyle += new AntdUI.Table.BeginEditInputStyleEventHandler(this.tFilterAdvanced_Modify_Head_CellBeginEditInputStyle);
            this.tFilterAdvanced_Modify_Head.CellEndEdit += new AntdUI.Table.EndEditEventHandler(this.tFilterAdvanced_Modify_Head_CellEndEdit);
            this.tFilterAdvanced_Modify_Head.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tFilterAdvanced_Modify_Head_KeyDown);
            // 
            // tpFromPosition
            // 
            this.tpFromPosition.Controls.Add(this.tFilterAdvanced_Modify_Position);
            this.tpFromPosition.Location = new System.Drawing.Point(-960, -65);
            this.tpFromPosition.Name = "tpFromPosition";
            this.tpFromPosition.Size = new System.Drawing.Size(960, 65);
            this.tpFromPosition.TabIndex = 1;
            this.tpFromPosition.Text = "Position";
            // 
            // tFilterAdvanced_Modify_Position
            // 
            this.tFilterAdvanced_Modify_Position.Bordered = true;
            this.tFilterAdvanced_Modify_Position.CellImpactHeight = true;
            this.tFilterAdvanced_Modify_Position.Cursor = System.Windows.Forms.Cursors.Hand;
            this.tFilterAdvanced_Modify_Position.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tFilterAdvanced_Modify_Position.EditInputStyle = AntdUI.TEditInputStyle.Full;
            this.tFilterAdvanced_Modify_Position.EditMode = AntdUI.TEditMode.DoubleClick;
            this.tFilterAdvanced_Modify_Position.Gap = 8;
            this.tFilterAdvanced_Modify_Position.GapCell = 0;
            this.tFilterAdvanced_Modify_Position.Location = new System.Drawing.Point(0, 0);
            this.tFilterAdvanced_Modify_Position.Name = "tFilterAdvanced_Modify_Position";
            this.tFilterAdvanced_Modify_Position.RowHeight = 40;
            this.tFilterAdvanced_Modify_Position.RowSelectedBg = System.Drawing.Color.Transparent;
            this.tFilterAdvanced_Modify_Position.Size = new System.Drawing.Size(960, 65);
            this.tFilterAdvanced_Modify_Position.TabIndex = 2;
            this.tFilterAdvanced_Modify_Position.CellClick += new AntdUI.Table.ClickEventHandler(this.tFilterAdvanced_Modify_Position_CellClick);
            this.tFilterAdvanced_Modify_Position.CellBeginEditInputStyle += new AntdUI.Table.BeginEditInputStyleEventHandler(this.tFilterAdvanced_Modify_Position_CellBeginEditInputStyle);
            this.tFilterAdvanced_Modify_Position.CellEndEdit += new AntdUI.Table.EndEditEventHandler(this.tFilterAdvanced_Modify_Position_CellEndEdit);
            this.tFilterAdvanced_Modify_Position.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tFilterAdvanced_Modify_Position_KeyDown);
            // 
            // tlpFilterSettings
            // 
            this.tlpFilterSettings.ColumnCount = 3;
            this.tlpFilterSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tlpFilterSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this.tlpFilterSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tlpFilterSettings.Controls.Add(this.pFilterProgression, 2, 1);
            this.tlpFilterSettings.Controls.Add(this.pFilterAppoint, 0, 1);
            this.tlpFilterSettings.Controls.Add(this.pFilterFunction, 2, 0);
            this.tlpFilterSettings.Controls.Add(this.pFilterAction, 0, 0);
            this.tlpFilterSettings.Controls.Add(this.tlpFilterFromAndMode, 1, 1);
            this.tlpFilterSettings.Controls.Add(this.tlpFilterNameAndAppoint, 1, 0);
            this.tlpFilterSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterSettings.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tlpFilterSettings.Location = new System.Drawing.Point(0, 256);
            this.tlpFilterSettings.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFilterSettings.Name = "tlpFilterSettings";
            this.tlpFilterSettings.RowCount = 2;
            this.tlpFilterSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tlpFilterSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tlpFilterSettings.Size = new System.Drawing.Size(984, 445);
            this.tlpFilterSettings.TabIndex = 1;
            // 
            // pFilterProgression
            // 
            this.pFilterProgression.BorderWidth = 2F;
            this.pFilterProgression.Controls.Add(this.tlpFilterProgression2);
            this.pFilterProgression.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pFilterProgression.Location = new System.Drawing.Point(691, 247);
            this.pFilterProgression.Name = "pFilterProgression";
            this.pFilterProgression.Radius = 10;
            this.pFilterProgression.Size = new System.Drawing.Size(290, 195);
            this.pFilterProgression.TabIndex = 10;
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
            this.tlpFilterProgression2.Size = new System.Drawing.Size(286, 191);
            this.tlpFilterProgression2.TabIndex = 0;
            // 
            // tlpFilterProgression
            // 
            this.tlpFilterProgression.ColumnCount = 3;
            this.tlpFilterProgression.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpFilterProgression.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterProgression.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpFilterProgression.Controls.Add(this.cbProgressionContinuous, 0, 0);
            this.tlpFilterProgression.Controls.Add(this.cbProgressionCarry, 0, 1);
            this.tlpFilterProgression.Controls.Add(this.nudProgressionStep, 1, 0);
            this.tlpFilterProgression.Controls.Add(this.nudProgressionCarry, 1, 1);
            this.tlpFilterProgression.Controls.Add(this.label1, 2, 0);
            this.tlpFilterProgression.Controls.Add(this.label5, 2, 1);
            this.tlpFilterProgression.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterProgression.Location = new System.Drawing.Point(0, 36);
            this.tlpFilterProgression.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFilterProgression.Name = "tlpFilterProgression";
            this.tlpFilterProgression.Padding = new System.Windows.Forms.Padding(3);
            this.tlpFilterProgression.RowCount = 3;
            this.tlpFilterProgression.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterProgression.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterProgression.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterProgression.Size = new System.Drawing.Size(286, 155);
            this.tlpFilterProgression.TabIndex = 1;
            // 
            // cbProgressionContinuous
            // 
            this.cbProgressionContinuous.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbProgressionContinuous.Location = new System.Drawing.Point(6, 6);
            this.cbProgressionContinuous.Name = "cbProgressionContinuous";
            this.cbProgressionContinuous.Size = new System.Drawing.Size(95, 39);
            this.cbProgressionContinuous.TabIndex = 40;
            this.cbProgressionContinuous.Text = "连续递进";
            // 
            // cbProgressionCarry
            // 
            this.cbProgressionCarry.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbProgressionCarry.Location = new System.Drawing.Point(6, 51);
            this.cbProgressionCarry.Name = "cbProgressionCarry";
            this.cbProgressionCarry.Size = new System.Drawing.Size(95, 39);
            this.cbProgressionCarry.TabIndex = 41;
            this.cbProgressionCarry.Text = "进位递进";
            this.cbProgressionCarry.CheckedChanged += new AntdUI.BoolEventHandler(this.cbProgressionCarry_CheckedChanged);
            // 
            // nudProgressionStep
            // 
            this.nudProgressionStep.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudProgressionStep.Location = new System.Drawing.Point(107, 6);
            this.nudProgressionStep.Name = "nudProgressionStep";
            this.nudProgressionStep.Size = new System.Drawing.Size(139, 39);
            this.nudProgressionStep.TabIndex = 42;
            this.nudProgressionStep.Text = "1";
            this.nudProgressionStep.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudProgressionStep.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudProgressionCarry
            // 
            this.nudProgressionCarry.Dock = System.Windows.Forms.DockStyle.Fill;
            this.nudProgressionCarry.Location = new System.Drawing.Point(107, 51);
            this.nudProgressionCarry.Name = "nudProgressionCarry";
            this.nudProgressionCarry.Size = new System.Drawing.Size(139, 39);
            this.nudProgressionCarry.TabIndex = 43;
            this.nudProgressionCarry.Text = "1";
            this.nudProgressionCarry.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudProgressionCarry.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(252, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(28, 39);
            this.label1.TabIndex = 44;
            this.label1.Text = "步长";
            // 
            // label5
            // 
            this.label5.AutoSizeMode = AntdUI.TAutoSize.Width;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(252, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(28, 39);
            this.label5.TabIndex = 45;
            this.label5.Text = "位数";
            // 
            // dFilterProgression
            // 
            this.dFilterProgression.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dFilterProgression.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dFilterProgression.Location = new System.Drawing.Point(3, 3);
            this.dFilterProgression.Name = "dFilterProgression";
            this.dFilterProgression.Orientation = AntdUI.TOrientation.Left;
            this.dFilterProgression.Size = new System.Drawing.Size(280, 30);
            this.dFilterProgression.TabIndex = 0;
            this.dFilterProgression.Text = "递进";
            // 
            // pFilterAppoint
            // 
            this.pFilterAppoint.BorderWidth = 2F;
            this.pFilterAppoint.Controls.Add(this.tlpFilterAppoint2);
            this.pFilterAppoint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pFilterAppoint.Location = new System.Drawing.Point(3, 247);
            this.pFilterAppoint.Name = "pFilterAppoint";
            this.pFilterAppoint.Radius = 10;
            this.pFilterAppoint.Size = new System.Drawing.Size(338, 195);
            this.pFilterAppoint.TabIndex = 9;
            // 
            // tlpFilterAppoint2
            // 
            this.tlpFilterAppoint2.BackColor = System.Drawing.Color.Transparent;
            this.tlpFilterAppoint2.ColumnCount = 1;
            this.tlpFilterAppoint2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterAppoint2.Controls.Add(this.tlpFilterAppoint, 0, 1);
            this.tlpFilterAppoint2.Controls.Add(this.dFilterAppoint, 0, 0);
            this.tlpFilterAppoint2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterAppoint2.Location = new System.Drawing.Point(2, 2);
            this.tlpFilterAppoint2.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFilterAppoint2.Name = "tlpFilterAppoint2";
            this.tlpFilterAppoint2.RowCount = 2;
            this.tlpFilterAppoint2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterAppoint2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterAppoint2.Size = new System.Drawing.Size(334, 191);
            this.tlpFilterAppoint2.TabIndex = 0;
            // 
            // tlpFilterAppoint
            // 
            this.tlpFilterAppoint.ColumnCount = 2;
            this.tlpFilterAppoint.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpFilterAppoint.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterAppoint.Controls.Add(this.txtFilter_LengthContent, 1, 2);
            this.tlpFilterAppoint.Controls.Add(this.cbFilter_AppointLength, 0, 2);
            this.tlpFilterAppoint.Controls.Add(this.cbFilter_AppointSocket, 0, 0);
            this.tlpFilterAppoint.Controls.Add(this.cbFilter_AppointPort, 0, 1);
            this.tlpFilterAppoint.Controls.Add(this.txtFilter_SocketContent, 1, 0);
            this.tlpFilterAppoint.Controls.Add(this.txtFilter_PortContent, 1, 1);
            this.tlpFilterAppoint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterAppoint.Location = new System.Drawing.Point(0, 36);
            this.tlpFilterAppoint.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFilterAppoint.Name = "tlpFilterAppoint";
            this.tlpFilterAppoint.RowCount = 4;
            this.tlpFilterAppoint.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterAppoint.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterAppoint.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterAppoint.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterAppoint.Size = new System.Drawing.Size(334, 155);
            this.tlpFilterAppoint.TabIndex = 1;
            // 
            // txtFilter_LengthContent
            // 
            this.txtFilter_LengthContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFilter_LengthContent.Location = new System.Drawing.Point(90, 93);
            this.txtFilter_LengthContent.Name = "txtFilter_LengthContent";
            this.txtFilter_LengthContent.PlaceholderText = "例如 0-99;100";
            this.txtFilter_LengthContent.Size = new System.Drawing.Size(241, 39);
            this.txtFilter_LengthContent.TabIndex = 5;
            this.txtFilter_LengthContent.TextChanged += new System.EventHandler(this.txtFilter_LengthContent_TextChanged);
            // 
            // cbFilter_AppointLength
            // 
            this.cbFilter_AppointLength.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbFilter_AppointLength.Location = new System.Drawing.Point(3, 93);
            this.cbFilter_AppointLength.Name = "cbFilter_AppointLength";
            this.cbFilter_AppointLength.Size = new System.Drawing.Size(67, 39);
            this.cbFilter_AppointLength.TabIndex = 4;
            this.cbFilter_AppointLength.Text = "长度";
            this.cbFilter_AppointLength.CheckedChanged += new AntdUI.BoolEventHandler(this.cbFilter_AppointLength_CheckedChanged);
            // 
            // cbFilter_AppointSocket
            // 
            this.cbFilter_AppointSocket.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbFilter_AppointSocket.Location = new System.Drawing.Point(3, 3);
            this.cbFilter_AppointSocket.Name = "cbFilter_AppointSocket";
            this.cbFilter_AppointSocket.Size = new System.Drawing.Size(81, 39);
            this.cbFilter_AppointSocket.TabIndex = 0;
            this.cbFilter_AppointSocket.Text = "套接字";
            this.cbFilter_AppointSocket.CheckedChanged += new AntdUI.BoolEventHandler(this.cbFilter_AppointSocket_CheckedChanged);
            // 
            // cbFilter_AppointPort
            // 
            this.cbFilter_AppointPort.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbFilter_AppointPort.Location = new System.Drawing.Point(3, 48);
            this.cbFilter_AppointPort.Name = "cbFilter_AppointPort";
            this.cbFilter_AppointPort.Size = new System.Drawing.Size(67, 39);
            this.cbFilter_AppointPort.TabIndex = 1;
            this.cbFilter_AppointPort.Text = "端口";
            this.cbFilter_AppointPort.CheckedChanged += new AntdUI.BoolEventHandler(this.cbFilter_AppointPort_CheckedChanged);
            // 
            // txtFilter_SocketContent
            // 
            this.txtFilter_SocketContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFilter_SocketContent.Location = new System.Drawing.Point(90, 3);
            this.txtFilter_SocketContent.Name = "txtFilter_SocketContent";
            this.txtFilter_SocketContent.PlaceholderText = "支持 ; 分隔符";
            this.txtFilter_SocketContent.Size = new System.Drawing.Size(241, 39);
            this.txtFilter_SocketContent.TabIndex = 2;
            this.txtFilter_SocketContent.TextChanged += new System.EventHandler(this.txtFilter_SocketContent_TextChanged);
            // 
            // txtFilter_PortContent
            // 
            this.txtFilter_PortContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFilter_PortContent.Location = new System.Drawing.Point(90, 48);
            this.txtFilter_PortContent.Name = "txtFilter_PortContent";
            this.txtFilter_PortContent.PlaceholderText = "例如 80-89;1080";
            this.txtFilter_PortContent.Size = new System.Drawing.Size(241, 39);
            this.txtFilter_PortContent.TabIndex = 3;
            this.txtFilter_PortContent.TextChanged += new System.EventHandler(this.txtFilter_PortContent_TextChanged);
            // 
            // dFilterAppoint
            // 
            this.dFilterAppoint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dFilterAppoint.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dFilterAppoint.Location = new System.Drawing.Point(3, 3);
            this.dFilterAppoint.Name = "dFilterAppoint";
            this.dFilterAppoint.Orientation = AntdUI.TOrientation.Left;
            this.dFilterAppoint.Size = new System.Drawing.Size(328, 30);
            this.dFilterAppoint.TabIndex = 0;
            this.dFilterAppoint.Text = "指定类型";
            // 
            // pFilterFunction
            // 
            this.pFilterFunction.BorderWidth = 2F;
            this.pFilterFunction.Controls.Add(this.tlpFilterFunction);
            this.pFilterFunction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pFilterFunction.Location = new System.Drawing.Point(691, 3);
            this.pFilterFunction.Name = "pFilterFunction";
            this.pFilterFunction.Radius = 10;
            this.pFilterFunction.Size = new System.Drawing.Size(290, 238);
            this.pFilterFunction.TabIndex = 8;
            // 
            // tlpFilterFunction
            // 
            this.tlpFilterFunction.BackColor = System.Drawing.Color.Transparent;
            this.tlpFilterFunction.ColumnCount = 1;
            this.tlpFilterFunction.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterFunction.Controls.Add(this.tlpFilterFunction2, 0, 1);
            this.tlpFilterFunction.Controls.Add(this.dFilterFunction, 0, 0);
            this.tlpFilterFunction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterFunction.Location = new System.Drawing.Point(2, 2);
            this.tlpFilterFunction.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFilterFunction.Name = "tlpFilterFunction";
            this.tlpFilterFunction.RowCount = 2;
            this.tlpFilterFunction.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterFunction.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterFunction.Size = new System.Drawing.Size(286, 234);
            this.tlpFilterFunction.TabIndex = 0;
            // 
            // tlpFilterFunction2
            // 
            this.tlpFilterFunction2.ColumnCount = 3;
            this.tlpFilterFunction2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpFilterFunction2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterFunction2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpFilterFunction2.Controls.Add(this.cbFilterFunction_Send, 0, 0);
            this.tlpFilterFunction2.Controls.Add(this.cbFilterFunction_WSARecvFrom, 2, 3);
            this.tlpFilterFunction2.Controls.Add(this.cbFilterFunction_RecvFrom, 0, 3);
            this.tlpFilterFunction2.Controls.Add(this.cbFilterFunction_WSARecv, 2, 2);
            this.tlpFilterFunction2.Controls.Add(this.cbFilterFunction_Recv, 0, 2);
            this.tlpFilterFunction2.Controls.Add(this.cbFilterFunction_WSASendTo, 2, 1);
            this.tlpFilterFunction2.Controls.Add(this.cbFilterFunction_SendTo, 0, 1);
            this.tlpFilterFunction2.Controls.Add(this.cbFilterFunction_WSASend, 2, 0);
            this.tlpFilterFunction2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterFunction2.Location = new System.Drawing.Point(0, 36);
            this.tlpFilterFunction2.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFilterFunction2.Name = "tlpFilterFunction2";
            this.tlpFilterFunction2.Padding = new System.Windows.Forms.Padding(3);
            this.tlpFilterFunction2.RowCount = 5;
            this.tlpFilterFunction2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterFunction2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterFunction2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterFunction2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterFunction2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterFunction2.Size = new System.Drawing.Size(286, 198);
            this.tlpFilterFunction2.TabIndex = 3;
            // 
            // cbFilterFunction_Send
            // 
            this.cbFilterFunction_Send.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbFilterFunction_Send.Location = new System.Drawing.Point(6, 6);
            this.cbFilterFunction_Send.Name = "cbFilterFunction_Send";
            this.cbFilterFunction_Send.Size = new System.Drawing.Size(67, 39);
            this.cbFilterFunction_Send.TabIndex = 51;
            this.cbFilterFunction_Send.Text = "发送";
            // 
            // cbFilterFunction_WSARecvFrom
            // 
            this.cbFilterFunction_WSARecvFrom.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbFilterFunction_WSARecvFrom.Location = new System.Drawing.Point(166, 141);
            this.cbFilterFunction_WSARecvFrom.Name = "cbFilterFunction_WSARecvFrom";
            this.cbFilterFunction_WSARecvFrom.Size = new System.Drawing.Size(114, 39);
            this.cbFilterFunction_WSARecvFrom.TabIndex = 50;
            this.cbFilterFunction_WSARecvFrom.Text = "WSA接收自";
            // 
            // cbFilterFunction_RecvFrom
            // 
            this.cbFilterFunction_RecvFrom.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbFilterFunction_RecvFrom.Location = new System.Drawing.Point(6, 141);
            this.cbFilterFunction_RecvFrom.Name = "cbFilterFunction_RecvFrom";
            this.cbFilterFunction_RecvFrom.Size = new System.Drawing.Size(81, 39);
            this.cbFilterFunction_RecvFrom.TabIndex = 48;
            this.cbFilterFunction_RecvFrom.Text = "接收自";
            // 
            // cbFilterFunction_WSARecv
            // 
            this.cbFilterFunction_WSARecv.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbFilterFunction_WSARecv.Location = new System.Drawing.Point(166, 96);
            this.cbFilterFunction_WSARecv.Name = "cbFilterFunction_WSARecv";
            this.cbFilterFunction_WSARecv.Size = new System.Drawing.Size(100, 39);
            this.cbFilterFunction_WSARecv.TabIndex = 47;
            this.cbFilterFunction_WSARecv.Text = "WSA接收";
            // 
            // cbFilterFunction_Recv
            // 
            this.cbFilterFunction_Recv.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbFilterFunction_Recv.Location = new System.Drawing.Point(6, 96);
            this.cbFilterFunction_Recv.Name = "cbFilterFunction_Recv";
            this.cbFilterFunction_Recv.Size = new System.Drawing.Size(67, 39);
            this.cbFilterFunction_Recv.TabIndex = 45;
            this.cbFilterFunction_Recv.Text = "接收";
            // 
            // cbFilterFunction_WSASendTo
            // 
            this.cbFilterFunction_WSASendTo.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbFilterFunction_WSASendTo.Location = new System.Drawing.Point(166, 51);
            this.cbFilterFunction_WSASendTo.Name = "cbFilterFunction_WSASendTo";
            this.cbFilterFunction_WSASendTo.Size = new System.Drawing.Size(114, 39);
            this.cbFilterFunction_WSASendTo.TabIndex = 44;
            this.cbFilterFunction_WSASendTo.Text = "WSA发送到";
            // 
            // cbFilterFunction_SendTo
            // 
            this.cbFilterFunction_SendTo.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbFilterFunction_SendTo.Location = new System.Drawing.Point(6, 51);
            this.cbFilterFunction_SendTo.Name = "cbFilterFunction_SendTo";
            this.cbFilterFunction_SendTo.Size = new System.Drawing.Size(81, 39);
            this.cbFilterFunction_SendTo.TabIndex = 42;
            this.cbFilterFunction_SendTo.Text = "发送到";
            // 
            // cbFilterFunction_WSASend
            // 
            this.cbFilterFunction_WSASend.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbFilterFunction_WSASend.Location = new System.Drawing.Point(166, 6);
            this.cbFilterFunction_WSASend.Name = "cbFilterFunction_WSASend";
            this.cbFilterFunction_WSASend.Size = new System.Drawing.Size(100, 39);
            this.cbFilterFunction_WSASend.TabIndex = 41;
            this.cbFilterFunction_WSASend.Text = "WSA发送";
            // 
            // dFilterFunction
            // 
            this.dFilterFunction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dFilterFunction.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dFilterFunction.Location = new System.Drawing.Point(3, 3);
            this.dFilterFunction.Name = "dFilterFunction";
            this.dFilterFunction.Orientation = AntdUI.TOrientation.Left;
            this.dFilterFunction.Size = new System.Drawing.Size(280, 30);
            this.dFilterFunction.TabIndex = 0;
            this.dFilterFunction.Text = "作用类别";
            // 
            // pFilterAction
            // 
            this.pFilterAction.BorderWidth = 2F;
            this.pFilterAction.Controls.Add(this.tlpFilterAction);
            this.pFilterAction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pFilterAction.Location = new System.Drawing.Point(3, 3);
            this.pFilterAction.Name = "pFilterAction";
            this.pFilterAction.Radius = 10;
            this.pFilterAction.Size = new System.Drawing.Size(338, 238);
            this.pFilterAction.TabIndex = 7;
            // 
            // tlpFilterAction
            // 
            this.tlpFilterAction.BackColor = System.Drawing.Color.Transparent;
            this.tlpFilterAction.ColumnCount = 1;
            this.tlpFilterAction.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterAction.Controls.Add(this.tlpFilterAction2, 0, 1);
            this.tlpFilterAction.Controls.Add(this.dFilterAction, 0, 0);
            this.tlpFilterAction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterAction.Location = new System.Drawing.Point(2, 2);
            this.tlpFilterAction.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFilterAction.Name = "tlpFilterAction";
            this.tlpFilterAction.RowCount = 2;
            this.tlpFilterAction.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterAction.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterAction.Size = new System.Drawing.Size(334, 234);
            this.tlpFilterAction.TabIndex = 0;
            // 
            // tlpFilterAction2
            // 
            this.tlpFilterAction2.ColumnCount = 3;
            this.tlpFilterAction2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpFilterAction2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterAction2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpFilterAction2.Controls.Add(this.rbFilterAction_Replace, 0, 0);
            this.tlpFilterAction2.Controls.Add(this.rbFilterAction_Change, 1, 0);
            this.tlpFilterAction2.Controls.Add(this.rbFilterAction_NoModify_Display, 2, 0);
            this.tlpFilterAction2.Controls.Add(this.rbFilterAction_Intercept, 0, 1);
            this.tlpFilterAction2.Controls.Add(this.rbFilterAction_NoModify_NoDisplay, 2, 1);
            this.tlpFilterAction2.Controls.Add(this.cbFilterAction_Execute, 0, 2);
            this.tlpFilterAction2.Controls.Add(this.cbbFilterAction_ExecuteType, 1, 2);
            this.tlpFilterAction2.Controls.Add(this.cbbFilterAction_Execute, 2, 2);
            this.tlpFilterAction2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterAction2.Location = new System.Drawing.Point(0, 36);
            this.tlpFilterAction2.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFilterAction2.Name = "tlpFilterAction2";
            this.tlpFilterAction2.RowCount = 4;
            this.tlpFilterAction2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterAction2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterAction2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterAction2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterAction2.Size = new System.Drawing.Size(334, 198);
            this.tlpFilterAction2.TabIndex = 5;
            // 
            // rbFilterAction_Replace
            // 
            this.rbFilterAction_Replace.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbFilterAction_Replace.Location = new System.Drawing.Point(3, 3);
            this.rbFilterAction_Replace.Name = "rbFilterAction_Replace";
            this.rbFilterAction_Replace.Size = new System.Drawing.Size(67, 39);
            this.rbFilterAction_Replace.TabIndex = 23;
            this.rbFilterAction_Replace.Text = "替换";
            // 
            // rbFilterAction_Change
            // 
            this.rbFilterAction_Change.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbFilterAction_Change.Location = new System.Drawing.Point(76, 3);
            this.rbFilterAction_Change.Name = "rbFilterAction_Change";
            this.rbFilterAction_Change.Size = new System.Drawing.Size(67, 39);
            this.rbFilterAction_Change.TabIndex = 24;
            this.rbFilterAction_Change.Text = "换包";
            // 
            // rbFilterAction_NoModify_Display
            // 
            this.rbFilterAction_NoModify_Display.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbFilterAction_NoModify_Display.Location = new System.Drawing.Point(193, 3);
            this.rbFilterAction_NoModify_Display.Name = "rbFilterAction_NoModify_Display";
            this.rbFilterAction_NoModify_Display.Size = new System.Drawing.Size(138, 39);
            this.rbFilterAction_NoModify_Display.TabIndex = 25;
            this.rbFilterAction_NoModify_Display.Text = "不修改 - 只显示";
            // 
            // rbFilterAction_Intercept
            // 
            this.rbFilterAction_Intercept.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbFilterAction_Intercept.Location = new System.Drawing.Point(3, 48);
            this.rbFilterAction_Intercept.Name = "rbFilterAction_Intercept";
            this.rbFilterAction_Intercept.Size = new System.Drawing.Size(67, 39);
            this.rbFilterAction_Intercept.TabIndex = 26;
            this.rbFilterAction_Intercept.Text = "拦截";
            // 
            // rbFilterAction_NoModify_NoDisplay
            // 
            this.rbFilterAction_NoModify_NoDisplay.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbFilterAction_NoModify_NoDisplay.Location = new System.Drawing.Point(193, 48);
            this.rbFilterAction_NoModify_NoDisplay.Name = "rbFilterAction_NoModify_NoDisplay";
            this.rbFilterAction_NoModify_NoDisplay.Size = new System.Drawing.Size(138, 39);
            this.rbFilterAction_NoModify_NoDisplay.TabIndex = 27;
            this.rbFilterAction_NoModify_NoDisplay.Text = "不修改 - 不显示";
            // 
            // cbFilterAction_Execute
            // 
            this.cbFilterAction_Execute.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbFilterAction_Execute.Location = new System.Drawing.Point(3, 93);
            this.cbFilterAction_Execute.Name = "cbFilterAction_Execute";
            this.cbFilterAction_Execute.Size = new System.Drawing.Size(67, 39);
            this.cbFilterAction_Execute.TabIndex = 28;
            this.cbFilterAction_Execute.Text = "执行";
            this.cbFilterAction_Execute.CheckedChanged += new AntdUI.BoolEventHandler(this.cbFilterAction_Execute_CheckedChanged);
            // 
            // cbbFilterAction_ExecuteType
            // 
            this.cbbFilterAction_ExecuteType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbbFilterAction_ExecuteType.List = true;
            this.cbbFilterAction_ExecuteType.ListAutoWidth = true;
            this.cbbFilterAction_ExecuteType.Location = new System.Drawing.Point(76, 93);
            this.cbbFilterAction_ExecuteType.Name = "cbbFilterAction_ExecuteType";
            this.cbbFilterAction_ExecuteType.PlaceholderText = "请选择";
            this.cbbFilterAction_ExecuteType.Size = new System.Drawing.Size(111, 39);
            this.cbbFilterAction_ExecuteType.TabIndex = 29;
            this.cbbFilterAction_ExecuteType.SelectedIndexChanged += new AntdUI.IntEventHandler(this.cbbFilterAction_ExecuteType_SelectedIndexChanged);
            // 
            // cbbFilterAction_Execute
            // 
            this.cbbFilterAction_Execute.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cbbFilterAction_Execute.List = true;
            this.cbbFilterAction_Execute.ListAutoWidth = true;
            this.cbbFilterAction_Execute.Location = new System.Drawing.Point(193, 93);
            this.cbbFilterAction_Execute.Name = "cbbFilterAction_Execute";
            this.cbbFilterAction_Execute.PlaceholderText = "请选择";
            this.cbbFilterAction_Execute.Size = new System.Drawing.Size(138, 39);
            this.cbbFilterAction_Execute.TabIndex = 30;
            // 
            // dFilterAction
            // 
            this.dFilterAction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dFilterAction.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dFilterAction.Location = new System.Drawing.Point(3, 3);
            this.dFilterAction.Name = "dFilterAction";
            this.dFilterAction.Orientation = AntdUI.TOrientation.Left;
            this.dFilterAction.Size = new System.Drawing.Size(328, 30);
            this.dFilterAction.TabIndex = 0;
            this.dFilterAction.Text = "动作";
            // 
            // tlpFilterFromAndMode
            // 
            this.tlpFilterFromAndMode.ColumnCount = 2;
            this.tlpFilterFromAndMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.tlpFilterFromAndMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.tlpFilterFromAndMode.Controls.Add(this.pFilterMode, 1, 0);
            this.tlpFilterFromAndMode.Controls.Add(this.pFilterModifyFrom, 0, 0);
            this.tlpFilterFromAndMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterFromAndMode.Location = new System.Drawing.Point(344, 244);
            this.tlpFilterFromAndMode.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFilterFromAndMode.Name = "tlpFilterFromAndMode";
            this.tlpFilterFromAndMode.RowCount = 1;
            this.tlpFilterFromAndMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterFromAndMode.Size = new System.Drawing.Size(344, 201);
            this.tlpFilterFromAndMode.TabIndex = 0;
            // 
            // pFilterMode
            // 
            this.pFilterMode.BorderWidth = 2F;
            this.pFilterMode.Controls.Add(this.tlpFilterMode);
            this.pFilterMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pFilterMode.Location = new System.Drawing.Point(209, 3);
            this.pFilterMode.Name = "pFilterMode";
            this.pFilterMode.Radius = 10;
            this.pFilterMode.Size = new System.Drawing.Size(132, 195);
            this.pFilterMode.TabIndex = 9;
            // 
            // tlpFilterMode
            // 
            this.tlpFilterMode.BackColor = System.Drawing.Color.Transparent;
            this.tlpFilterMode.ColumnCount = 1;
            this.tlpFilterMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterMode.Controls.Add(this.rbFilterMode_Advanced, 0, 2);
            this.tlpFilterMode.Controls.Add(this.dFilterMode, 0, 0);
            this.tlpFilterMode.Controls.Add(this.rbFilterMode_Normal, 0, 1);
            this.tlpFilterMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterMode.Location = new System.Drawing.Point(2, 2);
            this.tlpFilterMode.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFilterMode.Name = "tlpFilterMode";
            this.tlpFilterMode.RowCount = 4;
            this.tlpFilterMode.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterMode.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterMode.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterMode.Size = new System.Drawing.Size(128, 191);
            this.tlpFilterMode.TabIndex = 0;
            // 
            // rbFilterMode_Advanced
            // 
            this.rbFilterMode_Advanced.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbFilterMode_Advanced.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbFilterMode_Advanced.Location = new System.Drawing.Point(3, 84);
            this.rbFilterMode_Advanced.Name = "rbFilterMode_Advanced";
            this.rbFilterMode_Advanced.Size = new System.Drawing.Size(67, 39);
            this.rbFilterMode_Advanced.TabIndex = 2;
            this.rbFilterMode_Advanced.Text = "高级";
            // 
            // dFilterMode
            // 
            this.dFilterMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dFilterMode.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dFilterMode.Location = new System.Drawing.Point(3, 3);
            this.dFilterMode.Name = "dFilterMode";
            this.dFilterMode.Orientation = AntdUI.TOrientation.Left;
            this.dFilterMode.Size = new System.Drawing.Size(122, 30);
            this.dFilterMode.TabIndex = 0;
            this.dFilterMode.Text = "模式";
            // 
            // rbFilterMode_Normal
            // 
            this.rbFilterMode_Normal.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbFilterMode_Normal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbFilterMode_Normal.Location = new System.Drawing.Point(3, 39);
            this.rbFilterMode_Normal.Name = "rbFilterMode_Normal";
            this.rbFilterMode_Normal.Size = new System.Drawing.Size(67, 39);
            this.rbFilterMode_Normal.TabIndex = 1;
            this.rbFilterMode_Normal.Text = "普通";
            this.rbFilterMode_Normal.CheckedChanged += new AntdUI.BoolEventHandler(this.rbFilterMode_Normal_CheckedChanged);
            // 
            // pFilterModifyFrom
            // 
            this.pFilterModifyFrom.BorderWidth = 2F;
            this.pFilterModifyFrom.Controls.Add(this.tlpFilterModifyFrom);
            this.pFilterModifyFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pFilterModifyFrom.Location = new System.Drawing.Point(3, 3);
            this.pFilterModifyFrom.Name = "pFilterModifyFrom";
            this.pFilterModifyFrom.Radius = 10;
            this.pFilterModifyFrom.Size = new System.Drawing.Size(200, 195);
            this.pFilterModifyFrom.TabIndex = 8;
            // 
            // tlpFilterModifyFrom
            // 
            this.tlpFilterModifyFrom.BackColor = System.Drawing.Color.Transparent;
            this.tlpFilterModifyFrom.ColumnCount = 1;
            this.tlpFilterModifyFrom.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterModifyFrom.Controls.Add(this.rbFilterModifyFrom_Position, 0, 2);
            this.tlpFilterModifyFrom.Controls.Add(this.rbFilterModifyFrom_Head, 0, 1);
            this.tlpFilterModifyFrom.Controls.Add(this.dFilterModifyFrom, 0, 0);
            this.tlpFilterModifyFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterModifyFrom.Location = new System.Drawing.Point(2, 2);
            this.tlpFilterModifyFrom.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFilterModifyFrom.Name = "tlpFilterModifyFrom";
            this.tlpFilterModifyFrom.RowCount = 4;
            this.tlpFilterModifyFrom.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterModifyFrom.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterModifyFrom.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterModifyFrom.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterModifyFrom.Size = new System.Drawing.Size(196, 191);
            this.tlpFilterModifyFrom.TabIndex = 0;
            // 
            // rbFilterModifyFrom_Position
            // 
            this.rbFilterModifyFrom_Position.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbFilterModifyFrom_Position.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbFilterModifyFrom_Position.Location = new System.Drawing.Point(3, 84);
            this.rbFilterModifyFrom_Position.Name = "rbFilterModifyFrom_Position";
            this.rbFilterModifyFrom_Position.Size = new System.Drawing.Size(165, 39);
            this.rbFilterModifyFrom_Position.TabIndex = 3;
            this.rbFilterModifyFrom_Position.Text = "自发现有连锁的位置";
            // 
            // rbFilterModifyFrom_Head
            // 
            this.rbFilterModifyFrom_Head.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.rbFilterModifyFrom_Head.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rbFilterModifyFrom_Head.Location = new System.Drawing.Point(3, 39);
            this.rbFilterModifyFrom_Head.Name = "rbFilterModifyFrom_Head";
            this.rbFilterModifyFrom_Head.Size = new System.Drawing.Size(109, 39);
            this.rbFilterModifyFrom_Head.TabIndex = 2;
            this.rbFilterModifyFrom_Head.Text = "数据包开头";
            this.rbFilterModifyFrom_Head.CheckedChanged += new AntdUI.BoolEventHandler(this.rbFilterModifyFrom_Head_CheckedChanged);
            // 
            // dFilterModifyFrom
            // 
            this.dFilterModifyFrom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dFilterModifyFrom.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dFilterModifyFrom.Location = new System.Drawing.Point(3, 3);
            this.dFilterModifyFrom.Name = "dFilterModifyFrom";
            this.dFilterModifyFrom.Orientation = AntdUI.TOrientation.Left;
            this.dFilterModifyFrom.Size = new System.Drawing.Size(190, 30);
            this.dFilterModifyFrom.TabIndex = 0;
            this.dFilterModifyFrom.Text = "修改起始于";
            // 
            // tlpFilterNameAndAppoint
            // 
            this.tlpFilterNameAndAppoint.ColumnCount = 1;
            this.tlpFilterNameAndAppoint.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterNameAndAppoint.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpFilterNameAndAppoint.Controls.Add(this.pFilterAppoint_Advance, 0, 1);
            this.tlpFilterNameAndAppoint.Controls.Add(this.pFilterName, 0, 0);
            this.tlpFilterNameAndAppoint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterNameAndAppoint.Location = new System.Drawing.Point(344, 0);
            this.tlpFilterNameAndAppoint.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFilterNameAndAppoint.Name = "tlpFilterNameAndAppoint";
            this.tlpFilterNameAndAppoint.RowCount = 2;
            this.tlpFilterNameAndAppoint.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpFilterNameAndAppoint.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpFilterNameAndAppoint.Size = new System.Drawing.Size(344, 244);
            this.tlpFilterNameAndAppoint.TabIndex = 6;
            // 
            // pFilterAppoint_Advance
            // 
            this.pFilterAppoint_Advance.BorderWidth = 2F;
            this.pFilterAppoint_Advance.Controls.Add(this.tlpFilterAppoint_Advance);
            this.pFilterAppoint_Advance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pFilterAppoint_Advance.Location = new System.Drawing.Point(3, 125);
            this.pFilterAppoint_Advance.Name = "pFilterAppoint_Advance";
            this.pFilterAppoint_Advance.Radius = 10;
            this.pFilterAppoint_Advance.Size = new System.Drawing.Size(338, 116);
            this.pFilterAppoint_Advance.TabIndex = 9;
            // 
            // tlpFilterAppoint_Advance
            // 
            this.tlpFilterAppoint_Advance.BackColor = System.Drawing.Color.Transparent;
            this.tlpFilterAppoint_Advance.ColumnCount = 1;
            this.tlpFilterAppoint_Advance.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterAppoint_Advance.Controls.Add(this.tlpFilterAppoint_Advance2, 0, 1);
            this.tlpFilterAppoint_Advance.Controls.Add(this.dFilterAppoint_Advance, 0, 0);
            this.tlpFilterAppoint_Advance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterAppoint_Advance.Location = new System.Drawing.Point(2, 2);
            this.tlpFilterAppoint_Advance.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFilterAppoint_Advance.Name = "tlpFilterAppoint_Advance";
            this.tlpFilterAppoint_Advance.RowCount = 2;
            this.tlpFilterAppoint_Advance.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterAppoint_Advance.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterAppoint_Advance.Size = new System.Drawing.Size(334, 112);
            this.tlpFilterAppoint_Advance.TabIndex = 0;
            // 
            // tlpFilterAppoint_Advance2
            // 
            this.tlpFilterAppoint_Advance2.ColumnCount = 2;
            this.tlpFilterAppoint_Advance2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpFilterAppoint_Advance2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterAppoint_Advance2.Controls.Add(this.cbFilter_AppointHeader, 0, 0);
            this.tlpFilterAppoint_Advance2.Controls.Add(this.txtFilter_HeaderContent, 1, 0);
            this.tlpFilterAppoint_Advance2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterAppoint_Advance2.Location = new System.Drawing.Point(0, 36);
            this.tlpFilterAppoint_Advance2.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFilterAppoint_Advance2.Name = "tlpFilterAppoint_Advance2";
            this.tlpFilterAppoint_Advance2.Padding = new System.Windows.Forms.Padding(3);
            this.tlpFilterAppoint_Advance2.RowCount = 2;
            this.tlpFilterAppoint_Advance2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterAppoint_Advance2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterAppoint_Advance2.Size = new System.Drawing.Size(334, 76);
            this.tlpFilterAppoint_Advance2.TabIndex = 1;
            // 
            // cbFilter_AppointHeader
            // 
            this.cbFilter_AppointHeader.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbFilter_AppointHeader.Location = new System.Drawing.Point(6, 6);
            this.cbFilter_AppointHeader.Name = "cbFilter_AppointHeader";
            this.cbFilter_AppointHeader.Size = new System.Drawing.Size(95, 39);
            this.cbFilter_AppointHeader.TabIndex = 32;
            this.cbFilter_AppointHeader.Text = "指定包头";
            this.cbFilter_AppointHeader.CheckedChanged += new AntdUI.BoolEventHandler(this.cbFilter_AppointHeader_CheckedChanged);
            // 
            // dFilterAppoint_Advance
            // 
            this.dFilterAppoint_Advance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dFilterAppoint_Advance.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dFilterAppoint_Advance.Location = new System.Drawing.Point(3, 3);
            this.dFilterAppoint_Advance.Name = "dFilterAppoint_Advance";
            this.dFilterAppoint_Advance.Orientation = AntdUI.TOrientation.Left;
            this.dFilterAppoint_Advance.Size = new System.Drawing.Size(328, 30);
            this.dFilterAppoint_Advance.TabIndex = 0;
            this.dFilterAppoint_Advance.Text = "高级指定";
            // 
            // pFilterName
            // 
            this.pFilterName.BorderWidth = 2F;
            this.pFilterName.Controls.Add(this.tlpFilterName);
            this.pFilterName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pFilterName.Location = new System.Drawing.Point(3, 3);
            this.pFilterName.Name = "pFilterName";
            this.pFilterName.Radius = 10;
            this.pFilterName.Size = new System.Drawing.Size(338, 116);
            this.pFilterName.TabIndex = 8;
            // 
            // tlpFilterName
            // 
            this.tlpFilterName.BackColor = System.Drawing.Color.Transparent;
            this.tlpFilterName.ColumnCount = 1;
            this.tlpFilterName.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterName.Controls.Add(this.dFilterName, 0, 0);
            this.tlpFilterName.Controls.Add(this.txtFilterName, 0, 1);
            this.tlpFilterName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpFilterName.Location = new System.Drawing.Point(2, 2);
            this.tlpFilterName.Margin = new System.Windows.Forms.Padding(0);
            this.tlpFilterName.Name = "tlpFilterName";
            this.tlpFilterName.RowCount = 3;
            this.tlpFilterName.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterName.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpFilterName.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpFilterName.Size = new System.Drawing.Size(334, 112);
            this.tlpFilterName.TabIndex = 0;
            // 
            // dFilterName
            // 
            this.dFilterName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dFilterName.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dFilterName.Location = new System.Drawing.Point(3, 3);
            this.dFilterName.Name = "dFilterName";
            this.dFilterName.Orientation = AntdUI.TOrientation.Left;
            this.dFilterName.Size = new System.Drawing.Size(328, 30);
            this.dFilterName.TabIndex = 0;
            this.dFilterName.Text = "滤镜名称";
            // 
            // txtFilterName
            // 
            this.txtFilterName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFilterName.Location = new System.Drawing.Point(3, 39);
            this.txtFilterName.Name = "txtFilterName";
            this.txtFilterName.PlaceholderText = "请输入字符和数字";
            this.txtFilterName.Size = new System.Drawing.Size(328, 40);
            this.txtFilterName.TabIndex = 1;
            this.txtFilterName.TextChanged += new System.EventHandler(this.txtFilterName_TextChanged);
            // 
            // txtFilter_HeaderContent
            // 
            this.txtFilter_HeaderContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFilter_HeaderContent.Location = new System.Drawing.Point(107, 6);
            this.txtFilter_HeaderContent.Name = "txtFilter_HeaderContent";
            this.txtFilter_HeaderContent.PlaceholderText = "请输入十六进制和空格";
            this.txtFilter_HeaderContent.Size = new System.Drawing.Size(221, 39);
            this.txtFilter_HeaderContent.TabIndex = 33;
            this.txtFilter_HeaderContent.TextChanged += new System.EventHandler(this.txtFilter_HeaderContent_TextChanged);
            // 
            // FilterEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 761);
            this.Controls.Add(this.tlpFilterEdit);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FilterEditForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FilterEditForm";
            this.Load += new System.EventHandler(this.FilterEditForm_Load);
            this.tlpFilterEdit.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.tabFilterEdit.ResumeLayout(false);
            this.tpNormal.ResumeLayout(false);
            this.tlpNormal.ResumeLayout(false);
            this.tpAdvance.ResumeLayout(false);
            this.tlpAdvance.ResumeLayout(false);
            this.tabFilterFrom.ResumeLayout(false);
            this.tpFromHead.ResumeLayout(false);
            this.tpFromPosition.ResumeLayout(false);
            this.tlpFilterSettings.ResumeLayout(false);
            this.pFilterProgression.ResumeLayout(false);
            this.tlpFilterProgression2.ResumeLayout(false);
            this.tlpFilterProgression.ResumeLayout(false);
            this.tlpFilterProgression.PerformLayout();
            this.pFilterAppoint.ResumeLayout(false);
            this.tlpFilterAppoint2.ResumeLayout(false);
            this.tlpFilterAppoint.ResumeLayout(false);
            this.tlpFilterAppoint.PerformLayout();
            this.pFilterFunction.ResumeLayout(false);
            this.tlpFilterFunction.ResumeLayout(false);
            this.tlpFilterFunction2.ResumeLayout(false);
            this.tlpFilterFunction2.PerformLayout();
            this.pFilterAction.ResumeLayout(false);
            this.tlpFilterAction.ResumeLayout(false);
            this.tlpFilterAction2.ResumeLayout(false);
            this.tlpFilterAction2.PerformLayout();
            this.tlpFilterFromAndMode.ResumeLayout(false);
            this.pFilterMode.ResumeLayout(false);
            this.tlpFilterMode.ResumeLayout(false);
            this.tlpFilterMode.PerformLayout();
            this.pFilterModifyFrom.ResumeLayout(false);
            this.tlpFilterModifyFrom.ResumeLayout(false);
            this.tlpFilterModifyFrom.PerformLayout();
            this.tlpFilterNameAndAppoint.ResumeLayout(false);
            this.pFilterAppoint_Advance.ResumeLayout(false);
            this.tlpFilterAppoint_Advance.ResumeLayout(false);
            this.tlpFilterAppoint_Advance2.ResumeLayout(false);
            this.tlpFilterAppoint_Advance2.PerformLayout();
            this.pFilterName.ResumeLayout(false);
            this.tlpFilterName.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpFilterEdit;
        private AntdUI.Tabs tabFilterEdit;
        private AntdUI.TabPage tpNormal;
        private AntdUI.TabPage tpAdvance;
        private System.Windows.Forms.TableLayoutPanel tlpAdvance;
        private AntdUI.Table tFilterAdvanced_Search;
        private AntdUI.Tabs tabFilterFrom;
        private AntdUI.TabPage tpFromHead;
        private AntdUI.Table tFilterAdvanced_Modify_Head;
        private AntdUI.TabPage tpFromPosition;
        private AntdUI.Table tFilterAdvanced_Modify_Position;
        private System.Windows.Forms.TableLayoutPanel tlpFilterSettings;
        private AntdUI.Panel pFilterProgression;
        private System.Windows.Forms.TableLayoutPanel tlpFilterProgression2;
        private System.Windows.Forms.TableLayoutPanel tlpFilterProgression;
        private AntdUI.Checkbox cbProgressionContinuous;
        private AntdUI.Checkbox cbProgressionCarry;
        private AntdUI.InputNumber nudProgressionStep;
        private AntdUI.InputNumber nudProgressionCarry;
        private AntdUI.Label label1;
        private AntdUI.Label label5;
        private AntdUI.Divider dFilterProgression;
        private AntdUI.Panel pFilterAppoint;
        private System.Windows.Forms.TableLayoutPanel tlpFilterAppoint2;
        private System.Windows.Forms.TableLayoutPanel tlpFilterAppoint;
        private AntdUI.Input txtFilter_LengthContent;
        private AntdUI.Checkbox cbFilter_AppointLength;
        private AntdUI.Checkbox cbFilter_AppointSocket;
        private AntdUI.Checkbox cbFilter_AppointPort;
        private AntdUI.Input txtFilter_SocketContent;
        private AntdUI.Input txtFilter_PortContent;
        private AntdUI.Divider dFilterAppoint;
        private AntdUI.Panel pFilterFunction;
        private System.Windows.Forms.TableLayoutPanel tlpFilterFunction;
        private System.Windows.Forms.TableLayoutPanel tlpFilterFunction2;
        private AntdUI.Checkbox cbFilterFunction_Send;
        private AntdUI.Checkbox cbFilterFunction_WSARecvFrom;
        private AntdUI.Checkbox cbFilterFunction_RecvFrom;
        private AntdUI.Checkbox cbFilterFunction_WSARecv;
        private AntdUI.Checkbox cbFilterFunction_Recv;
        private AntdUI.Checkbox cbFilterFunction_WSASendTo;
        private AntdUI.Checkbox cbFilterFunction_SendTo;
        private AntdUI.Checkbox cbFilterFunction_WSASend;
        private AntdUI.Divider dFilterFunction;
        private AntdUI.Panel pFilterAction;
        private System.Windows.Forms.TableLayoutPanel tlpFilterAction;
        private System.Windows.Forms.TableLayoutPanel tlpFilterAction2;
        private AntdUI.Radio rbFilterAction_Replace;
        private AntdUI.Radio rbFilterAction_Change;
        private AntdUI.Radio rbFilterAction_NoModify_Display;
        private AntdUI.Radio rbFilterAction_Intercept;
        private AntdUI.Radio rbFilterAction_NoModify_NoDisplay;
        private AntdUI.Checkbox cbFilterAction_Execute;
        private AntdUI.Select cbbFilterAction_ExecuteType;
        private AntdUI.Select cbbFilterAction_Execute;
        private AntdUI.Divider dFilterAction;
        private System.Windows.Forms.TableLayoutPanel tlpFilterFromAndMode;
        private AntdUI.Panel pFilterMode;
        private System.Windows.Forms.TableLayoutPanel tlpFilterMode;
        private AntdUI.Radio rbFilterMode_Advanced;
        private AntdUI.Divider dFilterMode;
        private AntdUI.Radio rbFilterMode_Normal;
        private AntdUI.Panel pFilterModifyFrom;
        private System.Windows.Forms.TableLayoutPanel tlpFilterModifyFrom;
        private AntdUI.Radio rbFilterModifyFrom_Position;
        private AntdUI.Radio rbFilterModifyFrom_Head;
        private AntdUI.Divider dFilterModifyFrom;
        private System.Windows.Forms.TableLayoutPanel tlpFilterNameAndAppoint;
        private AntdUI.Panel pFilterAppoint_Advance;
        private System.Windows.Forms.TableLayoutPanel tlpFilterAppoint_Advance;
        private System.Windows.Forms.TableLayoutPanel tlpFilterAppoint_Advance2;
        private AntdUI.Checkbox cbFilter_AppointHeader;
        private AntdUI.Divider dFilterAppoint_Advance;
        private AntdUI.Panel pFilterName;
        private System.Windows.Forms.TableLayoutPanel tlpFilterName;
        private AntdUI.Divider dFilterName;
        private AntdUI.Input txtFilterName;
        private System.Windows.Forms.TableLayoutPanel tlpNormal;
        private AntdUI.Table tFilterNormal;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private AntdUI.Input txtFilter_HeaderContent;
    }
}