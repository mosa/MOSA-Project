// Copyright (c) MOSA Project. Licensed under the New BSD License.

/*
 * Portions of this code is:
 *
 * Copyright 1998-2001, VMware, Inc.
 * Distributed under the terms of the MIT License.
 *
 */

// References:
// https://gitlab.freedesktop.org/xorg/driver/xf86-video-vmware/-/blob/master/src/svga_reg.h
// https://gitlab.freedesktop.org/xorg/driver/xf86-video-vmware/-/blob/master/src/guest_os.h
// https://gitlab.freedesktop.org/xorg/driver/xf86-video-vmware/-/blob/master/src/svga_limits.h
// https://gitlab.freedesktop.org/xorg/driver/xf86-video-vmware/-/blob/master/src/svga_modes.h
// https://sourceforge.net/p/vmware-svga/git/ci/master/tree/lib/vmware/svga_reg.h
// https://sourceforge.net/p/vmware-svga/git/ci/master/tree/lib/refdriver/svga.c

using System.Runtime.InteropServices;
using Mosa.DeviceSystem;

namespace Mosa.DeviceDriver.PCI.VMware
{
	/// <summary>
	/// VMware SVGA II Device Driver
	/// </summary>
	//[PCIDeviceDriver(VendorID = 0x15AD, DeviceID = 0x0405, Platforms = PlatformArchitecture.X86AndX64)]
	public class VMwareSVGA2 : BaseDeviceDriver, IPixelGraphicsDevice
	{
		#region Definitions

		// Taken from:
		// PCI Device IDs
		internal struct PCI
		{
			internal const ushort VENDOR_ID_VMWARE = 0x15AD;
			internal const ushort DEVICE_ID_VMWARE_SVGA2 = 0x0405;
		}

		/* Port offsets, relative to BAR0 */
		internal struct PORT
		{
			internal const byte INDEX = 0x00;
			internal const byte VALUE = 0x01;
			internal const byte BIOS = 0x02;
			internal const byte IRQ_STATUS = 0x08;
		}

		internal struct SVGA_VERSION_ID
		{
			internal const uint Magic = 0x900000;
			internal const uint V0 = (Magic << 8) | 0;
			internal const uint V1 = (Magic << 8) | 1;
			internal const uint V2 = (Magic << 8) | 2;
			internal const uint Invalid = 0xFFFFFFFF;
		}

		/*
		 * Interrupt source flags for IRQSTATUS_PORT and IRQMASK.
		 *
		 * Interrupts are only supported when the
		 * SVGA_CAP_IRQMASK capability is present.
         */
		internal struct IRQFLAG
		{
			internal const byte ANY_FENCE = 0x01;       /* Any fence was passed */
			internal const byte FIFO_PROGRESS = 0x02;   /* Made forward progress in the FIFO */
			internal const byte FENCE_GOAL = 0x04;      /* SVGA_FIFO_FENCE_GOAL reached */
		}

		internal struct REG_ENABLE
		{
			internal const byte DISABLE = 0x00;
			internal const byte ENABLE = 0x01;
			internal const byte HIDE = 0x02;
			internal const byte ENABLE_HIDE = ENABLE | HIDE;
		}

		/*
		 * Legal values for the SVGA_REG_CURSOR_ON register in old-fashioned
		 * cursor bypass mode. This is still supported, but no new guest
		 * drivers should use it.
		 */
		internal struct CURSOR_ON
		{
			internal const byte HIDE = 0x00;
			internal const byte SHOW = 0x01;
			internal const byte REMOVE_FROM_FB = 0x02;
			internal const byte RESTORE_TO_FB = 0x03;
		}

		internal struct GUEST_OS
		{
			internal const ushort BASE = 0x5000;
			internal const ushort DOS = BASE + 1;
			internal const ushort WINDOWS31 = BASE + 2;
			internal const ushort WINDOWS95 = BASE + 3;
			internal const ushort WINDOWS98 = BASE + 4;
			internal const ushort WINDOWSME = BASE + 5;
			internal const ushort WINDOWSNT = BASE + 6;
			internal const ushort WINDOWS2000 = BASE + 7;
			internal const ushort LINUX = BASE + 8;
			internal const ushort OS2 = BASE + 9;
			internal const ushort OTHER = BASE + 10;
			internal const ushort FREEBSD = BASE + 11;
			internal const ushort WHISTLER = BASE + 12;
		}

