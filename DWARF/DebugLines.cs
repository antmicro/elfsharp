//
// Copyright (c) 2023 Antmicro
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
//
using System;
using System.Linq;
using System.Collections.Generic;

namespace DWARF
{
    public class DebugLines
    {
        public DebugLines(byte[] sectionContent, Strings strings, Strings lineStrings)
        {
            cache = new Dictionary<ulong, DebugLinesPart>();

            this.sectionContent = sectionContent;
            this.strings = strings;
            this.lineStrings = lineStrings;
        }

        public DebugLinesPart GetPart(ulong cursor, CompilationUnit cu)
        {
            DebugLinesPart part;
            if(!cache.TryGetValue(cursor, out part))
            {
                part = new DebugLinesPart(sectionContent, strings, lineStrings, ref cursor, cu);
                cache[cursor] = part;
            }

            return part;
        }

        public class DebugLinesPart
        {
            public DebugLinesPart(byte[] sectionContent, Strings strings, Strings lineStrings, ref ulong cursor, CompilationUnit cu)
            {
                this.sectionContent = sectionContent;
                this.strings = strings;
                this.lineStrings = lineStrings;

                Files = new List<Entry>();
                Directories = new List<Entry>();
                Lines = new List<Line>();

                ParseHeader(ref cursor, cu);
                ReadBytecode(ref cursor, cu);
            }

            public bool TryFindMatch(ulong pc, out Line line)
            {
                if(Lines.Count == 0)
                {
                    line = default(Line);
                    return false;
                }

                var idx = Array.BinarySearch(Lines.Select(x => x.Address).ToArray(), pc);
                if(idx >= 0)
                {
                    line = Lines[idx];
                    return true;
                }

                // this is how `BinarySearch` encodes additional information in case of a failure:
                // idx is the bitwise complement of an index of the first entry larger than `pc`
                idx = ~idx;

                if(idx == 0)
                {
                    line = default(Line);
                    return false;
                }

                line = Lines[idx - 1];
                return true;
            }

            // REV: change into enumerable?
            public List<Entry> Directories { get; private set; }
            public List<Entry> Files { get; private set; }
            public List<Line> Lines { get; private set; }

            public ushort Version {get; private set;}
            public ulong Start { get; private set; }
            public byte AddressSize { get; private set; }
            public byte SegmentSectorSize { get; private set; }
            public ulong Length { get; private set; }
            public bool DWARF64 { get; private set; }
            public ulong HeaderLength { get; private set; }
            public byte MinimumInstructionLength { get; private set; }
            public byte MaximumOperationsPerInstruction { get; private set; } = 1;
            public bool DefaultIsStmt { get; private set; }
            public sbyte LineBase { get; private set; }
            public byte LineRange { get; private set; }
            public byte OpcodeBase { get; private set; }
            public uLeb128[] StandardOpcodeLengths { get; private set; }
            public uLeb128[] DirectoryEntryFormat { get; private set; }
            public uLeb128[] FileEntryFormat { get; private set; }
            public CompilationUnit CompilationUnit { get; private set; }

            private void AddToLines(Line l)
            {
                if(Lines.Count > 0 && Lines[Lines.Count - 1].Address == l.Address)
                {
                    Lines.RemoveAt(Lines.Count - 1);
                }
                Lines.Add(l);
            }

