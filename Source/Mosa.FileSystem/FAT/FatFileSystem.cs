// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.DeviceSystem;
using Mosa.FileSystem.FAT.Vfs;
using Mosa.FileSystem.VFS;

namespace Mosa.FileSystem.FAT;

#region Constants

/// <summary>
/// Boot Sector
/// </summary>
internal struct BootSector
{
	internal const uint JumpInstruction = 0x00; // 3
	internal const uint EOMName = 0x03; // 8 - "IBM  3.3", "MSDOS5.0", "MSWIN4.1", "FreeDOS"
	internal const uint BytesPerSector = 0x0B;  // 2 - common value 512
	internal const uint SectorsPerCluster = 0x0D;   // 1 - valid 1 to 128
	internal const uint ReservedSectors = 0x0E; // 2 - 1 for FAT12/FAT16, usually 32 for FAT32
	internal const uint FatAllocationTables = 0x10; // 1 - always 2
	internal const uint MaxRootDirEntries = 0x11; // 2
	internal const uint TotalSectors16 = 0x13;  // 2
	internal const uint MediaDescriptor = 0x15; // 1
	internal const uint SectorsPerFAT = 0x16; // 2
	internal const uint SectorsPerTrack = 0x18; // 2
	internal const uint NumberOfHeads = 0x1A;   // 2
	internal const uint HiddenSectors = 0x1C; // 4
	internal const uint TotalSectors32 = 0x20; // 4

	// Extended BIOS Parameter Block

	internal const uint PhysicalDriveNbr = 0x24; // 1
	internal const uint ReservedCurrentHead = 0x25; // 1
	internal const uint ExtendedBootSignature = 0x26; // 1 // value: 0x29 or 0x28
	internal const uint IDSerialNumber = 0x27; // 4
	internal const uint VolumeLabel = 0x2B; // 11
	internal const uint FATType = 0x36; // 8 - padded with blanks (0x20) "FAT12"; "FAT16"
	internal const uint OSBootCode = 0x3E; // 448 - Operating system boot code
	internal const uint BootSectorSignature = 0x1FE; // 2 - value: 0x55 0xaa

	// FAT32

	internal const uint FAT32_SectorPerFAT = 0x24; // 4
	internal const uint FAT32_Flags = 0x28; // 2
	internal const uint FAT32_Version = 0x2A; // 2
	internal const uint FAT32_ClusterNumberOfRoot = 0x2C; // 4
	internal const uint FAT32_SectorFSInformation = 0x30; // 2
	internal const uint FAT32_SecondBootSector = 0x32; // 2
	internal const uint FAT32_Reserved1 = 0x34; // 12
	internal const uint FAT32_PhysicalDriveNbr = 0x40; // 1
	internal const uint FAT32_Reserved2 = 0x40; // 1
	internal const uint FAT32_ExtendedBootSignature = 0x42; // 1
	internal const uint FAT32_IDSerialNumber = 0x43; // 4
	internal const uint FAT32_VolumeLabel = 0x47; // 2
	internal const uint FAT32_FATType = 0x52; // 2
	internal const uint FAT32_OSBootCode = 0x5A; // 2
}

/// <summary>
/// FSInfo
/// </summary>
internal struct FSInfo
{
	internal const uint FSI_LeadSignature = 0x00; // 4 - always 0x41615252
	internal const uint FSI_Reserved1 = 0x04; // 480 - always 0
	internal const uint FSI_StructureSigature = 484; // 4 - always 0x61417272
	internal const uint FSI_FreeCount = 488; // 4
	internal const uint FSI_NextFree = 492; // 4
	internal const uint FSI_Reserved2 = 496; // 4 - always 0
	internal const uint FSI_TrailSignature = 508; // 4 - always 0xAA550000
	internal const uint FSI_TrailSignature2 = 510; // 4 - always 0xAA55
}

/// <summary>
/// Entry
/// </summary>
internal struct Entry
{
	internal const uint DOSName = 0x00; // 8
	internal const uint DOSExtension = 0x08;    // 3
	internal const uint FileAttributes = 0x0B;  // 1
	internal const uint Reserved = 0x0C;    // 1
	internal const uint CreationTimeFine = 0x0D; // 1
	internal const uint CreationTime = 0x0E; // 2
	internal const uint CreationDate = 0x10; // 2
	internal const uint LastAccessDate = 0x12; // 2
	internal const uint EAIndex = 0x14; // 2
	internal const uint LastModifiedTime = 0x16; // 2
	internal const uint LastModifiedDate = 0x18; // 2
	internal const uint FirstCluster = 0x1A; // 2
	internal const uint FileSize = 0x1C; // 4
	internal const uint EntrySize = 32;
}

/// <summary>
/// FileName Attribute
/// </summary>
internal struct FileNameAttribute
{
	internal const uint LastEntry = 0x00;
	internal const uint Escape = 0x05;  // special msdos hack where 0x05 really means 0xE5 (since 0xE5 was already used for delete)
	internal const uint Dot = 0x2E;
	internal const uint Deleted = 0xE5;
}

#endregion Constants

/// <summary>
/// Fat File System
/// </summary>
public class FatFileSystem : GenericFileSystem
{
	/// <summary>
	/// The end of cluster mark
	/// </summary>
	private uint endOfClusterMark;

	/// <summary>
	/// The bad cluster mark
	/// </summary>
	private uint badClusterMark;

	/// <summary>
	/// The reserved cluster mark
	/// </summary>
	private uint reservedClusterMark;

	/// <summary>
	/// The fat mask
	/// </summary>
	private uint fatMask;

	/// <summary>
	/// The bytes per sector
	/// </summary>
	private uint bytesPerSector;

	/// <summary>
	/// The sectors per cluster
	/// </summary>
	private byte sectorsPerCluster;

	/// <summary>
	/// The reserved sectors
	/// </summary>
	private byte reservedSectors;

	/// <summary>
	/// The NBR fats
	/// </summary>
	private byte nbrFats;

	/// <summary>
	/// The root entries
	/// </summary>
	private uint rootEntries;

	/// <summary>
	/// The total clusters
	/// </summary>
	private uint totalClusters;

	/// <summary>
	/// The root dir sectors
	/// </summary>
	private uint rootDirSectors;

	/// <summary>
	/// The first data sector
	/// </summary>
	private uint firstDataSector;

	/// <summary>
	/// The total sectors
	/// </summary>
	private uint totalSectors;

	/// <summary>
	/// The data sectors
	/// </summary>
	private uint dataSectors;

