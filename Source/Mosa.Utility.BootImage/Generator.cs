// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.IO;
using Mosa.DeviceSystem;
using Mosa.FileSystem.FAT;

namespace Mosa.Utility.BootImage;

/// <summary>
/// Generator
/// </summary>
public static class Generator
{
	private const int SectorSize = 512;

	/// <summary>
	/// Creates the specified options.
	/// </summary>
	/// <param name="options">The options.</param>
	public static void Create(BootImageOptions options)
	{
		if (File.Exists(options.DiskImageFileName))
		{
			File.Delete(options.DiskImageFileName);
		}

		var blockCount = options.BlockCount;

		if (blockCount == 0)
		{
			blockCount = 2048 * 4 + 1;
			foreach (var file in options.IncludeFiles)
			{
				blockCount += (uint)file.Content.Length / SectorSize + 10;
			}
		}

		var diskGeometry = new DiskGeometry();
		diskGeometry.GuessGeometry(blockCount);

		// Create disk image file
		using (var diskDeviceDriver = new BlockFileStreamDriver(options.DiskImageFileName))
		{
			var diskDevice = new Device { DeviceDriver = diskDeviceDriver };

			// Setup device -- required as part of framework in operating system
			diskDeviceDriver.Setup(diskDevice);
			diskDeviceDriver.Initialize();
			diskDeviceDriver.Start();

			/*if (options.ImageFormat == ImageFormat.VDI)
			{
				// Create header
				var header = VDI.CreateHeader(
					blockCount,
					options.MediaGuid.ToByteArray(),
					options.MediaLastSnapGuid.ToByteArray(),
					diskGeometry
				);

				diskDeviceDriver.WriteBlock(0, 1, header);

				var map = VDI.CreateImageMap(blockCount);

				diskDeviceDriver.WriteBlock(1, (uint)(map.Length / SectorSize), map);

				diskDeviceDriver.BlockOffset = 1 + (uint)(map.Length / 512);
			}*/

			// Expand disk image
			diskDeviceDriver.WriteBlock(blockCount - 1, 1, new byte[SectorSize]);

			// Create partition device driver
			var partitionDevice = new PartitionDeviceDriver();

			// Setup partition configuration
			var configuration = new DiskPartitionConfiguration
			{
				Index = 0,
				ReadOnly = false,
			};

			if (options.ImageFirmware == ImageFirmware.Bios)
			{
				// Create master boot block record
				var mbr = new MasterBootBlock(diskDeviceDriver)
				{
					// Setup partition entry
					DiskSignature = 0x12345678,
					Code = null
				};

				mbr.Partitions[0].Bootable = true;
				mbr.Partitions[0].StartLBA = 2048;
				mbr.Partitions[0].TotalBlocks = blockCount - mbr.Partitions[0].StartLBA;
				mbr.Partitions[0].PartitionType = options.FileSystem switch
				{
					FileSystem.FAT12 => PartitionType.FAT12,
					FileSystem.FAT16 => PartitionType.FAT16,
					FileSystem.FAT32 => PartitionType.FAT32,
					_ => throw new NotImplementedException()
				};

				mbr.Write();

				configuration.StartLBA = mbr.Partitions[0].StartLBA;
				configuration.TotalBlocks = mbr.Partitions[0].TotalBlocks;
			}
			else if (options.ImageFirmware == ImageFirmware.Uefi)
			{
				// Create protective MBR
				var mbr = new MasterBootBlock(diskDeviceDriver)
				{
					// Setup partition entry
					DiskSignature = 0x12345678,
					Code = null
				};

				mbr.Partitions[0].Bootable = false;
				mbr.Partitions[0].StartLBA = 1;
				mbr.Partitions[0].TotalBlocks = blockCount - mbr.Partitions[0].StartLBA;
				mbr.Partitions[0].PartitionType = 0xEE; // GPT protective MBR ID

				mbr.Write();

				// Create GUID partition table
				var gpt = new GuidPartitionTable(diskDeviceDriver);

				gpt.Write();

				configuration.StartLBA = 3;
				configuration.TotalBlocks = blockCount - configuration.StartLBA;
			}
			else
			{
				configuration.StartLBA = 0;
				configuration.TotalBlocks = diskDeviceDriver.TotalBlocks;
			}

			// Setup device -- required as part of framework in operating system
			var device = new Device
			{
				Configuration = configuration,
				DeviceDriver = partitionDevice,
				Parent = diskDevice
			};

			// Setup and initialize
			partitionDevice.Setup(device);
			partitionDevice.Initialize();
			partitionDevice.Start();

			// Set FAT settings
			var fatSettings = new FatSettings
			{
				FATType = options.FileSystem switch
				{
					FileSystem.FAT12 => FatType.FAT12,
					FileSystem.FAT16 => FatType.FAT16,
					FileSystem.FAT32 => FatType.FAT32,
					_ => throw new NotImplementedException()
				},
				FloppyMedia = false,
				VolumeLabel = options.VolumeLabel,
				SerialID = new byte[] { 0x01, 0x02, 0x03, 0x04 },
				SectorsPerTrack = diskGeometry.SectorsPerTrack,
				NumberOfHeads = diskGeometry.Heads,
				HiddenSectors = diskGeometry.SectorsPerTrack,
				OSBootCode = null
			};

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

				var newname = (Path.GetFileNameWithoutExtension(includeFile.Filename).PadRight(8).Substring(0, 8) + Path.GetExtension(includeFile.Filename).PadRight(4).Substring(1, 3)).ToUpperInvariant();
				var location = fat.CreateFile(newname, fileAttributes);

				if (!location.IsValid)
					throw new Exception("Unable to write file");

				var fatFileStream = new FatFileStream(fat, location);
				fatFileStream.Write(includeFile.Content, 0, includeFile.Content.Length);
				fatFileStream.Flush();
			}

			if (options.ImageFormat == ImageFormat.VHD)
			{
				// Create footer
				var footer = VHD.CreateFooter(
					blockCount,
					(uint)(DateTime.Now - new DateTime(2000, 1, 1, 0, 0, 0)).Seconds,
					options.MediaGuid.ToByteArray(),
					diskGeometry
				);

				diskDeviceDriver.WriteBlock(blockCount, 1, footer);
			}
		}

		// Not needed for UEFI
		if (options.ImageFirmware == ImageFirmware.Bios)
			Limine.Deploy(options.DiskImageFileName);
	}
}
