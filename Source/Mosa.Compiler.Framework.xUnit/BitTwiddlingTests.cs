// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Mosa.Compiler.Common;
using Xunit;

namespace Mosa.Compiler.Framework.xUnit;

public class BitTwiddlingTests
{
	[Fact]
	public void IsPowerOfTwo()
	{
		Debug.Assert(BitTwiddling.IsPowerOfTwo(4096UL));
		Debug.Assert(!BitTwiddling.IsPowerOfTwo(0UL));
		Debug.Assert(!BitTwiddling.IsPowerOfTwo(67UL));
	}

	[Fact]
	public void GetPowerOfTwo()
	{
		Debug.Assert(BitTwiddling.GetPowerOfTwo(0b1000000000000UL) == 12);
		Debug.Assert(BitTwiddling.GetPowerOfTwo(0b100000000000UL) != 12);
	}

	[Fact]
	public void CountTrailingZeros()
	{
		Debug.Assert(BitTwiddling.CountTrailingZeros(ulong.MaxValue) == 0);
		Debug.Assert(BitTwiddling.CountTrailingZeros(0b1111000000UL) == 6);
		Debug.Assert(BitTwiddling.CountTrailingZeros(0b0UL) == 64);
		Debug.Assert(BitTwiddling.CountTrailingZeros(0b1111UL) != 4);
	}

	[Fact]
	public void CountLeadingZeros()
	{
		Debug.Assert(BitTwiddling.CountLeadingZeros(uint.MaxValue) == 0);
		Debug.Assert(BitTwiddling.CountLeadingZeros(0b1111111111U) == 22);
		Debug.Assert(BitTwiddling.CountLeadingZeros(0b0U) == 32);
		Debug.Assert(BitTwiddling.CountLeadingZeros(0b1111U) != 4);

		Debug.Assert(BitTwiddling.CountLeadingZeros(ulong.MaxValue) == 0);
		Debug.Assert(BitTwiddling.CountLeadingZeros(0b1111111111UL) == 54);
		Debug.Assert(BitTwiddling.CountLeadingZeros(0b0UL) == 64);
		Debug.Assert(BitTwiddling.CountLeadingZeros(0b1111UL) != 4);
	}

	[Fact]
	public void GetHighestSetBitPosition()
	{
		Debug.Assert(BitTwiddling.GetHighestSetBitPosition(uint.MaxValue) == 32);
		Debug.Assert(BitTwiddling.GetHighestSetBitPosition(0b1111111111U) == 10);
		Debug.Assert(BitTwiddling.GetHighestSetBitPosition(0b0U) == 0);
		Debug.Assert(BitTwiddling.GetHighestSetBitPosition(0b1111U) != 0);

		Debug.Assert(BitTwiddling.GetHighestSetBitPosition(ulong.MaxValue) == 64);
		Debug.Assert(BitTwiddling.GetHighestSetBitPosition(0b1111111111UL) == 10);
		Debug.Assert(BitTwiddling.GetHighestSetBitPosition(0b0UL) == 0);
		Debug.Assert(BitTwiddling.GetHighestSetBitPosition(0b1111UL) != 0);
	}

	[Fact]
	public void GetBitsOver()
	{
		Debug.Assert(BitTwiddling.GetBitsOver(0b11UL) == 0b1111111111111111111111111111111111111111111111111111111111111100UL);
		Debug.Assert(BitTwiddling.GetBitsOver(0b1111111111UL) == 0b1111111111111111111111111111111111111111111111111111110000000000UL);
		Debug.Assert(BitTwiddling.GetBitsOver(0b0UL) == 0b1111111111111111111111111111111111111111111111111111111111111111UL);
		Debug.Assert(BitTwiddling.GetBitsOver(0b1UL) != ulong.MaxValue);
	}

	[Fact]
	public void CountSetBits()
	{
		Debug.Assert(BitTwiddling.CountSetBits(uint.MaxValue) == 32);
		Debug.Assert(BitTwiddling.CountSetBits(0b110U) == 2);
		Debug.Assert(BitTwiddling.CountSetBits(0b1U) == 1);
		Debug.Assert(BitTwiddling.CountSetBits(0b1111U) != 5);

		Debug.Assert(BitTwiddling.CountSetBits(ulong.MaxValue) == 64);
		Debug.Assert(BitTwiddling.CountSetBits(0b110UL) == 2);
		Debug.Assert(BitTwiddling.CountSetBits(0b1UL) == 1);
		Debug.Assert(BitTwiddling.CountSetBits(0b1111UL) != 5);
	}
}
