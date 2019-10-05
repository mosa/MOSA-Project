// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Mosa.TestWorld.x86
{
	internal class Test
	{
		public static ulong KernelBootStartCycles { get; private set; }

		public static void Setup()
		{
			//KernelBootStartCycles = CpuCyclesSinceSystemBoot();
		}

		public static void Setup(ulong kernelBootStartCycles)
		{
			// It will generate invalid opcode!!
			KernelBootStartCycles = kernelBootStartCycles;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public static ulong CpuCyclesSinceSystemBoot()
		{
			return 0;
		}

		public static ulong CpuCyclesSinceKernelBoot()
		{
			//return CpuCyclesSinceSystemBoot() - KernelBootStartCycles;
			return CpuCyclesSinceSystemBoot() - 12500000000L;
		}
	}
}
