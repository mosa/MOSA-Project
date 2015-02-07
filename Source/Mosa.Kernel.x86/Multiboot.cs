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

		unsafe internal static MultiBootInfo* MultiBootInfo;

		/// <summary>
		/// Location of the Multiboot Structure
		/// </summary>
		public static uint MultibootStructure { get; private set; }

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
			MultibootStructure = 0x0;
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
			MultibootStructure = address;
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
			get { return (MultibootStructure != 0x0); }
		}

		/// <summary>
		/// Gets the flags.
		/// </summary>
		/// <value>The flags.</value>
		public static uint Flags
		{
			get
			{
				return Intrinsic.Load32(MultibootStructure);
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
				return Intrinsic.Load32(MultibootStructure, 4);
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
				return Intrinsic.Load32(MultibootStructure, 8);
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
				return Intrinsic.Load32(MultibootStructure, 12);
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
				return Intrinsic.Load32(MultibootStructure, 16);
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
				return Intrinsic.Load32(MultibootStructure, 20);
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
				return Intrinsic.Load32(MultibootStructure, 24);
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
				return Intrinsic.Load32(MultibootStructure, 44);
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
				return Intrinsic.Load32(MultibootStructure, 48);
			}
		}

		/// <summary>
		/// Counts the memory map.
		/// </summary>
		private static void CountMemoryMap()
		{
			memoryMapCount = 0;
			uint location = MemoryMapStart;

			while (location < (MemoryMapStart + MemoryMapLength))
			{
				memoryMapCount++;
				location = Intrinsic.Load32(location) + location + 4;
			}
		}

		/// <summary>
		/// Gets the memory map index location.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		private static uint GetMemoryMapIndexLocation(uint index)
		{
			uint location = MemoryMapStart;

			for (uint i = 0; i < index; i++)
			{
				location = location + Intrinsic.Load32(location) + 4;
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
			return Intrinsic.Load32(GetMemoryMapIndexLocation(index), 4);
		}

		/// <summary>
		/// Gets the length of the memory map.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static uint GetMemoryMapLength(uint index)
		{
			return Intrinsic.Load32(GetMemoryMapIndexLocation(index), 12);
		}

		/// <summary>
		/// Gets the type of the memory map.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static byte GetMemoryMapType(uint index)
		{
			return Native.Get8(GetMemoryMapIndexLocation(index) + 20);
		}

		/// <summary>
		/// Gets the length of the drive.
		/// </summary>
		/// <value>The length of the drive.</value>
		public static uint DriveLength
		{
			get
			{
				return Intrinsic.Load32(MultibootStructure, 52);
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
				return Intrinsic.Load32(MultibootStructure, 56);
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
				return Intrinsic.Load32(MultibootStructure, 60);
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
				return Intrinsic.Load32(MultibootStructure, 64);
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
				return Intrinsic.Load32(MultibootStructure, 68);
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
				return Intrinsic.Load32(MultibootStructure, 72);
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
				return Intrinsic.Load32(MultibootStructure, 72);
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
				return Intrinsic.Load32(MultibootStructure, 76);
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
				return Intrinsic.Load32(MultibootStructure, 80);
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
				return Intrinsic.Load32(MultibootStructure, 84);
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
				return Intrinsic.Load32(MultibootStructure, 86);
			}
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	unsafe internal struct MultiBootInfo
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
		public MultiBootMemoryMap* memMapAddress;		//if bit 6 in flags is set
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
	internal struct MultiBootElfSectionHeaderTable
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
	internal struct ElfSectionHeader
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
	internal struct MultiBootMemoryMap
	{
		public uint size;
		public uint base_addr;
		public uint length;
		public uint type;
		public const uint EntrySize = 16;
	}
}