// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Text;
using Mosa.DeviceSystem;

namespace Mosa.Utility.BootImage;

//https://en.wikipedia.org/wiki/GUID_Partition_Table
public class GuidPartitionTable
{
	private const uint HeaderSize = 0x5C; // 92 bytes
	private const uint PartitionEntrySize = 0x80; // 128 bytes

	private readonly IDiskDevice diskDevice;

	private static readonly byte[] Signature = { 0x45, 0x46, 0x49, 0x20, 0x50, 0x41, 0x52, 0x54 }; // "EFI PART"
	private static readonly byte[] Revision = { 0x00, 0x00, 0x01, 0x00 }; // Revision 1.0 (UEFI 2.8)

	public GuidPartitionTable(IDiskDevice diskDevice)
	{
		this.diskDevice = diskDevice;
	}

	public void Write()
	{
		var efiSystemPartition = new DataBlock(diskDevice.BlockSize);
		efiSystemPartition.SetBytes(0, Guid.Parse("C12A7328-F81F-11D2-BA4B-00A0C93EC93B").ToByteArray());
		efiSystemPartition.SetBytes(16, Guid.NewGuid().ToByteArray());
		efiSystemPartition.SetULong(32, 3);
		efiSystemPartition.SetULong(40, diskDevice.TotalBlocks - 1);
		efiSystemPartition.SetULong(40, 0b0000000000000000000000000000000000000000000000000000000000000000);
		efiSystemPartition.SetBytes(56, Encoding.Unicode.GetBytes("MOSA-PROJECT-POWERED-BY-DO"));

		var partitionEntries = new DataBlock[1];
		partitionEntries[0] = efiSystemPartition;

		var partitionTableHeader = new DataBlock(diskDevice.BlockSize);
		partitionTableHeader.SetBytes(0, Signature);
		partitionTableHeader.SetBytes(8, Revision);
		partitionTableHeader.SetUInt32(12, HeaderSize);
		partitionTableHeader.SetUInt32(16, 0); // CRC32 checksum, zeroed during calculation
		partitionTableHeader.SetUInt32(20, 0); // Reserved, must be 0
		partitionTableHeader.SetULong(24, 1); // Current LBA
		partitionTableHeader.SetULong(32, 1); // TODO: Backup LBA
		partitionTableHeader.SetULong(40, 3); // TODO: First usable LBA
		partitionTableHeader.SetULong(48, diskDevice.TotalBlocks - 1); // TODO: Last usable LBA
		partitionTableHeader.SetBytes(56, Guid.NewGuid().ToByteArray()); // Disk GUID
		partitionTableHeader.SetULong(72, 2); // Starting LBA of array of partition entries
		partitionTableHeader.SetUInt32(80, (uint)partitionEntries.Length); // Number of partition entries in array
		partitionTableHeader.SetUInt32(84, PartitionEntrySize); // Size of a single partition entry
		partitionTableHeader.SetUInt32(88, 0); // TODO: CRC32 checksum of partition entries array

		for (uint offset = 92; offset < partitionTableHeader.Length; offset++)
			partitionTableHeader.SetByte(offset, 0);

		diskDevice.WriteBlock(1, 1, partitionTableHeader.Data);

		for (uint i = 0; i < partitionEntries.Length; i++)
			diskDevice.WriteBlock(i + 2, 1, partitionEntries[i].Data);
	}
}
