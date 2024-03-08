// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Disks;
using Mosa.DeviceSystem.Framework;
using Mosa.DeviceSystem.HardwareAbstraction;
using Mosa.DeviceSystem.Misc;

namespace Mosa.DeviceDriver.ISA;

// http://www.t13.org/Documents/UploadedDocuments/docs2004/d1572r3-EDD3.pdf
// http://mirrors.josefsipek.net/www.nondot.org/sabre/os/files/Disk/IDE-tech.html

/// <summary>
/// IDE Controller
/// </summary>
//[ISADeviceDriver(AutoLoad = true, BasePort = 0x1F0, PortRange = 8, Platforms = PlatformArchitecture.X86AndX64)]
//[ISADeviceDriver(AutoLoad = false, BasePort = 0x170, PortRange = 8, ForceOption = "ide2", Platforms = PlatformArchitecture.X86AndX64)]
public class IDEController : BaseDeviceDriver, IDiskControllerDevice
{
	#region Definitions

	/// <summary>
	/// IDE Command
	/// </summary>
	private struct IDECommand
	{
		internal const byte ReadSectorsWithRetry = 0x20;
		internal const byte WriteSectorsWithRetry = 0x30;
		internal const byte IdentifyDrive = 0xEC;
	}

	private struct StatusRegister
	{
		internal const byte Busy = 1 << 7;
		internal const byte DriveReady = 1 << 6;
		internal const byte DriveWriteFault = 1 << 5;
		internal const byte DriveSeekComplete = 1 << 4;
		internal const byte DataRequest = 1 << 3;
		internal const byte CorrectedData = 1 << 2;
		internal const byte Index = 1 << 1;
		internal const byte Error = 1 << 0;
	}

	/// <summary>
	/// Identify Drive
	/// </summary>
	private struct IdentifyDrive
	{
		internal const uint GeneralConfig = 0x00;
		internal const uint LogicalCylinders = 0x02;
		internal const uint LogicalHeads = 0x08;
		internal const uint LogicalSectors = 0x06 * 2;
		internal const uint SerialNbr = 0x14;
		internal const uint ControllerType = 0x28;
		internal const uint BufferSize = 0x15 * 2;
		internal const uint FirmwareRevision = 0x17 * 2;
		internal const uint ModelNumber = 0x1B * 2;
		internal const uint SupportDoubleWord = 0x30 * 2;

		internal const uint CommandSetSupported83 = 83 * 2; // 1 word
		internal const uint MaxLBA28 = 60 * 2; // 2 words
		internal const uint MaxLBA48 = 100 * 2; // 3 words
	}

	#endregion Definitions

	/// <summary>
	/// The drives per controller
	/// </summary>
	public const uint DrivesPerController = 2; // The maximum supported

	/// <summary>
	/// The data port
	/// </summary>
	private IOPortReadWrite dataPort;

	/// <summary>
	/// The feature port
	/// </summary>
	private IOPortReadWrite featurePort;

	/// <summary>
	/// The error port
	/// </summary>
	private IOPortRead errorPort;

	/// <summary>
	/// The sector count port
	/// </summary>
	private IOPortReadWrite sectorCountPort;

	/// <summary>
	/// The lba low port
	/// </summary>
	private IOPortReadWrite lbaLowPort;

	/// <summary>
	/// The lba mid port
	/// </summary>
	private IOPortReadWrite lbaMidPort;

	/// <summary>
	/// The lba high port
	/// </summary>
	private IOPortReadWrite lbaHighPort;

	/// <summary>
	/// The device head port
	/// </summary>
	private IOPortReadWrite deviceHeadPort;

	/// <summary>
	/// The status port
	/// </summary>
	private IOPortRead statusPort;

	/// <summary>
	/// The command port
	/// </summary>
	private IOPortWrite commandPort;

	/// <summary>
	/// The bus control register port
	/// </summary>
	private IOPortWrite controlPort;

	/// <summary>
	/// The status port
	/// </summary>
	private IOPortRead altStatusPort;

	private enum AddressingMode
	{
		NotSupported,
		LBA28,
		LBA48
	}

	private enum SectorOperation
	{
		Read,
		Write
	}

