// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;

namespace Mosa.DeviceSystem.VirtIO;

public class VirtIOQueue
{
	public ushort Size { get; }

	public uint DescriptorSize { get; }

	public uint AvailableSize { get; }

	public uint UsedSize { get; }

	public Pointer Buffer { get; }

	public uint BufferSize { get; }

	private ushort index;

	public VirtIOQueue(ushort size)
	{
		Size = size;

		DescriptorSize = (uint)(16 * size);
		AvailableSize = (uint)(6 + 2 * size);
		UsedSize = (uint)(6 + 8 * size);

		// We need to allocate physical memory!
		BufferSize = DescriptorSize + AvailableSize + UsedSize;
		Buffer = GC.AllocateObject(BufferSize); // -Not a GC object

		Internal.MemorySet(Buffer, 0, BufferSize);

		for (uint i = 0; i < size - 1; i++)
			DescriptorWrite16(i, VirtIOQueueDescriptor.NextDescIdx, (ushort)(i + 1));
		DescriptorWrite16((uint)(size - 1), VirtIOQueueDescriptor.NextDescIdx, 0xFFFF); // Last entry in the list, point it to an invalid value

		index = 0;
	}

	public ushort NextDescriptor()
	{
		return index++;
	}

	public void SetHead(ushort head)
	{
		var availableIndex = AvailableRingRead16(VirtIOQueueAvailableRing.Index);

		if (availableIndex == ushort.MaxValue)
			availableIndex = 0;

		AvailableRingWrite16((uint)(VirtIOQueueAvailableRing.Rings + availableIndex % Size), head);
		AvailableRingWrite16(VirtIOQueueAvailableRing.Index, (ushort)(availableIndex + 1));

		index = 0;

		Internal.MemorySet(Buffer + AvailableSize, 0, UsedSize);
	}

	public void DescriptorWrite16(uint descriptor, uint offset, ushort value)
	{
		Buffer.Store16(descriptor * 16 + offset, value);
	}

	public void DescriptorWrite32(uint descriptor, uint offset, uint value)
	{
		Buffer.Store32(descriptor * 16 + offset, value);
	}

	public void DescriptorWrite64(uint descriptor, uint offset, ulong value)
	{
		Buffer.Store64(descriptor * 16 + offset, value);
	}

	public ushort AvailableRingRead16(uint offset)
	{
		return Buffer.Load16(DescriptorSize + offset);
	}

	public void AvailableRingWrite16(uint offset, ushort value)
	{
		Buffer.Store16(DescriptorSize + offset, value);
	}

	public ushort UsedRingRead16(uint offset)
	{
		return Buffer.Load16(AvailableSize + offset);
	}
}
