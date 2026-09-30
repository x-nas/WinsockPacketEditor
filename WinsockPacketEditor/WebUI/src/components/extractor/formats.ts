/*
  取值器变量的「写入字节格式」候选表。

  滤镜的取值器替换格与变量引用插入器用的是同一份口径 ——
  两处各写一份，迟早出现「插入器给的格式在替换格里被判非法」。
*/
export interface FormatOption { label: string; value: string }

/** 按变量数据类型给出可选的写入格式；第一项是默认值。 */
export function formatOptions(dataType: number): FormatOption[] {
  if (dataType === 0) {
    return [
      { label: 'u8', value: 'u8' },
      { label: 'u16 LE', value: 'u16le' }, { label: 'u16 BE', value: 'u16be' },
      { label: 'u32 LE', value: 'u32le' }, { label: 'u32 BE', value: 'u32be' },
      { label: 'u64 LE', value: 'u64le' }, { label: 'u64 BE', value: 'u64be' },
    ]
  }
  if (dataType === 1) {
    return [
      { label: 'f32 LE', value: 'f32le' }, { label: 'f32 BE', value: 'f32be' },
      { label: 'f64 LE', value: 'f64le' }, { label: 'f64 BE', value: 'f64be' },
    ]
  }
  if (dataType === 3) {
    return [{ label: 'UTF-8', value: 'utf8' }, { label: 'GB18030', value: 'gb18030' }, { label: 'Big5', value: 'big5' }]
  }
  return [{ label: '原始字节', value: '' }]
}
