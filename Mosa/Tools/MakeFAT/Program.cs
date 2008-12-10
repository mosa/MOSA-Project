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
			string filename = string.Empty;
			uint blockCount = 0;

			// TODO: Parse command line parameters

			if (args.Length < 3) {
				Console.WriteLine("ERROR: Missing arguments");
				Console.WriteLine("MakeFAT [-mbr <master boot record>] <number of blocks>  <destination img file>");
				return -1;
			}

			try {
				// Create disk image file
				Mosa.EmulatedDevices.Synthetic.DiskDevice diskDevice = new Mosa.EmulatedDevices.Synthetic.DiskDevice(filename);

				// Expand disk image
				diskDevice.WriteBlock(blockCount - 1, 1, new byte[512]);

				// Create master boot block record
				MasterBootBlock mbr = new MasterBootBlock(diskDevice);

				mbr.DiskSignature = 0x12345678;
				mbr.Partitions[0].Bootable = true;
				mbr.Partitions[0].StartLBA = 1;
				mbr.Partitions[0].TotalBlocks = blockCount - 1;
				mbr.Partitions[0].PartitionType = PartitionType.Fat12;

				// TODO: mbr.Code = < something >;

				mbr.Write();

				// Open partition within image File
				PartitionDevice partitionDevice = new PartitionDevice(diskDevice, mbr.Partitions[0], false);

				// Set FAT settings
				FATSettings fatSettings = new FATSettings();

				fatSettings.FATType = FATType.FAT16;
				fatSettings.FloppyMedia = false;
				fatSettings.VolumeLabel = "MOSA BOOT";
				fatSettings.SerialID = new byte[4] { 0x01, 0x02, 0x03, 0x04 };
				
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
	}
}
