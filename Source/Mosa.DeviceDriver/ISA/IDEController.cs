// Copyright (c) MOSA Project. Licensed under the New BSD License.

// References
// http://www.t13.org/Documents/UploadedDocuments/docs2004/d1572r3-EDD3.pdf
// http://mirrors.josefsipek.net/www.nondot.org/sabre/os/files/Disk/IDE-tech.html

using Mosa.ClassLib;
using Mosa.DeviceSystem;
using Mosa.HardwareSystem;

namespace Mosa.DeviceDriver.ISA
{
	/// <summary>
	///
	/// </summary>
	//[ISADeviceDriver(AutoLoad = true, BasePort = 0x1F0, PortRange = 8, Platforms = PlatformArchitecture.X86AndX64)]
	//[ISADeviceDriver(AutoLoad = false, BasePort = 0x170, PortRange = 8, ForceOption = "ide2", Platforms = PlatformArchitecture.X86AndX64)]
	public class IDEController : HardwareDevice, IDiskControllerDevice
	{
		#region Definitions

		/// <summary>
		///
		/// </summary>
		internal struct IDECommand
		{
			internal const byte ReadSectorsWithRetry = 0x20;
			internal const byte WriteSectorsWithRetry = 0x30;
			internal const byte IdentifyDrive = 0xEC;
		}

		internal struct StatusRegister
		{
			internal const byte Busy = (byte)(1 << 7);
			internal const byte DriveReady = (byte)(1 << 6);
			internal const byte DriveWriteFault = (byte)(1 << 5);
			internal const byte DriveSeekComplete = (byte)(1 << 4);
			internal const byte DataRequest = (byte)(1 << 3);
			internal const byte CorrectedData = (byte)(1 << 2);
			internal const byte Index = (byte)(1 << 1);
			internal const byte Error = (byte)(1 << 0);
		}

		/// <summary>
		///
		/// </summary>
		internal struct IdentifyDrive
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
		/// The spin lock
		/// </summary>
		protected SpinLock spinLock;

		/// <summary>
		/// The drives per conroller
		/// </summary>
		public const uint DrivesPerConroller = 2; // the maximum supported

		/// <summary>
		/// The data port
		/// </summary>
		protected IReadWriteIOPort DataPort;

		/// <summary>
		/// The feature port
		/// </summary>
		protected IReadWriteIOPort FeaturePort;

		/// <summary>
		/// The error port
		/// </summary>
		protected IReadOnlyIOPort ErrorPort;

		/// <summary>
		/// The sector count port
		/// </summary>
		protected IReadWriteIOPort SectorCountPort;

		/// <summary>
		/// The lba low port
		/// </summary>
		protected IReadWriteIOPort LBALowPort;

		/// <summary>
		/// The lba mid port
		/// </summary>
		protected IReadWriteIOPort LBAMidPort;

		/// <summary>
		/// The lba high port
		/// </summary>
		protected IReadWriteIOPort LBAHighPort;

		/// <summary>
		/// The device head port
		/// </summary>
		protected IReadWriteIOPort DeviceHeadPort;

		/// <summary>
		/// The status port
		/// </summary>
		protected IReadOnlyIOPort StatusPort;

		/// <summary>
		/// The command port
		/// </summary>
		protected IWriteOnlyIOPort CommandPort;

		/// <summary>
		/// The status port
		/// </summary>
		protected IReadOnlyIOPort AltStatusPort;

		//protected IRQHandler IdeIRQ;

		public enum LBAType { LBA28, LBA48 }

		/// <summary>
		///
		/// </summary>
		protected struct DriveInfo
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
			public LBAType LBAType;
		}

		/// <summary>
		/// The drive information
		/// </summary>
		protected DriveInfo[] driveInfo;

		/// <summary>
		/// Initializes a new instance of the <see cref="IDEController"/> class.
		/// </summary>
		public IDEController()
		{
			driveInfo = new DriveInfo[DrivesPerConroller];
		}

		/// <summary>
		/// Setups this hardware device driver
		/// </summary>
		/// <param name="hardwareResources"></param>
		/// <returns></returns>
		public override bool Setup(HardwareResources hardwareResources)
		{
			this.HardwareResources = hardwareResources;
			base.Name = "IDE_0x" + base.HardwareResources.GetIOPort(0, 0).Address.ToString("X");

			DataPort = base.HardwareResources.GetIOPort(0, 0);
			ErrorPort = base.HardwareResources.GetIOPort(0, 1);
			FeaturePort = base.HardwareResources.GetIOPort(0, 1);
			SectorCountPort = base.HardwareResources.GetIOPort(0, 2);
			LBALowPort = base.HardwareResources.GetIOPort(0, 3);
			LBAMidPort = base.HardwareResources.GetIOPort(0, 4);
			LBAHighPort = base.HardwareResources.GetIOPort(0, 5);
			DeviceHeadPort = base.HardwareResources.GetIOPort(0, 6);
			CommandPort = base.HardwareResources.GetIOPort(0, 7);
			StatusPort = base.HardwareResources.GetIOPort(0, 7);
			AltStatusPort = base.HardwareResources.GetIOPort(1, 6);

			for (var drive = 0; drive < DrivesPerConroller; drive++)
			{
				driveInfo[drive].Present = false;
				driveInfo[drive].MaxLBA = 0;
			}

			base.DeviceStatus = DeviceStatus.Online;

			return true;
		}

