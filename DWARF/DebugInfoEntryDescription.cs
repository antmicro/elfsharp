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
    public class DebugInfoEntryAbbreviation
    {
        public static DebugInfoEntryAbbreviation Decode(byte[] sectionContent, ref ulong cursor)
        {
            var entry = new DebugInfoEntryAbbreviation();

            entry.Type = (DebugInfoEntryType)(ulong)Parser.ReadULeb128(sectionContent, ref cursor);
            entry.HasChildren = (Parser.ReadUByte(sectionContent, ref cursor) == 1); // REV: what about other values?

            while(true)
            {
                var attributeName = (AttributeName)(ulong)Parser.ReadULeb128(sectionContent, ref cursor);
                var attributeType = (AttributeType)(ulong)Parser.ReadULeb128(sectionContent, ref cursor); // REV: should we read attribute type when attribute name is 0?

                if(attributeName == AttributeName.NONE && attributeType == (AttributeType)0) // TODO: use proper enum value
                {
                    break;
                }

                entry.attributes.Add(new Tuple<AttributeName, AttributeType>(attributeName, attributeType));
                entry.nameToType.Add(attributeName, attributeType); // REV: hmm?!

                if(attributeType == AttributeType.DW_FORM_implicit_const)
                {
                    entry.implicitConsts.Add(attributeName, Parser.ReadSLeb128(sectionContent, ref cursor));
                }
            }

            return entry;
        }

        public DebugInfoEntryType Type { get; private set; }
        public bool HasChildren { get; private set; }

        public IEnumerable<Tuple<AttributeName, AttributeType>> Attributes => attributes;
        public Dictionary<AttributeName, AttributeType> NameToType => nameToType;
        public Dictionary<AttributeName, Leb128> ImplicitConsts => implicitConsts;

        private readonly List<Tuple<AttributeName, AttributeType>> attributes = new List<Tuple<AttributeName, AttributeType>>();
        private readonly Dictionary<AttributeName, AttributeType> nameToType = new Dictionary<AttributeName, AttributeType>();
        private readonly Dictionary<AttributeName, Leb128> implicitConsts = new Dictionary<AttributeName, Leb128>();
    }
}

