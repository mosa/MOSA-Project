// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.DeviceSystem;
using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal.MultibootSpecification;

public sealed class MultibootV2
{
	public const uint Magic = 0x36D76289;

	public readonly Pointer CommandLinePointer;
	public readonly Pointer BootloaderNamePointer;
	public readonly uint MemoryLower;
	public readonly uint MemoryUpper;
	public readonly uint EntrySize;
	public readonly uint EntryVersion;
	public readonly uint Entries;
	public readonly MultibootV2MemoryMapEntry FirstEntry;
	public readonly bool FramebufferAvailable;
	public readonly FrameBuffer32 Framebuffer;
	public readonly bool ACPIv2;
	public readonly Pointer RSDP;

	public MultibootV2(Pointer entry)
	{
		if (entry.IsNull) Environment.FailFast("Multiboot v2 is not available!");

		var pointer = entry + 8;

		uint type;
		while ((type = pointer.Load32(0)) != 0)
		{
			var size = pointer.Load32(4);

			switch (type)
			{
				case 1:
				{
					CommandLinePointer = pointer + 8;
					break;
				}
				case 2:
				{
					BootloaderNamePointer = pointer + 8;
					break;
				}
				case 4:
				{
					MemoryLower = pointer.Load32(8);
					MemoryUpper = pointer.Load32(12);
					break;
				}
				case 6:
				{
					EntrySize = pointer.Load32(8);
					EntryVersion = pointer.Load32(12);
					Entries = (size - 16) / EntrySize;
					FirstEntry = new MultibootV2MemoryMapEntry(pointer + 16);
					break;
				}
				case 8:
				{
					var address = (Pointer)pointer.Load64(8);
					var pitch = pointer.Load32(16);
					var width = pointer.Load32(20);
					var height = pointer.Load32(24);
					var bpp = pointer.Load8(28);
					var fbType = pointer.Load8(29);

					FramebufferAvailable = bpp == 32 && fbType <= 1;
					Framebuffer = new FrameBuffer32(
						new ConstrainedPointer(address, width * height * 4),
						width,
						height,
						(x, y) => y * pitch + x * 4
					);

					break;
				}
				case 13:
				{
					// TODO: SMBIOS
					break;
				}
				case 14:
				{
					ACPIv2 = false;
					RSDP = pointer + 8;
					break;
				}
				case 15:
				{
					ACPIv2 = true;
					RSDP = pointer + 8;
					break;
				}
			}

			pointer += (size + 7) & ~7;
		}
	}
}
