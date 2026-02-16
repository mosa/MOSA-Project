// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Mosa.Compiler.Common;
using Xunit;

namespace Mosa.Compiler.Framework.xUnit;

public class BitTwiddlingTests
{
	[Fact]
	public void IsPowerOfTwo()
	{
		Assert.True(BitTwiddling.IsPowerOfTwo(4096UL));
		Assert.False(BitTwiddling.IsPowerOfTwo(0UL));
		Assert.False(BitTwiddling.IsPowerOfTwo(67UL));
	}

	[Fact]
	public void GetPowerOfTwo()
	{
		Assert.Equal(12u, BitTwiddling.GetPowerOfTwo(0b1000000000000UL));
		Assert.NotEqual(12u, BitTwiddling.GetPowerOfTwo(0b100000000000UL));
	}

	[Fact]
	public void CountTrailingZeros()
	{
		Assert.Equal(0, BitTwiddling.CountTrailingZeros(ulong.MaxValue));
		Assert.Equal(6, BitTwiddling.CountTrailingZeros(0b1111000000UL));
		Assert.Equal(64, BitTwiddling.CountTrailingZeros(0b0UL));
		Assert.NotEqual(4, BitTwiddling.CountTrailingZeros(0b1111UL));
	}

	[Fact]
	public void CountLeadingZeros()
	{
		Assert.Equal(0, BitTwiddling.CountLeadingZeros(uint.MaxValue));
		Assert.Equal(22, BitTwiddling.CountLeadingZeros(0b1111111111U));
		Assert.Equal(32, BitTwiddling.CountLeadingZeros(0b0U));
		Assert.NotEqual(4, BitTwiddling.CountLeadingZeros(0b1111U));

		Assert.Equal(0, BitTwiddling.CountLeadingZeros(ulong.MaxValue));
		Assert.Equal(54, BitTwiddling.CountLeadingZeros(0b1111111111UL));
		Assert.Equal(64, BitTwiddling.CountLeadingZeros(0b0UL));
		Assert.NotEqual(4, BitTwiddling.CountLeadingZeros(0b1111UL));
	}

	[Fact]
	public void GetHighestSetBitPosition()
	{
		Assert.Equal(32, BitTwiddling.GetHighestSetBitPosition(uint.MaxValue));
		Assert.Equal(10, BitTwiddling.GetHighestSetBitPosition(0b1111111111U));
		Assert.Equal(0, BitTwiddling.GetHighestSetBitPosition(0b0U));
		Assert.NotEqual(0, BitTwiddling.GetHighestSetBitPosition(0b1111U));

		Assert.Equal(64, BitTwiddling.GetHighestSetBitPosition(ulong.MaxValue));
		Assert.Equal(10, BitTwiddling.GetHighestSetBitPosition(0b1111111111UL));
		Assert.Equal(0, BitTwiddling.GetHighestSetBitPosition(0b0UL));
		Assert.NotEqual(0, BitTwiddling.GetHighestSetBitPosition(0b1111UL));
	}

	[Fact]
	public void GetBitsOver()
	{
		Assert.Equal(0b1111111111111111111111111111111111111111111111111111111111111100UL, BitTwiddling.GetBitsOver(0b11UL));
		Assert.Equal(0b1111111111111111111111111111111111111111111111111111110000000000UL, BitTwiddling.GetBitsOver(0b1111111111UL));
		Assert.Equal(0b1111111111111111111111111111111111111111111111111111111111111111UL, BitTwiddling.GetBitsOver(0b0UL));
		Assert.NotEqual(ulong.MaxValue, BitTwiddling.GetBitsOver(0b1UL));
	}

	[Fact]
	public void CountSetBits()
	{
		Assert.Equal(32, BitTwiddling.CountSetBits(uint.MaxValue));
		Assert.Equal(2, BitTwiddling.CountSetBits(0b110U));
		Assert.Equal(1, BitTwiddling.CountSetBits(0b1U));
		Assert.NotEqual(5, BitTwiddling.CountSetBits(0b1111U));

		Assert.Equal(64, BitTwiddling.CountSetBits(ulong.MaxValue));
		Assert.Equal(2, BitTwiddling.CountSetBits(0b110UL));
		Assert.Equal(1, BitTwiddling.CountSetBits(0b1UL));
		Assert.NotEqual(5, BitTwiddling.CountSetBits(0b1111UL));
	}
}
