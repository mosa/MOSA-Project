// Copyright (c) MOSA Project. Licensed under the New BSD License.

using Xunit;

namespace Mosa.Compiler.Common.xUnit;

public class DivisionTwiddlingTests
{
	[Fact]
	public static void SignedMagic_3()
	{
		var (M, s) = DivisionTwiddling.GetMagicNumber(3);
		Assert.Equal(0x55555556u, M);
		Assert.Equal(0u, s);
	}

	[Fact]
	public static void SignedMagic_7()
	{
		var (M, s) = DivisionTwiddling.GetMagicNumber(7);
		Assert.Equal(0x92492493u, M);
		Assert.Equal(2u, s);
	}

	[Fact]
	public static void UnsignedMagic_3()
	{
		var (M, s, a) = DivisionTwiddling.GetMagicNumber(3u);
		Assert.Equal(0xAAAAAAABu, M);
		Assert.Equal(1u, s);
		Assert.False(a);
	}

	[Fact]
	public static void UnsignedMagic_7()
	{
		var (M, s, a) = DivisionTwiddling.GetMagicNumber(7u);
		Assert.Equal(0x24924925u, M);
		Assert.Equal(3u, s);
		Assert.True(a);
	}

	[Fact]
	public static void UnsignedMagic_13()
	{
		var (M, s, a) = DivisionTwiddling.GetMagicNumber(13u);
		Assert.Equal(1321528399u, M);
		Assert.Equal(2u, s);
		Assert.False(a);
	}

	[Fact]
	public static void UnsignedMagic2_3()
	{
		var (M, s, a) = DivisionTwiddling.GetMagicNumber(3u);
		Assert.Equal(0xAAAAAAABu, M);
		Assert.Equal(1u, s);
		Assert.False(a);
	}

	[Fact]
	public static void UnsignedMagic2_7()
	{
		var (M, s, a) = DivisionTwiddling.GetMagicNumber(7u);
		Assert.Equal(0x24924925u, M);
		Assert.Equal(3u, s);
		Assert.True(a);
	}
}
