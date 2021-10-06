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
// https://github.com/prepare/vmware-svga

using System;
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

		// PCI Device IDs
		internal struct PCI
		{
			internal const ushort VendorIDVmware = 0x15AD;
			internal const ushort DeviceIDVmwareSVGA2 = 0x0405;
		}

		/* Port offsets, relative to BAR0 */
		internal struct PORT
		{
			internal const byte Index = 0x00;
			internal const byte Value = 0x01;
			internal const byte Bios = 0x02;
			internal const byte IRQStatus = 0x08;
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
		internal struct IRQ_FLAGS
		{
			internal const byte AnyFence = 0x01;       /* Any fence was passed */
			internal const byte FIFOProgress = 0x02;   /* Made forward progress in the FIFO */
			internal const byte FenceGoal = 0x04;      /* SVGA_FIFO_FENCE_GOAL reached */
		}

		internal struct REG_ENABLE
		{
			internal const byte Disable = 0x00;
			internal const byte Enable = 0x01;
			internal const byte Hide = 0x02;
			internal const byte EnableHide = Enable | Hide;
		}

		/*
		 * Legal values for the SVGA_REG_CURSOR_ON register in old-fashioned
		 * cursor bypass mode. This is still supported, but no new guest
		 * drivers should use it.
		 */
		internal struct CURSOR_ON
		{
			internal const byte Hide = 0x00;
			internal const byte Show = 0x01;
			internal const byte RemoveFromFB = 0x02;
			internal const byte RestoreToFB = 0x03;
		}

		internal struct GUEST_OS
		{
			internal const ushort Base = 0x5000;
			internal const ushort Dos = Base + 1;
			internal const ushort Windows31 = Base + 2;
			internal const ushort Windows95 = Base + 3;
			internal const ushort Windows98 = Base + 4;
			internal const ushort WindowsME = Base + 5;
			internal const ushort WindowsNT = Base + 6;
			internal const ushort Windows2000 = Base + 7;
			internal const ushort Linux = Base + 8;
			internal const ushort Os2 = Base + 9;
			internal const ushort Other = Base + 10;
			internal const ushort FreeBSD = Base + 11;
			internal const ushort Whistler = Base + 12;
		}

		internal struct SVGA_CONSTANTS
		{
			internal const uint VRAMSize = 16 * 1024 * 1024;
			internal const uint MemSize = 256 * 1024;

			internal const uint FBMaxTraceableSize = 0x1000000;
			internal const byte MaxPseudoColorDepth = 0x08;
			internal const ushort MaxPseudoColors = 1 << MaxPseudoColorDepth;
			internal const ushort NumPaletteRegs = 3 * MaxPseudoColors;

			/* Base and Offset gets us headed the right way for PCI Base Addr Registers */
			internal const ushort LegacyBasePort = 0x4560;
			internal const byte NumPorts = 0x3;

			internal const ushort PaletteBase = 1024;     /* Base of SVGA color map */
			/* Next 768 (== 256*3) registers exist for colormap */
			internal const ushort ScratchBase = PaletteBase + NumPaletteRegs; /* Base of scratch registers */
			/* Next reg[SVGA_REG_SCRATCH_SIZE] registers exist for scratch usage:
		       First 4 are reserved for VESA BIOS Extension; any remaining are for
			   the use of the current SVGA driver. */

			internal const byte NumOVerlayUnits = 32;

			internal const byte VideoFlagColorKey = 0x01;
			internal const uint CmdMaxDataSize = 256 * 1024;
			internal const byte CmdMaxArgs = 64;

			// FrameBufferStart
			internal const uint FBLegacyStart = 0x7EFC0000;
			internal const uint FBLegacyBigMem = 0xE0000000;

			internal const byte ScreenMustBeSet = 1 << 0;
			internal const byte ScreenHasRoot = 1 << 0;
			internal const byte ScreenIsPrimary = 1 << 1;
			internal const byte ScreenFullScreenHint = 1 << 2;
			internal const byte ScreenDeactivate = 1 << 3;
			internal const byte ScreenBlanking = 1 << 4;
		}

		internal struct SVGA_CAPABILITIES
		{
			internal const uint None = 0x00000000;
			internal const uint RectCopy = 0x00000002;
			internal const uint Cursor = 0x00000020;
			internal const uint CursorByPass = 0x00000040;     // Legacy (Use Cursor Bypass 3 instead)
			internal const uint CursorByPass2 = 0x00000080;   // Legacy (Use Cursor Bypass 3 instead)
			internal const uint Emulation8Bit = 0x00000100;
			internal const uint AlphaCursor = 0x00000200;
			internal const uint HW3D = 0x00004000;
			internal const uint ExtendedFifo = 0x00008000;
			internal const uint MultiMon = 0x00010000;          // Legacy multi-monitor support
			internal const uint PitchLock = 0x00020000;
			internal const uint IRQMask = 0x00040000;
			internal const uint DisplayTopology = 0x00080000;  // Legacy multi-monitor support
			internal const uint GMR = 0x00100000;
			internal const uint Traces = 0x00200000;
			internal const uint GMR2 = 0x00400000;
			internal const uint ScreenObject2 = 0x00800000;
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
			internal const byte MemStart = 18;          /* Memory for command FIFO and bitmaps */
			internal const byte MemSize = 19;
			internal const byte ConfigDone = 20;        /* Set when memory area configured */
			internal const byte Sync = 21;              /* Write to force synchronization */
			internal const byte Busy = 22;              /* Read to check if sync is done */
			internal const byte GuestID = 23;           /* Set guest OS identifier */
			internal const byte CursorID = 24;          /* ID of cursor */
			internal const byte CursorX = 25;           /* Set cursor X position */
			internal const byte CursorY = 26;           /* Set cursor Y position */
			internal const byte CursorOn = 27;          /* Turn cursor on/off */
			internal const byte HostBitsPerPixel = 28;  /* Current bpp in the host */
			internal const byte ScratchSize = 29;       /* Number of scratch registers */
			internal const byte MemRegs = 30;           /* Number of FIFO registers */
			internal const byte NumDisplays = 31;       /* Number of guest displays */
			internal const byte PitchLock = 32;         /* Fixed pitch for all modes */
			internal const byte IRQMask = 33;           /* Interrupt mask */

			/* Legacy multi-monitor support */
			internal const byte NumGuestDisplays = 34;  /* Number of guest displays in X/Y direction */
			internal const byte DisplayID = 35;            /* Display ID for the following display attributes */
			internal const byte DisplayIsPrimary = 36;    /* Whether this is a primary display */
			internal const byte DisplayPositionX = 37;    /* The display position x */
			internal const byte DisplayPositionY = 38;    /* The display position y */
			internal const byte DisplayWidth = 39;         /* The display's width */
			internal const byte DisplayHeight = 40;        /* The display's height */

			/* See "Guest memory regions" below. */
			internal const byte GMRID = 41;
			internal const byte GMRDescriptor = 42;
			internal const byte GMRMaxIDs = 43;
			internal const byte GMRMaxDescriptorLength = 44;

			internal const byte Traces = 45;                /* Enable trace-based updates even when FIFO is on */
			internal const byte GMRsMaxPages = 46;        /* Maximum number of 4KB pages for all GMRs */
			internal const byte MemorySize = 47;           /* Total dedicated device memory excluding FIFO */
			internal const byte Top = 48;                   /* Must be 1 more than the last register */
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
			internal const byte InvalidCmd = 0;
			internal const byte Update = 1;
			internal const byte RectFill = 2;
			internal const byte RectCopy = 3;
			internal const byte DefineBitmap = 4;
			internal const byte DefineBitmapScanline = 5;
			internal const byte DefinePixmap = 6;
			internal const byte DefinePixmapScanline = 7;
			internal const byte RectBitmapFill = 8;
			internal const byte RectPixmapFill = 9;
			internal const byte RectBitmapCopy = 10;
			internal const byte RectPixmapCopy = 11;
			internal const byte FreeObject = 12;
			internal const byte RectRopFill = 13;
			internal const byte RectRopCopy = 14;
			internal const byte RectRopBitmapFill = 15;
			internal const byte RectRopPixmapFill = 16;
			internal const byte RectRopBitmapCopy = 17;
			internal const byte RectRopPixmapCopy = 18;
			internal const byte DefineCursor = 19;
			internal const byte DisplayCursor = 20;
			internal const byte MoveCursor = 21;
			internal const byte DefineAlphaCursor = 22;
			internal const byte UpdateVerbose = 25;
			internal const byte FrontRopFill = 29;
			internal const byte Fence = 30;
			internal const byte Escape = 33;
			internal const byte DefineScreen = 34;
			internal const byte DestroyScreen = 35;
			internal const byte DefineGMRFB = 36;
			internal const byte BlitGMRFBToScreen = 37;
			internal const byte BlitScreenToGMRFB = 38;
			internal const byte AnnotationFill = 39;
			internal const byte AnnotationCopy = 40;
			internal const byte DefineGMR2 = 41;
			internal const byte RemapGMR2 = 42;
			internal const byte Max = 43;
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

		/// <summary>Gets the version.</summary>
		protected uint SVGAGetVersion()
		{
			SVGASetRegister(SVGA_REGISTERS.ID, SVGA_VERSION_ID.V2);
			if (SVGAGetRegister(SVGA_REGISTERS.ID) == SVGA_VERSION_ID.V2) { return SVGA_VERSION_ID.V2; }

			SVGASetRegister(SVGA_REGISTERS.ID, SVGA_VERSION_ID.V1);
			if (SVGAGetRegister(SVGA_REGISTERS.ID) == SVGA_VERSION_ID.V1) { return SVGA_VERSION_ID.V1; }

			SVGASetRegister(SVGA_REGISTERS.ID, SVGA_VERSION_ID.V0);
			if (SVGAGetRegister(SVGA_REGISTERS.ID) == SVGA_VERSION_ID.V0) { return SVGA_VERSION_ID.V0; }

			return SVGA_VERSION_ID.Invalid;
		}

		public override void Start()
		{
			const ushort ScreenWidth = 1366;
			const ushort ScreenHeight = 768;
			const byte BitsPerPixel = 0;        // Use the hosts bitsperpixel

			if (Device.Status != DeviceStatus.Available) { return; }

			version = SVGAGetVersion();

			if (version == SVGA_VERSION_ID.V1 || version == SVGA_VERSION_ID.V2)
			{
				SVGASetRegister(SVGA_REGISTERS.GuestID, GUEST_OS.Other); // 0x05010 == GUEST_OS_OTHER (vs GUEST_OS_WIN2000)

				svgaCapabilities = SVGAGetRegister(SVGA_REGISTERS.Capabilities);
			}

			FIFOInitialize();

			SVGASetMode(ScreenWidth, ScreenHeight, BitsPerPixel);
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

			redMaskShift = GetColorMaskShift(redMask);
			greenMaskShift = GetColorMaskShift(greenMask);
			blueMaskShift = GetColorMaskShift(blueMask);
			alphaMaskShift = GetColorMaskShift(alphaMask);

			offset = SVGAGetRegister(SVGA_REGISTERS.FrameBufferOffset);

			Device.Status = DeviceStatus.Online;

			Mandelbrot();
			MosaLogoDraw(frameBuffer, 10);
		}

		public override bool OnInterrupt()
		{
			return false;
		}

		/// <summary>Sends the command.</summary>
		/// <param name="command">The command.</param>
		/// <param name="value">The value.</param>
		protected void SVGASetRegister(uint command, uint value)
		{
			indexPort.Write32(command);
			valuePort.Write32(value);
		}

		/// <summary>Gets the value.</summary>
		/// <param name="command">The command.</param>
		/// <returns></returns>
		protected uint SVGAGetRegister(uint command)
		{
			indexPort.Write32(command);
			return valuePort.Read32();
		}

		/// <summary>Sets the mode.</summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="bitsPerPixel">
		/// If bitsPerPixel=0, then HostBitsPerPixel is used automatically.
		/// If bitsPerPixel!=0, then the requested bitsPerPixel is used.
		/// </param>
		public bool SVGASetMode(ushort width, ushort height, byte bitsPerPixel = 0)
		{
			this.width = width;
			this.height = height;

			if (bitsPerPixel == 0)
			{
				// use the host's bits per pixel
				this.bitsPerPixel = SVGAGetRegister(SVGA_REGISTERS.HostBitsPerPixel);
			}
			else
			{
				// use the requested bits per pixel
				this.bitsPerPixel = bitsPerPixel;
			}

			// set height  & width
			SVGASetRegister(SVGA_REGISTERS.Width, this.width);
			SVGASetRegister(SVGA_REGISTERS.Height, this.height);
			SVGASetRegister(SVGA_REGISTERS.BitsPerPixel, this.bitsPerPixel);
			SVGASetRegister(SVGA_REGISTERS.Enable, 1);
			SVGASetRegister(SVGA_REGISTERS.ConfigDone, 1);

			// get the frame buffer offset
			offset = SVGAGetRegister(SVGA_REGISTERS.FrameBufferOffset);

			// get the bytes per line (pitch)
			bytesPerLine = SVGAGetRegister(SVGA_REGISTERS.BytesPerLine);

			switch (this.bitsPerPixel)
			{
				case 8: frameBuffer = new FrameBuffer8bpp(memory, width, height, offset, bytesPerLine, false); break;
				case 16: frameBuffer = new FrameBuffer16bpp(memory, width, height, offset, bytesPerLine, false); break;
				case 24: frameBuffer = new FrameBuffer24bpp(memory, width, height, offset, bytesPerLine, false); break;
				case 32: frameBuffer = new FrameBuffer32bpp(memory, width, height, offset, bytesPerLine, false); break;
				default: return false;
			}

			return true;
		}

		protected void SVGAShutdown()
		{
			SVGASetRegister(SVGA_REGISTERS.Enable, 0);
		}

		/// <summary>Initializes the fifo.</summary>
		protected void FIFOInitialize()
		{
			uint fifoSize = SVGAGetRegister(SVGA_REGISTERS.MemSize);

			FIFOSetRegister(FIFO_REGISTERS.Min, FifoNumRegs * 4);
			FIFOSetRegister(FIFO_REGISTERS.Max, fifoSize);
			FIFOSetRegister(FIFO_REGISTERS.NextCmd, FifoNumRegs * 4);
			FIFOSetRegister(FIFO_REGISTERS.Stop, FifoNumRegs * 4);

			SVGASetRegister(SVGA_REGISTERS.Enable, 1);
			SVGASetRegister(SVGA_REGISTERS.ConfigDone, 1);
		}

		/// <summary>Sets the fifo.</summary>
		/// <param name="index">The index.</param>
		/// <param name="value">The value.</param>
		protected void FIFOSetRegister(uint index, uint value)
		{
			fifo.Write32(index * 4, value);
		}

		/// <summary>Gets the fifo.</summary>
		/// <param name="index">The index.</param>
		protected uint FIFOGetRegister(uint index)
		{
			return fifo.Read32(index * 4);
		}

		/// <summary>Waits for fifo.</summary>
		protected void WaitForFifo()
		{
			SVGASetRegister(SVGA_REGISTERS.Sync, 1);

			while (SVGAGetRegister(SVGA_REGISTERS.Busy) != 0)
			{
				HAL.Sleep(5);
			}
		}

		/// <summary>Writes to fifo.</summary>
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

		protected bool FIFOHasCapability(uint Capability)
		{
			return (FIFOGetRegister(FIFO_REGISTERS.Capabilities) & Capability) != 0;
		}

		protected void FIFOCmdUpdate(uint X, uint Y, uint Width, uint Height)
		{
			WriteToFifo(FIFO_COMMANDS.Update);

			WriteToFifo(X);
			WriteToFifo(Y);
			WriteToFifo(Width);
			WriteToFifo(Height);

			WaitForFifo();
		}

		protected void FIFOCmdRectCopy(uint SrcX, uint SrcY, uint DstX, uint DstY, uint Width, uint Height)
		{
			WriteToFifo(FIFO_COMMANDS.RectCopy);

			WriteToFifo(SrcX);
			WriteToFifo(SrcY);
			WriteToFifo(DstX);
			WriteToFifo(DstY);
			WriteToFifo(Width);
			WriteToFifo(Height);

			WaitForFifo();
		}

		/// <summary>Gets the mask shift.</summary>
		/// <param name="colorMask">The mask.</param>
		protected byte GetColorMaskShift(uint colorMask)
		{
			if (colorMask == 0) { return 0; }

			byte count = 0;

			while ((colorMask & 1) == 0)
			{
				count++;
				colorMask >>= 1;
			}

			return count;
		}

		/// <summary>Converts the color to the frame-buffer color</summary>
		/// <param name="color">The color.</param>
		protected uint ConvertColorToUInt(Color color)
		{
			return (uint)
			(
				((color.Alpha << alphaMaskShift) & alphaMask) |
				((color.Red << redMaskShift) & redMask) |
				((color.Green << greenMaskShift) & greenMask) |
				((color.Blue << blueMaskShift) & blueMask)
			);
		}

		protected Color ConvertUIntToColor(uint color)
		{
			return new Color
			(
				(byte)((color & redMask) >> redMaskShift),
				(byte)((color & greenMask) >> greenMaskShift),
				(byte)((color & blueMask) >> blueMaskShift),
				(byte)((color & alphaMask) >> alphaMaskShift)
			);
		}

		/// <summary>Updates the whole screen.</summary>
		protected void UpdateScreen()
		{
			FIFOCmdUpdate(0, 0, this.width, this.height);
		}

		/// <summary>Updates the screen area.</summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		protected void UpdateScreen(ushort x, ushort y, ushort width, ushort height)
		{
			FIFOCmdUpdate(x, y, width, height);
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

		/// <summary>Writes a single pixel.</summary>
		/// <param name="color">The color.</param>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		public void WritePixel(Color color, ushort x, ushort y)
		{
			frameBuffer.SetPixel(ConvertColorToUInt(color), x, y);
		}

		/// <summary>Reads a single pixel.</summary>
		/// <param name="x">The x.</param>
		/// <param name="y">The y.</param>
		public Color ReadPixel(ushort x, ushort y)
		{
			uint color = frameBuffer.GetPixel(x, y);

			return ConvertUIntToColor(color);
		}

		/// <summary>Clears the screen with specified color.</summary>
		/// <param name="color">The color.</param>
		public void Clear(Color color)
		{
			uint ClearColor = 0x00000000;

			frameBuffer.FillRectangle(ClearColor, 0, 0, (uint)width / 2, (uint)height / 2);

			UpdateScreen();
		}

		public void MosaLogoDraw(IFrameBuffer frameBuffer, uint tileSize)
		{
			const uint _width = 23;
			const uint _height = 7;

			uint positionX = (frameBuffer.Width / 2) - ((_width * tileSize) / 2);
			uint positionY = (frameBuffer.Height / 2) - ((_height * tileSize) / 2);

			//Can't store these as a static fields, they seem to break something
			uint[] logo = new uint[] { 0x39E391, 0x44145B, 0x7CE455, 0x450451, 0x450451, 0x451451, 0x44E391 };
			uint[] colors = new uint[] { 0x00FF0000, 0x0000FF00, 0x000000FF, 0x00FFFF00 }; //Colors for each pixel

			for (int ty = 0; ty < _height; ty++)
			{
				uint data = logo[ty];

				for (int tx = 0; tx < _width; tx++)
				{
					int mask = 1 << tx;

					if ((data & mask) == mask)
					{
						frameBuffer.FillRectangle(colors[tx / 6], (uint)(positionX + (tileSize * tx)), (uint)(positionY + (tileSize * ty)) + 50, tileSize, tileSize); //Each pixel is aprox 5 tiles in width
					}
				}
			}

			UpdateScreen();
		}

		public static uint MandelbrotColor (byte Red, byte Green, byte Blue, byte Alpha)
		{
			return (uint)(Alpha << 24 | Red << 16 | Green << 8 | Blue << 0);
		}

		uint[] palette =
		{
				MandelbrotColor(66, 30, 15, 255),
				MandelbrotColor(25, 7, 26, 255),
				MandelbrotColor(9, 1, 47, 255),
				MandelbrotColor(4, 4, 73, 255),
				MandelbrotColor(0, 7, 100, 255),
				MandelbrotColor(12, 44, 138, 255),
				MandelbrotColor(24, 82, 177, 255),
				MandelbrotColor(57, 125, 209, 255),
				MandelbrotColor(134, 181, 229, 255),
				MandelbrotColor(211, 236, 248, 255),
				MandelbrotColor(241, 233, 191, 255),
				MandelbrotColor(248, 201, 95, 255),
				MandelbrotColor(255, 170, 0, 255),
				MandelbrotColor(204, 128, 0, 255),
				MandelbrotColor(153, 87, 0, 255),
				MandelbrotColor(106, 52, 3, 255)
		};

		public void Mandelbrot()
		{
			const double xmin = -2.1;
			const double ymin = -1.3;

			const double xmax = 1;
			const double ymax = 1.3;

			const uint maxIterations = 100;

			uint color = 0;

			int Width = height;
			int Height = height;
			ushort GapLeft = (ushort)(((ushort)width - (ushort)height) / 2);

			double x, y, x1, y1, xx;

			uint looper, s, z = 0;
			double integralX, integralY = 0.0;

			integralX = (xmax - xmin) / Width; // Make it fill the whole window
			integralY = (ymax - ymin) / Height;
			x = xmin;

			for (s = 1; s < Width; s++)
			{
				y = ymin;

				for (z = 1; z < Height; z++)
				{
					x1 = 0;
					y1 = 0;
					looper = 0;

					while (looper < maxIterations && Math.Sqrt((x1 * x1) + (y1 * y1)) < 2)
					{
						looper++;

						xx = (x1 * x1) - (y1 * y1) + x;
						y1 = 2 * x1 * y1 + y;
						x1 = xx;
					}

					color = palette[looper % 16];

					frameBuffer.SetPixel(color, z + GapLeft, s);

					y += integralY;
				}

				x += integralX;
			}

			UpdateScreen(GapLeft, 0, (ushort)height, (ushort)height);
		}
	}
}