	/// <summary>
	/// Drive Info
	/// </summary>
	private struct DriveInfo
	{
		/// <summary>
		/// The present
		/// </summary>
		public bool Present;

		/// <summary>
		/// The maximum lba
		/// </summary>
		public uint MaxLBA;

		/// <summary>
		/// The lba type
		/// </summary>
		public AddressingMode AddressingMode;
	}

	/// <summary>
	/// The drive information
	/// </summary>
	private readonly DriveInfo[] driveInfo = new DriveInfo[DrivesPerController];

	public override void Initialize()
	{
		Device.Name = "IDE_0x" + Device.Resources.IOPortRegions[0].BaseIOPort.ToString("X");
		Device.ComponentID = Device.Resources.IOPortRegions[0].BaseIOPort;

		dataPort = Device.Resources.GetIOPortReadWrite(0, 0);
		errorPort = Device.Resources.GetIOPortRead(0, 1);
		featurePort = Device.Resources.GetIOPortReadWrite(0, 1);
		sectorCountPort = Device.Resources.GetIOPortReadWrite(0, 2);
		lbaLowPort = Device.Resources.GetIOPortReadWrite(0, 3);
		lbaMidPort = Device.Resources.GetIOPortReadWrite(0, 4);
		lbaHighPort = Device.Resources.GetIOPortReadWrite(0, 5);
		deviceHeadPort = Device.Resources.GetIOPortReadWrite(0, 6);
		commandPort = Device.Resources.GetIOPortWrite(0, 7);
		statusPort = Device.Resources.GetIOPortRead(0, 7);
		controlPort = Device.Resources.GetIOPortWrite(1, 0);
		altStatusPort = Device.Resources.GetIOPortRead(1, 6);

		MaximumDriveCount = 2;

		for (var drive = 0; drive < DrivesPerController; drive++)
		{
			driveInfo[drive].Present = false;
			driveInfo[drive].MaxLBA = 0;
		}
	}

	public override void Probe()
	{
		lbaLowPort.Write8(0x88);
		var found = lbaLowPort.Read8() == 0x88;

		Device.Status = found ? DeviceStatus.Available : DeviceStatus.NotFound;
	}

	public override void Start()
	{
		controlPort.Write8(0);

		for (byte drive = 0; drive < MaximumDriveCount; drive++)
			DoIdentifyDrive(drive);

		Device.Status = DeviceStatus.Online;
	}

	private void DoIdentifyDrive(byte index)
	{
		driveInfo[index].Present = false;

		//Send the identify command to the selected drive
		deviceHeadPort.Write8((byte)(index == 0 ? 0xA0 : 0xB0));
		sectorCountPort.Write8(0);
		lbaLowPort.Write8(0);
		lbaMidPort.Write8(0);
		lbaHighPort.Write8(0);
		commandPort.Write8(IDECommand.IdentifyDrive);

		if (statusPort.Read8() == 0)
			return; // Drive doesn't exist

		// Wait until a ready status is present
		if (!WaitForReadyStatus())
			return; // There's no ready status, this drive doesn't exist

		// Check if the drive is ATA
		if (lbaMidPort.Read8() != 0 && lbaHighPort.Read8() != 0)
			return; // In this case the drive is ATAPI

		// Wait until the identify data is present (256x16 bits)
		if (!WaitForIdentifyData())
			return; // ID Error

		// An ATA drive is present
		driveInfo[index].Present = true;

		// Read the identification info
		var info = new DataBlock(512);
		for (var ix = 0U; ix < 256; ix++)
			info.SetUShort(ix * 2, dataPort.Read16());

		// Find the addressing mode
		var lba28SectorCount = info.GetUInt32(IdentifyDrive.MaxLBA28);

		AddressingMode aMode = AddressingMode.NotSupported;
		if ((info.GetUShort(IdentifyDrive.CommandSetSupported83) & 0x200) == 0x200) // Check the LBA48 support bit
		{
			aMode = AddressingMode.LBA48;
			driveInfo[index].MaxLBA = info.GetUInt32(IdentifyDrive.MaxLBA48);
		}
		else if (lba28SectorCount > 0) // LBA48 not supported, check LBA28
		{
			aMode = AddressingMode.LBA28;
			driveInfo[index].MaxLBA = lba28SectorCount;
		}

		driveInfo[index].AddressingMode = aMode;
	}

