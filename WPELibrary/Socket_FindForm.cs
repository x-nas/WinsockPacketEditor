using Be.Windows.Forms;
using System;
using System.Reflection;
using System.Windows.Forms;
using WPE.Lib;

namespace WPELibrary
{
    public partial class Socket_FindForm : Form
    {
        private bool isValid = false;

        #region//窗体初始化

        public Socket_FindForm()
        {
            MultiLanguage.SetDefaultLanguage(MultiLanguage.DefaultLanguage);
            InitializeComponent();
            InitForm();
        }        

        private void InitForm()
        {
            try
            {
                Operate.PacketConfig.List.DoSearch = false;                

                if (Operate.PacketConfig.List.FindOptions.Type == FindType.Text)
                { 
                    this.rbString.Checked = true;                    
                }
                else if(Operate.PacketConfig.List.FindOptions.Type == FindType.Hex)
                {                    
                    this.rbHex.Checked = true;                    
                }

                this.txtFind.Text = Operate.PacketConfig.List.FindOptions.Text;
                //this.chkMatchCase.Checked = Operate.PacketConfig.List.FindOptions.MatchCase;              

                if (Operate.PacketConfig.List.FindOptions.Hex != null && Operate.PacketConfig.List.FindOptions.Hex.Length > 0)
                {
                    hexFind.ByteProvider = new DynamicByteProvider(Operate.PacketConfig.List.FindOptions.Hex);                   
                }
                else
                {
                    byte[] bNew = new byte[0];
                    hexFind.ByteProvider = new DynamicByteProvider(bNew);
                }                
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//数据校验

        private void ValidateFind()
        {
            try
            {                
                isValid = false;                               

                if (rbString.Checked && txtFind.Text.Length > 0)
                {
                    isValid = true;
                }

                if (rbHex.Checked && hexFind.ByteProvider.Length > 0)
                {                    
                    isValid = true;
                }                
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        #endregion

        #region//查找按钮

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                this.ValidateFind();

                if (this.isValid)
                {
                    if (rbString.Checked)
                    {
                        Operate.PacketConfig.List.FindOptions.Type = FindType.Text;
                    }
                    else
                    {
                        Operate.PacketConfig.List.FindOptions.Type = FindType.Hex;
                    }

                    Operate.PacketConfig.List.FindOptions.Text = txtFind.Text;
                    //Operate.PacketConfig.List.FindOptions.MatchCase = chkMatchCase.Checked;

                    DynamicByteProvider dbp = this.hexFind.ByteProvider as DynamicByteProvider;

                    if (dbp != null && dbp.Bytes.Count > 0)
                    {
                        Operate.PacketConfig.List.FindOptions.Hex = dbp.Bytes.ToArray();
                    }

                    Operate.PacketConfig.List.FindOptions.IsValid = true;
                    Operate.PacketConfig.List.DoSearch = true;
                    this.Close();
                }
                else
                {
                    Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_30));
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }       

        #endregion

        #region//取消按钮

        private void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {                
                this.Close();
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//查找类型切换        

        private void rbString_CheckedChanged(object sender, EventArgs e)
        {
            this.FindTypeChanged();
        }

        private void rbHex_CheckedChanged(object sender, EventArgs e)
        {
            this.FindTypeChanged();
        }

        private void FindTypeChanged()
        {
            try
            {
                if (rbString.Checked)
                {  
                    this.txtFind.Enabled = true;
                    this.hexFind.Enabled = false;

                    this.txtFind.Focus();
                }
                else if(rbHex.Checked)
                {  
                    this.hexFind.Enabled = true;
                    this.txtFind.Enabled = false;

                    this.hexFind.Focus();
                }                
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion        
    }
}
