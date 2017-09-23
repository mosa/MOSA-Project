
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

		public enum AddressingMode { NotSupported, LBA28, LBA48 }

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
			public AddressingMode AddressingMode;
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
			for (byte drive = 0; drive < MaximunDriveCount; drive++)
			{
				DoIdentifyDrive(drive);
			}

			return DeviceDriverStartStatus.Started;
		}

		private void DoIdentifyDrive(byte index)
		{
			HAL.DebugWriteLine("Device " + index.ToString() + " ID...");

			driveInfo[index].Present = false;

			//Send the identify command to the selected drive
			DeviceHeadPort.Write8((byte)(index == 0 ? 0x0A : 0x0B));
			SectorCountPort.Write8(0);
			LBALowPort.Write8(0);
			LBAMidPort.Write8(0);
			LBAHighPort.Write8(0);
			CommandPort.Write8(IDECommand.IdentifyDrive);

			//Wait until a ready status is present
			if (!WaitForReadyStatus())
				return; //There's no ready status, this drive doesn't exist

			if (LBAMidPort.Read8() != 0 && LBAHighPort.Read8() != 0) //Check if the drive is ATA
			{
				//In this case the drive is ATAPI

				HAL.DebugWriteLine("Device " + index.ToString() + " not ATA");

				return;
			}

			//Wait until the identify data is present (256x16 bits)
			if (!WaitForIdentifyData())
			{
				HAL.DebugWriteLine("Device " + index.ToString() + " ID error");
				return;
			}

			//An ATA drive is present
			driveInfo[index].Present = true;

			//Read the identification info
			var info = new DataBlock(512);
			for (uint ix = 0; ix < 256; ix++)
			{
				info.SetUShort(ix * 2, DataPort.Read16());
			}

			//Find the addressing mode
			uint lba28SectorCount = info.GetUInt(IdentifyDrive.MaxLBA28);

			AddressingMode aMode = AddressingMode.NotSupported;
			if((info.GetUShort(IdentifyDrive.CommandSetSupported83) & 0x200) == 0x200) //Check the LBA48 support bit
			{
				aMode = AddressingMode.LBA48;
				driveInfo[index].MaxLBA = info.GetUInt(IdentifyDrive.MaxLBA48);
			}
			else if(lba28SectorCount > 0) //LBA48 not supported, check LBA28
			{
				aMode = AddressingMode.LBA28;
				driveInfo[index].MaxLBA = lba28SectorCount;
			}

			driveInfo[index].AddressingMode = aMode;

			HAL.DebugWriteLine("Device " + index.ToString() + " present - MaxLBA=" + driveInfo[index].MaxLBA.ToString());
		}

		/// <summary>
		/// Called when an interrupt is received.
		/// </summary>
		/// <returns></returns>
		public override bool OnInterrupt()
		{
			return true;
		}

		/// <summary>
		/// Waits for register ready.
		/// </summary>
		/// <returns>True if the drive is ready.</returns>
		private bool WaitForReadyStatus()
		{
			byte status;
			do
			{
				status = StatusPort.Read8();
			} while ((status & StatusRegister.Busy) == StatusRegister.Busy);

			return true;

			//TODO: Timeout -> return false
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
				status = StatusPort.Read8();
			} while ((status & StatusRegister.DataRequest) != StatusRegister.DataRequest && (status & StatusRegister.Error) != StatusRegister.Error);

			return ((status & StatusRegister.Error) != StatusRegister.Error);
		}

		/// <summary>
		/// Send a CacheFlush (0xE7) command to the selected drive.
		/// </summary>
		/// <returns>True if the cache flush command is successful, false if not.</returns>
		private bool DoCacheFlush()
		{
			CommandPort.Write8(0xE7);

			return WaitForReadyStatus();
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

			if (!driveInfo[drive].Present)
				return false;

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

			DeviceHeadPort.Write8((byte)(0xE0 | (drive << 4) | ((lba >> 24) & 0x0F)));
			FeaturePort.Write8(0);
			SectorCountPort.Write8(1);
			LBAHighPort.Write8((byte)((lba >> 16) & 0xFF));
			LBAMidPort.Write8((byte)((lba >> 8) & 0xFF));
			LBALowPort.Write8((byte)(lba & 0xFF));

			CommandPort.Write8((operation == SectorOperation.Write) ? IDECommand.WriteSectorsWithRetry : IDECommand.ReadSectorsWithRetry);

			if (!WaitForReadyStatus())
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
				//NOTE: Transfering 16bits at a time seems to fail(?) to write each second 16bits - transfering 32bits seems to fix this (???)
				for (uint index = 0; index < 128; index++)
				{
					DataPort.Write32(sector.GetUInt(offset + (index * 4)));
				}

				//Cache flush
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
		protected bool PerformLBA48(SectorOperation operation, uint drive, uint lba, byte[] data, uint offset)
		{
			if (drive >= MaximunDriveCount || !driveInfo[drive].Present)
				return false;

			DeviceHeadPort.Write8((byte)(0x40 | (drive << 4)));
			SectorCountPort.Write8(0);

			LBALowPort.Write8((byte)((lba >> 24) & 0xFF));
			LBAMidPort.Write8((byte)((lba >> 32) & 0xFF));
			LBAHighPort.Write8((byte)((lba >> 40) & 0xFF));

			SectorCountPort.Write8(1);

			LBALowPort.Write8((byte)(lba & 0xFF));
			LBAMidPort.Write8((byte)((lba >> 8) & 0xFF));
			LBAHighPort.Write8((byte)((lba >> 16) & 0xFF));

			FeaturePort.Write8(0);
			FeaturePort.Write8(0);

			CommandPort.Write8((byte)((operation == SectorOperation.Write) ? 0x34 : 0x24));

			if (!WaitForReadyStatus())
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
				for (uint index = 0; index < 128; index++)
				{
					DataPort.Write32(sector.GetUInt(offset + (index * 4)));
				}

				//Cache flush
				DoCacheFlush();
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
					switch (driveInfo[drive].AddressingMode)
					{
						case AddressingMode.LBA28:
							if (!PerformLBA28(SectorOperation.Read, drive, block + index, data, index * 512))
								return false;
							break;
						case AddressingMode.LBA48:
							if (!PerformLBA48(SectorOperation.Read, drive, block + index, data, index * 512))
								return false;
							break;
					}
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
					switch (driveInfo[drive].AddressingMode)
					{
						case AddressingMode.LBA28:
							if (!PerformLBA28(SectorOperation.Write, drive, block + index, data, index * 512))
								return false;
							break;
						case AddressingMode.LBA48:
							if (!PerformLBA48(SectorOperation.Write, drive, block + index, data, index * 512))
								return false;
							break;
					}
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
