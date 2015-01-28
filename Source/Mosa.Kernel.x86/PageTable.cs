/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.Kernel.Helpers;
using Mosa.Platform.Internal.x86;
using System.Runtime.InteropServices;

namespace Mosa.Kernel.x86
{
	/// <summary>
	///
	/// </summary>
	//public static class PageTable_
	//{
	//	// Location for page directory starts at 20MB
	//	private static uint pageDirectory = 1024 * 1024 * 20; // 0x1400000

	//	// Location for page tables start at 16MB
	//	private static uint pageTable = 1024 * 1024 * 16;	// 0x1000000

	//	/// <summary>
	//	/// Sets up the PageTable
	//	/// </summary>
	//	public static void Setup()
	//	{
	//		// Setup Page Directory
	//		for (int index = 0; index < 1024; index++)
	//		{
	//			Native.Set32((uint)(pageDirectory + (index << 2)), (uint)(pageTable + (index * 4096) | 0x04 | 0x02 | 0x01));
	//		}

	//		// Map the first 256MB of memory (65536 4K pages) (why 256MB?)
	//		for (int index = 0; index < 1024 * 64; index++)
	//		{
	//			Native.Set32((uint)(pageTable + (index << 2)), (uint)(index * 4096) | 0x04 | 0x02 | 0x01);
	//		}

	//		// Set CR3 register on processor - sets page directory
	//		Native.SetCR3(pageDirectory);

	//		// Set CR0 register on processor - turns on virtual memory
	//		Native.SetCR0(Native.GetCR0() | BitMask.Bit31);
	//	}

	//	/// <summary>
	//	/// Maps the virtual address to physical.
	//	/// </summary>
	//	/// <param name="virtualAddress">The virtual address.</param>
	//	/// <param name="physicalAddress">The physical address.</param>
	//	public static void MapVirtualAddressToPhysical(uint virtualAddress, uint physicalAddress)
	//	{
	//		//FUTURE: traverse page directory from CR3 --- do not assume page table is linearly allocated
	//		Native.Set32(pageTable + ((virtualAddress & 0xFFC00000) >> 10), (uint)(physicalAddress & 0xFFC00000 | 0x04 | 0x02 | 0x01));
	//	}

	//	/// <summary>
	//	/// Gets the physical memory.
	//	/// </summary>
	//	/// <param name="virtualAddress">The virtual address.</param>
	//	/// <returns></returns>
	//	public static uint GetPhysicalAddressFromVirtual(uint virtualAddress)
	//	{
	//		//FUTURE: traverse page directory from CR3 --- do not assume page table is linearly allocated
	//		return Native.Get32(pageTable + ((virtualAddress & 0xFFFFF000) >> 10)) + (virtualAddress & 0xFFF);
	//	}
	//}

	unsafe public static class PageTable
	{
		// Location for page directory starts at 20MB
		private static uint pageDirectoryAddress = 1024 * 1024 * 20; // 0x1400000

		// Location for page tables start at 16MB
		private static uint pageTableAddress = 1024 * 1024 * 16;	// 0x1000000

		internal static PageDirectoryEntry* pageDirectoryEntries;
		internal static PageTableEntry* pageTableEntries;

		public const uint PageDirectoryLength = 1024;
		public const uint PageDirectorySize = PageDirectoryLength * PageDirectoryEntry.EntrySize;

		public const uint PageTableLength = 1024;
		public const uint PageTableSize = PageTableLength * PageTableEntry.EntrySize;

		/// <summary>
		/// Sets up the PageTable
		/// </summary>
		public static void Setup()
		{
			pageDirectoryEntries = (PageDirectoryEntry*)pageDirectoryAddress;
			pageTableEntries = (PageTableEntry*)pageTableAddress;

			ClearPageDirectory();
			ClearPageTable();

			// Map the first 256MB of memory (65536 4K pages) (why 256MB?)
			for (uint index = 0; index < 1024 * 64; index++)
				AllocatePage();

			// Set CR3 register on processor - sets page directory
			Native.SetCR3(pageDirectoryAddress);

			// Set CR0 register on processor - turns on virtual memory
			Native.SetCR0(Native.GetCR0() | BitMask.Bit31);
		}

