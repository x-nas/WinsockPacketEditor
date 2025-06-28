namespace WPE.InjectMode
{
    partial class HookSettingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HookSettingsForm));
            this.tlpHookSettings = new System.Windows.Forms.TableLayoutPanel();
            this.tlpButton = new System.Windows.Forms.TableLayoutPanel();
            this.bSave = new AntdUI.Button();
            this.bExit = new AntdUI.Button();
            this.dWS1 = new AntdUI.Divider();
            this.tlpWS1 = new System.Windows.Forms.TableLayoutPanel();
            this.cbHookWS1_RecvFrom = new AntdUI.Checkbox();
            this.cbHookWS1_Recv = new AntdUI.Checkbox();
            this.cbHookWS1_SendTo = new AntdUI.Checkbox();
            this.cbHookWS1_Send = new AntdUI.Checkbox();
            this.divider1 = new AntdUI.Divider();
            this.tlpWS2 = new System.Windows.Forms.TableLayoutPanel();
            this.cbHookWSA_RecvFrom = new AntdUI.Checkbox();
            this.cbHookWSA_Recv = new AntdUI.Checkbox();
            this.cbHookWSA_SendTo = new AntdUI.Checkbox();
            this.cbHookWSA_Send = new AntdUI.Checkbox();
            this.cbHookWS2_RecvFrom = new AntdUI.Checkbox();
            this.cbHookWS2_Recv = new AntdUI.Checkbox();
            this.cbHookWS2_SendTo = new AntdUI.Checkbox();
            this.cbHookWS2_Send = new AntdUI.Checkbox();
            this.tlpHookSettings.SuspendLayout();
            this.tlpButton.SuspendLayout();
            this.tlpWS1.SuspendLayout();
            this.tlpWS2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tlpHookSettings
            // 
            this.tlpHookSettings.ColumnCount = 1;
            this.tlpHookSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpHookSettings.Controls.Add(this.tlpWS2, 0, 5);
            this.tlpHookSettings.Controls.Add(this.tlpWS1, 0, 2);
            this.tlpHookSettings.Controls.Add(this.tlpButton, 0, 6);
            this.tlpHookSettings.Controls.Add(this.dWS1, 0, 0);
            this.tlpHookSettings.Controls.Add(this.divider1, 0, 3);
            this.tlpHookSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpHookSettings.Location = new System.Drawing.Point(0, 0);
            this.tlpHookSettings.Margin = new System.Windows.Forms.Padding(0);
            this.tlpHookSettings.Name = "tlpHookSettings";
            this.tlpHookSettings.RowCount = 7;
            this.tlpHookSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpHookSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpHookSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tlpHookSettings.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpHookSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlpHookSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpHookSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpHookSettings.Size = new System.Drawing.Size(484, 461);
            this.tlpHookSettings.TabIndex = 0;
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
            this.tlpButton.Location = new System.Drawing.Point(0, 401);
            this.tlpButton.Margin = new System.Windows.Forms.Padding(0);
            this.tlpButton.Name = "tlpButton";
            this.tlpButton.RowCount = 3;
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpButton.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpButton.Size = new System.Drawing.Size(484, 60);
            this.tlpButton.TabIndex = 3;
            // 
            // bSave
            // 
            this.bSave.BackExtend = "135, #6253E1, #04BEFE";
            this.bSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bSave.IconSvg = "SaveOutlined";
            this.bSave.Location = new System.Drawing.Point(115, 7);
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
            this.bExit.Location = new System.Drawing.Point(255, 7);
            this.bExit.Name = "bExit";
            this.bExit.Size = new System.Drawing.Size(114, 46);
            this.bExit.TabIndex = 1;
            this.bExit.Text = "退出";
            this.bExit.Type = AntdUI.TTypeMini.Primary;
            this.bExit.Click += new System.EventHandler(this.bExit_Click);
            // 
            // dWS1
            // 
            this.dWS1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dWS1.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dWS1.Location = new System.Drawing.Point(3, 3);
            this.dWS1.Name = "dWS1";
            this.dWS1.Orientation = AntdUI.TOrientation.Left;
            this.dWS1.Size = new System.Drawing.Size(478, 23);
            this.dWS1.TabIndex = 4;
            this.dWS1.Text = "Winsock 1.1";
            this.dWS1.Thickness = 1.5F;
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
            this.tlpWS1.Location = new System.Drawing.Point(0, 49);
            this.tlpWS1.Margin = new System.Windows.Forms.Padding(0);
            this.tlpWS1.Name = "tlpWS1";
            this.tlpWS1.RowCount = 3;
            this.tlpWS1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpWS1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpWS1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpWS1.Size = new System.Drawing.Size(484, 100);
            this.tlpWS1.TabIndex = 5;
            // 
            // cbHookWS1_RecvFrom
            // 
            this.cbHookWS1_RecvFrom.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbHookWS1_RecvFrom.Location = new System.Drawing.Point(245, 51);
            this.cbHookWS1_RecvFrom.Name = "cbHookWS1_RecvFrom";
            this.cbHookWS1_RecvFrom.Size = new System.Drawing.Size(118, 42);
            this.cbHookWS1_RecvFrom.TabIndex = 3;
            this.cbHookWS1_RecvFrom.Text = "接收自 1.1";
            // 
            // cbHookWS1_Recv
            // 
            this.cbHookWS1_Recv.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbHookWS1_Recv.Location = new System.Drawing.Point(3, 51);
            this.cbHookWS1_Recv.Name = "cbHookWS1_Recv";
            this.cbHookWS1_Recv.Size = new System.Drawing.Size(102, 42);
            this.cbHookWS1_Recv.TabIndex = 2;
            this.cbHookWS1_Recv.Text = "接收 1.1";
            // 
            // cbHookWS1_SendTo
            // 
            this.cbHookWS1_SendTo.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbHookWS1_SendTo.Location = new System.Drawing.Point(245, 3);
            this.cbHookWS1_SendTo.Name = "cbHookWS1_SendTo";
            this.cbHookWS1_SendTo.Size = new System.Drawing.Size(118, 42);
            this.cbHookWS1_SendTo.TabIndex = 1;
            this.cbHookWS1_SendTo.Text = "发送到 1.1";
            // 
            // cbHookWS1_Send
            // 
            this.cbHookWS1_Send.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbHookWS1_Send.Location = new System.Drawing.Point(3, 3);
            this.cbHookWS1_Send.Name = "cbHookWS1_Send";
            this.cbHookWS1_Send.Size = new System.Drawing.Size(102, 42);
            this.cbHookWS1_Send.TabIndex = 0;
            this.cbHookWS1_Send.Text = "发送 1.1";
            // 
            // divider1
            // 
            this.divider1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.divider1.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.divider1.Location = new System.Drawing.Point(3, 152);
            this.divider1.Name = "divider1";
            this.divider1.Orientation = AntdUI.TOrientation.Left;
            this.divider1.Size = new System.Drawing.Size(478, 23);
            this.divider1.TabIndex = 6;
            this.divider1.Text = "Winsock 2.0";
            this.divider1.Thickness = 1.5F;
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
            this.tlpWS2.Location = new System.Drawing.Point(0, 198);
            this.tlpWS2.Margin = new System.Windows.Forms.Padding(0);
            this.tlpWS2.Name = "tlpWS2";
            this.tlpWS2.RowCount = 5;
            this.tlpWS2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpWS2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpWS2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpWS2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpWS2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpWS2.Size = new System.Drawing.Size(484, 203);
            this.tlpWS2.TabIndex = 7;
            // 
            // cbHookWSA_RecvFrom
            // 
            this.cbHookWSA_RecvFrom.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbHookWSA_RecvFrom.Location = new System.Drawing.Point(245, 147);
            this.cbHookWSA_RecvFrom.Name = "cbHookWSA_RecvFrom";
            this.cbHookWSA_RecvFrom.Size = new System.Drawing.Size(127, 42);
            this.cbHookWSA_RecvFrom.TabIndex = 8;
            this.cbHookWSA_RecvFrom.Text = "WSA接收自";
            // 
            // cbHookWSA_Recv
            // 
            this.cbHookWSA_Recv.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbHookWSA_Recv.Location = new System.Drawing.Point(3, 147);
            this.cbHookWSA_Recv.Name = "cbHookWSA_Recv";
            this.cbHookWSA_Recv.Size = new System.Drawing.Size(111, 42);
            this.cbHookWSA_Recv.TabIndex = 7;
            this.cbHookWSA_Recv.Text = "WSA接收";
            // 
            // cbHookWSA_SendTo
            // 
            this.cbHookWSA_SendTo.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbHookWSA_SendTo.Location = new System.Drawing.Point(245, 99);
            this.cbHookWSA_SendTo.Name = "cbHookWSA_SendTo";
            this.cbHookWSA_SendTo.Size = new System.Drawing.Size(127, 42);
            this.cbHookWSA_SendTo.TabIndex = 6;
            this.cbHookWSA_SendTo.Text = "WSA发送到";
            // 
            // cbHookWSA_Send
            // 
            this.cbHookWSA_Send.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbHookWSA_Send.Location = new System.Drawing.Point(3, 99);
            this.cbHookWSA_Send.Name = "cbHookWSA_Send";
            this.cbHookWSA_Send.Size = new System.Drawing.Size(111, 42);
            this.cbHookWSA_Send.TabIndex = 5;
            this.cbHookWSA_Send.Text = "WSA发送";
            // 
            // cbHookWS2_RecvFrom
            // 
            this.cbHookWS2_RecvFrom.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbHookWS2_RecvFrom.Location = new System.Drawing.Point(245, 51);
            this.cbHookWS2_RecvFrom.Name = "cbHookWS2_RecvFrom";
            this.cbHookWS2_RecvFrom.Size = new System.Drawing.Size(90, 42);
            this.cbHookWS2_RecvFrom.TabIndex = 4;
            this.cbHookWS2_RecvFrom.Text = "接收自";
            // 
            // cbHookWS2_Recv
            // 
            this.cbHookWS2_Recv.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbHookWS2_Recv.Location = new System.Drawing.Point(3, 51);
            this.cbHookWS2_Recv.Name = "cbHookWS2_Recv";
            this.cbHookWS2_Recv.Size = new System.Drawing.Size(74, 42);
            this.cbHookWS2_Recv.TabIndex = 3;
            this.cbHookWS2_Recv.Text = "接收";
            // 
            // cbHookWS2_SendTo
            // 
            this.cbHookWS2_SendTo.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbHookWS2_SendTo.Location = new System.Drawing.Point(245, 3);
            this.cbHookWS2_SendTo.Name = "cbHookWS2_SendTo";
            this.cbHookWS2_SendTo.Size = new System.Drawing.Size(90, 42);
            this.cbHookWS2_SendTo.TabIndex = 2;
            this.cbHookWS2_SendTo.Text = "发送到";
            // 
            // cbHookWS2_Send
            // 
            this.cbHookWS2_Send.AutoSizeMode = AntdUI.TAutoSize.Auto;
            this.cbHookWS2_Send.Location = new System.Drawing.Point(3, 3);
            this.cbHookWS2_Send.Name = "cbHookWS2_Send";
            this.cbHookWS2_Send.Size = new System.Drawing.Size(74, 42);
            this.cbHookWS2_Send.TabIndex = 1;
            this.cbHookWS2_Send.Text = "发送";
            // 
            // HookSettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 461);
            this.Controls.Add(this.tlpHookSettings);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "HookSettingsForm";
            this.Text = "HookSettings";
            this.Load += new System.EventHandler(this.HookSettingsForm_Load);
            this.tlpHookSettings.ResumeLayout(false);
            this.tlpButton.ResumeLayout(false);
            this.tlpWS1.ResumeLayout(false);
            this.tlpWS1.PerformLayout();
            this.tlpWS2.ResumeLayout(false);
            this.tlpWS2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpHookSettings;
        private System.Windows.Forms.TableLayoutPanel tlpButton;
        private AntdUI.Button bSave;
        private AntdUI.Button bExit;
        private System.Windows.Forms.TableLayoutPanel tlpWS1;
        private AntdUI.Checkbox cbHookWS1_RecvFrom;
        private AntdUI.Checkbox cbHookWS1_Recv;
        private AntdUI.Checkbox cbHookWS1_SendTo;
        private AntdUI.Checkbox cbHookWS1_Send;
        private AntdUI.Divider dWS1;
        private System.Windows.Forms.TableLayoutPanel tlpWS2;
        private AntdUI.Checkbox cbHookWSA_RecvFrom;
        private AntdUI.Checkbox cbHookWSA_Recv;
        private AntdUI.Checkbox cbHookWSA_SendTo;
        private AntdUI.Checkbox cbHookWSA_Send;
        private AntdUI.Checkbox cbHookWS2_RecvFrom;
        private AntdUI.Checkbox cbHookWS2_Recv;
        private AntdUI.Checkbox cbHookWS2_SendTo;
        private AntdUI.Checkbox cbHookWS2_Send;
        private AntdUI.Divider divider1;
    }
}