	/// <summary>
	/// The data area start
	/// </summary>
	private uint dataAreaStart;

	/// <summary>
	/// The entries per sector
	/// </summary>
	private uint entriesPerSector;

	/// <summary>
	/// The first root directory sector
	/// </summary>
	private uint firstRootDirectorySector;

	/// <summary>
	/// The root cluster32
	/// </summary>
	private uint rootCluster32;

	/// <summary>
	/// The fat entries
	/// </summary>
	private uint fatEntries;

	/// <summary>
	/// ICompare
	/// </summary>
	public interface ICompare
	{
		/// <summary>
		/// Compares the specified data.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <param name="offset">The offset.</param>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		bool Compare(byte[] data, uint offset, FatType type);
	}

	/// <summary>
	/// Gets the type of the FAT.
	/// </summary>
	/// <value>The type of the FAT.</value>
	public FatType FATType { get; private set; }

	/// <summary>
	/// Initializes a new instance of the <see cref="FatFileSystem"/> class.
	/// </summary>
	/// <param name="partition">The partition.</param>
	public FatFileSystem(IPartitionDevice partition)
		: base(partition)
	{
		ReadBootSector();
	}

	/// <summary>
	/// Gets the type of the settings.
	/// </summary>
	/// <value>The type of the settings.</value>
	public GenericFileSystemSettings SettingsType => new FatSettings();

	/// <summary>
	/// Creates the VFS mount.
	/// </summary>
	/// <returns></returns>
	public override IFileSystem CreateVFSMount()
	{
		return new VfsFileSystem(this);
	}

	/// <summary>
	/// Gets a value indicating whether this instance is read only.
	/// </summary>
	/// <value>
	/// 	<c>true</c> if this instance is read only; otherwise, <c>false</c>.
	/// </value>
	public bool IsReadOnly => true;

	/// <summary>
	/// Gets the cluster size in bytes.
	/// </summary>
	/// <value>
	/// The cluster size in bytes.
	/// </value>
	public uint ClusterSizeInBytes { get; private set; }

	/// <summary>
	/// Gets the sectors per cluster.
	/// </summary>
	/// <value>The sectors per cluster.</value>
	public uint SectorsPerCluster => sectorsPerCluster;

	/// <summary>
	/// Reads the cluster.
	/// </summary>
	/// <param name="cluster">The cluster.</param>
	/// <returns></returns>
	public byte[] ReadCluster(uint cluster)
	{
		return partition.ReadBlock(dataAreaStart + (cluster - 2) * sectorsPerCluster, sectorsPerCluster);
	}

	/// <summary>
	/// Reads the cluster.
	/// </summary>
	/// <param name="cluster">The cluster.</param>
	/// <param name="block">The block.</param>
	/// <returns></returns>
	public bool ReadCluster(uint cluster, byte[] block)
	{
		return partition.ReadBlock(dataAreaStart + (cluster - 2) * sectorsPerCluster, sectorsPerCluster, block);
	}

	/// <summary>
	/// Writes the cluster.
	/// </summary>
	/// <param name="cluster">The cluster.</param>
	/// <param name="block">The block.</param>
	/// <returns></returns>
	public bool WriteCluster(uint cluster, byte[] block)
	{
		return partition.WriteBlock(dataAreaStart + (cluster - 2) * sectorsPerCluster, sectorsPerCluster, block);
	}

	/// <summary>
	/// Reads the boot sector.
	/// </summary>
	/// <returns></returns>
	protected bool ReadBootSector()
	{
		IsValid = false;

		if (BlockSize != 512)   // only going to work with 512 sector sizes (for now)
			return false;

		var bootSector = new DataBlock(partition.ReadBlock(0, 1));

		if (bootSector.GetUShort(BootSector.BootSectorSignature) != 0xAA55)
			return false;

		byte extendedBootSignature = bootSector.GetByte(BootSector.ExtendedBootSignature);
		byte extendedBootSignature32 = bootSector.GetByte(BootSector.FAT32_ExtendedBootSignature);

		if (extendedBootSignature != 0x29 && extendedBootSignature != 0x28 && extendedBootSignature32 != 0x29)
			return false;

		VolumeLabel = bootSector.GetString(BootSector.VolumeLabel, 8).TrimEnd();
		bytesPerSector = bootSector.GetUShort(BootSector.BytesPerSector);
		sectorsPerCluster = bootSector.GetByte(BootSector.SectorsPerCluster);
		reservedSectors = bootSector.GetByte(BootSector.ReservedSectors);
		nbrFats = bootSector.GetByte(BootSector.FatAllocationTables);
		rootEntries = bootSector.GetUShort(BootSector.MaxRootDirEntries);
		rootCluster32 = bootSector.GetUInt32(BootSector.FAT32_ClusterNumberOfRoot);

		uint sectorsPerFat16 = bootSector.GetUShort(BootSector.SectorsPerFAT);
		uint sectorsPerFat32 = bootSector.GetUInt32(BootSector.FAT32_SectorPerFAT);
		uint totalSectors16 = bootSector.GetUShort(BootSector.TotalSectors16);
		uint totalSectors32 = bootSector.GetUInt32(BootSector.TotalSectors32);
		uint sectorsPerFat = sectorsPerFat16 != 0 ? sectorsPerFat16 : sectorsPerFat32;
		uint fatSectors = 0;

		try
		{
			fatSectors = nbrFats * sectorsPerFat;
			ClusterSizeInBytes = sectorsPerCluster * BlockSize;
			rootDirSectors = (rootEntries * 32 + (bytesPerSector - 1)) / bytesPerSector;
			firstDataSector = reservedSectors + nbrFats * sectorsPerFat + rootDirSectors;
			totalSectors = totalSectors16 != 0 ? totalSectors16 : totalSectors32;
			dataSectors = totalSectors - (reservedSectors + nbrFats * sectorsPerFat + rootDirSectors);
			totalClusters = dataSectors / sectorsPerCluster;
			entriesPerSector = bytesPerSector / 32;
			firstRootDirectorySector = reservedSectors + fatSectors;
			dataAreaStart = firstRootDirectorySector + rootDirSectors;
		}
		catch
		{
			return false;
		}

		// Some basic checks
		if (nbrFats is 0 or > 2 || totalSectors == 0 || sectorsPerFat == 0)
			return false;

		if (totalClusters < 4085)
			FATType = FatType.FAT12;
		else if (totalClusters < 65525)
			FATType = FatType.FAT16;
		else
			FATType = FatType.FAT32;

		if (FATType == FatType.FAT12)
		{
			reservedClusterMark = 0xFF0;
			endOfClusterMark = 0x0FF8;
			badClusterMark = 0x0FF7;
			fatMask = 0xFFFFFFFF;
			fatEntries = sectorsPerFat * 3 * BlockSize / 2;
		}
		else if (FATType == FatType.FAT16)
		{
			reservedClusterMark = 0xFFF0;
			endOfClusterMark = 0xFFF8;
			badClusterMark = 0xFFF7;
			fatMask = 0xFFFFFFFF;
			fatEntries = sectorsPerFat * BlockSize / 2;
		}
		else
		{ //  if (type == FatType.FAT32) {
			reservedClusterMark = 0xFFF0;
			endOfClusterMark = 0x0FFFFFF8;
			badClusterMark = 0x0FFFFFF7;
			fatMask = 0x0FFFFFFF;
			fatEntries = sectorsPerFat * BlockSize / 4;
		}

		// More basic checks
		if (FATType == FatType.FAT32 && rootCluster32 == 0)
			return false;

		SerialNumber = bootSector.GetBytes(FATType != FatType.FAT32 ? BootSector.IDSerialNumber : BootSector.FAT32_IDSerialNumber, 4);

		IsValid = true;

		return IsValid;
	}

