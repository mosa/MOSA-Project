/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

namespace Mosa.DeviceDrivers
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
		private uint start;

        /// <summary>
        /// 
        /// </summary>
		private uint blockCount;

        /// <summary>
        /// 
        /// </summary>
		private bool readOnly;

        /// <summary>
        /// 
        /// </summary>
		public uint BlockCount { get { return blockCount; } }

        /// <summary>
        /// 
        /// </summary>
		public uint BlockSize { get { return diskDevice.BlockSize; } }

        /// <summary>
        /// 
        /// </summary>
		public bool CanWrite { get { return !readOnly; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partition"></param>
        /// <param name="diskDevice"></param>
        /// <param name="readOnly"></param>
		public PartitionDevice(GenericPartition partition, IDiskDevice diskDevice, bool readOnly)
		{
			this.diskDevice = diskDevice;
			this.start = partition.StartLBA;
			this.blockCount = partition.TotalBlocks;
			this.readOnly = readOnly;

			base.parent = diskDevice as Device;
			base.name = base.parent.Name + "/Partition" + (partition.Index + 1).ToString();	// need to give it a unique name
			base.deviceStatus = DeviceStatus.Online;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="block"></param>
        /// <param name="count"></param>
        /// <returns></returns>
		public byte[] ReadBlock(uint block, uint count)
		{
			return diskDevice.ReadBlock(block + start, count);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="block"></param>
        /// <param name="count"></param>
        /// <param name="data"></param>
        /// <returns></returns>
		public bool ReadBlock(uint block, uint count, byte[] data)
		{
			return diskDevice.ReadBlock(block + start, count, data);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="block"></param>
        /// <param name="count"></param>
        /// <param name="data"></param>
        /// <returns></returns>
		public bool WriteBlock(uint block, uint count, byte[] data)
		{
			return diskDevice.WriteBlock(block + start, count, data);
		}

	}
}
