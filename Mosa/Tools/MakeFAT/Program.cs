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
			Mosa.FileSystem.FATFileSystem.FileAttributes[] fileAttributes;
			bool vhd = true;
			bool patchSyslinux = true;

			uint blockCount = 1024 * 1024 * 3 / 512;

			// TODO: Parse command line parameters

			copyFiles = new string[5];
			fileAttributes = new Mosa.FileSystem.FATFileSystem.FileAttributes[5];

			imageFilename = @"x:\boot\bootimage.vhd";
			//mbrFilename = @"x:\boot\freedos\freedos-mbr.bin";
			//fatcodeFilename = @"x:\boot\freedos\freedos-boot.bin";
			//copyFiles[0] = @"X:\Boot\freedos\KERNEL.SYS";
			//copyFiles[1] = @"X:\Boot\freedos\COMMAND.COM";
			fileAttributes[0] = Mosa.FileSystem.FATFileSystem.FileAttributes.Archive;
			fileAttributes[1] = Mosa.FileSystem.FATFileSystem.FileAttributes.Archive;

			mbrFilename = @"x:\boot\syslinux\syslinux-mbr.bin";
			fatcodeFilename = @"x:\boot\syslinux\syslinux-boot.bin";
			copyFiles[0] = @"X:\Boot\syslinux\hello.exe";
			copyFiles[1] = @"X:\Boot\syslinux\mboot.c32";
			copyFiles[2] = @"X:\Boot\syslinux\syslinux.cfg";
			copyFiles[3] = @"X:\Boot\syslinux\ldlinux.sys";
			fileAttributes[0] = Mosa.FileSystem.FATFileSystem.FileAttributes.Archive;
			fileAttributes[1] = Mosa.FileSystem.FATFileSystem.FileAttributes.Archive;
			fileAttributes[2] = Mosa.FileSystem.FATFileSystem.FileAttributes.Archive;
			fileAttributes[3] = Mosa.FileSystem.FATFileSystem.FileAttributes.Archive | Mosa.FileSystem.FATFileSystem.FileAttributes.Hidden | Mosa.FileSystem.FATFileSystem.FileAttributes.ReadOnly | Mosa.FileSystem.FATFileSystem.FileAttributes.System;

			//if (args.Length < 3) {
			//    Console.WriteLine("ERROR: Missing arguments");
			//    Console.WriteLine("MakeFAT [-mbr <master boot record>] <number of blocks> <destination img file>");
			//    return -1;
			//}

			try {
				if (System.IO.File.Exists(imageFilename))
					System.IO.File.Delete(imageFilename);

				byte[] mbrCode = ReadFile(mbrFilename);
				byte[] fatCode = ReadFile(fatcodeFilename);

				DiskGeometry diskGeometry = new Mosa.DeviceSystem.DiskGeometry();
				diskGeometry.GuessGeometry(blockCount);

				// Create disk image file
				Mosa.EmulatedDevices.Synthetic.DiskDevice diskDevice = new Mosa.EmulatedDevices.Synthetic.DiskDevice(imageFilename);

				// Expand disk image
				diskDevice.WriteBlock(blockCount - 1, 1, new byte[512]);

				// Create master boot block record
				MasterBootBlock mbr = new MasterBootBlock(diskDevice);

				mbr.DiskSignature = 0x12345678;
				mbr.Partitions[0].Bootable = true;
				mbr.Partitions[0].StartLBA = diskGeometry.SectorsPerTrack;
				mbr.Partitions[0].TotalBlocks = blockCount - mbr.Partitions[0].StartLBA;
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
				fatSettings.SectorsPerTrack = diskGeometry.SectorsPerTrack;
				fatSettings.NumberOfHeads = diskGeometry.Heads;
				fatSettings.HiddenSectors = diskGeometry.SectorsPerTrack;
				fatSettings.FloppyMedia = false;

				// Create FAT file system
				FAT fat = new FAT(partitionDevice);
				fat.Format(fatSettings);

				fat.SetVolumeName("MOSABOOT");

				for (uint index = 0; index < copyFiles.Length; index++) {
					string filename = copyFiles[index];

					if (filename != null) {
						byte[] file = ReadFile(filename);
						string name = (Path.GetFileNameWithoutExtension(filename).PadRight(8).Substring(0, 8) + Path.GetExtension(filename).PadRight(3).Substring(1, 3)).ToUpper();
						DirectoryEntryLocation location = fat.CreateFile(name, fileAttributes[index], 0);
						FATFileStream fatFileStream = new FATFileStream(fat, location);
						fatFileStream.Write(file, 0, file.Length);
						fatFileStream.Flush();
					}
				}

				if (patchSyslinux) {
					// Locate ldlinux.sys file for patching

					string filename = "ldlinux.sys";
					string name = (Path.GetFileNameWithoutExtension(filename) + Path.GetExtension(filename).PadRight(3).Substring(0, 4)).ToUpper();

					DirectoryEntryLocation location = fat.FindEntry(new Mosa.FileSystem.FATFileSystem.Find.WithName(name), 0);

					if (location.Valid) {
						// Read boot sector
						Mosa.ClassLib.BinaryFormat bootSector = new Mosa.ClassLib.BinaryFormat(partitionDevice.ReadBlock(0, 1));

						// Set the first sector location of the file
						bootSector.SetUInt(0x1F8, fat.GetSectorByCluster(location.FirstCluster));

						// Change jump address
						bootSector.SetUInt(0x01, 0x58);

						// Write back patched boot sector
						partitionDevice.WriteBlock(0, 1, bootSector.Data);

						// Get the file size & number of sectors used
						uint fileSize = fat.GetFileSize(location.DirectorySector, location.DirectorySectorIndex);
						uint sectorCount = (fileSize + 511) >> 9;

						uint[] sectors = new uint[65];
						uint nsec = 0;

						// Create list of the first 65 sectors of the file
						for (uint cluster = location.FirstCluster; ((cluster != 0) & (nsec <= 64)); cluster = fat.GetNextCluster(cluster)) {
							uint sec = fat.GetSectorByCluster(cluster);
							for (uint s = 0; s < fat.SectorsPerCluster; s++)
								sectors[nsec++] = sec + s;
						}

						// Read the first cluster of the file
						Mosa.ClassLib.BinaryFormat firstCluster = new Mosa.ClassLib.BinaryFormat(fat.ReadCluster(location.FirstCluster));

						uint patchArea = 0;

						// Search for 0x3EB202FE (magic)
						for (patchArea = 0; (firstCluster.GetUInt(patchArea) != 0x3EB202FE) && (patchArea < fat.ClusterSizeInBytes); patchArea += 4) ;

						patchArea = patchArea + 8;

						if (patchArea < fat.ClusterSizeInBytes) {
							// Set up the totals 
							firstCluster.SetUShort(patchArea, (ushort)(fileSize >> 2));
							firstCluster.SetUShort(patchArea + 2, (ushort)(sectorCount - 1));

							// Clear sector entries
							firstCluster.Fill(patchArea + 8, 0, 64 * 4);

							// Set sector entries
							for (nsec = 0; nsec < 64; nsec++)
								firstCluster.SetUInt(patchArea + 8 + (nsec * 4), sectors[nsec + 1]);

							// Clear out checksum
							firstCluster.SetUInt(patchArea + 4, 0);

							// Write back the updated cluster
							fat.WriteCluster(location.FirstCluster, firstCluster.Data);

							// Re-Calculate checksum by openning the file
							FATFileStream file = new FATFileStream(fat, location);

							uint csum = 0x3EB202FE;
							for (uint i = 0; i < file.Length >> 2; i++) {
								uint value = (uint)file.ReadByte() | ((uint)file.ReadByte() << 8) | ((uint)file.ReadByte() << 16) | ((uint)file.ReadByte() << 24);
								csum -= value;
							}

							// Set the checksum
							firstCluster.SetUInt(patchArea + 4, csum);

							// Write patched cluster back to disk
							fat.WriteCluster(location.FirstCluster, firstCluster.Data);
						}

					}

				}

				if (vhd) {

					// Create footer
					byte[] footer = Mosa.DeviceSystem.VHD.CreateFooter(
						blockCount * 512,
						(uint)(DateTime.Now - (new DateTime(2000, 1, 1, 0, 0, 0))).Seconds,
						(new Guid()).ToByteArray(),
						diskGeometry
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
