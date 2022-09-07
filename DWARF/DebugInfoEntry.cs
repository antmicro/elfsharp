//
// Copyright (c) 2023 Antmicro
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
//
using System;
using System.Collections.Generic;
using System.Linq;

using ELFSharp.ELF.Sections;

namespace DWARF
{
    public class DebugInfoEntry
    {
        public static DebugInfoEntry Decode(byte[] section, ref ulong cursor, CompilationUnit cu)
        {
            var die = new DebugInfoEntry();

            die.Code = Parser.ReadULeb128(section, ref cursor);
            die.CompilationUnit = cu;

            if(die.Code.Low == 0 && die.Code.High == 0)
            {
                return die;
            }

            foreach(var attribute in die.Description.Attributes)
            {
                var payloadLength = AttributeParser.SizeOf(attribute.Item2, section, cursor, cu.DWARF64, cu.AddressSize);
                var payload = new byte[payloadLength];
                // REV: array.copy?
                for(var i = 0ul; i < payloadLength; i++)
                {
                    payload[i] = section[cursor + i];
                }
                die.attributes.Add(attribute.Item1, payload);
                cursor += payloadLength;
            }

            return die;
        }

        public ulong? GetAttributeLinePTR(AttributeName attribute)
        {
            var form = Description.NameToType[attribute];

            if(form == AttributeType.DW_FORM_sec_offset || form == AttributeType.DW_FORM_data4)
            {
                var bytes = this.attributes[attribute];

                ulong cursor = 0;
                //return Parser.ReadUDouble(bytes, ref cursor);
                return Parser.ReadUWord(bytes, ref cursor);
            }

            return null;
        }

/*
        public void AddChild(DebugInfoEntry child)
        {
            children.Add(child);
        }
*/
        public CompilationUnit CompilationUnit { get; private set; }
        public DebugInfoEntryAbbreviation Description => CompilationUnit.GetAbbrev(Code); // lazy decoding
        // public IEnumerable<DebugInfoEntry> Children => children;
        // public Dictionary<AttributeName, byte[]> Attributes => attributes;

        private readonly Dictionary<AttributeName, byte[]> attributes = new Dictionary<AttributeName, byte[]>();
        // private readonly List<DebugInfoEntry> children = new List<DebugInfoEntry>();

        public uLeb128 Code { get; private set; }
    }
}

