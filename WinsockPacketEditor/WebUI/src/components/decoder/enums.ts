/* 解码器的枚举值。与 C# 的 DecoderKind / DecoderProtocol / … 一一对应，按 int 过桥。 */

export const DecKind = { Xor: 1, Aes: 2, Des: 3, Protobuf: 4, MessagePack: 5, Rc4: 6, Xxtea: 7, Amf: 8, TextCharset: 9, Bson: 10, FlatBuffers: 11 } as const
export const DecProtocol = { Any: 0, Tcp: 1, Udp: 2, Http: 3, WebSocket: 4 } as const
export const DecDirection = { Any: 0, Request: 1, Response: 2 } as const
export const DecKeyFormat = { Hex: 0, Base64: 1, Text: 2 } as const
export const DecCharset = { Default: 0, GBK: 1, UTF7: 2, UTF8: 3, UTF16BE: 4, UTF32: 5, UTF16LE: 6, Base64: 7 } as const

/** 对称加密的分组模式，下标即 C# DecoderCipherMode。 */
// CTS 在 .NET Framework provider 中没有一致可用的实现；不提供会“保存成功、运行失败”的选项。
export const CIPHER_MODES = ['CBC', 'ECB', 'OFB', 'CFB']

/** 填充，下标即 C# DecoderPadding。 */
export const PADDINGS = ['None', 'PKCS7', 'Zeros', 'ANSIX923', 'ISO10126']

/** 字符集显示名，下标即 C# DecoderCharset（Default 由界面用 i18n 覆盖）。 */
export const CHARSET_LABELS = ['Default', 'GBK', 'UTF-7', 'UTF-8', 'UTF-16 BE', 'UTF-32 LE', 'UTF-16 LE', 'Base64']

/** 是否对称加密（要用密钥 / IV / 模式 / 填充）。 */
export function isCipher(kind: number): boolean {
  return kind === DecKind.Aes || kind === DecKind.Des
}

/** 是否需要密钥。 */
export function needsKey(kind: number): boolean {
  return kind === DecKind.Xor || kind === DecKind.Rc4 || kind === DecKind.Xxtea || isCipher(kind)
}
