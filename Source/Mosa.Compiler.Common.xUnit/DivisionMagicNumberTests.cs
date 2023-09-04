// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Xunit;

namespace Mosa.Compiler.Common.xUnit;

public class DivisionMagicNumberTests
{
	[Fact]
	public static void SignedMagic_3()
	{
		var result = DivisionMagicNumber.GetMagicNumber(3);
		Debug.Assert(result.M + result.s == 0x55555556 + 0);
	}

	[Fact]
	public static void SignedMagic_7()
	{
		var result = DivisionMagicNumber.GetMagicNumber(7);
		Debug.Assert(result.M + result.s == 0x92492493 + 2);
	}

	[Fact]
	public static void UnsignedMagic_3()
	{
		var result = DivisionMagicNumber.GetMagicNumber(3u);
		Debug.Assert(result.M + result.a + result.s == 0xAAAAAAAB + 0 + 1);
	}

	[Fact]
	public static void UnsignedMagic_7()
	{
		var result = DivisionMagicNumber.GetMagicNumber(7u);
		Debug.Assert(result.M + result.a + result.s == 0x24924925 + 1 + 3);
	}

	[Fact]
	public static void UnsignedMagic2_3()
	{
		var result = DivisionMagicNumber.GetMagicNumber(3u);
		Debug.Assert(result.M + result.a + result.s == 0xAAAAAAAB + 0 + 1);
	}

	[Fact]
	public static void UnsignedMagic2_7()
	{
		var result = DivisionMagicNumber.GetMagicNumber(7u);
		Debug.Assert(result.M + result.a + result.s == 0x24924925 + 1 + 3);
	}
}
