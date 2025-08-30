using AntdUI;
using System;

namespace WinsockPacketEditor
{
    public class FilterLogInfo : NotifyProperty
    {
        #region//时间戳

        DateTime _LogTime;

        public DateTime LogTime
        {
            get => _LogTime;
            set
            {
                if (_LogTime == value) return;
                _LogTime = value;
                OnPropertyChanged();
            }
        }

        #endregion        

        #region//滤镜名称

        string _FName;

        public string FName
        {
            get => _FName;
            set
            {
                if (_FName == value) return;
                _FName = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//动作

        Operate.FilterConfig.Filter.FilterAction _FAction;

        public Operate.FilterConfig.Filter.FilterAction FAction
        {
            get => _FAction;
            set
            {
                if (_FAction == value) return;
                _FAction = value;
                OnPropertyChanged();
            }
        }

        #endregion        

        #region//匹配数

        int _MatchNum;

        public int MatchNum
        {
            get => _MatchNum;
            set
            {
                if (_MatchNum == value) return;
                _MatchNum = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//封包类别

        Operate.PacketConfig.Packet.PacketType _PacketType;

        public Operate.PacketConfig.Packet.PacketType PacketType
        {
            get => _PacketType;
            set
            {
                if (_PacketType == value) return;
                _PacketType = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//封包长度

        int _PacketLen;

        public int PacketLen
        {
            get => _PacketLen;
            set
            {
                if (_PacketLen == value) return;
                _PacketLen = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//FilterLogInfo

        public FilterLogInfo(
            string FName, 
            Operate.FilterConfig.Filter.FilterAction FAction, 
            int MatchNum,
            Operate.PacketConfig.Packet.PacketType pType, 
            int PacketLen)
        {
            this._LogTime = DateTime.Now;
            this._FName = FName;
            this._FAction = FAction;
            this._MatchNum = MatchNum;
            this._PacketType = pType;
            this._PacketLen = PacketLen;
        }

        #endregion        
    }
}
