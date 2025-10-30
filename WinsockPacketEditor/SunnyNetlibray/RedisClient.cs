using SunnyNetlibray.Internal;
using static SunnyNetlibray.WSClient;
using System.Runtime.InteropServices;
using System.Security.Policy;
using SunnyNetRedisClient = SunnyNetlibray.Internal.RedisClient;
using SunnyNetlibray.Event;
using System;
using System.Collections.Generic;

namespace SunnyNetlibray
{
    /// <summary>
    ///  Redis 连接类。
    /// </summary>
    class RedisClient
    {
        private long _context;
        private int _db = 0;
        private IntPtr _errorPtr = Tool.mallocIntptr(256);
        private List<RedisDefaultCallback> _callbacks = new List<RedisDefaultCallback>();

        // Redis 回调委托
        public delegate void RedisDefaultCallback(IntPtr dataPointer);

        public RedisClient()
        {
            _context = Bridge.CreateRedis();
        }

        ~RedisClient()
        {

            Bridge.RedisClose(_context);
            _callbacks.Clear();
            // 自动销毁
            Bridge.RemoveRedis(_context);
            Tool.PtrFree(_errorPtr);
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// 订阅消息
        /// </summary>  
        public void RedisSubscribe(string scribe, SunnyNetRedisClient call)
        {
            if (call == null)
            {
                return;
            }
            RedisDefaultCallback funcCall = (dataPointer) =>
            {
                call.OnCallback(_context, Tool.PtrToString(dataPointer));
            };
            _callbacks.Add(funcCall);//避免  funcCall 被系统GC
            Bridge.RedisSubscribe(_context, scribe, Marshal.GetFunctionPointerForDelegate(funcCall), true);
        }
        /// <summary>
        /// 订阅当前数据库过期消息
        /// </summary>  
        public void SubscribeExpiredMessage(SunnyNetRedisClient call)
        {
            RedisSubscribe("__keyevent@" + _db + "__:expired", call);
        }
        /// <summary>
        /// 获取最近的错误信息。
        /// </summary>
        /// <returns>错误信息字符串。</returns>
        public string GetError()
        {
            return Tool.PtrToString(_errorPtr);
        }

        /// <summary>
        /// 连接到 Redis 服务器。
        /// </summary>
        /// <param name="host">Redis 服务器主机名，例如 "127.0.0.1:6739"。</param>
        /// <param name="password">连接密码。</param>
        /// <param name="database">数据库索引，默认为 0。</param>
        /// <param name="poolSize">连接池大小，默认为 15。</param>
        /// <param name="minConnections">最小连接数，默认为 10。</param>
        /// <param name="connectionTimeout">连接超时时间，单位秒，默认为 5。</param>
        /// <param name="readTimeout">读取超时时间，单位秒，默认为 5。</param>
        /// <param name="writeTimeout">写入超时时间，单位秒，默认为 5。</param>
        /// <param name="poolTimeout">客户端等待可用连接的最大等待时间，单位秒，默认为 5。</param>
        /// <param name="idleCheckPeriod">闲置连接检查周期，单位秒，默认为 60。</param>
        /// <param name="idleTimeout">闲置超时，单位秒，默认为 5。</param>
        /// <returns>连接是否成功。</returns>
        public bool Connect(string host, string password = "", int database = 0, int poolSize = 15, int minConnections = 10,
                            int connectionTimeout = 5, int readTimeout = 5, int writeTimeout = 5,
                            int poolTimeout = 5, int idleCheckPeriod = 60, int idleTimeout = 5)
        {
            _db = database;
            return Bridge.RedisDial(_context, host, password, _db, poolSize, minConnections,
                                    connectionTimeout, readTimeout, writeTimeout,
                                    poolTimeout, idleCheckPeriod, idleTimeout, _errorPtr);
        }

        /// <summary>
        /// 获取指定键的字符串值。
        /// </summary>
        /// <param name="key">键名。</param>
        /// <returns>对应的字符串值。</returns>
        public string GetStringValue(string key)
        {
            return Bridge.RedisGetStr(_context, key);
        }

        /// <summary>
        /// 获取指定键的字节数组值。
        /// </summary>
        /// <param name="key">键名。</param>
        /// <returns>对应的字节数组。</returns>
        public byte[] GetByteArray(string key)
        {
            return Bridge.RedisGetBytes(_context, key);
        }

        /// <summary>
        /// 检查指定键是否存在。
        /// </summary>
        /// <param name="key">键名。</param>
        /// <returns>如果键存在则返回 true；否则返回 false。</returns>
        public bool KeyExists(string key)
        {
            return Bridge.RedisExists(_context, key);
        }

        /// <summary>
        /// 如果键名存在,返回false
        /// </summary>
        /// <param name="key">键名。</param>
        /// <param name="value">要设置的值。</param>
        /// <param name="expiry">过期时间，单位秒，0 表示永不过期。</param>
        /// <returns>设置是否成功。</returns>
        public bool SetIfNotExists(string key, string value, int expiry = 0)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            return Bridge.RedisSetNx(_context, key, value, expiry);
        }

