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
using Mosa.Runtime;

namespace Mosa.DeviceDriver.PCI.VMware;

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

	private struct SvgaVersionId
	{
		internal const uint Magic = 0x900000;
		internal const uint V0 = (Magic << 8) | 0;
		internal const uint V1 = (Magic << 8) | 1;
		internal const uint V2 = (Magic << 8) | 2;
	}

	private struct FifoCapability
	{
		internal const uint None = 0;
		internal const uint Fence = 1 << 0;
		internal const uint AccelFront = 1 << 1;
		internal const uint PitchLock = 1 << 2;
		internal const uint Video = 1 << 3;
		internal const uint CursorBypass3 = 1 << 4;
		internal const uint Escape = 1 << 5;
		internal const uint Reserve = 1 << 6;
		internal const uint ScreenObject = 1 << 7;
		internal const uint Gmr2 = 1 << 8;
		internal const uint ThreeDHwVersionRevised = Gmr2;
		internal const uint ScreenObject2 = 1 << 9;
		internal const uint Dead = 1 << 10;
	}

	private struct SvgaRegister
	{
		internal const byte Id = 0;
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
		internal const byte FrameBufferStart = 13; // Deprecated!
		internal const byte FrameBufferOffset = 14;
		internal const byte VRamSize = 15;
		internal const byte FrameBufferSize = 16;

		/* ID 0 implementation only had the above registers; then the palette */

		internal const byte Capabilities = 17;
		internal const byte MemSize = 19;
		internal const byte ConfigDone = 20;        /* Set when memory area configured */
		internal const byte Sync = 21;              /* Write to force synchronization */
		internal const byte Busy = 22;              /* Read to check if sync is done */
		internal const byte GuestId = 23;           /* Set guest OS identifier */
		internal const byte ScratchSize = 29;       /* Number of scratch registers */
		internal const byte MemRegs = 30;           /* Number of FIFO registers */
		internal const byte NumDisplays = 31;       /* Number of guest displays */
		internal const byte PitchLock = 32;         /* Fixed pitch for all modes */
		internal const byte IrqMask = 33;           /* Interrupt mask */

		/* Legacy multi-monitor support */
		internal const byte NumGuestDisplays = 34;  /* Number of guest displays in X/Y direction */
		internal const byte DisplayId = 35;            /* Display ID for the following display attributes */
		internal const byte DisplayIsPrimary = 36;    /* Whether this is a primary display */
		internal const byte DisplayPositionX = 37;    /* The display position x */
		internal const byte DisplayPositionY = 38;    /* The display position y */
		internal const byte DisplayWidth = 39;         /* The display's width */
		internal const byte DisplayHeight = 40;        /* The display's height */

		/* See "Guest memory regions" below. */
		internal const byte GmrId = 41;
		internal const byte GmrDescriptor = 42;
		internal const byte GmrMaxIDs = 43;
		internal const byte GmrMaxDescriptorLength = 44;

		internal const byte Traces = 45;                /* Enable trace-based updates even when FIFO is on */
		internal const byte GmrSMaxPages = 46;        /* Maximum number of 4KB pages for all GMRs */
		internal const byte MemorySize = 47;           /* Total dedicated device memory excluding FIFO */
		internal const byte Top = 48;                   /* Must be 1 more than the last register */
	}

	private struct SvgaCapability
	{
		internal const uint None = 0x00000000;
		internal const uint RectCopy = 0x00000002;
		internal const uint Cursor = 0x00000020;
		internal const uint EightBitEmulation = 0x00000100;
		internal const uint AlphaCursor = 0x00000200;
		internal const uint ThreeD = 0x00004000;
		internal const uint ExtendedFifo = 0x00008000;
		internal const uint MultiMonitor = 0x00010000;
		internal const uint PitchLock = 0x00020000;
		internal const uint IrqMask = 0x00040000;
		internal const uint DisplayTopology = 0x00080000;
		internal const uint Gmr = 0x00100000;
		internal const uint Traces = 0x00200000;
		internal const uint Gmr2 = 0x00400000;
		internal const uint ScreenObject2 = 0x00800000;
	}

	private struct FifoRegister
	{
		internal const ushort Min = 0x00;
		internal const ushort Max = 0x01;
		internal const ushort NextCmd = 0x02;
		internal const ushort Stop = 0x03;
		internal const ushort Capabilities = 0x04;
		internal const ushort Flags = 0x05;
		internal const ushort Fence = 0x06;
		internal const ushort Hw3DVersion = 0x07;
		internal const ushort PitchLock = 0x08;
		internal const ushort CursorOn = 0x09;
		internal const ushort CursorX = 0x0A;
		internal const ushort CursorY = 0x0B;
		internal const ushort CursorCount = 0x0C;
		internal const ushort CursorLastUpdated = 0x0D;
		internal const ushort Reserved = 0x0E;
		internal const ushort CursorScreenId = 0x0F;
		internal const ushort Dead = 0x10;
		internal const ushort Hw3DVersionRevised = 0x11;
		internal const ushort Capabilities3D = 0x20;
		internal const ushort Capabilities3DLast = 0x20 + 0xFF;
		internal const ushort GuestHw3DVersion = 0x120;
		internal const ushort FenceGoal = 0x121;
		internal const ushort Busy = 0x122;
		internal const ushort NumRegs = 0x123;
	}

	private struct FifoCommand
	{
		internal const byte InvalidCmd = 0;
		internal const byte Update = 1;
		internal const byte RectCopy = 3;
		internal const byte DefineCursor = 19;
		internal const byte DefineAlphaCursor = 22;
		internal const byte UpdateVerbose = 25;
		internal const byte FrontRopFill = 29;
		internal const byte Fence = 30;
		internal const byte Escape = 33;
		internal const byte DefineScreen = 34;
		internal const byte DestroyScreen = 35;
		internal const byte DefineGmrFb = 36;
		internal const byte BlitGmrFbToScreen = 37;
		internal const byte BlitScreenToGmrFb = 38;
		internal const byte AnnotationFill = 39;
		internal const byte AnnotationCopy = 40;
		internal const byte DefineGmr2 = 41;
		internal const byte RemapGmr2 = 42;
		internal const byte Max = 43;
	}

	#endregion Definitions

	private IOPortReadWrite indexPort, valuePort;

	private uint vramSize, bufferSize, fifoSize, capabilities;

	private ConstrainedPointer buffer, fifo;

	public FrameBuffer32 FrameBuffer { get; private set; }

	public override void Initialize()
	{
		Device.Name = "VMWARE_SVGA2_0x" + Device.Resources.GetIOPortRegion(0).BaseIOPort.ToString("X");

		// Doesn't work on VMware Workstation
		//buffer = Device.Resources.GetMemory(0);
		fifo = Device.Resources.GetMemory(1);

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

		WriteRegister(SvgaRegister.Id, SvgaVersionId.V2);
		if (ReadRegister(SvgaRegister.Id) != SvgaVersionId.V2)
		{
			HAL.Abort("VMware SVGA device version too old or invalid!");
			return;
		}

		vramSize = ReadRegister(SvgaRegister.VRamSize);
		fifoSize = ReadRegister(SvgaRegister.MemSize);
		capabilities = ReadRegister(SvgaRegister.Capabilities);

		WriteFifoRegister(FifoRegister.Min, FifoRegister.NumRegs * 4);
		WriteFifoRegister(FifoRegister.Max, fifoSize);
		WriteFifoRegister(FifoRegister.NextCmd, ReadFifoRegister(FifoRegister.Min));
		WriteFifoRegister(FifoRegister.Stop, ReadFifoRegister(FifoRegister.Min));

		if (HasFifoCapability(SvgaCapability.ExtendedFifo) && ReadRegister(FifoRegister.Min) > FifoRegister.GuestHw3DVersion << 2)
			WriteFifoRegister(FifoRegister.GuestHw3DVersion, (2 << 16) | (1 & 0xFF));

		Device.Status = DeviceStatus.Online;
	}

	public override bool OnInterrupt()
	{
		return false;
	}

	public void SetMode(uint width, uint height)
	{
		if (ReadRegister(SvgaRegister.Enable) == 1)
			Disable();

		// Set width, height and bpp
		WriteRegister(SvgaRegister.Width, width);
		WriteRegister(SvgaRegister.Height, height);
		WriteRegister(SvgaRegister.BitsPerPixel, 32);

		Enable();
		WriteRegister(SvgaRegister.ConfigDone, 1);

		bufferSize = ReadRegister(SvgaRegister.FrameBufferSize);
		var frame = new Pointer(ReadRegister(SvgaRegister.FrameBufferStart));

		buffer = HAL.GetPhysicalMemory(frame, bufferSize);

		var offset = ReadRegister(SvgaRegister.FrameBufferOffset);
		var bytesPerLine = ReadRegister(SvgaRegister.BytesPerLine);

		FrameBuffer = new FrameBuffer32(buffer, width, height, (x, y) => offset + y * bytesPerLine + x * 4);
	}

	public void Disable()
	{
		WriteRegister(SvgaRegister.Enable, 0);
	}

	public void Enable()
	{
		WriteRegister(SvgaRegister.Enable, 1);
	}

	public void Update(uint x, uint y, uint width, uint height)
	{
		WriteToFifo(FifoCommand.Update);

		WriteToFifo(x);
		WriteToFifo(y);
		WriteToFifo(width);
		WriteToFifo(height);
	}

	public void CopyRectangle(uint x, uint y, uint newX, uint newY, uint width, uint height)
	{
		if ((capabilities & SvgaCapability.RectCopy) == 0)
			throw new InvalidOperationException();

		WriteToFifo(FifoCommand.RectCopy);

		WriteToFifo(x);
		WriteToFifo(y);
		WriteToFifo(newX);
		WriteToFifo(newY);
		WriteToFifo(width);
		WriteToFifo(height);
	}

	public bool SupportsHardwareCursor()
	{
		return (capabilities & SvgaCapability.AlphaCursor) != 0;
	}

	public void DefineCursor(FrameBuffer32 image)
	{
		if (!SupportsHardwareCursor())
			throw new InvalidOperationException();

		WriteToFifo(FifoCommand.DefineAlphaCursor);
		WriteToFifo(0); // ID
		WriteToFifo(0); // Hotspot X
		WriteToFifo(0); // Hotspot Y
		WriteToFifo(image.Width); // Width
		WriteToFifo(image.Height); // Height

		for (uint y = 0; y < image.Height; y++)
			for (uint x = 0; x < image.Width; x++)
				WriteToFifo(image.GetPixel(x, y));
	}

	public void SetCursor(bool visible, uint x, uint y)
	{
		if (HasFifoCapability(FifoCapability.ScreenObject))
			WriteFifoRegister(FifoRegister.CursorScreenId, 0);

		if (!HasFifoCapability(FifoCapability.CursorBypass3))
			throw new InvalidOperationException();

		WriteFifoRegister(FifoRegister.CursorOn, (uint)(visible ? 1 : 0));
		WriteFifoRegister(FifoRegister.CursorX, x);
		WriteFifoRegister(FifoRegister.CursorY, y);
		WriteFifoRegister(FifoRegister.CursorCount, ReadFifoRegister(FifoRegister.CursorCount) + 1);
	}

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
		WriteRegister(SvgaRegister.Sync, 1);

		while (ReadRegister(SvgaRegister.Busy) != 0)
			HAL.Yield();
	}

	private void WriteToFifo(uint value)
	{
		if (ReadFifoRegister(FifoRegister.NextCmd) == ReadFifoRegister(FifoRegister.Max) - 4 && ReadFifoRegister(FifoRegister.Stop) == ReadFifoRegister(FifoRegister.Min) ||
			ReadFifoRegister(FifoRegister.NextCmd) + 4 == ReadFifoRegister(FifoRegister.Stop))
			WaitForFifo();

		WriteFifoRegister(ReadFifoRegister(FifoRegister.NextCmd) / 4, value);
		WriteFifoRegister(FifoRegister.NextCmd, ReadFifoRegister(FifoRegister.NextCmd) + 4);

		if (ReadFifoRegister(FifoRegister.NextCmd) == ReadFifoRegister(FifoRegister.Max))
			WriteFifoRegister(FifoRegister.NextCmd, ReadFifoRegister(FifoRegister.Min));
	}

	private bool HasFifoCapability(uint capability)
	{
		return (ReadFifoRegister(FifoRegister.Capabilities) & capability) != 0;
	}

	#endregion FIFO
}
