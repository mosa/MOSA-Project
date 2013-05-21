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
		private bool readOnly;

		/// <summary>
		/// Gets a value indicating whether this instance can write.
		/// </summary>
		/// <value><c>true</c> if this instance can write; otherwise, <c>false</c>.</value>
		public bool CanWrite { get { return !readOnly; } }

		/// <summary>
		/// Gets the total blocks.
		/// </summary>
		/// <value>The total blocks.</value>
		public uint TotalBlocks { get { return totalSectors; } }

		/// <summary>
		/// Gets the size of the block.
		/// </summary>
		/// <value>The size of the block.</value>
		public uint BlockSize { get { return diskController.GetSectorSize(driveNbr); } }

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
		}

		/// <summary>
		/// Reads the block.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="count">The count.</param>
		/// <returns></returns>
		public byte[] ReadBlock(uint block, uint count)
		{
			byte[] data = new byte[count * BlockSize];
			diskController.ReadBlock(driveNbr, block, count, data);
			return data;
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
			return diskController.ReadBlock(driveNbr, block, count, data);
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
			return diskController.WriteBlock(driveNbr, block, count, data);
		}
	}
}