// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.Compiler.Common.xUnit;

public class IntegerTwiddlingTests
{
	[Fact]
	public void IsAddUnsignedCarry()
	{
		Assert.True(IntegerTwiddling.IsAddUnsignedCarry(uint.MaxValue, 1U));
		Assert.True(IntegerTwiddling.IsAddUnsignedCarry(1U, uint.MaxValue));
		Assert.False(IntegerTwiddling.IsAddUnsignedCarry(uint.MaxValue, 0U));
		Assert.False(IntegerTwiddling.IsAddUnsignedCarry(0U, 0U));
		Assert.False(IntegerTwiddling.IsAddUnsignedCarry(0U, uint.MaxValue));

		Assert.True(IntegerTwiddling.IsAddUnsignedCarry(ulong.MaxValue, 1UL));
		Assert.True(IntegerTwiddling.IsAddUnsignedCarry(1UL, ulong.MaxValue));
		Assert.False(IntegerTwiddling.IsAddUnsignedCarry(ulong.MaxValue, 0UL));
		Assert.False(IntegerTwiddling.IsAddUnsignedCarry(0UL, 0UL));
		Assert.False(IntegerTwiddling.IsAddUnsignedCarry(0UL, ulong.MaxValue));
	}

	[Fact]
	public void IsAddSignedOverflow()
	{
		Assert.True(IntegerTwiddling.IsAddSignedOverflow(int.MaxValue, 1));
		Assert.True(IntegerTwiddling.IsAddSignedOverflow(1, int.MaxValue));
		Assert.False(IntegerTwiddling.IsAddSignedOverflow(int.MaxValue, 0));
		Assert.False(IntegerTwiddling.IsAddSignedOverflow(0, 0));
		Assert.False(IntegerTwiddling.IsAddSignedOverflow(0, int.MaxValue));

		Assert.True(IntegerTwiddling.IsAddSignedOverflow(long.MaxValue, 1L));
		Assert.True(IntegerTwiddling.IsAddSignedOverflow(1L, long.MaxValue));
		Assert.False(IntegerTwiddling.IsAddSignedOverflow(long.MaxValue, 0L));
		Assert.False(IntegerTwiddling.IsAddSignedOverflow(0L, 0L));
		Assert.False(IntegerTwiddling.IsAddSignedOverflow(0L, long.MaxValue));
	}

	[Fact]
	public void IsSubSignedOverflow()
	{
		Assert.True(IntegerTwiddling.IsSubSignedOverflow(int.MinValue, -1));
		Assert.False(IntegerTwiddling.IsSubSignedOverflow(int.MinValue, 0));
		Assert.False(IntegerTwiddling.IsSubSignedOverflow(0, 0));
		Assert.False(IntegerTwiddling.IsSubSignedOverflow(0, int.MinValue));

		Assert.True(IntegerTwiddling.IsSubSignedOverflow(long.MinValue, -1L));
		Assert.False(IntegerTwiddling.IsSubSignedOverflow(long.MinValue, 0L));
		Assert.False(IntegerTwiddling.IsSubSignedOverflow(0L, 0L));
		Assert.False(IntegerTwiddling.IsSubSignedOverflow(0L, long.MinValue));
	}

	[Fact]
	public void IsSubUnsignedCarry()
	{
		Assert.True(IntegerTwiddling.IsSubUnsignedCarry(1U, uint.MaxValue));
		Assert.False(IntegerTwiddling.IsSubUnsignedCarry(uint.MaxValue, 0U));
		Assert.False(IntegerTwiddling.IsSubUnsignedCarry(0U, 0U));
		Assert.False(IntegerTwiddling.IsSubUnsignedCarry(uint.MaxValue, 0U));

		Assert.True(IntegerTwiddling.IsSubUnsignedCarry(1UL, ulong.MaxValue));
		Assert.False(IntegerTwiddling.IsSubUnsignedCarry(ulong.MaxValue, 0UL));
		Assert.False(IntegerTwiddling.IsSubUnsignedCarry(0UL, 0UL));
		Assert.False(IntegerTwiddling.IsSubUnsignedCarry(ulong.MaxValue, 0UL));
	}

