/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceSystem
{
    /// <summary>
    /// 
    /// </summary>
	public class PartitionDevice : Device, IDevice, IPartitionDevice
	{
        /// <summary>
        /// 
        /// </summary>
		private IDiskDevice diskDevice;

        /// <summary>
        /// 
        /// </summary>
		private uint startBlock;

        /// <summary>
        /// 
        /// </summary>
		private uint blockCount;

        /// <summary>
        /// 
        /// </summary>
		private bool readOnly;

		/// <summary>
		/// Gets the start block.
		/// </summary>
		/// <value>The start block.</value>
		public uint StartBlock { get { return startBlock; } }

		/// <summary>
		/// Gets the block count.
		/// </summary>
		/// <value>The block count.</value>
		public uint BlockCount { get { return blockCount; } }

		/// <summary>
		/// Gets the size of the block.
		/// </summary>
		/// <value>The size of the block.</value>
		public uint BlockSize { get { return diskDevice.BlockSize; } }

		/// <summary>
		/// Gets a value indicating whether this instance can write.
		/// </summary>
		/// <value><c>true</c> if this instance can write; otherwise, <c>false</c>.</value>
		public bool CanWrite { get { return !readOnly; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="PartitionDevice"/> class.
		/// </summary>
		/// <param name="diskDevice">The disk device.</param>
		/// <param name="readOnly">if set to <c>true</c> [read only].</param>
		public PartitionDevice(IDiskDevice diskDevice, bool readOnly)
		{
			this.diskDevice = diskDevice;
			this.startBlock = 0;
			this.blockCount = diskDevice.TotalBlocks;
			this.readOnly = readOnly;

			base.parent = diskDevice as Device;
			base.name = base.parent.Name + "/Raw";	// need to give it a unique name
			base.deviceStatus = DeviceStatus.Online;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PartitionDevice"/> class.
		/// </summary>
		/// <param name="diskDevice">The disk device.</param>
		/// <param name="partition">The partition.</param>
		/// <param name="readOnly">if set to <c>true</c> [read only].</param>
		public PartitionDevice(IDiskDevice diskDevice, GenericPartition partition, bool readOnly)
		{
			this.diskDevice = diskDevice;
			this.startBlock = partition.StartLBA;
			this.blockCount = partition.TotalBlocks;
			this.readOnly = readOnly;

			base.parent = diskDevice as Device;
			base.name = base.parent.Name + "/Partition" + (partition.Index + 1).ToString();	// need to give it a unique name
			base.deviceStatus = DeviceStatus.Online;
		}

		/// <summary>
		/// Reads the block.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="count">The count.</param>
		/// <returns></returns>
		public byte[] ReadBlock(uint block, uint count)
		{
			return diskDevice.ReadBlock(block + startBlock, count);
		}

		/// <summary>
		/// Reads the block.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="count">The count.</param>
		/// <param name="data">The data.</param>
		/// <returns></returns>
		public bool ReadBlock(uint block, uint count, byte[] data)
		{
			return diskDevice.ReadBlock(block + startBlock, count, data);
		}

		/// <summary>
		/// Writes the block.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="count">The count.</param>
		/// <param name="data">The data.</param>
		/// <returns></returns>
		public bool WriteBlock(uint block, uint count, byte[] data)
		{
			return diskDevice.WriteBlock(block + startBlock, count, data);
		}

	}
}
