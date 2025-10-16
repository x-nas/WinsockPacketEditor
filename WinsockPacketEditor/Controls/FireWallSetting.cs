using AntdUI;
using System;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class FireWallSetting : UserControl
    {
        private Form form = null;

        #region//窗体事件

        public FireWallSetting(Form form)
        {
            InitializeComponent();
            this.form = form;
        }

        private void FireWallSetting_Load(object sender, EventArgs e)
        {
            this.InitTable_WhiteList();
            this.InitTable_BlackList();

            this.cbEnableFireWall.Checked = Operate.ProxyConfig.Proxy.EnableFireWall;
            this.EnableFireWall_Changed();

            if (Operate.ProxyConfig.Proxy.WhiteListMode)
            {
                this.rbWhiteListMode.Checked = true;
            }
            else
            {
                this.rbBlackListMode.Checked = true;
            }
        }

        #endregion

        #region//初始化数据表

        private void InitTable_WhiteList()
        {
            tWhiteList.Columns = new AntdUI.ColumnCollection {
                new AntdUI.Column("IPAddress", "IP地址").SetFixed().SetLocalizationTitleID("Table.WhiteList.Column."),
                new AntdUI.Column("IPLocation", "所属地").SetLocalizationTitleID("Table.WhiteList.Column."),
                new AntdUI.Column("CellLinks", "操作")
                {
                    Render = (value, record, rowindex)=>
                    {
                        return new AntdUI.CellLink[]
                        {
                            new AntdUI.CellButton("bDelete", null, AntdUI.TTypeMini.Error).SetIcon("CloseOutlined"),
                        };
                    },
                }.SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.WhiteList.Column."),
            };

            this.tWhiteList.Binding(Operate.ProxyConfig.Proxy.lstWhiteList);
        }

        private void InitTable_BlackList()
        {
            tBlackList.Columns = new AntdUI.ColumnCollection {
                new AntdUI.Column("IPAddress", "IP地址").SetFixed().SetLocalizationTitleID("Table.BlackList.Column."),
                new AntdUI.Column("IPLocation", "所属地").SetLocalizationTitleID("Table.BlackList.Column."),
                new AntdUI.Column("ExpiryTime", "过期时间").SetSortOrder().SetLocalizationTitleID("Table.BlackList.Column."),
                new AntdUI.Column("CellLinks", "操作")
                {
                    Render = (value, record, rowindex)=>
                    {
                        return new AntdUI.CellLink[]
                        {
                            new AntdUI.CellButton("bDelete", null, AntdUI.TTypeMini.Error).SetIcon("CloseOutlined"),
                        };
                    },
                }.SetFixed().SetWidth("auto").SetLocalizationTitleID("Table.BlackList.Column."),
            };

            this.tBlackList.Binding(Operate.ProxyConfig.Proxy.lstBlackList);
        }

        #endregion

        #region//启用连接控制

        private void cbEnableFireWall_CheckedChanged(object sender, AntdUI.BoolEventArgs e)
        {
            this.EnableFireWall_Changed();
        }

        private void EnableFireWall_Changed()
        { 
            this.rbWhiteListMode.Enabled = 
                this.rbBlackListMode.Enabled = 
                this.tWhiteList.Enabled =
                this.tBlackList.Enabled =
                this.bWhiteList.Enabled =
                this.bBlackList.Enabled =                
                this.cbEnableFireWall.Checked;
        }

        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            Operate.ProxyConfig.Proxy.EnableFireWall = this.cbEnableFireWall.Checked;
            Operate.ProxyConfig.Proxy.WhiteListMode = this.rbWhiteListMode.Checked;

            AntdUI.Message.open(new AntdUI.Message.Config(this.form, "防火墙设置保存成功", TType.Success)
            {
                LocalizationText = "FireWallSetting.Success"
            });

            this.Dispose();
        }

        #endregion

        #region//退出

        private void bExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        #endregion        
    }
}
