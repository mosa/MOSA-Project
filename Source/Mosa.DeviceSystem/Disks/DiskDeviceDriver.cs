// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Framework;

namespace Mosa.DeviceSystem.Disks;

/// <summary>
/// A device driver for physical disks. It implements the <see cref="IDiskDevice"/> interface.
/// </summary>
public class DiskDeviceDriver : BaseDeviceDriver, IDiskDevice
{
	private IDiskControllerDevice diskController;

	private uint DriveNbr;

	private bool readOnly;

	public bool CanWrite => !readOnly;

	public uint TotalBlocks { get; private set; }

	public uint BlockSize => diskController.GetSectorSize(DriveNbr);

	public override void Initialize()
	{
		if (Device.Configuration is not DiskDeviceConfiguration configuration)
		{
			Device.Status = DeviceStatus.Error;
			return;
		}

		DriveNbr = configuration.DriveNbr;
		readOnly = configuration.ReadOnly;

		Device.ComponentID = DriveNbr;
		Device.Name = Device.Parent.Name + "/Disk" + DriveNbr;

		diskController = Device.Parent.DeviceDriver as IDiskControllerDevice;
		if (diskController == null)
			Device.Status = DeviceStatus.Error;
	}

	public override void Probe() => Device.Status = DeviceStatus.Available;

	public override void Start()
	{
		TotalBlocks = diskController.GetTotalSectors(DriveNbr);

		if (!readOnly)
			readOnly = !diskController.CanWrite(DriveNbr);

		Device.Status = DeviceStatus.Online;
	}

	public override bool OnInterrupt() => true;

	public byte[] ReadBlock(uint block, uint count)
	{
		var data = new byte[count * BlockSize];
		diskController.ReadBlock(DriveNbr, block, count, data);
		return data;
	}

	public bool ReadBlock(uint block, uint count, byte[] data) => diskController.ReadBlock(DriveNbr, block, count, data);

	public bool WriteBlock(uint block, uint count, byte[] data) => diskController.WriteBlock(DriveNbr, block, count, data);
}
