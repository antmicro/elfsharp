//
// Copyright (c) 2023 Antmicro
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
//
using System;
using System.Text;

namespace DWARF
{
    public struct uLeb128 : IEquatable<uLeb128>
    {
        public static implicit operator byte(uLeb128 val) => (byte)val.Low;
        public static implicit operator ushort(uLeb128 val) => (ushort)val.Low;
        public static implicit operator uint(uLeb128 val) => (uint)val.Low;
        public static implicit operator ulong(uLeb128 val) => val.Low;

        public static implicit operator Leb128(uLeb128 val)
        {
            Leb128 leb;
            leb.Low = (long)val.Low;
            leb.High = (long)val.High;
            return leb;
        }

        public uLeb128(ulong val)
        {
            this.Low = val;
            this.High = 0;
        }

        public uLeb128(uint val)
        {
            this.Low = (ulong)val;
            this.High = 0;
        }

        public bool Equals(uLeb128 obj)
        {
            return this.Low == obj.Low && this.High == obj.High;
        }

        public override string ToString()
        {
            return $"uleb: High=0x{High:X}, Low=0x{Low:X}";
        }

        public ulong Low;
        public ulong High;
    }

    // REV: extract, should be immutable?
    public struct Leb128
    {
        public static implicit operator sbyte(Leb128 val) => (sbyte)val.Low;
        public static implicit operator short(Leb128 val) => (short)val.Low;
        public static implicit operator int(Leb128 val) => (int)val.Low;
        public static implicit operator long(Leb128 val) => val.Low;

        public static implicit operator uLeb128(Leb128 val)
        {
            uLeb128 leb;
            leb.Low = (ulong)val.Low;
            leb.High = (ulong)val.High;
            return leb;
        }

        public Leb128(long val)
        {
            this.Low = val;
            this.High = 0;
        }

        public Leb128(int val)
        {
            this.Low = (long)val;
            this.High = 0;
        }

        public override string ToString()
        {
            return $"leb: High=0x{High:X}, Low=0x{Low:X}";
        }

        public long Low;
        public long High;
    }
}
