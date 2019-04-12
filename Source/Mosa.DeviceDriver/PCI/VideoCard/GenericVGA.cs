// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

/*
 * Portions of this device driver was adapted from:
 *
 * Sets VGA-compatible video modes without using the BIOS
 * Chris Giese <geezer@execpc.com>	http://www.execpc.com/~geezer
 * This code is public domain (no copyright).
 * You can do whatever you want with it.
 */

namespace Mosa.DeviceDriver.PCI.VideoCard
{
	/// <summary>
	/// Generic VGA Device Driver
	/// </summary>
	//[PCIDeviceDriver(ClassCode = 0X03, SubClassCode = 0x00, ProgIF = 0x00, Platforms = PlatformArchitecture.X86AndX64)]
	public class GenericVGA : BaseDeviceDriver, IPixelPaletteGraphicsDevice
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

		#endregion Definitions

		#region Ports

		/// <summary>
		///
		/// </summary>
		protected BaseIOPortWrite miscellaneousOutputWrite;

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
		protected BaseIOPortReadWrite dacPaletteMask;

		/// <summary>
		///
		/// </summary>
		protected BaseIOPortReadWrite dacIndexWrite;

		/// <summary>
		///
		/// </summary>
		protected BaseIOPortReadWrite dacIndexRead;

		/// <summary>
		///
		/// </summary>
		protected BaseIOPortReadWrite dacData;

		/// <summary>
		///
		/// </summary>
		protected BaseIOPortRead inputStatus1;

		/// <summary>
		///
		/// </summary>
		protected BaseIOPortRead miscellaneousOutputRead;

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
		protected BaseIOPortReadWrite activeControllerIndex;

		/// <summary>
		///
		/// </summary>
		protected BaseIOPortReadWrite activeControllerData;

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

		#endregion Ports

		/// <summary>
		///
		/// </summary>
		protected ConstrainedPointer memory;

		/// <summary>
		///
		/// </summary>
		protected uint offset = 0x8000;

		/// <summary>
		///
		/// </summary>
		protected ushort width;

		/// <summary>
		///
		/// </summary>
		protected ushort height;

		/// <summary>
		///
		/// </summary>
		protected ushort colors;

		/// <summary>
		///
		/// </summary>
		private enum WriteMethod : byte { Pixel1, Pixel2, Pixel4p, Pixel8, Pixel8x };

		/// <summary>
		///
		/// </summary>
		private WriteMethod writeMethod;

		public override void Initialize()
		{
			Device.Name = "GenericVGA_0x" + Device.Resources.GetIOPortRegion(0).BaseIOPort.ToString("X");

			byte portBar = (byte)(Device.Resources.IOPointRegionCount - 1);

			miscellaneousOutputRead = Device.Resources.GetIOPortReadWrite(portBar, 0x1C);
			crtControllerIndex = Device.Resources.GetIOPortReadWrite(portBar, 0x04);
			crtControllerData = Device.Resources.GetIOPortReadWrite(portBar, 0x05);
			crtControllerIndexColor = Device.Resources.GetIOPortReadWrite(portBar, 0x24);
			crtControllerDataColor = Device.Resources.GetIOPortReadWrite(portBar, 0x25);
			dacPaletteMask = Device.Resources.GetIOPortReadWrite(portBar, 0x16);
			dacIndexRead = Device.Resources.GetIOPortReadWrite(portBar, 0x17);
			dacIndexWrite = Device.Resources.GetIOPortReadWrite(portBar, 0x18);
			dacData = Device.Resources.GetIOPortReadWrite(portBar, 0x19);
			inputStatus1 = Device.Resources.GetIOPortReadWrite(portBar, 0x12);
			miscellaneousOutputWrite = Device.Resources.GetIOPortWrite(portBar, 0x12);
			sequencerAddress = Device.Resources.GetIOPortReadWrite(portBar, 0x14);
			sequencerData = Device.Resources.GetIOPortReadWrite(portBar, 0x15);
			graphicsControllerAddress = Device.Resources.GetIOPortReadWrite(portBar, 0x1E);
			graphicsControllerData = Device.Resources.GetIOPortReadWrite(portBar, 0x1F);
			inputStatus1ReadB = Device.Resources.GetIOPortReadWrite(portBar, 0x2A);
			attributeAddress = Device.Resources.GetIOPortReadWrite(portBar, 0x10);
			attributeData = Device.Resources.GetIOPortReadWrite(portBar, 0x11);

			memory = Device.Resources.GetMemory((byte)(Device.Resources.AddressRegionCount - 1));
		}

