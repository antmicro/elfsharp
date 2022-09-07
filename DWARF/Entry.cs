//
// Copyright (c) 2023 Antmicro
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
//
using System;
using System.Collections.Generic;

namespace DWARF
{
    public struct Entry
    {
        public string Path { get; set; }
        public uLeb128 DirectoryIndex { get; set; }
        public uLeb128 Timestamp { get; set; }
        public uLeb128 Size { get; set; }
        public uLeb128 Md5 { get; set; }

        public static Entry Parse(byte[] section, ref ulong cursor, uLeb128[] format, bool dwarf64, Strings strings, Strings lineStrings)
        {
            var entry = new Entry();
            for(int i = 0; i < format.Length; i += 2)
            {
                var type = (StandardContentDescription)(ulong)format[i];
                var form = (AttributeType)(ulong)format[i + 1];

                ParseFragment(ref entry, type, form, section, ref cursor, format, dwarf64, strings, lineStrings);
            }

            return entry;
        }

        public static void ParseFragment(ref Entry entry, StandardContentDescription type, AttributeType form, byte[] section, ref ulong cursor, uLeb128[] format, bool dwarf64, Strings strings, Strings lineStrings)
        {
            switch(type)
            {
                case StandardContentDescription.DW_LNCT_path:
                {
                    switch(form)
                    {
                        case AttributeType.DW_FORM_string:
                            entry.Path = Parser.ReadString(section, ref cursor);
                            break;

                        case AttributeType.DW_FORM_strp:
                        {
                            var address = Parser.ReadAddress(section, ref cursor, dwarf64);
                            entry.Path = strings.Get(address);
                            break;
                        }

                        case AttributeType.DW_FORM_line_strp:
                        {
                            var address = Parser.ReadAddress(section, ref cursor, dwarf64);
                            entry.Path = lineStrings.Get(address);
                            break;
                        }

                        case AttributeType.DW_FORM_strp_sup:
                            throw new DwarfDecodingException("DWARF split across multiple files is not yet supported");

                        default:
                            throw new DwarfDecodingException($"Unsupported form: {form}");
                    }
                    break;
                }

                case StandardContentDescription.DW_LNCT_directory_index:
                {
                    switch(form)
                    {
                        case AttributeType.DW_FORM_data1:
                            entry.DirectoryIndex = new uLeb128(Parser.ReadUByte(section, ref cursor));
                            break;

                        case AttributeType.DW_FORM_data2:
                            entry.DirectoryIndex = new uLeb128(Parser.ReadUHalf(section, ref cursor));
                            break;

                        case AttributeType.DW_FORM_udata:
                            entry.DirectoryIndex = Parser.ReadULeb128(section, ref cursor);
                            break;

                        default:
                            throw new DwarfDecodingException($"Unsupported form: {form}");
                    }
                    break;
                }

                case StandardContentDescription.DW_LNCT_timestamp:
                {
                    switch(form)
                    {
                        case AttributeType.DW_FORM_udata:
                            entry.Timestamp = Parser.ReadULeb128(section, ref cursor);
                            break;

                        case AttributeType.DW_FORM_data4:
                            entry.DirectoryIndex = new uLeb128(Parser.ReadUWord(section, ref cursor));
                            break;

                        case AttributeType.DW_FORM_data8:
                            entry.DirectoryIndex = new uLeb128(Parser.ReadUDouble(section, ref cursor));
                            break;

                        default:
                            throw new DwarfDecodingException($"Unsupported form: {form}");
                    }
                    break;
                }

                case StandardContentDescription.DW_LNCT_size:
                {
                    switch(form)
                    {
                        case AttributeType.DW_FORM_data1:
                            entry.DirectoryIndex = new uLeb128(Parser.ReadUByte(section, ref cursor));
                            break;

                        case AttributeType.DW_FORM_data2:
                            entry.DirectoryIndex = new uLeb128(Parser.ReadUHalf(section, ref cursor));
                            break;

                        case AttributeType.DW_FORM_data4:
                            entry.DirectoryIndex = new uLeb128(Parser.ReadUWord(section, ref cursor));
                            break;

                        case AttributeType.DW_FORM_data8:
                            entry.DirectoryIndex = new uLeb128(Parser.ReadUDouble(section, ref cursor));
                            break;

                        case AttributeType.DW_FORM_udata:
                            entry.DirectoryIndex = Parser.ReadULeb128(section, ref cursor);
                            break;

                        default:
                            throw new DwarfDecodingException($"Unsupported form: {form}");
                    }
                    break;
                }

                default:
                    throw new DwarfDecodingException($"Unsupported type: {type}");
            }
        }
    }
}