		internal struct SVGA_CONSTANTS
		{
			internal const uint SVGA_VRAM_SIZE = 16 * 1024 * 1024;
			internal const uint SVGA_MEM_SIZE = 256 * 1024;

			internal const uint SVGA_FB_MAX_TRACEABLE_SIZE = 0x1000000;
			internal const byte SVGA_MAX_PSEUDOCOLOR_DEPTH = 0x08;
			internal const ushort SVGA_MAX_PSEUDOCOLORS = 1 << SVGA_MAX_PSEUDOCOLOR_DEPTH;
			internal const ushort SVGA_NUM_PALETTE_REGS = 3 * SVGA_MAX_PSEUDOCOLORS;

			/* Base and Offset gets us headed the right way for PCI Base Addr Registers */
			internal const ushort SVGA_LEGACY_BASE_PORT = 0x4560;
			internal const byte SVGA_NUM_PORTS = 0x3;

			internal const ushort SVGA_PALETTE_BASE = 1024;     /* Base of SVGA color map */
			/* Next 768 (== 256*3) registers exist for colormap */
			internal const ushort SVGA_SCRATCH_BASE = SVGA_PALETTE_BASE + SVGA_NUM_PALETTE_REGS; /* Base of scratch registers */
			/* Next reg[SVGA_REG_SCRATCH_SIZE] registers exist for scratch usage:
		       First 4 are reserved for VESA BIOS Extension; any remaining are for
			   the use of the current SVGA driver. */

			internal const byte SVGA_NUM_OVERLAY_UNITS = 32;
			internal const byte SVGA_VIDEO_FLAG_COLORKEY = 0x01;
			internal const uint SVGA_CMD_MAX_DATASIZE = 256 * 1024;
			internal const byte SVGA_CMD_MAX_ARGS = 64;

			// FrameBufferStart
			internal const uint SVGA_FB_LEGACY_START = 0x7EFC0000;
			internal const uint SVGA_FB_LEGACY_BIGMEM = 0xE0000000;

			internal const byte SVGA_SCREEN_MUST_BE_SET = 1 << 0;
			internal const byte SVGA_SCREEN_HAS_ROOT = 1 << 0;
			internal const byte SVGA_SCREEN_IS_PRIMARY = 1 << 1;
			internal const byte SVGA_SCREEN_FULLSCREEN_HINT = 1 << 2;
			internal const byte SVGA_SCREEN_DEACTIVATE = 1 << 3;
			internal const byte SVGA_SCREEN_BLANKING = 1 << 4;
		}

		// SVGA_DEFAULT_MODES
		// 4:3 modes
		// "320x240", "400x300", "512x384", "640x480", "800x600", "1024x768", "1152x864", "1280x960", "1400x1050", "1600x1200", "1920x1440", "2048x1536", "2560x1920",
		// 16:9 modes
		// "854x480", "1280x720", "1366x768", "1920x1080", "2560x1440",
		// 16:10 modes
		// "1280x800", "1440x900", "1680x1050", "1920x1200", "2560x1600",
		// DVD modes
		// "720x480", "720x576",
		// Odd modes
		// "320x200", "640x400", "800x480", "1280x768", "1280x1024"

