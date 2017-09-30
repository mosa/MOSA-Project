// Copyright (c) MOSA Project. Licensed under the New BSD License.

// References
// http://wiki.osdev.org/AHCI

using System;
using Mosa.DeviceSystem;

namespace Mosa.DeviceDriver.PCI.MassStorage
{
	public class AHCI : HardwareDevice, IDiskControllerDevice
	{
		public uint MaximunDriveCount
		{
			get { return 0; }
		}

		public override bool Setup(HardwareResources hardwareResources)
		{
			return true;
		}

		public override DeviceDriverStartStatus Start()
		{
			return DeviceDriverStartStatus.Started;
		}

		public override bool Probe()
		{
			return false;
		}

		public bool CanWrite(uint drive) => true;

		public uint GetSectorSize(uint drive) => 512;

		public uint GetTotalSectors(uint drive)
		{
			throw new NotImplementedException();
		}

		public override bool OnInterrupt()
		{
			throw new NotImplementedException();
		}

		public bool Open(uint drive)
		{
			throw new NotImplementedException();
		}

		public bool Release(uint drive)
		{
			throw new NotImplementedException();
		}

		public bool ReadBlock(uint drive, uint block, uint count, byte[] data)
		{
			throw new NotImplementedException();
		}

		public bool WriteBlock(uint drive, uint block, uint count, byte[] data)
		{
			throw new NotImplementedException();
		}
	}
}
