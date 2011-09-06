using System;
using System.IO;

namespace ELFSharp
{
	public class SectionHeader32 : SectionHeader
	{
		internal SectionHeader32(BinaryReader reader, StringTable table = null) : base(reader, Class.Bit32, table)
        {
            
        }
		
		public uint Flags
		{
			get
			{
				return unchecked((uint) FlagsLong);
			}
		}
		
		public uint LoadAddress
		{
			get
			{
				return unchecked((uint) LoadAddressLong);
			}
		}
		
		public uint Size
		{
			get
			{
				return unchecked((uint) SizeLong);
			}
		}
		
	}
}

