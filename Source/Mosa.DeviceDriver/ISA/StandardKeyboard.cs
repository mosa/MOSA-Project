// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

namespace Mosa.DeviceDriver.ISA;

/// <summary>
/// Standard Keyboard Device Driver
/// </summary>
//[ISADeviceDriver(AutoLoad = true, BasePort = 0x60, PortRange = 1, AltBasePort = 0x64, AltPortRange = 1, IRQ = 1, Platforms = PlatformArchitecture.X86AndX64)]
public class StandardKeyboard : BaseDeviceDriver, IKeyboardDevice
{
	protected IOPortReadWrite dataPort;

	protected IOPortRead statusPort;

	protected IOPortWrite commandPort;

	protected const ushort fifoSize = 256;

	protected byte[] fifoBuffer;

	protected uint fifoStart;

	protected uint fifoEnd;

	public override void Initialize()
	{
		Device.Name = "StandardKeyboard";

		dataPort = Device.Resources.GetIOPortReadWrite(0, 0);       // 0x60
		statusPort = Device.Resources.GetIOPortRead(1, 0);          // 0x64
		commandPort = Device.Resources.GetIOPortWrite(1, 0);        // 0x64

		fifoBuffer = new byte[fifoSize];
		fifoStart = 0;
		fifoEnd = 0;
	}

	/// <summary>
	/// Probes this instance.
	/// </summary>
	/// <remarks>
	/// Override for ISA devices, if example
	/// </remarks>
	public override void Probe() => Device.Status = DeviceStatus.Available;

	/// <summary>
	/// Starts this hardware device.
	/// </summary>
	public override void Start() => Device.Status = DeviceStatus.Online;

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
		return fifoEnd != fifoStart;
	}

	/// <summary>
	/// Determines whether the FIFO is full
	/// </summary>
	/// <returns>
	/// 	<c>true</c> if the FIFO is full; otherwise, <c>false</c>.
	/// </returns>
	protected bool IsFIFOFull()
	{
		return (fifoEnd + 1 == fifoSize ? 0 : fifoEnd + 1) == fifoStart;
	}

	/// <summary>
	/// Reads the scan code from the device
	/// </summary>
	protected void ReadScanCode()
	{
		lock (_lock)
		{
			byte status = statusPort.Read8();

			if ((status & 0x01) == 0x01)
			{
				byte data = dataPort.Read8();

				AddToFIFO(data);
			}
		}
	}

	/// <summary>
	/// Gets the scan code from the fifo
	/// </summary>
	/// <returns></returns>
	public byte GetScanCode()
	{
		lock (_lock)
		{
			if (IsFIFODataAvailable())
			{
				return GetFromFIFO();
			}
		}
		return 0;
	}
}