            private void ParseHeader(ref ulong cursor, CompilationUnit cu)
            {
                this.Length = Parser.ReadInitialLength(sectionContent, ref cursor, out var dwarf64);
                this.DWARF64 = dwarf64;
                this.Start = cursor;
                this.Version = Parser.ReadUHalf(sectionContent, ref cursor);
                this.CompilationUnit = cu;

                if(this.Version >= 5)
                {
                    this.AddressSize = Parser.ReadUByte(sectionContent, ref cursor);
                    this.SegmentSectorSize = Parser.ReadUByte(sectionContent, ref cursor);
                }

                this.HeaderLength = Parser.ReadAddress(sectionContent, ref cursor, this.DWARF64);
                this.MinimumInstructionLength = Parser.ReadUByte(sectionContent, ref cursor);

                if(this.Version >= 5)
                {
                    this.MaximumOperationsPerInstruction = Parser.ReadUByte(sectionContent, ref cursor);
                }

                this.DefaultIsStmt = (Parser.ReadUByte(sectionContent, ref cursor) == 1);
                this.LineBase = (sbyte)Parser.ReadUByte(sectionContent, ref cursor);
                this.LineRange = Parser.ReadUByte(sectionContent, ref cursor);
                this.OpcodeBase = Parser.ReadUByte(sectionContent, ref cursor);

                this.StandardOpcodeLengths = new uLeb128[this.OpcodeBase];
                for(var i = 0; i < this.OpcodeBase; i++)
                {
                    this.StandardOpcodeLengths[i] = Parser.ReadULeb128(sectionContent, ref cursor);
                }

                if(this.Version >= 5)
                {
                    var directoryEntryFormatCount = Parser.ReadUByte(sectionContent, ref cursor);
                    this.DirectoryEntryFormat = new uLeb128[directoryEntryFormatCount * 2];
                    for(var i = 0; i < this.DirectoryEntryFormat.Length; i++)
                    {
                        this.DirectoryEntryFormat[i] = Parser.ReadULeb128(sectionContent, ref cursor);
                    }

                    var directoriesCount = (ulong)Parser.ReadULeb128(sectionContent, ref cursor);
                    for(var i = 0uL; i < directoriesCount; i++)
                    {
                        var entry = Entry.Parse(sectionContent, ref cursor, this.DirectoryEntryFormat, this.DWARF64, strings, lineStrings);
                        this.Directories.Add(entry);
                    }

                    var fileEntryFormatCount = Parser.ReadUByte(sectionContent, ref cursor);
                    this.FileEntryFormat = new uLeb128[fileEntryFormatCount * 2];
                    for(var i = 0; i < this.DirectoryEntryFormat.Length; i++)
                    {
                        this.DirectoryEntryFormat[i] = Parser.ReadULeb128(sectionContent, ref cursor);
                    }

                    var filesCount = (ulong)Parser.ReadULeb128(sectionContent, ref cursor);
                    for(var i = 0uL; i < filesCount; i++)
                    {
                        var entry = Entry.Parse(sectionContent, ref cursor, this.FileEntryFormat, this.DWARF64, strings, lineStrings);
                        this.Files.Add(entry);
                    }
                }
                else
                {
                    while(sectionContent[cursor] != 0)
                    {
                        var entry = new Entry();
                        entry.Path = Parser.ReadString(sectionContent, ref cursor);
                        this.Directories.Add(entry);
                    }
                    // REV: why?
                    cursor += 1;

                    while(sectionContent[cursor] != 0)
                    {
                        var entry = new Entry();

                        entry.Path = Parser.ReadString(sectionContent, ref cursor);
                        entry.DirectoryIndex = Parser.ReadULeb128(sectionContent, ref cursor);
                        entry.Timestamp = Parser.ReadULeb128(sectionContent, ref cursor);
                        entry.Size = Parser.ReadULeb128(sectionContent, ref cursor);
                        entry.Md5 = new uLeb128(0);

                        this.Files.Add(entry);
                    }
                    // REV: why?
                    cursor += 1;
                }

                if(this.Version < 5)
                {
                    this.AddressSize = this.CompilationUnit.AddressSize;
                }
            }

            public void ReadBytecode(ref ulong cursor, CompilationUnit cu)
            {
                var state = new Line();
                state.CompilationUnit = cu;
                state.SetDefaults(DefaultIsStmt);

                while(cursor < Start + Length)
                {
                    var opcode = Parser.ReadUByte(sectionContent, ref cursor);

                    if(opcode >= OpcodeBase)
                    {
                        // Special opcode
                        var adjustedOpcode = (byte)(opcode - OpcodeBase);
                        var operationAdvance = (ulong)(adjustedOpcode / this.LineRange);

                        var newAddress = state.Address + this.MinimumInstructionLength * ((state.OpIndex + operationAdvance) / MaximumOperationsPerInstruction);
                        var newOpIndex = (state.OpIndex + operationAdvance) % MaximumOperationsPerInstruction;

                        var addressIncrement = (ulong)((adjustedOpcode / this.LineRange) * this.MinimumInstructionLength);
                        var lineIncrement = (int)(this.LineBase + (adjustedOpcode % this.LineRange));

                        state.Address = newAddress;
                        state.LineNumber += lineIncrement;
                        state.OpIndex = newOpIndex;

                        var clone = (Line)state.Clone();
                        AddToLines(clone);

                        state.BasicBlock = false;
                        state.PrologueEnd = false;
                        state.EpilogueBegin = false;
                        state.Discriminator.Low = 0;
                        state.Discriminator.High = 0;
                    }

                    if(opcode != (byte)StandardLineOpcodes.EXTENDED)
                    {
                        HandleStandardOpcode(opcode, ref state, ref cursor);
                    }
                    else
                    {
                        HandleExtendedOpcode(ref state, ref cursor);
                    }
                }
            }

