﻿/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

// References
// http://www.t13.org/Documents/UploadedDocuments/docs2004/d1572r3-EDD3.pdf
// http://www.osdever.net/tutorials/lba.php
// http://www.nondot.org/sabre/os/files/Disk/IDE-tech.html

using Mosa.ClassLib;
using Mosa.DeviceSystem;

namespace Mosa.DeviceDrivers.ISA
{
	/// <summary>
	///
	/// </summary>
	[ISADeviceDriver(AutoLoad = true, BasePort = 0x1F0, PortRange = 8, Platforms = PlatformArchitecture.X86AndX64)]
	[ISADeviceDriver(AutoLoad = false, BasePort = 0x170, PortRange = 8, ForceOption = "ide2", Platforms = PlatformArchitecture.X86AndX64)]
	public class IDEController : HardwareDevice, IDevice, IHardwareDevice, IDiskControllerDevice
	{
		#region Definitions

		/// <summary>
		///
		/// </summary>
		internal struct IDECommands
		{
			internal const byte ReadSectorsWithRetry = 0x20;
			internal const byte WriteSectorsWithRetry = 0x30;
			internal const byte IdentifyDrive = 0xEC;
		};

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
			internal const uint MaxLBA48 = 100 * 2;	// 3 words
		}

		#endregion Definitions

		/// <summary>
		///
		/// </summary>
		protected SpinLock spinLock;

		/// <summary>
		///
		/// </summary>
		public const uint DrivesPerConroller = 2; // the maximum supported

		/// <summary>
		///
		/// </summary>
		protected IReadWriteIOPort DataPort;

		/// <summary>
		///
		/// </summary>
		protected IReadWriteIOPort FeaturePort;

		/// <summary>
		///
		/// </summary>
		protected IReadOnlyIOPort ErrorPort;

		/// <summary>
		///
		/// </summary>
		protected IReadWriteIOPort SectorCountPort;

		/// <summary>
		///
		/// </summary>
		protected IReadWriteIOPort LBALowPort;

		/// <summary>
		///
		/// </summary>
		protected IReadWriteIOPort LBAMidPort;

		/// <summary>
		///
		/// </summary>
		protected IReadWriteIOPort LBAHighPort;

		/// <summary>
		///
		/// </summary>
		protected IReadWriteIOPort DeviceHeadPort;

		/// <summary>
		///
		/// </summary>
		protected IReadOnlyIOPort StatusPort;

		/// <summary>
		///
		/// </summary>
		protected IWriteOnlyIOPort CommandPort;

		//protected IRQHandler IdeIRQ;

		/// <summary>
		///
		/// </summary>
		public enum LBAType
		{
			/// <summary>
			///
			/// </summary>
			LBA28,

			/// <summary>
			///
			/// </summary>
			LBA48
		}

		/// <summary>
		///
		/// </summary>
		protected struct DriveInfo
		{
			/// <summary>
			///
			/// </summary>
			public bool Present;

			/// <summary>
			///
			/// </summary>
			public uint MaxLBA;

			/// <summary>
			///
			/// </summary>
			public LBAType LBAType;
		}

		/// <summary>
		///
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
		public override bool Setup(IHardwareResources hardwareResources)
		{
			this.hardwareResources = hardwareResources;
			base.name = "IDE_0x" + base.hardwareResources.GetIOPort(0, 0).Address.ToString("X");

			DataPort = base.hardwareResources.GetIOPort(0, 0);
			ErrorPort = base.hardwareResources.GetIOPort(0, 1);
			FeaturePort = base.hardwareResources.GetIOPort(0, 1);
			SectorCountPort = base.hardwareResources.GetIOPort(0, 2);
			LBALowPort = base.hardwareResources.GetIOPort(0, 3);
			LBAMidPort = base.hardwareResources.GetIOPort(0, 4);
			LBAHighPort = base.hardwareResources.GetIOPort(0, 5);
			DeviceHeadPort = base.hardwareResources.GetIOPort(0, 6);
			CommandPort = base.hardwareResources.GetIOPort(0, 7);
			StatusPort = base.hardwareResources.GetIOPort(0, 7);

			for (int drive = 0; drive < DrivesPerConroller; drive++)
			{
				driveInfo[drive].Present = false;
				driveInfo[drive].MaxLBA = 0;
			}

			base.deviceStatus = DeviceStatus.Online;
			return true;
		}

		/// <summary>
		/// Probes this instance.
		/// </summary>
		/// <returns></returns>
		public bool Probe()
		{
			LBALowPort.Write8(0x88);

			if (LBALowPort.Read8() != 0x88)
				return false;

			return true;
		}

