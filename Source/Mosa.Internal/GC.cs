// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Internal
{
	public static class GC
	{
		// This method will be plugged by the platform specific implementation;
		// On x86, it is be Mosa.Kernel.x86.KernelMemory._AllocateMemory
		private unsafe static void* AllocateMemory(uint size)
		{
			return (void*)0;
		}

		public unsafe static void* AllocateObject(uint size)
		{
			return AllocateMemory(size);
		}

		public static void Setup()
		{
		}
	}
}
