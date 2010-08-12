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
		private string biosVendor = string.Empty;
		
		/// <summary>
		///
		/// </summary>
		public BiosInformationStructure () : base (SmbiosManager.GetStructureOfType (0x00))
		{
			this.biosVendor = GetStringFromIndex (Native.Get8 (address + 0x04u));
		}
		
		/// <summary>
		///
		/// </summary>
		public string BiosVendor
		{
			get { return this.biosVendor; }
		}
	}
}