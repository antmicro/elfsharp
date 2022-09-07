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
    public class AttributeParser
    {
        public static ulong SizeOf(AttributeType type, byte[] section, ulong cursor, bool DWARF64, byte addressSize)
        {
            ulong head = cursor;
            ulong length;
            ulong size;
            switch(type)
            {
                case AttributeType.DW_FORM_addr:
                    return addressSize;
                case AttributeType.DW_FORM_ref_addr:
                case AttributeType.DW_FORM_sec_offset:
                case AttributeType.DW_FORM_strp_sup:
                case AttributeType.DW_FORM_line_strp:
                case AttributeType.DW_FORM_strp:
                    return (ulong)(DWARF64 ? 8 : 4);

                case AttributeType.DW_FORM_block1:
                    length = Parser.ReadUByte(section, ref cursor);
                    return 1 + length;

                case AttributeType.DW_FORM_block2:
                    length = Parser.ReadUHalf(section, ref cursor);
                    return 2 + length;

                case AttributeType.DW_FORM_block4:
                    length = Parser.ReadUByte(section, ref cursor);
                    return 1 + length;

                case AttributeType.DW_FORM_block:
                case AttributeType.DW_FORM_exprloc:
                    length = Parser.ReadULeb128(section, ref cursor);
                    size = cursor - head;
                    return size + length;

                case AttributeType.DW_FORM_flag_present:
                case AttributeType.DW_FORM_implicit_const:
                    return 0;

                case AttributeType.DW_FORM_data1:
                case AttributeType.DW_FORM_flag:
                case AttributeType.DW_FORM_strx1:
                case AttributeType.DW_FORM_addrx1:
                    return 1;

                case AttributeType.DW_FORM_data2:
                case AttributeType.DW_FORM_ref2:
                case AttributeType.DW_FORM_strx2:
                case AttributeType.DW_FORM_addrx2:
                    return 2;

                case AttributeType.DW_FORM_strx3:
                case AttributeType.DW_FORM_addrx3:
                    return 3;

                case AttributeType.DW_FORM_data4:
                case AttributeType.DW_FORM_ref4:
                case AttributeType.DW_FORM_ref_sup4:
                case AttributeType.DW_FORM_strx4:
                case AttributeType.DW_FORM_addrx4:
                    return 4;

                case AttributeType.DW_FORM_data8:
                case AttributeType.DW_FORM_ref8:
                case AttributeType.DW_FORM_ref_sig8:
                case AttributeType.DW_FORM_ref_sup8:
                    return 8;

                case AttributeType.DW_FORM_data16:
                    return 16;

                case AttributeType.DW_FORM_strx:
                case AttributeType.DW_FORM_addrx:
                case AttributeType.DW_FORM_loclistx:
                case AttributeType.DW_FORM_rnglistx:
                case AttributeType.DW_FORM_udata:
                case AttributeType.DW_FORM_ref_udata:
                    // REV: is this correct?
                    Parser.ReadULeb128(section, ref cursor);
                    size = cursor - head;
                    return size;

                case AttributeType.DW_FORM_sdata:
                    // REV: is this correct?
                    Parser.ReadSLeb128(section, ref cursor);
                    size = cursor - head;
                    return size;

                // REV: ?
                case AttributeType.DW_FORM_indirect:
                    var dynamicType = Parser.ReadULeb128(section, ref cursor);
                    size = cursor - head + SizeOf((AttributeType)(ulong)dynamicType, section, cursor, DWARF64, addressSize);
                    return size;

                case AttributeType.DW_FORM_string:
                    // REV: read string?!
                    while(Parser.ReadSByte(section, ref cursor) != 0) {}
                    length = cursor - head;
                    return length;

                default:
                    throw new NotImplementedException($"Unknown form {type}, not implemented");
            }
        }
    }
}
