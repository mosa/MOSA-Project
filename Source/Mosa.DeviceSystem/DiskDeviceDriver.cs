// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.DeviceSystem
{
	/// <summary>
	/// Disk Device
	/// </summary>
	public class DiskDeviceDriver : BaseDeviceDriver, IDiskDevice
	{
		/// <summary>
		/// The disk controller
		/// </summary>
		private IDiskControllerDevice diskController;

		/// <summary>
		/// The drive NBR
		/// </summary>
		private uint driveNbr;

		/// <summary>
		/// The total sectors
		/// </summary>
		private uint totalSectors;

		/// <summary>
		/// The read only
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

		protected override void Initialize()
		{
			Device.Name = Device.Parent.Name + "/Disk" + driveNbr.ToString();
			Device.Status = DeviceStatus.Initializing;

			var configuration = Device.Configuration as DiskDeviceConfiguration;

			driveNbr = configuration.DriveNbr;
			readOnly = configuration.ReadOnly;

			diskController = Device.Parent as IDiskControllerDevice;

			if (diskController == null)
			{
				Device.Status = DeviceStatus.Error;
				return;
			}

			Device.Status = DeviceStatus.Available;
		}

		public override void Probe() => Device.Status = DeviceStatus.Available;

		public override void Start()
		{
			if (Device.Status != DeviceStatus.Available)
				return;

			totalSectors = diskController.GetTotalSectors(driveNbr);

			if (!readOnly)
				readOnly = diskController.CanWrite(driveNbr);

			Device.Status = DeviceStatus.Online;
		}

		public override bool OnInterrupt() => true;

		/// <summary>
		/// Reads the block.
		/// </summary>
		/// <param name="block">The block.</param>
		/// <param name="count">The count.</param>
		/// <returns></returns>
		public byte[] ReadBlock(uint block, uint count)
		{
			var data = new byte[count * BlockSize];
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
