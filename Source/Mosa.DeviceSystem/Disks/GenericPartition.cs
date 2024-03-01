// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem.Disks;

/// <summary>
/// Describes a generic partition of a disk. Used by the <see cref="MasterBootBlock"/> for listing partitions on a disk.
/// </summary>
public class GenericPartition
{
	public GenericPartition(uint index) => Index = index;

	public bool Bootable { get; set; }

	public uint Index { get; private set; }

	public uint StartLBA { get; set; }

	public uint EndLBA => StartLBA + TotalBlocks;

	public uint TotalBlocks { get; set; }

	public byte PartitionType { get; set; }

	public uint[] GUID { get; set; }
}
