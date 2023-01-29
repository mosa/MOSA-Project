// Copyright (c) MOSA Project. Licensed under the New BSD License.

// 82C54 CHMOS Programmable Interval Timer Datasheet
// http://bochs.sourceforge.net/techspec/intel-82c54-timer.pdf.gz

using Mosa.DeviceSystem;

namespace Mosa.DeviceDriver.ISA;

/// <summary>
/// Programmable Interval Timer (PIT) Device Driver
/// </summary>
//[ISADeviceDriver(AutoLoad = true, BasePort = 0x40, PortRange = 4, IRQ = 0, Platforms = PlatformArchitecture.X86AndX64)]
// TODO: Fix interrupt not being received
public class PIT : BaseDeviceDriver, ITimer
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
	protected BaseIOPortReadWrite modeControlPort;

	/// <summary>
	///
	/// </summary>
	protected BaseIOPortReadWrite counter0Divisor;

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
	public override void Initialize()
	{
		Device.Name = "PIT_0x" + Device.Resources.GetIOPortRegion(0).BaseIOPort.ToString("X");

		counter0Divisor = Device.Resources.GetIOPortReadWrite(0, 0); // 0x40
		modeControlPort = Device.Resources.GetIOPortReadWrite(1, 0); // 0x43
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
	/// Waits for a specific time, in milliseconds.
	/// </summary>
	void ITimer.Wait(uint ms)
	{
		Ticks = 0;
		IsWaiting = true;
		while (Ticks < ms) ;
		IsWaiting = false;
	}
}
