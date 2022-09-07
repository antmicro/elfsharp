//
// Copyright (c) 2023 Antmicro
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
//

using System.Collections.Generic;

using ELFSharp.ELF.Sections;

namespace DWARF
{
    public class AbbreviationsTable
    {
        public AbbreviationsTable(byte[] sectionContent)
        {
            this.sectionContent = sectionContent;
            abbrevations = new Dictionary<ulong, Dictionary<uLeb128, DebugInfoEntryAbbreviation>>();
        }

        public DebugInfoEntryAbbreviation GetDieDescription(ulong offset, uLeb128 id)
        {
            if(!abbrevations.TryGetValue(offset, out var mapping))
            {
                mapping = Decode(offset);
                abbrevations[offset] = mapping;
            }

            if(!mapping.TryGetValue(id, out var result))
            {
                throw new DwarfDecodingException($"Tried to access non-existing DIE abbreviations of id {id} at offset {offset}");
            }

            return result;
        }

        private Dictionary<uLeb128, DebugInfoEntryAbbreviation> Decode(ulong cursor)
        {
            var result = new Dictionary<uLeb128, DebugInfoEntryAbbreviation>();

            while(cursor < (ulong)sectionContent.Length)
            {
                var id = Parser.ReadULeb128(sectionContent, ref cursor);
                if(id.Low == 0 && id.High == 0)
                {
                    break;
                }

                var description = DebugInfoEntryAbbreviation.Decode(sectionContent, ref cursor);
                result.Add(id, description);
            }

            return result;
        }

        private readonly byte[] sectionContent;
        private readonly Dictionary<ulong, Dictionary<uLeb128, DebugInfoEntryAbbreviation>> abbrevations;
    }
}
