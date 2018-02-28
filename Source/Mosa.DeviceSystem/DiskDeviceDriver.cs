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
		private uint DriveNbr;

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
		public uint BlockSize { get { return diskController.GetSectorSize(DriveNbr); } }

		protected override void Initialize()
		{
			var configuration = Device.Configuration as DiskDeviceConfiguration;

			DriveNbr = configuration.DriveNbr;
			readOnly = configuration.ReadOnly;

			Device.Status = DeviceStatus.Initializing;
			Device.SubComponentID = DriveNbr;
			Device.Name = Device.Parent.Name + "/Disk" + DriveNbr.ToString();

			diskController = Device.Parent.DeviceDriver as IDiskControllerDevice;

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

			totalSectors = diskController.GetTotalSectors(DriveNbr);

			if (!readOnly)
				readOnly = diskController.CanWrite(DriveNbr);

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
			diskController.ReadBlock(DriveNbr, block, count, data);
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
			return diskController.ReadBlock(DriveNbr, block, count, data);
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
			return diskController.WriteBlock(DriveNbr, block, count, data);
		}
	}
}
