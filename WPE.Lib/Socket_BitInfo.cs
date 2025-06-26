﻿using System;
using System.Collections;

namespace WPE.Lib
{    
    public class Socket_BitInfo
    {
        private byte _value;

        public byte Value
        {
            get { return _value; }
            set { _value = value; }
        }

        public long Position { get; set; }

        public override string ToString()
        {
            var result = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}"
                , GetBitAsString(7)
                , GetBitAsString(6)
                , GetBitAsString(5)
                , GetBitAsString(4)
                , GetBitAsString(3)
                , GetBitAsString(2)
                , GetBitAsString(1)
                , GetBitAsString(0)
                );
            return result;
        }

        public string GetBitAsString(int index)
        {
            if (this[index])
                return "1";
            else
                return "0";
        }

        public bool this[int index]
        {
            get
            {
                return (_value & (1 << index)) != 0;
            }
            set
            {
                if (value)
                    _value |= (byte)(1 << index);
                else
                    _value &= (byte)(~(1 << index));
            }
        }

        byte ConvertToByte(BitArray bits)
        {
            if (bits.Count != 8)
            {
                throw new ArgumentException("bits");
            }
            byte[] bytes = new byte[1];
            bits.CopyTo(bytes, 0);
            return bytes[0];
        }

        public Socket_BitInfo(byte value, long position)
        {
            this._value = value;
            this.Position = position;
        }
    }
}
