using Mosa.Platforms.x86;

namespace Mosa.Kernel.X86.Smbios
{
	public class CpuStructure : SmbiosStructure
	{
		private ushort maxSpeed = 0;
		
		public CpuStructure (uint address) : base (address)
		{
			this.maxSpeed = Native.Get16 (this.address + 0x18u);
		}
		
		public ushort MaxSpeed
		{
			get { return this.maxSpeed; }
		}
	}
}