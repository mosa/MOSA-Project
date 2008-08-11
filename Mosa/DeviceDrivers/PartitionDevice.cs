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

    public class PartitionDevice : Device, IPartitionDevice, IDevice
    {
        private IDiskDevice diskDevice;
        private uint start;
        private uint blockCount;
        private bool readOnly;

        public uint BlockCount { get { return blockCount; } }
        public uint BlockSize { get { return diskDevice.BlockSize; } }
        public bool CanWrite { get { return !readOnly; } }

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

        public byte[] ReadBlock(uint block, uint count)
        {
            return diskDevice.ReadBlock(block + start, count);
        }

        public bool ReadBlock(uint block, uint count, byte[] data)
        {
            return diskDevice.ReadBlock(block + start, count, data);
        }

        public bool WriteBlock(uint block, uint count, byte[] data)
        {
            return diskDevice.WriteBlock(block + start, count, data);
        }

    }
}
