// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.x86;

namespace Mosa.Kernel.x86
{
	public static class MSR
	{
		public const uint CPUID_FLAG_MSR = 1 << 5;

		public static bool HasMSR()
		{
			uint eax = Native.CpuIdEax(1);
			uint edx = Native.CpuIdEdx(1);

			return (edx & CPUID_FLAG_MSR) != 0;
		}

		public static ulong GetMSR(uint msr)
		{
			return Native.RdMSR(msr);
		}

		public static void SetMSR(uint msr, ulong value)
		{
			Native.WrMSR(msr, value);
		}
	}
}