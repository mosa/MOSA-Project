﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

namespace Mosa.DeviceDriver.ISA
{
	/// <summary>
	/// VGA Text Device Driver
	/// </summary>
	//[ISADeviceDriver(AutoLoad = true, BasePort = 0x03B0, PortRange = 0x1F, BaseAddress = 0xB0000, AddressRange = 0x10000, Platforms = PlatformArchitecture.X86AndX64)]
	public class VGAText : BaseDeviceDriver, ITextDevice
	{
		#region Definitions

		internal struct CRTCommands
		{
			internal const byte HorizontalTotal = 0x00;
			internal const byte HorizontalDisplayEnableEnd = 0x01;
			internal const byte CursorStart = 0x0A;
			internal const byte CursorEnd = 0x0B;
			internal const byte CursorLocationHigh = 0x0E;
			internal const byte CursorLocationLow = 0x0F;
			internal const byte VerticalDisplayEnableEnd = 0x12;
		}

		#endregion Definitions

		#region Ports

		/// <summary>
		///
		/// </summary>
		protected BaseIOPortReadWrite miscellaneousOutput;

		/// <summary>
		///
		/// </summary>
		protected BaseIOPortReadWrite crtControllerIndex;

		/// <summary>
		///
		/// </summary>
		protected BaseIOPortReadWrite crtControllerData;

		/// <summary>
		///
		/// </summary>
		protected BaseIOPortReadWrite crtControllerIndexColor;

		/// <summary>
		///
		/// </summary>
		protected BaseIOPortReadWrite crtControllerDataColor;

		/// <summary>
		///
		/// </summary>
		protected BaseIOPortWrite miscellaneousOutputWrite;

		/// <summary>
		///
		/// </summary>
		protected BaseIOPortReadWrite sequencerAddress;

		/// <summary>
		///
		/// </summary>
		protected BaseIOPortReadWrite sequencerData;

		/// <summary>
		///
		/// </summary>
		protected BaseIOPortReadWrite graphicsControllerAddress;

		/// <summary>
		///
		/// </summary>
		protected BaseIOPortReadWrite graphicsControllerData;

		/// <summary>
		///
		/// </summary>
		protected BaseIOPortReadWrite inputStatus1ReadB;

		/// <summary>
		///
		/// </summary>
		protected BaseIOPortReadWrite attributeAddress;

		/// <summary>
		///
		/// </summary>
		protected BaseIOPortReadWrite attributeData;

		/// <summary>
		///
		/// </summary>
		protected ConstrainedPointer memory;

		/// <summary>
		///
		/// </summary>
		protected BaseIOPortReadWrite activeControllerIndex;

		/// <summary>
		///
		/// </summary>
		protected BaseIOPortReadWrite activeControllerData;

		#endregion Ports

		/// <summary>
		///
		/// </summary>
		protected bool colorMode = false;

		/// <summary>
		///
		/// </summary>
		protected uint offset = 0x8000;

		/// <summary>
		///
		/// </summary>
		protected byte width = 80;

		/// <summary>
		///
		/// </summary>
		protected byte height = 25;

		/// <summary>
		///
		/// </summary>
		protected byte bytePerChar = 2;

		/// <summary>
		///
		/// </summary>
		protected TextColor defaultBackground = TextColor.White;

		public override void Initialize()
		{
			Device.Name = "VGAText";

			miscellaneousOutput = Device.Resources.GetIOPortReadWrite(0, 0x1C);
			crtControllerIndex = Device.Resources.GetIOPortReadWrite(0, 0x04);
			crtControllerData = Device.Resources.GetIOPortReadWrite(0, 0x05);
			crtControllerIndexColor = Device.Resources.GetIOPortReadWrite(0, 0x24);
			crtControllerDataColor = Device.Resources.GetIOPortReadWrite(0, 0x25);

			miscellaneousOutputWrite = Device.Resources.GetIOPortWrite(0, 0x12);
			sequencerAddress = Device.Resources.GetIOPortReadWrite(0, 0x14);
			sequencerData = Device.Resources.GetIOPortReadWrite(0, 0x15);
			graphicsControllerAddress = Device.Resources.GetIOPortReadWrite(0, 0x1E);
			graphicsControllerData = Device.Resources.GetIOPortReadWrite(0, 0x1F);
			inputStatus1ReadB = Device.Resources.GetIOPortReadWrite(0, 0x2A);
			attributeAddress = Device.Resources.GetIOPortReadWrite(0, 0x10);
			attributeData = Device.Resources.GetIOPortReadWrite(0, 0x11);

			memory = Device.Resources.GetMemory(0);
		}

		public override void Probe() => Device.Status = DeviceStatus.Available;

		public override void Start()
		{
			if (Device.Status != DeviceStatus.Available)
				return;

			WriteSettings(VGAText80x25);

			colorMode = ((miscellaneousOutput.Read8() & 1) == 1);

			if (colorMode)
			{
				offset = 0x8000;
				bytePerChar = 2;
				activeControllerIndex = crtControllerIndexColor;
				activeControllerData = crtControllerDataColor;
			}
			else
			{
				offset = 0x0;
				bytePerChar = 1;
				activeControllerIndex = crtControllerIndex;
				activeControllerData = crtControllerData;
			}

			width = GetValue(CRTCommands.HorizontalDisplayEnableEnd);
			height = GetValue(CRTCommands.VerticalDisplayEnableEnd);

			width++;
			height = 25;

			Device.Status = DeviceStatus.Online;
		}

		/// <summary>
		/// Called when an interrupt is received.
		/// </summary>
		/// <returns></returns>
		public override bool OnInterrupt() => true;