		/// <summary>
		/// Probes this instance.
		/// </summary>
		/// <returns></returns>
		public override bool Probe()
		{
			LBALowPort.Write8(0x88);

			return LBALowPort.Read8() == 0x88;
		}

		/// <summary>
		/// Starts this hardware device.
		/// </summary>
		/// <returns></returns>
		public override DeviceDriverStartStatus Start()
		{
			SelectDrive(0);
			driveInfo[0].Present = ((AltStatusPort.Read8() & StatusRegister.DriveReady) == StatusRegister.DriveReady);

			//SelectDrive(1);
			//driveInfo[1].Present = ((AltStatusPort.Read8() & StatusRegister.DriveReady) == StatusRegister.DriveReady);

			return DeviceDriverStartStatus.Started;
		}

		/// <summary>
		/// Called when an interrupt is received.
		/// </summary>
		/// <returns></returns>
		public override bool OnInterrupt()
		{
			return true;
		}

		private byte previousStatus = 0;

		/// <summary>
		/// Waits for register ready.
		/// </summary>
		/// <returns></returns>
		protected bool WaitUntilStatus(byte mask, byte value = 0)
		{
			// Wait 400ns
			for (var i = 0; i < 4; i++)
			{
				var status = AltStatusPort.Read8(); // This wastes 100ns
			}

			while (true)
			{
				var status = AltStatusPort.Read8();

				if (status != previousStatus)
				{
					HAL.DebugWriteLine("Status: 0x" + previousStatus.ToString("x") + " => 0x" + status.ToString("x"));
				}

				previousStatus = status;

				if ((status & mask) != value)
					return true;

				//TODO: add timeout check
			}

			//return false;
		}

		/// <summary>
		/// Selects the drive.
		/// </summary>
		/// <param name="drive">The drive.</param>
		/// <returns></returns>
		protected bool SelectDrive(byte drive)
		{
			if (!WaitUntilStatus(StatusRegister.Busy | StatusRegister.DataRequest))
				return false;

			// select drive; bit 4 is the drive selection, bits 7 and 5 are set high by spec
			DeviceHeadPort.Write8((byte)((drive == 0) ? 0xA0 : 0xB0));

			// Wait at least 400ns
			for (var i = 0; i < 4; i++)
			{
				var status = AltStatusPort.Read8(); // wastes 100ns
			}

			if (!WaitUntilStatus(StatusRegister.Busy | StatusRegister.DataRequest))
				return false;

			return true;
		}

		/// <summary>
		/// Opens the specified drive.
		/// </summary>
		/// <param name="drive">The drive.</param>
		/// <returns></returns>
		public bool Open(uint drive)
		{
			if (drive >= MaximunDriveCount || !driveInfo[drive].Present)
				return false;

			SelectDrive((byte)drive);

			CommandPort.Write8(IDECommand.IdentifyDrive);

			if (!WaitUntilStatus(StatusRegister.DataRequest))
				return false;

			var info = new DataBlock(512);

			for (uint index = 0; index < 256; index++)
			{
				var d = DataPort.Read16();
				info.SetUShort(index * 2, d);
			}

			driveInfo[drive].MaxLBA = info.GetUInt(IdentifyDrive.MaxLBA28);

			return true;
		}

		protected enum SectorOperation { Read, Write }

