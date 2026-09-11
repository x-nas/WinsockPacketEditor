using System;
using System.Threading;

namespace WinsockPacketEditor
{
    public class BlackListInfo : NotifyProperty, IIpRule
    {
        #region//IP地址

        string _IPAddress;

        public string IPAddress
        {
            get => _IPAddress;
            set
            {
                if (_IPAddress == value) return;
                _IPAddress = value;
                OnPropertyChanged();
            }
        }

        long _StartIP;

        public long StartIP
        {
            get => _StartIP;
            set
            {
                if (_StartIP == value) return;
                _StartIP = value;
                OnPropertyChanged();
            }
        }

        long _EndIP;

        public long EndIP
        {
            get => _EndIP;
            set
            {
                if (_EndIP == value) return;
                _EndIP = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//所属地

        string _IPLocation;

        public string IPLocation
        {
            get => _IPLocation;
            set
            {
                if (_IPLocation == value) return;
                _IPLocation = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//生效次数

        long _EffectCount;

        public long EffectCount
        {
            get => _EffectCount;
            set
            {
                if (_EffectCount == value) return;
                _EffectCount = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//是否过期

        bool _IsExpiry;

        public bool IsExpiry
        {
            get => _IsExpiry;
            set
            {
                if (_IsExpiry == value) return;
                _IsExpiry = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//过期时间

        DateTime _ExpiryTime;

        public DateTime ExpiryTime
        {
            get => _ExpiryTime;
            set
            {
                if (_ExpiryTime == value) return;
                _ExpiryTime = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//创建时间

        DateTime _CreateTime;

        public DateTime CreateTime
        {
            get => _CreateTime;
            set
            {
                if (_CreateTime == value) return;
                _CreateTime = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region//BlackListInfo

        public BlackListInfo(
            string IPAddress, 
            string IPLocation, 
            bool IsExpiry, 
            DateTime ExpiryTime,
            DateTime CreateTime)
        {
            this._IPAddress = IPAddress;
            this._IPLocation = IPLocation;
            this._EffectCount = 0;
            this._IsExpiry = IsExpiry;
            this._ExpiryTime = ExpiryTime;
            this._CreateTime = CreateTime;

            var ParseResult = Operate.ProxyConfig.Proxy.ParseIpRange(IPAddress);
            this._StartIP = ParseResult.StartIP;
            this._EndIP = ParseResult.EndIP;
        }

        #endregion

        #region//ContainsIp

        public bool ContainsIp(long ipValue)
        {
            return this._StartIP != -1 && this._EndIP != -1 && ipValue >= this._StartIP && ipValue <= this._EndIP;
        }

        /// <summary>
        /// 命中一次（<see cref="IIpRule.HitOnce"/>）。
        ///
        /// <para>
        /// ⚠️ <b>直接改后备字段，两条都是刻意的</b>：
        /// </para>
        /// <para>
        /// ① 用 <c>Interlocked</c>：这里跑在 SuperSocket 的<b>连接线程</b>上，
        /// 多条连接会并发命中同一条规则，而 <c>long</c> 的 <c>++</c> 是读-改-写三步，
        /// 丢掉的计数<b>再也补不回来</b>（与代理列表那六个计数器同一条教训）。
        /// </para>
        /// <para>
        /// ② <b>不走属性、不发 OnPropertyChanged</b>：那会经 <c>BindingList.ItemChanged</c>
        /// 冒到 WinForms 那张绑着这份列表的表格上 —— 跨线程改绑定列表是崩溃路径。
        /// 「生效次数」这一列由 1 秒统计拍统一标脏刷新，晚一拍无所谓。
        /// </para>
        /// </summary>
        public void HitOnce()
        {
            Interlocked.Increment(ref this._EffectCount);
        }

        #endregion        
    }
}