		/// <summary>
		/// Sends the command.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <param name="value">The value.</param>
		protected void SendCommand(byte command, byte value)
		{
			activeControllerIndex.Write8(command);
			activeControllerData.Write8(value);
		}

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <returns></returns>
		protected byte GetValue(byte command)
		{
			activeControllerIndex.Write8(command);
			return activeControllerData.Read8();
		}

		/// <summary>
		/// Sets the size of the cursor.
		/// </summary>
		/// <param name="start">The start.</param>
		/// <param name="end">The end.</param>
		protected void SetCursorSize(byte start, byte end)
		{
			SendCommand(CRTCommands.CursorStart, start);
			SendCommand(CRTCommands.CursorEnd, end);
		}

		/// <summary>
		/// Gets the width.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		public byte Width => width;

		/// <summary>
		/// Gets the height.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		public byte Height => height;

		/// <summary>
		/// Writes the char at the position indicated.
		/// </summary>
		/// <param name="x">The x position.</param>
		/// <param name="y">The y position.</param>
		/// <param name="c">The character.</param>
		/// <param name="foreground">The foreground color.</param>
		/// <param name="background">The background color.</param>
		public void WriteChar(ushort x, ushort y, char c, TextColor foreground, TextColor background)
		{
			if (colorMode)
			{
				uint index = (ushort)(offset + (((y * width) + x) * 2));
				memory[index] = (byte)c;
				memory[index + 1] = (byte)((byte)foreground | ((byte)background << 4));
			}
			else
			{
				uint index = (ushort)(offset + (y * width) + x);
				index = index + x;
				memory[index] = (byte)c;
			}
		}

		/// <summary>
		/// Sets the cursor position.
		/// </summary>
		/// <param name="x">The x position.</param>
		/// <param name="y">The y position.</param>
		public void SetCursor(ushort x, ushort y)
		{
			uint position = (uint)(x + (y * width));
			SendCommand(CRTCommands.CursorLocationHigh, (byte)((position >> 8) & 0xFF));
			SendCommand(CRTCommands.CursorLocationLow, (byte)(position & 0xFF));
		}

		/// <summary>
		/// Clears the screen.
		/// </summary>
		public void ClearScreen()
		{
			uint index = offset;
			uint size = (uint)(height * width);

			if (bytePerChar == 2)
			{
				for (int i = 0; i < size; i++)
				{
					memory[(uint)(index + (i * 2))] = 0;
					memory[(uint)(index + (i * 2) + 1)] = (byte)((byte)defaultBackground << 4);
				}
			}
			else
			{
				for (int i = 0; i < size; i = i + bytePerChar)
				{
					memory[(uint)(index + i)] = 0;
				}
			}
		}

		/// <summary>
		/// Scrolls up.
		/// </summary>
		public void ScrollUp()
		{
			uint index = offset;
			uint size = (uint)(((height * width) - width) * bytePerChar);

			for (uint i = index; i < (index + size); i++)
			{
				memory[i] = memory[(uint)(i + (width * bytePerChar))];
			}

			index = (uint)(index + ((height - 1) * width * bytePerChar));

			for (int i = 0; i < width * 2; i++)
			{
				memory[(uint)(index + i)] = 0;
			}
		}

		/// <summary>
		/// Writes the settings.
		/// </summary>
		/// <param name="settings">The settings.</param>
		protected void WriteSettings(byte[] settings)
		{
			// Write MISCELLANEOUS reg
			miscellaneousOutputWrite.Write8(settings[0]);

			// Write SEQUENCER regs
			for (byte i = 0; i < 5; i++)
			{
				sequencerAddress.Write8(i);
				sequencerData.Write8(settings[1 + i]);
			}

			// Unlock CRTC registers
			crtControllerIndexColor.Write8(0x03);
			crtControllerDataColor.Write8((byte)(crtControllerData.Read8() | 0x80));
			crtControllerIndexColor.Write8(0x11);
			crtControllerDataColor.Write8((byte)(crtControllerData.Read8() & 0x7F));

			// Make sure they remain unlocked
			settings[0x03] = (byte)(settings[0x03] | 0x80);
			settings[0x11] = (byte)(settings[0x11] & 0x7F);

			// Write CRTC regs
			for (byte i = 0; i < 25; i++)
			{
				crtControllerIndexColor.Write8(i);
				crtControllerDataColor.Write8(settings[6 + i]);
			}

			// Write GRAPHICS CONTROLLER regs
			for (byte i = 0; i < 9; i++)
			{
				graphicsControllerAddress.Write8(i);
				graphicsControllerData.Write8(settings[31 + i]);
			}

			// Write ATTRIBUTE CONTROLLER regs
			for (byte i = 0; i < 21; i++)
			{
				inputStatus1ReadB.Read8();
				attributeAddress.Write8(i);
				attributeAddress.Write8(settings[40 + i]); // TODO: Double check
			}

			// Lock 16-color palette and unblank display */
			inputStatus1ReadB.Read8();
			attributeAddress.Write8(0x20);
		}

		#region Modes

		private static readonly byte[] VGAText80x25 = new byte[] {
		/* MISC */
			0x67,
		/* SEQ */
			0x03, 0x00, 0x03, 0x00, 0x02,
		/* CRTC */
			0x5F, 0x4F, 0x50, 0x82, 0x55, 0x81, 0xBF, 0x1F,
			0x00, 0x4F, 0x0D, 0x0E, 0x00, 0x00, 0x00, 0x50,
			0x9C, 0x0E, 0x8F, 0x28, 0x1F, 0x96, 0xB9, 0xA3,
			0xFF,
		/* GC */
			0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x0E, 0x00,
			0xFF,
		/* AC */
			0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x14, 0x07,
			0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F,
			0x0C, 0x00, 0x0F, 0x08, 0x00
		};

		#endregion Modes
	}
}
