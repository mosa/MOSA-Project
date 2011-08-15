/*
* (c) 2008 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

namespace Mosa.DeviceSystem.PCI
{
	/// <summary>
	/// 
	/// </summary>
	public static class ClassCodeTable
	{
		/// <summary>
		/// Looks up the specified class code.
		/// </summary>
		/// <param name="classCode">The class code.</param>
		/// <returns></returns>
		public static string Lookup(byte classCode)
		{
			switch (classCode)
			{
				case 0x00: return "Pre PCI 2.0 device";
				case 0x01: return "Mass storage controller";
				case 0x02: return "Network controller";
				case 0x03: return "Display controller";
				case 0x04: return "Multimedia device";
				case 0x05: return "Memory Controller";
				case 0x06: return "Bridge Device";
				case 0x07: return "Simple communications controller";
				case 0x08: return "Base system peripheral";
				case 0x09: return "Inupt device";
				case 0x0A: return "Docking Station";
				case 0x0B: return "Processort";
				case 0x0C: return "Serial bus controller";
				case 0x0D: return "Wireless controller";
				case 0x0E: return "Intelligent IO controller";
				case 0x0F: return "Satellite communications controller";
				case 0x10: return "Encryption/Decryption controller";
				case 0x11: return "Signal processing controller";
				case 0xFF: return "Misc";
				default:
					if ((classCode >= 0x0D) && (classCode <= 0xFE))
						return "Reserved";
					else
						return string.Empty;
			}
		}
	}
}