		/// <summary>
		/// Starts this hardware device.
		/// </summary>
		/// <returns></returns>
		public override DeviceDriverStartStatus Start()
		{
			DeviceHeadPort.Write8(0xA0);

			HAL.Sleep(1000 / 250); // wait 1/250th of a second

			if ((StatusPort.Read8() & 0x40) == 0x40)
				driveInfo[0].Present = true;

			DeviceHeadPort.Write8(0xB0);

			HAL.Sleep(1000 / 250); // wait 1/250th of a second

			if ((StatusPort.Read8() & 0x40) == 0x40)
				driveInfo[1].Present = true;

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

		/// <summary>
		/// Waits for reqister ready.
		/// </summary>
		/// <returns></returns>
		protected bool WaitForReqisterReady()
		{
			while (true)
			{
				uint status = StatusPort.Read8();

				if ((status & 0x08) == 0x08)
					return true;

				//TODO: add timeout check
			}

			//return false;
		}

		/// <summary>
		///
		/// </summary>
		protected enum SectorOperation
		{
			/// <summary>
			///
			/// </summary>
			Read,

			/// <summary>
			///
			/// </summary>
			Write
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
		protected bool PerformLBA28(SectorOperation operation, uint drive, uint lba, byte[] data, uint offset)
		{
			if (drive > MaximunDriveCount)
				return false;

			FeaturePort.Write8(0);
			SectorCountPort.Write8(1);

			LBALowPort.Write8((byte)(lba & 0xFF));
			LBAMidPort.Write8((byte)((lba >> 8) & 0xFF));
			LBAHighPort.Write8((byte)((lba >> 16) & 0xFF));

			DeviceHeadPort.Write8((byte)(0xE0 | (drive << 4) | ((lba >> 24) & 0x0F)));

			if (operation == SectorOperation.Write)
				CommandPort.Write8(IDECommands.WriteSectorsWithRetry);
			else
				CommandPort.Write8(IDECommands.ReadSectorsWithRetry);

			if (!WaitForReqisterReady())
				return false;

			BinaryFormat sector = new BinaryFormat(data);

			//TODO: Don't use PIO
			if (operation == SectorOperation.Read)
			{
				for (uint index = 0; index < 256; index++)
					sector.SetUShort(offset + (index * 2), DataPort.Read16());
			}
			else
			{
				for (uint index = 0; index < 256; index++)
					DataPort.Write16(sector.GetUShort(offset + (index * 2)));
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
			if (drive > MaximunDriveCount)
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

			if (operation == SectorOperation.Write)
				CommandPort.Write8(0x34);
			else
				CommandPort.Write8(0x24);

			if (!WaitForReqisterReady())
				return false;

			BinaryFormat sector = new BinaryFormat(data);

			//TODO: Don't use PIO
			if (operation == SectorOperation.Read)
			{
				for (uint index = 0; index < 256; index++)
					sector.SetUShort(offset + (index * 2), DataPort.Read16());
			}
			else
			{
				for (uint index = 0; index < 256; index++)
					DataPort.Write16(sector.GetUShort(offset + (index * 2)));
			}

			return true;
		}

		/// <summary>
		/// Opens the specified drive NBR.
		/// </summary>
		/// <param name="drive">The drive NBR.</param>
		/// <returns></returns>
		public bool Open(uint drive)
		{
			if (drive > MaximunDriveCount)
				return false;

			if (!driveInfo[drive].Present)
				return false;

			if (drive == 0)
				DeviceHeadPort.Write8(0xA0);
			else
				if (drive == 1)
					DeviceHeadPort.Write8(0xB0);
				else
					return false;

			CommandPort.Write8(IDECommands.IdentifyDrive);

			if (!WaitForReqisterReady())
				return false;

			BinaryFormat info = new BinaryFormat(new byte[512]);

			for (uint index = 0; index < 256; index++)
				info.SetUShort(index * 2, DataPort.Read16());

			driveInfo[drive].MaxLBA = info.GetUInt(IdentifyDrive.MaxLBA28);

			return true;
		}

		/// <summary>
		/// Releases the specified drive NBR.
		/// </summary>
		/// <param name="drive">The drive NBR.</param>
		/// <returns></returns>
		public bool Release(uint drive)
		{
			return true;
		}

		/// <summary>
		/// Gets the maximun drive count.
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
			if (drive > MaximunDriveCount)
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
			if (drive > MaximunDriveCount)
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
			if (drive > MaximunDriveCount)
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