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
    public class CompilationUnit
    {
        public DebugInfoEntryAbbreviation GetAbbrev(uLeb128 id)
        {
            return abbrev.GetDieDescription(DebugAbbrevOffset, id);
        }

        public static CompilationUnit Decode(byte[] sectionContent, ref ulong cursor, AbbreviationsTable abbrev, DebugLines debugLines)
        {
            var compilationUnit = new CompilationUnit();

            compilationUnit.abbrev = abbrev;
            compilationUnit.dlines = debugLines;

            var originalCursor = cursor;
            compilationUnit.Length = Parser.ReadInitialLength(sectionContent, ref cursor, out var dwarf64);
            compilationUnit.DWARF64 = dwarf64;
            compilationUnit.Version = Parser.ReadUHalf(sectionContent, ref cursor);

            if(compilationUnit.Version >= 5)
            {
                // there is a new `unit_field` type added in DWARFv5 + `address_size` and `debug_abbrev_offset` are reordered
                compilationUnit.UnitType = Parser.ReadUByte(sectionContent, ref cursor);
                compilationUnit.AddressSize = Parser.ReadUByte(sectionContent, ref cursor);
                compilationUnit.DebugAbbrevOffset = Parser.ReadAddress(sectionContent, ref cursor, compilationUnit.DWARF64);
            }
            else
            {
                compilationUnit.DebugAbbrevOffset = Parser.ReadAddress(sectionContent, ref cursor, compilationUnit.DWARF64);
                compilationUnit.AddressSize = Parser.ReadUByte(sectionContent, ref cursor);
            }

            compilationUnit.Root = DebugInfoEntry.Decode(sectionContent, ref cursor, compilationUnit);

            cursor = originalCursor + compilationUnit.Length + 4; // TODO: why?
            return compilationUnit;
        }

        public override string ToString()
        {
            return $"length: {this.Length} version: {this.Version} addrsize: {this.AddressSize} abbrevoffset: {this.DebugAbbrevOffset}";
        }

        public byte UnitType { get; private set; }

        public ulong DebugAbbrevOffset { get; private set; }
        public byte AddressSize { get; private set; }
        public ulong Length { get; private set; }
        public ushort Version { get; private set; }
        public ulong TypeSignature { get; private set; }
        public ulong TypeOffset { get; private set; }

        public DebugInfoEntry Root { get; private set; }

        public DebugLines.DebugLinesPart Lines
        {
            get
            {
                // TODO: fix casting
                var lineCursor = (ulong)Root.GetAttributeLinePTR(AttributeName.DW_AT_stmt_list);
                return dlines.GetPart(lineCursor, this);
            }
        }

        public bool DWARF64 { get; private set; }

        private AbbreviationsTable abbrev;
        private DebugLines dlines;
    }
}
