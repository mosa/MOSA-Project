// Copyright (c) MOSA Project. Licensed under the New BSD License.

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
			uint eax = Native.GetMultibootEAX();
			uint ebx = Native.GetMultibootEBX();

			SetMultibootLocation(ebx, eax);
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
				return MultiBootInfo->Flags;
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
				return MultiBootInfo->MemLower;
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
				return MultiBootInfo->MemUpper;
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
				return MultiBootInfo->BootDevice;
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
				return MultiBootInfo->CommandLine;
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
				return MultiBootInfo->ModuleAddress;
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
				return MultiBootInfo->ModuleCount;
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
				return MultiBootInfo->MemMapLength;
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
				return MultiBootInfo->MemMapAddress;
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
			return (uint)GetMemoryMapIndexLocation(index)->BaseAddr;
		}

		/// <summary>
		/// Gets the length of the memory map.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static uint GetMemoryMapLength(uint index)
		{
			return (uint)GetMemoryMapIndexLocation(index)->Length;
		}

		/// <summary>
		/// Gets the type of the memory map.
		/// </summary>
		/// <param name="index">The index.</param>
		/// <returns></returns>
		public static byte GetMemoryMapType(uint index)
		{
			return (byte)GetMemoryMapIndexLocation(index)->Type;
		}

		/// <summary>
		/// Gets the length of the drive.
		/// </summary>
		/// <value>The length of the drive.</value>
		public static uint DriveLength
		{
			get
			{
				return MultiBootInfo->DrivesLength;
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
				return MultiBootInfo->DrivesAddress;
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
				return MultiBootInfo->ConfigTable;
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
				return MultiBootInfo->ApmTable;
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
				return MultiBootInfo->VbeControlInfo;
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
				return MultiBootInfo->VbeModeInfo;
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
				return MultiBootInfo->VbeMode;
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
				return MultiBootInfo->VbeInterfaceSeg;
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
				return MultiBootInfo->VbeInterfaceOff;
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
				return MultiBootInfo->VbeInterfaceLength;
			}
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	unsafe public struct MultiBootInfo
	{
		public uint Flags;              //required
		public uint MemLower;           //if bit 0 in flags are set
		public uint MemUpper;           //if bit 0 in flags are set
		public uint BootDevice;       //if bit 1 in flags are set
		public uint CommandLine;        //if bit 2 in flags are set
		public uint ModuleCount;        //if bit 3 in flags are set
		public uint ModuleAddress;  //if bit 3 in flags are set
		public MultiBootElfSectionHeaderTable Syms; //if bits 4 or 5 in flags are set
		public uint MemMapLength;       //if bit 6 in flags is set
		public uint MemMapAddress;  //if bit 6 in flags is set
		public uint DrivesLength;       //if bit 7 in flags is set
		public uint DrivesAddress;  //if bit 7 in flags is set
		public uint ConfigTable;        //if bit 8 in flags is set
		public uint ApmTable;               //if bit 9 in flags is set
		public uint VbeControlInfo; //if bit 10 in flags is set
		public uint VbeModeInfo;        //if bit 11 in flags is set
		public uint VbeMode;                // all vbe_* set if bit 12 in flags are set
		public uint VbeInterfaceSeg;
		public uint VbeInterfaceOff;
		public uint VbeInterfaceLength;

		private static class FlagsOffset
		{
			public const byte SymIsELF = 5;
		}

		public bool SymIsELF
		{
			get
			{
				return Flags.IsBitSet(5);
			}
		}

		unsafe public MultiBootElfSectionHeaderTable* ElfSectionHeaderTable
		{
			get
			{
				Assert.True(SymIsELF, "MultiBoot info does not contain ELF sections");

				fixed (void* ptr = &this)
					return (MultiBootElfSectionHeaderTable*)ptr;
			}
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MultiBootElfSectionHeaderTable
	{
		public uint Num;
		public uint Size;
		unsafe public MultiBootElfSectionHeader* Sections;
		public uint Shndx;

		unsafe public MultiBootElfSectionHeader* StringTableSection
		{
			get
			{
				return Sections + Shndx;
			}
		}

		unsafe public StringBuffer GetSectionName(int idx)
		{
			//TODO / confirm: Why name idx -1?
			return StringBuffer.CreateFromNullTerminatedString((byte*)(StringTableSection->Addr + (Sections + idx)->Name - 1));
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MultiBootElfSectionHeader
	{
		public uint Name;                /* Section name (string tbl index) */
		public uint Type;                /* Section type */
		public uint Flags;               /* Section flags */
		public uint Addr;                /* Section virtual addr at execution */
		public uint Offset;              /* Section file offset */
		public uint Size;                /* Section size in bytes */
		public uint Link;                /* Link to another section */
		public uint Info;                /* Additional section information */
		public uint AddrAlign;           /* Section alignment */
		public uint EntSize;             /* Entry size if section holds table */
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct MultiBootMemoryMap
	{
		public uint Size;
		public ulong BaseAddr;
		public ulong Length;
		public uint Type;

		//public bool IsLast
		//{
		//	get { return size == 0; }
		//}

		unsafe private MultiBootMemoryMap* thisPtr
		{
			get
			{
				fixed (MultiBootMemoryMap* ptr = &this)
					return ptr;
			}
		}

		unsafe public MultiBootMemoryMap* Next
		{
			get
			{
				//Assert.False(IsLast);
				return (MultiBootMemoryMap*)(((uint)thisPtr) + Size + 4);
			}
		}
	}
}
