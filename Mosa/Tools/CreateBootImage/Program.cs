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
using System.Collections.Generic;
using Mosa.DeviceSystem;
using Mosa.FileSystem.FAT;

namespace Mosa.Tools.CreateBootImage
{
	/// <summary>
	/// 
	/// </summary>
	class Program
	{
		enum FileSystem { FAT12, FAT16, FAT32 };
		enum ImageFormat { IMG, VHD, VDI };

		/// <summary>
		/// Main
		/// </summary>
		/// <param name="args">The args.</param>
		/// <returns></returns>
		static int Main(string[] args)
		{
			Console.WriteLine("MakeImageBoot v1.0 [www.mosa-project.org]");
			Console.WriteLine("Copyright 2009. New BSD License.");
			Console.WriteLine("Written by Philipp Garcia (phil@thinkedge.com)");
			Console.WriteLine();
			Console.WriteLine("Usage: CreateBootImage <boot.config file> <image name>");
			Console.WriteLine();
			
			string mbrFilename = string.Empty;
			string fatcodeFilename = string.Empty;
			string volumeLabel = string.Empty;
			ImageFormat imageFormat = ImageFormat.VHD;
			bool mbrOption = true;
			bool patchSyslinuxOption = false;
			uint blockCount = 1024 * 1024 / 512;
			FileSystem fileSystem = FileSystem.FAT12;
			List<IncludeFile> includeFiles = new List<IncludeFile>();

			bool valid = (args.Length < 2);

			if (valid)
				valid = System.IO.File.Exists(args[0]);

			if (valid) {
				Console.Error.WriteLine("ERROR: Missing arguments");
				return -1;
			}

			Console.WriteLine("Building image...");

			try {

				StreamReader reader = File.OpenText(args[0]);

				while (true) {
					string line = reader.ReadLine();
					if (line == null) break;

					if (string.IsNullOrEmpty(line))
						continue;

					string[] parts = line.Split('\t');

					switch (parts[0].Trim()) {
						case "-mbr": mbrOption = true; mbrFilename = (parts.Length > 1) ? parts[1] : null; break;
						case "-boot": fatcodeFilename = (parts.Length > 1) ? parts[1] : null; break;
						case "-vhd": imageFormat = ImageFormat.VHD; break;
						case "-img": imageFormat = ImageFormat.IMG; break;
						case "-vdi": imageFormat = ImageFormat.VDI; break;
						case "-syslinux": patchSyslinuxOption = true; break;
						case "-fat12": fileSystem = FileSystem.FAT12; break;
						case "-fat16": fileSystem = FileSystem.FAT16; break;
						case "-fat32": fileSystem = FileSystem.FAT32; break;
						case "-file": if (parts.Length > 2) includeFiles.Add(new IncludeFile(parts[1], parts[2]));
							else includeFiles.Add(new IncludeFile(parts[1])); break;
						case "-blocks": blockCount = Convert.ToUInt32(parts[1]); break;
						case "-volume": volumeLabel = parts[1]; break;
						default: break;
					}
				}

				reader.Close();

				if (System.IO.File.Exists(args[1]))
					System.IO.File.Delete(args[1]);

				DiskGeometry diskGeometry = new Mosa.DeviceSystem.DiskGeometry();
				diskGeometry.GuessGeometry(blockCount);

				// Create disk image file
				Mosa.EmulatedDevices.Synthetic.DiskDevice diskDevice = new Mosa.EmulatedDevices.Synthetic.DiskDevice(args[1]);

				if (imageFormat == ImageFormat.VDI) {
					// Create header
					byte[] header = Mosa.DeviceSystem.VDI.CreateHeader(
						blockCount,
						Guid.NewGuid().ToByteArray(),
						Guid.NewGuid().ToByteArray(),
						diskGeometry
					);

					diskDevice.WriteBlock(0, 1, header);

					byte[] map = Mosa.DeviceSystem.VDI.CreateImageMap(blockCount);

					diskDevice.WriteBlock(1, (uint)(map.Length / 512), map);

					diskDevice.BlockOffset = 1 + (uint)(map.Length / 512);
				}

				// Expand disk image
				diskDevice.WriteBlock(blockCount - 1, 1, new byte[512]);

				// Create partition device
				PartitionDevice partitionDevice;

				if (mbrOption) {
					// Create master boot block record
					MasterBootBlock mbr = new MasterBootBlock(diskDevice);

					// Setup partition entry					
					mbr.DiskSignature = 0x12345678;
					mbr.Partitions[0].Bootable = true;
					mbr.Partitions[0].StartLBA = diskGeometry.SectorsPerTrack;
					mbr.Partitions[0].TotalBlocks = blockCount - mbr.Partitions[0].StartLBA;

					switch (fileSystem) {
						case FileSystem.FAT12: mbr.Partitions[0].PartitionType = PartitionType.FAT12; break;
						case FileSystem.FAT16: mbr.Partitions[0].PartitionType = PartitionType.FAT16; break;
						case FileSystem.FAT32: mbr.Partitions[0].PartitionType = PartitionType.FAT32; break;
						default: break;
					}

					if (!string.IsNullOrEmpty(mbrFilename))
						mbr.Code = ReadFile(mbrFilename);

					mbr.Write();

					partitionDevice = new PartitionDevice(diskDevice, mbr.Partitions[0], false);
				}
				else {
					partitionDevice = new PartitionDevice(diskDevice, false);
				}

				// Set FAT settings
				FatSettings fatSettings = new FatSettings();

				switch (fileSystem) {
					case FileSystem.FAT12: fatSettings.FATType = FatType.FAT12; break;
					case FileSystem.FAT16: fatSettings.FATType = FatType.FAT16; break;
					case FileSystem.FAT32: fatSettings.FATType = FatType.FAT32; break;
					default: break;
				}

				fatSettings.FloppyMedia = false;
				fatSettings.VolumeLabel = volumeLabel;
				fatSettings.SerialID = new byte[4] { 0x01, 0x02, 0x03, 0x04 };
				fatSettings.SectorsPerTrack = diskGeometry.SectorsPerTrack;
				fatSettings.NumberOfHeads = diskGeometry.Heads;
				fatSettings.HiddenSectors = diskGeometry.SectorsPerTrack;
				if (!string.IsNullOrEmpty(fatcodeFilename))
					fatSettings.OSBootCode = ReadFile(fatcodeFilename);
				fatSettings.FloppyMedia = false;

				// Create FAT file system
				FatFileSystem fat = new FatFileSystem(partitionDevice);
				fat.Format(fatSettings);

				fat.SetVolumeName(volumeLabel);

				foreach (IncludeFile includeFile in includeFiles) {
					string filename = includeFile.Filename;

					Mosa.FileSystem.FAT.FatFileAttributes fileAttributes = new Mosa.FileSystem.FAT.FatFileAttributes();
					if (includeFile.Archive) fileAttributes |= Mosa.FileSystem.FAT.FatFileAttributes.Archive;
					if (includeFile.ReadOnly) fileAttributes |= Mosa.FileSystem.FAT.FatFileAttributes.ReadOnly;
					if (includeFile.Hidden) fileAttributes |= Mosa.FileSystem.FAT.FatFileAttributes.Hidden;
					if (includeFile.System) fileAttributes |= Mosa.FileSystem.FAT.FatFileAttributes.System;

					byte[] file = ReadFile(filename);
					string newname = (Path.GetFileNameWithoutExtension(includeFile.Newname).PadRight(8).Substring(0, 8) + Path.GetExtension(includeFile.Newname).PadRight(3).Substring(1, 3)).ToUpper();
					FatFileLocation location = fat.CreateFile(newname, fileAttributes, 0);

					if (!location.Valid)
						throw new Exception("Unable to write file");

					FatFileStream fatFileStream = new FatFileStream(fat, location);
					fatFileStream.Write(file, 0, file.Length);
					fatFileStream.Flush();
				}

				if (patchSyslinuxOption) {
					// Locate ldlinux.sys file for patching
					string filename = "ldlinux.sys";
					string name = (Path.GetFileNameWithoutExtension(filename) + Path.GetExtension(filename).PadRight(3).Substring(0, 4)).ToUpper();

					FatFileLocation location = fat.FindEntry(new Mosa.FileSystem.FAT.Find.WithName(name), 0);

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
							FatFileStream file = new FatFileStream(fat, location);

							uint csum = 0x3EB202FE;
							for (uint index = 0; index < (file.Length >> 2); index++) {
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

				if (imageFormat == ImageFormat.VHD) {
					// Create footer
					byte[] footer = Mosa.DeviceSystem.VHD.CreateFooter(
						blockCount,
						(uint)(DateTime.Now - (new DateTime(2000, 1, 1, 0, 0, 0))).Seconds,
						Guid.NewGuid().ToByteArray(),
						diskGeometry
					);

					diskDevice.WriteBlock(blockCount, 1, footer);
				}

				Console.WriteLine("Completed!");
			}
			catch (Exception e) {
				Console.Error.WriteLine("Error: " + e.ToString());
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

		/// <summary>
		/// 
		/// </summary>
		class IncludeFile
		{
			public string Filename;
			public string Newname;
			public bool ReadOnly = false;
			public bool Hidden = false;
			public bool Archive = true;
			public bool System = false;

			/// <summary>
			/// Initializes a new instance of the <see cref="IncludeFile"/> class.
			/// </summary>
			/// <param name="filename">The filename.</param>
			public IncludeFile(string filename)
			{
				Filename = filename;

				Newname = filename.Replace('\\', '/');

				int at = Newname.LastIndexOf('/');

				if (at > 0)
					Newname = Newname.Substring(at + 1, Newname.Length - at - 1);
			}

			/// <summary>
			/// Initializes a new instance of the <see cref="IncludeFile"/> class.
			/// </summary>
			/// <param name="filename">The filename.</param>
			public IncludeFile(string filename, string newname)
			{
				Filename = filename;
				Newname = newname;
			}
		}

	}
}
