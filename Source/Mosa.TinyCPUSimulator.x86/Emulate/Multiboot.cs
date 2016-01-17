// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.TinyCPUSimulator.x86.Emulate
{
	public class Multiboot : BaseSimDevice
	{
		public static readonly uint Magic = 0x2BADB002;
		public static readonly uint MultibootStructure = 0x10090; // same as QEMU

		public Multiboot(SimCPU simCPU)
			: base(simCPU)
		{
		}

		public override BaseSimDevice Clone(SimCPU simCPU)
		{
			return new Multiboot(simCPU);
		}

		public override void Initialize()
		{
			simCPU.AddMemory(0x00200000, 0x08, 2);
		}

		public override void Reset()
		{
			var x86 = simCPU as CPUx86;

			x86.EAX.Value = Magic;
			x86.EBX.Value = MultibootStructure;

			x86.CR0.Paging = false;
			x86.CR0.ProtectedModeEnable = true;

			simCPU.Write32(0x200000, Magic);
			simCPU.Write32(0x200004, MultibootStructure);

			uint multiboot = MultibootStructure;

			uint mem_upper = 0;

			foreach (var region in simCPU.MemoryRegions)
			{
				if (region.Type == 1 && region.End > mem_upper)
				{
					mem_upper = (uint)region.End;
				}
			}

			mem_upper = mem_upper - (1024 * 1024);

			simCPU.Write32(multiboot + 0, 0x01 | 0x40); // flags
			simCPU.Write32(multiboot + 4, 640);     // mem_lower - assuming at least 640k
			simCPU.Write32(multiboot + 8, mem_upper / 1024);    // mem_upper
			simCPU.Write32(multiboot + 12, 0x0);    // boot_device
			simCPU.Write32(multiboot + 16, 0x0);    // cmdline
			simCPU.Write32(multiboot + 20, 0x0);    // mods_count
			simCPU.Write32(multiboot + 24, 0x0);    // mods_addr
			simCPU.Write32(multiboot + 28, 0x0);    // syms
			simCPU.Write32(multiboot + 44, 1 * 24); // mmap_length
			simCPU.Write32(multiboot + 48, multiboot + 96); // mmap_addr
			simCPU.Write32(multiboot + 52, 0x0);    // drives_length
			simCPU.Write32(multiboot + 56, 0x0);    // drives_addr
			simCPU.Write32(multiboot + 60, 0x0);    // config_table
			simCPU.Write32(multiboot + 64, 0x0);    // boot_loader_name
			simCPU.Write32(multiboot + 68, 0x0);    // apm_table
			simCPU.Write32(multiboot + 72, 0x0);    // vbe_control_info
			simCPU.Write32(multiboot + 76, 0x0);    // vbe_mode_info
			simCPU.Write32(multiboot + 80, 0x0);    // vbe_mode
			simCPU.Write32(multiboot + 84, 0x0);    // vbe_interface_seg
			simCPU.Write32(multiboot + 88, 0x0);    // vbe_interface_off
			simCPU.Write32(multiboot + 92, 0x0);    // vbe_interface_len

			multiboot = multiboot + 96;

			foreach (var region in simCPU.MemoryRegions)
			{
				simCPU.Write32(multiboot + 0, 20);      // Size
				simCPU.Write32(multiboot + 4, (uint)region.Address);    // base_addr_low
				simCPU.Write32(multiboot + 8, 0x00);    // base_addr_high
				simCPU.Write32(multiboot + 12, (uint)region.Size);  // length_low
				simCPU.Write32(multiboot + 16, 0x00);   // length_high
				simCPU.Write32(multiboot + 20, region.Type);  // type
				multiboot = multiboot + 24;
			}
		}
	}
}
