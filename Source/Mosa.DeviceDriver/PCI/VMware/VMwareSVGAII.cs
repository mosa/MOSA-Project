// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.DeviceSystem;

/*
 * Portions of this code is:
 *
 * Copyright 1998-2001, VMware, Inc.
 * Distributed under the terms of the MIT License.
 *
 */

namespace Mosa.DeviceDriver.PCI.VMware
{
	/// <summary>
	/// VMware SVGA II Device Driver
	/// </summary>
	//[PCIDeviceDriver(VendorID = 0x15AD, DeviceID = 0x0405, Platforms = PlatformArchitecture.X86AndX64)]
	public class VMwareSVGAII : BaseDeviceDriver, IPixelGraphicsDevice
	{
		#region Definitions

		internal struct Register
		{
			internal const byte ID = 0;
			internal const byte Enable = 1;
			internal const byte Width = 2;
			internal const byte Height = 3;
			internal const byte MaxWidth = 4;
			internal const byte MaxHeight = 5;
			internal const byte Depth = 6;
			internal const byte BitsPerPixel = 7; /* Current bpp in the guest */
			internal const byte PseudoColor = 8;
			internal const byte RedMask = 9;
			internal const byte GreenMask = 10;
			internal const byte BlueMask = 11;
			internal const byte BytesPerLine = 12;
			internal const byte FrameBufferStart = 13;
			internal const byte FrameBufferOffset = 14;
			internal const byte VRamSize = 15;
			internal const byte FrameBufferSize = 16;

			/* ID 0 implementation only had the above registers; then the palette */

			internal const byte Capabilities = 17;
			internal const byte MemStart = 18;      /* Memory for command FIFO and bitmaps */
			internal const byte MemSize = 19;
			internal const byte ConfigDone = 20;    /* Set when memory area configured */
			internal const byte Sync = 21;          /* Write to force synchronization */
			internal const byte Busy = 22;          /* Read to check if sync is done */
			internal const byte GuestID = 23;       /* Set guest OS identifier */
			internal const byte CursorID = 24;      /* ID of cursor */
			internal const byte CursorX = 25;       /* Set cursor X position */
			internal const byte CursorY = 26;       /* Set cursor Y position */
			internal const byte CursorOn = 27;      /* Turn cursor on/off */
			internal const byte HostBitsPerPixel = 28; /* Current bpp in the host */
			internal const byte ScratchSize = 29;   /* Number of scratch registers */
			internal const byte MemRegs = 30;       /* Number of FIFO registers */
			internal const byte NumDisplays = 31;   /* Number of guest displays */
			internal const byte PitchLock = 32;     /* Fixed pitch for all modes */
		}

		internal struct ID
		{
			internal const uint Magic = 0x900000;
			internal const uint V0 = Magic << 8;
			internal const uint V1 = (Magic << 8) | 1;
			internal const uint V2 = (Magic << 8) | 2;
			internal const uint Invalid = 0xFFFFFFFF;
		}

		internal struct Fifo
		{
			internal const uint Min = 0x00;
			internal const uint Max = 0x01;
			internal const uint NextCmd = 0x02;
			internal const uint Stop = 0x03;
		}

		internal struct FifoCommand
		{
			internal const uint Update = 1;
			internal const uint RECT_FILL = 2;
			internal const uint RECT_COPY = 3;
			internal const uint DEFINE_BITMAP = 4;
			internal const uint DEFINE_BITMAP_SCANLINE = 5;
			internal const uint DEFINE_PIXMAP = 6;
			internal const uint DEFINE_PIXMAP_SCANLINE = 7;
			internal const uint RECT_BITMAP_FILL = 8;
			internal const uint RECT_PIXMAP_FILL = 9;
			internal const uint RECT_BITMAP_COPY = 10;
			internal const uint RECT_PIXMAP_COPY = 11;
			internal const uint FREE_OBJECT = 12;
			internal const uint RECT_ROP_FILL = 13;
			internal const uint RECT_ROP_COPY = 14;
			internal const uint RECT_ROP_BITMAP_FILL = 15;
			internal const uint RECT_ROP_PIXMAP_FILL = 16;
			internal const uint RECT_ROP_BITMAP_COPY = 17;
			internal const uint RECT_ROP_PIXMAP_COPY = 18;
			internal const uint DEFINE_CURSOR = 19;
			internal const uint DISPLAY_CURSOR = 20;
			internal const uint MOVE_CURSOR = 21;
			internal const uint DEFINE_ALPHA_CURSOR = 22;
		}

