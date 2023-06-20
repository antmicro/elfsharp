//
// Copyright (c) 2023 Antmicro
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
//
using System;
using System.Collections.Generic;
using System.Linq;

using ELFSharp.ELF;
using ELFSharp.ELF.Sections;

namespace DWARF
{
    public class DWARFReader
    {
        public static DWARFReader ParseDWARF(IELF elf)
        {
            var dwarf = new DWARFReader();

            var abbreviationsTableSection = GetSectionContent(elf, ".debug_abbrev");
            var stringsTableSection = GetSectionContent(elf, ".debug_str");
            var debugLinesStringsSection = GetSectionContent(elf, ".debug_line_str", required: false);
            var debugLinesSection = GetSectionContent(elf, ".debug_line");
            var debugInfoSection = GetSectionContent(elf, ".debug_info");

            dwarf.AbbreviationsTable = new AbbreviationsTable(abbreviationsTableSection);
            dwarf.Strings = new Strings(stringsTableSection);
            dwarf.LineStrings = new Strings(debugLinesStringsSection);
            dwarf.DebugLines = new DebugLines(debugLinesSection, dwarf.Strings, dwarf.LineStrings);
            dwarf.DebugInfo = new DebugInfo(debugInfoSection, dwarf.AbbreviationsTable, dwarf.DebugLines);

            return dwarf;
        }

        private static byte[] GetSectionContent(IELF elf, string name, bool required = true)
        {
            var section = elf.Sections.SingleOrDefault(x => x.Name == name);
            if(section == null)
            {
                if(required)
                {
                    throw new DwarfDecodingException($"Required '{name}' section not found");
                }

                return new byte[0];
            }

            var contents = section.GetContents();
            if(contents == null || contents.Length == 0)
            {
                if(required)
                {
                    throw new DwarfDecodingException($"Could not access required section's content: '{name}'");
                }

                return new byte[0];
            }

            return contents;
        }

        public bool TryGetLineForPC(ulong pc, out Line line)
        {
            if(pcToLineCache.TryGetValue(pc, out line))
            {
                return true;
            }

            Line? fallbackMatch = null;
            foreach(var compilationUnit in DebugInfo.CompilationUnits)
            {
                if(compilationUnit.Lines.TryFindMatch(pc, out line))
                {
                    if(line.Address == pc)
                    {
                        // exact match
                        pcToLineCache[pc] = line;
                        return true;
                    }

                    if(!fallbackMatch.HasValue || line.Address > fallbackMatch.Value.Address)
                    {
                        fallbackMatch = line;
                    }
                }
            }

            if(fallbackMatch.HasValue)
            {
                line = fallbackMatch.Value;
                return true;
            }

            line = default(Line);
            return false;
        }

        public AbbreviationsTable AbbreviationsTable { get; private set; }
        public DebugInfo DebugInfo { get; private set; }
        public Strings Strings { get; private set; }
        public Strings LineStrings { get; private set; }
        public DebugLines DebugLines { get; private set; }

        private readonly Dictionary<ulong, Line> pcToLineCache = new Dictionary<ulong, Line>();
    }
}