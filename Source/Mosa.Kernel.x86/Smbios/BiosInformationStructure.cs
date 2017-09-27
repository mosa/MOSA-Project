// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.x86.Smbios
{
	/// <summary>
	/// Bios Information Structure
	/// </summary>
	public class BiosInformationStructure : SmbiosStructure
	{
		/// <summary>
		/// The bios vendor
		/// </summary>
		private string biosVendor = string.Empty;

		/// <summary>
		/// The bios version
		/// </summary>
		private string biosVersion = string.Empty;

		/// <summary>
		/// The bios date
		/// </summary>
		private string biosDate = string.Empty;

		/// <summary>
		/// Initializes a new instance of the <see cref="BiosInformationStructure"/> class.
		/// </summary>
		public BiosInformationStructure()
			: base(SmbiosManager.GetStructureOfType(0x00))
		{
			biosVendor = GetStringFromIndex(Intrinsic.Load8(address, 0x04u));
			biosVersion = GetStringFromIndex(Intrinsic.Load8(address, 0x05u));
			biosDate = GetStringFromIndex(Intrinsic.Load8(address, 0x08u));
		}

		/// <summary>
		/// Gets the bios vendor.
		/// </summary>
		/// <value>
		/// The bios vendor.
		/// </value>
		public string BiosVendor
		{
			get { return biosVendor; }
		}

		/// <summary>
		/// Gets the bios version.
		/// </summary>
		/// <value>
		/// The bios version.
		/// </value>
		public string BiosVersion
		{
			get { return biosVersion; }
		}

		/// <summary>
		/// Gets the bios date.
		/// </summary>
		/// <value>
		/// The bios date.
		/// </value>
		public string BiosDate
		{
			get { return biosDate; }
		}
	}
}