		internal struct SVGA_CAPABILITIES
		{
			internal const uint NONE = 0x00000000;
			internal const uint RECT_COPY = 0x00000002;
			internal const uint CURSOR = 0x00000020;
			internal const uint CURSOR_BYPASS = 0x00000040;     // Legacy (Use Cursor Bypass 3 instead)
			internal const uint CURSOR_BYPASS_2 = 0x00000080;   // Legacy (Use Cursor Bypass 3 instead)
			internal const uint EMULATION_8BIT = 0x00000100;
			internal const uint ALPHA_CURSOR = 0x00000200;
			internal const uint HW3D = 0x00004000;
			internal const uint EXTENDED_FIFO = 0x00008000;
			internal const uint MULTIMON = 0x00010000;          // Legacy multi-monitor support
			internal const uint PITCHLOCK = 0x00020000;
			internal const uint IRQMASK = 0x00040000;
			internal const uint DISPLAY_TOPOLOGY = 0x00080000;  // Legacy multi-monitor support
			internal const uint GMR = 0x00100000;
			internal const uint TRACES = 0x00200000;
			internal const uint GMR2 = 0x00400000;
			internal const uint SCREEN_OBJECT_2 = 0x00800000;
		}

		internal struct SVGA_REGISTERS
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
			internal const byte MemStart = 18;			/* Memory for command FIFO and bitmaps */
			internal const byte MemSize = 19;
			internal const byte ConfigDone = 20;		/* Set when memory area configured */
			internal const byte Sync = 21;				/* Write to force synchronization */
			internal const byte Busy = 22;				/* Read to check if sync is done */
			internal const byte GuestID = 23;			/* Set guest OS identifier */
			internal const byte CursorID = 24;			/* ID of cursor */
			internal const byte CursorX = 25;			/* Set cursor X position */
			internal const byte CursorY = 26;			/* Set cursor Y position */
			internal const byte CursorOn = 27;			/* Turn cursor on/off */
			internal const byte HostBitsPerPixel = 28;	/* Current bpp in the host */
			internal const byte ScratchSize = 29;		/* Number of scratch registers */
			internal const byte MemRegs = 30;			/* Number of FIFO registers */
			internal const byte NumDisplays = 31;		/* Number of guest displays */
			internal const byte PitchLock = 32;			/* Fixed pitch for all modes */
			internal const byte IRQMask = 33;			/* Interrupt mask */

			/* Legacy multi-monitor support */
			internal const byte NUM_GUEST_DISPLAYS = 34;	/* Number of guest displays in X/Y direction */
			internal const byte DISPLAY_ID = 35;            /* Display ID for the following display attributes */
			internal const byte DISPLAY_IS_PRIMARY = 36;    /* Whether this is a primary display */
			internal const byte DISPLAY_POSITION_X = 37;    /* The display position x */
			internal const byte DISPLAY_POSITION_Y = 38;    /* The display position y */
			internal const byte DISPLAY_WIDTH = 39;         /* The display's width */
			internal const byte DISPLAY_HEIGHT = 40;        /* The display's height */

			/* See "Guest memory regions" below. */
			internal const byte GMR_ID = 41;
			internal const byte GMR_DESCRIPTOR = 42;
			internal const byte GMR_MAX_IDS = 43;
			internal const byte GMR_MAX_DESCRIPTOR_LENGTH = 44;

			internal const byte TRACES = 45;                /* Enable trace-based updates even when FIFO is on */
			internal const byte GMRS_MAX_PAGES = 46;        /* Maximum number of 4KB pages for all GMRs */
			internal const byte MEMORY_SIZE = 47;           /* Total dedicated device memory excluding FIFO */
			internal const byte TOP = 48;                   /* Must be 1 more than the last register */
		}

		internal struct FIFO_REGISTERS
		{
			internal const ushort Min = 0x00;
			internal const ushort Max = 0x01;
			internal const ushort NextCmd = 0x02;
			internal const ushort Stop = 0x03;
			internal const ushort Capabilities = 0x04;
			internal const ushort Flags = 0x05;
			internal const ushort Fence = 0x06;
			internal const ushort HW3DVersion = 0x07;
			internal const ushort PitchLock = 0x08;
			internal const ushort CursorOn = 0x09;
			internal const ushort CursorX = 0x0A;
			internal const ushort CursorY = 0x0B;
			internal const ushort CursorCount = 0x0C;
			internal const ushort CursorLastUpdated = 0x0D;
			internal const ushort Reserved = 0x0E;
			internal const ushort CursorScreenID = 0x0F;
			internal const ushort Dead = 0x10;
			internal const ushort HW3DVersionRevised = 0x11;
			internal const ushort Capabilities3D = 0x20;
			internal const ushort Capabilities3DLast = 0x20 + 0xFF;
			internal const ushort GuestHW3DVersion = 0x120;
			internal const ushort FenceGoal = 0x121;
			internal const ushort Busy = 0x122;
			internal const ushort NumRegs = 0x123;
		}