	/// <summary>
	/// Formats the partition with specified fat settings.
	/// </summary>
	/// <param name="fatSettings">The fat settings.</param>
	/// <returns></returns>
	public bool Format(FatSettings fatSettings)
	{
		if (!partition.CanWrite)
			return false;

		FATType = fatSettings.FATType;
		bytesPerSector = 512;
		nbrFats = 2;

		totalSectors = partition.BlockCount;
		sectorsPerCluster = GetSectorsPerClusterByTotalSectors(FATType, totalSectors);

		if (sectorsPerCluster == 0)
			return false;

		if (FATType == FatType.FAT32)
		{
			reservedSectors = 32;
			rootEntries = 0;
		}
		else
		{
			reservedSectors = 1;
			rootEntries = 512;
		}

		rootDirSectors = (rootEntries * 32 + (bytesPerSector - 1)) / bytesPerSector;

		uint val1 = totalSectors - (reservedSectors + rootDirSectors);
		uint val2 = (uint)(sectorsPerCluster * 256 + nbrFats);

		if (FATType == FatType.FAT32)
			val2 /= 2;

		uint sectorsPerFat = (val1 + (val2 - 1)) / val2;

		firstRootDirectorySector = reservedSectors + sectorsPerFat;

		var bootSector = new DataBlock(512);

		bootSector.SetUInt32(BootSector.JumpInstruction, 0);
		bootSector.SetString(BootSector.EOMName, "MOSA    ");
		bootSector.SetUShort(BootSector.BytesPerSector, (ushort)bytesPerSector);
		bootSector.SetByte(BootSector.SectorsPerCluster, sectorsPerCluster);
		bootSector.SetUShort(BootSector.ReservedSectors, reservedSectors);
		bootSector.SetByte(BootSector.FatAllocationTables, nbrFats);
		bootSector.SetUShort(BootSector.MaxRootDirEntries, (ushort)rootEntries);
		bootSector.SetUShort(BootSector.BootSectorSignature, 0xAA55);

		if (totalSectors > 0xFFFF)
		{
			bootSector.SetUShort(BootSector.TotalSectors16, 0);
			bootSector.SetUInt32(BootSector.TotalSectors32, totalSectors);
		}
		else
		{
			bootSector.SetUShort(BootSector.TotalSectors16, (ushort)totalSectors);
			bootSector.SetUInt32(BootSector.TotalSectors32, 0);
		}

		if (fatSettings.FloppyMedia)
		{
			// Default is 1.44
			bootSector.SetByte(BootSector.MediaDescriptor, 0xF0); // 0xF0 = 3.5" Double Sided, 80 tracks per side, 18 sectors per track (1.44MB).
		}
		else
		{
			bootSector.SetByte(BootSector.MediaDescriptor, 0xF8); // 0xF8 = Hard disk
		}

		bootSector.SetUShort(BootSector.SectorsPerTrack, fatSettings.SectorsPerTrack);
		bootSector.SetUShort(BootSector.NumberOfHeads, fatSettings.NumberOfHeads);
		bootSector.SetUInt32(BootSector.HiddenSectors, fatSettings.HiddenSectors);

		if (FATType != FatType.FAT32)
		{
			bootSector.SetUShort(BootSector.SectorsPerFAT, (ushort)sectorsPerFat);
			if (fatSettings.FloppyMedia)
				bootSector.SetByte(BootSector.PhysicalDriveNbr, 0x00);
			else
				bootSector.SetByte(BootSector.PhysicalDriveNbr, 0x80);

			bootSector.SetByte(BootSector.ReservedCurrentHead, 0);
			bootSector.SetByte(BootSector.ExtendedBootSignature, 0x29);
			bootSector.SetBytes(BootSector.IDSerialNumber, fatSettings.SerialID, 0, (uint)Math.Min(4, fatSettings.SerialID.Length));

			if (string.IsNullOrEmpty(fatSettings.VolumeLabel))
			{
				bootSector.SetString(BootSector.VolumeLabel, "NO NAME    ");
			}
			else
			{
				bootSector.SetString(BootSector.VolumeLabel, "           ");  // 11 blank spaces
				bootSector.SetString(BootSector.VolumeLabel, fatSettings.VolumeLabel, (uint)Math.Min(11, fatSettings.VolumeLabel.Length));
			}

			if (fatSettings.OSBootCode != null)
			{
				bootSector.SetBytes(BootSector.JumpInstruction, fatSettings.OSBootCode, BootSector.JumpInstruction, 3);
				bootSector.SetBytes(BootSector.OSBootCode, fatSettings.OSBootCode, BootSector.OSBootCode, (uint)Math.Min(448, fatSettings.OSBootCode.Length));
			}

			if (FATType == FatType.FAT12)
				bootSector.SetString(BootSector.FATType, "FAT12   ");
			else
				bootSector.SetString(BootSector.FATType, "FAT16   ");
		}

		if (FATType == FatType.FAT32)
		{
			bootSector.SetUShort(BootSector.SectorsPerFAT, 0);
			bootSector.SetUInt32(BootSector.FAT32_SectorPerFAT, sectorsPerFat);
			bootSector.SetByte(BootSector.FAT32_Flags, 0);
			bootSector.SetUShort(BootSector.FAT32_Version, 0);
			bootSector.SetUInt32(BootSector.FAT32_ClusterNumberOfRoot, 2);
			bootSector.SetUShort(BootSector.FAT32_SectorFSInformation, 1);
			bootSector.SetUShort(BootSector.FAT32_SecondBootSector, 6);
			bootSector.SetByte(BootSector.FAT32_PhysicalDriveNbr, 0x80);
			bootSector.SetByte(BootSector.FAT32_Reserved2, 0);
			bootSector.SetByte(BootSector.FAT32_ExtendedBootSignature, 0x29);
			bootSector.SetBytes(BootSector.FAT32_IDSerialNumber, fatSettings.SerialID, 0, (uint)Math.Min(4, fatSettings.SerialID.Length));
			bootSector.SetString(BootSector.FAT32_VolumeLabel, "           ");  // 11 blank spaces
			bootSector.SetString(BootSector.FAT32_VolumeLabel, fatSettings.VolumeLabel, (uint)(fatSettings.VolumeLabel.Length <= 11 ? fatSettings.VolumeLabel.Length : 11));
			bootSector.SetString(BootSector.FAT32_FATType, "FAT32   ");

			if (fatSettings.OSBootCode.Length == 512)
			{
				bootSector.SetBytes(BootSector.JumpInstruction, fatSettings.OSBootCode, BootSector.JumpInstruction, 3);
				bootSector.SetBytes(BootSector.FAT32_OSBootCode, fatSettings.OSBootCode, BootSector.FAT32_OSBootCode, 420);
			}
			else
			{
				bootSector.SetByte(BootSector.JumpInstruction, 0xEB);  // 0xEB = JMP Instruction
				bootSector.SetByte(BootSector.JumpInstruction + 1, 0x58);
				bootSector.SetByte(BootSector.JumpInstruction + 2, 0x90);
				bootSector.SetBytes(BootSector.FAT32_OSBootCode, fatSettings.OSBootCode, 0, (uint)Math.Min(420, fatSettings.OSBootCode.Length));
			}
		}

		// Write Boot Sector
		partition.WriteBlock(0, 1, bootSector.Data);

		if (FATType == FatType.FAT32)
		{
			// Write backup Boot Sector
			if (FATType == FatType.FAT32)
				partition.WriteBlock(6, 1, bootSector.Data);

			// Create FSInfo Structure
			var infoSector = new DataBlock(512);

			infoSector.SetUInt32(FSInfo.FSI_LeadSignature, 0x41615252);

			//FSInfo.FSI_Reserved1
			infoSector.SetUInt32(FSInfo.FSI_StructureSigature, 0x61417272);
			infoSector.SetUInt32(FSInfo.FSI_FreeCount, 0xFFFFFFFF);
			infoSector.SetUInt32(FSInfo.FSI_NextFree, 0xFFFFFFFF);

			//FSInfo.FSI_Reserved2
			bootSector.SetUInt32(FSInfo.FSI_TrailSignature, 0xAA550000);

			// Write FSInfo Structure
			partition.WriteBlock(1, 1, infoSector.Data);
			partition.WriteBlock(7, 1, infoSector.Data);

			// Create 2nd sector
			var secondSector = new DataBlock(512);

			secondSector.SetUShort(FSInfo.FSI_TrailSignature2, 0xAA55);

			partition.WriteBlock(2, 1, secondSector.Data);
			partition.WriteBlock(8, 1, secondSector.Data);
		}

		// Create FAT table(s)

		// Clear primary & secondary FATs
		var emptyFat = new DataBlock(512);

		for (uint i = 1; i < sectorsPerFat; i++)
			partition.WriteBlock(reservedSectors + i, 1, emptyFat.Data);

		if (nbrFats == 2)
		{
			for (uint i = 1; i < sectorsPerFat; i++)
			{
				partition.WriteBlock(reservedSectors + sectorsPerFat + i, 1, emptyFat.Data);
			}
		}

		// First FAT block is special
		var firstFat = new DataBlock(512);

		if (FATType == FatType.FAT12)
		{
			firstFat.SetByte(1, 0xFF);
			firstFat.SetByte(2, 0xFF); // 0xF8
		}
		else if (FATType == FatType.FAT16)
		{
			firstFat.SetUShort(0, 0xFFFF);
			firstFat.SetUShort(2, 0xFFFF); // 0xFFF8
		}
		else // if (type == FatType.FAT32)
		{
			firstFat.SetUInt32(0, 0x0FFFFFFF);
			firstFat.SetUInt32(4, 0x0FFFFFFF); // 0x0FFFFFF8
			firstFat.SetUInt32(8, 0x0FFFFFFF); // Also reserve the 2nd cluster for root directory
		}

		if (fatSettings.FloppyMedia)
			firstFat.SetByte(0, 0xF0);
		else
			firstFat.SetByte(0, 0xF8);

		partition.WriteBlock(reservedSectors, 1, firstFat.Data);

		if (nbrFats == 2)
			partition.WriteBlock(reservedSectors + sectorsPerFat, 1, firstFat.Data);

		// Create Empty Root Directory
		if (FATType != FatType.FAT32)
		{
			for (uint i = 0; i < rootDirSectors; i++)
			{
				partition.WriteBlock(firstRootDirectorySector + i, 1, emptyFat.Data);
			}
		}

		return ReadBootSector();
	}

