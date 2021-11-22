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

using Mosa.DeviceSystem;
using Mosa.Runtime;
using System;
using System.Drawing;

namespace Mosa.DeviceDriver.PCI.VMware
{
	public struct SVGAFifoCmdDefineCursor
	{
		public uint ID; // Reserved, must be zero.

		public uint HotspotX;
		public uint HotspotY;

		public uint Width;
		public uint Height;

		public uint AndMaskDepth; // Value must be 1 or equal to BITS_PER_PIXEL
		public uint XorMaskDepth; // Value must be 1 or equal to BITS_PER_PIXEL

		/*
		Followed by scanline data for AND mask, then XOR mask.
		Each scanline is padded to a 32-bit boundary.
		*/
	}

	/// <summary>
	/// VMware SVGA II Device Driver
	/// </summary>
	//[PCIDeviceDriver(VendorID = 0x15AD, DeviceID = 0x0405, Platforms = PlatformArchitecture.X86AndX64)]
	public class VMwareSVGA2 : BaseDeviceDriver, IGraphicsDevice
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

		private BaseIOPortReadWrite indexPort, valuePort;

		private ConstrainedPointer fifo;

		private FrameBuffer32 frameBuffer;

		/// <summary>The frame buffer.</summary>
		public FrameBuffer32 FrameBuffer { get => frameBuffer; }

		public override void Initialize()
		{
			Device.Name = "VMWARE_SVGA2_0x" + Device.Resources.GetIOPortRegion(0).BaseIOPort.ToString("X");

			indexPort = Device.Resources.GetIOPortReadWrite(0, 0);
			valuePort = Device.Resources.GetIOPortReadWrite(0, 1);
		}

		public override void Probe()
		{
			Device.Status = DeviceStatus.Available;
		}

		public override void Start()
		{
			if (Device.Status != DeviceStatus.Available)
				return;

			uint version = GetVersion();
			if (version == SVGA_VERSION_ID.V1 || version == SVGA_VERSION_ID.V2)
				WriteRegister(SVGA_REGISTERS.GuestID, GUEST_OS.Other); // 0x05010 == GUEST_OS_OTHER (vs GUEST_OS_WIN2000)

			Device.Status = DeviceStatus.Online;
		}

		public override bool OnInterrupt()
		{
			return false;
		}

		/// <summary>Sets the mode.</summary>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		public void SetMode(ushort width, ushort height)
		{
			Disable();

			// Set width, height and bpp
			WriteRegister(SVGA_REGISTERS.Width, width);
			WriteRegister(SVGA_REGISTERS.Height, height);
			WriteRegister(SVGA_REGISTERS.BitsPerPixel, 32);

			Enable();

			// Initialize FIFO
			uint fifoSize = ReadRegister(SVGA_REGISTERS.MemSize);
			uint fifoNumRegs = FIFO_REGISTERS.NumRegs * 4;

			fifo = HAL.GetPhysicalMemory(
				new Pointer(ReadRegister(SVGA_REGISTERS.MemStart)),
				fifoSize
			);

			WriteFifoRegister(FIFO_REGISTERS.Min, fifoNumRegs);
			WriteFifoRegister(FIFO_REGISTERS.Max, fifoSize);
			WriteFifoRegister(FIFO_REGISTERS.NextCmd, fifoNumRegs);
			WriteFifoRegister(FIFO_REGISTERS.Stop, fifoNumRegs);

			WriteRegister(SVGA_REGISTERS.ConfigDone, 1);

			// Create frame buffer
			frameBuffer = new FrameBuffer32(
				HAL.GetPhysicalMemory(
					new Pointer(ReadRegister(SVGA_REGISTERS.FrameBufferStart)),
					ReadRegister(SVGA_REGISTERS.FrameBufferSize)),

				width, height,

				ReadRegister(SVGA_REGISTERS.FrameBufferOffset),
				ReadRegister(SVGA_REGISTERS.BytesPerLine)
			);
		}

		/// <summary>Disables the graphics device.</summary>
		public void Disable()
		{
			WriteRegister(SVGA_REGISTERS.Enable, 0);
		}

		/// <summary>Enables the graphics device.</summary>
		public void Enable()
		{
			WriteRegister(SVGA_REGISTERS.Enable, 1);
		}

