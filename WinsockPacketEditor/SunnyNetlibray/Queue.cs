using SunnyNetlibray.Internal;
using SunnyNetTCPClient = SunnyNetlibray.Internal.TCPClient;
namespace SunnyNetlibray
{
    /// <summary>
    /// 表示一个队列类，参考易语言用法。
    /// </summary>
    public class Queue
    {
        private string _context;

        /// <summary>
        /// 设置队列的唯一标识。
        /// </summary>
        /// <param name="identifier">唯一标识字符串。</param>
        public void SetUniqueIdentifier(string identifier)
        {
            _context = identifier;
        }

        /// <summary>
        /// 检查队列是否为空。
        /// </summary>
        /// <returns>如果队列为空，则返回 true；否则返回 false。</returns>
        public bool IsEmpty()
        {
            return Bridge.QueueIsEmpty(_context);
        }

        /// <summary>
        /// 清空队列。
        /// </summary>
        public void Clear()
        {
            Destroy();
            Create(string.Empty);
        }

        /// <summary>
        /// 手动销毁队列，释放资源。
        /// </summary>
        public void Destroy()
        {
            Bridge.QueueRelease(_context);
        }

        /// <summary>
        /// 创建队列。
        /// </summary>
        /// <param name="id">队列的唯一标识符。</param>
        public void Create(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                _context = id;
                Bridge.CreateQueue(_context);
            }
        }

        /// <summary>
        /// 获取队列的长度。
        /// </summary>
        /// <returns>队列中的元素数量。</returns>
        public int GetQueueLength()
        {
            return Bridge.QueueLength(_context);
        }

        /// <summary>
        /// 将字节数组压入队列。
        /// </summary>
        /// <param name="data">要压入队列的字节数组。</param>
        public void PushBytes(byte[] data)
        {
            Bridge.QueuePush(_context, data);
        }

        /// <summary>
        /// 将字符串压入队列。
        /// </summary>
        /// <param name="text">要压入队列的字符串。</param>
        public void Push(string text)
        {
            byte[] byteArray = Tool.StrToBytes(text);
            Bridge.QueuePush(_context, byteArray);
        }

        /// <summary>
        /// 从队列中弹出一个字符串。
        /// </summary>
        /// <returns>弹出的字符串。</returns>
        public string Pop()
        {
            byte[] data = PopBytes();
            return Tool.BytesToStr(data);
        }

        /// <summary>
        /// 从队列中弹出字节数组。
        /// </summary>
        /// <returns>弹出的字节数组。</returns>
        public byte[] PopBytes()
        {
            return Bridge.QueuePull(_context);
        }
    }
}