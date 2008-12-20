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
using Mosa.FileSystem.FATFileSystem;

namespace Mosa.Tools.MakeBoot
{
	/// <summary>
	/// 
	/// </summary>
	class Program
	{
		enum FileSystem { FAT12, FAT16 };
		enum ImageFormat { Raw, VHD, VDI };

		/// <summary>
		/// Main
		/// </summary>
		/// <param name="args">The args.</param>
		/// <returns></returns>
		static int Main(string[] args)
		{
			Console.WriteLine("MakeBoot v0.1 [www.mosa-project.org]");
			Console.WriteLine("Copyright 2008. New BSD License.");
			Console.WriteLine("Written by Philipp Garcia (phil@thinkedge.com)");
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
				Console.WriteLine("MakeBoot <boot.config> <image name>");
				Console.WriteLine("ERROR: Missing arguments");
				return -1;
			}

			Console.WriteLine("Building image...");

			try {

				StreamReader reader = File.OpenText(args[0]);
				string line = reader.ReadLine();

				while (true) {
					line = reader.ReadLine();
					if (line == null) break;

					if (string.IsNullOrEmpty(line))
						continue;

					string[] parts = line.Split('\t');

					switch (parts[0].Trim()) {
						case "-mbr": mbrOption = true; mbrFilename = (parts.Length > 1) ? parts[1] : null; break;
						case "-boot": fatcodeFilename = (parts.Length > 1) ? parts[1] : null; break;
						case "-vhd": imageFormat = ImageFormat.VHD; break;
						case "-raw": imageFormat = ImageFormat.Raw; break;
						case "-vdi": imageFormat = ImageFormat.VDI; break;
						case "-syslinux": patchSyslinuxOption = true; break;
						case "-fat12": fileSystem = FileSystem.FAT12; break;
						case "-fat16": fileSystem = FileSystem.FAT16; break;
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

				GenericPartition partition = new GenericPartition(0);

				partition.Bootable = true;
				partition.StartLBA = diskGeometry.SectorsPerTrack;
				partition.TotalBlocks = blockCount - partition.StartLBA;
				partition.PartitionType = (fileSystem == FileSystem.FAT12) ? PartitionType.FAT12 : PartitionType.FAT16;

				if (mbrOption) {
					// Create master boot block record
					MasterBootBlock mbr = new MasterBootBlock(diskDevice);

					mbr.DiskSignature = 0x12345678;
					mbr.Partitions[0] = partition;

					if (!string.IsNullOrEmpty(mbrFilename))
						mbr.Code = ReadFile(mbrFilename);

					mbr.Write();

					partition = mbr.Partitions[0];
				}

				// Open partition within image file
				PartitionDevice partitionDevice = new PartitionDevice(diskDevice, partition, false);

				// Set FAT settings
				FATSettings fatSettings = new FATSettings();

				fatSettings.FATType = (fileSystem == FileSystem.FAT12) ? FATType.FAT12 : FATType.FAT16;
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
				FAT fat = new FAT(partitionDevice);
				fat.Format(fatSettings);

				fat.SetVolumeName(volumeLabel);

				foreach (IncludeFile includeFile in includeFiles) {
					string filename = includeFile.Filename;

					Mosa.FileSystem.FATFileSystem.FileAttributes fileAttributes = new Mosa.FileSystem.FATFileSystem.FileAttributes();
					if (includeFile.Archive) fileAttributes |= Mosa.FileSystem.FATFileSystem.FileAttributes.Archive;
					if (includeFile.ReadOnly) fileAttributes |= Mosa.FileSystem.FATFileSystem.FileAttributes.ReadOnly;
					if (includeFile.Hidden) fileAttributes |= Mosa.FileSystem.FATFileSystem.FileAttributes.Hidden;
					if (includeFile.System) fileAttributes |= Mosa.FileSystem.FATFileSystem.FileAttributes.System;

					if (filename != null) {
						byte[] file = ReadFile(filename);
						string newname = (Path.GetFileNameWithoutExtension(includeFile.Newname).PadRight(8).Substring(0, 8) + Path.GetExtension(includeFile.Newname).PadRight(3).Substring(1, 3)).ToUpper();
						DirectoryEntryLocation location = fat.CreateFile(newname, fileAttributes, 0);
						FATFileStream fatFileStream = new FATFileStream(fat, location);
						fatFileStream.Write(file, 0, file.Length);
						fatFileStream.Flush();
					}
				}

				if (patchSyslinuxOption) {
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
