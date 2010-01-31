/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System;
using System.IO;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Text;

using Mosa.ClassLib;
using Mosa.EmulatedKernel;
using Mosa.EmulatedDevices.Synthetic;

namespace Mosa.EmulatedDevices.Emulated
{

	/// <summary>
	/// Represents an emulated VGA Text Device
	/// </summary>
	public class VGAConsole : IHardwareDevice, IIOPortDevice
	{
		private DisplayForm dislayForm;
		private Font font;
		private int fontWidth;
		private int fontHeight;
		private Mosa.DeviceSystem.ColorPalette palette;

		private int cursorX = 0;
		private int cursorY = 0;

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

		#region Variables

		/// <summary>
		/// 
		/// </summary>
		public const ushort StandardIOBase = 0x03B0;

		/// <summary>
		/// 
		/// </summary>
		public const uint StandardAddressBase = 0xB0000;

		/// <summary>
		/// 
		/// </summary>
		public const uint StandardMemorySize = 0x10000;

		/// <summary>
		/// 
		/// </summary>
		protected ushort ioBase;

		/// <summary>
		/// 
		/// </summary>
		protected byte miscellaneous;

		/// <summary>
		/// 
		/// </summary>
		protected byte crtControllerIndex;

		/// <summary>
		/// 
		/// </summary>
		protected byte crtControllerData;

		/// <summary>
		/// 
		/// </summary>
		protected byte crtControllerIndexColor;

		/// <summary>
		/// 
		/// </summary>
		protected byte crtControllerDataColor;

		/// <summary>
		/// 
		/// </summary>
		protected byte[] memory = new byte[StandardMemorySize];

		/// <summary>
		/// 
		/// </summary>
		protected uint baseAddress;

		/// <summary>
		/// 
		/// </summary>
		protected byte height;

		/// <summary>
		/// 
		/// </summary>
		protected byte width;

		/// <summary>
		/// 
		/// </summary>
		protected ushort cursorPosition;

		/// <summary>
		/// 
		/// </summary>
		protected byte lastCommand;

		#endregion

		/// <summary>
		/// Initializes a new instance of the <see cref="VGAConsole"/> class.
		/// </summary>
		public VGAConsole(DisplayForm dislayForm)
		{
			ioBase = StandardIOBase;
			baseAddress = StandardAddressBase;

			width = 80;
			height = 27;

			cursorX = cursorY = 0;
			cursorPosition = 0;
			lastCommand = 0;

			font = new Font("Lucida Console", 9, FontStyle.Regular);	
			fontWidth = (int)font.SizeInPoints;
			fontHeight = (int)font.SizeInPoints + 5;
			palette = Mosa.DeviceSystem.ColorPalette.CreateStandard16ColorPalette();

			this.dislayForm = dislayForm;
			dislayForm.SetSize(fontWidth * width + 12, fontHeight * height + 10);

			MemoryDispatch.RegisterMemory(baseAddress, StandardMemorySize, 2, Read8, Write8);

			Initialize();
		}


		/// <summary>
		/// Initializes this instance.
		/// </summary>
		/// <returns></returns>
		public bool Initialize()
		{
			miscellaneous = 0x01;	// set color mode
			return true;
		}

		/// <summary>
		/// Resets this instance.
		/// </summary>
		public void Reset()
		{
			Initialize();
		}

		/// <summary>
		/// Reads the specified port.
		/// </summary>
		/// <param name="port"></param>
		/// <returns></returns>
		public byte Read8(ushort port)
		{
			switch (port - ioBase) {
				case 0x04: return ReadIndex();
				case 0x1C: return miscellaneous;
				case 0x25: return ReadIndexColor();
				default: return 0xFF;
			}
		}

		/// <summary>
		/// Reads the specified port.
		/// </summary>
		/// <param name="port"></param>
		/// <returns></returns>
		public ushort Read16(ushort port)
		{
			return (ushort)(Read8(port) | (Read8(port) << 8));
		}

		/// <summary>
		/// Reads the specified port.
		/// </summary>
		/// <param name="port"></param>
		/// <returns></returns>
		public uint Read32(ushort port)
		{
			return (ushort)(Read8(port) | (Read8(port) << 8) | (Read8(port) << 16) | (Read8(port) << 24));
		}

		/// <summary>
		/// Writes to the specified port.
		/// </summary>
		/// <param name="port"></param>
		/// <param name="data"></param>
		public void Write8(ushort port, byte data)
		{
			switch (port - ioBase) {
				case 0x04: WriteCommand(data); return;
				case 0x05: WriteIndex(data); return;
				case 0x1C: miscellaneous = data; return;
				case 0x24: WriteCommandColor(data); return;
				case 0x25: WriteIndex(data); return;
				default: return;
			}
		}

		/// <summary>
		/// Writes to the specified port.
		/// </summary>
		/// <param name="port">The port.</param>
		/// <param name="data">The data.</param>
		public void Write16(ushort port, ushort data)
		{
			Write8(port, (byte)(data & 0xFF));
			Write8(port, (byte)((data >> 8) & 0xFF));
		}

