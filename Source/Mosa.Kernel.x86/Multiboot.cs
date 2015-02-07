/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Internal;
using Mosa.Kernel.Helpers;
using Mosa.Kernel.x86.Helpers;
using Mosa.Platform.Internal.x86;
using System.Runtime.InteropServices;

namespace Mosa.Kernel.x86
{
	/// <summary>
	/// Static class of helpful memory functions
	/// </summary>
	unsafe public static class Multiboot
	{
		private static uint multibootptr = 0x200004;
		private static uint multibootsignature = 0x200000;

		unsafe public static MultiBootInfo* MultiBootInfo;

		/// <summary>
		/// Magic value that indicates that kernel was loaded by a Multiboot-compliant boot loader
		/// </summary>
		public const uint MultibootMagic = 0x2BADB002;

		/// <summary>
		///
		/// </summary>
		private static uint memoryMapCount = 0;

		/// <summary>
		/// Gets the memory map count.
		/// </summary>
		/// <value>The memory map count.</value>
		public static uint MemoryMapCount { get { return memoryMapCount; } }

		/// <summary>
		/// Setups this multiboot.
		/// </summary>
		public static void Setup()
		{
			SetMultibootLocation(Intrinsic.Load32(multibootptr), Intrinsic.Load32(multibootsignature));
		}

		/// <summary>
		/// Sets the multiboot location, if given the proper magic value
		/// </summary>
		/// <param name="address">The address.</param>
		/// <param name="magic">The magic value.</param>
		public static void SetMultibootLocation(uint address, uint magic)
		{
			if (magic == MultibootMagic)
			{
				SetMultibootLocation(address);
			}
			CountMemoryMap();
		}

		/// <summary>
		/// Sets the multiboot location.
		/// </summary>
		/// <param name="address">The address.</param>
		unsafe public static void SetMultibootLocation(uint address)
		{
			MultiBootInfo = (MultiBootInfo*)address;
			//CountMemoryMap();
		}

		/// <summary>
		/// Gets a value indicating whether this instance is multiboot enabled.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if this instance is multiboot enabled; otherwise, <c>false</c>.
		/// </value>
		public static bool IsMultibootEnabled
		{
			get { return (MultiBootInfo != null); }
		}

		/// <summary>
		/// Gets the flags.
		/// </summary>
		/// <value>The flags.</value>
		public static uint Flags
		{
			get
			{
				return MultiBootInfo->flags;
			}
		}

		/// <summary>
		/// Gets the memory lower.
		/// </summary>
		/// <value>The lower memory.</value>
		public static uint MemoryLower
		{
			get
			{
				return MultiBootInfo->memLower;
			}
		}

		/// <summary>
		/// Gets the memory upper.
		/// </summary>
		/// <value>The memory upper.</value>
		public static uint MemoryUpper
		{
			get
			{
				return MultiBootInfo->memUpper;
			}
		}

		/// <summary>
		/// Gets the boot device.
		/// </summary>
		/// <value>The boot device.</value>
		public static uint BootDevice
		{
			get
			{
				return MultiBootInfo->bootDevice;
			}
		}

		/// <summary>
		/// Gets the CMD line address.
		/// </summary>
		/// <value>The CMD line address.</value>
		public static uint CmdLineAddress
		{
			get
			{
				return MultiBootInfo->commandLine;
			}
		}

		/// <summary>
		/// Gets the modules start.
		/// </summary>
		/// <value>The modules start.</value>
		public static uint ModulesStart
		{
			get
			{
				return MultiBootInfo->moduleAddress;
			}
		}

		/// <summary>
		/// Gets the modules count.
		/// </summary>
		/// <value>The modules count.</value>
		public static uint ModulesCount
		{
			get
			{
				return MultiBootInfo->moduleCount;
			}
		}

		/// <summary>
		/// Gets the length of the memory map.
		/// </summary>
		/// <value>The length of the memory map.</value>
		public static uint MemoryMapLength
		{
			get
			{
				return MultiBootInfo->memMapLength;
			}
		}

