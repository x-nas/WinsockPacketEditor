using AntdUI;
using System;
using System.Windows.Forms;

namespace WinsockPacketEditor
{
    public partial class AutoStoresEdit : UserControl
    {
        private Form form;
        private AutoStoresList aslForm;
        private AutoStoresInfo asiSelect;

        #region//窗体事件

        public AutoStoresEdit(Form form, AutoStoresList aslForm, AutoStoresInfo asiSelect)
        {
            InitializeComponent();
            this.form = form;
            this.aslForm = aslForm;            
            this.asiSelect = asiSelect;
        }

        private void AutoStoresEdit_Load(object sender, EventArgs e)
        {
            if (this.asiSelect == null)
            {
                Operate.SystemConfig.InitWareHouseInfo(this.ddlWareHouse, Guid.Empty);
            }
            else 
            {
                this.txtPacketHead.Text = this.asiSelect.PacketHead;
                Operate.SystemConfig.InitWareHouseInfo(this.ddlWareHouse, this.asiSelect.WID);
            }            
        }

        #endregion

        #region//检查保存设置

        private bool CheckSetting()
        {
            try
            {
                if (string.IsNullOrEmpty(this.txtPacketHead.Text.Trim()))
                {
                    this.txtPacketHead.Status = TType.Error;

                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "指定包头设置错误", TType.Error)
                    {
                        LocalizationText = "AutoStoresEdit.PacketHead.Error"
                    });

                    return false;
                }
                else
                {
                    this.txtPacketHead.Status = TType.Success;
                }

                if (this.ddlWareHouse.SelectedIndex == -1)
                {
                    this.ddlWareHouse.Status = TType.Error;

                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "请选择入库名称", TType.Error)
                    {
                        LocalizationText = "AutoStoresEdit.WareHouse.Error"
                    });

                    return false;
                }
                else
                {
                    this.ddlWareHouse.Status = TType.Success;
                }

                return true;
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(CheckSetting), ex);
            }

            return false;
        }


        #endregion

        #region//保存

        private void bSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!this.CheckSetting())
                {
                    return;
                }

                if (this.ddlWareHouse.SelectedValue is WareHouseInfo whi)
                {
                    string PacketHead = this.txtPacketHead.Text.Trim();

                    if (this.asiSelect == null)
                    {
                        Operate.WareHouseConfig.WareHouse.AddAutoStores(false, PacketHead, whi.WID);
                    }
                    else
                    {
                        Operate.WareHouseConfig.WareHouse.UpdateAutoStores(this.asiSelect, PacketHead, whi.WID);
                    }

                    AntdUI.Message.open(new AntdUI.Message.Config(this.form, "自动入库保存成功", TType.Success)
                    {
                        LocalizationText = "AutoStores.Save.Success"
                    });

                    this.aslForm.RefreshAutoStores();
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(bSave_Click), ex);
            }            
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
