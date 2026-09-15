using System;
using SuperSocket.SocketBase.Protocol;

namespace WinsockPacketEditor
{
    /// <summary>
    /// SOCKS5 的接收过滤器 —— <b>按 SuperSocket 1.6 的 IReceiveFilter 契约</b>切帧。
    ///
    /// 【契约】Filter 收到一段字节后：
    ///   · 切得出一帧就返回它（BinaryRequestInfo），并用 rest 告诉 SuperSocket 这段里还剩多少字节没用 ——
    ///     它会拿着剩下的部分再调一次 Filter，直到 rest 为 0；
    ///   · 切不出就返回 null，把不完整的部分存进 m_Buffer（LeftBufferSize 报告它的长度，
    ///     SuperSocket 拿它跟 MaxRequestLength 比）；
    ///   · 报文格式不对就置 State = Error，SuperSocket 以 ProtocolError 关会话。
    /// 返回的帧经 AppServer.NewRequestReceived 送到 ProxySession.OnFrame（见 SocksProxyServer），
    /// SuperSocket 顺手更新 LastActiveTime，空闲清理因此能看到「客户端在发」。
    ///
    /// 【为什么重写】2026-09-14 之前这个类永远返回 null、rest = 0，自己拼帧后 <c>_ = ProcessCombinedData()</c> 放飞。
    /// 三个后果：命令步 await 期间到达的数据会被当成命令帧解析并在任务完成时被清掉（客户端紧接 CONNECT 发数据就丢头几字节）；
    /// MaxRequestLength 形同虚设、非握手步的缓冲无上限；LastActiveTime 只在服务端往外发时更新。
    ///
    /// 【步骤由过滤器自己推进】握手帧一切出来就按 SelectAuthMethod 决定下一步，不等异步的握手处理完 ——
    /// 这样粘在后面的认证 / 命令 / 数据都能按正确的帧长切，顺序由 ProxySession 的帧队列保证。
    /// </summary>
    public class Socks5ProxyReceiveFilter : IReceiveFilter<BinaryRequestInfo>
    {
        /// <summary>握手 / 认证 / 命令三步一帧最长 4 + 1 + 255 + 2 = 262 字节；超过 1 KB 还切不出帧就是垃圾。</summary>
        private const int MaxHeaderBytes = 1024;

        public const string KeyHandshake = "s5.handshake";
        public const string KeyAuth = "s5.auth";
        public const string KeyCommand = "s5.command";
        public const string KeyForward = "s5.forward";
        public const string KeyControl = "wpc.control";

        private readonly ProxySession m_Session;
        private byte[] m_Buffer = Array.Empty<byte>();
        private Operate.ProxyConfig.Proxy.ProxyStep m_Step;

        public int LeftBufferSize { get { return m_Buffer.Length; } }

        public IReceiveFilter<BinaryRequestInfo> NextReceiveFilter { get { return null; } }

        public FilterState State { get; private set; }

        public Socks5ProxyReceiveFilter(ProxySession session)
        {
            this.m_Session = session;
            this.m_Session.ProxyType = Operate.ProxyConfig.Proxy.ProxyType.Socket5;
            this.m_Session.ProxyStep = Operate.ProxyConfig.Proxy.ProxyStep.Handshake;
            this.m_Step = Operate.ProxyConfig.Proxy.ProxyStep.Handshake;
        }

        public void Reset()
        {
            this.State = FilterState.Normal;
            this.m_Buffer = Array.Empty<byte>();
        }

