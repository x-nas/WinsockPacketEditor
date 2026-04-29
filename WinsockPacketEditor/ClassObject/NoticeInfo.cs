using AntdUI;
using System;

namespace WinsockPacketEditor
{
    public class NoticeInfo : NotifyProperty
    {
        #region//序号

        Guid _NID;

        public Guid NID
        {
            get => _NID;
            set
            {
                if (_NID == value) return;
                _NID = value;
                OnPropertyChanged();
            }
        }

        #endregion        

        #region//类型

        int _NoticeType;

        public int NoticeType
        {
            get => _NoticeType;
            set
            {
                if (_NoticeType == value) return;
                _NoticeType = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//标题

        string _NoticeTitle;

        public string NoticeTitle
        {
            get => _NoticeTitle;
            set
            {
                if (_NoticeTitle == value) return;
                _NoticeTitle = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//内容

        string _NoticeContent;

        public string NoticeContent
        {
            get => _NoticeContent;
            set
            {
                if (_NoticeContent == value) return;
                _NoticeContent = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//更多详情链接

        string _NoticeMore;

        public string NoticeMore
        {
            get => _NoticeMore;
            set
            {
                if (_NoticeMore == value) return;
                _NoticeMore = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//发布时间

        DateTime _NoticeTime;

        public DateTime NoticeTime
        {
            get => _NoticeTime;
            set
            {
                if (_NoticeTime == value) return;
                _NoticeTime = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//NoticeInfo

        public NoticeInfo(Guid NID, int noticeType, string noticeTitle, string noticeContent, string noticeMore, DateTime noticeTime)
        {
            this._NID = NID;
            this._NoticeType = noticeType;
            this._NoticeTitle = noticeTitle;
            this._NoticeContent = noticeContent;
            this._NoticeMore = noticeMore;
            this._NoticeTime = noticeTime;
        }

        #endregion
    }
}
