/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.ClassLib;
using Mosa.DeviceSystem;

namespace Mosa.DeviceDrivers.ISA
{
	/// <summary>
	/// VGA Text Device Driver
	/// </summary>
	//[DeviceSignature(AutoLoad = true, BasePort = 0x03B0, PortRange = 0x1F, BaseAddress = 0xB0000, AddressRange = 0x10000, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	public class VGAText : HardwareDevice, IDevice, ITextDevice
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

		#endregion

		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort miscellaneousOutput;
		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort crtControllerIndex;
		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort crtControllerData;
		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort crtControllerIndexColor;
		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort crtControllerDataColor;

		/// <summary>
		/// 
		/// </summary>
		protected IMemory memory;

		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort activeControllerIndex;
		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort activeControllerData;

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

		/// <summary>
		/// Initializes a new instance of the <see cref="VGAText"/> class.
		/// </summary>
		public VGAText() { }

		/// <summary>
		/// Setups this hardware device driver
		/// </summary>
		/// <returns></returns>
		public override bool Setup(IHardwareResources hardwareResources)
		{
			this.hardwareResources = hardwareResources;
			base.name = "VGAText";

			miscellaneousOutput = base.hardwareResources.GetIOPort(0, (byte)(0x3CC - 0x3B0));

			crtControllerIndex = base.hardwareResources.GetIOPort(0, (byte)(0x3B4 - 0x3B0));
			crtControllerData = base.hardwareResources.GetIOPort(0, (byte)(0x3B5 - 0x3B0));

			crtControllerIndexColor = base.hardwareResources.GetIOPort(0, (byte)(0x3D4 - 0x3B0));
			crtControllerDataColor = base.hardwareResources.GetIOPort(0, (byte)(0x3D5 - 0x3B0));

			memory = base.hardwareResources.GetMemory(0);

			return true;
		}

		/// <summary>
		/// Starts this hardware device.
		/// </summary>
		/// <returns></returns>
		public override DeviceDriverStartStatus Start()
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

			width = GetValue(CRTCommands.HorizontalDisplayEnableEnd);
			height = GetValue(CRTCommands.VerticalDisplayEnableEnd);

			width++;
			width = (byte)(width / bytePerChar);

			height = 25; // override for bug?

			base.deviceStatus = DeviceStatus.Online;
			return DeviceDriverStartStatus.Started;
		}

		/// <summary>
		/// Creates the sub devices.
		/// </summary>
		/// <returns></returns>		
		public override LinkedList<IDevice> CreateSubDevices() { return null; }

		/// <summary>
		/// Called when an interrupt is received.
		/// </summary>
		/// <returns></returns>
		public override bool OnInterrupt() { return true; }

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
		public byte Width { get { return width; } }

		/// <summary>
		/// Gets the height.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		public byte Height { get { return height; } }

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
		public void ClearScreen()
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

		/// <summary>
		/// Scrolls up.
		/// </summary>
		public void ScrollUp()
		{
			uint index = offset;
			uint size = (uint)(((height * width) - width) * bytePerChar);

			for (uint i = index; i < (index + size); i++)
				memory[i] = memory[(uint)(i + (width * bytePerChar))];

			index = (uint)(index + ((height - 1) * width * bytePerChar));

			for (int i = 0; i < width * 2; i++)
				memory[(uint)(index + i)] = 0;
		}
	}
}
