using Mosa.Platform.x86.Intrinsic;

namespace Mosa.Kernel.x86.Smbios
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
		/// <summary>
		/// 
		/// </summary>
		private string vendor = null;
		
		/// <summary>
		///
		/// </summary>
		public CpuStructure () : base (SmbiosManager.GetStructureOfType (0x04))
		{
			this.clockFrequency = Native.Get16 (address + 0x12u);
			this.maxSpeed = Native.Get16(address + 0x14u);
			this.vendor = GetStringFromIndex(Native.Get8(address + 0x07u));
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