		/// <summary>
		/// Writes to the specified port.
		/// </summary>
		/// <param name="port">The port.</param>
		/// <param name="data">The data.</param>
		public void Write32(ushort port, uint data)
		{
			Write8(port, (byte)(data & 0xFF));
			Write8(port, (byte)((data >> 8) & 0xFF));
			Write8(port, (byte)((data >> 16) & 0xFF));
			Write8(port, (byte)((data >> 24) & 0xFF));
		}

		/// <summary>
		/// Gets the ports requested.
		/// </summary>
		/// <returns></returns>
		public ushort[] GetPortsRequested()
		{
			return PortRange.GetPortList(ioBase, 0x2F);
		}

		/// <summary>
		/// Puts the char.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <param name="c">The c.</param>
		/// <param name="colorindex">The colorindex.</param>
		/// <param name="backgroundindex">The backgroundindex.</param>
		protected void PutChar(ushort x, ushort y, char c, byte colorindex, byte backgroundindex)
		{
			Mosa.DeviceSystem.Color palettecolor = palette.GetColor(colorindex);
			Mosa.DeviceSystem.Color palettebackground = palette.GetColor(backgroundindex);

			Brush color = new SolidBrush(Color.FromArgb(palettecolor.Red, palettecolor.Green, palettecolor.Blue));
			Brush background = new SolidBrush(Color.FromArgb(palettebackground.Red, palettebackground.Green, palettebackground.Blue));

			lock (dislayForm.bitmap) {
				dislayForm.graphic.FillRectangle(background, new Rectangle(x * fontWidth, y * fontHeight, fontWidth + 1, fontHeight + 1));
				dislayForm.graphic.DrawString(c.ToString(), font, color, x * fontWidth, y * fontHeight);
			}
			dislayForm.Changed = true;
		}

		/// <summary>
		/// Reads at the specified address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <returns></returns>
		protected byte Read8(uint address)
		{
			return memory[address - baseAddress];
		}

		/// <summary>
		/// Writes at the specified address.
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="value">The value.</param>
		protected void Write8(uint address, byte value)
		{
			if (memory[address - baseAddress] == value)
				return;

			memory[address - baseAddress] = value;

			if (address % 2 == 1)
				address--;

			int text = memory[address - baseAddress];
			int color = memory[address - baseAddress + 1];

			uint index = address - baseAddress - 0x8000;

			ushort y = (ushort)(index / ((uint)width * 2));
			ushort x2 = (ushort)(index - (y * (uint)width * 2));
			ushort x = (ushort)(x2 >> 1);

			PutChar(x, y, (char)text, (byte)(color & 0x0F), (byte)(color >> 4));
		}

		/// <summary>
		/// Sets the cursor.
		/// </summary>
		protected void SetCursor()
		{
			cursorY = (int)(cursorPosition / width);
			cursorX = (int)(cursorPosition - (cursorY * width));
		}

		/// <summary>
		/// Commands the write.
		/// </summary>
		/// <param name="data">The data.</param>
		protected void WriteCommand(byte data)
		{
			switch (data) {
				case CRTCommands.HorizontalDisplayEnableEnd: crtControllerData = (byte)(width); break;
				case CRTCommands.VerticalDisplayEnableEnd: crtControllerData = height; break;
				default: break;
			}

			lastCommand = data;
		}

		/// <summary>
		/// Commands the color.
		/// </summary>
		/// <param name="data">The data.</param>
		protected void WriteCommandColor(byte data)
		{
			switch (data) {
				case CRTCommands.HorizontalDisplayEnableEnd: crtControllerDataColor = (byte)(width * 2); break;
				case CRTCommands.VerticalDisplayEnableEnd: crtControllerDataColor = height; break;
				default: break;
			}

			lastCommand = data;
		}

		/// <summary>
		/// Writes the index.
		/// </summary>
		/// <param name="data">The data.</param>
		protected void WriteIndex(byte data)
		{
			switch (lastCommand) {
				case CRTCommands.CursorLocationHigh: cursorPosition = (ushort)(((cursorPosition & 0x00FF) | (data << 8))); SetCursor(); break;
				case CRTCommands.CursorLocationLow: cursorPosition = (ushort)(((cursorPosition & 0xFF00) | data)); SetCursor(); break;
				default: break;
			}
		}

		/// <summary>
		/// Reads the index.
		/// </summary>
		/// <returns></returns>
		protected byte ReadIndex()
		{
			switch (lastCommand) {
				case CRTCommands.HorizontalDisplayEnableEnd: return (byte)(width - 1);
				case CRTCommands.VerticalDisplayEnableEnd: return (byte)(height - 1);
				default: return 0xFF;
			}
		}

		/// <summary>
		/// Reads the index.
		/// </summary>
		/// <returns></returns>
		protected byte ReadIndexColor()
		{
			switch (lastCommand) {
				case CRTCommands.HorizontalDisplayEnableEnd: return (byte)(width * 2 - 1);
				case CRTCommands.VerticalDisplayEnableEnd: return (byte)(height - 1);
				default: return 0xFF;
			}
		}
	}
}
