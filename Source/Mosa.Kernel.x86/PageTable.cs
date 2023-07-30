// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime;
using Mosa.Runtime.x86;

namespace Mosa.Kernel.x86;

/// <summary>
/// Page Table
/// </summary>
public static class PageTable
{
	private static class Constant
	{
		public const uint Present = 1 << 0;
		public const uint ReadWrite = 1 << 1;
		public const uint UserSupervisor = 1 << 2;
		public const uint WriteThrough = 1 << 3;
		public const uint CacheDisable = 1 << 4;
		public const uint Accessed = 1 << 5;
		public const uint Dirty = 1 << 6;
	}

	public static void Setup()
	{
		// Setup Page Directory
		for (int index = 0; index < 1024; index++)
		{
			new Pointer(Address.PageDirectory).Store32(index << 2, (uint)(Address.PageTable + index * 4096 | Constant.UserSupervisor | Constant.ReadWrite | Constant.Present));
		}

		// Map the first 128MB of memory (32786 4K pages) (why 128MB?)
		for (int index = 0; index < 1024 * 32; index++)
		{
			new Pointer(Address.PageTable).Store32(index << 2, (uint)(index * 4096) | 0x04 | 0x02 | 0x01);
		}

		// Unmap the first page for null pointer exceptions
		MapVirtualAddressToPhysical(0x0, 0x0, false);

		// Set CR3 register on processor - sets page directory
		Native.SetCR3(Address.PageDirectory);

		// Set CR0 register on processor - turns on virtual memory
		Native.SetCR0(Native.GetCR0() | 0x80000000);
	}

	/// <summary>
	/// Maps the virtual address to physical.
	/// </summary>
	/// <param name="virtualAddress">The virtual address.</param>
	/// <param name="physicalAddress">The physical address.</param>
	public static void MapVirtualAddressToPhysical(uint virtualAddress, uint physicalAddress, bool present = true)
	{
		//FUTURE: traverse page directory from CR3 --- do not assume page table is linearly allocated

		//Native.Set32(Address.PageTable + ((virtualAddress & 0xFFC00000u) >> 10), physicalAddress & 0xFFC00000u | 0x04u | 0x02u | (present ? 0x1u : 0x0u));
		new Pointer(Address.PageTable).Store32((virtualAddress & 0xFFFFF000u) >> 10, physicalAddress & 0xFFFFF000u | 0x04u | 0x02u | (present ? 0x1u : 0x0u));
	}

	/// <summary>
	/// Gets the physical memory.
	/// </summary>
	/// <param name="virtualAddress">The virtual address.</param>
	/// <returns></returns>
	public static Pointer GetPhysicalAddressFromVirtual(Pointer virtualAddress)
	{
		//FUTURE: traverse page directory from CR3 --- do not assume page table is linearly allocated
		var offset = (((uint)virtualAddress.ToInt32() & 0xFFFFF000u) >> 10) + ((uint)virtualAddress.ToInt32() & 0xFFFu);

		return Intrinsic.LoadPointer(new Pointer(Address.PageTable), offset);
	}
}
