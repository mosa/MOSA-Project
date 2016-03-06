// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

namespace Mosa.DeviceDriver.ISA
{
	/// <summary>
	/// Standard Keyboard Device Driver
	/// </summary>
	[ISADeviceDriver(AutoLoad = true, BasePort = 0x60, PortRange = 1, AltBasePort = 0x64, AltPortRange = 1, IRQ = 1, Platforms = PlatformArchitecture.X86AndX64)]
	public class StandardKeyboard : HardwareDevice, IDevice, IHardwareDevice, IKeyboardDevice
	{
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
		protected const ushort fifoSize = 256;

		/// <summary>
		///
		/// </summary>
		protected byte[] fifoBuffer;

		/// <summary>
		///
		/// </summary>
		protected uint fifoStart;

		/// <summary>
		///
		/// </summary>
		protected uint fifoEnd;

		/// <summary>
		///
		/// </summary>
		protected SpinLock spinLock;

		/// <summary>
		/// Initializes a new instance of the <see cref="StandardKeyboard"/> class.
		/// </summary>
		public StandardKeyboard()
		{
		}

		/// <summary>
		/// Setups the standard keyboard driver
		/// </summary>
		/// <returns></returns>
		public override bool Setup(HardwareResources hardwareResources)
		{
			this.HardwareResources = hardwareResources;
			base.Name = "StandardKeyboard";

			commandPort = base.HardwareResources.GetIOPort(0, 0);
			dataPort = base.HardwareResources.GetIOPort(1, 0);

			fifoBuffer = new byte[fifoSize];
			fifoStart = 0;
			fifoEnd = 0;

			return true;
		}

		/// <summary>
		/// Starts the standard keyboard device.
		/// </summary>
		/// <returns></returns>
		public override DeviceDriverStartStatus Start()
		{
			return DeviceDriverStartStatus.Started;
		}

		/// <summary>
		/// Called when interrupt is received.
		/// </summary>
		/// <returns></returns>
		public override bool OnInterrupt()
		{
			ReadScanCode();
			return true;
		}

		/// <summary>
		/// Adds scan code to FIFO.
		/// </summary>
		/// <param name="value">The value.</param>
		protected void AddToFIFO(byte value)
		{
			uint next = fifoEnd + 1;

			if (next == fifoSize)
				next = 0;

			if (next == fifoStart)
				return; // out of room

			fifoBuffer[next] = value;
			fifoEnd = next;
		}

		/// <summary>
		/// Gets scan code from FIFO.
		/// </summary>
		/// <returns></returns>
		protected byte GetFromFIFO()
		{
			if (fifoEnd == fifoStart)
				return 0;   // should not happen

			byte value = fifoBuffer[fifoStart];

			fifoStart++;

			if (fifoStart == fifoSize)
				fifoStart = 0;

			return value;
		}

		/// <summary>
		/// Determines whether FIFO data is available
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [FIFO data is available]; otherwise, <c>false</c>.
		/// </returns>
		protected bool IsFIFODataAvailable()
		{
			return (fifoEnd != fifoStart);
		}

		/// <summary>
		/// Determines whether the FIFO is full
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if the FIFO is full; otherwise, <c>false</c>.
		/// </returns>
		protected bool IsFIFOFull()
		{
			if ((((fifoEnd + 1) == fifoSize) ? 0 : fifoEnd + 1) == fifoStart)
				return true;
			else
				return false;
		}

		/// <summary>
		/// Reads the scan code from the device
		/// </summary>
		protected void ReadScanCode()
		{
			spinLock.Enter();

			byte v = commandPort.Read8();

			AddToFIFO(v);

			HAL.DebugWrite(" scancode: " + v.ToString() + "   ");

			spinLock.Exit();
		}

		/// <summary>
		/// Gets the scan code from the fifo
		/// </summary>
		/// <returns></returns>
		public byte GetScanCode()
		{
			if (!IsFIFODataAvailable())
				return 0;

			return GetFromFIFO();
		}
	}
}
