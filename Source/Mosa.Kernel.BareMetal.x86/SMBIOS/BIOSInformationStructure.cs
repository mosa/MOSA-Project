// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.x86.SMBIOS;

/// <summary>
/// Bios Information Structure
/// </summary>
public class BIOSInformationStructure : SMBIOSStructure
{
	/// <summary>
	/// Gets the bios vendor.
	/// </summary>
	/// <value>
	/// The bios vendor.
	/// </value>
	public string BiosVendor { get; set; }

	/// <summary>
	/// Gets the bios version.
	/// </summary>
	/// <value>
	/// The bios version.
	/// </value>
	public string BiosVersion { get; set; }

	/// <summary>
	/// Gets the bios date.
	/// </summary>
	/// <value>
	/// The bios date.
	/// </value>
	public string BiosDate { get; set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="BIOSInformationStructure"/> class.
	/// </summary>
	public BIOSInformationStructure()
		: base(SMBIOSManager.GetStructureOfType(0x00))
	{
		BiosVendor = GetStringFromIndex(Intrinsic.Load8(address, 0x04u));
		BiosVersion = GetStringFromIndex(Intrinsic.Load8(address, 0x05u));
		BiosDate = GetStringFromIndex(Intrinsic.Load8(address, 0x08u));
	}
}
