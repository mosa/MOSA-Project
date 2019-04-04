// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.Extension;
using Mosa.Runtime.x86;
using System;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// Multiboot
	/// </summary>
	public static class Multiboot
	{
		/// <summary>
		/// Magic value that indicates that kernel was loaded by a Multiboot-compliant boot loader
		/// </summary>
		public const uint MultibootMagic = 0x2BADB002;

		#region MultiBootInfoOffset

		private struct MultiBootInfoOffset
		{
			public const uint Flags = 0;
			public const uint MemLower = 4;
			public const uint MemUpper = 8;
			public const uint BootDevice = 12;
			public const uint CommandLine = 16;
			public const uint ModuleCount = 20;
			public const uint ModuleAddress = 24;
			public const uint Syms1 = 28;
			public const uint Syms2 = 32;
			public const uint Syms3 = 36;
			public const uint Syms4 = 40;
			public const uint MemMapLength = 44;
			public const uint MemMapAddress = 48;
			public const uint DriveLength = 52;
			public const uint DriveAddress = 56;
			public const uint ConfigTable = 60;
			public const uint BootLoaderName = 64;
			public const uint ApmTable = 68;
			public const uint VbeControlInfo = 72;
			public const uint VbeModeInfo = 76;
			public const uint VbeMode = 80;
			public const uint VbeInterfaceSeg = 84;
			public const uint VbeInterfaceOff = 88;
			public const uint VbeInterfaceLength = 92;
		}

		#endregion MultiBootInfoOffset

		#region MultiBootMemoryMapOffset

		private struct MultiBootMemoryMapOffset
		{
			public const uint Size = 0;
			public const uint BaseAddr = 4;
			public const uint Length = 12;
			public const uint Type = 20;
			public const uint Next = 24;
		}

		#endregion MultiBootMemoryMapOffset

		/// <summary>
		/// Location of the Multiboot Structure
		/// </summary>
		public static IntPtr MultibootStructure { get; private set; }

		/// <summary>
		/// Gets the memory map count.
		/// </summary>
		/// <value>The memory map count.</value>
		public static uint MemoryMapCount { get; internal set; }

		/// <summary>
		/// Setup multiboot.
		/// </summary>
		public static void Setup()
		{
			MultibootStructure = IntPtr.Zero;

			var magic = Native.GetMultibootEAX();

			if (magic == MultibootMagic)
			{
				var address = Native.GetMultibootEBX();

				MultibootStructure = new IntPtr(address);// Intrinsic.LoadPointer(new IntPtr(address));
				CountMemoryMap();
			}
		}

		/// <summary>
		/// Gets a value indicating whether multiboot is enabled.
		/// </summary>
		public static bool IsMultibootAvailable => !MultibootStructure.IsNull();

		private static uint GetValue(uint offset)
		{
			return Intrinsic.Load32(MultibootStructure, offset);
		}

		private static IntPtr GetPointer(uint offset)
		{
			return Intrinsic.LoadPointer(MultibootStructure, offset);
		}

		/// <summary>
		/// Gets the flags.
		/// </summary>
		public static uint Flags { get { return GetValue(MultiBootInfoOffset.Flags); } }

		/// <summary>
		/// Gets the memory lower.
		/// </summary>
		public static IntPtr MemoryLower { get { return GetPointer(MultiBootInfoOffset.MemLower); } }

		/// <summary>
		/// Gets the memory upper.
		/// </summary>
		public static IntPtr MemoryUpper { get { return GetPointer(MultiBootInfoOffset.MemUpper); } }

		/// <summary>
		/// Gets the boot device.
		/// </summary>
		public static uint BootDevice { get { return GetValue(MultiBootInfoOffset.BootDevice); } }

		/// <summary>
		/// Gets the command line address.
		/// </summary>
		public static IntPtr CommandLineAddress { get { return GetPointer(MultiBootInfoOffset.CommandLine); } }

		/// <summary>
		/// Gets the module count.
		/// </summary>
		public static uint ModuleCount { get { return GetValue(MultiBootInfoOffset.ModuleCount); } }

		/// <summary>
		/// Gets the module start.
		/// </summary>
		public static IntPtr ModuleStart { get { return GetPointer(MultiBootInfoOffset.ModuleAddress); } }

		/// <summary>
		/// Gets the length of the memory map.
		/// </summary>
		public static uint MemoryMapLength { get { return GetValue(MultiBootInfoOffset.MemMapLength); } }

		/// <summary>
		/// Gets the memory map start.
		/// </summary>
		public static IntPtr MemoryMapStart { get { return GetPointer(MultiBootInfoOffset.MemMapAddress); } }

		/// <summary>
		/// Gets the length of the drive.
		/// </summary>
		public static uint DriveLength { get { return GetValue(MultiBootInfoOffset.DriveLength); } }

		/// <summary>
		/// Gets the drive start.
		/// </summary>
		public static uint DriveStart { get { return GetValue(MultiBootInfoOffset.DriveAddress); } }

		/// <summary>
		/// Gets the configuration table.
		/// </summary>
		public static uint ConfigurationTable { get { return GetValue(MultiBootInfoOffset.ConfigTable); } }

		/// <summary>
		/// Gets the name of the boot loader address.
		/// </summary>
		public static uint BootLoaderName { get { return GetValue(MultiBootInfoOffset.BootLoaderName); } }

		/// <summary>
		/// Gets the APM table.
		/// </summary>
		public static IntPtr APMTable { get { return GetPointer(MultiBootInfoOffset.ApmTable); } }

		/// <summary>
		/// Gets the VBE control information.
		/// </summary>
		public static uint VBEControlInformation { get { return GetValue(MultiBootInfoOffset.VbeControlInfo); } }

		/// <summary>
		/// Gets the VBE mode info.
		/// </summary>
		public static IntPtr VBEModeInfo { get { return GetPointer(MultiBootInfoOffset.VbeModeInfo); } }

		/// <summary>
		/// Gets the VBE mode.
		/// </summary>
		public static uint VBEMode { get { return GetValue(MultiBootInfoOffset.VbeMode); } }

		/// <summary>
		/// Gets the VBE interface seg.
		/// </summary>
		public static uint VBEInterfaceSeg { get { return GetValue(MultiBootInfoOffset.VbeInterfaceSeg); } }

		/// <summary>
		/// Gets the VBE interface off.
		/// </summary>
		public static uint VBEInterfaceOff { get { return GetValue(MultiBootInfoOffset.VbeInterfaceOff); } }

		/// <summary>
		/// Gets the VBE interface len.
		/// </summary>
		public static uint VBEInterfaceLen { get { return GetValue(MultiBootInfoOffset.VbeInterfaceLength); } }

		/// <summary>
		/// Counts the memory map.
		/// </summary>
		private static void CountMemoryMap()
		{
			MemoryMapCount = 0;

			var location = MemoryMapStart;
			var end = IntPtr.Add(location, (int)MemoryMapLength);

			while (location.LessThan(end))
			{
				MemoryMapCount++;

				var size = Intrinsic.Load32(location, MultiBootMemoryMapOffset.Size) + 4;
				location += (int)size;
			}
		}

		/// <summary>
		/// Gets the memory map index location.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		private static IntPtr GetMemoryMapIndexLocation(uint index)
		{
			var location = MemoryMapStart;

			for (uint i = 0; i < index; i++)
			{
				var size = Intrinsic.Load32(location, MultiBootMemoryMapOffset.Size) + 4;

				location += (int)size;
			}

			return location;
		}

		/// <summary>
		/// Gets the memory map base.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static uint GetMemoryMapBase(uint index)
		{
			return Intrinsic.Load32(GetMemoryMapIndexLocation(index), MultiBootMemoryMapOffset.BaseAddr);
		}

		/// <summary>
		/// Gets the length of the memory map.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static uint GetMemoryMapLength(uint index)
		{
			return Intrinsic.Load32(GetMemoryMapIndexLocation(index), MultiBootMemoryMapOffset.Length);
		}

		/// <summary>
		/// Gets the type of the memory map.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static byte GetMemoryMapType(uint index)
		{
			return Intrinsic.Load8(GetMemoryMapIndexLocation(index), MultiBootMemoryMapOffset.Type);
		}
	}
}
