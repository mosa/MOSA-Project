// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Text;
using Mosa.Kernel.BareMetal;
using Mosa.Runtime;

namespace Mosa.BareMetal.HelloWorld.Apps;

public class BootInfo : IApp
{
	public string Name => "BootInfo";

	public string Description => "Shows Multiboot v2 information.";

	public void Execute()
	{
		Console.WriteLine("Command line           : " + NullTermString(Multiboot.V2.CommandLinePointer));
		Console.WriteLine("Bootloader name        : " + NullTermString(Multiboot.V2.BootloaderNamePointer));
		Console.WriteLine("Memory lower           : " + Multiboot.V2.MemoryLower / 1024 + " MiB");
		Console.WriteLine("Memory upper           : " + Multiboot.V2.MemoryUpper / 1024 + " MiB");
		Console.WriteLine("Memory map entries     : " + Multiboot.V2.Entries);
		Console.WriteLine("Framebuffer available  : " + Multiboot.V2.FramebufferAvailable);
		if (Multiboot.V2.FramebufferAvailable) Console.WriteLine("Framebuffer resolution : " + Multiboot.V2.Framebuffer.Width + "x" + Multiboot.V2.Framebuffer.Height);
		Console.WriteLine("ACPI version           : " + (Multiboot.V2.ACPIv2 ? "2" : "1"));
	}

	private readonly StringBuilder Builder = new();

	private string NullTermString(Pointer pointer)
	{
		Builder.Clear();

		var i = 0;
		byte b;

		do
		{
			b = pointer.Load8(i++);
			Builder.Append((char)b);
		} while (b != 0);

		return Builder.ToString();
	}
}
