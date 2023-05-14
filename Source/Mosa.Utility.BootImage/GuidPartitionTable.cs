// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.IO.Hashing;
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
		var tableHeaderLba = 1U;
		var partitionEntriesLba = 2U;
		var backupTableHeaderLba = diskDevice.TotalBlocks - 1; // Last LBA
		var backupPartitionEntriesLba = diskDevice.TotalBlocks - 2;

		// EFI System Partition
		var efiSystemPartition = new DataBlock(diskDevice.BlockSize);
		efiSystemPartition.SetBytes(0, Guid.Parse("C12A7328-F81F-11D2-BA4B-00A0C93EC93B").ToByteArray());
		efiSystemPartition.SetBytes(16, Guid.NewGuid().ToByteArray());
		efiSystemPartition.SetULong(32, partitionEntriesLba + 1);
		efiSystemPartition.SetULong(40, backupPartitionEntriesLba - 1);
		efiSystemPartition.SetULong(48, 0b0000000000000000000000000000000000000000000000000000000000000000);
		efiSystemPartition.SetBytes(56, Encoding.Unicode.GetBytes("MOSA-PROJECT-POWERED-BY-DO"));

		var partitionEntries = new DataBlock[1];
		partitionEntries[0] = efiSystemPartition;

		var partitionEntriesCrc32 = Crc32.Hash(efiSystemPartition.Data[..128]);

		// Partition table header
		var partitionTableHeader = new DataBlock(diskDevice.BlockSize);
		partitionTableHeader.SetBytes(0, Signature);
		partitionTableHeader.SetBytes(8, Revision);
		partitionTableHeader.SetUInt32(12, HeaderSize);
		partitionTableHeader.SetUInt32(16, 0); // CRC32 checksum, zeroed during calculation
		partitionTableHeader.SetUInt32(20, 0); // Reserved
		partitionTableHeader.SetULong(24, tableHeaderLba);
		partitionTableHeader.SetULong(32, backupTableHeaderLba);
		partitionTableHeader.SetULong(40, (uint)(partitionEntriesLba + partitionEntries.Length + 1)); // First usable LBA
		partitionTableHeader.SetULong(48, (uint)(backupPartitionEntriesLba - partitionEntries.Length - 1)); // Last usable LBA
		partitionTableHeader.SetBytes(56, Guid.NewGuid().ToByteArray()); // Disk GUID
		partitionTableHeader.SetULong(72, partitionEntriesLba);
		partitionTableHeader.SetUInt32(80, (uint)partitionEntries.Length);
		partitionTableHeader.SetUInt32(84, PartitionEntrySize);
		partitionTableHeader.SetBytes(88, partitionEntriesCrc32); // CRC32 checksum of partition entries array

		var tableHeaderCrc32 = Crc32.Hash(partitionTableHeader.Data[..0x5C]);
		partitionTableHeader.SetBytes(16, tableHeaderCrc32);

		for (uint offset = 92; offset < partitionTableHeader.Length; offset++)
			partitionTableHeader.SetByte(offset, 0);

		// Main table header
		diskDevice.WriteBlock(tableHeaderLba, 1, partitionTableHeader.Data);

		// Main entries
		for (uint i = 0; i < partitionEntries.Length; i++)
			diskDevice.WriteBlock(partitionEntriesLba + i, 1, partitionEntries[i].Data);

		// Backup entries
		for (uint i = 0; i < partitionEntries.Length; i++)
			diskDevice.WriteBlock(backupPartitionEntriesLba + i, 1, partitionEntries[i].Data);

		// Backup table header
		diskDevice.WriteBlock(backupTableHeaderLba, 1, partitionTableHeader.Data);
	}
}
