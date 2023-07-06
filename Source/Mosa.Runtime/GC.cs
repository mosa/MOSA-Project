// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Runtime.CompilerServices;

namespace Mosa.Runtime;

public static class GC
{
	// This method will be plugged by the platform specific implementation;
	// On x86, it is be Mosa.Kernel.x86.KernelMemory._AllocateMemory by the classic Kernel (not BareMetal)
	private static Pointer AllocateMemory(uint size)
	{
		return Pointer.Zero;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Pointer AllocateObject(uint size)
	{
		return AllocateMemory(size);
	}

	public static void Setup()
	{
	}
}
