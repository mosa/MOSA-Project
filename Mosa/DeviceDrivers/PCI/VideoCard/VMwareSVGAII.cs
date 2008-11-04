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
using Mosa.DeviceSystem.PCI;

/*
 * Portions of this code is:
 * 
 * Copyright 1998-2001, VMware, Inc.
 * Distributed under the terms of the MIT License.
 *
 */

namespace Mosa.DeviceDrivers.PCI.VideoCard
{
	/// <summary>
	/// VMware SVGA II Device Driver
	/// </summary>
	[PCIDeviceSignature(VendorID = 0x15AD, DeviceID = 0x0405, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	public class VMwareSVGAII : PCIHardwareDevice, IHardwareDevice // , IPixelGraphicsDevice
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
			internal const byte MemStart = 18;		/* Memory for command FIFO and bitmaps */
			internal const byte MemSize = 19;
			internal const byte ConfigDone = 20;  	/* Set when memory area configured */
			internal const byte Sync = 21; 			/* Write to force synchronization */
			internal const byte Busy = 22; 			/* Read to check if sync is done */
			internal const byte GuestID = 23; 		/* Set guest OS identifier */
			internal const byte CursorID = 24;		/* ID of cursor */
			internal const byte CursorX = 25; 		/* Set cursor X position */
			internal const byte CursorY = 26; 		/* Set cursor Y position */
			internal const byte CursorOn = 27;		/* Turn cursor on/off */
			internal const byte HostBitsPerPixel = 28; /* Current bpp in the host */
			internal const byte ScratchSize = 29; 	/* Number of scratch registers */
			internal const byte MemRegs = 30; 		/* Number of FIFO registers */
			internal const byte NumDisplays = 31; 	/* Number of guest displays */
			internal const byte PitchLock = 32;		/* Fixed pitch for all modes */
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

		internal struct Command
		{
			internal const uint UPDATE = 1;
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

		#endregion

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
		protected IReadWriteIOPort indexPort;

		/// <summary>
		/// 
		/// </summary>
		protected IReadWriteIOPort valuePort;

		/// <summary>
		/// 
		/// </summary>
		protected IMemory memory;

		/// <summary>
		/// 
		/// </summary>
		protected IMemory fifo;

		/// <summary>
		/// 
		/// </summary>
		protected IFrameBuffer frameBuffer;

		/// <summary>
		/// 
		/// </summary>
		protected SpinLock spinLock;

		/// <summary>
		/// 
		/// </summary>
		protected uint version;

		/// <summary>
		/// 
		/// </summary>
		protected uint offset;

		/// <summary>
		/// 
		/// </summary>
		protected uint videoRamSize;
		/// <summary>
		/// 
		/// </summary>
		protected uint maxWidth;
		/// <summary>
		/// 
		/// </summary>
		protected uint maxHeight;
		/// <summary>
		/// 
		/// </summary>
		protected uint bitsPerPixel;
		/// <summary>
		/// 
		/// </summary>
		protected uint bytesPerLine;
		/// <summary>
		/// 
		/// </summary>
		protected uint redMask;
		/// <summary>
		/// 
		/// </summary>
		protected uint greenMask;
		/// <summary>
		/// 
		/// </summary>
		protected uint blueMask;
		/// <summary>
		/// 
		/// </summary>
		protected uint alphaMask;
		/// <summary>
		/// 
		/// </summary>
		protected byte redMaskShift;
		/// <summary>
		/// 
		/// </summary>
		protected byte greenMaskShift;
		/// <summary>
		/// 
		/// </summary>
		protected byte blueMaskShift;
		/// <summary>
		/// 
		/// </summary>
		protected byte alphaMaskShift;
		/// <summary>
		/// 
		/// </summary>
		protected uint capabilities;

		/// <summary>
		/// Initializes a new instance of the <see cref="VMwareSVGAII"/> class.
		/// </summary>
		/// <param name="pciDevice"></param>
		public VMwareSVGAII(PCIDevice pciDevice) : base(pciDevice) { }

		/// <summary>
		/// Setups this hardware device driver
		/// </summary>
		/// <returns></returns>
		public override bool Setup()
		{
			base.name = "VMWARE_SVGA_0x" + busResources.GetIOPortRegion(0).BaseIOPort.ToString("X");

			indexPort = busResources.GetIOPort(0, 0);
			valuePort = busResources.GetIOPort(0, 1);

			memory = base.busResources.GetMemory(0);
			fifo = base.busResources.GetMemory(1);

			return true;
		}

		/// <summary>
		/// Probes for this device.
		/// </summary>
		/// <returns></returns>
		public override bool Probe() { return true; }

		/// <summary>
		/// Starts this hardware device.
		/// </summary>
		/// <returns></returns>
		public override bool Start()
		{
			capabilities = GetValue(Register.Capabilities);
			videoRamSize = GetValue(Register.VRamSize);
			maxWidth = GetValue(Register.MaxWidth);
			maxHeight = GetValue(Register.MaxHeight);
			bitsPerPixel = GetValue(Register.BitsPerPixel);
			bytesPerLine = GetValue(Register.BytesPerLine);
			redMask = GetValue(Register.RedMask);
			greenMask = GetValue(Register.GreenMask);
			blueMask = GetValue(Register.BlueMask);
			redMaskShift = GetMaskShift(redMask);
			greenMaskShift = GetMaskShift(greenMask);
			blueMaskShift = GetMaskShift(blueMask);
			alphaMaskShift = GetMaskShift(alphaMask);
			offset = GetValue(Register.FrameBufferOffset);

			SetMode(640, 480);

			InitializeFifo();

			return true;
		}

		/// <summary>
		/// Called when an interrupt is received.
		/// </summary>
		/// <returns></returns>
		public override bool OnInterrupt() { return false; }

		/// <summary>
		/// Creates the sub devices.
		/// </summary>
		/// <returns></returns>
		public override LinkedList<IDevice> CreateSubDevices() { return null; }

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
		protected uint GetValue(uint command)
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

			SendCommand(Register.Width, width);
			SendCommand(Register.Height, height);
			SendCommand(Register.Enable, 1);

			offset = GetValue(Register.FrameBufferOffset);

			SendCommand(Register.GuestID, 0x5010); // ??
			bytesPerLine = GetValue(Register.BytesPerLine);

			switch (bitsPerPixel) {
				case 8: frameBuffer = new FrameBuffer8bpp(memory, width, height, offset, bytesPerLine); break;
				case 16: frameBuffer = new FrameBuffer16bpp(memory, width, height, offset, bytesPerLine); break;
				case 24: frameBuffer = new FrameBuffer24bpp(memory, width, height, offset, bytesPerLine); break;
				case 32: frameBuffer = new FrameBuffer32bpp(memory, width, height, offset, bytesPerLine); break;
				default: return false;
			}

			return true;
		}

		/// <summary>
		/// Initializes the fifo.
		/// </summary>
		/// <returns></returns>
		protected bool InitializeFifo()
		{
			uint start = GetValue(Register.MemStart);
			uint size = GetValue(Register.MemSize);

			fifo.Write32(Fifo.Min * 4, 16);
			fifo.Write32(Fifo.Max * 4, size);
			fifo.Write32(Fifo.NextCmd * 4, 16);
			fifo.Write32(Fifo.Stop * 4, 16);

			SendCommand(Register.ConfigDone, 1);

			return true;
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

			while (GetValue(Register.Busy) != 0) {
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
				WaitForFifo();

			SetFifo(GetFifo(Fifo.NextCmd) / 4, value);
			SetFifo(Fifo.NextCmd, GetFifo(Fifo.NextCmd) + 4);

			if (GetFifo(Fifo.NextCmd) == GetFifo(Fifo.Max))
				SetFifo(Fifo.NextCmd, GetFifo(Fifo.Min));
		}

		/// <summary>
		/// Gets the version.
		/// </summary>
		/// <returns></returns>
		protected uint GetVersion()
		{
			SendCommand(Register.ID, ID.V2);
			if (GetValue(Register.ID) == ID.V2)
				return ID.V2;

			SendCommand(Register.ID, ID.V1);
			if (GetValue(Register.ID) == ID.V1)
				return ID.V1;

			if (GetValue(Register.ID) == ID.V0)
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

			while ((mask & 1) == 0) {
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
			WriteToFifo(Command.UPDATE);
			WriteToFifo(x);
			WriteToFifo(y);
			WriteToFifo(width);
			WriteToFifo(height);
		}

		/// <summary>
		/// Converts the color to the framebuffer color
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
		public void WritePixel(Color color, ushort x, ushort y)
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
		public Color ReadPixel(ushort x, ushort y)
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
		public void Clear(Color color)
		{
			//TODO
		}

		/// <summary>
		/// Gets the width.
		/// </summary>
		/// <returns></returns>
		public ushort Width { get { return width; } }

		/// <summary>
		/// Gets the height.
		/// </summary>
		/// <returns></returns>
		public ushort Height { get { return height; } }
	}
}
