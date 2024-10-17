using Be.Windows.Forms;
using System;
using System.Reflection;
using System.Windows.Forms;
using WPELibrary.Lib;

namespace WPELibrary
{
    public partial class Socket_FindForm : Form
    {
        #region//窗体初始化

        public Socket_FindForm()
        {
            InitializeComponent();
            InitForm();
        }        

        private void InitForm()
        {
            try
            {
                Socket_Cache.DoSearch = false;                

                if (Socket_Cache.FindOptions.Type == FindType.Text)
                { 
                    this.rbString.Checked = true;                    
                }
                else if(Socket_Cache.FindOptions.Type == FindType.Hex)
                {                    
                    this.rbHex.Checked = true;                    
                }

                this.txtFind.Text = Socket_Cache.FindOptions.Text;
                this.chkMatchCase.Checked = Socket_Cache.FindOptions.MatchCase;              

                if (Socket_Cache.FindOptions.Hex != null && Socket_Cache.FindOptions.Hex.Length > 0)
                {
                    hexFind.ByteProvider = new DynamicByteProvider(Socket_Cache.FindOptions.Hex);                   
                }
                else
                {
                    byte[] bNew = new byte[0];
                    hexFind.ByteProvider = new DynamicByteProvider(bNew);
                }
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//数据校验

        private void txtFind_TextChanged(object sender, EventArgs e)
        {
            this.ValidateFind();
        }

        private void hexFind_ByteProviderChanged(object sender, EventArgs e)
        {
            this.ValidateFind();
        }

        private void ValidateFind()
        {
            try
            {
                bool isValid = false;

                if (rbString.Checked && txtFind.Text.Length > 0)
                {
                    isValid = true;
                }

                if (rbHex.Checked && hexFind.ByteProvider.Length > 0)
                {
                    isValid = true;
                }

                this.btnOK.Enabled = isValid;
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }            
        }

        #endregion

        #region//查找按钮

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (rbString.Checked)
                {
                    Socket_Cache.FindOptions.Type = FindType.Text;                    
                }
                else
                {
                    Socket_Cache.FindOptions.Type = FindType.Hex;                    
                }

                Socket_Cache.FindOptions.Text = txtFind.Text;
                Socket_Cache.FindOptions.MatchCase = chkMatchCase.Checked;

                DynamicByteProvider dbp = this.hexFind.ByteProvider as DynamicByteProvider;

                if (dbp != null && dbp.Bytes.Count > 0)
                {
                    Socket_Cache.FindOptions.Hex = dbp.Bytes.ToArray();
                }

                Socket_Cache.FindOptions.IsValid = true;
                Socket_Cache.DoSearch = true;
                this.Close();
            }
            catch (Exception ex)
            {
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
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
                Socket_Operation.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion        
    }
}
