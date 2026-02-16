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
		Assert.Equal(BitTwiddling.GetPowerOfTwo(0b1000000000000UL), 12u);
		Assert.NotEqual(BitTwiddling.GetPowerOfTwo(0b100000000000UL), 12u);
	}

	[Fact]
	public void CountTrailingZeros()
	{
		Assert.Equal(BitTwiddling.CountTrailingZeros(ulong.MaxValue), 0);
		Assert.Equal(BitTwiddling.CountTrailingZeros(0b1111000000UL), 6);
		Assert.Equal(BitTwiddling.CountTrailingZeros(0b0UL), 64);
		Assert.NotEqual(BitTwiddling.CountTrailingZeros(0b1111UL), 4);
	}

	[Fact]
	public void CountLeadingZeros()
	{
		Assert.Equal(BitTwiddling.CountLeadingZeros(uint.MaxValue), 0);
		Assert.Equal(BitTwiddling.CountLeadingZeros(0b1111111111U), 22);
		Assert.Equal(BitTwiddling.CountLeadingZeros(0b0U), 32);
		Assert.NotEqual(BitTwiddling.CountLeadingZeros(0b1111U), 4);

		Assert.Equal(BitTwiddling.CountLeadingZeros(ulong.MaxValue), 0);
		Assert.Equal(BitTwiddling.CountLeadingZeros(0b1111111111UL), 54);
		Assert.Equal(BitTwiddling.CountLeadingZeros(0b0UL), 64);
		Assert.NotEqual(BitTwiddling.CountLeadingZeros(0b1111UL), 4);
	}

	[Fact]
	public void GetHighestSetBitPosition()
	{
		Assert.Equal(BitTwiddling.GetHighestSetBitPosition(uint.MaxValue), 32);
		Assert.Equal(BitTwiddling.GetHighestSetBitPosition(0b1111111111U), 10);
		Assert.Equal(BitTwiddling.GetHighestSetBitPosition(0b0U), 0);
		Assert.NotEqual(BitTwiddling.GetHighestSetBitPosition(0b1111U), 0);

		Assert.Equal(BitTwiddling.GetHighestSetBitPosition(ulong.MaxValue), 64);
		Assert.Equal(BitTwiddling.GetHighestSetBitPosition(0b1111111111UL), 10);
		Assert.Equal(BitTwiddling.GetHighestSetBitPosition(0b0UL), 0);
		Assert.NotEqual(BitTwiddling.GetHighestSetBitPosition(0b1111UL), 0);
	}

	[Fact]
	public void GetBitsOver()
	{
		Assert.Equal(BitTwiddling.GetBitsOver(0b11UL), 0b1111111111111111111111111111111111111111111111111111111111111100UL);
		Assert.Equal(BitTwiddling.GetBitsOver(0b1111111111UL), 0b1111111111111111111111111111111111111111111111111111110000000000UL);
		Assert.Equal(BitTwiddling.GetBitsOver(0b0UL), 0b1111111111111111111111111111111111111111111111111111111111111111UL);
		Assert.NotEqual(BitTwiddling.GetBitsOver(0b1UL), ulong.MaxValue);
	}

	[Fact]
	public void CountSetBits()
	{
		Assert.Equal(BitTwiddling.CountSetBits(uint.MaxValue), 32);
		Assert.Equal(BitTwiddling.CountSetBits(0b110U), 2);
		Assert.Equal(BitTwiddling.CountSetBits(0b1U), 1);
		Assert.NotEqual(BitTwiddling.CountSetBits(0b1111U), 5);

		Assert.Equal(BitTwiddling.CountSetBits(ulong.MaxValue), 64);
		Assert.Equal(BitTwiddling.CountSetBits(0b110UL), 2);
		Assert.Equal(BitTwiddling.CountSetBits(0b1UL), 1);
		Assert.NotEqual(BitTwiddling.CountSetBits(0b1111UL), 5);
	}
}
