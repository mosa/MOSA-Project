// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Xunit;

namespace Mosa.Compiler.Common.xUnit;

public class IntegerTwiddlingTests
{
	[Fact]
	public void IsAddUnsignedCarry()
	{
		Debug.Assert(IntegerTwiddling.IsAddUnsignedCarry(uint.MaxValue, 1U));
		Debug.Assert(IntegerTwiddling.IsAddUnsignedCarry(1U, uint.MaxValue));
		Debug.Assert(!IntegerTwiddling.IsAddUnsignedCarry(uint.MaxValue, 0U));
		Debug.Assert(!IntegerTwiddling.IsAddUnsignedCarry(0U, 0U));
		Debug.Assert(!IntegerTwiddling.IsAddUnsignedCarry(0U, uint.MaxValue));

		Debug.Assert(IntegerTwiddling.IsAddUnsignedCarry(ulong.MaxValue, 1UL));
		Debug.Assert(IntegerTwiddling.IsAddUnsignedCarry(1UL, ulong.MaxValue));
		Debug.Assert(!IntegerTwiddling.IsAddUnsignedCarry(ulong.MaxValue, 0UL));
		Debug.Assert(!IntegerTwiddling.IsAddUnsignedCarry(0UL, 0UL));
		Debug.Assert(!IntegerTwiddling.IsAddUnsignedCarry(0UL, ulong.MaxValue));
	}

	[Fact]
	public void IsAddSignedOverflow()
	{
		Debug.Assert(IntegerTwiddling.IsAddSignedOverflow(int.MaxValue, 1));
		Debug.Assert(IntegerTwiddling.IsAddSignedOverflow(1, int.MaxValue));
		Debug.Assert(!IntegerTwiddling.IsAddSignedOverflow(int.MaxValue, 0));
		Debug.Assert(!IntegerTwiddling.IsAddSignedOverflow(0, 0));
		Debug.Assert(!IntegerTwiddling.IsAddSignedOverflow(0, int.MaxValue));

		Debug.Assert(IntegerTwiddling.IsAddSignedOverflow(long.MaxValue, 1L));
		Debug.Assert(IntegerTwiddling.IsAddSignedOverflow(1L, long.MaxValue));
		Debug.Assert(!IntegerTwiddling.IsAddSignedOverflow(long.MaxValue, 0L));
		Debug.Assert(!IntegerTwiddling.IsAddSignedOverflow(0L, 0L));
		Debug.Assert(!IntegerTwiddling.IsAddSignedOverflow(0L, long.MaxValue));
	}

	[Fact]
	public void IsSubSignedOverflow()
	{
		Debug.Assert(IntegerTwiddling.IsSubSignedOverflow(int.MinValue, -1));
		Debug.Assert(!IntegerTwiddling.IsSubSignedOverflow(int.MinValue, 0));
		Debug.Assert(!IntegerTwiddling.IsSubSignedOverflow(0, 0));
		Debug.Assert(!IntegerTwiddling.IsSubSignedOverflow(0, int.MinValue));

		Debug.Assert(IntegerTwiddling.IsSubSignedOverflow(long.MinValue, -1L));
		Debug.Assert(!IntegerTwiddling.IsSubSignedOverflow(long.MinValue, 0L));
		Debug.Assert(!IntegerTwiddling.IsSubSignedOverflow(0L, 0L));
		Debug.Assert(!IntegerTwiddling.IsSubSignedOverflow(0L, long.MinValue));
	}

	[Fact]
	public void IsSubUnsignedCarry()
	{
		Debug.Assert(IntegerTwiddling.IsSubUnsignedCarry(1U, uint.MaxValue));
		Debug.Assert(!IntegerTwiddling.IsSubUnsignedCarry(uint.MaxValue, 0U));
		Debug.Assert(!IntegerTwiddling.IsSubUnsignedCarry(0U, 0U));
		Debug.Assert(!IntegerTwiddling.IsSubUnsignedCarry(uint.MaxValue, 0U));

		Debug.Assert(IntegerTwiddling.IsSubUnsignedCarry(1UL, ulong.MaxValue));
		Debug.Assert(!IntegerTwiddling.IsSubUnsignedCarry(ulong.MaxValue, 0UL));
		Debug.Assert(!IntegerTwiddling.IsSubUnsignedCarry(0UL, 0UL));
		Debug.Assert(!IntegerTwiddling.IsSubUnsignedCarry(ulong.MaxValue, 0UL));
	}

