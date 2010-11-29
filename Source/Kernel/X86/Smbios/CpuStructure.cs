using Mosa.Platform.X86.Instrinics;

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
		private uint maxSpeed = 0;
		/// <summary>
		///
		/// </summary>
		private uint clockFrequency = 0;
		private uint dummy = 0;
		private string vendor = null;
		
		/// <summary>
		///
		/// </summary>
		public CpuStructure () : base (SmbiosManager.GetStructureOfType (0x04))
		{
			clockFrequency = Native.Get16 (address + 0x12u);
			maxSpeed = Native.Get16 (address + 0x14u);
			vendor = GetStringFromIndex (Native.Get8 (address + 0x07u));
			dummy = Native.Get16 (address + 0x16u);
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
		public uint ClockFrequency
		{
			get { return this.clockFrequency; }
		}
		
		/// <summary>
		///
		/// </summary>
		public string Vendor
		{
			get { return this.vendor; }
		}
	}
}