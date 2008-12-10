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
using Mosa.DeviceSystem;
using Mosa.FileSystem.FATFileSystem;

namespace Mosa.Tools.MakeFAT
{
	class Program
	{

		static int Main(string[] args)
		{
			string imageFilename = string.Empty;
			string mbrFilename = string.Empty;
			string fatcodeFilename = string.Empty;
			uint blockCount = 1024 * 1024 * 4 / 512;

			// TODO: Parse command line parameters

			imageFilename = @"x:\boot\bootimage.img";
			mbrFilename = @"x:\boot\freedos-mbr.bin";
			fatcodeFilename = @"x:\boot\freedos-boot.bin";

			//if (args.Length < 3) {
			//    Console.WriteLine("ERROR: Missing arguments");
			//    Console.WriteLine("MakeFAT [-mbr <master boot record>] <number of blocks>  <destination img file>");
			//    return -1;
			//}

			try {
				byte[] mbrCode = ReadFile(mbrFilename);
				byte[] fatCode = ReadFile(fatcodeFilename);

				// Create disk image file
				Mosa.EmulatedDevices.Synthetic.DiskDevice diskDevice = new Mosa.EmulatedDevices.Synthetic.DiskDevice(imageFilename);

				// Expand disk image
				diskDevice.WriteBlock(blockCount - 1, 1, new byte[512]);

				// Create master boot block record
				MasterBootBlock mbr = new MasterBootBlock(diskDevice);

				mbr.DiskSignature = 0x12345678;
				mbr.Partitions[0].Bootable = true;
				mbr.Partitions[0].StartLBA = 1;
				mbr.Partitions[0].TotalBlocks = blockCount - 1;
				mbr.Partitions[0].PartitionType = PartitionType.Fat12;
				mbr.Code = mbrCode;

				mbr.Write();

				// Open partition within image File
				PartitionDevice partitionDevice = new PartitionDevice(diskDevice, mbr.Partitions[0], false);

				// Set FAT settings
				FATSettings fatSettings = new FATSettings();

				fatSettings.FATType = FATType.FAT16;
				fatSettings.FloppyMedia = false;
				fatSettings.VolumeLabel = "MOSA BOOT";
				fatSettings.SerialID = new byte[4] { 0x01, 0x02, 0x03, 0x04 };
				fatSettings.OSBootCode = fatCode;

				// Create FAT file system
				FAT fat = new FAT(partitionDevice);

				fat.Format(fatSettings);

			}
			catch (Exception e) {
				Console.WriteLine("Error: " + e.ToString());
				return -1;
			}

			return 0;
		}

		static private byte[] ReadFile(string filename)
		{
			FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read);
			byte[] data = new byte[fileStream.Length];
			fileStream.Read(data, 0, data.Length);
			fileStream.Close();
			return data;
		}
	}
}