	/// <summary>
	/// Called when an interrupt is received.
	/// </summary>
	/// <returns></returns>
	public override bool OnInterrupt() => true;

	/// <summary>
	/// Waits for register ready.
	/// </summary>
	/// <returns>True if the drive is ready.</returns>
	private bool WaitForReadyStatus()
	{
		byte status;
		do
		{
			status = statusPort.Read8();
		}
		while ((status & StatusRegister.Busy) == StatusRegister.Busy);

		return true;

		// TODO: Timeout -> return false
	}

	/// <summary>
	/// Waits for the selected drive to send the identify data.
	/// </summary>
	/// <returns>True if the data is received, False if an error ocurred.</returns>
	private bool WaitForIdentifyData()
	{
		byte status;
		do
		{
			status = statusPort.Read8();
		}
		while ((status & StatusRegister.DataRequest) != StatusRegister.DataRequest && (status & StatusRegister.Error) != StatusRegister.Error);

		return (status & StatusRegister.Error) != StatusRegister.Error;
	}

	/// <summary>
	/// Send a CacheFlush (0xE7) command to the selected drive.
	/// </summary>
	/// <returns>True if the cache flush command is successful, false if not.</returns>
	private bool DoCacheFlush()
	{
		commandPort.Write8(0xE7);
		return WaitForReadyStatus();
	}

	/// <summary>
	/// Performs the LBA28.
	/// </summary>
	/// <param name="operation">The operation.</param>
	/// <param name="drive">The drive NBR.</param>
	/// <param name="lba">The lba.</param>
	/// <param name="data">The data.</param>
	/// <param name="offset">The offset.</param>
	/// <returns></returns>
	private bool PerformLBA28(SectorOperation operation, uint drive, uint lba, byte[] data, uint offset)
	{
		if (drive >= MaximumDriveCount || !driveInfo[drive].Present)
			return false;

		deviceHeadPort.Write8((byte)(0xE0 | (drive << 4) | ((lba >> 24) & 0x0F)));
		featurePort.Write8(0);
		sectorCountPort.Write8(1);
		lbaHighPort.Write8((byte)((lba >> 16) & 0xFF));
		lbaMidPort.Write8((byte)((lba >> 8) & 0xFF));
		lbaLowPort.Write8((byte)(lba & 0xFF));

		commandPort.Write8(operation == SectorOperation.Write ? IDECommand.WriteSectorsWithRetry : IDECommand.ReadSectorsWithRetry);

		if (!WaitForReadyStatus())
			return false;

		var sector = new DataBlock(data);

		if (operation == SectorOperation.Read)
		{
			for (var index = 0U; index < 256; index++)
				sector.SetUShort(offset + index * 2, dataPort.Read16());
		}
		else
		{
			// NOTE: Transferring 16bits at a time seems to fail(?) to write each second 16bits - transferring 32bits seems to fi
			// this (???)
			for (var index = 0U; index < 128; index++)
				dataPort.Write32(sector.GetUInt32(offset + index * 4));

			DoCacheFlush();
		}

		return true;
	}

	/// <summary>
	/// Reads the LBA48.
	/// </summary>
	/// <param name="operation">The operation.</param>
	/// <param name="drive">The drive.</param>
	/// <param name="lba">The lba.</param>
	/// <param name="data">The data.</param>
	/// <param name="offset">The offset.</param>
	/// <returns></returns>
	private bool PerformLBA48(SectorOperation operation, uint drive, uint lba, byte[] data, uint offset)
	{
		if (drive >= MaximumDriveCount || !driveInfo[drive].Present)
			return false;

		deviceHeadPort.Write8((byte)(0x40 | (drive << 4)));
		sectorCountPort.Write8(0);

		lbaLowPort.Write8((byte)((lba >> 24) & 0xFF));
		lbaMidPort.Write8((byte)((lba >> 32) & 0xFF));
		lbaHighPort.Write8((byte)((lba >> 40) & 0xFF));

		sectorCountPort.Write8(1);

		lbaLowPort.Write8((byte)(lba & 0xFF));
		lbaMidPort.Write8((byte)((lba >> 8) & 0xFF));
		lbaHighPort.Write8((byte)((lba >> 16) & 0xFF));

		featurePort.Write8(0);
		featurePort.Write8(0);

		commandPort.Write8((byte)(operation == SectorOperation.Write ? 0x34 : 0x24));

		if (!WaitForReadyStatus())
			return false;

		var sector = new DataBlock(data);

		if (operation == SectorOperation.Read)
		{
			for (var index = 0U; index < 256; index++)
			{
				sector.SetUShort(offset + index * 2, dataPort.Read16());
			}
		}
		else
		{
			for (var index = 0U; index < 128; index++)
			{
				dataPort.Write32(sector.GetUInt32(offset + index * 4));
			}

			//Cache flush
			DoCacheFlush();
		}

		return true;
	}