            private void HandleStandardOpcode(byte opcode, ref Line state, ref ulong cursor)
            {
                switch((StandardLineOpcodes)opcode)
                {
                    case StandardLineOpcodes.DW_LNS_copy:
                    {
                        var clone = (Line)state.Clone();
                        AddToLines(clone);

                        state.BasicBlock = false;
                        state.PrologueEnd = false;
                        state.EpilogueBegin = false;
                        state.Discriminator.Low = 0;
                        state.Discriminator.High = 0;
                        break;
                    }

                    case StandardLineOpcodes.DW_LNS_advance_pc:
                    {
                        var operand = (ulong)Parser.ReadULeb128(sectionContent, ref cursor);
                        operand *= this.MinimumInstructionLength;
                        state.Address += operand;
                        break;
                    }

                    case StandardLineOpcodes.DW_LNS_advance_line:
                    {
                        var operand = Parser.ReadSLeb128(sectionContent, ref cursor);
                        state.LineNumber += (int)operand;
                        break;
                    }

                    case StandardLineOpcodes.DW_LNS_set_file:
                    {
                        var operand = Parser.ReadULeb128(sectionContent, ref cursor);
                        state.File = (uint)operand;
                        break;
                    }

                    case StandardLineOpcodes.DW_LNS_set_column:
                    {
                        var operand = Parser.ReadULeb128(sectionContent, ref cursor);
                        state.Column = (uint)operand;
                        break;
                    }

                    case StandardLineOpcodes.DW_LNS_negate_stmt:
                    {
                        state.IsStmt = !state.IsStmt;
                        break;
                    }

                    case StandardLineOpcodes.DW_LNS_set_basic_block:
                    {
                        state.BasicBlock = true;
                        break;
                    }

                    case StandardLineOpcodes.DW_LNS_const_add_pc:
                    {
                        var operationAdvance = (ulong)opcode / LineRange;
                        state.Address = state.Address + MinimumInstructionLength * ((state.OpIndex + operationAdvance) / MaximumOperationsPerInstruction);
                        state.OpIndex = (state.OpIndex + operationAdvance) % MaximumOperationsPerInstruction;
                        break;
                    }

                    case StandardLineOpcodes.DW_LNS_fixed_advance_pc:
                    {
                        var operand = Parser.ReadUHalf(sectionContent, ref cursor);
                        state.Address += operand;
                        state.OpIndex = 0;
                        break;
                    }

                    case StandardLineOpcodes.DW_LNS_set_prologue_end:
                    {
                        state.PrologueEnd = true;
                        break;
                    }

                    case StandardLineOpcodes.DW_LNS_set_epilogue_begin:
                    {
                        state.EpilogueBegin = true;
                        break;
                    }

                    case StandardLineOpcodes.DW_LNS_set_isa:
                    {
                        state.Isa = Parser.ReadULeb128(sectionContent, ref cursor);
                        break;
                    }
                }
            }

            private void HandleExtendedOpcode(ref Line state, ref ulong cursor)
            {
                var length = Parser.ReadULeb128(sectionContent, ref cursor);
                var extendedOpcode = (ExtendedLineOpcodes)Parser.ReadUByte(sectionContent, ref cursor);

                switch(extendedOpcode)
                {
                    case ExtendedLineOpcodes.DW_LNE_end_sequence:
                    {
                        state.EndSequence = true;
                        var copy = (Line)state.Clone();
                        AddToLines(copy);

                        state.SetDefaults(DefaultIsStmt);
                        break;
                    }

                    case ExtendedLineOpcodes.DW_LNE_set_address:
                    {
                        state.Address = Parser.ReadAddress(sectionContent, ref cursor, AddressSize);
                        state.OpIndex = 0;
                        break;
                    }

                    case ExtendedLineOpcodes.DW_LNE_define_file:
                    {
                        var entry = new Entry();

                        entry.Path = Parser.ReadString(sectionContent, ref cursor);
                        entry.DirectoryIndex = Parser.ReadULeb128(sectionContent, ref cursor);
                        entry.Timestamp = Parser.ReadULeb128(sectionContent, ref cursor);
                        entry.Size = Parser.ReadULeb128(sectionContent, ref cursor);

                        Files.Add(entry);
                        break;
                    }

                    case ExtendedLineOpcodes.DW_LNE_set_discriminator:
                    {
                        var discriminator = Parser.ReadULeb128(sectionContent, ref cursor);
                        state.Discriminator = discriminator;
                        break;
                    }

                    default:
                    {
                        // length - 1 because we already read opcode
                        cursor += (ulong)length - 1;
                        break;
                    }
                }
            }

            private readonly byte[] sectionContent;
            private readonly Strings strings;
            private readonly Strings lineStrings;
        }

        private readonly Dictionary<ulong, DebugLinesPart> cache;

        private readonly byte[] sectionContent;
        private readonly Strings strings;
        private readonly Strings lineStrings;
    }
}
