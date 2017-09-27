// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

namespace Mosa.DeviceDriver.ISA
{
	/// <summary>
	/// Serial Device Driver
	/// </summary>
	//[ISADeviceDriver(AutoLoad = true, BasePort = 0x03F8, PortRange = 8, IRQ = 4, Platforms = PlatformArchitecture.X86AndX64)]
	//[ISADeviceDriver(AutoLoad = false, BasePort = 0x02F8, PortRange = 8, IRQ = 3, Platforms = PlatformArchitecture.X86AndX64)]
	//[ISADeviceDriver(AutoLoad = false, BasePort = 0x03E8, PortRange = 8, IRQ = 4, Platforms = PlatformArchitecture.X86AndX64)]
	//[ISADeviceDriver(AutoLoad = false, BasePort = 0x02E8, PortRange = 8, IRQ = 3, Platforms = PlatformArchitecture.X86AndX64)]
	public class Serial : HardwareDevice, ISerialDevice
	{
		/// <summary>
		/// Receive Buffer Register (read only)
		/// </summary>
		protected IOPortRead rbrBase;

		/// <summary>
		/// Transmitter Holding Register (write only)
		/// </summary>
		protected IOPortWrite thrBase;

		/// <summary>
		/// Interrupt Enable Register
		/// </summary>
		protected IOPortReadWrite ierBase;

		/// <summary>
		/// Divisor Latch (LSB and MSB)
		/// </summary>
		protected IOPortReadWrite dllBase;

		/// <summary>
		///
		/// </summary>
		protected IOPortReadWrite dlmBase;

		/// <summary>
		/// Interrupt Identification Register (read only)
		/// </summary>
		protected IOPortRead iirBase;

		/// <summary>
		/// FIFO Control Register (write only, 16550+ only)
		/// </summary>
		protected IOPortWrite fcrBase;

		/// <summary>
		/// Line Control Register
		/// </summary>
		protected IOPortReadWrite lcrBase;

		/// <summary>
		/// Modem Control Register
		/// </summary>
		protected IOPortReadWrite mcrBase;

		/// <summary>
		/// Line Status Register
		/// </summary>
		protected IOPortReadWrite lsrBase;

		/// <summary>
		/// Modem Status Register
		/// </summary>
		protected IOPortReadWrite msrBase;

		/// <summary>
		/// Scratch Register (16450+ and some 8250s, special use with some boards)
		/// </summary>
		protected IOPortReadWrite scrBase;

		/// <summary>
		///
		/// </summary>
		protected SpinLock spinLock;

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

		#region Flags

		/// <summary>
		///
		/// </summary>
		[System.Flags]
		private enum IER : byte
		{
			/// <summary>
			///
			/// </summary>
			DR = 0x01, // Data ready, it is generated if data waits to be read by the CPU.

			/// <summary>
			///
			/// </summary>
			THRE = 0x02, // THR Empty, this interrupt tells the CPU to write characters to the THR.

			/// <summary>
			///
			/// </summary>
			SI = 0x04, // Status interrupt. It informs the CPU of occurred transmission errors during reception.

			/// <summary>
			///
			/// </summary>
			MSI = 0x08 // Modem status interrupt. It is triggered whenever one of the delta-bits is set (see MSR).
		}

		/// <summary>
		///
		/// </summary>
		[System.Flags]
		private enum FCR : byte
		{
			// Receiver FIFO trigger level
			/// <summary>
			///
			/// </summary>
			TL1 = 0x00,

			/// <summary>
			///
			/// </summary>
			Enabled = 0x01,

			/// <summary>
			///
			/// </summary>
			CLR_RCVR = 0x02,

			/// <summary>
			///
			/// </summary>
			CLR_XMIT = 0x04,

			/// <summary>
			///
			/// </summary>
			DMA = 0x08,

			/// <summary>
			///
			/// </summary>
			TL4 = 0x40,

			/// <summary>
			///
			/// </summary>
			TL8 = 0x80,

			/// <summary>
			///
			/// </summary>
			TL14 = 0xC0
		}

		/// <summary>
		///
		/// </summary>
		[System.Flags]
		private enum LCR : byte
		{
			// Word length
			/// <summary>
			///
			/// </summary>
			CS5 = 0x00,

			// Stop bit
			/// <summary>
			///
			/// </summary>
			ST1 = 0x00,

			// Parity
			/// <summary>
			///
			/// </summary>
			PNO = 0x00,

			/// <summary>
			///
			/// </summary>
			CS6 = 0x01,

			/// <summary>
			///
			/// </summary>
			CS7 = 0x02,

			/// <summary>
			///
			/// </summary>
			CS8 = 0x03,

