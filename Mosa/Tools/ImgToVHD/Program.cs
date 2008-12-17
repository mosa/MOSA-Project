/*
* (c) 2008 MOSA - The Managed Operating System Alliance
*
* Licensed under the terms of the New BSD License.
*
* Authors:
*  Phil Garcia (tgiphil) <phil@thinkedge.com>
*/

using System;
using System.IO;

namespace Mosa.Tools.ImgToVHD
{
	class Program
	{
		static int Main(string[] args)
		{
			if (args.Length < 2) {
				Console.WriteLine("ERROR: Missing arguments");
				Console.WriteLine("ImgToVHD <source img file> <destination vhd file>");
				return -1;
			}

			try {
				// Copy file - the first part of an IMG file is the same within the Fixed VHD file
				File.Copy(args[0], args[1], true);

				// File Stream
				FileStream fileStream = new FileStream(args[1], FileMode.Append, FileAccess.Write);

				ulong size = (ulong)fileStream.Length;

				fileStream.Seek(0, SeekOrigin.End);

				// Pad to 512 byte block
				for (int index = (int)Mosa.DeviceSystem.VHD.GetAlignmentPadding(size); index > 0; index--)
					fileStream.WriteByte(0);

				// Create footer
				byte[] footer = Mosa.DeviceSystem.VHD.CreateFooter(
					size,
					(uint)(DateTime.Now - (new DateTime(2000, 1, 1, 0, 0, 0))).Seconds,
					(new Guid()).ToByteArray(),
					new Mosa.DeviceSystem.DiskGeometry(0x78, 4, 17)
				);

				// Added footer
				fileStream.Write(footer, 0, 512);

				fileStream.Close();
			}
			catch (Exception e) {
				Console.WriteLine("Error: " + e.ToString());
				return -1;
			}

			return 0;
		}
	}
}