		internal struct FIFO_CAPABILITIES
		{
			internal const ushort None = 0;
			internal const ushort Fence = 1 << 0;
			internal const ushort AccelFront = 1 << 1;
			internal const ushort PitchLock = 1 << 2;
			internal const ushort Video = 1 << 3;
			internal const ushort CursorByPass3 = 1 << 4;
			internal const ushort Escape = 1 << 5;
			internal const ushort Reserve = 1 << 6;
			internal const ushort ScreenObject = 1 << 7;
			internal const ushort GMR2 = 1 << 8;
			internal const ushort HW3DVersionRevised = GMR2;
			internal const ushort ScreenObject2 = 1 << 9;
			internal const ushort Dead = 1 << 10;
		}

		internal struct FIFO_FLAGS
		{
			internal const uint None = 0;
			internal const uint AccelFront = 1 << 0;
			internal const uint Reserved = (uint)1 << 31;
		}

		internal struct FIFO_COMMANDS
		{
			internal const byte INVALID_CMD = 0;
			internal const byte UPDATE = 1;
			internal const byte RECT_FILL = 2;
			internal const byte RECT_COPY = 3;
			internal const byte DEFINE_BITMAP = 4;
			internal const byte DEFINE_BITMAP_SCANLINE = 5;
			internal const byte DEFINE_PIXMAP = 6;
			internal const byte DEFINE_PIXMAP_SCANLINE = 7;
			internal const byte RECT_BITMAP_FILL = 8;
			internal const byte RECT_PIXMAP_FILL = 9;
			internal const byte RECT_BITMAP_COPY = 10;
			internal const byte RECT_PIXMAP_COPY = 11;
			internal const byte FREE_OBJECT = 12;
			internal const byte RECT_ROP_FILL = 13;
			internal const byte RECT_ROP_COPY = 14;
			internal const byte RECT_ROP_BITMAP_FILL = 15;
			internal const byte RECT_ROP_PIXMAP_FILL = 16;
			internal const byte RECT_ROP_BITMAP_COPY = 17;
			internal const byte RECT_ROP_PIXMAP_COPY = 18;
			internal const byte DEFINE_CURSOR = 19;
			internal const byte DISPLAY_CURSOR = 20;
			internal const byte MOVE_CURSOR = 21;
			internal const byte DEFINE_ALPHA_CURSOR = 22;
			internal const byte UPDATE_VERBOSE = 25;
			internal const byte FRONT_ROP_FILL = 29;
			internal const byte FENCE = 30;
			internal const byte ESCAPE = 33;
			internal const byte DEFINE_SCREEN = 34;
			internal const byte DESTROY_SCREEN = 35;
			internal const byte DEFINE_GMRFB = 36;
			internal const byte BLIT_GMRFB_TO_SCREEN = 37;
			internal const byte BLIT_SCREEN_TO_GNMRFB = 38;
			internal const byte ANNOTATION_FILL = 39;
			internal const byte ANNOTATION_COPY = 40;
			internal const byte DEFINE_GMR2 = 41;
			internal const byte REMAP_GMR2 = 42;
			internal const byte MAX = 43;
		}

