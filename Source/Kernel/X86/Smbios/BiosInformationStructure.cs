using Mosa.Platforms.x86;

namespace Mosa.Kernel.X86.Smbios
{
	/// <summary>
	///
	/// </summary>
	public class BiosInformationStructure : SmbiosStructure
	{
		/// <summary>
		///
		/// </summary>
		string vendor = string.Empty;
		
		public BiosInformationStructure (uint address) : base (address)
		{
			ReadHeader (address);
			vendor = GetStringFromIndex (Native.Get8 (address + 0x04u));
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