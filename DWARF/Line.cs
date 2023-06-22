//
// Copyright (c) 2023 Antmicro
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
//
using System;
using System.IO;
using System.Collections.Generic;

namespace DWARF
{
    public struct Line : ICloneable
    {
        public object Clone()
        {
            var copy = new Line();

            copy.Address = this.Address;
            copy.File = this.File;
            copy.LineNumber = this.LineNumber;
            copy.Column = this.Column;
            copy.IsStmt = this.IsStmt;
            copy.BasicBlock = this.BasicBlock;
            copy.EndSequence = this.EndSequence;
            copy.EpilogueBegin = this.EpilogueBegin;
            copy.PrologueEnd = this.PrologueEnd;
            copy.Isa = this.Isa;
            copy.Discriminator = this.Discriminator;
            copy.CompilationUnit = this.CompilationUnit;

            return copy;
        }

        public void SetDefaults(bool defaultIsStmt)
        {
            Address = 0;
            File = 1;
            LineNumber = 1;
            Column = 0;
            BasicBlock = false;
            EndSequence = false;
            PrologueEnd = false;
            EpilogueBegin = false;
            Isa = 0;
            Discriminator = new uLeb128(0);
            OpIndex = 0;
        }

        public string FilePath
        {
            get
            {
                if(File == 0)
                {
                    return "[unknown]";
                }

                var f = CompilationUnit.Lines.Files[(int)(File - 1)];
                return f.DirectoryIndex != 0
                    ? string.Format("{0}/{1}", CompilationUnit.Lines.Directories[f.DirectoryIndex - 1].Path, f.Path)
                    : f.Path;
            }
        }

        // REV: properites or private fields
        public ulong Address;
        public int LineNumber;
        public uint Column;
        public bool IsStmt;
        public bool BasicBlock;
        public bool EndSequence;
        public bool PrologueEnd;
        public bool EpilogueBegin;
        public uint Isa;
        public ulong OpIndex;
        public uLeb128 Discriminator;
        public uint File;
        public CompilationUnit CompilationUnit;
    }
}