	[Fact]
	public void IsMultiplyUnsignedCarry()
	{
		Debug.Assert(IntegerTwiddling.IsMultiplyUnsignedCarry(uint.MaxValue, 2U));
		Debug.Assert(IntegerTwiddling.IsMultiplyUnsignedCarry(2U, uint.MaxValue));
		Debug.Assert(!IntegerTwiddling.IsMultiplyUnsignedCarry(uint.MaxValue, 1U));
		Debug.Assert(!IntegerTwiddling.IsMultiplyUnsignedCarry(uint.MaxValue, 0U));
		Debug.Assert(!IntegerTwiddling.IsMultiplyUnsignedCarry(0U, 0U));
		Debug.Assert(!IntegerTwiddling.IsMultiplyUnsignedCarry(0U, uint.MaxValue));

		Debug.Assert(IntegerTwiddling.IsMultiplyUnsignedCarry(ulong.MaxValue, 2UL));
		Debug.Assert(IntegerTwiddling.IsMultiplyUnsignedCarry(2UL, ulong.MaxValue));
		Debug.Assert(!IntegerTwiddling.IsMultiplyUnsignedCarry(ulong.MaxValue, 1UL));
		Debug.Assert(!IntegerTwiddling.IsMultiplyUnsignedCarry(ulong.MaxValue, 0UL));
		Debug.Assert(!IntegerTwiddling.IsMultiplyUnsignedCarry(0UL, 0UL));
		Debug.Assert(!IntegerTwiddling.IsMultiplyUnsignedCarry(0UL, ulong.MaxValue));
	}

	[Fact]
	public void IsMultiplySignedOverflow()
	{
		Debug.Assert(IntegerTwiddling.IsMultiplySignedOverflow(int.MinValue, -1));
		Debug.Assert(IntegerTwiddling.IsMultiplySignedOverflow(int.MaxValue, 2));
		Debug.Assert(IntegerTwiddling.IsMultiplySignedOverflow(2, int.MaxValue));
		Debug.Assert(!IntegerTwiddling.IsMultiplySignedOverflow(int.MaxValue, 1));
		Debug.Assert(!IntegerTwiddling.IsMultiplySignedOverflow(int.MaxValue, 0));
		Debug.Assert(!IntegerTwiddling.IsMultiplySignedOverflow(0, 0));
		Debug.Assert(!IntegerTwiddling.IsMultiplySignedOverflow(0, int.MaxValue));

		Debug.Assert(IntegerTwiddling.IsMultiplySignedOverflow(long.MinValue, -1L));
		Debug.Assert(IntegerTwiddling.IsMultiplySignedOverflow(long.MaxValue, 2L));
		Debug.Assert(IntegerTwiddling.IsMultiplySignedOverflow(2L, long.MaxValue));
		Debug.Assert(!IntegerTwiddling.IsMultiplySignedOverflow(long.MaxValue, 1L));
		Debug.Assert(!IntegerTwiddling.IsMultiplySignedOverflow(long.MaxValue, 0L));
		Debug.Assert(!IntegerTwiddling.IsMultiplySignedOverflow(0L, 0L));
		Debug.Assert(!IntegerTwiddling.IsMultiplySignedOverflow(0L, long.MaxValue));
	}

	[Fact]
	public void HasSignBitSet()
	{
		Debug.Assert(IntegerTwiddling.HasSignBitSet(-255));
		Debug.Assert(IntegerTwiddling.HasSignBitSet(-74784778L));
		Debug.Assert(!IntegerTwiddling.HasSignBitSet(741));
		Debug.Assert(!IntegerTwiddling.HasSignBitSet(4741875L));
		Debug.Assert(!IntegerTwiddling.HasSignBitSet(0));
		Debug.Assert(!IntegerTwiddling.HasSignBitSet(0L));
	}
}
