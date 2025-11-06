using SunnyNetlibray;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SunnyNetlibray.Internal
{
    internal class api
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

        private static string LibraryName = Environment.Is64BitProcess ? "x64\\SunnyNet64.dll" : "x86\\SunnyNet.dll";       
        static IntPtr Handle = LoadLibrary(LibraryName);

        private static IntPtr GetFunction(string func)
        {
            if (Handle == IntPtr.Zero)
            {
                Debug.WriteLine("载入 SunnyNet Library[" + LibraryName + "] 失败");
                throw new Exception("载入 SunnyNet Library 失败");
            }
            IntPtr functionPointer = GetProcAddress(Handle, func);
            if (functionPointer == IntPtr.Zero)
            {
                throw new Exception("无法从 SunnyNet Library 找到方法函数 " + func + "");
            }
            return functionPointer;
        }


        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_CreateSunnyNet();
        public static api_CreateSunnyNet Sunny_CreateSunnyNet = Marshal.GetDelegateForFunctionPointer<api_CreateSunnyNet>(GetFunction("CreateSunnyNet"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_ReleaseSunnyNet(IntPtr SunnyContext);
        public static api_ReleaseSunnyNet Sunny_ReleaseSunnyNet = Marshal.GetDelegateForFunctionPointer<api_ReleaseSunnyNet>(GetFunction("ReleaseSunnyNet"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SunnyNetStart(IntPtr SunnyContext);
        public static api_SunnyNetStart Sunny_SunnyNetStart = Marshal.GetDelegateForFunctionPointer<api_SunnyNetStart>(GetFunction("SunnyNetStart"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SunnyNetSetPort(IntPtr SunnyContext, IntPtr Port);
        public static api_SunnyNetSetPort Sunny_SunnyNetSetPort = Marshal.GetDelegateForFunctionPointer<api_SunnyNetSetPort>(GetFunction("SunnyNetSetPort"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SunnyNetClose(IntPtr SunnyContext);
        public static api_SunnyNetClose Sunny_SunnyNetClose = Marshal.GetDelegateForFunctionPointer<api_SunnyNetClose>(GetFunction("SunnyNetClose"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SunnyNetSetCert(IntPtr SunnyContext, IntPtr CertificateManagerId);
        public static api_SunnyNetSetCert Sunny_SunnyNetSetCert = Marshal.GetDelegateForFunctionPointer<api_SunnyNetSetCert>(GetFunction("SunnyNetSetCert"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_SunnyNetInstallCert(IntPtr SunnyContext);
        public static api_SunnyNetInstallCert Sunny_SunnyNetInstallCert = Marshal.GetDelegateForFunctionPointer<api_SunnyNetInstallCert>(GetFunction("SunnyNetInstallCert"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SunnyNetSetCallback(IntPtr SunnyContext, IntPtr httpCallback, IntPtr tcpCallback, IntPtr wsCallback, IntPtr udpCallback);
        public static api_SunnyNetSetCallback Sunny_SunnyNetSetCallback = Marshal.GetDelegateForFunctionPointer<api_SunnyNetSetCallback>(GetFunction("SunnyNetSetCallback"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SunnyNetSocket5AddUser(IntPtr SunnyContext, IntPtr User, IntPtr Pass);
        public static api_SunnyNetSocket5AddUser Sunny_SunnyNetSocket5AddUser = Marshal.GetDelegateForFunctionPointer<api_SunnyNetSocket5AddUser>(GetFunction("SunnyNetSocket5AddUser"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SunnyNetVerifyUser(IntPtr SunnyContext, bool open);
        public static api_SunnyNetVerifyUser Sunny_SunnyNetVerifyUser = Marshal.GetDelegateForFunctionPointer<api_SunnyNetVerifyUser>(GetFunction("SunnyNetVerifyUser"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SunnyNetSocket5DelUser(IntPtr SunnyContext, IntPtr User);
        public static api_SunnyNetSocket5DelUser Sunny_SunnyNetSocket5DelUser = Marshal.GetDelegateForFunctionPointer<api_SunnyNetSocket5DelUser>(GetFunction("SunnyNetSocket5DelUser"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_SunnyNetMustTcp(IntPtr SunnyContext, bool open);
        public static api_SunnyNetMustTcp Sunny_SunnyNetMustTcp = Marshal.GetDelegateForFunctionPointer<api_SunnyNetMustTcp>(GetFunction("SunnyNetMustTcp"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_HTTPSetCertManager(IntPtr Context, IntPtr CertManagerContext);
        public static api_HTTPSetCertManager Sunny_HTTPSetCertManager = Marshal.GetDelegateForFunctionPointer<api_HTTPSetCertManager>(GetFunction("HTTPSetCertManager"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_CompileProxyRegexp(IntPtr SunnyContext, IntPtr Regexp);
        public static api_CompileProxyRegexp Sunny_CompileProxyRegexp = Marshal.GetDelegateForFunctionPointer<api_CompileProxyRegexp>(GetFunction("CompileProxyRegexp"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SetMustTcpRegexp(IntPtr SunnyContext, IntPtr Regexp, bool RulesAllow);
        public static api_SetMustTcpRegexp Sunny_SetMustTcpRegexp = Marshal.GetDelegateForFunctionPointer<api_SetMustTcpRegexp>(GetFunction("SetMustTcpRegexp"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_SunnyNetError(IntPtr SunnyContext);
        public static api_SunnyNetError Sunny_SunnyNetError = Marshal.GetDelegateForFunctionPointer<api_SunnyNetError>(GetFunction("SunnyNetError"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_SetRequestOutTime(IntPtr Context, IntPtr times);
        public static api_SetRequestOutTime Sunny_SetRequestOutTime = Marshal.GetDelegateForFunctionPointer<api_SetRequestOutTime>(GetFunction("SetRequestOutTime"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SetGlobalProxy(IntPtr SunnyContext, IntPtr ProxyAddress, IntPtr outTime);
        public static api_SetGlobalProxy Sunny_SetGlobalProxy = Marshal.GetDelegateForFunctionPointer<api_SetGlobalProxy>(GetFunction("SetGlobalProxy"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_ExportCert(IntPtr SunnyContext);
        public static api_ExportCert Sunny_ExportCert = Marshal.GetDelegateForFunctionPointer<api_ExportCert>(GetFunction("ExportCert"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SetIeProxy(IntPtr SunnyContext);
        public static api_SetIeProxy Sunny_SetIeProxy = Marshal.GetDelegateForFunctionPointer<api_SetIeProxy>(GetFunction("SetIeProxy"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_CancelIEProxy(IntPtr SunnyContext);
        public static api_CancelIEProxy Sunny_CancelIEProxy = Marshal.GetDelegateForFunctionPointer<api_CancelIEProxy>(GetFunction("CancelIEProxy"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_SetRequestCookie(IntPtr MessageId, IntPtr name, IntPtr val);
        public static api_SetRequestCookie Sunny_SetRequestCookie = Marshal.GetDelegateForFunctionPointer<api_SetRequestCookie>(GetFunction("SetRequestCookie"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_SetRequestAllCookie(IntPtr MessageId, IntPtr val);
        public static api_SetRequestAllCookie Sunny_SetRequestAllCookie = Marshal.GetDelegateForFunctionPointer<api_SetRequestAllCookie>(GetFunction("SetRequestAllCookie"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_GetRequestCookie(IntPtr MessageId, IntPtr name);
        public static api_GetRequestCookie Sunny_GetRequestCookie = Marshal.GetDelegateForFunctionPointer<api_GetRequestCookie>(GetFunction("GetRequestCookie"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_GetRequestALLCookie(IntPtr MessageId);
        public static api_GetRequestALLCookie Sunny_GetRequestALLCookie = Marshal.GetDelegateForFunctionPointer<api_GetRequestALLCookie>(GetFunction("GetRequestALLCookie"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_DelResponseHeader(IntPtr MessageId, IntPtr name);
        public static api_DelResponseHeader Sunny_DelResponseHeader = Marshal.GetDelegateForFunctionPointer<api_DelResponseHeader>(GetFunction("DelResponseHeader"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_DelRequestHeader(IntPtr MessageId, IntPtr name);
        public static api_DelRequestHeader Sunny_DelRequestHeader = Marshal.GetDelegateForFunctionPointer<api_DelRequestHeader>(GetFunction("DelRequestHeader"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_RawRequestDataToFile(IntPtr MessageId, IntPtr name, IntPtr nameLen);
        public static api_RawRequestDataToFile Sunny_RawRequestDataToFile = Marshal.GetDelegateForFunctionPointer<api_RawRequestDataToFile>(GetFunction("RawRequestDataToFile"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_IsRequestRawBody(IntPtr MessageId);
        public static api_IsRequestRawBody Sunny_IsRequestRawBody = Marshal.GetDelegateForFunctionPointer<api_IsRequestRawBody>(GetFunction("IsRequestRawBody"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_SetRequestHeader(IntPtr MessageId, IntPtr name, IntPtr val);
        public static api_SetRequestHeader Sunny_SetRequestHeader = Marshal.GetDelegateForFunctionPointer<api_SetRequestHeader>(GetFunction("SetRequestHeader"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_SetResponseHeader(IntPtr MessageId, IntPtr name, IntPtr val);
        public static api_SetResponseHeader Sunny_SetResponseHeader = Marshal.GetDelegateForFunctionPointer<api_SetResponseHeader>(GetFunction("SetResponseHeader"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_GetRequestHeader(IntPtr MessageId, IntPtr name);
        public static api_GetRequestHeader Sunny_GetRequestHeader = Marshal.GetDelegateForFunctionPointer<api_GetRequestHeader>(GetFunction("GetRequestHeader"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_GetResponseHeader(IntPtr MessageId, IntPtr name);
        public static api_GetResponseHeader Sunny_GetResponseHeader = Marshal.GetDelegateForFunctionPointer<api_GetResponseHeader>(GetFunction("GetResponseHeader"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_SetResponseAllHeader(IntPtr MessageId, IntPtr value);
        public static api_SetResponseAllHeader Sunny_SetResponseAllHeader = Marshal.GetDelegateForFunctionPointer<api_SetResponseAllHeader>(GetFunction("SetResponseAllHeader"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_GetResponseProto(IntPtr MessageId);
        public static api_GetResponseProto Sunny_GetResponseProto = Marshal.GetDelegateForFunctionPointer<api_GetResponseProto>(GetFunction("GetResponseProto"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_GetResponseAllHeader(IntPtr MessageId);
        public static api_GetResponseAllHeader Sunny_GetResponseAllHeader = Marshal.GetDelegateForFunctionPointer<api_GetResponseAllHeader>(GetFunction("GetResponseAllHeader"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_GetRequestAllHeader(IntPtr MessageId);
        public static api_GetRequestAllHeader Sunny_GetRequestAllHeader = Marshal.GetDelegateForFunctionPointer<api_GetRequestAllHeader>(GetFunction("GetRequestAllHeader"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_GetRequestProto(IntPtr MessageId);
        public static api_GetRequestProto Sunny_GetRequestProto = Marshal.GetDelegateForFunctionPointer<api_GetRequestProto>(GetFunction("GetRequestProto"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SetRequestProxy(IntPtr MessageId, IntPtr ProxyUrl, IntPtr outTime);
        public static api_SetRequestProxy Sunny_SetRequestProxy = Marshal.GetDelegateForFunctionPointer<api_SetRequestProxy>(GetFunction("SetRequestProxy"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_RandomRequestCipherSuites(IntPtr MessageId);
        public static api_RandomRequestCipherSuites Sunny_RandomRequestCipherSuites = Marshal.GetDelegateForFunctionPointer<api_RandomRequestCipherSuites>(GetFunction("RandomRequestCipherSuites"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_SetRequestALLHeader(IntPtr MessageId, IntPtr value);
        public static api_SetRequestALLHeader Sunny_SetRequestALLHeader = Marshal.GetDelegateForFunctionPointer<api_SetRequestALLHeader>(GetFunction("SetRequestALLHeader"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SetRequestHTTP2Config(IntPtr MessageId, IntPtr config);
        public static api_SetRequestHTTP2Config Sunny_SetRequestHTTP2Config = Marshal.GetDelegateForFunctionPointer<api_SetRequestHTTP2Config>(GetFunction("SetRequestHTTP2Config"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_GetResponseStatusCode(IntPtr MessageId);
        public static api_GetResponseStatusCode Sunny_GetResponseStatusCode = Marshal.GetDelegateForFunctionPointer<api_GetResponseStatusCode>(GetFunction("GetResponseStatusCode"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_GetRequestClientIp(IntPtr MessageId);
        public static api_GetRequestClientIp Sunny_GetRequestClientIp = Marshal.GetDelegateForFunctionPointer<api_GetRequestClientIp>(GetFunction("GetRequestClientIp"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_GetResponseStatus(IntPtr MessageId);
        public static api_GetResponseStatus Sunny_GetResponseStatus = Marshal.GetDelegateForFunctionPointer<api_GetResponseStatus>(GetFunction("GetResponseStatus"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_GetResponseServerAddress(IntPtr MessageId);
        public static api_GetResponseServerAddress Sunny_GetResponseServerAddress = Marshal.GetDelegateForFunctionPointer<api_GetResponseServerAddress>(GetFunction("GetResponseServerAddress"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_SetResponseStatus(IntPtr MessageId, IntPtr code);
        public static api_SetResponseStatus Sunny_SetResponseStatus = Marshal.GetDelegateForFunctionPointer<api_SetResponseStatus>(GetFunction("SetResponseStatus"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SetRequestUrl(IntPtr MessageId, IntPtr URI);
        public static api_SetRequestUrl Sunny_SetRequestUrl = Marshal.GetDelegateForFunctionPointer<api_SetRequestUrl>(GetFunction("SetRequestUrl"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_GetRequestBodyLen(IntPtr MessageId);
        public static api_GetRequestBodyLen Sunny_GetRequestBodyLen = Marshal.GetDelegateForFunctionPointer<api_GetRequestBodyLen>(GetFunction("GetRequestBodyLen"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_GetResponseBodyLen(IntPtr MessageId);
        public static api_GetResponseBodyLen Sunny_GetResponseBodyLen = Marshal.GetDelegateForFunctionPointer<api_GetResponseBodyLen>(GetFunction("GetResponseBodyLen"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SetResponseData(IntPtr MessageId, IntPtr data, IntPtr dataLen);
        public static api_SetResponseData Sunny_SetResponseData = Marshal.GetDelegateForFunctionPointer<api_SetResponseData>(GetFunction("SetResponseData"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SetRequestData(IntPtr MessageId, IntPtr data, IntPtr dataLen);
        public static api_SetRequestData Sunny_SetRequestData = Marshal.GetDelegateForFunctionPointer<api_SetRequestData>(GetFunction("SetRequestData"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_GetRequestBody(IntPtr MessageId);
        public static api_GetRequestBody Sunny_GetRequestBody = Marshal.GetDelegateForFunctionPointer<api_GetRequestBody>(GetFunction("GetRequestBody"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_GetResponseBody(IntPtr MessageId);
        public static api_GetResponseBody Sunny_GetResponseBody = Marshal.GetDelegateForFunctionPointer<api_GetResponseBody>(GetFunction("GetResponseBody"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_GetWebsocketBodyLen(IntPtr MessageId);
        public static api_GetWebsocketBodyLen Sunny_GetWebsocketBodyLen = Marshal.GetDelegateForFunctionPointer<api_GetWebsocketBodyLen>(GetFunction("GetWebsocketBodyLen"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_GetWebsocketBody(IntPtr MessageId);
        public static api_GetWebsocketBody Sunny_GetWebsocketBody = Marshal.GetDelegateForFunctionPointer<api_GetWebsocketBody>(GetFunction("GetWebsocketBody"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SetWebsocketBody(IntPtr MessageId, IntPtr data, IntPtr dataLen);
        public static api_SetWebsocketBody Sunny_SetWebsocketBody = Marshal.GetDelegateForFunctionPointer<api_SetWebsocketBody>(GetFunction("SetWebsocketBody"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SendWebsocketBody(IntPtr TheologyID, IntPtr MessageType, IntPtr data, IntPtr dataLen);
        public static api_SendWebsocketBody Sunny_SendWebsocketBody = Marshal.GetDelegateForFunctionPointer<api_SendWebsocketBody>(GetFunction("SendWebsocketBody"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SendWebsocketClientBody(IntPtr TheologyID, IntPtr MessageType, IntPtr data, IntPtr dataLen);
        public static api_SendWebsocketClientBody Sunny_SendWebsocketClientBody = Marshal.GetDelegateForFunctionPointer<api_SendWebsocketClientBody>(GetFunction("SendWebsocketClientBody"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_CloseWebsocket(IntPtr theology);
        public static api_CloseWebsocket Sunny_CloseWebsocket = Marshal.GetDelegateForFunctionPointer<api_CloseWebsocket>(GetFunction("CloseWebsocket"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SetTcpBody(IntPtr MessageId, IntPtr MsgType, IntPtr data, IntPtr dataLen);
        public static api_SetTcpBody Sunny_SetTcpBody = Marshal.GetDelegateForFunctionPointer<api_SetTcpBody>(GetFunction("SetTcpBody"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SetTcpAgent(IntPtr MessageId, IntPtr ProxyUrl, IntPtr outTime);
        public static api_SetTcpAgent Sunny_SetTcpAgent = Marshal.GetDelegateForFunctionPointer<api_SetTcpAgent>(GetFunction("SetTcpAgent"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_TcpCloseClient(IntPtr theology);
        public static api_TcpCloseClient Sunny_TcpCloseClient = Marshal.GetDelegateForFunctionPointer<api_TcpCloseClient>(GetFunction("TcpCloseClient"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SetTcpConnectionIP(IntPtr MessageId, IntPtr address);
        public static api_SetTcpConnectionIP Sunny_SetTcpConnectionIP = Marshal.GetDelegateForFunctionPointer<api_SetTcpConnectionIP>(GetFunction("SetTcpConnectionIP"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_TcpSendMsg(IntPtr theology, IntPtr data, IntPtr dataLen);
        public static api_TcpSendMsg Sunny_TcpSendMsg = Marshal.GetDelegateForFunctionPointer<api_TcpSendMsg>(GetFunction("TcpSendMsg"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_TcpSendMsgClient(IntPtr theology, IntPtr data, IntPtr dataLen);
        public static api_TcpSendMsgClient Sunny_TcpSendMsgClient = Marshal.GetDelegateForFunctionPointer<api_TcpSendMsgClient>(GetFunction("TcpSendMsgClient"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_BytesToInt(IntPtr data, IntPtr dataLen);
        public static api_BytesToInt Sunny_BytesToInt = Marshal.GetDelegateForFunctionPointer<api_BytesToInt>(GetFunction("BytesToInt"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_GzipUnCompress(IntPtr data, IntPtr dataLen);
        public static api_GzipUnCompress Sunny_GzipUnCompress = Marshal.GetDelegateForFunctionPointer<api_GzipUnCompress>(GetFunction("GzipUnCompress"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_ZSTDDecompress(IntPtr data, IntPtr dataLen);
        public static api_ZSTDDecompress Sunny_ZSTDDecompress = Marshal.GetDelegateForFunctionPointer<api_ZSTDDecompress>(GetFunction("ZSTDDecompress"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_ZSTDCompress(IntPtr data, IntPtr dataLen);
        public static api_ZSTDCompress Sunny_ZSTDCompress = Marshal.GetDelegateForFunctionPointer<api_ZSTDCompress>(GetFunction("ZSTDCompress"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_BrUnCompress(IntPtr data, IntPtr dataLen);
        public static api_BrUnCompress Sunny_BrUnCompress = Marshal.GetDelegateForFunctionPointer<api_BrUnCompress>(GetFunction("BrUnCompress"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_BrCompress(IntPtr data, IntPtr dataLen);
        public static api_BrCompress Sunny_BrCompress = Marshal.GetDelegateForFunctionPointer<api_BrCompress>(GetFunction("BrCompress"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_BrotliCompress(IntPtr data, IntPtr dataLen);
        public static api_BrotliCompress Sunny_BrotliCompress = Marshal.GetDelegateForFunctionPointer<api_BrotliCompress>(GetFunction("BrotliCompress"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_GzipCompress(IntPtr data, IntPtr dataLen);
        public static api_GzipCompress Sunny_GzipCompress = Marshal.GetDelegateForFunctionPointer<api_GzipCompress>(GetFunction("GzipCompress"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_ZlibCompress(IntPtr data, IntPtr dataLen);
        public static api_ZlibCompress Sunny_ZlibCompress = Marshal.GetDelegateForFunctionPointer<api_ZlibCompress>(GetFunction("ZlibCompress"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_ZlibUnCompress(IntPtr data, IntPtr dataLen);
        public static api_ZlibUnCompress Sunny_ZlibUnCompress = Marshal.GetDelegateForFunctionPointer<api_ZlibUnCompress>(GetFunction("ZlibUnCompress"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_DeflateUnCompress(IntPtr data, IntPtr dataLen);
        public static api_DeflateUnCompress Sunny_DeflateUnCompress = Marshal.GetDelegateForFunctionPointer<api_DeflateUnCompress>(GetFunction("DeflateUnCompress"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_DeflateCompress(IntPtr data, IntPtr dataLen);
        public static api_DeflateCompress Sunny_DeflateCompress = Marshal.GetDelegateForFunctionPointer<api_DeflateCompress>(GetFunction("DeflateCompress"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_WebpToJpegBytes(IntPtr data, IntPtr dataLen, IntPtr SaveQuality);
        public static api_WebpToJpegBytes Sunny_WebpToJpegBytes = Marshal.GetDelegateForFunctionPointer<api_WebpToJpegBytes>(GetFunction("WebpToJpegBytes"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_WebpToPngBytes(IntPtr data, IntPtr dataLen);
        public static api_WebpToPngBytes Sunny_WebpToPngBytes = Marshal.GetDelegateForFunctionPointer<api_WebpToPngBytes>(GetFunction("WebpToPngBytes"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_WebpToJpeg(IntPtr webpPath, IntPtr savePath, IntPtr SaveQuality);
        public static api_WebpToJpeg Sunny_WebpToJpeg = Marshal.GetDelegateForFunctionPointer<api_WebpToJpeg>(GetFunction("WebpToJpeg"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_WebpToPng(IntPtr webpPath, IntPtr savePath);
        public static api_WebpToPng Sunny_WebpToPng = Marshal.GetDelegateForFunctionPointer<api_WebpToPng>(GetFunction("WebpToPng"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_OpenDrive(IntPtr SunnyContext, IntPtr devMode);
        public static api_OpenDrive Sunny_OpenDrive = Marshal.GetDelegateForFunctionPointer<api_OpenDrive>(GetFunction("OpenDrive"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_UnDrive(IntPtr SunnyContext);
        public static api_UnDrive Sunny_UnDrive = Marshal.GetDelegateForFunctionPointer<api_UnDrive>(GetFunction("UnDrive"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_ProcessAddName(IntPtr SunnyContext, IntPtr Name);
        public static api_ProcessAddName Sunny_ProcessAddName = Marshal.GetDelegateForFunctionPointer<api_ProcessAddName>(GetFunction("ProcessAddName"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_ProcessDelName(IntPtr SunnyContext, IntPtr Name);
        public static api_ProcessDelName Sunny_ProcessDelName = Marshal.GetDelegateForFunctionPointer<api_ProcessDelName>(GetFunction("ProcessDelName"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_ProcessAddPid(IntPtr SunnyContext, IntPtr pid);
        public static api_ProcessAddPid Sunny_ProcessAddPid = Marshal.GetDelegateForFunctionPointer<api_ProcessAddPid>(GetFunction("ProcessAddPid"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_ProcessDelPid(IntPtr SunnyContext, IntPtr pid);
        public static api_ProcessDelPid Sunny_ProcessDelPid = Marshal.GetDelegateForFunctionPointer<api_ProcessDelPid>(GetFunction("ProcessDelPid"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_ProcessCancelAll(IntPtr SunnyContext);
        public static api_ProcessCancelAll Sunny_ProcessCancelAll = Marshal.GetDelegateForFunctionPointer<api_ProcessCancelAll>(GetFunction("ProcessCancelAll"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_ProcessALLName(IntPtr SunnyContext, bool open, bool StopNet);
        public static api_ProcessALLName Sunny_ProcessALLName = Marshal.GetDelegateForFunctionPointer<api_ProcessALLName>(GetFunction("ProcessALLName"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_GetCommonName(IntPtr Context);
        public static api_GetCommonName Sunny_GetCommonName = Marshal.GetDelegateForFunctionPointer<api_GetCommonName>(GetFunction("GetCommonName"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_ExportP12(IntPtr Context, IntPtr path, IntPtr pass);
        public static api_ExportP12 Sunny_ExportP12 = Marshal.GetDelegateForFunctionPointer<api_ExportP12>(GetFunction("ExportP12"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_ExportPub(IntPtr Context);
        public static api_ExportPub Sunny_ExportPub = Marshal.GetDelegateForFunctionPointer<api_ExportPub>(GetFunction("ExportPub"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_ExportKEY(IntPtr Context);
        public static api_ExportKEY Sunny_ExportKEY = Marshal.GetDelegateForFunctionPointer<api_ExportKEY>(GetFunction("ExportKEY"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_ExportCA(IntPtr Context);
        public static api_ExportCA Sunny_ExportCA = Marshal.GetDelegateForFunctionPointer<api_ExportCA>(GetFunction("ExportCA"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_CreateCA(IntPtr Context, IntPtr Country, IntPtr Organization, IntPtr OrganizationalUnit, IntPtr Province, IntPtr CommonName, IntPtr Locality, IntPtr bits, IntPtr NotAfter);
        public static api_CreateCA Sunny_CreateCA = Marshal.GetDelegateForFunctionPointer<api_CreateCA>(GetFunction("CreateCA"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_AddClientAuth(IntPtr Context, IntPtr val);
        public static api_AddClientAuth Sunny_AddClientAuth = Marshal.GetDelegateForFunctionPointer<api_AddClientAuth>(GetFunction("AddClientAuth"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_AddCertPoolText(IntPtr Context, IntPtr cer);
        public static api_AddCertPoolText Sunny_AddCertPoolText = Marshal.GetDelegateForFunctionPointer<api_AddCertPoolText>(GetFunction("AddCertPoolText"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_AddCertPoolPath(IntPtr Context, IntPtr cer);
        public static api_AddCertPoolPath Sunny_AddCertPoolPath = Marshal.GetDelegateForFunctionPointer<api_AddCertPoolPath>(GetFunction("AddCertPoolPath"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_GetServerName(IntPtr Context);
        public static api_GetServerName Sunny_GetServerName = Marshal.GetDelegateForFunctionPointer<api_GetServerName>(GetFunction("GetServerName"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SetServerName(IntPtr Context, IntPtr name);
        public static api_SetServerName Sunny_SetServerName = Marshal.GetDelegateForFunctionPointer<api_SetServerName>(GetFunction("SetServerName"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SetInsecureSkipVerify(IntPtr Context, bool b);
        public static api_SetInsecureSkipVerify Sunny_SetInsecureSkipVerify = Marshal.GetDelegateForFunctionPointer<api_SetInsecureSkipVerify>(GetFunction("SetInsecureSkipVerify"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_LoadX509Certificate(IntPtr Context, IntPtr Host, IntPtr CA, IntPtr KEY);
        public static api_LoadX509Certificate Sunny_LoadX509Certificate = Marshal.GetDelegateForFunctionPointer<api_LoadX509Certificate>(GetFunction("LoadX509Certificate"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_LoadX509KeyPair(IntPtr Context, IntPtr CaPath, IntPtr KeyPath);
        public static api_LoadX509KeyPair Sunny_LoadX509KeyPair = Marshal.GetDelegateForFunctionPointer<api_LoadX509KeyPair>(GetFunction("LoadX509KeyPair"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_LoadP12Certificate(IntPtr Context, IntPtr Name, IntPtr Password);
        public static api_LoadP12Certificate Sunny_LoadP12Certificate = Marshal.GetDelegateForFunctionPointer<api_LoadP12Certificate>(GetFunction("LoadP12Certificate"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_RemoveCertificate(IntPtr Context);
        public static api_RemoveCertificate Sunny_RemoveCertificate = Marshal.GetDelegateForFunctionPointer<api_RemoveCertificate>(GetFunction("RemoveCertificate"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_CreateCertificate();
        public static api_CreateCertificate Sunny_CreateCertificate = Marshal.GetDelegateForFunctionPointer<api_CreateCertificate>(GetFunction("CreateCertificate"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_KeysWriteStr(IntPtr KeysHandle, IntPtr name, IntPtr val, IntPtr len);
        public static api_KeysWriteStr Sunny_KeysWriteStr = Marshal.GetDelegateForFunctionPointer<api_KeysWriteStr>(GetFunction("KeysWriteStr"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_KeysGetJson(IntPtr KeysHandle);
        public static api_KeysGetJson Sunny_KeysGetJson = Marshal.GetDelegateForFunctionPointer<api_KeysGetJson>(GetFunction("KeysGetJson"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_KeysGetCount(IntPtr KeysHandle);
        public static api_KeysGetCount Sunny_KeysGetCount = Marshal.GetDelegateForFunctionPointer<api_KeysGetCount>(GetFunction("KeysGetCount"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_KeysEmpty(IntPtr KeysHandle);
        public static api_KeysEmpty Sunny_KeysEmpty = Marshal.GetDelegateForFunctionPointer<api_KeysEmpty>(GetFunction("KeysEmpty"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_KeysReadInt(IntPtr KeysHandle, IntPtr name);
        public static api_KeysReadInt Sunny_KeysReadInt = Marshal.GetDelegateForFunctionPointer<api_KeysReadInt>(GetFunction("KeysReadInt"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_KeysWriteInt(IntPtr KeysHandle, IntPtr name, IntPtr val);
        public static api_KeysWriteInt Sunny_KeysWriteInt = Marshal.GetDelegateForFunctionPointer<api_KeysWriteInt>(GetFunction("KeysWriteInt"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate long api_KeysReadLong(IntPtr KeysHandle, IntPtr name);
        public static api_KeysReadLong Sunny_KeysReadLong = Marshal.GetDelegateForFunctionPointer<api_KeysReadLong>(GetFunction("KeysReadLong"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_KeysWriteLong(IntPtr KeysHandle, IntPtr name, Int64 val);
        public static api_KeysWriteLong Sunny_KeysWriteLong = Marshal.GetDelegateForFunctionPointer<api_KeysWriteLong>(GetFunction("KeysWriteLong"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Double api_KeysReadFloat(IntPtr KeysHandle, IntPtr name);
        public static api_KeysReadFloat Sunny_KeysReadFloat = Marshal.GetDelegateForFunctionPointer<api_KeysReadFloat>(GetFunction("KeysReadFloat"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_KeysWriteFloat(IntPtr KeysHandle, IntPtr name, Double val);
        public static api_KeysWriteFloat Sunny_KeysWriteFloat = Marshal.GetDelegateForFunctionPointer<api_KeysWriteFloat>(GetFunction("KeysWriteFloat"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_KeysWrite(IntPtr KeysHandle, IntPtr name, IntPtr val, IntPtr length);
        public static api_KeysWrite Sunny_KeysWrite = Marshal.GetDelegateForFunctionPointer<api_KeysWrite>(GetFunction("KeysWrite"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_KeysRead(IntPtr KeysHandle, IntPtr name);
        public static api_KeysRead Sunny_KeysRead = Marshal.GetDelegateForFunctionPointer<api_KeysRead>(GetFunction("KeysRead"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_KeysDelete(IntPtr KeysHandle, IntPtr name);
        public static api_KeysDelete Sunny_KeysDelete = Marshal.GetDelegateForFunctionPointer<api_KeysDelete>(GetFunction("KeysDelete"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_RemoveKeys(IntPtr KeysHandle);
        public static api_RemoveKeys Sunny_RemoveKeys = Marshal.GetDelegateForFunctionPointer<api_RemoveKeys>(GetFunction("RemoveKeys"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_CreateKeys();
        public static api_CreateKeys Sunny_CreateKeys = Marshal.GetDelegateForFunctionPointer<api_CreateKeys>(GetFunction("CreateKeys"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_HTTPSetRedirect(IntPtr Context, bool Redirect);
        public static api_HTTPSetRedirect Sunny_HTTPSetRedirect = Marshal.GetDelegateForFunctionPointer<api_HTTPSetRedirect>(GetFunction("HTTPSetRedirect"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_HTTPGetCode(IntPtr Context);
        public static api_HTTPGetCode Sunny_HTTPGetCode = Marshal.GetDelegateForFunctionPointer<api_HTTPGetCode>(GetFunction("HTTPGetCode"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_HTTPGetBody(IntPtr Context);
        public static api_HTTPGetBody Sunny_HTTPGetBody = Marshal.GetDelegateForFunctionPointer<api_HTTPGetBody>(GetFunction("HTTPGetBody"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_HTTPGetHeads(IntPtr Context);
        public static api_HTTPGetHeads Sunny_HTTPGetHeads = Marshal.GetDelegateForFunctionPointer<api_HTTPGetHeads>(GetFunction("HTTPGetHeads"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_HTTPGetBodyLen(IntPtr Context);
        public static api_HTTPGetBodyLen Sunny_HTTPGetBodyLen = Marshal.GetDelegateForFunctionPointer<api_HTTPGetBodyLen>(GetFunction("HTTPGetBodyLen"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_HTTPSendBin(IntPtr Context, IntPtr body, IntPtr bodyLength);
        public static api_HTTPSendBin Sunny_HTTPSendBin = Marshal.GetDelegateForFunctionPointer<api_HTTPSendBin>(GetFunction("HTTPSendBin"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_HTTPSetTimeouts(IntPtr Context, IntPtr t1);
        public static api_HTTPSetTimeouts Sunny_HTTPSetTimeouts = Marshal.GetDelegateForFunctionPointer<api_HTTPSetTimeouts>(GetFunction("HTTPSetTimeouts"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_HTTPSetProxyIP(IntPtr Context, IntPtr ProxyURL);
        public static api_HTTPSetProxyIP Sunny_HTTPSetProxyIP = Marshal.GetDelegateForFunctionPointer<api_HTTPSetProxyIP>(GetFunction("HTTPSetProxyIP"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_HTTPSetHeader(IntPtr Context, IntPtr name, IntPtr value);
        public static api_HTTPSetHeader Sunny_HTTPSetHeader = Marshal.GetDelegateForFunctionPointer<api_HTTPSetHeader>(GetFunction("HTTPSetHeader"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_HTTPSetServerIP(IntPtr Context, IntPtr ip);
        public static api_HTTPSetServerIP Sunny_HTTPSetServerIP = Marshal.GetDelegateForFunctionPointer<api_HTTPSetServerIP>(GetFunction("HTTPSetServerIP"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_HTTPOpen(IntPtr Context, IntPtr Method, IntPtr URL);
        public static api_HTTPOpen Sunny_HTTPOpen = Marshal.GetDelegateForFunctionPointer<api_HTTPOpen>(GetFunction("HTTPOpen"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_RemoveHTTPClient(IntPtr Context);
        public static api_RemoveHTTPClient Sunny_RemoveHTTPClient = Marshal.GetDelegateForFunctionPointer<api_RemoveHTTPClient>(GetFunction("RemoveHTTPClient"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_CreateHTTPClient();
        public static api_CreateHTTPClient Sunny_CreateHTTPClient = Marshal.GetDelegateForFunctionPointer<api_CreateHTTPClient>(GetFunction("CreateHTTPClient"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_JsonToPB(IntPtr bin, IntPtr binLen);
        public static api_JsonToPB Sunny_JsonToPB = Marshal.GetDelegateForFunctionPointer<api_JsonToPB>(GetFunction("JsonToPB"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_PbToJson(IntPtr bin, IntPtr binLen);
        public static api_PbToJson Sunny_PbToJson = Marshal.GetDelegateForFunctionPointer<api_PbToJson>(GetFunction("PbToJson"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_QueuePull(IntPtr name);
        public static api_QueuePull Sunny_QueuePull = Marshal.GetDelegateForFunctionPointer<api_QueuePull>(GetFunction("QueuePull"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_QueuePush(IntPtr name, IntPtr val, IntPtr valLen);
        public static api_QueuePush Sunny_QueuePush = Marshal.GetDelegateForFunctionPointer<api_QueuePush>(GetFunction("QueuePush"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_QueueLength(IntPtr name);
        public static api_QueueLength Sunny_QueueLength = Marshal.GetDelegateForFunctionPointer<api_QueueLength>(GetFunction("QueueLength"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_QueueRelease(IntPtr name);
        public static api_QueueRelease Sunny_QueueRelease = Marshal.GetDelegateForFunctionPointer<api_QueueRelease>(GetFunction("QueueRelease"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_QueueIsEmpty(IntPtr name);
        public static api_QueueIsEmpty Sunny_QueueIsEmpty = Marshal.GetDelegateForFunctionPointer<api_QueueIsEmpty>(GetFunction("QueueIsEmpty"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_CreateQueue(IntPtr name);
        public static api_CreateQueue Sunny_CreateQueue = Marshal.GetDelegateForFunctionPointer<api_CreateQueue>(GetFunction("CreateQueue"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_SocketClientWrite(IntPtr Context, IntPtr OutTimes, IntPtr val, IntPtr valLen);
        public static api_SocketClientWrite Sunny_SocketClientWrite = Marshal.GetDelegateForFunctionPointer<api_SocketClientWrite>(GetFunction("SocketClientWrite"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_SocketClientClose(IntPtr Context);
        public static api_SocketClientClose Sunny_SocketClientClose = Marshal.GetDelegateForFunctionPointer<api_SocketClientClose>(GetFunction("SocketClientClose"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_SocketClientReceive(IntPtr Context, IntPtr OutTimes);
        public static api_SocketClientReceive Sunny_SocketClientReceive = Marshal.GetDelegateForFunctionPointer<api_SocketClientReceive>(GetFunction("SocketClientReceive"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SocketClientDial(IntPtr Context, IntPtr addr, IntPtr call, bool isTls, bool synchronous, IntPtr ProxyUrl, IntPtr CertificateConText, IntPtr timeOut, IntPtr RouterIP);
        public static api_SocketClientDial Sunny_SocketClientDial = Marshal.GetDelegateForFunctionPointer<api_SocketClientDial>(GetFunction("SocketClientDial"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SocketClientSetBufferSize(IntPtr Context, IntPtr BufferSize);
        public static api_SocketClientSetBufferSize Sunny_SocketClientSetBufferSize = Marshal.GetDelegateForFunctionPointer<api_SocketClientSetBufferSize>(GetFunction("SocketClientSetBufferSize"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_SocketClientGetErr(IntPtr Context);
        public static api_SocketClientGetErr Sunny_SocketClientGetErr = Marshal.GetDelegateForFunctionPointer<api_SocketClientGetErr>(GetFunction("SocketClientGetErr"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_RemoveSocketClient(IntPtr Context);
        public static api_RemoveSocketClient Sunny_RemoveSocketClient = Marshal.GetDelegateForFunctionPointer<api_RemoveSocketClient>(GetFunction("RemoveSocketClient"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_CreateSocketClient();
        public static api_CreateSocketClient Sunny_CreateSocketClient = Marshal.GetDelegateForFunctionPointer<api_CreateSocketClient>(GetFunction("CreateSocketClient"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_WebsocketClientReceive(IntPtr Context, IntPtr OutTimes);
        public static api_WebsocketClientReceive Sunny_WebsocketClientReceive = Marshal.GetDelegateForFunctionPointer<api_WebsocketClientReceive>(GetFunction("WebsocketClientReceive"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_WebsocketReadWrite(IntPtr Context, IntPtr val, IntPtr valLen, IntPtr messageType);
        public static api_WebsocketReadWrite Sunny_WebsocketReadWrite = Marshal.GetDelegateForFunctionPointer<api_WebsocketReadWrite>(GetFunction("WebsocketReadWrite"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_WebsocketClose(IntPtr Context);
        public static api_WebsocketClose Sunny_WebsocketClose = Marshal.GetDelegateForFunctionPointer<api_WebsocketClose>(GetFunction("WebsocketClose"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_WebsocketDial(IntPtr Context, IntPtr URL, IntPtr Heads, IntPtr call, bool synchronous, IntPtr ProxyUrl, IntPtr CertificateConText, IntPtr outTime, IntPtr RouterIP);
        public static api_WebsocketDial Sunny_WebsocketDial = Marshal.GetDelegateForFunctionPointer<api_WebsocketDial>(GetFunction("WebsocketDial"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_WebsocketHeartbeat(IntPtr Context, IntPtr HeartbeatTime, IntPtr call);
        public static api_WebsocketHeartbeat Sunny_WebsocketHeartbeat = Marshal.GetDelegateForFunctionPointer<api_WebsocketHeartbeat>(GetFunction("WebsocketHeartbeat"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_WebsocketGetErr(IntPtr Context);
        public static api_WebsocketGetErr Sunny_WebsocketGetErr = Marshal.GetDelegateForFunctionPointer<api_WebsocketGetErr>(GetFunction("WebsocketGetErr"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_RemoveWebsocket(IntPtr Context);
        public static api_RemoveWebsocket Sunny_RemoveWebsocket = Marshal.GetDelegateForFunctionPointer<api_RemoveWebsocket>(GetFunction("RemoveWebsocket"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_CreateWebsocket();
        public static api_CreateWebsocket Sunny_CreateWebsocket = Marshal.GetDelegateForFunctionPointer<api_CreateWebsocket>(GetFunction("CreateWebsocket"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_AddHttpCertificate(IntPtr host, IntPtr CertManagerId, IntPtr Rules);
        public static api_AddHttpCertificate Sunny_AddHttpCertificate = Marshal.GetDelegateForFunctionPointer<api_AddHttpCertificate>(GetFunction("AddHttpCertificate"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_DelHttpCertificate(IntPtr host);
        public static api_DelHttpCertificate Sunny_DelHttpCertificate = Marshal.GetDelegateForFunctionPointer<api_DelHttpCertificate>(GetFunction("DelHttpCertificate"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_RedisSubscribe(IntPtr Context, IntPtr scribe, IntPtr call, bool nc);
        public static api_RedisSubscribe Sunny_RedisSubscribe = Marshal.GetDelegateForFunctionPointer<api_RedisSubscribe>(GetFunction("RedisSubscribe"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_RedisDelete(IntPtr Context, IntPtr key);
        public static api_RedisDelete Sunny_RedisDelete = Marshal.GetDelegateForFunctionPointer<api_RedisDelete>(GetFunction("RedisDelete"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_RedisFlushDB(IntPtr Context);
        public static api_RedisFlushDB Sunny_RedisFlushDB = Marshal.GetDelegateForFunctionPointer<api_RedisFlushDB>(GetFunction("RedisFlushDB"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_RedisFlushAll(IntPtr Context);
        public static api_RedisFlushAll Sunny_RedisFlushAll = Marshal.GetDelegateForFunctionPointer<api_RedisFlushAll>(GetFunction("RedisFlushAll"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_RedisClose(IntPtr Context);
        public static api_RedisClose Sunny_RedisClose = Marshal.GetDelegateForFunctionPointer<api_RedisClose>(GetFunction("RedisClose"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate Int64 api_RedisGetInt(IntPtr Context, IntPtr key);
        public static api_RedisGetInt Sunny_RedisGetInt = Marshal.GetDelegateForFunctionPointer<api_RedisGetInt>(GetFunction("RedisGetInt"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_RedisGetKeys(IntPtr Context, IntPtr key);
        public static api_RedisGetKeys Sunny_RedisGetKeys = Marshal.GetDelegateForFunctionPointer<api_RedisGetKeys>(GetFunction("RedisGetKeys"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_RedisDo(IntPtr Context, IntPtr args, IntPtr error);
        public static api_RedisDo Sunny_RedisDo = Marshal.GetDelegateForFunctionPointer<api_RedisDo>(GetFunction("RedisDo"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_RedisGetStr(IntPtr Context, IntPtr key);
        public static api_RedisGetStr Sunny_RedisGetStr = Marshal.GetDelegateForFunctionPointer<api_RedisGetStr>(GetFunction("RedisGetStr"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_RedisGetBytes(IntPtr Context, IntPtr key);
        public static api_RedisGetBytes Sunny_RedisGetBytes = Marshal.GetDelegateForFunctionPointer<api_RedisGetBytes>(GetFunction("RedisGetBytes"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_RedisExists(IntPtr Context, IntPtr key);
        public static api_RedisExists Sunny_RedisExists = Marshal.GetDelegateForFunctionPointer<api_RedisExists>(GetFunction("RedisExists"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_RedisSetNx(IntPtr Context, IntPtr key, IntPtr val, IntPtr expr);
        public static api_RedisSetNx Sunny_RedisSetNx = Marshal.GetDelegateForFunctionPointer<api_RedisSetNx>(GetFunction("RedisSetNx"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_RedisSet(IntPtr Context, IntPtr key, IntPtr val, IntPtr expr);
        public static api_RedisSet Sunny_RedisSet = Marshal.GetDelegateForFunctionPointer<api_RedisSet>(GetFunction("RedisSet"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_RedisSetBytes(IntPtr Context, IntPtr key, IntPtr val, IntPtr valLen, IntPtr expr);
        public static api_RedisSetBytes Sunny_RedisSetBytes = Marshal.GetDelegateForFunctionPointer<api_RedisSetBytes>(GetFunction("RedisSetBytes"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_RedisDial(IntPtr Context, IntPtr host, IntPtr pass, IntPtr db, IntPtr PoolSize, IntPtr MinIdleCons, IntPtr DialTimeout, IntPtr ReadTimeout, IntPtr WriteTimeout, IntPtr PoolTimeout, IntPtr IdleCheckFrequency, IntPtr IdleTimeout, IntPtr error);
        public static api_RedisDial Sunny_RedisDial = Marshal.GetDelegateForFunctionPointer<api_RedisDial>(GetFunction("RedisDial"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_RemoveRedis(IntPtr Context);
        public static api_RemoveRedis Sunny_RemoveRedis = Marshal.GetDelegateForFunctionPointer<api_RemoveRedis>(GetFunction("RemoveRedis"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_CreateRedis();
        public static api_CreateRedis Sunny_CreateRedis = Marshal.GetDelegateForFunctionPointer<api_CreateRedis>(GetFunction("CreateRedis"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_GetUdpData(IntPtr MessageId);
        public static api_GetUdpData Sunny_GetUdpData = Marshal.GetDelegateForFunctionPointer<api_GetUdpData>(GetFunction("GetUdpData"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SetUdpData(IntPtr MessageId, IntPtr val, IntPtr valLen);
        public static api_SetUdpData Sunny_SetUdpData = Marshal.GetDelegateForFunctionPointer<api_SetUdpData>(GetFunction("SetUdpData"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_UdpSendToClient(IntPtr MessageId, IntPtr val, IntPtr valLen);
        public static api_UdpSendToClient Sunny_UdpSendToClient = Marshal.GetDelegateForFunctionPointer<api_UdpSendToClient>(GetFunction("UdpSendToClient"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_UdpSendToServer(IntPtr MessageId, IntPtr val, IntPtr valLen);
        public static api_UdpSendToServer Sunny_UdpSendToServer = Marshal.GetDelegateForFunctionPointer<api_UdpSendToServer>(GetFunction("UdpSendToServer"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_Free(IntPtr Ptr);
        public static api_Free Sunny_Free = Marshal.GetDelegateForFunctionPointer<api_Free>(GetFunction("Free"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_GetSunnyVersion();
        public static api_GetSunnyVersion Sunny_GetSunnyVersion = Marshal.GetDelegateForFunctionPointer<api_GetSunnyVersion>(GetFunction("GetSunnyVersion"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_SunnyNetGetSocket5User(IntPtr WeiYiId);
        public static api_SunnyNetGetSocket5User Sunny_SunnyNetGetSocket5User = Marshal.GetDelegateForFunctionPointer<api_SunnyNetGetSocket5User>(GetFunction("SunnyNetGetSocket5User"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SetCipherSuites(IntPtr Context, IntPtr val);
        public static api_SetCipherSuites Sunny_SetCipherSuites = Marshal.GetDelegateForFunctionPointer<api_SetCipherSuites>(GetFunction("SetCipherSuites"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_HTTPSetRandomTLS(IntPtr Context, bool RandomTLS);
        public static api_HTTPSetRandomTLS Sunny_HTTPSetRandomTLS = Marshal.GetDelegateForFunctionPointer<api_HTTPSetRandomTLS>(GetFunction("HTTPSetRandomTLS"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_HTTPGetRequestHeader(IntPtr Context);
        public static api_HTTPGetRequestHeader Sunny_HTTPGetRequestHeader = Marshal.GetDelegateForFunctionPointer<api_HTTPGetRequestHeader>(GetFunction("HTTPGetRequestHeader"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_HTTPSetH2Config(IntPtr Context, IntPtr config);
        public static api_HTTPSetH2Config Sunny_HTTPSetH2Config = Marshal.GetDelegateForFunctionPointer<api_HTTPSetH2Config>(GetFunction("HTTPSetH2Config"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_SetDnsServer(IntPtr dns);
        public static api_SetDnsServer Sunny_SetDnsServer = Marshal.GetDelegateForFunctionPointer<api_SetDnsServer>(GetFunction("SetDnsServer"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SetRandomTLS(IntPtr Context, bool RandomTLS);
        public static api_SetRandomTLS Sunny_SetRandomTLS = Marshal.GetDelegateForFunctionPointer<api_SetRandomTLS>(GetFunction("SetRandomTLS"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_DisableUDP(IntPtr Context, bool Disable);
        public static api_DisableUDP Sunny_DisableUDP = Marshal.GetDelegateForFunctionPointer<api_DisableUDP>(GetFunction("DisableUDP"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_DisableTCP(IntPtr Context, bool Disable);
        public static api_DisableTCP Sunny_DisableTCP = Marshal.GetDelegateForFunctionPointer<api_DisableTCP>(GetFunction("DisableTCP"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_SetScriptPage(IntPtr Context, IntPtr config);
        public static api_SetScriptPage Sunny_SetScriptPage = Marshal.GetDelegateForFunctionPointer<api_SetScriptPage>(GetFunction("SetScriptPage"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate IntPtr api_SetScriptCode(IntPtr Context, IntPtr config);
        public static api_SetScriptCode Sunny_SetScriptCode = Marshal.GetDelegateForFunctionPointer<api_SetScriptCode>(GetFunction("SetScriptCode"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SetHTTPRequestMaxUpdateLength(IntPtr Context, long MaxLength);
        public static api_SetHTTPRequestMaxUpdateLength Sunny_SetHTTPRequestMaxUpdateLength = Marshal.GetDelegateForFunctionPointer<api_SetHTTPRequestMaxUpdateLength>(GetFunction("SetHTTPRequestMaxUpdateLength"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void api_SetScriptCall(IntPtr Context, IntPtr ScriptlogCallback, IntPtr ScriptsaveCallback);
        public static api_SetScriptCall Sunny_SetScriptCall = Marshal.GetDelegateForFunctionPointer<api_SetScriptCall>(GetFunction("SetScriptCall"));


        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_SetOutRouterIP(IntPtr Context, IntPtr ip);
        public static api_SetOutRouterIP Sunny_SetOutRouterIP = Marshal.GetDelegateForFunctionPointer<api_SetOutRouterIP>(GetFunction("SetOutRouterIP"));


        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_RequestSetOutRouterIP(IntPtr MessageId, IntPtr ip);
        public static api_RequestSetOutRouterIP Sunny_RequestSetOutRouterIP = Marshal.GetDelegateForFunctionPointer<api_RequestSetOutRouterIP>(GetFunction("RequestSetOutRouterIP"));

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate bool api_HTTPSetOutRouterIP(IntPtr Context, IntPtr ip);
        public static api_HTTPSetOutRouterIP Sunny_HTTPSetOutRouterIP = Marshal.GetDelegateForFunctionPointer<api_HTTPSetOutRouterIP>(GetFunction("HTTPSetOutRouterIP"));


    }
}