	/// <summary>
	/// Determines whether [is cluster free] [the specified cluster].
	/// </summary>
	/// <param name="cluster">The cluster.</param>
	/// <returns>
	/// 	<c>true</c> if [is cluster free] [the specified cluster]; otherwise, <c>false</c>.
	/// </returns>
	protected bool IsClusterFree(uint cluster)
	{
		return (cluster & fatMask) == 0x00;
	}

	/// <summary>
	/// Determines whether [is cluster reserved] [the specified cluster].
	/// </summary>
	/// <param name="cluster">The cluster.</param>
	/// <returns>
	/// 	<c>true</c> if [is cluster reserved] [the specified cluster]; otherwise, <c>false</c>.
	/// </returns>
	protected bool IsClusterReserved(uint cluster)
	{
		return (cluster & fatMask) == 0x00 || ((cluster & fatMask) >= reservedClusterMark && (cluster & fatMask) < badClusterMark);
	}

	/// <summary>
	/// Determines whether [is cluster bad] [the specified cluster].
	/// </summary>
	/// <param name="cluster">The cluster.</param>
	/// <returns>
	/// 	<c>true</c> if [is cluster bad] [the specified cluster]; otherwise, <c>false</c>.
	/// </returns>
	protected bool IsClusterBad(uint cluster)
	{
		return (cluster & fatMask) == badClusterMark;
	}