	#region IDiskControllerDevice

	/// <summary>
	/// Gets the maximum drive count.
	/// </summary>
	/// <value>The drive count.</value>
	public uint MaximumDriveCount { get; private set; }

	/// <summary>
	/// Opens the specified drive.
	/// </summary>
	/// <param name="drive">The drive.</param>
	/// <returns></returns>
	public bool Open(uint drive) => drive < MaximumDriveCount && driveInfo[drive].Present;

	/// <summary>
	/// Releases the specified drive.
	/// </summary>
	/// <param name="drive">The drive.</param>
	/// <returns></returns>
	public bool Release(uint drive) => true;

	/// <summary>
	/// Gets the size of the sector.
	/// </summary>
	/// <param name="drive">The drive NBR.</param>
	/// <returns></returns>
	public uint GetSectorSize(uint drive) => 512;

	/// <summary>
	/// Gets the total sectors.
	/// </summary>
	/// <param name="drive">The drive NBR.</param>
	/// <returns></returns>
	public uint GetTotalSectors(uint drive) => drive >= MaximumDriveCount || !driveInfo[drive].Present ? 0 : driveInfo[drive].MaxLBA;

	/// <summary>
	/// Determines whether this instance can write to the specified drive.
	/// </summary>
	/// <param name="drive">The drive NBR.</param>
	/// <returns>
	/// 	<c>true</c> if this instance can write to the specified drive; otherwise, <c>false</c>.
	/// </returns>
	public bool CanWrite(uint drive) => true; // TODO

	/// <summary>
	/// Reads the block.
	/// </summary>
	/// <param name="drive">The drive NBR.</param>
	/// <param name="block">The block.</param>
	/// <param name="count">The count.</param>
	/// <param name="data">The data.</param>
	/// <returns></returns>
	public bool ReadBlock(uint drive, uint block, uint count, byte[] data)
	{
		if (drive >= MaximumDriveCount || !driveInfo[drive].Present)
			return false;

		if (data.Length < count * 512)
			return false;

		lock (DriverLock)
		{
			for (var index = 0U; index < count; index++)
				switch (driveInfo[drive].AddressingMode)
				{
					case AddressingMode.LBA28:
						{
							if (!PerformLBA28(SectorOperation.Read, drive, block + index, data, index * 512))
								return false;

							break;
						}
					case AddressingMode.LBA48:
						{
							if (!PerformLBA48(SectorOperation.Read, drive, block + index, data, index * 512))
								return false;

							break;
						}
				}

			return true;
		}
	}

	/// <summary>
	/// Writes the block.
	/// </summary>
	/// <param name="drive">The drive NBR.</param>
	/// <param name="block">The block.</param>
	/// <param name="count">The count.</param>
	/// <param name="data">The data.</param>
	/// <returns></returns>
	public bool WriteBlock(uint drive, uint block, uint count, byte[] data)
	{
		if (drive >= MaximumDriveCount || !driveInfo[drive].Present)
			return false;

		if (data.Length < count * 512)
			return false;

		lock (DriverLock)
		{
			for (var index = 0U; index < count; index++)
				switch (driveInfo[drive].AddressingMode)
				{
					case AddressingMode.LBA28:
						{
							if (!PerformLBA28(SectorOperation.Write, drive, block + index, data, index * 512))
								return false;

							break;
						}
					case AddressingMode.LBA48:
						{
							if (!PerformLBA48(SectorOperation.Write, drive, block + index, data, index * 512))
								return false;

							break;
						}
				}

			return true;
		}
	}

	#endregion IDiskControllerDevice
}
