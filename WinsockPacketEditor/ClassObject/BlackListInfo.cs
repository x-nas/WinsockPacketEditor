using AntdUI;
using System;

namespace WinsockPacketEditor
{
    public class BlackListInfo : NotifyProperty
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

        long _startIp;

        public long StartIp
        {
            get => _startIp;
            set
            {
                if (_startIp == value) return;
                _startIp = value;
                OnPropertyChanged();
            }
        }

        long _endIp;

        public long EndIp
        {
            get => _endIp;
            set
            {
                if (_endIp == value) return;
                _endIp = value;
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

        #region//BlackListInfo

        public BlackListInfo(string IPAddress, string IPLocation, bool IsExpiry, DateTime ExpiryTime)
        {
            this.IPAddress = IPAddress;
            this.IPLocation = IPLocation;
            this.IsExpiry = IsExpiry;
            this.ExpiryTime = ExpiryTime;

            this.ParseIpRange(IPAddress);
        }

        #endregion

        #region//ContainsIp

        public bool ContainsIp(long ipValue)
        {
            return this.StartIp != -1 && this.EndIp != -1 && ipValue >= this.StartIp && ipValue <= this.EndIp;
        }

        #endregion

        #region//ParseIpRange

        private void ParseIpRange(string ipAddress)
        {
            try
            {
                // 支持单个IP和IP范围（如：192.168.1.1 或 192.168.1.1-192.168.1.100）
                if (ipAddress.Contains("-"))
                {
                    var parts = ipAddress.Split('-');
                    if (parts.Length == 2)
                    {
                        this.StartIp = Operate.ProxyConfig.Proxy.ConvertIpToLong(parts[0].Trim());
                        this.EndIp = Operate.ProxyConfig.Proxy.ConvertIpToLong(parts[1].Trim());
                    }
                }
                else if (ipAddress.Contains("/"))
                {
                    // 支持CIDR格式（如：192.168.1.0/24）
                    var cidrResult = Operate.ProxyConfig.Proxy.ParseCidr(ipAddress);
                    if (cidrResult != null)
                    {
                        this.StartIp = cidrResult.Value.Start;
                        this.EndIp = cidrResult.Value.End;
                    }
                }
                else
                {
                    // 单个IP
                    long ipLong = Operate.ProxyConfig.Proxy.ConvertIpToLong(ipAddress.Trim());
                    this.StartIp = ipLong;
                    this.EndIp = ipLong;
                }
            }
            catch (Exception ex)
            {
                this.StartIp = -1;
                this.EndIp = -1;

                Operate.DoLog(nameof(ParseIpRange), ex.Message);
            }
        }

        #endregion
    }
}
