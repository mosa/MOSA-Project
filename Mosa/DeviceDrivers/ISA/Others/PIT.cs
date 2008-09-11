/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

// 82C54 CHMOS Programmable Interval Timer Datasheet
// http://bochs.sourceforge.net/techspec/intel-82c54-timer.pdf.gz

using Mosa.DeviceDrivers;
using Mosa.DeviceDrivers.Kernel;
using Mosa.DeviceDrivers.PCI;
using Mosa.ClassLib;

namespace Mosa.DeviceDrivers.ISA
{
	[ISADeviceSignature(AutoLoad = true, BasePort = 0x0040, PortRange = 4, IRQ = 0, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	public class PIT : ISAHardwareDevice, IDevice, IHardwareDevice
	{
		#region Definitions

		private const byte SquareWave = 0x36;
		private const uint Frequency = 1193182;
		private const ushort Hz = 100;

		#endregion

		protected IReadWriteIOPort modeControlPort;
		protected IReadWriteIOPort counter0Divisor;
		protected uint tickCount;

		public PIT() { }

		public override bool Setup()
		{
			base.name = "PIT_0x" + base.busResources.GetIOPort(0, 0).Address.ToString("X");

			modeControlPort = base.busResources.GetIOPort(0, 3);
			counter0Divisor = base.busResources.GetIOPort(0, 0);

			return true;
		}

		public override bool Probe() { return true; }

		public override bool Start()
		{
			ushort timerCount = (ushort)(Frequency / Hz);

			// Set to Mode 3 - Square Wave Generator
			modeControlPort.Write8(SquareWave);
			counter0Divisor.Write8((byte)(timerCount & 0xFF));
			counter0Divisor.Write8((byte)((timerCount & 0xFF00) >> 8));

			tickCount = 0;

			return true;
		}

		public override LinkedList<IDevice> CreateSubDevices() { return null; }

		public override bool OnInterrupt()
		{
			tickCount++;
			return true;
		}

		public uint GetTickCount()
		{
			return tickCount;
		}
	}
}
