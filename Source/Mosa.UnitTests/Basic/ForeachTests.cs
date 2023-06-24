
// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.UnitTests.Basic;

public static class ForeachTests
{

	[MosaUnitTest]
	public static byte ForeachU1()
	{
		var a = new byte[5];
		for (var i = 0; i < 5; i++)
			a[i] = (byte)i;

		byte total = 0;

		foreach (var v in a)
			total += v;

		return total;
	}

	[MosaUnitTest]
	public static ushort ForeachU2()
	{
		var a = new ushort[5];
		for (var i = 0; i < 5; i++)
			a[i] = (ushort)i;

		ushort total = 0;

		foreach (var v in a)
			total += v;

		return total;
	}

	[MosaUnitTest]
	public static uint ForeachU4()
	{
		var a = new uint[5];
		for (var i = 0; i < 5; i++)
			a[i] = (uint)i;

		uint total = 0;

		foreach (var v in a)
			total += v;

		return total;
	}

	[MosaUnitTest]
	public static ulong ForeachU8()
	{
		var a = new ulong[5];
		for (var i = 0; i < 5; i++)
			a[i] = (ulong)i;

		ulong total = 0;

		foreach (var v in a)
			total += v;

		return total;
	}

	[MosaUnitTest]
	public static sbyte ForeachI1()
	{
		var a = new sbyte[5];
		for (var i = 0; i < 5; i++)
			a[i] = (sbyte)i;

		sbyte total = 0;

		foreach (var v in a)
			total += v;

		return total;
	}

	[MosaUnitTest]
	public static short ForeachI2()
	{
		var a = new short[5];
		for (var i = 0; i < 5; i++)
			a[i] = (short)i;

		short total = 0;

		foreach (var v in a)
			total += v;

		return total;
	}

	[MosaUnitTest]
	public static int ForeachI4()
	{
		var a = new int[5];
		for (var i = 0; i < 5; i++)
			a[i] = (int)i;

		int total = 0;

		foreach (var v in a)
			total += v;

		return total;
	}

	[MosaUnitTest]
	public static long ForeachI8()
	{
		var a = new long[5];
		for (var i = 0; i < 5; i++)
			a[i] = (long)i;

		long total = 0;

		foreach (var v in a)
			total += v;

		return total;
	}
}

