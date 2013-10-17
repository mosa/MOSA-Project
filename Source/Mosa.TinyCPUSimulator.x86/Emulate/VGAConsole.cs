/*
 * (c) 2013 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using System.Drawing;

namespace Mosa.TinyCPUSimulator.x86.Emulate
{
	/// <summary>
	/// Represents an emulated VGA Text Device
	/// </summary>
	public class VGAConsole : BaseSimDevice
	{
		private DisplayForm dislayForm;
		private Font font;
		private int fontWidth;
		private int fontHeight;

		#region Definitions

		private static byte[,] palette = new byte[,]
		{
			{0, 0, 0, 0},
			{1, 0, 0, 170},
			{2, 0, 170, 0},
			{3, 0, 170, 170},
			{4, 170, 0, 0},
			{5, 170, 0, 170},
			{6, 170, 85, 0},
			{7, 170, 170, 170},
			{8, 85, 85, 85},
			{9, 85, 85, 255},
			{10, 85, 255, 85},
			{11, 85, 255, 255},
			{12, 255, 85, 85},
			{13, 255, 85, 255},
			{14, 255, 255, 85},
			{15, 255, 255, 255},
		};

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

		#endregion Variables

		/// <summary>
		/// Initializes a new instance of the <see cref="VGAConsole"/> class.
		/// </summary>
		public VGAConsole(SimCPU simCPU, DisplayForm dislayForm)
			: base(simCPU)
		{
			this.dislayForm = dislayForm;

			ioBase = StandardIOBase;
			baseAddress = StandardAddressBase;

			width = 80;
			height = 27;

			cursorPosition = 0;
			lastCommand = 0;

			font = new Font("Lucida Console", 9, FontStyle.Regular);
			fontWidth = (int)font.SizeInPoints;
			fontHeight = (int)font.SizeInPoints + 5;

			dislayForm.SetSize(fontWidth * width + 12, fontHeight * height + 10);
		}

		public override void Initialize()
		{
			simCPU.AddMemory(baseAddress, StandardMemorySize, 2);
		}

		public override void Reset()
		{
			miscellaneous = 0x01;	// set color mode

			for (ulong a = baseAddress; a < baseAddress + StandardMemorySize; a++)
				simCPU.DirectWrite8(a, 0);
		}

		public override void MemoryWrite(ulong address, byte size)
		{
			for (ulong a = address; a < address + (ulong)(size / 8); a++)
			{
				if (a >= baseAddress && a < baseAddress + StandardAddressBase)
					Write8((uint)a, simCPU.DirectRead8(a));
			}
		}

		public override void PortWrite(uint port, byte value)
		{
			switch (port - ioBase)
			{
				case 0x04: WriteCommand(value); return;
				case 0x05: WriteIndex(value); return;
				case 0x1C: miscellaneous = value; return;
				case 0x24: WriteCommandColor(value); return;
				case 0x25: WriteIndex(value); return;
				default: return;
			}
		}

		public override byte PortRead(uint port)
		{
			switch (port - ioBase)
			{
				case 0x04: return ReadIndex();
				case 0x1C: return miscellaneous;
				case 0x25: return ReadIndexColor();
				default: return 0xFF;
			}
		}

		public override ushort[] GetPortList()
		{
			return GetPortList(ioBase, 0x2F);
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
		/// Puts the char.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <param name="c">The c.</param>
		/// <param name="colorindex">The colorindex.</param>
		/// <param name="backgroundindex">The backgroundindex.</param>
		protected void PutChar(ushort x, ushort y, char c, byte colorindex, byte backgroundindex)
		{
			Brush color = new SolidBrush(Color.FromArgb(palette[colorindex, 1], palette[colorindex, 2], palette[colorindex, 3]));
			Brush background = new SolidBrush(Color.FromArgb(palette[backgroundindex, 1], palette[backgroundindex, 2], palette[backgroundindex, 3]));

			lock (dislayForm.bitmap)
			{
				dislayForm.graphic.FillRectangle(background, new Rectangle(x * fontWidth, y * fontHeight, fontWidth + 1, fontHeight + 1));
				dislayForm.graphic.DrawString(c.ToString(), font, color, x * fontWidth, y * fontHeight);
			}
			dislayForm.Changed = true;
		}

		/// <summary>
		/// Sets the cursor.
		/// </summary>
		protected void SetCursor()
		{
			//cursorY = (int)(cursorPosition / width);
			//cursorX = (int)(cursorPosition - (cursorY * width));
		}

		/// <summary>
		/// Commands the write.
		/// </summary>
		/// <param name="data">The data.</param>
		protected void WriteCommand(byte data)
		{
			switch (data)
			{
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
			switch (data)
			{
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
			switch (lastCommand)
			{
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
			switch (lastCommand)
			{
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
			switch (lastCommand)
			{
				case CRTCommands.HorizontalDisplayEnableEnd: return (byte)(width * 2 - 1);
				case CRTCommands.VerticalDisplayEnableEnd: return (byte)(height - 1);
				default: return 0xFF;
			}
		}
	}
}