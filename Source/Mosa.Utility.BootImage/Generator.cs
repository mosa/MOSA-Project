// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using Mosa.FileSystem.FAT;
using System;
using System.IO;
using System.Collections.Generic;
using Mosa.ClassLib;
using Mosa.Compiler.Common;

//using Mosa.Compiler.Common;

namespace Mosa.Utility.BootImage
{
	/// <summary>
	///
	/// </summary>
	public class Generator
	{
		/// <summary>
		/// Creates the specified options.
		/// </summary>
		/// <param name="options">The options.</param>
		static public void Create(Options options)
		{
			if (File.Exists(options.DiskImageFileName))
			{
				File.Delete(options.DiskImageFileName);
			}

			uint blockCount = options.BlockCount;

			if (blockCount == 0)
			{
				blockCount = 8400 + 1;
				foreach (var file in options.IncludeFiles)
				{
					blockCount += ((uint)file.Content.Length / SectorSize) + 1;
				}
			}

			var diskGeometry = new DiskGeometry();
			diskGeometry.GuessGeometry(blockCount);

			// Create disk image file
			var diskDevice = new BlockFileStream(options.DiskImageFileName);

			if (options.ImageFormat == ImageFormatType.VDI)
			{
				// Create header
				byte[] header = VDI.CreateHeader(
					blockCount,
					options.MediaGuid.ToByteArray(),
					options.MediaLastSnapGuid.ToByteArray(),
					diskGeometry
				);

				diskDevice.WriteBlock(0, 1, header);

				byte[] map = VDI.CreateImageMap(blockCount);

				diskDevice.WriteBlock(1, (uint)(map.Length / SectorSize), map);

				diskDevice.BlockOffset = 1 + (uint)(map.Length / 512);
			}

			// Expand disk image
			diskDevice.WriteBlock(blockCount - 1, 1, new byte[SectorSize]);

			// Create partition device
			PartitionDevice partitionDevice;

			if (options.MBROption)
			{
				// Create master boot block record
				var mbr = new MasterBootBlock(diskDevice);

				// Setup partition entry
				mbr.DiskSignature = 0x12345678;
				mbr.Partitions[0].Bootable = true;
				mbr.Partitions[0].StartLBA = diskGeometry.SectorsPerTrack;
				mbr.Partitions[0].TotalBlocks = blockCount - mbr.Partitions[0].StartLBA;

				switch (options.FileSystem)
				{
					case FileSystemType.FAT12: mbr.Partitions[0].PartitionType = PartitionType.FAT12; break;
					case FileSystemType.FAT16: mbr.Partitions[0].PartitionType = PartitionType.FAT16; break;
					case FileSystemType.FAT32: mbr.Partitions[0].PartitionType = PartitionType.FAT32; break;
					default: break;
				}

				mbr.Code = options.MBRCode;

				mbr.Write();

				partitionDevice = new PartitionDevice(diskDevice, mbr.Partitions[0], false);
			}
			else
			{
				partitionDevice = new PartitionDevice(diskDevice, false);
			}

			// Set FAT settings
			var fatSettings = new FatSettings();

			switch (options.FileSystem)
			{
				case FileSystemType.FAT12: fatSettings.FATType = FatType.FAT12; break;
				case FileSystemType.FAT16: fatSettings.FATType = FatType.FAT16; break;
				case FileSystemType.FAT32: fatSettings.FATType = FatType.FAT32; break;
				default: break;
			}

			fatSettings.FloppyMedia = false;
			fatSettings.VolumeLabel = options.VolumeLabel;
			fatSettings.SerialID = new byte[4] { 0x01, 0x02, 0x03, 0x04 };
			fatSettings.SectorsPerTrack = diskGeometry.SectorsPerTrack;
			fatSettings.NumberOfHeads = diskGeometry.Heads;
			fatSettings.HiddenSectors = diskGeometry.SectorsPerTrack;
			fatSettings.OSBootCode = options.FatBootCode;

			// Create FAT file system
			var fat = new FatFileSystem(partitionDevice);

			if (!fat.Format(fatSettings))
			{
				throw new Exception("ERROR: Invalid FAT settings");
			}

			fat.SetVolumeName(options.VolumeLabel);

			foreach (var includeFile in options.IncludeFiles)
			{
				var fileAttributes = new FatFileAttributes();
				if (includeFile.Archive) fileAttributes |= FatFileAttributes.Archive;
				if (includeFile.ReadOnly) fileAttributes |= FatFileAttributes.ReadOnly;
				if (includeFile.Hidden) fileAttributes |= FatFileAttributes.Hidden;
				if (includeFile.System) fileAttributes |= FatFileAttributes.System;

				string newname = (Path.GetFileNameWithoutExtension(includeFile.Filename).PadRight(8).Substring(0, 8) + Path.GetExtension(includeFile.Filename).PadRight(4).Substring(1, 3)).ToUpper();
				var location = fat.CreateFile(newname, fileAttributes, 0);

				if (!location.IsValid)
					throw new Exception("Unable to write file");

				var fatFileStream = new FatFileStream(fat, location);
				fatFileStream.Write(includeFile.Content, 0, includeFile.Content.Length);
				fatFileStream.Flush();
			}

			if (options.PatchSyslinuxOption)
			{
				// Locate ldlinux.sys file for patching
				string filename = "ldlinux.sys";
				string name = (Path.GetFileNameWithoutExtension(filename) + Path.GetExtension(filename).PadRight(4).Substring(0, 4)).ToUpper();

				var location = fat.FindEntry(new Mosa.FileSystem.FAT.Find.WithName(name), 0);

				if (!location.IsValid)
					throw new InvalidProgramException("Unable to find syslinux.sys");

				// Get the file size & number of sectors used
				uint fileSize = fat.GetFileSize(location.DirectorySector, location.DirectorySectorIndex);

				var sectors = new List<uint>();

				// Create list of the sectors of the file
				for (uint cluster = location.FirstCluster; (cluster != 0); cluster = fat.GetNextCluster(cluster))
				{
					uint sec = fat.GetSectorByCluster(cluster);
					for (uint i = 0; i < fat.SectorsPerCluster; i++)
					{
						sectors.Add(sec + i);
					}
				}

				// Get the ldlinux.sys file stream
				var ldlinux = new FatFileStream(fat, location);

				var ldlinuxReader = new EndianAwareBinaryReader(ldlinux, Endianness.Little);

				// Search for 0x3EB202FE (magic)
				while ((ldlinuxReader.ReadUInt32() != Syslinux.LDLINUX_MAGIC) && (ldlinux.Position < ldlinux.Length)) ;

				if (ldlinux.Position >= ldlinux.Length || ldlinux.Position <= 0)
					throw new InvalidProgramException("Unable to find patch location for syslinux");

				uint patchArea = (uint)ldlinux.Position - 4;

				// Get Extended Patch Area offset
				ldlinux.Position = patchArea + Syslinux.PatchAreaOffset.EPAOffset;
				ushort epa = ldlinuxReader.ReadUInt16();

				ldlinux.Position = epa + Syslinux.ExtendedPatchAreaOffset.Sect1Ptr0;
				uint sect1Ptr0 = (uint)ldlinuxReader.ReadUInt16();

				ldlinux.Position = epa + Syslinux.ExtendedPatchAreaOffset.Sect1Ptr1;
				uint sect1Ptr1 = (uint)ldlinuxReader.ReadUInt16();

				ldlinux.Position = epa + Syslinux.ExtendedPatchAreaOffset.SecPtrOffset;
				uint ex = (uint)ldlinuxReader.ReadUInt16();

				ldlinux.Position = epa + Syslinux.ExtendedPatchAreaOffset.SecPtrCnt;
				uint nptrs = (uint)ldlinuxReader.ReadUInt16();

				ldlinux.Position = epa + Syslinux.ExtendedPatchAreaOffset.AdvPtrOffset;
				uint advptrs = (uint)ldlinuxReader.ReadUInt16();

				if (sectors.Count > nptrs)
					throw new InvalidProgramException("Insufficient space for patching syslinux");

				var ldlinuxWriter = new EndianAwareBinaryWriter(ldlinux, Endianness.Little);

				// Set up the totals
				ldlinux.Position = patchArea + Syslinux.PatchAreaOffset.DataSectors;
				ldlinuxWriter.Write((ushort)sectors.Count);

				ldlinux.Position = patchArea + Syslinux.PatchAreaOffset.DataSectors;
				ldlinuxWriter.Write((ushort)2);

				ldlinux.Position = patchArea + Syslinux.PatchAreaOffset.DataSectors;
				ldlinuxWriter.Write((uint)fileSize >> 2);

				// Generate Extents
				var extents = GenerateExtents(sectors);

				ldlinux.Position = ex;

				// Write out extents
				foreach (var extent in extents)
				{
					ldlinuxWriter.Write((ulong)extent.Start);
					ldlinuxWriter.Write((ushort)extent.Length);
				}

				// Write out ADV
				ldlinux.Position = advptrs;
				ldlinuxWriter.Write((ulong)sectors[sectors.Count - 2]);
				ldlinuxWriter.Write((ulong)sectors[sectors.Count - 1]);

				// Clear out checksum
				ldlinux.Position = patchArea + Syslinux.PatchAreaOffset.Checksum;
				ldlinuxWriter.Write((uint)0);

				// Write back the updated cluster
				ldlinuxWriter.Flush();

				// Re-Calculate checksum
				ldlinux.Position = 0;
				uint csum = Syslinux.LDLINUX_MAGIC;
				for (uint index = 0; index < (ldlinux.Length >> 2); index++)
				{
					csum = csum + ldlinuxReader.ReadUInt32();
				}

				// Set the checksum
				ldlinux.Position = patchArea + Syslinux.PatchAreaOffset.Checksum;
				ldlinuxWriter.Write(csum);

				// Write patched cluster back to disk
				ldlinuxWriter.Flush();

				// Read boot sector
				var fatBootSector = new BinaryFormat(partitionDevice.ReadBlock(0, 1));

				// Set the first sector location of the file
				fatBootSector.SetUInt(sect1Ptr0, fat.GetSectorByCluster(location.FirstCluster));
				fatBootSector.SetUInt(sect1Ptr1, 0);   // since only 32-bit offsets are support, the high portion of 64-bit value is zero

				// Write back patched boot sector
				partitionDevice.WriteBlock(0, 1, fatBootSector.Data);
			}

			if (options.ImageFormat == ImageFormatType.VHD)
			{
				// Create footer
				byte[] footer = VHD.CreateFooter(
					blockCount,
					(uint)(DateTime.Now - (new DateTime(2000, 1, 1, 0, 0, 0))).Seconds,
					options.MediaGuid.ToByteArray(),
					diskGeometry
				);

				diskDevice.WriteBlock(blockCount, 1, footer);
			}

			diskDevice.Dispose();
		}

