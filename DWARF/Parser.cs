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
    // REV: rename to Decoder?
    public static class Parser
    {
        public static string ReadString(byte[] bytes, ref ulong cursor)
        {
            var str = new StringBuilder();

            while(true)
            {
                var c = (char)ReadUByte(bytes, ref cursor);
                if(c == 0)
                {
                    return str.ToString();
                }
                str.Append(c);
            }
        }

        //REV: protect agains running out of array?
        public static sbyte ReadSByte(byte[] bytes, ref ulong cursor)
        {
            var val = bytes[cursor];
            cursor += 1;
            return(sbyte)val;
        }

        public static byte ReadUByte(byte[] bytes, ref ulong cursor)
        {
            var val = bytes[cursor];
            cursor += 1;
            return(byte)val;
        }

        public static short ReadSHalf(byte[] bytes, ref ulong cursor)
        {
            return (short)ReadUHalf(bytes, ref cursor);
        }

// here we assume little endian
        public static ushort ReadUHalf(byte[] bytes, ref ulong cursor)
        {
            var val = ((ushort)bytes[cursor + 1] << 8)
                    + (ushort)bytes[cursor];

            cursor += 2;
            return (ushort)val;
        }

        public static uint ReadUWord(byte[] bytes, ref ulong cursor)
        {
            var val = ((uint)bytes[cursor + 3] << 24)
                    + ((uint)bytes[cursor + 2] << 16)
                    + ((uint)bytes[cursor + 1] << 8)
                    + (uint)bytes[cursor];

            cursor += 4;
            return val;
        }

        public static int ReadSWord(byte[] bytes, ref ulong cursor)
        {
            return (int)ReadUWord(bytes, ref cursor);
        }

        public static ulong ReadUDouble(byte[] bytes, ref ulong cursor)
        {
            var val = ((ulong)bytes[cursor + 7] << 56)
                    + ((ulong)bytes[cursor + 6] << 48)
                    + ((ulong)bytes[cursor + 5] << 40)
                    + ((ulong)bytes[cursor + 4] << 32)
                    + ((ulong)bytes[cursor + 3] << 24)
                    + ((ulong)bytes[cursor + 2] << 16)
                    + ((ulong)bytes[cursor + 1] << 8)
                    + (ulong)bytes[cursor];

           cursor += 8;
           return val;
        }

        public static long ReadSDouble(byte [] bytes, ref ulong cursor)
        {
            return (long)ReadUDouble(bytes, ref cursor);
        }

        // REV: think about this
        public static ulong ReadAddress(byte[] bytes, ref ulong cursor, bool DWARF64)
        {
            return ReadAddress(bytes, ref cursor, (byte)(DWARF64 ? 8 : 4));
        }

        public static ulong ReadAddress(byte[] bytes, ref ulong cursor, byte addressSize)
        {
            if(addressSize == 8)
            {
                return ReadUDouble(bytes, ref cursor);
            }
            else if(addressSize == 4)
            {
                return ReadUWord(bytes, ref cursor);
            }
            else if(addressSize == 0)
            {
                return 0;
            }
            else
            {
                throw new NotSupportedException($"Tried to read address of unhandled size {addressSize}");
            }
        }

        public static uLeb128 ReadULeb128(byte[] bytes, ref ulong cursor)
        {
            uLeb128 leb;
            leb.Low = 0;
            leb.High = 0;

            // since each byte contains 7-bit of actual value there are max 19 bytes in LEB128
            var currentByte = 0;
            for(; currentByte < 19; currentByte++)
            // do
            {
                var val = (ulong)bytes[cursor + (ulong)currentByte];

                // most significant BIT is not set in the most significant BYTE
                var isMostSignificantByte = (val >> 7) == 0;
                // clear the most significant BIT - it's not a part of actual value
                val &= 0x7F;

                if(currentByte < 4)
                {
                    leb.Low |= (val << (currentByte * 7));
                }
                else if(currentByte == 5)
                {
                    leb.Low |= ((val & 0xF) << (currentByte * 7));
                    leb.High = (val >> 4);
                }
                else
                {
                    leb.High |= (val << ((currentByte * 7) + 4));
                }

                if(isMostSignificantByte)
                {
                    // TODO: check if currentByte is ok here
                    currentByte++;
                    break;
                }
            }

            // TODO check if currentByte is not out of range
            cursor += (ulong)currentByte;
            return leb;
        }

        // TODO: check if this could be simplified
        public static Leb128 ReadSLeb128(byte[] bytes, ref ulong cursor)
        {
            Leb128 leb;
            leb.Low = 0;
            leb.High = 0;

            // since each byte contains 7-bit of actual value there are max 19 bytes in LEB128
            var currentByte = 0;
            var val = 0L;
            for(; currentByte < 19; currentByte++)
            // do
            {
                val = (long)bytes[cursor + (ulong)currentByte];

                // most significant BIT is not set in the most significant BYTE
                var isMostSignificantByte = (val >> 7) == 0;
                // clear the most significant BIT - it's not a part of actual value
                val &= 0x7F;

                if(currentByte < 4)
                {
                    leb.Low |= (val << (currentByte * 7));
                }
                else if(currentByte == 5)
                {
                    leb.Low |= ((val & 0xF) << (currentByte * 7));
                    leb.High = (val >> 4);
                }
                else
                {
                    leb.High |= (val << ((currentByte * 7) + 4));
                }

                if(isMostSignificantByte)
                {
                    // TODO: check if currentByte is ok here
                    currentByte++;
                    break;
                }
            }

            // restore sign; let's assume LEBLow here for now only
            if((val & 0x40) != 0)
            {
                var mask = ulong.MaxValue << (currentByte * 7);
                leb.Low |= (long)mask;
            }

            // TODO check if currentByte is not out of range
            cursor += (ulong)currentByte;
            return leb;
        }

        public static ulong ReadInitialLength(byte[] bytes, ref ulong cursor, out bool DWARF64)
        {
            // 4-byte or 12-byte unsigned integer
            ulong length = ReadUWord(bytes, ref cursor);
            DWARF64 = (length == 0xffffffff);
            if(DWARF64)
            {
                // 64-bit DWARF is encoded as 0xffffffff followed by an 8-byte unsigned integer with the actual value
                length = ReadUDouble(bytes, ref cursor);
            }
            return length;
        }
    }
}
