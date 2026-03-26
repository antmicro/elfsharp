using System;
using System.Runtime.InteropServices;

namespace ELFSharp.PE
{
    // Struct layout based on: http://www.pinvoke.net/default.aspx/Structures/IMAGE_FILE_HEADER32.html
    // and https://www.pinvoke.net/default.aspx/Structures/IMAGE_DATA_DIRECTORY

    public enum MagicType : UInt16
    {
        OptionalHdr32Magic = 0x10B,
        OptionalHdr64Magic = 0x20B
    }
    
    [StructLayout(LayoutKind.Explicit)]
    public struct ImageOptionalHeader32
    {
        [FieldOffset(0)]
        public MagicType Magic;

        [FieldOffset(2)]
        public byte MajorLinkerVersion;

        [FieldOffset(3)]
        public byte MinorLinkerVersion;

        [FieldOffset(4)]
        public UInt32 SizeOfCode;

        [FieldOffset(8)]
        public UInt32 SizeOfInitializedData;

        [FieldOffset(12)]
        public UInt32 SizeOfUninitializedData;

        [FieldOffset(16)]
        public UInt32 AddressOfEntryPoint;

        [FieldOffset(20)]
        public UInt32 BaseOfCode;

        // PE32 contains this additional field
        [FieldOffset(24)]
        public UInt32 BaseOfData;

        [FieldOffset(28)]
        public UInt32 ImageBase;

        [FieldOffset(32)]
        public UInt32 SectionAlignment;

        [FieldOffset(36)]
        public UInt32 FileAlignment;

        [FieldOffset(40)]
        public UInt16 MajorOperatingSystemVersion;

        [FieldOffset(42)]
        public UInt16 MinorOperatingSystemVersion;

        [FieldOffset(44)]
        public UInt16 MajorImageVersion;

        [FieldOffset(46)]
        public UInt16 MinorImageVersion;

        [FieldOffset(48)]
        public UInt16 MajorSubsystemVersion;

        [FieldOffset(50)]
        public UInt16 MinorSubsystemVersion;

        [FieldOffset(52)]
        public UInt32 Win32VersionValue;

        [FieldOffset(56)]
        public UInt32 SizeOfImage;

        [FieldOffset(60)]
        public UInt32 SizeOfHeaders;

        [FieldOffset(64)]
        public UInt32 CheckSum;

        [FieldOffset(68)]
        public UInt16 Subsystem;

        [FieldOffset(70)]
        public UInt16 DllCharacteristics;

        [FieldOffset(72)]
        public UInt32 SizeOfStackReserve;

        [FieldOffset(76)]
        public UInt32 SizeOfStackCommit;

        [FieldOffset(80)]
        public UInt32 SizeOfHeapReserve;

        [FieldOffset(84)]
        public UInt32 SizeOfHeapCommit;

        [FieldOffset(88)]
        public UInt32 LoaderFlags;

        [FieldOffset(92)]
        public UInt32 NumberOfRvaAndSizes;

        [FieldOffset(96)]
        public ImageDataDirectory ExportTable;

        [FieldOffset(104)]
        public ImageDataDirectory ImportTable;

        [FieldOffset(112)]
        public ImageDataDirectory ResourceTable;

        [FieldOffset(120)]
        public ImageDataDirectory ExceptionTable;

        [FieldOffset(128)]
        public ImageDataDirectory CertificateTable;

        [FieldOffset(136)]
        public ImageDataDirectory BaseRelocationTable;

        [FieldOffset(144)]
        public ImageDataDirectory Debug;

        [FieldOffset(152)]
        public ImageDataDirectory Architecture;

        [FieldOffset(160)]
        public ImageDataDirectory GlobalPtr;

        [FieldOffset(168)]
        public ImageDataDirectory TLSTable;

        [FieldOffset(176)]
        public ImageDataDirectory LoadConfigTable;

        [FieldOffset(184)]
        public ImageDataDirectory BoundImport;

        [FieldOffset(192)]
        public ImageDataDirectory IAT;

        [FieldOffset(200)]
        public ImageDataDirectory DelayImportDescriptor;

