// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.Compiler.Framework.xUnit;

public class BitValueTests
{
	[Fact]
	public void TestNarrowMax1Then0()
	{
		var bitValue = new BitValue(true);

		bitValue.NarrowMax(1);

		Assert.Equal(bitValue.MaxValue, 1ul);
		Assert.Equal(bitValue.MinValue, 0ul);
		Assert.True(bitValue.IsSignBitClear32);
		Assert.Equal(bitValue.BitsClear, ~(ulong)1);
		Assert.Equal(bitValue.BitsClear32, ~(uint)1);
		Assert.Equal(bitValue.BitsSet, 0ul);
		Assert.True(bitValue.IsZeroOrOne);

		bitValue.NarrowMax(0);

		Assert.Equal(bitValue.MaxValue, 0ul);
		Assert.Equal(bitValue.MinValue, 0ul);
		Assert.True(bitValue.IsSignBitClear32);
		Assert.Equal(bitValue.BitsClear, ulong.MaxValue);
		Assert.Equal(bitValue.BitsClear32, uint.MaxValue);
		Assert.Equal(bitValue.BitsSet, 0ul);
		Assert.True(bitValue.IsZeroOrOne);
		Assert.True(bitValue.IsZero);
		Assert.False(bitValue.IsNotZero);
	}

	[Fact]
	public void TestNarrowMin1()
	{
		var bitValue = new BitValue(true);

		bitValue.NarrowMin(1);

		Assert.Equal(bitValue.MaxValue, uint.MaxValue);
		Assert.Equal(bitValue.MinValue, 1ul);
		Assert.False(bitValue.IsSignBitClear32);
		Assert.True(bitValue.IsNotZero);
		Assert.False(bitValue.IsZero);
		Assert.False(bitValue.IsOne);
		Assert.Equal(bitValue.BitsSet, 0ul);
		Assert.Equal(bitValue.BitsClear, ~(ulong)uint.MaxValue);
	}

	[Fact]
	public void Zero()
	{
		var bitValue = new BitValue(true);

		bitValue.SetValue(0);

		Assert.Equal(bitValue.MaxValue, 0ul);
		Assert.Equal(bitValue.MinValue, 0ul);
		Assert.Equal(bitValue.BitsClear, ulong.MaxValue);
		Assert.Equal(bitValue.BitsClear32, uint.MaxValue);
		Assert.Equal(bitValue.BitsSet, 0ul);
		Assert.True(bitValue.IsZero);
		Assert.True(bitValue.IsSignBitClear32);
		Assert.False(bitValue.IsNotZero);
	}

	[Fact]
	public void One()
	{
		var bitValue = new BitValue(true);

		bitValue.SetValue(1);

		Assert.Equal(bitValue.MaxValue, 1ul);
		Assert.Equal(bitValue.MinValue, 1ul);
		Assert.Equal(bitValue.BitsClear, ~1ul);
		Assert.Equal(bitValue.BitsSet, 1ul);
		Assert.False(bitValue.IsZero);
		Assert.True(bitValue.IsOne);
		Assert.True(bitValue.IsSignBitClear32);
		Assert.True(bitValue.IsNotZero);
	}

	[Fact]
	public void Two()
	{
		var bitValue = new BitValue(true);

		bitValue.SetValue(2);

		Assert.Equal(bitValue.MaxValue, 2ul);
		Assert.Equal(bitValue.MinValue, 2ul);
		Assert.Equal(bitValue.BitsClear, ~2ul);
		Assert.Equal(bitValue.BitsSet, 2ul);
		Assert.False(bitValue.IsZero);
		Assert.False(bitValue.IsOne);
		Assert.True(bitValue.IsSignBitClear32);
		Assert.True(bitValue.IsNotZero);
	}
}
