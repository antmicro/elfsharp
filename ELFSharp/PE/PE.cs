using System;
using System.Runtime.InteropServices;
using System.IO;
using System.Linq;
using MiscUtil;

namespace ELFSharp.PE
{
    public class PE
    {
        public PE(BinaryReader reader)
        {
            Init(reader);
        }

        public PE(string path)
        {
            using(var reader = new BinaryReader(File.OpenRead(path)))
            {
                Init(reader);
            }
        }

        public string[] GetExportedSymbols() => ExportedSymbolsNames;

        private void Init(BinaryReader reader)
        {
            var dosHeader = StreamToStructure<ImageDosHeader>(reader);
            
            reader.BaseStream.Seek(dosHeader.e_lfanew, SeekOrigin.Begin);
            fileHeader = StreamToStructure<ImageFileHeader>(reader);
            var optionalHeaderRaw = reader.BaseStream.Position;
            var sectionHeaderRaw = optionalHeaderRaw + fileHeader.SizeOfOptionalHeader;

            var optionalHeader32 = StreamToStructure<ImageOptionalHeader32>(reader);
            var exportTableHeader = new ImageDataDirectory();
            if(optionalHeader32.IsValid)
            {
                 exportTableHeader = optionalHeader32.ExportTable;
            }
            else
            {
                reader.BaseStream.Seek(optionalHeaderRaw, SeekOrigin.Begin);
                var optionalHeader64 = StreamToStructure<ImageOptionalHeader64>(reader);
                if(!optionalHeader64.IsValid)
                {
                    return;
                }
                exportTableHeader = optionalHeader64.ExportTable;
            }

            reader.BaseStream.Seek(sectionHeaderRaw, SeekOrigin.Begin);
            sectionHeaders = Misc.Iterate<ImageSectionHeader>(() => StreamToStructure<ImageSectionHeader>(reader))
                .Take(fileHeader.NumberOfSections).ToArray();

            reader.BaseStream.Seek(VdaToRaw(exportTableHeader.VirtualAddress), SeekOrigin.Begin);
            var exportDirectory = StreamToStructure<ImageExportDirectory>(reader);

            var nameTableAddress = VdaToRaw(exportDirectory.AddressOfNames);
            var namesTable = new NamesTable(reader, nameTableAddress, exportDirectory.NumberOfNames, this);
            ExportedSymbolsNames = namesTable.GetEntriesNames();
        }

        public UInt32 VdaToRaw(UInt32 vda)
        {
            TryFindSectionContaining(vda, out var foundSectionHeader);
            var relativeToSection = vda - foundSectionHeader.VirtualAddress;
            return foundSectionHeader.PointerToRawData + relativeToSection;
        }

        private bool TryFindSectionContaining(UInt32 vda, out ImageSectionHeader foundSectionHeader)
        {
            foundSectionHeader = new ImageSectionHeader();
            var found = false;
            foreach(var s in sectionHeaders)
            {
                if(s.VirtualAddress <= vda && vda < s.VirtualAddress + s.VirtualSize)
                {
                    found = true;
                    foundSectionHeader = s;
                }
            }
            return found;
        }

        private static T StreamToStructure<T>(BinaryReader reader) where T : struct
        {
            var bytes = reader.ReadBytes(Marshal.SizeOf(typeof(T)));
            GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            var result = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
            handle.Free();
            return result;
        }

        private ImageFileHeader fileHeader;
        private ImageSectionHeader[] sectionHeaders;
        private string[] ExportedSymbolsNames = new string[0];
    }
}

