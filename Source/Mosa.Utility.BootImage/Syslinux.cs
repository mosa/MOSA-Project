// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using Mosa.FileSystem.FAT;
using System;
using System.IO;
using System.Collections.Generic;
using Mosa.ClassLib;
using Mosa.Compiler.Common;

namespace Mosa.Utility.BootImage
{
	public static class Syslinux
	{
		private const int SectorSize = 512;

		public const uint LDLINUX_MAGIC = 0x3EB202FE;

		public struct PatchAreaOffset
		{
			public const uint Magic = 0;            // LDLINUX_MAGIC
			public const uint Instance = 4;         // Per-version value
			public const uint DataSectors = 8;      // Number of sectors (not including bootsec)
			public const uint AdvSectors = 10;      // Additional sectors for ADVs
			public const uint Dwords = 12;          // Total dwords starting at ldlinux_sys
			public const uint Checksum = 16;        // Checksum starting at ldlinux_sys
			public const uint MaxTransfer = 20;     // Max sectors to transfer
			public const uint EPAOffset = 22;       // Pointer to the extended patch area
		}

		public struct ExtendedPatchAreaOffset
		{
			public const uint AdvPtrOffset = 0;     // ADV pointers
			public const uint DirOffset = 2;        // Current directory field
			public const uint DirLen = 4;           // Length of current directory field
			public const uint SubVolOffset = 6;     // Subvolume field
			public const uint SubVolLen = 8;        // Length of subvolume field
			public const uint SecPtrOffset = 10;    // Sector extent pointers
			public const uint SecPtrCnt = 12;       // Number of sector extent pointers
			public const uint Sect1Ptr0 = 14;       // Boot sector offset of sector 1 ptr LSW
			public const uint Sect1Ptr1 = 16;       // Boot sector offset of sector 1 ptr MSW
			public const uint RaidPatch = 18;       // Boot sector RAID mode patch pointer
		}

		public static void PatchSyslinux(PartitionDevice partitionDevice, FatFileSystem fat)
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

		private static List<Extent> GenerateExtents(List<uint> sectors)
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
