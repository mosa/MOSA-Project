/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.ClassLib;

namespace Mosa.DeviceDrivers
{
	public interface IDiskDevice
	{
		GenericPartition this[uint partitionNbr] { get; }
		bool CanWrite { get; }
		uint TotalBlocks { get; }
		uint BlockSize { get; }

		byte[] ReadBlock(uint block, uint count);
		bool ReadBlock(uint block, uint count, byte[] data);
		bool WriteBlock(uint block, uint count, byte[] data);

		LinkedList<IDevice> CreatePartitionDevices();
	}
}
