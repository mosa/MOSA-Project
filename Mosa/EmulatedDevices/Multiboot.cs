/*
 * (c) 2008 MOSA - The Managed Operating System Alliance
 *
 * Licensed under the terms of the New BSD License.
 *
 * Authors:
 *  Phil Garcia (tgiphil) <phil@thinkedge.com>
 */

using Mosa.EmulatedKernel;
using Mosa.EmulatedDevices.Emulated;

namespace Mosa.EmulatedDevices
{

	/// <summary>
	/// Emulates the Multiboot environment
	/// </summary>
	public static class Multiboot
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="Multiboot"/> class.
		/// </summary>
		public static void Setup()
		{
			uint mem = 0x1000000;

			uint mem_upper = 0;
			foreach (MemoryHandler segment in MemoryDispatch.MemorySegments)
				if (segment.Address + segment.Size - 1024 * 1024 > mem_upper)
					mem_upper = segment.Address + segment.Size - 1024 * 1024;

			MemoryDispatch.Write32(mem + 0, 0x01 | 0x40);	// flags
			MemoryDispatch.Write32(mem + 4, 640 * 1024);	// mem_lower - assuming at least 640k
			MemoryDispatch.Write32(mem + 8, mem_upper);	// mem_upper
			MemoryDispatch.Write32(mem + 12, 0x0);	// boot_device
			MemoryDispatch.Write32(mem + 16, 0x0);	// cmdline
			MemoryDispatch.Write32(mem + 20, 0x0);	// mods_count
			MemoryDispatch.Write32(mem + 24, 0x0);	// mods_addr
			MemoryDispatch.Write32(mem + 28, 0x0);	// syms
			MemoryDispatch.Write32(mem + 44, 0x0);	// mmap_length
			MemoryDispatch.Write32(mem + 48, 0x0);	// mmap_addr
			MemoryDispatch.Write32(mem + 52, 0x0);	// drives_length
			MemoryDispatch.Write32(mem + 56, 0x0);	// drives_addr
			MemoryDispatch.Write32(mem + 60, 0x0);	// config_table
			MemoryDispatch.Write32(mem + 64, 0x0);	// boot_loader_name
			MemoryDispatch.Write32(mem + 68, 0x0);	// apm_table
			MemoryDispatch.Write32(mem + 72, 0x0);	// vbe_control_info
			MemoryDispatch.Write32(mem + 76, 0x0);	// vbe_mode_info
			MemoryDispatch.Write32(mem + 80, 0x0);	// vbe_mode
			MemoryDispatch.Write32(mem + 84, 0x0);	// vbe_interface_seg
			MemoryDispatch.Write32(mem + 88, 0x0);	// vbe_interface_off
			MemoryDispatch.Write32(mem + 92, 0x0);	// vbe_interface_len

			mem = 96;
			foreach (MemoryHandler segment in MemoryDispatch.MemorySegments) {
				MemoryDispatch.Write32(mem + 0, 0x08);		// Size
				MemoryDispatch.Write32(mem + 4, segment.Address);	// base_addr_low
				MemoryDispatch.Write32(mem + 8, 0x00);	// base_addr_high
				MemoryDispatch.Write32(mem + 12, segment.Size);	// length_low
				MemoryDispatch.Write32(mem + 16, 0x00);	// length_high
				MemoryDispatch.Write32(mem + 20, segment.Type);	// type
				mem = mem + 24;
			}
		}
	}
}