        [FieldOffset(208)]
        public ImageDataDirectory CLRRuntimeHeader;

        [FieldOffset(216)]
        public ImageDataDirectory Reserved;

        public bool IsValid => Magic == MagicType.OptionalHdr32Magic;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct ImageOptionalHeader64
    {
        [FieldOffset(0)]
        public MagicType Magic;

        [FieldOffset(2)]
        public byte MajorLinkerVersion;

        [FieldOffset(3)]
        public byte MinorLinkerVersion;

        [FieldOffset(4)]
        public UInt32 SizeOfCode;

        [FieldOffset(8)]
        public UInt32 SizeOfInitializedData;

        [FieldOffset(12)]
        public UInt32 SizeOfUninitializedData;

        [FieldOffset(16)]
        public UInt32 AddressOfEntryPoint;

        [FieldOffset(20)]
        public UInt32 BaseOfCode;

        [FieldOffset(24)]
        public UInt64 ImageBase;

        [FieldOffset(32)]
        public UInt32 SectionAlignment;

        [FieldOffset(36)]
        public UInt32 FileAlignment;

        [FieldOffset(40)]
        public UInt16 MajorOperatingSystemVersion;

        [FieldOffset(42)]
        public UInt16 MinorOperatingSystemVersion;

        [FieldOffset(44)]
        public UInt16 MajorImageVersion;

        [FieldOffset(46)]
        public UInt16 MinorImageVersion;

        [FieldOffset(48)]
        public UInt16 MajorSubsystemVersion;

        [FieldOffset(50)]
        public UInt16 MinorSubsystemVersion;

        [FieldOffset(52)]
        public UInt32 Win32VersionValue;

        [FieldOffset(56)]
        public UInt32 SizeOfImage;

        [FieldOffset(60)]
        public UInt32 SizeOfHeaders;

        [FieldOffset(64)]
        public UInt32 CheckSum;

        [FieldOffset(68)]
        public UInt16 Subsystem;

        [FieldOffset(70)]
        public UInt16 DllCharacteristics;

        [FieldOffset(72)]
        public UInt64 SizeOfStackReserve;

        [FieldOffset(80)]
        public UInt64 SizeOfStackCommit;

        [FieldOffset(88)]
        public UInt64 SizeOfHeapReserve;

        [FieldOffset(96)]
        public UInt64 SizeOfHeapCommit;

        [FieldOffset(104)]
        public UInt32 LoaderFlags;

        [FieldOffset(108)]
        public UInt32 NumberOfRvaAndSizes;

        [FieldOffset(112)]
        public ImageDataDirectory ExportTable;

        [FieldOffset(120)]
        public ImageDataDirectory ImportTable;

        [FieldOffset(128)]
        public ImageDataDirectory ResourceTable;

        [FieldOffset(136)]
        public ImageDataDirectory ExceptionTable;

        [FieldOffset(144)]
        public ImageDataDirectory CertificateTable;

        [FieldOffset(152)]
        public ImageDataDirectory BaseRelocationTable;

        [FieldOffset(160)]
        public ImageDataDirectory Debug;

        [FieldOffset(168)]
        public ImageDataDirectory Architecture;

        [FieldOffset(176)]
        public ImageDataDirectory GlobalPtr;

        [FieldOffset(184)]
        public ImageDataDirectory TLSTable;

        [FieldOffset(192)]
        public ImageDataDirectory LoadConfigTable;

        [FieldOffset(200)]
        public ImageDataDirectory BoundImport;

        [FieldOffset(208)]
        public ImageDataDirectory IAT;

        [FieldOffset(216)]
        public ImageDataDirectory DelayImportDescriptor;

        [FieldOffset(224)]
        public ImageDataDirectory CLRRuntimeHeader;

        [FieldOffset(232)]
        public ImageDataDirectory Reserved;

        public bool IsValid => Magic == MagicType.OptionalHdr64Magic;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct ImageDataDirectory
    {
        public UInt32 VirtualAddress;
        public UInt32 Size;
    }
}

