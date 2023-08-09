// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Kernel.BareMetal.x86.SMBIOS;

/// <summary>
/// Cpu Structure
/// </summary>
public class CPUStructure : SMBIOSStructure
{
	/// <summary>
	/// Gets the maximum speed.
	/// </summary>
	/// <value>
	/// The maximum speed.
	/// </value>
	public uint MaxSpeed { get; private set; }

	/// <summary>
	/// Gets the socket.
	/// </summary>
	/// <value>
	/// The socket.
	/// </value>
	public string Socket { get; private set; }

	/// <summary>
	/// Gets the vendor.
	/// </summary>
	/// <value>
	/// The vendor.
	/// </value>
	public string Vendor { get; private set; }

	/// <summary>
	/// Gets the version.
	/// </summary>
	/// <value>
	/// The version.
	/// </value>
	public string Version { get; private set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="CPUStructure"/> class.
	/// </summary>
	public CPUStructure()
		: base(SMBIOSManager.GetStructureOfType(0x04))
	{
		Version = GetStringFromIndex(address.Load8(0x10u));
		Socket = GetStringFromIndex(address.Load8(0x04u));
		MaxSpeed = address.Load16(0x16u);
		Vendor = GetStringFromIndex(address.Load8(0x07u));
	}
}
