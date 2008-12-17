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
		/// <summary>
		/// Main
		/// </summary>
		/// <param name="args">The args.</param>
		/// <returns></returns>
		static int Main(string[] args)
		{
			string imageFilename = string.Empty;
			string mbrFilename = string.Empty;
			string fatcodeFilename = string.Empty;
			string[] copyFiles;
			bool vhd = true;

			uint blockCount = 1024 * 1024 * 4 / 512;

			// TODO: Parse command line parameters

			copyFiles = new string[2];
			imageFilename = @"x:\boot\bootimage.vhd";
			mbrFilename = @"x:\boot\freedos-mbr.bin";
			fatcodeFilename = @"x:\boot\freedos-boot.bin";
			copyFiles[0] = @"X:\Boot\Boot\KERNEL.SYS";
			copyFiles[1] = @"X:\Boot\Boot\COMMAND.COM";

			//if (args.Length < 3) {
			//    Console.WriteLine("ERROR: Missing arguments");
			//    Console.WriteLine("MakeFAT [-mbr <master boot record>] <number of blocks>  <destination img file>");
			//    return -1;
			//}

			try {
				System.IO.File.Delete(imageFilename);

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
				mbr.Partitions[0].StartLBA = 17;
				mbr.Partitions[0].TotalBlocks = blockCount - 17 - 32;
				mbr.Partitions[0].PartitionType = PartitionType.FAT12;
				mbr.Code = mbrCode;

				mbr.Write();

				// Open partition within image file
				PartitionDevice partitionDevice = new PartitionDevice(diskDevice, mbr.Partitions[0], false);

				// Set FAT settings
				FATSettings fatSettings = new FATSettings();

				fatSettings.FATType = FATType.FAT12;
				fatSettings.FloppyMedia = false;
				fatSettings.VolumeLabel = "MOSABOOT";
				fatSettings.SerialID = new byte[4] { 0x01, 0x02, 0x03, 0x04 };
				fatSettings.OSBootCode = fatCode;

				// Create FAT file system
				FAT fat = new FAT(partitionDevice);
				fat.Format(fatSettings);

				fat.SetVolumeName("MOSABOOT");

				foreach (string filename in copyFiles)
					if (filename != null) {
						byte[] file = ReadFile(filename);
						string name = Path.GetFileNameWithoutExtension(filename).PadRight(8).Substring(0, 8) + Path.GetExtension(filename).PadRight(3).Substring(1, 3);
						DirectoryEntryLocation location = fat.CreateFile(name, 0);
						FATFileStream fatFileStream = new FATFileStream(fat, location);
						fatFileStream.Write(file, 0, file.Length);
						fatFileStream.Flush();
					}

				if (vhd) {

					// Create footer
					byte[] footer = Mosa.DeviceSystem.VHD.CreateFooter(
						blockCount * 512,
						(uint)(DateTime.Now - (new DateTime(2000, 1, 1, 0, 0, 0))).Seconds,
						(new Guid()).ToByteArray(),
						new Mosa.DeviceSystem.DiskGeometry(0x78, 4, 17)
					);

					diskDevice.WriteBlock(blockCount, 1, footer);
				}
			}
			catch (Exception e) {
				Console.WriteLine("Error: " + e.ToString());
				return -1;
			}

			return 0;
		}

		/// <summary>
		/// Reads the file.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <returns></returns>
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
