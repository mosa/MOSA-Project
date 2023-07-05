// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Runtime.x86;

namespace Mosa.Kernel.BareMetal.x86;

public static class MSR
{
	public const uint CPUID_FLAG_MSR = 1 << 5;

	public static bool HasMSR()
	{
		uint eax = Native.CpuIdEAX(1, 0);
		uint edx = Native.CpuIdEDX(1, 0);

		return (edx & CPUID_FLAG_MSR) != 0;
	}

	public static ulong GetMSR(uint msr)
	{
		return Native.RdMSR(msr);
	}

	public static void SetMSR(uint msr, uint eax, uint edx)
	{
		Native.WrMSR(msr, eax, edx);
	}
}
