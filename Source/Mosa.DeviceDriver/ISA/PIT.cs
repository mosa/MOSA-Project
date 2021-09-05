// Copyright (c) MOSA Project. Licensed under the New BSD License.

// 82C54 CHMOS Programmable Interval Timer Datasheet
// http://bochs.sourceforge.net/techspec/intel-82c54-timer.pdf.gz

using Mosa.DeviceSystem;

namespace Mosa.DeviceDriver.ISA
{
	/// <summary>
	/// Programmable Interval Timer (PIT) Device Driver
	/// </summary>
	//[ISADeviceDriver(AutoLoad = true, BasePort = 0x40, PortRange = 4, IRQ = 0, Platforms = PlatformArchitecture.X86AndX64)]
	public class PIT : DeviceSystem.DeviceDriver
	{
		#region Definitions

		/// <summary>
		///
		/// </summary>
		private const byte SquareWave = 0x36;

		/// <summary>
		///
		/// </summary>
		private const uint Frequency = 1193182;

		/// <summary>
		///
		/// </summary>
		private const ushort Hz = 1000;

		#endregion Definitions

		/// <summary>
		///
		/// </summary>
		protected IOPortReadWrite modeControlPort;

		/// <summary>
		///
		/// </summary>
		protected IOPortReadWrite counter0Divisor;

		/// <summary>
		///
		/// </summary>
		protected uint tickCount;

		/// <summary>
		///
		/// </summary>
		public bool IsWaiting;

		/// <summary>
		///
		/// </summary>
		public ulong Ticks;

		/// <summary>
		/// Initializes this device.
		/// </summary>
		protected override void Initialize()
		{
			Device.Name = "PIT_0x" + Device.Resources.GetIOPortRegion(0).BaseIOPort.ToString("X");

			modeControlPort = Device.Resources.GetIOPortReadWrite(0, 3);
			counter0Divisor = Device.Resources.GetIOPortReadWrite(0, 0);
		}

		/// <summary>
		/// Probes this instance.
		/// </summary>
		/// <remarks>
		/// Overide for ISA devices, if example
		/// </remarks>
		public override void Probe() => Device.Status = DeviceStatus.Available;

		/// <summary>
		/// Starts this hardware device.
		/// </summary>
		public override void Start()
		{
			if (Device.Status != DeviceStatus.Available)
				return;

			ushort timerCount = (ushort)(Frequency / Hz);

			// Set to Mode 3 - Square Wave Generator
			modeControlPort.Write8(SquareWave);
			counter0Divisor.Write8((byte)(timerCount & 0xFF));
			counter0Divisor.Write8((byte)((timerCount & 0xFF00) >> 8));

			tickCount = 0;

			Device.Status = DeviceStatus.Online;
		}

		/// <summary>
		/// Called when an interrupt is received.
		/// </summary>
		/// <returns></returns>
		public override bool OnInterrupt()
		{
			if (IsWaiting)
				Ticks += 1000 / Hz;
			tickCount += 1000 / Hz;

			return true;
		}

		/// <summary>
		/// Gets the tick count.
		/// </summary>
		/// <returns></returns>
		public uint GetTickCount()
		{
			return tickCount;
		}

		/// <summary>
		/// Waits for a specific time, in milliseconds.
		/// </summary>
		public void Wait(uint ms)
		{
			Ticks = 0;
			IsWaiting = true;
			while (Ticks < ms);
			IsWaiting = false;
		}
	}
}