	[Fact]
	public void IsMultiplyUnsignedCarry()
	{
		Assert.True(IntegerTwiddling.IsMultiplyUnsignedCarry(uint.MaxValue, 2U));
		Assert.True(IntegerTwiddling.IsMultiplyUnsignedCarry(2U, uint.MaxValue));
		Assert.False(IntegerTwiddling.IsMultiplyUnsignedCarry(uint.MaxValue, 1U));
		Assert.False(IntegerTwiddling.IsMultiplyUnsignedCarry(uint.MaxValue, 0U));
		Assert.False(IntegerTwiddling.IsMultiplyUnsignedCarry(0U, 0U));
		Assert.False(IntegerTwiddling.IsMultiplyUnsignedCarry(0U, uint.MaxValue));

		Assert.True(IntegerTwiddling.IsMultiplyUnsignedCarry(ulong.MaxValue, 2UL));
		Assert.True(IntegerTwiddling.IsMultiplyUnsignedCarry(2UL, ulong.MaxValue));
		Assert.False(IntegerTwiddling.IsMultiplyUnsignedCarry(ulong.MaxValue, 1UL));
		Assert.False(IntegerTwiddling.IsMultiplyUnsignedCarry(ulong.MaxValue, 0UL));
		Assert.False(IntegerTwiddling.IsMultiplyUnsignedCarry(0UL, 0UL));
		Assert.False(IntegerTwiddling.IsMultiplyUnsignedCarry(0UL, ulong.MaxValue));
	}

	[Fact]
	public void IsMultiplySignedOverflow()
	{
		Assert.True(IntegerTwiddling.IsMultiplySignedOverflow(int.MinValue, -1));
		Assert.True(IntegerTwiddling.IsMultiplySignedOverflow(int.MaxValue, 2));
		Assert.True(IntegerTwiddling.IsMultiplySignedOverflow(2, int.MaxValue));
		Assert.False(IntegerTwiddling.IsMultiplySignedOverflow(int.MaxValue, 1));
		Assert.False(IntegerTwiddling.IsMultiplySignedOverflow(int.MaxValue, 0));
		Assert.False(IntegerTwiddling.IsMultiplySignedOverflow(0, 0));
		Assert.False(IntegerTwiddling.IsMultiplySignedOverflow(0, int.MaxValue));

		Assert.True(IntegerTwiddling.IsMultiplySignedOverflow(long.MinValue, -1L));
		Assert.True(IntegerTwiddling.IsMultiplySignedOverflow(long.MaxValue, 2L));
		Assert.True(IntegerTwiddling.IsMultiplySignedOverflow(2L, long.MaxValue));
		Assert.False(IntegerTwiddling.IsMultiplySignedOverflow(long.MaxValue, 1L));
		Assert.False(IntegerTwiddling.IsMultiplySignedOverflow(long.MaxValue, 0L));
		Assert.False(IntegerTwiddling.IsMultiplySignedOverflow(0L, 0L));
		Assert.False(IntegerTwiddling.IsMultiplySignedOverflow(0L, long.MaxValue));
	}

	[Fact]
	public void HasSignBitSet()
	{
		Assert.True(IntegerTwiddling.HasSignBitSet(-255));
		Assert.True(IntegerTwiddling.HasSignBitSet(-74784778L));
		Assert.False(IntegerTwiddling.HasSignBitSet(741));
		Assert.False(IntegerTwiddling.HasSignBitSet(4741875L));
		Assert.False(IntegerTwiddling.HasSignBitSet(0));
		Assert.False(IntegerTwiddling.HasSignBitSet(0L));
	}
}
