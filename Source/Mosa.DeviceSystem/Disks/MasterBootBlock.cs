// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Misc;

namespace Mosa.DeviceSystem.Disks;

#region Definitions

internal struct MBR
{
	internal const uint CodeArea = 0x00;
	internal const uint DiskSignature = 0x01B8;
	internal const uint FirstPartition = 0x01BE;
	internal const uint MBRSignature = 0x01FE;
}

internal struct MBRConstant
{
	internal const ushort MBRSignature = 0xAA55;
	internal const byte Bootable = 0x80;
	internal const ushort CodeAreaSize = 446;
	internal const byte PartitionSize = 16;
}

internal struct PartitionRecord
{
	internal const uint Status = 0x00; // 1
	internal const uint FirstCRS = 0x01; // 3
	internal const uint PartitionType = 0x04;   // 1
	internal const uint LastCRS = 0x05; // 3
	internal const uint LBA = 0x08; // 4
	internal const uint Sectors = 0x0C; // 4
}

#endregion Definitions

/// <summary>
/// Describes the MBR (Master Boot Record) block on a disk.
/// </summary>
public class MasterBootBlock
{
	public const uint MaxPartitions = 4;

	public bool Valid { get; private set; }

	public uint DiskSignature { get; set; }

	public GenericPartition[] Partitions { get; }

	public DataBlock DataBlock { get; set; }

	public byte[] Code
	{
		get
		{
			if (code == null)
				return null;

			var copy = new byte[code.Length];
			for (var i = 0; i < code.Length; i++)
				copy[i] = code[i];

			return copy;
		}
		set
		{
			if (value == null)
			{
				code = null;
				return;
			}

			code = new byte[value.Length];
			for (var i = 0; i < value.Length; i++)
				code[i] = value[i];
		}
	}

	private readonly IDiskDevice diskDevice;

	private byte[] data, code;

	public MasterBootBlock(IDiskDevice diskDevice)
	{
		this.diskDevice = diskDevice;

		Partitions = new GenericPartition[MaxPartitions];
		for (uint i = 0; i < MaxPartitions; i++)
			Partitions[i] = new GenericPartition(i);

		Read();
	}

	public bool Read()
	{
		Valid = false;

		if (diskDevice.BlockSize != 512 || diskDevice.TotalBlocks < 3)
			return false; // Only going to work with 512 sector sizes and disks more than 2 blocks

		data = diskDevice.ReadBlock(0, 1);
		DataBlock = new DataBlock(data);

		if (DataBlock.GetUShort(MBR.MBRSignature) != MBRConstant.MBRSignature)
			return false;

		Valid = true;

		DiskSignature = DataBlock.GetUInt32(MBR.DiskSignature);

		for (uint index = 0; index < MaxPartitions; index++)
		{
			var offset = MBR.FirstPartition + index * MBRConstant.PartitionSize;

			Partitions[index] = new GenericPartition(index)
			{
				Bootable = DataBlock.GetByte(offset + PartitionRecord.Status) == MBRConstant.Bootable,
				PartitionType = DataBlock.GetByte(offset + PartitionRecord.PartitionType),
				StartLBA = DataBlock.GetUInt32(offset + PartitionRecord.LBA),
				TotalBlocks = DataBlock.GetUInt32(offset + PartitionRecord.Sectors)
			};
		}

		// TODO: Extended Partitions

		code = new byte[MBRConstant.CodeAreaSize];
		for (var index = 0U; index < MBRConstant.CodeAreaSize; index++)
			code[index] = DataBlock.GetByte(index);

		return Valid;
	}

	public bool Write()
	{
		if (!diskDevice.CanWrite)
			return false;

		var block = new DataBlock(512);

		block.SetUInt32(MBR.DiskSignature, DiskSignature);
		block.SetUShort(MBR.MBRSignature, MBRConstant.MBRSignature);

		if (code != null)
			for (var index = 0U; index < MBRConstant.CodeAreaSize && index < code.Length; index++)
				block.SetByte(index, code[index]);

		for (var index = 0U; index < MaxPartitions; index++)
		{
			if (Partitions[index].TotalBlocks == 0)
				continue;

			var offset = MBR.FirstPartition + index * 16;
			block.SetByte(offset + PartitionRecord.Status, (byte)(Partitions[index].Bootable ? 0x80 : 0x00));
			block.SetByte(offset + PartitionRecord.PartitionType, Partitions[index].PartitionType);
			block.SetUInt32(offset + PartitionRecord.LBA, Partitions[index].StartLBA);
			block.SetUInt32(offset + PartitionRecord.Sectors, Partitions[index].TotalBlocks);

			var diskGeometry = new DiskGeometry();
			diskGeometry.GuessGeometry(diskDevice.TotalBlocks);

			var chsStart = new CHS(diskGeometry, Partitions[index].StartLBA);
			var chsEnd = new CHS(diskGeometry, Partitions[index].StartLBA + Partitions[index].TotalBlocks - 1);

			block.SetByte(offset + PartitionRecord.FirstCRS, chsStart.Head);
			block.SetByte(offset + PartitionRecord.FirstCRS + 1, (byte)((chsStart.Sector & 0x3F) | ((chsStart.Cylinder >> 8) & 0x03)));
			block.SetByte(offset + PartitionRecord.FirstCRS + 2, (byte)(chsStart.Cylinder & 0xFF));
			block.SetByte(offset + PartitionRecord.LastCRS, chsEnd.Head);
			block.SetByte(offset + PartitionRecord.LastCRS + 1, (byte)((chsEnd.Sector & 0x3F) | ((chsEnd.Cylinder >> 8) & 0x03)));
			block.SetByte(offset + PartitionRecord.LastCRS + 2, (byte)(chsEnd.Cylinder & 0xFF));
		}

		diskDevice.WriteBlock(0, 1, block.Data);
		return true;
	}
}
