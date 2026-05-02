using AntdUI;
using System;
using System.ComponentModel;

namespace WinsockPacketEditor
{
    public class ServerInfo : NotifyProperty
    {
        #region//是否启用

        bool _IsEnable;

        public bool IsEnable
        {
            get => _IsEnable;
            set
            {
                if (_IsEnable == value) return;
                _IsEnable = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//序号

        Guid _SID;

        public Guid SID
        {
            get => _SID;
            set
            {
                if (_SID == value) return;
                _SID = value;
                OnPropertyChanged();
            }
        }

        #endregion        

        #region//服务器名称

        string _ServerName;

        public string ServerName
        {
            get => _ServerName;
            set
            {
                if (_ServerName == value) return;
                _ServerName = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//服务器IP

        string _ServerIP;

        public string ServerIP
        {
            get => _ServerIP;
            set
            {
                if (_ServerIP == value) return;
                _ServerIP = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//服务器端口

        int _ServerPort;

        public int ServerPort
        {
            get => _ServerPort;
            set
            {
                if (_ServerPort == value) return;
                _ServerPort = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//找回密码链接

        string _ForgotURL;

        public string ForgotURL
        {
            get => _ForgotURL;
            set
            {
                if (_ForgotURL == value) return;
                _ForgotURL = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//立即注册链接

        string _RegisterURL;

        public string RegisterURL
        {
            get => _RegisterURL;
            set
            {
                if (_RegisterURL == value) return;
                _RegisterURL = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//规则信息

        BindingList<RuleInfo> _ServerRInfo = new BindingList<RuleInfo>();

        public BindingList<RuleInfo> ServerRInfo
        {
            get => _ServerRInfo;
            set
            {
                if (_ServerRInfo == value) return;
                _ServerRInfo = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//ServerInfo

        public ServerInfo(
            bool IsEnable, 
            Guid SID, 
            string ServerName, 
            string ServerIP, 
            int ServerPort, 
            string ForgotURL, 
            string RegisterURL,
            BindingList<RuleInfo> ServerRInfo)
        {
            this._IsEnable = IsEnable;
            this._SID = SID;
            this._ServerName = ServerName;
            this._ServerIP = ServerIP;
            this._ServerPort = ServerPort;
            this._ForgotURL = ForgotURL;
            this._RegisterURL = RegisterURL;
            this._ServerRInfo = ServerRInfo;
        }

        #endregion
    }
}
