// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.PCI;

/// <summary>
/// Provides a table associating PCI device class codes with names.
/// </summary>
public static class ClassCodeTable
{
	public static string Lookup(byte classCode) => classCode switch
	{
		0x00 => "Pre PCI 2.0 device",
		0x01 => "Mass storage controller",
		0x02 => "Network controller",
		0x03 => "Display controller",
		0x04 => "Multimedia device",
		0x05 => "Memory Controller",
		0x06 => "Bridge Device",
		0x07 => "Simple communications controller",
		0x08 => "Base system peripheral",
		0x09 => "Input device",
		0x0A => "Docking Station",
		0x0B => "Processor",
		0x0C => "Serial bus controller",
		0x0D => "Wireless controller",
		0x0E => "Intelligent IO controller",
		0x0F => "Satellite communications controller",
		0x10 => "Encryption/Decryption controller",
		0x11 => "Signal processing controller",
		0xFF => "Misc",
		_ => classCode is >= 0x0D and <= 0xFE ? "Reserved" : string.Empty
	};
}
