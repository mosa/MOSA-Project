// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Common;

public static class IntegerExtension
{
	public static string ToHex(this int i)
	{
		return $"0x{i:X}";
	}

	public static string ToHex(this long i)
	{
		return $"0x{i:X}";
	}

	public static string ToHex(this uint i)
	{
		return $"0x{i:X}";
	}

	public static string ToHex(this ulong i)
	{
		return $"0x{i:X}";
	}
}