		public override void Start()
		{
			if (Device.Status != DeviceStatus.Available)
				return;

			// TODO

			//if (!SetMode(13))
			//{
			//	Device.Status = DeviceStatus.Error;
			//}

			Device.Status = DeviceStatus.Online;
		}

		public override bool OnInterrupt()
		{
			return false;
		}

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
		/// Writes the pixel.
		/// </summary>
		/// <param name="colorIndex">Index of the color.</param>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		public void WritePixel(byte colorIndex, ushort x, ushort y)
		{
			if (writeMethod == WriteMethod.Pixel8)
			{
				memory.Write8((uint)(y * 320 + x), colorIndex);
			}
			if (writeMethod == WriteMethod.Pixel2)
			{ // ???
				uint address = (uint)(y * 320 + x / 2);
				colorIndex = (byte)(colorIndex & 0xF);

				if ((x & 0x01) == 0)
					memory.Write8(address & 0xF, (byte)(colorIndex << 4));
				else
					memory.Write8(address & 0x0F, colorIndex);
			}

			// TODO: Support more video modes
		}

		/// <summary>
		/// Reads the pixel.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
		public byte ReadPixel(ushort x, ushort y)
		{
			if (writeMethod == WriteMethod.Pixel8)
			{
				return memory.Read8((uint)(y * 320 + x));
			}

			// TODO: Support more video modes
			return 0;
		}

		/// <summary>
		/// Clears device with the specified color index.
		/// </summary>
		/// <param name="colorIndex">Index of the color.</param>
		public void Clear(byte colorIndex)
		{
			// TODO: write faster version
			for (ushort x = 0; x < width; x++)
				for (ushort y = 0; y < height; y++)
					WritePixel(colorIndex, x, y);
		}

		/// <summary>
		/// Sets the palette.
		/// </summary>
		/// <param name="colorIndex">Index of the color.</param>
		/// <param name="color">The color.</param>
		public void SetPalette(byte colorIndex, Color color)
		{
			dacPaletteMask.Write8(0xFF);
			dacIndexWrite.Write8(colorIndex);
			dacData.Write8(color.Red);
			dacData.Write8(color.Green);
			dacData.Write8(color.Blue);
		}

		/// <summary>
		/// Gets the palette.
		/// </summary>
		/// <param name="colorIndex">Index of the color.</param>
		/// <returns></returns>
		public Color GetPalette(byte colorIndex)
		{
			Color color = new Color();

			dacPaletteMask.Write8(0xFF);
			dacIndexRead.Write8(colorIndex);
			color.Red = dacData.Read8();
			color.Green = dacData.Read8();
			color.Blue = dacData.Read8();

			return color;
		}

		/// <summary>
		/// Gets the size of the palette.
		/// </summary>
		/// <value>The size of the palette.</value>
		public ushort PaletteSize { get { return colors; } }

		/// <summary>
		/// Gets the width.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		public ushort Width { get { return width; } }

		/// <summary>
		/// Gets the height.
		/// </summary>
		/// <value></value>
		/// <returns></returns>
		public ushort Height { get { return height; } }

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
			crtControllerDataColor.Write8((byte)(crtControllerData.Read8() & ~0x80));

