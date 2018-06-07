// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Runtime.CompilerServices;

namespace Mosa.Runtime
{
	public static class GC
	{
		// This method will be plugged by the platform specific implementation;
		// On x86, it is be Mosa.Kernel.x86.KernelMemory._AllocateMemory
		private unsafe static IntPtr AllocateMemory(uint size)
		{
			return IntPtr.Zero;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public unsafe static IntPtr AllocateObject(uint size)
		{
			return AllocateMemory(size);
		}

		public static void Setup()
		{
		}
	}
}
