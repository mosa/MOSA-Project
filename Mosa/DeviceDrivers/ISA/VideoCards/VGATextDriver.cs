/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.ClassLib;
using Mosa.DeviceDrivers;
using Mosa.DeviceDrivers.PCI;
using Mosa.DeviceDrivers.Kernel;

namespace Mosa.DeviceDrivers.ISA.VideoCards
{
	[ISADeviceSignature(AutoLoad = true, BasePort = 0x03B0, PortRange = 0x1F, BaseAddress = 0xB0000, AddressRange = 0x10000)]
	public class VGATextDriver : ISAHardwareDevice, IDevice, ITextDevice
	{
		#region Definitions

		internal struct CRTCommands
		{
			internal const byte HorizontalTotal = 0x00;
			internal const byte HorizontalDisplayed = 0x01;
			internal const byte HorizontalSyncPosition = 0x02;
			internal const byte HorizontalSyncPulseWidth = 0x03;
			internal const byte VerticalTotal = 0x04;
			internal const byte VerticalDisplayed = 0x05;
			internal const byte VerticalSyncPosition = 0x06;
			internal const byte VerticalSuncPulseWidth = 0x07;
			internal const byte InterlaceMode = 0x08;
			internal const byte MaximumScanLines = 0x09;
			internal const byte CursorStart = 0x0A;
			internal const byte CursorEnd = 0x0B;
			internal const byte StartAddress = 0x0C;
			internal const byte StartAddressHigh = 0x0C;
			internal const byte StartAddressLow = 0x0D;
			internal const byte CursorLocationHigh = 0x0E;
			internal const byte CursorLocationLow = 0x0F;
			internal const byte LightPenHigh = 0x10;
			internal const byte LightPenLow = 0x11;
		}

		#endregion

		protected IReadWriteIOPort miscellaneousOutput;
		protected IReadWriteIOPort crtControllerIndex;
		protected IReadWriteIOPort crtControllerData;
		protected IReadWriteIOPort crtControllerIndexColor;
		protected IReadWriteIOPort crtControllerDataColor;

		protected IMemory memory;

		protected IReadWriteIOPort activeControllerIndex;
		protected IReadWriteIOPort activeControllerData;

		protected bool colorMode = false;
		protected uint offset = 0x8000;
		protected byte width = 80;
		protected byte height = 25;
		protected byte bytePerChar = 2;

		protected TextColor defaultBackground = TextColor.White;

		public VGATextDriver() { }
		public void Dispose() { }

		public override bool Setup()
		{
			base.name = "VGA";

			miscellaneousOutput = base.busResources.GetIOPortRegion(0).GetIOPort((byte)(0x3CC - 0x3B0));

			crtControllerIndex = base.busResources.GetIOPortRegion(0).GetIOPort((byte)(0x3B4 - 0x3B0));
			crtControllerData = base.busResources.GetIOPortRegion(0).GetIOPort((byte)(0x3B5 - 0x3B0));

			crtControllerIndexColor = base.busResources.GetIOPortRegion(0).GetIOPort((byte)(0x3D4 - 0x3B0));
			crtControllerDataColor = base.busResources.GetIOPortRegion(0).GetIOPort((byte)(0x3D5 - 0x3B0));

			memory = base.busResources.GetMemoryRegion(0).GetMemory();

			return true;
		}

		public override bool Start()
		{
			colorMode = ((miscellaneousOutput.Read8() & 1) == 1);

			if (colorMode) {
				offset = 0x8000;
				bytePerChar = 2;
				activeControllerIndex = crtControllerIndexColor;
				activeControllerData = crtControllerDataColor;
			}
			else {
				offset = 0x0;
				bytePerChar = 1;
				activeControllerIndex = crtControllerIndex;
				activeControllerData = crtControllerData;
			}

			width = GetValue(CRTCommands.HorizontalDisplayed);
			height = GetValue(CRTCommands.VerticalDisplayed);

			width++;
			width = (byte)(width / bytePerChar);

			height = 25; // override for bug?

			return true;
		}

		public override bool Probe() { return true; }
		public override LinkedList<IDevice> CreateSubDevices() { return null; }
		public override bool OnInterrupt() { return true; }

		protected void SendCommand(byte command, byte value)
		{
			activeControllerIndex.Write8(command);
			activeControllerData.Write8(value);
		}

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
		/// <returns></returns>
		public byte GetWidth() { return width; }

		/// <summary>
		/// Gets the height.
		/// </summary>
		/// <returns></returns>
		public byte GetHeight() { return height; }

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
			if (colorMode) {
				uint index = (ushort)(offset + (((y * width) + x) * 2));
				memory[index] = (byte)c;
				memory[index + 1] = (byte)((byte)foreground | ((byte)background << 4));
			}
			else {
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
		public void Clear()
		{
			uint index = offset;
			uint size = (uint)(height * width);

			if (bytePerChar == 2)
				for (int i = 0; i < size; i = i + bytePerChar) {
					memory[(uint)(index + i)] = 0;
					memory[(uint)(index + i + 1)] = (byte)defaultBackground;
				}
			else
				for (int i = 0; i < size * bytePerChar; i++)
					memory[(uint)(index + i)] = 0;
		}

		public void ScrollUp()
		{
			uint index = offset;
			uint size = (uint)(((height * width) - width) * bytePerChar);

			for (uint i = index; i < (index + size); i++)
				memory[i] = memory[i + width];

			index = (uint)(index + ((height - 1) * width * bytePerChar));

			for (int i = 0; i < size; i++)
				memory[(uint)(index + i)] = 0;
		}
	}
}
