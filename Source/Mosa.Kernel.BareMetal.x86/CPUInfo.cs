// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using Mosa.Runtime.x86;

namespace Mosa.Kernel.BareMetal.x86;

public static class CPUInfo
{
	public static uint NumberOfCores => (Native.CpuIdEAX(4, 0) >> 26) + 1;

	public static uint Type => (Native.CpuIdEAX(1, 0) & 0x3000) >> 12;

	public static uint Stepping => Native.CpuIdEAX(1, 0) & 0xF;

	public static uint Model => (Native.CpuIdEAX(1, 0) & 0xF0) >> 4;

	public static uint Family => (Native.CpuIdEAX(1, 0) & 0xF00) >> 8;

	public static bool SupportsExtendedCpuid => (Native.CpuIdEAX(0x80000000, 0) & 0x80000000) != 0;

	public static bool SupportsBrandString => Native.CpuIdEAX(0x80000000, 0) >= 0x80000004U;

	public static void PrintVendorString()
	{
		var identifier = Native.CpuIdEBX(0, 0);
		for (var i = 0; i < 4; ++i)
			Console.Write((char)((identifier >> (i * 8)) & 0xFF));

		identifier = Native.CpuIdEDX(0, 0);
		for (var i = 0; i < 4; ++i)
			Console.Write((char)((identifier >> (i * 8)) & 0xFF));

		identifier = Native.CpuIdECX(0, 0);
		for (var i = 0; i < 4; ++i)
			Console.Write((char)((identifier >> (i * 8)) & 0xFF));
	}

	public static void PrintBrandString()
	{
		if (SupportsBrandString)
		{
			PrintBrand(0x80000002);
			PrintBrand(0x80000003);
			PrintBrand(0x80000004);
			return;
		}

		Console.Write(@"Unknown (Generic x86)");
	}

	private static void PrintBrand(uint param)
	{
		var whitespace = true;

		var identifier = Native.CpuIdEAX(param, 0);
		if (identifier != 0x20202020)
			for (var i = 0; i < 4; ++i)
				PrintBrandPart(identifier, i, ref whitespace);

		identifier = Native.CpuIdEBX(param, 0);
		if (identifier != 0x20202020)
			for (var i = 0; i < 4; ++i)
				PrintBrandPart(identifier, i, ref whitespace);

		identifier = Native.CpuIdECX(param, 0);
		if (identifier != 0x20202020)
			for (var i = 0; i < 4; ++i)
				PrintBrandPart(identifier, i, ref whitespace);

		identifier = Native.CpuIdEDX(param, 0);
		if (identifier != 0x20202020)
			for (var i = 0; i < 4; ++i)
				PrintBrandPart(identifier, i, ref whitespace);
	}

	private static void PrintBrandPart(uint identifier, int i, ref bool whitespace)
	{
		var character = (char)((identifier >> (i * 8)) & 0xFF);

		switch (whitespace)
		{
			case true when character == ' ': return;
			case true when character != ' ':
			{
				Console.Write(character);
				whitespace = false;
				return;
			}
		}

		if (character == ' ') whitespace = true;

		Console.Write(character);
	}
}