	/// <summary>
	/// Determines whether [is cluster last] [the specified cluster].
	/// </summary>
	/// <param name="cluster">The cluster.</param>
	/// <returns>
	/// 	<c>true</c> if [is cluster last] [the specified cluster]; otherwise, <c>false</c>.
	/// </returns>
	protected bool IsClusterLast(uint cluster)
	{
		return (cluster & fatMask) >= endOfClusterMark;
	}

	/// <summary>
	/// Gets the cluster by sector.
	/// </summary>
	/// <param name="sector">The sector.</param>
	/// <returns></returns>
	protected uint GetClusterBySector(uint sector)
	{
		if (sector < dataAreaStart)
			return 0;

		return (sector - dataAreaStart) / sectorsPerCluster;
	}

	/// <summary>
	/// Gets the sector by cluster.
	/// </summary>
	/// <param name="cluster">The cluster.</param>
	/// <returns></returns>
	public uint GetSectorByCluster(uint cluster)
	{
		return dataAreaStart + (cluster - 2) * sectorsPerCluster;
	}

	/// <summary>
	/// Gets the cluster entry value.
	/// </summary>
	/// <param name="cluster">The cluster.</param>
	/// <returns></returns>
	protected uint GetClusterEntryValue(uint cluster)
	{
		uint fatoffset = 0;

		if (FATType == FatType.FAT12)
			fatoffset = cluster + cluster / 2;
		else if (FATType == FatType.FAT16)
			fatoffset = cluster * 2;
		else //if (type == FatType.FAT32)
			fatoffset = cluster * 4;

		uint sector = reservedSectors + fatoffset / bytesPerSector;
		uint sectorOffset = fatoffset % bytesPerSector;
		uint nbrSectors = 1;

		if (FATType == FatType.FAT12 && sectorOffset == bytesPerSector - 1)
			nbrSectors = 2;

		var fat = new DataBlock(partition.ReadBlock(sector, nbrSectors));

		uint clusterValue;

		if (FATType == FatType.FAT12)
		{
			clusterValue = fat.GetUShort(sectorOffset);
			if (cluster % 2 == 1)
				clusterValue >>= 4;
			else
				clusterValue &= 0x0FFF;
		}
		else if (FATType == FatType.FAT16)
		{
			clusterValue = fat.GetUShort(sectorOffset);
		}
		else //if (type == FatType.FAT32)
		{
			clusterValue = fat.GetUInt32(sectorOffset) & 0x0FFFFFFF;
		}

		return clusterValue;
	}

	/// <summary>
	/// Sets the cluster entry value.
	/// </summary>
	/// <param name="cluster">The cluster.</param>
	/// <param name="nextcluster">The next cluster.</param>
	/// <returns></returns>
	protected bool SetClusterEntryValue(uint cluster, uint nextcluster)
	{
		uint fatOffset = 0;

		if (FATType == FatType.FAT12)
			fatOffset = cluster + cluster / 2;
		else if (FATType == FatType.FAT16)
			fatOffset = cluster * 2;
		else //if (type == FatType.FAT32)
			fatOffset = cluster * 4;

		uint sector = reservedSectors + fatOffset / bytesPerSector;
		uint sectorOffset = fatOffset % bytesPerSector;
		uint nbrSectors = 1;

		if (FATType == FatType.FAT12 && sectorOffset == bytesPerSector - 1)
			nbrSectors = 2;

		var fat = new DataBlock(partition.ReadBlock(sector, nbrSectors));

		switch (FATType)
		{
			case FatType.FAT12:
				{
					uint clustervalue = fat.GetUShort(sectorOffset);

					if (cluster % 2 == 1)
						clustervalue = (clustervalue & 0x000F) | (nextcluster << 4);
					else
						clustervalue = (clustervalue & 0xF000) | (nextcluster & 0x0FFF);

					fat.SetUShort(sectorOffset, (ushort)clustervalue);
					break;
				}
			case FatType.FAT16:
				{
					fat.SetUShort(sectorOffset, (ushort)(nextcluster & 0xFFFF));
					break;
				}
			default:
				{
					fat.SetUInt32(sectorOffset, nextcluster);
					break;
				}
		}

		partition.WriteBlock(sector, nbrSectors, fat.Data);

		return true;
	}

	/// <summary>
	/// Gets the sectors per cluster by total sectors.
	/// </summary>
	/// <param name="type">The type.</param>
	/// <param name="sectors">The sectors.</param>
	/// <returns></returns>
	public static byte GetSectorsPerClusterByTotalSectors(FatType type, uint sectors)
	{
		switch (type)
		{
			case FatType.FAT12:
				{
					if (sectors < 512) return 1;
					else if (sectors == 720) return 2;
					else if (sectors == 1440) return 2;
					else if (sectors <= 2880) return 1;
					else if (sectors <= 5760) return 2;
					else if (sectors <= 16384) return 4;
					else if (sectors <= 32768) return 8;
					else return 0;
				}
			case FatType.FAT16:
				{
					if (sectors < 8400) return 0;
					else if (sectors < 32680) return 2;
					else if (sectors < 262144) return 4;
					else if (sectors < 524288) return 8;
					else if (sectors < 1048576) return 16;
					else if (sectors < 2097152) return 32;
					else if (sectors < 4194304) return 64;
					else return 0;
				}
			case FatType.FAT32:
				{
					if (sectors < 66600) return 0;
					else if (sectors < 532480) return 1;
					else if (sectors < 16777216) return 8;
					else if (sectors < 33554432) return 16;
					else if (sectors < 67108864) return 32;
					else return 64;
				}
			default: return 0;
		}
	}

	/// <summary>
	/// Extracts the name of the file.
	/// </summary>
	/// <param name="directory">The directory.</param>
	/// <param name="index">The index.</param>
	/// <returns></returns>
	internal static string ExtractFileName(byte[] directory, uint index)
	{
		// rewrite to use string
		var entry = new DataBlock(directory);

		var name = new char[12];

		for (uint i = 0; i < 8; i++)
			name[i] = (char)entry.GetByte(index + i + Entry.DOSName);

		int len = 8;

		for (int i = 7; i > 0; i--)
		{
			if (name[i] == ' ')
			{
				len--;
			}
			else
			{
				break;
			}
		}

		// special case where real character is same as the delete
		if (len >= 1 && name[0] == (char)FileNameAttribute.Escape)
			name[0] = (char)FileNameAttribute.Deleted;

		name[len] = '.';

		len++;

		for (uint i = 0; i < 3; i++)
			name[len + i] = (char)entry.GetByte(index + i + Entry.DOSExtension);

		len += 3;

		int spaces = 0;
		for (int i = len - 1; i >= 0; i--)
		{
			if (name[i] != ' ')
			{
				break;
			}
			else
			{
				spaces++;
			}
		}

		if (spaces == 3)
			spaces = 4;

		len -= spaces;

		// FIXME
		string str = string.Empty;

		for (uint i = 0; i < len; i++)
			str += name[i];

		return str;
	}

