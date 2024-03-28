// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.DeviceSystem.Framework;
using Mosa.DeviceSystem.HardwareAbstraction;
using Mosa.DeviceSystem.Mouse;

namespace Mosa.DeviceDriver.ISA;

//https://github.com/nifanfa/MOSA-Core/blob/master/Source/Mosa.External.x86/Driver/Input/PS2Mouse.cs
//https://forum.osdev.org/viewtopic.php?t=10247
//https://wiki.osdev.org/Mouse_Input

/// <summary>
/// A standard PS/2 mouse. It implements the <see cref="IMouseDevice"/> interface.
/// </summary>
public class StandardMouse : BaseDeviceDriver, IMouseDevice
{
	private enum WaitType
	{
		Read,
		Write
	}

	private enum PhaseType
	{
		HandleAcknowledgeReply,
		HandleState,
		HandleXCoordinate,
		HandleYCoordinate
	}

	private IOPortReadWrite command, data;

	private const byte SetDefaults = 0xF6, EnableDataReporting = 0xF4, SetSampleRate = 0xF3;

	private uint screenWidth, screenHeight;
	private PhaseType phase;

	public uint X { get; set; }

	public uint Y { get; set; }

	public MouseState State { get; private set; } = MouseState.None;

	public override void Initialize()
	{
		Device.Name = "StandardMouse";

		data = Device.Resources.GetIOPortReadWrite(0, 0);
		command = Device.Resources.GetIOPortReadWrite(1, 0);

		// Enable the auxiliary mouse device
		WaitReady(WaitType.Write);
		command.Write8(0xA8);

		// Enable the interrupts
		WaitReady(WaitType.Write);
		command.Write8(0x20);
		WaitReady(0);
		var status = (byte)(data.Read8() | 3);
		WaitReady(WaitType.Write);
		command.Write8(0x60);
		WaitReady(WaitType.Write);
		data.Write8(status);

		WriteRegister(SetDefaults);
		WriteRegister(EnableDataReporting);
		WriteRegister(SetSampleRate, 200);
	}

	public override void Probe() => Device.Status = DeviceStatus.Available;

	public override void Start() => Device.Status = DeviceStatus.Online;

	public override bool OnInterrupt()
	{
		var b = data.Read8();

		switch (phase)
		{
			case PhaseType.HandleAcknowledgeReply:
				{
					if (b == 0xFA)
						phase = PhaseType.HandleState;
					return true;
				}
			case PhaseType.HandleState:
				{
					if ((b & (1 << 3)) != 1 << 3)
						return true;

					State = (b & 0x07) switch
					{
						0x01 => MouseState.Left,
						0x02 => MouseState.Right,

						// TODO: Add scroll wheel
						_ => MouseState.None
					};

					phase = PhaseType.HandleXCoordinate;
					return true;
				}
			case PhaseType.HandleXCoordinate:
				{
					var xOffset = b > 127 ? -(255 - b) : b;
					X = (uint)Math.Clamp(X + xOffset, 0, screenWidth);

					phase = PhaseType.HandleYCoordinate;
					return true;
				}
			case PhaseType.HandleYCoordinate:
				{
					var yOffset = b > 127 ? -(255 - b) : b;
					Y = (uint)Math.Clamp(Y - yOffset, 0, screenHeight);

					phase = PhaseType.HandleState;
					return true;
				}
			default: throw new ArgumentOutOfRangeException();
		}
	}

	public void SetScreenResolution(uint width, uint height)
	{
		screenWidth = width;
		screenHeight = height;

		X = width / 2;
		Y = height / 2;
	}

	#region Private

	private void WriteRegister(byte value)
	{
		WaitReady(WaitType.Write);
		command.Write8(0xD4);
		WaitReady(WaitType.Write);
		data.Write8(value);

		ReadRegister();
	}

	private void WriteRegister(byte cmd, byte value)
	{
		WaitReady(WaitType.Write);
		command.Write8(cmd);
		WaitReady(WaitType.Write);
		data.Write8(value);
		WaitReady(WaitType.Write);

		ReadRegister();
	}

	private byte ReadRegister()
	{
		WaitReady(WaitType.Read);
		return data.Read8();
	}

	private void WaitReady(WaitType type)
	{
		var timeout = 100000;

		switch (type)
		{
			case WaitType.Read:
				{
					while (timeout > 0 && (command.Read8() & 1) != 1)
						timeout--;
					return;
				}
			case WaitType.Write:
				{
					while (timeout > 0 && (command.Read8() & 2) != 0)
						timeout--;
					return;
				}
			default: throw new ArgumentOutOfRangeException(nameof(type));
		}
	}

	#endregion Private
}