		public static void ClearPageDirectory()
		{
			Memory.Clear(pageDirectoryAddress, PageDirectorySize);
		}

		public static void ClearPageTable()
		{
			Memory.Clear(pageTableAddress, PageTableSize);
		}

		internal static PageDirectoryEntry* GetPageDirectoryEntry(uint index)
		{
#if DEBUG
			Assert.InRange(index, PageDirectoryLength);
#endif
			return pageDirectoryEntries + index;
		}

		public static PageTableEntry* GetPageTableEntry(uint index)
		{
#if DEBUG
			Assert.InRange(index, PageDirectoryLength);
#endif
			return pageTableEntries + index;
		}

		private static uint currentDictionaryEntryCount;
		private static uint currentTableEntryCount;
		private static uint allTableEntryCount;

		private static PageDirectoryEntry* RegisterPage(PageTableEntry* page)
		{
			if (currentDictionaryEntryCount == 0 || currentTableEntryCount == 1024)
			{
				currentDictionaryEntryCount++;
				var dicEntry = GetPageDirectoryEntry(currentDictionaryEntryCount - 1);
				dicEntry->PageTableEntry = page;
				dicEntry->Present = true;
				dicEntry->Readonly = true; //??
				dicEntry->User = true;
				currentTableEntryCount = 0;
			}
			return GetPageDirectoryEntry(currentDictionaryEntryCount - 1);
		}

		public static void AllocatePage()
		{
			currentTableEntryCount++;
			allTableEntryCount++;
			var tabEntry = GetPageTableEntry(allTableEntryCount - 1);
			tabEntry->PhysicalAddress = (allTableEntryCount - 1) * 4096;
			tabEntry->Present = true;
			tabEntry->Readonly = true; //??
			tabEntry->User = true;
			RegisterPage(tabEntry);
		}

		/// <summary>
		/// Maps the virtual address to physical.
		/// </summary>
		/// <param name="virtualAddress">The virtual address.</param>
		/// <param name="physicalAddress">The physical address.</param>
		public static void MapVirtualAddressToPhysical(uint virtualAddress, uint physicalAddress)
		{
			//FUTURE: traverse page directory from CR3 --- do not assume page table is linearly allocated
			Native.Set32(pageTableAddress + ((virtualAddress & 0xFFC00000) >> 10), (uint)(physicalAddress & 0xFFC00000 | 0x04 | 0x02 | 0x01));
		}

		/// <summary>
		/// Gets the physical memory.
		/// </summary>
		/// <param name="virtualAddress">The virtual address.</param>
		/// <returns></returns>
		public static uint GetPhysicalAddressFromVirtual(uint virtualAddress)
		{
			//FUTURE: traverse page directory from CR3 --- do not assume page table is linearly allocated
			return Native.Get32(pageTableAddress + ((virtualAddress & 0xFFFFF000) >> 10)) + (virtualAddress & 0xFFF);
		}
	}

	[StructLayout(LayoutKind.Explicit)]
	unsafe public struct PageDirectoryEntry
	{
		[FieldOffset(0)]
		private uint Value;

		public const byte EntrySize = 4;

		private class Offset
		{
			public const byte Present = 0;
			public const byte Readonly = 1;
			public const byte User = 2;
			public const byte WriteThrough = 3;
			public const byte DisableCache = 4;
			public const byte Accessed = 5;
			private const byte UNKNOWN6 = 6;
			public const byte PageSize4Mib = 7;
			private const byte IGNORED8 = 8;
			public const byte Custom = 9;
			public const byte Address = 11;
		}

		private uint Address
		{
			get { return (byte)Value.GetBits(Offset.Address, 20); }
			set
			{
				if (value.GetBits(0, Offset.Address + 1) != 0)
					Panic.Error("Adress needs to be 4k aligned");
				Value = Value.SetBits(Offset.Address, 20, value);
			}
		}

