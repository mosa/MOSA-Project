// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using System;

namespace Mosa.DeviceDriver.ISA
{
	/// <summary>
	/// CMOS Device Driver
	/// </summary>
	//[ISADeviceDriver(AutoLoad = true, BasePort = 0x0070, PortRange = 2, Platforms = PlatformArchitecture.X86)]
	public class CMOS : BaseDeviceDriver
	{
		/// <summary>
		/// The command port
		/// </summary>
		protected BaseIOPortReadWrite commandPort;

		/// <summary>
		/// The data port
		/// </summary>
		protected BaseIOPortReadWrite dataPort;

		public override void Initialize()
		{
			Device.Name = "CMOS";

			commandPort = Device.Resources.GetIOPortReadWrite(0, 0);
			dataPort = Device.Resources.GetIOPortReadWrite(0, 4);
		}

		public override void Probe() => Device.Status = DeviceStatus.Available;

		public override void Start()
		{
			Device.Status = DeviceStatus.Online;
		}

		/// <summary>
		/// Called when an interrupt is received.
		/// </summary>
		/// <returns></returns>
		public override bool OnInterrupt() => true;

		/// <summary>
		/// Reads the specified address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		protected byte Read(byte address)
		{
			lock (_lock)
			{
				commandPort.Write8(address);
				return dataPort.Read8();
			}
		}

		private static byte BCDToBinary(bool bcd, byte value)
		{
			if (bcd)
				return BCDToBinary(value);
			else
				return value;
		}

		private static byte BCDToBinary(byte bcd)
		{
			return (byte)(((bcd / 16) * 10) + (bcd & 0xF));
		}
	}
}