	/// <summary>
	/// Determines whether [is valid fat character] [the specified c].
	/// </summary>
	/// <param name="c">The c.</param>
	/// <returns>
	/// 	<c>true</c> if [is valid fat character] [the specified c]; otherwise, <c>false</c>.
	/// </returns>
	protected static bool IsValidFatCharacter(char c)
	{
		if ((c >= 'A' && c <= 'Z') || (c >= '0' && c <= '9'))
			return true;

		//if (c >= 128 || c >= 255)
		//return false;

		const string valid = " !#$%&'()-@^_`{}~";

		for (int i = 0; i < valid.Length; i++)
		{
			if (valid[i] == c)
			{
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// Gets the cluster entry.
	/// </summary>
	/// <param name="data">The data.</param>
	/// <param name="index">The index.</param>
	/// <param name="type">The type.</param>
	/// <returns></returns>
	internal static uint GetClusterEntry(byte[] data, uint index, FatType type)
	{
		var entry = new DataBlock(data);

		uint cluster = entry.GetUShort(Entry.FirstCluster + index * Entry.EntrySize);

		if (type == FatType.FAT32)
			cluster |= (uint)entry.GetUShort(Entry.EAIndex + index * Entry.EntrySize) << 16;

		return cluster;
	}

	public FatFileLocation FindEntry(string name)
	{
		return FindEntry(new Find.WithName(name), 0);
	}

	/// <summary>
	/// Finds the entry.
	/// </summary>
	/// <param name="compare">The compare.</param>
	/// <param name="startCluster">The start cluster.</param>
	/// <returns></returns>
	internal FatFileLocation FindEntry(FatFileSystem.ICompare compare, uint startCluster)
	{
		uint activeSector = startCluster * sectorsPerCluster;

		if (startCluster == 0)
			activeSector = FATType == FatType.FAT32 ? GetSectorByCluster(rootCluster32) : firstRootDirectorySector;

		uint increment = 0;

		for (; ; )
		{
			var directory = new DataBlock(partition.ReadBlock(activeSector, 1));

			for (uint index = 0; index < entriesPerSector; index++)
			{
				if (compare.Compare(directory.Data, index * 32, FATType))
				{
					var attribute = (FatFileAttributes)directory.GetByte(index * Entry.EntrySize + Entry.FileAttributes);
					return new FatFileLocation(GetClusterEntry(directory.Data, index, FATType), activeSector, index, (attribute & FatFileAttributes.SubDirectory) != 0);
				}

				if (directory.GetByte(Entry.DOSName + index * Entry.EntrySize) == FileNameAttribute.LastEntry)
					return new FatFileLocation();
			}

			++increment;

			if (startCluster == 0 && FATType != FatType.FAT32)
			{
				// FAT12/16 Root directory
				if (increment >= rootDirSectors)
					return new FatFileLocation();

				activeSector = startCluster + increment;
				continue;
			}
			else
			{
				// subdirectory
				if (increment < sectorsPerCluster)
				{
					// still within cluster
					activeSector = startCluster + increment;
					continue;
				}

				// exiting cluster

				// goto next cluster (if any)
				uint cluster = GetClusterBySector(startCluster);

				if (cluster == 0)
					return new FatFileLocation();

				uint nextCluster = GetClusterEntryValue(cluster);

				if (IsClusterLast(nextCluster) || IsClusterBad(nextCluster) || IsClusterFree(nextCluster) || IsClusterReserved(nextCluster))
					return new FatFileLocation();

				activeSector = (uint)(dataAreaStart + (nextCluster - 1 * sectorsPerCluster));

				continue;
			}
		}
	}

	/// <summary>
	/// Gets the size of the file.
	/// </summary>
	/// <param name="directoryBlock">The directory block.</param>
	/// <param name="index">The index.</param>
	/// <returns></returns>
	public uint GetFileSize(uint directoryBlock, uint index)
	{
		var directory = new DataBlock(partition.ReadBlock(directoryBlock, 1));
		return directory.GetUInt32(index * Entry.EntrySize + Entry.FileSize);
	}

	/// <summary>
	/// Updates the length.
	/// </summary>
	/// <param name="size">The size.</param>
	/// <param name="firstCluster">The first cluster.</param>
	/// <param name="directorySector">The directory sector.</param>
	/// <param name="directorySectorIndex">Index of the directory sector.</param>
	public void UpdateLength(uint size, uint firstCluster, uint directorySector, uint directorySectorIndex)
	{
		// Truncate the file
		var entry = new DataBlock(partition.ReadBlock(directorySector, 1));

		// Truncate the file length and set
		entry.SetUInt32(Entry.FileSize + directorySectorIndex * Entry.EntrySize, size);

		if (size == 0)
			entry.SetUInt32(Entry.FirstCluster + directorySectorIndex * Entry.EntrySize, 0);

		partition.WriteBlock(directorySector, 1, entry.Data);

		if (size == 0)
		{
			FreeClusterChain(firstCluster);
		}
	}

	/// <summary>
	/// Creates the file.
	/// </summary>
	/// <param name="filename">The filename.</param>
	/// <param name="fileAttributes">The file attributes.</param>
	/// <returns></returns>
	public FatFileLocation CreateFile(string filename, FatFileAttributes fileAttributes)
	{
		return CreateFile(filename, fileAttributes, 0);
	}

	/// <summary>
	/// Creates the file.
	/// </summary>
	/// <param name="filename">The filename.</param>
	/// <param name="fileAttributes">The file attributes.</param>
	/// <param name="directoryCluster">The directory cluster.</param>
	/// <returns></returns>
	public FatFileLocation CreateFile(string filename, FatFileAttributes fileAttributes, uint directoryCluster)
	{
		var location = FindEntry(new Find.WithName(filename), directoryCluster);

		if (location.IsValid)
		{
			// Truncate the file
			var entry = new DataBlock(partition.ReadBlock(location.DirectorySector, 1));

			// Truncate the file length and reset the start cluster
			entry.SetUInt32(Entry.FileSize + location.DirectorySectorIndex * Entry.EntrySize, 0);
			entry.SetUInt32(Entry.FirstCluster + location.DirectorySectorIndex * Entry.EntrySize, 0);

			partition.WriteBlock(location.DirectorySector, 1, entry.Data);

			FreeClusterChain(location.FirstCluster);

			location.FirstCluster = 0;

			return location;
		}

		// Find an empty location in the directory
		location = FindEntry(new Find.Empty(), directoryCluster);

		if (!location.IsValid)
		{
			// Extend Directory

			// TODO

			return location;
		}

		var directory = new DataBlock(partition.ReadBlock(location.DirectorySector, 1));

		if (filename.Length > 11)
			filename = filename.Substring(0, 11);

		// Create Entry
		directory.SetString(Entry.DOSName + location.DirectorySectorIndex * Entry.EntrySize, "            ", 11);
		directory.SetString(Entry.DOSName + location.DirectorySectorIndex * Entry.EntrySize, filename);
		directory.SetByte(Entry.FileAttributes + location.DirectorySectorIndex * Entry.EntrySize, (byte)fileAttributes);
		directory.SetByte(Entry.Reserved + location.DirectorySectorIndex * Entry.EntrySize, 0);
		directory.SetByte(Entry.CreationTimeFine + location.DirectorySectorIndex * Entry.EntrySize, 0);
		directory.SetUShort(Entry.CreationTime + location.DirectorySectorIndex * Entry.EntrySize, 0);
		directory.SetUShort(Entry.CreationDate + location.DirectorySectorIndex * Entry.EntrySize, 0);
		directory.SetUShort(Entry.LastAccessDate + location.DirectorySectorIndex * Entry.EntrySize, 0);
		directory.SetUShort(Entry.LastModifiedTime + location.DirectorySectorIndex * Entry.EntrySize, 0);
		directory.SetUShort(Entry.LastModifiedDate + location.DirectorySectorIndex * Entry.EntrySize, 0);
		directory.SetUShort(Entry.FirstCluster + location.DirectorySectorIndex * Entry.EntrySize, 0);
		directory.SetUInt32(Entry.FileSize + location.DirectorySectorIndex * Entry.EntrySize, 0);

		partition.WriteBlock(location.DirectorySector, 1, directory.Data);

		return location;
	}

	/// <summary>
	/// Deletes the specified file or directory
	/// </summary>
	/// <param name="targetCluster">The target cluster.</param>
	/// <param name="directorySector">The directory sector.</param>
	/// <param name="directorySectorIndex">Index of the directory sector.</param>
	public void Delete(uint targetCluster, uint directorySector, uint directorySectorIndex)
	{
		var entry = new DataBlock(partition.ReadBlock(directorySector, 1));

		entry.SetByte(Entry.DOSName + directorySectorIndex * Entry.EntrySize, (byte)FileNameAttribute.Deleted);

		partition.WriteBlock(directorySector, 1, entry.Data);

		FreeClusterChain(targetCluster);
	}

	/// <summary>
	/// Sets the name of the volume.
	/// </summary>
	/// <param name="volumeName">Name of the volume.</param>
	public void SetVolumeName(string volumeName)
	{
		if (VolumeLabel.Length > 8)
			VolumeLabel = VolumeLabel.Substring(0, 8);

		var location = FindEntry(new Find.Volume(), 0);

		if (!location.IsValid)
		{
			location = FindEntry(new Find.Empty(), 0);

			if (!location.IsValid)
				return; // TODO: something went wrong
		}

		var directory = new DataBlock(partition.ReadBlock(location.DirectorySector, 1));

		if (volumeName.Length > 8)
			volumeName = volumeName.Substring(0, 8);

		// Create Entry
		directory.SetString(Entry.DOSName + location.DirectorySectorIndex * Entry.EntrySize, "            ", 11);
		directory.SetString(Entry.DOSName + location.DirectorySectorIndex * Entry.EntrySize, volumeName);
		directory.SetByte(Entry.FileAttributes + location.DirectorySectorIndex * Entry.EntrySize, (byte)FatFileAttributes.VolumeLabel);
		directory.SetByte(Entry.Reserved + location.DirectorySectorIndex * Entry.EntrySize, 0);
		directory.SetByte(Entry.CreationTimeFine + location.DirectorySectorIndex * Entry.EntrySize, 0);
		directory.SetUShort(Entry.CreationTime + location.DirectorySectorIndex * Entry.EntrySize, 0);
		directory.SetUShort(Entry.CreationDate + location.DirectorySectorIndex * Entry.EntrySize, 0);
		directory.SetUShort(Entry.LastAccessDate + location.DirectorySectorIndex * Entry.EntrySize, 0);
		directory.SetUShort(Entry.LastModifiedTime + location.DirectorySectorIndex * Entry.EntrySize, 0);
		directory.SetUShort(Entry.LastModifiedDate + location.DirectorySectorIndex * Entry.EntrySize, 0);
		directory.SetUShort(Entry.FirstCluster + location.DirectorySectorIndex * Entry.EntrySize, 0);
		directory.SetUInt32(Entry.FileSize + location.DirectorySectorIndex * Entry.EntrySize, 0);

		partition.WriteBlock(location.DirectorySector, 1, directory.Data);
	}

	/// <summary>
	/// Frees the cluster chain.
	/// </summary>
	/// <param name="firstCluster">The first cluster.</param>
	/// <returns></returns>
	protected bool FreeClusterChain(uint firstCluster)
	{
		if (firstCluster == 0)
			return true;

		uint at = firstCluster;

		while (true)
		{
			uint next = GetClusterEntryValue(firstCluster);
			SetClusterEntryValue(at, 0);

			if (IsClusterLast(next))
				return true;

			if (IsClusterFree(next) || IsClusterBad(next) || IsClusterReserved(next))
				return false;

			at = next;
		}
	}

	/// <summary>
	/// Finds the Nth cluster.
	/// </summary>
	/// <param name="start">The start.</param>
	/// <param name="count">The count.</param>
	/// <returns></returns>
	public uint FindNthCluster(uint start, uint count)
	{
		// TODO: add locking
		uint at = start;

		for (int i = 0; i < count; i++)
		{
			at = GetClusterEntryValue(at);

			if (IsClusterLast(at))
				return 0;

			if (IsClusterFree(at) || IsClusterBad(at) || IsClusterReserved(at))
				return 0;
		}

		return at;
	}

	/// <summary>
	/// Gets the next cluster.
	/// </summary>
	/// <param name="start">The start.</param>
	/// <returns></returns>
	public uint GetNextCluster(uint start)
	{
		// TODO: add locking
		uint at = GetClusterEntryValue(start);

		if (IsClusterLast(at))
			return 0;

		if (IsClusterFree(at) || IsClusterBad(at) || IsClusterReserved(at))
			return 0;

		return at;
	}

	protected uint lastFreeHint;

	/// <summary>
	/// Allocates the cluster.
	/// </summary>
	/// <returns></returns>
	protected uint AllocateCluster()
	{
		uint at = lastFreeHint + 1;

		if (at < 2)
			at = 2;

		uint last = at - 1;

		if (last == 1)
			last = fatEntries;

		while (at != last)
		{
			uint value = GetClusterEntryValue(at);

			if (IsClusterFree(value))
			{
				SetClusterEntryValue(at, 0xFFFFFFFF /*endOfClusterMark*/);
				lastFreeHint = at;
				return at;
			}

			at++;

			if (at >= fatEntries)
				at = 2;
		}

		return 0;   // mean no free space
	}

	/// <summary>
	/// Allocates the first cluster.
	/// </summary>
	/// <param name="directorySector">The directory sector.</param>
	/// <param name="directorySectorIndex">Index of the directory sector.</param>
	/// <returns></returns>
	public uint AllocateFirstCluster(uint directorySector, uint directorySectorIndex)
	{
		uint newCluster = AllocateCluster();

		if (newCluster == 0)
			return 0;

		// Truncate set first cluster
		var entry = new DataBlock(partition.ReadBlock(directorySector, 1));
		entry.SetUInt32(Entry.FirstCluster + directorySectorIndex * Entry.EntrySize, newCluster);
		partition.WriteBlock(directorySector, 1, entry.Data);

		return newCluster;
	}

	/// <summary>
	/// Adds the cluster.
	/// </summary>
	/// <param name="lastCluster">The last cluster.</param>
	/// <returns></returns>
	public uint AddCluster(uint lastCluster)
	{
		uint newCluster = AllocateCluster();

		if (newCluster == 0)
			return 0;

		if (!SetClusterEntryValue(lastCluster, newCluster))
			return 0;

		return newCluster;
	}

	//protected OpenFile ExtractFileInformation (MemoryBlock directory, uint index, OpenFile parent)
	//{
	//    uint offset = index * 32;

	//    byte first = directory.GetByte (offset + Entry.DOSName);

	//    if ((first == FileNameAttribute.LastEntry) || (first == FileNameAttribute.Deleted))
	//        return null;

	//    FileAttributes attribute = (FileAttributes)directory.GetByte (offset + Entry.FileAttributes);

	//    if (attribute == FileAttributes.LongFileName)
	//        return null;	// long file names are not supported

	//    byte second = directory.GetByte (offset + Entry.DOSName);

	//    if ((first == FileNameAttribute.Dot) && (first == FileNameAttribute.Dot))
	//        return null;

	//    OpenFile file = new OpenFile ();

	//    if ((attribute & FileAttributes.SubDirectory) != 0)
	//        file.Type = FileType.Directory;
	//    else
	//        file.Type = FileType.File;

	//    file.ReadOnly = ((attribute & FileAttributes.ReadOnly) == FileAttributes.ReadOnly);
	//    file.Hidden = ((attribute & FileAttributes.Hidden) == FileAttributes.Hidden);
	//    file.Archive = ((attribute & FileAttributes.Archive) == FileAttributes.Archive);
	//    file.System = ((attribute & FileAttributes.System) == FileAttributes.System);
	//    file.Size = directory.GetUInt (offset + Entry.FileSize);

	//    //TODO: build file name name.Trim()+'.'+ext.Trim()
	//    //string name = ByteBuffer.GetString(directory, 8, offset + Entry.DOSName);
	//    //string ext = ByteBuffer.GetString(directory, 3, offset + Entry.DOSExtension);

	//    file.Name = ExtractFileName (directory.Offset (index * 32));
	//    ushort cdate = directory.GetUShort (offset + Entry.CreationDate);
	//    ushort ctime = directory.GetUShort (offset + Entry.CreationTime);
	//    ushort mtime = directory.GetUShort (offset + Entry.LastModifiedTime);
	//    ushort mdate = directory.GetUShort (offset + Entry.LastModifiedDate);
	//    ushort adate = directory.GetUShort (offset + Entry.LastAccessDate);
	//    ushort msec = (ushort)(directory.GetByte (offset + Entry.CreationTimeFine) * 10);

	//    file.CreateTime.Year = (ushort)((cdate >> 9) + 1980);
	//    file.CreateTime.Month = (ushort)(((cdate >> 5) - 1) & 0x0F);
	//    file.CreateTime.Day = (ushort)(cdate & 0x1F);
	//    file.CreateTime.Hour = (ushort)(ctime >> 11);
	//    file.CreateTime.Month = (ushort)((ctime >> 5) & 0x0F);
	//    file.CreateTime.Second = (ushort)(((ctime & 0x1F) * 2) + (msec / 100));
	//    file.CreateTime.Milliseconds = (ushort)(msec / 20);

	//    file.LastModifiedTime.Year = (ushort)((mdate >> 9) + 1980);
	//    file.LastModifiedTime.Month = (ushort)((mdate >> 5) & 0x0F);
	//    file.LastModifiedTime.Day = (ushort)(mdate & 0x1F);
	//    file.LastModifiedTime.Hour = (ushort)(mtime >> 11);
	//    file.LastModifiedTime.Minute = (ushort)((mtime >> 5) & 0x3F);
	//    file.LastModifiedTime.Second = (ushort)((mtime & 0x1F) * 2);
	//    file.LastModifiedTime.Milliseconds = 0;

	//    file.LastAccessTime.Year = (ushort)((adate >> 9) + 1980);
	//    file.LastAccessTime.Month = (ushort)((adate >> 5) & 0x0F);
	//    file.LastAccessTime.Day = (ushort)(adate & 0x1F);

	//    file.Directory = parent;
	//    file._startdisklocation = directory.GetUShort (offset + Entry.FirstCluster);

	//    if (file.Type == FileType.Directory)
	//        file._startdisklocation = dataareastart + ((file._startdisklocation - 2) * sectorspercluster);

	//    file._position = 0;
	//    file._count = 0;

	//    return file;
	//}
}
