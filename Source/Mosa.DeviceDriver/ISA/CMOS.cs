// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

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
		public byte Read(byte address)
		{
			lock (_lock)
			{
				commandPort.Write8(address);
				return dataPort.Read8();
			}
		}

		/// <summary>
		/// Gets the second.
		/// </summary>
		/// <value>The second.</value>
		public byte Second => Read(0);

		/// <summary>
		/// Gets the minute.
		/// </summary>
		/// <value>The minute.</value>
		public byte GetMinute() => Read(2);

		/// <summary>
		/// Gets the hour.
		/// </summary>
		/// <value>The hour.</value>
		public byte Hour => Read(4);

		/// <summary>
		/// Gets the year.
		/// </summary>
		/// <value>The year.</value>
		public byte Year => Read(9);

		/// <summary>
		/// Gets the month.
		/// </summary>
		/// <value>The month.</value>
		public byte Month => Read(8);

		/// <summary>
		/// Gets the day.
		/// </summary>
		/// <value>The day.</value>
		public byte Day => Read(7);
	}
}
