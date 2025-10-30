using SunnyNetlibray;
using SunnyNetlibray.Internal;
namespace SunnyNetlibray
{
    /// <summary>
    /// 表示一个简单的键值存储类，允许存取多种类型的值。
    /// </summary>
    public class KeyTable
    {
        private long _context = 0;

        public KeyTable()
        {
            _context = Bridge.CreateKeys();
        }

        ~KeyTable()
        {
            // 自动销毁
            Bridge.RemoveKeys(_context);
        }

        /// <summary>
        /// 重新创建键值表。
        /// </summary>
        public void Reset()
        {
            Bridge.RemoveKeys(_context);
            _context = Bridge.CreateKeys();
        }

        /// <summary>
        /// 写入文本值。
        /// </summary>
        /// <param name="name">键名。</param>
        /// <param name="value">要写入的文本值。</param>
        public void WriteString(string name, string value)
        {
            Bridge.KeysWriteStr(_context, name, value);
        }

        /// <summary>
        /// 写入字节数组值。
        /// </summary>
        /// <param name="name">键名。</param>
        /// <param name="value">要写入的字节数组。</param>
        public void WriteBytes(string name, byte[] value)
        {
            Bridge.KeysWrite(_context, name, value);
        }

        /// <summary>
        /// 写入双精度浮点数值。
        /// </summary>
        /// <param name="name">键名。</param>
        /// <param name="value">要写入的双精度值。</param>
        public void WriteDouble(string name, double value)
        {
            Bridge.KeysWriteFloat(_context, name, value);
        }

        /// <summary>
        /// 写入长整型值。
        /// </summary>
        /// <param name="name">键名。</param>
        /// <param name="value">要写入的长整型值。</param>
        public void WriteLong(string name, long value)
        {
            Bridge.KeysWriteLong(_context, name, value);
        }

        /// <summary>
        /// 写入整型值。
        /// </summary>
        /// <param name="name">键名。</param>
        /// <param name="value">要写入的整型值。</param>
        public void WriteInt(string name, int value)
        {
            Bridge.KeysWriteInt(_context, name, value);
        }

        /// <summary>
        /// 读取整型值。
        /// </summary>
        /// <param name="name">键名。</param>
        /// <returns>读取到的整型值。</returns>
        public int ReadInt(string name)
        {
            return Bridge.KeysReadInt(_context, name);
        }

        /// <summary>
        /// 读取长整型值。
        /// </summary>
        /// <param name="name">键名。</param>
        /// <returns>读取到的长整型值。</returns>
        public long ReadLong(string name)
        {
            return Bridge.KeysReadLong(_context, name);
        }

        /// <summary>
        /// 读取双精度浮点数值。
        /// </summary>
        /// <param name="name">键名。</param>
        /// <returns>读取到的双精度值。</returns>
        public double ReadDouble(string name)
        {
            return Bridge.KeysReadFloat(_context, name);
        }

        /// <summary>
        /// 读取文本值。
        /// </summary>
        /// <param name="name">键名。</param>
        /// <returns>读取到的文本值。</returns>
        public string ReadString(string name)
        {
            byte[] bytes = ReadBytes(name);
            return Tool.BytesToStr(bytes);
        }

        /// <summary>
        /// 读取字节数组值。
        /// </summary>
        /// <param name="name">键名。</param>
        /// <returns>读取到的字节数组。</returns>
        public byte[] ReadBytes(string name)
        {
            return Bridge.KeysRead(_context, name);
        }

        /// <summary>
        /// 清空键值表。
        /// </summary>
        public void Clear()
        {
            Bridge.KeysEmpty(_context);
        }

        /// <summary>
        /// 删除指定键。
        /// </summary>
        /// <param name="name">键名。</param>
        public void Delete(string name)
        {
            Bridge.KeysDelete(_context, name);
        }

        /// <summary>
        /// 转换键值表为 JSON 格式字符串。
        /// </summary>
        /// <returns>JSON 格式字符串。</returns>
        public string ToJson()
        {
            return Bridge.KeysGetJson(_context);
        }

        /// <summary>
        /// 获取键值对的数量。
        /// </summary>
        /// <returns>键值对的数量。</returns>
        public int GetCount()
        {
            return Bridge.KeysGetCount(_context);
        }
    }
}