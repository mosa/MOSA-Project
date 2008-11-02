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
	/// 
	/// </summary>
	[PCIDeviceSignature(VendorID = 0x15AD, DeviceID = 0x0405, Platforms = PlatformArchitecture.Both_x86_and_x64)]
	public class VMware : PCIHardwareDevice, IHardwareDevice
	{
		#region Definitions

		internal struct SVGARegisters
		{
			internal const byte ID = 0;
			internal const byte ENABLE = 1;
			internal const byte WIDTH = 2;
			internal const byte HEIGHT = 3;
			internal const byte MAX_WIDTH = 4;
			internal const byte MAX_HEIGHT = 5;
			internal const byte DEPTH = 6;
			internal const byte BITS_PER_PIXEL = 7; /* Current bpp in the guest */
			internal const byte PSEUDOCOLOR = 8;
			internal const byte RED_MASK = 9;
			internal const byte GREEN_MASK = 10;
			internal const byte BLUE_MASK = 11;
			internal const byte BYTES_PER_LINE = 12;
			internal const byte FB_START = 13;
			internal const byte FB_OFFSET = 14;
			internal const byte VRAM_SIZE = 15;
			internal const byte FB_SIZE = 16;

			/* ID 0 implementation only had the above registers; then the palette */

			internal const byte CAPABILITIES = 17;
			internal const byte MEM_START = 18;		/* Memory for command FIFO and bitmaps */
			internal const byte MEM_SIZE = 19;
			internal const byte CONFIG_DONE = 20;  	/* Set when memory area configured */
			internal const byte SYNC = 21; 			/* Write to force synchronization */
			internal const byte BUSY = 22; 			/* Read to check if sync is done */
			internal const byte GUEST_ID = 23; 		/* Set guest OS identifier */
			internal const byte CURSOR_ID = 24;		/* ID of cursor */
			internal const byte CURSOR_X = 25; 		/* Set cursor X position */
			internal const byte CURSOR_Y = 26; 		/* Set cursor Y position */
			internal const byte CURSOR_ON = 27;		/* Turn cursor on/off */
			internal const byte HOST_BITS_PER_PIXEL = 28; /* Current bpp in the host */
			internal const byte SCRATCH_SIZE = 29; 	/* Number of scratch registers */
			internal const byte MEM_REGS = 30; 		/* Number of FIFO registers */
			internal const byte NUM_DISPLAYS = 31; 	/* Number of guest displays */
			internal const byte PITCHLOCK = 32;		/* Fixed pitch for all modes */
		}

		internal struct SVGAID
		{
			internal const uint Magic = 0x900000;
			internal const uint V0 = Magic << 8;
			internal const uint V1 = (Magic << 8) | 1;
			internal const uint V2 = (Magic << 8) | 2;
			internal const uint Invalid = 0xFFFFFFFF;
		}

		#endregion

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
		protected IBitMap bitMap;

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
		protected uint redMaskShift;
		/// <summary>
		/// 
		/// </summary>
		protected uint greenMaskShift;
		/// <summary>
		/// 
		/// </summary>
		protected uint blueMaskShift;
		/// <summary>
		/// 
		/// </summary>
		protected uint alphaMaskShift;
		/// <summary>
		/// 
		/// </summary>
		protected uint capabilities;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pciDevice"></param>
		public VMware(PCIDevice pciDevice) : base(pciDevice) { }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override bool Setup()
		{
			base.name = "VMWARE_SVGA_0x" + busResources.GetIOPortRegion(0).BaseIOPort.ToString("X");

			indexPort = busResources.GetIOPort(0, 0);
			valuePort = busResources.GetIOPort(0, 1);

			memory = base.busResources.GetMemory(0);

			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override bool Probe() { return true; }

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override bool Start()
		{
			capabilities = GetValue(SVGARegisters.CAPABILITIES);
			videoRamSize = GetValue(SVGARegisters.VRAM_SIZE);
			maxWidth = GetValue(SVGARegisters.MAX_WIDTH);
			maxHeight = GetValue(SVGARegisters.MAX_HEIGHT);
			bitsPerPixel = GetValue(SVGARegisters.BITS_PER_PIXEL);
			bytesPerLine = GetValue(SVGARegisters.BYTES_PER_LINE);
			redMask = GetValue(SVGARegisters.RED_MASK);
			greenMask = GetValue(SVGARegisters.GREEN_MASK);
			blueMask = GetValue(SVGARegisters.BLUE_MASK);
			redMaskShift = GetMaskShift(redMask);
			greenMaskShift = GetMaskShift(greenMask);
			blueMaskShift = GetMaskShift(blueMask);
			alphaMaskShift = GetMaskShift(alphaMask);
			offset = GetValue(SVGARegisters.FB_OFFSET);

			switch (bitsPerPixel) {
				case 8: bitMap = new BitMap8bpp(memory, maxWidth, maxHeight, offset, bytesPerLine); break;
				case 16: bitMap = new BitMap16bpp(memory, maxWidth, maxHeight, offset, bytesPerLine); break;
				case 24: bitMap = new BitMap24bpp(memory, maxWidth, maxHeight, offset, bytesPerLine); break;
				case 32: bitMap = new BitMap32bpp(memory, maxWidth, maxHeight, offset, bytesPerLine); break;
				default: return false;
			}

			return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override bool OnInterrupt() { return false; }

		/// <summary>
		/// 
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
		public bool SetMode(uint width, uint height)
		{
			SendCommand(SVGARegisters.WIDTH, width);
			SendCommand(SVGARegisters.HEIGHT, height);
			SendCommand(SVGARegisters.ENABLE, 1);

			offset = GetValue(SVGARegisters.FB_OFFSET);

			SendCommand(SVGARegisters.GUEST_ID, 0x5010); // ??
			bytesPerLine = GetValue(SVGARegisters.BYTES_PER_LINE);

			switch (bitsPerPixel) {
				case 8: bitMap = new BitMap8bpp(memory, maxWidth, maxHeight, offset, bytesPerLine); break;
				case 16: bitMap = new BitMap16bpp(memory, maxWidth, maxHeight, offset, bytesPerLine); break;
				case 24: bitMap = new BitMap24bpp(memory, maxWidth, maxHeight, offset, bytesPerLine); break;
				case 32: bitMap = new BitMap32bpp(memory, maxWidth, maxHeight, offset, bytesPerLine); break;
				default: return false;
			}

			return true;
		}

		/// <summary>
		/// Gets the version.
		/// </summary>
		/// <returns></returns>
		protected uint GetVersion()
		{
			SendCommand(SVGARegisters.ID, SVGAID.V2);
			if (GetValue(SVGARegisters.ID) == SVGAID.V2)
				return SVGAID.V2;

			SendCommand(SVGARegisters.ID, SVGAID.V1);
			if (GetValue(SVGARegisters.ID) == SVGAID.V1)
				return SVGAID.V1;

			if (GetValue(SVGARegisters.ID) == SVGAID.V0)
				return SVGAID.V0;

			return SVGAID.Invalid;
		}

		/// <summary>
		/// Gets the mask shift.
		/// </summary>
		/// <param name="mask">The mask.</param>
		/// <returns></returns>
		protected uint GetMaskShift(uint mask)
		{
			if (mask == 0)
				return 0;

			uint count = 0;

			while ((mask & 1) == 0) {
				count++;
				mask = mask >> 1;
			}

			return count;
		}
	}
}
