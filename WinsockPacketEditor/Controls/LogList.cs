using AntdUI;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            this.cbLogList_AutoClear.Checked = Operate.LogConfig.List.AutoClear;
            this.txtLogList_AutoClear.Value = Operate.LogConfig.List.AutoClear_Value;
            this.cbLogList_AutoRoll.Checked = Operate.LogConfig.List.AutoRoll;

            this.tabLogList.SelectTab(0);

            this.InitTable_SystemLogList();
            this.InitTable_FilterLogList();
            this.InitTable_ProxyLog();
            this.Dark_Changed();
        }

        private void InitTable_SystemLogList()
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
                        return ((DateTime)value).ToString("HH:mm:ss");
                    },
                }.SetLocalizationTitleID("Table.LogList.Column."),
                new AntdUI.Column("FuncName", "模块").SetLocalizationTitleID("Table.LogList.Column."),
                new AntdUI.Column("LogContent", "日志内容").SetLocalizationTitleID("Table.LogList.Column."),
            };

            this.tSystemLog.ColumnFont = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.tSystemLog.Binding(Operate.LogConfig.List.lstLogInfo);
        }

        private void InitTable_FilterLogList()
        {
            tFilterLog.Columns = new AntdUI.ColumnCollection {
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
                new AntdUI.Column("FName", "滤镜名称", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.FilterList.Column."),
                new AntdUI.Column("FAction", "动作")
                {
                    Render = (value, record, rowindex)=>
                    {
                        switch((Operate.FilterConfig.Filter.FilterAction)value)
                        {
                            case Operate.FilterConfig.Filter.FilterAction.Replace:
                                return AntdUI.Localization.Get("Replace", "替换");

                            case Operate.FilterConfig.Filter.FilterAction.Change:
                                return AntdUI.Localization.Get("Change", "换包");

                            case Operate.FilterConfig.Filter.FilterAction.Intercept:
                                return AntdUI.Localization.Get("Intercept", "拦截");

                            case Operate.FilterConfig.Filter.FilterAction.NoModify_NoDisplay:
                                return AntdUI.Localization.Get("NoModifyNoDisplay", "不修改不显示");

                            case Operate.FilterConfig.Filter.FilterAction.NoModify_Display:
                                return AntdUI.Localization.Get("NoModifyDisplay", "不修改只显示");

                            default:
                                return value;
                        }
                    },
                }.SetLocalizationTitleID("Table.FilterList.Column."),
                new AntdUI.Column("MatchNum", "匹配数", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.FilterLogList.Column."),
                new AntdUI.Column("PacketType", "类别", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return Operate.PacketConfig.Packet.GetName_ByPacketType((Operate.PacketConfig.Packet.PacketType)value);
                    },
                }.SetLocalizationTitleID("Table.PacketList.Column."),
                new AntdUI.Column("PacketLen", "长度", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.PacketList.Column."),
            };

            this.tFilterLog.ColumnFont = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.tFilterLog.Binding(Operate.LogConfig.List.lstFilterLogInfo);
        }

        private void InitTable_ProxyLog()
        {
            tProxyLog.Columns = new AntdUI.ColumnCollection {
                new AntdUI.Column("", "序号", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return (rowindex + 1);
                    },
                }.SetFixed().SetLocalizationTitleID("Table.ProxyLog.Column.ID"),
                new AntdUI.Column("LogTime", "时间戳")
                {
                    Render = (value, record, rowindex)=>
                    {
                        return ((DateTime)value).ToString("HH:mm:ss");
                    },
                }.SetLocalizationTitleID("Table.ProxyLog.Column."),
                new AntdUI.Column("UserName", "账号", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.ProxyLog.Column."),
                new AntdUI.Column("LoginIP", "IP地址")
                {
                    Render = (value, record, rowindex)=>
                    {
                        return new CellText(value?.ToString() ?? string.Empty)
                        {
                            Prefix = Operate.SystemConfig.GetFlagByLocation(value.ToString()),
                            IconRatio = 1.0F
                        };
                    },
                }.SetLocalizationTitleID("Table.ProxyLog.Column."),
                new AntdUI.Column("LogContent", "日志内容").SetLocalizationTitleID("Table.ProxyLog.Column."),
            };

            this.tProxyLog.ColumnFont = new Font("Microsoft YaHei UI", 9F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.tProxyLog.Binding(Operate.LogConfig.List.lstProxyLogInfo);
        }

        public void Dark_Changed()
        {
            if (AntdUI.Config.IsDark)
            {
                this.tSystemLog.BackColor = Operate.SystemConfig.Color_40;
                this.tSystemLog.ColumnBack = Operate.SystemConfig.Color_40;

                this.tFilterLog.BackColor = Operate.SystemConfig.Color_40;
                this.tFilterLog.ColumnBack = Operate.SystemConfig.Color_40;

                this.tProxyLog.BackColor = Operate.SystemConfig.Color_40;
                this.tProxyLog.ColumnBack = Operate.SystemConfig.Color_40;
            }
            else
            {
                this.tSystemLog.BackColor = Color.White;
                this.tSystemLog.ColumnBack = null;

                this.tFilterLog.BackColor = Color.White;
                this.tFilterLog.ColumnBack = null;

                this.tProxyLog.BackColor = Color.White;
                this.tProxyLog.ColumnBack = null;
            }
        }

        public void RefreshSystemLog()
        {
            if (Operate.LogConfig.List.AutoRoll)
            {
                tSystemLog.ScrollBar.ValueY = tSystemLog.ScrollBar.MaxY;
            }

            if (Operate.LogConfig.List.AutoClear)
            {
                if (Operate.LogConfig.List.lstLogInfo.Count > Operate.LogConfig.List.AutoClear_Value)
                {
                    this.CleanUp_SystemLog();
                }
            }
        }

        public void RefreshFilterLog()
        {
            if (Operate.LogConfig.List.AutoRoll)
            {
                tFilterLog.ScrollBar.ValueY = tFilterLog.ScrollBar.MaxY;
            }

            if (Operate.LogConfig.List.AutoClear)
            {
                if (Operate.LogConfig.List.lstFilterLogInfo.Count > Operate.LogConfig.List.AutoClear_Value)
                {
                    this.CleanUp_FilterLog();
                }
            }
        }

        public void RefreshProxyLog()
        {
            if (Operate.LogConfig.List.AutoRoll)
            {
                tProxyLog.ScrollBar.ValueY = tProxyLog.ScrollBar.MaxY;
            }

            if (Operate.LogConfig.List.AutoClear)
            {
                if (Operate.LogConfig.List.lstProxyLogInfo.Count > Operate.LogConfig.List.AutoClear_Value)
                {
                    this.CleanUp_ProxyLog();
                }
            }
        }

        public void CleanUp_SystemLog()
        {
            Operate.LogConfig.Queue.ClearLogQueue();
            Operate.LogConfig.List.ClearLogList();
        }

        public void CleanUp_FilterLog()
        {
            Operate.LogConfig.Queue.ClearFilterLogQueue();
            Operate.LogConfig.List.ClearFilterLogList();
        }

        public void CleanUp_ProxyLog()
        {
            Operate.LogConfig.Queue.ClearProxyLogQueue();
            Operate.LogConfig.List.ClearProxyLogList();
        }

        public void CleanUp_LogList()
        {
            this.CleanUp_SystemLog();
            this.CleanUp_FilterLog();
            this.CleanUp_ProxyLog();
        }

        #endregion

        #region//自动清理

        private void cbLogList_AutoClear_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.LogList_AutoClear_Changed();

            Operate.LogConfig.List.AutoClear = this.cbLogList_AutoClear.Checked;
        }

        private void txtLogList_AutoClear_ValueChanged(object sender, DecimalEventArgs e)
        {
            Operate.LogConfig.List.AutoClear_Value = this.txtLogList_AutoClear.Value;
        }

        private void LogList_AutoClear_Changed()
        {
            this.txtLogList_AutoClear.Enabled = this.cbLogList_AutoClear.Checked;
        }

        #endregion

        #region//自动滚动

        private void cbLogList_AutoRoll_CheckedChanged(object sender, BoolEventArgs e)
        {
            this.LogList_AutoRoll_Changed();
        }

        private void LogList_AutoRoll_Changed()
        {
            Operate.LogConfig.List.AutoRoll = this.cbLogList_AutoRoll.Checked;
        }

        #endregion        

        #region//系统日志列表 - 右键菜单

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
                                this.tSystemLog.CopyData(this.tSystemLog.SelectedIndexs);

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
                                    this.CleanUp_SystemLog();

                                    return true;
                                }
                            });

                            break;

                        case "SelectAll":

                            int[] IndexALL = new int[Operate.LogConfig.List.lstLogInfo.Count];

                            for (int i = 0; i < IndexALL.Length; i++)
                            {
                                IndexALL[i] = i + 1;
                            }

                            this.tSystemLog.SelectedIndexs = IndexALL;

                            break;

                        case "DeSelect":

                            this.tSystemLog.SelectedIndex = -1;

                            break;
                    }
                }, Operate.LogConfig.List.GetCMS_LogList());
            }
        }

        #endregion

        #region//滤镜日志列表 - 右键菜单

        private void tFilterLog_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Operate.LogConfig.List.lstFilterLogInfo.Count == 0)
                {
                    return;
                }

                AntdUI.ContextMenuStrip.open(tFilterLog, item =>
                {
                    List<FilterLogInfo> liList = new List<FilterLogInfo>();

                    foreach (int SelectIndex in this.tFilterLog.SelectedIndexs)
                    {
                        liList.Add(Operate.LogConfig.List.lstFilterLogInfo[SelectIndex - 1]);
                    }

                    switch (item.ID)
                    {
                        case "Copy":

                            if (liList.Count > 0)
                            {
                                this.tFilterLog.CopyData(this.tFilterLog.SelectedIndexs);

                                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "已复制到剪贴板", TType.Success)
                                {
                                    LocalizationText = "CopyToClipboard"
                                });
                            }

                            break;

                        case "ToExcel":

                            Operate.LogConfig.List.SaveFilterLogList_Dialog(this.form, this.tFilterLog, Operate.PacketConfig.Packet.InjectProcess, liList);

                            break;

                        case "ClearUp":

                            AntdUI.Modal.open(new AntdUI.Modal.Config(this.form, AntdUI.Localization.Get("LogList.LogList", "日志列表"), AntdUI.Localization.Get("SureToDelete", "\r\n确定删除所有数据吗\r\n\r\n"))
                            {
                                Icon = TType.Warn,
                                Keyboard = false,
                                MaskClosable = false,
                                OnOk = config =>
                                {
                                    this.CleanUp_FilterLog();

                                    return true;
                                }
                            });

                            break;

                        case "SelectAll":

                            int[] IndexALL = new int[Operate.LogConfig.List.lstFilterLogInfo.Count];

                            for (int i = 0; i < IndexALL.Length; i++)
                            {
                                IndexALL[i] = i + 1;
                            }

                            this.tFilterLog.SelectedIndexs = IndexALL;

                            break;

                        case "DeSelect":

                            this.tFilterLog.SelectedIndex = -1;

                            break;
                    }
                }, Operate.LogConfig.List.GetCMS_LogList());
            }
        }

        #endregion

        #region//代理日志列表 - 右键菜单

        private void tProxyLog_CellClick(object sender, TableClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (Operate.LogConfig.List.lstProxyLogInfo.Count == 0)
                {
                    return;
                }

                AntdUI.ContextMenuStrip.open(tProxyLog, item =>
                {
                    List<ProxyLogInfo> liList = new List<ProxyLogInfo>();

                    foreach (int SelectIndex in this.tProxyLog.SelectedIndexs)
                    {
                        liList.Add(Operate.LogConfig.List.lstProxyLogInfo[SelectIndex - 1]);
                    }

                    switch (item.ID)
                    {
                        case "Copy":

                            if (liList.Count > 0)
                            {
                                this.tProxyLog.CopyData(this.tProxyLog.SelectedIndexs);

                                AntdUI.Message.open(new AntdUI.Message.Config(this.form, "已复制到剪贴板", TType.Success)
                                {
                                    LocalizationText = "CopyToClipboard"
                                });
                            }

                            break;

                        case "ToExcel":

                            Operate.LogConfig.List.SaveProxyLogList_Dialog(this.form, this.tProxyLog, Operate.PacketConfig.Packet.InjectProcess, liList);

                            break;

                        case "ClearUp":

                            AntdUI.Modal.open(new AntdUI.Modal.Config(this.form, AntdUI.Localization.Get("LogList.LogList", "日志列表"), AntdUI.Localization.Get("SureToDelete", "\r\n确定删除所有数据吗\r\n\r\n"))
                            {
                                Icon = TType.Warn,
                                Keyboard = false,
                                MaskClosable = false,
                                OnOk = config =>
                                {
                                    this.CleanUp_ProxyLog();

                                    return true;
                                }
                            });

                            break;

                        case "SelectAll":

                            int[] IndexALL = new int[Operate.LogConfig.List.lstProxyLogInfo.Count];

                            for (int i = 0; i < IndexALL.Length; i++)
                            {
                                IndexALL[i] = i + 1;
                            }

                            this.tProxyLog.SelectedIndexs = IndexALL;

                            break;

                        case "DeSelect":

                            this.tProxyLog.SelectedIndex = -1;

                            break;
                    }
                }, Operate.LogConfig.List.GetCMS_LogList());
            }
        }

        #endregion
    }
}
