using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace WinsockPacketEditor
{
    public class IPHelper
    {
        //IP库文件地址
        private readonly string mLibraryFilePath;
        //第一条索引的绝对地址
        private readonly uint mFirstIndex;
        //最后一条索引的绝对地址
        private readonly uint mLastIndex;

        public IPHelper()
        {
            mLibraryFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IPLocation", "qqwry.dat");

            //定位索引区
            using (var fs = new FileStream(mLibraryFilePath, FileMode.Open, FileAccess.Read))
            {
                var reader = new BinaryReader(fs);
                //文件头
                var header = reader.ReadBytes(IPFormat.HeaderLength);
                mFirstIndex = BitConverter.ToUInt32(header, 0);
                mLastIndex = BitConverter.ToUInt32(header, 4);
            }
        }

        /// <summary>
        /// 获取IP的归属地
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <returns></returns>
        public IPLocation GetIpLocation(IPAddress ip)
        {
            using (var fs = new FileStream(mLibraryFilePath, FileMode.Open, FileAccess.Read))
            {
                var reader = new BinaryReader(fs);
                //从大端顺序转为小端顺序
                var ipBytes = BitConverter.GetBytes(IPAddress.NetworkToHostOrder(BitConverter.ToInt32(ip.GetAddressBytes(), 0)));
                var offset = FindIpStartPos(fs, reader, mFirstIndex, mLastIndex, ipBytes);
                return GetIPInfo(fs, reader, offset, ip, ipBytes);
            }
        }

        /// <summary>
        /// 在索引区中查找目标IP的索引的起始位置
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="reader"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        private uint FindIpStartPos(FileStream fs, BinaryReader reader, uint startIndex, uint endIndex, byte[] ip)
        {
            var ipVal = BitConverter.ToUInt32(ip, 0);
            fs.Position = startIndex;

            while (fs.Position <= endIndex)
            {
                var bytes = reader.ReadBytes(IPFormat.IndexRecLength);
                var curVal = BitConverter.ToUInt32(bytes, 0);
                if (curVal > ipVal)
                {
                    fs.Position = fs.Position - 2 * IPFormat.IndexRecLength;
                    bytes = reader.ReadBytes(IPFormat.IndexRecLength);
                    var offsetByte = new byte[4];
                    Array.Copy(bytes, 4, offsetByte, 0, 3);
                    return BitConverter.ToUInt32(offsetByte, 0);
                }
            }

            return 0;
        }

        /// <summary>
        /// 读取目标IP的信息
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="reader"></param>
        /// <param name="offset"></param>
        /// <param name="ipToLoc"></param>
        /// <param name="ipBytes"></param>
        /// <returns></returns>
        private IPLocation GetIPInfo(FileStream fs, BinaryReader reader, long offset, IPAddress ipToLoc, byte[] ipBytes)
        {
            fs.Position = offset;
            //确认目标IP在记录的IP范围内
            var endIP = reader.ReadBytes(4);
            var endIpVal = BitConverter.ToUInt32(endIP, 0);
            var ipVal = BitConverter.ToUInt32(ipBytes, 0);
            if (endIpVal < ipVal) return null;

            string country;
            string zone;
            //读取重定向模式字节
            var pattern = reader.ReadByte();
            if (pattern == RedirectMode.Mode_1)
            {
                var countryOffsetBytes = reader.ReadBytes(IPFormat.RecOffsetLength);
                var countryOffset = IPFormat.ToUint(countryOffsetBytes);

                if (countryOffset == 0) return GetUnknownLocation(ipToLoc);

                fs.Position = countryOffset;
                if (fs.ReadByte() == RedirectMode.Mode_2)
                {
                    return ReadMode2Record(fs, reader, ipToLoc);
                }
                fs.Position--;
                country = ReadString(reader);
                zone = ReadZone(fs, reader, Convert.ToUInt32(fs.Position));
            }
            else if (pattern == RedirectMode.Mode_2)
            {
                return ReadMode2Record(fs, reader, ipToLoc);
            }
            else
            {
                fs.Position--;
                country = ReadString(reader);
                zone = ReadZone(fs, reader, Convert.ToUInt32(fs.Position));
            }
            return new IPLocation(ipToLoc, country, zone);
        }

        /// <summary>
        /// 获取识别失败的结果
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        private IPLocation GetUnknownLocation(IPAddress ip)
        {
            var country = IPFormat.UnknownCountry;
            var zone = IPFormat.UnknownZone;
            return new IPLocation(ip, country, zone);
        }

        /// <summary>
        /// 按模式2读取记录
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="reader"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        private IPLocation ReadMode2Record(FileStream fs, BinaryReader reader, IPAddress ip)
        {
            var countryOffset = IPFormat.ToUint(reader.ReadBytes(IPFormat.RecOffsetLength));
            var curOffset = Convert.ToUInt32(fs.Position);
            if (countryOffset == 0) return GetUnknownLocation(ip);
            fs.Position = countryOffset;
            var country = ReadString(reader);
            var zone = ReadZone(fs, reader, curOffset);
            return new IPLocation(ip, country, zone);
        }

        /// <summary>
        /// 从二进制文件中读取字符串
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        private string ReadString(BinaryReader reader)
        {
            var stringLst = new List<byte>();
            byte byteRead = 0;
            while ((byteRead = reader.ReadByte()) != 0)
            {
                stringLst.Add(byteRead);
            }
            return Encoding.GetEncoding("gb2312").GetString(stringLst.ToArray());
        }

        /// <summary>
        /// 读取区域信息
        /// </summary>
        /// <param name="fs"></param>
        /// <param name="reader"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private string ReadZone(FileStream fs, BinaryReader reader, uint offset)
        {
            fs.Position = offset;
            var b = reader.ReadByte();
            if (b == RedirectMode.Mode_1 || b == RedirectMode.Mode_2)
            {
                var zoneOffset = IPFormat.ToUint(reader.ReadBytes(3));
                if (zoneOffset == 0) return IPFormat.UnknownZone;
                return ReadZone(fs, reader, zoneOffset);
            }
            fs.Position--;
            return ReadString(reader);
        }
    }

    public class IPLocation
    {
        public IPLocation(IPAddress ip, string country, string loc)
        {
            IP = ip;
            Country = country;
            Zone = loc;
        }

        public IPAddress IP { get; }

        public string Country { get; }

        public string Zone { get; }
    }

    public class IPFormat
    {
        //文件头为8个字节
        public static readonly int HeaderLength = 8;
        //一条索引的长度
        public static readonly int IndexRecLength = 7;
        public static readonly int IndexOffset = 3;
        public static readonly int RecOffsetLength = 3;

        public static readonly string UnknownCountry = "未知的国家";
        public static readonly string UnknownZone = "未知的地区";

        public static uint ToUint(byte[] val)
        {
            if (val.Length > 4) throw new ArgumentException();
            if (val.Length < 4)
            {
                var copyBytes = new byte[4];
                Array.Copy(val, 0, copyBytes, 0, val.Length);
                return BitConverter.ToUInt32(copyBytes, 0);
            }
            return BitConverter.ToUInt32(val, 0);
        }
    }

    public class RedirectMode
    {
        public static readonly int Mode_1 = 1;
        public static readonly int Mode_2 = 2;
    }
}
