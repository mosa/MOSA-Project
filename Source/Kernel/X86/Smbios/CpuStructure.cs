using Mosa.Platforms.x86;

namespace Mosa.Kernel.X86.Smbios
{
	/// <summary>
	///
	/// </summary>
	public class CpuStructure : SmbiosStructure
	{
		/// <summary>
		///
		/// </summary>
		private uint currentSpeed = 0;
		/// <summary>
		///
		/// </summary>
		private uint maxSpeed = 0;
		/// <summary>
		///
		/// </summary>
		private uint clockFrequency = 0;
		
		/// <summary>
		///
		/// </summary>
		public CpuStructure () : base (SmbiosManager.GetStructureOfType (0x04))
		{
			clockFrequency = Native.Get16 (address + 0x12u);
			maxSpeed = Native.Get16 (address + 0x14u);
			currentSpeed = Native.Get16 (address + 0x16u);
		}
		
		/// <summary>
		///
		/// </summary>
		public uint MaxSpeed
		{
			get { return this.maxSpeed; }
		}
		
		/// <summary>
		///
		/// </summary>
		public uint CurrentSpeed
		{
			get { return this.currentSpeed; }
		}
		
		/// <summary>
		///
		/// </summary>
		public uint ClockFrequency
		{
			get { return this.clockFrequency; }
		}
	}
}