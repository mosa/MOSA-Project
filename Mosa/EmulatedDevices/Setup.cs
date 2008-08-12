/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.EmulatedDevices
{
	public static class Setup
	{
		public static void Initialize()
		{
			new CMOS(CMOS.StandardIOBase);

			string[] files = new string[1];
			files[0] = @"..\..\..\hd.img";

			new IDEDiskDevice(IDEDiskDevice.PrimaryIOBase, files);
		}
	}
}
