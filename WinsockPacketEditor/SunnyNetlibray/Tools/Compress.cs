using SunnyNetlibray.Internal;

namespace SunnyNetlibray
{
    /// <summary>
    /// 提供数据压缩和解压缩功能，支持多种算法。
    /// </summary>
    public class Compress
    {
        /// <summary>
        /// 使用 Brotli 算法解压缩数据。
        /// </summary>
        /// <param name="data">需要解压缩的字节数组。</param>
        /// <returns>解压缩后的字节数组。</returns>
        public static byte[] BrUnCompress(byte[] data)
        {
            return Bridge.BrUnCompress(data);
        }

        /// <summary>
        /// 使用 Brotli 算法压缩数据。
        /// </summary>
        /// <param name="data">需要压缩的字节数组。</param>
        /// <returns>压缩后的字节数组。</returns>
        public static byte[] BrCompress(byte[] data)
        {
            return Bridge.BrCompress(data);
        }

        /// <summary>
        /// 使用 Deflate 算法解压缩数据。
        /// </summary>
        /// <param name="data">需要解压缩的字节数组。</param>
        /// <returns>解压缩后的字节数组。</returns>
        public static byte[] DeflateUnCompress(byte[] data)
        {
            return Bridge.DeflateUnCompress(data);
        }

        /// <summary>
        /// 使用 Deflate 算法压缩数据。
        /// </summary>
        /// <param name="data">需要压缩的字节数组。</param>
        /// <returns>压缩后的字节数组。</returns>
        public static byte[] DeflateCompress(byte[] data)
        {
            return Bridge.DeflateCompress(data);
        }

        /// <summary>
        /// 使用 Gzip 算法解压缩数据。
        /// </summary>
        /// <param name="data">需要解压缩的字节数组。</param>
        /// <returns>解压缩后的字节数组。</returns>
        public static byte[] GzipUnCompress(byte[] data)
        {
            return Bridge.GzipUnCompress(data);
        }

        /// <summary>
        /// 使用 Gzip 算法压缩数据。
        /// </summary>
        /// <param name="data">需要压缩的字节数组。</param>
        /// <returns>压缩后的字节数组。</returns>
        public static byte[] GzipCompress(byte[] data)
        {
            return Bridge.GzipCompress(data);
        }

        /// <summary>
        /// 使用 Zlib 算法解压缩数据。
        /// </summary>
        /// <param name="data">需要解压缩的字节数组。</param>
        /// <returns>解压缩后的字节数组。</returns>
        public static byte[] ZlibUnCompress(byte[] data)
        {
            return Bridge.ZlibUnCompress(data);
        }

        /// <summary>
        /// 使用 Zlib 算法压缩数据。
        /// </summary>
        /// <param name="data">需要压缩的字节数组。</param>
        /// <returns>压缩后的字节数组。</returns>
        public static byte[] ZlibCompress(byte[] data)
        {
            return Bridge.ZlibCompress(data);
        }

        /// <summary>
        /// 使用 Zstandard 算法解压缩数据。
        /// </summary>
        /// <param name="data">需要解压缩的字节数组。</param>
        /// <returns>解压缩后的字节数组。</returns>
        public static byte[] ZSTDUnCompress(byte[] data)
        {
            return Bridge.ZSTDUnCompress(data);
        }

        /// <summary>
        /// 使用 Zstandard 算法压缩数据。
        /// </summary>
        /// <param name="data">需要压缩的字节数组。</param>
        /// <returns>压缩后的字节数组。</returns>
        public static byte[] ZSTDCompress(byte[] data)
        {
            return Bridge.ZSTDCompress(data);
        }
    }
}