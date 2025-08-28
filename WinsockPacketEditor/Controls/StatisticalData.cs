using System;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class StatisticalData : UserControl
    {
        #region//窗体事件

        public StatisticalData()
        {
            InitializeComponent();
        }

        private void StatisticalData_Load(object sender, EventArgs e)
        {
            this.tabStatistical.TabMenuVisible = false;
            this.InitTable_StatisticalFilter();
            this.Dark_Changed();
        }

        private void InitTable_StatisticalFilter()
        {
            tStatisticalFilter.Columns = new AntdUI.ColumnCollection {
                new AntdUI.Column("", "", AntdUI.ColumnAlign.Center)
                {
                    Render = (value, record, rowindex)=>
                    {
                        return (rowindex + 1);
                    },
                }.SetFixed(),
                new AntdUI.Column("FName", "滤镜名称").SetLocalizationTitleID("Table.StatisticalData.Column."),
                new AntdUI.Column("Status", "状态")
                {
                    Render = (value, record, rowindex)=>
                    {
                        if(record is FilterInfo fi)
                        {
                            if(fi.IsEnable)
                            {
                                if(fi.ExecutionCount > 0)
                                {
                                    return new AntdUI.CellBadge(AntdUI.TState.Processing, AntdUI.Localization.Get("Working", "处理中"));
                                }
                                else
                                {
                                    return new AntdUI.CellBadge(AntdUI.TState.Success, AntdUI.Localization.Get("Enable", "启用"));
                                }
                            }
                            else
                            {
                                return new AntdUI.CellBadge(AntdUI.TState.Error, AntdUI.Localization.Get("Disable", "停止"));
                            }
                        }

                        return value;
                    },
                }.SetLocalizationTitleID("Table.StatisticalData.Column."),
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
                                return AntdUI.Localization.Get("NoModifyNoDisplay", "不显示");

                            case Operate.FilterConfig.Filter.FilterAction.NoModify_Display:
                                return AntdUI.Localization.Get("NoModifyDisplay", "只显示");

                            default:
                                return value;
                        }
                    },
                }.SetLocalizationTitleID("Table.StatisticalData.Column."),
                new AntdUI.Column("ExecutionCount", "执行次数", AntdUI.ColumnAlign.Center).SetLocalizationTitleID("Table.StatisticalData.Column."),
            };

            this.tStatisticalFilter.ColumnFont = new Font("Microsoft YaHei UI", 11F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
            this.tStatisticalFilter.Binding(Operate.FilterConfig.List.lstFilterInfo);
        }

        public void Dark_Changed()
        {
            if (AntdUI.Config.IsDark)
            {
                this.progressExecute.Back = Color.FromArgb(48, 58, 66);
                this.progressReplace.Back = Color.FromArgb(39, 41, 83);
                this.progressReplace.ForeColor = Color.White;
                this.progressChange.Back = Color.FromArgb(47, 46, 80);
                this.progressChange.ForeColor = Color.White;
                this.progressIntercept.Back = Color.FromArgb(57, 47, 78);
                this.progressIntercept.ForeColor = Color.White;
                this.progressDisplay.Back = Color.FromArgb(67, 46, 76);
                this.progressDisplay.ForeColor = Color.White;
                this.progressNoDisplay.Back = Color.FromArgb(80, 47, 79);
                this.progressNoDisplay.ForeColor = Color.White;
                this.tStatisticalFilter.ColumnBack = Operate.SystemConfig.Color_35;
            }
            else
            {
                this.progressExecute.Back = null;
                this.progressReplace.Back = null;
                this.progressReplace.ForeColor = null;
                this.progressChange.Back = null;
                this.progressChange.ForeColor = null;
                this.progressIntercept.Back = null;
                this.progressIntercept.ForeColor = null;
                this.progressDisplay.Back = null;
                this.progressDisplay.ForeColor = null;
                this.progressNoDisplay.Back = null;
                this.progressNoDisplay.ForeColor = null;
                this.tStatisticalFilter.ColumnBack = null;
            }
        }

        #endregion

        #region//统计数据（异步）

        private void bStatistical_Filter_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.bgwStatistical.IsBusy)
                {
                    this.bStatistical_Filter.Loading = true;
                    this.bgwStatistical.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bgwStatistical_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                long ProxyTotal_CNT =
                    Operate.ProxyConfig.Proxy.TCP_Req_CNT +
                    Operate.ProxyConfig.Proxy.TCP_Resp_CNT +
                    Operate.ProxyConfig.Proxy.UDP_Req_CNT +
                    Operate.ProxyConfig.Proxy.UDP_Resp_CNT;

                long FilterExec = Operate.FilterConfig.Filter.FilterExecute_CNT;
                long FilterReplace = Operate.FilterConfig.Filter.FilterReplace_CNT;
                long FilterChange = Operate.FilterConfig.Filter.FilterChange_CNT;
                long FilterIntercept = Operate.FilterConfig.Filter.FilterIntercept_CNT;
                long FilterDisplay = Operate.FilterConfig.Filter.FilterDisplay_CNT;
                long FilterNoDisplay = Operate.FilterConfig.Filter.FilterNoDisplay_CNT;

                decimal dExecute = 0;
                if (ProxyTotal_CNT > 0)
                {
                    dExecute = (decimal)FilterExec / ProxyTotal_CNT;
                    dExecute = Math.Round(dExecute, 2);
                }

                this.progressExecute.Value = (float)dExecute;
                this.progressExecute.Text = (dExecute * 100).ToString() + "%";

                decimal dReplace = 0, dChange = 0, dIntercept = 0, dDisplay = 0, dNoDisplay = 0;
                if (FilterExec > 0)
                {
                    dReplace = (decimal)FilterReplace / FilterExec;
                    dReplace = Math.Round(dReplace, 2);
                    dChange = (decimal)FilterChange / FilterExec;
                    dChange = Math.Round(dChange, 2);
                    dIntercept = (decimal)FilterIntercept / FilterExec;
                    dIntercept = Math.Round(dIntercept, 2);
                    dDisplay = (decimal)FilterDisplay / FilterExec;
                    dDisplay = Math.Round(dDisplay, 2);
                    dNoDisplay = (decimal)FilterNoDisplay / FilterExec;
                    dNoDisplay = Math.Round(dNoDisplay, 2);
                }

                this.progressReplace.Value = (float)dReplace;
                this.progressReplace.Text = (dReplace * 100).ToString() + "%";
                this.progressChange.Value = (float)dChange;
                this.progressChange.Text = (dChange * 100).ToString() + "%";
                this.progressIntercept.Value = (float)dIntercept;
                this.progressIntercept.Text = (dIntercept * 100).ToString() + "%";
                this.progressDisplay.Value = (float)dDisplay;
                this.progressDisplay.Text = (dDisplay * 100).ToString() + "%";
                this.progressNoDisplay.Value = (float)dNoDisplay;
                this.progressNoDisplay.Text = (dNoDisplay * 100).ToString() + "%";
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void bgwStatistical_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            try
            {
                this.bStatistical_Filter.Loading = false;
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion        
    }
}
