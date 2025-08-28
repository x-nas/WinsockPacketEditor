using AntdUI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class LogList : UserControl
    {
        private Form form;

        #region//窗体事件

        public LogList(Form _form)
        {
            InitializeComponent();
            this.form = _form;
        }

        private void LogList_Load(object sender, EventArgs e)
        {
            this.InitTable_LogList();
            this.Dark_Changed();
        }

        private void InitTable_LogList()
        {
            tSystemLog.Columns = new AntdUI.ColumnCollection {
                new AntdUI.Column("", "序号", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return (rowindex + 1);
                    },
                }.SetFixed().SetLocalizationTitleID("Table.LogList.Column.ID"),
                new AntdUI.Column("LogTime", "时间戳")
                {
                    Render = (value, record, rowindex)=>
                    {
                        return ((DateTime)value).ToString("HH:mm:ss:fffffff");
                    },
                }.SetLocalizationTitleID("Table.LogList.Column."),
                new AntdUI.Column("FuncName", "模块", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.LogList.Column."),
                new AntdUI.Column("LogContent", "日志内容").SetLocalizationTitleID("Table.LogList.Column."),
            };

            this.tSystemLog.ColumnFont = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.tSystemLog.DataSource = Operate.LogConfig.List.lstLogInfo;
        }

        public void Dark_Changed()
        {
            if (AntdUI.Config.IsDark)
            {
                this.tSystemLog.BackColor = Operate.SystemConfig.Color_40;
                this.tSystemLog.ColumnBack = Operate.SystemConfig.Color_40;
            }
            else
            {
                this.tSystemLog.BackColor = Color.White;
                this.tSystemLog.ColumnBack = null;
            }
        }

        public void RefreshLogList()
        {
            this.tSystemLog.Refresh();
        }

        public void ScrollToBottom()
        {
            tSystemLog.ScrollBar.ValueY = tSystemLog.ScrollBar.MaxY;
        }

        public void CleanUp_LogList()
        {
            try
            {
                Operate.LogConfig.Queue.ClearLogQueue();
                Operate.LogConfig.List.ClearLogList();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//日志列表 - 右键菜单

        private void tSystemLog_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Operate.LogConfig.List.lstLogInfo.Count == 0)
                {
                    return;
                }

                AntdUI.ContextMenuStrip.open(tSystemLog, item =>
                {
                    List<LogInfo> liList = new List<LogInfo>();

                    foreach (int SelectIndex in this.tSystemLog.SelectedIndexs)
                    {
                        liList.Add(Operate.LogConfig.List.lstLogInfo[SelectIndex - 1]);
                    }

                    switch (item.ID)
                    {
                        case "Copy":

                            if (liList.Count > 0)
                            {
                                string LogString = string.Empty;
                                foreach (LogInfo li in liList)
                                {
                                    LogString += li.LogTime.ToString() + ": " + li.FuncName + " - " + li.LogContent + "\r\n";
                                }

                                Clipboard.SetText(LogString);

                                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "已复制到剪贴板", TType.Success)
                                {
                                    LocalizationText = "CopyToClipboard"
                                });
                            }

                            break;

                        case "ToExcel":

                            Operate.LogConfig.List.SaveLogList_Dialog(this.form, this.tSystemLog, Operate.PacketConfig.Packet.InjectProcess, liList);

                            break;

                        case "ClearUp":

                            AntdUI.Modal.open(new AntdUI.Modal.Config(this.form, AntdUI.Localization.Get("LogList.LogList", "日志列表"), AntdUI.Localization.Get("SureToDelete", "\r\n确定删除所有数据吗\r\n\r\n"))
                            {
                                Icon = TType.Warn,
                                Keyboard = false,
                                MaskClosable = false,                                
                                OnOk = config =>
                                {
                                    this.CleanUp_LogList();

                                    return true;
                                }
                            });

                            break;

                        case "DeSelect":

                            this.tSystemLog.SelectedIndex = -1;

                            break;
                    }
                }, Operate.LogConfig.List.GetCMS_LogList());
            }
        }

        #endregion
    }
}
