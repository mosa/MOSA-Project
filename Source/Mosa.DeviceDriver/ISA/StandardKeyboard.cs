// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem.Framework;
using Mosa.DeviceSystem.HardwareAbstraction;
using Mosa.DeviceSystem.Keyboard;

namespace Mosa.DeviceDriver.ISA;

/// <summary>
/// A standard PS/2 keyboard. It implements the IKeyboardDevice interface, and uses a FIFO (First In First Out) buffer to store
/// the raw scancodes, which can be retrieved using the GetScanCode() function.
/// </summary>
//[ISADeviceDriver(AutoLoad = true, BasePort = 0x60, PortRange = 1, AltBasePort = 0x64, AltPortRange = 1, IRQ = 1, Platforms = PlatformArchitecture.X86AndX64)]
public class StandardKeyboard : BaseDeviceDriver, IKeyboardDevice
{
	private IOPortReadWrite dataPort;
	private IOPortRead statusPort;

	private const ushort FIFOSize = 256;

	private byte[] fifoBuffer;
	private uint fifoStart, fifoEnd;

	public override void Initialize()
	{
		Device.Name = "StandardKeyboard";

		dataPort = Device.Resources.GetIOPortReadWrite(0, 0); // 0x60
		statusPort = Device.Resources.GetIOPortRead(1, 0);    // 0x64

		fifoBuffer = new byte[FIFOSize];
		fifoStart = fifoEnd = 0;
	}

	public override void Probe() => Device.Status = DeviceStatus.Available;

	public override void Start() => Device.Status = DeviceStatus.Online;

	public override bool OnInterrupt()
	{
		ReadScanCode();
		return true;
	}

	public byte GetScanCode(bool blocking = false)
	{
		if (!blocking)
			return IsFIFODataAvailable() ? GetFromFIFO() : (byte)0;

		while (!IsFIFODataAvailable()) HAL.Yield();
		return GetFromFIFO();
	}

	private void AddToFIFO(byte scancode)
	{
		lock (DriverLock)
		{
			var next = fifoEnd + 1;

			if (next == FIFOSize)
				next = 0;

			if (next == fifoStart) // Out of room
				return;

			fifoBuffer[next] = scancode;
			fifoEnd = next;
		}
	}

	private byte GetFromFIFO()
	{
		lock (DriverLock)
		{
			if (fifoEnd == fifoStart) // Should not happen
				return 0;

			var value = fifoBuffer[fifoStart++];

			if (fifoStart == FIFOSize)
				fifoStart = 0;

			return value;
		}
	}

	private bool IsFIFODataAvailable() => fifoEnd != fifoStart;

	private bool IsFIFOFull() => (fifoEnd + 1 == FIFOSize ? 0 : fifoEnd + 1) == fifoStart;

	private void ReadScanCode()
	{
		var status = statusPort.Read8();
		if ((status & 0x01) != 0x01)
			return;

		var data = dataPort.Read8();
		AddToFIFO(data);
	}
}
