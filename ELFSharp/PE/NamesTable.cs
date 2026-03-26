using System;
using System.IO;
using System.Text;
using System.Linq;

namespace ELFSharp.PE
{
    public class NamesTable
    {
        public NamesTable(BinaryReader reader, uint nameTableAddress, uint size, PE pe)
        {
            reader.BaseStream.Seek(nameTableAddress, SeekOrigin.Begin);
            entries = new Entry[size];
            for(var i = 0; i < entries.Length; i++)
            {
                entries[i].NamePointer = reader.ReadUInt32();
            }

            for(var i = 0; i < entries.Length; i++)
            {
                reader.BaseStream.Seek(pe.VdaToRaw(entries[i].NamePointer), SeekOrigin.Begin);
                var name = new StringBuilder();
                byte b;
                do
                {
                    b = reader.ReadByte();
                    if(b != 0)
                    {
                        name.Append((char)b);
                    }
                } 
                while(b != 0);
                entries[i].Name = name.ToString();
            }
        }

        public string[] GetEntriesNames()
        {
            return entries.Select(x => x.Name).ToArray();
        }

        private readonly Entry[] entries;

        private struct Entry
        {
            public UInt32 NamePointer;
            public string Name;
        }
    }
}