			/// <summary>
			///
			/// </summary>
			ST2 = 0x04,

			/// <summary>
			///
			/// </summary>
			POD = 0x08,

			/// <summary>
			///
			/// </summary>
			PEV = 0x18,

			/// <summary>
			///
			/// </summary>
			PMK = 0x28,

			/// <summary>
			///
			/// </summary>
			PSP = 0x38,

			/// <summary>
			///
			/// </summary>
			BRK = 0x40,

			/// <summary>
			///
			/// </summary>
			DLAB = 0x80
		}

		/// <summary>
		///
		/// </summary>
		[System.Flags]
		private enum MCR : byte
		{
			/// <summary>
			///
			/// </summary>
			DTR = 0x01,

			/// <summary>
			///
			/// </summary>
			RTS = 0x02,

			/// <summary>
			///
			/// </summary>
			OUT1 = 0x04,

			/// <summary>
			///
			/// </summary>
			OUT2 = 0x08,

			/// <summary>
			///
			/// </summary>
			LOOP = 0x10,
		}

		/// <summary>
		///
		/// </summary>
		[System.Flags]
		private enum LSR : byte
		{
			/// <summary>
			/// Data Ready. Reset by reading RBR (but only if the RX FIFO is empty, 16550+).
			/// </summary>
			DR = 0x01,

			/// <summary>
			/// Overrun Error. Reset by reading LSR. Indicates loss of data.
			/// </summary>
			OE = 0x02,

			/// <summary>
			/// Parity Error. Indicates transmission error. Reset by LSR.
			/// </summary>
			PE = 0x04,

			/// <summary>
			/// Framing Error. Indicates missing stop bit. Reset by LSR.
			/// </summary>
			FE = 0x08,

			/// <summary>
			/// Break Indicator. Set if RxD is 'space' for more than 1 word ('break'). Reset by reading LSR.
			/// </summary>
			BI = 0x10,

			/// <summary>
			/// Transmitter Holding Register Empty. Indicates that a new word can be written to THR. Reset by writing THR.
			/// </summary>
			THRE = 0x20,

			/// <summary>
			/// Transmitter Empty. Indicates that no transmission is running. Reset by reading LSR.
			/// </summary>
			TEMT = 0x40,
		}

		/// <summary>
		///
		/// </summary>
		[System.Flags]
		private enum MSR : byte
		{
			/// <summary>
			///
			/// </summary>
			DCTS = 0x01,

			/// <summary>
			///
			/// </summary>
			DDSR = 0x02,

			/// <summary>
			///
			/// </summary>
			DRI = 0x04,

			/// <summary>
			///
			/// </summary>
			DDCD = 0x08,

			/// <summary>
			///
			/// </summary>
			CTS = 0x10,

			/// <summary>
			///
			/// </summary>
			DSR = 0x20,

			/// <summary>
			///
			/// </summary>
			RI = 0x40,

			/// <summary>
			///
			/// </summary>
			DCD = 0x80
		}

		#endregion Flags

		/// <summary>
		/// Initializes a new instance of the <see cref="Serial"/> class.
		/// </summary>
		public Serial()
		{
		}

		/// <summary>
		/// Setups this hardware device driver
		/// </summary>
		/// <param name="hardwareResources"></param>
		/// <returns></returns>
		public override bool Setup(HardwareResources hardwareResources)
		{
			this.HardwareResources = hardwareResources;
			base.Name = "COM_0x" + base.HardwareResources.GetIOPortRegion(0).BaseIOPort.ToString("X");

			rbrBase = base.HardwareResources.GetIOPortReadWrite(0, 0); // Receive Buffer Register (read only)
			thrBase = base.HardwareResources.GetIOPortWrite(0, 0); // Transmitter Holding Register (write only)
			ierBase = base.HardwareResources.GetIOPortReadWrite(0, 1); // Interrupt Enable Register
			dllBase = base.HardwareResources.GetIOPortReadWrite(0, 0); // Divisor Latch (LSB and MSB)
			dlmBase = base.HardwareResources.GetIOPortReadWrite(0, 1);
			iirBase = base.HardwareResources.GetIOPortReadWrite(0, 2); // Interrupt Identification Register (read only)
			fcrBase = base.HardwareResources.GetIOPortWrite(0, 2); // FIFO Control Register (write only, 16550+ only)
			lcrBase = base.HardwareResources.GetIOPortReadWrite(0, 3); // Line Control Register
			mcrBase = base.HardwareResources.GetIOPortReadWrite(0, 4); // Modem Control Register
			lsrBase = base.HardwareResources.GetIOPortReadWrite(0, 5); // Line Status Register
			msrBase = base.HardwareResources.GetIOPortReadWrite(0, 6); // Modem Status Register
			scrBase = base.HardwareResources.GetIOPortReadWrite(0, 7); // Scratch Register (16450+ and some 8250s, special use with some boards)

			fifoBuffer = new byte[fifoSize];
			fifoStart = 0;
			fifoEnd = 0;

			base.DeviceStatus = DeviceStatus.Online;
			return true;
		}

