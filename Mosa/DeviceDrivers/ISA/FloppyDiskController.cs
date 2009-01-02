/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

// References:
// http://en.wikipedia.org/wiki/Floppy_disk_controller
// http://www.osdev.org/wiki/Floppy_Disk_Controller
// ftp://download.intel.com/design/archives/periphrl/docs/29047403.pdf
// http://www.osdever.net/documents/82077AA_FloppyControllerDatasheet.pdf?the_id=41
// http://www.isdaman.com/alsos/hardware/fdc/floppy.htm
// http://www.osdev.org/phpBB2/viewtopic.php?t=13538


using Mosa.ClassLib;
using Mosa.DeviceSystem;

namespace Mosa.DeviceDrivers.ISA
{
    /// <summary>
    /// Floppy Disk Controller (FDC) Device Driver
    /// </summary>
	[ISADeviceDriver(AutoLoad = false, BasePort = 0x03F0, PortRange = 8, IRQ = 6, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	[ISADeviceDriver(AutoLoad = false, BasePort = 0x0370, PortRange = 8, IRQ = 5, ForceOption = "fdc2", Platforms = PlatformArchitecture.Both_x86_and_x64)]
	public class FloppyDiskController : HardwareDevice, IDevice, IHardwareDevice, IDiskControllerDevice
	{

		#region Definitions

		internal struct FIFOCommand
		{
			internal const byte ReadTrack = 0x02;
			internal const byte Specify = 0x03;
			internal const byte SenseDriveStatus = 0x04;
			internal const byte WriteSector = 0x05;
			internal const byte ReadSector = 0x06;
			internal const byte Recalibrate = 0x07;
			internal const byte SenseInterrupt = 0x08;
			internal const byte WriteDeletedSector = 0x09;
			internal const byte ReadID = 0x0A;
			internal const byte ReadDeletedSector = 0x0C;
			internal const byte FormatTrack = 0x0D;
			internal const byte Seek = 0x0F;
			internal const byte Version = 0x10;
			internal const byte ScanEqual = 0x11;
			internal const byte PerpendicularMode = 0x12;
			internal const byte Configure = 0x13;
			internal const byte Verify = 0x16;
			internal const byte ScanLowOrEqual = 0x19;
			internal const byte ScanHighOrEqual = 0x1D;
			internal const byte MFMModeMask = 0x40;
		};

		internal struct DORFlags
		{
			internal const byte MotorEnableShift = 0x04;
			internal const byte MotorEnableMask = 0x0F;
			internal const byte ResetController = 0x00;
			internal const byte EnableDMA = 0x08;
			internal const byte EnableController = 0x04;
			internal const byte MotorShift = 0x10;
			internal const byte DisableAll = 0x00;
			internal const uint driveSelectMask = 0x03;
		};

		internal struct FDC
		{
			internal const uint BytesPerSector = 512;
			internal const uint MaxSectorsPerTracks = 36; // 2.88 = 36, DMF formatted disks can have up to 21 sectors
			internal const uint MaxBytesPerTrack = MaxSectorsPerTracks * BytesPerSector;
		};

		#endregion

        /// <summary>
        /// 
        /// </summary>
		public enum FloppyDriveType
		{
            /// <summary>
            /// 
            /// </summary>
			None,
            /// <summary>
            /// 
            /// </summary>
			Floppy_5_25,
            /// <summary>
            /// 
            /// </summary>
			Floppy_3_5,
            /// <summary>
            /// 
            /// </summary>
			Unknown
		}

        /// <summary>
        /// 
        /// </summary>
		public struct FloppyDriveInfo
		{
            /// <summary>
            /// 
            /// </summary>
			public FloppyDriveType Type;
            /// <summary>
            /// 
            /// </summary>
			public uint KiloByteSize;
		}

        /// <summary>
        /// 
        /// </summary>
		public struct FloppyMediaInfo
		{
            /// <summary>
            /// 
            /// </summary>
			public uint SectorsPerTrack;
            /// <summary>
            /// 
            /// </summary>
			public uint TotalTracks;
            /// <summary>
            /// 
            /// </summary>
			public byte Gap1Length;
            /// <summary>
            /// 
            /// </summary>
			public byte Gap2Length;
		}

        /// <summary>
        /// 
        /// </summary>
		protected struct LastSeek
		{
            /// <summary>
            /// 
            /// </summary>
			public bool calibrated;
            /// <summary>
            /// 
            /// </summary>
			public uint drive;
            /// <summary>
            /// 
            /// </summary>
			public byte track;
            /// <summary>
            /// 
            /// </summary>
			public byte head;
		}

        /// <summary>
        /// 
        /// </summary>
		protected struct TrackCache
		{
            /// <summary>
            /// 
            /// </summary>
			public byte[] buffer;
            /// <summary>
            /// 
            /// </summary>
			public bool valid;
            /// <summary>
            /// 
            /// </summary>
			public byte track;
            /// <summary>
            /// 
            /// </summary>
			public byte head;
		}

        /// <summary>
        /// 
        /// </summary>
		protected SpinLock spinLock;
        /// <summary>
        /// 
        /// </summary>
		protected bool enchancedController = false;

        /// <summary>
        /// 
        /// </summary>
		public const uint DrivesPerController = 2; // the maximum supported

        /// <summary>
        /// 
        /// </summary>
		protected FloppyDriveInfo[] floppyDrives;
        /// <summary>
        /// 
        /// </summary>
		protected FloppyMediaInfo[] floppyMedia;

        /// <summary>
        /// 
        /// </summary>
		protected TrackCache[] trackCache;
        /// <summary>
        /// 
        /// </summary>
		protected LastSeek[] lastSeek;

        /// <summary>
        /// 
        /// </summary>
		protected IReadWriteIOPort commandPort;
        /// <summary>
        /// 
        /// </summary>
		protected IReadWriteIOPort dataPort;
        /// <summary>
        /// 
        /// </summary>
		protected IReadWriteIOPort configPort;
        /// <summary>
        /// 
        /// </summary>
		protected IReadWriteIOPort statusPort;

        /// <summary>
        /// 
        /// </summary>
		protected IDMAChannel floppyDMA;

        /// <summary>
        /// 
        /// </summary>
		protected bool interruptSet = false;

		//protected enum OperationPending
		//{ 
		//    None,
		//    Read,
		//    Write,
		//    Seek,
		//    Recalibrate,
		//    Reset,
		//}

		//protected OperationPending pendingOperation;

        /// <summary>
		/// Initializes a new instance of the <see cref="FloppyDiskController"/> class.
        /// </summary>
		public FloppyDiskController()
		{
			floppyDrives = new FloppyDriveInfo[DrivesPerController];
			floppyMedia = new FloppyMediaInfo[DrivesPerController];

			trackCache = new TrackCache[DrivesPerController];
			lastSeek = new LastSeek[DrivesPerController];

			for (int drive = 0; drive < DrivesPerController; drive++)
				trackCache[drive].buffer = new byte[FDC.MaxBytesPerTrack];
		}

		/// <summary>
		/// Setups this hardware device driver
		/// </summary>
		/// <returns></returns>
		public override bool Setup(IHardwareResources hardwareResources)
		{
			this.hardwareResources = hardwareResources;
			base.name = "FDC_0x" + base.hardwareResources.GetIOPort(0, 0).Address.ToString("X");
			base.parent = null; // no parent

			commandPort = base.hardwareResources.GetIOPort(0, 2);
			statusPort = base.hardwareResources.GetIOPort(0, 4);
			dataPort = base.hardwareResources.GetIOPort(0, 5);
			configPort = base.hardwareResources.GetIOPort(0, 7);

			//			floppyDMA = base.CreateDMAChannel(2);
			//			floppyIRQ = base.CreateIRQHandler(6);

			return true;
		}

		/// <summary>
		/// Starts this hardware device.
		/// </summary>
		/// <returns></returns>
		public override DeviceDriverStartStatus Start()
		{
			for (int drive = 0; drive < DrivesPerController; drive++) {
				trackCache[drive].valid = false;
				lastSeek[drive].calibrated = false;

				// default
				floppyMedia[drive].SectorsPerTrack = 18;
				floppyMedia[drive].TotalTracks = 80;
				//TODO: for 5.25, Gap1 = 0x2A and Gap2 = 0x50
				floppyMedia[drive].Gap1Length = 0x1B;	// 27
				floppyMedia[drive].Gap2Length = 0x54;
			}

			ResetController();

			commandPort.Write8(DORFlags.EnableController | DORFlags.EnableDMA);

			SendByte(FIFOCommand.SenseInterrupt);
			GetByte();
			GetByte();

			configPort.Write8(0x00); // 500 Kb/s (MFM)			

			// Set step rate to 3ms & head unload time to 240ms 
			SendByte(FIFOCommand.Specify);
			SendByte((((16 - (3)) << 4) | ((240 / 16))));

			// Set head load time to 2ms
			SendByte(0x02);

			SendByte(FIFOCommand.Version);

			if (GetByte() == 0x80)
				enchancedController = false;
			else
				enchancedController = true;

			DetectDrives();

			return DeviceDriverStartStatus.Started;
		}

		/// <summary>
		/// Called when an interrupt is received.
		/// </summary>
		/// <returns></returns>
		public override bool OnInterrupt()
		{
			interruptSet = true;
			return true;
		}

		/// <summary>
		/// Opens the specified drive.
		/// </summary>
		/// <param name="drive">The drive NBR.</param>
		/// <returns></returns>
		public bool Open(uint drive)
		{
			if (drive > MaximunDriveCount)
				return false;

			if (floppyDrives[drive].Type != FloppyDriveType.None)
				return false;

			// clear it
			floppyMedia[drive].TotalTracks = 0;
			floppyMedia[drive].SectorsPerTrack = 0;

			byte[] temp = new byte[FDC.BytesPerSector];
			spinLock.Enter();

			try {
				//TODO: check drive type first

				// attempt to read 2.88MB/2880KB
				floppyMedia[drive].SectorsPerTrack = 36;
				floppyMedia[drive].TotalTracks = 80;
				floppyMedia[drive].Gap1Length = 0x1B;
				floppyMedia[drive].Gap2Length = 0x54;

				if (ReadBlock(drive, CHSToLBA(drive, floppyMedia[drive].TotalTracks - 1, 0, floppyMedia[drive].SectorsPerTrack - 1), 1, temp))
					return true;

				// attempt to read 1.64MB/1680KB (DMF)
				floppyMedia[drive].SectorsPerTrack = 21;
				floppyMedia[drive].TotalTracks = 80;
				floppyMedia[drive].Gap1Length = 0x0C;
				floppyMedia[drive].Gap2Length = 0x1C;

				if (ReadBlock(drive, CHSToLBA(drive, floppyMedia[drive].TotalTracks - 1, 0, floppyMedia[drive].SectorsPerTrack - 1), 1, temp))
					return true;

				// attempt to read 1.44MB
				floppyMedia[drive].TotalTracks = 80;
				floppyMedia[drive].SectorsPerTrack = 18;
				floppyMedia[drive].Gap1Length = 0x1B;
				floppyMedia[drive].Gap2Length = 0x54;

				if (ReadBlock(drive, CHSToLBA(drive, floppyMedia[drive].TotalTracks - 1, 0, floppyMedia[drive].SectorsPerTrack - 1), 1, temp))
					return true;

				// attempt to read 720Kb
				floppyMedia[drive].TotalTracks = 80;
				floppyMedia[drive].SectorsPerTrack = 9;
				floppyMedia[drive].Gap1Length = 0x1B;
				floppyMedia[drive].Gap2Length = 0x54;

				if (ReadBlock(drive, CHSToLBA(drive, floppyMedia[drive].TotalTracks - 1, 0, floppyMedia[drive].SectorsPerTrack - 1), 1, temp))
					return true;

				// unable to read floppy media 
				floppyMedia[drive].TotalTracks = 0;
				floppyMedia[drive].SectorsPerTrack = 0;

				return false;
			}
			finally {
				spinLock.Exit();
			}

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
		/// Gets the maximun drive count.
		/// </summary>
		/// <value>The drive count.</value>
		public uint MaximunDriveCount { get { return 2; } }

		/// <summary>
		/// Gets the size of the sector.
		/// </summary>
		/// <param name="drive">The drive.</param>
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

			return floppyMedia[drive].SectorsPerTrack * floppyMedia[drive].TotalTracks * 2;
		}

		/// <summary>
		/// Determines whether this instance can write the specified drive NBR.
		/// </summary>
		/// <param name="drive">The drive NBR.</param>
		/// <returns>
		/// 	<c>true</c> if this instance can write the specified drive NBR; otherwise, <c>false</c>.
		/// </returns>
		public bool CanWrite(uint drive)
		{
			if (drive > MaximunDriveCount)
				return false;

			if (floppyMedia[drive].SectorsPerTrack == 0)
				return false;

			return true;
		}

        /// <summary>
        /// Waits for reqister ready.
        /// </summary>
        /// <returns></returns>
		protected bool WaitForReqisterReady()
		{
			// wait for RQM data register to be ready
			while (true) {
				uint status = statusPort.Read8();

				if ((status & 0x80) == 0x80)
					return true;

				//TODO: add timeout check
			}

			//return false;
		}

        /// <summary>
        /// Sends the byte.
        /// </summary>
        /// <param name="command">The command.</param>
		protected void SendByte(byte command)
		{
			WaitForReqisterReady();
			dataPort.Write8(command);
		}

        /// <summary>
        /// Gets the byte.
        /// </summary>
        /// <returns></returns>
		protected byte GetByte()
		{
			WaitForReqisterReady();
			return dataPort.Read8();
		}

        /// <summary>
        /// Clears the interrupt.
        /// </summary>
		protected void ClearInterrupt()
		{
			interruptSet = false;
		}

        /// <summary>
        /// Waits for interrupt.
        /// </summary>
        /// <param name="milliseconds">The milliseconds.</param>
        /// <returns></returns>
		protected bool WaitForInterrupt(uint milliseconds)
		{
			while (!interruptSet) ;

			return true;
		}

        /// <summary>
        /// Resets the controller.
        /// </summary>
		protected void ResetController()
		{
			ClearInterrupt();

			commandPort.Write8(DORFlags.ResetController);

			HAL.Sleep(200);

			commandPort.Write8(DORFlags.EnableController);

			WaitForInterrupt(3000);
		}

        /// <summary>
        /// Determines the type of the by.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
		protected static FloppyDriveInfo DetermineByType(byte type)
		{
			FloppyDriveInfo floppy = new FloppyDriveInfo();

			switch (type) {
				case 0: { floppy.Type = FloppyDriveType.None; floppy.KiloByteSize = 0; break; }
				case 1: { floppy.Type = FloppyDriveType.Floppy_5_25; floppy.KiloByteSize = 360; break; }
				case 2: { floppy.Type = FloppyDriveType.Floppy_5_25; floppy.KiloByteSize = 1200; break; }
				case 3: { floppy.Type = FloppyDriveType.Floppy_3_5; floppy.KiloByteSize = 720; break; }
				case 4: { floppy.Type = FloppyDriveType.Floppy_3_5; floppy.KiloByteSize = 1440; break; }
				case 5: { floppy.Type = FloppyDriveType.Floppy_3_5; floppy.KiloByteSize = 2880; break; }
				default: { floppy.Type = FloppyDriveType.Unknown; floppy.KiloByteSize = 0; break; }
			}

			return floppy;
		}

        /// <summary>
        /// Detects the drives.
        /// </summary>
		protected void DetectDrives()
		{
			// TODO - connect to CMOS driver and request this information
			// for now, just assume one 3.5in 1.44Mb drive

			//CMOSComand = base.CreateIOPort(0x70);
			//CMOSResponse = base.CreateIOPort(0x71);

			//CMOSComand.Write8(0x10);
			//byte types = CMOSResponse.Read8();

			//floppyDrives[0] = DetermineByType((byte)(types >> 4));
			//floppyDrives[1] = DetermineByType((byte)(types & 0xF));

			floppyDrives[0].Type = FloppyDriveType.Floppy_3_5;
			floppyDrives[0].KiloByteSize = 14400;

			floppyDrives[1].Type = FloppyDriveType.None;
			floppyDrives[1].KiloByteSize = 0;
		}

        /// <summary>
        /// Turns the off motor.
        /// </summary>
        /// <param name="drive">The drive.</param>
		protected void TurnOffMotor(uint drive)
		{
			commandPort.Write8((byte)(DORFlags.EnableDMA | DORFlags.EnableController | drive));
		}

        /// <summary>
        /// Turns the on motor.
        /// </summary>
        /// <param name="drive">The drive.</param>
		protected void TurnOnMotor(uint drive)
		{
			byte reg = commandPort.Read8();
			byte bits = (byte)(DORFlags.MotorShift << (byte)drive | DORFlags.EnableDMA | DORFlags.EnableController | (byte)drive);

			if (reg != bits) {
				commandPort.Write8(bits);
				HAL.Sleep(500); // 500 msec
			}
		}

        /// <summary>
        /// Recalibrates the specified drive.
        /// </summary>
        /// <param name="drive">The drive.</param>
        /// <returns></returns>
		protected bool Recalibrate(uint drive)
		{
			lastSeek[drive].calibrated = false;

			for (int i = 0; i < 5; i++) {
				TurnOnMotor(drive);

				ClearInterrupt();

				SendByte(FIFOCommand.Recalibrate);
				SendByte((byte)drive);

				WaitForInterrupt(3000);

				SendByte(FIFOCommand.SenseInterrupt);
				byte sr0 = GetByte();
				byte fdc_track = GetByte();

				if (((sr0 & 0xC0) == 0x00) && (fdc_track == 0)) {
					lastSeek[drive].calibrated = true;
					lastSeek[drive].track = 0;
					lastSeek[drive].head = 2;	// invalid head (required)
					return true;	// Note: motor is left on				
				}
			}

			TurnOffMotor(drive);

			return false;
		}

        /// <summary>
        /// Seeks the specified drive.
        /// </summary>
        /// <param name="drive">The drive.</param>
        /// <param name="track">The track.</param>
        /// <param name="head">The head.</param>
        /// <returns></returns>
		protected bool Seek(uint drive, byte track, byte head)
		{
			TurnOnMotor(drive);

			if (!lastSeek[drive].calibrated)
				if (!Recalibrate(drive))
					return false;

			if ((lastSeek[drive].calibrated) && (lastSeek[drive].track == track) && (lastSeek[drive].head == head))
				return true;

			for (int i = 0; i < 5; i++) {
				ClearInterrupt();

				lastSeek[drive].calibrated = false;

				SendByte(FIFOCommand.Seek);
				SendByte((byte)(((byte)drive | (head << 2))));
				SendByte(track);

				if (!WaitForInterrupt(3000))
					return false;

				HAL.Sleep(20);

				SendByte(FIFOCommand.SenseInterrupt);
				byte sr0 = GetByte();
				byte trk = GetByte();

				if ((sr0 == (0x20 + ((byte)drive | (head << 2)))) && (trk == track)) {
					lastSeek[drive].calibrated = true;
					lastSeek[drive].track = track;
					lastSeek[drive].head = head;
					return true;
				}

			}

			return false;
		}

        /// <summary>
        /// LBAs to track.
        /// </summary>
        /// <param name="drive">The drive.</param>
        /// <param name="lba">The lba.</param>
        /// <returns></returns>
		protected byte LBAToTrack(uint drive, uint lba)
		{
			return (byte)(lba / (floppyMedia[drive].SectorsPerTrack * 2));
		}

        /// <summary>
        /// LBAs to head.
        /// </summary>
        /// <param name="drive">The drive.</param>
        /// <param name="lba">The lba.</param>
        /// <returns></returns>
		protected byte LBAToHead(uint drive, uint lba)
		{
			return (byte)((lba % (floppyMedia[drive].SectorsPerTrack * 2)) / floppyMedia[drive].SectorsPerTrack);
		}

        /// <summary>
        /// LBAs to sector.
        /// </summary>
        /// <param name="drive">The drive.</param>
        /// <param name="lba">The lba.</param>
        /// <returns></returns>
		protected byte LBAToSector(uint drive, uint lba)
		{
			return (byte)((lba % (floppyMedia[drive].SectorsPerTrack * 2)) % floppyMedia[drive].SectorsPerTrack);
		}

        /// <summary>
        /// CHSs to LBA.
        /// </summary>
        /// <param name="drive">The drive.</param>
        /// <param name="cylinder">The cylinder.</param>
        /// <param name="head">The head.</param>
        /// <param name="sector">The sector.</param>
        /// <returns></returns>
		protected uint CHSToLBA(uint drive, uint cylinder, uint head, uint sector)
		{
			return ((cylinder * 2 + head) * floppyMedia[drive].SectorsPerTrack) + sector - 1;
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
			if (data.Length < count * 512)
				return false;

			try {
				spinLock.Enter();
				for (uint index = 0; index < count; index++) {
					if (!ReadBlock2(drive, block + index, data, index * FDC.BytesPerSector))
						return false;
				}
				return true;
			}
			finally {
				TurnOffMotor(drive);	//TODO: create timer to turn off drive motors after 1 sec.
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
			if (data.Length < count * 512)
				return false;

			try {
				spinLock.Enter();
				for (uint index = 0; index < count; index++) {
					if (!WriteBlock2(drive, block + index, 1, data, index * FDC.BytesPerSector))
						return false;
				}
				return true;
			}
			finally {
				TurnOffMotor(drive);	//TODO: create timer to turn off drive motors after 1 sec.
				spinLock.Exit();
			}
		}

        /// <summary>
        /// Reads the block2.
        /// </summary>
        /// <param name="drive">The drive.</param>
        /// <param name="lba">The lba.</param>
        /// <param name="data">The data.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
		protected bool ReadBlock2(uint drive, uint lba, byte[] data, uint offset)
		{
			if (drive > MaximunDriveCount)
				return false;

			byte track = LBAToTrack(drive, lba);
			byte head = LBAToHead(drive, lba);
			byte sector = LBAToSector(drive, lba);

			if (!((trackCache[drive].valid) && (trackCache[drive].track == track) && (trackCache[drive].head == head))) {
				trackCache[drive].valid = false;

				if (!PerformIO(SectorOperation.Read, drive, 0, track, head, floppyMedia[drive].SectorsPerTrack, trackCache[drive].buffer, 0))
					return false;

				trackCache[drive].valid = true;
				trackCache[drive].head = head;
				trackCache[drive].track = track;
			}

			for (uint i = 0; i < FDC.BytesPerSector; i++)
				data[offset + i] = trackCache[drive].buffer[(sector * FDC.BytesPerSector) + i];

			return true;
		}

        /// <summary>
        /// Writes the block.
        /// </summary>
        /// <param name="drive">The drive.</param>
        /// <param name="lba">The lba.</param>
        /// <param name="count">The count.</param>
        /// <param name="data">The data.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
		protected bool WriteBlock2(uint drive, uint lba, uint count, byte[] data, uint offset)
		{
			if (drive > MaximunDriveCount)
				return false;
			
			byte track = LBAToTrack(drive, lba);
			byte head = LBAToHead(drive, lba);
			byte sector = LBAToSector(drive, lba);

			if (sector + count > floppyMedia[drive].SectorsPerTrack)
				return false;

			if ((trackCache[drive].track == track) && (trackCache[drive].head == head)) {
				// updated track cache
				for (uint i = 0; i < count * FDC.BytesPerSector; i++)
					trackCache[drive].buffer[(sector * FDC.BytesPerSector) + i] = data[offset + i];
			}

			if (!PerformIO(SectorOperation.Write, drive, sector, track, head, count, data, offset))
				return false;

			return true;
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
        /// Performs the IO.
        /// </summary>
        /// <param name="operation">The operation.</param>
        /// <param name="drive">The drive.</param>
        /// <param name="sector">The sector.</param>
        /// <param name="track">The track.</param>
        /// <param name="head">The head.</param>
        /// <param name="count">The count.</param>
        /// <param name="data">The data.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
		protected bool PerformIO(SectorOperation operation, uint drive, byte sector, byte track, byte head, uint count, byte[] data, uint offset)
		{
			for (int i = 0; i < 5; i++) {
				int error = 0;

				TurnOnMotor(drive);

				//TODO: Check for disk change

				if (Seek(drive, track, head)) {
					if (operation == SectorOperation.Write) {
						floppyDMA.TransferIn(count * FDC.BytesPerSector, data, offset);
						floppyDMA.SetupChannel(DMAMode.ReadFromMemory, DMATransferType.Single, false, count * FDC.BytesPerSector);
					}
					else
						floppyDMA.SetupChannel(DMAMode.WriteToMemory, DMATransferType.Single, false, count * FDC.BytesPerSector);

					ClearInterrupt();

					if (operation == SectorOperation.Write)
						SendByte(FIFOCommand.WriteSector | FIFOCommand.MFMModeMask);
					else
						SendByte(FIFOCommand.ReadSector | FIFOCommand.MFMModeMask);

					SendByte((byte)((byte)drive | (head << 2)));	// 0:0:0:0:0:HD:US1:US0 = head and drive
					SendByte(track);// C: 
					SendByte(head);	// H: first head (should match with above)
					SendByte((byte)(sector + 1));	// R: first sector, strangely counts from 1
					SendByte(2);	// N: bytes/sector, 128*2^x (x=2 -> 512) 
					SendByte((byte)(sector + count)); // EOT
					SendByte(floppyMedia[drive].Gap1Length);	// GPL: GAP3 length, 27 is default for 3.5" 
					SendByte(0xFF);	// DTL: (bytes to transfer) = unused

					if (!WaitForInterrupt(3000))
						error = 3;

					byte st0 = GetByte();
					byte st1 = GetByte();
					byte st2 = GetByte();

					byte trk = GetByte();	// track (cylinder)
					byte rhe = GetByte();	// head
					//byte sec = GetByte();	// sector number
					GetByte(); // sector number
					byte bps = GetByte();	// bytes per sector

					if ((st0 & 0xC0) != 0x0) {
						//if (verbose) TextMode.WriteLine ("FloppyDiskController: error");
						error = 1;
					}
					if (trk != track + 1) {
						//if (verbose) TextMode.WriteLine ("FloppyDiskController: wrong track: ", trk - 1);
						error = 1;
					}
					if (rhe != head) {
						//if (verbose) TextMode.WriteLine ("FloppyDiskController: wrong track: ", trk - 1);
						error = 1;
					}
					if ((st1 & 0x80) != 0x0) {
						//if (verbose) TextMode.WriteLine ("FloppyDiskController: end of cylinder");
						error = 1;
					}
					if ((st0 & 0x08) != 0x0) {
						//if (verbose) TextMode.WriteLine ("FloppyDiskController: drive not ready");
						error = 1;
					}
					if ((st1 & 0x20) != 0x0) {
						//if (verbose) TextMode.WriteLine ("FloppyDiskController: CRC error");
						error = 1;
					}
					if ((st1 & 0x10) != 0x0) {
						//if (verbose) TextMode.WriteLine ("FloppyDiskController: controller timeout");
						error = 1;
					}
					if ((st1 & 0x04) != 0x0) {
						//if (verbose) TextMode.WriteLine ("FloppyDiskController: no data found");
						error = 1;
					}
					if (((st1 | st2) & 0x01) != 0x0) {
						//if (verbose) TextMode.WriteLine ("FloppyDiskController: no address mark found");
						error = 1;
					}
					if ((st2 & 0x40) != 0x0) {
						//if (verbose) TextMode.WriteLine ("FloppyDiskController: deleted address mark");
						error = 1;
					}
					if ((st2 & 0x20) != 0x0) {
						//if (verbose) TextMode.WriteLine ("FloppyDiskController: CRC error in data");
						error = 1;
					}
					if ((st2 & 0x10) != 0x0) {
						//if (verbose) TextMode.WriteLine ("FloppyDiskController: wrong cylinder");
						error = 1;
					}
					if ((st2 & 0x04) != 0x0) {
						//if (verbose) TextMode.WriteLine ("FloppyDiskController: sector not found");
						error = 1;
					}
					if ((st2 & 0x02) != 0x0) {
						//if (verbose) TextMode.WriteLine ("FloppyDiskController: bad cylinder");
						error = 1;
					}
					if (bps != 0x02) {
						//if (verbose) TextMode.WriteLine ("FloppyDiskController: wanted 512B/sector, got something else: ", (int)bps);
						error = 1;
					}
					if ((st1 & 0x02) != 0x0) {
						//if (verbose) TextMode.WriteLine ("FloppyDiskController: not writable");
						error = 2;
					}
				}
				else {
					error = 1; // seek failed
				}

				if (error == 0) {
					if (operation == SectorOperation.Write)
						return true;
					else
						if (floppyDMA.TransferOut(count * FDC.BytesPerSector, data, offset))
							return true;

					return false;
				}

				lastSeek[drive].calibrated = false;	// will force recalibration

				if (error > 1) {
					TurnOffMotor(drive);
					break;
				}

			}

			return false;
		}

	}
}
