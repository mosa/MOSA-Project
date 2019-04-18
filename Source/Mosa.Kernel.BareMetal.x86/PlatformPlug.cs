// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Kernel.BareMetal.BootMemory;
using Mosa.Runtime.Plug;
using Mosa.Runtime.x86;
using System;

namespace Mosa.Kernel.BareMetal.x86
{
	public static class PlatformPlug
	{
		[Plug("Mosa.Kernel.BareMetal.Platform::GetPageShift")]
		public static uint GetPageShift()
		{
			return 12;
		}

		[Plug("Mosa.Kernel.BareMetal.Platform::EntryPoint")]
		public static void EntryPoint()
		{
			var eax = Native.GetMultibootEAX();
			var ebx = Native.GetMultibootEBX();

			Multiboot.Setup(new IntPtr(eax), ebx);

			SSE.Setup();
		}

		[Plug("Mosa.Kernel.BareMetal.Platform::GetMemoryMapLocation")]
		public static IntPtr GetMemoryMapLocation()
		{
			return new IntPtr(0x00007E00);
		}

		[Plug("Mosa.Kernel.BareMetal.Platform::UpdateBootMemoryMap")]
		public static void UpdateBootMemoryMap()
		{
			// Reserve the first 1MB
			BootMemoryMap.SetMemoryMap(new IntPtr(0), 1024 * 1024, BootMemoryMapType.Reserved);
		}
	}
}
