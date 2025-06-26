﻿using System;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using WPE.Lib;

namespace WPELibrary
{
    public partial class Proxy_AccountAuthForm : Form
    {
        private BindingSource _authBindingSource = new BindingSource();

        #region//窗体事件

        public Proxy_AccountAuthForm()
        {
            InitializeComponent();

            this.InitDGV();
        }

        private void Proxy_AccountAuthForm_Load(object sender, EventArgs e)
        {
            Operate.ProxyConfig.ProxyAccount.IsShow_ProxyAuth = true;
        }

        private void Proxy_AccountAuthForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Operate.ProxyConfig.ProxyAccount.IsShow_ProxyAuth = false;
        }

        private void InitDGV()
        {
            try
            {
                _authBindingSource.DataSource = Operate.ProxyConfig.ProxyAccount.lstProxyAuth;

                dgvAuth.AutoGenerateColumns = false;
                dgvAuth.DataSource = _authBindingSource;
                dgvAuth.GetType().GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(dgvAuth, true, null);
                Operate.ProxyConfig.ProxyAccount.RecProxyAuth += new Operate.ProxyConfig.ProxyAccount.ProxyAuthReceived(Event_RecProxyAuth);
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//显示认证列表（异步）        

        private void Event_RecProxyAuth(Proxy_AuthInfo pai)
        {
            try
            {
                if (this.IsDisposed || dgvAuth.IsDisposed)
                    return;

                this.BeginInvoke(new Action(() =>
                {
                    try
                    {
                        if (this.IsDisposed || dgvAuth.IsDisposed)
                            return;

                        var existingItem = Operate.ProxyConfig.ProxyAccount.lstProxyAuth
                            .FirstOrDefault(item => item.IPAddress == pai.IPAddress && item.AID == pai.AID);

                        if (existingItem != null)
                        {
                            existingItem.AuthResult = pai.AuthResult;
                            existingItem.AuthTime = pai.AuthTime;
                        }
                        else
                        {
                            Operate.ProxyConfig.ProxyAccount.lstProxyAuth.Add(pai);
                        }

                        _authBindingSource.ResetBindings(false);
                        this.ShowAuthListInfo();
                    }
                    catch (Exception ex)
                    {
                        Operate.DoLog("Event_RecProxyAuth(inner)", ex.Message);
                    }
                }));
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        private void dgvAuth_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == dgvAuth.Columns["cAuthID"].Index)
                {
                    e.Value = (e.RowIndex + 1).ToString();
                    e.FormattingApplied = true;
                }
                else if (e.ColumnIndex == dgvAuth.Columns["cUserName"].Index)
                {
                    Guid AID = Guid.Parse(dgvAuth.Rows[e.RowIndex].Cells["cAID"].Value.ToString());
                    e.Value = Operate.ProxyConfig.ProxyAccount.GetUserName_ByAccountID(AID);
                    e.FormattingApplied = true;
                }
                else if (e.ColumnIndex == dgvAuth.Columns["cAuthResult"].Index)
                {
                    //e.Value = Operate.ProxyConfig.ProxyAccount.GetImg_ByAuthResult(Convert.ToBoolean(e.Value));
                    e.FormattingApplied = true;
                }
            }
            catch (Exception ex)
            {
                Operate.DoLog(MethodBase.GetCurrentMethod().Name, ex.Message);
            }
        }

        #endregion

        #region//显示认证列表信息

        private void ShowAuthListInfo()
        { 
            this.tsslAuthCount_Value.Text = Operate.ProxyConfig.ProxyAccount.lstProxyAuth.Count.ToString();
            this.tsslLinksCount_Value.Text = Operate.ProxyConfig.ProxyAccount.GetLinksCount_FromProxyAuthList().ToString();
            this.tsslDevicesCount_Value.Text = Operate.ProxyConfig.ProxyAccount.GetDevicesCount_FromProxyAuthList().ToString();
        }

        #endregion

        #region//查找下一个

        private void bSearch_Account_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.dgvAuth.Rows.Count == 0)
                {
                    return;
                }

                string FindString = this.txtSearch_Account.Text.Trim();
                if (string.IsNullOrEmpty(FindString))
                {
                    return;
                }

                int FromIndex = 0;
                if (this.dgvAuth.SelectedRows.Count > 0)
                {
                    FromIndex = this.dgvAuth.SelectedRows[0].Index + 1;
                }

                int iIndex = Operate.ProxyConfig.ProxyAccount.SearchForProxyAuthList(FromIndex, FindString);
                if (iIndex >= 0 && iIndex < this.dgvAuth.Rows.Count)
                {
                    this.dgvAuth.Rows[iIndex].Selected = true;
                    this.dgvAuth.CurrentCell = this.dgvAuth.Rows[iIndex].Cells[0];
                    this.dgvAuth.FirstDisplayedScrollingRowIndex = iIndex;
                }
                else
                {
                    Socket_Operation.ShowMessageBox(MultiLanguage.GetDefaultLanguage(MultiLanguage.MutiLan_23));
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