		/*
		 * Offsets for the video overlay registers
		*/
		internal struct VIDEO_OVERLAY_REGISTERS
		{
			internal const byte Enabled = 0;
			internal const byte Flags = 1;
			internal const byte DataOffset = 2;
			internal const byte Format = 3;
			internal const byte ColorKey = 4;
			internal const byte Size = 5;
			internal const byte Width = 6;
			internal const byte Height = 7;
			internal const byte SourceX = 8;
			internal const byte SourceY = 9;
			internal const byte SourceWidth = 10;
			internal const byte SourceHeight = 11;
			internal const byte DestinationX = 12;
			internal const byte DestinationY = 13;
			internal const byte DestinationWidth = 14;
			internal const byte DestinationHeight = 15;
			internal const byte Pitch1 = 16;
			internal const byte Pitch2 = 17;
			internal const byte Pitch3 = 18;
			internal const byte DataGMRID = 19;
			internal const byte DestinationScreenID = 20;
			internal const byte NumRegs = 21;
		}

		#endregion Definitions

		#region Ports

		protected BaseIOPortReadWrite indexPort;

		protected BaseIOPortReadWrite valuePort;

		#endregion Ports

		protected ConstrainedPointer memory;

		protected ConstrainedPointer fifo;

		protected IFrameBuffer frameBuffer;

		/// <summary>The width</summary>
		protected ushort width;

		/// <summary>The height</summary>
		protected ushort height;

		/// <summary>The version</summary>
		protected uint version;

		/// <summary>The offset</summary>
		protected uint offset;

		/// <summary>The video ram size</summary>
		protected uint videoRamSize;

		/// <summary>The maximum width</summary>
		protected uint maxWidth;

		/// <summary>The maximum height</summary>
		protected uint maxHeight;

		/// <summary>The bits per pixel</summary>
		protected uint bitsPerPixel;

		/// <summary>The bytes per line</summary>
		protected uint bytesPerLine;

		/// <summary>The red mask</summary>
		protected uint redMask;

		/// <summary>The green mask</summary>
		protected uint greenMask;

		/// <summary>The blue mask</summary>
		protected uint blueMask;

		/// <summary>The alpha mask</summary>
		protected uint alphaMask;

		/// <summary>The red mask shift</summary>
		protected byte redMaskShift;

		/// <summary>The green mask shift</summary>
		protected byte greenMaskShift;

		/// <summary>The blue mask shift</summary>
		protected byte blueMaskShift;

		/// <summary>The alpha mask shift</summary>
		protected byte alphaMaskShift;

		/// <summary>The svga capabilities</summary>
		protected uint svgaCapabilities;

		/// <summary>The frame buffer size</summary>
		protected uint frameBufferSize;

		/// <summary>The fifo size</summary>
		protected uint fifoSize;

		/// <summary>The fifo number regs</summary>
		protected const uint FifoNumRegs = FIFO_REGISTERS.NumRegs;

		public override void Initialize()
		{
			Device.Name = "VMWARE_SVGA2_0x" + Device.Resources.GetIOPortRegion(0).BaseIOPort.ToString("X");

			indexPort = Device.Resources.GetIOPortReadWrite(0, 0);
			valuePort = Device.Resources.GetIOPortReadWrite(0, 1);
			memory = Device.Resources.GetMemory(0);
			fifo = Device.Resources.GetMemory(1);
		}

		public override void Probe()
		{
			Device.Status = DeviceStatus.Available;
		}