		protected const int SectorSize = 512;

		protected class Extent
		{
			public ulong Start { get; set; }
			public ushort Length { get; set; }

			public Extent(ulong start, ushort length)
			{
				Start = start;
				Length = length;
			}
		}

		protected static List<Extent> GenerateExtents(List<uint> sectors)
		{
			var extends = new List<Extent>();

			Extent extent = null;

			uint address = 0x8000;   // ldlinux.sys starts loading here
			uint baseAddress = address;

			// Skip the first and last two sectors
			for (int i = 1; i < sectors.Count - 2; i++)
			{
				var sector = sectors[i];

				address = address + SectorSize;

				bool extend =

					// Only if there is an active extent
					(extent != null) &&

					// Sectors in extent must be continuous
					(extent.Start + extent.Length == sector) &&

					// Extents must be strictly less than 64K in size.
					(extent.Length < (65536 / SectorSize)) &&

					// Extents can not cross 64K boundaries - first two hard coded boundaries
					(address != 64 * 1024) && (address != 128 * 1024);

				if (extend)
				{
					// expand extent by one sector
					extent.Length++;
					continue;
				}
				else
				{
					extent = new Extent(sector, 1);
					extends.Add(extent);
					baseAddress = address;
				}
			}

			return extends;
		}
	}
}
