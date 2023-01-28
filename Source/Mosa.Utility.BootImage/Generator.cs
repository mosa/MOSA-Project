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
	static public void Create(BootImageOptions options)
	{
		if (File.Exists(options.DiskImageFileName))
		{
			File.Delete(options.DiskImageFileName);
		}

		uint blockCount = options.BlockCount;

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
		var diskDeviceDriver = new BlockFileStreamDriver(options.DiskImageFileName);

		var diskDevice = new Device() { DeviceDriver = diskDeviceDriver };

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
		var configuraiton = new DiskPartitionConfiguration()
		{
			Index = 0,
			ReadOnly = false,
		};

		if (options.MBROption)
		{
			// Create master boot block record
			var mbr = new MasterBootBlock(diskDeviceDriver)
			{
				// Setup partition entry
				DiskSignature = 0x12345678
			};

			mbr.Partitions[0].Bootable = true;
			mbr.Partitions[0].StartLBA = 2048;    // minimum for grub legacy = 64, grub2 = 2048 (1Mb)
			mbr.Partitions[0].TotalBlocks = blockCount - mbr.Partitions[0].StartLBA;

			switch (options.FileSystem)
			{
				case FileSystem.FAT12: mbr.Partitions[0].PartitionType = PartitionType.FAT12; break;
				case FileSystem.FAT16: mbr.Partitions[0].PartitionType = PartitionType.FAT16; break;
				case FileSystem.FAT32: mbr.Partitions[0].PartitionType = PartitionType.FAT32; break;
				default: break;
			}

			mbr.Code = options.MBRCode;

			mbr.Write();

			configuraiton.StartLBA = mbr.Partitions[0].StartLBA;
			configuraiton.TotalBlocks = mbr.Partitions[0].TotalBlocks;
		}
		else
		{
			configuraiton.StartLBA = 0;
			configuraiton.TotalBlocks = diskDeviceDriver.TotalBlocks;
		}

		// Setup device -- required as part of framework in operating system
		var device = new Device()
		{
			Configuration = configuraiton,
			DeviceDriver = partitionDevice,
			Parent = diskDevice,
		};

		// Setup and initialize
		partitionDevice.Setup(device);
		partitionDevice.Initialize();
		partitionDevice.Start();

		// Set FAT settings
		var fatSettings = new FatSettings();

		switch (options.FileSystem)
		{
			case FileSystem.FAT12: fatSettings.FATType = FatType.FAT12; break;
			case FileSystem.FAT16: fatSettings.FATType = FatType.FAT16; break;
			case FileSystem.FAT32: fatSettings.FATType = FatType.FAT32; break;
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

			string newname = (Path.GetFileNameWithoutExtension(includeFile.Filename).PadRight(8).Substring(0, 8) + Path.GetExtension(includeFile.Filename).PadRight(4).Substring(1, 3)).ToUpperInvariant();
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

		diskDeviceDriver.Dispose();

		Limine.Deploy(options.DiskImageFileName);
	}
}