		/// <summary>Updates the whole screen.</summary>
		public void Update()
		{
			Update(0, 0, FrameBuffer.Width, FrameBuffer.Height);
		}

		/*public unsafe void DefineCursor(SVGAFifoCmdDefineCursor* info)
		{
			uint andPitch = ((info->AndMaskDepth * info->Width + 31) >> 5) << 2;
			uint andSize = andPitch * info->Height;
			uint xorPitch = ((info->XorMaskDepth * info->Width + 31) >> 5) << 2;
			uint xorSize = xorPitch * info->Height;

			SVGAFifoCmdDefineCursor* cmd = 0; // TODO
			*cmd = *info;

			void** andMask, xorMask;
			*andMask = (void*)(cmd + 1);
			*xorMask = (void*)(andSize + (byte*)*andMask);

			// Black
			*andMask = 0xFFFFFF;
			*xorMask = (uint)Color.Gray.ToArgb();

			// TODO: Write the pointer to FIFO
		}*/

		#region Private

		private void WriteRegister(uint command, uint value)
		{
			indexPort.Write32(command);
			valuePort.Write32(value);
		}

		private uint ReadRegister(uint command)
		{
			indexPort.Write32(command);
			return valuePort.Read32();
		}

		private uint GetVersion()
		{
			WriteRegister(SVGA_REGISTERS.ID, SVGA_VERSION_ID.V2);
			if (ReadRegister(SVGA_REGISTERS.ID) == SVGA_VERSION_ID.V2)
				return SVGA_VERSION_ID.V2;

			WriteRegister(SVGA_REGISTERS.ID, SVGA_VERSION_ID.V1);
			if (ReadRegister(SVGA_REGISTERS.ID) == SVGA_VERSION_ID.V1)
				return SVGA_VERSION_ID.V1;

			WriteRegister(SVGA_REGISTERS.ID, SVGA_VERSION_ID.V0);
			if (ReadRegister(SVGA_REGISTERS.ID) == SVGA_VERSION_ID.V0)
				return SVGA_VERSION_ID.V0;

			return SVGA_VERSION_ID.Invalid;
		}

		#endregion Private

		#region FIFO

		private void WriteFifoRegister(uint index, uint value)
		{
			fifo.Write32(index * 4, value);
		}

		private uint ReadFifoRegister(uint index)
		{
			return fifo.Read32(index * 4);
		}

		private void WaitForFifo()
		{
			WriteRegister(SVGA_REGISTERS.Sync, 1);

			while (ReadRegister(SVGA_REGISTERS.Busy) != 0)
				HAL.Sleep(5);
		}

		private void WriteToFifo(uint value)
		{
			if (((ReadFifoRegister(FIFO_REGISTERS.NextCmd) == ReadFifoRegister(FIFO_REGISTERS.Max) - 4) && ReadFifoRegister(FIFO_REGISTERS.Stop) == ReadFifoRegister(FIFO_REGISTERS.Min)) ||
				(ReadFifoRegister(FIFO_REGISTERS.NextCmd) + 4 == ReadFifoRegister(FIFO_REGISTERS.Stop)))
				WaitForFifo();

			WriteFifoRegister(ReadFifoRegister(FIFO_REGISTERS.NextCmd) / 4, value);
			WriteFifoRegister(FIFO_REGISTERS.NextCmd, ReadFifoRegister(FIFO_REGISTERS.NextCmd) + 4);

			if (ReadFifoRegister(FIFO_REGISTERS.NextCmd) == ReadFifoRegister(FIFO_REGISTERS.Max))
				WriteFifoRegister(FIFO_REGISTERS.NextCmd, ReadFifoRegister(FIFO_REGISTERS.Min));
		}

		private void Update(uint X, uint Y, uint Width, uint Height)
		{
			WriteToFifo(FIFO_COMMANDS.Update);

			WriteToFifo(X);
			WriteToFifo(Y);
			WriteToFifo(Width);
			WriteToFifo(Height);
		}

		private void RectCopy(uint SrcX, uint SrcY, uint DstX, uint DstY, uint Width, uint Height)
		{
			WriteToFifo(FIFO_COMMANDS.RectCopy);

			WriteToFifo(SrcX);
			WriteToFifo(SrcY);
			WriteToFifo(DstX);
			WriteToFifo(DstY);
			WriteToFifo(Width);
			WriteToFifo(Height);
		}

		#endregion FIFO
	}
}