		/// <summary>
		/// Gets the memory map start.
		/// </summary>
		/// <value>The memory map start.</value>
		public static uint MemoryMapStart
		{
			get
			{
				return MultiBootInfo->memMapAddress;
			}
		}

		/// <summary>
		/// Counts the memory map.
		/// </summary>
		private static void CountMemoryMap()
		{
			memoryMapCount = 0;
			MultiBootMemoryMap* location = (MultiBootMemoryMap*)MemoryMapStart;

			while ((uint)location < (MemoryMapStart + MemoryMapLength))
			{
				memoryMapCount++;
				//location = (MultiBootMemoryMap*)(((uint)location) + location->size + 4);
				location = location->Next;
			}
		}

		/// <summary>
		/// Gets the memory map index location.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		private static MultiBootMemoryMap* GetMemoryMapIndexLocation(uint index)
		{
			MultiBootMemoryMap* location = (MultiBootMemoryMap*)MemoryMapStart;

			for (uint i = 0; i < index; i++)
			{
				location = location->Next;
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
			return (uint)GetMemoryMapIndexLocation(index)->base_addr;
		}

		/// <summary>
		/// Gets the length of the memory map.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static uint GetMemoryMapLength(uint index)
		{
			return (uint)GetMemoryMapIndexLocation(index)->length;
		}

		/// <summary>
		/// Gets the type of the memory map.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static byte GetMemoryMapType(uint index)
		{
			return (byte)GetMemoryMapIndexLocation(index)->type;
		}

		/// <summary>
		/// Gets the length of the drive.
		/// </summary>
		/// <value>The length of the drive.</value>
		public static uint DriveLength
		{
			get
			{
				return MultiBootInfo->drivesLength;
			}
		}

		/// <summary>
		/// Gets the drive start.
		/// </summary>
		/// <value>The drive start.</value>
		public static uint DriveStart
		{
			get
			{
				return MultiBootInfo->drivesAddress;
			}
		}

		/// <summary>
		/// Gets the configuration table.
		/// </summary>
		/// <value>The configuration table.</value>
		public static uint ConfigurationTable
		{
			get
			{
				return MultiBootInfo->configTable;
			}
		}

		/// <summary>
		/// Gets the name of the boot loader.
		/// </summary>
		/// <value>The name of the boot loader.</value>
		public static uint BootLoaderName
		{
			get
			{
				return Intrinsic.Load32((uint)MultiBootInfo, 64);
			}
		}

		/// <summary>
		/// Gets the APM table.
		/// </summary>
		/// <value>The APM table.</value>
		public static uint APMTable
		{
			get
			{
				return MultiBootInfo->apmTable;
			}
		}

		/// <summary>
		/// Gets the VBE control information.
		/// </summary>
		/// <value>The VBE control information.</value>
		public static uint VBEControlInformation
		{
			get
			{
				return MultiBootInfo->vbeControlInfo;
			}
		}

		/// <summary>
		/// Gets the VBE mode info.
		/// </summary>
		/// <value>The VBE mode info.</value>
		public static uint VBEModeInfo
		{
			get
			{
				return MultiBootInfo->vbeModeInfo;
			}
		}

		/// <summary>
		/// Gets the VBE mode.
		/// </summary>
		/// <value>The VBE mode.</value>
		public static uint VBEMode
		{
			get
			{
				return MultiBootInfo->vbeMode;
			}
		}

		/// <summary>
		/// Gets the VBE interface seg.
		/// </summary>
		/// <value>The VBE interface seg.</value>
		public static uint VBEInterfaceSeg
		{
			get
			{
				return MultiBootInfo->vbeInterfaceSeg;
			}
		}

		/// <summary>
		/// Gets the VBE interface off.
		/// </summary>
		/// <value>The VBE interface off.</value>
		public static uint VBEInterfaceOff
		{
			get
			{
				return MultiBootInfo->vbeInterfaceOff;
			}
		}

		/// <summary>
		/// Gets the VBE interface len.
		/// </summary>
		/// <value>The VBE interface len.</value>
		public static uint VBEInterfaceLen
		{
			get
			{
				return MultiBootInfo->vbeInterfaceLength;
			}
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	unsafe public struct MultiBootInfo
	{
		public uint flags;			//required
		public uint memLower;		//if bit 0 in flags are set
		public uint memUpper;		//if bit 0 in flags are set
		public uint bootDevice;		//if bit 1 in flags are set
		public uint commandLine;		//if bit 2 in flags are set
		public uint moduleCount;		//if bit 3 in flags are set
		public uint moduleAddress;		//if bit 3 in flags are set
		public MultiBootElfSectionHeaderTable syms;		//if bits 4 or 5 in flags are set
		public uint memMapLength;		//if bit 6 in flags is set
		public uint memMapAddress;		//if bit 6 in flags is set
		public uint drivesLength;		//if bit 7 in flags is set
		public uint drivesAddress;		//if bit 7 in flags is set
		public uint configTable;		//if bit 8 in flags is set
		public uint apmTable;		//if bit 9 in flags is set
		public uint vbeControlInfo;	//if bit 10 in flags is set
		public uint vbeModeInfo;		//if bit 11 in flags is set
		public uint vbeMode;		// all vbe_* set if bit 12 in flags are set
		public uint vbeInterfaceSeg;
		public uint vbeInterfaceOff;
		public uint vbeInterfaceLength;

		private static class FlagsOffset
		{
			public const byte SymIsELF = 5;
		}

		public bool SymIsELF
		{
			get
			{
				return flags.IsBitSet(5);
			}
		}

		//unsafe public uint MemoryMapCount
		//{
		//	get
		//	{
		//		return (uint)(memMapLength / MultiBootMemoryMap.EntrySize);
		//	}
		//}

		unsafe public MultiBootElfSectionHeaderTable* ElfSectionHeaderTable
		{
			get
			{
				Assert.True(SymIsELF, "MultiBoot info does not contain ELF sections");

				//FIXME: COMPILER BUG
				//fixed (void* ptr = &this)
				//	return (MultiBootElfSectionHeaderTable*)ptr;

				uint ui;
				fixed (void* ptr = &syms)
					ui = (uint)ptr;
				return (MultiBootElfSectionHeaderTable*)ui;
			}
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MultiBootElfSectionHeaderTable
	{
		public uint num;
		public uint size;
		unsafe public ElfSectionHeader* Sections;
		public uint shndx;

		unsafe public ElfSectionHeader* StringTableSection
		{
			get
			{
				return Sections + shndx;
			}
		}

		unsafe public StringBuffer GetSectionName(int idx)
		{
			//return StringBuffer.CreateFromNullTerminatedString((byte*)(StringTableSection->sh_addr + (Sections + idx)->sh_name));

			//TODO: Why name idx -1?
			return StringBuffer.CreateFromNullTerminatedString((byte*)(StringTableSection->sh_addr + (Sections + idx)->sh_name - 1));
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct ElfSectionHeader
	{
		public uint sh_name;                /* Section name (string tbl index) */
		public uint sh_type;                /* Section type */
		public uint sh_flags;               /* Section flags */
		public uint sh_addr;                /* Section virtual addr at execution */
		public uint sh_offset;              /* Section file offset */
		public uint sh_size;                /* Section size in bytes */
		public uint sh_link;                /* Link to another section */
		public uint sh_info;                /* Additional section information */
		public uint sh_addralign;           /* Section alignment */
		public uint sh_entsize;             /* Entry size if section holds table */
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MultiBootMemoryMap
	{
		public uint size;
		public ulong base_addr;
		public ulong length;
		public uint type;
		public const uint EntrySize = 16;

		//public bool IsLast
		//{
		//	get { return size == 0; }
		//}

		unsafe private MultiBootMemoryMap* thisPtr
		{
			get
			{
				//fixed (MultiBootMemoryMap* ptr = &this)
				//	return ptr;

				uint addr;
				fixed (void* ptr = &this)
					addr = (uint)ptr;
				return (MultiBootMemoryMap*)addr;
			}
		}

		unsafe public MultiBootMemoryMap* Next
		{
			get
			{
				//Assert.False(IsLast);
				return (MultiBootMemoryMap*)(((uint)thisPtr) + size + 4);
			}
		}
	}
}