			// Make sure they remain unlocked
			settings[0x03] = (byte)(settings[0x03] | 0x80);
			settings[0x11] = (byte)(settings[0x11] & ~0x80);

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
				attributeAddress.Write8(settings[52 + i]);
			}

			// Lock 16-color palette and unblank display */
			inputStatus1ReadB.Read8();
			attributeAddress.Write8(0x20);
		}

		/// <summary>
		/// Sets the mode.
		/// </summary>
		/// <param name="mode">The mode.</param>
		/// <returns></returns>
		public bool SetMode(byte mode)
		{
			switch (mode)
			{
				case 4: { WriteSettings(VGA320x200x4); width = 320; height = 200; colors = 4; writeMethod = WriteMethod.Pixel2; return true; }
				case 5: { WriteSettings(VGA320x200x4); width = 320; height = 200; colors = 4; writeMethod = WriteMethod.Pixel2; return true; }

				//case 11: { WriteSettings(VGA640x480x2); width = 640; height = 480; colors = 2; writeMethod = WriteMethod.xxx; return true; }
				//case 12: { WriteSettings(VGA640x480x16); width = 640; height = 480; colors = 16; writeMethod = WriteMethod.xxx; return true; }
				case 13: { WriteSettings(VGA320x200x256); width = 320; height = 200; colors = 256; writeMethod = WriteMethod.Pixel8; return true; }

				// Custom Standard Modes:
				//case 99: { WriteSettings(VGA720x480x16); width = 720; height = 480; colors = 16; writeMethod = WriteMethod.xxx; return true; }
				default: { return false; }
			}
		}

		#region Modes

		private static byte[] VGA320x200x4 = new byte[] {
			/* MISC */
			0x63,
			/* SEQ */
			0x03, 0x09, 0x03, 0x00, 0x02,
			/* CRTC */
			0x2D, 0x27, 0x28, 0x90, 0x2B, 0x80, 0xBF, 0x1F,
			0x00, 0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
			0x9C, 0x0E, 0x8F, 0x14, 0x00, 0x96, 0xB9, 0xA3,
			0xFF,
			/* GC */
			0x00, 0x00, 0x00, 0x00, 0x00, 0x30, 0x02, 0x00,
			0xFF,
			/* AC */
			0x00, 0x13, 0x15, 0x17, 0x02, 0x04, 0x06, 0x07,
			0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16, 0x17,
			0x01, 0x00, 0x03, 0x00, 0x00
		};

		private static byte[] VGA640x480x2 = new byte[] {
			/* MISC */
			0xE3,
			/* SEQ */
			0x03, 0x01, 0x0F, 0x00, 0x06,
			/* CRTC */
			0x5F, 0x4F, 0x50, 0x82, 0x54, 0x80, 0x0B, 0x3E,
			0x00, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
			0xEA, 0x0C, 0xDF, 0x28, 0x00, 0xE7, 0x04, 0xE3,
			0xFF,
			/* GC */
			0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x05, 0x0F,
			0xFF,
			/* AC */
			0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x14, 0x07,
			0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F,
			0x01, 0x00, 0x0F, 0x00, 0x00
		};

		private static byte[] VGA720x480x16 = new byte[]    {
			/* MISC */
			0xE7,
			/* SEQ */
			0x03, 0x01, 0x08, 0x00, 0x06,
			/* CRTC */
			0x6B, 0x59, 0x5A, 0x82, 0x60, 0x8D, 0x0B, 0x3E,
			0x00, 0x40, 0x06, 0x07, 0x00, 0x00, 0x00, 0x00,
			0xEA, 0x0C, 0xDF, 0x2D, 0x08, 0xE8, 0x05, 0xE3,
			0xFF,
			/* GC */
			0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x05, 0x0F,
			0xFF,
			/* AC */
			0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
			0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F,
			0x01, 0x00, 0x0F, 0x00, 0x00,
		};

		private static byte[] VGA320x200x256 = new byte[] {
			/* MISC */
			0x63,
			/* SEQ */
			0x03, 0x01, 0x0F, 0x00, 0x0E,
			/* CRTC */
			0x5F, 0x4F, 0x50, 0x82, 0x54, 0x80, 0xBF, 0x1F,
			0x00, 0x41, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
			0x9C, 0x0E, 0x8F, 0x28, 0x40, 0x96, 0xB9, 0xA3,
			0xFF,
			/* GC */
			0x00, 0x00, 0x00, 0x00, 0x00, 0x40, 0x05, 0x0F,
			0xFF,
			/* AC */
			0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
			0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F,
			0x41, 0x00, 0x0F, 0x00, 0x00
		};

		private static byte[] VGA640x480x16 = new byte[] {
			/* MISC */
			0xE3,
			/* SEQ */
			0x03, 0x01, 0x08, 0x00, 0x06,
			/* CRTC */
			0x5F, 0x4F, 0x50, 0x82, 0x54, 0x80, 0x0B, 0x3E,
			0x00, 0x40, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
			0xEA, 0x0C, 0xDF, 0x28, 0x00, 0xE7, 0x04, 0xE3,
			0xFF,
			/* GC */
			0x00, 0x00, 0x00, 0x00, 0x03, 0x00, 0x05, 0x0F,
			0xFF,
			/* AC */
			0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x14, 0x07,
			0x38, 0x39, 0x3A, 0x3B, 0x3C, 0x3D, 0x3E, 0x3F,
			0x01, 0x00, 0x0F, 0x00, 0x00
		};

		#endregion Modes
	}
}
