//
// Copyright (c) 2023 Antmicro
//
// This file is licensed under the MIT License.
// Full license text is available in 'licenses/MIT.txt'.
//
using System.Collections.Generic;
using System.Linq;
using System;

using ELFSharp.ELF.Sections;

namespace DWARF
{
    public class Strings
    {
        public Strings(byte[] sectionContent)
        {
            strings = new Dictionary<ulong, string>();

            ulong cursor = 0;
            while(cursor < (ulong)sectionContent.Length)
            {
                var textOffset = cursor;
                var text = Parser.ReadString(sectionContent, ref cursor);

                strings.Add(textOffset, text);
            }
        }

        public string Get(ulong address)
        {
            if(strings.TryGetValue(address, out var val))
            {
                return val;
            }

            var smallerKeys = strings.Keys.Where(key => key < address).ToArray();
            if(smallerKeys == null)
            {
                return null;
            }
            ulong previousAddress = smallerKeys.Max();
            var offset = (int)(address - previousAddress);

            if(offset >= strings[previousAddress].Length)
            {
                return null;
            }

            var subString = strings[previousAddress].Substring(offset);
            strings[address] = subString;

            return subString;
        }

        private readonly Dictionary<ulong, string> strings;
    }
}