        public BinaryRequestInfo Filter(byte[] readBuffer, int offset, int length, bool toBeCopied, out int rest)
        {
            rest = 0;

            try
            {
                if (readBuffer == null || length <= 0) { return null; }

                //转发阶段没有帧：这一段就是一帧，整段交出去（必须拷贝，readBuffer 返回后会被下一次接收复用）
                if (this.m_Step == Operate.ProxyConfig.Proxy.ProxyStep.ForwardData)
                {
                    return new BinaryRequestInfo(KeyForward, Copy(readBuffer, offset, length));
                }

                //握手 / 认证 / 命令：残片 + 新数据一起判帧长
                byte[] combined = null;
                ReadOnlySpan<byte> data;
                if (this.m_Buffer.Length == 0)
                {
                    data = new ReadOnlySpan<byte>(readBuffer, offset, length);
                }
                else
                {
                    combined = Operate.ProxyConfig.Proxy.CombineData(this.m_Buffer, readBuffer, offset, length);
                    data = combined;
                }

                int need = Operate.ProxyConfig.Proxy.Socks5FrameLength(this.m_Step, data, out bool bad);
                if (bad)
                {
                    this.Fail("报文格式不对");
                    return null;
                }

                if (need < 0 || data.Length < need)
                {
                    int cap = this.m_Step == Operate.ProxyConfig.Proxy.ProxyStep.WpcControl ? Operate.WPCConfig.Device.MaxFrameBytes : MaxHeaderBytes;
                    if (data.Length > cap)
                    {
                        this.Fail("握手数据超长仍切不出一帧");
                        return null;
                    }

                    this.m_Buffer = combined ?? Copy(readBuffer, offset, length);
                    return null;
                }

                /*
                    切出一帧。rest 指的是 readBuffer 这一段里没用到的尾巴：
                    残片自己凑不成帧（否则上一轮就切出来了），所以这一帧一定吃完了整个残片、再吃掉新数据的前几个字节，
                    剩下的 data.Length - need 个字节正是 readBuffer 段的末尾，SuperSocket 会拿它们再调一次 Filter。
                */
                byte[] frame = data.Slice(0, need).ToArray();
                rest = data.Length - need;
                this.m_Buffer = Array.Empty<byte>();

                string key = KeyFor(this.m_Step);
                this.m_Step = NextStep(this.m_Step, frame);
                return new BinaryRequestInfo(key, frame);
            }
            catch (Exception ex)
            {
                Operate.DoLog(nameof(Filter), ex);
                this.Fail(ex.Message);
                return null;
            }
        }

        private void Fail(string Why)
        {
            this.State = FilterState.Error;
            this.m_Buffer = Array.Empty<byte>();
            Operate.DoLog(nameof(Socks5ProxyReceiveFilter), string.Format("{0}，关闭连接 [ {1}:{2} ]", Why, this.m_Session.ClientIP, this.m_Session.ClientPort));
        }

        private static byte[] Copy(byte[] Src, int Offset, int Length)
        {
            byte[] b = new byte[Length];
            Buffer.BlockCopy(Src, Offset, b, 0, Length);
            return b;
        }

        private static string KeyFor(Operate.ProxyConfig.Proxy.ProxyStep Step)
        {
            switch (Step)
            {
                case Operate.ProxyConfig.Proxy.ProxyStep.Handshake: return KeyHandshake;
                case Operate.ProxyConfig.Proxy.ProxyStep.AuthUserName: return KeyAuth;
                case Operate.ProxyConfig.Proxy.ProxyStep.Command: return KeyCommand;
                case Operate.ProxyConfig.Proxy.ProxyStep.WpcControl: return KeyControl;
                default: return KeyForward;
            }
        }

        /// <summary>切出一帧后下一帧按哪一步切。与 Operate.ProxyConfig.Proxy.Handshake 的应答同一份规则（SelectAuthMethod）。</summary>
        private static Operate.ProxyConfig.Proxy.ProxyStep NextStep(Operate.ProxyConfig.Proxy.ProxyStep Step, byte[] Frame)
        {
            switch (Step)
            {
                case Operate.ProxyConfig.Proxy.ProxyStep.Handshake:
                    {
                        byte method = Operate.ProxyConfig.Proxy.SelectAuthMethod(Frame);
                        //0xFF：握手处理会回 05 FF 并关会话，之后不会再有帧；填 Command 只是给个确定值
                        if (method == (byte)Operate.ProxyConfig.Proxy.AuthType.Wpc) { return Operate.ProxyConfig.Proxy.ProxyStep.WpcControl; }
                        return method == (byte)Operate.ProxyConfig.Proxy.AuthType.UserName
                            ? Operate.ProxyConfig.Proxy.ProxyStep.AuthUserName
                            : Operate.ProxyConfig.Proxy.ProxyStep.Command;
                    }

                case Operate.ProxyConfig.Proxy.ProxyStep.AuthUserName:
                    return Operate.ProxyConfig.Proxy.ProxyStep.Command;

                case Operate.ProxyConfig.Proxy.ProxyStep.WpcControl:
                    //控制连接一辈子都是控制帧
                    return Operate.ProxyConfig.Proxy.ProxyStep.WpcControl;

                default:
                    return Operate.ProxyConfig.Proxy.ProxyStep.ForwardData;
            }
        }
    }
}
