// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.Compiler.Framework.xUnit;

public class BitValueTests
{
	[Fact]
	public void NarrowMax1Then0()
	{
		var bitValue = new BitValue(true);

		bitValue.NarrowMax(1);

		Assert.Equal(1ul, bitValue.MaxValue);
		Assert.Equal(0ul, bitValue.MinValue);
		Assert.True(bitValue.IsSignBitClear32);
		Assert.Equal(~(ulong)1, bitValue.BitsClear);
		Assert.Equal(~(uint)1, bitValue.BitsClear32);
		Assert.Equal(0ul, bitValue.BitsSet);
		Assert.True(bitValue.IsZeroOrOne);

		bitValue.NarrowMax(0);

		Assert.Equal(0ul, bitValue.MaxValue);
		Assert.Equal(0ul, bitValue.MinValue);
		Assert.True(bitValue.IsSignBitClear32);
		Assert.Equal(ulong.MaxValue, bitValue.BitsClear);
		Assert.Equal(uint.MaxValue, bitValue.BitsClear32);
		Assert.Equal(0ul, bitValue.BitsSet);
		Assert.True(bitValue.IsZeroOrOne);
		Assert.True(bitValue.IsZero);
		Assert.False(bitValue.IsNotZero);
	}

	[Fact]
	public void NarrowMin1()
	{
		var bitValue = new BitValue(true);

		bitValue.NarrowMin(1);

		Assert.Equal(uint.MaxValue, bitValue.MaxValue);
		Assert.Equal(1ul, bitValue.MinValue);
		Assert.False(bitValue.IsSignBitClear32);
		Assert.True(bitValue.IsNotZero);
		Assert.False(bitValue.IsZero);
		Assert.False(bitValue.IsOne);
		Assert.Equal(0ul, bitValue.BitsSet);
		Assert.Equal(~(ulong)uint.MaxValue, bitValue.BitsClear);
	}

	[Fact]
	public void Zero()
	{
		var bitValue = new BitValue(true);

		bitValue.SetValue(0);

		Assert.Equal(0ul, bitValue.MaxValue);
		Assert.Equal(0ul, bitValue.MinValue);
		Assert.Equal(ulong.MaxValue, bitValue.BitsClear);
		Assert.Equal(uint.MaxValue, bitValue.BitsClear32);
		Assert.Equal(0ul, bitValue.BitsSet);
		Assert.True(bitValue.IsZero);
		Assert.True(bitValue.IsSignBitClear32);
		Assert.False(bitValue.IsNotZero);
	}

	[Fact]
	public void One()
	{
		var bitValue = new BitValue(true);

		bitValue.SetValue(1);

		Assert.Equal(1ul, bitValue.MaxValue);
		Assert.Equal(1ul, bitValue.MinValue);
		Assert.Equal(~1ul, bitValue.BitsClear);
		Assert.Equal(1ul, bitValue.BitsSet);
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

		Assert.Equal(2ul, bitValue.MaxValue);
		Assert.Equal(2ul, bitValue.MinValue);
		Assert.Equal(~2ul, bitValue.BitsClear);
		Assert.Equal(2ul, bitValue.BitsSet);
		Assert.False(bitValue.IsZero);
		Assert.False(bitValue.IsOne);
		Assert.True(bitValue.IsSignBitClear32);
		Assert.True(bitValue.IsNotZero);
	}

	[Fact]
	public void Xor1()
	{
		var operand1 = new BitValue(true).NarrowClearBits(0b1111111);
		var operand2 = new BitValue(true, 77892622);

		var bitsKnown = operand1.BitsKnown & operand2.BitsKnown & uint.MaxValue;

		var xor = (operand1.BitsSet ^ operand2.BitsSet) & bitsKnown;

		var result = new BitValue(true)
			.NarrowSetBits(xor)
			.NarrowClearBits((~xor) & bitsKnown)
			.SetStable(operand1, operand2);

		Assert.Equal((ulong)0b0001110, result.BitsSet);
	}

	[Fact]
	public void Xor2()
	{
		var operand1 = new BitValue(true).NarrowClearBits(0b1111111);
		var operand2 = new BitValue(true, 77892622);

		var result = new BitValue(true)
			.NarrowBits(operand1.BitsSet ^ operand2.BitsSet, operand1.BitsKnown & operand2.BitsKnown);

		Assert.Equal((ulong)0b0001110, result.BitsSet);
	}
}
