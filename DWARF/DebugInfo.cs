//
// Copyright (c) 2023 Antmicro
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
//
using System;
using System.Collections.Generic;
using System.Linq;

namespace DWARF
{
    public class DebugInfo
    {
        public DebugInfo (byte[] debugInfoSectionContent, AbbreviationsTable abbrev, DebugLines dlines)
        {
            ulong cursor = 0;
            while(cursor < (ulong)debugInfoSectionContent.Length)
            {
                var cu = CompilationUnit.Decode(debugInfoSectionContent, ref cursor, abbrev, dlines);
                compilationUnits.Add(cu);
            }
        }

        public IEnumerable<CompilationUnit> CompilationUnits => compilationUnits;

        private readonly List<CompilationUnit> compilationUnits = new List<CompilationUnit>();
    }
}
