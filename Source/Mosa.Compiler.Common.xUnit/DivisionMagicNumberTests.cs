﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Diagnostics;
using Xunit;

namespace Mosa.Compiler.Common.xUnit;

public class DivisionMagicNumberTests
{
	[Fact]
	public static void SignedMagic_3()
	{
		var result = DivisionMagicNumber.GetMagicNumber(3);
		Debug.Assert(result.M == 0x55555556);
		Debug.Assert(result.s == 0);
	}

	[Fact]
	public static void SignedMagic_7()
	{
		var result = DivisionMagicNumber.GetMagicNumber(7);
		Debug.Assert(result.M == 0x92492493);
		Debug.Assert(result.s == 2);
	}

	[Fact]
	public static void UnsignedMagic_3()
	{
		var result = DivisionMagicNumber.GetMagicNumber(3u);
		Debug.Assert(result.M == 0xAAAAAAAB);
		Debug.Assert(result.s == 1);
		Debug.Assert(!result.a);
	}

	[Fact]
	public static void UnsignedMagic_7()
	{
		var result = DivisionMagicNumber.GetMagicNumber(7u);
		Debug.Assert(result.M == 0x24924925);
		Debug.Assert(result.s == 3);
		Debug.Assert(result.a);
	}

	//[Fact]
	//public static void UnsignedMagic_11()
	//{
	//	var result = DivisionMagicNumber.GetMagicNumber(13u);
	//	Debug.Assert(result.M + result.a + result.s == ((uint)-1171354717) + 0 + 3);
	//}

	[Fact]
	public static void UnsignedMagic_13()
	{
		var result = DivisionMagicNumber.GetMagicNumber(13u);
		Debug.Assert(result.M == 1321528399);
		Debug.Assert(result.s == 2);
		Debug.Assert(!result.a);
	}

	[Fact]
	public static void UnsignedMagic2_3()
	{
		var result = DivisionMagicNumber.GetMagicNumber(3u);
		Debug.Assert(result.M == 0xAAAAAAAB);
		Debug.Assert(result.s == 1);
		Debug.Assert(!result.a);
	}

	[Fact]
	public static void UnsignedMagic2_7()
	{
		var result = DivisionMagicNumber.GetMagicNumber(7u);
		Debug.Assert(result.M == 0x24924925);
		Debug.Assert(result.s == 3);
		Debug.Assert(result.a);
	}
}
