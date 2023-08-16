// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.MultibootSpecification;
using Mosa.Runtime;

namespace Mosa.Kernel.BareMetal;

public static class Multiboot
{
	public static MultibootV2 V2 { get; private set; }

	public static Pointer CommandLinePointer => V2.CommandLinePointer;

	public static void Setup(Pointer location, uint magic)
	{
		V2 = new MultibootV2(magic == MultibootV2.Magic ? location : Pointer.Zero);
	}
}
