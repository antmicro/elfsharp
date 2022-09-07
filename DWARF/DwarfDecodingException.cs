//
// Copyright (c) 2023 Antmicro
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
//
using System;

using ELFSharp.ELF;
using ELFSharp.ELF.Sections;

namespace DWARF
{
    public class DwarfDecodingException : Exception
    {
        public DwarfDecodingException(string message) : base(message)
        {
        }
    }
}
