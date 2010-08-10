using Mosa.Platforms.x86;

namespace Mosa.Kernel.X86.Smbios
{
	public class CpuStructure : SmbiosStructure
	{
		private ushort clockFrequency = 0;
		private ushort maxSpeed = 0;
		private ushort currentSpeed = 0;
		
		public CpuStructure (uint address) : base (address)
		{
			this.clockFrequency = Native.Get16 (address + 0x12u);
			this.maxSpeed = Native.Get16 (address + 0x14u);
			this.currentSpeed = Native.Get16 (address + 0x16u);
		}
		
		public ushort MaxSpeed
		{
			get { return this.maxSpeed; }
		}
		
		public ushort CurrentSpeed
		{
			get { return this.currentSpeed; }
		}
		
		public ushort ClockFrequency
		{
			get { return this.clockFrequency; }
		}
	}
}