// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;
using System;

namespace Mosa.DeviceDriver.ISA
{
	//https://github.com/nifanfa/MOSA-Core/blob/master/Source/Mosa.External.x86/Driver/Input/PS2Mouse.cs
	//https://forum.osdev.org/viewtopic.php?t=10247
	//https://wiki.osdev.org/Mouse_Input

	public class StandardMouse : BaseDeviceDriver, IMouseDevice
	{
		private BaseIOPortReadWrite command;
		private BaseIOPortReadWrite data;

		private const byte SetDefaults = 0xF6, EnableDataReporting = 0xF4;

		private int mouseState, screenWidth, screenHeight, phase = 0, aX, aY;

		private byte[] mData = new byte[3];

		public int X, Y;

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
			byte status = ((byte)(data.Read8() | 3));
			Wait(1);
			command.Write8(0x60);
			Wait(1);
			data.Write8(status);

			WriteRegister(SetDefaults);
			WriteRegister(EnableDataReporting);

			// Get "MouseID", but somehow do nothing with it (?)
			WriteRegister(0xF2);

			// Set sample rate to 200 Hz (maximum)
			// We're gradually going down to 80 Hz, see below
			WriteRegister(0xF3);
			WriteRegister(200);

			// Set sample rate to 100 Hz
			// See below for final register write of sample rate
			WriteRegister(0xF3);
			WriteRegister(100);

			// Set sample rate to 800 Hz
			// We're done!
			WriteRegister(0xF3);
			WriteRegister(80);

			// Get "MouseID"
			WriteRegister(0xF2);
			byte result = ReadRegister();

			if (result == 3)
			{
				// The scroll wheel is available
			}

			mouseState = byte.MaxValue;
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
				if ((D & (1 << 3)) == (1 << 3))
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
				mouseState = mData[0] switch
				{
					0x01 => 0,
					0x02 => 1,
					// TODO: Add scroll wheel
					_ => byte.MaxValue,
				};

				if (mData[1] > 127)
					aX = -(255 - mData[1]);
				else
					aX = mData[1];

				if (mData[2] > 127)
					aY = -(255 - mData[2]);
				else
					aY = mData[2];

				X = Math.Clamp(X + aX, 0, screenWidth);
				Y = Math.Clamp(Y - aY, 0, screenHeight);
			}

			return true;
		}

		public int GetMouseState()
		{
			return mouseState;
		}

		public void SetScreenResolution(int width, int height)
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

		private byte ReadRegister()
		{
			Wait(0);
			return data.Read8();
		}

		#endregion
	}
}
