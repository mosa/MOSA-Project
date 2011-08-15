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
		private string vendor = null;
		/// <summary>
		/// 
		/// </summary>
		private string version = null;
		/// <summary>
		///
		/// </summary>
		private uint maxSpeed = 0;
		/// <summary>
		///
		/// </summary>
		private string socket = null;
		
		/// <summary>
		///
		/// </summary>
		public CpuStructure () : base (SmbiosManager.GetStructureOfType (0x04))
		{
			this.version = GetStringFromIndex(Native.Get8(address + 0x10u));
			this.socket = GetStringFromIndex(Native.Get8(address + 0x04u));
			this.maxSpeed = Native.Get16(address + 0x16u);
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
		public string Socket
		{
			get { return this.socket; }
		}
		
		/// <summary>
		///
		/// </summary>
		public string Vendor
		{
			get { return this.vendor; }
		}

		/// <summary>
		///
		/// </summary>
		public string Version
		{
			get { return this.version; }
		}
	}
}