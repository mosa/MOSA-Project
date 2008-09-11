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
    /// <summary>
    /// 
    /// </summary>
	public class DiskDevice : Device, IDiskDevice, IDevice
	{
        /// <summary>
        /// 
        /// </summary>
		private IDiskControllerDevice diskController;

        /// <summary>
        /// 
        /// </summary>
		private uint driveNbr;

        /// <summary>
        /// 
        /// </summary>
		private uint totalSectors;

        /// <summary>
        /// 
        /// </summary>
		private MasterBootBlock mbr;

        /// <summary>
        /// 
        /// </summary>
		private bool readOnly;

        /// <summary>
        /// 
        /// </summary>
		public bool CanWrite { get { return !readOnly; } }

        /// <summary>
        /// 
        /// </summary>
		public uint TotalBlocks { get { return totalSectors; } }

        /// <summary>
        /// 
        /// </summary>
		public uint BlockSize { get { return diskController.GetSectorSize(driveNbr); } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="partitionNbr"></param>
        /// <returns></returns>
		public GenericPartition this[uint partitionNbr] { get { return mbr[partitionNbr]; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="DiskDevice"/> class.
		/// </summary>
		/// <param name="diskController">The disk controller.</param>
		/// <param name="driveNbr">The drive number.</param>
		/// <param name="readOnly">if set to <c>true</c> [read only].</param>
		public DiskDevice(IDiskControllerDevice diskController, uint driveNbr, bool readOnly)
		{
			base.parent = diskController as Device;
			base.name = base.parent.Name + "/Disk" + driveNbr.ToString();
			base.deviceStatus = DeviceStatus.Online;
			this.totalSectors = diskController.GetTotalSectors(driveNbr);
			this.diskController = diskController;
			this.driveNbr = driveNbr;

			if (readOnly)
				this.readOnly = true;
			else
				this.readOnly = this.diskController.CanWrite(driveNbr);

			mbr = new MasterBootBlock(this);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="block"></param>
        /// <param name="count"></param>
        /// <returns></returns>
		public byte[] ReadBlock(uint block, uint count)
		{
			byte[] data = new byte[count * BlockSize];
			diskController.ReadBlock(driveNbr, block, count, data);
			return data;
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
			return diskController.ReadBlock(driveNbr, block, count, data);
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
			return diskController.WriteBlock(driveNbr, block, count, data);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
		public LinkedList<IDevice> CreatePartitionDevices()
		{
			LinkedList<IDevice> devices = new LinkedList<IDevice>();

			if (mbr.Valid)
				for (uint i = 0; i < MasterBootBlock.MaxMBRPartitions; i++)
					if (mbr[i].PartitionType != PartitionTypes.Empty)
						devices.Add(new PartitionDevice(mbr[i], this, readOnly));

			return devices;
		}
	}
}