		/// <summary>
		/// Starts this hardware device.
		/// </summary>
		/// <returns></returns>
		public override DeviceDriverStartStatus Start()
		{
			//TODO: auto detect - otherwise just assume one is there
			//TODO: could use BIOS to help w/ detection; 0x0400-x0403 supply base address for COM1-4

			// Disable all UART interrupts
			ierBase.Write8(0x00);

			// Enable DLAB (set baud rate divisor)
			lcrBase.Write8((byte)LCR.DLAB);

			// Set Baud rate
			const int baudRate = 115200;
			const int divisor = 115200 / baudRate;
			dllBase.Write8((byte)(divisor & 0xFF));
			dlmBase.Write8((byte)(divisor >> 8 & 0xFF));

			// Reset DLAB, Set 8 bits, no parity, one stop bit
			lcrBase.Write8((byte)(LCR.CS8 | LCR.ST1 | LCR.PNO));

			// Enable FIFO, clear them, with 14-byte threshold
			fcrBase.Write8((byte)(FCR.Enabled | FCR.CLR_RCVR | FCR.CLR_XMIT | FCR.TL14));

			// IRQs enabled, RTS/DSR set
			mcrBase.Write8((byte)(MCR.DTR | MCR.RTS | MCR.OUT2));

			// Interrupt when data received
			ierBase.Write8((byte)IER.DR);

			return DeviceDriverStartStatus.Started;
		}

		/// <summary>
		/// Called when an interrupt is received.
		/// </summary>
		/// <returns></returns>
		public override bool OnInterrupt()
		{
			ReadSerial();
			return true;
		}

		/// <summary>
		/// Adds to FIFO.
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
		/// Gets from FIFO.
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
		/// Determines whether [is FIFO data available].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [is FIFO data available]; otherwise, <c>false</c>.
		/// </returns>
		protected bool IsFIFODataAvailable()
		{
			return fifoEnd != fifoStart;
		}

		/// <summary>
		/// Determines whether [is FIFO full].
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [is FIFO full]; otherwise, <c>false</c>.
		/// </returns>
		protected bool IsFIFOFull()
		{
			if ((((fifoEnd + 1) == fifoSize) ? 0 : fifoEnd + 1) == fifoStart)
				return true;
			else
				return false;
		}

		/// <summary>
		/// Determines whether this instance can transmit.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if this instance can transmit; otherwise, <c>false</c>.
		/// </returns>
		protected bool CanTransmit()
		{
			return (lsrBase.Read8() & (byte)LSR.THRE) != 0;
		}

		/// <summary>
		/// Writes the specified ch.
		/// </summary>
		/// <param name="ch">The ch.</param>
		public void Write(byte ch)
		{
			try
			{
				spinLock.Enter();

				while (!CanTransmit())
					;

				thrBase.Write8(ch);
			}
			finally
			{
				spinLock.Exit();
			}
		}

		/// <summary>
		/// Determines whether this instance can read.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if this instance can read; otherwise, <c>false</c>.
		/// </returns>
		protected bool CanRead()
		{
			return ((lsrBase.Read8()) & (byte)LSR.DR) != 0;
		}

		/// <summary>
		/// Reads the serial.
		/// </summary>
		protected void ReadSerial()
		{
			try
			{
				spinLock.Enter();

				if (!IsFIFOFull())
					while (CanRead())
						AddToFIFO(rbrBase.Read8());
			}
			finally
			{
				spinLock.Exit();
			}
		}

		/// <summary>
		/// Disables the data received interrupt.
		/// </summary>
		public void DisableDataReceivedInterrupt()
		{
			IER ier = (IER)(ierBase.Read8());
			ier &= ~IER.DR;
			ierBase.Write8((byte)ier);
		}

		/// <summary>
		/// Enables the data received interrupt.
		/// </summary>
		public void EnableDataReceivedInterrupt()
		{
			byte ier = ierBase.Read8();
			ier |= (byte)IER.DR;
			ierBase.Write8(ier);
		}

		/// <summary>
		/// Reads the byte.
		/// </summary>
		/// <returns></returns>
		public int ReadByte()
		{
			try
			{
				spinLock.Enter();

				if (!IsFIFODataAvailable())
					return -1;

				return GetFromFIFO();
			}
			finally
			{
				spinLock.Exit();
			}
		}
	}
}
