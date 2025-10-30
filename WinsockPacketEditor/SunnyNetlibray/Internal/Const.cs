using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Data;
namespace SunnyNetlibray
{
    /// <summary>
    /// 常量类
    /// </summary>
    public class Const
    {
        /// <summary>
        /// WebSocket 消息类型常量。
        /// </summary>
        public static class WsMessageType
        {
            /// <summary>
            /// 文本消息。
            /// </summary>
            public const int Text = 1;

            /// <summary>
            /// 二进制消息。
            /// </summary>
            public const int Binary = 2;

            /// <summary>
            /// 关闭消息。
            /// </summary>
            public const int Close = 8;

            /// <summary>
            /// Ping 消息。
            /// </summary>
            public const int Ping = 9;

            /// <summary>
            /// Pong 消息。
            /// </summary>
            public const int Pong = 10;

            /// <summary>
            /// 无效消息。
            /// </summary>
            public const int Invalid = -1;
        }

        /// <summary>
        /// 强制走TCP规则
        /// </summary>
        public static class MustTcpRule
        {
            /// <summary>
            /// 规则之内走TCP
            /// </summary>
            public const bool Within = false;

            /// <summary>
            /// 规则外走TCP
            /// </summary>
            public const bool Outside = true;
        }


        public const String HTTP2_Fingerprint_Config_Firefox = "{\"ConnectionFlow\":12517377,\"HeaderPriority\":{\"StreamDep\":13,\"Exclusive\":false,\"Weight\":41},\"Priorities\":[{\"PriorityParam\":{\"StreamDep\":0,\"Exclusive\":false,\"Weight\":200},\"StreamID\":3},{\"PriorityParam\":{\"StreamDep\":0,\"Exclusive\":false,\"Weight\":100},\"StreamID\":5},{\"PriorityParam\":{\"StreamDep\":0,\"Exclusive\":false,\"Weight\":0},\"StreamID\":7},{\"PriorityParam\":{\"StreamDep\":7,\"Exclusive\":false,\"Weight\":0},\"StreamID\":9},{\"PriorityParam\":{\"StreamDep\":3,\"Exclusive\":false,\"Weight\":0},\"StreamID\":11},{\"PriorityParam\":{\"StreamDep\":0,\"Exclusive\":false,\"Weight\":240},\"StreamID\":13}],\"PseudoHeaderOrder\":[\":method\",\":path\",\":authority\",\":scheme\"],\"Settings\":{\"1\":65536,\"4\":131072,\"5\":16384},\"SettingsOrder\":[1,4,5]}";
        public const String HTTP2_Fingerprint_Config_Opera = "{\"ConnectionFlow\":15663105,\"HeaderPriority\":null,\"Priorities\":null,\"PseudoHeaderOrder\":[\":method\",\":authority\",\":scheme\",\":path\"],\"Settings\":{\"1\":65536,\"3\":1000,\"4\":6291456,\"6\":262144},\"SettingsOrder\":[1,3,4,6]}";
        public const String HTTP2_Fingerprint_Config_Safari_IOS_17_0 = "{\"ConnectionFlow\":10485760,\"HeaderPriority\":null,\"Priorities\":null,\"PseudoHeaderOrder\":[\":method\",\":scheme\",\":path\",\":authority\"],\"Settings\":{\"2\":0,\"3\":100,\"4\":2097152},\"SettingsOrder\":[2,4,3]}";
        public const String HTTP2_Fingerprint_Config_Safari_IOS_16_0 = "{\"ConnectionFlow\":10485760,\"HeaderPriority\":null,\"Priorities\":null,\"PseudoHeaderOrder\":[\":method\",\":scheme\",\":path\",\":authority\"],\"Settings\":{\"3\":100,\"4\":2097152},\"SettingsOrder\":[4,3]}";
        public const String HTTP2_Fingerprint_Config_Safari = "{\"ConnectionFlow\":10485760,\"HeaderPriority\":null,\"Priorities\":null,\"PseudoHeaderOrder\":[\":method\",\":scheme\",\":path\",\":authority\"],\"Settings\":{\"3\":100,\"4\":4194304},\"SettingsOrder\":[4,3]}";
        public const String HTTP2_Fingerprint_Config_Chrome_117_120_124 = "{\"ConnectionFlow\":15663105,\"HeaderPriority\":null,\"Priorities\":null,\"PseudoHeaderOrder\":[\":method\",\":authority\",\":scheme\",\":path\"],\"Settings\":{\"1\":65536,\"2\":0,\"4\":6291456,\"6\":262144},\"SettingsOrder\":[1,2,4,6]}";
        public const String HTTP2_Fingerprint_Config_Chrome_106_116 = "{\"ConnectionFlow\":15663105,\"HeaderPriority\":null,\"Priorities\":null,\"PseudoHeaderOrder\":[\":method\",\":authority\",\":scheme\",\":path\"],\"Settings\":{\"1\":65536,\"2\":0,\"3\":1000,\"4\":6291456,\"6\":262144},\"SettingsOrder\":[1,2,3,4,6]}";
        public const String HTTP2_Fingerprint_Config_Chrome_103_105 = "{\"ConnectionFlow\":15663105,\"HeaderPriority\":null,\"Priorities\":null,\"PseudoHeaderOrder\":[\":method\",\":authority\",\":scheme\",\":path\"],\"Settings\":{\"1\":65536,\"3\":1000,\"4\":6291456,\"6\":262144},\"SettingsOrder\":[1,3,4,6]}";

    }
}
