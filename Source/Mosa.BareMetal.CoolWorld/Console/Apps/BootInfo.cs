// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Text;
using Mosa.Kernel.BareMetal;
using Mosa.Runtime;

namespace Mosa.BareMetal.CoolWorld.Console.Apps;

public class BootInfo : IApp
{
	public string Name => "BootInfo";

	public string Description => "Shows Multiboot v2 information.";

	public void Execute()
	{
		System.Console.WriteLine("Command line           : " + NullTermString(Multiboot.V2.CommandLine));
		System.Console.WriteLine("Bootloader name        : " + NullTermString(Multiboot.V2.BootloaderName));
		System.Console.WriteLine("Memory lower           : " + Multiboot.V2.MemoryLower / 1024 + " MiB");
		System.Console.WriteLine("Memory upper           : " + Multiboot.V2.MemoryUpper / 1024 + " MiB");
		System.Console.WriteLine("Memory map entries     : " + Multiboot.V2.MemoryMapEntries);
		System.Console.WriteLine("Framebuffer available  : " + !Multiboot.V2.FrameBuffer.IsNull);

		if (!Multiboot.V2.FrameBuffer.IsNull)
		{
			System.Console.WriteLine("Framebuffer resolution : " + Multiboot.V2.FrameBufferWidth + "x" + Multiboot.V2.FrameBufferHeight);
		}

		System.Console.WriteLine("RSDPv1 version           : " + !Multiboot.V2.RSDPv1.IsNull);
		System.Console.WriteLine("RSDPv2 version           : " + !Multiboot.V2.RSDPv2.IsNull);
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
