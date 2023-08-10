// Copyright (c) MOSA Project. Licensed under the New BSD License.

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

	public static string GetVendorString()
	{
		var vendor = string.Empty;

		var identifier = Native.CpuIdEBX(0, 0);
		for (var i = 0; i < 4; ++i)
			vendor += (char)((identifier >> (i * 8)) & 0xFF);

		identifier = Native.CpuIdEDX(0, 0);
		for (var i = 0; i < 4; ++i)
			vendor += (char)((identifier >> (i * 8)) & 0xFF);

		identifier = Native.CpuIdECX(0, 0);
		for (var i = 0; i < 4; ++i)
			vendor += (char)((identifier >> (i * 8)) & 0xFF);

		return vendor;
	}

	public static string GetBrandString()
	{
		if (!SupportsBrandString) return "Unknown (Generic x86)";

		var brand = PrintBrand(0x80000002);
		brand += PrintBrand(0x80000003);
		brand += PrintBrand(0x80000004);
		return brand;
	}

	private static string PrintBrand(uint param)
	{
		var whitespace = true;
		var brand = string.Empty;

		var identifier = Native.CpuIdEAX(param, 0);
		if (identifier != 0x20202020)
			for (var i = 0; i < 4; ++i)
				brand += PrintBrandPart(identifier, i, ref whitespace);

		identifier = Native.CpuIdEBX(param, 0);
		if (identifier != 0x20202020)
			for (var i = 0; i < 4; ++i)
				brand += PrintBrandPart(identifier, i, ref whitespace);

		identifier = Native.CpuIdECX(param, 0);
		if (identifier != 0x20202020)
			for (var i = 0; i < 4; ++i)
				brand += PrintBrandPart(identifier, i, ref whitespace);

		identifier = Native.CpuIdEDX(param, 0);
		if (identifier != 0x20202020)
			for (var i = 0; i < 4; ++i)
				brand += PrintBrandPart(identifier, i, ref whitespace);

		return brand;
	}

	private static string PrintBrandPart(uint identifier, int i, ref bool whitespace)
	{
		var character = (char)((identifier >> (i * 8)) & 0xFF);

		switch (whitespace)
		{
			case true when character == ' ': return string.Empty;
			case true when character != ' ':
			{
				whitespace = false;
				return character.ToString();
			}
		}

		if (character == ' ') whitespace = true;

		return character.ToString();
	}
}
