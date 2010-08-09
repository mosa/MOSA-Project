using Mosa.Platforms.x86;

namespace Mosa.Kernel.X86.Smbios
{
	public class SmbiosStructure
	{
		private SmbiosManager.StructureHeader header;
		protected uint address = 0u;
		
		public SmbiosStructure (uint address)
		{
			this.address = address;
			header.Type = Native.Get8 (address);
			header.Length = Native.Get8 (address + 1);
			header.Handle = Native.Get16 (address + 2);
		}
		
		public SmbiosManager.StructureHeader Header
		{
			get { return header; }
		}
	}
}