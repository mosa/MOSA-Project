// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Xunit;

namespace Mosa.Compiler.Framework.xUnit;

public class BitValueTests
{
	[Fact]
	public void TestNarrowMax1Then0()
	{
		var bitValue = new BitValue(true);

		bitValue.NarrowMax(1);

		Debug.Assert(bitValue.MaxValue == 1);
		Debug.Assert(bitValue.MinValue == 0);
		Debug.Assert(bitValue.IsSignBitClear32);
		Debug.Assert(bitValue.BitsClear == ~(ulong)1);
		Debug.Assert(bitValue.BitsClear32 == ~(uint)1);
		Debug.Assert(bitValue.BitsSet == 0);
		Debug.Assert(bitValue.IsZeroOrOne);
		//Debug.Assert(bitValue.IsNotZero);

		bitValue.NarrowMax(0);

		Debug.Assert(bitValue.MaxValue == 0);
		Debug.Assert(bitValue.MinValue == 0);
		Debug.Assert(bitValue.IsSignBitClear32);
		Debug.Assert(bitValue.BitsClear == ulong.MaxValue);
		Debug.Assert(bitValue.BitsClear32 == uint.MaxValue);
		Debug.Assert(bitValue.BitsSet == 0);
		Debug.Assert(bitValue.IsZeroOrOne);
		Debug.Assert(bitValue.IsZero);
		Debug.Assert(!bitValue.IsNotZero);
	}

	[Fact]
	public void TestNarrowMin1()
	{
		var bitValue = new BitValue(true);

		bitValue.NarrowMin(1);

		Debug.Assert(bitValue.MaxValue == uint.MaxValue);
		Debug.Assert(bitValue.MinValue == 1);
		Debug.Assert(!bitValue.IsSignBitClear32);
		Debug.Assert(bitValue.IsNotZero);
		Debug.Assert(!bitValue.IsZero);
		Debug.Assert(!bitValue.IsOne);
		Debug.Assert(bitValue.BitsSet == 0);
		Debug.Assert(bitValue.BitsClear == ~(ulong)uint.MaxValue);
	}

	[Fact]
	public void Zero()
	{
		var bitValue = new BitValue(true);

		bitValue.SetValue(0);

		Debug.Assert(bitValue.MaxValue == 0);
		Debug.Assert(bitValue.MinValue == 0);
		Debug.Assert(bitValue.BitsClear == ulong.MaxValue);
		Debug.Assert(bitValue.BitsClear32 == uint.MaxValue);
		Debug.Assert(bitValue.BitsSet == 0);
		Debug.Assert(bitValue.IsZero);
		Debug.Assert(bitValue.IsSignBitClear32);
		Debug.Assert(!bitValue.IsNotZero);
	}

	[Fact]
	public void One()
	{
		var bitValue = new BitValue(true);

		bitValue.SetValue(1);

		Debug.Assert(bitValue.MaxValue == 1);
		Debug.Assert(bitValue.MinValue == 1);
		Debug.Assert(bitValue.BitsClear == ~1ul);
		Debug.Assert(bitValue.BitsSet == 1);
		Debug.Assert(!bitValue.IsZero);
		Debug.Assert(bitValue.IsOne);
		Debug.Assert(bitValue.IsSignBitClear32);
		Debug.Assert(bitValue.IsNotZero);
	}

	[Fact]
	public void Two()
	{
		var bitValue = new BitValue(true);

		bitValue.SetValue(2);

		Debug.Assert(bitValue.MaxValue == 2);
		Debug.Assert(bitValue.MinValue == 2);
		Debug.Assert(bitValue.BitsClear == ~2ul);
		Debug.Assert(bitValue.BitsSet == 2);
		Debug.Assert(!bitValue.IsZero);
		Debug.Assert(!bitValue.IsOne);
		Debug.Assert(bitValue.IsSignBitClear32);
		Debug.Assert(bitValue.IsNotZero);
	}
}