        /// <summary>
        /// 设置指定键的值。
        /// </summary>
        /// <param name="key">键名。</param>
        /// <param name="value">要设置的值。</param>
        /// <param name="expiry">过期时间，单位秒，0 表示永不过期。</param>
        /// <returns>设置是否成功。</returns>
        public bool Set(string key, string value, int expiry = 0)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            return Bridge.RedisSet(_context, key, value, expiry);
        }

        /// <summary>
        /// 设置指定键的字节数组值。
        /// </summary>
        /// <param name="key">键名。</param>
        /// <param name="value">要设置的字节数组值。</param>
        /// <param name="expiry">过期时间，单位秒，0 表示永不过期。</param>
        /// <returns>设置是否成功。</returns>
        public bool SetByteArray(string key, byte[] value, int expiry)
        {
            if (string.IsNullOrEmpty(key))
            {
                return false;
            }
            return Bridge.RedisSetBytes(_context, key, value, expiry);
        }

        /// <summary>
        /// 执行自定义命令并返回操作结果，可能是值或 JSON 文本。
        /// </summary>
        /// <param name="command">执行的命令。</param>
        /// <returns>操作结果字符串。</returns>
        public string Do(string command)
        {
            return Bridge.RedisDo(_context, command, _errorPtr);
        }
        /// <summary>
        /// 若要使用 订阅消息 或 订阅当前数据库过期消息，需执行一次此函数,如果执行后,程序重启了，只要服务端没有重启，就不需要再次执行
        /// </summary>
        public bool EnableEventsNotification()
        {
            Do("config set notify-keyspace-events KEA");
            return Tool.PtrToString(_errorPtr) == "";
        }
        /// <summary>
        /// 根据指定条件获取键名数组。
        /// </summary>
        /// <param name="pattern">匹配模式。</param>
        /// <returns>匹配的键名数组。</returns>
        public string[] GetKeysByPattern(string pattern)
        {
            return Bridge.RedisGetKeys(_context, pattern);
        }

        /// <summary>
        /// 获取指定键的整数值。
        /// </summary>
        /// <param name="key">键名。</param>
        /// <returns>整数值。</returns>
        public long GetIntegerValue(string key)
        {
            return Bridge.RedisGetInt(_context, key);
        }

        /// <summary>
        /// 关闭 Redis 连接，通常在类销毁时自动调用。
        /// </summary>
        public void Close()
        {
            Bridge.RedisClose(_context);
            _callbacks.Clear();
            // 自动销毁
            Bridge.RemoveRedis(_context);
            Tool.PtrFree(_errorPtr);
            _errorPtr = Tool.mallocIntptr(256);
            _context = Bridge.CreateRedis();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 清空整个 Redis 服务器的数据（删除所有数据库的所有键）。
        /// </summary>
        public void FlushAll()
        {
            Bridge.RedisFlushAll(_context);
        }

        /// <summary>
        /// 清空当前数据库中的所有键。
        /// </summary>
        public void FlushDatabase()
        {
            Bridge.RedisFlushDB(_context);
        }

        /// <summary>
        /// 删除指定键。
        /// </summary>
        /// <param name="key">键名。</param>
        /// <returns>删除是否成功。</returns>
        public bool Delete(string key)
        {
            return Bridge.RedisDelete(_context, key);
        }

        /// <summary>
        /// 获取所有键名。
        /// </summary>
        /// <returns>所有的键名数组。</returns>
        public string[] GetAllKeys()
        {
            return GetKeysByPattern("*");
        }
    }
}