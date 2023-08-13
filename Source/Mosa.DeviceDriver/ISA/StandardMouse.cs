// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.DeviceSystem;

namespace Mosa.DeviceDriver.ISA;
//https://github.com/nifanfa/MOSA-Core/blob/master/Source/Mosa.External.x86/Driver/Input/PS2Mouse.cs
//https://forum.osdev.org/viewtopic.php?t=10247
//https://wiki.osdev.org/Mouse_Input

public class StandardMouse : BaseDeviceDriver, IMouseDevice
{
	private IOPortReadWrite command, data;

	private const byte SetDefaults = 0xF6, EnableDataReporting = 0xF4, SetSampleRate = 0xF3;

	private uint screenWidth, screenHeight;
	private int phase, aX, aY;

	private readonly byte[] mData = new byte[3];

	public uint X { get; set; }

	public uint Y { get; set; }

	public MouseState State { get; private set; }

	public override void Initialize()
	{
		Device.Name = "StandardMouse";

		data = Device.Resources.GetIOPortReadWrite(0, 0); // 0x60
		command = Device.Resources.GetIOPortReadWrite(1, 0); // 0x64

		// Enable the auxiliary mouse device
		Wait(1);
		command.Write8(0xA8);

		// Enable the interrupts
		Wait(1);
		command.Write8(0x20);
		Wait(0);
		var status = (byte)(data.Read8() | 3);
		Wait(1);
		command.Write8(0x60);
		Wait(1);
		data.Write8(status);

		WriteRegister(SetDefaults);
		WriteRegister(EnableDataReporting);
		WriteRegister(SetSampleRate, 200);

		State = MouseState.None;
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

	public override bool OnInterrupt()
	{
		byte D = data.Read8();

		if (phase == 0)
		{
			if (D == 0xfa)
				phase = 1;
			return true;
		}

		if (phase == 1)
		{
			if ((D & (1 << 3)) == 1 << 3)
			{
				mData[0] = D;
				phase = 2;
			}
			return true;
		}

		if (phase == 2)
		{
			mData[1] = D;
			phase = 3;
			return true;
		}

		if (phase == 3)
		{
			mData[2] = D;
			phase = 1;

			mData[0] &= 0x07;
			State = mData[0] switch
			{
				0x01 => MouseState.Left,
				0x02 => MouseState.Right,

				// TODO: Add scroll wheel
				_ => MouseState.None,
			};

			if (mData[1] > 127)
				aX = -(255 - mData[1]);
			else
				aX = mData[1];

			if (mData[2] > 127)
				aY = -(255 - mData[2]);
			else
				aY = mData[2];

			X = (uint)Math.Clamp(X + aX, 0, screenWidth);
			Y = (uint)Math.Clamp(Y - aY, 0, screenHeight);
		}

		return true;
	}

	public void SetScreenResolution(uint width, uint height)
	{
		screenWidth = width;
		screenHeight = height;

		X = width / 2;
		Y = height / 2;
	}

	#region Private

	private void Wait(byte type)
	{
		int timeOut = 100000;

		if (type == 0)
		{
			for (; timeOut > 0; timeOut--)
				if ((command.Read8() & 1) == 1)
					return;

			return;
		}
		else
		{
			for (; timeOut > 0; timeOut--)
				if ((command.Read8() & 2) == 0)
					return;

			return;
		}
	}

	private void WriteRegister(byte value)
	{
		Wait(1);
		command.Write8(0xD4);
		Wait(1);
		data.Write8(value);

		ReadRegister();
	}

	private void WriteRegister(byte cmd, byte value)
	{
		Wait(1);
		command.Write8(cmd);
		Wait(1);
		data.Write8(value);
		Wait(1);

		ReadRegister();
	}

	private byte ReadRegister()
	{
		Wait(0);
		return data.Read8();
	}

	#endregion Private
}