		#endregion Definitions

		#region Ports

		protected BaseIOPortReadWrite indexPort;

		protected BaseIOPortReadWrite valuePort;

		#endregion Ports

		protected ConstrainedPointer memory;

		protected ConstrainedPointer fifo;

		protected IFrameBuffer frameBuffer;

		/// <summary>
		/// The width
		/// </summary>
		protected ushort width;

		/// <summary>
		/// The height
		/// </summary>
		protected ushort height;

		/// <summary>
		/// The version
		/// </summary>
		protected uint version;

		/// <summary>
		/// The offset
		/// </summary>
		protected uint offset;

		/// <summary>
		/// The video ram size
		/// </summary>
		protected uint videoRamSize;

		/// <summary>
		/// The maximum width
		/// </summary>
		protected uint maxWidth;

		/// <summary>
		/// The maximum height
		/// </summary>
		protected uint maxHeight;

		/// <summary>
		/// The bits per pixel
		/// </summary>
		protected uint bitsPerPixel;

		/// <summary>
		/// The bytes per line
		/// </summary>
		protected uint bytesPerLine;

		/// <summary>
		/// The red mask
		/// </summary>
		protected uint redMask;

		/// <summary>
		/// The green mask
		/// </summary>
		protected uint greenMask;

		/// <summary>
		/// The blue mask
		/// </summary>
		protected uint blueMask;

		/// <summary>
		/// The alpha mask
		/// </summary>
		protected uint alphaMask;

		/// <summary>
		/// The red mask shift
		/// </summary>
		protected byte redMaskShift;

		/// <summary>
		/// The green mask shift
		/// </summary>
		protected byte greenMaskShift;

		/// <summary>
		/// The blue mask shift
		/// </summary>
		protected byte blueMaskShift;

		/// <summary>
		/// The alpha mask shift
		/// </summary>
		protected byte alphaMaskShift;

		/// <summary>
		/// The capabilities
		/// </summary>
		protected uint capabilities;

		/// <summary>
		/// The frame buffer size
		/// </summary>
		protected uint frameBufferSize;

		/// <summary>
		/// The fifo size
		/// </summary>
		protected uint fifoSize;

		/// <summary>
		/// The fifo number regs
		/// </summary>
		protected const uint FifoNumRegs = 32 + 255 + 1 + 1 + 1;

		public override void Initialize()
		{
			Device.Name = "VMWARE_SVGA_0x" + Device.Resources.GetIOPortRegion(0).BaseIOPort.ToString("X");

			indexPort = Device.Resources.GetIOPortReadWrite(0, 0);
			valuePort = Device.Resources.GetIOPortReadWrite(0, 1);
			memory = Device.Resources.GetMemory(0);
			fifo = Device.Resources.GetMemory(1);
		}

		public override void Start()
		{
		}