		/// <summary>
		/// Performs the LBA28.
		/// </summary>
		/// <param name="operation">The operation.</param>
		/// <param name="drive">The drive NBR.</param>
		/// <param name="lba">The lba.</param>
		/// <param name="data">The data.</param>
		/// <param name="offset">The offset.</param>
		/// <returns></returns>
		protected bool PerformLBA28(SectorOperation operation, uint drive, uint lba, byte[] data, uint offset)
		{
			if (drive >= MaximunDriveCount || !driveInfo[drive].Present)
				return false;

			HAL.DebugWrite("Disk/Block: ");
			HAL.DebugWrite(drive.ToString());
			HAL.DebugWrite("/");
			HAL.DebugWrite(lba.ToString());
			HAL.DebugWrite(" [");

			HAL.DebugWrite("A");
			SelectDrive((byte)drive);

			HAL.DebugWrite("B");
			DeviceHeadPort.Write8((byte)(0xE0 | (drive << 4) | ((lba >> 24) & 0x0F)));
			FeaturePort.Write8(0);
			SectorCountPort.Write8(1);
			LBALowPort.Write8((byte)(lba & 0xFF));
			LBAMidPort.Write8((byte)((lba >> 8) & 0xFF));
			LBAHighPort.Write8((byte)((lba >> 16) & 0xFF));

			CommandPort.Write8((operation == SectorOperation.Write) ? IDECommand.WriteSectorsWithRetry : IDECommand.ReadSectorsWithRetry);

			HAL.DebugWrite("C");

			if (!WaitUntilStatus(StatusRegister.Busy))
			{
				HAL.DebugWriteLine("error");
				return false;
			}

			HAL.DebugWriteLine("]");

			var sector = new DataBlock(data);

			//TODO: Don't use PIO
			if (operation == SectorOperation.Read)
			{
				for (uint index = 0; index < 256; index++)
				{
					var s = DataPort.Read16();
					sector.SetUShort(offset + (index * 2), s);

					if (index < 8 || index > 256 - 8)
					{
						HAL.DebugWrite(index.ToString("x"));
						HAL.DebugWrite(":");
						HAL.DebugWrite(s.ToString("x"));
						HAL.DebugWrite(" ");
					}
				}
			}
			else
			{
				for (uint index = 0; index < 256; index++)
				{
					DataPort.Write16(sector.GetUShort(offset + (index * 2)));
				}
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
		protected bool ReadLBA48(SectorOperation operation, uint drive, uint lba, byte[] data, uint offset)
		{
			if (drive >= MaximunDriveCount || !driveInfo[drive].Present)
				return false;

			FeaturePort.Write8(0);
			FeaturePort.Write8(0);

			SectorCountPort.Write8(0);
			SectorCountPort.Write8(1);

			LBALowPort.Write8((byte)((lba >> 24) & 0xFF));
			LBALowPort.Write8((byte)(lba & 0xFF));

			LBAMidPort.Write8((byte)((lba >> 32) & 0xFF));
			LBAMidPort.Write8((byte)((lba >> 8) & 0xFF));

			LBAHighPort.Write8((byte)((lba >> 40) & 0xFF));
			LBAHighPort.Write8((byte)((lba >> 16) & 0xFF));

			DeviceHeadPort.Write8((byte)(0x40 | (drive << 4)));

			CommandPort.Write8((byte)((operation == SectorOperation.Write) ? 0x34 : 0x24));

			if (!WaitUntilStatus(StatusRegister.Busy))
				return false;

			var sector = new DataBlock(data);

			//TODO: Don't use PIO
			if (operation == SectorOperation.Read)
			{
				for (uint index = 0; index < 256; index++)
				{
					sector.SetUShort(offset + (index * 2), DataPort.Read16());
				}
			}
			else
			{
				for (uint index = 0; index < 256; index++)
				{
					DataPort.Write16(sector.GetUShort(offset + (index * 2)));
				}
			}

			return true;
		}

		/// <summary>
		/// Releases the specified drive.
		/// </summary>
		/// <param name="drive">The drive.</param>
		/// <returns></returns>
		public bool Release(uint drive)
		{
			return true;
		}

		/// <summary>
		/// Gets the maximum drive count.
		/// </summary>
		/// <value>The drive count.</value>
		public uint MaximunDriveCount { get { return 2; } }

		/// <summary>
		/// Gets the size of the sector.
		/// </summary>
		/// <param name="drive">The drive NBR.</param>
		/// <returns></returns>
		public uint GetSectorSize(uint drive)
		{
			return 512;
		}

		/// <summary>
		/// Gets the total sectors.
		/// </summary>
		/// <param name="drive">The drive NBR.</param>
		/// <returns></returns>
		public uint GetTotalSectors(uint drive)
		{
			if (drive >= MaximunDriveCount || !driveInfo[drive].Present)
				return 0;

			return driveInfo[drive].MaxLBA;
		}

		/// <summary>
		/// Determines whether this instance can write to the specified drive.
		/// </summary>
		/// <param name="drive">The drive NBR.</param>
		/// <returns>
		/// 	<c>true</c> if this instance can write to the specified drive; otherwise, <c>false</c>.
		/// </returns>
		public bool CanWrite(uint drive)
		{
			//todo
			return true;
		}

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
			if (drive >= MaximunDriveCount || !driveInfo[drive].Present)
				return false;

			if (data.Length < count * 512)
				return false;

			try
			{
				spinLock.Enter();
				for (uint index = 0; index < count; index++)
				{
					if (!PerformLBA28(SectorOperation.Read, drive, block + index, data, index * 512))
						return false;
				}
				return true;
			}
			finally
			{
				spinLock.Exit();
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
			if (drive >= MaximunDriveCount || !driveInfo[drive].Present)
				return false;

			if (data.Length < count * 512)
				return false;

			try
			{
				spinLock.Enter();
				for (uint index = 0; index < count; index++)
				{
					if (!PerformLBA28(SectorOperation.Write, drive, block + index, data, index * 512))
						return false;
				}
				return true;
			}
			finally
			{
				spinLock.Exit();
			}
		}
	}
}
