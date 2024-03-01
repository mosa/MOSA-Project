// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.Disks;

/// <summary>
/// Describes the partition type for a <see cref="MasterBootBlock"/>.
/// </summary>
public struct PartitionType
{
	public const byte Empty = 0x00;
	public const byte GPT = 0xEE;
	public const byte ExtendedPartition = 0x0F;
	public const byte OldExtendedPartition = 0x05; // Limited to disks under 8.4Gb
	public const byte FAT12 = 0x01;
	public const byte FAT16 = 0x04;
	public const byte FAT32 = 0x0B;
}
