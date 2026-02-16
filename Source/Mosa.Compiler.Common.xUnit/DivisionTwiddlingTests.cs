// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.Compiler.Common.xUnit;

public class DivisionTwiddlingTests
{
	[Fact]
	public static void SignedMagic_3()
	{
		var (M, s) = DivisionTwiddling.GetMagicNumber(3);
		Assert.Equal(M, 0x55555556u);
		Assert.Equal(s, 0u);
	}

	[Fact]
	public static void SignedMagic_7()
	{
		var (M, s) = DivisionTwiddling.GetMagicNumber(7);
		Assert.Equal(M, 0x92492493u);
		Assert.Equal(s, 2u);
	}

	[Fact]
	public static void UnsignedMagic_3()
	{
		var (M, s, a) = DivisionTwiddling.GetMagicNumber(3u);
		Assert.Equal(M, 0xAAAAAAABu);
		Assert.Equal(s, 1u);
		Assert.False(a);
	}

	[Fact]
	public static void UnsignedMagic_7()
	{
		var (M, s, a) = DivisionTwiddling.GetMagicNumber(7u);
		Assert.Equal(M, 0x24924925u);
		Assert.Equal(s, 3u);
		Assert.True(a);
	}

	[Fact]
	public static void UnsignedMagic_13()
	{
		var (M, s, a) = DivisionTwiddling.GetMagicNumber(13u);
		Assert.Equal(M, 1321528399u);
		Assert.Equal(s, 2u);
		Assert.False(a);
	}

	[Fact]
	public static void UnsignedMagic2_3()
	{
		var (M, s, a) = DivisionTwiddling.GetMagicNumber(3u);
		Assert.Equal(M, 0xAAAAAAABu);
		Assert.Equal(s, 1u);
		Assert.False(a);
	}

	[Fact]
	public static void UnsignedMagic2_7()
	{
		var (M, s, a) = DivisionTwiddling.GetMagicNumber(7u);
		Assert.Equal(M, 0x24924925u);
		Assert.Equal(s, 3u);
		Assert.True(a);
	}
}
