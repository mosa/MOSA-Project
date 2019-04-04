// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.Extension;
using System;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// Static
	/// </summary>
	public static class VBE
	{
		#region VBEModeInfoOffset

		internal struct VBEModeInfoOffset
		{
			public const int Attributes = 0;
			public const int WindowA = 2;
			public const int WindowB = 3;
			public const int Granularity = 4;
			public const int WindowSize = 6;
			public const int SegmentA = 8;
			public const int SegmentB = 10;
			public const int WinFuncPtr = 12;
			public const int Pitch = 16;
			public const int ScreenWidth = 18;
			public const int ScreenHeight = 20;
			public const int WChar = 22;
			public const int YChar = 23;
			public const int Planes = 24;
			public const int BitsPerPixel = 25;
			public const int Banks = 26;
			public const int MemoryModel = 27;
			public const int BankSize = 28;
			public const int ImagePages = 29;
			public const int Reserved0 = 30;
			public const int RedMask = 31;
			public const int RedPosition = 32;
			public const int GreenMask = 33;
			public const int GreenPosition = 34;
			public const int BlueMask = 35;
			public const int BluePosition = 36;
			public const int ReservedMask = 37;
			public const int ReservedPosition = 38;
			public const int DirectColorAttributes = 39;
			public const int PhysBase = 40;
			public const int OffScreenMemoryOff = 44;
			public const int OffScreenMemorSize = 48;
			public const int Reserved1 = 50;
		}

		#endregion VBEModeInfoOffset

		/// <summary>
		/// Gets a value indicating whether VBE is available.
		/// </summary>
		public static bool IsVBEAvailable => Multiboot.IsMultibootAvailable && !VBEModeInfo.IsNull();

		private static IntPtr VBEModeInfo => Multiboot.VBEModeInfo;

		/// <summary>
		/// Setup VBE.
		/// </summary>
		public static void Setup()
		{
		}

		private static byte GetValue8(uint offset)
		{
			return Intrinsic.Load8(VBEModeInfo, offset);
		}

		private static ushort GetValue16(uint offset)
		{
			return Intrinsic.Load16(VBEModeInfo, offset);
		}

		private static uint GetValue32(uint offset)
		{
			return Intrinsic.Load32(VBEModeInfo, offset);
		}

		private static IntPtr GetPointer(uint offset)
		{
			return Intrinsic.LoadPointer(VBEModeInfo, offset);
		}

		public static ushort Attributes { get { return GetValue16(VBEModeInfoOffset.Attributes); } }

		public static byte WindowA { get { return GetValue8(VBEModeInfoOffset.WindowA); } }

		public static byte WindowB { get { return GetValue8(VBEModeInfoOffset.WindowB); } }

		public static ushort Granularity { get { return GetValue16(VBEModeInfoOffset.Granularity); } }

		public static ushort WindowSize { get { return GetValue16(VBEModeInfoOffset.WindowSize); } }

		public static ushort SegmentA { get { return GetValue16(VBEModeInfoOffset.SegmentA); } }

		public static ushort SegmentB { get { return GetValue16(VBEModeInfoOffset.SegmentB); } }

		public static uint WinFuncPtr { get { return GetValue32(VBEModeInfoOffset.WinFuncPtr); } }

		public static ushort Pitch { get { return GetValue16(VBEModeInfoOffset.Pitch); } }

		public static ushort ScreenWidth { get { return GetValue16(VBEModeInfoOffset.ScreenWidth); } }

		public static ushort ScreenHeight { get { return GetValue16(VBEModeInfoOffset.ScreenHeight); } }

		public static byte WChar { get { return GetValue8(VBEModeInfoOffset.WChar); } }

		public static byte YChar { get { return GetValue8(VBEModeInfoOffset.YChar); } }

		public static byte Planes { get { return GetValue8(VBEModeInfoOffset.Planes); } }

		public static byte BitsPerPixel { get { return GetValue8(VBEModeInfoOffset.BitsPerPixel); } }

		public static byte Banks { get { return GetValue8(VBEModeInfoOffset.Banks); } }

		public static byte MemoryModel { get { return GetValue8(VBEModeInfoOffset.MemoryModel); } }

		public static byte BankSize { get { return GetValue8(VBEModeInfoOffset.BankSize); } }

		public static byte ImagePages { get { return GetValue8(VBEModeInfoOffset.ImagePages); } }

		public static byte Reserved0 { get { return GetValue8(VBEModeInfoOffset.Reserved0); } }

		public static byte RedMask { get { return GetValue8(VBEModeInfoOffset.RedMask); } }

		public static byte RedPosition { get { return GetValue8(VBEModeInfoOffset.RedPosition); } }

		public static byte GreenMask { get { return GetValue8(VBEModeInfoOffset.GreenMask); } }

		public static byte GreenPosition { get { return GetValue8(VBEModeInfoOffset.GreenPosition); } }

		public static byte BlueMask { get { return GetValue8(VBEModeInfoOffset.BlueMask); } }

		public static byte BluePosition { get { return GetValue8(VBEModeInfoOffset.BluePosition); } }

		public static byte ReservedMask { get { return GetValue8(VBEModeInfoOffset.ReservedMask); } }

		public static byte ReservedPosition { get { return GetValue8(VBEModeInfoOffset.ReservedPosition); } }

		public static byte DirectColorAttributes { get { return GetValue8(VBEModeInfoOffset.DirectColorAttributes); } }

		public static IntPtr MemoryPhysicalLocation { get { return GetPointer(VBEModeInfoOffset.PhysBase); } }

		public static uint OffScreenMemoryOff { get { return GetValue32(VBEModeInfoOffset.OffScreenMemoryOff); } }

		public static ushort OffScreenMemorSize { get { return GetValue16(VBEModeInfoOffset.OffScreenMemorSize); } }
	}
}