		public override void Start()
		{
			const ushort ScreenWidth = 1366;
			const ushort ScreenHeight = 768;

			if (Device.Status != DeviceStatus.Available) { return; }

			version = GetVersion();

			if (version == SVGA_VERSION_ID.V1 || version == SVGA_VERSION_ID.V2)
			{
				SVGASetRegister(SVGA_REGISTERS.GuestID, GUEST_OS.OTHER); // 0x05010 == GUEST_OS_OTHER (vs GUEST_OS_WIN2000)

				svgaCapabilities = SVGAGetRegister(SVGA_REGISTERS.Capabilities);
			}

			FIFOInitialize();

			SVGASetMode(ScreenWidth, ScreenHeight);

			FIFOCmdDefineScreen(0, 0, ScreenWidth, ScreenHeight, SVGA_CONSTANTS.SVGA_SCREEN_HAS_ROOT | SVGA_CONSTANTS.SVGA_SCREEN_IS_PRIMARY);
			UpdateScreen();

			videoRamSize = SVGAGetRegister(SVGA_REGISTERS.VRamSize);
			frameBufferSize = SVGAGetRegister(SVGA_REGISTERS.FrameBufferSize);
			fifoSize = SVGAGetRegister(SVGA_REGISTERS.MemSize);

			maxWidth = SVGAGetRegister(SVGA_REGISTERS.MaxWidth);
			maxHeight = SVGAGetRegister(SVGA_REGISTERS.MaxHeight);
			bitsPerPixel = SVGAGetRegister(SVGA_REGISTERS.BitsPerPixel);
			bytesPerLine = SVGAGetRegister(SVGA_REGISTERS.BytesPerLine);
			redMask = SVGAGetRegister(SVGA_REGISTERS.RedMask);
			greenMask = SVGAGetRegister(SVGA_REGISTERS.GreenMask);
			blueMask = SVGAGetRegister(SVGA_REGISTERS.BlueMask);
			redMaskShift = GetMaskShift(redMask);
			greenMaskShift = GetMaskShift(greenMask);
			blueMaskShift = GetMaskShift(blueMask);
			alphaMaskShift = GetMaskShift(alphaMask);
			offset = SVGAGetRegister(SVGA_REGISTERS.FrameBufferOffset);

			Device.Status = DeviceStatus.Online;

			FIFOCmdDefineGMRFB(ScreenWidth * 4, 32, 32);

			Clear(new Color(0xff, 0x00, 0x00));

			FIFOMoveCursor(1, ScreenWidth / 2, ScreenHeight / 2);

			FIFOCmdBlitGMRFBToScreen(0, 0, 0, 0, ScreenWidth, ScreenHeight, 0);

			UpdateScreen();
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
		protected void SVGASetRegister(uint command, uint value)
		{
			indexPort.Write32(command);
			valuePort.Write32(value);
		}

		/// <summary>
		/// Gets the value.
		/// </summary>
		/// <param name="command">The command.</param>
		/// <returns></returns>
		protected uint SVGAGetRegister(uint command)
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
		public bool SVGASetMode(ushort width, ushort height)
		{
			this.width = width;
			this.height = height;
			// use the host's bits per pixel
			this.bitsPerPixel = SVGAGetRegister(SVGA_REGISTERS.HostBitsPerPixel);

			// set height  & width
			SVGASetRegister(SVGA_REGISTERS.Height, height);
			SVGASetRegister(SVGA_REGISTERS.Width, width);
			SVGASetRegister(SVGA_REGISTERS.BitsPerPixel, bitsPerPixel);
			SVGASetRegister(SVGA_REGISTERS.Enable, 1);

			// get the frame buffer offset
			offset = SVGAGetRegister(SVGA_REGISTERS.FrameBufferOffset);

			// get the bytes per line (pitch)
			bytesPerLine = SVGAGetRegister(SVGA_REGISTERS.BytesPerLine);

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
		protected void FIFOInitialize()
		{
			uint start = SVGAGetRegister(SVGA_REGISTERS.MemStart);
			uint fifoSize = SVGAGetRegister(SVGA_REGISTERS.MemSize);

			FIFOSetRegister(FIFO_REGISTERS.Min, FifoNumRegs * 4);
			FIFOSetRegister(FIFO_REGISTERS.Max, fifoSize);
			FIFOSetRegister(FIFO_REGISTERS.NextCmd, FifoNumRegs * 4);
			FIFOSetRegister(FIFO_REGISTERS.Stop, FifoNumRegs * 4);

			SVGASetRegister(SVGA_REGISTERS.Enable, 1);
			SVGASetRegister(SVGA_REGISTERS.ConfigDone, 1);
		}

		protected void SVGAShutdown()
		{
			SVGASetRegister(SVGA_REGISTERS.Enable, 0);
		}

		/// <summary>
		/// Gets the fifo.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		protected uint FIFOGetRegister(uint index)
		{
			return fifo.Read32(index * 4);
		}

		/// <summary>
		/// Sets the fifo.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		protected void FIFOSetRegister(uint index, uint value)
		{
			fifo.Write32(index * 4, value);
		}

		/// <summary>
		/// Waits for fifo.
		/// </summary>
		protected void WaitForFifo()
		{
			SVGASetRegister(SVGA_REGISTERS.Sync, 1);

			while (SVGAGetRegister(SVGA_REGISTERS.Busy) != 0)
			{
				HAL.Sleep(5);
			}
		}

		/// <summary>
		/// Writes to fifo.
		/// </summary>
		/// <param name="value">The value.</param>
		protected void WriteToFifo(uint value)
		{
			if (((FIFOGetRegister(FIFO_REGISTERS.NextCmd) == FIFOGetRegister(FIFO_REGISTERS.Max) - 4) && FIFOGetRegister(FIFO_REGISTERS.Stop) == FIFOGetRegister(FIFO_REGISTERS.Min)) ||
				(FIFOGetRegister(FIFO_REGISTERS.NextCmd) + 4 == FIFOGetRegister(FIFO_REGISTERS.Stop)))
			{
				WaitForFifo();
			}

			FIFOSetRegister(FIFOGetRegister(FIFO_REGISTERS.NextCmd) / 4, value);
			FIFOSetRegister(FIFO_REGISTERS.NextCmd, FIFOGetRegister(FIFO_REGISTERS.NextCmd) + 4);

			if (FIFOGetRegister(FIFO_REGISTERS.NextCmd) == FIFOGetRegister(FIFO_REGISTERS.Max))
			{
				FIFOSetRegister(FIFO_REGISTERS.NextCmd, FIFOGetRegister(FIFO_REGISTERS.Min));
			}
		}

		/// <summary>
		/// Gets the version.
		/// </summary>
		/// <returns></returns>
		protected uint GetVersion()
		{
			SVGASetRegister(SVGA_REGISTERS.ID, SVGA_VERSION_ID.V2);
			if (SVGAGetRegister(SVGA_REGISTERS.ID) == SVGA_VERSION_ID.V2) { return SVGA_VERSION_ID.V2; }

			SVGASetRegister(SVGA_REGISTERS.ID, SVGA_VERSION_ID.V1);
			if (SVGAGetRegister(SVGA_REGISTERS.ID) == SVGA_VERSION_ID.V1) { return SVGA_VERSION_ID.V1; }

			SVGASetRegister(SVGA_REGISTERS.ID, SVGA_VERSION_ID.V0);
			if (SVGAGetRegister(SVGA_REGISTERS.ID) == SVGA_VERSION_ID.V0) { return SVGA_VERSION_ID.V0; }

			return SVGA_VERSION_ID.Invalid;
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
		protected void UpdateScreen()
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
			WriteToFifo(FIFO_COMMANDS.UPDATE);

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
			uint ClearColor = ConvertColor(color);

			for (uint y = 0; y < height; y++)
			{
				for (uint x = 0; x < width; x++)
				{
					frameBuffer.SetPixel(ClearColor, x, y);
				}
			}

			UpdateScreen();
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

		protected void FIFOCmdUpdate (uint X, uint Y, uint Width, uint Height)
		{
			WriteToFifo(FIFO_COMMANDS.UPDATE);

			WriteToFifo(X);
			WriteToFifo(Y);
			WriteToFifo(Width);
			WriteToFifo(Height);

			WaitForFifo();
		}

		protected void FIFOCmdRectCopy (uint SrcX, uint SrcY, uint DstX, uint DstY, uint Width, uint Height)
		{
			WriteToFifo(FIFO_COMMANDS.RECT_COPY);

			WriteToFifo(SrcX);
			WriteToFifo(SrcY);
			WriteToFifo(DstX);
			WriteToFifo(DstY);
			WriteToFifo(Width);
			WriteToFifo(Height);

			WaitForFifo();
		}

		protected void FIFOCmdDefineScreen (int x, int y, uint width, uint height, uint flags)
		{
			if (FIFOHasCapability(FIFO_CAPABILITIES.ScreenObject) || FIFOHasCapability(FIFO_CAPABILITIES.ScreenObject2))
			{
				WriteToFifo(FIFO_COMMANDS.DEFINE_SCREEN);

				WriteToFifo(28);        // Sizeof (SVGAScreenObject)
				WriteToFifo(0);         // New ScreenId
				WriteToFifo(flags);
				WriteToFifo(width);
				WriteToFifo(height);
				WriteToFifo((uint)x);   // root x
				WriteToFifo((uint)y);   // root y

				WaitForFifo();
			}
		}

		protected void FIFOCmdDestroyScreen (uint screenId)
		{
			WriteToFifo(FIFO_COMMANDS.DESTROY_SCREEN);

			WriteToFifo(screenId);

			WaitForFifo();
		}

		protected void FIFOCmdDefineGMRFB (uint bytesPerLine, byte bitsPerPixel, byte colorDepth)
		{
			WriteToFifo(FIFO_COMMANDS.DEFINE_GMRFB);

			WriteToFifo(0);     // gmrId
			WriteToFifo(0);     // offset
			WriteToFifo(bytesPerLine);
			WriteToFifo(bitsPerPixel);
			WriteToFifo(colorDepth);

			WaitForFifo();
		}

		protected void FIFOCmdBlitGMRFBToScreen (int SrcX, int SrcY, int DestLeft, int DestTop, int DestRight, int DestBottom, uint DestScreen)
		{
			WriteToFifo(FIFO_COMMANDS.BLIT_GMRFB_TO_SCREEN);

			WriteToFifo((uint)SrcX);
			WriteToFifo((uint)SrcY);
			WriteToFifo((uint)DestLeft);
			WriteToFifo((uint)DestTop);
			WriteToFifo((uint)DestRight);
			WriteToFifo((uint)DestBottom);
			WriteToFifo(DestScreen);

			WaitForFifo();
		}

		protected bool FIFOHasCapability (uint Capability)
		{
			return (FIFOGetRegister(FIFO_REGISTERS.Capabilities) & Capability) != 0;
		}

		protected void FIFOMoveCursor (uint Visible, uint X, uint Y)
		{
			if (FIFOHasCapability(FIFO_CAPABILITIES.ScreenObject))
			{
				FIFOSetRegister(FIFO_REGISTERS.CursorScreenID, 0);
			}

			if (FIFOHasCapability(FIFO_CAPABILITIES.CursorByPass3))
			{
				FIFOSetRegister(FIFO_REGISTERS.CursorOn, Visible);
				FIFOSetRegister(FIFO_REGISTERS.CursorX, X);
				FIFOSetRegister(FIFO_REGISTERS.CursorY, Y);
				FIFOSetRegister(FIFO_REGISTERS.CursorCount, FIFOGetRegister(FIFO_REGISTERS.CursorCount) + 1);
			}
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct SVGAScreenObject
		{
			internal uint structSize;   // sizeof(SVGAScreenObject)
			internal uint id;
			internal uint flags;
			internal uint width;
			internal uint height;
			internal int x;
			internal int y;
			/*
			 * Added and required by SVGA_FIFO_CAP_SCREEN_OBJECT_2, optional
			 * with SVGA_FIFO_CAP_SCREEN_OBJECT.

			SVGAGuestImage backingStore;
			uint32 cloneCount;
			*/
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct SVGAGuestPtr
		{
			uint gmrId;
			uint offset;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct SVGAGMRImageFormat
		{
			byte bitsPerPixel;
			byte colorDepth;
			ushort reserved;	// Must be zero
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct SVGAGuestImage
		{
			SVGAGuestPtr ptr;
			uint pitch;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct SVGAColorBGRX
		{
			byte b;
			byte g;
			byte r;
			byte x;		// Unused
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct SVGASignedRect
		{
			int left;
			int top;
			int right;
			int bottom;
		}

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		internal struct SVGASignedPoint
		{
			int x;
			int y;
		}
	}
}