		internal PageTableEntry* PageTableEntry
		{
			get { return (PageTableEntry*)Address; }
			set { Address = (uint)value; }
		}

		public bool Present
		{
			get { return Value.IsBitSet(Offset.Present); }
			set { Value = Value.SetBit(Offset.Present, value); }
		}

		public bool Readonly
		{
			get { return Value.IsBitSet(Offset.Readonly); }
			set { Value = Value.SetBit(Offset.Readonly, value); }
		}

		public bool User
		{
			get { return Value.IsBitSet(Offset.User); }
			set { Value = Value.SetBit(Offset.User, value); }
		}

		public bool WriteThrough
		{
			get { return Value.IsBitSet(Offset.WriteThrough); }
			set { Value = Value.SetBit(Offset.WriteThrough, value); }
		}

		public bool DisableCache
		{
			get { return Value.IsBitSet(Offset.DisableCache); }
			set { Value = Value.SetBit(Offset.DisableCache, value); }
		}

		public bool Accessed
		{
			get { return Value.IsBitSet(Offset.Accessed); }
			set { Value = Value.SetBit(Offset.Accessed, value); }
		}

		public bool PageSize4Mib
		{
			get { return Value.IsBitSet(Offset.PageSize4Mib); }
			set { Value = Value.SetBit(Offset.PageSize4Mib, value); }
		}

		public byte Custom
		{
			get { return (byte)Value.GetBits(Offset.Custom, 2); }
			set { Value = Value.SetBits(Offset.Custom, 2, value); }
		}
	}

	[StructLayout(LayoutKind.Explicit)]
	public struct PageTableEntry
	{
		[FieldOffset(0)]
		private uint Value;

		public const byte EntrySize = 4;

		private class Offset
		{
			public const byte Present = 0;
			public const byte Readonly = 1;
			public const byte User = 2;
			public const byte WriteThrough = 3;
			public const byte DisableCache = 4;
			public const byte Accessed = 5;
			public const byte Dirty = 6;
			private const byte SIZE0 = 7;
			public const byte Global = 8;
			public const byte Custom = 9;
			public const byte Address = 11;
		}

		/// <summary>
		/// 4k aligned physical address
		/// </summary>
		public uint PhysicalAddress
		{
			get { return (byte)Value.GetBits(Offset.Address, 20); }
			set
			{
				if (value.GetBits(0, Offset.Address) != 0)
					Panic.Error("Address needs to be 4k aligned");

				Value = Value.SetBits(Offset.Address, 20, value);
			}
		}

		public bool Present
		{
			get { return Value.IsBitSet(Offset.Present); }
			set { Value = Value.SetBit(Offset.Present, value); }
		}

		public bool Readonly
		{
			get { return Value.IsBitSet(Offset.Readonly); }
			set { Value = Value.SetBit(Offset.Readonly, value); }
		}

		public bool User
		{
			get { return Value.IsBitSet(Offset.User); }
			set { Value = Value.SetBit(Offset.User, value); }
		}

		public bool WriteThrough
		{
			get { return Value.IsBitSet(Offset.WriteThrough); }
			set { Value = Value.SetBit(Offset.WriteThrough, value); }
		}

		public bool DisableCache
		{
			get { return Value.IsBitSet(Offset.DisableCache); }
			set { Value = Value.SetBit(Offset.DisableCache, value); }
		}

		public bool Accessed
		{
			get { return Value.IsBitSet(Offset.Accessed); }
			set { Value = Value.SetBit(Offset.Accessed, value); }
		}

		public bool Global
		{
			get { return Value.IsBitSet(Offset.Global); }
			set { Value = Value.SetBit(Offset.Global, value); }
		}

		public bool Dirty
		{
			get { return Value.IsBitSet(Offset.Dirty); }
			set { Value = Value.SetBit(Offset.Dirty, value); }
		}

		public byte Custom
		{
			get { return (byte)Value.GetBits(Offset.Custom, 2); }
			set { Value = Value.SetBits(Offset.Custom, 2, value); }
		}
	}
}