/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.DeviceDrivers;
using Mosa.ClassLib;
using Mosa.DeviceDrivers.PCI;
using Mosa.DeviceDrivers.Kernel;

namespace Mosa.DeviceDrivers.ISA
{
	[ISADeviceSignature(AutoLoad = true, BasePort = 0x0060, PortRange = 1, AltBasePort = 0x0064, AltPortRange = 1, IRQ = 1, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	public class Keyboard : ISAHardwareDevice, IDevice, IHardwareDevice
	{
		protected IReadWriteIOPort commandPort;
		protected IReadWriteIOPort dataPort;

		protected const ushort fifoSize = 256;
		protected byte[] fifoBuffer;
		protected uint fifoStart;
		protected uint fifoEnd;

		protected SpinLock spinLock;

		public Keyboard() { }
		public void Dispose() { }

		public override bool Setup()
		{
			base.name = "Keyboard";

			commandPort = base.busResources.GetIOPort(0, 0);
			dataPort = base.busResources.GetIOPort(1, 0);

			this.fifoBuffer = new byte[fifoSize];
			this.fifoStart = 0;
			this.fifoEnd = 0;

			return true;
		}

		public override bool Start() { return true; }
		public override bool Probe() { return true; }
		public override LinkedList<IDevice> CreateSubDevices() { return null; }
		public override bool OnInterrupt() { return true; }

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

		protected byte GetFromFIFO()
		{
			if (fifoEnd == fifoStart)
				return 0;	// should not happen

			byte value = fifoBuffer[fifoStart];

			fifoStart++;

			if (fifoStart == fifoSize)
				fifoStart = 0;

			return value;
		}

		protected bool IsFIFODataAvailable()
		{
			return (fifoEnd != fifoStart);
		}

		protected bool IsFIFOFull()
		{
			if ((((fifoEnd + 1) == fifoSize) ? 0 : fifoEnd + 1) == fifoStart)
				return true;
			else
				return false;
		}

		protected void ReadKey()
		{
			spinLock.Enter();
			byte b = dataPort.Read8();


			spinLock.Exit();			
		}
	}
}