		public void _Start()
		{
			videoRamSize = ReadRegister(Register.VRamSize);
			maxWidth = ReadRegister(Register.MaxWidth);
			maxHeight = ReadRegister(Register.MaxHeight);
			bitsPerPixel = ReadRegister(Register.BitsPerPixel);
			bytesPerLine = ReadRegister(Register.BytesPerLine);
			redMask = ReadRegister(Register.RedMask);
			greenMask = ReadRegister(Register.GreenMask);
			blueMask = ReadRegister(Register.BlueMask);
			redMaskShift = GetMaskShift(redMask);
			greenMaskShift = GetMaskShift(greenMask);
			blueMaskShift = GetMaskShift(blueMask);
			alphaMaskShift = GetMaskShift(alphaMask);
			offset = ReadRegister(Register.FrameBufferOffset);
			fifoSize = ReadRegister(Register.MemSize);

			//fifoNumRegs = GetValue(Register.MemRegs);

			version = GetVersion();

			if (version != ID.V0)
			{
				SendCommand(Register.GuestID, 0x5010); // 0x05010 == GUEST_OS_OTHER (vs GUEST_OS_WIN2000)
			}

			if (version != ID.V1 || version != ID.V2)
			{
				capabilities = ReadRegister(Register.Capabilities);
			}

			InitializeFifo();

			SetMode(640, 480);

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
		protected void SendCommand(uint command, uint value)
		{
			indexPort.Write32(command);
			valuePort.Write32(value);
		}

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <returns></returns>
		protected uint ReadRegister(uint command)
		{
			indexPort.Write32(command);
			return valuePort.Read32();
		}

		/// <summary>
		/// Sets the mode.
		/// </summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <returns></returns>
		public bool SetMode(ushort width, ushort height)
		{
			this.width = width;
			this.height = height;

			// set height  & width
			SendCommand(Register.Height, height);
			SendCommand(Register.Width, width);
			SendCommand(Register.BitsPerPixel, 32);
			SendCommand(Register.Enable, 1);

			// use the host's bits per pixel
			bitsPerPixel = 32; // ReadRegister(Register.BitsPerPixel);

			// get the frame buffer offset
			offset = ReadRegister(Register.FrameBufferOffset);

			// get the bytes per line (pitch)
			bytesPerLine = ReadRegister(Register.BytesPerLine);

			switch (bitsPerPixel)
			{
				case 8: frameBuffer = new FrameBuffer8bpp(memory, width, height, offset, 1); break;
				case 16: frameBuffer = new FrameBuffer16bpp(memory, width, height, offset, 2); break;
				case 24: frameBuffer = new FrameBuffer24bpp(memory, width, height, offset, 3); break;
				case 32: frameBuffer = new FrameBuffer32bpp(memory, width, height, offset, 4); break;
				default: return false;
			}

			return true;
		}

		/// <summary>
		/// Initializes the fifo.
		/// </summary>
		/// <returns></returns>
		protected void InitializeFifo()
		{
			uint start = ReadRegister(Register.MemStart);
			uint fifoSize = ReadRegister(Register.MemSize);

			SetFifo(Fifo.Min, FifoNumRegs * 4);
			SetFifo(Fifo.Max, fifoSize);
			SetFifo(Fifo.NextCmd, FifoNumRegs * 4);
			SetFifo(Fifo.Stop, FifoNumRegs * 4);

			SendCommand(Register.Enable, 1);
			SendCommand(Register.ConfigDone, 1);
		}

		/// <summary>
		/// Gets the fifo.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		protected uint GetFifo(uint index)
		{
			return fifo.Read32(index * 4);
		}

		/// <summary>
		/// Sets the fifo.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		protected void SetFifo(uint index, uint value)
		{
			fifo.Write32(index * 4, value);
		}

		/// <summary>
		/// Waits for fifo.
		/// </summary>
		protected void WaitForFifo()
		{
			SendCommand(Register.Sync, 1);

			while (ReadRegister(Register.Busy) != 0)
			{
				HAL.Sleep(10);
			}
		}

		/// <summary>
		/// Writes to fifo.
		/// </summary>
		/// <param name="value">The value.</param>
		protected void WriteToFifo(uint value)
		{
			if (((GetFifo(Fifo.NextCmd) == GetFifo(Fifo.Max) - 4) && GetFifo(Fifo.Stop) == GetFifo(Fifo.Min)) ||
				(GetFifo(Fifo.NextCmd) + 4 == GetFifo(Fifo.Stop)))
			{
				WaitForFifo();
			}

			SetFifo(GetFifo(Fifo.NextCmd) / 4, value);
			SetFifo(Fifo.NextCmd, GetFifo(Fifo.NextCmd) + 4);

			if (GetFifo(Fifo.NextCmd) == GetFifo(Fifo.Max))
			{
				SetFifo(Fifo.NextCmd, GetFifo(Fifo.Min));
			}
		}

		/// <summary>
		/// Gets the version.
		/// </summary>
		/// <returns></returns>
		protected uint GetVersion()
		{
			SendCommand(Register.ID, ID.V2);
			if (ReadRegister(Register.ID) == ID.V2)
				return ID.V2;

			SendCommand(Register.ID, ID.V1);
			if (ReadRegister(Register.ID) == ID.V1)
				return ID.V1;

			if (ReadRegister(Register.ID) == ID.V0)
				return ID.V0;

			return ID.Invalid;
		}

		/// <summary>
		/// Gets the mask shift.
		/// </summary>
		/// <param name="mask">The mask.</param>
		/// <returns></returns>
		protected byte GetMaskShift(uint mask)
		{
			if (mask == 0)
				return 0;

			byte count = 0;

			while ((mask & 1) == 0)
			{
				count++;
				mask = mask >> 1;
			}

			return count;
		}

		/// <summary>
		/// Updates the frame.
		/// </summary>
		protected void UpdateFrame()
		{
			UpdateFrame(0, 0, width, height);
		}

		/// <summary>
		/// Updates the frame.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		protected void UpdateFrame(ushort x, ushort y, ushort width, ushort height)
		{
			WriteToFifo(FifoCommand.Update);
			WriteToFifo(x);
			WriteToFifo(y);
			WriteToFifo(width);
			WriteToFifo(height);
			WaitForFifo();
		}

		/// <summary>
		/// Converts the color to the frame-buffer color
		/// </summary>
		/// <param name="color">The color.</param>
		/// <returns></returns>
		protected uint ConvertColor(Color color)
		{
			return (uint)(
				((color.Alpha << alphaMaskShift) & alphaMask) |
				((color.Red << redMaskShift) & redMask) |
				((color.Green << greenMaskShift) & greenMask) |
				((color.Blue << blueMaskShift) & blueMask)
			);
		}

		/// <summary>
		/// Writes the pixel.
		/// </summary>
		/// <param name="color">The color.</param>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		void IPixelGraphicsDevice.WritePixel(Color color, ushort x, ushort y)
		{
			frameBuffer.SetPixel(ConvertColor(color), x, y);
			UpdateFrame(x, y, 1, 1);
		}

		/// <summary>
		/// Reads the pixel.
		/// </summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <returns></returns>
		Color IPixelGraphicsDevice.ReadPixel(ushort x, ushort y)
		{
			uint color = frameBuffer.GetPixel(x, y);
			return new Color(
				(byte)((color & redMask) >> redMaskShift),
				(byte)((color & greenMask) >> greenMaskShift),
				(byte)((color & blueMask) >> blueMaskShift),
				(byte)((color & alphaMask) >> alphaMaskShift)
			);
		}

		/// <summary>
		/// Clears the specified color.
		/// </summary>
		/// <param name="color">The color.</param>
		void IPixelGraphicsDevice.Clear(Color color)
		{
			//TODO
		}

		/// <summary>
		/// Gets the width.
		/// </summary>
		/// <returns></returns>
		ushort IPixelGraphicsDevice.Width { get { return width; } }

		/// <summary>
		/// Gets the height.
		/// </summary>
		/// <returns></returns>
		ushort IPixelGraphicsDevice.Height { get { return height; } }
	}
}
