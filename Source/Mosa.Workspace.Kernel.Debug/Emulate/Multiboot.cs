// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Workspace.Kernel.Internal;

namespace Mosa.Workspace.Kernel.Emulate;

public class Multiboot
{
	public static readonly uint Magic = 0x2BADB002;
	public static readonly uint MultibootStructure = 0x10090; // same as QEMU

	public static void Setup(uint totalMemory)
	{
		CPU.Write32(0x200000, Magic);
		CPU.Write32(0x200004, MultibootStructure);

		uint multiboot = MultibootStructure;

		CPU.Write32(multiboot + 0, 0x01 | 0x40); // flags

		CPU.Write32(multiboot + 4, 640);     // mem_lower - assuming at least 640k (in kilobytes)
		CPU.Write32(multiboot + 8, (totalMemory - (1024 * 1024)) / 1024);    // mem_upper (in kilobytes)
		CPU.Write32(multiboot + 12, 0x0);    // boot_device
		CPU.Write32(multiboot + 16, 0x0);    // cmdline
		CPU.Write32(multiboot + 20, 0x0);    // mods_count
		CPU.Write32(multiboot + 24, 0x0);    // mods_addr
		CPU.Write32(multiboot + 28, 0x0);    // syms
		CPU.Write32(multiboot + 44, 6 * 24); // mmap_length
		CPU.Write32(multiboot + 48, multiboot + 96); // mmap_addr
		CPU.Write32(multiboot + 52, 0x0);    // drives_length
		CPU.Write32(multiboot + 56, 0x0);    // drives_addr
		CPU.Write32(multiboot + 60, 0x0);    // config_table
		CPU.Write32(multiboot + 64, 0x0);    // boot_loader_name
		CPU.Write32(multiboot + 68, 0x0);    // apm_table
		CPU.Write32(multiboot + 72, 0x0);    // vbe_control_info
		CPU.Write32(multiboot + 76, 0x0);    // vbe_mode_info
		CPU.Write32(multiboot + 80, 0x0);    // vbe_mode
		CPU.Write32(multiboot + 84, 0x0);    // vbe_interface_seg
		CPU.Write32(multiboot + 88, 0x0);    // vbe_interface_off
		CPU.Write32(multiboot + 92, 0x0);    // vbe_interface_len

		multiboot += 96;

		multiboot = SetMemoryRegion(multiboot, 0x00000000, 0x009FC00, 1);
		multiboot = SetMemoryRegion(multiboot, 0x0009FC00, 0x0000400, 2);
		multiboot = SetMemoryRegion(multiboot, 0x000F0000, 0x0010000, 2);
		multiboot = SetMemoryRegion(multiboot, 0x00100000, 0x7EE0000, 1);   // change - 0x7EE0000 to (total memory - starting address)
		multiboot = SetMemoryRegion(multiboot, 0x07FE0000, 0x0020000, 2);
		multiboot = SetMemoryRegion(multiboot, 0xFFFC0000, 0x0020000, 2);
	}

	private static uint SetMemoryRegion(uint at, uint address, uint size, uint type)
	{
		CPU.Write32(at + 0, 20);      // Size
		CPU.Write32(at + 4, address); // base_addr_low
		CPU.Write32(at + 8, 0x00);    // base_addr_high
		CPU.Write32(at + 12, size);   // length_low
		CPU.Write32(at + 16, 0x00);   // length_high
		CPU.Write32(at + 20, type);   // type
		return at + 24;
	}
}
